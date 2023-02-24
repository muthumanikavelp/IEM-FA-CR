using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;
using System.Web.Mvc;
using IEM.Helper;
using Newtonsoft.Json;
using IEM.Areas.FLEXISPEND.Models;
using System.IO;
using ClosedXML.Excel;
using IEM.Common;
using Excel;
using IEM.Areas.MASTERS.Controllers;
using IEM.App_Start;
namespace IEM.Areas.FLEXISPEND.Controllers
{
    [NoDirectAccess]
    public class ChqDespatchAWBUpdationController : Controller
    {

        #region Declaration
        dbLib db = new dbLib();
        proLib plib = new proLib();
        ForMailController MailController = new ForMailController();
        #endregion

        //
        // GET: /FLEXISPEND/ChqDespatchAWBUpdation/

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetChequeDespatchAwbUpdation(string ChqDate, string PVNo, string BatchNo, string PayDate, string CourierNameId, string Location, string RaiserBranch)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetChequeDespatchAWBUpdation(plib.ConvertDate(ChqDate), PVNo, plib.ConvertDate(PayDate), CourierNameId, BatchNo, Location, RaiserBranch, "0", plib.LoginUserId);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(ds.Tables[0]); }
                    if (ds.Tables[1].Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(ds.Tables[1]); }
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
        public JsonResult SetChequeDespatchAwbUpdation(string DespatchDate, string PvNos, string CourierId, string AwbNo, string ViewType)
        {
            try
            {

                if (ViewType == "0")
                {
                    string Data1 = "", sheets = "";
                    DataSet ds = db.SetChequeDespatchAWBUpdation(plib.ConvertDate(DespatchDate), PvNos, CourierId, AwbNo, ViewType, null, plib.LoginUserId, sheets);
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(ds.Tables[0]); }
                        return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                    }
                    else { return null; }
                }
                else if (ViewType == "1")
                {
                    try
                    {
                        string Data1 = "", Data2 = "", Xml = ""; DataSet Ds;
                        string Extension1 = "";
                        if (Session["_attAWBFile"] != null)
                        {

                            //upload the file to server.
                            HttpPostedFileBase _attFile = Session["_attAWBFile"] as HttpPostedFileBase;
                            string Extension = Path.GetFileName(_attFile.FileName);
                            Extension1 = System.IO.Path.GetExtension(_attFile.FileName);
                            // DataSet dts = db.getuploaddata("1", Extension1);
                            if (Extension1 == ".xlsx" || Extension1 == ".xls" || _attFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || _attFile.ContentType == "application/vnd.ms-excel")
                            {
                                Stream stream = _attFile.InputStream;
                                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                                excelReader.IsFirstRowAsColumnNames = true;
                                Ds = excelReader.AsDataSet();
                                excelReader.Close();
                                if (Ds != null)
                                {
                                    if (Ds.Tables[0].Rows.Count > 0)
                                    {
                                        Xml = Ds.GetXml();
                                        if (Ds.Tables[0].Rows.Count > 0) Data2 = JsonConvert.SerializeObject(Ds.Tables[0]);
                                        string sheets = Ds.Tables[0].TableName.ToString();
                                        sheets = "NewDataSet" + "/" + sheets;
                                        DataSet ds2 = db.SetChequeDespatchAWBUpdation("", "", "0", "", ViewType, Xml, plib.LoginUserId, sheets);
                                        if (ds2.Tables[0].Rows[0]["Clear"].ToString() == "True" && ds2.Tables[0].Rows[0]["Message"].ToString() == "Bulk Payment Despatched Successfully!!")
                                        {
                                            for (int i = 0; Ds.Tables[0].Rows.Count > i; i++)
                                            {
                                                DataSet PVId = db.GetPVID_Chq_pouch_no(Ds.Tables[0].Rows[i]["ChqNo"].ToString(), Ds.Tables[0].Rows[i]["PvNo"].ToString());
                                                if (PVId.Tables[0].Rows[0]["Column1"].ToString() != "" && PVId.Tables[0].Rows[0]["Column1"].ToString() != null)
                                                {
                                                    string[] data = { PVId.Tables[0].Rows[0]["Column2"].ToString() };
                                                    MailController.sendusermailfs("FS", "Cheque Dispatch", PVId.Tables[0].Rows[0]["Column1"].ToString(), "S", data, "0");
                                                }
                                            }
                                        }
                                        // Session.Remove("_attAWBFile");
                                        if (ds2.Tables[0].Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(ds2.Tables[0]); }
                                        return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                else
                                {
                                    DataTable dt2 = new DataTable();
                                    dt2.Columns.Add("Message", typeof(string));
                                    dt2.Columns.Add("Clear", typeof(string));
                                    dt2.Rows.Add(new object[] { "Upload template not valid!", "false" });
                                    if (dt2.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt2); }
                                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                DataTable dt2 = new DataTable();
                                dt2.Columns.Add("Message", typeof(string));
                                dt2.Columns.Add("Clear", typeof(string));
                                dt2.Rows.Add(new object[] { "Upload only Excel file(.xlsx or .xls extension)!", "false" });
                                if (dt2.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt2); }
                                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                            }

                        }
                        else
                        {
                            DataTable dt2 = new DataTable();
                            dt2.Columns.Add("Message", typeof(string));
                            dt2.Columns.Add("Clear", typeof(string));
                            dt2.Rows.Add(new object[] { "Upload excel file", "false" });

                            if (dt2.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt2); }
                            return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorLog objErrorLog = new ErrorLog();
                        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                        return null;
                    }

                }
                return null;
            }
            catch (Exception ex)
            {
                ErrorLog objErrorLog = new ErrorLog();
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return null;
            }
        }

        //File Uploading Part
        [HttpPost]
        public void UploadAttachment(string uploadfor, string attach = null, string attaching_format = null) //Pandiaraj 18-11-2019 
        {
            try
            {
                Session["_attAWBFile"] = null;
                //foreach (string file in Request.Files)
                //{ 
                //    HttpPostedFileBase hpfBase = Request.Files[file] as HttpPostedFileBase;
                //    Session["_attAWBFile"] = hpfBase;
                //}
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    { 
                        string fileextension = Path.GetExtension(Path.GetFileName(fileContent.FileName));
                        string[] attaching_fileformat = attaching_format.Split(',');
                        if (attaching_fileformat.Contains(fileextension.ToLower()))
                        {
                            var stream = fileContent.InputStream;
                            byte[] bytFile = null;
                            using (var memoryStream = new MemoryStream())
                            {
                                stream.CopyTo(memoryStream);
                                bytFile = memoryStream.ToArray(); 
                            }
                            bool isEXE = CmnFunctions.GetMimeTypeFromAttachment(bytFile, attach, fileextension.ToLower());
                            if (isEXE == false)
                            {
                                HttpPostedFileBase hpfBase = Request.Files[file] as HttpPostedFileBase;
                                Session["_attAWBFile"] = hpfBase;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog objErrorLog = new ErrorLog();
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
        }

        [HttpPost]
        public JsonResult DownloadChequeDespatchAwbUpdation(string ChqDate, string PVNo, string BatchNo, string PayDate, string CourierNameId, string Location, string RaiserBranch)
        {
            try
            {
                DataSet ds = db.GetChequeDespatchAWBUpdation(plib.ConvertDate(ChqDate), PVNo, plib.ConvertDate(PayDate), CourierNameId, BatchNo, Location, RaiserBranch, "1", plib.LoginUserId);
                if (ds != null)
                {
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            DataTable dt = ds.Tables[1].Copy();
                            dt.Columns.Remove("PvId");
                            //dt.Columns.Remove("ChqNo");
                            dt.Columns.Remove("ChqbookNo");
                            dt.Columns.Remove("CourierId");
                            wb.Worksheets.Add(dt);
                            wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                            wb.Style.Font.Bold = true;

                            Response.Clear();
                            Response.Buffer = true;
                            Response.Charset = "";
                            Response.ContentType = "application/vnd.ms-excel";

                            Response.AddHeader("content-disposition", "attachment;filename= AWBPosting.xls");
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
                    else
                    {
                        return Json("Sorry No records to download!", JsonRequestBehavior.AllowGet);
                    }
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }
    }
}
