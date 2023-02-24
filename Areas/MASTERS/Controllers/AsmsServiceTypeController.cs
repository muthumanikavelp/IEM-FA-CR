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
   
    public class AsmsServiceTypeController : Controller
    {
        private Asms_IRepository ist;
        //
        // GET: /ServiceType/
        public AsmsServiceTypeController() :
            this(new Asms_DataModel()) { }

        public AsmsServiceTypeController(Asms_IRepository Objist)
        {
            ist = Objist;
        }

        public ActionResult Index()
        {
            string mt = null;
            Session["exceldownload"] = null;
            ServiceTypeModel records = new ServiceTypeModel();
            records.serviceTypeModelList = ist.GetServiceType(mt).ToList();
            return View(records);
        }
        [HttpPost]
        public ActionResult Index(string filter = null, string command = null)
        {
            Session["exceldownload"] = null;
            ServiceTypeModel records = new ServiceTypeModel();
            string mt = null;
            if (command == "Search" || command == "Refresh")
            {
                if (string.IsNullOrEmpty(filter))
                {
                    records.serviceTypeModelList = ist.GetServiceType(mt).ToList();
                }
                if (string.IsNullOrEmpty(filter) == false || string.IsNullOrWhiteSpace(filter) == false)
                {
                    ViewBag.filter = filter.ToString();
                    records.serviceTypeModelList = ist.GetServiceType(filter).ToList();
                }
                if (records.serviceTypeModelList.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }
            else
            {
                records.serviceTypeModelList = ist.GetServiceType(mt).ToList();
            }
            Session["exceldownload"] = records.serviceTypeModelList;
            return View(records);
        }

        [HttpGet]
        public PartialViewResult Create()
        {
            ServiceTypeModel TypeModel = new ServiceTypeModel();
            return PartialView(TypeModel);
        }

        [HttpPost]
        public JsonResult Create(ServiceTypeModel TypeModel)
        {
            try
            {
                string check = ist.InsertServiceType(TypeModel);
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
            ServiceTypeModel TypeModel = ist.GetServiceTypeById(id);
            return PartialView(TypeModel);
        }


        [HttpPost]
        public JsonResult Edit(ServiceTypeModel TypeModel)
        {
            try
            {
                string check = ist.EditServiceType(TypeModel);
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
        public JsonResult Delete(ServiceTypeModel TypeModel)
        {
            try
            {
                var ids = TypeModel.serviceID;
              TypeModel= ist.DeleteServiceType(ids);
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
            List<ServiceTypeModel> cList;
            if (Session["exceldownload"] == null)
            {
                cList = ist.GetServiceType(mt).ToList();
            }
            else
            {
                cList = (List<ServiceTypeModel>)Session["exceldownload"];
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("Supplier Service Type");
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                    i + 1
                    , cList[i].serviceName.ToString()
                    );
            }
            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            //Session["exceldownload"] = gv;
            if (gv.Rows.Count != 0)
            {
                //return new DownloadFileActionResult((GridView)Session["exceldownload"], "Supplier Service Type.xls");
                return new DownloadFileActionResult(gv, "SupplierServiceType.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }
    }
}
