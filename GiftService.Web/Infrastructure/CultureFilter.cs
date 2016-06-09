using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace GiftService.Web.Infrastructure
{
    public class CultureFilter : IAuthorizationFilter
    {
        private readonly string defaultCulture;

        public CultureFilter(string defaultCulture)
        {
            this.defaultCulture = defaultCulture;
        }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            var values = filterContext.RouteData.Values;

            string culture = (string)values["culture"] ?? this.defaultCulture;

            CultureInfo ci = new CultureInfo(culture);

            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(ci.Name);
        }
    }
}
