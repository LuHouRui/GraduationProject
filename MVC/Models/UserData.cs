using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class UserData
    {
        [Key]
        [DisplayName("編號")]
        public int UID { get; set; }
        [Required]
        [DisplayName("帳號")]
        public string account { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("密碼")]
        public string password { get; set; }
        [Required]
        [DisplayName("縣市")]
        public string city { get; set; }
        [Required]
        [DisplayName("鄉鎮")]
        public string village { get; set; }
        [Required]
        [DisplayName("地址")]
        public string address { get; set; }
    }
}