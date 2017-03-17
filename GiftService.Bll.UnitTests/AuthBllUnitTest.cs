using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GiftService.Models.Exceptions;
using System;

namespace GiftService.Bll.UnitTests
{
    [TestClass]
    public class AuthBllUnitTest
    {
        private IAuthBll _bll = new AuthBll(BllFactory.Current.ValidationBll);

        [TestMethod]
        public void WebLogin_ValidationException_On_Empty_Email()
        {
            bool was = false;
            try
            {
                var user = _bll.WebLogin("", "123");
            }
            catch (ValidationListException vlex)
            {
                was = vlex.Errors.Count(x => x.ErrCode == ValidationErrors.Empty) == 1;
            }

            Assert.IsTrue(was);
        }

        [TestMethod]
        public void WebLogin_Exception_On_Email_Wo_Tld()
        {
            bool was = false;
            try
            {
                var user = _bll.WebLogin("root@localhost", "123");
            }
            catch (ValidationListException vlex)
            {
                was = vlex.Errors.Count(x => x.ErrCode == ValidationErrors.RuleViolation) == 1;
            }

            Assert.IsTrue(was);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WebLogin_NoException_On_Empty_Password()
        {
            var user = _bll.WebLogin("fasfsda@fsadfdsfsafdsf.fsf", "");
        }
    }
}
