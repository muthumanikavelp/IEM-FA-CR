//Version 1.0 23/07/2015
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
using System.Web.UI.WebControls;
using System.Web.UI;
using ClosedXML.Excel;
using IEM.Areas.IFAMS.Models;
using IEM.Common;
using IEM.App_Start;

namespace IEM.Areas.IFAMS.Controllers
{

    [NoDirectAccess]
    public class AssetWriteOffController : Controller
    {
        //
        // GET: /IFAMS/AssetWriteOff/
        private IRepository ifr;
        private CmnFunctions objCmnFunc = new CmnFunctions();
        private FileServer Cmnfs = new FileServer();
        ErrorLog objErrorLog = new ErrorLog();
        public AssetWriteOffController() :
            this(new DataModel())
        {

        }
        public AssetWriteOffController(IRepository objModel)
        {
            ifr = objModel;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult WMSummary()
        {
            Session["WriteoffNo"] = null;
            Session["WrtStatus"] = null;
            Session["woaErrdatatable"] = null;
            Session["woamaindatatable"] = null;
            Session["woaRAISERattmtfileexcel"] = null;
            Session["orginalfilename"] = null;
            Session["woaerr"] = null;
            Session["woaTempdataset"] = null;
            Session["woafilename"] = null;
            Session["woaTempdatatable"] = null;
            Session["woagid"] = null;
            WriteOffModel records = new WriteOffModel();
            try
            {
                string status = Request.QueryString["reqstatus"];
                ViewBag.Status = "DRAFT";
                ViewBag.stsrejected = "REJECTED";
                if (status == ViewBag.Status)
                {
                    records.WModel = ifr.GetWoaDetailDraft(objCmnFunc.GetLoginUserGid()).ToList();
                    if (records.WModel.Count == 0) { ViewBag.Message = "No records found"; }
                    Session["WriteExceldownload"] = records.WModel;
                }
                else if (status == ViewBag.stsrejected)
                {
                    records.WModel = ifr.GetWoaDetailReject(objCmnFunc.GetLoginUserGid()).ToList();
                    if (records.WModel.Count == 0) { ViewBag.Message = "No records found"; }
                    Session["WriteExceldownload"] = records.WModel;
                }
                else
                {
                    records.WModel = ifr.GetWoaDetailDraft(objCmnFunc.GetLoginUserGid()).ToList();
                    if (records.WModel.Count == 0) { ViewBag.Message = "No records found"; }
                    Session["WriteExceldownload"] = records.WModel;
                }

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

        [HttpPost]
        public ActionResult WMSummary(string status, string filter, string DATEfilter, FormCollection collections, string command)
        {
            WriteOffModel details = new WriteOffModel();
            try
            {
                if (command != "SEARCH")
                    details.WModel = ifr.GetWoaDetailDraft(objCmnFunc.GetLoginUserGid()).ToList();
                if (command == "SEARCH")
                {

                    details.WModel = ifr.GetWoaDetail(objCmnFunc.GetLoginUserGid()).ToList();


                    if ((string.IsNullOrEmpty(filter) || string.IsNullOrWhiteSpace(filter)) && (string.IsNullOrEmpty(DATEfilter) || string.IsNullOrWhiteSpace(DATEfilter)) && (status == "--Select Status--"))
                    {
                        details.WModel = ifr.GetWoaDetailDraft(objCmnFunc.GetLoginUserGid()).ToList();
                    }
                    if (filter != null && filter != "")
                    {
                        ViewBag.filter = filter;
                        details.WModel = details.WModel.Where(x => filter.ToUpper() == null || (x._woa_number.ToUpper().Contains(filter.ToUpper()))).ToList();
                    }
                    if (DATEfilter != null)
                    {
                        ViewBag.filter1 = DATEfilter;
                        details.WModel = details.WModel.Where(x => DATEfilter == null || (x._woa_date.ToUpper().Contains(DATEfilter.ToUpper()))).ToList();
                    }

                    if (status != "--Select Status--" && status != "FORMYAPPROVAL")
                    {
                        ViewBag.Status = status;
                        details.WModel = details.WModel.Where(x => status == null || (x._woa_status.ToUpper().Equals(status.ToUpper()))).ToList();
                    }
                    //else if (status == "FORMYAPPROVAL")
                    //{
                    //    //selva
                    //    details.WModel = ifr.GetWrtDetailforChecker(Convert.ToString(objCmnFunc.GetLoginUserGid())).ToList();
                    //    ViewBag.Status = status;
                    //    details.WModel = details.WModel.Where(x => status == null || (x._woa_status.ToUpper().Equals(status.ToUpper()))).ToList();
                    //}
                    if (details.WModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    Session["WriteExceldownload"] = details.WModel;
                    return View(details);
                }
                if (command == "Clear")
                { return RedirectToAction("WMSummary", "AssetWriteOff"); }
                else if (command == "WRITEOFF")
                {
                    return RedirectToAction("WMSummary");
                }
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
        [HttpGet]
        public PartialViewResult BulkWriteoff()
        {
            try
            {
                Session["WriteoffNo"] = null;
                Session["WrtStatus"] = null;
                Session["woaErrdatatable"] = null;
                Session["woamaindatatable"] = null;
                Session["woaRAISERattmtfileexcel"] = null;
                Session["orginalfilename"] = null;
                Session["woaerr"] = null;
                Session["woaTempdataset"] = null;
                Session["woafilename"] = null;
                Session["woaTempdatatable"] = null;
                Session["woagid"] = null;
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
        public JsonResult ImpairStatus()
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

        private void datasupload(DataTable dataTable)
        {
            int sno = 0;
            int totalrecord = 0;
            int invalid = 0;
            int valid = 0;
            string grpId1 = "";
            string grpId2 = "";
            string duplicateasset = "";
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
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        errs = "";
                        RowNo = i + 1;
                        assetdata = dataTable.Rows[i][1].ToString();
                        if (duplicateasset.Contains(assetdata) == true)
                        {
                            errs = "Duplicate Asset id Found.";
                        }
                        else
                        {
                            if (dataTable.Rows[i][1].ToString() == "")
                            {
                                errs = "Asset Id must not be empty.";
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][1].ToString();
                                status = ifr.GetStatusexcel(assetdata, exceltext, "AssetIdCheck");
                                if (status == "Free")
                                {
                                    status = ifr.GetStatusexcel(assetdata, exceltext, "AssetIdProcessCheck");
                                    if (status == "Free")
                                    {
                                        duplicateasset += duplicateasset == "" ? assetdata : ("," + assetdata);
                                        grpId1 = ifr.GetGroupGid(dataTable.Rows[0][1].ToString());
                                        grpId2 = ifr.GetGroupGid(assetdata);
                                        if (grpId2 == grpId1 || grpId2 == "")
                                        {
                                            grpId1 = ifr.GetGrpCount(grpId1);
                                            if (grpId1 != "0")
                                                if (Convert.ToInt32(grpId1) != dataTable.Rows.Count)
                                                {
                                                    if (errs == "")
                                                    {
                                                        errs = "All assets in the Group are not Provided.";
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
                                                errs = " All Asset ID must be of Same Group.";
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
                        }
                        if (dataTable.Rows[i][2].ToString() == "")
                        {
                            if (errs == "")
                            {
                                errs = "Branch should not be Empty.";
                            }
                            else
                            {
                                errs = errs + "," + "Branch should not be Empty.";
                            }
                        }
                        else
                        {
                            exceltext = dataTable.Rows[i][2].ToString();
                            status = ifr.GetStatusexcel(assetdata, exceltext, "AssetLoc");
                            if (status == "notexists")
                            {
                                if (errs == "")
                                {
                                    errs = "Asset belongs to Diffrent Branch.";
                                }
                                else
                                {
                                    errs = errs + "," + "Asset belongs to Diffrent Branch.";
                                }
                            }
                        }


                        if (dataTable.Rows[i][3].ToString() == "")
                        {
                            if (errs == "")
                            {
                                errs = "CC should not be Empty.";
                            }
                            else
                            {
                                errs = errs + "," + "CC should not be Empty.";
                            }
                        }
                        else
                        {
                            exceltext = dataTable.Rows[i][3].ToString();

                            status = ifr.GetStatusexcel(assetdata, exceltext, "fccc");
                            if (status == "notexists")
                            {
                                if (errs == "")
                                {
                                    errs = "CC is invalid.";
                                }
                                else
                                {
                                    errs = errs + "," + "CC is invalid.";
                                }
                            }
                        }
                        if (dataTable.Rows[i][5].ToString() == "")
                        {
                            if (errs == "")
                            {
                                errs = "Inward No should not be Empty.";
                            }
                            else
                            {
                                errs = errs + "," + "Inward No should not be Empty.";
                            }
                        }
                        else
                        {
                            exceltext = dataTable.Rows[i][5].ToString();

                            status = ifr.GetStatusexcel(assetdata, exceltext, "InwardNo");
                            if (status == "notexists")
                            {
                                if (errs == "")
                                {
                                    errs = "Inward No is invalid.";
                                }
                                else
                                {
                                    errs = errs + "," + "Inward No is invalid.";
                                }
                            }
                        }

                        DateTime dateTime;
                        if (dataTable.Rows[i][4].ToString() == "")
                        {
                            if (errs == "")
                            {
                                errs = "Write-Off Date should not be Empty.";
                            }
                            else
                            {
                                errs = errs + "," + "Write-Off Date should not be Empty.";
                            }
                        }
                        else if (DateTime.TryParse(dataTable.Rows[i][4].ToString(),
                            // format,
                            //    System.Globalization.CultureInfo.InvariantCulture,
                            // System.Globalization.DateTimeStyles.None,
                                 out dateTime))
                        {
                            exceltext = dataTable.Rows[i][4].ToString();
                            DateTime date = Convert.ToDateTime(exceltext);
                            DateTime year = DateTime.Today;
                            string finyear = ifr.toGetFincialyear(date);
                            string fintodayyear = ifr.toGetFincialyear();
                            string[] arrYer = finyear.Split('-');
                            if (finyear != fintodayyear)
                            {
                                //if (date.Year == year.Year || date.Year == (year.Year - 1))

                                if (errs == "")
                                {
                                    errs = "Date does not belong to this financial year.";
                                }
                                else
                                {
                                    errs = errs + "," + "Date does not belong to this financial year.";
                                }
                            }
                        }
                        else
                        {
                            if (errs == "")
                            {
                                errs = "Date format is invalid.";
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

                invalid = Errdatatable.Rows.Count;
                valid = totalrecord - invalid;
                ViewBag.vbvalid = "Total No of Vaild Record(s) :" + valid;
                ViewBag.vbinvalid = "Total No of InVaild Record(s) :" + invalid;
                ViewBag.vbtotalrecord = "Total No of Record(s) in Excel File :" + totalrecord;
                Session["woaErrdatatable"] = Errdatatable;
                Session["woamaindatatable"] = maindatatable;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
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
                        Session["woaRAISERattmtfileexcel"] = hpf;
                        Session["orginalfilename"] = hpf.FileName;
                    }
                }
                //return Json(filename);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                //return Json("Upload failed");
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
                        filename = objCmnFunc.GetSequenceNoFam("WOAA");
                        var fileextension = Path.GetExtension(myFile.FileName);
                        var stream = myFile.InputStream;
                        var CmnFilePath = objCmnFunc.IEMAttachmentPath();
                        var path = Path.Combine(CmnFilePath, filename + fileextension);
                        // var path = Path.Combine(@"C:\temp\", filename + fileextension);
                        //using (var fileStream = System.IO.File.Create(path))
                        //{
                        //    stream.CopyTo(fileStream);
                        //}

                        byte[] bytFile = null;
                        using (var memoryStream = new MemoryStream())
                        {
                            stream.CopyTo(memoryStream);
                            bytFile = memoryStream.ToArray();
                            ViewBag.Result = "inside File Stream";
                        }
                        var FileString = Convert.ToBase64String(bytFile);


                        filename = filename + fileextension;
                        var result = Cmnfs.SaveFileToServer(FileString, filename).Result;
                        Session["woafilename"] = filename;
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
                if (Session["woaRAISERattmtfileexcel"] != null)
                {
                    HttpPostedFileBase savefile = (HttpPostedFileBase)Session["woaRAISERattmtfileexcel"];
                    string Extension = Path.GetFileName(savefile.FileName);
                    string n = string.Format("WOA-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                    Extension1 = System.IO.Path.GetExtension(savefile.FileName);
                    Session["woafilename"] = n + " _ " + Extension;
                    var CmnFilePath = objCmnFunc.IEMAttachmentPath();
                    path1 = CmnFilePath + n + " _ " + Extension;
                    //    path1 = @"C:\temp\" + n + " _ " + Extension;
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

                Session["woaTempdataset"] = result1;
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
                Session["woaerr"] = "";
                DataTable dtnew = (DataTable)Session["woaErrdatatable"];
                DataTable dtmainnew = (DataTable)Session["woamaindatatable"];
                dtmainnew.Columns.Add("Error Description");
                for (int i = 1; i <= dtmainnew.Rows.Count; i++)
                    for (int j = 0; j < dtnew.Rows.Count; j++)
                    {
                        int line = Convert.ToInt32(dtnew.Rows[j]["Line"]);
                        if (i == line)
                            dtmainnew.Rows[line - 1]["Error Description"] = dtnew.Rows[j]["Error Description"];
                    }
                DataTable dt = dtmainnew;
                string str = string.Empty;
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "WriteOff_Template");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=WriteOff_ErrorLog.xlsx");
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
                //Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "WriteOff_Errorlog.xls"));
                //Response.ContentType = "application/ms-excel";
                //DataTable dt = dtnew;
                //string str = string.Empty;
                //foreach (DataColumn dtcol in dt.Columns)
                //{
                //    Response.Write(str + dtcol.ColumnName);
                //    str = "\t";
                //}
                //Response.Write("\n");
                //foreach (DataRow dr in dt.Rows)
                //{
                //    str = "";
                //    for (int j = 0; j < dt.Columns.Count; j++)
                //    {
                //        Response.Write(str + Convert.ToString(dr[j]));
                //        str = "\t";
                //    }
                //    Response.Write("\n");
                //}
                //Response.End();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return View();
        }
        [HttpGet]
        public PartialViewResult Uploadstatus(string ddlSelectsheet)
        {
            try
            {
                if (Session["woaTempdatatable"] != null)
                {
                    DataTable errdataval = new DataTable();
                    errdataval = (DataTable)Session["woaTempdatatable"];
                    datasupload(errdataval);
                    errdataval = (DataTable)Session["woaErrdatatable"];
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
            return PartialView();
        }

        public ActionResult samdownloadsexcel()
        {
            using (DataTable dtnew = new DataTable())
            {
                dtnew.Columns.Add("SNo");
                dtnew.Columns.Add("Asset Id");
                dtnew.Columns.Add("Branch");
                dtnew.Columns.Add("CC");
                //dtnew.Columns.Add("Write-Off Date(dd-mm-yyyy)");
                dtnew.Columns.Add("Date(dd-mm-yyyy)");
                dtnew.Columns.Add("Inward No");
                dtnew.Columns.Add("Reason");
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtnew, "WriteOff_Template");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=WriteOff_Template.xlsx");
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

        //public ActionResult samdownloadsexcel()
        //{
        //    try
        //    {
        //        DataTable dtnew = new DataTable();
        //        dtnew.Columns.Add("SNo");
        //        dtnew.Columns.Add("Asset Id");
        //        dtnew.Columns.Add("Branch");
        //        dtnew.Columns.Add("CC");
        //        dtnew.Columns.Add("Write-Off Date(dd-mm-yyyy)"); 
        //        dtnew.Columns.Add("Inward No");
        //        dtnew.Columns.Add("Reason");
        //        Response.ClearContent();
        //        Response.Buffer = true;
        //        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "WriteOff_Template.xls"));
        //        Response.ContentType = "application/ms-excel";
        //        DataTable dt = dtnew;
        //        string str = string.Empty;
        //        foreach (DataColumn dtcol in dt.Columns)
        //        {
        //            Response.Write(str + dtcol.ColumnName);
        //            str = "\t";
        //        }
        //        Response.Write("\n");
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            str = "";
        //            for (int j = 0; j < dt.Columns.Count; j++)
        //            {
        //                Response.Write(str + Convert.ToString(dr[j]));
        //                str = "\t";
        //            }
        //            Response.Write("\n");
        //        }
        //        Response.End();
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //    }
        //    return View();
        //}


        public JsonResult Makerupdate(WriteOffModel obj)
        {
            string mag = "";
            string strCols = "";
            string[] strColArr;
            try
            {
                DataSet resultwrite = new DataSet();
                resultwrite = (DataSet)Session["woaTempdataset"];
                var dataTable = new DataTable();
                dataTable = resultwrite.Tables[obj.SheetName.ToString().Trim()];
                foreach (DataColumn dtColumn in dataTable.Columns)
                {
                    strCols = strCols + dtColumn.ColumnName.Trim() + ":";
                }
                strCols = strCols.Substring(0, strCols.Length - 1);
                strColArr = strCols.Split(':');
                if (strColArr[0].ToString() == "SNo"
                    && strColArr[1].ToString() == "Asset Id"
                    && strColArr[2].ToString() == "Branch"
                    && strColArr[3].ToString() == "CC"
                    //  && strColArr[4].ToString() == "Write-Off Date(dd-mm-yyyy)"
                   && strColArr[4].ToString() == "Date(dd-mm-yyyy)"
                    && strColArr[5].ToString() == "Inward No"
                    && strColArr[6].ToString() == "Reason"
                    && strColArr.Count().ToString() == "7")
                {
                    mag = "Success";
                }
                else
                {
                    mag = "Faild";
                }
                if (mag == "Success")
                {
                    if (dataTable.Rows.Count > 0)
                    {
                        int count = dataTable.Rows.Count;
                        for (int i = 0; i < count; i++)
                        {
                            if (dataTable.Rows[i]["SNo"].ToString() == ""
                                && dataTable.Rows[i]["Asset Id"].ToString() == ""
                                //&& dataTable.Rows[i]["Write-Off Date(dd-mm-yyyy)"].ToString() == ""
                                 && dataTable.Rows[i]["Date(dd-mm-yyyy)"].ToString() == ""
                                && dataTable.Rows[i]["Branch"].ToString() == ""
                                && dataTable.Rows[i]["CC"].ToString() == ""
                                 && dataTable.Rows[i]["Inward No"].ToString() == ""
                                && dataTable.Rows[i]["Reason"].ToString() == "")
                            {
                                dataTable.Rows[i].Delete();
                                dataTable.AcceptChanges();
                                count--;
                                i--;
                            }
                        }
                        Session["woaTempdatatable"] = dataTable;
                    }
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
            Session["woaerr"] = "flag";
            return PartialView();
        }

        //File upload button k click  woa
        public JsonResult localdetails()
        {
            string check = "";
            try
            {
                Session["woaerr"] = "flag";
                DataTable uploadedData = new DataTable();
                uploadedData = (DataTable)Session["woamaindatatable"];
                string assettype = "WOA";
                string filename = Session["woafilename"].ToString();
                check = ifr.ImportAssetDetails(uploadedData, filename, assettype).ToString();
                // check = ifr.ImportAssetDetails(uploadedData, filename).ToString();

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(check, JsonRequestBehavior.AllowGet);
        }
        public void ViewAttachment(string id)
        {
            string filename = ifr.getfilename(id);
            DownloadDocument(filename);
        }
        [HttpPost]
        public JsonResult CheckFileExists(string id)
        {
            string result = "0";
            try
            {
                //WebReference.Service1 ObjService = new WebReference.Service1();
                //var isExists = ObjService.CheckFileExists(id);
                //if (isExists == 1)
                //{
                //    result = "1";
                //}
                var CmnFilePath = objCmnFunc.IEMAttachmentPath();
                var localpath = Path.Combine(CmnFilePath, id);
                //var localpath = Path.Combine(@"C:\temp\", id);
                //if (System.IO.File.Exists(localpath))
                //{
                //    result = "1";
                //}
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
        public FileResult DownloadDocument(string id)
        {
            var path = "";
            string filename = "";
            try
            {
                filename = id;
                var CmnFilePath = objCmnFunc.IEMAttachmentPath();
                path = Path.Combine(CmnFilePath, filename);
                // path = Path.Combine(@"C:\Temp\", filename);
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
        // WriteoffMakerView
        [HttpPost]
        public JsonResult Save_attach(WriteOffModel model)
        {
            string result = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    result = ifr.save_attachment(model);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete_attach(int id)
        {
            string result = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    result = ifr.Delete_attach(id);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public PartialViewResult woaAttach(int id, string viewfor)
        {
            WriteOffModel wm = new WriteOffModel();
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
                    wm._gid = id;
                    wm._attach_list = ifr.Getattachment(id.ToString(), ifr.GetRef("WOA").ToString(), ifr.GetAttachType().ToString());
                }
                if (wm._attach_list.Count == 0)
                    ViewBag.Message = "No records found";

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
            return PartialView(wm);
        }


        [HttpGet]
        public PartialViewResult woaAttachgrid(int id, string viewfor)
        {
            WriteOffModel wm = new WriteOffModel();
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
                    wm._gid = id;
                    wm._attach_list = ifr.Getattachment(id.ToString(), ifr.GetRef("WOA").ToString(), ifr.GetAttachType().ToString());
                }
                if (wm._attach_list.Count == 0)
                    ViewBag.Message = "No records found";
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
            return PartialView(wm);
        }
        public PartialViewResult WriteoffMakerView(string id, string viewfor, FormCollection collections, WriteOffModel status)
        {
            try
            {

                string userGroup = string.Empty;
                userGroup = ifr.GetUserType(id);
                ViewBag.success = userGroup;


                string _woa_number = ifr.GetWrtNo(id);
                Session["WriteoffNo"] = _woa_number;
                Session["woagid"] = id;
                string stat = ifr.Getwrtststus(status, id);
                Session["WrtStatus"] = stat;
                WriteOffModel llist = new WriteOffModel();
                llist.WModel = ifr.GetWriteOffDetail(_woa_number).ToList();
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
                if (llist.WModel.Count == 0)
                {
                    ViewBag.Message = "No Records Found";
                }
                return PartialView(llist);
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


        // //Submit and  Approve Click 
        [HttpPost]
        public JsonResult WriteoffMakerView(WriteOffModel assetwriteoffDetails)
        {
            string Checker = string.Empty;
            string Maker = string.Empty;
            int Result = 0;
            string Msg = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                if (assetwriteoffDetails._woa_number != "")
                {
                    Session["WriteoffNo"] = assetwriteoffDetails._woa_number;
                    dt = ifr.updateWrtStatus(assetwriteoffDetails);
                    Msg = dt.Rows[0][0].ToString();
                    Result = Convert.ToInt32(dt.Rows[0][1].ToString());
                }
                return Json(new { assetwriteoffDetails.commandType, Msg, Result }, JsonRequestBehavior.AllowGet);
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


        //for query screen view
        public PartialViewResult QueryscreenWriteoffView(string id, string viewfor, FormCollection collections, WriteOffModel status)
        {
            try
            {

                string userGroup = string.Empty;
                userGroup = ifr.GetUserType(id);
                ViewBag.success = userGroup;

                string _woa_number = ifr.GetWrtNo(id);
                Session["WriteoffNo"] = _woa_number;
                Session["woagid"] = id;
                string stat = ifr.Getwrtststus(status, id);
                Session["WrtStatus"] = stat;
                WriteOffModel llist = new WriteOffModel();
                llist.WModel = ifr.GetWriteOffDetail(_woa_number).ToList();
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
                if (llist.WModel.Count == 0)
                {
                    ViewBag.Message = "No Records Found";
                }
                return PartialView(llist);
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




        //[HttpPost]
        //public JsonResult WriteoffMakerView(string command, string viewfor, string id, string remarks, Models.WriteOffModel status, FormCollection collections)
        //{
        //    string Result = string.Empty, Maker = string.Empty, Checker = string.Empty;
        //    Maker = objCmnFunc.authorize("279");
        //    Checker = objCmnFunc.authorize("280");
        //    try
        //    {
        //        if (Maker == "Success")
        //        {
        //            if (command == "abort")
        //            {
        //                // id = status._toa_number;
        //                ifr.AbortWriteoff(id);
        //                ViewBag.viewfor = "abort";

        //            }
        //            if (command == "submit")
        //            {
        //                if (id != "")
        //                {
        //                    Session["WriteoffNo"] = id;
        //                    ifr.updateWrtStatus(status, id);
        //                }
        //            }
        //        }

        //        if (Checker == "Success")
        //        {
        //            if (command == "Approve")
        //            {
        //                if (id != "")
        //                {
        //                    Session["WriteoffNo"] = id;
        //                    Result = ifr.AppRejStatus(id, "APPROVED", remarks);
        //                    command = Result;
        //                }

        //            }
        //            if (command == "Reject")
        //            {
        //                if (id != "")
        //                {
        //                    Session["WriteoffNo"] = id;
        //                    Result = ifr.AppRejStatus(id, "REJECTED", remarks);
        //                    command = Result;
        //                }
        //            }
        //        }

        //        if (command == "cancel")
        //        {

        //        }
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

        //Writeoff Dashboard Link For MyApproval

        [HttpGet]
        public ActionResult WCSummary()
        {
            WriteOffModel records = new WriteOffModel();
            try
            {
                string success = ifr.getTheUser("TOACHK");
                ViewBag.success = success;
                if (success == "success")
                {
                    records.WModel = ifr.GetWrtDetailforChecker("").ToList();

                    if (records.WModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }

                }
                else
                {
                    // records.WModel = ifr.GetWrtDetailforChecker("").ToList();
                    records.WModel = ifr.GetWrtDetailforSupervisior("").ToList(); // Supervisior login and get databind grid

                    if (records.WModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    Session["WriteExceldownload"] = records.WModel;
                }



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
        //WOA
        [HttpPost]
        public ActionResult WCSummary(string filter, string filter1, FormCollection collections, string command)
        {
            try
            {
                WriteOffModel details = new WriteOffModel();

                details.WModel = ifr.GetWrtDetailforSupervisior(Convert.ToString(objCmnFunc.GetLoginUserGid())).ToList();

                if (command == "SEARCH")
                {
                    // details.WModel = ifr.GetWrtDetailforChecker(Convert.ToString(objCmnFunc.GetLoginUserGid())).ToList();


                    if ((string.IsNullOrEmpty(filter) || string.IsNullOrWhiteSpace(filter)) && (string.IsNullOrEmpty(filter1) || string.IsNullOrWhiteSpace(filter1)))
                    {
                        details.WModel = ifr.GetWrtDetailforSupervisior(Convert.ToString(objCmnFunc.GetLoginUserGid())).ToList();
                    }
                    if (filter != null && filter != "")
                    {
                        ViewBag.checkerfilter = filter;
                        details.WModel = details.WModel.Where(x => filter.ToUpper() == null || (x._woa_number.Contains(filter.ToUpper()))).ToList();
                    }
                    if (filter1 != null && filter1 != "")
                    {
                        ViewBag.checkerfilter1 = filter1;
                        details.WModel = details.WModel.Where(x => filter1.ToUpper() == null || (x._woa_date.ToUpper().Equals(filter1.ToUpper()))).ToList();


                    }
                    if (details.WModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    Session["WriteExceldownload"] = details.WModel;
                    return View(details);
                }
                if (command == "Clear")
                { return RedirectToAction("WCSummary", "AssetWriteOff"); }
                else if (command == "WRITEOFF")
                {
                    return RedirectToAction("WCSummary");
                }
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
        public JsonResult verifyWriteoff(WriteOffModel objk)
        {
            List<WriteOffModel> obkl = new List<WriteOffModel>();
            string Result = string.Empty;
            try
            {

                obkl = ifr.verifyWoalogeddetails(objk._gid.ToString()).ToList();
                if (obkl.Count == 0)
                {
                    Result = ifr.Updatelogedstatus(objk._gid.ToString());
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(obkl, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult updatelogeddetials(WriteOffModel uobj)
        {
            string changelogstatus = string.Empty;
            try
            {
                changelogstatus = ifr.changlogedstatus(uobj._gid.ToString());
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(changelogstatus, JsonRequestBehavior.AllowGet);
        }

        public ActionResult WriteExceldownload()
        {

            List<WriteOffModel> cList;
            cList = (List<WriteOffModel>)Session["WriteExceldownload"];
            DataTable dt = new DataTable();
            dt.Columns.Add("S.No.");
            dt.Columns.Add("Write Off Number");
            dt.Columns.Add("Write Off Date");
            dt.Columns.Add("No of Records");
            dt.Columns.Add("Write Off Status");
            dt.Columns.Add("Uploded File");
            for (int i = 0; i < cList.Count; i++)
            {
                dt.Rows.Add(
                    i + 1
                    , cList[i]._woa_number.ToString()
                     , cList[i]._woa_date.ToString()
                      , cList[i]._no_records.ToString()
                       , cList[i]._woa_status.ToString()
                     , cList[i]._upld_fname.ToString());
            }

            //export to excel from gridview
            GridView gv = new GridView();
            gv.DataSource = dt;
            gv.DataBind();
            // Session["exceldownload"] = gv;
            if (gv.Rows.Count != 0)
            {
                //return new DownloadFileActionResult((GridView)Session["exceldownload"], "DocumentGroup.xls");
                return new DownloadFileActionResult(gv, "Write Off Summary.xls");
            }
            else
            {
                ViewBag.Message = "No records found";
            }

            return RedirectToAction("Index");
        }


        // Dashboard link Inprocess WOA
        [HttpGet]
        public ActionResult WritoffDasboardInprocessSummary()
        {
            WriteOffModel records = new WriteOffModel();
            try
            {
                records.WModel = ifr.GetWRFDtlInprocessSummary(objCmnFunc.GetLoginUserGid()).ToList();

                if (records.WModel.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
                Session["WriteExceldownload"] = records.WModel;

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

        //WOA
        [HttpPost]
        public ActionResult WritoffDasboardInprocessSummary(string status, string filter, string DATEfilter, FormCollection collections, string command)
        {
            WriteOffModel details = new WriteOffModel();
            try
            {
                if (command != "SEARCH")
                    details.WModel = ifr.GetWRFDtlInprocessSummary(objCmnFunc.GetLoginUserGid()).ToList();
                if (command == "SEARCH")
                {

                    details.WModel = ifr.GetWRFDtlInprocessSummary(objCmnFunc.GetLoginUserGid()).ToList();


                    if ((string.IsNullOrEmpty(filter) || string.IsNullOrWhiteSpace(filter)) && (string.IsNullOrEmpty(DATEfilter) || string.IsNullOrWhiteSpace(DATEfilter)) && (status == "--Select Status--"))
                    {
                        details.WModel = ifr.GetWRFDtlInprocessSummary(objCmnFunc.GetLoginUserGid()).ToList();
                    }
                    if (filter != null && filter != "")
                    {
                        ViewBag.filter = filter;
                        details.WModel = details.WModel.Where(x => filter.ToUpper() == null || (x._woa_number.ToUpper().Contains(filter.ToUpper()))).ToList();
                    }
                    if (DATEfilter != null)
                    {
                        ViewBag.filter1 = DATEfilter;
                        details.WModel = details.WModel.Where(x => DATEfilter == null || (x._woa_date.ToUpper().Contains(DATEfilter.ToUpper()))).ToList();
                    }

                    if (details.WModel.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                    Session["WriteExceldownload"] = details.WModel;
                    return View(details);
                }
                if (command == "Clear")
                { return RedirectToAction("WritoffDasboardInprocessSummary", "AssetWriteOff"); }
                else if (command == "WRITEOFF")
                {
                    return RedirectToAction("WritoffDasboardInprocessSummary");
                }
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



    }
}
