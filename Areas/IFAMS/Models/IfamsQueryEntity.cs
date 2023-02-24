using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace IEM.Areas.IFAMS.Models
{
    public class IfamsQueryEntity
    {
        public string barcodedetials_bar_no { get; set; }
        public int barcodedetails_gid { get; set; }
        public int assetcategory_gid { get; set; }
        public string assetcategory_name { get; set; }
        public string assetdetails_assetdet_id { get; set; }
        public int assetsubcategory_gid { get; set; }
        public string assetsubcategory_name { get; set; }
        public string assetdetails_cap_date { get; set; }
        public string ecf_no { get; set; }
        public int poheader_gid { get; set; }
        public string poheader_pono { get; set; }
        public int cbfheader_gid { get; set; }
        public string cbfheader_cbfno { get; set; }
        public decimal assetdetails_assetdet_value { get; set; }
        public int branch_gid { get; set; }
        public string branch_legacy_code { get; set; }
        public string invoice_no { get; set; }
        public string excelvalidate_name { get; set; }
        public SelectList Assetcategory{ get; set; }
        public SelectList Assetsubcategory{ get; set; }
        public SelectList AssetStatusName { get; set; }
        public SelectList GetLocation { get; set; }  


    }
    public class Assetcategory
    {
        public int assetcatid { get; set; }
        public string assetcatname { get; set; }
    }
    public class Assetsubcategory
    {
        public int assetsubcatid { get; set; }
        public string assetsubcatname { get; set; }
    }
    public class AssetStatusName
    {
        public int assetstatusid { get; set; }
        public string assetstatusname { get; set; }
    }
    public class GetLocation
    {
        public int BrnachId { get; set; }
        public string BranchNmae { get; set; }
    }
}