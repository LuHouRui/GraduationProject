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
    public class GuiderController : Controller
    {
        private DataBaseContext db = new DataBaseContext();

        public ActionResult ResetPassWord(int? id)
        {
            var guider = db.Guider.Find(id);
            var phone = guider.Phone as string;
            var new_password = "";
            for (int i = phone.Length - 3; i < phone.Length; i++)
            {
                new_password += phone[i];
            }
            guider.Password = new_password;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Guider
        public ActionResult Index()
        {
            return View(db.Guider.ToList());
        }

        // GET: Guider/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guider guider = db.Guider.Find(id);
            if (guider == null)
            {
                return HttpNotFound();
            }
            return View(guider);
        }

        // GET: Guider/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Guider/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Number,Password,Name,Phone")] Guider guider)
        {
            if (ModelState.IsValid)
            {
                db.Guider.Add(guider);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(guider);
        }

        // GET: Guider/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guider guider = db.Guider.Find(id);
            if (guider == null)
            {
                return HttpNotFound();
            }
            return View(guider);
        }

        // POST: Guider/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Number,Password,Name,Phone")] Guider guider)
        {
            if (ModelState.IsValid)
            {
                db.Entry(guider).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(guider);
        }

        // GET: Guider/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Guider guider = db.Guider.Find(id);
            if (guider == null)
            {
                return HttpNotFound();
            }
            return View(guider);
        }

        // POST: Guider/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Guider guider = db.Guider.Find(id);
            db.Guider.Remove(guider);
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
