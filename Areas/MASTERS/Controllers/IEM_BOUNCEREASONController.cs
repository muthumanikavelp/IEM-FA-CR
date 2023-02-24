using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Common;
using IEM.Areas.MASTERS.Models;
using IEM.App_Start;

namespace IEM.Areas.MASTERS.Controllers
{

      [NoDirectAccess]
    public class IEM_BOUNCEREASONController : Controller
    {
        //
        // GET: /MASTERS/IEM_BOUNCEREASON/
        private Iiem_mst_bounce bounce;
        CmnFunctions com = new CmnFunctions();
       
        public IEM_BOUNCEREASONController() :
            this(new IEM_MST_BOUNCEREASON()) { }

        public IEM_BOUNCEREASONController(Iiem_mst_bounce Objist) 
        {
            bounce = Objist;
        }
        public ActionResult Index()
        {
            List<iem_mst_bouncereason> bouncerec = new List<iem_mst_bouncereason>();
            bouncerec = bounce.getbounce().ToList();
            return View(bouncerec);
        }
        [HttpPost]
        public ActionResult Index(string filter_code = null,string filter_name = null,string command=null)
        {

            List<iem_mst_bouncereason> bouncerec = new List<iem_mst_bouncereason>();
            if(command=="Search" || command=="Refresh")
            { 
            Session["records"] = "";
            bouncerec = bounce.getbounce(filter_code,filter_name).ToList();
            Session["records"] = bouncerec;
            @ViewBag.filter_code = filter_code;
            @ViewBag.filter_name = filter_name;
            if (bouncerec.Count == 0)
            {
                ViewBag.Message = "No records found";
            }
           }
            if(command=="Clear")
            {
                bouncerec = bounce.getbounce().ToList();
            }
            
            return View(bouncerec);
        }
        [HttpGet]
        public PartialViewResult Create()
        {
            iem_mst_bouncereason TypeModel = new iem_mst_bouncereason();
            return PartialView(TypeModel);
        }
        [HttpPost]
        public JsonResult CreateBounce(iem_mst_bouncereason TypeModel)
        {
            string result = "";
            try
            {
                if (ModelState.IsValid)
                {
                    TypeModel.bouncereason_insert_by = int.Parse(com.GetLoginUserGid().ToString());
                    result = bounce.InsertBounce(TypeModel);
                    if (result == "success") RedirectToAction("Index");
                }

                return Json(result, JsonRequestBehavior.AllowGet);
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
            iem_mst_bouncereason TypeModel = bounce.GetBounceById(id);
            return PartialView(TypeModel);
        }

        [HttpGet]
        public PartialViewResult ViewBounce(int id)
        {
            iem_mst_bouncereason TypeModel = bounce.GetBounceById(id);
            return PartialView(TypeModel);
        }

        [HttpPost]
        public JsonResult UpdateBounceDetails(iem_mst_bouncereason TypeModel)
        {
            string result="";
            try
            {
                if (ModelState.IsValid)
                {
                    TypeModel.bouncereason_update_by = int.Parse(com.GetLoginUserGid().ToString());
                    result = bounce.UpdateBounce(TypeModel);
                    if (result == "success") RedirectToAction("Index");
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public PartialViewResult Delete(int id)
        {
            try
            {
                // TODO: Add delete logic here
                iem_mst_bouncereason TypeModel = bounce.GetBounceById(id);
                return PartialView(TypeModel);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public JsonResult DeleteBounceDetails(iem_mst_bouncereason TypeModel)
        {
            try
            {
                var ids = TypeModel.bouncereason_gid;
                int uperson = TypeModel.bouncereason_update_by;
                uperson = int.Parse(com.GetLoginUserGid().ToString());
                bounce.DeleteBounce(ids, uperson);
                return Json("Record deleted successfully !", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }      

        //
        // GET: /IEM_BOUNCEREASON/Details/5

        public ActionResult Details(int id)
        {
            return View();
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
