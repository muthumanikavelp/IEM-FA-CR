using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace IEM.Areas.FLEXIBUY.Models
{
    public interface IrepositoryAn
    {
        
       // IEnumerable<CbfParHeader> GetParHeaderopexsearch(CbfPrHeader cbfhead);
        Employee_gid GetEmployeeDetails(string code);
        string VendorPardetails_id(string lsdate1);
        string Rquestvalue(string reqvl);
        CbfSumEntity GetCbfSumry();
        List<CbfSumEntity> getStatus();
        string getStatusName(int status_gid);
        List<CbfRaiseHeader> GetList();
        List<CbfRaiseHeader> GetList1();
        List<CbfRaiseHeader> GetBranchCode();
        CbfSumEntity GetPrHeader(int lnPrHeaderGid);
        CbfSumEntity GetParHeader(int cbfgid = 0, string isBudgeted = "Y");
        // CbfSumEntity GetParHeader(CbfParHeader budgeted);
        CbfSumEntity GetPrDetail(CbfPrHeader PrHeGid);
        CbfSumEntity GetCbfDetails(DataTable dt);
        DataTable DtTable(CbfDetails PrDtGid);
        string InsertCbfHeader(CbfRaiseHeader Classifications, DataTable dt);
        string InsertCbfHeader1(CbfRaiseHeader Classifications, DataTable dt);
        CbfSumEntity GetFccc();
        CbfSumEntity GetFcccSerach(FcccMaster fc);
        string GeneartePrRefno(int id, string prefix);
        CbfSumEntity AttachmentnameInline(CbfSumEntity filename);
        CbfSumEntity MyAttachmnetCbfDetails(string filename);
        CbfSumEntity Attachmentname(CbfSumEntity filename);
        string GetStatusName(int status_gid);
        CbfSumEntity Getcbfaiseredit(string number);
        CbfSumEntity CbfPardetails(CbfParHeader par);
        List<CbfDetails> Getproductlist();
        CbfSumEntity GetProdServDetails(int id);
        List<CbfDetails> GetUomList();
        CbfSumEntity GetCbfParDetails(DataTable dt);
        CbfSumEntity CbfSave(DataTable dt, int i);
        string UpdateCfHeaderSave(CbfRaiseHeader cbfheader);
        string UpdateCfHeaderSubmit(CbfRaiseHeader cbfheader);
        CbfSumEntity GetCbfPardetails(CbfDetails objCbfdetailsSavePar);
        void DeleteProductServiceDetails(string objDelete);
        void DeleteAttachment(string objDelete);
        CbfSumEntity GetPrDetailEdit(CbfPrHeader PrHeGid);
        CbfSumEntity AttachmnetDetails(CbfSumEntity filename);
        string GetFcccSerachText(string lsFccc);
        string Cancel(CbfRaiseHeader cbfheader);
        string Delete(CbfRaiseHeader cbfheader);
        CbfSumEntity parattachment(int lnPardetails_gid);
        List<CbfRaiseHeader> GetBudgetowner();
        CbfSumEntity CbfPardetailsedit(CbfParHeader par);
        int GetBudgetownerGid(string lsNumber);

        string GetBranchNameForEdit(int branchid);
        //------Checker----------//
        CbfSumEntity GetCbfSumryChecker();
        CbfSumEntity Getcbfchecker(string lsnumber);
        List<CbfCheckerHeader> GetSupervisor();
        void InsertConversation(RaiserTicket objTicket);
        IEnumerable<RaiserTicket> SelectDate(RaiserTicket objTicket);
        string QueueProcess(quenue queue);
        string QueueProcessreject(quenue queue);
        List<Attachment> attachment(string Cbfheader_gid);
        //=========Cacnellation========//
        CbfSumEntity GetCbfcancellationSummary();
        string Approvalcancellation(string remarks);
        string Rejectcancellation(string remarks);

        //===========Closure===========//
        List<CbfCheckerSummary> GetCbfClosureSummary();
        CbfSumEntity GetCbfClosureSummary1();
        string Closure(CbfRaiseHeader cbfheader);
        CbfSumEntity GetCbfClosureChkSummary();
        string Rejectclosure(string remarks);
        string Approvalclosure(string remarks);
        CbfSumEntity GetUltilizedamount(CbfSumEntity objCrheader);

        //==========Bluk Closure===================//
        string GetCBFClosureExcelValid(string validate, string action);
        string BulkCBFInsert(DataTable dt, int validRecords, string fileName);
        //===========Approval========//
        CbfSumEntity GetCbfApprovalSummary();

        //========Reopen============//
        CbfSumEntity GetCbfReopenSummary();
        string Reopen(CbfRaiseHeader cbfheader);
        CbfSumEntity GetCbfReopenChkSummary();
        string Rejectreopen(string remarks);
        string ApprovalReopen(string remarks);

        //===========Bulk Reopen============/
        string GetCBFReopenExcelValid(string validate, string action);
        string BulkReopenInsert(DataTable dt, int validRecords, string fileName);


        //=========PARDEtails============//
        CbfSumEntity GetParDetailsForPAR(string lsParheader_gid);
        CbfSumEntity GetParDetailsSave(DataTable dt);
        CbfSumEntity GetParSummary();
        List<Employee_gid> GetEmployeeDetails();
        CbfSumEntity SaveEmployee_details(DataTable dt, List<CbfSumEntity> lst = null);
        string Insertparheader(Parheader objParheader, DataTable objDt, DataTable objapproval);
        CbfSumEntity getParEdit(string lsparheader_gid);
        string InsertparheaderSubmit(Parheader objParheader, DataTable objDt, DataTable objapproval);
        List<ParDetails> GetCapex();
        List<ParDetails> GetBudjected();
        string DeletePAR();
        void DeletePARapproval(string objDelete);
        void DeletePARdetailsLine(string objDelete);
        string updateparheaderSubmit(Parheader objParheader, DataTable objDt, DataTable dt, DataTable objapproval);
        string updateparheader(Parheader objParheader, DataTable objDt, DataTable dt, DataTable objapproval);
        //=======parchecker=====//
        CbfSumEntity GetParchecker();
        string parcheckerapproval(string remarks);
        string parcheckerReject(string remarks);
        CbfSumEntity GetCBFSummaryHeader();

        //========login check for PIP or IT==========//
        string GetGroupForUser();
        string GetGroupForUserCBF(); 
        string GetReqGroup();
        //========vendorselection=========//
        List<vendorselection> getvendorselection();
        CbfSumEntity Getvendorgridcopex(DataTable dt);
        string vendorselectionsave();
        CbfSumEntity GetPrHeaderopex(int lnPrHeaderGid);
        CbfSumEntity GetParHeaderopex(CbfPrHeader cbfhead);
        CbfSumEntity GetPrDetailopex(CbfPrHeader PrHeGid);
        CbfSumEntity VendorPardetails(CbfParHeader par);
        List<CbfDetails> GetServicelist();
        CbfSumEntity GetCbfParDetailsOpex(DataTable dt);
        CbfSumEntity GetCbfDetailsOpex(DataTable dt);
        DataTable DtTableOPex(CbfDetails prdtgid);
        string InsertCbfHeaderopex(CbfRaiseHeader cbfheader, DataTable dt);


        //=========AuditTrail=============//
        List<CbfSumEntity> PopulateAuditTrail(CbfSumEntity pat,string audittrialfor = null);

        //==============Delmate=================//
        string GetDelmatemployee(string prhgid, string status, quenue objque, string remarks);
        int Getbranchgid();
        DataSet getdelmatfinalapprover(int delmatetypegid, string delmatname, decimal tamount);
        List<shipment> getbranchdetails();
        string attach_parinline(ParDetails parcat);
        string attach_parinlineEdit(ParDetails parcat);
        List<Attachment> Getattachment_val(CbfSumEntity dateval);
        string Deleteattach(int attachId);
        string attach_parinline_cbf(CbfSumEntity parcat);
        string attach_cbfinlineEdit(CbfSumEntity parcat);
        List<Attachment> Getattachment(string id, string refe, string type);

        DataTable dtgetemployeeilist(int pargid);
        void deletetempstoredattach(string att);
        List<shipment> getbranchForLoginUser();
        IEnumerable<partransfer> getpartransfersummary();
        //IEnumerable<pardetailtransfer> getpardetailtransfersummary(string parheadid,string viewfor)
        IEnumerable<pardetailtransfer> getpardetailtransfersummary(string parheadid);
        IEnumerable<pardetailtransfer> getpardetailamountsummary(string pardetails_gid);
        IEnumerable<pardetailtransfer> getpardetailtransfersummary_report();
        //IEnumerable<pardetailtransfer> getpardetailswithamount(DataTable dt);
        IEnumerable<pardetailtransfer> getpardetailswithamount(DataTable dt, string viewfor);

        int GetQueueGidForMail(int refgid = 0, string ref_name = "");
        int GetRefGidForMail(string ref_name = "");
        string GetRequestStatus(int refgid = 0, string refname = "");

        string GetNextApproverDetails(int refgid = 0, string refname = "");
        List<Attachment> getattachmentdetails(string id, string process);
        List<CbfSumEntity> GetCBFAttachmentTemp(int rownum = 0,int cbfdetid = 0);
        void DeleteCBFAttachment(int refgid = 0);
        void InsertCBFAttachment(CbfSumEntity objCBFSumEntity); 
        List<CbfSumEntity> GetPROpexAttachment(int prdetid = 0);
        List<CbfSumEntity> GetPARAttachmentTemp(int rownum = 0, int cbfdetid = 0);
        void DeletePARAttachment(int refgid = 0);
        void InsertPARAttachment(CbfSumEntity objCBFSumEntity);  
    }
}