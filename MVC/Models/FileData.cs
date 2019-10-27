using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace MVC.Models
{
    public class FileData
    {
        [Key]
        public int ID { get; set; }
        [DisplayName("檔案名稱")]
        public string FileName { get; set; }
        [DisplayName("檔案路徑")]
        public string FilePath { get; set; }

    }
}