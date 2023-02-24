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
    public class GstHsnController : Controller
    {
          private GstHSNCode Res;
         
        CmnFunctions com = new CmnFunctions();
        public GstHsnController() :
            this(new GstHsnCodeModel()) { }

        public GstHsnController(GstHSNCode Objist)
        {
            Res = Objist;
        }
        [HttpGet]
        public ActionResult Index()
        {
            List<EntityGstHsn> records = new List<EntityGstHsn>();
            records.Clear();
            records = Res.gethsncode().ToList();
            if (records.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            return View(records);
        }

        [HttpPost]
        public ActionResult Index(string hsncode = null, string command = null)
        {
            List<EntityGstHsn> records = new List<EntityGstHsn>();

            Session["records"] = "";
            if (command == "Search" || command == "Refresh")
            {
                records = Res.getHsncode_list(hsncode).ToList();
                @ViewBag.filter = hsncode;
                if (records.Count == 0)
                {
                    ViewBag.records = "No Records Found";
                }
            }
            else if (command == "Clear")
            {
                records = Res.gethsncode().ToList();
            }
            return View(records);
        }

        [HttpGet]
        public PartialViewResult Create_HSNcode()
        {
            EntityGstHsn TypeModel = new EntityGstHsn();
            return PartialView(TypeModel);
        }

        [HttpPost]
        public JsonResult CreateHSNcode(EntityGstHsn pinmodel)
        {
            try
            {
                string dn = "";
                //if (ModelState.IsValid)
                //{                 
                dn = Res.Inserthsncode(pinmodel);
                RedirectToAction("Index");
                //}

                return Json(dn, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public PartialViewResult Edit_Hsncode(int id, string viewfor)
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

            EntityGstHsn TypeModel = Res.gethsncodeid(id);
            return PartialView(TypeModel);

        }

        [HttpPost]
        public JsonResult EditHsncode(EntityGstHsn pinmodel)
        {
            string result = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    result = Res.Updatehsncode(pinmodel);
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

            EntityGstHsn TypeModel = Res.gethsncodeid(id);            
            return PartialView(TypeModel);
        }


        [HttpPost]
        public JsonResult Deletehsncode(EntityGstHsn pincode)
        {
            string result = string.Empty;
            try
            {
                var ids = pincode.hsn_gid;
                result = Res.Deletehsncode(ids);

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
    }
}
