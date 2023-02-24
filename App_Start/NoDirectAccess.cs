using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace IEM.App_Start
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class NoDirectAccessAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.UrlReferrer == null ||
                        filterContext.HttpContext.Request.Url.Host != filterContext.HttpContext.Request.UrlReferrer.Host)
            {
                string NoDirectAccess = System.Configuration.ConfigurationManager.AppSettings["NoDirectAccess"].ToString();
                if (NoDirectAccess=="enable")
                {
                    filterContext.Result = new RedirectToRouteResult(new
                              RouteValueDictionary(new { controller = "Loginpage", action = "empLoginPage", area = "" }));
                }
              
            }
        }
    }
}