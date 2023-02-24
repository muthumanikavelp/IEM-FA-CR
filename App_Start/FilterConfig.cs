using IEM.Helper;
using System.Web;
using System.Web.Mvc;

namespace IEM
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CustomValidateAntiForgeryToken());
        }
    }
}