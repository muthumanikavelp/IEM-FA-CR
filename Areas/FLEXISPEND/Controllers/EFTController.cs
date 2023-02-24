using IEM.App_Start;
using IEM.Areas.FLEXISPEND.Models;
using IEM.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.MASTERS.Controllers;
using System.Net;
//using ICSharpCode.SharpZipLib.Zip;
using IEM.Areas.FLEXISPEND.CreatePDF;
using Excel;
using ClosedXML.Excel;
using IEM.Common;

namespace IEM.Areas.FLEXISPEND.Controllers
{

    [NoDirectAccess]
    public class EFTController : PdfViewController
    {

        #region Decalartion

        dbLib db = new dbLib();
        FSRepository _fsRep = new FSRepository();
        proLib plib = new proLib();
        Message msg = new Message();
        DataTable dt = new DataTable();
        ForMailController MailController = new ForMailController();
        #endregion

        private readonly HtmlViewRenderer htmlViewRenderer;
        private readonly StandardPdfRenderer standardPdfRenderer;

        public EFTController()
        {
            this.htmlViewRenderer = new HtmlViewRenderer();
            this.standardPdfRenderer = new StandardPdfRenderer();
        }

        #region EFT File Generation.
        public ActionResult FileGeneration()
        {
            return View();
        }

