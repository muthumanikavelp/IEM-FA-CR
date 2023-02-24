using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.MASTERS.Models;
using IEM.App_Start;

namespace IEM.Areas.MASTERS.Controllers
{

    [NoDirectAccess]
    public class IEM_MST_ACCOMODATIONController : Controller
    {
        //
        // GET: /MASTERS/IEM_MST_ACCOMODATION/
        private Iiem_mst_accomodationtype accomotype;
        public IEM_MST_ACCOMODATIONController():
            this(new IEM_MST_ACCOMODATION()) { }
        public IEM_MST_ACCOMODATIONController(Iiem_mst_accomodationtype obj)
        {
            accomotype = obj;
        }
        public ActionResult Index()
        {
            List<iem_mst_accomodationtype> record = new List<iem_mst_accomodationtype>();
            record = accomotype.GetAccomodationtype().ToList();
            if(record.Count==0)
            {
                ViewBag.Message = "No Record";
            }
            return View(record);
        }
        [HttpPost]
        public ActionResult Index(string filter_accommodationtype, string command = null)
        {
            List<iem_mst_accomodationtype> record = new List<iem_mst_accomodationtype>();
            if (command == "Refresh" || command == "Search")
            {

                record = accomotype.GetAccomodationtype(filter_accommodationtype).ToList();
                ViewBag.filter_accommodationtype = filter_accommodationtype;
                if (record.Count == 0)
                {
                    ViewBag.Message = "No Record's Found";
                }
            }
            else
            {
                record = accomotype.GetAccomodationtype().ToList();
            }            
            return View(record);
        }

        [HttpGet]
        public PartialViewResult Create()
        {
            iem_mst_accomodationtype TypeModel = new iem_mst_accomodationtype();
            return PartialView(TypeModel);
        }


        [HttpPost]
        public JsonResult CreateAccomadation(iem_mst_accomodationtype TypeModel)
        {
            string result = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    result = accomotype.Insertaccomodation(TypeModel);
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
            iem_mst_accomodationtype TypeModel = accomotype.GetAccomodationtypeById(id);
            return PartialView(TypeModel);
        }
        [HttpPost]
        public JsonResult Updateaccomodationdetails(iem_mst_accomodationtype TypeModel)
        {
            string result = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    result = accomotype.UpdateAccomodationtype(TypeModel);
                    if (result == "success") RedirectToAction("Index");
                }

            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
       
    }
}
