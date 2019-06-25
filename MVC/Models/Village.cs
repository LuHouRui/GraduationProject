using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class Village
    {
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string VillageId { get; set; }
        public string VillageName { get; set; }
    }
}