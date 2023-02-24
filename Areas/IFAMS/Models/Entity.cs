
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Web.Helpers;
namespace IEM.Areas.IFAMS.Models
{
    public class Entity
    {

    }
    public class TransferMakerModel
    {

        public int GLId { get; set; }
        public string GLCode { get; set; }
        public int _gid { get; set; }
        public string _asset_id { get; set; }
        public string asset_clearence_gl { get; set; }
        public string _new_asset_id { get; set; }
        public string _toa_number { get; set; }
        public string _tfr_date { get; set; }
        public decimal _no_records { get; set; }
        public string _upld_fname { get; set; }
        public string SheetName { get; set; }
        public string _assetdet_gid { get; set; }
        public string _AssetCode { get; set; }
        public string _AssetCatCode { get; set; }
        public string _loc_from { get; set; }
        public string _loc_to { get; set; }
        public string _AssetDesp { get; set; }
        public decimal _tfr_value { get; set; }
        public string _tfr_status { get; set; }
        public DateTime _process_date { get; set; }
        public string _jobcode { get; set; }
        public string _upld_flag { get; set; }
        public DateTime _upld_date { get; set; }
        public DateTime _value_date { get; set; }
        public int _maker_id { get; set; }
        public DateTime _maker_date { get; set; }
        public int _checker_id { get; set; }
        public DateTime _checker_date { get; set; }
        public int _toahead_gid { get; set; }
        public string _asset_value { get; set; }
        public string _new_assetdetgid { get; set; }
        public string _tfr_reason { get; set; }
        public string _rectif_amount { get; set; }
        public string _fccc_gid { get; set; }
        public string _fccc { get; set; }
        public string _barcode { get; set; }
        public string _Groupid { get; set; }
        public string _Ponumber { get; set; }
        public string _attach_file { get; set; }
        public string _attach_date { get; set; }
        public string _attach_desc { get; set; }
        public string _assetowner { get; set; }
        public string _change_date { get; set; }
        public string _change_assetcode { get; set; }
        public string _changestatus { get; set; }
        public string _cap_date { get; set; }
        public string _dep_rate { get; set; }
        public string _lease_startdate { get; set; }
        public string _lease_enddate { get; set; }
        public string _inw_no { get; set; }
        public decimal rectiAmt { get; set; }

        public int raiserId { get; set; }
        public string raiserCode { get; set; }
        public string raiserName { get; set; }
        public string raiserBranchCode { get; set; }
        public string raiserBranchName { get; set; }
        public int receiverbranchid { get; set; }

        public string receiverId { get; set; }
        public string receiverCode { get; set; }
        public string receiverName { get; set; }
        public string receiverBranchCode { get; set; }
        public string receiverBranchName { get; set; }
        public string actionRemarks { get; set; }
        public string commandType { get; set; }


        public List<TransferMakerModel> TModel { get; set; }

        public List<Attachment> _attach_list { get; set; }
        public List<TransferMakerModel> auditTrailLst { get; set; }
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
        public int queue_gid { get; set; }

        public List<FcccMaster> fcccList { get; set; }
        public int _fcccgid { get; set; }
        public string _fccc_code { get; set; }
        public string loged_by { get; set; }
        public string loged_date { get; set; }
        public string loged_empname { get; set; }
        public string loged_empcode { get; set; }
        public int loged_empgid { get; set; }

        public string depfrm { get; set; }
        public string depto { get; set; }
        public int hsnid { get; set; }
        public List<TransferMakerModel> gstlist { get; set; }

        public int hsn_gid { get; set; }
        public string hsn_code { get; set; }
        public string hsn_description { get; set; }
        public string taxsubtype { get; set; }
        public decimal taxrate { get; set; }
        public decimal amount { get; set; }
        public decimal taxamount { get; set; }
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

    public class FcccMaster
    {
        public string fcccCode { get; set; }
        public string fcccGid { get; set; }
        public string fcccName { get; set; }
    }

    public class SheetData
    {
        public int SheetId { get; set; }
        public string SheetName { get; set; }
    }

