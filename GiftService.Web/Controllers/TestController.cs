using GiftService.Models;
using GiftService.Models.JsonModels;
using GiftService.Models.Products;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GiftService.Web.Controllers
{
    [Authorize(Roles = "Developer")]
    public class TestController : BaseController
    {
        // Test products for shop
        private static IList<ProductBdo> _products;

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

        public TestController()
        {
            var pos = new PosBdo();
            pos.Id = 6666;
            pos.Name = "Test shop";

            string uid;
            if (_products == null)
            {
                _products = new List<ProductBdo>();

                uid = Factory.HelperBll.GenerateProductUid(pos.Id);
                _products.Add(new ProductBdo { ProductUid = uid, Id = 666001, ProductName = "Test product #1", ProductPrice = 666.01m, PosId = pos.Id, PosName = pos.Name });
                uid = Factory.HelperBll.GenerateProductUid(pos.Id);
                _products.Add(new ProductBdo { ProductUid = uid, Id = 666001, ProductName = "Test product #2", ProductPrice = 666.22m, PosId = pos.Id, PosName = pos.Name }); 
            }
        }

        // GET: Test
        public ActionResult Index()
        {
            //Session["xxx"] = "13246579874651564645";
            //if (String.IsNullOrEmpty(Request["posId"]) == false)
            //{
            //    SessionStore.PosId = int.Parse(Request["posId"]);
            //}
            //Logger.Debug(Server.MapPath("~/Content"));
            //SetTempMessage(Resources.Language.Payment_PaymentIsOk);
            //return RedirectToAction("Get", "Gift", new { id = "fe91f4287ca54d8cac3fa7ca4d5eb0ac" });
            return Content("Order ID: " + Factory.GiftsBll.GetUniqueOrderId(1005));
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
        public JsonResult Validate(string id)
        {
            var resp = new PaymentRequestValidationResponse();
            try
            {
                var product = _products.First(x => x.ProductUid == id);
                string[] errors = new string[0];

                resp.Status = true;
                resp.Message = "Ok";
                resp.Errors = errors;
                resp.ProductName = product.ProductName;
                resp.RequestedAmountMinor = (int)(product.ProductPrice * 100);
                resp.CurrencyCode = "EUR";
                resp.Locations = new List<ProductServiceLocation>();
                resp.Locations.Add(new ProductServiceLocation { Id = 666001, Name = "LocalShop", City = "Computer", Address = "127.0.0.1" });
                //return Json(new PaymentRequestValidationResponse
                //{
                //    Status = true,
                //    Message = "Ok",
                //    Errors = errors,
                //    RequestedAmountMinor = 666,
                //    CurrencyCode = "EUR",
                //    ProductName = "Massage #2",
                //    ProductDescription = "Very good and sensitive"
                //});
            }
            catch (Exception ex)
            {
                resp.Status = false;
                resp.Errors = new string[] { ex.Message };
            }
            return Json(resp);
        }

        // GET: /Test/Shop
        public ActionResult Shop(string id)
        {
            if (String.IsNullOrEmpty(id) == false)
            {
                SessionStore.PosId = int.Parse(id);
            }
            return View("Shop", GetLayoutForPos(), _products);
        }
    }
}
