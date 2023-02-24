using IEM.Areas.FLEXISPEND.Models;
using IEM.Common;
using IEM.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.App_Start;
namespace IEM.Areas.FLEXISPEND.Controllers
{

    [NoDirectAccess]
    public class HelperController : Controller
    {
        #region Declaration
        proLib plib = new proLib();
        dbLib db = new dbLib();
        FSRepository _fsRep = new FSRepository();
        ErrorLog objErrorLog = new ErrorLog();
        #endregion

        // GET: /FLEXISPEND/Helper/
        public ActionResult Index()
        {
            return View();
        }

        //Courier Auto Complete
        public JsonResult GetAutoCompleteCourier(string txt)
        {
            try { return Json(_fsRep.GetAutoCompleteCourier(txt, plib.LoginUserId), JsonRequestBehavior.AllowGet); }
            catch { return null; }
        }

        //Employee Code Auto Complete
        public JsonResult GetAutoCompleteEmployeeCode(string txt)
        {
            try
            {
                return Json(_fsRep.GetAutoCompleteEmployeeCode(txt, plib.LoginUserId), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Employee Name Auto Complete
        public JsonResult GetAutoCompleteEmployee(string txt)
        {
            try
            {
                return Json(_fsRep.GetAutoCompleteEmployee(txt, plib.LoginUserId), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Supplier COde Auto Complete
        public JsonResult GetAutoCompleteSupplierCode(string txt)
        {
            try
            {
                return Json(_fsRep.GetAutoCompleteSupplierCode(txt, plib.LoginUserId), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Supplier Name Auto Complete
        public JsonResult GetAutoCompleteSupplier(string txt)
        {
            try
            {
                return Json(_fsRep.GetAutoCompleteSupplier(txt, plib.LoginUserId), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Supplier Code & Name Combained Auto Complete
        public JsonResult GetAutoCompleteSupplierAll(string txt)
        {
            try
            {
                return Json(_fsRep.GetAutoCompleteSupplierAll(txt, plib.LoginUserId), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Branch Auto Complete
        public JsonResult GetAutoCompleteBranch(string txt)
        {
            try
            {
                return Json(_fsRep.GetAutoCompleteBranch(txt, plib.LoginUserId), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Branch Auto Complete
        [HttpPost]
        public JsonResult GetAutoCompleteBranchPouch(string txt)
        {
            try
            {
                return Json(_fsRep.GetAutoCompleteBranchPouch(txt, plib.LoginUserId), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

         
        public JsonResult GetBranchAddressDetails(string LocationId)
        {
            string Address = "";
            DataTable dt = null;
            try
            {
                DataSet ds = db.GetAutoComplete(LocationId, "11", plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Address = dt.Rows[0]["Text"].ToString(); }

                    return Json(new { Address }, JsonRequestBehavior.AllowGet);
                }
                else { return Json(new { Address }, JsonRequestBehavior.AllowGet); }
            }
            catch
            {
                return Json(new { Address }, JsonRequestBehavior.AllowGet);
            }
        }

        //ECF Module Doc Type.
        public JsonResult MasterDocumentType()
        {
            try
            {
                return Json(_fsRep.MasterDocumentType(plib.LoginUserId), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Document Type for DropDown--With Select All Option
        public JsonResult LoadDocumentType()
        {
            try
            {
                return Json(_fsRep.LoadDocumentType(plib.LoginUserId), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Document Type for DropDown--With Select All Option
        public JsonResult GetDocType()
        {
            try
            {
                string Data1 = "";
                DataTable dt = _fsRep.GetDocType("0");
                if (dt != null && dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Document Type for DropDown--Without Select All Option
        public JsonResult GetPRDocType()
        {
            try
            {
                string Data1 = "";
                DataTable dt = _fsRep.GetDocType("1");
                if (dt != null && dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //ECF Module Doc Type.
        public JsonResult GetDocumentNumber(string DocNo)
        {
            try
            {
                try
                {
                    DataTable dt = null;
                    string Data1 = "";
                    DataSet ds = db.GetDocument(DocNo);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                }
                catch
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        //ECF Module Doc Type.
        public JsonResult GetDocumentECFDetails(string DocNo)
        {
            string ecfId = "0", docSubTypeId = "0";
            try
            {
                DataTable dt = null;

                DataSet ds = db.GetDocument(DocNo);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0)
                {
                    ecfId = dt.Rows[0]["ecfId"].ToString().Trim() == "" ? "0" : dt.Rows[0]["ecfId"].ToString();
                    docSubTypeId = dt.Rows[0]["DocSubTypeId"].ToString().Trim() == "" ? "0" : dt.Rows[0]["DocSubTypeId"].ToString();
                }
                else
                {
                    ecfId = docSubTypeId = "0";
                }

                return Json(new { ecfId, docSubTypeId }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { ecfId, docSubTypeId }, JsonRequestBehavior.AllowGet);
            }
        }

        //Branch Auto Complete
        public JsonResult LoadBankDropDown()
        {
            try
            {
                return Json(_fsRep.LoadBankMaster(plib.LoginUserId), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Paymode Auto Complete
        public JsonResult LoadPayModeDropDown()
        {
            try
            {
                return Json(_fsRep.LoadPayModeMaster(plib.LoginUserId), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
    


        //DocType For Check List Master.
        public JsonResult GetDocumentTypeCheckList()
        {
            try
            {
                string Data0 = "";
                DataSet ds = db.GetMaster("10", plib.LoginUserId);
                if (ds != null)
                {
                    List<ParentDetails> result = _fsRep.GetDocumentTypeCheckList(ds.Tables[0], ds.Tables[1]);
                    if (result != null) { Data0 = JsonConvert.SerializeObject(result); }

                    return Json(new { Data0 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        //ClaimType--With Select All Option
        public JsonResult LoadClaimTypeWA()
        {
            try
            {
                return Json(_fsRep.LoadClaimType("0"), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //ClaimType--WithOut Select All Option
        public JsonResult LoadClaimType()
        {
            try
            {
                return Json(_fsRep.LoadClaimType("1"), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Payment Mode DropDown
        public JsonResult LoadPayModeType()
        {
            try
            {
                return Json(_fsRep.MasterPayMode(plib.LoginUserId), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Currency Type
        [HttpPost]
        public JsonResult GetCurrencyType()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("3", "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Invoice Type
        [HttpPost]
        public JsonResult GetInvoiceType()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("2", "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Get Tax Type
        [HttpPost]
        public JsonResult GetTaxType()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("14", "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Get Tax Type
        [HttpPost]
        public JsonResult GetGSTTaxType()
        {
            try
            {
                string Data1 = "";
                //DataSet ds = db.GetMaster("14", "1");
                //if (ds != null && ds.Tables.Count > 0)
                //{
                //DataTable dt = ds.Tables[0];
                DataTable dt = new DataTable();
                dt.Clear();
                dt.Columns.Add("Id");
                dt.Columns.Add("Combo");
                DataRow row = dt.NewRow();
                //dt.Rows.InsertAt(row, 0);
                row["Id"] = "0";
                row["Combo"] = "-- Select One --"; 
                dt.Rows.Add(row);
                row = dt.NewRow();
                row["Id"] = "128";
                row["Combo"] = "GST";  
                dt.Rows.Add(row);
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                //}
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        //Get Business Segment(BU)
        [HttpPost]
        public JsonResult GetBusinessSegment()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("15", "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Get Cost Center(CC)
        [HttpPost]
        public JsonResult GetCostCenter()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("16", "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Get Pay Mode
        [HttpPost]
        public JsonResult GetPayMode()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("17", "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Get ARF Supplier Pay Mode
        [HttpPost]
        public JsonResult GetEditLCPayMode()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("54", "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Get ARF Supplier Pay Mode
        [HttpPost]
        public JsonResult GetARFSupplierPayMode()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("45", "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        [HttpPost]
        public JsonResult GetLCSupplierPayMode()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("54", "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Get Employee Supplier Pay Mode
        [HttpPost]
        public JsonResult GetARFEmployeePayMode()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("45", "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[1];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Nature of Expenses Type
        [HttpPost]
        public JsonResult GetNatureOfExpenses()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("5", "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Get Expense Category
        [HttpPost]
        public JsonResult GetExpenseCategory(string CatId)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("6", CatId, "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Get Expense Sub Category
        [HttpPost]
        public JsonResult GetExpenseSubCategory(string CatId)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("13", CatId, "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }


        #region
        [HttpPost]
        public JsonResult GetHsnArray(string CatId, string POType = "")
        {
            try
            {
                string Data1 = "";
                if (POType == "PO")
                {
                    POType = "P";
                }
                else
                {
                    POType = "W";
                }
                DataSet ds = db.GetHSNMaster("53", CatId, "1", POType);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return null;
            }
        }
        #endregion
      


        //Product Code Auto Complete
        public JsonResult GetAutoCompleteProductCode(string txt)
        {
            try { return Json(_fsRep.GetAutoCompleteProductCode(txt, plib.LoginUserId), JsonRequestBehavior.AllowGet); }
            catch { return null; }
        }

        //Location Code Auto Complete
        public JsonResult GetAutoCompleteLocationCode(string txt, string Code)
        {
            try { return Json(_fsRep.GetAutoCompleteLocationCode(txt, Code, plib.LoginUserId), JsonRequestBehavior.AllowGet); }
            catch { return null; }
        }

        //Get Attachment Type
        [HttpPost]
        public JsonResult GetAttachmentType()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("18", "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Get Advance Type
        [HttpPost]
        public JsonResult GetAdvanceType()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("19", "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Get Place
        [HttpPost]
        public JsonResult GetPlace()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("20", "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Get Travel Mode
        [HttpPost]
        public JsonResult GetTravelMode()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("21", "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Get Travel Class
        [HttpPost]
        public JsonResult GetTravelClass(string CatId)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("22", CatId, "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Nature of Expenses Type
        [HttpPost]
        public JsonResult GetNatureOfExpensesForTravel()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("5", "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[1];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Advance Type DropDown
        public JsonResult LoadAdvanceType()
        {
            try
            {
                return Json(_fsRep.MasterAdvanceMode(plib.LoginUserId), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Load Amort Duration
        public JsonResult LoadAmortCycle()
        {
            try
            {
                return Json(_fsRep.LoadAmortCycle(plib.LoginUserId), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //With Holding Tax Type
        [HttpPost]
        public JsonResult GetWHTaxType()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("4", "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[1];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //With Holding Tax Sub Type
        [HttpPost]
        public JsonResult GetWHTaxSubType(string CatId)
        {
            try
            {
                string Data1 = "";
                //DataSet ds = db.GetMaster("24", CatId, "1");
                DataSet ds = db.GetMaster1("24", "1",CatId);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }                    
                    
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Get Paymode Employee Claim
        [HttpPost]
        public JsonResult GetPayModeEmployee()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("17", "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[1];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Amort Gl
        [HttpPost]
        public JsonResult GetAmortGl()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("25", "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Get Tax Rate
        [HttpPost]
        public JsonResult GetTaxRate()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster1("26", "1", "0");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "0";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //[GL UPLOADS]GL Upload Types DropDown
        public JsonResult LoadGLUploadTypes()
        {
            try
            {
                return Json(_fsRep.LoadGLUploadTypes(plib.LoginUserId), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Receipt Source DropDown
        public JsonResult LoadReceiptSource()
        {
            try
            {
                return Json(_fsRep.LoadReceiptSource(plib.LoginUserId), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Receipt Mode DropDown
        public JsonResult LoadReceiptMode()
        {
            try
            {
                return Json(_fsRep.LoadReceiptMode(plib.LoginUserId), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Receipt GL AC Auto Complete
        public JsonResult GetAutoCompleteReceiptGLAC(string txt)
        {
            try { return Json(_fsRep.LoadGLACNumber(txt, plib.LoginUserId), JsonRequestBehavior.AllowGet); }
            catch { return null; }
        }

        //Receipt Supplier/Employee Complete
        public JsonResult GetAutoReceiptEmployee(string txt)
        {
            try { return Json(_fsRep.GetAutoReceiptEmployee(txt, plib.LoginUserId), JsonRequestBehavior.AllowGet); }
            catch { return null; }
        }

        //Receipt Supplier/Employee Complete
        public JsonResult GetAutoReceiptSupplier(string txt)
        {
            try { return Json(_fsRep.GetAutoReceiptSupplier(txt, plib.LoginUserId), JsonRequestBehavior.AllowGet); }
            catch { return null; }
        }

        //Activity Auto Complete
        public JsonResult LoadActivityDropDown()
        {
            try
            {
                return Json(_fsRep.LoadActivityTypes(plib.LoginUserId), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Provision FC Business Auto Complete
        public JsonResult GetAutoCompleteFCBusiness(string txt)
        {
            try { return Json(_fsRep.GetAutoCompleteFCBusiness(txt, plib.LoginUserId), JsonRequestBehavior.AllowGet); }
            catch { return null; }
        }

        //Provision CC Unit Auto Complete
        public JsonResult GetAutoCompleteCCUnit(string txt)
        {
            try { return Json(_fsRep.GetAutoCompleteCCUnit(txt, plib.LoginUserId), JsonRequestBehavior.AllowGet); }
            catch { return null; }
        }

        //Get Pay Bank Details
        [HttpPost]
        public JsonResult GetPayBank()
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetMaster("40", "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    if (ds.Tables.Count > 1)
                    {
                        dt = ds.Tables[1];
                        if (dt.Rows.Count > 0) { Data2 = dt.Rows[0][0].ToString(); }
                    }
                }
                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Get Paymode Employee Claim
        [HttpPost]
        public JsonResult GetPayModeCommon()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("41", "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetPayModeCommon1()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("51", "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }


        //EFT Paymode.
        public JsonResult LoadEFTPayMode()
        {
            try
            {
                return Json(_fsRep.LoadEFTPayMode(plib.LoginUserId), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Get Asset Category
        [HttpPost]
        public JsonResult GetAssetCategory()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("43", "0", "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Get Asset Sub Category
        [HttpPost]
        public JsonResult GetAssetSubCategory(string CatId)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("44", CatId, "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }


        public JsonResult LoadGSTINHSNCode()
        {
            try
            {
                return Json(_fsRep.LoadHSNCode(plib.LoginUserId), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetGSTINArray(string ViewType, string RefId)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetGSTMaster(ViewType, RefId, "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (ViewType != "4")
                    {
                        dt.Rows[0][2] = "";
                    }
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetGSTNVendorDetails(string ViewType, string GSTIN, string SubrefId)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetGSTNVendorDetails(ViewType, GSTIN, SubrefId, "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        //Employee Code Auto Complete
        public JsonResult GetAutoCompleteGLCode(string txt)
        {
            try
            {
                return Json(_fsRep.GetAutoCompleteGLCode(txt, plib.LoginUserId), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
    }
}
