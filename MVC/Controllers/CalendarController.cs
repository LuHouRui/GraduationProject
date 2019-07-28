using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Reflection;
using MVC.Models;

namespace MVC.Controllers
{
    public class CalendarController : Controller
    {
        private DataBaseContext db = new DataBaseContext();
        #region Index method  

        /// <summary>  
        /// GET: Home/Index method.  
        /// </summary>  
        /// <returns>Returns - index view page</returns>   
        public ActionResult Index()
        {
            // Info.  
            return this.View();
        }

        #endregion

        #region Get Calendar data method.  

        /// <summary>  
        /// GET: /Home/GetCalendarData  
        /// </summary>  
        /// <returns>Return data</returns>  
        public ActionResult GetCalendarData(string id)
        {
            // Initialization.  
            JsonResult result = new JsonResult();

            try
            {
                // Loading.  
                //List<PublicHoliday> data = this.LoadDataOfDb();
                List<Scheule> scheules = db.Scheule.Where(x=>x.guider == id).ToList();
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

        #endregion

        #region Helpers  

        #region Load Data  

        /// <summary>  
        /// Load data method.  
        /// </summary>  
        /// <returns>Returns - Data</returns>  
        private List<PublicHoliday> LoadData()
        {
            // Initialization.  
            List<PublicHoliday> lst = new List<PublicHoliday>();

            try
            {
                // Initialization.  
                string line = string.Empty;
                string srcFilePath = @"D:\DcTenXen0621\Data\School\0_0大學專題\專題\主管端網頁\GraduationProject\MVC\Content\files\PublicHoliday.txt";
                //獲得根目錄
                var rootPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
                //將srcFilePath與rootPath結合為Uri路徑
                var fullPath = Path.Combine(rootPath, srcFilePath);
                string filePath = new Uri(fullPath).LocalPath;
                StreamReader sr = new StreamReader(new FileStream(srcFilePath, FileMode.Open, FileAccess.Read));

                // Read file.  
                while ((line = sr.ReadLine()) != null)
                {
                    // Initialization.  
                    PublicHoliday infoObj = new PublicHoliday();
                    string[] info = line.Split(',');

                    // Setting.  
                    infoObj.Sr = Convert.ToInt32(info[0].ToString());
                    infoObj.Title = info[1].ToString();
                    infoObj.Desc = info[2].ToString();
                    infoObj.Start_Date = info[3].ToString();
                    infoObj.End_Date = info[4].ToString();

                    // Adding.  
                    lst.Add(infoObj);
                }

                // Closing.  
                sr.Dispose();
                sr.Close();
            }
            catch (Exception ex)
            {
                // info.  
                Console.Write(ex);
            }

            // info.  
            return lst;
        }

        /// <summary>  
        /// Load data method.  
        /// </summary>  
        /// <returns>Returns - Data</returns>  
        private List<PublicHoliday> LoadDataOfDb()
        {
            // Initialization.  
            List<PublicHoliday> lst = new List<PublicHoliday>();
            List<Scheule> Result = new List<Scheule>();
            try
            {
                Result = db.Scheule.ToList();

                // Read Data.
                foreach (var item in Result)
                {
                    PublicHoliday infoObj = new PublicHoliday();

                    // Setting.  
                    infoObj.Sr = item.id;
                    infoObj.Title = item.title;
                    infoObj.Desc = item.discription;
                    infoObj.Start_Date = item.start;
                    infoObj.End_Date = item.end;

                    // Adding.  
                    lst.Add(infoObj);
                }
                
            }
            catch (Exception ex)
            {
                // info.  
                Console.Write(ex);
            }

            // info.  
            return lst;
        }
        #endregion

        #endregion
    }
}