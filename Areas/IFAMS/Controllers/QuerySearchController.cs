using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Mvc;
using IEM.Areas.IFAMS.Models;
using IEM.App_Start;
namespace IEM.Areas.IFAMS.Controllers
{

    [NoDirectAccess]
    public class QuerySearchController : Controller
    {
        //
        // GET: /IFAMS/QuerySearch/

        private IfamsQueryRepository Query;
        public QuerySearchController() :
            this(new IfamsQueryDataModel()) { }

        public QuerySearchController(IfamsQueryRepository Objist)
        {
            Query = Objist;
        }
        public ActionResult Index()
        {

            List<IfamsQueryEntity> getrecord = new List<IfamsQueryEntity>();
            getrecord = Query.GetQueryDetails().ToList();

            IfamsQueryEntity Assetcatdoctype = new IfamsQueryEntity();
            Assetcatdoctype.Assetcategory = new SelectList(Query.Assetcategory().ToList(), "assetcatname", "assetcatname");
            ViewBag.AssetcatDoc = Assetcatdoctype;

            IfamsQueryEntity Assetsubdoctype = new IfamsQueryEntity();
            Assetsubdoctype.Assetsubcategory = new SelectList(Query.Assetsubcategory().ToList(), "assetsubcatname", "assetsubcatname");
            ViewBag.AssetSubcat1 = Assetsubdoctype;

            IfamsQueryEntity Assetstatusvalue = new IfamsQueryEntity();
            Assetstatusvalue.AssetStatusName = new SelectList(Query.AssetStatusName().ToList(), "assetstatusname", "assetstatusname");
            ViewBag.Assetstatusvalue = Assetstatusvalue;

            IfamsQueryEntity AssetLocation = new IfamsQueryEntity();
            AssetLocation.GetLocation = new SelectList(Query.GetLocation().ToList(), "BranchNmae", "BranchNmae");
            ViewBag.LocationDetails = AssetLocation;


            ViewBag.Assetstatus = "";
            if (Session["searchrecords"] != null)
            {
                getrecord = (List<IfamsQueryEntity>)Session["searchrecords"];
                IfamsQueryEntity getvalue = new IfamsQueryEntity();
                IfamsQueryEntity Getviewbagvalue = new IfamsQueryEntity();
                getvalue = (IfamsQueryEntity)Session["GetSearchValue"];
                Getviewbagvalue.barcodedetials_bar_no = getvalue.barcodedetials_bar_no;
                Getviewbagvalue.assetdetails_assetdet_id = getvalue.assetdetails_assetdet_id;
                Getviewbagvalue.branch_legacy_code = getvalue.branch_legacy_code;
                //Getviewbagvalue.assetcategory_name = getvalue.assetcategory_name;
                //Getviewbagvalue.assetsubcategory_name = getvalue.assetsubcategory_name;
                @ViewBag.Assetcategory = getvalue.assetcategory_name;
                @ViewBag.AssetSubcat = getvalue.assetsubcategory_name;
                @ViewBag.Assetstatus = getvalue.excelvalidate_name;
                @ViewBag.GetLocation = getvalue.branch_legacy_code;
                Getviewbagvalue.assetdetails_cap_date = getvalue.assetdetails_cap_date;
                Getviewbagvalue.ecf_no = getvalue.ecf_no;
                Getviewbagvalue.invoice_no = getvalue.invoice_no;
                Getviewbagvalue.poheader_pono = getvalue.poheader_pono;
                Getviewbagvalue.cbfheader_cbfno = getvalue.cbfheader_cbfno;

            }
            else
            {
                Session["searchrecords"] = null;
                Session["GetSearchValue"] = null;
            }
            Session["QuerySearchExceldownload"] = getrecord;
            return View(getrecord);
        }
        //public JsonResult QuerySearchreport()
        //{
        //    List<IfamsQueryEntity> getrecord = new List<IfamsQueryEntity>();
        //    try
        //    {
        //        getrecord = Query.GetQueryDetails().ToList();
        //        return Json(getrecord, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(getrecord, JsonRequestBehavior.AllowGet);
        //    }
        //}
        [HttpPost]
        public ActionResult Index(string Barcode, string AssetID, string Location, string AssetCat, string AssetSubcat, string CaptilizationDate, string EcfNo, string InvoiceNo, string AssetStatus, string AssetValue, string PONo, string CBFNO, string command = null)
        {

            List<IfamsQueryEntity> getrecord = new List<IfamsQueryEntity>();
            List<SelectListItem> selctedvalue = new List<SelectListItem>();
            IfamsQueryEntity viewbags = new IfamsQueryEntity();

            IfamsQueryEntity Assetcatdoctype = new IfamsQueryEntity();
            Assetcatdoctype.Assetcategory = new SelectList(Query.Assetcategory().ToList(), "assetcatname", "assetcatname");
            ViewBag.AssetcatDoc = Assetcatdoctype;

            IfamsQueryEntity Assetsubdoctype = new IfamsQueryEntity();
            Assetsubdoctype.Assetsubcategory = new SelectList(Query.Assetsubcategory().ToList(), "assetsubcatname", "assetsubcatname");
            ViewBag.AssetSubcat1 = Assetsubdoctype;

            IfamsQueryEntity Assetstatusvalue = new IfamsQueryEntity();
            Assetstatusvalue.AssetStatusName = new SelectList(Query.AssetStatusName().ToList(), "assetstatusname", "assetstatusname");
            ViewBag.Assetstatusvalue = Assetstatusvalue;

            IfamsQueryEntity AssetLocation = new IfamsQueryEntity();
            AssetLocation.GetLocation = new SelectList(Query.GetLocation().ToList(), "BranchNmae", "BranchNmae");
            ViewBag.LocationDetails = AssetLocation;

            Session["records"] = "";
            if (command == "Search" || command == "Refresh")
            {
                getrecord = Query.GetQueryDetails(Barcode, AssetID, Location, AssetCat, AssetSubcat, CaptilizationDate, EcfNo, InvoiceNo, AssetStatus, AssetValue, PONo, CBFNO).ToList();
                Session["searchrecords"] = getrecord;
                //if(AssetStatus!="")
                //{              

                //foreach(var item in AssetStatus)
                //{
                //    Boolean selected = false;
                //    if (AssetStatus == "Transferred")
                //    {
                //        selected = true;
                //        ViewBag.Assetstatus = AssetStatus;
                //    }
                //    else if (AssetStatus == "Sold")
                //    {
                //        selected = true;
                //        ViewBag.Assetstatus = AssetStatus;
                //    }
                //    else if (AssetStatus == "Impaired")
                //    {
                //        selected = true;
                //        ViewBag.Assetstatus = AssetStatus;
                //    }
                //    selctedvalue.Add(new SelectListItem
                //        {
                //            Selected = selected
                //        });
                //}

                //}               
                //else
                //{
                //    ViewBag.Assetstatus = "";
                //}
                if (AssetCat != "" && AssetCat != "-----Select----")
                {
                    @ViewBag.Assetcategory = AssetCat;
                }
                if (AssetSubcat != "" && AssetSubcat != "-----Select----")
                {
                    @ViewBag.AssetSubcat = AssetSubcat;
                }
                if (AssetStatus != "" && AssetStatus != "-----Select----")
                {
                    @ViewBag.AssetSubcat = AssetSubcat;
                }
                @ViewBag.txtBarcode = Barcode;
                @ViewBag.txtAssetID = AssetID;
                @ViewBag.GetLocation = Location;
                @ViewBag.Assetcategory = AssetCat;
                @ViewBag.Assetsubcategory = AssetSubcat;
                @ViewBag.AssetStatusName = AssetStatus;
                @ViewBag.CaptilizationDate = CaptilizationDate;
                @ViewBag.EcfNo = EcfNo;
                @ViewBag.InvoiceNo = InvoiceNo;
                @ViewBag.PoNo = PONo;
                @ViewBag.CBFNo = CBFNO;

                viewbags.barcodedetials_bar_no = Barcode;
                viewbags.assetdetails_assetdet_id = AssetID;
                viewbags.branch_legacy_code = Location;
                viewbags.assetcategory_name = AssetCat;
                viewbags.assetsubcategory_name = AssetSubcat;
                viewbags.assetdetails_cap_date = CaptilizationDate;
                viewbags.ecf_no = EcfNo;
                viewbags.invoice_no = InvoiceNo;
                viewbags.poheader_pono = PONo;
                viewbags.cbfheader_cbfno = CBFNO;
                viewbags.excelvalidate_name = AssetStatus;
                Session["GetSearchValue"] = viewbags;
                if (getrecord.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }
            if (command == "Clear")
            {
                Session["searchrecords"] = null;
                Session["GetSearchValue"] = null;
                ViewBag.Assetstatus = "";
                getrecord = Query.GetQueryDetails().ToList();
            }
            Session["QuerySearchExceldownload"] = getrecord;
            return View(getrecord);


        }
        //[HttpGet]
        //public JsonResult GetlocAsset(string ids)
        //{
        //    IfamsQueryEntity AssetLocation = new IfamsQueryEntity();
        //    AssetLocation.GetLocation = new SelectList(Query.GetLocation().ToList(), "BranchNmae", "BranchNmae");
        //    ViewBag.LocationDetails = AssetLocation;
        //    //List<AssetParentModel> AssetList = new List<AssetParentModel>();
        //    return Json(AssetLocation, JsonRequestBehavior.AllowGet);
        //}
        public ActionResult QuerySearchExceldownload()
        {

            List<IfamsQueryEntity> cList;
            cList = (List<IfamsQueryEntity>)Session["QuerySearchExceldownload"];

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("Barcode Number");
            dt.Columns.Add("Asset ID");
            dt.Columns.Add("Branch");
            dt.Columns.Add("Asset Category");
            dt.Columns.Add("Asset SubCategory");
            
            dt.Columns.Add("Captilization Date");
            dt.Columns.Add("ECF No");
            dt.Columns.Add("Invoice No");
            dt.Columns.Add("Asset Value");
            dt.Columns.Add("PO No");
            dt.Columns.Add("CBF No");
            dt.Columns.Add("Asset Status");
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                i + 1
                , cList[i].barcodedetials_bar_no.ToString()
                , cList[i].assetdetails_assetdet_id.ToString()
                , cList[i].branch_legacy_code.ToString()
                , cList[i].assetcategory_name.ToString()
                , cList[i].assetsubcategory_name.ToString()
                , cList[i].assetdetails_cap_date.ToString()
                , cList[i].ecf_no.ToString()
                , cList[i].invoice_no.ToString()
                , cList[i].assetdetails_assetdet_value.ToString()
                , cList[i].poheader_pono.ToString(),
                cList[i].cbfheader_cbfno.ToString()
                , cList[i].excelvalidate_name.ToString());
            }

            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            // Session["exceldownload"] = gv;
            if (gv.Rows.Count != 0)
            {
                //return new DownloadFileActionResult((GridView)Session["exceldownload"], "DocumentGroup.xls");
                return new DownloadFileActionResult(gv, "Asset Query Summary.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }
    }


}

