using IEM.Areas.EOW.Models;
using IEM.Common;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using System.Data.OleDb;
using System.Collections;
using Excel;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;
using ClosedXML.Excel;
using System.Web.Hosting;
using IEM.Areas.Dashboard.Models;
using Rotativa.Options;
using IEM.Helper;
using IEM.Areas.MASTERS.Controllers;
using Newtonsoft.Json;
using IEM.App_Start;
namespace IEM.Areas.EOW.Controllers
{

    [NoDirectAccess]
    public class LocalConveyanceNewController : Controller
    {
        //
        // GET: /EOW/LocalConveyanceNew/
        ErrorLog objErrorLog = new ErrorLog();
        private EOW_IRepository objModel;
        CmnFunctions objCmnFunctions = new CmnFunctions();
        private FileServer Cmnfs = new FileServer();
        CommonIUD objCommonIUD = new CommonIUD();
        proLib plib = new proLib();
        dbLib db = new dbLib();
        DataTable dt;
        public LocalConveyanceNewController()
            : this(new EOW_DataModel())
        {

        }
        public LocalConveyanceNewController(EOW_IRepository objM)
        {
            objModel = objM;
        }
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                Session["QueueGide"] = "";
                if (Session["DashBoard"] != null)
                {
                    EOW_EmployeeeExpense eowemp = new EOW_EmployeeeExpense();
                    eowemp = (EOW_EmployeeeExpense)Session["DashBoard"];
                    if (eowemp.ecfremark != null)
                    {
                        ViewBag.ecfrmarks = eowemp.ecfremark;
                    }
                    Session["EcfGid"] = eowemp.ecf_GID.ToString();
                    Session["invoiceGid"] = eowemp.invoice_GID.ToString();
                    if (eowemp.Queue_GID != 0)
                    {
                        Session["QueueGide"] = eowemp.Queue_GID.ToString();
                    }
                    else
                    {
                        Session["QueueGide"] = "";
                    }
                    ViewBag.processdata = "first";
                    ViewBag.process = "second";
                    ViewBag.processdatasec = "first";
                    Session["Ecfamountlocal"] = eowemp.ECF_Amount;
                    Session["delmatamount"] = eowemp.ecfdelmatamt;
                    Session["EcfDatemain"] = eowemp.ECF_Date;
                    Session["EcfDatemainmonth"] = getconverttomonthtodateto(eowemp.ClaimMonth);
                    //eowemp.ClaimMonth = getconverttomonthtodateto(eowemp.ClaimMonth);
                    if (eowemp.raisermodeId == "P")
                    {
                        Session["SelfModeRaiseid"] = objCmnFunctions.GetLoginProxyUserGid().ToString();
                    }
                    else
                    {
                        Session["SelfModeRaiseid"] = objCmnFunctions.GetLoginUserGid().ToString();
                    }
                    eowemp.ECF_Amount = objCmnFunctions.GetINRAmount(eowemp.ECF_Amount);
                    ViewBag.EOW_EmployeeeExpenseheader = eowemp;
                    Session["DashBoard"] = null;
                    Session["SelfModeecfval"] = objCmnFunctions.GetLoginUserGid().ToString();
                }
                else
                {
                    Session["SelfModeecfval"] = objCmnFunctions.GetLoginUserGid().ToString();
                    List<EOW_EmployeeeExpense> objmaindetail = new List<EOW_EmployeeeExpense>();
                    objmaindetail = objModel.SelectEmployeeeheader(objCmnFunctions.GetLoginUserGid().ToString()).ToList();

                    EOW_EmployeeeExpense eowemp = new EOW_EmployeeeExpense();
                    eowemp.Grade = objmaindetail[0].Grade.ToString();
                    eowemp.Raiser_Code = objmaindetail[0].Raiser_Code.ToString();
                    eowemp.Raiser_Name = objmaindetail[0].Raiser_Name.ToString();
                    eowemp.raisermodeId = "S";
                    eowemp.Raiser_Modedata = new SelectList(objModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", eowemp.raisermodeId);
                    ViewBag.EOW_EmployeeeExpenseheader = eowemp;
                    ViewBag.process = "";
                    Session["EcfGid"] = null;
                    Session["invoiceGid"] = null;
                    Session["Ecfamountpayment1"] = null;
                    Session["Ecfamountval"] = null;
                    Session["Ecfamountvalmain"] = null;
                    ViewBag.processdata = "first";
                }
                Session["Ecfdeclaratonnote"] = objModel.GetDecnote("2", "S").ToString();
                return View();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }

        }

