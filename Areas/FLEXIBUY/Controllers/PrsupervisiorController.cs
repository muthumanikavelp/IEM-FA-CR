using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.UI.WebControls;
using System.Net;
using IEM.Areas.FLEXIBUY.Models;
using IEM.Common;
using System.Data;
using System.Data.SqlClient;
using IEM.Areas.MASTERS.Controllers;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class PrsupervisiorController : Controller
    {
        //
        // GET: /FLEXIBUY/Prsupervisior/
        ErrorLog objErrorLog = new ErrorLog();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        CommonIUD objuid = new CommonIUD();

        // GET: /PrSummary/
        private IRepositoryKIR objrep;


        public PrsupervisiorController()
            : this(new prsummodel())
        {
        }
        public PrsupervisiorController(IRepositoryKIR objm)
        {
            objrep = objm;

        }

        //public PrsupervisiorController(prsupervisior prsupervisior)
        //{
        //    // TODO: Complete member initialization
        //    this.prsupervisior = prsupervisior;
        //}

        //public ActionResult index1()
        //{
        //    Session["prsummary1"] = null;
        //    PrSumEntity obj = new PrSumEntity();

        //    obj = objrep.GetPrsupervisorSumry();

        //    obj.ddlBranch = new SelectList(objrep.GetBranchList(), "branchGid", "branchName");

        //    if (Session["prsummary1"] != null)
        //    {
        //        obj.lstprSum = (List<PrsummaryModel>)Session["prsummary1"];
        //    }
        //    Session["prlist"] = obj.lstprSum;
        //    return View(obj);
        //}
        public ActionResult Prsupervisor()
        {
            Session["prsummary1"] = null;
            PrSumEntity obj = new PrSumEntity();
            try
            {
                obj = objrep.GetPrsupervisorSumry();

                obj.ddlBranch = new SelectList(objrep.GetBranchList(), "branchGid", "branchName");

                if (Session["prsummary1"] != null)
                {
                    obj.lstprSum = (List<PrsummaryModel>)Session["prsummary1"];
                }
                Session["prlist"] = obj.lstprSum;
                return View(obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(obj);
            }
        }

        public ActionResult prsupervisorview()
        {

            return View();

        }

        [HttpPost]
        public ActionResult Prsupervisor(string txtprrefno, string txtprdate, string command, string dropRequestfor, string dropStatus, string dropBranch)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                //    obj = objrep.GetPrsupervisorSumry();
                obj.lstprSum = (List<PrsummaryModel>)Session["prlist"];
                int total = 0;

                if (command == "SEARCH")
                {
                    if ((string.IsNullOrEmpty(txtprrefno)) == false)
                    {

                        //obj = objrep.CheckExistancyPrRefNo(txtprrefno);

                        //total = obj.lstsupervisor.Count;

                        //if (obj.lstsupervisor.Count == 0)
                        //{
                        //    ViewBag.Message = "No records found";
                        //}
                        obj.lstprSum = obj.lstprSum.Where(x => txtprrefno == null || (x.prRefNo.ToUpper().Contains(txtprrefno.ToUpper()))).ToList();
                    }
                    if ((string.IsNullOrEmpty(txtprdate)) == false)
                    {
                        ViewBag.txtprdate = txtprdate;
                        obj.lstprSum = obj.lstprSum.Where(x => txtprdate == null || (x.prDate.Contains(txtprdate))).ToList();
                        Session["prsummary1"] = obj.lstprSum;
                    }
                    //if ((string.IsNullOrEmpty(dropRequestfor)) == false)
                    //{
                    //    obj.requestForGid = Convert.ToInt32(dropRequestfor);

                    //    obj.requestForName = objrep.GetRequestforName(obj.requestForGid);

                    //    ViewBag.txtprreqfor = obj.requestForName;
                    //    obj.lstprSum = obj.lstprSum.Where(x => obj.requestForName == null || (x.prReqFor.Contains(obj.requestForName))).ToList();
                    //    Session["prsummary1"] = obj.lstprSum;
                    //}
                    //if ((string.IsNullOrEmpty(dropStatus)) == false)
                    //{
                    //    obj.statusGid = Convert.ToInt32(dropStatus);
                    //    obj.statusName = objrep.GetStatusName(obj.statusGid);
                    //    ViewBag.txtstatus = obj.statusName;
                    //    obj.lstprSum = obj.lstprSum.Where(x => obj.statusName == null ||
                    //        (x.prStatus.Contains(obj.statusName))).ToList();
                    //    Session["prsummary1"] = obj.lstprSum;
                    //}
                    if ((string.IsNullOrEmpty(dropBranch)) == false)
                    {
                        obj.branchGid = Convert.ToInt32(dropBranch);
                        obj.branchName = objrep.GetBranchName(obj.branchGid);
                        ViewBag.txtbranch = obj.branchName;
                        obj.lstprSum = obj.lstprSum.Where(x => obj.branchName == null ||
                            (x.prBranch.Contains(obj.branchName))).ToList();
                        Session["prsummary1"] = obj.lstprSum;
                    }
                    //if ((string.IsNullOrEmpty(txtprrefno)) == true && (string.IsNullOrEmpty(txtprdate)) == true && (string.IsNullOrEmpty(dropRequestfor)) == true && (string.IsNullOrEmpty(dropStatus)) == true && (string.IsNullOrEmpty(dropBranch)) == true)
                    //{
                    //    ViewBag.Error = "Please fill search Criteria";
                    //}
                    if (obj.lstprSum.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                }
                obj.ddlBranch = new SelectList(objrep.GetBranchList(), "branchGid", "branchName");
                //obj.ddlRequestfor = new SelectList(objrep.GetRequestForList(), "requestForGid", "requestForName");
                //obj.ddlStatus = new SelectList(objrep.GetStatusList(), "statusGid", "statusName");
                if (command != "Refresh")
                {
                    Session["prsummary1"] = null;

                }
                return View(obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(obj);
            }
        }

        public ActionResult index(string id, PrHeader pr)
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
                Session["prgid"] = id;
                Session["rblProductService"] = obj.product.prodserv_Type;
                Session["rblBranchType"] = obj.prHead.prBranchType;
                Session["rblExpenses"] = obj.prHead.prExpense;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View(obj);
            }
            return View(obj);
        }
        public PartialViewResult ViewQuatation(string id)
        {
            PrSumEntity prqutation = new PrSumEntity();
            try
            {
                int prdetgid = Convert.ToInt32(string.IsNullOrEmpty(id)?"0":id);
                prqutation = objrep.ViewInlineAttachmentsPR(prdetgid, "PRDET"); 
                return PartialView(prqutation);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView(prqutation);
            }
        }
        [HttpPost]
        public JsonResult SuperReject(PrHeader objprhead)
        {
            string data = objrep.rejectprsup(objprhead);
            try
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]

        public JsonResult SuperApprove(PrHeader pr)
        {
            //Delmate queue
            try
            {
                string approve = objrep.approvesupp(pr);


                #region "Delmate"

                //  string GetEmployeeDetails = objrep.Getemployeedetails(pr.prGid);
                string GetEmployeeDetails = objrep.GetDelmatemployee(pr);
                #endregion

                ForMailController mailsender = new ForMailController();
                CbfSumModel objMail = new CbfSumModel();
                int refgid = Convert.ToInt32(pr.prGid);
                string reqstatus = objMail.GetRequestStatus(refgid, "PR");
                int queuegid = objMail.GetQueueGidForMail(refgid, "PR");
                mailsender.sendusermail("FB", "PR", Convert.ToString(queuegid), reqstatus, "0");
                return Json(GetEmployeeDetails, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult supervisorraiser(PrHeader pr)
        {
            try
            {
                PrSumEntity obj = new PrSumEntity();
                DataTable temppro = new DataTable();
                DataTable tempser = new DataTable();
                List<Product> lstpro = new List<Product>();
                List<Product> lstser = new List<Product>();
                DataTable dt = new DataTable();

                if (Session["editprotemp"] != null)
                {
                    temppro = (DataTable)Session["editprotemp"];

                    //lstpro = objrep.GetProduct(temppro);
                    obj = objrep.supervisorheader(pr);
                    //obj = objrep.EditPrDetails(lstpro, obj.prDet.prDet_PrHeader_Gid);
                }
                if (Session["editservtemp"] != null)
                {
                    tempser = (DataTable)Session["editservtemp"];

                    lstser = objrep.GetService(tempser);
                    obj = objrep.supervisorheader(pr);
                    //obj = objrep.EditPrDetails(lstser, obj.prDet.prDet_PrHeader_Gid);

                }
                if (Session["editattachment"] != null)
                {
                    PrSumEntity obj1 = new PrSumEntity();
                    obj1 = (PrSumEntity)Session["editattachment"];

                    obj = objrep.InsertAttachment(pr.prRefNo, "PR", obj1.attachment);

                }
                if (Session["Removedrows"] != null)
                {

                    dt = (DataTable)Session["Removedrows"];
                    obj = objrep.DeleteAttachment(dt);

                }

                if (Session["Removedrowsproduct"] != null)
                {

                    dt = (DataTable)Session["Removedrowsproduct"];
                    obj = objrep.DeleteProductServiceDetails(dt);

                }
                if (Session["Removedrowsservice"] != null)
                {
                    dt = (DataTable)Session["Removedrowsservice"];
                    obj = objrep.DeleteProductServiceDetails(dt);

                }

                return Json("1", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

    }
}
