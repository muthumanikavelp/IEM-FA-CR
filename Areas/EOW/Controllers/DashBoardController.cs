using IEM.Areas.EOW.Models;
using IEM.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Collections.Specialized;
using IEM.Areas.MASTERS.Controllers;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using IEM.App_Start;
namespace IEM.Areas.EOW.Controllers
{
    [NoDirectAccess]
    public class DashBoardController : Controller
    {
        //
        // GET: /EOW/DashBoard/
        ErrorLog objErrorLog = new ErrorLog();
        private EOW_IRepository objModel;
        CmnFunctions objCmnFunctions = new CmnFunctions();
        private FileServer Cmnfs = new FileServer();
        CentralModel objd = new CentralModel();
        ECFModel ECFModelobj = new ECFModel();
        CommonIUD objCommonIUD = new CommonIUD();
        LocalConveyanceNewController locals = new LocalConveyanceNewController();
        public DashBoardController()
            : this(new EOW_DataModel())
        {

        }
        public DashBoardController(EOW_IRepository objM)
        {
            objModel = objM;
        }
        public ActionResult Index()
        {
            Session["docAppoalc"] = "app";
            List<DashBoard> lstutlity = new List<DashBoard>();
            lstutlity = objModel.GetDashBoardUtlity(objCmnFunctions.GetLoginUserGid().ToString(), "C").ToList();
            int count = lstutlity.Count;
            Session["UtilityPayments"] = count.ToString();
            if (Session["Activetab"] == null)
            {
                Session["Activetab"] = "0";
            }
            string centralmkerorchk = objModel.Selectsupplier(objCmnFunctions.GetLoginUserGid().ToString());
            Session["centralmkerorchk"] = centralmkerorchk.ToString();
            Session["ToGetMyRequest"] = null;
            Session["ToGetMyRequestapp"] = null;
            Session["ToGetCentralchkr"] = null;
            Session["ToGetCentralmkr"] = null;
            Session["ToGetMyRequesthold"] = null;
            return View();
        }
        //public ActionResult Index()
        //{
        //    Session["docAppoalc"] = "app";
        //    DashBoard eowemp = new DashBoard();
        //    eowemp.DocTypeData = new SelectList(objModel.doctypedata().ToList(), "DocTypeIdd", "DocTypeName");
        //    eowemp.StatusTypeData = new SelectList(objModel.GetStatusType().ToList(), "StatusTypeId", "StatusTypeName");
        //    ViewBag.DashBoardheaderser = eowemp;
        //    DashBoard eowempapp = new DashBoard();
        //    eowempapp.DocTypeData = new SelectList(objModel.doctypedata().ToList(), "DocTypeIdd", "DocTypeName");
        //    eowempapp.StatusTypeData = new SelectList(objModel.GetStatusTypeapp().ToList(), "StatusTypeId", "StatusTypeName");
        //    ViewBag.DashBoardheaderserapp = eowempapp;
        //    ViewBag.DashBoardheadercentralteammkr = eowempapp;
        //    ViewBag.DashBoardheadercentralteamchkr = eowempapp;
        //    if (Session["DashSearchItems"] != null)
        //    {
        //        DashBoard viewbags = new DashBoard();
        //        viewbags = (DashBoard)Session["frstwork"];

        //        ViewBag.docval = viewbags.DocTypeName;
        //        ViewBag.satval = viewbags.StatusTypeName;
        //        ViewBag.DashBoardheaderser = viewbags;
        //    }
        //    else
        //    {
        //        Session["DashSearchItems"] = null;
        //    }
        //    if (Session["DashSearchItemsapp"] != null)
        //    {
        //        DashBoard viewbags = new DashBoard();
        //        viewbags = (DashBoard)Session["scndwork"];

        //        ViewBag.docvala = viewbags.DocTypeName;
        //        ViewBag.satvala = viewbags.StatusTypeName;
        //        ViewBag.DashBoardheaderserapp = viewbags;
        //    }
        //    else
        //    {
        //        Session["DashSearchItemsapp"] = null;
        //    }
        //    if (Session["DashSearchItemschk"] != null)
        //    {
        //        DashBoard viewbags = new DashBoard();
        //        viewbags = (DashBoard)Session["thrdwork"];

        //        ViewBag.docvala = viewbags.DocTypeName;
        //        ViewBag.satvala = viewbags.StatusTypeName;
        //        ViewBag.DashBoardheadercentralteamchkr = viewbags;
        //    }
        //    else
        //    {
        //        Session["DashSearchItemschk"] = null;
        //    }
        //    if (Session["DashSearchItemsmkr"] != null)
        //    {
        //        DashBoard viewbags = new DashBoard();
        //        viewbags = (DashBoard)Session["thrdwork"];

