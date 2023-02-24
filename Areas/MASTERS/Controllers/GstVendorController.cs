using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.MASTERS.Models;
using IEM.Common; 
using System.IO; 
using System.Data;
using System.Reflection;
using IEM.App_Start;

namespace IEM.Areas.MASTERS.Controllers
{
     [NoDirectAccess]
    public class GstVendorController : Controller
    {
        private GstVendor Res;
        CmnFunctions com = new CmnFunctions();
        public GstVendorController() :
            this(new GstVendorModel()) { }
       

        //
        // GET: /MASTERS/GstVendor/
        public GstVendorController(GstVendor Objist)
        {
            Res = Objist;
        }
        [HttpGet]
        
        public ActionResult Index()
        {
            List<EntityGstvendor> records = new List<EntityGstvendor>();
            records.Clear();
            records= Res.getmaker().ToList();
            ViewBag.IsChecker = records[0].IsChecker;
            records = Res.getvendor().ToList();
            if (records.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
         // int.Parse(com.GetLoginUserGid().ToString());
             
            return View(records);
        }
        [HttpGet]
        public PartialViewResult Create_Vendor()
        {
            EntityGstvendor TypeModel = new EntityGstvendor();
            TypeModel.GetState = new SelectList(Res.GetState(), "suppliergst_stateid", "suppliergst_state");
            return PartialView(TypeModel);
        }
            
        [HttpPost]
        public JsonResult CreateVendor(EntityGstvendor pinmodel)
        {
            try
            {
                string dn = ""; 
               dn = Res.Insertvendor(pinmodel);
                RedirectToAction("Index"); 
                return Json(dn, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public PartialViewResult Edit_Vendor(int id, string viewfor)
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
            List<EntityGstvendor> records = new List<EntityGstvendor>();
            records = Res.getmaker().ToList();
            ViewBag.IsChecker = records[0].IsChecker;
            EntityGstvendor TypeModel = Res.getVendorid(id);
            TypeModel.GetState = new SelectList(Res.GetState(),"suppliergst_stateid", "suppliergst_state", TypeModel.selectedstate_gid);
            return PartialView(TypeModel);

        }

        [HttpPost]
        public JsonResult EditVendor(EntityGstvendor pinmodel)
        {
            string result = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    result = Res.Updatevendor(pinmodel);
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

            EntityGstvendor TypeModel = Res.getVendorid(id);
            TypeModel.GetState = new SelectList(Res.GetState(),"suppliergst_state_gid", "state_name", TypeModel.selectedstate_gid); 
            return PartialView(TypeModel);
        }
        [HttpPost]
        public JsonResult DeleteVendor(EntityGstvendor pincode)
        {
            string result = string.Empty;
            try
            {
                var ids = pincode.suppliergst_gid; ;
                result = Res.DeleteVendor(ids);

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}
