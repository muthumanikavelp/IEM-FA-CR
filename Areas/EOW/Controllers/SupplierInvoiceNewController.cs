
using IEM.Areas.EOW.Models;
using IEM.Common;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using IEM.Areas.FLEXIBUY.Models;
//using Excel = Microsoft.Office.Interop.Excel;
//using System.Data.OleDb;
using Excel;
using ClosedXML.Excel;
using IEM.Areas.MASTERS.Models;
using IEM.Helper;
using Newtonsoft.Json;
using IEM.App_Start;
using IEM.Models;
namespace IEM.Areas.EOW.Controllers
{

    [NoDirectAccess]
    public class SupplierInvoiceNewController : Controller
    {
        //
        // GET: /EOW/SupplierInvoiceNew/
        private EOW_IRepository objModel; 
        ErrorLog objErrorLog = new ErrorLog();
        private FileServer Cmnfs = new FileServer();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        CommonIUD objCommonIUD = new CommonIUD();
        dbLib db = new dbLib();
        proLib plib = new proLib();
        DataTable dt = new DataTable();
        IEM_MST_EXPENSECATEGORY expcat = new IEM_MST_EXPENSECATEGORY();
        CygnetDataModel ObjCygnet = new CygnetDataModel();
        private CentralRepository objCModel;
        public SupplierInvoiceNewController()
            : this(new EOW_DataModel(), new CentralModel())
        {

        }
        public SupplierInvoiceNewController(EOW_IRepository objM, CentralRepository objCM)
        {
            objModel = objM;
            objCModel = objCM;
        }
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
                Session["CygnetFlag"] = "Y";
                if (Session["DashBoard"] != null)
                {
                    EOW_Supplierinvoice eowemp = new EOW_Supplierinvoice();
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
                    Session["Ecfdeclaratonnote"] = objModel.GetDecnote("1", "E").ToString();

                    Session["Mainecfamt"] = eowemp.ECF_Amount.ToString();

                    ViewBag.supcode = eowemp.Suppliercode;
                    ViewBag.supname = eowemp.Suppliername;
                    Session["SupplierGidname"] = eowemp.Suppliername;
                    Session["SupplierGid"] = eowemp.Suppliergid;
                    ViewBag.exrate = eowemp.Exrate;
                    ViewBag.ecfPayMode = eowemp.ecf_Paymode;
                    EOW_Employeelst draftsup = new EOW_Employeelst();
                    draftsup.empCode = eowemp.Suppliercode;
                    draftsup.empName = eowemp.Suppliername;
                    draftsup.employeeGid = Convert.ToInt32(eowemp.Suppliergid);
                    Session["Supplierdetails"] = draftsup;
                    if (eowemp.DocId == "1")
                    {
                        ViewBag.POStatus = "PO";
                    }
                    if (eowemp.DocId == "2")
                    {
                        ViewBag.POStatus = "WO";
                    }
                    if (eowemp.DocId == "3")
                    {
                        ViewBag.POStatus = "Non PO/WO";
                    }
                    if (eowemp.DocId == "4")
                    {
                        ViewBag.POStatus = "Utility";
                    }
                    ViewBag.GstCharged = "N";
                    eowemp.ECF_Amount = objCmnFunctions.GetINRAmount(eowemp.ECF_Amount);
                    ViewBag.EOW_Supplierheader = eowemp;
                    Session["DashBoard"] = null;
                    Session["SelfModeecfval"] = objCmnFunctions.GetLoginUserGid().ToString();
                }
                else
                {
                    Session["SelfModeecfval"] = objCmnFunctions.GetLoginUserGid().ToString();
                    List<EOW_EmployeeeExpense> objmaindetail = new List<EOW_EmployeeeExpense>();
                    objmaindetail = objModel.SelectEmployeeeheader(objCmnFunctions.GetLoginUserGid().ToString()).ToList();

                    EOW_Supplierinvoice eowemp = new EOW_Supplierinvoice();
                    eowemp.Grade = objmaindetail[0].Grade.ToString();
                    eowemp.Raiser_Code = objmaindetail[0].Raiser_Code.ToString();
                    eowemp.Raiser_Name = objmaindetail[0].Raiser_Name.ToString();
                    eowemp.raisermodeId = "S";
                    eowemp.Raiser_Modedata = new SelectList(objModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", eowemp.raisermodeId);
                    eowemp.doctypedata = new SelectList(objModel.GetDoctype().ToList(), "DocId", "DocName", eowemp.DocId);
                    eowemp.Currencydata = new SelectList(objModel.GetCurrency().ToList(), "CurrencyId", "CurrencyName", "INR");
                    eowemp.CurrencyName = "INR";
                    ViewBag.EOW_Supplierheader = eowemp;
                    ViewBag.processdata = "first";
                }
                List<EOW_EmployeeeExpense> objmaindetaild = new List<EOW_EmployeeeExpense>();
                objmaindetaild = objModel.SelectEmployeeeheader(objCmnFunctions.GetLoginUserGid().ToString()).ToList();
                Session["raiserfcc"] = objmaindetaild[0].Exp_FC.ToString();
                Session["raiserccc"] = objmaindetaild[0].Exp_CC.ToString();
                Session["Ecfdeclaratonnote"] = objModel.GetDecnote("4", "S").ToString();
                return View();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
        }
        [HttpGet]
        public ActionResult SupplierIndex()
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
                Session["CygnetFlag"] = "N";
                if (Session["DashBoard"] != null)
                {
                    EOW_Supplierinvoice eowemp = new EOW_Supplierinvoice();
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
                    Session["Ecfdeclaratonnote"] = objModel.GetDecnote("1", "E").ToString();

                    Session["Mainecfamt"] = eowemp.ECF_Amount.ToString();

                    ViewBag.supcode = eowemp.Suppliercode;
                    ViewBag.supname = eowemp.Suppliername;
                    Session["SupplierGidname"] = eowemp.Suppliername;
                    Session["SupplierGid"] = eowemp.Suppliergid;
                    ViewBag.exrate = eowemp.Exrate;
                    ViewBag.ecfPayMode = eowemp.ecf_Paymode;
                    EOW_Employeelst draftsup = new EOW_Employeelst();
                    draftsup.empCode = eowemp.Suppliercode;
                    draftsup.empName = eowemp.Suppliername;
                    draftsup.employeeGid = Convert.ToInt32(eowemp.Suppliergid);
                    Session["Supplierdetails"] = draftsup;
                    if (eowemp.DocId == "1")
                    {
                        ViewBag.POStatus = "PO";
                    }
                    if (eowemp.DocId == "2")
                    {
                        ViewBag.POStatus = "WO";
                    }
                    if (eowemp.DocId == "3")
                    {
                        ViewBag.POStatus = "Non PO/WO";
                    }
                    if (eowemp.DocId == "4")
                    {
                        ViewBag.POStatus = "Utility";
                    }
                    ViewBag.GstCharged = "N";
                    eowemp.ECF_Amount = objCmnFunctions.GetINRAmount(eowemp.ECF_Amount);
                    ViewBag.EOW_Supplierheader = eowemp;
                    Session["DashBoard"] = null;
                    Session["SelfModeecfval"] = objCmnFunctions.GetLoginUserGid().ToString();
                }
                else
                {
                    Session["SelfModeecfval"] = objCmnFunctions.GetLoginUserGid().ToString();
                    List<EOW_EmployeeeExpense> objmaindetail = new List<EOW_EmployeeeExpense>();
                    objmaindetail = objModel.SelectEmployeeeheader(objCmnFunctions.GetLoginUserGid().ToString()).ToList();

                    EOW_Supplierinvoice eowemp = new EOW_Supplierinvoice();
                    eowemp.Grade = objmaindetail[0].Grade.ToString();
                    eowemp.Raiser_Code = objmaindetail[0].Raiser_Code.ToString();
                    eowemp.Raiser_Name = objmaindetail[0].Raiser_Name.ToString();
                    eowemp.raisermodeId = "S";
                    eowemp.Raiser_Modedata = new SelectList(objModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", eowemp.raisermodeId);
                    eowemp.doctypedata = new SelectList(objModel.GetDoctype().ToList(), "DocId", "DocName", eowemp.DocId);
                    eowemp.Currencydata = new SelectList(objModel.GetCurrency().ToList(), "CurrencyId", "CurrencyName", "INR");
                    eowemp.CurrencyName = "INR";
                    ViewBag.EOW_Supplierheader = eowemp;
                    ViewBag.processdata = "first";
                    ViewBag.ecfPayMode = "Single Payment";
                }
                List<EOW_EmployeeeExpense> objmaindetaild = new List<EOW_EmployeeeExpense>();
                objmaindetaild = objModel.SelectEmployeeeheader(objCmnFunctions.GetLoginUserGid().ToString()).ToList();
                Session["raiserfcc"] = objmaindetaild[0].Exp_FC.ToString();
                Session["raiserccc"] = objmaindetaild[0].Exp_CC.ToString();
                Session["Ecfdeclaratonnote"] = objModel.GetDecnote("4", "S").ToString();
                return View();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
        }
        [HttpGet]
        public ActionResult CtMakerIndex()
        {
            try
            {
                Session["activeCT"] = "CT";
                Session["activetabrsic"] = "1";
                Session["QueueGide"] = "";
                if (Session["CentralTables"] != null)
                {
                    EOW_Supplierinvoice eowemp = new EOW_Supplierinvoice();
                    eowemp = (EOW_Supplierinvoice)Session["CentralTablesMain"];

                    ViewBag.processdatasec = "first";
                    ViewBag.processdata = "first";
                    if (Session["EcfGid"] == null)
                    {
                        Session["EcfGid"] = "0";
                    }
                    Session["invoiceGid"] = "0";
                    Session["Mainecfamt"] = eowemp.ECF_Amount.ToString();
                    Session["centralteamraiser"] = eowemp.raisermodeName;
                    ViewBag.supcode = eowemp.Suppliercode;
                    ViewBag.supname = eowemp.Suppliername;
                    Session["SupplierGid"] = eowemp.Suppliergid;
                    Session["SupplierName"] = eowemp.Suppliername;
                    Session["SupplierGidname"] = eowemp.Suppliername;
                    ViewBag.exrate = eowemp.Exrate;
                    ViewBag.ecfPayMode = "Single Payment";
                    if (eowemp.DocId == "1")
                    {
                        ViewBag.POStatus = "PO";
                    }
                    if (eowemp.DocId == "2")
                    {
                        ViewBag.POStatus = "WO";
                    }
                    if (eowemp.DocId == "3")
                    {
                        ViewBag.POStatus = "Non PO/WO";
                    }
                    if (eowemp.DocId == "4")
                    {
                        ViewBag.POStatus = "Utility";
                    }
                    ViewBag.GstCharged = "N";
                    eowemp.Raiser_Mode = "C";
                    Session["CTmakerclaim"] = "C";

                    if (Session["EcfGid"] == "0")
                    {
                        string insrtinvoice = objModel.UpdateCentralecf(eowemp, objCmnFunctions.GetLoginUserGid().ToString(), "S", Session["QueueGide"].ToString(), Session["centralteamraiser"].ToString());
                        EOW_EmployeeeExpense EmployeeeExpense = new EOW_EmployeeeExpense();
                        Session["EcfGid"] = objModel.selectecfgidBasic(EmployeeeExpense, objCmnFunctions.GetLoginUserGid().ToString());
                    }
                    eowemp.ECF_Amount = objCmnFunctions.GetINRAmount(eowemp.ECF_Amount);
                    ViewBag.EOW_Supplierheader = eowemp;

                    List<EOW_EmployeeeExpense> objmaindetaild = new List<EOW_EmployeeeExpense>();
                    objmaindetaild = objModel.SelectEmployeeeheader(Session["centralteamraiser"].ToString()).ToList();
                    Session["raiserfcc"] = objmaindetaild[0].Exp_FC.ToString();
                    Session["raiserccc"] = objmaindetaild[0].Exp_CC.ToString();
                    Session["Ecfdeclaratonnote"] = objModel.GetDecnote("4", "S").ToString();
                    Session["SelfModeecfval"] = Session["centralteamraiser"].ToString();
                }
                else
                {
                    EOW_Supplierinvoice eowemp = new EOW_Supplierinvoice();
                    eowemp = (EOW_Supplierinvoice)Session["CentralTablesMain"];

                    ViewBag.processdatasec = "first";
                    ViewBag.processdata = "first";
                    if (Session["EcfGid"] == null)
                    {
                        Session["EcfGid"] = "0";
                    }
                    Session["invoiceGid"] = "0";
                    Session["Mainecfamt"] = eowemp.ECF_Amount.ToString();
                    Session["centralteamraiser"] = eowemp.raisermodeName;
                    ViewBag.supcode = eowemp.Suppliercode;
                    ViewBag.supname = eowemp.Suppliername;
                    Session["SupplierGid"] = eowemp.Suppliergid;
                    Session["SupplierName"] = eowemp.Suppliername;
                    Session["SupplierGidname"] = eowemp.Suppliername;
                    ViewBag.exrate = eowemp.Exrate;
                    ViewBag.ecfPayMode = "Single Payment";
                    if (eowemp.DocId == "1")
                    {
                        ViewBag.POStatus = "PO";
                    }
                    if (eowemp.DocId == "2")
                    {
                        ViewBag.POStatus = "WO";
                    }
                    if (eowemp.DocId == "3")
                    {
                        ViewBag.POStatus = "Non PO/WO";
                    }
                    if (eowemp.DocId == "4")
                    {
                        ViewBag.POStatus = "Utility";
                    }
                    ViewBag.GstCharged = "N";
                    eowemp.Raiser_Mode = "C";
                    Session["CTmakerclaim"] = "C";

                    if (Session["EcfGid"].ToString().Equals("0"))
                    {
                        string insrtinvoice = objModel.UpdateCentralecf(eowemp, objCmnFunctions.GetLoginUserGid().ToString(), "S", Session["QueueGide"].ToString(), Session["centralteamraiser"].ToString());
                        EOW_EmployeeeExpense EmployeeeExpense = new EOW_EmployeeeExpense();
                        Session["EcfGid"] = objModel.selectecfgidBasic(EmployeeeExpense, objCmnFunctions.GetLoginUserGid().ToString());
                    }
                    eowemp.ECF_Amount = objCmnFunctions.GetINRAmount(eowemp.ECF_Amount);
                    ViewBag.EOW_Supplierheader = eowemp;
                    List<EOW_EmployeeeExpense> objmaindetaild = new List<EOW_EmployeeeExpense>();
                    objmaindetaild = objModel.SelectEmployeeeheader(Session["centralteamraiser"].ToString()).ToList();
                    Session["raiserfcc"] = objmaindetaild[0].Exp_FC.ToString();
                    Session["raiserccc"] = objmaindetaild[0].Exp_CC.ToString();
                    Session["Ecfdeclaratonnote"] = objModel.GetDecnote("4", "S").ToString();
                    Session["SelfModeecfval"] = Session["centralteamraiser"].ToString();
                }
                return View();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
        }
        [HttpPost]
        public ActionResult Index(string txtSuppliercode, string txtSuppliername, string txtAmortDescription, string CurrencyNameval, string txtecfamontal, EOW_Supplierinvoice obj, string rblAmort)
        {
            try
            {
                Session["activeCT"] = "Basic";
                string ecfnewamt = txtecfamontal.ToString();
                ecfnewamt = objCmnFunctions.GetRemovecommas(ecfnewamt);
                Session["activetabrsi"] = "1";
                if (obj.ecfremark != null)
                {
                    ViewBag.ecfrmarks = obj.ecfremark;
                }
                string message = "";
                List<EOW_EmployeeeExpense> objmaindetail = new List<EOW_EmployeeeExpense>();
                objmaindetail = objModel.SelectEmployeeeheader(objCmnFunctions.GetLoginUserGid().ToString()).ToList();

                EOW_Supplierinvoice eowemp = new EOW_Supplierinvoice();
                eowemp.Grade = objmaindetail[0].Grade.ToString();
                eowemp.Raiser_Code = objmaindetail[0].Raiser_Code.ToString();
                eowemp.ECF_Amount = txtecfamontal.ToString();
                eowemp.Raiser_Name = objmaindetail[0].Raiser_Name.ToString();
                eowemp.raisermodeId = "S";
                eowemp.Raiser_Modedata = new SelectList(objModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", eowemp.raisermodeId);
                eowemp.doctypedata = new SelectList(objModel.GetDoctype().ToList(), "DocId", "DocName", obj.DocId);
                eowemp.Currencydata = new SelectList(objModel.GetCurrency().ToList(), "CurrencyId", "CurrencyName", obj.CurrencyId);
                eowemp.CurrencyName = obj.CurrencyName;
                eowemp.CurrencyId = obj.CurrencyId;
                ViewBag.exrate = obj.Exrate;
                ViewBag.ecfPayMode = "Single Payment";
                if (txtAmortDescription != null)
                {
                    eowemp.amortdec = txtAmortDescription.ToString();
                    eowemp.from = obj.from.ToString();
                    eowemp.to = obj.to.ToString();
                }
                else
                {
                    eowemp.amortdec = "";
                    eowemp.from = "";
                    eowemp.to = "";
                }
                eowemp.amort = rblAmort.ToString();

                Session["Mainecfamt"] = ecfnewamt.ToString();

                if (obj.DocId == "1")
                {
                    ViewBag.POStatus = "PO";
                }
                if (obj.DocId == "2")
                {
                    ViewBag.POStatus = "WO";
                }
                if (obj.DocId == "3")
                {
                    ViewBag.POStatus = "Non PO/WO";
                }
                if (obj.DocId == "4")
                {
                    ViewBag.POStatus = "Utility";
                }
                ViewBag.GstCharged = "N";
                if (Session["Supplierdetails"].ToString() != "")
                {
                    EOW_Employeelst supplier = (EOW_Employeelst)Session["Supplierdetails"];
                    ViewBag.supcode = supplier.empCode.ToString();
                    ViewBag.supname = supplier.empName.ToString();
                    Session["SupplierGid"] = supplier.employeeGid.ToString();
                    Session["SupplierGidname"] = supplier.empName.ToString();
                }
                //if (Session["currentrate"].ToString() != "")
                //{
                //    ViewBag.exrate = Session["currentrate"].ToString();
                //}
                //else
                //{
                //    Session["currentrate"] = "1";
                //    obj.CurrencyId = "1";
                //    CurrencyNameval = "INR";
                //}

                EOW_Supplierinvoice objNew = new EOW_Supplierinvoice();
                objNew.ECF_Date = obj.ECF_Date.ToString();
                objNew.ECF_Amount = ecfnewamt.ToString();
                objNew.raisermodeId = eowemp.raisermodeId.ToString();
                objNew.CurrencyId = obj.CurrencyId;
                objNew.CurrencyName = obj.CurrencyName;
                objNew.Exrate = obj.Exrate;
                objNew.ecf_Paymode = "Single Payment";
                objNew.ecfremark = obj.ecfremark;
                objNew.DocId = obj.DocId.ToString();
                objNew.amort = rblAmort.ToString();
                objNew.ecfdescription = obj.ecfdescription;
                if (txtAmortDescription != null)
                {
                    objNew.amortdec = txtAmortDescription.ToString();
                }
                else
                {
                    objNew.amortdec = "";
                }
                if (obj.from != null)
                {
                    objNew.from = obj.from.ToString();
                }
                else
                {
                    objNew.from = "";
                }
                if (obj.to != null)
                {
                    objNew.to = "";
                    objNew.to = obj.to.ToString();
                }
                else
                {
                    objNew.to = "";
                }
                ViewBag.processdatasec = "first";
                ViewBag.processdata = "first";
                int nodays = 0;
                string result = objModel.GetStatusexcel("", Session["SelfModeecfval"].ToString(), "", "Ecfraiser");
                nodays = Convert.ToInt32(result);
                if (nodays < 15)
                {
                    ViewBag.processdatasec = "second";
                    ViewBag.processdata = "first";
                    ViewBag.message = "Employee Can't Raise ECF";
                }
                else
                {
                    if (Session["EcfGid"].ToString() != "")
                    {
                        message = objModel.InsertEmployeeeBasicupdatesup(objNew, objCmnFunctions.GetLoginUserGid().ToString(), Session["SupplierGid"].ToString(), objCmnFunctions.GetLoginUserGid().ToString(), Session["EcfGid"].ToString());
                        if (message == "Success")
                        {
                            ViewBag.processdatasec = "first";
                            ViewBag.processdata = "first";
                        }
                    }
                    else
                    {
                        message = objModel.InsertEmployeeeBasicsup(objNew, objCmnFunctions.GetLoginUserGid().ToString(), Session["SupplierGid"].ToString(), "S");
                        if (message == "success")
                        {
                            ViewBag.processdatasec = "first";
                            ViewBag.processdata = "first";
                            EOW_EmployeeeExpense objnewe = new EOW_EmployeeeExpense();
                            Session["EcfGid"] = objModel.selectecfgidBasic(objnewe, objCmnFunctions.GetLoginUserGid().ToString());
                        }
                    }
                }
                ViewBag.EOW_Supplierheader = eowemp;
                return View();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
        }
        [HttpPost]
        public ActionResult SupplierIndex(string txtSuppliercode, string txtSuppliername, string txtAmortDescription, string CurrencyNameval, string txtecfamontal, EOW_Supplierinvoice obj, string rblAmort)
        {
            try
            {
                Session["activeCT"] = "Basic";
                string ecfnewamt = txtecfamontal.ToString();
                ecfnewamt = objCmnFunctions.GetRemovecommas(ecfnewamt);
                Session["activetabrsi"] = "1";
                if (obj.ecfremark != null)
                {
                    ViewBag.ecfrmarks = obj.ecfremark;
                }
                string message = "";
                List<EOW_EmployeeeExpense> objmaindetail = new List<EOW_EmployeeeExpense>();
                objmaindetail = objModel.SelectEmployeeeheader(objCmnFunctions.GetLoginUserGid().ToString()).ToList();

                EOW_Supplierinvoice eowemp = new EOW_Supplierinvoice();
                eowemp.Grade = objmaindetail[0].Grade.ToString();
                eowemp.Raiser_Code = objmaindetail[0].Raiser_Code.ToString();
                eowemp.ECF_Amount = txtecfamontal.ToString();
                eowemp.Raiser_Name = objmaindetail[0].Raiser_Name.ToString();
                eowemp.raisermodeId = "S";
                eowemp.Raiser_Modedata = new SelectList(objModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", eowemp.raisermodeId);
                eowemp.doctypedata = new SelectList(objModel.GetDoctype().ToList(), "DocId", "DocName", obj.DocId);
                eowemp.Currencydata = new SelectList(objModel.GetCurrency().ToList(), "CurrencyId", "CurrencyName", obj.CurrencyId);
                eowemp.CurrencyName = obj.CurrencyName;
                eowemp.CurrencyId = obj.CurrencyId;
                ViewBag.exrate = obj.Exrate;
                ViewBag.ecfPayMode = "Single Payment";
                if (txtAmortDescription != null)
                {
                    eowemp.amortdec = txtAmortDescription.ToString();
                    eowemp.from = obj.from.ToString();
                    eowemp.to = obj.to.ToString();
                }
                else
                {
                    eowemp.amortdec = "";
                    eowemp.from = "";
                    eowemp.to = "";
                }
                eowemp.amort = rblAmort.ToString();

                Session["Mainecfamt"] = ecfnewamt.ToString();

                if (obj.DocId == "1")
                {
                    ViewBag.POStatus = "PO";
                }
                if (obj.DocId == "2")
                {
                    ViewBag.POStatus = "WO";
                }
                if (obj.DocId == "3")
                {
                    ViewBag.POStatus = "Non PO/WO";
                }
                if (obj.DocId == "4")
                {
                    ViewBag.POStatus = "Utility";
                }
                ViewBag.GstCharged = "N";
                if (Session["Supplierdetails"].ToString() != "")
                {
                    EOW_Employeelst supplier = (EOW_Employeelst)Session["Supplierdetails"];
                    ViewBag.supcode = supplier.empCode.ToString();
                    ViewBag.supname = supplier.empName.ToString();
                    Session["SupplierGid"] = supplier.employeeGid.ToString();
                    Session["SupplierGidname"] = supplier.empName.ToString();
                }
                
                EOW_Supplierinvoice objNew = new EOW_Supplierinvoice();
                objNew.ECF_Date = obj.ECF_Date.ToString();
                objNew.ECF_Amount = ecfnewamt.ToString();
                objNew.raisermodeId = eowemp.raisermodeId.ToString();
                objNew.CurrencyId = obj.CurrencyId;
                objNew.CurrencyName = obj.CurrencyName;
                objNew.Exrate = obj.Exrate;
                objNew.ecf_Paymode = "Single Payment";
                objNew.ecfremark = obj.ecfremark;
                objNew.DocId = obj.DocId.ToString();
                objNew.amort = rblAmort.ToString();
                objNew.ecfdescription = obj.ecfdescription;
                if (txtAmortDescription != null)
                {
                    objNew.amortdec = txtAmortDescription.ToString();
                }
                else
                {
                    objNew.amortdec = "";
                }
                if (obj.from != null)
                {
                    objNew.from = obj.from.ToString();
                }
                else
                {
                    objNew.from = "";
                }
                if (obj.to != null)
                {
                    objNew.to = "";
                    objNew.to = obj.to.ToString();
                }
                else
                {
                    objNew.to = "";
                }
                ViewBag.processdatasec = "first";
                ViewBag.processdata = "first";
                int nodays = 0;
                string result = objModel.GetStatusexcel("", Session["SelfModeecfval"].ToString(), "", "Ecfraiser");
                nodays = Convert.ToInt32(result);
                if (nodays < 15)
                {
                    ViewBag.processdatasec = "second";
                    ViewBag.processdata = "first";
                    ViewBag.message = "Employee Can't Raise ECF";
                }
                else
                {
                    if (Session["EcfGid"].ToString() != "")
                    {
                        message = objModel.InsertEmployeeeBasicupdatesup(objNew, objCmnFunctions.GetLoginUserGid().ToString(), Session["SupplierGid"].ToString(), objCmnFunctions.GetLoginUserGid().ToString(), Session["EcfGid"].ToString());
                        if (message == "Success")
                        {
                            ViewBag.processdatasec = "first";
                            ViewBag.processdata = "first";
                        }
                    }
                    else
                    {
                        message = objModel.InsertEmployeeeBasicsup(objNew, objCmnFunctions.GetLoginUserGid().ToString(), Session["SupplierGid"].ToString(), "S");
                        if (message == "success")
                        {
                            ViewBag.processdatasec = "first";
                            ViewBag.processdata = "first";
                            EOW_EmployeeeExpense objnewe = new EOW_EmployeeeExpense();
                            Session["EcfGid"] = objModel.selectecfgidBasic(objnewe, objCmnFunctions.GetLoginUserGid().ToString());
                        }
                    }
                }
                ViewBag.EOW_Supplierheader = eowemp;
                return View();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
        }
        [HttpGet]
        public PartialViewResult _supplierAttachmentCreate()
        {
            EOW_File objMAttachment = new EOW_File();
            objMAttachment.AttachmentTypeData = new SelectList(objModel.AttachmentTypedata().ToList(), "AttachmentTypeId", "AttachmentTypeName");
            return PartialView(objMAttachment);
        }

        [HttpPost]
        public JsonResult EmpAttachmentDelete(EOW_File EmployeeeExpensemodel)
        {
            int EmployeeePaymentGID = (int)EmployeeeExpensemodel.AttachmentFilenameId;
            string delamt = objModel.DeleteEmployeeeAttachment(EmployeeePaymentGID, Session["EcfGid"].ToString());
            //Session["Ecfamountpayment"] = Convert.ToInt32(Session["Ecfamountpayment"].ToString()) + Convert.ToInt32(delamt.ToString());
            return Json(EmployeeeExpensemodel, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult _supplierAttachmentDetails()
        {
            List<EOW_File> lstAttachment = new List<EOW_File>();
            //lstAttachment = objModel.GetEmployeeeAttachment(Session["EcfGid"].ToString(), objCmnFunctions.GetLoginUserGid().ToString()).ToList();
            return PartialView(lstAttachment);
        }
        [HttpPost]
        public JsonResult _supplierAttachmentCreate(EOW_File EmployeeeExpenseModel)
        {
            try
            {
                string msg = "2";
                if (TempData["_attFile"] != null)
                {
                    HttpPostedFileBase savefile = TempData["_attFile"] as HttpPostedFileBase;
                    string getname = objModel.InsertEmpAtt(savefile, EmployeeeExpenseModel, Session["EcfGid"].ToString(),  objCmnFunctions.GetLoginUserGid().ToString());
                    if (getname != "")
                    {
                        //var fileextension = Path.GetExtension(savefile.FileName);
                        //var stream = savefile.InputStream;
                        //var path = Path.Combine(@"C:\temp\", getname + fileextension);
                        //using (var fileStream = System.IO.File.Create(path))
                        //{
                        //    stream.CopyTo(fileStream);
                        //}
                        //Session["supplierattmtfile"] = null;

                        HttpPostedFileBase _attFile = TempData["_attFile"] as HttpPostedFileBase;
                        var stream = _attFile.InputStream;
                        string uploaderUrl = string.Format("{0}{1}", objModel.HoldFileUploadUrl(), getname + "." + _attFile.FileName.Split('.')[_attFile.FileName.Split('.').Length - 1]);
                        //using (var fileStream = System.IO.File.Create(uploaderUrl))
                        //{
                        //    stream.CopyTo(fileStream);
                        //}
                        //Muthu 2016-10-19
                        byte[] bytFile = null;
                        using (var memoryStream = new MemoryStream())
                        {
                            stream.CopyTo(memoryStream);
                            bytFile = memoryStream.ToArray();
                            ViewBag.Result = "inside File Stream";
                        }
                        var FileString = Convert.ToBase64String(bytFile);
                        var filename = getname + "." + _attFile.FileName.Split('.')[_attFile.FileName.Split('.').Length - 1];
                        var result = Cmnfs.SaveFileToServer(FileString, filename).Result;
                        //remove the temp data content.
                        TempData.Remove("_attFile");
                        msg = "1";
                        
                    }
                } 
                ViewBag.SearchItems = null;
                return Json(msg, JsonRequestBehavior.AllowGet);
               
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public PartialViewResult _InvoiceAttachmentCreate()
        {
            EOW_File objMAttachment = new EOW_File();
            objMAttachment.AttachmentTypeData = new SelectList(objModel.AttachmentTypedata().ToList(), "AttachmentTypeId", "AttachmentTypeName");
            return PartialView(objMAttachment);
        }
        [HttpPost]
        public JsonResult _InvoiceAttachmentCreate(EOW_File EmployeeeExpenseModel)
        {
            try
            {
                string msg = "2";
                if (TempData["_attFile"] != null)
                {
                    HttpPostedFileBase savefile = TempData["_attFile"] as HttpPostedFileBase;
                    string getname = objModel.InsertEmpAttinv(savefile, EmployeeeExpenseModel, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), objCmnFunctions.GetLoginUserGid().ToString());
                    if (getname != "")
                    {
                        //var fileextension = Path.GetExtension(savefile.FileName);
                        //var stream = savefile.InputStream;
                        //var path = Path.Combine(@"C:\temp\", getname + fileextension);
                        //using (var fileStream = System.IO.File.Create(path))
                        //{
                        //    stream.CopyTo(fileStream);
                        //}
                        //Session["supplierattmtfile"] = null;

                        HttpPostedFileBase _attFile = TempData["_attFile"] as HttpPostedFileBase;
                        var stream = _attFile.InputStream;
                        string uploaderUrl = string.Format("{0}{1}", objModel.HoldFileUploadUrl(), getname + "." + _attFile.FileName.Split('.')[_attFile.FileName.Split('.').Length - 1]);
                        ////using (var fileStream = System.IO.File.Create(uploaderUrl))
                        ////{
                        ////    stream.CopyTo(fileStream);
                        ////}
                        //remove the temp data content.

                        //Muthu 2016-10-19
                        byte[] bytFile = null;
                        using (var memoryStream = new MemoryStream())
                        {
                            stream.CopyTo(memoryStream);
                            bytFile = memoryStream.ToArray();

                        }
                        var FileString = Convert.ToBase64String(bytFile);
                        var filename = getname + "." + _attFile.FileName.Split('.')[_attFile.FileName.Split('.').Length - 1];
                        var result = Cmnfs.SaveFileToServer(FileString, filename).Result;
                        TempData.Remove("_attFile");
                        msg = "1";
                    }

                    ViewBag.SearchItems = null;
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ViewBag.SearchItems = null;
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public PartialViewResult _InvoiceAttachmentDetails()
        {
            List<EOW_File> lstAttachment = new List<EOW_File>();
            //lstAttachment = objModel.GetinvoiceAttachment(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString()).ToList();
            return PartialView(lstAttachment);
        }
        public FileResult Download(int id)
        {
            string FileName = objModel.downloadAttachment(id, Session["EcfGid"].ToString());
            ////int index = delamt.LastIndexOf(".");
            // //string[] seqNum = new string[] { delamt.Substring(0, index), delamt.Substring(index + 1) };
            ////string directory = @"C:\temp\";
            ////directory = directory + id.ToString() + "." + seqNum[1].ToString();
            ////byte[] fileBytes = System.IO.File.ReadAllBytes(directory);
            ////string fileName = seqNum[0].ToString() + "." + seqNum[1].ToString();
            ////return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            string[] str = FileName.Split('.');
            string directory = id.ToString() + "." + str[str.Length - 1].ToString();
            //string directory = objModel.HoldFileUploadUrl() + id.ToString() + "." + str[str.Length - 1].ToString();
            //byte[] fileBytes = System.IO.File.ReadAllBytes(directory);

            var apiresult = Cmnfs.DownloadFile(directory).Result;
            if (apiresult == "Fail")
            {
                return File("", "");
            }
            else
            {
                byte[] filebyte = Convert.FromBase64String(apiresult);
                return File(filebyte, System.Net.Mime.MediaTypeNames.Application.Octet, FileName);
            }
            // return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, FileName);
        }
        [HttpPost]
        public void UploadFilesatt(string attach = null, string attaching_format = null) //Pandiaraj 18-11-2019 
        {
            try
            {
                TempData["_attFile"] = null;
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        string fileextension = Path.GetExtension(Path.GetFileName(fileContent.FileName));
                        string[] attaching_fileformat = attaching_format.Split(',');
                        if (attaching_fileformat.Contains(fileextension.ToLower()))
                        {
                            var stream = fileContent.InputStream;
                            byte[] bytFile = null;
                            using (var memoryStream = new MemoryStream())
                            {
                                stream.CopyTo(memoryStream);
                                bytFile = memoryStream.ToArray();
                            }
                            bool isEXE = CmnFunctions.GetMimeTypeFromAttachment(bytFile, attach, fileextension.ToLower());
                            if (isEXE == false)
                            {
                                HttpPostedFileBase hpfBase = Request.Files[file] as HttpPostedFileBase;
                                TempData["_attFile"] = hpfBase;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
        }
        [HttpPost]
        public JsonResult Getsupplier(EOW_Employeelst EmployeeeExpensemodel)
        {
            Session["Supplierdetails"] = EmployeeeExpensemodel;
            return Json(EmployeeeExpensemodel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Getcurrencyrate(EOW_Currency brandID)
        {
            string rate = objModel.Getcurrencydata(brandID);
            Session["currentrate"] = rate.ToString();
            return Json(rate, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetRatenew(sinvotax tax)
        {
            return Json(objModel.GetRate(tax), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult _supplierTaxCreate()
        {
            ViewBag.Tax = new SelectList(objModel.GetTax().ToList(), "TaxId", "TaxName");
            sinvotax objtax = new sinvotax();
            return PartialView(objtax);
        }
        [HttpPost]
        public JsonResult _supplierTaxCreate(sinvotax tax)
        {
            string output = objModel.InsertsupplierTaxCreate(tax, Session["invoiceGid"].ToString(), objCmnFunctions.GetLoginUserGid().ToString(), Session["SupplierGid"].ToString());
            return Json(output, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SeuplierTaxDelete(sinvotax TravelClaimmodel)
        {
            int TravelModeGID = (int)TravelClaimmodel.TaxgId;
            string delamt = objModel.DeleteSupplideTax(TravelModeGID, Session["invoiceGid"].ToString());
            return Json(TravelClaimmodel, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult _SupplierTaxDetailsGrid()
        {
            List<sinvotax> lstnew = new List<sinvotax>();
            //lstnew = objModel.GetSupplierTax(Session["invoiceGid"].ToString()).ToList();
            return PartialView(lstnew);
        }
        [HttpGet]
        public PartialViewResult _TabtaxEdit(int id, string viewfor)
        {
            try
            {
                sinvotax objtax = new sinvotax();
                List<sinvotax> objobjMExpense = new List<sinvotax>();
                if (viewfor == "edit")
                {
                    ViewBag.viewfor = "edit";
                }
                else if (viewfor == "view")
                {
                    ViewBag.viewfor = "view";
                }
                Session["suppliertaxrow"] = id.ToString();
                ViewBag.SearchItems = null;
                objobjMExpense = objModel.SelectSuppliertaxid(Session["invoiceGid"].ToString(), id).ToList();
                objtax.Intax_Tax = objobjMExpense[0].Intax_Tax.ToString();
                objtax.Intax_Rate = Convert.ToString(objobjMExpense[0].Intax_Rate.ToString());
                //objtax.Intax_Taxableamt = Convert.ToDecimal(objobjMExpense[0].Intax_Taxableamt.ToString());
                //objtax.Intax_Taxamt = Convert.ToDecimal(objobjMExpense[0].Intax_Taxamt.ToString());

                objtax.Intax_Taxableamt = objCmnFunctions.GetINRAmount(objobjMExpense[0].Intax_Taxableamt.ToString());
                objtax.Intax_Taxamt = objCmnFunctions.GetINRAmount(objobjMExpense[0].Intax_Taxamt.ToString());

                objtax.Intax_Change = objobjMExpense[0].Intax_Change.ToString();
                ViewBag.Tax = new SelectList(objModel.GetTax().ToList(), "TaxId", "TaxName", objobjMExpense[0].Intax_Tax.ToString());
                int TaxId = Convert.ToInt32(objobjMExpense[0].Intax_Tax.ToString());
                ViewBag.TaxSubType = new SelectList(objModel.GetTaxSubTypeList(TaxId).ToList(), "TaxSubTypeID", "TaxSubTypeName", objobjMExpense[0].TaxSubTypeID);
                ViewBag.TaxData = objtax;
                return PartialView();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView();
            }
        }
        [HttpPost]
        public JsonResult _TabtaxEdit(sinvotax TravelClaimModel)
        {
            string output = "";
            try
            {
                output = objModel.updatesupplierTaxCreate(TravelClaimModel, Session["invoiceGid"].ToString(), Session["suppliertaxrow"].ToString(), Session["SupplierGid"].ToString());
                ViewBag.SearchItems = null;
                return Json(output, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public PartialViewResult _TabdebitlineCreate()
        {
            EOW_TravelClaim objMExpense = new EOW_TravelClaim();
            List<EOW_TravelClaim> objobjMExpense = new List<EOW_TravelClaim>();
            objobjMExpense = objModel.tSelectEmployeeeBasic(Session["SelfModeecfval"].ToString()).ToList();
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
        [HttpPost]
        public JsonResult _TabdebitlineCreate(EOW_TravelClaim TravelClaimModel)
        {
            string message = "";
            try
            {
                message = objModel.InsertsupplierCreate(TravelClaimModel, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), objCmnFunctions.GetLoginUserGid().ToString());
                ViewBag.SearchItems = null;
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public PartialViewResult _TabdebitlineDetails()
        {
            List<EOW_TravelClaim> lstnew = new List<EOW_TravelClaim>();
            //lstnew = objModel.GetSuppliserDedit(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), "S").ToList();
            return PartialView(lstnew);
        }
        [HttpPost]
        public JsonResult _TabdebitlineEdit(EOW_TravelClaim TravelClaimModel)
        {
            string message = "";
            try
            {
                message = objModel.UpdateSupplierDebit(TravelClaimModel, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), objCmnFunctions.GetLoginUserGid().ToString(), Session["supplierdebitactiverow"].ToString());
                ViewBag.SearchItems = null;
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
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
                objobjMExpense = objModel.GetSupplierDebitsingle(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), "E", id).ToList();
                objMExpenseEdit.OUCode = objobjMExpense[0].OUCode.ToString();
                objMExpenseEdit.ProductCode = objobjMExpense[0].ProductCode.ToString();

                objMExpenseEdit.FC = objobjMExpense[0].FC.ToString();
                objMExpenseEdit.CC = objobjMExpense[0].CC.ToString();
                objMExpenseEdit.travelDescription = objobjMExpense[0].travelDescription.ToString();
                objMExpenseEdit.TravelHsnDesc = objobjMExpense[0].TravelHsnDesc.ToString(); 
                objMExpenseEdit.AssetCatList = new SelectList(objModel.GetAssetCategoryList().ToList(), "AssetCatId", "AssetCatName", objobjMExpense[0].AssetCatId.ToString());
                objMExpenseEdit.AssetSubCatList = new SelectList(objModel.GetAssetSubCategoryList(objobjMExpense[0].AssetCatId).ToList(), "AssetSubCatId", "AssetSubCatName", objobjMExpense[0].AssetSubCatId.ToString());
               
                
                objMExpenseEdit.ExpNatureofExpdata = new SelectList(objModel.NatureofExpensesdataothersupplier().ToList(), "NatureofExpensesId", "NatureofExpensesName", objobjMExpense[0].NatureofExpensesId.ToString());
                objMExpenseEdit.ExpCatdata = new SelectList(objModel.ExpenseCategorydata(objobjMExpense[0].NatureofExpensesId).ToList(), "ExpenseCategoryId", "ExpenseCategoryName", objobjMExpense[0].ExpenseCategoryId.ToString());
                objMExpenseEdit.ExpSubCatdata = new SelectList(objModel.SubCategorydata(objobjMExpense[0].ExpenseCategoryId).ToList(), "SubCategoryId", "SubCategoryName", objobjMExpense[0].SubCategoryId.ToString());
                if (objobjMExpense[0].ProdServCategory.ToString().Equals("E"))
                {
                    objMExpenseEdit.HsncodeList = new SelectList(objModel.GetAssetExpHsndetails(Convert.ToInt32(objobjMExpense[0].SubCategoryId),"E").ToList(), "TravelHsnid", "TravelHsnCode", objobjMExpense[0].TravelHsnid.ToString()); //Ramya
                }
                else
                {
                    objMExpenseEdit.HsncodeList = new SelectList(objModel.GetAssetExpHsndetails(Convert.ToInt32(objobjMExpense[0].AssetSubCatId), "A").ToList(), "TravelHsnid", "TravelHsnCode", objobjMExpense[0].TravelHsnid.ToString()); //Ramya
                }
                //  }
                objMExpenseEdit.Exp_FCC = new SelectList(objModel.fcdata().ToList(), "raiserfcId", "raiserfcName", objobjMExpense[0].FC.ToString());
                objMExpenseEdit.Exp_CCC = new SelectList(objModel.ccdata().ToList(), "raiserccId", "raiserccName", objobjMExpense[0].CC.ToString());
                //objMExpenseEdit.Exp_FCCC = objobjMExpense[0].Exp_FCCC.ToString();

                // objMExpenseEdit.Amount = objobjMExpense[0].Amount.ToString();
                objMExpenseEdit.Amount = objCmnFunctions.GetINRAmount(objobjMExpense[0].Amount.ToString());
                objMExpenseEdit.ProdServCategory = objobjMExpense[0].ProdServCategory.ToString();
                objMExpenseEdit.RCMFlag = objobjMExpense[0].RCMFlag.ToString();
                //Session["Ecfamountvaltmeditoe"] = Convert.ToDecimal(Session["Ecfamountvaloe"].ToString()) + Convert.ToDecimal(objobjMExpense[0].Amount.ToString());
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
        public JsonResult GetExpenseCategory(EOW_TravelClaim EmployeeeExpense)
        {
            return Json(objModel.ExpenseCategorydata(EmployeeeExpense.NatureofExpensesId), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetSubCategory(EOW_TravelClaim EmployeeeExpense)
        {
            return Json(objModel.SubCategorydata(EmployeeeExpense.ExpenseCategoryId), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteOtherTravelExpense(EOW_TravelClaim TravelClaimmodel)
        {
            int TravelModeGID = (int)TravelClaimmodel.TravelMode_GID;
            string delamt = objModel.DeleteSupplideExpense(TravelModeGID, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString());
            return Json(TravelClaimmodel, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult _supplierPaymentCreate()
        {
            EOW_Payment objMPayment = new EOW_Payment();
            objMPayment.PaymentModedata = new SelectList(objModel.PaymentModesupplierdata(Session["SupplierGid"].ToString()).ToList(), "PaymentModeId", "PaymentModeName");
            objMPayment.Beneficiary = objModel.GetSupplierGID(Session["SupplierGid"].ToString());
            return PartialView(objMPayment);
        }
        [HttpPost]
        public JsonResult _supplierPaymentCreate(EOW_Payment EmployeeeExpenseModel)
        {
            try
            {
                string meaasge = objModel.InsertSupplierPayment(EmployeeeExpenseModel, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), Session["SupplierGid"].ToString(), Session["Ecfamountpaymentfirst"].ToString());
                //Session["Ecfamountpayment"] = Convert.ToInt32(Session["Ecfamountpayment"].ToString()) + Convert.ToInt32(EmployeeeExpenseModel.EmployeeePayment_Amount.ToString());
                ViewBag.SearchItems = null;
                return Json(meaasge, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult EmpPaymentDelete(EOW_Payment EmployeeeExpensemodel)
        {
            int EmployeeePaymentGID = (int)EmployeeeExpensemodel.Paymentgid;
            string delamt = objModel.DeleteSupplierPayment(EmployeeePaymentGID, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), Session["invoicePaymentamnt"].ToString(), Session["SupplierGid"].ToString());
            return Json(EmployeeeExpensemodel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult _SupplierPaymentEdit(EOW_Payment objMExpenseEdit)
        {
            string message = objModel.UpdateSupplierPayment(objMExpenseEdit, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), Session["suppPaymentactiverow"].ToString(), Session["Ecfamountpaymentfirst"].ToString(), Session["invoicePaymentamnt"].ToString(), Session["SupplierGid"].ToString());
            Session["EmpPaymentactiverowExceptions"] = "0";
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult _SupplierPaymentEdit(int id, string viewfor)
        {
            try
            {
                EOW_Payment objMPaymentEdit = new EOW_Payment();
                List<EOW_Payment> objobjMPayment = new List<EOW_Payment>();
                if (viewfor == "edit")
                {
                    ViewBag.viewfor = "edit";
                }
                else if (viewfor == "view")
                {
                    ViewBag.viewfor = "view";
                }
                Session["suppPaymentactiverow"] = id.ToString();
                ViewBag.SearchItems = null;
                objobjMPayment = objModel.SelectEmployeeePaymentid(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), id).ToList();
                objMPaymentEdit.Description = objobjMPayment[0].Description.ToString();
                objMPaymentEdit.Beneficiary = objobjMPayment[0].Beneficiary.ToString();
                objMPaymentEdit.Ifsccode = objobjMPayment[0].Ifsccode.ToString();
                //objMPaymentEdit.PaymentAmount = objobjMPayment[0].PaymentAmount.ToString();
                objMPaymentEdit.PaymentAmount = objCmnFunctions.GetINRAmount(objobjMPayment[0].PaymentAmount.ToString());
                objMPaymentEdit.RefNoName = objobjMPayment[0].RefNoName.ToString();
                objMPaymentEdit.PaymentModedata = new SelectList(objModel.PaymentModesupplierdata(Session["SupplierGid"].ToString()).ToList(), "PaymentModeName", "PaymentModeName", objobjMPayment[0].PaymentModeName.ToString());
                if (objobjMPayment[0].PaymentModeName == "CRN")
                {
                    objMPaymentEdit.Refdata = new SelectList(objModel.RefNodatasuppliercrn(Session["SupplierGid"].ToString()).ToList(), "RefNoName", "RefNoName", objobjMPayment[0].RefNoName.ToString());
                }
                else if (objobjMPayment[0].PaymentModeName == "PPX")
                {
                    objMPaymentEdit.Refdata = new SelectList(objModel.EmployeeePaymentRefNodatasupplier(Session["SupplierGid"].ToString()).ToList(), "RefNoName", "RefNoName", objobjMPayment[0].RefNoName.ToString());
                }
                else if (objobjMPayment[0].PaymentModeName == "EFT")
                {
                    List<EOW_RefNo> objselect = new List<EOW_RefNo>();
                    EOW_RefNo objselModel = new EOW_RefNo();
                    objselModel.RefNoId = objobjMPayment[0].RefNoName.ToString();
                    objselModel.RefNoName = objobjMPayment[0].RefNoName.ToString();
                    objselect.Add(objselModel);
                    objMPaymentEdit.Refdata = new SelectList(objselect, "RefNoName", "RefNoName");
                    //objMPaymentEdit.Refdata = new SelectList(objModel.RefNodatasupplierSUS(Session["SupplierGid"].ToString()).ToList(), "RefNoName", "RefNoName", objobjMPayment[0].RefNoName.ToString());
                }
                else if (objobjMPayment[0].PaymentModeName == "DD" || objobjMPayment[0].PaymentModeName == "SUS")
                {
                    objMPaymentEdit.Beneficiarycardno = objobjMPayment[0].RefNoName.ToString();
                    List<EOW_RefNo> objselect = new List<EOW_RefNo>();
                    EOW_RefNo objselModel = new EOW_RefNo();
                    objselModel.RefNoId = Convert.ToString("0");
                    objselModel.RefNoName = Convert.ToString("Select");
                    objselect.Add(objselModel);
                    objMPaymentEdit.Refdata = new SelectList(objselect, "RefNoName", "RefNoName");
                }
                else
                {
                    List<EOW_RefNo> objselect = new List<EOW_RefNo>();
                    EOW_RefNo objselModel = new EOW_RefNo();
                    objselModel.RefNoId = Convert.ToString("0");
                    objselModel.RefNoName = Convert.ToString("Select");
                    objselect.Add(objselModel);
                    objMPaymentEdit.Refdata = new SelectList(objselect, "RefNoName", "RefNoName");
                }
                //Session["EmpPaymentactiverowamt"] = objobjMPayment[0].PaymentAmount.ToString();
                Session["Ecfamountpaymentecfi"] = Convert.ToDecimal(Session["Ecfamountpaymentfirst"].ToString()) + Convert.ToDecimal(objobjMPayment[0].PaymentAmount.ToString());
                Session["EmpPaymentactiverowbefore"] = objobjMPayment[0].PaymentAmount.ToString();
                decimal arfamt = Convert.ToDecimal(objobjMPayment[0].Exception.ToString()) + Convert.ToDecimal(objobjMPayment[0].PaymentAmount.ToString());
                Session["EmpPaymentactiverowExceptions"] = objobjMPayment[0].Exception.ToString();
                ViewBag.Exception = arfamt;

                return PartialView(objMPaymentEdit);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView();
            }
        }
        [HttpPost]
        public JsonResult GetPaymodeRefNo(EOW_EmployeeeExpense EmployeeeExpense)
        {
            return Json(objModel.EmployeeePaymentRefNodatasupplier(Session["SupplierGid"].ToString()), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult _supplierPaymentmodepopup()
        {
            List<EOW_RefNo> lst = new List<EOW_RefNo>();
            //lst = objModel.EmployeeePaymentRefNodatagri(Session["SupplierGid"].ToString(), "S").ToList();
            return PartialView(lst);
        }
        [HttpGet]
        public PartialViewResult _supplierPaymentmodecrn()
        {
            List<EOW_RefNo> lst = new List<EOW_RefNo>();
            //lst = objModel.EmployeeePaymentRefNodatacrn(Session["SupplierGid"].ToString(), "S").ToList();
            //lst = objModel.RefNodatasuppliercrn(Session["SupplierGid"].ToString()).ToList();
            return PartialView(lst);
        }
        [HttpGet]
        public PartialViewResult _supplierPaymentmodeeft()
        {
            List<EOW_RefNo> lst = new List<EOW_RefNo>();
            //lst = objModel.EmployeeePaymentRefNodatacrn(Session["SupplierGid"].ToString(), "S").ToList();
            return PartialView(lst);
        }
        [HttpGet]
        public PartialViewResult _supplierPaymentmodeChq()
        {
            List<EOW_RefNo> lst = new List<EOW_RefNo>();
            //lst = objModel.EmployeeePaymentchq(Session["SupplierGid"].ToString(), "S").ToList();
            return PartialView(lst);
        }
        public ActionResult GLSearch(string listfor)
        {

            try
            {
                List<EOW_Supplierinvoice> obj = new List<EOW_Supplierinvoice>();
                EOW_Supplierinvoice Getvaluesearchvalue = new EOW_Supplierinvoice();
                if (listfor == "search")
                {
                    if (Session["SearchEmployeedata"] != null)
                    {
                        if (Session["SerchViewbag"] != "" && Session["SerchViewbag"] != null)
                        {
                            Getvaluesearchvalue = (EOW_Supplierinvoice)Session["SerchViewbag"];
                        }
                        @ViewBag.EmployeeName = Getvaluesearchvalue.expcat_name;
                        @ViewBag.EmployeeCode = Getvaluesearchvalue.expcat_code;
                        obj = (List<EOW_Supplierinvoice>)Session["SearchEmployeedata"];
                    }
                    else
                    {
                        obj = objModel.SelectGLCode().ToList();
                    }
                }
                else
                {
                    Session["SearchEmployeedata"] = null;
                    Session["SerchViewbag"] = null;
                    obj = objModel.SelectGLCode().ToList();
                }
                return View(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult GLSearchWithParam(string GLName = "", string GLCode = "")
        {
            List<EOW_Supplierinvoice> recordsearch = new List<EOW_Supplierinvoice>();
            EOW_Supplierinvoice emp = new EOW_Supplierinvoice();
            if (GLName != "" || GLCode != "")
            {
                @ViewBag.EmployeeName = GLName;
                @ViewBag.EmployeeCode = GLCode;
                emp.expcat_code = GLCode;
                emp.expcat_name = GLName;
                Session["SerchViewbag"] = emp;
                recordsearch = objModel.SelectGLSearch(GLName, GLCode).ToList();
                Session["SearchEmployeedata"] = recordsearch;
            }
            else
            {
                Session["SerchViewbag"] = null;
                Session["SerchViewbag"] = null;
                recordsearch = objModel.SelectGLSearch(GLName, GLCode).ToList();
            }

            return Json(recordsearch, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult _SupplierPaymentGrid()
        {
            List<EOW_Payment> lstPayment = new List<EOW_Payment>();
            //lstPayment = objModel.GetEmployeeePayment(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString()).ToList();
            return PartialView(lstPayment);
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
            objobjMPayment = objModel.GetSuppliersingledata(Session["EcfGid"].ToString(), "S", TravelModeGID).ToList();
            ViewBag.invId = objobjMPayment[0].Invoicegid;
            objMPaymentEdit.Invoicegid = Convert.ToInt32(objobjMPayment[0].Invoicegid.ToString());
            objMPaymentEdit.InvoiceDate = objobjMPayment[0].InvoiceDate.ToString();
            objMPaymentEdit.InvoiceNo = objobjMPayment[0].InvoiceNo.ToString();
            objMPaymentEdit.Description = objobjMPayment[0].Description.ToString();
            objMPaymentEdit.Amount = objobjMPayment[0].Amount.ToString();
            objMPaymentEdit.TaxableAmount = objobjMPayment[0].TaxableAmount;
            objMPaymentEdit.SerMonth = objobjMPayment[0].SerMonth.ToString();
            objMPaymentEdit.Retensionflg = objobjMPayment[0].Retensionflg.ToString();
            objMPaymentEdit.Retensionper = objobjMPayment[0].Retensionper.ToString();
            objMPaymentEdit.Retensionamount = objobjMPayment[0].Retensionamount.ToString();
            objMPaymentEdit.Retensionrelse = objobjMPayment[0].Retensionrelse.ToString();
            objMPaymentEdit.Provision = objobjMPayment[0].Provision.ToString();
            objMPaymentEdit.GstCharged = objobjMPayment[0].GstCharged.ToString();
            objMPaymentEdit.ProviderLocation = objobjMPayment[0].ProviderLocation.ToString();
            objMPaymentEdit.ReceiverLocation = objobjMPayment[0].ReceiverLocation.ToString();
            objMPaymentEdit.GstinVendor = Convert.ToString(objobjMPayment[0].GstinVendor);
            objMPaymentEdit.GstinFiccl = Convert.ToString(objobjMPayment[0].GstinFiccl);
            objMPaymentEdit.invoice_Cygnet_Gid = objobjMPayment[0].invoice_Cygnet_Gid.ToString();
            Session["editorview"] = "Edit";
            return Json(objobjMPayment, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult _suppliserinvoiceupdate(string GstCharged, string ProviderLocation, string GstinVendor, string ReceiverLocation, string GstinFiccl)
        {
            string Data1 = "";
            try
            {
                if (Session["invoiceGid"].ToString() != "" || Session["invoiceGid"].ToString() != null)
                {
                    DataSet ds = db.supplierGstUpdate(Session["invoiceGid"].ToString(), GstCharged, ProviderLocation, GstinVendor, ReceiverLocation, GstinFiccl);
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                        Session["GSTFlag"] = objModel.GetStatusexcel(Session["EcfGid"].ToString(), "", "", "GSTFlag");
                        // Err = "success";
                    }
                }
                return Json(Data1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public PartialViewResult _GetGstDetails()
        {
            //Session["addcurpogis"] = null;
            // List<EOW_PO> lstnew = new List<EOW_PO>();
            // lstnew = objModel.Getpodetailsgrid(values, valuesid, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), "S").ToList();
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
        [HttpPost]
        public JsonResult createSupplierinvoice(EOW_EmployeeeExpense Suppliermodel)
        {
            Session["invoiceGid"] = "";
            //string insrtinvoice = objModel.InsertSupplierinvoice(Suppliermodel, Session["SupplierGid"].ToString(), Session["EcfGid"].ToString(), "S");
            //Session["invoiceGid"] = objModel.selectinvoiceidBasic(Suppliermodel, Session["SupplierGid"].ToString(), Session["EcfGid"].ToString(), "S");
            Session["editorview"] = "New";
            Session["Supplierpaydebitedit"] = "Edit";
            return Json("Sucess", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult _suppliserinvoiceadd(EOW_SupplierModelgrid Suppliermodel)
        {
            string CTmakerclaim = "",Err = "",Errstatusout = ""; 
            try
            {
                Err = Session["invoiceGid"].ToString();
                CTmakerclaim = Session["CTmakerclaim"].ToString();
                //Session["SupplierGidname"]
                Errstatusout = objModel.UpdateSupplierinvoice(Suppliermodel, Session["SupplierGid"].ToString(), Err, Session["EcfGid"].ToString(), "S", Session["SupplierGidname"].ToString());
                EOW_EmployeeeExpense model = new EOW_EmployeeeExpense();
                if (Err == "" && Errstatusout != "DuplicateInvoice")
                {
                    Session["invoiceGid"] = objModel.selectinvoiceidBasic(model, Session["SupplierGid"].ToString(), Session["EcfGid"].ToString(), "S");
                    ViewBag.invId = Convert.ToString(Session["invoiceGid"]);
                }
                if (Errstatusout == "success" || Errstatusout == "Success")
                {
                    string invoicdate = Suppliermodel.InvoiceDate;
                    int nodays = 0;
                    string datas = objModel.GetStatusexcel(objCmnFunctions.convertoDateTimeString(invoicdate), "", "", "Invoicedate");
                    Session["GSTFlag"] = objModel.GetStatusexcel(Session["EcfGid"].ToString(), "", "", "GSTFlag");
                    nodays = Convert.ToInt32(datas);
                    if (nodays > 90)
                    {
                        if (Convert.ToDecimal(Suppliermodel.Amount) > 200000)
                        {
                            Errstatusout = "FinanceApprove";
                        }
                    }
                }
                Err = "success";
                return Json(Errstatusout, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult _suppliserinvoicechkppx(EOW_SupplierModelgrid Suppliermodel)
        {
            string Err = "Sucess";
            try
            {
                string result = objModel.EmployeeePaymentppxcheck(Session["SupplierGid"].ToString(), Suppliermodel.Amount, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString());
                if (result == "Yes")
                {
                    Err = "PPX";
                }
                else
                {
                    Err = "Sucess";
                }
                return Json(Err, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult _suppliserinvoiceCreateb(EOW_SupplierModelgrid Suppliermodel)
        {
            string Err = "";
            try
            {
                decimal debit = Convert.ToDecimal(Session["invoiceDebitamnt"].ToString());
                decimal pay = Convert.ToDecimal(Session["invoicePaymentamnt"].ToString());
                decimal tax = Convert.ToDecimal(Session["invoiceTaxamnt"].ToString());
                decimal sumdebitax = debit + tax;
                decimal invoiceamt = Convert.ToDecimal(Suppliermodel.Amount);

                if (debit != invoiceamt)
                {
                    Err = "Debit";
                }
                else if (pay != invoiceamt)
                {
                    Err = "Payment";
                }
                //else if (Suppliermodel.potype == "PO" || Suppliermodel.potype == "WO")
                //{
                //    decimal poamt = Convert.ToDecimal(Suppliermodel.TaxableAmount.ToString());
                //    decimal toltacpay = poamt;
                //    if (toltacpay > invoiceamt)
                //    {
                //        Err = "POAmt";
                //    }
                //}

                if (Err != "Debit" && Err != "POAmt" && Err != "Payment")
                {
                    Err = Session["invoiceGid"].ToString();
                    //string insrtinvoice = objModel.UpdateSupplierinvoice(Suppliermodel, Session["SupplierGid"].ToString(), Err, Session["EcfGid"].ToString(), "S", Session["SupplierGidname"].ToString());
                    Session["invoiceGid"] = "";
                    Session["invoiceDebitamnt"] = "0";
                    Session["invoicePaymentamnt"] = "0";
                    Session["invoiceTaxamnt"] = "0";
                    Err = "Success";
                }
                return Json(Err, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult _suppliserinvoiceclear(EOW_SupplierModelgrid Suppliermodel)
        {
            string Err = "";
            string ecfno = "";
            try
            {
                Session["invoiceDebitamnt"] = "";
                Session["invoicePaymentamnt"] = "";
                Err = Session["invoiceGid"].ToString();
                ecfno = Session["EcfGid"].ToString();
                if (Session["editorview"].ToString() == "New")
                {
                    string insrtinvoice = objModel.DeleteSuppliernewlst(Err, ecfno);
                    if (insrtinvoice == "Success")
                    {
                        Session["invoiceGid"] = "";
                        Err = "Success";
                    }
                    if (Err == "")
                    {
                        Err = "Error";
                    }
                }

                return Json(Err, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult DeleteSupppersdExpense(EOW_TravelClaim TravelClaimmodel)
        {
            int TravelModeGID = (int)TravelClaimmodel.TravelMode_GID;
            string delamt = objModel.DeleteSupppers(TravelModeGID, Session["EcfGid"].ToString());
            Session["invoiceGid"] = "";
            return Json(TravelClaimmodel, JsonRequestBehavior.AllowGet);
        }
        public ActionResult downloadsexcel()
        {
            DataTable dtnew = (DataTable)Session["Errdatatablesu"];
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "SupplierDebitline.xls"));
            Response.ContentType = "application/vnd.ms-excel";
            DataTable dt = dtnew;
            string str = string.Empty;
            foreach (DataColumn dtcol in dt.Columns)
            {
                Response.Write(str + dtcol.ColumnName);
                str = "\t";
            }
            Response.Write("\n");
            foreach (DataRow dr in dt.Rows)
            {
                str = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Response.Write(str + Convert.ToString(dr[j]));
                    str = "\t";
                }
                Response.Write("\n");
            }
            Response.End();
            return View();
        }
        public ActionResult samdownloadsexcelsup()
        {
            using (DataTable dtnew = new DataTable())
            {
                dtnew.Columns.Add("SNo", typeof(int));
                dtnew.Columns.Add("Nature of Expenses", typeof(string));
                dtnew.Columns.Add("Main Category", typeof(string));
                dtnew.Columns.Add("Sub Category", typeof(string));
                dtnew.Columns.Add("Description", typeof(string));
                dtnew.Columns.Add("Function Code", typeof(string));
                dtnew.Columns.Add("Cost Code", typeof(string));
                dtnew.Columns.Add("Product Code", typeof(string));
                dtnew.Columns.Add("OU Code", typeof(string));
                dtnew.Columns.Add("Amount", typeof(decimal));
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtnew, "SupplierinoiceDebitline");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=SupplierinoiceDebitlineTemplate.xls");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            return View();
        }

        public ActionResult samdownloadsexcelsupold()
        {
            DataTable dtnew = new DataTable();
            dtnew.Columns.Add("SNo", typeof(int));
            dtnew.Columns.Add("Nature of Expenses", typeof(string));
            dtnew.Columns.Add("Main Category", typeof(string));
            dtnew.Columns.Add("Sub Category", typeof(string));
            dtnew.Columns.Add("Description", typeof(string));
            dtnew.Columns.Add("Function Code", typeof(string));
            dtnew.Columns.Add("Cost Code", typeof(string));
            dtnew.Columns.Add("Product Code", typeof(string));
            dtnew.Columns.Add("OU Code", typeof(string));
            dtnew.Columns.Add("Amount", typeof(decimal));

            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "SupplierDebitline.xls"));
            Response.ContentType = "application/vnd.ms-excel";
            DataTable dt = dtnew;
            string str = string.Empty;
            foreach (DataColumn dtcol in dt.Columns)
            {
                Response.Write(str + dtcol.ColumnName);
                str = "\t";
            }
            Response.Write("\n");
            foreach (DataRow dr in dt.Rows)
            {
                str = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Response.Write(str + Convert.ToString(dr[j]));
                    str = "\t";
                }
                Response.Write("\n");
            }
            Response.End();
            return View();
        }
        public string HoldFileUploadUrlDSA()
        {
            string x = "";
            try
            {
                x = System.Configuration.ConfigurationManager.AppSettings["HoldFileUpload"].ToString();
            }
            catch { x = ""; }
            return (x == null || x.Trim() == "") ? "" : x;
        }
        [HttpPost]
        public void UploadFilesnew()
        {
            try
            {
                foreach (string filenew in Request.Files)
                {
                    var fileContent = Request.Files[filenew];

                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        HttpPostedFileBase hpf = Request.Files[filenew] as HttpPostedFileBase;
                        TempData["_supplierattmtfileexcel"] = hpf;
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
        }
        [HttpPost]
        public async Task<JsonResult> UploadFiles()
        {
            List<SheetData> objparent = new List<SheetData>();
            string Extensionnew = "";
            string Extension1 = "";
            string error = "";
            try
            {
                string path1 = "";
                if (TempData["_supplierattmtfileexcel"] != null)
                {
                    //HttpPostedFileBase savefile = (HttpPostedFileBase)Session["supplierattmtfileexcel"];
                    HttpPostedFileBase savefile = TempData["_supplierattmtfileexcel"] as HttpPostedFileBase;
                    string Extension = Path.GetFileName(savefile.FileName);
                    string n = string.Format(objCmnFunctions.GetLoginUserGid().ToString() + "Local-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                    Extension1 = System.IO.Path.GetExtension(savefile.FileName);
                    //path1 = objModel.HoldFileUploadUrl() + n + Extension;
                    Extensionnew = n + Extension1;
                    path1 = string.Format("{0}{1}", HoldFileUploadUrlDSA(), Extensionnew);
                    if (System.IO.File.Exists(path1))
                    {
                        System.IO.File.Delete(path1);
                    }
                    savefile.SaveAs(path1);

                    SheetData objModel;
                    int count = 0;
                    string sheets = "";
                    FileStream stream = new FileStream(path1, FileMode.Open, FileAccess.Read);
                    DataSet result1 = new DataSet();
                    if (Extension1.ToLower() == ".xlsx")
                    {
                        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        excelReader.IsFirstRowAsColumnNames = true;
                        result1 = excelReader.AsDataSet();
                        excelReader.Close();
                    }
                    else if (Extension1.ToLower() == ".xls")
                    {
                        IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                        excelReader.IsFirstRowAsColumnNames = true;
                        result1 = excelReader.AsDataSet();
                        excelReader.Close();
                    }
                    else
                    {
                        error = "Error";
                        count++;
                        objModel = new SheetData();
                        objModel.SheetId = count;
                        objModel.SheetName = error.ToString();
                        objparent.Add(objModel);
                    }

                    //FileStream stream = new FileStream();
                    //stream = File.Open(path1, FileMode.Open, FileAccess.Read);
                    //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                    //IExcelDataReader excelReaderf = ExcelReaderFactory.CreateBinaryReader(stream);
                    //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                    //IExcelDataReader excelReadern = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    //...
                    //3. DataSet - The result of each spreadsheet will be created in the result.Tables
                    //DataSet result = excelReader.AsDataSet();
                    //4. DataSet - Create column names from first row
                    if (result1 != null && result1.Tables.Count > 0)
                    {
                        foreach (DataTable dt in result1.Tables)
                        {
                            sheets = dt.TableName.ToString().Trim();
                            count++;
                            objModel = new SheetData();
                            objModel.SheetId = count;
                            objModel.SheetName = sheets.ToString();
                            objparent.Add(objModel);
                        }
                    }

                    Session["Tempdataset"] = result1;
                    TempData.Remove("_supplierattmtfileexcel");
                }
                return Json(objparent.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        public List<SheetData> GetSheetData(string excelConnectionString)
        {
            List<SheetData> objparent = new List<SheetData>();
            SheetData objModel;
            //OleDbConnection con = null;
            //DataTable dt = null;
            //con = new OleDbConnection(excelConnectionString);
            //con.Open();
            //dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            //if (dt == null)
            //{
            //    return null;
            //}
            //int count = 0;
            //string sheets = "";
            //foreach (DataRow row in dt.Rows)
            //{
            //    sheets = row["TABLE_NAME"].ToString();
            //    sheets = sheets.Replace("$", "");
            //    count++;
            //    objModel = new SheetData();
            //    objModel.SheetId = count;
            //    objModel.SheetName = sheets.ToString();
            //    objparent.Add(objModel);
            //}
            return objparent;
        }
        public JsonResult Localconveyancestatuserrs(EOW_TravelClaim obj)
        {
            string mag = "";
            string strCols = "";
            string[] strColArr;
            try
            {
                DataSet result1 = new DataSet();
                result1 = (DataSet)Session["Tempdataset"];
                var dataTable = new DataTable();
                dataTable = result1.Tables[obj.Branch.ToString()];
                Session["Tempdatatablesu"] = dataTable;
                //string excelname = obj.Branch.ToString().Trim() + "$";
                //string excelConnectionString = Session["excelfilepathSupdebit"].ToString();
                //using (OleDbConnection con = new OleDbConnection(excelConnectionString))
                //{
                //    string query = string.Format("SELECT * FROM [{0}]", excelname);
                //    con.Open();
                //    OleDbDataAdapter adapter = new OleDbDataAdapter(query, con);
                //    adapter.Fill(dataTable);
                //    Session["Tempdatatablesu"] = dataTable;
                //}
                foreach (DataColumn dtColumn in dataTable.Columns)
                {
                    strCols = strCols + dtColumn.ColumnName.Trim() + ":";
                }
                strCols = strCols.Substring(0, strCols.Length - 1);
                strColArr = strCols.Split(':');
                if (strColArr[0].ToString() == "SNo"
                    && strColArr[1].ToString() == "Nature of Expenses"
                    && strColArr[2].ToString() == "Main Category"
                    && strColArr[3].ToString() == "Sub Category"
                    && strColArr[4].ToString() == "Description"
                    && strColArr[5].ToString() == "Function Code"
                    && strColArr[6].ToString() == "Cost Code"
                    && strColArr[7].ToString() == "Product Code"
                    && strColArr[8].ToString() == "OU Code"
                    && strColArr[9].ToString() == "Amount"
                    && strColArr[10].ToString()=="HSN Code"
                    && strColArr.Count().ToString() == "11")
                {
                    mag = "Success";
                }
                else
                {
                    if (strColArr.Count().ToString() == "11")
                    {
                        mag = "Success";
                    }
                    else
                    {
                        mag = "Faild";
                    }
                }
                return Json(mag, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public PartialViewResult _Supplierexcelstatus(string ddlSelectsheet)
        {
            try
            {
                if (Session["Tempdatatablesu"] != null)
                {
                    DataTable errdataval = new DataTable();
                    errdataval = (DataTable)Session["Tempdatatablesu"];
                    datasupload(errdataval);
                    errdataval = (DataTable)Session["Errdatatablesu"];
                    if (errdataval.Rows.Count == 0)
                    {
                        ViewBag.visble = "Yes";
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return PartialView();
        }
        private void datasupload(DataTable dataTable)
        {
            int sno = 0;
            int totalrecord = 0;
            int invalid = 0;
            int valid = 0;
            DataTable maindatatable = new DataTable();
            maindatatable = dataTable;
            totalrecord = maindatatable.Rows.Count;
            DataTable Errdatatable = new DataTable();
            Errdatatable.Columns.Add("SNo");
            Errdatatable.Columns.Add("Line");
            Errdatatable.Columns.Add("Error Description");
            try
            {
                if (dataTable.Rows.Count > 0)
                {
                    string exceltext = "";
                    string exceltextnew = "";
                    string status = "";
                    string errs = "";
                    int RowNo = 0;
                    decimal vales = 0;
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        if (dataTable.Rows[i][0].ToString() != "")
                        {
                            errs = "";
                            RowNo = i + 1;
                            if (dataTable.Rows[i][1].ToString() == "")
                            {
                                errs = "Nature of Expenses Should Not be Empty ";
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][1].ToString();

                                status = objModel.GetStatusexcel(exceltext, "", "", "NatureofExpenses");
                                if (status == "notexists")
                                {
                                    if (errs == "")
                                    {
                                        errs = "Nature of Expenses Was Not Found ";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "Nature of Expenses Was Not Found ";
                                    }
                                }
                            }

                            if (dataTable.Rows[i][2].ToString() == "")
                            {

                                if (errs == "")
                                {
                                    errs = "Main Category Should Not be Empty";
                                }
                                else
                                {
                                    errs = errs + "," + "Main Category Should Not be Empty";
                                }
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][2].ToString();

                                status = objModel.GetStatusexcel(exceltext, "", "", "MainCategorycode");
                                if (status == "notexists")
                                {
                                    if (errs == "")
                                    {
                                        errs = "Main Category Was Not Found ";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "Main Category Was Not Found ";
                                    }
                                }

                            }
                            if (dataTable.Rows[i][3].ToString() == "")
                            {
                                if (errs == "")
                                {
                                    errs = "Sub Category Should Not be Empty";
                                }
                                else
                                {
                                    errs = errs + "," + "Sub Category Should Not be Empty";
                                }
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][3].ToString();
                                status = objModel.GetStatusexcel(exceltext, "", "", "SubCategorycode");
                                if (status == "notexists")
                                {
                                    if (errs == "")
                                    {
                                        errs = "Sub Category Was Not Found ";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "Sub Category Was Not Found ";
                                    }
                                }
                            }
                            if (dataTable.Rows[i][9].ToString() == "")
                            {
                                if (errs == "")
                                {
                                    errs = "Amount Should Not be Empty";
                                }
                                else
                                {
                                    errs = errs + "," + "Amount Should Not be Empty";
                                }
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][9].ToString();

                                vales = Convert.ToDecimal(exceltext.ToString().Trim());
                                if (vales <= 0)
                                {
                                    if (errs == "")
                                    {
                                        errs = "Amount Should Not be Less Then Zero";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "Amount Should Not be Less Then Zero";
                                    }
                                }
                            }
                            if (dataTable.Rows[i][5].ToString() == "")
                            {
                                if (errs == "")
                                {
                                    errs = "Function Code Should Not be Empty";
                                }
                                else
                                {
                                    errs = errs + "," + "Function Code Should Not be Empty";
                                }
                            }
                            else
                            {
                                if (dataTable.Rows[i][8].ToString() != "")
                                {
                                    exceltext = dataTable.Rows[i][5].ToString();
                                    // dataTable.Rows[i][8].ToString() == ""
                                    string exceloucode = dataTable.Rows[i][8].ToString();
                                    string login = objCmnFunctions.GetLoginUserGid().ToString();
                                    status = objModel.GetStatusexcel(exceltext, exceloucode, "", "FunctionCodewithou");
                                    if (status == "notexists")
                                    {
                                        if (errs == "")
                                        {
                                            errs = "Function Code Not match with the OU code  ";
                                        }
                                        else
                                        {
                                            errs = errs + "," + "Function Code Not match with the OU code ";
                                        }
                                    }
                                }
                                else
                                {
                                    errs = "OU Code Was Not Found ";
                                }

                            }
                            if (dataTable.Rows[i][6].ToString() == "")
                            {
                                if (errs == "")
                                {
                                    errs = "Cost Code Should Not be Empty";
                                }
                                else
                                {
                                    errs = errs + "," + "Cost Code Should Not be Empty";
                                }
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][6].ToString();

                                status = objModel.GetStatusexcel(exceltext, "", "", "CostCode");
                                if (status == "notexists")
                                {
                                    if (errs == "")
                                    {
                                        errs = "Cost Code Was Not Found ";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "Cost Code Was Not Found ";
                                    }
                                }
                            }
                            if (dataTable.Rows[i][6].ToString() != "" && dataTable.Rows[i][5].ToString() != "")
                            {
                                status = objModel.selectfcccval(dataTable.Rows[i][6].ToString(), dataTable.Rows[i][5].ToString());
                                if (status == "notexists")
                                {
                                    if (errs == "")
                                    {
                                        errs = "Invalid Function Code and Cost Code";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "Invalid Function Code and Cost Code";
                                    }
                                }
                            }
                            if (dataTable.Rows[i][7].ToString() == "")
                            {
                                if (errs == "")
                                {
                                    errs = "Product Code Should Not be Empty";
                                }
                                else
                                {
                                    errs = errs + "," + "Product Code Should Not be Empty";
                                }
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][7].ToString();

                                status = objModel.GetStatusexcel(exceltext, "", "", "ProductCode");
                                if (status == "notexists")
                                {
                                    if (errs == "")
                                    {
                                        errs = "Product Code Was Not Found ";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "Product Code Was Not Found ";
                                    }
                                }
                            }
                            if (dataTable.Rows[i][8].ToString() == "")
                            {
                                if (errs == "")
                                {
                                    errs = "OU Code Should Not be Empty";
                                }
                                else
                                {
                                    errs = errs + "," + "OU Code Should Not be Empty";
                                }
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][8].ToString();

                                status = objModel.GetStatusexcel(exceltext, "", "", "OUCode");
                                if (status == "notexists")
                                {
                                    if (errs == "")
                                    {
                                        errs = "OU Code Was Not Found ";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "OU Code Was Not Found ";
                                    }
                                }
                            }
                            if (dataTable.Rows[i][10].ToString() == "")
                            {
                                if (errs == "")
                                {
                                    errs = "HSN Code Should Not be Empty";
                                }
                                else
                                {
                                    errs = errs + "," + "HSN Code Should Not be Empty";
                                }
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][10].ToString();

                                status = objModel.GetStatusexcel(exceltext, "", "", "HSNCode");
                                if (status == "notexists")
                                {
                                    if (errs == "")
                                    {
                                        errs = "HSN Code Was Not Found ";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "HSN Code Was Not Found ";
                                    }
                                }
                            }
                            if (errs != "")
                            {
                                sno++;
                                Errdatatable.Rows.Add(sno, RowNo, errs);
                            }
                        }
                    }
                }
                else
                {
                    Errdatatable.Rows.Add(1, "Please  Select Valid Sheet");
                }
                invalid = Errdatatable.Rows.Count;
                valid = totalrecord - invalid;
                ViewBag.vbvalid = "Total No of Vaild Record :" + valid;
                ViewBag.vbinvalid = "Total No of InVaild Record :" + invalid;
                ViewBag.vbtotalrecord = "Total No of Record Excel File :" + totalrecord;

                Session["Errdatatablesu"] = Errdatatable;
                Session["maindatatablesu"] = maindatatable;
            }
            catch (Exception ex)
            {
                sno++;
                Errdatatable.Rows.Add(sno, Errdatatable.Rows.Count + 1, ex.Message.ToString() + " Please ckeck Excel File Error While Reading Data" + ex.ToString());

                invalid = Errdatatable.Rows.Count;
                valid = totalrecord - invalid;
                ViewBag.vbvalid = "Total No of Vaild Record :" + valid;
                ViewBag.vbinvalid = "Total No of InVaild Record :" + invalid;
                ViewBag.vbtotalrecord = "Total No of Record Excel File :" + totalrecord;

                Session["Errdatatablesu"] = Errdatatable;
                Session["maindatatablesu"] = maindatatable;
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
        }
        public JsonResult localdetailssu()
        {
            try
            {
                DataTable dt = (DataTable)Session["maindatatablesu"];
                List<EOW_TravelClaim> objlist = new List<EOW_TravelClaim>();

                string success = objModel.ExcelSupplierPayment(dt, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString());

                return Json(success, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public PartialViewResult _Suppliersearch(string listfor)
        {
            List<EOW_Employeelst> objowner = new List<EOW_Employeelst>();
            EOW_Employeelst Getviewbagdata = new EOW_Employeelst();
            if (listfor == "search")
            {
                if (Session["Searchdatasup"] != null)
                {
                    Getviewbagdata = (EOW_Employeelst)Session["searinvoiceviewbag"];
                    @ViewBag.suppcode = Getviewbagdata.empCode;
                    @ViewBag.suppliername = Getviewbagdata.empName;
                    objowner = (List<EOW_Employeelst>)Session["Searchdatasup"];
                }
            }
            else
            {
                Session["Searchdatasup"] = null;
                Session["searinvoiceviewbag"] = null;
                objowner = objModel.getSupplierdetails();
            }
            return PartialView(objowner);
        }
        [HttpPost]
        public JsonResult SupplierSearchnew(EOW_Employeelst data)
        {
            List<EOW_Employeelst> obj = new List<EOW_Employeelst>();
            EOW_Employeelst searchinvnew = new EOW_Employeelst();
            if (data.empCode != "" || data.empName != "")
            {
                obj = objModel.SelectSupplierSearch(data.empName, data.empCode).ToList();
                searchinvnew.empCode = data.empCode;
                searchinvnew.empName = data.empName;
                @ViewBag.suppcode = data.empCode;
                @ViewBag.suppliername = data.empName;
                Session["searinvoiceviewbag"] = searchinvnew;
                Session["Searchdatasup"] = obj;
            }
            else
            {
                Session["Searchdatasup"] = null;
                Session["searinvoiceviewbag"] = null;
                obj = objModel.getSupplierdetails().ToList();
            }

            return Json(obj, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult _SupplierECFCTCreate(EOW_Supplierinvoice Suppliermodel)
        {
            string Err = "";
            try
            {
                decimal gvamt = Convert.ToDecimal(Session["Supplierecfamountval"].ToString());
                decimal mainecf = Convert.ToDecimal(Session["Mainecfamt"].ToString());

                decimal ecfdebitlinesum = Convert.ToDecimal(objModel.Getbebitlineamt(Session["EcfGid"].ToString(), Session["EcfGid"].ToString()));
                string result = objModel.GetSupplierBankDetailsBypayMode(Session["EcfGid"].ToString());
                if (Err == "")
                {
                    if (result == "No")
                    {
                        Err = "SupBank";
                    }
                }
                if (gvamt != mainecf)
                {
                    Err = "ECF";
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
                    Err = Session["EcfGid"].ToString();
                    Err = objModel.UpdateCentralecffinal(Suppliermodel, objCmnFunctions.GetLoginUserGid().ToString(), Err, Session["QueueGide"].ToString(), Session["centralteamraiser"].ToString());
                    if (Err == "")
                    {
                        Err = "Error";
                    }
                    else
                    {
                        Err = Err.ToString();
                        Session["EcfGid"] = "";
                        Session["QueueGide"] = null;
                        Session["EcfGid"] = null;
                        Session["invoiceGid"] = null;
                        Session["Mainecfamt"] = null;
                        Session["CentralTables"] = null;
                        Session["currentrate"] = null;
                        Session["Supplierdetails"] = null;
                    }
                }
                return Json(Err, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
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
                string pomapping =  (objModel.GetPoMappingExists(Session["EcfGid"].ToString()));
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
                if(pomapping == "N")
                {
                     Err = "Pomapped";
                }

                if (Err == "")
                {
                    Err = Session["EcfGid"].ToString();
                    Err = objModel.UpdateSupplierinvoicefinal(Suppliermodel, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), Session["SupplierGid"].ToString(), objCmnFunctions.GetLoginUserGid().ToString(), "S", Session["QueueGide"].ToString());
                    if (Err == "")
                    {
                        Err = "Error";
                    }
                    else
                    {
                        Err = Err.ToString();
                        Session["EcfGid"] = "";
                        Session["QueueGide"] = null;
                        Session["EcfGid"] = null;
                        Session["invoiceGid"] = null;
                        Session["Mainecfamt"] = null;
                        Session["currentrate"] = null;
                        Session["Supplierdetails"] = null;
                    }
                }
                return Json(Err, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult _EmpECFDraft(EOW_EmployeeeExpense EmployeeeExpenseModel)
        {
            string Err = "";
            try
            {
                Err = "Success";
                Err = objModel.Insertecfdraft(EmployeeeExpenseModel, Session["EcfGid"].ToString());
                if (Err == "")
                {
                    Err = "Error";
                }
                else
                {
                    Err = "Success";
                    Session["QueueGide"] = null;
                    Session["EcfGid"] = null;
                    Session["invoiceGid"] = null;
                    Session["Ecfamountpayment1"] = null;
                    Session["Ecfamountval"] = null;
                    Session["Ecfamountvalmain"] = null;
                }
                ViewBag.SearchItems = null;
                return Json(Err, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        //[HttpGet]
        //public PartialViewResult _PoAdd()
        //{
        //    List<EOW_PO> TypeModel = new List<EOW_PO>();
        //    TypeModel = objModel.GetPoDetails().ToList();
        //    return PartialView(TypeModel.ToList());
        //}
        [HttpPost]
        public JsonResult _PoAdd(EOW_PO addpo)
        {
            string message = "";
            try
            {
                message = objModel.Insertpodetails(addpo, Session["invoiceGid"].ToString());
                ViewBag.SearchItems = null;
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public PartialViewResult _PODetailsGrid(string values, string valuesid)
        {
            Session["addcurpogis"] = null;
            List<EOW_PO> lstnew = new List<EOW_PO>();
            lstnew = objModel.Getpodetailsgrid(values, valuesid, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), "S").ToList();
            return PartialView(lstnew);
        }
        [HttpGet]
        public PartialViewResult _Pomappingdetail(string id)
        {
            try
            {
                EOW_PO Header = new EOW_PO();
                List<EOW_PO> getsingleid = new List<EOW_PO>();
                getsingleid = objModel.GetPoDetailssingledata(id).ToList();
                Header.POGid = id.ToString();
                Header.PONo = getsingleid[0].PONo.ToString();
                Header.POdate = getsingleid[0].POdate.ToString();
                Header.POStatus = getsingleid[0].POStatus.ToString();
                Header.POBalamount = getsingleid[0].POBalamount.ToString();
                //Header.POUtilizedamount = getsingleid[0].POUtilizedamount.ToString();
                Header.POUtilizedamount = objCmnFunctions.GetINRAmount(getsingleid[0].POUtilizedamount.ToString());
                Header.POApprovedStatus = getsingleid[0].POApprovedStatus.ToString();
                ViewBag.poheaderheader = Header;

                List<EOW_PO> TypeModel = new List<EOW_PO>();
                TypeModel = objModel.GetPoDetailsitem(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), id).ToList();
                return PartialView(TypeModel.ToList());
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView();
            }
        }
        [HttpPost]
        public JsonResult SeuplierPODelete(EOW_PO TravelClaimmodel)
        {
            string TravelModeGID = (string)TravelClaimmodel.POGid;
            string delamt = objModel.DeleteSupplidepo(TravelModeGID, Session["invoiceGid"].ToString());
            return Json(TravelClaimmodel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult searchpodetails(EOW_PO data, string potype)
        {
            List<EOW_PO> obj = new List<EOW_PO>();
            EOW_PO emp = new EOW_PO();
            obj = objModel.SelectPOSearch(data.PONo, data.POdate, data.POAmount, Session["SupplierGid"].ToString(), potype).ToList();
            Session["Searchdatasupo"] = obj;
            Session["PONo"] = data.PONo;
            Session["POdate"] = data.POdate;
            Session["POAmount"] = data.POAmount;
            return Json(obj, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public PartialViewResult _PoAdd(string listfor = null, string potype = null)
        {
            List<EOW_PO> objowner = new List<EOW_PO>();
            if (listfor == "search")
            {
                if (Session["Searchdatasupo"] != null)
                {
                    objowner = (List<EOW_PO>)Session["Searchdatasupo"];
                }
            }
            else
            {
                Session["PONo"] = "";
                Session["POdate"] = "";
                Session["POAmount"] = "";
                objowner = objModel.GetPoDetails(Session["SupplierGid"].ToString(), potype).ToList();
            }
            return PartialView(objowner.ToList());
        }
        [HttpPost]
        public JsonResult Updatepomapp(EOW_PO objMExpenseEdit)
        {
            string data = objModel.Insertitemdetails(objMExpenseEdit, Session["invoiceGid"].ToString());
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult _Emppolicypopup(string id)
        {
            Session["policyhelp"] = null;
            if (Session["policyhelp"] == null)
            {
                string policy = "";
                policy = objModel.selectpolicy(id.ToString());
                Session["policyhelp"] = policy;
            }
            List<EOW_RefNo> lst = new List<EOW_RefNo>();
            //lst = objModel.EmployeeePaymentRefNodatagri(Session["SelfModeRaiseid"].ToString()).ToList();
            return PartialView(lst);
        }
        [HttpGet]
        public PartialViewResult _Lastthrrmonth()
        {
            List<EOW_SupplierModelgrid> obj = new List<EOW_SupplierModelgrid>();
            obj = objModel.Lastthrrmonth(Session["SupplierGid"].ToString()).ToList();
            return PartialView(obj);
        }
        [HttpPost]
        public JsonResult invoicedatevat(EOW_SupplierModelgrid valatatedate)
        {
            string results = "Sucess";
            try
            {
                string invoicdate = valatatedate.InvoiceDate;
                int nodays = 0;
                string datas = objModel.GetStatusexcel(objCmnFunctions.convertoDateTimeString(invoicdate), "", "", "Invoicedate");
                nodays = Convert.ToInt32(datas);
                if (nodays > 90)
                {
                    results = "Deviation";
                }
                else
                {
                    results = "Sucess";
                }
            }
            catch (Exception ex)
            {

            }
            return Json(results, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AutoCompletecodes(string term)
        {
            string Err = "";
            List<CentraldataModel> objparenttax = new List<CentraldataModel>();
            objparenttax = objModel.SelectSupplierSearchcode(term).ToList();
            return Json(objparenttax, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AutoCompletenames(string term)
        {
            string Err = "";
            List<CentraldataModel> objparenttax = new List<CentraldataModel>();
            objparenttax = objModel.SelectSupplierSearchname(term).ToList();
            return Json(objparenttax, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult _Addsupplierinvoice()
        {
            return PartialView();
        }
        [HttpGet]
        public ActionResult PoEditDetails(poraiser objraiser, string viewfor, string id)
        {
            fb_DataModelpr objModelfb = new fb_DataModelpr();
            //Session["ShipTemp"] = null;
            Session["viewfor"] = null;
            if (viewfor == "edit")
            {
                Session["viewfor"] = "edit";
                // ViewBag.viewfor = "edit";
            }
            else if (viewfor == "view")
            {
                Session["viewfor"] = "view";
                //ViewBag.viewfor = "view";
            }
            else if (viewfor == "cancel")
            {
                Session["viewfor"] = "cancel";
                //ViewBag.viewfor = "cancel";
            }
            else if (viewfor == "delete")
            {
                Session["viewfor"] = "delete";
                //ViewBag.viewfor = "delete";
            }
            else if (viewfor == "checkercancel")
            {
                Session["viewfor"] = "checkercancel";
            }
            else if (viewfor == "checker")
            {
                Session["viewfor"] = "checker";
            }
            else if (viewfor == "closure")
            {
                Session["viewfor"] = "closure";
            }
            else if (viewfor == "closurechecker")
            {
                Session["viewfor"] = "closurechecker";
            }
            else if (viewfor == "amend")
            {
                Session["viewfor"] = "amend";
            }
            else if (viewfor == "delmat")
            {
                Session["viewfor"] = "delmat";
            }

            poraiser obj = new poraiser();
            shipment objship = new shipment();
            DataTable objtable = new DataTable();
            DataTable objTable = objModelfb.GetPoDetails(Convert.ToInt32(id));
            Session["podetail"] = objTable;
            DataTable dt = new DataTable();
            for (int i = 0; i < objTable.Rows.Count; i++)
            {
                dt = objModelfb.GetShipTable(Convert.ToInt32(objTable.Rows[i]["podetails_gid"].ToString()));
            }
            Session["totalship"] = dt;
            Session["shipdelete"] = dt;
            //objship.shiplist = objModel.GetShipmentDetails(dt);
            obj.objlist = objModelfb.GetPoDetailsList(objTable);
            obj.podetGid = obj.objlist[0].podetGid;
            obj.poheadGid = obj.objlist[0].poheadGid;
            obj.poCancelGid = objraiser.poCancelGid;
            obj.remarks = objraiser.remarks;
            obj.poClosureGid = objraiser.poClosureGid;
            obj.porefno = obj.objlist[0].porefno;
            obj.podate = obj.objlist[0].podate;
            obj.poenddate = obj.objlist[0].poenddate;
            obj.cbfheadAmount = obj.objlist[0].cbfheadAmount;
            obj.devamount = obj.objlist[0].devamount;
            obj.cbfdetailsQty = obj.objlist[0].cbfdetailsQty;
            obj.projmanagergid = obj.objlist[0].projmanagergid;
            obj.vendorNote = obj.objlist[0].vendorNote;
            obj.vendorgid = obj.objlist[0].vendorgid;
            obj.tempName = obj.objlist[0].tempName;
            obj.templname = obj.objlist[0].templname;
            obj.additionTemplate = obj.objlist[0].additionTemplate;
            obj.itType = obj.objlist[0].itType;
            obj.department = obj.objlist[0].department;

            obj.vendorName = obj.objlist[0].vendorName;
            obj.templateGid = obj.objlist[0].templateGid;
            obj.templateList = new SelectList(objModelfb.GetTemplateList(), "templateGid", "tempName", obj.templateGid);
            obj.projmanagerList = new SelectList(objModelfb.GetProjectOwnerList(), "projmanagergid", "projmanagername", obj.projmanagergid);
            obj.raisedby = objModelfb.GetLoginUser();
            //if (Session["podelete"] != null)
            //    obj.count = (int)Session["podelete"];
            Session["polist"] = obj;
            Session["Eowtabclose"] = "Eowtabclose";
            return RedirectToAction("PoEdit", "PoRaiserEdit", new { area = "FLEXIBUY" });
        }
        [HttpGet]
        public ActionResult viewsupplierDetails(string supid, string pagefor)
        {
            Session["EOWsupplierview"] = "EOWsupplierview";
            string tmpsupid = objModel.GetSupplierTmpGid(supid);
            return RedirectToAction("Index", "Onboarding", new { supid = tmpsupid, pagefor = "5", area = "ASMS" });
        }

        [HttpPost]
        public JsonResult GetAssetSubCat(EOW_TravelClaim objAssetCat)
        {
            try
            {
                var AssetCatId = objAssetCat.AssetCatId;
                return Json(objModel.GetAssetSubCategoryList(AssetCatId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                List<EOW_TravelClaim> lst = new List<EOW_TravelClaim>();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetTaxSubCat(sinvotax objTaxId)
        {
            try
            {
                var TaxId = objTaxId.TaxgId;
                return Json(objModel.GetTaxSubTypeList(TaxId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                List<EOW_TravelClaim> lst = new List<EOW_TravelClaim>();
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult _suppliserdebitclear(EOW_SupplierModelgrid Suppliermodel)
        {
            string Err = "";
            string ecfno = "";
            try
            {
                ecfno = Session["EcfGid"].ToString();
                Err = Session["invoiceGid"].ToString();
                string insrtinvoice = objModel.DeleteSupplierdebitclear(Err, ecfno);
                if (insrtinvoice == "Success")
                {
                    Err = "Success";
                }
                return Json(Err, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public PartialViewResult _supplierGridDetails()
        {
            List<EOW_SupplierModelgrid> lst = new List<EOW_SupplierModelgrid>();
            //lst = objModel.GetSupplierdata(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), "S").ToList();
            return PartialView(lst);
        }
        [HttpGet]
        public PartialViewResult _grnheraderdetails(EOW_PO POobj, string POGid, string POdetlGid, string POserprogid, string POrate, string POserprocode, string POserprodesc)
        {
            try
            {
                EOW_PO objMpodetals = new EOW_PO();
                objMpodetals.POGid = POGid;
                objMpodetals.POdetlGid = POdetlGid;
                objMpodetals.POserprogid = POserprogid;
                objMpodetals.POrate = objCmnFunctions.GetINRAmount(POrate);
                objMpodetals.POserprocode = POserprocode;
                objMpodetals.POserprodesc = POserprodesc;
                ViewBag.currentgrnheader = objMpodetals;
                return PartialView();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView();
            }
        }
        [HttpPost]
        public JsonResult Getgrninwrddetview(EOW_EmployeeeExpense EmployeeeExpense)
        {
            try
            {
                List<EOW_PO> lstDashBoard = new List<EOW_PO>();
                lstDashBoard = objModel.grninwrddetview(EmployeeeExpense.ExpenseCategoryId, EmployeeeExpense.Doctypeid, Session["invoiceGid"].ToString()).ToList();
                var jsonResult = Json(new { qrylist = lstDashBoard }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = int.MaxValue;
                return jsonResult;
                //return Json(objModel.grninwrddetview(EmployeeeExpense.ExpenseCategoryId, Session["invoiceGid"].ToString()), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult grndetailssubmit(string[] SelectedValues, EOW_PO grnsavedata)
        {
            //string resultdata = "Success";
            try
            {
                string data = objModel.Insertitemdetailsnew(grnsavedata, Session["invoiceGid"].ToString(), SelectedValues);
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult CheckGstDtls()
        {
            string Data1 = "";
            try
            {
                DataSet ds = db.CheckGstInvdels(Session["invoiceGid"].ToString());
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(Data1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult checkhsnfordebitline()
        {
            string Data1 = "";
            try
            {
                DataSet ds = db.checkdebithsn(Session["invoiceGid"].ToString());
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(Data1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public JsonResult GetPoGstDetails(string Poid)
        {
            string Data1 = "";
            try
            {
                DataSet ds = db.GetpoGstDetails(Poid, Session["invoiceGid"].ToString());
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(Data1, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult CheckWithCBF(string ecfgid = "")
        {
            string result = "0";
            try
            {
                //  int ecf_gid = Convert.ToInt32(string.IsNullOrEmpty(ecfgid)?"0":ecfgid);
                int ecf_gid = Convert.ToInt32(Session["EcfGid"].ToString());
                result = objModel.CheckWithCBF(ecf_gid);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult SetCessTaxDetails(InvoiceTax iTax)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.SetCessTaxDetails(iTax.action, iTax.InvoiceTaxgid, iTax.InvId, iTax.TaxId, iTax.SubTaxId, iTax.TaxRate, iTax.TaxableAmt, iTax.TaxAmount, plib.LoginUserId);

                DataTable dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }


                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        //Ramya
        [HttpPost]
        public JsonResult GetHSNByAssetSubCategory(EOW_EmployeeeExpense EmployeeeExpense)
        {
            try
            {
                string Data2 = "";
                DataSet ds = db.GetAssetHsn(EmployeeeExpense.ExpenseCategoryId.ToString());
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                return Json(new {  Data2 }, JsonRequestBehavior.AllowGet); 
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public FileResult DownloadDebitlineTemplate()
        {
            //get the temp folder and file path in server
            string fullPath = System.Configuration.ConfigurationManager.AppSettings["Debitline"];

            //return the file for download, this is an Excel 
            //so I set the file content type to "application/vnd.ms-excel"
            return File(fullPath, "application/vnd.ms-excel", "DebitLineTemplate.xls");
        }

        #region GST Phase3 Ramya
        
        public ActionResult _CygnetSearch_Supplier()
        {
            List<CygnetSearchModel> obj = new List<CygnetSearchModel>();
            CygnetSearchModel invsearch = new CygnetSearchModel();
            invsearch.Cygnet_SupplierPanNo = "";
            obj = ObjCygnet.SelectInvoiceSearch(invsearch).ToList();
            return PartialView(obj);
        }

        public PartialViewResult CygnetGrid(string panno = "", string suppliername = "", string GSNTNNo = "", string InvoiceNo = "", string FromDate = "", string ToDate = "")
        {
            List<CygnetSearchModel> objowner = new List<CygnetSearchModel>();

            if (Session["SearchItems"] != null)
            {
                objowner = (List<CygnetSearchModel>)TempData["SearchItems"];
            } 
            else
            {              
                CygnetSearchModel invsearch = new CygnetSearchModel();
                invsearch.Cygnet_SupplierPanNo = panno;
                invsearch.Cygnet_SupplierName = suppliername;
                invsearch.Cygnet_Supplier_GSTNNo = GSNTNNo;
                invsearch.Cygnet_InvoiceNo = InvoiceNo;
                invsearch.Cygnet_InvoiceFromDate = FromDate;
                invsearch.Cygnet_InvoiceToDate = ToDate;
                objowner = ObjCygnet.SelectInvoiceSearch(invsearch).ToList();
                TempData["SearchItems"] = objowner;
            }
            return PartialView(objowner.ToList());
        }
        
        [HttpPost]
        public JsonResult CreateECFInvoice(string Cygnet_Gids)
        {
            DataSet ds = new DataSet();
            DataTable dtECF = new DataTable();
            try
            {
                string Data0 = "";
                ds = ObjCygnet.InsertECFInvoiceFromCygnet(Cygnet_Gids, plib.LoginUserId, string.IsNullOrEmpty(Session["EcfGid"].ToString()) ? "0" : Session["EcfGid"].ToString());
                if (ds != null)
                {
                    dt = ds.Tables[0];  
                    if (dt.Rows.Count > 0) { Data0 = JsonConvert.SerializeObject(dt); }
                    Session["EcfGid"] = dt.Rows[0]["ecf_gid"].ToString(); 
                    return Json(new { Data0 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }
        [HttpPost]
        public JsonResult SupplierSearchPO11(string PONo = "", string pototal = "")
        {
            try
            {
                List<CentraldataModel> recordsearch = new List<CentraldataModel>();
                CentraldataModel emp = new CentraldataModel();
                if (pototal != "" || PONo != "")
                {
                    @ViewBag.EmployeeName = pototal;
                    @ViewBag.EmployeeCode = PONo;
                    emp.RaiserCode = PONo;
                    emp.RaiserName = pototal;
                    Session["SerchViewbag"] = emp;
                    recordsearch = objCModel.SelectPodetails(pototal, PONo, int.Parse(Session["posearchid"].ToString()), Convert.ToString(Session["claimType"])).ToList();
                    Session["SearchEmployeedata"] = recordsearch;
                }
                else
                {
                    emp.RaiserCode = PONo;
                    emp.RaiserName = pototal;
                    Session["SerchViewbag"] = emp;
                    recordsearch = objCModel.SelectPodetails(pototal, PONo, int.Parse(Session["posearchid"].ToString()), Convert.ToString(Session["claimType"])).ToList();
                    Session["SearchEmployeedata"] = recordsearch;
                }

                return Json(recordsearch, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult POSearch(string id, string claimType)
        {
            try
            {
                string listfor = "search";
                List<CentraldataModel> obj = new List<CentraldataModel>();
                CentraldataModel Getvaluesearchvalue = new CentraldataModel();
                if (listfor == "search")
                {
                    if (Session["SearchEmployeedata"] != null)
                    {
                        Getvaluesearchvalue = (CentraldataModel)Session["SerchViewbag"];
                        @ViewBag.EmployeeName = Getvaluesearchvalue.pototal;
                        @ViewBag.EmployeeCode = Getvaluesearchvalue.PONo;
                        obj = (List<CentraldataModel>)Session["SearchEmployeedata"];
                    }
                }
                else
                {
                    Session["SearchEmployeedata"] = null;
                    Session["SerchViewbag"] = null;
                    if (!string.IsNullOrEmpty(claimType))
                    {
                        Session["claimType"] = claimType;
                    }
                    else
                    {
                        claimType = "";
                    }
                    obj = objCModel.SelectPodetailswithoutarg(int.Parse(Session["posearchid"].ToString()), claimType).ToList();
                }
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult InvoiceMismatch(string Cygnet_Gids)
        {
            DataSet ds = new DataSet();
            DataTable dtECF = new DataTable();
            try
            {
                string Data0 = "";
                ds = ObjCygnet.SetInvoiceMismatch(Cygnet_Gids, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data0 = JsonConvert.SerializeObject(dt); }
                    return Json(new { Data0 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}
