using GiftService.Models;
using GiftService.Models.Exceptions;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GiftService.Web.Controllers
{
    public class GiftController : BaseController
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

        // GET: Gift
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Gift/Get/UniqueGiftId
        public ActionResult Get(string id)
        {
            var model = new ProductInformationModel();

            try
            {
                var jsonUrl = new Uri("http://localhost:8079/it/testgs.php?act=info&posXXXXUid=" + id);
                Logger.InfoFormat("Requesting product info from POS: `{0}`", jsonUrl.ToString());
                WebClient wc = new WebClient();
                byte[] ba = wc.UploadData(jsonUrl, "POST", new byte[0]);
                if (ba == null || ba.Length == 0)
                {
                    throw new BadResponseException("Validate URL returns NULL response from POS");
                }

                string resp = "";
                resp = Encoding.UTF8.GetString(ba);
                Logger.Debug("Got product infromation from POS: " + resp);

                model.Pos = new PosBdo { Name = "Ritos masazai", PosUrl = new Uri("http://www.ritosmasazai.lt") };
                model.Product = (ProductBdo)Newtonsoft.Json.JsonConvert.DeserializeObject<ProductBdo>(resp);
                model.Product.ProductPrice = model.Product.ProductPrice / 100m;
                model.Product.ValidFrom = DateTime.Now;
                model.Product.ValidTill = model.Product.ValidFrom.AddMonths(3);
            }
            catch (InvalidCastException icex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }


            return View(model);
        }

        // GET: /Gift/Download/UniqueGiftId
        public ActionResult Download(string id)
        {
            return OutputFile(id, true);
        }

        // GET: /Gift/Print/UniqueGiftId
        public ActionResult Print(string id)
        {
            return OutputFile(id, false);
        }

        private ActionResult OutputFile(string productUid, bool forceDownload)
        {
            try
            {
                byte[] pdf = Factory.PdfBll.GetProductPdf(productUid);

                if (forceDownload)
                {
                    return File(pdf, "application/pdf", "Kuponas.pdf");
                }

                Response.AppendHeader("content-disposition", "inline; filename=Kuponas.pdf");
                return File(pdf, "application/pdf");
            }
            catch (InvalidOperationException ioex)
            {
                Logger.Error("Could not find product by UID: " + productUid, ioex);
                return new RedirectResult(Url.Action("NotFound"));
            }
            catch (Exception ex)
            {
                Logger.Error("Error showing product page", ex);
                return new RedirectResult(Url.Action("NotFound"));
            }
        }

        // GET: /Gift/NotFound
        public ViewResult NotFound()
        {
            return View();
        }
    }
}
