using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IEM.Areas.EOW.Models
{
    public class EOW_Travel
    {

    }
    public class EOW_Employeelst
    {
        public int employeeGid { get; set; }
        public string empCode { get; set; }
        public string empName { get; set; }
        public string empbranch { get; set; }
        public string empgrade { get; set; }
        public string empfc { get; set; }
        public string empcc { get; set; }
        public string maingid { get; set; }
        public string sign { get; set; }
    }
    public class EOW_RCMEntries
    {
        public SelectList RCMEnteries { get; set; }
        public string Expsubcat_Name { get; set; }
        public string HsnCode { get; set; }
        public string DebitRCM_Type { get; set; }
        public string Taxable_Amount { get; set; }
        public decimal debitrcm_rate { get; set; }
        public decimal debitrcm_amount { get; set; }
        public string debitrcm_inputcreditgl { get; set; }
        public string debitrcm_reversechargegl { get; set; }
        public decimal debitInputcreditRate { get; set; }
        public decimal debitInputcreditAmt{ get; set; }
        public decimal Taxgst_InputCredit_Display_Rate { get; set; }
        public decimal Taxgst_RCM_Display_Rate { get; set; }
    }
    public class EOW_TravelClaim
    {
        //TravelClaim Basic Model

        public SelectList Raiser_Modedata { get; set; }
        public string Raiser_Code { get; set; }
        public string Raiser_Mode { get; set; }
        public string Raiser_Name { get; set; }
        public string ECF_Date { get; set; }
        public string Grade { get; set; }
        public string ECF_Amount { get; set; }
        public string ClaimMonth { get; set; }

        public string raisermodeId { get; set; }
        public string raisermodeName { get; set; }

        //TravelClaim Mode Model 
        public int TravelMode_GID { get; set; }
        //public string TravelMode_NatureofExpenses { get; set; }
        //public string TravelMode_ExpenseCategory { get; set; }
        //public string TravelMode_SubCategory { get; set; }
        public string Branch { get; set; }
        public string PlaceFrom { get; set; }
        public string PlaceTo { get; set; }
        public string ClaimPeriodFrom { get; set; }
        public string ClaimPeriodTo { get; set; }
        public string Rate { get; set; }
        public string Distance { get; set; }
        //public string FC { get; set; }
        //public string CC { get; set; }
        public string ProductCode { get; set; }
        public string OUCode { get; set; }
        public string Amount { get; set; }
        public string AmountINR { get; set; }

        public SelectList Exp_FCC { get; set; }
        public string FC { get; set; }
        public SelectList Exp_CCC { get; set; }
        public string CC { get; set; }
        public string Exp_FCCC { get; set; }
        public string travelDescription { get; set; }
        public string Traveltypes { get; set; }
        public string Travelclass { get; set; }

        public SelectList ExpNatureofExpdata { get; set; }
        public int NatureofExpensesId { get; set; }
        public string NatureofExpensesName { get; set; }

        public SelectList ExpCatdata { get; set; }
        public int ExpenseCategoryId { get; set; }
        public string ExpenseCategoryName { get; set; }

        public SelectList ExpSubCatdata { get; set; }
        public int SubCategoryId { get; set; }
        public string SubCategoryName { get; set; }


        public SelectList TravelModedata { get; set; }
        public int TravelModeId { get; set; }
        public string TravelModeName { get; set; }

        public SelectList TravelClassdata { get; set; }
        public int TravelClassId { get; set; }
        public string TravelClassName { get; set; }

        public SelectList Citydata { get; set; }
        public int TravelCityId { get; set; }
        public string TravelCityName { get; set; }

        public SelectList Citydatato { get; set; }
        public int TravelCitytoId { get; set; }
        public string TravelCitytoName { get; set; }

        public SelectList AssetCatList { get; set; }
        public int AssetCatId { get; set; }
        public string AssetCatName { get; set; }
        
        public SelectList AssetSubCatList { get; set; }
        public int AssetSubCatId { get; set; }
        public string AssetSubCatName { get; set; }

        public string ProdServCategory { get; set; }
        public string GLCode { get; set; }
        public string invoicepoitem_GID { get; set; }

        public SelectList HsncodeList { get; set; }
        public int TravelHsnid { get; set; }
        public string TravelHsnCode { get; set; }
        public string TravelHsnDesc { get; set; }
        public string InvGid { get; set; }

        // Gst Add Details
        public string GstApplicable { get; set; }
        public string Hsncode { get; set; }
        public string HsnDesc { get; set; }
        public string Subtax { get; set; }
        public decimal TaxableAmt { get; set; }
        public decimal GstRate { get; set; }
        public decimal TaxAmt { get; set; }
        public int InvoiceTaxGid { get; set; }
        public int HsnId { get; set; }
        //Ramya Added
        public string RCMFlag { get; set; }
    }

}