        [HttpPost]
        public JsonResult FetchEFTMemoDetails(string PvDateFrom, string PvDateTo, string PvNo, string PvAmountFrom, string PvAmountTo, string EmpCodeId, string EmpNameId, string SuppCodeId, string SuppNameId,
            string ClaimType, string PayMode, string BankId, string BatchNo, string MemoNo)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetEFTMemoDetails(plib.ConvertDate(PvDateFrom), plib.ConvertDate(PvDateTo), PvNo, PvAmountFrom == null || PvAmountFrom == "" ? "0" : PvAmountFrom, PvAmountTo == null || PvAmountTo == "" ? "0" : PvAmountTo, EmpCodeId, EmpNameId, SuppCodeId, SuppNameId,
                                    ClaimType, PayMode, BankId, BatchNo, MemoNo, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult SetEFTMemoDetails(string PvIds, string Status, string CancelPvId, string Reason, string PayMode)
        {
            try
            {
                TempData["PVIds"] = null;

                string Data1 = "";
                DataSet ds = db.SetEFTMemoDetails(PvIds, Status, CancelPvId, Reason, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        Data1 = JsonConvert.SerializeObject(dt);

                        if (ds.Tables[0].Rows[0]["Clear"].ToString().ToLower() == "true" || ds.Tables[0].Rows[0]["Clear"].ToString().ToLower() == "1")
                        {
                            TempData["PVIds"] = PvIds;
                            TempData.Keep("PVIds");
                        }
                    }

                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult PrintExisting(string PvIds)
        {
            try
            {
                TempData["PVIds"] = null;
                TempData["PVIds"] = PvIds + "|";
                TempData.Keep("PVIds");
                return Json("OK", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Downloading Notepad File.. when we hit this action.
        public JsonResult GenerateDocuments(string Id, string subId, string PayBankGId)
        {
            string IsFileGenerated = "";
            string _Date = "", _SerialNo = "", _PayMode = "", _ViewType = "", _BankName = "";
            _PayMode = Id;
            _ViewType = subId == "" || subId == null ? "0" : subId;

            string PvIds = "";
            PvIds = TempData["PVIds"] != null ? TempData["PVIds"].ToString() : "";
            DataSet ds = db.PrintEFTMemoDetails(PvIds, _PayMode, _ViewType, PayBankGId, plib.LoginUserId);
            if (ds != null)
            {
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    _Date = ds.Tables[0].Rows[0]["CurrentDate"].ToString();
                    _SerialNo = ds.Tables[0].Rows[0]["SerialNo"].ToString();
                    if (_PayMode.ToLower().Trim() != "rrp")
                        _BankName = ds.Tables[0].Rows[0]["PayBankname"].ToString();
                }

                if (_Date != string.Empty && _SerialNo != string.Empty)
                {
                    IsFileGenerated = IsCheckFolder(_Date, _SerialNo, _PayMode, _ViewType, _BankName, "");
                }

                //Generate Online Template for EFT Template.
                if (_PayMode.ToLower().Trim() == "eft" && _ViewType == "0" && IsFileGenerated != string.Empty)
                {
                    string _NormalFile = "", _EncryptedFile = "";
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        _NormalFile = string.Format("{0}/{1}.txt", IsFileGenerated, ds.Tables[0].Rows[0]["NormalTextFile"].ToString());

                        string _encrypt = plib.EncryptMemoDownloadUrl;
                        DirectoryInfo dir = new DirectoryInfo(_encrypt);

                        //check folder Presence
                        if (dir.Exists)
                        {
                            _EncryptedFile = string.Format("{0}/{1}~{2}~Online~{3}.txt", _encrypt, _Date, _SerialNo, ds.Tables[0].Rows[0]["Encrypted"].ToString());
                        }
                        else
                        {
                            _EncryptedFile = "";
                        }
                    }

                    GenerateNotePadFile(_NormalFile, _EncryptedFile, ds.Tables[1]);
                }

                //Ramya added for Employee Claim
                //Generate Online Template for EFT Template.
                if (_PayMode.ToLower().Trim() == "era" && _ViewType == "0" && IsFileGenerated != string.Empty)
                {
                    string _NormalFile = "", _EncryptedFile = "";
                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        _NormalFile = string.Format("{0}/{1}.txt", IsFileGenerated, ds.Tables[0].Rows[0]["NormalTextFile"].ToString());

                        string _encrypt = plib.EncryptMemoDownloadUrl;
                        DirectoryInfo dir = new DirectoryInfo(_encrypt);

                        //check folder Presence
                        if (dir.Exists)
                        {
                            _EncryptedFile = string.Format("{0}/{1}~{2}~Online~{3}.txt", _encrypt, _Date, _SerialNo, ds.Tables[0].Rows[0]["Encrypted"].ToString());
                        }
                        else
                        {
                            _EncryptedFile = "";
                        }
                    }

                    GenerateNotePadFile(_NormalFile, _EncryptedFile, ds.Tables[1]);
                }

                //EFT Template Work [RTGS/NEFT]
                if (_PayMode.ToLower().Trim() == "eft" && _ViewType == "1")
                {
                    XLWorkbook xl = null;
                    if (ds.Tables[1].Rows.Count > 0)
                    {

                        DataTable dt = new DataTable();
                        dt.Columns.Add("SL No", typeof(string));
                        dt.Columns.Add("Name of Vendor", typeof(string));
                        dt.Columns.Add("Account No", typeof(string));
                        dt.Columns.Add("Bank Name", typeof(string));
                        dt.Columns.Add("IFSC Code", typeof(string));
                        dt.Columns.Add("Amount", typeof(string));
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            dt.Rows.Add(new object[] { i + 1, ds.Tables[1].Rows[i]["BenName"].ToString(), ds.Tables[1].Rows[i]["BenAccNo"].ToString(), ds.Tables[1].Rows[i]["BenBankname"].ToString(), ds.Tables[1].Rows[i]["BenBankIfscCode"].ToString(), ds.Tables[1].Rows[i]["Amount"].ToString() });
                        }



                        MemoDetail result = new MemoDetail();
                        result = GetMemoDetailsPrint(ds.Tables[1], ds.Tables[1]);

                        IsFileGenerated = IsCheckFolder(_Date, _SerialNo, _PayMode, _ViewType, _BankName, "NEFT");

                        xl = new XLWorkbook();
                        xl.Worksheets.Add(dt, "HDFC_NEFT");
                        xl.SaveAs(string.Format("{0}/HDFC_NEFT.xlsx", IsFileGenerated));

                        //save the DD Content to local folder.
                        string fileName = "";
                        fileName = string.Format("{0}/NEFT.html", IsFileGenerated);

                        // Render the view html to a string.
                        string htmlText = this.htmlViewRenderer.RenderViewToString(this, "TmpRTGS_HDFC_A_NEW", result);

                        //save the document as html.
                        using (FileStream fs = new FileStream(fileName, FileMode.Create))
                        {
                            using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                            {
                                w.Write(htmlText);
                            }
                        }
                        // Let the html be rendered into a PDF document through iTextSharp.
                        //byte[] buffer = standardPdfRenderer.Render(htmlText, "");

                        //using (FileStream fs = new FileStream(fileName, FileMode.Create))
                        //{
                        //    fs.Write(buffer, 0, buffer.Length);
                        //}
                    }

                    if (ds.Tables[2].Rows.Count > 0)
                    {

                        DataTable dt = new DataTable();
                        dt.Columns.Add("SL No", typeof(string));
                        dt.Columns.Add("Name of Vendor", typeof(string));
                        dt.Columns.Add("Account No", typeof(string));
                        dt.Columns.Add("Bank Name", typeof(string));
                        dt.Columns.Add("IFSC Code", typeof(string));
                        dt.Columns.Add("Amount", typeof(string));
                        for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                        {
                            dt.Rows.Add(new object[] { i + 1, ds.Tables[2].Rows[i]["BenName"].ToString(), ds.Tables[2].Rows[i]["BenAccNo"].ToString(), ds.Tables[2].Rows[i]["BenBankname"].ToString(), ds.Tables[2].Rows[i]["BenBankIfscCode"].ToString(), ds.Tables[2].Rows[i]["Amount"].ToString() });
                        }

                        MemoDetail result = new MemoDetail();
                        result = GetMemoDetailsPrint(ds.Tables[2], ds.Tables[2]);

                        IsFileGenerated = IsCheckFolder(_Date, _SerialNo, _PayMode, _ViewType, _BankName, "RTGS");

                        //save the DD Content to local folder.
                        string fileName = "";
                        fileName = string.Format("{0}/RTGS.html", IsFileGenerated);

                        xl = new XLWorkbook();
                        xl.Worksheets.Add(dt, "HDFC_NEFT");
                        xl.SaveAs(string.Format("{0}/HDFC_NEFT.xlsx", IsFileGenerated));

                        // Render the view html to a string.
                        string htmlText = this.htmlViewRenderer.RenderViewToString(this, "TmpRTGS_HDFC_A_NEW", result);

                        //save the document as html.
                        using (FileStream fs = new FileStream(fileName, FileMode.Create))
                        {
                            using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                            {
                                w.Write(htmlText);
                            }
                        }
                        // Let the html be rendered into a PDF document through iTextSharp.
                        //byte[] buffer = standardPdfRenderer.Render(htmlText, "");

                        //using (FileStream fs = new FileStream(fileName, FileMode.Create))
                        //{
                        //    fs.Write(buffer, 0, buffer.Length);
                        //}
                    }
                }

                //DD Template Work
                if (_PayMode.ToLower().Trim() == "dd" && IsFileGenerated != string.Empty)
                {
                    XLWorkbook xl = null;
                    DataTable dt = new DataTable();
                    dt.Columns.Add("SL No", typeof(string));
                    dt.Columns.Add("DD Favoring", typeof(string));
                    dt.Columns.Add("Payable at", typeof(string));
                    dt.Columns.Add("Amount", typeof(string));

                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        dt.Rows.Add(new object[] { i + 1, ds.Tables[1].Rows[i]["DDFavoring"].ToString(), ds.Tables[1].Rows[i]["PayableAt"].ToString(), ds.Tables[1].Rows[i]["Amount"].ToString() });
                    }
                    DDTemplate result = new DDTemplate();
                    result = GetDDTemplate(ds.Tables[0], ds.Tables[1]);

                    xl = new XLWorkbook();
                    xl.Worksheets.Add(dt, "HDFC_DD");
                    xl.SaveAs(string.Format("{0}/HDFC_DD.xlsx", IsFileGenerated));

                    //save the DD Content to local folder.
                    string fileName = "";
                    fileName = string.Format("{0}/HDFC_DD.html", IsFileGenerated);

                    // Render the view html to a string.
                    string htmlText = this.htmlViewRenderer.RenderViewToString(this, "TmpDD_HDFC", result);

                    //save the document as html.
                    using (FileStream fs = new FileStream(fileName, FileMode.Create))
                    {
                        using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                        {
                            w.Write(htmlText);
                        }
                    }
                    // Let the html be rendered into a PDF document through iTextSharp.
                    //byte[] buffer = standardPdfRenderer.Render(htmlText, "");

                    //using (FileStream fs = new FileStream(fileName, FileMode.Create))
                    //{
                    //    fs.Write(buffer, 0, buffer.Length);
                    //}
                }

                if (_PayMode.ToLower().Trim() == "rrp" && IsFileGenerated != string.Empty)
                {
                    XLWorkbook xl = null;
                    DataTable dt = new DataTable();
                    dt.Columns.Add("SL No", typeof(string));
                    dt.Columns.Add("Pay Number", typeof(string));
                    dt.Columns.Add("Employee Code", typeof(string));
                    dt.Columns.Add("Employee Name", typeof(string));
                    dt.Columns.Add("Location", typeof(string));
                    dt.Columns.Add("Amount", typeof(string));
                    dt.Columns.Add("ECF No", typeof(string));

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        dt.Rows.Add(new object[] { i + 1, ds.Tables[0].Rows[i]["PVNo"].ToString(), ds.Tables[0].Rows[i]["EmployeeSupplierCode"].ToString(), ds.Tables[0].Rows[i]["EmployeeSupplierName"].ToString()
                        , ds.Tables[0].Rows[i]["Location"].ToString(), ds.Tables[0].Rows[i]["Amount"].ToString(), ds.Tables[0].Rows[i]["ECFNo"].ToString()});
                    }

                    xl = new XLWorkbook();
                    xl.Worksheets.Add(dt, "RRP");
                    xl.SaveAs(string.Format("{0}/" + @System.Configuration.ConfigurationManager.AppSettings["CompanyName"].ToString() + "-RRP-REG-" + _Date.Replace("-", "") + _SerialNo + ".xlsx", IsFileGenerated));
                }

                //ERA Template Work
                if (_PayMode.ToLower().Trim() == "era" && IsFileGenerated != string.Empty)
                {
                    string _ViewName = "";
                    XLWorkbook xl = null;
                    ERATemplate result = new ERATemplate();

                    result = GetERATemplate(ds.Tables[0], ds.Tables[2]);

                    if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && result != null)
                    {

                        if (_BankName.ToLower().Contains("citi"))
                        {
                            _ViewName = "TmpDT_CITI";
                        }

                        if (_BankName.ToLower().Contains("hdfc"))
                        {
                            _ViewName = "TmpDT_HDFC_ERA";
                        }

                        if (_BankName.ToLower().Contains("icici"))
                        {
                            _ViewName = "TmpDT_ICICI";
                        }
                        if (_BankName.ToLower().Contains("sbi") || _BankName.ToUpper().Contains("STATE BANK OF INDIA"))
                        {
                            _ViewName = "TmpDT_SBI";
                        }
                        if (_BankName.ToLower().Contains("uti") || _BankName.ToUpper().Contains("AXIS"))
                        {
                            _ViewName = "TmpDT_UTI";
                        }
                        if (_ViewName == "")
                        {
                            _ViewName = "TmpDT_HDFC_ERA";
                        }
                    }
                    if (_ViewName != "")
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("SL No", typeof(string));
                        dt.Columns.Add("Employee Code", typeof(string));
                        dt.Columns.Add("Employee Name", typeof(string));
                        dt.Columns.Add("Account No", typeof(string));
                        dt.Columns.Add("Amount", typeof(string));
                        //RAMYA for IFSC Code
                        dt.Columns.Add("IFSC Code", typeof(string));
                        for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                        {
                            dt.Rows.Add(new object[] { i + 1, ds.Tables[2].Rows[i]["EmployeeSupplierCode"].ToString(), ds.Tables[2].Rows[i]["EmployeeSupplierName"].ToString(), ds.Tables[2].Rows[i]["AccNo"].ToString(), ds.Tables[2].Rows[i]["Amount"].ToString(), ds.Tables[2].Rows[i]["IFSCCode"].ToString() });
                        }
                        //save the DD Content to local folder.
                        string fileName = "";
                        if (_ViewName == "TmpDT_CITI")
                        {
                            fileName = string.Format("{0}/CITI.html", IsFileGenerated);
                            xl = new XLWorkbook();
                            xl.Worksheets.Add(dt, "CITI");
                            xl.SaveAs(string.Format("{0}/CITI.xlsx", IsFileGenerated));
                        }
                        else if (_ViewName == "TmpDT_HDFC_ERA")
                        {
                            fileName = string.Format("{0}/HDFC.html", IsFileGenerated);
                            xl = new XLWorkbook();
                            xl.Worksheets.Add(dt, "HDFC");
                            xl.SaveAs(string.Format("{0}/HDFC.xlsx", IsFileGenerated));
                        }
                        else if (_ViewName == "TmpDT_ICICI")
                        {
                            fileName = string.Format("{0}/ICICI.html", IsFileGenerated);
                            xl = new XLWorkbook();
                            xl.Worksheets.Add(dt, "ICICI");
                            xl.SaveAs(string.Format("{0}/ICICI.xlsx", IsFileGenerated));
                        }
                        else if (_ViewName == "TmpDT_SBI")
                        {
                            fileName = string.Format("{0}/SBIDT.txt", IsFileGenerated);
                            GenerateNotePad(fileName, ds.Tables[1]);
                            fileName = string.Format("{0}/SBI.html", IsFileGenerated);
                            xl = new XLWorkbook();
                            xl.Worksheets.Add(dt, "SBI");
                            xl.SaveAs(string.Format("{0}/SBI.xlsx", IsFileGenerated));
                        }
                        else if (_ViewName == "TmpDT_UTI")
                        {
                            fileName = string.Format("{0}/UTI.html", IsFileGenerated);
                            xl = new XLWorkbook();
                            xl.Worksheets.Add(dt, "UTI");
                            xl.SaveAs(string.Format("{0}/UTI.xlsx", IsFileGenerated));
                        }


                        // Render the view html to a string.
                        string htmlText = this.htmlViewRenderer.RenderViewToString(this, _ViewName, result);

                        //save the document as html.
                        using (FileStream fs = new FileStream(fileName, FileMode.Create))
                        {
                            using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                            {
                                w.Write(htmlText);
                            }
                        }

                    }
                    // Let the html be rendered into a PDF document through iTextSharp.
                    //byte[] buffer = standardPdfRenderer.Render(htmlText, "");

                    //using (FileStream fs = new FileStream(fileName, FileMode.Create))
                    //{
                    //    fs.Write(buffer, 0, buffer.Length);
                    //}
                }
            }
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public void GenerateNotePadFile(string NormalFile, string Encrypted, DataTable dt)
        {
            string delemited = ",";

            //NORMAL FILE -- Create Notepad
            //clear the previous data's
            FileStream fs = new FileStream(NormalFile, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter _fsstr = new StreamWriter(fs);
            _fsstr.WriteLine(string.Empty);
            _fsstr.Flush(); _fsstr.Close();

            //update the notepad content for downloading.
            if (dt.Rows.Count > 0)
            {
                StreamWriter str = new StreamWriter(NormalFile, false, System.Text.Encoding.Default);
                foreach (DataRow datarow in dt.Rows)
                {
                    string row = string.Empty;
                    row = datarow["TransactionType"].ToString() + "," + datarow["SupplierCode"].ToString() + "," + datarow["AccNo"].ToString() + ","
                        + datarow["PVAmount"].ToString() + "," + datarow["SupplierName"].ToString() + ",,,,,,,,," + datarow["PVNo"].ToString() + ",,,,,,,,,"
                        + datarow["FileSpoolingDate"].ToString() + ",," + datarow["IFSCCode"].ToString() + "," + datarow["Bankname"].ToString() + ",,";
                    str.WriteLine(row);
                }
                str.Flush();
                str.Close();
            }

            //ENCRYPT FILE -- Create Notepad
            //clear the previous data's
            if (Encrypted != "")
            {
                FileStream efs = new FileStream(Encrypted, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter _efsstr = new StreamWriter(efs);
                _efsstr.WriteLine(string.Empty);
                _efsstr.Flush(); _efsstr.Close();

                //update the notepad content for downloading.
                if (dt.Rows.Count > 0)
                {
                    StreamWriter str = new StreamWriter(Encrypted, false, System.Text.Encoding.Default);
                    foreach (DataRow datarow in dt.Rows)
                    {
                        string row = string.Empty;
                        row = datarow["TransactionType"].ToString() + "," + datarow["SupplierCode"].ToString() + "," + datarow["AccNo"].ToString() + ","
                            + datarow["PVAmount"].ToString() + "," + datarow["SupplierName"].ToString() + ",,,,,,,,," + datarow["PVNo"].ToString() + ",,,,,,,,,"
                            + datarow["FileSpoolingDate"].ToString() + ",," + datarow["IFSCCode"].ToString() + "," + datarow["Bankname"].ToString() + ",,";
                        str.WriteLine(row);
                    }
                    str.Flush();
                    str.Close();
                }
            }
        }

        public void GenerateNotePad(string NormalFile, DataTable dt)
        {
            FileStream fs = new FileStream(NormalFile, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter _fsstr = new StreamWriter(fs);
            _fsstr.WriteLine(string.Empty);
            _fsstr.Flush(); _fsstr.Close();

            //update the notepad content for downloading.
            if (dt.Rows.Count > 0)
            {
                StreamWriter str = new StreamWriter(NormalFile, false, System.Text.Encoding.Default);
                foreach (DataRow datarow in dt.Rows)
                {
                    string row = string.Empty;
                    row = datarow["AccNo"].ToString() + "          " + datarow["Amount"].ToString();
                    str.WriteLine(row);
                }
                str.Flush();
                str.Close();
            }
        }

        public string IsCheckFolder(string Date, string SerialNo, string PayMode, string SubFolder, string BankName, string Mode)
        {
            try
            {
                bool _MFolder = false, _DFolder = false, _SerFolder = false, _SBFolder = false, _SBFolder1 = false, _SUBFolder2 = false;
                string _mainFolder = "", _dateFolder = "", _serialFolder = "", _subFolder = "", _subFolder1 = "", _subFolder2 = "";


                //Create Root Folder
                _mainFolder = plib.DownloadMemoUrl;
                _MFolder = FolderCreation(_mainFolder);

                //Create Date Folder
                _dateFolder = string.Format("{0}/{1}", _mainFolder, Date);
                _DFolder = FolderCreation(_dateFolder);

                //Create Serial No Folder
                _serialFolder = string.Format("{0}/{1}", _dateFolder, SerialNo);
                _SerFolder = FolderCreation(_serialFolder);

                if ((PayMode.ToLower().Trim() == "eft" || PayMode.ToLower().Trim() == "era") && SubFolder == "0")
                {
                    _subFolder1 = string.Format("{0}/{1}", _serialFolder, "Online");
                    _SBFolder1 = FolderCreation(_subFolder1);
                }
                else if (PayMode.ToLower().Trim() == "dd")
                {
                    _subFolder1 = string.Format("{0}/{1}", _serialFolder, "DD");
                    _SBFolder1 = FolderCreation(_subFolder1);
                }
                else if (PayMode.ToLower().Trim() == "rrp")
                {
                    _subFolder1 = string.Format("{0}/{1}", _serialFolder, "RRP");
                    _SBFolder1 = FolderCreation(_subFolder1);
                }
                else
                {
                    _subFolder = string.Format("{0}/{1}", _serialFolder, "MEMO");
                    _SBFolder = FolderCreation(_subFolder);

                    //Create Memo Folder
                    _subFolder2 = string.Format("{0}/{1}", _subFolder, PayMode);
                    _SUBFolder2 = FolderCreation(_subFolder2);

                    if (PayMode.ToLower().Trim() == "era")
                    {
                        if (BankName.ToLower().Contains("citi"))
                        {
                            _subFolder1 = string.Format("{0}/{1}", _subFolder2, "CITI");
                            _SBFolder1 = FolderCreation(_subFolder1);
                        }
                        else if (BankName.ToLower().Contains("icici"))
                        {
                            _subFolder1 = string.Format("{0}/{1}", _subFolder2, "ICICI");
                            _SBFolder1 = FolderCreation(_subFolder1);
                        }
                        else if (BankName.ToLower().Contains("sbi") || BankName.ToUpper().Contains("STATE BANK OF INDIA"))
                        {
                            _subFolder1 = string.Format("{0}/{1}", _subFolder2, "SBI");
                            _SBFolder1 = FolderCreation(_subFolder1);
                        }
                        else if (BankName.ToLower().Contains("uti") || BankName.ToUpper().Contains("AXIS"))
                        {
                            _subFolder1 = string.Format("{0}/{1}", _subFolder2, "AXIS");
                            _SBFolder1 = FolderCreation(_subFolder1);
                        }
                        else
                        {
                            _subFolder1 = string.Format("{0}/{1}", _subFolder2, "HDFC");
                            _SBFolder1 = FolderCreation(_subFolder1);
                        }
                    }
                    else if (PayMode.ToLower().Trim() == "eft")
                    {
                        if (Mode.ToLower().Contains("rtgs"))
                        {
                            _subFolder1 = string.Format("{0}/{1}", _subFolder2, "HDFC - RTGS");
                            _SBFolder1 = FolderCreation(_subFolder1);
                        }
                        else if (Mode.ToLower().Contains("neft"))
                        {
                            _subFolder1 = string.Format("{0}/{1}", _subFolder2, "HDFC - NEFT");
                            _SBFolder1 = FolderCreation(_subFolder1);
                        }
                    }
                }

                return _subFolder1;
            }
            catch
            {
                return "";
            }
        }

        public bool FolderCreation(string FolderName)
        {
            try
            {
                DirectoryInfo dir = new DirectoryInfo(FolderName);

                //check folder Presence
                if (!dir.Exists)
                {
                    dir.Create();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Downloading Notepad File.. when we hit this action.
        public ViewResult Print(string Id)
        {
            List<MemoDetail> result = new List<MemoDetail>();

            string PvIds = "";
            PvIds = TempData["PVIds"] != null ? TempData["PVIds"].ToString() : "";
            if (PvIds != null && PvIds != "" && PvIds != "0")
            {
                DataSet ds = null;//= db.PrintEFTMemoDetails(PvIds, "1", plib.LoginUserId);
                if (ds != null)
                {
                    result = GetMemoDetailsPrint(ds.Tables[0], ds.Tables[1], Id);
                }
            }
            return View(result);
        }

        public DDTemplate GetDDTemplate(DataTable dtDet, DataTable dtRem)
        {
            DDTemplate rec = new DDTemplate();
            DataTable dt = null;
            if (dtDet != null)
            {
                if (dtDet.Rows.Count > 0)
                {
                    rec.Date = dtDet.Rows[0]["Date"].ToString();
                    rec.LetterNo = dtDet.Rows[0]["MemoNo"].ToString();
                    rec.BankAddress = dtDet.Rows[0]["BankAddress"].ToString();
                    rec.CompanyAccountNo = dtDet.Rows[0]["CompanyAccNo"].ToString();
                    //kavitha for dd favor ,pay at shows blank
                    rec.DDFavoring = dtDet.Rows[0]["DDFavoring"].ToString();
                    rec.PayableAt = dtDet.Rows[0]["PayableAt"].ToString();
                    rec.Amount = dtDet.Rows[0]["Amount"].ToString();
                    rec.AmountInWords = dtDet.Rows[0]["AmountInWords"].ToString();

                    dt = dtRem;//LoadChildList(dtRem, dtDet.Rows[i]["PvId"].ToString());

                    List<DDInnerDetails> childArray = new List<DDInnerDetails>();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow cdr in dt.Rows)
                        {
                            DDInnerDetails child = new DDInnerDetails();
                            child.DDFavoring = cdr["DDFavoring"].ToString();
                            child.PayableAt = cdr["PayableAt"].ToString();
                            child.Amount = cdr["Amount"].ToString();
                            childArray.Add(child);
                        }
                        rec.Totalamount = childArray.Sum(item => Convert.ToDecimal(item.Amount));
                    }
                    rec.DetailArray = childArray;
                }
            }
            return rec;
        }

        public ERATemplate GetERATemplate(DataTable dtDet, DataTable dtRem)
        {
            ERATemplate rec = new ERATemplate();
            DataTable dt = null;
            if (dtDet != null)
            {
                if (dtDet.Rows.Count > 0)
                {
                    rec.Date = dtDet.Rows[0]["Date"].ToString();
                    rec.LetterNo = dtDet.Rows[0]["MemoNo"].ToString();
                    rec.BankAddress = dtDet.Rows[0]["BankAddress"].ToString();
                    rec.CompanyAccountNo = dtDet.Rows[0]["CompanyAccNo"].ToString();
                    rec.CompanyAccountNo = dtDet.Rows[0]["CompanyAccNo"].ToString();
                    rec.KindAttan = dtDet.Rows[0]["KindAttan"].ToString();
                    rec.Amount = dtDet.Rows[0]["Amount"].ToString();
                    rec.AmountInWords = dtDet.Rows[0]["AmountInWords"].ToString();

                    dt = dtRem;
                    List<ERAInnerDetails> childArray = new List<ERAInnerDetails>();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow cdr in dt.Rows)
                        {
                            ERAInnerDetails child = new ERAInnerDetails();
                            child.VendorCode = cdr["EmployeeSupplierCode"].ToString();
                            child.NameOfVendor = cdr["EmployeeSupplierName"].ToString();
                            child.BankAccountNo = cdr["AccNo"].ToString();
                            child.IFSCCode = cdr["IFSCCode"].ToString();
                            child.Amount = cdr["Amount"].ToString();
                            child.RemittanceDetails = cdr["RemitterDetails"].ToString();
                            childArray.Add(child);
                        }
                    }
                    rec.DetailArray = childArray;
                }
            }
            return rec;
        }

        public MemoDetail GetMemoDetailsPrint(DataTable dtDet, DataTable dtRem)
        {
            MemoDetail rec = new MemoDetail();
            DataTable dt = null;
            if (dtDet != null)
            {

                //Top Details
                rec.Mode = dtDet.Rows[0]["TransactionType"].ToString();
                rec.AccountType = dtDet.Rows[0]["AccountType"].ToString();
                rec.BranchDetails = dtDet.Rows[0]["branchCode"].ToString() != "" ? dtDet.Rows[0]["branchCode"].ToString() : dtDet.Rows[0]["branchName"].ToString();
                rec.MemoNo = dtDet.Rows[0]["MemoNo"].ToString();
                rec.MemoTime = dtDet.Rows[0]["Time"].ToString();
                rec.MemoDate = dtDet.Rows[0]["Date"].ToString();

                //Beneficiary Details
                if (dtRem.Rows.Count > 1)
                {
                    rec.BenName = "AS PER LIST";
                    rec.BenAccNo = "AS PER LIST";
                    rec.BenAddress = "AS PER LIST";
                    rec.BenBankname = "AS PER LIST";
                    rec.BenBankIfscCode = "AS PER LIST";
                    rec.AmountInWords = "AS PER LIST";
                    rec.AmountInFigures = "AS PER LIST";
                }
                else
                {
                    rec.BenName = dtDet.Rows[0]["BenName"].ToString();
                    rec.BenAccNo = dtDet.Rows[0]["BenAccNo"].ToString();
                    rec.BenAddress = dtDet.Rows[0]["BenAddress"].ToString();
                    rec.BenBankname = dtDet.Rows[0]["BenBankname"].ToString();
                    rec.BenBankIfscCode = dtDet.Rows[0]["BenBankIfscCode"].ToString();
                    rec.AmountInWords = dtDet.Rows[0]["AmountInWords"].ToString();
                    rec.AmountInFigures = dtDet.Rows[0]["Amount"].ToString();
                }

                //Our Details
                if (dtRem.Rows.Count > 1)
                {
                    rec.RemitterName = "AS PER LIST";
                    rec.RemitterAccNo = "AS PER LIST";
                    rec.RemitterCashDeposited = "AS PER LIST";
                    rec.RemitterMobilePhoneNo = "AS PER LIST";
                    rec.RemitterEmailId = "AS PER LIST";
                    rec.RemitterAddress = "AS PER LIST";
                    rec.Remarks = "AS PER LIST";
                }
                else
                {
                    rec.RemitterName = dtDet.Rows[0]["RemitterName"].ToString();
                    rec.RemitterAccNo = dtDet.Rows[0]["RemitterAccNo"].ToString();
                    rec.RemitterCashDeposited = dtDet.Rows[0]["RemitterCashDeposited"].ToString();
                    rec.RemitterMobilePhoneNo = dtDet.Rows[0]["RemitterMobilePhoneNo"].ToString();
                    rec.RemitterEmailId = dtDet.Rows[0]["RemitterEmailId"].ToString();
                    rec.RemitterAddress = dtDet.Rows[0]["RemitterAddress"].ToString();
                    rec.Remarks = dtDet.Rows[0]["Remarks"].ToString();
                }


                /*rec.TotalAmount = dtDet.Rows[i]["TotAmount"].ToString();
                rec.Amount = dtDet.Rows[i]["Amount"].ToString();
                rec.BatchNo = dtDet.Rows[i]["BatchNo"].ToString();
                rec.PvId = dtDet.Rows[i]["PvId"].ToString();
                rec.EmployeeSupplierCode = dtDet.Rows[i]["EmployeeSupplierCode"].ToString();
                rec.EmployeeSupplierName = dtDet.Rows[i]["EmployeeSupplierName"].ToString();
                rec.RemitterDetails = dtDet.Rows[i]["RemitterDetails"].ToString();
                rec.RemitterCode = dtDet.Rows[i]["RemitterCode"].ToString();
                rec.CompanyAccNo = dtDet.Rows[i]["CompanyAccNo"].ToString();
                rec.BankAddress = dtDet.Rows[i]["BankAddress"].ToString();*/

                dt = dtRem;

                List<InnerDetails> childArray = new List<InnerDetails>();
                if (dt != null && dt.Rows.Count > 1)
                {
                    foreach (DataRow cdr in dt.Rows)
                    {
                        InnerDetails child = new InnerDetails();
                        child.NameOfVendor = cdr["BenName"].ToString();
                        child.BankAccNo = cdr["BenAccNo"].ToString();
                        child.BankName = cdr["BenBankname"].ToString();
                        child.IFSCCode = cdr["BenBankIfscCode"].ToString();
                        child.Amount = cdr["Amount"].ToString() == "" ? "0" : cdr["Amount"].ToString();
                        child.RemitterDetails = cdr["RemitterDetails"].ToString();
                        childArray.Add(child);
                    }
                    rec.TotalAmount = childArray.Sum(item => Convert.ToDecimal(item.Amount));
                }
                else
                {
                    childArray = null;
                    string _tmpVal = "";
                    _tmpVal = dtDet.Rows[0]["Amount"].ToString() == "" ? "0" : dtDet.Rows[0]["Amount"].ToString();
                    rec.TotalAmount = Convert.ToDecimal(_tmpVal);
                }

                rec.DetailArray = childArray;

            }
            return rec;
        }

        public List<MemoDetail> GetMemoDetailsPrint(DataTable dtDet, DataTable dtRem, string Mode)
        {
            List<MemoDetail> result = new List<MemoDetail>();
            DataTable dt = null;
            if (dtDet != null)
            {
                for (int i = 0; i < dtDet.Rows.Count; i++)
                {
                    MemoDetail rec = new MemoDetail();
                    rec.Mode = Mode;
                    rec.BranchDetails = dtDet.Rows[i]["branchCode"].ToString() != "" ? dtDet.Rows[i]["branchCode"].ToString() : dtDet.Rows[i]["branchName"].ToString();
                    rec.IsShowTable = dtDet.Rows[i]["Bit"].ToString();
                    rec.PVDate = dtDet.Rows[i]["PVDate"].ToString();
                    rec.PVNo = dtDet.Rows[i]["PVNo"].ToString();
                    //rec.TotalAmount = dtDet.Rows[i]["TotAmount"].ToString();
                    rec.Amount = dtDet.Rows[i]["Amount"].ToString();
                    rec.BatchNo = dtDet.Rows[i]["BatchNo"].ToString();
                    rec.PvId = dtDet.Rows[i]["PvId"].ToString();
                    rec.MemoNo = dtDet.Rows[i]["MemoNo"].ToString();
                    rec.MemoTime = dtDet.Rows[i]["MemoDateTime"].ToString();
                    rec.MemoDate = dtDet.Rows[i]["MemoDate"].ToString();
                    rec.EmployeeSupplierCode = dtDet.Rows[i]["EmployeeSupplierCode"].ToString();
                    rec.EmployeeSupplierName = dtDet.Rows[i]["EmployeeSupplierName"].ToString();
                    rec.BenName = dtDet.Rows[i]["BenName"].ToString();
                    rec.BenAccNo = dtDet.Rows[i]["BenAccNo"].ToString();
                    rec.BenAddress = dtDet.Rows[i]["BenAddress"].ToString();
                    rec.BenBankname = dtDet.Rows[i]["BenBankname"].ToString();
                    rec.BenBankIfscCode = dtDet.Rows[i]["BenBankIfscCode"].ToString();
                    rec.AmountInWords = dtDet.Rows[i]["AmountInWords"].ToString();
                    rec.RemitterName = dtDet.Rows[i]["RemitterName"].ToString();
                    rec.RemitterAccNo = dtDet.Rows[i]["RemitterAccNo"].ToString();
                    rec.RemitterDetails = dtDet.Rows[i]["RemitterDetails"].ToString();
                    rec.RemitterMobilePhoneNo = dtDet.Rows[i]["RemitterMobilePhoneNo"].ToString();
                    rec.RemitterAddress = dtDet.Rows[i]["RemitterAddress"].ToString();
                    rec.RemitterCashDeposited = dtDet.Rows[i]["RemitterCashDeposited"].ToString();
                    rec.RemitterEmailId = dtDet.Rows[i]["RemitterEmailId"].ToString();
                    rec.Remarks = dtDet.Rows[i]["Remarks"].ToString();

                    rec.RemitterCode = dtDet.Rows[i]["RemitterCode"].ToString();
                    rec.CompanyAccNo = dtDet.Rows[i]["CompanyAccNo"].ToString();
                    rec.BankAddress = dtDet.Rows[i]["BankAddress"].ToString();

                    dt = LoadChildList(dtRem, dtDet.Rows[i]["PvId"].ToString());

                    List<InnerDetails> childArray = new List<InnerDetails>();
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow cdr in dt.Rows)
                        {
                            InnerDetails child = new InnerDetails();
                            child.NameOfVendor = cdr["RemitterName"].ToString();
                            child.BankAccNo = cdr["RemitterAccNo"].ToString();
                            child.BankName = cdr["RemitterDetails"].ToString();
                            child.IFSCCode = cdr["BenBankIfscCode"].ToString();
                            child.Amount = cdr["Amount"].ToString();
                            child.RemitterDetails = dtDet.Rows[i]["RemitterCode"].ToString();
                            childArray.Add(child);
                        }
                    }
                    rec.DetailArray = childArray;
                    result.Add(rec);
                }
            }
            return result;
        }

