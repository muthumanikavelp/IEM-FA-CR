using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.EOW.Models
{
    public class RetentionInvoiceReportEntity
    {
    }
    public class EOW_RetentionInvoiceReport
    {
        public string invoice_type { get; set; }
        public string invoice_date { get; set; }
        public string invoice_service_month { get; set; }
        public string invoice_desc { get; set; }
        public decimal invoice_amount { get; set; }
        public decimal invoice_wotax_amount { get; set; }
        public string invoice_payment_nett { get; set; }
        public string invoice_dedup_no { get; set; }
        public int invoice_dedup_status { get; set; }
        public string Description { get; set; }
        public string invoice_provision_flag { get; set; }
        public string invoice_retention_flag { get; set; }       
        public decimal invoice_retention_rate { get; set; }
        public decimal invoice_retention_amount { get; set; }
        public decimal invoice_retention_exception { get; set; }
        public int invoice_retention_curr_status { get; set; }
        public int invoice_retention_status { get; set; }
        public string invoice_retention_releaseon { get; set; }
        public string supplierheader_suppliercode { get; set; }
        public string supplierheader_name { get; set; }
        public decimal retentionrelease_amount { get; set; }
        public string retentionrelease_on { get; set; }
        public string invoice_no { get; set; }
        public string ecf_no { get; set; }
        public string employee_name { get; set; }
    }
}