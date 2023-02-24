using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.MASTERS.Models;
using IEM.Helper;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using IEM.App_Start;
 
namespace IEM.Areas.MASTERS.Controllers
{
     
     [NoDirectAccess]
    public class FbBudgetOwnerController : Controller
    {
        proLib plib = new proLib();
        dbLib dblib = new dbLib();

        //
        // GET: /fb_mst_budgetowner/
        private FbIrepository objModel;
        public FbBudgetOwnerController()
            : this(new FbDataModel())
        {

        }
        public FbBudgetOwnerController(FbIrepository objM)
        {
            objModel = objM;
        }
        public ActionResult Index()
        {
            Session["ExcelExportBudget"] = null;
            BudgetOwner obj = new BudgetOwner();
            obj.lListBudgOwner = objModel.GetBudgetOwner();
            return View(obj);
        }

        [HttpPost]
        public ActionResult Index(string budgetownername, string command)
        {
            BudgetOwner objSearch = new BudgetOwner();
            objSearch.lListBudgOwner = objModel.GetBudgetOwner();
            if (command == "search")
            {
                if ((string.IsNullOrEmpty(budgetownername)) == false)
                {
                    ViewBag.budgetownername = budgetownername;
                    objSearch.lListBudgOwner = objSearch.lListBudgOwner.Where(x => budgetownername == null ||
                        (x.BudgOwner.ToUpper().Contains(budgetownername.ToUpper()))).ToList();
                }
                if (objSearch.lListBudgOwner.Count == 0)
                {
                    ViewBag.records = "No records Found";
                }
            }
            return View(objSearch);
        }
        [HttpGet]
        public PartialViewResult Create(BudgetOwner objowner)
        {
            BudgetOwner obj = new BudgetOwner();
            return PartialView();
        }

        [HttpPost]
        public JsonResult Create(BudgetOwner objowner, string empName, string empCode, string employeeGid)
        {
            objowner.result = objModel.InsertBudgOwnerDetails  (objowner, empName, empCode, employeeGid);
            if (!string.IsNullOrEmpty(objowner.result))
            {
                return Json(objowner.result, JsonRequestBehavior.AllowGet);
            }
          
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult searchEmployee(string listfor)
        {
            BudgetOwner obj = new BudgetOwner();
            if (listfor == "search")
            {
                if (Session["objowner"] != null)
                { 
                    obj.lListBudgOwner = (List<BudgetOwner>)Session["objowner"];
                }
                else
                {
                    obj.lListBudgOwner = objModel.GetEmpDetails();
                }
                if (TempData["Norecords"] != null)
                    TempData["records"] = TempData["Norecords"];
                if (TempData["Empcode"] != null)
                    TempData["empcode"] = TempData["Empcode"];
                if (TempData["EmpName"] != null)
                    TempData["empname"] = TempData["EmpName"];
            }
            else
            {
                Session["objowner"] = "";
                obj.lListBudgOwner = objModel.GetEmpDetails();
            }
            return PartialView("searchEmployee", obj);
        }

        [HttpPost]
        public JsonResult searchEmployee(BudgetOwner objbudgetsearch)
        {
            BudgetOwner objSearch = new BudgetOwner();
            objSearch.lListBudgOwner = objModel.GetEmpDetails();
            Session["objowner"] = null;
            if (objbudgetsearch != null)
            {
                if ((string.IsNullOrEmpty(objbudgetsearch.empCode)) == false)
                {
                    TempData["Empcode"]= objbudgetsearch.empCode;
                    objSearch.lListBudgOwner = objSearch.lListBudgOwner.Where(x => objbudgetsearch.empCode == null ||
                        (x.empCode.ToUpper().Contains(objbudgetsearch.empCode.ToUpper()))).ToList();
                    Session["objowner"] = objSearch.lListBudgOwner;
                }
                if ((string.IsNullOrEmpty(objbudgetsearch.empName)) == false)
                {
                    TempData["EmpName"] = objbudgetsearch.empName;
                    objSearch.lListBudgOwner = objSearch.lListBudgOwner.Where(x => objbudgetsearch.empName == null ||
                        (x.empName.ToUpper().Contains(objbudgetsearch.empName.ToUpper()))).ToList();
                    Session["objowner"] = objSearch.lListBudgOwner;
                }
                if (objSearch.lListBudgOwner.Count == 0)
                {
                    TempData["Norecords"] = "No records Found";
                }
            }
            //return PartialView("searchEmployee", objowner);
            return Json(objSearch, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult DeleteBudgetOwner(int id)
        {
            try
            {
                BudgetOwner obj = new BudgetOwner();
                obj.BudgOwner = objModel.GetBudgetOwnerById(id);
                obj.BudgetOwnergid = id;
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult Delete(BudgetOwner objDeleteID)
        {
            try
            {
                objModel.DeletebBudgetOwnerDetails(objDeleteID);
                return Json(objDeleteID, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult GetAutoCompleteEmployeeAdd(string txt)
        {
            try { return Json(GetAutoCompleteEmployeeAdd(txt, plib.LoginUserId), JsonRequestBehavior.AllowGet); }
            catch { return null; }
        }

        //Employee Auto Complete
        public List<string> GetAutoCompleteEmployeeAdd(string txt, string _UsrId)
        {
            try
            {
                List<string> result = new List<string>();
                DataSet ds = dblib.GetAutoComplete(txt, "13", _UsrId);
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        result.Add(string.Format("{0}~{1}", dr[0], dr[1]));
                    }
                }
                ds.Dispose();
                return result;
            }
            catch { return null; }
        }
        [HttpPost]
        public JsonResult DownloadExcel(string ViewType, string FileName)
        {

            DataSet ds = (DataSet)Session["ExcelExportBudget"];
            DataTable _downloadingData = ds.Tables[0];
            if (_downloadingData != null)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_downloadingData);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename= " + FileName + ".xls");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

    }
}
