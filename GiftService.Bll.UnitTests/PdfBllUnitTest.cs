using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GiftService.Bll.UnitTests
{
    [TestClass]
    public class PdfBllUnitTest
    {

        [TestMethod]
        public void Test_Create_Coupon()
        {
            BllFactory.Current.PdfBll.GeneratProductPdf("PUID_000010000000000000000000000");
        }
    }
}
