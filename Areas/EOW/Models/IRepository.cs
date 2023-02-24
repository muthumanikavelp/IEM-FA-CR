using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using IEM.Areas.EOW.Models;
namespace IEM.Areas.EOW.Models
{
    public interface IRepository
    {
        IEnumerable<SupClassificationModel> GetClassification();
        SupClassificationModel GetClassificationById(int ClassificationId);
        void InsertClassification(SupClassificationModel Classifications);
        void DeleteClassification(int ClassificationId);
        void UpdateClassification(SupClassificationModel Classifications);
        string ClassificationIsExists(string classification);
        void Registration(SupClassificationModel sub);
        //IEnumerable<VariableModel> GetSlabDetails();
        string AddSlab(SupClassificationModel sub);
        string AddSlabRange(SupClassificationModel sub);
        IEnumerable<SupClassificationModel> selectslabrange(string slabname, int id);
        IEnumerable<SupClassificationModel> selectslabrangeedit(int id);
        IEnumerable<SupClassificationModel> SelectSlabname();
        IEnumerable<SupClassificationModel> SearchSlabname();
        string UpdateSlab(SupClassificationModel sub);
        string DeleteSlab(SupClassificationModel sub);
        string DeleteSlabIndex(int id);
        string DeleteSlab_all(int sub);
        string DeleteSlab_rangeedit(int sub);
        string IfExistsCheck(SupClassificationModel sub);
        DataTable GetEmployeeGender(string empid);
        string SaveSlab(SupClassificationModel sub);
        string SaveSlabRanges(SupClassificationModel sub);
        IEnumerable<SupClassificationModel> GridLoad();
        IEnumerable<SupClassificationModel> GridLoadEdit(int id);
        string EditSaveSlab(SupClassificationModel sub);
        string EditSaveSlabRanges(SupClassificationModel sub);
    }
    public interface HolidayRepository
    {
        IEnumerable<SupClassificationModel> GetClassification();
        DataTable SelectStates();
        string AddHoliday(SupClassificationModel sub);
        IEnumerable<SupClassificationModel> SelectHoliday();
        string AddHolidayState(SupClassificationModel sub, string[] SelectedState);
        DataTable ViewHoliday(int id);
        string EditHoliday(SupClassificationModel sub, int id);
        string EditHolidayState(SupClassificationModel sub, string[] SelectedState, int id);
        string DeleteHoliday(SupClassificationModel sub, int id);
        string DeleteHolidayState(SupClassificationModel sub, int id);
    }
    public interface RollRepository
    {
        DataTable selectchildnodevalue(int parent_gid);
        DataTable FindParenNode();
    }
    public interface RoleEmployee
    {
        IEnumerable<SupClassificationModel> SelectRole();
        IEnumerable<SupClassificationModel> SelectRole(string role, string rolegroup);
        DataTable SelectRoleName(int id);
        DataTable RoleName(int id);
        DataTable SelectRoleNamebyempname(int id, string empname);
        IEnumerable<SupClassificationModel> SelectEmployeeDetails(int code);
        IEnumerable<SupClassificationModel> SelectEditEmployeeDetails(string code);
        string EmployeeRoleSubmit(SupClassificationModel sub);
        string DeleteEmployeeRole(int id, int roleid);
        DataTable SelectEmployeeId(SupClassificationModel sub);
        IEnumerable<CentraldataModel> SelectEmployee();
        IEnumerable<CentraldataModel> SelectEmployee(string raisername, string raisercode);
    }
    public interface EmployeeRole
    {
        DataTable SelectedRole(int id);
        DataTable RoleList();
        DataTable SelectEmployeeName(int id);
        DataTable SelectRoleById(int id);
        IEnumerable<SupEmployeeRole> SelectEmployee();
        IEnumerable<SupEmployeeRole> SelectEmployee(string code, string name);
        IEnumerable<SupEmployeeRole> SelectRole();
        IEnumerable<SupEmployeeRole> SelectRoleDetails(int rolegid);
        string RoleEmployeeSubmit(SupEmployeeRole sub, string[] uncheck, string[] check);
        string DeleteRoleEmployee(int id);
        DataTable SelectEmployeeNamebyRole(int id, string role);

    }
    public interface CentralRepository
    {
        IEnumerable<CentraldataModel> SelectrejectDetails(string txtdbdocdateReject, string txtdbdocnoReject, string txtdbdocamountReject);
        void Insertapprovedactionreject(Approveraction EmployeeeExpense, string ecfgid, string invoicegid, string empgid, string ecftype, string queuegid, string queueamt);
        //  string EcfraiserReject(string[] SelectedValues);
        IEnumerable<CentraldataModel> CentralRejectDetails();
        void Insertapprovedaction(Approveraction EmployeeeExpense, string ecfgid, string invoicegid, string empgid, string ecftype, string queuegid, string queueamt);
        IEnumerable<CentraldataModel> SearchReport(string SupplierType, string RecivedDateFrom, string RecivedDateTo, string invoiceAmount, string InvoiceDate, string raisercode, string raisername, string suppliercode, string Suppliername, string Department, string InvoiceNo, string PONO, string Rolelist);
        IEnumerable<CentraldataModel> SearchInwardReport(string ReceivedDateFrom, string ReceivedDateTo, string InvoiceDateFrom, string InvoiceDateTo, string Status, string Ecfno, string InvoiceNo, string searchtype);
        IEnumerable<CentraldataModel> SelectStatus();
        string AddReturn(string[] SelectedValues, string Reason);
        string AddHold(string[] SelectedValues, string Reason);
        string UpdateHold(string[] SelectedValues, string Reason);
        DataTable SearchReportNew();
        IEnumerable<CentraldataModel> SelectCentralInward();
        IEnumerable<CentraldataModel> SelectCentralMaker();
        IEnumerable<CentraldataModel> SelectCentralHold();
        CentraldataModel SelectCentralInwardbyid(int Id);
        CentraldataModel SelectCentralInwardbyidForMaker(int Id);
        IEnumerable<CentraldataModel> SelectSupplier();
        IEnumerable<CentraldataModel> SelectSupplierSearch(string SupplierName, string SupplierCode, string PanNo);
        string AddSupplier(CentraldataModel cen);
        string EditSupplier(CentraldataModel cen);
        string DeleteSupplier(int id);
        IEnumerable<CentraldataModel> Search(string SupplierType, string RecivedDateFrom, string RecivedDateTo, string invoiceAmount, string InvoiceDate, string raisercode, string raisername, string suppliercode, string Suppliername, string Department, string InvoiceNo, string PONO);
        IEnumerable<CentraldataModel> SearchHold(string SupplierType, string RecivedDateFrom, string RecivedDateTo, string invoiceAmount, string InvoiceDate, string raisercode, string raisername, string suppliercode, string Suppliername, string Department, string InvoiceNo, string PONO);
        IEnumerable<CentraldataModel> SearchMaker(string SupplierType, string RecivedDateFrom, string RecivedDateTo, string invoiceAmount, string InvoiceDate, string raisercode, string raisername, string suppliercode, string Suppliername, string Department, string InvoiceNo, string PONO);
        IEnumerable<CentraldataModel> SelectEmployee();
        IEnumerable<CentraldataModel> SelectEmployeeDelegation();
        IEnumerable<CentraldataModel> SelectEmployeeSearch(string EmployeeName, string EmployeeCode);
        IEnumerable<CentraldataModel> SelectDepartment(string department);
        CentraldataModel SelectEmployeeSearch111(string EmployeeName, string EmployeeCode);
        IEnumerable<CentraldataModel> SelectPodetails(string EmployeeName, string EmployeeCode, int poid, string claimType);
        IEnumerable<CentraldataModel> SelectPodetailswithoutarg(int SupId, string claimType);
        string UpdateCentralHold(string[] SelectedValues);
        string UpdateCentralHoldRelease(string[] SelectedValues);
        string UpdateCentralReturnback(string[] SelectedValues);
        string AddSupplierDuplicateCheck(CentraldataModel cen);
        IEnumerable<CentraldataModel> SelectCentralCheckerDetails(string txtdbdocdate, string txtdbdocno, string txtdbdocamount);
        IEnumerable<CentraldataModel> CentralCheckerDetails(string LoginEmpCode);
        IEnumerable<CentraldataModel> CentralMakerView(int id);
        IEnumerable<CentraldataModel> SearchByDate();
        IEnumerable<CentraldataModel> HoldSearchByDate();
        IEnumerable<CentraldataModel> MakerSearchByDate();
        IEnumerable<CentraldataModel> SelectPONo(CentraldataModel delnote);
        IEnumerable<CentraldataModel> SelectPONo();
        IEnumerable<CentraldataModel> SelectPONoByid(int gid);
        string Ecfraiser(string[] SelectedValues);//thirumalai
        string selectsuppcode(string EmployeeeGid);//thirumalai
        DataTable selectcurrency(string EmployeeeGid);//thirumalai
        string CheckPono(string PONO);
        CentraldataModel SelectCentralInwardEditbyid(int Id);
        IEnumerable<CentraldataModel> GetCurrency();
        IEnumerable<CentraldataModel> AutofilterEmployee(string EmployeeName,string EmployeeCode);


