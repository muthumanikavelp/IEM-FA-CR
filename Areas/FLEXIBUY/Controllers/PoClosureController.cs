using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.FLEXIBUY.Models;
using IEM.Models;
using IEM.App_Start;

namespace IEM.Areas.FLEXIBUY.Controllers
{
    [NoDirectAccess]
    public class PoClosureController : Controller
    {
        //
        // GET: /FLEXIBUY/PoClosure/
        private Irepositorypr objModel;
        public PoClosureController()
            : this(new fb_DataModelpr())
        {

        }
        public PoClosureController(Irepositorypr objM)
        {
            objModel = objM;
        }

        public ActionResult PoClosureSummary()
        {
            poRaising obj = new poRaising();
            obj.objposummary = objModel.GetPoClosureSummary();
            if (obj.objposummary.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            return View(obj);
        }
        [HttpPost]
        public ActionResult PoClosureSummary(poRaising objheader, string command)
        {

            objheader.objposummary = objModel.GetPoClosureSummary().ToList();
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
        public JsonResult PoSummaryClosure(poraiser objdel)
        {
            try
            {
                if (objdel.poheadGid != null)
                    objdel.result = objModel.ClosurePoSummaryDetails(objdel);
                if (objdel.result != null)
                {
                    return Json(objdel, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
