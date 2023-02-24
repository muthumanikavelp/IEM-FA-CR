using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.IFAMS.Models
{
    public class IfamsAssetCCEntity
    {

    }

    public class AssetCCModel
    {
        public Int32 accSheetid { get; set; }
        public string accSheetname { get; set; }
        public string assetdetdetid { get; set; }
        public string assetdetcode { get; set; }
        public string assetdetnewcode { get; set; }

        public string assetdetLoc { get; set; }
        public string assetdetAsc { get; set; }
        public string[] assetdetArr { get; set; }
        public string assetdetbrnchid { get; set; }
        public string branchlegacy { get; set; }

        public List<AssetCCModel> CodeModel { get; set; }
    }
    public class AccSheetData
    {
        public Int32 accSheetid { get; set; }
        public string accSheetname { get; set; }
    }
}