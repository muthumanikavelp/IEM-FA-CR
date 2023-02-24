using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Models;
using System.Data;
using IEM.Common;
using System.IO;
using System.DirectoryServices;

namespace IEM.Controllers
{

    public class LoginpageController : Controller
    {
        IEMRepository rep;
        CmnFunctions objCmnFunctions = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();

        public LoginpageController()
            : this(new IEMDataModel())
        {

        }

        public LoginpageController(IEMRepository Irep)
        {
            rep = Irep;
        }
        //[HandleError]
        //protected override void OnException(ExceptionContext ErrContext) 
        //{
        //    base.OnException(ErrContext);

        //    objCmnFunctions.SaveLog(ErrContext.Exception);
        //}


        public ActionResult empLoginPage(string pagefor = null)
        {
            LoginPage lobj = new LoginPage();
            try
            {
                if (pagefor == "logoff")
                {
                    Session["ErrMsg"] = null;
                    if (Session["employee_gid"] != null)
                    {
                        int empgid = objCmnFunctions.GetLoginUserGid();
                        if (Session["employee_code"] != null)
                        {
                            string empcodelogout = (string)Session["employee_code"];
                            rep.insertloginattempt(Request.UserHostAddress, empcodelogout, "LogOut", Request.Browser.Browser);
                        }

                        rep.UpdateLoginFlag(empgid, 0);
                    }
                    else
                    {

                    }

                    Session.RemoveAll();
                    Session.Clear();
                    Session.Abandon();
                    Response.AppendHeader("Cache-Control", "no-store");
                    Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", ""));
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);

                    HttpCookie myCookie = new HttpCookie("__RequestVerificationToken");
                    myCookie.Expires = DateTime.Now.AddDays(-1d);
                    Response.Cookies.Add(myCookie);                    
                }
                Session["login"] = string.Empty;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return View(lobj);

        }

        //*********************************************** AD login things**************************************************//

