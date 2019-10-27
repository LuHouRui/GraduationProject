using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class Leave
    {
        [Key]
        public int id { get; set; }
        [DisplayName("編號")]
        public string Number { get; set; }
        [DisplayName("姓名")]
        public string Name { get; set; }
        [Required]
        [DisplayName("開始時間")]
        public DateTime Start { get; set; }
        [Required]
        [DisplayName("結束時間")]
        public DateTime End { get; set; }
        [Required]
        [DisplayName("請假理由")]
        public string Reason { get; set; }
    }
}