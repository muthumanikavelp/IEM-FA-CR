using IEM.Common;
using IEM.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.App_Start;
namespace IEM.Areas.FLEXISPEND.Controllers
{

    [NoDirectAccess]
    public class ProvisionController : Controller
    {
        #region Declaration
        dbLib db = new dbLib();
        proLib plib = new proLib();
        DataTable dt = new DataTable();
        #endregion

        //
        // GET: /FLEXISPEND/Provision/
        public ActionResult Mapping()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SetProvisionUpload()
        {
            DataTable dt = new DataTable();
            DataTable dtTemp = new DataTable();
            string AttachmentName = "", Data1 = "", SheetName = "";
            try
            {
                if (Session["_attPMFile"] != null)
                {
                    HttpPostedFileBase _attFile = Session["_attPMFile"] as HttpPostedFileBase;
                    AttachmentName = _attFile.FileName.ToString();
                }
            }
            catch
            {
                AttachmentName = "";
            }

            if (AttachmentName != "")
            {
                DataSet _dsTemp;
                try
                {
                    HttpPostedFileBase uploadedExcelFile = Session["_attPMFile"] as HttpPostedFileBase;
                    if (uploadedExcelFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || uploadedExcelFile.ContentType == "application/vnd.ms-excel")
                    {
                        var stream = uploadedExcelFile.InputStream;
                        string uploaderUrl = string.Format("{0}{1}", plib.ProvisionMappingUploadUrl, uploadedExcelFile.FileName);
                        if (System.IO.File.Exists(uploaderUrl))
                        {
                            System.IO.File.Delete(uploaderUrl);
                        }

                        //using (var fileStream = System.IO.File.Create(uploaderUrl))
                        //{
                        //    stream.CopyTo(fileStream);
                        //}

                        byte[] bytFile = null;
                        using (var memoryStream = new MemoryStream())
                        {
                            stream.CopyTo(memoryStream);
                            bytFile = memoryStream.ToArray();
                            ViewBag.Result = "inside File Stream";
                        }
                        var FileString = Convert.ToBase64String(bytFile);
                        FileServer Cmnfs = new FileServer();
                        var result = Cmnfs.SaveFileToServer(FileString, uploadedExcelFile.FileName).Result;
                        if (result == "success")
                        {
                            if (System.IO.File.Exists(uploaderUrl))
                            {
                                System.IO.File.Delete(uploaderUrl);
                            }
                        }
                        //Ramya changed the version. 12.0 is not working at HFC server
                        //var connectionstring = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + uploaderUrl + ";Extended Properties='Excel 12.0;IMEX=1;HDR=Yes'";
                        var connectionstring = "Provider=Microsoft.ACE.OLEDB.4.0;Data Source=" + uploaderUrl + ";Extended Properties='Excel 8.0;IMEX=1;HDR=Yes'";
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
                                    _dsTemp = new DataSet();
                                    adapter.Fill(_dsTemp);
                                    conn.Close();
                                    conn.Dispose();

                                    if (_dsTemp != null && _dsTemp.Tables[0].Rows.Count != 0)
                                    {
                                        SheetName = sheets.Rows[0]["TABLE_NAME"].ToString().Replace('$', ' ');

                                        DataSet ds = db.SetProvisionUpload(AttachmentName, SheetName, _dsTemp.Tables[0], plib.LoginUserId);
                                        if (ds != null)
                                        {
                                            dtTemp = ds.Tables[0];

                                            if (dtTemp.Rows[0]["Clear"].ToString().ToLower() == "true" || dtTemp.Rows[0]["Clear"].ToString().ToLower() == "1")
                                            {
                                                Session.Remove("_attPMFile");
                                            }
                                            if (dtTemp.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dtTemp); }
                                        }
                                    }
                                    else
                                    {
                                        dt.Columns.Add("Msg", typeof(string));
                                        dt.Columns.Add("Clear", typeof(string));
                                        dt.Rows.Add(new object[] { "Excel file should not be empty!", "false" });
                                        if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                                    }
                                }
                            }
                        }
                        catch
                        {
                            dt.Columns.Add("Msg", typeof(string));
                            dt.Columns.Add("Clear", typeof(string));
                            dt.Rows.Add(new object[] { "Please upload the valid file!", "false" });
                            if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                        }
                    }
                    else
                    {
                        dt.Columns.Add("Msg", typeof(string));
                        dt.Columns.Add("Clear", typeof(string));
                        dt.Rows.Add(new object[] { "Upload only Excel file(.xlsx/ .xls extension)!", "false" });

                        if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                    }
                }
                //catch the error exception at the time of file uploading.
                catch { return null; }
            }
            else
            {
                dt.Columns.Add("Msg", typeof(string));
                dt.Columns.Add("Clear", typeof(string));
                dt.Rows.Add(new object[] { "Please upload the valid file!", "false" });
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

            }
            return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
        }

        //File Uploading Part
        [HttpPost]
        public void UploadAttachment(string uploadfor, string attach = null, string attaching_format = null) //Pandiaraj 18-11-2019 
        {
            try
            {
                Session["_attPMFile"] = null;

                //foreach (string file in Request.Files)
                //{
                //    HttpPostedFileBase hpfBase = Request.Files[file] as HttpPostedFileBase;
                //    Session["_attPMFile"] = hpfBase;
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
                                Session["_attPMFile"] = hpfBase;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        [HttpPost]
        public JsonResult SearchProvisionMapping(string PDateFrom, string PDateTo, string PAmount, string CCCode, string FCCode, string FC, string CC, string ProvisionBy)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.FetchProvisionMapping(plib.ConvertDate(PDateFrom), plib.ConvertDate(PDateTo), PAmount == null || PAmount == "" ? "0" : PAmount,
                    CCCode, FCCode, FC, CC, ProvisionBy, plib.LoginUserId);
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

        // GET: /FLEXISPEND/Provision/
        public ActionResult InvoiceMapping()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetProvisionMapping(string InvFrom, string InvTo, string InvNo, string InvAmt,
            string ECFFrom, string ECFTo, string ECFNo, string ECFAmt, string SCode, string SName)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetProvisionMapping(plib.ConvertDate(InvFrom), plib.ConvertDate(InvTo), InvNo, InvAmt == null || InvAmt == "" ? "0" : InvAmt,
                    plib.ConvertDate(ECFFrom), plib.ConvertDate(ECFTo), ECFNo, ECFAmt == null || ECFAmt == "" ? "0" : ECFAmt, SCode == "0" ? "" : SCode, SName == "0" ? "" : SName, plib.LoginUserId);
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
        public JsonResult SearchProvisionDetails(string Month, string Amount, string FC, string CC)
        {
            try
            {
                string strMonth = "0";
                string strYear = "0";
                if (Month != "")
                {
                    string[] strSplitMonth = Month.Split('-');
                    strMonth = plib.GetMonth(strSplitMonth[0]);
                    strYear = strSplitMonth[1];
                }

                string Data1 = "", Data2 = "";
                DataSet ds = db.SearchProvisionDetails(strMonth, strYear, Amount == null || Amount == "" ? "0" : Amount, FC, CC, plib.LoginUserId);
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
        public JsonResult SetProvisionMapping(string provisiongid, string Invoicegid, string MAmt, string MDesc)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.SetProvisionMapping(provisiongid, Invoicegid, MAmt == null || MAmt == "" ? "0" : MAmt, MDesc, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        // GET: /FLEXISPEND/Provision/
        public ActionResult InvoiceUnMapping()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetProvisionUnMapping(string InvFrom, string InvTo, string InvNo, string InvAmt,
            string ECFFrom, string ECFTo, string ECFNo, string ECFAmt, string SCode, string SName)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "";
                DataSet ds = db.GetProvisionUnMapping(plib.ConvertDate(InvFrom), plib.ConvertDate(InvTo), InvNo, InvAmt == null || InvAmt == "" ? "0" : InvAmt,
                    plib.ConvertDate(ECFFrom), plib.ConvertDate(ECFTo), ECFNo, ECFAmt == null || ECFAmt == "" ? "0" : ECFAmt, SCode, SName, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[2];
                    if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1, Data2, Data3 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SetProvisionUnMapping(string provisionmapgid, string Invoicegid, string MDesc)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.SetProvisionUnMapping(provisionmapgid, Invoicegid, MDesc, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
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