    public class WriteOffModel
    {
        public int _gid { get; set; }
        public string _asset_id { get; set; }
        public string _new_asset_id { get; set; }
        public string _woa_number { get; set; }
        public string _woa_date { get; set; }
        public decimal _no_records { get; set; }
        public string _upld_fname { get; set; }
        public string SheetName { get; set; }
        public string _assetdet_gid { get; set; }
        public string _AssetCode { get; set; }
        public string _AssetDesp { get; set; }
        public decimal _wdv_value { get; set; }
        public string _woa_status { get; set; }
        public DateTime _process_date { get; set; }
        public string _jobcode { get; set; }
        public string _upld_flag { get; set; }
        public DateTime _upld_date { get; set; }
        public DateTime _value_date { get; set; }
        public int _maker_id { get; set; }
        public DateTime _maker_date { get; set; }
        public int _checker_id { get; set; }
        public DateTime _checker_date { get; set; }
        public int _woahead_gid { get; set; }
        public string _asset_value { get; set; }
        public string _woa_reason { get; set; }
        public string _rectif_amount { get; set; }
        public string _loc { get; set; }
        public string _AssetCatCode { get; set; }
        public string _fccc { get; set; }
        public string _inw_no { get; set; }
        public string commandType { get; set; }
        public string actionremarks { get; set; }
        public int queue_gid { get; set; }

        public List<WriteOffModel> WModel { get; set; }

        public string _attach_file { get; set; }
        public string _attach_date { get; set; }
        public string _attach_desc { get; set; }
        public List<Attachment> _attach_list { get; set; }
        public List<WriteOffModel> auditTrailLst { get; set; }
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
        public string loged_by { get; set; }
        public string loged_date { get; set; }
        public string loged_empname { get; set; }
        public string loged_empcode { get; set; }
        public int loged_empgid { get; set; }

    }

    public class BarcodeGenerationModel
    {
        public int _gid { get; set; }
        public string _asset_id { get; set; }
        public string _new_asset_id { get; set; }
        public string _courier_name { get; set; }
        public string _despatch_date { get; set; }
        public string _awb_no { get; set; }
        public string _assetdet_gid { get; set; }
        public string _assetCode { get; set; }
        public string _loc { get; set; }
        public string _assetDesp { get; set; }
        public string _source_id { get; set; }
        public string _source_name { get; set; }
        public string _cap_date { get; set; }
        public string _barcode_status { get; set; }
        public string _asset_type { get; set; }
        public string _fccc { get; set; }
        public string _barcode { get; set; }
        public string _loc_gid { get; set; }
        public string _assetcode_gid { get; set; }
        public string _ecf_no { get; set; }
        public string _inv_no { get; set; }
        public string _asset_sno { get; set; }
        public string _ecf_gid { get; set; }
        public string _invoice_gid { get; set; }
        public string _invoice_amount { get; set; }
        public string _asset_category { get; set; }
        public string _gl_code { get; set; }
        public string _qty { get; set; }
        public string _rate { get; set; }
        public string _amount { get; set; }
        public string _tran_type { get; set; }
        public string _shipment_type { get; set; }
        public string _satus { get; set; }
        public string _brname { get; set; }
        public string _comname { get; set; }
        

        public List<BarcodeGenerationModel> BModel { get; set; }
    }

    public class GroupModel
    {
        public int _gid { get; set; }
        public string _asset_id { get; set; }
        public string _assetdet_gid { get; set; }
        public string _loc_gid { get; set; }
        public string _loc { get; set; }
        public string _asset_cat { get; set; }
        public string _asset_subcat { get; set; }
        public string _asset_subcat_name { get; set; }
        public string _is5K { get; set; }
        public decimal _asset_value { get; set; }
        public string _qty { get; set; }
        public int _maker_id { get; set; }
        public DateTime _maker_date { get; set; }
        public int _checker_id { get; set; }
        public DateTime _checker_date { get; set; }
        public string _assetcode_gid { get; set; }
        public string _cap_date { get; set; }
        public List<GroupModel> GModel { get; set; }
        public List<GroupModel> GModelView { get; set; }
        public string _group_gid { get; set; }
        public string _group_id { get; set; }
        public string _asset_status { get; set; }
        public string _asset_count { get; set; }

