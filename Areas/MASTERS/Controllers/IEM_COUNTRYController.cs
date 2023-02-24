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
    public class IEM_COUNTRYController : Controller
    {
        //
        // GET: /MASTERS/MASTERS/MASTERS/MASTERS/MASTERS/MASTERS/MASTERS/IEM_COUNTRY/
        private Iiem_mst_country country;
        CmnFunctions com = new CmnFunctions();

        public IEM_COUNTRYController() :
            this(new IEM_MST_COUNTRY()) { }

        public IEM_COUNTRYController(Iiem_mst_country Objist)
        {
            country = Objist;
        }
        [HttpGet]
        public ActionResult Index()
        {
            List<iem_mst_country> records = new List<iem_mst_country>();
            records = country.getcountry().ToList();
            if (records.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            return View(records);
        }
        [HttpPost]
        public ActionResult Index(string filter = null, string filter1 = null, string filter2 = null,string command=null)
        {
            List<iem_mst_country> records = new List<iem_mst_country>();
           
            Session["records"] = "";
            if (command == "Search" || command == "Refresh")
            {
                records = country.getcountry1(filter, filter1, filter2).ToList();
                @ViewBag.filter = filter;
                @ViewBag.filter1 = filter1;
                @ViewBag.filter2 = filter2;
                if (records.Count == 0)
                {
                    ViewBag.records = "No Records Found";
                }
            }
            else if(command=="Clear")
            {
                records = country.getcountry().ToList();
            }
            return View(records);
        }
        [HttpGet]
        public PartialViewResult Create()
        {
            iem_mst_country TypeModel = new iem_mst_country();
            TypeModel.Getcurrency = new SelectList(country.GetCurrency(), "Currencygid", "Currencyname");

            return PartialView(TypeModel);
        }
        [HttpPost]
        public JsonResult CreateCountry(iem_mst_country Countrymodel)
        {
            try
            {
                string dn="";
                if (ModelState.IsValid)
                {
                    GetcurrencyById getcou = country.GetCurrencyById(Countrymodel.currency_gid);
                    Countrymodel.currency_code = getcou.Currencycode;
                    Countrymodel.country_insert_by = int.Parse(com.GetLoginUserGid().ToString());
                    dn = country.Insertcountry(Countrymodel);
                    RedirectToAction("Index");
                }

                return Json(dn, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

            iem_mst_country TypeModel = country.GetCountryId(id);
            TypeModel.Getcurrency = new SelectList(country.GetCurrency(), "Currencygid", "Currencyname", TypeModel.selectedcurrency_gid);
            return PartialView(TypeModel);

        }

        [HttpGet]
        public PartialViewResult Delete(int id, string viewfor)
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

            iem_mst_country TypeModel = country.GetCountryId(id);
            TypeModel.Getcurrency = new SelectList(country.GetCurrency(), "Currencygid", "Currencyname", TypeModel.selectedcurrency_gid);
            return PartialView(TypeModel);
        }

        [HttpPost]
        public JsonResult EditCountry(iem_mst_country Countrymodel)
        {
            string result = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    Countrymodel.country_update_by = int.Parse(com.GetLoginUserGid().ToString());
                    result=country.UpdateCountry(Countrymodel);

                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //[HttpGet]
        //public PartialViewResult Delete(int id)
        //{
        //    try
        //    {
        //        iem_mst_country TypeModel = country.GetCountryId(id);
        //        return PartialView(TypeModel);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        [HttpPost]
        public JsonResult DeleteCountry(iem_mst_country County)
        {
            string result = string.Empty;
            try
            {
                var ids = County.country_gid;               
                result=country.Deletecountry(ids);               

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
