using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Controllers
{
    public class MainController : Controller
    {
        private DataBaseContext db = new DataBaseContext();
        private Repository repository = new Repository();
        // GET: Main
        public ActionResult Index()
        {
            MainViewModel model = new MainViewModel();
            model.Guiders = db.Guider.ToList();
            model.Scheulable_Event = db.Scheule.Where(x => x.guider == null).ToList();
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
        public JsonResult UpdateEvent(int id, DateTime t)
        {
            var status = false;
            try
            {
                var result = db.Scheule.Find(id);
                if (result != null)
                {
                    result.start = t;
                    result.end = t;
                }
                db.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }
            return new JsonResult { Data = new { status = status } };
        }

        [HttpPost]
        public Scheule GetScheule(string title){

            return null;
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