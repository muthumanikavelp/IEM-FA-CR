//Version 1.0 23/07/2015
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.IO;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using System.Net;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Reflection;
using IEM.Areas.MASTERS.Models;
using IEM.App_Start;



namespace IEM.Areas.MASTERS.Controllers
{
    [NoDirectAccess]
    public class AsmsSeCategoryController : Controller
    {

        private Asms_IRepository ist;

        public AsmsSeCategoryController() :
            this(new Asms_DataModel()) { }

        public AsmsSeCategoryController(Asms_IRepository Objist)
        {
            ist = Objist;
        }
        //
        // GET: /SECategory/

        public ActionResult Index()
        {
            string mt = null;
            Session["exceldownload"] = null;
            SECategoryModel records = new SECategoryModel();
            records.seCategoryModelList = ist.GetSECategory(mt).ToList();
            return View(records);
        }
        [HttpPost]
        public ActionResult Index(string filter = null, string command = null)
        {
            SECategoryModel records = new SECategoryModel();
            string mt = null;
            if (command == "Search" || command == "Refresh")
            {
                if (string.IsNullOrEmpty(filter))
                {
                    records.seCategoryModelList = ist.GetSECategory(mt).ToList();
                }
                if (string.IsNullOrEmpty(filter) == false || string.IsNullOrWhiteSpace(filter) == false)
                {
                    ViewBag.filter = filter.ToString();
                    records.seCategoryModelList = ist.GetSECategory(filter).ToList();
                }
                if (records.seCategoryModelList.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }
            else
            {
                records.seCategoryModelList = ist.GetSECategory(mt).ToList();
            }
            Session["exceldownload"] = records.seCategoryModelList;
            return View(records);
        }


        [HttpGet]
        public PartialViewResult Create()
        {
            SECategoryModel TypeModel = new SECategoryModel();
            return PartialView(TypeModel);
        }



        [HttpPost]
        public JsonResult Create(SECategoryModel TypeModel)
        {
            try
            {
                string check = ist.InsertSECategory(TypeModel);
                if (check == null)
                {
                    RedirectToAction("Index");
                }
                return Json(check, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            SECategoryModel TypeModel = ist.GetSECategoryById(id);
            return PartialView(TypeModel);
        }


        [HttpPost]
        public JsonResult Edit(SECategoryModel TypeModel)
        {
            try
            {
                string check = ist.EditSECategory(TypeModel);
                if (check == null)
                {
                    RedirectToAction("Index");
                }
                return Json(check, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public JsonResult Delete(SECategoryModel TypeModel)
        {
            try
            {
                string check = null;
                var ids = TypeModel.seCategoryID;
                check = ist.DeleteSECategory(ids);
                return Json(check, JsonRequestBehavior.AllowGet);

                // TODO: Add delete logic here



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult downloadsexcel()
        {
            string mt = null;
            List<SECategoryModel> cList;
            if (Session["exceldownload"] == null)
            {
                cList = ist.GetSECategory(mt).ToList();
            }
            else
            {
                cList = (List<SECategoryModel>)Session["exceldownload"];
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("Supplier Evaluation Category");            
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                    i + 1
                    , cList[i].seCategoryName.ToString()
                    );

            }
            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            //Session["exceldownload"] = gv;
            if (gv.Rows.Count != 0)
            {
                //return new DownloadFileActionResult((GridView)Session["exceldownload"], "Supplier Evaluation Category.xls");
                return new DownloadFileActionResult(gv, "SupplierEvaluationCategory.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }
    }
}
