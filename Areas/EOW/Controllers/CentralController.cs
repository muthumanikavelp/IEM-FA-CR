using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using IEM.Areas.EOW.Models;
using System.Reflection;
using System.Web.Helpers;
using System.IO;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using System.Net;
//using Excel = Microsoft.Office.Interop.Excel;
using System.Data.OleDb;
using System.Collections;
using System.Configuration;
using IEM.Common;


using Excel;
using ClosedXML.Excel;
using IEM.Areas.MASTERS.Models;
using IEM.Helper;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using IEM.App_Start;

namespace IEM.Areas.EOW.Controllers
{
    [NoDirectAccess]
    public class CentralController : Controller
    {
        ErrorLog objErrorLog = new ErrorLog();
        DataTable dt = new DataTable();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        CommonIUD objCommonIUD = new CommonIUD();
        EOW_DataModel EOWDataModel = new EOW_DataModel();
        CentraldataModel sub = new CentraldataModel();
        EOW_Currency data = new EOW_Currency();
        private EOW_IRepository eowrep;
        private CentralRepository objModel;
        int id;
        string cmbid;
        string sup;
        String result = String.Empty;
        List<string> list = new List<string>();
        public CentralController()
            : this(new CentralModel())
        {

        }
        public CentralController(CentralRepository objM)
        {
            objModel = objM;
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetValue(CentraldataModel delnote)
        {
            return Json(objModel.SelectPONo(delnote), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Centralteaminward()
        {
            try
            {
                list.Add("Approved");
                list.Add("UnApproved");
                List<string> Rolelist = new List<string>();
                {
                    Rolelist = list;
                };
                List<SelectListItem> role = new List<SelectListItem>();
                foreach (var item in Rolelist)
                {
                    role.Add(new SelectListItem
                    {
                        Text = item.ToString(),
                        //Value = item.ToString()
                    });
                }
                ViewBag.SupplierType = role;
                List<CentraldataModel> obj = new List<CentraldataModel>();
                if (TempData["Check"] != "Search")
                {
                    Session["Viewbagdata"] = null;
                    Session["Report"] = null;
                    Session["recdateto"] = null;
                }
                if (Session["Report"] != null)
                {
                    obj = (List<CentraldataModel>)Session["Report"];

                    CentraldataModel Viewbags = new CentraldataModel();
                    Viewbags = (CentraldataModel)Session["Viewbagdata"];
                    if (Session["recdateto"] != null)
                    {
                        ViewBag.RecivedDateTo = Session["recdateto"].ToString();
                    }
                    ViewBag.RecivedDateFrom = Viewbags.ReceivedDate;
                    ViewBag.invoiceAmount = Viewbags.InvoiceAmount;
                    ViewBag.InvoiceDate = Viewbags.InvoiceDate;
                    ViewBag.raisercode = Viewbags.RaiserCode;
                    ViewBag.raisername = Viewbags.RaiserName;
                    ViewBag.suppliercode = Viewbags.SupplierCode;
                    ViewBag.Suppliername = Viewbags.SupplierName;
                    ViewBag.Department = Viewbags.EmployeeDepartment;
                    ViewBag.InvoiceNo = Viewbags.InvoiceNo;
                    ViewBag.PONO = Viewbags.PONo;
                    list.Add("Approved");
                    list.Add("UnApproved");
                    List<string> Rolelist1 = new List<string>();
                    {
                        Rolelist1 = list;
                    };
                    List<SelectListItem> role1 = new List<SelectListItem>();
                    foreach (var item in Rolelist1)
                    {
                        bool selected = false;
                        if (item == Viewbags.StringSupplierType.ToString())
                        {
                            selected = true;
                        }
                        role1.Add(new SelectListItem
                        {
                            Text = item.ToString(),
                            Selected = selected
                        });
                    }
                    ViewBag.SupplierType = role1;
                }
                else
                {
                    obj = objModel.SelectCentralInward().ToList();
                }
                if (obj.Count == 0)
                {
                    ViewBag.Message = "No Records";
                }
                return View(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult Centralteaminward(string modeNf = null, string RecivedDateFrom = null, string RecivedDateTo = null, string invoiceAmount = null, string InvoiceDate = null, string raisercode = null, string raisername = null, string suppliercode = null, string Suppliername = null, string Department = null, string InvoiceNo = null, string PONO = null, string command = null, string SupplierType = "")
        {
            try
            {
                TempData["Check"] = "Search";
                Session["recdateto"] = RecivedDateTo;
                CentraldataModel Viewbags = new CentraldataModel();

                List<CentraldataModel> Rolelist1 = new List<CentraldataModel>();
                {
                    Rolelist1 = objModel.SelectPONo().ToList();
                };
                List<SelectListItem> role1 = new List<SelectListItem>();
                foreach (var item in Rolelist1)
                {
                    role1.Add(new SelectListItem
                    {
                        Text = item.PONo.ToString(),
                        Value = item.POid.ToString()
                    });
                }
                ViewBag.PONo = role1;

                list.Add("Approved");
                list.Add("UnApproved");
                List<string> Rolelist = new List<string>();
                {
                    Rolelist = list;
                };
                List<SelectListItem> role = new List<SelectListItem>();
                foreach (var item in Rolelist)
                {
                    bool selected = false;
                    if (item == SupplierType)
                    {
                        selected = true;
                    }
                    role.Add(new SelectListItem
                    {
                        Text = item.ToString(),
                        Selected = selected
                        //Value = item.ToString()
                    });
                }
                ViewBag.SupplierType = role;

                List<CentraldataModel> obj = new List<CentraldataModel>();
                if (command == "Clear")
                {
                    ViewBag.RecivedDateFrom = "";
                    ViewBag.RecivedDateTo = "";
                    ViewBag.invoiceAmount = "";
                    ViewBag.InvoiceDate = "";
                    ViewBag.raisercode = "";
                    ViewBag.raisername = "";
                    ViewBag.suppliercode = "";
                    ViewBag.Suppliername = "";
                    ViewBag.Department = "";
                    ViewBag.InvoiceNo = "";
                    ViewBag.PONO = "";
                }
                else
                {
                    if (modeNf == "U")
                    {
                        ViewBag.U = "checked";
                    }
                    if (modeNf == "A")
                    {
                        ViewBag.A = "checked";
                    }

                    Viewbags.ReceivedDate = RecivedDateFrom;
                    Viewbags.InvoiceAmount = invoiceAmount;
                    Viewbags.InvoiceDate = InvoiceDate;
                    Viewbags.RaiserCode = raisercode;
                    Viewbags.RaiserName = raisername;
                    Viewbags.SupplierCode = suppliercode;
                    Viewbags.SupplierName = Suppliername;
                    Viewbags.EmployeeDepartment = Department;
                    Viewbags.InvoiceNo = InvoiceNo;
                    Viewbags.PONo = PONO;
                    Viewbags.StringSupplierType = SupplierType;
                    Session["Viewbagdata"] = Viewbags;

                    ViewBag.RecivedDateFrom = RecivedDateFrom;
                    ViewBag.RecivedDateTo = RecivedDateTo;
                    ViewBag.invoiceAmount = invoiceAmount;
                    ViewBag.InvoiceDate = InvoiceDate;
                    ViewBag.raisercode = raisercode;
                    ViewBag.raisername = raisername;
                    ViewBag.suppliercode = suppliercode;
                    ViewBag.Suppliername = Suppliername;
                    ViewBag.Department = Department;
                    ViewBag.InvoiceNo = InvoiceNo;
                    ViewBag.PONO = PONO;
                    obj = objModel.Search(SupplierType, RecivedDateFrom, RecivedDateTo, invoiceAmount, InvoiceDate, raisercode, raisername, suppliercode, Suppliername, Department, InvoiceNo, PONO).ToList();
                    Session["Report"] = obj;
                    if (obj.Count == 0)
                    {
                        ViewBag.Message = "No Records Found";
                    }
                }
                return View(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public ActionResult ClearInward()
        {
            try
            {
                Session["Viewbagdata"] = null;
                Session["Report"] = null;
                Session["recdateto"] = null;
                return RedirectToAction("Centralteaminward", "Central");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult CentralteaminwardAddNew()
        {
            try
            {

                CentraldataModel central = new CentraldataModel();

                List<CentraldataModel> Rolelist1 = new List<CentraldataModel>();
                {
                    Rolelist1 = objModel.GetCurrency().ToList();
                };
                List<SelectListItem> role1 = new List<SelectListItem>();
                foreach (var item in Rolelist1)
                {
                    bool selected = false;
                    if (item.CurrencyName.Contains("INR"))
                    {
                        selected = true;
                    }
                    role1.Add(new SelectListItem
                    {
                        Text = item.CurrencyName.ToString(),
                        Value = item.CurrencyId.ToString(),
                        Selected = selected
                    });
                }
                ViewBag.CentralCurrency = role1;
                ViewBag.exrate = "1.00";

                list.Add("Approved");
                list.Add("UnApproved");
                List<string> Rolelist = new List<string>();
                {
                    Rolelist = list;
                };
                List<SelectListItem> role = new List<SelectListItem>();
                foreach (var item in Rolelist)
                {
                    role.Add(new SelectListItem
                    {
                        Text = item.ToString(),
                        //Value = item.ToString()
                    });
                }
                ViewBag.SupplierType = role;
                //central.Currencydata = new SelectList(objModel.GetCurrency().ToList(), "CurrencyId", "CurrencyName", data.CurrencyId);
                central.CurrencyName = "INR";

                return PartialView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult CheckValidInwardNo(CentraldataModel Typemodel)
        {
            try
            {
                if (Typemodel.PONo != null)
                {
                    result = objModel.CheckPono(Typemodel.PONo);
                }
                else
                {
                    result = "";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult SupplierSearch(string listfor)
        {
            try
            {
                List<CentraldataModel> obj = new List<CentraldataModel>();
                CentraldataModel Getsuppsearchval = new CentraldataModel();
                if (listfor == "search")
                {
                    if (Session["SearchSupp"] != null)
                    {
                        Getsuppsearchval = (CentraldataModel)Session["SerchSupplierData"];
                        @ViewBag.SupplierName = Getsuppsearchval.SupplierName;
                        @ViewBag.SupplierCode = Getsuppsearchval.SupplierCode;
                        @ViewBag.PanNo = Getsuppsearchval.PanNo;
                        obj = (List<CentraldataModel>)Session["SearchSupp"];
                    }
                }
                else
                {
                    Session["SearchSupp"] = null;
                    obj = objModel.SelectSupplier().ToList();
                }
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult EditSupplierSearch(string listfor)
        {
            try
            {
                List<CentraldataModel> obj = new List<CentraldataModel>();
                CentraldataModel Getsuppsearchval = new CentraldataModel();
                if (listfor == "search")
                {
                    if (Session["SearchSupp"] != null)
                    {
                        Getsuppsearchval = (CentraldataModel)Session["SerchSupplierData"];
                        @ViewBag.SupplierName = Getsuppsearchval.SupplierName;
                        @ViewBag.SupplierCode = Getsuppsearchval.SupplierCode;
                        obj = (List<CentraldataModel>)Session["SearchSupp"];
                    }
                }
                else
                {
                    Session["SearchSupp"] = null;
                    obj = objModel.SelectSupplier().ToList();
                }
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult SupplierSearchECF11(string SupplierName = "", string SupplierCode = "", string PanNo = "")
        {
            try
            {
                List<CentraldataModel> obj = new List<CentraldataModel>();
                CentraldataModel supplier = new CentraldataModel();
                if (SupplierCode != "" || SupplierName != "")
                {
                    obj = objModel.SelectSupplierSearch(SupplierName, SupplierCode, PanNo).ToList();
                    @ViewBag.SupplierName = SupplierName;
                    @ViewBag.SupplierCode = SupplierCode;
                    @ViewBag.PanNo = PanNo;

                    supplier.SupplierCode = SupplierCode;
                    supplier.SupplierName = SupplierName;
                    supplier.PanNo = PanNo;
                    Session["SerchSupplierData"] = supplier;
                    Session["SearchSupp"] = obj;
                    if (obj.Count == 0)
                    {
                        ViewBag.Message = "No Record Found !";
                    }
                }
                else
                {
                    Session["SerchSupplierData"] = null;
                    Session["SearchSupp"] = null;
                    obj = objModel.SelectSupplier().ToList();

                }
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult POSearch(string listfor, string claimType)
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
                    obj = objModel.SelectPodetailswithoutarg(int.Parse(Session["posearchid"].ToString()), claimType).ToList();
                }
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                throw ex;
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
                    recordsearch = objModel.SelectPodetails(pototal, PONo, int.Parse(Session["posearchid"].ToString()), Convert.ToString(Session["claimType"])).ToList();
                    Session["SearchEmployeedata"] = recordsearch;
                }
                else
                {
                    emp.RaiserCode = PONo;
                    emp.RaiserName = pototal;
                    Session["SerchViewbag"] = emp;
                    recordsearch = objModel.SelectPodetails(pototal, PONo, int.Parse(Session["posearchid"].ToString()), Convert.ToString(Session["claimType"])).ToList();
                    Session["SearchEmployeedata"] = recordsearch;
                }

                return Json(recordsearch, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult posearchdetails(int id)
        {
            try
            {
                List<CentraldataModel> obj = new List<CentraldataModel>();
                CentraldataModel posearch = new CentraldataModel();
                obj = objModel.SelectPONoByid(id).ToList();
                if (obj.Count > 0)
                {
                    Session["posearchid"] = Convert.ToString(id);
                }
                else
                {
                    ViewBag.Message = "No Record's Found !";
                    result = "Empty";
                }
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult Editposearchdetails(int id)
        {
            try
            {
                List<CentraldataModel> obj = new List<CentraldataModel>();
                CentraldataModel posearch = new CentraldataModel();
                obj = objModel.SelectPONoByid(id).ToList();
                if (obj.Count > 0)
                {
                    Session["posearchid"] = Convert.ToString(id);
                }
                else
                {
                    ViewBag.Message = "No Record's Found !";
                }
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public PartialViewResult SupplierSearchEdit(string PONo = "", string pototal = "")
        {
            try
            {
                List<CentraldataModel> obj = new List<CentraldataModel>();
                CentraldataModel emp = new CentraldataModel();
                if (pototal == "" && PONo == "")
                {
                    obj = objModel.SelectPONoByid(int.Parse(Session["posearchid"].ToString())).ToList();
                }
                else
                {
                    obj = objModel.SelectPodetails(pototal, PONo, int.Parse(Session["posearchid"].ToString()), null).ToList();
                    if (obj != null)
                    {
                        ViewBag.EmployeeCode = PONo;
                        ViewBag.SupplierName = pototal;
                    }
                    if (obj.Count == 0)
                    {
                        ViewBag.Message = "No Record Found !";
                    }
                }
                return PartialView("posearchdetails", obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PartialViewResult EmployeeSearch(string listfor)
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
                    obj = objModel.SelectEmployee().ToList();
                }
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult EmployeeSearchforinward(string RaiserName = "", string RaiserCode = "")
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
                recordsearch = objModel.SelectEmployeeSearch(RaiserName, RaiserCode).ToList();
                Session["SearchEmployeedata"] = recordsearch;
            }
            else
            {
                Session["SerchViewbag"] = null;
                Session["SerchViewbag"] = null;
                recordsearch = objModel.SelectEmployeeSearch(RaiserName, RaiserCode).ToList();
            }

            return Json(recordsearch, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AddSupplierDuplicateCheck(CentraldataModel Student)
        {
            try
            {
                Session["EmployeeId"] = Student.EmployeeId;
                result = objModel.AddSupplierDuplicateCheck(Student);
                if (result == "DuplicateInvoice")
                {
                    result = "Duplicate Invoice Number : Invoice Number Already Exists";
                }
                else
                {
                    int nodays = 0;
                    string resultnew = EOWDataModel.GetStatusexcel("", Student.EmployeeId.ToString(), "", "Ecfraiser");
                    nodays = Convert.ToInt32(resultnew);
                    if (nodays < 15)
                    {
                        result = "Employee Can't Raise ECF";
                    }
                    else
                    {
                        result = "NotExists";
                    }
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult AddSupplier(CentraldataModel Student)
        {
            try
            {
                if (Session["EmployeeId"] != null)
                {
                    Student.EmployeeId = Convert.ToInt32(Session["EmployeeId"]);
                }
                result = objModel.AddSupplier(Student);
                if (result == "sub")
                {
                    result = "Inwarded Successfully, Do you want to add Similar invoice?";
                }
                if (result == "DuplicateInvoice")
                {
                    result = "Duplicate Invoice Number : Invoice Number Already Exists";
                }
                if (result == "DuplicateInvoiceinward")
                {
                    result = "Duplicate Invoice Number : Invoice Number Already Exists in Inward Table";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult EditSupplierView(int id)
        {
            try
            {
                CentraldataModel central = new CentraldataModel();

                Session["CentralId"] = id;
                CentraldataModel cen = objModel.SelectCentralInwardEditbyid(id);
                if (cen.SupplierType.ToString() == "A")
                {
                    sup = "Approved";
                }
                if (cen.SupplierType.ToString() == "U")
                {
                    sup = "UnApproved";
                }

                list.Add("Approved");
                list.Add("UnApproved");


                List<string> Rolelist = new List<string>();
                {
                    Rolelist = list;
                };
                List<SelectListItem> role = new List<SelectListItem>();
                foreach (var item in Rolelist)
                {
                    bool selected = false;
                    if (item == sup)
                    {
                        selected = true;
                    }
                    role.Add(new SelectListItem
                    {
                        Text = item.ToString(),
                        Selected = selected
                    });
                }
                ViewBag.SupplierTypeEdit = role;

                List<CentraldataModel> Rolelist1 = new List<CentraldataModel>();
                {
                    Rolelist1 = objModel.GetCurrency().ToList();
                };
                List<SelectListItem> role1 = new List<SelectListItem>();
                foreach (var item in Rolelist1)
                {
                    bool selected = false;
                    if (item.CurrencyName == cen.CurrencyId)
                    {
                        selected = true;
                    }
                    role1.Add(new SelectListItem
                    {
                        Text = item.CurrencyName.ToString(),
                        Value = item.CurrencyId.ToString(),
                        Selected = selected
                    });
                }
                ViewBag.CentralCurrency = role1;
                ViewBag.exrate = cen.Exrate;

                return View(cen);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult Getcurrencyrate(EOW_Currency brandID)
        {
            string rate = EOWDataModel.Getcurrencydata(brandID);
            Session["currentrate"] = rate.ToString();
            return Json(rate, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EditSupplier(CentraldataModel Student)
        {
            try
            {
                List<CentraldataModel> Rolelist1 = new List<CentraldataModel>();
                {
                    Rolelist1 = objModel.SelectPONo().ToList();
                };
                List<SelectListItem> role1 = new List<SelectListItem>();
                foreach (var item in Rolelist1)
                {
                    role1.Add(new SelectListItem
                    {
                        Text = item.PONo.ToString(),
                        Value = item.POid.ToString()
                    });
                }
                ViewBag.PONo = role1;
                int nodays = 0;
                string resultnew = EOWDataModel.GetStatusexcel("", Student.EmployeeId.ToString(), "", "Ecfraiser");
                nodays = Convert.ToInt32(resultnew);
                if (nodays < 15)
                {
                    result = "Employee Can't Raise ECF";
                }
                else
                {
                    Student.InwardId = Convert.ToInt32(Session["CentralId"]);
                    result = objModel.EditSupplier(Student);
                    if (result == "sub")
                    {
                        result = "Successfully Edited";
                    }
                    if (result == "DuplicateInvoice")
                    {
                        result = "Duplicate Invoice Number : Invoice Number Already Exists in Invoice Table";
                    }
                    if (result == "DuplicateInvoiceinward")
                    {
                        result = "Duplicate Invoice Number : Invoice Number Already Exists in Inward Table";
                    }
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult View(int id)
        {
            try
            {
                Session["CentralId"] = id;
                CentraldataModel cen = objModel.SelectCentralInwardEditbyid(id);

                if (cen.SupplierType.ToString() == "A")
                {
                    sup = "Approved";
                }
                if (cen.SupplierType.ToString() == "U")
                {
                    sup = "UnApproved";
                }

                list.Add("Approved");
                list.Add("UnApproved");
                List<string> Rolelist = new List<string>();
                {
                    Rolelist = list;
                };
                List<SelectListItem> role = new List<SelectListItem>();
                foreach (var item in Rolelist)
                {
                    bool selected = false;
                    if (item == sup)
                    {
                        selected = true;
                    }
                    role.Add(new SelectListItem
                    {
                        Text = item.ToString(),
                        Selected = selected

                    });
                }
                ViewBag.SupplierTypeView = role;

                List<CentraldataModel> Rolelist1 = new List<CentraldataModel>();
                {
                    Rolelist1 = objModel.GetCurrency().ToList();
                };
                List<SelectListItem> role1 = new List<SelectListItem>();
                foreach (var item in Rolelist1)
                {
                    bool selected = false;
                    if (item.CurrencyName == cen.CurrencyId)
                    {
                        selected = true;
                    }
                    role1.Add(new SelectListItem
                    {
                        Text = item.CurrencyName.ToString(),
                        Value = item.CurrencyId.ToString(),
                        Selected = selected
                    });
                }
                ViewBag.CentralCurrency = role1;
                ViewBag.exrate = cen.Exrate;
                return PartialView(cen);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult Delete(int id)
        {
            try
            {
                Session["CentralId"] = id;
                CentraldataModel cen = objModel.SelectCentralInwardEditbyid(id);
                if (cen.SupplierType.ToString() == "A")
                {
                    sup = "Approved";
                }
                if (cen.SupplierType.ToString() == "U")
                {
                    sup = "UnApproved";
                }

                list.Add("Approved");
                list.Add("UnApproved");
                List<string> Rolelist = new List<string>();
                {
                    Rolelist = list;
                };
                List<SelectListItem> role = new List<SelectListItem>();
                foreach (var item in Rolelist)
                {
                    bool selected = false;
                    if (item == sup)
                    {
                        selected = true;
                    }
                    role.Add(new SelectListItem
                    {
                        Text = item.ToString(),
                        Selected = selected

                    });
                }
                ViewBag.SupplierTypeDelete = role;

                List<CentraldataModel> Rolelist1 = new List<CentraldataModel>();
                {
                    Rolelist1 = objModel.GetCurrency().ToList();
                };
                List<SelectListItem> role1 = new List<SelectListItem>();
                foreach (var item in Rolelist1)
                {
                    bool selected = false;
                    if (item.CurrencyName == cen.CurrencyId)
                    {
                        selected = true;
                    }
                    role1.Add(new SelectListItem
                    {
                        Text = item.CurrencyName.ToString(),
                        Value = item.CurrencyId.ToString(),
                        Selected = selected
                    });
                }
                ViewBag.CentralCurrency = role1;
                ViewBag.exrate = cen.Exrate;

                return PartialView(cen);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult DeleteSupplier()
        {
            try
            {
                result = objModel.DeleteSupplier(Convert.ToInt32(Session["CentralId"]));
                result = "del";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public ActionResult ClearReport()
        {
            try
            {
                Session["ReportViewbagdata"] = null;
                Session["reportrecdateto"] = null;
                Session["Report"] = null;
                Session["CentralReport"] = null;
                Session["id"] = null;
                return RedirectToAction("CentralReport", "Central");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public ActionResult CentralReport(string id)
        {
            try
            {
                //Session["id"] = null;
                //list.Add("Approved");
                //list.Add("UnApproved");
                //List<string> Rolelist1 = new List<string>();
                //{
                //    Rolelist1 = list;
                //};
                //List<SelectListItem> role1 = new List<SelectListItem>();
                //foreach (var item in Rolelist1)
                //{
                //    role1.Add(new SelectListItem
                //    {
                //        Text = item.ToString(),
                //        //Value = item.ToString()
                //    });
                //}
                //ViewBag.SupplierType = role1;

                //List<CentraldataModel> Rolelist = new List<CentraldataModel>();
                //{
                //    Rolelist = objModel.SelectStatus().ToList();
                //};
                //List<SelectListItem> role = new List<SelectListItem>();
                //foreach (var item in Rolelist)
                //{
                //    role.Add(new SelectListItem
                //    {
                //        Text = item.statusname.ToString(),
                //        Value = item.StatusGid.ToString()
                //    });
                //}
                //ViewBag.Rolelist = role;
                //if (id == "new")
                //{
                //    Session["ReportViewbagdata"] = null;
                //    Session["reportrecdateto"] = null;
                //    Session["Report"] = null;
                //    Session["CentralReport"] = null;
                //    Session["id"] = null;
                //}
                //List<CentraldataModel> cen = new List<CentraldataModel>();
                //if (Session["CentralReport"] != null)
                //{
                //    cen = (List<CentraldataModel>)Session["CentralReport"];

                //    CentraldataModel Viewbags = new CentraldataModel();
                //    Viewbags = (CentraldataModel)Session["ReportViewbagdata"];
                //    if (Session["reportrecdateto"] != null)
                //    {
                //        ViewBag.RecivedDateTo = Session["reportrecdateto"].ToString();
                //    }
                //    ViewBag.RecivedDateFrom = Viewbags.ReceivedDate;
                //    ViewBag.invoiceAmount = Viewbags.InvoiceAmount;
                //    ViewBag.InvoiceDate = Viewbags.InvoiceDate;
                //    ViewBag.raisercode = Viewbags.RaiserCode;
                //    ViewBag.raisername = Viewbags.RaiserName;
                //    ViewBag.suppliercode = Viewbags.SupplierCode;
                //    ViewBag.Suppliername = Viewbags.SupplierName;
                //    ViewBag.Department = Viewbags.EmployeeDepartment;
                //    ViewBag.InvoiceNo = Viewbags.InvoiceNo;
                //    ViewBag.PONO = Viewbags.PONo;

                //    list.Add("Approved");
                //    list.Add("UnApproved");
                //    List<string> Rolelist2 = new List<string>();
                //    {
                //        Rolelist2 = list;
                //    };
                //    List<SelectListItem> role2 = new List<SelectListItem>();
                //    foreach (var item in Rolelist2)
                //    {
                //        bool selected = false;
                //        if (item == Viewbags.StringSupplierType.ToString())
                //        {
                //            selected = true;
                //        }
                //        role2.Add(new SelectListItem
                //        {
                //            Text = item.ToString(),
                //            Selected = selected
                //        });
                //    }
                //    ViewBag.SupplierType = role2;

                //}
                //else
                //{
                //    cen = objModel.SearchReport().ToList();
                //}
                //if (cen.Count == 0)
                //{
                //    ViewBag.Message = "No Records";
                //}
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult CentralReport(string modeNf = null, string RecivedDateFrom = null, string RecivedDateTo = null, string invoiceAmount = null, string InvoiceDate = null, string raisercode = null, string raisername = null, string suppliercode = null, string Suppliername = null, string Department = null, string InvoiceNo = null, string PONO = null, string Rolelist = null, string SupplierType = "")
        {
            try
            {
                TempData["CheckReport"] = "CheckSearch";
                Session["reportrecdateto"] = RecivedDateTo;
                CentraldataModel Viewbags = new CentraldataModel();

                list.Add("Approved");
                list.Add("UnApproved");
                List<string> Rolelist1 = new List<string>();
                {
                    Rolelist1 = list;
                };
                List<SelectListItem> role1 = new List<SelectListItem>();
                foreach (var item in Rolelist1)
                {
                    bool selected = false;
                    if (item == SupplierType)
                    {
                        selected = true;
                    }
                    role1.Add(new SelectListItem
                    {
                        Text = item.ToString(),
                        Selected = selected
                        //Value = item.ToString()
                    });
                }
                ViewBag.SupplierType = role1;


                List<CentraldataModel> Statuslist = new List<CentraldataModel>();
                {
                    Statuslist = objModel.SelectStatus().ToList();
                };
                List<SelectListItem> role = new List<SelectListItem>();
                foreach (var item in Statuslist)
                {
                    Boolean selectedclas = false;
                    if (item.StatusGid.ToString() == Rolelist)
                    {
                        selectedclas = true;
                    }

                    role.Add(new SelectListItem
                    {
                        Text = item.statusname.ToString(),
                        Value = item.StatusGid.ToString(),
                        Selected = selectedclas
                    });
                }
                if (modeNf == "U")
                {
                    ViewBag.U = "checked";
                }
                if (modeNf == "A")
                {
                    ViewBag.A = "checked";
                }
                Viewbags.ReceivedDate = RecivedDateFrom;
                Viewbags.InvoiceAmount = invoiceAmount;
                Viewbags.InvoiceDate = InvoiceDate;
                Viewbags.RaiserCode = raisercode;
                Viewbags.RaiserName = raisername;
                Viewbags.SupplierCode = suppliercode;
                Viewbags.SupplierName = Suppliername;
                Viewbags.EmployeeDepartment = Department;
                Viewbags.InvoiceNo = InvoiceNo;
                Viewbags.PONo = PONO;
                Viewbags.StringSupplierType = SupplierType;
                Session["ReportViewbagdata"] = Viewbags;

                ViewBag.Rolelist = role;
                ViewBag.RecivedDateFrom = RecivedDateFrom;
                ViewBag.RecivedDateTo = RecivedDateTo;
                ViewBag.invoiceAmount = invoiceAmount;
                ViewBag.InvoiceDate = InvoiceDate;
                ViewBag.raisercode = raisercode;
                ViewBag.raisername = raisername;
                ViewBag.suppliercode = suppliercode;
                ViewBag.Suppliername = Suppliername;
                ViewBag.Department = Department;
                ViewBag.InvoiceNo = InvoiceNo;
                ViewBag.PONO = PONO;
                List<CentraldataModel> obj = new List<CentraldataModel>();
                obj = objModel.SearchReport(SupplierType, RecivedDateFrom, RecivedDateTo, invoiceAmount, InvoiceDate, raisercode, raisername, suppliercode, Suppliername, Department, InvoiceNo, PONO, Rolelist).ToList();
                Session["CentralReport"] = obj;
                if (obj.Count == 0)
                {
                    ViewBag.Message = "No Records Found";
                }
                Session["exceldownload"] = obj;
                return View(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult downloadsexcel() 
        {
            try
            {
                //Session.Clear();
                string mt = null;
                List<CentraldataModel> cList;
                if (Session["exceldownload"] == null)
                {
                    cList = null;

                }
                else
                {
                    cList = (List<CentraldataModel>)Session["exceldownload"];
                }

                DataTable dt = new DataTable();
                dt.Columns.Add("S.No.");
                dt.Columns.Add("Received Date");
                dt.Columns.Add("Supplier Name");
                dt.Columns.Add("Raiser");
                dt.Columns.Add("Ecf No");
                dt.Columns.Add("Invoice No");
                dt.Columns.Add("Invoice Date");
                dt.Columns.Add("PO No");
                dt.Columns.Add("Invoice Amount");
                dt.Columns.Add("Paid Date"); //PaidDATE
                dt.Columns.Add("ECF Status");
               
                for (int i = 0; i < cList.Count; i++)
                {
                    dt.Rows.Add(
                        i + 1,
                        cList[i].ReceivedDate.ToString(),
                        cList[i].SupplierName.ToString(),
                        cList[i].RaiserName.ToString(),
                        cList[i].Docno.ToString(),
                        cList[i].InvoiceNo.ToString(),
                        cList[i].InvoiceDate.ToString(),
                        cList[i].PONo.ToString(),
                        cList[i].InvoiceAmount.ToString(),
                         cList[i].PaidDate.ToString(), //PaidDATE
                        cList[i].ecfstatus.ToString()
                       
                        );
                }
                //export to excel from gridview
                GridView gv = new GridView();
                gv.DataSource = dt;
                gv.DataBind();
                Session["exceldownload"] = gv;
                if (gv.Rows.Count != 0)
                {
                    return new DownloadFileActionResult((GridView)Session["exceldownload"], "DocumentGroup.xls");
                }
                else
                {
                    ViewBag.Message = "No records found";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult EmployeeSearchInward(string listfor)
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
                    obj = objModel.SelectEmployee().ToList();
                }
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult EmployeeSearchInwardWithpar(string RaiserName = "", string RaiserCode = "")
        {
            try
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
                    recordsearch = objModel.SelectEmployeeSearch(RaiserName, RaiserCode).ToList();
                    Session["SearchEmployeedata"] = recordsearch;
                }
                else
                {
                    Session["SerchViewbag"] = null;
                    Session["SerchViewbag"] = null;
                    recordsearch = objModel.SelectEmployeeSearch(RaiserName, RaiserCode).ToList();
                }

                return Json(recordsearch, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult SupplierSearchCentral(string listfor)
        {
            try
            {
                List<CentraldataModel> obj = new List<CentraldataModel>();
                CentraldataModel Getsuppsearchval = new CentraldataModel();
                if (listfor == "search")
                {
                    if (Session["SearchSupp"] != null)
                    {
                        Getsuppsearchval = (CentraldataModel)Session["SerchSupplierData"];
                        @ViewBag.SupplierName = Getsuppsearchval.SupplierName;
                        @ViewBag.SupplierCode = Getsuppsearchval.SupplierCode;
                        @ViewBag.PanNo = Getsuppsearchval.PanNo;
                        obj = (List<CentraldataModel>)Session["SearchSupp"];
                    }
                }
                else
                {
                    Session["SearchSupp"] = null;
                    obj = objModel.SelectSupplier().ToList();
                }
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult SupplierSearchECFCentral(string SupplierName = "", string SupplierCode = "", string PanNo = "")
        {
            try
            {
                List<CentraldataModel> obj = new List<CentraldataModel>();
                CentraldataModel supplier = new CentraldataModel();
                if (SupplierCode != "" || SupplierName != "" || PanNo != "")
                {
                    obj = objModel.SelectSupplierSearch(SupplierName, SupplierCode, PanNo).ToList();
                    @ViewBag.SupplierName = SupplierName;
                    @ViewBag.SupplierCode = SupplierCode;
                    @ViewBag.PanNo = PanNo;

                    supplier.SupplierCode = SupplierCode;
                    supplier.SupplierName = SupplierName;
                    supplier.PanNo = PanNo;
                    Session["SerchSupplierData"] = supplier;
                    Session["SearchSupp"] = obj;
                    if (obj.Count == 0)
                    {
                        ViewBag.Message = "No Record Found !";
                    }
                }
                else
                {
                    Session["SerchSupplierData"] = null;
                    Session["SearchSupp"] = null;
                    obj = objModel.SelectSupplier().ToList();

                }
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult LogViewecf(string id, string types, string ecfstsus)
        {
            try
            {
                string centralteam = "";
                string confirmsupplier = "";
                if (types == "D")
                {
                    centralteam = EOWDataModel.selectsupplierinvoicestatuscen(id.ToString(), "D").ToString();
                    confirmsupplier = EOWDataModel.selectsupplierinvoice(id.ToString(), "D").ToString();
                }
                else
                {
                    centralteam = EOWDataModel.selectsupplierinvoicestatuscen(id.ToString(), "A").ToString();
                    confirmsupplier = EOWDataModel.selectsupplierinvoice(id.ToString(), "A").ToString();
                }

                if (confirmsupplier == "4")
                {
                    EOW_Supplierinvoice seowemp = new EOW_Supplierinvoice();
                    List<EOW_Supplierinvoice> sobjobjMExpense = new List<EOW_Supplierinvoice>();
                    if (types == "D")
                    {
                        sobjobjMExpense = EOWDataModel.SelectViewdatasupplier(id, "ctdata").ToList();
                    }
                    else
                    {

                        if (centralteam == "C" || centralteam == "R")
                        {
                            sobjobjMExpense = EOWDataModel.SelectViewdatasupplier(id, "checkview").ToList();
                        }
                        else
                        {
                            sobjobjMExpense = EOWDataModel.SelectViewdatasupplier(id, "A").ToList();
                        }
                    }

                    if (sobjobjMExpense[0].ecfstatus == "262144" || sobjobjMExpense[0].ecfstatus == "524288")
                    {
                        Session["Centralautit"] = "C";
                    }
                    else
                    {
                        Session["Centralautit"] = "S";
                    }
                    Session["chkraiser"] = sobjobjMExpense[0].chkraiser_gid.ToString();
                    seowemp.ECF_Amount = sobjobjMExpense[0].ECF_Amount.ToString();
                    Session["Ecfqueueamount"] = sobjobjMExpense[0].ECF_Amount.ToString();
                    seowemp.ECF_Date = sobjobjMExpense[0].ECF_Date.ToString();
                    seowemp.raisermodeId = sobjobjMExpense[0].raisermodeId.ToString();
                    seowemp.Grade = sobjobjMExpense[0].Grade.ToString();
                    seowemp.Raiser_Code = sobjobjMExpense[0].Raiser_Code.ToString();
                    seowemp.Raiser_Name = sobjobjMExpense[0].Raiser_Name.ToString();
                    seowemp.Raiser_Modedata = new SelectList(EOWDataModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", sobjobjMExpense[0].raisermodeId.ToString());
                    seowemp.amort = sobjobjMExpense[0].amort.ToString();
                    seowemp.amortdec = sobjobjMExpense[0].amortdec.ToString();
                    seowemp.from = sobjobjMExpense[0].from.ToString();
                    seowemp.to = sobjobjMExpense[0].to.ToString();
                    //string stratus = sobjobjMExpense[0].ecfstatus.ToString();
                    //if (stratus == "1")
                    //{
                    //    seowemp.ecfstatus = "Approved";
                    //}
                    //else if (stratus == "10")
                    //{
                    //    seowemp.ecfstatus = "CT Draft";
                    //}
                    //else
                    //{
                    //    seowemp.ecfstatus = "Inprocess";
                    //}
                    seowemp.ecfstatus = ecfstsus;
                    seowemp.ecfdescription = sobjobjMExpense[0].ecfdescription.ToString();
                    seowemp.Suppliercode = sobjobjMExpense[0].Suppliercode.ToString();
                    seowemp.Suppliername = sobjobjMExpense[0].Suppliername.ToString();
                    seowemp.CurrencyName = sobjobjMExpense[0].CurrencyName.ToString();
                    seowemp.Exrate = sobjobjMExpense[0].Exrate.ToString();
                    seowemp.Currencyamount = sobjobjMExpense[0].Currencyamount.ToString();
                    seowemp.DocName = Convert.ToString(sobjobjMExpense[0].DocName);
                    seowemp.ecfno = sobjobjMExpense[0].ecfno.ToString();
                    ViewBag.EOW_supplierExpenseheader = seowemp;
                    ViewBag.doctype = objCmnFunctions.GetDocType(sobjobjMExpense[0].Doctypeid.ToString());
                    Session["EcfType"] = objCmnFunctions.GetDocType(sobjobjMExpense[0].Doctypeid.ToString());
                    Session["EcfGid"] = sobjobjMExpense[0].ecf_GID.ToString();
                    Session["invoiceGid"] = "";
                    Session["QueueGid"] = sobjobjMExpense[0].Queue_GID.ToString();
                    ViewBag.ApproverAction = sobjobjMExpense[0].queueactionfor.ToString();
                    ViewBag.frst = "Suppliers";
                    string potype = Convert.ToString(sobjobjMExpense[0].DocName);
                    if (potype == "PO" || potype == "WO")
                    {
                        ViewBag.POStatus = "PO";
                    }

                    Session["Ecfdeclaratonnote"] = EOWDataModel.GetDecnote(confirmsupplier.ToString(), "A").ToString();
                }
                return PartialView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult _SupplierPaymentGrid()
        {
            try
            {
                List<EOW_Payment> lstPayment = new List<EOW_Payment>();
                lstPayment = EOWDataModel.GetEmployeeePayment(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString()).ToList();
                return PartialView(lstPayment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult _SupplierTaxDetailsGrid()
        {
            try
            {
                List<sinvotax> lstnew = new List<sinvotax>();
                lstnew = EOWDataModel.GetSupplierTax(Session["invoiceGid"].ToString()).ToList();
                return PartialView(lstnew);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult _TabdebitlineDetails()
        {
            try
            {
                List<EOW_TravelClaim> lstnew = new List<EOW_TravelClaim>();
                lstnew = EOWDataModel.GetSuppliserDedit(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), "S").ToList();
                return PartialView(lstnew);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult ViewSupplierinvoice(EOW_SupplierModelgrid Suppliermodel)
        {
            try
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
                objobjMPayment = EOWDataModel.GetSuppliersingledata(Session["EcfGid"].ToString(), "S", TravelModeGID).ToList();
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult _PODetailsGridV(string values, string valuesid)
        {
            try
            {
                List<EOW_PO> lstnew = new List<EOW_PO>();
                //lstnew = EOWDataModel.Getpodetailsgrid(values, valuesid, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), "S").ToList();
                return PartialView(lstnew);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult _PomappingdetailV(string id)
        {
            try
            {
                EOW_PO Header = new EOW_PO();
                List<EOW_PO> getsingleid = new List<EOW_PO>();
                getsingleid = EOWDataModel.GetPoDetailssingledata(id).ToList();
                Header.POGid = id.ToString();
                Header.PONo = getsingleid[0].PONo.ToString();
                Header.POdate = getsingleid[0].POdate.ToString();
                Header.POStatus = getsingleid[0].POStatus.ToString();
                Header.POBalamount = getsingleid[0].POBalamount.ToString();
                Header.POUtilizedamount = getsingleid[0].POUtilizedamount.ToString();
                Header.POApprovedStatus = getsingleid[0].POApprovedStatus.ToString();
                ViewBag.poheaderheader = Header;

                List<EOW_PO> TypeModel = new List<EOW_PO>();
                TypeModel = EOWDataModel.GetPoDetailsitem(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), id).ToList();
                return PartialView(TypeModel.ToList());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PartialViewResult LogView(int id)
        {
            Session["inwardid"] = id;
            return PartialView();
        }
        public PartialViewResult ReportLogView(int id)
        {
            try
            {
                if (Session["inwardid"] != null)
                {
                    id = Convert.ToInt32(Session["inwardid"]);
                }
                List<CentraldataModel> obj = new List<CentraldataModel>();
                obj = objModel.CentralMakerView(id).ToList();
                ViewBag.InvoiceNo = obj[0].InvoiceNo;
                ViewBag.invoiceAmount = obj[0].InvoiceAmount;
                ViewBag.InvoiceDate = obj[0].InvoiceDate;
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult ReportInwardView(int id)
        {
            try
            {
                CentraldataModel cen = objModel.SelectCentralInwardbyidForMaker(id);

                if (cen.SupplierType.ToString() == "A")
                {
                    sup = "Approved";
                }
                if (cen.SupplierType.ToString() == "U")
                {
                    sup = "UnApproved";
                }
                list.Add("Approved");
                list.Add("UnApproved");
                List<string> Rolelist = new List<string>();
                {
                    Rolelist = list;
                };
                List<SelectListItem> role = new List<SelectListItem>();
                foreach (var item in Rolelist)
                {
                    bool selected = false;
                    if (item == sup)
                    {
                        selected = true;
                    }
                    role.Add(new SelectListItem
                    {
                        Text = item.ToString(),
                        Selected = selected

                    });
                }
                ViewBag.SupplierTypeView = role;

                List<CentraldataModel> Rolelist1 = new List<CentraldataModel>();
                {
                    Rolelist1 = objModel.GetCurrency().ToList();
                };
                List<SelectListItem> role1 = new List<SelectListItem>();
                foreach (var item in Rolelist1)
                {
                    bool selected = false;
                    if (item.CurrencyName == cen.CurrencyId)
                    {
                        selected = true;
                    }
                    role1.Add(new SelectListItem
                    {
                        Text = item.CurrencyName.ToString(),
                        Value = item.CurrencyId.ToString(),
                        Selected = selected
                    });
                }
                ViewBag.CentralCurrency = role1;

                ViewBag.exrate = cen.Exrate;

                return PartialView(cen);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Viewdata(string id)
        {
            try
            {
                string confirmsupplier = EOWDataModel.selectsupplierinvoice(id.ToString(), "A").ToString();
                if (confirmsupplier == "4")
                {
                    EOW_Supplierinvoice seowemp = new EOW_Supplierinvoice();
                    List<EOW_Supplierinvoice> sobjobjMExpense = new List<EOW_Supplierinvoice>();
                    sobjobjMExpense = EOWDataModel.SelectViewdatasupplier(id, "check").ToList();
                    Session["chkraiser"] = sobjobjMExpense[0].chkraiser_gid.ToString();
                    seowemp.ECF_Amount = sobjobjMExpense[0].ECF_Amount.ToString();
                    Session["Ecfqueueamount"] = sobjobjMExpense[0].ECF_Amount.ToString();
                    seowemp.ECF_Date = sobjobjMExpense[0].ECF_Date.ToString();
                    seowemp.raisermodeId = sobjobjMExpense[0].raisermodeId.ToString();
                    seowemp.Grade = sobjobjMExpense[0].Grade.ToString();
                    seowemp.Raiser_Code = sobjobjMExpense[0].Raiser_Code.ToString();
                    seowemp.Raiser_Name = sobjobjMExpense[0].Raiser_Name.ToString();
                    seowemp.Raiser_Modedata = new SelectList(EOWDataModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", sobjobjMExpense[0].raisermodeId.ToString());
                    seowemp.amort = sobjobjMExpense[0].amort.ToString();
                    seowemp.amortdec = sobjobjMExpense[0].amortdec.ToString();
                    seowemp.from = sobjobjMExpense[0].from.ToString();
                    seowemp.to = sobjobjMExpense[0].to.ToString();
                    seowemp.ecfdescription = sobjobjMExpense[0].ecfdescription.ToString();
                    seowemp.Suppliercode = sobjobjMExpense[0].Suppliercode.ToString();
                    seowemp.Suppliername = sobjobjMExpense[0].Suppliername.ToString();
                    seowemp.CurrencyName = sobjobjMExpense[0].CurrencyName.ToString();
                    seowemp.Exrate = sobjobjMExpense[0].Exrate.ToString();
                    seowemp.Currencyamount = sobjobjMExpense[0].Currencyamount.ToString();
                    seowemp.DocName = Convert.ToString(sobjobjMExpense[0].DocName);
                    seowemp.ecfno = sobjobjMExpense[0].ecfno.ToString();
                    ViewBag.EOW_supplierExpenseheader = seowemp;
                    ViewBag.doctype = objCmnFunctions.GetDocType(sobjobjMExpense[0].Doctypeid.ToString());
                    Session["EcfType"] = objCmnFunctions.GetDocType(sobjobjMExpense[0].Doctypeid.ToString());
                    Session["EcfGid"] = sobjobjMExpense[0].ecf_GID.ToString();
                    Session["invoiceGid"] = "";
                    Session["QueueGid"] = sobjobjMExpense[0].Queue_GID.ToString();
                    ViewBag.ApproverAction = sobjobjMExpense[0].queueactionfor.ToString();
                    ViewBag.frst = "Suppliers";
                    string potype = Convert.ToString(sobjobjMExpense[0].DocName);
                    if (potype == "PO" || potype == "WO")
                    {
                        ViewBag.POStatus = "PO";
                    }
                }
                else
                {
                    EOW_EmployeeeExpense eowemp = new EOW_EmployeeeExpense();
                    List<EOW_EmployeeeExpense> objobjMExpense = new List<EOW_EmployeeeExpense>();

                    confirmsupplier = EOWDataModel.selectsupplierinvoice(id.ToString(), "A").ToString();
                    if (confirmsupplier == "6" || confirmsupplier == "7")
                    {
                        objobjMExpense = EOWDataModel.SelectViewdata(id, "AL").ToList();
                    }
                    if (confirmsupplier == "2" || confirmsupplier == "5")
                    {
                        objobjMExpense = EOWDataModel.SelectViewdata(id, "AL").ToList();
                    }
                    else if (confirmsupplier != "2" && confirmsupplier != "5" && confirmsupplier != "6" && confirmsupplier != "7")
                    {
                        objobjMExpense = EOWDataModel.SelectViewdata(id, "A").ToList();
                    }
                    eowemp.ecfno = objobjMExpense[0].ecfno.ToString();
                    eowemp.ECF_Amount = objobjMExpense[0].ECF_Amount.ToString();
                    Session["Ecfqueueamount"] = objobjMExpense[0].ECF_Amount.ToString();
                    eowemp.ClaimMonth = objobjMExpense[0].ECF_Date.ToString();
                    eowemp.ECF_Date = objobjMExpense[0].ECF_Date.ToString();
                    eowemp.Grade = objobjMExpense[0].Grade.ToString();
                    eowemp.Raiser_Code = objobjMExpense[0].Raiser_Code.ToString();
                    eowemp.Raiser_Name = objobjMExpense[0].Raiser_Name.ToString();
                    eowemp.ecfdescription = objobjMExpense[0].ecfdescription.ToString();
                    eowemp.SubCategoryName = objobjMExpense[0].SubCategoryName.ToString();
                    eowemp.Raiser_Modedata = new SelectList(EOWDataModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", objobjMExpense[0].raisermodeId.ToString());
                    ViewBag.EOW_EmployeeeExpenseheader = eowemp;
                    ViewBag.doctype = objCmnFunctions.GetDocType(objobjMExpense[0].Doctypeid.ToString());
                    Session["EcfType"] = objCmnFunctions.GetDocType(objobjMExpense[0].Doctypeid.ToString());
                    Session["EcfGid"] = objobjMExpense[0].ecf_GID.ToString();
                    Session["invoiceGid"] = objobjMExpense[0].invoice_GID.ToString();
                    Session["QueueGid"] = objobjMExpense[0].Queue_GID.ToString();
                    ViewBag.ApproverAction = objobjMExpense[0].queueactionfor.ToString();
                    ViewBag.frst = "Others";
                }
                Session["Ecfdeclaratonnote"] = EOWDataModel.GetDecnote(confirmsupplier.ToString(), "A").ToString();
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult _InvoiceAttachmentDetails()
        {
            List<EOW_File> lstAttachment = new List<EOW_File>();
            return PartialView(lstAttachment);
        }
        [HttpGet]
        public JsonResult ToGetinwardrpt(string ReceivedDateFrom, string ReceivedDateTo, string InvoiceDateFrom, string InvoiceDateTo, string Status, string Ecfno, string InvoiceNo, string searchtype)
        {
            //string data0 = string.Empty;
            //try
            //{
            //    DataTable dt = new DataTable();
            //    dt = objModel.SearchReportNew();
            //    if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
            //    var jsonResult = Json(new { data0 }, JsonRequestBehavior.AllowGet);
            //    jsonResult.MaxJsonLength = 2147483647;
            //    return jsonResult;
            //}
            //catch (Exception ex)
            //{
            //    objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            //    return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            //}

            List<CentraldataModel> lstinward = new List<CentraldataModel>();
            lstinward = objModel.SearchInwardReport(ReceivedDateFrom, ReceivedDateTo, InvoiceDateFrom, InvoiceDateTo, Status, Ecfno, InvoiceNo, searchtype).ToList();
            Session["exceldownload"] = lstinward;
            var jsonResult = Json(new { qrylist = lstinward }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        //----Pandiaraj 31-01-2019 Bulk Upload

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
                        Session["supplierattmtfileexcel"] = hpf;
                        Session["saoriginal"] = hpf.FileName;
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
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
        [HttpPost]
      //  public async Task<JsonResult> bulkUploadFiles()
        public JsonResult UploadFileslocal()
        {

            List<string> objparent = new List<string>();

            string Extensionnew = "";
            string Extension1 = "";
            string error = "0";
            //------------Duplicate Columns Found
            String xmlstr = "";
            DataSet dbcolds = new DataSet();
            ArrayList dbinvoicelist = new ArrayList();
            ArrayList dbdebitList = new ArrayList();
            ArrayList debitinno = new ArrayList();
            ArrayList debitindate = new ArrayList();
            DataTable dtinvoice = new DataTable();
            DataTable dtdebit = new DataTable();
            DataTable distinctinvoice = new DataTable();
            DataTable distinctdebit = new DataTable();
            DataTable result = new DataTable();
            DataTable errortab = new DataTable();
            try
            {
                string path1 = "";
                errortab.Columns.Add("Error S.No.");
                errortab.Columns.Add("Error Line");
                errortab.Columns.Add("Error Description");

                //foreach (string filenew in Request.Files)
                //{
                //    var fileContent = Request.Files[filenew];

                //    if (fileContent != null && fileContent.ContentLength > 0)
                //    {
                //        HttpPostedFileBase hpf = Request.Files[filenew] as HttpPostedFileBase;
                //        TempData["_supplierattmtfileexcel"] = hpf;
                //    }
                //}
                if (Session["supplierattmtfileexcel"] != null)
                {
               // HttpPostedFileBase upload = Session["supplierattmtfileexcel"] as HttpPostedFileBase;
                HttpPostedFileBase savefile = Session["supplierattmtfileexcel"] as HttpPostedFileBase;
                Session["supplierattmtfileexcel"] = "";
                Session["saoriginal"] ="";
                //string Extension = Path.GetFileName(savefile.FileName);
                //string n = string.Format(objCmnFunctions.GetLoginUserGid().ToString() + "Local-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                //Extension1 = System.IO.Path.GetExtension(savefile.FileName);
                //Extensionnew = n + Extension1;
                //path1 = string.Format("{0}{1}", HoldFileUploadUrlDSA(), Extensionnew);
                //if (System.IO.File.Exists(path1))
                //{
                //    System.IO.File.Delete(path1);
                //}
                //savefile.SaveAs(path1);

                //SheetData sobjModel;
                //int count = 0;
                //string sheets = "";
                //FileStream stream = new FileStream(path1, FileMode.Open, FileAccess.Read);

                Stream stream = savefile.InputStream;
                IExcelDataReader reader = null;
                //string GetExten = Path.GetExtension(upload.FileName); Session["saoriginal"]
                Extension1 = Path.GetExtension(savefile.FileName);

                    DataSet Excelds = new DataSet();

                    if (Extension1.ToLower() == ".xlsx")
                    {
                        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        excelReader.IsFirstRowAsColumnNames = true;
                        Excelds = excelReader.AsDataSet();
                        excelReader.Close();
                    }
                    else if (Extension1.ToLower() == ".xls")
                    {
                        IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                        excelReader.IsFirstRowAsColumnNames = true;
                        Excelds = excelReader.AsDataSet();
                        excelReader.Close();
                    }
                    else
                    {
                        error = "1";  //--Invoice Excel file
                        objparent.Add(error);
                        return Json(error, JsonRequestBehavior.AllowGet);
                    }
                    if (Excelds != null && Excelds.Tables.Count > 0)
                    {
                        DataSet downds = new DataSet();
                        dtinvoice = Excelds.Tables[0];
                        dtdebit = Excelds.Tables[1];

                        DataSet ds = new DataSet();
                        int ss = 0;
                        ds = objModel.forexcel(ss, "A");
                        ds.Tables[0].TableName = "Invoice";
                        ds.Tables[1].TableName = "Debitline";
                        ds.Tables[2].TableName = "State Master";
                        ds.Tables[3].TableName = "HSN Master";
                        ds.Tables[4].TableName = "Function Master";
                        ds.Tables[5].TableName = "Cost Master";
                        ds.Tables[6].TableName = "Product Master";
                        ds.Tables[7].TableName = "OU Master";
                        ds.Tables[8].TableName = "Expane Nature Master";
                        ds.Tables[9].TableName = "Category Master";
                        ds.Tables[10].TableName = "SubCategory Master";
                        Session["downloadDS"] = ds;
                        //-------------Database Columns
                        dbcolds = (DataSet)Session["downloadDS"];
                        DataColumn[] dbs0 = new DataColumn[dbcolds.Tables[0].Columns.Count];
                        for (int i = 0; i < dbs0.Length; i++)
                        {
                            // dbs1[i] = dbcolds.Tables[0].Columns[i];
                            dbinvoicelist.Add(dbcolds.Tables[0].Columns[i].ColumnName.Trim());
                        }

                        //-------------- Invoice Excel List

                        DataColumn[] s1col = new DataColumn[dtinvoice.Columns.Count];
                        for (int i = 0; i < s1col.Length; i++)
                        {
                            //s1col[i] = dtinvoice.Columns[i];
                            if (!dbinvoicelist.Contains(dtinvoice.Columns[i].ColumnName.Trim()))
                            {
                                error = "2"; //Dupicate Invoice columns
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                        }


                        //------------Duplicate Rows Found
                        //var UniqueRows = dtinvoice.AsEnumerable().Distinct(DataRowComparer.Default);
                        //DataTable deduptable = UniqueRows.CopyToDataTable();
                        //if (deduptable.Rows.Count != dtinvoice.Rows.Count)
                        //{
                        //    error = "3"; //Dupicate Invoice  Rows
                        //    objparent.Add(error);
                        //    return Json(error, JsonRequestBehavior.AllowGet);
                        //}
                        //----------------------


                        /////-------------------------------Debitline Excel-------------------


                        DataColumn[] dbs1 = new DataColumn[dbcolds.Tables[1].Columns.Count];
                        for (int i = 0; i < dbs1.Length; i++)
                        {
                            dbdebitList.Add(dbcolds.Tables[1].Columns[i].ColumnName.Trim());
                        }
                        DataColumn[] s2col = new DataColumn[dtdebit.Columns.Count];
                        for (int i = 0; i < s2col.Length; i++)
                        {
                            if (!dbdebitList.Contains(dtdebit.Columns[i].ColumnName.Trim()))
                            {
                                error = "4"; //Dupicate Debitline columns
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                        }

                        //deduptable.Rows.Clear();
                        //UniqueRows = dtdebit.AsEnumerable().Distinct(DataRowComparer.Default);
                        //deduptable = UniqueRows.CopyToDataTable();
                        //if (deduptable.Rows.Count != dtdebit.Rows.Count)
                        //{
                        //    error = "5"; //Dupicate Debitline Rows.
                        //    objparent.Add(error);
                        //    return Json(error, JsonRequestBehavior.AllowGet);
                        //}


                        /////------------------------------ 
                        string isamount = "";
                        decimal resulter = 0;
                        string isgstin = "";
                        string isrcm = "";
                        for (int m = 0; m < dtinvoice.Rows.Count; m++)
                        {
                            //for (int i = 0; i < dtinvoice.Columns.Count; i++)
                            //{
                            //    if (dtinvoice.Rows[m][i].ToString().Trim() == "")
                            //    {
                            //        error = "6"; //Empty Rows 
                            //        objparent.Add(error);
                            //        return Json(error, JsonRequestBehavior.AllowGet);
                            //    }

                            //}

                            if (dtinvoice.Rows[m]["ReceivedDate"].ToString().Trim() == "")
                            {
                                error = "6"; //Location should be provide if GST Yes 
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                            if (dtinvoice.Rows[m]["ReceivedDate"].ToString().Trim() != "")
                            {
                                string rsdate = dtinvoice.Rows[m]["ReceivedDate"].ToString().Trim();
                                //DateTime rdate = Convert.ToDateTime(rsdate);
                                //string todaydate = DateTime.Now.ToString("dd/MM/yyyy");
                                DateTime dtSuppliedDate = DateTime.Parse(rsdate);
                                if (dtSuppliedDate.Subtract(DateTime.Today).Days > 0)
                                {
                                    error = "17"; //Location should be provide if GST Yes 
                                    objparent.Add(error);
                                    return Json(error, JsonRequestBehavior.AllowGet);
                                }
                            }
                            if (dtinvoice.Rows[m]["RaiserCode"].ToString().Trim() == "")
                            {
                                error = "6"; //Location should be provide if GST Yes 
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                            if (dtinvoice.Rows[m]["SupplierCode"].ToString().Trim() == "")
                            {
                                error = "6"; //Location should be provide if GST Yes 
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                            if (dtinvoice.Rows[m]["InvoiceDate"].ToString().Trim() == "")
                            {
                                error = "6"; //Location should be provide if GST Yes 
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                            if (dtinvoice.Rows[m]["InvoiceDate"].ToString().Trim() != "")
                            {
                                string rsdate = dtinvoice.Rows[m]["InvoiceDate"].ToString().Trim();
                                DateTime dtSuppliedDate = DateTime.Parse(rsdate);
                                if (dtSuppliedDate.Subtract(DateTime.Today).Days > 0)
                                {
                                    error = "17"; //Location should be provide if GST Yes 
                                    objparent.Add(error);
                                    return Json(error, JsonRequestBehavior.AllowGet);
                                }
                                DateTime vdate = Convert.ToDateTime(dtinvoice.Rows[m]["ReceivedDate"].ToString().Trim());
                                if (dtSuppliedDate.Subtract(vdate).Days > 0)
                                {
                                    error = "18"; //Location should be provide if GST Yes 
                                    objparent.Add(error);
                                    return Json(error, JsonRequestBehavior.AllowGet);
                                }
                            }

                            if (dtinvoice.Rows[m]["Remarks"].ToString().Trim() == "")
                            {
                                error = "6"; //Location should be provide if GST Yes 
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                            isgstin = dtinvoice.Rows[m]["SupplierType"].ToString().Trim().Substring(0, 1).ToUpper();
                            if (!(isgstin.Contains("A") || isgstin.Contains("U")))
                            {
                                error = "8"; //Supplier Type Approved or UnApproved
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                            isamount = dtinvoice.Rows[m]["CurrencyAmount"].ToString().Trim();
                            isgstin = dtinvoice.Rows[m]["GST_YesNo"].ToString().Trim().Substring(0, 1).ToUpper();
                            if (!System.Decimal.TryParse(isamount, out resulter))
                            {
                                error = "7"; //Currency Amount Columns has ivalid Amount format.
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                            if (!(isgstin.Contains("Y") || isgstin.Contains("N")))
                            {
                                error = "9"; //only Applicable GST Yes or No
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                            if (dtinvoice.Rows[m]["ProviderLocation"].ToString().Trim() == "" || dtinvoice.Rows[m]["ReceiverLocation"].ToString().Trim() == "")
                            {
                                error = "10"; //Location should be provide if GST Yes 
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                            isgstin = dtinvoice.Rows[m]["InvoiceType"].ToString().Trim().ToUpper();
                            if (!(isgstin.Contains("PO") || isgstin.Contains("WO") || isgstin.Contains("NON POWO")))
                            {
                                error = "11"; //only Applicable Invoice Type Yes or No
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            } 
                            if (isgstin == "PO" || isgstin =="WO")
                            {
                                isgstin = dtinvoice.Rows[m]["POWO_No"].ToString().Trim().ToUpper();
                                if (isgstin ==""){
                                error = "12"; //POWO No are invalid 
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else if (isgstin != "NON POWO")
                            {
                                error = "11"; //only Applicable Invoice Type Yes or No
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                        }

                        for (int m = 0; m < dtdebit.Rows.Count; m++)
                        {
                            for (int i = 0; i < dtdebit.Columns.Count; i++)
                            {
                                if (dtdebit.Rows[m][i].ToString().Trim() == "")
                                {
                                    error = "6"; //Debitline Empty Rows 
                                    objparent.Add(error);
                                    return Json(error, JsonRequestBehavior.AllowGet);
                                }
                            }

                            isamount = dtdebit.Rows[m]["Amount"].ToString().Trim();
                            if (!System.Decimal.TryParse(isamount, out resulter))
                            {
                                error = "7"; //Debit Amount Columns has ivalid Amount format.
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                            isgstin = dtdebit.Rows[m]["RCM_YesNo"].ToString().Trim().Substring(0, 1).ToUpper();
                            if (!(isgstin.Contains("Y") || isgstin.Contains("N")))
                            {
                                error = "9"; //only RCM GST Yes or No
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                        }
                        //foreach (DataRow rowval in dtinvoice.Rows)
                        //{
                        //    object inrow = rowval["CurrencyAmount"];
                        //    object isgst = rowval["GST_YesNo"];
                        //    object objreceive = rowval["ReceiverLocation"];
                        //    object objprovide = rowval["ProviderLocation"];

                        //    if (inrow == DBNull.Value || inrow == "")
                        //    {
                        //        error = "6"; //Empty Rows 
                        //        objparent.Add(error);
                        //        return Json(error, JsonRequestBehavior.AllowGet);
                        //    }
                        //    else
                        //    {
                        //        decimal resulter;
                        //        if (!System.Decimal.TryParse(inrow.ToString(), out resulter))
                        //        {
                        //            error = "7"; //Currency Amount Columns has ivalid Amount format.
                        //            objparent.Add(error);
                        //            return Json(error, JsonRequestBehavior.AllowGet);
                        //        }
                        //    }
                        //    if (isgst == DBNull.Value || isgst == "")
                        //    {
                        //        error = "8"; //Empty Rows 
                        //        objparent.Add(error);
                        //        return Json(error, JsonRequestBehavior.AllowGet);
                        //    }
                        //    string isgstin = isgst.ToString().Substring(0, 1).ToUpper();
                        //    if (!(isgstin.Contains("Y") || isgstin.Contains("N")))
                        //    {
                        //        error = "9"; //only Applicable GST Yes or No
                        //        objparent.Add(error);
                        //        return Json(error, JsonRequestBehavior.AllowGet);
                        //    }
                        //    else if (isgstin == "Y")
                        //    {
                        //        if (objreceive.ToString().Trim() == "" || objprovide.ToString().Trim() == "")
                        //        {
                        //            error = "10"; //Location should be provide if GST Yes 
                        //            objparent.Add(error);
                        //            return Json(error, JsonRequestBehavior.AllowGet);
                        //        }
                        //    }
                        //}




                        //---------- Debit sheet Invoice no and invoice date match with Invoice sheet                         
                        //var distinctValues = dtdebit.AsEnumerable()
                        //.Select(row => new
                        //{
                        //    InvoiceNo = row.Field<string>("InvoiceNo"),
                        //    InvoiceDate = row.Field<string>("InvoiceDate")
                        //})
                        //.Distinct();

                        DataView viewin = new DataView(dtinvoice);
                        distinctinvoice = viewin.ToTable(true, "InvoiceNo", "GST_YesNo");
                        DataView view = new DataView(dtdebit);
                        distinctdebit = view.ToTable(true, "InvoiceNo", "RCM_YesNo");

                        for (int j = 0; j <= dtinvoice.Rows.Count - 1; j++)
                        {
                            isamount = dtinvoice.Rows[j]["InvoiceNo"].ToString().Trim();
                            isrcm = dtinvoice.Rows[j]["GST_YesNo"].ToString().Trim().Substring(0, 1).ToUpper();

                            for (int k = 0; k <= dtdebit.Rows.Count - 1; k++)
                            {
                                if (dtinvoice.Rows[j]["InvoiceNo"].ToString().Trim() == dtdebit.Rows[k]["InvoiceNo"].ToString())
                                {
                                    if (isrcm == dtdebit.Rows[k]["RCM_YesNo"].ToString().Trim().Substring(0, 1).ToUpper())
                                    {
                                        error = "13"; //Invalid RCM Flag Details
                                        objparent.Add(error);
                                        return Json(error, JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }

                        }


                        view = new DataView(dtdebit);
                        distinctdebit = view.ToTable(true, "InvoiceNo", "InvoiceDate");
                        viewin = new DataView(dtinvoice);
                        distinctinvoice = viewin.ToTable(true, "InvoiceNo", "InvoiceDate");

                        if (dtinvoice.Rows.Count > distinctdebit.Rows.Count)
                        {
                            error = "14"; //Invalid Expense Details
                            objparent.Add(error);
                            return Json(error, JsonRequestBehavior.AllowGet);
                        }
                        else if (dtinvoice.Rows.Count < distinctdebit.Rows.Count)
                        {
                            error = "15"; //Invalid Debit line Details are added.
                            objparent.Add(error);
                            return Json(error, JsonRequestBehavior.AllowGet);
                        }
                        if (distinctinvoice.Rows.Count == distinctdebit.Rows.Count)
                        {
                            //DataTable dtMerged = (from a in distinctinvoice.AsEnumerable()
                            //                      join b in distinctdebit.AsEnumerable()
                            //                      on a["InvoiceNo"].ToString() equals b["InvoiceNo"].ToString()   
                            //                      into g
                            //                      where g.Count() > 0
                            //                      select a).CopyToDataTable();

                            //DataTable dt3 = (from r in distinctdebit.AsEnumerable()
                            //                 where !distinctinvoice.AsEnumerable().Any(r2 => 
                            //            r["InvoiceNo"].ToString().Trim().ToLower() == r2["InvoiceNo"].ToString().Trim().ToLower()
                            //            && r["InvoiceDate"].ToString().Trim().ToLower() == r2["InvoiceDate"].ToString().Trim().ToLower()
                            //            //&& r["Email"].ToString().Trim().ToLower() == r2["Email"].ToString().Trim().ToLower()
                            //            //&& r["POBox"].ToString().Trim().ToLower() == r2["POBox"].ToString().Trim().ToLower()
                            //            )
                            //     select r).CopyToDataTable();

                            var rowsOnlyInDt1 = distinctdebit.AsEnumerable().Where(r => !distinctinvoice.AsEnumerable()
                                .Any(r2 => r["InvoiceNo"].ToString().ToLower() == r2["InvoiceNo"].ToString().Trim().ToLower()
                                    && r["InvoiceDate"].ToString().Trim().ToLower() == r2["InvoiceDate"].ToString().Trim().ToLower()));

                            if (rowsOnlyInDt1 != null && rowsOnlyInDt1.Count() > 0)
                            {
                                result = rowsOnlyInDt1.CopyToDataTable();//The third table
                                int dfdf = result.Rows.Count;
                            }

                            if (result.Rows.Count > 0)
                            {
                                error = "16"; //Invalid Invoice No and Invoice Date are in Debitline Sheet. 
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                        }




                        if (error == "0")
                        {
                            DataSet uploadds = new DataSet();
                            uploadds.Tables.Add(dtinvoice.Copy());
                            uploadds.Tables[0].TableName = "Invoice";
                            uploadds.Tables.Add(dtdebit.Copy());
                            uploadds.Tables[1].TableName = "Debitline";
                            //string  invoiceXML = Excelds.GetXml();
                            StringWriter sw = new StringWriter();

                            uploadds.WriteXml(sw, XmlWriteMode.IgnoreSchema);
                            string strtoXML = "";
                            strtoXML = sw.ToString().Replace("_x0020_", "");
                            strtoXML = strtoXML.Replace("_x002F_", "_");
                            strtoXML = strtoXML.Replace("_x0028_", "");
                            strtoXML = strtoXML.Replace("_x0029_", "");

                            string modifiedXml = Regex.Replace(strtoXML,
                                            @"<ReceivedDate>(?<year>\d{4})-(?<month>\d{2})-(?<date>\d{2}).*?</ReceivedDate>",
                                //@"<ReceivedDate>${month}/${date}/${year}</Received Date>",
                                              @"<ReceivedDate>${year}-${month}-${date}</ReceivedDate>",
                                            RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                            strtoXML = Regex.Replace(modifiedXml,
                                            @"<InvoiceDate>(?<year>\d{4})-(?<month>\d{2})-(?<date>\d{2}).*?</InvoiceDate>",
                                            @"<InvoiceDate>${year}-${month}-${date}</InvoiceDate>",
                                            RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                            Session["XMLdata"] = strtoXML;
                            DataTable getbulk = new DataTable();
                            Session["errortable"] = "";
                            getbulk = bulktodatabase(strtoXML, Path.GetFileNameWithoutExtension(savefile.FileName));
                            getbulk.TableName = "Error Invoice Upload";
                            //Session["Tempdataset"] = uploadds;
                            //TempData.Remove("_supplierattmtfileexcel"); 
                            error = "100";
                            if (getbulk.Rows.Count <= 0)
                                error = "150";
                            else
                                Session["errortable"] = getbulk;
                        }
                    }
                }
                Session["supplierattmtfileexcel"] = "";
                Session["saoriginal"] = "";
                //return Json(objparent.ToList(), JsonRequestBehavior.AllowGet);
                return Json(error, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }

           
        }

        public JsonResult UploadFileslocaldddd(string file)
        {

            List<string> objparent = new List<string>();

            string Extensionnew = "";
            string Extension1 = "";
            string error = "0";
            //------------Duplicate Columns Found
            String xmlstr = "";
            DataSet dbcolds = new DataSet();
            ArrayList dbinvoicelist = new ArrayList();
            ArrayList dbdebitList = new ArrayList();
            ArrayList debitinno = new ArrayList();
            ArrayList debitindate = new ArrayList();
            DataTable dtinvoice = new DataTable();
            DataTable dtdebit = new DataTable();
            DataTable distinctinvoice = new DataTable();
            DataTable distinctdebit = new DataTable();
            DataTable result = new DataTable();
            DataTable errortab = new DataTable();
            try
            {
                string path1 = "";
                errortab.Columns.Add("Error S.No.");
                errortab.Columns.Add("Error Line");
                errortab.Columns.Add("Error Description");


                //HttpPostedFileBase upload = Session["supplierattmtfileexcel"] as HttpPostedFileBase; 
                //Stream stream = upload.InputStream;
                //IExcelDataReader reader = null;
                ////string GetExten = Path.GetExtension(upload.FileName); Session["saoriginal"]
                //string GetExten = Path.GetExtension(upload.FileName);
                //TempData["exten"] = "Y";
               // Session["supplierattmtfileexcel"] = file.Trim();
                HttpPostedFileBase savefile = Session["supplierattmtfileexcel"] as HttpPostedFileBase;
                string getfile = file.Trim();
                    string Extension = Path.GetFileName(savefile.FileName);
                    string n = string.Format(objCmnFunctions.GetLoginUserGid().ToString() + "Local-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                    Extension1 = System.IO.Path.GetExtension(savefile.FileName);
                    Extensionnew = n + Extension1;
                    path1 = string.Format("{0}{1}", HoldFileUploadUrlDSA(), Extensionnew);
                    if (System.IO.File.Exists(path1))
                    {
                        System.IO.File.Delete(path1);
                    }
                    savefile.SaveAs(path1);

                    SheetData sobjModel;
                    int count = 0;
                    string sheets = "";
                    FileStream stream = new FileStream(path1, FileMode.Open, FileAccess.Read);

                    DataSet Excelds = new DataSet();

                    if (Extension1.ToLower() == ".xlsx")
                    {
                        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        excelReader.IsFirstRowAsColumnNames = true;
                        Excelds = excelReader.AsDataSet();
                        excelReader.Close();
                    }
                    else if (Extension1.ToLower() == ".xls")
                    {
                        IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                        excelReader.IsFirstRowAsColumnNames = true;
                        Excelds = excelReader.AsDataSet();
                        excelReader.Close();
                    }
                    else
                    {
                        error = "1";  //--Invoice Excel file
                        objparent.Add(error);
                        return Json(error, JsonRequestBehavior.AllowGet);
                    }
                    if (Excelds != null && Excelds.Tables.Count > 0)
                    {
                        DataSet downds = new DataSet();
                        dtinvoice = Excelds.Tables[0];
                        dtdebit = Excelds.Tables[1];

                        DataSet ds = new DataSet();
                        int ss = 0;
                        ds = objModel.forexcel(ss, "A");
                        ds.Tables[0].TableName = "Invoice";
                        ds.Tables[1].TableName = "Debitline";
                        ds.Tables[2].TableName = "State Master";
                        ds.Tables[3].TableName = "HSN Master";
                        ds.Tables[4].TableName = "Function Master";
                        ds.Tables[5].TableName = "Cost Master";
                        ds.Tables[6].TableName = "Product Master";
                        ds.Tables[7].TableName = "OU Master";
                        ds.Tables[8].TableName = "Expane Nature Master";
                        ds.Tables[9].TableName = "Category Master";
                        ds.Tables[10].TableName = "SubCategory Master";
                        Session["downloadDS"] = ds;
                        //-------------Database Columns
                        dbcolds = (DataSet)Session["downloadDS"];
                        DataColumn[] dbs0 = new DataColumn[dbcolds.Tables[0].Columns.Count];
                        for (int i = 0; i < dbs0.Length; i++)
                        {
                            // dbs1[i] = dbcolds.Tables[0].Columns[i];
                            dbinvoicelist.Add(dbcolds.Tables[0].Columns[i].ColumnName.Trim());
                        }

                        //-------------- Invoice Excel List

                        DataColumn[] s1col = new DataColumn[dtinvoice.Columns.Count];
                        for (int i = 0; i < s1col.Length; i++)
                        {
                            //s1col[i] = dtinvoice.Columns[i];
                            if (!dbinvoicelist.Contains(dtinvoice.Columns[i].ColumnName.Trim()))
                            {
                                error = "2"; //Dupicate Invoice columns
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                        }


                       


            //Hashtable hTable = new Hashtable();
            //ArrayList duplicateLists = new ArrayList();

                        DataColumn[] dbs1 = new DataColumn[dbcolds.Tables[1].Columns.Count];
                        for (int i = 0; i < dbs1.Length; i++)
                        {
                            dbdebitList.Add(dbcolds.Tables[1].Columns[i].ColumnName.Trim());
                        }
                        DataColumn[] s2col = new DataColumn[dtdebit.Columns.Count];
                        for (int i = 0; i < s2col.Length; i++)
                        {
                            if (!dbdebitList.Contains(dtdebit.Columns[i].ColumnName.Trim()))
                            {
                                error = "4"; //Dupicate Debitline columns
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                        }

                      


                        /////------------------------------ 
                        string isamount = "";
                        decimal resulter = 0;
                        string isgstin = "";
                        string isrcm = "";
                        for (int m = 0; m < dtinvoice.Rows.Count; m++)
                        {
                             

                            if (dtinvoice.Rows[m]["ReceivedDate"].ToString().Trim() == "")
                            {
                                error = "6"; //Location should be provide if GST Yes 
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                            if (dtinvoice.Rows[m]["ReceivedDate"].ToString().Trim() != "")
                            {
                                string rsdate = dtinvoice.Rows[m]["ReceivedDate"].ToString().Trim();
                                //DateTime rdate = Convert.ToDateTime(rsdate);
                                //string todaydate = DateTime.Now.ToString("dd/MM/yyyy");
                                DateTime dtSuppliedDate = DateTime.Parse(rsdate);
                                if (dtSuppliedDate.Subtract(DateTime.Today).Days > 0)
                                {
                                    error = "17"; //Location should be provide if GST Yes 
                                    objparent.Add(error);
                                    return Json(error, JsonRequestBehavior.AllowGet);
                                }
                            }
                            if (dtinvoice.Rows[m]["RaiserCode"].ToString().Trim() == "")
                            {
                                error = "6"; //Location should be provide if GST Yes 
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                            if (dtinvoice.Rows[m]["SupplierCode"].ToString().Trim() == "")
                            {
                                error = "6"; //Location should be provide if GST Yes 
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                            if (dtinvoice.Rows[m]["InvoiceDate"].ToString().Trim() == "")
                            {
                                error = "6"; //Location should be provide if GST Yes 
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                            if (dtinvoice.Rows[m]["InvoiceDate"].ToString().Trim() != "")
                            {
                                string rsdate = dtinvoice.Rows[m]["InvoiceDate"].ToString().Trim();
                                DateTime dtSuppliedDate = DateTime.Parse(rsdate);
                                if (dtSuppliedDate.Subtract(DateTime.Today).Days > 0)
                                {
                                    error = "17"; //Location should be provide if GST Yes 
                                    objparent.Add(error);
                                    return Json(error, JsonRequestBehavior.AllowGet);
                                }
                                DateTime vdate = Convert.ToDateTime(dtinvoice.Rows[m]["ReceivedDate"].ToString().Trim());
                                if (dtSuppliedDate.Subtract(vdate).Days > 0)
                                {
                                    error = "18"; //Location should be provide if GST Yes 
                                    objparent.Add(error);
                                    return Json(error, JsonRequestBehavior.AllowGet);
                                }
                            }

                            if (dtinvoice.Rows[m]["Remarks"].ToString().Trim() == "")
                            {
                                error = "6"; //Location should be provide if GST Yes 
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                            isgstin = dtinvoice.Rows[m]["SupplierType"].ToString().Trim().Substring(0, 1).ToUpper();
                            if (!(isgstin.Contains("A") || isgstin.Contains("U")))
                            {
                                error = "8"; //Supplier Type Approved or UnApproved
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                            isamount = dtinvoice.Rows[m]["CurrencyAmount"].ToString().Trim();
                            isgstin = dtinvoice.Rows[m]["GST_YesNo"].ToString().Trim().Substring(0, 1).ToUpper();
                            if (!System.Decimal.TryParse(isamount, out resulter))
                            {
                                error = "7"; //Currency Amount Columns has ivalid Amount format.
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                            if (!(isgstin.Contains("Y") || isgstin.Contains("N")))
                            {
                                error = "9"; //only Applicable GST Yes or No
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                            if (dtinvoice.Rows[m]["ProviderLocation"].ToString().Trim() == "" || dtinvoice.Rows[m]["ReceiverLocation"].ToString().Trim() == "")
                            {
                                error = "10"; //Location should be provide if GST Yes 
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                            isgstin = dtinvoice.Rows[m]["InvoiceType"].ToString().Trim().ToUpper();
                            if (!(isgstin.Contains("PO") || isgstin.Contains("WO") || isgstin.Contains("NON POWO")))
                            {
                                error = "11"; //only Applicable Invoice Type Yes or No
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                            if (isgstin != "NON POWO")
                            {
                                error = "12"; //POWO No are invalid 
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                        }

                        for (int m = 0; m < dtdebit.Rows.Count; m++)
                        {
                            for (int i = 0; i < dtdebit.Columns.Count; i++)
                            {
                                if (dtdebit.Rows[m][i].ToString().Trim() == "")
                                {
                                    error = "6"; //Debitline Empty Rows 
                                    objparent.Add(error);
                                    return Json(error, JsonRequestBehavior.AllowGet);
                                }
                            }

                            isamount = dtdebit.Rows[m]["Amount"].ToString().Trim();
                            if (!System.Decimal.TryParse(isamount, out resulter))
                            {
                                error = "7"; //Debit Amount Columns has ivalid Amount format.
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                            isgstin = dtdebit.Rows[m]["RCM_YesNo"].ToString().Trim().Substring(0, 1).ToUpper();
                            if (!(isgstin.Contains("Y") || isgstin.Contains("N")))
                            {
                                error = "9"; //only RCM GST Yes or No
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                        }
                        

                        DataView viewin = new DataView(dtinvoice);
                        distinctinvoice = viewin.ToTable(true, "InvoiceNo", "GST_YesNo");
                        DataView view = new DataView(dtdebit);
                        distinctdebit = view.ToTable(true, "InvoiceNo", "RCM_YesNo");

                        for (int j = 0; j <= dtinvoice.Rows.Count - 1; j++)
                        {
                            isamount = dtinvoice.Rows[j]["InvoiceNo"].ToString().Trim();
                            isrcm = dtinvoice.Rows[j]["GST_YesNo"].ToString().Trim().Substring(0, 1).ToUpper();

                            for (int k = 0; k <= dtdebit.Rows.Count - 1; k++)
                            {
                                if (dtinvoice.Rows[j]["InvoiceNo"].ToString().Trim() == dtdebit.Rows[k]["InvoiceNo"].ToString())
                                {
                                    if (isrcm == dtdebit.Rows[k]["RCM_YesNo"].ToString().Trim().Substring(0, 1).ToUpper())
                                    {
                                        error = "13"; //Invalid RCM Flag Details
                                        objparent.Add(error);
                                        return Json(error, JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }
                        }


                        view = new DataView(dtdebit);
                        distinctdebit = view.ToTable(true, "InvoiceNo", "InvoiceDate");
                        viewin = new DataView(dtinvoice);
                        distinctinvoice = viewin.ToTable(true, "InvoiceNo", "InvoiceDate");

                        if (dtinvoice.Rows.Count > distinctdebit.Rows.Count)
                        {
                            error = "14"; //Invalid Expense Details
                            objparent.Add(error);
                            return Json(error, JsonRequestBehavior.AllowGet);
                        }
                        else if (dtinvoice.Rows.Count < distinctdebit.Rows.Count)
                        {
                            error = "15"; //Invalid Debit line Details are added.
                            objparent.Add(error);
                            return Json(error, JsonRequestBehavior.AllowGet);
                        }
                        if (distinctinvoice.Rows.Count == distinctdebit.Rows.Count)
                        {
                            
                            var rowsOnlyInDt1 = distinctdebit.AsEnumerable().Where(r => !distinctinvoice.AsEnumerable()
                                .Any(r2 => r["InvoiceNo"].ToString().ToLower() == r2["InvoiceNo"].ToString().Trim().ToLower()
                                    && r["InvoiceDate"].ToString().Trim().ToLower() == r2["InvoiceDate"].ToString().Trim().ToLower()));

                            if (rowsOnlyInDt1 != null && rowsOnlyInDt1.Count() > 0)
                            {
                                result = rowsOnlyInDt1.CopyToDataTable();//The third table
                                int dfdf = result.Rows.Count;
                            }

                            if (result.Rows.Count > 0)
                            {
                                error = "16"; //Invalid Invoice No and Invoice Date are in Debitline Sheet. 
                                objparent.Add(error);
                                return Json(error, JsonRequestBehavior.AllowGet);
                            }
                        }

                        if (error == "0")
                        {
                            DataSet uploadds = new DataSet();
                            uploadds.Tables.Add(dtinvoice.Copy());
                            uploadds.Tables[0].TableName = "Invoice";
                            uploadds.Tables.Add(dtdebit.Copy());
                            uploadds.Tables[1].TableName = "Debitline";
                            //string  invoiceXML = Excelds.GetXml();
                            StringWriter sw = new StringWriter();

                            uploadds.WriteXml(sw, XmlWriteMode.IgnoreSchema);
                            string strtoXML = "";
                            strtoXML = sw.ToString().Replace("_x0020_", "");
                            strtoXML = strtoXML.Replace("_x002F_", "_");
                            strtoXML = strtoXML.Replace("_x0028_", "");
                            strtoXML = strtoXML.Replace("_x0029_", "");

                            string modifiedXml = Regex.Replace(strtoXML,
                                            @"<ReceivedDate>(?<year>\d{4})-(?<month>\d{2})-(?<date>\d{2}).*?</ReceivedDate>",
                                //@"<ReceivedDate>${month}/${date}/${year}</Received Date>",
                                              @"<ReceivedDate>${year}-${month}-${date}</ReceivedDate>",
                                            RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                            strtoXML = Regex.Replace(modifiedXml,
                                            @"<InvoiceDate>(?<year>\d{4})-(?<month>\d{2})-(?<date>\d{2}).*?</InvoiceDate>",
                                            @"<InvoiceDate>${year}-${month}-${date}</InvoiceDate>",
                                            RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                            Session["XMLdata"] = strtoXML;
                            DataTable getbulk = new DataTable();
                            getbulk = bulktodatabase(strtoXML, Path.GetFileNameWithoutExtension(savefile.FileName));
                            getbulk.TableName = "Error Invoice Upload"; 
                            error = "100";
                            if (getbulk.Rows.Count <= 0)
                                error = "150";
                            else
                                Session["errortable"] = getbulk;
                        }
                    } 
                return Json(error, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
 
        }
        public DataTable bulktodatabase(string bulkinvoice, string fileupload)
        {
            DataTable getdata = new DataTable();
            getdata = objModel.bulkinvoice(bulkinvoice, fileupload);
            return (getdata);
        }
        public string ConvertDatatableToXML(DataTable dt)
        {
            MemoryStream str = new MemoryStream();
            dt.WriteXml(str, true);
            str.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(str);
            string xmlstr;
            xmlstr = sr.ReadToEnd();
            return (xmlstr);
        }
        public ActionResult downloadTemplets()
        {
            DataSet ds = new DataSet();
            int ss = 0;
            ds = objModel.forexcel(ss, "A");
            ds.Tables[0].TableName = "Invoice";
            ds.Tables[1].TableName = "Debitline";
            ds.Tables[2].TableName = "State Master";
            ds.Tables[3].TableName = "HSN Master";
            ds.Tables[4].TableName = "Function Master";
            ds.Tables[5].TableName = "Cost Master";
            ds.Tables[6].TableName = "Product Master";
            ds.Tables[7].TableName = "OU Master";
            ds.Tables[8].TableName = "Expane Nature Master";
            ds.Tables[9].TableName = "Category Master";
            ds.Tables[10].TableName = "SubCategory Master";
            Session["downloadDS"] = ds;
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=BulkInvoice_Upload.xlsx");

                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);

                    Response.Flush();
                    Response.End();
                }
            }
            return View();
        }
        public ActionResult ErrordownloadTemplets()
        {
            DataTable dt = new DataTable();
            dt = (DataTable)Session["errortable"];
            dt.TableName = "Error Invoice_bulk";
            using (XLWorkbook wb = new XLWorkbook())
            {

                wb.Worksheets.Add(dt);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=BulkInvoice_Error.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            return View();
        }
    }
}
