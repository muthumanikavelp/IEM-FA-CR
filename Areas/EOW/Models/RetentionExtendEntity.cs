using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.EOW.Models
{
    public class RetentionExtendEntity
    {
    }
    public class Eow_RetentionExtend
    {
        public string ReleaseDate { get; set; }
        public string ecf_suppliername { get; set; }
        public string supplierheader_suppliercode { get; set; }
        public string supplierheader_name { get; set; }
        public string ecf_no { get; set; }
        public string ecf_date { get; set; }
        public string ecf_remark { get; set; }
        public string ecf_suppliercode { get; set; }
        public Decimal ecf_amount { get; set; }
        public string extendeddate { get; set; }
        public int invoice_ecf_gid { get; set; }
        public string retention_expiry { get; set; }
        public int invoice_gid { get; set; }
        public Decimal invoice_retention_amount { get; set; }
        public Decimal invoice_retention_exception { get; set; }
        public string invoice_no { get; set; }
        public Decimal invoice_amount { get; set; }
        public string invoice_desc { get; set; }
        public string invoice_retention_releaseon { get; set; }
        public string extended_date { get; set; }
        public string retention_date { get; set; }
        public int retention_invoice_gid { get; set; }
        public int retention_serialno { get; set; }
        public decimal retention_rate { get; set; }
        public decimal retention_amount { get; set; }
        public  decimal retention_releaseamount{get;set;}
        public decimal retention_exception { get; set; }
        public int retention_release_gid { get; set; }       
        public string retention_status { get; set; }
        public string retention_isactive { get; set; }
        public string invoice_date { get; set; }
        public string remarks { get; set; }

        public int invoice_retention_status{ get; set; }
        public Decimal release_amount { get; set; }
    }
}