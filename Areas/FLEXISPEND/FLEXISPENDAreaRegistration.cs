using System.Web.Mvc;

namespace IEM.Areas.FLEXISPEND
{
    public class FLEXISPENDAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "FLEXISPEND";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //sample
            //context.MapRoute(
            //    name: "FLEXISPEND_defaultmodel1",
            //    url: "FLEXISPEND/{controller}/{action}/{id}/{subTypeId}",
            //    defaults: new { controller = "AuditWorkTray", action = "SupplierInvoiceDetails" },
            //    namespaces: new[] { "IEM.Areas.FLEXISPEND.Controllers" }
            //);

            context.MapRoute(
                "FLEXISPEND_defaultmodel2",
                "FLEXISPEND/{controller}/{action}/{Id}/{subId}/{ecfNo}",
                new { action = "Index" },
                namespaces: new[] { "IEM.Areas.FLEXISPEND.Controllers" }
            );

            context.MapRoute(
                "FLEXISPEND_defaultmodel1",
                "FLEXISPEND/{controller}/{action}/{Id}/{subId}",
                new { action = "Index" },
                namespaces: new[] { "IEM.Areas.FLEXISPEND.Controllers" }
            );
            
            context.MapRoute(
                "FLEXISPEND_defaultmodel0",
                "FLEXISPEND/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "IEM.Areas.FLEXISPEND.Controllers" }
            );
        }
    }
}
