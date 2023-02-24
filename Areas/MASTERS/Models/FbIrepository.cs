using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using IEM.Areas.MASTERS.Models;

namespace IEM.Areas.MASTERS.Models
{
    public interface FbIrepository
    {

       // string InsertProduct(fb_Entity objDetails, string Radio, string ddlflag,string ddlparent, string RadioGL);
        IEnumerable<FbEntity> GetProductList();
        IEnumerable<FbEntity> GetProductListUpd();
        List<FbEntity> GetServiceList();
        List<FbEntity> GetServiceListUpd();
        List<FbEntity> GetAssetCategory();
        string GetAssetGl(int assetGid);
        string GetExpenseGl(int expenseGid);
        List<FbEntity> GetExpenseCategory();
        IEnumerable<FbEntity> GetProductServiceDetails();
        List<FbEntity> GetProductServiceDetailsById(int ProductGid);
        //string UpdateProductDetails(fb_Entity objDetails, string RadioGL, string ddlflag,string ddlparent,string Radio, string productID);
        void DeleteProductServiceDetails(FbEntity objDelete);
        DataSet GetCategory(int parentProductGid);
        string InsertProductServiceDetails(FbEntity objDetails);
        string UpdateProductServiceDetails(FbEntity objDetails);
        int GetParentProductGid(int productGid);
        //projectOwner

        List<ProjectOwner> GetEmployeeDetails();
        List<ProjectOwner> GetProjectOwner();
        string InsertProjOwnerDetails(ProjectOwner objOwner, string empName, string empCode, string employeeGid);
        string DeleteProjectOwnerDetails(ProjectOwner objOwner);
        string GetProjectOwnerById(int id);
        //budgetOwner
        List<BudgetOwner> GetEmpDetails();
        List<BudgetOwner> GetBudgetOwner();
        string InsertBudgOwnerDetails(BudgetOwner objOwner, string empName, string empCode, string employeeGid);
        void DeletebBudgetOwnerDetails(BudgetOwner objOwner);
        string GetBudgetOwnerById(int id);

        List<FbEntity> GetExpNatureList();
        List<FbEntity> GetAssetSubCatList(int assetgid = 0);
        List<FbEntity> GetExpenseCatList(int expnatgid = 0);

        string UpdateProdServNew(FbEntity objDetails); 
    }
}