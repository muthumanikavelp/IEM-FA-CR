using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.EOW.Models
{
    public class RetentiorealeaseReportEntity
    {
    }
    public class Eow_RetentionRealeaseReport
    {
        public int invoice_gid { get; set; }
        public int retentionrelease_gid { get; set; }
        public string retentionrelease_date { get; set; }
        public string supplierheader_suppliercode { get; set; }
        public string supplierheader_name { get; set; }
        public decimal retentionrelease_amount { get; set; }
        public string retentionrelease_on { get; set; }
        public Decimal retention_amount { get; set; }
        public Decimal retention_releaseamount { get; set; }
        public Decimal retention_exception { get; set; }
        public string retention_expiry { get; set; }
        public string invoice_no { get; set; }
        public string ecf_no { get; set; }
        public string employee_name { get; set; }
        public string retentionrelease_cancel_by { get; set; }
        public string retentionrelease_cancel_date { get; set; }
        public string RetentionStatus { get; set; }


    }
}