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
using System.Web.UI.WebControls;
using System.Web.UI;
using ClosedXML.Excel;
using IEM.App_Start;

namespace IEM.Areas.IFAMS.Controllers
{

    [NoDirectAccess]
    public class CWIPinwardController : Controller
    {

        //
        // GET: /IFAMS/CWIPinward/
        private IRepository ifr;
        private CmnFunctions objCmnFunctions = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        public CWIPinwardController()
            : this(new DataModel()) { }

        public CWIPinwardController(IRepository objModel)
        {
            ifr = objModel;
        }

        public ActionResult CwipMaker()
        {
            try
            {
                CWIPModel records = new CWIPModel();
                Session["CWIPErrdatatable"] = null;
                Session["CWIPmaindatatable"] = null;
                Session["CWIPRaiserAttmtFileExcel"] = null;
                Session["CWIPorginal"] = null;
                Session["CWIPerr"] = null;
                Session["CWIPTempdataset"] = null;
                Session["CWIPfilename"] = null;
                Session["CWIPTempdatatable"] = null;
                records.Model = ifr.GetCWIPMakerList().ToList();
                ViewBag.Status = "WAITING FOR APPROVAL";
                if (records.Model.Count == 0) { ViewBag.Message = "No records found"; }
                Session["CWIPexceldownload"] = records.Model;
                ifr.CancelDraftCWIP();
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

        private void datasupload(DataTable dataTable)
        {
            int sno = 0;
            int totalrecord = 0;
            int invalid = 0;
            int valid = 0;
            string duplicateasset = "";
            string grpId1 = "";
            string grpId2 = "";
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
                    string exceltext = "";
                    string assetdata = "";
                    string status = "";
                    string errs = "";
                    int RowNo = 0;
                    string assetcodes = "0";

                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        errs = "";
                        RowNo = i + 1;
                        assetdata = dataTable.Rows[i][1].ToString();
                        if (duplicateasset.Contains(assetdata) == true)
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
                                exceltext = dataTable.Rows[i][1].ToString();
                                status = ifr.GetStatusexcel(assetdata, exceltext, "CWIPAssetIdCheck");
                                if (status == "Free")
                                {
                                    status = ifr.GetStatusexcel(assetdata, exceltext, "CWIPAssetIdProcessCheck");
                                    if (status == "Free")
                                    {
                                        duplicateasset += duplicateasset == "" ? assetdata : ("," + assetdata);
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
                        }
                        if (dataTable.Rows[i][2].ToString() == "")
                        {
                            if (errs == "")
                            {
                                errs = "Asset code Should Not be Empty.";
                            }
                            else
                            {
                                errs = errs + "," + "Asset code Should Not be Empty.";
                            }
                        }
                        else
                        {
                            exceltext = dataTable.Rows[i][2].ToString();
                            status = ifr.GetStatusexcel(assetdata, exceltext, "CWIPAssetCode");
                            if (status == "notexists")
                            {
                                if (errs == "")
                                {
                                    errs = "Asset code not maintained.";
                                }
                                else
                                {
                                    errs = errs + "," + "Asset code not maintained .";
                                }
                            }
                        }
                        if (dataTable.Rows[i][3].ToString() == "")
                        {
                            if (errs == "")
                            {
                                errs = "Branch Should Not be Empty.";
                            }
                            else
                            {
                                errs = errs + "," + "Branch Should Not be Empty.";
                            }
                        }
                        else
                        {
                            exceltext = dataTable.Rows[i][3].ToString();
                            assetcodes = dataTable.Rows[i][2].ToString();
                           // status = ifr.GetStatusexcel(assetdata, exceltext, "CWIPAssetLoc");
                            status = ifr.GetStatusexcel(assetcodes, exceltext, "CWIPAssetLoc"); //Muthu 16-09-2016
                            if (status == "notexists")
                            {
                                if (errs == "")
                                {
                                    errs = "Branch not maintained.";
                                }
                                else
                                {
                                    errs = errs + "," + "Branch not maintained.";
                                }
                            }
                            else if (status == "branch_start_date")
                            {
                                if (errs == "")
                                {
                                    errs = "Branch start date not maintained.";
                                }
                                else
                                {
                                    errs = errs + "," + "Branch start date not maintained.";
                                }
                            }
                            else if (status == "branch_lease_end_date")
                            {
                                if (errs == "")
                                {
                                    errs = "Branch lease end dates not maintained.";
                                }
                                else
                                {
                                    errs = errs + "," + "Branch lease end dates not maintained.";
                                }
                            }
                            else if (status == "branch_lease_start_date")
                            {
                                if (errs == "")
                                {
                                    errs = "Branch lease start dates not maintained.";
                                }
                                else
                                {
                                    errs = errs + "," + "Branch lease start dates not maintained.";
                                }
                            }
                        }
                        //if (dataTable.Rows[i][4].ToString() == "")
                        //{
                        //    if (errs == "")
                        //    {
                        //        errs = "BS Should Not be Empty.";
                        //    }
                        //    else
                        //    {
                        //        errs = errs + "," + "BS Should Not be Empty.";
                        //    }
                        //}
                        //else
                        //{
                        //    exceltext = dataTable.Rows[i][4].ToString();
                        //    status = ifr.GetStatusexcel(assetdata, exceltext, "CWIPBS");
                        //    if (status == "notexists")
                        //    {
                        //        if (errs == "")
                        //        {
                        //            errs = "BS is invalid.";
                        //        }
                        //        else
                        //        {
                        //            errs = errs + "," + "BS is invalid.";
                        //        }
                        //    }
                        //}
                        if (dataTable.Rows[i][4].ToString() == "")
                        {
                            if (errs == "")
                            {
                                errs = "CC Should Not be Empty.";
                            }
                            else
                            {
                                errs = errs + "," + "CC Should Not be Empty.";
                            }
                        }
                        else
                        {
                            exceltext = dataTable.Rows[i][4].ToString();
                            status = ifr.GetStatusexcel(assetdata, exceltext, "CWIPCC");
                            if (status == "notexists")
                            {
                                if (errs == "")
                                {
                                    errs = "CC not maintained.";
                                }
                                else
                                {
                                    errs = errs + "," + "CC not maintained.";
                                }
                            }
                        }

                        string assetcode = dataTable.Rows[i][2].ToString();
                        exceltext = dataTable.Rows[i][8].ToString();
                        string exceltext2 = dataTable.Rows[i][7].ToString();
                        status = ifr.GetStatusexcel(assetcode, assetcode, "CWIPSerialNo");
                        if (status == "Exists")
                        {
                            if (exceltext == "" && exceltext2 == "")
                            {
                                if (errs == "")
                                {
                                    errs = "Serial no and Manufacturer Name is Mandatory.";
                                }
                                else
                                {
                                    errs = errs + "," + "Serial no and Manufacturer Name is Mandatory.";
                                }
                            }
                            else if (exceltext2 == "")
                            {
                                if (errs == "")
                                {
                                    errs = "Manufacturer Name is Mandatory.";
                                }
                                else
                                {
                                    errs = errs + "," + "Manufacturer Name is Mandatory.";
                                }
                            }
                            else if (exceltext == "")
                            {
                                if (errs == "")
                                {
                                    errs = "Serial no  is Mandatory.";
                                }
                                else
                                {
                                    errs = errs + "," + "Serial no is Mandatory.";
                                }
                            }

                        }
                        string format = "dd-mm-yyyy";
                        DateTime dateTime;

                        if (dataTable.Rows[i][6].ToString() == "")
                        {
                            if (errs == "")
                            {
                                errs = "Date of Capitalization should not be Empty.";
                            }
                            else
                            {
                                errs = errs + "," + "Date of Capitalization should not be Empty.";
                            }
                        }
                        else if (DateTime.TryParse(dataTable.Rows[i][6].ToString(), out dateTime))
                        {

                        }
                        else
                        {
                            if (errs == "")
                            {
                                errs = "Invalid date format.";
                            }
                            else
                            {
                                errs = errs + "," + "Invalid date format.";
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
                    Errdatatable.Rows.Add(1, "Please select a valid sheet");
                }
                duplicateasset = "";
                invalid = Errdatatable.Rows.Count;
                valid = totalrecord - invalid;
                ViewBag.vbvalid = "Total No of Vaild Record(s) :" + valid;
                ViewBag.vbinvalid = "Total No of InVaild Record(s) :" + invalid;
                ViewBag.vbtotalrecord = "Total No of Record(s) in Excel File :" + totalrecord;
                Session["CWIPErrdatatable"] = Errdatatable;
                Session["CWIPmaindatatable"] = maindatatable;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            finally
            {
            }
        }

        public void UploadFilesnew()
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
                        Session["CWIPRaiserAttmtFileExcel"] = hpf;
                        Session["CWIPorginal"] = hpf.FileName;
                    }
                }
                //return Json(filename);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                //return Json("Upload failed");
            }
            finally
            {
            }
        }

