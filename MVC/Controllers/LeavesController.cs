using System;
using System.Collections.Generic;
using System.Data;

using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Controllers
{
    public class LeavesController : Controller
    {
        private DataBaseContext db = new DataBaseContext();

        // GET: Leaves
        public ActionResult Index()
        {
            return View(db.Leaves.ToList());
        }

        //檢測日期有無衝突
        public ActionResult Check_Confilct(int? id)
        {
            var com_event = db.Leaves.Find(id);
            TimeSpan Com_Duration = com_event.End - com_event.Start;
            List<Schedule> list = db.Schedule.Where(x => x.guider == com_event.Number).ToList();
            bool Is_Conflict = false;
            List<Schedule> Con_list = new List<Schedule>();
            for (int i = 0; i < list.Count; i++)
            {
                DateTime start = list[i].start;
                DateTime end = list[i].end;

                //如果你開始時間比別人的晚或同天，且你的結束時間比別人的早 或 你結束時間比別人的開始時間晚又比別人的結束時間早
                if ((DateTime.Compare(com_event.Start, start) >= 0 && DateTime.Compare(com_event.Start, end) < 0)
                    || (DateTime.Compare(com_event.End, start) >= 0 && DateTime.Compare(com_event.End, end) < 0))
                {
                    Con_list.Add(list[i]);
                    Is_Conflict = true;
                }
            }
            if (Is_Conflict)
            {
                TempData["Con_list"] = Con_list;
                return RedirectToAction("Confirm", new { id = com_event.id });
            }
            else
            {
                return RedirectToAction("Agree", new { id = com_event.id });
            }
        }

        public ActionResult Confirm(int? id)
        {

            var list = TempData["Con_list"] as List<Schedule>;
            ViewBag.Con_list = list;
            ViewData["Con_list"] = list;
            TempData["Con_list"] = list;
            var result = db.Leaves.Find(id);
            return View(result);
        }

        public ActionResult Agree(int? id)
        {
            var leave = db.Leaves.Find(id);
            Schedule Leave_Event = new Schedule();
            Leave_Event.discription = leave.Reason;
            Leave_Event.guider = leave.Number;
            Leave_Event.start = leave.Start;
            Leave_Event.end = leave.End;
            Leave_Event.title = "請假";
            Leave_Event.color = "Blue";
            List<Schedule> temp = TempData["Con_list"] as List<Schedule>;
            try
            {
                db.Leaves.Remove(leave);
                db.SaveChanges();
                db.Schedule.Add(Leave_Event);

                foreach (var item in temp)
                {
                    var schedule = db.Schedule.Find(item.id);
                    schedule.guider = "";
                }

                db.SaveChanges();
            }
            catch (Exception ex)
            {

                ViewBag.Msg = ex.ToString();
            }
            return RedirectToAction("Index","Main",new { id = leave.Number});
        }

        public ActionResult DisAgree(int? id)
        {
            var leave = db.Leaves.Find(id);
            db.Leaves.Remove(leave);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Leaves/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leave leave = db.Leaves.Find(id);
            if (leave == null)
            {
                return HttpNotFound();
            }
            return View(leave);
        }

        // GET: Leaves/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Leaves/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Number,Name,Start,End,Reason")] Leave leave)
        {
            if (ModelState.IsValid)
            {
                db.Leaves.Add(leave);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(leave);
        }

        // GET: Leaves/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leave leave = db.Leaves.Find(id);
            if (leave == null)
            {
                return HttpNotFound();
            }
            return View(leave);
        }

        // POST: Leaves/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Number,Name,Start,End,Reason")] Leave leave)
        {
            if (ModelState.IsValid)
            {
                db.Entry(leave).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(leave);
        }

        // GET: Leaves/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leave leave = db.Leaves.Find(id);
            if (leave == null)
            {
                return HttpNotFound();
            }
            return View(leave);
        }

        // POST: Leaves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Leave leave = db.Leaves.Find(id);
            db.Leaves.Remove(leave);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


    }
}
