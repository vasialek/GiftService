using AutoMapper;
using GiftService.Dal;
using GiftService.Models;
using GiftService.Models.Exceptions;
using GiftService.Models.Payments;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Bll
{
    public interface ITransactionsBll
    {
        TransactionBdo GetTransactionByPaySystemUid(string paySystemUid);
        TransactionBdo StartTransaction(string posUserUid, ProductBdo product);
        TransactionBdo FinishTransaction(PayseraPaymentResponse resp);
        TransactionBdo CancelTransactionByUser(string paySystemUid);
        TransactionBdo CancelTransactionByUserUsingOrderNr(string paymentOrderNr);
        IEnumerable<TransactionBdo> GetLastTransactions(int posId, int offset, int limit);
    }

    public class TransactionsBll : ITransactionsBll
    {
        private ILog _logger = null;
        private ILog Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = LogManager.GetLogger(GetType());
                    log4net.Config.XmlConfigurator.Configure();
                }
                return _logger;
            }
        }

        protected ITransactionDal _transactionDal = null;
        protected ISecurityBll _securityBll = null;
        protected IConfigurationBll _configurationBll = null;

        public TransactionsBll(IConfigurationBll configurationBll, ISecurityBll securityBll, ITransactionDal transactionDal)
        {
            if (configurationBll == null)
            {
                throw new ArgumentNullException("configurationBll");
            }
            if (securityBll == null)
            {
                throw new ArgumentNullException("securityBll");
            }
            if (transactionDal == null)
            {
                throw new ArgumentNullException("transactionDal");
            }

            _configurationBll = configurationBll;
            _securityBll = securityBll;
            _transactionDal = transactionDal;
        }

        public TransactionBdo GetTransactionByPaySystemUid(string paySystemUid)
        {
            Logger.InfoFormat("Searching for payment transaction by payment system UID: `{0}`", paySystemUid);
            return _transactionDal.GetTransactionByPaySystemUid(paySystemUid);
        }

        public TransactionBdo StartTransaction(string posUserUid, ProductBdo product)
        {
            var t = new TransactionBdo();

            t.IsTestPayment = _configurationBll.Get().UseTestPayment;
            t.PosUserUid = posUserUid;
            t.PaySystemUid = product.PaySystemUid;

            t.PaymentStatus = PaymentStatusIds.NotProcessed;
            t.IsPaymentProcessed = false;

            t.PosId = product.PosId;
            t.CreatedAt = DateTime.UtcNow;

            t.ProductUid = product.ProductUid;
            t.RequestedAmount = product.ProductPrice;
            t.RequestedCurrencyCode = product.CurrencyCode;

            // TODO: add project ID
            t.ProjectId = 0;

            t.OrderNr = BllFactory.Current.GiftsBll.GetUniqueOrderId(t.PosId);
            Logger.DebugFormat("  generate unique order nr for new transaction: `{0}`", t.OrderNr);

            _transactionDal.StartTransaction(t);

            return t;
        }

        public TransactionBdo CancelTransactionByUserUsingOrderNr(string paymentOrderNr)
        {
            Logger.InfoFormat("User is canceling transaction by payment system order nr: `{0}", paymentOrderNr);
            _securityBll.ValidateOrderId(paymentOrderNr);

            var t = _transactionDal.GetTransactionByOrderNr(paymentOrderNr);
            if (t == null)
            {
                throw new TransactionDoesNotExist("Transaction was not found by payment order nr", paymentOrderNr);
            }

            SetTransactionStatusToCancelled(t);

            return t;
        }

        public TransactionBdo CancelTransactionByUser(string paySystemUid)
        {
            Logger.InfoFormat("User is canceling transaction by payment system UID: `{0}", paySystemUid);
            _securityBll.ValidateUid(paySystemUid);

            var t = _transactionDal.GetTransactionByPaySystemUid(paySystemUid);
            if (t == null)
            {
                throw new TransactionDoesNotExist("Transaction was not found by payment system UID", paySystemUid);
            }

            SetTransactionStatusToCancelled(t);

            return t;
        }

        private void SetTransactionStatusToCancelled(TransactionBdo t)
        {
            // Allow to cancel not processed and waiting for payment
            if (t.PaymentStatus != PaymentStatusIds.NotProcessed && t.PaymentStatus != PaymentStatusIds.WaitingForPayment)
            {
                throw new TransactionStatusException("User could not cancel transaction with status: " + t.PaymentStatus + " (" + (int)t.PaymentStatus + ")");
            }

            t.PaymentStatus = PaymentStatusIds.UserCancelled;
            t.PaySystemResponseAt = DateTime.UtcNow;

            Logger.InfoFormat("  updating status of transaction #{0} to {1}", t.Id, t.PaymentStatus);
            _transactionDal.Update(t);
        }

        public TransactionBdo FinishTransaction(PayseraPaymentResponse resp)
        {
            if (resp == null)
            {
                throw new ArgumentNullException("Could not finish transaction for NULL response from payment system");
            }

            // Paysera OrderId == PaySystemUid
            _securityBll.ValidateOrderId(resp.OrderId);

            var t = _transactionDal.GetTransactionByOrderNr(resp.OrderId);

            if (t.PaymentStatus == PaymentStatusIds.PaidOk)
            {
                Logger.Warn("Transaction status is already PaidOk, do not update and exit");
                return t;
            }

            t.IsPaymentProcessed = true;
            t.PaySystemResponseAt = DateTime.UtcNow;

            t.PaidAmount = resp.PayAmount;
            t.PaidCurrencyCode = resp.PayCurrencyCode;

            t.PayerEmail = resp.CustomerEmail;
            t.PayerLastName = resp.CustomerLastName;
            t.PayerName = resp.CustomerName;
            t.PayerPhone = resp.CustomerPhone;

            t.PaidThrough = resp.Payment;

            /**
            Payment status:
                0 - payment has no been executed
                1 - payment successful
                2 - payment order accepted, but not yet executed
                3 - additional payment information
            */
            switch (resp.Status)
            {
                case "1":
                    t.PaymentStatus = PaymentStatusIds.PaidOk;
                    break;
                case "2":
                    t.PaymentStatus = PaymentStatusIds.AcceptedButNotExecuted;
                    break;
                default:
                    Logger.Warn("Payment status (from Paysera) is not OK: " + resp.Status);
                    break;
            }

            t.ResponseFromPaymentSystem = t.ResponseFromPaymentSystem;

            Logger.Info("Updating transaction after finishing");
            _transactionDal.Update(t);

            return t;
        }

        public IEnumerable<TransactionBdo> GetLastTransactions(int posId, int offset, int limit)
        {
            if (offset < 0)
            {
                offset = 0;
            }
            if (limit < 0)
            {
                limit = 50;
            }

            return _transactionDal.GetLastTransactions(posId, offset, limit);
        }
    }
}