        [HttpPost]
        public ActionResult empLoginPage(LoginPage lobj, string employeelogintype)
        {
            string ReturnAction = string.Empty;
            Session["IpAddress"] = Request.UserHostAddress;
            try
            {
                Emp.SessionStatus = 1;
                DataTable dt = new DataTable();
                var result = "0";
                string loginstatus = "";
                string OldIPAddress = "";
                string OldBrowser = "";
                Session["ErrMsg"] = null;
                Session["employee_name"] = null;
                Session["employee_code"] = null;
                string dominName = string.Empty;
                string adPath = string.Empty;
                string userName = lobj.employeeId.Trim().ToUpper();
                Session["EmployeeCodeForErrorLog"] = lobj.employeeId.ToUpper().Trim();
                string strError = string.Empty;
                string LoginFor = System.Configuration.ConfigurationManager.AppSettings["LoginFor"].ToString();
                if (LoginFor == "production")
                {
                    string autheticate = "";
                    foreach (string key in System.Configuration.ConfigurationManager.AppSettings.Keys)
                    {
                        dominName = key.Contains("DirectoryDomain") ? System.Configuration.ConfigurationManager.AppSettings[key] : dominName;
                        adPath = key.Contains("DirectoryPath") ? System.Configuration.ConfigurationManager.AppSettings[key] : adPath;
                        if (!String.IsNullOrEmpty(dominName) && !String.IsNullOrEmpty(adPath))
                        {
                            if (true == AuthenticateUser(dominName, userName, lobj.passward, adPath, out strError))
                            {
                                result = "1";
                            }
                            else
                            {
                                result = "0";
                            }
                            dominName = string.Empty;
                            adPath = string.Empty;
                            Session["Authenticate"] = "";
                            if (String.IsNullOrEmpty(strError)) break;
                        }
                    }
                    if (!string.IsNullOrEmpty(strError))
                    {
                        autheticate = "Invalid user name or Password!";
                        Session["Authenticate"] = autheticate;
                        ViewBag.ErrMsg = "Invalid user name / Password!";
                        result = "0";
                    }
                    Session["DashSearchItems"] = null;
                    Session["DashSearchItemsapp"] = null;
                    Session["DashSearchItems_fb"] = null;
                    Session["DashSearchItemsapp_fb"] = null;
                    Session["SearchData"] = null;
                    Session["SearcECFCancellationRecord"] = null;
                    Session["SearcECFDeletionRecord"] = null;
                    Session["SearchECfDespatchData"] = null; 
                    DataTable dtproxy = new DataTable(); 
                    Session["EmployeeCodeForErrorLog"] = lobj.employeeId.ToUpper().Trim();
                    Session["ErrMsg"] = null; 
                    dt = rep.GetLoginUserDetails(lobj.employeeId.ToUpper().Trim());
                    string Result = rep.AuthorizeEntity(lobj.employeeId.ToUpper().Trim());
                    if (dt.Rows.Count == 0)
                    {
                        loginstatus = "Invalid UserCode/Password";
                        ReturnAction = "../Loginpage/empLoginPage";
                    }
                    else if (employeelogintype == null)
                    {
                        loginstatus = "Invalid UserCode/Password";
                        ReturnAction = "../Loginpage/empLoginPage";
                    }
                    else if (lobj.empname == "0")
                    {
                        loginstatus = "Invalid UserCode/Password";
                        ReturnAction = "../Loginpage/empLoginPage";
                    }
                    if (result != "1")
                    {
                        loginstatus = "Invalid UserCode/Password";
                        ReturnAction = "../Loginpage/empLoginPage";
                    }
                    else
                    {

                        int logincheck = 0;
                        logincheck = rep.CheckDuplicateLogin(lobj.employeeId);
                        if (logincheck < 1)
                        {
                            OldIPAddress = rep.GetPreviousIPAddress(lobj.employeeId);
                            OldBrowser = rep.GetPreviousBrowser(lobj.employeeId);
                            if (OldIPAddress == Request.UserHostAddress)
                            {
                                if (Request.Browser.Browser == OldBrowser)
                                {
                                    rep.ReleaseEmp(lobj.employeeId);
                                    logincheck = 1;
                                }
                                else
                                {
                                    logincheck = -2;
                                }
                            }
                            else
                            {
                                int sessioninterval = rep.GetSessionInterval(lobj.employeeId);
                                if (sessioninterval > 0)
                                    logincheck = 1;
                                else
                                    logincheck = -2;
                            }
                        }
                        if (logincheck > 0)
                        {
                            loginstatus = "Logged In";
                            ReturnAction = "../IEMCommon/Welcome";
                            Session["employee_gid"] = dt.Rows[0]["employee_gid"].ToString();
                            Session["employee_name"] = dt.Rows[0]["employee_name"].ToString();
                            Session["employee_code"] = dt.Rows[0]["employee_code"].ToString();
                            //Static Variables
                            Emp.empcodeST = dt.Rows[0]["employee_code"].ToString();
                            Emp.empgidST = Convert.ToInt32(dt.Rows[0]["employee_gid"].ToString());
                            Emp.IPAddress = Request.UserHostAddress;
                            Emp.BrowserName = Request.Browser.Browser;
                            //
                            Session["login-mode"] = employeelogintype.ToString();
                            Session["Pto"] = "Proxy To";
                            Session["loginraisemode"] = null;
                            Session["Proxyemployee_gid"] = null;
                            if (Session["login-mode"].ToString() == "Proxy")
                            {
                                dtproxy = rep.GetLoginUserDetails(lobj.empname.Trim());
                                Session["Proassuser"] = dtproxy.Rows[0]["employee_name"].ToString() + " [ " + dtproxy.Rows[0]["employee_code"].ToString() + " ] ";
                                Session["Proassusercode"] = dtproxy.Rows[0]["employee_code"].ToString();
                                Session["Proxyemployee_gid"] = dtproxy.Rows[0]["employee_gid"].ToString();
                                Session["loginraisemode"] = "P";
                            }

                            Session["employee_dept"] = dt.Rows[0]["employee_dept_name"].ToString();
                            if (dt.Rows[0]["employee_gender"].ToString() == "M")
                            {
                                Session["Link"] = "/Content/images/Mr1.png";
                            }
                            if (dt.Rows[0]["employee_gender"].ToString() == "F")
                            {
                                Session["Link"] = "/Content/images/Ms1.png";
                            }
                            int empgid = Convert.ToInt32(dt.Rows[0]["employee_gid"].ToString());
                            rep.UpdateLoginFlag(empgid, 1);
                            Session["login"] = "No";
                            if (Request.Cookies["Employee_Code"] != null)
                            {
                                var c = new HttpCookie("Employee_Code");
                                c.Expires = DateTime.Now.AddDays(-1);
                                Response.SetCookie(c);
                            }

                            HttpCookie cookie = new HttpCookie("Employee_Code");
                            cookie.Value = lobj.employeeId.ToUpper().Trim();
                            HttpContext.Response.SetCookie(cookie);
                        }
                        else
                        {
                            loginstatus = "The User[" + lobj.employeeId.ToString().ToUpper() + "] session is active @ " + OldIPAddress + " - " + OldBrowser; 
                            ReturnAction = "../Loginpage/empLoginPage";
                        }
                    }

                }
                else
                {
                    Session["DashSearchItems"] = null;
                    Session["DashSearchItemsapp"] = null;
                    Session["DashSearchItems_fb"] = null;
                    Session["DashSearchItemsapp_fb"] = null;
                    Session["SearchData"] = null;
                    Session["SearcECFCancellationRecord"] = null;
                    Session["SearcECFDeletionRecord"] = null;
                    Session["SearchECfDespatchData"] = null;
                    DataTable dtproxy = new DataTable(); 
                    Session["EmployeeCodeForErrorLog"] = lobj.employeeId.ToUpper().Trim();
                    Session["ErrMsg"] = null; 
                    dt = rep.GetLoginUserDetails(lobj.employeeId.ToUpper().Trim());
                    string Result = rep.AuthorizeEntity(lobj.employeeId.ToUpper().Trim());
                    if (dt.Rows.Count == 0)
                    {
                        loginstatus = "Invalid UserCode/Password";
                        ReturnAction = "../Loginpage/empLoginPage";
                    }
                    else if (employeelogintype == null)
                    {
                        loginstatus = "Invalid UserCode/Password";
                        ReturnAction = "../Loginpage/empLoginPage";
                    }
                    else if (lobj.empname == "0")
                    {
                        loginstatus = "Invalid UserCode/Password";
                        ReturnAction = "../Loginpage/empLoginPage";
                    }
                    else
                    {
                        int logincheck = 0;
                        logincheck = rep.CheckDuplicateLogin(lobj.employeeId);
                        if (logincheck < 1)
                        {
                            OldIPAddress = rep.GetPreviousIPAddress(lobj.employeeId);
                            OldBrowser = rep.GetPreviousBrowser(lobj.employeeId);
                            if (OldIPAddress == Request.UserHostAddress)
                            {
                                if (Request.Browser.Browser == OldBrowser)
                                {
                                    rep.ReleaseEmp(lobj.employeeId);
                                    logincheck = 1;
                                }
                                else
                                {
                                    logincheck = -2;
                                }
                            }
                            else
                            {
                                int sessioninterval = rep.GetSessionInterval(lobj.employeeId);
                                if (sessioninterval > 0)
                                    logincheck = 1;
                                else
                                    logincheck = -2;
                            }
                        }
                        if (logincheck > 0)
                        {
                            loginstatus = "Logged In";
                            ReturnAction = "../IEMCommon/Welcome";
                            Session["employee_gid"] = dt.Rows[0]["employee_gid"].ToString();
                            Session["employee_name"] = dt.Rows[0]["employee_name"].ToString();
                            Session["employee_code"] = dt.Rows[0]["employee_code"].ToString();
                            //Static Variables
                            Emp.empcodeST = dt.Rows[0]["employee_code"].ToString();
                            Emp.empgidST = Convert.ToInt32(dt.Rows[0]["employee_gid"].ToString());
                            Emp.IPAddress = Request.UserHostAddress;
                            Emp.BrowserName = Request.Browser.Browser;
                            //
                            Session["login-mode"] = employeelogintype.ToString();
                            Session["Pto"] = "Proxy To";
                            Session["loginraisemode"] = null;
                            Session["Proxyemployee_gid"] = null;
                            if (Session["login-mode"].ToString() == "Proxy")
                            {
                                dtproxy = rep.GetLoginUserDetails(lobj.empname.Trim());
                                Session["Proassuser"] = dtproxy.Rows[0]["employee_name"].ToString() + " [ " + dtproxy.Rows[0]["employee_code"].ToString() + " ] ";
                                Session["Proassusercode"] = dtproxy.Rows[0]["employee_code"].ToString();
                                Session["Proxyemployee_gid"] = dtproxy.Rows[0]["employee_gid"].ToString();
                                Session["loginraisemode"] = "P";
                            }

                            Session["employee_dept"] = dt.Rows[0]["employee_dept_name"].ToString();
                            if (dt.Rows[0]["employee_gender"].ToString() == "M")
                            {
                                Session["Link"] = "/Content/images/Mr1.png";
                            }
                            if (dt.Rows[0]["employee_gender"].ToString() == "F")
                            {
                                Session["Link"] = "/Content/images/Ms1.png";
                            }
                            int empgid = Convert.ToInt32(dt.Rows[0]["employee_gid"].ToString());
                            rep.UpdateLoginFlag(empgid, 1);
                            Session["login"] = "No";
                            if (Request.Cookies["Employee_Code"] != null)
                            {
                                var c = new HttpCookie("Employee_Code");
                                c.Expires = DateTime.Now.AddDays(-1);
                                Response.SetCookie(c);
                            }
                            HttpCookie cookie = new HttpCookie("Employee_Code");
                            cookie.Value = lobj.employeeId.ToUpper().Trim();
                            HttpContext.Response.SetCookie(cookie);
                        }
                        else
                        {
                            loginstatus = "The User[" + lobj.employeeId.ToString().ToUpper() + "] session is active @ " + OldIPAddress + " - " + OldBrowser;
                            ReturnAction = "../Loginpage/empLoginPage";
                        }
                    }
                }
                rep.insertloginattempt(Request.UserHostAddress, lobj.employeeId, loginstatus, Request.Browser.Browser);
                Session["ErrMsg"] = loginstatus;
                return RedirectToAction(ReturnAction);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                rep.insertloginattempt(Request.UserHostAddress, lobj.employeeId, "Application Error", Request.Browser.Browser);
                Session["ErrMsg"] = "Application Error";
                return RedirectToAction("../Loginpage/empLoginPage");
            }
            finally
            {
            }
        }

