using IEM.Areas.FLEXIBUY.Models;
using IEM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Models;
namespace IEM.Areas.FLEXIBUY.Controllers
{
    public class CbfClosureController : Controller
    {
        CmnFunctions objCmnFunctions = new CmnFunctions();
        private IrepositoryAn objRep;
        public CbfClosureController()
            : this(new CbfSumModel())
        {

        }
        public CbfClosureController(IrepositoryAn objModel)
        {
            objRep = objModel;
        }
        public ActionResult Cbfclosuresumary()
        {

            List<CbfCheckerSummary> objClosure = new List<CbfCheckerSummary>();
            objClosure = objRep.GetCbfClosureSummary();
            Session["lscbfsmychecker"] = objClosure;
            return View(objClosure);
        }
        [HttpPost]
        public ActionResult CbfCancellationSummary(string lstxtcbfno, string lstxtcbfdate, string command, string lstxtcbfmode)
        {
            CbfSumEntity obj;

            obj = objRep.GetCbfReopenSummary();
            if ((string.IsNullOrEmpty(lstxtcbfno.ToUpper())) == false)
            {
                ViewBag.txtcbfno = lstxtcbfno;
                obj.cbfCheckersumamry = obj.cbfCheckersumamry.Where(x => lstxtcbfno.ToUpper() == null
                    || (x.cbfNoChecker.Contains(lstxtcbfno.ToUpper()))).ToList();
                if (obj.cbfSum.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }
            if ((string.IsNullOrEmpty(lstxtcbfdate)) == false)
            {
                ViewBag.txtcbfdate = lstxtcbfdate;
                obj.cbfCheckersumamry = obj.cbfCheckersumamry.Where(x => lstxtcbfdate.ToUpper() == null
                    || (x.cbfDateChecker.Contains(lstxtcbfdate.ToUpper()))).ToList();
                if (obj.cbfSum.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }
            if ((string.IsNullOrEmpty(lstxtcbfmode.ToUpper())) == false)
            {
                ViewBag.txtcbfmode = lstxtcbfmode;
                obj.cbfCheckersumamry = obj.cbfCheckersumamry.Where(x => lstxtcbfmode.ToUpper() == null || (x.cbfModeChecker.Contains(lstxtcbfmode.ToUpper()))).ToList();
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
        [HttpPost]//
        public ActionResult Cbfclosuresumary(string txtcbfnochecker, string dtecbfdatechecker, string command, string txtcbfmode)
        {
            CbfSumEntity obj;


            obj = objRep.GetCbfClosureSummary1();
            ViewBag.txtcbfno = txtcbfnochecker;
            ViewBag.txtcbfdate = dtecbfdatechecker;
            ViewBag.txtcbfmode = txtcbfmode;
            // 1 Parameters
            if (!string.IsNullOrEmpty(txtcbfnochecker) && string.IsNullOrEmpty(dtecbfdatechecker) && txtcbfmode == "0")
            {

                obj.GetCbfClosureSummary = obj.GetCbfClosureSummary.Where(x => txtcbfnochecker.ToUpper() == null
                    || (x.cbfNoChecker.Contains(txtcbfnochecker.ToUpper()))).ToList();
                if (obj.GetCbfClosureSummary.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }

            if (!string.IsNullOrEmpty(dtecbfdatechecker) && string.IsNullOrEmpty(txtcbfnochecker) && txtcbfmode == "0")
            {

                obj.GetCbfClosureSummary = obj.GetCbfClosureSummary.Where(x => dtecbfdatechecker.ToUpper() == null
                  || (x.cbfDateChecker.Contains(dtecbfdatechecker.ToUpper()))).ToList();
                if (obj.GetCbfClosureSummary.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }

            if (txtcbfmode != "0" && string.IsNullOrEmpty(txtcbfnochecker) && string.IsNullOrEmpty(dtecbfdatechecker))
            {

                obj.GetCbfClosureSummary = obj.GetCbfClosureSummary.Where(x => txtcbfmode.ToUpper() == null
                  || (x.cbfModeChecker.Contains(txtcbfmode.ToUpper()))).ToList();
                if (obj.GetCbfClosureSummary.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }
            //2 Parameters
            if (!string.IsNullOrEmpty(txtcbfnochecker) && (!string.IsNullOrEmpty(dtecbfdatechecker)) && txtcbfmode == "0")
            {
                ViewBag.txtcbfno = txtcbfnochecker;
                obj.GetCbfClosureSummary = obj.GetCbfClosureSummary.Where(x => txtcbfnochecker.ToUpper() == null
                    || (x.cbfNoChecker.Contains(txtcbfnochecker.ToUpper())) && (x.cbfDateChecker.Contains(dtecbfdatechecker.ToUpper()))).ToList();
                if (obj.GetCbfClosureSummary.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }

            if (!string.IsNullOrEmpty(txtcbfnochecker) && (string.IsNullOrEmpty(dtecbfdatechecker)) && txtcbfmode != "0")
            {
                ViewBag.txtcbfdate = dtecbfdatechecker;
                ViewBag.txtcbfno = txtcbfnochecker;
                obj.GetCbfClosureSummary = obj.GetCbfClosureSummary.Where(x => txtcbfnochecker.ToUpper() == null
                    || (x.cbfNoChecker.Contains(txtcbfnochecker.ToUpper())) && (x.cbfModeChecker.Contains(txtcbfmode.ToUpper()))).ToList();
                if (obj.GetCbfClosureSummary.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }

            if (string.IsNullOrEmpty(txtcbfnochecker) && (!string.IsNullOrEmpty(dtecbfdatechecker)) && txtcbfmode != "0")
            {
                ViewBag.txtcbfno = txtcbfnochecker;
                obj.GetCbfClosureSummary = obj.GetCbfClosureSummary.Where(x => txtcbfnochecker.ToUpper() == null
                    || (x.cbfModeChecker.Contains(txtcbfmode.ToUpper())) && (x.cbfDateChecker.Contains(dtecbfdatechecker.ToUpper()))).ToList();
                if (obj.GetCbfClosureSummary.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }

            //3 Parameters
            if ((string.IsNullOrEmpty(txtcbfnochecker.ToUpper())) == false && (string.IsNullOrEmpty(dtecbfdatechecker)) == false && txtcbfmode != "0")
            {
                ViewBag.txtcbfdate = dtecbfdatechecker;
                ViewBag.txtcbfno = txtcbfnochecker;
                ViewBag.txtcbfmode = txtcbfmode;
                obj.GetCbfClosureSummary = obj.GetCbfClosureSummary.Where(x => txtcbfnochecker.ToUpper() == null
                    || (x.cbfNoChecker.Contains(txtcbfnochecker.ToUpper())) && (x.cbfDateChecker.Contains(dtecbfdatechecker.ToUpper())) && (x.cbfModeChecker.Contains(txtcbfmode.ToUpper()))).ToList();
                if (obj.GetCbfClosureSummary.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }


            if ((string.IsNullOrEmpty(txtcbfnochecker)) == true && (string.IsNullOrEmpty(dtecbfdatechecker)) == true && (string.IsNullOrEmpty(txtcbfmode)) == true)
            {
                ViewBag.Error = "Please fill search Criteria";
            }
            if (command != "SEARCH")
            {
                obj = (CbfSumEntity)Session["cbfsummary"];
            }

            List<CbfCheckerSummary> objClosure = new List<CbfCheckerSummary>();

            objClosure = obj.GetCbfClosureSummary;

            return View(objClosure);
        }
      

    }
}
