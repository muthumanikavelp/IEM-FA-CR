using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace IEM.Models
{
    public interface IEMRepository
    {
        IEnumerable<MenuModel> GetMenu();
        DataTable GetLoginUserDetails(string empcode);
        void insertloginattempt(string ipaddr, string empcode, string loginstatus, string browser = null); 
        int CheckDuplicateLogin(string empcode);
        void UpdateLoginFlag(int empgid, int updateloginflagfor);
        IEnumerable<GetProxyDetails> GetProxyDetails(string iogincode);
        DataTable getProxyId(string loginid);

        DataSet GetTicketSummary();
        void InsertTicketComments(RaiseTicketEntity RaiseTic);
        void InsertTicketCommentsNew(RaiseTicketEntity RaiseTic);
        DataSet GetTicketSummarySingle(string refflag = null, string itemrefno = null);
        IEnumerable<PolicyDataModel> GetPolicyFiles();
        IEnumerable<PolicyDataModel> GetPolicyFilesView(int id);
        string GetPreviousIPAddress(string empcode);
        string GetPreviousBrowser(string empcode);  
        void ReleaseEmp(string empcode);
        void updateloginattempt(string empcode = null);
        int GetSessionInterval(string empcode = null);
        string AuthorizeEntity(string empcode);
    }
}