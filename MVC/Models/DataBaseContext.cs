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
                return "server = specialtopic.ddns.net;port =3306;database = mvc;user = aokisho;password = Love*0621";
            }
        }
        public DbSet<UserData> UserData { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<Village> Village { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Guider> Guider { get; set; }
        public DbSet<FileData> FileData { get; set; }
        public DbSet<Leave> Leaves { get;set; }
        public DataBaseContext() : base() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }
    }
}