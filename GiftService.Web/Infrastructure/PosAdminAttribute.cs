using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GiftService.Web.Infrastructure
{
    public class PosAdminAttribute : AuthorizeAttribute
    {

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool isAuthorized = base.AuthorizeCore(httpContext);
            if (isAuthorized == false)
            {
                return false;
            }

            return httpContext.User.IsInRole("POS administrator");
        }

    }
}