        public bool AuthenticateUser(string domain, string username, string password, string LdapPath, out string Errmsg)
        {
            Errmsg = "";
            string domainAndUsername = domain + @"\" + username;
            DirectoryEntry entry = new DirectoryEntry(LdapPath, domainAndUsername, password);
            try
            {
                // Bind to the native AdsObject to force authentication.
                Object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();
                if (null == result)
                {
                    return false;
                }
                // Update the new path to the user in the directory
                LdapPath = result.Path;
                string _filterAttribute = (String)result.Properties["cn"][0];
            }
            catch (Exception ex)
            {
                Errmsg = ex.Message;
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return false;
                //throw new Exception("Error authenticating user." + ex.Message);
            }
            return true;
        }

        ////*******************************************************************************************************************

        ////**************************************************************************************************
        ////[HttpPost]
        ////public ActionResult empLoginPage(LoginPage lobj)
        ////{
        ////    DataTable dt = new DataTable();
        ////    string loginstatus = "";
        ////    Session["ErrMsg"] = null;
        ////    Session["employee_name"] = null;
        ////    Session["employee_code"] = null;
        ////    string ReturnAction = string.Empty;
        ////    dt = rep.GetLoginUserDetails(lobj.employeeId.ToUpper().Trim());
        ////    if (dt.Rows.Count == 0)
        ////    {
        ////        loginstatus = "Invalid UserCode/Password";
        ////        ReturnAction = "../Loginpage/empLoginPage";
        ////    }
        ////    else
        ////    {
        ////        int logincheck = 0;
        ////        logincheck = rep.CheckDuplicateLogin(lobj.employeeId);
        ////        if (logincheck > 0)
        ////        {
        ////            loginstatus = "";
        ////            ReturnAction = "../IEMCommon/Welcome";
        ////            Session["employee_gid"] = dt.Rows[0]["employee_gid"].ToString();
        ////            Session["employee_name"] = dt.Rows[0]["employee_name"].ToString();
        ////            Session["employee_code"] = dt.Rows[0]["employee_code"].ToString();
        ////            Session["employee_dept"] = dt.Rows[0]["employee_dept_name"].ToString();
        ////            if (dt.Rows[0]["employee_gender"].ToString() == "M")
        ////            {
        ////                Session["Link"] = "/Content/images/Mr1.png";
        ////            }
        ////            if (dt.Rows[0]["employee_gender"].ToString() == "F")
        ////            {
        ////                Session["Link"] = "/Content/images/Ms1.png";
        ////            }
        ////            int empgid = Convert.ToInt32(dt.Rows[0]["employee_gid"].ToString());
        ////            rep.UpdateLoginFlag(empgid, 1);
        ////            Session["login"] = "No";
        ////        }
        ////        else
        ////        {
        ////            loginstatus = "This Employee (" + lobj.employeeId.ToUpper().Trim() + " ) already Logged In an another system";
        ////            ReturnAction = "../Loginpage/empLoginPage";
        ////        }

