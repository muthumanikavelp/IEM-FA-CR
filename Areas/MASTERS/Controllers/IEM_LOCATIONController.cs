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
    public class IEM_LOCATIONController : Controller
    {
        //
        // GET: /MASTERS/IEM_LOCATION/
        private Iiem_mst_location location;
        public IEM_LOCATIONController() :
            this(new IEM_MST_LOCATION()) { }

        public IEM_LOCATIONController(Iiem_mst_location Objist) 
        {
            location = Objist;
        }

        public ActionResult Index()
        {
            List<iem_mst_location> getrecord = new List<iem_mst_location>();           
           
            iem_mst_location typemodel = new iem_mst_location();
            typemodel.GetCity = new SelectList(location.GetCity().ToList(), "cityname", "cityname");
            typemodel.Gettier = new SelectList(location.Gettier().ToList(), "tiername", "tiername");
            ViewBag.City = typemodel;
            ViewBag.Tier = typemodel;
            if (Session["RecorsLoc"] != null)
            {
                iem_mst_location Viewbags = new iem_mst_location();
                Viewbags = (iem_mst_location)Session["Locationsearch"];
                ViewBag.cityvalue = Viewbags.location_city;
                ViewBag.tiervalue = Viewbags.location_tier;

            }
            else
            {
                Session["Locationsearch"] = null;
                Session["RecorsLoc"] = null;
                getrecord = location.getlocation().ToList();

            }
            if (getrecord.Count==0)
            {
                ViewBag.Message = "No Record";
            }
            return View(getrecord);
        }
        [HttpPost]
        public ActionResult Index(string filter_code, string filter_name, string City, string Tier, string command = null)
        {
            iem_mst_location viewbag = new iem_mst_location();
            iem_mst_location typemodel = new iem_mst_location();
            typemodel.GetCity = new SelectList(location.GetCity().ToList(), "cityname", "cityname");
            typemodel.Gettier = new SelectList(location.Gettier().ToList(), "tiername", "tiername");
            ViewBag.City = typemodel;
            ViewBag.Tier = typemodel;
            string tempcity = string.Empty;
            tempcity = City;
             string temptier = string.Empty;
            temptier = Tier;
            List<iem_mst_location> getrecord = new List<iem_mst_location>();
            if (command == "Refresh" || command == "Search")
            {
                if (City == "-----Select----")
                {
                    City = "";
                }else
                {
                    City = tempcity;
                }
                if (Tier == "-----Select----")
                {
                    Tier = "";
                }
                else
                {
                    Tier = temptier;
                }
                getrecord = location.getlocation(filter_code, filter_name,City,Tier).ToList();
                Session["RecorsLoc"] = getrecord;
                if (getrecord.Count == 0)
                {
                    ViewBag.Message = "No Record";
                }
                viewbag.location_city = City;
                viewbag.location_tier = Tier;
                Session["Locationsearch"] = viewbag;
                ViewBag.cityvalue = tempcity;
                ViewBag.tiervalue = temptier;
                ViewBag.filter_code = filter_code;
                ViewBag.filter_name = filter_name;
            }
            else
            {
                Session["Locationsearch"] = null;
                Session["RecorsLoc"] = null;
                getrecord = location.getlocation().ToList();
            }
            return View(getrecord);
        }
        [HttpGet]
        public PartialViewResult Create()
        {
            iem_mst_location typemodel = new iem_mst_location();
            typemodel.GetCity = new SelectList(location.GetCity().ToList(), "citygid", "cityname");
            typemodel.Gettier = new SelectList(location.Gettier().ToList(), "tiergid", "tiername");
            ViewBag.City = typemodel;
            ViewBag.Tier = typemodel;
            return PartialView(typemodel);

        }
        [HttpPost]
        public JsonResult create(iem_mst_location insertlocationinformation)
        {
            string res = string.Empty;
            res = location.Insertlocation(insertlocationinformation);
            return Json(res, JsonRequestBehavior.AllowGet);
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
            iem_mst_location TypeModel = location.GetlocationrById(id);
            TypeModel.GetCity = new SelectList(location.GetCity().ToList(), "citygid", "cityname",TypeModel.location_city_gid);
            TypeModel.Gettier = new SelectList(location.Gettier().ToList(), "tiergid", "tiername",TypeModel.location_tier_gid);
            return PartialView(TypeModel);
        }
        [HttpPost]
        public JsonResult UpdateLocation(iem_mst_location updatelocationinformation)
        {
            string res = string.Empty;
            res = location.Updatelocation(updatelocationinformation);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteLocationdetails(iem_mst_location deleteloc)
        {
            string res = string.Empty;
            res = location.Deletelocation(deleteloc.location_gid);
            return Json(res,JsonRequestBehavior.AllowGet);
        }
      
    }
}
