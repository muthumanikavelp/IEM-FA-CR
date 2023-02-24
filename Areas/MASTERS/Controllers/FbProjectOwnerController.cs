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
    public class FbProjectOwnerController : Controller
    {
        proLib plib = new proLib();
        dbLib dblib = new dbLib();

        //
        // GET: /Project/
        private FbIrepository objModel;
        public FbProjectOwnerController()
            : this(new FbDataModel())
        {

        }
        public FbProjectOwnerController(FbIrepository objM)
        {
            objModel = objM;
        }
        [HttpGet]
        public ActionResult Index()
        {
            Session["ExcelExportProject"] = null;
            ProjectOwner obj = new ProjectOwner();
            obj.lListProjOwner=objModel.GetProjectOwner();
            return View(obj);
        }
        [HttpPost]
        public ActionResult Index(string projownername, string command)
        {
            ProjectOwner objSearch = new ProjectOwner();
            objSearch.lListProjOwner= objModel.GetProjectOwner();
            if (command == "search")
            {
                if ((string.IsNullOrEmpty(projownername)) == false)
                {
                    ViewBag.projownername = projownername;
                    objSearch.lListProjOwner = objSearch.lListProjOwner.Where(x => projownername == null ||
                        (x.projOwner.ToUpper().Contains(projownername.ToUpper()))).ToList();
                }
                if (objSearch.lListProjOwner.Count == 0)
                {
                    ViewBag.records = "No records Found";
                }
            }
            return View(objSearch);
        }
        [HttpGet]
        public PartialViewResult Create(ProjectOwner objowner)
        {
            ProjectOwner obj = new ProjectOwner();
            return PartialView();
        }

        [HttpPost]
        public JsonResult Create(ProjectOwner objowner, string empName, string empCode, string employeeGid)
        {
            objowner.result = objModel.InsertProjOwnerDetails(objowner, empName, empCode, employeeGid);
            if (!string.IsNullOrEmpty(objowner.result))
            {
                return Json(objowner.result, JsonRequestBehavior.AllowGet);
            }
            return Json(0, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult searchEmp(string listfor)
        {
            ProjectOwner obj = new ProjectOwner();
            //List<ProjectOwner> objowner = new List<ProjectOwner>();
            if(listfor=="search")
            {
                if (Session["objowner"] != null)
                { 
                    obj.lListProjOwner = (List<ProjectOwner>)Session["objowner"];
                }
                else
                {
                    obj.lListProjOwner = objModel.GetEmployeeDetails();
                }
                if (TempData["Norecords"] != null)
                    TempData["records"] = TempData["Norecords"];
                if (TempData["empcode"] != null)
                    TempData["code"] = TempData["empcode"];
                if (TempData["empname"] != null)
                    TempData["name"] = TempData["empname"];
            }
            else
            {
                Session["objowner"] = "";
                obj.lListProjOwner = objModel.GetEmployeeDetails();
            }
            return PartialView("searchEmp", obj);
        }
        [HttpPost]
        public JsonResult searchEmp(ProjectOwner objownersearch)
        {
           // List<ProjectOwner> objowner = new List<ProjectOwner>();
            ProjectOwner objSearch = new ProjectOwner();
            objSearch.lListProjOwner = objModel.GetEmployeeDetails();
           // Session["objowner"] = null;
            if (objownersearch != null)
            {
                if ((string.IsNullOrEmpty(objownersearch.empCode)) == false)
                {
                    TempData["empcode"] = objownersearch.empCode;
                    objSearch.lListProjOwner = objSearch.lListProjOwner.Where(x => objownersearch.empCode == null ||
                        (x.empCode.ToUpper().Contains(objownersearch.empCode.ToUpper()))).ToList();
                    Session["objowner"] = objSearch.lListProjOwner;
                }
                if ((string.IsNullOrEmpty(objownersearch.empName)) == false)
                {
                    TempData["empname"] = objownersearch.empName;
                    objSearch.lListProjOwner = objSearch.lListProjOwner.Where(x => objownersearch.empName == null ||
                        (x.empName.ToUpper().Contains(objownersearch.empName.ToUpper()))).ToList();
                    Session["objowner"] = objSearch.lListProjOwner;
                }
                if (objSearch.lListProjOwner.Count == 0)
                {
                    TempData["Norecords"] = "No records Found";
                }
            }
           // return PartialView("searchEmp", objowner);
            return Json(objSearch, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult DeleteOwner(int id)
         {
             try
              {
                  ProjectOwner obj = new ProjectOwner();
                  obj.projOwner=objModel.GetProjectOwnerById(id);
                  obj.projOwnerGid = id;
                  return PartialView(obj);
              }
             catch(Exception ex)
              {
                    throw ex;
              }
         }
        [HttpPost]
        public JsonResult DeleteOwnerDetails(ProjectOwner objDeleteID)
        {
            try
            {
                objModel.DeleteProjectOwnerDetails(objDeleteID);
                return Json(objDeleteID, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Employee Auto Complete
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
            DataSet ds = (DataSet)Session["ExcelExportProject"];
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
