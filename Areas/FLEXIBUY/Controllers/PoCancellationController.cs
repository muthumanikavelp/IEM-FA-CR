using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.FLEXIBUY.Models;
using IEM.Models;

namespace IEM.Areas.FLEXIBUY.Controllers
{
    public class PoCancellationController : Controller
    {
        //
        // GET: /FLEXIBUY/PoCancellation/
        private Irepositorypr objModel;
        public PoCancellationController()
            : this(new fb_DataModelpr())
        {

        }
        public PoCancellationController(Irepositorypr objM)
        {
            objModel = objM;
        }

        [HttpGet]
        public ActionResult PoCheckerSummary()
        {
            poRaising obj = new poRaising();
            obj.objposummary = objModel.GetCancelPoSummaryDetails();
            if(obj.objposummary.Count>0)
            { 
            obj.remarks = obj.objposummary[0].remarks;
            obj.poCancelGid = obj.objposummary[0].poCancelGid;
            }
            if (obj.objposummary.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            return View(obj);
        }

        [HttpPost]
        public ActionResult PoCheckerSummary(poRaising objheader,string command)
        {

            objheader.objposummary = objModel.GetCancelPoSummaryDetails().ToList();
            if (command == "search")
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
                 if (objheader.objposummary.Count == 0)
                {
                    ViewBag.records = "No records Found";
                }
            return View(objheader);
        }
        [HttpPost]
        public JsonResult PoCancelApprove(poRaising objCancel)
        {
            try
            {
                objCancel.result = objModel.PoCancelApprove(objCancel);
                if(objCancel.result!=null)
                {
                    return Json(objCancel, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                throw ex;
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
