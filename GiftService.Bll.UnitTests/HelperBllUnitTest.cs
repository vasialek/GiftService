using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GiftService.Bll.UnitTests
{
    [TestClass]
    public class HelperBllUnitTest
    {
        private IHelperBll _bll = new HelperBll();

        [TestMethod]
        public void Test_Convert_Zero_As_Unix_Begin()
        {
            uint tm = 0;

            DateTime d = _bll.ConvertFromUnixTimestamp(tm);

            Assert.AreEqual(_bll.UnixStart, d);
        }

        [TestMethod]
        public void Test_Get_Current_Timestamp()
        {
            uint tm = _bll.GetUnixTimestamp();
            DateTime dt = _bll.ConvertFromUnixTimestamp(tm);

            Assert.IsTrue((DateTime.UtcNow - dt).TotalSeconds < 1);
        }
    }
}
