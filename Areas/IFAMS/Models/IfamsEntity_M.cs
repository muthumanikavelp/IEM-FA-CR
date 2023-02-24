using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IEM.Areas.IFAMS.Models
{
    public class IfamsEntity_M
    {
    }

    public class SaleMakerModel
    {
        //soaheader table
        public Int32 soaGid { get; set; }
        public DateTime soaProcessdate { get; set; }
        public string soaSalenumber { get; set; }
        public decimal soaSalevalue { get; set; }
        public string soaSaledate { get; set; }
        public Int32 soaLocgid { get; set; }
        public Int32 soaJobcode { get; set; }
        public string soaUploadfilename { get; set; }
        public DateTime soaUploasdate { get; set; }
        public Int32 soaMakerid { get; set; }
        public DateTime soaMakerdate { get; set; }
        public Int32 soaCheckerid { get; set; }
        public DateTime soaCheckerdate { get; set; }
        public String soaisremoved { get; set; }
        public Int32 soaNorecords { get; set; }
        public string soaStatus { get; set; }
        public string saleSheetname { get; set; }
        public Int32 saleSheetid { get; set; }
        public string soaVendorname { get; set; }
        public string soavendoradd { get; set; }
        public string soaVatamt { get; set; }
        public string soaVatper { get; set; }
        public string soaProportion { get; set; }
        public Int32 soaTnorecords { get; set; }
        public decimal soaWtaxamt { get; set; }
        public decimal soaRectifAmt { get; set; }
        public string soacaptilisationdat { get; set; }
        public string takenby { get; set; }

        //Assetdetails
        public string assetdetDetid { get; set; }
        public string assetdetCode { get; set; }
        public string assetdetDescription { get; set; }
        public Int32 assetdetLocationcode { get; set; }
        public decimal assetdetAssetvalue { get; set; }
        public Int32 assetdetSaleid { get; set; }
        public Int32 assetdetgid { get; set; }
        public Int32 assetgid { get; set; }
        public decimal assetdettrfval { get; set; }
      //  public Int64 assetdetgroupid { get; set; }
        public string assetdetgroupid { get; set; }

        //Soadetail
        public decimal soadetSalevalue { get; set; }
        public decimal soadetVatvalue { get; set; }
        public decimal soadetWdvalue { get; set; }
        public decimal soadetProfitloss { get; set; }
        public decimal soadetIvvalue { get; set; }
        public Int32 soadetsoaheadgid { get; set; }
        public Int32 soadetgid { get; set; }
        public Int32 soadetassetdetgid { get; set; }
        public decimal saleamt { get; set; }
        public string soadetinwno { get; set; }


    //soapayment
        public Int32 soapayGid { get; set; }
        public Int32 soapaySoaheadergid { get; set; }
        public string soapaySaleno { get; set; }
        public string soapayChqno { get; set; }
        public string soapayAmount { get; set; }
        public string soapayRealizationdate { get; set; }
        public string soapayIsremoved { get; set; }
        public string soapaychqdat { get; set; }

        //Attach
        //public Int32 saattachGid { get; set; }
        //public DateTime saattachDate { get; set; }
        //public string saattachFilename { get; set; }
        //public string saattachDescription { get; set; }
        //public Int32 saattachemplyGid { get; set; }
        public List<Attachment> attach_list { get; set; }
        public string attach_file { get; set; }
        public string attach_date { get; set; }
        public string attach_desc { get; set; }


        //status table
        public string statname { get; set; }
        public string vatpercentage { get; set; }

        //Assetcategory
        public string assetcategory { get; set; }
        public string assetsubcategory { get; set; }

        //Deprecation
        public string assetdep { get; set; }


       public  List<SaleMakerModel> TModel { get; set; }


       public List<SaleMakerModel> auditTrailLst { get; set; }
       public int employee_gid { get; set; }
       public string employee_code { get; set; }
       public string employee_name { get; set; }
       public string action_status { get; set; }
       public string action_remark { get; set; }
       public string action_date { get; set; }
       public string action_remarks { get; set; }
       public string number { get; set; }
       public string ref_number { get; set; }
       public string ref_flag { get; set; }
       public string approval_stage { get; set; }
       public string queue_from { get; set; }
       public string queue_to { get; set; }
       public string queue_to_type { get; set; }
       public int gid { get; set; }
       public int queue_gid { get; set; }

       public SelectList VatModel { get; set; }
       public SelectList HsnDetails { get; set; }
       public List<HsnData> HsnModel { get;set;} 
       public Int32 vatid { get; set; }
       public string vatstate { get; set; }

       public List<Salemakermodelgst> Tmodelgst { get; set; }

       //Customer table
       public int customer_gid { get; set; }
       public int contact_gid {get; set; }
       public string customer_code { get; set; }
       public string customer_name { get; set; }
       public int state_gid { get; set; }
       public string Address { get; set; }
       public string State { get; set; }
       public string District { get; set; }
       public string Pincode { get; set; }
       public string gstnumber { get; set; }
       public string gstcharged { get; set; }
       public int provider_stategid { get; set; }
       public string provider_statename { get; set; }
       public string provider_gstnumber { get; set; }


       // Hsn Code 
       public int hsn_gid { get; set; }
       public string hsn_code { get; set; }
       public string hsn_description { get; set; }
       public string taxsubtype { get; set; }
       public decimal taxrate { get; set; }
       public decimal amount { get; set; }
       public decimal taxamount { get; set; }

       // For Branch Details 

       public int branch_gid { get; set; }
       public string branch_code { get; set; }
       public string branch_name { get; set; }
       public int branch_state_gid { get; set; }
       public int branch_district_gid { get; set; }
       public string branch_pincode { get; set; }
       public int branch_pincode_gid { get; set; }

       // For GST 

       public decimal cgst_rate { get; set; }
       public decimal sgst_rate { get; set; }
       public decimal igst_rate { get; set; }
       public decimal cgst_amount { get; set; }
       public decimal sgst_amount { get; set; }
       public decimal igst_amount { get; set; }
       public decimal cgcess_rate { get; set; }
       public decimal sgcess_rate { get; set; }
       public decimal igcess_rate { get; set; }
       public decimal cgcess_amount { get; set; }
       public decimal sgcess_amount { get; set; }
       public decimal igcess_amount { get; set; }

    }
    public class SaleSheetData
    {
        public Int32 saleSheetid { get; set; }
        public string saleSheetname { get; set; }
    }

    public class VatModel
    {
        public string vatid { get; set; }
        public string vatstate { get; set; }
        public List<VatModel> vatlist { get; set; }
    }


    public class Salemakermodelgst
    {
        public int hsn_gid { get; set; }
        public string hsn_code { get; set; }
        public string hsn_description { get; set; }
        public string taxsubtype { get; set; }
        public decimal taxrate { get; set; }
        public decimal amount { get; set; }
        public decimal taxamount { get; set; }

    }
    public class SearchCustomer
    {
        public int customer_gid { get; set; }
        public string customer_code { get; set; }
        public string customer_name { get; set; }
        public string customer_address { get; set; }
        public string pincode_code { get; set; }
        public int state_gid { get; set; }
        public string state_name { get; set; }
        public int district_gid { get; set; }
        public string district_name { get; set; }
        public string Gstin_number { get; set; }
        public int pincode_gid { get; set; }
        public int contact_gid { get; set; }

    }

    public class HsnData
    {
        public int hsn_gid { get; set; }
        public string hsn_code { get; set; }
    }

}