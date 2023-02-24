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
using IEM.App_Start;
using IEM.Areas.MASTERS.Models;

namespace IEM.Areas.MASTERS.Controllers
{
    [NoDirectAccess]
    public class AsmsDocumentGroupController : Controller
    {
        private Asms_IRepository ist;
        //
        // GET: /DocGrp/
        public AsmsDocumentGroupController() :
            this(new Asms_DataModel()) { }

        public AsmsDocumentGroupController(Asms_IRepository Objist)
        {
            ist = Objist;
        }

        public ActionResult Index()
        {
            string mt = null;
            Session["exceldownload"] = null;
            DocGrpModel records = new DocGrpModel();
            records.docGrpList = ist.GetDocGrp(mt).ToList();
            
            return View(records);
        }
        [HttpPost]
        public ActionResult Index(string filter = null ,string command=null)
        {
            Session["exceldownload"] = null;
            DocGrpModel records = new DocGrpModel();
            string mt = null;
            if (command == "Search" || command == "Refresh")
            {
                if (string.IsNullOrEmpty(filter) || string.IsNullOrWhiteSpace(filter))
                {
                    records.docGrpList = ist.GetDocGrp(mt).ToList();

                }
                if (string.IsNullOrEmpty(filter) == false || string.IsNullOrWhiteSpace(filter) == false)
                {
                    ViewBag.filter = filter.ToString();
                    records.docGrpList = ist.GetDocGrp(filter).ToList();

                }
                if (records.docGrpList.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }
            else
            {
                records.docGrpList = ist.GetDocGrp(mt).ToList();
            }
            Session["exceldownload"] = records.docGrpList;
            return View(records);
        }


        public PartialViewResult Details(int id)
        {
            DocGrpModel objDetails;
            objDetails = ist.GetDocGrpById(id);
            return PartialView(objDetails);
        }


        [HttpGet]
        public PartialViewResult Create()
        {
            DocGrpModel TypeModel = new DocGrpModel();
            return PartialView(TypeModel);
        }



        [HttpPost]
        public JsonResult Create(DocGrpModel TypeModel)
        {
            try
            {
                string check = ist.InsertDocGrp(TypeModel);
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
            DocGrpModel TypeModel = ist.GetDocGrpById(id);
            return PartialView(TypeModel);
        }



        [HttpPost]
        public JsonResult Edit(DocGrpModel TypeModel)
        {
            try
            {
                string check = ist.EditDocGrp(TypeModel); ;
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
        public JsonResult Delete(DocGrpModel TypeModel)
        {
            try
            {
                string check = null;
                var ids = TypeModel.docGrpID;
                check = ist.DeleteDocGrp(ids);
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
            List<DocGrpModel> cList;
            if (Session["exceldownload"] == null)
            {
                cList = ist.GetDocGrp(mt).ToList();  
            }
            else
            {
                cList = (List<DocGrpModel>)Session["exceldownload"];
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("Document Group");            
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                    i + 1
                    , cList[i].docGrpName.ToString());                  
            }
            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
           // Session["exceldownload"] = gv;
            if (gv.Rows.Count != 0)
            {
                //return new DownloadFileActionResult((GridView)Session["exceldownload"], "DocumentGroup.xls");
                return new DownloadFileActionResult(gv, "DocumentGroup.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }


    }
}
