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
        [HttpGet]
        public ActionResult Index()
        {
            MainViewModel model = new MainViewModel();
            model.Guiders = db.Guider.ToList();
            model.Scheules = db.Scheule.Where(x => x.guider == null);
            return View(model);
        }
        [HttpPost]
        public ActionResult Index(string id)
        {
            MainViewModel model = new MainViewModel();
            model.Guiders = db.Guider.ToList();
            model.Scheules = db.Scheule.Where(x => x.guider == null);
            return View(model);
        }
        [HttpGet]
        public JsonResult GetCalendarData()
        {
            // Initialization.  
            JsonResult result = new JsonResult();

            try
            {
                // Loading.  
                //List<PublicHoliday> data = this.LoadDataOfDb();
                List<Scheule> scheules = db.Scheule.Where(x => x.guider == null).ToList();
                // Processing.  
                result = this.Json(scheules, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }

            // Return info.  
            return result;
        }
        [HttpPost]
        public JsonResult GetCalendarData(string id = null)
        {
            // Initialization.  
            JsonResult result = new JsonResult();

            try
            {
                // Loading.  
                //List<PublicHoliday> data = this.LoadDataOfDb();
                List<Scheule> scheules = db.Scheule.Where(x => x.guider == id).ToList();
                // Processing.  
                result = this.Json(scheules, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }

            // Return info.  
            return result;
        }

    }
}