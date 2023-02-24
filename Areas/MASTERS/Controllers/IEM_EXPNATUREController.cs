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
    public class IEM_EXPNATUREController : Controller
    {
        //
        // GET: /MASTERS/IEM_EXPNATURE/
         private Iiem_mst_expnature Expnature;
         CmnFunctions com = new CmnFunctions();
       
        public IEM_EXPNATUREController() :
            this(new IEM_MST_EXPNATURE()) { }

        public IEM_EXPNATUREController(Iiem_mst_expnature Objist) 
        {
            Expnature = Objist;
        }
        public ActionResult Index()
        {
            List<iem_mst_expnature> Exprecord = new List<iem_mst_expnature>();
            Exprecord = Expnature.getexpnature().ToList();
            if (Exprecord.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            return View(Exprecord);
        }
        [HttpPost]
        public ActionResult Index(string filter_name = null,string command=null)
        {
            List<iem_mst_expnature> Exprecord = new List<iem_mst_expnature>();
            if (command == "Search" || command == "Refresh")
            {
                Exprecord = Expnature.getexpnature(filter_name).ToList();
                @ViewBag.filter_name = filter_name;
                if (Exprecord.Count == 0)
                {
                    ViewBag.records = "No Records Found";
                }
            }
            if(command=="Clear")
            {
                Exprecord = Expnature.getexpnature().ToList();
            }
            return View(Exprecord);
        }
        
        [HttpGet]
        public PartialViewResult Create()
        {
            iem_mst_expnature TypeModel = new iem_mst_expnature();
            return PartialView(TypeModel);
        }

        [HttpPost]
        public JsonResult CreateExpNature(iem_mst_expnature TypeModel)
        {
            string result = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    TypeModel.expnature_insert_by = int.Parse(com.GetLoginUserGid().ToString());
                    result = Expnature.Insertexpnature(TypeModel);
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
            iem_mst_expnature TypeModel = Expnature.GetexpnatureById(id);
            return PartialView(TypeModel);
        }
        [HttpPost]
        public JsonResult UpdateExpNature(iem_mst_expnature TypeModel)
        {
            string result = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    TypeModel.expnature_update_by = int.Parse(com.GetLoginUserGid().ToString());
                    result = Expnature.Updateexpnature(TypeModel);
                    if (result == "success") RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteExpNature(iem_mst_expnature ExpNature)
        {
            string result = string.Empty;
            try
            {                
               result= Expnature.Deleteexpnature(ExpNature.expnature_gid);
               return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }      
        public ActionResult Details(int id)
        {
            return View();
        }

       
        // GET: /MASTERS/IEM_EXPNATURE/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /MASTERS/IEM_EXPNATURE/Edit/5

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

    }
}
