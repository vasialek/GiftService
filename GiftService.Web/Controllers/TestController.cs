using GiftService.Models.JsonModels;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GiftService.Web.Controllers
{
    public class TestController : Controller
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

        // GET: Test
        public ActionResult Index()
        {
            Logger.Debug(Server.MapPath("~/Content"));
            return View();
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
