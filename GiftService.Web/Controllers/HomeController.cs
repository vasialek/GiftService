using GiftService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GiftService.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View("Index", GetLayoutForPos());
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        // GET: /Home/Rules
        public ActionResult Rules()
        {
            return View("Rules", GetLayoutForPos());
        }

        // GET: /Home/Contact
        public ActionResult Contact()
        {
            var model = new ContactUsModel();
            return View("Contact", GetLayoutForPos(), model);
        }

        // POST: /Home/Contact
        [HttpPost]
        public ActionResult Contact(ContactUsModel model)
        {
            if (ModelState.IsValid)
            {
                model.IsSent = true;
            }
            return View("Contact", GetLayoutForPos(), model);
        }
    }
}
