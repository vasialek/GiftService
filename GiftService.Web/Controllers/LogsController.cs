using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GiftService.Web.Controllers
{
    public class LogsController : BaseController
    {
        // GET: Logs/List
        public ActionResult List()
        {
            var logs = Factory.LogsBll.GetLastLogs(0, 100);
            return View("List", "_LayoutAdmin", logs);
        }
    }
}
