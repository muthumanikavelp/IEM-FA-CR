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

    public class AsmsFrequencyController : Controller
    {
        private Asms_IRepository ist;

        //
        // GET: /DocFrq/
        public AsmsFrequencyController() :
            this(new Asms_DataModel()) { }

        public AsmsFrequencyController(Asms_IRepository Objist)
        {
            ist = Objist;
        }

        public ActionResult Index()
        {
            string mt = null;
            Session["exceldownload"] = null;
           DocFrqModel records = new DocFrqModel();
            records.docFrqList = ist.GetDocFrq(mt).ToList();           
            return View(records);
        }
        [HttpPost]
        public ActionResult Index(string filter = null, string command = null)
        {
            Session["exceldownload"] = null;       
            DocFrqModel records = new DocFrqModel();
            string mt = null;

            if (command == "Search" || command == "Refresh")
            {
                if (string.IsNullOrEmpty(filter))
                {
                    records.docFrqList = ist.GetDocFrq(mt).ToList();
                }
                if (string.IsNullOrEmpty(filter) == false || string.IsNullOrWhiteSpace(filter) == false)
                {
                    ViewBag.filter = filter.ToString();
                    records.docFrqList = ist.GetDocFrq(filter).ToList();
                }

                if (records.docFrqList.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }
            else {
                records.docFrqList = ist.GetDocFrq(mt).ToList();
            }
            Session["exceldownload"] = records.docFrqList;
            return View(records);
        }

        public PartialViewResult Details(int id)
        {
            DocFrqModel objDetails;
            objDetails = ist.GetDocFrqById(id);
            return PartialView(objDetails);
        }



        [HttpGet]
        public PartialViewResult Create()
        {
            DocFrqModel TypeModel = new DocFrqModel();
            return PartialView(TypeModel);
        }


        [HttpPost]
        public JsonResult Create(DocFrqModel TypeModel)
        {
            try
            {
                string check = ist.InsertDocFrq(TypeModel);
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
            DocFrqModel TypeModel = ist.GetDocFrqById(id);
            return PartialView(TypeModel);
        }



        [HttpPost]
        public JsonResult Edit(DocFrqModel TypeModel)
        {
            try
            {
                string check = ist.EditDocFrq(TypeModel);
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
        public JsonResult Delete(DocFrqModel TypeModel)
        {
            try
            {
                string check = null;
                var ids = TypeModel.docFrqID;
                check = ist.DeleteDocFrq(ids);
                return Json(check, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult downloadsexcel()
        {
            string mt = null;
            List<DocFrqModel> cList;
            if (Session["exceldownload"] == null)
            {
                cList = ist.GetDocFrq(mt).ToList();         
            }
            else
            {
                cList = (List<DocFrqModel>)Session["exceldownload"];
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");            
            dt.Columns.Add("Frequency");
            dt.Columns.Add("Months");
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                    i + 1
                    , cList[i].docFrqName.ToString()
                    ,cList[i].docFrqMonth.ToString());
                    
            }
            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            //Session["exceldownload"] = gv;
            if (gv.Rows.Count != 0)
            {
                //return new DownloadFileActionResult((GridView)Session["exceldownload"], "Frequency.xls");
                return new DownloadFileActionResult(gv, "Frequency.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }
    }
}
