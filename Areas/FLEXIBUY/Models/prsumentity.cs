using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace IEM.Areas.FLEXIBUY.Models
{
    public class PrsummaryModel
    {
        public string Prgid { get; set; }
        public string prRefNo { get; set; }
        public string prDate { get; set; }
        public string prBranch { get; set; }
        public string prDept { get; set; }
        public string prDesc { get; set; }
        public string prReqFor { get; set; }
        public string prStatus { get; set; }
        public int srNo { get; set; }

    }

    public class AttachmentList
    {

        public string fileName { get; set; }
        public string attachedDate { get; set; }
        public string description { get; set; }
        public string attachGid { get; set; }

    }
    public class PrHeader
    {
        public string prRefNo { get; set; }
        public string newPrRefNo { get; set; }
        public string prRaisedBy { get; set; }
        public string prFcc { get; set; }
        public string prExpense { get; set; }
        public string prDate { get; set; }
        public string prBranchType { get; set; }
        public string prBranch { get; set; }
        public string prDesc { get; set; }
        public string prReqFor { get; set; }
        public string prStatus { get; set; }
        public string prRemarks { get; set; }
        public string prGid { get; set; }
        public string requestForName { get; set; }

    }
    public class PrDetails
    {
        public Int32 prDet_PrHeader_Gid { get; set; }
        public Int32 prDet_Gid { get; set; }
        public string prDet_Product_Code { get; set; }
        public string prDet_Product_Group { get; set; }
        public string prDet_Product_Name { get; set; }
        public string prDet_Product_Description { get; set; }
        public string prDet_Unit { get; set; }
        public string prDet_Expense { get; set; }
        public string prDet_prBudget { get; set; }
        public string prDet_Department { get; set; }
        public int prDet_Qty { get; set; }
        public decimal prDet_Rate { get; set; }
        public decimal prDet_CostEstimation { get; set; }
        public int srNo { get; set; }

        public Int64 prDet_Attch_Gid { get; set; }
    }
    public class Product
    {
        public Int32 prDet_Gid { get; set; }
        public string product_gid { get; set; }
        public string product_Group { get; set; }
        public string product_Code { get; set; }
        public string product_Name { get; set; }
        public string prodserv_Type { get; set; }
        public string product_Description { get; set; }
        public string product_Unit { get; set; }
        public decimal product_Qty { get; set; }
        public string product_rate { get; set; }
        public string product_cost { get; set; }

        public string service_Group { get; set; }
        public string service_Code { get; set; }
        public string service_Name { get; set; }
        public string service_Description { get; set; }

        public string prodservgrp_Gid { get; set; }
        public string productUnit_Gid { get; set; }

        public int srNo { get; set; }

    }

    public class prsupervisior
    {
        public int prgid { get; set; }
        public string prRefNo { get; set; }
        public string newPrRefNo { get; set; }
        public string prRaisedBy { get; set; }
        public string prFcc { get; set; }
        public string prExpense { get; set; }
        public string prDate { get; set; }
        public string prBranchType { get; set; }
        public string prBranch { get; set; }
        public string prDesc { get; set; }
        public string prReqFor { get; set; }
        public string prStatus { get; set; }
        public string prRemarks { get; set; }
        public string prDept { get; set; }
        public int srNo { get; set; }
        public List<prsupervisior> prdetailslist { get; set; }


    }
    public class Attachment1
    {
        public string fileName { get; set; }
        public string attachedDate { get; set; }
        public string description { get; set; }
        public string attachGid { get; set; }
        public string employee_gid { get; set; }
        public string employee_name { get; set; }
        public string attachment_Gid { get; set; }
    }

    public class PrSumEntity
    {

        public Int64 prDet_Attch_Gid { get; set; }

        public List<PrsummaryModel> lstprSum { get; set; }

        public List<Product> lstproduct { get; set; }
        public List<prsupervisior> lstsupervisor { get; set; }

        public PrHeader prHead { get; set; }

        public prsupervisior prsupervisor { get; set; }
        public PrDetails prDet { get; set; }
        public Product product { get; set; }



        public List<PrHeader> lstprHead { get; set; }
        public List<PrDetails> lstprDet { get; set; }

        public List<Attachment> attachment { get; set; }


        public List<PrSumEntity> lstattfile { get; set; }

        public SelectList ddlBranch { get; set; }
        public Int32 branchGid { get; set; }
        public string branchName { get; set; }

        public SelectList ddlStatus { get; set; }
        public Int32 statusGid { get; set; }
        public string statusName { get; set; }

        public SelectList ddlRequestfor { get; set; }
        public Int32 requestForGid { get; set; }
        public string requestForName { get; set; }

        public SelectList ddlFCCC { get; set; }
        public Int32 fcccGid { get; set; }
        public string fcccName { get; set; }


        public SelectList ddlProductGroup { get; set; }
        public Int32 productGroupGid { get; set; }
        public string productGroupName { get; set; }

        public SelectList ddlUom { get; set; }
        public Int32 uomGid { get; set; }
        public string uomCode { get; set; }


        public string FilesToBeUploaded { get; set; }
        //public  string srno { get; set; }
        public string documentName { get; set; }
        public string attachmentDate { get; set; }
        public string attachmentDesc { get; set; }
        int count { get; set; }

        public string attachid { get; set; }
        public string attachName { get; set; }
        public string attachFileType { get; set; }
        public string attachlength { get; set; }

        public string attachment1 { get; set; }

        public string selectedValue { get; set; }
        public string selectedValue1 { get; set; }
        public string prRefNo { get; set; }
        public string prdate { get; set; }
        public string MyFile { get; set; }

        //audittrail
        public List<PrSumEntity> auditTrailLst { get; set; }
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
    }

    public class flexibuydashboard
    {
        public List<flexibuydashboard> pardashboard { get; set; }
        public string Requesttype { get; set; }
        public int draft { get; set; }
        public int inprocess { get; set; }
        public int approval { get; set; }
        public int reject { get; set; }
        public int formyapproval { get; set; }
        public string refno { get; set; }
        public string date { get; set; }
        public string branch { get; set; }
        public string department { get; set; }
        public string requestfor { get; set; }
        public string description { get; set; }
        public string raiser { get; set; }
        List<flexibuydashboard> Prdashbord { get; set; }
        public int dsno { get; set; }
        public int dgid { get; set; }
        public string category { get; set; }
        public string docNo { get; set; }
        public string ddate { get; set; }
        public string amount { get; set; }
        public string status { get; set; }
        public string statusId { get; set; }
        public string docRaiser { get; set; }

        public SelectList DocTypeData { get; set; }
        public string DocTypeIdd { get; set; }
        public string DocTypeName { get; set; }

        public SelectList StatusTypeData { get; set; }
        public string StatusTypeId { get; set; }
        public string StatusTypeName { get; set; }

        public SelectList DocTypeDataApp { get; set; }
        public string DocTypeAppId { get; set; }
        public string DocTypeAppName { get; set; }

        public SelectList StatusTypeApp { get; set; }
        public string StatusTypeAppId { get; set; }
        public string StatusTypeAppName { get; set; }

        public string parHeader_Gid { get; set; }
        public string parNo { get; set; }
        public string parDate { get; set; }
        public string parDescription { get; set; }
        public string parStatus { get; set; }

        public int prsrNo { get; set; }
        public string prgid { get; set; }
        public string prRefNo { get; set; }
        public string prDate { get; set; }
        public string prBranch { get; set; }
        public string prDept { get; set; }
        public string prDesc { get; set; }
        public string prReqFor { get; set; }
        public string prStatus { get; set; }

        public string cbfGid { get; set; }
        public string cbfNo { get; set; }
        public string cbfDate { get; set; }
        public string cbfEnddate { get; set; }
        public string cbfProjectOwner { get; set; }
        public decimal cbfDevi_Amo { get; set; }
        public decimal cbfAmo { get; set; }
        public string cbfDescription { get; set; }
        public string fincon_Bugt { get; set; }
        public string cbfRequestfor { get; set; }
        public string cbfStatus { get; set; }
        public string cbfMode { get; set; }

        public int poslno { get; set; }
        public int poDetgid { get; set; }
        public string poDate { get; set; }
        public string poRefNo { get; set; }
        public string povendorName { get; set; }
        public string postatus { get; set; }
        public string poAmount { get; set; }
        public string poVersion { get; set; }

        public int woslno { get; set; }
        public int woDetgid { get; set; }
        public string woDate { get; set; }
        public string woRefNo { get; set; }
        public string wovendorName { get; set; }
        public string wostatus { get; set; }
        public string woAmount { get; set; }
    }

    public class grnconfirmation
    {
        public int grnheadgid { get; set; }
        public string grndate { get; set; }
        public string grnrefno { get; set; }
        public string poworefno { get; set; }
        public string poVersion { get; set; }
        public string vendorname { get; set; }
        public double powoamount { get; set; }
        public decimal grnQuantity { get; set; }
        public int slno { get; set; }
        public string remarks { get; set; }
        public List<grnconfirmation> grnconfirmations { get; set; }
    }
    #region Modified
    public class grnconfirmationdetails
    {
        public int slno { get; set; }
        public string grndate { get; set; }
        public int grnheadGid { get; set; }
        public string grnrefno { get; set; }
        public string poworefno { get; set; }
        public string vendorname { get; set; }
        public double powoamount { get; set; }
        public string dcno { get; set; }
        public string invoiceno { get; set; }
        public string productgroup { get; set; }
        public string productcode { get; set; }
        public string productname { get; set; }
        public string uomcode { get; set; }
        public decimal receivedqty { get; set; }
        public string receiveddate { get; set; }
        public string manfName { get; set; }
        public string assetSlno { get; set; }
        public string putToUseDate { get; set; }
        public string grnTag { get; set; }
        public int grndetGid { get; set; }
        public string IsSerial { get; set; }
        public string IsBranch { get; set; }
        public string GRNType { get; set; }
        public string GRNdes { get; set; }
        public List<grnconfirmationdetails> grnconfirmationdetail { get; set; }

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

}