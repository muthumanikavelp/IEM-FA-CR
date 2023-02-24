using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IEM.Areas.EOW.Models
{
    public class EOW_Employee
    {
        public List<EOW_EmployeeeExpense> ListEmployeeeExpense { get; set; }
    }
    public class ApproverAction
    {
        public string Status { get; set; }
        public SelectList RejectholdData { get; set; }
        public int RejectholdId { get; set; }
        public string RejectholdName { get; set; }

        public string Concurrentapprovalid { get; set; }
        public string Concurrentapproval { get; set; }
        public string msgConcurrentapproval { get; set; }
    }
    public class RejectholdData
    {
        public int RejectholdId { get; set; }
        public string RejectholdName { get; set; }
    }
    public class ApproverHistry
    {
        public int rowcount { get; set; }
        public string empcode { get; set; }
        public string empname { get; set; }
        public string actiontype { get; set; }
        public string status { get; set; }
        public string empdel { get; set; }
        public string empdesi { get; set; }
        public string statusdate { get; set; }
        public string remarks { get; set; }
    }
    public class upcommingApprover
    {
        public string empcode { get; set; }
        public string empname { get; set; }
        public string actiontype { get; set; }
        public string status { get; set; }
        public string statusdate { get; set; }
        public string remarks { get; set; }
    }
    public class Approveraction
    {
        public string status { get; set; }
        public string Rejecthold { get; set; }
        public string Concurrent { get; set; }
        public string Concurrentmsg { get; set; }
        public string appkremarks { get; set; }
    }
    public class DashBoard
    {
        List<DashBoard> lstDashBoardMyReuest = new List<DashBoard>();
        List<DashBoard> lstDashBoardMyApp = new List<DashBoard>();
        List<DashBoard> lstDashBoardCtChkr = new List<DashBoard>();
        List<DashBoard> lstDashBoardCtMkr = new List<DashBoard>();

        public int Doctypeid { get; set; }
        public int Docnogid { get; set; }
        public string DocDate { get; set; }
        public string Docno { get; set; }
        public string Docamount { get; set; }

        public string Invoiceno { get; set; }

        public SelectList DocTypeData { get; set; }
        public string DocTypeIdd { get; set; }
        public string DocTypeName { get; set; }

        public SelectList StatusTypeData { get; set; }
        public string StatusTypeId { get; set; }
        public string StatusTypeName { get; set; }

        public string raiserName { get; set; }
        public string emporsupp { get; set; }

        public string DashBoardRequestType { get; set; }
        public int DraftCount { get; set; }
        public int InprocessCount { get; set; }
        public int ApprovedCount { get; set; }
        public int RejectedCount { get; set; }
        public int PaidCount { get; set; }
        public int ForApprovalCount { get; set; }
        public string ecfdescription { get; set; }
        public string ecfdelegate { get; set; }

        public int EPUInprocessCount { get; set; }
        public int EPURejectedCount { get; set; }
        public int CancelledCount { get; set; }

        public string ecfview { get; set; }
        public string ecfprint { get; set; }
        public string ecfselect { get; set; }
        public string ecfprintid { get; set; }
    }
    public class EOW_File
    {
        public SelectList AttachmentTypeData { get; set; }
        public int AttachmentTypeId { get; set; }
        public string AttachmentTypeName { get; set; }
        public string AttachmentFilenamedata { get; set; }
        public int AttachmentFilenameId { get; set; }
        public string AttachmentFilename { get; set; }
        public string AttachmentDescription { get; set; }
        public string AttachmentDate { get; set; }
        public string AttachmentBy { get; set; }
        public int attachment_by { get; set; }
        public string Name { get; set; }
        public int Length { get; set; }
        public string Type { get; set; }

        public int FileId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
    public class EOW_Payment
    {
        public SelectList PaymentModedata { get; set; }
        public int PaymentModeId { get; set; }
        public string PaymentModeName { get; set; }

        public SelectList Refdata { get; set; }
        public int RefNoId { get; set; }
        public string RefNoName { get; set; }
        public int Paymentgid { get; set; }
        public string Beneficiary { get; set; }
        public string Description { get; set; }
        public string Beneficiarycardno { get; set; }
        public string PaymentAmount { get; set; }
        public string AmountINR { get; set; }
        public string Employeecode { get; set; }
        public string Employeename { get; set; }
        public string Exception { get; set; }
        public string Ifsccode { get; set; }
        //Ramya 01 Aug 2018 Start
        public string ECFID { get; set; }
        public string GLID { get; set; }
        //Ramya 01 Aug 2018 End
    }

    public class EOW_Attachment
    {
        public int AttachmentTypeId { get; set; }
        public string AttachmentTypeName { get; set; }
    }
    public class EOW_Raisermode
    {
        public string raisermodeId { get; set; }
        public string raisermodeName { get; set; }
    }
    public class EOW_PaymentMode
    {
        public int PaymentModeId { get; set; }
        public string PaymentModeName { get; set; }
    }
    public class EOW_RefNo
    {
        public string RefNoId { get; set; }
        public string RefNoName { get; set; }
        public string RefAmount { get; set; }
        public string RefException { get; set; }
        public string RefLiquidationdate { get; set; }
        public string RefDescription { get; set; }

        public ecfdmodels ecfdetails { get; set; }
        public List<invoicedmodela> invoiceDetails { get; set; }
    }
    public class invoicedmodela
    {
        public string RefNoIde { get; set; }
        public string invoicegid { get; set; }
        public string invoicedate { get; set; }
        public string invoiceno { get; set; }
        public string invoiceamt { get; set; }
        public string invoicedesc { get; set; }
        public string invoicemonth { get; set; }
    }
    public class ecfdmodels
    {
        public string RefNoIde { get; set; }
        public string RefNoNamee { get; set; }
        public string RefAmounte { get; set; }
        public string RefExceptione { get; set; }
        public string RefLiquidationdatee { get; set; }
        public string RefDescriptione { get; set; }
    }
    public class EOW_NatureofExpenses
    {
        public int NatureofExpensesId { get; set; }
        public string NatureofExpensesName { get; set; }
    }
    public class EOW_ExpenseCategory
    {
        public int ExpenseCategoryId { get; set; }
        public string ExpenseCategoryName { get; set; }
    }
    public class EOW_SubCategory
    {
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public string GLCode { get; set; }
    }
    public class EOW_TravelMode
    {
        public int TravelModeId { get; set; }
        public string TravelModeName { get; set; }
    }
    public class EOW_TravelClass
    {
        public int TravelClassId { get; set; }
        public string TravelClassName { get; set; }
    }
    public class EOW_TravelCity
    {
        public int TravelCityId { get; set; }
        public string TravelCityName { get; set; }
    }
    public class SheetData
    {
        public int SheetId { get; set; }
        public string SheetName { get; set; }
    }
    public class EOW_Raiserfc
    {
        public string raiserfcId { get; set; }
        public string raiserfcName { get; set; }
    }
    public class EOW_Raisercc
    {
        public string raiserccId { get; set; }
        public string raiserccName { get; set; }
    }
    public class EOW_citys
    {
        public string citysId { get; set; }
        public string citysName { get; set; }
    }
    public class letterWords
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string field3 { get; set; }
        public string field4 { get; set; }
        public string field5 { get; set; }
    }
    //Ramya - 01 Aug 18
    public class InvoiceCreditLine
    {
        public string Ecfid { get; set; }
        public string InvId { get; set; }
        public string CreditlineGId { get; set; }
        public string RefId { get; set; }
        public string paymode { get; set; }
        public string RefNo { get; set; }
        public string Beneficiary { get; set; }
        public string Desc { get; set; }
        public string Amount { get; set; }
        public string BankId { get; set; }
        public string IsRemoved { get; set; }
        public string IfscCode { get; set; }
        public string Id { get; set; }
    }
    public class EOW_EmployeeeExpense
    {
        //EmployeeeBasic Model
        public Int32 supplierheader_ggid { get; set; }
        public SelectList Raiser_Modedata { get; set; }
        public string Raiser_Code { get; set; }
        public string Raiser_Mode { get; set; }
        public string Raiser_Name { get; set; }
        public string ECF_Date { get; set; }
        public string Grade { get; set; }
        public string ECF_Amount { get; set; }
        public string ClaimMonth { get; set; }
        public string raisermodeId { get; set; }
        public string raisermodeName { get; set; }
        public string ecfremark { get; set; }
        public string noofperson { get; set; }

        public string ecfno { get; set; }
        //EmployeeeExpense Model 
        public int Exp_GID { get; set; }
        public int invoice_GID { get; set; }
        public int ecf_GID { get; set; }
        public int Queue_GID { get; set; }
        public int Doctypeid { get; set; }
        public string queueactionfor { get; set; }
        //public string Exp_NatureofExpenses { get; set; }
        //public string Exp_ExpenseCategory { get; set; }
        //public string Exp_SubCategory { get; set; }
        public string Exp_ClaimPeriodFrom { get; set; }
        public string Exp_ClaimPeriodTo { get; set; }
        public string Exp_ClaimMonth { get; set; }

        public SelectList Exp_FCC { get; set; }
        public string Exp_FC { get; set; }
        public SelectList Exp_CCC { get; set; }
        public string Exp_CC { get; set; }
        public string Exp_FCCC { get; set; }

        public string Exp_ProductCode { get; set; }
        public string Exp_OUCode { get; set; }
        public string Exp_Amount { get; set; }

        public SelectList ExpNatureofExpdata { get; set; }
        public int NatureofExpensesId { get; set; }
        public string NatureofExpensesName { get; set; }

        public SelectList ExpCatdata { get; set; }
        public int ExpenseCategoryId { get; set; }
        public string ExpenseCategoryName { get; set; }

        public SelectList ExpSubCatdata { get; set; }
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }
        public string ecfdescription { get; set; }
        public string ecfdelmatamt { get; set; }

        public int ecf_raiser { get; set; }  // arf 
        public string ecf_raisername { get; set; }  // arf 

        public SelectList TravelModedata { get; set; }
        public int TravelModeId { get; set; }
        public string TravelModeName { get; set; }

        public SelectList TravelClassdata { get; set; }
        public int TravelClassId { get; set; }
        public string TravelClassName { get; set; }

        public SelectList Citydata { get; set; }
        public int TravelCityId { get; set; }
        public string TravelCityName { get; set; }

        public SelectList Citydatato { get; set; }
        public int TravelCitytoId { get; set; }
        public string TravelCitytoName { get; set; }

        public string PlaceFrom { get; set; }
        public string PlaceTo { get; set; }
        public string Rate { get; set; }
        public string Distance { get; set; }
        public string NatureofExpenseslocal { get; set; }
        public string travelDescription { get; set; }


        public Int32 expsubcat_gid { get; set; }
        public Int32 expsubcat_expnature_gid { get; set; }
        public int selectedexpsubcat_expnature_gid { get; set; }
        public Int32 expsubcat_expcat_gid { get; set; }
        public string expsubcat_code { get; set; }
        public string expsubcat_name { get; set; }
        public string expsubcat_help { get; set; }
        public string expsubcat_active { get; set; }
        public Int32 expsubcat_insert_by { get; set; }
        public DateTime expsubcat_insert_date { get; set; }
        public Int32 expsubcat_update_by { get; set; }
        public DateTime expsubcat_update_date { get; set; }
        public string expsubcat_isremoved { get; set; }
        public int selectedexpcat_gid { get; set; }
        public Int32 expcat_gid { get; set; }
        public string expcat_name { get; set; }
        public SelectList Getexpcat { get; set; }
        public SelectList GetexpcatById { get; set; }

        public Int32 expnature_gid { get; set; }
        public string expnature_name { get; set; }
        public SelectList Getexpnature { get; set; }
        public SelectList GetexpnatureById { get; set; }

        public string arftype { get; set; }
        public string arfempsupcode { get; set; }
        public string arfempsupname { get; set; }
        public int Hsnid { get; set; }
        public SelectList HsncodeList { get; set; }
        public string HsnDescription { get; set; }
        public string RCMFlag { get; set; }
        public string ecf_paymode { get; set; }
        public string CygnetFlag { get; set; }
        //public decimal ecf_amount { get; set; }
        //EmployeeePayment Model 

        //public SelectList EmployeeePayment_PaymentModedata { get; set; }
        //public int PaymentModeId { get; set; }
        //public string PaymentModeName { get; set; }

        //public SelectList EmployeeePayment_RefNodata { get; set; }
        //public int RefNoId { get; set; }
        //public string RefNoName { get; set; }
        //public int Paymentmainid { get; set; }
        //public string EmployeeePayment_Beneficiary { get; set; }
        //public string EmployeeePayment_Description { get; set; }
        //public string EmployeeePayment_Amount { get; set; }

        //EmployeeeAttachment Model 

        //public SelectList EmployeeeAttachment_AttachmentTypedata { get; set; }
        //public int AttachmentTypeId { get; set; }
        //public string AttachmentTypeName { get; set; }

        //public string EmpFilesToBeUploaded { get; set; }
        //public HttpPostedFileBase EmpFilesToBeUploadeddata { get; set; }

        //public string EmployeeeAttachment_Filename { get; set; }
        //public string EmployeeeAttachment_Description { get; set; }
        //public string EmployeeeAttachment_AttachedDate { get; set; }
        //public string EmployeeeAttachment_Attachedby { get; set; }
    }
    public class EOW_HSN
    {
        public int HsnId { get; set; }
        public string HsnCode { get; set; }
        public string HsnDesc { get; set; }

    }
    public class Mailalert
    {
        public SelectList Template { get; set; }
        public string TemplateId { get; set; }
        public string TemplateName { get; set; }

        public SelectList TransactionType { get; set; }
        public string TransactionTypeId { get; set; }
        public string TransactionTypeName { get; set; }

        public SelectList TriggerType { get; set; }
        public string TriggerTypeId { get; set; }
        public string TriggerTypeName { get; set; }


        public SelectList TriggerFreType { get; set; }
        public string TriggerTypeFreId { get; set; }
        public string TriggerTypeFreName { get; set; }
        public string Triggernodays { get; set; }

        public SelectList AudienceType { get; set; }
        public string AudienceId { get; set; }
        public string AudienceName { get; set; }

        public SelectList ToType { get; set; }
        public string ToTypeId { get; set; }
        public string ToTypeName { get; set; }

        public SelectList ToGetType { get; set; }
        public string ToGetTypeId { get; set; }
        public string ToGetTypeName { get; set; }

        public string TOid { get; set; }
        public string BCCid { get; set; }
        public string CCid { get; set; }
        public string Subject { get; set; }
        public string Includeflg { get; set; }

        public SelectList Attachment { get; set; }
        public string AttachmentId { get; set; }
        public string AttachmentName { get; set; }

        public string Content { get; set; }
        public string Signature { get; set; }

        public SelectList Tables { get; set; }
        public string TablesId { get; set; }
        public string TablesName { get; set; }

        public SelectList Tablescol { get; set; }
        public string TablescolId { get; set; }
        public string TablescolName { get; set; }

        public string[] TablescolNamenew { get; set; }

        public SelectList Moduledata { get; set; }
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }

        [AllowHtml]
        public string HtmlContent { get; set; }
    }

    public class InvoiceTax
    {
        public string action { get; set; }
        public string InvoiceTaxgid { get; set; }
        public string InvId { get; set; }
        public string SupplierId { get; set; }
        public string TaxId { get; set; }
        public string SubTaxId { get; set; }
        public string TaxRate { get; set; }
        public string TaxableAmt { get; set; }
        public string TaxAmount { get; set; }
        public string IsRemoved { get; set; }

        public string GSNapplicable { get; set; }
        public string HSNgid { get; set; }
    }

    //Ramya Added
    public class RCMcheck
    {
        public string DebitlineGId { get; set; }
        public string hsngid { get; set; }
        public string ProviderLocation { get; set; }
        public string RcmFlag { get; set; }
        public string action { get; set; }
    }

    public class RCMEnteries
    {
        public string Expsubcat_Name { get; set; }
        public string HSNCode { get; set; }
        public string RCMType { get; set; }
        public decimal RCM_Rate { get; set; }
        public decimal RCM_Amount { get; set; }
        public string RCM_InputCredit_GL { get; set; }
        public string RCM_ReverseCharge_GL { get; set; }
    }

}