        ////    }
        ////    Session["ErrMsg"] = loginstatus;
        ////    rep.insertloginattempt(Request.UserHostAddress, lobj.employeeId, loginstatus,Request.Browser.Browser);
        ////    return RedirectToAction(ReturnAction);
        ////}
        //  //****************************************************135 login things***********************************************************
        //[HttpPost]
        //public ActionResult empLoginPage(LoginPage lobj, string employeelogintype)
        //{
        //    string ReturnAction = string.Empty;
        //    try
        //    {
        //        Session["DashSearchItems"] = null;
        //        Session["DashSearchItemsapp"] = null;
        //        Session["DashSearchItems_fb"] = null;
        //        Session["DashSearchItemsapp_fb"] = null;
        //        Session["SearchData"] = null;
        //        Session["SearcECFCancellationRecord"] = null;
        //        Session["SearcECFDeletionRecord"] = null;
        //        Session["SearchECfDespatchData"] = null;
        //        DataTable dt = new DataTable();
        //        DataTable dtproxy = new DataTable();
        //        string loginstatus = "";
        //        Session["EmployeeCodeForErrorLog"] = lobj.employeeId.ToUpper().Trim();
        //        Session["ErrMsg"] = null;
        //        //   int a = (Int32)Session["ErrMsg"];
        //        dt = rep.GetLoginUserDetails(lobj.employeeId.ToUpper().Trim());
        //        if (dt.Rows.Count == 0)
        //        {
        //            loginstatus = "Invalid UserCode/Password";
        //            ReturnAction = "../Loginpage/empLoginPage";
        //        }
        //        else if (employeelogintype == null)
        //        {
        //            loginstatus = "Invalid UserCode/Password";
        //            ReturnAction = "../Loginpage/empLoginPage";
        //        }
        //        else if (lobj.empname == "0")
        //        {
        //            loginstatus = "Invalid UserCode/Password";
        //            ReturnAction = "../Loginpage/empLoginPage";
        //        }
        //        else
        //        {
        //            int logincheck = 0;
        //            logincheck = rep.CheckDuplicateLogin(lobj.employeeId);
        //            if (logincheck > 0)
        //            {
        //                loginstatus = "";
        //                ReturnAction = "../IEMCommon/Welcome";
        //                Session["employee_gid"] = dt.Rows[0]["employee_gid"].ToString();
        //                Session["employee_name"] = dt.Rows[0]["employee_name"].ToString();
        //                Session["employee_code"] = dt.Rows[0]["employee_code"].ToString();
        //                Session["login-mode"] = employeelogintype.ToString();
        //                Session["Pto"] = "Proxy To";
        //                Session["loginraisemode"] = null;
        //                Session["Proxyemployee_gid"] = null;
        //                if (Session["login-mode"].ToString() == "Proxy")
        //                {
        //                    dtproxy = rep.GetLoginUserDetails(lobj.empname.Trim());
        //                    Session["Proassuser"] = dtproxy.Rows[0]["employee_name"].ToString() + " [ " + dtproxy.Rows[0]["employee_code"].ToString() + " ] ";
        //                    Session["Proassusercode"] = dtproxy.Rows[0]["employee_code"].ToString();
        //                    Session["Proxyemployee_gid"] = dtproxy.Rows[0]["employee_gid"].ToString();
        //                    Session["loginraisemode"] = "P";
        //                }

