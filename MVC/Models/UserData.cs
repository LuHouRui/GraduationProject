using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class UserData
    {
        [Key]
        [Required]
        public string UID { get; set; }
        [Required]
        public string account { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public string village { get; set; }
        [Required]
        public string address { get; set; }
    }
}