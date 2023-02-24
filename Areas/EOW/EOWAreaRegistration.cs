using System.Web.Mvc;

namespace IEM.Areas.EOW
{
    public class EOWAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "EOW";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "EOW_default",
                "EOW/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                 namespaces: new[] { "IEM.Areas.EOW.Controllers" }
            );
        }
    }
}
