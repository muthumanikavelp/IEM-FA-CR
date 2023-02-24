using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using IEM.Areas.EOW.Models;
using System.Reflection;
using System.Text;
using IEM.Common;
using System.Text.RegularExpressions;
using System.IO;
using System.Configuration;
using IEM.App_Start;

namespace IEM.Areas.EOW.Controllers
{
    [NoDirectAccess]
    public class CentralMakerController : Controller
    {
        CentraldataModel sub = new CentraldataModel();
        EOW_DataModel EOWDataModel = new EOW_DataModel();
        private FileServer Cmnfs = new FileServer();
        private CentralRepository objModel;
        String result = String.Empty;
        DataTable dt = new DataTable();
        int CentraiId;
        string sup;
        CentralModel cen = new CentralModel();
        string[] splitvalues;
        List<string> list = new List<string>();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        CommonIUD objCommonIUD = new CommonIUD();
        public CentralMakerController()
            : this(new CentralModel())
        {

        }
        public CentralMakerController(CentralRepository objM)
        {
            objModel = objM;
        }

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult CentralMakerIndex(string viewfor = null)
        {
            try
            {
                //list.Add("Approved");
                //list.Add("UnApproved");
                //List<string> Rolelist = new List<string>();
                //{
                //    Rolelist = list;
                //};
                //List<SelectListItem> role = new List<SelectListItem>();
                //foreach (var item in Rolelist)
                //{
                //    role.Add(new SelectListItem
                //    {
                //        Text = item.ToString(),
                //        //Value = item.ToString()
                //    });
                //}
                //ViewBag.SupplierType = role;

                //List<CentraldataModel> obj = new List<CentraldataModel>();
                //if (TempData["check"] != "search")
                //{
                //    Session["MakerViewbagdata"] = null;
                //    Session["makerrecdateto"] = null;
                //    Session["CentralMaker"] = null;
                //}
                //if (Session["CentralMaker"] != null)
                //{
                //    obj = (List<CentraldataModel>)Session["CentralMaker"];

                //    CentraldataModel Viewbags = new CentraldataModel();
                //    Viewbags = (CentraldataModel)Session["MakerViewbagdata"];
                //    if (Session["makerrecdateto"] != null)
                //    {
                //        ViewBag.RecivedDateTo = Session["makerrecdateto"].ToString();
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
                //    List<string> Rolelist1 = new List<string>();
                //    {
                //        Rolelist1 = list;
                //    };
                //    List<SelectListItem> role1 = new List<SelectListItem>();
                //    foreach (var item in Rolelist1)
                //    {
                //        bool selected = false;
                //        if (item == Viewbags.StringSupplierType.ToString())
                //        {
                //            selected = true;
                //        }
                //        role1.Add(new SelectListItem
                //        {
                //            Text = item.ToString(),
                //            Selected = selected
                //        });
                //    }
                //    ViewBag.SupplierType = role1;
                //}
                //else
                //{
                //    obj = objModel.SelectCentralMaker().ToList();
                //}

                //if (obj.Count == 0)
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
        [HttpGet]
        public ActionResult ClearMaker()
        {
            try
            {
                Session["MakerViewbagdata"] = null;
                Session["makerrecdateto"] = null;
                Session["CentralMaker"] = null;
                return RedirectToAction("CentralMakerIndex", "CentralMaker");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult CentralMakerIndex(string modeNf = null, string RecivedDateFrom = null, string RecivedDateTo = null, string invoiceAmount = null, string InvoiceDate = null, string raisercode = null, string raisername = null, string suppliercode = null, string Suppliername = null, string Department = null, string InvoiceNo = null, string PONO = null, string command = null, string SupplierType = "")
        {
            try
            {
                CentraldataModel Viewbags = new CentraldataModel();
                Session["makerrecdateto"] = RecivedDateTo;
                TempData["check"] = "Search";
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
                    Session["MakerViewbagdata"] = Viewbags;

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
                    obj = objModel.SearchMaker(SupplierType, RecivedDateFrom, RecivedDateTo, invoiceAmount, InvoiceDate, raisercode, raisername, suppliercode, Suppliername, Department, InvoiceNo, PONO).ToList();
                    Session["CentralMaker"] = obj;
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
        public PartialViewResult CentralMakerView(int id)
        {
            try
            {
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
        public PartialViewResult ReturnView(string[] SelectedValues)
        {
            Session["SelectedValues"] = SelectedValues;
            return PartialView();
        }
        public JsonResult Return(string[] SelectedValues, string Remarks)
        {
            try
            {
                if (Session["SelectedValues"] != null)
                {
                    SelectedValues = Session["SelectedValues"] as string[];
                }
                if (SelectedValues != null)
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (string value in SelectedValues)
                    {
                        builder.Append(value);
                    }
                    string arrayvalue = builder.ToString();
                    splitvalues = Regex.Split(arrayvalue, ",");
                }

                result = objModel.UpdateCentralReturnback(splitvalues);
                result = objModel.AddReturn(splitvalues, Remarks);
                if (result == "sub")
                {
                    result = "Inward Returned Successfully";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult HoldView(string[] SelectedValues)
        {
            Session["SelectedHoldValues"] = SelectedValues;
            return PartialView();
        }
        public JsonResult Hold(string[] SelectedValues, string Remarks)
        {
            try
            {
                if (Session["SelectedHoldValues"] != null)
                {
                    SelectedValues = Session["SelectedHoldValues"] as string[];
                }
                StringBuilder builder = new StringBuilder();
                if (SelectedValues != null)
                {
                    foreach (string value in SelectedValues)
                    {
                        builder.Append(value);
                    }
                    string arrayvalue = builder.ToString();
                    splitvalues = Regex.Split(arrayvalue, ",");
                }
                result = objModel.UpdateCentralHold(splitvalues);
                result = objModel.AddHold(splitvalues, Remarks);
                if (result == "sub")
                {
                    result = "Successfully Hold";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public ActionResult ClearHold()
        {
            Session["HoldViewbagdata"] = null;
            Session["holdrecdateto"] = null;
            Session["CentralHold"] = null;
            return RedirectToAction("CentralHoldIndex", "CentralMaker");
        }
        [HttpGet]
        public ActionResult CentralHoldIndex()
        {
            try
            {
                //list.Add("Approved");
                //list.Add("UnApproved");
                //List<string> Rolelist = new List<string>();
                //{
                //    Rolelist = list;
                //};
                //List<SelectListItem> role = new List<SelectListItem>();
                //foreach (var item in Rolelist)
                //{
                //    role.Add(new SelectListItem
                //    {
                //        Text = item.ToString(),
                //        //Value = item.ToString()
                //    });
                //}
                //ViewBag.SupplierType = role;

                //List<CentraldataModel> obj = new List<CentraldataModel>();
                //if (TempData["checkHold"] != "Search")
                //{
                //    Session["HoldViewbagdata"] = null;
                //    Session["holdrecdateto"] = null;
                //    Session["CentralHold"] = null;
                //}
                //if (Session["CentralHold"] != null)
                //{
                //    obj = (List<CentraldataModel>)Session["CentralHold"];

                //    CentraldataModel Viewbags = new CentraldataModel();
                //    Viewbags = (CentraldataModel)Session["HoldViewbagdata"];
                //    if (Session["holdrecdateto"] != null)
                //    {
                //        ViewBag.RecivedDateTo = Session["holdrecdateto"].ToString();
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
                //    List<string> Rolelist1 = new List<string>();
                //    {
                //        Rolelist1 = list;
                //    };
                //    List<SelectListItem> role1 = new List<SelectListItem>();
                //    foreach (var item in Rolelist1)
                //    {
                //        bool selected = false;
                //        if (item == Viewbags.StringSupplierType.ToString())
                //        {
                //            selected = true;
                //        }
                //        role1.Add(new SelectListItem
                //        {
                //            Text = item.ToString(),
                //            Selected = selected
                //        });
                //    }
                //    ViewBag.SupplierType = role1;
                //}
                //else
                //{
                //    obj = objModel.SelectCentralHold().ToList();
                //}
                //if (obj.Count == 0)
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
        public ActionResult CentralHoldIndex(string modeNf = null, string RecivedDateFrom = null, string RecivedDateTo = null, string invoiceAmount = null, string InvoiceDate = null, string raisercode = null, string raisername = null, string suppliercode = null, string Suppliername = null, string Department = null, string InvoiceNo = null, string PONO = null, string command = null, string SupplierType = "")
        {
            try
            {
                TempData["checkHold"] = "Search";
                Session["holdrecdateto"] = RecivedDateTo;
                CentraldataModel Viewbags = new CentraldataModel();
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
                    Session["HoldViewbagdata"] = Viewbags;

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

                    obj = objModel.SearchHold(SupplierType, RecivedDateFrom, RecivedDateTo, invoiceAmount, InvoiceDate, raisercode, raisername, suppliercode, Suppliername, Department, InvoiceNo, PONO).ToList();
                    Session["CentralHold"] = obj;
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
        public PartialViewResult CentralHoldView(int id)
        {
            try
            {
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
        public PartialViewResult ReturnBackView(string[] SelectedValues)
        {
            Session["SelectedValues"] = SelectedValues;
            return PartialView();
        }
        public JsonResult ReturnBack(string[] SelectedValues, string Remarks)
        {
            try
            {
                if (Session["SelectedValues"] != null)
                {
                    SelectedValues = Session["SelectedValues"] as string[];
                }
                if (SelectedValues != null)
                {
                    StringBuilder builder = new StringBuilder();
                    foreach (string value in SelectedValues)
                    {
                        builder.Append(value);
                    }
                    string arrayvalue = builder.ToString();
                    splitvalues = Regex.Split(arrayvalue, ",");
                }
                result = objModel.UpdateCentralReturnback(splitvalues);
                result = objModel.AddReturn(splitvalues, Remarks);
                if (result == "sub")
                {
                    result = "Inward Returned Successfully";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult HoldReleaseView(string[] SelectedValues)
        {
            Session["SelectedHoldValues"] = SelectedValues;
            return PartialView();
        }
        public JsonResult HoldRelease(string[] SelectedValues, string Remarks)
        {
            try
            {
                if (Session["SelectedHoldValues"] == null)
                {
                    SelectedValues = Session["SelectedHoldValues"] as string[];
                }
                //if (SelectedValues != null)
                //{
                //    StringBuilder builder = new StringBuilder();
                //    foreach (string value in SelectedValues)
                //    {
                //        if (value != "0")
                //        {
                //            builder.Append(value);
                //        }

                //    }
                //    string arrayvalue = builder.ToString();
                //    splitvalues = Regex.Split(arrayvalue, ",");
                //}
                result = objModel.UpdateCentralHoldRelease(SelectedValues);
                result = objModel.UpdateHold(SelectedValues, Remarks);
                if (result == "sub")
                {
                    result = "Successfully Released";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult CentralMakerDetails()
        {
            try
            {
                Session["RejectProcess"] = "";
                List<CentraldataModel> obj = new List<CentraldataModel>();
                obj = objModel.CentralCheckerDetails(objCmnFunctions.GetLoginUserGid().ToString()).ToList();
                if (obj.Count == 0)
                {
                    ViewBag.Message = "No Record's Found";
                }
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult CentralRejectDetails()
        {
            try
            {
                Session["RejectProcess"] = "Reject";
                List<CentraldataModel> obj = new List<CentraldataModel>();
                obj = objModel.CentralRejectDetails().ToList();
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public PartialViewResult CentralMakerDetails(string txtdbdocdate, string txtdbdocno, string txtdbdocamount)
        {
            try
            {
                List<CentraldataModel> obj = new List<CentraldataModel>();
                ViewBag.txtdbdocdate = txtdbdocdate;
                ViewBag.txtdbdocno = txtdbdocno;
                ViewBag.txtdbdocamount = txtdbdocamount;
                if (txtdbdocdate == "")
                {
                    obj = objModel.SelectCentralCheckerDetails(txtdbdocdate, txtdbdocno, txtdbdocamount).ToList();
                }
                else
                {
                    if (string.IsNullOrEmpty(txtdbdocdate) == false || string.IsNullOrWhiteSpace(txtdbdocdate) == false)
                    {
                        obj = objModel.SelectCentralCheckerDetails(txtdbdocdate, txtdbdocno, txtdbdocamount).ToList();
                        obj = obj.Where(x => txtdbdocdate == null ||
                               (x.DocDate.Contains(txtdbdocdate))).ToList();
                    }
                }
                if (obj.Count == 0)
                {
                    ViewBag.Message = "No Record's Found";
                }
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public PartialViewResult CentralRejectDetails(string txtdbdocdate, string txtdbdocno, string txtdbdocamount, string command = null)
        {
            try
            {
                List<CentraldataModel> obj = new List<CentraldataModel>();
                ViewBag.txtdbdocdate = txtdbdocdate;
                ViewBag.txtdbdocno = txtdbdocno;
                ViewBag.txtdbdocamount = txtdbdocamount;

                if (command == "Clear")
                {
                    //List<CentraldataModel> obj = new List<CentraldataModel>();
                    obj = objModel.CentralRejectDetails().ToList();
                }
                else
                {
                    if (txtdbdocdate == "")
                    {
                        obj = objModel.SelectrejectDetails(txtdbdocdate, txtdbdocno, txtdbdocamount).ToList();
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(txtdbdocdate) == false || string.IsNullOrWhiteSpace(txtdbdocdate) == false)
                        {
                            obj = objModel.SelectrejectDetails(txtdbdocdate, txtdbdocno, txtdbdocamount).ToList();
                            obj = obj.Where(x => txtdbdocdate == null ||
                                   (x.DocDate.Contains(txtdbdocdate))).ToList();
                        }
                    }

                }
                if (obj.Count == 0)
                {
                    ViewBag.Message = "No Record's Found";
                }

                return PartialView(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public JsonResult RaiseecfscreenReject(string[] SelectedValues)
        //{
        //    decimal Cecfamount = 0;
        //    string resultdata = objModel.EcfraiserReject(SelectedValues);
        //    if (Session["CentralTables"] != null)
        //    {
        //        EOW_Supplierinvoice seowemp = new EOW_Supplierinvoice();
        //        DataTable dtmain = new DataTable();
        //        dtmain = (DataTable)Session["CentralTables"];

        //        if (dtmain.Rows.Count > 0)
        //        {
        //            Cecfamount = Convert.ToDecimal(dtmain.Rows[0]["centralinward_invoice_amount"].ToString());
        //        }
        //        seowemp.ECF_Amount = Cecfamount.ToString();
        //        Session["Ecfqueueamount"] = Cecfamount.ToString();
        //        seowemp.ECF_Date = DateTime.Now.ToString("dd-MM-yyyy");
        //        seowemp.raisermodeId = "C";

        //        string approved = "R";
        //        seowemp.Grade = Convert.ToString(dtmain.Rows[0]["employee_grade_code"].ToString());
        //        seowemp.Raiser_Code = Convert.ToString(dtmain.Rows[0]["employee_code"].ToString());
        //        seowemp.Raiser_Name = Convert.ToString(dtmain.Rows[0]["employee_name"].ToString());
        //        seowemp.Raiser_Modedata = new SelectList(EOWDataModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", seowemp.raisermodeId);
        //        seowemp.amort = "No";
        //        seowemp.amortdec = "";
        //        seowemp.from = "";
        //        seowemp.to = "";

        //        if (approved.ToString() == "A")
        //        {
        //            seowemp.Suppliergid = Convert.ToString(dtmain.Rows[0]["centralinward_supplier_gid"].ToString());
        //            seowemp.Suppliercode = objModel.selectsuppcode(Convert.ToString(dtmain.Rows[0]["centralinward_supplier_gid"].ToString()));
        //            seowemp.Suppliername = Convert.ToString(dtmain.Rows[0]["centralinward_supplier_name"].ToString());
        //        }
        //        else
        //        {
        //            seowemp.Suppliergid = "0";
        //            seowemp.Suppliercode = "";
        //            seowemp.Suppliername = Convert.ToString(dtmain.Rows[0]["centralinward_supplier_name"].ToString());
        //        }
        //        DataTable dt = objModel.selectcurrency("INR");
        //        seowemp.CurrencyName = dt.Rows[0]["currency_code"].ToString();
        //        seowemp.Exrate = dt.Rows[0]["currencyrate_value"].ToString();
        //        seowemp.CurrencyId = dt.Rows[0]["currency_gid"].ToString();
        //        seowemp.Currencyamount = Cecfamount.ToString();
        //        seowemp.DocName = "0";
        //        seowemp.DocId = "0";

        //        seowemp.Raiser_Modedata = new SelectList(EOWDataModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", seowemp.raisermodeId);
        //        seowemp.doctypedata = new SelectList(EOWDataModel.GetDoctype().ToList(), "DocId", "DocName", seowemp.DocId);
        //        seowemp.Currencydata = new SelectList(EOWDataModel.GetCurrency().ToList(), "CurrencyId", "CurrencyName", seowemp.CurrencyId);
        //        Session["CentralTablesMain"] = seowemp;
        //    }
        //    return Json(resultdata, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult Raiseecfscreen(string[] SelectedValues)
        {
            try
            {
                Session["EcfGid"] = null;
                decimal Cecfamount = 0;
                decimal Ccuramount = 0;
                string resultdata = objModel.Ecfraiser(SelectedValues);
                if (Session["CentralTables"] != null)
                {
                    EOW_Supplierinvoice seowemp = new EOW_Supplierinvoice();
                    DataTable dtmain = new DataTable();
                    dtmain = (DataTable)Session["CentralTables"];

                    if (dtmain.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtmain.Rows.Count; i++)
                        {
                            if (Cecfamount == 0)
                            {
                                Cecfamount = Convert.ToDecimal(dtmain.Rows[i]["centralinward_invoice_amount"].ToString());
                                Ccuramount = Convert.ToDecimal(dtmain.Rows[i]["centralinward_currencyamount"].ToString());
                            }
                            else
                            {
                                Cecfamount = Cecfamount + Convert.ToDecimal(dtmain.Rows[i]["centralinward_invoice_amount"].ToString());
                                Ccuramount = Ccuramount + Convert.ToDecimal(dtmain.Rows[i]["centralinward_currencyamount"].ToString());
                            }
                        }
                    }
                    seowemp.ECF_Amount = Cecfamount.ToString();
                    Session["Ecfqueueamount"] = Cecfamount.ToString();
                    seowemp.ECF_Date = DateTime.Now.ToString("dd-MM-yyyy");
                    seowemp.raisermodeId = "C";

                    string approved = Convert.ToString(dtmain.Rows[0]["centralinward_supplier_type"].ToString());
                    seowemp.Grade = Convert.ToString(dtmain.Rows[0]["employee_grade_code"].ToString());
                    seowemp.Raiser_Code = Convert.ToString(dtmain.Rows[0]["employee_code"].ToString());
                    seowemp.raisermodeName = Convert.ToString(dtmain.Rows[0]["employee_gid"].ToString());
                    seowemp.Raiser_Name = Convert.ToString(dtmain.Rows[0]["employee_name"].ToString());
                    seowemp.Raiser_Modedata = new SelectList(EOWDataModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", seowemp.raisermodeId);
                    seowemp.amort = "No";
                    seowemp.amortdec = "";
                    seowemp.from = "";
                    seowemp.to = "";

                    if (approved.ToString() == "A")
                    {
                        seowemp.Suppliergid = Convert.ToString(dtmain.Rows[0]["centralinward_supplier_gid"].ToString());
                        seowemp.Suppliercode = objModel.selectsuppcode(Convert.ToString(dtmain.Rows[0]["centralinward_supplier_gid"].ToString()));
                        seowemp.Suppliername = Convert.ToString(dtmain.Rows[0]["centralinward_supplier_name"].ToString());
                    }
                    else
                    {
                        seowemp.Suppliergid = "0";
                        seowemp.Suppliercode = "";
                        seowemp.Suppliername = Convert.ToString(dtmain.Rows[0]["centralinward_supplier_name"].ToString());
                    }
                    DataTable dt = objModel.selectcurrency(dtmain.Rows[0]["currency_code"].ToString());
                    if (dt.Rows.Count > 0)
                    {
                        seowemp.CurrencyName = dt.Rows[0]["currency_code"].ToString();
                        seowemp.Exrate = dt.Rows[0]["currencyrate_value"].ToString();
                        seowemp.CurrencyId = dt.Rows[0]["currency_gid"].ToString();
                    }
                    else
                    {
                        resultdata = "Please maintain Currency Code and Currency Rate ";
                    }
                    seowemp.Currencyamount = Ccuramount.ToString();
                    seowemp.DocName = "0";
                    string suppo = Convert.ToString(dtmain.Rows[0]["centralinward_po_type"].ToString());

                    string powo = suppo.ToString();
                    if (powo == "P")
                    {
                        seowemp.DocId = "1";
                    }
                    else if (powo == "W")
                    {
                        seowemp.DocId = "2";
                    }
                    else if (powo == "U")
                    {
                        seowemp.DocId = "4";
                    }
                    else
                    {
                        seowemp.DocId = "3";
                    }

                    seowemp.Raiser_Modedata = new SelectList(EOWDataModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", seowemp.raisermodeId);
                    seowemp.doctypedata = new SelectList(EOWDataModel.GetDoctype().ToList(), "DocId", "DocName", seowemp.DocId);
                    seowemp.Currencydata = new SelectList(EOWDataModel.GetCurrency().ToList(), "CurrencyId", "CurrencyName", seowemp.CurrencyId);
                    Session["CentralTablesMain"] = seowemp;
                    //Session["Temptaxtable"] = null;
                    //Session["Temppaymenttable"] = null;
                    //Session["Tempdebittable"] = null;
                    //Session["Temppamapptable"] = null;
                    //Session["Temppoitemtable"] = null;
                }
                return Json(resultdata, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetAutodisplayofNextItem()
        {
            string resultstatus="";
            Int32 data = 0;
            List<CentraldataModel> obj = new List<CentraldataModel>();
            try
            {
                Session["RejectProcess"] = "";
                obj = objModel.CentralCheckerDetails(objCmnFunctions.GetLoginUserGid().ToString()).ToList();
                if (obj.Count == 0)
                {
                    ViewBag.Message = "No Record Found"; 
                    resultstatus = "No Record";
                }
                else
                {
                    data =Convert.ToInt32 (obj[0].Docnogid);
                    resultstatus = "success";
                }
            }
            catch (Exception ex)
            {
                resultstatus = ex.Message; 
            }

            return Json(data, resultstatus, JsonRequestBehavior.AllowGet);
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
        public ActionResult selectdata(string id)
        {
            try
            {
                int redirectpage = 0;
                string confirmsupplier = EOWDataModel.selectsupplierinvoice(id.ToString(), "A").ToString();
                string centeam = EOWDataModel.selectsupplierinvoicestatuscen(id.ToString(), "A");
                if (confirmsupplier == "4")
                {
                    EOW_Supplierinvoice seowemp = new EOW_Supplierinvoice();
                    List<EOW_Supplierinvoice> sobjobjMExpense = new List<EOW_Supplierinvoice>();

                    if (centeam == "C")
                    {
                        sobjobjMExpense = EOWDataModel.SelectViewdatasupplier(id, "check").ToList();
                    }
                    else
                    {
                        sobjobjMExpense = EOWDataModel.SelectViewdatasupplier(id, "A").ToList();
                    }

                    seowemp.ECF_Amount = sobjobjMExpense[0].ECF_Amount.ToString();
                    Session["Ecfqueueamount"] = sobjobjMExpense[0].ECF_Amount.ToString();
                    seowemp.ECF_Date = sobjobjMExpense[0].ECF_Date.ToString();
                    seowemp.raisermodeId = sobjobjMExpense[0].raisermodeId.ToString();
                    seowemp.Grade = sobjobjMExpense[0].Grade.ToString();
                    seowemp.Raiser_Code = sobjobjMExpense[0].Raiser_Code.ToString();
                    seowemp.Raiser_Name = sobjobjMExpense[0].Raiser_Name.ToString();
                    seowemp.ecfdescription = sobjobjMExpense[0].ecfdescription.ToString();
                    seowemp.Raiser_Modedata = new SelectList(EOWDataModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", sobjobjMExpense[0].raisermodeId.ToString());
                    seowemp.amort = sobjobjMExpense[0].amort.ToString();
                    seowemp.amortdec = sobjobjMExpense[0].amortdec.ToString();
                    seowemp.from = sobjobjMExpense[0].from.ToString();
                    seowemp.to = sobjobjMExpense[0].to.ToString();
                    seowemp.raisermodeName = sobjobjMExpense[0].raisermodeName.ToString();
                    seowemp.Suppliergid = sobjobjMExpense[0].Suppliergid.ToString();
                    seowemp.Suppliercode = sobjobjMExpense[0].Suppliercode.ToString();
                    seowemp.Suppliername = sobjobjMExpense[0].Suppliername.ToString();
                    seowemp.CurrencyName = sobjobjMExpense[0].CurrencyName.ToString();
                    seowemp.Exrate = sobjobjMExpense[0].Exrate.ToString();
                    seowemp.Currencyamount = sobjobjMExpense[0].Currencyamount.ToString();
                    seowemp.DocName = Convert.ToString(sobjobjMExpense[0].DocName);
                    seowemp.CurrencyId = sobjobjMExpense[0].CurrencyId.ToString();
                    string doc = Convert.ToString(sobjobjMExpense[0].DocName);

                    if (doc == "PO")
                    {
                        seowemp.DocId = "1";
                    }
                    if (doc == "WO")
                    {
                        seowemp.DocId = "2";
                    }
                    if (doc == "Non PO/WO")
                    {
                        seowemp.DocId = "3";
                    }
                    if (doc == "Utility")
                    {
                        seowemp.DocId = "4";
                    }
                    seowemp.Raiser_Modedata = new SelectList(EOWDataModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", seowemp.raisermodeId);
                    seowemp.doctypedata = new SelectList(EOWDataModel.GetDoctype().ToList(), "DocId", "DocName", seowemp.DocId);
                    seowemp.Currencydata = new SelectList(EOWDataModel.GetCurrency().ToList(), "CurrencyId", "CurrencyName", seowemp.CurrencyId);
                    seowemp.ecf_GID = Convert.ToInt32(sobjobjMExpense[0].ecf_GID.ToString());
                    seowemp.Queue_GID = Convert.ToInt32(sobjobjMExpense[0].Queue_GID.ToString());
                    Session["DashBoardcentral"] = seowemp;
                    redirectpage = Convert.ToInt32(confirmsupplier.ToString());
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

                    eowemp.ECF_Amount = objobjMExpense[0].ECF_Amount.ToString();
                    eowemp.ClaimMonth = objobjMExpense[0].ECF_Date.ToString();
                    eowemp.ECF_Date = objobjMExpense[0].ECF_Date.ToString();
                    eowemp.Grade = objobjMExpense[0].Grade.ToString();
                    eowemp.noofperson = objobjMExpense[0].noofperson.ToString();
                    eowemp.Raiser_Code = objobjMExpense[0].Raiser_Code.ToString();
                    eowemp.Raiser_Name = objobjMExpense[0].Raiser_Name.ToString();
                    eowemp.ecfdescription = objobjMExpense[0].ecfdescription.ToString();
                    eowemp.Raiser_Modedata = new SelectList(EOWDataModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", objobjMExpense[0].raisermodeId.ToString());
                    eowemp.ecf_GID = Convert.ToInt32(objobjMExpense[0].ecf_GID.ToString());
                    eowemp.invoice_GID = Convert.ToInt32(objobjMExpense[0].invoice_GID.ToString());
                    eowemp.Queue_GID = Convert.ToInt32(objobjMExpense[0].Queue_GID.ToString());
                    eowemp.SubCategoryName = objobjMExpense[0].SubCategoryName.ToString();
                    Session["DashBoardcentral"] = eowemp;
                    redirectpage = Convert.ToInt32(objobjMExpense[0].Doctypeid.ToString());

                }
                int regular = Convert.ToInt32(ConfigurationManager.AppSettings["EcfRegular"].ToString());
                int travel = Convert.ToInt32(ConfigurationManager.AppSettings["EcfTravel"].ToString());
                int LocalCoveyance = Convert.ToInt32(ConfigurationManager.AppSettings["EcfLocalCoveyance"].ToString());
                int SupplierInvoiceDSA = Convert.ToInt32(ConfigurationManager.AppSettings["EcfSupplierInvoiceDSA"].ToString());
                int SupplierInvoice = Convert.ToInt32(ConfigurationManager.AppSettings["EcfSupplierInvoice"].ToString());

                if (regular == redirectpage)
                {
                    return RedirectToAction("Index", "EmployeeClaimNew");
                }
                if (travel == redirectpage)
                {
                    return RedirectToAction("Index", "TravelClaimNew");
                }
                if (LocalCoveyance == redirectpage)
                {
                    return RedirectToAction("Index", "LocalConveyanceNew");
                }
                if (SupplierInvoiceDSA == redirectpage)
                {
                    return RedirectToAction("Index", "SupplierInvoiceDSA");
                }
                if (SupplierInvoice == redirectpage)
                {
                    return RedirectToAction("Index", "SupplierInvoiceNewc");
                }
                else
                {
                    return RedirectToAction("Index", "RaisingArf");
                }
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult _EmpAttachmentCreatev()
        {
            try
            {
                EOW_File objMAttachment = new EOW_File();
                objMAttachment.AttachmentTypeData = new SelectList(EOWDataModel.AttachmentTypedata().ToList(), "AttachmentTypeId", "AttachmentTypeName");
                return PartialView(objMAttachment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult EmpAttachmentDeletev(EOW_File EmployeeeExpensemodel)
        {
            try
            {
                int EmployeeePaymentGID = (int)EmployeeeExpensemodel.AttachmentFilenameId;
                string delamt = EOWDataModel.DeleteEmployeeeAttachment(EmployeeePaymentGID, Session["EcfGid"].ToString());
                //Session["Ecfamountpayment"] = Convert.ToInt32(Session["Ecfamountpayment"].ToString()) + Convert.ToInt32(delamt.ToString());
                return Json(EmployeeeExpensemodel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult _EmpAttachmentCreatev(EOW_File EmployeeeExpenseModel)
        {
            try
            {
                if (TempData["_attFile"] != null)
                {
                    HttpPostedFileBase savefile = TempData["_attFile"] as HttpPostedFileBase;
                    string getname = EOWDataModel.InsertEmpAtt(savefile, EmployeeeExpenseModel, Session["EcfGid"].ToString(), objCmnFunctions.GetLoginUserGid().ToString());
                    if (getname != "")
                    {
                        //string path = "//192.168.0.203/temp/";
                        //string path = @"C:\temp\";
                        //string path1 = Path.GetFileName(savefile.FileName);
                        //string[] str = path1.Split('.');
                        ////var index = path1.LastIndexOf(".") + 1;
                        //getname = getname + "." + str[1].ToString();
                        //string savedFileName = Path.Combine(path, getname);
                        //savefile.SaveAs(savedFileName);

                        //var fileextension = Path.GetExtension(savefile.FileName);
                        //var stream = savefile.InputStream;
                        //var path = Path.Combine(@"C:\temp\", getname + fileextension);
                        //using (var fileStream = System.IO.File.Create(path))
                        //{
                        //    stream.CopyTo(fileStream);
                        //}
                        //Session["empattmtfile"] = null;

                        //upload the file to server.
                        HttpPostedFileBase _attFile = TempData["_attFile"] as HttpPostedFileBase;
                        var stream = _attFile.InputStream;
                        string uploaderUrl = string.Format("{0}{1}", EOWDataModel.HoldFileUploadUrl(), getname + "." + _attFile.FileName.Split('.')[_attFile.FileName.Split('.').Length - 1]);
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
                    }
                }
                ViewBag.SearchItems = null;
                return Json(EmployeeeExpenseModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult _EmpAttachmentDetailsv()
        {
            try
            {
                List<EOW_File> lstAttachment = new List<EOW_File>();
                lstAttachment = EOWDataModel.GetEmployeeeAttachment(Session["EcfGid"].ToString(), objCmnFunctions.GetLoginUserGid().ToString()).ToList();
                return PartialView(lstAttachment);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult _EmpEmployeeDetailsv()
        {
            try
            {
                List<EOW_Employeelst> lstGetEmployee = new List<EOW_Employeelst>();
                lstGetEmployee = EOWDataModel.GetEmployeeelist(Session["EcfGid"].ToString()).ToList();
                return PartialView(lstGetEmployee);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult _TravelModeDetailsv()
        {
            try
            {
                List<EOW_TravelClaim> lstnew = new List<EOW_TravelClaim>();
                lstnew = EOWDataModel.GetTravelModedata(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), "T").ToList();
                return PartialView(lstnew);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult _OtherTravelExpenseDetailsv()
        {
            try
            {
                List<EOW_TravelClaim> lstnew = new List<EOW_TravelClaim>();
                lstnew = EOWDataModel.GetTravelModedata(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), "E").ToList();
                return PartialView(lstnew);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public FileResult Download(int id)
        {
            try
            {
                string FileName = EOWDataModel.downloadAttachment(id, Session["EcfGid"].ToString());
                ////int index = delamt.LastIndexOf(".");
                ////string[] seqNum = new string[] { delamt.Substring(0, index), delamt.Substring(index + 1) };
                ////string directory = @"C:\temp\";
               // //directory = directory + id.ToString() + "." + seqNum[1].ToString();
                ////byte[] fileBytes = System.IO.File.ReadAllBytes(directory);
                ////string fileName = seqNum[0].ToString() + "." + seqNum[1].ToString();
                ////return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

                string[] str = FileName.Split('.');
                string directory =  id.ToString() + "." + str[str.Length - 1].ToString();
                //string directory = EOWDataModel.HoldFileUploadUrl() + id.ToString() + "." + str[str.Length - 1].ToString();
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
                //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, FileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public void UploadFiles()
        {
            try
            {
                string filename = "";
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                    Session["empattmtfilee"] = hpf;
                }
            }
            catch (Exception)
            {
            }
        }

        public ActionResult Printdata(int id)
        {
            return View();
        }

        public JsonResult _Viewsubmitcon(Approveraction AppModel)
        {
            string Err = "";
            try
            {
                string reject = Convert.ToString(Session["RejectProcess"]);
                if (ModelState.IsValid)
                {
                    //if (reject != "Reject")
                    //{
                    string reaiserchecker = Session["chkraiser"].ToString();
                    EOWDataModel.centralapprovedaction(AppModel, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), objCmnFunctions.GetLoginUserGid().ToString(), Session["EcfType"].ToString(), Session["QueueGid"].ToString(), Session["Ecfqueueamount"].ToString(), Session["chkraiser"].ToString());
                    Err = "Success";
                    Session["DashSearchItems"] = null;
                    Session["DashSearchItemsapp"] = null;
                    // }
                    //else
                    //{
                    //     objModel.Insertapprovedactionreject(AppModel, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), objCmnFunctions.GetLoginUserGid().ToString(), Session["EcfType"].ToString(), Session["QueueGid"].ToString(), Session["Ecfqueueamount"].ToString());
                    //    Err = "SuccessReject";
                    //}
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
                    obj = cen.SelectEmployee().ToList();
                }
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult EmployeeSearcenmaker(string RaiserName = "", string RaiserCode = "")
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
                    recordsearch = cen.SelectEmployeeSearch(RaiserName, RaiserCode).ToList();
                    Session["SearchEmployeedata"] = recordsearch;
                }
                else
                {
                    Session["SerchViewbag"] = null;
                    Session["SerchViewbag"] = null;
                    recordsearch = cen.SelectEmployeeSearch(RaiserName, RaiserCode).ToList();
                }

                return Json(recordsearch, JsonRequestBehavior.AllowGet); ;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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
                        obj = (List<CentraldataModel>)Session["SearchSupp"];
                    }
                }
                else
                {
                    Session["SearchSupp"] = null;
                    obj = cen.SelectSupplier().ToList();
                }
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [HttpPost]
        public JsonResult SupplierSearchcent(string SupplierName = "", string SupplierCode = "",string PanNo = "")
        {
            try
            {
                List<CentraldataModel> obj = new List<CentraldataModel>();
                CentraldataModel supplier = new CentraldataModel();
                if (SupplierCode != "" || SupplierName != "" || PanNo != "")
                {
                    obj = cen.SelectSupplierSearch(SupplierName, SupplierCode,PanNo).ToList();
                    @ViewBag.SupplierName = SupplierName;
                    @ViewBag.SupplierCode = SupplierCode;
                    supplier.SupplierCode = SupplierCode;
                    supplier.SupplierName = SupplierName;
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
                    obj = cen.SelectSupplier().ToList();

                }
                return Json(obj, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetEmployee(ECFDataModel ecf)
        {
            Session["Employeecode"] = ecf.Code;
            Session["EmployeeName"] = ecf.Name;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult CentralInwardView(int id)
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

                return PartialView(cen);
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
            //lstAttachment = objModel.GetinvoiceAttachment(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString()).ToList();
            return PartialView(lstAttachment);
        }
        [HttpGet]
        public JsonResult ToGetctmakerrpt()
        {
            List<CentraldataModel> lstctmaker = new List<CentraldataModel>();
            lstctmaker = objModel.SelectCentralMaker().ToList();
            return Json(lstctmaker, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ToGetholdrelserpt()
        {
            List<CentraldataModel> lstholdrelse = new List<CentraldataModel>();
            lstholdrelse = objModel.SelectCentralHold().ToList();
            var jsonResult = Json(new { qrylist = lstholdrelse }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
    }
}

