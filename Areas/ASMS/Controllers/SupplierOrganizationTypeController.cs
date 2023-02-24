using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.ASMS.Models;

namespace IEM.Areas.ASMS.Controllers
{
    public class SupplierOrganizationType : Controller
    {

        private IRepositoryRenewal Ire;

        public SupplierOrganizationType() :this(new  SupRenwalModel())
    {

    }
        public SupplierOrganizationType(IRepositoryRenewal Iree)
        {
            Ire = Iree;
        }
        public ActionResult Index()
        {

            return View();
        }

    }
}
