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
using Newtonsoft.Json;

namespace MVC.Controllers
{
    public class MainController : Controller
    {
        private DataBaseContext db = new DataBaseContext();
        private Repository repository = new Repository();
        // GET: Main
        public ActionResult Index(string id = null)
        {
            MainViewModel model = new MainViewModel();
            model.Guiders = db.Guider.ToList();
            model.Scheulable_Event = db.Scheule.Where(x => (x.guider == null) || (x.guider == "")).ToList();
            ViewBag.Number = id;
            return View(model);
        }

        public ActionResult Register()
        {
            List<City> cityList = db.City.ToList();
            List<Village> villageList = new List<Village>();
            ViewBag.CityList = cityList;
            ViewBag.VillageList = villageList;
            return View(new FormUserData());
        }
        [HttpPost]
        public ActionResult Register(FormUserData data)
        {

            if (string.IsNullOrWhiteSpace(data.password1) || data.password1 != data.password2)
            {
                List<City> cityList = db.City.ToList();
                List<Village> villageList = new List<Village>();
                if (!string.IsNullOrWhiteSpace(data.city))
                    villageList = db.Village.Where(x=>x.CityId == data.city).ToList();
                ViewBag.CityList = cityList;
                ViewBag.VillageList = villageList;
                ViewBag.Msg = "密碼輸入錯誤";
                return View(data);
            }
            else
            {
                if (AddUserData(data))
                {
                    Response.Redirect("~/Main/Login");
                    return new EmptyResult();
                }
                else
                {
                    ViewBag.Msg = "註冊失敗...";
                    return View(data);
                }
            }
        }

        public bool AddUserData(FormUserData data)
        {
            try
            {
                UserData user = new UserData();
                user.UID = data.UID;
                user.account = data.account;
                user.password = data.password1;
                user.city = data.city;
                user.village = data.village;
                user.address = data.address;
                db.UserData.Add(user);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost]
        public ActionResult Village(string id = "")
        {
            List<Village> list = db.Village.Where(x=> x.CityId == id).ToList();
            string result = "";
            if (list == null)
            {
                //讀取資料庫錯誤
                return Json(result);
            }
            else
            {
                result = JsonConvert.SerializeObject(list);
                return Json(result);
            }
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection post)
        {
            string account = post["account"];
            string password = post["password"];

            //驗證密碼
            var result = db.UserData.Where(x => x.account == account && x.password == password);
            if (result != null)
            {
                Response.Redirect("~/Main/Index");
                return new EmptyResult();
            }
            else
            {
                ViewBag.Msg = "登入失敗...";
                return View();
            }
        }

        [HttpPost]
        public JsonResult GetCalendarData(string id = null)
        {
            // Initialization.  
            JsonResult result = new JsonResult();
            if(id == "Index")
            {
                id = null;
            }
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
        [HttpPost]
        public JsonResult UpdateEvent(int id, DateTime t,string guider=null)
        {
            var status = false;
            try
            {
                var result = db.Scheule.Find(id);
                if (result != null)
                {
                    result.start = t;
                    result.end = t;
                    result.guider = guider;
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
    }
}