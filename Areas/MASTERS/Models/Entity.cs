using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IEM.Areas.MASTERS.Models
{

    public class iem_mst_bank
    {
        public Int32 bank_gid { get; set; }
        public string bank_code { get; set; }
        public string bank_name { get; set; }
        public Int32 bank_insert_by { get; set; }
        public DateTime bank_insert_date { get; set; }
        public Int32 bank_update_by { get; set; }
        public DateTime bank_update_date { get; set; }
        public string bank_isremoved { get; set; }

    }

    public class iem_mst_courier
    {
        public Int32 courier_gid { get; set; }
        public string courier_name { get; set; }
        public Int32 courier_insert_by { get; set; }
        public DateTime courier_insert_date { get; set; }
        public Int32 courier_update_by { get; set; }
        public DateTime courier_update_date { get; set; }
        public string courier_isremoved { get; set; }

    }
    public class iem_mst_currency
    {
        public Int32 currency_gid { get; set; }
        public string currency_code { get; set; }
        public string currency_name { get; set; }
        public Int32 currency_insert_by { get; set; }
        public DateTime currency_insert_date { get; set; }
        public Int32 currency_update_by { get; set; }
        public DateTime currency_update_date { get; set; }
        public string currency_isremoved { get; set; }
        public string currencyrate_isremoved { get; set; }


    }
    public class iem_mst_currencyrate
    {
        public int currencyrate_currency_gid { get; set; }
        public Decimal currencyrate_value { get; set; }
        public string currencyrate_period_from { get; set; }
        public string currencyrate_period_to { get; set; }
        public int currencyrate_insert_by { get; set; }
        public string currencyrate_insert_date { get; set; }
        public int currencyrate_update_by { get; set; }
        public string currencyrate_update_date { get; set; }
        public int currencyrate_gid { get; set; }
        //public string currency_name { get; set; }
        public SelectList Getcurrency { get; set; }
        public int selectedcurrency_gid { get; set; }

        public Int32 currency_gid { get; set; }
        public string currency_code { get; set; }
        public string currency_name { get; set; }
    }



    public class iem_mst_region
    {
        public Int32 region_gid { get; set; }
        public string region_name { get; set; }
        public Int32 region_insert_by { get; set; }
        public DateTime region_insert_date { get; set; }
        public Int32 region_update_by { get; set; }
        public DateTime region_update_date { get; set; }
        public string region_isremoved { get; set; }

    }
    public class iem_mst_uom
    {
        public Int32 uom_gid { get; set; }
        public string uom_code { get; set; }
        public string uom_name { get; set; }
        public Int32 uom_insert_by { get; set; }
        public DateTime uom_insert_date { get; set; }
        public Int32 uom_update_by { get; set; }
        public DateTime uom_update_date { get; set; }
        public string uom_isremoved { get; set; }

    }
    public class iem_mst_bouncereason
    {
        public Int32 bouncereason_gid { get; set; }
        public string bouncereason_code { get; set; }
        public string bouncereason_name { get; set; }
        public string bouncereason_type { get; set; }
        public Int32 bouncereason_insert_by { get; set; }
        public DateTime bouncereason_insert_date { get; set; }
        public Int32 bouncereason_update_by { get; set; }
        public DateTime bouncereason_update_date { get; set; }
        public string bouncereason_isremoved { get; set; }

    }
    public class iem_mst_cityclass
    {
        public Int32 cityclass_gid { get; set; }
        public string cityclass_code { get; set; }
        public string cityclass_name { get; set; }
        public Int32 cityclass_insert_by { get; set; }
        public DateTime cityclass_insert_date { get; set; }
        public Int32 cityclass_update_by { get; set; }
        public DateTime cityclass_update_date { get; set; }
        public string cityclass_isremoved { get; set; }

    }
    public class iem_mst_tier
    {
        public Int32 tier_gid { get; set; }
        public string tier_code { get; set; }
        public string tier_name { get; set; }
        public Int32 tier_insert_by { get; set; }
        public DateTime tier_insert_date { get; set; }
        public Int32 tier_update_by { get; set; }
        public DateTime tier_update_date { get; set; }
        public string tier_isremoved { get; set; }

    }


    public class iem_mst_country
    {
        public Int32 currency_gid { get; set; }
        public string currency_code { get; set; }
        public string currency_name { get; set; }
        public SelectList Getcurrency { get; set; }
        public SelectList GetcurrencyById { get; set; }

        public int selectedcurrency_gid { get; set; }
        public Int32 country_gid { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public Int32 country_currency_gid { get; set; }
        public string country_currency_code { get; set; }
        public Int32 country_insert_by { get; set; }
        public DateTime country_insert_date { get; set; }
        public Int32 country_update_by { get; set; }
        public DateTime country_update_date { get; set; }
        public string country_isremoved { get; set; }

    }
    public class Getcurrency
    {
        public Int32 Currencygid { get; set; }
        public string Currencyname { get; set; }
        public string Currencycode { get; set; }
    }
    public class GetcurrencyById
    {
        public Int32 Currencygid { get; set; }
        public string Currencyname { get; set; }
        public string Currencycode { get; set; }
    }

    public class iem_mst_city
    {
        public Int32 city_gid { get; set; }
        public string city_code { get; set; }
        public string city_name { get; set; }
        public Int32 city_pincode { get; set; }
        public Int32 city_insert_by { get; set; }
        public DateTime city_insert_date { get; set; }
        public Int32 city_update_by { get; set; }
        public DateTime city_update_date { get; set; }
        public string city_isremoved { get; set; }

        public int selectedstate_gid { get; set; }
        public Int32 state_gid { get; set; }
        public string state_code { get; set; }
        public string state_name { get; set; }
        public SelectList Getstate { get; set; }
        public SelectList GetstateById { get; set; }

        public int selectedregion_gid { get; set; }
        public Int32 region_gid { get; set; }
        public string region_name { get; set; }
        public string region_code { get; set; }
        public SelectList Getregion { get; set; }
        public SelectList GetregionById { get; set; }

        public int selectedcountry_gid { get; set; }
        public Int32 country_gid { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public SelectList Getcountry { get; set; }
        public SelectList GetcountryById { get; set; }

        public int selectedcityclass_gid { get; set; }
        public Int32 cityclass_gid { get; set; }
        public string cityclass_code { get; set; }
        public string cityclass_name { get; set; }
        public SelectList Getcityclass { get; set; }
        public SelectList GetcityclassById { get; set; }

        public int selectedtier_gid { get; set; }
        public Int32 tier_gid { get; set; }
        public string tier_code { get; set; }
        public string tier_name { get; set; }
        public SelectList Gettier { get; set; }
        public SelectList GettierById { get; set; }

    }
    public class Getstate
    {
        public Int32 stategid { get; set; }
        public string statename { get; set; }
        public string statecode { get; set; }
    }
    public class GetstateById
    {
        public Int32 stategid { get; set; }
        public string statename { get; set; }
        public string statecode { get; set; }
    }
    public class Getregion
    {
        public Int32 regiongid { get; set; }
        public string regionname { get; set; }
        public string regioncode { get; set; }
    }
    public class GetregionById
    {
        public Int32 regiongid { get; set; }
        public string regionname { get; set; }
        public string regioncode { get; set; }
    }
    public class Getcountry
    {
        public Int32 countrygid { get; set; }
        public string countryname { get; set; }
        public string countrycode { get; set; }
    }
    public class GetcountryById
    {
        public Int32 countrygid { get; set; }
        public string countryname { get; set; }
        public string countrycode { get; set; }
    }
    public class Getcityclass
    {
        public Int32 cityclassgid { get; set; }
        public string cityclassname { get; set; }
        public string cityclasscode { get; set; }
    }
    public class GetcityclassById
    {
        public Int32 cityclassgid { get; set; }
        public string cityclassname { get; set; }
        public string cityclasscode { get; set; }
    }
    public class Gettier
    {
        public Int32 tiergid { get; set; }
        public string tiername { get; set; }
        public string tiercode { get; set; }
    }
    public class GettierById
    {
        public Int32 tiergid { get; set; }
        public string tiername { get; set; }
        public string tiercode { get; set; }
    }
    public class iem_mst_state
    {
        public Int32 state_gid { get; set; }
        public string state_code { get; set; }
        public string state_name { get; set; }
        public string state_gst { get; set; }//added by Pandiaraj
        public Int32 state_region_gid { get; set; }
        public string state_region_name { get; set; }
        public Int32 state_country_gid { get; set; }
        public string state_country_code { get; set; }
        public Int32 state_insert_by { get; set; }
        public DateTime state_insert_date { get; set; }
        public Int32 state_update_by { get; set; }
        public DateTime state_update_date { get; set; }
        public string state_isremoved { get; set; }

        public int selectedregion_gid { get; set; }
        public Int32 region_gid { get; set; }
        public string region_name { get; set; }
        public string region_code { get; set; }
        public SelectList Getregion { get; set; }
        public SelectList GetregionById { get; set; }

        public int selectedcountry_gid { get; set; }
        public Int32 country_gid { get; set; }
        public string country_code { get; set; }
        public string country_name { get; set; }
        public SelectList Getcountry { get; set; }
        public SelectList GetcountryById { get; set; }
    }
    public class iem_mst_tax_rate
    {
        public Int32 taxname_gid { get; set; }
        public string taxname_name { get; set; }
        public Int32 taxsubtype_gid { get; set; }
        public string taxsubtypename_name { get; set; }
        public string taxrate_taxsubtype_gid { get; set; }
        public string taxrate_rate { get; set; }
        public Int32 taxrate_gid { get; set; }
        public string taxrate_tax_gid { get; set; }
        public string taxrate_taxsubtype_flag { get; set; }
        public DateTime taxrate_insert_date { get; set; }
        public Int32 taxrate_insert_by { get; set; }

        public DateTime taxrate_update_date { get; set; }
        public string taxrate_change_flag { get; set; }
        public SelectList Gettaxrate { get; set; }
        public SelectList GettaxrateId { get; set; }

        public SelectList Gettaxratetaxname { get; set; }
        public SelectList Gettaxratetaxsubname { get; set; }
        public Int32 taxrate_update_by { get; set; }

        public string taxrate_period_from { get; set; }
        public string taxrate_period_to { get; set; }
        public string taxrate_active_flag { get; set; }
    }
    public class iem_mst_tax_subtype
    {
        public int GLId { get; set; }
        public string GLCode { get; set; }
        public string SubCategoryName { get; set; }
        public string ExpenseCategoryName { get; set; }

        public SelectList ExpNatureofExpdata { get; set; }
        public SelectList ExpCatdata { get; set; }
        public SelectList ExpSubCatdata { get; set; }

        public int NatureofExpensesId { get; set; }
        public string NatureofExpensesName { get; set; }
        public int ExpenseCategoryId { get; set; }
        public int SubCategoryId { get; set; }
        public Int32 tax_gid { get; set; }
        public string tax_name { get; set; }
        public Int32 taxsubtype_gid { get; set; }
        public string taxsubtype_name { get; set; }
        public string taxsubtype_code { get; set; }
        public string taxsubtype_active_flag { get; set; }
        public DateTime taxsubtype_insert_date { get; set; }
        public Int32 taxsubtype_insert_by { get; set; }

        public DateTime taxsubtype_update_date { get; set; }
        public string taxsubtype_parent_name { get; set; }
        public SelectList Gettaxsub { get; set; }
        public SelectList GettaxsubById { get; set; }
        public Int32 taxsubtype_update_by { get; set; }
    }
    public class iem_mst_tax
    {
        public Int32 tax_gid { get; set; }
        public string tax_name { get; set; }
        public string tax_code { get; set; }
        public string tax_active { get; set; }
        public string tax_reverse_flag { get; set; }
        public string tax_inputcredit_flag { get; set; }
        public DateTime tax_insert_date { get; set; }
        public Int32 tax_insert_by { get; set; }

        public string tax_receivable_flag { get; set; }
        public string tax_payable_flag { get; set; }
        public string tax_withhold_flag { get; set; }
        public string tax_parent_flag { get; set; }
        public DateTime tax_update_date { get; set; }
        public string tax_parent_name { get; set; }
        public SelectList Gettax { get; set; }
        public SelectList GettaxById { get; set; }
        public string tax_parent_name1 { get; set; }
        public Int32 tax_update_by { get; set; }
    }
    public class iem_mst_expnature
    {
        public Int32 expnature_gid { get; set; }
        public string expnature_name { get; set; }
        public string bouncereason_name { get; set; }
        public string expnature_active { get; set; }
        public Int32 expnature_insert_by { get; set; }
        public DateTime expnature_insert_date { get; set; }
        public Int32 expnature_update_by { get; set; }
        public DateTime expnature_update_date { get; set; }
        public string expnature_isremoved { get; set; }
        public string expnature_istravel { get; set; }
        public string expnature_isothertravel { get; set; }
    }
    public class iem_mst_expensecategory
    {
        public Int32 expcat_gid { get; set; }
        public int selectedexpnature_gid { get; set; }
        public Int32 expcat_expnature_gid { get; set; }
        public string expcat_code { get; set; }
        public string expcat_name { get; set; }
        public string expcat_gl_no { get; set; }
        public string selectedgl_no { get; set; }
        public string expcat_active { get; set; }
        public Int32 expcat_insert_by { get; set; }
        public DateTime expcat_insert_date { get; set; }
        public Int32 expcat_update_by { get; set; }
        public DateTime expcat_update_date { get; set; }
        public string expcat_isremoved { get; set; }
        public int expcat_mode { get; set; }

        public Int32 gl_gid { get; set; }
        public string gl_no { get; set; }
        public SelectList GetGL { get; set; }
        public SelectList GetGLById { get; set; }

        public Int32 expnature_gid { get; set; }
        public string expnature_name { get; set; }
        public SelectList Getexpnature { get; set; }
        public SelectList GetexpnatureById { get; set; }
    }
    public class GetGL
    {
        public Int32 gl_gid { get; set; }
        public string gl_no { get; set; }
    }
    public class GetGLById
    {
        public Int32 gl_gid { get; set; }
        public string gl_no { get; set; }
    }
    public class Gettaxratetaxsubname
    {
        public Int32 taxsub_gid { get; set; }
        public string taxsub_name { get; set; }
    }
    public class Gettaxratetaxname
    {
        public Int32 tax_gid { get; set; }
        public string tax_name { get; set; }
    }
    public class Gettaxsub
    {
        public Int32 tax_gid { get; set; }
        public string tax_name { get; set; }
    }
    public class Gettax
    {
        public Int32 tax_gid { get; set; }
        public string tax_name { get; set; }
    }
    public class Getexpnature
    {
        public Int32 expnature_gid { get; set; }
        public string expnature_name { get; set; }
    }
    public class GetexpnatureById
    {
        public Int32 expnature_gid { get; set; }
        public string expnature_name { get; set; }
    }
    public class iem_mst_expensesubcategory
    {
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
        public SelectList Getexpcat { get; set; }
        public SelectList GetexpcatById { get; set; }

        public Int32 expnature_gid { get; set; }
        public string expnature_name { get; set; }
        public SelectList Getexpnature { get; set; }
        public SelectList GetexpnatureById { get; set; }
        public string GlNo { get; set; }

        public Int32 Hsn_gid { get; set; }
        public string hsn_code { get; set; }
        public string hsn_desc { get; set; }
        public SelectList GetHsncode { get; set; }
        public int[] SelectedItemIds { get; set; }
        public MultiSelectList Items { get; set; }
    }
    public class GetHsncode
    {
        public Int32 Hsn_gid { get; set; }
        public string hsn_code { get; set; }
        public string hsn_desc { get; set; }
    }
    public class Getexpcat
    {
        public Int32 expcat_gid { get; set; }
        public string expcat_name { get; set; }
    }
    public class GetexpcatById
    {
        public Int32 expcat_gid { get; set; }
        public string expcat_name { get; set; }
    }
    public class iem_mst_delmat
    {
        public Int32 s_no { get; set; }
        public Int32 delmat_gid { get; set; }
        public Int32 delmat_slab_gid { get; set; }
        public string delmat_name { get; set; }
        public Int32 delmat_type_ID { get; set; }
        public string delmat_branch_flag { get; set; }
        public string delmat_branchtype_flag { get; set; }
        public string delmat_claim_flag { get; set; }
        public string delmat_dsa_flag { get; set; }
        public string delmat_pipit_flag { get; set; }
        public string delmat_it_flag { get; set; }
        public string delmat_exp_flag { get; set; }
        public string delmat_budget_flag { get; set; }
        public string delmat_jump_flag { get; set; }
        public string delmat_active { get; set; }
        public Int32 delmat_insert_by { get; set; }
        public DateTime delmat_insert_date { get; set; }
        public Int32 delmat_update_by { get; set; }
        public DateTime delmat_update_date { get; set; }
        public string delmat_isremoved { get; set; }
        public string delmatexception_name { get; set; }
        //DelmatException value
        public Int32 delmatexception_gid { get; set; }
        public Int32 delmatexception_delmat_gid { get; set; }
        public Int32 delmatexception_to { get; set; }
        public Decimal delmatexception_limit { get; set; }


        //delmatsetflow object's

        public Int32 SNo { get; set; }
        public string Title { get; set; }
        public Int32 Titlegid { get; set; }
        public Int32 TitleNamegid { get; set; }
        public string TitleName { get; set; }
        public string AddApporoval { get; set; }
        public Int32 Slabrangegid { get; set; }
        public Int32 Flow { get; set; }

        //delmatmatrix & setdlow
        public Int32 delmatmatrixgid { get; set; }
        public Int32 delmatsetflowgid { get; set; }


        public string[] lstSelecteddepartmentGid = new string[64];
        public string[] lstSelecAddapprovalGid = new string[64];
        public string[] lstSelecSlabrangeGid = new string[64];
        public string[] lsselectedSlabrange = new string[64];

        public SelectList listofdata { get; set; }

        public string delmattype_branch_visible { get; set; }
        public string delmattype_branchtype_visible { get; set; }
        public string delmattype_claim_visible { get; set; }
        public string delmattype_dsa_visible { get; set; }
        public string delmattype_pipit_visible { get; set; }
        public string delmattype_it_visible { get; set; }
        public string delmattype_exp_visible { get; set; }
        public string delmattype_budget_visible { get; set; }
        public string delmattype_jump_visible { get; set; }
        public string delmattype_active_visible { get; set; }

        public Int32 slabrange_gid { get; set; }
        public Int32 slabrange_slab_gid { get; set; }
        public string slabrange_name { get; set; }
        public Decimal slabrange_range_from { get; set; }
        public Decimal slabrange_range_to { get; set; }
        public Int32 slabrangecount { get; set; }

        public Int32 delmattype_gid { get; set; }
        public string delmattype_name { get; set; }

        public Int32 dept_gid { get; set; }
        public string dept_name { get; set; }
        public Int32 slab_gid { get; set; }
        public string slab_name { get; set; }
        public IList<SelectListItem> Department { get; set; }
        public IList<SelectListItem> slabrangegidcount { get; set; }
        //public IList<SelectListItem> ColumnList { get; set; }
        public string Viewfor { get; set; }
        //Delmat title 
        public Int32 title_gid { get; set; }
        public Int32 title_flag { get; set; }
        public string title_name { get; set; }
        public string title_table_name { get; set; }
        public string title_field_name { get; set; }
        public string title_pkfield_name { get; set; }

        public Int32 value_gid { get; set; }

        //Employee Value
        public Int32 employee_gid { get; set; }
        public string employee_code { get; set; }

        //Employee Designation
        public Int32 designation_gid { get; set; }
        public string designation_code { get; set; }

        //Employee Role
        public Int32 role_gid { get; set; }
        public string role_code { get; set; }

        //Employee Grade
        public Int32 grade_gid { get; set; }
        public string grade_code { get; set; }

        //SetFlow Id
        public Int32 GID { get; set; }

        public Int32 Countvalue { get; set; }
        public Int32 Snocount { get; set; }
        public Int32 Flowvalue { get; set; }
        public Int32 InFlowCount { get; set; }
        public string lsdelmatflowtitlevalue { get; set; }
        public string lsdelmatflowaddapprovalvalue { get; set; }
        public string lsTitlename { get; set; }
        public string lsMatrixAccess { get; set; }
        public Int32 InRowscount { get; set; }


        public SelectList GetDelmat { get; set; }
        public SelectList GetslabRangeByID { get; set; }
        public SelectList GetDepartment { get; set; }
        public SelectList GetSlab { get; set; }
        public DelmatVisisbleById DelmatVisisbleById { get; set; }
        public SelectList GetTitle { get; set; }
        public SelectList GettitleById { get; set; }

        public SelectList GetEmployee { get; set; }
        public SelectList GetGrade { get; set; }
        public SelectList GetDesignation { get; set; }
        public SelectList GetRole { get; set; }

    }
    public class GetslabRangeByID
    {
        public Int32 slabrange_gid { get; set; }
        public string slabrange_name { get; set; }
        public Decimal slabrange_range_from { get; set; }
        public Decimal slabrange_range_to { get; set; }
    }
    public class GetEmployee
    {
        public Int32 employee_gid { get; set; }
        public string employee_code { get; set; }
    }
    public class GetGrade
    {
        public Int32 employee_gid { get; set; }
        public string employee_code { get; set; }

    }
    public class GetDesignation
    {
        public Int32 employee_gid { get; set; }
        public string employee_code { get; set; }

    }
    public class GetRole
    {
        public Int32 employee_gid { get; set; }
        public string employee_code { get; set; }
    }
    public class GetTitle
    {
        public Int32 title_gid { get; set; }
        public Int32 title_flag { get; set; }
        public string title_name { get; set; }
    }
    public class GettitleById
    {
        public Int32 title_gid { get; set; }
        public Int32 title_flag { get; set; }
        public string title_name { get; set; }
    }

    public class DelmatVisisbleById
    {
        public string delmattype_branch_visible { get; set; }
        public string delmattype_branchtype_visible { get; set; }
        public string delmattype_claim_visible { get; set; }
        public string delmattype_dsa_visible { get; set; }
        public string delmattype_pipit_visible { get; set; }
        public string delmattype_it_visible { get; set; }
        public string delmattype_exp_visible { get; set; }
        public string delmattype_budget_visible { get; set; }
        public string delmattype_jump_visible { get; set; }
        public string delmattype_active_visible { get; set; }
        // public string delmattype_active_visible { get; set; }  

    }
    public class GetDelmat
    {
        public Int32 delmattype_gid { get; set; }
        public string delmattype_name { get; set; }
    }
    public class GetDelmatById
    {
        public Int32 delmattype_gid { get; set; }
        public string delmattype_name { get; set; }
    }
    public class GetDepartment
    {
        public Int32 dept_gid { get; set; }
        public string dept_name { get; set; }
    }
    public class GetDepartmentById
    {
        public Int32 dept_gid { get; set; }
        public string dept_name { get; set; }
    }
    public class GetSlab
    {
        public Int32 slab_gid { get; set; }
        public string slab_name { get; set; }

    }
    public class GetSlabById
    {
        public Int32 slab_gid { get; set; }
        public string slab_name { get; set; }

    }
    public class iem_mst_role
    {
        public Int32 role_gid { get; set; }
        public Int32 rolegroup_gid { get; set; }
        public string role_code { get; set; }
        public string role_name { get; set; }
        public SelectList Getrolegroup { get; set; }
        public string[] lstSelectedStateGid = new string[32];
        public Int32 role_rolegroup_gid { get; set; }
        public Int32 role_insert_by { get; set; }
        public DateTime role_insert_date { get; set; }
        public Int32 role_update_by { get; set; }
        public DateTime role_update_date { get; set; }
        public string rolegroup_name { get; set; }
        public List<iem_mst_role> RoleList { get; set; }
        public int id { get; set; }
        public List<menu> menu { get; set; }
        public string role_assignedto { get; set; }
    }

    public partial class menu
    {
        public int menu_gid { get; set; }
        public string menu_code { get; set; }
        public string menu_name { get; set; }
        public string menu_link { get; set; }
        public int menu_parent_gid { get; set; }
        public bool menu_access { get; set; }
    }

    public class Getrolegroup
    {
        public Int32 rolegroup_gid { get; set; }
        public string rolegroup_name { get; set; }
    }
    public class iem_mst_advancetype
    {
        public int advancetype_gid { get; set; }
        public string advancetype_name { get; set; }
        public string advancetype_gl_no { get; set; }
        public string advancetype_active { get; set; }
        public int advancetype_insert_by { get; set; }
        public int advancetype_insert_date { get; set; }
        public int advancetype_update_by { get; set; }
        public int advancetype_update_date { get; set; }
        public int gl_gid { get; set; }
        public string gl_no { get; set; }
        public int tempadvancetype_gl_no { get; set; }
        public string advancesubtype_name { get; set; }
        public string advancetype_gl_desc { get; set; }
        public SelectList GetGL { get; set; }
        public string advancetype_help { get; set; }
    }
    public class iem_mst_branchtype
    {
        public int branchtype_gid { get; set; }
        public string branchtype_name { get; set; }
        public int branchtype_insert_by { get; set; }
        public string branchtype_insert_date { get; set; }
        public int branchtype_update_by { get; set; }
        public string branchtype_update_date { get; set; }
        public string branchtype_active { get; set; }

    }
    public class iem_mst_branchtier
    {
        public int branchtier_gid { get; set; }
        public string branchtier_name { get; set; }
        public int branchtier_insert_by { get; set; }
        public int branchtier_insert_date { get; set; }
        public int branchtier_update_by { get; set; }
        public int branchtier_update_date { get; set; }


    }
    public class iem_mst_location
    {
        public int location_gid { get; set; }
        public string location_code { get; set; }
        public string location_name { get; set; }
        public Int32 location_pincode { get; set; }
        public int location_city_gid { get; set; }
        public string location_city { get; set; }
        public string location_tier { get; set; }
        public int location_tier_gid { get; set; }
        public int location_insert_by { get; set; }
        public string location_insert_date { get; set; }
        public int location_update_by { get; set; }
        public string location_update_date { get; set; }
        public SelectList GetCity { get; set; }
        public SelectList Gettier { get; set; }
    }
    public class GetCity
    {
        public int citygid { get; set; }
        public string cityname { get; set; }
    }

    public class iem_mst_hotel
    {
        public int hoteltype_gid { get; set; }
        public string hoteltype_name { get; set; }
        public string hoteltype_insert_by { get; set; }
        public string hoteltype_insert_date { get; set; }
        public string hoteltype_update_by { get; set; }
        public string hoteltype_update_date { get; set; }

    }
    public class iem_mst_delegation
    {
        public int delegate_gid { get; set; }
        public int delegate_by { get; set; }
        public int delegate_bygid { get; set; }
        public int delegate_to { get; set; }
        public string delegate_delmat_flag { get; set; }
        public string delegate_supervisory_flag { get; set; }
        public string delegate_period_from { get; set; }
        public string delegate_period_to { get; set; }
        public string delegate_active { get; set; }
        public string delegate_remark { get; set; }
        public int delegate_insert_by { get; set; }
        public string delegate_insert_date { get; set; }
        public int delegate_update_by { get; set; }
        public string delegate_update_date { get; set; }
        public int delegate_delmattype { get; set; }
        public SelectList GetDelmattype { get; set; }
        public string temp_delegate_by { get; set; }
        public string temp_delegate_to { get; set; }
        public string temp_delegate_delmattype { get; set; }
        public string RaiserCode { get; set; }
        public string RaiserName { get; set; }
        public int EmployeeId { get; set; }
        public int employee_gid { get; set; }
        public int delmattype_gid { get; set; }
        public string delmattypename { get; set; }
        public string employee_code { get; set; }
    }
    public class GetDelmattype
    {
        public int delmatypegid { get; set; }
        public string delmattypename { get; set; }
    }

    public class iem_mst_auditgroup
    {
        public int auditgroup_gid { get; set; }
        public string auditgroup_name { get; set; }
        public int auditgroup_doccat_gid { get; set; }
        public string doccatname { get; set; }
        public string doctype_name { get; set; }
        public string docsubtypename { get; set; }
        public int auditgroup_insert_by { get; set; }
        public string auditgroup_insert_date { get; set; }
        public int auditgroup_update_by { get; set; }
        public string auditgroup_update_date { get; set; }
        public int auditgroup_doctype_gid { get; set; }
        public int auditgroup_docsubtype_gid { get; set; }
        public string doctypename { get; set; }
        public int doctype_gid { get; set; }
        public int doccat_gid { get; set; }
        public int docsubtype_gid { get; set; }
        public SelectList doctype { get; set; }
        public SelectList docsubtype { get; set; }
        public SelectList doccat { get; set; }
    }
    public class doctype
    {
        public int doctypegid { get; set; }
        public string doctypename { get; set; }

    }
    public class docsubtype
    {
        public int docsubtypegid { get; set; }
        public string docsubtypename { get; set; }

    }
    public class doccat
    {
        public int doccatgid { get; set; }
        public string doccatname { get; set; }

    }
    public class iem_mst_auditlist
    {
        public int auditlist_gid { get; set; }
        public int auditlist_auditsubgroup_gid { get; set; }
        public string auditlist_name { get; set; }
        public string auditlist_template { get; set; }
        public int auditlist_order { get; set; }
        public int auditlist_insert_by { get; set; }
        public string auditlist_insert_date { get; set; }
        public int auditlist_update_by { get; set; }
        public string auditlist_update_date { get; set; }
        public int auditlist_auditgroup_gid { get; set; }
        public string auditgroup_name { get; set; }
        public string auditsubgroup_name { get; set; }
        public int auditgroup_gid { get; set; }
        public int auditsubgroup_gid { get; set; }
        public SelectList selctgroupname { get; set; }
        public SelectList selectsubgroupname { get; set; }
        public string groupname { get; set; }
        public string subgroupname { get; set; }

    }
    public class selctgroupname
    {
        public int groupnamegid { get; set; }
        public string groupname { get; set; }
    }
    public class selectsubgroupname
    {
        public int subgroupnamegid { get; set; }
        public string subgroupname { get; set; }
    }

    public class iem_mst_finayear
    {
        public int finyear_gid { get; set; }
        public string finyear_code { get; set; }
        public string finyear_start_date { get; set; }
        public string finyear_end_date { get; set; }
        public string finyear_insert_date { get; set; }
        public string finyear_insert_by { get; set; }
        public string finyear_update_date { get; set; }
        public int finyear_update_by { get; set; }

    }
    public class iem_mst_auditsubgroup
    {
        public int auditsubgroup_gid { get; set; }
        public int auditsubgroup_auditgroup_gid { get; set; }
        public string auditsubgroup_name { get; set; }
        public int auditsubgroup_order { get; set; }
        public int auditsubgroup_insert_by { get; set; }
        public string auditsubgroup_insert_date { get; set; }
        public int auditsubgroup_update_by { get; set; }
        public string auditsubgroup_update_date { get; set; }
        public SelectList selctgroupname { get; set; }
        public string auditgroup_name { get; set; }
        public string groupname { get; set; }
        public int auditgroup_gid { get; set; }
    }

    public class iem_mst_accomodationtype
    {
        public int accommodationtype_gid { get; set; }
        public string accommodationtype_name { get; set; }
        public int accommodationtype_insert_by { get; set; }
        public string accommodationtype_insert_date { get; set; }
        public int accommodationtype_update_by { get; set; }
        public string accommodationtype_update_date { get; set; }


    }
    public class othersystemdatamodel
    {
        public int Branchtype_gid { get; set; }
        public string BranchType { get; set; }
        public int Branch_gid { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public int City_gid { get; set; }
        public string City { get; set; }
        public string BranchTier { get; set; }
        public string BranchIncharge { get; set; }
        public string StartDate { get; set; }
        public string ClosedDate { get; set; }
        public string ActiveStatus { get; set; }
        public string Branchfl { get; set; }
        public string Location { get; set; }
        public int Location_gid { get; set; }
        public string LocationName { get; set; }
        public string BranchAddress { get; set; }
        public string OldBranchCode { get; set; }

        public int EmployeeId { get; set; }
        public string EmployeeDepartment { get; set; }
        public string RaiserCode { get; set; }
        public string RaiserName { get; set; }

        public string StartLeaseDate { get; set; }
        public string EndLeaseDate { get; set; }
        public string PinCode { get; set; }
        public string Fc_Code { get; set; }
        public string Fc_Name { get; set; }
        public string Cc_Name { get; set; }
        public string Cc_Code { get; set; }
        public string productCode { get; set; }
        public string ProductName { get; set; }

        public string gl_no { get; set; }
        public string gl_name { get; set; }
        public string gl_company_code { get; set; }
        public string gl_business_segment { get; set; }
        public string gl_location_code { get; set; }
        public string gl_product_code { get; set; }

        public string employee_code { get; set; }
        public string employee_name { get; set; }
        public string employee_dob { get; set; }
        public string employee_iem_designation { get; set; }
        public string employee_grade_code { get; set; }
        public string employee_dept_name { get; set; }


    }

    // GST Ganesh 24-01-2017
    public class EntityGSTPincode
    {
        public Int32 Pincode_gid { get; set; }
        public string Pincode_code { get; set; }
        public int Pincode_state_gid { get; set; }
        public string Pincode_state_name { get; set; }
        public int Pincode_district_gid { get; set; }
        public string Pincode_district_name { get; set; }
        public string Pincode_status { get; set; }
        public string Pincode_isremoved { get; set; }
        public string Pincode_insertby { get; set; }
        public DateTime Pincode_insertdate { get; set; }
        public string Pincode_updateby { get; set; }
        public DateTime Pincode_updatedate { get; set; }
        public SelectList GetDistrict { get; set; }
        public SelectList GetDistrictById { get; set; }
        public int selecteddistrict_gid { get; set; }

        public SelectList getPincode { get; set; }
        public SelectList GetState { get; set; }
        public SelectList GetStateById { get; set; }
        public int selectedstate_gid { get; set; }


    }
    public class EntityGstvendor
    {
        //  ,,,,,suppliergst_isremoved,suppliergst_insertby
        public Int32 suppliergst_gid { get; set; }
        public string suppliergst_app { get; set; }
        public string suppliergst_vertical { get; set; }
        public Int32 suppliergst_stateid { get; set; }
        public string suppliergst_state { get; set; }
        public string suppliergst_threshold { get; set; }
        public string suppliergst_tin { get; set; }
        public string suppliergst_status { get; set; }
       


        public SelectList GetState { get; set; }
        public SelectList GetStateById { get; set; }
        public int selectedstate_gid { get; set; }
        public string IsChecker { get; set; }
        public string approved_status { get; set; }

    }
    public class EntityGstHsn
    {
        public int hsn_gid { get; set; }
        public string hsn_code { get; set; }
        public string hsn_description { get; set; }
        public string hsn_addition { get; set; }
        public string hsn_status { get; set; }
        public string hsn_insertby { get; set; }
        public string hsn_updateby { get; set; }
        public DateTime ? hsn_fromdate { get; set; }
        public DateTime ? hsn_todate { get; set; }
        public string hsn_creationdate { get; set; }
        public string hsn_updationdate { get; set; }

    }
    public class EntityGstFicc
    {
        public string Gstapplicable { get; set; }
        public string State { get; set; }
        public string Businessvertical { get; set; }
        public string Gsttin { get; set; }
        public string Status { get; set; }
        public string Pincodeid { get; set; }


        public string branchcode { get; set; }
        public string BranchName { get; set; }
        public string BranchType { get; set; }
        public string BranchTier { get; set; }
        public string OldBranchCode { get; set; }
        public string BrnachIncharge { get; set; }
        public string StartDate { get; set; }
        public string EndStart { get; set; }
        public string NonBranch { get; set; }
        public string City { get; set; }
        public string LeaseStartDate { get; set; }
        public string LeaseEndDate { get; set; }
        public string BranchAddress { get; set; }
        public string Pincode { get; set; }
        public string District { get; set; }
        public string Threshold { get; set; }
        public string state { get; set; }
        public string gstin { get; set; }
    }

    public class EntityGSTFiccBranch
    {
        public int Branchtype_gid { get; set; }
        public string BranchType { get; set; }
        public int Branch_gid { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public int City_gid { get; set; }
        public string City { get; set; }
        public string BranchTier { get; set; }
        public string BranchIncharge { get; set; }
        public string StartDate { get; set; }
        public string ClosedDate { get; set; }
        public string ActiveStatus { get; set; }
        //public string Branch { get; set; }
        public string Branchfl { get; set; }
        public string Location { get; set; }
        public int Location_gid { get; set; }
        public string LocationName { get; set; }
        public string BranchAddress { get; set; }
        public string OldBranchCode { get; set; }

        public int EmployeeId { get; set; }
        public string EmployeeDepartment { get; set; }
        public string RaiserCode { get; set; }
        public string RaiserName { get; set; }

        public string StartLeaseDate { get; set; }
        public string EndLeaseDate { get; set; }
        public string PinCode { get; set; }

        public Int32 pincode_gid { get; set; }
        public string pincode_code { get; set; }
        public SelectList GetPincode { get; set; }
        public string pincode_list { get; set; }

        public SelectList GetDistrict { get; set; }
        public SelectList Getstate { get; set; }
        public string statecode { get; set; }
        public string dictrictcode { get; set; }
        public Int32 state_gid { get; set; }
        public Int32 district_gid { get; set; }
        public string branch_gstin { get; set; }

    }
    //added by Pandiaraj 04May17
    public class EntityIEM_Auditing
    {
        public string Table_Id { get; set; }
        public string Table_name { get; set; }

        public int Emp_id { get; set; }
        public string Emp_name { get; set; }

        public Int32 Col_Id { get; set; }
        public string Col_name { get; set; }
        public SelectList GetTable_cols { get; set; }

        public string txtfrom { get; set; }
        public string txtto { get; set; }
    }
    //added by Pandiaraj
    #region // add by sugumar
    public class Entity_taxgst
    {
        public int Taxgst_gid { get; set; }
        public int Taxsubtypeid { get; set; }
        public int hsnid { get; set; }
        public string Hsn_desc { get; set; }
        public decimal inputcredit { get; set; }
        public decimal reverse_charge { get; set; }
        public string effective_date { get; set; }
        public string effective_todate { get; set; }
        public int stategid { get; set; }
        public int inputcredit_gid { get; set; }
        public int reversecharge_glid { get; set; }
        public string Active { get; set; }
        public decimal Rate { get; set; }
        public string Remarks { get; set; }
        public int insertedby { get; set; }
        public DateTime inserteddate { get; set; }
        public DateTime modifieddate { get; set; }
        public string Status { get; set; }
        public string Approvalstatus { get; set; }
        public string isremoved { get; set; }
        public string Taxsubtypecode { get; set; }
        public string Taxhsncode { get; set; }
        public string Statename { get; set; }
        public string Taxglno { get; set; }
        public string IpcdGLno { get; set; }
        public string RcGLno { get; set; }
        public string Rolename { get; set; }
        public string SqlAction { get; set; }
        public string Vwstatus { get; set; }
        public SelectList GetTaxsubtype { get; set; }
        public SelectList GetHsncod { get; set; }
        public SelectList Getstatelist { get; set; }
        public SelectList Getgllist { get; set; }
        public SelectList GetglCr { get; set; }
        public SelectList GetglRe { get; set; }

        //pandiaraj
        public DataTable dt { get; set; }
        public DataSet ds { get; set; }
        public int uploadgid { get; set; }
        public string uploadno { get; set; }
        public string uploaddate { get; set; }
        public string upload_filename { get; set; }
        public int upload_count { get; set; }
        public int upload_sno { get; set; }
        public List<Entity_taxgst> GSTAuditList { get; set; }
        public string TaxSubtypeName { get; set; }
    }
    public class GetTaxsubtype
    {
        public Int32 Taxsubtypeid { get; set; }
        public string taxsubtype_code { get; set; }
    }
    public class GetHsncod
    {
        public Int32 hsnid { get; set; }
        public string Hsn_code { get; set; }
        public string Hsn_desc { get; set; }
    }
    public class Getstatelist
    {
        public Int32 stategid { get; set; }
        public string State_code { get; set; }
    }
    public class Getgllist
    {
        public Int32 Taxgl_id { get; set; }
        public string Glno { get; set; }

    }
    public class GetglCr
    {
        public Int32 inputcredit_gid { get; set; }
        public string CrGlno { get; set; }
    }
    public class GetglRe
    {
        public Int32 reversecharge_glid { get; set; }
        public string RcGlno { get; set; }
    }
    #endregion

    //added by bharathi
    public class CustomerdetailModel
    {
        public Int32 sNo { get; set; }
        public Int32 CustomerID { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public SelectList CustomerCodeMod { get; set; }
        public SelectList CustomerNameMod { get; set; }
        public Int32 CustomerdetailID { get; set; }
        public string CustomerdetailName { get; set; }
        public List<CustomerdetailModel> CustomerDetailList { get; set; }
        public List<CustomerdetailModel> CustomerList { get; set; }
        public Int32 PincodeID { get; set; }
        public string Pincode { get; set; }
        public Int32 stateid { get; set; }
        public string statename { get; set; }
        public Int32 districtID { get; set; }
        public string districtname { get; set; }
        public Int32 countryid { get; set; }
        public string countryname { get; set; }
        public SelectList PincodeMod { get; set; }
        public SelectList stateMod { get; set; }
        public SelectList DistrictMod { get; set; }
        public SelectList CountryMod { get; set; }


        public int selectedStateID { get; set; }
        public int selectedDistrictID { get; set; }
        public int selectedPincodeID { get; set; }
        public int _PinCode { get; set; }
        public string Panno { get; set; }
        public string customergst_gstin { get; set; }
        List<CustomerDetailHeader> Headermodel { get; set; }
        List<CustomerContactDetails> Contactmodel { get; set; }
        List<CustomerTaxDetails> Taxmodel { get; set; }
        List<EntityGstCustomer> Gsttaxmodel { get; set; }
    }
    //added by bharathi

    public class CustomerDetailHeader
    {
        public int _HeaderID { get; set; }
        public string _CustomerType { get; set; }
        public string _CustomerCode { get; set; }
        public string _CustomerName { get; set; }
        public string _CompanyRegNo { get; set; }
        public int _NoOfDirectors { get; set; }
        public string _website { get; set; }
        public string _IsActiveContract { get; set; }
        public string _ReasonForNoContract { get; set; }
        public string _ContractFrom { get; set; }
        public string _ContractTo { get; set; }
        public string _ApproxSpend { get; set; }
        public string _ActualSpend { get; set; }
        public string _EmailID { get; set; }
        public string _RequestType { get; set; }
        public string _Requeststatus { get; set; }
        public string _CustomerStatus { get; set; }

        public string _RenewalDate { get; set; }
        public int _OwnerId { get; set; }
        public string _OwnerCode { get; set; }
        public string _OwnerName { get; set; }
        //public List<customerType> lstcustomerType { get; set; }
        //public string _customerTypeSelected { get; set; }

        public int _ServiceTypeID { get; set; }
        public int selectedServiceID { get; set; }
        public string _ServiceTypeName { get; set; }
        public SelectList lstServiceType { get; set; }

        public int _OrganizationTypeID { get; set; }
        public int selectedOrganizationID { get; set; }
        public string _OrganizationTypeName { get; set; }
        public SelectList lstOrganizationType { get; set; }

        public int _CustomerCategoryID { get; set; }
        public int SelectedCustomerCategoryID { get; set; }
        public string _CustomerCategoryName { get; set; }
        public SelectList lstCustomerCategory { get; set; }

        public int _CustomerContactTypeID { get; set; }
        public int SelectedCustomerContactTypeID { get; set; }
        public string _CustomerContactTypeName { get; set; }
        public SelectList lstCustomerContactType { get; set; }
        public int _CurrentApprovalStage { get; set; }

        public int _NoofYearsIBusiness { get; set; }
        public int _NoofYearsOfAssociation { get; set; }

        public int _NoOfFactories { get; set; }
        public int _NoOfOffices { get; set; }

        public int _TotalEmployees { get; set; }
        public int _PermanentEmployees { get; set; }
        public int _ContractEmployees { get; set; }

        public string _IssueAppointmentLetters { get; set; }
        public string _PerformIntegrityChecks { get; set; }
        public string _IntegrityCheckDesc { get; set; }
        public string _Payminwages { get; set; }
        public string _EmployLabourbelow18Yrs { get; set; }
        public string _EmpPFRegNo { get; set; }
        public string _ShopRegNo { get; set; }
        public string _EmpStateRegNo { get; set; }

        public string _Raiser { get; set; }
        public string _Remarks { get; set; }
        public string _ApprovalDate { get; set; }
        public string _ApprovalStage { get; set; }

        public string _TaxisMSMED { get; set; }

        public string _CurrentApprovalStageName { get; set; }
        public string _IsAllowApproverToEdit { get; set; }

        public string _Activitycount { get; set; }//thiru
        public string _AgeingBucket { get; set; }//thiru
        public string _PanNo { get; set; }
        public string _CustomerTypeRemarks { get; set; }
        public string _IsExistsFlagApprover { get; set; }
        public string _IsExistsApproverName { get; set; }
        public int _CustomerInsertBy { get; set; }

        public string _LockedByCode { get; set; }
        public string _LockedByName { get; set; }
        public string _LockedDate { get; set; }
        public string _Savemode { get; set; }

    }


    public class CustomerContactDetails
    {
        public int _CustContactDetailsID { get; set; }
        public string _Address1 { get; set; }
        public string _Address2 { get; set; }
        public string _Address3 { get; set; }

        public int _AddressTypeID { get; set; }
        public int selectedAddressTypeID { get; set; }
        public string _AddressTypeName { get; set; }
        public SelectList lstAddressType { get; set; }

        public int _CountryID { get; set; }
        public int selectedCountryID { get; set; }
        public string _CountryName { get; set; }
        public SelectList lstCountry { get; set; }

        public int _StateID { get; set; }
        public int selectedStateID { get; set; }
        public string _StateName { get; set; }
        public SelectList lstState { get; set; }

        public int _CityID { get; set; }
        public int selectedCityID { get; set; }
        public string _CityName { get; set; }
        public SelectList lstCity { get; set; }

        //Pandiaraj 03/07/2017 District Add
        public int _DistrictID { get; set; }
        public int selectedDistrictID { get; set; }
        public string _DistrictName { get; set; }
        public SelectList lstDistrict { get; set; }

        public int _PinCode { get; set; }
        public string _PhoneNo { get; set; }
        public string _Fax { get; set; }
        public int Pincodeid { get; set; }
        public string Pincode { get; set; }
        public SelectList lstPincode { get; set; }
        public int selectedPincodeID { get; set; }
    }


    public class CustomerTaxDetails
    {
        public int _TaxDetailsID { get; set; }
        public string _TaxRegNo { get; set; }
        public string _TaxRate { get; set; }
        public string _TaxisMSMED { get; set; }

        public int _TaxDocumentGroupID { get; set; }
        public int selectedTaxDocumentGroupID { get; set; }
        public string _TaxDocumentGroupName { get; set; }
        public SelectList lstTaxDocumentGroup { get; set; }

        public int _TaxDocumentNameID { get; set; }
        public int selectedTaxDocumentNameID { get; set; }
        public string _TaxDocumentName { get; set; }
        public SelectList lstTaxDocumentName { get; set; }

        public int _TaxTypeID { get; set; }
        public int selectedTaxTypeNameID { get; set; }
        public string _TaxTypeName { get; set; }
        public SelectList lstTaxTypeName { get; set; }
        public string _TaxTypeCode { get; set; }
        public int _TdsID { get; set; }

        public int _TaxAttachmentID { get; set; }
        public string _TaxAttachmentFileName { get; set; }
        public string _TaxAttachmentDate { get; set; }
        public string _TaxAttachmentDescription { get; set; }
        public string _TaxAttachmentActualFileName { get; set; }

        public int _TdsServiceTypeID { get; set; }
        public int selectedTdsServiceTypeID { get; set; }
        public string _TdsServiceTypeName { get; set; }
        public string _TdsServiceTypeSection { get; set; }
        public SelectList lstTdsServiceType { get; set; }

        public string _IsExemption { get; set; }
        public string _TDSRate { get; set; }

        public string _ExemptedRate { get; set; }
        public string _ExemptionPeriodFrom { get; set; }
        public string _ExemptionPeriodTo { get; set; }
        public string _ExemptionThresholdValue { get; set; }
        public string _ExemptionAttachedFileName { get; set; }
        public string _ExemptionDescription { get; set; }
        public string _ExemptionCertificateNo { get; set; }
        public string _ExemptionAttachedActualFileName { get; set; }

        public int _TdsDocumentGroupID { get; set; }
        public int selectedTdsDocumentGroupID { get; set; }
        public string _TdsDocumentGroupName { get; set; }
        public SelectList lstTdsDocumentGroup { get; set; }

        public int _TdsDocumentNameID { get; set; }
        public int selectedTdsDocumentNameID { get; set; }
        public string _TdsDocumentName { get; set; }
        public SelectList lstTdsDocumentName { get; set; }

        public int _TdsAttachmentID { get; set; }
        public string _TdsAttachmentFileName { get; set; }
        public string _TdsAttachmentActualFileName { get; set; }
        public string _TdsAttachmentDate { get; set; }
        public string _TdsAttachmentDescription { get; set; }

        public int _VatID { get; set; }
        public string _VatCity { get; set; }
        public string _VatRate { get; set; }

        public int _VatStateID { get; set; }
        public int selectedVatStateID { get; set; }
        public string _VatStateName { get; set; }
        public SelectList lstVatState { get; set; }

        public int _IsAllowAdd { get; set; }
        public string _TaxReceivableFlag { get; set; }

    }

    public class EntityGstCustomer
    {
        //  ,,,,,customergst_isremoved,customergst_insertby
        public Int32 customergst_gid { get; set; }
        public string customergst_app { get; set; }
        public string customergst_vertical { get; set; }
        public Int32 customergst_stateid { get; set; }
        public string customergst_state { get; set; }
        public string customergst_threshold { get; set; }
        public string customergst_tin { get; set; }
        public string customergst_status { get; set; }
        public int customerheader_gid { get; set; }


        public SelectList GetState { get; set; }
        public SelectList GetStateById { get; set; }
        public int selectedstate_gid { get; set; }
        public string IsChecker { get; set; }
        public string approved_status { get; set; }

    }
    public class iem_common_delegation
    {

        public int delegate_gid { get; set; }
        public int delegate_by { get; set; }
        public int delegate_to { get; set; }
        public string delegate_delmat_flag { get; set; }
        public string delegate_supervisory_flag { get; set; }
        public string delegate_period_from { get; set; }
        public string delegate_period_to { get; set; }
        public string delegate_active { get; set; }
        public string delegate_remark { get; set; }
        public int delegate_insert_by { get; set; }
        public string delegate_insert_date { get; set; }
        public int delegate_update_by { get; set; }
        public string delegate_update_date { get; set; }
        public int delegate_delmattype { get; set; }
        public SelectList GetDelmattype { get; set; }
        public SelectList GetDepartmenttype { get; set; }
        public string temp_delegate_by { get; set; }
        public string temp_delegate_to { get; set; }
        public string temp_delegate_delmattype { get; set; }
        public string RaiserCode { get; set; }
        public string RaiserName { get; set; }
        public int EmployeeId { get; set; }
        public int employee_gid { get; set; }
        public int delmattype_gid { get; set; }
        public string delmattypename { get; set; }
        public string employee_code { get; set; }
        public int delegate_bygid { get; set; }
        public int delegate_togid { get; set; }
        public string delegate_byempcode { get; set; }
        public string delegate_toempcode { get; set; }
        public string delegate_byempname { get; set; }
        public string delegate_toempname { get; set; }
        public string delegate_isAdmin { get; set; }
        public int delegate_department_gid { get; set; }
    }
}

