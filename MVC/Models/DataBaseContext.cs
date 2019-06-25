using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class DataBaseContext:DbContext
    {
        private MySqlConnection con;
        public string ConnectionString
        {
            get
            {
                return "server = specialtopic.ddns.net;port =3306;database = mvc;user = root;password = root";
            }
        }
        public DbSet<UserData> UserData { get; set; }
        public DbSet<Scheule> Scheule { get; set; }
        public DbSet<Village> Village { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Guider> Guider { get; set; }
        public DataBaseContext() : base() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}