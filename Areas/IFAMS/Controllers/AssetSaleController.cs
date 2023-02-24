using IEM.Areas.IFAMS.Models;
using IEM.Common;
//using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Excel;
using System.Threading.Tasks;
using System.Data;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI;
using ClosedXML.Excel;
using Newtonsoft.Json;
using IEM.App_Start;

namespace IEM.Areas.IFAMS.Controllers
{

    [NoDirectAccess]
    public class AssetSaleController : Controller
    {
        private IfamsRepository_M ifr;

        private IRepository Tfr;
        private CmnFunctions objCmnFunctions = new CmnFunctions();
        private FileServer Cmnfs = new FileServer();
        ErrorLog objErrorLog = new ErrorLog();
        public AssetSaleController() : this(new IfamsDataModel_M(), new DataModel()) { }
        //public SupplierEvaluationController(ASMSIRepository Objist, Asms_IRepository od)
        //{
        //    ist = Objist;
        //    mst = od;
        //}
        public AssetSaleController(IfamsRepository_M objModel, IRepository obTOA)
        {
            ifr = objModel;
            Tfr = obTOA;
        }

        [HttpGet]
        public ActionResult SASummary()
        {
            try
            {
                SaleMakerModel records = new SaleMakerModel();
                Session["saTransferNo"] = null;
                Session["saStatus"] = null;
                Session["saErrdatatable"] = null;
                Session["samaindatatable"] = null;
                Session["sasupplierattmtfileexcel"] = null;
                Session["saorginal"] = null;
                Session["saerr"] = null;
                Session["saTempdataset"] = null;
                Session["safilename"] = null;
                Session["saTempdatatable"] = null;
                Session["samaindatatable"] = null;
                Session["sagid"] = null;
                string status = Request.QueryString["reqstatus"];
                ViewBag.status = "DRAFT";
                ViewBag.stsrejected = "REJECTED";
                string success = ifr.getTheUser("SOAMKR");
                ViewBag.success = success;
                if (success == "success")
                {
                    // records.TModel = ifr.GetSoaDetailDraft(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                    records.TModel = ifr.GetSoaDetailMaker(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                    if (records.TModel.Count == 0) { ViewBag.Message = "No records found"; }
                    else
                    {
                        ViewBag.status = "DRAFT";
                    }
                    Session["saleexcel"] = records.TModel;
                    return View(records);
                }
                else if (status == ViewBag.Status)
                {
                    records.TModel = ifr.GetSoaDetailDraft(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                    if (records.TModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    Session["saleexcel"] = records.TModel;
                    return View(records);
                }

                else if (status == ViewBag.stsrejected)
                {
                    records.TModel = ifr.GetSoaDetailReject(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                    if (records.TModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    Session["saleexcel"] = records.TModel;
                    return View(records);
                }

                else
                {
                    records.TModel = ifr.GetSoaDetailDraft(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                    if (records.TModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    Session["saleexcel"] = records.TModel;
                    return View(records);
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
        }

        [HttpPost]
        public ActionResult SASummary(string filter, string filter1, FormCollection collections, string command, string status)
        {
            SaleMakerModel sadetails = new SaleMakerModel();
            if (command == "SEARCH" && status == "DRAFT")
            {
                sadetails.TModel = ifr.GetSoaDetailDraft(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();

                if ((string.IsNullOrEmpty(filter) || string.IsNullOrWhiteSpace(filter)) && (string.IsNullOrEmpty(filter1) || string.IsNullOrWhiteSpace(filter1)) && (status == "--Select Status--"))
                {
                    sadetails.TModel = ifr.GetSoaDetailDraft(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                }
                if (filter != null)
                {
                    ViewBag.filter = filter;
                    sadetails.TModel = sadetails.TModel.Where(x => filter.ToUpper() == null || (x.soaSalenumber.Contains(filter.ToUpper()))).ToList();
                }
                if (filter1 != null)
                {
                    ViewBag.filter1 = filter1;
                    sadetails.TModel = sadetails.TModel.Where(x => filter1 == null || (x.soaSaledate.Contains(filter1.ToUpper()))).ToList();
                }
                if (status != "--Select Status--")
                {
                    ViewBag.status = status;
                    sadetails.TModel = sadetails.TModel.Where(x => status.ToUpper() == null || (x.soaStatus.Equals(status.ToUpper()))).ToList();
                }
                if (sadetails.TModel.Count == 0)
                {
                    ViewBag.Message = "NO records found";
                }
                Session["saleexcel"] = sadetails.TModel;
                return View(sadetails);
            }

            if (command == "SEARCH" && status == "INPROCESS")
            {
                sadetails.TModel = ifr.GetSoaDetailSummary(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();

                if ((string.IsNullOrEmpty(filter) || string.IsNullOrWhiteSpace(filter)) && (string.IsNullOrEmpty(filter1) || string.IsNullOrWhiteSpace(filter1)) && (status == "--Select Status--"))
                {
                    sadetails.TModel = ifr.GetSoaDetailSummary(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                }
                if (filter != null)
                {
                    ViewBag.filter = filter;
                    sadetails.TModel = sadetails.TModel.Where(x => filter.ToUpper() == null || (x.soaSalenumber.Contains(filter.ToUpper()))).ToList();
                }
                if (filter1 != null)
                {
                    ViewBag.filter1 = filter1;
                    sadetails.TModel = sadetails.TModel.Where(x => filter1 == null || (x.soaSaledate.Contains(filter1.ToUpper()))).ToList();
                }
                if (status != "--Select Status--")
                {
                    ViewBag.status = status;
                    sadetails.TModel = sadetails.TModel.Where(x => status.ToUpper() == null || (x.soaStatus.Equals(status.ToUpper()))).ToList();
                }
                if (sadetails.TModel.Count == 0)
                {
                    ViewBag.Message = "NO records found";
                }
                Session["saleexcel"] = sadetails.TModel;
                return View(sadetails);
            }

            if (command == "SEARCH" && status == "REJECTED")
            {
                sadetails.TModel = ifr.GetSoaDetailReject(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();

                if ((string.IsNullOrEmpty(filter) || string.IsNullOrWhiteSpace(filter)) && (string.IsNullOrEmpty(filter1) || string.IsNullOrWhiteSpace(filter1)) && (status == "--Select Status--"))
                {
                    sadetails.TModel = ifr.GetSoaDetailSummary(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                }
                if (filter != null)
                {
                    ViewBag.filter = filter;
                    sadetails.TModel = sadetails.TModel.Where(x => filter.ToUpper() == null || (x.soaSalenumber.Contains(filter.ToUpper()))).ToList();
                }
                if (filter1 != null)
                {
                    ViewBag.filter1 = filter1;
                    sadetails.TModel = sadetails.TModel.Where(x => filter1 == null || (x.soaSaledate.Contains(filter1.ToUpper()))).ToList();
                }
                if (status != "--Select Status--")
                {
                    ViewBag.status = status;
                    sadetails.TModel = sadetails.TModel.Where(x => status.ToUpper() == null || (x.soaStatus.Equals(status.ToUpper()))).ToList();
                }
                if (sadetails.TModel.Count == 0)
                {
                    ViewBag.Message = "NO records found";
                }
                Session["saleexcel"] = sadetails.TModel;
                return View(sadetails);
            }

            if (command == "SEARCH" && status == "WAITING FOR APPROVAL")
            {
                sadetails.TModel = ifr.GetSoaDetailMakerWFAP(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();

                if ((string.IsNullOrEmpty(filter) || string.IsNullOrWhiteSpace(filter)) && (string.IsNullOrEmpty(filter1) || string.IsNullOrWhiteSpace(filter1)) && (status == "--Select Status--"))
                {
                    sadetails.TModel = ifr.GetSoaDetailMaker(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                }
                if (filter != null)
                {
                    ViewBag.filter = filter;
                    sadetails.TModel = sadetails.TModel.Where(x => filter.ToUpper() == null || (x.soaSalenumber.Contains(filter.ToUpper()))).ToList();
                }
                if (filter1 != null)
                {
                    ViewBag.filter1 = filter1;
                    sadetails.TModel = sadetails.TModel.Where(x => filter1 == null || (x.soaSaledate.Contains(filter1.ToUpper()))).ToList();
                }
                if (status != "--Select Status--")
                {
                    ViewBag.status = status;
                    sadetails.TModel = sadetails.TModel.Where(x => status.ToUpper() == null || (x.soaStatus.Equals(status.ToUpper()))).ToList();
                }
                if (sadetails.TModel.Count == 0)
                {
                    ViewBag.Message = "NO records found";
                }
                Session["saleexcel"] = sadetails.TModel;
                return View(sadetails);
            }

            if (command == "SEARCH" && status == "APPROVED")
            {
                sadetails.TModel = ifr.GetSoaDetailCheckerApproved(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();

                if ((string.IsNullOrEmpty(filter) || string.IsNullOrWhiteSpace(filter)) && (string.IsNullOrEmpty(filter1) || string.IsNullOrWhiteSpace(filter1)) && (status == "--Select Status--"))
                {
                    sadetails.TModel = ifr.GetSoaDetailCheckerApproved(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                }
                if (filter != null)
                {
                    ViewBag.filter = filter;
                    sadetails.TModel = sadetails.TModel.Where(x => filter.ToUpper() == null || (x.soaSalenumber.Contains(filter.ToUpper()))).ToList();
                }
                if (filter1 != null)
                {
                    ViewBag.filter1 = filter1;
                    sadetails.TModel = sadetails.TModel.Where(x => filter1 == null || (x.soaSaledate.Contains(filter1.ToUpper()))).ToList();
                }
                if (status != "--Select Status--")
                {
                    ViewBag.status = status;
                    sadetails.TModel = sadetails.TModel.Where(x => status.ToUpper() == null || (x.soaStatus.Equals(status.ToUpper()))).ToList();
                }
                if (sadetails.TModel.Count == 0)
                {
                    ViewBag.Message = "NO records found";
                }
                Session["saleexcel"] = sadetails.TModel;
                return View(sadetails);
            }



            if (command == "Clear")
            {
                return RedirectToAction("SASummary", "AssetSale");
            }
            else if (command == "SALE")
            {
                return RedirectToAction("SASummary");
            }


            return View();
        }

        public ActionResult saledownloadexcel()
        {
            using (DataTable dtnew = new DataTable())
            {
                dtnew.Columns.Add("SNo");
                dtnew.Columns.Add("Asset Id");
                dtnew.Columns.Add("Cheque Split Amount");
                dtnew.Columns.Add("Inward No");
                dtnew.Columns.Add("HSN Code");
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtnew, "Sale_Template");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=Sale_Template.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            return View();
        }

        //public ActionResult saledownloadexcel()
        //{
        //    DataTable dtnew = new DataTable();
        //    dtnew.Columns.Add("SNo");
        //    dtnew.Columns.Add("Asset Id");
        //    dtnew.Columns.Add("Cheque Split Amount");
        //    //dtnew.Columns.Add("Cheque Sale Amount");
        //    dtnew.Columns.Add("Inward No");
        //    Response.ClearContent();
        //    Response.Buffer = true;
        //    Response.AddHeader("content-disposition",string.Format("attachment; filename={0}", "Sale_Template.xls"));
        //    Response.ContentType = "application/ms-excel";
        //    DataTable dt = dtnew;
        //    string str = string.Empty;
        //    foreach (DataColumn dtcol in dt.Columns)
        //    {
        //        Response.Write(str + dtcol.ColumnName);
        //        str = "\t";
        //    }
        //    Response.Write("\n");
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        str = "";
        //        for (int j = 0; j < dt.Columns.Count; j++)
        //        {
        //            Response.Write(str + Convert.ToString(dr[j]));
        //            str = "\t";
        //        }
        //        Response.Write("\n");
        //    }
        //    Response.End();
        //    return View();
        //}

        public void SaleUploadFilesnew()
        {
            try
            {
                string filename = "";
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];

                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        HttpPostedFileBase hpf = Request.Files[file] as HttpPostedFileBase;
                        Session["supplierattmtfileexcel"] = hpf;
                        Session["saoriginal"] = hpf.FileName;
                    }
                }
                // return Json(filename);
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                //return Json("Uplopad failed");
            }
        }


        [HttpPost]
        public async Task<JsonResult> SaleUploadFiles()
        {
            string Extension1 = "";
            string error = "";
            try
            {
                string path1 = "";
                if (Session["supplierattmtfileexcel"] != null)
                {
                    HttpPostedFileBase savefile = (HttpPostedFileBase)Session["supplierattmtfileexcel"];
                    string Extension = Path.GetFileName(savefile.FileName);
                    string n = string.Format("SaleOfAsset-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                    Extension1 = System.IO.Path.GetExtension(savefile.FileName);
                    Session["filename"] = n + Extension;
                    var CmnFilePath = objCmnFunctions.IEMAttachmentPath();
                    path1 = CmnFilePath + n + Extension;
                    // path1 = @"C:\temp\" + n + Extension;
                    if (System.IO.File.Exists(path1))
                    {
                        System.IO.File.Delete(path1);
                    }
                    savefile.SaveAs(path1);
                }
                List<SaleSheetData> objparent = new List<SaleSheetData>();
                SaleSheetData objmodel1;
                int count = 0;
                string sheets = "";
                FileStream stream = new FileStream(path1, FileMode.Open, FileAccess.Read);
                DataSet result1 = new DataSet();
                if (Extension1 == ".xlsx")
                {
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    excelReader.IsFirstRowAsColumnNames = true;
                    result1 = excelReader.AsDataSet();
                    excelReader.Close();
                }
                else if (Extension1 == ".xls")
                {
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
                    excelReader.IsFirstRowAsColumnNames = true;
                    result1 = excelReader.AsDataSet();
                    excelReader.Close();
                }
                else
                {
                    error = "Error";
                    count++;
                    objmodel1 = new SaleSheetData();
                    objmodel1.saleSheetid = count;
                    objmodel1.saleSheetname = error.ToString();
                    objparent.Add(objmodel1);
                }

                foreach (DataTable dt in result1.Tables)
                {

                    sheets = dt.TableName.ToString().Trim();
                    count++;
                    objmodel1 = new SaleSheetData();
                    objmodel1.saleSheetname = "";
                    objmodel1.saleSheetid = count;
                    objmodel1.saleSheetname = sheets.ToString();
                    objparent.Add(objmodel1);
                }

                Session["saTempdataset"] = result1;
                return Json(objparent.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }

        }

        [HttpGet]
        public PartialViewResult SaUploadstatus(string ddlSelectsheet)
        {
            try
            {
                if (Session["saTempdatatable"] != null)
                {
                    DataTable errdataval = new DataTable();
                    errdataval = (DataTable)Session["saTempdatatable"];
                    // datasupload(errdataval);
                    if (errdataval.Rows.Count == 0)
                    {
                        ViewBag.visible = "Yes";
                    }
                }
            }
            catch
            {

            }
            return PartialView();
        }

        public JsonResult SaleMakerUpdate(SaleMakerModel obj)
        {
            string mag = "";
            string strCols = "";
            string[] strColArr;
            try
            {
                DataSet result1 = new DataSet();
                result1 = (DataSet)Session["saTempdataset"];
                var dataTable = new DataTable();
                dataTable = result1.Tables[obj.saleSheetname.ToString()];
                int count = dataTable.Rows.Count;
                foreach (DataColumn dtColumn in dataTable.Columns)
                {
                    strCols = strCols + dtColumn.ColumnName.Trim() + ":";
                }
                strCols = strCols.Substring(0, strCols.Length - 1);
                strColArr = strCols.Split(':');
                if (strColArr[0].ToString() == "SNo"
                    && strColArr[1].ToString() == "Asset Id"
                    && strColArr[2].ToString() == "Cheque Split Amount"
                    && strColArr[3].ToString() == "Inward No"
                    && strColArr[4].ToString() == "HSN Code")
                //&& strColArr[2].ToString() == "Cheque Sale Amount")
                {
                    mag = "Success";
                }

                else
                {
                    mag = "Faild";
                }

                if (mag == "Success")
                {
                    for (int i = 0; i < count; i++)
                    {
                        if (dataTable.Rows[i]["SNo"].ToString() == "" && dataTable.Rows[i]["Asset Id"].ToString() == ""
                            && dataTable.Rows[i]["Cheque Split Amount"].ToString() == "" && dataTable.Rows[i]["Inward No"].ToString() == ""
                            && dataTable.Rows[i]["HSN Code"].ToString() == ""
                            //&& dataTable.Rows[i]["Cheque Sale Amount"].ToString() == ""
                            )
                        {
                            dataTable.Rows[i].Delete();
                            count = count - 1;
                            i = i - 1;
                            dataTable.AcceptChanges();

                        }
                    }
                    Session["saTempdatatable"] = dataTable;
                }

                return Json(mag, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public PartialViewResult SaleUploadstatus(string ddlSelectSheetsa)
        {
            try
            {
                if (Session["saTempdatatable"] != null)
                {
                    DataTable errdataval = new DataTable();
                    errdataval = (DataTable)Session["saTempdatatable"];
                    saledatasupload(errdataval);
                    errdataval = (DataTable)Session["saErrdatatable"];
                    if (errdataval.Rows.Count == 0)
                    {
                        ViewBag.visble = "Yes";
                    }
                }
            }
            catch
            {

            }
            return PartialView();
        }

        private void saledatasupload(DataTable dataTable)
        {
            int sno = 0;
            int totalrecord = 0;
            int invalid = 0;
            int valid = 0;
            DataTable maindatatable = new DataTable();
            Hashtable empchck = new Hashtable();
            maindatatable = dataTable;
            totalrecord = maindatatable.Rows.Count;
            DataTable Errdatatable = new DataTable();
            Errdatatable.Columns.Add("SNo");
            Errdatatable.Columns.Add("Line");
            Errdatatable.Columns.Add("Error Description");
            try
            {
                if (dataTable.Rows.Count > 0)
                {
                    string exceltxt = "";
                    string assetdata = "";
                    string status = "";
                    string errs = "";
                    string sagrpId1 = "";
                    string sagrpId2 = "";
                    string Assetid = "";
                    string strRowCommaSeparated = "";
                    int RowNo = 0;
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        errs = "";
                        RowNo = i + 1;
                        assetdata = dataTable.Rows[i][1].ToString();
                        //////**************************

                        if (strRowCommaSeparated.Contains(assetdata) == true)
                        {

                            errs = "Duplicate Asset id Found";
                        }
                        else
                        {


                            if (dataTable.Rows[i][1].ToString() == "")
                            {
                                errs = "Asset Id must not be empty ";
                            }
                            else
                            {
                                exceltxt = dataTable.Rows[i][1].ToString();
                                status = ifr.GetSaleStatusexcel(assetdata, exceltxt, "AssetId");
                                if (status == "Free")
                                {
                                    status = ifr.GetSaleStatusexcel(assetdata, exceltxt, "AssetIdProcess");
                                    if (status == "Free")
                                    {
                                        strRowCommaSeparated += strRowCommaSeparated == "" ? assetdata : ("," + assetdata);
                                        sagrpId1 = Tfr.GetGroupGid(dataTable.Rows[0][1].ToString());
                                        sagrpId2 = Tfr.GetGroupGid(assetdata);
                                        if (sagrpId2 == sagrpId1 || sagrpId2 == "")
                                        {
                                            sagrpId1 = ifr.GetGrpCountSA(sagrpId1);
                                            if (sagrpId1 != "0")
                                                if (Convert.ToInt32(sagrpId1) != dataTable.Rows.Count)
                                                {
                                                    if (errs == "")
                                                    {
                                                        errs = "All assets in the Group are not provided.";
                                                    }
                                                    else
                                                    {
                                                        errs = errs + "," + "All assets in the Group are not Provided.";
                                                    }
                                                }
                                        }

                                        else
                                        {
                                            if (errs == "")
                                            {
                                                errs = "All Asset Id must be of Same Group.";
                                            }
                                            else
                                            {
                                                errs = errs + "," + "All Asset ID must be of Same Group.";
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (errs == "")
                                        {
                                            errs = status;
                                        }
                                        else
                                        {
                                            errs = errs + "," + status;
                                        }
                                    }
                                }
                                else
                                {
                                    if (errs == "")
                                    {
                                        errs = status;
                                    }
                                    else
                                    {
                                        errs = errs + "," + status;
                                    }
                                }
                            }

                            if (dataTable.Rows[i][2].ToString() == "")
                            {
                                if (errs == "")
                                {
                                    errs = "Cheque Split Amount must not be empty.";
                                }

                                else
                                {
                                    errs = errs + "," + "Cheque Split Amount must not be empty.";
                                }
                            }
                            else
                            {
                                exceltxt = dataTable.Rows[i][2].ToString();
                                if (Convert.ToDecimal(exceltxt) <= 0)
                                {
                                    if (errs == "")
                                    {
                                        errs = "The Cheque Split Amount can't be" + exceltxt;
                                    }
                                    else
                                    {
                                        errs = errs + "," + "The Cheque Split Amount can't be" + exceltxt;
                                    }
                                }
                            }

                            if (dataTable.Rows[i][3].ToString() == "")
                            {
                                if (errs == "")
                                {
                                    errs = "Inward No must not be empty.";
                                }
                                else
                                {
                                    errs = errs + "," + "Inward No must not be empty.";
                                }
                            }

                            else
                            {
                                exceltxt = dataTable.Rows[i][3].ToString();
                                status = ifr.GetSaleStatusexcel(assetdata, exceltxt, "Inward No");
                                if (status != "Free")
                                {
                                    if (errs == "")
                                    {
                                        errs = status;
                                    }
                                    else
                                    {
                                        errs = errs + "," + status;
                                    }
                                }
                            }

                            if (dataTable.Rows[i][4].ToString() == "")
                            {
                                if (errs == "")
                                {
                                    errs = "HSN Code. Should Not be Empty.";
                                }
                                else
                                {
                                    errs = errs + "," + "HSN Code. Should Not be Empty.";
                                }
                            }
                            else
                            {
                                exceltxt = dataTable.Rows[i][4].ToString();
                                status = ifr.GetSaleStatusexcel(assetdata, exceltxt, "HSN");
                                if (status == "notexists")
                                {
                                    if (errs == "")
                                    {
                                        errs = "HSN is not mapped for asset.";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "HSN is not mapped for asset";
                                    }
                                }
                            }

                        }
                        if (i == 0) { Assetid = dataTable.Rows[i][1].ToString(); }
                        else { Assetid = dataTable.Rows[i - 1][1].ToString(); }
                        status = ifr.GetSaleStatusexcel(assetdata, Assetid, "Branch");
                        if (status != "Free")
                        {
                            if (errs == "")
                            {
                                errs = "All Asset ID must be of Same Branch.";
                            }
                            else
                            {
                                errs = errs + "," + "All Asset ID must be of Same Branch.";
                            }
                        }
                        if (errs != "")
                        {
                            sno++;
                            Errdatatable.Rows.Add(sno, RowNo, errs);
                        }
                    }
                }

                else
                {
                    Errdatatable.Rows.Add(1, "Please Select Valid Sheet");
                }

                invalid = Errdatatable.Rows.Count;
                valid = totalrecord - invalid;
                ViewBag.vbvalid = "Total No of Valid Record(s) :" + valid;
                ViewBag.vbinvalid = "Total No of InVaild Record(s) :" + invalid;
                ViewBag.vbtotalrecord = "Total No of Record(s) in Excel file :" + totalrecord;
                Session["saErrdatatable"] = Errdatatable;
                Session["samaindatatable"] = maindatatable;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
        }
        [HttpGet]
        public PartialViewResult salocaldetails(string ddlSelectsheet)
        {
            Session["saerr"] = "flag";
            return PartialView();
        }

        public JsonResult salocaldetails()
        {
            try
            {
                Session["saerr"] = "flag";

                DataTable uploadedData = new DataTable();
                uploadedData = (DataTable)Session["samaindatatable"];
                string check;
                string assettype = "SOA";
                string filename = Session["filename"].ToString();
                //check = ifr.InsertSale(uploadedData, filename).ToString();
                check = ifr.ImportAssetDetails(uploadedData, filename, assettype).ToString();
                return Json(check, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { }
        }



        [HttpPost]
        public JsonResult SAcheckFileExists(string id)
        {
            string result = "0";
            try
            {
                var CmnFilePath = objCmnFunctions.IEMAttachmentPath();
                var localpath1 = Path.Combine(CmnFilePath, id);
                // var localpath1 = Path.Combine(@"C:\temp\", id);
                var apiresult = Cmnfs.DownloadFile(id).Result;
                if (apiresult != "Fail")
                {
                    result = "1";
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        public FileResult SADownloadDocument(string id)
        {
            try
            {
                string filename = id;
                var CmnFilePath = objCmnFunctions.IEMAttachmentPath();
                var sapath = Path.Combine(CmnFilePath, filename);
                //var sapath = Path.Combine(@"C:\temp\", filename);
                var apiresult = Cmnfs.DownloadFile(id).Result;
                if (apiresult == "Fail")
                {
                    return File("", "");
                }
                else
                {
                    byte[] filebyte = Convert.FromBase64String(apiresult);
                    return File(filebyte, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return File("", "");
            }
        }
        //bharathidhasan kumar salemaker  View Button Click Sale
        [HttpGet]
        public PartialViewResult SaleMakerView(string id, string viewfor, FormCollection collections, SaleMakerModel status)
        {

            try
            {
                string userGroup = string.Empty;
                userGroup = ifr.GetUserType(id);
                ViewBag.success = userGroup;
                SaleMakerModel salist = new SaleMakerModel();



                Session["SaleNo"] = id;
                string sastat = ifr.SAGetstus(status, id);
                Session["SAStatus"] = sastat;


                if (viewfor == "REJECTED")
                {
                    salist = ifr.GetSaleDetailFORREJECT(id);
                    status = ifr.GetchqSalDetail(id);

                }

                else
                {
                    salist = ifr.GetSaleDetail(id);
                    status = ifr.GetchqSalDetail(id);
                }


                //salist = ifr.GetSaleDetail(id);
                //status = ifr.GetchqSalDetail(id);
                // statuss.VatModel = new SelectList(ifr.Getvat(), "vatid", "vatstate", salist.vatstate);
                //salist.TModel = SelectList(ifr.Getvat());
                //status.TModel = salist.VatModel;

                status.TModel = salist.TModel;
                status.soadetSalevalue = salist.soadetSalevalue;
                status.soaSalevalue = salist.soaSalevalue;
                if (viewfor == "checkerView")
                {
                    ViewBag.viewfor = "checkerView";
                }
                if (viewfor == "abort")
                {
                    ViewBag.viewfor = "abort";
                }
                if (viewfor == "view")
                {
                    ViewBag.viewfor = "view";

                }
                if (salist.TModel.Count == 0)
                {
                    ViewBag.Message = "No Records Found";
                }
                if (viewfor == "REJECTED")
                {
                    ViewBag.viewfor = "view";
                }

                return PartialView(status);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView();
            }
            finally
            {
            }
        }



        //For query screen view Sale

        [HttpGet]
        public PartialViewResult QueryScreenSaleMakerView(string id, string viewfor, FormCollection collections, SaleMakerModel status)
        {

            try
            {
                string userGroup = string.Empty;
                userGroup = ifr.GetUserType(id);
                ViewBag.success = userGroup;
                SaleMakerModel salist = new SaleMakerModel();



                Session["SaleNo"] = id;
                string sastat = ifr.SAGetstus(status, id);
                Session["SAStatus"] = sastat;


                if (viewfor == "REJECTED")
                {
                    salist = ifr.GetSaleDetailFORREJECT(id);
                    status = ifr.GetchqSalDetail(id);

                }

                else
                {
                    salist = ifr.GetSaleDetail(id);
                    status = ifr.GetchqSalDetail(id);
                }

                status.TModel = salist.TModel;
                status.soadetSalevalue = salist.soadetSalevalue;
                status.soaSalevalue = salist.soaSalevalue;
                if (viewfor == "checkerView")
                {
                    ViewBag.viewfor = "checkerView";
                }
                if (viewfor == "abort")
                {
                    ViewBag.viewfor = "abort";
                }
                if (viewfor == "view")
                {
                    ViewBag.viewfor = "view";

                }
                if (salist.TModel.Count == 0)
                {
                    ViewBag.Message = "No Records Found";
                }
                if (viewfor == "REJECTED")
                {
                    ViewBag.viewfor = "view";
                }

                return PartialView(status);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView();
            }
            finally
            {
            }
        }


        [HttpGet]
        public JsonResult Vendordetails(string vendorcode, int saleid)
        {
            string[,] subresult;
            var json = "";
            try
            {
                if (ModelState.IsValid)
                {
                    subresult = ifr.GetVendor(vendorcode, saleid);
                    json = JsonConvert.SerializeObject(subresult);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        //Bharathidhasan kumar
        //[HttpPost]
        //public JsonResult SaleMakerView(string command, string viewfor, string id, Models.SaleMakerModel status, FormCollection collections, string remarks)
        //{
        //    string Maker = string.Empty, Checker = string.Empty;
        //    //Maker = objCmnFunctions.authorize("281");
        //    //Checker = objCmnFunctions.authorize("225");
        //    int Result = 0;
        //    string Msg = string.Empty;
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        //if (Maker == "Success")
        //        //{
        //            if (command == "abort")
        //            {
        //               ifr.AbortSale(status, id);
        //                ViewBag.viewfor = "abort";
        //                //RedirectToAction("SASummary");
        //            }
        //            if (command == "submit")
        //            {
        //                if (id != "")
        //                {
        //                    Session["Saleno"] = id;
        //                  // Msg= ifr.assetdetailssave(status);
        //                    Msg = ifr.AssetSaveDetails(status);
        //                    //RedirectToAction("SASummary");
        //                }
        //            }
        //      //  }
        //            //if (Checker == "Success")
        //            //{
        //            if (command == "Approve")
        //            {
        //                if (id != "")
        //                {
        //                    Session[""] = id;
        //                   ifr.SAChkApprovalStatus(id, "APPROVED", remarks);
        //                    RedirectToAction("SAChkSummary");
        //                }
        //            }
        //            if (command == "Reject")
        //            {
        //                if (id != "")
        //                {
        //                    Session["Saleno"] = id;
        //                    ifr.SAChkApprovalStatus(id, "REJECTED", remarks);
        //                    RedirectToAction("SAChkSummary");
        //                }
        //            }
        //      //  }

        //        return Json(new { command, Maker, Checker }, JsonRequestBehavior.AllowGet);
        //    }

        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //        return Json("", JsonRequestBehavior.AllowGet);
        //    }
        //    finally
        //    {
        //    }
        //}
        
        //Submit and Approve SOA
        [HttpPost]
        public JsonResult SaleMakerView(Models.SaleMakerModel status)
        {
            string Checker = string.Empty;
            string Maker = string.Empty;
            int Result = 0;
            string Msg = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                if (status.soaSalenumber != "")
                {
                    Session["SaleNumber"] = status.soaSalenumber;
                    dt = ifr.AssetSaveDetails(status);
                    Msg = dt.Rows[0][0].ToString();
                    Result = Convert.ToInt32(dt.Rows[0][1].ToString());
                }

                return Json(new { status.action_status, Msg, Result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
            finally
            {

            }
        }



        public ActionResult Sale_ERRORLOG()
        {
            //Session["Errdatatable"] = Errdatatable;
            Session["saerr"] = "";
            DataTable dtnew = (DataTable)Session["saErrdatatable"];
            DataTable dtmainnew = (DataTable)Session["samaindatatable"];
            dtmainnew.Columns.Add("Error Description");
            for (int i = 1; i <= dtmainnew.Rows.Count; i++)
                for (int j = 0; j < dtnew.Rows.Count; j++)
                {
                    int line = Convert.ToInt32(dtnew.Rows[j]["Line"]);
                    if (i == line)
                        dtmainnew.Rows[line - 1]["Error Description"] = dtnew.Rows[j]["Error Description"];
                }
            DataTable dt = dtmainnew;
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Sale_Template");

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Sale_Errorlog.xlsx");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    Response.End();
                }
            }
            //Response.ClearContent();
            //Response.Buffer = true;
            //Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Sale_Errorlog.xls"));
            ////Response.AddHeader("content-disposition", string.Format("attachement; filename={0}", "SaleofAsset.xls"));
            //Response.ContentType = "application/ms-excel";
            //DataTable dt = dtnew;
            //string str = string.Empty;
            //foreach (DataColumn dtcol in dt.Columns)
            //{
            //    Response.Write(str + dtcol.ColumnName);
            //    str = "\t";
            //}
            //Response.Write("\n");
            //foreach (DataRow drr in dt.Rows)
            //{
            //    str = "";
            //    for (int j = 0; j < dt.Columns.Count; j++)
            //    {
            //        Response.Write(str + Convert.ToString(drr[j]));
            //        str = "\t";
            //    }
            //    Response.Write("\n");
            //}
            ////Response.AddHeader("content-disposition", string.Format("attachment; filename{0}", "Sale_ERRORLOG.xls"));
            //Response.End();
            return View();
        }

        public ActionResult SAExcelDown()
        {
            string sa = null;
            List<SaleMakerModel> saList;
            if (Session["saleexcel"] == null)
            {
                saList = ifr.GetSADetail(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
            }
            else
            {
                saList = (List<SaleMakerModel>)Session["saleexcel"];
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("S.No");
            dt.Columns.Add("Sale No");
            dt.Columns.Add("Sale Amount");
            dt.Columns.Add("Sale Date");
            dt.Columns.Add("Status");
            for (int i = 0; i < saList.Count; i++)
            {
                dt.Rows.Add(i + 1,
                    saList[i].soaSalenumber.ToString(),
                    saList[i].soaSalevalue.ToString(),
                    saList[i].soaSaledate.ToString(),
                    saList[i].soaStatus.ToString()
                    );
            }

            GridView GV = new GridView();
            GV.DataSource = dt;
            GV.DataBind();
            Session["Summary"] = GV;
            if (GV.Rows.Count != 0)
            {
                return new DownloadFileActionResult((GridView)Session["Summary"], "Sale Summary.xls");
            }
            else
            {
                ViewBag.Message = "No Records Found";
            }
            return RedirectToAction("SASummary");
        }

        [HttpGet]
        public PartialViewResult BulkSale()
        {
            try
            {
                return PartialView();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        [HttpGet]
        public PartialViewResult soaAttach(int id, string viewfor)
        {
            SaleMakerModel sam = new SaleMakerModel();
            try
            {

                if (viewfor == "viewmode")
                {
                    ViewBag.viewfor = "view";
                }
                else
                {
                    ViewBag.viewfor = "edit";
                }
                if (id != 0)
                {
                    sam.soaGid = id;
                    sam.attach_list = Tfr.Getattachment(id.ToString(), Tfr.GetRef("SOA").ToString(), Tfr.GetAttachType().ToString());
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
            return PartialView(sam);
        }


        [HttpPost]
        public virtual ActionResult SubUploadedFiles()
        {
            string subfilename = "";
            bool saisUploaded = false;
            string samessage = "File upload failed";
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase myFile = Request.Files["soaUploadfilename"];
                if (myFile != null && myFile.ContentLength != 0)
                {
                    string pathForSaving = HttpContext.Application["path"] as string;

                    try
                    {
                        subfilename = Path.GetFileNameWithoutExtension(myFile.FileName);
                        subfilename = objCmnFunctions.GetSequenceNoFam("SOAA");
                        var subfileextension = Path.GetExtension(myFile.FileName);
                        var substream = myFile.InputStream;
                        var CmnFilePath = objCmnFunctions.IEMAttachmentPath();
                        var path = Path.Combine(CmnFilePath, subfilename + subfileextension);
                        // var path = Path.Combine(@"C:\temp\", subfilename + subfileextension);
                        //using (var subfileStream = System.IO.File.Create(path))
                        //{
                        //    substream.CopyTo(subfileStream);
                        //}


                        byte[] bytFile = null;
                        using (var memoryStream = new MemoryStream())
                        {
                            substream.CopyTo(memoryStream);
                            bytFile = memoryStream.ToArray();
                            ViewBag.Result = "inside File Stream";
                        }
                        var FileString = Convert.ToBase64String(bytFile);

                        subfilename = subfilename + subfileextension;
                        var result = Cmnfs.SaveFileToServer(FileString, subfilename).Result;
                        Session["subfilename"] = subfilename;
                        saisUploaded = true;
                        if (result == "Success")
                        {
                            samessage = "File uploaded successfully!";
                            samessage = subfilename;
                        }
                        else { samessage = "File Not uploaded!"; samessage = subfilename; }


                    }
                    catch (Exception ex)
                    {
                        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                        samessage = string.Format("File upload failed: {0}", ex.Message);
                    }
                }
            }
            return Json(new { isUploaded = saisUploaded, message = samessage }, "text/html");
        }

        [HttpGet]
        public PartialViewResult soaAttachgrid(int id, string viewfor)
        {
            SaleMakerModel smm = new SaleMakerModel();
            try
            {
                if (viewfor == "viewmode")
                {
                    ViewBag.viewfor = "view";
                }
                else
                {
                    ViewBag.viewfor = "edit";

                }
                if (id != 0)
                {
                    smm.soaGid = id;
                    smm.attach_list = Tfr.Getattachment(id.ToString(), Tfr.GetRef("SOA").ToString(), Tfr.GetAttachType().ToString());
                }
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
            return PartialView(smm);
        }

        [HttpPost]
        public JsonResult SASaveattach(SaleMakerModel samodel)
        {
            string result = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    result = ifr.saveSaattachment(samodel);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Deleteattachsub(int id)
        {
            string subresult = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    subresult = ifr.Deleteattachsub(id);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(subresult, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult vatper(SaleMakerModel VATcal)
        {
            string result = string.Empty;
            // IEnumerable<SaleMakerModel> stat;
            try
            {
                // obj = new IEnumerable<SaleMakerModel>();
                decimal chamt = Convert.ToDecimal(VATcal.soapayAmount.ToString());
                string vadid = Convert.ToString(VATcal.vatid);
                //Decimal vatper = Convert.ToDecimal(ifr.Vatcalculation(vadid).ToString());
                Decimal vatpercen = Convert.ToDecimal(ifr.Vatcalculation(vadid).ToString());
                result = Convert.ToString((chamt * 100) / (100 + Math.Round(vatpercen, 2)));
                //Math.Round(result)
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult vatup(SaleMakerModel vatup)
        {
            try
            {
                string id = vatup.soaGid.ToString();
                if (id != "" && id != "0")
                {
                    Session["Saleno"] = id;
                    ifr.subSApaymentdetailup(vatup);
                    //RedirectToAction("SASummary");
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json("success", JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public JsonResult saleautosaleno(string term, string status)
        {
            try
            {
                List<SaleMakerModel> autoloc = new List<SaleMakerModel>();
                autoloc = ifr.SAAutosaleno(term, status, Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                return Json(autoloc, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        public JsonResult RevokeSale(string soagid)
        {
            try
            {
                if (soagid != null || soagid != "")
                {
                    ifr.RevokeSaleDetail(soagid);
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Error", JsonRequestBehavior.AllowGet);
                }
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


        //************************************************************
        // GET: /SaleCheckerSummary/

        [HttpGet]
        public ActionResult SAChkSummary()
        {
            SaleMakerModel sarecords = new SaleMakerModel();
            try
            {
                string sasuccess = ifr.getTheUser("SOACHK");
                ViewBag.sucess = sasuccess;
                if (sasuccess == "success")
                {
                    sarecords.TModel = ifr.GetSADetailChecker("").ToList();
                    if (sarecords.TModel.Count == 0)
                    {
                        ViewBag.Message = "No records Found";
                    }
                    Session["saleexcel"] = sarecords.TModel;
                }
                else
                {
                    sarecords.TModel = ifr.GetSaleDetailSummary("").ToList();
                    if (sarecords.TModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    Session["saleexcel"] = sarecords.TModel;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
            return View(sarecords);
        }

        [HttpPost]
        public ActionResult SAChkSummary(string sacfilter, string sacfilter1, FormCollection collections, string command)
        {
            try
            {
                SaleMakerModel SACdetails = new SaleMakerModel();
                if (command == "SEARCH")
                {

                    SACdetails.TModel = ifr.GetSaleDetailSummary(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();

                    if (sacfilter != null)
                    {
                        ViewBag.chckfilter = sacfilter;
                        SACdetails.TModel = SACdetails.TModel.Where(x => sacfilter.ToUpper() == null || (x.soaSalenumber.Contains(sacfilter.ToUpper()))).ToList();
                    }
                    if (sacfilter1 != null)
                    {
                        ViewBag.chckfilter1 = sacfilter1;
                        SACdetails.TModel = SACdetails.TModel.Where(x => sacfilter1.ToUpper() == null || (x.soaSaledate.ToUpper().Contains(sacfilter1.ToUpper()))).ToList();
                    }
                    if (SACdetails.TModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                }
                if (command == "Clear")
                {
                    return RedirectToAction("SAChkSummary", "AssetSale");
                }
                //else if (command == "SALE")
                //{
                //    return RedirectToAction("SAChkSummary");
                //}
                return View(SACdetails);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
            return View();
        }

        [HttpPost]
        public JsonResult saleautosalenochk(string term)
        {
            try
            {
                List<SaleMakerModel> autoloc = new List<SaleMakerModel>();
                autoloc = ifr.SAAutosalenochk(term, Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();
                return Json(autoloc, JsonRequestBehavior.AllowGet);
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


        public string gstdetailsale(string pincode, string saleid, string hsndesc)
        {
            List<Salemakermodelgst> records = new List<Salemakermodelgst>();
            records = ifr.gstsalemakerdetails(pincode, saleid, hsndesc).ToList();
            string strTable = "";
            strTable = strTable + "<table class='tableSmall table-bordered table-hover table-responsive'> <thead> <tr><th>HSN Code</th><th>HSN  Description</th> <th> Tax Type <th> Tax Rate </th>";
            strTable = strTable + "<th>Taxable Amount</th><th>Tax  Amount</th></tr></thead><tbody>";


            foreach (var item in records)
            {
                strTable = strTable + "<tr>";
                strTable = strTable + "<td>" + item.hsn_code + "</td>";
                strTable = strTable + "<td>" + item.hsn_description + "</td>";
                strTable = strTable + "<td>" + item.taxsubtype + "</td>";
                strTable = strTable + "<td>" + item.taxrate + "</td>";
                strTable = strTable + "<td>" + item.amount + "</td>";
                strTable = strTable + "<td>" + item.taxamount + "</td>";
                strTable = strTable + "</tr>";
            }
            strTable = strTable + "</tbody></table>";
            return strTable;
        }

        [HttpPost]
        public JsonResult assetsalegstupdate(SaleMakerModel objgst)
        {
            try
            {
                //bharathidhasan kumar1
                string id = objgst.soaGid.ToString();
                if (id != "" && id != "0")
                {
                    Session["Saleno"] = id;
                    ifr.assetdetailupdate(objgst);
                    //RedirectToAction("SASummary");
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json("success", JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public PartialViewResult SearchCustomer(string listfor, string formname)
        {
            try
            {
                ViewBag.IsGStCharged = listfor;
                List<SearchCustomer> obj = new List<SearchCustomer>();
                SearchCustomer Getvaluesearchvalue = new SearchCustomer();

                if (listfor == "search")
                {
                    if (Session["SearchEmployeedata"] != null)
                    {
                        Getvaluesearchvalue = (SearchCustomer)Session["SerchViewbag"];
                        @ViewBag.EmployeeName = Getvaluesearchvalue.customer_code;
                        @ViewBag.EmployeeCode = Getvaluesearchvalue.customer_name;
                        obj = (List<SearchCustomer>)Session["SearchEmployeedata"];
                    }
                }
                else
                {
                    Session["SearchEmployeedata"] = null;
                    Session["SerchViewbag"] = null;
                    obj = ifr.GetCustomerList().ToList();
                }


                return PartialView(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult GetGstCustomerDetails(SaleMakerModel CustomerDetail)
        {
            string Data1 = "";
            DataTable dt = ifr.GetGstCustomerDetails(CustomerDetail);
            if (dt.Rows.Count > 0)
            {
                Data1 = JsonConvert.SerializeObject(dt);
            }

            return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateHsnDetail(SaleMakerModel saledetail)
        {
            try
            {

                ifr.UpdateSOAHsndetail(saledetail);
                return Json("Sucess", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public JsonResult getAutocompleteCode(string customer_code)
        {
            List<SearchCustomer> obj = new List<SearchCustomer>();
            SearchCustomer v = new SearchCustomer();
            obj = ifr.SelectCustomerSearch(customer_code, "").ToList();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult getAutocomplete(string customer_name)
        {
            List<SearchCustomer> obj = new List<SearchCustomer>();
            SearchCustomer v = new SearchCustomer();
            obj = ifr.SelectCustomerSearch("", customer_name).ToList();
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchCustomerdata(string customer_code = "", string customer_name = "")
        {
            List<SearchCustomer> recordsearch = new List<SearchCustomer>();
            SearchCustomer emp = new SearchCustomer();
            if (customer_code != "" || customer_name != "")
            {
                @ViewBag.CustomerCode = customer_code;
                @ViewBag.CustomerName = customer_name;
                emp.customer_code = customer_code;
                emp.customer_name = customer_name;
                Session["SerchViewbag"] = emp;
                recordsearch = ifr.SelectCustomerSearch(customer_code, customer_name).ToList();
                Session["SearchEmployeedata"] = recordsearch;
            }
            else
            {
                Session["SerchViewbag"] = null;
                Session["SerchViewbag"] = null;
                recordsearch = ifr.SelectCustomerSearch(customer_code, customer_name).ToList();
            }

            return Json(recordsearch, JsonRequestBehavior.AllowGet);
        }

        //uat

        // Dashboard link Inprocess
        [HttpGet]
        public ActionResult SaleDasboardInprocessSummary()
        {
            SaleMakerModel records = new SaleMakerModel();
            try
            {
                records.TModel = ifr.GetSoaDetailSummary(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();

                if (records.TModel.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }

                Session["saleexcel"] = records.TModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
            return View(records);
        }

        //Inprocess

        [HttpPost]
        public ActionResult SaleDasboardInprocessSummary(string filter, string filter1, FormCollection collections, string command, string status)
        {
            SaleMakerModel sadetails = new SaleMakerModel();
            if (command == "SEARCH")
            {

                sadetails.TModel = ifr.GetSoaDetailSummary(Convert.ToString(objCmnFunctions.GetLoginUserGid())).ToList();

                if (filter != null)
                {
                    ViewBag.filter = filter;
                    sadetails.TModel = sadetails.TModel.Where(x => filter.ToUpper() == null || (x.soaSalenumber.Contains(filter.ToUpper()))).ToList();
                }
                if (filter1 != null)
                {
                    ViewBag.filter1 = filter1;
                    sadetails.TModel = sadetails.TModel.Where(x => filter1 == null || (x.soaSaledate.Contains(filter1.ToUpper()))).ToList();
                }

                if (sadetails.TModel.Count == 0)
                {
                    ViewBag.Message = "NO records found";
                }
                Session["saleexcel"] = sadetails.TModel;
                return View(sadetails);
            }

            if (command == "Clear")
            {
                return RedirectToAction("SaleDasboardInprocessSummary", "AssetSale");
            }
            else if (command == "SALE")
            {
                return RedirectToAction("SaleDasboardInprocessSummary");
            }


            return View();
        }



    }

}

