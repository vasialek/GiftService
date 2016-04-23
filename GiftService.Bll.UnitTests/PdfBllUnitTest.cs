using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Moq;
using GiftService.Models;
using GiftService.Dal;
using GiftService.Models.Pos;
using System.Linq;
using System.Collections.Generic;
using GiftService.Bll.UnitTests.Fakes;

namespace GiftService.Bll.UnitTests
{
    [TestClass]
    public class PdfBllUnitTest
    {

        IPdfBll _pdfBll = null;
        ICommunicationBll _communicationBll = null;
        IProductsBll _productsBll = null;
        Mock<IProductsDal> _productsDalMock = new Mock<IProductsDal>();
        Mock<IPosDal> _posDalMock = new Mock<IPosDal>();
        Mock<IConfigurationBll> _configBllMock = null;

        IEnumerable<ProductBdo> _products = null;

        [TestInitialize]
        public void Init()
        {
            _configBllMock = new Mock<IConfigurationBll>();
            _configBllMock.Setup(x => x.Get())
                .Returns(() => new MySettings {
                    LengthOfPosUid = 32,
                    LengthOfPdfDirectoryName = 5,
                    PathToPdfStorage = "c:\\temp\\giftservice\\trash\\",
                    PathToPosContent = "c:\\_projects\\GiftService\\GiftService.Web\\Content\\"
                });
            _configBllMock.Setup(x => x.GetPdfLayout(It.IsAny<int>()))
                .Returns((int posId) => new PosPdfLayout
                {
                    PosId = posId,
                    FooterImage = null,
                    HeaderImage = null
                });

            _productsDalMock.Setup(x => x.GetProductByUid(It.IsAny<string>()))
                .Returns((string productUid) =>
                {
                    var p = ProductsDalFake.GetProducts().First();
                    p.ProductUid = productUid;
                    return p;
                });

            _productsDalMock.Setup(x => x.GetProductByPaySystemUid(It.IsAny<string>()))
                .Returns((string paySystemUid) =>
                {
                    var p = ProductsDalFake.GetProducts().First();
                    p.PaySystemUid = paySystemUid;
                    return p;
                });

            _pdfBll = new PdfBll(_configBllMock.Object, _productsDalMock.Object);

            _communicationBll = new CommunicationBll();

            _productsBll = new ProductsBll(_productsDalMock.Object, DalFactory.Current.PosDal);
        }

        [TestMethod]
        public void Test_PdfSharp_Library()
        {
            var configBll = new Mock<IConfigurationBll>();

            configBll.Setup(x => x.Get())
                .Returns(() => new MySettings
                {
                    LengthOfPosUid = 32,
                    LengthOfPdfDirectoryName = 5,
                    PathToPdfStorage = "c:\\temp\\giftservice\\trash\\",
                    PathToPosContent = "c:\\_projects\\GiftService\\GiftService.Web\\Content\\"
                });

            configBll.Setup(x => x.GetPdfLayout(It.IsAny<int>()))
                .Returns((int posId) => new PosPdfLayout
                {
                    PosId = posId,
                    FooterImage = "footer.jpg",
                    HeaderImage = "header_72dpi.jpg"
                });

            IPdfBll pdfSharpBll = new PdfShartBll(configBll.Object, _productsBll);
            byte[] ba = pdfSharpBll.GeneratProductPdf("PUID_000010000000000000000000000");

            Assert.IsNotNull(ba);
            File.WriteAllBytes("c:\\temp\\gs.pdf", ba);
        }

        [TestMethod]
        public void Test_Create_Coupon()
        {
            byte[] ba = _pdfBll.GeneratProductPdf("PUID_000010000000000000000000000");

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
