using IEM.Areas.FLEXIBUY.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{
    [NoDirectAccess]
    public class CBFCancellationSummaryController : Controller
    {
        private IrepositoryAn objRep;
        public CBFCancellationSummaryController()
            : this(new CbfSumModel())
        {

        }
        public CBFCancellationSummaryController(IrepositoryAn objModel)
        {
            objRep = objModel;
        }

        public ActionResult CbfCancellationSummary()
        {
            CbfSumEntity objSumEntity;
            objSumEntity = objRep.GetCbfcancellationSummary();
            Session["lscbfsmychecker"] = objSumEntity;
            return View(objSumEntity);
        }
        [HttpPost]
        public ActionResult CbfCancellationSummary(string lstxtcbfno, string lstxtcbfdate, string command, string lstxtcbfmode)
        {
            CbfSumEntity obj;

            obj = objRep.GetCbfcancellationSummary();
            if ((string.IsNullOrEmpty(lstxtcbfno)) == false)
            {
                ViewBag.txtcbfno = lstxtcbfno;
                obj.cbfCheckersumamry = obj.cbfCheckersumamry.Where(x => lstxtcbfno == null
                    || (x.cbfNoChecker.Contains(lstxtcbfno))).ToList();
                if (obj.cbfSum.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }
            if ((string.IsNullOrEmpty(lstxtcbfdate)) == false)
            {
                ViewBag.txtcbfdate = lstxtcbfdate;
                obj.cbfCheckersumamry = obj.cbfCheckersumamry.Where(x => lstxtcbfdate == null
                    || (x.cbfDateChecker.Contains(lstxtcbfdate))).ToList();
                if (obj.cbfSum.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }
            if ((string.IsNullOrEmpty(lstxtcbfmode)) == false)
            {
                ViewBag.txtcbfmode = lstxtcbfmode;
                obj.cbfCheckersumamry = obj.cbfCheckersumamry.Where(x => lstxtcbfmode == null || (x.cbfModeChecker.Contains(lstxtcbfmode))).ToList();
                if (obj.cbfSum.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
                ViewBag.cbfmode = lstxtcbfmode;
            }

            if ((string.IsNullOrEmpty(lstxtcbfno)) == true && (string.IsNullOrEmpty(lstxtcbfdate)) == true && (string.IsNullOrEmpty(lstxtcbfmode)) == true)
            {
                ViewBag.Error = "Please fill search Criteria";
            }
            if (command != "SEARCH")
            {
                obj = (CbfSumEntity)Session["cbfsummary"];
            }
            return View(obj);
        }

    }
}
