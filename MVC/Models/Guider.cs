using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class Guider
    {
        [Key]
        public int id { get; set; }
        [DisplayName("編號")]
        public int Number { get; set; }
        [DisplayName("名字")]
        public string Name { get; set; }
        [DisplayName("電話")]
        public string Phone { get; set; }
    }
}