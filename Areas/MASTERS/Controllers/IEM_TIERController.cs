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
    public class IEM_TIERController : Controller
    {
        //
        // GET: /IEM_TIER/
         private Iiem_mst_tier tier;
         CmnFunctions com = new CmnFunctions();
        public IEM_TIERController() :
            this(new IEM_MST_TIER()) { }

        public IEM_TIERController(Iiem_mst_tier Objist) 
        {
            tier = Objist;
        }
        public ActionResult Index()
        {
            List<iem_mst_tier> tierrec = new List<iem_mst_tier>();
            tierrec = tier.gettier().ToList();
            return View(tierrec);
        }
        [HttpPost]
        public ActionResult Index(string filter_code = null, string filter_name = null,string command=null)
        {

            List<iem_mst_tier> tierrec = new List<iem_mst_tier>();
            if (command == "Search" || command == "Refresh")
            {
                tierrec = tier.gettier(filter_code, filter_name).ToList();
                if (tierrec.Count == 0)
                {
                    ViewBag.records = "No Records Found"; ;
                }
                @ViewBag.filter_code = filter_code;
                @ViewBag.filter_name = filter_name;
            }
            if(command=="Clear")
            {
                tierrec = tier.gettier().ToList();
            }
            return View(tierrec);
        }
       
        [HttpGet]
        public PartialViewResult Create()
        {
            iem_mst_tier TypeModel = new iem_mst_tier();
            return PartialView(TypeModel);
        }
        [HttpPost]
        public JsonResult CreateTier(iem_mst_tier TypeModel)
        {
            string result=string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    TypeModel.tier_insert_by = int.Parse(com.GetLoginUserGid().ToString());
                    result = tier.InsertTier(TypeModel);
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
            iem_mst_tier TypeModel = tier.GetTierById(id);
            return PartialView(TypeModel);
        }
       
        [HttpPost]
        public JsonResult UpdateTier(iem_mst_tier TypeModel)
        {
            string result = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    TypeModel.tier_update_by = int.Parse(com.GetLoginUserGid().ToString());
                    result = tier.UpdateTier(TypeModel);

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
        public PartialViewResult Delete(int id)
        {
            try
            {
                iem_mst_tier TypeModel = tier.GetTierById(id);
                return PartialView(TypeModel);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult DeleteTierDetails(iem_mst_tier TypeModel)
        {
            string reslut = string.Empty;
            try
            {
                var ids = TypeModel.tier_gid;                
                reslut= tier.DeleteTier(ids);
                return Json(reslut, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      
        //
        // GET: /IEM_TIER/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /IEM_TIER/Create

        //public ActionResult Create()
        //{
        //    return View();
        //}

        //
        // POST: /MASTERS/IEM_TIER/Create

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
        // GET: /MASTERS/IEM_TIER/Edit/5

        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //
        // POST: /MASTERS/IEM_TIER/Edit/5

        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //
        // GET: /MASTERS/IEM_TIER/Delete/5

        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //
        // POST: /MASTERS/IEM_TIER/Delete/5

        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
