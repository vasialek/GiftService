using Microsoft.AspNet.Identity;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GiftService.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new Microsoft.Owin.Security.Cookies.CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new Microsoft.Owin.PathString("/Auth/Login"),
                ExpireTimeSpan = new TimeSpan(1, 0, 0)
            });
        }
    }
}
