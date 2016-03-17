using AutoMapper;
using GiftService.Models;
using GiftService.Models.Exceptions;
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
            Logger.Info("Got payment callback from Paysera:");
            Logger.Info(Request.RawUrl);

            try
            {
                string data = Request["data"];
                string ss1 = Request["ss1"];

                var resp = Factory.PayseraBll.ParseData(data);
                Logger.InfoFormat("    for project (payer) ID: `{0}`", resp.ProjectId);
                var pos = Factory.PosBll.GetByPayseraPayerId(resp.ProjectId);
                Factory.PayseraBll.ValidateSs1(pos.PayseraPassword, data, ss1);
                //Factory.PayseraBll.ValidateSs1(Factory.ConfigurationBll.Get().PayseraPassword, data, ss1);

                Logger.Info("Payment callback is valid, update status of transaction");
                Factory.TransactionsBll.FinishTransaction(resp);
            }
            catch (Exception ex)
            {
                Logger.Error("Error processing payment information", ex);
            }

            // Response OK anyway
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

                string data = Request["data"];
                string ss1 = Request["ss1"];

                //data = "b3JkZXJpZD02ZmU2OGE2MTYzZWM0MDQzYjBjMDdlYWRmMjFmMDY4YiZhbW91bnQ9MSZjdXJyZW5jeT1FVVImY291bnRyeT1MVCZ0ZXN0PTAmcGF5bWVudD12YjImcF9lbWFpbD1wcm8uZ2xhbWVyJTQwZ21haWwuY29tJnBfZmlyc3RuYW1lPUFsZWtzZWorVGFrJnBfbGFzdG5hbWU9JnBfcGhvbmU9JTJCMzcwKzYwMCsxMjM0NSZwX2NvbW1lbnQ9JnBfaXA9JnBfYWdlbnQ9JnBfZmlsZT0mdmVyc2lvbj0xLjYmcHJvamVjdGlkPTc2NDU3JnBheXRleHQ9Uml0b3NNYXNhemFpLmx0Ky0rR2lsdXMrcHJpc2lsaWV0aW1hcy4rSnVzdSt1enNha3ltYXMraHR0cCUzQSUyRiUyRnd3dy5kb3ZhbnVrdXBvbmFpLmNvbSUyRmdpZnQlMkZnZXQlMkY2ZmU2OGE2MTYzZWM0MDQzYjBjMDdlYWRmMjFmMDY4Yi4rRGVrdW9qdSUyQytSaXRhKyVDNSVCRGlidXRpZW4lQzQlOTcmbGFuZz1saXQmbV9wYXlfcmVzdG9yZWQ9OTExNTA5OTcmc3RhdHVzPTImcmVxdWVzdGlkPTkxMTUwOTk3Jm5hbWU9QWxla3NlaiZzdXJlbmFtZT1WYXNpbm92JnBheWFtb3VudD0xJnBheWN1cnJlbmN5PUVVUiZhY2NvdW50PUxUMzQ3MDQ0MDAwNDM5NjE0NjQ0";
                //ss1 = "37d30d102b7d9044a55a4994bfe65ec2";

                var responseFromPaysera = Factory.PayseraBll.ParseData(data);
                Logger.InfoFormat("  Response from Paysera is parsed, checking SS1: `{0}`", ss1);
                Logger.InfoFormat("    for project (payer) ID: `{0}`", responseFromPaysera.ProjectId);
                var pos = Factory.PosBll.GetByPayseraPayerId(responseFromPaysera.ProjectId);
                Factory.PayseraBll.ValidateSs1(pos.PayseraPassword, data, ss1);
                //Factory.PayseraBll.ValidateSs1(Factory.ConfigurationBll.Get().PayseraPassword, data, ss1);

                var t = Factory.TransactionsBll.FinishTransaction(responseFromPaysera);
                if (t.PaymentStatus != PaymentStatusIds.PaidOk && t.PaymentStatus != PaymentStatusIds.AcceptedButNotExecuted)
                {
                    Logger.WarnFormat("Transaction `{0}` is not accepted, bad PaymentStatus {1} ({2})", t.PaySystemUid, t.PaymentStatus, (int)t.PaymentStatus);
                    return Bad();
                }

                Logger.InfoFormat("Transaction `{0}` is accepted, status is: {1}", t.PaySystemUid, t.PaymentStatus);
                SetTempMessage(Resources.Language.Payment_PaymentIsOk);
                return RedirectToAction("Get", "Gift", new { id = t.PaySystemUid });
            }
            catch (Exception ex)
            {
                Logger.Error("Error accepting callback from payment system", ex);
                return Bad();
            }
        }

        // GET: /Payment/Cancel
        public ActionResult Cancel()
        {
            PosBdo pos = null;
            try
            {
                Logger.Info("Got response from payment system to cancel transaction (by user)");
                //string paySystemUid = Session["__PaySystemUid"] as string;
                Logger.InfoFormat("  Payment Order nr in session is `{0}`", SessionStore.PaymentOrderNr);

                //var responseFromPaysera = Factory.PayseraBll.ParseData(Request["data"]);
                //var t = Factory.TransactionsBll.CancelTransactionByUser(paySystemUid);
                var t = Factory.TransactionsBll.CancelTransactionByUserUsingOrderNr(SessionStore.PaymentOrderNr);

                // Try to get POS information
                if (SessionStore.PosId > 0)
                {
                    pos = Factory.PosBll.GetById(SessionStore.PosId);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("Error canceling transaction", ex);
            }

            return View("Cancel", GetLayoutForPos(), pos);
        }

        public ActionResult Bad()
        {
            return View("Bad", GetLayoutForPos());
        }

        // GET: /Payment/Incorrect
        public ActionResult Incorrect(string msg = null)
        {
            ViewBag.Message = msg;
            return View("Incorrect", GetLayoutForPos());
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
                SessionStore.PosId = pos.Id;
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
                model.RequestedAmount = posResponse.RequestedAmountMinor / 100m;
                //model.CurrencyCode = posResponse.CurrencyCode;
                model.Locations = posResponse.Locations ?? new List<ProductServiceLocation>();

                return View("Checkout", GetLayoutForPos(posId), model);
            }
            catch (IncorrectPaymentParamersException ippex)
            {
                Logger.Error("Incorrect price of product (less than 0)", ippex);
                return Incorrect(Resources.Language.Payment_Make_Error_IncorrectPriceFromRequest);
            }
            catch (System.Net.WebException wex)
            {
                Logger.Error("Probably, there is no connection with POS", wex);
                //throw;
                return Incorrect();
            }
            catch (Newtonsoft.Json.JsonReaderException jre)
            {
                Logger.Error("Incorrect JSON response from POS", jre);
                return Incorrect();
            }
            catch (Exception ex)
            {
                Logger.Error("Error validating payment request from POS", ex);
                //return new RedirectResult(Url.Action("Error"));
                return Incorrect();
            }
        }

        // POST: /Payment/Checkout/UniquePaymentId
        [HttpPost]
        public ActionResult Checkout(string id, ProductCheckoutModel checkout)
        {

            try
            {
                var posResponse = Session["__Product"] as PaymentRequestValidationResponse;
                if (posResponse == null)
                {
                    throw new ArgumentNullException("No product information from POS in session");
                }

                if (checkout.LocationId < 1)
                {
                    ModelState.AddModelError("LocationId", Resources.Language.Payment_Checkout_ChooseLocation);
                }
                if (ModelState.IsValid == false)
                {
                    checkout.ProductName = posResponse.ProductName;
                    checkout.ProductDescription = posResponse.ProductDescription;
                    checkout.ProductDuration = posResponse.ProductDuration;
                    checkout.PosUserUid = id;
                    checkout.ProductValidTill = Factory.HelperBll.ConvertFromUnixTimestamp(posResponse.ProductValidTillTm);
                    checkout.RequestedAmount = posResponse.RequestedAmountMinor / 100m;
                    checkout.Locations = posResponse.Locations ?? new List<ProductServiceLocation>();
                    return View("Checkout", GetLayoutForPos(posResponse.PosId), checkout);
                }

                var product = Factory.GiftsBll.SaveProductInformationFromPos(id, posResponse, checkout);
                var transaction = Factory.TransactionsBll.StartTransaction(id, product);

                var configuration = Factory.ConfigurationBll.Get();

                //Factory.CommunicationBll.SendEmailToClientOnSuccess(product);
                var rq = new PayseraPaymentRequest();

                // Paysera information is stored in POS
                var pos = Factory.PosBll.GetById(posResponse.PosId);
                rq.ProjectId = pos.PayseraPayerId;
                rq.PayseraProjectPassword = pos.PayseraPassword;
                Logger.DebugFormat("  configuring Paysera payment to use payer ID: {0} for POS ID: {1}", rq.ProjectId, pos.Id);

                //rq.OrderId = transaction.PaySystemUid;
                rq.OrderId = transaction.OrderNr;
                rq.AmountToPay = posResponse.RequestedAmountMinor / 100m;
                rq.CurrencyCode = posResponse.CurrencyCode;
                rq.Country = PayseraPaymentRequest.Countries.LT;

                rq.AcceptUrl = Url.Action("accept", "payment", null, Request.Url.Scheme);
                rq.CancelUrl = Url.Action("cancel", "payment", null, Request.Url.Scheme);
                rq.CallbackUrl = Url.Action("callback", "payment", null, Request.Url.Scheme);

                rq.CustomerName = checkout.CustomerName;
                rq.CustomerEmail = checkout.CustomerEmail;
                rq.CustomerPhone = checkout.CustomerPhone;

                rq.PayText = Factory.PosBll.FormatNoteForPayment(pos, product, configuration.MaxLengthOfPayseraNote);
                Logger.Debug("  sending PayText: " + rq.PayText);

                //rq.Language = PayseraPaymentRequest.Languages.LIT;
                rq.IsTestPayment = configuration.UseTestPayment;

                Uri paymentUri = Factory.PayseraBll.PreparePaymentLink(configuration.PayseraPaymentUrl, rq);
                Logger.Info("Redirecting to Paysera:");
                Logger.Info(paymentUri.ToString());

                Logger.InfoFormat("  saving payment order nr in session in case of cancel: `{0}`", rq.OrderId);
                //Session["__PaySystemUid"] = rq.OrderId;
                SessionStore.PaymentOrderNr = rq.OrderId;

                return Redirect(paymentUri.ToString());

            }
            catch (Exception ex)
            {
                Logger.Error("Error processing payment form", ex);
                return Incorrect();
            }
        }

    }

}