        [HttpPost]
        public virtual ActionResult UploadedFiles()
        {

            string filename = "";
            bool isUploaded = false;
            string message = "File upload failed";
            foreach (string file in Request.Files)
            {
                HttpPostedFileBase myFile = Request.Files["_upld_fname"];
                if (myFile != null && myFile.ContentLength != 0)
                {
                    string pathForSaving = HttpContext.Application["path"] as string; ;

                    try
                    {
                        filename = Path.GetFileNameWithoutExtension(myFile.FileName);
                        filename = objCmnFunctions.GetSequenceNoFam("TOAA");
                        var fileextension = Path.GetExtension(myFile.FileName);
                        var stream = myFile.InputStream;
                        var CmnFilePath = objCmnFunctions.IEMAttachmentPath();
                        var path = Path.Combine(CmnFilePath, filename + fileextension);
                        //  var path = Path.Combine(@"C:\temp\", filename + fileextension);
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                        filename = filename + fileextension;
                        Session["CWIPfilename"] = filename;
                        isUploaded = true;
                        message = "File uploaded successfully!";
                        message = filename;
                    }

                    catch (Exception ex)
                    {
                        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                        message = string.Format("File upload failed: {0}", ex.Message);
                    }
                }
            }
            return Json(new { isUploaded = isUploaded, message = message }, "text/html");
        }
        [HttpPost]
        public async Task<JsonResult> UploadFiles()
        {
            string Extension1 = "";
            string error = "";
            try
            {
                string path1 = "";
                if (Session["CWIPRaiserAttmtFileExcel"] != null)
                {
                    HttpPostedFileBase savefile = (HttpPostedFileBase)Session["CWIPRaiserAttmtFileExcel"];
                    string Extension = Path.GetFileName(savefile.FileName);
                    string n = string.Format("TOA{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                    Extension1 = System.IO.Path.GetExtension(savefile.FileName);
                    Session["CWIPfilename"] = n + " _ " + Extension;
                    var CmnFilePath = objCmnFunctions.IEMAttachmentPath();
                    path1 = CmnFilePath + n + " _ " + Extension;
                    //   path1 = @"C:\temp\" + n + " _ " + Extension;
                    if (System.IO.File.Exists(path1))
                    {
                        System.IO.File.Delete(path1);
                    }
                    savefile.SaveAs(path1);
                }
                List<SheetData> objparent = new List<SheetData>();
                SheetData objModel;
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
                    objModel = new SheetData();
                    objModel.SheetId = count;
                    objModel.SheetName = error.ToString();
                    objparent.Add(objModel);
                }


                foreach (DataTable dt in result1.Tables)
                {
                    sheets = dt.TableName.ToString().Trim();
                    count++;
                    objModel = new SheetData();
                    objModel.SheetId = count;
                    objModel.SheetName = sheets.ToString();
                    objparent.Add(objModel);
                }

                Session["CWIPTempdataset"] = result1;
                return Json(objparent.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }
        }

        public ActionResult downloadsexcel()
        {
            try
            {
                Session["CWIPerr"] = "";
                DataTable dtnew = (DataTable)Session["CWIPErrdatatable"];
                DataTable dtmainnew = (DataTable)Session["CWIPmaindatatable"];
                dtmainnew.Columns.Add("ERROR_DESCRIPTION");
                for (int i = 1; i <= dtmainnew.Rows.Count; i++)
                    for (int j = 0; j < dtnew.Rows.Count; j++)
                    {
                        int line = Convert.ToInt32(dtnew.Rows[j]["Line"]);
                        if (i == line)
                            dtmainnew.Rows[line - 1]["ERROR_DESCRIPTION"] = dtnew.Rows[j]["Error Description"];
                    }
                DataTable dt = dtmainnew;
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "CWIP_TEMPLATE");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=CWIP_ErrorLog.xlsx");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return View();
        }

        public PartialViewResult Uploadstatus(string ddlSelectsheet)
        {
            try
            {
                if (Session["CWIPTempdatatable"] != null)
                {
                    DataTable errdataval = new DataTable();
                    errdataval = (DataTable)Session["CWIPTempdatatable"];
                    datasupload(errdataval);
                    errdataval = (DataTable)Session["CWIPErrdatatable"];
                    if (errdataval.Rows.Count == 0)
                    {
                        ViewBag.visble = "Yes";
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return PartialView(string.Empty);
        }

        public ActionResult samdownloadsexcel()
        {
            try
            {
                using (DataTable dtnew = new DataTable())
                {
                    dtnew.Columns.Add("SNo");
                    dtnew.Columns.Add("WAREHOUSE_ASSET_ID");
                    dtnew.Columns.Add("ASSET_CODE");
                    dtnew.Columns.Add("BRANCH_CODE");                   
                    dtnew.Columns.Add("CC");
                    dtnew.Columns.Add("DESCRIPTION");
                    dtnew.Columns.Add("CAPITALISATION_DATE(dd-mm-yyyy)");
                    dtnew.Columns.Add("MANUFACTURER_NAME");
                    dtnew.Columns.Add("SERIAL_NUMBER");
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dtnew, "CWIP_TEMPLATE");

                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=CWIP_TEMPLATE.xlsx");
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return View();
        }

        public JsonResult Makerupdate(TransferMakerModel obj)
        {
            string mag = "";
            string strCols = "";
            string[] strColArr;
            string Maker = string.Empty;
            Maker = objCmnFunctions.authorize("437");
            try
            {
                if (Maker == "Success")
                {
                    DataSet result1 = new DataSet();
                    result1 = (DataSet)Session["CWIPTempdataset"];
                    var dataTable = new DataTable();
                    dataTable = result1.Tables[obj.SheetName.ToString()];
                    foreach (DataColumn dtColumn in dataTable.Columns)
                    {
                        strCols = strCols + dtColumn.ColumnName.Trim() + ":";
                    }
                    strCols = strCols.Substring(0, strCols.Length - 1);
                    strColArr = strCols.Split(':');
                    if (strColArr[0].ToString() == "SNo"
                        && strColArr[1].ToString() == "WAREHOUSE_ASSET_ID"
                        && strColArr[2].ToString() == "ASSET_CODE"
                        && strColArr[3].ToString() == "BRANCH_CODE"
                        && strColArr[4].ToString() == "CC"
                        && strColArr[5].ToString() == "DESCRIPTION"
                        && strColArr[6].ToString() == "CAPITALISATION_DATE(dd-mm-yyyy)"
                         && strColArr[7].ToString() == "MANUFACTURER_NAME"
                          && strColArr[8].ToString() == "SERIAL_NUMBER"
                        && strColArr.Count().ToString() == "9"
                        )
                    {
                        mag = "Success";
                    }
                    else
                    {
                        mag = "Faild";
                    }
                    if (mag == "Success")
                    {
                        int count = dataTable.Rows.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (dataTable.Rows[i]["SNo"].ToString() == ""
                                && dataTable.Rows[i]["WAREHOUSE_ASSET_ID"].ToString() == ""
                                && dataTable.Rows[i]["ASSET_CODE"].ToString() == ""
                                && dataTable.Rows[i]["BRANCH_CODE"].ToString() == ""
                                && dataTable.Rows[i]["BS"].ToString() == ""
                                && dataTable.Rows[i]["CC"].ToString() == ""
                                && dataTable.Rows[i]["DESCRIPTION"].ToString() == ""
                                && dataTable.Rows[i]["CAPITALISATION_DATE(dd-mm-yyyy)"].ToString() == ""
                                && dataTable.Rows[i]["MANUFACTURER_NAME"].ToString() == ""
                                && dataTable.Rows[i]["SERIAL_NUMBER"].ToString() == "")
                            {
                                dataTable.Rows[i].Delete();
                                dataTable.AcceptChanges();
                                count--;
                                i--;
                            }
                        }
                        Session["CWIPTempdatatable"] = dataTable;
                    }
                }
                else
                {
                    mag = "Unauthorized User!";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(mag, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult localdetails(string ddlSelectsheet)
        {
            try { Session["CWIPerr"] = "flag"; }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return PartialView();
        }

        public JsonResult localdetails()
        {
            try
            {
                Session["CWIPerr"] = "flag";
                DataTable uploadedData = new DataTable();
                uploadedData = (DataTable)Session["CWIPmaindatatable"];
                string check;
                string filename = Session["CWIPfilename"].ToString();
                check = ifr.SubmitDraftCWIP(uploadedData).ToString();
                return Json(check, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult BulkCWIP()
        {
            try
            {
                Session["CWIPErrdatatable"] = null;
                Session["CWIPmaindatatable"] = null;
                Session["CWIPRaiserAttmtFileExcel"] = null;
                Session["CWIPorginal"] = null;
                Session["CWIPerr"] = null;
                Session["CWIPTempdataset"] = null;
                Session["CWIPfilename"] = null;
                Session["CWIPTempdatatable"] = null;
                ifr.CancelDraftCWIP();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
            return PartialView();
        }

        [HttpPost]
        public ActionResult CwipMaker(string status, string filter, string filter1, FormCollection collections, string command)
        {
            try
            {
                CWIPModel details = new CWIPModel();
                details.Model = ifr.GetCWIPMakerList().ToList();
                if (command == "SEARCH")
                {
                    details.Model = ifr.GetCWIPMakerList().ToList();
                    if ((string.IsNullOrEmpty(filter) || string.IsNullOrWhiteSpace(filter)) && (string.IsNullOrEmpty(filter1) || string.IsNullOrWhiteSpace(filter1)) && (status == "--Select Status--"))
                    {
                        details.Model = ifr.GetCWIPMakerList().ToList();
                    }
                    if (filter != null)
                    {
                        ViewBag.filter = filter;
                        details.Model = details.Model.Where(x => filter.ToUpper() == null || (x.cwip_asset_id.Contains(filter.ToUpper()))).ToList();
                    }
                    if (filter1 != null)
                    {
                        ViewBag.filter1 = filter1;
                        details.Model = details.Model.Where(x => filter1 == null || (x.cwip_group_id.ToUpper().Contains(filter1.ToUpper()))).ToList();
                    }
                    if (status != "--Select Status--")
                    {
                        ViewBag.Status = status;
                        details.Model = details.Model.Where(x => status == null || (x.cwip_status.ToUpper().Contains(status.ToUpper()))).ToList();
                    }
                    if (details.Model.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    Session["CWIPexceldownload"] = details.Model;
                    return View(details);
                }
                if (command == "Clear")
                { return RedirectToAction("CwipMaker", "CWIPinward"); }

                return View();
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

        public JsonResult Status()
        {
            PhysicalVerificationAsset Verify = new PhysicalVerificationAsset();
            Ifams_Modelx Model = new Ifams_Modelx();
            IFAMS.Models.Ifams_Propertyx.ImpairmentsModel details = new IFAMS.Models.Ifams_Propertyx.ImpairmentsModel();

            try
            {
                return Json(Model.GetImpairStatus(), JsonRequestBehavior.AllowGet);
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
        public ActionResult SubmitCWIP()
        {
            CWIPModel list = new CWIPModel();
            try { list.Model = ifr.GetViewDetailsList().ToList(); }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
            } return View(list);
        }

        [HttpPost]
        public ActionResult SubmitCWIP(string ids = null, string ids2 = null, string command = null, string Asset = null)
        {
            string check = string.Empty;
            try
            {
                CWIPModel details = new CWIPModel();
                if (command == "SEARCH")
                {
                    details.Model = ifr.GetViewDetailsList().ToList();
                    if ((string.IsNullOrEmpty(Asset) || string.IsNullOrWhiteSpace(Asset)))
                    {
                        details.Model = ifr.GetViewDetailsList().ToList();
                    }
                    if (!string.IsNullOrEmpty(Asset))
                    {
                        ViewBag.filter = Asset;
                        details.Model = details.Model.Where(x => Asset.ToUpper() == null || (x.cwip_asset_id.Contains(Asset.ToUpper()))).ToList();
                    }
                    if (details.Model.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }

                    return View(details);
                }
                else
                {
                    if (ids != null)
                    {
                        string[] ids1 = ids.Split(',');
                        check = ifr.Verify_details(ids1);
                        if (check == "SUCCESS")
                        {
                            check = ifr.SubmitCapCWIP(ids).ToString();
                            check = ifr.UnseletedCWIP(ids2).ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
            }
            return Json(check, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult cancel()
        {
            string check = string.Empty;
            try
            { check = ifr.CancelDraftCWIP().ToString(); }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
            }
            return Json(check, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult Edit_View(int cwip_gid, string viewfor)
        {
            CWIPModel obj = new CWIPModel();
            try
            {
                if (viewfor == "edit")
                {
                    ViewBag.viewfor = "edit";
                }
                else if (viewfor == "view")
                {
                    ViewBag.viewfor = "view";
                }
                obj = ifr.GetCWIPViewDetails(cwip_gid);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
            }
            return PartialView(obj);
        }

        [HttpPost]
        public ActionResult Edit_View(CWIPModel obj)
        {
            string result = string.Empty;
            try
            {
                if (obj != null)
                {
                    result = ifr.SetCWIPDetails(obj);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CWIPChecker()
        {
            CWIPModel list = new CWIPModel();
            try
            {
                list.Model = ifr.GetCWIPCheckerList().ToList();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
            }
            return View(list);
        }

        [HttpPost]
        public ActionResult CWIPChecker(string ids = null, string status = null, string command = null, string Asset = null)
        {
            string check = string.Empty;
            string Checker = string.Empty;
            try
            {
                CWIPModel details = new CWIPModel();
                if (command == "SEARCH")
                {
                    details.Model = ifr.GetCWIPCheckerList().ToList();
                    if ((string.IsNullOrEmpty(Asset) || string.IsNullOrWhiteSpace(Asset)))
                    {
                        details.Model = ifr.GetCWIPCheckerList().ToList();
                    }
                    if (!string.IsNullOrEmpty(Asset))
                    {
                        ViewBag.filter = Asset;
                        details.Model = details.Model.Where(x => Asset.ToUpper() == null || (x.cwip_asset_id.Contains(Asset.ToUpper()))).ToList();
                    }
                    if (details.Model.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    return View(details);
                }
                else
                {
                    Checker = objCmnFunctions.authorize("438");
                    if (ids != null && Checker == "Success")
                    {
                        check = ifr.AppRejCapCWIP(ids, status).ToString();
                    }
                    else
                    {
                        check = "Unauthorized User!";
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
            }
            return Json(check, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CWIPexceldownload()
        {
            string mt = null;
            List<CWIPModel> cList;
            cList = (List<CWIPModel>)Session["CWIPexceldownload"];


            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("Asset id");
            dt.Columns.Add("Inward Date");
            dt.Columns.Add("Group Id");
            dt.Columns.Add("Capitalization Date");
            dt.Columns.Add("Status");
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                    i + 1
                    , cList[i].cwip_asset_id.ToString()
                    , cList[i].cwip_inward_date.ToString()
                    , cList[i].cwip_group_id.ToString()
                    , cList[i].cwip_capitalisation_date.ToString()
                    , cList[i].cwip_status.ToString());
            }

            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            // Session["exceldownload"] = gv;
            if (gv.Rows.Count != 0)
            {
                //return new DownloadFileActionResult((GridView)Session["exceldownload"], "DocumentGroup.xls");
                return new DownloadFileActionResult(gv, "CWIPSummary.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult CWIPreport()
        {
            CWIPModel list = new CWIPModel();
            try
            {
                list.Model = ifr.GetReportDetails(list).ToList(); 
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
            }
            return View(list);
        }

        [HttpPost]
        public ActionResult CWIPreport(string AssetIds = null, string inwardDate = null, string command = null, string CbfNo = null, string PoNo = null, string Code = null)
        {
            string check = string.Empty;
            try
            {
                CWIPModel details = new CWIPModel();
                if (command == "SEARCH")
                {
                    details.cwip_asset_id = AssetIds;
                    details.cwip_inward_date = inwardDate;
                    details.cwip_cbf_number = CbfNo;
                    details.cwip_po_number = PoNo;
                    details.cwip_asset_code = Code;

                    ViewBag.AssetIds = AssetIds;
                    ViewBag.inwardDate = inwardDate;
                    ViewBag.CbfNo = CbfNo;
                    ViewBag.PoNo = PoNo;
                    ViewBag.Code = Code;

                    details.Model = ifr.GetReportDetails(details).ToList();
                   
                    if (details.Model.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    return View(details);
                }
                else if (command == "Clear") { details.Model = ifr.GetReportDetails(details).ToList(); }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
            }
            return Json(check, JsonRequestBehavior.AllowGet);
        }
       
        public void DownloadExcel()
        {

            DataTable _downloadingData = new DataTable("dtname"); _downloadingData=(DataTable)Session["reportSession"];
            _downloadingData.TableName="dtname";
            if (_downloadingData != null)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_downloadingData);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=CWIP_REPORT.xls");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }

          
        }
        [HttpGet]
        public ActionResult CapCWIPreport()
        {
            CWIPModel list = new CWIPModel();
            try
            {
                list.Model = ifr.GetCapReportDetails(list).ToList();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
            }
            return View(list);
        }

        [HttpPost]
        public ActionResult CapCWIPreport(string AssetIds = null, string inwardDate = null, string command = null, string CbfNo = null, string PoNo = null, string Code = null)
        {
            string check = string.Empty;
            try
            {
                CWIPModel details = new CWIPModel();
                if (command == "SEARCH")
                {
                    details.cwip_asset_id = AssetIds;
                    details.cwip_inward_date = inwardDate;
                    details.cwip_cbf_number = CbfNo;
                    details.cwip_po_number = PoNo;
                    details.cwip_asset_code = Code;

                    ViewBag.AssetIds = AssetIds;
                    ViewBag.inwardDate = inwardDate;
                    ViewBag.CbfNo = CbfNo;
                    ViewBag.PoNo = PoNo;
                    ViewBag.Code = Code;

                    details.Model = ifr.GetCapReportDetails(details).ToList();

                    if (details.Model.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    return View(details);
                }
                else if (command == "Clear") { details.Model = ifr.GetCapReportDetails(details).ToList(); }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
            }
            return Json(check, JsonRequestBehavior.AllowGet);
        }

        public void DownloadExcelCapCWIPreport()
        {

            DataTable _downloadingData = new DataTable("dtname"); _downloadingData = (DataTable)Session["reportSessionCapCWIPreport"];
            _downloadingData.TableName = "dtname";
            if (_downloadingData != null)
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(_downloadingData);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("content-disposition", "attachment;filename=Capitalized_CWIP_REPORT.xls");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                }
            }


        }

    }
}
