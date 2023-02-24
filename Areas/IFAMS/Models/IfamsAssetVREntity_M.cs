using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.IFAMS.Models
{
    public class IfamsAssetVREntity_M
    {
    }
    public class AssetVRModel
    {
        //Assetdetails
        public Int32 assetdetId { get; set; }
        public string assetdetDetid { get; set; }
        public string assetdetCode { get; set; }
        public string assetdetDescription { get; set; }
        public decimal assetdetAssetvalue { get; set; }
        public Int32 assetdetLocationcode { get; set; }
        public Decimal assetdetreduval { get; set; }
        public Decimal assetRectifAmt { get; set; }
        //public Int32 assetdetGroupid { get; set; }
        public string assetdetGroupid { get; set; }

        //reduce
        public Decimal assetreduval { get; set; }
        public string assetredudat { get; set; }
        public string assetrefno { get; set; }
        public string assetnewval { get; set; }
        public string assetreducedamt { get; set; }
        public string assetstatus { get; set; }

        //category
        public string assetcategory { get; set; }
        public string assetsubcategory { get; set; }

        //branch
        public string branchcode { get; set; }
        public string branchgid { get; set; }


        public string avrstatus { get; set; }

        public List<AssetVRModel> VRModel { get; set; }


        public List<AssetVRModel> auditTrailLst { get; set; }
        public int employee_gid { get; set; }
        public string employee_code { get; set; }
        public string employee_name { get; set; }
        public string action_status { get; set; }
        public string action_date { get; set; }
        public string action_remarks { get; set; }
        public string number { get; set; }
        public string ref_number { get; set; }
        public string ref_flag { get; set; }
        public string approval_stage { get; set; }
        public string queue_from { get; set; }
        public string queue_to { get; set; }
        public string queue_to_type { get; set; }
        public int gid { get; set; }
    }
}