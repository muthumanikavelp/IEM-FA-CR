using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace IEM.Areas.IFAMS.Models
{
    
    public interface IfamsRepository_M
    {
        //Maker
        IEnumerable<SaleMakerModel> GetSoaDetailDraft(string id);
        IEnumerable<SaleMakerModel> GetSoaDetailReject(string id);
        IEnumerable<SaleMakerModel> GetSoaDetailSummary(string id);
        IEnumerable<SaleMakerModel> GetSoaDetailMaker(string id);
        IEnumerable<SaleMakerModel> GetSoaDetailMakerWFAP(string id);
        IEnumerable<SaleMakerModel> GetSoaDetailCheckerApproved(string id);
        

        string ImportAssetDetails(DataTable assetdata, string filename, string assettype);
        IEnumerable<SaleMakerModel> GetSoaDetail(string id);
        string GetSaleStatusexcel(string assetdata, string valid, string action);
        string InsertSale(DataTable assetdata, string filename);
        string SAGetstus(SaleMakerModel status, string id);
        SaleMakerModel GetSaleDetail(string Saleno);
        IEnumerable<SaleMakerModel> GetSADetail(string id);
        void AbortSale(Models.SaleMakerModel status, string id);
        string GetSalNo(Models.SaleMakerModel status, int id);
        string subSApaymentdetail(Models.SaleMakerModel status);
        string subSApaymentdetailup(Models.SaleMakerModel vatup);
        string getTheUser(string groupCode);
        //string saGetRef(string ref_name);
        //string saGetAttachType();
        //List<Attachment> saGetattachment(string id, string refe, string type);
        string saveSaattachment(SaleMakerModel model);
        string Deleteattachsub(int subattachid);
        SaleMakerModel GetchqSalDetail(string id);
        IEnumerable<VatModel> Getvat();
        string Vatcalculation(string id);
        string GetGrpCountSA(string id);
        List<SaleMakerModel> SAAutosaleno(string saleno, string status, string loginid);
        List<SaleMakerModel> SAAutosalenochk(string saleno, string loginid);
        string RevokeSaleDetail(string soahgid);

        //SOAChecker
        IEnumerable<SaleMakerModel> GetSADetailChecker(string id);
        string SAChkApprovalStatus(string id, string chstatus, string Remarks);
        IEnumerable<SaleMakerModel> GetSaleDetailSummary(string id);
        DataTable AssetSaveDetails(Models.SaleMakerModel status);


        List<Salemakermodelgst> gstsalemakerdetails(string pincode, string salegid,string hsndesc);
        string[,] GetVendor(string vendorcode, int saleid);
        string assetdetailupdate(Models.SaleMakerModel status);
        string assetdetailssave(Models.SaleMakerModel status);
        string UpdateSOAHsndetail(Models.SaleMakerModel staus);

        SaleMakerModel GetSaleDetailFORREJECT(string Saleno);
        List<SearchCustomer> GetCustomerList();
        DataTable GetGstCustomerDetails(SaleMakerModel salemodel);

        IEnumerable<SearchCustomer> SelectCustomerSearch(string customercode,string Customername);
        string GetUserType(string saleid);
    }
}