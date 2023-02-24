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
    public class IEM_COURIERController : Controller
    {
             
         private Iiem_mst_courier courier;
         CmnFunctions com = new CmnFunctions();
        public IEM_COURIERController() :
             this(new IEM_MST_COURIER()) { }

        public IEM_COURIERController(Iiem_mst_courier Objist) 
        {
            courier = Objist;
        }
        public ActionResult Index()
        {
            List<iem_mst_courier> cour = new List<iem_mst_courier>();
            cour = courier.getcourier().ToList();
            return View(cour);
        }


        [HttpPost]
        public ActionResult Index(string filter_name = null,string command=null)
        {
            List<iem_mst_courier> cour = new List<iem_mst_courier>();
            if (command == "Search" || command == "Refresh")
            {
                cour = courier.getcourier(filter_name).ToList();
                @ViewBag.filter_name = filter_name;
                if (cour.Count == 0)
                {
                    ViewBag.records = "No Records Found";
                }
            }
            if(command=="Clear")
            {
                cour = courier.getcourier().ToList();
            }
            return View(cour);
        }
        //
        // GET: /IEM_COURIER/Details/5

      
        [HttpGet]
        public PartialViewResult Create()
        {
            iem_mst_courier TypeModel = new iem_mst_courier();
            return PartialView(TypeModel);
        }


        [HttpPost]
        public JsonResult CreateCourier(iem_mst_courier TypeModel)
        {
            string result = "";

            try
            {
                if (ModelState.IsValid)
                {
                    TypeModel.courier_insert_by = int.Parse(com.GetLoginUserGid().ToString());
                    result = courier.InsertCourier(TypeModel);

                    if (result == "success") RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Details(int id)
        {
            return View();
        }

       

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
            iem_mst_courier TypeModel = courier.GetCourierById(id);
            return PartialView(TypeModel);
        }
        [HttpGet]
        public PartialViewResult ViewCourier(int id)
        {
            iem_mst_courier TypeModel = courier.GetCourierById(id);
            return PartialView(TypeModel);
        }
        [HttpPost]
        public JsonResult UpdateCourierDetails(iem_mst_courier TypeModel)
        {
            string result = "";

            try
            {
                if (ModelState.IsValid)
                {
                    TypeModel.courier_update_by = int.Parse(com.GetLoginUserGid().ToString());
                    result = courier.UpdateCourier(TypeModel);
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
                iem_mst_courier TypeModel = courier.GetCourierById(id);
                return PartialView(TypeModel);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public JsonResult DeleteCourier(iem_mst_courier TypeModel)
        {
            string res = string.Empty;
            try
            {
                var ids = TypeModel.courier_gid;              
                res= courier.DeleteCourier(ids);
                return Json(res, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
    }
}
