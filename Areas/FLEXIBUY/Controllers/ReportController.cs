using ClosedXML.Excel;
using IEM.Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{
    [NoDirectAccess]
    public class ReportController : Controller
    {
        #region Declaration
        proLib pl = new proLib();
        dbLib db = new dbLib();
        DataTable dt;
        #endregion

        //
        // GET: /FLEXIBUY/Report/
        public ActionResult PR()
        {
            return View();
        }

        //
        // GET: /FLEXIBUY/Report/
        public ActionResult PAR()
        {
            return View();
        }
        // GET: /FLEXIBUY/Report/
        public ActionResult POinvoice()
        {
            return View();
        }
        public ActionResult WOinvoice()
        {
            return View();
        }
        //GET:/FLEXIBUY/Report/
        public ActionResult CbfUtilitiesRpt()
        {
            return View();
        }

        [HttpPost]
        public JsonResult FetchReportData(string RegionId, string Type, string Fromdate, string Todate)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.FetchReport(RegionId, Type, pl.LoginUserId, Fromdate, Todate);
                if (ds != null && ds.Tables.Count > 1)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        [HttpPost]
        public JsonResult FetchReportPOInvoiceData(string pono, string Fromdate, string Todate)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.FetchPoInvoiceRpt(pono, pl.LoginUserId, Fromdate, Todate);
                if(ds!=null && ds.Tables.Count>1){
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult FetchReportWOInvoiceData(string wono, string Fromdate, string Todate)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.FetchWoInvoiceRpt(wono, pl.LoginUserId, Fromdate, Todate);
                if (ds != null && ds.Tables.Count > 1)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult FetchReportCBFData(string cbfno, string Fromdate, string Todate)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.FetchCbfDataRpt(cbfno, pl.LoginUserId, Fromdate, Todate);
                if (ds != null && ds.Tables.Count > 1)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult DownloadcbfReportData(string cbfno, string Fromdate, string Todate)
        {
            try
            {
                Session["FBRptCBFFile"] = null;
                string Data1 = "";
                DataSet ds = db.FetchCbfDataRpt(cbfno, pl.LoginUserId, Fromdate, Todate);
                if (ds != null && ds.Tables.Count > 1)
                {
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0)
                    {
                        try
                        {
                            DataTable _tmpdata = ds.Tables[1];
                            //_tmpdata.Columns.RemoveAt(1);
                            Session["FBRptCBFFile"] = _tmpdata;
                            if (_tmpdata != null && _tmpdata.Rows.Count > 0)
                            {
                                Data1 = "process";
                            }
                        }
                        catch
                        {
                            Session["FBRptCBFFile"] = null;
                        }
                    }
                }
                return Json(Data1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        public JsonResult DownloadCBFRpt()
        {
            DataTable _downloadingData = Session["FBRptCBFFile"] as DataTable;
            if (_downloadingData != null)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_downloadingData);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename= CBF Report.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DownloadPOInvoiceReportData(string pono, string Fromdate, string Todate)
        {
            try
            {
                Session["FBRptpoinvoiceFile"] = null;
                string Data1 = "";
                DataSet ds = db.FetchPoInvoiceRpt(pono, pl.LoginUserId, Fromdate, Todate);
                if (ds != null && ds.Tables.Count > 1)
                {
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0)
                    {
                        try
                        {
                            DataTable _tmpdata = ds.Tables[1];
                           // _tmpdata.Columns.RemoveAt(1);
                            Session.Remove("FBRptpoinvoiceFile");
                            Session["FBRptpoinvoiceFile"] = _tmpdata;
                            if (_tmpdata != null && _tmpdata.Rows.Count > 0)
                            {
                                Data1 = "process";
                            }
                        }
                        catch
                        {
                            Session["FBRptpoinvoiceFile"] = null;
                        }
                    }
                }
                return Json(Data1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        public JsonResult DownloadPoInvoiceRpt()
        {
            DataTable _downloadingData = Session["FBRptpoinvoiceFile"] as DataTable;
            if (_downloadingData != null)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_downloadingData);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename= PO Invoice Report.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DownloadWOInvoiceReportData(string wono, string Fromdate, string Todate)
        {
            try
            {
                Session["FBRptwoinvoiceFile"] = null;
                string Data1 = "";
                DataSet ds = db.FetchWoInvoiceRpt(wono, pl.LoginUserId, Fromdate, Todate);
                if (ds != null && ds.Tables.Count > 1)
                {
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0)
                    {
                        try
                        {
                            DataTable _tmpdata = ds.Tables[1];
                           // _tmpdata.Columns.RemoveAt(1);
                            Session["FBRptwoinvoiceFile"] = _tmpdata;
                            if (_tmpdata != null && _tmpdata.Rows.Count > 0)
                            {
                                Data1 = "process";
                            }
                        }
                        catch
                        {
                            Session["FBRptwoinvoiceFile"] = null;
                        }
                    }
                }
                return Json(Data1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        public JsonResult DownloadWoInvoiceRpt()
        {
            DataTable _downloadingData = Session["FBRptwoinvoiceFile"] as DataTable;
            if (_downloadingData != null)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_downloadingData);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename= WO Invoice Report.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DownloadReportData(string RegionId, string Type, string Fromdate, string Todate)
        {
            try
            {
                Session["FBRptFile"] = null;
                string Data1 = "";
                DataSet ds = db.FetchReport(RegionId, Type, pl.LoginUserId, Fromdate, Todate);
                if (ds != null && ds.Tables.Count > 1)
                {
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0)
                    {
                        try
                        {

                            DataTable _tmpdata = ds.Tables[1];
                            _tmpdata.Columns.RemoveAt(1);

                            if (Type == "0")
                                _tmpdata.TableName = "PAR";
                            else
                                _tmpdata.TableName = "PR";

                            Session["FBRptFile"] = _tmpdata;
                            if (_tmpdata != null && _tmpdata.Rows.Count > 0)
                            {
                                Data1 = "process";
                            }
                        }
                        catch
                        {
                            Session["FBRptFile"] = null;
                        }
                    }
                }
                return Json(Data1, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //downloading Report data to Excel File
        public JsonResult DownloadRpt()
        {
            DataTable _downloadingData = Session["FBRptFile"] as DataTable;
            if (_downloadingData != null)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_downloadingData);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    if (_downloadingData.TableName == "PAR")
                        Response.AddHeader("content-disposition", "attachment;filename= PAR Report.xlsx");
                    else
                        Response.AddHeader("content-disposition", "attachment;filename= PR Report.xlsx");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        //Region Auto Complete
        public JsonResult GetAutoCompleteRegion(string txt)
        {
            try { return Json(GetAutoData(txt, pl.LoginUserId), JsonRequestBehavior.AllowGet); }
            catch { return null; }
        }

        //Region details Auto Complete
        public List<string> GetAutoData(string txt, string _UsrId)
        {
            try
            {
                List<string> result = new List<string>();
                DataSet ds = db.GetAutoComplete(txt, "19", _UsrId);
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        result.Add(string.Format("{0}~{1}", dr[0], dr[1]));
                    }
                }
                ds.Dispose();
                return result;
            }
            catch { return null; }
        }
    }
}
