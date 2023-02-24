using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.Dashboard.Models
{
    public class EProcurePDFModel
    {
        public string DocType { get; set; }
        public CBFPDFModel CBFDoc { get; set; }
        public POPDFModel PODoc { get; set; }
        public PARPDFModel PARDoc { get; set; }
        public PRPDFModel PRDoc { get; set; }
    }

    public class CBFPDFModel
    {
        public string CBFNo { get; set; }
        public string CBFDate { get; set; }
        public string Raiser { get; set; }
        public string RequestFor { get; set; }
        public string CBFAmount { get; set; }
        public string CBFMode { get; set; }
        public string PARPRNo { get; set; }
        public string PARPRAmt { get; set; }
        public string CBFApproval { get; set; }
        public string Budgeted { get; set; }
        public string Branch { get; set; }
        public string CBFJustification { get; set; }
        public string PreviousApprover { get; set; }
        public string PreviousApproverComment { get; set; }
        public List<CBFInnerDetails> ProductList { get; set; }
    }

    public class CBFInnerDetails
    {
        public string Type { get; set; }
        public string ProductServiceGroup { get; set; }
        public string ProductServiceName { get; set; }
        public string ProductServiceDesc { get; set; }
        public string UOM { get; set; }
        public string Amount { get; set; }
        public string Qty { get; set; }
        public string GLCode { get; set; }
        public string CC { get; set; }
        public string BudgetedLine { get; set; }
    }

    public class POPDFModel
    {
        public string PONo { get; set; }
        public string PODate { get; set; }
        public string Raiser { get; set; }
        public string Supplier { get; set; }
        public string CBFNo { get; set; }
        public string CBFLineAmount { get; set; }
        public string Mode { get; set; }
        public string PARPRNo { get; set; }
        public string BranchType { get; set; }
        public string BranchName { get; set; }
        public string CC { get; set; }
        public string POAmount { get; set; }        
        public string PreviousApprover { get; set; }
        public string PreviousApproverComment { get; set; }
        public List<POInnerDetails> ProductList { get; set; }
    }

    public class POInnerDetails
    {
        public string Code { get; set; }
        public string Product { get; set; }        
        public string Description { get; set; }
        public string UOM { get; set; }
        public string Qty { get; set; }
        public string UnitPrice { get; set; }
        public string Discount { get; set; }
        public string Tax { get; set; }
        public string Others { get; set; }
        public string TotalAmount { get; set; }
    }

    public class PARPDFModel
    {
        public string PARNo { get; set; }
        public string PARDate { get; set; }
        public string Raiser { get; set; }
        public string Budgeted { get; set; }
        public string PARAmount { get; set; }
        public string RaiserComment { get; set; }
        public List<PARInnerDetails0> ProductList0 { get; set; }
        public List<PARInnerDetails1> ProductList1 { get; set; }
    }

    public class PARInnerDetails0
    {
        public string Type { get; set; }
        public string FYFrom { get; set; }
        public string FYTo { get; set; }
        public string Amount { get; set; }
    }
    public class PARInnerDetails1
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Date { get; set; }
    }

    public class PRPDFModel
    {
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public string Raiser { get; set; }
        public string IsBudgeted { get; set; }
        public string Amount { get; set; }
        public string RequestFor { get; set; }
        public string ExpenseType { get; set; }
        public string BSCC { get; set; }
        public string BranchName { get; set; }
        
        public string RaiserComments { get; set; }
        public string PreviousApprover { get; set; }
        public string PreviousApproverComment { get; set; }
        public List<PRInnerDetails> ProductList { get; set; }
    }

    public class PRInnerDetails
    {
        public string Product { get; set; }
        public string Description { get; set; }        
        public string UOM { get; set; }
        public string Amount { get; set; }
        public string Qty { get; set; }
    }
}