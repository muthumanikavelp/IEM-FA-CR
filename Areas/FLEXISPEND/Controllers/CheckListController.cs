using IEM.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using System.IO;
using IEM.App_Start;
namespace IEM.Areas.FLEXISPEND.Controllers
{

    [NoDirectAccess]
    public class CheckListController : Controller
    {
        #region Declaration
        proLib pl = new proLib();
        dbLib db = new dbLib();
        DataTable dt;
        #endregion

        //
        // GET: /FLEXISPEND/CheckList/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetMasterCheckList(string DocSubTypeId, string POType, string IsActive)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "";
                DataSet ds = db.GetCheckList(DocSubTypeId, "0", POType, IsActive, "2", pl.LoginUserId);
                dt = ds.Tables[0];

                //Add Default Row
                DataRow dr = dt.NewRow();
                dr[0] = "0"; dr[1] = "-Add New-";
                dt.Rows.InsertAt(dr, 0);

                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[2];
                if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1, Data2, Data3 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        public JsonResult GetCheckListGrid(string DocSubTypeId, string ParentId, string POType, string IsActive)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "";
                DataSet ds = db.GetCheckList(DocSubTypeId, ParentId, POType, IsActive, "1", pl.LoginUserId);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                dt = ds.Tables[2];
                if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1, Data2, Data3 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Save Check List Grid
        public JsonResult SaveCheckListGrid(string ChkLstId, string ParentId, string DocSubTypeId, string PoType, string Title, string IsConfirmed, string Reason, string IsActive, string DisOrder, string IsActiveFilter)
        {
            try
            {
                string Clear = "", Data1 = "", Data2 = "", Data3 = "";
                DataSet ds = db.SetCheckList(ChkLstId, ParentId, DocSubTypeId, PoType, Title, IsConfirmed, Reason, IsActive, DisOrder, "0", IsActiveFilter, pl.LoginUserId);
                if (ds != null)
                {
                    Clear = ds.Tables[0].Rows[0]["Clear"].ToString().ToLower();

                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    if (Clear == "1" || Clear == "true")
                    {
                        dt = ds.Tables[1];

                        //Add Default Row
                        DataRow dr = dt.NewRow();
                        dr[0] = "0"; dr[1] = "-Add New-";
                        dt.Rows.InsertAt(dr, 0);

                        if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                        dt = ds.Tables[2];
                        if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                        return Json(new { Data1, Data2, Data3 }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch { return null; }
        }

        //downloading Excel File
        [HttpPost]
        public JsonResult DownloadExcel(string ViewType, string FileName)
        {
            DataSet ds = db.GetCommonExcelDownload(ViewType, pl.LoginUserId);
            DataTable _downloadingData = ds.Tables[0];
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
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename= " + FileName + ".xls");

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
