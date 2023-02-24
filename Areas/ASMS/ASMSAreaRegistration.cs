using System.Web.Mvc;

namespace IEM.Areas.ASMS
{
    public class ASMSAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ASMS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ASMS_default",
                "ASMS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "IEM.Areas.ASMS.Controllers" }
            );
        }
    }
}
