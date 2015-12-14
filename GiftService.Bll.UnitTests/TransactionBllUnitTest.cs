using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GiftService.Models;
using Moq;
using GiftService.Dal;
using GiftService.Models.Payments;
using GiftService.Models.Exceptions;

namespace GiftService.Bll.UnitTests
{
    [TestClass]
    public class TransactionBllUnitTest
    {
        private Mock<IConfigurationBll> _configurationBllMock = null;
        private Mock<ITransactionDal> _transactionDal = null;
        private ITransactionsBll _bll = null;
        private ProductBdo _product = null;

        [TestInitialize]
        public void Init()
        {
            _configurationBllMock = new Mock<IConfigurationBll>();
            _configurationBllMock.Setup(x => x.Get())
                .Returns(() => new MySettings
                {
                    UseTestPayment = true
                });

            _transactionDal = new Mock<ITransactionDal>();
            _bll = new TransactionsBll(_configurationBllMock.Object, BllFactory.Current.SecurityBll, _transactionDal.Object);

            _product = new ProductBdo();
            _product.PosId = 1005;
        }

        [TestMethod]
        public void Test_Finish_Transaction_On_Bad_Status()
        {
            var resp = new PayseraPaymentResponse();
            resp.OrderId = Guid.NewGuid().ToString("N");
            _transactionDal.Setup(x => x.GetTransactionByPaySystemUid(It.IsAny<string>()))
                .Returns((string paySysUid) => new TransactionBdo
                {
                    PaySystemUid = paySysUid,
                    PaymentStatus = PaymentStatusIds.WaitingForPayment
                });
            resp.Status = "0";

            var t = _bll.FinishTransaction(resp);

            Assert.AreNotEqual(PaymentStatusIds.PaidOk, t.PaymentStatus);
        }

        [TestMethod]
        public void Test_FinishTransaction_Success()
        {
            _transactionDal.Setup(x => x.GetTransactionByPaySystemUid(It.IsAny<string>()))
                .Returns((string paySysUid) => new TransactionBdo
                {
                    PaySystemUid = paySysUid,
                    PaymentStatus = PaymentStatusIds.WaitingForPayment
                });
            var resp = new PayseraPaymentResponse();
            resp.OrderId = Guid.NewGuid().ToString("N");
            resp.Status = "1";
            resp.PayAmount = 543.21m;
            resp.PayCurrencyCode = "USD";
            resp.Payment = "nordealt";

            var t = _bll.FinishTransaction(resp);

            Assert.AreEqual(PaymentStatusIds.PaidOk, t.PaymentStatus);
            Assert.AreEqual(resp.PayAmount, t.PaidAmount);
            Assert.AreEqual(resp.PayCurrencyCode, t.PaidCurrencyCode);
            Assert.AreEqual(resp.Payment, t.PaidThrough);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_User_Cancels_Transaction_Empty_PaySystemUid()
        {
            // OrderId from Paysera is same as PaySystemUid
            var resp = new PayseraPaymentResponse();
            resp.OrderId = null;

            _bll.CancelTransactionByUser(resp.OrderId);
        }

        [TestMethod]
        [ExpectedException(typeof(TransactionDoesNotExist))]
        public void Test_User_Cancels_NonExisting_Transaction()
        {
            // OrderId from Paysera is same as PaySystemUid
            var resp = new PayseraPaymentResponse();
            resp.OrderId = Guid.NewGuid().ToString("N");

            _bll.CancelTransactionByUser(resp.OrderId);
        }

        [TestMethod]
        [ExpectedException(typeof(TransactionStatusException))]
        public void Test_User_Cancels_Processed_Transaction()
        {
            string paySystemUid = Guid.NewGuid().ToString("N");

            _transactionDal.Setup(x => x.GetTransactionByPaySystemUid(It.IsAny<string>()))
                .Returns((string paySysUid) => 
                {
                    return new TransactionBdo
                    {
                        PaySystemUid = paySysUid,
                        PaymentStatus = PaymentStatusIds.PaidOk
                    };
                });

            _bll.CancelTransactionByUser(paySystemUid);
        }

        [TestMethod]
        public void Test_User_Cancels_Transaction_Success()
        {
            string paySystemUid = Guid.NewGuid().ToString("N");

            _transactionDal.Setup(x => x.GetTransactionByPaySystemUid(It.IsAny<string>()))
                .Returns((string paySysUid) =>
                {
                    return new TransactionBdo
                    {
                        PaySystemUid = paySysUid,
                        PaymentStatus = PaymentStatusIds.WaitingForPayment
                    };
                });

            var t = _bll.CancelTransactionByUser(paySystemUid);

            Assert.AreEqual(PaymentStatusIds.UserCancelled, t.PaymentStatus);
            Assert.IsTrue(((TimeSpan)(DateTime.UtcNow - t.PaySystemResponseAt)).TotalMilliseconds < 200);

        }

        [TestMethod]
        public void Test_StartTransaction()
        {
            string posUserUid = Guid.NewGuid().ToString("N");
            _product.ProductUid = Guid.NewGuid().ToString("N");

            var t = _bll.StartTransaction(posUserUid, _product);

            Assert.AreEqual(posUserUid, t.PosUserUid);
            Assert.AreEqual(_product.PosId, t.PosId);
            //Assert.IsFalse(String.IsNullOrEmpty(t.PaySystemUid));
            Assert.AreEqual(_product.ProductUid, t.ProductUid);
            Assert.AreEqual(DateTime.MinValue, t.PaySystemResponseAt);
        }

        [TestMethod]
        public void Test_StartTransaction_Times_Are_Utc()
        {
            string posUserUid = Guid.NewGuid().ToString("N");

            var t = _bll.StartTransaction(posUserUid, _product);

            TimeSpan ts = DateTime.UtcNow - t.CreatedAt;
            Assert.IsTrue(ts.TotalMilliseconds < 100);
        }

        [TestMethod]
        public void Test_StartTransaction_Payment_Is_Not_Processed()
        {
            string posUserUid = Guid.NewGuid().ToString("N");

            var t = _bll.StartTransaction(posUserUid, _product);

            Assert.IsFalse(t.IsPaymentProcessed);
        }
    }
}
