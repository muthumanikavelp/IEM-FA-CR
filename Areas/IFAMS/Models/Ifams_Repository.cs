using IEM.Areas.EOW.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IEM.Areas.IFAMS.Models
{
    public interface Ifams_Repository
    {
        //List<Ifams_Property> Getassetcategory();
        //List<Ifams_Property> Getsubcategory(int category);
        //List<Ifams_Property> GetAssetDetails(Ifams_Property objfa);
        //string InsertAssetDetails(Ifams_Property objin);
        //string UpdateAssetDetails(Ifams_Property objup);
        //string DeleteAssetDetails(string id);
        // List<capitalizationMaker> GetECFdetails();
        List<Ifams_Property> Getassetcategory();
        List<Ifams_Property> Getsubcategory(int category);
        List<Ifams_Property> GetAssetDetails(Ifams_Property objfa);
        string InsertAssetDetails(Ifams_Property objin,string hsn);
        string UpdateAssetDetails(Ifams_Property objup, string hsn);
        string DeleteAssetDetails(string id);
        List<capitalizationMaker> GetECFdetails();
      //  List<capitalizationMaker> GetECFdetailsGRN();
      //  List<capitalizationMaker> GetECFdetailsBranch();
        DataTable Getinvoice(int id);
        List<capitalizationMaker> Getpoqtydetails(int id);
       // List<capitalizationMaker> Getpodetails(capitalizationMaker CAP);
        List<capitalizationMaker> Getpodetails(int id);
        List<capitalizationMaker> Getcapitalized(int id, string status);
        List<capitalizationMaker> GetCapitalizationgrid(int id);
        string UpdateAmtDetails(capitalizationMaker cap);
       // List<capitalizationMaker> CreateAssetId();
        string SubmittoChecker(List<capitalizationMaker> inobj,string[] ids);
        List<capitalizationMaker> getapprovedetails();
        List<capitalizationMaker> getapprovedetailswait();
        capitalizationMaker getcapapprovaldetails(int id);
        capitalizationMaker UpdateIndexDetails(capitalizationMaker model, string invoice);

       List<capitalizationMaker> UpdateIndexDetailstax(capitalizationMaker tax);
        string deletetemp(string id);
        List<capitalizationMaker> GetAssetSubCategory();
        List<capitalizationMaker> GetAssetSubCategory(string Assetcategoryid);
        List<capitalizationMaker> GetBranchandlocation();
       List<capitalizationMaker> Getpodetails(string id);
        //string inserttemp(capitalizationMaker model);
       string FinalApprovalstage(int inv, string status,string Remarks);
       DataTable Getcapamount(int ecfid,int id);
       List<capitalizationMaker> Getinvoicetaxdet(int invgid,string view);
       DataTable Getexpsum(string invgid, string status);
       List<capitalizationMaker> Updatefulltax(int invgid, decimal amt, decimal dis, decimal tax, decimal others, decimal total);
       IEnumerable<EOW_PO> GetPoDetailssingledata(string id);
       IEnumerable<EOW_PO> GetPoDetailsitem(string ecfid, string invoiceid, string id);
     //   string Insertcapdetails(List<capitalizationMaker> capins);
       capitalizationMaker Getpodetailstoedit(int grnid, int invid);
       string GetCatNSubcatId(string subcat);
       string GetDepNGLcode(string values);
       string Verify_details(string[] values,string id);
       string GetInvoiceQty(string invoicegid = null);
       string GetGLDetails(string catgid);
       string Gethsndesc(string hnsgid);
       capitalizationMaker RefreshCapitalisationdetails(string invoicegid = null);
        // Add by sugumar for GST
       List<Ifams_Property> GetHsncode();


       List<SelectListItem> GetHsnforasset(string id);

       List<SelectListItem> GetHsncode1();
       List<SelectListItem> GetAllHsn();
       string GethsndescASSET(string hnsgid);

    }
}