        public List<GroupModel> _auditTrailLst { get; set; }
        public int _employee_gid { get; set; }
        public string _employee_code { get; set; }
        public string _employee_name { get; set; }
        public string _action_status { get; set; }
        public string _action_date { get; set; }
        public string _action_remarks { get; set; }
        public string _number { get; set; }
        public string _ref_number { get; set; }
        public string _ref_flag { get; set; }
        public string _approval_stage { get; set; }
        public string _queue_from { get; set; }
        public string _queue_to { get; set; }
        public string _queue_to_type { get; set; }
        public int gid { get; set; }
    }

    public class ChangeDepreciationRate
    {
        public string SheetName { get; set; }
        public int _gid { get; set; }
        public string _asset_id { get; set; }
        public string _dep_value { get; set; }
        public List<ChangeDepreciationRate> _ChgDepRate_list { get; set; }
        public string _inw_no { get; set; }
    }

    public class SaleInvoice
    {
        public string _sale_no { get; set; }
        public int _sale_gid { get; set; }
        public string _sale_amt { get; set; }
        public string _sale_date { get; set; }
        public string _invoice_no { get; set; }
        public string _invoice_date { get; set; }
        public string _vat_amt { get; set; }
        public string _vendor_name { get; set; }
        public string _vendor_address { get; set; }
        public string _customer_address { get; set; }
        public string _customer_name { get; set; }
        public string _Gstin_number { get; set; }
        public string _state_code { get; set; }
        public string _state_name { get; set; }
        public string _particulars { get; set; }
        public string _code { get; set; }
        public string _vatpercentage { get; set; }
        public string _qty { get; set; }
        public decimal total { get; set; }
        public string _FiccGstin { get; set; }
        public string _Ficc_Branchaddress { get; set; }
        public string _Ficc_Location { get; set; }
        public string _hsn_code { get; set; }
        public decimal _CGstamt { get; set; }
        public decimal _SGstamt { get; set; }
        public decimal _IGstamt { get; set; }
        public decimal _CGstrat { get; set; }
        public decimal _SGstrat { get; set; }
        public decimal _IGstrat { get; set; }
        public decimal _Cessamt { get; set; }
        public decimal _Cessrat { get; set; }
        public decimal _Ccessamt { get; set; }
        public decimal _Ccessrat { get; set; }
        public decimal _Scessamt { get; set; }
        public decimal _Scessrat { get; set; }
        public decimal _Icessamt { get; set; }
        public decimal _Icessrat { get; set; }
        public decimal _taxTotal { get; set; }
        public decimal _taxRateTotal { get; set; }
        public decimal _cessTotal { get; set; }
        public decimal _totalCessrate { get; set; }
        public List<SaleInvoice> _SaleInvoice_list { get; set; }
        public List<SaleInvoice> _SaleInvoice_details_list { get; set; }
        public List<SaleInvoice> _FICC_details { get; set; }
        public List<SaleInvoice> _InvTAX_details { get; set; }
        public List<SaleInvoice> _CessTAX_details { get; set; }
        public decimal tot_taxableamount { get; set; }
        public string _supplier_statename { get; set; }
        public string customer_city { get; set; }
        public string customer_pincode { get; set; }
        public string customer_district { get; set; }
    }

