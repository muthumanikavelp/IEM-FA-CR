
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Web.Helpers;

namespace IEM.Areas.IFAMS.Models
{
    public class AssetqtyEntity
    {
        
    }
    public class AssetqtyModel
    {
        public Int32 aqtySheetid { get; set; }
        public string aqtySheetname { get; set; }
        public Int32 asetSheetid { get; set; }
        public string asetSheetname { get; set; }

        public string assetdetid { get; set; }
        public int assetqty { get; set; }
        public int assetgid { get; set; }
      
    }

    public class AssetqtySheetData
    {
        public int aqtySheetid { get; set; }
        public string aqtySheetname { get; set; }
    }
}
