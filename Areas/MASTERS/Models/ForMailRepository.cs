using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace IEM.Areas.MASTERS.Models
{
    public interface ForMailRepository
    {
        string USERMailIDFrom(string Module, string TransactionType, string gid, string request);
        DataTable USERMailIDTo(string Module, string TransactionType, string gid, string request, string isfinalapproval = null);
        DataTable USERMailIDTocentralteam(string gid);
        DataTable USERMailIDTocentralteamall(string gid);
        IEnumerable<ForMailEntity> SelectSupplierSearch(string SupplierName, string SupplierCode);
        string selectfcccval(string fc, string cc);
        IEnumerable<ForMailEntity> GetTables();
        string selectpolicy(string Gid);
        IEnumerable<ForMailEntity> SelectMailalertbyid(string idnew);
        string selectfccc(string fc, string cc);
        IEnumerable<ForMailEntity> getuserfunction(string transactiontype, string modulename);
        IEnumerable<ForMailEntity> GetTablescol(string tblname);
        IEnumerable<ForMailEntity> GetModuleType();
        IEnumerable<ForMailEntity> GetTransactionType(string modulename);
        IEnumerable<ForMailEntity> GetWorkFlow(string transactiontype, string modulename);
        IEnumerable<ForMailEntity> getuserdata(string id);
        string Insertmailtemplate(ForMailEntity Mailalertobj);
        string Updatetemplate(ForMailEntity Mailalertobj, string gid);
        string selectdocsubtype(string queuegid, string type);
        DataTable USERMailIDTofs(string raisergid, string type, string PVId);
        IEnumerable<ForMailEntity> SelectMailtemplate(string Module, string TransactionType, string gid, string request, string nextdata2);
        IEnumerable<ForMailEntity> Getmailselectdfield(string id);
        DataTable Selectbyid(string Module, string TransactionType, string gid, string request);
        IEnumerable<ForMailEntity> gettemplatedetails();
        DataTable selectmailfields(string gid);
        string Deletetemplate(string template);
        string mailsentmessage(string mailtemplategid, string refflag, string refgid, string refstatus, string mailalertdate, string to, string cc, string bcc, string subject, string content, string scheduledate, string sendflag, string senddate, string isremoved);
        IEnumerable<ForMailEntity> getloginemployeedetails();
        string SelectMailIdsForRoleGroup(string rolegroupname, string queuegid, string workflow, string modulename, string requesttype);
        DataTable GetNextApproverLink(string module, string transactiontype, string gid);
        string GetNextApproverLink1(string module, string transactiontype, string gid);
        string GetNextApproverIsMandatory(string TransactionType, string Module, string gid);
        string CheckIsFinalApproverFB(string gid);
        string GetCurrApprStage(string queuegid = "", string refname = "");
        string GetWorkFlowFB(string queuegid = "");
        DataTable EftErapaymentAlert(string pvId);
        DataTable ChequeStatusUpdateDetails(string pvId);
        //correct by sugumar 11-18-2016
        DataTable GetBounchresone(string Ecfgi);
        DataTable Getaddress();

    }
}