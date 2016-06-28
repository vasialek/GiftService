using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GiftService.Models;

namespace GiftService.Dal.Fakes
{
    public class FakeTransactionsDal : ITransactionDal
    {
        private static List<TransactionBdo> _transactions = null;

        public FakeTransactionsDal()
        {
            if (_transactions == null)
            {
                _transactions = new List<TransactionBdo>();

                _transactions.Add(new TransactionBdo
                {
                    Id = 10001,
                    PaySystemUid = "2b90a96612704938a0f73107d87d4fed",
                    PosUserUid = "2b90a96612704938a0f73107d87d4fed",
                    ProductUid = "2b90a96612704938a0f73107d87d4fed",
                    OrderNr = "X66-00000001",

                    ProductId = 1001,
                    PosId = 1005,
                    
                    RequestedAmount = 10.50m,
                    PaidAmount = 10.50m,
                    RequestedCurrencyCode = "EUR",
                    PaidCurrencyCode = "EUR",

                    IsPaymentProcessed = true,
                    IsTestPayment = true,

                    PaidThrough = "ps",

                    PayerName = "Aleksej",
                    PayerEmail = "av@fscc.lt",
                    PayerLastName = "Tak",

                    PaymentStatus = PaymentStatusIds.PaidOk,
                    PaySystemResponseAt = DateTime.Now.AddHours(-5),
                    PaymentSystem = PaymentSystems.Paysera
                });

                _transactions.Add(new TransactionBdo
                {
                    Id = 10002,
                    PaySystemUid = "5d26299e25b444d3a7dc5b9f3aa0a8ef",
                    PosUserUid = "5d26299e25b444d3a7dc5b9f3aa0a8ef",
                    ProductUid = "5d26299e25b444d3a7dc5b9f3aa0a8ef",
                    OrderNr = "X66-00000002",

                    ProductId = 1001,
                    PosId = 1005,

                    RequestedAmount = 10.50m,
                    PaidAmount = 0,
                    RequestedCurrencyCode = "EUR",
                    PaidCurrencyCode = "EUR",

                    IsPaymentProcessed = false,
                    IsTestPayment = true,

                    PaidThrough = "",

                    PayerName = "",
                    PayerEmail = "",
                    PayerLastName = "",

                    PaymentStatus = PaymentStatusIds.WaitingForPayment,
                    PaySystemResponseAt = DateTime.MinValue,
                    PaymentSystem = PaymentSystems.None
                });
            }
        }

        public IEnumerable<TransactionBdo> GetLastTransactions(int posId, int offset, int limit)
        {
            return _transactions.Where(x => x.PosId == posId);
        }

        public TransactionBdo GetTransactionByOrderNr(string orderNr)
        {
            throw new NotImplementedException();
        }

        public TransactionBdo GetTransactionByPaySystemUid(string paySystemUid)
        {
            return _transactions.First(x => x.PaySystemUid == paySystemUid);
        }

        public void StartTransaction(TransactionBdo t)
        {
            throw new NotImplementedException();
        }

        public void Update(TransactionBdo t)
        {
            throw new NotImplementedException();
        }
    }
}
