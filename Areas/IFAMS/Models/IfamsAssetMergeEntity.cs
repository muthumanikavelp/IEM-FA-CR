using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.IFAMS.Models
{
    //public class IfamsAssetMergeEntity
    //{
    //    public string AssetId { get; set; }

    //}
    public class AssetParentModel
    {
        public int AssetDetGid { get; set; }
        public string AssetId { get; set; }
        public string AssetSubCode { get; set; }
        public string AssetDes { get; set; }
        public string AssetValue { get; set; }
        public double MAssetValue { get; set; }
        public string AssetCatName { get; set; }
        public string Location { get; set; }
        public string AssetCode { get; set; }
        public string status { get; set; }
        public string NewAssetId { get; set; }
        public string objidm { get; set; }
        public string Assetgrpid { get; set; }

        public string AssetIDNew { get; set; }
        public string BranchNew { get; set; }
        public string LocationNew { get; set; }

        public string Physicalid { get; set; }
        public string assetcap { get; set; }

        public decimal rectifAmt { get; set; } 
        public List<AssetParentModel> AssetParentList { get; set; }
        public List<AssetParentModel> AssetChildList { get; set; }
        public List<AssetParentModel> AssetParentSummary { get; set; }

        public List<AssetParentModel> Merged { get; set; }
        public List<AssetParentModel> MergedNew { get; set; }

        public string AssetType { get; set; }
      //  public string [] CheckedIds =new string[64];

        public string[] CheckedIds { get; set; }
        public string[] getIds { get; set; }
        public string[] getassetsubcode { get; set; }

        public List<string[]> Check { get; set; }

        public string command { get; set; }


        public string empname_id { get; set; }
        public string date { get; set; }
        public string role { get; set; }
        public string Assetremarks { get; set; }
       // public string status { get; set; }


    }
    
    public class Checklist
    {
        public string AssetDetGid { get; set; }
    }




   
}