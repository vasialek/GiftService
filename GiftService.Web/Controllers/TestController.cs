using GiftService.Models.JsonModels;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GiftService.Web.Controllers
{
    public class TestController : BaseController
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

        public ActionResult MeasureCache()
        {
            int total = 0;
            string v;
            foreach (string key in Session.Keys)
            {
                v = Session[key] as string;
                total += v == null ? 0 : v.Length;
            }

            return Content("Total in session: " + total + " bytes");
        }

        // GET: Test
        public ActionResult Index()
        {
            Session["xxx"] = "13246579874651564645";
            if (String.IsNullOrEmpty(Request["posId"]) == false)
            {
                SessionStore.PosId = int.Parse(Request["posId"]);
            }
            //Logger.Debug(Server.MapPath("~/Content"));
            //SetTempMessage(Resources.Language.Payment_PaymentIsOk);
            //return RedirectToAction("Get", "Gift", new { id = "fe91f4287ca54d8cac3fa7ca4d5eb0ac" });
            return View("", GetLayoutForPos());
        }

        public ActionResult Log()
        {
            Logger.Debug("Test debug...");
            Logger.Info("Test info...");
            Logger.Warn("Test warn...");
            Logger.Error("Test error...");
            Logger.Fatal("Test fatal...");
            return Content("OK");
        }

        // POST: /Test/Validate
        [HttpPost]
        public JsonResult Validate()
        {
            string[] errors = new string[0];
            return Json(new PaymentRequestValidationResponse
            {
                Status = true,
                Message = "Ok",
                Errors = errors,
                RequestedAmountMinor = 666,
                CurrencyCode = "EUR",
                ProductName = "Massage #2",
                ProductDescription = "Very good and sensitive"
            });
        }
    }
}
