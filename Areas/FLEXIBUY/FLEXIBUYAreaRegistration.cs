using System.Web.Mvc;

namespace IEM.Areas.FLEXIBUY
{
    public class FLEXIBUYAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "FLEXIBUY";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "FLEXIBUY_default1",
                "FLEXIBUY/{controller}/{action}/{Id}/{Access}",
                new { action = "Index" },
                namespaces: new[] { "IEM.Areas.FLEXIBUY.Controllers" }
            );

            context.MapRoute(
                "FLEXIBUY_default",
                "FLEXIBUY/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                 namespaces: new[] { "IEM.Areas.FLEXIBUY.Controllers" }
            );
        }
    }
}
