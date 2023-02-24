using System.Web.Mvc;

namespace IEMDashboard.Areas.Dashboard
{
    public class DashboardAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Dashboard";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            /*context.MapRoute(
               "Dashboard_default01",
               "Dashboard",
               new { Controller = "Dashboard", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "IEMDashboard.Areas.Dashboard.Controllers" }
           );

            context.MapRoute(
               "Dashboard_default00",
               "Dashboard/{controller}/{action}/{id}",
               new { action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "IEMDashboard.Areas.Dashboard.Controllers" }
           );
            */

            context.MapRoute(
                "Dashboard_default01",
                "Dashboard",
                new { Controller = "Dashboard", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "IEM.Areas.Dashboard.Controllers" }
            );
            context.MapRoute(
                "Dashboard_default00",
                "Dashboard/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "IEM.Areas.Dashboard.Controllers" }
            );
        }
    }
}
