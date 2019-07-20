using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class PublicHoliday
    {
        [DisplayName("編號")]
        public int Sr { get; set; }
        [DisplayName("標題")]
        public string Title { get; set; }
        [DisplayName("描述")]
        public string Desc { get; set; }
        [DisplayName("開始時間")]
        public string Start_Date { get; set; }
        [DisplayName("結束時間")]
        public string End_Date { get; set; }
    }
}