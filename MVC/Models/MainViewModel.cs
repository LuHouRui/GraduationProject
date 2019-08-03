using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC.Models
{
    public class MainViewModel
    {
        public IEnumerable<Guider> Guiders { get; set; }
        public IEnumerable<Scheule> Scheules { get; set; }
    }
}