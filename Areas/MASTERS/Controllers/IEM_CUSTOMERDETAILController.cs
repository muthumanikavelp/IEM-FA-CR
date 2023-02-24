using System;
using IEM.Common;
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
using ClosedXML.Excel;
using IEM.Areas.MASTERS.Models;
using IEM.App_Start;
namespace IEM.Areas.MASTERS.Controllers
{
     [NoDirectAccess]
    public class IEM_CUSTOMERDETAILController : Controller
    {
        CmnFunctions objCmnFunctions = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        private Iiem_mst_CustomerDetail  ist;
        string sLinkVar = "~/Areas/MASTERS/Views/IEM_CUSTOMERDETAIL/", sExeVar = ".cshtml";

        //
        // GET: /MASTERS/IEM_CUSTOMERDETAIL/

        public IEM_CUSTOMERDETAILController() :
            this(new CustmerDetail_DataModel()) { }

       public IEM_CUSTOMERDETAILController(Iiem_mst_CustomerDetail Objist)
        {
            ist = Objist;
        }

         [HttpGet]
        public ActionResult Index()
        {


            string mt = null;
            Session["exceldownload"] = null;
            CustomerdetailModel records = new CustomerdetailModel();
            records.CustomerDetailList = ist.GetCustomerDetail(mt).ToList();
             return View(records);


        }
         [HttpPost]
         public ActionResult Index(string filter = null, string filter1 = null, string filter2 = null, string command = null)
            
         {
              string mt = null;
              CustomerdetailModel records = new  CustomerdetailModel();
             if (command == "Search" || command == "Refresh")
             {
                 records.CustomerDetailList = ist.GetCustomerDetailById(filter, filter1, filter2).ToList();
                 if (records.CustomerDetailList.Count == 0)
                 {
                     ViewBag.Message = "No Records Found";
                 }
                 @ViewBag.filter = filter;
                 @ViewBag.filter1 = filter1;
                 @ViewBag.filter2 = filter2;
             }
             else if (command == "Clear")
             {
                 records.CustomerDetailList = ist.GetCustomerDetail(mt).ToList();
             }
             return View(records);
         }

      [HttpGet]
        public PartialViewResult Create()
        {
             
            List<CustomerdetailModel> mod = new List<CustomerdetailModel>();
            //records.stateMod = new SelectList(ist.GetState(), "Stateid", "Statename", records.stateid);
            //records.DistrictMod = new SelectList(ist.GetDistrict(), "DistrictiD", "Districtname", records.districtID);
            //records.PincodeMod = new SelectList(ist.GetPincode(), "PincodeID", "Pincode", records.PincodeID);
            return PartialView();
        }

      [HttpGet]
      public PartialViewResult Header(string id, string viewfor,string pagefor=null)
      {

          CustomerDetailHeader sh = new CustomerDetailHeader();
          Session["CustCode"] = null;
          Session["CustName"] = null;
          Session["CustomerHeaderGid"] = null;
          Session["PageMode"] = "1";
           
          if (viewfor == "edit")
          {   
              ViewBag.viewfor = "edit";
              Session["CustomerHeaderGid"] = Convert.ToInt32(id.ToString());
              sh = ist.GetcustomerdetailHeader(id);
              ViewBag.customerheader=sh;
          }
          else if (viewfor == "view")
          {
              ViewBag.viewfor = "view";
              Session["CustomerHeaderGid"] = Convert.ToInt32(id.ToString());
              sh = ist.GetcustomerdetailHeader(id);
              ViewBag.customerheader = sh;
          }
          else if (viewfor == "delete")
          {
              ViewBag.viewfor = "delete";
          }
          else if(viewfor=="create")
          {
              ViewBag.viewfor = "create";
              sh = ist.GetcustomerdetailHeader("0");
              ViewBag.customerheader = sh;
          }

          return PartialView();
      }

 
      //[HttpPost]
      //public PartialViewResult Header(string id, string viewfor, CustomerDetailHeader status)
      //{

      //    try
      //    {

      //        status = ist.GetcustomerdetailHeader(id);
 
      //        return PartialView(status);
      //    }
      //    catch (Exception ex)
      //    {
      //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
      //        return PartialView();
      //    }
      //    finally
      //    {
      //    }
      //}



        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult CustomerDetailsIndex()
        {
            List<CustomerdetailModel> mod = new List<CustomerdetailModel>();
            return PartialView(mod);
        }


