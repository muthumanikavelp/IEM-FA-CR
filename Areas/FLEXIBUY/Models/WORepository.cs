using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace IEM.Areas.FLEXIBUY.Models
{
    public interface WORepository
    {
        DataSet GetWODetails(int cbfdetailsgid = 0);
        int AddParentWODetails(WOEntity objWoEntity); //First WO Detail - Insert WO Header & Generates WO number
        int AddParentWODetails1(WOEntity objWoEntity);  //For Additional WO Details - Update WO Header
        DataSet GetWODetailsParent(int poheadergid = 0);
        DataSet GetWODetailsParentForEdit(int wogid = 0);
        DataSet GetWODetailsChild(int wodetailsgid = 0);  
        int UpdateParentWODetails(WOEntity objWoEntity);
        DataSet GetProductGroup(string flag =null);
        DataSet GetProductByGroup(string flag = null,int ProdGroupGid = 0);
        DataSet GetProductDesc(int prodgid = 0);
        int AddChildWODetails(WOEntity objWoEntity);
        int UpdateChildWODetails(WOEntity objWoEntity);
        DataSet GetBranchAutoComplete(string searchtext = null);
        DataSet GetEmployeeComplete(string searchtext = null);
        int ADDWOShipmentDetails(WOEntity objWoEntity);
        DataSet GetWOShipmentDetails(int wodetailsgid = 0, int Vendorid = 0);
        DataSet DeleteWoParentDetails(int refgid = 0,string deletefor = null);
        int SaveAsDraft(WOEntity objWoEntity);
        string SubmitRaiser(WOEntity objWoEntity);
        string GetDelmatemployeeForWo(WOEntity ObjWOEntity);
        string RejectWO(WOEntity objWOEntity);

        DataSet GetSCNInwardDetails(int WOGid = 0);
        DataSet GetWOSummary();
        string AddInwardDetails(SCNInward objSCNInward);
        List<AuditTrailWO> PopulateAuditTrail(int refgid = 0, string refname = null);
        string ChkWOShipment(int wodetgid = 0);
        DataSet ValidateWOShipment(int wogid = 0);
        DataSet GetVendorNameAutoComplete(string searchtext = null);
        DataSet GetTermCondContent(int termsandcondgid = 0);

        int DeleteWo(int wogid = 0);
        DataSet GetSCNInwardDetailsView(int scngid = 0);
        int InsertWOHeader(int cbfdetgid = 0);
        void InsertWODetails(int cbfdetgid = 0, int wogid = 0);
        void UpdateWOHeaderAmount(int wogid = 0);
        string ChkWOSplitExists(int wodetgid = 0);

        DataSet GetWOAmendment();
        DataTable DoAmend(int wogid = 0);

        string CheckFinalApproverTag(int ref_gid = 0, int ref_flag = 0, int queue_gid = 0);

        DataSet GetPOSummary();
        DataSet GetGRNInwardDetails(int WOGid = 0);
        DataSet GetGRNInwardDetailsView(int scngid = 0);
        string AddInwardDetailsGRN(SCNInward objSCNInward);

        //Dhasarathan 
        int CreateWOAttachment(Attachments POAttachment);
        int DeleteWOAttachment(string id);
        DataSet GetWOAttachments(int scngid);
        //Dhasarathan 
    } 
}