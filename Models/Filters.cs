using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace IEM.Models
{
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext ctx = HttpContext.Current;
            if (HttpContext.Current.Session["employee_code"] == null)
            {
               // FormsAuthentication.SignOut();  
                filterContext.Result = new RedirectResult("~/Loginpage/empLoginPage");
                return;
            }
            base.OnActionExecuting(filterContext);



            
        }

        
    }  
}