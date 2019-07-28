using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.Models;

namespace MVC.Controllers
{
    public class MainController : Controller
    {
        private DataBaseContext db = new DataBaseContext();
        // GET: Main
        public ActionResult Index()
        {
            MainViewModel model = new MainViewModel();
            model.Guiders = db.Guider.ToList();
            model.Scheules = db.Scheule.Where(x => x.guider == null);
            return View(model);
        }
    }
}