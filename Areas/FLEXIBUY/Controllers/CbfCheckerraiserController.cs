using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using IEM.Areas.FLEXIBUY.Models;
using System.IO;
using IEM.Common;
using IEM.Areas.MASTERS.Controllers;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{
    [NoDirectAccess]
    public class CbfCheckerraiserController : Controller
    {
        CmnFunctions objCmnFunctions = new CmnFunctions();
        private IrepositoryAn objRep;
        public CbfCheckerraiserController()
            : this(new CbfSumModel())
        {

        }
        public CbfCheckerraiserController(IrepositoryAn objModel)
        {
            objRep = objModel;
        }
        public ActionResult CbfChck(int lnid, string lsviewfor)
        {
            Session["Cbfgid"] = lnid;
            Session["Cancellationgid"] = "";
            string[] lsviewfor12 = lsviewfor.Split('_');
            string process = "";
            if (Request.QueryString["process"] != null)
            {
                process = Request.QueryString["process"].ToString();
            }

            string Isview = lsviewfor12[0].ToString();
            if (Isview == "CancellationChecker")
            {
                Session["viewfor"] = "CancellationChecker";
                ViewBag.viewfor = "CancellationChecker";
                Session["Cancellationgid"] = lsviewfor12[1].ToString();
                if (process.ToString() != "")
                {
                    ViewBag.Cancellationmaker = process.ToString();
                }
            }

            if (Isview == "Approval")
            {
                ViewBag.viewfor = "Approval";
            }
            if (Isview == "closermaker")
            {
                ViewBag.viewfor = "closermaker";
            }
            if (Isview == "closurechecker")
            {
                ViewBag.viewfor = "closurechecker";
                Session["Cancellationgid"] = lsviewfor12[1].ToString();
                if (process.ToString() != "")
                {
                    string[] processsplit = process.Split(',');
                    ViewBag.closuredate = processsplit[0].ToString();
                    ViewBag.remarks = processsplit[1].ToString();
                }
            }
            if (Isview == "reopenmaker")
            {
                Session["Cancellationgid"] = lsviewfor12[1].ToString();
                ViewBag.viewfor = "Reopenmaker";
            }
            if (Isview == "Checker")
            {
                ViewBag.viewfor = "Checker";
            }
            if (Isview == "reopenchecker")
            {
                Session["Cancellationgid"] = lsviewfor12[1].ToString();
                if (process.ToString() != "")
                {
                    string[] processsplit = process.Split(',');
                    ViewBag.closuredate = processsplit[0].ToString();
                    ViewBag.remarks = processsplit[1].ToString();
                }
                ViewBag.viewfor = "reopenchecker";
            }
            if(Isview=="delmat")
            {
                ViewBag.viewfor = "delmat";
            }
            CbfSumEntity objCrheader = new CbfSumEntity();

            String lsNumber = lnid.ToString();
            Session["Cbfnochecker"] = lnid;

            objCrheader = objRep.Getcbfchecker(lsNumber);

            objCrheader.productGroupList = new SelectList(objRep.Getproductlist(), "productgroupid", "productgroupidname");
            objCrheader.uomList = new SelectList(objRep.GetUomList(), "uomgid", "uomcode");
            objCrheader.requestFor = new SelectList(objRep.GetList1(), "requeestforgid", "requestforname", objCrheader.cbfCheckerHeader.requeestForGidChecker);
            objCrheader.projectOwner1 = new SelectList(objRep.GetList(), "projectownergid", "projectownername", objCrheader.cbfCheckerHeader.projectOwnerGidChecker);
            objCrheader.branchCode1 = new SelectList(objRep.GetBranchCode(), "branchcodegid", "branchcodename", objCrheader.cbfCheckerHeader.branchCodeGidChecker);
            objCrheader.Supervisor = new SelectList(objRep.GetSupervisor(), "supervisorGidChecker", "SupervisornameChecker");
            if (Isview == "closermaker")
            {
                objCrheader = objRep.GetUltilizedamount(objCrheader);
            }
            if (Isview == "closurechecker")
            {
                objCrheader = objRep.GetUltilizedamount(objCrheader);
            }
            Session["CbfChecker"] = objCrheader;

            return View(objCrheader);
        }
        [HttpGet]
        public PartialViewResult CbfChckDetails()
        {
            CbfSumEntity objCrheader = new CbfSumEntity();
            objCrheader = (CbfSumEntity)Session["CbfChecker"];
            return PartialView(objCrheader);
        }
        [HttpGet]
        public PartialViewResult Viewboqattachment()
        {
            CbfSumEntity objCrheader = new CbfSumEntity();
            objCrheader = (CbfSumEntity)Session["CbfChecker"];
            return PartialView(objCrheader);
        }
        [HttpPost]
        public PartialViewResult Viewboqattachment(string lsCbfAttach)
        {
            CbfSumEntity objCrheader = new CbfSumEntity();
            objCrheader = (CbfSumEntity)Session["CbfChecker"];
            return PartialView(objCrheader);
        }
        [HttpPost]
        public JsonResult Closure(CbfRaiseHeader objCbfRaiser)
        {
            string data = objRep.Closure(objCbfRaiser);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult RaiserTicket()
        {
            CbfSumEntity objCrheader = new CbfSumEntity();
            objCrheader.Supervisor = new SelectList(objRep.GetSupervisor(), "supervisorGidChecker", "SupervisornameChecker");

            return PartialView(objCrheader);

        }
        [HttpPost]
        public JsonResult SaveRaiserTicket(RaiserTicket objTicket)
        {
            List<RaiserTicket> SelectDate = new List<RaiserTicket>();
            if (objTicket.message != "Drop")
            {
                objRep.InsertConversation(objTicket);
            }
            SelectDate = objRep.SelectDate(objTicket).ToList();
            return Json(SelectDate, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult Approved(quenue quenue)
        {
            string data = string.Empty;
            if(quenue.actionstatus=="checker")
            {
                data = objRep.QueueProcess(quenue);
            }
            if (quenue.actionstatus == "supervisor")
            {
                string prhgid = Session["Cbfnochecker"].ToString();
                string remarks=string.Empty;
                data = objRep.GetDelmatemployee(prhgid, quenue.actionstatus, quenue, remarks);
            }
            ForMailController mailsender = new ForMailController();
            CbfSumModel objMail = new CbfSumModel();
            if (Session["Cbfnochecker"] != null)
            {
                int refgid = Convert.ToInt32(Session["Cbfnochecker"]);
                string reqstatus = objMail.GetRequestStatus(refgid, "CBF");
                int queuegid = objMail.GetQueueGidForMail(refgid, "CBF");
                mailsender.sendusermail("FB", "CBF", Convert.ToString(queuegid), reqstatus, "0");
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Rejected(quenue quenue)
        {
            string data = objRep.QueueProcessreject(quenue);
            ForMailController mailsender = new ForMailController();
            CbfSumModel objMail = new CbfSumModel();
            if (Session["Cbfnochecker"] != null)
            {
                int refgid = Convert.ToInt32(Session["Cbfnochecker"]);
                string reqstatus = objMail.GetRequestStatus(refgid, "CBF");
                int queuegid = objMail.GetQueueGidForMail(refgid, "CBF");
                mailsender.sendusermail("FB", "CBF", Convert.ToString(queuegid), reqstatus, "0");
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ApprovalCancellation(string remarks)
        {
            string data = objRep.Approvalcancellation(remarks);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RejectedCancellation(string remarks)
        {
            string data = objRep.Rejectcancellation(remarks);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ApprovalClosure(string remarks)
        {
            string data = objRep.Approvalclosure(remarks);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RejectedClosure(string remarks)
        {
            string data = objRep.Rejectclosure(remarks);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Reopen(CbfRaiseHeader objCbfRaiser)
        {
            string data = objRep.Reopen(objCbfRaiser);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ApprovalReopen(string remarks)
        {
            string data = objRep.ApprovalReopen(remarks);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RejectedReopen(string remarks)
        {
            string data = objRep.Rejectreopen(remarks);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DownloadDocument(CbfSumEntity obj)
        {
            Session["downfile"] = obj.attachment1;
            CbfSumEntity obj1 = new CbfSumEntity();
            return Json(obj1, JsonRequestBehavior.AllowGet);
        }
        public string HoldFileUploadUrlDSA()
        {
            string x = "";
            try
            {
                x = System.Configuration.ConfigurationManager.AppSettings["IEMFileUpload"].ToString();
            }
            catch { x = ""; }
            return (x == null || x.Trim() == "") ? "" : x;
        }
        public FileResult Download(CbfSumEntity obj)
        {

            try
            {
                string txt1 = Session["downfile"].ToString();
                string directory = HoldFileUploadUrlDSA() + txt1.ToString();
                byte[] fileBytes = System.IO.File.ReadAllBytes(directory);
                string fileName = "Download" + txt1.ToString();
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult tab_attach()
        {
            try
            {
                string cbfattchemnet = Request.QueryString["id"].ToString();
                Session["cbfdetails"] = cbfattchemnet;
                CbfSumEntity objMAttachment = new CbfSumEntity();

                if (cbfattchemnet == "")
                {
                    objMAttachment.amoun = cbfattchemnet;
                    objMAttachment = objRep.Attachmentname(objMAttachment);
                }
                else
                {
                    objMAttachment.amoun = cbfattchemnet;
                    objMAttachment = objRep.Attachmentname(objMAttachment);
                }
                objMAttachment.attachmentDate = DateTime.Now.ToShortDateString();
                return PartialView(objMAttachment);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult tab_attachgrid()
        {
            try
            {
                ViewBag.NoRecordsFound = "";
                CbfSumEntity objAttachment = new CbfSumEntity();
                if (Session["cbfdetails"].ToString() == "")
                {
                    objAttachment = (CbfSumEntity)Session["attachment"];
                    if (objAttachment.attachment.Count == 0)
                    {
                        ViewBag.NoRecordsFound = "No Records Found";
                    }
                }
                else
                {
                    objAttachment = (CbfSumEntity)Session["attachmentdetails"];
                    if (objAttachment.attachment.Count == 0)
                    {
                        ViewBag.NoRecordsFound = "No Records Found";
                    }
                }

                return PartialView(objAttachment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult tab_attachgrid(CbfSumEntity objAttachment)
        {
            try
            {
                ViewBag.NoRecordsFound = "";
                CbfSumEntity objAttachment1 = new CbfSumEntity();
                if (ModelState.IsValid)
                {
                    objAttachment.amoun = Session["cbfdetails"].ToString();

                    objAttachment1 = objRep.Attachmentname(objAttachment);
                    if (Session["cbfdetails"].ToString() == "")
                    {
                        Session["attachment"] = objAttachment1;
                        if (objAttachment1.attachment.Count == 0)
                        {
                            ViewBag.NoRecordsFound = "No Records Found";
                        }
                    }
                    else
                    {
                        Session["attachmentdetails"] = objAttachment1;
                        if (objAttachment1.attachment.Count == 0)
                        {
                            ViewBag.NoRecordsFound = "No Records Found";
                        }
                    }

                }
                return Json(objAttachment1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public virtual ActionResult UploadedFiles()
        {
            string filename = "";
            bool isUploaded = false;
            string message = "File upload failed";
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase myFile = Request.Files["boqfile"];
                if (myFile != null && myFile.ContentLength != 0)
                {
                    string pathForSaving = HttpContext.Application["path"] as string;
                    try
                    {
                        filename = Path.GetFileNameWithoutExtension(myFile.FileName);
                        filename = objCmnFunctions.GetSequenceNo("CBF");
                        var fileextension = Path.GetExtension(myFile.FileName);
                        var stream = myFile.InputStream;
                        var path = Path.Combine(HoldFileUploadUrlDSA(), filename + fileextension);
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                        filename = filename + fileextension;
                        Session["filename"] = filename;
                        isUploaded = true;
                        message = "File uploaded successfully!";
                        message = filename;
                    }
                    catch (Exception ex)
                    {
                        message = string.Format("File upload failed: {0}", ex.Message);
                    }
                }
            }
            return Json(new { isUploaded = isUploaded, message = message }, "text/html");
        }
        public JsonResult DeleteAttachment(CbfSumEntity obj)
        {
            ViewBag.NoRecordsFound = "";
            DataTable AttachDelete;
            CbfSumEntity objAttachment1 = new CbfSumEntity();
            if (Session["cbfdetails"].ToString() == "")
            {
                AttachDelete = (DataTable)Session["AccessToken"];
            }
            else
            {
                AttachDelete = (DataTable)Session["AccessTokenheader"];

            }
            if (AttachDelete.Rows.Count > 0)
            {
                for (int i = 0; i < AttachDelete.Rows.Count; i++)
                {
                    if (AttachDelete.Rows[i]["Sno"].ToString() == obj.attachment1)
                    {
                        AttachDelete.Rows.RemoveAt(i);
                        objAttachment1.amoun = Session["cbfdetails"].ToString();
                    }
                }
            }


            objAttachment1 = objRep.Attachmentname(objAttachment1);
            objAttachment1.amoun = Session["cbfdetails"].ToString();

            if (Session["cbfdetails"].ToString() == "")
            {
                Session["attachment"] = objAttachment1;
                if (objAttachment1.attachment.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
            }
            else
            {
                Session["attachmentdetails"] = objAttachment1;
                if (objAttachment1.attachmentCbfdetails.Count == 0)
                {
                    ViewBag.NoRecordsFound = "No Records Found";
                }
            }
            return Json(objAttachment1, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult BoqAttachgridinline()
        {
            try
            {
                ViewBag.NoRecordsFound = "";
                CbfSumEntity objAttachment = new CbfSumEntity();
                if (Session["cbfdetails"].ToString() == "")
                {
                    objAttachment = (CbfSumEntity)Session["attachment"];
                    if (objAttachment.attachment.Count == 0)
                    {
                        ViewBag.NoRecordsFound = "No Records Found";
                    }
                }
                else
                {
                    objAttachment = (CbfSumEntity)Session["attachmentdetails"];
                    if (objAttachment.attachment.Count == 0)
                    {
                        ViewBag.NoRecordsFound = "No Records Found";
                    }
                }

                return PartialView(objAttachment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult BoqAttachgridinline(CbfSumEntity objAttachment)
        {
            try
            {
                ViewBag.NoRecordsFound = "";
                CbfSumEntity objAttachment1 = new CbfSumEntity();
                if (ModelState.IsValid)
                {
                    objAttachment.amoun = Session["cbfdetails"].ToString();
                    if (objAttachment.amoun == "IEM.Areas.FLEXIBUY.Models.CbfSumEntity")
                    {
                        objAttachment.amoun = "";
                    }
                    objAttachment1 = objRep.AttachmentnameInline(objAttachment);
                    if (Session["cbfdetails"].ToString() == "")
                    {
                        Session["attachment"] = objAttachment1;
                        if (objAttachment1.attachment.Count == 0)
                        {
                            ViewBag.NoRecordsFound = "No Records Found";
                        }
                    }
                    else
                    {
                        Session["attachmentdetails"] = objAttachment1;
                        if (objAttachment1.attachment.Count == 0)
                        {
                            ViewBag.NoRecordsFound = "No Records Found";
                        }
                    }

                }
                return Json(objAttachment1, JsonRequestBehavior.AllowGet);
            }



            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult BoqAttached()
        {
            try
            {
                Session["cbfdetails"] = "";
                string cbfattchemnet = Request.QueryString["id"].ToString();
                Session["cbfdetails"] = cbfattchemnet;
                CbfSumEntity objMAttachment = new CbfSumEntity();

                if (cbfattchemnet == "")
                {
                    objMAttachment.amoun = cbfattchemnet;
                    objMAttachment = objRep.AttachmentnameInline(objMAttachment);
                }
                else
                {
                    objMAttachment.amoun = cbfattchemnet;
                    objMAttachment = objRep.AttachmentnameInline(objMAttachment);
                }
                objMAttachment.attachmentDate = DateTime.Now.ToShortDateString();
                return PartialView(objMAttachment);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public PartialViewResult BoqAttachgridinline1()
        {
            try
            {
                Session["cbfdetails"] = "";
                string cbfattchemnet = Request.QueryString["id"].ToString();
                Session["cbfdetails"] = cbfattchemnet;
                CbfSumEntity objMAttachment = new CbfSumEntity();

                if (cbfattchemnet == "")
                {
                    objMAttachment.amoun = cbfattchemnet;
                    objMAttachment = objRep.AttachmentnameInline(objMAttachment);
                }
                else
                {
                    objMAttachment.amoun = cbfattchemnet;
                    objMAttachment = objRep.MyAttachmnetCbfDetails(cbfattchemnet);
                }
                objMAttachment.attachmentDate = DateTime.Now.ToShortDateString();
                return PartialView(objMAttachment);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        //Cbf Delmat

        public JsonResult DelmatApproveForCbf(CbfSummarymodel objcbf)
        {
            try
            {
                string status = objcbf.cbfStatus.ToString();
                quenue objque=new quenue();
                string GetEmployeeDetails = objRep.GetDelmatemployee(objcbf.cbfGid.ToString(), status, objque,objcbf.delmatRemarks);
                ForMailController mailsender = new ForMailController();
                CbfSumModel objMail = new CbfSumModel();
                int refgid = objcbf.cbfGid;
                string reqstatus = objMail.GetRequestStatus(refgid, "CBF");
                int queuegid = objMail.GetQueueGidForMail(refgid, "CBF");
                mailsender.sendusermail("FB", "CBF", Convert.ToString(queuegid), reqstatus, "0");
                return Json(GetEmployeeDetails, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
