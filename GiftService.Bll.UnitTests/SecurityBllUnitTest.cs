using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using GiftService.Models.JsonModels;
using GiftService.Models;
using GiftService.Models.Exceptions;

namespace GiftService.Bll.UnitTests
{
    [TestClass]
    public class SecurityBllUnitTest
    {
        private PosBdo _pos = new PosBdo();
        private Mock<ICommunicationBll> _communicationBllMock = new Mock<ICommunicationBll>();
        private Mock<IConfigurationBll> _configBllMock = null;
        private ISecurityBll _securityBll = null;

        [TestInitialize]
        public void Init()
        {
            _pos = new PosBdo();
            _pos.Id = 1005;
            _pos.Name = "Ritos Masazai";
            _pos.PosUrl = new Uri("http://www.ritosmasazai.lt/");
            _pos.ValidateUrl = new Uri("http://localhost:56620/Test/Validate");

            _communicationBllMock.Setup(x => x.GetJsonResponse<BaseResponse>(It.IsAny<Uri>())).Returns(new BaseResponse { Status = true, Message = "Faked OK message" });

            _configBllMock = new Mock<IConfigurationBll>();
            _configBllMock.Setup(x => x.Get())
                .Returns(() => new MySettings
                {
                    LengthOfPosUid = 32
                });

            _securityBll = new SecurityBll(_configBllMock.Object, _communicationBllMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_Uid_Is_Not_Empty()
        {
            string uid = "";

            _securityBll.ValidateUid(uid);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_Uid_Has_Bad_Length()
        {
            string uid = "1".PadRight(_configBllMock.Object.Get().LengthOfPosUid + 1);

            _securityBll.ValidateUid(uid);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_Deny_On_Empty_ValidateUrl()
        {
            _pos.ValidateUrl = null;

            _securityBll.ValidatePosPaymentRequest(_pos, "");
        }

        [TestMethod]
        [ExpectedException(typeof(BadResponseException))]
        public void Test_Deny_On_Incorrect_Response_From_Pos()
        {
            _communicationBllMock.Setup(x => x.GetJsonResponse<BaseResponse>(It.IsAny<Uri>())).Returns<BaseResponse>(null);

            _securityBll.ValidatePosPaymentRequest(_pos, "");
        }

        [TestMethod]
        public void Test_Validate_On_Positive_Response_From_Pos()
        {
            _communicationBllMock.Setup(x => x.GetJsonResponse<PaymentRequestValidationResponse>(It.IsAny<Uri>()))
                .Returns(new PaymentRequestValidationResponse()
                {
                    RequestedAmountMinor = 1,
                    ProductName = "Product name",
                    CurrencyCode = "EUR"
                });
            var resp = _securityBll.ValidatePosPaymentRequest(_pos, "");

            Assert.IsNotNull(resp);
        }

        [TestMethod]
        [ExpectedException(typeof(IncorrectPaymentParamersException))]
        public void Test_Deny_Negative_Payment_RequestedAmount()
        {
            _communicationBllMock.Setup(x => x.GetJsonResponse<PaymentRequestValidationResponse>(It.IsAny<Uri>())).Returns(new PaymentRequestValidationResponse());

            var resp = _securityBll.ValidatePosPaymentRequest(_pos, "");
        }

        [TestMethod]
        [ExpectedException(typeof(IncorrectPaymentParamersException))]
        public void Test_Deny_Empty_Payment_ProductName()
        {
            _communicationBllMock.Setup(x => x.GetJsonResponse<PaymentRequestValidationResponse>(It.IsAny<Uri>()))
                .Returns(new PaymentRequestValidationResponse()
                {
                    RequestedAmountMinor = 1,
                    CurrencyCode = "EUR"
                });

            var resp = _securityBll.ValidatePosPaymentRequest(_pos, "");
        }

        [TestMethod]
        [ExpectedException(typeof(IncorrectPaymentParamersException))]
        public void Test_Deny_Empty_Payment_CurrencyCode()
        {
            _communicationBllMock.Setup(x => x.GetJsonResponse<PaymentRequestValidationResponse>(It.IsAny<Uri>()))
                .Returns(new PaymentRequestValidationResponse()
                {
                    RequestedAmountMinor = 1,
                    ProductName = "Product name"
                });

            var resp = _securityBll.ValidatePosPaymentRequest(_pos, "");
        }

        [TestMethod]
        public void Test_Payment_CurrencyCode_Is_Ok()
        {
            _securityBll.ValidateCurrencyCode("EUR", new PosBdo());
        }
    }
}