    public class Depreciation
    {
        public string _asset_id { get; set; }
        public string _date { get; set; }
        public int _gid { get; set; }
        public string _group_id { get; set; }
        public string _group_gid { get; set; }
        public decimal _asset_value { get; set; }
        public string _asset_code { get; set; }
        public decimal _dep_rate { get; set; }
        public decimal _assetdet_dep_rate { get; set; }
        public decimal _ast_dep_rate { get; set; }
        public string _asset_glcode { get; set; }
        public string _asset_status { get; set; }
        public string _AssetDesp { get; set; }
        public decimal _dep_amt { get; set; }
        public decimal _dep_amtpre { get; set; }
        public string _cap_date { get; set; }
        public string _dep_month { get; set; }
        public string _dep_full { get; set; }
        public string _po_number { get; set; }
        public string _loc { get; set; }
        public string _recon_ref { get; set; }
        public string _current_month { get; set; }
        public string _dep_type { get; set; }
        public string _is_5k { get; set; }
        public int _no_of_days { get; set; }
        public string _Till_date { get; set; }
        public string _Trf_date { get; set; }
        public string _Trf_id { get; set; }
        public decimal _Trf_value { get; set; }
        public string _Trf_status { get; set; }
        public string _Sale_date { get; set; }
        public string _Wrt_date { get; set; }
        public string _Imp_date { get; set; }
        public string _Sale_status { get; set; }
        public string _leasedate_start { get; set; }
        public string _branch_leasedate_start { get; set; }
        public string _branch_leasedate_end { get; set; }
        public string _leasedate_end { get; set; }
        public string _branchdate_start { get; set; }
        public string _branch { get; set; }
        public string _ratio { get; set; }
        public string _Split_date { get; set; }
        public string[] _bs { get; set; }
        public string[] _cc { get; set; }
        public int _qty { get; set; }
        public List<Depreciation> _Dep_list { get; set; }
        public List<Depreciation> _Dep_listHeld { get; set; }
    }
    public class CWIPModel
    {
        public string cwip_acc_depr { get; set; }
        public int cwip_asset_gid { get; set; }
        public string cwip_asset_id { get; set; }
        public string cwip_asset_code { get; set; }
        public string cwip_asset_descp { get; set; }
        public string cwip_asset_glcode { get; set; }
        public string cwip_asset_depglcode { get; set; }
        public string cwip_asset_resglcode { get; set; }
        public int cwip_asset_cat_gid { get; set; }
        public string cwip_asset_cat { get; set; }
        public string cwip_asset_status { get; set; }
        public decimal cwip_asset_value { get; set; }
        public decimal cwip_wd_value { get; set; }
        public decimal cwip_transfer_value { get; set; }
        public int cwip_assetdetails_gid { get; set; }
        public int cwip_branch_gid { get; set; }
        public string cwip_bs { get; set; }
        public string cwip_capitalisation_date { get; set; }
        public string cwip_sale_date { get; set; }
        public string cwip_transfer_date { get; set; }
        public string cwip_cbf_number { get; set; }
        public string cwip_cc { get; set; }
        public string cwip_checker_date { get; set; }
        public int cwip_checkerid { get; set; }
        public string cwip_transfer_from { get; set; }
        public string cwip_department { get; set; }
        public decimal cwip_depr_rate { get; set; }
        public string cwip_description { get; set; }
        public string cwip_ecf_number { get; set; }
        public int cwip_gid { get; set; }
        public string cwip_group_id { get; set; }
        public string cwip_inward_date { get; set; }
        public string cwip_maker_date { get; set; }
        public int cwip_makerid { get; set; }
        public string cwip_dep_type { get; set; }
        public string cwip_asset_type { get; set; }
        public string cwip_po_number { get; set; }
        public string cwip_quantity { get; set; }
        public string cwip_reference_date { get; set; }
        public string cwip_remark { get; set; }
        public string cwip_status { get; set; }
        public string cwip_supplier_name { get; set; }
        public int cwip_supplierheader_gid { get; set; }
        public string cwip_manft_name { get; set; }
        public string cwip_serial_number { get; set; }
        public string SheetName { get; set; }
        public string cwip_branch_code { get; set; }
        public string cwip_branch_start { get; set; }
        public string cwip_lea_start { get; set; }
        public string cwip_lea_end { get; set; }
        public List<CWIPModel> Model { get; set; }
    }

