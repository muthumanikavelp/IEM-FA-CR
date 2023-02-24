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
    public class AsmsDocumentNameController : Controller
    {

        private Asms_IRepository ist;

        //
        // GET: /DocName/
        public AsmsDocumentNameController() :
            this(new Asms_DataModel()) { }

        public AsmsDocumentNameController(Asms_IRepository objIst)
        {
            ist = objIst;
        }

        public ActionResult Index()        
        {
            string mt = null;
            Session["exceldownload"] = null;
            DocNameModel records = new DocNameModel();
            records.docNameList = ist.GetDocName(mt).ToList();
            records.docGrpModel = new SelectList(ist.GetDocGrp(), "docGrpID", "docGrpName", records.docGrpID);
            records.FrqModel = new SelectList(ist.GetFrq(), "frqID", "frqName", records.frqID);
            Session["exceldownload"] = records.docNameList;
            return View(records);
        }
        [HttpPost]
        public ActionResult Index(string filter, int docGrpID, int frqID, string command = null)
        {
            Session["exceldownload"] = null;
            //dropdown search
            DocNameModel records = new DocNameModel();
            records.docGrpName = ist.GetDocGrpID(docGrpID).ToString();
            records.frqName = ist.GetDocFrqID(frqID).ToString();
            string mt = null;
            ViewBag.frq = string.Empty;
            ViewBag.grp = string.Empty;
           
            //search
            if (command == "Search" || command == "Refresh")
            {
                if (string.IsNullOrEmpty(filter) || frqID == 0 || docGrpID == 0)
                {
                    records.docNameList = ist.GetDocName(mt).ToList();
                }
                if (string.IsNullOrEmpty(filter) == false || string.IsNullOrWhiteSpace(filter) == false)
                {
                    Session["records"] = "";
                    ViewBag.filter = filter.ToString();
                    records.docNameList = ist.GetDocName(filter).ToList();
                }
                if (frqID != 0)
                {
                    ViewBag.frq = records.frqName.ToString();
                    records.docNameList = records.docNameList.Where(x => records.frqName == null ||
                           (x.frqName.Contains(records.frqName))).ToList();
                }
                if (docGrpID != 0)
                {
                    ViewBag.grp = records.docGrpName.ToString();
                    records.docNameList = records.docNameList.Where(x => records.docGrpName == null ||
                           (x.docGrpName.Contains(records.docGrpName))).ToList();
                }
                if (records.docNameList.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }
            else
            {
                ViewBag.frq = 0;
                ViewBag.grp = 0;           
                records.docNameList = ist.GetDocName(mt).ToList();
            }
            //dropdown           
            records.docGrpModel = new SelectList(ist.GetDocGrp(), "docGrpID", "docGrpName");
            records.FrqModel = new SelectList(ist.GetFrq(), "frqID", "frqName");
            Session["exceldownload"] = records.docNameList;
            return View(records);
        }

        [HttpGet]
        public PartialViewResult Create()
        {
            DocNameModel model = new DocNameModel();
            model.FrqModel = new SelectList(ist.GetFrq(), "frqID", "frqName");
            model.docGrpModel = new SelectList(ist.GetDocGrp(), "DocGrpID", "DocGrpName");
            model.docFrqModel = new SelectList(ist.GetDocFrq(), "DocFrqID", "DocFrqName");
            return PartialView(model);
        }


        [HttpPost]
        public JsonResult Create(DocNameModel DocNameModel)
        {
            try
            {
                string check = ist.InsertDocName(DocNameModel);
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
            ViewBag.man = "Mandatory";
            ViewBag.opt = "Optional";           
            //string mt = null;
            DocNameModel TypeModel = new DocNameModel();
            TypeModel = ist.GetDocNameById(id);
            ViewBag.frq = TypeModel.frqName;
            TypeModel.FrqModel = new SelectList(ist.GetFrq(), "frqID", "frqName", TypeModel.frqID);
            TypeModel.docGrpModel = new SelectList(ist.GetDocGrp(), "DocGrpID", "DocGrpName", TypeModel.docGrpID);
            TypeModel.docFrqModel = new SelectList(ist.GetDocFrq(), "DocFrqID", "DocFrqName", TypeModel.docFrqID);
            //   TypeModel.docGrpModel = new SelectList(ist.GetDocGrp(mt), "DocGrpID", "DocGrpName", TypeModel.docGrpID);
            //   TypeModel.docFrqModel = new SelectList(ist.GetDocFrqedit(), "DocFrqID", "DocFrqName", TypeModel.docFrqID);
            return PartialView(TypeModel);
        }


        [HttpPost]
        public JsonResult Edit(DocNameModel docNameModel)
        {
            try
            {
                string check = ist.EditDocName(docNameModel);
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
        public JsonResult Delete(DocNameModel model)
        {
            try
            {
                var id = model.docNameID;
                model= ist.DeleteDocName(id);
                return Json(model, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult downloadsexcel()
        {
            string mt = null;
            List<DocNameModel> cList;
            if (Session["exceldownload"] == null)
            {
                cList = ist.GetDocName(mt).ToList();
            }
            else
            {
                cList = (List<DocNameModel>)Session["exceldownload"];
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("Document Group");
            dt.Columns.Add("Document Name");
            dt.Columns.Add("Document Frequency");
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                    i + 1
                    ,cList[i].docNameName.ToString()
                    , cList[i].docGrpName.ToString()
                    , cList[i].frqName.ToString());
            }
            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            //Session["exceldownload"] = gv;
            if (gv.Rows.Count != 0)
            {
                //return new DownloadFileActionResult((GridView)Session["exceldownload"], "DocumentName.xls");
                return new DownloadFileActionResult(gv, "DocumentName.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }

    }
}
