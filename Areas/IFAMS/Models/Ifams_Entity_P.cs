using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using  System.Web.Mvc;
namespace IEM.Areas.IFAMS.Models
{
    public class Ifams_Modelp
    {
    }
    public class PhysicalVerificationAsset
    {
        public string SheetName { get; set; }
        ///public string FinancialYear { get; set; }
        public int SelectedId { get; set; }
       //    public SelectList FinancialYear;
        public string FinancialYear { get; set; }
        public int PV_gid { get; set; }
        public int Fid { get; set; }
        public List<PhysicalVerificationAsset> PVImportFileQuery { get; set; }
        public List<PhysicalVerificationAsset> PVSerach { get; set; }
        public List<PhysicalVerificationAsset> DropFillPVImportFileQuery { get; set; }
        public List<PhysicalVerificationAsset> PVReport { get; set; }
        public List<PhysicalVerificationAsset> PVStatusReport { get; set; }
        public string command { get; set; }

        public int AssetGID { get; set; }
        public string AssetID { get;set ;}
        public string AssetCode { get;set ;}
        public string AssetDesc { get;set ;}
        public string Status { get;set ;}
        public string FileName { get;set ;}
        public string Branch { get; set; }
        public string ReceiptDate { get; set; }
        public string Barcode { get; set; }
        public string PVstatus { get; set; }
        public string Asset_Category { get; set; }
        public string Asset_Sub_Category { get; set; }
        public string ReMarks { get; set; }
        public decimal wdv { get; set; }

        //Summary
        public int AsPerFA { get; set; }
        public int NoOfRecords { get; set; }
        public int NoOfFiles { get; set; }
        public int FAMatched { get; set; }
        public int FAMissMatched { get; set; }
        public int NotinFA { get; set; }
        public int CountFA { get; set; }
        public int NotVerfied { get; set; }
        public int TotalFileLoc { get; set; }

    }



    public class DepreciationHR
    {
        public string SheetName { get; set; }
        public string file { get; set; }
        public string branch { get; set; }
        public string status { get; set; }
   
    }
    
    
    
    
    
    public class filequery
    {
        public string year { get; set; }
        public string file { get; set; }
        public string branch { get; set; }
        public string status { get; set; }
        public List<filequery> query { get; set; }
        public string assetid { get; set; }
        public string assetCode { get; set; }
        public string assetDesc { get; set; }
        public string status1 { get; set; }
        public string filename { get; set; }
        public string branch1 { get; set; }
        public string receiptdate { get; set; }
        public string totalrecords { get; set; }
        public string nooffile { get; set; }
        public string okaycount { get; set; }
        public string locationmismatch { get; set; }
    }
   public class report
   {
       public string year { get; set; }
       public string assetid { get; set; }
       public string location { get;set;}
       public string barcode { get; set; }
       public List<report> rep { get; set; }
       public string assetcode { get; set; }
       public string yr { get; set; }
       public string assetid1{ get; set; }
       public string assetDesc { get; set; }
       public string remarks { get; set; }
       public string filename { get; set; }
       public string pvdate { get; set; }
       public string loccode { get; set; }
       public string barcodeno { get; set; }
       public string status1 { get; set; }
   }
    


    public class captializationdate
    {
        public int assetgid { get; set; }
        public string assetid { get; set; }
        public string location { get; set; }
        public DateTime date { get; set; }
        public List<captializationdate> capdat{ get; set; }
        public List<captializationdate> changedate { get; set; }
        public List<captializationdate> viewchangedate { get; set; }
        public List<captializationdate> Auditchangedate { get; set; }
        public string assetid1 { get; set; }
        public string assetcode { get; set; }
        public string assetDesc { get; set; }
        public string location1 { get; set; }
        public string assetgrpid { get; set; }
        public string assetvalue { get; set; }
        public string depamount { get; set; }
        public DateTime captialdate { get; set; }
        public string capnewdate { get; set; }
        public string capolddate { get; set; }
        public string capchangenewdate { get; set; }
        public string status { get; set; }
        public string command { get; set; }
        public decimal rectifamount { get; set; }
        public string employeeidname { get; set; }
        public string actiondate { get; set; }
        public string role { get; set; }
        public string astatus { get; set; }



    }


    public class captializationdatechecker
    {
        public string assetid { get; set; }
        public string location { get; set; }
        public DateTime date { get; set; }
        public List<captializationdatechecker> capdat { get; set; }
        public string assetid1 { get; set; }
        public string assetcode { get; set; }
        public string assetDesc { get; set; }
        public string location1 { get; set; }
        public string depamount { get; set; }
      
        public DateTime oldcaptialdate { get; set; }
        public DateTime newcaptialdate { get; set; }
    }
    public class assetvaluereduction
    {
        public string assetgroupid { get; set; }
        public string assetid { get; set; }
        public List<assetvaluereduction> valreduction { get; set; }
        public string assid { get; set; }
        public string grpid { get; set; }
        public string assetcode { get; set; }
        public string assetDesc { get; set; }
        public string assetvalue { get; set; }
        public string assetids { get; set; }
        public string reducedby { get; set; }
        public string currentval { get; set; }
        public string newval { get; set; }
        public DateTime reduceddate { get; set; }
        public string refno { get; set; }
    }
   
  
    
   
   
}