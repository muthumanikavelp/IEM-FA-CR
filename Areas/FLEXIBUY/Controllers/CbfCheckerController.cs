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
    public class CbfCheckerController : Controller
    {
        private IrepositoryAn objRep;
        public CbfCheckerController()
            : this(new CbfSumModel())
        {

        }
        public CbfCheckerController(IrepositoryAn objModel)
        {
            objRep = objModel;
        }
        public ActionResult CbfSummaryChecker()
        {
            CbfSumEntity objSumEntity;
            objSumEntity = objRep.GetCbfSumryChecker();
            Session["lscbfsmychecker"] = objSumEntity;
            return View(objSumEntity);
        }
        [HttpPost]
        public ActionResult CbfSummaryChecker(string lstxtcbfno, string lstxtcbfdate, string command, string lstxtcbfmode)
        {
            CbfSumEntity obj;

            obj = objRep.GetCbfSumryChecker();
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


    }
}
