using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.IFAMS.Models
{
    public class IfamsSplitEntity
    {
    }
    public class AssetSplitModel
    {
        //Assetdetails
        public Int32 assetdetId { get; set; }
        public string assetdetDetid { get; set; }
        public string assetdetCode { get; set; }
        public string assetdetDescription { get; set; }
        public decimal assetdetAssetvalue { get; set; }
        public Int32 assetdetLocationcode { get; set; }
        public Int32 locationId { get; set; }
        public Decimal Tdpreciation { get; set; }

        public Decimal rectificationAmt { get; set; }
        
        //Assetcategory
        public string assetcategory { get; set; }
        public string assetsubcategory { get; set; }

        //depreciation
        public string assetdep { get; set; }



        //SplitMerge
        public Int32 assetsmassetId { get; set; }
        public Decimal assetsmassetVal { get; set; }
        public string assetsmNassetdetId { get; set; }
        public Decimal assetsmNassetVal { get; set; }
        public string assetsmEntryby { get; set; }
        public string assetsmEntrydate { get; set; }
        public string assetsmStatus { get; set; }


        //status
        public string statid { get; set; }

        public Decimal[] splitamont { get; set; }
        public Decimal splitamt { get; set; }
        public Decimal splitamt1 { get; set; }

        public List<AssetSplitModel> SplitModel {get; set;}


        public List<AssetSplitModel> auditTrailLst { get; set; }
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






        public List<Depreciation> _Dep_list { get; set; }

    }
}