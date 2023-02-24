using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.FLEXIBUY.Models
{
    public class PrintEntity
    {

    }
    public class CbfPrintEntity
    {
        public int CBfGid { get; set; }
        public string CBfMode { get; set; } 
        public string CBfNumber { get; set; }
        public string CBfDate { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; } 
        public string RaiserName { get; set; }
        public string RaiserCode { get; set; }
        public string Department { get; set; }
        public string BSCC { get; set; }
        public string City { get; set; }
        public string CityClass { get; set; }
        public string BranchType { get; set; }
        public string BranchClass { get; set; }
        public string CBFHeaderAmount { get; set; }
        public string CBFHeaderAmountInWords { get; set; } 
        public string CBFHeaderDevAmount { get; set; }
        public string BudgetedFlag { get; set; }
        public string Justification { get; set; }   

        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string UOM { get; set; }
        public string Qty { get; set; }
        public string UnitAmount { get; set; }
        public string TotalAmount { get; set; }
        public string ChartOfAccount { get; set; }
        public string FCCC { get; set; }
        public string BudgetLine { get; set; }

        public List<CbfPrintEntity> CBFDetailsList { get; set; }
        public List<ApprovalHistoryForPrint> ApprovalsList { get; set; } 
    }
    public class POPrintEntity 
    { 
        public int POGid { get; set; } 
        public string PONumber { get; set; }
        public string PODate { get; set; }
        public string RaiserName { get; set; }
        public string POAmount { get; set; }
        public string POAmountInWords { get; set; } 
        public string VendorName { get; set; }
        public string VendorNotes { get; set; } 
        public string RequestFor { get; set; }
        public string VendorAddress { get; set; }
        public string VendorCity { get; set; }
        public string VendorState { get; set; }
        public string VendorCountry { get; set; }
        public string VendorPinCode { get; set; }
        public string VendorPhone { get; set; }
        public string VendorFax { get; set; }
        public string cbfmode { get; set; }
        public string cbfbranch { get; set; }
        public string parno { get; set; }
        public string prno { get; set; }
        public string deptname { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string UOM { get; set; }
        public string Qty { get; set; }
        public string UnitAmount { get; set; }
        public string BaseAmount { get; set; } 
        public string Discount { get; set; }
        public string Tax { get; set; }
        public string OtherCharge { get; set; }
        public string fccc { get; set; } 
        public string POCBFNumber { get; set; }
        public string TermAndCondn { get; set; }
        public string AdditionalTermsAndCondition { get; set; }
        public int BranchCount { get; set; }
        public string PoVersion{ get; set; }
        public string AmendmentDate { get; set; }

        public List<POPrintEntity> PODetailsList { get; set; } 
        public List<ApprovalHistoryForPrint> ApprovalsList { get; set; }
        public List<ShipmentDetails> ShipmentList { get; set; }
        public string gstvalue { get; set; }
    }
    public class WOPrintEntity 
    {
        public int WOGid { get; set; } 
        public string WONumber { get; set; } 
        public string WODate { get; set; } 
        public string RaiserName { get; set; }
        public string WOAmount { get; set; }
        public string WOAmountInWords { get; set; }  
        public string VendorName { get; set; }
        public string RequestFor { get; set; }
        public string VendorAddress { get; set; }
        public string VendorCity { get; set; }
        public string VendorState { get; set; }
        public string VendorCountry { get; set; }
        public string VendorPinCode { get; set; }
        public string VendorPhone { get; set; }
        public string VendorFax { get; set; }
        public string VendorNotes { get; set; }
        public string fccc { get; set; }
        public string WOCBFNumber { get; set; } 
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string ServiceMonth { get; set; } 
        public string Percentage { get; set; } 
        public string TotalAmount { get; set; }
        public string UnitPrice { get; set; } 
        public string Qty { get; set; } 
        public string TermAndCondn { get; set; }
        public string AdditionalTermsAndCondition { get; set; }
        public string cbfmode { get; set; }
        public string cbfbranch { get; set; }
        public string parno { get; set; }
        public string prno { get; set; }
        public string deptname { get; set; }
        public string UOM { get; set; }
        public string WoVersion { get; set; }
        public string WoAmendmentDate { get; set; } 

        public List<WOPrintEntity> WODetailsList { get; set; }  
        public List<ApprovalHistoryForPrint> ApprovalsList { get; set; }
        public List<ShipmentDetails> ShipmentList { get; set; } 
    }
    public class ApprovalHistoryForPrint 
    {
        public string ApproverName { get; set; }
        public string ApprovalDate { get; set; }
        public string ApprovalStage { get; set; }
        public string Remarks { get; set; }
        public string EmpCode { get; set; }
    }
    public class ShipmentDetails
    {
        public string BranchName { get; set; } 
        public string BranchAddress { get; set; } 
        public string ShippedQty { get; set; } 
        public string ShipmentType { get; set; }
        public string ShipmentProdCode { get; set; }
        public string ShipmentprodName { get; set; }    
    }
    public class OBFDetails
    {
        public string BudgetLine { get; set; }
        public string CC { get; set; }
        public int SupplierGid { get; set; }

        public int OBFDetailGid { get; set; }
        public int OBFGid { get; set; }
        public string PARGid { get; set; }
        public string OBFProductGid { get; set; }
        public string OBFProductGroupGid { get; set; }
        public string OBFProductDesc{ get; set; }    
        public string OBFJustification { get; set; }
        public string OBFDescription { get; set; }
        public string OBFITType { get; set; }
        public string OBFRequestFor { get; set; }   
    }

}