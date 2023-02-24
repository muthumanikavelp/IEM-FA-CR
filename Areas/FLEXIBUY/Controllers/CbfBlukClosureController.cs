using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
//using Excel = Microsoft.Office.Interop.Excel;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.FLEXIBUY.Models;
using IEM.Common;
using Excel;
using IEM.Models;
using IEM.App_Start;
namespace IEM.Areas.FLEXIBUY.Controllers
{

    [NoDirectAccess]
    public class CbfBlukClosureController : Controller
    {
        //
        // GET: /FLEXIBUY/PoBulkClosure/
        private IrepositoryAn objModel;
        CmnFunctions obj = new CmnFunctions();
        int count = 0;
        public CbfBlukClosureController()
            : this(new CbfSumModel())
        {

        }
        public CbfBlukClosureController(IrepositoryAn objM)
        {
            objModel = objM;
        }
        public ActionResult CBFBulkClosure(string viewfor)
        {
            CBFBulkClosure objClosure = new CBFBulkClosure();
            Session["exceldatas"] = null;
            Session["cbfbulkclfile"] = null;
            if (viewfor == "cancel")
            {
                Session["maindatatable"] = null;
                List<CBFBulkClosure> lLstClosure = new List<CBFBulkClosure>();
                objClosure.lListBulkUpload = lLstClosure;
                return View("CBFBulkClosure", objClosure);
            }
        
            if (Session["maindatatable"] != null)
            {
                DataTable dt = (DataTable)Session["maindatatable"];
                List<CBFBulkClosure> objlist = new List<CBFBulkClosure>();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objClosure = new CBFBulkClosure();
                    objClosure.slNo = i + 1;
                    objClosure.CbfNo = dt.Rows[i]["CBFNo"].ToString();
                    objClosure.CBfDate = obj.ddMMyyyyString(dt.Rows[i]["CBFDate"].ToString());
                    objClosure.remarks = dt.Rows[i]["Remarks"].ToString();
                    objlist.Add(objClosure);
                }
                objClosure.lListBulkUpload = objlist;
                return View(objClosure);
            }

