using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GiftService.Web.Controllers
{
    public class PosController : BaseController
    {
        // GET: Pos
        public ActionResult Index()
        {
            return RedirectToAction("OurClients");
        }

        // GET: /Pos/OurClients
        public ActionResult OurClients()
        {
            var clients = Factory.PosBll.GetOurClients();

            return View("OurClients", clients);
        }

        // GET: /Pos/Preview/<POS_ID>
        public ActionResult Preview(int id)
        {
            return View("Preview", GetLayoutForPos(id));
        }
    }
}
