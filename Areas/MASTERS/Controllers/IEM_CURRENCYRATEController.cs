using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.MASTERS.Models;
using IEM.Common;
using IEM.App_Start;

namespace IEM.Areas.MASTERS.Controllers
{
     [NoDirectAccess]
    public class IEM_CURRENCYRATEController : Controller
    {
        //
        // GET: /MASTERS/IEM_CURRENCYRATE/
        private Iiem_mst_currencyrate currencyrate;
        CmnFunctions com = new CmnFunctions();

        public IEM_CURRENCYRATEController() :
            this(new IEM_MST_CURRENCYRATE ()) { }

        public IEM_CURRENCYRATEController(Iiem_mst_currencyrate Objist)
        {
            currencyrate = Objist;
        }
        public ActionResult Index()
        {
            List<iem_mst_currencyrate> records = new List<iem_mst_currencyrate>();
            records = currencyrate.getcurrencyRate().ToList();
            return View(records); 
        }
        [HttpPost]
        public ActionResult Index(string Currencyname = null, string periodfrom = null, string periodto = null, string command = null)
        {
            List<iem_mst_currencyrate> records = new List<iem_mst_currencyrate>();
            if (command == "Search" || command == "Refresh")
            {
                records = currencyrate.getcurrencyRate(Currencyname, periodfrom, periodto).ToList();
                if (records.Count == 0)
                {
                    ViewBag.Message = "No Records Found";
                }
                @ViewBag.Currencyname = Currencyname;
                @ViewBag.periodfrom = periodfrom;
                @ViewBag.periodto = periodto;
            }
            else if (command == "Clear")
            {
                records = currencyrate.getcurrencyRate().ToList();
            }
            return View(records);
        }

        [HttpGet]
        public PartialViewResult Create()
        {
            iem_mst_currencyrate TypeModel = new iem_mst_currencyrate();
            TypeModel.Getcurrency = new SelectList(currencyrate.Getcurrency() , "Currencygid", "Currencyname", TypeModel.selectedcurrency_gid);
            return PartialView(TypeModel);  

        }
       
       [HttpPost]
        public JsonResult CreateCurrencyRate(iem_mst_currencyrate currencuratedetails)
        {
            string res = string.Empty;
            res = currencyrate.InsertCurrencyRate(currencuratedetails);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
       [HttpGet]
       public PartialViewResult Edit(int id, string viewfor)
       {
           if (viewfor == "edit")
           {
               ViewBag.viewfor = "edit";
           }
           else if (viewfor == "view")
           {
               ViewBag.viewfor = "view";
           }
           else if (viewfor == "delete")
           {
               ViewBag.viewfor = "delete";
           }
           iem_mst_currencyrate TypeModel = new iem_mst_currencyrate();
           TypeModel = currencyrate.GetCurrencyRateById(id);
           TypeModel.Getcurrency = new SelectList(currencyrate.Getcurrency(), "Currencygid", "Currencyname", TypeModel.selectedcurrency_gid);
           return PartialView(TypeModel);
       }
       [HttpPost]
       public JsonResult Deletecurrencyrate(iem_mst_currencyrate TypeModel)
       {
           string lsresult = string.Empty;
           try
           {
               var ids = TypeModel.currencyrate_gid;
               lsresult = currencyrate.DeleteCurrencyRate(ids);
               ViewBag.viewfor = "delete";
               return Json(lsresult, JsonRequestBehavior.AllowGet);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       [HttpPost]
       public JsonResult UpdateCurrencyRate(iem_mst_currencyrate TypeModel)
       {
           string result = "";
           try
           {
               if (ModelState.IsValid)
               {
                   result = currencyrate.UpdateCurrencyRate(TypeModel);
                   if (result == "success") RedirectToAction("Index");
                   return Json(result, JsonRequestBehavior.AllowGet);
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }

           return Json(result, JsonRequestBehavior.AllowGet);
       }
    }
}
