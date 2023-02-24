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
    public class IEM_UOMController : Controller
    {
        //
        // GET: /IEM_UOM/
        private Iiem_mst_uom uom;
        CmnFunctions com = new CmnFunctions();
       
        public IEM_UOMController() :
            this(new IEM_MST_UOM()) { }

        public IEM_UOMController(Iiem_mst_uom Objist) 
        {
            uom = Objist;
        }
        public ActionResult Index()
        {
            List<iem_mst_uom> um = new List<iem_mst_uom>();
            um = uom.getuom().ToList();
            return View(um);           
        }
        [HttpPost]
        public ActionResult Index(string filter_code = null,string filter_name=null,string command=null)
        {
            List<iem_mst_uom> um = new List<iem_mst_uom>();
            if (command == "Search" || command == "Refresh")
            {
                um = uom.getuom(filter_code, filter_name).ToList();
                if (um.Count == 0)
                {
                    ViewBag.records = "No Records Found";
                }
                @ViewBag.filter_code = filter_code;
                @ViewBag.filter_name = filter_name;
            }
            if(command=="Clear")
            {
                um = uom.getuom().ToList();
            }

            return View(um);
        }

        [HttpGet]
        public PartialViewResult Create()
        {
            iem_mst_uom TypeModel = new iem_mst_uom();
            return PartialView(TypeModel);
        }

        [HttpPost]
        public JsonResult CreateUom(iem_mst_uom TypeModel)
        {
            string result = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    TypeModel.uom_insert_by = int.Parse(com.GetLoginUserGid().ToString());
                    result = uom.InsertUom(TypeModel);
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
            iem_mst_uom TypeModel = uom.GetUomById(id);
            return PartialView(TypeModel);
        }
        [HttpGet]
        public PartialViewResult ViewUom(int id)
        {
            iem_mst_uom TypeModel = uom.GetUomById(id);
            return PartialView(TypeModel);
        }

        [HttpPost]
        public JsonResult UpdatEomDetails(iem_mst_uom TypeModel)
        {
            string result = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    TypeModel.uom_update_by = int.Parse(com.GetLoginUserGid().ToString());
                    result = uom.UpdateUom(TypeModel);

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
                // TODO: Add delete logic here
                iem_mst_uom TypeModel = uom.GetUomById(id);
                return PartialView(TypeModel);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public JsonResult DeleteUomDetails(iem_mst_uom TypeModel)
        {
            string result = string.Empty;
            try
            {
                var ids = TypeModel.uom_gid;
                result=uom.DeleteUom(ids);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }      

        //
        // GET: /IEM_UOM/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /IEM_UOM/Create

        //public ActionResult Create()
        //{
        //    return View();
        //}

        //
        // POST: /IEM_UOM/Create

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
        // GET: /IEM_UOM/Edit/5

        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //
        // POST: /IEM_UOM/Edit/5

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
        // GET: /IEM_UOM/Delete/5

        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //
        // POST: /IEM_UOM/Delete/5

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
