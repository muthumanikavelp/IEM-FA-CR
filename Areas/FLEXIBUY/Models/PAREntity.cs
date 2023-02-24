using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.FLEXIBUY.Models
{
    public class PAREntity
    {
        public int PARGid { get; set; }
        public string PARFinalApprovalDate { get; set; }
        public string PARPeriod { get; set; }
        public string PARContigency { get; set; }
        public string PARContigencyAmount { get; set; }
        public string PARDescription { get; set; }
        public string PARBudgetedFlag { get; set; }
        public string PARAmount { get; set; }
        public string Status { get; set; }

        public int PARDetailGid { get; set; } 
        public int RequestForGid { get; set; }
        public string PARExpenseType { get; set; }
        public string PARDetailPeriod { get; set; }
        public string PARDetailAmount { get; set; }
        public string PARDetailDescription { get; set; }

        public int PARApproverGid { get; set; } 
        public string PARApproverName { get; set; }
        public string PARApproverDate { get; set; }

        public string DeleteFor { get; set; }

        public string PARApprovedBy { get; set; } 
        public string PARApprovalRemarks { get; set; }
    }
}