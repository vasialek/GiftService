using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GiftService.Models.JsonModels;
using Newtonsoft.Json;

namespace GiftService.Bll.UnitTests
{
    [TestClass]
    public class CommunicationBllUnitTest
    {
        private ICommunicationBll _communicationBll = new CommunicationBll();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_Exception_On_Null_Url()
        {
            _communicationBll.GetJsonResponse<BaseResponse>(null);
        }

        [TestMethod]
        public void Test_Parse_Valid_Json()
        {
            BaseResponse r = new BaseResponse() { Status = true, Message = "Some good mesagges", Errors = new string[0] };
            string json = JsonConvert.SerializeObject(r);

            var resp = _communicationBll.ParseJson<BaseResponse>(json);

            Assert.AreEqual(r.Status, resp.Status);
            Assert.AreEqual(r.Message, resp.Message);
            Assert.AreEqual(r.Errors.Length, resp.Errors.Length);
        }
    }
}
