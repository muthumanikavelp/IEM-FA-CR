using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IEM.Areas.FLEXIBUY.Models
{
    public class fb_Entitypr
    {

    }
    public class poRaising
    {
        public int poDetgid { get; set; }
        public string poDate { get; set; }
        public string poRefNo { get; set; }
        public string poVersion { get; set; }
        public string vendorName { get; set; }
        public string status { get; set; }
        public string poAmount { get; set; }
        public List<poRaising> objposummary { get; set; }
        //status dropdown
        public SelectList statusList { get; set; }
        public int statusgid { get; set; }

        public string statusname { get; set; }
        public string remarks { get; set; }
        public int poCancelGid { get; set; }
        public int poheadGid { get; set; }
        public int result { get; set; }
        public string viewCancel { get; set; }
        public int slno { get; set; }
        public int poclosureGid { get; set; }
        public string groupRes { get; set; }
        public string itType { get; set; }
        public string delmatview { get; set; }
    }
    public class cbfselection
    {
        public Int64 cbfheadgid { get; set; }
        public string cbfmode { get; set; }
        public string cbfno { get; set; }
        public string cbfdate { get; set; }
        public string cbfEnddate { get; set; }
        public string department { get; set; }
        public string finconBudget { get; set; }
        public decimal cbfamount { get; set; }
        public string regularlater { get; set; }
        public string projectowner { get; set; }
        public decimal deviationamount { get; set; }
        public string status { get; set; }
        public string description { get; set; }
        public List<cbfselection> cbfsummary { get; set; }
        public Int64 uncheckgid { get; set; }
        //dropdown
        public SelectList requestlist { get; set; }
        public int requestforgid { get; set; }
        public string requestforname { get; set; }
        public int slno { get; set; }
        public string result { get; set; }
    }
    public class cbfdetail
    {
        public Int64 cbfheadgid { get; set; }
        public Int64 cbfdetailgid { get; set; }
        public string cbfno { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public string productDesc { get; set; }
        public string uom { get; set; }
        public int quantity { get; set; }
        public decimal unitPrice { get; set; }
        public decimal totalAmount { get; set; }
        public string vendorName { get; set; }
        public int slno { get; set; }
        public decimal cbfheadAmount { get; set; }
        public List<cbfdetail> lstcbfdetail { get; set; }
    }
    public class poraiser
    {
        //PO header
        public int queue_gid { get; set; }
        public string viewfor { get; set; }
        public int poheadGid { get; set; }
        public int podetGid { get; set; }
        public int poCancelGid { get; set; }
        public int poClosureGid { get; set; }
        public string porefno { get; set; }
        public string podate { get; set; }
        public string poenddate { get; set; }
        public string raisedby { get; set; }
        public string projectmanager { get; set; }
        public string department { get; set; }
        public string vendorName { get; set; }
        public string vendorNote { get; set; }
        public string itType { get; set; }
        public decimal overTotal { get; set; }
        public decimal cbfheadAmount { get; set; }
        public decimal cbfdetailsQty { get; set; }
        //PO details
        public string prodservicegroup { get; set; }
        public string prodservicecode { get; set; }
        public string prodservicename { get; set; }
        public string prodservicedesc { get; set; }
        public int prodservicegid { get; set; }
        public int uomgid { get; set; }
        public int cbfdetailsgid { get; set; }
        public string units { get; set; }
        public string servicemonth { get; set; }
        public decimal quantity { get; set; }
        public decimal actQuantity { get; set; }
        public decimal unitprice { get; set; }
        public decimal discount { get; set; }
        public decimal baseamount { get; set; }
        public decimal devamount { get; set; }
        public decimal unutilizedAmount { get; set; }
        public decimal tax1 { get; set; }
        public decimal tax2 { get; set; }
        public decimal tax3 { get; set; }
        public decimal total { get; set; }
        public decimal grandTotal { get; set; }
        public int prodservgid { get; set; }
        public int vendorgid { get; set; }
        public int requestforgid { get; set; }
        public string actionName { get; set; }
        public decimal poRemaingAmount { get; set; }
        public string remarks { get; set; }
        public List<poraiser> objlist { get; set; }

        //PoTemplate Dropdown
        public string additionTemplate { get; set; }
        public SelectList templateList { get; set; }
        public int templateGid { get; set; }
        public string tempName { get; set; }
        public string templname { get; set; }

        //projectManager dropdown
        public SelectList projmanagerList { get; set; }
        public int projmanagergid { get; set; }
        public string projmanagername { get; set; }

        public string result { get; set; }
        public int count { get; set; }
        public string cbfno { get; set; }

        //audittrail
        public List<poraiser> auditTrailLst { get; set; }
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

    public class shipment
    {
        public string branchedtype { get; set; }
        public string shipmentName { get; set; }
        public string shipmentgid { get; set; }
        public string branchName { get; set; }
        public string address { get; set; }
        public string location { get; set; }
        public string branchCode { get; set; }
        public int branchgid { get; set; }
        public string inchargeID { get; set; }
        public decimal shippedqty { get; set; }
        public SelectList shipmentlist { get; set; }
        public int empgid { get; set; }
        public List<shipment> shiplist { get; set; }
        public int SrNo { get; set; }
        public string selectedValue { get; set; }
        public int cbfdetGid { get; set; }

        public string releasedqty { get; set; }
        public decimal balancedqty { get; set; }
        public decimal releasedqty1 { get; set; }
        public decimal totalqty { get; set; }

        public int shipGid { get; set; }
        public int sheetGid { get; set; }
        public string grndetGid { get; set; }
        public string poshipmentgid { get; set; }
    }
    public class SheetDatas
    {
        public int sheetId { get; set; }
        public string sheetName { get; set; }
    }
    public class BulkUpload
    {
        public string shipmentType { get; set; }
        public string branchCode { get; set; }
        public int quantity { get; set; }
        public string sheetName { get; set; }
        public List<BulkUpload> lListUpload { get; set; }
        public string slNo { get; set; }

        public int cbfSlNo { get; set; }
    }
    public class PoTemplate
    {
        public string templateName { get; set; }
        public string templateContent { get; set; }
        public string result { get; set; }
    }
    public class ViewBOQ
    {
        public string attachmentDate { get; set; }
        public string poFileName { get; set; }
        public string poDescription { get; set; }
        public List<ViewBOQ> boqlist { get; set; }
    }
    public class PoBulkClosure
    {
        public string poNo { get; set; }
        public string poDate { get; set; }
        public string sheetName { get; set; }
        public int sheetId { get; set; }
        public List<PoBulkClosure> lListBulkUpload { get; set; }
        public int slNo { get; set; }
        public string remarks { get; set; }
        public string result { get; set; }
        public string filename { get; set; }
    }

    //WO Raising
    public class WoSummary
    {
        public int woDetgid { get; set; }
        public string woDate { get; set; }
        public string woRefNo { get; set; }
        public string vendorName { get; set; }
        public string status { get; set; }
        public string woAmount { get; set; }
        public List<WoSummary> objWoSummary { get; set; }
        //status dropdown
        public SelectList statusList { get; set; }
        public int statusgid { get; set; }
        public string statusname { get; set; }
        public string remarks { get; set; }
        public int woCancelGid { get; set; }
        public int woheadGid { get; set; }
        public int result { get; set; }
        public string viewCancel { get; set; }
        public int slno { get; set; }
        public int woclosureGid { get; set; }
        public string grpRes { get; set; }
        public string delmatView { get; set; }
        public int Version { get; set; }
    }
    public class obfselection
    {
        public Int64 obfheadgid { get; set; }
        public string obfmode { get; set; }
        public string obfno { get; set; }
        public string obfdate { get; set; }
        public string obfEnddate { get; set; }
        public string department { get; set; }
        public string finconBudget { get; set; }
        public decimal obfamount { get; set; }
        public string projectowner { get; set; }
        //public decimal deviationamount { get; set; }
        public string status { get; set; }
        public string description { get; set; }
        public List<obfselection> obfsummary { get; set; }
        public Int64 uncheckgid { get; set; }
        //dropdown
        public SelectList requestlist { get; set; }
        public int requestforgid { get; set; }
        public string requestforname { get; set; }
        public int slno { get; set; }
        public string result { get; set; }
    }
    public class obfdetail
    {
        public Int64 obfheadgid { get; set; }
        public Int64 obfdetailgid { get; set; }
        public string obfno { get; set; }
        public string serviceCode { get; set; }
        public string serviceName { get; set; }
        public string serviceDesc { get; set; }
        public string uom { get; set; }
        //public int quantity { get; set; }
        //public decimal unitPrice { get; set; }
        public decimal obfheadAmount { get; set; }
        public string vendorName { get; set; }
        public int slno { get; set; }
        //public decimal cbfheadAmount { get; set; }
        public List<obfdetail> lstobfdetail { get; set; }
    }
    public class woraiser
    {
        public string viewfor { get; set; }
        public int woheadGid { get; set; }
        public int wodetGid { get; set; }
        public int woCancelGid { get; set; }
        public int woClosureGid { get; set; }
        public string worefno { get; set; }
        public string wodate { get; set; }
        public string woenddate { get; set; }
        public string raisedby { get; set; }
        public string projectmanager { get; set; }
        public string department { get; set; }
        public string vendorName { get; set; }
        public string vendorNote { get; set; }
        public string itType { get; set; }
        public decimal overTotal { get; set; }
        public decimal obfheadAmount { get; set; }
        public int podetailsgid { get; set; }
        public string templname { get; set; }
        //public decimal cbfdetailsQty { get; set; }
        //PO details
        //public string prodservicegroup { get; set; }
        public string serviceCode { get; set; }
        public string serviceName { get; set; }
        public string serviceDesc { get; set; }
        public int prodservicegid { get; set; }
        public int uomgid { get; set; }
        public int obfdetailsgid { get; set; }
        public string units { get; set; }
        public string serviceMonth { get; set; }
        public decimal percentage { get; set; }
        public decimal total { get; set; }
        public decimal grandTotal { get; set; }
        public int prodservgid { get; set; }
        public int vendorgid { get; set; }
        public int requestforgid { get; set; }
        public string actionName { get; set; }
        public string remarks { get; set; }
        public List<woraiser> obflist { get; set; }

        //PoTemplate Dropdown
        public string additionTemplate { get; set; }
        public SelectList templateList { get; set; }
        public int templateGid { get; set; }
        public string tempName { get; set; }

        //projectManager dropdown
        public SelectList projmanagerList { get; set; }
        public int projmanagergid { get; set; }
        public string projmanagername { get; set; }

        public string result { get; set; }
        public int count { get; set; }

        //frequency months
        public SelectList monthList { get; set; }
        public int monthId { get; set; }
        public string monthName { get; set; }

        public SelectList monthList1 { get; set; }
        public int monthId1 { get; set; }
        public string monthName1 { get; set; }

        //frequency months
        public SelectList freqList { get; set; }
        public int freqId { get; set; }
        public string freqName { get; set; }
        //AUDITRAILS
        public List<woraiser> auditTrailLst { get; set; }
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
        public int queue_gid { get; set; }
        public string queue_to { get; set; }
        public string queue_to_type { get; set; }
        public int gid { get; set; }
    }
    #region Modified
    public class GRNInward
    {
        public string grnDate { get; set; }
        public string grnRefNo { get; set; }
        public string poRefNo { get; set; }
        public string poDate { get; set; }
        public int poheadGid { get; set; }
        public string poVersion { get; set; }
        public string podetailsGid { get; set; }
        public int grnReleaseGid { get; set; }
        public string vendorName { get; set; }
        public string poAmount { get; set; }
        public string grnStatus { get; set; }
        public string grnDcNo { get; set; }
        public string grnInvoiceNo { get; set; }
        public string grnRemarks { get; set; }
        public int grnheadgid { get; set; }
        public int grndetgid { get; set; }
        public int slno { get; set; }
        public string grnType { get; set; }
        public string actionName { get; set; }
        public string branchCode { get; set; }
        public string branchname { get; set; }
        public string assetSlno { get; set; }
        public string assetDescr { get; set; }
        public string IsSerial { get; set; }
        public List<GRNInward> objInwardList { get; set; }
        public string poreleasedDate { get; set; }
        public SelectList statusList { get; set; }
        public int statusgid { get; set; }
        public string statusname { get; set; }

        public int poshipmentGid { get; set; }
        public string groupRes { get; set; }

        //ForPoDetails
        public string productGroup { get; set; }
        public string productName { get; set; }
        public string productCode { get; set; }
        public string uomcode { get; set; }
        public decimal quantity { get; set; }
        public decimal quantReleased { get; set; }
        public decimal quantReceived { get; set; }
        public string receivedDate { get; set; }
        public string raisedBy { get; set; }
        public string result { get; set; }
        public string viewfor { get; set; }
        public string chkResult { get; set; }
        public decimal quantBalanced { get; set; }
        public string Description { get; set; }
        public string UnitPrice { get; set; }
        public string PoDetailsAmount { get; set; }
        //audittrail
        public List<poraiser> auditTrailLst { get; set; }
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
        //attachment
        public string attach_id { get; set; }
        public string attachname { get; set; }
        public string attachdes { get; set; }
        public string attachdate { get; set; }
        public string selectedValue { get; set; }
        public List<Attachment> attachment { get; set; }
        public string attachment1 { get; set; }
        public List<Attachment> attachmentCbfdetails { get; set; }

    }
    #endregion
    public class WoBulkClosure
    {
        public string woNo { get; set; }
        public string woDate { get; set; }
        public string sheetName { get; set; }
        public int sheetId { get; set; }
        public List<WoBulkClosure> lListBulkUpload { get; set; }
        public int slNo { get; set; }
        public string remarks { get; set; }
        public string result { get; set; }
        public string filename { get; set; }
    }
    public class EmployeeSearch
    {
        //Project Owners
        public List<EmployeeSearch> lListEmp { get; set; }
        public int slNo { get; set; }
        public int employeeGid { get; set; }
        public string empCode { get; set; }
        public string empName { get; set; }
        public string empStatus { get; set; }
        public string empdesignation { get; set; }
        public string result { get; set; }
        public string rowNum { get; set; }
    }
}