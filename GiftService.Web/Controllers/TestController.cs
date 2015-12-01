using GiftService.Models.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GiftService.Web.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
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
