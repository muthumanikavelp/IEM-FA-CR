using System;
using System.Security.Principal;
using System.Runtime.InteropServices;
using System.ComponentModel;
using IEM.Common;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
namespace IEM.Common
{
    public class FileServer
    {
        ErrorLog objErrorLog = new ErrorLog();
        HttpClient client = new HttpClient();


        #region enumtypes
        enum LogonType
        {
            Interactive = 2,
            Network = 3,
            Batch = 4,
            Service = 5,
            Unlock = 7,
            NetworkClearText = 8,
            NewCredentials = 9
        }

        private string DomainName = System.Configuration.ConfigurationManager.AppSettings["DomainName"].ToString(),//"FCHVERPIMG01",
                       DomainUser = System.Configuration.ConfigurationManager.AppSettings["DomainUser"].ToString(),//"P110741",
                       DomainPwd = System.Configuration.ConfigurationManager.AppSettings["DomainPwd"].ToString();//"Pa$$word147";
        enum LogonProvider
        {
            Default = 0,
            WinNT35 = 1,
            WinNT40 = 2,
            WinNT50 = 3
        }
        #endregion
        #region publicmethods
        public string SaveFiles(byte[] byteimg, string filename)
        {
            try
            {
                string result = "";
                IntPtr token = IntPtr.Zero;
                LogonUser(DomainUser,
                            DomainName,
                            DomainPwd,
                            (int)LogonType.NewCredentials,
                            (int)LogonProvider.WinNT50,
                            ref token);

                using (WindowsImpersonationContext context = WindowsIdentity.Impersonate(token))
                {
                    CloseHandle(token);
                    if (System.IO.File.Exists(System.Configuration.ConfigurationManager.AppSettings["Filepath"].ToString() + filename))
                        System.IO.File.Delete(System.Configuration.ConfigurationManager.AppSettings["Filepath"].ToString() + filename);
                    System.IO.File.WriteAllBytes(System.Configuration.ConfigurationManager.AppSettings["Filepath"].ToString() + filename, byteimg);
                    result = "success";
                }
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
        }
        public string DeleteFiles(string filename)
        {
            try
            {
                string result = "";
                IntPtr token = IntPtr.Zero;
                LogonUser(DomainUser,
                            DomainName,
                            DomainPwd,
                            (int)LogonType.NewCredentials,
                            (int)LogonProvider.WinNT50,
                            ref token);

                using (WindowsImpersonationContext context = WindowsIdentity.Impersonate(token))
                {
                    CloseHandle(token);
                    if (System.IO.File.Exists(System.Configuration.ConfigurationManager.AppSettings["Filepath"].ToString() + filename))
                        System.IO.File.Delete(System.Configuration.ConfigurationManager.AppSettings["Filepath"].ToString() + filename);
                    result = "success";
                }
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
        }
        public int CheckFileExists(string filename)
        {
            try
            {
                int result = 0;
                IntPtr token = IntPtr.Zero;
                LogonUser(DomainUser,
                            DomainName,
                            DomainPwd,
                            (int)LogonType.NewCredentials,
                            (int)LogonProvider.WinNT50,
                            ref token);

                using (WindowsImpersonationContext context = WindowsIdentity.Impersonate(token))
                {
                    CloseHandle(token);
                    if (System.IO.File.Exists(System.Configuration.ConfigurationManager.AppSettings["Filepath"].ToString() + filename))
                        result = 1;
                }
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
        }
        public byte[] CopyFiles(string filename)
        {
            byte[] bytimg = null;
            try
            {
                IntPtr token = IntPtr.Zero;
                LogonUser(DomainUser,
                            DomainName,
                            DomainPwd,
                            (int)LogonType.NewCredentials,
                            (int)LogonProvider.WinNT50,
                            ref token);

                using (WindowsImpersonationContext context = WindowsIdentity.Impersonate(token))
                {
                    CloseHandle(token);
                    if (System.IO.File.Exists(System.Configuration.ConfigurationManager.AppSettings["Filepath"].ToString() + filename))
                        bytimg = System.IO.File.ReadAllBytes(System.Configuration.ConfigurationManager.AppSettings["Filepath"].ToString() + filename);
                }
                return bytimg;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return bytimg;
            }
        }
        public string CreateDirectoryNew(string foldername, string folderfor = null)
        {
            try
            {
                IntPtr token = IntPtr.Zero;
                LogonUser(DomainUser,
                            DomainName,
                            DomainPwd,
                            (int)LogonType.NewCredentials,
                            (int)LogonProvider.WinNT50,
                            ref token);

                using (WindowsImpersonationContext context = WindowsIdentity.Impersonate(token))
                {
                    CloseHandle(token);
                    if (folderfor == null || folderfor == "")
                    {
                        if (!System.IO.Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["Filepath"].ToString() + foldername + "\\"))
                            System.IO.Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["Filepath"].ToString() + foldername + "\\");
                    }
                    else if (folderfor == "main")
                    {
                        if (System.IO.Directory.Exists(System.Configuration.ConfigurationManager.AppSettings["Filepath"].ToString() + foldername + "\\"))
                            System.IO.Directory.Delete(System.Configuration.ConfigurationManager.AppSettings["Filepath"].ToString() + foldername + "\\");
                        System.IO.Directory.CreateDirectory(System.Configuration.ConfigurationManager.AppSettings["Filepath"].ToString() + foldername + "\\");
                    }

                }
                return "success";
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "error";
            }
        }
        public string MoveFilesECF(string foldername, string filename)
        {
            try
            {
                string result = "";
                IntPtr token = IntPtr.Zero;
                LogonUser(DomainUser,
                            DomainName,
                            DomainPwd,
                            (int)LogonType.NewCredentials,
                            (int)LogonProvider.WinNT50,
                            ref token);

                using (WindowsImpersonationContext context = WindowsIdentity.Impersonate(token))
                {
                    CloseHandle(token);
                    if (System.IO.File.Exists(System.Configuration.ConfigurationManager.AppSettings["Filepath"].ToString() + filename))
                    {
                        System.IO.File.Copy(System.Configuration.ConfigurationManager.AppSettings["Filepath"].ToString() + filename, System.Configuration.ConfigurationManager.AppSettings["Filepath"].ToString() + foldername + "\\" + filename);
                    }
                    // System.IO.File.Delete(@"\\192.168.0.106\IEMAttachments\" + filename);
                    //  System.IO.File.WriteAllBytes(@"\\192.168.0.106\IEMAttachments\" + filename, byteimg);
                    result = "success";
                }
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
        }
        public byte[] ECFDocs(DataTable dt1)
        {
            byte[] bytimg = null;
            try
            {
                IntPtr token = IntPtr.Zero;
                LogonUser(DomainUser,
                            DomainName,
                            DomainPwd,
                            (int)LogonType.NewCredentials,
                            (int)LogonProvider.WinNT50,
                            ref token);
                string empcode = string.Empty;
                if (HttpContext.Current.Session["employee_code"] != null)
                {
                    empcode = HttpContext.Current.Session["employee_code"].ToString();
                }
               // string timestring = "ECF_and_Docs_" + empcode + System.DateTime.Today.ToShortDateString() + System.DateTime.Today.ToShortTimeString();
                string timestring = "ECF_and_Docs_" + empcode + System.DateTime.Today.ToShortDateString().Replace(":", "") + System.DateTime.Today.ToShortTimeString().Replace(":", "");
                using (WindowsImpersonationContext context = WindowsIdentity.Impersonate(token))
                {
                    CloseHandle(token);
                    IEM.Areas.MASTERS.Controllers.ForMailController fm = new IEM.Areas.MASTERS.Controllers.ForMailController();
                    SqlConnection con = new SqlConnection();
                    DataTable dt = new DataTable();
                    con.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                    con.Open();
                    SqlDataAdapter da = new SqlDataAdapter();
                   // string mainDirectory = timestring + "\\" + timestring;
                    string mainDirectory = timestring;
                    using (var zip = new Ionic.Zip.ZipFile())
                    {
                        string folderLocation = string.Empty;
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            SqlCommand cmd = new SqlCommand("pr_eow_trn_docdownload", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@ecfno", SqlDbType.VarChar).Value = dt1.Rows[i]["ECF Number"].ToString();
                            cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getdocs";
                            dt = new DataTable();
                            da = new SqlDataAdapter(cmd);

                            da.Fill(dt);
                            CreateDirectoryNew(mainDirectory, "main");

                            folderLocation = System.Configuration.ConfigurationManager.AppSettings["Filepath"].ToString() + mainDirectory + "\\" + dt1.Rows[i]["ECF Number"].ToString() + "\\";
                            if (CreateDirectoryNew(mainDirectory + "\\" + dt1.Rows[i]["ECF Number"].ToString()) == "success")
                            {
                                SaveFiles(fm.challanPrintmail(dt.Rows[0]["ecf_gid"].ToString(), dt.Rows[0]["ecf_supplier_employee"].ToString()), dt1.Rows[i]["ECF Number"].ToString() + ".pdf");

                                string fileToCopy = dt1.Rows[i]["ECF Number"].ToString() + ".pdf";
                                MoveFilesECF(mainDirectory + "\\" + dt1.Rows[i]["ECF Number"].ToString(), fileToCopy);
                                if (dt.Rows.Count > 0)
                                {
                                    for (int ii = 0; ii < dt.Rows.Count; ii++)
                                    {
                                        fileToCopy = dt.Rows[ii]["file"].ToString();
                                        MoveFilesECF(mainDirectory + "\\" + dt1.Rows[i]["ECF Number"].ToString(), fileToCopy);
                                    }

                                }

                            }
                        }
                        //foreach (var dire in System.IO.Directory.GetDirectories(@"\\\\192.168.0.106\\IEMAttachments\\" + mainDirectory + "\\"))
                        //{
                        zip.AddDirectory(System.Configuration.ConfigurationManager.AppSettings["Filepath"].ToString() + timestring);
                        //}
                        DeleteFiles(timestring + ".zip");
                        zip.Save(System.Configuration.ConfigurationManager.AppSettings["Filepath"].ToString() + timestring + ".zip");
                    }
                    bytimg = System.IO.File.ReadAllBytes(System.Configuration.ConfigurationManager.AppSettings["Filepath"].ToString() + timestring + ".zip");
                }
                return bytimg;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return bytimg;
            }
        }
        #endregion
        #region dllimport
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool LogonUser(string lpszUsername, string lpszDomain, string lpszPassword, int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr token);
        #endregion

        #region
        // Created By Dhasarathan 13oct2016
        public async virtual Task<string> SaveFileToServer(string FileString, string FileName)
        {

            try
            {
                RequestData RequestData = new RequestData();
                string result;
                RequestData.FileName = FileName;
                RequestData.FileString = FileString;
                string url = FileServerWebApi();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.PostAsJsonAsync("api/Upload", RequestData).ConfigureAwait(false);
                if (responseMessage.IsSuccessStatusCode)

                    result = "Success";

                else
                    result = "Fail";

                return result;

            }
            catch (Exception ex)
            {

                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "error";

            }

        }

        public async virtual Task<string> DownloadFile(string filename)
        {

            try
            {
                byte[] filebyte;
                string url = FileServerWebApi();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage responseMessage = await client.GetAsync("api/Upload?filename=" + filename).ConfigureAwait(false);
                if (responseMessage.IsSuccessStatusCode)
                {
                    var responseData = responseMessage.Content.ReadAsStringAsync().Result;
                    var RequestData = JsonConvert.DeserializeObject<string>(responseData);
                    filebyte = Convert.FromBase64String(RequestData);
                    return RequestData;
                }
                else
                    return "Fail";


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "error";
            }


        }

        public string FileServerWebApi()
        {
            string x = "";
            try
            {
                x = System.Configuration.ConfigurationManager.AppSettings["WebApiFileServer"].ToString();
                //x = "http://192.168.0.135/upload/"; 

            }
            catch { x = ""; }
            return (x == null || x.Trim() == "") ? "" : x;
        }

        // Created By Dhasarathan 13oct2016
        #endregion

    }
    // Created By Dhasarathan 13oct2016
    public class RequestData
    {
        public string FileString { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
    }
    // Created By Dhasarathan 13oct2016

}