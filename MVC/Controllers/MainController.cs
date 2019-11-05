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

                return RedirectToAction("Index");
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

        public ActionResult _ModalView(int? id )
        {
            MainViewModel model = new MainViewModel();

            var Event = db.Schedule.Find(id);//欲分派行程
            var Guiders = db.Guider.ToList();
            var Guiders_list = new List<string> { };
            foreach (var item in Guiders)
            {
                Guiders_list.Add(item.Number);
            }
            var Schedule_list = db.Schedule.ToList();
            foreach (var item in Schedule_list)
            {
                if (item.id != Event.id)
                {
                    DateTime start = item.start;
                    DateTime end = item.end;

                    //如果你開始時間比別人的晚或同天，且你的結束時間比別人的早 或 你結束時間比別人的開始時間晚又比別人的結束時間早
                    if ((DateTime.Compare(Event.start, start) >= 0 && DateTime.Compare(Event.start, end) < 0)
                        || (DateTime.Compare(Event.end, start) >= 0 && DateTime.Compare(Event.end, end) < 0))
                    {
                        Guiders_list.Remove(item.guider);
                    }
                }
            }
            var result = new List<Guider>();
            foreach (var item in Guiders_list)
            {
                var temp = db.Guider.Where(x => x.Number == item).Single();
                result.Add(temp);//適合之導遊清單
            }
            model.Guiders = result;
            model.Scheules = new List<Schedule>();
            model.Scheules.Add(Event);
            return View(model);
        }
        public ActionResult Assign(int id, string guider = null)
        {
            try
            {
                var schedule = db.Schedule.Find(id);
                if (schedule != null)
                {
                    schedule.guider = guider;
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                // Info 
                ViewBag.Msg = ex.ToString();
            }
            return RedirectToAction("Index");
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------//
        //------------------------------------------------------------Function---------------------------------------------------------------------//
        //-----------------------------------------------------------------------------------------------------------------------------------------//

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

        //檢測日期有無衝突
        public bool Check_Confilct(int id, string guider)
        {
            Schedule com_event = db.Schedule.Find(id);
            TimeSpan Com_Duration = com_event.end - com_event.start;
            List<Schedule> list = db.Schedule.Where(x => x.guider == guider).ToList();
            bool Is_Conflict = false;
            for (int i = 0; i < list.Count; i++)
            {
                DateTime start = list[i].start;
                DateTime end = list[i].end;

                //如果你開始時間比別人的晚或同天，且你的結束時間比別人的早 或 你結束時間比別人的開始時間晚又比別人的結束時間早
                if ((DateTime.Compare(com_event.start, start) >= 0 && DateTime.Compare(com_event.start, end) < 0)
                    || (DateTime.Compare(com_event.end, start) >= 0 && DateTime.Compare(com_event.end, end) < 0))
                {
                    if (id != list[i].id)
                    {
                        Is_Conflict = true;
                        break;
                    }
                }
            }
            return Is_Conflict;
        }

        public FileResult GetReport(int? id)
        {
            var result = db.Schedule.Find(id);
            byte[] FileBytes = System.IO.File.ReadAllBytes(result.filepath);
            return File(FileBytes, "application/pdf");
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------//
        //------------------------------------------------------------Ajax Call---------------------------------------------------------------------//
        //-----------------------------------------------------------------------------------------------------------------------------------------//

        [HttpPost]
        public JsonResult FindBestGuiders(int? id)
        {
            var Event = db.Schedule.Find(id);//欲分派行程
            var Guiders = db.Guider.ToList();
            var Guiders_list = new List<string> { } ;
            foreach (var item in Guiders)
            {
                Guiders_list.Add(item.Number);
            }
            var Schedule_list = db.Schedule.ToList();
            foreach (var item in Schedule_list)
            {
                if(item.id != Event.id)
                {
                    DateTime start = item.start;
                    DateTime end = item.end;

                    //如果你開始時間比別人的晚或同天，且你的結束時間比別人的早 或 你結束時間比別人的開始時間晚又比別人的結束時間早
                    if ((DateTime.Compare(Event.start, start) >= 0 && DateTime.Compare(Event.start, end) < 0)
                        || (DateTime.Compare(Event.end, start) >= 0 && DateTime.Compare(Event.end, end) < 0))
                    {
                        Guiders_list.Remove(item.guider);
                    }
                }
            }
            var result = new List<Guider>();
            foreach (var item in Guiders_list)
            {
                var temp = db.Guider.Where(x => x.Number == item).Single();
                result.Add(temp);//適合之導遊清單
            }
            
            // Return info.  
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCalendarData(string id = null)
        {
            // Initialization.  
            JsonResult result = new JsonResult();
            if (id == "Index")
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
        public JsonResult AssignGuider(int id, string guider = null)
        {
            try
            {
                if (Check_Confilct(id, guider))
                {
                    return Json(new { ok = false, message = "與其他行程衝突，請重新確認!" });
                }
                else
                {
                    var schedule = db.Schedule.Find(id);
                    if (schedule != null)
                    {
                        schedule.guider = guider;
                    }
                    db.SaveChanges();
                    return Json(new { ok = true, message = "分派行程成功!!" });
                }

            }
            catch (Exception ex)
            {
                // Info 
                return Json(new { ok = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult UpdateEvent(Schedule schedule)
        {
            try
            {
                if (Check_Confilct(schedule.id, schedule.guider))
                {
                    return Json(new { ok = false, message = "與其他行程衝突，請重新確認!" });
                }
                else
                {
                    var result = db.Schedule.Find(schedule.id);
                    if (result != null)
                    {
                        result.start = schedule.start;
                        result.end = schedule.end;
                        result.guider = schedule.guider;
                    }
                    db.SaveChanges();
                    return Json(new { ok = true, message = "更改行程成功!!" });
                }
            }
            catch (Exception ex)
            {
                // Info  
                return Json(new { ok = false, message = ex.Message });

            }
        }

        [HttpPost]
        public JsonResult CancelGuider(int id, string guider = null)
        {
            try
            {
                var schedule = db.Schedule.Find(id);
                if (schedule != null)
                {
                    schedule.guider = guider;
                }
                db.SaveChanges();
                return Json(new { ok = true, message = "已取消分派行程!!" });
            }
            catch (Exception ex)
            {
                // Info 
                return Json(new { ok = false, message = ex.Message });
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
    }
}