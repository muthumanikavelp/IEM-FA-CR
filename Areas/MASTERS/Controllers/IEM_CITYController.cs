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
    public class IEM_CITYController : Controller
    {
        //
        // GET: /MASTERS/IEM_CITY/
        private Iiem_mst_city city;
        CmnFunctions com = new CmnFunctions();
       
       
        public IEM_CITYController() :
            this(new IEM_MST_CITY()) { }

        public IEM_CITYController(Iiem_mst_city Objist) 
        {
            city = Objist;
        }
        [HttpGet]
        public ActionResult Index()
        {
            List<iem_mst_city> records = new List<iem_mst_city>();
            records = city.getcity().ToList();
           
            return View(records);   
        }
        [HttpPost]
        public ActionResult Index(string filter = null, string filter1 = null, string filter2=null,string command=null)
        {
            List<iem_mst_city> records = new List<iem_mst_city>();
            if (command == "Search" || command == "Refresh") { 
            records = city.getcity_id(filter, filter1, filter2).ToList();
            if (records.Count == 0)
            {
                ViewBag.Message = "No Records Found";
            }
            @ViewBag.filter = filter;
            @ViewBag.filter1 = filter1;
            @ViewBag.filter2 = filter2;
        }
            else if(command=="Clear")
            {
                records = city.getcity().ToList();
            }
            return View(records);   
        }
         [HttpGet]
        public PartialViewResult Create()
        {

                iem_mst_city TypeModel = new iem_mst_city();
                TypeModel.Getstate = new SelectList(city.Getstate(), "stategid", "statename", TypeModel.selectedstate_gid);
                TypeModel.Getregion = new SelectList(city.Getregion(), "regiongid", "regionname", TypeModel.selectedregion_gid);
                TypeModel.Getcountry = new SelectList(city.Getcountry(), "countrygid", "countryname", TypeModel.selectedcountry_gid);
                TypeModel.Getcityclass = new SelectList(city.Getcityclass(), "cityclassgid", "cityclassname", TypeModel.selectedcityclass_gid);
                TypeModel.Gettier = new SelectList(city.Gettier(), "tiergid", "tiername",TypeModel.selectedtier_gid);
                return PartialView(TypeModel);        
        }

        [HttpPost]
        public JsonResult CreateCity(iem_mst_city City)
        {
            string result = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {                  
                
                    GetstateById getstate=city.GetstateById(City.state_gid);
                    City.state_code = getstate.statecode;
                    City.state_name=getstate.statename;

                    GetregionById getreg=city.GetregionById(City.region_gid);                   
                    City.region_name=getreg.regionname;

                     GetcountryById getcountry=city.GetcountryById(City.country_gid);
                    City.country_code = getcountry.countrycode;
                    City.country_name=getcountry.countryname;

                     GetcityclassById getcityclass=city.GetcityclassById(City.cityclass_gid);
                    City.cityclass_code = getcityclass.cityclasscode;

                     GettierById gettier=city.GettierById(City.tier_gid);
                     City.tier_code = gettier.tiercode;
                     City.city_insert_by = int.Parse(com.GetLoginUserGid().ToString());              
                    result= city.InsertCity(City);
                   //if (result == "success") 
                   //    RedirectToAction("Index");
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
           
            iem_mst_city TypeModel=city.GetcityId(id);
            TypeModel.Getstate = new SelectList(city.Getstate(), "stategid", "statename", TypeModel.selectedstate_gid);
            TypeModel.Getregion = new SelectList(city.Getregion(), "regiongid", "regionname", TypeModel.selectedregion_gid);
            TypeModel.Getcountry = new SelectList(city.Getcountry(), "countrygid", "countryname", TypeModel.selectedcountry_gid);
            TypeModel.Getcityclass = new SelectList(city.Getcityclass(), "cityclassgid", "cityclassname", TypeModel.selectedcityclass_gid);
            TypeModel.Gettier = new SelectList(city.Gettier(), "tiergid", "tiername", TypeModel.selectedtier_gid);           
            return PartialView(TypeModel);

        }
         [HttpPost]
         public JsonResult City(iem_mst_city expensen)
         {
             try
             {

                 int Region = expensen.city_gid;
                 return Json(city.GetregionBy_Id(Region), JsonRequestBehavior.AllowGet);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }
         [HttpPost]
         public JsonResult City1(iem_mst_city expensen)
         {
             try
             {

                 int Region = expensen.city_gid;
                 return Json(city.GetcountryBy_Id(Region), JsonRequestBehavior.AllowGet);
             }
             catch (Exception ex)
             {
                 throw ex;
             }
         }
        [HttpPost]
         public JsonResult EditCity(iem_mst_city City)
        {
            string res=string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    GetstateById getstate = city.GetstateById(City.selectedstate_gid);
                    City.state_code = getstate.statecode;
                    City.state_name = getstate.statename;

                    GetregionById getreg = city.GetregionById(City.selectedregion_gid);
                    City.region_name = getreg.regionname;

                    GetcountryById getcountry = city.GetcountryById(City.selectedcountry_gid);
                    City.country_code = getcountry.countrycode;
                    City.country_name = getcountry.countryname;

                    GetcityclassById getcityclass = city.GetcityclassById(City.selectedcityclass_gid);
                    City.cityclass_code = getcityclass.cityclasscode;

                    GettierById gettier = city.GettierById(City.selectedtier_gid);
                    City.tier_code = gettier.tiercode;
                    City.city_update_by = int.Parse(com.GetLoginUserGid().ToString());
                    res= city.UpdateCity(City);
                }
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public JsonResult DeleteCity(iem_mst_city City)
        {
            try
            {

                city.DeleteCity(City.city_gid);
                return Json("Record deleted successfully !", JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //
        // GET: /MASTERS/IEM_CITY/Details/5

        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //
        // GET: /MASTERS/IEM_CITY/Create

        //public ActionResult Create()
        //{
        //    return View();
        //}

        //
        // POST: /MASTERS/IEM_CITY/Create

        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //
        // GET: /MASTERS/IEM_CITY/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /MASTERS/IEM_CITY/Edit/5

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
        // GET: /MASTERS/IEM_CITY/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /MASTERS/IEM_CITY/Delete/5

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
