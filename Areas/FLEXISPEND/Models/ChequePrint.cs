using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.FLEXISPEND.Models
{
    public class ChequePrint
    {
        public string Pay { get; set; }
        public string Rupees { get; set; }
        public string Amount0 { get; set; }
        public string Amount1 { get; set; }
        public string Date { get; set; }
        public string PVNo { get; set; } 
    }
}