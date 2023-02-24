using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IEM.Areas.MASTERS.Models
{
    public class FbEntity
    {
        public int slNo { get; set; }
      
        //servicedropdownlist
        public SelectList serviceList { get; set; }
        public int serviceGid { get; set; }
        public string serviceName { get; set; }
        //Service
        public int serviceParentGid { get; set; }
        public int productParentGid { get; set; }
        public string service { get; set; }
        public string serviceDescription { get; set; }
        //Product
        public int productGid { get; set; }
        public string productCode { get; set; }
        public string productName { get; set; }
        public string parentProduct { get; set; }

        public string productService { get; set; }
        public string productServflag { get; set; }
        public string parentFlag { get; set; }
        public string productDescription { get; set; }
        public string category { get; set; }
        public List<FbEntity> lListProdService { get; set; }
        public SelectList categoryList { get; set; }
        public int productId { get; set; }
        public string assetCategory { get; set; }
        public Int32 assetGid { get; set; }

        public Int32 SelectedAssetCatGid { get; set; }

        public SelectList AssetSubCatList { get; set; }  
        public string AssetSubCat { get; set; }
        public Int32 AssetSubCatGid { get; set; }
        public Int32 SelectedAssetSubCatGid { get; set; }


        public SelectList ExpNatureList { get; set; }
        public string ExpNature { get; set; }
        public Int32 ExpNatureGid { get; set; }
        public Int32 SelectedExpNatureGid { get; set; }   

        //expensecategory dropdownlist 
        public SelectList expenseList { get; set; }
        public string expenseCategory { get; set; }
        public Int32 expenseGid { get; set; }
        public Int32 SelectedExpGid { get; set; }  

        public string glCode { get; set; }
        public string result { get; set; }

        public IEnumerable<SelectListItem> Data { get; set; }

        //productdropdownlist
        public SelectList productList { get; set; }
        public int parentProductGid { get; set; }
        public string parentProductName { get; set; }
     
    }
    public class ProjectOwner
    {
        //Project Owners
        public int slNo { get; set; }
        public int projOwnerGid { get; set; }
        public string projOwner { get; set; }
        public int employeeGid{get;set;}
        public string empCode{get;set;}
        public string empName{get;set;}
        public string empStatus{get;set;}
        public string result { get; set; }
        public List<ProjectOwner> lListProjOwner { get; set; }
    }
    public class BudgetOwner
    {
        public int slNo { get; set; }
        public int BudgetOwnergid { get; set; }
        public string BudgOwner { get; set; }
        public int employeeGid { get; set; }
        public string empCode { get; set; }
        public string empName { get; set; }
        public string empStatus { get; set; }
        public string result { get; set; }
        public List<BudgetOwner> lListBudgOwner { get; set; }
    }
}