        [HttpPost]
        public ActionResult Index(EOW_EmployeeeExpense obj)
        {
            try
            {
                string ecfnewamt = obj.ECF_Amount;
                obj.ECF_Amount = objCmnFunctions.GetRemovecommas(obj.ECF_Amount);
                if (obj.ecfremark != null)
                {
                    ViewBag.ecfrmarks = obj.ecfremark;
                }
                string clmmonth = "";
                string message = "";
                List<EOW_EmployeeeExpense> objmaindetail = new List<EOW_EmployeeeExpense>();
                objmaindetail = objModel.SelectEmployeeeheader(objCmnFunctions.GetLoginUserGid().ToString()).ToList();
                clmmonth = obj.ClaimMonth;
                obj.Grade = objmaindetail[0].Grade.ToString();
                obj.Raiser_Code = objmaindetail[0].Raiser_Code.ToString();
                obj.Raiser_Name = objmaindetail[0].Raiser_Name.ToString();
                obj.raisermodeId = "S";
                Session["Ecfamountlocal"] = obj.ECF_Amount;
                Session["EcfDatemain"] = obj.ECF_Date;
                Session["EcfDatemainmonth"] = getconverttomonthtodate(obj.ClaimMonth);
                obj.Raiser_Modedata = new SelectList(objModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", obj.raisermodeId);
                if (obj.raisermodeId == "P")
                {
                    Session["SelfModeRaiseid"] = objCmnFunctions.GetLoginProxyUserGid().ToString();
                }
                else
                {
                    Session["SelfModeRaiseid"] = objCmnFunctions.GetLoginUserGid().ToString();
                }

                obj.ClaimMonth = getconverttomonthtodate(obj.ClaimMonth);
                int nodays = 0;
                string result = objModel.GetStatusexcel("", Session["SelfModeecfval"].ToString(), "", "Ecfraiser");
                nodays = Convert.ToInt32(result);
                if (nodays < 15)
                {
                    ViewBag.process = "third";
                    ViewBag.processdata = "first";
                    ViewBag.processdatasec = "third";
                    ViewBag.message = "Employee Can't Raise ECF";
                }
                else
                {
                    if (Session["EcfGid"] != null)
                    {
                        message = objModel.InsertEmployeeeBasicupdate(obj, objCmnFunctions.GetLoginUserGid().ToString(), objCmnFunctions.GetLoginUserGid().ToString(), Session["EcfGid"].ToString());
                        if (message != "")
                        {
                            ViewBag.processdata = "first";
                            ViewBag.process = "second";
                            if (Session["err"] == "flag" || (Session["QueueGide"] != "" && Session["QueueGide"] != "0" && Session["QueueGide"] != null))
                            {
                                ViewBag.processdatasec = "first";
                                Session["err"] = "";
                            }
                        }
                    }
                    else
                    {
                        if (obj.ClaimMonth != null && obj.ECF_Amount != null && obj.ECF_Date != null)
                        {
                            message = objModel.InsertEmployeeeBasic(obj, objCmnFunctions.GetLoginUserGid().ToString(), "L");
                            if (message != "")
                            {
                                ViewBag.processdata = "first";
                                ViewBag.process = "second";
                                Session["EcfGid"] = objModel.selectecfgidBasic(obj, objCmnFunctions.GetLoginUserGid().ToString());
                            }
                        }
                        else
                        {
                            ViewBag.processdata = "first";
                            // ViewBag.process = "second";
                        }
                    }
                }
                obj.ClaimMonth = clmmonth;
                obj.ECF_Amount = ecfnewamt;
                ViewBag.EOW_EmployeeeExpenseheader = obj;
                return View();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return View();
            }
        }
        [HttpPost]
        public async Task<JsonResult> UploadFiles()
        {
            string excelConnectionString = "";
            try
            {
                foreach (string file in Request.Files)
                {
                    //var fileContent = Request.Files[file];

                    //if (fileContent != null && fileContent.ContentLength > 0)
                    //{
                    //    hpfile = Request.Files[file] as HttpPostedFileBase;
                    //    Session["localfileup"] = hpfile;
                    //}
                    //string Extension = Path.GetFileName(hpfile.FileName);
                    //string n = string.Format(objCmnFunctions.GetLoginUserGid().ToString() + "Local-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                    //string Extension1 = System.IO.Path.GetExtension(Request.Files[file].FileName);
                    //string path1 = @"C:\Temp\" + n + Extension;
                    //if (System.IO.File.Exists(path1))
                    //{
                    //    System.IO.File.Delete(path1);
                    //}
                    //Request.Files[file].SaveAs(path1);

                    //excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=Excel 12.0;Persist Security Info=False";
                    //Session["excelfilepathlocal"] = excelConnectionString;

                    HttpPostedFileBase hpfBase = Request.Files[file] as HttpPostedFileBase;

                    string Extension = Path.GetFileName(hpfBase.FileName);
                    string n = string.Format(objCmnFunctions.GetLoginUserGid().ToString() + "Local-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                    string Extension1 = System.IO.Path.GetExtension(Request.Files[file].FileName);
                    string path1 = objModel.HoldFileUploadUrl() + n + Extension;
                    if (System.IO.File.Exists(path1))
                    {
                        System.IO.File.Delete(path1);
                    }
                    Request.Files[file].SaveAs(path1);

                    excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + path1 + ";Extended Properties=Excel 12.0;Persist Security Info=False";
                    Session["excelfilepathlocal"] = excelConnectionString;

                }
                return Json(GetSheetData(excelConnectionString).ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred...", JsonRequestBehavior.AllowGet);
            }
        }

        public string HoldFileUploadUrlDSA()
        {
            string x = "";
            try
            {
                x = System.Configuration.ConfigurationManager.AppSettings["HoldFileUpload"].ToString();
            }
            catch { x = ""; }
            return (x == null || x.Trim() == "") ? "" : x;
        }
        [HttpPost]
        public ActionResult UploadContact(HttpPostedFileBase file)
        {
            return PartialView();
        }
        [HttpPost]
        public void UploadFileslocal()
        {
            try
            {
                foreach (string filenew in Request.Files)
                {
                    var fileContent = Request.Files[filenew];

                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        HttpPostedFileBase hpf = Request.Files[filenew] as HttpPostedFileBase;
                        TempData["_attFileupload"] = hpf;
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
        }
        [HttpPost]
        public async Task<JsonResult> getdsheets()
        {
            List<SheetData> objparent = new List<SheetData>();
            Session["Tempdatasetlocal"] = "";
            string Extensionnew = "";
            string Extension1 = "";
            string error = "";
            try
            {
                string path1 = "";
                if (TempData["_attFileupload"] != null)
                {
                    HttpPostedFileBase savefile = TempData["_attFileupload"] as HttpPostedFileBase;
                    string Extension = Path.GetFileName(savefile.FileName);
                    string n = string.Format("Local-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                    Extension1 = System.IO.Path.GetExtension(savefile.FileName);
                    Extensionnew = n + Extension1;
                    path1 = string.Format("{0}{1}", HoldFileUploadUrlDSA(), Extensionnew);
                    if (System.IO.File.Exists(path1))
                    {
                        System.IO.File.Delete(path1);
                    }
                    savefile.SaveAs(path1);

                    SheetData objModel;
                    int count = 0;
                    string sheets = "";
                    FileStream stream = new FileStream(path1, FileMode.Open, FileAccess.Read);
                    DataSet result1 = new DataSet();
                    if (Extension1.ToLower() == ".xlsx")
                    {
                        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        excelReader.IsFirstRowAsColumnNames = true;
                        result1 = excelReader.AsDataSet();
                        excelReader.Close();
                    }
                    else if (Extension1.ToLower() == ".xls")
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
                    if (result1 != null && result1.Tables.Count > 0)
                    {
                        foreach (DataTable dt in result1.Tables)
                        {
                            sheets = dt.TableName.ToString().Trim();
                            count++;
                            objModel = new SheetData();
                            objModel.SheetId = count;
                            objModel.SheetName = sheets.ToString();
                            objparent.Add(objModel);
                        }
                    }
                    Session["Tempdatasetlocal"] = result1;
                    TempData.Remove("_attFileupload");
                }
                return Json(objparent.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred...", JsonRequestBehavior.AllowGet);
            }
        }

        public string[] GetExcelSheetNames(string excelConnectionString)
        {
            OleDbConnection con = null;
            DataTable dt = null;
            con = new OleDbConnection(excelConnectionString);
            con.Open();
            dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (dt == null)
            {
                return null;
            }
            String[] excelSheetNames = new String[dt.Rows.Count];
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
                excelSheetNames[i] = row["TABLE_NAME"].ToString();
                i++;
            }
            return excelSheetNames;
        }
        public List<SheetData> GetSheetData(string excelConnectionString)
        {
            List<SheetData> objparent = new List<SheetData>();
            try
            {
                SheetData objModel;
                OleDbConnection con = null;
                DataTable dt = null;
                con = new OleDbConnection(excelConnectionString);
                con.Open();
                dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dt == null)
                {
                    return null;
                }
                int count = 0;
                string sheets = "";
                foreach (DataRow row in dt.Rows)
                {
                    sheets = row["TABLE_NAME"].ToString();
                    sheets = sheets.Replace("$", "");
                    count++;
                    objModel = new SheetData();
                    objModel.SheetId = count;
                    objModel.SheetName = sheets.ToString();
                    objparent.Add(objModel);
                }
                return objparent;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objparent;
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

                DataTable errmain = new DataTable();
                errmain = (DataTable)Session["gridmaindatatable"];

                List<EOW_Payment> objlist = new List<EOW_Payment>();
                EOW_Payment objModell;

                string result = objModel.GetEmployeeelocal(errmain, err1, Session["EcfGid"].ToString(), Session["EcfDatemain"].ToString());
                //objlist = objModel.GetEmployeeePaymentlocal(err1, Session["EcfGid"].ToString(), Session["EcfDatemain"].ToString()).ToList();
                //Session["objlist"] = objlist;
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred...", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult _Localconveyancecclear(EOW_SupplierModelgrid Suppliermodel)
        {
            string Err = "";
            string ecfno = "";
            try
            {
                ecfno = Session["EcfGid"].ToString();
                Err = "L";
                string insrtinvoice = objModel.DeleteSuppliernewlstdsa(Err, ecfno);
                if (insrtinvoice == "Success")
                {
                    Session["invoiceclear"] = "clear";
                    Session["err"] = "";
                    Session["invoiceGid"] = "";
                    Session["delmatamount"] = "0";
                    Err = "Success";
                }
                if (Err == "")
                {
                    Err = "Error";
                }
                return Json(Err, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred...", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult Localconveyancestatuserr(EOW_TravelClaim obj)
        {
            string mag = "";
            string strCols = "";
            string[] strColArr;
            try
            {
                Session["EcfDatemainmonth"] = getconverttomonthtodate(obj.ClaimMonth);
                Session["Tempdatatableamt"] = "0";
                Session["Tempdatatable"] = "";
                Session["Errdatatable"] = null;
                Session["gridmaindatatable"] = null;
                Session["maindatatable"] = null;
                var dataTable = new DataTable();
                DataSet result1 = new DataSet();
                result1 = (DataSet)Session["Tempdatasetlocal"];
                dataTable = result1.Tables[obj.Branch.ToString()];
                Session["Tempdatatable"] = dataTable;


                //string excelname = obj.Branch.ToString().Trim() + "$";
                //string excelConnectionString = Session["excelfilepathlocal"].ToString();
                //using (OleDbConnection con = new OleDbConnection(excelConnectionString))
                //{
                //    string query = string.Format("SELECT * FROM [{0}]", excelname);
                //    con.Open();
                //    OleDbDataAdapter adapter = new OleDbDataAdapter(query, con);
                //    adapter.Fill(dataTable);
                //    Session["Tempdatatable"] = dataTable;
                //}
                foreach (DataColumn dtColumn in dataTable.Columns)
                {
                    strCols = strCols + dtColumn.ColumnName.Trim() + ":";
                }
                strCols = strCols.Substring(0, strCols.Length - 1);
                strColArr = strCols.Split(':');
                if (strColArr[0].ToString() == "SNo"
                    //&& strColArr[1].ToString() == "Nature of Expenses" 
                    //&& strColArr[2].ToString() == "Main Category"
                    //&& strColArr[3].ToString() == "Sub Category"
                    && strColArr[1].ToString() == "Employee ID"
                    && strColArr[2].ToString() == "Employee Name"
                    //&& strColArr[6].ToString() == "Branch"
                    //&& strColArr[7].ToString() == "From Date"
                    //&& strColArr[8].ToString() == "To Date"
                    && strColArr[3].ToString() == "Date"
                    && strColArr[4].ToString() == "Place From"
                    && strColArr[5].ToString() == "Place To"
                    && strColArr[6].ToString() == "Travel Mode"
                    && strColArr[7].ToString() == "Distance"
                    && strColArr[8].ToString() == "Rate"
                    && strColArr[9].ToString() == "Amount"
                    //&& strColArr[13].ToString() == "Function Code"
                    //&& strColArr[14].ToString() == "Cost Code"
                    //&& strColArr[15].ToString() == "Product Code"
                    //&& strColArr[16].ToString() == "OU Code"
                    && strColArr.Count().ToString() == "10")
                {
                    Session["Tempdatatableamt"] = obj.Amount;
                    mag = "Success";
                }
                else
                {
                    if (strColArr.Count().ToString() == "10")
                    {
                        Session["Tempdatatableamt"] = obj.Amount;
                        mag = "Success";
                    }
                    else
                    {
                        mag = "Faild";
                    }
                }
                return Json(mag, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred...", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public PartialViewResult _Localconveyancestatus(string ddlSelectsheet, string noperson, string ecfamount)
        {
            try
            {
                if (Session["Tempdatatable"] != null)
                {
                    DataTable errdataval = new DataTable();
                    errdataval = (DataTable)Session["Tempdatatable"];
                    //string ecfamount = Session["Tempdatatableamt"].ToString();
                    datasupload(errdataval, ecfamount, noperson);
                    errdataval = (DataTable)Session["Errdatatable"];
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

        public ActionResult downloadsexcel()
        {
            Session["err"] = "";
            DataTable dtnew = (DataTable)Session["Errdatatable"];
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "LocalConveyance.xls"));
            Response.ContentType = "application/vnd.ms-excel";
            DataTable dt = dtnew;
            if (dtnew.Rows.Count > 0)
            {
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
            }
            return View();
        }
        public ActionResult samdownloadsexcel()
        {
            using (DataTable dtnew = new DataTable())
            {
                dtnew.Columns.Add("SNo", typeof(int));
                dtnew.Columns.Add("Employee ID", typeof(string));
                dtnew.Columns.Add("Employee Name", typeof(string));
                dtnew.Columns.Add("Date", typeof(DateTime));
                dtnew.Columns.Add("Place From", typeof(string));
                dtnew.Columns.Add("Place To", typeof(string));
                dtnew.Columns.Add("Travel Mode", typeof(string));
                dtnew.Columns.Add("Distance", typeof(int));
                dtnew.Columns.Add("Rate", typeof(decimal));
                dtnew.Columns.Add("Amount", typeof(decimal));
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtnew, "LocalConveyance");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=LocalConveyanceTemplate.xls");
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
        public FileResult DownloadTemplate()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(HostingEnvironment.MapPath("~/Download"));
            byte[] fileBytes = System.IO.File.ReadAllBytes(dirInfo.FullName + "/LCFTemplate.xls");
            string fileName = "LCFTemplate.xls";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        private void datasupload(DataTable dataTable, string ecfamount, string noperson)
        {
            int sno = 0;
            int totalrecord = 0;
            int invalid = 0;
            int valid = 0;
            decimal tolamt = 0;
            decimal tolamtexcel = 0;
            if (ecfamount != "")
            {
                tolamtexcel = Convert.ToDecimal(ecfamount.ToString().Trim());
            }

            DataTable gridmaindatatable = new DataTable();
            DataTable maindatatable = new DataTable();
            Hashtable empchck = new Hashtable();

            gridmaindatatable.Columns.Add("SNo");
            gridmaindatatable.Columns.Add("EmployeeID");
            gridmaindatatable.Columns.Add("DateFrom");
            gridmaindatatable.Columns.Add("DateTo");
            gridmaindatatable.Columns.Add("Amount");
            gridmaindatatable.Columns.Add("emp_gid");
            gridmaindatatable.Columns.Add("emp_name");
            gridmaindatatable.Columns.Add("emp_branchgid");
            gridmaindatatable.Columns.Add("emp_branchcode");
            gridmaindatatable.Columns.Add("emp_fccode");
            gridmaindatatable.Columns.Add("emp_cccode");
            gridmaindatatable.Columns.Add("emp_oucode");
            gridmaindatatable.Columns.Add("emp_productcode");
            gridmaindatatable.Columns.Add("NatureofExpenses");
            gridmaindatatable.Columns.Add("MainCategory");
            gridmaindatatable.Columns.Add("SubCategory");

            string loginbranch = objModel.selectemmpdetailforlocallogn(objCmnFunctions.GetLoginUserGid().ToString());
            string NatureofExpenses = Convert.ToString(ConfigurationManager.AppSettings["Ecflocalnature"].ToString());
            string MainCategory = Convert.ToString(ConfigurationManager.AppSettings["Ecflocalcat"].ToString());
            string SubCategory = Convert.ToString(ConfigurationManager.AppSettings["Ecflocalsubcat"].ToString());

            string todatedate = objModel.GetStatusexcel("", "", "", "Gettodaydate");
            DateTime gettodate;
            gettodate = objCmnFunctions.convertoDateTime(todatedate);

            maindatatable = dataTable;
            //totalrecord = maindatatable.Rows.Count;
            totalrecord = 0;
            DataTable Errdatatable = new DataTable();
            Errdatatable.Columns.Add("SNo");
            Errdatatable.Columns.Add("Line");
            Errdatatable.Columns.Add("Error Description");
            Boolean ecfamountchk = false;
            Boolean ecfpersnchk = false;
            Boolean ecfdatachk = false;
            try
            {
                decimal delmatamount = 0;
                if (dataTable.Rows.Count > 0)
                {
                    string emperr = "";
                    int distntemp = 0;
                    int empcount = 0;
                    string empstatus = "";
                    string exceltext = "";
                    decimal vales = 0;
                    for (int tr = 0; tr < dataTable.Rows.Count; tr++)
                    {
                        if (dataTable.Rows[tr][0].ToString() != "")
                        {
                            emperr = "";
                            totalrecord = totalrecord + 1;
                            empcount = tr + 1;
                            if (dataTable.Rows[tr][1].ToString() == "")
                            {
                                if (emperr == "")
                                {
                                    emperr = "Employee ID Should Not be Empty";
                                }
                                else
                                {
                                    emperr = emperr + "," + "Employee ID Should Not be Empty";
                                }
                            }
                            else
                            {
                                empstatus = objModel.GetStatusexcel(dataTable.Rows[tr][1].ToString().Trim(), "", "", "EmployeeID");
                                if (empstatus == "notexists")
                                {
                                    if (emperr == "")
                                    {
                                        emperr = "Employee ID Was Not Found ";
                                    }
                                    else
                                    {
                                        emperr = emperr + "," + "Employee ID Was Not Found ";
                                    }
                                }
                                else
                                {
                                    //string curempbrach = objModel.selectempbrabchcode(dataTable.Rows[tr][1].ToString().Trim());
                                    //if (curempbrach != loginbranch)
                                    //{
                                    //    if (emperr == "")
                                    //    {
                                    //        emperr = "This employee [" + dataTable.Rows[tr][1].ToString().Trim() + "] not in a raiser branch ";
                                    //    }
                                    //    else
                                    //    {
                                    //        emperr = emperr + "," + "This employee [" + dataTable.Rows[tr][1].ToString().Trim() + "] not in a raiser branch ";
                                    //    }
                                    //}

                                    //pandiaraj 26-04-18
                                    DataTable getemp = new DataTable();
                                    getemp = objModel.selectemmpdetailforlocal(dataTable.Rows[tr][1].ToString().Trim());
                                    if (getemp.Rows.Count > 0)
                                    {
                                        getemp.Rows[0]["employee_branch_code"].ToString();
                                        string curempbrach = getemp.Rows[0]["employee_branch_code"].ToString().Trim();
                                        if (curempbrach != loginbranch)
                                        {
                                            if (emperr == "")
                                            {
                                                emperr = "This employee [" + dataTable.Rows[tr][1].ToString().Trim() + "] not in a raiser branch ";
                                            }
                                            else
                                            {
                                                emperr = emperr + "," + "This employee [" + dataTable.Rows[tr][1].ToString().Trim() + "] not in a raiser branch ";
                                            }
                                        }
                                        string empbakacc = getemp.Rows[0]["employee_era_acc_no"].ToString().Trim();
                                        string empbankcode = getemp.Rows[0]["employee_era_bank_code"].ToString().Trim();
                                        string empbankgid = getemp.Rows[0]["employee_era_bank_gid"].ToString().Trim();

                                        if ((!string.IsNullOrWhiteSpace(empbakacc) && empbakacc == "0") || (!string.IsNullOrWhiteSpace(empbankcode) && empbankcode == "0") || (!string.IsNullOrWhiteSpace(empbankgid) && empbankgid == "0"))
                                        {
                                            if (emperr == "")
                                            {
                                                emperr = "This employee [" + dataTable.Rows[tr][1].ToString().Trim() + "] not a Bank details ";
                                            }
                                            else
                                            {
                                                emperr = emperr + "," + "This employee [" + dataTable.Rows[tr][1].ToString().Trim() + "] not a Bank details ";
                                            }
                                        }

                                    }
                                    //pandiaraj 26-04-18

                                    if (empchck.Count == 0)
                                    {
                                        empchck.Add(distntemp, dataTable.Rows[tr][1].ToString().Trim());
                                    }
                                    else
                                    {
                                        if (!empchck.ContainsValue(dataTable.Rows[tr][1].ToString().Trim()))
                                        {
                                            distntemp++;
                                            empchck.Add(distntemp, dataTable.Rows[tr][1].ToString().Trim());
                                        }
                                    }
                                }
                            }
                            if (dataTable.Rows[tr][2].ToString() == "")
                            {
                                if (emperr == "")
                                {
                                    emperr = "Employee Name Should Not be Empty";
                                }
                                else
                                {
                                    emperr = emperr + "," + "Employee Name Should Not be Empty";
                                }
                            }
                            if (dataTable.Rows[tr][3].ToString() == "")
                            {
                                if (emperr == "")
                                {
                                    emperr = "To Date Should Not be Empty";
                                }
                                else
                                {
                                    emperr = emperr + "," + "To Date Should Not be Empty";
                                }
                            }
                            else
                            {
                                exceltext = dataTable.Rows[tr][3].ToString().Trim();

                                string[] splitsdate = exceltext.Split('-');
                                string[] splitsdatesec = exceltext.Split('/');
                                if (splitsdate.Length > 1)
                                {
                                    exceltext = exceltext.ToString();
                                }
                                else if (splitsdatesec.Length > 1)
                                {
                                    exceltext = exceltext.ToString();
                                }
                                else
                                {
                                    double d = double.Parse(exceltext);
                                    DateTime conv = DateTime.FromOADate(d);
                                    exceltext = conv.ToString();
                                }

                                DateTime fdate = objCmnFunctions.convertoDateTime(exceltext);

                                if (fdate >= gettodate)
                                {
                                    if (emperr == "")
                                    {
                                        emperr = " Date Should Not be Greater Then Current Date";
                                    }
                                    else
                                    {
                                        emperr = emperr + "," + " Date Should Not be Greater Then Current Date";
                                    }
                                }
                                DateTime ecfdate = objCmnFunctions.convertoDateTime(Session["EcfDatemainmonth"].ToString());
                                if (fdate.Month != ecfdate.Month || fdate.Year != ecfdate.Year)
                                {
                                    if (emperr == "")
                                    {
                                        emperr = " Date Should be ECF Claim Month Date";
                                    }
                                    else
                                    {
                                        emperr = emperr + "," + " Date Should be ECF Claim Month Date";
                                    }
                                }

                            }
                            if (dataTable.Rows[tr][4].ToString() == "")
                            {
                                if (emperr == "")
                                {
                                    emperr = "Place From Should Not be Empty";
                                }
                                else
                                {
                                    emperr = emperr + "," + "Place From Should Not be Empty";
                                }
                            }
                            if (dataTable.Rows[tr][5].ToString() == "")
                            {
                                if (emperr == "")
                                {
                                    emperr = "Place To Should Not be Empty";
                                }
                                else
                                {
                                    emperr = emperr + "," + "Place To Should Not be Empty";
                                }
                            }
                            if (dataTable.Rows[tr][6].ToString() == "")
                            {
                                if (emperr == "")
                                {
                                    emperr = "Travel Mode Should Not be Empty";
                                }
                                else
                                {
                                    emperr = emperr + "," + "Travel Mode Should Not be Empty";
                                }
                            }
                            else
                            {
                                exceltext = dataTable.Rows[tr][6].ToString();

                                empstatus = objModel.GetStatusexcel(exceltext.Trim(), "", "", "TravelMode");
                                if (empstatus == "notexists")
                                {
                                    if (emperr == "")
                                    {
                                        emperr = "Travel Mode Was Not Found ";
                                    }
                                    else
                                    {
                                        emperr = emperr + "," + "Travel Mode Was Not Found ";
                                    }
                                }
                            }
                            if (dataTable.Rows[tr][7].ToString() == "")
                            {
                                if (emperr == "")
                                {
                                    emperr = "Distance Should Not be Empty";
                                }
                                else
                                {
                                    emperr = emperr + "," + "Distance Should Not be Empty";
                                }
                            }
                            else
                            {
                                exceltext = dataTable.Rows[tr][7].ToString();

                                vales = Convert.ToDecimal(exceltext.ToString().Trim().Trim());
                                if (vales <= 0)
                                {
                                    if (emperr == "")
                                    {
                                        emperr = "Distance Should Not be Less Then Zero";
                                    }
                                    else
                                    {
                                        emperr = emperr + "," + "Distance Should Not be Less Then Zero";
                                    }
                                }

                            }
                            if (dataTable.Rows[tr][8].ToString() == "")
                            {
                                if (emperr == "")
                                {
                                    emperr = "Rate Should Not be Empty";
                                }
                                else
                                {
                                    emperr = emperr + "," + "Rate Should Not be Empty";
                                }
                            }
                            else
                            {
                                exceltext = dataTable.Rows[tr][8].ToString();

                                vales = Convert.ToDecimal(exceltext.ToString().Trim());
                                if (vales <= 0)
                                {
                                    if (emperr == "")
                                    {
                                        emperr = "Rate Should Not be Less Then Zero";
                                    }
                                    else
                                    {
                                        emperr = emperr + "," + "Rate Should Not be Less Then Zero";
                                    }
                                }

                                string[] numbers = exceltext.ToString().Trim().Split('.');

                                if (Convert.ToInt32(numbers[0].Length) > 6)
                                {
                                    emperr = emperr + "," + "Rate Should Not be Greater Then 8 digits";
                                }
                                else if (Convert.ToInt32(numbers[0].Length) > 2)
                                {
                                    emperr = emperr + "," + "Rate Should Not be Greater then  2 Digits after Dot";
                                }


                                //else if (Convert.ToString(vales).Length > 8)
                                //{
                                //    if (emperr == "")
                                //    {
                                //        emperr = "Rate Should Not be Greater Then 8 digits";
                                //    }
                                //    else
                                //    {
                                //        emperr = emperr + "," + "Rate Should Not be Greater Then 8 digits";
                                //    }

                                //}



                            }


                            if (dataTable.Rows[tr][9].ToString() == "")
                            {
                                if (emperr == "")
                                {
                                    emperr = "Amount Should Not be Empty";
                                }
                                else
                                {
                                    emperr = emperr + "," + "Amount Should Not be Empty";
                                }
                            }
                            else
                            {
                                exceltext = dataTable.Rows[tr][9].ToString();

                                vales = Convert.ToDecimal(exceltext.ToString().Trim());
                                if (vales <= 0)
                                {
                                    if (emperr == "")
                                    {
                                        emperr = "Amount Should Not be Less Then Zero";
                                    }
                                    else
                                    {
                                        emperr = emperr + "," + "Amount Should Not be Less Then Zero";
                                    }
                                }
                                else
                                {
                                    string exceltextdis = dataTable.Rows[tr][7].ToString();
                                    string exceltextrst = dataTable.Rows[tr][8].ToString();
                                    if (exceltextdis != "" && exceltextrst != "")
                                    {
                                        decimal valesdis = Convert.ToDecimal(exceltextdis.ToString().Trim());
                                        decimal valesrat = Convert.ToDecimal(exceltextrst.ToString().Trim());

                                        decimal tolamtrow = valesdis * valesrat;
                                        if (tolamtrow != vales)
                                        {
                                            if (emperr == "")
                                            {
                                                emperr = "Amount Should be Equal To Multiply of Distance and Rate";
                                            }
                                            else
                                            {
                                                emperr = emperr + "," + "Amount Should be Equal To Multiply of Distance and Rate";
                                            }
                                        }
                                    }
                                }
                            }
                            if (emperr == "")
                            {
                                string exceltextdate = dataTable.Rows[tr][3].ToString();

                                string[] splitsdate = exceltextdate.Split('-');
                                string[] splitsdatesec = exceltextdate.Split('/');
                                if (splitsdate.Length > 1)
                                {
                                    exceltextdate = exceltextdate.ToString();
                                }
                                else if (splitsdatesec.Length > 1)
                                {
                                    exceltextdate = exceltextdate.ToString();
                                }
                                else
                                {
                                    double d = double.Parse(exceltextdate);
                                    DateTime conv = DateTime.FromOADate(d);
                                    exceltextdate = conv.ToString();
                                }

                                DateTime fdate = objCmnFunctions.convertoDateTime(exceltextdate);
                                exceltextdate = fdate.ToString("dd/MM/yyyy");
                                string statusnew = objModel.GetStatusexcelduplicate(
                                               "",
                                               "",
                                               "",
                                                dataTable.Rows[tr][1].ToString(),
                                                exceltextdate.ToString(),
                                                exceltextdate.ToString(),
                                               "",
                                               "", "", "", "", "localdateonly"
                                               );
                                if (statusnew == "Exists")
                                {
                                    sno++;
                                    Errdatatable.Rows.Add(sno, empcount, "Duplicate data : data already exits in database");
                                }
                                else
                                {
                                    string statusnewll = objModel.GetStatusexcelduplicate(
                                            "",
                                            "",
                                            "",
                                             dataTable.Rows[tr][1].ToString(),
                                             exceltextdate.ToString(),
                                             exceltextdate.ToString(),
                                            "",
                                            "", "", "", "", "localemplclamll"
                                            );
                                    if (statusnewll == "Exists")
                                    {
                                        sno++;
                                        Errdatatable.Rows.Add(sno, empcount, "Duplicate data : data already exist in ECF Local Conveyance ");
                                    }
                                    else
                                    {
                                        string rests = objModel.GetStatusexcelduplicate(dataTable.Rows[tr][1].ToString(),
                                                                                       dataTable.Rows[tr][4].ToString(),
                                                                                       dataTable.Rows[tr][5].ToString(),
                                                                                       dataTable.Rows[tr][6].ToString(),
                                                                                       exceltextdate.ToString(),
                                                                                       exceltextdate.ToString(),
                                                                                       dataTable.Rows[tr][7].ToString(),
                                                                                       dataTable.Rows[tr][8].ToString(),
                                                                                       dataTable.Rows[tr][9].ToString(),
                                                                                       "", "",
                                                                                       "localdedup"
                                                                                       );
                                        if (rests == "Exists")
                                        {
                                            sno++;
                                            Errdatatable.Rows.Add(sno, empcount, "Duplicate data : data already exist in ECF Local Conveyance");
                                        }
                                    }
                                }
                            }
                            else
                            {
                                sno++;
                                Errdatatable.Rows.Add(sno, empcount, emperr);
                            }
                        }
                        else
                        {
                            sno++;
                            Errdatatable.Rows.Add(sno, empcount, "S.No Should Not Be Empty");
                        }
                    }
                    if (Errdatatable.Rows.Count > 0)
                    {
                        DataTable dupicats = new DataTable();
                        dupicats = dataTable.DefaultView.ToTable(true, "Employee ID", "Employee Name", "Date", "Place From", "Place To", "Travel Mode", "Distance", "Rate", "Amount");
                        if (dupicats.Rows.Count != dataTable.Rows.Count)
                        {
                            ecfdatachk = true;
                            sno++;
                            Errdatatable.Rows.Add(sno, sno, "Duplicated data - file");
                        }
                    }
                    else
                    {
                        DataTable dupicats = new DataTable();
                        dupicats = dataTable.DefaultView.ToTable(true, "Employee ID", "Employee Name", "Date", "Place From", "Place To", "Travel Mode", "Distance", "Rate", "Amount");
                        if (dupicats.Rows.Count != dataTable.Rows.Count)
                        {
                            ecfdatachk = true;
                            sno++;
                            Errdatatable.Rows.Add(sno, sno, "Duplicated data - file");
                        }

                        for (int tr = 0; tr < empchck.Count; tr++)
                        {
                            string emmpid = empchck[tr].ToString();
                            DataTable uniqueemp = new DataTable();
                            dataTable.DefaultView.RowFilter = "Convert([Employee ID], 'System.String') ='" + emmpid + "'";
                            uniqueemp = dataTable.DefaultView.ToTable();

                            string date = "";
                            string amount = "";
                            string frst = "";
                            DateTime firstdate = new DateTime();
                            DateTime scenddate = new DateTime();
                            decimal totalsum = 0;
                            for (int trcol = 0; trcol < uniqueemp.Rows.Count; trcol++)
                            {
                                amount = uniqueemp.Rows[trcol][9].ToString();
                                date = uniqueemp.Rows[trcol][3].ToString();

                                string[] splitsdaten = date.Split('-');
                                string[] splitsdatesecn = date.Split('/');
                                if (splitsdaten.Length > 1)
                                {
                                    date = date.ToString();
                                }
                                else if (splitsdatesecn.Length > 1)
                                {
                                    date = date.ToString();
                                }
                                else
                                {
                                    double dn = double.Parse(date);
                                    DateTime convn = DateTime.FromOADate(dn);
                                    date = convn.ToString();
                                }

                                if (totalsum == 0)
                                {
                                    totalsum = Convert.ToDecimal(amount);
                                }
                                else
                                {
                                    totalsum = totalsum + Convert.ToDecimal(amount);
                                }

                                if (uniqueemp.Rows.Count == 1)
                                {
                                    firstdate = objCmnFunctions.convertoDateTime(date);
                                    scenddate = objCmnFunctions.convertoDateTime(date);
                                }
                                else if (frst.ToString() == "")
                                {
                                    firstdate = objCmnFunctions.convertoDateTime(date);
                                    frst = firstdate.ToString();
                                }
                                else
                                {
                                    scenddate = objCmnFunctions.convertoDateTime(date);

                                    if (firstdate > scenddate)
                                    {
                                        firstdate = scenddate;
                                        scenddate = firstdate;
                                    }
                                    else
                                    {
                                        firstdate = firstdate;
                                        scenddate = scenddate;
                                    }
                                }
                            }

                            DataTable getemp = new DataTable();
                            getemp = objModel.selectemmpdetailforlocal(empchck[tr].ToString().Trim());
                            if (getemp.Rows.Count > 0)
                            {
                                gridmaindatatable.Rows.Add((tr + 1).ToString(),
                                    empchck[tr].ToString(),
                                    objCmnFunctions.convertoDateTimeString(firstdate.ToString()),
                                    objCmnFunctions.convertoDateTimeString(scenddate.ToString()),
                                    totalsum.ToString(),
                                    getemp.Rows[0]["employee_gid"].ToString(),
                                    getemp.Rows[0]["employee_name"].ToString(),
                                    getemp.Rows[0]["employee_branch_gid"].ToString(),
                                    getemp.Rows[0]["employee_branch_code"].ToString(),
                                    getemp.Rows[0]["employee_fc_code"].ToString(),
                                    getemp.Rows[0]["employee_cc_code"].ToString(),
                                    getemp.Rows[0]["employee_ou_code"].ToString(),
                                    getemp.Rows[0]["employee_product_code"].ToString(),
                                    NatureofExpenses,
                                    MainCategory,
                                    SubCategory
                               );
                            }
                        }

                        for (int trcolb = 0; trcolb < gridmaindatatable.Rows.Count; trcolb++)
                        {
                            vales = Convert.ToDecimal(gridmaindatatable.Rows[trcolb]["Amount"].ToString());
                            if (tolamt == 0)
                            {
                                tolamt = vales;
                            }
                            else
                            {
                                tolamt = tolamt + vales;
                            }
                            if (delmatamount == 0)
                            {
                                delmatamount = vales;
                            }
                            else
                            {
                                if (delmatamount < vales)
                                {
                                    delmatamount = vales;
                                }
                            }
                        }
                        if (tolamt != tolamtexcel)
                        {
                            ecfamountchk = true;
                            sno++;
                            Errdatatable.Rows.Add(sno, sno, "ECF Amount Should Be Equal Sum Of Excel Total Amount");
                        }
                        if (Convert.ToInt32(noperson) != empchck.Count)
                        {
                            ecfpersnchk = true;
                            sno++;
                            Errdatatable.Rows.Add(sno, sno, "Please Check No. of Persons : The Expense List Count Should Be Equal Total No. Person");
                        }
                    }
                }
                else
                {
                    Errdatatable.Rows.Add(1, "Please  Select Valid Sheet");
                }
                Session["delmatamount"] = delmatamount.ToString();
                invalid = Errdatatable.Rows.Count;
                if (ecfamountchk == true)
                {
                    invalid = invalid - 1;
                }
                else
                {
                    invalid = Errdatatable.Rows.Count;
                }
                if (ecfpersnchk == true)
                {
                    invalid = invalid - 1;
                }
                else
                {
                    invalid = Errdatatable.Rows.Count;
                }
                if (ecfdatachk == true)
                {
                    invalid = invalid - 1;
                }
                else
                {
                    invalid = Errdatatable.Rows.Count;
                }
                valid = totalrecord - invalid;
                ViewBag.vbvalid = "Total No of Vaild Record :" + valid;
                ViewBag.vbinvalid = "Total No of InVaild Record :" + invalid;
                ViewBag.vbtotalrecord = "Total No of Record Excel File :" + totalrecord;

                Session["Errdatatable"] = Errdatatable;
                Session["maindatatable"] = maindatatable;
                Session["gridmaindatatable"] = gridmaindatatable;
            }
            catch (Exception ex)
            {
                sno++;
                Errdatatable.Rows.Add(sno, Errdatatable.Rows.Count + 1, ex.Message.ToString() + "Please ckeck Excel File Error While Reading Data");

                invalid = Errdatatable.Rows.Count;
                valid = totalrecord - invalid;
                ViewBag.vbvalid = "Total No of Vaild Record :" + valid;
                ViewBag.vbinvalid = "Total No of InVaild Record :" + invalid;
                ViewBag.vbtotalrecord = "Total No of Record Excel File :" + totalrecord;

                Session["Errdatatable"] = Errdatatable;
                Session["maindatatable"] = maindatatable;
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
        }
        [HttpGet]
        public PartialViewResult _LocalAttachmentCreate()
        {
            try
            {
                EOW_File objMAttachment = new EOW_File();
                objMAttachment.AttachmentTypeData = new SelectList(objModel.AttachmentTypedata().ToList(), "AttachmentTypeId", "AttachmentTypeName");
                return PartialView(objMAttachment);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView();
            }

        }
        [HttpPost]
        public JsonResult EmpAttachmentDelete(EOW_File EmployeeeExpensemodel)
        {
            try
            {
                int EmployeeePaymentGID = (int)EmployeeeExpensemodel.AttachmentFilenameId;
                string delamt = objModel.DeleteEmployeeeAttachment(EmployeeePaymentGID, Session["EcfGid"].ToString());
                //Session["Ecfamountpayment"] = Convert.ToInt32(Session["Ecfamountpayment"].ToString()) + Convert.ToInt32(delamt.ToString());
                return Json(EmployeeeExpensemodel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred...", JsonRequestBehavior.AllowGet);
            }

        }
        [HttpGet]
        public PartialViewResult _LocalAttachmentDetails()
        {
            try
            {
                List<EOW_File> lstAttachment = new List<EOW_File>();
                lstAttachment = objModel.GetEmployeeeAttachment(Session["EcfGid"].ToString(), objCmnFunctions.GetLoginUserGid().ToString()).ToList();
                return PartialView(lstAttachment);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return PartialView();
            }
        }
        [HttpPost]
        public JsonResult _LocalAttachmentCreate(EOW_File EmployeeeExpenseModel)
        {
            string img = "";
            try
            {
                if (TempData["_attFile"] != null)
                {
                    HttpPostedFileBase savefile = TempData["_attFile"] as HttpPostedFileBase;
                    string getname = objModel.InsertEmpAtt(savefile, EmployeeeExpenseModel, Session["EcfGid"].ToString(), objCmnFunctions.GetLoginUserGid().ToString());
                    if (getname != "")
                    {
                        //string path = "//192.168.0.203/temp/";
                        //string path = @"C:\temp\";
                        //string path1 = Path.GetFileName(savefile.FileName);
                        //string exten = Path.GetExtension(savefile.FileName);
                        //string[] str = path1.Split('.');
                        ////var index = path1.LastIndexOf(".") + 1;
                        //getname = getname + exten.ToString();
                        //string savedFileName = Path.Combine(path, getname);
                        //savefile.SaveAs(savedFileName);
                        //img = "Yes";

                        //var fileextension = Path.GetExtension(savefile.FileName);
                        //var stream = savefile.InputStream;
                        //var path = Path.Combine(@"C:\temp\", getname + fileextension);
                        //using (var fileStream = System.IO.File.Create(path))
                        //{
                        //    stream.CopyTo(fileStream);
                        //}
                        //Session["empattmtfilee"] = null;
                        //img = "Yes";

                        HttpPostedFileBase _attFile = TempData["_attFile"] as HttpPostedFileBase;
                        var stream = _attFile.InputStream;
                        string uploaderUrl = string.Format("{0}{1}", objModel.HoldFileUploadUrl(), getname + "." + _attFile.FileName.Split('.')[_attFile.FileName.Split('.').Length - 1]);
                        //using (var fileStream = System.IO.File.Create(uploaderUrl))
                        //{
                        //    stream.CopyTo(fileStream);
                        //}
                        //Muthu 2016-10-19
                        byte[] bytFile = null;
                        using (var memoryStream = new MemoryStream())
                        {
                            stream.CopyTo(memoryStream);
                            bytFile = memoryStream.ToArray();
                            ViewBag.Result = "inside File Stream";
                        }
                        var FileString = Convert.ToBase64String(bytFile);
                        var filename = getname + "." + _attFile.FileName.Split('.')[_attFile.FileName.Split('.').Length - 1];
                        var result = Cmnfs.SaveFileToServer(FileString, filename).Result;
                        //remove the temp data content.
                        TempData.Remove("_attFile");
                        img = "Yes";
                    }
                }
                else
                {
                    TempData.Remove("_attFile");
                    img = "Invalid File Format!";
                }
                ViewBag.SearchItems = null;
                return Json(img, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred...", JsonRequestBehavior.AllowGet);
            }
        }
        public FileResult Download(int id)
        {
            string FileName = objModel.downloadAttachment(id, Session["EcfGid"].ToString());
            // //int index = delamt.LastIndexOf(".");
            // //string[] seqNum = new string[] { delamt.Substring(0, index), delamt.Substring(index + 1) };
            // //string directory = @"C:\temp\";
            // //directory = directory + id.ToString() + "." + seqNum[1].ToString();
            // //byte[] fileBytes = System.IO.File.ReadAllBytes(directory);
            // //string fileName = seqNum[0].ToString() + "." + seqNum[1].ToString();
            // //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            string[] str = FileName.Split('.');
            string directory = id.ToString() + "." + str[str.Length - 1].ToString();
            // string directory = objModel.HoldFileUploadUrl() + id.ToString() + "." + str[str.Length - 1].ToString();
            // byte[] fileBytes = System.IO.File.ReadAllBytes(directory);
            var apiresult = Cmnfs.DownloadFile(directory).Result;
            if (apiresult == "Fail")
            {
                return File("", "");
            }
            else
            {
                byte[] filebyte = Convert.FromBase64String(apiresult);
                return File(filebyte, System.Net.Mime.MediaTypeNames.Application.Octet, FileName);
            }
            //  return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, FileName);
        }
        [HttpPost]
        public void UploadFilesatt(string attach = null, string attaching_format = null)
        {
            try
            {
                TempData["_attFile"] = null;
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        string fileextension = Path.GetExtension(Path.GetFileName(fileContent.FileName));
                        string[] attaching_fileformat = attaching_format.Split(',');
                        if (attaching_fileformat.Contains(fileextension.ToLower()))
                        {
                            var stream = fileContent.InputStream;
                            byte[] bytFile = null;
                            using (var memoryStream = new MemoryStream())
                            {
                                stream.CopyTo(memoryStream);
                                bytFile = memoryStream.ToArray();
                            }
                            bool isEXE = CmnFunctions.GetMimeTypeFromAttachment(bytFile, attach, fileextension.ToLower());
                            if (isEXE == false)
                            {
                                HttpPostedFileBase hpfBase = Request.Files[file] as HttpPostedFileBase;
                                TempData["_attFile"] = hpfBase;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
        }
        [HttpPost]
        public JsonResult _EmpECFCreate(EOW_EmployeeeExpense EmployeeeExpenseModel)
        {
            string Err = "";
            try
            {
                if (ModelState.IsValid)
                {
                    decimal ecfmain = Convert.ToDecimal(EmployeeeExpenseModel.Exp_Amount.ToString());
                    decimal ecfexcel = Convert.ToDecimal(Session["EcfamountExcelamt"].ToString());

                    var result = objModel.GetEmployeePayModeEraAcc(Convert.ToInt32(Session["SelfModeRaiseid"]));
                    if (ecfmain != ecfexcel)
                    {
                        Err = "Expense";
                    }
                    else if (result == "No")
                    {
                        Err = "BankAcc";
                    }
                    else
                    {
                        //string demlat = objModel.Getraiserdelmat(Session["EcfGid"].ToString(), Session["SelfModeRaiseid"].ToString());
                        //if (demlat == "Yes")
                        //{
                        Err = objModel.Insertecf(EmployeeeExpenseModel, Session["EcfGid"].ToString(), Session["delmatamount"].ToString(), objCmnFunctions.GetLoginUserGid().ToString(), objCmnFunctions.GetLoginUserGid().ToString(), "L", Session["QueueGide"].ToString());
                        if (Err == "")
                        {
                            Err = "Error";
                        }
                        else
                        {
                            Err = Err.ToString();
                            Session["EcfGid"] = null;
                            Session["EcfamountExcelamt"] = null;
                            Session["Ecfamountlocal"] = null;
                            Session["QueueGide"] = null;
                        }
                        //}
                        //else
                        //{
                        //    Err = "Delmat";
                        //}
                    }
                }
                ViewBag.SearchItems = null;
                return Json(Err, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred...", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult _EmpECFDraft(EOW_EmployeeeExpense EmployeeeExpenseModel)
        {
            string Err = "";
            try
            {
                if (ModelState.IsValid)
                {
                    Err = objModel.Insertecfdraft(EmployeeeExpenseModel, Session["EcfGid"].ToString());
                    if (Err == "")
                    {
                        Err = "Error";
                    }
                    else
                    {
                        Err = "Success";
                        Session["EcfGid"] = null;
                        Session["EcfamountExcelamt"] = null;
                        Session["Ecfamountlocal"] = null;
                        Session["QueueGide"] = null;
                    }
                }
                ViewBag.SearchItems = null;
                return Json(Err, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred...", JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public PartialViewResult _LocalExpense()
        {
            List<EOW_TravelClaim> lst = new List<EOW_TravelClaim>();
            //DataTable err1 = new DataTable();
            //err1 = (DataTable)Session["maindatatable"];
            //lst = objModel.GetTravelModedatalocal(err1, Session["EcfGid"].ToString(), Session["EcfDatemain"].ToString()).ToList();
            return PartialView(lst);
        }
        ///Ramya LC PayMode
        [HttpPost]
        public JsonResult GetLocalPayment(string EcfId)
        {
            EcfId = Session["EcfGid"].ToString();
            try
            {
                string Data1 = "";
                DataSet ds = db.GetLocalPayment(EcfId.ToString(), "L");

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                //dt = ds.Tables[1];
                //if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }

        }
        [HttpGet]
        public PartialViewResult _LocalPayment()
        {
            List<EOW_Payment> lstPayment = new List<EOW_Payment>();
            //DataTable err1 = new DataTable();
            //err1 = (DataTable)Session["maindatatable"];
            lstPayment = objModel.GetLocalPayment(Session["EcfGid"].ToString(), "L").ToList();
            return PartialView(lstPayment);
        }
        [HttpGet]
        public PartialViewResult _ExpensedetailsView(int id, string viewfor)
        {
            Session["Ecfcurrentid"] = id.ToString();
            return PartialView();
        }
        //Local Conveyance - Edit - Ramya
        [HttpPost]
        public JsonResult GetEditLCPayMode()
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetMaster("54", "1");
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    DataRow row = dt.NewRow();
                    dt.Rows.InsertAt(row, 0);
                    dt.Rows[0][0] = "0";
                    dt.Rows[0][1] = "-- Select One --";
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                }
                return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        [HttpPost]
        public JsonResult SetEditECFCreditLineDetails(InvoiceCreditLine iCredit)
        {
            try
            {
                string Data1 = "", Data2 = "";

                DataSet ds = db.SetEditECFCreditLineDetails(iCredit.Ecfid, iCredit.InvId, iCredit.CreditlineGId, iCredit.RefId, iCredit.paymode, iCredit.RefNo,
                    iCredit.Beneficiary, iCredit.Desc, iCredit.Amount, iCredit.BankId, iCredit.IsRemoved, iCredit.IfscCode, plib.LoginUserId);

                dt = ds.Tables[0];
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                dt = ds.Tables[1];
                if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }


        public string getconverttomonthtodateto(string months)
        {
            DateTime convrtdate = new DateTime();
            convrtdate = objCmnFunctions.convertoDateTime(months);
            string monthyear = "";
            string year = "";
            string month = "";

            year = convrtdate.Year.ToString();
            month = convrtdate.Month.ToString();

            if (month.ToString() != "0")
            {
                if (month.ToString() == "1")
                {
                    monthyear = "January-" + year;
                    return monthyear;
                }
                else if (month.ToString() == "2")
                {
                    monthyear = "February-" + year;
                    return monthyear;
                }
                else if (month.ToString() == "3")
                {
                    monthyear = "March-" + year;
                    return monthyear;
                }
                else if (month.ToString() == "4")
                {
                    monthyear = "April-" + year;
                    return monthyear;
                }
                else if (month.ToString() == "5")
                {
                    monthyear = "May-" + year;
                    return monthyear;
                }
                else if (month.ToString() == "6")
                {
                    monthyear = "June-" + year;
                    return monthyear;
                }
                else if (month.ToString() == "7")
                {
                    monthyear = "July-" + year;
                    return monthyear;
                }
                else if (month.ToString() == "8")
                {
                    monthyear = "August-" + year;
                    return monthyear;
                }
                else if (month.ToString() == "9")
                {
                    monthyear = "September-" + year;
                    return monthyear;
                }
                else if (month.ToString() == "10")
                {
                    monthyear = "October-" + year;
                    return monthyear;
                }
                else if (month.ToString() == "11")
                {
                    monthyear = "November-" + year;
                    return monthyear;
                }
                else if (month.ToString() == "12")
                {
                    monthyear = "December-" + year;
                    return monthyear;
                }
                else
                {
                    monthyear = "January-" + year;
                    return monthyear;
                }
            }
            return monthyear;
        }
        public string getconverttomonthtodate(string month)
        {
            string date = "";

            string[] str;
            str = month.Split('-');
            if (str.Length > 1)
            {
                if (str[0].ToString() == "January")
                {
                    date = "01-01-" + str[1].ToString();
                }
                else if (str[0].ToString() == "February")
                {
                    date = "01-02-" + str[1].ToString();
                }
                else if (str[0].ToString() == "March")
                {
                    date = "01-03-" + str[1].ToString();
                }
                else if (str[0].ToString() == "April")
                {
                    date = "01-04-" + str[1].ToString();
                }
                else if (str[0].ToString() == "May")
                {
                    date = "01-05-" + str[1].ToString();
                }
                else if (str[0].ToString() == "June")
                {
                    date = "01-06-" + str[1].ToString();
                }
                else if (str[0].ToString() == "July")
                {
                    date = "01-07-" + str[1].ToString();
                }
                else if (str[0].ToString() == "August")
                {
                    date = "01-08-" + str[1].ToString();
                }
                else if (str[0].ToString() == "September")
                {
                    date = "01-09-" + str[1].ToString();
                }
                else if (str[0].ToString() == "October")
                {
                    date = "01-10-" + str[1].ToString();
                }
                else if (str[0].ToString() == "November")
                {
                    date = "01-11-" + str[1].ToString();
                }
                else if (str[0].ToString() == "December")
                {
                    date = "01-12-" + str[1].ToString();
                }
                else
                {
                    date = "01-01-" + str[1].ToString();
                }
            }
            return date;
        }

        [HttpGet]
        public FileResult DownloadLCFTemplate()
        {
            //get the temp folder and file path in server
            string fullPath = System.Configuration.ConfigurationManager.AppSettings["LCF"];

            //return the file for download, this is an Excel 
            //so I set the file content type to "application/vnd.ms-excel"
            return File(fullPath, "application/vnd.ms-excel", "LCFTemplate.xls");
        }
    }
}
