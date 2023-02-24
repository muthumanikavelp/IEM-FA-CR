using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.FLEXISPEND.Models
{
    public class FlexiSpend
    {
    }

    public class PInward
    {
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string CourierId { get; set; }
        public string AWBNo { get; set; }
    }

    public class Pullout
    {
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string DocInwNo { get; set; }
        public string DocRefNo { get; set; }
        public string DocType { get; set; }
        public string DocAmount { get; set; }
        public string ReqBy { get; set; }
    }

    public class PPullout
    {
        public string PloId { get; set; }
        public string RequestDate { get; set; }
        public string PulloutDate { get; set; }
        public string RequestBy { get; set; }
        public string InwardNo { get; set; }
        public string DocRefNo { get; set; }
        public string Reason { get; set; }
        public string PloRId { get; set; }
        public string ReturnDate { get; set; }
        public string Remarks { get; set; }
        public string ReturnBy { get; set; }
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

    public class InvoiceDebitLine
    {
        public string DebitlineGId { get; set; }
        public string ECFId { get; set; }
        public string invid { get; set; }
        public string expnaturegid { get; set; }
        public string expCatId { get; set; }
        public string expSubcatId { get; set; }
        public string ClaimFrm { get; set; }
        public string ClaimTo { get; set; }
        public string Desc { get; set; }
        public string CCCode { get; set; }
        public string FCCode { get; set; }
        public string OUCode { get; set; }
        public string ProductCode { get; set; }
        public string Amount { get; set; }
        public string IsRemoved { get; set; }
        public string hsngid { get; set; }
        public string rcm { get; set; }
    }

    public class RCMcheck
    {
        public string DebitlineGId { get; set; }
        public string ECFId { get; set; }
        public string invid { get; set; }
        public string hsngid { get; set; }
        public string ProviderLocation { get; set; }
        public string RcmFlag { get; set; }
        public string action { get; set; }

    }


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

    public class Attachment
    {
        public string EcfId { get; set; }
        public string Invoicegid { get; set; }
        public string AttachmentId { get; set; }
        public string AttachmentName { get; set; }
        public string AttachmentType { get; set; }
        public string Desc { get; set; }
        public string IsRemoved { get; set; }
    }

    public class ECFHeader
    {
        public string EcfId { get; set; }
        public string ReducedAmt { get; set; }
        public string ProcessedAmt { get; set; }
        public string PaymentNett { get; set; }
        public string Amort { get; set; }
        public string currencygid { get; set; }
        public string curencycode { get; set; }
        public string currencyrate { get; set; }
        public string currencyamt { get; set; }
        public string ecfamt { get; set; }
        public string ecfdesc { get; set; }
    }

    public class InvoiceHeader
    {
        public string EcfId { get; set; }
        public string InvId { get; set; }
        public string InvDate { get; set; }
        public string InvNo { get; set; }
        public string Desc { get; set; }
        public string Amount { get; set; }
        public string WOTaxAmount { get; set; }
        public string ServiceMonth { get; set; }
        public string Amort { get; set; }
        public string ProvisionFlag { get; set; }
        public string RetentionFlag { get; set; }
        public string rententionRate { get; set; }
        public string rententionAmount { get; set; }
        public string ReleaseOn { get; set; }
        public string IsVerify { get; set; }
        public string IsRemoved { get; set; }

        public string GstCharged { get; set; }
        public string ProviderLocation { get; set; }
        public string GstinVendor { get; set; }
        public string ReceiverLocation { get; set; }
        public string GstinFiccl { get; set; }
        public string Suppliergid { get; set; }
    }

    public class ARFDebit
    {
        public string ecfartgid { get; set; }
        public string ECFId { get; set; }
        public string advancetypegid { get; set; }
        public string promoinvoice { get; set; }
        public string pono { get; set; }
        public string cbfno { get; set; }
        public string CCCode { get; set; }
        public string FCCode { get; set; }
        public string OUCode { get; set; }
        public string ProductCode { get; set; }
        public string Amount { get; set; }
        public string liQdate { get; set; }
        public string desc { get; set; }
        public string IsRemoved { get; set; }

    }

    public class TravelDetails
    {
        public string ecftravelgid { get; set; }
        public string ecfId { get; set; }
        public string invId { get; set; }
        public string expnaturegid { get; set; }
        public string expCatId { get; set; }
        public string expSubcatId { get; set; }
        public string placefrom { get; set; }
        public string placeto { get; set; }
        public string claimperiodfrm { get; set; }
        public string claimperiodto { get; set; }
        public string trasportgid { get; set; }
        public string transportclassgid { get; set; }
        public string Distance { get; set; }
        public string Rate { get; set; }
        public string FCCode { get; set; }
        public string CCCode { get; set; }
        public string OUCode { get; set; }
        public string ProductCode { get; set; }
        public string Amount { get; set; }
        public string Desc { get; set; }
        public string IsRemoved { get; set; }
        public string hsngid { get; set; }
        public string rcm { get; set; }
    }

    public class EFT
    {
        public string MemoNo { get; set; }
        public string PVNo { get; set; }
        public decimal Amount { get; set; }
        public string PvDate { get; set; }
        public string BankName { get; set; }
        public string Beneficiary { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
        public string Remarks { get; set; }
    }

    public class WHTax
    {
        public string withholdtaxgid { get; set; }
        public string Invoicegid { get; set; }
        public string TaxId { get; set; }
        public string taxsubtypegid { get; set; }
        public string TaxRate { get; set; }
        public string TaxableAmt { get; set; }
        public string TaxAmount { get; set; }
        public string NetAmount { get; set; }
        public string IsRemoved { get; set; }
    }

    public class AmortSchedule
    {
        public string amortgid { get; set; }
        public string EcfId { get; set; }
        public string InvId { get; set; }
        public string supplierId { get; set; }
        public string amount { get; set; }
        public string glno { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string frequencygid { get; set; }
        public string split { get; set; }
        public string active { get; set; }
        public string isremoved { get; set; }
        public string Percentage { get; set; }
    }

    public class ChequeSearch
    {
        public string BatchNo { get; set; }
        public string PVNoFrom { get; set; }
        public string PVNoTo { get; set; }
        public string PayDateFrom { get; set; }
        public string PayDateTo { get; set; }
        public string ClaimType { get; set; }
        public string PVAmountFrom { get; set; }
        public string PVAmountTo { get; set; }
        public string EmpNameId { get; set; }
        public string EmpCodeId { get; set; }
        public string SuppCodeId { get; set; }
        public string SuppNameId { get; set; }
        public string UId { get; set; }
    }

    public class ChequeResult
    {
        public string SlNo { get; set; }
        public string PVDate { get; set; }
        public string PVNo { get; set; }
        public string PayMode { get; set; }
        public string PVAmount { get; set; }
        public string ClaimType { get; set; }
        public string EmployeeSupplierCode { get; set; }
        public string EmployeeSupplierName { get; set; }
        public string PaymentBatchNo { get; set; }
        public string PvId { get; set; }
        public string EmployeeSupplierId { get; set; }
    }

    public class AutoSuggesion
    {
        public string Id { get; set; }
        public string Combo { get; set; }
    }

    public class ChequePrinting
    {
        public string ChqDetails { get; set; }
        public string BankId { get; set; }
        public string ChqBookNo { get; set; }
        public string ChqNoFrom { get; set; }
        public string ChqNoTo { get; set; }
        public string ChqCount { get; set; }
        public string UId { get; set; }
    }

    public class ChequeList
    {
        public string CHQDate { get; set; }
        public string AmountInWords { get; set; }
        public string Beneficiary { get; set; }
        public string PvAmount { get; set; }
        public string PvNo { get; set; }
    }

    public class ChequePrintResult
    {
        public string Message { get; set; }
        public string Clear { get; set; }
        public List<ChequeList> chequelist { get; set; }

    }

    //public class SetEcfGID
    //{
    //    public string ecf_gid { get; set; }

    //    public string claimType { get; set; }
    //}

    #region Supplier Invoice Maker Classes
    public class ParentDetails
    {
        public string Id { get; set; }
        public string GroupName { get; set; }
        public List<ChildDetails> ChildGroup { get; set; }
    }

    public class ChildDetails
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Name { get; set; }
    }
    #endregion
}