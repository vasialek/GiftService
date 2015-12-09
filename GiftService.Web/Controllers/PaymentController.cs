using AutoMapper;
using GiftService.Models;
using GiftService.Models.JsonModels;
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

        // GET: /Payment/Checkout/UniquePaymentId
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

            Factory.CommunicationBll.SendEmailToClientOnSuccess(product);


            return RedirectToAction("Get", "Gift", new { id = product.PaySystemUid });
        }

        private string GetLayoutForPos(int posId)
        {
            return "_Layout_Pos_1005";
        }
    }

}