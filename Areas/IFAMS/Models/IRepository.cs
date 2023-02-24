using IEM.Areas.IFAMS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Web;


namespace IEM.Areas.IFAMS.Models
{
    public interface IRepository
    {
        string getTheUser(string groupCode);

        //ToaMaker
        IEnumerable<TransferMakerModel> GetGlNumber(string glnumber);
        string updateFCCC(TransferMakerModel fc);
        string GetFcccSerachText(string lsFccc);
        TransferMakerModel GetFccc();
        TransferMakerModel GetFcccSerach(FcccMaster fc);
        string Getststus(TransferMakerModel status, string id);

       // void updateTrfStatus(TransferMakerModel status, string id);
        DataTable updateTrfStatus(TransferMakerModel status);

        IEnumerable<TransferMakerModel> GetTrfDetailFull(string id);
        IEnumerable<TransferMakerModel> GetTrfDetailDraft(string id);
        IEnumerable<TransferMakerModel> GetTrfDetailRejected(string id);        
        IEnumerable<TransferMakerModel> GetTrfDetailWFAP(string id);
        IEnumerable<TransferMakerModel> GetTransferDetail(string Transferno);
        IEnumerable<TransferMakerModel> GetTransferDetailnew(string Transferno);
        IEnumerable<TransferMakerModel> GetTrfDtlInprocessSummary(string id);
        TransferMakerModel GetRaiserDetail(string raiserId, string toa_number);
        void AbortTransfer(Models.TransferMakerModel status, string id);
        string GetTrfNo(string id);
        string GetStatusexcel(string assetdata, string valid, string action);
        //string InsertTransfer(DataTable assetdata, string filename);
        string ImportAssetDetails(DataTable assetdata, string filename, string assettype); // For Asset Transfer File Upload
        string getfilename(string toanumber);
        string save_attachment(TransferMakerModel model);
        string Delete_attach(int attachId);
        string GetRef(string ref_name);
        string GetAttachType();
        string GetGroupGid(string assetid);
        string GetGrpCount(string grpId);
        List<Attachment> Getattachment(string id, string refe, string type);
        List<TransferMakerModel> verifylogeddetails(string headergid);
        List<IFAMS.Models.Ifams_Propertyx.ImpairmentsModel> GetImpairStatus();
        //ToaChecker
        IEnumerable<TransferMakerModel> GetTrfDetailforChecker(string id);
        string ApprovalStatus(string id, string status, string remarks);
        decimal getRetificationAmount(string assetId);
        //WoaMaker
        List<WriteOffModel> GetWoaDetail(int id);
        List<WriteOffModel> GetWoaDetailDraft(int id);
        List<WriteOffModel> GetWoaDetailReject(int id);       

        List<WriteOffModel> GetWRFDtlInprocessSummary(int id);
        string Updatelogedstatus(string headergid);
        string changlogedstatus(string tohederid);
        string save_attachment(WriteOffModel model);
        IEnumerable<WriteOffModel> GetWriteOffDetail(string Transferno);
        DataTable updateWrtStatus(WriteOffModel status);
        void AbortWriteoff(string id);
       // void updateWrtStatus(WriteOffModel status, string id);
        IEnumerable<WriteOffModel> GetWrtDetailforChecker(string id);
        IEnumerable<WriteOffModel> GetWrtDetailforSupervisior(string id);
        List<WriteOffModel> verifyWoalogeddetails(string headergid);
        string InsertWriteOff(DataTable assetdata, string filename);
        string GetWrtNo(string id);
        string Getwrtststus(WriteOffModel status, string id);
        string AppRejStatus(string id, string status, string remarks);

        //barcode
        List<BarcodeGenerationModel> GetDetailForBarcode(string loccode, string assetcode, string Dp_type, string capdate, string captodate);
        string GenerateBarcode(List<string> ids);
        string GetBarcodeSequence(string assetCode, string assetcode_gid);
        List<BarcodeGenerationModel> GetDetailForBarcodePrintingSum();
        List<BarcodeGenerationModel> GetDetailForBarcodePrintingSearch();
        string Barcode(List<string> barcodeid);
        string Savedespatch(BarcodeGenerationModel model, string ids);
        string GetddlSource();
        List<BarcodeGenerationModel> GetddlStatus();
        DataTable GetDetailsForBarcodePrint(string id);
        
        //grouping
        List<GroupModel> GetDetailToGroup();
        string GenerateGoupId(string ids, string grpid);
        List<GroupModel> GetDetailForMkrSummary(string id);
        List<GroupModel> GetDetailForChkSummary();
        List<GroupModel> GetDetailForView(string groupid);
        string GetGroupID(string id);
        string ApproveRejectGrp(string no, string status, string remaks);
        List<GroupModel> GetDetailToGroupsearch(string location, string capdate);
        DataSet GetAssetDetails(string LocationCode = null, string capdate = null);

