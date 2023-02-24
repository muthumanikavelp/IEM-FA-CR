using IEM.Areas.MASTERS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Common;
using IEM.App_Start;
namespace IEM.Areas.MASTERS.Controllers
{

    [NoDirectAccess]
    public class IEM_REGIONController : Controller
    {
        //
        // GET: /IEM_REGION/
        private Iiem_mst_region region;
        CmnFunctions com = new CmnFunctions();

        public IEM_REGIONController() :
            this(new IEM_MST_REGION()) { }

        public IEM_REGIONController(Iiem_mst_region Objist)
        {
            region = Objist;
        }
        public ActionResult Index()
        {

            List<iem_mst_region> reg = new List<iem_mst_region>();
            reg = region.getregion().ToList();
            return View(reg);
        }
        [HttpPost]
        public ActionResult Index(string filter = null, string command = null)
        {

            List<iem_mst_region> reg = new List<iem_mst_region>();
            if (command == "Search" || command == "Refresh")
            {
                reg = region.getregion(filter).ToList();
                if (reg.Count == 0)
                {
                    ViewBag.records = "No Records Found";
                }
                @ViewBag.filter = filter;

            }
            if (command == "Clear")
            {
                reg = region.getregion().ToList();
            }
            return View(reg);
        }

        [HttpGet]
        public PartialViewResult Create()
        {
            iem_mst_region TypeModel = new iem_mst_region();
            return PartialView(TypeModel);
        }
        [HttpPost]

        public JsonResult CreateRegion(iem_mst_region TypeModel)
        {
            string result = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    TypeModel.region_insert_by = int.Parse(com.GetLoginUserGid().ToString());
                    result = region.InsertRegion(TypeModel);
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
            iem_mst_region TypeModel = region.GetRegionById(id);
            return PartialView(TypeModel);
        }

        [HttpGet]
        public PartialViewResult ViewRegion(int id)
        {
            iem_mst_region TypeModel = region.GetRegionById(id);
            return PartialView(TypeModel);
        }

        [HttpPost]
        public JsonResult UpdateRegionDetails(iem_mst_region TypeModel)
        {
            string result = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    TypeModel.region_update_by = int.Parse(com.GetLoginUserGid().ToString());
                    result = region.UpdateRegion(TypeModel);
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
                iem_mst_region TypeModel = region.GetRegionById(id);
                return PartialView(TypeModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //[HttpPost]
        //public JsonResult DeleteRegion(iem_mst_region TypeModel)
        //{
        //    try
        //    {
        //        var ids = TypeModel.region_gid;
        //        int uperson = TypeModel.region_update_by;
        //        uperson = int.Parse(com.GetLoginUserGid().ToString());
        //        region.DeleteRegion(ids);
        //        return Json("Record deleted successfully !", JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}           
        [HttpPost]
        public JsonResult DeleteRegion(iem_mst_region TypeModel)
        {
            string res = string.Empty;
            try
            {
                var ids = TypeModel.region_gid;
                res = region.DeleteRegion(ids);
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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
