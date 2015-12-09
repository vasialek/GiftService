using AutoMapper;
using GiftService.Dal;
using GiftService.Models;
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

        public TransactionsBll(ITransactionDal transactionDal)
        {
            if (transactionDal == null)
            {
                throw new ArgumentNullException("transactionDal");
            }

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

            t.PosUserUid = posUserUid;
            t.PosId = product.PosId;
            t.CreatedAt = DateTime.UtcNow;

            t.ProductUid = product.ProductUid;
            t.IsPaymentProcessed = false;


            t.PaySystemUid = product.PaySystemUid;

            _transactionDal.StartTransaction(t);

            return t;
        }

    }
}
