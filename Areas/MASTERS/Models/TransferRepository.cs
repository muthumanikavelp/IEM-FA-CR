using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace IEM.Areas.MASTERS.Models
{
    public interface TransferRepository
    {
        IEnumerable<TransferDataModel> SelectModule();
        IEnumerable<TransferDataModel> SelectedId(int id);
        DataTable Selectedownerid(int id);
        DataTable SelectOwnership(string modulename);
        IEnumerable<TransferDataModel> SelectEmployee();
        string InsertOwner(TransferDataModel trn);
        string Delete(int tranid);
        string DeleteOwner(int tranid);
        string InsertOwnerDetails(string[] SelectedOwner);
        string EditOwner(TransferDataModel trn);
        string EditOwnerDetails(string[] SelectedOwner,int transid);
        IEnumerable<TransferDataModel> Select();
        IEnumerable<TransferDataModel> Search(string TransferDateFrom = null, string TransferDateTo = null, string TransferEmployeeId = null, string TransferEmployeeTOId = null, string TransferModuleId = null);
        //Dhasarathan 14-11-2016
        DataTable SelectEmployeeQueue(int empfromid, int emptoid, int transfertype, int moduleid, int subDocType, int ecfstatus);
        string TransferOwnerShip(int empfromid, int emptoid, int transfertype, int refflag, int refgid, int queuegid);
        DataTable SelectEmployeeDetails(string txt);
        DataTable GetEcfSubDocType();
        //Dhasarathan
    }
    public interface MyprofileRepository
    {
        IEnumerable<MyProfileDataModel> SelectEmployeeDetails();
        IEnumerable<MyProfileDataModel> GetMenu();
        MyProfileDataModel menu(int id);
        List<MyProfileDataModel> GetHierarchy();
    }
    public interface EmployeeReleaseRepository   
    {
        IEnumerable<EmployeeRelease> GetEmployeeDetails(int LoginUserGid); 
        void UpdateEmployeeDetails(EmployeeRelease objEmpRelease);  
    }
    public interface BranchRepository
    {
        IEnumerable<BranchDataModel> SlectBranchType();
        IEnumerable<BranchDataModel> SelectCity();
        IEnumerable<BranchDataModel> SelectLoction();
        IEnumerable<BranchDataModel> SelectEmployee();
        IEnumerable<BranchDataModel> SelectEmployeeSearch(string EmployeeName, string EmployeeCode);
        IEnumerable<BranchDataModel> SelectBranch();
        IEnumerable<BranchDataModel> SelectBranchEdit(int id);
        string InsertBranch(BranchDataModel bran);
        string UpdateBranch(BranchDataModel bran);
        string EditBranch(BranchDataModel bran);
        string DeleteBranch(int id);
        IEnumerable<BranchDataModel> Search(string BranchCode = null, string BranchName = null, string BranchType = null, string City = null, string Branch = null, string ActiveStatus = null);
        DataTable BranchLoad();

        //--Pandiaraj 29/06/2017 GSTIN add

        IEnumerable<BranchDataModel> getPincode();
        IEnumerable<BranchDataModel> GetDistrictByStateId(string stateId);
    }
    public interface TravelAccomodationRepository
    {
        IEnumerable<BranchDataModel> SelectTravelAccomodation();
        IEnumerable<BranchDataModel> SelectTravelAccomodationEdit(int id);
        string InsertTravelAccomodation(BranchDataModel bran);
        string UpdateTravelAccomodation(BranchDataModel bran);
        string DeleteTravelAccomodation(int id);
    }
    public interface PayBankRepository
    {
        IEnumerable<PayBankDataModel> SelectPayBankGrid();
        IEnumerable<PayBankDataModel> SelectBank();
        IEnumerable<PayBankDataModel> SelectEmployeeSearch(string EmployeeName, string EmployeeCode);
        string InsertPayBank(PayBankDataModel pay);
        string UpdatePayBank(PayBankDataModel pay);
        string DeletePayBank(int id);
        PayBankDataModel SelectEditPayBank(int pay_gid);
    }
    public interface othersystemintegrationRepository
    {
        DataTable BranchLoad();
        IEnumerable<othersystemdatamodel> SelectFC();
        IEnumerable<othersystemdatamodel> SelectCC();
        IEnumerable<othersystemdatamodel> Product();
        IEnumerable<othersystemdatamodel> GL();
        IEnumerable<othersystemdatamodel> Employee();
    }
    
}