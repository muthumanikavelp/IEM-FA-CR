using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace IEM.Areas.FLEXIBUY.Models
{
    public interface PRNewRepository
    {
        DataSet GetPRHeaderDetails();
        DataSet GetUOM();
        int AddPRParentDetailsList(PRNewEntity PRParentDetailsInsert);
        DataSet GetPRDetailsAll(int prgid = 0);
        DataSet GetPRDetailParent(int prdetgid = 0);
        int AddPRParentDetailsListNew(PRNewEntity PRParentDetailsInsert);
        int UpdatePRParentList(PRNewEntity PRParentDetailsInsert);
        int DeletePRParentDetails(int refgid = 0, string deletefor = null);
        int SaveAsDraft(PRNewEntity PRParentDetailsInsert);
        string SubmitApprover(PRNewEntity PRParentDetailsInsert);
        int RejectApprover(PRNewEntity PRParentDetailsInsert);
        string GetDelmatemployee(PRNewEntity PRParentDetailsInsert);
        int getprrpvgid(string prheadergid);
        int CreatePRAttachment(Attachments PRParentDetailsInsert);
        DataSet GetPRAttachments(string attachmentfor = "", int refgid = 0);
        int DeletePRAttachment(Attachments PRParentDetailsInsert);
        DataTable DoClone(int prgid = 0);
        int CheckAmount(int prgid = 0);
    }
}