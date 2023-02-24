using ClosedXML.Excel;
using IEM.Areas.EOW.Models;
using IEM.Areas.MASTERS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.App_Start;

namespace IEM.Areas.EOW.Controllers
{

    [NoDirectAccess]
    public class ProxyController : Controller
    {
         private ProxyRepository objModel;
         CentralModel cen = new CentralModel();
         CentraldataModel sub = new CentraldataModel();
        string result;
        public ProxyController(): this(new ProxyModel())
        {

        }
        public ProxyController(ProxyRepository objM)
        {
            objModel = objM;
        }
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                List<ProxyDataModel> proxy = new List<ProxyDataModel>();
                proxy = objModel.SelectProxy().ToList();
                if (proxy.Count == 0)
                {
                    ViewBag.Message = "No Records";
                }
                return View(proxy);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult Index(string DateFrom = null, string DateTo = null, string ProxyTo=null)
        {
            try
            {
                ViewBag.DateFrom = DateFrom;
                ViewBag.DateTo = DateTo;
                ViewBag.ProxyTo = ProxyTo;
                List<ProxyDataModel> proxy = new List<ProxyDataModel>();
                proxy = objModel.SearchProxy(DateFrom, DateTo, ProxyTo).ToList();
                if (proxy.Count == 0)
                {
                    ViewBag.Message = "No Records Found";
                }
                return View(proxy);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult NewProxy()
        {
            return PartialView();
        }
        //[HttpGet]
        //public PartialViewResult EmployeeSearch(string listfor)
        //{
        //    List<ProxyDataModel> obj = new List<ProxyDataModel>();
        //    ProxyDataModel emp = new ProxyDataModel();
        //    if (Session["RaiserName"] != null)
        //    {
        //        ViewBag.EmployeeName = Session["RaiserName"].ToString();
        //    }
        //    if (Session["RaiserCode"] != null)
        //    {
        //        ViewBag.EmployeeCode = Session["RaiserCode"].ToString();
        //    }
        //    if (listfor == "search")
        //    {
        //        if (Session["SearchEmployeedata"] != null)
        //        {
        //            obj = (List<ProxyDataModel>)Session["SearchEmployeedata"];
        //        }
        //    }
        //    else
        //    {
        //        obj = objModel.SelectEmployee().ToList();
        //    }
        //    return PartialView(obj);
        //}
        //[HttpPost]
        //public JsonResult EmployeeSearch(string RaiserName = "", string RaiserCode = "")
        //{
        //    List<ProxyDataModel> obj = new List<ProxyDataModel>();
        //    ProxyDataModel emp = new ProxyDataModel();
        //    obj = objModel.SelectEmployeeSearch(RaiserName, RaiserCode).ToList();
        //    Session["SearchEmployeedata"] = obj;
        //    Session["RaiserName"] = RaiserName;
        //    Session["RaiserCode"] = RaiserCode;
        //    return Json(result, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult SaveProxy(ProxyDataModel proxy)
        {
            result = objModel.SaveProxy(proxy);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult EditProxy(int id)
        {
            try
            {
                Session["ProxyId"] = id;
                List<ProxyDataModel> proxy = new List<ProxyDataModel>();
                proxy = objModel.SelectEditProxy(id).ToList();
                if (proxy.Count > 0)
                {
                    ViewBag.EmployeeEditId = proxy[0].proxy_to;
                    ViewBag.ProxyEditId = proxy[0].EmployeeCode;
                    ViewBag.ProxyEditName = proxy[0].EmployeeName;
                    ViewBag.TitleEditGrade = proxy[0].ProxyGrade;
                    ViewBag.MobileEditNo = proxy[0].MobileNo;
                    ViewBag.DateEditFrom = proxy[0].proxy_period_from;
                    ViewBag.DateEditTo = proxy[0].proxy_period_to;
                    ViewBag.ProxyActivate = proxy[0].proxy_active;
                    ViewBag.remark = proxy[0].proxy_remark;

                }
                return PartialView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult SaveEditProxy(ProxyDataModel proxy)
        {
            try
            {
                if (Session["ProxyId"] != null)
                {
                    proxy.proxy_gid = Convert.ToInt16(Session["ProxyId"]);
                }
                result = objModel.EditProxy(proxy);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult ViewProxy(int id)
        {
            try
            {
                Session["ProxyId"] = id;
                List<ProxyDataModel> proxy = new List<ProxyDataModel>();
                proxy = objModel.SelectEditProxy(id).ToList();
                if (proxy.Count > 0)
                {
                    ViewBag.EmployeeEditId = proxy[0].proxy_to;
                    ViewBag.ProxyEditId = proxy[0].EmployeeCode;
                    ViewBag.ProxyEditName = proxy[0].EmployeeName;
                    ViewBag.TitleEditGrade = proxy[0].ProxyGrade;
                    ViewBag.MobileEditNo = proxy[0].MobileNo;
                    ViewBag.DateEditFrom = proxy[0].proxy_period_from;
                    ViewBag.DateEditTo = proxy[0].proxy_period_to;
                    ViewBag.ProxyActivate = proxy[0].proxy_active;
                    ViewBag.remark = proxy[0].proxy_remark;
                }
                return PartialView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult DeleteProxy(int id)
        {
            try
            {
                Session["ProxyId"] = id;
                List<ProxyDataModel> proxy = new List<ProxyDataModel>();
                proxy = objModel.SelectEditProxy(id).ToList();
                if (proxy.Count > 0)
                {
                    ViewBag.EmployeeEditId = proxy[0].proxy_to;
                    ViewBag.ProxyEditId = proxy[0].EmployeeCode;
                    ViewBag.ProxyEditName = proxy[0].EmployeeName;
                    ViewBag.TitleEditGrade = proxy[0].ProxyGrade;
                    ViewBag.MobileEditNo = proxy[0].MobileNo;
                    ViewBag.DateEditFrom = proxy[0].proxy_period_from;
                    ViewBag.DateEditTo = proxy[0].proxy_period_to;
                    ViewBag.ProxyActivate = proxy[0].proxy_active;
                    ViewBag.remark = proxy[0].proxy_remark;
                }
                return PartialView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult DeletesaveProxy(ProxyDataModel proxy)
        {
            try
            {
                if (Session["ProxyId"] != null)
                {
                    proxy.proxy_gid = Convert.ToInt16(Session["ProxyId"]);
                }
                result = objModel.DeleteProxy(proxy);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult EmployeeSearch(string listfor)
        {
            try
            {
                List<ProxyDataModel> obj = new List<ProxyDataModel>();
                CentraldataModel emp = new CentraldataModel();
                if (Session["RaiserName"] != null)
                {
                    ViewBag.EmployeeName = Session["RaiserName"].ToString();
                }
                if (Session["RaiserCode"] != null)
                {
                    ViewBag.EmployeeCode = Session["RaiserCode"].ToString();
                }
                if (listfor == "search")
                {
                    if (Session["SearchEmployeedata"] != null)
                    {
                        obj = (List<ProxyDataModel>)Session["SearchEmployeedata"];
                    }
                }
                else
                {
                    obj = objModel.SelectEmployee().ToList();
                }
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public PartialViewResult EmployeeSearchforproxy(string RaiserName = "", string RaiserCode = "")
        {
            try
            {
                List<ProxyDataModel> obj = new List<ProxyDataModel>();
                CentraldataModel emp = new CentraldataModel();
                if (RaiserCode == "" && RaiserName == "")
                {
                    obj = objModel.SelectEmployee().ToList();
                }
                else
                {
                    obj = objModel.SelectEmployeeSearch(RaiserName, RaiserCode).ToList();
                }

                //    obj = cen.SelectEmployeeSearch(RaiserName, RaiserCode).ToList();
                if (obj != null)
                {

                    ViewBag.proempcode = RaiserCode;
                    ViewBag.proempname = RaiserName;
                    if (obj.Count == 0)
                    {
                        ViewBag.Message = "No Record's Found !";
                    }

                }
                return PartialView("EmployeeSearch", obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult DownloadExcel(string ViewType, string FileName)
        {
            DataSet ds = (DataSet)Session["ExcelExportProxy"];
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
