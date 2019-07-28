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
        public ActionResult Index(string id = null)
        {
            MainViewModel model = new MainViewModel();
            model.Guiders = db.Guider.ToList();
            model.Scheules = db.Scheule.Where(x => x.guider == id);
            return View(model);
        }
        public ActionResult GetCalendarData()
        {
            // Initialization.  
            JsonResult result = new JsonResult();

            try
            {
                // Loading.  
                //List<PublicHoliday> data = this.LoadDataOfDb();
                List<Scheule> scheules = db.Scheule.ToList();
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