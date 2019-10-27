using MVC.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class FileDataController : Controller
    {
        private DataBaseContext db = new DataBaseContext();
        private Repository repository = new Repository();

        public FileResult GetReport()
        {
            string URL = @"D:\DcTenXen0621\Data\FileData\中山碩士推甄信封面.pdf";
            byte[] FileBytes = System.IO.File.ReadAllBytes(URL);
            return File(FileBytes,"application/pdf");
        }
        public ActionResult Download()
        {

            FileInfo fl = new FileInfo(@"C:\Users\Administrator\Downloads\SQLEXPRADV_x64_ENU.exe");
            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = fl.Name,
                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());

            var readStream = new FileStream(fl.FullName, FileMode.Open, FileAccess.Read);
            string contentType = MimeMapping.GetMimeMapping(fl.FullName);
            return File(readStream, contentType);
        }

        // GET: FileData
        public ActionResult Index()
        {
            foreach (string upload in Request.Files)
            {
                if (!HasFile(Request.Files[upload])) continue;
                string mimeType = Request.Files[upload].ContentType;
                Stream fileStream = Request.Files[upload].InputStream;
                string fileName = Path.GetFileName(Request.Files[upload].FileName);
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
                    db.FileData.Add(data);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    // Info  
                    Console.Write(ex);
                }
            }
            return View();
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
    }
}