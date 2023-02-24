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
    public class AsmsSupplierCategoryController : Controller
    {

        private Asms_IRepository ist;
        //
        // GET: /Category/
        public AsmsSupplierCategoryController() :
            this(new Asms_DataModel()) { }

        public AsmsSupplierCategoryController(Asms_IRepository Objist)
        {
            ist = Objist;
        }

        public ActionResult Index()
        {
            string mt = null;
            Session["exceldownload"] = null;
            CategoryModel records = new CategoryModel();
            records.categoryModelList = ist.GetCategory(mt).ToList();
            return View(records);
        }
        [HttpPost]
        public ActionResult Index(string filter = null, string command = null)
        {
            Session["exceldownload"] = null;
            CategoryModel records = new CategoryModel();
            string mt = null;
            if (command == "Search" || command == "Refresh")
            {
                if (string.IsNullOrEmpty(filter) || string.IsNullOrWhiteSpace(filter))
                {
                    records.categoryModelList = ist.GetCategory(mt).ToList();
                }
                if (string.IsNullOrEmpty(filter) == false || string.IsNullOrWhiteSpace(filter) == false)
                {
                    ViewBag.filter = filter.ToString();
                    records.categoryModelList = ist.GetCategory(filter).ToList();
                }
                if (records.categoryModelList.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }
            else { records.categoryModelList = ist.GetCategory(mt).ToList(); }
            Session["exceldownload"] = records.categoryModelList;
            return View(records);
        }


        [HttpGet]
        public PartialViewResult Create()
        {
            CategoryModel TypeModel = new CategoryModel();
            return PartialView(TypeModel);
        }


        [HttpPost]
        public JsonResult Create(CategoryModel TypeModel)
        {
            try
            {
                string check = ist.InsertCategory(TypeModel);
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
            CategoryModel TypeModel = ist.GetCategoryById(id);
            return PartialView(TypeModel);
        }



        [HttpPost]
        public JsonResult Edit(CategoryModel TypeModel)
        {
            try
            {
                string check = ist.EditCategory(TypeModel);
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
        public JsonResult Delete(CategoryModel TypeModel)
        {
            try
            {
                var ids = TypeModel.categoryID;
               TypeModel= ist.DeleteCategory(ids);
               return Json(TypeModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult downloadsexcel()
        {
            string mt = null;
            List<CategoryModel> cList;
            if (Session["exceldownload"] == null)
            {
                cList = ist.GetCategory(mt).ToList();
            }
            else
            {
                cList = (List<CategoryModel>)Session["exceldownload"];
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("Supplier Classification");
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                    i + 1
                    , cList[i].categoryName.ToString()
                    );
            }
            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            //Session["exceldownload"] = gv;
            if (gv.Rows.Count != 0)
            {
                //return new DownloadFileActionResult((GridView)Session["exceldownload"], "Supplier Classification.xls");
                return new DownloadFileActionResult(gv, "SupplierClassification.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }

    }
}
