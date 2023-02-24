using IEM.Areas.Dashboard.Models;
using IEM.Areas.FLEXIBUY.Models;
using IEM.Areas.MASTERS.Controllers;
using IEM.Common;
using IEM.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IEM.Helper;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class PARNewController : Controller
    {
        CmnFunctions objCmnFunctions = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        proLib plib = new proLib();
        dbLib db = new dbLib();
        private PARRepository objModel;
        private IrepositoryAn objRep;
        private IRepositoryKIR objparModel;
        public PARNewController()
            : this(new PARDataModel(),new prsummodel())
        {

        }
        public PARNewController(PARRepository objM, IRepositoryKIR objpr)
        {
            objModel = objM;
            objparModel = objpr;
        }
        // GET: /FLEXIBUY/PARNew/

        public ActionResult Index(string pargid = null, string viewtype = null)
        {
            ViewBag.isapproval = "";
            ViewBag.isapprovalMode = "0";
            ViewBag.deleteflag = "0";
            ViewBag.pargid = "0";
            ViewBag.viewtype = "0";
            if (pargid != null)
            {
                ViewBag.pargid = pargid;
            }
            else if(pargid==null && viewtype==null)
            {
                ViewBag.pargid = "0";
            }
            else
            {
                if (Session["PARHeaderGid"] != null)
                    ViewBag.pargid = (Int32)Session["PARHeaderGid"];
                else
                    ViewBag.pargid = "0";
            }
            if (viewtype == "view")
            {
                ViewBag.viewtype = "1";
            }
            else if (viewtype == "checker")
            {
                ViewBag.viewtype = "1";
                ViewBag.isapprovalMode = "1";
                ViewBag.isapproval = viewtype;
            }
            else if (viewtype == "delete")
            {
                ViewBag.viewtype = "1";
                ViewBag.deleteflag = "1";
            }
            else
            {
                ViewBag.viewtype = "0";
            }
            return View();
        }


        [HttpPost]
        public JsonResult GetPARHeaderDetails()
        {
            string data0 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objModel.GetPARHeaderDetails();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult GetRequestFor()
        {
            string data0 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objModel.GetRequestFor();
                dt = ds.Tables[0];
                DataRow row = dt.NewRow();
                dt.Rows.InsertAt(row, 0);
                dt.Rows[0][0] = 0;
                dt.Rows[0][1] = "-- Select One --";
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetPeriodList()
        {
            string data0 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objModel.GetPeriodList();
                dt = ds.Tables[0];
                DataRow row = dt.NewRow();
                dt.Rows.InsertAt(row, 0);
                dt.Rows[0][0] = 0;
                dt.Rows[0][1] = "-- Select One --";
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetPARHeaderDetailsAll(string pargid = "") 
        {
            string data0 = string.Empty;
            string data1 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "0";
                int result = Convert.ToInt32(string.IsNullOrEmpty(pargid) ? "0" : pargid);
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objModel.GetPARDetailsAll(result);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                return Json(new { data0, data1, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, data1, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AddPARParentDetailsList(PAREntity PARParentDetailsInsert)
        {
            string data0 = string.Empty;
            string data1 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "1";
                int result = objModel.AddPARParentDetailsList(PARParentDetailsInsert);
                if (result == 0)
                {
                    ErrorMsg = "1";
                }
                else
                {
                    Session["PARHeaderGid"] = result;
                    ErrorMsg = "0";
                    DataSet ds = new DataSet();
                    DataTable dt;
                    ds = objModel.GetPARDetailsAll(result); 
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                }

                return Json(new { data0, data1, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, data1, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AddPARParentDetailsListNew(PAREntity PARParentDetailsInsert)  
        {
            string data0 = string.Empty;
            string data1 = string.Empty;
            string data2 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "1";
                int result = objModel.AddPARParentDetailsListNew(PARParentDetailsInsert);
                if (result == 0)
                {
                    ErrorMsg = "1";
                }
                else
                {
                    ErrorMsg = "0";
                    DataSet ds = new DataSet();
                    DataTable dt;
                    ds = objModel.GetPARDetailsAll(result);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                }

                return Json(new { data0, data1, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, data1, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetPARDetailParent(string pardetailgid = "") 
        {
            string data0 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "0";
                int result = Convert.ToInt32(string.IsNullOrEmpty(pardetailgid) ? "0" : pardetailgid);
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objModel.GetPARDetailParent(result);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult UpdatePARParentList(PAREntity PARParentDetailsInsert)
        {
            string data0 = string.Empty;
            string data1 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "1";
                int result = objModel.UpdatePARParentList(PARParentDetailsInsert);
                if (result == 0)
                {
                    ErrorMsg = "1";
                }
                else
                {
                    ErrorMsg = "0";
                    DataSet ds = new DataSet();
                    DataTable dt;
                    ds = objModel.GetPARDetailsAll(result);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                }

                return Json(new { data0, data1, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, data1, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AddPARApprover(PAREntity PARParentDetailsInsert)
        {
            string data0 = string.Empty;
            string finalapprdate = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "1";
                int result = objModel.AddPARApprover(PARParentDetailsInsert);
                if (result == 0)
                {
                    ErrorMsg = "1";
                }
                else
                {
                    ErrorMsg = "0";
                    DataSet ds = new DataSet();
                    DataTable dt;
                    ds = objModel.GetPARApprover(result); 
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { finalapprdate = dt.Rows[0][0].ToString(); }
                }

                return Json(new { data0,finalapprdate, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, finalapprdate, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetPARApprover(string pargid = "")
        {
            string data0 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "0";
                int result = Convert.ToInt32(string.IsNullOrEmpty(pargid) ? "0" : pargid);
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objModel.GetPARApprover(result);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); 
                }

                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public void UploadedFiles(string uploadfor, string fname = null, string attach = null, string attaching_format = null) //Ramya 18-11-2019 
        {
            try
            {
                Session["filenamePAR"] = null;
                Session["uploadfor"] = null;
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
                                Session["filenamePAR"] = hpfBase;
                                Session["uploadfor"] = uploadfor;
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
        public void UploadedFilesold(string uploadfor, string fname = null)
        {
            try
            {
                Session["filenamePAR"] = null;
                string filename = "";

                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];

                    if (fileContent != null && fileContent.ContentLength > 0)
                    {

                        if (fname != null && fname.Trim() != "")
                        {
                            filename = fname.Substring(0, fname.LastIndexOf(".") + 0);
                        }
                        else
                        {
                            if (uploadfor != null)
                            {
                                filename = objCmnFunctions.GetSequenceNo(uploadfor);
                            }
                        }
                        var fileextension = Path.GetExtension(fileContent.FileName);
                        var stream = fileContent.InputStream;
                        var CmnFilePath = objCmnFunctions.IEMAttachmentPath();
                        // var path = Path.Combine(@"C:\temp\", filename + fileextension);
                        var path = Path.Combine(CmnFilePath, filename + fileextension);
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                        filename = filename + fileextension;
                        Session["filenamePAR"] = filename;
                        byte[] bytFile = System.IO.File.ReadAllBytes(path);
                        string Filestring = Convert.ToBase64String(bytFile);
                        FileServer objserver = new FileServer();
                        string result = objserver.SaveFileToServer(Filestring, filename).Result;
                        if (result == "success")
                        {
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                        }
                    }
                }
                //  return Json(filename);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                //  return Json("Upload failed");
            }
        }
        [HttpPost]
        public JsonResult CreatePARAttachment(Attachments PARParentDetailsInsert)
        {
            string data0 = string.Empty;
            string ErrorMsg = string.Empty;
            string filename = "";
            try
            {
                ErrorMsg = "1";
                if (Session["filenamePAR"] != null && Session["uploadfor"] != null)
                {
                    filename = objCmnFunctions.GetSequenceNo(Session["uploadfor"].ToString());

                    HttpPostedFileBase _attFile = Session["filenamePAR"] as HttpPostedFileBase;
                    var fileextension = "." + _attFile.FileName.Split('.')[_attFile.FileName.Split('.').Length - 1];
                    var stream = _attFile.InputStream;
                    var CmnFilePath = objCmnFunctions.IEMAttachmentPath();
                    var path = Path.Combine(CmnFilePath, filename + fileextension);
                    using (var fileStream = System.IO.File.Create(path))
                    {
                        stream.CopyTo(fileStream);
                    }
                    filename = filename + fileextension;
                    //Session["filenamePR"] = filename;
                    byte[] bytFile = System.IO.File.ReadAllBytes(path);
                    string Filestring = Convert.ToBase64String(bytFile);
                    FileServer objserver = new FileServer();
                    string result = objserver.SaveFileToServer(Filestring, filename).Result;
                    if (result == "Success")
                    {
                        PARParentDetailsInsert.TempFileName = filename;
                        int data = objModel.CreatePARAttachment(PARParentDetailsInsert);
                        if (data > 0)
                        {
                            ErrorMsg = "0";
                            DataSet ds = new DataSet();
                            DataTable dt;
                            ds = objModel.GetPARAttachments(PARParentDetailsInsert.AttachmentFor, PARParentDetailsInsert.AttachmentRefGid);
                            dt = ds.Tables[0];
                            if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                        }
                    }
                }
                else
                {
                    ErrorMsg = "2";
                }
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult CreatePARAttachmentold(Attachments PARParentDetailsInsert) 
        {
            string data0 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "1";
                if (Session["filenamePAR"] != null)
                {
                    PARParentDetailsInsert.TempFileName = (string)Session["filenamePAR"];
                    int result = objModel.CreatePARAttachment(PARParentDetailsInsert);
                    if (result > 0)
                    {
                        ErrorMsg = "0";
                        DataSet ds = new DataSet();
                        DataTable dt;
                        ds = objModel.GetPARAttachments(PARParentDetailsInsert.AttachmentFor, PARParentDetailsInsert.AttachmentRefGid);
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                    }

                }
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetAttachment(Attachments PARParentDetailsInsert)
        {
            string data0 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "1";
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objModel.GetPARAttachments(PARParentDetailsInsert.AttachmentFor, PARParentDetailsInsert.AttachmentRefGid);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                ErrorMsg = "0";
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult DeletePARParentDetails(PAREntity PARParentDetailsInsert)
        {
            string data0 = string.Empty;
            string data1 = string.Empty;
            string data2 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "1";
                int itemgid = Convert.ToInt32(string.IsNullOrEmpty(PARParentDetailsInsert.PARDetailGid.ToString()) ? "0" : PARParentDetailsInsert.PARDetailGid.ToString());
                int result = objModel.DeletePARParentDetails(itemgid, string.IsNullOrEmpty(PARParentDetailsInsert.DeleteFor) ? "" : PARParentDetailsInsert.DeleteFor);
                if (result == 0)
                {
                    ErrorMsg = "1";
                }
                else
                {
                    ErrorMsg = "0";
                    if (PARParentDetailsInsert.DeleteFor != "parent" && PARParentDetailsInsert.DeleteFor != "po")
                    {
                        DataSet ds = new DataSet();
                        DataTable dt;
                        ds = objModel.GetPARDetailsAll(result);
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                        dt = ds.Tables[1];
                        if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                    }
                }

                return Json(new { data0, data1, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, data1, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult DeletePARAttachment(Attachments PARParentDetailsInsert)
        {
            string data0 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "1";
                int result = objModel.DeletePARAttachment(PARParentDetailsInsert);
                if (result > 0)
                {
                    ErrorMsg = "0";
                    DataSet ds = new DataSet();
                    DataTable dt;
                    ds = objModel.GetPARAttachments(PARParentDetailsInsert.AttachmentFor, PARParentDetailsInsert.AttachmentRefGid);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult SaveAsDraft(PAREntity PARParentDetailsInsert)
        {
            string ErrorMsg = string.Empty;
            string data = string.Empty;
            try
            {
                ErrorMsg = "1";
                string skipflag = "0";
                if (PARParentDetailsInsert.DeleteFor == "submit")
                {
                    int checkapprover = objModel.CheckFinalApprover(Convert.ToInt32(string.IsNullOrEmpty(PARParentDetailsInsert.PARGid.ToString()) ? "0" : PARParentDetailsInsert.PARGid.ToString()));
                    if (checkapprover == 404)
                    {
                        skipflag = "1";
                        ErrorMsg = "404";
                    }
                    else if (checkapprover == 0)
                    {
                        skipflag = "1";
                        ErrorMsg = "1";
                    }
                }
                if (skipflag == "0")
                {
                    int result = objModel.SaveAsDraft(PARParentDetailsInsert);
                    if (result == 0)
                    {
                        ErrorMsg = "1";
                    }
                    else
                    {
                        if (PARParentDetailsInsert.DeleteFor == "submit")
                        {
                            ForMailController mailsender = new ForMailController();
                            CbfSumModel objMail = new CbfSumModel();
                            int refgid = objMail.GetRefGidForMail("PAR");
                            string reqstatus = objMail.GetRequestStatus(refgid, "PAR");
                            int queuegid = objMail.GetQueueGidForMail(refgid, "PAR");
                            mailsender.sendusermail("FB", "PAR", Convert.ToString(queuegid), "S", "0");
                            ErrorMsg = "0";
                            Session["PARHeaderGid"] = null;
                        }
                        else
                        {
                            ErrorMsg = "0";
                            Session["PARHeaderGid"] = null;
                        }
                    }
                }

                return Json(new { ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult SubmitApprover(PAREntity PARParentDetailsInsert)
        {
            string ErrorMsg = string.Empty;
            string result1 = string.Empty;
            try
            {
                string Data1 = "0";
                string ref_name = "PAR";
                DataSet ds = new DataSet();
                DataTable dt1 = new DataTable();
                ErrorMsg = "1";
                 DataTable dt = new DataTable();
                 DataSet ds1 = db.CheckAuthorize(Convert.ToString(PARParentDetailsInsert.PARGid), Convert.ToString(plib.LoginUserId), "PAR");

                dt = ds1.Tables[0];

                if (dt.Rows.Count > 0)
                {
                    if (Convert.ToString(dt.Rows[0]["Result"]) == "Y")
                    {
                        result1 = Convert.ToString(dt.Rows[0]["Result"]);
                    }
                    else
                    {

                        result1 = JsonConvert.SerializeObject(dt);
                    }
                }
                else
                {

                    result1 = JsonConvert.SerializeObject(dt);
                }

                if (result1 == "Y")
                {
                    string result = objModel.SubmitApprover(PARParentDetailsInsert);

                    ds = objparModel.Getapprovaldetails(ref_name); 
                    dt1 = ds.Tables[0];
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Data1 = JsonConvert.SerializeObject(dt1);
                    }

                    if (result.ToUpper() != "SUCCESS")
                    {
                        ErrorMsg = result;
                    }
                    else
                    {
                        ForMailController mailsender = new ForMailController();
                        CbfSumModel objMail = new CbfSumModel();
                        int refgid = objMail.GetRefGidForMail("PAR");
                        string reqstatus = objMail.GetRequestStatus(PARParentDetailsInsert.PARGid, "PAR");
                        int queuegid = objMail.GetQueueGidForMail(PARParentDetailsInsert.PARGid, "PAR");
                        mailsender.sendusermail("FB", "PAR", Convert.ToString(queuegid), reqstatus, "0");
                        ErrorMsg = "0";
                    }
                }
                else
                {
                    ErrorMsg = "2";
                }
                return Json(new { ErrorMsg, Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult RejectApprover(PAREntity PARParentDetailsInsert)
        {
            string ErrorMsg = string.Empty;
            try
            {
                string Data1 = "0";
                string ref_name = "PAR";
                DataSet ds = new DataSet();
                DataTable dt1 = new DataTable();
                ErrorMsg = "1";
                int result = objModel.RejectApprover(PARParentDetailsInsert);

                ds = objparModel.Getapprovaldetails(ref_name); 
                dt1 = ds.Tables[0];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Data1 = JsonConvert.SerializeObject(dt1);
                }

                if (result > 0)
                {
                    ForMailController mailsender = new ForMailController();
                    CbfSumModel objMail = new CbfSumModel();
                    int refgid = objMail.GetRefGidForMail("PAR");
                    string reqstatus = objMail.GetRequestStatus(PARParentDetailsInsert.PARGid, "PAR");
                    int queuegid = objMail.GetQueueGidForMail(PARParentDetailsInsert.PARGid, "PAR");
                    mailsender.sendusermail("FB", "PAR", Convert.ToString(queuegid), reqstatus, "0");
                    ErrorMsg = "0";
                }

                return Json(new { ErrorMsg, Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
