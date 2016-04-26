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

        //IPdfBll _pdfBll = null;
        IPdfBll _pdfSharpBll = null;
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
                    FooterImage = "footer.jpg",
                    HeaderImage = "header.jpg"
                });

            _productsDalMock.Setup(x => x.GetProductByUid(It.IsAny<string>()))
                .Returns((string productUid) =>
                {
                    var p = ProductsDalFake.GetProducts().First(x => x.ProductUid == productUid);
                    return p;
                });

            _productsDalMock.Setup(x => x.GetProductByPaySystemUid(It.IsAny<string>()))
                .Returns((string paySystemUid) =>
                {
                    var p = ProductsDalFake.GetProducts().First(x => x.PaySystemUid == paySystemUid);
                    p.PaySystemUid = paySystemUid;
                    return p;
                });

            _productsBll = new ProductsBll(_productsDalMock.Object, DalFactory.Current.PosDal);

            //_pdfBll = new PdfBll(_configBllMock.Object, _productsDalMock.Object);
            _pdfSharpBll = new PdfShartBll(_configBllMock.Object, _productsBll);

            _communicationBll = new CommunicationBll();
        }

        [TestMethod]
        public void Test_Pdf_Coupon_RitosMasazai_1005()
        {
            int posId = 1005;
            var p = ProductsDalFake.GetProducts().First(x => x.PosId == posId);
            byte[] ba = _pdfSharpBll.GeneratProductPdf(p.ProductUid);

            Assert.IsNotNull(ba);
            File.WriteAllBytes("c:\\temp\\gs_" + posId + ".pdf", ba);
        }

        [TestMethod]
        public void Test_Pdf_Coupon_Knygynai_1006()
        {
            int posId = 1006;
            var p = ProductsDalFake.GetProducts().First(x => x.PosId == posId);

            byte[] ba = _pdfSharpBll.GeneratProductPdf(p.ProductUid);

            Assert.IsNotNull(ba);
            File.WriteAllBytes("c:\\temp\\gs_" + posId + ".pdf", ba);
        }

        [TestMethod]
        public void Test_Pdf_Coupon_Melisanda_1007()
        {
            int posId = 1007;
            var p = ProductsDalFake.GetProducts().First(x => x.PosId == posId);

            byte[] ba = _pdfSharpBll.GeneratProductPdf(p.ProductUid);

            Assert.IsNotNull(ba);
            File.WriteAllBytes("c:\\temp\\gs_" + posId + ".pdf", ba);
        }

        [TestMethod]
        public void Test_Send_Email_To_Client_Success()
        {
            var p = _productsDalMock.Object.GetProductByUid(Guid.NewGuid().ToString("N"));

            _communicationBll.SendEmailToClientOnSuccess(p);
        }
    }
}
