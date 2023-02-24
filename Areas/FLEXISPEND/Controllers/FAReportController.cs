using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Helper;
using Newtonsoft.Json;
using IEM.Areas.FLEXISPEND.Models;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using IEM.App_Start;
namespace IEM.Areas.FLEXISPEND.Controllers
{

    [NoDirectAccess]
    public class FAReportController : Controller
    {

        #region Declaration
        proLib pl = new proLib();
        dbLib db = new dbLib();
        DataTable dt;
        #endregion

        //
        // GET: /FLEXISPEND/FAReport/
        public ActionResult AdvanceReport()
        {
            return View();
        }

        [HttpPost]
        public JsonResult FetchAdvanceReport(string Date, string Dateto, string ECFNo, string EmpId, string SuppId, string Raiserid, string glno, string Branch)
        {
            try
            {
               
                string Data1 = "", Data2 = "";
                DataSet ds = db.FetchAdvanceReport(Date, Dateto, ECFNo, EmpId, SuppId, Raiserid, glno, Branch, pl.LoginUserId);
                if (ds != null && ds.Tables.Count > 1)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                }
                var jsonResult = Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
               // return Json(Data2 , JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult DownloadAdvanceReport(string Date, string Dateto, string ECFNo, string EmpId, string SuppId, string Raiserid, string glno, string branch)
        {
            try
            {
                Session["ARDownlinkFile"] = null;
                string Data1 = "";
                DataSet ds = db.FetchAdvanceReport(Date, Dateto, ECFNo, EmpId, SuppId, Raiserid, glno, branch, pl.LoginUserId);
                if (ds != null && ds.Tables.Count > 1)
                {
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0)
                    {
                        try
                        {
                            
                            DataTable _tmpdata = ds.Tables[1];
                            _tmpdata.Columns.RemoveAt(1);
                            _tmpdata.TableName = "AdvanceRpt";
                            Session["ARDownlinkFile"] = _tmpdata;
                            if (_tmpdata != null && _tmpdata.Rows.Count > 0)
                            {
                                Data1 = "process";
                            }
                        }
                        catch
                        {
                            Session["ARDownlinkFile"] = null;
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

        //downloading Advance Excel File
        public JsonResult DownloadExcelAdavanceRpt()
        {
            DataTable _downloadingData = Session["ARDownlinkFile"] as DataTable;
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
                    Response.AddHeader("content-disposition", "attachment;filename= FA Advance Report.xlsx");

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

        //
        // GET: /FLEXISPEND/FAReport/
        public ActionResult Retention()
        {
            return View();
        }

        [HttpPost]
        public JsonResult FetchRetentionReport(string Date, string ECFNo, string EmpId, string SuppId)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.FetchRetentionReport(pl.ConvertDate(Date == null ? "" : Date), ECFNo, EmpId, SuppId, pl.LoginUserId);
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
        public JsonResult DownloadRetentionReport(string Date, string ECFNo, string EmpId, string SuppId)
        {
            try
            {
                Session["RRDownlinkFile"] = null;
                string Data1 = "";
                DataSet ds = db.FetchRetentionReport(pl.ConvertDate(Date == null ? "" : Date), ECFNo, EmpId, SuppId, pl.LoginUserId);
                if (ds != null && ds.Tables.Count > 1)
                {
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0)
                    {
                        try
                        {
                            DataTable _tmpdata = ds.Tables[1];
                            _tmpdata.Columns.RemoveAt(1);
                            _tmpdata.TableName = "RetentionRpt";
                            Session["RRDownlinkFile"] = _tmpdata;
                            if (_tmpdata != null && _tmpdata.Rows.Count > 0) {
                                Data1 = "process";
                            }
                        }
                        catch
                        {
                            Session["RRDownlinkFile"] = null;
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

        //downloading Retention Excel File
        public JsonResult DownloadExcelRetentionRpt()
        {
            DataTable _downloadingData = Session["RRDownlinkFile"] as DataTable;
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
                    Response.AddHeader("content-disposition", "attachment;filename= FA Retention Report.xlsx");

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
        //
        // GET: /FLEXISPEND/FAReport/
        public ActionResult Sale()
        {
            return View();
        }

        [HttpPost]
        public JsonResult FetchSalesReport(string DateFrom, string DateTo)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.FetchSalesReport(pl.ConvertDate(DateFrom == null ? "" : DateFrom), pl.ConvertDate(DateTo == null ? "" : DateTo), pl.LoginUserId);
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
        public JsonResult DownloadSaleReport(string DateFrom, string DateTo)
        {
            try
            {
                Session["SRDownlinkFile"] = null;
                string Data1 = "";
                DataSet ds = db.FetchSalesReport(pl.ConvertDate(DateFrom == null ? "" : DateFrom), pl.ConvertDate(DateTo == null ? "" : DateTo), pl.LoginUserId);
                if (ds != null && ds.Tables.Count > 1)
                {
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0)
                    {
                        try
                        {
                            DataTable _tmpdata = ds.Tables[1];
                            //_tmpdata.Columns.RemoveAt(2);
                            _tmpdata.TableName = "SaleRpt";
                            Session["SRDownlinkFile"] = _tmpdata;
                            if (_tmpdata != null && _tmpdata.Rows.Count > 0)
                            {
                                Data1 = "process";
                            }
                        }
                        catch
                        {
                            Session["SRDownlinkFile"] = null;
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

        //downloading Sale Excel File
        public JsonResult DownloadExcelSaleRpt()
        {
            DataTable _downloadingData = Session["SRDownlinkFile"] as DataTable;
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
                    Response.AddHeader("content-disposition", "attachment;filename= FA Sale Report.xlsx");

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
        //
        // GET: /FLEXISPEND/FAReport/
        public ActionResult SaleInvoiceTracker()
        {
            return View();
        }

        [HttpPost]
        public JsonResult FetchSaleInvoiceTrackerReport(string DateFrom, string DateTo)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.FetchSaleInvoiceTrackerReport(pl.ConvertDate(DateFrom == null ? "" : DateFrom), pl.ConvertDate(DateTo == null ? "" : DateTo), pl.LoginUserId);
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
        public JsonResult DownloadSaleInvoiceTrackerReport(string DateFrom, string DateTo)
        {
            try
            {
                Session["SITRDownlinkFile"] = null;
                string Data1 = "";
                DataSet ds = db.FetchSaleInvoiceTrackerReport(pl.ConvertDate(DateFrom == null ? "" : DateFrom), pl.ConvertDate(DateTo == null ? "" : DateTo), pl.LoginUserId);
                if (ds != null && ds.Tables.Count > 1)
                {
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0)
                    {
                        try
                        {
                            DataTable _tmpdata = ds.Tables[1];
                            _tmpdata.TableName = "SaleInvoiceTrackerRpt";
                            Session["SITRDownlinkFile"] = _tmpdata;
                            if (_tmpdata != null && _tmpdata.Rows.Count > 0)
                            {
                                Data1 = "process";
                            }
                        }
                        catch
                        {
                            Session["SITRDownlinkFile"] = null;
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

        //downloading Sale Invoice Tracker Excel File
        public JsonResult DownloadExcelSaleInvoiceTrackerRpt()
        {
            DataTable _downloadingData = Session["SITRDownlinkFile"] as DataTable;
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
                    Response.AddHeader("content-disposition", "attachment;filename= Sale Invoice Tracker Report.xlsx");

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
        //
        // GET: /FLEXISPEND/FAReport/
        public ActionResult CBFTracker()
        {
            return View();
        }

        [HttpPost]
        public JsonResult FetchCBFTrackerReport(string Status)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.FetchCBFTrackerReport(Status, pl.LoginUserId);
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
        public JsonResult DownloadCBFTrackerReport(string Status)
        {
            try
            {
                Session["CBFTRDownlinkFile"] = null;
                string Data1 = "";
                DataSet ds = db.FetchCBFTrackerReport(Status, pl.LoginUserId);
                if (ds != null && ds.Tables.Count > 1)
                {
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0)
                    {
                        try
                        {
                            DataTable _tmpdata = ds.Tables[1];
                            _tmpdata.Columns.RemoveAt(1);
                            _tmpdata.TableName = "CBFTrackerRpt";
                            Session["CBFTRDownlinkFile"] = _tmpdata;
                            if (_tmpdata != null && _tmpdata.Rows.Count > 0)
                            {
                                Data1 = "process";
                            }
                        }
                        catch
                        {
                            Session["CBFTRDownlinkFile"] = null;
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

        //downloading CBF Tracker Excel File
        public JsonResult DownloadExcelCBFTrackerRpt()
        {
            DataTable _downloadingData = Session["CBFTRDownlinkFile"] as DataTable;
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
                    Response.AddHeader("content-disposition", "attachment;filename= CBF Tracker Report.xlsx");

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

        //
        // GET: /FLEXISPEND/FAReport/
        public ActionResult CBFUtilization()
        {
            return View();
        }

        [HttpPost]
        public JsonResult FetchCBFUtilizationReport(string CreationDateFrom, string CreationDateTo, string ApprovalDateFrom, string ApprovalDateTo, string CBFNo)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.FetchCBFUtilizationReport(pl.ConvertDate(CreationDateFrom == null ? "" : CreationDateFrom), pl.ConvertDate(CreationDateTo == null ? "" : CreationDateTo), pl.ConvertDate(ApprovalDateFrom == null ? "" : ApprovalDateFrom), pl.ConvertDate(ApprovalDateTo == null ? "" : ApprovalDateTo), CBFNo, pl.LoginUserId);
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
        public JsonResult DownloadUtilizationReport(string CreationDateFrom, string CreationDateTo, string ApprovalDateFrom, string ApprovalDateTo, string CBFNo)
        {
            try
            {
                Session["CBFURDownlinkFile"] = null;
                string Data1 = "";
                DataSet ds = db.FetchCBFUtilizationReport(pl.ConvertDate(CreationDateFrom == null ? "" : CreationDateFrom), pl.ConvertDate(CreationDateTo == null ? "" : CreationDateTo), pl.ConvertDate(ApprovalDateFrom == null ? "" : ApprovalDateFrom), pl.ConvertDate(ApprovalDateTo == null ? "" : ApprovalDateTo), CBFNo, pl.LoginUserId);
                if (ds != null && ds.Tables.Count > 1)
                {
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0)
                    {
                        try
                        {
                            DataTable _tmpdata = ds.Tables[1];
                            _tmpdata.TableName = "CBFUtilizationRpt";
                            Session["CBFURDownlinkFile"] = _tmpdata;
                            if (_tmpdata != null && _tmpdata.Rows.Count > 0)
                            {
                                Data1 = "process";
                            }
                        }
                        catch
                        {
                            Session["CBFURDownlinkFile"] = null;
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

        //downloading CBF Utilization Excel File
        public JsonResult DownloadExcelUtilizationRpt()
        {
            DataTable _downloadingData = Session["CBFURDownlinkFile"] as DataTable;
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
                    Response.AddHeader("content-disposition", "attachment;filename= CBF Utilization Report.xlsx");

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
    }
}
