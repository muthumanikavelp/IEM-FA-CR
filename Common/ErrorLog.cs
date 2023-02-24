using System.Configuration;
using System.Text;
using System.IO;
using System;
using System.Web;

namespace IEM.Common
{
    public class ErrorLog
    {
        //CmnFunctions objCmnFunctions = new CmnFunctions();
        public bool WriteErrorLog(string ErrDecription, string DetailedErrorMessage) 
        {
            bool Status = false;
            string LogDirectory = ConfigurationManager.AppSettings["LogDirectory"].ToString();

            DateTime CurrentDateTime = DateTime.Now;
            string CurrentDateTimeString = CurrentDateTime.ToString();
            CheckCreateLogDirectory(LogDirectory);
            string logLine = BuildLogLine(CurrentDateTime, ErrDecription, DetailedErrorMessage);
            LogDirectory = (LogDirectory + "ErrorLog_" + LogFileName(DateTime.Now) + ".txt");

            lock (typeof(ErrorLog))
            {
                StreamWriter oStreamWriter = null;
                try
                {
                    oStreamWriter = new StreamWriter(LogDirectory, true);
                    oStreamWriter.WriteLine(logLine);
                    Status = true;
                }
                catch
                {

                }
                finally
                {
                    if (oStreamWriter != null)
                    {
                        oStreamWriter.Close();
                    }
                }
            }
            return Status;
        }


        private bool CheckCreateLogDirectory(string LogPath)
        {
            bool loggingDirectoryExists = false;
            DirectoryInfo oDirectoryInfo = new DirectoryInfo(LogPath);
            if (oDirectoryInfo.Exists)
            {
                loggingDirectoryExists = true;
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(LogPath);
                    loggingDirectoryExists = true;
                }
                catch
                {
                    // Logging failure
                }
            }
            return loggingDirectoryExists;
        }


        private string BuildLogLine(DateTime CurrentDateTime, string ErrDecription, string DetailedErrorMessage)
        {
            StringBuilder loglineStringBuilder = new StringBuilder();
            loglineStringBuilder.Append("-----------------Error Details----------------");
            try
            {                              
                loglineStringBuilder.Append(Environment.NewLine);
                loglineStringBuilder.Append("Date & Time : ");
                // loglineStringBuilder.Append(" \t");
                loglineStringBuilder.Append(LogFileEntryDateTime(CurrentDateTime));

                if (HttpContext.Current != null)
                {
                    loglineStringBuilder.Append(Environment.NewLine);
                    loglineStringBuilder.Append("IP Address : ");
                    // loglineStringBuilder.Append(" \t");
                    var requestIPAddress = HttpContext.Current.Request.UserHostAddress;
                    loglineStringBuilder.Append(requestIPAddress);
                }

                if (HttpContext.Current.Session["EmployeeCodeForErrorLog"] != null)
                {
                    loglineStringBuilder.Append(Environment.NewLine);
                    loglineStringBuilder.Append("Login ID : ");
                    //   loglineStringBuilder.Append(" \t");
                    var EmpCode = (string)HttpContext.Current.Session["EmployeeCodeForErrorLog"];
                    loglineStringBuilder.Append(EmpCode);
                }
                loglineStringBuilder.Append(Environment.NewLine);
                loglineStringBuilder.Append("Error Description : ");
                loglineStringBuilder.Append(ErrDecription);
                loglineStringBuilder.Append(Environment.NewLine);
                loglineStringBuilder.Append("Error Message In Detail : ");
                loglineStringBuilder.Append(Environment.NewLine);
                loglineStringBuilder.Append(" \t");
                loglineStringBuilder.Append(DetailedErrorMessage);
                loglineStringBuilder.Append(Environment.NewLine);
            }
            catch(Exception ex)
            {
                loglineStringBuilder.Append(ex.Message.ToString());
            }
            return loglineStringBuilder.ToString();
        }
        
        public string LogFileEntryDateTime(DateTime CurrentDateTime)
        {
            return CurrentDateTime.ToString("dd-MMM-yyyy HH:mm:ss");
        }


        private string LogFileName(DateTime CurrentDateTime)
        {
            return CurrentDateTime.ToString("dd_MMM_yyyy");
        }
    }
}