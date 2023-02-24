using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Net;
//using Excel = Microsoft.Office.Interop.Excel;
//using System.Data.OleDb;
using System.Collections;
using IEM.Areas.ASMS.Models;
using IEM.Common;
using Excel;
using IEM.App_Start;
namespace IEM.Areas.ASMS.Controllers
{
    [NoDirectAccess]
    public class OnwerUpdationController : Controller
    {
        //
        // GET: /ASMS/OnwerUpdation/
        private ASMSIRepository objModel;
        CmnFunctions objCmnFunctions = new CmnFunctions();
        CommonIUD objCommonIUD = new CommonIUD();

        public OnwerUpdationController()
            : this(new ASMSDataModel())
        {

        }
        public OnwerUpdationController(ASMSIRepository objM)
        {
            objModel = objM;
        }
        public ActionResult OnwerUpdation()
        {
            return View();
        }
        public List<SheetData> GetSheetData(string excelConnectionString)
        {
            List<SheetData> objparent = new List<SheetData>();
            SheetData objModel;
            //OleDbConnection con = null;
            //DataTable dt = null;
            //con = new OleDbConnection(excelConnectionString);
            //con.Open();
            //dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            //if (dt == null)
            //{
            //    return null;
            //}
            //int count = 0;
            //string sheets = "";
            //foreach (DataRow row in dt.Rows)
            //{
            //    sheets = row["TABLE_NAME"].ToString();
            //    sheets = sheets.Replace("$", "");
            //    count++;
            //    objModel = new SheetData();
            //    objModel.SheetId = count;
            //    objModel.SheetName = sheets.ToString();
            //    objparent.Add(objModel);
            //}
            return objparent;
        }
        
        private void datasupload(DataTable dataTable)
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
                    string exceltext = "";
                    string supdata = "";
                    string status = "";
                    string errs = "";
                    int RowNo = 0;
                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        errs = "";
                        RowNo = i + 1;
                        supdata = dataTable.Rows[i][1].ToString();
                        if (dataTable.Rows[i][1].ToString() == "")
                        {
                            errs = "Supplier Code Should Not be Empty ";
                        }
                        else
                        {
                            exceltext = dataTable.Rows[i][1].ToString();

                            status = objModel.GetStatusexcel(supdata, exceltext, "SupplierCode");
                            if (status == "notexists")
                            {
                                if (errs == "")
                                {
                                    errs = "Supplier Code Was Not Found ";
                                }
                                else
                                {
                                    errs = errs + "," + "Supplier Code Was Not Found ";
                                }
                            }
                        }

                        if (dataTable.Rows[i][2].ToString() == "")
                        {

                            if (errs == "")
                            {
                                errs = "Supplier Name Should Not be Empty";
                            }
                            else
                            {
                                errs = errs + "," + "Supplier Name Should Not be Empty";
                            }
                        }

                        if (dataTable.Rows[i][3].ToString() == "")
                        {
                            if (errs == "")
                            {
                                errs = "Existing Owner Code Should Not be Empty";
                            }
                            else
                            {
                                errs = errs + "," + "Existing Owner Code Should Not be Empty";
                            }
                        }
                        else
                        {
                            exceltext = dataTable.Rows[i][3].ToString();
                            status = objModel.GetStatusexcel(supdata, exceltext, "ExistingOwnerCode");
                            if (status == "notexists")
                            {
                                if (errs == "")
                                {
                                    errs = "Existing Owner Code Was Not Found ";
                                }
                                else
                                {
                                    errs = errs + "," + "Existing Owner Code Was Not Found ";
                                }
                            }
                        }

                        if (dataTable.Rows[i][4].ToString() == "")
                        {
                            if (errs == "")
                            {
                                errs = "Existing Owner Name Should Not be Empty";
                            }
                            else
                            {
                                errs = errs + "," + "Existing Owner Name Should Not be Empty";
                            }

                        }
                        if (dataTable.Rows[i][5].ToString() == "")
                        {
                            if (errs == "")
                            {
                                errs = "New Owner Code Should Not be Empty";
                            }
                            else
                            {
                                errs = errs + "," + "New Owner Code Should Not be Empty";
                            }
                        }
                        else
                        {
                            exceltext = dataTable.Rows[i][5].ToString();

                            status = objModel.GetStatusexcel(supdata, exceltext, "NewOwnerCode");
                            if (status == "notexists")
                            {
                                if (errs == "")
                                {
                                    errs = "New Owner Code Was Not Found ";
                                }
                                else
                                {
                                    errs = errs + "," + "New Owner Code Was Not Found ";
                                }
                            }
                        }


