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
    public class WoClosureCheckerController : Controller
    {
        //
        // GET: /FLEXIBUY/PoClosureChecker/
        ErrorLog objErrorLog = new ErrorLog();
        private Irepositorypr objModel;
        public WoClosureCheckerController()
            : this(new fb_DataModelpr())
        {

        }
        public WoClosureCheckerController(Irepositorypr objM)
        {
            objModel = objM;
        }
        public ActionResult WoClosureCheckerSummary()
        {
            WoSummary obj = new WoSummary();
            try
            {
                obj.objWoSummary = objModel.GetWoClosureCheckerSummary();
                if (obj.objWoSummary.Count > 0)
                {
                    obj.remarks = obj.objWoSummary[0].remarks;
                    obj.woclosureGid = obj.objWoSummary[0].woclosureGid;
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
        public ActionResult WoClosureCheckerSummary(WoSummary objheader, string command)
        {
            try
            {
                objheader.objWoSummary = objModel.GetWoClosureCheckerSummary().ToList();
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
                        ViewBag.worefno = objheader.woRefNo;
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
        public JsonResult WoSummaryClosure(woraiser obj)
        {
            try
            {
                if (obj.woheadGid != null)
                    obj.result = objModel.ClosureWoSummaryInsert(obj);
                if (obj.result != null)
                {
                    return Json(obj, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
