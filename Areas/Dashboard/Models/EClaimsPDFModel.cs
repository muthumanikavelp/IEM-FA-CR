using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.Dashboard.Models
{
    public class EClaimsPDFModel
    {
        public string DocType { get; set; }
        public ECFEmployeeClaim EmployeeClaimDoc { get; set; }
        public LocalConveyanceType locConveyanceDoc { get; set; }
        public ECFSIClaim SIClaimDoc { get; set; }
        public DSASupplierType DSAClaimDoc { get; set; }
        public ARFEmployeeType ARFEmpClaimDoc { get; set; }
        public ARFSupplierType ARFSupClaimDoc { get; set; }
    }

    //DocSub-Type  [1, 3]
    public class ECFEmployeeClaim
    {
        public string Raiser { get; set; }
        public string ECFNo { get; set; }
        public string ECFAmount { get; set; }
        public string PeriodFrom { get; set; }
        public string PeriodTo { get; set; }
        public string ServiceMonth { get; set; }
        public string BranchName { get; set; }
        public string RaiserComments { get; set; }

        public string PreviousApprover { get; set; }
        public string PreviousApproverRemarks { get; set; }
        public List<EmployeeClaimInnerDetails> ExpenseList { get; set; }
    }
    public class EmployeeClaimInnerDetails
    {
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Amount { get; set; }
    }

    //DocSub-Type  [2]
    public class LocalConveyanceType
    {
        public string Raiser { get; set; }
        public string ECFNo { get; set; }
        public string ECFAmount { get; set; }
        public string NoOfPersons { get; set; }
        public string ServiceMonth { get; set; }
        public string HighestClaimAmount { get; set; }
        public string BranchName { get; set; }
        public string RaiserComments { get; set; }
        public string PreviousApprover { get; set; }
        public string PreviousApproverRemarks { get; set; }
    }

    //DocSub-Type  [4]
    //Supplier Invoice, PO & WO
    public class ECFSIClaim {
        public string Raiser { get; set; }
        public string Supplier { get; set; }
        public string ECFNo { get; set; }
        public string ECFAmount { get; set; }
        public string RaiserComments { get; set; }
        public string PreviousApprover { get; set; }
        public string PreviousApproverRemarks { get; set; }
        public string POType { get; set; }
        public List<ECFSIClaimInnerDetails> InvoiceList { get; set; }
    }
    public class ECFSIClaimInnerDetails
    {
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceAmount { get; set; }
        public string POWONo { get; set; }
        public string GSTApplicable { get; set; }
        public string TaxableAmount { get; set; }
        public string Taxtype { get; set; }
        public string TaxAmount { get; set; }
    }

    //DocSub-Type  [5]
    public class DSASupplierType
    {
        public string Raiser { get; set; }
        public string Supplier { get; set; }
        public string ECFNo { get; set; }
        public string ECFAmount { get; set; }
        public string NoOfPersons { get; set; }
        public string ServiceMonth { get; set; }
        public string RaiserComments { get; set; }
        public string PreviousApprover { get; set; }
        public string PreviousApproverRemarks { get; set; }
    }

    //DocSub-Type  [6, 8]
    public class ARFEmployeeType
    {
        public string Raiser { get; set; }
        public string ARFNo { get; set; }
        public string ARFAmount { get; set; }
        public string ARFType { get; set; }
        public string Advancetype { get; set; }
        public string TargetLiqDate { get; set; }
        public string BranchName { get; set; }
        public string ARFDescription { get; set; }

        public string RaiserComments { get; set; }
        public string PreviousApprover { get; set; }
        public string PreviousApproverRemarks { get; set; }
    }

    //DocSub-Type  [7]
    public class ARFSupplierType
    {
        public string Raiser { get; set; }
        public string SupplierEmployeeName {get;set;}
        public string ARFNo { get; set; }
        public string ARFAmount { get; set; }
        public string ARFType { get; set; }
        public string Advancetype { get; set; }
        public string TargetLiqDate { get; set; }
        public string BranchName { get; set; }
        public string ARFDescription { get; set; }
        public string PromoInvoice { get; set; }
        public string PONo { get; set; }
        public string RaiserComments { get; set; }
        public string PreviousApprover { get; set; }
        public string PreviousApproverRemarks { get; set; }
    }
}