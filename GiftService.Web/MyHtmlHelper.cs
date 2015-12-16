using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GiftService.Web
{
    public static class MyHtmlHelper
    {
        public static MvcHtmlString DisplayTempMessage(this HtmlHelper helper)
        {
            string tempMsg = helper.ViewContext.TempData["__TempMessage"] as string;
            if (String.IsNullOrEmpty(tempMsg))
            {
                return null;
            }
            var p = new TagBuilder("p");
            p.AddCssClass("alert alert-info");
            p.SetInnerText(tempMsg);
                
            return new MvcHtmlString(p.ToString());
        }
    }
}
