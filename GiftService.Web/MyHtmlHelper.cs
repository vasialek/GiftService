using GiftService.Bll;
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

        public static HtmlString LoaderDiv(this HtmlHelper htmlHelper)
        {
            return LoaderDiv(htmlHelper, null);
        }

        public static HtmlString LoaderDiv(this HtmlHelper htmlHelper, object htmlAttributes)
        {
            TagBuilder div = new TagBuilder("div");
            div.MergeAttributes(BllFactory.Current.HelperBll.DynamicObjectToDictionary(new { id = "LoaderDiv", style = "display: none;" }));
            if (htmlAttributes != null)
            {
                div.MergeAttributes(BllFactory.Current.HelperBll.DynamicObjectToDictionary(htmlAttributes), true);
            }
            UrlHelper urlHelper = ((Controller)htmlHelper.ViewContext.Controller).Url;

            TagBuilder img = new TagBuilder("img");
            img.MergeAttributes(BllFactory.Current.HelperBll.DynamicObjectToDictionary(new { width = 16, height = 16, alt = "Loading...", src = urlHelper.Content("~/content/images/loader.gif") }));

            div.InnerHtml = img.ToString();
            return new HtmlString(div.ToString());
        }

        public static MvcHtmlString DisplayOrNotSet(this HtmlHelper helper, string msg, string defaultValue = "not set")
        {
            return new MvcHtmlString(String.IsNullOrEmpty(msg) ? defaultValue : msg);
        }
    }
}
