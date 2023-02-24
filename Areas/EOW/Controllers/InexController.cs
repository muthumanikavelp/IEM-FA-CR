using IEM.Areas.EOW.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IEM.Areas.EOW.Controllers
{
    public class InexController : Controller
    {
       DataTable dt = new DataTable();
       List<string> list = new List<string>();
       private InexRepository objModel;
       CentralModel cen = new CentralModel();
       String result = String.Empty;
        public InexController()
            : this(new InexModel())
        {

        }
        public InexController(InexRepository objM)
        {
            objModel = objM;
        }
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ecfmode = "";
            ViewBag.Docsubtype1 = "";
            List<InexDataModel> inex = new List<InexDataModel>();
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
            
            InexDataModel ECFDoctype = new InexDataModel();
            ECFDoctype.selectdoctypedata = new SelectList(objModel.SelectDocType().ToList(), "Docgid", "Docname");
            ViewBag.DocType = ECFDoctype;

            InexDataModel ECFStatusviewbag = new InexDataModel();
            ECFStatusviewbag.statusnameData = new SelectList(objModel.SelectStatus().ToList(), "StatusGid", "statusname");
            ViewBag.ECFStatus = ECFStatusviewbag; 
           
            List<InexDataModel> Rolelist = new List<InexDataModel>();
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

            if (Session["RecordsInex"] != null)
            {
                inex = (List<InexDataModel>)Session["RecordsInex"];
            }
            else
            {
                inex = objModel.Search().ToList();
            }
            return View(inex);
        }
        [HttpPost]
        public ActionResult Index(string EcfDateFrom = null, string EcfDateTo = null, string DocType = null, string docsubtype = null, string Code = null, string Name = null, string ECFClaimMonth = null, string queryempsup = null, string ECFDespatchDateTo = null, string Suppliername = null, string ecfnumber = null, string ecfamount = null, string Ecfstatus = null, string ECFMode = null, string command = null)
        {
            string subtype = docsubtype;
            string docsubtyname = string.Empty;
            ECFDataModel viewbags = new ECFDataModel();
            DataTable getdoctypename = new DataTable();
            DataTable getdocsubtype = new DataTable();
            List<InexDataModel> inex = new List<InexDataModel>();
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
                if (item == queryempsup)
                {
                    selected = true;
                    ViewBag.SelectEmpOrSup = queryempsup;
                }
                role1.Add(new SelectListItem
                {
                    Text = item.ToString(),
                    Selected = selected
                   
                });
            }
            ViewBag.SupplierorEmployeeData = role1;

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
            List<InexDataModel> cen = new List<InexDataModel>();
            DataTable getecfstatusname = new DataTable();
            if (command == "Search" || command == "Refresh")
            {
                   
                if (Session["Employeecode"] != null)
                {
                    Code = Session["Employeecode"].ToString();
                    Name = Session["EmployeeName"].ToString();
                }

                InexDataModel ECFDoctype = new InexDataModel();
                ECFDoctype.selectdoctypedata = new SelectList(objModel.SelectDocType().ToList(), "Docgid", "Docname");
                ViewBag.DocType = ECFDoctype;

                InexDataModel ECFStatusviewbag = new InexDataModel();
                ECFStatusviewbag.statusnameData = new SelectList(objModel.SelectStatus().ToList(), "StatusGid", "statusname");
                ViewBag.ECFStatus = ECFStatusviewbag; 

               

                ViewBag.EcfDateFrom = EcfDateFrom;
                ViewBag.EcfDateTo = EcfDateTo;
                ViewBag.Code = Code;
                ViewBag.Name = Name;
                ViewBag.ECFClaimMonth = ECFClaimMonth;
                ViewBag.ecfnumber = ecfnumber;
                ViewBag.ecfamount = ecfamount;
                //ViewBag.ECFCourierName = ECFCourierName;
                //ViewBag.ECFAWBNo = ECFAWBNo;
                inex = objModel.Search(EcfDateFrom, EcfDateTo, DocType, subtype, Code, Name, ECFClaimMonth, queryempsup, ECFDespatchDateTo, Suppliername, ecfnumber, ecfamount, Ecfstatus, ECFMode).ToList();
                if (inex.Count == 0)
                {
                    ViewBag.Message = "No Record Found";
                }
                
            }
            else 
            {
                inex = objModel.Search().ToList();
            }

            return View(inex);
        }
        [HttpGet]
        public ActionResult Clear()
        {
            Session["Employeecode"] = null;
            Session["EmployeeName"] = null;
            Session["RecordsInex"] = null;
            Session["ecf_gid"] = null;
            return RedirectToAction("Index", "ECF");
        }
        [HttpPost]
        public JsonResult GetValue(ECFDataModel delnote)
        {
            ECFDataModel TypeModel = new ECFDataModel();
            TypeModel.SelectDocSubType = new SelectList(objModel.SelectDocSubType(delnote.Docgid), "DocSubname", "DocSubgid");
            return Json(objModel.SelectDocSubType(delnote.Docgid), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEmployee(InexDataModel ecf)
        {
            Session["Employeecode"] = ecf.Code;
            Session["EmployeeName"] = ecf.Name;
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult inexView(int id)
        {
            Session["ecf_gid"] = id;
            List<InexDataModel> cen = new List<InexDataModel>();
            cen = objModel.SelectView(id).ToList();
            return PartialView(cen);
        }
        public JsonResult InexUpdate()
        {
            int id = Convert.ToInt16(Convert.ToInt16(Session["ecf_gid"]));
            result = objModel.UpdateInex(id);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult InexReject()
        {
            int id = Convert.ToInt16(Convert.ToInt16(Session["ecf_gid"]));
            result = objModel.RejectInex(id);
            return Json(result, JsonRequestBehavior.AllowGet);
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
        public JsonResult SupplierSearchECF(string SupplierName = "", string SupplierCode = "")
        {
            List<CentraldataModel> obj = new List<CentraldataModel>();
            CentraldataModel supplier = new CentraldataModel();
            if (SupplierCode != "" || SupplierName != "")
            {
                obj = cen.SelectSupplierSearch(SupplierName, SupplierCode).ToList();
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
        [HttpGet]
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
    }
}
