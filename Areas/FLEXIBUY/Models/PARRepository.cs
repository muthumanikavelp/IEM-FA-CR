using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace IEM.Areas.FLEXIBUY.Models
{
    public interface PARRepository
    {
        DataSet GetPARHeaderDetails();
        DataSet GetRequestFor(); 
        DataSet GetPeriodList();
        DataSet GetPARDetailsAll(int pargid = 0);
        int AddPARParentDetailsList(PAREntity PARParentDetailsInsert);
        int AddPARParentDetailsListNew(PAREntity PARParentDetailsInsert);
        DataSet GetPARDetailParent(int pardetgid = 0);
        int AddPARApprover(PAREntity PARParentDetailsInsert);
        DataSet GetPARApprover(int pargid = 0);
        int CreatePARAttachment(Attachments PARParentDetailsInsert); 
        DataSet GetPARAttachments(string attachmentfor = "", int refgid = 0);
        int UpdatePARParentList(PAREntity PARParentDetailsInsert);
        int DeletePARParentDetails(int refgid = 0, string deletefor = null);
        int DeletePARAttachment(Attachments PARParentDetailsInsert);
        int SaveAsDraft(PAREntity PARParentDetailsInsert);
        int CheckFinalApprover(int pargid = 0);
        string SubmitApprover(PAREntity PARParentDetailsInsert);
        int RejectApprover(PAREntity PARParentDetailsInsert); 
    }
}