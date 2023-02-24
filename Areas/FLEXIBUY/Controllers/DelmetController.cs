using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls;
using System.Net;
using System.Threading.Tasks;
using IEM.Areas.FLEXIBUY.Models;
using IEM.Common;
using IEM.Areas.MASTERS.Controllers;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class DelmetController : Controller
    {
        //
        // GET: /FLEXIBUY/Delmet/


        private IRepositoryKIR objrep;


        public DelmetController()
            : this(new prsummodel())

        { }
        public DelmetController(IRepositoryKIR objm)
        {
            objrep = objm;
        }

        public ActionResult Delmate()
        {
            prsupervisior pobj = new prsupervisior();


            if (Session["searchdetails"] ==null)
            {
                pobj.prdetailslist = objrep.GetdelmateApprovalqueue();
                Session["prdetails"] = pobj.prdetailslist;
            }
            else
            {
                pobj.prdetailslist = (List<prsupervisior>)Session["searchdetails"];
            }
          
            return View(pobj);
        }

        public ActionResult DelmateView(string id, PrHeader pr)
        {
            PrSumEntity obj = new PrSumEntity();

            try
            {
                string ss = "View";
                if (id != null && id != "")
                {
                    pr.prGid = id;
                    obj = objrep.supervisorViewPrDetails(pr, ss);
                    obj.prHead.prGid = id;
                }
                Session["id"] = id;
                Session["rblProductService"] = obj.product.prodserv_Type;
                Session["rblBranchType"] = obj.prHead.prBranchType;
                Session["rblExpenses"] = obj.prHead.prExpense;
            }
            catch (Exception ex)
            {

            }
            return View(obj);
        }

        [HttpPost]
        public ActionResult Delmate(prsupervisior probj, string refresh)
        {
            probj.prdetailslist =(List<prsupervisior>)Session["prdetails"];
          
            try
            {
                if (refresh != "refresh")
                {
                    if(!string.IsNullOrEmpty(probj.prRefNo))
                    {
                        probj.prdetailslist = probj.prdetailslist.Where(x => probj.prRefNo == null || (x.prRefNo.ToUpper().Contains(probj.prRefNo.ToUpper()))).ToList();
                        Session["searchdetails"] = probj.prdetailslist;
                    }
                    else if(!string.IsNullOrEmpty(probj.prDate))
                    {
                        probj.prdetailslist = probj.prdetailslist.Where(x => probj.prDate == null || (x.prDate.Contains(probj.prDate))).ToList();
                        Session["searchdetails"] = probj.prdetailslist;
                    }
                }
                else
                {
                    Session["searchdetails"] = null;
                }
           
            }
            catch (Exception)
            {

            }
            return View(probj);
        }

        [HttpPost]

        public JsonResult SuperApprove(PrHeader pr)
        {
            //Delmate queue
            //praveen
            //string approve = objrep.approvesupp(pr);
            
            #region "Delmate"

           //string GetEmployeeDetails = objrep.Getemployeedetails(pr.prRefNo);
           string GetEmployeeDetails = objrep.GetDelmatemployee(pr);
           Session["searchdetails"] = null;
            #endregion

           ForMailController mailsender = new ForMailController();
           CbfSumModel objMail = new CbfSumModel();
           int refgid = Convert.ToInt32(pr.prGid);
           string reqstatus = objMail.GetRequestStatus(refgid, "PR");
           int queuegid = objMail.GetQueueGidForMail(refgid, "PR");
           mailsender.sendusermail("FB", "PR", Convert.ToString(queuegid), reqstatus, "0");
           return Json(GetEmployeeDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SuperReject(PrHeader prhead)
        {
            string data = objrep.rejectprsup(prhead);
            ForMailController mailsender = new ForMailController();
            CbfSumModel objMail = new CbfSumModel();
            int refgid = objMail.GetRefGidForMail();
            string reqstatus = objMail.GetRequestStatus(refgid, "PR");
            int queuegid = objMail.GetQueueGidForMail(refgid, "PR");
            mailsender.sendusermail("FB", "PR", Convert.ToString(queuegid), reqstatus, "0");
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        //public ActionResult Delmetapprovalforpo()
        //{
        //    return View();
        //}

        //public ActionResult Delmetapprovalforposummary()
        //{
        //    return View();
        //}

    }
}
