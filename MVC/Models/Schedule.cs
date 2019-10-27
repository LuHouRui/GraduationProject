using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class Schedule
    {
        [Key]
        [DisplayName("行程編號")]
        public int id { get; set; }
        [Required]
        [DisplayName("行程標題")]
        public string title { get; set; }
        [Required]
        [DisplayName("開始時間")]
        public DateTime start { get; set; }
        [Required]
        [DisplayName("結束時間")]
        public DateTime end { get; set; }
        [DisplayName("描述")]
        public string discription { get; set; }
        [DisplayName("領隊導遊")]
        public string guider { get; set; }
        [DisplayName("事件顏色")]
        public string color { get; set; }
        [DisplayName("日程表路徑")]
        public string filepath { get; set; }
    }
}