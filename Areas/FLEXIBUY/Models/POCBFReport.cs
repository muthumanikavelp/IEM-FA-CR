using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace IEM.Areas.FLEXIBUY.Models
{
    public class CBFPRPARDetails
    {
        public int cbfGid { get; set; }
        public string cbfNo { get; set; }
        public string PRNo { get; set; }
        public string PARNo { get; set; }
        public string PRPARAmount { get; set; }
        public string cbfDate { get; set; }
        public string cbfMode { get; set; }
        public string cbfEnddate { get; set; }
        public string cbfRequestfor { get; set; }
        public string fincon_Bugt { get; set; }
        public decimal cbfAmount { get; set; }
        public decimal cbfDevi_Amount { get; set; }
        public string cbfProjectOwner { get; set; }
        public string cbfStatus { get; set; }
        public string cbfDescription { get; set; }
        public string delmatRemarks { get; set; }
        public string UOM { get; set; }
        public string Status { get; set; }
        public decimal Qty { get; set; }
        public decimal UnitAmount { get; set; }
        public List<PODetails> PoDetails { get; set; }

    }
    public class PODetails
    {
        public int poheaderGid { get; set; }
        public string Pono { get; set; }
        public string ProductGroup { get; set; }
        public string ProductDesc { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string UOM { get; set; }
        public decimal Qty { get; set; }
        public decimal UnitAmount { get; set; }
        public string PoStatus { get; set; }
        public string vendorName { get; set; }
        public string ExpCode { get; set; }
        public string ExpSubCode { get; set; }
        public string POAmount { get; set; }
        public string POBranch { get; set; }
        public string POLocation { get; set; }
        public string Department { get; set; }
        public string CC { get; set; }
        public string CapexBudgetLineNo { get; set; }
        public string GRNNumber { get; set; }
        public string ProjectManager { get; set; }
        public List<ECFDetails> ECFDetails { get; set; }

    }
    public class ECFDetails
    {
        public int EcfGid { get; set; }
        public string ECFNo { get; set; }
        public string ECFDate { get; set; }
        public string InvDesc { get; set; }
        public string EcfDesc { get; set; }
        public decimal EcfAmount { get; set; }
        public decimal EcfProcessedAmount { get; set; }
        public string RaiserID { get; set; }
        public string RaiserDepartment { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string InvoiceServiceMonth { get; set; }
        public string InvoiceDesc { get; set; }
        public string PayMode { get; set; }
        public string PayDate { get; set; }
        public string EcfStatus { get; set; }
        public string EcfQueue { get; set; }
    }

}