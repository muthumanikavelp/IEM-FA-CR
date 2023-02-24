using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.FLEXIBUY.Models
{
    public class CBFHeader
    {
        public string CBFHeaderGId { get; set; }
        public string CBFDate { get; set; }
        public string CBFEndDate { get; set; }
        public string PARPRHeaderGId { get; set; }
        public string DeviationAmount { get; set; }
        public string ProjectOwnerId { get; set; }
        public string BranchId { get; set; }
        public string ReqBy { get; set; }
        public string CBFMode { get; set; }
        public string IsBudgeted { get; set; }
        public string CBFApproval { get; set; }
        public string BranchType { get; set; }
        public string BudgetOwnerId { get; set; }
        public string Description { get; set; }
        public string Justification { get; set; }
        public string IsRemoved { get; set; }
    }

    public class CBFDetails
    {
        public string CBFDetailGId { get; set; }
        public string CBFHeaderGId { get; set; }
        public string PARPRDetailGId { get; set; }
        public string PARPRDesc { get; set; }
        public string ProductGId { get; set; }
        public string ProductGroupGId { get; set; }
        public string Remarks { get; set; }
        public string UOMGId { get; set; }
        public string Qty { get; set; }
        public string UnitPrice { get; set; }
        public string TotalAmount { get; set; }
        public string ChartOfAcc { get; set; }
        public string FccCode { get; set; }
        public string BudgetLine { get; set; }
        public string IsRemoved { get; set; }
        public string IsContigency { get; set; }
    }

    public class CBFAttachment
    {
        public string RefGId { get; set; }
        public string AttachmentId { get; set; }
        public string AttachmentName { get; set; }
        public string Description { get; set; }
        public string RefFlag { get; set; }
        public string IsRemoved { get; set; }
    }

    public class AuditTrail
    {
        public string EmployeeName { get; set; }
        public string ActionDate { get; set; }
        public string Approval { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    }
}