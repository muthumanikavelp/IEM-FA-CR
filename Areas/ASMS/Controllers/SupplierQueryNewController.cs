using ClosedXML.Excel;
using IEM.Areas.ASMS.Models;
using IEM.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.App_Start;
namespace IEM.Areas.ASMS.Controllers
{

      [NoDirectAccess]
    public class SupplierQueryNewController : Controller
    {
         ErrorLog objErrorLog = new ErrorLog();
         private SupQueryRepository objModel;
        public SupplierQueryNewController()
             : this(new SupQueryDataModel())
        {

        }
        public SupplierQueryNewController(SupQueryRepository objM)
        {
            objModel = objM;
        }
        //
        // GET: /ASMS/SupplierQueryNew/

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetSupplierQuery()
        {

            string data0 = string.Empty;
            string data1 = string.Empty;
            //try
            //{
            //    DataSet ds = new DataSet();
            //    DataTable dt;
            //    ds = objModel.GetSupplierQuery();
            //    dt = ds.Tables[0];
            //    if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
            //    dt = ds.Tables[1];
            //    if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
            //    //return Json(new { data0, data1 }, JsonRequestBehavior.AllowGet);

            //    var jsonResult = Json(new { data0, data1 }, JsonRequestBehavior.AllowGet);
            //    jsonResult.MaxJsonLength = 2147483647;
            //    return jsonResult;
            //}
            //catch (Exception ex)
            //{
            //    objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            //    return Json(new { data0, data1 }, JsonRequestBehavior.AllowGet);
            //}

            try
            {
                objErrorLog.WriteErrorLog("Method started","64");
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objModel.GetSupplierQuery();
                dt = ds.Tables[0];
                objErrorLog.WriteErrorLog("dt count - "+dt.Rows.Count.ToString(), "69");
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                objErrorLog.WriteErrorLog("dt count - " + dt.Rows.Count.ToString(), "71");
                //return Json(new { data0, data1 }, JsonRequestBehavior.AllowGet);

                var jsonResult = Json(new { data0 }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                objErrorLog.WriteErrorLog("dt count - " + dt.Rows.Count.ToString(), "76");
                return jsonResult;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetSupplierQuerySearch(string fdate, string todate, int depgid, String supcode, String supname, string suppanno)
        {

            string data0 = string.Empty;
            string data1 = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objModel.GetSupplierQuerysearch(fdate, todate, depgid, supcode, supname, suppanno);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }

                //return Json(new { data0, data1 }, JsonRequestBehavior.AllowGet);

                var jsonResult = Json(new { data0 }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }
        public void DTtoExcell(string CreatedDateFrom, string CreatedDateTo, int deptgid, string supname, string suppanno)
        {
            DataSet ds = new DataSet();
            DataTable dt;
            ds = objModel.GetSupplierQueryReport(CreatedDateFrom, CreatedDateTo, deptgid, supname, suppanno);
            dt = ds.Tables[0];
            string dtnow = DateTime.Now.ToString("yyMMdd");
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "SupplierHeader");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=SupplierQuery_" + dtnow + ".xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }

        public JsonResult GetDeptList()
        {

            string data0 = string.Empty;
            string data1 = string.Empty;
          

            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objModel.GetDeptList();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }

                //return Json(new { data0, data1 }, JsonRequestBehavior.AllowGet);

                var jsonResult = Json(new { data0 }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
