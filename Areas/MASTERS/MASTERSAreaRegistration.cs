using System.Web.Mvc;

namespace IEM.Areas.MASTERS
{
    public class MASTERSAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "MASTERS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "MASTERS_default",
                "MASTERS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                 namespaces: new[] { "IEM.Areas.MASTERS.Controllers" }
            );
        }
    }
}
