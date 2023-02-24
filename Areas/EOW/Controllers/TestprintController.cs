using System.Web.Mvc;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using IEM.Areas.EOW.Models;
using IEM.Common;
using IEM.App_Start;
namespace IEM.Areas.EOW.Controllers
{
    [NoDirectAccess]
    public class TestprintController : Controller
    {
        //
        // GET: /EOW/Testprint/
        CmnFunctions objCmnFunctions = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        PrintModel EOWDataModel = new PrintModel();
        EOW_DataModel DataModel = new EOW_DataModel();
        public ActionResult Index(string ecfgid)
        {
            //challanPrint("33", "S");
            //challanPrintarf("9", "A");
            string ecfgids = "123";
            challanPrintct(ecfgids, "S");
            return View();
        }
        public void challanPrint(string ecfgid, string type)
        {
            try
            {
                iTextSharp.text.Font fontTitle = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle1 = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle11 = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle2 = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle3 = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitlenote = FontFactory.GetFont("Arial", 6, iTextSharp.text.Font.NORMAL, BaseColor.RED);
                using (MemoryStream ms = new MemoryStream())
                {
                    Document document = new Document(PageSize.A4, 10F, 10F, 10F, 0F);
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);
                    document.Open();

                    printdatamodel objModel = new printdatamodel();
                    PrintModel pm = new PrintModel();
                    if (type == "S")
                    {
                        objModel = EOWDataModel.SelectEmployeeSearch(ecfgid, "S");
                    }
                    else
                    {
                        objModel = EOWDataModel.SelectPrintGeneral(ecfgid, "E");
                    }
                    printdatamodel objModeldatadecl = new printdatamodel();
                    objModeldatadecl = pm.ToGetDeclaration(ecfgid);
                    objModel.dclnotesub = objModeldatadecl.dclnotesub;
                    objModel.dclnoteapp = objModeldatadecl.dclnoteapp;
                    document.NewPage();
                    PdfPTable HeaderTable = new PdfPTable(10);
                    HeaderTable.WidthPercentage = 100;

                    //--------------------------------------------ADD Invoice Details---------------------------------------
                    // BackgroundColor = BaseColor.LIGHT_GRAY 
                    PdfPCell Headerlogo = new PdfPCell();
                    string Headerlogoath = Server.MapPath("~/Content/images/Fullerton.bmp");
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Headerlogoath);
                    image.WidthPercentage = 90;
                    Headerlogo.AddElement(image);
                    Headerlogo.HorizontalAlignment = Element.ALIGN_CENTER;
                    Headerlogo.VerticalAlignment = Element.ALIGN_MIDDLE;
                    Headerlogo.Colspan = 3;
                    Headerlogo.Rowspan = 2;

                    string ecfno = objModel.Ecf_No;
                    PdfContentByte cb = new PdfContentByte(writer);
                    Barcode128 code128 = new Barcode128();
                    code128.CodeType = Barcode.CODE128_UCC;
                    code128.Code = ecfno;
                    iTextSharp.text.Image image128 = code128.CreateImageWithBarcode(cb, null, null);
                    image128.ScaleAbsolute(110f, 155.25f);

                    PdfPCell ECFNo = new PdfPCell();
                    ECFNo.HorizontalAlignment = Element.ALIGN_CENTER;
                    ECFNo.VerticalAlignment = Element.ALIGN_MIDDLE;
                    ECFNo.AddElement(image128);
                    HeaderTable.AddCell(new PdfPCell(new Phrase(objModel.Header, fontTitle2)) { Colspan = 7, Rowspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    HeaderTable.AddCell(Headerlogo);

                    PdfPTable t = new PdfPTable(9);
                    t.WidthPercentage = 100;

                    t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_No, fontTitle3)) { Rowspan = 3, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                    t.AddCell(new PdfPCell(new Phrase("Employee ID", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.EmployeeId, fontTitle1)) { Colspan = 2 });
                    t.AddCell(new PdfPCell(new Phrase("Invoice Type", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Invoice_type, fontTitle1)));

                    t.AddCell(new PdfPCell(new Phrase("ECF Date", fontTitle1)) { Colspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                    t.AddCell(new PdfPCell(new Phrase("Name", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.EmployeeName, fontTitle1)) { Colspan = 2 });
                    t.AddCell(new PdfPCell(new Phrase("Expense Type", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Expense_type, fontTitle1)));

                    t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_Date, fontTitle3)) { Rowspan = 2, Colspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                    t.AddCell(new PdfPCell(new Phrase("Title", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Title, fontTitle1)) { Colspan = 2 });
                    t.AddCell(new PdfPCell(new Phrase("Amort", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_Amort, fontTitle1)));

                    t.AddCell(new PdfPCell(ECFNo) { Rowspan = 3, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingLeft = 7f, PaddingBottom = 7f, PaddingTop = 7f, PaddingRight = 7f });

                    t.AddCell(new PdfPCell(new Phrase("Department", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Department, fontTitle1)) { Colspan = 2 });
                    t.AddCell(new PdfPCell(new Phrase("For Ex", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_forex, fontTitle1)));

                    t.AddCell(new PdfPCell(new Phrase("ECF Amount", fontTitle1)) { Colspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                    t.AddCell(new PdfPCell(new Phrase("Location Code", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.LocationCode, fontTitle1)) { Colspan = 2 });
                    t.AddCell(new PdfPCell(new Phrase("Utility", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_Utility, fontTitle1)));

                    t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_Amount, fontTitle3)) { Rowspan = 2, Colspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                    t.AddCell(new PdfPCell(new Phrase("PO / WO No.", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Invoice_powono, fontTitle1)) { Colspan = 2 });
                    t.AddCell(new PdfPCell(new Phrase("Payment Adjst", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_PaymentAdjst, fontTitle1)));

                    //--------------------------------------------ADD Invoice Details---------------------------------------
                    Font font8 = FontFactory.GetFont("ARIAL", 7);
                    PdfPTable InvoiceTable = new PdfPTable(5);
                    PdfPCell InvoicePCell = null;
                    if (type == "S")
                    {
                        InvoiceTable.AddCell(new PdfPCell(new Phrase("Invoice Details ", fontTitle11)) { Colspan = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                        InvoiceTable.AddCell(new PdfPCell(new Phrase("Vendor Code", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                        InvoiceTable.AddCell(new PdfPCell(new Phrase("Vendor Name", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                        InvoiceTable.AddCell(new PdfPCell(new Phrase("Invoice No.", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                        InvoiceTable.AddCell(new PdfPCell(new Phrase("Invoice Date", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                        InvoiceTable.AddCell(new PdfPCell(new Phrase("Invoice Amount", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                        DataTable dtinvoice = new DataTable();
                        dtinvoice = EOWDataModel.GetSupplieinvoice(ecfgid);
                        decimal tolamtinvoice = 0;
                        for (int rows = 0; rows < dtinvoice.Rows.Count; rows++)
                        {
                            InvoicePCell = new PdfPCell(new Phrase(new Chunk(dtinvoice.Rows[rows]["supplierheader_suppliercode"].ToString(), font8)));
                            InvoiceTable.AddCell(InvoicePCell);
                            InvoicePCell = new PdfPCell(new Phrase(new Chunk(dtinvoice.Rows[rows]["supplierheader_name"].ToString(), font8)));
                            InvoiceTable.AddCell(InvoicePCell);
                            InvoicePCell = new PdfPCell(new Phrase(new Chunk(dtinvoice.Rows[rows]["invoice_no"].ToString(), font8)));
                            InvoiceTable.AddCell(InvoicePCell);
                            InvoicePCell = new PdfPCell(new Phrase(new Chunk(dtinvoice.Rows[rows]["invoice_date"].ToString(), font8)));
                            InvoiceTable.AddCell(InvoicePCell);
                            InvoiceTable.AddCell(new PdfPCell(new Phrase(objCmnFunctions.GetINRAmount(dtinvoice.Rows[rows]["invoice_amount"].ToString()), font8)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });
                            if (tolamtinvoice == 0)
                            {
                                tolamtinvoice = Convert.ToDecimal(dtinvoice.Rows[rows]["invoice_amount"].ToString());
                            }
                            else
                            {
                                tolamtinvoice = tolamtinvoice + Convert.ToDecimal(dtinvoice.Rows[rows]["invoice_amount"].ToString());
                            }
                        }
                        InvoiceTable.AddCell(new PdfPCell(new Phrase("TOTAL FROM INVOICE TOTAL", fontTitle11)) { Colspan = 4, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE });
                        InvoiceTable.AddCell(new PdfPCell(new Phrase(objCmnFunctions.GetINRAmount(tolamtinvoice.ToString()), fontTitle11)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });
                        InvoiceTable.SpacingBefore = 15f;
                        InvoiceTable.WidthPercentage = 100;
                    }
                    //--------------------------------------------ADD Expense Details---------------------------------------
                    PdfPTable ExpenseTable = new PdfPTable(8);
                    PdfPCell ExpensePCell = null;
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("Expense / Asset Details", fontTitle11)) { Colspan = 8, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("Nature of Expense / Asset Description", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("Category", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("Sub Category", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("Product Code", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("Business Segment", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("Cost Center", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("Branch Code", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("Debit Amount", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                    DataTable dtDebit = new DataTable();
                    dtDebit = EOWDataModel.GetprintExpensesupplier(ecfgid.ToString());
                    decimal tolamtDebit = 0;
                    for (int rows = 0; rows < dtDebit.Rows.Count; rows++)
                    {
                        if (Convert.ToString(dtDebit.Rows[rows]["ecfdebitline_category_type"].ToString()) == "A")
                        {
                            ExpensePCell = new PdfPCell(new Phrase(new Chunk("", font8)));
                            ExpenseTable.AddCell(ExpensePCell);
                            ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["assetcategory_name"].ToString(), font8)));
                            ExpenseTable.AddCell(ExpensePCell);
                            ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["asset_description"].ToString(), font8)));
                            ExpenseTable.AddCell(ExpensePCell);
                        }
                        else
                        {
                            ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["expnature_name"].ToString(), font8)));
                            ExpenseTable.AddCell(ExpensePCell);
                            ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["expcat_name"].ToString(), font8)));
                            ExpenseTable.AddCell(ExpensePCell);
                            ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["expsubcat_name"].ToString(), font8)));
                            ExpenseTable.AddCell(ExpensePCell);
                        }
                        ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["ecfdebitline_product_code"].ToString(), font8)));
                        ExpenseTable.AddCell(ExpensePCell);
                        ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["ecfdebitline_fc_code"].ToString(), font8)));
                        ExpenseTable.AddCell(ExpensePCell);
                        ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["ecfdebitline_cc_code"].ToString(), font8)));
                        ExpenseTable.AddCell(ExpensePCell);
                        ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["ecfdebitline_ou_code"].ToString(), font8)));
                        ExpenseTable.AddCell(ExpensePCell);
                        ExpenseTable.AddCell(new PdfPCell(new Phrase(objCmnFunctions.GetINRAmount(dtDebit.Rows[rows]["ecftravel_amount"].ToString()), font8)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });
                        if (tolamtDebit == 0)
                        {
                            tolamtDebit = Convert.ToDecimal(dtDebit.Rows[rows]["ecftravel_amount"].ToString());
                        }
                        else
                        {
                            tolamtDebit = tolamtDebit + Convert.ToDecimal(dtDebit.Rows[rows]["ecftravel_amount"].ToString());
                        }
                    }
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("TOTAL FROM DEBIT TOTAL", fontTitle11)) { Colspan = 7, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE });
                    ExpenseTable.AddCell(new PdfPCell(new Phrase(objCmnFunctions.GetINRAmount(tolamtDebit.ToString()), fontTitle11)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });
                    ExpenseTable.SpacingBefore = 15f;
                    ExpenseTable.WidthPercentage = 100;

