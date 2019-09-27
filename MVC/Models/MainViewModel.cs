using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class MainViewModel
    {
        public string Guider_Number { get; set; }
        public List<Guider> Guiders { get; set; }
        public List<Scheule> Scheules { get; set; }
        public List<Scheule> Scheulable_Event { get; set; }
    }
}