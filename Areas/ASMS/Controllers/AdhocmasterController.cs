using ClosedXML.Excel;
using IEM.Areas.ASMS.Models;
using IEM.Common;
using IEM.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.App_Start;
namespace IEM.Areas.ASMS.Controllers
{
    [NoDirectAccess]
    public class AdhocmasterController : Controller
    {
        //
        // GET: /ASMS/Adhocmaster/

        private dbLib db = new dbLib();
        private IAdhocrepository adhoc;
        private proLib pl = new proLib();
        CmnFunctions com = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        public AdhocmasterController() :
            this(new Adhoc_model()) { }

        public AdhocmasterController(IAdhocrepository Objist) 
        {
            adhoc = Objist;
        }


        public ActionResult Index()
        {
            List<adhoc_model> g = new List<adhoc_model>();
            try
            {
                g = adhoc.getadhoclist().ToList();
               
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return View(g);
        }


        [HttpPost]
        public ActionResult Index(string filter_code = "", string filter_name = "", string command = null)
        {

            List<adhoc_model> records = new List<adhoc_model>();
            Session["records"] = "";
            if (command == "Search" || command == "Refresh")
            {
                records = adhoc.getadhoclist(filter_code, filter_name).ToList();
                Session["records"] = records;
                @ViewBag.filter_code = filter_code;
                @ViewBag.filter_name = filter_name;
                if (records.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }
            if (command == "Clear")
            {
                records = adhoc.getadhoclist().ToList();
            }
            return View(records);
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
            adhoc_model TypeModel = adhoc.GetAdhocById(id);
            return PartialView(TypeModel);
        }

        [HttpPost]
        public JsonResult UpdateadhocDetails(adhoc_model TypeModel)
        {
            string result = "";

            try
            {
                if (ModelState.IsValid)
                {

                    TypeModel.Audit_updateby = int.Parse(com.GetLoginUserGid().ToString());
                    result = adhoc.Updateadhoc(TypeModel);

                    if (result == "success") RedirectToAction("Index");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public PartialViewResult Delete(int id, string viewfor)
        {
            if (viewfor == "delete")
            {
                ViewBag.viewfor = "delete";
            }
            adhoc_model TypeModel = adhoc.GetAdhocById(id);
            return PartialView(TypeModel);
        }


        [HttpPost]
        public JsonResult Deleteadhoc(adhoc_model TypeModel)
        {
            string lsresult = string.Empty;
            try
            {
                string sc = (string)TypeModel.Audit_suppliercode;
                lsresult = adhoc.Deleteadhoc(sc);
                ViewBag.viewfor = "delete";
                return Json(lsresult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public JsonResult Getgstcode(adhoc_model obj)
        {
            try
            {
                string StateID = obj.Audit_stategid.ToString();
                
                string Data1 = "";
                Data1 = adhoc.getstategstcode(StateID);
              
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                List<SupplierContactDetails> lst = new List<SupplierContactDetails>();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpGet]
        public PartialViewResult View(int id)
        {
            List<adhoc_model> g = new List<adhoc_model>();

            try
            {
                g = adhoc.GetAdhochistoryById(id).ToList();

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            
            return PartialView(g);
        }
        //downloading Excel File
        [HttpPost]
        public JsonResult DownloadExcel(string ViewType, string FileName)
        {
            DataSet ds = db.GetCommonExcelDownload(ViewType, pl.LoginUserId);
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
