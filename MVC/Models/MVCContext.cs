﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class MVCContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public MVCContext() : base("name=MVCContext")
        {
        }

        public System.Data.Entity.DbSet<MVC.Models.Schedule> Scheules { get; set; }

        public System.Data.Entity.DbSet<MVC.Models.Village> Villages { get; set; }

        public System.Data.Entity.DbSet<MVC.Models.Guider> Guider { get; set; }

        public System.Data.Entity.DbSet<MVC.Models.Leave> Leaves { get; set; }
    }
}
