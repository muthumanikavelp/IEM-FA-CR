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
using System.Configuration;
using System.Data;
using IEM.App_Start;
namespace IEM.Areas.EOW.Controllers
{

    [NoDirectAccess]
    public class RaisingInsuranceController : Controller
    {
        //
        // GET: /EOW/RaisingInsurance/
        private ArfInsRepository objModel;
       
        EOW_DataModel EOW_DataModels = new EOW_DataModel();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        private FileServer Cmnfs = new FileServer();
        public RaisingInsuranceController()
            : this(new ArfInsRaising())
        {

        }
        public RaisingInsuranceController(ArfInsRepository objM)
        {
            objModel = objM;
        }
        public ActionResult Index()
        {
            Session["QueueGide"] = "";
            if (Session["DashBoard"] != null)
            {
                EOW_EmployeeeExpense eowemp = new EOW_EmployeeeExpense();
                eowemp = (EOW_EmployeeeExpense)Session["DashBoard"];


                Session["ecf_gid"] = eowemp.ecf_GID.ToString();
                Session["invoiceGid"] = eowemp.invoice_GID.ToString();

                if (eowemp.Queue_GID != 0)
                {
                    Session["QueueGide"] = eowemp.Queue_GID.ToString();
                }
                else
                {
                    Session["QueueGide"] = "";
                }
                ViewBag.process = "datapost";
                ViewBag.message = "datapost";
                Session["Draft"] = "Draft";
                //   Session["Ecfamountvalmain"] = eowemp.ECF_Amount;
                if (eowemp.raisermodeId == "P")
                {
                    Session["SelfModeRaiseid"] = objCmnFunctions.GetLoginProxyUserGid().ToString();
                }
                else
                {
                    Session["SelfModeRaiseid"] = objCmnFunctions.GetLoginUserGid().ToString();
                }
                EOW_EmployeeeExpense eowemp1 = new EOW_EmployeeeExpense();
                if (eowemp.SubCategoryName == "E" || eowemp.SubCategoryName == "P")
                {
                    eowemp1 = objModel.empraiserdetails(Session["ecf_gid"].ToString());
                    eowemp.Raiser_Mode = Convert.ToString(eowemp1.Raiser_Mode);
                }
                else
                {
                    eowemp1 = objModel.supraiserdetails(Session["ecf_gid"].ToString());
                    eowemp.Raiser_Mode = (eowemp1.Raiser_Mode);
                }
                Session["invoiceGid"] = eowemp1.supplierheader_ggid;
                eowemp.ecfremark = eowemp1.ecfremark;
                Session["supname"] = eowemp1.ecf_raisername;
                eowemp.ecf_raisername = eowemp1.ecf_raisername;
                Session["supplierheader_suppliercode"] = eowemp1.Raiser_Mode;
                Session["employeesupplier"] = eowemp.SubCategoryName;

                Session["Supplierecfid"] = eowemp1.supplierheader_ggid;
                Session["Supplierecfname"] = eowemp1.ecf_raisername;

                if (eowemp.SubCategoryName == "S")
                {
                    eowemp.SubCategoryName = "Supplier";
                }
                if (eowemp.SubCategoryName == "E")
                {
                    eowemp.SubCategoryName = "Employee";
                }
                if (eowemp.SubCategoryName == "P")
                {
                    eowemp.SubCategoryName = "Petty Cash";
                }
                if (eowemp.SubCategoryName == "I")
                {
                    eowemp.SubCategoryName = "Insurance";
                }
                Session["DashBoard"] = null;
                Session["RequestAmount"] = eowemp.ECF_Amount;
                EOW_ArfInsuranceraising TypeModel = objModel.advanceamount(Session["ecf_gid"].ToString());
                Session["Ecfamountpayment1"] = TypeModel.ecf_amount;
                EOW_ArfInsuranceraising TypeModel1 = objModel.payamount(Session["ecf_gid"].ToString());
                if (eowemp.SubCategoryName != "E" || eowemp.SubCategoryName == "P")
                {
                    Session["Ecfamountval"] = eowemp.ECF_Amount;
                }
                else
                {
                    Session["Ecfamountval"] = eowemp.ECF_Amount;
                }
                EOW_ArfInsuranceraising TypeModeldes = objModel.ecf_desc(Session["ecf_gid"].ToString());
                Session["ecfarf_desc"] = TypeModeldes.ecfarf_desc;
                Session["EcfEmpGid"] = Session["employee_gid"].ToString();
                Session["vldata"] = "vldata";
                eowemp.Exp_Amount = objCmnFunctions.GetINRAmount(eowemp.ECF_Amount);
                ViewBag.EOW_EmployeeeExpenseheader = eowemp;
            }
            else
            {
                Session["vldata"] = "";
                ViewBag.process = "data";
                ViewBag.message = "";
                EOW_ArfInsuranceraising eowemp = new EOW_ArfInsuranceraising();
                eowemp = objModel.empdetails(Session["employee_gid"].ToString());
                eowemp.grade = eowemp.grade;
                eowemp.ecf_raiser = eowemp.ecf_raiser_gid;
                eowemp.ecf_raisername = Session["employee_name"].ToString();
                ViewBag.EOW_EmployeeeExpenseheader = eowemp;
                Session["EcfGid"] = null;
                Session["invoiceGid"] = null;
                Session["Ecfamountpayment1"] = null;
                Session["Ecfamountval"] = null;
                Session["DashBoard"] = null;
                Session["ecf_gid"] = "";

            }
            List<EOW_EmployeeeExpense> objmaindetaild = new List<EOW_EmployeeeExpense>();
            objmaindetaild = EOW_DataModels.SelectEmployeeeheader(objCmnFunctions.GetLoginUserGid().ToString()).ToList();
            Session["raiserfcc"] = objmaindetaild[0].Exp_FC.ToString();
            Session["raiserccc"] = objmaindetaild[0].Exp_CC.ToString();
            Session["Ecfdeclaratonnote"] = objModel.GetDecnote("12", "S").ToString();
            EOW_ArfInsuranceraising arfinsadvance = new EOW_ArfInsuranceraising();
            ViewBag.viewprocess = "";
            arfinsadvance = objModel.empdetails(Session["employee_gid"].ToString());
            return View(arfinsadvance);

        }
       
