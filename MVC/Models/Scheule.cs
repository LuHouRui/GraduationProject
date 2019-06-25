using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class Scheule
    {
        [Key]
        [Required]
        public string id { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public string discription { get; set; }
        public string guider { get; set; }
    }
}