using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IEM.Areas.EOW.Models
{
    public class ArfEntity
    {


    }
    public class EOW_arfraising
    {
        public string AmountINR { get; set; }
        public string AmountINRnew { get; set; }
        public int Exp_GID { get; set; }
        public int invoice_GID { get; set; }
        public int ecf_GID { get; set; }
        public int Queue_GID { get; set; }
        public int Doctypeid { get; set; }
     //   public string ECF_Amount { get; set; }
        public string raisermodeId { get; set; }

        public Int32 ecfarf_gid { get; set; }
        public Int32 ecfarf_ecf_gid { get; set; }

        public string ecfarf_advancetype { get; set; }
        public string ecf_ecf_gid { get; set; }
        public Int32 ecfarf_advancetype_gid { get; set; }
        public string ecfarf_dr_gl_no { get; set; }
        public string ecfarf_pi_gl_no { get; set; }
        public string ecfarf_desc { get; set; }
        public string ecfarf_amount { get; set; }
        public string ecfarf_exception { get; set; }
        public string ecfarf_po_no { get; set; }
        public string PONo { get; set; }
        public int POid { get; set; }
        public string pototal { get; set; }
        public string ecfarf_cbf_no { get; set; }
        public string ecfarf_fc_code { get; set; }
        public string ecfarf_cc_code { get; set; }
        public string ecfarf_product_code { get; set; }
        public string ecfarf_ou_code { get; set; }
        public string ecfarf_liq_date { get; set; }
        public string ecfarf_promo_invoice { get; set; }
        public string btnupdate { get; set; }

       //getcreditlineinformation
        public Int32 creditline_gid { get; set; }
        public string creditline_ecf_gid { get; set; }
        public string creditline_pay_mode { get;set;}
        public string creditline_ref_no { get; set; }
        public string creditline_beneficiary { get; set; }
        public string creditline_desc { get; set; }
        public string creditline_amount { get; set; }
       
        public Int32 id { get; set; }
        public string name { get; set; }
        //attachementdetails
        public string attachment_type { get; set; }
        public Int32 attachment_lenth { get; set; }
        public Int32 attachment_gid { get; set; }
        public Int32 attachment_refgid { get; set; }
        public string attachment_filename { get; set; }
        public Int32 attachment_attachmenttype_gid { get; set; }
        public string attachment_desc { get; set; }
        public string attachment_date { get; set; }
        public Int32 attachment_by { get; set; }
        public string attachment_nameby { get; set; }
        //getemployeedetails
        public string employee_code { get; set; }
        public string employee_name { get; set; }

        //getemployeedetails
        public string supplierheader_suppliercode { get; set; }
        public string supplierheader_name { get; set; }
       

        public SelectList GetAdvancetype{ get; set; }
        public Int32 advancetype_gid { get; set; }
        public SelectList GetPaymode { get; set; }
        public SelectList GetAttach { get; set; }
        public SelectList GetBenificary { get; set; }
        public SelectList GetRef { get; set; }
        public Int32 paymode_gid { get; set; }
        public string payment_accountno { get; set; }
        public string payment_benificary { get; set; }

        //getecfdetails
        public string queueactionfor { get; set; }
        public string ecf_raisername { get; set; }
        public string ecf_no { get; set; }
        public string ecf_date { get; set; }
        public string ecf_raisersup { get; set; }
        public string ecf_raiser { get; set; }
        public string status { get; set; }
        public Decimal ecf_amount { get; set; }
        public string ecf_create_mode { get; set; }
        public int ecf_supplier_gid { get; set; }
        public int ecf_employee_gid { get; set; }
        public string ecf_raiser_gid { get; set; }

        public string arf_type { get; set; }
        public string grade { get; set; }

        public string arfremark { get; set; }

        public SelectList Getfc { get; set; }
        public SelectList Getcc { get; set; }
        public SelectList RefNoId{ get; set;}

    }
    public class GetAdvancetype
    {
        public Int32 ecfarf_advancetype_gid { get; set; }
        public Int32 advancetype_gid {get;set;}
        public string advancetype_name { get;set; }
    }
    public class GetBenificary
    {
        public string payment_benificary { get; set; }
    }
    public class GetRef
    {
        
        public string payment_accountno { get; set; }
    }
    public class GetPaymode
    {
        public Int32 paymode_gid { get; set; }
        public string paymode_name { get; set; }
        public string paymode_Code { get; set; }
        
    }
    public class GetAttach
    {
        public Int32 attachment_attachmenttype_gid { get; set; }
        public string attachment_type { get; set; }

    }
    public class Retention_Release
    {
        public int payrefergid { get; set; }
        public string BranchCode { get; set; }
        public string attachment_date { get; set; }
        public string attachment_nameby { get; set; }
        public string attachment_filename { get; set; }
        public Int32 attachment_gid { get; set; }
        public string attachment_desc { get; set; }
        public Int32 attachment_refgid { get; set; }
        public Int32 attachment_by { get; set; }
        public Int32 attachment_attachmenttype_gid { get; set; }
        public string attachment_type { get; set; }
        public Int32 attachment_attachmenttype_gid1 { get; set; }
        public string ReleaseDate { get; set; }
        public string ecf_suppliername { get; set; }
        public string ecf_no { get; set; }
        public string ecf_date { get; set; }
        public string Retention_date { get; set; }
        public string invoice_date { get; set; }
        public string ecf_suppliercode { get; set; }

        public string ecf_amount { get; set; }
        public string extendeddate { get; set; }
        public int invoice_ecf_gid { get; set; }
        public int invoice_gid { get; set; }
        public string invoice_retention_amount { get; set; }
        public int release_gid { get; set; }
        public SelectList GetAttach { get; set; }
        public string invoice_retention_exception { get; set; }
        public string invoice_no { get; set; }
        public string invoice_amount { get; set; }
        public string invoice_retention_releaseon { get; set; }
        public string extended_date { get; set; }

        public string release { get; set; }
        public string release_amount { get; set; }
        public string Retention_amount { get; set; }
        public string Balance_amount { get; set; }
        public string Balancerel_amount { get; set; }
        public string invoice_desc { get; set; }
        public string ecf_remarks { get; set; }
        public string release_remarks { get; set; }
        public int serialno { get; set; }

        public string payment_accountnocredit { get; set; }
        public int invoice_credit_gid { get; set; }
        public Int32 paymode_gidcredit { get; set; }
        public string paymode_namecredit { get; set; }
        public SelectList GetRefcredit { get; set; }
        public SelectList GetPaymodecredit { get; set; }
        public SelectList Getexpense { get; set; }
        public SelectList Getexpensesubcategory { get; set; }
        public SelectList Getexpensecategory { get; set; }
        public Int32 expense_gid { get; set; }
        public string expense_name { get; set; }
        public SelectList Getfc { get; set; }
        public Int32 fc_gid { get; set; }
        public string fc_name { get; set; }
        public SelectList Getcc { get; set; }
        public Int32 cc_gid { get; set; }
        public string cc_name { get; set; }
        public SelectList Getfccc { get; set; }
        public Int32 fccc_gid { get; set; }
        public string fccc_name { get; set; }
        public string product_code { get; set; }
        public string ou_code { get; set; }
        public Int32 expensecate_gid { get; set; }
        public Int32 expensesubcate_gid { get; set; }

        public string ecf_raiser { get; set; }
        public string creditpaymode { get; set; }
        public string creditref_no { get; set; }
        public string creditbecificary { get; set; }
        public string creditdesc { get; set; }
        public string creditamount { get; set; }
    }

    public class GetRefcredit
    {

        public string payment_accountnocredit { get; set; }
    }
    public class GetPaymodecredit
    {
        public Int32 paymode_gidcredit { get; set; }
        public string paymode_namecredit { get; set; }

    }
    public class Getexpensecategory
    {
        public Int32 expensecate_gid { get; set; }
        public string expensecate_name { get; set; }

    }
    public class Getexpensesubcategory
    {
        public Int32 expensesubcate_gid { get; set; }
        public string expensesubcate_name { get; set; }

    }
    public class Getexpense
    {
        public Int32 expense_gid { get; set; }
        public string expense_name { get; set; }

    }
    public class Getfc
    {
        public Int32 fc_gid { get; set; }
        public string fc_name { get; set; }

    }
    public class Getcc
    {
        public Int32 cc_gid { get; set; }
        public string cc_name { get; set; }
   
    }
    public class Getfccc
    {
        public Int32 fccc_gid { get; set; }
        public string fccc_name { get; set; }
       
    }
}