using GiftService.Models;
using GiftService.Models.Exceptions;
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
            return View();
        }

        // GET: /Gift/Get/UniqueGiftId
        public ActionResult Get(string id)
        {
            var model = new ProductInformationModel();

            try
            {
                var transaction = Factory.TransactionsBll.GetTransactionByPaySystemUid(id);
                model.Product = Factory.GiftsBll.GetProductByPaySystemUid(id);
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
            return OutputFile(id, true);
        }

        // GET: /Gift/Print/UniqueGiftId
        public ActionResult Print(string id)
        {
            return OutputFile(id, false);
        }

        private ActionResult OutputFile(string productUid, bool forceDownload)
        {
            try
            {
                //byte[] pdf = Factory.PdfBll.GetProductPdf(productUid);
                byte[] pdf = Factory.PdfBll.GeneratProductPdf(productUid);

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

        private string GetLayoutForPos(int posId)
        {
            if (posId == 1005)
            {
                return "_Layout_Pos_1005";
            }
            return "_Layout";
        }
    }
}
