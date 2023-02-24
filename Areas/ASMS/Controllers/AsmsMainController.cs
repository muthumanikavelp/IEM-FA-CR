using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.ASMS.Models;
using IEM.Common;
using IEM.App_Start;
namespace IEM.Areas.ASMS.Controllers
{

      [NoDirectAccess]
    public class AsmsMainController : Controller
    {
        CmnFunctions objCmnFunctions = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        private IRepository objModel;

        public AsmsMainController()
            :this(new SupDataModel()) 
        {

         }
        public AsmsMainController(IRepository objM)
        {
            objModel = objM; 
        }
        //
        // GET: /ASMS/AsmsMain/

        public ActionResult Index()
        {
            Session["isApproved"] = "";
            Session["ChkEmpIsFinanceReviewer"] = null;
            Session["PageMode"] = null;
            Session["queuefor"] = null;
            Session["SupCode"] = null;
            Session["isFinancialReviewer"] = null;
            objModel.DeleteInvalidDirectors();
            Session["SupplierHeaderGid"] = "0";
            return View();
        }
        public ActionResult DashBoard()
        {
            try
            {
                Session["isApproved"] = "";
                Session["ChkEmpIsFinanceReviewer"] = null;
                Session["PageMode"] = null;
                Session["queuefor"] = null;
                Session["SupCode"] = null;
                Session["isFinancialReviewer"] = null;
                Session["SupplierHeaderGid"] = null;
                List<DashBoard> objdashboardModel = new List<DashBoard>();
                objdashboardModel = objModel.GetDashboardForMyApproval().ToList();
                return View(objdashboardModel);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                List<DashBoard> objdashboardModel1 = new List<DashBoard>();
                return View(objdashboardModel1); 
            }
            
        }
        [HttpGet]
        public PartialViewResult StatusGridDetails()  
        {
            return PartialView();
        }
        [HttpGet]
        public PartialViewResult MyRequestsGrid() 
        {
            return PartialView();
        }
        [HttpGet]
        public PartialViewResult MyApprovalsGrid() 
        {
            return PartialView();
        }
        [HttpGet]
        public PartialViewResult FinancialReviewIndex() 
        {
            Session["ChkEmpIsFinanceReviewer"] = null;
            var EmpRoleGroup = objModel.ChkEmpIsFinanceReviewer();
            Session["ChkEmpIsFinanceReviewer"] = EmpRoleGroup;
            return PartialView();
        }
        [HttpGet]
        public PartialViewResult FinancialReview(string listfor) 
        {
            try
            {
                List<SupplierHeader> lst = new List<SupplierHeader>();
                if (listfor == "search")
                {
                    if (Session["SearchFinancialReviewSummary"] != null)
                    {
                        lst = (List<SupplierHeader>)Session["SearchFinancialReviewSummary"];
                    }
                }
                else
                {
                        lst = objModel.GetFinancialReviewSummary().ToList();
                 }

                return PartialView(lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult FinancialReview(SupplierHeader objSupHeader)  
        {
            try
            {
                var res = 0; ;
                List<SupplierHeader> objEmp = new List<SupplierHeader>();
                
                objEmp = objModel.GetFinancialReviewSummary().ToList();
               
                if (objEmp != null)
                {
                    if ((string.IsNullOrEmpty(objSupHeader._SupplierCode)) == false)
                    {
                        objEmp = objEmp.Where(x => objSupHeader._SupplierCode == null ||
                            (x._SupplierCode.ToUpper().Contains(objSupHeader._SupplierCode.ToUpper()))).ToList();
                        Session["SearchFinancialReviewSummary"] = objEmp;
                    }
                    if ((string.IsNullOrEmpty(objSupHeader._SupplierName)) == false)
                    {
                        objEmp = objEmp.Where(x => objSupHeader._SupplierName == null ||
                            (x._SupplierName.ToUpper().Contains(objSupHeader._SupplierName.ToUpper()))).ToList();
                        Session["SearchFinancialReviewSummary"] = objEmp;
                    }
                }
                if (objEmp.Count() == 0)
                {
                    res = 0;
                }
                else
                {
                    res = 1;
                }
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
