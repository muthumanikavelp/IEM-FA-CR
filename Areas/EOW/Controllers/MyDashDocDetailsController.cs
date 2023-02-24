using IEM.Areas.EOW.Models;
using IEM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using IEM.App_Start;
namespace IEM.Areas.EOW.Controllers
{

    [NoDirectAccess]
    public class MyDashDocDetailsController : Controller
    {
        //
        // GET: /EOW/MyDashDocDetails/
        private EOW_IRepository objModel;
        CmnFunctions objCmnFunctions = new CmnFunctions();
        CommonIUD objCommonIUD = new CommonIUD();

        public MyDashDocDetailsController()
            : this(new EOW_DataModel())
        {

        }
        public MyDashDocDetailsController(EOW_IRepository objM)
        {
            objModel = objM;
        }
        [HttpGet]
        public ActionResult MyDocDetails(string queuefor, string requesttype, string reqstatus)
        {
            Session["requesttype"] = "";
            Session["reqstatus"] = "";
            ViewBag.requesttypehd = requesttype;
            ViewBag.reqstatushd = reqstatus;
            ViewBag.queueforhd = queuefor;
            Session["requesttype"] = requesttype;
            Session["reqstatus"] = reqstatus;
            return View();
        }
        [HttpPost]
        public ActionResult MyDocDetails(string txtdbdocno, string txtdbdocdate, string txtdbdocamount, string type)
        {
            DashBoard records = new DashBoard();
            List<DashBoard> lstDashBoard = new List<DashBoard>();
            lstDashBoard = (List<DashBoard>)Session["Searchlst"];
            if (!String.IsNullOrEmpty(txtdbdocno))
            {
                lstDashBoard = lstDashBoard.Where(x => txtdbdocno == null ||
                          (x.Docno.Contains(txtdbdocno))).ToList();
            }
            if (!String.IsNullOrEmpty(txtdbdocdate))
            {
                lstDashBoard = lstDashBoard.Where(x => x.DocDate == txtdbdocdate).ToList();
            }
            if (!String.IsNullOrEmpty(txtdbdocamount))
            {
                lstDashBoard = lstDashBoard.Where(x => x.Docamount == txtdbdocamount).ToList();
            }
            ViewBag.txtdbdocdate = txtdbdocdate;
            ViewBag.txtdbdocno = txtdbdocno;
            ViewBag.txtdbdocamount = txtdbdocamount;
            return View(lstDashBoard);
        }

