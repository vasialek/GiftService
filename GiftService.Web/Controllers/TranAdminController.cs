﻿using GiftService.Models;
using GiftService.Models.Payments;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GiftService.Web.Controllers
{
    [Authorize]
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

        public ActionResult Validate()
        {
            return View();
        }

        // POST: /TranAdmin/Validate/E13-QWEREWR3
        [HttpPost]
        public ActionResult Validate(string id)
        {
            Logger.Info("Got request to validate transaction: " + id);
            bool isOk = false;
            string msg = "Nothing";

            try
            {
                TransactionBdo t = null;
                if (id.Length == MySettings.LengthOfPosUid)
                {
                    Factory.SecurityBll.ValidateUid(id);
                    t = Factory.TransactionsBll.GetTransactionByPaySystemUid(id);
                }
                else
                {
                    Factory.SecurityBll.ValidateOrderNr(id);
                    t = Factory.TransactionsBll.GetTransactionByOrderNr(id);
                }

                if (t == null)
                {
                    throw new Exception("Transaction is not found");
                }

                isOk = true;
                switch (t.PaymentStatus)
                {
                    case PaymentStatusIds.NotProcessed:
                        msg = String.Concat("Payment is not processed. Transaction created at: ", t.CreatedAt.ToString());
                        break;
                    case PaymentStatusIds.WaitingForPayment:
                        msg = String.Concat("Payment is not paid. Transaction created at: ", t.CreatedAt.ToString());
                        break;
                    case PaymentStatusIds.UserCancelled:
                        msg = String.Concat("Payment is cancelled by user. Transaction created at: ", t.CreatedAt.ToString());
                        break;
                    case PaymentStatusIds.PaidOk:
                        msg = String.Concat("Payment is paid. Paid at: ", t.PaySystemResponseAt.ToString(), ". Paid amount is: ", t.PaidAmount.ToString("### ##0.00"), t.PaidCurrencyCode);
                        break;
                    case PaymentStatusIds.AcceptedButNotExecuted:
                        msg = String.Concat("Payment is paid, but bank is processing it. Paid at: ", t.PaySystemResponseAt.ToString(), ". Paid amount is: ", t.PaidAmount.ToString("### ##0.00"), t.PaidCurrencyCode);
                        break;
                    default:
                        break;
                }
            }
            catch (InvalidOperationException ioex)
            {
                Logger.Error("No transaction was found", ioex);
                msg = "No transaction was found";
            }
            catch (Exception ex)
            {
                Logger.Error("Error validating transaction", ex);
                isOk = false;
                msg = ex.Message;
            }

            return Json(new { Status = isOk, Message = msg });
        }

        // GET: TranAdmin/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult GetLast(string id)
        {
            IEnumerable<TransactionBdo> transactions = null;
            try
            {
                int posId = -1;
                int.TryParse(id, out posId);
                transactions = Factory.TransactionsBll.GetLastTransactions(posId, 0, 50);
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

        // POST: /TranAdmin/DoParsePs
        public ActionResult DoParsePs()
        {
            bool isOk = false;
            string msg = "";
            PayseraPaymentResponse resp = null;
            string r = String.Empty;

            try
            {
                string data = Request["data"];
                if (String.IsNullOrEmpty(data))
                {
                    throw new ArgumentNullException("data", "No data from PS is provided");
                }

                resp = Factory.PayseraBll.ParseData(data);
                r = RenderPartialView("_PsResponsePartial", resp);
                isOk = true;
                msg = "Response from PS is parserd";
            }
            catch (Exception ex)
            {
                isOk = false;
                msg = ex.Message;
                resp = null;
            }

            return Json(new
            {
                Status = isOk,
                Message = msg,
                Response = r
            });
        }

        // POST: /TranAdmin/DoParsePsRequest
        public ActionResult DoParsePsRequest()
        {
            bool isOk = false;
            string msg = "";
            Dictionary<string, string> ar = null;
            string r = String.Empty;

            try
            {
                string data = Request["data"];
                if (String.IsNullOrEmpty(data))
                {
                    throw new ArgumentNullException("data", "No data from PS request is provided");
                }

                ar = Factory.PayseraBll.ParseRequestData(data);
                r = RenderPartialView("_PsRequestPartial", ar);
                isOk = true;
                msg = "Request from PS is parserd";
            }
            catch (Exception ex)
            {
                isOk = false;
                msg = ex.Message;
            }

            return Json(new
            {
                Status = isOk,
                Message = msg,
                Response = r
            });
        }

    }
}
