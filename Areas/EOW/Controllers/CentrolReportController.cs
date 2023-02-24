using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using IEM.Areas.EOW.Models;
using System.Reflection;
using System.Web.Helpers;
using System.IO;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using System.Net;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using System.Collections;
using System.Configuration;
using IEM.Common;
using IEM.App_Start;
namespace IEM.Areas.EOW.Controllers
{
    [NoDirectAccess]
    public class CentrolReportController : Controller
    {
        ErrorLog objErrorLog = new ErrorLog();
        DataTable dt = new DataTable();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        CommonIUD objCommonIUD = new CommonIUD();
        EOW_DataModel EOWDataModel = new EOW_DataModel();
        CentraldataModel sub = new CentraldataModel();
        EOW_Currency data = new EOW_Currency();
        private CentralReportRepository objModel;
        String result = String.Empty;
        List<string> list = new List<string>();
        public CentrolReportController()
            : this(new CentralReportModel())
        {

        }
        public CentrolReportController(CentralReportRepository objM)
        {
            objModel = objM;
        }
        [HttpGet]
        public ActionResult MISReport()
        {
            List<CentraldataModel> obj = new List<CentraldataModel>();
            return View(obj);
        }
        [HttpPost]
        public ActionResult MISReport(string EcfDate = null, string ecfnumber = null, string InvoiceDate = null, string InvoiceAmount = null, string Status = null)
        {
            List<CentraldataModel> obj = new List<CentraldataModel>();
            ViewBag.InvoiceDate = InvoiceDate;
            ViewBag.InvoiceAmount = InvoiceAmount;
            ViewBag.ecfnumber = ecfnumber;
            ViewBag.EcfDate = EcfDate;
            ViewBag.Status = Status;
            obj = objModel.MISReportLoad(InvoiceDate, InvoiceAmount, ecfnumber, EcfDate, Status).ToList();
            return View(obj);
        }
        [HttpGet]
        public ActionResult Collection_MISReport()
        {
            List<CentraldataModel> obj = new List<CentraldataModel>();
            return View(obj);
        }
        [HttpPost]
        public ActionResult Collection_MISReport(string EcfDate = null, string ecfnumber = null, string InvoiceDate = null, string InvoiceAmount = null, string Status = null)
        {
            List<CentraldataModel> obj = new List<CentraldataModel>();
            ViewBag.InvoiceDate = InvoiceDate;
            ViewBag.InvoiceAmount = InvoiceAmount;
            ViewBag.ecfnumber = ecfnumber;
            ViewBag.EcfDate = EcfDate;
            ViewBag.Status = Status;
            obj = objModel.Collection_MISReport(InvoiceDate, InvoiceAmount, ecfnumber, EcfDate, Status).ToList();
            return View(obj);
        }
        [HttpGet]
        public ActionResult MIS_DepartmentwiseAndVendorwise_Report()
        {
            List<CentraldataModel> obj = new List<CentraldataModel>();
            return View(obj);
        }
        [HttpPost]
        public ActionResult MIS_DepartmentwiseAndVendorwise_Report(string EcfDate = null, string ecfnumber = null, string InvoiceDate = null, string InvoiceAmount = null, string Status = null)
        {
            List<CentraldataModel> obj = new List<CentraldataModel>();
            ViewBag.InvoiceDate = InvoiceDate;
            ViewBag.InvoiceAmount = InvoiceAmount;
            ViewBag.ecfnumber = ecfnumber;
            ViewBag.EcfDate = EcfDate;
            ViewBag.Status = Status;
            obj = objModel.MIS_DepartmentwiseAndVendorwise_Report(InvoiceDate, InvoiceAmount, ecfnumber, EcfDate, Status).ToList();
            return View(obj);
        }
        [HttpGet]
        public ActionResult InvoiceMIS_VendorwiseAndPOWOwise_Report()
        {
            List<CentraldataModel> obj = new List<CentraldataModel>();
            return View(obj);
        }
        [HttpPost]
        public ActionResult InvoiceMIS_VendorwiseAndPOWOwise_Report(string EcfDate = null, string ecfnumber = null, string InvoiceDate = null, string InvoiceAmount = null, string Status = null)
        {
            List<CentraldataModel> obj = new List<CentraldataModel>();
            ViewBag.InvoiceDate = InvoiceDate;
            ViewBag.InvoiceAmount = InvoiceAmount;
            ViewBag.ecfnumber = ecfnumber;
            ViewBag.EcfDate = EcfDate;
            ViewBag.Status = Status;
            obj = objModel.InvoiceMIS_VendorwiseAndPOWOwise_Report(InvoiceDate, InvoiceAmount, ecfnumber, EcfDate, Status).ToList();
            return View(obj);
        }
        [HttpGet]
        public ActionResult TransferPettyCash()
        {
            List<SelectCourier> obj = new List<SelectCourier>();
            return View(obj);
        }
        [HttpPost]
        public ActionResult TransferPettyCash(string ARFFrom = null, string ARFTo = null, string ARFAmount = null, string ARFNo = null, string RaiserCode = null, string RaiserName = null,string BranchCode=null)
        {
            List<SelectCourier> obj = new List<SelectCourier>();
            ViewBag.ARFFrom = ARFFrom;
            ViewBag.ARFTo = ARFTo;
            ViewBag.ARFAmount = ARFAmount;
            ViewBag.ARFNo = ARFNo;
            ViewBag.RaiserCode = RaiserCode;
            ViewBag.RaiserName = RaiserName;
            ViewBag.BranchCode = BranchCode;
            obj = objModel.ChangePettyReport(ARFFrom, ARFTo, ARFAmount, ARFNo, RaiserCode, RaiserName,BranchCode).ToList();
            DataTable dt = objModel.ExcelExportPettyCashChange(ARFFrom, ARFTo, ARFAmount, ARFNo, RaiserCode, RaiserName, BranchCode);
            Session["ExcelExport"] = dt;
            return View(obj);
        }
        [HttpGet]
        public PartialViewResult TransferPettyCashPopup(string ecf_gid=null)
        {
            List<SelectCourier> obj = new List<SelectCourier>();
            obj = objModel.ChangePettyReportPopup(ecf_gid).ToList();
            ViewBag.EcfGid = obj[0].ECF_Id;
            ViewBag.ARFFrom = obj[0].ARF_Date_From;
            ViewBag.ARFTo = obj[0].ARF_Date_To;
            ViewBag.ARFAmount = obj[0].ARF_Amount;
            ViewBag.ARFNo = obj[0].ARF_No;
            ViewBag.RaiserCode = obj[0].RaiserCode;
            ViewBag.RaiserName = obj[0].RaiserName;
            ViewBag.TransferFrom = obj[0].RaiserCode;
            return PartialView();
        }
        public JsonResult AddTransferPettyCash(SelectCourier Transfer)
        {
            try
            {
                result = objModel.AddPettyTransfer(Transfer);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult AutocompleteEmployeeTo(string RaiserCode=null, string RaiserName=null)
        {
            List<SelectCourier> obj = new List<SelectCourier>();
            CentralReportModel v = new CentralReportModel();
            obj = v.AutofilterEmployeeTo(RaiserName, RaiserCode).ToList();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult TransferPettyCashLogPopup(string ecf_gid = null)
        {
            List<SelectCourier> obj = new List<SelectCourier>();
            obj = objModel.ChangePettyLogPopup(ecf_gid).ToList();
            return PartialView(obj);
        }
        [HttpPost]
        public JsonResult getAutocompleteBranch(string BranchCode)
        {
            List<SelectCourier> obj = new List<SelectCourier>();
            CentralReportModel v = new CentralReportModel();
            obj = v.BranchCodeAutocomplete(BranchCode).ToList();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult downloadexcel()
        {

            DataTable dtnew = (DataTable)Session["ExcelExport"];
            if (dtnew.Rows.Count > 0)
            {
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "ECF Summary.xls"));
                Response.ContentType = "application/vnd.ms-excel";
                DataTable dt = dtnew;

                string str = string.Empty;
                foreach (DataColumn dtcol in dt.Columns)
                {
                    Response.Write(str + dtcol.ColumnName);
                    str = "\t";
                }
                Response.Write("\n");
                foreach (DataRow dr in dt.Rows)
                {
                    str = "";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        string content = dr[j].ToString();
                        content.Replace(System.Environment.NewLine, " ");
                        Response.Write(str + Convert.ToString(dr[j]));
                        str = "\t";
                    }

                    Response.Write("\n");
                    //GGG
                }
                Response.End();
            }
            return View();
        }
    }
}
