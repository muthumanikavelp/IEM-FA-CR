using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.FLEXIBUY.Models
{
    public class PRNewEntity
    {
        public int PRGid { get; set; } 
        public string PRRefNumber { get; set; } 
        public string PRExpenseFlag { get; set; }
        public string PRDate { get; set; }
        public int PRFCCCGid { get; set; }
        public string RequestForName { get; set; } 
        public int RequestForValue { get; set; }
        public string PRProdServiceFlag { get; set; }
        public string PRBranchType { get; set; }       
        public string PRBranchGid { get; set; }
        public string PRDescription { get; set; }

        public int PRDetailGid { get; set; }
        public int ChildProductGroupGid { get; set; }
        public int ChildProductGid { get; set; }
        public int PRDetailUOM { get; set; }
        public string PRDetailQty { get; set; }
        public string PRDetailUnitAmount { get; set; }
        public string PRDetailTotalAmount { get; set; }
        public string PRDetailDescription { get; set; }

        public string PRApprovedBy { get; set; }
        public string PRApprovalRemarks { get; set; }

    }
    public class AuditTrailPR
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
    public class Attachments
    {
        public int AttachmentID { get; set; }
        public int AttachmentRefGid { get; set; } 
        public string AttachedFileName { get; set; }
        public string AttachDescription { get; set; }
        public string AttachDate { get; set; }
        public string AttachedActualFileName { get; set; }
        public string TempFileName { get; set; }
        public string AttachmentFor { get; set; }  
    }
}