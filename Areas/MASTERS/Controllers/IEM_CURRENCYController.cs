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
    public class IEM_CURRENCYController : Controller
    {
        //
        // GET: /IEM_CURRENCY/

        private Iiem_mst_currency currency;
        CmnFunctions com = new CmnFunctions();

        public IEM_CURRENCYController() :
            this(new IEM_MST_CURRENCY()) { }

        public IEM_CURRENCYController(Iiem_mst_currency Objist)
        {
            currency = Objist;
        }

        public ActionResult Index()
        {
            List<iem_mst_currency> g = new List<iem_mst_currency>();
            g = currency.getcurrency().ToList();
            return View(g);           
        }

        [HttpPost]
        public ActionResult Index(string filter_code,string filter_name,string command=null)
        {

            List<iem_mst_currency> currencyrec = new List<iem_mst_currency>();
            currencyrec = currency.getcurrency(filter_code, filter_name).ToList();
            if (command == "Search" || command == "Refresh")
            {
                @ViewBag.filter_code = filter_code;
                @ViewBag.filter_name = filter_name;
                if (currencyrec.Count == 0)
                {
                    ViewBag.records = "No Records Found";
                }
            }
            if(command=="Clear")
            {
                currencyrec = currency.getcurrency().ToList();
            }

            return View(currencyrec);
        }
        [HttpGet]
        public PartialViewResult Create()
        {
            iem_mst_currency TypeModel = new iem_mst_currency();
            return PartialView(TypeModel);
        }

        [HttpPost]
        public JsonResult CreateCurrency(iem_mst_currency CategoryModel)
        {
            string result = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    CategoryModel.currency_insert_by = int.Parse(com.GetLoginUserGid().ToString());
                    result = currency.InsertCurrency(CategoryModel);
                    if (result == "success") RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
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
            iem_mst_currency TypeModel = currency.GetCurrencyById(id);
            return PartialView(TypeModel);
        }

        [HttpGet]
        public PartialViewResult ViewCurrency(int id)
        {
            iem_mst_currency TypeModel = currency.GetCurrencyById(id);
            return PartialView(TypeModel);
        }

        [HttpPost]
        public JsonResult UpdateCurrencyDetails(iem_mst_currency CategoryModel)
        {
            string result = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    CategoryModel.currency_update_by = int.Parse(com.GetLoginUserGid().ToString());
                    result = currency.UpdateCurrency(CategoryModel);
                }

                if (result == "success") RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public PartialViewResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                iem_mst_currency TypeModel = currency.GetCurrencyById(id);
                return PartialView(TypeModel);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult DeleteCurrencyDetails(iem_mst_currency TypeModel)
        {
            string res = string.Empty;
            try
            {
                var ids = TypeModel.currency_gid;
                res=currency.DeleteCurrency(ids);
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //
        // GET: /IEM_CURRENCY/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //

        // POST: /IEM_CURRENCY/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /IEM_CURRENCY/Edit/5

        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //
        // POST: /IEM_CURRENCY/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /IEM_CURRENCY/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}


        //
        // POST: /IEM_CURRENCY/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
