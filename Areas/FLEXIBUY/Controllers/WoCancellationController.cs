using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.FLEXIBUY.Models;
using IEM.Common;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class WoCancellationController : Controller
    {
        //
        // GET: /FLEXIBUY/PoCancellation/
        ErrorLog objErrorLog = new ErrorLog();
        private Irepositorypr objModel;
        public WoCancellationController()
            : this(new fb_DataModelpr())
        {

        }
        public WoCancellationController(Irepositorypr objM)
        {
            objModel = objM;
        }

        [HttpGet]
        public ActionResult WoCancelCheckerSummary()
        {
            WoSummary obj = new WoSummary();
            try { 
            obj.objWoSummary = objModel.GetCancelWoSummaryDetails();
            if (obj.objWoSummary.Count > 0)
            {
                obj.remarks = obj.objWoSummary[0].remarks;
                obj.woCancelGid = obj.objWoSummary[0].woCancelGid;
            }
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
        public ActionResult WoCancelCheckerSummary(WoSummary objheader, string command)
        {
            try { 
            objheader.objWoSummary = objModel.GetCancelWoSummaryDetails().ToList();
            if (command == "search")
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
                    ViewBag.porefno = objheader.woRefNo;
                    objheader.objWoSummary = objheader.objWoSummary.Where(x => objheader.woRefNo == null ||
                        (x.woRefNo.Contains(objheader.woRefNo))).ToList();
                }
                if (objheader.objWoSummary.Count == 0)
                {
                    ViewBag.records = "No records Found";
                }
            }
            if (objheader.objWoSummary.Count == 0)
            {
                ViewBag.records = "No records Found";
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
        public JsonResult WoCancelApprove(WoSummary objCancel)
        {
            try
            {
                objCancel.result = objModel.WoCancelApprove(objCancel);
                if (objCancel.result != null)
                {
                    return Json(objCancel, JsonRequestBehavior.AllowGet);
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
        //public JsonResult PoCancelReject(poRaising objCancel)
        //{
        //    try
        //    {
        //        objCancel.result = objModel.PoCancelReject(objCancel);
        //        if (objCancel.result!=null)
        //        {
        //            return Json(objCancel, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json(0, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

    }
}
