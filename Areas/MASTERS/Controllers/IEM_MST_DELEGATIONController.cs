using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.MASTERS.Models;
using IEM.Common;
using IEM.Areas.EOW.Models;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using IEM.App_Start;
namespace IEM.Areas.MASTERS.Controllers
{

    [NoDirectAccess]
    public class IEM_MST_DELEGATIONController : Controller
    {
        //
        // GET: /MASTERS/IEM_MST_DELEGATION/
        String result = String.Empty;
        CentraldataModel sub = new CentraldataModel();
        CentralModel cen = new CentralModel();
         private Iiem_mst_delegation delegation;
        CmnFunctions com = new CmnFunctions();       
        public IEM_MST_DELEGATIONController() :
            this(new IEM_MST_DELEGATION()) { }

        public IEM_MST_DELEGATIONController(Iiem_mst_delegation Objist) 
        {
            delegation = Objist;
        }
        public ActionResult Index()
        {
            Session["ExcelExportDelegation"] = null;
            List<iem_mst_delegation> g = new List<iem_mst_delegation>();
            iem_mst_delegation Typemodel = new iem_mst_delegation();
            Typemodel.GetDelmattype = new SelectList(delegation.GetDelmattype().ToList(), "delmattypename", "delmattypename");
            ViewBag.DelmatypeName = Typemodel;
            g = delegation.GetDelegation().ToList();
            //Typemodel.delegate_gid = g[0].delegate_gid;
            //Typemodel.delegate_bygid = g[0].delegate_bygid;

            if(g.Count==0)
            {
                ViewBag.Message = "No Record";
            }
            return View(g);
        }   
        [HttpPost]
        public ActionResult Index(string filter_delegate_by, string filter_delegate_to, string period_from, string period_to, string Delmat, string command = null)
        {
            List<iem_mst_delegation> g = new List<iem_mst_delegation>();
            iem_mst_delegation Typemodel = new iem_mst_delegation();
            Typemodel.GetDelmattype = new SelectList(delegation.GetDelmattype().ToList(), "delmattypename", "delmattypename");
            ViewBag.DelmatypeName = Typemodel;

            ViewBag.DelegateByGid = Typemodel.delegate_bygid;
            ViewBag.DelegatePeriodFrom = Typemodel.delegate_period_from;

            if(command=="Refresh" || command=="Search")
            {
                ViewBag.delegate_by = filter_delegate_by;
                ViewBag.delegate_to = filter_delegate_to;
                ViewBag.period_from = period_from;
                ViewBag.period_to = period_to;
                ViewBag.delmatype = Delmat;
                g = delegation.GetDelegation(filter_delegate_by, filter_delegate_to, period_from, period_to, Delmat).ToList();
                if (g.Count == 0)
                {
                    ViewBag.Message = "No Record";
                }
            }
            else
            {
                g = delegation.GetDelegation().ToList();
            }

            return View(g);
        }
        [HttpGet]
        public PartialViewResult Create()
        {
            iem_mst_delegation Typemodel = new iem_mst_delegation();
            Typemodel.GetDelmattype = new SelectList(delegation.GetDelmattype().ToList(), "delmatypegid", "delmattypename");
            ViewBag.DelmatypeName = Typemodel;
            return PartialView(Typemodel);
        }
        [HttpGet]
        public PartialViewResult EmployeeSearch(string listfor)
        {

            try
            {
                List<CentraldataModel> obj = new List<CentraldataModel>();
                CentraldataModel Getvaluesearchvalue = new CentraldataModel();
                if (listfor == "search")
                {
                    if (Session["SearchEmployeedata"] != null)
                    {
                        Getvaluesearchvalue = (CentraldataModel)Session["SerchViewbag"];
                        @ViewBag.EmployeeName = Getvaluesearchvalue.RaiserName;
                        @ViewBag.EmployeeCode = Getvaluesearchvalue.RaiserCode;
                        obj = (List<CentraldataModel>)Session["SearchEmployeedata"];
                    }
                }
                else
                {
                    Session["SearchEmployeedata"] = null;
                    Session["SerchViewbag"] = null;
                    obj = cen.SelectEmployeeDelegation().ToList();
                }
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
 
        [HttpPost]
        public JsonResult EmployeeSearchdelegation(string RaiserName = "", string RaiserCode = "")
        {
            List<CentraldataModel> recordsearch = new List<CentraldataModel>();
            CentraldataModel emp = new CentraldataModel();
            if (RaiserName != "" || RaiserCode != "")
            {
                @ViewBag.EmployeeName = RaiserName;
                @ViewBag.EmployeeCode = RaiserCode;
                emp.RaiserCode = RaiserCode;
                emp.RaiserName = RaiserName;
                Session["SerchViewbag"] = emp;
                recordsearch = cen.SelectEmployeeSearch(RaiserName, RaiserCode).ToList();
                Session["SearchEmployeedata"] = recordsearch;
            }
            else
            {
                Session["SerchViewbag"] = null;
                Session["SerchViewbag"] = null;
                recordsearch = cen.SelectEmployeeSearch(RaiserName, RaiserCode).ToList();
            }

            return Json(recordsearch, JsonRequestBehavior.AllowGet);
        }
       
        public JsonResult GetEmployee(ECFDataModel ecf)
        {
            Session["Employeecode"] = ecf.Code;
            Session["EmployeeName"] = ecf.Name;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Insert(iem_mst_delegation insertdelegation)
        {
            string res = string.Empty;
            res = delegation.InsertDelegation(insertdelegation);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult Edit(int id, string viewfor)
        {
            if (viewfor == "edit")
            {
                ViewBag.viewfor = "edit";
            }
            else if (viewfor == "view")
            {
                ViewBag.viewfor = "view";
            }
            else if (viewfor == "delete")
            {
                ViewBag.viewfor = "delete";
            }
            iem_mst_delegation TypeModel = delegation.GetDelegationById(id);
            TypeModel.GetDelmattype = new SelectList(delegation.GetDelmattype().ToList(), "delmatypegid", "delmattypename", TypeModel.delmattype_gid);
            ViewBag.DelmatypeName = TypeModel;
            return PartialView(TypeModel);
        }
        [HttpPost]
        public JsonResult DeleteDeleagtion(iem_mst_delegation deledelegation)
        {
            string res=string.Empty;
            res = delegation.DeleteDelegation(deledelegation.delegate_gid);
            return Json(res,JsonRequestBehavior.AllowGet);
        } 
        [HttpPost]
        public JsonResult UpdateDelegation(iem_mst_delegation updatedelegation)
        {
            string res = string.Empty;
            res = delegation.UpdateDelegation(updatedelegation);
            return Json(res,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DownloadExcel(string ViewType, string FileName)
        {
            DataSet ds = (DataSet)Session["ExcelExportDelegation"];
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
