using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.FLEXIBUY.Models;
using System.Data;

namespace IEM.Areas.FLEXIBUY.Models
{
    public interface Irepositorypr
    {
        //--------------SCN WO--------------//

        List<poRaising> SCNWORelease(string request);
        List<poraiser> SCNWOReleaseHead(int woheadgid);
        List<poraiser> SCNWODetails(int woheadgid);
        string InsertWORelease(string ReleaseHeadGid);
        // Raising PO
        string Getchckdetails(string statusgid);
        List<poRaising> getpoSummary();
        List<poRaising> getStatus();
        string getStatusName(int statusgid);
        string GetGroupForUser();
        string GetGroupForUserPO();
        string GetGroupForUserWO();  
        string GetReqGroup();
        //cbfselection
        List<cbfselection> getcbfheader(string cbfmode);
        List<cbfselection> getrequestfor();
        string getrequestName(int requestforgid);
        DataTable getTable(cbfselection objcbf);
        List<cbfdetail> getcbflist(DataTable dt);

        //poraiser
        DataTable getcbfdetails(string cbfno);
        List<poraiser> getpolist(DataTable dt);
        List<poraiser> GetProjectOwnerList();
        string GetLoginUser();
        List<EmployeeSearch> GetEmployeeDetails();
        int GetemployId(string inchargeId);
        int GetBranchId(string branchCode);
        //shipment
        List<shipment> getshipmentType();
        List<shipment> getbranchdetails();
        List<shipment> GetShipment(DataTable dt);
        string GetShipmentStatus(string validate, string action);
        List<BulkUpload> GetUploadSummary(DataTable objTable);
        List<shipment> ExcelToShipment(DataTable dt);
        DataTable ShipmentFinalTable(DataTable dt);
        string InsertPo(poraiser objpoheader, DataTable objPoTable, DataTable objShipTable, DataTable objShipExcelTable, DataTable objExShip);

        //Template
        string InsertPoTemplate(PoTemplate objTemplate);
        List<poraiser> GetTemplateList();
        List<poraiser> GetTemplateListWO();
        string GetTemplateContent(int id);
        string GetTemplateName(int id);

        //BOQ Attachment
        List<ViewBOQ> GetAttachmentDetails(int cbfgid);

        //PoRaiserEdit
        DataTable GetPoDetails(int poheadGid);
        List<poraiser> GetPoDetailsList(DataTable dt);
        string GetVendorName(int vendorGid);
        DataTable GetShipTable(int poDetGid);
        List<shipment> GetShipmentDetails(DataTable dt);
        DataTable ShipmentFinalEditTable(DataTable dt);
        List<shipment> ExcelShipmentEdit(DataTable dt);
        string UpdatePo(poraiser objpoheader, DataTable objPoTable, DataTable objShipTable, DataTable objShipExcelTable, DataTable objExShip);
        string DeletePoSummaryDetails(poraiser obj);
        string CancelPoSummaryDetails(poraiser obj);
        List<poRaising> GetCancelPoSummaryDetails();
        int PoCancelApprove(poRaising obj);
        void DeleteShipdetails(int podetailgid);

        //PoChecker
        List<poRaising> GetPoCheckerSummary();
        string PoCheckerApprove(poRaising obj);

        //PoClosure
        List<poRaising> GetPoClosureSummary();
        string ClosurePoSummaryDetails(poraiser obj);
        string ClosurePoSummaryInsert(poraiser obj);
        List<poRaising> GetPoClosureCheckerSummary();

        //PoBulkClosure
        string GetPoClosureExcelValid(string validate, string pono, string action);
        string BulkPoInsert(DataTable dt, int validRecords, string filename);

        //Po Amendment
        List<poRaising> GetAmendSummary();
        
        //podelmat
        string GetDelmatemployee(poRaising objCancel);
        int GetPOQueueGid(string poheadGid);

        //Wo Summary
        List<WoSummary> GetWoSummary();

        //obfselection
        List<obfselection> getobfheader(string requestGroup);
        //objdetails
        List<obfdetail> getobflist(DataTable dt);
        DataTable getObfTable(obfselection objcbf);
        DataTable getobfdetails(string obfdetgid);

