using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class Guider
    {
        [Key]
        public int id { get; set; }
        [Required]
        [DisplayName("編號")]
        public string Number { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("密碼")]
        public string Password { get; set; }
        [Required]
        [DisplayName("名字")]
        public string Name { get; set; }
        [Required]
        [DisplayName("電話")]
        public string Phone { get; set; }
    }
}