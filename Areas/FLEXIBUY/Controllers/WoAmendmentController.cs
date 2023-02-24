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
    public class WoAmendmentController : Controller
    {
        //
        // GET: /FLEXIBUY/PoAmendment/
        ErrorLog objErrorLog = new ErrorLog();
        private Irepositorypr objModel;
        public WoAmendmentController()
            : this(new fb_DataModelpr())
        {

        }
        public WoAmendmentController(Irepositorypr objM)
        {
            objModel = objM;
        }

        [HttpGet]
        public ActionResult WoAmendSummary()
        {
            WoSummary obj = new WoSummary();
            try
            {
                obj.objWoSummary = objModel.GetWoAmendSummary();
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
        public ActionResult WoAmendSummary(WoSummary objheader, string command)
        {
            try
            {
                objheader.objWoSummary = objModel.GetWoAmendSummary();
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

    }
}
