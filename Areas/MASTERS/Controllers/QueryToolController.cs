using IEM.Helper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Common;
using IEM.Areas.MASTERS.Models;
using ClosedXML;
using Newtonsoft.Json;
using IEM.App_Start;
 
namespace IEM.Areas.MASTERS.Controllers
{
   
    [NoDirectAccess]
    public class QueryToolController : Controller
    {
        dbLib db = new dbLib();
        //
        // GET: /MASTERS/QueryTool/
        #region
        CmnFunctions com = new CmnFunctions();
        #endregion
        public ActionResult Index()
        {

            QueryTool q;


            List<QueryTool> qry_list = new List<QueryTool>();

            q = new QueryTool();

            q.sqlquery_gid = "0";
            q.sqlquery_name = "-- Select Query --";
            q.sqlquery_sql = "";

            qry_list.Add(q);

            DataSet ds = db.GetProcedure("pr_iem_mst_tsqlquery");

            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    q = new QueryTool();

                    q.sqlquery_gid = dr[0].ToString();
                    q.sqlquery_name = dr[1].ToString();
                    q.sqlquery_sql = dr[2].ToString();
                    qry_list.Add(q);
                }
            }

            ds.Dispose();

            return View(qry_list);
        
        
        }

        public FileResult ExecuteDownloadQry()
        {
            DataTable dt = new DataTable();
            if (TempData["QueryOutput"] != null)
            {
                dt = TempData["QueryOutput"] as DataTable;
                ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
                wbook.Worksheets.Add(dt, "tab1");
                // Flush the workbook to the Response.OutputStream
                MemoryStream memoryStream = new MemoryStream();
                wbook.SaveAs(memoryStream);
                return File(memoryStream.ToArray(), System.Net.Mime.MediaTypeNames.Application.Octet, "Report.xlsx");
            }
            else
            {
                return File("FileNotFound", System.Net.Mime.MediaTypeNames.Application.Octet, "Report.xlsx");
            }
        }


        public ActionResult ExecuteQry(string sql)
        {
            try
            {
                DataTable dt = com.GetQueryOutput(sql);
                TempData["QueryOutput"] = null;
                if (dt.Rows.Count > 0)
                {
                    TempData["QueryOutput"] = dt;
                    return Json(new { success = true, responseText = "File generated successfully !" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, responseText = "Records Not Found !" }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost]
        public JsonResult GetQueryParameter(string sqlqueryid)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataTable dt = new DataTable();
                DataSet ds = db.GetQueryParameter(Convert.ToString(sqlqueryid));
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                }
               // if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                var Jsonresult = Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                Jsonresult.MaxJsonLength = int.MaxValue;
                return Jsonresult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public ActionResult ExecuteQry(QueryToolModel q)
        //{
        //    try
        //    {
        //        var x = q.qry;
        //        var filename = Path.Combine(System.IO.Path.GetTempPath(), "report.xlsx");

        //        CmnFunctions com = new CmnFunctions();

        //        DataTable dt = com.GetQueryOutput(x);

        //        ClosedXML.Excel.XLWorkbook wbook = new ClosedXML.Excel.XLWorkbook();
        //        wbook.Worksheets.Add(dt, "tab1");


        //        // Flush the workbook to the Response.OutputStream
        //        MemoryStream memoryStream = new MemoryStream();
        //        wbook.SaveAs(memoryStream);

        //        //write to file
        //        FileStream file = new FileStream(filename, FileMode.Create, FileAccess.Write);
        //        memoryStream.WriteTo(file);
        //        file.Close();
        //        memoryStream.Close();

        //        return Json(new { success = true, responseText = "File generated successfully !" }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex) 
        //    {
        //        return Json(new { success = false, responseText = ex.Message }, JsonRequestBehavior.AllowGet);
        //    }
        //}
    }
}
