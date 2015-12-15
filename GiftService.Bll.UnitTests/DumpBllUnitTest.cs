using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GiftService.Bll.UnitTests
{
    [TestClass]
    public class DumpBllUnitTest
    {
        [TestMethod]
        public void Test_Dump_ProductBdo()
        {
            string s = DumpBll.Dump(new Models.ProductBdo());
            Assert.AreEqual("-", s);
        }
    }
}
