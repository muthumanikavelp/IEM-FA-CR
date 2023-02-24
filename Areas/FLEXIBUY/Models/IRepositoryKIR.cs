using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace IEM.Areas.FLEXIBUY.Models
{
    public interface IRepositoryKIR
    {
        IEnumerable<flexibuydashboard> getMyRequestqueryExcel(string date, string DocNo, string Amount, string Category, string Status, string Raiser);
        IEnumerable<flexibuydashboard> getMyRequestquery();
        PrSumEntity GetPrSumry();
        PrSumEntity GetPIPSumry();
        PrSumEntity GetProdServDetails(int id);
        PrSumEntity GetPrsupervisorSumry();
        List<PrSumEntity> GetBranchList();
        List<PrSumEntity> GetStatusList();
        List<PrSumEntity> GetRequestForList();
        List<PrSumEntity> GetFcccList();
        List<PrSumEntity> GetUomList();

        List<PrSumEntity> GetProductGroupList(string radio1);
        List<Product> GetSelectedProduct(Product code);
        List<Product> GetSelectedService(Product code);
        List<Product> GetService(DataTable code);
        List<Product> GetProduct(DataTable code);

        string GeneartePrRefNo(int id, string prefix);
        PrSumEntity InsertPrheader(PrHeader prh, string button);
        PrSumEntity InsertPrDetails(List<Product> productLst, int prheadergid);

        PrSumEntity EditPrheader(PrHeader prh);
        PrSumEntity EditPrheader1(PrHeader prh);
        PrSumEntity pipheader(PrHeader prh);
        PrSumEntity supervisorheader(PrHeader prh);
        PrSumEntity EditPrDetails(List<Product> productLst, int prheadergid);

        PrSumEntity ViewPrDetails(PrHeader pr, string action);
        PrSumEntity ClonePrDetails(string prrefno);

        PrSumEntity supervisorViewPrDetails(PrHeader pr1, string action);

        List<PrDetails> ViewPipPrDetails(string prrefno);



        PrSumEntity PIPAttachmentname(PrSumEntity filename, int row);


        string GetStatusName(int statusgid);
        string GetRequestforName(int Requestforgid);
        string GetBranchName(int Branchgid);
        PrSumEntity CheckExistancyPrRefNo(string prrefno);

        PrSumEntity PipUpdateEstimate(string prgid,string prrefno, List<PrDetails> prd);
        PrSumEntity InsertAttachment(string prrefno, string process, List<Attachment> attach);

        PrSumEntity PRAttachmentname(PrSumEntity filename);

        List<Attachment> getattachmentdetails(string id, string process);
        List<Attachment> EditPRAttachmentList(DataTable dt);

        PrSumEntity DeleteAttachment(DataTable dt);
        PrSumEntity DeleteProductServiceDetails(DataTable dt);

        PrSumEntity viewqotation(string id);
        string rejectprsup(PrHeader objprheader);
        string approvesupp(PrHeader objprheader);

        //List<dashboard> getdashboard();
        //List<dashboard> getMyRequest();
        //List<dashboard> getMyRequest(string ddlCategory,string txtDocNo,string txtDate,string txtAmount);
        
        IEnumerable<flexibuydashboard> getdashboard();
        IEnumerable<flexibuydashboard> getMyRequest();
        IEnumerable<flexibuydashboard> getMyRequest(string date, string DocNo, string Amount, string Category, string Status);
        IEnumerable<flexibuydashboard> getMyRequest1(string date, string DocNo, string Amount, string Category, string Status,string Raiser);
        IEnumerable<flexibuydashboard> getMyApproval();
        IEnumerable<flexibuydashboard> getMyApproval(string date, string DocNo, string Amount, string Category, string Status);
        IEnumerable<flexibuydashboard> getDashDetailsPR(string type, string process);
        IEnumerable<flexibuydashboard> getDashDetailsPAR(string type, string process);
        IEnumerable<flexibuydashboard> getDashDetailsCBF(string type, string process);
        IEnumerable<flexibuydashboard> getDashDetailsPO(string type, string process);
        IEnumerable<flexibuydashboard> getDashDetailsWO(string type, string process);
        IEnumerable<flexibuydashboard> doctypedata();
        IEnumerable<flexibuydashboard> GetStatusType();
        IEnumerable<flexibuydashboard> doctypedataapp();
        IEnumerable<flexibuydashboard> GetStatusTypeapp();

        List<flexibuydashboard> getpodashboard();

        List<flexibuydashboard> getwodashboard();

        List<flexibuydashboard> getcbfdashboard();

        List<grnconfirmation> getgrnconfirmation();
        List<grnconfirmation> getscnconfirmation();
        List<grnconfirmation> chechgrnprefno(string grnprnrefno);
        List<grnconfirmationdetails> getscnconfirmationdetails(grnconfirmation gr, int grnheadgid);
        List<grnconfirmationdetails> getgrnconfirmationdetails(grnconfirmation gr, int grnheadgid);
        string insertgrncfmdetails(grnconfirmationdetails objdetails, DataTable dt);
        List<grnconfirmationdetails> rebindconfirmdetails(DataTable dt);
        string grnapprove(grnconfirmation grheadid);
        string scnapprove(grnconfirmation grheadid);
        string Getemployeedetails(string id);
        string Insertqueue( int prheadergid,string queueTo,PrHeader objprheader);
        string Getpipemp();
        int getempid(string refno);
        List<prsupervisior> GetdelmateApprovalqueue();
        string GetDelmatemployee(PrHeader pr);
        int Getbranchgid();
        int Getfcccgid();

        int IsExistingApprover(int refgid, string refflag);
        int RejectPIPInputsOnPR(PrHeader prheader);
        int InsertPRAttachmentEditNew(PrSumEntity objPRSumEntity, int refgid = 0, string attachmenttype = "");
        int DeletePREditAttachment(int id);
        PrSumEntity ViewInlineAttachmentsPR(int id = 0, string process = "");
        List<CBFPRPARDetails> GetCBFReportTreeView(string CBFDateFrom, string CBFDateTo);
        DataSet GetCBFUtilReports(string CBFDateFrom, string CBFDateTo, string CBFNO, string PONO);
        DataSet GetCBFUtilReports(string CBFDateFrom, string CBFDateTo);
        DataSet Getapprovaldetails(string doctype);
    }
}