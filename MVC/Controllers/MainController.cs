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
            model.Schedulable_Event = db.Schedule.Where(x => (x.guider == null) || (x.guider == "")).ToList();
            if(id != null)
            {
                string Name = "";
                var guider = db.Guider.Where(x => x.Number == id).Single();
                Name = guider.Name;
                ViewBag.Name = Name;
            }
            return View(model);
        }

        [HttpPost]
        public JsonResult GetCalendarData(string id = null)
        {
            // Initialization.  
            JsonResult result = new JsonResult();
            if(id == "Index")
            {
                result = this.Json(null, JsonRequestBehavior.AllowGet);
            }
            try
            {
                // Loading.  
                //List<PublicHoliday> data = this.LoadDataOfDb();
                List<Schedule> schedules = db.Schedule.Where(x => x.guider == id).ToList();
                // Processing.  
                result = this.Json(schedules, JsonRequestBehavior.AllowGet);
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
        public Object UpdateEvent(int id, DateTime s,DateTime e,string guider=null)
        {
            var status = false;
            try
            {
                var schedule = db.Schedule.Find(id);
                if (schedule != null)
                {
                    schedule.start = s;
                    schedule.end = e;
                    schedule.guider = guider;
                }
                db.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }
            var result =  new { status = status };
            return result;
        }
        public Object AssignGuider(int id, string guider = null)
        {
            var status = false;
            try
            {
                var schedule = db.Schedule.Find(id);
                if (schedule != null)
                {
                    schedule.guider = guider;
                }
                db.SaveChanges();
                status = true;
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }
            var result = new { status = status };
            return result;
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------//
        //-----------------------------------------------------------------------------------------------------------------------------------------//
        //-----------------------------------------------------------------------------------------------------------------------------------------//
        //-----------------------------------------------------------------------------------------------------------------------------------------//
        //-----------------------------------------------------------------------------------------------------------------------------------------//
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection post)
        {
            string account = post["account"];
            string password = post["password"];
            var result = db.UserData.Where(x => x.account == account && x.password == password).ToList();

            //驗證密碼
            if (result.Capacity != 0)
            {
                //若登入成功，用Session紀錄帳號，並在需要登入驗證的View加上檢查Seesion的紀錄
                //Ex.Views/Main/Index
                Session["account"] = account;

                Response.Redirect("~/Main/Index");
                return new EmptyResult();
            }
            else
            {
                ViewBag.Msg = "登入失敗...";
                return View();
            }
        }
        public ActionResult Logout()
        {
            Session.RemoveAll();
            return Redirect(Url.Content("~/Main/Login"));
        }

        public ActionResult Register()
        {
            FormDataModel model = new FormDataModel();
            model.Cities = db.City.ToList();
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Register(FormUserData data)
        {
            FormDataModel model = new FormDataModel();
            if (string.IsNullOrWhiteSpace(data.password1) || data.password1 != data.password2)
            {
                model.Cities = db.City.ToList();
                if (!string.IsNullOrWhiteSpace(data.city))
                    model.villages = db.Village.Where(x => x.CityId == data.city).ToList();
                model.userData = data;
                ViewBag.Msg = "密碼輸入錯誤";
                return View(model);
            }
            else
            {
                try
                {
                    UserData user = new UserData();
                    user.account = data.account;
                    user.password = data.password1;
                    user.city = data.city;
                    user.village = data.village;
                    user.address = data.address;
                    db.UserData.Add(user);
                    db.SaveChanges();
                    Response.Redirect("~/Main/Login");
                    return new EmptyResult();
                }
                catch (Exception e)
                {
                    ViewBag.Msg = e;
                    return View(model);
                }
            }
        }

        [HttpPost]
        public ActionResult Village(string id = "")
        {
            List<Village> list = db.Village.Where(x => x.CityId == id).ToList();
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
        public bool AddUserData(FormUserData data)
        {
            try
            {
                UserData user = new UserData();
                user.account = data.account;
                user.password = data.password1;
                user.city = data.city;
                user.village = data.village;
                user.address = data.address;
                db.UserData.Add(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}