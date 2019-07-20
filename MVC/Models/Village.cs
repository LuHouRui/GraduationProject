using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class Village
    {
        [DisplayName("縣市編號")]
        public string CityId { get; set; }
        [DisplayName("縣市編號")]
        public string CityName { get; set; }
        [DisplayName("鄉鎮編號")]
        public string VillageId { get; set; }
        [DisplayName("鄉鎮名稱")]
        public string VillageName { get; set; }
    }
}