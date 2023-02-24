using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IEM.Areas.EOW.Models
{
    public class EOW_Supplier
    {

    }
    public class EOW_Supplierinvoice
    {
        public string ecfraisergid { get; set; }
        public string ecfstatus { get; set; }
        public string Raiser_Code { get; set; }
        public string Raiser_Mode { get; set; }
        public string Raiser_Name { get; set; }
        public string ECF_Date { get; set; }
        public string Grade { get; set; }
        public string ECF_Amount { get; set; }
        public string ecfno { get; set; }

        public SelectList Raiser_Modedata { get; set; }
        public string raisermodeId { get; set; }
        public string raisermodeName { get; set; }

        public string ecfremark { get; set; }

        public string Suppliergid { get; set; }
        public string Suppliercode { get; set; }
        public string Suppliername { get; set; }

        public SelectList doctypedata { get; set; }
        public string DocId { get; set; }
        public string DocName { get; set; }

        public SelectList Currencydata { get; set; }
        public string CurrencyId { get; set; }
        public string CurrencyName { get; set; }

        public string Exrate { get; set; }
        public string Currencyamount { get; set; }

        public string amort { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string amortdec { get; set; }

        public int chkraiser_gid { get; set; }
        public int ecf_GID { get; set; }
        public int Queue_GID { get; set; }
        public int Doctypeid { get; set; }
        public string queueactionfor { get; set; }
        public string ecfdescription { get; set; }


        public Int32 expsubcat_gid { get; set; }
        public Int32 expsubcat_expnature_gid { get; set; }
        public int selectedexpsubcat_expnature_gid { get; set; }
        public Int32 expsubcat_expcat_gid { get; set; }
        public string expsubcat_code { get; set; }
        public string expsubcat_name { get; set; }
        public string expsubcat_help { get; set; }
        public string expsubcat_active { get; set; }
        public Int32 expsubcat_insert_by { get; set; }
        public DateTime expsubcat_insert_date { get; set; }
        public Int32 expsubcat_update_by { get; set; }
        public DateTime expsubcat_update_date { get; set; }
        public string expsubcat_isremoved { get; set; }
        public int selectedexpcat_gid { get; set; }
        public Int32 expcat_gid { get; set; }
        public string expcat_name { get; set; }
        public string expcat_code { get; set; }
        public SelectList Getexpcat { get; set; }
        public SelectList GetexpcatById { get; set; }

        public Int32 expnature_gid { get; set; }
        public string expnature_name { get; set; }
        public SelectList Getexpnature { get; set; }
        public SelectList GetexpnatureById { get; set; }
        //IEM_GST_PAHSE3
        public string ecf_Paymode { get; set; }
        public string CygnetFlag { get; set; }

    }
    public class EOW_SupplierModelgrid
    {
        public int Invoicegid { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceNo { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string TaxableAmount { get; set; }
        public string Provision { get; set; }
        public string SerMonth { get; set; }
        public string Retensionflg { get; set; }
        public string Retensionper { get; set; }
        public string Retensionamount { get; set; }
        public string Retensionrelse { get; set; }
        public string poAmount { get; set; }
        public string SupplierCode { get; set; }
        public string SupplierName { get; set; }
        public string potype { get; set; }
        public string AmountINR { get; set; }
        public string GstCharged { get; set; }
        public string ProviderLocation { get; set; }
        public string ReceiverLocation { get; set; }
        public string GstinVendor { get; set; }
        public string GstinFiccl { get; set; }
        public string GstCess { get; set; }
        public string suppliergid { get; set; }
        public string ecfgid { get; set; }
        public string invoice_Cygnet_Gid { get; set; }
    }
    public class EOW_Doctype
    {
        public string DocId { get; set; }
        public string DocName { get; set; }
    }
    public class EOW_Currency
    {
        public string CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public string Currencyrate { get; set; }
        public SelectList Currencydata { get; set; }
    }
    public class EOW_PO
    {
        public string invoiceGid { get; set; }
        public string POGid { get; set; }
        public string POdetlGid { get; set; }
        public string POdetldesc { get; set; }
        public string POdate { get; set; }
        public string PONo { get; set; }
        public string POAmount { get; set; }
        public string POMapAmount { get; set; }
        public string POStatus { get; set; }

        public string POUtilizedamount { get; set; }
        public string POApprovedStatus { get; set; }
        public string POBalamount { get; set; }

        public string POserprogid { get; set; }
        public string POserprocode { get; set; }
        public string POserprodesc { get; set; }
        public string POassetgl { get; set; }
        public string POorderqty { get; set; }
        public string POreceivedqty { get; set; }
        public string POinvoiceqty { get; set; }
        public string POcurrentqty { get; set; }
        public string POsermonyh { get; set; }
        public string POrate { get; set; }

        public string POreceivedqtyamt { get; set; }
        public string POinvoiceqtyamt { get; set; }

        public string grninwrdheadergid { get; set; }
        public string BranchType { get; set; }
        public string GRNNumber { get; set; }
        public string ReceivedQty { get; set; }

        public string grninwrddetgid { get; set; }
        public string grninwrddetassetsrlno { get; set; }
        public string grninwrddetmftname { get; set; }
        public string grninwrddetrecedqty { get; set; }
        public string grninwrddetrecedqtychk { get; set; }
        public string PoGstStatus { get; set; }
    }
    public class EOW_Rate
    {
        public string RateId { get; set; }
        public string RateName { get; set; }
    }
    public class sinvotax
    {
        public int TaxgId { get; set; }
        public string Intax_Taxid { get; set; }
        public string Intax_Tax { get; set; }
        public string Intax_Change { get; set; }
        public string Intax_Taxsubid { get; set; }
        public string Intax_Taxsubname { get; set; }
        public string Intax_Rate { get; set; }
        public string Intax_Taxableamt { get; set; }
        public string Intax_Taxamt { get; set; }

        public int TaxSubTypeID { get; set; }
        public string TaxSubTypeName { get; set; }
    }
    public class Tax
    {
        public int TaxId { get; set; }
        public string TaxName { get; set; }
    }
    public class Taxsubtype
    {
        public int TaxsubtypeId { get; set; }
        public string TaxsubtypeName { get; set; }
    }
    public class Rate
    {
        public int RateId { get; set; }
        public string RateName { get; set; }
        public int TaxId { get; set; }
        public virtual Tax Tax { get; set; }
    }
}