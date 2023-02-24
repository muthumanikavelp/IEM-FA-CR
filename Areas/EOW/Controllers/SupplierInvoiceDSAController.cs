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
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using System.Web.Hosting;
using System.Configuration;
using IEM.App_Start;
namespace IEM.Areas.EOW.Controllers
{

    [NoDirectAccess]
    public class SupplierInvoiceDSAController : Controller
    {
        //
        // GET: /EOW/SupplierInvoiceDSA/
        ErrorLog objErrorLog = new ErrorLog();
        private EOW_IRepository objModel;
        CmnFunctions objCmnFunctions = new CmnFunctions();
        private FileServer Cmnfs = new FileServer();
        CommonIUD objCommonIUD = new CommonIUD();
        LocalConveyanceNewController locals = new LocalConveyanceNewController();
        public SupplierInvoiceDSAController()
            : this(new EOW_DataModel())
        {

        }
        public SupplierInvoiceDSAController(EOW_IRepository objM)
        {
            objModel = objM;
        }
        public ActionResult Index()
        {
            try
            {
                Session["QueueGide"] = "";
                Session["invoiceclear"] = "clear";
                Session["err"] = "";
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
                    Session["EcfDatemain"] = eowemp.ECF_Date;
                    Session["EcfDatemainmonth"] = locals.getconverttomonthtodateto(eowemp.ClaimMonth);
                    eowemp.ClaimMonth = locals.getconverttomonthtodateto(eowemp.ClaimMonth);
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
                Session["Ecfdeclaratonnote"] = objModel.GetDecnote("5", "S").ToString();
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
                Session["EcfamountExcelamt"] = obj.ECF_Amount;
                Session["EcfDatemain"] = obj.ECF_Date;
                Session["EcfDatemainmonth"] = locals.getconverttomonthtodate(obj.ClaimMonth);

                obj.Raiser_Modedata = new SelectList(objModel.GetRaiserMode().ToList(), "raisermodeId", "raisermodeName", obj.raisermodeId);
                if (obj.raisermodeId == "P")
                {
                    Session["SelfModeRaiseid"] = objCmnFunctions.GetLoginProxyUserGid().ToString();
                }
                else
                {
                    Session["SelfModeRaiseid"] = objCmnFunctions.GetLoginUserGid().ToString();
                }

                obj.ClaimMonth = locals.getconverttomonthtodate(obj.ClaimMonth);
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
                            if (Session["err"].ToString().Equals("flag") || Session["invoiceclear"].ToString() != "clear")
                            {
                                ViewBag.processdatasec = "first";
                                Session["err"] = "";
                            }
                        }
                    }
                    else
                    {
                        message = objModel.InsertEmployeeeBasic(obj, objCmnFunctions.GetLoginUserGid().ToString(), "D");
                        if (message != "")
                        {
                            ViewBag.processdata = "first";
                            ViewBag.process = "second";
                            Session["EcfGid"] = objModel.selectecfgidBasic(obj, objCmnFunctions.GetLoginUserGid().ToString());
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
                        TempData["_attFileuploaddsa"] = hpf;
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
            string Extensionnew = "";
            string Extension1 = "";
            string error = "";
            try
            {
                string path1 = "";
                if (TempData["_attFileuploaddsa"] != null)
                {
                    HttpPostedFileBase savefile = TempData["_attFileuploaddsa"] as HttpPostedFileBase;
                    string Extension = Path.GetFileName(savefile.FileName);
                    string n = string.Format("Local-{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
                    Extension1 = System.IO.Path.GetExtension(savefile.FileName);
                    //path1 = objModel.HoldFileUploadUrl() + n + Extension;
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
                    Session["Tempdatasetlocaldsa"] = result1;
                    TempData.Remove("_attFileuploaddsa");
                }
                return Json(objparent.ToList(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred...", JsonRequestBehavior.AllowGet);
            }
        }


        public List<SheetData> GetSheetData(string excelConnectionString)
        {
            List<SheetData> objparent = new List<SheetData>();
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

        public JsonResult _Suppliserstatuserr(EOW_TravelClaim obj)
        {
            string mag = "";
            string strCols = "";
            string[] strColArr;
            try
            {
                Session["EcfDatemainmonth"] = locals.getconverttomonthtodate(obj.ClaimMonth);
                Session["Tempdatatableamt"] = "0";
                Session["Tempdatatables"] = null;
                Session["Errdatatable"] = null;
                Session["maindissupplier"] = null;
                Session["maindissupplier"] = null;
                var dataTable = new DataTable();
                DataSet result1 = new DataSet();
                result1 = (DataSet)Session["Tempdatasetlocaldsa"];
                dataTable = result1.Tables[obj.Branch.ToString()];
                Session["Tempdatatables"] = dataTable;

                //string excelname = obj.Branch.ToString().Trim() + "$";
                //string excelConnectionString = Session["excelfilepathlocal"].ToString();
                //using (OleDbConnection con = new OleDbConnection(excelConnectionString))
                //{
                //    string query = string.Format("SELECT * FROM [{0}]", excelname);
                //    con.Open();
                //    OleDbDataAdapter adapter = new OleDbDataAdapter(query, con);
                //    adapter.Fill(dataTable);
                //    Session["Tempdatatables"] = dataTable;
                //}
                foreach (DataColumn dtColumn in dataTable.Columns)
                {
                    strCols = strCols + dtColumn.ColumnName.Trim() + ":";
                }
                strCols = strCols.Substring(0, strCols.Length - 1);
                strColArr = strCols.Split(':');
                if (strColArr[0].ToString() == "SNo"
                    && strColArr[1].ToString() == "Supplier Code"
                    && strColArr[2].ToString() == "Supplier Name"
                    && strColArr[3].ToString() == "Invoice Date"                    
                    && strColArr[4].ToString() == "Description"
                    && strColArr[5].ToString() == "Amount"
                    && strColArr[6].ToString() == "Provision"
                    && strColArr[7].ToString() == "Nature of Expenses"
                    && strColArr[8].ToString() == "Main Category"
                    && strColArr[9].ToString() == "Sub Category"
                    && strColArr[10].ToString() == "Function Code"
                    && strColArr[11].ToString() == "Cost Code"
                    && strColArr[12].ToString() == "Product Code"
                    && strColArr[13].ToString() == "OU Code"
                     /*&& strColArr[14].ToString() == "Invoice No"
                      && strColArr[15].ToString() == "Provider Location"
                      && strColArr[16].ToString() == "Receiver Location"
                      && strColArr[17].ToString() == "GST Y/N"
                      && strColArr[18].ToString() == "HSN Code"
                      && strColArr[19].ToString() == "RCM Y/N"*/
                     && strColArr[14].ToString() == "Provider Location"
                      && strColArr[15].ToString() == "Receiver Location"
                      && strColArr[16].ToString() == "GST Y/N"
                      && strColArr[17].ToString() == "HSN Code"
                      //&& strColArr[18].ToString() == "RCM Y/N"
                    && strColArr.Count().ToString() == "18")
                {
                    Session["Tempdatatableamt"] = obj.Amount;
                    mag = "Success";
                }
                else
                {
                    if (strColArr.Count().ToString() == "18")
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
        public PartialViewResult _Suppliserstatus()
        {
            List<EOW_TravelClaim> objparent1 = new List<EOW_TravelClaim>();
            try
            {
                if (Session["Tempdatatables"] != null)
                {
                    DataTable errdataval = new DataTable();
                    errdataval = (DataTable)Session["Tempdatatables"];
                    string ecfamount = Session["Tempdatatableamt"].ToString();
                    datasupload(errdataval, ecfamount);
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
                return PartialView();
            }
            return PartialView();
        }
        private void datasupload(DataTable dataTable, string ecfamount)
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
            Hashtable empchck = new Hashtable();
            int hashcont = 0;
            Hashtable empchckou = new Hashtable();
            int hashcontou = 0;
            string todatedate = objModel.GetStatusexcel("", "", "", "Gettodaydate");
            DateTime gettodate;
            gettodate = objCmnFunctions.convertoDateTime(todatedate);

            DataTable maindatatable = new DataTable();
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
                    string status = "";
                    string errs = "";
                    int RowNo = 0;
                    decimal vales = 0;
                    dataTable.Columns.Add("InvoiceDatenew", typeof(System.DateTime));

                    for (int i = 0; i < dataTable.Rows.Count; i++)
                    {
                        if (dataTable.Rows[i][0].ToString() != "")
                        {
                            errs = "";
                            RowNo = i + 1;
                            if (dataTable.Rows[i][1].ToString() == "")
                            {
                                errs = "Supplier Code Should Not be Empty ";
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][1].ToString();

                                //status = objModel.GetStatusexcel(exceltext.Trim(), "", "", "supplieractive");
                                status = objModel.GetStatusexcel(exceltext.Trim(), "", "", "supplieractiveisadhoc");//pandiaraj 26-04-18
                                if (status == "notexists")
                                {
                                    if (errs == "")
                                    {
                                        errs = exceltext + " Supplier Code Was Not Found ";
                                    }
                                    else
                                    {
                                        errs = errs + "," + exceltext + " Supplier Code Was Not Found ";
                                    }
                                }
                                else
                                {
                                    if (empchck.Count == 0)
                                    {
                                        empchck.Add(hashcont, dataTable.Rows[i][1].ToString());
                                        //empidchk.Rows.Add(RowNo, dataTable.Rows[i][4].ToString());
                                    }
                                    else
                                    {
                                        if (empchck.ContainsValue(dataTable.Rows[i][1].ToString()))
                                        {
                                            //var key = empchck.Keys.OfType<String>().FirstOrDefault(s => empchck[s] == dataTable.Rows[i][4].ToString());
                                            //if (errs == "")
                                            //{
                                            //    errs = "Supplier Code Already Exits ";
                                            //}
                                            //else
                                            //{
                                            //    errs = errs + "," + "Supplier Code Already Exits ";
                                            //}
                                        }
                                        else
                                        {
                                            hashcont++;
                                            empchck.Add(hashcont, dataTable.Rows[i][1].ToString());
                                        }

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
                                    errs = "Invoice Date Should Not be Empty";
                                }
                                else
                                {
                                    errs = errs + "," + "Invoice Date Should Not be Empty";
                                }
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][3].ToString();

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

                                dataTable.Rows[i]["InvoiceDatenew"] = fdate;

                                if (fdate >= gettodate)
                                {
                                    if (errs == "")
                                    {
                                        errs = "Invoice Date Should Not be Greather Then Current Date";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "Invoice Date Should Not be Greather Then Current Date";
                                    }
                                }

                                DateTime ecfdate = objCmnFunctions.convertoDateTime(Session["EcfDatemainmonth"].ToString());
                                if (fdate.Month != ecfdate.Month || fdate.Year != ecfdate.Year)
                                {
                                    if (errs == "")
                                    {
                                        errs = "Invoice Date Should be ECF Claim Month Date";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "Invoice Date Should be ECF Claim Month Date";
                                    }
                                }
                            }
                            if (dataTable.Rows[i][7].ToString() == "")
                            {
                                errs = "Nature of Expenses Should Not be Empty ";
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][7].ToString();

                                status = objModel.GetStatusexcel(exceltext.Trim(), "", "", "NatureofExpenses");
                                if (status == "notexists")
                                {
                                    if (errs == "")
                                    {
                                        errs = "Nature of Expenses Was Not Found ";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "Nature of Expenses Was Not Found ";
                                    }
                                }
                                else
                                {
                                    if (dataTable.Rows[i][8].ToString() == "")
                                    {

                                        if (errs == "")
                                        {
                                            errs = "Main Category Should Not be Empty";
                                        }
                                        else
                                        {
                                            errs = errs + "," + "Main Category Should Not be Empty";
                                        }
                                    }
                                    else
                                    {
                                        exceltext = dataTable.Rows[i][8].ToString();
                                        string exceltextnar = dataTable.Rows[i][7].ToString();

                                        status = objModel.GetStatusexcel(exceltext.Trim(), exceltextnar.Trim(), "", "MainCategorycodenew");
                                        if (status == "notexists")
                                        {
                                            if (errs == "")
                                            {
                                                errs = "Main Category Was Not Found ";
                                            }
                                            else
                                            {
                                                errs = errs + "," + "Main Category Was Not Found ";
                                            }
                                        }
                                        else
                                        {
                                            if (dataTable.Rows[i][9].ToString() == "")
                                            {
                                                if (errs == "")
                                                {
                                                    errs = "Sub Category Should Not be Empty";
                                                }
                                                else
                                                {
                                                    errs = errs + "," + "Sub Category Should Not be Empty";
                                                }
                                            }
                                            else
                                            {
                                                exceltext = dataTable.Rows[i][9].ToString();
                                                string exceltextcat = dataTable.Rows[i][8].ToString();

                                                status = objModel.GetStatusexcel(exceltext.Trim(), exceltextcat.Trim(), "", "SubCategorycodenew");
                                                if (status == "notexists")
                                                {
                                                    if (errs == "")
                                                    {
                                                        errs = "Sub Category Was Not Found ";
                                                    }
                                                    else
                                                    {
                                                        errs = errs + "," + "Sub Category Was Not Found ";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (dataTable.Rows[i][5].ToString() == "")
                            {
                                if (errs == "")
                                {
                                    errs = "Amount Should Not be Empty";
                                }
                                else
                                {
                                    errs = errs + "," + "Amount Should Not be Empty";
                                }
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][5].ToString();

                                vales = Convert.ToDecimal(exceltext.ToString().Trim());
                                if (vales <= 0)
                                {
                                    if (errs == "")
                                    {
                                        errs = "Amount Should Not be Less Then Zero";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "Amount Should Not be Less Then Zero";
                                    }
                                }
                                else
                                {
                                    if (tolamt == 0)
                                    {
                                        tolamt = vales;
                                    }
                                    else
                                    {
                                        tolamt = tolamt + vales;
                                    }
                                }
                            }
                            if (dataTable.Rows[i][10].ToString() == "")
                            {
                                if (errs == "")
                                {
                                    errs = "Function Code Should Not be Empty";
                                }
                                else
                                {
                                    errs = errs + "," + "Function Code Should Not be Empty";
                                }
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][10].ToString();

                                status = objModel.GetStatusexcel(exceltext.Trim(), "", "", "FunctionCode");
                                if (status == "notexists")
                                {
                                    if (errs == "")
                                    {
                                        errs = "Function Code Was Not Found ";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "Function Code Was Not Found ";
                                    }
                                }

                            }
                            if (dataTable.Rows[i][11].ToString() == "")
                            {
                                if (errs == "")
                                {
                                    errs = "Cost Code Should Not be Empty";
                                }
                                else
                                {
                                    errs = errs + "," + "Cost Code Should Not be Empty";
                                }
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][11].ToString();

                                status = objModel.GetStatusexcel(exceltext.Trim(), "", "", "CostCode");
                                if (status == "notexists")
                                {
                                    if (errs == "")
                                    {
                                        errs = "Cost Code Was Not Found ";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "Cost Code Was Not Found ";
                                    }
                                }
                            }
                            if (dataTable.Rows[i][12].ToString() == "")
                            {
                                if (errs == "")
                                {
                                    errs = "Product Code Should Not be Empty";
                                }
                                else
                                {
                                    errs = errs + "," + "Product Code Should Not be Empty";
                                }
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][12].ToString();

                                status = objModel.GetStatusexcel(exceltext.Trim(), "", "", "ProductCode");
                                if (status == "notexists")
                                {
                                    if (errs == "")
                                    {
                                        errs = "Product Code Was Not Found ";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "Product Code Was Not Found ";
                                    }
                                }
                            }
                            if (dataTable.Rows[i][13].ToString() == "")
                            {
                                if (errs == "")
                                {
                                    errs = "OU Code Should Not be Empty";
                                }
                                else
                                {
                                    errs = errs + "," + "OU Code Should Not be Empty";
                                }
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][13].ToString();

                                status = objModel.GetStatusexcel(exceltext.Trim(), "", "", "OUCode");
                                if (status == "notexists")
                                {
                                    if (errs == "")
                                    {
                                        errs = "OU Code Was Not Found ";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "OU Code Was Not Found ";
                                    }
                                }
                                else
                                {
                                    if (empchckou.Count == 0)
                                    {
                                        empchckou.Add(hashcontou, dataTable.Rows[i][13].ToString());
                                        //empidchk.Rows.Add(RowNo, dataTable.Rows[i][4].ToString());
                                    }
                                    else
                                    {
                                        if (empchckou.ContainsValue(dataTable.Rows[i][13].ToString()))
                                        {
                                            //var key = empchck.Keys.OfType<String>().FirstOrDefault(s => empchck[s] == dataTable.Rows[i][4].ToString());
                                            //if (errs == "")
                                            //{
                                            //    errs = "Supplier Code Already Exits ";
                                            //}
                                            //else
                                            //{
                                            //    errs = errs + "," + "Supplier Code Already Exits ";
                                            //}
                                        }
                                        else
                                        {
                                            hashcontou++;
                                            empchckou.Add(hashcontou, dataTable.Rows[i][13].ToString());
                                        }

                                    }
                                }
                            }
                            //Ramya Added DSA with GST & RCM Starts
                            //Invoice No.
                           /* if (dataTable.Rows[i][14].ToString() == "" && dataTable.Rows[i][17].ToString() == "Y")
                            {
                                if (errs == "")
                                {
                                    errs = "Invoice Number Should Not be Empty";
                                }
                                else
                                {
                                    errs = errs + "," + "Invoice Number Should Not be Empty";
                                }
                            }                            
                            else
                            {
                                exceltext = dataTable.Rows[i][14].ToString();

                                status = objModel.GetStatusexcel(exceltext.Trim(), "", "", "DuplicateInvoice");
                                if (status == "Exists")
                                {
                                    if (errs == "")
                                    {
                                        errs = "Duplicate Invoice No Found ";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "Duplicate Invoice No Found ";
                                    }
                                }
                            }*/

                            if (dataTable.Rows[i][14].ToString() == "")
                            {
                                if (errs == "")
                                {
                                    errs = "Provider Location Should Not be Empty";
                                }
                                else
                                {
                                    errs = errs + "," + "Provider Location Should Not be Empty";
                                }
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][14].ToString();
                                string Suppliercode = dataTable.Rows[i][1].ToString();

                                status = objModel.GetStatusexcel(exceltext.Trim(), Suppliercode.Trim(), "", "ProviderLocation");
                                if (status == "notexists")
                                {
                                    if (errs == "")
                                    {
                                        errs = "Invalid Provider Location! ";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "Invalid Provider Location! ";
                                    }
                                }
                            }
                            if (dataTable.Rows[i][15].ToString() == "")
                            {
                                if (errs == "")
                                {
                                    errs = "Receiver Location Should Not be Empty";
                                }
                                else
                                {
                                    errs = errs + "," + "Receiver Location Should Not be Empty";
                                }
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][15].ToString();
                                string OUCode = dataTable.Rows[i][13].ToString();
                                status = objModel.GetStatusexcel(exceltext.Trim(), OUCode.Trim(), "", "ReceiverLocation");
                                if (status == "notexists")
                                {
                                    if (errs == "")
                                    {
                                        errs = "Invalid Receiver Location! ";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "Invalid Receiver Location! ";
                                    }
                                }
                            }

                            if (dataTable.Rows[i][16].ToString() == "")
                            {
                                if (errs == "")
                                {
                                    errs = "GST Y/N should not be empty!";
                                }
                                else
                                {
                                    errs = errs + "," + "GST Y/N should not be empty!";
                                }
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][16].ToString();

                                if (exceltext != "Y" && exceltext != "N")
                                {
                                    if (errs == "")
                                    {
                                        errs = "Invalid GST Flag Value! ";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "Invalid GST Flag Value! ";
                                    }
                                }
                            }

                            if (dataTable.Rows[i][17].ToString() == "")
                            {
                                if (errs == "")
                                {
                                    errs = "HSN Code Should Not be Empty";
                                }
                                else
                                {
                                    errs = errs + "," + "HSN Code Should Not be Empty";
                                }
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][17].ToString();
                                string expcat = dataTable.Rows[i][8].ToString();
                                string expsubcat = dataTable.Rows[i][9].ToString();
                                status = objModel.GetStatusexcel(exceltext.Trim(), expsubcat.Trim(), expcat.Trim(), "HSNCode");
                                if (status == "notexists")
                                {
                                    if (errs == "")
                                    {
                                        errs = "Invalid HSN Code! ";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "Invalid HSN Code! ";
                                    }
                                }
                            }

                          /*  if (dataTable.Rows[i][18].ToString() == "")
                            {
                                if (errs == "")
                                {
                                    errs = "RCM Y/N should not be empty!";
                                }
                                else
                                {
                                    errs = errs + "," + "RCM Y/N should not be empty!";
                                }
                            }
                            else
                            {
                                exceltext = dataTable.Rows[i][18].ToString();
                                string GST = dataTable.Rows[i][16].ToString();

                                if (GST == "Y" && exceltext == "Y")
                                {
                                    if (errs == "")
                                    {
                                        errs = "GST Flag stated as Y then RCM should not be Y! ";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "GST Flag stated as Y then RCM should not be Y! ";
                                    }
                                }
                                else if (GST == "N" && (exceltext != "Y" && exceltext != "N"))
                                {
                                    if (errs == "")
                                    {
                                        errs = "Invalid RCM flag Value! ";
                                    }
                                    else
                                    {
                                        errs = errs + "," + "Invalid RCM flag Value! ";
                                    }
                                }
                            }*/


                            if (errs != "")
                            {
                                sno++;
                                Errdatatable.Rows.Add(sno, RowNo, errs);
                            }
                        }
                        else
                        {
                            sno++;
                            Errdatatable.Rows.Add(sno, RowNo, "S.No Should Not Be Empty");
                        }
                    }
                    if (Errdatatable.Rows.Count == 0)
                    {
                        for (int trou = 0; trou < empchck.Count; trou++)
                        {
                            DataTable err1 = new DataTable();
                            err1 = (DataTable)dataTable;
                            decimal tolrowinvoiceamt = 0;
                            DataTable uniqueemp = new DataTable();
                            string supliercodf = empchck[trou].ToString();
                            err1.DefaultView.RowFilter = "Convert([Supplier Code], 'System.String') ='" + supliercodf + "'";
                            uniqueemp = err1.DefaultView.ToTable();

                            if (uniqueemp.Rows.Count > 0)
                            {
                                for (int i = 0; i < uniqueemp.Rows.Count; i++)
                                {
                                    if (tolrowinvoiceamt == 0)
                                    {
                                        tolrowinvoiceamt = Convert.ToDecimal(uniqueemp.Rows[i]["Amount"].ToString().Trim());
                                    }
                                    else
                                    {
                                        tolrowinvoiceamt = tolrowinvoiceamt + Convert.ToDecimal(uniqueemp.Rows[i]["Amount"].ToString().Trim());
                                    }
                                }

                                string supcoede = supliercodf.ToString();
                                string invoicedate = uniqueemp.Rows[0]["Invoice Date"].ToString().Trim();

                                string[] splitsdate = invoicedate.Split('-');
                                string[] splitsdatesec = invoicedate.Split('/');
                                if (splitsdate.Length > 1)
                                {
                                    invoicedate = invoicedate.ToString();
                                }
                                else if (splitsdatesec.Length > 1)
                                {
                                    invoicedate = invoicedate.ToString();
                                }
                                else
                                {
                                    double d = double.Parse(invoicedate);
                                    DateTime conv = DateTime.FromOADate(d);
                                    invoicedate = conv.ToString();
                                }
                                DateTime invoiced = objCmnFunctions.convertoDateTime(invoicedate);
                                string startingyr = "";
                                int month = invoiced.Month;
                                if (month >= 4)
                                {
                                    startingyr = "01-04-" + invoiced.Year;
                                }
                                else
                                {
                                    int year = invoiced.Year - 1;
                                    startingyr = "01-04-" + year;
                                }
                                string messageamt = objModel.togetsumofinvoiceamt(supcoede, startingyr, invoicedate);

                                decimal tonrevamt = Convert.ToDecimal(messageamt);
                                decimal tonrevamtoverall = tonrevamt + tolrowinvoiceamt;
                                decimal dsatestamount = Convert.ToDecimal(ConfigurationManager.AppSettings["dsatestamount"].ToString());
                                // Commentted by Ramya due to User Request - Check mail - 24 Sep 18
                                //if (tonrevamtoverall > dsatestamount)
                                //{
                                //    sno++;
                                //    Errdatatable.Rows.Add(sno, sno, "The " + supcoede + " Total Invoice Amount exits 8 laks in the financial year");
                                //}
                            }
                        }
                    }
                }
                else
                {
                    Errdatatable.Rows.Add(1, "Please  Select Valid Sheet");
                }
                invalid = Errdatatable.Rows.Count;
                valid = totalrecord - invalid;
                ViewBag.vbvalid = "Total No of Vaild Record :" + valid;
                ViewBag.vbinvalid = "Total No of InVaild Record :" + invalid;
                ViewBag.vbtotalrecord = "Total No of Record Excel File :" + totalrecord;
                if (tolamt != tolamtexcel)
                {
                    sno++;
                    Errdatatable.Rows.Add(sno, sno, "ECF Amount Should Be Equal Sum Of Excel Total Amount");
                }
                Session["maindissupplier"] = empchck;
                Session["maindissupplierou"] = empchckou;
                Session["Errdatatable"] = Errdatatable;
                Session["maindatatable"] = maindatatable;
            }
            catch (Exception ex)
            {
                sno++;
                Errdatatable.Rows.Add(sno, Errdatatable.Rows.Count + 1, ex.Message.ToString() + " Please ckeck Excel File Error While Reading Data" + ex.ToString());

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
        public PartialViewResult Supplierdetails(string ddlSelectsheet)
        {
            Session["err"] = "flag";
            return PartialView();
        }
        public JsonResult Supplierdetails()
        {
            try
            {
                Session["err"] = "flag";
                DataTable err1 = new DataTable();
                err1 = (DataTable)Session["maindatatable"];

                Hashtable dissupplier = new Hashtable();
                dissupplier = (Hashtable)Session["maindissupplier"];

                Hashtable dissupplierou = new Hashtable();
                dissupplierou = (Hashtable)Session["maindissupplierou"];

                err1.Columns.Add("ecfgid", typeof(System.Int32));
                int ecfgids = Convert.ToInt32(Session["EcfGid"].ToString());
                foreach (DataRow row in err1.Rows)
                {
                    row["ecfgid"] = ecfgids;
                }

                string objlistdnew = objModel.GetSupplieruploaadexcel(err1, Session["EcfGid"].ToString(), "");

                //string objlistd = objModel.GetSupplieruploaad(err1, Session["EcfGid"].ToString(), Session["EcfDatemain"].ToString(), dissupplier, dissupplierou);
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Json("Error occurred...", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Sdownloadsexcel()
        {
            Session["err"] = "";
            Session["invoiceclear"] = "clear";
            DataTable dtnew = (DataTable)Session["Errdatatable"];
            if (dtnew.Rows.Count > 0)
            {
                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "SupplierInvoiceDSA.xls"));
                Response.ContentType = "application/vnd.ms-excel";
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
            }
            return View();
        }

        public ActionResult Ssamdownloadsexcel()
        {
            using (DataTable dtnew = new DataTable())
            {
                dtnew.Columns.Add("SNo", typeof(int));
                dtnew.Columns.Add("Supplier Code", typeof(string));
                dtnew.Columns.Add("Supplier Name", typeof(string));
                dtnew.Columns.Add("Invoice Date", typeof(DateTime));
                dtnew.Columns.Add("Description", typeof(string));
                dtnew.Columns.Add("Amount", typeof(decimal));
                dtnew.Columns.Add("Provision", typeof(string));
                dtnew.Columns.Add("Nature of Expenses", typeof(string));
                dtnew.Columns.Add("Main Category", typeof(string));
                dtnew.Columns.Add("Sub Category", typeof(string));
                dtnew.Columns.Add("Function Code", typeof(string));
                dtnew.Columns.Add("Cost Code", typeof(string));
                dtnew.Columns.Add("Product Code", typeof(string));
                dtnew.Columns.Add("OU Code", typeof(string));
                //dtnew.Columns.Add("Invoice No", typeof(string));
                dtnew.Columns.Add("Provider Location", typeof(string));
                dtnew.Columns.Add("Receiver Location", typeof(string));
                dtnew.Columns.Add("GST Y/N", typeof(string));
                dtnew.Columns.Add("HSN Code", typeof(string));
                //dtnew.Columns.Add("RCM Y/N", typeof(string));
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtnew, "SupplierInvoiceDSA");

                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=SupplierInvoiceDSATemplate.xls");
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
            byte[] fileBytes = System.IO.File.ReadAllBytes(dirInfo.FullName + "/SupplierDSA.xls");
            string fileName = "SupplierDSA.xls";
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        [HttpGet]
        public PartialViewResult _LocalAttachmentCreate()
        {
            EOW_File objMAttachment = new EOW_File();
            objMAttachment.AttachmentTypeData = new SelectList(objModel.AttachmentTypedata().ToList(), "AttachmentTypeId", "AttachmentTypeName");
            return PartialView(objMAttachment);
        }
        [HttpPost]
        public JsonResult EmpAttachmentDelete(EOW_File EmployeeeExpensemodel)
        {
            int EmployeeePaymentGID = (int)EmployeeeExpensemodel.AttachmentFilenameId;
            string delamt = objModel.DeleteEmployeeeAttachment(EmployeeePaymentGID, Session["EcfGid"].ToString());
            //Session["Ecfamountpayment"] = Convert.ToInt32(Session["Ecfamountpayment"].ToString()) + Convert.ToInt32(delamt.ToString());
            return Json(EmployeeeExpensemodel, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public PartialViewResult _LocalAttachmentDetails()
        {
            List<EOW_File> lstAttachment = new List<EOW_File>();
            //lstAttachment = objModel.GetEmployeeeAttachment(Session["EcfGid"].ToString(), objCmnFunctions.GetLoginUserGid().ToString()).ToList();
            return PartialView(lstAttachment);
        }
        [HttpPost]
        public JsonResult _LocalAttachmentCreate(EOW_File EmployeeeExpenseModel)
        {
            string img = "";
            try
            {
                if (TempData["_attFile"] != null)
                {
                    objErrorLog.WriteErrorLog("att create method", "success");
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
                    objErrorLog.WriteErrorLog("att create method else part", "failed");
                    img = "Invalid File Format!";
                }
                ViewBag.SearchItems = null;
                return Json(img, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw ex;
            }
        }
        public FileResult Download(int id)
        {
            string FileName = objModel.downloadAttachment(id, Session["EcfGid"].ToString());

           // //string delamt = objModel.downloadAttachment(id, Session["EcfGid"].ToString());
           // //int index = delamt.LastIndexOf(".");
            ////string[] seqNum = new string[] { delamt.Substring(0, index), delamt.Substring(index + 1) };
           // //string directory = @"C:\temp\";
           // //directory = directory + id.ToString() + "." + seqNum[1].ToString();
           // //byte[] fileBytes = System.IO.File.ReadAllBytes(directory);
           // //string fileName = seqNum[0].ToString() + "." + seqNum[1].ToString();
           // //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            string[] str = FileName.Split('.');
            string directory = id.ToString() + "." + str[str.Length - 1].ToString();
          //  string directory = objModel.HoldFileUploadUrl() + id.ToString() + "." + str[str.Length - 1].ToString();
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
            //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, FileName);
        }
        [HttpPost]
        public void UploadFilesatt(string attach = null, string attaching_format = null) //Pandiaraj 18-11-2019 
        {
            try
            {
                TempData["_attFile"] = null;
                objErrorLog.WriteErrorLog("first", "message");
                foreach (string file in Request.Files)
                {
                    objErrorLog.WriteErrorLog("second", "message");
                    var fileContent = Request.Files[file];
                    objErrorLog.WriteErrorLog("third", "message");
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        objErrorLog.WriteErrorLog("if condition First level validation crossed", "message");
                        string fileextension = Path.GetExtension(Path.GetFileName(fileContent.FileName));
                        objErrorLog.WriteErrorLog("if condition First level validation crossed", fileextension);
                        string[] attaching_fileformat = attaching_format.Split(',');
                        if (attaching_fileformat.Contains(fileextension.ToLower()))
                        {
                            objErrorLog.WriteErrorLog("First level validation crossed","message");
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
                                //objErrorLog.WriteErrorLog(TempData["_attFile"].ToString(), "message");
                                //objErrorLog.WriteErrorLog("second level validation crossed", "message");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), "upload files");
            }
        }
        [HttpPost]
        public JsonResult _suppliserinvoiceclear(EOW_SupplierModelgrid Suppliermodel)
        {
            string Err = "";
            string ecfno = "";
            try
            {
                ecfno = Session["EcfGid"].ToString();
                Err = "S";
                string insrtinvoice = objModel.DeleteSuppliernewlstdsa(Err, ecfno);
                if (insrtinvoice == "Success")
                {
                    Session["invoiceclear"] = "clear";
                    Session["err"] = "";
                    Session["invoiceGid"] = "";
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
        [HttpPost]
        public JsonResult _EmpECFCreate(EOW_EmployeeeExpense EmployeeeExpenseModel)
        {
            string Err = "";
            try
            {
                if (ModelState.IsValid)
                {
                    decimal ecfmain = Convert.ToDecimal(EmployeeeExpenseModel.Exp_Amount.ToString());
                    decimal ecfexcel = Convert.ToDecimal(Session["Supplierecfamountval"].ToString());
                     var result = objModel.GetSupplierBankDetailsBypayMode(Session["EcfGid"].ToString());

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
                        Err = objModel.Insertecf(EmployeeeExpenseModel, Session["EcfGid"].ToString(), Session["EcfGid"].ToString(), objCmnFunctions.GetLoginUserGid().ToString(), objCmnFunctions.GetLoginUserGid().ToString(), "D", Session["QueueGide"].ToString());
                        if (Err == "")
                        {
                            Err = "Error";
                        }
                        else
                        {
                            Err = Err.ToString();
                            Session["EcfGid"] = null;
                            Session["EcfamountExcelamt"] = null;
                            Session["Supplierecfamountval"] = null;
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
        public PartialViewResult _ExpensedetailsViewdsa(int id, string viewfor)
        {
            Session["Ecfcurrentiddsa"] = id.ToString();
            return PartialView();
        }
        public ActionResult downloadsexcelexport()
        {
            try
            {
                string mt = null;
                List<EOW_SupplierModelgrid> cList;

                cList = objModel.GetSupplierexceldata(Session["EcfGid"].ToString(), "S").ToList();

                DataTable dt = new DataTable();
                dt.Columns.Add("S.No.");
                dt.Columns.Add("Supplier Code");
                dt.Columns.Add("Supplier Name");
                dt.Columns.Add("Invoice Date");
                dt.Columns.Add("Invoice No");
                dt.Columns.Add("Invoice Amount");
                dt.Columns.Add("Description");
                for (int i = 0; i < cList.Count; i++)
                {
                    dt.Rows.Add(
                        i + 1,
                        cList[i].SupplierCode.ToString(),
                        cList[i].SupplierName.ToString(),
                        cList[i].InvoiceDate.ToString(),
                        cList[i].InvoiceNo.ToString(),
                        cList[i].Amount.ToString(),
                        cList[i].Description.ToString()
                        );
                }
                //export to excel from gridview
                GridView gv = new GridView();
                gv.DataSource = dt;
                gv.DataBind();
                Session["exceldownload"] = gv;
                if (gv.Rows.Count != 0)
                {
                    return new DownloadFileActionResult((GridView)Session["exceldownload"], "SupplierDSAGroup.xls");
                }
                else
                {
                    ViewBag.Message = "No records found";
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public FileResult DownloadDSATemplate()
        {
            //get the temp folder and file path in server
            string fullPath = System.Configuration.ConfigurationManager.AppSettings["DSA"];

            //return the file for download, this is an Excel 
            //so I set the file content type to "application/vnd.ms-excel"
            return File(fullPath, "application/vnd.ms-excel", "SupplierDSA.xls");
        }
    }
}
