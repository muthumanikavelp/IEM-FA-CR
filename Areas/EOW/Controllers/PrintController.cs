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
using System.Collections;
using System.Configuration;
using IEM.Common;
using System.Drawing;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Xml;
using iTextSharp.text.xml;
using System.Web.UI;
using IEM.App_Start;
namespace IEM.Areas.EOW.Controllers
{
    [NoDirectAccess]
    public class PrintController : Controller
    {
        DataTable dt = new DataTable();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        CommonIUD objCommonIUD = new CommonIUD();
        EOW_DataModel EOWDataModel = new EOW_DataModel();
        private PrintRepository objModel;
        int id;
        string cmbid;
        string sup;
        string filename = "Documents";
        String result = String.Empty;
        List<string> list = new List<string>();
        public PrintController()
            : this(new PrintModel())
        {

        }
        public PrintController(PrintRepository objM)
        {
            objModel = objM;
        }

        public ActionResult Index(string ecfgid, string ecftype = "S")
        {
            Session["lstInvoicedetails"] = null;
            Session["lstdebitlinedeails"] = null;
            Session["lstpaymentdeails"] = null;

            Session["EcfGid"] = ecfgid;
            Session["EcfType"] = ecftype;
            printdatamodel objModeldata = new printdatamodel();
            objModeldata = objModel.SelectEmployeeSearch(ecfgid, ecftype);
            //objModeldata.EmployeeId = "110509";
            //objModeldata.EmployeeName = "Anitha";
            //objModeldata.Title = "Senior Manager-Payment  Processing";
            //objModeldata.Department = "Technology";
            //objModeldata.LocationCode = "9990_Central_Mh_Bom_Powai Office";

            byte[] byteArray = null;
            //string path = Server.MapPath("~/Content/images/image.png");
            byteArray = GetImage(objModeldata.Ecf_No);
            //objModeldata.Ecf_nobarcode = objbar.getBarcodeImage(objbar.generateBarcode(), "ECF1211150001");
            //byteArray = System.IO.File.ReadAllBytes(path);
            objModeldata.Ecf_nobarcode = byteArray;

            //objModeldata.Ecf_Gid = "32";
            //objModeldata.Ecf_Date = "12-11-2015";
            //objModeldata.Ecf_Amount = "20000";
            //objModeldata.Ecf_Amountinword = objCmnFunctions.ConvertNumbertoWords(Convert.ToInt32(objModeldata.Ecf_Amount));
            //objModeldata.Ecf_No = "ECF1212150001";
            //objModeldata.Ecf_Amort = "No";
            //objModeldata.Ecf_forex = "No";
            //objModeldata.Ecf_Utility = "No";
            //objModeldata.Ecf_PaymentAdjst = "No";
            //objModeldata.Ecf_dcsc = "Supplier Payment for RBC for Aug-15";
            //objModeldata.Invoice_type = "PO";
            //objModeldata.Expense_type = "Capex";
            //objModeldata.Invoice_powono = "AD/12040";
           printdatamodel objModeldatadecl = new printdatamodel();
           objModeldatadecl = objModel.ToGetDeclaration(ecfgid);
           objModeldata.dclnotesub = objModeldatadecl.dclnotesub;
           objModeldata.dclnoteapp = objModeldatadecl.dclnoteapp;
            ViewBag.Printheader = objModeldata;
           
            return View();
        }
        private byte[] GetImage(string valueToEncode)
        {
            byte[] image = new byte[0];
            Barcode128 code128 = new Barcode128();
            code128.CodeType = Barcode.CODE128;
            code128.ChecksumText = true;
            code128.GenerateChecksum = true;
            code128.StartStopText = true;
            code128.Code = valueToEncode;
            code128.X = 4;
            System.Drawing.Bitmap bm = new System.Drawing.Bitmap(code128.CreateDrawingImage(System.Drawing.Color.Black, System.Drawing.Color.White));
            MemoryStream stream = new MemoryStream();
            bm.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            image = stream.ToArray();
            return image;
        }

