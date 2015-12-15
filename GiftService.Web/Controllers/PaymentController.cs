using AutoMapper;
using GiftService.Models;
using GiftService.Models.JsonModels;
using GiftService.Models.Payments;
using GiftService.Models.Products;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace GiftService.Web.Controllers
{
    public class PaymentController : BaseController
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

        // GET: Payment
        public ActionResult Index()
        {
            return Content("{\"Status\":\"Ok\"}", "application/json");
        }

        // GET: /Payment/Callback
        public ActionResult Callback()
        {
            Logger.Info("Got information about payment:");
            Logger.Info(Request.RawUrl);
            return Content("OK");
        }

        // GET: /Payment/Accept
        public ActionResult Accept()
        {
            // Expected successful response
            // ?data=b3JkZXJpZD1iZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYiZhbW91bnQ9MSZjdXJyZW5jeT1FVVImY291bnRyeT1MVCZ0ZXN0PTEmcGF5bWVudD1ub3JkZWFsdCZwX2VtYWlsPWl0JTQwaW50ZXJhdGVpdGlzLmx0JnBfZmlyc3RuYW1lPUFsZWtzZWomcF9sYXN0bmFtZT1WYXNpbm92JnBfcGhvbmU9JTJCMzcwKzUrMjE2NjQ4MSZwX2NvbW1lbnQ9SnVzdCt0ZXN0K3BheW1lbnQmcF9pcD0mcF9hZ2VudD0mcF9maWxlPSZ2ZXJzaW9uPTEuNiZwcm9qZWN0aWQ9NzY0NTcmbGFuZz1saXQmcGF5dGV4dD1VJUM1JUJFc2FreW1hcytuciUzQStiZmFhZmM2MTkwMGI0NTZjODczMTBjYzg3NTZkODBhYitodHRwJTNBJTJGJTJGd3d3LmRvdmFudWt1cG9uYWkuY29tK3Byb2pla3RlLislMjhQYXJkYXYlQzQlOTdqYXMlM0ErUml0YSslQzUlQkRpYnV0aWVuJUM0JTk3JTI5Jm1fcGF5X3Jlc3RvcmVkPTkwNjg1Mjg5JnN0YXR1cz0xJnJlcXVlc3RpZD05MDY4NTI4OSZwYXlhbW91bnQ9MSZwYXljdXJyZW5jeT1FVVI%3D&ss1=66a9c204373d26b7d3f071f2f8c07722&ss2=2UkCwi6g56yfYbCd27UVR8ZW2nHz715-HZjicZcSyzYa-bqIz2lNiE6A68v90Mm4xOEAeqpGhJSOSS85KT8pXAguylpsdXYZ-E_mKXoEsa6WzsAF-KqWgJFAXeDlS7dYDNa_1UF9Pbeu7DntDyroxJQbOb65CH5fEFhXNp0g1-Y%3D

            try
            {
                Logger.Info("Got response from payment system");
                Logger.Info(Request.RawUrl);

                var responseFromPaysera = Factory.PayseraBll.ParseData(Request["data"]);
                var t = Factory.TransactionsBll.FinishTransaction(responseFromPaysera);
                if (t.PaymentStatus != PaymentStatusIds.PaidOk)
                {
                    return Bad();
                }

                return RedirectToAction("Get", "Gift", new { id = t.PaySystemUid });
            }
            catch (Exception ex)
            {
                Logger.Error("Error accepting callback from payment system", ex);
                throw;
            }
        }

        // GET: /Payment/Cancel
        public ActionResult Cancel()
        {
            try
            {
                Logger.Info("Got response from payment system to cancel transaction (by user)");
                string paySystemUid = Session["__PaySystemUid"] as string;
                Logger.InfoFormat("  PaySystemUid in session is `{0}`", paySystemUid);

                //var responseFromPaysera = Factory.PayseraBll.ParseData(Request["data"]);
                var t = Factory.TransactionsBll.CancelTransactionByUser(paySystemUid);
            }
            catch (Exception ex)
            {
                Logger.Error("Error canceling transactio", ex);
            }

            return View("Cancel", GetLayoutForPos(1005));
        }

        public ActionResult Bad()
        {
            return View("Bad", GetLayoutForPos(1005));
        }

        // GET: /Payment/Make/UniquePaymentId
        public ActionResult Make(string id)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Got request to make payment");
                sb.AppendFormat("  POS URL: {0}", Request.UserHostAddress).AppendLine();
                sb.AppendFormat("  UID from POS: `{0}`", id);
                Logger.Info(sb.ToString());

                int posId = Factory.PosBll.GetPosIdFromUid(id);
                PosBdo pos = Factory.PosBll.GetById(posId);

                var posResponse = Factory.SecurityBll.ValidatePosPaymentRequest(pos, id);
                posResponse.PosId = pos.Id;
                if (posResponse.Status != true)
                {
                    Logger.ErrorFormat("POS returns false on request. " + posResponse.Message);
                    throw new Models.Exceptions.BadResponseException(posResponse.Message);
                }

                this.Session["__Product"] = posResponse;

                ProductCheckoutModel model = new ProductCheckoutModel();
                Mapper.CreateMap<PaymentRequestValidationResponse, ProductCheckoutModel>();
                model = Mapper.Map<ProductCheckoutModel>(posResponse);
                model.PosUserUid = id;
                //model.ProductName = posResponse.ProductName;
                //model.ProductDuration = posResponse.ProductDuration;
                model.ProductValidTill = Factory.HelperBll.ConvertFromUnixTimestamp(posResponse.ProductValidTillTm);
                model.RequestedAmount = posResponse.RequestedAmountMinor / 100;
                //model.CurrencyCode = posResponse.CurrencyCode;
                model.Locations = posResponse.Locations ?? new List<ProductServiceLocation>();

                return View("Checkout", GetLayoutForPos(posId), model);
            }
            catch (System.Net.WebException wex)
            {
                Logger.Error("Probably, there is no connection with POS", wex);
                throw;
            }
            catch (Newtonsoft.Json.JsonReaderException jre)
            {
                Logger.Error("Incorrect JSON response from POS", jre);
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error("Error making payment for POS", ex);
                //return new RedirectResult(Url.Action("Error"));
                throw;
            }
        }

        // POST: /Payment/Checkout/UniquePaymentId
        [HttpPost]
        public ActionResult Checkout(string id, ProductCheckoutModel checkout)
        {
            var posResponse = Session["__Product"] as PaymentRequestValidationResponse;
            if (posResponse == null)
            {
                throw new ArgumentNullException("No product information from POS in session");
            }

            var product = Factory.GiftsBll.SaveProductInformationFromPos(id, posResponse, checkout);
            var transaction = Factory.TransactionsBll.StartTransaction(id, product);

            var configuration = Factory.ConfigurationBll.Get();

            //Factory.CommunicationBll.SendEmailToClientOnSuccess(product);
            var rq = new PayseraPaymentRequest();

            rq.PayseraProjectPassword = configuration.PayseraPassword;
            rq.ProjectId = configuration.PayseraProjectId.ToString();

            rq.OrderId = transaction.PaySystemUid;
            rq.AmountToPay = posResponse.RequestedAmountMinor / 100m;
            rq.CurrencyCode = posResponse.CurrencyCode;
            rq.Country = PayseraPaymentRequest.Countries.LT;

            rq.AcceptUrl = Url.Action("accept", "payment", null, Request.Url.Scheme);
            rq.CancelUrl = Url.Action("cancel", "payment", null, Request.Url.Scheme);
            rq.CallbackUrl = Url.Action("callback", "payment", null, Request.Url.Scheme);
            //rq.AcceptUrl = "http://www.dovanukuponai.com/payment/accept/";
            //rq.CancelUrl = "http://www.dovanukuponai.com/payment/cancel/";
            //rq.CallbackUrl = "http://www.dovanukuponai.com/payment/callback/";

            rq.CustomerName = checkout.CustomerName;
            rq.CustomerEmail = checkout.CustomerEmail;
            rq.CustomerPhone = checkout.CustomerPhone;
            //rq.PayText = String.Format("Apmokėjimas už [owner_name] - {0}, per [site_name], http://www.dovanukuponai.com/gift/get/[order_nr]", checkout.ProductName);
            string shortProductName = posResponse.ProductName.Length > 90 ? posResponse.ProductName.Substring(0, 90) : posResponse.ProductName;
            rq.PayText = String.Concat("RitosMasazai.lt - ", shortProductName , ". Jusu uzsakymas http://www.dovanukuponai.com/gift/get/[order_nr]. Dekuoju, [owner_name]");
            Logger.Debug("  sending PayText: " + rq.PayText);
            //rq.Language = PayseraPaymentRequest.Languages.LIT;
            rq.IsTestPayment = configuration.UseTestPayment;

            Uri paymentUri = Factory.PayseraBll.PreparePaymentLink(configuration.PayseraPaymentUrl, rq);
            Logger.Info("Redirecting to Paysera:");
            Logger.Info(paymentUri.ToString());

            Logger.InfoFormat("  saving PaySystemUid in session in case of cancel: `{0}`", rq.OrderId);
            Session["__PaySystemUid"] = rq.OrderId;

            return Redirect(paymentUri.ToString());
            return RedirectToAction("Get", "Gift", new { id = product.PaySystemUid });
        }

        //protected decimal ParseDecimal(string s)
        //{
        //    decimal d = 0;

        //    return d;
        //}

        private string GetLayoutForPos(int posId)
        {
            return "_Layout_Pos_1005";
        }
    }

}