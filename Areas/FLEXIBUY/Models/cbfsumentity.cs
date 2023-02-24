using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace IEM.Areas.FLEXIBUY.Models
{
    public class CbfSumEntity
    {
        public List<partransfer> lstparSum { get; set; }
        // public List<pardetailtransfer> lstpardetilsSum { get; set; }
        public List<CbfSumEntity> cbflist { get; set; }
        public List<CbfSummarymodel> cbfSum { get; set; }
        public List<CbfRaiseHeader> cbfRasie { get; set; }
        public List<CbfPrHeader> cbfPrheader { get; set; }
        public List<CbfParHeader> cbfParheader { get; set; }
        public List<CbfParDetails> cbfPardetailslst { get; set; }
        public List<CbfPrDetails> cbfPrdt { get; set; }
        public CbfRaiseHeader cbdRasieobj { get; set; }
        public CbfPrHeader cbfPrheaderobj { get; set; }
        public CbfParHeader cbfParHeaderObj { get; set; }
        public CbfPrDetails cbfPrdtobj { get; set; }
        public CbfDetails cbfDetailsobj { get; set; }
        public SelectList branchCode1 { get; set; }
        public SelectList projectOwner1 { get; set; }
        public SelectList requestFor { get; set; }
        public List<CbfPrDetails> cbfPrdetarils { get; set; }
        public List<CbfDetails> cbfDetails { get; set; }
        public DataTable dtTable { get; set; }
        public string amoun { get; set; }
        public List<FcccMaster> searchFccc { get; set; }
        public CbfRaiseHeader raiser { get; set; }
        public string filesToBeUploaded { get; set; }
        public List<Attachment> attachment { get; set; }
        public string attachment1 { get; set; }
        public string attachmentDate { get; set; }
        public string des { get; set; }
        public SelectList productGroupList { get; set; }
        public SelectList uomList { get; set; }
        public SelectList statuslist { get; set; }
        public string selectedValue { get; set; }
        public string selectedValue1 { get; set; }
        public string cbfbranchName { get; set; }
        public string cbfGid { get; set; }
        public string cbfref { get; set; }
        public string Budgeted { get; set; }
        public string Attachment_date { get; set; }
        //-------Checker-----//
        public List<CbfCheckerSummary> cbfCheckersumamry { get; set; }
        public List<CbfCheckerSummary> GetCbfClosureSummary { get; set; }
        public CbfCheckerHeader cbfCheckerHeader { get; set; }
        public List<CbfCheckerDetails> cbfCheckerDetails { get; set; }
        public List<Attachment> attachmentCbfdetails { get; set; }
        public SelectList Supervisor { get; set; }
        public RaiserTicket Raiserticket { get; set; }
        public List<RaiserTicket> RaiserList { get; set; }
        public Int32 statusgid { get; set; }
        public string statusname { get; set; }
        public List<ParDetails> lListParDetails { get; set; }
        public List<Parheader> ListParHeader { get; set; }
        public Parheader ParheaderObj { get; set; }
        public List<Employee_gid> Employeedetails { get; set; }
        public SelectList BudgetOwner { get; set; }
        public CbfUtilizedAmount Utilized { get; set; }
        public SelectList capexlist { get; set; }
        public SelectList budjectedlist { get; set; }
        public string boqfile { get; set; }
        public string result { get; set; }
        public int budgetGid { get; set; }
        //audittrail
        public List<CbfSumEntity> auditTrailLst { get; set; }
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
        public int queue_gid { get; set; }
        public int gid { get; set; }

        public int slNo { get; set; }
        public int employeeGid { get; set; }
        public string empCode { get; set; }
        public string empName { get; set; }
        public string empStatus { get; set; }
        public string empdesignation { get; set; }
        public string Approvaldate { get; set; }
        public string approvalgid { get; set; }

        public string NextApproverDetail { get; set; }

        public int _CBFAttachmentID { get; set; }
        public string _CBFAttachmentFileName { get; set; }
        public string _CBFAttachmentDate { get; set; }
        public string _CBFAttachmentDescription { get; set; }
        public string _CBFAttachmentActualFileName { get; set; }
        public string _CBFAttachmentFor { get; set; }   
    } 
    public class CbfSummarymodel
    {

        public int cbfGid { get; set; }
        public string cbfNo { get; set; }
        public string cbfDate { get; set; }
        public string cbfMode { get; set; }
        public string cbfEnddate { get; set; }
        public string cbfRequestfor { get; set; }
        public string fincon_Bugt { get; set; }
        public decimal cbfAmo { get; set; }
        public decimal cbfDevi_Amo { get; set; }
        public string cbfProjectOwner { get; set; }
        public string cbfStatus { get; set; }
        public string cbfDescription { get; set; }
        public string delmatRemarks { get; set; }
    }
    public class CbfUtilizedAmount
    {
        public decimal cbfAmount { get; set; }
        public decimal cbfBalance { get; set; }
        public decimal cbfUtilized { get; set; }
    }
    public class CbfRaiseHeader
    {
        public string cbfNo { get; set; }
        public string cbfDate { get; set; }
        public string cbfEnddate { get; set; }
        public Int32 branchCode { get; set; }
        public string cbfMode { get; set; }
        public string cbfType { get; set; }
        public string budgeted { get; set; }
        public string projectOwner { get; set; }
        public decimal deviationAmt { get; set; }
        public decimal cbfAmt { get; set; }
        public string description { get; set; }
        public string requestFor { get; set; }
        public string requestType { get; set; }
        public string justfication { get; set; }
        public string branchType { get; set; }

        public Int32 branchcodeGid { get; set; }
        public string branchcodeName { get; set; }

        public Int32 projectOwnerGid { get; set; }
        public string projectOwnerName { get; set; }

        public Int32 requeestForGid { get; set; }
        public string requestForName { get; set; }

        public Int32 BudgetownerGid { get; set; }
        public string BudgetOwner_Name { get; set; }
        public string budgetowner { get; set; }

        public int ParPRGid { get; set; }

    }
    public class CbfParHeader
    {

        public string parDate { get; set; }
        public string parNo { get; set; }
        public Int64 parGid { get; set; }
        public string parDesc { get; set; }
        public decimal parAmt { get; set; }
        public decimal utilizedAmt { get; set; }
        public decimal balancedAmt { get; set; }
        public string par_Requestfor { get; set; }
        public string par_Budget { get; set; }
    }
    public class CbfParDetails
    {

        public Int64 pardet_Gid { get; set; }
        public string par_Expense { get; set; }
        public string par_Requestfor { get; set; }
        public string par_Budget { get; set; }
        public string description { get; set; }
        public string year { get; set; }
        public decimal b_FwdAmount { get; set; }
        public decimal lin_Amt { get; set; }
        public decimal ulis_Amt { get; set; }
        public decimal c_FwdAmount { get; set; }
        public decimal balance { get; set; }
        public decimal attch_Gid { get; set; }
        public string remarks { get; set; }
    }
    public class CbfPrHeader
    {
        public Int64 prhed_Gid { get; set; }
        public int branch_Gid { get; set; }
        public string prNo { get; set; }
        public string branch { get; set; }
        public string dept { get; set; }
        public string description { get; set; }
        public string prRequestFor { get; set; }
        public string source { get; set; }
        public string prDate { get; set; }


    }
    public class CbfPrDetails
    {
        public Int32 prdetGid { get; set; }
        public Int32 prheaderGid { get; set; }
        public string productCode { get; set; }
        public string productGroup { get; set; }
        public string productName { get; set; }
        public string productDescription { get; set; }
        public string expense { get; set; }
        public string prBudget { get; set; }
        public string unit { get; set; }
        public decimal qty { get; set; }
        public decimal rate { get; set; }
        public decimal costestimation { get; set; }
        public Int64 attchId { get; set; }
    }

    public class CbfDetails
    {
        public Int32 prheaderGid { get; set; }
        public int cbfDetGid { get; set; }
        public int uomGid { get; set; }
        public string uomCode { get; set; }
        public int productGroupId { get; set; }
        public string productGroupIdName { get; set; }
        public string department { get; set; }
        public string budeget { get; set; }
        public string description1 { get; set; }
        public Int64 prdetailsGid { get; set; }
        public Int64 unChecked12 { get; set; }
        public string productgid { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public string description { get; set; }
        public string uom { get; set; }
        public decimal qty { get; set; }
        public decimal unitAmt { get; set; }
        public decimal total { get; set; }
        public string remarks { get; set; }
        public string chartOfAccount { get; set; }
        //   public string productcode { get; set; }
        public int fccc { get; set; }
        public int budgetLine { get; set; }
        public int attch_Gid { get; set; }
        public string ids { get; set; }
        public string vendorselection { get; set; }
        public int vendorgid { get; set; }
        public string att_identify { get; set; }
    }

    public class FcccMaster
    {
        public string fcccCode { get; set; }
        public string fcccGid { get; set; }
        public string fcccName { get; set; }
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
        public string FileTempName { get; set; }
    }
    public class CbfCheckerSummary
    {

        public int cbFGidChecker { get; set; }
        public string cbfNoChecker { get; set; }
        public string cbfDateChecker { get; set; }
        public string cbfModeChecker { get; set; }
        public string cbfEndDateChecker { get; set; }
        public string cbfrequestForChecker { get; set; }
        public string fincon_Bugchecker { get; set; }
        public decimal cbfAmoChecker { get; set; }
        public decimal cbfDevi_AmoChecker { get; set; }
        public string cbfProjectOwnerChecker { get; set; }
        public string cbfStatusChecker { get; set; }
        public string cbfDescriptionChecker { get; set; }

        public string Cbfclosuregid { get; set; }
        public string cbfclosuredate { get; set; }
        public string cbfclosureremarks { get; set; }


        public string Cbfcancellationgid { get; set; }
        public string cbfCancellationRemarks { get; set; }


    }
    public class CbfCheckerHeader
    {
        public string cbfGidChecker { get; set; }
        public string cbfNoChecker { get; set; }
        public string cbfDateChecker { get; set; }
        public string cbfEndDateChecker { get; set; }
        public Int32 branchCodeChecker { get; set; }
        public string cbfModeChecker { get; set; }
        public string cbfTypeChecker { get; set; }
        public string budgetedchecker { get; set; }
        public string projectOwnerChecker { get; set; }
        public decimal deviationAmtChecker { get; set; }
        public decimal cbfAmtChecker { get; set; }
        public string descriptionChecker { get; set; }
        public string requestForChecker { get; set; }
        public string requestTypeChecker { get; set; }
        public string justficationChecker { get; set; }
        public string branchTypeChecker { get; set; }

        public Int32 branchCodeGidChecker { get; set; }
        public string branchCodeNameChecker { get; set; }

        public Int32 projectOwnerGidChecker { get; set; }
        public string projectOwnerNameChecker { get; set; }

        public Int32 requeestForGidChecker { get; set; }
        public string requestForNameChecker { get; set; }

        public string raiser_code { get; set; }
        public string raiser_name { get; set; }

        public int supervisorGidChecker { get; set; }
        public string SupervisornameChecker { get; set; }


    }
    public class CbfCheckerDetails
    {
        public int cbfDetailGidChecker { get; set; }
        public int uomGidChecker { get; set; }
        public string uomCodeChecker { get; set; }
        public int productGroupidChecker { get; set; }
        public string productGroupidNameChecker { get; set; }
        public string departmentChecker { get; set; }
        public string budegetChecker { get; set; }
        public string description1Checker { get; set; }
        public Int64 prdetailsGidChecker { get; set; }
        public Int64 unChecked12Checker { get; set; }
        public string productCodeChecker { get; set; }
        public string productNameChecker { get; set; }
        public string descriptionChecker { get; set; }
        public string uomChecker { get; set; }
        public decimal qtyChecker { get; set; }
        public decimal unit_AmtChecker { get; set; }
        public decimal totalChecker { get; set; }
        public string remarksChecker { get; set; }
        public string chartOfAccountChecker { get; set; }
        //   public string productcode { get; set; }
        public int fcccChecker { get; set; }
        public int budgetLineChecker { get; set; }
        public int attch_GidChecker
        {
            get;
            set;
        }
        public string Idschecker { get; set; }
        public string vendorselection { get; set; }
        public int vendorgid { get; set; }
    }
    public class SheetDatasCBF
    {
        public int sheetId { get; set; }
        public string sheetName { get; set; }
    }
    public class BulkUploadCBF
    {
        public string shipmentType { get; set; }
        public string branchCode { get; set; }
        public int quantity { get; set; }
        public string sheetName { get; set; }
        public List<BulkUploadCBF> lListUpload { get; set; }
        public string slNo { get; set; }

        public int cbfSlNo { get; set; }
    }
    public class RaiserTicket
    {
        public int fromID { get; set; }
        public int toID { get; set; }
        public DateTime dateRaiser { get; set; }
        public string message { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string Fromemp { get; set; }
    }
    public class quenue
    {
        public DateTime queue_date { get; set; }
        public int cbfamount { get; set; }
        public int queue_ref_flag { get; set; }
        public int queue_ref_gid { get; set; }
        public int queue_ref_status { get; set; }
        public int queue_from { get; set; }
        public int queue_approvalstage { get; set; }
        public string actionremark { get; set; }
        public string queueto { get; set; }
        public string actionstatus { get; set; }
        public string skipflag { get; set; }
        public string cbfstatus { get; set; }
        public string ApprovalType { get; set; }

    }
    public class CBFBulkClosure
    {
        public string CbfNo { get; set; }
        public string CBfDate { get; set; }
        public string sheetName { get; set; }
        public int sheetId { get; set; }
        public List<CBFBulkClosure> lListBulkUpload { get; set; }
        public int slNo { get; set; }
        public string remarks { get; set; }
        public string result { get; set; }
        public string filename { get; set; }
    }
    public class Parheader
    {
        public string ParHeader_Gid { get; set; }
        public string ParNo { get; set; }
        public string ParDate { get; set; }
        public Decimal ParAmount { get; set; }
        public string FinalApprovalDate { get; set; }
        public string period { get; set; }
        public string pardescription { get; set; }
        public string contigency { get; set; }
        public string contigencyamount { get; set; }
        public string status { get; set; }
        public string budgeted { get; set; }
        public string ApproverDeleteIDs { get; set; }
    }
    public class ParDetails
    {
        public string cbfGid { get; set; }
        public string cbfref { get; set; }
        public string att_identify { get; set; }
        public int ParGid { get; set; }
        public int ParDetails_Gid { get; set; }
        public string Expense { get; set; }
        public string Department { get; set; }
        public string Budgeted { get; set; }
        public string Description { get; set; }
        public string ToYear { get; set; }
        public string FromYear { get; set; }
        public Decimal ParAmount { get; set; }
        public string Remarks { get; set; }
        public string Attachment { get; set; }
        public string Attachment_date { get; set; }

        public Int32 requeestForGid { get; set; }
        public string requestForName { get; set; }

        public string capexId { get; set; }
        public string capexname { get; set; }

        public string budjectedid { get; set; }
        public string budjectedname { get; set; }

    }
    public class ApprovalDetailsPar
    {
        public int Employee_Gid { get; set; }
        public string Employee_Name { get; set; }
        public string Designation { get; set; }
        public string DateofApproval { get; set; }
        public string Employee_code { get; set; }
    }
    public class Employee_gid
    {
        //Project Owners
        public int slNo { get; set; }
        public int employeeGid { get; set; }
        public string empCode { get; set; }
        public string empName { get; set; }
        public string empStatus { get; set; }
        public string empdesignation { get; set; }
        public string result { get; set; }
        public string Approvaldate { get; set; }
        public string approvalgid { get; set; }
    }
    public class vendorselection
    {
        public string vendorcode { get; set; }
        public string vendorname { get; set; }
        public int vendorgid { get; set; }
        public int slno { get; set; }
        public List<vendorselection> lstVendor { get; set; }
    }

    public class partransfer
    {
        public int parheadergid { get; set; }
        public string parheaderrefno { get; set; }
        public string parheaderdate { get; set; }
        public string parheaderamount { get; set; }
        public string parutilizeramount { get; set; }
        public string parbalencedamount { get; set; }
        public string parheaderdescription { get; set; }



    }

    public class pardetailtransfer
    {
        public string parheader_refno { get; set; }
        public int pardetails_parhead_gid { get; set; }
        public int pardetailsgid { get; set; }
        public string pardetailsexpensetype { get; set; }
        public string pardetailsrequestfor { get; set; }
        public string pardetailsbudgeted { get; set; }
        public decimal pardetailstransferinamount { get; set; }
        public decimal pardetailslineamount { get; set; }
        public decimal pardetailsutilizedamount { get; set; }
        public decimal pardetailstransferoutamount { get; set; }
        public decimal pardetailsbalencedamount { get; set; }
        public decimal pardetailstransferamount { get; set; }
        //public string pardetails_amt { get; set; }
        public string pardetails_desc { get; set; }
        public string pardetails_yearfrom { get; set; }
        public string pardetails_yearto{ get; set; }
        public decimal totallineamount { get; set; }
        public decimal totalutilizedamount { get; set; }
        public decimal toatalbalencedamount { get; set; }
        //public string pardetails_remarks { get; set; }
    }
}
