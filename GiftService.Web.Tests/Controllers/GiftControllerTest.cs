using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GiftService.Web.Controllers;
using System.Web.Mvc;

namespace GiftService.Web.Tests.Controllers
{
    [TestClass]
    public class GiftControllerTest
    {
        [TestMethod]
        public void Test_Print_Redirect_To_404_On_Bad_Uid()
        {
            GiftController c = new GiftController();

            var r = c.Print("fasdfsdfsdjfklah");

            Assert.IsTrue(r is RedirectResult);
        }
    }
}
