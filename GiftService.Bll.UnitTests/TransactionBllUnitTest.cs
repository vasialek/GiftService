using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GiftService.Bll.UnitTests
{
    [TestClass]
    public class TransactionBllUnitTest
    {
        private ITransactionsBll _bll = new TransactionsBll();

        [TestMethod]
        public void Test_StartTransaction()
        {
            string posUserUid = Guid.NewGuid().ToString("N").Substring(0, 16);
            int posId = 1005;

            var t = _bll.StartTransaction(posUserUid, posId);

            Assert.AreEqual(posUserUid, t.PosUserUid);
            Assert.AreEqual(posId, t.PosId);
            Assert.IsFalse(String.IsNullOrEmpty(t.PaySystemUid));
            Assert.AreEqual(DateTime.MinValue, t.PaySystemResponseAt);
        }

        [TestMethod]
        public void Test_StartTransaction_Times_Are_Utc()
        {
            string posUserUid = Guid.NewGuid().ToString("N").Substring(0, 16);
            int posId = 1005;

            var t = _bll.StartTransaction(posUserUid, posId);

            TimeSpan ts = DateTime.UtcNow - t.CreatedAt;
            Assert.IsTrue(ts.TotalMilliseconds < 100);
        }

        [TestMethod]
        public void Test_StartTransaction_Payment_Is_Not_Processed()
        {
            string posUserUid = Guid.NewGuid().ToString("N").Substring(0, 16);
            int posId = 1005;

            var t = _bll.StartTransaction(posUserUid, posId);

            Assert.IsFalse(t.IsPaymentProcessed);
        }
    }
}
