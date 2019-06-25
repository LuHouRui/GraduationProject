using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class Guider
    {
        [Key]
        public int id { get; set; }
        public int Number { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
    }
}