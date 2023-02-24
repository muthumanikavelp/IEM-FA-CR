using IEM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.FLEXIBUY.Models;
using System.Data;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using IEM.Areas.MASTERS.Controllers;
using IEM.Areas.Dashboard.Models;
using IEM.Helper;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class PRNewController : Controller
    {
        CmnFunctions objCmnFunctions = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        proLib plib = new proLib();
        dbLib db = new dbLib();
        private PRNewRepository objModel;
        private IRepositoryKIR objprModel;
        public PRNewController()
            : this(new PRNewDataModel(),new prsummodel())
        {

        }
        public PRNewController(PRNewRepository objM, IRepositoryKIR objpr)
        {
            objModel = objM;
            objprModel = objpr;
        }
        //
        // GET: /FLEXIBUY/PRNew/

        public ActionResult Index(string prgid = null, string viewtype = null)
        {
            Session["filenamePR"] = null;
            ViewBag.isapproval = "";
            ViewBag.isapprovalMode = "0";
            ViewBag.deleteflag = "0";
            ViewBag.PIPInputs = "0";

            if (prgid != null){
                ViewBag.prgid = prgid;
            }
            else
            {
                if(Session["PRHeaderGid"] !=null)
                    ViewBag.prgid = (Int32)Session["PRHeaderGid"];
                else
                    ViewBag.prgid = "0";
            }
               
            if (viewtype == "view")
            {
                ViewBag.viewtype = "1";
            }
            else if (viewtype == "delmat" || viewtype == "supervisor")
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
            else if (viewtype == "pipinputs")
            {
                ViewBag.PIPInputs = "1";
                ViewBag.viewtype = "2";
                ViewBag.isapprovalMode = "1";
                ViewBag.isapproval = viewtype;
            }
            else
            {
                ViewBag.viewtype = "0";
            }
          
            //if (Session["PRHeaderGid"] != null)
            //{
            //    ViewBag.prgid = Convert.ToString((int)Session["PRHeaderGid"]);
            //}
            return View();
        }
        [HttpPost]
        public JsonResult GetPRHeaderDetails() 
        {
            string data0 = string.Empty;
            string data1 = string.Empty;
            string data2 = string.Empty; 
            string ErrorMsg = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objModel.GetPRHeaderDetails();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                dt = ds.Tables[1];
                DataRow row = dt.NewRow();
                dt.Rows.InsertAt(row, 0);
                dt.Rows[0][0] = 0;
                dt.Rows[0][1] = "-- Select One --";
                if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                dt = ds.Tables[2];
                DataRow row1 = dt.NewRow();
                dt.Rows.InsertAt(row1, 0);
                dt.Rows[0][0] = 0;
                dt.Rows[0][1] = "-- Select One --";
                if (dt.Rows.Count > 0) { data2 = JsonConvert.SerializeObject(dt); } 
                return Json(new { data0, data1, data2, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, data1, data2, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetPRHeaderDetailsAll(string prgid = "")
        {
            string data0 = string.Empty;
            string data1 = string.Empty;
            string data2 = string.Empty;
            string data3 = string.Empty; 
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "0";
                int result = Convert.ToInt32(string.IsNullOrEmpty(prgid) ? "0" : prgid);
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objModel.GetPRDetailsAll(result);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                dt = ds.Tables[2];
                DataRow row = dt.NewRow();
                dt.Rows.InsertAt(row, 0);
                dt.Rows[0][0] = 0;
                dt.Rows[0][1] = "-- Select One --";
                if (dt.Rows.Count > 0) { data2 = JsonConvert.SerializeObject(dt); }
                dt = ds.Tables[3];
                DataRow row1 = dt.NewRow();
                dt.Rows.InsertAt(row1, 0);
                dt.Rows[0][0] = 0;
                dt.Rows[0][1] = "-- Select One --";
                if (dt.Rows.Count > 0) { data3 = JsonConvert.SerializeObject(dt); } 
                return Json(new { data0, data1, data2, data3, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, data1, data2, data3, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetUOM() 
        {
            string data0 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objModel.GetUOM(); 
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
        public JsonResult AddPRParentDetailsList(PRNewEntity PRParentDetailsInsert)
        {
            string data0 = string.Empty;
            string data1 = string.Empty;
            string data2 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "1";
                int result = objModel.AddPRParentDetailsList(PRParentDetailsInsert);
                if (result == 0)
                {
                    ErrorMsg = "1";
                }
                else
                {
                    Session["PRHeaderGid"] = result;
                    ErrorMsg = "0";
                    DataSet ds = new DataSet();
                    DataTable dt;
                    ds = objModel.GetPRDetailsAll(result);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[2];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = 0;
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { data2 = JsonConvert.SerializeObject(dt); }
                }

                return Json(new { data0, data1, data2, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, data1, data2,  ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetPRDetailParent(string prdetailgid = "")
        {
            string data0 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                int prdetgid = Convert.ToInt32(string.IsNullOrEmpty(prdetailgid) ? "0" : prdetailgid);
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objModel.GetPRDetailParent(prdetgid);
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
        public JsonResult AddPRParentDetailsListNew(PRNewEntity PRParentDetailsInsert) 
        {
            string data0 = string.Empty;
            string data1 = string.Empty;
            string data2 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "1";
                int result = objModel.AddPRParentDetailsListNew(PRParentDetailsInsert); 
                if (result == 0)
                {
                    ErrorMsg = "1";
                }
                else
                {
                    ErrorMsg = "0";
                    DataSet ds = new DataSet();
                    DataTable dt;
                    ds = objModel.GetPRDetailsAll(result);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[2];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = 0;
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { data2 = JsonConvert.SerializeObject(dt); }
                }

                return Json(new { data0, data1, data2, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, data1, data2, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult UpdatePRParentList(PRNewEntity PRParentDetailsInsert)
        {
            string data0 = string.Empty;
            string data1 = string.Empty;
            string data2 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "1";
                int result = objModel.UpdatePRParentList(PRParentDetailsInsert);
                if (result == 0) 
                {
                    ErrorMsg = "1";
                }
                else
                {
                    ErrorMsg = "0";
                    DataSet ds = new DataSet();
                    DataTable dt;
                    ds = objModel.GetPRDetailsAll(result);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[2];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = 0;
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { data2 = JsonConvert.SerializeObject(dt); }
                }

                return Json(new { data0, data1, data2, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, data1, data2, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult DeletePRParentDetails(PRNewEntity PRParentDetailsInsert) 
        {
            string data0 = string.Empty;
            string data1 = string.Empty;
            string data2 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "1";
                int itemgid = Convert.ToInt32(string.IsNullOrEmpty(PRParentDetailsInsert.PRDetailGid.ToString()) ? "0" : PRParentDetailsInsert.PRDetailGid.ToString());
                int result = objModel.DeletePRParentDetails(itemgid, string.IsNullOrEmpty(PRParentDetailsInsert.RequestForName) ? "" : PRParentDetailsInsert.RequestForName);
                if (result == 0)
                {
                    ErrorMsg = "1";
                }
                else
                {
                    ErrorMsg = "0";
                    if (PRParentDetailsInsert.RequestForName == "child")
                    {
                        DataSet ds = new DataSet();
                        DataTable dt;
                        ds = objModel.GetPRDetailsAll(result);
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                        dt = ds.Tables[1];
                        if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                        dt = ds.Tables[2];
                        DataRow row = dt.NewRow();
                        dt.Rows.InsertAt(row, 0);
                        dt.Rows[0][0] = 0;
                        dt.Rows[0][1] = "-- Select One --";
                        if (dt.Rows.Count > 0) { data2 = JsonConvert.SerializeObject(dt); }
                    }
                }

                return Json(new { data0, data1, data2, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, data1, data2, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult SaveAsDraft(PRNewEntity PRParentDetailsInsert)
        {
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "1";
                int result = objModel.SaveAsDraft(PRParentDetailsInsert);
                if (result == 0)
                {
                    ErrorMsg = "1";
                }
                else
                {
                    ErrorMsg = "0";
                    Session["PRHeaderGid"] = null;
                }
                if (PRParentDetailsInsert.RequestForName == "submit")
                {
                    ForMailController mailsender = new ForMailController();
                    CbfSumModel objMail = new CbfSumModel();
                    int refgid = objMail.GetRefGidForMail("PR");
                    string reqstatus = objMail.GetRequestStatus(refgid, "PR");
                    int queuegid = objMail.GetQueueGidForMail(refgid, "PR");
                    mailsender.sendusermail("FB", "PR", Convert.ToString(queuegid), reqstatus, "0");
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
        public JsonResult SubmitApprover(PRNewEntity PRParentDetailsInsert)
        {
            string ErrorMsg = string.Empty;
            string result1 = string.Empty;
            List<flexibuydashboard> obj = new List<flexibuydashboard>();
           
            try
            {
               
                string Data1 = "0";
                string ref_name = "PR";
                DataSet ds = new DataSet();
                DataTable dt1 = new DataTable();
                ErrorMsg = "1";
                DataTable dt = new DataTable();
                DataSet ds1 = db.CheckAuthorize(Convert.ToString(PRParentDetailsInsert.PRGid), Convert.ToString(plib.LoginUserId), "PR");

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


                    string result = objModel.SubmitApprover(PRParentDetailsInsert);

                    ds = objprModel.Getapprovaldetails(ref_name); 
                    //dt1 = new DataTable(typeof(flexibuydashboard).Name);
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
                        int refgid = Convert.ToInt32(PRParentDetailsInsert.PRGid);
                        string reqstatus = objMail.GetRequestStatus(refgid, "PR");
                        int queuegid = objMail.GetQueueGidForMail(refgid, "PR");
                        mailsender.sendusermail("FB", "PR", Convert.ToString(queuegid), reqstatus, "0");
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
        public JsonResult RejectApprover(PRNewEntity PRParentDetailsInsert)
        {
            string ErrorMsg = string.Empty;
            try
            {
                string Data1 = "0";
                string ref_name = "PR";
                DataSet ds = new DataSet();
                DataTable dt1 = new DataTable();
                ErrorMsg = "1";
                int result = objModel.RejectApprover(PRParentDetailsInsert);
                ds = objprModel.Getapprovaldetails(ref_name); 
                dt1 = ds.Tables[0];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Data1 = JsonConvert.SerializeObject(dt1);
                }
                if (result > 0)
                {
                    ForMailController mailsender = new ForMailController();
                    CbfSumModel objMail = new CbfSumModel();
                    int refgid = Convert.ToInt32(PRParentDetailsInsert.PRGid);
                    string reqstatus = objMail.GetRequestStatus(refgid, "PR");
                    int queuegid = objMail.GetQueueGidForMail(refgid, "PR");
                    mailsender.sendusermail("FB", "PR", Convert.ToString(queuegid), reqstatus, "0");
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
        [HttpPost]
        public JsonResult GetAuditTrail(string prgid = null, string refname = null) 
        {
            List<AuditTrailWO> lst = new List<Models.AuditTrailWO>();
            int prheadergid = Convert.ToInt32(string.IsNullOrEmpty(prgid) ? "0" : prgid);
            try
            {
                WODataModel objWoEntity = new WODataModel();
                lst = objWoEntity.PopulateAuditTrail(prheadergid, refname);
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public void UploadedFiles(string uploadfor, string fname = null,string attach = null, string attaching_format = null) //Ramya 18-11-2019 
        {
            try
            {
                Session["filenamePR"] = null;
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
                                Session["filenamePR"] = hpfBase;
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
                Session["filenamePR"] = null;
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
                        var path = Path.Combine(CmnFilePath, filename + fileextension);
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                        filename = filename + fileextension;
                        Session["filenamePR"] = filename;
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
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); 
            }
        }
        [HttpPost]
        public JsonResult CreatePRAttachment(Attachments PRParentDetailsInsert)
        {
            string data0 = string.Empty;
            string ErrorMsg = string.Empty;
            string filename = "";
            try
            {
                ErrorMsg = "1";
                if (Session["filenamePR"] != null && Session["uploadfor"]!=null)
                {
                    filename = objCmnFunctions.GetSequenceNo(Session["uploadfor"].ToString());

                    HttpPostedFileBase _attFile = Session["filenamePR"] as HttpPostedFileBase;
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
                        PRParentDetailsInsert.TempFileName = filename;
                        int data = objModel.CreatePRAttachment(PRParentDetailsInsert);
                        if (data > 0)
                        {
                            ErrorMsg = "0";
                            DataSet ds = new DataSet();
                            DataTable dt;
                            ds = objModel.GetPRAttachments(PRParentDetailsInsert.AttachmentFor, PRParentDetailsInsert.AttachmentRefGid);
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
        public JsonResult GetAttachment(Attachments PRParentDetailsInsert) 
        {
            string data0 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "1";
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objModel.GetPRAttachments(PRParentDetailsInsert.AttachmentFor, PRParentDetailsInsert.AttachmentRefGid);
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
        public JsonResult CheckFileExists(string id ="")
        {
            string result = "0";
            try
            {
                var CmnFilePath = objCmnFunctions.IEMAttachmentPath();
                var localpath = Path.Combine(CmnFilePath, id);
                //if (System.IO.File.Exists(localpath))
                //{
                //    result = "1";
                //}
                FileServer ObjService = new FileServer();
                string isExists = ObjService.DownloadFile(id).Result;
                if (isExists != "Fail")
                {
                    result = "1";
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public FileResult DownloadDocument(string id = "")
        {
            try
            {
                string filename = id;
                //var CmnFilePath = objCmnFunctions.IEMAttachmentPath();
                //var localpath = Path.Combine(CmnFilePath, filename);
                FileServer ObjService = new FileServer();
                //var isExists = ObjService.CheckFileExists(filename);
                //if (isExists == 1)
                //{
                //    byte[] filebyte = ObjService.CopyFiles(filename);
                //    System.IO.File.WriteAllBytes(localpath, filebyte);
                //}
                //byte[] fileBytes = System.IO.File.ReadAllBytes(localpath);
                //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
                var result = ObjService.DownloadFile(id).Result;
                if (result != "Fail")
                {
                    byte[] fileBytes = Convert.FromBase64String(result);
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
                }
                else
                {
                    return File("C:/temp/nofile.txt", System.Net.Mime.MediaTypeNames.Application.Octet, filename);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult DeletePRAttachment(Attachments PRParentDetailsInsert) 
        {
            string data0 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "1";
                    int result = objModel.DeletePRAttachment(PRParentDetailsInsert);
                    if (result > 0)
                    {
                        ErrorMsg = "0";
                        DataSet ds = new DataSet();
                        DataTable dt;
                        ds = objModel.GetPRAttachments(PRParentDetailsInsert.AttachmentFor, PRParentDetailsInsert.AttachmentRefGid);
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
        public JsonResult DoClone(string prgid = "") 
        {
            string data0 = string.Empty;
            string ErrorMsg = string.Empty;  
            try
            {
                ErrorMsg = "1";
                DataTable dt = new DataTable();
                int result = Convert.ToInt32(string.IsNullOrEmpty(prgid) ? "0" : prgid);
                dt = objModel.DoClone(result);
                if (dt.Rows.Count > 0){
                    data0 = JsonConvert.SerializeObject(dt);
                    ErrorMsg = "0";
                }
                
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult CheckAmount(string prgid = "")
        {
            string data0 = "0";
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "1";
                DataTable dt = new DataTable();
                int result = Convert.ToInt32(string.IsNullOrEmpty(prgid) ? "0" : prgid);
                data0 = Convert.ToString(objModel.CheckAmount(result));
                ErrorMsg = "0";
                if (data0 == "-1")
                    ErrorMsg = "404";
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