            else
            {
                Session["filename"] = null;
                List<CBFBulkClosure> lLstClosure = new List<CBFBulkClosure>();
                objClosure.lListBulkUpload = lLstClosure;
                return View(objClosure);
            }
        }
        public async Task<JsonResult> ExcelUplFiles()
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
                        Session["cbfbulkclfile"] = hpf;
                    }
                }
                return Json(filename);
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }
        }
        public string HoldFileUploadUrlDSA()
        {
            string x = "";
            try
            {
                x = System.Configuration.ConfigurationManager.AppSettings["IEMFileUpload"].ToString();
            }
            catch { x = ""; }
            return (x == null || x.Trim() == "") ? "" : x;
        }
        [HttpPost]
        public async Task<JsonResult> FileUpload()
        {
            string Extension1 = "";
            string error = "";
            try
            {
                string path1 = "";
                if (Session["cbfbulkclfile"] != null)
                {
                    HttpPostedFileBase savefile = (HttpPostedFileBase)Session["cbfbulkclfile"];
                    string Extension = Path.GetFileName(savefile.FileName);
                    string n = string.Format("cbfbulkclosure-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                    Extension1 = System.IO.Path.GetExtension(savefile.FileName);
                    path1 = HoldFileUploadUrlDSA() + n + Extension;
                    if (System.IO.File.Exists(path1))
                    {
                        System.IO.File.Delete(path1);
                    }
                    savefile.SaveAs(path1);
                }
                List<SheetDatasCBF> objparent = new List<SheetDatasCBF>();
                SheetDatasCBF objModel;
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
                    objModel = new SheetDatasCBF();
                    objModel.sheetId = count;
                    objModel.sheetName = error.ToString();
                    objparent.Add(objModel);
                }

                //FileStream stream = new FileStream();
                //stream = File.Open(path1, FileMode.Open, FileAccess.Read);
                //1. Reading from a binary Excel file ('97-2003 format; *.xls)
                //IExcelDataReader excelReaderf = ExcelReaderFactory.CreateBinaryReader(stream);
                //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
                //IExcelDataReader excelReadern = ExcelReaderFactory.CreateOpenXmlReader(stream);
                //...
                //3. DataSet - The result of each spreadsheet will be created in the result.Tables
                //DataSet result = excelReader.AsDataSet();
                //4. DataSet - Create column names from first row

                foreach (DataTable dt in result1.Tables)
                {
                    sheets = dt.TableName.ToString().Trim();
                    count++;
                    objModel = new SheetDatasCBF();
                    objModel.sheetId = count;
                    objModel.sheetName = sheets.ToString();
                    objparent.Add(objModel);
                }

                Session["exceldatas"] = result1;
                return Json(objparent.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }
        }
        //public async Task<JsonResult> FileUpload()
        //{
        //    HttpPostedFileBase httpfileName = null;
        //    string excelConnectionString = "";
        //    try
        //    {
        //        foreach (string file in Request.Files)
        //        {
        //            var fileContent = Request.Files[file];

        //            if (fileContent != null && fileContent.ContentLength > 0)
        //            {
        //                httpfileName = Request.Files[file] as HttpPostedFileBase;
        //                Session["localFileup"] = httpfileName;
        //            }
        //            string Extension = Path.GetFileName(httpfileName.FileName);
        //            Session["filename"]=Extension;
        //            string n = string.Format("Local-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
        //            string Extension1 = System.IO.Path.GetExtension(Request.Files[file].FileName);
        //            string path1 = @"C:\temp\" + n + Extension;
        //            if (System.IO.File.Exists(path1))
        //            {
        //                System.IO.File.Delete(path1);
        //            }
        //            Request.Files[file].SaveAs(path1);

        //            excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=Excel 12.0;Persist Security Info=False";
        //            Session["excelFilePathLocal"] = excelConnectionString;

        //        }
        //        return Json(GetSheetData(excelConnectionString).ToList(), JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //        return Json("Upload failed");
        //    }
        //}

        //public List<SheetDatasCBF> GetSheetData(string excelConnectionString)
        //{
        //    List<SheetDatasCBF> objparent = new List<SheetDatasCBF>();
        //    SheetDatasCBF objSheet;
        //    OleDbConnection con = null;
        //    DataTable dt = null;
        //    con = new OleDbConnection(excelConnectionString);
        //    con.Open();
        //    dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        //    if (dt == null)
        //    {
        //        return null;
        //    }
        //    int count = 0;
        //    string sheets = "";
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        sheets = row["TABLE_NAME"].ToString();
        //        sheets = sheets.Replace("$", "");
        //        count++;
        //        objSheet = new SheetDatasCBF();
        //        objSheet.sheetId = count;
        //        objSheet.sheetName = sheets.ToString();
        //        objparent.Add(objSheet);
        //    }
        //    return objparent;
        //}
        public ActionResult DownloadExcel()
        {
            DataTable objTable = new DataTable();
            objTable.Columns.Add("SNo");
            objTable.Columns.Add("CBFNo");
            objTable.Columns.Add("CBFDate");
            objTable.Columns.Add("Remarks");
            Response.ClearContent();
            Response.Buffer = true;
            //Response.ContentType = "application/application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.AppendHeader("content-disposition", "attachment; filename=ShipmentTemplate.xlsx");
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "ShipmentTemplate.xls"));
            //Response.ContentType = "application/ms-excel";
            Response.ContentType = "application/vnd.ms-excel";
            // Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            DataTable dt = objTable;
            string str = string.Empty;
            foreach (DataColumn dtcol in dt.Columns)
            {
                Response.Write(str + dtcol.ColumnName);
                str = "\t";
            }
            Response.Write("\n");
            foreach (DataRow dr in dt.Rows)
            {
                str = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Response.Write(str + Convert.ToString(dr[j]));
                    str = "\t";
                }
                Response.Write("\n");
            }
            Response.End();
            return View();
        }
        public ActionResult DownloadErrorLogExcel()
        {
            DataTable dtnew = (DataTable)Session["Errdatatable"];
            Response.ClearContent();
            Response.Buffer = true;
            // Response.AddHeader('Content-Disposition: attachment; filename="Shipment.xls"');
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Shipment.xls"));
            Response.ContentType = "application/vnd.ms-excel";
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            DataTable dt = dtnew;
            string str = string.Empty;
            foreach (DataColumn dtcol in dt.Columns)
            {
                Response.Write(str + dtcol.ColumnName);
                str = "\t";
            }
            Response.Write("\n");
            foreach (DataRow dr in dt.Rows)
            {
                str = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    Response.Write(str + Convert.ToString(dr[j]));
                    str = "\t";
                }
                Response.Write("\n");
            }
            Response.End();
            return View();
        }
        //[HttpGet]
        //public PartialViewResult UploadSummaryTable()
        //{
        //    try
        //    {
        //        BulkUpload obj = new BulkUpload();
        //        DataTable objTable = new DataTable();
        //        objTable = (DataTable)Session["maindatatable"];
        //        obj.lListUpload = objModel.GetUploadSummary(objTable);
        //        return PartialView("UploadSummaryTable", obj);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public JsonResult ColumnValidation(BulkUploadCBF objValid)
        {
            try
            {
                string result = string.Empty;
                string strCols = string.Empty;
                string[] strColArr;
                 DataSet result1 = new DataSet();
                result1 = (DataSet)Session["exceldatas"];
                var dataTable = new DataTable();
                dataTable = result1.Tables[objValid.sheetName.ToString()];
                //string excelname = objValid.sheetName + "$";
                //string excelConnectionString = Session["excelFilePathLocal"].ToString();
                //using (OleDbConnection con = new OleDbConnection(excelConnectionString))
                //{
                //    var dataTable = new DataTable();
                //    string query = string.Format("SELECT * FROM [{0}]", excelname);
                //    con.Open();
                //    OleDbDataAdapter adapter = new OleDbDataAdapter(query, con);
                //    adapter.Fill(dataTable);
                    Session["ExcelTable"] = dataTable;

                    foreach (DataColumn dtColumn in dataTable.Columns)
                    {
                        strCols = strCols + dtColumn.ColumnName.Trim() + ":";
                    }
                    strCols = strCols.Substring(0, strCols.Length - 1);
                    strColArr = strCols.Split(':');
                    if (strColArr[0].ToString() == "SNo" && strColArr[1].ToString() == "CBFNo" && strColArr[2].ToString() == "CBFDate" && strColArr[3].ToString() == "Remarks" && strColArr.Count().ToString() == "4")
                    {
                        result = "Success";
                    }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult BulkUploadStatusForCBF()
        {
            CBFBulkClosure objValid = new CBFBulkClosure();
            try
            {
                if (Session["ExcelTable"] != null)
                {
                    DataTable objTable = new DataTable();
                    objTable = (DataTable)Session["ExcelTable"];
                    ValidateExcel(objTable);
                    DataTable objerrTable = new DataTable();
                    objerrTable = (DataTable)Session["Errdatatable"];
                    if (objerrTable.Rows.Count == 0)
                    {
                        ViewBag.visble = "Yes";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView();
        }

        private void ValidateExcel(DataTable objTable)
        {
            int sno = 0;
            int totalrecord = 0;
            int invalid = 0;
            int valid = 0;
            string columnName = string.Empty;

            DataTable objMainTable = new DataTable();
            objMainTable = objTable;
            totalrecord = objMainTable.Rows.Count;
            DataTable objErrTable = new DataTable();
            objErrTable.Columns.Add("SNo");
            objErrTable.Columns.Add("Line");
            objErrTable.Columns.Add("Column Name");
            objErrTable.Columns.Add("Error Description");


            try
            {
                if (objTable.Rows.Count > 0)
                {
                    string exceltext = "";
                    string status = "";
                    string errs = "";
                    int RowNo = 0;

                    for (int i = 0; i < objTable.Rows.Count; i++)
                    {
                        errs = "";
                        columnName = string.Empty;
                        RowNo = i + 1;
                        if (objTable.Rows[i][1].ToString() == "")
                        {
                            errs = "CBFNO Should Not be Empty ";
                            columnName = objTable.Columns[1].ToString();
                        }
                        else
                        {
                            exceltext = objTable.Rows[i][1].ToString();
                            status = objModel.GetCBFClosureExcelValid(exceltext, "CBFno");
                            if (status == "notexists")
                            {
                                if (errs == "" && columnName == "")
                                {
                                    errs = "CBFNO Was Not Found ";
                                    columnName = objTable.Columns[1].ToString();
                                }
                                else
                                {
                                    errs = errs + "," + "CBFNO Was Not Found ";
                                    columnName = columnName + "," + objTable.Columns[1].ToString();
                                }
                            }
                        }

                        if (objTable.Rows[i][2].ToString() == "")
                        {

                            if (errs == "" && columnName == "")
                            {
                                errs = "CBFDATE Should Not be Empty";
                                columnName = objTable.Columns[2].ToString();
                            }
                            else
                            {
                                errs = errs + "," + "CBFDATE Should Not be Empty";
                                columnName = columnName + "," + objTable.Columns[2].ToString();
                            }
                        }
                        else
                        {
                            exceltext = obj.convertoDateTimeString(objTable.Rows[i][2].ToString());
                            status = objModel.GetCBFClosureExcelValid(exceltext, "CBFdate");
                            if (status == "notexists")
                            {
                                if (errs == "" && columnName == "")
                                {
                                    errs = "CBFDATE Was Not Found ";
                                    columnName = objTable.Columns[2].ToString();
                                }
                                else
                                {
                                    errs = errs + "," + "CBFDATE Was Not Found ";
                                    columnName = columnName + "," + objTable.Columns[2].ToString();
                                }
                            }

                        }
                        if (objTable.Rows[i][3].ToString() == "")
                        {
                            if (errs == "" && columnName == "")
                            {
                                errs = "Remarks Should Not be Empty";
                                columnName = objTable.Columns[3].ToString();
                            }
                            else
                            {
                                errs = errs + "," + "Remarks Should Not be Empty";
                                columnName = columnName + "," + objTable.Columns[3].ToString();
                            }
                        }
                        else
                        {
                            if (objTable.Rows[i][3].ToString().Length > 256)
                            {
                                if (errs == "" && columnName == "")
                                {
                                    errs = "Remarks Should not be Greater than 250 Characters";
                                    columnName = objTable.Columns[3].ToString();
                                }
                                else
                                {
                                    errs = errs + "," + "Remarks Should not be Greater than 250 Characters";
                                    columnName = columnName + "," + objTable.Columns[3].ToString();
                                }
                            }
                        }
                        if (errs != "")
                        {
                            sno++;
                            objErrTable.Rows.Add(sno, RowNo, columnName, errs);
                        }
                    }
                   
                }
                else
                {
                    objErrTable.Rows.Add(1, "Please  Select Valid Sheet");
                }
                invalid = objErrTable.Rows.Count;
                valid = totalrecord - invalid;
                Session["validrecords"] = valid;
                ViewBag.validRecords = "Total No of Vaild Records :" + valid;
                ViewBag.invalidRecords = "Total No of InVaild Records :" + invalid;
                ViewBag.totalRecords = "Total No of Records in Excel File :" + totalrecord;
                Session["Errdatatable"] = objErrTable;
                Session["maindatatable"] = objMainTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult InsertBulkClosure()
        {
            try
            {
                CBFBulkClosure objClosure = new CBFBulkClosure();
                if (Session["maindatatable"] != null && Session["validrecords"] != null && Session["filename"]!=null)
                {
                    DataTable dt = (DataTable)Session["maindatatable"];
                    int validRecords = (int)Session["validrecords"];
                    string fileName = (string)Session["filename"];
                    objClosure.result = objModel.BulkCBFInsert(dt, validRecords, fileName);
                }
                if (objClosure.result == "Success")
                {
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
