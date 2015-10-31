using GiftService.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var model = Factory.GiftsBll.GetProductInformationByUid(id);

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
