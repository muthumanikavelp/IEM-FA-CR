using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Net;
using System.Collections;
using IEM.Areas.IFAMS.Models;
using IEM.Common;
using System.Configuration;
using iTextSharp.text;
using System.Web.Hosting;
using iTextSharp.text.pdf;
using System.Web.UI.WebControls;
using System.Web.UI;
using IEM.App_Start;

namespace IEM.Areas.IFAMS.Controllers
{
    [NoDirectAccess]
    public class BarCodeGenerationController : Controller
    {
        //
        // GET: /IFAMS/BarCodeGeneration/
        private IRepository ifr;
        private CmnFunctions objCmnFunc = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();

        public BarCodeGenerationController()
            : this(new DataModel()) { }

        public BarCodeGenerationController(IRepository objModel)
        {
            ifr = objModel;
        }

        [HttpGet]
        public ActionResult BarCodeSummary()
        {
            try
            {

                BarcodeGenerationModel records = new BarcodeGenerationModel();
               //records.BModel = ifr.GetDetailForBarcode().ToList();
                records.BModel = Enumerable.Empty<BarcodeGenerationModel>().ToList<BarcodeGenerationModel>(); 
                if (records.BModel.Count == 0) { 
                    ViewBag.Message = "No records found";
                    ViewBag.Dp_type = "--Select All--";
                }
                //Session["BarCodeGenerationExceldownload"] = records.BModel;
                return View(records);
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
            finally
            {
            }
        }

        [HttpPost]
        public ActionResult BarCodeSummary(string[] ids, string loccode, string assetcode,string Dp_type, string capdate,string captodate, string ecfno, FormCollection collections, string command)
        {
            BarcodeGenerationModel details = new BarcodeGenerationModel();
            try
            {
                if (command == "SEARCH")
                {

                    details.BModel = ifr.GetDetailForBarcode(loccode, assetcode, Dp_type,capdate,captodate).ToList();
                   /* if ((string.IsNullOrEmpty(loccode) || string.IsNullOrWhiteSpace(loccode)) &&
                        (string.IsNullOrEmpty(assetcode) || string.IsNullOrWhiteSpace(assetcode)) &&
                        (string.IsNullOrEmpty(ecfno) || string.IsNullOrWhiteSpace(ecfno)) &&
                         (Dp_type == "--Select All--") && 
                        (string.IsNullOrEmpty(capdate) || string.IsNullOrWhiteSpace(capdate))&&
                       (string.IsNullOrEmpty(captodate)|| string.IsNullOrWhiteSpace(captodate)))
                    {
                        details.BModel = ifr.GetDetailForBarcode().ToList();
                    }
                    if (Dp_type == "--Select All--")
                    { ViewBag.Dp_type = "--Select All--"; }
                    if (!string.IsNullOrEmpty(loccode))
                    {
                        ViewBag.loccode = loccode;
                        details.BModel = details.BModel.Where(x => loccode.ToUpper() == null || (x._loc.ToUpper().Contains(loccode.ToUpper()))).ToList();
                    }
                    if (!string.IsNullOrEmpty(assetcode))
                    {
                        ViewBag.assetcode = assetcode;
                        details.BModel = details.BModel.Where(x => assetcode.ToUpper() == null || (x._assetcode_gid.ToUpper().Contains(assetcode.ToUpper()))).ToList();
                    }
                    if (Dp_type != "--Select All--")
                    {
                        ViewBag.Dp_type = Dp_type;
                        details.BModel = details.BModel.Where(x => Dp_type.ToUpper() == null || (x._inv_no.ToUpper().Contains(Dp_type.ToUpper()))).ToList();
                    }

                    if (!string.IsNullOrEmpty(capdate))
                    {
                        if (!string.IsNullOrEmpty(captodate))
                        {
                            ViewBag.capdate = capdate;
                            ViewBag.captodate = captodate;
                            if (Convert.ToDateTime(capdate) > Convert.ToDateTime(captodate))
                            {
                                Session["alert"] = "Check date value";
                                ViewBag.Message = "No records found";
                            }
                            //details.BModel = details.BModel.Where(x => capdate == null || (x._cap_date.ToUpper().Contains(capdate.ToUpper()))).ToList();
                            details.BModel = details.BModel.Where(x => capdate == null || (Convert.ToDateTime(x._cap_date)) >= Convert.ToDateTime(capdate) && Convert.ToDateTime(x._cap_date) <= Convert.ToDateTime(captodate)).ToList();
                        }
                    }*/
                    if (details.BModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                        if (Dp_type != "--Select All--")
                        {
                            ViewBag.Dp_type = Dp_type;
                        }
                        else
                        {
                            ViewBag.Dp_type = "--Select All--";
                        }
                    }
                    Session["BarCodeGenerationExceldownload"] = details.BModel;
                    return View(details);
                }
                if (command == "Clear")
                {
                    return RedirectToAction("BarCodeSummary");
                }
                if (command == "genBarcode")
                {
                    List<string> id = new List<string>();
                    string check = "";
                    string Checker = string.Empty;
                    Checker = objCmnFunc.authorize("245");
                    if (Checker == "Success")
                    {
                        if (ids != null)
                        {
                            id = ids.ToList();
                            check = ifr.GenerateBarcode(id);
                            Session["alert"] = check;
                        }
                    }
                    else
                    {
                        Session["alert"] = "Unauthorized User!";
                    }

                    return RedirectToAction("BarCodeSummary");
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
            finally
            {
            }
            return View();
        }

        [HttpGet]
        public ActionResult BarCodeSummaryPrint()
        {
            try
            {
                BarcodeGenerationModel records = new BarcodeGenerationModel();
                records.BModel = ifr.GetDetailForBarcodePrintingSum().ToList();
                if (records.BModel.Count == 0) { ViewBag.Message = "No records found"; }
                ViewBag.Status = "NOT PRINTED";
                ViewBag.Source = "--Select Source--";
                Session["BarCodePrintdownload"] = records.BModel;
                return View(records);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
            finally
            {
            }
        }

        [HttpPost]
        public ActionResult BarCodeSummaryPrint(string[] ids, string locode, string brname, string assetid, string type, string barcode, string status,
            string source, string awbno, string courier, string despdate, string command
            )
        {
            BarcodeGenerationModel details = new BarcodeGenerationModel();
            string filename = "";
            try
            {
                details.BModel = ifr.GetDetailForBarcodePrintingSum().ToList();
                if (command == "SEARCH")
                {
                    details.BModel = ifr.GetDetailForBarcodePrintingSearch().ToList();
                    if ((string.IsNullOrEmpty(locode) || string.IsNullOrWhiteSpace(locode)) &&
                        (string.IsNullOrEmpty(brname) || string.IsNullOrWhiteSpace(brname)) &&
                        (string.IsNullOrEmpty(assetid) || string.IsNullOrWhiteSpace(assetid)) &&
                        (string.IsNullOrEmpty(type) || string.IsNullOrWhiteSpace(type)) &&
                        (string.IsNullOrEmpty(barcode) || string.IsNullOrWhiteSpace(barcode)) &&
                        (status == "--Select Status--") && (source == "--Select Source--") &&
                        (string.IsNullOrEmpty(awbno) || string.IsNullOrWhiteSpace(awbno)) &&
                        (string.IsNullOrEmpty(courier) || string.IsNullOrWhiteSpace(courier)) &&
                        (string.IsNullOrEmpty(despdate) || string.IsNullOrWhiteSpace(despdate)))
                    {
                        details.BModel = ifr.GetDetailForBarcodePrintingSum().ToList();
                    }
                    if (status == "--Select Status--")
                    { ViewBag.Status = "--Select Status--"; }
                    if (source == "--Select Source--")
                    { ViewBag.Source = "--Select Source--"; }
                    if (!string.IsNullOrEmpty(locode))
                    {
                        ViewBag.loccode = locode;
                        details.BModel = details.BModel.Where(x => locode.ToUpper() == null || (x._loc.ToUpper().Contains(locode.ToUpper()))).ToList();
                    }
                    if(!string.IsNullOrEmpty(brname))
                    {
                        ViewBag.brname = brname;
                        details.BModel = details.BModel.Where(x => brname.ToUpper() == null || (x._brname.ToUpper().Contains(brname.ToUpper()))).ToList();

                    }
                    if (!string.IsNullOrEmpty(assetid))
                    {
                        ViewBag.assetid = assetid;
                        details.BModel = details.BModel.Where(x => assetid.ToUpper() == null || (x._asset_id.ToUpper().Contains(assetid.ToUpper()))).ToList();
                    }
                    if (!string.IsNullOrEmpty(type))
                    {
                        ViewBag.type = type;
                        details.BModel = details.BModel.Where(x => type.ToUpper() == null || (x._asset_type.ToUpper().Contains(type.ToUpper()))).ToList();
                    }
                    if (!string.IsNullOrEmpty(barcode))
                    {
                        ViewBag.barcode = barcode;
                        details.BModel = details.BModel.Where(x => barcode.ToUpper() == null || (x._barcode.ToUpper().Contains(barcode.ToUpper()))).ToList();
                    }
                    if (status != "--Select Status--")
                    {
                        ViewBag.Status = status;
                        details.BModel = details.BModel.Where(x => status.ToUpper() == null || (x._barcode_status.ToUpper().Equals(status.ToUpper()))).ToList();
                    }
                    if (source != "--Select Source--")
                    {
                        ViewBag.Source = source;
                        details.BModel = details.BModel.Where(x => source.ToUpper() == null || (x._source_name.ToUpper().Equals(source.ToUpper()))).ToList();
                    }
                    if (!string.IsNullOrEmpty(awbno))
                    {
                        ViewBag.awbno = awbno;
                        details.BModel = details.BModel.Where(x => type.ToUpper() == null || (x._awb_no.ToUpper().Contains(awbno.ToUpper()))).ToList();
                    }
                    if (!string.IsNullOrEmpty(courier))
                    {
                        ViewBag.courier = courier;
                        details.BModel = details.BModel.Where(x => courier.ToUpper() == null || (x._courier_name.ToUpper().Contains(courier.ToUpper()))).ToList();
                    }
                    if (!string.IsNullOrEmpty(despdate))
                    {
                        ViewBag.despdate = despdate;
                        details.BModel = details.BModel.Where(x => despdate.ToUpper() == null || (x._despatch_date.ToUpper().Contains(despdate.ToUpper()))).ToList();
                    }
                    if (details.BModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    Session["BarCodePrintdownload"] = details.BModel;
                    return View(details);
                }

                if (command == "Clear")
                {
                    return RedirectToAction("BarCodeSummaryPrint");
                }
                else if (command == "PrintBarcode")
                {
                    List<string> id = new List<string>();
                    if (ids != null)
                    {
                        id = ids.ToList();
                        filename = ifr.Barcode(id);

                    }
                }
                return RedirectToAction("BarCodeSummaryPrint");
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
        }

        [HttpPost]
        public JsonResult SaveDespatch(BarcodeGenerationModel model, string ids)
        {

            string result = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    result = ifr.Savedespatch(model, ids);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public FileResult DownloadDocument(string id)
        {
            string barcodepath = ConfigurationManager.AppSettings["Barcodepath"].ToString();
            string barcode = string.Empty;
            try
            {
                string filename = id;
                var path = Path.Combine(barcodepath + filename + ".pdf");
                return File(path, System.Net.Mime.MediaTypeNames.Application.Octet, filename + ".pdf");
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return File("", "");
            }

        }

        public JsonResult DdlStatus()
        {

            try
            {
                return Json(ifr.GetddlStatus(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }
        }
        public JsonResult DdlSource()
        {
            try
            {
                return Json(ifr.GetddlSource(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            finally
            {
            }
        }

        [HttpGet]
        public void PrintBarcodes(string ids = null)
        {
            try
            {

                string[] ids1 = ids.Split(',');
                BarcodeGenerationModel bgm = new BarcodeGenerationModel();
                List<string> id = new List<string>();
                if (ids1 != null)
                {
                    id = ids1.ToList();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Document pdfdoc = new Document();
                        iTextSharp.text.Rectangle pagesize = new iTextSharp.text.Rectangle(350, 160, 10, 0);
                        pdfdoc = new Document(pagesize, 0, 0, 0, 0);
                        PdfWriter writer = PdfWriter.GetInstance(pdfdoc, ms);
                        PdfPTable tbl = new PdfPTable(3);
                        tbl.SpacingBefore = 2;
                        tbl.SpacingAfter = 2;
                        float[] widths = {
       		                40f,
                            50f,
                            40f
        	                };
                        tbl.SetWidths(widths);
                        tbl.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        tbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        tbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        pdfdoc.Open();
                        for (int i = 0; i < id.Count; i++)
                        {
                            if (id[i] != "undefined" && id[i] != "")
                            {
                                for (int j = 0; j < 1; j++)
                                {
                                    bgm = new BarcodeGenerationModel();

                                    DataTable dt = ifr.GetDetailsForBarcodePrint(id[i]);
                                    foreach (DataRow row in dt.Rows)
                                    {
                                        bgm._barcode = row["barcodedetials_bar_no"].ToString();
                                        bgm._barcode_status = row["barcodedetials_bar_status"].ToString();
                                        //bgm._asset_type = row["asset_type"].ToString();
                                        //if (bgm._asset_type == "M")
                                        //{
                                        //    bgm._asset_type = "Moveable Asset";
                                        //}
                                        //else
                                        //{
                                        //    bgm._asset_type = "Non-Moveable Asset";
                                        //}
                                        bgm._assetcode_gid = row["asset_code"].ToString();
                                        bgm._assetDesp = row["asset_description"].ToString();
                                        bgm._asset_sno = row["assetdetails_asset_serialno"].ToString();
                                        bgm._asset_id = row["assetdetails_assetdet_id"].ToString();
                                        bgm._comname = "10 - FICCL";
                                        //bgm._loc = row["branch_code"].ToString();
                                    }
                                    string strcode = bgm._barcode;
                                    PdfContentByte pdtcb = writer.DirectContent;
                                    Barcode128 code128 = new Barcode128();
                                    code128.Code = strcode;
                                    code128.Extended = true;
                                    code128.ChecksumText = true;
                                    code128.StartStopText = true;
                                    code128.CodeType = iTextSharp.text.pdf.Barcode.CODE128;
                                    code128.BarHeight = 30.00F;
                                    code128.Size = 10;
                                    code128.X = 0.8F;
                                    code128.Baseline = 10.0F;
                                    code128.TextAlignment = Element.ALIGN_CENTER;
                                    iTextSharp.text.Image image128 = code128.CreateImageWithBarcode(pdtcb, null, null);
                                    BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1252, false);
                                    iTextSharp.text.Font tbf = new iTextSharp.text.Font(bf, 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                                    //string text = System.Environment.NewLine + "Asset ID" + " - " + bgm._asset_id + System.Environment.NewLine + bgm._asset_sno + System.Environment.NewLine + "Asset Description" + " - " + bgm._assetDesp + System.Environment.NewLine + bgm._assetcode_gid + System.Environment.NewLine + bgm._comname;
                                    string text =  System.Environment.NewLine + bgm._asset_sno + System.Environment.NewLine + "Asset Description" + " - " + bgm._assetDesp + System.Environment.NewLine + bgm._assetcode_gid + System.Environment.NewLine + bgm._comname;
                                    PdfPCell cell1 = new PdfPCell(new Phrase(System.Environment.NewLine, tbf));
                                    cell1.Border = 0;
                                    cell1.Padding = 15;
                                    cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    PdfPCell cell2 = new PdfPCell(new Phrase(text, tbf));
                                    cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                                    cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
                                    cell2.Border = 0;
                                    cell2.Padding = 10;
                                    tbl.AddCell(cell1); tbl.AddCell(cell1); tbl.AddCell(cell1);
                                    tbl.AddCell(new Phrase(new Chunk(image128, 75, -30)));
                                    tbl.AddCell(cell1); tbl.AddCell(cell1); tbl.AddCell(cell1);
                                    tbl.AddCell(cell2); tbl.AddCell(cell1);
                                }
                            }
                        }
                        if (tbl.Rows.Count != 0)
                        {
                            pdfdoc.Add(tbl);
                            pdfdoc.Close();
                            writer.Close();
                            string fileprintname = "Barcode " + DateTime.Today;
                            Response.ContentType = "pdf/application";
                            Response.AddHeader("content-disposition",
                            "attachment;filename=" + fileprintname + ".pdf");
                            Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                            Response.End();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
        }
        public ActionResult BarCodeGenerationExceldownload()
        {

            List<BarcodeGenerationModel> cList;
            cList = (List<BarcodeGenerationModel>)Session["BarCodeGenerationExceldownload"];

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("Asset ID");
            dt.Columns.Add("Category");
            dt.Columns.Add("Sub-Category Code");
            dt.Columns.Add("Sub-Category");
            dt.Columns.Add("Branch");
            dt.Columns.Add("Capitalization Date");
            dt.Columns.Add("Asset SNo");
            dt.Columns.Add("Department");
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                i + 1
                , cList[i]._asset_id.ToString()
                , cList[i]._assetCode.ToString()
                , cList[i]._assetcode_gid.ToString()
                , cList[i]._assetDesp.ToString()
                , cList[i]._loc.ToString()
                , cList[i]._cap_date.ToString()
                , cList[i]._asset_sno.ToString()
                , cList[i]._inv_no.ToString()
                );
            }


            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            // Session["exceldownload"] = gv;
            if (gv.Rows.Count != 0)
            {
                //return new DownloadFileActionResult((GridView)Session["exceldownload"], "DocumentGroup.xls");
                return new DownloadFileActionResult(gv, "BarCode Generation Summary.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }
        public ActionResult BarCodePrintdownload()
        {

            List<BarcodeGenerationModel> cList;
            cList = (List<BarcodeGenerationModel>)Session["BarCodePrintdownload"];

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("Barcode");
            dt.Columns.Add("Branch");
            dt.Columns.Add("Branch Name");
            dt.Columns.Add("Asset ID");
            dt.Columns.Add("Asset Slno");
            dt.Columns.Add("Asset Type");
            dt.Columns.Add("AWB No");
            dt.Columns.Add("Courier Name");
            dt.Columns.Add("Despatch Date");
            dt.Columns.Add("Asset Source");
            dt.Columns.Add("Barcode Status");
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                i + 1
                , cList[i]._barcode.ToString()
                , cList[i]._loc.ToString()
                , cList[i]._brname.ToString()
                , cList[i]._asset_id.ToString()
                , cList[i]._asset_sno.ToString()
                , cList[i]._asset_type.ToString()
                , cList[i]._awb_no.ToString()
                , cList[i]._courier_name.ToString()
                , cList[i]._despatch_date.ToString()
                , cList[i]._source_name.ToString()
                , cList[i]._barcode_status.ToString()
                                     );
            }


            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            // Session["exceldownload"] = gv;
            if (gv.Rows.Count != 0)
            {
                //return new DownloadFileActionResult((GridView)Session["exceldownload"], "DocumentGroup.xls");
                return new DownloadFileActionResult(gv, "BarCode Printing Summary.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }
    }
}
