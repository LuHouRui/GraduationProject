using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class City
    {
        [Key]
        public string CityId { get; set; }
        public string CityName { get; set; }
    }
}