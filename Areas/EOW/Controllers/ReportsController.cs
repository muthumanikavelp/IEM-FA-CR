using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;
using IEM.Helper;
using ClosedXML.Excel;
using System.IO;
using IEM.Common;
using IEM.Areas.EOW.Models;
using IEM.App_Start;
namespace IEM.Areas.EOW.Controllers
{

    [NoDirectAccess]
    public class ReportsController : Controller
    {
        ErrorLog objErrorLog = new ErrorLog();

        #region Declaration
        dbLib db = new dbLib();
        proLib plib = new proLib();
        DataTable dt = new DataTable();
        private ReportRepository objModel;
        #endregion
        //
        // GET: /EOW/Reports/


         public ReportsController()
            : this(new Reports())
        {

        }
         public ReportsController(ReportRepository objM)
        {
            objModel = objM;
        }



        public ActionResult LocalConveyance()
        {
            return View();
        }

        #region Local Conveyance
        [HttpPost]
        public JsonResult GetLocalConveyanceDateWaise(string DateFrom, string DateTo)
        {
            try
            {
                string Data1 = "";
               // DataSet ds = db.GetLocalConveyanceDateWaise(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo));
                DataSet ds = db.GetLocalConveyanceDateWaise(DateFrom, DateTo);
                //DateTime FirstDate=Convert.ToDateTime(DateFrom);
                //DateTime SecondDate=Convert.ToDateTime (DateTo);

                //if (SecondDate.Subtract(FirstDate).Days >30)
                //{
                //    var jsonResult= "Days should not be greater than 30 Days.";
                //    return jsonResult;
                //}
                //DataSet ds = db.GetLocalConveyanceDateWaise(plib.ConvertDate("01/11/2016"), plib.ConvertDate("30/11/2016"));
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    //return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                    var jsonResult = Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = int.MaxValue;
                    return jsonResult;
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult DownloadLocalConveyanceDateWise(string DateFrom, string DateTo)
        {
            try
            {
                if (Session["employee_code"] != null)
                {

                    string Data1 = "";
                    DataSet ds = db.GetLocalConveyanceDateWaise(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo));
                    if (ds != null)
                    {
                        using (XLWorkbook wb = new XLWorkbook())
                        {

                            dt = ds.Tables[0];
                            if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                            if (dt.Rows.Count > 0)
                            {
                                wb.Worksheets.Add(ds.Tables[0]);
                                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                                wb.Style.Font.Bold = true;

                                Response.Clear();
                                Response.Buffer = true;
                                Response.Charset = "";
                                Response.ContentType = "application/vnd.ms-excel";

                                Response.AddHeader("content-disposition", "attachment;filename= Local Conveyance Daywise Report.xls");
                            }
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
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }

        }
        #endregion

        #region Query Invoice
        [HttpPost]
        public JsonResult GetQueryInvoiceDateWaise(string DateFrom, string DateTo)
        {
            try
            {
                if (Session["employee_code"] != null)
                {
                    string Data1 = "";
                    DataSet ds = db.GetQueryInvoiceDateWaise(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo));

                    if (ds != null)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                        var jsonResult = Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                        jsonResult.MaxJsonLength = int.MaxValue;
                        return jsonResult;
                    }
                    else { return null; }
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }


        [HttpPost]
        public JsonResult DownloadQueryInvoiceDateWise(string DateFrom, string DateTo)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetQueryInvoiceDateWaise(plib.ConvertDate(DateFrom), plib.ConvertDate(DateTo));
                if (ds != null)
                {
                    using (XLWorkbook wb = new XLWorkbook())
                    {

                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                        if (dt.Rows.Count > 0)
                        {
                            wb.Worksheets.Add(ds.Tables[0]);
                            wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            wb.Style.Font.Bold = true;

                            Response.Clear();
                            Response.Buffer = true;
                            Response.Charset = "";
                            Response.ContentType = "application/vnd.ms-excel";

                            Response.AddHeader("content-disposition", "attachment;filename= Query Invoice Daywise Report.xls");
                        }
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


        #region Employeetravel

        [HttpGet]
        public ActionResult Employee_Travel()
        {
            return View();
        }


        public JsonResult GetEmpTravelDetails(string FromDate = null, string ToDate = null)
        {
            string data0 = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objModel.GetEmpTravelDetails(FromDate, ToDate);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }

                var js = Json(new { data0 }, JsonRequestBehavior.AllowGet);
                js.MaxJsonLength = 2147483647;
                return js;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }

        public void DTtoExcel(string FromDate = null, string ToDate = null)
        {
            if (Session["employee_code"] != null)
            {


                DataSet ds = new DataSet();
                DataTable dt;
                if (!string.IsNullOrEmpty(FromDate) && !string.IsNullOrEmpty(ToDate))
                {
                    ds = objModel.GetEmpTravelDetails_Download(FromDate, ToDate);
                    dt = ds.Tables[0];
                    string dtnow = DateTime.Now.ToString("yyMMdd");
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt, "QueryResult");
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=QueryResult_" + dtnow + ".xlsx");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
            }
        }

      #endregion


    }
}
