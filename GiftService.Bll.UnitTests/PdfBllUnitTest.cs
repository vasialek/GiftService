using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Moq;
using GiftService.Models;
using GiftService.Dal;
using GiftService.Models.Pos;

namespace GiftService.Bll.UnitTests
{
    [TestClass]
    public class PdfBllUnitTest
    {

        IPdfBll _bll = null;
        ICommunicationBll _communicationBll = null;
        Mock<IProductsDal> _productsDalMock = new Mock<IProductsDal>();

        [TestInitialize]
        public void Init()
        {
            Mock<IConfigurationBll> configBllMock = new Mock<IConfigurationBll>();
            configBllMock.Setup(x => x.Get())
                .Returns(() => new MySettings {
                    LengthOfPosUid = 32,
                    LengthOfPdfDirectoryName = 5,
                    PathToPdfStorage = "c:\\temp\\giftservice\\trash\\",
                    PathToPosContent = "c:\\_projects\\GiftService\\GiftService.Web\\Content\\"
                });
            configBllMock.Setup(x => x.GetPdfLayout(It.IsAny<int>()))
                .Returns((int posId) => new PosPdfLayout
                {
                    PosId = posId,
                    FooterImage = null,
                    HeaderImage = null
                });

            _productsDalMock.Setup(x => x.GetProductByUid(It.IsAny<string>()))
                .Returns((string productUid) => new ProductBdo
                {
                    ProductUid = productUid,
                    PaySystemUid = Guid.NewGuid().ToString("N"),
                    PosUserUid = Guid.NewGuid().ToString("N"),

                    PosId = 1005,
                    PosName = "BABOR GROŽIO CENTRAS",
                    PosCity = "Wilno ųųųųų",
                    PosAddress = "Juozapavičiaus g. 9A - 174",
                    PosUrl = "www.url.com",

                    CustomerName = "Aleksej Vvvv",
                    CustomerEmail = "proglamer@gmail.com",
                    CustomerPhone = "+370 600 14789",
                    Remarks = "Some remarks...",

                    ProductName = "Grožiodeivė",
                    ProductDescription = "Atstatantis viso kūno įvyniojimas su šveitimu išsausėjusiai kūno odai",
                    ProductPrice = 12345.67m,
                    CurrencyCode = "EUR",

                    ValidFrom = DateTime.UtcNow,
                    ValidTill = DateTime.UtcNow.AddMonths(3),

                    PhoneForReservation = "+370 652 98422"

                });

            _bll = new PdfBll(configBllMock.Object, _productsDalMock.Object);

            _communicationBll = new CommunicationBll();
        }

        [TestMethod]
        public void Test_Create_Coupon()
        {
            byte[] ba = _bll.GeneratProductPdf("PUID_000010000000000000000000000");

            Assert.IsNotNull(ba);
            File.WriteAllBytes("c:\\temp\\gs.pdf", ba);
        }

        [TestMethod]
        public void Test_Send_Email_To_Client_Success()
        {
            var p = _productsDalMock.Object.GetProductByUid(Guid.NewGuid().ToString("N"));

            _communicationBll.SendEmailToClientOnSuccess(p);
        }
    }
}
