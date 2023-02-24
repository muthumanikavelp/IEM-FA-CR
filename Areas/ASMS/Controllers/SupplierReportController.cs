
using IEM.Areas.ASMS.Models;
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
namespace IEM.Areas.ASMS.Controllers
{

      [NoDirectAccess]
    public class SupplierReportController : Controller
    {
        //
        // GET: /ASMS/SupplierReport/
        private ASMSIRepository objModelirep;
        public SupplierReportController() : this(new ASMSDataModel()) { }
        public SupplierReportController(ASMSIRepository irep) { objModelirep = irep; }
        public ActionResult Index(string search = null)
        {
            ReportModel reportModel = new ReportModel();
            List<ReportModel> objdept = new List<ReportModel>();
            try
            {
                if (Session["reportlist"] != null)
                {
                    reportModel.reportList = (List<ReportModel>)Session["reportlist"];
                }
                else
                {
                    reportModel.reportList = objModelirep.GetReport(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "0", string.Empty, "0", string.Empty, string.Empty, "0", "0", "ALL");
                }
                if (search == "search")
                {
                    reportModel.reportList = objModelirep.GetReport(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "0", string.Empty, "0", string.Empty, string.Empty, "0", "0", "ALL");
                    Session["reportlist"] = reportModel.reportList;
                }
                if (reportModel.reportList.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }

            }
            catch (Exception ex)
            {

            }

            ViewBag.contract = new SelectList(objModelirep.Getcontract().ToList(), "AgreementId", "typeofAgreement");
            ViewBag.city = new SelectList(objModelirep.Getcity().ToList(), "cityId", "vendorCity");
            ViewBag.state = new SelectList(objModelirep.Getstate().ToList(), "stateId", "vendorState");
            ViewBag.Getcategory = new SelectList(objModelirep.Getcategory().ToList(), "categoryId", "categoryName");
            ViewBag.Getservicetype = new SelectList(objModelirep.Getservicetype().ToList(), "serviceId", "natureofServicesCategory");

            return View(reportModel);
        }
        [HttpPost]
        public JsonResult Search(ReportModel model)
        {
            // public ActionResult Index(ReportModel model, string Agreementddl, string categoryddl, string VendorCityddl, string VendorStateddl, string ddlservicetypename)
            try
            {

                model.reportList = objModelirep.GetReport(model.ownerName, model.ownerEmployeeCode, model.vendorName, model.vendorCode
                    , model.typeofAgreement, model.categoryName
            , model.agreementEndDate
            , model.natureofServicesCategory
            , model.coordinatorcode
           , model.coordinatorName
           , model.vendorCity
            , model.vendorState
            , "CERTAIN"
                );
                if (model.reportList.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
                Session["reportlist"] = model.reportList;
                ViewBag.categoryID = model.categoryName;
                ViewBag.cityID = model.vendorCity;
                ViewBag.service = model.natureofServicesCategory;
                ViewBag.stateID = model.vendorState;
                ViewBag.contractID = model.typeofAgreement;
                ViewBag.contract = new SelectList(objModelirep.Getcontract().ToList(), "AgreementId", "typeofAgreement");
                ViewBag.city = new SelectList(objModelirep.Getcity().ToList(), "cityId", "vendorCity");
                ViewBag.state = new SelectList(objModelirep.Getstate().ToList(), "stateId", "vendorState");
                ViewBag.Getcategory = new SelectList(objModelirep.Getcategory().ToList(), "categoryId", "categoryName");
                ViewBag.Getservicetype = new SelectList(objModelirep.Getservicetype().ToList(), "serviceId", "natureofServicesCategory");
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult downloadsexcel()
        {
           
            List<ReportModel> cList;
            if (Session["reportlist"] == null)
            {
                cList = objModelirep.GetReport(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, "0", string.Empty, "0", string.Empty, string.Empty, "0", "0", "ALL");
            }
            else
            {
                cList = (List<ReportModel>)Session["reportlist"];
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("Owner Department");
            dt.Columns.Add("Owner Name");
            dt.Columns.Add("Owner Employee Code");
            dt.Columns.Add("Owner Email ID");
            dt.Columns.Add("Coordinator Department");
            dt.Columns.Add("Coordinator Name");
            dt.Columns.Add("Coordinator Employee Code");
            dt.Columns.Add("Coordinator Email ID");
            dt.Columns.Add("Vendor Category");
            dt.Columns.Add("Services Category");
            dt.Columns.Add("Vendor Name");
            dt.Columns.Add("Vendor Code");
            dt.Columns.Add("Vendor Address");
            dt.Columns.Add("Vendor State");
            dt.Columns.Add("Vendor City");
            dt.Columns.Add("Agreement Type");  
            dt.Columns.Add("Vendor Empanelment Date");
            dt.Columns.Add("Agreement End Date");
            dt.Columns.Add("Final Vendor Status");
            dt.Columns.Add("Vendor Validity Date");
            dt.Columns.Add("PAN Number");
            dt.Columns.Add("Procurement Coordinator Name");
            dt.Columns.Add("Procurement Coordinator Employee Code");
            dt.Columns.Add("Procurement Coordinator Email ID");
            dt.Columns.Add("Remarks");
            dt.Columns.Add("Vendor Coordinator Name");
            dt.Columns.Add("Vendor Office Number");
            dt.Columns.Add("Vendor Email ID");
            dt.Columns.Add("Vendor Website Address");
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                    i + 1
                    , cList[i].ownerDepartment.ToString()
                    , cList[i].ownerName.ToString()
                    , cList[i].ownerEmployeeCode.ToString()
                    , cList[i].ownerEmailID.ToString()
                    , cList[i].coordinatorDepartment.ToString()
                    , cList[i].coordinatorName.ToString()
                    , cList[i].coordinatorcode.ToString()
                    , cList[i].coordinatorEmailID.ToString()
                    , cList[i].categoryName.ToString()
                    , cList[i].natureofServicesCategory.ToString()
                    , cList[i].vendorName.ToString()
                    , cList[i].vendorCode.ToString()
                    , cList[i].vendorAddress.ToString()
                    , cList[i].vendorState.ToString()
                    , cList[i].vendorCity.ToString()
                    , cList[i].typeofAgreement.ToString()
                    , cList[i].vendorEmpanelmentDate.ToString()
                    , cList[i].agreementEndDate.ToString()
                    , cList[i].finalvendorStatus.ToString()
                    , cList[i].vendorValidityDate.ToString()
                    , cList[i].pANNumber.ToString()
                    , cList[i].procurementName.ToString()
                    , cList[i].procurementEmployeeCode.ToString()
                    , cList[i].procurementEmailID.ToString()
                    , cList[i].remarks.ToString()
                    , cList[i].vendorCoordinatorName.ToString()
                    , cList[i].vendorOfficeNumber.ToString()
                    , cList[i].vendorEmailID.ToString()
                    , cList[i].vendorWebsiteAddress.ToString()
                    );
            }
            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            Session["reportlist"] = gv;
            if (gv.Rows.Count != 0)
            {
                return new DownloadFileActionResult((GridView)Session["reportlist"], "Vendor Master Report.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }

    }
}



