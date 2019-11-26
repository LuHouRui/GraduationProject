using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Controllers
{
    public class ScheduleController : Controller
    {
        private DataBaseContext db = new DataBaseContext();

        // GET: Scheule
        public async Task<ActionResult> Index(string searchString)
        {
            var result = from m in db.Schedule
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                result = result.Where(s => s.title.Contains(searchString));
            }

            return View(await result.ToListAsync());
        }

        // GET: Scheule/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule scheule = db.Schedule.Find(id);
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
        public ActionResult Create([Bind(Include = "id,title,start,end,discription,guider,color,filepath")] Schedule scheule)
        {
            if (ModelState.IsValid)
            {
                var title_check = db.Schedule.Where(x=>x.title == scheule.title).SingleOrDefault();
                //確認開始日期有無在結束日期之後
                if (scheule.start.CompareTo(scheule.end) > 0)
                {
                    ViewBag.Msg = "請確認結束日期是否在開始日期之前!!!";
                    return View(scheule);
                }
                //確認有無重複行程標題
                else if (title_check != null)
                {
                    ViewBag.Msg = "行程標題重複!! 請重新確認!!!";
                    return View(scheule);
                }
                else
                {
                    string fileName = null;
                    foreach (string upload in Request.Files)
                    {
                        if (!HasFile(Request.Files[upload])) continue;
                        string mimeType = Request.Files[upload].ContentType;
                        Stream fileStream = Request.Files[upload].InputStream;
                        fileName = Path.GetFileName(Request.Files[upload].FileName);
                        //int fileLength = Request.Files[upload].ContentLength;
                        //byte[] fileData = new byte[fileLength];
                        //fileStream.Read(fileData, 0, fileLength);

                        string path = @"D:\DcTenXen0621\Data\FileData\" + fileName;
                        System.IO.FileStream fi = new FileStream(path, FileMode.CreateNew);
                        CopyStream(fileStream, fi);
                        fi.Flush();
                        try
                        {
                            FileData data = new FileData();
                            data.FileName = fileName;
                            data.FilePath = path;
                            scheule.filepath = path;
                            scheule.guider = "";
                            db.Schedule.Add(scheule);
                            db.SaveChanges();
                            db.FileData.Add(data);
                            db.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            // Info  
                            Console.Write(ex);
                        }
                    }
                    if (fileName == null)
                    {
                        ViewBag.Msg = "請上傳行程之日程表!!!";
                        return View(scheule);
                    }
                    return RedirectToAction("Index");
                }
            }

            return View(scheule);
        }

        public static bool HasFile(HttpPostedFileBase file)
        {
            return (file != null && file.ContentLength > 0) ? true : false;
        }
        //分段寫入資料，避免過大檔案造成錯誤
        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[16 * 1024];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }

        // GET: Scheule/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule scheule = db.Schedule.Find(id);
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
        public ActionResult Edit([Bind(Include = "id,title,start,end,discription,guider,color,filepath")] Schedule schedule)
        {

            var title_check = db.Schedule.Where(x => x.title == schedule.title);
            if (ModelState.IsValid)
            { 
                //開始日期在結束日期之後
                if (schedule.start.CompareTo(schedule.end) > 0)
                {
                    ViewBag.Msg = "請確認結束日期是否在開始日期之前!!!";
                    return View(schedule);
                }
                else
                {
                    foreach (string upload in Request.Files)
                    {
                        if (HasFile(Request.Files[upload]))
                        {
                            string mimeType = Request.Files[upload].ContentType;
                            Stream fileStream = Request.Files[upload].InputStream;
                            string fileName = Path.GetFileName(Request.Files[upload].FileName);
                            //int fileLength = Request.Files[upload].ContentLength;
                            //byte[] fileData = new byte[fileLength];
                            //fileStream.Read(fileData, 0, fileLength);

                            string path = @"D:\DcTenXen0621\Data\FileData\" + fileName;
                            
                            try
                            {
                                Request.Files[upload].SaveAs(path);
                                FileData data = new FileData();
                                data.FileName = fileName;
                                data.FilePath = path;
                                schedule.filepath = path;
                                db.Entry(schedule).State = EntityState.Modified;
                                db.SaveChanges();
                                db.FileData.Add(data);
                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                // Info  
                                ViewBag.Msg = ex;
                                return View(schedule);
                            }
                        }
                        else
                        {
                            try
                            {
                                db.Entry(schedule).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                // Info  
                                ViewBag.Msg = ex;
                                return View(schedule);
                            }
                        }
                        
                    }
                    return RedirectToAction("Index");
                }

                
            }
            return View(schedule);
        }

        // GET: Scheule/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Schedule scheule = db.Schedule.Find(id);
            if (scheule == null)
            {
                return HttpNotFound();
            }
            return View(scheule);
        }

        // POST: Scheule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            Schedule scheule = db.Schedule.Find(id);
            db.Schedule.Remove(scheule);
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

        public FileResult GetReport(string path)
        {
            byte[] FileBytes = System.IO.File.ReadAllBytes(path);
            return File(FileBytes, "application/pdf");
        }
    }
}
