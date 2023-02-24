using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IEM.Areas.ASMS.Models;
using System.Data;

namespace IEM.Areas.ASMS.Models
{
    public interface IRepositoryRenewal
    {
        IEnumerable<SupplierContractRenewal> GetfullRenewal();
        IEnumerable<SupplierContractRenewal> GetSearchRenewal(string txtSuppliercode, string txtSupplierName, string txtContStartDate, string txtContEndDate, string ddlContractdays);
        // void DeleteClassification(int ClassificationId);
        //  string ClassificationIsExists(string classification);
        //List<SupplierContractRenewal> InsertRenewal(SupplierContractRenewal obj);
        //IEnumerable<SupplierContractRenewal> InsertRenewal(List<SupplierContractRenewal> obj);
        IEnumerable<SupplierContractRenewal> InsertRenewal(SupplierContractRenewal obj);
        IEnumerable<SupplierTriggerRenewal> GetTriggerDetails(SupplierTriggerRenewal subobj);
        // void InsertRenewal(List<SupplierContractRenewal> objrenew);
        string UpdateSupplierTrigger(SupplierTriggerRenewal objtrig);
        //IEnumerable<SupplierActivation> GetActivatequeue();
        DataSet GetActivatequeue(string txtSuppliercode, string txtSupplierName, string ddlSupplierStatus, string ddlSup_Clasification, string ddlRequestType, string ddlRequestStatus);
        DataSet GetDeActivatequeue(string txtSuppliercode, string txtSupplierName, string ddlSupplierStatus, string ddlSup_Clasification, string ddlRequestType, string ddlRequestStatus);
        DataSet Get_SupplierModification(string txtSuppliercode, string txtSupplierName, string ddlSupplierStatus, string ddlSup_Clasification, string ddlRequestType, string ddlRequestStatus);
        DataSet GetActive_emp(string id);
        string Update_SupplierActive(SupplierActivation uobj);
        string DeUpdate_SupplierActive(SupplierDeActvation Deobj);
        DataTable GetTriggerEditDetails(int id);
        string DeleteSupplierTrigger(SupplierTriggerRenewal objtrig);
        string Asms_ActivationQueue(SupplierActivation objAqq);
        string Asms_DeActivationQueue(SupplierDeActvation objDAqq);
        List<SupplierActivation> GetSupHeaderDetailsDashboard(string requsttype, string requeststatus);
        List<SupplierDeActvation> GetSupHeaderDeDetailsDashboard(string requsttype, string requeststatus);
        List<SupplierActivation> GetSuppliercatogory(string value);
        List<SupplierActivation> GetEmployeelist();
        DataSet GetActive_emptemp(string id, string type);
        List<SupplierDeActvation> GetDeSuppliercatogory(string value);
        List<SupplierDeActvation> GetDeEmployeelist();
        List<SupervisoryApproval> GetNextApprovalStage(string reqtype = null, int curlevel = 0);
        int SubmitToNextQueue(string queueto = null, string requesttype = null, string remarks = null, int actionstatus = 1, int skipflag = 0);
        DataTable Getempid(string empid);
        List<SupplierActivation> Sup_MoficationSummery();
        List<SupplierActivation> GetEmployeeListForApproval(string id);
        List<SupplierDeActvation> GetEmployeeDeListForApproval(string id);
        int ASubmitToNextQueue(string queueto = null, string requesttype = null, string remarks = null, int actionstatus = 1, int skipflag = 0);
        DataSet GetpreSupplierDetails(string id, string a);
        List<SupplierDeActvation> GetDSupplierApprovalHistory(string id, string action);
        List<SupplierActivation> GetSupplierApprovalHistory(string id, string action);
        List<SupplierQuery> GetSupplierquery(string SupplierCode, string SupplierName, string SupplierStatus, string ContractFrom, string ContractTo, string Type

            , string categoryname
             , string organizationtypename
             , string servicetypename
             , string employeecode
             , string employeename
            ,string requeststatus
            ,string requesttype
            ,string department
            ,string actionfrom
            ,string actionto);
        IEnumerable<supasmscategory> Getcategory();
        IEnumerable<supasmsorganizationtype> Getorganizationtype();
        IEnumerable<supasmsservicetype> Getservicetypey();
        IEnumerable<supasmsStutas> GetStatussMode();
       // string emplogin(string txtEmployeegid);
        List<SupplierQuery> GetemployeeDepartment();
        List<SupplierQuery> GetRequestType();
      string GetSupIdForMail(int id);
      string GetTriggercount(string days);
     // string supplierLoged(int supp_gid, int loggid);
      string ReleaseLog(string supplier_gid);
      DataTable getproxydetails(int deta);
    }
}