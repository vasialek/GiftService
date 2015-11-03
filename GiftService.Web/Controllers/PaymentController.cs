using GiftService.Models;
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

                Logger.InfoFormat("Validating payment request: `{0}`", pos.ValidateUrl);
                Factory.SecurityBll.ValidatePosPaymentRequest(pos);

                return View();
            }
            catch (Exception ex)
            {
                Logger.Error("Error making payment for POS", ex);
                return new RedirectResult(Url.Action("Error"));
            }
        }
    }

}