        public PartialViewResult Print()
        {
            return PartialView();
        }
        public ActionResult CTPrint()
        {
            return View();
        }
        public ActionResult CTPrintold()
        {
            return View();
        }
        [HttpGet]
        public JsonResult ToGetecfctprint()
        {
            ECFModel eowobjModel = new ECFModel();
            List<ECFDataModel> lstDashBoard = new List<ECFDataModel>();
            lstDashBoard = eowobjModel.CTPrintdata().ToList();
            return Json(lstDashBoard, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ToGetecfallprint()
        {
            ECFModel eowobjModel = new ECFModel();
            List<ECFDataModel> lstDashBoard = new List<ECFDataModel>();
            lstDashBoard = eowobjModel.AllPrintdata().ToList();
            var jsonResult = Json(new { qrylist = lstDashBoard }, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        public ActionResult PrintDemo()
        {
            //return new ActionAsPdf("Index", new { name = "Giorgio" }) { FileName = "Test.pdf" };
            return View();

        }
        public PartialViewResult Invoicedetails()
        {
            return PartialView();
        }
        public PartialViewResult Assetdetails()
        {
            return PartialView();
        }
        public PartialViewResult PaymentDetails()
        {
            return PartialView();
        }
        public ActionResult Employeeindex(string ecfgid, string ecftype = "S")
        {
            Session["lstInvoicedetails"] = null;
            Session["lstdebitlinedeails"] = null;
            Session["lstpaymentdeails"] = null;

            Session["EcfGid"] = ecfgid;
            Session["EcfType"] = ecftype;
            printdatamodel objModeldata = new printdatamodel();
            objModeldata = objModel.SelectPrintGeneral(ecfgid, ecftype);
            byte[] byteArray = null;
            byteArray = GetImage(objModeldata.Ecf_No);
            objModeldata.Ecf_nobarcode = byteArray;
            printdatamodel objModeldatadecl = new printdatamodel();
            objModeldatadecl = objModel.ToGetDeclaration(ecfgid);
            objModeldata.dclnotesub = objModeldatadecl.dclnotesub;
            objModeldata.dclnoteapp = objModeldatadecl.dclnoteapp;
            ViewBag.Printheader = objModeldata;
            return View();
        }
        public ActionResult Arfindex(string ecfgid, string ecftype = "S")
        {
            Session["SelectAdvance"] = null;
            Session["Selectsuplier"] = null;

            Session["EcfGid"] = ecfgid;
            Session["EcfType"] = ecftype;
            printdatamodel objModeldata = new printdatamodel();
            objModeldata = objModel.SelectPrintGeneral(ecfgid, ecftype);
            byte[] byteArray = null;
            byteArray = GetImage(objModeldata.Ecf_No);
            objModeldata.Ecf_nobarcode = byteArray;
            printdatamodel objModeldatadecl = new printdatamodel();
            objModeldatadecl = objModel.ToGetDeclaration(ecfgid);
            objModeldata.dclnotesub = objModeldatadecl.dclnotesub;
            objModeldata.dclnoteapp = objModeldatadecl.dclnoteapp;
            ViewBag.Printheader = objModeldata;
            return View();
        }
        public PartialViewResult AdvanceDetails()
        {
            return PartialView();
        }
        public PartialViewResult AdditionalInformation()
        {
            return PartialView();
        }
        [HttpGet]
        public JsonResult ToGetInvoicedetails()
        {
            List<EOW_SupplierModelgrid> lstInvoicedetails = new List<EOW_SupplierModelgrid>();
            lstInvoicedetails = (List<EOW_SupplierModelgrid>)Session["lstInvoicedetails"];
            if (lstInvoicedetails == null)
            {
                lstInvoicedetails = EOWDataModel.GetSupplieinvoice(Session["EcfGid"].ToString()).ToList();
                Session["lstInvoicedetails"] = lstInvoicedetails;
            }
            return Json(lstInvoicedetails, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ToGetdebitlinedeails()
        {
            List<EOW_TravelClaim> lstdebitlinedeails = new List<EOW_TravelClaim>();
            lstdebitlinedeails = (List<EOW_TravelClaim>)Session["lstdebitlinedeails"];
            if (lstdebitlinedeails == null)
            {
                lstdebitlinedeails = EOWDataModel.GetprintExpensesupplier(Session["EcfGid"].ToString()).ToList();
                Session["lstdebitlinedeails"] = lstdebitlinedeails;
            }
            return Json(lstdebitlinedeails, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ToGetpaymentdeails()
        {
            List<EOW_Payment> lstpaymentdeails = new List<EOW_Payment>();
            lstpaymentdeails = (List<EOW_Payment>)Session["lstpaymentdeails"];
            if (lstpaymentdeails == null)
            {
                lstpaymentdeails = EOWDataModel.GetprintPayment(Session["EcfGid"].ToString()).ToList();
                Session["lstpaymentdeails"] = lstpaymentdeails;
            }
            return Json(lstpaymentdeails, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ToGetarfindexdeails()
        {
            List<EOW_arfraising> SelectAdvance = new List<EOW_arfraising>();
            SelectAdvance = (List<EOW_arfraising>)Session["SelectAdvance"];
            if (SelectAdvance == null)
            {
                ArfRaising rais = new ArfRaising();
                SelectAdvance = rais.SelectAdvanceecf(Session["EcfGid"].ToString()).ToList();
                Session["SelectAdvance"] = SelectAdvance;
            }
            return Json(SelectAdvance, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ToGetarfempsupdeails()
        {
            List<EOW_arfraising> Selectsuplier = new List<EOW_arfraising>();
            Selectsuplier = (List<EOW_arfraising>)Session["Selectsuplier"];
            if (Selectsuplier == null)
            {
                ArfRaising rais = new ArfRaising();
                Selectsuplier = rais.SelectAdvanceprint(Session["EcfGid"].ToString()).ToList();
                Session["Selectsuplier"] = Selectsuplier;
            }
            return Json(Selectsuplier, JsonRequestBehavior.AllowGet);
        }
    }
}
