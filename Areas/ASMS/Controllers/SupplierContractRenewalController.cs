using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.ASMS.Models;
using IEM.App_Start;
namespace IEM.Areas.ASMS.Controllers
{

      [NoDirectAccess]
    public class SupplierContractRenewalController : Controller
    {
        private IRepositoryRenewal Ire;
        public SupplierContractRenewalController() : this (new SupRenwalModel())
        {

        }
        public SupplierContractRenewalController(IRepositoryRenewal Iree)
        {
            Ire = Iree;
        }

        [HttpGet]
        public ActionResult GetContractReneal()
        {
            List<SupplierContractRenewal> objvar = new List<SupplierContractRenewal>();
            if (Session["Gesearchresult"] != null)
            {
                objvar = (List<SupplierContractRenewal>) Session["Gesearchresult"];
            }
            else
            {
                objvar = Ire.GetfullRenewal().ToList();
            }
           
            return View(objvar);
        }

        [HttpPost]
        public ActionResult GetContractReneal(string txtSuppliercode, string txtSupplierName, string txtContStartDate, string txtContEndDate, string ddlContractdays)
        {
            List<SupplierContractRenewal> objvar1 = new List<SupplierContractRenewal>();
            objvar1 = Ire.GetSearchRenewal(txtSuppliercode, txtSupplierName, txtContStartDate, txtContEndDate,ddlContractdays).ToList();
            Session["Gesearchresult"] = objvar1;
            if (objvar1.Count==0)
            {
                ViewBag.status = "RecordNtFound";
            }
            return View(objvar1);
        }

   
    }
}
