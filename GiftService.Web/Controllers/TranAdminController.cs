using GiftService.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GiftService.Web.Controllers
{
    public class TranAdminController : BaseController
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

        // GET: TranAdmin
        public ActionResult Index()
        {
            return View("Index", "_LayoutAdmin");
        }

        // GET: TranAdmin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult GetLast()
        {
            IEnumerable<TransactionBdo> transactions = null;
            try
            {
                transactions = Factory.TransactionsBll.GetLastTransactions(1005, 0, 50);
            }
            catch (Exception ex)
            {
                Logger.Error("Error getting list of last transactions", ex);
                ViewBag.ErrorMessage = ex.Message;
            }

            return PartialView(transactions);
        }

        // GET: /TranAdmin/GetProduct
        public ActionResult GetProduct()
        {
            string name = "", location = "", price = "";
            try
            {
                string productUid = Request["productUid"];
                var product = Factory.GiftsBll.GetProductInformationByUid(productUid);

                name = product.Product.ProductName;
                price = String.Concat(product.Product.ProductPrice.ToString("### ##0.00"), product.Product.CurrencyCode);
                location = String.Concat(product.Product.PosName, " ", product.Product.PosAddress, ", ", product.Product.PosCity);
            }
            catch (Exception ex)
            {
                name = ex.Message;
                Logger.Error("Error getting product information by ProductUid: " + Request["productUid"], ex);
            }

            return Json(new { ProductName = name, ProductPrice = price, Location = location });
        }
    }
}
