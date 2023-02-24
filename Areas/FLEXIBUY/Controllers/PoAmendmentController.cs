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
    public class PoAmendmentController : Controller
    {
        //
        // GET: /FLEXIBUY/PoAmendment/
        private Irepositorypr objModel;
        public PoAmendmentController()
            : this(new fb_DataModelpr())
        {

        }
        public PoAmendmentController(Irepositorypr objM)
        {
            objModel = objM;
        }

        [HttpGet]
        public ActionResult PoAmendSummary()
        {
            poRaising obj = new poRaising();
            obj.objposummary = objModel.GetAmendSummary();
            if (obj.objposummary.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            return View(obj);
        }
        [HttpPost]
        public ActionResult PoAmendSummary(poRaising objheader, string command)
        {

            objheader.objposummary = objModel.GetAmendSummary();
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
    }
}
