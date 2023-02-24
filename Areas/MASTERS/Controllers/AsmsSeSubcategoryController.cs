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
    public class AsmsSeSubcategoryController : Controller
    {
        private Asms_IRepository ist;

        //
        // GET: /SESubCategory/
        public AsmsSeSubcategoryController() :
            this(new Asms_DataModel()) { }

        public AsmsSeSubcategoryController(Asms_IRepository Objist)
        {
            ist = Objist;
        }

        public ActionResult Index()
        {
            string mt = null;
            Session["exceldownload"] = null;
            SESubCategoryModel records = new SESubCategoryModel();
            records.seSubCategoryList = ist.GetSESubCategory(mt).ToList();
            records.seCategoryMod = new SelectList(ist.GetSECategory(), "SECategoryID", "SECategoryName");
            records.seRateMod = new SelectList(ist.GetSEratedrop(), "SErateID", "SErateName");
            return View(records);
        }
        [HttpPost]
        public ActionResult Index(string filter, int SECategoryID, int SErateID, string command = null)
        {
            Session["exceldownload"] = null;
            string MT = null;
            SESubCategoryModel records = new SESubCategoryModel();
            records.seCategoryName = ist.GetSECategoryId(SECategoryID).ToString();
            records.seRateName = ist.GetSErateById(SErateID).ToString();
            records.seSubCategoryList = ist.GetSESubCategory(MT).ToList();
            ViewBag.filter1 = 0;
            ViewBag.filter2 = 0;
            if (command == "Search" || command == "Refresh")
            {
                if (string.IsNullOrEmpty(filter) || SECategoryID == 0 || SErateID == 0)
                {
                    records.seSubCategoryList = ist.GetSESubCategory(MT).ToList();
                }
                if (string.IsNullOrEmpty(filter) == false || string.IsNullOrWhiteSpace(filter) == false)
                {
                    ViewBag.filter = filter.ToString();
                    records.seSubCategoryList = ist.GetSESubCategory(filter).ToList();
                }
                if (SErateID != 0)
                {
                    ViewBag.filter1 = records.seRateName.ToString();
                    records.seSubCategoryList = records.seSubCategoryList.Where(x => records.seRateName == null ||
                           (x.seRateName.Contains(records.seRateName))).ToList();
                }
                if (SECategoryID != 0)
                {
                    ViewBag.filter2 = records.seCategoryName.ToString();
                    records.seSubCategoryList = records.seSubCategoryList.Where(x => records.seCategoryName == null ||
                           (x.seCategoryName.Contains(records.seCategoryName))).ToList();


                }

                if (records.seSubCategoryList.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }           
            records.seCategoryMod = new SelectList(ist.GetSECategory(), "SECategoryID", "SECategoryName");
            records.seRateMod = new SelectList(ist.GetSEratedrop(), "SErateID", "SErateName");
            Session["exceldownload"] = records.seSubCategoryList;
            return View(records);
        }


        [HttpGet]
        public PartialViewResult Create()
        {
            SESubCategoryModel records = new SESubCategoryModel();
            records.seCategoryMod = new SelectList(ist.GetSECategory(), "SECategoryID", "SECategoryName", records.seCategoryID);
            records.seRateMod = new SelectList(ist.GetSEratedrop(), "SErateID", "SErateName", records.seRateID);
            return PartialView(records);
        }

        public PartialViewResult score(int id)
        {
            List<SESubCategoryModel> records = new List<SESubCategoryModel>();
            records = ist.GetScoreByGrp(id).ToList();
            return PartialView(records);
        }



        [HttpPost]
        public JsonResult Create(SESubCategoryModel SESubCategoryModel)
        {
            try
            {
                string check = ist.InsertSESubCategory(SESubCategoryModel);
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
            SESubCategoryModel records = new SESubCategoryModel();
            records = ist.GetSESubCategoryById(id);
            string mt = null;
            records.seCategoryMod = new SelectList(ist.GetSECategory(), "SECategoryID", "SECategoryName", records.seCategoryID);
            records.seRateMod = new SelectList(ist.GetSEratedrop(), "SErateID", "SErateName", records.seRateID);
            return PartialView(records);

        }


        [HttpPost]
        public JsonResult Edit(SESubCategoryModel SESubCategoryModel)
        {
            try
            {
                string check = ist.EditSESubCategory(SESubCategoryModel);
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
            finally
            {
                ist.EditSESubCategory(SESubCategoryModel);

            }
        }

        [HttpGet]
        public PartialViewResult Delete(int id)
        {
            try
            {
                SESubCategoryModel records = ist.GetSESubCategoryById(id);
                return PartialView(records);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult Delete(SESubCategoryModel TypeModel)
        {
            try
            {
                var ids = TypeModel.seSubCategoryID;
                TypeModel = ist.DeleteSESubCategory(ids);
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
            List<SESubCategoryModel> cList;
            if (Session["exceldownload"] == null)
            {
                cList = ist.GetSESubCategory(mt).ToList();
            }
            else
            {
                cList = (List<SESubCategoryModel>)Session["exceldownload"];
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("Category");
            dt.Columns.Add("Evaluation Questions");
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                    i + 1
                    , cList[i].seCategoryName.ToString()
                    , cList[i].seSubCategoryName.ToString()
                    );
            }
            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            //Session["exceldownload"] = gv;
            if (gv.Rows.Count != 0)
            {
                //return new DownloadFileActionResult((GridView)Session["exceldownload"], "Supplier Evaluation Questions.xls");
                return new DownloadFileActionResult(gv, "SupplierEvaluationQuestions.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }

    }
}
