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
    public class PARCheckerSummaryController : Controller
    {
        private IrepositoryAn objrep;
        public PARCheckerSummaryController()
            : this(new CbfSumModel())
        {

        }
        public PARCheckerSummaryController(IrepositoryAn objM)
        {
            objrep = objM;
        }
  
        public ActionResult Index()
        {
            CbfSumEntity obj=new CbfSumEntity ();
            if (Session["parheaderlistnew"] == null)
            {
                obj = objrep.GetParchecker();
                obj.statuslist = new SelectList(objrep.getStatus(), "statusgid", "statusname");
                Session["PARSUMMARY"] = obj;
            }
            else
            {
                obj = (CbfSumEntity)Session["parheaderlistnew"];
            }
           
            return View(obj);

        }
        [HttpPost]
        public ActionResult Index(CbfSumEntity obj,string command, string txtparno, string txtpardate, string txtstatus)
        {
            CbfSumEntity objj = new CbfSumEntity();
            objj = (CbfSumEntity)Session["PARSUMMARY"];
            try
            {
                if(Session["PARSUMMARY"] != null)
                {
                    if (command == "SEARCH")
                    {


                        if ((string.IsNullOrEmpty(txtparno)) == false)
                        {
                            ViewBag.txtparno = txtparno;
                            objj.ListParHeader = objj.ListParHeader.Where(x => txtparno == null ||
                                (x.ParNo.ToUpper().Contains(txtparno.ToUpper()))).ToList();
                            Session["parheaderlistnew"] = objj;
                        }
                        if ((string.IsNullOrEmpty(txtpardate)) == false)
                        {
                            ViewBag.txtpardate = txtpardate;
                            objj.ListParHeader = objj.ListParHeader.Where(x => txtpardate == null ||
                                (x.ParDate.Contains(txtpardate))).ToList();
                            Session["parheaderlistnew"] = objj;
                        }

                        //if ((string.IsNullOrEmpty(obj.statusname)) == false)
                        //{
                        //    ViewBag.statusname = objj.statusname;
                        //    objj.ListParHeader = objj.ListParHeader.Where(x => objj.statusname == null ||
                        //        (x.status.Contains(objj.statusname))).ToList();
                        //    Session["parheaderlist"] = objj;
                        //}
                        if (objj.ListParHeader.Count == 0)
                        {
                            ViewBag.records = "No records Found";
                        }
                    }
                    else if(command=="CLEAR")
                    {
                        objj =  objrep.GetParchecker();
                        Session["parheaderlistnew"] = null;
                    }
                   
                    if (objj.ListParHeader.Count == 0)
                    {
                        ViewBag.records = "No records Found";
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return View(objj);
        }
    }
}
