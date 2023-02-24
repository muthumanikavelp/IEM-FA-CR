using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using IEM.Areas.FLEXIBUY.Models;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class PIPITInputSummaryController : Controller
    {
        private IRepositoryKIR objrep;

        public PIPITInputSummaryController()
            : this(new prsummodel())
        { }
        public PIPITInputSummaryController(IRepositoryKIR objm)
        {
            objrep = objm;
        }

        public ActionResult PipSummary()
        {
            string status = string.Empty;
            PrSumEntity obj = new PrSumEntity();
            status = objrep.Getpipemp();
            if(status=="Y")
            {
                obj = objrep.GetPIPSumry();
            }
            else
            {
                obj.lstprSum = Enumerable.Empty<PrsummaryModel>().ToList<PrsummaryModel>();
            }
          
            obj.ddlBranch = new SelectList(objrep.GetBranchList(), "branchgid", "branchName");
            Session["prsummarypip"] = obj;
            return View(obj);
        }

        [HttpPost]
        public ActionResult PipSummary(string txtprrefno, string command, string txtprdate, string dropBranch)
        {
            PrSumEntity obj;
            obj = objrep.GetPIPSumry();
            Session["pipsummary"] = null;
            obj.ddlBranch = new SelectList(objrep.GetBranchList(), "branchgid", "branchName");

            if ((string.IsNullOrEmpty(txtprrefno)) == false)
            {

              //  obj = objrep.CheckExistancyPrRefNo(txtprrefno);

                ViewBag.txtprrefno = txtprrefno;
                obj.lstprSum = obj.lstprSum.Where(x => txtprrefno == null || (x.prRefNo.Contains(txtprrefno))).ToList();
                Session["pipsummary"] = obj.lstprSum;
                if (obj.lstprSum.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }
            if ((string.IsNullOrEmpty(txtprdate)) == false)
            {
                ViewBag.txtprdate = txtprdate;
                obj.lstprSum = obj.lstprSum.Where(x => txtprdate == null || (x.prDate.Contains(txtprdate))).ToList();
                Session["pipsummary"] = obj.lstprSum;
                if (obj.lstprSum.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }
            if ((string.IsNullOrEmpty(dropBranch)) == false)
            {
                obj.branchGid = Convert.ToInt32(dropBranch);
                obj.branchName = objrep.GetBranchName(obj.branchGid);
                ViewBag.txtbranch = obj.branchName;
                obj.lstprSum = obj.lstprSum.Where(x => obj.branchName == null ||
                    (x.prBranch.Contains(obj.branchName))).ToList();
                Session["pipsummary"] = obj.lstprSum;
                if (obj.lstprSum.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }
            //if ((string.IsNullOrEmpty(txtprrefno)) == true && (string.IsNullOrEmpty(txtprdate)) == true && (string.IsNullOrEmpty(dropBranch)) == true)
            //{
            //    ViewBag.Error = "Please fill search Criteria";
            //}
            if (command != "SEARCH")
            {
               obj = (PrSumEntity)Session["prsummarypip"];
                //if (Session["pipsummary"] != null)
                //{
                //    obj.lstprSum = (List<PrsummaryModel>)Session["pipsummary"];
                //}
            }
            return View(obj);
        }



    }
}
