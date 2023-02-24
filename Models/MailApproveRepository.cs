using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace IEM.Models
{
    public interface MailApproveRepository
    {
        DataTable IsQueueClosed(int queuegid);
        string CheckSupplierIsLocked(string suppliercode, int empgid);
        string IsExistingApprover(int suppliergid, int empgid);
        DataTable GetNextApprover(string supcode);
        DataTable GetMailDetailsApprove();
        void SubmitToNextQueueMail(string queueto = "", string requesttype = "", string remarks = "", int actionstatus = 1, int skipflag = 0);
        string GetSupIdForMail();

        IEnumerable<MailApproveEntity> SelectMailtemplate(string Module, string TransactionType, string gid, string request, string workflow);
        string SelectMailIdsForRoleGroup(string rolegroupname, string queuegid, string workflow, string modulename, string requesttype);
        string mailsentmessage(string mailtemplategid, string refflag, string refgid, string refstatus, string mailalertdate, string to, string cc, string bcc, string subject, string content, string scheduledate, string sendflag, string senddate, string isremoved);
        IEnumerable<MailApproveEntity> Getmailselectdfield(string id);
        DataTable Selectbyid(string Module, string TransactionType, string gid, string request);
        DataTable USERMailIDTo(string Module, string TransactionType, string gid, string request, string isfinalapproval = null);
        DataTable GetNextApproverLink(string module, string transactiontype, string gid);
        string GetNextApproverLink1(string module, string transactiontype, string gid);
        string GetNextApproverIsMandatory(string TransactionType, string Module, string gid);
        string GetCurrentApprovalStage(string TransactionType, string queuegid);
        string alreadyapproveby(string Gid);
        IEnumerable<MailApprovaleowModel> Getecfqueuedetails(string empcode, string queuegid);
        string Insertapprovalmail(string ecfgid, string empgid, string queuegid, string approvalsrequest);
    }
}