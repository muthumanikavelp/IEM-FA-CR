using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IEM.Areas.EOW.Models
{
    public class CygnetSearchModel  
    {
       public Int64 Cygnet_Gid { get; set; }
       public string Cygnet_InvoiceNo { get; set; }
       public string Cygnet_InvoiceDate { get; set; }
       public string Cygnet_InvoiceFromDate { get; set; }
       public string Cygnet_InvoiceToDate { get; set; }
       public string Cygnet_InvoiceAmt { get; set; }
       public string Cygnet_SupplierName { get; set; }
       public string Cygnet_SupplierCode { get; set; }
       public string Cygnet_SupplierPanNo { get; set; }
       public string Cygnet_Supplier_GSTNNo { get; set; }
       public string Cygnet_HSNCode { get; set; }
       public string Cygnet_ProviderLocation { get; set; }
       public string Cygnet_ReceiverLocation { get; set; }
       public string Cygnet_InvoiceType { get; set; }
       public string Cygnet_TaxableAmt { get; set; }
       public string Cygnet_CGSTAmt { get; set; }
       public string Cygnet_SGSTAmt { get; set; }
       public string Cygnet_IGSTAmt { get; set; }
       public string Cygnet_POWO_Gids { get; set; }
       public string Cygnet_InvoiceStatus { get; set; }
       public string InvoiceSupplier_gid { get; set; }
       public string PO_Nos { get; set; }
       public string Cygnet_Supplier { get; set; }
       public string Cygnet_Provider { get; set; }
       public string Cygnet_Receiver { get; set; }
       public string Cygnet_Provider_GSTN { get; set; }
       public string Cygnet_Receiver_GSTN { get; set; }
       public List<CygnetSearchModel> CygModel { get; set; }
    }
}