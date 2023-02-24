using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using IEM.Helper;
using Newtonsoft.Json;
using ClosedXML.Excel;
using System.IO;
using IEM.App_Start;
namespace IEM.Areas.FLEXISPEND.Controllers
{

    [NoDirectAccess]
    public class ReportsController : Controller
    {

        #region Declaration
        dbLib db = new dbLib();
        proLib plib = new proLib();
        DataTable dt = new DataTable();
        #endregion

        //
        // GET: /FLEXISPEND/Reports/


        #region Bank Summary
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetBankSummary(string DateFrom, string DateTo, string ECFNo, string View)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetBankSummaryReport(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo), ECFNo, View, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult DownloadBankSummary(string DateFrom, string DateTo, string ECFNo, string View)
        {
            try
            {

                DataSet ds = db.GetBankSummaryReport(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo), ECFNo, View, plib.LoginUserId);
                if (ds != null)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(ds.Tables[1]);
                        wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        wb.Style.Font.Bold = true;

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.ms-excel";

                        Response.AddHeader("content-disposition", "attachment;filename= Bank Summary Report.xls");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }

                    return Json("", JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region Amort Report
        public ActionResult Amort()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAmortReport(string DateFrom, string DateTo, string ECFNo)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetAmortReport(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo), ECFNo, "0", plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult DownloadAmortReport(string DateFrom, string DateTo, string ECFNo)
        {
            try
            {

                DataSet ds = db.GetAmortReport(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo), ECFNo, "1", plib.LoginUserId);
                if (ds != null)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(ds.Tables[1]);
                        wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        wb.Style.Font.Bold = true;

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.ms-excel";

                        Response.AddHeader("content-disposition", "attachment;filename= Amort Report.xls");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }

                    return Json("", JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region Advance Report
        public ActionResult Advance()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetAdvanceReport(string DateFrom, string DateTo, string LiqDateFrom, string LiqDateTo, string ECFNo, string Raiser, string Vendor, string glcode, string Location)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetAdvanceReport(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo), plib.ConvertDate(LiqDateFrom), plib.ConvertDate(LiqDateTo), ECFNo, "0", plib.LoginUserId, Raiser, Vendor, glcode, Location);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult DownloadAdvanceReport(string DateFrom, string DateTo, string LiqDateFrom, string LiqDateTo, string ECFNo, string Raiser, string Vendor, string glcode, string Location)
        {
            try
            {

                DataSet ds = db.GetAdvanceReport(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo), plib.ConvertDate(LiqDateFrom), plib.ConvertDate(LiqDateTo), ECFNo, "1", plib.LoginUserId, Raiser, Vendor, glcode, Location);
                if (ds != null)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(ds.Tables[1]);
                        wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        wb.Style.Font.Bold = true;

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.ms-excel";

                        Response.AddHeader("content-disposition", "attachment;filename= Advance Report.xls");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }

                    return Json("", JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region PPX Liquidation Report
        public ActionResult PPXLiquidation()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetPPXLiquidationReport(string DateFrom, string DateTo,  string ECFNo)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetPPXLiquidationReport(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo), ECFNo, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult DownloadPPXLiquidationReport(string DateFrom, string DateTo, string ECFNo)
        {
            try
            {

                DataSet ds = db.GetPPXLiquidationReport(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo), ECFNo, plib.LoginUserId);
                if (ds != null)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(ds.Tables[2]);
                        wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        wb.Style.Font.Bold = true;

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.ms-excel";

                        Response.AddHeader("content-disposition", "attachment;filename= PPX Liquidation Report.xls");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }

                    return Json("", JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region Debit Report
        public ActionResult Debit()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetDebitReport(string DateFrom, string DateTo, string ECFNo, string Raiser, string Vendor, string glcode, string Location)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetDebitReport(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo), ECFNo, "0", plib.LoginUserId, Raiser, Vendor, glcode, Location);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult DownloadDebitReport(string DateFrom, string DateTo, string ECFNo, string Raiser, string Vendor, string glcode, string Location)
        {
            try
            {

                DataSet ds = db.GetDebitReport(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo), ECFNo, "1", plib.LoginUserId, Raiser, Vendor, glcode, Location);
                if (ds != null)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(ds.Tables[1]);
                        wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        wb.Style.Font.Bold = true;

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.ms-excel";

                        Response.AddHeader("content-disposition", "attachment;filename= Debit Report.xls");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }

                    return Json("", JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region Credit Report
        public ActionResult Credit()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetCreditReport(string DateFrom, string DateTo, string ECFNo, string Raiser, string Vendor, string glcode, string Location)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetCreditReport(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo), ECFNo, "0", plib.LoginUserId, Raiser, Vendor, glcode, Location);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult DownloadCreditReport(string DateFrom, string DateTo, string ECFNo, string Raiser, string Vendor, string glcode, string Location)
        {
            try
            {

                DataSet ds = db.GetCreditReport(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo), ECFNo, "1", plib.LoginUserId, Raiser, Vendor, glcode, Location);
                if (ds != null)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(ds.Tables[1]);
                        wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        wb.Style.Font.Bold = true;

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.ms-excel";

                        Response.AddHeader("content-disposition", "attachment;filename= Credit Report.xls");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }

                    return Json("", JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }
        #endregion


        #region Cenvat Report
        public ActionResult Cenvat()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetCenvatReport(string DateFrom, string DateTo, string ECFNo)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetCenvantReport(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo), ECFNo, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult DownloadCenvatReport(string DateFrom, string DateTo, string ECFNo)
        {
            try
            {

                DataSet ds = db.GetCenvantReport(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo), ECFNo, plib.LoginUserId);
                if (ds != null)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(ds.Tables[1]);
                        wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        wb.Style.Font.Bold = true;

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.ms-excel";

                        Response.AddHeader("content-disposition", "attachment;filename= Cenvat Report.xls");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }

                    return Json("", JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region QC Report
        public ActionResult QC()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetQCReport(string DateFrom, string DateTo, string ECFNo, string EmpId, string SuppId)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetQCReport(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo), ECFNo, EmpId, SuppId, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1].Copy();
                    dt.Columns.Remove("ecfIsRemoved");
                    dt.Columns.Remove("ECFgid");
                    dt.Columns.Remove("Invoicegid");
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult DownloadQCReport(string DateFrom, string DateTo, string ECFNo, string EmpId, string SuppId)
        {
            try
            {

                DataSet ds = db.GetQCReport(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo), ECFNo, EmpId, SuppId,  plib.LoginUserId);
                if (ds != null)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        dt = ds.Tables[1].Copy();
                        dt.Columns.Remove("ecfIsRemoved");
                        dt.Columns.Remove("ECFgid");
                        dt.Columns.Remove("Invoicegid");

                        wb.Worksheets.Add(dt);
                        wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        wb.Style.Font.Bold = true;

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.ms-excel";

                        Response.AddHeader("content-disposition", "attachment;filename= QC Report.xls");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }

                    return Json("", JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}
