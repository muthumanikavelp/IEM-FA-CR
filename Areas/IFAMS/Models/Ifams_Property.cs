using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IEM.Areas.IFAMS.Models
{
    //public class Ifams_Property
    //{
    //    public string assetgid { get; set; }
    //    public string assetCode { get; set; }
    //    public string assetDesc { get; set; }

    //    public SelectList assetCategory { get; set; }
    //    public int assetCategoryId { get; set; }
    //    public string assetCategoryName { get; set; }

    //    public SelectList assetSubCategory { get; set; }
    //    public int assetSubCategoryId { get; set; }
    //    public string assetSubCategoryName { get; set; }

    //    public string glCode { get; set; }

    //    public SelectList classfication { get; set; }
    //    public int classficationId { get; set; }
    //    public string classficationName { get; set; }

    //    public SelectList ownedBy { get; set; }
    //    public int ownedById { get; set; }
    //    public string ownedByName { get; set; }


    //    public string assetDescType { get; set; }
    //    public string uLife { get; set; }
    //    public string descRate { get; set; }
    //    public string descGlCode { get; set; }
    //    public string descResGlCode { get; set; }
    //    public string Mandatory { get; set; }
    //    public string NonMantadatory { get; set; }
    //    public string Verifiable { get; set; }
    //    public string NonVerifiable { get; set; }
    //    public bool Quantified { get; set; }
    //    public bool Barcode { get; set; }

    //    public string SLM { get; set; }
    //    public string LPM { get; set; }
    //    public int umonths { get; set; }
    //    public List<Ifams_Property> AssetDetails { get; set; }
    //    public string Status { get; set; }
    //}

    //public class capitalizationMaker
    //{
    //    public int Ecfgid { get; set; }
    //    public string EcfNo { get; set; }
    //    public string Ecfdate { get; set; }
    //    public string EcfAmount { get; set; }
    //    public string Vendor { get; set; }
    //   public List<capitalizationMaker> CapitalizationList { get; set; }


    //}
    public class Ifams_Property
    {
        public string assetgid { get; set; }
        public string assetCode { get; set; }
        public string assetDesc { get; set; }

        public SelectList assetCategory { get; set; }
        public int assetCategoryId { get; set; }
        public string assetCategoryName { get; set; }

        public SelectList assetSubCategory { get; set; }
        public int assetSubCategoryId { get; set; }
        public string assetSubCategoryName { get; set; }

        public string glCode { get; set; }

        public SelectList classfication { get; set; }
        public int classficationId { get; set; }
        public string classficationName { get; set; }

        public SelectList ownedBy { get; set; }
        public int ownedById { get; set; }
        public string ownedByName { get; set; }
        public string Movable { get; set; }
        public string Immovable { get; set; }

        public string assetDescType { get; set; }
        public string uLife { get; set; }
        public string descRate { get; set; }
        public string descGlCode { get; set; }
        public string descResGlCode { get; set; }
        public string Mandatory { get; set; }
        public string NonMantadatory { get; set; }
        public string Verifiable { get; set; }
        public string NonVerifiable { get; set; }
        public bool Quantified { get; set; }
        public bool Barcode { get; set; }

        public string BarcodeIsMandatory { get; set; }
        public string IsQuantified { get; set; } 

        public string SLM { get; set; }
        public string LPM { get; set; }
        public int umonths { get; set; }
        public List<Ifams_Property> AssetDetails { get; set; }
        public string Status { get; set; }
        // add by sugumar
        public Int32 Hsn_gid { get; set; }
        public string hsn_code { get; set; }
        public string hsn_desc { get; set; }
        public SelectList GetHsncode { get; set; }
        public int[] SelectedItemIds { get; set; }
        public MultiSelectList Items { get; set; }
    }

    public class capitalizationMaker
    {
        public int Ecfgid { get; set; }
        public string EcfNo { get; set; }
        public string Ecfdate { get; set; }
        public string EcfAmount { get; set; }
        public string Vendor { get; set; }
        public List<capitalizationMaker> CapitalizationList { get; set; }
        public List<capitalizationMaker> CapitalizationListGRN { get; set; }
        public List<capitalizationMaker> CapitalizationListbr { get; set; }
        public string invoicegid { get; set; }
        public string invoiceno { get; set; }
        public string invoicedate { get; set; }
        public decimal invoiceamount { get; set; }
        //public decimal invTotalamt { get; set; }
        public string ponumber { get; set; }
        public int pohgid { get; set; }
        public int podetgid { get; set; }
        public string orderedby { get; set; }
        public decimal orderdqty { get; set; }
        public decimal invoiceqty { get; set; }
        public decimal Receivedqty { get; set; }
        public string AssetCode { get; set; }
        public string AssetName { get; set; }
        public string AssetGlcode { get; set; }
        public int value { get; set; }
        public decimal Discount { get; set; }
        public string Narration { get; set; }
        public string ModelNo { get; set; }
        public string LocationCode { get; set; }
        public string Capitalization { get; set; }
        public string Manufacture { get; set; }
        public decimal Tax1 { get; set; }
        public decimal Tax2 { get; set; }
        public decimal othres { get; set; }
        public int net { get; set; }
        public string glcode { get; set; }
        public string Description { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public string ApproverRemarks { get; set; }
        public decimal Amount { get; set; }
        public List<capitalizationMaker> InvoiceList { get; set; }
        public List<capitalizationMaker> Podetailslist { get; set; }
        public List<capitalizationMaker> Itemlevellist { get; set; }
        public List<capitalizationMaker> Capitalizationgridlist { get; set; }
        public List<capitalizationMaker> Addtaxlist { get; set; }
        public List<capitalizationMaker> Getapplist { get; set; }
        public List<capitalizationMaker> Getdetapplist { get; set; }
        public List<capitalizationMaker> GetApproveList { get; set; }
        public string GRNNo { get; set; }
        public string GRNDate { get; set; }
        public string GRNStaus { get; set; }
        public string UOM { get; set; }
        public decimal Quantity { get; set; }
        public string DCNo { get; set; }
        public int Podetialsid { get; set; }
        public string Status { get; set; }
        public string locationName { get; set; }
        public int index { get; set; }
        public string AssetCategory { get; set; }
        public string shipmentType { get; set; }
        public decimal ecfamt { get; set; }
        public SelectList ddlCategory { get; set; }
        public SelectList ddlSubcategory { get; set; }
        public SelectList ddlbranch { get; set; }
        public string ddllocacode { get; set; }
        public int ddllocaid { get; set; }
        public string AssetSubcategpry { get; set; }
        public int Assetsubcategoryid { get; set; }
        public string Assetcategpry { get; set; }
        public int Assetcategoryid { get; set; }
        public string commend { get; set; }
        public decimal Already_cap { get; set; }
        public decimal Available_cap { get; set; }
        public decimal Pending_cap { get; set; }
        public int Grn_gid { get; set; }
        public int Grn_detgid { get; set; }
        public string TaxName { get; set; }
        public string TaxSubTypeName { get; set; }
        public string Invoicetaxglno { get; set; }
        public decimal Invoicetaxamount { get; set; }
        public decimal Already_Capiptal { get; set; }
        public decimal Cap_Now { get; set; }
        public decimal Already_Exp { get; set; }
        public decimal Exp_Now { get; set; }
        public decimal Balance { get; set; }
        public int indextax { get; set; }
        public string AssetSrlno { get; set; }
        public string Assetmfn { get; set; }
        public decimal Expense { get; set; }
        public decimal Exp_sum { get; set; }
        public string view { get; set; }
        public string ddlstatus { get; set; }
        public string Remarks { get; set; }
        public string CAPDATE { get; set; }
        public string DEPRATE { get; set; }
        public int branchgid { get; set; }
        public string SplitedTaxAmnt { get; set; }
        public string br_st_date { get; set; }
        public string lpm_st_date { get; set; }
        public string lpm_ed_date { get; set; }




        public int Ecfgidg { get; set; }
        public string EcfNog { get; set; }
        public string Ecfdateg { get; set; }
        public string EcfAmountg { get; set; }
        public string invoicegidg { get; set; }
        public string invoicenog { get; set; }
        public decimal invoiceamountg { get; set; }
        public string Vendorg { get; set; }
        public string invoicedateg { get; set; }
        public string provider_location { get; set; }
        public string receiver_location { get; set; }

    }
}