                    //--------------------------------------------ADD Payment Details---------------------------------------
                    PdfPTable PaymentTable = new PdfPTable(5);
                    PdfPCell PaymentPCell = null;
                    PaymentTable.AddCell(new PdfPCell(new Phrase("Payment Details", fontTitle11)) { Colspan = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    PaymentTable.AddCell(new PdfPCell(new Phrase("Pay To", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    PaymentTable.AddCell(new PdfPCell(new Phrase("Beneficiary Name", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    PaymentTable.AddCell(new PdfPCell(new Phrase("Pay Mode", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    PaymentTable.AddCell(new PdfPCell(new Phrase("Bank A/C No./Payment Ref No.", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    PaymentTable.AddCell(new PdfPCell(new Phrase("Credit Amount", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                    DataTable dtPayment = new DataTable();
                    dtPayment = EOWDataModel.GetprintPayment(ecfgid.ToString());
                    decimal tolamtPayment = 0;
                    for (int rows = 0; rows < dtPayment.Rows.Count; rows++)
                    {
                        string empsup = dtPayment.Rows[rows]["ecf_supplier_employee"].ToString();
                        if (empsup == "E")
                        {
                            empsup = "Employee";
                        }
                        else
                        {
                            empsup = "Supplier";
                        }
                        PaymentPCell = new PdfPCell(new Phrase(new Chunk(empsup, font8)));
                        PaymentTable.AddCell(PaymentPCell);
                        PaymentPCell = new PdfPCell(new Phrase(new Chunk(dtPayment.Rows[rows]["ecfcreditline_beneficiary"].ToString(), font8)));
                        PaymentTable.AddCell(PaymentPCell);
                        PaymentPCell = new PdfPCell(new Phrase(new Chunk(dtPayment.Rows[rows]["ecfcreditline_pay_mode"].ToString(), font8)));
                        PaymentTable.AddCell(PaymentPCell);
                        PaymentPCell = new PdfPCell(new Phrase(new Chunk(dtPayment.Rows[rows]["ecfcreditline_ref_no"].ToString(), font8)));
                        PaymentTable.AddCell(PaymentPCell);
                        PaymentTable.AddCell(new PdfPCell(new Phrase(objCmnFunctions.GetINRAmount(dtPayment.Rows[rows]["ecfcreditline_amount"].ToString()), font8)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });

                        if (tolamtPayment == 0)
                        {
                            tolamtPayment = Convert.ToDecimal(dtPayment.Rows[rows]["ecfcreditline_amount"].ToString());
                        }
                        else
                        {
                            tolamtPayment = tolamtPayment + Convert.ToDecimal(dtPayment.Rows[rows]["ecfcreditline_amount"].ToString());
                        }
                    }
                    PaymentTable.AddCell(new PdfPCell(new Phrase("TOTAL FROM CREDIT TOTAL", fontTitle11)) { Colspan = 4, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE });
                    PaymentTable.AddCell(new PdfPCell(new Phrase(objCmnFunctions.GetINRAmount(tolamtPayment.ToString()), fontTitle11)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });
                    PaymentTable.SpacingBefore = 10f;
                    PaymentTable.WidthPercentage = 100;

                    PdfPTable tableamtdesc = new PdfPTable(2);
                    tableamtdesc.WidthPercentage = 100;
                    tableamtdesc.SpacingBefore = 10f;
                    tableamtdesc.AddCell(new PdfPCell(new Phrase("Amount in Words", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableamtdesc.AddCell(new PdfPCell(new Phrase("ECF Description", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableamtdesc.AddCell(new PdfPCell(new Phrase(objModel.Ecf_Amountinword, font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 10f });
                    tableamtdesc.AddCell(new PdfPCell(new Phrase(objModel.Ecf_dcsc, font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 10f });

                    PdfPTable tableDeclar = new PdfPTable(2);
                    tableDeclar.WidthPercentage = 100;
                    tableDeclar.SpacingBefore = 10f;
                    tableDeclar.AddCell(new PdfPCell(new Phrase("Declaration", fontTitle11)) { Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    tableDeclar.AddCell(new PdfPCell(new Phrase("Raiser Declaration", fontTitle11)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = PdfPCell.LEFT_BORDER });
                    tableDeclar.AddCell(new PdfPCell(new Phrase("Verifier Declaration", fontTitle11)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER });
                    //tableDeclar.AddCell(new PdfPCell(new Phrase("I confirm that I have incurred the expenditure wholly and exclusively for business purposes only.", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 2f, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER });
                    //tableDeclar.AddCell(new PdfPCell(new Phrase("I have verified the expense claim and approved on the basis that this Expense is incurred wholly and exclusively for Business Purposes.", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 2f, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER });

                    tableDeclar.AddCell(new PdfPCell(new Phrase(objModel.dclnotesub, font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 2f, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER });
                    tableDeclar.AddCell(new PdfPCell(new Phrase(objModel.dclnoteapp, font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 2f, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER });


                    //--------------------------------------------ADD Authorization Approvals Details---------------------------------------

                    PdfPTable AuthorizationTable = new PdfPTable(5);
                    PdfPCell AuthorizationPCell = null;
                    AuthorizationTable.AddCell(new PdfPCell(new Phrase("ECF Authorization Details", fontTitle11)) { Colspan = 8, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    AuthorizationTable.AddCell(new PdfPCell(new Phrase("Employee Name", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    AuthorizationTable.AddCell(new PdfPCell(new Phrase("Designation", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    AuthorizationTable.AddCell(new PdfPCell(new Phrase("Action Date", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    AuthorizationTable.AddCell(new PdfPCell(new Phrase("Status", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    AuthorizationTable.AddCell(new PdfPCell(new Phrase("Remarks", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                    EOW_DataModel objd = new EOW_DataModel();
                    List<ApproverHistry> lstaHistory = new List<ApproverHistry>();
                    lstaHistory = objd.GetecfappHistory(ecfgid.ToString(), ecfgid.ToString()).ToList();

                    for (int rows = 0; rows < lstaHistory.Count; rows++)
                    {
                        AuthorizationPCell = new PdfPCell(new Phrase(new Chunk(lstaHistory[rows].empcode, font8)));
                        AuthorizationTable.AddCell(AuthorizationPCell);
                        AuthorizationPCell = new PdfPCell(new Phrase(new Chunk(lstaHistory[rows].empname, font8)));
                        AuthorizationTable.AddCell(AuthorizationPCell);
                        AuthorizationPCell = new PdfPCell(new Phrase(new Chunk(lstaHistory[rows].statusdate, font8)));
                        AuthorizationTable.AddCell(AuthorizationPCell);
                        AuthorizationPCell = new PdfPCell(new Phrase(new Chunk(lstaHistory[rows].status, font8)));
                        AuthorizationTable.AddCell(AuthorizationPCell);
                        AuthorizationPCell = new PdfPCell(new Phrase(new Chunk(lstaHistory[rows].remarks, font8)));
                        AuthorizationTable.AddCell(AuthorizationPCell);
                    }
                    AuthorizationTable.SpacingBefore = 15f;
                    AuthorizationTable.WidthPercentage = 100;

                    PdfPTable tableSign = new PdfPTable(4);
                    tableSign.WidthPercentage = 100;
                    tableSign.SpacingBefore = 10f;
                    tableSign.AddCell(new PdfPCell(new Phrase("EPU Authentication", fontTitle11)) { Colspan = 4, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    tableSign.AddCell(new PdfPCell(new Phrase("", font8)) { Colspan = 4, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = 10f, Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER });
                    tableSign.AddCell(new PdfPCell(new Phrase("Emp ID & Emp Name ", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 30f, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER });

                    tableSign.AddCell(new PdfPCell(new Phrase("Maker ID", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 30f, Border = PdfPCell.BOTTOM_BORDER });
                    tableSign.AddCell(new PdfPCell(new Phrase("Authoriser ID", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 30f, Border = PdfPCell.BOTTOM_BORDER });
                    tableSign.AddCell(new PdfPCell(new Phrase("Verifier ID & Name", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 30f, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER });

                    //--------------------------------------------ADD Employeelist Details---------------------------------------

                    PdfPTable EmployeelistTable = new PdfPTable(3);
                    if (objModel.ecfdocsubtype == "2")
                    {
                        PdfPCell EmployeelistPCell = null;
                        //EmployeelistTable.AddCell(new PdfPCell(new Phrase("Employee list Details", fontTitle11)) { Colspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                        EmployeelistTable.AddCell(new PdfPCell(new Phrase("S.No", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                        EmployeelistTable.AddCell(new PdfPCell(new Phrase("Employee Name", fontTitle11)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                        EmployeelistTable.AddCell(new PdfPCell(new Phrase("Employee Signatures", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                        List<EOW_Employeelst> lstEmployeelst = new List<EOW_Employeelst>();
                        lstEmployeelst = objd.GetEmployeeelist(ecfgid.ToString()).ToList();

                        for (int rows = 0; rows < lstEmployeelst.Count; rows++)
                        {
                            int sno = rows + 1;
                            EmployeelistPCell = new PdfPCell(new Phrase(new Chunk(sno.ToString(), font8)));
                            EmployeelistTable.AddCell(EmployeelistPCell);
                            EmployeelistPCell = new PdfPCell(new Phrase(new Chunk(lstEmployeelst[rows].empCode + "-" + lstEmployeelst[rows].empName, font8)));
                            EmployeelistTable.AddCell(EmployeelistPCell);
                            EmployeelistPCell = new PdfPCell(new Phrase(new Chunk(lstEmployeelst[rows].sign, font8)));
                            EmployeelistTable.AddCell(EmployeelistPCell);
                        }
                        EmployeelistTable.SpacingBefore = 15f;
                        EmployeelistTable.WidthPercentage = 70;
                        EmployeelistTable.HorizontalAlignment = Element.ALIGN_LEFT;
                        int[] listTable = { 10, 30, 40 };
                        EmployeelistTable.SetWidths(listTable);
                    }

                    PdfPTable tablenote = new PdfPTable(1);
                    tablenote.WidthPercentage = 100;
                    tablenote.SpacingBefore = 10f;
                    tablenote.AddCell(new PdfPCell(new Phrase("* This is an electronically generated form and does not require signature. The IDs mentioned there in tantamount to signature of the relevant employees", fontTitlenote)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = 0 });

                    document.Add(HeaderTable);
                    document.Add(t);
                    if (type == "S")
                    {
                        document.Add(InvoiceTable);
                    }
                    document.Add(ExpenseTable);
                    document.Add(PaymentTable);
                    document.Add(tableamtdesc);
                    document.Add(tableDeclar);
                    document.Add(AuthorizationTable);
                    //document.Add(tableSign);
                    if (objModel.ecfdocsubtype == "2")
                    {
                        document.Add(EmployeelistTable);
                    }
                    document.Add(tablenote);
                    string updateresulr = DataModel.printstatus(ecfgid);
                    document.Close();
                    writer.Close();
                    string fileprintname = ecfno + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss");
                    Response.ContentType = "pdf/application";
                    Response.AddHeader("content-disposition",
                    "attachment;filename=" + fileprintname + ".pdf");
                    Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
        }
        public void challanPrintarf(string ecfgid, string type)
        {
            try
            {
                iTextSharp.text.Font fontTitle = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle1 = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle11 = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle2 = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle3 = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitlenote = FontFactory.GetFont("Arial", 6, iTextSharp.text.Font.NORMAL, BaseColor.RED);
                using (MemoryStream ms = new MemoryStream())
                {
                    Document document = new Document(PageSize.A4, 10F, 10F, 10F, 0F);
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);
                    document.Open();

                    printdatamodel objModel = new printdatamodel();
                    objModel = EOWDataModel.SelectPrintGeneral(ecfgid, "A");
                    PrintModel pm = new PrintModel();
                    printdatamodel objModeldatadecl = new printdatamodel();
                    objModeldatadecl = pm.ToGetDeclaration(ecfgid);
                    objModel.dclnotesub = objModeldatadecl.dclnotesub;
                    objModel.dclnoteapp = objModeldatadecl.dclnoteapp;
                    document.NewPage();
                    PdfPTable HeaderTable = new PdfPTable(4);
                    HeaderTable.WidthPercentage = 100;

                    //--------------------------------------------ADD Invoice Details---------------------------------------
                    // BackgroundColor = BaseColor.LIGHT_GRAY 
                    PdfPCell Headerlogo = new PdfPCell();
                    string Headerlogoath = Server.MapPath("~/Content/images/Fullerton.bmp");
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Headerlogoath);
                    image.WidthPercentage = 90;
                    Headerlogo.AddElement(image);
                    Headerlogo.HorizontalAlignment = Element.ALIGN_CENTER;
                    Headerlogo.VerticalAlignment = Element.ALIGN_MIDDLE;
                    Headerlogo.Rowspan = 2;

                    string ecfno = objModel.Ecf_No;
                    PdfContentByte cb = new PdfContentByte(writer);
                    Barcode128 code128 = new Barcode128();
                    code128.CodeType = Barcode.CODE128_UCC;
                    code128.Code = ecfno;
                    iTextSharp.text.Image image128 = code128.CreateImageWithBarcode(cb, null, null);
                    image128.ScaleAbsolute(110f, 155.25f);

                    PdfPCell ECFNo = new PdfPCell();
                    ECFNo.HorizontalAlignment = Element.ALIGN_CENTER;
                    ECFNo.VerticalAlignment = Element.ALIGN_MIDDLE;
                    ECFNo.AddElement(image128);
                    HeaderTable.AddCell(new PdfPCell(new Phrase(" ADVANCE REQUISITION FORM ", fontTitle2)) { Colspan = 3, Rowspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    HeaderTable.AddCell(Headerlogo);

                    PdfPTable t = new PdfPTable(8);
                    t.WidthPercentage = 100;

                    t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_No, fontTitle3)) { Rowspan = 3, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                    t.AddCell(new PdfPCell(new Phrase("Employee ID", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.EmployeeId, fontTitle1)) { Colspan = 3 });

                    t.AddCell(new PdfPCell(new Phrase("ARF Date", fontTitle1)) { Colspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                    t.AddCell(new PdfPCell(new Phrase("Name", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.EmployeeName, fontTitle1)) { Colspan = 3 });

                    t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_Date, fontTitle3)) { Rowspan = 2, Colspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                    t.AddCell(new PdfPCell(new Phrase("Title", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Title, fontTitle1)) { Colspan = 3 });

                    t.AddCell(new PdfPCell(ECFNo) { Rowspan = 3, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingLeft = 17f, PaddingBottom = 7f, PaddingTop = 7f, PaddingRight = 7f });

                    t.AddCell(new PdfPCell(new Phrase("Department", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Department, fontTitle1)) { Colspan = 3 });

                    t.AddCell(new PdfPCell(new Phrase("ARF Amount", fontTitle1)) { Colspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                    t.AddCell(new PdfPCell(new Phrase("Location Code", fontTitle1)) { Rowspan = 2, BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.LocationCode, fontTitle1)) { Colspan = 3, Rowspan = 2 });

                    t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_Amount, fontTitle3)) { Rowspan = 2, Colspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                    //t.AddCell(new PdfPCell(new Phrase("PO / WO No.", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    //t.AddCell(new PdfPCell(new Phrase(objModel.Invoice_powono, fontTitle1)) { Colspan = 3 });

                    //--------------------------------------------ADD Advance Details---------------------------------------
                    Font font8 = FontFactory.GetFont("ARIAL", 7);
                    PdfPTable AdvanceTable = new PdfPTable(7);
                    PdfPCell InvoicePCell = null;
                    AdvanceTable.AddCell(new PdfPCell(new Phrase("Advance Details", fontTitle11)) { Colspan = 7, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    AdvanceTable.AddCell(new PdfPCell(new Phrase("Advance Type", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    AdvanceTable.AddCell(new PdfPCell(new Phrase("Purpose", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    AdvanceTable.AddCell(new PdfPCell(new Phrase("Liquidation By", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    AdvanceTable.AddCell(new PdfPCell(new Phrase("Business Segment", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    AdvanceTable.AddCell(new PdfPCell(new Phrase("Cost Center", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    AdvanceTable.AddCell(new PdfPCell(new Phrase("Branch Code", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    AdvanceTable.AddCell(new PdfPCell(new Phrase("Advance Amount", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                    DataTable dtAdvance = new DataTable();
                    dtAdvance = EOWDataModel.SelectAdvanceecf(ecfgid.ToString());
                    decimal tolamtinvoice = 0;
                    for (int rows = 0; rows < dtAdvance.Rows.Count; rows++)
                    {
                        InvoicePCell = new PdfPCell(new Phrase(new Chunk(dtAdvance.Rows[rows]["ecfarf_advancetype_gid"].ToString(), font8)));
                        AdvanceTable.AddCell(InvoicePCell);
                        InvoicePCell = new PdfPCell(new Phrase(new Chunk(dtAdvance.Rows[rows]["ecfarf_desc"].ToString(), font8)));
                        AdvanceTable.AddCell(InvoicePCell);
                        InvoicePCell = new PdfPCell(new Phrase(new Chunk(dtAdvance.Rows[rows]["ecfarf_liq_date"].ToString(), font8)));
                        AdvanceTable.AddCell(InvoicePCell);
                        InvoicePCell = new PdfPCell(new Phrase(new Chunk(dtAdvance.Rows[rows]["ecfarf_fc_code"].ToString(), font8)));
                        AdvanceTable.AddCell(InvoicePCell);
                        InvoicePCell = new PdfPCell(new Phrase(new Chunk(dtAdvance.Rows[rows]["ecfarf_cc_code"].ToString(), font8)));
                        AdvanceTable.AddCell(InvoicePCell);
                        InvoicePCell = new PdfPCell(new Phrase(new Chunk(dtAdvance.Rows[rows]["ecfarf_ou_code"].ToString(), font8)));
                        AdvanceTable.AddCell(InvoicePCell);
                        AdvanceTable.AddCell(new PdfPCell(new Phrase(objCmnFunctions.GetINRAmount(dtAdvance.Rows[rows]["ecfarf_amount"].ToString()), font8)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });
                        if (tolamtinvoice == 0)
                        {
                            tolamtinvoice = Convert.ToDecimal(dtAdvance.Rows[rows]["ecfarf_amount"].ToString());
                        }
                        else
                        {
                            tolamtinvoice = tolamtinvoice + Convert.ToDecimal(dtAdvance.Rows[rows]["ecfarf_amount"].ToString());
                        }
                    }
                    AdvanceTable.AddCell(new PdfPCell(new Phrase("TOTAL FROM ADVANCE TOTAL", fontTitle11)) { Colspan = 6, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE });
                    AdvanceTable.AddCell(new PdfPCell(new Phrase(objCmnFunctions.GetINRAmount(tolamtinvoice.ToString()), fontTitle11)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });
                    AdvanceTable.SpacingBefore = 15f;
                    AdvanceTable.WidthPercentage = 100;

                    //--------------------------------------------ADD Additional Information Details---------------------------------------
                    PdfPTable PaymentTable = new PdfPTable(7);
                    PaymentTable.SpacingBefore = 10f;
                    PaymentTable.WidthPercentage = 100;
                    PdfPCell PaymentPCell = null;
                    PaymentTable.AddCell(new PdfPCell(new Phrase("Additional Information", fontTitle11)) { Colspan = 7, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    PaymentTable.AddCell(new PdfPCell(new Phrase("Vendor Code", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    PaymentTable.AddCell(new PdfPCell(new Phrase("Vendor Name", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    PaymentTable.AddCell(new PdfPCell(new Phrase("CBF No.", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    PaymentTable.AddCell(new PdfPCell(new Phrase("PO No.", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    PaymentTable.AddCell(new PdfPCell(new Phrase("Invoice Date", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    PaymentTable.AddCell(new PdfPCell(new Phrase("Invoice No", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    PaymentTable.AddCell(new PdfPCell(new Phrase("Invoice Amount", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                    DataTable dtsupplierdtails = new DataTable();
                    dtsupplierdtails = EOWDataModel.SelectAdvanceprint(ecfgid.ToString());
                    decimal tolamtPayment = 0;
                    if (dtsupplierdtails.Rows.Count > 0)
                    {
                        for (int rows = 0; rows < dtsupplierdtails.Rows.Count; rows++)
                        {
                            PaymentPCell = new PdfPCell(new Phrase(new Chunk(dtsupplierdtails.Rows[rows]["supplierheader_suppliercode"].ToString(), font8)));
                            PaymentTable.AddCell(PaymentPCell);
                            PaymentPCell = new PdfPCell(new Phrase(new Chunk(dtsupplierdtails.Rows[rows]["supplierheader_name"].ToString(), font8)));
                            PaymentTable.AddCell(PaymentPCell);
                            string po_no = Convert.ToString(dtsupplierdtails.Rows[rows]["ecfarf_po_no"].ToString());
                            if (po_no == "")
                            {
                                po_no = "-";
                            }
                            string cbf_no = Convert.ToString(dtsupplierdtails.Rows[rows]["ecfarf_cbf_no"].ToString());
                            if (cbf_no == "")
                            {
                                cbf_no = "-";
                            }
                            string ecf_date = Convert.ToString(dtsupplierdtails.Rows[rows]["invoice_date"].ToString());
                            if (ecf_date == "")
                            {
                                ecf_date = "-";
                            }
                            string ecf_no = Convert.ToString(dtsupplierdtails.Rows[rows]["invoice_no"].ToString());
                            if (ecf_no == "")
                            {
                                ecf_no = "-";
                            }
                            string ecf_amount = Convert.ToString(dtsupplierdtails.Rows[rows]["invoice_amount"].ToString());
                            if (ecf_amount == "")
                            {
                                ecf_amount = "-";
                            }

                            PaymentPCell = new PdfPCell(new Phrase(new Chunk(cbf_no, font8)));
                            PaymentTable.AddCell(PaymentPCell);
                            PaymentPCell = new PdfPCell(new Phrase(new Chunk(po_no, font8)));
                            PaymentTable.AddCell(PaymentPCell);
                            PaymentPCell = new PdfPCell(new Phrase(new Chunk(ecf_date, font8)));
                            PaymentTable.AddCell(PaymentPCell);
                            PaymentPCell = new PdfPCell(new Phrase(new Chunk(ecf_no, font8)));
                            PaymentTable.AddCell(PaymentPCell);
                            PaymentTable.AddCell(new PdfPCell(new Phrase(ecf_amount, font8)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });

                            if (ecf_amount != "" && ecf_amount != "-")
                            {
                                if (tolamtPayment == 0)
                                {
                                    tolamtPayment = Convert.ToDecimal(dtsupplierdtails.Rows[rows]["invoice_amount"].ToString());
                                }
                                else
                                {
                                    tolamtPayment = tolamtPayment + Convert.ToDecimal(dtsupplierdtails.Rows[rows]["invoice_amount"].ToString());
                                }
                            }
                        }
                        PaymentTable.AddCell(new PdfPCell(new Phrase("TOTAL FROM CREDIT TOTAL", fontTitle11)) { Colspan = 6, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE });
                        PaymentTable.AddCell(new PdfPCell(new Phrase(objCmnFunctions.GetINRAmount(tolamtPayment.ToString()), fontTitle11)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });
                    }
                    else
                    {
                        PaymentTable.AddCell(new PdfPCell(new Phrase("No Records Found", fontTitle11)) { Colspan = 7, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });
                    }
                    PdfPTable tableamtdesc = new PdfPTable(2);
                    tableamtdesc.WidthPercentage = 100;
                    tableamtdesc.SpacingBefore = 10f;
                    tableamtdesc.AddCell(new PdfPCell(new Phrase("Amount in Words", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableamtdesc.AddCell(new PdfPCell(new Phrase("ARF Description", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableamtdesc.AddCell(new PdfPCell(new Phrase(objModel.Ecf_Amountinword, font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 7f, PaddingTop = 7f });
                    tableamtdesc.AddCell(new PdfPCell(new Phrase(objModel.Ecf_dcsc, font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 7f, PaddingTop = 7f });

                    PdfPTable tableDeclar = new PdfPTable(2);
                    tableDeclar.WidthPercentage = 100;
                    tableDeclar.SpacingBefore = 10f;
                    tableDeclar.AddCell(new PdfPCell(new Phrase("Declaration", fontTitle11)) { Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    tableDeclar.AddCell(new PdfPCell(new Phrase("Raiser Declaration", fontTitle11)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = PdfPCell.LEFT_BORDER });
                    tableDeclar.AddCell(new PdfPCell(new Phrase("Verifier Declaration", fontTitle11)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER });
                    //tableDeclar.AddCell(new PdfPCell(new Phrase("I confirm that I have incurred the expenditure wholly and exclusively for business purposes only.", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 2f, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER });
                    //tableDeclar.AddCell(new PdfPCell(new Phrase("I have verified the expense claim and approved on the basis that this Expense is incurred wholly and exclusively for Business Purposes.", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 2f, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER });
                    tableDeclar.AddCell(new PdfPCell(new Phrase(objModel.dclnotesub, font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 2f, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER });
                    tableDeclar.AddCell(new PdfPCell(new Phrase(objModel.dclnoteapp, font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 2f, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER });


                    //--------------------------------------------ADD Authorization Approvals Details---------------------------------------

                    PdfPTable AuthorizationTable = new PdfPTable(5);
                    PdfPCell AuthorizationPCell = null;
                    AuthorizationTable.AddCell(new PdfPCell(new Phrase("ARF Authorization Details", fontTitle11)) { Colspan = 8, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    AuthorizationTable.AddCell(new PdfPCell(new Phrase("Employee Name", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    AuthorizationTable.AddCell(new PdfPCell(new Phrase("Designation", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    AuthorizationTable.AddCell(new PdfPCell(new Phrase("Action Date", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    AuthorizationTable.AddCell(new PdfPCell(new Phrase("Status", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    AuthorizationTable.AddCell(new PdfPCell(new Phrase("Remarks", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                    EOW_DataModel objd = new EOW_DataModel();
                    List<ApproverHistry> lstaHistory = new List<ApproverHistry>();
                    lstaHistory = objd.GetecfappHistory(ecfgid.ToString(), ecfgid.ToString()).ToList();

                    for (int rows = 0; rows < lstaHistory.Count; rows++)
                    {
                        AuthorizationPCell = new PdfPCell(new Phrase(new Chunk(lstaHistory[rows].empcode, font8)));
                        AuthorizationTable.AddCell(AuthorizationPCell);
                        AuthorizationPCell = new PdfPCell(new Phrase(new Chunk(lstaHistory[rows].empname, font8)));
                        AuthorizationTable.AddCell(AuthorizationPCell);
                        AuthorizationPCell = new PdfPCell(new Phrase(new Chunk(lstaHistory[rows].statusdate, font8)));
                        AuthorizationTable.AddCell(AuthorizationPCell);
                        AuthorizationPCell = new PdfPCell(new Phrase(new Chunk(lstaHistory[rows].status, font8)));
                        AuthorizationTable.AddCell(AuthorizationPCell);
                        AuthorizationPCell = new PdfPCell(new Phrase(new Chunk(lstaHistory[rows].remarks, font8)));
                        AuthorizationTable.AddCell(AuthorizationPCell);
                    }
                    AuthorizationTable.SpacingBefore = 15f;
                    AuthorizationTable.WidthPercentage = 100;

                    PdfPTable tableSign = new PdfPTable(4);
                    tableSign.WidthPercentage = 100;
                    tableSign.SpacingBefore = 10f;
                    tableSign.AddCell(new PdfPCell(new Phrase("EPU Authentication", fontTitle11)) { Colspan = 4, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    tableSign.AddCell(new PdfPCell(new Phrase("", font8)) { Colspan = 4, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = 10f, Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER });
                    tableSign.AddCell(new PdfPCell(new Phrase("Emp ID & Emp Name ", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 30f, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER });

                    tableSign.AddCell(new PdfPCell(new Phrase("Maker ID", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 30f, Border = PdfPCell.BOTTOM_BORDER });
                    tableSign.AddCell(new PdfPCell(new Phrase("Authoriser ID", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 30f, Border = PdfPCell.BOTTOM_BORDER });
                    tableSign.AddCell(new PdfPCell(new Phrase("Verifier ID & Name", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 30f, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER });

                    PdfPTable tablenote = new PdfPTable(1);
                    tablenote.WidthPercentage = 100;
                    tablenote.SpacingBefore = 10f;
                    tablenote.AddCell(new PdfPCell(new Phrase("* This is an electronically generated form and does not require signature. The IDs mentioned there in tantamount to signature of the relevant employees", fontTitlenote)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = 0 });

                    document.Add(HeaderTable);
                    document.Add(t);
                    document.Add(AdvanceTable);
                    document.Add(PaymentTable);
                    document.Add(tableamtdesc);
                    document.Add(tableDeclar);
                    document.Add(AuthorizationTable);
                    //document.Add(tableSign);
                    document.Add(tablenote);
                    string updateresulr = DataModel.printstatus(ecfgid);
                    document.Close();
                    writer.Close();
                    string fileprintname = ecfno + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss");
                    Response.ContentType = "pdf/application";
                    Response.AddHeader("content-disposition",
                    "attachment;filename=" + fileprintname + ".pdf");
                    Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
        }
        public void challanPrintct(string ecfgids, string type)
        {
            try
            {
                string[] ecfgidsplit = ecfgids.Split(',');

                iTextSharp.text.Font fontTitle = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle1 = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle11 = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle2 = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle3 = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitlenote = FontFactory.GetFont("Arial", 6, iTextSharp.text.Font.NORMAL, BaseColor.RED);
                using (MemoryStream ms = new MemoryStream())
                {
                    Document document = new Document(PageSize.A4, 10F, 10F, 10F, 0F);
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);
                    document.Open();

                    for (int i = 0; i < ecfgidsplit.Length; i++)
                    {
                        string ecfgid = ecfgidsplit[i].ToString();
                        if (ecfgid != "0")
                        {
                            printdatamodel objModel = new printdatamodel();
                            if (type == "S")
                            {
                                objModel = EOWDataModel.SelectEmployeeSearch(ecfgid, "S");
                            }
                            else
                            {
                                objModel = EOWDataModel.SelectPrintGeneral(ecfgid, "E");
                            }
                            PrintModel pm = new PrintModel();
                            printdatamodel objModeldatadecl = new printdatamodel();
                            objModeldatadecl = pm.ToGetDeclaration(ecfgid);
                            objModel.dclnotesub = objModeldatadecl.dclnotesub;
                            objModel.dclnoteapp = objModeldatadecl.dclnoteapp;
                            document.NewPage();
                            PdfPTable HeaderTable = new PdfPTable(10);
                            HeaderTable.WidthPercentage = 100;

                            //--------------------------------------------ADD Invoice Details---------------------------------------
                            // BackgroundColor = BaseColor.LIGHT_GRAY 
                            PdfPCell Headerlogo = new PdfPCell();
                            string Headerlogoath = Server.MapPath("~/Content/images/Fullerton.bmp");
                            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Headerlogoath);
                            image.WidthPercentage = 90;
                            Headerlogo.AddElement(image);
                            Headerlogo.HorizontalAlignment = Element.ALIGN_CENTER;
                            Headerlogo.VerticalAlignment = Element.ALIGN_MIDDLE;
                            Headerlogo.Colspan = 3;
                            Headerlogo.Rowspan = 2;

                            string ecfno = objModel.Ecf_No;
                            PdfContentByte cb = new PdfContentByte(writer);
                            Barcode128 code128 = new Barcode128();
                            code128.CodeType = Barcode.CODE128_UCC;
                            code128.Code = ecfno;
                            iTextSharp.text.Image image128 = code128.CreateImageWithBarcode(cb, null, null);
                            image128.ScaleAbsolute(110f, 155.25f);

                            PdfPCell ECFNo = new PdfPCell();
                            ECFNo.HorizontalAlignment = Element.ALIGN_CENTER;
                            ECFNo.VerticalAlignment = Element.ALIGN_MIDDLE;
                            ECFNo.AddElement(image128);
                            HeaderTable.AddCell(new PdfPCell(new Phrase(objModel.Header, fontTitle2)) { Colspan = 7, Rowspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                            HeaderTable.AddCell(Headerlogo);

                            PdfPTable t = new PdfPTable(9);
                            t.WidthPercentage = 100;

                            t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_No, fontTitle3)) { Rowspan = 3, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                            t.AddCell(new PdfPCell(new Phrase("Employee ID", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                            t.AddCell(new PdfPCell(new Phrase(objModel.EmployeeId, fontTitle1)) { Colspan = 2 });
                            t.AddCell(new PdfPCell(new Phrase("Invoice Type", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                            t.AddCell(new PdfPCell(new Phrase(objModel.Invoice_type, fontTitle1)));

                            t.AddCell(new PdfPCell(new Phrase("ECF Date", fontTitle1)) { Colspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                            t.AddCell(new PdfPCell(new Phrase("Name", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                            t.AddCell(new PdfPCell(new Phrase(objModel.EmployeeName, fontTitle1)) { Colspan = 2 });
                            t.AddCell(new PdfPCell(new Phrase("Expense Type", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                            t.AddCell(new PdfPCell(new Phrase(objModel.Expense_type, fontTitle1)));

                            t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_Date, fontTitle3)) { Rowspan = 2, Colspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                            t.AddCell(new PdfPCell(new Phrase("Title", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                            t.AddCell(new PdfPCell(new Phrase(objModel.Title, fontTitle1)) { Colspan = 2 });
                            t.AddCell(new PdfPCell(new Phrase("Amort", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                            t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_Amort, fontTitle1)));

                            t.AddCell(new PdfPCell(ECFNo) { Rowspan = 3, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingLeft = 7f, PaddingBottom = 7f, PaddingTop = 7f, PaddingRight = 7f });

                            t.AddCell(new PdfPCell(new Phrase("Department", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                            t.AddCell(new PdfPCell(new Phrase(objModel.Department, fontTitle1)) { Colspan = 2 });
                            t.AddCell(new PdfPCell(new Phrase("For Ex", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                            t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_forex, fontTitle1)));

                            t.AddCell(new PdfPCell(new Phrase("ECF Amount", fontTitle1)) { Colspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                            t.AddCell(new PdfPCell(new Phrase("Location Code", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                            t.AddCell(new PdfPCell(new Phrase(objModel.LocationCode, fontTitle1)) { Colspan = 2 });
                            t.AddCell(new PdfPCell(new Phrase("Utility", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                            t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_Utility, fontTitle1)));

                            t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_Amount, fontTitle3)) { Rowspan = 2, Colspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                            t.AddCell(new PdfPCell(new Phrase("PO / WO No.", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                            t.AddCell(new PdfPCell(new Phrase(objModel.Invoice_powono, fontTitle1)) { Colspan = 2 });
                            t.AddCell(new PdfPCell(new Phrase("Payment Adjst", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                            t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_PaymentAdjst, fontTitle1)));

                            //--------------------------------------------ADD Invoice Details---------------------------------------
                            Font font8 = FontFactory.GetFont("ARIAL", 7);
                            PdfPTable InvoiceTable = new PdfPTable(5);
                            PdfPCell InvoicePCell = null;
                            if (type == "S")
                            {
                                InvoiceTable.AddCell(new PdfPCell(new Phrase("Invoice Details ", fontTitle11)) { Colspan = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                                InvoiceTable.AddCell(new PdfPCell(new Phrase("Vendor Code", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                                InvoiceTable.AddCell(new PdfPCell(new Phrase("Vendor Name", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                                InvoiceTable.AddCell(new PdfPCell(new Phrase("Invoice No.", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                                InvoiceTable.AddCell(new PdfPCell(new Phrase("Invoice Date", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                                InvoiceTable.AddCell(new PdfPCell(new Phrase("Invoice Amount", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                                DataTable dtinvoice = new DataTable();
                                dtinvoice = EOWDataModel.GetSupplieinvoice(ecfgid.ToString());
                                decimal tolamtinvoice = 0;
                                for (int rows = 0; rows < dtinvoice.Rows.Count; rows++)
                                {
                                    InvoicePCell = new PdfPCell(new Phrase(new Chunk(dtinvoice.Rows[rows]["supplierheader_suppliercode"].ToString(), font8)));
                                    InvoiceTable.AddCell(InvoicePCell);
                                    InvoicePCell = new PdfPCell(new Phrase(new Chunk(dtinvoice.Rows[rows]["supplierheader_name"].ToString(), font8)));
                                    InvoiceTable.AddCell(InvoicePCell);
                                    InvoicePCell = new PdfPCell(new Phrase(new Chunk(dtinvoice.Rows[rows]["invoice_no"].ToString(), font8)));
                                    InvoiceTable.AddCell(InvoicePCell);
                                    InvoicePCell = new PdfPCell(new Phrase(new Chunk(dtinvoice.Rows[rows]["invoice_date"].ToString(), font8)));
                                    InvoiceTable.AddCell(InvoicePCell);
                                    InvoiceTable.AddCell(new PdfPCell(new Phrase(objCmnFunctions.GetINRAmount(dtinvoice.Rows[rows]["invoice_amount"].ToString()), font8)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });
                                    if (tolamtinvoice == 0)
                                    {
                                        tolamtinvoice = Convert.ToDecimal(dtinvoice.Rows[rows]["invoice_amount"].ToString());
                                    }
                                    else
                                    {
                                        tolamtinvoice = tolamtinvoice + Convert.ToDecimal(dtinvoice.Rows[rows]["invoice_amount"].ToString());
                                    }
                                }
                                InvoiceTable.AddCell(new PdfPCell(new Phrase("TOTAL FROM INVOICE TOTAL", fontTitle11)) { Colspan = 4, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE });
                                InvoiceTable.AddCell(new PdfPCell(new Phrase(objCmnFunctions.GetINRAmount(tolamtinvoice.ToString()), fontTitle11)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });
                                InvoiceTable.SpacingBefore = 15f;
                                InvoiceTable.WidthPercentage = 100;
                            }
                            //--------------------------------------------ADD Expense Details---------------------------------------
                            PdfPTable ExpenseTable = new PdfPTable(8);
                            PdfPCell ExpensePCell = null;
                            ExpenseTable.AddCell(new PdfPCell(new Phrase("Expense / Asset Details", fontTitle11)) { Colspan = 8, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                            ExpenseTable.AddCell(new PdfPCell(new Phrase("Nature of Expense / Asset Description", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                            ExpenseTable.AddCell(new PdfPCell(new Phrase("Category", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                            ExpenseTable.AddCell(new PdfPCell(new Phrase("Sub Category", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                            ExpenseTable.AddCell(new PdfPCell(new Phrase("Product Code", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                            ExpenseTable.AddCell(new PdfPCell(new Phrase("Business Segment", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                            ExpenseTable.AddCell(new PdfPCell(new Phrase("Cost Center", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                            ExpenseTable.AddCell(new PdfPCell(new Phrase("Branch Code", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                            ExpenseTable.AddCell(new PdfPCell(new Phrase("Debit Amount", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                            DataTable dtDebit = new DataTable();
                            dtDebit = EOWDataModel.GetprintExpensesupplier(ecfgid.ToString());
                            decimal tolamtDebit = 0;
                            for (int rows = 0; rows < dtDebit.Rows.Count; rows++)
                            {
                                if (Convert.ToString(dtDebit.Rows[rows]["ecfdebitline_category_type"].ToString()) == "A")
                                {
                                    ExpensePCell = new PdfPCell(new Phrase(new Chunk("", font8)));
                                    ExpenseTable.AddCell(ExpensePCell);
                                    ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["assetcategory_name"].ToString(), font8)));
                                    ExpenseTable.AddCell(ExpensePCell);
                                    ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["asset_description"].ToString(), font8)));
                                    ExpenseTable.AddCell(ExpensePCell);
                                }
                                else
                                {
                                    ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["expnature_name"].ToString(), font8)));
                                    ExpenseTable.AddCell(ExpensePCell);
                                    ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["expcat_name"].ToString(), font8)));
                                    ExpenseTable.AddCell(ExpensePCell);
                                    ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["expsubcat_name"].ToString(), font8)));
                                    ExpenseTable.AddCell(ExpensePCell);
                                }
                                ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["ecfdebitline_product_code"].ToString(), font8)));
                                ExpenseTable.AddCell(ExpensePCell);
                                ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["ecfdebitline_fc_code"].ToString(), font8)));
                                ExpenseTable.AddCell(ExpensePCell);
                                ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["ecfdebitline_cc_code"].ToString(), font8)));
                                ExpenseTable.AddCell(ExpensePCell);
                                ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["ecfdebitline_ou_code"].ToString(), font8)));
                                ExpenseTable.AddCell(ExpensePCell);
                                ExpenseTable.AddCell(new PdfPCell(new Phrase(objCmnFunctions.GetINRAmount(dtDebit.Rows[rows]["ecftravel_amount"].ToString()), font8)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });
                                if (tolamtDebit == 0)
                                {
                                    tolamtDebit = Convert.ToDecimal(dtDebit.Rows[rows]["ecftravel_amount"].ToString());
                                }
                                else
                                {
                                    tolamtDebit = tolamtDebit + Convert.ToDecimal(dtDebit.Rows[rows]["ecftravel_amount"].ToString());
                                }
                            }
                            ExpenseTable.AddCell(new PdfPCell(new Phrase("TOTAL FROM DEBIT TOTAL", fontTitle11)) { Colspan = 7, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE });
                            ExpenseTable.AddCell(new PdfPCell(new Phrase(objCmnFunctions.GetINRAmount(tolamtDebit.ToString()), fontTitle11)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });
                            ExpenseTable.SpacingBefore = 15f;
                            ExpenseTable.WidthPercentage = 100;

                            //--------------------------------------------ADD Payment Details---------------------------------------
                            PdfPTable PaymentTable = new PdfPTable(5);
                            PdfPCell PaymentPCell = null;
                            PaymentTable.AddCell(new PdfPCell(new Phrase("Payment Details", fontTitle11)) { Colspan = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                            PaymentTable.AddCell(new PdfPCell(new Phrase("Pay To", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                            PaymentTable.AddCell(new PdfPCell(new Phrase("Beneficiary Name", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                            PaymentTable.AddCell(new PdfPCell(new Phrase("Pay Mode", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                            PaymentTable.AddCell(new PdfPCell(new Phrase("Bank A/C No./Payment Ref No.", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                            PaymentTable.AddCell(new PdfPCell(new Phrase("Credit Amount", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                            DataTable dtPayment = new DataTable();
                            dtPayment = EOWDataModel.GetprintPayment(ecfgid.ToString());
                            decimal tolamtPayment = 0;
                            for (int rows = 0; rows < dtPayment.Rows.Count; rows++)
                            {
                                string empsup = dtPayment.Rows[rows]["ecf_supplier_employee"].ToString();
                                if (empsup == "E")
                                {
                                    empsup = "Employee";
                                }
                                else
                                {
                                    empsup = "Supplier";
                                }
                                PaymentPCell = new PdfPCell(new Phrase(new Chunk(empsup, font8)));
                                PaymentTable.AddCell(PaymentPCell);
                                PaymentPCell = new PdfPCell(new Phrase(new Chunk(dtPayment.Rows[rows]["ecfcreditline_beneficiary"].ToString(), font8)));
                                PaymentTable.AddCell(PaymentPCell);
                                PaymentPCell = new PdfPCell(new Phrase(new Chunk(dtPayment.Rows[rows]["ecfcreditline_pay_mode"].ToString(), font8)));
                                PaymentTable.AddCell(PaymentPCell);
                                PaymentPCell = new PdfPCell(new Phrase(new Chunk(dtPayment.Rows[rows]["ecfcreditline_ref_no"].ToString(), font8)));
                                PaymentTable.AddCell(PaymentPCell);
                                PaymentTable.AddCell(new PdfPCell(new Phrase(objCmnFunctions.GetINRAmount(dtPayment.Rows[rows]["ecfcreditline_amount"].ToString()), font8)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });

                                if (tolamtPayment == 0)
                                {
                                    tolamtPayment = Convert.ToDecimal(dtPayment.Rows[rows]["ecfcreditline_amount"].ToString());
                                }
                                else
                                {
                                    tolamtPayment = tolamtPayment + Convert.ToDecimal(dtPayment.Rows[rows]["ecfcreditline_amount"].ToString());
                                }
                            }
                            PaymentTable.AddCell(new PdfPCell(new Phrase("TOTAL FROM CREDIT TOTAL", fontTitle11)) { Colspan = 4, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE });
                            PaymentTable.AddCell(new PdfPCell(new Phrase(objCmnFunctions.GetINRAmount(tolamtPayment.ToString()), fontTitle11)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });
                            PaymentTable.SpacingBefore = 10f;
                            PaymentTable.WidthPercentage = 100;

                            PdfPTable tableamtdesc = new PdfPTable(2);
                            tableamtdesc.WidthPercentage = 100;
                            tableamtdesc.SpacingBefore = 10f;
                            tableamtdesc.AddCell(new PdfPCell(new Phrase("Amount in Words", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableamtdesc.AddCell(new PdfPCell(new Phrase("ECF Description", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                            tableamtdesc.AddCell(new PdfPCell(new Phrase(objModel.Ecf_Amountinword, font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 10f });
                            tableamtdesc.AddCell(new PdfPCell(new Phrase(objModel.Ecf_dcsc, font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 10f });

                            PdfPTable tableDeclar = new PdfPTable(2);
                            tableDeclar.WidthPercentage = 100;
                            tableDeclar.SpacingBefore = 10f;
                            tableDeclar.AddCell(new PdfPCell(new Phrase("Declaration", fontTitle11)) { Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                            tableDeclar.AddCell(new PdfPCell(new Phrase("Raiser Declaration", fontTitle11)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = PdfPCell.LEFT_BORDER });
                            tableDeclar.AddCell(new PdfPCell(new Phrase("Verifier Declaration", fontTitle11)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER });
                            //tableDeclar.AddCell(new PdfPCell(new Phrase("I confirm that I have incurred the expenditure wholly and exclusively for business purposes only.", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 2f, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER });
                            //tableDeclar.AddCell(new PdfPCell(new Phrase("I have verified the expense claim and approved on the basis that this Expense is incurred wholly and exclusively for Business Purposes.", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 2f, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER });

                            tableDeclar.AddCell(new PdfPCell(new Phrase(objModel.dclnotesub, font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 2f, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER });
                            tableDeclar.AddCell(new PdfPCell(new Phrase(objModel.dclnoteapp, font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 2f, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER });


                            //--------------------------------------------ADD Authorization Approvals Details---------------------------------------

                            PdfPTable AuthorizationTable = new PdfPTable(5);
                            PdfPCell AuthorizationPCell = null;
                            AuthorizationTable.AddCell(new PdfPCell(new Phrase("ECF Authorization Details", fontTitle11)) { Colspan = 8, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                            AuthorizationTable.AddCell(new PdfPCell(new Phrase("Employee Name", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                            AuthorizationTable.AddCell(new PdfPCell(new Phrase("Designation", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                            AuthorizationTable.AddCell(new PdfPCell(new Phrase("Action Date", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                            AuthorizationTable.AddCell(new PdfPCell(new Phrase("Status", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                            AuthorizationTable.AddCell(new PdfPCell(new Phrase("Remarks", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                            EOW_DataModel objd = new EOW_DataModel();
                            List<ApproverHistry> lstaHistory = new List<ApproverHistry>();
                            lstaHistory = objd.GetecfappHistory(ecfgid.ToString(), ecfgid.ToString()).ToList();

                            for (int rows = 0; rows < lstaHistory.Count; rows++)
                            {
                                AuthorizationPCell = new PdfPCell(new Phrase(new Chunk(lstaHistory[rows].empcode, font8)));
                                AuthorizationTable.AddCell(AuthorizationPCell);
                                AuthorizationPCell = new PdfPCell(new Phrase(new Chunk(lstaHistory[rows].empname, font8)));
                                AuthorizationTable.AddCell(AuthorizationPCell);
                                AuthorizationPCell = new PdfPCell(new Phrase(new Chunk(lstaHistory[rows].statusdate, font8)));
                                AuthorizationTable.AddCell(AuthorizationPCell);
                                AuthorizationPCell = new PdfPCell(new Phrase(new Chunk(lstaHistory[rows].status, font8)));
                                AuthorizationTable.AddCell(AuthorizationPCell);
                                AuthorizationPCell = new PdfPCell(new Phrase(new Chunk(lstaHistory[rows].remarks, font8)));
                                AuthorizationTable.AddCell(AuthorizationPCell);
                            }
                            AuthorizationTable.SpacingBefore = 15f;
                            AuthorizationTable.WidthPercentage = 100;

                            PdfPTable tableSign = new PdfPTable(4);
                            tableSign.WidthPercentage = 100;
                            tableSign.SpacingBefore = 10f;
                            tableSign.AddCell(new PdfPCell(new Phrase("EPU Authentication", fontTitle11)) { Colspan = 4, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                            tableSign.AddCell(new PdfPCell(new Phrase("", font8)) { Colspan = 4, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = 10f, Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER });
                            tableSign.AddCell(new PdfPCell(new Phrase("Emp ID & Emp Name ", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 30f, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER });

                            tableSign.AddCell(new PdfPCell(new Phrase("Maker ID", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 30f, Border = PdfPCell.BOTTOM_BORDER });
                            tableSign.AddCell(new PdfPCell(new Phrase("Authoriser ID", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 30f, Border = PdfPCell.BOTTOM_BORDER });
                            tableSign.AddCell(new PdfPCell(new Phrase("Verifier ID & Name", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 30f, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER });

                            PdfPTable tablenote = new PdfPTable(1);
                            tablenote.WidthPercentage = 100;
                            tablenote.SpacingBefore = 10f;
                            tablenote.AddCell(new PdfPCell(new Phrase("* This is an electronically generated form and does not require signature. The IDs mentioned there in tantamount to signature of the relevant employees", fontTitlenote)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = 0 });

                            document.Add(HeaderTable);
                            document.Add(t);
                            if (type == "S")
                            {
                                document.Add(InvoiceTable);
                            }
                            document.Add(ExpenseTable);
                            document.Add(PaymentTable);
                            document.Add(tableamtdesc);
                            document.Add(tableDeclar);
                            document.Add(AuthorizationTable);
                            //document.Add(tableSign);
                            document.Add(tablenote);
                            string updateresulr = DataModel.printstatus(ecfgid);
                        }
                    }
                    string fileprintname = "Print ECF " + DateTime.Now.ToString("ddMMyyyy") + DateTime.Now.ToString("HHMMss") + ".pdf";
                    document.Close();
                    writer.Close();
                    Response.ContentType = "pdf/application";
                    Response.AddHeader("content-disposition",
                    "attachment;filename=" + fileprintname);
                    Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
        }

        public byte[] challanPrintmail(string ecfgid, string type)
        {
            byte[] bytes = null;
            try
            {
                iTextSharp.text.Font fontTitle = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle1 = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle11 = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle2 = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle3 = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitlenote = FontFactory.GetFont("Arial", 6, iTextSharp.text.Font.NORMAL, BaseColor.RED);
                using (MemoryStream ms = new MemoryStream())
                {
                    Document document = new Document(PageSize.A4, 10F, 10F, 10F, 0F);
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);
                    document.Open();

                    printdatamodel objModel = new printdatamodel();
                    if (type == "S")
                    {
                        objModel = EOWDataModel.SelectEmployeeSearch(ecfgid, "S");
                    }
                    else
                    {
                        objModel = EOWDataModel.SelectPrintGeneral(ecfgid, "E");
                    }

                    PrintModel pm = new PrintModel();
                    printdatamodel objModeldatadecl = new printdatamodel();
                    objModeldatadecl = pm.ToGetDeclaration(ecfgid);
                    objModel.dclnotesub = objModeldatadecl.dclnotesub;
                    objModel.dclnoteapp = objModeldatadecl.dclnoteapp;

                    document.NewPage();
                    PdfPTable HeaderTable = new PdfPTable(10);
                    HeaderTable.WidthPercentage = 100;

                    //--------------------------------------------ADD Invoice Details---------------------------------------
                    // BackgroundColor = BaseColor.LIGHT_GRAY 
                    PdfPCell Headerlogo = new PdfPCell();
                    string Headerlogoath = Path.Combine(System.Web.HttpContext.Current.Server.MapPath(@"~/Content/images"), "Fullerton.bmp");
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Headerlogoath);
                    image.WidthPercentage = 90;
                    Headerlogo.AddElement(image);
                    Headerlogo.HorizontalAlignment = Element.ALIGN_CENTER;
                    Headerlogo.VerticalAlignment = Element.ALIGN_MIDDLE;
                    Headerlogo.Colspan = 3;
                    Headerlogo.Rowspan = 2;

                    string ecfno = objModel.Ecf_No;
                    PdfContentByte cb = new PdfContentByte(writer);
                    Barcode128 code128 = new Barcode128();
                    code128.CodeType = Barcode.CODE128_UCC;
                    code128.Code = ecfno;
                    iTextSharp.text.Image image128 = code128.CreateImageWithBarcode(cb, null, null);
                    image128.ScaleAbsolute(110f, 155.25f);

                    PdfPCell ECFNo = new PdfPCell();
                    ECFNo.HorizontalAlignment = Element.ALIGN_CENTER;
                    ECFNo.VerticalAlignment = Element.ALIGN_MIDDLE;
                    ECFNo.AddElement(image128);
                    HeaderTable.AddCell(new PdfPCell(new Phrase(objModel.Header, fontTitle2)) { Colspan = 7, Rowspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    HeaderTable.AddCell(Headerlogo);

                    PdfPTable t = new PdfPTable(9);
                    t.WidthPercentage = 100;

                    t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_No, fontTitle3)) { Rowspan = 3, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                    t.AddCell(new PdfPCell(new Phrase("Employee ID", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.EmployeeId, fontTitle1)) { Colspan = 2 });
                    t.AddCell(new PdfPCell(new Phrase("Invoice Type", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Invoice_type, fontTitle1)));

                    t.AddCell(new PdfPCell(new Phrase("ECF Date", fontTitle1)) { Colspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                    t.AddCell(new PdfPCell(new Phrase("Name", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.EmployeeName, fontTitle1)) { Colspan = 2 });
                    t.AddCell(new PdfPCell(new Phrase("Expense Type", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Expense_type, fontTitle1)));

                    t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_Date, fontTitle3)) { Rowspan = 2, Colspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                    t.AddCell(new PdfPCell(new Phrase("Title", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Title, fontTitle1)) { Colspan = 2 });
                    t.AddCell(new PdfPCell(new Phrase("Amort", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_Amort, fontTitle1)));

                    t.AddCell(new PdfPCell(ECFNo) { Rowspan = 3, Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingLeft = 7f, PaddingBottom = 7f, PaddingTop = 7f, PaddingRight = 7f });

                    t.AddCell(new PdfPCell(new Phrase("Department", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Department, fontTitle1)) { Colspan = 2 });
                    t.AddCell(new PdfPCell(new Phrase("For Ex", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_forex, fontTitle1)));

                    t.AddCell(new PdfPCell(new Phrase("ECF Amount", fontTitle1)) { Colspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                    t.AddCell(new PdfPCell(new Phrase("Location Code", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.LocationCode, fontTitle1)) { Colspan = 2 });
                    t.AddCell(new PdfPCell(new Phrase("Utility", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_Utility, fontTitle1)));

                    t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_Amount, fontTitle3)) { Rowspan = 2, Colspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE });

                    t.AddCell(new PdfPCell(new Phrase("PO / WO No.", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Invoice_powono, fontTitle1)) { Colspan = 2 });
                    t.AddCell(new PdfPCell(new Phrase("Payment Adjst", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    t.AddCell(new PdfPCell(new Phrase(objModel.Ecf_PaymentAdjst, fontTitle1)));

                    //--------------------------------------------ADD Invoice Details---------------------------------------
                    Font font8 = FontFactory.GetFont("ARIAL", 7);
                    PdfPTable InvoiceTable = new PdfPTable(5);
                    PdfPCell InvoicePCell = null;
                    if (type == "S")
                    {
                        InvoiceTable.AddCell(new PdfPCell(new Phrase("Invoice Details ", fontTitle11)) { Colspan = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                        InvoiceTable.AddCell(new PdfPCell(new Phrase("Vendor Code", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                        InvoiceTable.AddCell(new PdfPCell(new Phrase("Vendor Name", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                        InvoiceTable.AddCell(new PdfPCell(new Phrase("Invoice No.", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                        InvoiceTable.AddCell(new PdfPCell(new Phrase("Invoice Date", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                        InvoiceTable.AddCell(new PdfPCell(new Phrase("Invoice Amount", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                        DataTable dtinvoice = new DataTable();
                        dtinvoice = EOWDataModel.GetSupplieinvoice(ecfgid);
                        decimal tolamtinvoice = 0;
                        for (int rows = 0; rows < dtinvoice.Rows.Count; rows++)
                        {
                            InvoicePCell = new PdfPCell(new Phrase(new Chunk(dtinvoice.Rows[rows]["supplierheader_suppliercode"].ToString(), font8)));
                            InvoiceTable.AddCell(InvoicePCell);
                            InvoicePCell = new PdfPCell(new Phrase(new Chunk(dtinvoice.Rows[rows]["supplierheader_name"].ToString(), font8)));
                            InvoiceTable.AddCell(InvoicePCell);
                            InvoicePCell = new PdfPCell(new Phrase(new Chunk(dtinvoice.Rows[rows]["invoice_no"].ToString(), font8)));
                            InvoiceTable.AddCell(InvoicePCell);
                            InvoicePCell = new PdfPCell(new Phrase(new Chunk(dtinvoice.Rows[rows]["invoice_date"].ToString(), font8)));
                            InvoiceTable.AddCell(InvoicePCell);
                            InvoiceTable.AddCell(new PdfPCell(new Phrase(objCmnFunctions.GetINRAmount(dtinvoice.Rows[rows]["invoice_amount"].ToString()), font8)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });
                            if (tolamtinvoice == 0)
                            {
                                tolamtinvoice = Convert.ToDecimal(dtinvoice.Rows[rows]["invoice_amount"].ToString());
                            }
                            else
                            {
                                tolamtinvoice = tolamtinvoice + Convert.ToDecimal(dtinvoice.Rows[rows]["invoice_amount"].ToString());
                            }
                        }
                        InvoiceTable.AddCell(new PdfPCell(new Phrase("TOTAL FROM INVOICE TOTAL", fontTitle11)) { Colspan = 4, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE });
                        InvoiceTable.AddCell(new PdfPCell(new Phrase(objCmnFunctions.GetINRAmount(tolamtinvoice.ToString()), fontTitle11)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });
                        InvoiceTable.SpacingBefore = 15f;
                        InvoiceTable.WidthPercentage = 100;
                    }
                    //--------------------------------------------ADD Expense Details---------------------------------------
                    PdfPTable ExpenseTable = new PdfPTable(8);
                    PdfPCell ExpensePCell = null;
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("Expense / Asset Details", fontTitle11)) { Colspan = 8, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("Nature of Expense / Asset Description", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("Category", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("Sub Category", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("Product Code", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("Business Segment", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("Cost Center", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("Branch Code", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("Debit Amount", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                    DataTable dtDebit = new DataTable();
                    dtDebit = EOWDataModel.GetprintExpensesupplier(ecfgid.ToString());
                    decimal tolamtDebit = 0;
                    for (int rows = 0; rows < dtDebit.Rows.Count; rows++)
                    {
                        if (Convert.ToString(dtDebit.Rows[rows]["ecfdebitline_category_type"].ToString()) == "A")
                        {
                            ExpensePCell = new PdfPCell(new Phrase(new Chunk("", font8)));
                            ExpenseTable.AddCell(ExpensePCell);
                            ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["assetcategory_name"].ToString(), font8)));
                            ExpenseTable.AddCell(ExpensePCell);
                            ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["asset_description"].ToString(), font8)));
                            ExpenseTable.AddCell(ExpensePCell);
                        }
                        else
                        {
                            ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["expnature_name"].ToString(), font8)));
                            ExpenseTable.AddCell(ExpensePCell);
                            ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["expcat_name"].ToString(), font8)));
                            ExpenseTable.AddCell(ExpensePCell);
                            ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["expsubcat_name"].ToString(), font8)));
                            ExpenseTable.AddCell(ExpensePCell);
                        }
                        ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["ecfdebitline_product_code"].ToString(), font8)));
                        ExpenseTable.AddCell(ExpensePCell);
                        ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["ecfdebitline_fc_code"].ToString(), font8)));
                        ExpenseTable.AddCell(ExpensePCell);
                        ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["ecfdebitline_cc_code"].ToString(), font8)));
                        ExpenseTable.AddCell(ExpensePCell);
                        ExpensePCell = new PdfPCell(new Phrase(new Chunk(dtDebit.Rows[rows]["ecfdebitline_ou_code"].ToString(), font8)));
                        ExpenseTable.AddCell(ExpensePCell);
                        ExpenseTable.AddCell(new PdfPCell(new Phrase(objCmnFunctions.GetINRAmount(dtDebit.Rows[rows]["ecftravel_amount"].ToString()), font8)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });
                        if (tolamtDebit == 0)
                        {
                            tolamtDebit = Convert.ToDecimal(dtDebit.Rows[rows]["ecftravel_amount"].ToString());
                        }
                        else
                        {
                            tolamtDebit = tolamtDebit + Convert.ToDecimal(dtDebit.Rows[rows]["ecftravel_amount"].ToString());
                        }
                    }
                    ExpenseTable.AddCell(new PdfPCell(new Phrase("TOTAL FROM DEBIT TOTAL", fontTitle11)) { Colspan = 7, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE });
                    ExpenseTable.AddCell(new PdfPCell(new Phrase(objCmnFunctions.GetINRAmount(tolamtDebit.ToString()), fontTitle11)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });
                    ExpenseTable.SpacingBefore = 15f;
                    ExpenseTable.WidthPercentage = 100;

                    //--------------------------------------------ADD Payment Details---------------------------------------
                    PdfPTable PaymentTable = new PdfPTable(5);
                    PdfPCell PaymentPCell = null;
                    PaymentTable.AddCell(new PdfPCell(new Phrase("Payment Details", fontTitle11)) { Colspan = 5, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    PaymentTable.AddCell(new PdfPCell(new Phrase("Pay To", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    PaymentTable.AddCell(new PdfPCell(new Phrase("Beneficiary Name", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    PaymentTable.AddCell(new PdfPCell(new Phrase("Pay Mode", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    PaymentTable.AddCell(new PdfPCell(new Phrase("Bank A/C No./Payment Ref No.", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    PaymentTable.AddCell(new PdfPCell(new Phrase("Credit Amount", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                    DataTable dtPayment = new DataTable();
                    dtPayment = EOWDataModel.GetprintPayment(ecfgid.ToString());
                    decimal tolamtPayment = 0;
                    for (int rows = 0; rows < dtPayment.Rows.Count; rows++)
                    {
                        string empsup = dtPayment.Rows[rows]["ecf_supplier_employee"].ToString();
                        if (empsup == "E")
                        {
                            empsup = "Employee";
                        }
                        else
                        {
                            empsup = "Supplier";
                        }
                        PaymentPCell = new PdfPCell(new Phrase(new Chunk(empsup, font8)));
                        PaymentTable.AddCell(PaymentPCell);
                        PaymentPCell = new PdfPCell(new Phrase(new Chunk(dtPayment.Rows[rows]["ecfcreditline_beneficiary"].ToString(), font8)));
                        PaymentTable.AddCell(PaymentPCell);
                        PaymentPCell = new PdfPCell(new Phrase(new Chunk(dtPayment.Rows[rows]["ecfcreditline_pay_mode"].ToString(), font8)));
                        PaymentTable.AddCell(PaymentPCell);
                        PaymentPCell = new PdfPCell(new Phrase(new Chunk(dtPayment.Rows[rows]["ecfcreditline_ref_no"].ToString(), font8)));
                        PaymentTable.AddCell(PaymentPCell);
                        PaymentTable.AddCell(new PdfPCell(new Phrase(objCmnFunctions.GetINRAmount(dtPayment.Rows[rows]["ecfcreditline_amount"].ToString()), font8)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });

                        if (tolamtPayment == 0)
                        {
                            tolamtPayment = Convert.ToDecimal(dtPayment.Rows[rows]["ecfcreditline_amount"].ToString());
                        }
                        else
                        {
                            tolamtPayment = tolamtPayment + Convert.ToDecimal(dtPayment.Rows[rows]["ecfcreditline_amount"].ToString());
                        }
                    }
                    PaymentTable.AddCell(new PdfPCell(new Phrase("TOTAL FROM CREDIT TOTAL", fontTitle11)) { Colspan = 4, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE });
                    PaymentTable.AddCell(new PdfPCell(new Phrase(objCmnFunctions.GetINRAmount(tolamtPayment.ToString()), fontTitle11)) { HorizontalAlignment = Element.ALIGN_RIGHT, VerticalAlignment = Element.ALIGN_MIDDLE });
                    PaymentTable.SpacingBefore = 10f;
                    PaymentTable.WidthPercentage = 100;

                    PdfPTable tableamtdesc = new PdfPTable(2);
                    tableamtdesc.WidthPercentage = 100;
                    tableamtdesc.SpacingBefore = 10f;
                    tableamtdesc.AddCell(new PdfPCell(new Phrase("Amount in Words", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableamtdesc.AddCell(new PdfPCell(new Phrase("ECF Description", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    tableamtdesc.AddCell(new PdfPCell(new Phrase(objModel.Ecf_Amountinword, font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 10f });
                    tableamtdesc.AddCell(new PdfPCell(new Phrase(objModel.Ecf_dcsc, font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 10f });

                    PdfPTable tableDeclar = new PdfPTable(2);
                    tableDeclar.WidthPercentage = 100;
                    tableDeclar.SpacingBefore = 10f;
                    tableDeclar.AddCell(new PdfPCell(new Phrase("Declaration", fontTitle11)) { Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    tableDeclar.AddCell(new PdfPCell(new Phrase("Raiser Declaration", fontTitle11)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = PdfPCell.LEFT_BORDER });
                    tableDeclar.AddCell(new PdfPCell(new Phrase("Verifier Declaration", fontTitle11)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER });
                    //tableDeclar.AddCell(new PdfPCell(new Phrase("I confirm that I have incurred the expenditure wholly and exclusively for business purposes only.", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 2f, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER });
                    //tableDeclar.AddCell(new PdfPCell(new Phrase("I have verified the expense claim and approved on the basis that this Expense is incurred wholly and exclusively for Business Purposes.", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 2f, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER });

                    tableDeclar.AddCell(new PdfPCell(new Phrase(objModel.dclnotesub, font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 2f, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER });
                    tableDeclar.AddCell(new PdfPCell(new Phrase(objModel.dclnoteapp, font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 2f, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER });


                    //--------------------------------------------ADD Authorization Approvals Details---------------------------------------

                    PdfPTable AuthorizationTable = new PdfPTable(5);
                    PdfPCell AuthorizationPCell = null;
                    AuthorizationTable.AddCell(new PdfPCell(new Phrase("ECF Authorization Details", fontTitle11)) { Colspan = 8, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    AuthorizationTable.AddCell(new PdfPCell(new Phrase("Employee Name", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    AuthorizationTable.AddCell(new PdfPCell(new Phrase("Designation", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    AuthorizationTable.AddCell(new PdfPCell(new Phrase("Action Date", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    AuthorizationTable.AddCell(new PdfPCell(new Phrase("Status", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    AuthorizationTable.AddCell(new PdfPCell(new Phrase("Remarks", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                    EOW_DataModel objd = new EOW_DataModel();
                    List<ApproverHistry> lstaHistory = new List<ApproverHistry>();
                    lstaHistory = objd.GetecfappHistory(ecfgid.ToString(), ecfgid.ToString()).ToList();

                    for (int rows = 0; rows < lstaHistory.Count; rows++)
                    {
                        AuthorizationPCell = new PdfPCell(new Phrase(new Chunk(lstaHistory[rows].empcode, font8)));
                        AuthorizationTable.AddCell(AuthorizationPCell);
                        AuthorizationPCell = new PdfPCell(new Phrase(new Chunk(lstaHistory[rows].empname, font8)));
                        AuthorizationTable.AddCell(AuthorizationPCell);
                        AuthorizationPCell = new PdfPCell(new Phrase(new Chunk(lstaHistory[rows].statusdate, font8)));
                        AuthorizationTable.AddCell(AuthorizationPCell);
                        AuthorizationPCell = new PdfPCell(new Phrase(new Chunk(lstaHistory[rows].status, font8)));
                        AuthorizationTable.AddCell(AuthorizationPCell);
                        AuthorizationPCell = new PdfPCell(new Phrase(new Chunk(lstaHistory[rows].remarks, font8)));
                        AuthorizationTable.AddCell(AuthorizationPCell);
                    }
                    AuthorizationTable.SpacingBefore = 15f;
                    AuthorizationTable.WidthPercentage = 100;


                    PdfPTable tableSign = new PdfPTable(4);
                    tableSign.WidthPercentage = 100;
                    tableSign.SpacingBefore = 10f;
                    tableSign.AddCell(new PdfPCell(new Phrase("EPU Authentication", fontTitle11)) { Colspan = 4, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    tableSign.AddCell(new PdfPCell(new Phrase("", font8)) { Colspan = 4, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingTop = 10f, Border = PdfPCell.LEFT_BORDER | PdfPCell.RIGHT_BORDER });
                    tableSign.AddCell(new PdfPCell(new Phrase("Emp ID & Emp Name ", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 30f, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.LEFT_BORDER });

                    tableSign.AddCell(new PdfPCell(new Phrase("Maker ID", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 30f, Border = PdfPCell.BOTTOM_BORDER });
                    tableSign.AddCell(new PdfPCell(new Phrase("Authoriser ID", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 30f, Border = PdfPCell.BOTTOM_BORDER });
                    tableSign.AddCell(new PdfPCell(new Phrase("Verifier ID & Name", font8)) { Rowspan = 3, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, PaddingBottom = 10f, PaddingTop = 30f, Border = PdfPCell.BOTTOM_BORDER | PdfPCell.RIGHT_BORDER });

                    PdfPTable tablenote = new PdfPTable(1);
                    tablenote.WidthPercentage = 100;
                    tablenote.SpacingBefore = 10f;
                    tablenote.AddCell(new PdfPCell(new Phrase("* This is an electronically generated form and does not require signature. The IDs mentioned there in tantamount to signature of the relevant employees", fontTitlenote)) { HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, Border = 0 });

                    document.Add(HeaderTable);
                    document.Add(t);
                    if (type == "S")
                    {
                        document.Add(InvoiceTable);
                    }
                    document.Add(ExpenseTable);
                    document.Add(PaymentTable);
                    document.Add(tableamtdesc);
                    document.Add(tableDeclar);
                    document.Add(AuthorizationTable);
                    //document.Add(tableSign);
                    document.Add(tablenote);
                    document.Close();
                    writer.Close();

                    bytes = ms.ToArray();
                    return bytes;
                }
            }
            catch (Exception ex)
            {
                return bytes;
            }
            return bytes;
        }
    }
}
