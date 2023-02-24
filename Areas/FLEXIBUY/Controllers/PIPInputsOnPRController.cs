using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using IEM.Areas.FLEXIBUY.Models;
using IEM.Common;
using System.Configuration;
using IEM.Areas.MASTERS.Controllers;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{
    [NoDirectAccess]
    public class PIPInputsOnPRController : Controller
    {
        private IRepositoryKIR objrep;
        ErrorLog objErrorLog = new ErrorLog();
        public PIPInputsOnPRController()
            : this(new prsummodel())
        { }
        public PIPInputsOnPRController(IRepositoryKIR objm)
        {
            objrep = objm;
        }
        public ActionResult PipInputsOnPr1(string id, PrHeader pr)
        {

            PrSumEntity obj = new PrSumEntity();
            string ss = "View";
            //PrHeader pr = new PrHeader();
            if (id != null)
            {
                Session["id"] = id;
                Session["prgid"] = id;
                pr.prGid = id;
                Session["AccessToken1"] = null;
                Session["AccessTokenheader1"] = null;
                obj = objrep.ViewPrDetails(pr, ss);

                obj.lstprDet = objrep.ViewPipPrDetails(id);
                //obj.ddlBranch = new SelectList(objrep.GetBranchList(), "branchGid", "branchName",obj.prHead.prBranch);
                //obj.ddlFCCC = new SelectList(objrep.GetFcccList(), "fcccGid", "fcccName",obj.prHead.prFcc);
                //obj.ddlRequestfor = new SelectList(objrep.GetRequestForList(), "requestForGid", "requestForName",obj.prHead.prReqFor);
                //obj.ddlBranch = new SelectList(objrep.GetBranchList(), "branchGid", "branchName");
                //obj.ddlFCCC = new SelectList(objrep.GetFcccList(), "fcccGid", "fcccName");
                //obj.ddlRequestfor = new SelectList(objrep.GetRequestForList(), "requestForGid", "requestForName");
                obj.ddlBranch = new SelectList(objrep.GetBranchList(), "branchGid", "branchName", obj.branchGid);
                obj.ddlFCCC = new SelectList(objrep.GetFcccList(), "fcccGid", "fcccName", obj.fcccGid);
                obj.ddlRequestfor = new SelectList(objrep.GetRequestForList(), "requestForGid", "requestForName", obj.requestForGid);
                Session["prdetLst"] = obj.lstprDet;
                //Session["Requestfor"] = pr.prReqFor;
            }
            return View(obj);
        }
        //[HttpPost]
        //public JsonResult PipInputsOnPr1(PrHeader prheader)
        //{

        //    PrSumEntity obj = new PrSumEntity();
        //    prheader.prRefNo = Session["id"].ToString();
        //    // prheader.prRefNo = Session["id"].ToString();
        //    //pr.prRefNo = Session["id"].ToString();
        //    List<PrDetails> lstprDet = (List<PrDetails>)Session["prdetLst"];
        //    //string prreqno = "";
        //    //prreqno = prheader.prReqFor;
        //    obj = objrep.PipUpdateEstimate(prheader.prRefNo, lstprDet);
        //    if (Session["pipattachmentList"] != null)
        //    {
        //        PrSumEntity attachLst = new PrSumEntity();
        //        attachLst = (PrSumEntity)Session["pipattachmentList"];

        //        obj = objrep.InsertAttachment(prheader.prRefNo, "PIP", attachLst.attachment);

        //    }

        //    //obj = objrep.ViewPrDetails(pr.prHead, "View");
        //    //obj.lstprDet = objrep.ViewPipPrDetails(pr.prHead.prRefNo);
        //    string data = "";

        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}
        [HttpPost]
        public JsonResult Pipsave(PrHeader prheader)
        {
            string Result = string.Empty;
            int gid = 0;
            string data = string.Empty;
            PrSumEntity obj = new PrSumEntity();
            prheader.prGid = Session["id"].ToString();
            // prheader.prRefNo = Session["id"].ToString();
            //pr.prRefNo = Session["id"].ToString();
            List<PrDetails> lstprDet = (List<PrDetails>)Session["prdetLst"];
            for(int i=0;i<lstprDet.Count;i++)
            {
              if(lstprDet[i].prDet_CostEstimation==0)
              {
                  data = "failed";
                 
              }

            }
            
            //string prreqno = "";
            //prreqno = prheader.prReqFor;
            if (data != "failed")
            {
                obj = objrep.PipUpdateEstimate(prheader.prGid, prheader.prRefNo, lstprDet);
                gid = objrep.getempid(prheader.prRefNo);
                if (Session["AccessTokenheader1"] != null)
                {
                    DataTable dtnew = new DataTable();
                    List<Attachment> lst = new List<Attachment>();
                    dtnew = (DataTable)Session["AccessTokenheader1"];
                    if (dtnew.Rows.Count > 0)
                    {
                        string attachmenttype = "quotation";
                        for (int i = 0; i < dtnew.Rows.Count; i++)
                        {
                            int prdetgid = Convert.ToInt32(string.IsNullOrEmpty(dtnew.Rows[i]["prdetails"].ToString()) ? "0" : dtnew.Rows[i]["prdetails"].ToString());
                            PrSumEntity objPRSumEntity = new PrSumEntity();
                            objPRSumEntity.attachment1 = string.IsNullOrEmpty(dtnew.Rows[i]["Documnet_Name"].ToString()) ? dtnew.Rows[i]["Filename"].ToString() : dtnew.Rows[i]["Documnet_Name"].ToString();
                            objPRSumEntity.MyFile = "PRDET";
                            objPRSumEntity.attachmentDesc = string.IsNullOrEmpty(dtnew.Rows[i]["Document_des"].ToString()) ? dtnew.Rows[i]["AttachmentDescription"].ToString() : dtnew.Rows[i]["Document_des"].ToString();

                            objrep.InsertPRAttachmentEditNew(objPRSumEntity, prdetgid, attachmenttype);
                        }
                    }
                }
                 
                if (Session["pipattachmentList"] != null)
                {
                    PrSumEntity attachLst = new PrSumEntity();
                    attachLst = (PrSumEntity)Session["pipattachmentList"];

                    obj = objrep.InsertAttachment(prheader.prRefNo, "PIP", attachLst.attachment);

                }
                obj = objrep.pipheader(prheader);
                Result = objrep.Insertqueue(gid, "SUP",prheader);
                ForMailController mailsender = new ForMailController();
                CbfSumModel objMail = new CbfSumModel();
                int refgid = Convert.ToInt32(prheader.prGid);
                string reqstatus = objMail.GetRequestStatus(refgid, "PR");
                int queuegid = objMail.GetQueueGidForMail(refgid, "PR");
                mailsender.sendusermail("FB", "PR", Convert.ToString(queuegid), reqstatus, "0");

                data = "Success Fully Inserted";
            }
           
            //obj = objrep.ViewPrDetails(pr.prHead, "View");
            //obj.lstprDet = objrep.ViewPipPrDetails(pr.prHead.prRefNo);
          

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult viewpipattachment(string id)
        {
            PrSumEntity obj = new PrSumEntity();

            obj.attachment = objrep.getattachmentdetails(id, "PR");

            return PartialView(obj);
        }
        [HttpGet]
        public PartialViewResult getprdetailsforpip()
        {
            PrSumEntity objgetprpipdetails = new PrSumEntity();

            PrHeader pr = new PrHeader();
            pr.prRefNo = Session["id"].ToString();

            objgetprpipdetails.prHead = pr;
            objgetprpipdetails.lstprDet = (List<PrDetails>)Session["prdetLst"];
            return PartialView(objgetprpipdetails);
        }

        [HttpPost]
        public PartialViewResult getprdetailsforpip(PrDetails PrDetailModel)
        {
            try
            {

                PrSumEntity objgetprpipdetails = new PrSumEntity();
                List<PrDetails> prdetLst1 = (List<PrDetails>)Session["prdetLst"];

                int n = prdetLst1.Count;
               

                for (int i = 0; i < n; i++)
                {
                    if (prdetLst1[i].srNo == PrDetailModel.srNo)
                    {
                        prdetLst1[i].prDet_Qty = PrDetailModel.prDet_Qty;
                        prdetLst1[i].prDet_Rate = PrDetailModel.prDet_Rate;
                        prdetLst1[i].prDet_CostEstimation = PrDetailModel.prDet_CostEstimation;
                      
                    }


                }

                objgetprpipdetails.lstprDet = prdetLst1;


                Session["prdetLst"] = objgetprpipdetails.lstprDet;

                PrHeader pr = new PrHeader();
                pr.prRefNo = Session["id"].ToString();

                objgetprpipdetails.prHead = pr;

                objgetprpipdetails.ddlBranch = new SelectList(objrep.GetBranchList(), "branchGid", "branchName", objgetprpipdetails.branchGid);
                objgetprpipdetails.ddlFCCC = new SelectList(objrep.GetFcccList(), "fcccGid", "fcccName", objgetprpipdetails.fcccGid);
                objgetprpipdetails.ddlRequestfor = new SelectList(objrep.GetRequestForList(), "requestForGid", "requestForName", objgetprpipdetails.requestForGid);


                //pr.prBranch = new SelectList(objrep.GetBranchList(), "branchGid", "branchName" );
                //obj.ddlFCCC = new SelectList(objrep.GetFcccList(), "fcccGid", "fcccName", obj.fcccGid);
                //obj.ddlRequestfor = new SelectList(objrep.GetRequestForList(), "requestForGid", "requestForName", obj.requestForGid);

                return PartialView("pipinputsonpr1", objgetprpipdetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public JsonResult Downloaded(PrSumEntity prattachmentmodel)
        {
            Session["downfile"] = prattachmentmodel.attachment1;
            PrSumEntity obj = new PrSumEntity();
            return Json(obj, JsonRequestBehavior.AllowGet);
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
        public FileResult Download(string ff)
        {

            string txt1 = Session["downfile"].ToString();
            string directory = HoldFileUploadUrlDSA() + txt1.ToString();
            byte[] fileBytes = System.IO.File.ReadAllBytes(directory);
            string fileName = "Download" + txt1.ToString();
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        [HttpPost]
        public JsonResult CheckApproverIsExists(int refgid = 0,string refname="")  
        {
            try
            {
             //   int refflag = Convert.ToInt32(ConfigurationManager.AppSettings["prrefFlag"].ToString());
                var isExistsApprover = objrep.IsExistingApprover(refgid, refname);
                return Json(isExistsApprover, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(2, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult PIPRejectPR(PrHeader prheader)  
        {
            try
            {
                int result = objrep.RejectPIPInputsOnPR(prheader);
                if (result == 1)
                {
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
    }
}