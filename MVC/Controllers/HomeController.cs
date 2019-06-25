using MVC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        DataBaseContext db;
        public HomeController()
        {
            db = new DataBaseContext();
        }
        public ActionResult Home()
        {
            return View();
        }
        public  ActionResult Login()
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
                //Ex.Views/Home/Home
                Session["account"] = account;

                Response.Redirect("~/Home/Index");
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
            return Redirect(Url.Content("~/Home/Login"));
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
                    villageList = db.Village.Where(x => x.CityId == data.city).ToList();
                ViewBag.CityList = cityList;
                ViewBag.VillageList = villageList;
                ViewBag.Msg = "密碼輸入錯誤";
                return View(data);
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
                    Response.Redirect("~/Home/Login");
                    return new EmptyResult();
                }
                catch
                {
                    ViewBag.Msg = "註冊失敗...";
                    return View(data);
                }
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
        //---------------------------------------------------------------------------------------------------------------------//
        //---------------------------------------------------------------------------------------------------------------------//
        //---------------------------------------------------------------------------------------------------------------------//
        //---------------------------------------------------------------------------------------------------------------------//
        public ActionResult Index()
        {
            var result = db.UserData.ToList();
            ViewBag.Message = TempData["Message"];
            return View(result);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(UserData data)
        {
            if (ModelState.IsValid)//如果資料驗證成功
            {
                db.UserData.Add(data);
                db.SaveChanges();
                TempData["Message"] = string.Format("使用者[{0}]建立成功", data.account);
                return RedirectToAction("Index");
            }
            ViewBag.Message = "資料有誤，請檢查";
            return View(data);
        }
        [HttpGet]
        public ActionResult Edit(string id)
        {
            var result = db.UserData.Find(id);
            if (result != null)
            {
                return View(result);
            }
            else
            {
                TempData["Message"] = "資料有誤，請重新操作";
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public ActionResult Edit(UserData data)
        {
            if (ModelState.IsValid)
            {
                var result = db.UserData.Find(data.UID);
                result.account = data.account;
                result.password = data.password;
                result.city = data.city;
                result.village = data.village;
                result.address = data.address;
                db.SaveChanges();
                TempData["Message"] = string.Format("使用者[{0}]資料修改成功", data.UID);
                return RedirectToAction("Index");
            }
            else
            {
                return View(data);
            }
        }
        [HttpPost]
        public ActionResult Delete(string id)
        {
            var result = db.UserData.Find(id);
            if(result != null)
            {
                db.UserData.Remove(result);
                db.SaveChanges();
                TempData["Message"] = string.Format("使用者[{0}]資料修改成功", id);
                return RedirectToAction("Index");
            }
            else
            {
                TempData["Message"] = "指定資料不存在，無法刪除，請重新操作";
                return RedirectToAction("Index");
            }
        }


    }
}