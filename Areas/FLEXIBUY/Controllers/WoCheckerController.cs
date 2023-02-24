using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.FLEXIBUY.Models;
using IEM.Areas.MASTERS.Controllers;
using IEM.Common;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{
    [NoDirectAccess]
    public class WoCheckerController : Controller
    {
        //
        // GET: /FLEXIBUY/PoChecker/
        ErrorLog objErrorLog = new ErrorLog();
        ForMailController mailsender = new ForMailController();
        private Irepositorypr objModel;
        public WoCheckerController()
            : this(new fb_DataModelpr())
        {

        }
        public WoCheckerController(Irepositorypr objM)
        {
            objModel = objM;
        }

        [HttpGet]
        public ActionResult WoCheckerSummary()
        {

            WoSummary obj = new WoSummary();
            try
            {
                obj.objWoSummary = objModel.GetWoCheckerSummary();
                if (obj.objWoSummary.Count == 0)
                {
                    ViewBag.records = "No Records Found";
                }
                return View(obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(obj);
            }
        }

        [HttpPost]
        public ActionResult WoCheckerSummary(WoSummary objheader)
        {
            try
            {
                objheader.objWoSummary = objModel.GetWoCheckerSummary().ToList();
                if (objheader != null)
                {
                    objheader.statusname = objModel.getStatusName(objheader.statusgid);
                    if ((string.IsNullOrEmpty(objheader.woDate)) == false)
                    {
                        ViewBag.wodate = objheader.woDate;
                        objheader.objWoSummary = objheader.objWoSummary.Where(x => objheader.woDate == null ||
                            (x.woDate.Contains(objheader.woDate))).ToList();
                    }
                    if ((string.IsNullOrEmpty(objheader.woRefNo)) == false)
                    {
                        ViewBag.worefno = objheader.woRefNo;
                        objheader.objWoSummary = objheader.objWoSummary.Where(x => objheader.woRefNo == null ||
                            (x.woRefNo.Contains(objheader.woRefNo))).ToList();
                    }
                    if (objheader.objWoSummary.Count == 0)
                    {
                        ViewBag.records = "No records Found";
                    }
                }
                return View(objheader);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(objheader);
            }
        }
        [HttpPost]
        public JsonResult WoCheckerApprove(WoSummary objCancel)
        {
            try
            {
                string result = string.Empty;
                if (objCancel.viewCancel == "rejected")
                {
                    result = objModel.WoCheckerApprove(objCancel);
                }
                else
                {
                    result = objModel.GetDelmatemployeeForWo(objCancel);
                }

                if (result == "success" || result == "Success")
                {
                    CbfSumModel objforMail = new CbfSumModel();
                    int refgid = objCancel.woheadGid;
                    string reqstatus = objforMail.GetRequestStatus(refgid, "WO");
                    int queuegid = objforMail.GetQueueGidForMail(refgid, "WO");
                    string curapprovalstage = "0";
                    if (reqstatus == "S")
                    {
                        curapprovalstage = "0";
                    }
                    else
                    {
                        curapprovalstage = "1";
                    }
                    mailsender.sendusermail("FB", "WO", Convert.ToString(queuegid), reqstatus, curapprovalstage);
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult WoDelmatApprove(WoSummary objCancel)
        {
            try
            {
                string result = objModel.GetDelmatemployeeForWo(objCancel);
                if (result == "success" || result == "Success")
                {
                    CbfSumModel objforMail = new CbfSumModel();
                    int refgid = objCancel.woheadGid;
                    string reqstatus = objforMail.GetRequestStatus(refgid, "WO");
                    int queuegid = objforMail.GetQueueGidForMail(refgid, "WO");
                    string curapprovalstage = "0";
                    if (reqstatus == "S")
                    {
                        curapprovalstage = "0";
                    }
                    else
                    {
                        curapprovalstage = "1";
                    }
                    mailsender.sendusermail("FB", "WO", Convert.ToString(queuegid), reqstatus, curapprovalstage);
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
    }
}
