using GiftService.Web.Infrastructure;
using System.Web;
using System.Web.Mvc;

namespace GiftService.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CultureFilter("lt"));
            filters.Add(new HandleErrorAttribute());
        }
    }
}
