using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class Repository
    {
        private DataBaseContext db = new DataBaseContext();
        public void UpdateEvent(Scheule item) {
            db.Scheule.Add(item);
            db.SaveChanges();
        }

    }
}