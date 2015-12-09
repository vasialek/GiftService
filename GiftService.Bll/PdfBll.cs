using GiftService.Dal;
using GiftService.Models.Pos;
using iTextSharp.text;
using iTextSharp.text.pdf;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Bll
{
    public interface IPdfBll
    {
        byte[] GetProductPdf(string productUid);
        byte[] GeneratProductPdf(string productUid);
    }

    public class PdfBll : IPdfBll
    {
        private IConfigurationBll _configurationBll = null;
        private IProductsDal _productsDal = null;

        private ILog _logger = null;
        private ILog Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = LogManager.GetLogger(GetType());
                    log4net.Config.XmlConfigurator.Configure();
                }
                return _logger;
            }
        }

        public PdfBll(IConfigurationBll configurationBll, IProductsDal productsDal)
        {
            if (configurationBll == null)
            {
                throw new ArgumentNullException("configurationBll");
            }
            if (productsDal == null)
            {
                throw new ArgumentNullException("productsDal");
            }

            _configurationBll = configurationBll;
            _productsDal = productsDal;
        }

        public byte[] GetProductPdf(string productUid)
        {
            var p = _productsDal.GetProductByUid(productUid);
            string pathToPdf = Path.Combine(_configurationBll.Get().PathToPdfStorage, "test.pdf");
            return File.ReadAllBytes(pathToPdf);
        }

        public byte[] GeneratProductPdf(string productUid)
        {
            Logger.InfoFormat("Generation product coupon by product UID: `{0}`", productUid);
            var p = _productsDal.GetProductByUid(productUid);
            var layout = _configurationBll.GetPdfLayout(p.PosId);
            Logger.DebugFormat("  decorate PDF coupon with header: `{0}`", layout.HeaderImage);
            Logger.DebugFormat("  decorate PDF coupon with footer: `{0}`", layout.FooterImage);

            //EnsurePdfDirectoryExists(productUid);

            using (var ms = new MemoryStream())
            {

                // PDF images are in ~/content/p<POS_ID>/pdf/

                // Default is A4 document is 595 points wide and 842 pixels high with a 36 point margin all around by default.
                var d = new Document(PageSize.A4);
                PdfWriter w = PdfWriter.GetInstance(d, ms);

                d.Open();
                d.SetMargins(0, 0, 36, 36);
                DecoratePdf(d, layout);
                //d.Add(new Paragraph("Ritos masazai"));

                //string posUrl = "www.ritosmasazai.lt";
                //string customerName = "Some customer";
                //string informationToRegister = "8 652 98422";
                //string posLocation = "Juozapavičiaus g. 9A - 174, Vilnius";
                //string posName = "BABOR GROŽIO CENTRAS";
                string couponBarcode = "RM123456";

                Paragraph pg;

                //Font ordinalF = new Font(Font.FontFamily.HELVETICA, 12f, Font.NORMAL, BaseColor.BLACK);
                Font ordinalF = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1257, 18, Font.NORMAL);
                Font ordinalBoldF = new Font(Font.FontFamily.HELVETICA, 11f, Font.BOLD, BaseColor.BLACK);
                Font redF = new Font(Font.FontFamily.HELVETICA, 11f, Font.NORMAL, BaseColor.RED);
                Font smallF = new Font(Font.FontFamily.HELVETICA, 9f, Font.NORMAL, BaseColor.BLACK);
                Font smallRedF = new Font(Font.FontFamily.HELVETICA, 9f, Font.NORMAL, BaseColor.RED);

                pg = new Paragraph("Jūs įsigijote kuponą", ordinalF);
                pg.Alignment = Element.ALIGN_CENTER;
                d.Add(pg);
                pg = new Paragraph(p.PosUrl, smallF);
                pg.Alignment = Element.ALIGN_CENTER;
                d.Add(pg);

                d.Add(new Paragraph(""));

                pg = new Paragraph(p.CustomerName, ordinalBoldF);
                pg.Alignment = Element.ALIGN_CENTER;
                d.Add(pg);

                d.Add(new Paragraph(""));

                pg = new Paragraph(p.ProductName, ordinalF);
                pg.Alignment = Element.ALIGN_CENTER;
                d.Add(pg);
                pg = new Paragraph(String.Concat(p.ProductPrice.ToString("### ##0.00"), " ", p.CurrencyCode));
                pg.Alignment = Element.ALIGN_CENTER;
                d.Add(pg);

                pg = new Paragraph("Privalote užsiregistruoti:", smallF);
                pg.Alignment = Element.ALIGN_CENTER;
                d.Add(pg);
                pg = new Paragraph(p.PhoneForReservation, redF);
                pg.Alignment = Element.ALIGN_CENTER;
                d.Add(pg);

                d.Add(new Paragraph(""));

                pg = new Paragraph("Aptarnavimo vieta:", ordinalF);
                pg.Alignment = Element.ALIGN_CENTER;
                d.Add(pg);
                pg = new Paragraph(p.PosName, ordinalF);
                pg.Alignment = Element.ALIGN_CENTER;
                d.Add(pg);
                pg = new Paragraph(String.Concat(p.PosAddress, ", ", p.PosCity), ordinalF);
                pg.Alignment = Element.ALIGN_CENTER;
                d.Add(pg);

                d.Add(new Paragraph(""));

                pg = new Paragraph("Kuponas galioja ", smallRedF);
                pg.Alignment = Element.ALIGN_CENTER;
                d.Add(pg);
                pg = new Paragraph(p.ValidTill.ToShortDateString(), smallRedF);
                //pg = new Paragraph(String.Concat(p.ValidFrom.ToShortDateString(), " - ", p.ValidTill.ToShortDateString()), smallRedF);
                pg.Alignment = Element.ALIGN_CENTER;
                d.Add(pg);

                d.Close();

                w.Flush();

                return ms.ToArray();
            }




            //string pdfDirName = _configurationBll.GetDirectoryNameByUid(productUid);
            ////string path = Path.Combine(_configurationBll.Get().PathToPdfStorage, pdfDirName, String.Concat(productUid, ".pdf"));
            //string path = Path.Combine(_configurationBll.Get().PathToPdfStorage, String.Concat(productUid, ".pdf"));
            //Logger.Info("Going to save PDF in: " + path);
            //PdfWriter.GetInstance(d, new FileStream(path, FileMode.Create));


            //return File.ReadAllBytes(path);
        }

        protected Document DecoratePdf(Document d, PosPdfLayout layout)
        {
            var settings = _configurationBll.Get();
            string image = null;
            string posPdfDirectory = Path.Combine(settings.PathToPosContent, String.Concat("p", layout.PosId), "pdf");
            Logger.DebugFormat("  looking for PDF decorations in `{0}`", posPdfDirectory);


            if (String.IsNullOrEmpty(layout.HeaderImage) == false)
            {
                image = Path.Combine(posPdfDirectory, layout.HeaderImage);
                Logger.DebugFormat("  adding header image: `{0}`", image);
                Image header = Image.GetInstance(image);
                //header.SetAbsolutePosition(0, 0);
                d.Add(header);
            }

            if (String.IsNullOrEmpty(layout.FooterImage) == false)
            {
                image = Path.Combine(posPdfDirectory, layout.FooterImage);
                Logger.DebugFormat("  adding footer image: `{0}`", image);
                Image footer = Image.GetInstance(image);
                footer.SetAbsolutePosition(0, 0);
                d.Add(footer);
            }

            return d;
        }

        protected void EnsurePdfDirectoryExists(string productUid)
        {
            var settings = _configurationBll.Get();
            if (Directory.Exists(settings.PathToPdfStorage) == false)
            {
                Logger.InfoFormat("PDF storage does not exist, creating it `{0}`", settings.PathToPdfStorage);
                Directory.CreateDirectory(settings.PathToPdfStorage);
            }

            //string pdfDirName = _configurationBll.GetDirectoryNameByUid(productUid);
            //string path = Path.Combine(settings.PathToPdfStorage, pdfDirName);
            //if (Directory.Exists(path) == false)
            //{
            //    Logger.InfoFormat("PDF coupon directory does not exits, creating it `{0}`", path);
            //    Directory.CreateDirectory(path);
            //}
        }
    }
}
