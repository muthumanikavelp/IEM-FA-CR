using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.FLEXIBUY.Models
{
    public class WOEntity
    {
        public int WOGid { get; set; }
        public string WORefNumber { get; set; }
        public string WODate { get; set; } 
        public int WORaiserGid { get; set; } 
        public string WORaiserName { get; set; }
        public int WOVendorGid { get; set; }
        public string WOVendorName { get; set; }
        public int WORequestForGid { get; set; }  
        public string WORequestFor { get; set; }  
        public string WOITType { get; set; }       //Application or Infrastructure
        public string WODescription { get; set; }
        public string WOAmount { get; set; }
        public string WOUtilAmount { get; set; }
         
        public int WODetailGid { get; set; }
        public int WOProductGid { get; set; }
        public int CBFDetailGid { get; set; }
        public int CBFHeaderGid { get; set; }
        public string CBFRefNumber { get; set; }

        public int WODetailParentGid { get; set; }
        public int WODetailChildGid { get; set; } 

        public string WOProdServiceFlag { get; set; }
        public string WODetailProductCode { get; set; }
        public string WODetailProductName { get; set; }
        public string WODetailProductDescription { get; set; }
        public string WODetailServiceMonth { get; set; }
        public string WODetailQty { get; set; }
        public string WODetailUnitAmount { get; set; } 
        public string WODetailTotalAmount { get; set; }
        public string WODetailPercentage { get; set; }  
        public string WODetailParentFlag { get; set; }
        public string WODetailDescription { get; set; } 

        public int TermCondGid { get; set; }
        public string TermCondContent { get; set; }
        public string AdditionalTermCondContent { get; set; }

        public string WOErrMsg { get; set; }

        public string WOFrequencyFrom { get; set; }  
        public string WOFrequencyTo { get; set; }
        public string WOProdParentFlag { get; set; } 
        public int ChildProductGroupGid { get; set; }
        public int ChildProductGid { get; set; }   
        public string ChildProductGroup { get; set; }
        public string ChildProductCode { get; set; }  
        public string ChildProductName { get; set; } 
        public string ChildProductDescription { get; set; }
        public string WODetailAmount { get; set; }
        
        public string WOChildTotalAmount { get; set; }
        public string WODetailQtyParent { get; set; }
        public string WODetailQtyChild { get; set; }
        public string WODetailServiceMonthParent { get; set; }
        public string WODetailDescriptionChild { get; set; }
        public string WODetailTax { get; set; }
        public string WODetailTaxAmount { get; set; }
        public string WODetailUOM { get; set; }
        public string WODetailDiscount { get; set; }
        public string WODetailOthers { get; set; }

        public int WOShipmentGid { get; set; }  
        public int WOBranchGid { get; set; } 
        public string WOBranchName { get; set; }
        public int WOBranchInchargeGid { get; set; }
        public string WOBranchInchargeName { get; set; }
        public int WOShipmentEmpGid { get; set; }
        public int WOShipmentTypeGid { get; set; }
        public string WOShipmentQty { get; set; } 
        public int EmployeeGid { get; set; }
        public string EmployeeName { get; set; }

        public string WOApprovalRemarks { get; set; }
        public string WOApprovedBy { get; set; }

        public string UtilizedWOAmount { get; set; }
        public string CBFQuantity { get; set; }

        public int TermsAndCondValue { get; set; }
        public string TermsAndCondText { get; set; }

        public int vendAddressId { get; set; }
    }  
    public class SCNInward
    {
        public int SCnInwardGid { get; set; } 
        public string SCNRefNumber { get; set; } 
        public string SCNDate { get; set; } 
        public int SCNRaiserGid { get; set; } 
        public string SCNRaiserName { get; set; } 
        public int SCNVendorGid { get; set; }
        public string SCNVendorName { get; set; } 
        public int WOGid { get; set; } 
        public string WORefNumber { get; set; } 
        public string SCNDocNumber { get; set; }    
        public string SCNInvoiceNumber { get; set; } 
        public string SCNDescription { get; set; }
        public string SCNBranchType { get; set; }

        public int WODetailGid { get; set; }
        public string WOProductCode { get; set; }
        public string WOProductName { get; set; }
        public string WOProdServiceFlag { get; set; }
        public string WOProdDesc { get; set; }
        public string WOUOM { get; set; }
        public string ServiceMonth { get; set; }
        public string TotalAmount { get; set; }
        public string WODescription { get; set; }
        public string WOQty { get; set; }
        public string WOQtyOrg { get; set; }
        public string SCNReleaseBalanceQty { get; set; }
        public string WOReleasedDate { get; set; }
        public string[] SelectedItems { get; set; }
        public string[] ReceivedQty { get; set; }
        public string[] ReceivedRows { get; set; }
        public string[] IsAssetSerial { get; set; }  
    }
    public class AuditTrailWO
    {
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
}