        [HttpPost]
        public JsonResult Getcitylist(CustomerdetailModel objCustContact)
        {
            try
            {
                var PinCode = objCustContact._PinCode;
                return Json(ist.Getcitylist(PinCode), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                List<CustomerdetailModel> lst = new List<CustomerdetailModel>();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CreateCustomerDetails(CustomerdetailModel objContactDetails)
        {
            try
            {
                string result = "";
                if (ModelState.IsValid)
                {
                  result=  ist.InsertCustomerDetail(objContactDetails);
                }

                if (result == "success") RedirectToAction("Index");
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(objContactDetails, JsonRequestBehavior.AllowGet);
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
            CustomerdetailModel records = new CustomerdetailModel();
            records = ist.GetCustomerDetailId(id);
            string mt = null;
            //records.PincodeMod = new SelectList(ist.GetPincode(), "PincodeID", "Pincode", records.PincodeID);
            //records.stateMod = new SelectList(ist.GetAllState(), "stateid", "statename", records.stateid);
            //records.DistrictMod = new SelectList(ist.GetAllDistrict(), "districtID", "districtname", records.districtID);
            return PartialView(records);

        }




        [HttpPost]
        public JsonResult Edit(CustomerdetailModel CustomerDetailModel)
        {
            try
            {
                string check = ist.EditCustomerDetail(CustomerDetailModel);
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
                ist.EditCustomerDetail(CustomerDetailModel);

            }
        }


        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /MASTERS/IEM_CITY/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public JsonResult DeleteCustomerDetail(CustomerdetailModel customerdetail)
        {
            try
            {

                ist.DeleteCustomerDetail(customerdetail.CustomerID);
                return Json("Record deleted successfully !", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult DeleteCustomerMasterDetail(CustomerdetailModel customerdetail)
        {
            try
            {

                ist.DeleteCustomerMasterDetail(customerdetail.CustomerID);
                return Json("Sucess", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult ApproveCustomerMasterDetail(EntityGstCustomer pinmodel)
        {
            string dn;
            try
            {
                if (pinmodel.customergst_app == "N")
                {
                    ist.Insertcustomer(pinmodel);
                }
               
                    dn = ist.ApproveCustomerMasterDetail();
                    return Json(dn, JsonRequestBehavior.AllowGet);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult ExportCustomerDetail()
        {
            string mt = null;
             
            DataTable dt = new DataTable();
            dt = ist.GetCustomerDetailexcel();
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Customerdetail");

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=CustomerDetailList.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
                return View();
                // return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CheckCustomerName(CustomerDetailHeader objCustHeader1)
        {
            int liresult = 0;
            try
            {
                string CustName = (string)objCustHeader1._CustomerName;
                string PanNo = (string)objCustHeader1._PanNo;
                string CustCode = Convert.ToString(objCustHeader1._CustomerCode);
                liresult = ist.CheckCustNameIsExists(CustName, PanNo);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(liresult, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CheckCustomerPanNo(CustomerDetailHeader objCustHeader1)
        {
            int liresult = 0;
            try
            {
                string SupPanNo = (string)objCustHeader1._PanNo;
                liresult = ist.CheckCustPanNoIsExists(SupPanNo);

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(liresult, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public JsonResult InsertCustomerHeader(CustomerDetailHeader objCustHeader)
        {
            try
            {
                //objCustHeader._RequestType = "ONBOARDING";
                //objCustHeader._Requeststatus = "DRAFT";
                if (objCustHeader._Savemode == "Save")
                {
                    objCustHeader._CustomerStatus = "A";
                    var custHeader = objCmnFunctions.GetSequenceNo("SUP", objCustHeader._CustomerName);
                    objCustHeader._CustomerCode = custHeader.ToUpper();
                    Session["CustCode"] = custHeader.ToUpper();
                    ist.InsertCustomerHeaderDetails(objCustHeader);
                    Session["CustName"] = objCustHeader._CustomerName;
                    //Session["ServiceTypeId"] = objCustHeader.selectedServiceID;
                    Session["CustomerHeaderGid"] = ist.GetCustomerHeaderGid(objCustHeader);
                    objCustHeader._HeaderID = Convert.ToInt32(Session["CustomerHeaderGid"]);
                    List<CustomerDetailHeader> objLst = new List<CustomerDetailHeader>();
                    objLst.Add(objCustHeader);
                    ////ist.UpdateSupHeaderGidDirectors();
                    //  var result = (Int64)Session["SupplierHeaderGid"];
                    return Json(objLst, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    objCustHeader._CustomerStatus = "A";
                    //var custHeader = objCmnFunctions.GetSequenceNo("SUP", objCustHeader._CustomerName);
                    //objCustHeader._CustomerCode = custHeader.ToUpper();
                    //Session["CustCode"] = custHeader.ToUpper();
                    Session["CustomerHeaderGid"] = ist.GetCustomerHeaderGid(objCustHeader);
                    objCustHeader._HeaderID = Convert.ToInt32(Session["CustomerHeaderGid"]);
                    objCustHeader._CustomerStatus = ist.UpdateCustomerHeader(objCustHeader);
                    Session["CustName"] = objCustHeader._CustomerName;
                    //Session["ServiceTypeId"] = objCustHeader.selectedServiceID;

                    List<CustomerDetailHeader> objLst = new List<CustomerDetailHeader>();
                    objLst.Add(objCustHeader);
                    ////ist.UpdateSupHeaderGidDirectors();
                    //  var result = (Int64)Session["SupplierHeaderGid"];
                    return Json(objLst, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult ContactDetailsFields()
        {
            List<CustomerContactDetails> mod = new List<CustomerContactDetails>();
            return PartialView();
        }
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult ContactDetailsIndex()
        {
            List<CustomerContactDetails> mod = new List<CustomerContactDetails>();
            return PartialView(mod);
        }

        [HttpPost]
        public JsonResult CreateContactDetails(CustomerContactDetails objContactDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ist.InsertCustContactDetails(objContactDetails);
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
        public JsonResult EditContactDetailsSave(CustomerContactDetails objContactDetails)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ist.UpdateCustContactDetails(objContactDetails);
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
        public JsonResult SearchContactDetails(CustomerContactDetails objContactDetails)
        {
            return Json(objContactDetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetState(CustomerContactDetails objCustContact)
        {
            try
            {
                var CountryID = objCustContact.selectedCountryID;
                return Json(ist.GetState(CountryID), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                List<CustomerContactDetails> lst = new List<CustomerContactDetails>();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetCity(CustomerContactDetails objCustContact)
        {
            try
            {
                var StateID = objCustContact.selectedStateID;
                var CountryID = objCustContact.selectedCountryID;
                return Json(ist.GetCity(StateID, CountryID), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                List<CustomerContactDetails> lst = new List<CustomerContactDetails>();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost] // pandiaraj district add in contact tab 05/07/2017
        public JsonResult GetDistrictpincode(CustomerContactDetails objCustContact)
        {
            try
            {
                var Pincodes = objCustContact._PinCode;
                string sss = Pincodes.ToString();
                return Json(ist.GetDistrictpincode(sss), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                List<CustomerContactDetails> lst = new List<CustomerContactDetails>();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Getcitylists(CustomerContactDetails objCustContact)
        {
            try
            {
                var CountryID = objCustContact._PinCode;
                return Json(ist.Getcitylists(CountryID), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                List<CustomerContactDetails> lst = new List<CustomerContactDetails>();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }

        // *************>      GST  <**************************
        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult GSTIndex()
        {

            //----------------

            List<EntityGstCustomer> records = new List<EntityGstCustomer>();
            records.Clear();

            ViewBag.hdnFlag = Convert.ToInt32(Session["CustomerHeaderGid"]);
            ViewBag.records = ist.getcustomer().ToList();
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
            // List<SupplierTaxDetails> mod = new List<SupplierTaxDetails>();
            return PartialView();
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult GST_Create_Customer()
        {

            EntityGstCustomer gv = new EntityGstCustomer();
            gv.GetState = new SelectList(ist.GetState(), "customergst_stateid", "customergst_state", gv.selectedstate_gid);
            gv.customerheader_gid = Convert.ToInt32(Session["CustomerHeaderGid"]);
            ViewBag.customerheader = gv;
            //string filename = (string)Session["SupplierHeaderGid"];
            ViewBag.urlname = "";// filename;
             return PartialView(sLinkVar + "GST_Create_Customer" + sExeVar);
            //return PartialView();
        }

        [HttpPost]
        public JsonResult CreateCustomer(EntityGstCustomer pinmodel)
        {
            try
            {
                string dn = "";
                dn = ist.Insertcustomer(pinmodel);
                RedirectToAction("GSTIndex");
                 return Json(dn, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public PartialViewResult GST_Edit_Customer(int id, string viewfor)
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
            // EntityGstCustomer gv = new EntityGstCustomer();
            // gv = objModel.getVendorid(id);
            // gv.GetState = new SelectList(objModel.GetState(), "suppliergst_stateid", "suppliergst_state", gv.selectedstate_gid);
            //// return PartialView(TypeModel);
            // ViewBag.supplierheader = gv;
            // return PartialView(sLinkVar + "GST_Edit_Vendor" + sExeVar);

            EntityGstCustomer TypeModel = ist.getCustomerid(id);
            TypeModel.GetState = new SelectList(ist.GetState(), "customergst_stateid", "customergst_state", TypeModel.selectedstate_gid);
            return PartialView(TypeModel);
        }
        [HttpPost]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public JsonResult GST_Edit_Customer(EntityGstCustomer pinmodel)
        {
            string result = string.Empty;
            try
            {
                string dn = "";
                dn = ist.Updatecustomer(pinmodel);
                //string filename = (string)Session["CustomerHeaderGid"];
                //string PageMode = (string)Session["PageMode"];
                //RedirectToAction("../IEM_CUSTOMERDETAIL?pagefor='" + PageMode + "'&supid='" + filename + "'&' + new Date().getTime()");
                return Json(dn, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public JsonResult DeleteContactDetails(CustomerContactDetails objContactDetails)
        {
            try
            {
                int ContactId = (int)objContactDetails._CustContactDetailsID;
                ist.DeleteCustContactDetails(ContactId);
                TempData["ContactSearchItems"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return Json(objContactDetails, JsonRequestBehavior.AllowGet);
        }

    }
}
