using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IEM.Areas.IFAMS.Models
{
    public class Ifams_Propertyx
    {

        public class ImpairmentsModel
        {
            //public int _gid { get; set; }
            //public string _asset_id { get; set; }
            //public string _new_asset_id { get; set; }
            //public string _woa_number { get; set; }
            //public string _woa_date { get; set; }
            //public decimal _no_records { get; set; }
            //public string _upld_fname { get; set; }
            //public string SheetName { get; set; }
            //public string _assetdet_gid { get; set; }
            //public string _AssetCode { get; set; }
            //public string _AssetDesp { get; set; }
            //public decimal _wdv_value { get; set; }
            //public string _woa_status { get; set; }
            //public DateTime _process_date { get; set; }
            //public string _jobcode { get; set; }
            //public string _upld_flag { get; set; }
            //public DateTime _upld_date { get; set; }
            //public DateTime _value_date { get; set; }
            //public int _maker_id { get; set; }
            //public DateTime _maker_date { get; set; }
            //public int _checker_id { get; set; }
            //public DateTime _checker_date { get; set; }
            //public int _woahead_gid { get; set; }
            //public string _asset_value { get; set; }
            //public string _woa_reason { get; set; }
            //public string _rectif_amount { get; set; }






            public string _attach_file { get; set; }
            public string _attach_date { get; set; }
            public string _attach_desc { get; set; }



            public int _gid { get; set; }
           

            public string _imp_number { get; set; }
            public int _no_records { get; set; }
            public string _imp_date { get; set; }
           

            public string ImpairSheetname { get; set; }
            public Int32 ImpairSheetid { get; set; }

            public string imrheader_inw_no { get; set; }
           
            public Int32 imrheader_loc_gid { get; set; }
            public string imrheader_mc_flag { get; set; }
            public string imrheader_upld_date { get; set; }
            public string imrheader_upld_fname { get; set; }
            public string imrheader_trans_date { get; set; }
            public string imrheader_value_date { get; set; }
            public string imrheader_maker_id { get; set; }
            public string imrheader_checker_id { get; set; }
            public Int32 imrheader__attachment_gid { get; set; }
            public string imrheader_isremoved { get; set; }
            public string _upld_fname { get; set; }


            public int impdet_gid { get; set; }
            public int impheader_gid { get; set; }
          
            public string _impasset_id { get; set; }
            public string impstatus { get; set; }
            public string impseqnumber { get; set; }
            public string imrdet_department { get; set; }
            public string imrdet_branch_id { get; set; }
            public decimal imrdet_salevalue { get; set; }
            public string imrdet_date { get; set; }
            public string _impdet_inwno { get; set; }

            /// <summary>
            /// Audit Trials
            /// </summary>
            public string empname_id { get; set; }
            public string date { get; set; }
            public string role { get; set; }
            public string status { get; set; }
            public string remarks { get; set; }

            public decimal _imp_recti_amt { get; set; }


            public List<Attachment> _attach_list { get; set; }

            public List<ImpairmentsModel> AuditImpModel { get; set; }

            public List<ImpairmentsModel> ImpModel { get; set; }

            //public List<ImpairmentsModel> _attach_list { get; set; }
        }
        public class ImpSheetData
        {
            public Int32 ImpairSheetid { get; set; }
            public string ImpairSheetname { get; set; }
        }
        public class Attachment
        {
            public string fileName { get; set; }
            public string attachedDate { get; set; }
            public string description { get; set; }
            public string attachGid { get; set; }
            public string employee_gid { get; set; }
            public string employee_name { get; set; }
            public string attachment_Gid { get; set; }
        }

    }
}