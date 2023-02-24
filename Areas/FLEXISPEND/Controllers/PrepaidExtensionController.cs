using IEM.Common;
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
    public class PrepaidExtensionController : Controller
    {
        #region Declaration
        dbLib db = new dbLib();
        proLib plib = new proLib();
        DataTable dt = new DataTable();
        #endregion

        //
        // GET: /FLEXISPEND/PrepaidExtension/
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public JsonResult FetchPrepaidExtension(string ARFDateFrom, string ARFDateTo, string ARFNo, string ARFAmount, string LiquidDateFrom, string LiquidDateTo, string Type, string PayMode, string EmpCodeId,
        string EmpNameId, string SuppCodeId, string SuppNameId, string RaiserCodeId, string RaiserNameId)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetPrepaidExtension(ARFDateFrom, ARFDateTo, ARFNo, ARFAmount == "" ? "0" : ARFAmount, LiquidDateFrom, LiquidDateTo, Type, PayMode, EmpCodeId,  
                                                        EmpNameId, SuppCodeId, SuppNameId, RaiserCodeId, RaiserNameId, plib.LoginUserId);
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
        public JsonResult SetPrepaidExtension(string ECFARFGId, string OldLiquidationDate, string NewLiquidationDate, string Remarks)
        {
            try
            {
                string Data1 = "", AttachmentName = "";

                if (Session["_attPPEFile"] != null)
                {
                    HttpPostedFileBase _attFile = Session["_attPPEFile"] as HttpPostedFileBase;
                    AttachmentName = _attFile.FileName.ToString();
                }

                DataSet ds = db.SetPrepaidExtension(ECFARFGId, plib.ConvertDate(OldLiquidationDate), plib.ConvertDate(NewLiquidationDate), Remarks, AttachmentName, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (AttachmentName != "" && (dt.Rows[0]["Clear"].ToString().ToLower() != "true" || dt.Rows[0]["Clear"].ToString().ToLower() != "1"))
                    {
                        try
                        {
                            //upload the file to server.
                            HttpPostedFileBase _attFile = Session["_attPPEFile"] as HttpPostedFileBase;
                            var stream = _attFile.InputStream;
                            string uploaderUrl = string.Format("{0}{1}", plib.PrepaidExtensionUrl, _attFile.FileName);
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
                            var result = Cmnfs.SaveFileToServer(FileString, _attFile.FileName).Result;
                            if (result == "success")
                            {
                                if (System.IO.File.Exists(uploaderUrl))
                                {
                                    System.IO.File.Delete(uploaderUrl);
                                }
                            }

                            //remove the temp data content.
                            Session.Remove("_attPPEFile");
                        }
                        //catch the error exception at the time of file uploading.
                        catch { }
                    }

                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
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

        //File Uploading Part
        [HttpPost]
        public void UploadAttachment(string uploadfor, string attach = null, string attaching_format = null) //Pandiaraj 18-11-2019 
        {
            try
            {
                Session["_attPPEFile"] = null;

                //foreach (string file in Request.Files)
                //{
                //    HttpPostedFileBase hpfBase = Request.Files[file] as HttpPostedFileBase;
                //    Session["_attPPEFile"] = hpfBase;
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
                                Session["_attPPEFile"] = hpfBase;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

    }
}
