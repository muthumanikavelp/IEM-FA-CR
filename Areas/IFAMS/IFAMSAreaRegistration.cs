using System.Web.Mvc;

namespace IEM.Areas.IFAMS
{
    public class IFAMSAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "IFAMS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "IFAMS_default",
                "IFAMS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "IEM.Areas.IFAMS.Controllers"}
            );
        }
    }
}
