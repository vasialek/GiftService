using GiftService.Models;
using GiftService.Models.Texts;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GiftService.Web.Controllers
{
    public class HomeController : BaseController
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

        public ActionResult Index()
        {
            return View("Index");
        }

        public ActionResult About()
        {
            TextModule tm = null;

            try
            {
                tm = Factory.TextModuleBll.GetByLabel("AboutUs", "lt");
            }
            catch (Exception ex)
            {
                Logger.Error("TextModule with `AboutUs` is not found, use default", ex);
                tm = new TextModule();
                tm.Title = Resources.Language.Home_AboutUs_Title;
                tm.Text = Resources.Language.SystemMessage_NotFound;
            }

            return View(tm);
        }

        // GET: /Home/Rules
        public ActionResult Rules()
        {
            return View("Rules");
        }

        // GET: /Home/Contact
        public ActionResult Contact()
        {
            var model = new ContactUsModel();
            return View("Contact", model);
        }

        // POST: /Home/Contact
        [HttpPost]
        public ActionResult Contact(ContactUsModel model)
        {
            if (ModelState.IsValid)
            {
                model.IsSent = true;
            }
            return View("Contact", model);
        }
    }
}
