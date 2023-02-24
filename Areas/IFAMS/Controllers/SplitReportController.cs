using IEM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.IFAMS.Models;
using Newtonsoft.Json;
using ClosedXML.Excel;
using System.IO;
using IEM.App_Start;
namespace IEM.Areas.IFAMS.Controllers
{
      [NoDirectAccess]
    public class SplitReportController : Controller
    {
         ErrorLog objErrorLog = new ErrorLog();
         private Ifams_RptRepository objRep;
        public SplitReportController()
            : this(new Ifams_RptDataModel())
        {

        }
        public SplitReportController(Ifams_RptRepository objModel)
        {
            objRep = objModel;
        }
        //
        // GET: /IFAMS/SplitReport/

        public ActionResult SplitReport()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetSplitSummary()
        {
            string data0 = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objRep.GetSplitSummary();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }

                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult MergeReport() 
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetMergeSummary() 
        {
            string data0 = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objRep.GetMergeSummary(); 
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }

                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult CreateGroup() 
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetAssetDetails()
        {
            string data0 = string.Empty;
            string LocationCode = "", capdate = "";
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objRep.GetAssetDetails(LocationCode,capdate); 
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }

                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ReductionReport() 
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetReductionSummary() 
        {
            string data0 = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objRep.GetReductionSummary(); 
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }

                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }
        public void DTtoExcelFA(string rptfor = null) 
        {
            DataSet ds = new DataSet();
            DataTable dt;
            string fname = "";
            switch (rptfor)
            {
                case "reduction":
                    ds = objRep.GetReductionSummary();
                    fname = "Reduction_";
                    break;
                case "merge":
                    ds = objRep.GetMergeSummary();
                    fname = "Merge_";
                    break;
                case "split":
                    ds = objRep.GetSplitSummary();
                    fname = "Split_";
                    break;
                default:
                    break;
            }
            if (rptfor != null && rptfor != "")
            {
                dt = ds.Tables[0];
                string dtnow = DateTime.Now.ToString("yyMMdd");
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, fname);
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=RPT_" + fname + dtnow + ".xlsx");
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
}