        //--Pandiaraj 18-02-2019
        DataTable bulkinvoice(string bulkinvoice, string filename);
        DataSet forexcel(int uploadgid, string type);

    }
    public interface ECFRepository
    {
        DataTable myapprovedlistSearchexcel();
        IEnumerable<ECFDataModel> myapprovedlistSearch();
        IEnumerable<ECFDataModel> ReportSearch();
        IEnumerable<ECFDataModel> SelectDocType();
        IEnumerable<ECFDataModel> SelectDocsubType();
        IEnumerable<SelectCourier> SelectCourier();
        IEnumerable<SelectDocSubType> SelectDocSubType(int gid);
        IEnumerable<ECFDataModel> SelectStatus();
        DataTable SearchNew();
        DataTable SearchNewecfinvoicetext();
        //IEnumerable<ECFDataModel> Search();
        IEnumerable<ECFDataModel> Searchponumberbased();
        IEnumerable<ECFDataModel> SearchTEST();
        IEnumerable<ECFDataModel> Search(string EcfDateFrom = null, string EcfDateTo = null, string DocType = null, string docsubtype = null, string Code = null, string Name = null, string ECFClaimMonth = null, string queryempsup = null, string ECFDespatchDateTo = null, string ecfnumber = null, string ecfamount = null, string Ecfstatus = null, string ECFMode = null, string command = null, string Suppliercode = null, string Suppliername = null);
        IEnumerable<ECFDataModel> CancellationSearch();
        IEnumerable<ECFDataModel> CancellationSearch(string EcfDateFrom, string EcfDateTo, string DocType, string DocSubType, string Code, string Name, string ECFClaimMonth, string SupplierorEmployee, string ECFDespatchDateTo, string Suppliername, string ecfmode, string ECFStatus, string ecfamount);
        IEnumerable<ECFDataModel> DespatchSearch();
        IEnumerable<ECFDataModel> DespatchSearch(string EcfDateFrom, string EcfDateTo, string DocType, string DocSubType, string Code, string Name, string ECFClaimMonth, string suporemp, string ECFDespatchDateTo, string Suppliername, string ecfamount, string ecfnumber, string ECFStatus, string ecfmode);
        IEnumerable<ECFDataModel> DeletionSearch();
        IEnumerable<ECFDataModel> DeletionSearch(string EcfDateFrom, string EcfDateTo, string DocType, string DocSubType, string Code, string Name, string ECFClaimMonth, string SupplierorEmployee, string ECFDespatchDateTo, string Suppliername, string ecfnumber, string ecfmode, string ECFStatus, string ecfamount);
        IEnumerable<ECFDataModel> AllPrintdata();
        IEnumerable<ECFDataModel> View(int id);
        IEnumerable<ECFDataModel> Viewwithqueue(int id);
        string UpdateCancellation(int id);
        IEnumerable<ECFDataModel> SelectDespatch(int id);
        IEnumerable<ECFDataModel> SelectDespatchByEcfno(int id);
        string Delete(ECFDataModel obj, int id);
        string DespatchUpdate(ECFDataModel ecf, string[] ecfids);
        string IfCheck(ECFDataModel ecf);
        IEnumerable<ECFDataModel> SelectDespatchByEcfNumber(string id);
        IEnumerable<ECFDataModel> HoldReport(string EcfDateFrom, string DocType, string Code, string amount);
        IEnumerable<ECFDataModel> HoldDetails();
        string Update(int id, string remark);
        DataTable getdoctypecode(int id);
        DataTable getdocsubtypecode(int id);
        DataTable getecfnumbergid(string ecfno);
        IEnumerable<ECFDataModel> DespatchReport();
        DataTable DespatchIndexExportExcel();
        DataTable DespatchReportExportExcel();
        DataTable POWOReportExportExcel();
        //correction by dhasarathan
        DataTable EcfReport(string EcfDateFrom = null, string EcfDateTo = null, string DocType = null, string docsubtype = null, string Code = null, string Name = null, string ECFClaimMonth = null, string queryempsup = null, string ECFDespatchDateTo = null, string ecfnumber = null, string ecfamount = null, string Ecfstatus = null, string ECFMode = null, string command = null, string Suppliercode = null, string Suppliername = null);
        IEnumerable<ECFDataModel> EcfReport(string EcfDateFrom = null, string EcfDateTo = null, string EcfNo = null, string EcfAmount = null, string InvoiceDateFrom = null, string InvoiceDateTo = null, string InvoiceNo = null, string InvoiceAmount = null, string EcfMode = null, string Ecfstatus = null,string suppliercode = null, string suppliername = null, string command = null);
    }
    public interface RetentionCancel
    {
        IEnumerable<RetentionCancelData> RetentionReleaseGrid();
        IEnumerable<RetentionCancelData> Search(string ReleaseDate = null, string ECFDate = null, string ECFNo = null, string InvoiceNo = null, string Suppliercode = null, string Suppliername = null, string extendeddate = null);
        RetentionCancelData SelectById(int id);
        string CancelUpdate(RetentionCancelData ret);
    }
    public interface ProxyRepository
    {
        IEnumerable<ProxyDataModel> SelectEmployeeSearch(string EmployeeName, string EmployeeCode);
        IEnumerable<ProxyDataModel> SelectEmployee();
        string SaveProxy(ProxyDataModel proxy);
        IEnumerable<ProxyDataModel> SelectProxy();
        IEnumerable<ProxyDataModel> SelectEditProxy(int id);
        string EditProxy(ProxyDataModel proxy);
        string DeleteProxy(ProxyDataModel proxy);
        IEnumerable<ProxyDataModel> SearchProxy(string DateFrom, string DateTo, string ProxyTo);
    }
    public interface PrintRepository
    {
        printdatamodel SelectEmployeeSearch(string ECFgid, string ECFtype);
        printdatamodel SelectPrintGeneral(string ECFgid, string ECFtype);
        printdatamodel SelectPrintArf(string ECFgid, string ECFtype);
        printdatamodel ToGetDeclaration(string ecfgid);
    }
    public interface InexRepository
    {
        IEnumerable<InexDataModel> SelectDocType();
        IEnumerable<InexDataModel> SelectDocSubType();
        IEnumerable<InexDataModel> SelectStatus();
        IEnumerable<InexDataModel> Search();
        IEnumerable<InexDataModel> Search(string EcfDateFrom = null, string EcfDateTo = null, string DocType = null, string docsubtype = null, string Code = null, string Name = null, string ECFClaimMonth = null, string queryempsup = null, string ECFDespatchDateTo = null, string Suppliername = null, string ecfnumber = null, string ecfamount = null, string Ecfstatus = null, string ECFMode = null, string command = null);
        IEnumerable<InexDataModel> SelectView(int id);
        string UpdateInex(int id);
        string RejectInex(int id);
        DataTable getecfstatusname(int ecfstatusgid);
        IEnumerable<InexDataModel> SelectDocSubType(int gid);
        DataTable getdocsubtypecode(int id);
    }
    public interface CentralReportRepository
    {
       IEnumerable<CentraldataModel> MISReportLoad(string InvoiceDate, string invoiceAmount, string ECFNo, string ECFDate, string Status);
       IEnumerable<CentraldataModel> Collection_MISReport(string InvoiceDate, string invoiceAmount, string ECFNo, string ECFDate, string Status);
       IEnumerable<CentraldataModel> MIS_DepartmentwiseAndVendorwise_Report(string InvoiceDate, string invoiceAmount, string ECFNo, string ECFDate, string Status);
       IEnumerable<CentraldataModel> InvoiceMIS_VendorwiseAndPOWOwise_Report(string InvoiceDate, string invoiceAmount, string ECFNo, string ECFDate, string Status);
       IEnumerable<SelectCourier> ChangePettyReport(string ARFFrom, string ARFTo, string ARFAmount, string ARFNo, string RaiserCode, string RaiserName, string BranchCode);
       IEnumerable<SelectCourier> ChangePettyReportPopup(string Ecfid);
       string AddPettyTransfer(SelectCourier cen);
       IEnumerable<SelectCourier> AutofilterEmployeeTo(string EmployeeName, string EmployeeCode);
       IEnumerable<SelectCourier> ChangePettyLogPopup(string Ecfid);
       IEnumerable<SelectCourier> BranchCodeAutocomplete(string BranchCode);
       DataTable ExcelExportPettyCashChange(string ARFFrom, string ARFTo, string ARFAmount, string ARFNo, string RaiserCode, string RaiserName, string BranchCode);
    }


    public interface ReportRepository
    {
        DataSet GetEmpTravelDetails(string FromDate, string ToDate);
        DataSet GetEmpTravelDetails_Download(string FromDate, string ToDate);
    }

}