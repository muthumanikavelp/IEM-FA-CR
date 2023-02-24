using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using Excel;
using System.Collections;
using IEM.Areas.IFAMS.Models;
using IEM.Common;
using iTextSharp.text;
using System.Web.Hosting;
using iTextSharp.text.pdf;
using System.Web.UI.WebControls;
using IEM.App_Start;

namespace IEM.Areas.IFAMS.Controllers
{

    [NoDirectAccess]
    public class SaleInvoiceGenPrtController : Controller
    {
        //
        // GET: /IFAMS/SaleInvoiceGenPrt/
        private IRepository ifr;
        private CmnFunctions objCmnFunctions = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        public SaleInvoiceGenPrtController()
            : this(new DataModel()) { }

        public SaleInvoiceGenPrtController(IRepository objModel)
        {
            ifr = objModel;
        }
        public ActionResult SIMSummary()
        {
            SaleInvoice records = new SaleInvoice();
            try
            {
                records._SaleInvoice_list = ifr.getApprovedSaleDetails().ToList();
                if (records._SaleInvoice_list.Count == 0) { ViewBag.Message = "No records found"; }
                Session["SaleInvExceldownload"] = records._SaleInvoice_list;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return View(records);
        }
        [HttpPost]
        public ActionResult SIMSummary(string ids, string command, string fDate, string tDate, string SaleNo, string VendorName, string SaleAmount, string SaleDate)
        {
            SaleInvoice records = new SaleInvoice();
            records._SaleInvoice_list = ifr.getApprovedSaleDetails().ToList();
            try
            {
                if (command == "Search")
                {
                    if ((string.IsNullOrEmpty(fDate) || string.IsNullOrWhiteSpace(fDate)) &&
                        (string.IsNullOrEmpty(tDate) || string.IsNullOrWhiteSpace(tDate)) &&
                        (string.IsNullOrEmpty(SaleNo) || string.IsNullOrWhiteSpace(SaleNo)) &&
                        (string.IsNullOrEmpty(VendorName) || string.IsNullOrWhiteSpace(VendorName)) &&
                        (string.IsNullOrEmpty(SaleAmount) || string.IsNullOrWhiteSpace(SaleAmount)) &&
                        (string.IsNullOrEmpty(SaleDate) || string.IsNullOrWhiteSpace(SaleDate)))
                    {
                        records._SaleInvoice_list = ifr.getApprovedSaleDetails().ToList();
                    }
                    if (!string.IsNullOrEmpty(SaleNo))
                    {
                        ViewBag.SaleNo = SaleNo;
                        records._SaleInvoice_list = records._SaleInvoice_list.Where(x => SaleNo.ToUpper() == null || (x._sale_no.Contains(SaleNo.ToUpper()))).ToList();
                    }
                    if (!string.IsNullOrEmpty(VendorName))
                    {
                        ViewBag.VendorName = VendorName;
                        records._SaleInvoice_list = records._SaleInvoice_list.Where(x => VendorName == null || (x._vendor_name.ToUpper().Contains(VendorName.ToUpper()))).ToList();
                    }
                    if (!string.IsNullOrEmpty(SaleAmount))
                    {
                        ViewBag.SaleAmount = SaleAmount;
                        records._SaleInvoice_list = records._SaleInvoice_list.Where(x => SaleAmount == null || (x._sale_amt.ToString().ToUpper().Equals(SaleAmount.ToUpper()))).ToList();
                    }
                    if (!string.IsNullOrEmpty(SaleDate))
                    {
                        ViewBag.SaleDate = SaleDate;
                        records._SaleInvoice_list = records._SaleInvoice_list.Where(x => SaleDate == null || (x._sale_date.ToString().ToUpper().Contains(SaleDate.ToUpper()))).ToList();
                    }
                    if (records._SaleInvoice_list.Count == 0) { ViewBag.Message = "No records found"; }
                    Session["SaleInvExceldownload"] = records._SaleInvoice_list;
                }
                if (command == "gen")
                {

                    if (ids != null && ids != "")
                    {
                        Invoice(ids);
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return View(records);
        }
        //public JsonResult Invoice(string ids)
        //{
        //    try
        //    {
        //        if (ids != null && ids != "")
        //        {
        //            ifr.GenerateInvoicePDF(ids);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //    }
        //    return Json("1", JsonRequestBehavior.AllowGet);
        //}
        public void Invoice(string ids)
        {
            try
            {

                SaleInvoice ObjInvoice = new SaleInvoice();
                //ifr.GenerateInvoicePDF(ids);

                ObjInvoice._SaleInvoice_list = ifr.getSaleDetails(ids);
                ObjInvoice._SaleInvoice_details_list = ifr.getApprovedSale(ids);
                ObjInvoice._FICC_details = ifr.getFICCdetails(ids);
                ObjInvoice._InvTAX_details = ifr.GetTaxdetails(ids);
                ObjInvoice._CessTAX_details = ifr.GetCessTaxdetails(ids);

                iTextSharp.text.Font fontTitle = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle1 = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle11 = FontFactory.GetFont("Calibri", 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle2 = FontFactory.GetFont("Calibri", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle3 = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitlenote = FontFactory.GetFont("Arial", 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                Font font8 = FontFactory.GetFont("ARIAL", 7);

                using (MemoryStream ms = new MemoryStream())
                {
                    Document document = new Document(PageSize.A4, 10F, 10F, 10F, 10F);
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);
                    document.Open();

                    document.NewPage();
                    PdfPTable HeaderTable = new PdfPTable(10);
                    HeaderTable.WidthPercentage = 100;

                    PdfPTable ObjInvoiceDetails = new PdfPTable(4);
                    ObjInvoiceDetails.WidthPercentage = 100;
                    int[] ObjInvoiceDetailsCellWidth = { 15, 30, 15, 40 };
                    ObjInvoiceDetails.SetWidths(ObjInvoiceDetailsCellWidth);

                    ObjInvoiceDetails.AddCell(new PdfPCell(new Phrase("Invoice To", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails.AddCell(new PdfPCell(new Phrase(ObjInvoice._SaleInvoice_list[0]._vendor_name, fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });

                    ObjInvoiceDetails.AddCell(new PdfPCell(new Phrase("Invoice Number", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails.AddCell(new PdfPCell(new Phrase(System.Configuration.ConfigurationManager.AppSettings["PRE_INV"].ToString() +'/'+ ObjInvoice._SaleInvoice_list[0]._invoice_no, fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });

                    ObjInvoiceDetails.AddCell(new PdfPCell(new Phrase("", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails.AddCell(new PdfPCell(new Phrase(ObjInvoice._SaleInvoice_list[0]._customer_address + ',' + ObjInvoice._SaleInvoice_list[0].customer_city + ',' + ObjInvoice._SaleInvoice_list[0].customer_district + ',' + ObjInvoice._SaleInvoice_list[0].customer_pincode, fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                   
                    ObjInvoiceDetails.AddCell(new PdfPCell(new Phrase("Invoice Date", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails.AddCell(new PdfPCell(new Phrase(DateTime.Now.ToShortDateString(), fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });

                    ObjInvoiceDetails.AddCell(new PdfPCell(new Phrase("GSTIN :", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails.AddCell(new PdfPCell(new Phrase(ObjInvoice._SaleInvoice_list[0]._Gstin_number, fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });

                    //ObjInvoiceDetails.AddCell(new PdfPCell(new Phrase("Branch Code", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    //ObjInvoiceDetails.AddCell(new PdfPCell(new Phrase("314", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails.AddCell(new PdfPCell(new Phrase("State Code :", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails.AddCell(new PdfPCell(new Phrase(ObjInvoice._SaleInvoice_list[0]._state_code, fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });

                    ObjInvoiceDetails.AddCell(new PdfPCell(new Phrase("Place of Supply :", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails.AddCell(new PdfPCell(new Phrase(ObjInvoice._FICC_details[0]._supplier_statename, fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });


                    //ObjInvoiceDetails.AddCell(new PdfPCell(new Phrase("Branch State", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    //ObjInvoiceDetails.AddCell(new PdfPCell(new Phrase("MADHYA PRADESH", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails.AddCell(new PdfPCell(new Phrase("State Name :", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails.AddCell(new PdfPCell(new Phrase(ObjInvoice._SaleInvoice_list[0]._state_name, fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });


                    //SaleInv Details
                    PdfPTable ObjInvoiceDetailstable = new PdfPTable(6);
                    ObjInvoiceDetailstable.WidthPercentage = 100;
                    int[] ObjInvoiceDetailstableCellWidth = { 5, 10, 35, 6, 5, 10};
                    ObjInvoiceDetailstable.SetWidths(ObjInvoiceDetailstableCellWidth);

                    PdfPCell SaleInvDetailsCell = null;
                    ObjInvoiceDetailstable.AddCell(new PdfPCell(new Phrase("S.No", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    ObjInvoiceDetailstable.AddCell(new PdfPCell(new Phrase("Asset Code", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    ObjInvoiceDetailstable.AddCell(new PdfPCell(new Phrase("Asset Description", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    ObjInvoiceDetailstable.AddCell(new PdfPCell(new Phrase("Quantity", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    ObjInvoiceDetailstable.AddCell(new PdfPCell(new Phrase("UOM", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    //ObjInvoiceDetailstable.AddCell(new PdfPCell(new Phrase("Sale Amount", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                   // ObjInvoiceDetailstable.AddCell(new PdfPCell(new Phrase("Vat Rate", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                   // ObjInvoiceDetailstable.AddCell(new PdfPCell(new Phrase("VAT Amount", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    //ObjInvoiceDetailstable.AddCell(new PdfPCell(new Phrase("GST Tax Rate", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    //ObjInvoiceDetailstable.AddCell(new PdfPCell(new Phrase("GST Tax Amount", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    ObjInvoiceDetailstable.AddCell(new PdfPCell(new Phrase("Total Amount", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    decimal total = 0;
                    decimal saleamt = 0;
                    decimal vatamt = 0;
                    for (int rows = 0; rows < ObjInvoice._SaleInvoice_details_list.Count; rows++)
                    {
                        SaleInvDetailsCell = new PdfPCell(new Phrase(new Chunk((rows + 1).ToString(), font8)));
                        ObjInvoiceDetailstable.AddCell(SaleInvDetailsCell);
                        SaleInvDetailsCell = new PdfPCell(new Phrase(new Chunk(ObjInvoice._SaleInvoice_details_list[rows]._code.ToString(), font8)));
                        ObjInvoiceDetailstable.AddCell(SaleInvDetailsCell);
                        SaleInvDetailsCell = new PdfPCell(new Phrase(new Chunk(ObjInvoice._SaleInvoice_details_list[rows]._particulars.ToString(), font8)));
                        ObjInvoiceDetailstable.AddCell(SaleInvDetailsCell);
                        SaleInvDetailsCell = new PdfPCell(new Phrase(new Chunk(ObjInvoice._SaleInvoice_details_list[rows]._qty.ToString(), font8)));
                        ObjInvoiceDetailstable.AddCell(SaleInvDetailsCell);
                        SaleInvDetailsCell = new PdfPCell(new Phrase(new Chunk("NO.", font8)));
                        ObjInvoiceDetailstable.AddCell(SaleInvDetailsCell);
                       // SaleInvDetailsCell = new PdfPCell(new Phrase(new Chunk("INR " + ObjInvoice._SaleInvoice_details_list[rows]._sale_amt.ToString(), font8)));
                      //  saleamt = saleamt + Convert.ToDecimal(ObjInvoice._SaleInvoice_details_list[rows]._sale_amt);
                      //  ObjInvoiceDetailstable.AddCell(SaleInvDetailsCell);
                        //SaleInvDetailsCell = new PdfPCell(new Phrase(new Chunk(ObjInvoice._SaleInvoice_details_list[rows]._vatpercentage.ToString() + "%", font8)));
                        //ObjInvoiceDetailstable.AddCell(SaleInvDetailsCell);
                        //SaleInvDetailsCell = new PdfPCell(new Phrase(new Chunk("INR" + ObjInvoice._SaleInvoice_details_list[rows]._vat_amt.ToString(), font8)));
                        //vatamt = vatamt + Convert.ToDecimal(ObjInvoice._SaleInvoice_details_list[rows]._vat_amt);
                        //ObjInvoiceDetailstable.AddCell(SaleInvDetailsCell);
                        SaleInvDetailsCell = new PdfPCell(new Phrase(new Chunk("INR " + ObjInvoice._SaleInvoice_details_list[rows].total.ToString(), font8)));
                        ObjInvoiceDetailstable.AddCell(SaleInvDetailsCell);
                        total = total + Convert.ToDecimal(ObjInvoice._SaleInvoice_details_list[rows].total);
                    }
                    SaleInvDetailsCell = new PdfPCell(new Phrase(new Chunk("", font8)));
                    ObjInvoiceDetailstable.AddCell(SaleInvDetailsCell);
                    SaleInvDetailsCell = new PdfPCell(new Phrase(new Chunk("", font8)));
                    ObjInvoiceDetailstable.AddCell(SaleInvDetailsCell);
                    SaleInvDetailsCell = new PdfPCell(new Phrase(new Chunk("Total", fontTitle11)));
                    ObjInvoiceDetailstable.AddCell(SaleInvDetailsCell);
                    SaleInvDetailsCell = new PdfPCell(new Phrase(new Chunk("", font8)));
                    ObjInvoiceDetailstable.AddCell(SaleInvDetailsCell);
                    SaleInvDetailsCell = new PdfPCell(new Phrase(new Chunk("", font8)));
                    ObjInvoiceDetailstable.AddCell(SaleInvDetailsCell);
                    //SaleInvDetailsCell = new PdfPCell(new Phrase(new Chunk("INR" + saleamt.ToString(), fontTitle11)));
                    //ObjInvoiceDetailstable.AddCell(SaleInvDetailsCell);
                    //SaleInvDetailsCell = new PdfPCell(new Phrase(new Chunk("INR" + vatamt.ToString(), fontTitle11)));
                    //ObjInvoiceDetailstable.AddCell(SaleInvDetailsCell);
                    SaleInvDetailsCell = new PdfPCell(new Phrase(new Chunk("INR" + total.ToString(), fontTitle11)));
                    ObjInvoiceDetailstable.AddCell(SaleInvDetailsCell);


                    //GST GROUPING
                    PdfPTable ObjInvoiceDetailsgsttable = new PdfPTable(12);
                    ObjInvoiceDetailsgsttable.WidthPercentage = 100;
                    int[] ObjInvoiceDetailsgsttableCellWidth = { 5,10,6, 6, 6, 6, 6, 6, 6, 6, 6, 10 };
                    ObjInvoiceDetailsgsttable.SetWidths(ObjInvoiceDetailsgsttableCellWidth);

                    PdfPCell SaleInvDetailsgstCell = null;
                    ObjInvoiceDetailsgsttable.AddCell(new PdfPCell(new Phrase("S.No", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    ObjInvoiceDetailsgsttable.AddCell(new PdfPCell(new Phrase("HSN Code", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    ObjInvoiceDetailsgsttable.AddCell(new PdfPCell(new Phrase("Taxable Amount", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    ObjInvoiceDetailsgsttable.AddCell(new PdfPCell(new Phrase("CGST Amt", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    ObjInvoiceDetailsgsttable.AddCell(new PdfPCell(new Phrase("CGST Rate", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    ObjInvoiceDetailsgsttable.AddCell(new PdfPCell(new Phrase("SGST Amt", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    ObjInvoiceDetailsgsttable.AddCell(new PdfPCell(new Phrase("SGST Rate", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    ObjInvoiceDetailsgsttable.AddCell(new PdfPCell(new Phrase("IGST Amt", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    ObjInvoiceDetailsgsttable.AddCell(new PdfPCell(new Phrase("IGST Rate", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    ObjInvoiceDetailsgsttable.AddCell(new PdfPCell(new Phrase("Cess Amt", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    ObjInvoiceDetailsgsttable.AddCell(new PdfPCell(new Phrase("Cess Rate", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    ObjInvoiceDetailsgsttable.AddCell(new PdfPCell(new Phrase("Total Amount", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    decimal totalTax = 0;
                    if (ObjInvoice._InvTAX_details.Count > 0) {
                        for (int gst = 0; gst < ObjInvoice._InvTAX_details.Count; gst++)
                    {
                        SaleInvDetailsgstCell = new PdfPCell(new Phrase(new Chunk((gst + 1).ToString(), font8)));
                        ObjInvoiceDetailsgsttable.AddCell(SaleInvDetailsgstCell);
                        SaleInvDetailsgstCell = new PdfPCell(new Phrase(new Chunk(ObjInvoice._InvTAX_details[gst]._hsn_code.ToString(), font8)));
                        ObjInvoiceDetailsgsttable.AddCell(SaleInvDetailsgstCell);
                        SaleInvDetailsgstCell = new PdfPCell(new Phrase(new Chunk(ObjInvoice._InvTAX_details[gst].tot_taxableamount.ToString(), font8)));
                        ObjInvoiceDetailsgsttable.AddCell(SaleInvDetailsgstCell);
                        SaleInvDetailsgstCell = new PdfPCell(new Phrase(new Chunk(ObjInvoice._InvTAX_details[gst]._CGstamt.ToString(), font8)));
                        ObjInvoiceDetailsgsttable.AddCell(SaleInvDetailsgstCell);
                        SaleInvDetailsgstCell = new PdfPCell(new Phrase(new Chunk(ObjInvoice._InvTAX_details[gst]._CGstrat.ToString(), font8)));
                        ObjInvoiceDetailsgsttable.AddCell(SaleInvDetailsgstCell);
                        SaleInvDetailsgstCell = new PdfPCell(new Phrase(new Chunk(ObjInvoice._InvTAX_details[gst]._SGstamt.ToString(), font8)));
                        ObjInvoiceDetailsgsttable.AddCell(SaleInvDetailsgstCell);
                        SaleInvDetailsgstCell = new PdfPCell(new Phrase(new Chunk(ObjInvoice._InvTAX_details[gst]._SGstrat.ToString(), font8)));
                        ObjInvoiceDetailsgsttable.AddCell(SaleInvDetailsgstCell);
                        SaleInvDetailsgstCell = new PdfPCell(new Phrase(new Chunk(ObjInvoice._InvTAX_details[gst]._IGstamt.ToString(), font8)));
                        ObjInvoiceDetailsgsttable.AddCell(SaleInvDetailsgstCell);
                        SaleInvDetailsgstCell = new PdfPCell(new Phrase(new Chunk(ObjInvoice._InvTAX_details[gst]._IGstrat.ToString(), font8)));
                        ObjInvoiceDetailsgsttable.AddCell(SaleInvDetailsgstCell);
                        SaleInvDetailsgstCell = new PdfPCell(new Phrase(new Chunk(ObjInvoice._InvTAX_details[gst]._Cessamt.ToString(), font8)));
                        ObjInvoiceDetailsgsttable.AddCell(SaleInvDetailsgstCell);
                        SaleInvDetailsgstCell = new PdfPCell(new Phrase(new Chunk(ObjInvoice._InvTAX_details[gst]._Cessrat.ToString(), font8)));
                        ObjInvoiceDetailsgsttable.AddCell(SaleInvDetailsgstCell);
                        SaleInvDetailsgstCell = new PdfPCell(new Phrase(new Chunk("INR " + ObjInvoice._InvTAX_details[gst]._taxTotal.ToString(), font8)));
                        ObjInvoiceDetailsgsttable.AddCell(SaleInvDetailsgstCell);
                        totalTax = totalTax + Convert.ToDecimal(ObjInvoice._InvTAX_details[gst]._taxTotal);
                    }

                    SaleInvDetailsgstCell = new PdfPCell(new Phrase(new Chunk("", font8)));
                    ObjInvoiceDetailsgsttable.AddCell(SaleInvDetailsgstCell);
                    SaleInvDetailsgstCell = new PdfPCell(new Phrase(new Chunk("", font8)));
                    ObjInvoiceDetailsgsttable.AddCell(SaleInvDetailsgstCell);
                    SaleInvDetailsgstCell = new PdfPCell(new Phrase(new Chunk("", font8)));
                    ObjInvoiceDetailsgsttable.AddCell(SaleInvDetailsgstCell);
                    SaleInvDetailsgstCell = new PdfPCell(new Phrase(new Chunk("", font8)));
                    ObjInvoiceDetailsgsttable.AddCell(SaleInvDetailsgstCell);
                    SaleInvDetailsgstCell = new PdfPCell(new Phrase(new Chunk("Total TAX", fontTitle11)));
                    ObjInvoiceDetailsgsttable.AddCell(SaleInvDetailsgstCell);
                    SaleInvDetailsgstCell = new PdfPCell(new Phrase(new Chunk("", font8)));
                    ObjInvoiceDetailsgsttable.AddCell(SaleInvDetailsgstCell);
                    SaleInvDetailsgstCell = new PdfPCell(new Phrase(new Chunk("", font8)));
                    ObjInvoiceDetailsgsttable.AddCell(SaleInvDetailsgstCell);
                    SaleInvDetailsgstCell = new PdfPCell(new Phrase(new Chunk("", font8)));
                    ObjInvoiceDetailsgsttable.AddCell(SaleInvDetailsgstCell);
                    SaleInvDetailsgstCell = new PdfPCell(new Phrase(new Chunk("", font8)));
                    ObjInvoiceDetailsgsttable.AddCell(SaleInvDetailsgstCell);
                    SaleInvDetailsgstCell = new PdfPCell(new Phrase(new Chunk("", font8)));
                    ObjInvoiceDetailsgsttable.AddCell(SaleInvDetailsgstCell);
                    SaleInvDetailsgstCell = new PdfPCell(new Phrase(new Chunk("INR" + totalTax.ToString(), fontTitle11)));
                    ObjInvoiceDetailsgsttable.AddCell(SaleInvDetailsgstCell);
                    }

                   // if (ObjInvoice._CessTAX_details.Count > 0)
                   // {
                        //CESSGST GROUPING
                    /* BHARATHI
                        PdfPTable ObjInvoiceDetailscessgsttable = new PdfPTable(9);
                        ObjInvoiceDetailscessgsttable.WidthPercentage = 100;
                        int[] ObjInvoiceDetailscessgsttableCellWidth = { 5, 10, 6, 6, 6, 6, 6, 6, 10 };
                        ObjInvoiceDetailscessgsttable.SetWidths(ObjInvoiceDetailscessgsttableCellWidth);

                        PdfPCell SaleInvDetailscessgstCell = null;
                        ObjInvoiceDetailscessgsttable.AddCell(new PdfPCell(new Phrase("S.No", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        ObjInvoiceDetailscessgsttable.AddCell(new PdfPCell(new Phrase("HSN Code", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        ObjInvoiceDetailscessgsttable.AddCell(new PdfPCell(new Phrase("CessCGST Amt", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        ObjInvoiceDetailscessgsttable.AddCell(new PdfPCell(new Phrase("CessCGST Rate", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        ObjInvoiceDetailscessgsttable.AddCell(new PdfPCell(new Phrase("CessSGST Amt", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        ObjInvoiceDetailscessgsttable.AddCell(new PdfPCell(new Phrase("CessSGST Rate", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        ObjInvoiceDetailscessgsttable.AddCell(new PdfPCell(new Phrase("CessIGST Amt", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        ObjInvoiceDetailscessgsttable.AddCell(new PdfPCell(new Phrase("CessIGST Rate", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        ObjInvoiceDetailscessgsttable.AddCell(new PdfPCell(new Phrase("Total Amount", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                        decimal totalcessTax = 0;
                        if (ObjInvoice._CessTAX_details.Count > 0)
                        {
                            for (int gst = 0; gst < ObjInvoice._InvTAX_details.Count; gst++)
                            {
                                SaleInvDetailscessgstCell = new PdfPCell(new Phrase(new Chunk((gst + 1).ToString(), font8)));
                                ObjInvoiceDetailscessgsttable.AddCell(SaleInvDetailscessgstCell);
                                SaleInvDetailscessgstCell = new PdfPCell(new Phrase(new Chunk(ObjInvoice._CessTAX_details[gst]._hsn_code.ToString(), font8)));
                                ObjInvoiceDetailscessgsttable.AddCell(SaleInvDetailscessgstCell);
                                SaleInvDetailscessgstCell = new PdfPCell(new Phrase(new Chunk(ObjInvoice._CessTAX_details[gst]._Ccessamt.ToString(), font8)));
                                ObjInvoiceDetailscessgsttable.AddCell(SaleInvDetailscessgstCell);
                                SaleInvDetailscessgstCell = new PdfPCell(new Phrase(new Chunk(ObjInvoice._CessTAX_details[gst]._Ccessrat.ToString(), font8)));
                                ObjInvoiceDetailscessgsttable.AddCell(SaleInvDetailscessgstCell);
                                SaleInvDetailscessgstCell = new PdfPCell(new Phrase(new Chunk(ObjInvoice._CessTAX_details[gst]._Scessamt.ToString(), font8)));
                                ObjInvoiceDetailscessgsttable.AddCell(SaleInvDetailscessgstCell);
                                SaleInvDetailscessgstCell = new PdfPCell(new Phrase(new Chunk(ObjInvoice._CessTAX_details[gst]._Scessrat.ToString(), font8)));
                                ObjInvoiceDetailscessgsttable.AddCell(SaleInvDetailscessgstCell);
                                SaleInvDetailscessgstCell = new PdfPCell(new Phrase(new Chunk(ObjInvoice._CessTAX_details[gst]._Icessamt.ToString(), font8)));
                                ObjInvoiceDetailscessgsttable.AddCell(SaleInvDetailscessgstCell);
                                SaleInvDetailscessgstCell = new PdfPCell(new Phrase(new Chunk(ObjInvoice._CessTAX_details[gst]._Icessrat.ToString(), font8)));
                                ObjInvoiceDetailscessgsttable.AddCell(SaleInvDetailscessgstCell);
                                //SaleInvDetailscessgstCell = new PdfPCell(new Phrase(new Chunk(ObjInvoice._InvTAX_details[gst]._cessTotal.ToString(), font8)));
                                //ObjInvoiceDetailscessgsttable.AddCell(SaleInvDetailsgstCell);
                                //SaleInvDetailscessgstCell = new PdfPCell(new Phrase(new Chunk(ObjInvoice._InvTAX_details[gst]._totalCessrate.ToString(), font8)));
                               // ObjInvoiceDetailscessgsttable.AddCell(SaleInvDetailsgstCell);
                                SaleInvDetailscessgstCell = new PdfPCell(new Phrase(new Chunk("INR " + ObjInvoice._CessTAX_details[gst]._cessTotal.ToString(), font8)));
                                ObjInvoiceDetailscessgsttable.AddCell(SaleInvDetailscessgstCell);
                                totalcessTax = totalcessTax + Convert.ToDecimal(ObjInvoice._CessTAX_details[gst]._taxTotal);
                            }

                            SaleInvDetailscessgstCell = new PdfPCell(new Phrase(new Chunk("", font8)));
                            ObjInvoiceDetailscessgsttable.AddCell(SaleInvDetailscessgstCell);
                            SaleInvDetailscessgstCell = new PdfPCell(new Phrase(new Chunk("", font8)));
                            ObjInvoiceDetailscessgsttable.AddCell(SaleInvDetailscessgstCell);
                            SaleInvDetailscessgstCell = new PdfPCell(new Phrase(new Chunk("", font8)));
                            ObjInvoiceDetailscessgsttable.AddCell(SaleInvDetailscessgstCell);
                            SaleInvDetailscessgstCell = new PdfPCell(new Phrase(new Chunk("", font8)));
                            ObjInvoiceDetailscessgsttable.AddCell(SaleInvDetailscessgstCell);
                            SaleInvDetailscessgstCell = new PdfPCell(new Phrase(new Chunk("Total CESSTAX", fontTitle11)));
                            ObjInvoiceDetailscessgsttable.AddCell(SaleInvDetailscessgstCell);
                            SaleInvDetailscessgstCell = new PdfPCell(new Phrase(new Chunk("", font8)));
                            ObjInvoiceDetailscessgsttable.AddCell(SaleInvDetailscessgstCell);
                            SaleInvDetailscessgstCell = new PdfPCell(new Phrase(new Chunk("", font8)));
                            ObjInvoiceDetailscessgsttable.AddCell(SaleInvDetailscessgstCell);
                            SaleInvDetailscessgstCell = new PdfPCell(new Phrase(new Chunk("", font8)));
                            ObjInvoiceDetailscessgsttable.AddCell(SaleInvDetailscessgstCell);
                            SaleInvDetailscessgstCell = new PdfPCell(new Phrase(new Chunk("INR" + totalcessTax.ToString(), fontTitle11)));
                            ObjInvoiceDetailscessgsttable.AddCell(SaleInvDetailscessgstCell);
                        }

                  //  }
                    */
                    //Add Mid Content

                    PdfPTable invoicetbl = new PdfPTable(1);
                    invoicetbl.WidthPercentage = 100;
                    invoicetbl.DefaultCell.Border = Rectangle.BOX;
                    invoicetbl.AddCell(new PdfPCell(new Phrase("INVOICE", fontTitle2)) { Colspan = 10, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_BASELINE, BorderColorBottom = new BaseColor(System.Drawing.Color.Black), BorderWidthBottom = 1f, Border = 0 });

                    PdfPTable ObjInvoiceDetails2 = new PdfPTable(4);
                    ObjInvoiceDetails2.WidthPercentage = 100;
                    int[] ObjInvoiceDetailsCellWidth2 = { 20, 30, 10, 40 };
                    ObjInvoiceDetails2.SetWidths(ObjInvoiceDetailsCellWidth2);

                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase("Beneficiary Bank Name :", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase("ICICI BANK Ltd", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_LEFT });

                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase("", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase("For " + System.Configuration.ConfigurationManager.AppSettings["CompanyFullName"].ToString(), fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_RIGHT });

                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase("Beneficiary A/C No :", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(System.Configuration.ConfigurationManager.AppSettings["BEN_ACC_NO"].ToString(), fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_LEFT });

                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase("", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase("", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_RIGHT });

                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase("Beneficiary A/C Name :", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(System.Configuration.ConfigurationManager.AppSettings["CompanyFullName"].ToString(), fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_LEFT });

                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase("", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase("", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_RIGHT });

                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase("Beneficiary Bank Swift Code :", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(System.Configuration.ConfigurationManager.AppSettings["BEN_SWIFT_CODE"].ToString(), fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_LEFT });

                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase("", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase("", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_RIGHT });

                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase("Beneficiary Bank IFSC Code :", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(System.Configuration.ConfigurationManager.AppSettings["BEN_IFSC_CODE"].ToString(), fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_LEFT });

                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase("", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase("", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_RIGHT });


                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase("GSTIN :", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(System.Configuration.ConfigurationManager.AppSettings["BEN_GSTIN"].ToString(), fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                    ObjInvoiceDetails.AddCell(new PdfPCell(new Phrase(ObjInvoice._FICC_details[0]._FiccGstin, fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_RIGHT });

                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });

                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase("PAN :", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(System.Configuration.ConfigurationManager.AppSettings["BEN_PAN"].ToString(), fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_LEFT });

                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase("", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_RIGHT });

                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase("CIN :", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(System.Configuration.ConfigurationManager.AppSettings["BEN_CIN"].ToString(), fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_LEFT });

                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase("", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_RIGHT });

                    //ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase("Service tax Reg :", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                    //ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase("AAACD1707CSTOO1", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });


                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase("Authorized Signatory", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_RIGHT });

                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });

                    //ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase("TIN No :", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_LEFT });
                    //ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" 33151353935V ", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0, HorizontalAlignment = Element.ALIGN_LEFT });

                    //ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    //ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });

                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });

                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle11)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoiceDetails2.AddCell(new PdfPCell(new Phrase(" ", fontTitle1)) { BorderColor = BaseColor.WHITE, BorderWidth = 0 });


                    //   ObjInvoiceDetailstable.AddCell(new PdfPCell(new Phrase("Total Amount in words : " + objCmnFunctions.ConvertNumbertoWords(Convert.ToInt32(total)), fontTitle1)));

                    PdfPTable ObjInvoicedet = new PdfPTable(1);
                    ObjInvoicedet.WidthPercentage = 100;
                    ObjInvoicedet.DefaultCell.Border = Rectangle.BOX;
                    ObjInvoicedet.AddCell(new PdfPCell(new Phrase(System.Configuration.ConfigurationManager.AppSettings["CompanyFullName"].ToString(), fontTitle2)) { Colspan = 10, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    //ObjInvoicedet.AddCell(new PdfPCell(new Phrase("27 28 29, Mansarover Commercial Complex, Hoshangabad Road, Bhopal, Madhya Pradesh - 462003.", font8)) { Colspan = 10, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoicedet.AddCell(new PdfPCell(new Phrase(ObjInvoice._FICC_details[0]._Ficc_Branchaddress, fontTitle1)) { Colspan = 10, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    //ObjInvoicedet.AddCell(new PdfPCell(new Phrase("MADHYA PRADESH", font8)) { Colspan = 10, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjInvoicedet.AddCell(new PdfPCell(new Phrase(ObjInvoice._FICC_details[0]._Ficc_Location, fontTitle1)) { Colspan = 10, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BorderColor = BaseColor.WHITE, BorderWidth = 0 });

                    //Add Approver Details

                    PdfPTable ObjRegAddress = new PdfPTable(1);
                    ObjRegAddress.WidthPercentage = 100;
                    ObjRegAddress.DefaultCell.Border = Rectangle.BOX;
                    ObjRegAddress.AddCell(new PdfPCell(new Phrase("Regd. Office:  " + System.Configuration.ConfigurationManager.AppSettings["LAddress1"].ToString() + ", " + System.Configuration.ConfigurationManager.AppSettings["LAddress2"].ToString() + ", " + System.Configuration.ConfigurationManager.AppSettings["LArea"].ToString()+", ", fontTitle2)) { Colspan = 10, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    //ObjInvoicedet.AddCell(new PdfPCell(new Phrase("27 28 29, Mansarover Commercial Complex, Hoshangabad Road, Bhopal, Madhya Pradesh - 462003.", font8)) { Colspan = 10, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    ObjRegAddress.AddCell(new PdfPCell(new Phrase(System.Configuration.ConfigurationManager.AppSettings["LCity"].ToString(), fontTitle2)) { Colspan = 10, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                    //ObjInvoicedet.AddCell(new PdfPCell(new Phrase("MADHYA PRADESH", font8)) { Colspan = 10, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BorderColor = BaseColor.WHITE, BorderWidth = 0 });
                 


                    //document.Add(new Paragraph(new Phrase("FULLERTON INDIA CREDIT COMPANY LIMITED.")));
                    document.Add(ObjInvoicedet);
                    document.Add(new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(3.0F, 100.0F, new BaseColor(227, 111, 30), Element.ALIGN_LEFT, 1))));
                    document.Add(invoicetbl);
                    document.Add(new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(3.0F, 100.0F, new BaseColor(227, 111, 30), Element.ALIGN_LEFT, 1))));
                    document.Add(ObjInvoiceDetails);
                    document.Add(new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(3.0F, 100.0F, new BaseColor(227, 111, 30), Element.ALIGN_LEFT, 1))));
                    document.Add(ObjInvoiceDetailstable);
                    document.Add(new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(3.0F, 100.0F, new BaseColor(227, 111, 30), Element.ALIGN_LEFT, 1))));
                    document.Add(ObjInvoiceDetailsgsttable);
                    //document.Add(new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(3.0F, 100.0F, new BaseColor(227, 111, 30), Element.ALIGN_LEFT, 1))));
                    // bharathidhasan kumar-- document.Add(ObjInvoiceDetailscessgsttable);
                    document.Add(new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(3.0F, 100.0F, new BaseColor(227, 111, 30), Element.ALIGN_LEFT, 1))));
                    document.Add(new Paragraph(new Phrase("Total Amount in words (INR): " + objCmnFunctions.ConvertDecimaltoWords(Convert.ToDecimal(total)), fontTitle11)));
                    document.Add(new Paragraph(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(3.0F, 100.0F, new BaseColor(227, 111, 30), Element.ALIGN_LEFT, 1))));
                    //document.Add(new Paragraph(" "));
                    document.Add(ObjInvoiceDetails2);
                    //document.Add(ObjInvoicedet);
                    //document.Add(new Paragraph(" "));
                    //document.Add(new Paragraph(new Phrase("PAN : AAACD1707C", fontTitle1)));
                    //document.Add(new Paragraph(new Phrase("Service tax Reg :  AAACD1707CSTOO1", fontTitle1)));
                    //document.Add(new Paragraph(new Phrase("TIN No : 33151353935V ", fontTitle1)));
                    //document.Add(new Paragraph(" "));
                    //document.Add(new Paragraph(new Phrase("For Fullerton India Credit Company", fontTitle11)));
                    //document.Add(new Paragraph(" ")); document.Add(new Paragraph(" ")); document.Add(new Paragraph(" ")); document.Add(new Paragraph(" "));
                    //document.Add(new Paragraph(new Phrase("Authorized Signatory", fontTitle11)));
                    // document.Add(new Paragraph(" "));
                    //  document.Add(new Paragraph(new Phrase("We hereby certify that our registration certificate under the Maharastra Value Added Tax Act 2002 is in force on the date on which the sale of the goods specified in this invoice is made by us and that the transaction of sale covered by this invoice has been effected by us and it shall be accounted for in the turnover of sales while filing of return and the due tax, if any, payable on the sale has been paid or shall be paid.", fontTitle1)));                   
                    //  document.Add(new Paragraph(" "));
                    document.Add(ObjRegAddress);
                    document.Close();
                    writer.Close();
                    string fileprintname = "SALE_INVOICE_" + ObjInvoice._SaleInvoice_list[0]._invoice_no;
                    Response.ContentType = "pdf/application";
                    Response.AddHeader("content-disposition",
                    "attachment;filename=" + fileprintname + ".PDF");
                    Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
        }
        public ActionResult downloadsexcel()
        {
            
            List<SaleInvoice> cList;
            cList = (List<SaleInvoice>)Session["SaleInvExceldownload"];

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("Sale Number");
            dt.Columns.Add("Sale Date");
            dt.Columns.Add("Vendor Name");
            dt.Columns.Add("Vendor Address");
            dt.Columns.Add("Sale Amount");
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                i + 1
                , cList[i]._sale_no.ToString()
                , cList[i]._sale_date.ToString()
                , cList[i]._vendor_name.ToString()
                , cList[i]._vendor_address.ToString()
                , cList[i]._sale_amt.ToString());
            }


            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            // Session["exceldownload"] = gv;
            if (gv.Rows.Count != 0)
            {
                //return new DownloadFileActionResult((GridView)Session["exceldownload"], "DocumentGroup.xls");
                return new DownloadFileActionResult(gv, "Sale Invoice Summary.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }
    }
}