        //        ViewBag.docvala = viewbags.DocTypeName;
        //        ViewBag.satvala = viewbags.StatusTypeName;
        //        ViewBag.DashBoardheadercentralteammkr = viewbags;
        //    }
        //    else
        //    {
        //        Session["DashSearchItemsmkr"] = null;
        //    }
        //    List<DashBoard> lstutlity = new List<DashBoard>();
        //    lstutlity = objModel.GetDashBoardUtlity(objCmnFunctions.GetLoginUserGid().ToString(), "C").ToList();
        //    int count = lstutlity.Count;
        //    Session["UtilityPayments"] = count.ToString();
        //    if (Session["Activetab"] == null)
        //    {
        //        Session["Activetab"] = "0";
        //    }
        //    string centralmkerorchk = objModel.Selectsupplier(objCmnFunctions.GetLoginUserGid().ToString());
        //    Session["centralmkerorchk"] = centralmkerorchk.ToString();
        //    Session["ToGetMyRequest"] = null;
        //    Session["ToGetMyRequestapp"] = null;
        //    Session["ToGetCentralchkr"] = null;
        //    Session["ToGetCentralmkr"] = null;
        //    return View();
        //}
        [HttpGet]
        public PartialViewResult _LoadGstDetails()
        {
        //  List<EOW_TravelClaim> lstnew = new List<EOW_TravelClaim>();
            return PartialView();
        }
        [HttpGet]
        public PartialViewResult _LoadRCMDetails()
        {
        //    List<RCMEnteries> lstnew = new List<RCMEnteries>();
            return PartialView();
        }
        [HttpPost]
        public ActionResult Index(string txtdbdocdate = null, string txtdbdocno = null, string txtdbdocamount = null, string ddldbDocType = null, string ddldbDocStatus = null, string command = null)
        {
            CentralModel objdcentral = new CentralModel();
            Session["Activetab"] = "0";
            Session["docAppoalc"] = "app";
            DashBoard viewbags = new DashBoard();
            List<DashBoard> records = new List<DashBoard>();
            List<DashBoard> records1 = new List<DashBoard>();
            DashBoard fff = new DashBoard();
            if (command == "Search" || command == "Refresh")
            {
                records = objModel.GetDashBoardDetailssearch(objCmnFunctions.GetLoginUserGid().ToString(), txtdbdocdate, txtdbdocno, txtdbdocamount, ddldbDocType, ddldbDocStatus).ToList();
                //records = records1.ToList();
                Session["DashSearchItems"] = records;
                Session["DashSearchItemsapp"] = null;
                viewbags.DocTypeData = new SelectList(objModel.doctypedata().ToList(), "DocTypeIdd", "DocTypeName");
                viewbags.StatusTypeData = new SelectList(objModel.GetStatusType().ToList(), "StatusTypeId", "StatusTypeName");
                viewbags.DocDate = txtdbdocdate;
                viewbags.Docno = txtdbdocno;
                viewbags.Docamount = txtdbdocamount;
                ViewBag.DashBoardheaderser = viewbags;
                ViewBag.docval = ddldbDocType;
                ViewBag.satval = ddldbDocStatus;

                DashBoard eowempapp = new DashBoard();
                eowempapp.DocTypeData = new SelectList(objModel.doctypedata().ToList(), "DocTypeIdd", "DocTypeName");
                eowempapp.StatusTypeData = new SelectList(objModel.GetStatusTypeapp().ToList(), "StatusTypeId", "StatusTypeName");
                ViewBag.DashBoardheaderserapp = eowempapp;

                viewbags.DocTypeName = ddldbDocType;
                viewbags.StatusTypeName = ddldbDocStatus;
                Session["frstwork"] = viewbags;
                Session["Activetab"] = "1";
                DashBoard centralteammkr = new DashBoard();
                ViewBag.DashBoardheadercentralteammkr = centralteammkr;
                ViewBag.DashBoardheadercentralteamchkr = centralteammkr;
            }
            else if (command == "Searchapp")
            {
                records = objModel.GetDashBoardDetailssearcha(objCmnFunctions.GetLoginUserGid().ToString(), txtdbdocdate, txtdbdocno, txtdbdocamount, ddldbDocType, ddldbDocStatus).ToList();
                //records = records1.ToList();
                Session["DashSearchItemsapp"] = records;
                Session["DashSearchItems"] = null;
                viewbags.DocTypeData = new SelectList(objModel.doctypedata().ToList(), "DocTypeIdd", "DocTypeName");
                viewbags.StatusTypeData = new SelectList(objModel.GetStatusTypeapp().ToList(), "StatusTypeId", "StatusTypeName");
                viewbags.DocDate = txtdbdocdate;
                viewbags.Docno = txtdbdocno;
                viewbags.Docamount = txtdbdocamount;
                ViewBag.DashBoardheaderserapp = viewbags;
                ViewBag.docvala = ddldbDocType;
                ViewBag.satvala = ddldbDocStatus;

                DashBoard eowemp = new DashBoard();
                eowemp.DocTypeData = new SelectList(objModel.doctypedata().ToList(), "DocTypeIdd", "DocTypeName");
                eowemp.StatusTypeData = new SelectList(objModel.GetStatusType().ToList(), "StatusTypeId", "StatusTypeName");
                ViewBag.DashBoardheaderser = eowemp;

                viewbags.DocTypeName = ddldbDocType;
                viewbags.StatusTypeName = ddldbDocStatus;
                Session["scndwork"] = viewbags;
                Session["Activetab"] = "2";
                DashBoard centralteammkr = new DashBoard();
                ViewBag.DashBoardheadercentralteammkr = centralteammkr;
                ViewBag.DashBoardheadercentralteamchkr = centralteammkr;
            }
            else if (command == "Searchcentralmkr")
            {
                DashBoard eowempapp = new DashBoard();
                eowempapp.DocTypeData = new SelectList(objModel.doctypedata().ToList(), "DocTypeIdd", "DocTypeName");
                eowempapp.StatusTypeData = new SelectList(objModel.GetStatusTypeapp().ToList(), "StatusTypeId", "StatusTypeName");
                ViewBag.DashBoardheaderserapp = eowempapp;
                ViewBag.DashBoardheaderser = eowempapp;

                DashBoard centralteammkr = new DashBoard();
                centralteammkr.DocDate = txtdbdocdate;
                centralteammkr.Docno = txtdbdocno;
                centralteammkr.Docamount = txtdbdocamount;
                ViewBag.DashBoardheadercentralteammkr = centralteammkr;
                ViewBag.DashBoardheadercentralteamchkr = centralteammkr;
                Session["thrdwork"] = centralteammkr;

                List<CentraldataModel> emp = new List<CentraldataModel>();
                emp = objdcentral.SelectrejectDetails(txtdbdocdate, txtdbdocno, txtdbdocamount).ToList();
                Session["DashSearchItemsmkr"] = emp;
                Session["DashSearchItemsapp"] = null;
                Session["DashSearchItems"] = null;
                Session["DashSearchItemschk"] = null;
                Session["Activetab"] = "3";
            }
            else if (command == "Searchcentralchk")
            {
                DashBoard eowempapp = new DashBoard();
                eowempapp.DocTypeData = new SelectList(objModel.doctypedata().ToList(), "DocTypeIdd", "DocTypeName");
                eowempapp.StatusTypeData = new SelectList(objModel.GetStatusTypeapp().ToList(), "StatusTypeId", "StatusTypeName");
                ViewBag.DashBoardheaderserapp = eowempapp;
                ViewBag.DashBoardheaderser = eowempapp;

                DashBoard centralteamchk = new DashBoard();
                centralteamchk.DocDate = txtdbdocdate;
                centralteamchk.Docno = txtdbdocno;
                centralteamchk.Docamount = txtdbdocamount;
                ViewBag.DashBoardheadercentralteamchkr = centralteamchk;
                ViewBag.DashBoardheadercentralteammkr = centralteamchk;
                Session["thrdwork"] = centralteamchk;
                List<CentraldataModel> emp = new List<CentraldataModel>();
                emp = objdcentral.SelectCentralCheckerDetails(txtdbdocdate, txtdbdocno, txtdbdocamount).ToList();
                Session["DashSearchItemschk"] = emp;
                Session["DashSearchItemsapp"] = null;
                Session["DashSearchItems"] = null;
                Session["DashSearchItemsmkr"] = null;
                Session["Activetab"] = "3";
            }
            else if (command == "Clearcentralmkr")
            {
                Session["DashSearchItemsmkr"] = null;
                Session["DashSearchItemschk"] = null;
                Session["DashSearchItems"] = null;
                Session["DashSearchItemsapp"] = null;
                viewbags.DocTypeData = new SelectList(objModel.doctypedata().ToList(), "DocTypeId", "DocTypeName");
                viewbags.StatusTypeData = new SelectList(objModel.GetStatusType().ToList(), "StatusTypeId", "StatusTypeName");
                viewbags.DocDate = "";
                viewbags.Docno = "";
                viewbags.Docamount = "";
                ViewBag.DashBoardheaderser = viewbags;

                DashBoard eowempapp = new DashBoard();
                eowempapp.DocTypeData = new SelectList(objModel.doctypedata().ToList(), "DocTypeIdd", "DocTypeName");
                eowempapp.StatusTypeData = new SelectList(objModel.GetStatusTypeapp().ToList(), "StatusTypeId", "StatusTypeName");
                ViewBag.DashBoardheaderserapp = eowempapp;
                Session["frstwork"] = viewbags;
                Session["Activetab"] = "3";

                ViewBag.DashBoardheadercentralteamchkr = viewbags;
                ViewBag.DashBoardheadercentralteammkr = viewbags;
            }
            else if (command == "Clearcentralchk")
            {
                Session["DashSearchItemsmkr"] = null;
                Session["DashSearchItemschk"] = null;
                Session["DashSearchItems"] = null;
                Session["DashSearchItemsapp"] = null;
                viewbags.DocTypeData = new SelectList(objModel.doctypedata().ToList(), "DocTypeId", "DocTypeName");
                viewbags.StatusTypeData = new SelectList(objModel.GetStatusType().ToList(), "StatusTypeId", "StatusTypeName");
                viewbags.DocDate = "";
                viewbags.Docno = "";
                viewbags.Docamount = "";
                ViewBag.DashBoardheaderser = viewbags;

                DashBoard eowempapp = new DashBoard();
                eowempapp.DocTypeData = new SelectList(objModel.doctypedata().ToList(), "DocTypeIdd", "DocTypeName");
                eowempapp.StatusTypeData = new SelectList(objModel.GetStatusTypeapp().ToList(), "StatusTypeId", "StatusTypeName");
                ViewBag.DashBoardheaderserapp = eowempapp;
                Session["frstwork"] = viewbags;
                Session["Activetab"] = "3";
                ViewBag.DashBoardheadercentralteamchkr = viewbags;
                ViewBag.DashBoardheadercentralteammkr = viewbags;
            }
            else if (command == "Clear")
            {
                Session["DashSearchItemsmkr"] = null;
                Session["DashSearchItemschk"] = null;
                Session["DashSearchItems"] = null;
                Session["DashSearchItemsapp"] = null;
                viewbags.DocTypeData = new SelectList(objModel.doctypedata().ToList(), "DocTypeId", "DocTypeName");
                viewbags.StatusTypeData = new SelectList(objModel.GetStatusType().ToList(), "StatusTypeId", "StatusTypeName");
                viewbags.DocDate = "";
                viewbags.Docno = "";
                viewbags.Docamount = "";
                ViewBag.DashBoardheaderser = viewbags;

                DashBoard eowempapp = new DashBoard();
                eowempapp.DocTypeData = new SelectList(objModel.doctypedata().ToList(), "DocTypeIdd", "DocTypeName");
                eowempapp.StatusTypeData = new SelectList(objModel.GetStatusTypeapp().ToList(), "StatusTypeId", "StatusTypeName");
                ViewBag.DashBoardheaderserapp = eowempapp;
                Session["frstwork"] = viewbags;
                Session["Activetab"] = "1";
                ViewBag.DashBoardheadercentralteamchkr = viewbags;
                ViewBag.DashBoardheadercentralteammkr = viewbags;
            }
            else if (command == "Clearapp")
            {
                Session["DashSearchItemsmkr"] = null;
                Session["DashSearchItemschk"] = null;
                Session["DashSearchItemsapp"] = null;
                Session["DashSearchItems"] = null;
                viewbags.DocTypeData = new SelectList(objModel.doctypedata().ToList(), "DocTypeId", "DocTypeName");
                viewbags.StatusTypeData = new SelectList(objModel.GetStatusTypeapp().ToList(), "StatusTypeId", "StatusTypeName");
                viewbags.DocDate = "";
                viewbags.Docno = "";
                viewbags.Docamount = "";
                ViewBag.DashBoardheaderserapp = viewbags;

                DashBoard eowemp = new DashBoard();
                eowemp.DocTypeData = new SelectList(objModel.doctypedata().ToList(), "DocTypeIdd", "DocTypeName");
                eowemp.StatusTypeData = new SelectList(objModel.GetStatusType().ToList(), "StatusTypeId", "StatusTypeName");
                ViewBag.DashBoardheaderser = eowemp;
                Session["scndwork"] = viewbags;
                Session["Activetab"] = "2";
                ViewBag.DashBoardheadercentralteamchkr = viewbags;
                ViewBag.DashBoardheadercentralteammkr = viewbags;
            }
            return View();
        }
        public ActionResult selectdata(string id)
        {
            int redirectpage = 0;
            string CygnetFlag = "N";
            string confirmsupplier = objModel.selectsupplierinvoice(id.ToString(), "A").ToString();
            if (confirmsupplier == "4" || confirmsupplier == "11")
            {
                EOW_Supplierinvoice seowemp = new EOW_Supplierinvoice();
                List<EOW_Supplierinvoice> sobjobjMExpense = new List<EOW_Supplierinvoice>();
                sobjobjMExpense = objModel.SelectViewdatasupplier(id, "A").ToList();

                seowemp.ECF_Amount = sobjobjMExpense[0].ECF_Amount.ToString();
                Session["Ecfqueueamount"] = sobjobjMExpense[0].ECF_Amount.ToString();
                seowemp.ECF_Date = sobjobjMExpense[0].ECF_Date.ToString();
                seowemp.raisermodeId = sobjobjMExpense[0].raisermodeId.ToString();
                seowemp.Grade = sobjobjMExpense[0].Grade.ToString();
                seowemp.Raiser_Code = sobjobjMExpense[0].Raiser_Code.ToString();
                seowemp.Raiser_Name = sobjobjMExpense[0].Raiser_Name.ToString();
                seowemp.ecfdescription = sobjobjMExpense[0].ecfdescription.ToString();
                seowemp.Raiser_Modedata = new SelectList(objModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", sobjobjMExpense[0].raisermodeId.ToString());
                seowemp.amort = sobjobjMExpense[0].amort.ToString();
                seowemp.amortdec = sobjobjMExpense[0].amortdec.ToString();
                seowemp.from = sobjobjMExpense[0].from.ToString();
                seowemp.to = sobjobjMExpense[0].to.ToString();
                seowemp.ecfremark = sobjobjMExpense[0].ecfremark.ToString();

                seowemp.Suppliergid = sobjobjMExpense[0].Suppliergid.ToString();
                seowemp.Suppliercode = sobjobjMExpense[0].Suppliercode.ToString();
                seowemp.Suppliername = sobjobjMExpense[0].Suppliername.ToString();
                seowemp.CurrencyName = sobjobjMExpense[0].CurrencyName.ToString();
                seowemp.Exrate = sobjobjMExpense[0].Exrate.ToString();
                seowemp.Currencyamount = sobjobjMExpense[0].Currencyamount.ToString();
                seowemp.DocName = Convert.ToString(sobjobjMExpense[0].DocName);
                seowemp.CurrencyId = sobjobjMExpense[0].CurrencyId.ToString();
                seowemp.ecf_Paymode = sobjobjMExpense[0].ecf_Paymode.ToString();
                seowemp.CygnetFlag = sobjobjMExpense[0].CygnetFlag.ToString();
                CygnetFlag = sobjobjMExpense[0].CygnetFlag.ToString();
                Session["CygnetFlag"] = sobjobjMExpense[0].CygnetFlag.ToString();
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
                seowemp.Raiser_Modedata = new SelectList(objModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", seowemp.raisermodeId);
                seowemp.doctypedata = new SelectList(objModel.GetDoctype().ToList(), "DocId", "DocName", seowemp.DocId);
                seowemp.Currencydata = new SelectList(objModel.GetCurrency().ToList(), "CurrencyId", "CurrencyName", seowemp.CurrencyId);
                seowemp.ecf_GID = Convert.ToInt32(sobjobjMExpense[0].ecf_GID.ToString());
                seowemp.Queue_GID = Convert.ToInt32(sobjobjMExpense[0].Queue_GID.ToString());
                Session["DashBoard"] = seowemp;
                redirectpage = Convert.ToInt32(confirmsupplier.ToString());
            }
            else
            {
                EOW_EmployeeeExpense eowemp = new EOW_EmployeeeExpense();
                List<EOW_EmployeeeExpense> objobjMExpense = new List<EOW_EmployeeeExpense>();

                confirmsupplier = objModel.selectsupplierinvoice(id.ToString(), "A").ToString();
                if (confirmsupplier == "6" || confirmsupplier == "7" || confirmsupplier=="12")
                {
                    objobjMExpense = objModel.SelectViewdata(id, "AL").ToList();
                }
                if (confirmsupplier == "2" || confirmsupplier == "5")
                {
                    objobjMExpense = objModel.SelectViewdata(id, "AL").ToList();
                }
                else if (confirmsupplier != "2" && confirmsupplier != "5" && confirmsupplier != "6" && confirmsupplier != "7" && confirmsupplier != "12")
                {
                    objobjMExpense = objModel.SelectViewdata(id, "A").ToList();
                }

                eowemp.ecfremark = objobjMExpense[0].ecfremark.ToString();
                eowemp.ecfdelmatamt = objobjMExpense[0].ecfdelmatamt.ToString();
                eowemp.ECF_Amount = objobjMExpense[0].ECF_Amount.ToString();
                eowemp.ClaimMonth = locals.getconverttomonthtodateto(objobjMExpense[0].ClaimMonth.ToString());
                eowemp.ECF_Date = objobjMExpense[0].ECF_Date.ToString();
                eowemp.Grade = objobjMExpense[0].Grade.ToString();
                eowemp.ecfdescription = objobjMExpense[0].ecfdescription.ToString();
                eowemp.noofperson = objobjMExpense[0].noofperson.ToString();
                eowemp.Raiser_Code = objobjMExpense[0].Raiser_Code.ToString();
                eowemp.Raiser_Name = objobjMExpense[0].Raiser_Name.ToString();
                eowemp.Raiser_Modedata = new SelectList(objModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", objobjMExpense[0].raisermodeId.ToString());
                eowemp.ecf_GID = Convert.ToInt32(objobjMExpense[0].ecf_GID.ToString());
                eowemp.invoice_GID = Convert.ToInt32(objobjMExpense[0].invoice_GID.ToString());
                eowemp.Queue_GID = Convert.ToInt32(objobjMExpense[0].Queue_GID.ToString());
                eowemp.SubCategoryName = objobjMExpense[0].SubCategoryName.ToString();
                eowemp.ecf_paymode = objobjMExpense[0].ecf_paymode.ToString();
                eowemp.CygnetFlag = objobjMExpense[0].CygnetFlag.ToString();
                CygnetFlag = objobjMExpense[0].CygnetFlag.ToString();
                Session["CygnetFlag"] = objobjMExpense[0].CygnetFlag.ToString();
                Session["DashBoard"] = eowemp;
                redirectpage = Convert.ToInt32(objobjMExpense[0].Doctypeid.ToString());

            }
            int regular = Convert.ToInt32(ConfigurationManager.AppSettings["EcfRegular"].ToString());
            int travel = Convert.ToInt32(ConfigurationManager.AppSettings["EcfTravel"].ToString());
            int LocalCoveyance = Convert.ToInt32(ConfigurationManager.AppSettings["EcfLocalCoveyance"].ToString());
            int SupplierInvoiceDSA = Convert.ToInt32(ConfigurationManager.AppSettings["EcfSupplierInvoiceDSA"].ToString());
            int SupplierInvoice = Convert.ToInt32(ConfigurationManager.AppSettings["EcfSupplierInvoice"].ToString());
            int EcfPettyCash = Convert.ToInt32(ConfigurationManager.AppSettings["EcfPettyCash"].ToString());
            int Insurance = 11;
            int Insadvance = 12;

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
                //return RedirectToAction("Index", "SupplierInvoiceNew");
                if (CygnetFlag == "Y")
                {
                    return RedirectToAction("Index", "SupplierInvoiceNew");
                }
                else
                {
                    return RedirectToAction("SupplierIndex", "SupplierInvoiceNew");
                }
            }
            if (EcfPettyCash == redirectpage)
            {
                return RedirectToAction("Index", "EmployeeClaimNewPetty");
            }
            if (Insurance == redirectpage)
            {
                return RedirectToAction("Index", "Insurance");
            }

            if (Insadvance == redirectpage)
            {
                return RedirectToAction("Index", "RaisingInsurance");
            }
            else
            {
                return RedirectToAction("Index", "RaisingArf");
            }
        }
        public ActionResult selectctdraft(string id)
        {
            Session["EcfGid"] = id;
            EOW_Supplierinvoice seowemp = new EOW_Supplierinvoice();

            List<EOW_Supplierinvoice> sobjobjMExpense = new List<EOW_Supplierinvoice>();
            sobjobjMExpense = objModel.SelectViewdatasupplier(id, "ctdata").ToList();

            seowemp.ECF_Amount = sobjobjMExpense[0].ECF_Amount.ToString();
            Session["Ecfqueueamount"] = sobjobjMExpense[0].ECF_Amount.ToString();
            seowemp.ECF_Date = sobjobjMExpense[0].ECF_Date.ToString();
            seowemp.raisermodeId = sobjobjMExpense[0].raisermodeId.ToString();
            seowemp.Grade = sobjobjMExpense[0].Grade.ToString();
            seowemp.Raiser_Code = sobjobjMExpense[0].Raiser_Code.ToString();
            seowemp.Raiser_Name = sobjobjMExpense[0].Raiser_Name.ToString();
            seowemp.ecfdescription = sobjobjMExpense[0].ecfdescription.ToString();
            seowemp.raisermodeId = sobjobjMExpense[0].raisermodeId.ToString();
            seowemp.Raiser_Modedata = new SelectList(objModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", sobjobjMExpense[0].raisermodeId.ToString());
            seowemp.amort = sobjobjMExpense[0].amort.ToString();
            seowemp.amortdec = sobjobjMExpense[0].amortdec.ToString();
            seowemp.from = sobjobjMExpense[0].from.ToString();
            seowemp.to = sobjobjMExpense[0].to.ToString();
            seowemp.ecfremark = sobjobjMExpense[0].ecfremark.ToString();

            seowemp.raisermodeName = sobjobjMExpense[0].chkraiser_gid.ToString();
            seowemp.ecfraisergid = sobjobjMExpense[0].chkraiser_gid.ToString();

            seowemp.Suppliergid = sobjobjMExpense[0].Suppliergid.ToString();
            seowemp.Suppliercode = sobjobjMExpense[0].Suppliercode.ToString();
            seowemp.Suppliername = sobjobjMExpense[0].Suppliername.ToString();
            seowemp.CurrencyName = sobjobjMExpense[0].CurrencyName.ToString();
            seowemp.Exrate = sobjobjMExpense[0].Exrate.ToString();
            seowemp.Currencyamount = sobjobjMExpense[0].Currencyamount.ToString();
            seowemp.DocName = sobjobjMExpense[0].DocName.ToString();
            seowemp.CurrencyId = sobjobjMExpense[0].CurrencyId.ToString();
            string doc = sobjobjMExpense[0].DocName.ToString(); ;

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
            seowemp.Raiser_Modedata = new SelectList(objModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", seowemp.raisermodeId);
            seowemp.doctypedata = new SelectList(objModel.GetDoctype().ToList(), "DocId", "DocName", seowemp.DocId);
            seowemp.Currencydata = new SelectList(objModel.GetCurrency().ToList(), "CurrencyId", "CurrencyName", seowemp.CurrencyId);
            seowemp.ecf_GID = Convert.ToInt32(sobjobjMExpense[0].ecf_GID.ToString());
            seowemp.Queue_GID = Convert.ToInt32(sobjobjMExpense[0].Queue_GID.ToString());

            Session["CentralTablesMain"] = seowemp;
            return RedirectToAction("CtMakerIndex", "SupplierInvoiceNew");
        }
        public ActionResult selectdatadraft(string id)
        {
            try
            {
                //bharathidhasan kumar
                string supplierreman = "",CygnetFlag="N";

                int redirectpage = 0;
                string confirmsupplier = objModel.selectsupplierinvoice(id.ToString(), "D").ToString();
                if (confirmsupplier == "4" || confirmsupplier == "11")
                {
                    EOW_Supplierinvoice seowemp = new EOW_Supplierinvoice();
                    List<EOW_Supplierinvoice> sobjobjMExpense = new List<EOW_Supplierinvoice>();
                    sobjobjMExpense = objModel.SelectViewdatasupplier(id, "D").ToList();

                seowemp.ECF_Amount = sobjobjMExpense[0].ECF_Amount.ToString();
                Session["Ecfqueueamount"] = sobjobjMExpense[0].ECF_Amount.ToString();
                seowemp.ECF_Date = sobjobjMExpense[0].ECF_Date.ToString();
                seowemp.raisermodeId = sobjobjMExpense[0].raisermodeId.ToString();
                seowemp.Grade = sobjobjMExpense[0].Grade.ToString();
                seowemp.Raiser_Code = sobjobjMExpense[0].Raiser_Code.ToString();
                seowemp.Raiser_Name = sobjobjMExpense[0].Raiser_Name.ToString();
                seowemp.ecfdescription = sobjobjMExpense[0].ecfdescription.ToString();
                seowemp.raisermodeId = sobjobjMExpense[0].raisermodeId.ToString();
                if (sobjobjMExpense[0].raisermodeId.ToString() == "M")
                {
                    supplierreman = "M";
                }
                else
                {
                    supplierreman = "S";
                }
                seowemp.Raiser_Modedata = new SelectList(objModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", sobjobjMExpense[0].raisermodeId.ToString());
                seowemp.amort = sobjobjMExpense[0].amort.ToString();
                seowemp.amortdec = sobjobjMExpense[0].amortdec.ToString();
                seowemp.from = sobjobjMExpense[0].from.ToString();
                seowemp.to = sobjobjMExpense[0].to.ToString();
                seowemp.ecfremark = sobjobjMExpense[0].ecfremark.ToString();
                seowemp.ecfraisergid = sobjobjMExpense[0].chkraiser_gid.ToString();
                seowemp.Suppliergid = sobjobjMExpense[0].Suppliergid.ToString();
                seowemp.Suppliercode = sobjobjMExpense[0].Suppliercode.ToString();
                seowemp.Suppliername = sobjobjMExpense[0].Suppliername.ToString();
                seowemp.CurrencyName = sobjobjMExpense[0].CurrencyName.ToString();
                seowemp.Exrate = sobjobjMExpense[0].Exrate.ToString();
                seowemp.Currencyamount = sobjobjMExpense[0].Currencyamount.ToString();
                seowemp.DocName = sobjobjMExpense[0].DocName.ToString();
                seowemp.CurrencyId = sobjobjMExpense[0].CurrencyId.ToString();
                string doc = sobjobjMExpense[0].DocName.ToString(); ;

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
                seowemp.Raiser_Modedata = new SelectList(objModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", seowemp.raisermodeId);
                seowemp.doctypedata = new SelectList(objModel.GetDoctype().ToList(), "DocId", "DocName", seowemp.DocId);
                seowemp.Currencydata = new SelectList(objModel.GetCurrency().ToList(), "CurrencyId", "CurrencyName", seowemp.CurrencyId);
                seowemp.ecf_GID = Convert.ToInt32(sobjobjMExpense[0].ecf_GID.ToString());
                seowemp.Queue_GID = Convert.ToInt32(sobjobjMExpense[0].Queue_GID.ToString());
                seowemp.ecf_Paymode = sobjobjMExpense[0].ecf_Paymode.ToString();
                seowemp.CygnetFlag = sobjobjMExpense[0].CygnetFlag.ToString();
                CygnetFlag = sobjobjMExpense[0].CygnetFlag.ToString();
                Session["CygnetFlag"] = sobjobjMExpense[0].CygnetFlag.ToString();
                Session["DashBoard"] = seowemp;
                redirectpage = Convert.ToInt32(confirmsupplier.ToString());
            }
            else
            {
                EOW_EmployeeeExpense eowemp = new EOW_EmployeeeExpense();
                List<EOW_EmployeeeExpense> objobjMExpense = new List<EOW_EmployeeeExpense>();
                confirmsupplier = objModel.selectsupplierinvoice(id.ToString(), "L").ToString();
                if (confirmsupplier == "6" || confirmsupplier == "7" || confirmsupplier=="12")
                {
                    objobjMExpense = objModel.SelectViewdata(id, "L").ToList();
                }
                if (confirmsupplier == "2" || confirmsupplier == "5")
                {
                    objobjMExpense = objModel.SelectViewdata(id, "L").ToList();
                }
                else if (confirmsupplier != "2" && confirmsupplier != "5" && confirmsupplier != "6" && confirmsupplier != "7" && confirmsupplier != "12")
                {
                    objobjMExpense = objModel.SelectViewdata(id, "D").ToList();
                }
                eowemp.ecfremark = string.IsNullOrEmpty(Convert.ToString(objobjMExpense[0].ecfremark)) ? "" : Convert.ToString(objobjMExpense[0].ecfremark);
                eowemp.ecfdelmatamt = Convert.ToString(objobjMExpense[0].ecfdelmatamt);
                eowemp.ECF_Amount = Convert.ToString(objobjMExpense[0].ECF_Amount);
                eowemp.ClaimMonth = locals.getconverttomonthtodateto(Convert.ToString(objobjMExpense[0].ClaimMonth));
                eowemp.ECF_Date = Convert.ToString(objobjMExpense[0].ECF_Date);
                eowemp.Grade = Convert.ToString(objobjMExpense[0].Grade);
                eowemp.noofperson = Convert.ToString(objobjMExpense[0].noofperson);
                eowemp.Raiser_Code = Convert.ToString(objobjMExpense[0].Raiser_Code);
                eowemp.Raiser_Name = Convert.ToString(objobjMExpense[0].Raiser_Name);
                eowemp.ecfdescription = Convert.ToString(objobjMExpense[0].ecfdescription);
                eowemp.raisermodeId = Convert.ToString(objobjMExpense[0].raisermodeId);
                eowemp.Raiser_Modedata = new SelectList(objModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", Convert.ToString(objobjMExpense[0].raisermodeId));
                eowemp.ecf_GID = Convert.ToInt32(Convert.ToString(objobjMExpense[0].ecf_GID));
                eowemp.invoice_GID = Convert.ToInt32(Convert.ToString(objobjMExpense[0].invoice_GID));
                eowemp.SubCategoryName = Convert.ToString(objobjMExpense[0].SubCategoryName);
                eowemp.Queue_GID = 0;
                eowemp.ecf_paymode = objobjMExpense[0].ecf_paymode.ToString();
                eowemp.CygnetFlag = objobjMExpense[0].CygnetFlag.ToString();
                Session["CygnetFlag"] = objobjMExpense[0].CygnetFlag.ToString();
                CygnetFlag = objobjMExpense[0].CygnetFlag.ToString();
                Session["DashBoard"] = eowemp;
                redirectpage = Convert.ToInt32(objobjMExpense[0].Doctypeid.ToString());
            }

            int regular = Convert.ToInt32(ConfigurationManager.AppSettings["EcfRegular"].ToString());
            int travel = Convert.ToInt32(ConfigurationManager.AppSettings["EcfTravel"].ToString());
            int LocalCoveyance = Convert.ToInt32(ConfigurationManager.AppSettings["EcfLocalCoveyance"].ToString());
            int SupplierInvoiceDSA = Convert.ToInt32(ConfigurationManager.AppSettings["EcfSupplierInvoiceDSA"].ToString());
            int SupplierInvoice = Convert.ToInt32(ConfigurationManager.AppSettings["EcfSupplierInvoice"].ToString());
            int EcfPettyCash = Convert.ToInt32(ConfigurationManager.AppSettings["EcfPettyCash"].ToString());
            int Insurance = 11;
            int Insadvance = 12;
            //int EcfARF = Convert.ToInt32(ConfigurationManager.AppSettings["EcfARF"].ToString());

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
                if (supplierreman == "M")
                {
                    return RedirectToAction("Index", "SupplierInvoiceNewmanual");
                }
                else
                {
                    if (CygnetFlag=="Y")
                    {
                        return RedirectToAction("Index", "SupplierInvoiceNew");
                    }
                    else
                    {
                        return RedirectToAction("SupplierIndex", "SupplierInvoiceNew");
                    }
                }

            }
            if (EcfPettyCash == redirectpage)
            {
                return RedirectToAction("Index", "EmployeeClaimNewPetty");
            }
            if (Insurance == redirectpage)
            {
                return RedirectToAction("Index", "Insurance");
            }

                if (Insadvance == redirectpage)
                {
                    return RedirectToAction("Index", "RaisingInsurance");
                }
                else
                {
                    return RedirectToAction("Index", "RaisingArf");
                }
            }
            catch(Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString() + " inner exception - " + ex.InnerException.ToString(), ex.ToString()); ;
            }
            return View();
        }
        public ActionResult Viewdata(string id)
        {
            Session["requesttype"] = "";
            string confirmsupplier = objModel.selectsupplierinvoice(id.ToString(), "A").ToString();
            string viewapp = objModel.selectsupplierinvoicestatus(id.ToString(), "A").ToString();
            string centralteam = objModel.selectsupplierinvoicestatuscen(id.ToString(), "A").ToString();
            string cemtm = objModel.selectcemtam(id.ToString(), centralteam);
            //if (viewapp == objCmnFunctions.GetLoginUserGid().ToString())
            //{
            //    Session["docAppoalc"] = "docAppoalc";
            //}
            //else
            //{
            //    Session["docAppoalc"] = "docApp";
            //}
            if (confirmsupplier == "4" || confirmsupplier == "11")
            {
                EOW_Supplierinvoice seowemp = new EOW_Supplierinvoice();
                List<EOW_Supplierinvoice> sobjobjMExpense = new List<EOW_Supplierinvoice>();
                if (centralteam == "C" || centralteam == "R")
                {
                    sobjobjMExpense = objModel.SelectViewdatasupplier(id, "checkview").ToList();
                }
                else
                {
                    sobjobjMExpense = objModel.SelectViewdatasupplier(id, "A").ToList();
                }

                seowemp.ecfno = sobjobjMExpense[0].ecfno.ToString();

                seowemp.ECF_Amount = objCmnFunctions.GetINRAmount(sobjobjMExpense[0].ECF_Amount.ToString());
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
                Session["chkraiser"] = sobjobjMExpense[0].chkraiser_gid.ToString();
                seowemp.Grade = sobjobjMExpense[0].Grade.ToString();
                seowemp.Raiser_Code = sobjobjMExpense[0].Raiser_Code.ToString();
                seowemp.Raiser_Name = sobjobjMExpense[0].Raiser_Name.ToString();
                seowemp.ecfdescription = sobjobjMExpense[0].ecfdescription.ToString();
                seowemp.Raiser_Modedata = new SelectList(objModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", sobjobjMExpense[0].raisermodeId.ToString());
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

                ViewBag.EOW_supplierExpenseheader = seowemp;
                ViewBag.doctype = objCmnFunctions.GetDocType(sobjobjMExpense[0].Doctypeid.ToString());
                Session["EcfType"] = objCmnFunctions.GetDocType(sobjobjMExpense[0].Doctypeid.ToString());
                Session["EcfGid"] = sobjobjMExpense[0].ecf_GID.ToString();
                Session["invoiceGid"] = "";
                Session["QueueGid"] = sobjobjMExpense[0].Queue_GID.ToString();
                
            	if(sobjobjMExpense[0].Doctypeid == 1 ||sobjobjMExpense[0].Doctypeid == 2 ||  sobjobjMExpense[0].Doctypeid ==3 ||sobjobjMExpense[0].Doctypeid ==8)
                {
                     Session["requesttype"] = "EMPLOYEE CLAIMS";
                }
                else if(sobjobjMExpense[0].Doctypeid == 6 ||sobjobjMExpense[0].Doctypeid == 7 ||  sobjobjMExpense[0].Doctypeid ==9)
                {
                     Session["requesttype"] = "ADVANCE REQUEST";
                }
                else if(sobjobjMExpense[0].Doctypeid == 4 ||sobjobjMExpense[0].Doctypeid == 5 ||  sobjobjMExpense[0].Doctypeid ==10)
                {
                     Session["requesttype"] = "SUPPLIER INVOICE";
                }
	 

                if (sobjobjMExpense[0].raisermodeId.ToString() == "C" || sobjobjMExpense[0].raisermodeId.ToString() == "R")
                {
                    if (cemtm == "18")
                    {
                        Session["docAppoalc"] = "checker";
                        ViewBag.ApproverAction = "CT";
                    }
                    else if (cemtm == "17")
                    {
                        Session["docAppoalc"] = "maker";
                        ViewBag.ApproverAction = "CTMKR";
                    }
                    else
                    {
                        if (viewapp == objCmnFunctions.GetLoginUserGid().ToString())
                        {
                            Session["docAppoalc"] = "docAppoalc";
                        }
                        else
                        {
                            Session["docAppoalc"] = "docApp";
                            ViewBag.ApproverAction = sobjobjMExpense[0].queueactionfor.ToString();
                        }
                    }
                }
                else
                {
                    if (viewapp == objCmnFunctions.GetLoginUserGid().ToString())
                    {
                        Session["docAppoalc"] = "docAppoalc";
                    }
                    else
                    {
                        Session["docAppoalc"] = "docApp";
                        ViewBag.ApproverAction = sobjobjMExpense[0].queueactionfor.ToString();
                    }
                }

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

                confirmsupplier = objModel.selectsupplierinvoice(id.ToString(), "A").ToString();
                if (confirmsupplier == "6" || confirmsupplier == "7" ||confirmsupplier == "12")
                {
                    ViewBag.frst = "Othersarf";
                    objobjMExpense = objModel.SelectViewdata(id, "AL").ToList();
                }
                if (confirmsupplier == "2" || confirmsupplier == "5")
                {
                    ViewBag.frst = "Others";
                    objobjMExpense = objModel.SelectViewdata(id, "AL").ToList();
                }
                else if (confirmsupplier != "2" && confirmsupplier != "5" && confirmsupplier != "6" && confirmsupplier != "7" && confirmsupplier != "12")
                {
                    ViewBag.frst = "Others";
                    objobjMExpense = objModel.SelectViewdata(id, "A").ToList();
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
                if (eowemp.SubCategoryName == "S" || eowemp.SubCategoryName == "I")
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
                if (objobjMExpense[0].Doctypeid == 1 || objobjMExpense[0].Doctypeid == 2 || objobjMExpense[0].Doctypeid == 3 || objobjMExpense[0].Doctypeid == 8)
                {
                    Session["requesttype"] = "EMPLOYEE CLAIMS";
                }
                else if (objobjMExpense[0].Doctypeid == 6 || objobjMExpense[0].Doctypeid == 7 || objobjMExpense[0].Doctypeid == 9)
                {
                    Session["requesttype"] = "ADVANCE REQUEST";
                }
                else if (objobjMExpense[0].Doctypeid == 4 || objobjMExpense[0].Doctypeid == 5 || objobjMExpense[0].Doctypeid == 10)
                {
                    Session["requesttype"] = "SUPPLIER INVOICE";
                }
                eowemp.Raiser_Modedata = new SelectList(objModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", objobjMExpense[0].raisermodeId.ToString());
                ViewBag.EOW_EmployeeeExpenseheader = eowemp;
                ViewBag.doctype = objCmnFunctions.GetDocType(objobjMExpense[0].Doctypeid.ToString());
                Session["EcfType"] = objCmnFunctions.GetDocType(objobjMExpense[0].Doctypeid.ToString());
                Session["EcfGid"] = objobjMExpense[0].ecf_GID.ToString();
                Session["invoiceGid"] = objobjMExpense[0].invoice_GID.ToString();
                Session["QueueGid"] = objobjMExpense[0].Queue_GID.ToString();

                if (viewapp == objCmnFunctions.GetLoginUserGid().ToString())
                {
                    Session["docAppoalc"] = "docAppoalc";
                }
                else
                {
                    Session["docAppoalc"] = "docApp";
                    ViewBag.ApproverAction = objobjMExpense[0].queueactionfor.ToString();
                }
            }
            Session["Ecfdeclaratonnote"] = objModel.GetDecnote(confirmsupplier.ToString(), "A").ToString();
            ViewBag.RequestType = Session["requesttype"].ToString();
            return View();
        }
        [HttpGet]
        public PartialViewResult _EmpAttachmentCreatev()
        {
            EOW_File objMAttachment = new EOW_File();
            objMAttachment.AttachmentTypeData = new SelectList(objModel.AttachmentTypedata().ToList(), "AttachmentTypeId", "AttachmentTypeName");
            return PartialView(objMAttachment);
        }

        //[HttpGet]
        //public PartialViewResult _LoadGstDetails()
        //{
        //    return PartialView();
        //}
        [HttpPost]
        public JsonResult EmpAttachmentDeletev(EOW_File EmployeeeExpensemodel)
        {
            int EmployeeePaymentGID = (int)EmployeeeExpensemodel.AttachmentFilenameId;
            string delamt = objModel.DeleteEmployeeeAttachment(EmployeeePaymentGID, Session["EcfGid"].ToString());
            //Session["Ecfamountpayment"] = Convert.ToInt32(Session["Ecfamountpayment"].ToString()) + Convert.ToInt32(delamt.ToString());
            return Json(EmployeeeExpensemodel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult _EmpAttachmentCreatev(EOW_File EmployeeeExpenseModel)
        {
            try
            {
                string msg = "2";
                if (TempData["_attFile"] != null)
                {
                    HttpPostedFileBase savefile = TempData["_attFile"] as HttpPostedFileBase;
                    string getname = objModel.InsertEmpAtt(savefile, EmployeeeExpenseModel, Session["EcfGid"].ToString(), objCmnFunctions.GetLoginUserGid().ToString());
                    if (getname != "")
                    {
                        //string path = "//192.168.0.203/temp/";
                        //string path = @"C:\temp\";
                        //string path1 = Path.GetFileName(savefile.FileName);
                        //string exten = Path.GetExtension(savefile.FileName);
                        //string[] str = path1.Split('.');
                        ////var index = path1.LastIndexOf(".") + 1;
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
                        //Session["empattmtfile"] = null;

                        //upload the file to server.
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
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult _PODetailsGridV(string values, string valuesid)
        {
            List<EOW_PO> lstnew = new List<EOW_PO>();
            //lstnew = objModel.Getpodetailsgrid(values, valuesid, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), "S").ToList();
            return PartialView(lstnew);
        }
        [HttpGet]
        public PartialViewResult _PomappingdetailV(string id)
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
        [HttpGet]
        public PartialViewResult _EmpAttachmentDetailsv()
        {
            List<EOW_File> lstAttachment = new List<EOW_File>();
            lstAttachment = objModel.GetEmployeeeAttachment(Session["EcfGid"].ToString(), objCmnFunctions.GetLoginUserGid().ToString()).ToList();
            return PartialView(lstAttachment);
        }
        [HttpGet]
        public PartialViewResult _EmpEmployeeDetailsv()
        {
            List<EOW_Employeelst> lstGetEmployee = new List<EOW_Employeelst>();
            lstGetEmployee = objModel.GetEmployeeelist(Session["EcfGid"].ToString()).ToList();
            return PartialView(lstGetEmployee);
        }
        [HttpGet]
        public PartialViewResult _TravelModeDetailsv()
        {
            List<EOW_TravelClaim> lstnew = new List<EOW_TravelClaim>();
            lstnew = objModel.GetTravelModedata(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), "T").ToList();

            return PartialView(lstnew);
        }
        [HttpGet]
        public PartialViewResult _OtherTravelExpenseDetailsv()
        {
            List<EOW_TravelClaim> lstnew = new List<EOW_TravelClaim>();
            lstnew = objModel.GetTravelModedata(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), "E").ToList();

            return PartialView(lstnew);
        }
        [HttpGet]
        public PartialViewResult myWorkList()
        {
            List<DashBoard> lstGetEmployee = new List<DashBoard>();
            lstGetEmployee = objModel.GetDashBoardDetails(objCmnFunctions.GetLoginUserGid().ToString()).ToList();
            return PartialView(lstGetEmployee);
        }
        public FileResult Download(int id)
        {
            string FileName = objModel.downloadAttachment(id, Session["EcfGid"].ToString());
            ////int index = delamt.LastIndexOf(".");
            ////string[] seqNum = new string[] { delamt.Substring(0, index), delamt.Substring(index + 1) };
           // //string directory = @"C:\temp\";
           // //directory = directory + id.ToString() + "." + seqNum[1].ToString();
           // //byte[] fileBytes = System.IO.File.ReadAllBytes(directory);
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
           // return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, FileName);
        }
        [HttpPost]
        public void UploadFiles(string attach = null, string attaching_format = null) //Pandiaraj 18-11-2019 
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
        public ActionResult Printdata(int id)
        {
            return View();
        }
        public JsonResult _Viewsubmit(Approveraction AppModel)
        {
            string Err = "";
            string delegates = "";
            try
            {
                if (ModelState.IsValid)
                {
                    Err = objModel.Insertapprovedaction(AppModel, Session["EcfGid"].ToString(), Convert.ToString(Session["invoiceGid"]), objCmnFunctions.GetLoginUserGid().ToString(), Session["EcfType"].ToString(), Session["QueueGid"].ToString(), Session["Ecfqueueamount"].ToString(), Err, delegates);
                }
                ViewBag.SearchItems = null;
                Session["DashSearchItems"] = null;
                Session["DashSearchItemsapp"] = null;
                return Json(Err, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult _Viewsubmitconreject(Approveraction AppModel)
        {
            string Err = "";
            try
            {
                if (ModelState.IsValid)
                {
                    if (AppModel.status == "Approve")
                    {
                        //string demlat = objModel.Getraiserdelmat(Session["EcfGid"].ToString(), objCmnFunctions.GetLoginUserGid().ToString());
                        //if (demlat == "Yes")
                        //{
                        objModel.raiseapprovedaction(AppModel, Session["EcfGid"].ToString(), Convert.ToString(Session["invoiceGid"]), objCmnFunctions.GetLoginUserGid().ToString(), Session["EcfType"].ToString(), Session["QueueGid"].ToString(), Session["Ecfqueueamount"].ToString(), "");
                        //}
                        //else
                        //{
                        //    Err = "Delmat";
                        //}
                    }
                    else
                    {
                        Err = objModel.centralapprovedaction(AppModel, Session["EcfGid"].ToString(), Convert.ToString(Session["invoiceGid"]), objCmnFunctions.GetLoginUserGid().ToString(), Session["EcfType"].ToString(), Session["QueueGid"].ToString(), Session["Ecfqueueamount"].ToString(), "");
                        //sub.Insertapprovedactionreject(AppModel, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), objCmnFunctions.GetLoginUserGid().ToString(), Session["EcfType"].ToString(), Session["QueueGid"].ToString(), Session["Ecfqueueamount"].ToString());
                        //objModel.Insertapprovedactionreject(AppModel, Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), objCmnFunctions.GetLoginUserGid().ToString(), Session["EcfType"].ToString(), Session["QueueGid"].ToString(), Session["Ecfqueueamount"].ToString());
                    }
                }
                if (Err == "")
                {
                    Err = "Success";
                }
                Session["DashSearchItems"] = null;
                Session["DashSearchItemsapp"] = null;
                ViewBag.SearchItems = null;
                return Json(Err, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult _Viewsubmitcon(Approveraction AppModel)
        {
            string Err = "";
            try
            {
                if (ModelState.IsValid)
                {
                    Err = objModel.Insertapprovedactioncon(AppModel, Session["EcfGid"].ToString(), Convert.ToString(Session["invoiceGid"]), objCmnFunctions.GetLoginUserGid().ToString(), Session["EcfType"].ToString(), Session["QueueGid"].ToString());
                }
                Err = "Success";
                Session["DashSearchItems"] = null;
                Session["DashSearchItemsapp"] = null;
                ViewBag.SearchItems = null;
                return Json(Err, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult Viewsubmitcon(Approveraction AppModel)
        {
            string Err = "";
            try
            {
                string reaiserchecker = Session["chkraiser"].ToString();
                Err = objModel.centralapprovedaction(AppModel, Session["EcfGid"].ToString(), Convert.ToString(Session["invoiceGid"]), objCmnFunctions.GetLoginUserGid().ToString(), Session["EcfType"].ToString(), Session["QueueGid"].ToString(), Session["Ecfqueueamount"].ToString(), Session["chkraiser"].ToString());
                 
                Session["DashSearchItems"] = null;
                Session["DashSearchItemsapp"] = null;

                ViewBag.SearchItems = null;
                return Json(Err, JsonRequestBehavior.AllowGet);
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
        public PartialViewResult _SupplierPaymentGrid()
        {
            List<EOW_Payment> lstPayment = new List<EOW_Payment>();
            lstPayment = objModel.GetEmployeeePayment(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString()).ToList();
            return PartialView(lstPayment);
        }
        [HttpGet]
        public PartialViewResult _SupplierTaxDetailsGrid()
        {
            List<sinvotax> lstnew = new List<sinvotax>();
            lstnew = objModel.GetSupplierTax(Session["invoiceGid"].ToString()).ToList();
            return PartialView(lstnew);
        }
        [HttpGet]
        public PartialViewResult _TabdebitlineDetails()
        {
            List<EOW_TravelClaim> lstnew = new List<EOW_TravelClaim>();
            lstnew = objModel.GetSuppliserDedit(Session["EcfGid"].ToString(), Session["invoiceGid"].ToString(), "S").ToList();

            return PartialView(lstnew);
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
            objMPaymentEdit.GstCharged = objobjMPayment[0].GstCharged.ToString();
            objMPaymentEdit.ProviderLocation = objobjMPayment[0].ProviderLocation.ToString();
            objMPaymentEdit.ReceiverLocation = objobjMPayment[0].ReceiverLocation.ToString();
            objMPaymentEdit.GstinVendor = objobjMPayment[0].GstinVendor.ToString();
            objMPaymentEdit.GstinFiccl = objobjMPayment[0].GstinFiccl.ToString();
            Session["editorview"] = "Edit";
            return Json(objobjMPayment, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public PartialViewResult searchEmployeedetails(EOW_Employeelst objownersearch)
        {
            List<EOW_Employeelst> objowner = new List<EOW_Employeelst>();
            objowner = objModel.getemployeedetails();
            if (objownersearch != null)
            {
                if ((string.IsNullOrEmpty(objownersearch.empCode)) == false)
                {
                    ViewBag.empcode = objownersearch.empCode;
                    objowner = objowner.Where(x => objownersearch.empCode == null ||
                        (x.empCode.Contains(objownersearch.empCode))).ToList();
                }
                if ((string.IsNullOrEmpty(objownersearch.empName)) == false)
                {
                    ViewBag.empname = objownersearch.empName;
                    objowner = objowner.Where(x => objownersearch.empName == null ||
                        (x.empName.Contains(objownersearch.empName))).ToList();
                }
            }
            return PartialView("_TravelsearchEmp", objowner);
            //return Json(objowner, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult _TravelsearchEmpv()
        {
            List<EOW_Employeelst> objowner = new List<EOW_Employeelst>();
            objowner = objModel.getemployeedetails();
            return PartialView(objowner);
        }
        [HttpGet]
        public PartialViewResult EmployeeSearch(string listfor)
        {
            List<CentraldataModel> obj = new List<CentraldataModel>();
            CentraldataModel searchcendatat = new CentraldataModel();
            if (listfor == "search")
            {
                if (Session["Searchcendatat"] != null)
                {
                    searchcendatat = (CentraldataModel)Session["SearchViewbagData"];
                    ViewBag.EmployeeCode = searchcendatat.RaiserCode;
                    ViewBag.EmployeeName = searchcendatat.RaiserName;
                    obj = (List<CentraldataModel>)Session["Searchcendatat"];
                }
            }
            else
            {
                obj = objModel.SelectEmployee().ToList();
            }
            return PartialView(obj);
        }
        [HttpPost]
        public JsonResult EmployeeSearchforinward(string RaiserName = "", string RaiserCode = "")
        {
            List<CentraldataModel> obj = new List<CentraldataModel>();
            CentraldataModel search = new CentraldataModel();
            if (RaiserCode != "" || RaiserName != "")
            {
                obj = objModel.SelectEmployeeSearch(RaiserName, RaiserCode).ToList();
                ViewBag.EmployeeCode = RaiserCode;
                ViewBag.EmployeeName = RaiserName;
                search.RaiserCode = RaiserCode;
                search.RaiserName = RaiserName;
                if (obj.Count == 0)
                {
                    ViewBag.Message = "No Record's Found !";
                }
                Session["Searchcendatat"] = obj;
                Session["SearchViewbagData"] = search;
            }
            else
            {
                Session["Searchcendatat"] = null;
                obj = objModel.SelectEmployee().ToList();
            }

            return Json(obj, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AutoCompletecode(string term)
        {
            string Err = "";
            List<CentraldataModel> objparenttax = new List<CentraldataModel>();
            objparenttax = objModel.SelectEmployeeSearchCode(term).ToList();
            return Json(objparenttax, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AutoCompletename(string term)
        {
            string Err = "";
            List<CentraldataModel> objparenttax = new List<CentraldataModel>();
            objparenttax = objModel.SelectEmployeeSearchName(term).ToList();
            return Json(objparenttax, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult myWorkListcentralteam()
        {
            List<CentraldataModel> obj = new List<CentraldataModel>();
            return PartialView(obj);
        }
        [HttpGet]
        public PartialViewResult myWorkListcentralteammkr()
        {
            List<CentraldataModel> obj = new List<CentraldataModel>();
            return PartialView(obj);
        }
       
        [HttpGet]
        public JsonResult ToGetMyRequest()
        {
            List<DashBoard> lstDashBoard = new List<DashBoard>();
            lstDashBoard = (List<DashBoard>)Session["ToGetMyRequest"];
            if (lstDashBoard == null)
            {
                lstDashBoard = objModel.GetDashBoardDetails(objCmnFunctions.GetLoginUserGid().ToString()).ToList();
                Session["ToGetMyRequest"] = lstDashBoard;
            }
            return Json(lstDashBoard, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ToGetMyRequestapp()
        {
            List<DashBoard> lstDashBoard = new List<DashBoard>();
            lstDashBoard = (List<DashBoard>)Session["ToGetMyRequestapp"];
            if (lstDashBoard == null)
            {
                lstDashBoard = objModel.GetDashBoardDetailsa(objCmnFunctions.GetLoginUserGid().ToString()).ToList();
                Session["ToGetMyRequestapp"] = lstDashBoard;
            }
            return Json(lstDashBoard, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ToGetCentralchkr()
        {
            List<CentraldataModel> lstDashBoard = new List<CentraldataModel>();
            lstDashBoard = (List<CentraldataModel>)Session["ToGetCentralchkr"];
            if (lstDashBoard == null)
            {
                lstDashBoard = objd.CentralCheckerDetails(objCmnFunctions.GetLoginUserGid().ToString()).ToList();
                Session["ToGetCentralchkr"] = lstDashBoard;
            }
            return Json(lstDashBoard, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ToGetCentralmkr()
        {
            List<CentraldataModel> lstDashBoard = new List<CentraldataModel>();
            lstDashBoard = (List<CentraldataModel>)Session["ToGetCentralmkr"];
            if (lstDashBoard == null)
            {
                lstDashBoard = objd.CentralRejectDetails().ToList();
                Session["ToGetCentralmkr"] = lstDashBoard;
            }
            return Json(lstDashBoard, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ToGetMyDashboarddtils()
        {
            ////---------------- MyRequest
            List<DashBoard> lstMyRequest = new List<DashBoard>();
            lstMyRequest = (List<DashBoard>)Session["ToGetMyRequest"];
            if (lstMyRequest == null)
            {
                lstMyRequest = objModel.GetDashBoardDetails(objCmnFunctions.GetLoginUserGid().ToString()).ToList();
                Session["ToGetMyRequest"] = lstMyRequest;
            }
            ////---------------- My Approval Request
            List<DashBoard> lstMyAppRequest = new List<DashBoard>();
            lstMyAppRequest = (List<DashBoard>)Session["ToGetMyRequestapp"];
            if (lstMyAppRequest == null)
            {
                lstMyAppRequest = objModel.GetDashBoardDetailsa(objCmnFunctions.GetLoginUserGid().ToString()).ToList();
                Session["ToGetMyRequestapp"] = lstMyAppRequest;
            }
            ////---------------- Central chkr
            List<CentraldataModel> lstCentralchkr = new List<CentraldataModel>();
            lstCentralchkr = (List<CentraldataModel>)Session["ToGetCentralchkr"];
            if (lstCentralchkr == null)
            {
                lstCentralchkr = objd.CentralCheckerDetails(objCmnFunctions.GetLoginUserGid().ToString()).ToList();
                Session["ToGetCentralchkr"] = lstCentralchkr;
            }
            ////---------------- Central mkar
            List<CentraldataModel> lstCentralmkar = new List<CentraldataModel>();
            lstCentralmkar = (List<CentraldataModel>)Session["ToGetCentralmkr"];
            if (lstCentralmkar == null)
            {
                lstCentralmkar = objd.CentralRejectDetails().ToList();
                Session["ToGetCentralmkr"] = lstCentralmkar;
            }
            ////---------------- ECF Hold List
            List<ECFDataModel> lstecfhold = new List<ECFDataModel>();
            lstecfhold = (List<ECFDataModel>)Session["ToGetMyRequesthold"];
            if (lstecfhold == null)
            {
                lstecfhold = ECFModelobj.HoldDetails().ToList();
                Session["ToGetMyRequesthold"] = lstecfhold;
            }
            var jsonResult = Json(new { lstMyRequesta = lstMyRequest, lstMyAppRequesta = lstMyAppRequest, lstCentralchkra = lstCentralchkr, lstCentralmkara = lstCentralmkar, lstecfholda = lstecfhold }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult ViewECFPrint()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string testsendmaildata()
        {
            string msg = "";
            try
            {
                ForMailController formailsend = new ForMailController();
                string[] toaddress = new string[3];

                toaddress[0] = "thirumalai@flexicodeindia.com";
                toaddress[1] = "kathir@flexicodeindia.com";
                toaddress[2] = "thirumalai@flexicodeindia.com";

                //msg = formailsend.sendusermailfs("FS", "Payment Advice Email Alert", "11", "S", toaddress, "0");
                msg = formailsend.sendusermailfs("FS", "Petty Cash Email Alert", "290", "S", toaddress, "0");
                //msg = formailsend.sendusermailfs("FS", "EFT Rejection Email Alert", "14", "S", toaddress, "0");
                return msg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public class GetAll
        {
            public int CustomerId { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Date { get; set; }

            public string QueueId { get; set; }
            public string ECFId { get; set; }
            public string ECFNo { get; set; }
            public string ECFDate { get; set; }
            public string SupplierorEmployee { get; set; }
            public string SupplierorEmployeename { get; set; }
            public string ECFRaiser { get; set; }
            public string DocSubTypeName { get; set; }
            public string ECFAmount { get; set; }
            public string ECFStatus { get; set; }
            public string Reason { get; set; }

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
                SqlConnection con = new SqlConnection();
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
                //SqlConnection con = new SqlConnection("Data Source=WIN-M0GE46V6843;Initial Catalog=iem_miga;User Id=sa;password=gnsa;");
                //con.Open();
                //var query = "select  ecf_no,CONVERT(varchar(50),ecf_date,105) as ecf_date,invoice_gid,invoice_no,invoice_amount,invoice_dedup_no from iem_trn_tinvoice as a inner join iem_trn_tecf as b on b.ecf_gid=a.invoice_ecf_gid ";
                var query = "SELECT t1.*,QUEUE_GID=(SELECT MAX(queue_gid) FROM iem_trn_tqueue WHERE queue_ref_gid=t1.ecf_gid and queue_ref_flag=1) FROM  ";
                query += " (SELECT e.ecf_printflag,e.ecf_gid,CONVERT(VARCHAR,e.ecf_date,105)AS ecf_date,e.ecf_supplier_employee,ecfst.ecfstatus_name,            ";
                query += " sh.supplierheader_name,+emp.employee_code+'-'+emp.employee_name AS 'Raiser',doc.doctype_code,docs.docsubtype_name,docs.docsubtype_code,  ";
                query += " ecf_no,e.ecf_raiser, ecf_no as invoice_no,           ";
                query += " e.ecf_create_mode,CONVERT(CHAR(4), e.ecf_claim_month, 100)  +'-' + CONVERT(CHAR(4),  ";
                query += " e.ecf_claim_month, 120)AS ecf_claim_month,            ";
                query += " e.ecf_amount,CONVERT(VARCHAR,e.ecf_despatch_date,105)AS ecf_despatch_date, ";
                query += " e.ecf_courier_name,e.ecf_awb_no,e.ecf_cancel_by,em.employee_name AS'EmployeeName', ";
                query += " CONVERT(VARCHAR,ecf_cancel_date,105)AS ecf_cancel_date     ";
                query += " FROM iem_trn_tecf AS e           ";
                query += " LEFT JOIN asms_tmp_tsupplierheader AS sh ON e.ecf_supplier_gid=sh.supplierheader_gid  ";
                query += " LEFT JOIN iem_mst_temployee AS emp ON emp.employee_gid=e.ecf_raiser AND emp.employee_isremoved='N'  ";
                query += " LEFT JOIN  iem_mst_tdoctype AS doc ON doc.doctype_gid=e.ecf_doctype_gid AND doc.doctype_isremoved='N' ";
                query += " LEFT JOIN iem_mst_tdocsubtype AS docs ON docs.docsubtype_gid=e.ecf_docsubtype_gid AND docs.docsubtype_isremoved='N' ";
                query += " LEFT JOIN iem_mst_tecfstatus AS ecfst ON ecfst.ecfstatus_status=e.ecf_status AND ecfst.ecfstatus_isremoved='N'  ";
                query += " LEFT JOIN iem_mst_temployee AS em ON em.employee_gid=e.ecf_employee_gid ";
                query += " WHERE e.ecf_isremoved='N' ";
                query += " AND e.ecf_no<>'' ";
                query += "  )t1            ";
                query += "  ORDER BY t1.ecf_gid DESC ";

                var querynew = "pr_iem_trn_ECFReport";
                SqlCommand com = new SqlCommand(querynew, con); //creating SqlCommand object  
                com.CommandType = CommandType.StoredProcedure;
                com.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "QUERYSEARCHBYSAM";
                com.ExecuteNonQuery();
                con.Close();
                SqlDataAdapter adptr = new SqlDataAdapter(com);
                adptr.Fill(dt);
                //for (int i = 0; i < dt.Rows.Count; i++)
                foreach (DataRow row in dt.Rows)
                {
                    GetAll GetAll = new GetAll();
                    //GetAll.ECFId = dt.Rows[i]["ecf_gid"].ToString();
                    //GetAll.Name = dt.Rows[i]["ecf_no"].ToString();
                    //GetAll.Address = dt.Rows[i]["ecf_date"].ToString();
                    //GetAll.Email = dt.Rows[i]["invoice_no"].ToString();
                    //GetAll.PhoneNumber = dt.Rows[i]["ecf_amount"].ToString();
                    //GetAll.Date = dt.Rows[i]["ecf_amount"].ToString();

                    if (row["QUEUE_GID"].ToString() != "")
                    {
                        GetAll.QueueId = Convert.ToString(row["QUEUE_GID"].ToString());
                    }

                    GetAll.ECFId = Convert.ToString(row["ecf_gid"].ToString());
                    GetAll.ECFNo = row["ecf_no"].ToString();
                    GetAll.ECFDate = row["ecf_date"].ToString();
                    GetAll.SupplierorEmployee = row["ecf_supplier_employee"].ToString();
                    if (GetAll.SupplierorEmployee == "E" || GetAll.SupplierorEmployee == "P")
                    {
                        GetAll.SupplierorEmployeename = row["EmployeeName"].ToString();
                        GetAll.ECFRaiser = row["Raiser"].ToString();
                    }
                    if (GetAll.SupplierorEmployee == "S")
                    {
                        GetAll.SupplierorEmployeename = row["supplierheader_name"].ToString();
                        GetAll.ECFRaiser = row["Raiser"].ToString();
                    }
                    //GetAll.DocTypeName = row["doctype_code"].ToString();
                    GetAll.DocSubTypeName = row["docsubtype_name"].ToString();
                    //GetAll.CreateMode = row["ecf_create_mode"].ToString();
                    //GetAll.ClaimMonth = row["ecf_claim_month"].ToString();
                    GetAll.ECFAmount = objCmnFunctions.GetINRAmount(row["ecf_amount"].ToString());
                    //GetAll.Despatchdate = row["ecf_despatch_date"].ToString();
                    //GetAll.CourierName = row["ecf_courier_name"].ToString();
                    GetAll.ECFNo = row["ecf_no"].ToString();
                    GetAll.ECFStatus = row["ecfstatus_name"].ToString();
                    //GetAll.CancelBy = row["ecf_cancel_by"].ToString();
                    //GetAll.CancelDate = row["ecf_cancel_date"].ToString();
                    if (row["ecf_printflag"].ToString() == "Y")
                    {
                        GetAll.Reason = "Yes";
                    }
                    else
                    {
                        GetAll.Reason = "No";
                    }
                    list.Add(GetAll);
                }
            }
            catch (Exception e)
            {
            }
            var jsonResult = Json(list, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = 2147483647;
            return jsonResult;
            //return Json(list, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Deletectdraftecf(EOW_EmployeeeExpense EmployeeeExpensemodel)
        {
            try
            {
                string ecfgids = EmployeeeExpensemodel.noofperson;
                string delamt = objModel.Deletectdraftecfs(ecfgids, objCmnFunctions.GetLoginUserGid().ToString());
                return Json(EmployeeeExpensemodel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred..." + ex.Message.ToString(), JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAutodisplayofNextItem()
        {
            string resultstatus = "";
            Int32 data = 0;
            List<CentraldataModel> obj = new List<CentraldataModel>();
            try
            {
                Session["RejectProcess"] = "";
                //List<CentraldataModel> obj = new List<CentraldataModel>();
                obj = objd.CentralCheckerDetails(objCmnFunctions.GetLoginUserGid().ToString()).ToList(); 
                if (obj.Count == 0)
                {
                    ViewBag.Message = "No Record Found";
                    resultstatus = "No Record";
                }
                else
                {
                    data = Convert.ToInt32(obj[0].Docnogid);
                    resultstatus = "success";
                }
            }
            catch (Exception ex)
            {
                resultstatus = ex.Message;
            }

            return Json(data, resultstatus, JsonRequestBehavior.AllowGet);
        }
    }
}