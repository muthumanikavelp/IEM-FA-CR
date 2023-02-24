using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IEM.Areas.EOW.Models
{
    public class ModeOfTravelEntity
    {
        public string ModeFlag { get; set; }
        public string ModeOfTravel { get; set; }
        public int ModeId { get; set; }
    }
    public class TravelClassEntity
    {
        
        public string TravelClass { get; set; }
        public int TravelId { get; set; }
        public int TravelModeId { get; set; }
        public string TravelMode { get; set; }
        List<SelectListItem> Rolelist = new List<SelectListItem>();
        public SelectList listofdata { get; set; }
    }
}