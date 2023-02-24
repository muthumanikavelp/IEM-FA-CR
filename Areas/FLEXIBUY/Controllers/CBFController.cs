using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;
using IEM.Helper;
using IEM.Areas.FLEXIBUY.Models;
using IEM.Common;
using IEM.Areas.MASTERS.Controllers;
using IEM.Models;
using System.IO;
using IEM.App_Start;

namespace IEM.Areas.FLEXIBUY.Controllers
{
    [NoDirectAccess]
    public class CBFController : Controller
    {
        #region Declaration
        proLib plib = new proLib();
        dbLib db = new dbLib();
        DataTable dt;
        private IRepositoryKIR objcbfmodel;
        ErrorLog objErrorLog = new ErrorLog();
        #endregion

        public CBFController(): this (new prsummodel())
        {

        }
        public CBFController(IRepositoryKIR objcbf)
        {
            objcbfmodel = objcbf;
        }

        //
        // GET: /FLEXIBUY/CBF/

        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult Edit(int Id = 0, int Access = 1)
        //{
        //    ViewBag.CBFId = Id;
        //    ViewBag.Access = Access;
        //    return View();
        //}
        public ActionResult Edit(int Id = 0, int Access = 1, int Attachment = 0)
        {
            ViewBag.CBFId = Id;
            ViewBag.Access = Access;
            ViewBag.Attachment = Attachment;
            return View();
        }

        public ActionResult CBFChecker(int Id = 0, int Access = 1)
        {
            ViewBag.CBFId = Id;
            ViewBag.Access = Access;
            return View();
        }

        #region CBF Entry
        public JsonResult SetCBFHeader(CBFHeader cbfheader)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.SetCBFHeader(cbfheader.CBFHeaderGId, plib.ConvertDate(cbfheader.CBFDate), plib.ConvertDate(cbfheader.CBFEndDate), cbfheader.PARPRHeaderGId, cbfheader.DeviationAmount,
                    cbfheader.ProjectOwnerId, cbfheader.BranchId, cbfheader.ReqBy, cbfheader.CBFMode, cbfheader.IsBudgeted, cbfheader.CBFApproval, cbfheader.BranchType,
                    cbfheader.BudgetOwnerId, cbfheader.Description, cbfheader.Justification, cbfheader.IsRemoved, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog("CbfController_SetCBFHeader" + ex.Message.ToString(), ex.ToString());
                return null;
            }
        }

