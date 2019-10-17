using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class Repository
    {
        private DataBaseContext db = new DataBaseContext();
        public void UpdateEvent(Schedule item) {
            db.Schedule.Add(item);
            db.SaveChanges();
        }

    }
}