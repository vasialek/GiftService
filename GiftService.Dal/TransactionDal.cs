using AutoMapper;
using GiftService.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Dal
{

    public interface ITransactionDal
    {
        void StartTransaction(TransactionBdo t);
        TransactionBdo GetTransactionByPaySystemUid(string paySystemUid);
        TransactionBdo GetTransactionByOrderNr(string orderNr);
        void Update(TransactionBdo t);
        IEnumerable<TransactionBdo> GetLastTransactions(int posId, int offset, int limit);
    }

    public class TransactionDal : ITransactionDal
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

        public IEnumerable<TransactionBdo> GetLastTransactions(int posId, int offset, int limit)
        {
            Logger.InfoFormat("Getting last transactions for POS ID: #{0}", posId);
            Logger.DebugFormat("  setting offset to:    {0}", offset);
            Logger.DebugFormat("  setting limit to:     {0}", limit);

            var list = new List<TransactionBdo>();

            try
            {
                // TODO: validate offset and limit
                using (var db = new GiftServiceEntities())
                {
                    IQueryable<transaction> ts = db.transactions.OrderByDescending(x => x.id);
                    if (posId > 0)
                    {
                        ts = ts.Where(x => x.pos_id == posId);
                    }
                    ts.Skip(offset).Take(limit);

                    int projectId = 0;
                    PaymentStatusIds paymentStatus = PaymentStatusIds.NotProcessed;
                    PaymentSystems paymentSystem = PaymentSystems.None;

                    foreach (var t in ts)
                    {
                        int.TryParse(t.project_id, out projectId);
                        Enum.TryParse(t.pay_system_id.ToString(), out paymentSystem);
                        Enum.TryParse(t.payment_status_id.ToString(), out paymentStatus);

                        list.Add(new TransactionBdo
                        {
                            Id = t.id,
                            IsPaymentProcessed = t.is_payment_processed,
                            IsTestPayment = t.is_test_payment,

                            RequestedAmount = t.requested_amount,
                            RequestedCurrencyCode = t.requested_currency_code,
                            PaidAmount = t.paid_amount,
                            PaidCurrencyCode = t.paid_currency_code,
                            PaidThrough = t.paid_through,

                            PayerName = t.p_name,
                            PayerLastName = t.p_lastname,
                            PayerEmail = t.p_email,
                            PayerPhone = t.p_phone,
                            Remarks = t.remarks,

                            ProjectId = projectId,
                            PosId = t.pos_id,
                            PosUserUid = t.pos_user_uid,
                            PaySystemUid = t.pay_system_uid,
                            OrderNr = t.order_nr,
                            ProductId = t.product_id,
                            ProductUid = t.product_uid,

                            PaymentStatus = paymentStatus,
                            PaymentSystem = paymentSystem,

                            CreatedAt = t.created_at,
                            PaySystemResponseAt = t.pay_system_response_at.HasValue ? t.pay_system_response_at.Value : DateTime.MinValue
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Erro getting last transactions", ex);
                throw;
            }

            return list;
        }

        public TransactionBdo GetTransactionByOrderNr(string orderNr)
        {
            transaction t = null;
            using (var db = new GiftServiceEntities())
            {
                t = db.transactions.First(x => x.order_nr.Equals(orderNr));
            }

            return MapTo(t);
        }

        public TransactionBdo GetTransactionByPaySystemUid(string paySystemUid)
        {
            transaction t = null;
            using (var db = new GiftServiceEntities())
            {
                t = db.transactions.First(x => x.pay_system_uid.Equals(paySystemUid));
            }

            return MapTo(t);
        }

        private TransactionBdo MapTo(transaction t)
        {
            TransactionBdo transaction = null;
            if (t != null)
            {
                PaymentStatusIds paymentStatus = PaymentStatusIds.NotProcessed;
                PaymentSystems paymentSystem = PaymentSystems.None;
                int projectId = 0;

                int.TryParse(t.project_id, out projectId);
                Enum.TryParse(t.pay_system_id.ToString(), out paymentSystem);
                Enum.TryParse(t.payment_status_id.ToString(), out paymentStatus);

                transaction = new TransactionBdo
                {
                    Id = t.id,
                    IsPaymentProcessed = t.is_payment_processed,
                    IsTestPayment = t.is_test_payment,

                    RequestedAmount = t.requested_amount,
                    RequestedCurrencyCode = t.requested_currency_code,
                    PaidAmount = t.paid_amount,
                    PaidCurrencyCode = t.paid_currency_code,
                    PaidThrough = t.paid_through,

                    PayerName = t.p_name,
                    PayerLastName = t.p_lastname,
                    PayerEmail = t.p_email,
                    PayerPhone = t.p_phone,
                    Remarks = t.remarks,

                    ProjectId = projectId,
                    PosId = t.pos_id,
                    PosUserUid = t.pos_user_uid,
                    PaySystemUid = t.pay_system_uid,
                    ProductId = t.product_id,
                    ProductUid = t.product_uid,

                    PaymentStatus = paymentStatus,
                    PaymentSystem = paymentSystem,

                    CreatedAt = t.created_at,
                    PaySystemResponseAt = t.pay_system_response_at.HasValue ? t.pay_system_response_at.Value : DateTime.MinValue
                };
            }

            return transaction;
        }

        public void StartTransaction(TransactionBdo t)
        {
            //Mapper.CreateMap<TransactionBdo, transaction>()
            //    .ForMember(dest => dest.payment_status_id,
            //        orig => orig.MapFrom(x => (int)x.PaymentStatus));
            //AutoMapperConfigDal.SetMappingTypeFromBdoToDao();
            //var transaction = Mapper.Map<transaction>(t);
            try
            {
                var transaction = new transaction();
                transaction.is_test_payment = t.IsTestPayment;
                transaction.payment_status_id = (int)t.PaymentStatus;
                transaction.is_payment_processed = t.IsPaymentProcessed;
                transaction.pos_user_uid = t.PosUserUid;
                transaction.pay_system_uid = t.PaySystemUid;
                transaction.order_nr = t.OrderNr;
                transaction.requested_amount = t.RequestedAmount;
                transaction.requested_currency_code = t.RequestedCurrencyCode;
                transaction.paid_amount = t.PaidAmount;
                transaction.paid_currency_code = t.PaidCurrencyCode;
                transaction.paid_through = t.PaidThrough;
                transaction.p_name = t.PayerName;
                transaction.p_lastname = t.PayerLastName;
                transaction.p_email = t.PayerEmail;
                transaction.p_phone = t.PayerPhone;
                transaction.remarks = t.Remarks;
                transaction.pos_id = t.PosId;
                transaction.product_id = t.ProductId;
                transaction.product_uid = t.ProductUid;
                transaction.project_id = t.ProjectId.ToString();
                transaction.created_at = DateTime.UtcNow;

                Logger.Info("Saving transaction in DB:");
                Logger.DebugFormat("  setting is_test_payment:              `{0}`", transaction.is_test_payment);
                Logger.DebugFormat("  setting payment_status_id:            `{0}`", transaction.payment_status_id);
                Logger.DebugFormat("  setting is_payment_processed:         `{0}`", transaction.is_payment_processed);
                Logger.DebugFormat("  setting pos_user_uid:                 `{0}`", transaction.pos_user_uid);
                Logger.DebugFormat("  setting pay_system_uid:               `{0}`", transaction.pay_system_uid);
                Logger.DebugFormat("  setting order_nr:                     `{0}`", transaction.order_nr);

                Logger.DebugFormat("  setting requested_amount:             `{0}`", transaction.requested_amount);
                Logger.DebugFormat("  setting requested_currency_code:      `{0}`", transaction.requested_currency_code);
                Logger.DebugFormat("  setting paid_amount:                  `{0}`", transaction.paid_amount);
                Logger.DebugFormat("  setting paid_currency_code:           `{0}`", transaction.paid_currency_code);
                Logger.DebugFormat("  setting paid_through:                 `{0}`", transaction.paid_through);

                Logger.DebugFormat("  setting p_name:                       `{0}`", transaction.p_name);
                Logger.DebugFormat("  setting p_lastname:                   `{0}`", transaction.p_lastname);
                Logger.DebugFormat("  setting p_email:                      `{0}`", transaction.p_email);
                Logger.DebugFormat("  setting p_phone:                      `{0}`", transaction.p_phone);
                Logger.DebugFormat("  setting remarks:                      `{0}`", transaction.remarks);

                Logger.DebugFormat("  setting pos_id:                       `{0}`", transaction.pos_id);
                Logger.DebugFormat("  setting product_id:                   `{0}`", transaction.product_id);
                Logger.DebugFormat("  setting product_uid:                  `{0}`", transaction.product_uid);

                Logger.DebugFormat("  setting response_from_payment:        `{0}`", transaction.response_from_payment);
                Logger.DebugFormat("  setting pay_system_response_at:       `{0}`", transaction.pay_system_response_at);
                Logger.DebugFormat("  setting created_at:                   `{0}`", transaction.created_at);

                using (var db = new GiftServiceEntities())
                {
                    db.transactions.Add(transaction);
                    db.SaveChanges();
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbvex)
            {
                Logger.Error("Validation error saving transaction", dbvex);
                foreach (var e in dbvex.EntityValidationErrors)
                {
                    foreach (var sub in e.ValidationErrors)
                    {
                        Logger.ErrorFormat("  {0,-16}: {1}", sub.PropertyName, sub.ErrorMessage);
                    }
                }
                throw;
            }
        }

        public void Update(TransactionBdo transactionBdo)
        {
            try
            {
                using (var db = new GiftServiceEntities())
                {
                    var t = db.transactions.First(x => x.id == transactionBdo.Id);

                    t.is_payment_processed = transactionBdo.IsPaymentProcessed;
                    t.is_test_payment = transactionBdo.IsTestPayment;
                    t.paid_amount = transactionBdo.PaidAmount;
                    t.paid_currency_code = transactionBdo.PaidCurrencyCode;
                    t.paid_through = transactionBdo.PaidThrough;
                    t.payment_status_id = (int)transactionBdo.PaymentStatus;
                    t.pay_system_id = (int)transactionBdo.PaymentSystem;
                    t.pay_system_response_at = transactionBdo.PaySystemResponseAt;
                    t.pay_system_uid = transactionBdo.PaySystemUid;
                    t.pos_id = transactionBdo.PosId;
                    t.pos_user_uid = transactionBdo.PosUserUid;
                    t.product_id = transactionBdo.ProductId;
                    t.product_uid = transactionBdo.ProductUid;
                    t.project_id = transactionBdo.ProjectId.ToString();
                    t.p_email = transactionBdo.PayerEmail;
                    t.p_lastname = transactionBdo.PayerLastName;
                    t.p_name = transactionBdo.PayerName;
                    t.p_phone = transactionBdo.PayerPhone;
                    t.remarks = transactionBdo.Remarks;
                    t.requested_amount = transactionBdo.RequestedAmount;
                    t.requested_currency_code = transactionBdo.RequestedCurrencyCode;
                    t.response_from_payment = transactionBdo.ResponseFromPaymentSystem;

                    db.SaveChanges();
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbvex)
            {
                Logger.Error("Validation error updating transaction", dbvex);
                foreach (var e in dbvex.EntityValidationErrors)
                {
                    foreach (var sub in e.ValidationErrors)
                    {
                        Logger.ErrorFormat("  {0,-16}: {1}", sub.PropertyName, sub.ErrorMessage);
                    }
                }
                throw;
            }
        }
    }
}
