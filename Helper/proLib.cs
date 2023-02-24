using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;


namespace IEM.Helper
{
    public class proLib
    {
        #region Variables
        string LogFile = HttpContext.Current.Server.MapPath("ErrorLogs.txt");
        public Sessions sessions = new Sessions();
        #endregion

        #region Constructors
        public proLib()
        {
        }
        #endregion

        #region Error Log Methods
        public void CreateLog(string FileName, string MethodName, string ErrMsg)
        {
            try
            {
                //if (EnableLogs == false || (string.IsNullOrEmpty(ErrMsg.Trim()))) return;
                if (string.IsNullOrEmpty(ErrMsg.Trim())) return;

                FileStream fs = new FileStream(LogFile, FileMode.OpenOrCreate, FileAccess.Write);
                StreamWriter sw = new StreamWriter(fs);
                sw.BaseStream.Seek(0, SeekOrigin.End);

                sw.WriteLine(DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt --> ") + "File: " + FileName + ", Method: " + MethodName + ", Error: " + ErrMsg);
                sw.Flush();
                sw.Close();
            }
            catch { }
        }

        void SetErrorLog(string _method, string _errMsg)
        {
            CreateLog("proLib", _method, _errMsg);
        }
        #endregion

        #region Date Conversion
        public string ConvertDate(string _date)
        {
            //if (_date.Contains('/'))
            //{
            //    _date = _date.Replace('/', '-');
            //}
            if (_date != null && _date.Trim() != string.Empty)
            {
                try
                {
                    return _date.Split('/')[1] + "/" + _date.Split('/')[0] + "/" + _date.Split('/')[2];
                }
                catch
                {
                    return "";
                }

            }
            else { return ""; }
        }

        public string ConvertDateFormat(string _date)
        {
            //if (_date.Contains('/'))
            //{
            //    _date = _date.Replace('/', '-');
            //}
            if (_date != null && _date.Trim() != string.Empty)
            {
                try
                {
                    return _date.Split('/')[0] + "/" + _date.Split('/')[1] + "/" + _date.Split('/')[2];
                }
                catch
                {
                    return "";
                }

            }
            else { return ""; }
        }

        public bool DateValidation(string _dateFrom, string _dateTo)
        {
            try
            {
                //if (_dateFrom.Contains('/'))
                //{
                //    _dateFrom = _dateFrom.Replace('/', '-');
                //}
                //if (_dateTo.Contains('/'))
                //{
                //    _dateTo = _dateTo.Replace('/', '-');
                //}

                DateTime dtFrm = Convert.ToDateTime(_dateFrom);
                DateTime dtTo = Convert.ToDateTime(_dateTo);

                double days = (dtTo - dtFrm).TotalDays;

                if (days < 0)
                {
                    return false;
                }
                else { return true; }
            }
            catch { return false; }
        }

        public string ConvertDateMaster(string _date)
        {
            if (_date != null && _date.Trim() != string.Empty)
            {
                try
                {
                    return _date.Split('-')[1] + "/" + _date.Split('-')[0] + "/" + _date.Split('-')[2];
                }
                catch
                {
                    return "";
                }

            }
            else { return ""; }
        }
        #endregion

        #region Properties
        public string UnableToProcess
        {
            get { return "Unable to process your request! Please Try Again!"; }
        }

        public string DateValidationText
        {
            get { return "Ensure Valid DateFrom & DateTo!"; }
        }

        public string NoRecordFound
        {
            get { return "No Records found!"; }
        }

        public string GetMonth(string monthName)
        {
            string strMonth = "";
            string[] MonthNames = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            for (int i = 0; i < MonthNames.Length; i++)
            {
                if (MonthNames[i].Contains(monthName))
                {
                    strMonth = (i + 1).ToString();
                }
            }
            return strMonth;
        }
        #endregion

        #region Sessions Methods
        public string LoginUserId
        {
            get { string x = sessions.Get("employee_gid"); return (x.Trim() == "") ? "0" : x; }
            set { sessions.Add("employee_gid", value); }
        }

        public string PrefixUrl
        {
            get
            {
                string x = "";
                try
                {
                    x = System.Configuration.ConfigurationManager.AppSettings["PrefixUrl"].ToString();
                }
                catch { x = ""; }
                return (x == null || x.Trim() == "") ? "" : x;
            }
        }

        public string HoldFileUploadUrl
        {
            get
            {
                string x = "";
                try
                {
                    x = System.Configuration.ConfigurationManager.AppSettings["HoldFileUpload"].ToString();
                }
                catch { x = ""; }
                return (x == null || x.Trim() == "") ? "" : x;
            }
        }
        public string EFTDownloadPath
        {
            get
            {
                string x = "";
                try
                {
                    x = System.Configuration.ConfigurationManager.AppSettings["EFTDownloadPath"].ToString();
                }
                catch { x = ""; }
                return (x == null || x.Trim() == "") ? "" : x;
            }
        }

        public string ChqStatusUpdateFileUploadUrl
        {
            get
            {
                string x = "";
                try
                {
                    x = System.Configuration.ConfigurationManager.AppSettings["ChqStatusUpdateFileUpload"].ToString();
                }
                catch { x = ""; }
                return (x == null || x.Trim() == "") ? "" : x;
            }
        }

        public string ChqDespatchAWBUpdationXLUrl
        {
            get
            {
                string x = "";
                try
                {
                    x = System.Configuration.ConfigurationManager.AppSettings["ChqDespatchAWBUpdationXL"].ToString();
                }
                catch { x = ""; }
                return (x == null || x.Trim() == "") ? "" : x;
            }
        }

        public string ExcelDebitLineUrl
        {
            get
            {
                string x = "";
                try
                {
                    x = System.Configuration.ConfigurationManager.AppSettings["excelDebitLine"].ToString();
                }
                catch { x = ""; }
                return (x == null || x.Trim() == "") ? "" : x;
            }
        }

        public string StaleChqCheckerAtachment
        {
            get
            {
                string x = "";
                try
                {
                    x = System.Configuration.ConfigurationManager.AppSettings["StaleChqCheckerAttachment"].ToString();
                }
                catch { x = ""; }
                return (x == null || x.Trim() == "") ? "" : x;
            }
        }

        public string ProvisionMappingUploadUrl
        {
            get
            {
                string x = "";
                try
                {
                    x = System.Configuration.ConfigurationManager.AppSettings["ProvisionMappingUploadUrl"].ToString();
                }
                catch { x = ""; }
                return (x == null || x.Trim() == "") ? "" : x;
            }
        }

        public string PrepaidExtensionUrl
        {
            get
            {
                string x = "";
                try
                {
                    x = System.Configuration.ConfigurationManager.AppSettings["PrepaidExtensionUrl"].ToString();
                }
                catch { x = ""; }
                return (x == null || x.Trim() == "") ? "" : x;
            }
        }

        public string MemoDownloadUrl
        {
            get
            {
                string x = "";
                try
                {
                    x = System.Configuration.ConfigurationManager.AppSettings["MemoLocal"].ToString();
                }
                catch { x = ""; }
                return (x == null || x.Trim() == "") ? "" : x;
            }
        }

        public string EncryptMemoDownloadUrl
        {
            get
            {
                string x = "";
                try
                {
                    x = System.Configuration.ConfigurationManager.AppSettings["MemoLocalEncrypt"].ToString();
                }
                catch { x = ""; }
                return (x == null || x.Trim() == "") ? "" : x;
            }
        }

        public string DownloadMemoUrl
        {
            get
            {
                string x = "";
                try
                {
                    x = System.Configuration.ConfigurationManager.AppSettings["DownloadMemoLocal"].ToString();
                }
                catch { x = ""; }
                return (x == null || x.Trim() == "") ? "" : x;
            }
        }

        public string ChequePrintDownloadUrl
        {
            get
            {
                string x = "";
                try
                {
                    x = System.Configuration.ConfigurationManager.AppSettings["ChequePrintingLocal"].ToString();
                }
                catch { x = ""; }
                return (x == null || x.Trim() == "") ? "" : x;
            }
        }

        public string ApprovalSummaryDownloadUrl
        {
            get
            {
                string x = "";
                try
                {
                    x = System.Configuration.ConfigurationManager.AppSettings["ApprovalSummaryDownloadUrl"].ToString();
                }
                catch { x = ""; }
                return (x == null || x.Trim() == "") ? "" : x;
            }
        }
        #endregion

       
    }

    public class Sessions
    {
        public void Add(string key, string data)
        {
            try { HttpContext.Current.Session.Add(key, data); }
            catch { }
        }

        public string Get(string key)
        {
            try
            {
                if (HttpContext.Current.Session[key] != null)
                {
                    return HttpContext.Current.Session[key].ToString();
                }
            }
            catch { }
            return "";
        }

        public bool Remove(string key)
        {
            try
            {
                if (HttpContext.Current.Session[key] != null)
                {
                    HttpContext.Current.Session.Remove(key);
                    return true;
                }
            }
            catch { }
            return false;
        }
    }

    public sealed class GetData
    {
        public string Data1 { get; set; }
        public string Data2 { get; set; }
        public string Data3 { get; set; }
        public string Data4 { get; set; }
        public string Data5 { get; set; }
        public string Data6 { get; set; }
        public string Data7 { get; set; }
        public string Data8 { get; set; }
        public string Data9 { get; set; }
        public string Data10 { get; set; }
    }

    #region

    #endregion
}