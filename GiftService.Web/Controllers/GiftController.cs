using GiftService.Models;
using GiftService.Models.Exceptions;
using GiftService.Web.Models.Gifts;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
            return RedirectToAction("Index", "Home");
        }

        // GET: /Gift/Get/UniqueGiftId
        public ActionResult Get(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return new RedirectResult(Url.Action("Index", "Home"));
            }

            var model = new ProductInformationModel();
            try
            {
                TransactionBdo transaction = null;

                // Could be old UID or new OrderNr
                if (id.Length < this.MySettings.LengthOfPosUid)
                {
                    Logger.Info("Trying to get coupon by its order nr: " + id);
                    transaction = Factory.TransactionsBll.GetTransactionByOrderNr(id);
                }
                else
                {
                    // Try to get using UID
                    Logger.Info("Trying to get coupon by payment system UID: " + id);
                    transaction = Factory.TransactionsBll.GetTransactionByPaySystemUid(id);
                }

                model.Product = Factory.GiftsBll.GetProductByPaySystemUid(transaction.PaySystemUid);
                model.Pos = Factory.PosBll.GetById(model.Product.PosId);

                model.PaymentStatus = transaction.PaymentStatus;
                model.IsPaidOk = transaction.PaymentStatus == PaymentStatusIds.PaidOk;
                model.PaymentDate = transaction.PaySystemResponseAt;
            }
            catch (InvalidOperationException ioex)
            {
                Logger.Error(ioex);
                return NotFound();
            }
            catch (System.Data.Entity.Core.EntityException eex)
            {
                Logger.Fatal("Database exception, getting gift", eex);
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error("Error getting gift coupon by payment system UID: " + id, ex);
                throw;
            }


            return View("Get", GetLayoutForPos(model.Pos.Id), model);
        }

        // GET: /Gift/Download/UniqueGiftId
        public ActionResult Download(string id)
        {
            return OutputFile(id, false, true);
        }

        // GET: /Gift/Print/UniqueGiftId
        public ActionResult Print(string id)
        {
            return OutputFile(id, false, false);
        }

        // POST: /Gift/Friend
        [HttpPost]
        public ActionResult Friend(/*Models.Gifts.GiftToFriendModel model*/FormCollection form)
        {
            Logger.Info("Preparing gift coupon for friend...");
            var model = new GiftToFriendModel();
            TryUpdateModel<GiftToFriendModel>(model, "friendEmail");
            Logger.DebugFormat("  friend e-mail:   `{0}`", model.FriendEmail);
            Logger.DebugFormat("  text for friend: `{0}`", model.Text);
            Logger.DebugFormat("  BtnDownloadGift: `{0}`", model.BtnDownloadGift);
            Logger.DebugFormat("  BtnEmailGift:    `{0}`", model.BtnEmailGift);

            try
            {
                Factory.GiftsBll.MakeCouponGift(model.ProductUid, model.FriendEmail, model.Text);
            }
            catch (InvalidProductException ipex)
            {
                string msg;
                switch (ipex.Reason)
                {
                    case InvalidProductException.Reasons.ProductExpired:
                        msg = ipex.Message;
                        break;
                    case InvalidProductException.Reasons.ProductIsGiftAlready:
                        msg = "This coupon was dedicated to other user";
                        break;
                    default:
                        msg = Resources.Language.SystemMessage_WeWillContactYouOnError;
                        break;
                }
                this.SetTempMessage(msg);
                return Error();
            }

            return OutputFile(model.ProductUid, true, true);
        }

        // POST: /Gift/EmailFriend
        public ActionResult EmailFriend(FormCollection form)
        {
            bool isOk = false;
            string msg = "";

            Logger.Info("Preparing gift coupon for friend...");
            var model = new GiftToFriendModel();
            TryUpdateModel<GiftToFriendModel>(model, "friendEmail");
            Logger.DebugFormat("  friend e-mail:   `{0}`", model.FriendEmail);
            Logger.DebugFormat("  text for friend: `{0}`", model.Text);
            Logger.DebugFormat("  BtnDownloadGift: `{0}`", model.BtnDownloadGift);
            Logger.DebugFormat("  BtnEmailGift:    `{0}`", model.BtnEmailGift);

            try
            {
                Factory.GiftsBll.MakeCouponGift(model.ProductUid, model.FriendEmail, model.Text);
            }
            catch (InvalidProductException ipex)
            {
                switch (ipex.Reason)
                {
                    case InvalidProductException.Reasons.ProductExpired:
                        msg = ipex.Message;
                        break;
                    case InvalidProductException.Reasons.ProductIsGiftAlready:
                        msg = "This coupon was dedicated to other user";
                        break;
                    default:
                        msg = Resources.Language.SystemMessage_WeWillContactYouOnError;
                        break;
                }
            }

            try
            {
                var pdf = Factory.PdfBll.GeneratProductPdf(model.ProductUid, true);
                var product = Factory.GiftsBll.GetProductInformationByUid(model.ProductUid);
                Factory.CommunicationBll.SendCouponAsGift(model.FriendEmail, pdf, product);

                isOk = true;
                msg = String.Concat("PDF coupon is successfully sent to your friend email ", model.FriendEmail);
            }
            catch (Exception ex)
            {
                Logger.Error("Error sending PDF coupon as gift", ex);
                msg = ex.Message;
            }

            return Json(new
            {
                Status = isOk,
                Message = msg
            });
        }

        private ActionResult OutputFile(string productUid, bool asGift, bool forceDownload)
        {
            if (String.IsNullOrEmpty(productUid))
            {
                return new RedirectResult(Url.Action("Index", "Home"));
            }

            try
            {
                //byte[] pdf = Factory.PdfBll.GetProductPdf(productUid);
                byte[] pdf = Factory.PdfBll.GeneratProductPdf(productUid, asGift);

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
            return View("NotFound");
        }

        // GET: /Gift/Error
        public ViewResult Error()
        {
            return View("Error");
        }

    }
}
