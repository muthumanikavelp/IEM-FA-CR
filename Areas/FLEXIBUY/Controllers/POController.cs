using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;
using IEM.Helper;
using IEM.Common;
using IEM.Areas.FLEXIBUY.Models;
using IEM.Areas.MASTERS.Controllers;
using System.IO;
using System.Net;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class POController : Controller
    {

        #region Declaration
        proLib plib = new proLib();
        dbLib db = new dbLib();
        DataTable dt;
        CmnFunctions objCmnFunctions = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        #endregion
        private Irepositorypr objModel;
        public POController()
            : this(new fb_DataModelpr())
        {

        }
        public POController(Irepositorypr objM)
        {
            objModel = objM;
        }

        // GET: /FLEXIBUY/PO/

        public ActionResult CBFSelection()
        {
            return View();
        }

        public ActionResult RaisePO(int Id = 0, int Access = 1)
        {

            ViewBag.POId = Id;
            ViewBag.Access = Access;
            return View();
        }

        public ActionResult POChecker(int Id = 0, int Access = 1)
        {
            ViewBag.POId = Id;
            ViewBag.Access = Access;
            return View();
        }

        public ActionResult POAmentment()
        {
            return View();
        }

        #region CBF Selection
        public JsonResult GetPOCBFHeader(string CBFNo, string CBFDateFrom, string CBFDateTo, string CBFReqFor)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetPOCBFHeader(CBFNo, plib.ConvertDate(CBFDateFrom), plib.ConvertDate(CBFDateTo), CBFReqFor, plib.LoginUserId);

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

        public JsonResult GetPOCBFDetails(string CBFHeaderGId)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetPOCBFDetails(CBFHeaderGId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        public JsonResult SetPOCBF(string CBFDetailGIds)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.SetPOCBF(CBFDetailGIds, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return null;
            }
        }

        public JsonResult GetAttachment(string RefGId, string RefFlag, string AttachTypeGId)
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
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return null;
            }
        }

        public FileResult Download(string Id, string FileName)
        {
            string[] str = FileName.Split('.');
            // string directory = plib.HoldFileUploadUrl + Id.ToString() + "." + str[str.Length - 1].ToString();
            string directory = Id.ToString() + "." + str[str.Length - 1].ToString();
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
                    return File("", "");
                }
            }
            else
            {
                return File("", "");
            }
            //byte[] fileBytes = System.IO.File.ReadAllBytes(directory);
            //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, FileName);
        }

        #endregion

        #region Raise PO
        public JsonResult GetPO(string POHeaderGId)
        {
            try
            {
                string Data1 = "", Data2 = "", Data3 = "", Data4 = "", Data5 = "";
                List<AuditTrail> audittrail = new List<Models.AuditTrail>();
                DataSet ds = db.GetPO(POHeaderGId, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[2];
                if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[3];
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
                        int refgid = Convert.ToInt32(POHeaderGId);
                        int refflagnew = Convert.ToInt32(dt.Rows[i]["QRefFlag"].ToString());
                        string isfinalapprover = wodt.CheckFinalApproverTag(refgid, refflagnew, queue_gid);
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
                if (audittrail.Count > 0) { Data4 = JsonConvert.SerializeObject(audittrail); }
                dt = ds.Tables[4]; // Address Details
                if (dt.Rows.Count > 0) { Data5 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1, Data2, Data3, Data4, Data5 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return null;
            }
        }

        public JsonResult SetPOTemplate(string TemplateGId, string TemplateName, string Content, string IsRemoved)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.SetPOTemplate(TemplateGId, TemplateName, Content, IsRemoved, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return null;
            }
        }

        public JsonResult SetShipment(string ShipmentGId, string PODetailGId, string ShipmentTypeGId, string BranchGId, string InchargeGId, string ShippedQty, string IsRemoved)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.SetShipment(ShipmentGId, PODetailGId, ShipmentTypeGId, BranchGId, InchargeGId, ShippedQty, IsRemoved, plib.LoginUserId);

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
        // add gsttax
        public JsonResult SetPOTaxdetails(string PODetailGId,string HSNgid,string Taxsubtypeid,string Taxrate,string TaxAbleamt,string TaxAmt)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.SetPoTaxdtls(PODetailGId, HSNgid, Taxsubtypeid, Taxrate, TaxAbleamt, TaxAmt);
                if (ds.Tables.Count > 0)
                {
                    Data1 = JsonConvert.SerializeObject(ds.Tables[0]);
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return null;
            }
        }

        public JsonResult GetShipment(string PODetailGId, string VendorId)
        {
            try
            {
                //bharathidhasan kumar
                string Data1 = "", Data2 = "", Data0 = "", Data3 = "",Data4="";
                DataSet ds = db.GetShipment(PODetailGId, VendorId);

                dt = ds.Tables[0];// Poshiment details
                if (dt.Rows.Count > 0) { Data0 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];// po gst grid details
                if (dt.Rows.Count > 0)
                {
                    Data2 = JsonConvert.SerializeObject(dt);
                }
                dt = ds.Tables[2];// po gst header details
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                dt = ds.Tables[3];// provider location 
                DataRow row3 = dt.NewRow();
                dt.Rows.InsertAt(row3, 0);
                dt.Rows[0][0] = "0";
                dt.Rows[0][1] = "-- Select One --";
                if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[4];
                if (dt.Rows.Count > 0) { Data4 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data0, Data2, Data1, Data3,Data4 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return null;
            }
        }

        public JsonResult SetPODetails(string POHeaderGId, string PODetailGId, string ProductGId, string Description, string UOM, string Qty, string UnitPrice, string BaseAmount, string Discount, string Taxes, string TaxPercent, string OtherCharges, string NetAmount, string IsRemoved)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.SetPODetails(POHeaderGId, PODetailGId, ProductGId, Description, UOM, Qty, UnitPrice, BaseAmount, Discount, Taxes, TaxPercent, OtherCharges, NetAmount, IsRemoved, plib.LoginUserId);

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

        public JsonResult SplitPODetails(string POHeaderGId, string PODetailGId, string ProductGId, string Description, string UOM, string Qty, string UnitPrice, string BaseAmount, string Discount, string Taxes, string OtherCharges, string NetAmount, string POPercent = null)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.SplitPODetails(POHeaderGId, PODetailGId, ProductGId, Description, UOM, Qty, UnitPrice, BaseAmount, Discount, Taxes, OtherCharges, NetAmount, plib.LoginUserId, POPercent);

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

        public JsonResult SubmitPO(string POHeaderGId, string POEndDate, string ProjectOwnerGId, string VendorGId, string Type, string VendorNote, string TemplateGId, string AddedTermAndCondtion, string IsRemoved, string ViewType, string IsReject, string Remarks, string VendorcontactId)
        {
            try
            {
                CbfSumModel objforMail = new CbfSumModel();
                string Data1 = "",Data2="0";
                string result = "";
                DataSet ds2 = new DataSet();
                DataTable dt2 = new DataTable();
                if (ViewType != "1")
                {
                    DataSet ds1 = db.CheckAuthorize(POHeaderGId, Convert.ToString(plib.LoginUserId), "PO");

                    dt = ds1.Tables[0];

                    if (dt.Rows.Count > 0 && ViewType != "1")
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
                if ((ViewType == "1") || (ViewType != "1" && result == "Y"))
                {


                    DataSet ds = db.SubmitPO(POHeaderGId, plib.ConvertDate(POEndDate), ProjectOwnerGId, VendorGId, Type, VendorNote, TemplateGId, AddedTermAndCondtion, IsRemoved, ViewType, IsReject, Remarks, plib.LoginUserId, VendorcontactId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {

                        if (dt.Rows[0]["Clear"].ToString() == "True")
                        {
                            ForMailController mailsender = new ForMailController();
                            //CbfSumModel objforMail = new CbfSumModel();
                            //int refgid = objforMail.GetRefGidForMail("PO");
                            string reqstatus = objforMail.GetRequestStatus(Convert.ToInt32(POHeaderGId), "PO");
                            int queuegid = objforMail.GetQueueGidForMail(Convert.ToInt32(POHeaderGId), "PO");
                            string curapprovalstage = "0";
                            if (reqstatus == "S")
                            {
                                curapprovalstage = "0";
                            }
                            else
                            {
                                curapprovalstage = "1";
                            }
                            mailsender.sendusermail("FB", "PO", Convert.ToString(queuegid), reqstatus, curapprovalstage);
                        }
                    }
                    Data1 = JsonConvert.SerializeObject(dt);
                }
                prsummodel prmodel = new prsummodel();
                ds2 = prmodel.Getapprovaldetails("PO");
                dt2 = ds2.Tables[0];

                
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    Data2 = JsonConvert.SerializeObject(dt2);
                }

                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return null;
            }
        }

        #endregion

        #region PO Amentment

        public JsonResult GetPOResult(string PONo, string PODate, string VendorId)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetPOAmentment(plib.ConvertDate(PODate), PONo, VendorId, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                var Jsonresult= Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                Jsonresult.MaxJsonLength = int.MaxValue;
                return Jsonresult;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return null;
            }
        }

        public JsonResult SetPOAmendment(string POHeaderGId)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.SetPOAmendment(POHeaderGId, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return null;
            }
        }

        #endregion

        #region Attachment
        [HttpPost]
        public JsonResult CreatePOAttachment(Attachments POAttachmentInsert)
        {
            string data0 = string.Empty;
            string ErrorMsg = string.Empty;
            string filename = "";
            try
            {


                ErrorMsg = "1";
                if (Session["filenamePO"] != null && Session["uploadfor"] != null)
                {
                    filename = objCmnFunctions.GetSequenceNo(Session["uploadfor"].ToString());

                    HttpPostedFileBase _attFile = Session["filenamePO"] as HttpPostedFileBase;
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
                        POAttachmentInsert.TempFileName = filename;
                        int data = objModel.CreatePOAttachment(POAttachmentInsert);
                        if (data > 0)
                        {
                            ErrorMsg = "0"; 
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
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return null;
            }
        }
        [HttpPost]
        public JsonResult CreatePOAttachmentold(Attachments POAttachmentInsert)
        {
            string data0 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {


                ErrorMsg = "1";
                if (Session["filenamePO"] != null)
                {
                    if (Session["fileSaved"] == "Success")
                    {

                        POAttachmentInsert.TempFileName = (string)Session["filenamePO"];
                        objModel.CreatePOAttachment(POAttachmentInsert);
                        ErrorMsg = "0";

                    }
                    else
                    {
                        ErrorMsg = "1";
                    }
                }
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return null;
            }
        }

        [HttpPost]
        public void UploadedFiles(string uploadfor, string fname = null, string attach = null, string attaching_format = null) //Ramya 18-11-2019 
        {
            try
            {
                Session["filenamePO"] = null;
                Session["uploadfor"] = null;
                //Session["fileSaved"] = null;
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
                                Session["filenamePO"] = hpfBase;
                                Session["uploadfor"] = uploadfor;
                                //Session["fileSaved"] = "Success";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                Json("Upload failed");
            }
        }

        [HttpPost]
        public void UploadedFilesold(string uploadfor, string fname = null)
        {
            try
            {
                Session["filenamePO"] = null;
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
                        byte[] bytFile = null;
                        using (var memoryStream = new MemoryStream())
                        {
                            stream.CopyTo(memoryStream);
                            bytFile = memoryStream.ToArray();
                        }
                        filename = filename + fileextension;
                        Session["filenamePO"] = filename;
                        var FileString = Convert.ToBase64String(bytFile);
                        FileServer objserver = new FileServer();
                        // string result = objserver.SaveFiles(bytFile, filename);
                        string ff = objserver.SaveFileToServer(FileString, filename).Result;
                        if (ff == "Success")
                        {
                            Session["fileSaved"] = "Success";
                        }
                        else
                        {
                            Session["fileSaved"] = "Failed";
                        }
                    }
                }
                //  return Json(filename);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                Json("Upload failed");
            }
        }

        public JsonResult GetPoAttachments(string POHeaderGId)
        {
            string Data1 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "0";
                DataSet ds = new DataSet();
                ds = db.GetPoAttachments(POHeaderGId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1, ErrorMsg }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                ErrorMsg = "1";
                return Json(new { Data1, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }

        public FileResult DownloadDocument(string id = "", string filename = "")
        {
            string ErrorMsg = string.Empty;
            try
            {

                //string filename = id;
                byte[] fileBytes = null;
                FileServer fileserver = new FileServer();

                var result = fileserver.DownloadFile(id).Result;
                if (result != "Fail")
                {
                    fileBytes = Convert.FromBase64String(result);
                    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
                }
                else
                {
                    return File("", "");
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw ex;
            }
        }

        public JsonResult DeleteAttachment(string id)
        {
            string Data1 = string.Empty;
            string ErrorMsg = string.Empty;
            ErrorMsg = "1";
            int result = objModel.DeletePOAttachment(id);
            if (result > 0)
            {
                ErrorMsg = "0";
                return Json(new { Data1, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetVendorAddress(string id)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetVendorAddressDetails(id);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return null;
            }
        }

        #endregion

        #region Get gst details
        public JsonResult GetPOdetails(string PODetailGId)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetPodtlsgst(PODetailGId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return null;
            }
        }
        #endregion
        #region Get PO GstTax Details
        public JsonResult GetTaxdetails(string PODetailGId, string ProvLocId, string Baseamt, string ReciverLocId, string HSN_ID)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.Getgsttax(PODetailGId, ProvLocId, Baseamt, ReciverLocId,HSN_ID);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                if (ds.Tables.Count > 1)
                {
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                }

                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return null;
            }
        }
        #endregion
        #region SetPoGstdetails
        public JsonResult SetPoGstDet(string POHeaderGId, string PODetilsId, string ProvLocationId, string ReceiverLocationId, string HsnId, string TaxsubTypeId, string TaxablAmt, string TaxRate, string NetAmt, string Vendorid, string TaxAmt)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.SavepoGstdetails(POHeaderGId, PODetilsId, ProvLocationId, ReceiverLocationId, HsnId, TaxsubTypeId, TaxablAmt, TaxRate, NetAmt, Vendorid, TaxAmt);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return null;
            }
        }
        #endregion

        #region DeletPoGstDetails
        public JsonResult DeletePoGstDetails(string POId, string Header_type, string  VendorId, string  VendorContactId)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.DeletePoGstdtl(POId, Header_type, VendorId, VendorContactId);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return null;
            }
        }
        #endregion
        #region DeleteProdDetails
        public JsonResult DeleteProdDetails(string PODetailGId)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.DeletePoctdetails(PODetailGId);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return null;
            }
        }
        #endregion
    }
}
