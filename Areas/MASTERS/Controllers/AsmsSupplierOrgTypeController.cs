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
    public class AsmsSupplierOrgTypeController : Controller
    {

        private Asms_IRepository objModel;

        public AsmsSupplierOrgTypeController()
            : this(new Asms_DataModel())
        {

        }
        public AsmsSupplierOrgTypeController(Asms_IRepository objM)
        {
            objModel = objM;
        }

        //
        // GET: /SupplierOrgType/

        public ActionResult Index()
        {
            string mt = null;
            Session["exceldownload"] = null;
            OrgTypeModel records = new OrgTypeModel();
            records.orgTypeModelList = objModel.GetOrgType(mt).ToList();
            return View(records);
        }
        [HttpPost]
        public ActionResult Index(string filter = null, string command = null)
        {
            Session["exceldownload"] = null;
            string mt = null;
            OrgTypeModel records = new OrgTypeModel();
            if (command == "Search" || command == "Refresh")
            {
                if (string.IsNullOrEmpty(filter))
                {
                    records.orgTypeModelList = objModel.GetOrgType(mt).ToList();
                }
                if (string.IsNullOrEmpty(filter) == false || string.IsNullOrWhiteSpace(filter) == false)
                {
                    ViewBag.filter = filter.ToString();
                    records.orgTypeModelList = objModel.GetOrgType(filter).ToList();
                }
                if (records.orgTypeModelList.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }
            else { records.orgTypeModelList = objModel.GetOrgType(mt).ToList(); }
            Session["exceldownload"] = records.orgTypeModelList;
            return View(records);
        }


        //
        // GET: /SupplierOrgType/Create
        [HttpGet]
        public PartialViewResult Create()
        {
            OrgTypeModel TypeModel = new OrgTypeModel();
            return PartialView(TypeModel);
        }

        //
        // POST: /SupplierOrgType/Create

        [HttpPost]
        public JsonResult Create(OrgTypeModel TypeModel)
        {
            try
            {
                string check = objModel.InsertOrgType(TypeModel);
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

        //
        // GET: /SupplierOrgType/Edit
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
            OrgTypeModel TypeModel = objModel.GetOrgTypeById(id);
            return PartialView(TypeModel);
        }

        //
        // POST: /SupplierOrgType/Edit

        [HttpPost]
        public JsonResult Edit(OrgTypeModel TypeModel)
        {
            try
            {
                string check = objModel.UpdateOrgType(TypeModel);
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
        public JsonResult Delete(OrgTypeModel TypeModel)
        {
            try
            {
                var ids = TypeModel.typeID;
               TypeModel= objModel.DeleteOrgType(ids);
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
            List<OrgTypeModel> cList;
            if (Session["exceldownload"] == null)
            {
                cList = objModel.GetOrgType(mt).ToList();
            }
            else
            {
                cList = (List<OrgTypeModel>)Session["exceldownload"];
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("Organization Type");
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                    i + 1
                    , cList[i].typeName.ToString()
                    );
            }
            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            //Session["exceldownload"] = gv;
            if (gv.Rows.Count != 0)
            {
                //return new DownloadFileActionResult((GridView)Session["exceldownload"], "Supplier Organization Type.xls");
                return new DownloadFileActionResult(gv, "SupplierOrganizationType.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }
    }
}
