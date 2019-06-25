﻿using System;
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
    public class ScheuleController : Controller
    {
        private DataBaseContext db = new DataBaseContext();

        // GET: Scheule
        public async Task<ActionResult> Index(string searchString)
        {
            var result = from m in db.Scheule
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                result = result.Where(s => s.title.Contains(searchString));
            }

            return View(await result.ToListAsync());
        }

        // GET: Scheule/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scheule scheule = db.Scheule.Find(id);
            if (scheule == null)
            {
                return HttpNotFound();
            }
            return View(scheule);
        }

        // GET: Scheule/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Scheule/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,title,discription,guider")] Scheule scheule)
        {
            if (ModelState.IsValid)
            {
                db.Scheule.Add(scheule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(scheule);
        }

        // GET: Scheule/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scheule scheule = db.Scheule.Find(id);
            if (scheule == null)
            {
                return HttpNotFound();
            }
            return View(scheule);
        }

        // POST: Scheule/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,title,discription,guider")] Scheule scheule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scheule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(scheule);
        }

        // GET: Scheule/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Scheule scheule = db.Scheule.Find(id);
            if (scheule == null)
            {
                return HttpNotFound();
            }
            return View(scheule);
        }

        // POST: Scheule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Scheule scheule = db.Scheule.Find(id);
            db.Scheule.Remove(scheule);
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
