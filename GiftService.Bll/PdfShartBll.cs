using GiftService.Models;
using GiftService.Models.Pos;
using log4net;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiftService.Bll
{
    public class PdfShartBll : IPdfBll
    {
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

        //private IPosBll _posBll = null;
        private IConfigurationBll _configurationBll = null;
        private IProductsBll _productsBll = null;

        public PdfShartBll(IConfigurationBll configurationBll, IProductsBll productBll)
        {
            if (configurationBll == null)
            {
                throw new ArgumentNullException("configurationBll");
            }
            if (productBll == null)
            {
                throw new ArgumentNullException("productBll");
            }

            _productsBll = productBll;
            _configurationBll = configurationBll;
        }

        public byte[] GeneratProductPdf(string productUid)
        {
            Logger.InfoFormat("Generation product coupon by product UID: `{0}`", productUid);
            var pi = _productsBll.GetProductInformationByUid(productUid);
            var p = pi.Product;
            var config = _configurationBll.Get();
            var layout = _configurationBll.GetPdfLayout(p.PosId);

            using (var ms = new MemoryStream())
            {
                // Create a new MigraDoc document
                var doc = new Document();
                doc.Info.Title = String.Concat(config.ProjectName, " - ", p.ProductName);
                doc.Info.Author = "Aleksej Vasinov";

                DefineStyles(doc);

                CreatePage(doc, layout);

                FillContent(doc, p);

                PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always);
                pdfRenderer.Document = doc;
                pdfRenderer.RenderDocument();

                pdfRenderer.PdfDocument.Save(ms);

                return ms.ToArray();
            }
        }

        private void FillContent(Document doc, ProductBdo product)
        {
            var p = doc.LastSection.AddParagraph();
            p.Format.SpaceBefore = "8cm";
            p.Format.SpaceAfter = "1cm";
            p.Format.Font.Size = 14;
            p.Format.Font.Bold = true;
            p.Format.Alignment = ParagraphAlignment.Center;
            p.AddText(product.ProductName);

            var t = doc.LastSection.AddTable();
            t.BottomPadding = "0.5cm";
            t.Format.Alignment = ParagraphAlignment.Center;
            t.Borders.DistanceFromLeft = "2cm";

            // Before you can add a row, you must define the columns
            Column column = t.AddColumn("10cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            column = t.AddColumn("7cm");
            column.Format.Alignment = ParagraphAlignment.Left;

            Row r = t.AddRow();
            r.Cells[0].AddParagraph("Pirkejas:")
                .Format.Font.Bold = true;
            r.Cells[0].AddParagraph(product.CustomerName);

            r.Cells[1].AddParagraph("Kaina:")
                .Format.Font.Bold = true;
            r.Cells[1].AddParagraph(String.Concat(product.ProductPrice.ToString("### ##0.00"), " ", product.CurrencyCode));


            r = t.AddRow();
            r.Cells[0].AddParagraph("Privalote užsiregistruoti:")
                .Format.Font.Bold = true;
            r.Cells[0].AddParagraph(product.PhoneForReservation);

            r.Cells[1].AddParagraph("Trukme:")
                .Format.Font.Bold = true;
            //r.Cells[1].AddParagraph(product.)

            r = t.AddRow();
            r.Cells[0].AddParagraph("Aptarnavimo vieta:")
                .Format.Font.Bold = true;
            r.Cells[0].AddParagraph(product.PosName);
            //r.Cells[0].AddParagraph(String.Concat(product.PosAddress, ", ", product.PosCity));
            r.Cells[0].AddParagraph(product.PosAddress);
            r.Cells[0].AddParagraph(product.PosCity);

            r.Cells[1].AddParagraph("Kuponas galioja:")
                .Format.Font.Bold = true;
            r.Cells[1].AddParagraph(product.ValidTill.ToShortDateString())
                .Format.Font.Color = Colors.Red;

            var desription = doc.LastSection.AddParagraph();
            desription.Format.SpaceBefore = "1cm";
            desription.Format.Alignment = ParagraphAlignment.Justify;
            desription.Format.Font.Size = 10;
            desription.AddText(product.ProductDescription);

            var orderId = doc.LastSection.AddParagraph();
            orderId.Format.SpaceBefore = "1cm";
            orderId.Format.Font.Size = 13;
            orderId.AddText(product.ProductUid.ToUpper());
        }

        private void CreatePage(Document doc, PosPdfLayout layout)
        {
            Section section = doc.AddSection();

            var config = _configurationBll.Get();
            string posPdfDirectory = Path.Combine(config.PathToPosContent, String.Concat("p", layout.PosId), "pdf");

            if (String.IsNullOrEmpty(layout.HeaderImage) == false)
            {
                Logger.DebugFormat("  going to decorate PDF coupon with header: `{0}`", layout.HeaderImage);
                string f = Path.Combine(posPdfDirectory, layout.HeaderImage);
                if (File.Exists(f))
                {
                    Image image = section.Headers.Primary.AddImage(f);
                    image.LockAspectRatio = true;
                    image.RelativeVertical = RelativeVertical.Page;
                    image.RelativeHorizontal = RelativeHorizontal.Page;
                    image.Top = ShapePosition.Top;
                    image.Left = ShapePosition.Right;
                    image.WrapFormat.Style = WrapStyle.Through;
                }
                else
                {
                    Logger.Warn("  header file does not exist: " + f);
                }
            }

            if (String.IsNullOrEmpty(layout.FooterImage) == false)
            {
                Logger.DebugFormat("  going to decorate PDF coupon with footer: `{0}`", layout.FooterImage);
                string f = Path.Combine(posPdfDirectory, layout.FooterImage);
                if (File.Exists(f))
                {
                    Image image = section.Footers.Primary.AddImage(f);
                    image.LockAspectRatio = true;
                    image.RelativeVertical = RelativeVertical.Page;
                    image.RelativeHorizontal = RelativeHorizontal.Page;
                    image.Top = ShapePosition.Bottom;
                    image.Left = ShapePosition.Right;
                    image.WrapFormat.Style = WrapStyle.Through; 
                }
                else
                {
                    Logger.Warn("  footer file does not exist: " + f);
                }
            }

            Paragraph p = section.Footers.Primary.AddParagraph();
            p.Format.Alignment = ParagraphAlignment.Right;
            p.Format.RightIndent = 0;
            p.Format.Font.Size = 7;
            p.Format.SpaceAfter = "0.5cm";
            p.AddText(config.ProjectDomain);
        }

        private void DefineStyles(Document doc)
        {
            // Get the predefined style Normal.
            Style style = doc.Styles["Normal"];
            
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Verdana";

            //style = doc.Styles[StyleNames.Header];
            //style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            //style = doc.Styles[StyleNames.Footer];
            //style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called Table based on style Normal
            //style = doc.Styles.AddStyle("Table", "Normal");
            //style.Font.Name = "Verdana";
            //style.Font.Name = "Times New Roman";
            //style.Font.Size = 9;

            // Create a new style called Reference based on style Normal
            //style = doc.Styles.AddStyle("Reference", "Normal");
            //style.ParagraphFormat.SpaceBefore = "5mm";
            //style.ParagraphFormat.SpaceAfter = "5mm";
            //style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);
        }

        public byte[] GetProductPdf(string productUid)
        {
            return GeneratProductPdf(productUid);
        }
    }
}
