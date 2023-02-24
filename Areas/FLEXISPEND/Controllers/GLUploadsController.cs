using ClosedXML.Excel;
using IEM.Areas.FLEXISPEND.Models;
using IEM.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.App_Start;
namespace IEM.Areas.FLEXISPEND.Controllers
{
    [NoDirectAccess]
    public class GLUploadsController : Controller
    {
        #region Decalartion
        dbLib db = new dbLib();
        proLib plib = new proLib();
        DataTable dt = new DataTable();
        FSRepository _fsRep = new FSRepository();
        #endregion

        //
        // GET: /FLEXISPEND/GLUploads/
        public ActionResult Index()
        {
            return View();
        }

        #region GL Upload
        [HttpPost]
        public JsonResult SetGLUpload(string UploadFrom, string UploadTo, string UploadTypeId, string BatchNo, string Status)
        {
            try
            {
                Session["GLDownlinkFile"] = null;
                Session["GLBatchNo"] = null;
                string Data1 = "";
                DataSet ds = db.SetGLUploads(plib.ConvertDate(UploadFrom), plib.ConvertDate(UploadTo), UploadTypeId, BatchNo, Status, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        Data1 = JsonConvert.SerializeObject(dt);

                        if (dt.Rows[0]["Clear"].ToString().ToLower() == "true" || dt.Rows[0]["Clear"].ToString() == "1")
                        {
                            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                            {
                                DataTable _tmpdata = ds.Tables[1];
                                _tmpdata.TableName = "GLUpload";
                                Session["GLDownlinkFile"] = _tmpdata;
                                Session["GLBatchNo"] = dt.Rows[0]["BatchNo"].ToString();
                            }
                        }
                    }
                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        //downloading Excel File
        public JsonResult DownloadExcel()
        {
            DataTable _downloadingData = Session["GLDownlinkFile"] as DataTable;
            string _fileName = Session["GLBatchNo"] as string;
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
                    Response.AddHeader("content-disposition", "attachment;filename= " + _fileName + ".xlsx");

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

        public JsonResult LoadCUploadDetails(string UploadDate, string UploadTypeId, string ViewType)
        {
            try
            {
                return Json(_fsRep.LoadGLUploadTypesCancellation(plib.ConvertDate(UploadDate), UploadTypeId, ViewType, plib.LoginUserId), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region Uploads
        /*public JsonResult DownloadExcelFile()
        {
            DataTable dtnew = new DataTable();
            dtnew.TableName = "GLDetails";
            dtnew.Columns.Add("Header1", typeof(string));
            dtnew.Columns.Add("Header2", typeof(string));
            dtnew.Rows.Add(new object[] { "Message01", "Message02" });
            dtnew.Rows.Add(new object[] { "Message11", "Message12" });

            DownloadExcelFile(dtnew);

            dts.TableName = "GLDetails";
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dts);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename= EmployeeReport.xlsx");

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

        public Stream ConvertStringtoStream(string txt)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(txt);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }*/
        #endregion
    }
}
