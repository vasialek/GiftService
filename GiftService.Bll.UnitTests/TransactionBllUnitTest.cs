using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GiftService.Models;
using Moq;
using GiftService.Dal;

namespace GiftService.Bll.UnitTests
{
    [TestClass]
    public class TransactionBllUnitTest
    {
        private ITransactionsBll _bll = null;
        private ProductBdo _product = null;

        [TestInitialize]
        public void Init()
        {
            Mock<ITransactionDal> transactionDal = new Mock<ITransactionDal>();
            _bll = new TransactionsBll(transactionDal.Object);

            _product = new ProductBdo();
            _product.PosId = 1005;
        }

        [TestMethod]
        public void Test_StartTransaction()
        {
            string posUserUid = Guid.NewGuid().ToString("N");

            var t = _bll.StartTransaction(posUserUid, _product);

            Assert.AreEqual(posUserUid, t.PosUserUid);
            Assert.AreEqual(_product.PosId, t.PosId);
            Assert.IsFalse(String.IsNullOrEmpty(t.PaySystemUid));
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
