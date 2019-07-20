using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class Scheule
    {
        [Key]
        [Required]
        [DisplayName("行程編號")]
        public int id { get; set; }
        [Required]
        [DisplayName("行程標題")]
        public string title { get; set; }
        [DisplayName("開始時間")]
        public string start { get; set; }
        [DisplayName("結束時間")]
        public string end { get; set; }
        [Required]
        [DisplayName("描述")]
        public string discription { get; set; }
        [DisplayName("領隊導遊")]
        public string guider { get; set; }
    }
}