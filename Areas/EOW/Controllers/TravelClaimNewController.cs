using IEM.Areas.EOW.Models;
using IEM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using IEM.Areas.Dashboard.Models;
using Rotativa.Options;
using IEM.Areas.MASTERS.Controllers;
using IEM.Helper;
using System.Data;
using Newtonsoft.Json;
using IEM.App_Start;

namespace IEM.Areas.EOW.Controllers
{

    [NoDirectAccess]
    public class TravelClaimNewController : Controller
    {
        //
        // GET: /EOW/TravelClaimNew/
        ErrorLog objErrorLog = new ErrorLog();
        private EOW_IRepository objModelTravel;
        CmnFunctions objCmnFunctions = new CmnFunctions();
        private FileServer Cmnfs = new FileServer();
        LocalConveyanceNewController locals = new LocalConveyanceNewController();
        CentralModel cen = new CentralModel();
        CommonIUD objCommonIUD = new CommonIUD();
        proLib plib = new proLib();
        dbLib db = new dbLib();
        DataTable dt;
        public TravelClaimNewController()
            : this(new EOW_DataModel())
        {

        }
        public TravelClaimNewController(EOW_IRepository objM)
        {
            objModelTravel = objM;
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
                    //Session["invoiceGid"] = eowemp.invoice_GID.ToString();
                    if (eowemp.Queue_GID != 0)
                    {
                        Session["QueueGide"] = eowemp.Queue_GID.ToString();
                    }
                    else
                    {
                        Session["QueueGide"] = "";
                    }
                    ViewBag.process = "Postdata";
                    ViewBag.message = "success";
                    Session["Ecfamountvalmain"] = eowemp.ECF_Amount;
                    eowemp.ECF_Amount = objCmnFunctions.GetINRAmount(eowemp.ECF_Amount);
                    ViewBag.EOW_EmployeeeExpenseheader = eowemp;
                    Session["DashBoard"] = null;

                    if (Session["loginraisemode"] != null)
                    {
                        if (eowemp.raisermodeId == "P")
                        {
                            Session["SelfModeRaiseid"] = objCmnFunctions.GetLoginProxyUserGid().ToString();
                            Session["SelfModeecfval"] = objCmnFunctions.GetLoginUserGid().ToString();
                        }
                        else
                        {
                            Session["SelfModeRaiseid"] = objCmnFunctions.GetLoginUserGid().ToString();
                            Session["SelfModeecfval"] = objCmnFunctions.GetLoginUserGid().ToString();
                        }
                        List<EOW_EmployeeeExpense> objmaindetaild = new List<EOW_EmployeeeExpense>();
                        objmaindetaild = objModelTravel.SelectEmployeeeheader(objCmnFunctions.GetLoginProxyUserGid().ToString()).ToList();
                        Session["raiserfcc"] = objmaindetaild[0].Exp_FC.ToString();
                        Session["raiserccc"] = objmaindetaild[0].Exp_CC.ToString();
                    }
                    else
                    {
                        Session["SelfModeRaiseid"] = objCmnFunctions.GetLoginUserGid().ToString();
                        Session["SelfModeecfval"] = objCmnFunctions.GetLoginUserGid().ToString();

                        List<EOW_EmployeeeExpense> objmaindetaild = new List<EOW_EmployeeeExpense>();
                        objmaindetaild = objModelTravel.SelectEmployeeeheader(objCmnFunctions.GetLoginUserGid().ToString()).ToList();
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
                        Session["SelfModeRaiseid"] = objCmnFunctions.GetLoginProxyUserGid().ToString();
                        Session["SelfModeecfval"] = objCmnFunctions.GetLoginProxyUserGid().ToString();
                        objmaindetail = objModelTravel.SelectEmployeeeheader(objCmnFunctions.GetLoginProxyUserGid().ToString()).ToList();

                        eowemp.Grade = objmaindetail[0].Grade.ToString();
                        eowemp.Raiser_Code = objmaindetail[0].Raiser_Code.ToString();
                        eowemp.Raiser_Name = objmaindetail[0].Raiser_Name.ToString();

                        eowemp.raisermodeId = "P";
                        List<EOW_EmployeeeExpense> objmaindetaild = new List<EOW_EmployeeeExpense>();
                        objmaindetaild = objModelTravel.SelectEmployeeeheader(objCmnFunctions.GetLoginProxyUserGid().ToString()).ToList();
                        Session["raiserfcc"] = objmaindetaild[0].Exp_FC.ToString();
                        Session["raiserccc"] = objmaindetaild[0].Exp_CC.ToString();
                    }
                    else
                    {
                        Session["SelfModeRaiseid"] = objCmnFunctions.GetLoginUserGid().ToString();
                        Session["SelfModeecfval"] = objCmnFunctions.GetLoginUserGid().ToString();
                        objmaindetail = objModelTravel.SelectEmployeeeheader(objCmnFunctions.GetLoginUserGid().ToString()).ToList();

                        eowemp.Grade = objmaindetail[0].Grade.ToString();
                        eowemp.Raiser_Code = objmaindetail[0].Raiser_Code.ToString();
                        eowemp.Raiser_Name = objmaindetail[0].Raiser_Name.ToString();

                        eowemp.raisermodeId = "S";//"C OR S OR P" 
                        List<EOW_EmployeeeExpense> objmaindetaild = new List<EOW_EmployeeeExpense>();
                        objmaindetaild = objModelTravel.SelectEmployeeeheader(objCmnFunctions.GetLoginUserGid().ToString()).ToList();
                        Session["raiserfcc"] = objmaindetaild[0].Exp_FC.ToString();
                        Session["raiserccc"] = objmaindetaild[0].Exp_CC.ToString();
                    }
                    eowemp.Raiser_Modedata = new SelectList(objModelTravel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", eowemp.raisermodeId);
                    ViewBag.EOW_EmployeeeExpenseheader = eowemp;
                    ViewBag.process = "";
                    Session["EcfGid"] = null;
                    Session["invoiceGid"] = null;
                    Session["Ecfamountpaymenttt"] = null;
                    Session["Ecfamountvaloe"] = null;
                    Session["Ecfamountvaltm"] = null;
                }
                Session["NonBranchClaimFlage"] = objModelTravel.travelbranchflag(objCmnFunctions.GetLoginUserGid());
                Session["Ecfdeclaratonnote"] = objModelTravel.GetDecnote("3", "S").ToString();
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
                objmaindetail = objModelTravel.SelectEmployeeeheader(objCmnFunctions.GetLoginUserGid().ToString()).ToList();
                if (obj.ecfremark != null)
                {
                    ViewBag.ecfrmarks = obj.ecfremark;
                }

                EOW_EmployeeeExpense eowemp = new EOW_EmployeeeExpense();
                if (Session["loginraisemode"] != null)
                {
                    obj.raisermodeId = "P";
                    objmaindetail = objModelTravel.SelectEmployeeeheader(objCmnFunctions.GetLoginProxyUserGid().ToString()).ToList();
                    obj.Grade = objmaindetail[0].Grade.ToString();
                    obj.Raiser_Code = objmaindetail[0].Raiser_Code.ToString();
                    obj.Raiser_Name = objmaindetail[0].Raiser_Name.ToString();
                }
                else
                {
                    obj.raisermodeId = "S";//"C OR S OR P" 
                    objmaindetail = objModelTravel.SelectEmployeeeheader(objCmnFunctions.GetLoginUserGid().ToString()).ToList();
                    obj.Grade = objmaindetail[0].Grade.ToString();
                    obj.Raiser_Code = objmaindetail[0].Raiser_Code.ToString();
                    obj.Raiser_Name = objmaindetail[0].Raiser_Name.ToString();
                }

                Session["Ecfamountvalmain"] = obj.ECF_Amount;
                obj.Raiser_Modedata = new SelectList(objModelTravel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", obj.raisermodeId);
                clmmonth = obj.ClaimMonth;
                int nodays = 0;
                string result = objModelTravel.GetStatusexcel("", Session["SelfModeecfval"].ToString(), "", "Ecfraiser");
                nodays = Convert.ToInt32(result);
                if (nodays < 15)
                {
                    ViewBag.process = "Postdataerr";
                    message = "Employee Can't Raise ECF";
                }
                else
                {
                    obj.ClaimMonth = locals.getconverttomonthtodate(obj.ClaimMonth);

                    //if (Session["EcfGid"] != null && Session["invoiceGid"] != null)
                    if (Session["EcfGid"] != null)
                    {
                        message = objModelTravel.InsertEmployeeeBasicupdate(obj, objCmnFunctions.GetLoginUserGid().ToString(), Session["SelfModeRaiseid"].ToString(), Session["EcfGid"].ToString());
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
                            insrtinvoice = objModelTravel.InsertEmployeeeBasicinvoiceupdate(obj, Session["SelfModeRaiseid"].ToString(), Session["EcfGid"].ToString());
                            if (insrtinvoice!="")
                            {
                                Session["invoiceGid"] = insrtinvoice.ToString();
                                string IFSCCode = "";
                                ViewBag.IFSCCodemsg = "";
                                IFSCCode = objModelTravel.InsertEmployeeePaymentbasicupdate(Session["SelfModeRaiseid"].ToString(), Session["EcfGid"].ToString(), Session["Ecfamountvalmain"].ToString());
                                message = "success";
                                if(IFSCCode.ToString().Trim().Equals(""))
                                {
                                    ViewBag.IFSCCodemsg = "IFSC Code is not update!!! Your Claim may get REJECT by EPU Team!";
                                }
                                //message = objModelTravel.insertempperson(Employeelst, Session["EcfGid"].ToString());
                            }
                            else
                            {
                                message = "ECF details Not Updated Properly Please check ECF Header Details";
                            }
                        }
                    }
                    else
                    {
                        message = objModelTravel.InsertEmployeeeBasic(obj, objCmnFunctions.GetLoginUserGid().ToString(), "T");
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
                            Session["EcfGid"] = objModelTravel.selectecfgidBasic(obj, objCmnFunctions.GetLoginUserGid().ToString());
                            insrtinvoice = objModelTravel.InsertEmployeeeBasicinvoice(obj, Session["SelfModeRaiseid"].ToString(), Session["EcfGid"].ToString(), "T");
                            if (insrtinvoice !="")
                            {
                                string IFSCCode = "";
                                ViewBag.IFSCCodemsg = "";
                                Session["invoiceGid"] = objModelTravel.selectinvoiceidBasic(obj, Session["SelfModeRaiseid"].ToString(), Session["EcfGid"].ToString(), "E");
                                IFSCCode = objModelTravel.InsertEmployeeePaymentbasic(Session["SelfModeRaiseid"].ToString(), Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), Session["Ecfamountvalmain"].ToString());
                                if (IFSCCode.ToString().Trim().Equals(""))
                                {
                                    ViewBag.IFSCCodemsg = "IFSC Code is not updated!!! Your Claim may get REJECT by EPU Team!";
                                }
                                EOW_Employeelst addemp = new EOW_Employeelst();
                                addemp.employeeGid = Convert.ToInt32(Session["SelfModeRaiseid"].ToString());
                                addemp.empbranch = objModelTravel.Getemployeebrach(Session["SelfModeRaiseid"].ToString());
                                addemp.empfc = Session["raiserfcc"].ToString();
                                addemp.empcc = Session["raiserccc"].ToString();
                                message = objModelTravel.insertempperson(addemp, Session["EcfGid"].ToString());
                            }
                            else
                            {
                                message = "ECF details Not Inserted Properly Please check ECF Header Details";
                            }
                        }
                    }
                }
                ViewBag.message = message;
                obj.ClaimMonth = clmmonth;
                obj.ECF_Amount = ecfnewamt;
                ViewBag.EOW_EmployeeeExpenseheader = obj;
                Session["NonBranchClaimFlage"] = objModelTravel.travelbranchflag(objCmnFunctions.GetLoginUserGid());
                return View();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
        }
        [HttpGet]
        public PartialViewResult _TravelModeCreate()
        {
            try
            {
                EOW_TravelClaim objMExpense = new EOW_TravelClaim();
                List<EOW_TravelClaim> objobjMExpense = new List<EOW_TravelClaim>();
                objobjMExpense = objModelTravel.tSelectEmployeeeBasic(Session["SelfModeRaiseid"].ToString()).ToList();
                objMExpense.OUCode = objobjMExpense[0].OUCode.ToString();
                objMExpense.ProductCode = objobjMExpense[0].ProductCode.ToString();
                //objMExpense.FC = objobjMExpense[0].FC.ToString();
                //objMExpense.CC = objobjMExpense[0].CC.ToString();

                objMExpense.Exp_FCC = new SelectList(objModelTravel.fcdata().ToList(), "raiserfcId", "raiserfcName", Session["raiserfcc"].ToString());
                objMExpense.Exp_CCC = new SelectList(objModelTravel.ccdata().ToList(), "raiserccId", "raiserccName", Session["raiserccc"].ToString());
                objMExpense.Exp_FCCC = objobjMExpense[0].Exp_FCCC.ToString();

                objMExpense.ExpNatureofExpdata = new SelectList(objModelTravel.NatureofExpensesdata().ToList(), "NatureofExpensesId", "NatureofExpensesName");
                objMExpense.TravelModedata = new SelectList(objModelTravel.tTravelModedata().ToList(), "TravelModeId", "TravelModeName");
                objMExpense.Citydata = new SelectList(objModelTravel.tTravelcitydata().ToList(), "TravelCityId", "TravelCityName");
                return PartialView(objMExpense);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView();
            }
        }
        [HttpGet]
        public PartialViewResult _OtherTravelExpenseCreate()
        {
            try
            {
                EOW_TravelClaim objMExpense = new EOW_TravelClaim();
                List<EOW_TravelClaim> objobjMExpense = new List<EOW_TravelClaim>();
                objobjMExpense = objModelTravel.tSelectEmployeeeBasic(Session["SelfModeRaiseid"].ToString()).ToList();
                objMExpense.OUCode = objobjMExpense[0].OUCode.ToString();
                objMExpense.ProductCode = objobjMExpense[0].ProductCode.ToString();
                //objMExpense.FC = objobjMExpense[0].FC.ToString();
                //objMExpense.CC = objobjMExpense[0].CC.ToString();

                objMExpense.Exp_FCC = new SelectList(objModelTravel.fcdata().ToList(), "raiserfcId", "raiserfcName", Session["raiserfcc"].ToString());
                objMExpense.Exp_CCC = new SelectList(objModelTravel.ccdata().ToList(), "raiserccId", "raiserccName", Session["raiserccc"].ToString());
                objMExpense.Exp_FCCC = objobjMExpense[0].Exp_FCCC.ToString();

                objMExpense.ExpNatureofExpdata = new SelectList(objModelTravel.NatureofExpensesdataother().ToList(), "NatureofExpensesId", "NatureofExpensesName");
                objMExpense.Citydata = new SelectList(objModelTravel.tTravelcitydata().ToList(), "TravelCityId", "TravelCityName");
                return PartialView(objMExpense);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView();
            }
        }
        [HttpPost]
        public JsonResult _TravelModeCreate(EOW_TravelClaim TravelClaimModel)
        {
            string message = "";
            try
            {
                if (!string.IsNullOrEmpty(Convert.ToString(Session["invoiceGid"])))
                {
                    message = objModelTravel.InsertTravelModeCreate(TravelClaimModel, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), Session["SelfModeRaiseid"].ToString());
                    ViewBag.SearchItems = null;
                }
                else
                {
                    message = "Please Check Invoice Details";
                }
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult _OtherTravelExpenseCreate(EOW_TravelClaim TravelClaimModel)
        {
            string message = "";
            try
            {
                message = objModelTravel.InsertOtherTravelCreate(TravelClaimModel, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), Session["SelfModeRaiseid"].ToString());
                ViewBag.SearchItems = null;
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetExpenseCategory(EOW_TravelClaim EmployeeeExpense)
        {
            return Json(objModelTravel.ExpenseCategorydata(EmployeeeExpense.NatureofExpensesId), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetExpenseCategoryflag(EOW_TravelClaim EmployeeeExpense)
        {
            return Json(objModelTravel.ExpenseCategorydataflag(EmployeeeExpense.NatureofExpensesId), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetSubCategory(EOW_TravelClaim EmployeeeExpense)
        {
            string Data1 = "", Data2 = "";
            string ExpenseId = Convert.ToString(EmployeeeExpense.ExpenseCategoryId);
            DataSet ds = db.GetExpenseCategory(ExpenseId);
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
            return Json(new { Data1 }, JsonRequestBehavior.AllowGet); 
        }
        public JsonResult GetSubCategorypolicy(EOW_EmployeeeExpense EmployeeeExpense)
        {
            try
            {
                string policy = "", Data2 = "";
                policy = objModelTravel.selectpolicy(EmployeeeExpense.ExpenseCategoryId.ToString());
                Session["policyhelp"] = policy;
                DataSet ds = db.GetExpenseHsn(EmployeeeExpense.ExpenseCategoryId.ToString());
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
        public JsonResult GetTravelClassdata(EOW_TravelClaim EmployeeeExpense)
        {
            return Json(objModelTravel.tTravelClassdatadata(EmployeeeExpense.TravelModeId), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetTravelClassdataflag(EOW_TravelClaim EmployeeeExpense)
        {
            return Json(objModelTravel.Travelclassdataflag(EmployeeeExpense.TravelModeId), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult _EmpPaymentCreate()
        {
            EOW_Payment objMPayment = new EOW_Payment();
            objMPayment.PaymentModedata = new SelectList(objModelTravel.PaymentModedata().ToList(), "PaymentModeId", "PaymentModeName");
            objMPayment.Beneficiary = objModelTravel.GetEmployeeeGID(Session["SelfModeRaiseid"].ToString());
            return PartialView(objMPayment);
        }
        [HttpGet]
        public PartialViewResult _EmpPaymentmodepopup()
        {
            List<EOW_RefNo> lst = new List<EOW_RefNo>();
            lst = objModelTravel.EmployeeePaymentRefNodatagri(Session["SelfModeRaiseid"].ToString(), "E").ToList();
            return PartialView(lst);
        }
        [HttpGet]
        public PartialViewResult _EmpAttachmentCreate()
        {
            EOW_File objMAttachment = new EOW_File();
            objMAttachment.AttachmentTypeData = new SelectList(objModelTravel.AttachmentTypedata().ToList(), "AttachmentTypeId", "AttachmentTypeName");
            return PartialView(objMAttachment);
        }
        [HttpPost]
        public JsonResult _EmpPaymentCreate(EOW_Payment EmployeeeExpenseModel)
        {
            string message = "";
            try
            {
                message = objModelTravel.InsertEmployeeePayment(EmployeeeExpenseModel, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), Session["Ecfamountpaymenttt"].ToString());
                //Session["Ecfamountpayment"] = Convert.ToInt32(Session["Ecfamountpayment"].ToString()) + Convert.ToInt32(EmployeeeExpenseModel.EmployeeePayment_Amount.ToString());                
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
        public PartialViewResult _TravelModeDetails()
        {
            List<EOW_TravelClaim> lstnew = new List<EOW_TravelClaim>();
            //lstnew = objModelTravel.GetTravelModedata(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), "T").ToList();
            return PartialView();
        }
        [HttpGet]
        public PartialViewResult _OtherTravelExpenseDetails()
        {
            List<EOW_TravelClaim> lstnew = new List<EOW_TravelClaim>();
            //lstnew = objModelTravel.GetTravelModedata(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), "E").ToList();
            return PartialView(lstnew);
        }
        [HttpGet]
        public PartialViewResult _EmpPaymentDetails()
        {
            List<EOW_Payment> lstPayment = new List<EOW_Payment>();
            //lstPayment = objModelTravel.GetEmployeeePayment(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString()).ToList();
            //if (Session["Ecfamountpaymenttt"] == null)
            //{
            //    Session["Ecfamountpaymenttt"] = lstPayment[0].PaymentAmount.ToString();
            //}
            return PartialView(lstPayment);
        }
        [HttpGet]
        public PartialViewResult _EmpAttachmentDetails()
        {
            List<EOW_File> lstAttachment = new List<EOW_File>();
            //lstAttachment = objModelTravel.GetEmployeeeAttachment(Session["EcfGid"].ToString(), objCmnFunctions.GetLoginUserGid().ToString()).ToList();
            return PartialView(lstAttachment);
        }
        [HttpGet]
        public PartialViewResult _EmpEmployeeDetails()
        {
            List<EOW_Employeelst> lstGetEmployee = new List<EOW_Employeelst>();
            //lstGetEmployee = objModelTravel.GetEmployeeelist(Session["EcfGid"].ToString()).ToList();
            return PartialView(lstGetEmployee);
        }
        [HttpPost]
        public JsonResult _EmpPaymentEdit(EOW_Payment objMExpenseEdit)
        {
            string message = objModelTravel.UpdateEmployeeePayment(objMExpenseEdit, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), Session["EmpPaymentactiverow"].ToString(), Session["EmpPaymentactiverowamt"].ToString(), Session["Ecfamountpaymenttt"].ToString());
            Session["EmpPaymentactiverowException"] = "0";
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult _Emplistaddpa(EOW_Employeelst Employeelst)
        {
            Session["EmpPaymentactiverow"] = Employeelst.empName.ToString();
            string message = "";
            message = "Success";
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult _EmpPaymentEdit(int id, string viewfor)
        {
            EOW_Payment objMPaymentEdit = new EOW_Payment();
            try
            {
                List<EOW_Payment> objobjMPayment = new List<EOW_Payment>();
                if (viewfor == "edit")
                {
                    ViewBag.viewfor = "edit";
                }
                else if (viewfor == "view")
                {
                    ViewBag.viewfor = "view";
                }
                //Session["EmpPaymentactiverow"] = id.ToString();
                id = Convert.ToInt32(Session["EmpPaymentactiverow"].ToString());
                ViewBag.SearchItems = null;
                objobjMPayment = objModelTravel.SelectEmployeeePaymentid(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), id).ToList();
                objMPaymentEdit.Description = objobjMPayment[0].Description.ToString();
                objMPaymentEdit.Beneficiary = objobjMPayment[0].Beneficiary.ToString();
                objMPaymentEdit.PaymentAmount = objCmnFunctions.GetINRAmount(objobjMPayment[0].PaymentAmount.ToString());
                objMPaymentEdit.PaymentModedata = new SelectList(objModelTravel.PaymentModedata().ToList(), "PaymentModeName", "PaymentModeName", objobjMPayment[0].PaymentModeName.ToString());
                //objMPaymentEdit.Refdata = new SelectList(objModelTravel.EmployeeePaymentRefNodata(Session["SelfModeRaiseid"].ToString(), "E").ToList(), "RefNoName", "RefNoName", objobjMPayment[0].RefNoName.ToString());
                if (objobjMPayment[0].PaymentModeName == "PPX")
                {
                    objMPaymentEdit.Refdata = new SelectList(objModelTravel.EmployeeePaymentRefNodata(Session["SelfModeRaiseid"].ToString(), "E").ToList(), "RefNoName", "RefNoName", objobjMPayment[0].RefNoName.ToString());
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
                Session["Ecfamountpayment"] = Convert.ToDecimal(Session["Ecfamountpaymenttt"].ToString()) + Convert.ToDecimal(objobjMPayment[0].PaymentAmount.ToString());
                decimal arfamt = Convert.ToDecimal(objobjMPayment[0].Exception.ToString()) + Convert.ToDecimal(objobjMPayment[0].PaymentAmount.ToString());
                Session["EmpPaymentactiverowException"] = objobjMPayment[0].Exception.ToString();
                ViewBag.Exception = arfamt;
                return PartialView(objMPaymentEdit);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView(objMPaymentEdit);
            }

        }
        [HttpPost]
        public JsonResult EmpPaymentDelete(EOW_Payment EmployeeeExpensemodel)
        {
            int EmployeeePaymentGID = (int)EmployeeeExpensemodel.Paymentgid;
            string delamt = objModelTravel.DeleteEmployeeePayment(EmployeeePaymentGID, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), Session["Ecfamountpaymenttt"].ToString());
            return Json(EmployeeeExpensemodel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EmpAttachmentDelete(EOW_File EmployeeeExpensemodel)
        {
            int EmployeeePaymentGID = (int)EmployeeeExpensemodel.AttachmentFilenameId;
            string delamt = objModelTravel.DeleteEmployeeeAttachment(EmployeeePaymentGID, Session["EcfGid"].ToString());
            //Session["Ecfamountpayment"] = Convert.ToInt32(Session["Ecfamountpayment"].ToString()) + Convert.ToInt32(delamt.ToString());
            return Json(EmployeeeExpensemodel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPaymodeRefNo(EOW_EmployeeeExpense EmployeeeExpense)
        {
            return Json(objModelTravel.EmployeeePaymentRefNodata(Session["SelfModeRaiseid"].ToString(), "E"), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult _EmpAttachmentCreate(EOW_File EmployeeeExpenseModel)
        {
            string img = "No";
            try
            {
                if (TempData["_attFile"] != null)
                {
                    HttpPostedFileBase savefile = TempData["_attFile"] as HttpPostedFileBase;
                    string getname = objModelTravel.InsertEmpAtt(savefile, EmployeeeExpenseModel, Session["EcfGid"].ToString(),  objCmnFunctions.GetLoginUserGid().ToString());
                    if (getname != "")
                    {
                        ////string path = "//192.168.0.203/temp/";
                        //string path = @"C:\temp\";
                        //string path1 = Path.GetFileName(savefile.FileName);
                        //string exten = Path.GetExtension(savefile.FileName);
                        //string[] str = path1.Split('.');
                        ////var index = path1.LastIndexOf(".") + 1;
                        //getname = getname + exten.ToString();
                        //string savedFileName = Path.Combine(path, getname);
                        //savefile.SaveAs(savedFileName);
                        //img = "Yes";

                        //upload the file to server.
                        HttpPostedFileBase _attFile = TempData["_attFile"] as HttpPostedFileBase;
                        var stream = _attFile.InputStream;
                        string uploaderUrl = string.Format("{0}{1}", objModelTravel.HoldFileUploadUrl(), getname + "." + _attFile.FileName.Split('.')[_attFile.FileName.Split('.').Length - 1]);
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
            string FileName = objModelTravel.downloadAttachment(id, Session["EcfGid"].ToString());
            ////int index = delamt.LastIndexOf(".");
            // //string[] seqNum = new string[] { delamt.Substring(0, index), delamt.Substring(index + 1) };
            // //string directory = @"C:\temp\";
            // //directory = directory + id.ToString() + "." + seqNum[1].ToString();
            // //byte[] fileBytes = System.IO.File.ReadAllBytes(directory);
            // //string fileName = seqNum[0].ToString() + "." + seqNum[1].ToString();
            // //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            string[] str = FileName.Split('.');
            string directory = id.ToString() + "." + str[str.Length - 1].ToString();
            // string directory = objModelTravel.HoldFileUploadUrl() + id.ToString() + "." + str[str.Length - 1].ToString();
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
                //TempData["_attFile"] = null;
                //foreach (string file in Request.Files)
                //{
                //    HttpPostedFileBase hpfBase = Request.Files[file] as HttpPostedFileBase;
                //    TempData["_attFile"] = hpfBase;
                //}
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
        //[HttpPost]
        //public async Task<JsonResult> UploadFiles()
        //{
        //    try
        //    {
        //        string filename = "";
        //        foreach (string file in Request.Files)
        //        {
        //            var fileContent = Request.Files[file];

        //            if (fileContent != null && fileContent.ContentLength > 0)
        //            {
        //                HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
        //                Session["empattmtfile"] = hpf;
        //            }
        //        }
        //        return Json(filename);
        //    }
        //    catch (Exception)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //        return Json("Upload failed");
        //    }
        //}

        [HttpGet]
        public PartialViewResult _Emplistadd()
        {
            List<EOW_Employeelst> objowner = new List<EOW_Employeelst>();
            return PartialView();
        }
        [HttpPost]
        public JsonResult _Emplistadd(EOW_Employeelst Employeelst)
        {
            string message = "";
            message = objModelTravel.insertempperson(Employeelst, Session["EcfGid"].ToString());
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult _TravelsearchEmp(string listfor)
        {
            try
            {
                List<CentraldataModel> obj = new List<CentraldataModel>();
                CentraldataModel Getvaluesearchvalue = new CentraldataModel();
                if (listfor == "search")
                {
                    if (Session["SearchEmployeedata"] != null)
                    {
                        Getvaluesearchvalue = (CentraldataModel)Session["SerchViewbag"];
                        @ViewBag.EmployeeName = Getvaluesearchvalue.RaiserName;
                        @ViewBag.EmployeeCode = Getvaluesearchvalue.RaiserCode;
                        obj = (List<CentraldataModel>)Session["SearchEmployeedata"];
                    }
                }
                else
                {
                    Session["SearchEmployeedata"] = null;
                    Session["SerchViewbag"] = null;
                    obj = cen.SelectEmployee().ToList();
                }
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView();
            }
        }
        [HttpPost]
        public JsonResult EmployeeSearchWithParameter(string RaiserName = "", string RaiserCode = "")
        {
            List<CentraldataModel> recordsearch = new List<CentraldataModel>();
            CentraldataModel emp = new CentraldataModel();
            if (RaiserName != "" || RaiserCode != "")
            {
                @ViewBag.EmployeeName = RaiserName;
                @ViewBag.EmployeeCode = RaiserCode;
                emp.RaiserCode = RaiserCode;
                emp.RaiserName = RaiserName;
                Session["SerchViewbag"] = emp;
                recordsearch = cen.SelectEmployeeSearch(RaiserName, RaiserCode).ToList();
                Session["SearchEmployeedata"] = recordsearch;
            }
            else
            {
                Session["SerchViewbag"] = null;
                Session["SerchViewbag"] = null;
                recordsearch = cen.SelectEmployeeSearch(RaiserName, RaiserCode).ToList();
            }

            return Json(recordsearch, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult _TravelsearClear()
        {
            List<EOW_Employeelst> objowner = new List<EOW_Employeelst>();
            Session["Searchcendatat"] = null;
            Session["SearchViewbagData"] = null;
            objowner = objModelTravel.getemployeedetails();
            return PartialView(objowner);
        }
        [HttpPost]
        public JsonResult searchEmployeedetails(EOW_Employeelst objownersearch)
        {
            List<EOW_Employeelst> objowner = new List<EOW_Employeelst>();
            EOW_Employeelst objowner1 = new EOW_Employeelst();
            objowner = objModelTravel.getemployeedetails();
            // if (objownersearch.empCode != "" || objownersearch.empName!="")
            //{

            //if ((string.IsNullOrEmpty(objownersearch.empCode)) == false)
            //{
            //    ViewBag.empcode = objownersearch.empCode;
            //    objowner = objowner.Where(x => objownersearch.empCode == null ||
            //        (x.empCode.Contains(objownersearch.empCode))).ToList();
            //}
            //if ((string.IsNullOrEmpty(objownersearch.empName)) == false)
            //{
            //    ViewBag.empname = objownersearch.empName;
            //    objowner = objowner.Where(x => objownersearch.empName == null ||
            //        (x.empName.Contains(objownersearch.empName))).ToList();
            //}
            //}
            //return Json(objowner,JsonRequestBehavior.AllowGet);
            //return Json(objowner, JsonRequestBehavior.AllowGet);
            if (objownersearch.empCode != "" || objownersearch.empName != "")
            {
                objowner = objModelTravel.getemployeedetails(objownersearch.empName, objownersearch.empCode).ToList();
                @ViewBag.empcode = objownersearch.empCode;
                @ViewBag.empname = objownersearch.empName;
                objowner1.empCode = objownersearch.empCode;
                objowner1.empName = objownersearch.empName;
                if (objowner.Count == 0)
                {
                    ViewBag.Message = "No Record's Found !";
                }
                Session["Searchcendatat"] = objowner;
                Session["SearchViewbagData"] = objowner1;
            }
            else
            {
                Session["Searchcendatat"] = null;
                Session["SearchViewbagData"] = null;
                objowner = objModelTravel.getemployeedetails().ToList();
            }

            return Json(objowner, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult _Emplistaddtm(EOW_Employeelst Employeelst)
        {
            Session["TravelModeactiverow"] = Employeelst.empName.ToString();
            string message = "";
            message = "Success";
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult _TravelModeEdit(int id, string viewfor)
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
                id = Convert.ToInt32(Session["TravelModeactiverow"].ToString());
                ViewBag.SearchItems = null;
                objobjMExpense = objModelTravel.GetTravelModedatasingle(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), "T", id).ToList();
                objMExpenseEdit.OUCode = objobjMExpense[0].OUCode.ToString();
                objMExpenseEdit.ProductCode = objobjMExpense[0].ProductCode.ToString();
                objMExpenseEdit.ClaimPeriodFrom = objobjMExpense[0].ClaimPeriodFrom.ToString();
                objMExpenseEdit.ClaimPeriodTo = objobjMExpense[0].ClaimPeriodTo.ToString();
                objMExpenseEdit.FC = objobjMExpense[0].FC.ToString();
                objMExpenseEdit.CC = objobjMExpense[0].CC.ToString();

                objMExpenseEdit.Exp_FCC = new SelectList(objModelTravel.fcdata().ToList(), "raiserfcId", "raiserfcName", objobjMExpense[0].FC.ToString());
                objMExpenseEdit.Exp_CCC = new SelectList(objModelTravel.ccdata().ToList(), "raiserccId", "raiserccName", objobjMExpense[0].CC.ToString());
                objMExpenseEdit.Exp_FCCC = objobjMExpense[0].Exp_FCCC.ToString();

                objMExpenseEdit.travelDescription = objobjMExpense[0].travelDescription.ToString();
                objMExpenseEdit.Amount = objCmnFunctions.GetINRAmount(objobjMExpense[0].Amount.ToString());
                objMExpenseEdit.Distance = objobjMExpense[0].Distance.ToString();
                objMExpenseEdit.Rate = objobjMExpense[0].Rate.ToString();

                objMExpenseEdit.Traveltypes = objModelTravel.ExpenseCategorydataflag(objobjMExpense[0].NatureofExpensesId);
                objMExpenseEdit.Travelclass = objModelTravel.Travelclassdataflag(objobjMExpense[0].TravelModeId);

                objMExpenseEdit.ExpNatureofExpdata = new SelectList(objModelTravel.NatureofExpensesdata().ToList(), "NatureofExpensesId", "NatureofExpensesName", objobjMExpense[0].NatureofExpensesId.ToString());
                objMExpenseEdit.ExpCatdata = new SelectList(objModelTravel.ExpenseCategorydata(objobjMExpense[0].NatureofExpensesId).ToList(), "ExpenseCategoryId", "ExpenseCategoryName", objobjMExpense[0].ExpenseCategoryId.ToString());
                objMExpenseEdit.ExpSubCatdata = new SelectList(objModelTravel.SubCategorydata(objobjMExpense[0].ExpenseCategoryId).ToList(), "SubCategoryId", "SubCategoryName", objobjMExpense[0].SubCategoryId.ToString());
                objMExpenseEdit.Citydata = new SelectList(objModelTravel.tTravelcitydata().ToList(), "TravelCityName", "TravelCityName", objobjMExpense[0].PlaceFrom.ToString());
                objMExpenseEdit.Citydatato = new SelectList(objModelTravel.tTravelcitydata().ToList(), "TravelCityName", "TravelCityName", objobjMExpense[0].PlaceTo.ToString());
                objMExpenseEdit.TravelModedata = new SelectList(objModelTravel.tTravelModedata().ToList(), "TravelModeId", "TravelModeName", objobjMExpense[0].TravelModeId.ToString());
                //objMExpenseEdit.HsncodeList = new SelectList(objModelTravel.GetHsnList().ToList(), "TravelHsnid", "TravelHsnCode", objobjMExpense[0].TravelHsnid.ToString());
                objMExpenseEdit.HsncodeList = new SelectList(objModelTravel.HsnCodeList(objobjMExpense[0].SubCategoryId).ToList(), "hsnid", "hsncode", Convert.ToString(objobjMExpense[0].TravelHsnid));

                objMExpenseEdit.TravelHsnDesc = objobjMExpense[0].TravelHsnDesc.ToString();
                //Ramya Added
                objMExpenseEdit.RCMFlag = objobjMExpense[0].RCMFlag.ToString();
                objMExpenseEdit.TravelClassdata = new SelectList(objModelTravel.tTravelClassdatadata(objobjMExpense[0].TravelModeId).ToList(), "TravelClassId", "TravelClassName", objobjMExpense[0].TravelClassId.ToString());
                Session["Ecfamountvaltmeditt"] = Convert.ToDecimal(Session["Ecfamountvaltm"].ToString()) + Convert.ToDecimal(objobjMExpense[0].Amount.ToString());
                ViewBag.TravelotherClaimheader = objMExpenseEdit;
                return PartialView();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView();
            }
        }
        [HttpPost]
        public JsonResult _Emplistaddot(EOW_Employeelst Employeelst)
        {
            Session["OtherTravelactiverow"] = Employeelst.empName.ToString();
            string message = "";
            message = "Success";
            return Json(message, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult _OtherTravelExpenseEdit(int id, string viewfor)
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
                //Session["OtherTravelactiverow"] = id.ToString();
                id = Convert.ToInt32(Session["OtherTravelactiverow"].ToString());
                ViewBag.SearchItems = null;
                objobjMExpense = objModelTravel.GetTravelModedatasingle(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), "E", id).ToList();
                objMExpenseEdit.OUCode = objobjMExpense[0].OUCode.ToString();
                objMExpenseEdit.ProductCode = objobjMExpense[0].ProductCode.ToString();
                objMExpenseEdit.ClaimPeriodFrom = objobjMExpense[0].ClaimPeriodFrom.ToString();
                objMExpenseEdit.ClaimPeriodTo = objobjMExpense[0].ClaimPeriodTo.ToString();
                objMExpenseEdit.FC = objobjMExpense[0].FC.ToString();
                objMExpenseEdit.CC = objobjMExpense[0].CC.ToString();

                objMExpenseEdit.Exp_FCC = new SelectList(objModelTravel.fcdata().ToList(), "raiserfcId", "raiserfcName", objobjMExpense[0].FC.ToString());
                objMExpenseEdit.Exp_CCC = new SelectList(objModelTravel.ccdata().ToList(), "raiserccId", "raiserccName", objobjMExpense[0].CC.ToString());
                objMExpenseEdit.Exp_FCCC = objobjMExpense[0].Exp_FCCC.ToString();

                objMExpenseEdit.travelDescription = objobjMExpense[0].travelDescription.ToString();
                objMExpenseEdit.Amount = objobjMExpense[0].Amount.ToString();

                objMExpenseEdit.ExpNatureofExpdata = new SelectList(objModelTravel.NatureofExpensesdataother().ToList(), "NatureofExpensesId", "NatureofExpensesName", objobjMExpense[0].NatureofExpensesId.ToString());
                objMExpenseEdit.ExpCatdata = new SelectList(objModelTravel.ExpenseCategorydata(objobjMExpense[0].NatureofExpensesId).ToList(), "ExpenseCategoryId", "ExpenseCategoryName", objobjMExpense[0].ExpenseCategoryId.ToString());
                objMExpenseEdit.ExpSubCatdata = new SelectList(objModelTravel.SubCategorydata(objobjMExpense[0].ExpenseCategoryId).ToList(), "SubCategoryId", "SubCategoryName", objobjMExpense[0].SubCategoryId.ToString());
                objMExpenseEdit.Citydata = new SelectList(objModelTravel.tTravelcitydata().ToList(), "TravelCityName", "TravelCityName", objobjMExpense[0].PlaceFrom.ToString());

                Session["Ecfamountvaltmeditoe"] = Convert.ToDecimal(Session["Ecfamountvaloe"].ToString()) + Convert.ToDecimal(objobjMExpense[0].Amount.ToString());
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
        public JsonResult _TravelModeEdit(EOW_TravelClaim TravelClaimModel)
        {
            string message = "";
            try
            {
                message = objModelTravel.UpdateTravelModeCreate(TravelClaimModel, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), Session["SelfModeRaiseid"].ToString(), Session["TravelModeactiverow"].ToString());
                ViewBag.SearchItems = null;
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult _OtherTravelExpenseEdit(EOW_TravelClaim TravelClaimModel)
        {
            string message = "";
            try
            {
                message = objModelTravel.UpdateOtherTravelCreate(TravelClaimModel, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), Session["SelfModeRaiseid"].ToString(), Session["OtherTravelactiverow"].ToString());
                ViewBag.SearchItems = null;
                return Json(message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult DeleteTravelMode(EOW_TravelClaim TravelClaimmodel)
        {
            int TravelModeGID = (int)TravelClaimmodel.TravelMode_GID;
            string delamt = objModelTravel.DeleteTravelExpense(TravelModeGID, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString());
            return Json(TravelClaimmodel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteOtherTravelExpense(EOW_TravelClaim TravelClaimmodel)
        {
            int TravelModeGID = (int)TravelClaimmodel.TravelMode_GID;
            string delamt = objModelTravel.DeleteTravelExpense(TravelModeGID, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString());
            return Json(TravelClaimmodel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteEmployeelsts(EOW_Employeelst TravelClaimmodel)
        {
            int TravelModeGID = (int)TravelClaimmodel.employeeGid;
            string delamt = objModelTravel.DeleteEmployeelst(TravelModeGID, Session["EcfGid"].ToString());
            return Json(TravelClaimmodel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult _EmpECFCreate(EOW_EmployeeeExpense EmployeeeExpenseModel)
        {
            string Err = "";
            try
            {
                //decimal pay = Convert.ToDecimal(Session["Ecfamountpaymenttt"].ToString());
                //decimal expmode = Convert.ToDecimal(Session["Ecfamountvaltm"].ToString());
                //decimal expother = Convert.ToDecimal(Session["Ecfamountvaloe"].ToString());

                decimal ecfamt = Convert.ToDecimal(Session["Ecfamountvalmain"].ToString());
                decimal expmode = Convert.ToDecimal(Session["Ecfrowtolamt"].ToString());
                expmode = ecfamt;
                //decimal expother = Convert.ToDecimal(Session["Ecfrowtolamt"].ToString());

                //decimal toleee = expmode + expother;
                var result = objModelTravel.GetEmployeePayModeEraAcc(Convert.ToInt32(Session["SelfModeRaiseid"]));
                if (expmode != ecfamt)
                {
                    Err = "Mode";
                }
                else if (result == "No")
                {
                    Err = "BankAcc";
                }
                //else if (expother != 0)
                //{
                //    Err = "Other";
                //}

                //else if (pay < 0)
                //{
                //    Err = "Payment";
                //}
                else
                {
                    if (Session["loginraisemode"] != null)
                    {
                        Err = objModelTravel.Insertecfdraftproxcy(EmployeeeExpenseModel, Session["EcfGid"].ToString());
                        Err = "Proxy";
                    }
                    else
                    {
                        Err = objModelTravel.Insertecf(EmployeeeExpenseModel, Session["EcfGid"].ToString(), "0", objCmnFunctions.GetLoginUserGid().ToString(), Session["SelfModeRaiseid"].ToString(), "T", String.IsNullOrEmpty(Session["QueueGide"].ToString()) ? "" : Session["QueueGide"].ToString());
                        if (Err == "")
                        {
                            Err = "Error";
                        }
                        else if(Err.ToString().Contains("ECF Number"))
                        {
                            Err = Err.ToString();
                            Session["EcfGid"] = null;
                            Session["invoiceGid"] = null;
                            Session["Ecfamountpaymenttt"] = null;
                            Session["Ecfamountval"] = null;
                            Session["QueueGide"] = null;
                        }
                    }
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
        [HttpPost]
        public JsonResult _EmpECFDraft(EOW_EmployeeeExpense EmployeeeExpenseModel)
        {
            string Err = "";
            try
            {
                Err = objModelTravel.Insertecfdraft(EmployeeeExpenseModel, Session["EcfGid"].ToString());
                if (Err == "")
                {
                    Err = "Error";
                }
                else
                {
                    Err = "Success";
                    Session["EcfGid"] = null;
                    Session["invoiceGid"] = null;
                    Session["Ecfamountpaymenttt"] = null;
                    Session["Ecfamountvaloe"] = null;
                    Session["Ecfamountvaltm"] = null;
                    Session["QueueGide"] = null;
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
        [HttpGet]
        public PartialViewResult _Emppolicypopup(string id)
        {
            Session["policyhelp"] = null;
            if (Session["policyhelp"] == null)
            {
                string policy = "";
                policy = objModelTravel.selectpolicy(id.ToString());
                Session["policyhelp"] = policy;
            }
            List<EOW_RefNo> lst = new List<EOW_RefNo>();
            //lst = objModel.EmployeeePaymentRefNodatagri(Session["SelfModeRaiseid"].ToString()).ToList();
            return PartialView(lst);
        }
        [HttpPost]
        public JsonResult ChangeBussinessSegment(EOW_TravelClaim EmployeeeExpense)
        {
            List<EOW_TravelClaim> objobjMExpense = new List<EOW_TravelClaim>();
            objobjMExpense = objModelTravel.ChangeBussinessSegment(EmployeeeExpense.OUCode).ToList();
            return Json(objobjMExpense, JsonRequestBehavior.AllowGet);
        }
        #region
        [HttpPost]
        public JsonResult SetInvoiceTravelDetails(string EcfId, string InvId, string ProviderLocation, string GstinVendor, string Suppliergid, string InvNo,
            string InvDate, string Amount, string Desc, string WOTaxAmount, string IsVerify, string IsRemoved, string GstCharged, string ReceiverLocation, string GstinFiccl, string ServiceMonth)
        {
            try
            {
                DataTable dt = new DataTable();
                string Data1 = "", Data2 = "", Data3 = "";
                DataSet ds = db.SetInvoiceTravelDetails(Session["EcfGid"].ToString(), InvId, ProviderLocation, GstinVendor, Suppliergid, InvNo,
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
                        if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }
                    }
                    if (ds.Tables.Count > 2)
                    {
                        dt = ds.Tables[2];
                        if (dt.Rows.Count > 0) { Data3 = JsonConvert.SerializeObject(dt); }
                    }
                }
                //Session["invoicegid"] = inv.InvId;
                return Json(new { Data1, Data2, Data3 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult LoadGstvalues(string InvId)
        {
            try
            {
                DataTable dt = new DataTable();
                string Data1 = "", Data2 = "", Data3 = "";
                DataSet ds = new DataSet();
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
                //Session["invoicegid"] = inv.InvId;
                return Json(new { Data1, Data2, Data3 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
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
        public JsonResult CheckrcmExists(RCMcheck ircm)
        {
            try
            {
                string Data1 = "";
                string ECFID = Session["EcfGid"].ToString();
                string InvoiceID = Session["invoiceGid"].ToString(); 
                string hsngid = ircm.hsngid;
                string action = ircm.action;
                DataSet ds = db.CheckrcmExists(Session["EcfGid"].ToString(), ircm.DebitlineGId, Session["invoiceGid"].ToString(), ircm.hsngid, "0", ircm.action);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }


                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        public JsonResult LoadInvoiceHeaderDetails(string InvId)
        {
            try
            {
                DataTable dt = new DataTable();
                string Data1 = "", Data2 = "", Data3 = "";
                DataSet ds = db.LoadInvoiceHeaderDetails(Convert.ToString(Session["EcfGid"]), InvId);
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

        #endregion
    }
}