        //Wolist
        List<woraiser> GetWoList(DataTable dt);
        List<woraiser> GetFreqList();
        List<woraiser> GetMonth();
        List<woraiser> GetMonth1();
        string GetMonthName(int monthId);
        int GetMonthId(string monthName);

       // List<woraiser> GetWoSplitList(DataTable dt, int monthId, int monthId1);
        List<woraiser> GetWoSplitList(DataTable dt, int monthQ,int monthId,int monthId1,int freqId);
        List<woraiser> GetWoTempList(DataTable dt);
        int GetFreqMonth(int freqId);
        string InsertWo(woraiser objwoheader, DataTable objWoTable);
        DataTable GetWoDetails(int woheadGid);
        List<woraiser> GetWoEditList(DataTable dt);
        List<woraiser> GetWoEditTempList(DataTable dt);
        string UpdateWo(woraiser objwoheader, DataTable objWoTable);
        List<WoSummary> GetWoCheckerSummary();
        string WoCheckerApprove(WoSummary obj);
        int WoCancelApprove(WoSummary obj);
        //Wo Delete
        string DeleteWoSummaryDetails(woraiser obj);
        string CancelWoSummaryDetails(woraiser obj);

        List<WoSummary> GetCancelWoSummaryDetails();

        //Wo Closure
        List<WoSummary> GetWoClosureSummary();
        string ClosureWoSummaryDetails(woraiser obj);
        //Wo Closure Checker
        List<WoSummary> GetWoClosureCheckerSummary();
        string ClosureWoSummaryInsert(woraiser obj);

        //Wo Amendment
        List<WoSummary> GetWoAmendSummary();

        //Wo Bulk Closure
        string GetWoClosureExcelValid(string pono, string podate, string action);
        string BulkWoInsert(DataTable dt, int validRecords, string fileName);

        //Wo Delmat
        string GetDelmatemployeeForWo(WoSummary objapprove);
        int GetWOQueueGid(string woheadGid);
          //======Grn Release For po========//
        List<poRaising> getgrnReleaseForPO(string group);
        List<shipment> getbranchdetailsGrn(poraiser podetails);
        List<poraiser> GetPoDetailsListGRN(DataTable dt);
        DataTable DtTablegrn(shipment prdtgid);
        List<shipment> GetGRNDetailsForSave(DataTable dt);
        List<shipment> GetGRNDetailsForTempSave(DataTable dt);

        string InsertRelease(DataTable objTable);
        //grn inward Summary
        List<GRNInward> GetgrnSummary();
        List<GRNInward> GetscnSummary();
        List<GRNInward> getStatusForGRN();
        List<GRNInward> GetSelectionForPo();
        List<GRNInward> GetSelectionForWo();
        List<GRNInward> GetSelectionForPoCwip(string groupRes);
        List<GRNInward> GetPoDetailsForGRN(int poheadGid,string branchType);
        List<GRNInward> GetPoDetailsForGRNCwip(int poheadGid);
        List<GRNInward> GetPoDetailsTemp(DataTable dt);
        string InsertGrn(GRNInward objgrnheader, DataTable objTable, DataTable dtAttach);
      //  List<GRNInward> GetGRNDetails(int grnheadGid);
        string UpdateGrn(GRNInward objgrnheader, DataTable objTable,DataTable dtAttach);

        //========CWIP==============//

        List<GRNInward> GetApprovedPo();
        List<GRNInward> GetgrnSummaryForCwip();
        List<GRNInward> GetGRNDetailsForCwip(int grnheadGid);
        List<shipment> getbranchdetailsForCwip();
        List<GRNInward> GetGRNDetails(int grnheadGid);
        List<GRNInward> GetGrnDetailsTemp(DataTable dt);
        DataTable GetBranchDetailsForGRN(string detgid, shipment objdetails);
        List<shipment> GetBranchListForGRN(DataTable objtable);
        string InsertCwipRelease(DataTable objtable, GRNInward objshipment,DataTable dt);
        List<vendorselection> getvendorselection();
       // CbfSumEntity GetFccc();

      //====GRN New====//
       DataSet GetGRNSummaryNew();
       DataSet GetSCNSummaryNew();
       string GrnReleaseFull(int pogid = 0);
       string Getgrntype(string prodservicecode);

        //add Dhasarathan 01-12-2016
       int CreatePOAttachment(Attachments POAttachment);
       int DeletePOAttachment(string id);
    }
}