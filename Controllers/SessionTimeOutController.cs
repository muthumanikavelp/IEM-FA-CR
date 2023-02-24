using IEM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Common;

namespace IEM.Controllers
{
    public class SessionTimeOutController : Controller
    {
        //
        // GET: /SessionTimeOut/
        ErrorLog objErrorLog = new ErrorLog();
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public void UpdateSessionExpire()
        {
            try
            {
                IEMDataModel dm = new IEMDataModel();
                int empgidsession = Convert.ToInt32(Emp.empgidST);
                dm.UpdateLoginFlag(empgidsession, 0);
                dm.insertloginattempt(Emp.IPAddress, Emp.empcodeST, "Session Timed Out", Emp.BrowserName);
                Emp.SessionStatus = 0;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

        }
        [HttpPost]
        public void BrowserClose() 
        {
            try
            {
                IEMDataModel dm = new IEMDataModel();
                int empgidsession = Convert.ToInt32(Emp.empgidST);
                dm.UpdateLoginFlag(empgidsession, 0);
                dm.insertloginattempt(Emp.IPAddress, Emp.empcodeST, "Closing browser without proper logout", Emp.BrowserName);
                Emp.SessionStatus = 0;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

        }

        public ActionResult ErrorPage()
        {
            return View();
        }
    }
}