        public ActionResult FSDashboardLink(string EcfGid = null, string EcfStatus = null)
        {
            int EcfID = 0;
            //if (EcfStatus == "Paid")
            //{
            //    int QueueGid = Convert.ToInt32(EcfGid);
            //    EcfID = Convert.ToInt32(objModel.GetEcfGid(QueueGid));
            //}
            //else
            //{
            EcfID = Convert.ToInt32(EcfGid);
            //}

            int DocSubTypeGid = Convert.ToInt32(objModel.GetEcfSubTypeGid(EcfID));
            Session["EOWFS_Status"] = Convert.ToInt32(EcfStatus);
            return RedirectToAction("ViewECF", "ECFStatus", new { id = EcfID, subId = DocSubTypeGid, area = "FLEXISPEND" });
        }
        [HttpGet]
        public ActionResult ManualECF()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ToGetmanualecfquery()
        {
            List<DashBoard> lstDashBoard = new List<DashBoard>();
            lstDashBoard = objModel.Getmanualecfdetails(objCmnFunctions.GetLoginUserGid().ToString()).ToList();
            var jsonResult = Json(new { qrylist = lstDashBoard }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetAutodisplayofNextItem()
        {
            string requesttype = "", reqstatus = "";
            string result = "";
            Int32 ecfgid = 0;
            try
            {               
                if (Session["requesttype"] != null && Session["reqstatus"] != null)
                {
                    requesttype = Session["requesttype"].ToString();
                    reqstatus = Session["reqstatus"].ToString();
                }
                Session["docAppoalc"] = "app";
                DashBoard objDashBoard = new DashBoard();
                List<DashBoard> lstDashBoard = new List<DashBoard>();
                if (requesttype == "EMPLOYEE CLAIMS")
                {
                    Session["printflag"] = "E";
                    ViewBag.printflagd = "E";
                    if (reqstatus == "rejected")
                    {
                        lstDashBoard = objModel.GetMydocReject(objCmnFunctions.GetLoginUserGid().ToString(), "1").ToList();
                    }
                    if (reqstatus == "forapproval")
                    {
                        lstDashBoard = objModel.GetDashBoardMyAppval(objCmnFunctions.GetLoginUserGid().ToString(), "1").ToList();
                    }
                }
                else if (requesttype == "SUPPLIER INVOICE")
                {
                    Session["printflag"] = "S";
                    ViewBag.printflagd = "S";
                    if (reqstatus == "rejected")
                    {
                        lstDashBoard = objModel.GetMydocReject(objCmnFunctions.GetLoginUserGid().ToString(), "3").ToList();
                    }
                    if (reqstatus == "forapproval")
                    {
                        lstDashBoard = objModel.GetDashBoardMyAppval(objCmnFunctions.GetLoginUserGid().ToString(), "3").ToList();
                    }
                }
                else if (requesttype == "ADVANCE REQUEST")
                {
                    Session["printflag"] = "A";
                    ViewBag.printflagd = "A";
                    if (reqstatus == "rejected")
                    {
                        lstDashBoard = objModel.GetMydocReject(objCmnFunctions.GetLoginUserGid().ToString(), "2").ToList();
                    }
                    if (reqstatus == "forapproval")
                    {
                        lstDashBoard = objModel.GetDashBoardMyAppval(objCmnFunctions.GetLoginUserGid().ToString(), "2").ToList();
                    }

                }
                else if (requesttype == "UtilityPayments")
                {
                    lstDashBoard = objModel.GetDashBoardUtlity(objCmnFunctions.GetLoginUserGid().ToString(), "D").ToList();
                }
                else
                {
                    lstDashBoard = objModel.GetDashBoardDetailsa(objCmnFunctions.GetLoginUserGid().ToString()).ToList();
                    Session["ToGetMyRequestapp"] = lstDashBoard; 
                }
                if (lstDashBoard.Count == 0)
                {
                    result = "No Record";
                }
                else
                {
                    ecfgid = Convert.ToInt32(lstDashBoard[0].Docnogid);
                    result = "success";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return Json(ecfgid, result, JsonRequestBehavior.AllowGet);
            //var jsonResult = Json(new {result, ecfgid  }, JsonRequestBehavior.AllowGet);
            //return jsonResult;
        }

        [HttpGet]
        public JsonResult ToGetecfdoccount(string queuefor, string requesttype, string reqstatus)
        {
            Session["docAppoalc"] = "app";
            DashBoard objDashBoard = new DashBoard();
            List<DashBoard> lstDashBoard = new List<DashBoard>();
            if (requesttype == "EMPLOYEE CLAIMS")
            {
                Session["printflag"] = "E";
                ViewBag.printflagd = "E";
                if (reqstatus == "draft")
                {
                    lstDashBoard = objModel.GetMydocDraft(objCmnFunctions.GetLoginUserGid().ToString(), "1").ToList();
                }
                if (reqstatus == "rejected")
                {
                    lstDashBoard = objModel.GetMydocReject(objCmnFunctions.GetLoginUserGid().ToString(), "1").ToList();
                }
                if (reqstatus == "forapproval")
                {
                    lstDashBoard = objModel.GetDashBoardMyAppval(objCmnFunctions.GetLoginUserGid().ToString(), "1").ToList();
                }
                if (reqstatus == "inprocess")
                {
                    lstDashBoard = objModel.GetMydocAppoalc(objCmnFunctions.GetLoginUserGid().ToString(), "1").ToList();
                }
                if (reqstatus == "approved")
                {
                    lstDashBoard = objModel.GetMydocAppred(objCmnFunctions.GetLoginUserGid().ToString(), "1").ToList();
                }
                if (reqstatus == "Cancelled")
                {
                    lstDashBoard = objModel.GetMydocCancelled(objCmnFunctions.GetLoginUserGid().ToString(), "1").ToList();
                }
                if (reqstatus == "EPUInprocess")
                {
                    lstDashBoard = objModel.GetEPUInprocessCount(objCmnFunctions.GetLoginUserGid().ToString(), "1").ToList();
                }
                if (reqstatus == "EPURejected")
                {
                    lstDashBoard = objModel.GetEPURejectedCount(objCmnFunctions.GetLoginUserGid().ToString(), "1").ToList();
                }
                if (reqstatus == "Paid")
                {
                    lstDashBoard = objModel.GetPaidCount(objCmnFunctions.GetLoginUserGid().ToString(), "1").ToList();
                }
            }
            if (requesttype == "SUPPLIER INVOICE")
            {
                Session["printflag"] = "S";
                ViewBag.printflagd = "S";
                if (reqstatus == "draft")
                {
                    lstDashBoard = objModel.GetMydocDraft(objCmnFunctions.GetLoginUserGid().ToString(), "3").ToList();
                }
                if (reqstatus == "rejected")
                {
                    lstDashBoard = objModel.GetMydocReject(objCmnFunctions.GetLoginUserGid().ToString(), "3").ToList();
                }
                if (reqstatus == "forapproval")
                {
                    lstDashBoard = objModel.GetDashBoardMyAppval(objCmnFunctions.GetLoginUserGid().ToString(), "3").ToList();
                }
                if (reqstatus == "inprocess")
                {
                    lstDashBoard = objModel.GetMydocAppoalc(objCmnFunctions.GetLoginUserGid().ToString(), "3").ToList();
                }
                if (reqstatus == "approved")
                {
                    lstDashBoard = objModel.GetMydocAppred(objCmnFunctions.GetLoginUserGid().ToString(), "3").ToList();
                }
                if (reqstatus == "Cancelled")
                {
                    lstDashBoard = objModel.GetMydocCancelled(objCmnFunctions.GetLoginUserGid().ToString(), "3").ToList();
                }
                if (reqstatus == "EPUInprocess")
                {
                    lstDashBoard = objModel.GetEPUInprocessCount(objCmnFunctions.GetLoginUserGid().ToString(), "3").ToList();
                }
                if (reqstatus == "EPURejected")
                {
                    lstDashBoard = objModel.GetEPURejectedCount(objCmnFunctions.GetLoginUserGid().ToString(), "3").ToList();
                }
                if (reqstatus == "Paid")
                {
                    lstDashBoard = objModel.GetPaidCount(objCmnFunctions.GetLoginUserGid().ToString(), "3").ToList();
                }
            }
            if (requesttype == "ADVANCE REQUEST")
            {
                Session["printflag"] = "A";
                ViewBag.printflagd = "A";
                if (reqstatus == "draft")
                {
                    lstDashBoard = objModel.GetMydocDraft(objCmnFunctions.GetLoginUserGid().ToString(), "2").ToList();
                }
                if (reqstatus == "rejected")
                {
                    lstDashBoard = objModel.GetMydocReject(objCmnFunctions.GetLoginUserGid().ToString(), "2").ToList();
                }
                if (reqstatus == "forapproval")
                {
                    lstDashBoard = objModel.GetDashBoardMyAppval(objCmnFunctions.GetLoginUserGid().ToString(), "2").ToList();
                }
                if (reqstatus == "inprocess")
                {
                    lstDashBoard = objModel.GetMydocAppoalc(objCmnFunctions.GetLoginUserGid().ToString(), "2").ToList();
                }
                if (reqstatus == "approved")
                {
                    lstDashBoard = objModel.GetMydocAppred(objCmnFunctions.GetLoginUserGid().ToString(), "2").ToList();
                }
                if (reqstatus == "Cancelled")
                {
                    lstDashBoard = objModel.GetMydocCancelled(objCmnFunctions.GetLoginUserGid().ToString(), "2").ToList();
                }
                if (reqstatus == "EPUInprocess")
                {
                    lstDashBoard = objModel.GetEPUInprocessCount(objCmnFunctions.GetLoginUserGid().ToString(), "2").ToList();
                }
                if (reqstatus == "EPURejected")
                {
                    lstDashBoard = objModel.GetEPURejectedCount(objCmnFunctions.GetLoginUserGid().ToString(), "2").ToList();
                }
                if (reqstatus == "Paid")
                {
                    lstDashBoard = objModel.GetPaidCount(objCmnFunctions.GetLoginUserGid().ToString(), "2").ToList();
                }
            }
            if (requesttype == "UtilityPayments")
            {
                lstDashBoard = objModel.GetDashBoardUtlity(objCmnFunctions.GetLoginUserGid().ToString(), "D").ToList();
            }


            if (requesttype == "INSURANCE")
            {
                Session["printflag"] = "I";
                ViewBag.printflagd = "I";
                if (reqstatus == "draft")
                {
                    lstDashBoard = objModel.GetMydocDraft(objCmnFunctions.GetLoginUserGid().ToString(), "4").ToList();
                }
                if (reqstatus == "rejected")
                {
                    lstDashBoard = objModel.GetMydocReject(objCmnFunctions.GetLoginUserGid().ToString(), "4").ToList();
                }
                if (reqstatus == "forapproval")
                {
                    lstDashBoard = objModel.GetDashBoardMyAppval(objCmnFunctions.GetLoginUserGid().ToString(), "4").ToList();
                }
                if (reqstatus == "inprocess")
                {
                    lstDashBoard = objModel.GetMydocAppoalc(objCmnFunctions.GetLoginUserGid().ToString(), "4").ToList();
                }
                if (reqstatus == "approved")
                {
                    lstDashBoard = objModel.GetMydocAppred(objCmnFunctions.GetLoginUserGid().ToString(), "4").ToList();
                }
                if (reqstatus == "Cancelled")
                {
                    lstDashBoard = objModel.GetMydocCancelled(objCmnFunctions.GetLoginUserGid().ToString(), "4").ToList();
                }
                if (reqstatus == "EPUInprocess")
                {
                    lstDashBoard = objModel.GetEPUInprocessCount(objCmnFunctions.GetLoginUserGid().ToString(), "4").ToList();
                }
                if (reqstatus == "EPURejected")
                {
                    lstDashBoard = objModel.GetEPURejectedCount(objCmnFunctions.GetLoginUserGid().ToString(), "4").ToList();
                }
                if (reqstatus == "Paid")
                {
                    lstDashBoard = objModel.GetPaidCount(objCmnFunctions.GetLoginUserGid().ToString(), "4").ToList();
                }
            }

            if (requesttype == "INSURANCE ADVANCE")
            {
                Session["printflag"] = "I";
                ViewBag.printflagd = "I";
                if (reqstatus == "draft")
                {
                    lstDashBoard = objModel.GetMydocDraft(objCmnFunctions.GetLoginUserGid().ToString(), "5").ToList();
                }
                if (reqstatus == "rejected")
                {
                    lstDashBoard = objModel.GetMydocReject(objCmnFunctions.GetLoginUserGid().ToString(), "5").ToList();
                }
                if (reqstatus == "forapproval")
                {
                    lstDashBoard = objModel.GetDashBoardMyAppval(objCmnFunctions.GetLoginUserGid().ToString(), "5").ToList();
                }
                if (reqstatus == "inprocess")
                {
                    lstDashBoard = objModel.GetMydocAppoalc(objCmnFunctions.GetLoginUserGid().ToString(), "5").ToList();
                }
                if (reqstatus == "approved")
                {
                    lstDashBoard = objModel.GetMydocAppred(objCmnFunctions.GetLoginUserGid().ToString(), "5").ToList();
                }
                if (reqstatus == "Cancelled")
                {
                    lstDashBoard = objModel.GetMydocCancelled(objCmnFunctions.GetLoginUserGid().ToString(), "5").ToList();
                }
                if (reqstatus == "EPUInprocess")
                {
                   lstDashBoard = objModel.GetEPUInprocessCount(objCmnFunctions.GetLoginUserGid().ToString(), "5").ToList();
                }
                if (reqstatus == "EPURejected")
                {
                    lstDashBoard = objModel.GetEPURejectedCount(objCmnFunctions.GetLoginUserGid().ToString(), "5").ToList();
                }
                if (reqstatus == "Paid")
                {
                    lstDashBoard = objModel.GetPaidCount(objCmnFunctions.GetLoginUserGid().ToString(), "5").ToList();
                }
            }


            var jsonResult = Json(new { qrylist = lstDashBoard }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }
}