        [HttpGet]
        public PartialViewResult SupplierSearch(string listfor)
        {
            List<EOW_ArfInsuranceraising> obj = new List<EOW_ArfInsuranceraising>();
            EOW_ArfInsuranceraising emp = new EOW_ArfInsuranceraising();
            if (Session["Suppliername"] != null)
            {
                ViewBag.SupplierName = Session["Suppliername"].ToString();
            }
            if (Session["Suppliercode"] != null)
            {
                ViewBag.SupplierCode = Session["Suppliercode"].ToString();
            }
            if (listfor == "search")
            {
                if (Session["Searchdata"] != null)
                {
                    obj = (List<EOW_ArfInsuranceraising>)Session["Searchdata"];
                }
            }
            else
            {
                 obj = objModel.SelectSupplier().ToList();
            }
            return PartialView(obj);
        }


       
        [HttpPost]
        public PartialViewResult SupplierSearchECF11(string SupplierName = "", string SupplierCode = "")
        {
            List<EOW_ArfInsuranceraising> obj = new List<EOW_ArfInsuranceraising>();
            EOW_ArfInsuranceraising emp = new EOW_ArfInsuranceraising();
            if (SupplierCode == "" && SupplierName == "")
            {
                obj = objModel.SelectSupplier().ToList();
            }
            else
            {
                obj = objModel.SelectSupplierSearch(SupplierName, SupplierCode).ToList();
                if (obj != null)
                {

                    ViewBag.SupplierCode = SupplierCode;
                    ViewBag.SupplierName = SupplierName;
                    if (obj.Count == 0)
                    {
                        ViewBag.Message = "No Record Found !";
                    }

                }
            }

            return PartialView("SupplierSearch", obj);
        }
        public ActionResult Details(int id)
        {
            return View();
        }
        [HttpGet]
        public PartialViewResult ArfInsHeader()
        {
            ViewBag.process = "Postdata";
            ViewBag.message = "Postdata";
            ViewBag.binddata = "datapost";
            if (Session["vldata"] != "vldata")
            {
                ViewBag.arfdate = "arfdata";
            }
            return PartialView();
        }

