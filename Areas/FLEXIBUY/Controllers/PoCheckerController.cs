using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.FLEXIBUY.Models;
using IEM.Areas.MASTERS.Controllers;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class PoCheckerController : Controller
    {
        //
        // GET: /FLEXIBUY/PoChecker/
        ForMailController mailsender = new ForMailController();
        private Irepositorypr objModel;
        public PoCheckerController()
            : this(new fb_DataModelpr())
        {

        }
        public PoCheckerController(Irepositorypr objM)
        {
            objModel = objM;
        }

        [HttpGet]
        public ActionResult PoCheckerSummary()
        {
            poRaising obj = new poRaising();
            obj.objposummary = objModel.GetPoCheckerSummary();
            if (obj.objposummary.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            return View(obj);
        }

        [HttpPost]
        public ActionResult PoCheckerSummary(poRaising objheader)
        {

            objheader.objposummary = objModel.getpoSummary().ToList();
            if (objheader != null)
            {
                objheader.statusname = objModel.getStatusName(objheader.statusgid);
                if ((string.IsNullOrEmpty(objheader.poDate)) == false)
                {
                    ViewBag.podate = objheader.poDate;
                    objheader.objposummary = objheader.objposummary.Where(x => objheader.poDate == null ||
                        (x.poDate.Contains(objheader.poDate))).ToList();
                }
                if ((string.IsNullOrEmpty(objheader.poRefNo)) == false)
                {
                    ViewBag.porefno = objheader.poRefNo;
                    objheader.objposummary = objheader.objposummary.Where(x => objheader.poRefNo == null ||
                        (x.poRefNo.Contains(objheader.poRefNo))).ToList();
                }
                if (objheader.objposummary.Count == 0)
                {
                    ViewBag.records = "No records Found";
                }
            }
            return View(objheader);
        }
        [HttpPost]
        public JsonResult PoCheckerApprove(poRaising objCancel)
        {
            try
            {
              //  string prhgid;
                string result = string.Empty;

              //  prhgid = objCancel.poheadGid.ToString();
               // result = objModel.PoCheckerApprove(objCancel);
                if (objCancel.viewCancel == "rejected")
                {
                    result = objModel.PoCheckerApprove(objCancel);
                }
                else
                {
                    result = objModel.GetDelmatemployee(objCancel);
                }
                if (result == "success" || result=="Success")
                {
                    CbfSumModel objforMail = new CbfSumModel();
                    int refgid = objCancel.poheadGid;
                    string reqstatus = objforMail.GetRequestStatus(refgid, "PO");
                    int queuegid = objforMail.GetQueueGidForMail(refgid, "PO");
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
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult PoDelmatApprove(poRaising objCancel)
        {
            try
            {
                
                string result = objModel.GetDelmatemployee(objCancel);
                if (result == "success" || result == "Success")
                {
                    CbfSumModel objforMail = new CbfSumModel();
                    int refgid = objCancel.poheadGid;
                    string reqstatus = objforMail.GetRequestStatus(refgid, "PO");
                    int queuegid = objforMail.GetQueueGidForMail(refgid, "PO");
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
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
