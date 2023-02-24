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
using IEM.Areas.MASTERS.Controllers;
using IEM.Common;
using System.IO;
using IEM.App_Start;
namespace IEM.Areas.FLEXISPEND.Controllers
{

    [NoDirectAccess]
    public class ChequeStatusUpdateController : Controller
    {

        #region Declaration
        proLib pl = new proLib();
        dbLib db = new dbLib();
        DataTable dt;
        ForMailController MailController = new ForMailController();
        #endregion
        //
        // GET: /FLEXISPEND/ChequeStatusUpdate/
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult GetChequeStatusUpdate(string ChqNo, string PvNo)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetChequeStatusUpdate(ChqNo, PvNo, pl.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    if (ds.Tables.Count > 1)
                    {
                        dt = ds.Tables[1];
                        if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                    }

                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                }
                return null;
            }
            catch
            {
                return null;
            }
        }


        [HttpPost]
        public JsonResult SetChequeStatusUpdate(string ChqNo, string PvNo, string ChqStatus, string Date, string Reason,
            string ViewType)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.SetChequeStatusUpdate(ChqNo, PvNo, ChqStatus, pl.ConvertDate(Date), Reason, ViewType, "", pl.LoginUserId);
                if (ChqStatus == "Cleared" && ds.Tables[0].Rows[0]["Clear"].ToString() == "True")
                {
                    //DataSet PVId = db.GetPVID_Chq_pouch_no(ChqNo, PvNo);
                    //if(PVId.Tables[0].Rows[0]["Column1"].ToString() != "" && PVId.Tables[0].Rows[0]["Column1"].ToString() != null)
                    //{
                    //    string[] data = { PVId.Tables[0].Rows[0]["Column2"].ToString() };
                    //    MailController.sendusermailfs("FS", "Cheque Dispatch", PVId.Tables[0].Rows[0]["Column1"].ToString(), "S", data, "0");
                    //}
                }
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //File Uploading Part
        [HttpPost]
        public void UploadAttachment(string uploadfor, string attach = null, string attaching_format = null) //Pandiaraj 18-11-2019
        {
            try
            {
                Session["_attCSUFile"] = null;

                //foreach (string file in Request.Files)
                //{
                //    HttpPostedFileBase hpfBase = Request.Files[file] as HttpPostedFileBase;
                //    Session["_attCSUFile"] = hpfBase;
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
                                Session["_attCSUFile"] = hpfBase;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }


        ///Upload
        public JsonResult SetChqStatusUpdateExcel(string ChqNo, string PvNo, string ChqStatus, string Date, string Reason, string ViewType)
        {
            try
            {
                string Data1 = "", Data2 = "", Xml = ""; DataSet Ds;
                if (Session["_attCSUFile"] != null)
                {
                    HttpPostedFileBase _attFile = Session["_attCSUFile"] as HttpPostedFileBase;
                    string[] y = _attFile.FileName.Split('.');

                    //if (_attFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || _attFile.ContentType == "application/vnd.ms-excel")
                    if (y[y.Length - 1] == "xlsx" || y[y.Length - 1] == "xls" || _attFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || _attFile.ContentType == "application/vnd.ms-excel")
                    {
                        var stream = _attFile.InputStream;
                        string uploaderUrl = string.Format("{0}{1}", pl.ChqStatusUpdateFileUploadUrl, _attFile.FileName.Split('\\')[_attFile.FileName.Split('\\').Length - 1]);

                        //using (var fileStream = System.IO.File.Create(uploaderUrl))
                        //  //  stream.CopyTo(fileStream);
                        byte[] bytFile = null;
                        using (var memoryStream = new MemoryStream())
                        {
                            stream.CopyTo(memoryStream);
                            bytFile = memoryStream.ToArray();
                            ViewBag.Result = "inside File Stream";
                        }
                        var FileString = Convert.ToBase64String(bytFile);
                        FileServer Cmnfs = new FileServer();
                        var result = Cmnfs.SaveFileToServer(FileString, _attFile.FileName.Split('\\')[_attFile.FileName.Split('\\').Length - 1]).Result;
                        if (result == "success")
                        {
                            if (System.IO.File.Exists(uploaderUrl))
                            {
                                System.IO.File.Delete(uploaderUrl);
                            }
                        }
                        
                        Session.Remove("_attCSUFile");

                        var connectionstring = "";
                        if (y[y.Length - 1] == "xlsx" || y[y.Length - 1] == "xls" || _attFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                            connectionstring = "Provider=Microsoft.ACE.OLEDB.4.0;Data Source=" + uploaderUrl + ";Extended Properties='Excel 8.0;IMEX=1;HDR=Yes'";
                        else
                            connectionstring = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + uploaderUrl + ";Extended Properties='Excel 8.0;IMEX=1;HDR=Yes'";

                        try
                        {
                            using (var conn = new OleDbConnection(connectionstring))
                            {
                                conn.Open();
                                var sheets = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
                                using (var cmd = conn.CreateCommand())
                                {
                                    cmd.CommandText = "SELECT * FROM [" + sheets.Rows[0]["TABLE_NAME"].ToString() + "] ";
                                    var adapter = new OleDbDataAdapter(cmd);
                                    Ds = new DataSet();
                                    adapter.Fill(Ds);
                                    conn.Close();
                                    conn.Dispose();
                                    Xml = Ds.GetXml();
                                    if (Ds.Tables[0].Rows.Count > 0) Data2 = JsonConvert.SerializeObject(Ds.Tables[0]);
                                    //return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                                    DataSet ds2 = db.SetChequeStatusUpdate(ChqNo, PvNo, ChqStatus, Date, Reason, ViewType, Xml, pl.LoginUserId);
                                    if(ds2.Tables[0].Rows[0]["Clear"].ToString() == "True")
                                    {
                                        for(int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                                        {
                                            if(Ds.Tables[i].Rows[0]["Status"].ToString() == "Cleared")
                                            {
                                                DataSet PVId = db.GetPVID_Chq_pouch_no(Ds.Tables[i].Rows[0]["ChequeNo"].ToString(), Ds.Tables[i].Rows[0]["PVNo"].ToString());
                                                if (PVId.Tables[0].Rows[0]["Column1"].ToString() != "" && PVId.Tables[0].Rows[0]["Column1"].ToString() != null)
                                                {
                                                    string[] data = { PVId.Tables[0].Rows[0]["Column2"].ToString() };
                                                    MailController.sendusermailfs("FS", "Cheque Dispatch", PVId.Tables[0].Rows[0]["Column1"].ToString(), "S", data, "0");
                                                }
                                            }
                                        }
                                    }
                                    dt = ds2.Tables[0];
                                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }
                        catch
                        {
                            DataTable dt2 = new DataTable();
                            dt2.Columns.Add("Message", typeof(string));
                            dt2.Columns.Add("Clear", typeof(string));
                            dt2.Rows.Add(new object[] { "Please upload the valid file!", "false" });
                            if (dt2.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt2); }
                            return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        DataTable dt2 = new DataTable();
                        dt2.Columns.Add("Message", typeof(string));
                        dt2.Columns.Add("Clear", typeof(string));
                        dt2.Rows.Add(new object[] { "Upload only Excel file(.xlsx/ .xls extension)!", "false" });

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
            catch
            {
                return null;
            }
        }
    }
}
