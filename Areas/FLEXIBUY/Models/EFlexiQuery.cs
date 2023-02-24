using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IEM.Areas.FLEXIBUY.Models
{
    public class EFlexiQuery
    {
        public Int32 DocId { get; set; }
        public string DocType { get; set;}
        public string RefNumber { get; set; }
        public string DocDate { get; set; }
        public decimal DocAmount { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string FinalApprovalDate { get; set; }
        public string ApprovalPending { get; set; }
        public string PendingCount { get; set; }
        public SelectList DocList { get; set; }
        public int doctypeid { get; set; }
        public string docname { get; set; }
        public string PendingDate { get; set; }
        public string QueueDate { get; set; }
        public string Aging { get; set; }
    }
}