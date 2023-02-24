using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IEM.Areas.MASTERS.Models
{
    public class TransferDataModel
    {
        public int TranferId { get; set; }
        public int EmployeeId { get; set; }
        public int EmployeeIdTo { get; set; }
        public int ModuleId { get; set; }
        public int OwnerShipId { get; set; }
        public string EmployeeName { get; set; }
        public string ModuleName { get; set; }
        public string OwnerShipName { get; set; }
        public string TranferFrom { get; set; }
        public string TransferTo { get; set; }
        public string Module { get; set; }
        public string TransferDate { get; set; }
        public string Remarks { get; set; }
        public IList<SelectListItem> OwnerShip { get; set; }
        public string[] lstSelectedOwnerGid = new string[32];

    }
    public class MyProfileDataModel
    {
        public string employee_gid { get; set; }
        public string employee_code { get; set; }
        public string employee_name { get; set; }
        public string employee_gender { get; set; }
        public string employee_status { get; set; }
        public string employee_dob { get; set; }
        public string employee_doj { get; set; }
        public string employee_addr1 { get; set; }
        public string employee_city_name { get; set; }
        public string employee_personal_email { get; set; }
        public string employee_office_email { get; set; }
        public string employee_card_no { get; set; }
        public string employee_iem_designation { get; set; }
        public string employee_grade_code { get; set; }
        public string employee_dept_name { get; set; }
        public string employee_fccc_code { get; set; }
        public string employee_cc_name { get; set; }
        public string employee_fc_name { get; set; }
        public string employee_supervisor { get; set; }
        public string employee_era_acc_no { get; set; }
        public string employee_era_bank_code { get; set; }
        public string employee_era_ifsc_code { get; set; }
        public string hris_designation { get; set; }
        public string branch_flag { get; set; }
        public string iem_designation { get; set; }
        public string employee_function_name { get; set; }

        public string employee_ou_name { get; set; }
        public string employee_product_name { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public string url { get; set; }
        public int ParentId { get; set; }
        public int SortOrder { get; set; }
        public List<menu> menu { get; set; }
        public List<MyProfileDataModel> lstMyProfileDataModel { get; set; }
    }
    public partial class menu
    {
        public int menu_gid1 { get; set; }
        public string menu_name1 { get; set; }
        public int parentID1 { get; set; }

    }
    public class EmployeeRelease
    {
        public int _EmployeeGid { get; set; }
        public string _EmployeeCode { get; set; }
        public string _EmployeeName { get; set; }
    }
    public class Category
    {
        public int ID { get; set; }
        public int? Parent_ID { get; set; }
        public string Name { get; set; }
    }
    public class SeededCategories
    {
        public int? Seed { get; set; }
        public IList<Category> Categories { get; set; }
    }
    public class BranchDataModel
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

        //--Pandiaraj 29/06/2017 GSTIN add
        public Int32 pincode_gid { get; set; }
        public string pincode_code { get; set; }
        public SelectList GetPincode { get; set; }
        public string pincode_list { get; set; }
        public SelectList GetDistrictById { get; set; }
        public string gstin { get; set; }

        public int District_gid { get; set; } //Branch master
        public string District { get; set; }
        public int state_gid { get; set; } //Branch master
        public string state { get; set; }
 
    }
    public class TravelAccomodationDataModel
    {
        public string TravelAccommodationTypeId { get; set; }
        public string TravelAccommodationType { get; set; }

    }
    public class PayBankDataModel
    {
        public string PeriodFrom { get; set; }
        public string PeriodTo { get; set; }
        public int Payglid { get; set; }
        public string PayBankStatus { get; set; }
        public string BankName { get; set; }
        public int BankGid { get; set; }
        public string PayBankAcNo { get; set; }
        public string PayBankIfscCode { get; set; }
        public string PayBankBranchName { get; set; }
        public string PayBankGlNo { get; set; }
        public int PayBankGid { get; set; }
    }
    public class InexDataModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string ECFNo { get; set; }
        public int ECFId { get; set; }
        public int Docgid { get; set; }
        public string Docname { get; set; }
        public int DocSubgid { get; set; }
        public string DocSubname { get; set; }
        public int StatusGid { get; set; }
        public string statusname { get; set; }
        public string SupplierorEmployee { get; set; }
        public string SupplierorEmployeename { get; set; }
        public string ECFRaiser { get; set; }
        public string DocTypeName { get; set; }
        public string DocSubTypeName { get; set; }
        public string CreateMode { get; set; }
        public string ClaimMonth { get; set; }
        public string ECFAmount { get; set; }
        public string Despatchdate { get; set; }
        public string CourierName { get; set; }
        public string AWBNo { get; set; }
        public string ECFStatus { get; set; }
        public string CancelBy { get; set; }
        public string CancelDate { get; set; }
        public string ECFDate { get; set; }
        public string Command { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyAmount { get; set; }
        public string CurrencyRate { get; set; }
        public string ReducedAmount { get; set; }
        public string ProcessedAmount { get; set; }
        public string EcfRemark { get; set; }
        public string Reason { get; set; }
        public SelectList SelectDocSubType { get; set; }
        public SelectList SelectCourier { get; set; }
        public SelectList selectdoctypedata { get; set; }
        public SelectList statusnameData { get; set; }
    }
    // add Dhasarathan 14-11-2016
    public class Gidlist
    {
        public string GidData { get; set; }
    }

}