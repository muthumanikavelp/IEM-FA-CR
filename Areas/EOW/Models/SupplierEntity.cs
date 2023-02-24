using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace IEM.Areas.EOW.Models
{
    public class SupplierEntity
    {

    }
    public class SupSlabModel
    {
    }
    public class SupClassificationModel
    {
        public Int32 _ClassificationID { get; set; }
        public string _ClassificationName { get; set; }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public string UOM { get; set; }
        public int Quantity { get; set; }
        public DataTable dt { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Gmail { get; set; }
        public int Slabid { get; set; }
        public int Slabrange_Slabgid { get; set; }
        public int Slabrange_gid { get; set; }
        public string Slabname { get; set; }
        public string Slabrange_Name { get; set; }
        public string Slabrange_From { get; set; }
        public string Slabrange_To { get; set; }
        public int SNo { get; set; }
        public string slab_insert_by { get; set; }
        public string slab_update_by { get; set; }
        public string newslabadd { get; set; }
        public string newslabupdate { get; set; }
        public string HolidayDate { get; set; }
        public string HolidayName { get; set; }
        public int Holiday_gid { get; set; }
        public char StateHoliday { get; set; }
        public char NationalHoliday { get; set; }
        public char Cutoff { get; set; }
        public IList<SelectListItem> State { get; set; }
        public string[] lstSelectedStateGid = new string[32];
        public int HolidaySate_Holiday_Gid { get; set; }
        public string Rollname { get; set; }
        public int Rollgid { get; set; }

        public int Roleid { get; set; }
        public string Role { get; set; }
        public string RoleGroup { get; set; }

        public int Employeeid { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public string ButtonName { get; set; }
        public string Error { get; set; }
        public IList<SelectListItem> RoleMapping { get; set; }
        public string[] lstSelectedRoleGid = new string[32];
    }
    public class SupEmployeeRole
    {
        public IList<SelectListItem> RoleMapping { get; set; }
        public string[] lstSelectedRoleGid = new string[32];
        List<SelectListItem> Rolelist = new List<SelectListItem>();
        public int Roleid { get; set; }
        public string Role { get; set; }
        public string RoleGroup { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string Updateby { get; set; }
        public string Insertby { get; set; }
    }
    public class CentraldataModel
    {
        public int HoldId { get; set; }
        public int InwardId { get; set; }
        public int EmployeeId { get; set; }
        public string ReceivedDate { get; set; }
        public string PaidDate { get; set; }//selva
        public int SupplierId { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public char SupplierType { get; set; }
        public string StringSupplierType { get; set; }
        public string RaiserCode { get; set; }
        public string RaiserName { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceAmount { get; set; }
        public string Remarks { get; set; }
        public string HoldRemarks { get; set; }
        public string[] SelectedValues { get; set; }
        public int StatusGid { get; set; }
        public string statusname { get; set; }
        public int POid { get; set; }
        public string PONo { get; set; }
        public string pototal { get; set; }
        public string POtype{ get; set; }
        public string PanNo { get; set; }
        public string EcfNo { get; set; }
        public string EcfDate { get; set; }
        public string ReturnedDate { get; set; }

        public string Docno { get; set; }
        public string DocDate { get; set; }
        public string Docamount { get; set; }
        public string ECFMaker { get; set; }

        public string ecfselect { get; set; }
        public string ecfview { get; set; }

        public string emporsupp { get; set; }
        public string DocTypeName { get; set; }
        public string StatusTypeName { get; set; }
        public int Docnogid { get; set; }
        public string EmployeeGrade { get; set; }
        public string EmployeeMobile { get; set; }

        public string DOT { get; set; }
        public string EmployeeDepartment { get; set; }
        public string MODE { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public int BY { get; set; }
        public string ecfstatus { get; set; }
        public int queue_gid { get; set; }

        public int employeeGid { get; set; }
        public string empCode { get; set; }
        public string empName { get; set; }
        public string empbranch { get; set; }
        public string empfc { get; set; }
        public string empcc { get; set; }
        public string maingid { get; set; }

        public string Inwardedby { get; set; }
        public string Exrate { get; set; }
        public string Currencyamount { get; set; }
        public string CurrencyName { get; set; }
        public string CurrencyId { get; set; }
        public int Currencyrate { get; set; }
        public string ECF_Amount { get; set; }
        public SelectList Currencydata { get; set; }

        public string empfcname { get; set; }
        public string empccname { get; set; }
        public string empouname { get; set; }
        public string empproductname { get; set; }

        public string FinalApproverDate { get; set; }
        public string DateEcfSentToEPUbyCPPU { get; set; }
        public string EPURejectDate { get; set; }
        public string EPUPaidDate { get; set; }

        public string CBFNo { get; set; }
        public string POWOAmount { get; set; }
        public string POWODate { get; set; }
        public string POWOBalance { get; set; }
        public string POWOStatus { get; set; }
    }
    public class ECFDataModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ECFNo { get; set; }
        public int QueueId { get; set; }
        public int ECFId { get; set; }
        public int Docgid { get; set; }
        public string Docname { get; set; }
        public int DocSubgid { get; set; }
        public string DocSubname { get; set; }
        public int StatusGid { get; set; }
        public string statusname { get; set; }
        public SelectList statusnameData { get; set; }
        public SelectList selectdoctypedata { get; set; }
        public SelectList selectecffor { get; set; }
        public string SupplierorEmployee { get; set; }
        public string SupplierorEmployeename { get; set; }
        public string ECFRaiser { get; set; }
        public int ECFRaiserid { get; set; }
        public string DocTypeName { get; set; }
        public string DocSubTypeName { get; set; }
        public string CreateMode { get; set; }
        public string ClaimMonth { get; set; }
        public string ECFAmount { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string PrintStatus{ get; set; }

        public string Despatchdate { get; set; }
        public string CourierName { get; set; }
        public string AWBNo { get; set; }
        public string Reason { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceAmt { get; set; }
        public string OracleLocationCode { get; set; }
        public string GLcode { get; set; }
        public string BusinessSegment { get; set; }
        public string CostCentre { get; set; }
        public string ProductCode { get; set; }
        public string WorkorderNo { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string DebitLineAmount { get; set; }
        public string ECFdescription { get; set; }
        public string FinalStatus { get; set; }
        public string InvoiceDateFrom { get; set; }
        public string InvoiceDateTo { get; set; }
       
        public string InvoiceDescription { get; set; }
        public string ExpenseNature { get; set; }
        public string RaiserCode { get; set; }
        public string RaiserDepartment { get; set; }
        //public string EcfStatus { get; set; }
        public string ECFStatus { get; set; }
        public string CancelBy { get; set; }
        public string CancelDate { get; set; }
        public string ECFDate { get; set; }
        public string Command { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyAmount { get; set; }
        public string CurrencyRate { get; set; }
        public string ReducedAmount { get; set; }
        public string ProcessedAmount { get; set; }
        public string EcfRemark { get; set; }
        public string ecfFor { get; set; }
        public string ecfdatefrom { get; set; }
        public string ecfdateto { get; set; }
        public SelectList SelectDocSubType { get; set; }
        public SelectList SelectCourier { get; set; }
        public string CurrentuserApprovestatus { get; set; }

        public string PayMode { get; set; }
        public string CapitalizationDate { get; set; }
        public string TAT { get; set; }
        public string NetAmount { get; set; }
        public string PaidDate { get; set; }
        public string ServiceTax { get; set; }
        public string ValueAddedTax { get; set; }
        public string TaxDeductedatSource { get; set; }
        public string WorksContractTax { get; set; }
        public string Prepaid { get; set; }
        public string Suspense { get; set; }
        public string Creditnote { get; set; }
        public string Document { get; set; }

        public string InvoiceServiceMonth { get; set; }
        public string EcfDespatchDate { get; set; }

    }
    public class SelectCourier
    {
        public int courierid { get; set; }
        public string couriername { get; set; }

        public string ARF_No { get; set; }
        public string ARF_Date_From { get; set; }
        public string ARF_Date_To { get; set; }
        public string ARF_Amount { get; set; }
        public string OutStandingAmount { get; set; }
        public string RaiserCode { get; set; }
        public string RaiserName { get; set; }
        public int ECF_Id { get; set; }
        public string TransferFrom { get; set; }
        public int Raiser_Id { get; set; }
        public string Remark { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
    }
    public class SelectDocSubType
    {
        public int DocSubgid { get; set; }
        public string DocSubname { get; set; }
    }
    public class RetentionCancelData
    {
        public string ReleaseDate { get; set; }
        public string ecf_suppliername { get; set; }
        public string ecf_no { get; set; }
        public string ecf_date { get; set; }
        public string ecf_suppliercode { get; set; }
        public Double ecf_amount { get; set; }
        public string extendeddate { get; set; }
        public int invoice_ecf_gid { get; set; }
        public int invoice_gid { get; set; }
        public decimal invoice_retention_amount { get; set; }
        public decimal invoice_retention_exception { get; set; }
        public string invoice_no { get; set; }
        public Double invoice_amount { get; set; }
        public string invoice_retention_releaseon { get; set; }
        public string extended_date { get; set; }
        public Double release_amount { get; set; }
        public string Remarks { get; set; }
        public string invoice_date { get; set; }
        public string invoice_description { get; set; }
        public string Retention_date { get; set; }
        public string Retention_rate { get; set; }
        public Double Retention_Amount { get; set; }
        public Double Balance_Amount { get; set; }

        public string Retention_Releasedate { get; set; }
        public string Retention_ReleaseAmount { get; set; }
        public string Retention_ReleasedAmount { get; set; }
        public string Retention_BalanceAmount { get; set; }
        public string Retention_Remarks { get; set; }

        public string CancelDate { get; set; }
        public string CancelAmount { get; set; }
        public string BalanceAmount { get; set; }
    }
    public class ProxyDataModel
    {
        public int proxy_gid { get; set; }
        public string proxy_by { get; set; }
        public string proxy_to { get; set; }
        public string proxy_period_from { get; set; }
        public string proxy_period_to { get; set; }
        public string proxy_remark { get; set; }
        public string proxy_active { get; set; }
        public string ProxyGrade { get; set; }
        public string MobileNo { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public int EmployeeId { get; set; }
        public string RaiserName { get; set; }
        public string RaiserCode { get; set; }
        public string EmployeeGrade { get; set; }
        public string EmployeeMobile { get; set; }
    }
    public class printdatamodel
    {
        public string Ecf_Gid { get; set; }
        public string Ecf_Date { get; set; }
        public string Ecf_Amount { get; set; }
        public string Ecf_Amountinword { get; set; }
        public string Ecf_No { get; set; }
        public string Ecf_Raiser { get; set; }
        public string Ecf_Amort { get; set; }
        public string Ecf_forex { get; set; }
        public string Ecf_Utility { get; set; }
        public string Ecf_PaymentAdjst { get; set; }
        public string Ecf_dcsc { get; set; }
        public string OriginalPO { get; set; }
        public byte[] Ecf_nobarcode { get; set; }
        public string ecfdocsubtype { get; set; }
        public string Invoice_powono { get; set; }
        public string Invoice_type { get; set; }
        public string Expense_type { get; set; }

        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Title { get; set; }
        public string Department { get; set; }
        public string LocationCode { get; set; }
        public string Header { get; set; }
        public string dclnotesub { get; set; }
        public string dclnoteapp { get; set; }
    }
    public class InexDataModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ECFNo { get; set; }
        public int ECFId { get; set; }
        public int Docgid { get; set; }
        public string Docname { get; set; }
        public int DocSubgid { get; set; }
        public string DocSubname { get; set; }
        public int StatusGid { get; set; }
        public string statusname { get; set; }
        public string SupplierorEmployee { get; set; }
        public string SupplierorEmployeename { get; set; }
        public string ECFRaiser { get; set; }
        public string DocTypeName { get; set; }
        public string DocSubTypeName { get; set; }
        public string CreateMode { get; set; }
        public string ClaimMonth { get; set; }
        public string ECFAmount { get; set; }
        public string Despatchdate { get; set; }
        public string CourierName { get; set; }
        public string AWBNo { get; set; }
        public string ECFStatus { get; set; }
        public string CancelBy { get; set; }
        public string CancelDate { get; set; }
        public string ECFDate { get; set; }
        public string Command { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyAmount { get; set; }
        public string CurrencyRate { get; set; }
        public string ReducedAmount { get; set; }
        public string ProcessedAmount { get; set; }
        public string EcfRemark { get; set; }
        public string Reason { get; set; }
        public SelectList SelectDocSubType { get; set; }
        public SelectList SelectCourier { get; set; }
        public SelectList selectdoctypedata { get; set; }
        public SelectList statusnameData { get; set; }
    }
}