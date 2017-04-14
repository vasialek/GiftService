using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace GiftService.Web.Controllers
{
    public class AuthController : BaseController
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

        // GET: Auth
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return new RedirectResult(Url.Action("Index", "Home"));
            }

            var model = new Models.Auths.LoginModel();
            model.ReturnUrl = Request["ReturnUrl"];
            return View(model);
        }

        [HttpPost]
        public ActionResult Login(Models.Auths.LoginModel model)
        {
            try
            {
                Logger.InfoFormat("Trying to log user in by email `{0}`", model?.Email);
                var user = Factory.AuthBll.WebLogin(model.Email, model.Password);
                Logger.Info("  user is logged in");
                string s = Bll.DumpBll.Dump(user.Roles, String.Concat("User ", user.Username, " ", user.Email, " roles:"));
                Logger.DebugFormat("{0}{1}", Environment.NewLine, s);

                var claims = new List<System.Security.Claims.Claim>();
                claims.Add(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, user.UserId));
                claims.Add(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.Email));

                foreach (var r in user.Roles)
                {
                    claims.Add(new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, r.Name));
                }

                var identity = new System.Security.Claims.ClaimsIdentity(claims, "ApplicationCookie");
                HttpContext.GetOwinContext().Authentication.SignIn(
                    new Microsoft.Owin.Security.AuthenticationProperties
                    {
                        AllowRefresh = true
                    }, identity);

                if (String.IsNullOrEmpty(model.ReturnUrl) == false && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return new RedirectResult(model.ReturnUrl);
                }

                return new RedirectResult(Url.Action("Index", "Home"));
            }
            catch (Exception ex)
            {
                Logger.Error("Error logging user", ex);
                return View(model);
            }
        }

        public ActionResult Logout()
        {
            HttpContext.GetOwinContext().Authentication.SignOut();
            return new RedirectResult(Url.Action("Index", "Home"));
        }


    }
}