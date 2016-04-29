using GiftService.Models.Logs;
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
            IEnumerable<LogBdo> logs = Factory.LogsBll.GetLastLogs(0, 100);
            //IEnumerable<LogBdo> logs = new List<LogBdo>();
            return View("List", "_LayoutAdmin", logs);
        }
    }
}
