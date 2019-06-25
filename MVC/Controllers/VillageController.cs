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
    public class VillageController : Controller
    {
        private DataBaseContext db = new DataBaseContext();

        // GET: Villages
        public ActionResult Index()
        {
            return View(db.Village.ToList());
        }

        // GET: Villages/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Village village = db.Village.Find(id);
            if (village == null)
            {
                return HttpNotFound();
            }
            return View(village);
        }

        // GET: Villages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Villages/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CityId,CityName,VillageId,VillageName")] Village village)
        {
            if (ModelState.IsValid)
            {
                db.Village.Add(village);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(village);
        }

        // GET: Villages/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Village village = db.Village.Find(id);
            if (village == null)
            {
                return HttpNotFound();
            }
            return View(village);
        }

        // POST: Villages/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CityId,CityName,VillageId,VillageName")] Village village)
        {
            if (ModelState.IsValid)
            {
                db.Entry(village).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(village);
        }

        // GET: Villages/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Village village = db.Village.Find(id);
            if (village == null)
            {
                return HttpNotFound();
            }
            return View(village);
        }

        // POST: Villages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Village village = db.Village.Find(id);
            db.Village.Remove(village);
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