        public JsonResult SetCBFDeatails(CBFDetails cbfdetails)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.SetCBFDetails(cbfdetails.CBFDetailGId, cbfdetails.CBFHeaderGId, cbfdetails.PARPRDetailGId, cbfdetails.PARPRDesc, cbfdetails.ProductGId,
                    cbfdetails.ProductGroupGId, cbfdetails.Remarks, cbfdetails.UOMGId, cbfdetails.Qty, cbfdetails.UnitPrice, cbfdetails.TotalAmount, cbfdetails.ChartOfAcc,
                    cbfdetails.FccCode, cbfdetails.BudgetLine, cbfdetails.IsRemoved, cbfdetails.IsContigency, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog("CbfController_SetCBFDeatails" + ex.Message.ToString(), ex.ToString());
                return null;
            }
        }

        public JsonResult SubmitCBF(string CBFHeaderGId, string Type, string IsReject, string Remarks)
        {
            try
            {
                string Data2 = "0";
                string ref_name = "CBF";
                DataSet ds2 = new DataSet();
                DataTable dt2 = new DataTable();

                string Data1 = "";
                string result = "";

                if (Type != "1" && Type != "7")
                {
                    DataSet ds1 = db.CheckAuthorize(CBFHeaderGId, Convert.ToString(plib.LoginUserId), "CBF");

                    dt = ds1.Tables[0];

                    if (dt.Rows.Count > 0 && Type != "1")
                    {
                        if (Convert.ToString(dt.Rows[0]["Result"]) == "Y")
                        {
                            result = Convert.ToString(dt.Rows[0]["Result"]);
                        }
                        else
                        {
                            result = "You are Not Authorized";
                            Data1 = JsonConvert.SerializeObject(dt);
                        }
                    }
                    else
                    {
                        result = "You are Not Authorized";
                        Data1 = JsonConvert.SerializeObject(dt);
                    }
                }
                if ((Type == "1") || (Type != "1" && result == "Y") || (Type == "7"))
                {
                    DataSet ds = db.SubmitCBF(CBFHeaderGId, Type, IsReject, Remarks, plib.LoginUserId);

                dt = ds.Tables[0];
                string CBFHeaderGIds = CBFHeaderGId;
                if (dt.Rows.Count > 0)
                {
                    //string Cbfstatus = dt.Rows[0]["Clear"].ToString();
                    if (dt.Rows[0]["Clear"].ToString() == "True")
                    {
                        ForMailController mailsender = new ForMailController();
                        CbfSumModel objMail = new CbfSumModel();
                        //int refgid = objMail.GetRefGidForMailCbfheader("CBF", CBFHeaderGIds);
                        string reqstatus = objMail.GetRequestStatus(Convert.ToInt32(CBFHeaderGId), "CBF");
                        int queuegid = objMail.GetQueueGidForMail(Convert.ToInt32(CBFHeaderGId), "CBF");
                        mailsender.sendusermail("FB", "CBF", Convert.ToString(queuegid), reqstatus, "0");
                    }
                    Data1 = JsonConvert.SerializeObject(dt);
                    }
                }

                ds2 = objcbfmodel.Getapprovaldetails(ref_name);
                dt2 = ds2.Tables[0];

                if (ds2.Tables[0].Rows.Count > 0)
                {
                    Data2 = JsonConvert.SerializeObject(dt2);
                }

                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                objErrorLog.WriteErrorLog("CbfController_SubmitCBF" + ex.Message.ToString(), ex.ToString());
                return null;
            }
        }

        public JsonResult GetCBFPARHeader(string CBFHeaderGId, string IsBudgeted, string RequestForGId)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetCBFPARHeader(CBFHeaderGId, IsBudgeted, RequestForGId, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return null;
            }
        }

        public JsonResult GetCBFPARHeaderSearch(string IsBudgeted, string PARDate, string PARNo, string PARAmount, string RequestForGId)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetCBFPARHeaderSearch(IsBudgeted, plib.ConvertDate(PARDate), PARNo, PARAmount, RequestForGId, plib.LoginUserId);

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return null;
            }
        }

        public JsonResult GetCBFPARDetails(string CBFHeaderGId, string PARHeaderGId, string RequestForGId, string IsBudgeted)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetCBFPARDetails(CBFHeaderGId, PARHeaderGId, RequestForGId, IsBudgeted, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return null;
            }
        }

        public JsonResult GetCBFPRHeader(string CBFHeaderGId, string RequestForGId)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetCBFPRHeader(CBFHeaderGId, RequestForGId, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return null;
            }
        }

        public JsonResult GetCBFPRHeaderSearch(string RequestForGId, string PRDate, string PRNo)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetCBFPRHeaderSearch(RequestForGId, plib.ConvertDate(PRDate), PRNo, plib.LoginUserId);

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return null;
            }
        }

        public JsonResult GetCBFPRDetails(string CBFHeaderGId, string PRHeaderGId)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetCBFPRDetails(CBFHeaderGId, PRHeaderGId, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return null;
            }
        }

        public JsonResult SetCBFPRDetails(string CBFHeaderGId, string PRDetailGId, string Qty, string IsRemoved)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.SetCBFPRDetails(CBFHeaderGId, PRDetailGId, Qty, IsRemoved, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog("CbfController_SetCBFPRDetails" + ex.Message.ToString(), ex.ToString());
                return null;
            }
        }

        public JsonResult GetPARAttachment(string RefGId, string RefFlag, string AttachTypeGId)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetPARAttachment(RefGId, RefFlag, AttachTypeGId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog("CbfController_GetPARAttachment" + ex.Message.ToString(), ex.ToString());
                return null;
            }
        }

        public JsonResult GetRequestBy()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetRequestBy(plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog("CbfController_GetRequestBy" + ex.Message.ToString(), ex.ToString());
                return null;
            }
        }

        public JsonResult SetCBFAttachment(CBFAttachment attachment)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataTable dt1 = new DataTable();
                dt1.Columns.Add("Message", typeof(string));
                dt1.Columns.Add("Clear", typeof(bool));
                dt1.Rows.Add(new object[] { "Ensure Attachemnt File!", false });

                //if (TempData["_attFile"] != null)
                if (Session["_attFile"] != null)
                {
                    //HttpPostedFileBase _attFile = TempData["_attFile"] as HttpPostedFileBase;
                    HttpPostedFileBase _attFile = Session["_attFile"] as HttpPostedFileBase;
                    attachment.AttachmentName = _attFile.FileName.ToString();
                }
                if (attachment.AttachmentName == null && attachment.IsRemoved == "0")
                {
                    if (dt1.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt1); }
                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                }
                string filename = attachment.AttachmentName;
                if (attachment.IsRemoved == "1")
                {
                    filename = "Delete.txt";
                }
                //if (filename.Length < 64 && System.Text.RegularExpressions.Regex.IsMatch(filename, "^[a-zA-Z0-9\x20.]+$") == true)
                //{
                    DataSet ds = db.SetCBFAttachment(attachment.RefGId, attachment.AttachmentId, attachment.AttachmentName, attachment.Description, attachment.RefFlag, attachment.IsRemoved, plib.LoginUserId);
                    if (ds != null)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                        dt = ds.Tables[1];
                        if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                        if (attachment.AttachmentName != "" && attachment.IsRemoved == "0" && (ds.Tables[0].Rows[0]["Clear"].ToString().ToLower() != "true" || ds.Tables[0].Rows[0]["Clear"].ToString().ToLower() != "1"))
                        {
                            try
                            {
                                //upload the file to server.
                                //HttpPostedFileBase _attFile = TempData["_attFile"] as HttpPostedFileBase;
                                HttpPostedFileBase _attFile = Session["_attFile"] as HttpPostedFileBase;
                                var stream = _attFile.InputStream;
                                string uploaderUrl = string.Format("{0}{1}", plib.HoldFileUploadUrl, ds.Tables[0].Rows[0]["AttachmentGId"].ToString() + "." + _attFile.FileName.Split('.')[_attFile.FileName.Split('.').Length - 1]);
                                using (var fileStream = System.IO.File.Create(uploaderUrl))
                                {
                                    stream.CopyTo(fileStream);
                                }
                                var path = System.IO.Path.Combine(plib.HoldFileUploadUrl, ds.Tables[0].Rows[0]["AttachmentGId"].ToString() + "." + _attFile.FileName.Split('.')[_attFile.FileName.Split('.').Length - 1]);
                                byte[] bytFile = System.IO.File.ReadAllBytes(path);
                                string Filestring = Convert.ToBase64String(bytFile);
                                FileServer objserver = new FileServer();
                                string result = objserver.SaveFileToServer(Filestring, ds.Tables[0].Rows[0]["AttachmentGId"].ToString() + "." + _attFile.FileName.Split('.')[_attFile.FileName.Split('.').Length - 1]).Result;
                                // string result = objserver.SaveFiles(bytFile, ds.Tables[0].Rows[0]["AttachmentGId"].ToString() + "." + _attFile.FileName.Split('.')[_attFile.FileName.Split('.').Length - 1]);
                                if (result == "Success")
                                {
                                    if (System.IO.File.Exists(path))
                                    {
                                        System.IO.File.Delete(path);
                                    }
                                }
                                //remove the temp data content.
                                TempData.Remove("_attFile");
                            }
                            catch { }
                        }
                        return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return null;
                    }
                //}
                //else
                //{
                //    dt1.Clear();
                //    dt1.Rows.Add(new object[] { "Filename Lenght should be below 64 characters and Special characters not allowed!", false });
                //    if (dt1.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt1); }
                //    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                //}
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog("CbfController_SetcbfAttachment" + ex.Message.ToString(), ex.ToString());
                return null;
            }
        }

        public void UploadCBFAttachment(string attach = null, string attaching_format = null) //Pandiaraj 18-11-2019 
        {
            try
            {
                Session["_attFile"] = null;
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
                                Session["_attFile"] = hpfBase;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog("CbfController_Uploadcbfattachment" + ex.Message.ToString(), ex.ToString());

            }
        }

        public void UploadCBFAttachmentold()
        {
            try
            {
                //TempData["_attFile"] = null;
                Session["_attFile"] = null;
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase hpfBase = Request.Files[file] as HttpPostedFileBase;
                    //TempData["_attFile"] = hpfBase;
                    Session["_attFile"] = hpfBase;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog("CbfController_Uploadcbfattachment" + ex.Message.ToString(), ex.ToString());
                
            }
        }

        public FileResult Download(string Id, string FileName)
        {
            string[] str = FileName.Split('.');
            //string directory = plib.HoldFileUploadUrl + Id.ToString() + "." + str[str.Length - 1].ToString();
            string directory =  Id.ToString() + "." + str[str.Length - 1].ToString();
            //var localpath = System.IO.Path.Combine(plib.HoldFileUploadUrl, Id.ToString() + "." + str[str.Length - 1].ToString());
            //FileServer ObjService = new FileServer();
           // var isExists = ObjService.CheckFileExists(Id.ToString() + "." + str[str.Length - 1].ToString());
            //if (isExists == 1)
            //{
            //    byte[] filebyte = ObjService.CopyFiles(Id.ToString() + "." + str[str.Length - 1].ToString());
            //    System.IO.File.WriteAllBytes(localpath, filebyte);
            //}
            FileServer ObjService = new FileServer();
            var Filenamecont = ObjService.DownloadFile(directory).Result;

            if (Filenamecont != "error")
            {
                if (Filenamecont != "Fail")
                {
                    byte[] fileBytes = Convert.FromBase64String(Filenamecont);
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, FileName);
                }
                else
                {
                    return File("","");
                }
            }
            else
            {
                return File("","");
            }
        }

        public FileResult DownloadPARDetails(string FileName)
        {
            string directory = plib.HoldFileUploadUrl + FileName;
            var localpath = System.IO.Path.Combine(plib.HoldFileUploadUrl, FileName);
            FileServer ObjService = new FileServer();
            var isExists = ObjService.CheckFileExists(FileName);
            if (isExists == 1)
            {
                byte[] filebyte = ObjService.CopyFiles(FileName);
                System.IO.File.WriteAllBytes(localpath, filebyte);
            }
            byte[] fileBytes = System.IO.File.ReadAllBytes(directory);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, FileName);
        }

        #endregion

        #region CBF Edit
        public JsonResult GetCBFHeader(string CBFHeaderGId)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "";
                List<AuditTrail> audittrail = new List<Models.AuditTrail>();
                DataSet ds = db.GetCBFHeader(CBFHeaderGId, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[2];
                if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[3];
                if (dt.Rows.Count > 0) { Data4 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[4];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["QueuePrevGId"].ToString() == "0")
                        audittrail.Add(new AuditTrail() { EmployeeName = dt.Rows[i]["QFromEmployee"].ToString(), ActionDate = dt.Rows[i]["QEntryDate"].ToString(), Approval = "Raiser", Status = (i == 0 ? "Submitted" : "Resubmitted"), Remarks = "" });
                    if (dt.Rows[i]["Employee"].ToString() == "1")
                        dt.Rows[i]["Employee"] = db.GetAuditEmployee(dt.Rows[i]["RaiserGId"].ToString(), dt.Rows[i]["QToTypeName"].ToString(), dt.Rows[i]["QueueTo"].ToString()).Tables[0].Rows[0][0].ToString();
                    if ((i + 1) == dt.Rows.Count)
                    {
                        WODataModel wodt = new WODataModel();
                        int queue_gid = Convert.ToInt32(dt.Rows[i]["QueueGId"].ToString());
                        int refgid = Convert.ToInt32(CBFHeaderGId);
                        int refflagnew = Convert.ToInt32(dt.Rows[i]["QRefFlag"].ToString());
                        string isfinalapprover =wodt.CheckFinalApproverTag(refgid, refflagnew, queue_gid);
                        if (isfinalapprover == "Y")
                            dt.Rows[i]["ActionStatusName"] = "Final Approver";
                    }

                    if (dt.Rows[i]["delegation_id"].ToString() == "0")
                    {
                        audittrail.Add(new AuditTrail() { EmployeeName = dt.Rows[i]["Employee"].ToString(), ActionDate = dt.Rows[i]["ActionDate"].ToString(), Approval = dt.Rows[i]["ApprovalStage"].ToString(), Status = dt.Rows[i]["ActionStatusName"].ToString(), Remarks = dt.Rows[i]["Remarks"].ToString() });
                    }
                    else
                    {
                        audittrail.Add(new AuditTrail() { EmployeeName = dt.Rows[i]["delegation_by"].ToString() + " instead of  " + dt.Rows[i]["Employee"].ToString(), ActionDate = dt.Rows[i]["ActionDate"].ToString(), Approval = dt.Rows[i]["ApprovalStage"].ToString(), Status = dt.Rows[i]["ActionStatusName"].ToString(), Remarks = dt.Rows[i]["Remarks"].ToString() });
                    }

                    
                }
                if (audittrail.Count > 0) { Data5 = JsonConvert.SerializeObject(audittrail); }

                return Json(new { Data1, Data2, Data3, Data4, Data5}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog("CbfController_GetCBFHeader" + ex.Message.ToString(), ex.ToString());
                return null;
            }
        }
        #endregion

    }
}
