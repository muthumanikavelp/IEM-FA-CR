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
    public class IEM_CITYCLASSController : Controller
    {
        //
        // GET: /IEM_CITYCLASS/

         private Iiem_mst_cityclass cityclass;
         CmnFunctions com = new CmnFunctions();
       
        public IEM_CITYCLASSController() :
            this(new IEM_MST_CITYCLASS()) { }

        public IEM_CITYCLASSController(Iiem_mst_cityclass Objist) 
        {
            cityclass = Objist;
        }
        public ActionResult Index()
        {
            List<iem_mst_cityclass> cityclassdata = new List<iem_mst_cityclass>();
            cityclassdata = cityclass.getcity().ToList();
            return View(cityclassdata);
        }
        [HttpPost]
        public ActionResult Index(string filter_code = null,string filter_name = null,string command=null)
        {

            List<iem_mst_cityclass> cityclassdata = new List<iem_mst_cityclass>();
            if (command == "Search" || command == "Refresh")
            {
                cityclassdata = cityclass.getcity(filter_code, filter_name).ToList();
                Session["records"] = "";

                @ViewBag.filter_code = filter_code;
                @ViewBag.filter_name = filter_name;
                if (cityclassdata.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }
            if(command=="Clear")
            {
                cityclassdata = cityclass.getcity().ToList();
            }

            return View(cityclassdata);
        }

        [HttpGet]
        public PartialViewResult Create()
        {
            iem_mst_cityclass TypeModel = new iem_mst_cityclass();
            return PartialView(TypeModel);
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
            iem_mst_cityclass TypeModel = cityclass.GetCityById(id);
            return PartialView(TypeModel);
        }

        [HttpPost]
        public JsonResult UpdateCityclass(iem_mst_cityclass TypeModel)
        {
            string result = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    TypeModel.cityclass_update_by = int.Parse(com.GetLoginUserGid().ToString());
                    result = cityclass.UpdateCity(TypeModel);
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
                iem_mst_cityclass TypeModel = cityclass.GetCityById(id);
                return PartialView(TypeModel);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpPost]
        public JsonResult DeleteCityClassDetails(iem_mst_cityclass TypeModel)
        {
            try
            {
                string result = string.Empty;
                var ids = TypeModel.cityclass_gid;
                int uperson = TypeModel.cityclass_update_by;
                uperson = int.Parse(com.GetLoginUserGid().ToString());
                result=cityclass.DeleteCity(ids, uperson);
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult Create(iem_mst_cityclass TypeModel)
        {
            string result = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {
                    TypeModel.cityclass_insert_by = int.Parse(com.GetLoginUserGid().ToString());
                    result = cityclass.InsertCity(TypeModel);
                    if (result =="success") 
                        RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
       
        //
        // GET: /IEM_CITYCLASS/Details/5

        public ActionResult Details(int id)
        {
            return View();
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