        public DataTable LoadChildList(DataTable dt, string FilterId)
        {
            DataRow[] dr = dt.Select("PvId = " + FilterId);
            DataTable tdt = dt.Copy();
            tdt.Rows.Clear();
            foreach (DataRow tdr in dr)
            {
                tdt.ImportRow(tdr);
            }
            return tdt;
        }

        //Downloading Notepad File.. when we hit this action.
        //file = string.Format("{0}://{1}{2}Template/Memo.txt", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
        //file = Server.MapPath(Url.Content("~/Template/Memo.txt"));
        public FileResult DownloadFile()
        {
            string file = "", _colName = "", directory = "", PvIds = "", _memoLocal = "",
                _memoEncryptLocal = "", _fileName = "", _EncryptFileName = "", _NoEncryptFileName = "";
            string delemited = ",";
            string _downloadNormalFile = "";


            directory = plib.MemoDownloadUrl;
            _memoLocal = directory;
            _memoEncryptLocal = plib.EncryptMemoDownloadUrl;

            //Remove the previous files and directory's
            DirectoryInfo di = new DirectoryInfo(_memoLocal);
            foreach (FileInfo _file in di.GetFiles())
            {
                _file.Delete();
            }

            DirectoryInfo edi = new DirectoryInfo(_memoEncryptLocal);
            foreach (FileInfo _file in edi.GetFiles())
            {
                _file.Delete();
            }

            DirectoryInfo downloaddi = new DirectoryInfo(plib.DownloadMemoUrl);
            foreach (FileInfo _file in downloaddi.GetFiles())
            {
                _file.Delete();
            }

            PvIds = TempData["PVIds"] != null ? TempData["PVIds"].ToString() : "";
            DataSet ds = null;// db.PrintEFTMemoDetails(PvIds, "0", plib.LoginUserId);
            if (ds != null)
            {

                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataColumn dr in ds.Tables[1].Columns)
                    {
                        _colName = file = _fileName = "";
                        _fileName = ds.Tables[1].Rows[0][dr.ColumnName].ToString();
                        if (dr.ColumnName.Equals("Encrypted"))
                        {
                            _EncryptFileName = _fileName;
                            file = string.Format("{0}{1}.txt", _memoEncryptLocal, _fileName);

                            //clear the previous data's
                            FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write);
                            StreamWriter _fsstr = new StreamWriter(fs);
                            _fsstr.WriteLine(string.Empty);
                            _fsstr.Flush(); _fsstr.Close();

                            dt = ds.Tables[0];
                            //update the notepad content for downloading.
                            if (dt.Rows.Count > 0)
                            {
                                StreamWriter str = new StreamWriter(file, false, System.Text.Encoding.Default);
                                foreach (DataRow datarow in dt.Rows)
                                {
                                    string row = string.Empty;
                                    foreach (object items in datarow.ItemArray)
                                    {
                                        row += items.ToString() + delemited;
                                    }
                                    str.WriteLine(row.Remove(row.Length - 1, 1));
                                }
                                str.Flush();
                                str.Close();
                            }
                        }
                        if (dr.ColumnName.Equals("NormalTextFile"))
                        {
                            _NoEncryptFileName = _fileName;
                            //file = string.Format("{0}{1}.txt", _memoLocal, _fileName);
                            _downloadNormalFile = string.Format("{0}{1}.txt", plib.DownloadMemoUrl, _fileName);

                            //clear the previous data's
                            FileStream fs = new FileStream(_downloadNormalFile, FileMode.OpenOrCreate, FileAccess.Write);
                            StreamWriter _fsstr = new StreamWriter(fs);
                            _fsstr.WriteLine(string.Empty);
                            _fsstr.Flush(); _fsstr.Close();

                            dt = ds.Tables[0];
                            //update the notepad content for downloading.
                            if (dt.Rows.Count > 0)
                            {
                                StreamWriter str = new StreamWriter(_downloadNormalFile, false, System.Text.Encoding.Default);
                                foreach (DataRow datarow in dt.Rows)
                                {
                                    string row = string.Empty;
                                    foreach (object items in datarow.ItemArray)
                                    {
                                        row += items.ToString() + delemited;
                                    }
                                    str.WriteLine(row.Remove(row.Length - 1, 1));
                                }
                                str.Flush();
                                str.Close();
                            }
                        }
                    }
                }


                //Create Zip File -With Encryption
                //if (!string.IsNullOrWhiteSpace(_memoEncryptLocal) && Directory.Exists(_memoEncryptLocal))
                //{
                //    string targetFile = Path.Combine(plib.DownloadMemoUrl, _EncryptFileName + ".zip");
                //    ZipPath(targetFile, _memoEncryptLocal.Substring(0, _memoEncryptLocal.Length - 1), null, true, "Fullerton!nd!@");
                //}

                //if (!string.IsNullOrWhiteSpace(plib.DownloadMemoUrl) && Directory.Exists(plib.DownloadMemoUrl))
                //{
                //    string targetFile = Path.Combine(plib.MemoDownloadUrl, _NoEncryptFileName + ".zip");
                //    ZipPath(targetFile, plib.DownloadMemoUrl.Substring(0, plib.DownloadMemoUrl.Length - 1), null, true, null);
                //}

                file = string.Format("{0}{1}.zip", _memoLocal, _NoEncryptFileName);
            }
            return File(file, "application/zip", _NoEncryptFileName + ".zip");
        }

        public static void ZipPath(string zipFilePath, string sourceDir, string pattern, bool withSubdirs, string password)
        {
            //FastZip fz = new FastZip();
            //if (password != null)
            //    fz.Password = password;

            //fz.CreateZip(zipFilePath, sourceDir, withSubdirs, pattern);
        }
        #endregion

        #region Status Update
        #region Single Status Update
        // Get loading the Status update Page.
        public ActionResult StatusUpdate()
        {
            return View();
        }


        // Get the Memo details based on the memo Number.
        public JsonResult GetMemoDetails(string MemoNo, string PVNo)
        {
            DataSet ds = new DataSet();
            try
            {
                List<EFT> eftMemoDetails = new List<EFT>();
                ds = db.GetEFTMemoDetails(MemoNo, PVNo, plib.LoginUserId);
                if (ds != null)
                {
                    msg.MessageText = ds.Tables[0].Rows[0]["Message"].ToString();
                    if (msg.MessageText.Trim().Length == 0)
                    {
                        dt = ds.Tables[1];
                        if (dt.Rows.Count > 0)
                        {
                            eftMemoDetails = ConvertDataTable<EFT>(dt);
                        }
                        else
                        {
                            return Json(new { msg, eftMemoDetails }, JsonRequestBehavior.AllowGet);
                        }
                        msg.Clear = "true";
                        return Json(new { msg, eftMemoDetails }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        msg.Clear = "false";
                        return Json(new { msg }, JsonRequestBehavior.AllowGet);
                    }
                }
                return null;
            }
            catch
            {
                msg.Clear = "false";
                msg.MessageText = plib.UnableToProcess;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
        }

        /// Save the Memo Details.
        public JsonResult SaveMemoDetails(EFT eftDetails)
        {
            string MailRes = "", Msg = "", Clear = "";
            string PVId = "", emailId = "", ECFId = "";

            DataTable dt1 = null;
            string xml = "";
            string sheets = "";
            DataSet ds = db.SaveEFTDetails(eftDetails.MemoNo, eftDetails.PVNo, eftDetails.Status, plib.ConvertDate(eftDetails.Date), eftDetails.Remarks, "0", xml, plib.LoginUserId, sheets);
            if (ds != null)
            {
                msg.Clear = ds.Tables[0].Rows[0]["Clear"].ToString().ToLower();
                msg.MessageText = ds.Tables[0].Rows[0]["Message"].ToString();

                if (msg.Clear == "1" || msg.Clear == "true")
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        PVId = ds.Tables[1].Rows[i]["PvId"].ToString();
                        emailId = ds.Tables[1].Rows[i]["emailId"].ToString();
                        ECFId = ds.Tables[1].Rows[i]["EcfId"].ToString();

                    }
                    //Table
                    if (ds.Tables[1].Rows.Count > 0 && eftDetails.Status == "Bounce")
                    {
                        if (emailId != "")
                        {
                            MailRes = MailController.sendusermailfs("FS", "EFT Bounce Email Alert", PVId, "s", new[] { emailId }, "0");
                            if (MailRes == "Sucess")
                            {
                                Msg = "Mail Sent Successfully!";
                                Clear = "true";
                            }
                            else
                            {
                                Msg = "Mail has not been sent!";
                                Clear = "false";
                            }
                        }
                        msg.MessageText = ds.Tables[0].Rows[0]["Message"].ToString() + ", " + Msg;
                        msg.Clear = Clear;
                    }
                    else
                    {

                        DataSet DataForMail = db.GetEecGid_ClaimMode_PayMode(eftDetails.MemoNo, eftDetails.PVNo);
                        if (DataForMail.Tables[0].Rows[0]["Column3"].ToString() == "ERA" && DataForMail.Tables[0].Rows[0]["Column4"].ToString() == "Employee")
                        {
                            string[] data = { emailId };
                            MailRes = MailController.sendusermailfs("FS", "Payment Alert Employee Claim", ECFId, "S", data, PVId);
                        }
                        if (DataForMail.Tables[0].Rows[0]["Column3"].ToString() == "EFT" && DataForMail.Tables[0].Rows[0]["Column4"].ToString() == "Supplier")
                        {
                            if (DataForMail.Tables[0].Rows[0]["Column6"].ToString() == "C")
                            {
                                string[] data = { DataForMail.Tables[0].Rows[0]["Column5"].ToString(), DataForMail.Tables[0].Rows[0]["Column1"].ToString(), "corepayment@fullertonindia.com" };
                                MailRes = MailController.sendusermailfs("FS", "Payment Alert Vendor Claim", ECFId, "S", data, PVId);
                            }
                            else
                            {
                                string[] data = { DataForMail.Tables[0].Rows[0]["Column5"].ToString(), DataForMail.Tables[0].Rows[0]["Column1"].ToString() };
                                MailRes = MailController.sendusermailfs("FS", "Payment Alert Vendor Claim", ECFId, "S", data, PVId);
                            }
                        }
                        if (MailRes == "Sucess")
                        {
                            Msg = "Mail Sent Successfully!";
                            Clear = "true";
                        }
                        else
                        {
                            if (MailRes == "Email id is Invalid")
                            {
                                Msg = MailRes;
                                Clear = "false";
                            }
                            else if (MailRes == "template Not match")
                            {
                                Msg = MailRes;
                                Clear = "false";
                            }
                            else
                            {
                                Msg = "Mail has not been sent!";
                                Clear = "false";
                            }
                        }
                        msg.MessageText = ds.Tables[0].Rows[0]["Message"].ToString() + ", " + Msg;
                        msg.Clear = Clear;
                    }

                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(msg, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                msg.Clear = "false";
                msg.MessageText = plib.UnableToProcess;
                return Json(msg, JsonRequestBehavior.AllowGet);
            }

        }
        #endregion

        #region Bulk Status Update
        /// Get the excel file save in the Temp File.
        [HttpPost]
        public void Upload()
        {
            try
            {
                Session["UploadedExcelFile"] = null;
                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase hpfBase = Request.Files[file] as HttpPostedFileBase;
                    Session["UploadedExcelFile"] = hpfBase;
                }
            }
            catch (Exception)
            {
            }
        }

        // Bulk Uplaod using the excel file. Get the Excel file data as dataset, convert them into xml data and finally save the data to the database.        
        public JsonResult BulkUploadStatusUpdate(string MemoNo, string PVNo, string Status, string Date, string Remarks, string ViewType, string PvDate)
        {
            try
            {
                string Data1 = "", Data2 = "", Xml = ""; DataSet Ds; DataSet ds2;
                string Extension1 = ""; string mailout = ""; string Msg = "Mail template Not match!";

                if (Session["UploadedExcelFile"] != null)
                {
                    HttpPostedFileBase _attFile = Session["UploadedExcelFile"] as HttpPostedFileBase;
                    string Extension = Path.GetFileName(_attFile.FileName);
                    Extension1 = System.IO.Path.GetExtension(_attFile.FileName);
                    try
                    {
                        if (Extension1 == ".xlsx" || Extension1 == ".xls" || _attFile.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" || _attFile.ContentType == "application/vnd.ms-excel")
                        {
                            Stream stream = _attFile.InputStream;
                            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                            excelReader.IsFirstRowAsColumnNames = true;
                            Ds = excelReader.AsDataSet();
                            //Ds.Tables[0].Columns.Add("MemoNo");
                            //  Ds.Tables[0].Columns.Add("MemoDate");
                            //dt.Columns.Add("MemoDate");
                            //dt.Columns.Add("MemoNo");
                            excelReader.Close();
                            if (Ds != null && Ds.Tables[0].Rows.Count != 0)
                            {
                                Xml = Ds.GetXml();
                                if (Ds.Tables[0].Rows.Count > 0) Data2 = JsonConvert.SerializeObject(Ds.Tables[0]);
                                //return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                                DataTable dd = Ds.Tables[0];
                                string sheets = Ds.Tables[0].TableName.ToString();
                                sheets = "NewDataSet" + "/" + sheets;
                                ds2 = db.SaveEFTDetails(MemoNo, PVNo, Status, Date, Remarks, ViewType, Xml, plib.LoginUserId, sheets);
                                //to send mail
                                if (ds2.Tables[0].Rows[0]["Clear"].ToString() == "True")
                                {
                                    for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                                    {
                                        if (Ds.Tables[0].Rows[i]["Status"].ToString().ToUpper() == "CLEARED")
                                        {
                                            DataSet DataForMail = db.GetEecGid_ClaimMode_PayMode("", Ds.Tables[0].Rows[i]["PVNo"].ToString());
                                            if (DataForMail.Tables[0].Rows[0]["Column3"].ToString() == "ERA" && DataForMail.Tables[0].Rows[0]["Column4"].ToString() == "Employee")
                                            {
                                                string[] data = { DataForMail.Tables[0].Rows[0]["Column1"].ToString() };
                                                mailout = MailController.sendusermailfs("FS", "Payment Alert Employee Claim", DataForMail.Tables[0].Rows[0]["Column2"].ToString(), "S", data, DataForMail.Tables[0].Rows[0]["Column7"].ToString());
                                            }

                                            if (DataForMail.Tables[0].Rows[0]["Column3"].ToString() == "EFT" && DataForMail.Tables[0].Rows[0]["Column4"].ToString() == "Supplier")
                                            {
                                                if (DataForMail.Tables[0].Rows[0]["Column6"].ToString() == "C")
                                                {
                                                    string[] data = { DataForMail.Tables[0].Rows[0]["Column5"].ToString(), DataForMail.Tables[0].Rows[0]["Column1"].ToString(), "corepayment@fullertonindia.com" };
                                                    mailout = MailController.sendusermailfs("FS", "Payment Alert Vendor Claim", DataForMail.Tables[0].Rows[0]["Column2"].ToString(), "S", data, DataForMail.Tables[0].Rows[0]["Column7"].ToString());
                                                }
                                                else
                                                {
                                                    string[] data = { DataForMail.Tables[0].Rows[0]["Column5"].ToString(), DataForMail.Tables[0].Rows[0]["Column1"].ToString() };
                                                    mailout = MailController.sendusermailfs("FS", "Payment Alert Vendor Claim", DataForMail.Tables[0].Rows[0]["Column2"].ToString(), "S", data, DataForMail.Tables[0].Rows[0]["Column7"].ToString());
                                                }
                                            }
                                            dt = ds2.Tables[0];
                                            if (mailout == "Sucess")
                                            {
                                                Msg = "Mail Sent Successfully!";
                                            }
                                            else
                                            {
                                                if (mailout == "Email id is Invalid")
                                                {
                                                    Msg = mailout;
                                                }
                                                else if (mailout == "template Not match")
                                                {
                                                    Msg = mailout;

                                                }
                                                else
                                                {
                                                    Msg = "Mail has not been sent!";

                                                }
                                            }
                                        }
                                    }
                                    DataTable dt2 = new DataTable();
                                    dt2.Columns.Add("Message", typeof(string));
                                    dt2.Columns.Add("Clear", typeof(string));
                                    dt2.Rows.Add(new object[] { ds2.Tables[0].Rows[0]["Message"].ToString() + " " + Msg, "false" });
                                    if (dt2.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt2); }
                                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    DataTable dt2 = new DataTable();
                                    dt2.Columns.Add("Message", typeof(string));
                                    dt2.Columns.Add("Clear", typeof(string));
                                    dt2.Rows.Add(new object[] { ds2.Tables[0].Rows[0]["Message"].ToString() + " " + Msg, "false" });
                                    if (dt2.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt2); }
                                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                                }
                            }
                            else
                            {
                                DataTable dt2 = new DataTable();
                                dt2.Columns.Add("Message", typeof(string));
                                dt2.Columns.Add("Clear", typeof(string));
                                dt2.Rows.Add(new object[] { "Excel file should not be empty!", "false" });
                                if (dt2.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt2); }
                            }
                        }
                    }
                    catch
                    {
                        DataTable dt2 = new DataTable();
                        dt2.Columns.Add("Message", typeof(string));
                        dt2.Columns.Add("Clear", typeof(string));
                        dt2.Rows.Add(new object[] { "Please upload the valid file!", "false" });
                        if (dt2.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt2); }
                        return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    DataTable dt2 = new DataTable();
                    dt2.Columns.Add("Message", typeof(string));
                    dt2.Columns.Add("Clear", typeof(string));
                    dt2.Rows.Add(new object[] { "Upload only Excel file(.xlsx/ .xls extension)!", "false" });

                    if (dt2.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt2); }
                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                }
                if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }
                return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                ErrorLog objErrorLog = new ErrorLog();
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return null;
            }
        }
        #endregion
        #endregion

        #region RejectedMails
        public ActionResult RejectionMails()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetSingleRejectionDetails(string BounceDateFrom, string BounceDateTo, string PayMode, string Amount, string EmpCodeId, string EmpNameId, string SuppCodeId, string SuppNameId)
        {
            try
            {
                string Data1 = "", Data2 = "";
                DataSet ds = db.GetSingleRejectionDetails(plib.ConvertDate(BounceDateFrom), plib.ConvertDate(BounceDateTo), PayMode, Amount == null || Amount == "" ? "0" : Amount, EmpCodeId, EmpNameId, SuppCodeId, SuppNameId, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0) { Data2 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1, Data2 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        public JsonResult GetBulkRejectionDetails(string BounceDate, string Paymode)
        {
            try
            {
                string Data1 = "";
                DataSet ds = db.GetEFTBulkRejectionMails(plib.ConvertDate(BounceDate), Paymode, plib.LoginUserId);
                if (ds != null)
                {
                    dt = ds.Tables[0];
                    if (dt.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt); }

                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);
                }
                else { return null; }
            }
            catch
            {
                return null;
            }
        }
        #endregion

        //Email
        [HttpPost]
        public JsonResult GetSingleRejectionEmailAlert(string BounceDateFrom, string BounceDateTo, string PayMode, string Amount, string EmpCodeId, string EmpNameId, string SuppCodeId, string SuppNameId)
        {
            string MailRes = "", MailTempbit = "", Data1 = "", Msg = "", Clear = "";
            int SuccCount = 0, FailCount = 0;

            try
            {
                DataSet ds = db.GetSingleRejectionDetails(plib.ConvertDate(BounceDateFrom), plib.ConvertDate(BounceDateTo), PayMode, Amount == null || Amount == "" ? "0" : Amount, EmpCodeId, EmpNameId, SuppCodeId, SuppNameId, plib.LoginUserId);
                if (ds != null)
                {
                    //Table
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            if (ds.Tables[1].Rows[i]["emailId"].ToString() != "")
                            {
                                MailRes = MailController.sendusermailfs("FS", "EFT Rejection Email Alert", ds.Tables[1].Rows[i]["PvId"].ToString(), "s", new[] { ds.Tables[1].Rows[i]["emailId"].ToString() }, "0");
                                if (MailRes == "Sucess")
                                {
                                    SuccCount++;
                                    MailTempbit = "1";
                                    Msg = "Mail Sent Successfully!";
                                    Clear = "true";

                                }
                                else
                                {
                                    FailCount++;
                                    if (MailTempbit == "1") { }
                                    else
                                    {
                                        MailTempbit = "0";
                                        Msg = "Mail has not been sent!";
                                        Clear = "false";
                                    }
                                };
                            }
                            else
                            {
                                FailCount++;
                                if (MailTempbit == "")   //for email id not available
                                {
                                    MailTempbit = "0";
                                    Msg = "Mail has not been sent!";
                                    Clear = "false";
                                }
                            }
                        }
                    }
                    else
                    {
                        Msg = "Data not available for mail sending!";
                        Clear = "false";
                    }

                    DataTable dt2 = new DataTable();
                    dt2.Columns.Add("Message", typeof(string));
                    dt2.Columns.Add("Clear", typeof(string));
                    dt2.Rows.Add(new object[] { Msg + "  Total Record:" + ds.Tables[1].Rows.Count + ", Success Mail:" + SuccCount + ", Failure Mail:" + FailCount, Clear });
                    if (dt2.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt2); }
                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);

                }
                else
                { //TempData["PaymentAdvicePrint"] = null;
                }
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }

        //Email bulk

        [HttpPost]
        public JsonResult GetBulkRejectionDetailsEmailAlert(string BounceDate, string Paymode)
        {
            string MailRes = "", MailTempbit = "", Data1 = "", Msg = "", Clear = "";
            int SuccCount = 0, FailCount = 0;

            try
            {
                DataSet ds = db.GetEFTBulkRejectionMails(plib.ConvertDate(BounceDate), Paymode, plib.LoginUserId);
                if (ds != null)
                {
                    //Table
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            if (ds.Tables[1].Rows[i]["EmailId"].ToString() != "")
                            {
                                MailRes = MailController.sendusermailfs("FS", "EFT Rejection Email Alert", ds.Tables[1].Rows[i]["PvId"].ToString(), "s", new[] { ds.Tables[1].Rows[i]["EmailId"].ToString() }, "0");
                                if (MailRes == "Sucess")
                                {
                                    SuccCount++;
                                    MailTempbit = "1";
                                    Msg = "Mail Sent Successfully!";
                                    Clear = "true";

                                }
                                else
                                {
                                    FailCount++;
                                    if (MailTempbit == "1") { }
                                    else
                                    {
                                        MailTempbit = "0";
                                        Msg = "Mail has not been sent!";
                                        Clear = "false";
                                    }
                                };
                            }
                            else
                            {
                                FailCount++;
                                if (MailTempbit == "")   //for email id not available
                                {
                                    MailTempbit = "0";
                                    Msg = "Mail has not been sent!";
                                    Clear = "false";
                                }
                            }
                        }
                    }
                    else
                    {
                        Msg = "Data not available for mail sending!";
                        Clear = "false";
                    }

                    DataTable dt2 = new DataTable();
                    dt2.Columns.Add("Message", typeof(string));
                    dt2.Columns.Add("Clear", typeof(string));
                    dt2.Rows.Add(new object[] { Msg + "  Total Record:" + ds.Tables[1].Rows.Count + ", Success Mail:" + SuccCount + ", Failure Mail:" + FailCount, Clear });
                    if (dt2.Rows.Count > 0) { Data1 = JsonConvert.SerializeObject(dt2); }
                    return Json(new { Data1 }, JsonRequestBehavior.AllowGet);

                }
                else
                { //TempData["PaymentAdvicePrint"] = null;
                }
                return Json("Error", JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return null;
            }
        }
        [HttpPost]
        public JsonResult DownloadTemplate()
        {
            try
            {
                using (XLWorkbook wb = new XLWorkbook())
                {
                    DataTable dt = new DataTable("Temp");
                    dt.Columns.Add("Sno", typeof(Int32));
                    //dt.Columns.Add("MemoDate");
                    //dt.Columns.Add("MemoNo");
                    dt.Columns.Add("PVNo", typeof(string));
                    // dt.Columns.Add("Amount");
                    dt.Columns.Add("Status", typeof(string));
                    dt.Columns.Add("Cleared_StatusDate", typeof(DateTime));
                    dt.Columns.Add("Reason", typeof(string));
                    wb.Worksheets.Add(dt);
                    wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    wb.Style.Font.Bold = true;


                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";

                    Response.AddHeader("content-disposition", "attachment;filename=EFTStatusUpdateTemplate.xls");
                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                    }
                    return Json("", JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Convert Datatable to List
        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItem<T>(row);
                data.Add(item);
            }
            return data;
        }

        public static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }

        #endregion
    }
}