    public class CWIPModel1
    {
        public string ACF_NUMBER { get; set; }
        public string ASSET_BSCC { get; set; }
        public string ASSET_ID { get; set; }
        public string ASSET_VALUE { get; set; }
        public string ASSET_CODE { get; set; }
        public string BRANCH_CODE { get; set; }
        public string BRANCH_LAUNCH_DATE { get; set; }
        public string CAPITALISATION_DATE { get; set; }
        public string CBF_NUMBER { get; set; }
        public string DEPR_GL { get; set; }
        public string DEPR_PV_GL { get; set; }
        public string DEPR_RATE { get; set; }
        public string DEPR_TYPE { get; set; }
        public string DESCRIPTION { get; set; }
        public string GLCODE { get; set; }
        public string GROUP_ID { get; set; }
        public string INWARD_DATE { get; set; }
        public string LEASE_END_DATE { get; set; }
        public string LEASE_START_DATE { get; set; }
        public string PO_NUMBER { get; set; }
        public string QUANTITY { get; set; }
        public string REFERENCE_DATE { get; set; }
        public string SALE_DATE { get; set; }
        public string ASSET_STATUS { get; set; }
        public string TRANSFER_DATE { get; set; }
        public string DEPARTMENT { get; set; }
        public string TANGIBLE_STATUS { get; set; }
        public string TRANSFER_FROM { get; set; }
        public string TRANSFER_VALUE { get; set; }
        public string UPLOAD_BSCC { get; set; }
        public string VENDOR_NAME { get; set; }
        public string WD_VALUE { get; set; }

    }
    public class ECFDLDOCModel
    {
        public string ecf_no { get; set; }
        public string ecf_filename { get; set; }
        public string zip_file_name { get; set; }
        public string SheetName { get; set; }
        public List<ECFDLDOCModel> Model { get; set; }
    }
    public class TransferInvoice
    {
        public string _toa_no { get; set; }
        public int _toa_gid { get; set; }
        public string _toa_amt { get; set; }
        public string _toa_date { get; set; }
        public string _invoice_no { get; set; }
        public string _invoice_date { get; set; }
        public string _vat_amt { get; set; }
        public string _vendor_name { get; set; }
        public string _vendor_address { get; set; }
        public string _customer_address { get; set; }
        public string _customer_name { get; set; }
        public string _Gstin_number { get; set; }
        public string _state_code { get; set; }
        public string _state_name { get; set; }
        public string _particulars { get; set; }
        public string _code { get; set; }
        public string _vatpercentage { get; set; }
        public string _qty { get; set; }
        public decimal total { get; set; }
        public string _FiccGstin { get; set; }
        public string _Ficc_Branchaddress { get; set; }
        public string _Ficc_Location { get; set; }
        public string _hsn_code { get; set; }
        public decimal _CGstamt { get; set; }
        public decimal _SGstamt { get; set; }
        public decimal _IGstamt { get; set; }
        public decimal _CGstrat { get; set; }
        public decimal _SGstrat { get; set; }
        public decimal _IGstrat { get; set; }
        public decimal _Cessamt { get; set; }
        public decimal _Cessrat { get; set; }
        public decimal _Ccessamt { get; set; }
        public decimal _Ccessrat { get; set; }
        public decimal _Scessamt { get; set; }
        public decimal _Scessrat { get; set; }
        public decimal _Icessamt { get; set; }
        public decimal _Icessrat { get; set; }
        public decimal _taxTotal { get; set; }
        public decimal _taxRateTotal { get; set; }
        public decimal _cessTotal { get; set; }
        public decimal _totalCessrate { get; set; }
        public int recordcount { get; set; }
        public string toastatus { get; set; }
        public List<TransferInvoice> _TransferInvoice_list { get; set; }
        public List<TransferInvoice> _TransferInvoice_details_list { get; set; }
        public List<TransferInvoice> _FICC_details { get; set; }
        public List<TransferInvoice> _InvTAX_details { get; set; }
        public List<TransferInvoice> _CessTAX_details { get; set; }

        public string _tbranchtoname { get; set; }
        public string _tbranchToaddress { get; set; }
        public string _tbranchTogstin { get; set; }
        public string _transferFrom { get; set; }
        public string _tbranchFrom { get; set; }
        public string _tbranchFromgstin { get; set; }
        public string _supplier_statename { get; set; }
        public decimal tot_taxableamount { get; set; }
    }
}