        [HttpGet]
        public PartialViewResult advancepop(string id, string viewfor, string arftype)
        {

            if (viewfor == "edit")
            {
                ViewBag.viewfor = "edit";
            }
            else if (viewfor == "view")
            {
                ViewBag.viewfor = "view";
            }
            else if (viewfor == "Delete")
            {
                ViewBag.viewfor = "Delete";
            }
            EOW_ArfInsuranceraising TypeModel = objModel.empdetails(Session["employee_gid"].ToString());
            if (id != null)
            {
                TypeModel = objModel.empdetailsadvance_id(id);
                TypeModel.GetAdvancetype = new SelectList(objModel.GetAdvancetype(arftype), "advancetype_gid", "advancetype_name", TypeModel.ecfarf_advancetype_gid);
            }
            else
            {
                TypeModel.GetAdvancetype = new SelectList(objModel.GetAdvancetype(arftype), "advancetype_gid", "advancetype_name");
            }
            TypeModel.Getfc = new SelectList(EOW_DataModels.fcdata().ToList(), "raiserfcId", "raiserfcName", Session["raiserfcc"].ToString());
            TypeModel.Getcc = new SelectList(EOW_DataModels.ccdata().ToList(), "raiserccId", "raiserccName", Session["raiserccc"].ToString());
            return PartialView(TypeModel);
        }
        [HttpGet]
        public PartialViewResult paymentpop(string id, string viewfor)
        {
            if (viewfor == "edit")
            {
                ViewBag.viewfor = "edit";
            }
            else if (viewfor == "view")
            {
                ViewBag.viewfor = "view";
            }
            else if (viewfor == "Delete")
            {
                ViewBag.viewfor = "Delete";
            }
            EOW_ArfInsuranceraising TypeModel = new EOW_ArfInsuranceraising();
            if (id != null)
            {
                TypeModel = objModel.empdetailspay_id(id);
                TypeModel.GetPaymode = new SelectList(objModel.GetPaymode(Session["employeesupplier"].ToString(),Session["Supplierecfid"].ToString()), "paymode_gid", "paymode_name", TypeModel.creditline_pay_mode);
                TypeModel.GetRef = new SelectList(objModel.GetRefant(Session["employeesupplier"].ToString()), "payment_accountno", "payment_accountno", TypeModel.creditline_ref_no);
            }
            else
            {
                TypeModel.GetPaymode = new SelectList(objModel.GetPaymode(Session["employeesupplier"].ToString(), Session["Supplierecfid"].ToString()), "paymode_gid", "paymode_name");
                TypeModel.GetRef = new SelectList(objModel.GetRefant(Session["employeesupplier"].ToString()), "payment_accountno", "payment_accountno");
            }
            return PartialView(TypeModel);
        }
        [HttpGet]
        public PartialViewResult attachpop()
        {
            return PartialView();
        }
        public JsonResult proxylogin(EOW_ArfInsuranceraising proxy)
        {
            try
            {
                string ExpCat_gid = Convert.ToString(proxy.ecf_employee_gid);
                EOW_ArfInsuranceraising TypeModel = new EOW_ArfInsuranceraising();
                TypeModel = objModel.proxy(ExpCat_gid);
                return Json(TypeModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult ArfInsAdvance(int id, string viewfor, string type)
        {
            if (id != 0 && viewfor != "")
            {
                if (viewfor == "edit")
                {
                    ViewBag.viewfor = "edit";
                }
                else if (viewfor == "view")
                {
                    ViewBag.viewfor = "view";
                }
                else if (viewfor == "Delete")
                {
                    ViewBag.viewfor = "Delete";
                }
                EOW_ArfInsuranceraising TypeModel = objModel.SelectAdvanceById(id);
                TypeModel.GetAdvancetype = new SelectList(objModel.GetAdvancetype(type), "advancetype_gid", "advancetype_name", TypeModel.ecfarf_advancetype_gid);
                TypeModel = objModel.empdetails(Session["employee_gid"].ToString());
                return PartialView("ArfInsAdvance", TypeModel);
            }
            else
            {
                EOW_ArfInsuranceraising TypeModel = objModel.empdetails(Session["employee_gid"].ToString());
                return PartialView(TypeModel);
            }

        }
        [HttpPost]
        public JsonResult Getdelmatdata()
        {
            try
            {
                List<EOW_ArfInsuranceraising> lst = new List<EOW_ArfInsuranceraising>();
                DataTable GetflowgidCount = new DataTable();
                ArfInsRaising rais = new ArfInsRaising();
                GetflowgidCount = rais.SelectAdvance(Session["ecf_gid"].ToString());
                if (GetflowgidCount.Rows.Count > 0)
                {
                    foreach (DataRow dr in GetflowgidCount.Rows)
                    {

                        lst.Add(
                        new EOW_ArfInsuranceraising
                        {
                            ecfarf_gid = Convert.ToInt32(dr["ecfarf_gid"].ToString()),
                            ecfarf_ecf_gid = Convert.ToInt32(dr["ecfarf_ecf_gid"].ToString()),
                            ecfarf_advancetype = Convert.ToString(dr["ecfarf_advancetype_gid"].ToString()),
                            ecfarf_desc = Convert.ToString(dr["ecfarf_desc"]),
                            ecfarf_liq_date = Convert.ToString(dr["ecfarf_liq_date"].ToString()),
                            ecfarf_pi_gl_no = Convert.ToString(dr["ecfarf_pi_gl_no"].ToString()),
                            ecfarf_po_no = Convert.ToString(dr["ecfarf_po_no"].ToString()),
                            ecfarf_cbf_no = Convert.ToString(dr["ecfarf_cbf_no"].ToString()),
                            ecfarf_fc_code = Convert.ToString(dr["ecfarf_fc_code"].ToString()),
                            ecfarf_cc_code = Convert.ToString(dr["ecfarf_cc_code"].ToString()),
                            ecfarf_product_code = Convert.ToString(dr["ecfarf_product_code"].ToString()),
                            ecfarf_ou_code = Convert.ToString(dr["ecfarf_ou_code"]),
                            ecfarf_amount = objCmnFunctions.GetINRAmount(dr["ecfarf_amount"].ToString())

                        });

                    };
                }
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult Getsupplier(EOW_Employeelst EmployeeeExpensemodel)
        {
            Session["Supplierecfid"] = EmployeeeExpensemodel.employeeGid;
            Session["Supplierecfname"] = EmployeeeExpensemodel.empName;

            return Json(EmployeeeExpensemodel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Getpaymentdata()
        {
            List<EOW_ArfInsuranceraising> lst = new List<EOW_ArfInsuranceraising>();
            DataTable GetflowgidCount = new DataTable();
            ArfInsRaising rais = new ArfInsRaising();
                string result = "";
                if (Convert.ToString(Session["ecf_gid"].ToString()) != "")
                {
                    string exits = objModel.GetStatusexcel(Convert.ToString(Session["ecf_gid"].ToString()), "arfpaymentchk");
                    if (exits == "Exists")
                    {
                        GetflowgidCount = objModel.Getpaymodedata(Convert.ToString(Session["ecf_gid"].ToString()), "arfpaymentdata");

                        lst.Add(
                      new EOW_ArfInsuranceraising
                      {
                          creditline_gid = Convert.ToInt32(GetflowgidCount.Rows[0]["creditline_gid"].ToString()),
                          creditline_pay_mode = Convert.ToString(GetflowgidCount.Rows[0]["ecfcreditline_pay_mode"].ToString()),
                          creditline_ref_no = Convert.ToString(GetflowgidCount.Rows[0]["ecfcreditline_ref_no"].ToString()),
                          creditline_beneficiary = Convert.ToString(GetflowgidCount.Rows[0]["ecfcreditline_beneficiary"].ToString()),
                          creditline_desc = Convert.ToString(GetflowgidCount.Rows[0]["ecfcreditline_desc"].ToString()),
                          creditline_amount = objCmnFunctions.GetINRAmount(GetflowgidCount.Rows[0]["ecfcreditline_amount"].ToString())
                      });
                    }
                    else
                    {
                        result = objModel.insertsuplierpayment(Session["Supplierecfid"].ToString(), Session["ecf_gid"].ToString(), Session["RequestAmount"].ToString(), Session["Supplierecfname"].ToString());

                        GetflowgidCount = objModel.Getpaymodedata(Convert.ToString(Session["ecf_gid"].ToString()), "arfpaymentdata");

                        lst.Add(
                      new EOW_ArfInsuranceraising
                      {
                          creditline_gid = Convert.ToInt32(GetflowgidCount.Rows[0]["creditline_gid"].ToString()),
                          creditline_pay_mode = Convert.ToString(GetflowgidCount.Rows[0]["ecfcreditline_pay_mode"].ToString()),
                          creditline_ref_no = Convert.ToString(GetflowgidCount.Rows[0]["ecfcreditline_ref_no"].ToString()),
                          creditline_beneficiary = Convert.ToString(GetflowgidCount.Rows[0]["ecfcreditline_beneficiary"].ToString()),
                          creditline_desc = Convert.ToString(GetflowgidCount.Rows[0]["ecfcreditline_desc"].ToString()),
                          creditline_amount = objCmnFunctions.GetINRAmount(GetflowgidCount.Rows[0]["ecfcreditline_amount"].ToString())
                      });
                    }
                }
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Getattachdata()
        {
            List<EOW_ArfInsuranceraising> lst = new List<EOW_ArfInsuranceraising>();
            DataTable GetflowgidCount = new DataTable();
            ArfInsRaising rais = new ArfInsRaising();
            GetflowgidCount = rais.Selectattach(Session["ecf_gid"].ToString());
            foreach (DataRow row in GetflowgidCount.Rows)
            {

                lst.Add(
                new EOW_ArfInsuranceraising
                {
                    attachment_gid = Convert.ToInt32(row["attachment_gid"].ToString()),
                    attachment_filename = Convert.ToString(row["attachment_filename"]),
                    attachment_type = Convert.ToString(row["attachment_attachmenttype_gid"].ToString()),
                    attachment_desc = Convert.ToString(row["attachment_desc"]),
                    attachment_nameby = Convert.ToString(row["attachment_by"]),
                    attachment_date = Convert.ToString(row["attachment_date"].ToString())

                });

            };
            return Json(lst, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult arfpolicy(string id)
        {
            Session["policyhelp1"] = null;
            if (Session["policyhelp1"] == null)
            {
                string policy = "";
                policy = objModel.selectpolicy1(id.ToString());
                Session["policyhelp1"] = policy;
            }
            List<EOW_RefNo> lst = new List<EOW_RefNo>();
            return PartialView(lst);
        }
        [HttpPost]
        public JsonResult GetSubCategorypolicy(EOW_ArfInsuranceraising EmployeeeExpense)
        {
            string policy = "";
            policy = objModel.selectpolicy1(EmployeeeExpense.ecfarf_advancetype_gid.ToString());
            Session["policyhelp1"] = policy;
            return Json(policy, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult ArfInsPayment(int id, string viewfor)
        {
            if (id != 0 && viewfor != "")
            {
                if (viewfor == "edit1")
                {
                    ViewBag.viewfor = "edit1";
                }
                else if (viewfor == "view1")
                {
                    ViewBag.viewfor = "view1";
                }
                else if (viewfor == "Delete1")
                {
                    ViewBag.viewfor = "Delete1";
                }
                EOW_ArfInsuranceraising TypeModel = objModel.SelectpaymentById(id);
                TypeModel.GetPaymode = new SelectList(objModel.GetPaymode(Session["employeesupplier"].ToString(), Session["Supplierecfid"].ToString()), "paymode_gid", "paymode_name", TypeModel.creditline_pay_mode);
                TypeModel.GetRef = new SelectList(objModel.GetRefant(Session["employeesupplier"].ToString()), "payment_accountno", "payment_accountno", TypeModel.creditline_ref_no);
                TypeModel.GetBenificary = new SelectList(objModel.GetBenificary(), "payment_benificary", "payment_benificary", TypeModel.creditline_beneficiary);
                return PartialView(TypeModel);
            }
            else
            {
                EOW_ArfInsuranceraising arfinsadvance = new EOW_ArfInsuranceraising();

                return PartialView(arfinsadvance);
            }
        }
        [HttpPost]
        public JsonResult _EmpARFCreatedraft(EOW_ArfInsuranceraising EmployeeeExpenseModel)
        {
            string Err = "";
            try
            {
                Err = objModel.Insertecfderaft(EmployeeeExpenseModel, Session["ecf_gid"].ToString());
                return Json(Err, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult _EmpARFCreate(EOW_ArfInsuranceraising EmployeeeExpenseModel)
        {
            EOW_DataModel EOW_DataModels = new EOW_DataModel();
            string Err = "";
            try
            {
                if (ModelState.IsValid)
                {
                    string reamnt = Convert.ToString(Session["RequestAmount"]);
                    string payamt = Convert.ToString(Session["Ecfamountpayment1"]);
                    string expamt = Convert.ToString(Session["Ecfamountval"]);
                    if (reamnt != "" && payamt != "" && expamt != "")
                    {
                        decimal reamount = Convert.ToDecimal(Session["RequestAmount"].ToString());
                        decimal pay = Convert.ToDecimal(Session["Ecfamountpayment1"].ToString());
                        decimal exp = Convert.ToDecimal(Session["Ecfamountval"].ToString());
                        //decimal ecf = Convert.ToInt32(Session["Ecfamountvalmain"].ToString());
                        var result = "";
                        if (Convert.ToString(Session["employeesupplier"]) == "E")
                        {
                            result = objModel.GetEmployeePayModeEraAcc(Convert.ToInt32(Session["EcfEmpGid"]));
                        }
                        else if (Convert.ToString(Session["employeesupplier"]) == "I")
                        {
                            result = objModel.GetSupplierBankDetailsBypayMode(Convert.ToString(Session["ecf_gid"]));
                        }
                        if (pay != reamount)
                        {
                            Err = "Expense";
                        }
                        else if (exp != reamount)
                        {
                            Err = "Payment";
                        }
                        else if (result=="No")
                        {
                            Err = "BankAcc";
                        }
                        else
                        {
                            string ecfgid = Convert.ToString(Session["ecf_gid"]);
                            if (ecfgid != "")
                            {
                                EmployeeeExpenseModel.ecf_ecf_gid = Convert.ToString(Session["ecf_gid"].ToString());
                                Err = objModel.Insertecf(EmployeeeExpenseModel, Session["EcfEmpGid"].ToString(), Session["invoiceGid"].ToString(), Session["QueueGide"].ToString());
                                if (Err == "delmat")
                                {
                                    Err = "Please check delmat and then proceed";
                                }

                                else
                                {
                                    Err = Err.ToString();
                                    Session["QueueGide"] = null;
                                    Session["EcfGid"] = null;
                                    Session["invoiceGid"] = null;
                                    Session["Ecfamountpayment1"] = null;
                                    Session["Ecfamountval"] = null;
                                }
                            }
                            else
                            {
                                Err = "Please save Request and Advance & Payment Entries and Than Proceed";

                            }

                        }
                    }
                    else
                    {
                        Err = "Please save Request and Advance & Payment Entries and Than Proceed";
                    }

                }
                ViewBag.SearchItems = null;
                return Json(Err, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult ArfInsAttachement()
        {

            return PartialView();
        }
        [HttpGet]
        public PartialViewResult pohead()
        {
            List<EOW_ArfInsuranceraising> TypeModel = new List<EOW_ArfInsuranceraising>();
            TypeModel = objModel.pono(Session["invoiceGid"].ToString()).ToList();
            return PartialView(TypeModel);
        }
        [HttpPost]
        public PartialViewResult poheadref(EOW_ArfInsuranceraising pouchmdl)
        {
            List<EOW_ArfInsuranceraising> TypeModel = new List<EOW_ArfInsuranceraising>();
            TypeModel = objModel.pono(Session["invoiceGid"].ToString()).ToList();
            return PartialView("pohead", TypeModel);
        }
        [HttpGet]
        public PartialViewResult CBF()
        {
            List<EOW_ArfInsuranceraising> TypeModel = new List<EOW_ArfInsuranceraising>();
            TypeModel = objModel.cbfno(Session["invoiceGid"].ToString()).ToList();
            return PartialView(TypeModel);
        }
        [HttpPost]
        public PartialViewResult CBFref(EOW_ArfInsuranceraising pouchmdl)
        {
            List<EOW_ArfInsuranceraising> TypeModel = new List<EOW_ArfInsuranceraising>();
            TypeModel = objModel.cbfno(Session["invoiceGid"].ToString()).ToList();
            return PartialView("CBF", TypeModel);
        }
        public JsonResult GetRefamount(EOW_ArfInsuranceraising request)
        {
            String result = String.Empty;
            EOW_ArfInsuranceraising rais = new EOW_ArfInsuranceraising();
            rais.GetRef = new SelectList(objModel.GetRefant(Session["employeesupplier"].ToString()), "payment_accountno", "payment_accountno", rais.creditline_ref_no);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveRequest(EOW_ArfInsuranceraising request)
        {
            Session["RequestAmount"] = request.ecf_amount;
            if (request.arf_type == "I" )
            {
                Session["employeesupplier"] = "I";
            }
            Session["ecfarf_desc"] = request.ecfarf_desc;
            Session["supname"] = request.supplierheader_name;
            Session["Ecfamountval"] = request.ecf_amount;
            String result = String.Empty;
            Session["supplierheader_suppliercode"] = request.supplierheader_suppliercode;
            request.ecf_ecf_gid = Convert.ToString(Session["ecf_gid"]);
            result = objModel.SaveRequestval(request);
            if (result != "Can't Proceed This Login Employee")
            {
                Session["EcfEmpGid"] = string.IsNullOrEmpty(objCmnFunctions.GetLoginUserGid().ToString()) ? "" : objCmnFunctions.GetLoginUserGid().ToString();
                Session["invoiceGid"] = string.IsNullOrEmpty(objCmnFunctions.GetLoginUserGid().ToString()) ? "" : objCmnFunctions.GetLoginUserGid().ToString();
                Session["ecf_gid"] = result;
                if (result != "")
                {
                    result = "NotExists";
                }
            }
            else
            {
                result = "Can't Proceed This Login Employee";
            }
            ViewBag.viewprocess = "advance";
            ViewBag.arfdate = "arfdata";
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveAdvance(EOW_ArfInsuranceraising advance)
        {
            String result = String.Empty;
            if (Convert.ToDecimal(advance.ecfarf_amount) <= Convert.ToDecimal(Session["RequestAmount"].ToString()))
            {
                Session["Ecfamountpayment1"] = advance.ecfarf_amount.ToString();
                EOW_ArfInsuranceraising TypeModel = objModel.advanceamount(Session["ecf_gid"].ToString());
                Decimal adamount = Convert.ToDecimal(TypeModel.ecf_amount) + Convert.ToDecimal(advance.ecfarf_amount);
                if (adamount <= Convert.ToDecimal(Session["RequestAmount"].ToString()))
                {

                    string ecfgid = Convert.ToString(Session["ecf_gid"]);
                    if (ecfgid != "")
                    {
                        advance.ecf_ecf_gid = Convert.ToString(Session["ecf_gid"].ToString());
                        result = objModel.SaveAdvance(advance);
                        EOW_ArfInsuranceraising TypeModel1 = objModel.advanceamount(Session["ecf_gid"].ToString());
                        Session["Ecfamountpayment1"] = TypeModel1.ecf_amount;

                    }
                    else { result = "Please Save Advance Request And Then Procceed"; }
                }
                else
                {
                    result = "Advance Amount Equal To Request Amount";
                }
            }
            else
            {
                result = "Advance Amount Equal To Request Amount.";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateAdvance(EOW_ArfInsuranceraising advance)
        {
            String result = String.Empty;
            decimal amt = Convert.ToDecimal(advance.ecfarf_amount);
            string payamount = Convert.ToString(Session["RequestAmount"]);
            if (Convert.ToDecimal(payamount) >= amt)
            {
                result = objModel.UpdatAdvance(advance);
                EOW_ArfInsuranceraising TypeModel1 = objModel.advanceamount(Session["ecf_gid"].ToString());
                Session["Ecfamountpayment1"] = TypeModel1.ecf_amount;
            }
            else
            {
                result = "Advance Amount Equal To Request Amount";
            }
            return Json(result, JsonRequestBehavior.AllowGet);


        }
        [HttpPost]
        public JsonResult DeleteAdvance(EOW_ArfInsuranceraising advance)
        {
            String result = String.Empty;
            result = objModel.DeleteAdvance(advance);
            if (Convert.ToString(Session["Ecfamountpayment1"]) != "")
            {
                decimal blamt = Convert.ToDecimal(Session["Ecfamountpayment1"].ToString());
                blamt = blamt - Convert.ToDecimal(advance.ecfarf_amount);
                Session["Ecfamountpayment1"] = Convert.ToString(blamt);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SavePayment(EOW_ArfInsuranceraising Payment)
        {
            String result = String.Empty;
            if (Convert.ToDecimal(Session["Ecfamountval"].ToString()) != Convert.ToDecimal(Session["RequestAmount"].ToString()))
            {
                EOW_ArfInsuranceraising TypeModel1 = objModel.payamount(Session["ecf_gid"].ToString());
                Decimal payamount = Convert.ToDecimal(TypeModel1.ecf_amount) + Convert.ToDecimal(Payment.creditline_amount);
                if (payamount <= Convert.ToDecimal(Session["RequestAmount"].ToString()))
                {
                    string ecfgid = Convert.ToString(Session["ecf_gid"]);
                    if (ecfgid != "")
                    {
                        Payment.creditline_ecf_gid = Convert.ToString(Session["ecf_gid"].ToString());
                        result = objModel.Savepayment(Payment);
                        EOW_ArfInsuranceraising TypeModel11 = objModel.payamount(Session["ecf_gid"].ToString());
                    }
                    else { result = "Please Save Advance Request And Then Procceed"; }
                }
                else
                {
                    result = "Payment Amount Equal To Request Amount";
                }
            }
            else
            {
                result = "Payment Amount Equal To Request Amount";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult updatePayment(EOW_ArfInsuranceraising Payment)
        {
            String result = String.Empty;
            if (Convert.ToDecimal(Payment.creditline_amount) <= Convert.ToDecimal(Session["RequestAmount"].ToString()))
            {
                string ecfgid = Convert.ToString(Session["ecf_gid"]);
                if (ecfgid != "")
                {
                    result = objModel.updatePayment(Payment);
                    EOW_ArfInsuranceraising TypeMode1l = objModel.payamount(Session["ecf_gid"].ToString());
                  
                }
                else { result = "Please Save Advance Request And Then Procceed"; }
               
            }
            else
            {
                result = "Payment Amount Equal To Request Amount";
            }
            return Json(result, JsonRequestBehavior.AllowGet);




        }
        [HttpPost]
        public void UploadFiless(string attach = null, string attaching_format = null) //Pandiaraj 18-11-2019 
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
            catch (Exception e)
            {
                throw e;
            }
        }

        [HttpPost]
        public JsonResult AdvanceType(EOW_ArfInsuranceraising advan, string Type)
        {
            try
            {
                return Json(objModel.GetAdvancetype(Type), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult Refno(EOW_ArfInsuranceraising refno)
        {
            try
            {
                return Json(objModel.GetRefant(Session["employeesupplier"].ToString()), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult Benificary(EOW_ArfInsuranceraising benifi)
        {
            try
            {
                return Json(objModel.GetBenificary(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult Paymode(EOW_ArfInsuranceraising pay)
        {
            try
            {
                string jh = Session["employeesupplier"].ToString();
                return Json(objModel.GetPaymode(jh, Session["Supplierecfid"].ToString()), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult _EmpAttachmentCreate(EOW_ArfInsuranceraising EmployeeeExpenseModel)
        {
            string img = "";
            try
            {
                if (TempData["_attFile"] != null)
                {
                    string ecfgid = Convert.ToString(Session["ecf_gid"]);
                    if (ecfgid != "")
                    {
                        EmployeeeExpenseModel.attachment_by = Convert.ToInt32(Session["EcfEmpGid"].ToString());
                        EmployeeeExpenseModel.attachment_refgid = Convert.ToInt32(ecfgid);

                        HttpPostedFileBase savefile = TempData["_attFile"] as HttpPostedFileBase;
                        string getname = objModel.InsertEmpAtt(savefile, EmployeeeExpenseModel);
                        if (getname != "")
                        {
                           
                            //upload the file to server.
                            HttpPostedFileBase _attFile = TempData["_attFile"] as HttpPostedFileBase;
                            var stream = _attFile.InputStream;
                            string uploaderUrl = string.Format("{0}{1}", HoldFileUploadUrlDSA(), getname + "." + _attFile.FileName.Split('.')[_attFile.FileName.Split('.').Length - 1]);
                           
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
                throw ex;
            }
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
        public FileResult Download(int id)
        {
            string FileName = objModel.downloadAttachment(id);
            string[] str = FileName.Split('.');
            string directory =  id.ToString() + "." + str[str.Length - 1].ToString();
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
        }
        [HttpPost]
        public JsonResult DeleteAttachment(EOW_ArfInsuranceraising attach)
        {
            String result = String.Empty;
            result = objModel.DeleteAttachment(attach.attachment_gid);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult _pono(EOW_ArfInsuranceraising verify)
        {
            string _pono = Convert.ToString(verify.ecfarf_po_no);
            string da = objModel._pono(_pono);

            return Json(da, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult productcode(EOW_ArfInsuranceraising verify)
        {
            string productcode = Convert.ToString(verify.ecfarf_product_code);
            string da = objModel.productcode(productcode);

            return Json(da, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult fccode(EOW_ArfInsuranceraising verify)
        {
            string fccode = Convert.ToString(verify.ecfarf_fc_code);
            string da = objModel.fccode(fccode);
            return Json(da, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult cccode(EOW_ArfInsuranceraising verify)
        {
            string cccode = Convert.ToString(verify.ecfarf_cc_code);
            string da = objModel.cccode(cccode);
            return Json(da, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult oucode(EOW_ArfInsuranceraising verify)
        {
            string oucode = Convert.ToString(verify.ecfarf_ou_code);
            string da = objModel.oucode(oucode);
            return Json(da, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeletePayment(EOW_ArfInsuranceraising Payment)
        {
            String result = String.Empty;
            result = objModel.DeletePayment(Payment.creditline_gid);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult GetEmployeeDetail(string vartextcode, string viewfor)
        {
            string jj = Convert.ToString(Session["EmployeeCode"]);
            List<EOW_ArfInsuranceraising> TypeModel = new List<EOW_ArfInsuranceraising>();
            TypeModel = objModel.GetarfEmployeedetails(jj).ToList();
            Session["EmployeeCode"] = vartextcode.ToString();

            return PartialView(TypeModel);
        }
        [HttpPost]
        public PartialViewResult GetEmployeeDetailvlref(string vartextcode, string viewfor)
        {
            string jj = Convert.ToString(Session["EmployeeCode"]);
            List<EOW_ArfInsuranceraising> TypeModel = new List<EOW_ArfInsuranceraising>();
            TypeModel = objModel.GetarfEmployeedetails(jj).ToList();
            return PartialView("GetEmployeeDetail", TypeModel);
        }
        [HttpPost]
        public PartialViewResult GetEmployeeDetailvl(EOW_ArfInsuranceraising pouchmdl)
        {

            List<EOW_ArfInsuranceraising> objowner = new List<EOW_ArfInsuranceraising>();
            objowner = objModel.GetarfEmployeedetailssearch().ToList();
            if (pouchmdl != null)
            {
                if ((string.IsNullOrEmpty(pouchmdl.employee_code)) == false)
                {
                    ViewBag.employee_code = pouchmdl.employee_code;
                    objowner = objowner.Where(x => pouchmdl.employee_code == null ||
                        (x.employee_code.ToUpper().Contains(pouchmdl.employee_code.ToUpper()))).ToList();
                }
                if ((string.IsNullOrEmpty(pouchmdl.employee_name)) == false)
                {
                    ViewBag.employee_name = pouchmdl.employee_name;
                    objowner = objowner.Where(x => pouchmdl.employee_name == null ||
                        (x.employee_name.ToUpper().Contains(pouchmdl.employee_name.ToUpper()))).ToList();
                }
            }
            if (objowner.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            return PartialView("GetEmployeeDetail", objowner);

        }
        [HttpPost]
        public JsonResult cbf_po(EOW_ArfInsuranceraising verify)
        {
            string POid = Convert.ToString(verify.POid);
            string da = objModel.cc_po(POid);
            return Json(da, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public PartialViewResult po_novl(string PONo = "", string pototal = "")
        {

            List<EOW_ArfInsuranceraising> objowner = new List<EOW_ArfInsuranceraising>();
            objowner = objModel.pono(Session["invoiceGid"].ToString()).ToList();
            if ((string.IsNullOrEmpty(PONo)) == false)
            {
                ViewBag.ecfarf_po_no = PONo;
                objowner = objowner.Where(x => PONo == null ||
                    (x.PONo.ToUpper().Contains(PONo.ToUpper()))).ToList();
            }
            if ((string.IsNullOrEmpty(pototal)) == false)
            {
                ViewBag.ecfarf_po_no = pototal;
                objowner = objowner.Where(x => pototal == null ||
                    (x.pototal.ToUpper().Contains(pototal.ToUpper()))).ToList();
            }
            if (objowner.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            return PartialView("pohead", objowner);

        }
        [HttpPost]
        public PartialViewResult cbf_novl(EOW_ArfInsuranceraising pouchmdl)
        {

            List<EOW_ArfInsuranceraising> objowner = new List<EOW_ArfInsuranceraising>();
            objowner = objModel.cbfno(Session["invoiceGid"].ToString()).ToList();
            if (pouchmdl != null)
            {
                if ((string.IsNullOrEmpty(pouchmdl.ecfarf_cbf_no)) == false)
                {
                    ViewBag.cbf_no = pouchmdl.ecfarf_cbf_no;
                    objowner = objowner.Where(x => pouchmdl.ecfarf_cbf_no == null ||
                        (x.ecfarf_cbf_no.ToUpper().Contains(pouchmdl.ecfarf_cbf_no.ToUpper()))).ToList();
                }
            }
            if (objowner.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            return PartialView("CBF", objowner);

        }
        [HttpGet]
        public PartialViewResult GetSupplierDetail(string vartextcode, string viewfor)
        {
            EOW_ArfInsuranceraising TypeModel = new EOW_ArfInsuranceraising();
            Session["SupplierCode"] = vartextcode.ToString();
            return PartialView(TypeModel);
        }

        [HttpGet]
        public PartialViewResult GetEcfEmployeeDetail(int vartextcode)
        {
            EOW_ArfInsuranceraising TypeModel = new EOW_ArfInsuranceraising();
            Session["ecf_employee_gid"] = vartextcode.ToString();
            return PartialView(TypeModel);
        }
        [HttpGet]
        public PartialViewResult GetEcfSupplierDetail(int vartextcode)
        {

            EOW_ArfInsuranceraising TypeModel = new EOW_ArfInsuranceraising();
            Session["ecf_employee_gid"] = vartextcode.ToString();
            return PartialView(TypeModel);
        }

       
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Edit(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Delete(int id)
        {
            return View();
        }
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
        public PartialViewResult ArfsupplierPaymentEdit(string id, string viewfor)
        {
            if (viewfor == "edit")
            {
                ViewBag.viewfor = "edit";
            }
            else if (viewfor == "view")
            {
                ViewBag.viewfor = "view";
            }

            EOW_ArfInsuranceraising TypeModel = new EOW_ArfInsuranceraising();
            if (id != null)
            {
                TypeModel = objModel.empdetailspay_id(id);
                TypeModel.GetPaymode = new SelectList(objModel.GetPaymode(Session["employeesupplier"].ToString(), Session["Supplierecfid"].ToString()), "paymode_Code", "paymode_name", TypeModel.creditline_pay_mode);
                TypeModel.GetRef = new SelectList(objModel.GetRefant(Session["employeesupplier"].ToString()), "payment_accountno", "payment_accountno", TypeModel.creditline_ref_no);

            }
            else
            {
                TypeModel.GetPaymode = new SelectList(objModel.GetPaymode(Session["employeesupplier"].ToString(), Session["Supplierecfid"].ToString()), "paymode_Code", "paymode_name");
                TypeModel.GetRef = new SelectList(objModel.GetRefant(Session["employeesupplier"].ToString()), "payment_accountno", "payment_accountno");
                TypeModel.RefNoId = new SelectList(objModel.GetRefant(Session["employeesupplier"].ToString()), "payment_accountno", "payment_accountno");

            }
            return PartialView(TypeModel);
        }
        [HttpGet]
        public PartialViewResult _supplierPaymentmodepopup()
        {
            List<EOW_RefNo> lst = new List<EOW_RefNo>();
            return PartialView(lst);
        }
        [HttpGet]
        public PartialViewResult _supplierPaymentmodeeft()
        {
            List<EOW_RefNo> lst = new List<EOW_RefNo>();
            return PartialView(lst);
        }
        [HttpGet]
        public PartialViewResult _supplierPaymentmodeChq()
        {
            List<EOW_RefNo> lst = new List<EOW_RefNo>();
            return PartialView(lst);
        }
    }
}
