//Project Name       :  IEM
//Module Name        :  EOW-Employee on Web
//File Name          :  IEM\EOW\EmployeeClaimNewPettyController\Index
//Last Modified Date :  26-08-2015
//Purpose            :  
//Description        :  
//Created On         :  01-06-2015
//Created By         :  Flexicode India Pvt Ltd - Thirumalai
//Language Used      :  CSharp
//Database           :  ficc_iem
//Table's Used       :  
//Views Used         :  
//Procedures Used    :  
//Function Used      :
//Report Called      :  
//To Do              :  
//Known Issues       :  
//History:
//Date & Time                   Task                       By                          Type
//********************************************************************************************
//01-06-2015                   Design Started          Thirumalai                      New
//01-06-2015                   Coding Started          Thirumalai                      New

using IEM.Areas.EOW.Models;
using IEM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using IEM.Areas.MASTERS.Controllers;
using IEM.Helper;
using IEM.Areas.Dashboard.Models;
using Rotativa.Options;
using System.Data;
using Newtonsoft.Json;
using IEM.App_Start;
namespace IEM.Areas.EOW.Controllers
{

    [NoDirectAccess]
    public class EmployeeClaimNewPettyController : Controller
    {
        //
        // GET: /EOW/EmployeeClaimNewPetty/  
        ErrorLog objErrorLog = new ErrorLog();
        private EOW_IRepository objModel;
        CmnFunctions objCmnFunctions = new CmnFunctions();
        private FileServer Cmnfs = new FileServer();
        CommonIUD objCommonIUD = new CommonIUD();
        LocalConveyanceNewController locals = new LocalConveyanceNewController();
        proLib plib = new proLib();
        dbLib db = new dbLib();
        public EmployeeClaimNewPettyController()
            : this(new EOW_DataModel())
        {

        }
        public EmployeeClaimNewPettyController(EOW_IRepository objM)
        {
            objModel = objM;
        }
        public ActionResult Index()
        {
            try
            {
                Session["QueueGide"] = "";
                if (Session["DashBoard"] != null)
                {
                    EOW_EmployeeeExpense eowemp = new EOW_EmployeeeExpense();
                    eowemp = (EOW_EmployeeeExpense)Session["DashBoard"];
                    if (eowemp.ecfremark != null)
                    {
                        ViewBag.ecfrmarks = eowemp.ecfremark;
                    }
                    Session["EcfGid"] = eowemp.ecf_GID.ToString();
                    Session["invoiceGid"] = eowemp.invoice_GID.ToString();
                    if (eowemp.Queue_GID != 0)
                    {
                        Session["QueueGide"] = eowemp.Queue_GID.ToString();
                    }
                    else
                    {
                        Session["QueueGide"] = "";
                    }
                    ViewBag.process = "Postdata";
                    ViewBag.message = "Postdata";

                    Session["Ecfamountvalmain"] = eowemp.ECF_Amount;
                    eowemp.ECF_Amount = objCmnFunctions.GetINRAmount(eowemp.ECF_Amount);
                    ViewBag.EOW_EmployeeeExpenseheader = eowemp;
                    Session["DashBoard"] = null;

                    if (Session["loginraisemode"] != null)
                    {
                        if (eowemp.raisermodeId == "P")
                        {
                            Session["SelfModeRaiseid"] = objCmnFunctions.GetLoginProxyUserGid().ToString();
                            Session["SelfModeecfval"] = objCmnFunctions.GetLoginProxyUserGid().ToString();
                        }
                        else
                        {
                            Session["SelfModeRaiseid"] = objCmnFunctions.GetLoginUserGid().ToString();
                            Session["SelfModeecfval"] = objCmnFunctions.GetLoginUserGid().ToString();
                        }
                        List<EOW_EmployeeeExpense> objmaindetaild = new List<EOW_EmployeeeExpense>();
                        objmaindetaild = objModel.SelectEmployeeeheader(objCmnFunctions.GetLoginProxyUserGid().ToString()).ToList();
                        Session["raiserfcc"] = objmaindetaild[0].Exp_FC.ToString();
                        Session["raiserccc"] = objmaindetaild[0].Exp_CC.ToString();
                    }
                    else
                    {
                        Session["SelfModeRaiseid"] = objCmnFunctions.GetLoginUserGid().ToString();
                        Session["SelfModeecfval"] = objCmnFunctions.GetLoginUserGid().ToString();

                        List<EOW_EmployeeeExpense> objmaindetaild = new List<EOW_EmployeeeExpense>();
                        objmaindetaild = objModel.SelectEmployeeeheader(objCmnFunctions.GetLoginUserGid().ToString()).ToList();
                        Session["raiserfcc"] = objmaindetaild[0].Exp_FC.ToString();
                        Session["raiserccc"] = objmaindetaild[0].Exp_CC.ToString();
                    }
                }
                else
                {
                    List<EOW_EmployeeeExpense> objmaindetail = new List<EOW_EmployeeeExpense>();
                    EOW_EmployeeeExpense eowemp = new EOW_EmployeeeExpense();

                    if (Session["loginraisemode"] != null)
                    {
                        Session["SelfModeecfval"] = objCmnFunctions.GetLoginProxyUserGid().ToString();
                        objmaindetail = objModel.SelectEmployeeeheader(objCmnFunctions.GetLoginProxyUserGid().ToString()).ToList();

                        eowemp.Grade = objmaindetail[0].Grade.ToString();
                        eowemp.Raiser_Code = objmaindetail[0].Raiser_Code.ToString();
                        eowemp.Raiser_Name = objmaindetail[0].Raiser_Name.ToString();

                        eowemp.raisermodeId = "P";
                        List<EOW_EmployeeeExpense> objmaindetaild = new List<EOW_EmployeeeExpense>();
                        objmaindetaild = objModel.SelectEmployeeeheader(objCmnFunctions.GetLoginProxyUserGid().ToString()).ToList();
                        Session["raiserfcc"] = objmaindetaild[0].Exp_FC.ToString();
                        Session["raiserccc"] = objmaindetaild[0].Exp_CC.ToString();
                    }
                    else
                    {
                        Session["SelfModeecfval"] = objCmnFunctions.GetLoginUserGid().ToString();
                        objmaindetail = objModel.SelectEmployeeeheader(objCmnFunctions.GetLoginUserGid().ToString()).ToList();

                        eowemp.Grade = objmaindetail[0].Grade.ToString();
                        eowemp.Raiser_Code = objmaindetail[0].Raiser_Code.ToString();
                        eowemp.Raiser_Name = objmaindetail[0].Raiser_Name.ToString();

                        eowemp.raisermodeId = "S";//"C OR S OR P" 
                        List<EOW_EmployeeeExpense> objmaindetaild = new List<EOW_EmployeeeExpense>();
                        objmaindetaild = objModel.SelectEmployeeeheader(objCmnFunctions.GetLoginUserGid().ToString()).ToList();
                        Session["raiserfcc"] = objmaindetaild[0].Exp_FC.ToString();
                        Session["raiserccc"] = objmaindetaild[0].Exp_CC.ToString();
                    }

                    eowemp.Raiser_Modedata = new SelectList(objModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", eowemp.raisermodeId);
                    ViewBag.EOW_EmployeeeExpenseheader = eowemp;
                    ViewBag.process = "";
                    Session["EcfGid"] = null;
                    Session["invoiceGid"] = null;
                    Session["Ecfamountpayment1"] = null;
                    Session["Ecfamountval"] = null;
                }
                Session["Ecfdeclaratonnote"] = objModel.GetDecnote("1", "S").ToString();
                return View();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
        }
        [HttpPost]
        public ActionResult Index(EOW_EmployeeeExpense obj)
        {
            try
            {
                string ecfnewamt = obj.ECF_Amount;
                obj.ECF_Amount = objCmnFunctions.GetRemovecommas(obj.ECF_Amount);
                string clmmonth = "";
                string message = "";
                string insrtinvoice = "";
                List<EOW_EmployeeeExpense> objmaindetail = new List<EOW_EmployeeeExpense>();
                EOW_EmployeeeExpense eowemp = new EOW_EmployeeeExpense();
                if (obj.ecfremark != null)
                {
                    ViewBag.ecfrmarks = obj.ecfremark;
                }
                if (Session["loginraisemode"] != null)
                {
                    obj.raisermodeId = "P";
                    objmaindetail = objModel.SelectEmployeeeheader(objCmnFunctions.GetLoginProxyUserGid().ToString()).ToList();
                    obj.Grade = objmaindetail[0].Grade.ToString();
                    obj.Raiser_Code = objmaindetail[0].Raiser_Code.ToString();
                    obj.Raiser_Name = objmaindetail[0].Raiser_Name.ToString();
                }
                else
                {
                    obj.raisermodeId = "S";//"C OR S OR P" 
                    objmaindetail = objModel.SelectEmployeeeheader(objCmnFunctions.GetLoginUserGid().ToString()).ToList();
                    obj.Grade = objmaindetail[0].Grade.ToString();
                    obj.Raiser_Code = objmaindetail[0].Raiser_Code.ToString();
                    obj.Raiser_Name = objmaindetail[0].Raiser_Name.ToString();
                }
                Session["Ecfamountvalmain"] = obj.ECF_Amount;
                obj.Raiser_Modedata = new SelectList(objModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", obj.raisermodeId);
                clmmonth = obj.ClaimMonth;
                int nodays = 0;
                string result = objModel.GetStatusexcel("", Session["SelfModeecfval"].ToString(), "", "Ecfraiser");
                nodays = Convert.ToInt32(result);
                if (nodays < 15)
                {
                    ViewBag.process = "Postdataerr";
                    message = "Employee Can't Raise ECF";
                }
                else
                {
                    string resultpetty = objModel.GetStatuspetty(obj.ECF_Amount, objCmnFunctions.GetLoginUserGid().ToString());

                    if (resultpetty == "Sucess")
                    {
                        obj.ClaimMonth = locals.getconverttomonthtodate(obj.ClaimMonth);

                        //if (Session["EcfGid"] != null && Session["invoiceGid"] != null)
                        if (Session["EcfGid"] != null)
                        {
                            message = objModel.InsertEmployeeeBasicupdate(obj, objCmnFunctions.GetLoginUserGid().ToString(), Session["SelfModeRaiseid"].ToString(), Session["EcfGid"].ToString());
                            if (message != "")
                            {
                                ViewBag.process = "Postdata";
                                if (obj.raisermodeId == "P")
                                {
                                    if (Session["loginraisemode"] != null)
                                    {
                                        Session["SelfModeRaiseid"] = objCmnFunctions.GetLoginProxyUserGid().ToString();
                                    }
                                    else
                                    {
                                        Session["SelfModeRaiseid"] = objCmnFunctions.GetLoginUserGid().ToString();
                                    }
                                }
                                else
                                {
                                    Session["SelfModeRaiseid"] = objCmnFunctions.GetLoginUserGid().ToString();
                                }
                                insrtinvoice = objModel.InsertEmployeeeBasicinvoiceupdate(obj, Session["SelfModeRaiseid"].ToString(), Session["EcfGid"].ToString());
                                Session["invoiceGid"] = insrtinvoice.ToString();
                                string IFSCCode = "";
                                ViewBag.IFSCCodemsg = "";
                                IFSCCode = objModel.InsertEmployeeePaymentbasicupdatepetty(Session["SelfModeRaiseid"].ToString(), Session["EcfGid"].ToString(),  Session["Ecfamountvalmain"].ToString());
                                if (IFSCCode.ToString().Trim().Equals(""))
                                {
                                    ViewBag.IFSCCodemsg = "IFSC Code is not updated!!! Your Claim may get REJECT by EPU Team!";
                                } 
                            }
                        }
                        else
                        {
                            message = objModel.InsertEmployeeeBasic(obj, objCmnFunctions.GetLoginUserGid().ToString(), "P");
                            if (message != "")
                            {
                                ViewBag.process = "Postdata";
                                if (obj.raisermodeId == "P")
                                {
                                    if (Session["loginraisemode"] != null)
                                    {
                                        Session["SelfModeRaiseid"] = objCmnFunctions.GetLoginProxyUserGid().ToString();
                                    }
                                    else
                                    {
                                        Session["SelfModeRaiseid"] = objCmnFunctions.GetLoginUserGid().ToString();
                                    }
                                }
                                else
                                {
                                    Session["SelfModeRaiseid"] = objCmnFunctions.GetLoginUserGid().ToString();
                                }
                                Session["EcfGid"] = objModel.selectecfgidBasic(obj, objCmnFunctions.GetLoginUserGid().ToString());
                                insrtinvoice = objModel.InsertEmployeeeBasicinvoice(obj, Session["SelfModeRaiseid"].ToString(), Session["EcfGid"].ToString(), "E");
                                Session["invoiceGid"] = objModel.selectinvoiceidBasic(obj, Session["SelfModeRaiseid"].ToString(), Session["EcfGid"].ToString(), "E");
                                string IFSCCode = "";
                                ViewBag.IFSCCodemsg = "";
                                IFSCCode = objModel.InsertEmployeeePaymentbasic(Session["SelfModeRaiseid"].ToString(), Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), Session["Ecfamountvalmain"].ToString());
                                if (IFSCCode.ToString().Trim().Equals(""))
                                {
                                    ViewBag.IFSCCodemsg = "IFSC Code is not updated!!! Your Claim may get REJECT by EPU Team!";
                                } 
                            }
                        }
                    }
                    else
                    {
                        ViewBag.process = "Postdataerr";
                        message = resultpetty.ToString();
                    }
                }
                ViewBag.message = message;
                obj.ClaimMonth = clmmonth;
                obj.ECF_Amount = ecfnewamt;
                ViewBag.EOW_EmployeeeExpenseheader = obj;
                return View();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
        }
        [HttpGet]
        public PartialViewResult _EmpExpenseCreate()
        {
            try
            {
                EOW_EmployeeeExpense objMExpense = new EOW_EmployeeeExpense();
                List<EOW_EmployeeeExpense> objobjMExpense = new List<EOW_EmployeeeExpense>();
                objobjMExpense = objModel.SelectEmployeeeBasic(Session["SelfModeRaiseid"].ToString()).ToList();
                objMExpense.Exp_OUCode = objobjMExpense[0].Exp_OUCode.ToString();
                objMExpense.Exp_ProductCode = objobjMExpense[0].Exp_ProductCode.ToString();
                //objMExpense.Exp_FC = objobjMExpense[0].Exp_FC.ToString();
                //objMExpense.Exp_CC = objobjMExpense[0].Exp_CC.ToString();
                objMExpense.Exp_FCC = new SelectList(objModel.fcdata().ToList(), "raiserfcId", "raiserfcName", Session["raiserfcc"].ToString());
                objMExpense.Exp_CCC = new SelectList(objModel.ccdata().ToList(), "raiserccId", "raiserccName", Session["raiserccc"].ToString());
                objMExpense.Exp_FCCC = objobjMExpense[0].Exp_FCCC.ToString();

                objMExpense.TravelModedata = new SelectList(objModel.tTravelModedata().ToList(), "TravelModeId", "TravelModeName");
                objMExpense.HsncodeList = new SelectList(objModel.HsnCodeList(objobjMExpense[0].SubCategoryId).ToList(), "hsnid", "hsncode", Convert.ToString(objobjMExpense[0].Hsnid));
                objMExpense.HsnDescription = objobjMExpense[0].HsnDescription;
                //objMExpense.RCMFlag = objobjMExpense[0].RCMFlag;
                objMExpense.ExpNatureofExpdata = new SelectList(objModel.NatureofExpensesdataother().ToList(), "NatureofExpensesId", "NatureofExpensesName");
                return PartialView(objMExpense);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView();
            }

        }
        [HttpGet]
        public PartialViewResult _EmpPaymentCreate()
        {
            try
            {
                EOW_Payment objMPayment = new EOW_Payment();
                objMPayment.PaymentModedata = new SelectList(objModel.PaymentModedata().ToList(), "PaymentModeId", "PaymentModeName");
                objMPayment.Beneficiary = objModel.GetEmployeeeGID(Session["SelfModeRaiseid"].ToString());
                return PartialView(objMPayment);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView();
            }

        }
        [HttpGet]
        public PartialViewResult _EmpPaymentmodepopup()
        {
            try
            {
                List<EOW_RefNo> lst = new List<EOW_RefNo>();
                lst = objModel.EmployeeePaymentRefNodatagri(Session["SelfModeRaiseid"].ToString(), "P").ToList();
                return PartialView(lst);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView();
            }

        }
        [HttpGet]
        public PartialViewResult _Emppolicypopup(string id)
        {
            try
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
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView();
            }

        }
        [HttpGet]
        public PartialViewResult _EmpAttachmentCreate()
        {
            try
            {
                EOW_File objMAttachment = new EOW_File();
                objMAttachment.AttachmentTypeData = new SelectList(objModel.AttachmentTypedata().ToList(), "AttachmentTypeId", "AttachmentTypeName");
                return PartialView(objMAttachment);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView();
            }

        }

        [HttpPost]
        public JsonResult _EmpExpenseCreate(EOW_EmployeeeExpense EmployeeeExpenseModel)
        {
            string msg = "";
            try
            {
                if (ModelState.IsValid)
                {
                    msg = objModel.InsertEmployeeeExpensenew(EmployeeeExpenseModel, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), Session["SelfModeRaiseid"].ToString());
                }
                ViewBag.SearchItems = null;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred...", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult _EmpPaymentCreate(EOW_Payment EmployeeeExpenseModel)
        {
            string message = "";
            try
            {
                if (ModelState.IsValid)
                {
                    message = objModel.InsertEmployeeePaymentpetty(EmployeeeExpenseModel, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), Session["Ecfamountpayment1"].ToString());
                    //Session["Ecfamountpayment"] = Convert.ToInt32(Session["Ecfamountpayment"].ToString()) + Convert.ToInt32(EmployeeeExpenseModel.EmployeeePayment_Amount.ToString());
                }
                ViewBag.SearchItems = null;
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred...", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public PartialViewResult _EmpExpenseDetails()
        {
            List<EOW_EmployeeeExpense> lst = new List<EOW_EmployeeeExpense>();
            //lst = objModel.GetEmployeeeExpense(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString()).ToList();
            return PartialView(lst);
        }
        [HttpGet]
        public PartialViewResult _EmpPaymentDetails()
        {
            List<EOW_Payment> lstPayment = new List<EOW_Payment>();
            //lstPayment = objModel.GetEmployeeePayment(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString()).ToList();
            if (Session["Ecfamountpayment1"] == null)
            {
                Session["Ecfamountpayment1"] = lstPayment[0].PaymentAmount.ToString();
            }
            return PartialView(lstPayment);
        }
        [HttpGet]
        public PartialViewResult _EmpAttachmentDetails()
        {
            List<EOW_File> lstAttachment = new List<EOW_File>();
            //lstAttachment = objModel.GetEmployeeeAttachment(Session["EcfGid"].ToString(), objCmnFunctions.GetLoginUserGid().ToString()).ToList();
            return PartialView(lstAttachment);
        }
        [HttpPost]
        public JsonResult GetExpenseCategory(EOW_EmployeeeExpense EmployeeeExpense)
        {
            return Json(objModel.ExpenseCategorydata(EmployeeeExpense.NatureofExpensesId), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetSubCategory(EOW_EmployeeeExpense EmployeeeExpense)
        {
            return Json(objModel.SubCategorydata(EmployeeeExpense.ExpenseCategoryId), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetSubCategorypolicy(EOW_EmployeeeExpense EmployeeeExpense)
        {
            string policy = "";
            var Data2 = "";
            policy = objModel.selectpolicy(EmployeeeExpense.ExpenseCategoryId.ToString());
            Session["policyhelp"] = policy;
            DataSet ds = db.GetExpenseHsn(EmployeeeExpense.ExpenseCategoryId.ToString());
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
            return Json(new { policy, Data2 }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult _Emplistadd(EOW_Employeelst Employeelst)
        {
            Session["EmpExpenseactiverow"] = Employeelst.empName.ToString();
            string message = "";
            message = "Success";
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult _EmpExpenseEdit(int id, string viewfor)
        {
            EOW_EmployeeeExpense objMExpenseEdit = new EOW_EmployeeeExpense();
            try
            {
                string dd = ViewBag.id;
                List<EOW_EmployeeeExpense> objobjMExpense = new List<EOW_EmployeeeExpense>();
                if (viewfor == "edit")
                {
                    ViewBag.viewfor = "edit";
                }
                else if (viewfor == "view")
                {
                    ViewBag.viewfor = "view";
                }
                id = Convert.ToInt32(Session["EmpExpenseactiverow"].ToString());
                ViewBag.SearchItems = null;
                objobjMExpense = objModel.SelectEmployeeeExpensebyidnew(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), id).ToList();
                objMExpenseEdit.Exp_OUCode = objobjMExpense[0].Exp_OUCode.ToString();
                objMExpenseEdit.Exp_ProductCode = objobjMExpense[0].Exp_ProductCode.ToString();
                objMExpenseEdit.Exp_ClaimPeriodFrom = objobjMExpense[0].Exp_ClaimPeriodFrom.ToString();
                objMExpenseEdit.Exp_ClaimPeriodTo = objobjMExpense[0].Exp_ClaimPeriodTo.ToString();
                //objMExpenseEdit.Exp_FC = objobjMExpense[0].Exp_FC.ToString();
                //objMExpenseEdit.Exp_CC = objobjMExpense[0].Exp_CC.ToString();
                objMExpenseEdit.Exp_FCC = new SelectList(objModel.fcdata().ToList(), "raiserfcId", "raiserfcName", objobjMExpense[0].Exp_FC.ToString());
                objMExpenseEdit.Exp_CCC = new SelectList(objModel.ccdata().ToList(), "raiserccId", "raiserccName", objobjMExpense[0].Exp_CC.ToString());

                objMExpenseEdit.Exp_FCCC = objModel.selectfccc(objobjMExpense[0].Exp_FC.ToString(), objobjMExpense[0].Exp_CC.ToString());

                objMExpenseEdit.Exp_Amount = objCmnFunctions.GetINRAmount(objobjMExpense[0].Exp_Amount.ToString());
                objMExpenseEdit.NatureofExpensesId = objobjMExpense[0].NatureofExpensesId;
                objMExpenseEdit.ExpenseCategoryId = objobjMExpense[0].ExpenseCategoryId;
                objMExpenseEdit.SubCategoryId = objobjMExpense[0].SubCategoryId;
                objMExpenseEdit.ExpNatureofExpdata = new SelectList(objModel.NatureofExpensesdataother().ToList(), "NatureofExpensesId", "NatureofExpensesName", objobjMExpense[0].NatureofExpensesId.ToString());
                objMExpenseEdit.ExpCatdata = new SelectList(objModel.ExpenseCategorydata(objobjMExpense[0].NatureofExpensesId).ToList(), "ExpenseCategoryId", "ExpenseCategoryName", objobjMExpense[0].ExpenseCategoryId.ToString());
                objMExpenseEdit.ExpSubCatdata = new SelectList(objModel.SubCategorydata(objobjMExpense[0].ExpenseCategoryId).ToList(), "SubCategoryId", "SubCategoryName", objobjMExpense[0].SubCategoryId.ToString());
                objMExpenseEdit.HsncodeList = new SelectList(objModel.HsnCodeList(objobjMExpense[0].SubCategoryId).ToList(), "hsnid", "hsncode", Convert.ToString(objobjMExpense[0].Hsnid));
                objMExpenseEdit.HsnDescription = objobjMExpense[0].HsnDescription;
                objMExpenseEdit.PlaceFrom = objobjMExpense[0].PlaceFrom;
                objMExpenseEdit.PlaceTo = objobjMExpense[0].PlaceTo;
                objMExpenseEdit.TravelModeId = objobjMExpense[0].TravelModeId;
                objMExpenseEdit.TravelClassId = objobjMExpense[0].TravelClassId;
                objMExpenseEdit.Rate = objobjMExpense[0].Rate;
                objMExpenseEdit.Distance = objobjMExpense[0].Distance;
                objMExpenseEdit.travelDescription = objobjMExpense[0].travelDescription;
                objMExpenseEdit.Hsnid = objobjMExpense[0].Hsnid;
                //objMExpenseEdit.Citydata = new SelectList(objModel.tTravelcitydata().ToList(), "TravelCityName", "TravelCityName", objobjMExpense[0].PlaceFrom);
                //objMExpenseEdit.Citydatato = new SelectList(objModel.tTravelcitydata().ToList(), "TravelCityName", "TravelCityName", objobjMExpense[0].PlaceTo);
                objMExpenseEdit.TravelModedata = new SelectList(objModel.tTravelModedata().ToList(), "TravelModeId", "TravelModeName", objobjMExpense[0].TravelModeId);
                objMExpenseEdit.TravelClassdata = new SelectList(objModel.tTravelClassdatadata(objobjMExpense[0].TravelModeId).ToList(), "TravelClassId", "TravelClassName", objobjMExpense[0].TravelClassId);
                objMExpenseEdit.RCMFlag = objobjMExpense[0].RCMFlag;
                Session["Ecfamountvalforedit"] = Convert.ToDecimal(Session["Ecfamountval"].ToString()) + Convert.ToDecimal(objobjMExpense[0].Exp_Amount.ToString());
                return PartialView(objMExpenseEdit);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView();
            }

        }
        [HttpPost]
        public JsonResult _EmpExpenseEdit(EOW_EmployeeeExpense objMExpenseEdit)
        {
            try
            {
                string msg = "";
                msg = objModel.UpdateEmployeeeExpensenew(objMExpenseEdit, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), Session["EmpExpenseactiverow"].ToString(), Session["SelfModeRaiseid"].ToString());
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult _EmpPaymentEdit(EOW_Payment objMExpenseEdit)
        {
            string message = objModel.UpdateEmployeeePaymentpetty(objMExpenseEdit, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), Session["EmpPaymentactiverow"].ToString(), Session["EmpPaymentactiverowamt"].ToString(), Session["Ecfamountpayment1"].ToString());
            Session["EmpPaymentactiverowException"] = "0";
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult _Emplistaddp(EOW_Employeelst Employeelst)
        {
            Session["EmpPaymentactiverow"] = Employeelst.empName.ToString();
            string message = "";
            message = "Success";
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult _EmpPaymentEdit(int id, string viewfor)
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
                id = Convert.ToInt32(Session["EmpPaymentactiverow"].ToString());
                ViewBag.SearchItems = null;
                objobjMPayment = objModel.SelectEmployeeePaymentid(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), id).ToList();
                objMPaymentEdit.Description = objobjMPayment[0].Description.ToString();
                objMPaymentEdit.Beneficiary = objobjMPayment[0].Beneficiary.ToString();
                objMPaymentEdit.PaymentAmount = objCmnFunctions.GetINRAmount(objobjMPayment[0].PaymentAmount.ToString());
                objMPaymentEdit.PaymentModedata = new SelectList(objModel.PaymentModedata().ToList(), "PaymentModeName", "PaymentModeName", objobjMPayment[0].PaymentModeName.ToString());
                //objMPaymentEdit.Refdata = new SelectList(objModel.EmployeeePaymentRefNodata(Session["SelfModeRaiseid"].ToString(), "P").ToList(), "RefNoName", "RefNoName", objobjMPayment[0].RefNoName.ToString());
                if (objobjMPayment[0].PaymentModeName == "PPX")
                {
                    objMPaymentEdit.Refdata = new SelectList(objModel.EmployeeePaymentRefNodata(Session["SelfModeRaiseid"].ToString(), "E").ToList(), "RefNoName", "RefNoName", objobjMPayment[0].RefNoName.ToString());
                }
                else
                {
                    objMPaymentEdit.Beneficiarycardno = objobjMPayment[0].RefNoName.ToString();
                    List<EOW_RefNo> objselect = new List<EOW_RefNo>();
                    EOW_RefNo objselModel = new EOW_RefNo();
                    objselModel.RefNoId = Convert.ToString("0");
                    objselModel.RefNoName = Convert.ToString("Select");
                    objselect.Add(objselModel);
                    objMPaymentEdit.Refdata = new SelectList(objselect, "RefNoName", "RefNoName");
                }
                Session["EmpPaymentactiverowamt"] = objobjMPayment[0].PaymentAmount.ToString();
                Session["Ecfamountpayment"] = Convert.ToDecimal(Session["Ecfamountpayment1"].ToString()) + Convert.ToDecimal(objobjMPayment[0].PaymentAmount.ToString());
                decimal arfamt = Convert.ToDecimal(objobjMPayment[0].Exception.ToString()) + Convert.ToDecimal(objobjMPayment[0].PaymentAmount.ToString());
                Session["EmpPaymentactiverowException"] = objobjMPayment[0].Exception.ToString();
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
        public JsonResult DeleteEmpExpense(EOW_EmployeeeExpense EmployeeeExpensemodel)
        {
            try
            {
                int TravelModeGID = (int)EmployeeeExpensemodel.Exp_GID;
                string delamt = objModel.DeleteTravelExpense(TravelModeGID, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString());
                return Json(EmployeeeExpensemodel, JsonRequestBehavior.AllowGet);

                //int EmployeeeExpenseGID = (int)EmployeeeExpensemodel.Exp_GID;
                //string delamt = objModel.DeleteEmployeeeExpense(EmployeeeExpenseGID, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString());
                //return Json(EmployeeeExpensemodel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred...", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult EmpPaymentDelete(EOW_Payment EmployeeeExpensemodel)
        {
            try
            {
                int EmployeeePaymentGID = (int)EmployeeeExpensemodel.Paymentgid;
                string delamt = objModel.DeleteEmployeeePayment(EmployeeePaymentGID, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), Session["Ecfamountpayment1"].ToString());
                //Session["Ecfamountpayment"] = Convert.ToInt32(Session["Ecfamountpayment"].ToString()) + Convert.ToInt32(delamt.ToString());
                return Json(EmployeeeExpensemodel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred...", JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult EmpAttachmentDelete(EOW_File EmployeeeExpensemodel)
        {
            try
            {
                int EmployeeePaymentGID = (int)EmployeeeExpensemodel.AttachmentFilenameId;
                string delamt = objModel.DeleteEmployeeeAttachment(EmployeeePaymentGID, Session["EcfGid"].ToString());
                //Session["Ecfamountpayment"] = Convert.ToInt32(Session["Ecfamountpayment"].ToString()) + Convert.ToInt32(delamt.ToString());
                return Json(EmployeeeExpensemodel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred...", JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public JsonResult GetPaymodeRefNo(EOW_EmployeeeExpense EmployeeeExpense)
        {
            try
            {
                return Json(objModel.EmployeeePaymentRefNodata(Session["SelfModeRaiseid"].ToString(), "P"), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred...", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult _EmpAttachmentCreate(EOW_File EmployeeeExpenseModel)
        {
            try
            {
                string img = "";
                if (TempData["_attFile"] != null)
                {
                    HttpPostedFileBase savefile = TempData["_attFile"] as HttpPostedFileBase;
                    string getname = objModel.InsertEmpAtt(savefile, EmployeeeExpenseModel, Session["EcfGid"].ToString(), objCmnFunctions.GetLoginUserGid().ToString());
                    if (getname != "")
                    {
                        //string path = @"C:\temp\";
                        //string exten = Path.GetExtension(savefile.FileName);
                        //getname = getname + exten.ToString();
                        //string savedFileName = Path.Combine(path, getname);
                        //savefile.SaveAs(savedFileName);

                        //var fileextension = Path.GetExtension(savefile.FileName);
                        //var stream = savefile.InputStream;
                        //var path = Path.Combine(@"C:\temp\", getname + fileextension);
                        //using (var fileStream = System.IO.File.Create(path))
                        //{
                        //    stream.CopyTo(fileStream);
                        //}

                        //upload the file to server.
                        HttpPostedFileBase _attFile = TempData["_attFile"] as HttpPostedFileBase;
                        var stream = _attFile.InputStream;
                        string uploaderUrl = string.Format("{0}{1}", objModel.HoldFileUploadUrl(), getname + "." + _attFile.FileName.Split('.')[_attFile.FileName.Split('.').Length - 1]);
                        //using (var fileStream = System.IO.File.Create(uploaderUrl))
                        //{
                        //    stream.CopyTo(fileStream);
                        //}
                        //remove the temp data content.
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
                        TempData.Remove("_attFile");

                        img = "Yes";
                    }
                }
                else
                {
                    TempData.Remove("_attFile");
                    img = "Invalid File Format!";
                }
                ViewBag.SearchItems = null;
                return Json(img, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        public FileResult Download(int id)
        {
            string FileName = objModel.downloadAttachment(id, Session["EcfGid"].ToString());

           // //int index = delamt.LastIndexOf(".");
          //  //string[] seqNum = new string[] { delamt.Substring(0, index), delamt.Substring(index + 1) };            
          //  //string directory = @"C:\temp\";
          //  //directory = directory + id.ToString() + "." + seqNum[1].ToString();
          //  //byte[] fileBytes = System.IO.File.ReadAllBytes(directory);
           // //string fileName = seqNum[0].ToString() + "." + seqNum[1].ToString();
          //  //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            string[] str = FileName.Split('.');
            string directory =  id.ToString() + "." + str[str.Length - 1].ToString();
           // string directory = objModel.HoldFileUploadUrl() + id.ToString() + "." + str[str.Length - 1].ToString();
           // byte[] fileBytes = System.IO.File.ReadAllBytes(directory);
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
            //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, FileName);
        }


        [HttpPost]
        public void UploadFiles(string attach = null, string attaching_format = null)
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
            catch (Exception)
            {
            }
        }

        [HttpPost]
        public void UploadedFiles()
        {
            try
            {
                string filename = "";
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        filename = Path.GetFileNameWithoutExtension(fileContent.FileName);
                        var fileextension = Path.GetExtension(fileContent.FileName);
                        var stream = fileContent.InputStream;
                        var path = Path.Combine(@"C:\temp\", filename + fileextension);
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                        filename = filename + fileextension;
                        Session["filename"] = filename;
                    }

                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
        }

        [HttpPost]
        public JsonResult _EmpECFCreate(EOW_EmployeeeExpense EmployeeeExpenseModel)
        {
            ForMailController mailsender = new ForMailController();
            string Err = "";
            try
            {
                if (ModelState.IsValid)
                {
                    decimal pay = 0, exp = 0, ecfamount = 0;
                    //decimal pay = Convert.ToDecimal(Session["Ecfamountpayment1"].ToString());
                    //decimal exp = Convert.ToDecimal(Session["Ecfamountval"].ToString());
                    ////decimal ecf = Convert.ToInt32(Session["Ecfamountvalmain"].ToString());
                    //decimal ecfamount = Convert.ToDecimal(Session["Ecfamountvalmain"].ToString());
                    try
                    {
                        List<EOW_Payment> lstPayment = new List<EOW_Payment>();
                        lstPayment = objModel.GetEmployeeePayment(Session["EcfGid"].ToString(), "0").ToList();
                        for (int tr = 0; lstPayment.Count > tr; tr++)
                        {
                            pay = pay + Convert.ToDecimal(lstPayment[tr].PaymentAmount);
                        }

                        List<EOW_EmployeeeExpense> lst = new List<EOW_EmployeeeExpense>();
                        lst = objModel.GetEmployeeeExpensenew(Session["EcfGid"].ToString(), "0").ToList();
                        //Ramya added acfamount
                        ecfamount = Convert.ToDecimal(lst[0].ECF_Amount.ToString());
                        for (int tr = 0; lst.Count > tr; tr++)
                        {
                            exp = exp + Convert.ToDecimal(lst[tr].Exp_Amount.ToString()); 
                        }
                        exp = exp - ecfamount;
                    }
                    catch (Exception ex)
                    {
                        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                    }
                    var result = objModel.GetEmployeePayModeEraAcc(Convert.ToInt32(Session["SelfModeRaiseid"]));
                    if (exp != 0)
                    {
                        Err = "Expense";
                    }
                    else if (pay < 0)
                    {
                        Err = "Payment";
                    }
                    else if (result == "No")
                    {
                        Err = "BankAcc";
                    }
                    else
                    {
                        if (Session["loginraisemode"] != null)
                        {
                            Err = objModel.Insertecfdraftproxcy(EmployeeeExpenseModel, Session["EcfGid"].ToString());
                            Err = "Proxy";
                        }
                        else
                        {
                            //string demlat = objModel.Getraiserdelmat(Session["EcfGid"].ToString(), Session["SelfModeRaiseid"].ToString());
                            //if (demlat == "Yes")
                            //{
                            Err = objModel.Insertecf(EmployeeeExpenseModel, Session["EcfGid"].ToString(), "0", objCmnFunctions.GetLoginUserGid().ToString(), Session["SelfModeRaiseid"].ToString(), "E", Session["QueueGide"].ToString());
                            if (Err == "")
                            {
                                Err = "Error";
                            }
                            else
                            {
                                Err = Err.ToString();
                                Session["QueueGide"] = null;
                                Session["EcfGid"] = null;
                                Session["invoiceGid"] = null;
                                Session["Ecfamountpayment1"] = null;
                                Session["Ecfamountval"] = null;
                                //string results = mailsender.sendusermail("EOW", "Regular", "658", "S", "0");
                            }
                            //}
                            //else
                            //{
                            //    Err = "Delmat";
                            //}
                        }
                    }
                }
                ViewBag.SearchItems = null;
                return Json(Err, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred...", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult _EmpECFDraft(EOW_EmployeeeExpense EmployeeeExpenseModel)
        {
            string Err = "";
            try
            {
                if (ModelState.IsValid)
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
                }
                ViewBag.SearchItems = null;
                return Json(Err, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred...", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult checkvalataeemployeeS(EOW_EmployeeeExpense EmployeeeExpense)
        {
            int nodays = 0;
            string result = objModel.GetStatusexcel("", Session["SelfModeecfval"].ToString(), "", "Ecfraiser");
            nodays = Convert.ToInt32(result);
            if (nodays < 15)
            {
                result = "NotRaise";
            }
            else
            {
                result = "Raise";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult _LoadGstDetails()
        {
            List<EOW_TravelClaim> lstnew = new List<EOW_TravelClaim>();
            return PartialView();
        }
        [HttpGet]
        public PartialViewResult _LoadRCMDetails()
        {
            List<RCMEnteries> lstnew = new List<RCMEnteries>();
            return PartialView();
        }

        [HttpPost]
        public JsonResult SetInvoiceNonTravelDetails(string EcfId, string InvId, string ProviderLocation, string GstinVendor, string Suppliergid, string InvNo,
            string InvDate, string Amount, string Desc, string WOTaxAmount, string IsVerify, string IsRemoved, string GstCharged, string ReceiverLocation, string GstinFiccl, string ServiceMonth)
        {
            try
            {
                DataTable dt = new DataTable();
                string Data1 = "", Data2 = "", Data3 = "",Data4="";
                DataSet ds = db.SetInvoiceNonTravelDetails(Session["EcfGid"].ToString(), InvId, ProviderLocation, GstinVendor, Suppliergid, InvNo,
             InvDate, Amount, Desc, WOTaxAmount, IsVerify, IsRemoved, GstCharged, ReceiverLocation, GstinFiccl, ServiceMonth, objCmnFunctions.GetLoginUserGid().ToString());
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];

                        if (dt.Rows.Count > 0)
                        {
                            Session["invoiceGid"] = Convert.ToString(dt.Rows[0]["InvId"]);
                            Data1 = JsonConvert.SerializeObject(dt);
                        }
                    }
                    if (ds.Tables.Count > 1)
                    {
                        dt = ds.Tables[1];
                        if (dt.Rows.Count > 0)
                        {


                            Data2 = JsonConvert.SerializeObject(dt);
                        }
                    }
                    if (ds.Tables.Count > 2)
                    {
                        dt = ds.Tables[2];
                        if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }
                    }
                    if (ds.Tables.Count > 3)
                    {
                        dt = ds.Tables[3];
                        if (dt.Rows.Count > 0) { Data4 = JsonConvert.SerializeObject(dt); }
                    }
                }
                //Session["invoicegid"] = inv.InvId;
                return Json(new { Data1, Data2, Data3, Data4 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        [HttpPost]
        public JsonResult LoadInvoiceHeaderDetails(string InvId)
        {
            try
            {
                DataTable dt = new DataTable();
                string Data1 = "", Data2 = "", Data3 = "";
                DataSet ds = db.LoadInvoiceHeaderDetails(Convert.ToString(Session["EcfGid"]), InvId);
                Session["invoiceGid"] = null;
                Session["invoiceGid"] = !string.IsNullOrEmpty(InvId) ? InvId : null;
                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                        if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                    }
                    if (ds.Tables.Count > 1)
                    {
                        dt = ds.Tables[1];
                        if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                    }
                    if (ds.Tables.Count > 2)
                    {
                        dt = ds.Tables[2];
                        if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }
                    }
                }
                return Json(new { Data1, Data2, Data3 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        [HttpPost]
        public JsonResult DestroySession()
        {
            Session["invoiceGid"] = null;
            string Data1 = "Success";
            Data1 = JsonConvert.SerializeObject(Data1);
            return Json(new { Data1 }, JsonRequestBehavior.AllowGet);

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
    }
}
