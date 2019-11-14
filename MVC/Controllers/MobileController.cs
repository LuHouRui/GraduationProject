using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class MobileController : Controller
    {
        // GET: Mobile
        public ActionResult GetGuiderData()
        {
            return View();
        }
    }
}