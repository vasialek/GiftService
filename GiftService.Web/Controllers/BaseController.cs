using GiftService.Bll;
using GiftService.Models;
using GiftService.Models.Web;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace GiftService.Web.Controllers
{
    public class BaseController : Controller
    {
        private ILog _logger = null;
        private MySettings _settings = null;
        protected SessionStore _sessionStore = null;
        protected BllFactory _bllFactory = null;

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

        protected MySettings MySettings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = Factory.ConfigurationBll.Get();
                }
                return _settings;
            }
        }

        protected BllFactory Factory
        {
            get
            {
                return BllFactory.Current;
            }
        }

        protected SessionStore SessionStore
        {
            get
            {
                if (_sessionStore == null)
                {
                    //Logger.Debug("Creating/restoring session store");
                    _sessionStore = Session["__GsSession"] as SessionStore;
                    if (_sessionStore == null)
                    {
                        //Logger.Debug("  creating new session store");
                        _sessionStore = new SessionStore();
                        Session["__GsSession"] = _sessionStore;
                    }
                }

                return _sessionStore;
            }
        }

        protected CultureInfo CultureUi
        {
            get
            {
                return System.Threading.Thread.CurrentThread.CurrentUICulture;
            }
        }

        public BaseController()
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("lt-LT");
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("lt-LT");
        }

        protected virtual string GetLayoutForPos(int posId = 0)
        {
            if (posId > 0)
            {
                //Logger.DebugFormat("Layout for POS ID: {0}", posId);
                // TODO: check if valid POS
                return String.Concat("_LayoutPos_", posId);
            }

            if (SessionStore.PosId > 0)
            {
                //Logger.DebugFormat("Layout for POS ID (from session): {0}", SessionStore.PosId);
                return String.Concat("_LayoutPos_", SessionStore.PosId);
            }

            //Logger.Debug("Default Layout: _Layout");
            return "_Layout";
        }

        protected void SetTempMessage(string msg)
        {
            TempData["__TempMessage"] = msg;
        }

        protected override ViewResult View(string viewName, string masterName, object model)
        {
            this.ViewBag.ProjectName = MySettings.ProjectName;
            return base.View(viewName, masterName, model);
        }

        protected override void HandleUnknownAction(string actionName)
        {
            Logger.Warn("[404] Unknown action: " + actionName + ". Request is: " + Request.RawUrl);
            base.HandleUnknownAction(actionName);
        }
    }
}