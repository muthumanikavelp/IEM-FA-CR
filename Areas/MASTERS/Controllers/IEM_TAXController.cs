using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.MASTERS.Models;
using System.Data;
using ClosedXML.Excel;
using System.IO;
using IEM.Helper;
using IEM.App_Start;
namespace IEM.Areas.MASTERS.Controllers
{
    [NoDirectAccess]
    public class IEM_TAXController : Controller
    {
        //
        // GET: /MASTERS/IEM_TAX/
        private Iiem_mst_Tax Tax;
        private dbLib db = new dbLib();
        private proLib pl = new proLib();
        public IEM_TAXController() :
            this(new IEM_MST_TAX()) { }

        public IEM_TAXController(Iiem_mst_Tax taxobj)
        {
            Tax = taxobj;
        }
        public ActionResult Index()
        {
            List<iem_mst_tax> taxrecords = new List<iem_mst_tax>();
            taxrecords = Tax.gettax().ToList();
            if (taxrecords.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            return View(taxrecords);
        }
        [HttpPost]
        public ActionResult Index(string filter_name = null, string filter_name1 = null)
        {
            List<iem_mst_tax> taxrecords = new List<iem_mst_tax>();
            taxrecords = Tax.gettaxid(filter_name, filter_name1).ToList();
            @ViewBag.filter_name = filter_name;
            @ViewBag.filter_name1 = filter_name1;
            if (taxrecords.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            return View(taxrecords);
        }
        [HttpGet]
        public PartialViewResult Create()
        {

            iem_mst_tax TypeModel = new iem_mst_tax();
            TypeModel.Gettax = new SelectList(Tax.Gettaxvl(), "tax_gid", "tax_name");
            return PartialView(TypeModel);

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
            else if (viewfor == "Delete")
            {
                ViewBag.viewfor = "Delete";
            }
            iem_mst_tax TypeModel = Tax.GettaxById(id);
            TypeModel.Gettax = new SelectList(Tax.Gettaxvl_id(id), "tax_gid", "tax_name", TypeModel.tax_parent_name);
            return PartialView(TypeModel);

        }
        [HttpPost]
        public JsonResult Createtax(iem_mst_tax taxModel)
        {
            string result = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    taxModel.tax_insert_by = int.Parse(Session["employee_gid"].ToString());
                    result = Tax.Inserttax(taxModel);
                    if (result == "success") RedirectToAction("Index");

                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult Delettax(iem_mst_tax taxModel)
        {
            string result = string.Empty;
            try
            {
                result = Tax.Deletetax(taxModel.tax_gid);

                if (result == "success") RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EditTAX(iem_mst_tax taxCat)
        {
            string result = string.Empty;
            //try
            //{


            //    taxCat.tax_update_by = int.Parse(Session["employee_gid"].ToString());
            //    Tax.Updattax(taxCat);
            //    if (result == "success") RedirectToAction("Index");
            //    return Json(taxCat, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            try
            {
                if (ModelState.IsValid)
                {
                    result = Tax.Updattax(taxCat);
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
