using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class FormUserData
    {
        [Key]
        [Required]
        public string account { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string password1 { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string password2 { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public string village { get; set; }
        public string address { get; set; }
    }
}