        //change dep rate
        IEnumerable<ChangeDepreciationRate> GetAssetDetailHelp();
        string UpdateDepRateBulk(DataTable datatable, string filename);
        string UpdateDepRateManul(ChangeDepreciationRate data);


        //Deperication
        // string CalDep(string asset_id);
        DataTable GetSelectreportDetails();
        DataTable SelectreportDetails(string date);
        Boolean StartProcess(string type);
        void EndProcess(string type);
        Boolean DataSynchronise();
        Boolean DepreciationDone(string date,string runtype);
        string Format(string date);
        List<Depreciation> SelectDepDetails();
        string SelectDepDetailsnInsert(DateTime tilldate,string deptye);
        string UpdateNot5KCase();
        void UpdateReversals();
        void UpdateLeaseDates(string assetid, string lease_start, string lease_end);
        Decimal GetTotalDep(DateTime dtCapitalisationDate, DateTime dtTillDate,
                                decimal dAssetValue, string sAssetCode,
                                 string cNot5000Case, string sBranch1, string sBranch2,
                                  DateTime dtBranchLaunch, DateTime dtLeaseStart, DateTime dtLeaseEnd,
                                 string sDepType, decimal dDepRate,
                                 string sAssetGroup = null,
                                 string sPONumber = null,
                                 string sFICCRef = null,
                                 string sAsset_GroupId = null,
                                 decimal dTransfer_value = 0,
                                 string CanDepreciateFully = null,
                                 int dDepDevRate = 0);
        void InsertDep(string _asset_id, string _group_id, decimal CurrentDepAmt, string depdate, string deptype);
        void InsertFromDepToPreDep();
        Boolean CanDepreciateToZero(string sAssetGroup, string sPONumber, string sFICCRefNumber);
        List<Depreciation> SelectreportDetails(string typeOfReport,string date);
        List<Depreciation> SelectPreRunDetails();
        void InsertPreDep(string _asset_id, string _group_id, decimal CurrentDepAmt, string depdate , string deptype);             
        void InsertFromDepToForeDep();
        List<AssetReportModel> SelectFARreportDetails(string Selecteddate,string tilldate);
        void InsertDepMonths(string selectedDate, string prevDate,string refe,string tillDate);
        Depreciation selcetdates(string ref_flag);
        string toGetFincialyear();
        string toGetFincialyear(DateTime date);
        List<Depreciation> GetdetailsforDepHold();
      //  DataSet GetdetailsforDepHold();
       // DataSet GetdetailsforDepRelease();
        List<Depreciation> GetdetailsforDepRelease();
        void Insert_deponhold(string asset_gid);
        void Update_deponhold(string asset_gid);
        DateTime getDepRateChangeDetail(string asset_gid);
        void InsertForecasteDep(string _asset_id, string _group_id, decimal CurrentDepAmt, string depdate, string deptype);

        //Sale Invoice
        List<SaleInvoice> getApprovedSaleDetails();
        List<SaleInvoice> getSaleDetails(string id);
        List<SaleInvoice> getApprovedSale(string id);
        List<SaleInvoice> getFICCdetails(string id);
        List<SaleInvoice> GetTaxdetails(string id);
        List<SaleInvoice> GetCessTaxdetails(string id);
        void GenerateInvoicePDF(string id);

       // Transfer Invoice
        List<TransferInvoice> getApprovedtoaDetails();
        List<TransferInvoice> gettoaDetails(string id);
        List<TransferInvoice> getApprovedtoa(string id);
        List<TransferInvoice> getFICCdetailstoa(string id);
        List<TransferInvoice> getTaxdetailstoa(string id);
        //List<TransferInvoice> GetCessTaxdetailstoa(string id);
        //void GenerateInvoicePDFtoa(string id);
      



        //CWIP
        IEnumerable<CWIPModel> GetCWIPMakerList();
        IEnumerable<CWIPModel> GetCWIPCheckerList();
        IEnumerable<CWIPModel> GetViewDetailsList();
        CWIPModel GetCWIPViewDetails(int cwip_gid);
        string SetCWIPDetails(CWIPModel obj);
        string SubmitDraftCWIP(DataTable assetdata);
        string CancelDraftCWIP();
        string SubmitCapCWIP(string ids);
        string UnseletedCWIP(string ids);
        string Verify_details(string[] ids);
        string AppRejCapCWIP(string ids,string status);
        IEnumerable<CWIPModel> GetReportDetails(CWIPModel model);
        IEnumerable<CWIPModel> GetCapReportDetails(CWIPModel model);
        DataTable GetReportDetailsreport(CWIPModel model);
        void Savetransfergstdetails(string toanumber);
        string GetUserTypeTransfer(string id);
        IEnumerable<TransferMakerModel> GetTrfDetailSummary(string id);
        List<TransferMakerModel> verifylogedindetails(string headergid);
        int UpdatelogedinStatus(string headergid);


        string GetUserType(string id);
    }
}