        //                Session["employee_dept"] = dt.Rows[0]["employee_dept_name"].ToString();
        //                if (dt.Rows[0]["employee_gender"].ToString() == "M")
        //                {
        //                    Session["Link"] = "/Content/images/Mr1.png";
        //                }
        //                if (dt.Rows[0]["employee_gender"].ToString() == "F")
        //                {
        //                    Session["Link"] = "/Content/images/Ms1.png";
        //                }
        //                int empgid = Convert.ToInt32(dt.Rows[0]["employee_gid"].ToString());
        //                rep.UpdateLoginFlag(empgid, 1);
        //                Session["login"] = "No";
        //            }
        //            else
        //            {
        //                loginstatus = "This Employee (" + lobj.employeeId.ToUpper().Trim() + " ) already Logged In an another system";
        //                ReturnAction = "../Loginpage/empLoginPage";
        //            }

        //        }
        //        Session["ErrMsg"] = loginstatus;
        //        rep.insertloginattempt(Request.UserHostAddress, lobj.employeeId, loginstatus,Request.Browser.Browser);

        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //    }
        //    return RedirectToAction(ReturnAction);
        //}


        [HttpPost]
        public JsonResult GetLogindetails(LoginPage lobj)
        {
            try
            {
                LoginPage TypeModel = new LoginPage();
                DataTable getproxygid = new DataTable();
                getproxygid = rep.getProxyId(lobj.employeeId);
                if (getproxygid.Rows.Count > 0)
                {
                    lobj.employeeId = getproxygid.Rows[0]["employee_gid"].ToString();
                    //TypeModel.GetProxyDetails = new SelectList(rep.GetProxyDetails(lobj.employeeId), "empname", "empname");
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Json(rep.GetProxyDetails(lobj.employeeId), JsonRequestBehavior.AllowGet);
        }
        // //  *******************************************************************************************************************


    }

}
