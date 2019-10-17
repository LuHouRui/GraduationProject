using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class FormDataModel
    {
        public FormDataModel()
        {
            userData = new FormUserData();
            Cities = new List<City>();
            villages = new List<Village>();
        }
        public FormUserData userData { get; set; }
        public List<City> Cities { get; set; }
        public List<Village> villages { get; set; }
    } 
    
}