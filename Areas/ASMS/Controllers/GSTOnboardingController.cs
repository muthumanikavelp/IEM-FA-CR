using IEM.Areas.ASMS.Models;
using IEM.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Providers.Entities;
using IEM.Areas.MASTERS.Controllers;
using System.Configuration;
using IEM.Helper;
using Newtonsoft.Json;
using IEM.App_Start;
namespace IEM.Areas.ASMS.Controllers
{

      [NoDirectAccess]
    public class GSTOnboardingController : Controller
    {
        CmnFunctions objCmnFunctions = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        private FileServer Cmnfs = new FileServer();
        dbLib db = new dbLib();

        private GSTIRepository objModel;
        string sLinkVar = "~/Areas/ASMS/Views/GSTOnboarding/", sExeVar = ".cshtml";

        public GSTOnboardingController()
            : this(new GSTSupDataModel())
        {

        }
        public GSTOnboardingController(GSTIRepository objM)
        {
            objModel = objM;
        }
        public JsonResult contracttodate()
        {
            DataTable dt = new DataTable();
            int result = 0;
            try
            {

                dt = objModel.getcontracttodate();
                result = Convert.ToInt32(dt.Rows[0]["datepickerextendedto"].ToString());
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Index(string pagefor = null, string supid = null, string isnewsupplier = null, string isFinancialReviewer = null)
        {
            try
            {
                Session["isApproved"] = "";
                DataTable dt = new DataTable();
                SupplierHeader sh = new SupplierHeader();
                Session["IsAllowApproverToEdit"] = "0";
                Session["isFinancialReviewer"] = null;
                if (pagefor == null)
                {
                    Session["PageMode"] = "1";
                }
                else
                {
                    Session["PageMode"] = pagefor;
                }
                if (supid != null)
                {
                    Session["SupplierHeaderGid"] = supid;
                }
                if (Session["PageMode"] != null || pagefor != null)
                {
                    if (pagefor == null)
                    {
                        pagefor = (string)Session["PageMode"];
                    }
                    switch (pagefor)
                    {
                        case "1":  //onboarding

                            sh._OwnerId = (Int32)objCmnFunctions.GetLoginUserGid();
                            dt = objCmnFunctions.GetLoginUserDetails(sh._OwnerId);
                            sh._OwnerCode = dt.Rows[0]["employee_code"].ToString();
                            sh._OwnerName = dt.Rows[0]["employee_name"].ToString();
                            sh.lstOrganizationType = new SelectList(objModel.GetOrganizationType(), "_OrganizationTypeID", "_OrganizationTypeName");
                            sh.lstServiceType = new SelectList(objModel.GetServiceType(), "_ServiceTypeID", "_ServiceTypeName");
                            sh.lstSupContactType = new SelectList(objModel.GetContactType(), "_SupContactTypeID", "_SupContactTypeName");
                            sh.lstSupplierCategory = new SelectList(objModel.GetSupplierCategory(), "_SupplierCategoryID", "_SupplierCategoryName");

                            if (Session["SupCode"] != null)
                            {
                                sh._SupplierCode = (string)Session["SupCode"];
                            }
                            dt = objModel.getcontracttodate();
                            ViewBag.RenewalDateInterval = Convert.ToInt32(dt.Rows[0]["datepickerextendedto"].ToString());

                            break;
                        case "2":  //edit
                            sh = objModel.GetSupHeaderDetailsByID();
                            sh.lstOrganizationType = new SelectList(objModel.GetOrganizationType(), "_OrganizationTypeID", "_OrganizationTypeName", sh.selectedOrganizationID);
                            sh.lstServiceType = new SelectList(objModel.GetServiceType(), "_ServiceTypeID", "_ServiceTypeName", sh.selectedServiceID);
                            sh.lstSupContactType = new SelectList(objModel.GetContactType(), "_SupContactTypeID", "_SupContactTypeName", sh.SelectedSupContactTypeID);
                            sh.lstSupplierCategory = new SelectList(objModel.GetSupplierCategory(), "_SupplierCategoryID", "_SupplierCategoryName", sh.SelectedSupplierCategoryID);
                            sh._NoOfDirectors = objModel.GetDirectorsCountForEdit();
                            Session["SupName"] = sh._SupplierName;
                               dt = objModel.getcontracttodate();
                            ViewBag.RenewalDateInterval = Convert.ToInt32(dt.Rows[0]["datepickerextendedto"].ToString());
                            sh._IsAllowApproverToEdit = "N";
                            sh._IsExistsFlagApprover = "N";
                            Session["IsAllowApproverToEdit"] = "0";
                            List<EntityGstvendor> records = new List<EntityGstvendor>();
                            records = objModel.getmaker().ToList();
                            ViewBag.IsChecker = records[0].IsChecker;
                            break;
                        case "3":  //view
                            sh = objModel.GetSupHeaderDetailsByID();                           
                            records = objModel.getmaker().ToList();
                            ViewBag.IsChecker = records[0].IsChecker;
                            sh.lstOrganizationType = new SelectList(objModel.GetOrganizationType(), "_OrganizationTypeID", "_OrganizationTypeName", sh.selectedOrganizationID);
                            sh.lstServiceType = new SelectList(objModel.GetServiceType(), "_ServiceTypeID", "_ServiceTypeName", sh.selectedServiceID);
                            sh.lstSupContactType = new SelectList(objModel.GetContactType(), "_SupContactTypeID", "_SupContactTypeName", sh.SelectedSupContactTypeID);
                            sh.lstSupplierCategory = new SelectList(objModel.GetSupplierCategory(), "_SupplierCategoryID", "_SupplierCategoryName", sh.SelectedSupplierCategoryID);
                            sh._NoOfDirectors = objModel.GetDirectorsCountForEdit();
                            DataTable dt1 = new DataTable();
                            dt1 = objModel.GetCurrentApprovalStageDetails();
                            sh._CurrentApprovalStageName = dt1.Rows[0]["approvalstage_name"].ToString();
                            sh._IsAllowApproverToEdit = dt1.Rows[0]["approvalstage_isallowedit"].ToString();

                            sh._IsExistsFlagApprover = objModel.IsExistsFlagApprover();
                            DataTable dtIsExistsFlagApprover = new DataTable();
                            dtIsExistsFlagApprover = objCmnFunctions.GetLoginUserDetails(Convert.ToInt32(objCmnFunctions.GetLoginUserGid()));
                            sh._IsExistsApproverName = (string.IsNullOrEmpty(dtIsExistsFlagApprover.Rows[0]["employee_code"].ToString()) ? "" : dtIsExistsFlagApprover.Rows[0]["employee_code"].ToString().ToUpper()) + "-" + (string.IsNullOrEmpty(dtIsExistsFlagApprover.Rows[0]["employee_name"].ToString()) ? "" : dtIsExistsFlagApprover.Rows[0]["employee_name"].ToString().ToUpper());
                            if (sh._IsAllowApproverToEdit == "Y")
                            {
                                Session["IsAllowApproverToEdit"] = "1";
                            }
                            else
                            {
                                Session["IsAllowApproverToEdit"] = "0";
                            }
                            Session["SupName"] = sh._SupplierName;
                            break;

                        case "4":  //submit
                            sh = objModel.GetSupHeaderDetailsByID();
                            sh.lstOrganizationType = new SelectList(objModel.GetOrganizationType(), "_OrganizationTypeID", "_OrganizationTypeName", sh.selectedOrganizationID);
                            sh.lstServiceType = new SelectList(objModel.GetServiceType(), "_ServiceTypeID", "_ServiceTypeName", sh.selectedServiceID);
                            sh.lstSupContactType = new SelectList(objModel.GetContactType(), "_SupContactTypeID", "_SupContactTypeName", sh.SelectedSupContactTypeID);
                            sh.lstSupplierCategory = new SelectList(objModel.GetSupplierCategory(), "_SupplierCategoryID", "_SupplierCategoryName", sh.SelectedSupplierCategoryID);
                            sh._NoOfDirectors = objModel.GetDirectorsCountForEdit();
                            Session["SupName"] = sh._SupplierName;
                            break;
                        case "5":  //view
                            sh = objModel.GetSupHeaderDetailsByID();
                            sh.lstOrganizationType = new SelectList(objModel.GetOrganizationType(), "_OrganizationTypeID", "_OrganizationTypeName", sh.selectedOrganizationID);
                            sh.lstServiceType = new SelectList(objModel.GetServiceType(), "_ServiceTypeID", "_ServiceTypeName", sh.selectedServiceID);
                            sh.lstSupContactType = new SelectList(objModel.GetContactType(), "_SupContactTypeID", "_SupContactTypeName", sh.SelectedSupContactTypeID);
                            sh.lstSupplierCategory = new SelectList(objModel.GetSupplierCategory(), "_SupplierCategoryID", "_SupplierCategoryName", sh.SelectedSupplierCategoryID);
                            sh._NoOfDirectors = objModel.GetDirectorsCountForEdit();
                            if (isFinancialReviewer == "yes")
                            {
                                Session["isFinancialReviewer"] = "yes";
                            }
                            break;
                        case "6":  //renewal
                            sh = objModel.GetSupHeaderDetailsByID();
                            sh.lstOrganizationType = new SelectList(objModel.GetOrganizationType(), "_OrganizationTypeID", "_OrganizationTypeName", sh.selectedOrganizationID);
                            sh.lstServiceType = new SelectList(objModel.GetServiceType(), "_ServiceTypeID", "_ServiceTypeName", sh.selectedServiceID);
                            sh.lstSupContactType = new SelectList(objModel.GetContactType(), "_SupContactTypeID", "_SupContactTypeName", sh.SelectedSupContactTypeID);
                            sh.lstSupplierCategory = new SelectList(objModel.GetSupplierCategory(), "_SupplierCategoryID", "_SupplierCategoryName", sh.SelectedSupplierCategoryID);
                            sh._NoOfDirectors = objModel.GetDirectorsCountForEdit();
                            Session["SupName"] = sh._SupplierName;
                            dt = objModel.getcontracttodate();
                            ViewBag.RenewalDateInterval = Convert.ToInt32(dt.Rows[0]["datepickerextendedto"].ToString());
                            break;
                        case "12":  //GST EDIT
                            sh = objModel.GetSupHeaderDetailsByID();
                            sh.lstOrganizationType = new SelectList(objModel.GetOrganizationType(), "_OrganizationTypeID", "_OrganizationTypeName", sh.selectedOrganizationID);
                            sh.lstServiceType = new SelectList(objModel.GetServiceType(), "_ServiceTypeID", "_ServiceTypeName", sh.selectedServiceID);
                            sh.lstSupContactType = new SelectList(objModel.GetContactType(), "_SupContactTypeID", "_SupContactTypeName", sh.SelectedSupContactTypeID);
                            sh.lstSupplierCategory = new SelectList(objModel.GetSupplierCategory(), "_SupplierCategoryID", "_SupplierCategoryName", sh.SelectedSupplierCategoryID);
                            sh._NoOfDirectors = objModel.GetDirectorsCountForEdit();
                            Session["SupName"] = sh._SupplierName;

                            //   dt = objModel.getcontracttodate();
                            //ViewBag.RenewalDateInterval = Convert.ToInt32(dt.Rows[0]["datepickerextendedto"].ToString());


                             DataTable dt2 = new DataTable();
                            dt2 = objModel.GetCurrentApprovalStageDetails();
                            sh._CurrentApprovalStageName = dt2.Rows[0]["approvalstage_name"].ToString();

                            //sh._IsAllowApproverToEdit = dt2.Rows[0]["approvalstage_isallowedit"].ToString();
                            //sh._IsExistsFlagApprover = objModel.IsExistsFlagApprover();

                           
                            DataTable dtIsExistsFlagApprovers = new DataTable();
                            dtIsExistsFlagApprovers = objCmnFunctions.GetLoginUserDetails(Convert.ToInt32(objCmnFunctions.GetLoginUserGid()));
                            if (dtIsExistsFlagApprovers.Rows.Count - 1 >= 0)
                            {
                                sh._IsExistsApproverName = (string.IsNullOrEmpty(dtIsExistsFlagApprovers.Rows[0]["employee_code"].ToString()) ? "" : dtIsExistsFlagApprovers.Rows[0]["employee_code"].ToString().ToUpper()) + "-" + (string.IsNullOrEmpty(dtIsExistsFlagApprovers.Rows[0]["employee_name"].ToString()) ? "" : dtIsExistsFlagApprovers.Rows[0]["employee_name"].ToString().ToUpper());
                            }
                            //if (sh._IsAllowApproverToEdit == "Y")
                            //{
                            //    Session["IsAllowApproverToEdit"] = "1";
                            //}
                            //else
                            //{s
                            //    Session["IsAllowApproverToEdit"] = "0";
                            //}
                            
 
                            break;
                        default:
                            break;
                    }
                }

                ViewBag.supplierheader = sh;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return View();
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Profile()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Profile(FormCollection collection)
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdateDirectors(DirectorModel objm)
        {
            try
            {
                objModel.UpdateDirector(objm);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            List<DirectorModel> lst = objModel.GetDirector().ToList();
            return PartialView(sLinkVar + "FindDirectors" + sExeVar, lst);

        }
        [HttpPost]
        public ActionResult InsertDirectors(DirectorModel objm)
        {
            try
            {
                objModel.InsertDirector(objm);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            List<DirectorModel> lst = objModel.GetDirector().ToList();
            return PartialView(sLinkVar + "FindDirectors" + sExeVar, lst);
        }
        [HttpPost]
        public ActionResult DeleteDirectors(DirectorModel objm)
        {
            try
            {
                int directorID = (int)objm._DirectorsID;
                objModel.DeleteDirector(directorID);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            List<DirectorModel> lst = objModel.GetDirector().ToList();
            return PartialView(sLinkVar + "FindDirectors" + sExeVar, lst);
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult FindDirectorsFields()
        {
            return PartialView();
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult FindDirectorsIndex()
        {
            return PartialView(sLinkVar + "FindDirectorsIndex" + sExeVar);
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult FindDirectors()
        {
            List<DirectorModel> lst = objModel.GetDirector().ToList();
            return PartialView(lst);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult CustomerDetails()
        {
            List<CustomersModel> lst = new List<CustomersModel>();
            return PartialView(lst);
        }

        [HttpPost]
        public JsonResult DeleteCustomer(CustomersModel custmodel)
        {
            try
            {
                int customerID = (int)custmodel._CustomerID;
                objModel.DeleteCustomer(customerID);
                TempData["CustomerSearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(custmodel, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult CreateCustomer()
        {
            CustomersModel objM = new CustomersModel();
            TempData["CustomerSearchItems"] = null;
            return PartialView(objM);
        }

        [HttpPost]
        public JsonResult CreateCustomer(CustomersModel objCustomerModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.InsertCustomer(objCustomerModel);
                }
                TempData["CustomerSearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objCustomerModel, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult EditCustomer(int id, string viewfor)
        {
            try
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
                TempData["CustomerSearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            CustomersModel model = objModel.GetCustomerById(id);
            return PartialView(sLinkVar + "EditCustomer" + sExeVar, model);
        }

        [HttpPost]
        public JsonResult EditCustomer(CustomersModel customersmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.UpdateCustomer(customersmodel);
                    TempData["CustomerSearchItems"] = null;
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(customersmodel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchCustomer(CustomersModel customersmodelSearch)
        {
            try
            {
                if (customersmodelSearch != null)
                {
                    string custName = Convert.ToString(customersmodelSearch._CustomerName) == null ? "" : Convert.ToString(customersmodelSearch._CustomerName);
                    string custService = Convert.ToString(customersmodelSearch._CustomerServiceName) == null ? "" : Convert.ToString(customersmodelSearch._CustomerServiceName);
                    string custContactPerson = Convert.ToString(customersmodelSearch._CustomerContactPerson) == null ? "" : Convert.ToString(customersmodelSearch._CustomerContactPerson);
                    string custMobileNo = Convert.ToString(customersmodelSearch._CustomerMobileNo) == null ? "" : Convert.ToString(customersmodelSearch._CustomerMobileNo);
                    string custAgeOfProduct = Convert.ToString(customersmodelSearch._CustomerAgeOfProduct) == null ? "" : Convert.ToString(customersmodelSearch._CustomerAgeOfProduct);
                    string custPhoneNo = Convert.ToString(customersmodelSearch._CustomerPhoneNo) == null ? "" : Convert.ToString(customersmodelSearch._CustomerPhoneNo);

                    List<CustomersModel> lst = new List<CustomersModel>();
                    lst = objModel.GetCustomer().ToList();
                    lst = lst.Where(x => custName == null ||
                       (x._CustomerName.ToUpper().Contains(custName.ToUpper())) &&
                       (x._CustomerContactPerson.ToUpper().Contains(custContactPerson.ToUpper())) &&
                       (x._CustomerMobileNo.ToUpper().Contains(custMobileNo.ToUpper())) &&
                       (x._CustomerPhoneNo.ToUpper().Contains(custPhoneNo.ToUpper())) &&
                        (x._CustomerPhoneNo.ToUpper().Contains(custAgeOfProduct.ToUpper())) &&
                       (x._CustomerServiceName.ToUpper().Contains(custService.ToUpper()))).ToList();
                    TempData["CustomerSearchItems"] = lst;
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(customersmodelSearch, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult InsertSupplierHeader(SupplierHeader objSupHeader)
        {
            try
            {
                objSupHeader._RequestType = "GSTONBOARDING";
                objSupHeader._Requeststatus = "DRAFT";
                objSupHeader._SupplierStatus = "";
                var supHeader = objCmnFunctions.GetSequenceNo("SUP", objSupHeader._SupplierName);
                objSupHeader._SupplierCode = supHeader.ToUpper();
                Session["SupCode"] = supHeader.ToUpper();
                objModel.InsertSupplierHeader(objSupHeader);
                Session["SupName"] = objSupHeader._SupplierName;
                Session["ServiceTypeId"] = objSupHeader.selectedServiceID;
                Session["SupplierHeaderGid"] = objModel.GetSupplierHeaderGid(objSupHeader);
                objSupHeader._HeaderID = Convert.ToInt32(Session["SupplierHeaderGid"]);
                List<SupplierHeader> objLst = new List<SupplierHeader>();
                objLst.Add(objSupHeader);
                objModel.UpdateSupHeaderGidDirectors();
                //  var result = (Int64)Session["SupplierHeaderGid"];
                return Json(objLst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult UpdateSupplierHeader(SupplierHeader objSupHeader)
        {
            try
            {
                string result="";
                if (Session["SupplierHeaderGid"] == null || Session["SupplierHeaderGid"] == "" || Session["SupplierHeaderGid"] == "0")
                    return Json(2, JsonRequestBehavior.AllowGet);
                objModel.UpdateSupplierHeader(objSupHeader);
                 //if (result == "Success")
                objModel.UpdateSupHeaderGidDirectors();
                // else
                    // return Json(2, JsonRequestBehavior.AllowGet);
                // else return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            return Json(objSupHeader, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult SupplierServiceDetails()
        {
            List<SupplierServiceDetails> lst = new List<SupplierServiceDetails>();
            return PartialView(lst);
        }

        [HttpPost]
        public JsonResult DeleteSupplierService(SupplierServiceDetails objServiceDetails)
        {
            try
            {
                int serviceID = (int)objServiceDetails._SupServiceDetailsID;
                objModel.DeleteSupplierServiceDetails(serviceID);
                TempData["ServiceSearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return Json(objServiceDetails, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult CreateSupplierService()
        {
            SupplierServiceDetails objM = new SupplierServiceDetails();
            TempData["ServiceSearchItems"] = null;
            return PartialView(objM);
        }

        [HttpPost]
        public JsonResult CreateSupplierService(SupplierServiceDetails objServiceDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.InsertSupplierServiceDetails(objServiceDetails);
                }
                TempData["ServiceSearchItems"] = null;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objServiceDetails, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult EditSupplierService(int id, string viewfor)
        {
            try
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
                TempData["ServiceSearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            SupplierServiceDetails model = objModel.GetSupplierServiceDetailsById(id);
            return PartialView(sLinkVar + "EditSupplierService" + sExeVar, model);
        }

        [HttpPost]
        public JsonResult EditSupplierService(SupplierServiceDetails objServiceDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.UpdateSupplierServiceDetails(objServiceDetails);
                    TempData["ServiceSearchItems"] = null;
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objServiceDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchSupplierService(SupplierServiceDetails objServiceDetails)
        {
            try
            {
                if (objServiceDetails != null)
                {
                    string serviceName = Convert.ToString(objServiceDetails._SupServiceDetailsName) == null ? "" : Convert.ToString(objServiceDetails._SupServiceDetailsName);

                    List<SupplierServiceDetails> lst = new List<SupplierServiceDetails>();
                    lst = objModel.GetSupplierServiceDetails().ToList();
                    lst = lst.Where(x => serviceName == null ||
                       (x._SupServiceDetailsName.ToUpper().Contains(serviceName.ToUpper()))).ToList();
                    TempData["ServiceSearchItems"] = lst;
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objServiceDetails, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult SubContractorDetails()
        {
            List<SubContractorDetails> lst = new List<SubContractorDetails>();
            return PartialView(lst);
        }

        [HttpPost]
        public JsonResult DeleteSubContractor(SubContractorDetails objSubContDetails)
        {
            try
            {
                int subcontractorID = (int)objSubContDetails._SubContractorID;
                objModel.DeleteSubContractorDetails(subcontractorID);
                TempData["SubContSearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return Json(objSubContDetails, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult CreateSubContractor()
        {
            SubContractorDetails objM = new SubContractorDetails();
            TempData["SubContSearchItems"] = null;
            return PartialView(objM);
        }

        [HttpPost]
        public JsonResult CreateSubContractor(SubContractorDetails objSubContDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.InsertSubContractorDetails(objSubContDetails);
                    TempData["SubContSearchItems"] = null;
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objSubContDetails, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult EditSubContractor(int id, string viewfor)
        {
            try
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
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            SubContractorDetails model = objModel.GetSubContractorDetailsById(id);
            TempData["SubContSearchItems"] = null;
            return PartialView(sLinkVar + "EditSubContractor" + sExeVar, model);
        }

        [HttpPost]
        public JsonResult EditSubContractor(SubContractorDetails objSubContDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.UpdateSubContractorDetails(objSubContDetails);
                    TempData["SubContSearchItems"] = null;
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objSubContDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchSubContractor(SubContractorDetails objSubContDetails)
        {
            try
            {
                if (objSubContDetails != null)
                {
                    string subContractorName = Convert.ToString(objSubContDetails._SubContractorName) == null ? "" : Convert.ToString(objSubContDetails._SubContractorName);
                    string subContractorServiceName = Convert.ToString(objSubContDetails._SubContractorServiceName) == null ? "" : Convert.ToString(objSubContDetails._SubContractorServiceName);

                    List<SubContractorDetails> lst = new List<SubContractorDetails>();
                    lst = objModel.GetSubContractorDetails().ToList();
                    lst = lst.Where(x => subContractorName == null ||
                       (x._SubContractorName.ToUpper().Contains(subContractorName.ToUpper())) &&
                       (x._SubContractorServiceName.ToUpper().Contains(subContractorServiceName.ToUpper()))).ToList();
                    TempData["SubContSearchItems"] = lst;
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objSubContDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult SupplierBranchDetails()
        {
            List<SupplierBranchDetails> lst = new List<SupplierBranchDetails>();
            return PartialView(lst);
        }

        [HttpPost]
        public JsonResult DeleteSupplierBranch(SupplierBranchDetails objSupBranchDetails)
        {
            try
            {
                int SupBranchID = (int)objSupBranchDetails._SupBranchID;
                objModel.DeleteSupplierBranchDetails(SupBranchID);
                TempData["BranchSearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return Json(objSupBranchDetails, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult CreateSupplierBranch()
        {
            SupplierBranchDetails objM = new SupplierBranchDetails();
            TempData["BranchSearchItems"] = null;
            return PartialView(objM);
        }

        [HttpPost]
        public JsonResult CreateSupplierBranch(SupplierBranchDetails objSupBranchDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.InsertSupplierBranchDetails(objSupBranchDetails);
                }
                TempData["BranchSearchItems"] = null;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objSupBranchDetails, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult EditSupplierBranch(int id, string viewfor)
        {
            try
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
                TempData["BranchSearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            SupplierBranchDetails model = objModel.GetSupplierBranchDetailsById(id);
            return PartialView(sLinkVar + "EditSupplierBranch" + sExeVar, model);
        }

        [HttpPost]
        public JsonResult EditSupplierBranch(SupplierBranchDetails objSupBranchetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.UpdateSupplierBranchDetails(objSupBranchetails);
                    TempData["BranchSearchItems"] = null;
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objSupBranchetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchSupplierBranch(SupplierBranchDetails objSupBranchDetails)
        {
            try
            {
                // objSupBranchDetails._SupBranchName = objModel.GetCityName(objSupBranchDetails._SupBranchID).ToString();
                if (objSupBranchDetails != null)
                {
                    int SupBranchName = Convert.ToInt32(objSupBranchDetails.selectedCityID);

                    List<SupplierBranchDetails> lst = new List<SupplierBranchDetails>();
                    lst = objModel.GetSupplierBranchDetails().ToList();
                    if (SupBranchName != 0)
                    {
                        lst = lst.Where(x => SupBranchName == null ||
                   (x.selectedCityID.Equals(SupBranchName))).ToList();
                    }

                    TempData["BranchSearchItems"] = lst;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objSupBranchDetails, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult SupAwardDetails()
        {
            List<SupAwardDetails> lst = new List<SupAwardDetails>();
            return PartialView(lst);
        }

        [HttpPost]
        public JsonResult DeleteSupAward(SupAwardDetails objSupAwardDetails)
        {
            try
            {
                int SupAwardID = (int)objSupAwardDetails._SupAwardID;
                objModel.DeleteSupAwardDetails(SupAwardID);
                TempData["SupAwardSearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return Json(objSupAwardDetails, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult CreateSupAward()
        {
            SupAwardDetails objM = new SupAwardDetails();
            TempData["SupAwardSearchItems"] = null;
            return PartialView(objM);
        }

        [HttpPost]
        public JsonResult CreateSupAward(SupAwardDetails objSupAwardDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.InsertSupAwardDetails(objSupAwardDetails);
                    TempData["SupAwardSearchItems"] = null;
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objSupAwardDetails, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult EditSupAward(int id, string viewfor)
        {
            try
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
                SupAwardDetails model = objModel.GetSupAwardDetailsById(id);
                TempData["SupAwardSearchItems"] = null;
                return PartialView(sLinkVar + "EditSupAward" + sExeVar, model);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                SupAwardDetails model1 = new SupAwardDetails();
                return PartialView(model1);
            }

        }

        [HttpPost]
        public JsonResult EditSupAward(SupAwardDetails objSupAwardDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.UpdateSupAwardDetails(objSupAwardDetails);
                    TempData["SupAwardSearchItems"] = null;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objSupAwardDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchSupAward(SupAwardDetails objSupAwardDetails)
        {
            try
            {
                if (objSupAwardDetails != null)
                {
                    string SupAwardName = Convert.ToString(objSupAwardDetails._SupAwardName) == null ? "" : Convert.ToString(objSupAwardDetails._SupAwardName);
                    string SupAwardDesc = Convert.ToString(objSupAwardDetails._SupAwardDesc) == null ? "" : Convert.ToString(objSupAwardDetails._SupAwardDesc);

                    List<SupAwardDetails> lst = new List<SupAwardDetails>();
                    lst = objModel.GetSupAwardDetails().ToList();
                    lst = lst.Where(x => SupAwardName == null ||
                       (x._SupAwardName.Contains(SupAwardName)) &&
                       (x._SupAwardDesc.Contains(SupAwardDesc))).ToList();
                    TempData["SupAwardSearchItems"] = lst;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objSupAwardDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult ClientDetails()
        {
            List<ClientDetails> lst = new List<ClientDetails>();
            return PartialView(lst);
        }

        [HttpPost]
        public JsonResult DeleteClient(ClientDetails objClientDetails)
        {
            try
            {
                int ClentID = (int)objClientDetails._ClientID;
                objModel.DeleteClient(ClentID);
                TempData["ClientSearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return Json(objClientDetails, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult CreateClient()
        {
            ClientDetails objM = new ClientDetails();
            TempData["ClientSearchItems"] = null;
            return PartialView(objM);
        }

        [HttpPost]
        public JsonResult CreateClient(ClientDetails objClientDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.InsertClient(objClientDetails);
                }
                TempData["ClientSearchItems"] = null;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objClientDetails, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult EditClient(int id, string viewfor)
        {
            try
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
                TempData["ClientSearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            ClientDetails model = objModel.GetClientById(id);
            return PartialView(sLinkVar + "EditClient" + sExeVar, model);
        }

        [HttpPost]
        public JsonResult EditClient(ClientDetails objClientDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.UpdateClient(objClientDetails);
                    TempData["ClientearchItems"] = null;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objClientDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchClient(ClientDetails objClientDetails)
        {
            try
            {
                if (objClientDetails != null)
                {
                    string ClientName = Convert.ToString(objClientDetails._ClientName) == null ? "" : Convert.ToString(objClientDetails._ClientName);
                    //  string ClientAgeOfProduct = Convert.ToString(objClientDetails._ClientAgeOfProduct) == null ? "" : Convert.ToString(objClientDetails._ClientAgeOfProduct);
                    string ClientAddress = Convert.ToString(objClientDetails._ClientAddress) == null ? "" : Convert.ToString(objClientDetails._ClientAddress);
                    // string ClientMobileNo = Convert.ToString(objClientDetails._ClientMobileNo) == null ? "" : Convert.ToString(objClientDetails._ClientMobileNo);
                    // string ClientPhoneNo = Convert.ToString(objClientDetails._ClientPhoneNo) == null ? "" : Convert.ToString(objClientDetails._ClientPhoneNo);

                    List<ClientDetails> lst = new List<ClientDetails>();
                    lst = objModel.GetClient().ToList();
                    lst = lst.Where(x => ClientName == null ||
                       (x._ClientName.ToUpper().Contains(ClientName.ToUpper())) &&
                       (x._ClientAddress.ToUpper().Contains(ClientAddress.ToUpper()))).ToList();
                    TempData["ClientSearchItems"] = lst;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objClientDetails, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult InsertSupplierProfile(SupplierProfile objSupProfile)
        {
            try
            {
                objModel.InsertSupplierProfile(objSupProfile);
                var result = objModel.GetSupplierProfileGid(objSupProfile);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult UpdateSupplierProfile(SupplierProfile objSupProfile)
        {
            try
            {
                objModel.UpdateSupplierProfile(objSupProfile);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objSupProfile, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetState(SupplierContactDetails objSupContact)
        {
            try
            {
                var CountryID = objSupContact.selectedCountryID;
                return Json(objModel.GetState(CountryID), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                List<SupplierContactDetails> lst = new List<SupplierContactDetails>();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetCity(SupplierContactDetails objSupContact)
        {
            try
            {
                var StateID = objSupContact.selectedStateID;
                var CountryID = objSupContact.selectedCountryID;
                return Json(objModel.GetCity(StateID, CountryID), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                List<SupplierContactDetails> lst = new List<SupplierContactDetails>();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult ContactDetailsFields()
        {
            List<SupplierContactDetails> mod = new List<SupplierContactDetails>();
            return PartialView();
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult ContactDetailsIndex()
        {
            List<SupplierContactDetails> mod = new List<SupplierContactDetails>();
            return PartialView(mod);
        }

        [HttpPost]
        public JsonResult DeleteContactDetails(SupplierContactDetails objContactDetails)
        {
            try
            {
                int ContactId = (int)objContactDetails._SupContactDetailsID;
                objModel.DeleteSupContactDetails(ContactId);
                TempData["ContactSearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return Json(objContactDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateContactDetails(SupplierContactDetails objContactDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.InsertSupContactDetails(objContactDetails);
                }
                TempData["ContactSearchItems"] = null;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objContactDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditContactDetailsSave(SupplierContactDetails objContactDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.UpdateSupContactDetails(objContactDetails);
                    TempData["ContactSearchItems"] = null;
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objContactDetails, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SearchContactDetails(SupplierContactDetails objContactDetails)
        {
            return Json(objContactDetails, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult ContactPersonDetailsFields()
        {
            List<SupplierContactPersonDetails> mod = new List<SupplierContactPersonDetails>();
            return PartialView();
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult ContactPersonDetailsIndex()
        {
            List<SupplierContactPersonDetails> mod = new List<SupplierContactPersonDetails>();
            return PartialView(mod);
        }

        [HttpPost]
        public JsonResult DeleteContactPersonDetails(SupplierContactPersonDetails objContactPersonDetails)
        {
            try
            {
                int ContactPersonId = (int)objContactPersonDetails._SupContactPersonID;
                objModel.DeleteSupContactPersonDetails(ContactPersonId);
                TempData["ContactPersonSearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return Json(objContactPersonDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateContactPersonDetails(SupplierContactPersonDetails objContactPersonDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.InsertSupContactPersonDetails(objContactPersonDetails);
                }
                TempData["ContactPersonSearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objContactPersonDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditContactPersonDetailsSave(SupplierContactPersonDetails objContactPersonDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.UpdateSupContactPersonDetails(objContactPersonDetails);
                    TempData["ContactPersonSearchItems"] = null;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objContactPersonDetails, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SearchContactPersonDetails(SupplierContactPersonDetails objContactPersonDetails)
        {
            return Json(objContactPersonDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult TaxDetails()
        {
            List<SupplierTaxDetails> mod = new List<SupplierTaxDetails>();
            return PartialView();
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult TaxDetailsFields()
        {
            List<SupplierTaxDetails> mod = new List<SupplierTaxDetails>();
            return PartialView();
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult TaxDetailsIndex()
        {
            List<SupplierTaxDetails> mod = new List<SupplierTaxDetails>();
            return PartialView(mod);
        }

        [HttpPost]
        public JsonResult DeleteTaxDetails(SupplierTaxDetails objTaxDetails)
        {
            try
            {
                int TaxDetailsId = (int)objTaxDetails._TaxDetailsID;
                objModel.DeleteSupTaxDetails(TaxDetailsId);
                TempData["TaxSearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return Json(objTaxDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateTaxDetails(SupplierTaxDetails objTaxDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.InsertSupTaxDetails(objTaxDetails);
                }
                TempData["TaxSearchItems"] = null;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objTaxDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditTaxDetailsSave(SupplierTaxDetails objTaxDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.UpdateSupTaxDetails(objTaxDetails);
                    TempData["TaxSearchItems"] = null;
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objTaxDetails, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SearchTaxDetails(SupplierTaxDetails objTaxDetails)
        {
            return Json(objTaxDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult SearchEmployee(string listfor, string formname)
        {
            List<SearchEmployee> lst = new List<SearchEmployee>();
            try
            {

                if (listfor == "search")
                {
                    if (Session["SearchEmployees"] != null)
                    {
                        lst = (List<SearchEmployee>)Session["SearchEmployees"];
                    }
                }
                else
                {
                    if (formname == "approver" || formname == "4")
                    {
                        lst = objModel.GetEmployeeListForApproval().ToList();
                        ViewBag.formname = "4";
                    }
                    else
                    {
                        lst = objModel.GetEmployeeList().ToList();
                    }

                }
                if (formname == "owner" || formname == "1")
                {
                    ViewBag.formname = "1";
                }
                else if (formname == "oic" || formname == "2")
                {
                    ViewBag.formname = "2";
                }
                else if (formname == "contactperson" || formname == "3")
                {
                    ViewBag.formname = "3";
                }

                if (ViewBag.formname != null || ViewBag.formname != "")
                {
                    Session["formname"] = ViewBag.formname;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return PartialView(lst);
        }
        [HttpPost]
        public JsonResult SearchEmployee(SearchEmployee objSearchEmployee)
        {
            var res = 0;
            try
            {

                List<SearchEmployee> objEmp = new List<SearchEmployee>();
                if (objSearchEmployee._EmployeeFor == "4")
                {
                    objEmp = objModel.GetEmployeeListForApproval().ToList();
                }
                else
                {
                    objEmp = objModel.GetEmployeeList().ToList();
                }

                if (objEmp != null)
                {
                    if ((string.IsNullOrEmpty(objSearchEmployee._EmployeeCode)) == false)
                    {
                        objEmp = objEmp.Where(x => objSearchEmployee._EmployeeCode == null ||
                            (x._EmployeeCode.ToUpper().Contains(objSearchEmployee._EmployeeCode.ToUpper()))).ToList();
                        Session["SearchEmployees"] = objEmp;
                    }
                    if ((string.IsNullOrEmpty(objSearchEmployee._EmployeeName)) == false)
                    {
                        objEmp = objEmp.Where(x => objSearchEmployee._EmployeeName == null ||
                            (x._EmployeeName.ToUpper().Contains(objSearchEmployee._EmployeeName.ToUpper()))).ToList();
                        Session["SearchEmployees"] = objEmp;
                    }
                }
                if (objEmp.Count() == 0)
                {
                    res = 0;
                }
                else
                {
                    res = 1;
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult OtherTaxView()
        {
            List<SupplierTaxDetails> mod = new List<SupplierTaxDetails>();
            return PartialView();
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult TdsDetails()
        {
            List<SupplierTaxDetails> mod = new List<SupplierTaxDetails>();
            return PartialView();
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult TdsFields()
        {
            List<SupplierTaxDetails> mod = new List<SupplierTaxDetails>();
            return PartialView();
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult TdsIndex(int taxdetailsID, string taxsubtype, string receiveflag = null, string viewtype = null, string headername = null)
        {
            try
            {
                if (taxsubtype != null && taxsubtype != "")
                {
                    Session["TaxSubType"] = taxsubtype;
                }
                if (taxdetailsID != null)
                {
                    Session["TaxDetailsID"] = taxdetailsID;
                }
                if (viewtype == "view")
                {
                    ViewBag.tdsviewmode = "view";
                    TempData["tdsviewmode"] = "view";
                }
                else if (viewtype == "edit")
                {
                    ViewBag.tdsviewmode = "edit";
                    TempData["tdsviewmode"] = "edit";
                }
                if (receiveflag != null && receiveflag != "")
                {
                    TempData["ReceiveFlag"] = null;
                    TempData["ReceiveFlag"] = receiveflag;
                }
                //if (headername != null && headername != "")
                //{
                //    TempData["headername"] = headername.ToUpper().Replace("_", " ");
                //}
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            SupplierTaxDetails mod = new SupplierTaxDetails();
            return PartialView(sLinkVar + "TdsIndex" + sExeVar, mod);
        }
        [HttpPost]
        public JsonResult GetTDSSection(SupplierTaxDetails objSupplierTaxDetails)
        {
            try
            {
                var TdsServiceTypeID = objSupplierTaxDetails.selectedTdsServiceTypeID;
                return Json(objModel.GetTdsSection(TdsServiceTypeID), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                List<SupplierTaxDetails> lst = new List<SupplierTaxDetails>();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public void UploadedFiles(string uploadfor, string fname = null)
        {
            try
            {
                string filename = "";

                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];

                    if (fileContent != null && fileContent.ContentLength > 0)
                    {

                        if (fname != null && fname.Trim() != "")
                        {
                            filename = fname.Substring(0, fname.LastIndexOf(".") + 0);
                        }
                        else
                        {
                            if (uploadfor != null)
                            {
                                filename = objCmnFunctions.GetSequenceNo(uploadfor);
                            }
                        }
                        var fileextension = Path.GetExtension(Path.GetFileName(fileContent.FileName));
                        var stream = fileContent.InputStream;
                        var CmnFilePath = objCmnFunctions.IEMAttachmentPath();
                        // var path = Path.Combine(@"C:\temp\", filename + fileextension);
                        var path = Path.Combine(CmnFilePath, filename + fileextension);
                        //using (var fileStream = System.IO.File.Create(path))
                        //{
                        //    stream.CopyTo(fileStream);
                        //}


                        byte[] bytFile = null;
                        using (var memoryStream = new MemoryStream())
                        {
                            stream.CopyTo(memoryStream);
                            bytFile = memoryStream.ToArray();
                            ViewBag.Result = "inside File Stream";
                        }
                        var FileString = Convert.ToBase64String(bytFile);

                        filename = filename + fileextension;

                        var result = Cmnfs.SaveFileToServer(FileString, filename).Result;
                        // filename = filename + fileextension;
                        Session["filenamesa"] = filename;
                        //  byte[] bytFile = System.IO.File.ReadAllBytes(path);
                        // FileServer objserver = new FileServer();
                        // string result =objserver.SaveFiles(bytFile, filename);
                        //if (result == "success")
                        //{
                        //    if (System.IO.File.Exists(path))
                        //    {
                        //       System.IO.File.Delete(path);
                        //    }
                        //}

                    }
                }
                //  return Json(filename);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                //  return Json("Upload failed");
            }
        }
        [HttpPost]
        public JsonResult UploadedFiles1(string fname, string uploadfor)
        {
            try
            {
                // string filename = "";
                //if (Request.Files.Count > 0)
                //{
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var CmnFilePath = objCmnFunctions.IEMAttachmentPath();
                    //  var path = Path.Combine(@"C:\temp\", fileName);
                    var path = Path.Combine(CmnFilePath, fileName);
                    file.SaveAs(path);
                    byte[] bytFile = System.IO.File.ReadAllBytes(path);
                    FileServer objserver = new FileServer();
                    string result = objserver.SaveFiles(bytFile, fileName);
                    if (result == "success")
                    {
                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }
                    }
                }
                // }
                //foreach (string file in Request.Files)
                //{
                //    var fileContent = Request.Files[file];

                //    if (fileContent != null && fileContent.ContentLength > 0)
                //    {

                //        if (fname != null && fname.Trim() != "")
                //        {
                //            filename = fname.Substring(0, fname.LastIndexOf(".") + 0);
                //        }
                //        else
                //        {
                //            if (uploadfor != null)
                //            {
                //                filename = objCmnFunctions.GetSequenceNo(uploadfor);
                //            }
                //        }
                //        var fileextension = Path.GetExtension(fileContent.FileName);
                //        var stream = fileContent.InputStream;
                //        var path = Path.Combine(@"C:\temp\", filename + fileextension);
                //        using (var fileStream = System.IO.File.Create(path))
                //        {
                //            stream.CopyTo(fileStream);
                //        }
                //        filename = filename + fileextension;
                //    }
                //}
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("0", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public PartialViewResult VatDetails()
        {
            List<SupplierTaxDetails> mod = new List<SupplierTaxDetails>();
            return PartialView();
        }
        [HttpGet]
        public PartialViewResult VatFields()
        {
            List<SupplierTaxDetails> mod = new List<SupplierTaxDetails>();
            return PartialView();
        }
        [HttpGet]
        public PartialViewResult VatIndex(int taxdetailsID, string viewtype = null)
        {
            try
            {
                if (taxdetailsID != null)
                {
                    Session["TaxDetailsID"] = taxdetailsID;
                }
                if (viewtype == "view")
                {
                    ViewBag.vatviewmode = "view";
                }
                else if (viewtype == "edit")
                {
                    ViewBag.vatviewmode = "edit";
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            SupplierTaxDetails mod = new SupplierTaxDetails();
            return PartialView(mod);
        }
        [HttpPost]
        public JsonResult DeleteVatDetails(SupplierTaxDetails objSupplierTaxDetails)
        {
            try
            {
                int VatId = (int)objSupplierTaxDetails._VatID;
                objModel.DeleteVatDetails(VatId);
                Session["VatSearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return Json(objSupplierTaxDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateVatDetails(SupplierTaxDetails objSupplierTaxDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.InsertVatDetails(objSupplierTaxDetails);
                }
                Session["VatSearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objSupplierTaxDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditVatDetailsSave(SupplierTaxDetails objSupplierTaxDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.UpdateVatDetails(objSupplierTaxDetails);
                    Session["VatSearchItems"] = null;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objSupplierTaxDetails, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SearchVatDetails(SupplierTaxDetails objSupplierTaxDetails)
        {
            try
            {
                if (objSupplierTaxDetails != null)
                {
                    string city = Convert.ToString(objSupplierTaxDetails._VatCity) == null ? "" : Convert.ToString(objSupplierTaxDetails._VatCity);
                    string rate = Convert.ToString(objSupplierTaxDetails._VatRate) == null ? "" : Convert.ToString(objSupplierTaxDetails._VatRate);

                    List<SupplierTaxDetails> lst = new List<SupplierTaxDetails>();
                    lst = objModel.GetVatDetails().ToList();
                    lst = lst.Where(x => city == null ||
                       (x._VatRate.ToString().StartsWith(rate)) &&
                       (x._VatCity.StartsWith(city))).ToList();
                    Session["VatSearchItems"] = lst;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objSupplierTaxDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteTdsDetails(SupplierTaxDetails objSupplierTaxDetails)
        {
            try
            {
                int TdsId = (int)objSupplierTaxDetails._TdsID;
                objModel.DeleteTdsDetails(TdsId);
                Session["TdsSearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return Json(objSupplierTaxDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateTdsDetails(SupplierTaxDetails objSupplierTaxDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.InsertTdsDetails(objSupplierTaxDetails);
                }
                Session["TdsSearchItems"] = null;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objSupplierTaxDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditTdsDetailsSave(SupplierTaxDetails objSupplierTaxDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.UpdateTdsDetails(objSupplierTaxDetails);
                    Session["TdsSearchItems"] = null;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objSupplierTaxDetails, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SearchTdsDetails(SupplierTaxDetails objSupplierTaxDetails)
        {
            try
            {
                if (objSupplierTaxDetails != null)
                {
                    string city = Convert.ToString(objSupplierTaxDetails._VatCity) == null ? "" : Convert.ToString(objSupplierTaxDetails._VatCity);
                    string rate = Convert.ToString(objSupplierTaxDetails._VatRate) == null ? "" : Convert.ToString(objSupplierTaxDetails._VatRate);

                    List<SupplierTaxDetails> lst = new List<SupplierTaxDetails>();
                    lst = objModel.GetVatDetails().ToList();
                    lst = lst.Where(x => city == null ||
                       (x._VatRate.ToString().StartsWith(rate)) &&
                       (x._VatCity.StartsWith(city))).ToList();
                    Session["TdsSearchItems"] = lst;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objSupplierTaxDetails, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult SupPaymentDetails()
        {
            List<PaymentDetails> mod = new List<PaymentDetails>();
            return PartialView();
        }
        [HttpGet]
        public PartialViewResult SupPaymentDetailsFields()
        {
            List<PaymentDetails> mod = new List<PaymentDetails>();
            return PartialView();
        }
        [HttpGet]
        public PartialViewResult SupPaymentIndex()
        {
            PaymentDetails mod = new PaymentDetails();
            return PartialView(mod);
        }
        [HttpPost]
        public JsonResult DeletepaymentDetails(PaymentDetails objSupPaymentDetails)
        {
            try
            {
                int PaymentId = (int)objSupPaymentDetails._PaymentID;
                objModel.DeletePaymentDetails(PaymentId);
                Session["PaymentSearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return Json(objSupPaymentDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreatePaymentDetails(PaymentDetails objSupPaymentDetails)
        {
            try
            {
                objModel.InsertPaymentDetails(objSupPaymentDetails);
                Session["PaymentSearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objSupPaymentDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditPaymentDetailsSave(PaymentDetails objSupPaymentDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.UpdatePaymentDetails(objSupPaymentDetails);
                    Session["PaymentSearchItems"] = null;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objSupPaymentDetails, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SearchPaymentDetails(PaymentDetails objSupPaymentDetails)
        {
            return Json(objSupPaymentDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult SupActivityDetails()
        {
            List<SupActivity> mod = new List<SupActivity>();
            return PartialView();
        }
        [HttpGet]
        public PartialViewResult SupActivityDetailsFields()
        {
            List<SupActivity> mod = new List<SupActivity>();
            return PartialView();
        }
        [HttpGet]
        public PartialViewResult SupActivityIndex()
        {
            SupActivity mod = new SupActivity();
            return PartialView(mod);
        }
        [HttpPost]
        public JsonResult DeleteActivityDetails(SupActivity objSupActivity)
        {
            try
            {
                int ActivityId = (int)objSupActivity._ActivityID;
                objModel.DeleteActivityDetails(ActivityId);
                Session["ActivitySearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return Json(objSupActivity, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateActivityDetails(SupActivity objSupActivity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.InsertActivityDetails(objSupActivity);
                }
                Session["ActivitySearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objSupActivity, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditActivityDetailsSave(SupActivity objSupActivity)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.UpdateActivityDetails(objSupActivity);
                    Session["ActivitySearchItems"] = null;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objSupActivity, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SearchActivityDetails(SupActivity objSupActivity)
        {
            return Json(objSupActivity, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetCategory(SupActivity objSupActivity)
        {
            try
            {
                var IsProdService = objSupActivity._Activitytype;
                return Json(objModel.GetCategory(IsProdService), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                List<SupActivity> lst = new List<SupActivity>();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetSubcategory(SupActivity objSupActivity)
        {
            try
            {
                var CategoryID = objSupActivity.selectedcategoryID;
                return Json(objModel.GetSubcategory(CategoryID), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                List<SupActivity> lst = new List<SupActivity>();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public PartialViewResult SupOthers()
        {
            SupOtherDetails mod = new SupOtherDetails();
            return PartialView();
        }

        [HttpPost]
        public JsonResult CreateOthersDetails(SupOtherDetails objSupOthers)
        {
            int result = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.InsertSupOthers(objSupOthers);
                    result = objModel.GetOthersID();
                }
                Session["RelationshipSearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditOthersdetailsSave(SupOtherDetails objSupOthers)
        {
            int result = 0;
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.UpdateSupOthers(objSupOthers);
                    result = objModel.GetOthersID();
                    Session["RelationshipSearchItems"] = null;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult RelationshipDetails()
        {
            List<SupOtherDetails> mod = new List<SupOtherDetails>();
            return PartialView();
        }
        [HttpGet]
        public PartialViewResult RelationshipFields()
        {
            List<SupOtherDetails> mod = new List<SupOtherDetails>();
            return PartialView();
        }

        [HttpPost]
        public JsonResult Deleterelationship(SupOtherDetails objSupOthers)
        {
            try
            {
                int RelationshipId = (int)objSupOthers._RelationshipID;
                objModel.DeleteEmpRelationship(RelationshipId);
                Session["RelationshipSearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return Json(objSupOthers, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateRelationshipDetails(SupOtherDetails objSupOthers)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.InsertEmpRelationship(objSupOthers);
                }
                Session["RelationshipSearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objSupOthers, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditRelationshipSave(SupOtherDetails objSupOthers)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.UpdateEmpRelationship(objSupOthers);
                    Session["RelationshipSearchItems"] = null;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objSupOthers, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SearchRelationship(SupOtherDetails objSupOthers)
        {
            return Json(objSupOthers, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult SupAttachmentDetails()
        {
            List<SupAttachment> mod = new List<SupAttachment>();
            return PartialView();
        }
        [HttpGet]
        public PartialViewResult SupAttachmentFields()
        {
            SupAttachment mod = new SupAttachment();
            return PartialView(mod);
        }
        [HttpGet]
        public PartialViewResult SupAttachmentIndex()
        {
            SupAttachment mod = new SupAttachment();
            return PartialView(mod);
        }
        [HttpPost]
        public JsonResult DeleteSupAttachment(SupAttachment objSupAttachment)
        {
            try
            {
                int AttachmentId = (int)objSupAttachment._SupAttachmentID;
                objModel.DeleteSupAttachment(AttachmentId);
                Session["AttachmentSearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objSupAttachment, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateSupAttachmentDetails(SupAttachment objSupAttachment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objSupAttachment._SupAttachedFileName = (string)Session["filenamesa"];
                    objModel.InsertSupAttachment(objSupAttachment);
                    string filename = (string)Session["filenamesa"];
                    // var path = Path.Combine(@"C:\temp\", filename);
                    // byte[] bytFile = System.IO.File.ReadAllBytes(path); 
                    // WebReference.Service1 ObjService = new WebReference.Service1();
                    //string result = ObjService.SaveFiles(bytFile, filename);
                    //if (result == "success")
                    //{
                    //    if (System.IO.File.Exists(path))
                    //    {
                    //        System.IO.File.Delete(path);
                    //    }
                    //}

                }
                Session["AttachmentSearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objSupAttachment, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditSupAttachmentSave(SupAttachment objSupAttachment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objModel.UpdateSupAttachment(objSupAttachment);
                    Session["AttachmentSearchItems"] = null;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objSupAttachment, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SearchSupAttachment(SupAttachment objSupAttachment)
        {
            return Json(objSupAttachment, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetDocumentName(SupAttachment objSupAttachment)
        {
            try
            {
                var DocGroupID = objSupAttachment.selectedDocumentGroupID;
                return Json(objModel.GetDocumentName(DocGroupID), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                List<SupAttachment> lst = new List<SupAttachment>();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetDocumentNameAll(SupAttachment objSupAttachment)
        {
            try
            {
                return Json(objModel.GetDocumentNameAll(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                List<SupAttachment> lst = new List<SupAttachment>();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetDocumentGroupByID(SupAttachment objSupAttachment)
        {
            try
            {
                var DocNameGid = objSupAttachment.selectedDocumentNameID;
                return Json(objModel.GetDocumentGroupById(DocNameGid), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                List<SupAttachment> lst = new List<SupAttachment>();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }
        public FileResult DownloadDocument(string id)
        {
            try
            {
                string filename = id;
                var CmnFilePath = objCmnFunctions.IEMAttachmentPath();
                var localpath = Path.Combine(CmnFilePath, filename);

                //FileServer ObjService = new FileServer();
                //var isExists = ObjService.CheckFileExists(filename);
                //if (isExists == 1)
                //{
                //    byte[] filebyte = ObjService.CopyFiles(filename);
                //    System.IO.File.WriteAllBytes(localpath, filebyte);
                //}

                //return File(localpath, System.Net.Mime.MediaTypeNames.Application.Octet, filename);

                var apiresult = Cmnfs.DownloadFile(id).Result;
                if (apiresult == "Fail")
                {
                    return File("", "");
                }
                else
                {
                    byte[] filebyte = Convert.FromBase64String(apiresult);
                    return File(filebyte, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult CheckFileExists(string id)
        {
            string result = "0";
            try
            {
                //FileServer ObjService = new FileServer();
                //var isExists = ObjService.CheckFileExists(id);
                //if (isExists == 1)
                //{
                //    result = "1";
                //}
                //return Json(result, JsonRequestBehavior.AllowGet);

                var apiresult = Cmnfs.DownloadFile(id).Result;
                if (apiresult != "Fail")
                {
                    result = "1";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetEmpGid(string empcode)
        {
            try
            {
                var EmpID = objModel.GetEmpGid(empcode);
                return Json(EmpID, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetNextApprovalStage(ApprovalModel objApprovalModel)
        {
            try
            {
                return Json(objModel.GetNextApprovalStage(objApprovalModel._RequestType, objApprovalModel._QueueCurrentLevel), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                List<ApprovalModel> lst = new List<ApprovalModel>();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult SubmitToQueue(ApprovalModel objApprovalModel)
        {
            try
            {
                if (Session["SupplierHeaderGid"] != null && Session["SupplierHeaderGid"].ToString() != "")
                {
                    GSTSupDataModel supmodel = new GSTSupDataModel();
                    DataTable dtForMail = objModel.GetMailDetails();
                    var reqType = dtForMail.Rows[0]["supplierheader_requesttype"].ToString();
                    var requestfor = dtForMail.Rows[0]["requestfor"].ToString();
                    var curapprovalstage = string.IsNullOrEmpty(dtForMail.Rows[0]["supplierheader_currentapprovalstage"].ToString()) ? "0" : dtForMail.Rows[0]["supplierheader_currentapprovalstage"].ToString();
                    var queueto = objApprovalModel._QueueTo == null ? "" : objApprovalModel._QueueTo;
                    var remarks = objApprovalModel._ActionRemarks == null ? "" : objApprovalModel._ActionRemarks;
                    var requesttype = objApprovalModel._RequestType == null ? "" : objApprovalModel._RequestType.ToString();
                    var actionstatus = Convert.ToInt32(objApprovalModel._ActionStatus);
                    var skipflag = Convert.ToInt32(objApprovalModel._SkipFlag);
                    requesttype = reqType.ToUpper().Trim();
                    var ErrMsg = objModel.SubmitToNextQueue(queueto, requesttype, remarks, actionstatus, skipflag);
                    if (ErrMsg == "success")
                    {
                        string gid = supmodel.GetSupIdForMail(Convert.ToInt32(Session["SupplierHeaderGid"]));
                        //objModel.GetSupplierHeaderGid(objSupHeader)
                        ForMailController mailsender = new ForMailController();
                        //DataTable dtForMail = objModel.GetMailDetails();
                        //var reqType = dtForMail.Rows[0]["supplierheader_requesttype"].ToString();
                        //var requestfor = dtForMail.Rows[0]["requestfor"].ToString();
                        if (requestfor != "S")
                        {
                            if (actionstatus == 1)
                            {
                                requestfor = "A";
                            }
                            else if (actionstatus == 0)
                            {
                                requestfor = "R";
                            }
                            if (curapprovalstage == "0")
                            {
                                requestfor = "S";
                            }
                            //  curapprovalstage = (Convert.ToInt32(curapprovalstage) - 1).ToString();
                        }
                        mailsender.sendusermail("ASMS", reqType.ToUpper().Trim(), gid, requestfor, curapprovalstage);
                    }

                    return Json(ErrMsg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Session Expired", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public PartialViewResult TaxAttachmentDetails()
        {
            List<SupplierTaxDetails> mod = new List<SupplierTaxDetails>();
            return PartialView();
        }
        [HttpGet]
        public PartialViewResult TaxAttachmentFields()
        {
            List<SupplierTaxDetails> mod = new List<SupplierTaxDetails>();
            return PartialView();
        }
        [HttpGet]
        public PartialViewResult TaxAttachmentIndex(int taxdetailsID, string viewtype = null)
        {
            try
            {
                if (taxdetailsID != null)
                {
                    Session["TaxDetailsID"] = taxdetailsID;
                }
                if (viewtype == "view")
                {
                    ViewBag.taxattachmentviewmode = "view";
                }
                else if (viewtype == "edit")
                {
                    ViewBag.taxattachmentviewmode = "edit";
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            SupplierTaxDetails mod = new SupplierTaxDetails();
            return PartialView(sLinkVar + "TaxAttachmentIndex" + sExeVar, mod);
        }
        [HttpPost]
        public JsonResult DeleteTaxAttachment(SupplierTaxDetails objTaxAttachment)
        {
            try
            {
                int taxattachmentId = (int)objTaxAttachment._TaxAttachmentID;
                objModel.DeleteTaxAttachment(taxattachmentId);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objTaxAttachment, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateTaxAttachment(SupplierTaxDetails objTaxAttachment)
        {
            try
            {
                objTaxAttachment._TaxAttachmentFileName = (string)Session["filenamesa"];
                objModel.InsertTaxAttachment(objTaxAttachment);
                string filename = (string)Session["filenamesa"];
                //var path = Path.Combine(@"C:\temp\", filename);
                //byte[] bytFile = System.IO.File.ReadAllBytes(path);
                //WebReference.Service1 ObjService = new WebReference.Service1();
                //string result = ObjService.SaveFiles(bytFile, filename);
                //if (result == "success")
                //{
                //    if (System.IO.File.Exists(path))
                //    {
                //        System.IO.File.Delete(path);
                //    }
                //}
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objTaxAttachment, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EditTaxAttachmentSave(SupplierTaxDetails objTaxAttachment)
        {
            try
            {
                objModel.UpdateTaxAttachment(objTaxAttachment);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objTaxAttachment, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateMsmed(SupplierTaxDetails objTaxAttachment)
        {
            try
            {
                objModel.UpdateMSMED(objTaxAttachment);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objTaxAttachment, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult ApprovalHistory()
        {
            List<SupplierHeader> lst = new List<SupplierHeader>();
            try
            {
                DataTable dt = new DataTable();

                lst = objModel.GetAllRequestHistory().ToList();
                dt = objModel.GetSupplierName(Convert.ToInt32(Session["SupplierHeaderGid"]));
                ViewBag.Code1 = dt.Rows[0]["supplierheader_suppliercode"].ToString();
                ViewBag.Name1 = dt.Rows[0]["supplierheader_name"].ToString().ToUpper();
                if (lst.Count > 0)
                {
                    ViewBag.nextapprover1 = dt.Rows[0]["nextapprover"].ToString().ToUpper();
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return PartialView(lst);
        }
        [HttpGet]
        public PartialViewResult SearchSupplierName(string listfor, string formname)
        {
            List<SupplierHeader> lst = new List<SupplierHeader>();
            try
            {
                if (listfor == "search")
                {
                    if (Session["SearchSupplierName"] != null)
                    {
                        lst = (List<SupplierHeader>)Session["SearchSupplierName"];
                    }
                }
                else
                {
                    lst = objModel.GetSupplierNameList().ToList();
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return PartialView(sLinkVar + "SearchSupplierName" + sExeVar, lst);
        }
        [HttpPost]
        public JsonResult SearchSupplierName(SupplierHeader objSearchSupplierName)
        {
            var res = 0;
            try
            {
                List<SupplierHeader> objSupName = new List<SupplierHeader>();
                objSupName = objModel.GetSupplierNameList().ToList();

                if (objSupName != null)
                {
                    if ((string.IsNullOrEmpty(objSearchSupplierName._SupplierName)) == false)
                    {
                        objSupName = objSupName.Where(x => objSearchSupplierName._SupplierName == null ||
                            (x._SupplierName.ToUpper().StartsWith(objSearchSupplierName._SupplierName.ToUpper()))).ToList();
                        Session["SearchSupplierName"] = objSupName;
                    }
                }
                if (objSupName.Count() == 0)
                {
                    res = 0;
                }
                else
                {
                    res = 1;
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult SearchContactPersonName(string listfor)
        {
            List<SupActivity> lst = new List<SupActivity>();
            try
            {
                if (listfor == "search")
                {
                    if (Session["SearchContactPersonName"] != null)
                    {
                        lst = (List<SupActivity>)Session["SearchContactPersonName"];
                    }
                }
                else
                {
                    lst = objModel.GetContactPersonNameList().ToList();
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return PartialView(lst);
        }
        [HttpPost]
        public JsonResult SearchContactPersonName(SupActivity ObjSearchContactPersonName)
        {
            var res = 0;
            try
            {
                List<SupActivity> objContactPersonName = new List<SupActivity>();
                objContactPersonName = objModel.GetContactPersonNameList().ToList();

                if (objContactPersonName != null)
                {
                    if ((string.IsNullOrEmpty(ObjSearchContactPersonName._ContactPersonName)) == false)
                    {
                        objContactPersonName = objContactPersonName.Where(x => ObjSearchContactPersonName._ContactPersonName == null ||
                            (x._ContactPersonName.ToUpper().StartsWith(ObjSearchContactPersonName._ContactPersonName.ToUpper()))).ToList();
                        Session["SearchContactPersonName"] = objContactPersonName;
                    }
                }
                if (objContactPersonName.Count() == 0)
                {
                    res = 0;
                }
                else
                {
                    res = 1;
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateFinReviewStatus(SupplierTaxDetails objtaxdet)
        {
            try
            {
                objModel.UpdateFinReviewStatus();
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(0, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public PartialViewResult SearchSupplierPanNo(string listfor, string formname)
        {
            List<SupplierHeader> lst = new List<SupplierHeader>();
            try
            {
                if (listfor == "search")
                {
                    if (Session["SearchSupplierPanNo"] != null)
                    {
                        lst = (List<SupplierHeader>)Session["SearchSupplierPanNo"];
                    }
                }
                else
                {
                    lst = objModel.GetSupplierPanNoList().ToList();
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return PartialView(sLinkVar + "SearchSupplierPanNo" + sExeVar, lst);
        }
        [HttpPost]
        public JsonResult SearchSupplierPanNo(SupplierHeader objSearchSupplierPanNo)
        {
            var res = 0;
            try
            {
                List<SupplierHeader> objSupName = new List<SupplierHeader>();
                objSupName = objModel.GetSupplierPanNoList().ToList();

                if (objSupName != null)
                {
                    if ((string.IsNullOrEmpty(objSearchSupplierPanNo._PanNo)) == false)
                    {
                        objSupName = objSupName.Where(x => objSearchSupplierPanNo._PanNo == null ||
                            (x._PanNo.ToUpper().StartsWith(objSearchSupplierPanNo._PanNo.ToUpper()))).ToList();
                        Session["SearchSupplierPanNo"] = objSupName;
                    }
                }
                if (objSupName.Count() == 0)
                {
                    res = 0;
                }
                else
                {
                    res = 1;
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult TdsAttachmentDetails()
        {
            List<SupplierTaxDetails> mod = new List<SupplierTaxDetails>();
            return PartialView();
        }
        [HttpGet]
        public PartialViewResult TdsAttachmentFields()
        {
            SupplierTaxDetails mod = new SupplierTaxDetails();
            return PartialView(mod);
        }
        [HttpGet]
        public PartialViewResult TdsAttachmentIndex(int tdsdetailsID = 0, string viewtype = null)
        {
            try
            {
                if (tdsdetailsID != 0)
                {
                    Session["TdsDetailsGid"] = tdsdetailsID;
                }
                if (viewtype == "view")
                {
                    TempData["tdsattachmentviewmode2"] = "view";
                }
                else if (viewtype == "edit")
                {
                    TempData["tdsattachmentviewmode2"] = "edit";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            SupplierTaxDetails mod = new SupplierTaxDetails();
            return PartialView(sLinkVar + "tdsAttachmentIndex" + sExeVar, mod);
        }
        [HttpPost]
        public JsonResult DeleteTdsAttachment(SupplierTaxDetails objTdsAttachment)
        {
            try
            {
                int AttachmentId = (int)objTdsAttachment._TdsAttachmentID;
                objModel.DeleteTdsAttachment(AttachmentId);
                Session["TdsAttachmentSearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objTdsAttachment, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CreateTdsAttachmentDetails(SupplierTaxDetails objTdsAttachment)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    objTdsAttachment._TdsAttachmentFileName = (string)Session["filenamesa"];
                    objModel.InsertTdsAttachment(objTdsAttachment);
                    string filename = (string)Session["filenamesa"];
                    //var path = Path.Combine(@"C:\temp\", filename);
                    //byte[] bytFile = System.IO.File.ReadAllBytes(path);
                    //WebReference.Service1 ObjService = new WebReference.Service1();
                    //string result = ObjService.SaveFiles(bytFile, filename);
                    //if (result == "success")
                    //{
                    //    if (System.IO.File.Exists(path))
                    //    {
                    //        System.IO.File.Delete(path);
                    //    }
                    //}
                }
                Session["TdsAttachmentSearchItems"] = null;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objTdsAttachment, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CheckSupplierName(SupplierHeader objSupHeader1)
        {
            int liresult = 0;
            try
            {
                string SupName = (string)objSupHeader1._SupplierName;
                string PanNo = (string)objSupHeader1._PanNo;
                liresult = objModel.CheckSupNameIsExists(SupName, PanNo);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(liresult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CheckSupplierPanNo(SupplierHeader objSupHeader1)
        {
            int liresult = 0;
            try
            {
                string SupPanNo = (string)objSupHeader1._PanNo;
                liresult = objModel.CheckSupPanNoIsExists(SupPanNo);

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(liresult, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public PartialViewResult SupAttachmentDetailsConfirm()
        {
            List<SupAttachment> mod = new List<SupAttachment>();
            return PartialView();
        }

        [HttpPost]
        public JsonResult GetSupNameForPayment(SupplierHeader objSupHeaderDetails)
        {
            try
            {
                var SupName = objModel.GetSupplierNameForPayment();
                return Json(SupName, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CheckSupplierIsLocked(SupplierHeader objSupHeaderDetails)
        {
            List<SupplierHeader> lst = new List<SupplierHeader>();
            try
            {
                lst = objModel.IsSupplierLocked(objSupHeaderDetails._SupplierCode).ToList();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult LockMySupplier(SupplierHeader objSupHeaderDetails)
        {
            try
            {
                objModel.LockMySupplier(objSupHeaderDetails._SupplierCode);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult ReleaseMySupplier(SupplierHeader objSupHeaderDetails)
        {
            try
            {
                objModel.ReleaseMySupplier(objSupHeaderDetails._SupplierCode);
                return Json(1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult CheckApproverIsExists(SupplierHeader objSupHeaderDetails)
        {
            try
            {
                var isExistsApprover = objModel.IsExistsFlagApprover();
                return Json(isExistsApprover, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetSupplierPanNumber(SupplierHeader objSupHeader1)
        {
            string SupPanNo = "";
            try
            {
                SupPanNo = objModel.GetSupplierPanNumber();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(SupPanNo, JsonRequestBehavior.AllowGet);
        }
        public FileResult DownloadTemplates(string id)
        {
            string filename = "";
            string path1 = "";
            try
            {
                filename = id;
                path1 = Server.MapPath("~/App_Code");

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            var path = Path.Combine(path1, filename);
            return File(path, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }
        [HttpPost]
        public JsonResult DeleteSupplier(SupplierHeader objSupHeader1)
        {
            string result = "0";
            try
            {
                result = objModel.DeleteSupplier(objSupHeader1._SupplierCode);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // *************>      GST  <**************************
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult GSTIndex()
        {
             
            //----------------

            List<EntityGstvendor> records = new List<EntityGstvendor>();
            records.Clear();
            records = objModel.getmaker().ToList();
            ViewBag.IsChecker = records[0].IsChecker;
            ViewBag.records = objModel.getvendor().ToList();
            if (records.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            ////return View(records);
            return PartialView(records);
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult GSTDetailsIndex()
        {
            //List<EntityGstvendor> mod = new List<EntityGstvendor>();
            return PartialView();
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult GST_Create_Vendor()
        {

            EntityGstvendor gv = new EntityGstvendor();
            gv.GetState = new SelectList(objModel.GetState(), "suppliergst_stateid", "suppliergst_state", gv.selectedstate_gid);
            ViewBag.supplierheader = gv;
            string filename = (string)Session["SupplierHeaderGid"];
            ViewBag.urlname = filename;
            return PartialView(sLinkVar + "GST_Create_Vendor" + sExeVar);
        }

        [HttpPost]
        public JsonResult CreateVendor(EntityGstvendor pinmodel)
        {
            try
            {
                string dn = "";
                dn = objModel.Insertvendor(pinmodel);
                string filename = (string)Session["SupplierHeaderGid"];
                string PageMode = (string)Session["PageMode"];
                RedirectToAction("../GSTOnboarding?pagefor='" + PageMode + "'&supid='" + filename + "'&' + new Date().getTime()");
                return Json(dn, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult GST_Edit_Vendor(int id, string viewfor)
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
           // EntityGstvendor gv = new EntityGstvendor();
           // gv = objModel.getVendorid(id);
           // gv.GetState = new SelectList(objModel.GetState(), "suppliergst_stateid", "suppliergst_state", gv.selectedstate_gid);
           //// return PartialView(TypeModel);
           // ViewBag.supplierheader = gv;
           // return PartialView(sLinkVar + "GST_Edit_Vendor" + sExeVar);
            List<EntityGstvendor> records = new List<EntityGstvendor>();
            records = objModel.getmaker().ToList();
            ViewBag.IsChecker = records[0].IsChecker;

            EntityGstvendor TypeModel = objModel.getVendorid(id);
            TypeModel.GetState = new SelectList(objModel.GetState(), "suppliergst_stateid", "suppliergst_state", TypeModel.selectedstate_gid);
            return PartialView(TypeModel);
        }
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GST_Edit_Vendor(EntityGstvendor pinmodel)
        {
            string result = string.Empty;
            try
            {
                string dn = "";
                dn = objModel.Updatevendor(pinmodel);
                string filename = (string)Session["SupplierHeaderGid"];
                string PageMode = (string)Session["PageMode"];
                RedirectToAction("../GSTOnboarding?pagefor='" + PageMode + "'&supid='" + filename + "'&' + new Date().getTime()");
                return Json(dn, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
             
        }

        // *************>      GST  <**************************

        [HttpPost]
        public JsonResult Getcitylist(SupplierContactDetails objSupContact)
        {
            try
            {
                string CountryID = objSupContact._PinCode.ToString();

                string Data1 = "";
                DataSet ds = db.Getstatebypincode(CountryID);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                //dt = ds.Tables[1];
                //if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }


                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                List<SupplierContactDetails> lst = new List<SupplierContactDetails>();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetDistrictpincode(SupplierContactDetails objSupContact)
        {
            try
            {
                var Pincodes = objSupContact._PinCode;
                string sss = Pincodes.ToString();
                return Json(objModel.GetDistrictpincode(sss), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                List<SupplierContactDetails> lst = new List<SupplierContactDetails>();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }
        
    }
}
