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
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{
    [NoDirectAccess]
    public class flexibuydashboardController : Controller
    {
        //
        // GET: /FLEXIBUY/Dashboard/

        CmnFunctions objCmnFunctions = new CmnFunctions();
        CommonIUD objuid = new CommonIUD();
        ErrorLog objErrorLog = new ErrorLog();
        // GET: /PrSummary/
        private IRepositoryKIR objrep;


        public flexibuydashboardController()
            : this(new prsummodel())

        { }
        public flexibuydashboardController(IRepositoryKIR objm)
        {
            objrep = objm;
        }

        public ActionResult Index(string redirect)
        {
            Session["FlexiDash"] = null;
            ViewBag.tabIndex = 0;

            if (redirect == "yes")
            {
                Session["DashSearchItems_fb"] = null;
                Session["DashSearchItemsapp_fb"] = null;
            }

            flexibuydashboard dashRequest = new flexibuydashboard();
            dashRequest.DocTypeData = new SelectList(objrep.doctypedata().ToList(), "DocTypeName", "DocTypeName");
            dashRequest.StatusTypeData = new SelectList(objrep.GetStatusType().ToList(), "StatusTypeId", "StatusTypeName");
            ViewBag.DashBoardheaderser = dashRequest;

            flexibuydashboard dashApproval = new flexibuydashboard();
            dashApproval.DocTypeDataApp = new SelectList(objrep.doctypedataapp().ToList(), "DocTypeAppId", "DocTypeAppName");
            dashApproval.StatusTypeApp = new SelectList(objrep.GetStatusTypeapp().ToList(), "StatusTypeAppId", "StatusTypeAppName");
            ViewBag.DashBoardheaderserapp = dashApproval;

            if (Session["DashSearchItems_fb"] != null)
            {
                flexibuydashboard dash = new flexibuydashboard();
                dash = (flexibuydashboard)Session["request"];

                ViewBag.domval = dash.DocTypeName;
                ViewBag.setval = dash.StatusTypeName;
                ViewBag.DashBoardheaderser = dash;
            }
            else
            {
                Session["DashSearchItems_fb"] = null;
            }
            if (Session["DashSearchItemsapp_fb"] != null)
            {
                flexibuydashboard dash = new flexibuydashboard();
                dash = (flexibuydashboard)Session["approval"];

                ViewBag.domvala = dash.DocTypeName;
                ViewBag.setvala = dash.StatusTypeName;
                ViewBag.DashBoardheaderserapp = dash;
            }
            else
            {
                Session["DashSearchItemsapp_fb"] = null;
            }
            return View();
        }

        [HttpPost]
        public ActionResult Index(string txtdbdocdate = null, string txtdbdocdatea = null, string txtdbdocno = null, string txtdbdocamount = null, string ddldbDocType = null, string ddldbDocStatus = null, string command = null)
        {
            ViewBag.tabIndex = 0;
            flexibuydashboard viewbags = new flexibuydashboard();
            List<flexibuydashboard> records = new List<flexibuydashboard>();
            List<flexibuydashboard> records1 = new List<flexibuydashboard>();
            
            if (command == "Search")
            {
                records = objrep.getMyRequest(txtdbdocdate, txtdbdocno, txtdbdocamount, ddldbDocType, ddldbDocStatus).ToList();
                Session["DashSearchItems_fb"] = records;
                Session["DashSearchItemsapp_fb"] = null;
                viewbags.DocTypeData = new SelectList(objrep.doctypedata().ToList(), "DocTypeName", "DocTypeName");
                viewbags.StatusTypeData = new SelectList(objrep.GetStatusType().ToList(), "StatusTypeName", "StatusTypeName");
                viewbags.date = txtdbdocdate;
                viewbags.docNo = txtdbdocno;
                viewbags.amount = txtdbdocamount;
                //viewbags.DocTypeName = ddldbDocType;
                //viewbags.StatusTypeName = ddldbDocStatus;
                ViewBag.DashBoardheaderser = viewbags;
                ViewBag.domval = ddldbDocType;
                ViewBag.setval = ddldbDocStatus;

                flexibuydashboard dashApproval = new flexibuydashboard();
                dashApproval.DocTypeDataApp = new SelectList(objrep.doctypedataapp().ToList(), "DocTypeAppName", "DocTypeAppName");
                dashApproval.StatusTypeApp = new SelectList(objrep.GetStatusTypeapp().ToList(), "StatusTypeAppId", "StatusTypeAppName");
                ViewBag.DashBoardheaderserapp = dashApproval;

                viewbags.DocTypeName = ddldbDocType;
                viewbags.StatusTypeName = ddldbDocStatus;
                Session["request"] = viewbags;

                ViewBag.tabIndex = 1;


            }
            else if (command == "Clear")
            {
                Session["DashSearchItems_fb"] = null;
                Session["DashSearchItemsapp_fb"] = null;
                viewbags.DocTypeData = new SelectList(objrep.doctypedata().ToList(), "DocTypeName", "DocTypeName");
                viewbags.StatusTypeData = new SelectList(objrep.GetStatusType().ToList(), "StatusTypeId", "StatusTypeName");
                viewbags.date = "";
                viewbags.docNo = "";
                viewbags.amount = "";
                ViewBag.DashBoardheaderser = viewbags;

                flexibuydashboard dashApproval = new flexibuydashboard();
                dashApproval.DocTypeDataApp = new SelectList(objrep.doctypedataapp().ToList(), "DocTypeAppName", "DocTypeAppName");
                dashApproval.StatusTypeApp = new SelectList(objrep.GetStatusTypeapp().ToList(), "StatusTypeAppId", "StatusTypeAppName");
                ViewBag.DashBoardheaderserapp = dashApproval;
                Session["request"] = viewbags;

                ViewBag.tabIndex = 1;
                
            }
            else if (command == "Searchapp")
            {
                records1 = objrep.getMyApproval(txtdbdocdatea, txtdbdocno, txtdbdocamount, ddldbDocType, ddldbDocStatus).ToList();
                Session["DashSearchItemsapp_fb"] = records1;
                Session["DashSearchItems_fb"] = null;
                //viewbags.DocTypeData = new SelectList(objrep.doctypedata().ToList(), "DocTypeName", "DocTypeName");
                //viewbags.StatusTypeData = new SelectList(objrep.GetStatusType().ToList(), "StatusTypeId", "StatusTypeName");
                viewbags.DocTypeDataApp = new SelectList(objrep.doctypedataapp().ToList(), "DocTypeAppName", "DocTypeAppName");
                viewbags.StatusTypeApp = new SelectList(objrep.GetStatusTypeapp().ToList(), "StatusTypeAppId", "StatusTypeAppName");
                viewbags.date = txtdbdocdatea;
                viewbags.docNo = txtdbdocno;
                viewbags.amount = txtdbdocamount;
                ViewBag.DashBoardheaderserapp = viewbags;
                ViewBag.domvala = ddldbDocType;
                ViewBag.setvala = ddldbDocStatus;

                flexibuydashboard dashApproval = new flexibuydashboard();
                dashApproval.DocTypeData = new SelectList(objrep.doctypedata().ToList(), "DocTypeName", "DocTypeName");
                dashApproval.StatusTypeData = new SelectList(objrep.GetStatusType().ToList(), "StatusTypeId", "StatusTypeName");
                ViewBag.DashBoardheaderser = dashApproval;

                viewbags.DocTypeName = ddldbDocType;
                viewbags.StatusTypeName = ddldbDocStatus;
                Session["approval"] = viewbags;

                ViewBag.tabIndex = 2;
            }
            else if (command == "Clearapp")
            {
                Session["DashSearchItems_fb"] = null;
                Session["DashSearchItemsapp_fb"] = null;
                viewbags.DocTypeDataApp = new SelectList(objrep.doctypedata().ToList(), "DocTypeAppName", "DocTypeAppName");
                viewbags.StatusTypeApp = new SelectList(objrep.GetStatusType().ToList(), "StatusTypeAppId", "StatusTypeAppName");
                viewbags.date = "";
                viewbags.docNo = "";
                viewbags.amount = "";
                ViewBag.DashBoardheaderserapp = viewbags;
                Session["approval"] = viewbags;

                flexibuydashboard dashApproval = new flexibuydashboard();
                dashApproval.DocTypeData = new SelectList(objrep.doctypedata().ToList(), "DocTypeName", "DocTypeName");
                dashApproval.StatusTypeData = new SelectList(objrep.GetStatusType().ToList(), "StatusTypeId", "StatusTypeName");
                ViewBag.DashBoardheaderser = dashApproval;

                flexibuydashboard dashApproval1 = new flexibuydashboard();
                dashApproval1.DocTypeDataApp = new SelectList(objrep.doctypedataapp().ToList(), "DocTypeAppName", "DocTypeAppName");
                dashApproval1.StatusTypeApp = new SelectList(objrep.GetStatusTypeapp().ToList(), "StatusTypeAppId", "StatusTypeAppName");
                ViewBag.DashBoardheaderserapp = dashApproval1;

                ViewBag.tabIndex = 2;
            }
            return View();
        }


        public ActionResult myDocuments(string Type, string Process)
        {
            List<flexibuydashboard> lstDashDetails = new List<flexibuydashboard>();
            if (Type.Trim() == "PR")
            {
                lstDashDetails = objrep.getDashDetailsPR(Type,Process).ToList();
                Session["FlexiDash"] = lstDashDetails;
                return RedirectToAction("PrSummary1", "PrSummary");
            }
            else if (Type.Trim() == "PAR")
            {
                lstDashDetails = objrep.getDashDetailsPAR(Type, Process).ToList();
                Session["FlexiDash"] = lstDashDetails;
                return RedirectToAction("Index", "ParSummary");
                
            }
            else if (Type.Trim() == "CBF")
            {
                lstDashDetails = objrep.getDashDetailsCBF(Type, Process).ToList();
                Session["FlexiDash"] = lstDashDetails;
                return RedirectToAction("CbfSummaryIndex", "CbfSummary");
            }
            else if (Type.Trim() == "PO")
            {
                lstDashDetails = objrep.getDashDetailsPO(Type, Process).ToList();
                Session["FlexiDash"] = lstDashDetails;
                return RedirectToAction("Index", "Poheader");
            }
            else if (Type.Trim() == "WO")
            {
                lstDashDetails = objrep.getDashDetailsWO(Type, Process).ToList();
                Session["FlexiDash"] = lstDashDetails;
                return RedirectToAction("WoSummary", "WoSummary");
            }
            ViewBag.type = Type;
            return PartialView();
        }

        [HttpGet]
        public ActionResult ForMyApprovalSummaryIndex(string type = "") 
        {
            try
            {
                Session["ForMyApprovalSearch"] = null;
                TempData["type"] = type;
                return View();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }

        }
        [HttpGet]
        public PartialViewResult ForMyApprovalSummaryFields() 
        {
            try
            {
                return PartialView();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView();
            }

        }
        [HttpGet]
        public PartialViewResult ForMyApprovalSummary(string type = "")
        {
            try
            {
                TempData["type"] = type;
                return PartialView();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView();
            }
           
        }
        [HttpPost]
        public JsonResult ForMyApprovalSummary(string RefNo = "",string type="") 
        {
            try
            {
                TempData["type"] = type;
                List<flexibuydashboard> lstDashBoard = new List<flexibuydashboard>();
                lstDashBoard = objrep.getMyApproval().ToList();
                lstDashBoard = lstDashBoard.Where(a => a.category.Equals(type)).ToList();
                if (RefNo != "")
                {
                    lstDashBoard = lstDashBoard.Where(a => a.docNo.ToUpper().Contains(RefNo.ToUpper())).ToList();
                }
                Session["ForMyApprovalSearch"] = lstDashBoard;
                return Json(1,JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(0, JsonRequestBehavior.AllowGet);
            }

        }
    }
}
