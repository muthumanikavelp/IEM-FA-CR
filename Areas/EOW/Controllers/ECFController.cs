using IEM.Areas.EOW.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Text;
using Newtonsoft.Json;
using IEM.Common;
using System.Web.Script.Serialization;
using IEM.App_Start;
namespace IEM.Areas.EOW.Controllers
{
    [NoDirectAccess]
    public class ECFController : Controller
    {
        ErrorLog objErrorLog = new ErrorLog();
        DataTable dt = new DataTable();
        CentraldataModel sub = new CentraldataModel();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        LocalConveyanceNewController locals = new LocalConveyanceNewController();
        private ECFRepository objModel;
        private EOW_IRepository objModelQuery;
        List<string> list = new List<string>();
        String result = String.Empty;
        CentralModel cen = new CentralModel();
        string[] splitvalues;
        int flag1;
      
        public ECFController()
            : this(new ECFModel(), new EOW_DataModel()) 
        {

        } 

        public ECFController(ECFRepository objM, EOW_IRepository objMQ)
        {
            objModel = objM;
            objModelQuery = objMQ;
        }
        [HttpGet]
        public ActionResult Index()
        {
            Session["ToGetecfquerylist"] = null;
            ViewBag.ecfmode = "";
            ViewBag.Docsubtype1 = "";
            //EMPLOYEE OR SUPPLIER DROPDOWN LOAD
            list.Add("Employee");
            list.Add("Supplier");
            List<string> Rolelist1 = new List<string>();
            {
                Rolelist1 = list;
            };
            List<SelectListItem> role1 = new List<SelectListItem>();
            foreach (var item in Rolelist1)
            {

                role1.Add(new SelectListItem
                {
                    Text = item.ToString(),

                });
            }
            ViewBag.SupplierorEmployeeData = role1;

            List<ECFDataModel> cen = new List<ECFDataModel>();
            ECFDataModel ECFDoctype = new ECFDataModel();
            ECFDoctype.selectdoctypedata = new SelectList(objModel.SelectDocType().ToList(), "Docgid", "Docname");
            ViewBag.DocType = ECFDoctype;

            List<ECFDataModel> Rolelist = new List<ECFDataModel>();
            {
                //Rolelist = objModel.SelectDocType().ToList();
            };
            List<SelectListItem> role = new List<SelectListItem>();
            foreach (var item in Rolelist)
            {

                role.Add(new SelectListItem
                {
                    Text = item.Docname.ToString(),
                    Value = item.Docgid.ToString(),
                });
            }
            ViewBag.DocType111 = role;

            ECFDataModel ECFStatusviewbag = new ECFDataModel();
            ECFStatusviewbag.statusnameData = new SelectList(objModel.SelectStatus().ToList(), "StatusGid", "statusname");
            ViewBag.ECFStatus = ECFStatusviewbag;

            if (Session["Records"] != null)
            {
                ECFDataModel viewbags = new ECFDataModel();
                if (Session["SearchData"] != null)
                {
                    viewbags = (ECFDataModel)Session["SearchData"];
                    ViewBag.ecfnumber = viewbags.ECFNo;
                    ViewBag.ecfamount = viewbags.ECFAmount;
                    ViewBag.docvalue = viewbags.statusname;
                    ViewBag.satval = viewbags.ECFStatus;
                    ViewBag.ECFClaimMonth = viewbags.ClaimMonth;
                    ViewBag.EcfDateFrom = viewbags.ecfdatefrom;
                    ViewBag.EcfDateTo = viewbags.ecfdateto;
                    ViewBag.Code = viewbags.Code;
                    ViewBag.Name = viewbags.Name;
                    ViewBag.SupplierorEmployee = viewbags.SupplierorEmployee;
                    ViewBag.ecfmode = viewbags.ecfFor;
                    ViewBag.Docsubtymename = viewbags.DocSubTypeName;
                    ViewBag.SupplierCode = viewbags.SupplierCode;
                    ViewBag.SupplierName = viewbags.SupplierName;
                }
                cen = (List<ECFDataModel>)Session["Records"];
                //if (cen.Count == 0)
                //{
                //    ViewBag.Message = "No Record";
                //}
            }
            else
            {
                //cen = objModel.Search().ToList();
                //if (cen.Count == 0)
                //{
                //    ViewBag.Message = "No Record";
                //}
            }
            return View(cen);

        }
        [HttpPost]
        public JsonResult ViewDespatchinformation(ECFDataModel datamodel)
        {
            List<ECFDataModel> cen = new List<ECFDataModel>();
            cen = objModel.SelectDespatchByEcfNumber(datamodel.ECFNo).ToList();
            ECFDataModel typmemodel = new ECFDataModel();
            if (cen.Count > 0)
            {
                typmemodel.ECFNo = cen[0].ECFNo.ToString();
                typmemodel.ECFDate = cen[0].ECFDate.ToString();
                typmemodel.ECFRaiser = cen[0].ECFRaiser.ToString();
                typmemodel.SupplierorEmployeename = cen[0].SupplierorEmployeename.ToString();
                typmemodel.ClaimMonth = cen[0].ClaimMonth.ToString();
                typmemodel.ECFAmount = cen[0].ECFAmount.ToString();
                // typmemodel.Despatchdate = cen[0].Despatchdate.ToString();
            }

            return Json(typmemodel, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult LogViewecf(string id, string types, string ecfstsus)
        {
            try
            { 
                //string confirmsupplier = objModel.selectsupplierinvoice(id.ToString(), "A").ToString();
                //string centralteam = objModel.selectsupplierinvoicestatuscen(id.ToString(), "A").ToString();
                string centralteam = "";
                string confirmsupplier = "";
                if (types == "D")
                {
                    centralteam = objModelQuery.selectsupplierinvoicestatuscen(id.ToString(), "D").ToString();
                    confirmsupplier = objModelQuery.selectsupplierinvoice(id.ToString(), "D").ToString();
                }
                else
                {
                    centralteam = objModelQuery.selectsupplierinvoicestatuscen(id.ToString(), "A").ToString();
                    confirmsupplier = objModelQuery.selectsupplierinvoice(id.ToString(), "A").ToString();
                }
                //string cemtm = objModel.selectcemtam(id.ToString(), centralteam);
                Session["docAppoalc"] = "docAppoalc";
                if (confirmsupplier == "4")
                {
                    EOW_Supplierinvoice seowemp = new EOW_Supplierinvoice();
                    List<EOW_Supplierinvoice> sobjobjMExpense = new List<EOW_Supplierinvoice>();

                    if (types == "D")
                    {
                        sobjobjMExpense = objModelQuery.SelectViewdatasupplier(id, "ctdata").ToList();
                    }
                    else
                    {
                        if (centralteam == "C" || centralteam == "R")
                        {
                            sobjobjMExpense = objModelQuery.SelectViewdatasupplier(id, "checkview").ToList();
                        }
                        else
                        {
                            sobjobjMExpense = objModelQuery.SelectViewdatasupplier(id, "A").ToList();
                        }
                    }
                    seowemp.ecfno = sobjobjMExpense[0].ecfno.ToString();

                    //seowemp.ECF_Amount = sobjobjMExpense[0].ECF_Amount.ToString();
                    Session["Ecfqueueamount"] = sobjobjMExpense[0].ECF_Amount.ToString();
                    seowemp.ECF_Date = sobjobjMExpense[0].ECF_Date.ToString();
                    seowemp.raisermodeId = sobjobjMExpense[0].raisermodeId.ToString();
                    if (sobjobjMExpense[0].ecfstatus == "262144" || sobjobjMExpense[0].ecfstatus == "524288")
                    {
                        Session["Centralautit"] = "C";
                    }
                    else
                    {
                        Session["Centralautit"] = "S";
                    }
                    seowemp.Grade = sobjobjMExpense[0].Grade.ToString();
                    seowemp.Raiser_Code = sobjobjMExpense[0].Raiser_Code.ToString();
                    seowemp.Raiser_Name = sobjobjMExpense[0].Raiser_Name.ToString();
                    seowemp.ecfdescription = sobjobjMExpense[0].ecfdescription.ToString();
                    seowemp.Raiser_Modedata = new SelectList(objModelQuery.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", sobjobjMExpense[0].raisermodeId.ToString());
                    seowemp.amort = sobjobjMExpense[0].amort.ToString();
                    seowemp.amortdec = sobjobjMExpense[0].amortdec.ToString();
                    seowemp.from = sobjobjMExpense[0].from.ToString();
                    seowemp.to = sobjobjMExpense[0].to.ToString();
                    seowemp.Suppliergid = sobjobjMExpense[0].Suppliergid.ToString();
                    seowemp.Suppliercode = sobjobjMExpense[0].Suppliercode.ToString();
                    seowemp.Suppliername = sobjobjMExpense[0].Suppliername.ToString();
                    seowemp.CurrencyName = sobjobjMExpense[0].CurrencyName.ToString();
                    seowemp.Exrate = sobjobjMExpense[0].Exrate.ToString();
                    seowemp.Currencyamount = sobjobjMExpense[0].Currencyamount.ToString();
                    seowemp.DocName = Convert.ToString(sobjobjMExpense[0].DocName);

                    seowemp.ECF_Amount = objCmnFunctions.GetINRAmount(sobjobjMExpense[0].ECF_Amount.ToString());
                    ViewBag.EOW_supplierExpenseheader = seowemp;
                    ViewBag.doctype = objCmnFunctions.GetDocType(sobjobjMExpense[0].Doctypeid.ToString());
                    Session["EcfType"] = objCmnFunctions.GetDocType(sobjobjMExpense[0].Doctypeid.ToString());
                    //Session["EcfGid"] = sobjobjMExpense[0].ecf_GID.ToString();
                    Session["QEcfGid"] = sobjobjMExpense[0].ecf_GID.ToString();// Prefix Q added to indicate session is coming from Query Screen
                    Session["QinvoiceGid"] = "";// Prefix Q added to indicate session is coming from Query Screen
                    Session["QQueueGid"] = sobjobjMExpense[0].Queue_GID.ToString();// Prefix Q added to indicate session is coming from Query Screen
                    ViewBag.frst = "Suppliers";
                    string potype = Convert.ToString(sobjobjMExpense[0].DocName);
                    if (potype == "PO" || potype == "WO")
                    {
                        ViewBag.POStatus = "PO";
                    }
                }
                else
                {
                    Session["Centralautit"] = "S";

                    EOW_EmployeeeExpense eowemp = new EOW_EmployeeeExpense();
                    List<EOW_EmployeeeExpense> objobjMExpense = new List<EOW_EmployeeeExpense>();

                    confirmsupplier = objModelQuery.selectsupplierinvoice(id.ToString(), "A").ToString();
                    if (confirmsupplier == "6" || confirmsupplier == "7")
                    {
                        ViewBag.frst = "Othersarf";
                        objobjMExpense = objModelQuery.SelectViewdata(id, "AL").ToList();
                    }
                    if (confirmsupplier == "2" || confirmsupplier == "5")
                    {
                        ViewBag.frst = "Others";
                        objobjMExpense = objModelQuery.SelectViewdata(id, "AL").ToList();
                    }
                    else if (confirmsupplier != "2" && confirmsupplier != "5" && confirmsupplier != "6" && confirmsupplier != "7")
                    {
                        ViewBag.frst = "Others";
                        objobjMExpense = objModelQuery.SelectViewdata(id, "A").ToList();
                    }

                    eowemp.ecfno = objobjMExpense[0].ecfno.ToString();

                    eowemp.ECF_Amount = objCmnFunctions.GetINRAmount(objobjMExpense[0].ECF_Amount.ToString());
                    eowemp.ecfdelmatamt = objobjMExpense[0].ecfdelmatamt.ToString();
                    Session["Ecfqueueamount"] = objobjMExpense[0].ECF_Amount.ToString();
                    eowemp.ClaimMonth = locals.getconverttomonthtodateto(objobjMExpense[0].ClaimMonth.ToString());
                    eowemp.ECF_Date = objobjMExpense[0].ECF_Date.ToString();
                    eowemp.Grade = objobjMExpense[0].Grade.ToString();
                    eowemp.Raiser_Code = objobjMExpense[0].Raiser_Code.ToString();
                    eowemp.Raiser_Name = objobjMExpense[0].Raiser_Name.ToString();
                    eowemp.ecfdescription = objobjMExpense[0].ecfdescription.ToString();
                    eowemp.SubCategoryName = objobjMExpense[0].SubCategoryName.ToString();

                    if (eowemp.SubCategoryName == "E")
                    {
                        eowemp.arftype = "Employee";
                        eowemp.arfempsupcode = objobjMExpense[0].Raiser_Code.ToString();
                        eowemp.arfempsupname = objobjMExpense[0].Raiser_Name.ToString();
                    }
                    if (eowemp.SubCategoryName == "P")
                    {
                        eowemp.arftype = "Petty Cash";
                        eowemp.arfempsupcode = objobjMExpense[0].arfempsupcode.ToString();
                        eowemp.arfempsupname = objobjMExpense[0].arfempsupname.ToString();
                    }
                    if (eowemp.SubCategoryName == "S")
                    {
                        eowemp.arftype = "Supplier";
                        if (objobjMExpense[0].arfempsupcode != null)
                        {
                            eowemp.arfempsupcode = objobjMExpense[0].arfempsupcode.ToString();
                        }
                        if (objobjMExpense[0].arfempsupname != null)
                        {
                            eowemp.arfempsupname = objobjMExpense[0].arfempsupname.ToString();
                        }
                    }

                    eowemp.Raiser_Modedata = new SelectList(objModelQuery.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", objobjMExpense[0].raisermodeId.ToString());
                    ViewBag.EOW_EmployeeeExpenseheader = eowemp;
                    ViewBag.doctype = objCmnFunctions.GetDocType(objobjMExpense[0].Doctypeid.ToString());
                    Session["EcfType"] = objCmnFunctions.GetDocType(objobjMExpense[0].Doctypeid.ToString());
                    //Session["EcfGid"] = objobjMExpense[0].ecf_GID.ToString();
                    Session["QEcfGid"] = objobjMExpense[0].ecf_GID.ToString();
                    Session["QinvoiceGid"] = objobjMExpense[0].invoice_GID.ToString();
                    Session["QQueueGid"] = objobjMExpense[0].Queue_GID.ToString();
                    ViewBag.ApproverAction = objobjMExpense[0].queueactionfor.ToString();
                }
                Session["Ecfdeclaratonnote"] = objModelQuery.GetDecnote(confirmsupplier.ToString(), "A").ToString();
                return PartialView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult HoldReleaseRemark(int id)
        {
            Session["holdrep"] = Convert.ToString(id);
            return PartialView();
        }
        [HttpGet]
        public PartialViewResult ViewDespatch(int id)
        {
            ECFDataModel typmemodel = new ECFDataModel();
            List<SelectCourier> Rolelist = new List<SelectCourier>();
            {
                Rolelist = objModel.SelectCourier().ToList();
            };

            List<SelectListItem> role = new List<SelectListItem>();
            foreach (var item in Rolelist)
            {
                //bool selected = false;
                //if (item.doctypecode == DocType)
                //{
                //    selected = true;
                //}
                role.Add(new SelectListItem
                {
                    Text = item.couriername.ToString(),
                    Value = item.couriername.ToString(),
                    //Selected = selected
                });
            }
            ViewBag.Courier = role;

            List<ECFDataModel> cen = new List<ECFDataModel>();
            cen = objModel.SelectDespatch(id).ToList();
            if (cen.Count > 0)
            {
                typmemodel.ECFNo = cen[0].ECFNo.ToString();
                typmemodel.ECFDate = cen[0].ECFDate.ToString();
                typmemodel.ECFRaiser = cen[0].ECFRaiser.ToString();
                typmemodel.SupplierorEmployeename = cen[0].SupplierorEmployeename.ToString();
                typmemodel.ClaimMonth = cen[0].ClaimMonth.ToString();
                typmemodel.ECFAmount = cen[0].ECFAmount.ToString();
                typmemodel.Despatchdate = cen[0].Despatchdate.ToString();
            }

            return PartialView(typmemodel);

        }
        [HttpPost]
        public ActionResult Index(string EcfDateFrom = null, string EcfDateTo = null, string DocType = null, string docsubtype = null, string Code = null, string Name = null, string ECFClaimMonth = null, string queryempsup = null, string ECFDespatchDateTo = null, string ecfnumber = null, string ecfamount = null, string Ecfstatus = null, string ECFMode = null, string command = null, string Suppliercode = null, string Suppliername = null)
        {
            string docsubtyname = string.Empty;
            ECFDataModel viewbags = new ECFDataModel();
            DataTable getdoctypename = new DataTable();
            DataTable getdocsubtype = new DataTable();
            if (Session["Employeecode"] != null)
            {
                Code = Session["Employeecode"].ToString();
                Name = Session["EmployeeName"].ToString();
            }
            list.Add("Employee");
            list.Add("Supplier");
            List<string> Rolelist1 = new List<string>();
            {
                Rolelist1 = list;
            };
            List<SelectListItem> role1 = new List<SelectListItem>();
            foreach (var item in Rolelist1)
            {
                Boolean selected = false;
                if (item == queryempsup)
                {
                    selected = true;
                    ViewBag.SelectEmpOrSup = queryempsup;
                }


                role1.Add(new SelectListItem
                {
                    Text = item.ToString(),
                    Selected = selected
                    //Value = item.ToString()
                });
            }
            ViewBag.SupplierorEmployeeData = role1;

            if (DocType != "")
            {
                getdoctypename = objModel.getdoctypecode(int.Parse(DocType));
                DocType = getdoctypename.Rows[0]["doctype_code"].ToString();
            }
            else
            {
                DocType = "";
            }
            if (docsubtype != "0" && docsubtype != null)
            {
                getdoctypename = objModel.getdocsubtypecode(int.Parse(docsubtype));
                docsubtype = getdoctypename.Rows[0]["docsubtype_name"].ToString();
                docsubtyname = docsubtype;
                List<SelectListItem> docsub = new List<SelectListItem>();
                foreach (var item in docsubtyname)
                {
                    Boolean selected = false;
                    if (docsubtype == docsubtyname)
                    {
                        selected = true;
                    }
                    docsub.Add(new SelectListItem
                    {
                        Text = docsubtyname,
                        Value = int.Parse("1").ToString(),
                        Selected = selected
                    });
                }
                ViewBag.Docsubtype1 = docsubtyname;
                ViewBag.Docsubtymename = docsub;
            }
            if (docsubtype == null || docsubtype == "0")
            {
                docsubtype = "";
                List<SelectListItem> docsub = new List<SelectListItem>();

                Boolean selected = false;
                docsub.Add(new SelectListItem
                {
                    Text = "-----Select-----",
                    Value = int.Parse("1").ToString(),
                    Selected = selected
                });

                ViewBag.Docsubtype1 = "Dummy";
                ViewBag.Docsubtymename = docsub;
            }
            if (ECFMode == "select" || ECFMode == "")
            {
                ECFMode = "";
                ViewBag.ecfmode = "";
            }

            if (ECFMode != "" && ECFMode != "select")
            {
                List<SelectListItem> ddd = new List<SelectListItem>();
                foreach (var item in ECFMode)
                {
                    Boolean selected = false;
                    if (ECFMode == "S")
                    {
                        selected = true;
                        ViewBag.ecfmode = "Self";
                    }
                    else if (ECFMode == "P")
                    {
                        selected = true;
                        ViewBag.ecfmode = "Proxy";
                    }
                    else if (ECFMode == "C")
                    {
                        selected = true;
                        ViewBag.ecfmode = "CentralTeam";
                    }
                    else if (ECFMode == "R")
                    {
                        selected = true;
                        ViewBag.ecfmode = "Retention";
                    }
                    ddd.Add(new SelectListItem
                    {
                        // Text = item.Docname.ToString(),
                        //Value = item.Docgid.ToString(),
                        Selected = selected
                    });
                }

            }

            List<SelectCourier> courier = new List<SelectCourier>();
            {
                courier = objModel.SelectCourier().ToList();
            };

            ECFDataModel ECFDoctype = new ECFDataModel();
            ECFDoctype.selectdoctypedata = new SelectList(objModel.SelectDocType().ToList(), "Docgid", "Docname");
            ViewBag.DocType = ECFDoctype;

            ECFDataModel ECFStatusviewbag = new ECFDataModel();
            ECFStatusviewbag.statusnameData = new SelectList(objModel.SelectStatus().ToList(), "StatusGid", "statusname");
            ViewBag.ECFStatus = ECFStatusviewbag;

            ViewBag.EcfDateFrom = EcfDateFrom;
            ViewBag.EcfDateTo = EcfDateTo;
            ViewBag.Code = Code;
            ViewBag.Name = Name;
            ViewBag.ECFClaimMonth = ECFClaimMonth;
            ViewBag.ECFDespatchDateFrom = queryempsup;
            ViewBag.ECFDespatchDateTo = ECFDespatchDateTo;
            ViewBag.ecfnumber = ecfnumber;
            ViewBag.ecfamount = ecfamount;
            ViewBag.satval = Ecfstatus;
            ViewBag.docvalue = DocType;
            ViewBag.suppliercode = Suppliercode;
            ViewBag.suppliername = Suppliername;

            if (queryempsup == null)
            {
                queryempsup = "";
            }
            if (queryempsup == "Employee")
            {
                queryempsup = "E";

            }
            else if (queryempsup == "Supplier")
            {
                queryempsup = "S";

            }
            viewbags.statusname = DocType;
            viewbags.ECFStatus = Ecfstatus;
            viewbags.Code = Code;
            viewbags.Name = Name;
            viewbags.ECFNo = ecfnumber;
            viewbags.ECFAmount = ecfamount;
            viewbags.ClaimMonth = ECFClaimMonth;
            viewbags.ecfdatefrom = EcfDateFrom;
            viewbags.ecfdateto = EcfDateTo;
            viewbags.ecfFor = ViewBag.ecfmode;
            viewbags.SupplierorEmployee = ViewBag.SelectEmpOrSup;
            viewbags.DocSubTypeName = docsubtyname;
            viewbags.SupplierCode = Suppliercode;
            viewbags.SupplierName = Suppliername;
            Session["SearchData"] = viewbags;
            List<ECFDataModel> cen = new List<ECFDataModel>();
            cen = objModel.Search(EcfDateFrom, EcfDateTo, DocType, docsubtype, Code, Name, ECFClaimMonth, queryempsup, ECFDespatchDateTo, ecfnumber, ecfamount, Ecfstatus, ECFMode,command, Suppliercode, Suppliername).ToList();
            Session["Records"] = cen;
            if (command == "Clear")
            {
                Session["SearchData"] = "";
            }
            if (cen.Count == 0)
            {
                ViewBag.Message = "No Record's Found";
            }

            return View(cen);
        }
        [HttpGet]
        public PartialViewResult SupplierSearch(string listfor)
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

        [HttpPost]
        public JsonResult SupplierSearchECF(string SupplierName = "", string SupplierCode = "", string PanNo = "")
        {
            List<CentraldataModel> obj = new List<CentraldataModel>();
            CentraldataModel supplier = new CentraldataModel();
            if (SupplierCode != "" || SupplierName != "" || PanNo != "")
            {
                obj = cen.SelectSupplierSearch(SupplierName, SupplierCode, PanNo).ToList();
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
                obj = cen.SelectSupplier().ToList();

            }
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult EmployeeSearch(string listfor)
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
        public PartialViewResult ClearSearchEmployeeData()
        {
            List<CentraldataModel> obj = new List<CentraldataModel>();
            Session["SearchEmployeedata"] = null;
            obj = cen.SelectEmployee().ToList();
            return PartialView(obj);
        }
        [HttpPost]
        public JsonResult EmployeeSearchdetsa(string RaiserName = "", string RaiserCode = "")
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


        public PartialViewResult View(int id)
        {
            List<ECFDataModel> ecf = new List<ECFDataModel>();
            ecf = objModel.View(id).ToList();
            return PartialView(ecf);
        }
        [HttpGet]
        public ActionResult Clear()
        {
            Session["Employeecode"] = null;
            Session["EmployeeName"] = null;
            Session["Records"] = null;
            Session["SearchData"] = null;
            Session["SearchECfDespatchData"] = null;
            Session["SearcECFDeletionRecord"] = null;
            Session["SearcECFCancellationRecord"] = null;
            return RedirectToAction("Index", "ECF");
        }
        public JsonResult GetEmployee(ECFDataModel ecf)
        {
            Session["Employeecode"] = ecf.Code;
            Session["EmployeeName"] = ecf.Name;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult ECFCancellation()
        {
            //ViewBag.ecfmode = "";
            //ViewBag.Docsubtype1 = "";
            //list.Add("Employee");
            //list.Add("Supplier");
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
            //    });
            //}
            //ViewBag.SupplierorEmployeeData = role1;

            //List<ECFDataModel> cen = new List<ECFDataModel>();
            //ECFDataModel ECFDoctype = new ECFDataModel();
            //ECFDoctype.selectdoctypedata = new SelectList(objModel.SelectDocType().ToList(), "Docgid", "Docname");
            //ViewBag.DocType = ECFDoctype;

            //List<ECFDataModel> Rolelist = new List<ECFDataModel>();
            //{
            //    Rolelist = objModel.SelectDocType().ToList();
            //};
            //List<SelectListItem> role = new List<SelectListItem>();
            //foreach (var item in Rolelist)
            //{

            //    role.Add(new SelectListItem
            //    {
            //        Text = item.Docname.ToString(),
            //        Value = item.Docgid.ToString(),
            //    });
            //}
            //ViewBag.DocType111 = role;
            //if (Session["RecordsCancellation"] != null)
            //{
            //    ECFDataModel ViewBagEcfCancellation = new ECFDataModel();
            //    ViewBagEcfCancellation = (ECFDataModel)Session["SearcECFCancellationRecord"];
            //    ViewBag.ecfamount = ViewBagEcfCancellation.ECFAmount;
            //    ViewBag.docvalue = ViewBagEcfCancellation.statusname;
            //    ViewBag.ECFClaimMonth = ViewBagEcfCancellation.ClaimMonth;
            //    ViewBag.EcfDateFrom = ViewBagEcfCancellation.ecfdatefrom;
            //    ViewBag.EcfDateTo = ViewBagEcfCancellation.ecfdateto;
            //    ViewBag.Code = ViewBagEcfCancellation.Code;
            //    ViewBag.Name = ViewBagEcfCancellation.Name;
            //    ViewBag.SupplierorEmployee = ViewBagEcfCancellation.SupplierorEmployee;
            //    ViewBag.ecfmode = ViewBagEcfCancellation.ecfFor;
            //    ViewBag.Docsubtymename = ViewBagEcfCancellation.DocSubTypeName;
            //    cen = (List<ECFDataModel>)Session["RecordsCancellation"];
            //    cen = objModel.CancellationSearch().ToList();
            //    if (cen.Count == 0)
            //    {
            //        ViewBag.Message = "No Record";
            //    }
            //}
            //else
            //{
            //    cen = objModel.CancellationSearch().ToList();
            //    if (cen.Count == 0)
            //    {
            //        ViewBag.Message = "No Record";
            //    }
            //}

            //------------------------------------------------------------------------------------
            return View();
        }
        [HttpPost]
        public JsonResult GetValue(ECFDataModel delnote)
        {
            ECFDataModel TypeModel = new ECFDataModel();
            TypeModel.SelectDocSubType = new SelectList(objModel.SelectDocSubType(delnote.Docgid), "DocSubname", "DocSubgid");
            ViewBag.Docsubtype1 = "";
            return Json(objModel.SelectDocSubType(delnote.Docgid), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ECFCancellation(string EcfDateFrom = null, string EcfDateTo = null, string DocType = null, string DocSubType = null, string Code = null, string SupplierorEmployee = null, string Name = null, string ECFClaimMonth = null, string ECFDespatchDateFrom = null, string ECFDespatchDateTo = null, string Suppliername = null, string ECFMode = null, string ECFStatus = null, string ecfamount = null, string command = null)
        {
            string docsubtyname = string.Empty;
            ECFDataModel ViewBagEcfCancellation = new ECFDataModel();
            list.Add("Employee");
            list.Add("Supplier");
            List<string> Rolelist1 = new List<string>();
            {
                Rolelist1 = list;
            };
            List<SelectListItem> role1 = new List<SelectListItem>();
            foreach (var item in Rolelist1)
            {
                Boolean selected = false;
                if (item == SupplierorEmployee)
                {
                    selected = true;
                    ViewBag.SelectEmpOrSup = SupplierorEmployee;
                }
                role1.Add(new SelectListItem
                {
                    Text = item.ToString(),
                    Selected = selected


                });
            }
            ViewBag.SupplierorEmployeeData = role1;

            DataTable getdoctypename = new DataTable();
            DataTable getdocsubtype1 = new DataTable();
            if (DocType != "")
            {
                getdoctypename = objModel.getdoctypecode(int.Parse(DocType));
                DocType = getdoctypename.Rows[0]["doctype_code"].ToString();
            }
            else
            {
                DocType = "";
            }
            if (DocSubType == "0" || DocSubType == null)
            {
                DocSubType = "";

                List<SelectListItem> docsub = new List<SelectListItem>();

                Boolean selected = false;
                docsub.Add(new SelectListItem
                {
                    Text = "-----Select-----",
                    Value = int.Parse("1").ToString(),
                    Selected = selected
                });

                ViewBag.Docsubtype1 = "Dummy";
                ViewBag.Docsubtymename = docsub;

            }
            else
            {
                getdocsubtype1 = objModel.getdocsubtypecode(int.Parse(DocSubType));
                DocSubType = getdocsubtype1.Rows[0]["docsubtype_name"].ToString();
                docsubtyname = DocSubType;
                List<SelectListItem> docsub = new List<SelectListItem>();
                foreach (var item in docsubtyname)
                {
                    Boolean selected = false;
                    if (DocSubType == docsubtyname)
                    {
                        selected = true;
                    }
                    docsub.Add(new SelectListItem
                    {
                        Text = docsubtyname,
                        Value = int.Parse("1").ToString(),
                        Selected = selected
                    });
                }
                ViewBag.Docsubtype1 = docsubtyname;
                ViewBag.Docsubtymename = docsub;
            }
            if (Session["Employeecode"] != null)
            {
                Code = Session["Employeecode"].ToString();
                Name = Session["EmployeeName"].ToString();
            }
            if (ECFMode == "select" || ECFMode == "")
            {
                ECFMode = "";
                ViewBag.ecfmode = "";
            }
            if (ECFMode != "" && ECFMode != "select")
            {
                List<SelectListItem> ddd = new List<SelectListItem>();
                foreach (var item in ECFMode)
                {
                    Boolean selected = false;
                    if (ECFMode == "S")
                    {
                        selected = true;
                        ViewBag.ecfmode = "Self";
                    }
                    else if (ECFMode == "P")
                    {
                        selected = true;
                        ViewBag.ecfmode = "Proxy";
                    }
                    else if (ECFMode == "C")
                    {
                        selected = true;
                        ViewBag.ecfmode = "CentralTeam";
                    }
                    else if (ECFMode == "R")
                    {
                        selected = true;
                        ViewBag.ecfmode = "Retention";
                    }
                    ddd.Add(new SelectListItem
                    {
                        // Text = item.Docname.ToString(),
                        //Value = item.Docgid.ToString(),
                        Selected = selected
                    });
                }
            }
            List<ECFDataModel> cen = new List<ECFDataModel>();
            ECFDataModel ECFDoctype = new ECFDataModel();
            ECFDoctype.selectdoctypedata = new SelectList(objModel.SelectDocType().ToList(), "Docgid", "Docname");
            ViewBag.DocType = ECFDoctype;

            ViewBag.EcfDateFrom = EcfDateFrom;
            ViewBag.EcfDateTo = EcfDateTo;
            ViewBag.Code = Code;
            ViewBag.Name = Name;
            ViewBag.ECFClaimMonth = ECFClaimMonth;
            ViewBag.ECFDespatchDateFrom = ECFDespatchDateFrom;
            ViewBag.ECFDespatchDateTo = ECFDespatchDateTo;
            ViewBag.ecfamount = ecfamount;
            ViewBag.ECFAWBNo = ECFMode;
            ViewBag.docvalue = DocType;
            if (SupplierorEmployee == null)
            {
                SupplierorEmployee = "";
            }
            if (SupplierorEmployee == "Employee")
            {
                SupplierorEmployee = "E";

            }
            else if (SupplierorEmployee == "Supplier")
            {
                SupplierorEmployee = "S";
            }
            ViewBagEcfCancellation.statusname = DocType;
            ViewBagEcfCancellation.Code = Code;
            ViewBagEcfCancellation.Name = Name;
            ViewBagEcfCancellation.ECFAmount = ecfamount;
            ViewBagEcfCancellation.ClaimMonth = ECFClaimMonth;
            ViewBagEcfCancellation.ecfdatefrom = EcfDateFrom;
            ViewBagEcfCancellation.ecfdateto = EcfDateTo;
            ViewBagEcfCancellation.ecfFor = ViewBag.ecfmode;
            ViewBagEcfCancellation.SupplierorEmployee = ViewBag.SelectEmpOrSup;
            ViewBagEcfCancellation.DocSubTypeName = docsubtyname;
            Session["SearcECFCancellationRecord"] = ViewBagEcfCancellation;
            if (command == "Clear")
            {
                Session["SearcECFCancellationRecord"] = null;
            }
            else
            {
                cen = objModel.CancellationSearch(EcfDateFrom, EcfDateTo, DocType, DocSubType, Code, Name, ECFClaimMonth, SupplierorEmployee, ECFDespatchDateTo, Suppliername, ECFMode, ECFStatus, ecfamount).ToList();
                Session["RecordsCancellation"] = cen;
                if (cen.Count == 0)
                {
                    ViewBag.Message = "No Record Found";
                }
            }

            return View(cen);
        }
        public PartialViewResult CancellationView(int id)
        {
            Session["ecfid"] = id;
            List<ECFDataModel> ecf = new List<ECFDataModel>();
            ecf = objModel.View(id).ToList();
            return PartialView(ecf);
        }
        public ActionResult CancellationClear()
        {
            Session["Employeecode"] = null;
            Session["EmployeeName"] = null;
            Session["RecordsCancellation"] = null;
            Session["ecfid"] = null;
            Session["SearchData"] = null;
            Session["SearchECfDespatchData"] = null;
            Session["SearcECFDeletionRecord"] = null;
            Session["SearcECFCancellationRecord"] = null;
            return RedirectToAction("ECFCancellation", "ECF");
        }
        public ActionResult DespatchClear()
        {
            Session["Employeecode"] = null;
            Session["EmployeeName"] = null;
            Session["RecordsDespatch"] = null;
            Session["ecfid"] = null;
            Session["SearchData"] = null;
            Session["SearchECfDespatchData"] = null;
            Session["SearcECFDeletionRecord"] = null;
            Session["SearcECFCancellationRecord"] = null;
            return RedirectToAction("DespatchIndex", "ECF");
        }
        public JsonResult UpdateCancellation()
        {
            result = objModel.UpdateCancellation(Convert.ToInt32(Session["ecfid"]));
            if (result == "UpdatedCancel")
            {
                result = "Successfully Cancelled";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ECFDeletion()
        {
            //ViewBag.ecfmode = "";
            //ViewBag.Docsubtype1 = "";
            //ECFDataModel ECFDoctype = new ECFDataModel();
            //ECFDoctype.selectdoctypedata = new SelectList(objModel.SelectDocType().ToList(), "Docgid", "Docname");
            //ViewBag.DocType = ECFDoctype;
            //List<ECFDataModel> cen = new List<ECFDataModel>();

            //List<ECFDataModel> Rolelist = new List<ECFDataModel>();
            //{
            //    Rolelist = objModel.SelectDocType().ToList();
            //};
            //List<SelectListItem> role = new List<SelectListItem>();
            //foreach (var item in Rolelist)
            //{

            //    role.Add(new SelectListItem
            //    {
            //        Text = item.Docname.ToString(),
            //        Value = item.Docgid.ToString(),
            //    });
            //}
            //ViewBag.DocType111 = role;
            //List<ECFDataModel> Rolelist = new List<ECFDataModel>();
            //{
            //    Rolelist = objModel.SelectDocType().ToList();
            //};
            //List<SelectListItem> role = new List<SelectListItem>();
            //foreach (var item in Rolelist)
            //{

            //    role.Add(new SelectListItem
            //    {
            //        Text = item.Docname.ToString(),
            //        Value = item.Docgid.ToString()
            //    });
            //}
            //ViewBag.DocType = role;

            //list.Add("Employee");
            //list.Add("Supplier");
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
            //ViewBag.SupplierorEmployeedata = role1;
            //2
            //List<ECFDataModel> Rolelist1 = new List<ECFDataModel>();
            //{
            //    Rolelist1 = objModel.SelectDocSubType().ToList();
            //};
            //List<SelectListItem> role1 = new List<SelectListItem>();
            //foreach (var item in Rolelist1)
            //{
            //    role1.Add(new SelectListItem
            //    {
            //        Text = item.DocSubname.ToString(),
            //        Value = item.DocSubgid.ToString()
            //    });
            //}
            //ViewBag.DocSubType = role1;
            //4
            //List<ECFDataModel> Rolelist2 = new List<ECFDataModel>();
            //{
            //    Rolelist2 = objModel.SelectStatus().ToList();
            //};
            //List<SelectListItem> role2 = new List<SelectListItem>();
            //foreach (var item in Rolelist2)
            //{
            //    role2.Add(new SelectListItem
            //    {
            //        Text = item.statusname.ToString(),
            //        Value = item.StatusGid.ToString()
            //    });
            //}
            //ViewBag.ECFStatus = role2;
            //if (Session["RecordsDeletion"] != null)
            //{
            //    ECFDataModel ViewBagEcfdeletion = new ECFDataModel();
            //    ViewBagEcfdeletion = (ECFDataModel)Session["SearcECFDeletionRecord"];
            //    ViewBag.EcfDateFrom = ViewBagEcfdeletion.ecfdatefrom;
            //    ViewBag.docvalue = ViewBagEcfdeletion.statusname;
            //    ViewBag.EcfDateTo = ViewBagEcfdeletion.ecfdatefrom;
            //    ViewBag.Code = ViewBagEcfdeletion.Code;
            //    ViewBag.Name = ViewBagEcfdeletion.Name;
            //    ViewBag.ECFClaimMonth = ViewBagEcfdeletion.ClaimMonth;
            //    ViewBag.ecfnumber = ViewBagEcfdeletion.ECFNo;
            //    ViewBag.ecfamount = ViewBagEcfdeletion.ECFAmount;
            //    cen = (List<ECFDataModel>)Session["RecordsDeletion"];
            //    if (cen.Count == 0)
            //    {
            //        ViewBag.Message = "No Record";
            //    }
            //}
            //else
            //{
            //    cen = objModel.DeletionSearch().ToList();
            //    if (cen.Count == 0)
            //    {
            //        ViewBag.Message = "No Record";
            //    }
            //}
            return View();
        }
        [HttpPost]
        public ActionResult ECFDeletion(string EcfDateFrom = null, string EcfDateTo = null, string DocType = null, string DocSubType = null, string Code = null, string Name = null, string ECFClaimMonth = null, string SupplierorEmployee = null, string ECFDespatchDateTo = null, string Suppliername = null, string delecfnumber = null, string ecfamount = null, string ECFAWBNo = null, string ECFStatus = null, string ECFMode = null, string command = null)
        {
            ECFDataModel ViewBagEcfdeletion = new ECFDataModel();
            DataTable getdoctypename = new DataTable();
            DataTable getdocsubtype = new DataTable();
            string docsubtyname = string.Empty;
            if (DocType != "")
            {
                getdoctypename = objModel.getdoctypecode(int.Parse(DocType));
                DocType = getdoctypename.Rows[0]["doctype_code"].ToString();
            }
            else
            {
                DocType = "";
            }
            if (DocSubType == "0" || DocSubType == null)
            {
                DocSubType = "";
                List<SelectListItem> docsub = new List<SelectListItem>();

                Boolean selected = false;
                docsub.Add(new SelectListItem
                {
                    Text = "-----Select-----",
                    Value = int.Parse("1").ToString(),
                    Selected = selected
                });

                ViewBag.Docsubtype1 = "Dummy";
                ViewBag.Docsubtymename = docsub;
            }
            else
            {
                getdocsubtype = objModel.getdocsubtypecode(int.Parse(DocSubType));
                DocSubType = getdocsubtype.Rows[0]["docsubtype_name"].ToString();

                docsubtyname = DocSubType;
                List<SelectListItem> docsub = new List<SelectListItem>();
                foreach (var item in docsubtyname)
                {
                    Boolean selected = false;
                    if (DocSubType == docsubtyname)
                    {
                        selected = true;
                    }
                    docsub.Add(new SelectListItem
                    {
                        Text = docsubtyname,
                        Value = int.Parse("1").ToString(),
                        Selected = selected
                    });
                }
                ViewBag.Docsubtype1 = docsubtyname;
                ViewBag.Docsubtymename = docsub;
            }
            if (ECFMode == "select" || ECFMode == "")
            {
                ECFMode = "";
                ViewBag.ecfmode = "";
            }
            if (ECFMode != "" && ECFMode != "select")
            {
                List<SelectListItem> ddd = new List<SelectListItem>();
                foreach (var item in ECFMode)
                {
                    Boolean selected = false;
                    if (ECFMode == "S")
                    {
                        selected = true;
                        ViewBag.ecfmode = "Self";
                    }
                    else if (ECFMode == "P")
                    {
                        selected = true;
                        ViewBag.ecfmode = "Proxy";
                    }
                    else if (ECFMode == "C")
                    {
                        selected = true;
                        ViewBag.ecfmode = "CentralTeam";
                    }
                    else if (ECFMode == "R")
                    {
                        selected = true;
                        ViewBag.ecfmode = "Retention";
                    }
                    ddd.Add(new SelectListItem
                    {
                        // Text = item.Docname.ToString(),
                        //Value = item.Docgid.ToString(),
                        Selected = selected
                    });
                }

            }
            List<ECFDataModel> cen = new List<ECFDataModel>();
            //List<ECFDataModel> Rolelist = new List<ECFDataModel>();
            //{
            //    Rolelist = objModel.SelectDocType().ToList();
            //};
            //List<SelectListItem> role = new List<SelectListItem>();
            //foreach (var item in Rolelist)
            //{
            //    Boolean selected = false;
            //    if (item.Docname.ToString() == DocType.ToString())
            //    {
            //        selected = true;
            //    }
            //    role.Add(new SelectListItem
            //    {
            //        Text = item.Docname.ToString(),
            //        Value = item.Docgid.ToString(),
            //        Selected = selected
            //    });
            //}
            //ViewBag.DocType = role;
            ECFDataModel ECFDoctype = new ECFDataModel();
            ECFDoctype.selectdoctypedata = new SelectList(objModel.SelectDocType().ToList(), "Docgid", "Docname");
            ViewBag.DocType = ECFDoctype;

            list.Add("Employee");
            list.Add("Supplier");
            List<string> Ro = new List<string>();
            {
                Ro = list;
            };
            List<SelectListItem> ro = new List<SelectListItem>();
            foreach (var item in Ro)
            {
                bool selected = false;
                if (item == SupplierorEmployee)
                {
                    selected = true;
                }
                ro.Add(new SelectListItem
                {
                    Text = item.ToString(),
                    Selected = selected
                    //Value = item.ToString()
                });
            }
            ViewBag.SupplierorEmployeedata = ro;

            //2
            //List<ECFDataModel> Rolelist1 = new List<ECFDataModel>();
            //{
            //    Rolelist1 = objModel.SelectDocSubType().ToList();
            //};
            //List<SelectListItem> role1 = new List<SelectListItem>();
            //foreach (var item in Rolelist1)
            //{
            //    Boolean selected = false;
            //    if (item.DocSubgid.ToString() == DocSubType.ToString())
            //    {
            //        selected = true;
            //    }
            //    role1.Add(new SelectListItem
            //    {
            //        Text = item.DocSubname.ToString(),
            //        Value = item.DocSubgid.ToString(),
            //        Selected=selected
            //    });
            //}
            //ViewBag.DocSubType = role1;
            //4
            List<ECFDataModel> Rolelist2 = new List<ECFDataModel>();
            {
                Rolelist2 = objModel.SelectStatus().ToList();
            };
            //List<SelectListItem> role2 = new List<SelectListItem>();
            //foreach (var item in Rolelist2)
            //{
            //    Boolean selected = false;
            //    if (item.StatusGid.ToString() == ECFStatus.ToString())
            //    {
            //        selected = true;
            //    }
            //    role2.Add(new SelectListItem
            //    {
            //        Text = item.statusname.ToString(),
            //        Value = item.StatusGid.ToString(),
            //        Selected=selected
            //    });
            //}
            //ViewBag.ECFStatus = role2;
            if (SupplierorEmployee == null)
            {
                SupplierorEmployee = "";
            }
            if (SupplierorEmployee == "Employee")
            {
                SupplierorEmployee = "E";

            }
            else if (SupplierorEmployee == "Supplier")
            {
                SupplierorEmployee = "S";

            }

            ViewBag.EcfDateFrom = EcfDateFrom;
            ViewBag.EcfDateTo = EcfDateTo;
            ViewBag.Code = Code;
            ViewBag.Name = Name;
            ViewBag.ECFClaimMonth = ECFClaimMonth;
            ViewBag.ECFDespatchDateFrom = SupplierorEmployee;
            ViewBag.ECFDespatchDateTo = ECFDespatchDateTo;
            ViewBag.ecfnumber = delecfnumber;
            ViewBag.ecfamount = ecfamount;
            ViewBag.ECFAWBNo = ECFAWBNo;
            ViewBag.docvalue = DocType;

            ViewBagEcfdeletion.statusname = DocType;
            ViewBagEcfdeletion.Code = Code;
            ViewBagEcfdeletion.Name = Name;
            ViewBagEcfdeletion.ECFNo = delecfnumber;
            ViewBagEcfdeletion.ECFAmount = ecfamount;
            ViewBagEcfdeletion.ClaimMonth = ECFClaimMonth;
            ViewBagEcfdeletion.ecfdatefrom = EcfDateFrom;
            ViewBagEcfdeletion.ecfdateto = EcfDateTo;
            ViewBagEcfdeletion.ecfFor = ViewBag.ecfmode;
            ViewBagEcfdeletion.SupplierorEmployee = ViewBag.SelectEmpOrSup;
            ViewBagEcfdeletion.DocSubTypeName = docsubtyname;

            Session["SearcECFDeletionRecord"] = ViewBagEcfdeletion;

            if (command == "Clear")
            {
                Session["SearcECFDeletionRecord"] = null;
            }
            else
            {
                cen = objModel.DeletionSearch(EcfDateFrom, EcfDateTo, DocType, DocSubType, Code, Name, ECFClaimMonth, SupplierorEmployee, ECFDespatchDateTo, Suppliername, delecfnumber, ECFMode, ECFStatus, ecfamount).ToList();
                Session["RecordsDeletion"] = cen;
                if (cen.Count == 0)
                {
                    ViewBag.Message = "No Record Found";
                }
            }

            return View(cen);
        }
        public PartialViewResult DeletionView(int id)
        {
            Session["ecfid"] = id;
            List<ECFDataModel> ecf = new List<ECFDataModel>();
            ecf = objModel.Viewwithqueue(id).ToList();
            return PartialView(ecf);
        }
        public ActionResult DeletionClear()
        {
            Session["Employeecode"] = null;
            Session["EmployeeName"] = null;
            Session["RecordsDeletion"] = null;
            Session["ecfid"] = null;
            Session["SearchData"] = null;
            Session["SearchECfDespatchData"] = null;
            Session["SearcECFDeletionRecord"] = null;
            Session["SearcECFCancellationRecord"] = null;
            return RedirectToAction("ECFDeletion", "ECF");
        }
        public JsonResult Delete(ECFDataModel obj)
        {
            result = objModel.Delete(obj, Convert.ToInt32(Session["ecfid"]));
            if (result == "Deleted")
            {
                result = "Successfully Deleted";
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult DespatchIndex()
        {
            //ViewBag.ecfmode = "";
            //ViewBag.Docsubtype1 = "";
            //List<ECFDataModel> cen = new List<ECFDataModel>();

            //ECFDataModel ECFDoctype = new ECFDataModel();
            //ECFDoctype.selectdoctypedata = new SelectList(objModel.SelectDocType().ToList(), "Docgid", "Docname");
            //ViewBag.DocType = ECFDoctype;

            //List<ECFDataModel> Rolelist = new List<ECFDataModel>();
            //{
            //    Rolelist = objModel.SelectDocType().ToList();
            //};
            //List<SelectListItem> role = new List<SelectListItem>();
            //foreach (var item in Rolelist)
            //{

            //    role.Add(new SelectListItem
            //    {
            //        Text = item.Docname.ToString(),
            //        Value = item.Docgid.ToString(),
            //    });
            //}
            //ViewBag.DocType111 = role;
            //list.Add("Employee");
            //list.Add("Supplier");
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

            //    });
            //}
            //ViewBag.SupplierorEmployeeData1 = role1;

            //if (Session["RecordsDespatch"] != null)
            //{
            //    ECFDataModel EcfdespatchRecord = new ECFDataModel();
            //    EcfdespatchRecord = (ECFDataModel)Session["SearchECfDespatchData"];
            //    ViewBag.EcfDateFrom = EcfdespatchRecord.ecfdatefrom;
            //    ViewBag.EcfDateTo = EcfdespatchRecord.ecfdateto;
            //    ViewBag.Code = EcfdespatchRecord.Code;
            //    ViewBag.Name = EcfdespatchRecord.Name;
            //    ViewBag.ECFClaimMonth = EcfdespatchRecord.ClaimMonth;
            //    ViewBag.desecfnumber = EcfdespatchRecord.ECFNo;
            //    ViewBag.desecfamount = EcfdespatchRecord.ECFAmount;
            //    ViewBag.SelectEmpOrSup = EcfdespatchRecord.SupplierorEmployee;
            //    ViewBag.docvalue = EcfdespatchRecord.statusname;
            //    cen = (List<ECFDataModel>)Session["RecordsDespatch"];
            //    if (cen.Count == 0)
            //    {
            //        ViewBag.Message = "No Record";
            //    }
            //}
            //else
            //{
            //    cen = objModel.DespatchSearch().ToList();
            //    if (cen.Count == 0)
            //    {
            //        ViewBag.Message = "No Record";
            //    }
            //}


            return View();
        }
        [HttpPost]
        public ActionResult DespatchIndex(string EcfDateFrom = null, string EcfDateTo = null, string DocType = null, string DocSubType = null, string Code = null, string Name = null, string ECFClaimMonth = null, string despatchsuporemp = null, string ECFDespatchDateTo = null, string Suppliername = null, string desecfnumber = null, string desecfamount = null, string ECFStatus = null, string ECFMode = null, string command = null)
        {
            DataTable getdoctypename1 = new DataTable();
            DataTable getdoctypename = new DataTable();
            ECFDataModel EcfdespatchRecord = new ECFDataModel();
            string docsubtyname = string.Empty;
            if (DocType != "")
            {
                getdoctypename1 = objModel.getdoctypecode(int.Parse(DocType));
                DocType = getdoctypename1.Rows[0]["doctype_code"].ToString();
            }
            else
            {
                DocType = "";
            }
            if (DocSubType != "0" && DocSubType != null)
            {
                getdoctypename = objModel.getdocsubtypecode(int.Parse(DocSubType));
                DocSubType = getdoctypename.Rows[0]["docsubtype_name"].ToString();

                docsubtyname = DocSubType;
                List<SelectListItem> docsub = new List<SelectListItem>();
                foreach (var item in docsubtyname)
                {
                    Boolean selected = false;
                    if (DocSubType == docsubtyname)
                    {
                        selected = true;
                    }
                    docsub.Add(new SelectListItem
                    {
                        Text = docsubtyname,
                        Value = int.Parse("1").ToString(),
                        Selected = selected
                    });
                }
                ViewBag.Docsubtype1 = docsubtyname;
                ViewBag.Docsubtymename = docsub;
            }
            else if (DocSubType == null || DocSubType == "0")
            {
                DocSubType = "";
                List<SelectListItem> docsub = new List<SelectListItem>();

                Boolean selected = false;
                docsub.Add(new SelectListItem
                {
                    Text = "-----Select-----",
                    Value = int.Parse("1").ToString(),
                    Selected = selected
                });

                ViewBag.Docsubtype1 = "Dummy";
                ViewBag.Docsubtymename = docsub;
            }
            if (Session["Employeecode"] != null)
            {
                Code = Session["Employeecode"].ToString();
                Name = Session["EmployeeName"].ToString();
            }
            //List<ECFDataModel> Rolelist = new List<ECFDataModel>();
            //{
            //    Rolelist = objModel.SelectDocType().ToList();
            //};
            //List<SelectListItem> role = new List<SelectListItem>();
            //foreach (var item in Rolelist)
            //{
            //    Boolean selected = false;
            //    if (item.Docname.ToString() == DocType.ToString())
            //    {
            //        selected = true;
            //    }
            //    role.Add(new SelectListItem
            //    {
            //        Text = item.Docname.ToString(),
            //        Value = item.Docname.ToString(),
            //        Selected = selected
            //    });
            //}

            ECFDataModel ECFDoctype = new ECFDataModel();
            ECFDoctype.selectdoctypedata = new SelectList(objModel.SelectDocType().ToList(), "Docgid", "Docname");
            ViewBag.DocType = ECFDoctype;
            if (ECFMode == "select" || ECFMode == "")
            {
                ECFMode = "";
                ViewBag.ecfmode = "";
            }
            if (ECFMode != "" && ECFMode != "select")
            {
                List<SelectListItem> ddd = new List<SelectListItem>();
                foreach (var item in ECFMode)
                {
                    Boolean selected = false;
                    if (ECFMode == "S")
                    {
                        selected = true;
                        ViewBag.ecfmode = "Self";
                    }
                    else if (ECFMode == "P")
                    {
                        selected = true;
                        ViewBag.ecfmode = "Proxy";
                    }
                    else if (ECFMode == "C")
                    {
                        selected = true;
                        ViewBag.ecfmode = "CentralTeam";
                    }
                    else if (ECFMode == "R")
                    {
                        selected = true;
                        ViewBag.ecfmode = "Retention";
                    }
                    ddd.Add(new SelectListItem
                    {
                        // Text = item.Docname.ToString(),
                        //Value = item.Docgid.ToString(),
                        Selected = selected
                    });
                }

            }
            //  ViewBag.DocType = role;
            list.Add("Employee");
            list.Add("Supplier");
            List<string> Rolelist1 = new List<string>();
            {
                Rolelist1 = list;
            };
            List<SelectListItem> role1 = new List<SelectListItem>();
            foreach (var item in Rolelist1)
            {
                bool selected = false;
                if (item == despatchsuporemp)
                {
                    selected = true;
                    ViewBag.SelectEmpOrSup = despatchsuporemp;
                }
                role1.Add(new SelectListItem
                {
                    Text = item.ToString(),
                    Selected = selected
                    //Value = item.ToString()
                });
            }
            ViewBag.SupplierorEmployeeData1 = role1;
            //2
            //List<ECFDataModel> Rolelist1 = new List<ECFDataModel>();
            //{
            //    Rolelist1 = objModel.SelectDocSubType().ToList();
            //};
            //List<SelectListItem> role1 = new List<SelectListItem>();
            //foreach (var item in Rolelist1)
            //{
            //    Boolean selected = false;
            //    if (item.DocSubgid.ToString() == DocSubType.ToString())
            //    {
            //        selected = true;
            //    }
            //    role1.Add(new SelectListItem
            //    {
            //        Text = item.DocSubname.ToString(),
            //        Value = item.DocSubgid.ToString(),
            //        Selected = selected
            //    });
            //}
            //ViewBag.DocSubType = role1;

            //4
            List<ECFDataModel> Rolelist2 = new List<ECFDataModel>();
            {
                Rolelist2 = objModel.SelectStatus().ToList();
            };


            if (despatchsuporemp == null)
            {
                despatchsuporemp = "";
            }
            if (despatchsuporemp == "Employee")
            {
                despatchsuporemp = "E";

            }
            else if (despatchsuporemp == "Supplier")
            {
                despatchsuporemp = "S";

            }

            ViewBag.EcfDateFrom = EcfDateFrom;
            ViewBag.EcfDateTo = EcfDateTo;
            ViewBag.Code = Code;
            ViewBag.Name = Name;
            ViewBag.ECFClaimMonth = ECFClaimMonth;
            ViewBag.ECFDespatchDateFrom = despatchsuporemp;
            ViewBag.ECFDespatchDateTo = ECFDespatchDateTo;
            ViewBag.desecfnumber = desecfnumber;
            ViewBag.desecfamount = desecfamount;
            ViewBag.docvalue = DocType;

            EcfdespatchRecord.statusname = DocType;
            EcfdespatchRecord.Code = Code;
            EcfdespatchRecord.Name = Name;
            EcfdespatchRecord.ECFNo = desecfnumber;
            EcfdespatchRecord.ECFAmount = desecfamount;
            EcfdespatchRecord.ClaimMonth = ECFClaimMonth;
            EcfdespatchRecord.ecfdatefrom = EcfDateFrom;
            EcfdespatchRecord.ecfdateto = EcfDateTo;
            EcfdespatchRecord.ecfFor = ViewBag.ecfmode;
            EcfdespatchRecord.SupplierorEmployee = ViewBag.SelectEmpOrSup;
            EcfdespatchRecord.DocSubTypeName = docsubtyname;
            Session["SearchECfDespatchData"] = EcfdespatchRecord;

            if (command == "Clear")
            {
                Session["SearchECfDespatchData"] = null;
            }
            List<ECFDataModel> cen = new List<ECFDataModel>();
            cen = objModel.DespatchSearch(EcfDateFrom, EcfDateTo, DocType, DocSubType, Code, Name, ECFClaimMonth, despatchsuporemp, ECFDespatchDateTo, Suppliername, desecfnumber, desecfamount, ECFStatus, ECFMode).ToList();
            Session["RecordsDespatch"] = cen;
            if (cen.Count == 0)
            {
                ViewBag.Message = "No Record's Found";
            }
            return View(cen);
        }
        [HttpGet]
        public PartialViewResult NewDespatch(string[] CheckListVal)
        {
            Session["ids"] = CheckListVal;
            ECFDataModel tymemodel = new ECFDataModel();
            List<SelectCourier> courier = new List<SelectCourier>();
            {
                courier = objModel.SelectCourier().ToList();
            };

            List<SelectListItem> roleforcourier = new List<SelectListItem>();
            foreach (var item in courier)
            {
                // bool selected = false;
                //if (item.SelectCourier == courier)
                //{
                //    selected = true;
                //}
                roleforcourier.Add(new SelectListItem
                {
                    Text = item.couriername.ToString(),
                    Value = item.couriername.ToString(),
                    // Selected = selected
                });
            }
            ViewBag.Courier = roleforcourier;
            return PartialView(tymemodel);
        }
        [HttpPost]
        public JsonResult NewDespatch(ECFDataModel ecf)
        {
            List<ECFDataModel> cen = new List<ECFDataModel>();
            cen = objModel.SelectDespatchByEcfNumber(ecf.ECFNo).ToList();
            if (cen.Count > 0)
            {
                //Session["ECFNo"] = cen[0].ECFNo.ToString();
                //Session["ECFDate"] = cen[0].ECFDate.ToString();
                //Session["Raiser"] = cen[0].ECFRaiser.ToString();
                //Session["Supplier"] = cen[0].SupplierorEmployeename.ToString();
                //Session["ClaimMonth"] = cen[0].ClaimMonth.ToString();
                //Session["EcfAmount"] = cen[0].ECFAmount.ToString();
                ecf.ECFAmount = cen[0].ECFAmount.ToString();
                ecf.ECFDate = cen[0].ECFDate.ToString();
                ecf.ECFRaiser = cen[0].ECFRaiser.ToString();
                ecf.SupplierorEmployeename = cen[0].SupplierorEmployeename.ToString();
                ecf.ClaimMonth = cen[0].ClaimMonth.ToString();
                ecf.ECFAmount = cen[0].ECFAmount.ToString();
                //ViewBag.EcfAmount = cen[0].ECFAmount.ToString();
            }
            //Session["ECFNo"] = "ECF123456";
            //Session["ECFDate"] = "17-02-2015";
            //Session["Raiser"] = "wwww";
            //Session["Supplier"] = "power";
            //Session["ClaimMonth"] = "16-07-2013";
            //Session["EcfAmount"] = "3000";
            //result = "set";
            return Json(ecf, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Check(ECFDataModel ecf)
        {
            result = objModel.IfCheck(ecf);

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DispatchUpdate(ECFDataModel ecf, string[] SelectedValues)
        {
            if (Session["ids"] != null)
            {
                SelectedValues = Session["ids"] as string[];
            }
            if (SelectedValues != null)
            {
                StringBuilder builder = new StringBuilder();
                foreach (string value in SelectedValues)
                {
                    if (value != "0")
                    {
                        builder.Append(value);
                    }
                }
                string arrayvalue = builder.ToString();
                splitvalues = Regex.Split(arrayvalue, ",");
            }
            DataTable getecfgid = new DataTable();

            if (ecf.CourierName == "-----Select-----")
            {
                ecf.CourierName = "";
            }

            //getecfgid = objModel.getecfnumbergid(ecf.ECFNo);
            //if (getecfgid.Rows.Count > 0)
            //{
            //ecf.ECFId = int.Parse(getecfgid.Rows[0]["ecf_gid"].ToString());
            result = objModel.DespatchUpdate(ecf, splitvalues);
            //}

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult clearsession()
        {
            Session["ECFNo"] = null;
            Session["ECFDate"] = null;
            Session["Raiser"] = null;
            Session["Supplier"] = null;
            Session["ClaimMonth"] = null;
            Session["EcfAmount"] = null;
            Session["RecordsDespatch"] = null;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult HoldReport()
        {
            //List<ECFDataModel> cen = new List<ECFDataModel>();
            //List<ECFDataModel> Rolelist = new List<ECFDataModel>();
            //{
            //    Rolelist = objModel.SelectDocType().ToList();
            //};
            //List<SelectListItem> role = new List<SelectListItem>();
            //foreach (var item in Rolelist)
            //{
            //    role.Add(new SelectListItem
            //    {
            //        Text = item.Docname.ToString(),
            //        Value = item.Docgid.ToString()
            //    });
            //}
            //ViewBag.DocType = role;
            //2
            //List<ECFDataModel> Rolelist1 = new List<ECFDataModel>();
            //{
            //    Rolelist1 = objModel.SelectDocSubType().ToList();
            //};
            //List<SelectListItem> role1 = new List<SelectListItem>();
            //foreach (var item in Rolelist1)
            //{
            //    role1.Add(new SelectListItem
            //    {
            //        Text = item.DocSubname.ToString(),
            //        Value = item.DocSubgid.ToString()
            //    });
            //}
            //ViewBag.DocSubType = role1;

            //if (Session["RecordsReport"] != null)
            //{
            //    cen = (List<ECFDataModel>)Session["RecordsReport"];
            //}
            //else
            //{

            //}
            //cen = objModel.HoldDetails().ToList();
            //if (cen.Count == 0)
            //{
            //    ViewBag.records = "No Records Found";
            //}
            return View();
        }
        [HttpPost]
        public ActionResult HoldReport(string EcfDateFrom = null, string DocType = null, string Code = null, string amount = null)
        {
            if (Session["Employeecode"] != null)
            {
                Code = Session["Employeecode"].ToString();
            }
            List<ECFDataModel> Rolelist = new List<ECFDataModel>();
            {
                Rolelist = objModel.SelectDocType().ToList();
            };
            List<SelectListItem> role = new List<SelectListItem>();
            foreach (var item in Rolelist)
            {
                Boolean selected = false;
                if (item.Docgid.ToString() == DocType.ToString())
                {
                    selected = true;
                }
                role.Add(new SelectListItem
                {
                    Text = item.Docname.ToString(),
                    Value = item.Docgid.ToString(),
                    Selected = selected
                });
            }
            ViewBag.DocType = role;
            ViewBag.EcfDateFrom = EcfDateFrom;
            ViewBag.Code = Code;
            ViewBag.amount = amount;
            List<ECFDataModel> cen = new List<ECFDataModel>();
            cen = objModel.HoldReport(EcfDateFrom, DocType, Code, amount).ToList();
            Session["RecordsReport"] = cen;
            if (cen.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            return View(cen);
        }
        [HttpGet]
        public ActionResult clearHoldReportsession()
        {
            Session["ECFNo"] = null;
            Session["ECFDate"] = null;
            Session["Raiser"] = null;
            Session["Supplier"] = null;
            Session["ClaimMonth"] = null;
            Session["EcfAmount"] = null;
            Session["RecordsReport"] = null;
            return RedirectToAction("HoldReport", "ECF");
        }
        public ActionResult HoldRelease(int id)
        {
            Session["Queueid"] = id;
            List<ECFDataModel> cen = new List<ECFDataModel>();
            cen = objModel.HoldDetails().ToList();
            return View(cen);
        }
        public JsonResult HoldReleaseReaport(int id, string remark)
        {
            Session["Queueid"] = id;
            result = objModel.Update(Convert.ToInt32(id), remark);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult getAutocompleteDepartment(string RaiserName)
        {
            List<CentraldataModel> obj = new List<CentraldataModel>();
            CentralModel v = new CentralModel();
            obj = v.SelectDepartment(RaiserName).ToList();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult getAutocomplete(string RaiserName)
        {
            List<CentraldataModel> obj = new List<CentraldataModel>();
            CentralModel v = new CentralModel();
            obj = v.SelectEmployeeSearch(RaiserName, "").ToList();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult getAutocompleteCode(string RaiserCode)
        {
            List<CentraldataModel> obj = new List<CentraldataModel>();
            CentralModel v = new CentralModel();
            obj = v.SelectEmployeeSearch("", RaiserCode).ToList();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getAutocompleteSupname(string Suppname)
        {
            List<CentraldataModel> obj = new List<CentraldataModel>();
            CentralModel v = new CentralModel();
            obj = v.SelectSupplierSearch(Suppname, "","").ToList();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult getAutocompleteSuppCode(string Suppcode)
        {
            List<CentraldataModel> obj = new List<CentraldataModel>();
            CentralModel v = new CentralModel();
            obj = v.SelectSupplierSearch("", Suppcode,"").ToList();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        public ActionResult downloadsqueryexcel()
        {
            DataTable dtnew = (DataTable)Session["QUERYSEARCH"];
            if (dtnew.Rows.Count > 0)
            {
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "QUERYSEARCH.xls"));
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
                    //GGG
                }
                Response.End();
            }
            return View();
        }
        [HttpGet]
        public JsonResult ToGetecfquery()
        {
            
            string data0 = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                //if (flag1 == 1)
                //{
                dt = objModel.SearchNew();
                //}
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                var jsonResult = Json(new { data0 }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = 2147483647;
                return jsonResult;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult ToGetecfquerythirumalai()
        {
            string ecfdate = "";
            string strquery = "";
            DataTable dt;
            string data0 = string.Empty;
            try
            {
                if (Session["ToGetecfquerylist"] == null)
                {
                    ecfdate = "20-04-2016";

                    dt = new DataTable();
                    dt = objModel.SearchNew();
                    Session["ToGetecfquerylist"] = dt;

                    if (ecfdate != null)
                    {
                        strquery = "ECFDate='" + ecfdate + "'";
                    }


                    DataView dv = dt.DefaultView;
                    dv.RowFilter = strquery;
                    dt = dv.ToTable();
                }
                else
                {
                    ecfdate = "20-04-2016";
                    string ecfdate1 = "30-04-2016";
                    dt = new DataTable();
                    dt = (DataTable)Session["ToGetecfquerylist"];


                    if (ecfdate != null)
                    {
                        strquery = "ECFDate >= '" + ecfdate + "' AND  ECFDate <= '" + ecfdate1 + "'";
                    }


                    DataView dv = dt.DefaultView;
                    dv.RowFilter = strquery;
                    dt = dv.ToTable();
                }

                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                var jsonResult = Json(new { data0 }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = 2147483647;
                return jsonResult;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult ToGetecfqueryecfinvoicetest()
        {
            string data0 = string.Empty;
            try
            {
                DataTable dt = new DataTable();
                dt = objModel.SearchNewecfinvoicetext();
                if (dt.Rows.Count > 0)
                {
                    data0 = JsonConvert.SerializeObject(dt);
                }
                var jsonResult = Json(new { data0 }, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = 2147483647;
                return jsonResult;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ToGetecfquerynew()
        {
            //ECFModel objModelnew = new ECFModel();
            ////List<ECFDataModel> lstDashBoard = new List<ECFDataModel>();
            //DataTable lstDashBoard = new DataTable();
            //lstDashBoard = objModelnew.Searchnew();
            //return Json(lstDashBoard, JsonRequestBehavior.AllowGet);

            DataTable dt = new DataTable();
            List<GetAll> list = new List<GetAll>();
            try
            {
                SqlConnection con = new SqlConnection("Data Source=WIN-M0GE46V6843;Initial Catalog=IEM8_MIGRATION;User Id=sa;password=gnsa;");
                con.Open();
                var query = "select * from iem_trn_tecf";
                SqlCommand com = new SqlCommand(query, con); //creating SqlCommand object  
                com.CommandType = CommandType.Text;
                com.ExecuteNonQuery();
                con.Close();
                SqlDataAdapter adptr = new SqlDataAdapter(com);
                adptr.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    GetAll GetAll = new GetAll();
                    GetAll.CustomerId = Convert.ToInt32(dt.Rows[i]["ecf_gid"]);
                    GetAll.Name = dt.Rows[i]["ecf_date"].ToString();
                    GetAll.Address = dt.Rows[i]["ecf_no"].ToString();
                    GetAll.Email = dt.Rows[i]["ecf_amount"].ToString();
                    GetAll.PhoneNumber = dt.Rows[i]["ecf_status"].ToString();
                    GetAll.Date = dt.Rows[i]["ecf_description"].ToString();
                    list.Add(GetAll);
                }
            }
            catch (Exception e)
            {
            }
            var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
            //return Json(list, JsonRequestBehavior.AllowGet);
        }
        public class GetAll
        {
            public int CustomerId { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Date { get; set; }
        }
        [HttpGet]
        public JsonResult ToGetecfdeletion()
        {
            List<ECFDataModel> lstdeletion = new List<ECFDataModel>();
            lstdeletion = objModel.DeletionSearch().ToList();
            var jsonResult = Json(new { qrylist = lstdeletion }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpGet]
        public JsonResult ToGetecfcancellation()
        {
            List<ECFDataModel> lstcancellation = new List<ECFDataModel>();
            lstcancellation = objModel.CancellationSearch().ToList();
            var jsonResult = Json(new { qrylist = lstcancellation }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpGet]
        public JsonResult ToGetecfhold()
        {
            List<ECFDataModel> lstDashBoard = new List<ECFDataModel>();
            //lstDashBoard = (List<ECFDataModel>)Session["ToGetMyRequesthold"];
            //if (lstDashBoard == null)
            //{
            lstDashBoard = objModel.HoldDetails().ToList();
            //Session["ToGetMyRequesthold"] = lstDashBoard;
            //}
            var jsonResult = Json(new { qrylist = lstDashBoard }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpGet]
        public JsonResult ToGetecftestIndex()
        {
            List<ECFDataModel> lstDashBoard = new List<ECFDataModel>();
            lstDashBoard = objModel.SearchTEST().ToList();
            return Json(lstDashBoard, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult testIndex()
        {
            return View();
        }
        [HttpGet]
        public ActionResult testIndex1()
        {
            return View();
        }
        public class CountryList
        {
            public int EmpId;
            public string name;
            public string Email;
            public CountryList(int _EmpId, string _name, string _Email)
            {
                EmpId = _EmpId;
                name = _name;
                Email = _Email;
            }
        }
        [HttpGet]
        public JsonResult GetCountyList()
        {
            List<CountryList> list = new List<CountryList>();
            for (int i = 0; i <= 20; i++)
            {
                list.Add(new CountryList(1, "India", "India"));
                list.Add(new CountryList(2, "USA", "India"));
                list.Add(new CountryList(3, "Canada", "India"));
                list.Add(new CountryList(4, "Italy", "India"));
                list.Add(new CountryList(1, "India", "India"));

                list.Add(new CountryList(2, "USA", "India"));
                list.Add(new CountryList(3, "Canada", "India"));
                list.Add(new CountryList(4, "Italy", "India"));
                list.Add(new CountryList(1, "India", "India"));
                list.Add(new CountryList(2, "USA", "India"));

                list.Add(new CountryList(3, "Canada", "India"));
                list.Add(new CountryList(4, "Italy", "India"));
                list.Add(new CountryList(1, "India", "India"));
                list.Add(new CountryList(2, "USA", "India"));
                list.Add(new CountryList(3, "Canada", "India"));

                list.Add(new CountryList(3, "Canada", "India"));
                list.Add(new CountryList(4, "Italy", "India"));
                list.Add(new CountryList(1, "India", "India"));
                list.Add(new CountryList(2, "USA", "India"));
                list.Add(new CountryList(3, "Canada", "India"));
            }
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult DespatchReport()
        {

            return PartialView();
        }
        public JsonResult DespatchReportAll()
        {
            ECFModel eowobjModel = new ECFModel();
            List<ECFDataModel> lstDashBoard = new List<ECFDataModel>();
            lstDashBoard = eowobjModel.DespatchReport().ToList();
            var jsonResult = Json(new { qrylist = lstDashBoard }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public JsonResult DespatchReportctAll(string ecffromdate, string ecfTodate, string ecfno, string printstatus, string searchtype)
        {
            ECFModel eowobjModel = new ECFModel();
            List<ECFDataModel> lstDashBoard = new List<ECFDataModel>();
            lstDashBoard = eowobjModel.ctdatdPrintdata(ecffromdate, ecfTodate, ecfno, printstatus, searchtype).ToList();
            var jsonResult = Json(new { qrylist = lstDashBoard }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult UserReport()
        {
            return View();
        }
        public JsonResult ReportAll()
        {
            ECFModel eowobjModel = new ECFModel();
            List<ECFDataModel> lstDashBoard = new List<ECFDataModel>();
            lstDashBoard = eowobjModel.ReportSearch().ToList();
            var jsonResult = Json(new { qrylist = lstDashBoard }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult ecfpowobasedreport()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ToGetecfpobasedquery()
        {
            List<ECFDataModel> lstDashBoard = new List<ECFDataModel>();
            lstDashBoard = objModel.Searchponumberbased().ToList();
            var jsonResult = Json(new { qrylist = lstDashBoard }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult despatchindexexportexcel()
        {
            DataTable dtnew = objModel.DespatchIndexExportExcel();
            if (dtnew.Rows.Count > 0)
            {
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "DESPATCH.xls"));
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
                    //GGG
                }
                Response.End();
            }
            return View();
        }
        public ActionResult despatchReportexportexcel()
        {
            DataTable dtnew = objModel.DespatchReportExportExcel();
            if (dtnew.Rows.Count > 0)
            {
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "DESPATCHREPORT.xls"));
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
                    //GGG
                }
                Response.End();
            }
            return View();
        }
        [HttpGet]
        public ActionResult myapprovedlist()
        {
            Session["Tomyapprovedlist"] = null;
            return View();
        }
        [HttpGet]
        public JsonResult Tomyapprovedlist()
        {
            List<ECFDataModel> lstDashBoard = new List<ECFDataModel>();
            lstDashBoard = (List<ECFDataModel>)Session["Tomyapprovedlist"];
            if (lstDashBoard == null)
            {
                lstDashBoard = objModel.myapprovedlistSearch().ToList();
                Session["Tomyapprovedlist"] = lstDashBoard;
            }

            var jsonResult = Json(lstDashBoard, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = 2147483647;
            return jsonResult;
        }
        public ActionResult Tomyapprovedlistexcel()
        {
            DataTable dtnew = objModel.myapprovedlistSearchexcel();
            if (dtnew.Rows.Count > 0)
            {
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Myapprovedlist.xls"));
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
                    //GGG
                }
                Response.End();
            }
            return View();
        }
        public ActionResult POWOReportexportexcel()
        {
            DataTable dtnew = objModel.POWOReportExportExcel();
            if (dtnew.Rows.Count > 0)
            {
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "POWOREPORT.xls"));
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
                    //GGG
                }
                Response.End();
            }
            return View();
        }
        // correction by dhasarathan
        public ActionResult downloadexcel(string EcfDateFrom = null, string EcfDateTo = null, string DocType = null, string docsubtype = null, string Code = null, string Name = null, string ECFClaimMonth = null, string queryempsup = null, string ECFDespatchDateTo = null, string ecfnumber = null, string ecfamount = null, string Ecfstatus = null, string ECFMode = null, string command = null, string Suppliercode = null, string Suppliername = null)
        {  
            if (ECFMode == "select" || ECFMode == "")
            {
                ECFMode = "";
                ViewBag.ecfmode = "";
            }

            if (queryempsup == null)
            {
                queryempsup = "";
            }
            if (queryempsup == "Employee")
            {
                queryempsup = "E";

            }
            else if (queryempsup == "Supplier")
            {
                queryempsup = "S";

            }

            DataTable dtnew = objModel.EcfReport(EcfDateFrom, EcfDateTo, DocType, docsubtype, Code, Name, ECFClaimMonth, queryempsup, ECFDespatchDateTo, ecfnumber, ecfamount, Ecfstatus, ECFMode, command, Suppliercode, Suppliername);
            // DataTable dtnew = ConvertListToDataTable((List<ECFDataModel>)Session["Records"]);
            dtnew.Columns.Remove("ecf_gid");
            //dtnew.Columns["ecf_date"].ColumnName = "ECF DATE";
            if (dtnew.Rows.Count > 0)
            {
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "ECF Summary.xls"));
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
                        string content = dr[j].ToString();
                        content.Replace(System.Environment.NewLine, " ");
                        Response.Write(str + Convert.ToString(dr[j]));
                        str = "\t";
                    }
                   
                    Response.Write("\n");
                    //GGG
                }
                Response.End();
            }
            return View();
        }
        [HttpPost]
        public JsonResult AutocompleteEmployeeCode(string RaiserCode)
        {
            List<CentraldataModel> obj = new List<CentraldataModel>();
            CentralModel v = new CentralModel();
            obj = v.AutofilterEmployee("", RaiserCode).ToList();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AutocompleteEmployeeName(string RaiserName)
        {
            List<CentraldataModel> obj = new List<CentraldataModel>();
            CentralModel v = new CentralModel();
            obj = v.AutofilterEmployee(RaiserName, "").ToList();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult EcfReport()
        {
            ViewBag.ecfmode = "";
            ViewBag.Docsubtype1 = "";
            list.Add("Employee");
            list.Add("Supplier");
            List<string> Rolelist1 = new List<string>();
            {
                Rolelist1 = list;
            };
            List<SelectListItem> role1 = new List<SelectListItem>();
            foreach (var item in Rolelist1)
            {

                role1.Add(new SelectListItem
                {
                    Text = item.ToString(),

                });
            }
            ViewBag.SupplierorEmployeeData = role1;
            List<ECFDataModel> cen = new List<ECFDataModel>();
            ECFDataModel ECFDoctype = new ECFDataModel();
            ECFDoctype.selectdoctypedata = new SelectList(objModel.SelectDocType().ToList(), "Docgid", "Docname");
            ViewBag.DocType = ECFDoctype;

            List<ECFDataModel> Rolelist = new List<ECFDataModel>();
            {
                Rolelist = objModel.SelectDocType().ToList();
            };
            List<SelectListItem> role = new List<SelectListItem>();
            foreach (var item in Rolelist)
            {

                role.Add(new SelectListItem
                {
                    Text = item.Docname.ToString(),
                    Value = item.Docgid.ToString(),
                });
            }
            ViewBag.DocType111 = role;
            ECFDataModel ECFStatusviewbag = new ECFDataModel();
            ECFStatusviewbag.statusnameData = new SelectList(objModel.SelectStatus().ToList(), "StatusGid", "statusname");
            ViewBag.ECFStatus = ECFStatusviewbag;

            if (Session["Records"] != null)
            {
                ECFDataModel viewbags = new ECFDataModel();
                viewbags = (ECFDataModel)Session["SearchData"];
                ViewBag.ecfnumber = viewbags.ECFNo;
                ViewBag.ecfamount = viewbags.ECFAmount;
                ViewBag.docvalue = viewbags.statusname;
                ViewBag.satval = viewbags.ECFStatus;
                ViewBag.ECFClaimMonth = viewbags.ClaimMonth;
                ViewBag.EcfDateFrom = viewbags.ecfdatefrom;
                ViewBag.EcfDateTo = viewbags.ecfdateto;
                ViewBag.Code = viewbags.Code;
                ViewBag.Name = viewbags.Name;
                ViewBag.SupplierorEmployee = viewbags.SupplierorEmployee;
                ViewBag.ecfmode = viewbags.ecfFor;
                ViewBag.Docsubtymename = viewbags.DocSubTypeName;
                cen = (List<ECFDataModel>)Session["Records"];
                //if (cen.Count == 0)
                //{
                //    ViewBag.Message = "No Record";
                //}
            }
            else
            {
                //cen = objModel.Search().ToList();
                //if (cen.Count == 0)
                //{
                //    ViewBag.Message = "No Record";
                //}
            }
            return View(cen);
        }
        [HttpPost]
        public ActionResult EcfReport(string EcfDateFrom = null, string EcfDateTo = null, string EcfNo = null, string EcfAmount = null, string InvoiceDateFrom = null, string InvoiceDateTo = null, string InvoiceNo = null, string InvoiceAmount = null, string EcfMode = null, string Ecfstatus = null, string suppliername = null, string suppliercode = null, string command = null)
        {
            string docsubtyname = string.Empty;
            ECFDataModel viewbags = new ECFDataModel();
            DataTable getdoctypename = new DataTable();
            DataTable getdocsubtype = new DataTable();
            //if (Session["Employeecode"] != null)
            //{
            //    Code = Session["Employeecode"].ToString();
            //    Name = Session["EmployeeName"].ToString();
            //}

            if (EcfMode == "select" || EcfMode == "")
            {
                EcfMode = "";
                ViewBag.ecfmode = "";
            }

            if (EcfMode != "" && EcfMode != "select")
            {
                List<SelectListItem> ddd = new List<SelectListItem>();
                foreach (var item in EcfMode)
                {
                    Boolean selected = false;
                    if (EcfMode == "S")
                    {
                        selected = true;
                        ViewBag.ecfmode = "Self";
                    }
                    else if (EcfMode == "P")
                    {
                        selected = true;
                        ViewBag.ecfmode = "Proxy";
                    }
                    else if (EcfMode == "C")
                    {
                        selected = true;
                        ViewBag.ecfmode = "CentralTeam";
                    }
                    else if (EcfMode == "R")
                    {
                        selected = true;
                        ViewBag.ecfmode = "Retention";
                    }
                    ddd.Add(new SelectListItem
                    {
                        // Text = item.Docname.ToString(),
                        //Value = item.Docgid.ToString(),
                        Selected = selected
                    });
                }

            }

            List<SelectCourier> courier = new List<SelectCourier>();
            {
                courier = objModel.SelectCourier().ToList();
            };

            ECFDataModel ECFDoctype = new ECFDataModel();
            ECFDoctype.selectdoctypedata = new SelectList(objModel.SelectDocType().ToList(), "Docgid", "Docname");
            ViewBag.DocType = ECFDoctype;

            ECFDataModel ECFStatusviewbag = new ECFDataModel();
            ECFStatusviewbag.statusnameData = new SelectList(objModel.SelectStatus().ToList(), "StatusGid", "statusname");
            ViewBag.ecfStatus = ECFStatusviewbag;

            ViewBag.EcfDateFrom = EcfDateFrom;
            ViewBag.EcfDateTo = EcfDateTo;
            ViewBag.invoicedatefrom = InvoiceDateFrom;
            ViewBag.invoicedateto = InvoiceDateTo;
            ViewBag.ecfnumber = EcfNo;
            ViewBag.ecfamount = EcfAmount;
            ViewBag.satval = Ecfstatus;
            ViewBag.invoiceno = InvoiceNo;
            ViewBag.invoiceamount = InvoiceAmount;
            ViewBag.Code = suppliercode;
            ViewBag.Name = suppliername;
            


            viewbags.ECFStatus = Ecfstatus;
            viewbags.ECFNo = ViewBag.ecfnumber;
            viewbags.ECFAmount = ViewBag.ecfamount;
            viewbags.ecfdatefrom = ViewBag.EcfDateFrom;
            viewbags.ecfdateto = ViewBag.EcfDateTo;
            viewbags.ecfFor = ViewBag.ecfmode;
            viewbags.InvoiceDateFrom = ViewBag.invoicedatefrom;
            viewbags.InvoiceDateTo = ViewBag.invoicedateto;
            viewbags.InvoiceNo = ViewBag.invoiceno;
            viewbags.InvoiceAmt = ViewBag.invoiceamount;

            Session["SearchData"] = viewbags;
            List<ECFDataModel> cen = new List<ECFDataModel>();
            cen = objModel.EcfReport(EcfDateFrom, EcfDateTo, EcfNo, EcfAmount, InvoiceDateFrom, InvoiceDateTo, InvoiceNo, InvoiceAmount, EcfMode, Ecfstatus,suppliercode,suppliername,command).ToList();
            Session["Records"] = cen;
            if (command == "Clear")
            {
                Session["SearchData"] = "";
            }
            if (cen.Count == 0)
            {
                ViewBag.Message = "No Record's Found";
            }

            return View(cen);
        }
        public ActionResult InvoiceClear()
        {
            Session["Employeecode"] = null;
            Session["EmployeeName"] = null;
            Session["Records"] = null;
            Session["SearchData"] = null;
            Session["SearchECfDespatchData"] = null;
            Session["SearcECFDeletionRecord"] = null;
            Session["SearcECFCancellationRecord"] = null;
            return RedirectToAction("EcfReport", "ECF");
        }

        [HttpPost]
        public JsonResult GetAutodisplayofNextItem()
        {
            string result = "";
            Int32 ecfgid = 0;
            try
            {
                List<ECFDataModel> lstcancellation = new List<ECFDataModel>();
                lstcancellation = objModel.CancellationSearch().ToList();               
                if (lstcancellation.Count == 0)
                {
                    result = "No Record";
                }
                else
                {
                    ecfgid = Convert.ToInt32(lstcancellation[0].ECFId);
                    result = "success";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return Json(ecfgid, result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAutodisplayofNextItemToDelete()
        {
            string result = "";
            Int32 ecfgid = 0;
            try
            {
                List<ECFDataModel> lstdeletion = new List<ECFDataModel>();
                lstdeletion = objModel.DeletionSearch().ToList();
                if (lstdeletion.Count == 0)
                {
                    result = "No Record";
                }
                else
                {
                    ecfgid = Convert.ToInt32(lstdeletion[0].ECFId);
                    result = "success";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return Json(ecfgid, result, JsonRequestBehavior.AllowGet);
        }
    }
}
