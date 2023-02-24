using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Web.Http;
using System.Web.Mvc;
//using System.Web.Mvc.AuthorizationContext;
using System.Web.Routing;
using IEM.App_Start;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web.UI.WebControls;

namespace IEM.App_Start
{
    public class Attribute
    { }


    public class AuthorizeChecker : FilterAttribute, IAuthorizationFilter
    {
        public string MenuId { get; set; }
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            string Result = string.Empty;
            DataTable dt = new DataTable();
            dt = (DataTable)HttpContext.Current.Session["Role"];
            

            int i = 0;
            for (i = 0; i < dt.Rows.Count; i++)
            {

                if (Convert.ToInt32(dt.Rows[i]["menu_gid"]) == Convert.ToInt32(MenuId))
                {
                    Result = "True";
                    break;

                }
                else
                {
                    Result = "False";
                }

            }
            HttpContext.Current.Session["AttributeResult"] = Result;

            if (Result == "False")
            {
                filterContext.Result = new RedirectResult("~/LoginPage/empLoginPage?id=logoff");
            }
        }
    }

}


   /* public class AuthorizeChecker : AuthorizeAttribute
    {
        public string MenuId { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //var isAuthorized = base.AuthorizeCore (httpContext);
            //if (!isAuthorized)
            //{
            //    return false;
            //}
            string Result = string.Empty;
            DataTable dt = new DataTable();
            //HttpContextBase httpContext = new HttpContextBase();
            dt = (DataTable)HttpContext.Current.Session["Role"];

            int i = 0;
            for (i = 0; i < dt.Rows.Count; i++)
            {

                if (Convert.ToInt32(dt.Rows[i]["menu_gid"]) == Convert.ToInt32(MenuId))
                {
                    Result = "True";
                    break;

                }
                else
                {
                    Result = "False";
                }

            }
            if (Result == "True")
            { return true; 
            }
            else
            {
                //AuthorizationContext context = new AuthorizationContext(); 
                //context.Result = new RedirectResult("/UnauthorizedRequest.html");
                return false;
            }

        }

    }
}*/
        

      /*  public class AuthorizeMaker : AuthorizeAttribute
        {
            public string MenuId { get; set; }

            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                //var isAuthorized = base.AuthorizeCore (httpContext);
                //if (!isAuthorized)
                //{
                //    return false;
                //}

                string CurrentUserRole = "FAMMKR";
                string abort = "abort";
                if ((this.MenuId.Contains(CurrentUserRole) && this.MenuId.Contains(abort)))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }
       */
//}