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
    public class IEM_MST_HOTELTYPEController : Controller
    {
        private Iiem_mst_hotel hoteltype;
         CmnFunctions com = new CmnFunctions();
        public IEM_MST_HOTELTYPEController() :
             this(new IEM_MST_HOTELTYPE()) { }

        public IEM_MST_HOTELTYPEController(Iiem_mst_hotel Objist) 
        {
            hoteltype = Objist;
        }

        public ActionResult Index()
        {
            List<iem_mst_hotel> cour = new List<iem_mst_hotel>();
            cour = hoteltype.GetHotel().ToList();
            if (cour.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            return View(cour);
        }
        [HttpPost]
        public ActionResult Index(string filter_name = null, string command = null)
        {
            List<iem_mst_hotel> cour = new List<iem_mst_hotel>();
            if (command == "Search" || command == "Refresh")
            {
                cour = hoteltype.GetHotel(filter_name).ToList();
                @ViewBag.filter_name = filter_name;
                if (cour.Count == 0)
                {
                    ViewBag.records = "No Records Found";
                }
            }
            if (command == "Clear")
            {
                cour = hoteltype.GetHotel().ToList();
            }
            return View(cour);
        }


        [HttpGet]
        public PartialViewResult Create()
        {
            iem_mst_courier TypeModel = new iem_mst_courier();
            return PartialView(TypeModel);
        }

        [HttpPost]
        public JsonResult CreateHotel(iem_mst_hotel TypeModel)
        {
            string result = "";

            try
            {
                if (ModelState.IsValid)
                {
                    //TypeModel.courier_insert_by = int.Parse(com.GetLoginUserGid().ToString());
                    result = hoteltype.InsertHotel(TypeModel);

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
            iem_mst_hotel TypeModel = hoteltype.GetHotelById(id);
            return PartialView(TypeModel);
        }
        [HttpPost]
        public JsonResult UpdateHotelDetails(iem_mst_hotel TypeModel)
        {
            string result = "";

            try
            {
                if (ModelState.IsValid)
                {
                   // TypeModel.courier_update_by = int.Parse(com.GetLoginUserGid().ToString());
                    result = hoteltype.UpdateHotel(TypeModel);
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
        public JsonResult DeleteHotel(iem_mst_hotel TypeModel)
        {
            string res = string.Empty;
            try
            {
               // var ids = TypeModel.courier_gid;
                res = hoteltype.DeleteHotel(TypeModel.hoteltype_gid);
                return Json(res, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        

       
    }
}