                        if (dataTable.Rows[i][6].ToString() == "")
                        {
                            if (errs == "")
                            {
                                errs = "New Owner Name Should Not be Empty";
                            }
                            else
                            {
                                errs = errs + "New Owner Name Should Not be Empty";
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
                    Errdatatable.Rows.Add(1, "Please  Select Valid Sheet");
                }

                invalid = Errdatatable.Rows.Count;
                valid = totalrecord - invalid;
                ViewBag.vbvalid = "Total No of vaild record(s) :" + valid;
                ViewBag.vbinvalid = "Total No of invaild record(s) :" + invalid;
                ViewBag.vbtotalrecord = "Total No of record(s) in excel file :" + totalrecord;               
                Session["Errdatatable"] = Errdatatable;
                Session["maindatatable"] = maindatatable;

            }
            catch
            {

            }
        }
        public ActionResult downloadsexcel()
        {
            
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "SupplierOwnerUpdation.xls"));
            Response.ContentType = "application/ms-excel";
          
            Session["err"] = "";
            DataTable dtnew = (DataTable)Session["Errdatatable"];
            DataTable dtmainnew = (DataTable)Session["maindatatable"];
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
        [HttpGet]
        public PartialViewResult Uploadstatus(string ddlSelectsheet=null)
        {
            try
            {
                if (Session["Tempdatatablesu"] != null)
                {
                    DataTable errdataval = new DataTable();
                    errdataval = (DataTable)Session["Tempdatatablesu"];
                    datasupload(errdataval);
                    errdataval = (DataTable)Session["Errdatatable"];
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
        public ActionResult samdownloadsexcel()
        {
            DataTable dtnew = new DataTable();
            dtnew.Columns.Add("SNo");
            dtnew.Columns.Add("Supplier Code");
            dtnew.Columns.Add("Supplier Name");
            dtnew.Columns.Add("Existing Owner Code");
            dtnew.Columns.Add("Existing Owner Name");
            dtnew.Columns.Add("New Owner Code");
            dtnew.Columns.Add("New Owner Name");
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "SupplierOwnerUpdation.xls"));
            Response.ContentType = "application/ms-excel";
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
        public JsonResult Localconveyancestatuserr(SEModel objsheet)
        {
            string mag = "";
            string strCols = "";
            string[] strColArr;
            try
            {
                DataSet result1 = new DataSet();
                result1 = (DataSet)Session["Tempdataset"];
                var dataTable = new DataTable();
                dataTable = result1.Tables[objsheet.SheetName.ToString()];
                Session["Tempdatatablesu"] = dataTable;
                foreach (DataColumn dtColumn in dataTable.Columns)
                {
                    strCols = strCols + dtColumn.ColumnName.Trim() + ":";
                }
                strCols = strCols.Substring(0, strCols.Length - 1);
                strColArr = strCols.Split(':');
                if (strColArr[0].ToString() == "SNo"
                    && strColArr[1].ToString() == "Supplier Code"
                    && strColArr[2].ToString() == "Supplier Name"
                    && strColArr[3].ToString() == "Existing Owner Code"
                    && strColArr[4].ToString() == "Existing Owner Name"
                    && strColArr[5].ToString() == "New Owner Code"
                    && strColArr[6].ToString() == "New Owner Name"
                    && strColArr.Count().ToString() == "7")
                {                   
                    mag = "Success";
                }
                else
                {
                    mag = "Faild";
                }
                return Json(mag, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult localdetails(string ddlSelectsheet)
        {
            Session["err"] = "flag";
            return PartialView();
        }
        public JsonResult localdetails()
        {
            try
            {
                Session["err"] = "flag";
                DataTable err1 = new DataTable();
                err1 = (DataTable)Session["maindatatable"];
                string check;                
                check = objModel.UpdateOwner(err1).ToString();
                return Json(check, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public PartialViewResult SearchEmployee(string listfor, string formname)
        {
            try
            {
                List<SearchEmployee> lst = new List<SearchEmployee>();
                if (listfor == "search")
                {
                    if (Session["SearchEmployees"] != null)
                    {
                        lst = (List<SearchEmployee>)Session["SearchEmployees"];
                    }
                }
                else
                {
                    lst = objModel.GetEmployeeList().ToList();
                }
                if (formname == "owner" || formname == "1")
                {
                    ViewBag.formname = "1";
                }
                else if (formname == "oic" || formname == "2")
                {
                    ViewBag.formname = "2";
                }
                else if (formname == "contactperson" || formname == "3")
                {
                    ViewBag.formname = "3";
                }

                if (ViewBag.formname != null || ViewBag.formname != "")
                {
                    Session["formname"] = ViewBag.formname;
                }

                return PartialView(lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult SearchEmployee(SearchEmployee objSearchEmployee)
        {
            try
            {
                var res = 0; ;
                List<SearchEmployee> objEmp = new List<SearchEmployee>();
                objEmp = objModel.GetEmployeeList().ToList();
                if (objEmp != null)
                {
                    if ((string.IsNullOrEmpty(objSearchEmployee._EmployeeCode)) == false)
                    {
                        objEmp = objEmp.Where(x => objSearchEmployee._EmployeeCode == null ||
                            (x._EmployeeCode.ToUpper().Contains(objSearchEmployee._EmployeeCode.ToUpper()))).ToList();
                        Session["SearchEmployees"] = objEmp;
                    }
                    if ((string.IsNullOrEmpty(objSearchEmployee._EmployeeName)) == false)
                    {
                        objEmp = objEmp.Where(x => objSearchEmployee._EmployeeName == null ||
                            (x._EmployeeName.ToUpper().Contains(objSearchEmployee._EmployeeName.ToUpper()))).ToList();
                        Session["SearchEmployees"] = objEmp;
                    }
                }
                if (objEmp.Count() == 0)
                {
                    res = 0;
                }
                else
                {
                    res = 1;
                }
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult SearchSupplier(string listfor, string formname)
        {
            try
            {
                List<SupplierHeader> lst = new List<SupplierHeader>();
                if (listfor == "search")
                {
                    if (Session["SearchSupplierName"] != null)
                    {
                        lst = (List<SupplierHeader>)Session["SearchSupplierName"];
                    }
                }
                else
                {                    
                    lst = objModel.GetStatusGridSupplierDetails().ToList();
                }

                return PartialView(lst);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult SearchSupplier(SupplierHeader objSearchSupplierName)
        {
            try
            {
                var res = 0; ;
                List<SupplierHeader> objSupName = new List<SupplierHeader>();
                objSupName = objModel.GetStatusGridSupplierDetails().ToList();

                if (objSupName != null)
                {
                    if ((string.IsNullOrEmpty(objSearchSupplierName._SupplierName)) == false)
                    {
                        objSupName = objSupName.Where(x => objSearchSupplierName._SupplierName == null ||
                            (x._SupplierName.ToUpper().Contains(objSearchSupplierName._SupplierName.ToUpper()))).ToList();
                        Session["SearchSupplierName"] = objSupName;
                    }
                }
                if (objSupName.Count() == 0)
                {
                    res = 0;
                }
                else
                {
                    res = 1;
                }
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult UpdateOwnerManual(SupplierHeader model)
        {
            try
            {
                string check = objModel.UpdateOnwer(model);
                if (check == "success")
                {
                    RedirectToAction("OnwerUpdation");
                }
                return Json(check, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<JsonResult> UploadFilesnew()
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
        [HttpPost]
        public async Task<JsonResult> UploadFiles()
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
                    string n = string.Format("OwnerUpdation-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                    Extension1 = System.IO.Path.GetExtension(savefile.FileName);
                  
                    var CmnFilePath = objCmnFunctions.IEMAttachmentPath();
                    //path1 = @"C:\temp\" + n + Extension;
                    path1 = CmnFilePath + n + Extension;
                   
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
                    objModel = new SheetData();
                    objModel.SheetId = count;
                    objModel.SheetName = sheets.ToString();
                    objparent.Add(objModel);
                }

                Session["Tempdataset"] = result1;
                return Json(objparent.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }
        }

        //[HttpPost]
        //public ActionResult UploadedFilesOwner(string uploadfor, string fname = null) 
        //{
        //    try
        //    {
        //        string filename = "";

        //        foreach (string file in Request.Files)
        //        {
        //            var fileContent = Request.Files[file];

        //            if (fileContent != null && fileContent.ContentLength > 0)
        //            {

        //                if (fname != null && fname.Trim() != "")
        //                {
        //                    filename = fname.Substring(0, fname.LastIndexOf(".") + 0);
        //                }
        //                else
        //                {
        //                    if (uploadfor != null)
        //                    {
        //                        filename = objCmnFunctions.GetSequenceNo(uploadfor);
        //                    }
        //                    else
        //                    {
        //                        filename = "ownerupdation";
        //                    }
        //                }
        //                var fileextension = Path.GetExtension(fileContent.FileName);
        //                var stream = fileContent.InputStream;
        //                var path = Path.Combine(@"C:\temp\", filename + fileextension);
        //                using (var fileStream = System.IO.File.Create(path))
        //                {
        //                    stream.CopyTo(fileStream);
        //                }
        //                filename = filename + fileextension;
        //               var excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path + ";Extended Properties=Excel 12.0;Persist Security Info=False";
        //                Session["excelfilepathlocal"] = excelConnectionString;
        //                return RedirectToAction("Localconveyancestatuserr");
        //                Session["filenamesa"] = filename;

        //            }
        //        }
        //        if (filename == null || filename == "")
        //        {
        //            return RedirectToAction("");
        //        }
        //        else { return Json(filename); }
        //    }
        //    catch (Exception)
        //    {
        //        Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //        return Json("Upload failed");
        //    }
        //}
      
    }
}
