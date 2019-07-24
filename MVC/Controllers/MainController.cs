using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC.Models;
using System.Dynamic;
namespace MVC.Controllers
{
    public class MainController : Controller
    {
        private DataBaseContext db = new DataBaseContext();
        // GET: Main
        public ActionResult Index()
        {
            /*http://cs60811.pixnet.net/blog/post/345090569-%E5%A4%9A%E5%80%8Bmodel%E5%9C%A8%E5%90%8C%E4%B8%80%E5%80%8Bview%E4%B8%AD%E9%A1%AF%E7%A4%BA%E7%AF%84%E4%BE%8B*/
            dynamic models = new ExpandoObject();
            return View(db.Scheule.Where(x=> x.guider == null));
        }
    }
}