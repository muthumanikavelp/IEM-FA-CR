using IEM.Areas.FLEXIBUY.Models;
using IEM.Common;
//using iTextSharp.text;
//using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
//using Rotativa;
//using RazorPDF;
using iTextSharp.text;
using System.Web.Hosting;
using iTextSharp.text.pdf;
using System.Data;
using System.Globalization;
using Newtonsoft.Json;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class PrintController : Controller
    {
        ErrorLog objErrorLog = new ErrorLog();
         private PrintRepository objRep;
        public PrintController()
            : this(new PrintDataModel())
        {

        }
        public PrintController(PrintRepository objModel)
        {
            objRep = objModel;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CBFSummary() 
        {
            Session["CBFNumber"] = null;
            return View();
        }
        public JsonResult GetCBFDetails()  
        {
            List<CbfPrintEntity> lst = new List<CbfPrintEntity>();
            try
            {  
                  lst = objRep.GetCBFSummary().ToList();
                  var js= Json(lst,JsonRequestBehavior.AllowGet);
                  js.MaxJsonLength = 2147483647;
                  return js;   
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(lst, JsonRequestBehavior.AllowGet);     
            }
        }
        public ActionResult PrintCbfIndex(string CBFNumber = null, string viewfor = null)
        {
            if (CBFNumber != null)
            {
                Session["CBFNumber"] = CBFNumber;
            }
            CbfPrintEntity objCbfPrintEntity = new CbfPrintEntity();
            objCbfPrintEntity = objRep.GetCBFData(CBFNumber);
            objCbfPrintEntity.CBFDetailsList = objRep.GetCBFDetailsList(CBFNumber).ToList();
            objCbfPrintEntity.ApprovalsList = objRep.GetApproverList(CBFNumber, "CBF").ToList();
            return View(objCbfPrintEntity);
           // return new RazorPDF.PdfResult(objCbfPrintEntity, "PrintCbfIndex");
        }
     
        public ActionResult POSummary() 
        {
            return View();
        }
        public JsonResult GetPODetails() 
        {
            List<POPrintEntity> lst = new List<POPrintEntity>();
            string data0 = string.Empty;
            try
            {
                lst = objRep.GetPOSummary().ToList();
                var js= Json(lst, JsonRequestBehavior.AllowGet);
                js.MaxJsonLength = 2147483647;
                return js;            
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult PrintPOIndex(string PONumber = null)  
        {
            POPrintEntity objPOPrintEntity = new POPrintEntity();
            objPOPrintEntity = objRep.GetPOData(PONumber); 
            objPOPrintEntity.PODetailsList = objRep.GetPODetailsList(PONumber).ToList();
            objPOPrintEntity.ApprovalsList = objRep.GetApproverList(PONumber, "PO").ToList();
            return View(objPOPrintEntity);
            // return new RazorPDF.PdfResult(objCbfPrintEntity, "PrintCbfIndex");
        }
        public ActionResult WOSummary() 
        {
            return View();
        }
        public JsonResult GetWODetails() 
        {
            List<WOPrintEntity> lst = new List<WOPrintEntity>();
            try
            {
                lst = objRep.GetWOSummary().ToList(); 
                var js= Json(lst, JsonRequestBehavior.AllowGet);
                js.MaxJsonLength = 2147483647;
                return js;   
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(lst, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult PrintWOIndex(string WONumber = null) 
        {
            WOPrintEntity objWOPrintEntity = new WOPrintEntity();
            objWOPrintEntity = objRep.GetWOData(WONumber);
            objWOPrintEntity.WODetailsList = objRep.GetWODetailsList(WONumber).ToList();
            objWOPrintEntity.ApprovalsList = objRep.GetApproverList(WONumber, "WO").ToList();
            return View(objWOPrintEntity);
            // return new RazorPDF.PdfResult(objCbfPrintEntity, "PrintCbfIndex");
        }
        //public ActionResult PDFPrintCBF(string CBFNumber = null)
        //{
        //      CbfPrintEntity objCbfPrintEntity = new CbfPrintEntity();
        //      try
        //      {
        //          objCbfPrintEntity = objRep.GetCBFData(CBFNumber);
        //          objCbfPrintEntity.CBFDetailsList = objRep.GetCBFDetailsList(CBFNumber).ToList();
        //          objCbfPrintEntity.ApprovalsList = objRep.GetApproverList(CBFNumber, "CBF").ToList();
        //         return new ViewAsPdf("PrintCbfIndex", objCbfPrintEntity);
        //        //  return new RazorPDF.PdfResult(objCbfPrintEntity,"PrintCbfIndex");
        //      }
        //      catch (Exception ex)
        //      {
        //          objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //          return View("PrintCbfIndex", objCbfPrintEntity);
        //      }
        //}
        //public ActionResult PDFPrintPO(string PONumber = null) 
        //{
        //    POPrintEntity objPOPrintEntity = new POPrintEntity();
          
        //    try{
        //            objPOPrintEntity = objRep.GetPOData(PONumber);
        //            objPOPrintEntity.PODetailsList = objRep.GetPODetailsList(PONumber).ToList();
        //            objPOPrintEntity.ApprovalsList = objRep.GetApproverList(PONumber, "PO").ToList();
        //            return new ActionAsPdf("PrintPOIndex", objPOPrintEntity);
        //      }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //        return View("PrintPOIndex", objPOPrintEntity);
        //    }
        //}
        //public ActionResult PDFPrintWO(string WONumber = null)  
        //{
        //    WOPrintEntity objWOPrintEntity = new WOPrintEntity();
        //    try
        //    {
        //        objWOPrintEntity = objRep.GetWOData(WONumber);
        //        objWOPrintEntity.WODetailsList = objRep.GetWODetailsList(WONumber).ToList();
        //        objWOPrintEntity.ApprovalsList = objRep.GetApproverList(WONumber, "WO").ToList();
        //        return new ActionAsPdf("PrintWOIndex", objWOPrintEntity);
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //        return View("PrintWOIndex", objWOPrintEntity);
        //    }
          
        //}
        public void PrintPDFForCBF(string CBFNumber = null)  
        {
            try
            {
                CbfPrintEntity objCbfPrintEntity = new CbfPrintEntity();
                objCbfPrintEntity = objRep.GetCBFData(CBFNumber);
                objCbfPrintEntity.CBFDetailsList = objRep.GetCBFDetailsList(CBFNumber).ToList();
                objCbfPrintEntity.ApprovalsList = objRep.GetApproverList(CBFNumber, "CBF").ToList();

                iTextSharp.text.Font fontTitle = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle1 = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle11 = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle2 = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle3 = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitlenote = FontFactory.GetFont("Arial", 6, iTextSharp.text.Font.NORMAL, BaseColor.RED);
                Font font8 = FontFactory.GetFont("ARIAL", 7);

                using (MemoryStream ms = new MemoryStream())
                {
                    Document document = new Document(PageSize.A4, 10F, 10F, 10F, 10F);
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);
                    document.Open();

                    document.NewPage();
                    PdfPTable HeaderTable = new PdfPTable(10);
                    HeaderTable.WidthPercentage = 100;

                    //Title & Logo
                    PdfPCell Headerlogo = new PdfPCell();
                    string Headerlogoath = Server.MapPath("~/Content/images/Fullerton.bmp");
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Headerlogoath);
                    image.WidthPercentage = 90;
                    Headerlogo.AddElement(image);
                    Headerlogo.HorizontalAlignment = Element.ALIGN_CENTER;
                    Headerlogo.VerticalAlignment = Element.ALIGN_MIDDLE;
                    Headerlogo.Colspan = 3;
                    Headerlogo.Rowspan = 2;

                    PdfPCell TitleText = new PdfPCell();
                    TitleText.HorizontalAlignment = Element.ALIGN_CENTER;
                    TitleText.VerticalAlignment = Element.ALIGN_MIDDLE;
                    HeaderTable.AddCell(new PdfPCell(new Phrase(" CAPITAL BUDGET FORM ", fontTitle2)) { Colspan = 7, Rowspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    HeaderTable.AddCell(Headerlogo);

                    //Header
                    PdfPTable CBFHeaderDetails = new PdfPTable(4);
                    CBFHeaderDetails.WidthPercentage = 100;
                    int[] CBFHeaderDetailsCellWidth = { 15, 30, 15, 40 };
                    CBFHeaderDetails.SetWidths(CBFHeaderDetailsCellWidth); 

                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("CBF Number", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objCbfPrintEntity.CBfNumber, fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("Branch Code", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objCbfPrintEntity.BranchCode, fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("CBF Date", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objCbfPrintEntity.CBfDate, fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("Branch Name", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objCbfPrintEntity.BranchName, fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("Initiator", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objCbfPrintEntity.RaiserName, fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("Branch Type", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objCbfPrintEntity.BranchType, fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("City Class", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objCbfPrintEntity.CityClass, fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("Department", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objCbfPrintEntity.Department, fontTitle1)));

                    //CBF Details
                    PdfPTable CBFTable = new PdfPTable(10);
                    CBFTable.WidthPercentage = 100;
                    int[] CBFTableCellWidth = { 5, 9, 20, 22, 8, 4, 10, 10, 7, 5 };
                    CBFTable.SetWidths(CBFTableCellWidth); 

                    PdfPCell CBFDetailsCell = null;
                    CBFTable.AddCell(new PdfPCell(new Phrase("CBF Details", fontTitle11)) { Colspan = 10, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    CBFTable.AddCell(new PdfPCell(new Phrase("S.No", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFTable.AddCell(new PdfPCell(new Phrase("Product Code", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFTable.AddCell(new PdfPCell(new Phrase("Product Name", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFTable.AddCell(new PdfPCell(new Phrase("Description", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFTable.AddCell(new PdfPCell(new Phrase("UOM", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFTable.AddCell(new PdfPCell(new Phrase("Qty", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFTable.AddCell(new PdfPCell(new Phrase("Unit Amount", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFTable.AddCell(new PdfPCell(new Phrase("Total Amount", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFTable.AddCell(new PdfPCell(new Phrase("FCCC", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFTable.AddCell(new PdfPCell(new Phrase("Budget Line", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                    for (int rows = 0; rows < objCbfPrintEntity.CBFDetailsList.Count; rows++)
                    {
                        CBFDetailsCell = new PdfPCell(new Phrase(new Chunk((rows + 1).ToString(), font8)));
                        CBFTable.AddCell(CBFDetailsCell);
                        CBFDetailsCell = new PdfPCell(new Phrase(new Chunk(objCbfPrintEntity.CBFDetailsList[rows].ProductCode.ToString(), font8)));
                        CBFTable.AddCell(CBFDetailsCell);
                        CBFDetailsCell = new PdfPCell(new Phrase(new Chunk(objCbfPrintEntity.CBFDetailsList[rows].ProductName.ToString(), font8)));
                        CBFTable.AddCell(CBFDetailsCell);
                        CBFDetailsCell = new PdfPCell(new Phrase(new Chunk(objCbfPrintEntity.CBFDetailsList[rows].ProductDescription.ToString(), font8)));
                        CBFTable.AddCell(CBFDetailsCell);
                        CBFDetailsCell = new PdfPCell(new Phrase(new Chunk(objCbfPrintEntity.CBFDetailsList[rows].UOM.ToString(), font8)));
                        CBFTable.AddCell(CBFDetailsCell);
                        CBFDetailsCell = new PdfPCell(new Phrase(new Chunk(objCbfPrintEntity.CBFDetailsList[rows].Qty.ToString(), font8)));
                        CBFTable.AddCell(CBFDetailsCell);
                        CBFDetailsCell = new PdfPCell(new Phrase(new Chunk(objCbfPrintEntity.CBFDetailsList[rows].UnitAmount.ToString(), font8)));
                        CBFTable.AddCell(CBFDetailsCell);
                        CBFDetailsCell = new PdfPCell(new Phrase(new Chunk(objCbfPrintEntity.CBFDetailsList[rows].TotalAmount.ToString(), font8)));
                        CBFTable.AddCell(CBFDetailsCell);
                        CBFDetailsCell = new PdfPCell(new Phrase(new Chunk(objCbfPrintEntity.CBFDetailsList[rows].FCCC.ToString(), font8)));
                        CBFTable.AddCell(CBFDetailsCell);
                        CBFDetailsCell = new PdfPCell(new Phrase(new Chunk(objCbfPrintEntity.CBFDetailsList[rows].BudgetLine.ToString(), font8)));
                        CBFTable.AddCell(CBFDetailsCell);
                    }
                    
                    //Add Mid Content
                    PdfPTable CBFHeaderTable = new PdfPTable(4);
                    CBFHeaderTable.WidthPercentage = 100;
                    CBFHeaderTable.DefaultCell.Border = Rectangle.NO_BORDER;
                   
                    CBFHeaderTable.AddCell(new PdfPCell(new Phrase("CBF Total Amount", fontTitle1)));
                    CBFHeaderTable.AddCell(new PdfPCell(new Phrase(objCbfPrintEntity.CBFHeaderAmount, fontTitle1)) { Colspan = 3, HorizontalAlignment = Element.ALIGN_LEFT });
                    CBFHeaderTable.AddCell(new PdfPCell(new Phrase("CBF Amount In Words", fontTitle1)));
                    CBFHeaderTable.AddCell(new PdfPCell(new Phrase(objCbfPrintEntity.CBFHeaderAmountInWords, fontTitle1)) { Colspan = 3, HorizontalAlignment = Element.ALIGN_LEFT });
                    CBFHeaderTable.AddCell(new PdfPCell(new Phrase("Deviation Amount", fontTitle1)));
                    CBFHeaderTable.AddCell(new PdfPCell(new Phrase(objCbfPrintEntity.CBFHeaderDevAmount, fontTitle1)) { Colspan = 3, HorizontalAlignment = Element.ALIGN_LEFT });
                    CBFHeaderTable.AddCell(new PdfPCell(new Phrase("Justification", fontTitle1)));
                    CBFHeaderTable.AddCell(new PdfPCell(new Phrase(objCbfPrintEntity.Justification, fontTitle1)) { Colspan = 3, HorizontalAlignment = Element.ALIGN_LEFT });
                    CBFHeaderTable.AddCell(new PdfPCell(new Phrase("Fincon budgeted", fontTitle1)));
                    CBFHeaderTable.AddCell(new PdfPCell(new Phrase(objCbfPrintEntity.BudgetedFlag, fontTitle1)) { Colspan = 3, HorizontalAlignment = Element.ALIGN_LEFT });

                    //Add Approver Details
                    PdfPTable CBFApproverTable = new PdfPTable(6);
                    CBFApproverTable.WidthPercentage = 100;
                    int[] CBFApproverTableCellWidth = { 5, 25, 17, 25, 18, 10 };
                    CBFApproverTable.SetWidths(CBFApproverTableCellWidth); 

                    PdfPCell CBFApproverCell = null;
                    CBFApproverTable.AddCell(new PdfPCell(new Phrase("Approver Details", fontTitle11)) { Colspan = 6, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    CBFApproverTable.AddCell(new PdfPCell(new Phrase("S.No", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFApproverTable.AddCell(new PdfPCell(new Phrase("Approver Name", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFApproverTable.AddCell(new PdfPCell(new Phrase("Approval Date", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFApproverTable.AddCell(new PdfPCell(new Phrase("Approval Stage", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFApproverTable.AddCell(new PdfPCell(new Phrase("Remarks", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFApproverTable.AddCell(new PdfPCell(new Phrase("Signature", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                   
                    for (int rows = 0; rows < objCbfPrintEntity.ApprovalsList.Count; rows++)
                    {
                        CBFApproverCell = new PdfPCell(new Phrase(new Chunk((rows + 1).ToString(), font8)));
                        CBFApproverTable.AddCell(CBFApproverCell);
                        CBFApproverCell = new PdfPCell(new Phrase(new Chunk(objCbfPrintEntity.ApprovalsList[rows].ApproverName.ToString(), font8)));
                        CBFApproverTable.AddCell(CBFApproverCell);
                        CBFApproverCell = new PdfPCell(new Phrase(new Chunk(objCbfPrintEntity.ApprovalsList[rows].ApprovalDate.ToString(), font8)));
                        CBFApproverTable.AddCell(CBFApproverCell);
                        CBFApproverCell = new PdfPCell(new Phrase(new Chunk(objCbfPrintEntity.ApprovalsList[rows].ApprovalStage.ToString(), font8)));
                        CBFApproverTable.AddCell(CBFApproverCell);
                        CBFApproverCell = new PdfPCell(new Phrase(new Chunk(objCbfPrintEntity.ApprovalsList[rows].Remarks.ToString(), font8)));
                        CBFApproverTable.AddCell(CBFApproverCell);
                        string ApproverSign = Server.MapPath("~/Content/signatures/" + objCbfPrintEntity.ApprovalsList[rows].EmpCode.ToString().Replace(" ", "") + ".png");
                        if (System.IO.File.Exists(ApproverSign))
                        {
                            iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(ApproverSign);
                            //  image1.WidthPercentage = 10;
                            image1.ScaleToFit(50, 40);
                            CBFApproverCell.AddElement(image1);
                            CBFApproverTable.AddCell(CBFApproverCell);
                        }
                        else
                        {
                            CBFApproverTable.AddCell(new PdfPCell(new Phrase("", fontTitle11)));
                        }

                    }
                      
                    document.Add(HeaderTable);
                    document.Add(CBFHeaderDetails); 
                    document.Add(new Paragraph(" "));
                    document.Add(CBFTable);
                    document.Add(new Paragraph(" "));
                    document.Add(CBFHeaderTable);
                    document.Add(new Paragraph(" "));
                    document.Add(CBFApproverTable);

                    document.Close();
                    writer.Close();
                    string fileprintname = objCbfPrintEntity.CBfNumber;
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
        public void PrintPDFForPO(string PONumber = null)  
        {
            try
            {
                POPrintEntity objPOPrintEntity = new POPrintEntity();
                objPOPrintEntity = objRep.GetPOData(PONumber);
                int GetBranchIsSingle = objRep.GetBranchIsSingle(objPOPrintEntity.POGid);
                objPOPrintEntity.PODetailsList = objRep.GetPODetailsList(PONumber).ToList();
                objPOPrintEntity.ApprovalsList = objRep.GetApproverList(PONumber, "PO").ToList();
                objPOPrintEntity.ShipmentList = objRep.GetShipmentDetails(objPOPrintEntity.POGid).ToList();

                iTextSharp.text.Font fontTitle = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle1 = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle11 = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle2 = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle3 = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitlenote = FontFactory.GetFont("Arial", 6, iTextSharp.text.Font.NORMAL, BaseColor.RED);
                iTextSharp.text.Font fonttermsandcond = FontFactory.GetFont("Arial", 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK); 
                Font font8 = FontFactory.GetFont("ARIAL", 7);

                using (MemoryStream ms = new MemoryStream())
                {
                    Document document = new Document(PageSize.A4, 10F, 10F, 10F, 20F);
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);
                    document.Open();

                    writer.PageEvent = new PdfPageEventHelper();
                    document.NewPage();
                   
                    PdfPTable HeaderTable = new PdfPTable(10);
                    HeaderTable.WidthPercentage = 100;

                    //Title & Logo
                    PdfPCell Headerlogo = new PdfPCell();
                    string Headerlogoath = Server.MapPath("~/Content/images/Fullerton.bmp");
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Headerlogoath);
                    image.WidthPercentage = 90;
                    Headerlogo.AddElement(image);
                    Headerlogo.HorizontalAlignment = Element.ALIGN_CENTER;
                    Headerlogo.VerticalAlignment = Element.ALIGN_MIDDLE;
                    Headerlogo.Colspan = 3;
                    Headerlogo.Rowspan = 2;

                    PdfPCell TitleText = new PdfPCell();
                    TitleText.HorizontalAlignment = Element.ALIGN_CENTER;
                    TitleText.VerticalAlignment = Element.ALIGN_MIDDLE;
                    HeaderTable.AddCell(new PdfPCell(new Phrase(" PURCHASE ORDER ", fontTitle2)) { Colspan = 7, Rowspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    HeaderTable.AddCell(Headerlogo);

                    //Header
                    PdfPTable CBFHeaderDetails = new PdfPTable(4);
                    CBFHeaderDetails.WidthPercentage = 100;
                    int[] CBFHeaderDetailsCellWidth = { 15, 30, 15, 40 };
                    CBFHeaderDetails.SetWidths(CBFHeaderDetailsCellWidth);

                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("PO Number", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objPOPrintEntity.PONumber, fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("Vendor Name", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objPOPrintEntity.VendorName, fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("PO Date", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objPOPrintEntity.PODate, fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("Vendor Notes", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objPOPrintEntity.VendorNotes, fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("Initiator", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objPOPrintEntity.RaiserName, fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("Request For", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objPOPrintEntity.RequestFor, fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("CBF Number", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objPOPrintEntity.PODetailsList[0].POCBFNumber.ToString(), fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("CC ", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objPOPrintEntity.PODetailsList[0].fccc.ToString(), fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("Mode", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objPOPrintEntity.PODetailsList[0].cbfmode.ToString(), fontTitle1)));

                    if (objPOPrintEntity.PODetailsList[0].cbfmode == "PAR")
                    {
                        CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("PAR NO", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                        CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objPOPrintEntity.PODetailsList[0].parno.ToString(), fontTitle1)));
                    }
                    else if (objPOPrintEntity.PODetailsList[0].cbfmode == "PR")
                    {
                        CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("PR NO", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                        CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objPOPrintEntity.PODetailsList[0].prno.ToString(), fontTitle1)));
                    }
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("Branch", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objPOPrintEntity.PODetailsList[0].cbfbranch.ToString(), fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("Department Name", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objPOPrintEntity.deptname.ToString(), fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("PO Version", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objPOPrintEntity.PODetailsList[0].PoVersion.ToString(), fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("PO Amendment Date", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objPOPrintEntity.PODetailsList[0].AmendmentDate.ToString(), fontTitle1)));

                    //Vendor Address & Fullerton Address
                    PdfPTable CBFTable = new PdfPTable(3);
                    CBFTable.WidthPercentage = 100;
                    int[] CBFTableCellWidth = { 33,34,33 };
                    CBFTable.SetWidths(CBFTableCellWidth);

                    CBFTable.AddCell(new PdfPCell(new Phrase("Vendor Address", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFTable.AddCell(new PdfPCell(new Phrase("Deliver to", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFTable.AddCell(new PdfPCell(new Phrase("Billing Address", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                    var p = new Paragraph();
                    p.Font = fontTitle1;
                    p.Add(objPOPrintEntity.VendorName + "\n");
                    p.Add(objPOPrintEntity.VendorAddress + "\n");
                    p.Add(objPOPrintEntity.VendorCity + "\n");
                    p.Add(objPOPrintEntity.VendorState + "\n");
                    p.Add(objPOPrintEntity.VendorCountry + "\n");
                    p.Add("Pin Code : " + objPOPrintEntity.VendorPinCode + "\n");
                    p.Add("Phone : " + objPOPrintEntity.VendorPhone + "\n");
                    p.Add("Fax : " + objPOPrintEntity.VendorFax);
                    CBFTable.AddCell(p);
                    var p2 = new Paragraph();
                    p2.Font = fontTitle1;
                    PdfPTable POTableAnn = new PdfPTable(7);
                    if (GetBranchIsSingle > 1)
                    {
                        p2.Add("Multiple Branches. \n");
                        p2.Add("Refer Annexure \n");

                        //Annexure
                        POTableAnn.WidthPercentage = 100;
                        int[] POTableCellWidthAnn = { 5, 15, 45, 9, 15, 4, 7 };
                        POTableAnn.SetWidths(POTableCellWidthAnn);

                        PdfPCell PODetailsCellAnn = null;
                        POTableAnn.AddCell(new PdfPCell(new Phrase("Annexure", fontTitle11)) { Colspan = 7, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                        POTableAnn.AddCell(new PdfPCell(new Phrase("S.No", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                        POTableAnn.AddCell(new PdfPCell(new Phrase("Branch Name", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                        POTableAnn.AddCell(new PdfPCell(new Phrase("Branch Address", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                        POTableAnn.AddCell(new PdfPCell(new Phrase("Product Code", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                        POTableAnn.AddCell(new PdfPCell(new Phrase("Product Name", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                        POTableAnn.AddCell(new PdfPCell(new Phrase("Qty", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                        POTableAnn.AddCell(new PdfPCell(new Phrase("type", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                       
                        for (int rows = 0; rows < objPOPrintEntity.ShipmentList.Count; rows++)
                        {
                            PODetailsCellAnn = new PdfPCell(new Phrase(new Chunk((rows + 1).ToString(), font8)));
                            POTableAnn.AddCell(PODetailsCellAnn);
                            PODetailsCellAnn = new PdfPCell(new Phrase(new Chunk(objPOPrintEntity.ShipmentList[rows].BranchName.ToString(), font8)));
                            POTableAnn.AddCell(PODetailsCellAnn);
                            PODetailsCellAnn = new PdfPCell(new Phrase(new Chunk(objPOPrintEntity.ShipmentList[rows].BranchAddress.ToString(), font8)));
                            POTableAnn.AddCell(PODetailsCellAnn);
                            PODetailsCellAnn = new PdfPCell(new Phrase(new Chunk(objPOPrintEntity.ShipmentList[rows].ShipmentProdCode.ToString(), font8)));
                            POTableAnn.AddCell(PODetailsCellAnn);
                            PODetailsCellAnn = new PdfPCell(new Phrase(new Chunk(objPOPrintEntity.ShipmentList[rows].ShipmentprodName.ToString(), font8)));
                            POTableAnn.AddCell(PODetailsCellAnn);
                            PODetailsCellAnn = new PdfPCell(new Phrase(new Chunk(objPOPrintEntity.ShipmentList[rows].ShippedQty.ToString(), font8)));
                            POTableAnn.AddCell(PODetailsCellAnn);
                            PODetailsCellAnn = new PdfPCell(new Phrase(new Chunk(objPOPrintEntity.ShipmentList[rows].ShipmentType.ToString(), font8)));
                            POTableAnn.AddCell(PODetailsCellAnn);
                        }
                    }
                    else
                    {
                        p2.Add(System.Configuration.ConfigurationManager.AppSettings["CompanyFullName"].ToString() + "\n");
                        if (objPOPrintEntity.ShipmentList.Count > 0)
                        {
                            p2.Add(objPOPrintEntity.ShipmentList[0].BranchName);
                            p2.Add(objPOPrintEntity.ShipmentList[0].BranchAddress);
                        }
                        else
                        {
                            p2.Add("");
                            p2.Add("");
                        }

                    }
                    CBFTable.AddCell(p2);
                    var p1 = new Paragraph();
                    p1.Font = fontTitle1;
                    p1.Add(System.Configuration.ConfigurationManager.AppSettings["CompanyFullName"].ToString() + "\n");
                    p1.Add(System.Configuration.ConfigurationManager.AppSettings["MAddress1"].ToString() + ",\n");
                    p1.Add(System.Configuration.ConfigurationManager.AppSettings["MAddress2"].ToString() + ",\n");
                    p1.Add(System.Configuration.ConfigurationManager.AppSettings["MArea"].ToString() + ",\n");
                    p1.Add(System.Configuration.ConfigurationManager.AppSettings["MCity"].ToString() + "\n");
                    p1.Add(System.Configuration.ConfigurationManager.AppSettings["MPhoneNo"].ToString() + "\n");
                   // p1.Add("\n");
                   // p1.Add("Note to Supplier: Please forward your invoice and other documents to the above address ");
                    CBFTable.AddCell(p1);

                    //PO Details
                    PdfPTable POTable = new PdfPTable(11);
                    POTable.WidthPercentage = 100;
                    int[] POTableCellWidth = { 5, 9, 17, 13, 7, 4, 9, 8, 8, 9,11 };
                    POTable.SetWidths(POTableCellWidth);

                    PdfPCell PODetailsCell = null; 
                    POTable.AddCell(new PdfPCell(new Phrase("PO Details", fontTitle11)) { Colspan = 11, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    POTable.AddCell(new PdfPCell(new Phrase("S.No", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    POTable.AddCell(new PdfPCell(new Phrase("Product Code", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    POTable.AddCell(new PdfPCell(new Phrase("Product Name", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    POTable.AddCell(new PdfPCell(new Phrase("Description", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    POTable.AddCell(new PdfPCell(new Phrase("UOM", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    POTable.AddCell(new PdfPCell(new Phrase("Qty", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    POTable.AddCell(new PdfPCell(new Phrase("Unit Amount", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    POTable.AddCell(new PdfPCell(new Phrase("Discount", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    POTable.AddCell(new PdfPCell(new Phrase("Tax", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    POTable.AddCell(new PdfPCell(new Phrase("Others", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    POTable.AddCell(new PdfPCell(new Phrase("Total Amount", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                  

                    for (int rows = 0; rows < objPOPrintEntity.PODetailsList.Count; rows++)
                    {
                        PODetailsCell = new PdfPCell(new Phrase(new Chunk((rows + 1).ToString(), font8)));
                        POTable.AddCell(PODetailsCell);
                        PODetailsCell = new PdfPCell(new Phrase(new Chunk(objPOPrintEntity.PODetailsList[rows].ProductCode.ToString(), font8)));
                        POTable.AddCell(PODetailsCell);
                        PODetailsCell = new PdfPCell(new Phrase(new Chunk(objPOPrintEntity.PODetailsList[rows].ProductName.ToString(), font8)));
                        POTable.AddCell(PODetailsCell);
                        PODetailsCell = new PdfPCell(new Phrase(new Chunk(objPOPrintEntity.PODetailsList[rows].ProductDescription.ToString(), font8)));
                        POTable.AddCell(PODetailsCell);
                        PODetailsCell = new PdfPCell(new Phrase(new Chunk(objPOPrintEntity.PODetailsList[rows].UOM.ToString(), font8)));
                        POTable.AddCell(PODetailsCell);
                        PODetailsCell = new PdfPCell(new Phrase(new Chunk(objPOPrintEntity.PODetailsList[rows].Qty.ToString(), font8)));
                        POTable.AddCell(PODetailsCell);
                        PODetailsCell = new PdfPCell(new Phrase(new Chunk(objPOPrintEntity.PODetailsList[rows].UnitAmount.ToString(), font8)));
                        POTable.AddCell(PODetailsCell);
                        PODetailsCell = new PdfPCell(new Phrase(new Chunk(objPOPrintEntity.PODetailsList[rows].Discount.ToString(), font8)));
                        POTable.AddCell(PODetailsCell);
                        PODetailsCell = new PdfPCell(new Phrase(new Chunk(objPOPrintEntity.PODetailsList[rows].gstvalue.ToString(), font8)));
                        POTable.AddCell(PODetailsCell);
                        PODetailsCell = new PdfPCell(new Phrase(new Chunk(objPOPrintEntity.PODetailsList[rows].OtherCharge.ToString(), font8)));
                        POTable.AddCell(PODetailsCell);  
                        PODetailsCell = new PdfPCell(new Phrase(new Chunk(objPOPrintEntity.PODetailsList[rows].BaseAmount.ToString(), font8)));
                        POTable.AddCell(PODetailsCell);                       
                    }
                    //Add Mid Content
                    PdfPTable POHeaderTable = new PdfPTable(4);
                    POHeaderTable.WidthPercentage = 100;
                    POHeaderTable.DefaultCell.Border = Rectangle.NO_BORDER;
                   
                    POHeaderTable.AddCell(new PdfPCell(new Phrase("PO Total Amount (INR)", fontTitle1)) { Border = 0});
                    POHeaderTable.AddCell(new PdfPCell(new Phrase(objPOPrintEntity.POAmount, fontTitle1)) { Border = 0, Colspan = 3, HorizontalAlignment = Element.ALIGN_LEFT });
                    POHeaderTable.AddCell(new PdfPCell(new Phrase("PO Amount In Words (INR)", fontTitle1)) { Border = 0});
                    POHeaderTable.AddCell(new PdfPCell(new Phrase(objPOPrintEntity.POAmountInWords, fontTitle1)) { Border = 0, Colspan = 3, HorizontalAlignment = Element.ALIGN_LEFT });
                                                         
                    //Add Terms And Cond
                    PdfPTable POTermsAndCondn = new PdfPTable(1);
                    POTermsAndCondn.SplitLate = false; 
                    POTermsAndCondn.SplitRows = true;
                    POTermsAndCondn.WidthPercentage = 100;

                    PdfPCell POTermsAndCondnCell = null; 
                    POTermsAndCondn.AddCell(new PdfPCell(new Phrase("Instructions To Supplier", fontTitle11)) { Colspan = 1, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });

                    POTermsAndCondnCell = new PdfPCell(new Phrase(new Chunk(objPOPrintEntity.TermAndCondn, fonttermsandcond)));
                    POTermsAndCondn.AddCell(POTermsAndCondnCell);

                    //Additional Terms and Conditions
                    PdfPTable POTermsAndCondn1 = new PdfPTable(1);
                    POTermsAndCondn1.SplitLate = false;
                    POTermsAndCondn1.SplitRows = true;
                    POTermsAndCondn1.WidthPercentage = 100;

                    PdfPCell POTermsAndCondnCell1 = null;
                    POTermsAndCondn1.AddCell(new PdfPCell(new Phrase("Retention", fontTitle11)) { Colspan = 1, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });

                    POTermsAndCondnCell1 = new PdfPCell(new Phrase(new Chunk(objPOPrintEntity.AdditionalTermsAndCondition, fonttermsandcond)));
                    POTermsAndCondn1.AddCell(POTermsAndCondnCell1);

                    //Add Approver Details
                    PdfPTable POApproverTable = new PdfPTable(3);
                    POApproverTable.WidthPercentage = 100;
                    int[] POApproverTableCellWidth = { 5, 45, 50 };
                    POApproverTable.SetWidths(POApproverTableCellWidth);
                    POApproverTable.DefaultCell.FixedHeight = 50f;

                    //string ApproverSign = Server.MapPath("~/Content/images/sign.png");
                    //iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(ApproverSign);
                    ////  image1.WidthPercentage = 10;
                    //image1.ScaleToFit(50, 30);

                    PdfPCell POApproverCell = null;
                   
                    POApproverTable.AddCell(new PdfPCell(new Phrase("Approver Details", fontTitle11)) { Colspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    POApproverTable.AddCell(new PdfPCell(new Phrase("S.No", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    POApproverTable.AddCell(new PdfPCell(new Phrase("Approver Name", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    //POApproverTable.AddCell(new PdfPCell(new Phrase("Approval Date", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    //POApproverTable.AddCell(new PdfPCell(new Phrase("Approval Stage", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    //POApproverTable.AddCell(new PdfPCell(new Phrase("Remarks", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    POApproverTable.AddCell(new PdfPCell(new Phrase("Signature", fontTitle11)) { HorizontalAlignment = Element.ALIGN_MIDDLE, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    
                    for (int rows = 0; rows < objPOPrintEntity.ApprovalsList.Count; rows++)
                    {
                        POApproverCell = new PdfPCell(new Phrase(new Chunk((rows + 1).ToString(), font8)));
                        POApproverTable.AddCell(POApproverCell);
                        POApproverCell = new PdfPCell(new Phrase(new Chunk(objPOPrintEntity.ApprovalsList[rows].ApproverName.ToString(), font8)));
                        POApproverTable.AddCell(POApproverCell);
                        //POApproverCell = new PdfPCell(new Phrase(new Chunk(objPOPrintEntity.ApprovalsList[rows].ApprovalDate.ToString(), font8)));
                        //POApproverTable.AddCell(POApproverCell);
                        //POApproverCell = new PdfPCell(new Phrase(new Chunk(objPOPrintEntity.ApprovalsList[rows].ApprovalStage.ToString(), font8)));
                        //POApproverTable.AddCell(POApproverCell);
                        //POApproverCell = new PdfPCell(new Phrase(new Chunk(objPOPrintEntity.ApprovalsList[rows].Remarks.ToString(), font8)));
                        //POApproverTable.AddCell(POApproverCell);
                      
                        string ApproverSign = Server.MapPath("~/Content/signatures/" + objPOPrintEntity.ApprovalsList[rows].EmpCode.ToString().Replace(" ","") + ".png");
                        if (System.IO.File.Exists(ApproverSign))
                        {
                            POApproverCell = new PdfPCell();
                            iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(ApproverSign);
                            //  image1.WidthPercentage = 10;
                            image1.ScaleToFit(50, 40);
                            POApproverCell.AddElement(image1);
                            POApproverTable.AddCell(POApproverCell);
                        }
                        else
                        {
                            POApproverTable.AddCell(new PdfPCell(new Phrase("", fontTitle11)));
                        }
                   
                    }

                    document.Add(HeaderTable);
                    document.Add(CBFHeaderDetails);
                    document.Add(new Paragraph(" "));
                    document.Add(CBFTable);
                    document.Add(new Paragraph(" "));
                    var pVendorNote = new Paragraph();
                    pVendorNote.Font = fontTitle1;
                    //pVendorNote.Add("Note to Supplier : ");
                    //pVendorNote.Add(objPOPrintEntity.VendorNotes);
                    document.Add(pVendorNote);
                    document.Add(new Paragraph(" "));
                    document.Add(POTable);
                    document.Add(new Paragraph(" "));
                    document.Add(POHeaderTable);
                    if (objPOPrintEntity.TermAndCondn != "")
                    {
                        document.Add(new Paragraph(" "));
                        document.Add(POTermsAndCondn);
                    }
                    if (objPOPrintEntity.AdditionalTermsAndCondition != "")
                    {
                        document.Add(new Paragraph(" "));
                        document.Add(POTermsAndCondn1);
                    }
                    document.Add(new Paragraph(" "));
                    document.Add(POApproverTable);

                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph("Acknowledged By "));
                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph("Supplier                                                              Date"));

                    document.NewPage();
                    PdfPTable tblTC = new PdfPTable(2);
                    tblTC.WidthPercentage = 100;

                    PdfPCell tblTCell = null;
                    tblTC.AddCell(new PdfPCell(new Phrase("Terms And Condition", fontTitle11)) { Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    DataTable dt = new DataTable(); 
                    dt = objRep.GetTC();
                    if (dt.Rows.Count > 1)
                    {
                     
                        tblTCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[0]["TermAndCond"].ToString(), fonttermsandcond)));
                        tblTCell.Border = 0;
                        tblTC.AddCell(tblTCell);
                        tblTCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[1]["TermAndCond"].ToString(), fonttermsandcond)));
                        tblTCell.Border = 0;
                        tblTC.AddCell(tblTCell);
                    }
                    document.Add(tblTC);
                    if (GetBranchIsSingle > 1)
                    {
                        document.NewPage();
                        document.Add(new Paragraph(" "));
                        document.Add(POTableAnn);
                    }
                  
                    document.Close();
                    writer.Close();
                     byte[] content = ms.ToArray();
                    string Filepath = System.Configuration.ConfigurationManager.AppSettings["Pdffiledownload"].ToString();
                    var path = Path.Combine(Filepath, "test" + ".PDF");
                     using (var fs = System.IO.File.Create(path))
                     {
                         fs.Write(content, 0, (int)content.Length);
                     }
                     byte[] bytes = System.IO.File.ReadAllBytes(path);
                     Font blackFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);
                     using (MemoryStream stream = new MemoryStream())
                     {
                         PdfReader reader = new PdfReader(bytes);
                         using (PdfStamper stamper = new PdfStamper(reader, stream))
                         {
                             int pages = reader.NumberOfPages;
                             for (int i = 1; i <= pages; i++)
                             {
                                 ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, new Phrase("Page No:" + i.ToString(), blackFont), 568f, 15f, 0);
                             }
                         }
                         if (System.IO.File.Exists(path))
                         {
                             System.IO.File.Delete(path);
                         }
                         string fileprintname = objPOPrintEntity.PONumber;
                         Response.ContentType = "pdf/application";
                         Response.AddHeader("content-disposition",
                         "attachment;filename=" + fileprintname + ".PDF");
                         Response.OutputStream.Write(stream.GetBuffer(), 0, stream.GetBuffer().Length);
                         Response.End();
                     }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
        }
      
        public void PrintPDFForWO(string WONumber = null)  
        {
            try
            {
                WOPrintEntity objWOPrintEntity = new WOPrintEntity();
                objWOPrintEntity = objRep.GetWOData(WONumber);
                int GetBranchIsSingle = objRep.GetBranchIsSingle(objWOPrintEntity.WOGid);
                objWOPrintEntity.WODetailsList = objRep.GetWODetailsList(WONumber).ToList();
                objWOPrintEntity.ApprovalsList = objRep.GetApproverList(WONumber, "WO").ToList();
                objWOPrintEntity.ShipmentList = objRep.GetShipmentDetailsWO(objWOPrintEntity.WOGid).ToList();

                iTextSharp.text.Font fontTitle = FontFactory.GetFont("Arial", 8, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle1 = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle11 = FontFactory.GetFont("Arial", 7, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle2 = FontFactory.GetFont("Arial", 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitle3 = FontFactory.GetFont("Arial", 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
                iTextSharp.text.Font fontTitlenote = FontFactory.GetFont("Arial", 6, iTextSharp.text.Font.NORMAL, BaseColor.RED);
                iTextSharp.text.Font fonttermsandcond = FontFactory.GetFont("Arial", 6, iTextSharp.text.Font.NORMAL, BaseColor.BLACK); 
                Font font8 = FontFactory.GetFont("ARIAL", 7);
                POPrintEntity objPOPrintEntity = new POPrintEntity();
                using (MemoryStream ms = new MemoryStream())
                {
                    Document document = new Document(PageSize.A4, 10F, 10F, 10F, 20F);
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);
                    document.Open();
                    document.NewPage();
                    PdfPTable HeaderTable = new PdfPTable(10);
                    HeaderTable.WidthPercentage = 100;

                    //Title & Logo
                    PdfPCell Headerlogo = new PdfPCell();
                    string Headerlogoath = Server.MapPath("~/Content/images/Fullerton.bmp");
                    iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(Headerlogoath);
                    image.WidthPercentage = 90;
                    Headerlogo.AddElement(image);
                    Headerlogo.HorizontalAlignment = Element.ALIGN_CENTER;
                    Headerlogo.VerticalAlignment = Element.ALIGN_MIDDLE;
                    Headerlogo.Colspan = 3;
                    Headerlogo.Rowspan = 2;

                    PdfPCell TitleText = new PdfPCell();
                    TitleText.HorizontalAlignment = Element.ALIGN_CENTER;
                    TitleText.VerticalAlignment = Element.ALIGN_MIDDLE;
                    HeaderTable.AddCell(new PdfPCell(new Phrase(" WORK ORDER ", fontTitle2)) { Colspan = 7, Rowspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    HeaderTable.AddCell(Headerlogo);

                    //Header
                    PdfPTable CBFHeaderDetails = new PdfPTable(4);
                    CBFHeaderDetails.WidthPercentage = 100;
                    int[] CBFHeaderDetailsCellWidth = { 15, 30, 15, 40 };
                    CBFHeaderDetails.SetWidths(CBFHeaderDetailsCellWidth);

                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("WO Number", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objWOPrintEntity.WONumber, fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("Vendor Name", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objWOPrintEntity.VendorName, fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("WO Date", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objWOPrintEntity.WODate, fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("Description(Vendor Note)", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objWOPrintEntity.VendorNotes, fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("Initiator", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objWOPrintEntity.RaiserName, fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("Request For", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objWOPrintEntity.RequestFor, fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("CBF Number", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objWOPrintEntity.WODetailsList[0].WOCBFNumber.ToString(), fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("CC ", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objWOPrintEntity.WODetailsList[0].fccc.ToString(), fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("Mode", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objWOPrintEntity.WODetailsList[0].cbfmode.ToString(), fontTitle1)));

                    if (objWOPrintEntity.WODetailsList[0].cbfmode == "PAR")
                    {
                        CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("PAR NO", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                        CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objWOPrintEntity.WODetailsList[0].parno.ToString(), fontTitle1)));
                    }
                    else if (objWOPrintEntity.WODetailsList[0].cbfmode == "PR")
                    {
                        CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("PR NO", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                        CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objWOPrintEntity.WODetailsList[0].prno.ToString(), fontTitle1)));
                    }
                    //CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("Branch", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    //CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objWOPrintEntity.WODetailsList[0].cbfbranch.ToString(), fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("Wo Version", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objWOPrintEntity.WoVersion.ToString(), fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("WO Amendment Date", fontTitle1)) { BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objWOPrintEntity.WoAmendmentDate.ToString(), fontTitle1)));
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase("Department Name", fontTitle1)) {  BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFHeaderDetails.AddCell(new PdfPCell(new Phrase(objWOPrintEntity.deptname.ToString(), fontTitle1)) { Colspan = 3 });
                   

                    //Vendor Address & Fullerton Address
                    PdfPTable CBFTable = new PdfPTable(3);
                    CBFTable.WidthPercentage = 100;
                    int[] CBFTableCellWidth = { 33, 34, 33 };
                    CBFTable.SetWidths(CBFTableCellWidth);

                    CBFTable.AddCell(new PdfPCell(new Phrase("Vendor Address", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFTable.AddCell(new PdfPCell(new Phrase("Deliver to", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    CBFTable.AddCell(new PdfPCell(new Phrase("Billing Address", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                    var p = new Paragraph();
                    p.Font = fontTitle1;
                    p.Add(objWOPrintEntity.VendorName + "\n");
                    p.Add(objWOPrintEntity.VendorAddress + "\n");
                    p.Add(objWOPrintEntity.VendorCity + "\n");
                    p.Add(objWOPrintEntity.VendorState + "\n");
                    p.Add(objWOPrintEntity.VendorCountry + "\n");
                    p.Add("Pin Code : " + objWOPrintEntity.VendorPinCode + "\n");
                    p.Add("Phone : " + objWOPrintEntity.VendorPhone + "\n");
                    p.Add("Fax : " + objWOPrintEntity.VendorFax);
                    CBFTable.AddCell(p);

                    var p2 = new Paragraph();
                    p2.Font = fontTitle1;
                    PdfPTable POTableAnn = new PdfPTable(7);
                    if (GetBranchIsSingle > 1)
                    {
                        p2.Add("Multiple Branches. \n");
                        p2.Add("Refer Annexure \n");

                        //Annexure
                        POTableAnn.WidthPercentage = 100;
                        int[] POTableCellWidthAnn = { 5, 15, 45, 9, 15, 4, 7 };
                        POTableAnn.SetWidths(POTableCellWidthAnn);

                        PdfPCell PODetailsCellAnn = null;
                        POTableAnn.AddCell(new PdfPCell(new Phrase("Annexure", fontTitle11)) { Colspan = 7, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                        POTableAnn.AddCell(new PdfPCell(new Phrase("S.No", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                        POTableAnn.AddCell(new PdfPCell(new Phrase("Branch Name", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                        POTableAnn.AddCell(new PdfPCell(new Phrase("Branch Address", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                        POTableAnn.AddCell(new PdfPCell(new Phrase("Product Code", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                        POTableAnn.AddCell(new PdfPCell(new Phrase("Product Name", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                        POTableAnn.AddCell(new PdfPCell(new Phrase("Qty", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                        POTableAnn.AddCell(new PdfPCell(new Phrase("type", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });

                        for (int rows = 0; rows < objWOPrintEntity.ShipmentList.Count; rows++)
                        {
                            PODetailsCellAnn = new PdfPCell(new Phrase(new Chunk((rows + 1).ToString(), font8)));
                            POTableAnn.AddCell(PODetailsCellAnn);
                            PODetailsCellAnn = new PdfPCell(new Phrase(new Chunk(objWOPrintEntity.ShipmentList[rows].BranchName.ToString(), font8)));
                            POTableAnn.AddCell(PODetailsCellAnn);
                            PODetailsCellAnn = new PdfPCell(new Phrase(new Chunk(objWOPrintEntity.ShipmentList[rows].BranchAddress.ToString(), font8)));
                            POTableAnn.AddCell(PODetailsCellAnn);
                            PODetailsCellAnn = new PdfPCell(new Phrase(new Chunk(objWOPrintEntity.ShipmentList[rows].ShipmentProdCode.ToString(), font8)));
                            POTableAnn.AddCell(PODetailsCellAnn);
                            PODetailsCellAnn = new PdfPCell(new Phrase(new Chunk(objWOPrintEntity.ShipmentList[rows].ShipmentprodName.ToString(), font8)));
                            POTableAnn.AddCell(PODetailsCellAnn);
                            PODetailsCellAnn = new PdfPCell(new Phrase(new Chunk(objWOPrintEntity.ShipmentList[rows].ShippedQty.ToString(), font8)));
                            POTableAnn.AddCell(PODetailsCellAnn);
                            PODetailsCellAnn = new PdfPCell(new Phrase(new Chunk(objWOPrintEntity.ShipmentList[rows].ShipmentType.ToString(), font8)));
                            POTableAnn.AddCell(PODetailsCellAnn);
                        }
                    }
                    else
                    {
                        p2.Add(System.Configuration.ConfigurationManager.AppSettings["CompanyFullName"].ToString() + "\n");
                        p2.Add(objWOPrintEntity.ShipmentList[0].BranchName);
                        p2.Add(objWOPrintEntity.ShipmentList[0].BranchAddress);
                    }
                    CBFTable.AddCell(p2);

                    var p1 = new Paragraph();
                    p1.Font = fontTitle1;
                    //p1.Add("Fullerton India Credit Company Limited \n");
                    //p1.Add("Floor 5 & 6, B Wing, Supreme IT Park,\n");
                    //p1.Add("Supreme City, Behind Lake Castle,\n");
                    //p1.Add("Powai,\n");
                    //p1.Add("Mumbai - 400 076.\n");
                    //p1.Add("Tel # +91 22 67491234 \n");
                    p1.Add(System.Configuration.ConfigurationManager.AppSettings["CompanyFullName"].ToString() + "\n");
                    p1.Add(System.Configuration.ConfigurationManager.AppSettings["MAddress1"].ToString() + ",\n");
                    p1.Add(System.Configuration.ConfigurationManager.AppSettings["MAddress2"].ToString() + ",\n");
                    p1.Add(System.Configuration.ConfigurationManager.AppSettings["MArea"].ToString() + ",\n");
                    p1.Add(System.Configuration.ConfigurationManager.AppSettings["MCity"].ToString() + "\n");
                    p1.Add(System.Configuration.ConfigurationManager.AppSettings["MPhoneNo"].ToString() + "\n");
                 //   p1.Add("\n");
                  //  p1.Add("Note to Supplier: Please forward your invoice and other documents to the above address ");
                    CBFTable.AddCell(p1);

                    //PO Details
                    PdfPTable POTable = new PdfPTable(8);
                    POTable.WidthPercentage = 100;
                    int[] POTableCellWidth = { 5, 10, 20, 20, 13, 6, 13, 13 };
                    POTable.SetWidths(POTableCellWidth);

                    PdfPCell PODetailsCell = null;
                    POTable.AddCell(new PdfPCell(new Phrase("WO Details", fontTitle11)) { Colspan = 8, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    POTable.AddCell(new PdfPCell(new Phrase("S.No", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    POTable.AddCell(new PdfPCell(new Phrase("Product Code", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    POTable.AddCell(new PdfPCell(new Phrase("Product Name", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    POTable.AddCell(new PdfPCell(new Phrase("Description", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    POTable.AddCell(new PdfPCell(new Phrase("UOM", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    POTable.AddCell(new PdfPCell(new Phrase("Qty", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    POTable.AddCell(new PdfPCell(new Phrase("Unit Price(INR)", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    POTable.AddCell(new PdfPCell(new Phrase("Total Amount(INR)", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                   
                    for (int rows = 0; rows < objWOPrintEntity.WODetailsList.Count; rows++)
                    {
                        PODetailsCell = new PdfPCell(new Phrase(new Chunk((rows + 1).ToString(), font8)));
                        POTable.AddCell(PODetailsCell);
                        PODetailsCell = new PdfPCell(new Phrase(new Chunk(objWOPrintEntity.WODetailsList[rows].ProductCode.ToString(), font8)));
                        POTable.AddCell(PODetailsCell);
                        PODetailsCell = new PdfPCell(new Phrase(new Chunk(objWOPrintEntity.WODetailsList[rows].ProductName.ToString(), font8)));
                        POTable.AddCell(PODetailsCell);
                        PODetailsCell = new PdfPCell(new Phrase(new Chunk(objWOPrintEntity.WODetailsList[rows].ProductDescription.ToString(), font8)));
                        POTable.AddCell(PODetailsCell);
                        PODetailsCell = new PdfPCell(new Phrase(new Chunk(objWOPrintEntity.WODetailsList[rows].UOM.ToString(), font8)));
                        POTable.AddCell(PODetailsCell);
                        PODetailsCell = new PdfPCell(new Phrase(new Chunk(objWOPrintEntity.WODetailsList[rows].Qty.ToString(), font8)));
                        POTable.AddCell(PODetailsCell);
                        PODetailsCell = new PdfPCell(new Phrase(new Chunk(objWOPrintEntity.WODetailsList[rows].UnitPrice.ToString(), font8)));
                        POTable.AddCell(PODetailsCell);
                        PODetailsCell = new PdfPCell(new Phrase(new Chunk(objWOPrintEntity.WODetailsList[rows].TotalAmount.ToString(), font8)));
                        POTable.AddCell(PODetailsCell);
                    }
                    //Add Mid Content
                    PdfPTable POHeaderTable = new PdfPTable(4);
                    POHeaderTable.WidthPercentage = 100;
                    POHeaderTable.DefaultCell.Border = Rectangle.NO_BORDER;

                    POHeaderTable.AddCell(new PdfPCell(new Phrase("WO Total Amount (INR)", fontTitle1)) { Border = 0});
                    POHeaderTable.AddCell(new PdfPCell(new Phrase(objWOPrintEntity.WOAmount, fontTitle1)) { Border = 0, Colspan = 3, HorizontalAlignment = Element.ALIGN_LEFT });
                    POHeaderTable.AddCell(new PdfPCell(new Phrase("WO Amount In Words (INR)", fontTitle1)) { Border = 0});
                    POHeaderTable.AddCell(new PdfPCell(new Phrase(objWOPrintEntity.WOAmountInWords, fontTitle1)) { Border = 0, Colspan = 3, HorizontalAlignment = Element.ALIGN_LEFT });

                    //Add Approver Details
                    PdfPTable POApproverTable = new PdfPTable(3);
                    POApproverTable.WidthPercentage = 100;
                    int[] POApproverTableCellWidth = { 5, 45, 50 };
                    POApproverTable.SetWidths(POApproverTableCellWidth);
                    

                    PdfPCell POApproverCell = null;
                    POApproverTable.AddCell(new PdfPCell(new Phrase("Approver Details", fontTitle11)) { Colspan = 3, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    POApproverTable.AddCell(new PdfPCell(new Phrase("S.No", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    POApproverTable.AddCell(new PdfPCell(new Phrase("Approver Name", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    //POApproverTable.AddCell(new PdfPCell(new Phrase("Approval Date", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    //POApproverTable.AddCell(new PdfPCell(new Phrase("Approval Stage", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    //POApproverTable.AddCell(new PdfPCell(new Phrase("Remarks", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                    POApproverTable.AddCell(new PdfPCell(new Phrase("Signature", fontTitle11)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = BaseColor.LIGHT_GRAY });
                   
                   
                    for (int rows = 0; rows < objWOPrintEntity.ApprovalsList.Count; rows++)
                    {
                        POApproverCell = new PdfPCell(new Phrase(new Chunk((rows + 1).ToString(), font8)));
                        POApproverTable.AddCell(POApproverCell);
                        POApproverCell = new PdfPCell(new Phrase(new Chunk(objWOPrintEntity.ApprovalsList[rows].ApproverName.ToString(), font8)));
                        POApproverTable.AddCell(POApproverCell);
                        //POApproverCell = new PdfPCell(new Phrase(new Chunk(objWOPrintEntity.ApprovalsList[rows].ApprovalDate.ToString(), font8)));
                        //POApproverTable.AddCell(POApproverCell);
                        //POApproverCell = new PdfPCell(new Phrase(new Chunk(objWOPrintEntity.ApprovalsList[rows].ApprovalStage.ToString(), font8)));
                        //POApproverTable.AddCell(POApproverCell);
                        //POApproverCell = new PdfPCell(new Phrase(new Chunk(objWOPrintEntity.ApprovalsList[rows].Remarks.ToString(), font8)));
                        //POApproverTable.AddCell(POApproverCell);
                      
                        //string ApproverSign = Server.MapPath("~/Content/signatures/sign.png");
                        //iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(ApproverSign);
                        ////  image1.WidthPercentage = 10;
                        //image1.ScaleToFit(50, 40);
                        //POApproverCell.AddElement(image1); 
                        //POApproverTable.AddCell(POApproverCell);
                        string ApproverSign = Server.MapPath("~/Content/signatures/" + objWOPrintEntity.ApprovalsList[rows].EmpCode.ToString().Replace(" ", "") + ".png");
                        if (System.IO.File.Exists(ApproverSign))
                        {
                            POApproverCell = new PdfPCell();
                            iTextSharp.text.Image image1 = iTextSharp.text.Image.GetInstance(ApproverSign);
                            //  image1.WidthPercentage = 10;
                            image1.ScaleToFit(50, 40);
                            POApproverCell.AddElement(image1);
                            POApproverTable.AddCell(POApproverCell);
                        }
                        else
                        {
                            POApproverTable.AddCell(new PdfPCell(new Phrase("", fontTitle11)));
                        }
                   
                    }
                    //Add Terms And Cond
                    PdfPTable POTermsAndCondn = new PdfPTable(1);
                    POTermsAndCondn.SplitLate = false;
                    POTermsAndCondn.SplitRows = true;
                    POTermsAndCondn.WidthPercentage = 100;

                    PdfPCell POTermsAndCondnCell = null;
                    POTermsAndCondn.AddCell(new PdfPCell(new Phrase("Instructions To Supplier", fontTitle11)) { Colspan = 1, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });

                    POTermsAndCondnCell = new PdfPCell(new Phrase(new Chunk(objWOPrintEntity.TermAndCondn, fonttermsandcond)));
                    POTermsAndCondn.AddCell(POTermsAndCondnCell);

                    PdfPTable POTermsAndCondnAdd = new PdfPTable(1);
                    POTermsAndCondnAdd.SplitLate = false;
                    POTermsAndCondnAdd.SplitRows = true;
                    POTermsAndCondnAdd.WidthPercentage = 100;
                    PdfPCell POTermsAndCondnCellAdd = null;
                    POTermsAndCondnAdd.AddCell(new PdfPCell(new Phrase("Additional Terms And Condition", fontTitle11)) { Colspan = 1, HorizontalAlignment = Element.ALIGN_LEFT, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });

                    POTermsAndCondnCellAdd = new PdfPCell(new Phrase(new Chunk(objWOPrintEntity.AdditionalTermsAndCondition, fonttermsandcond)));
                    POTermsAndCondnAdd.AddCell(POTermsAndCondnCellAdd);

                    document.Add(HeaderTable);
                    document.Add(CBFHeaderDetails);
                    document.Add(new Paragraph(" "));
                    document.Add(CBFTable);
                    document.Add(new Paragraph(" "));
                    var pVendorNote = new Paragraph();
                    pVendorNote.Font = fontTitle1;
                    //pVendorNote.Add("Note to Supplier : ");
                    //pVendorNote.Add(objWOPrintEntity.VendorNotes);
                    document.Add(pVendorNote);
                    document.Add(new Paragraph(" "));
                    document.Add(POTable);
                    document.Add(new Paragraph(" "));
                    document.Add(POHeaderTable);
                    if (objWOPrintEntity.TermAndCondn != "")
                    {
                        document.Add(new Paragraph(" "));
                        document.Add(POTermsAndCondn);
                    }
                    if (objWOPrintEntity.AdditionalTermsAndCondition != "")
                    {
                        document.Add(new Paragraph(" "));
                        document.Add(POTermsAndCondnAdd);
                    }
                    document.Add(new Paragraph(" "));
                    document.Add(POApproverTable);

                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph("Acknowledged By "));
                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph(" "));
                    document.Add(new Paragraph("Supplier                                                                         Date"));

                    document.NewPage();
                    PdfPTable tblTC = new PdfPTable(2);
                    tblTC.WidthPercentage = 100;

                    PdfPCell tblTCell = null;
                    tblTC.AddCell(new PdfPCell(new Phrase("Terms And Condition", fontTitle11)) { Colspan = 2, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_MIDDLE, BackgroundColor = new BaseColor(198, 217, 241) });
                    DataTable dt = new DataTable();
                    dt = objRep.GetTC();
                    if (dt.Rows.Count > 1)
                    {
                        tblTCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[0]["TermAndCond"].ToString(), fonttermsandcond)));
                        tblTCell.Border = 0;
                        tblTC.AddCell(tblTCell);
                        tblTCell = new PdfPCell(new Phrase(new Chunk(dt.Rows[1]["TermAndCond"].ToString(), fonttermsandcond)));
                        tblTCell.Border = 0;
                        tblTC.AddCell(tblTCell);
                    }
                    document.Add(tblTC);
                    if (GetBranchIsSingle > 1)
                    {
                        document.NewPage();
                        document.Add(new Paragraph(" "));
                        document.Add(POTableAnn);
                    }
                    document.Close();
                    writer.Close();
                    byte[] content = ms.ToArray();
                    string Filepath = System.Configuration.ConfigurationManager.AppSettings["Pdffiledownload"].ToString();
                    var path = Path.Combine(Filepath, "test" + ".PDF");
                     using (var fs = System.IO.File.Create(path))
                     {
                         fs.Write(content, 0, (int)content.Length);
                     }
                     byte[] bytes = System.IO.File.ReadAllBytes(path);
                     Font blackFont = FontFactory.GetFont("Arial", 12, Font.NORMAL, BaseColor.BLACK);
                     using (MemoryStream stream = new MemoryStream())
                     {
                         PdfReader reader = new PdfReader(bytes);
                         using (PdfStamper stamper = new PdfStamper(reader, stream))
                         {
                             int pages = reader.NumberOfPages;
                             for (int i = 1; i <= pages; i++)
                             {
                                 ColumnText.ShowTextAligned(stamper.GetUnderContent(i), Element.ALIGN_RIGHT, new Phrase("Page No:" + i.ToString(), blackFont), 568f, 15f, 0);
                             }
                         }
                         if (System.IO.File.Exists(path))
                         {
                             System.IO.File.Delete(path);
                         }
                         string fileprintname = objWOPrintEntity.WONumber;
                         Response.ContentType = "pdf/application";
                         Response.AddHeader("content-disposition",
                         "attachment;filename=" + fileprintname + ".PDF");
                         Response.OutputStream.Write(stream.GetBuffer(), 0, stream.GetBuffer().Length);
                         Response.End();
                     }
                }

                
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
        }
        public FileResult DownloadDocument(string id)
        {
            try
            {
                string filename = id;
                var localpath = Path.Combine(@"C:\temp\", filename + ".pdf");
                return File(localpath, System.Net.Mime.MediaTypeNames.Application.Octet, filename + ".pdf");
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw ex;
            }
        }
        public ActionResult ApproversDocs() 
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetDocSummary()
        {
            string data0 = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objRep.GetDocSummary();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
               
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult QueryScreen() 
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetQueryScreenData() 
        {
            string data0 = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objRep.GetQueryScreenData();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }

                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }
        public void DTtoExcell()
        {
            DataSet ds = new DataSet();
            DataTable dt;
            ds = objRep.GetQueryScreenData();
            dt = ds.Tables[0];
            string dtnow = DateTime.Now.ToString("yyMMdd");
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "QueryResult");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=QueryResult_" + dtnow + ".xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
        public ActionResult VendorSelectionScreen() 
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetVendorSelectionData() 
        {
            string data0 = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objRep.GetVendorSelectionData();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }

                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }
        public void DTtoExcellVS() 
        {
            DataSet ds = new DataSet();
            DataTable dt;
            ds = objRep.GetVendorSelectionData();
            dt = ds.Tables[0];
            string dtnow = DateTime.Now.ToString("yyMMdd");
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "VendorSelection");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=VendorSelectionData_" + dtnow + ".xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
        public ActionResult VendorSelectionOpex() 
        {
            Session["OBFGid"] = null;
            return View();
        }
        [HttpGet]
        public JsonResult VendorSelectionOpexData() 
        {
            string data0 = string.Empty;
            string data1 = string.Empty; 
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objRep.VendorSelectionOpexData(); 
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult VendorSelectionOpexDetails(string docgid = null,string doctype = null) 
        {
            ViewBag.docgid = docgid;
           // ViewBag.doctype = doctype;
            return View();
        }
        [HttpPost]
        public JsonResult VendorSelectionOpexDetailsSummary(string docgid = null)  
        {
            string data0 = string.Empty;
            string data1 = string.Empty;
            try
            {
                if (Session["OBFGid"] != null)
                {
                    string obfgid = (string)Session["OBFGid"];
                    DataSet ds = new DataSet();
                    DataTable dt;
                    ds = objRep.VendorSelectionOpexDetailsSummaryOld(obfgid);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                }
                else
                {
                    DataSet ds = new DataSet();
                    DataTable dt;
                    ds = objRep.VendorSelectionOpexDetailsSummary(docgid);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { 
                        string obfheadergid = dt.Rows[0]["OBFGid"].ToString();
                        Session["OBFGid"] = obfheadergid;
                        data0 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                }
               
                return Json(new { data0, data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, data1 }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetCCAutoComplete(string searchtext = null)
        {
            List<string> result = new List<string>();
            try
            {
                DataSet ds = new DataSet();
                ds = objRep.GetCCAutoComplete(searchtext);
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        result.Add(string.Format("{0}~{1}", dr[0], dr[1]));
                    }
                }
                ds.Dispose();
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { result }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetOBFDetailsById(string docgid = null)
        {
            string data0 = string.Empty;
            string data1 = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objRep.GetOBFDetailsById(docgid); 
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult UpdateOBFDetails(OBFDetails objOBFDetails) 
        {
            string data0 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "1";
                int result = objRep.UpdateOBFDetails(objOBFDetails);
                if (result == 0)
                {
                    ErrorMsg = "1";
                }
                else
                {
                    ErrorMsg = "0";
                    DataSet ds = new DataSet();
                    DataTable dt;
                    ds = objRep.GetOBFDetailsAll(result); 
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
       
        [HttpPost]
        public JsonResult SubmitOBFDetails(OBFDetails objOBFDetails)
        {
            string data0 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "1";
                int result = objRep.SubmitOBFDetails(objOBFDetails);
                if (result == 0)
                {
                    ErrorMsg = "1";
                }
                else
                {
                    ErrorMsg = "0";
                }
                return Json(new { ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult VendorSelectionOpexDetailsSummary1(string docgid = null) 
        {
            string data0 = string.Empty;
            string data1 = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objRep.VendorSelectionOpexDetailsSummary1(docgid);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                return Json(new { data0, data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, data1 }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult VendorSelectionOpexDetailsPAR(string docgid = null, string doctype = null) 
        {
            ViewBag.docgid = docgid;
            // ViewBag.doctype = doctype;
            return View();
        }
        [HttpPost]
        public JsonResult VendorSelectionOpexDetailsSummaryPAR(string docgid = null)
        {
            string data0 = string.Empty;
            string data1 = string.Empty;
            try
            {
                    DataSet ds = new DataSet();
                    DataTable dt;
                    ds = objRep.GetHeaderDetailsPAR(docgid);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }

                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GenerateOBFDetails(OBFDetails objOBFDetails)
        {
            string data0 = string.Empty;
            string data1 = string.Empty; 
            try
            {
                if (Session["OBFGid"] != null)
                {
                    string obfgid = (string)Session["OBFGid"];
                    DataSet ds = new DataSet();
                    DataTable dt;
                    ds = objRep.VendorSelectionOpexDetailsSummaryOldPAR(obfgid);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                }
                else
                {
                    DataSet ds = new DataSet();
                    DataTable dt;
                    ds = objRep.GenerateOBFDetails(objOBFDetails);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        string obfheadergid = dt.Rows[0]["OBFGid"].ToString();
                        Session["OBFGid"] = obfheadergid;
                        data0 = JsonConvert.SerializeObject(dt);
                    }
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }
                }

                return Json(new { data0, data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, data1 }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult GetOBFDetailsByIdPAR(string docgid = null) 
        {
            string data0 = string.Empty;
            string data1 = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objRep.GetOBFDetailsByIdPAR(docgid);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult UpdateOBFDetailsPAR(OBFDetails objOBFDetails) 
        {
            string data0 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "1";
                int result = objRep.UpdateOBFDetailsPAR(objOBFDetails); 
                if (result == 0)
                {
                    ErrorMsg = "1";
                }
                else
                {
                    ErrorMsg = "0";
                    DataSet ds = new DataSet();
                    DataTable dt;
                    ds = objRep.GetOBFDetailsAllPAR(result);
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult SubmitOBFDetailsPAR(OBFDetails objOBFDetails)
        {
            string data0 = string.Empty;
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "1";
                int result = objRep.SubmitOBFDetailsPAR(objOBFDetails);
                if (result == 0)
                {
                    ErrorMsg = "1";
                }
                else
                {
                    ErrorMsg = "0";
                }
                return Json(new { ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult CheckProductExists(string obfgid = "") 
        {
            string data0 = "0";
            string ErrorMsg = string.Empty;
            try
            {
                ErrorMsg = "1";
                DataTable dt = new DataTable();
                int result = Convert.ToInt32(string.IsNullOrEmpty(obfgid) ? "0" : obfgid);
                data0 = Convert.ToString(objRep.CheckProductExists(result));
                ErrorMsg = "0";
                if (data0 == "-1")
                    ErrorMsg = "404";
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult PORpt() 
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetPORptData(string cbfnumber = null, string ponumber = null, string podate = null, string poenddate = null) 
        {
            string data0 = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objRep.GetPORptData(cbfnumber, ponumber, podate, poenddate);
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }

                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0 }, JsonRequestBehavior.AllowGet);
            }
        }
        public void DTtoExcellPORpt(string cbfnumber = null,string ponumber = null,string podate = null,string poenddate = null)   
        {
            DataSet ds = new DataSet();
            DataTable dt;
            ds = objRep.GetPORptDataFull(cbfnumber,ponumber,podate,poenddate);
            dt = ds.Tables[0];
            string dtnow = DateTime.Now.ToString("yyyyMMdd");
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "PO Report");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=POReport_" + dtnow + ".xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
        public void DTtoExcellPORptFull() 
        {
            DataSet ds = new DataSet();
            DataTable dt;
            ds = objRep.GetPORptDataFull();
            dt = ds.Tables[0];
            string dtnow = DateTime.Now.ToString("yyyyMMdd");
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "PO Report");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=POReport_" + dtnow + ".xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
        }
        public ActionResult PendingForConfirmation()
        {
            return View();
        }
        [HttpGet]
        public JsonResult PendingForConfirmationData() 
        {
            string data0 = string.Empty;
            string data1 = string.Empty;
            try
            {
                DataSet ds = new DataSet();
                DataTable dt;
                ds = objRep.PendingForConfirmationData();
                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { data0 = JsonConvert.SerializeObject(dt); }
                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { data1 = JsonConvert.SerializeObject(dt); }

                return Json(new { data0, data1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(new { data0, data1 }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
