using IEM.App_Start;
using IEM.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace IEM
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            MvcHandler.DisableMvcResponseHeader = true; 
            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

          Application["path"] = @"C:\temp\";
            // Application["supplierpath"] = "C:\\temp\\";

        }
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {

            var app = sender as HttpApplication;
            if (app != null && app.Context != null)
            {
                //Added for Info Sec Points - from Bharathi
                //HttpContext.Current.Response.Headers.Remove("Server");
                //HttpContext.Current.Response.Headers.Remove("X-Powered-By");
                HttpContext.Current.Response.Headers.Remove("X-Powered-By");
                HttpContext.Current.Response.Headers.Remove("X-AspNet-Version");
                HttpContext.Current.Response.Headers.Remove("X-AspNetMvc-Version");
                HttpContext.Current.Response.Headers.Remove("Server");

                app.Context.Response.Headers.Remove("Server");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.Cache.AppendCacheExtension("no-store, must-revalidate");
                Response.AppendHeader("Pragma", "no-cache");
                Response.AppendHeader("Expires", "0");
            } 

            CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            newCulture.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            newCulture.DateTimeFormat.DateSeparator = "-";
            Thread.CurrentThread.CurrentCulture = newCulture;
            if (Emp.empcodeST != null && Emp.empcodeST != "")
            {
                IEMDataModel objModel = new IEMDataModel();
                objModel.updateloginattempt(Emp.empcodeST);
            }


           // GlobalFilters.Filters.Add(new NoDirectAccessAttribute());
        }
    

        protected void Session_Start(object sender, EventArgs e)
        {
            //HttpContext.Current.Response.Headers.Remove("X-Powered-By");
            //HttpContext.Current.Response.Headers.Remove("X-AspNet-Version");
            //HttpContext.Current.Response.Headers.Remove("X-AspNetMvc-Version");
            //HttpContext.Current.Response.Headers.Remove("Server");

            Emp.SessionStatus = 1;
            //// This code will mark the forms authentication cookie and the
            //// session cookie as Secure.
            //if (HttpContext.Current.Response.Cookies.Count > 0)
            //{
            //    foreach (string s in HttpContext.Current.Response.Cookies.AllKeys)
            //    {
            //        //if (s == FormsAuthentication.FormsCookieName || s.ToLower() == "asp.net_sessionid")
            //        //{
            //          HttpContext.Current.Response.Cookies[s].Secure = true;
            //        //}
            //    }
            //}

        }
        protected void Session_End(object sender, EventArgs e)
        {
            //HttpContext.Current.Response.Headers.Remove("X-Powered-By");
            //HttpContext.Current.Response.Headers.Remove("X-AspNet-Version");
            //HttpContext.Current.Response.Headers.Remove("X-AspNetMvc-Version");
            //HttpContext.Current.Response.Headers.Remove("Server");

            IEMDataModel objModel = new IEMDataModel();
            objModel.insertloginattempt(Emp.IPAddress, Emp.empcodeST, "Session Expired", Emp.BrowserName);
            objModel.UpdateLoginFlag(Emp.empgidST, 0);
            Emp.SessionStatus = 0;
            Session.Abandon();
            Session["employee_gid"] = null;
        }

        protected void Application_PreSendRequestHeaders()
        {
           // Response.Buffer = true;
            // Response.Headers.Remove("Server");
            //HttpContext.Current.Response.Headers.Remove("X-Powered-By");
            //HttpContext.Current.Response.Headers.Remove("X-AspNet-Version");
            //HttpContext.Current.Response.Headers.Remove("X-AspNetMvc-Version");
            //HttpContext.Current.Response.Headers.Remove("Server");

            Response.Headers.Set("Server", "My httpd server");
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");
        }



        //protected void Application_AcquireRequestState(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string lcReqPath = Request.Path.ToLower();

        //        // Session is not stable in AcquireRequestState - Use Current.Session instead.
        //        System.Web.SessionState.HttpSessionState curSession = HttpContext.Current.Session;
        //        string loginpath = System.Configuration.ConfigurationManager.AppSettings["LoginSessionTimeOut"].ToString();
        //        // If we do not have a OK Logon (remember Session["LogonOK"] = null; on logout, and set to true on logon.)
        //        //  and we are not already on loginpage, redirect.
        //        IEMDataModel objModel = new IEMDataModel();
        //        // note: on missing pages curSession is null, Test this without 'curSession == null || ' and catch exception.
        //        if (Emp.empcodeST != null && Emp.empcodeST != "")
        //        {
        //            if (lcReqPath != loginpath.ToLower() &&
        //          (Emp.SessionStatus == 0))
        //            {
        //                // Redirect nicely
        //                objModel.insertloginattempt(Emp.IPAddress, Emp.empcodeST, "Request After Session Expired", Emp.BrowserName);
        //                objModel.UpdateLoginFlag(Emp.empgidST, 0);
        //                Context.Server.ClearError();
        //                Context.Response.AddHeader("Location", loginpath);
        //                Context.Response.TrySkipIisCustomErrors = true;
        //                Context.Response.StatusCode = (int)System.Net.HttpStatusCode.Redirect;
        //                // End now end the current request so we dont leak.
        //                Context.Response.Output.Close();
        //                Context.Response.End();
        //                return;
        //            }
        //       }
        //    }
        //    catch (Exception)
        //    {

        //        // todo: handle exceptions nicely!
        //    }
        //}
    }
}