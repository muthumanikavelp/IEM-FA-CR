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
using System.Web.UI.WebControls;
using System.Web.UI;
using ClosedXML.Excel;
using System.Collections;
using IEM.Areas.IFAMS.Models;
using IEM.Common;
using IEM.App_Start;

namespace IEM.Areas.IFAMS.Controllers
{

    [NoDirectAccess]
    public class ChangeDepreciationRateController : Controller
    {
        //
        // GET: /IFAMS/changedepreciationrate/
        private IRepository ifr;
        private CmnFunctions objCmnFunc = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        public ChangeDepreciationRateController() :
            this(new DataModel())
        {

        }
        public ChangeDepreciationRateController(IRepository objModel)
        {
            ifr = objModel;
        }
        public ActionResult BulkChangeDepreciationRate()
        {
            try
            {
                Session["cdrErrdatatable"] = null;
                Session["cdrmaindatatable"] = null;
                Session["cdrRAISERattmtfileexcel"] = null;
                Session["orginalfilename"] = null;
                Session["cdrerr"] = null;
                Session["cdrTempdataset"] = null;
                Session["cdrfilename"] = null;
                Session["cdrTempdatatable"] = null;
                Session["cdrgid"] = null;
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

        private void datasupload(DataTable dataTable)
        {
            int sno = 0;
            int totalrecord = 0;
            int invalid = 0;
            int valid = 0; string grpId1 = "";
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
                                errs = "Depreciation Rate Not be Empty.";
                            }
                            else
                            {
                                errs = errs + "," + "Depreciation Rate Should Not be Empty.";
                            }
                        }
                        else
                        {
                            exceltext = dataTable.Rows[i][2].ToString(); 
                            double result=0;
                            if (!(Double.TryParse(exceltext, out result)) || exceltext=="0")
                            {
                                if (errs == "")
                                {
                                    errs = "Depreciation Rate is invalid.";
                                }
                                else
                                {
                                    errs = errs + "," + "Depreciation Rate is invalid.";
                                }
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
                ViewBag.vbvalid = "Total No of Vaild Record :" + valid;
                ViewBag.vbinvalid = "Total No of InVaild Record :" + invalid;
                ViewBag.vbtotalrecord = "Total No of Record Excel File :" + totalrecord;
                Session["cdrErrdatatable"] = Errdatatable;
                Session["cdrmaindatatable"] = maindatatable;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
        }
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
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                        filename = filename + fileextension;
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
                        Session["cdrRAISERattmtfileexcel"] = hpf;
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
        public async Task<JsonResult> UploadFiles()
        {
            string Extension1 = "";
            string error = "";
            try
            {
                string path1 = "";
                if (Session["cdrRAISERattmtfileexcel"] != null)
                {
                    HttpPostedFileBase savefile = (HttpPostedFileBase)Session["cdrRAISERattmtfileexcel"];
                    string Extension = Path.GetFileName(savefile.FileName);
                    string n = string.Format("CDR-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                    Extension1 = System.IO.Path.GetExtension(savefile.FileName);
                    Session["cdrfilename"] = n + " _ " + Extension;
                    var CmnFilePath = objCmnFunc.IEMAttachmentPath();
                    path1 = CmnFilePath + n + " _ " + Extension;
                   // path1 = @"C:\temp\" + n + " _ " + Extension;
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

                Session["cdrTempdataset"] = result1;
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
                Session["cdrerr"] = "";
                DataTable dtnew = (DataTable)Session["cdrErrdatatable"];
                DataTable dtmainnew = (DataTable)Session["cdrmaindatatable"];
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
                    wb.Worksheets.Add(dt, "ChangeDepRate_Template");
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=ChangeDepRate_Errorlog.xlsx");
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
                //Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "ChangeDepRate_Errorlog.xls"));
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
                if (Session["cdrTempdatatable"] != null)
                {
                    DataTable errdataval = new DataTable();
                    errdataval = (DataTable)Session["cdrTempdatatable"];
                    datasupload(errdataval);
                    errdataval = (DataTable)Session["cdrErrdatatable"];
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
                dtnew.Columns.Add("Depreciation Rate");
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtnew, "ChangeDepRate_Template");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=ChangeDepRate_Template.xlsx");
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
        //        dtnew.Columns.Add("Depreciation Rate");
        //        Response.ClearContent();
        //        Response.Buffer = true;
        //        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "ChangeDepRate_Template.xls"));
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
                resultwrite = (DataSet)Session["cdrTempdataset"];
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
                    && strColArr[2].ToString() == "Depreciation Rate"
                    && strColArr.Count().ToString() == "3")
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
                                && dataTable.Rows[i]["Depreciation Rate"].ToString() == ""
                               )
                            {
                                dataTable.Rows[i].Delete();
                                dataTable.AcceptChanges();
                                count--;
                                i--;
                            }
                        }
                        Session["cdrTempdatatable"] = dataTable;
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
            Session["cdrerr"] = "flag";
            return PartialView();
        }
        public JsonResult localdetails()
        {
            string check = "";
            try
            {
                Session["cdrerr"] = "flag";
                DataTable uploadedData = new DataTable();
                uploadedData = (DataTable)Session["cdrmaindatatable"];

                string filename = Session["cdrfilename"].ToString();
                check = ifr.UpdateDepRateBulk(uploadedData, filename).ToString();

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(check, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult AssetIDHelp()
        {
            ChangeDepreciationRate cdr = new ChangeDepreciationRate();
            try
            {
                cdr._ChgDepRate_list = ifr.GetAssetDetailHelp().ToList();
                if (cdr._ChgDepRate_list.Count == 0)
                {
                    ViewBag.Message = "No Records Found";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return PartialView();
            }
            finally
            {

            }
            return PartialView(cdr);
        }

        [HttpPost]
        public PartialViewResult AssetIDHelp(string assetid, string deprate, string command)
        {
            ChangeDepreciationRate cdr = new ChangeDepreciationRate();
            try
            {
                if (command == "SEARCH")
                {
                    cdr._ChgDepRate_list = ifr.GetAssetDetailHelp().ToList();
                    if (assetid != null && assetid != "")
                    {
                        ViewBag.assetid = assetid;
                        cdr._ChgDepRate_list = cdr._ChgDepRate_list.Where(x => assetid.ToUpper() == null || (x._asset_id.Contains(assetid.ToUpper()))).ToList();
                    }
                    if (deprate != null && deprate != "")
                    {
                        ViewBag.spfilter1 = deprate;
                        cdr._ChgDepRate_list = cdr._ChgDepRate_list.Where(x => deprate.ToUpper() == null || (x._dep_value.Contains(deprate.ToUpper()))).ToList();
                    }
                    if (cdr._ChgDepRate_list.Count == 0)
                    {
                        ViewBag.Message = "No records found";
                    }
                }
                if (command == "Clear")
                {

                }
                return PartialView(cdr);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView(cdr);
            }
            finally
            {

            }
        }
        public JsonResult Changetherate(ChangeDepreciationRate model)
        {
            string result = "";
            try
            {
                if (model != null)
                {
                    result = ifr.UpdateDepRateManul(model);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
