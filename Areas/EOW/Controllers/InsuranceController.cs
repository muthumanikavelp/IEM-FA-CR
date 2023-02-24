using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Helper;
using Newtonsoft.Json;
using IEM.Areas.FLEXISPEND.Models;
using IEM.Areas.EOW.Models;
using System.Collections;
using IEM.Common;
using IEM.App_Start;
namespace IEM.Areas.EOW.Controllers
{

    [NoDirectAccess]
    public class InsuranceController : Controller
    {
        //
        // GET: /EOW/Insurance/
        #region declaration
        private EOW_IRepository objModel;
        EOWhelper db = new EOWhelper();
        DataTable dt = new DataTable();
        proLib plib = new proLib();
        FSRepository _fsRep = new FSRepository();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        dbLib db1 = new dbLib();
        public InsuranceController()
            : this(new EOW_DataModel())
        {

        }
        public InsuranceController(EOW_IRepository objM)
        {
            objModel = objM;
        }

        #endregion
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                Session["activeCT"] = "Basic";
                Session["activetabrsi"] = "0";
                Session["CTmakerclaim"] = "S";
                Session["QueueGidecentral"] = "";
                Session["QueueGide"] = "";
                Session["EcfGid"] = "";
                Session["invoiceGid"] = "";
                Session["currentrate"] = "";
                Session["Supplierdetails"] = "";
                EOW_Supplierinvoice eowemp = new EOW_Supplierinvoice();
                if (Session["DashBoard"] != null)
                {

                    eowemp = (EOW_Supplierinvoice)Session["DashBoard"];
                    if (eowemp.ecfremark != null)
                    {
                        ViewBag.ecfrmarks = eowemp.ecfremark;
                    }
                    Session["EcfGid"] = eowemp.ecf_GID.ToString();
                    Session["invoiceGid"] = "";
                    if (eowemp.Queue_GID != 0)
                    {
                        Session["QueueGide"] = eowemp.Queue_GID.ToString();
                    }
                    else
                    {
                        Session["QueueGide"] = "";
                    }
                    ViewBag.processdatasec = "first";
                    ViewBag.processdata = "first";
                    Session["Ecfdeclaratonnote"] = objModel.GetDecnote("11", "E").ToString();

                    Session["Mainecfamt"] = eowemp.ECF_Amount.ToString();

                    ViewBag.supcode = eowemp.Suppliercode;
                    ViewBag.supname = eowemp.Suppliername;
                    ViewBag.suppliergid = eowemp.Suppliergid;
                    Session["SupplierGidname"] = eowemp.Suppliername;
                    Session["SupplierGid"] = eowemp.Suppliergid;
                    ViewBag.exrate = eowemp.Exrate;
                    EOW_Employeelst draftsup = new EOW_Employeelst();
                    draftsup.empCode = eowemp.Suppliercode;
                    draftsup.empName = eowemp.Suppliername;
                    draftsup.employeeGid = Convert.ToInt32(eowemp.Suppliergid);
                    Session["Supplierdetails"] = draftsup;
                    ViewBag.GstCharged = "N";
                    eowemp.ECF_Amount = objCmnFunctions.GetINRAmount(eowemp.ECF_Amount);
                    ViewBag.EOW_Supplierheader = eowemp;
                    Session["DashBoard"] = null;
                    Session["SelfModeecfval"] = objCmnFunctions.GetLoginUserGid().ToString();
                }
                else
                {
                    Session["EcfGid"] = "";
                    Session["invoiceGid"] = "";
                    Session["SupplierGid"] = "";
                    Session["SupplierGidname"] = "";
                    Session["QueueGide"] = "";
                    ViewBag.processdata = "first";

                    ViewBag.CTmakerclaim = "S";
                    ViewBag.activetabrsi = "1";
                    if (ViewBag.processdatasec == "first")
                    {
                        ViewBag.processdatasec = "first";
                    }

                }
                if (objCmnFunctions.GetLoginUserGid().ToString() != "0")
                {
                    List<EOW_EmployeeeExpense> objmaindetail = new List<EOW_EmployeeeExpense>();
                    objmaindetail = objModel.SelectEmployeeeheader(objCmnFunctions.GetLoginUserGid().ToString()).ToList();
                    //EOW_Supplierinvoice eowemp = new EOW_Supplierinvoice();
                    eowemp.Grade = objmaindetail[0].Grade.ToString();
                    eowemp.Raiser_Code = objmaindetail[0].Raiser_Code.ToString();
                    eowemp.Raiser_Name = objmaindetail[0].Raiser_Name.ToString();
                    eowemp.raisermodeId = "S";
                    eowemp.Raiser_Modedata = new SelectList(objModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", eowemp.raisermodeId);
                    eowemp.Currencydata = new SelectList(objModel.GetCurrency().ToList(), "CurrencyId", "CurrencyName", "INR");
                    eowemp.CurrencyName = "INR";

                    ViewBag.EOW_Supplierheader = eowemp;
                    List<EOW_EmployeeeExpense> objmaindetaild = new List<EOW_EmployeeeExpense>();
                    objmaindetaild = objModel.SelectEmployeeeheader(objCmnFunctions.GetLoginUserGid().ToString()).ToList();
                    Session["raiserfcc"] = objmaindetaild[0].Exp_FC.ToString();
                    Session["raiserccc"] = objmaindetaild[0].Exp_CC.ToString();
                }
                return View();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
        }
        public JsonResult Getsupplierlist()
        {
            string Data1 = "";
            try
            {

                DataSet ds = db.getsupplierlist();
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetAutoCompleteSupplierCode(string txt)
        {
            try { return Json(_fsRep.GetAutoCompleteSupplierCode(txt, plib.LoginUserId), JsonRequestBehavior.AllowGet); }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        //Supplier Name Auto Complete
        public JsonResult GetAutoCompleteSupplier(string txt)
        {
            try
            {
                return Json(_fsRep.GetAutoCompleteSupplier(txt, plib.LoginUserId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult SearchSupplierDetails(string suppliercode, string suppliername)
        {
            string Data1 = "";
            try
            {

                DataSet ds = db.Fetchsupplier(suppliercode, suppliername);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetCurrency()
        {
            string Data1 = "";
            try
            {

                DataSet ds = db.Getcurrency();

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetCurrencyRate(string Currency)
        {
            string Data1 = "";
            try
            {
                DataSet ds = db.GetMaster("27", Currency, plib.LoginUserId);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Index(string txtSuppliercode, string txtSuppliername, string txtAmortDescription, string CurrencyNameval, string txtecfamontal, EOW_Supplierinvoice data, string rblAmort)
        {
            string Data1 = "";
            int ecfgid = 0;
            try
            {
                string ecfnewamt = txtecfamontal.ToString();
                ecfnewamt = objCmnFunctions.GetRemovecommas(ecfnewamt);
                DataSet ds = new DataSet();
                string eMP_Gid = objCmnFunctions.GetLoginUserGid().ToString();
                if (data.ecfremark != null)
                {
                    ViewBag.ecfrmarks = data.ecfremark;
                }
                if (eMP_Gid != "" && eMP_Gid != null)
                {
                    data.raisermodeId = "S";
                    data.ecfraisergid = eMP_Gid;
                    data.amort = rblAmort.ToString();
                    data.ECF_Amount = string.IsNullOrEmpty(txtecfamontal.ToString()) ? "0" : txtecfamontal.ToString();
                    data.amortdec = string.IsNullOrEmpty(txtAmortDescription) ? "" : txtAmortDescription.ToString();
                    data.Suppliercode = txtSuppliercode;
                    data.Suppliername = txtSuppliername;
                    //if (Session["EcfGid"] != null || Session["EcfGid"]=="")
                    //{
                    data.ecf_GID = String.IsNullOrEmpty(Session["EcfGid"].ToString()) ? 0 : Convert.ToInt32(Session["EcfGid"]);
                    //}
                    ds = db.Setecfheaderdetails(data);
                    if (ds != null)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            Data1 = dt.Rows[0]["msg"].ToString();
                            if (Data1 == "1")
                            {
                                dt = ds.Tables[1];
                                if (dt.Rows.Count > 0)
                                {
                                    ecfgid = Convert.ToInt32(dt.Rows[0]["ecf_gid"]);
                                    Session["EcfGid"] = ecfgid;
                                    Session["SupplierGid"] = Convert.ToInt32(dt.Rows[0]["ecf_supplier_gid"]);
                                }

                                ViewBag.processdata = "first";
                                ViewBag.processdatasec = "first";

                                List<EOW_EmployeeeExpense> objmaindetail = new List<EOW_EmployeeeExpense>();
                                objmaindetail = objModel.SelectEmployeeeheader(objCmnFunctions.GetLoginUserGid().ToString()).ToList();
                                EOW_Supplierinvoice eowemp = new EOW_Supplierinvoice();
                                eowemp.Grade = objmaindetail[0].Grade.ToString();
                                eowemp.Raiser_Code = objmaindetail[0].Raiser_Code.ToString();
                                eowemp.Raiser_Name = objmaindetail[0].Raiser_Name.ToString();
                                eowemp.raisermodeId = "S";
                                eowemp.Raiser_Modedata = new SelectList(objModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", eowemp.raisermodeId);
                                eowemp.Currencydata = new SelectList(objModel.GetCurrency().ToList(), "CurrencyId", "CurrencyName", "INR");
                                eowemp.CurrencyName = "INR";
                                eowemp.ECF_Amount = objCmnFunctions.GetINRAmount(txtecfamontal);
                                eowemp.ecf_GID = ecfgid;
                                ViewBag.ecfgid = ecfgid;
                                ViewBag.activetabrsi = "1";
                                ViewBag.EOW_Supplierheader = eowemp;
                                ViewBag.CTmakerclaim = "S";
                                ViewBag.supname = txtSuppliername;
                                ViewBag.supcode = txtSuppliercode;
                               // ViewBag.suppliergid = data.Suppliergid;
                                ViewBag.EOW_Supplierheader = eowemp;
                                Session["SupplierGidname"] = txtSuppliername;
                                Session["Ecfdeclaratonnote"] = objModel.GetDecnote("11", "E").ToString();
                                Session["Mainecfamt"] = ecfnewamt.ToString();
                                //Session["SupplierGid"] = data.Suppliergid;
                            }
                        }
                    }

                }

                return View();
            }
            catch (Exception ex)
            {  
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult Newinvoiceadd(EOW_SupplierModelgrid NewAttmodel)
        {
            string Data1 = "";
            string invoicegid = "";
            string result = "start";
            DataSet ds = new DataSet();
            try
            {
                string ecfid = string.IsNullOrEmpty(ViewBag.ecfgid) ? "" : ViewBag.ecfgid;
                if(ecfid=="")
                {
                    ecfid = Session["EcfGid"].ToString();
                }
                if (Session["invoiceGid"] != null && Session["invoiceGid"].ToString() != "")
                {
                    NewAttmodel.Invoicegid = Convert.ToInt32(Session["invoiceGid"]);
                }
                NewAttmodel.ecfgid = ecfid;
                ds = db.UpdateSupplierinvoice(NewAttmodel);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        Data1 = dt.Rows[0]["Result"].ToString();
                        if (Data1 == "1")
                        {
                            dt = ds.Tables[1];
                            if (dt.Rows.Count > 0)
                            {
                                invoicegid = dt.Rows[0]["invoice_gid"].ToString();
                                Session["invoiceGid"] = invoicegid;
                                Session["EcfGid"] = NewAttmodel.ecfgid;
                                ViewBag.invId = invoicegid;
                            }
                            string invoicdate = NewAttmodel.InvoiceDate;
                            int nodays = 0;
                            string datas = objModel.GetStatusexcel(objCmnFunctions.convertoDateTimeString(invoicdate), "", "", "Invoicedate");
                            nodays = Convert.ToInt32(datas);
                            if (nodays > 90)
                            {
                                if (Convert.ToDecimal(NewAttmodel.Amount) > 200000)
                                {
                                    result = "FinanceApprove";
                                }
                            }
                            else
                            {
                                result = "success";
                            }


                        }
                        else
                        {
                            result = Data1;
                        }
                    }
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public PartialViewResult _SupplierTaxDetailsGrid()
        {
            List<sinvotax> lstnew = new List<sinvotax>();
            return PartialView(lstnew);
        }
        [HttpGet]
        public PartialViewResult _TabdebitlineDetails()
        {
            List<EOW_TravelClaim> lstnew = new List<EOW_TravelClaim>();
            return PartialView(lstnew);
        }
        [HttpGet]
        public PartialViewResult _SupplierPaymentGrid()
        {
            List<EOW_Payment> lstPayment = new List<EOW_Payment>();
            return PartialView(lstPayment);
        }
        [HttpGet]
        public PartialViewResult _InvoiceAttachmentDetails()
        {
            List<EOW_File> lstAttachment = new List<EOW_File>();
            return PartialView(lstAttachment);
        }
        [HttpGet]
        public PartialViewResult _GetGstDetails()
        {
            return PartialView();
        }
        [HttpGet]
        public PartialViewResult _LoadRCMDetails()
        {
            //Session["addcurpogis"] = null;
            // List<EOW_PO> lstnew = new List<EOW_PO>();
            // lstnew = objModel.Getpodetailsgrid(values, valuesid, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), "S").ToList();
            return PartialView();
        }
        [HttpGet]
        public PartialViewResult _TabdebitlineCreate()
        {
            EOW_TravelClaim objMExpense = new EOW_TravelClaim();
            try
            {

                List<EOW_TravelClaim> objobjMExpense = new List<EOW_TravelClaim>();
                objobjMExpense = objModel.tSelectEmployeeeBasic(objCmnFunctions.GetLoginUserGid().ToString()).ToList();
                objMExpense.OUCode = objobjMExpense[0].OUCode.ToString();
                objMExpense.ProductCode = objobjMExpense[0].ProductCode.ToString();
                objMExpense.FC = objobjMExpense[0].FC.ToString();
                objMExpense.CC = objobjMExpense[0].CC.ToString();

                objMExpense.Exp_FCC = new SelectList(objModel.fcdata().ToList(), "raiserfcId", "raiserfcName", Session["raiserfcc"].ToString());
                objMExpense.Exp_CCC = new SelectList(objModel.ccdata().ToList(), "raiserccId", "raiserccName", Session["raiserccc"].ToString());
                objMExpense.Exp_FCCC = objobjMExpense[0].Exp_FCCC.ToString();

                objMExpense.ExpNatureofExpdata = new SelectList(objModel.NatureofExpensesdataothersupplier().ToList(), "NatureofExpensesId", "NatureofExpensesName");
                return PartialView(objMExpense);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView(objMExpense);
            }
        }
        [HttpPost]
        public JsonResult _TabdebitlineCreate(EOW_TravelClaim TravelClaimModel)
        {
            string message = "";
            try
            {
                message = objModel.InsertInsurancesupplierCreate(TravelClaimModel, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), objCmnFunctions.GetLoginUserGid().ToString());
                ViewBag.SearchItems = null;
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Request posted to catch" + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult _TabdebitlineEdit(EOW_TravelClaim TravelClaimModel)
        {
            string message = "";
            try
            {
                message = objModel.UpdateInsranceSupplierDebit(TravelClaimModel, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), objCmnFunctions.GetLoginUserGid().ToString(), Session["supplierdebitactiverow"].ToString());
                ViewBag.SearchItems = null;
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Request posted to catch" + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public PartialViewResult _TabdebitlineEdit(int id, string viewfor)
        {
            try
            {
                EOW_TravelClaim objMExpenseEdit = new EOW_TravelClaim();
                List<EOW_TravelClaim> objobjMExpense = new List<EOW_TravelClaim>();
                if (viewfor == "edit")
                {
                    ViewBag.viewfor = "edit";
                }
                else if (viewfor == "view")
                {
                    ViewBag.viewfor = "view";
                }
                Session["supplierdebitactiverow"] = id.ToString();
                ViewBag.SearchItems = null;
                objobjMExpense = objModel.GetSupplierINSDebitsingle(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), "E", id).ToList();
                objMExpenseEdit.OUCode = objobjMExpense[0].OUCode.ToString();
                objMExpenseEdit.ProductCode = objobjMExpense[0].ProductCode.ToString();

                objMExpenseEdit.FC = objobjMExpense[0].FC.ToString();
                objMExpenseEdit.CC = objobjMExpense[0].CC.ToString();
                objMExpenseEdit.travelDescription = objobjMExpense[0].travelDescription.ToString();
                //objMExpenseEdit.TravelHsnDesc = objobjMExpense[0].TravelHsnDesc.ToString();
                objMExpenseEdit.AssetCatList = new SelectList(objModel.GetAssetCategoryList().ToList(), "AssetCatId", "AssetCatName", objobjMExpense[0].AssetCatId.ToString());
                objMExpenseEdit.AssetSubCatList = new SelectList(objModel.GetAssetSubCategoryList(objobjMExpense[0].AssetCatId).ToList(), "AssetSubCatId", "AssetSubCatName", objobjMExpense[0].AssetSubCatId.ToString());
                objMExpenseEdit.HsncodeList = new SelectList(objModel.GetHsnList().ToList(), "TravelHsnid", "TravelHsnCode", objobjMExpense[0].TravelHsnid.ToString());
                objMExpenseEdit.ExpNatureofExpdata = new SelectList(objModel.NatureofExpensesdataothersupplier().ToList(), "NatureofExpensesId", "NatureofExpensesName", objobjMExpense[0].NatureofExpensesId.ToString());
                objMExpenseEdit.ExpCatdata = new SelectList(objModel.ExpenseCategorydata(objobjMExpense[0].NatureofExpensesId).ToList(), "ExpenseCategoryId", "ExpenseCategoryName", objobjMExpense[0].ExpenseCategoryId.ToString());
                objMExpenseEdit.ExpSubCatdata = new SelectList(objModel.SubCategorydata(objobjMExpense[0].ExpenseCategoryId).ToList(), "SubCategoryId", "SubCategoryName", objobjMExpense[0].SubCategoryId.ToString());
                objMExpenseEdit.Exp_FCC = new SelectList(objModel.fcdata().ToList(), "raiserfcId", "raiserfcName", objobjMExpense[0].FC.ToString());
                objMExpenseEdit.Exp_CCC = new SelectList(objModel.ccdata().ToList(), "raiserccId", "raiserccName", objobjMExpense[0].CC.ToString());
                objMExpenseEdit.Amount = objCmnFunctions.GetINRAmount(objobjMExpense[0].Amount.ToString());
                objMExpenseEdit.ProdServCategory = objobjMExpense[0].ProdServCategory.ToString();
                objMExpenseEdit.RCMFlag = objobjMExpense[0].RCMFlag.ToString();
                objMExpenseEdit.TravelHsnid = objobjMExpense[0].TravelHsnid;
                objMExpenseEdit.TravelHsnCode = objobjMExpense[0].TravelHsnCode;
                objMExpenseEdit.TravelHsnDesc = objobjMExpense[0].TravelHsnDesc.ToString();
                ViewBag.TravelClaimheader = objMExpenseEdit;
                return PartialView();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView();
            }
        }




        [HttpPost]
        public JsonResult _SupplierECFCreate(EOW_Supplierinvoice Suppliermodel)
        {
            string Err = "";
            try
            {
                decimal gvamt = Convert.ToDecimal(Session["Supplierecfamountval"].ToString());
                decimal mainecf = Convert.ToDecimal(Session["Mainecfamt"].ToString());

                decimal ecfdebitlinesum = Convert.ToDecimal(objModel.Getbebitlineamt(Session["EcfGid"].ToString(), Session["EcfGid"].ToString()));
                decimal ecftaxsum = Convert.ToDecimal(objModel.Getinvtaxamt(Session["EcfGid"].ToString(), Session["EcfGid"].ToString()));
                decimal newecfdebitlinesum = ecfdebitlinesum + ecftaxsum;
                var result = objModel.GetSupplierBankDetailsBypayMode(Session["EcfGid"].ToString());
                if (gvamt != mainecf)
                {
                    Err = "ECF";
                }
                if (result == "No")
                {
                    Err = "BankAcc";
                }

                if (Err == "")
                {
                    if (ecfdebitlinesum != mainecf)
                    {
                        Err = "DebitLine";
                    }
                }

                if (Err == "")
                {
                    Err = objModel.UpdateInsuranceinvoicefinal(Suppliermodel, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), Session["SupplierGid"].ToString(), objCmnFunctions.GetLoginUserGid().ToString(), "S", Session["QueueGide"].ToString());
                    if (Err == "")
                    {
                        Err = "Please submit again";
                    }
                    else
                    {
                        Err = Err.ToString();
                        Session["EcfGid"] = "";
                        Session["QueueGide"] = null;
                        Session["EcfGid"] = null;
                        Session["invoiceGid"] = null;
                        Session["Mainecfamt"] = null;
                        Session["Supplierdetails"] = null;
                    }
                }
                return Json(Err, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(Err, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult _suppliserinvoiceCreateb(EOW_SupplierModelgrid Suppliermodel)
        {
            DataSet ds = new DataSet();
            string Err = "", Data1 = "";
            try
            {
                decimal debit = Convert.ToDecimal(Session["invoiceDebitamnt"].ToString());
                decimal pay = Convert.ToDecimal(Session["invoicePaymentamnt"].ToString());
                decimal tax = Convert.ToDecimal(Session["invoiceTaxamnt"].ToString());
                decimal sumdebitax = debit + tax;
                decimal invoiceamt = Convert.ToDecimal(Suppliermodel.Amount);
                if (Session["invoiceGid"] != null && Session["invoiceGid"].ToString() != "")
                {
                    Suppliermodel.Invoicegid = Convert.ToInt32(Session["invoiceGid"]);
                }
                if (debit != invoiceamt)
                {
                    Err = "Debit";
                }
                else if (pay != invoiceamt)
                {
                    Err = "Payment";
                }
                else if (Suppliermodel.potype == "PO" || Suppliermodel.potype == "WO")
                {
                    decimal poamt = Convert.ToDecimal(Suppliermodel.TaxableAmount.ToString());
                    decimal toltacpay = poamt;
                    if (toltacpay > invoiceamt)
                    {
                        Err = "POAmt";
                    }
                }

                if (Err != "Debit" && Err != "POAmt" && Err != "Payment")
                {
                    Err = Session["invoiceGid"].ToString();
                    ds = db.UpdateSupplierinvoice(Suppliermodel);

                    if (ds != null)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0)
                        {
                            Data1 = dt.Rows[0]["Result"].ToString();
                            if (Data1 == "1")
                            {
                                dt = ds.Tables[1];
                                Err = "Success";
                            }
                            else
                            {
                                Err = Data1;
                            }
                        }
                    }
                    Session["invoiceGid"] = "";
                    Session["invoiceDebitamnt"] = "0";
                    Session["invoicePaymentamnt"] = "0";
                    Session["invoiceTaxamnt"] = "0";
                }
                return Json(Err, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(Err, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetSubCategory(EOW_EmployeeeExpense EmployeeeExpense)
        {
            try
            {

                string Data1 = "", Data2 = "";
                string ExpenseId = Convert.ToString(EmployeeeExpense.ExpenseCategoryId);
                DataSet ds = db1.GetExpenseCategory(ExpenseId);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                //return Json(objModel.SubCategorydata(EmployeeeExpense.ExpenseCategoryId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetSubCategorypolicy(EOW_EmployeeeExpense EmployeeeExpense)
        {
            try
            {
                string policy = "", Data2 = "";
                policy = objModel.selectpolicy(EmployeeeExpense.ExpenseCategoryId.ToString());
                Session["policyhelp"] = policy;
                DataSet ds = db1.GetExpenseHsn(EmployeeeExpense.ExpenseCategoryId.ToString());
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                return Json(new { policy, Data2 }, JsonRequestBehavior.AllowGet);
                //return Json(policy, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Gethsndesc(string hsngid)
        {
            try
            {
                string Data2 = "";
                Data2 = objModel.GetExpenseHsndesc(hsngid);

                return Json(Data2, JsonRequestBehavior.AllowGet);
                //return Json(policy, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult ViewSupplierinvoice(EOW_SupplierModelgrid Suppliermodel)
        {
            int TravelModeGID = (int)Suppliermodel.Invoicegid;
            Session["invoiceGid"] = TravelModeGID.ToString();

            string vieworedit = Suppliermodel.Provision;

            if (vieworedit == "View")
            {
                Session["Supplierpaydebitedit"] = "View";
            }
            if (vieworedit == "Edit")
            {
                Session["Supplierpaydebitedit"] = "Edit";
            }

            EOW_SupplierModelgrid objMPaymentEdit = new EOW_SupplierModelgrid();
            List<EOW_SupplierModelgrid> objobjMPayment = new List<EOW_SupplierModelgrid>();
            objobjMPayment = objModel.GetINSSuppliersingledata(Session["EcfGid"].ToString(), "S", TravelModeGID).ToList();
            objMPaymentEdit.Invoicegid = Convert.ToInt32(objobjMPayment[0].Invoicegid.ToString());
            objMPaymentEdit.InvoiceDate = objobjMPayment[0].InvoiceDate.ToString();
            objMPaymentEdit.InvoiceNo = objobjMPayment[0].InvoiceNo.ToString();
            objMPaymentEdit.Description = objobjMPayment[0].Description.ToString();
            objMPaymentEdit.Amount = objobjMPayment[0].Amount.ToString();
            objMPaymentEdit.SerMonth = objobjMPayment[0].SerMonth.ToString();
            objMPaymentEdit.Retensionflg = objobjMPayment[0].Retensionflg.ToString();
            objMPaymentEdit.Retensionper = objobjMPayment[0].Retensionper.ToString();
            objMPaymentEdit.Retensionamount = objobjMPayment[0].Retensionamount.ToString();
            objMPaymentEdit.Retensionrelse = objobjMPayment[0].Retensionrelse.ToString();
            objMPaymentEdit.Provision = objobjMPayment[0].Provision.ToString();
            Session["editorview"] = "Edit";
            return Json(objobjMPayment, JsonRequestBehavior.AllowGet);
        }


    }
}
