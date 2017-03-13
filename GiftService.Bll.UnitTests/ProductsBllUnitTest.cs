using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GiftService.Models.JsonModels;
using Newtonsoft.Json;
using Moq;
using GiftService.Dal;
using GiftService.Models;
using GiftService.Models.Products;
using System.Collections.Generic;
using System.Linq;

namespace GiftService.Bll.UnitTests
{
    [TestClass]
    public class ProductsBllUnitTest
    {

        private IProductsBll _productsBll = null;
        private PaymentRequestValidationResponse _posResponse = new PaymentRequestValidationResponse();

        [TestInitialize]
        public void Init()
        {
            Mock<IProductsDal> productsDal = new Mock<IProductsDal>();
            Mock<IPosDal> posDal = new Mock<IPosDal>();

            productsDal
                .Setup(x => x.SaveProductInformationFromPos(It.IsAny<ProductBdo>(), It.IsAny<PosBdo>()))
                .Returns((ProductBdo product, PosBdo pos) => product);

            _productsBll = new ProductsBll(BllFactory.Current.GiftValidationBll, BllFactory.Current.SecurityBll, productsDal.Object, posDal.Object);

            _posResponse.PosId = 1005;
            _posResponse.ProductName = "Gilus prisilietimas";
            _posResponse.ProductDuration = "56 min";
            _posResponse.ProductDescription = "No descriptions";

            _posResponse.Locations = new List<ProductServiceLocation>();
            _posResponse.Locations.Add(new ProductServiceLocation { Id = 1, Name = "LocName", City = "Wilno", Address = "Juozapavi\u010diaus g. 9A - 174" });
        }

        [TestMethod]
        public void Test_SaveProductInformationFromPos_Map_Response()
        {
            DateTime validTill = DateTime.UtcNow;
            string phoneReservation = "8 600 54321";
            string emailReservation = "info@dovanukuponai.com";

            ProductServiceLocation location = new ProductServiceLocation();
            location.Id = 1;
            location.Name = "LocName";
            location.City = "Wilno";
            location.Address = "Juozapavi\u010diaus g. 9A - 174";
            
            uint productValidTillTm = new HelperBll().GetUnixTimestamp();
            var jsonResponse = "{\"Status\":\"true\",\"Message\":\"\",\"PosId\":1005,\"ProductName\":\"Gilus prisilietimas\",\"ProductDuration\":\"40 min\",\"ProductDescription\":\"NO\",\"RequestedAmountMinor\":\"3500\",\"CurrencyCode\":\"EUR\",\"ProductValidTillTm\":" + productValidTillTm + ",\"PosName\":\"\",\"PosUrl\":\"\",\"PosCity\":\"\",\"PosAddress\":\"\",\"PhoneForReservation\":\"" + phoneReservation + "\",\"EmailForReservation\":\"" + emailReservation + "\",\"Locations\":[{\"Id\":\"" + location.Id + "\",\"Name\":\"" + location.Name + "\",\"City\":\"" + location.City + "\",\"Address\":\"" + location.Address + "\"},{\"Id\":\"2\",\"Name\":\"SIGMOS SPORTO KLUBAS\",\"City\":\"Vilnius\",\"Address\":\"Kalvarij\u0173 g.131  Luk\u0161io g. 2\"}]}";
            var posResponse = JsonConvert.DeserializeObject<PaymentRequestValidationResponse>(jsonResponse);

            var co = new ProductCheckoutModel();
            co.PaymentSystem = PaymentSystems.Paysera;
            co.LocationId = location.Id;
            co.CurrencyCode = "EUR";
            co.CustomerEmail = "av@asdf.lt";
            co.CustomerName = "Aleksej Tak";
            co.CustomerPhone = "+370 600 12345";
            co.Remarks = "Suck it!";

            var product = _productsBll.SaveProductInformationFromPos("12345678901234567890123456789012", posResponse, co);

            Assert.AreEqual("Gilus prisilietimas", product.ProductName);
            Assert.AreEqual("NO", product.ProductDescription);
            Assert.AreEqual(35m, product.ProductPrice);
            Assert.AreEqual("EUR", product.CurrencyCode);
            Assert.AreEqual(1005, product.PosId);
            Assert.IsTrue((validTill - product.ValidTill).TotalSeconds < 1);

            Assert.AreEqual(co.CustomerName, product.CustomerName);
            Assert.AreEqual(co.CustomerEmail, product.CustomerEmail);
            Assert.AreEqual(co.CustomerPhone, product.CustomerPhone);
            Assert.AreEqual(co.Remarks, product.Remarks);

            Assert.AreEqual(PaymentSystems.Paysera, product.PaymentSystem);

            Assert.IsFalse(String.IsNullOrEmpty(product.ProductUid));

            Assert.AreEqual(location.Name, product.PosName);
            Assert.AreEqual(location.City, product.PosCity);
            Assert.AreEqual(location.Address, product.PosAddress);

            Assert.AreEqual(product.PhoneForReservation, phoneReservation);
            Assert.AreEqual(product.EmailForReservation, emailReservation);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Test_Incorrect_LocationId_From_Checkout()
        {
            var co = new ProductCheckoutModel();
            co.LocationId = _posResponse.Locations.Max(x => x.Id) + 10;

            _productsBll.SaveProductInformationFromPos("12345678901234567890123456789012", _posResponse, co);
        }
    }
}
