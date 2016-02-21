using GiftService.Bll;
using GiftService.Models.Web;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace GiftService.Web.Controllers
{
    public class BaseController : Controller
    {
        private ILog _logger = null;
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

    }
}