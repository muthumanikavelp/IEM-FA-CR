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
    public class IEM_STATEController : Controller
    {
        //
        // GET: /IEM_STATE/
        private Iiem_mst_state state;
        CmnFunctions com = new CmnFunctions();
       
        public IEM_STATEController() :
            this(new IEM_MST_STATE()) { }

        public IEM_STATEController(Iiem_mst_state Objist) 
        {
            state = Objist;
        }
        [HttpGet]
        public ActionResult Index()
        {
            List<iem_mst_state> records = new List<iem_mst_state>();
            records = state.getstate().ToList();
            if (records.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            return View(records);  
        }
        [HttpPost]
        public ActionResult Index(string filter = null,string filter1=null,string command=null)
        {

            List<iem_mst_state> records = new List<iem_mst_state>();
            records = state.getstateid(filter, filter1).ToList();
            if (command == "Search" || command == "Refresh")
            {
                if (records.Count == 0)
                {
                    ViewBag.records = "No Records Found";
                }
                @ViewBag.filter = filter;
                @ViewBag.filter1 = filter1;
            }
            if(command=="Clear")
            {
                records = state.getstate().ToList();
            }
            return View(records);
        }
        [HttpGet]
        public PartialViewResult Create()
        {
            iem_mst_state TypeModel = new iem_mst_state();               
            TypeModel.Getregion = new SelectList(state.Getregion(), "regiongid", "regionname");
            TypeModel.Getcountry = new SelectList(state.Getcountry(), "countrygid", "countryname");
            return PartialView(TypeModel);           
           
        }

        [HttpPost]
        public JsonResult CreateState(iem_mst_state State)
        {
            string result = string.Empty;
            try
            {               
                if (ModelState.IsValid)
                {

                    GetregionById getreg = state.GetregionById(State.region_gid);
                    State.region_name = getreg.regionname;

                    GetcountryById getcountry = state.GetcountryById(State.country_gid);
                    State.country_code = getcountry.countrycode;
                    State.country_name = getcountry.countryname;

                    State.state_insert_by = int.Parse(com.GetLoginUserGid().ToString());
                    result = state.InsertState(State);
                    if (result == "success") RedirectToAction("Index");

                    //GetregionById getreg = state.GetregionById(State.region_gid);
                    //GetcountryById getcountry = state.GetcountryById(State.country_gid);
                    //State.country_code = getcountry.countrycode;
                    //State.state_insert_by = int.Parse(com.GetLoginUserGid().ToString());
                    //result = state.InsertState(State);
                    //RedirectToAction("Index");
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
            iem_mst_state TypeModel = state.GetStateId(id);
            TypeModel.Getregion = new SelectList(state.Getregion(), "regiongid", "regionname", TypeModel.selectedregion_gid);            
            TypeModel.Getcountry = new SelectList(state.Getcountry(), "countrygid", "countryname", TypeModel.selectedcountry_gid); 
            return PartialView(TypeModel);
        }
        [HttpPost]
        public JsonResult UpdateState(iem_mst_state State)
        {
            string res=string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    GetregionById getreg = state.GetregionById(State.selectedregion_gid);
                    State.region_name = getreg.regionname;

                    GetcountryById getcountry = state.GetcountryById(State.selectedcountry_gid);
                    State.country_code = getcountry.countrycode;

                    State.state_update_by = int.Parse(com.GetLoginUserGid().ToString());
                    res=state.UpdateState(State);
                }
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        [HttpPost]
        public JsonResult DeleteState(iem_mst_state State)
        {
            string res = string.Empty;
            try
            {

                var ids = State.state_gid;
               
               res= state.DeleteState(ids);
                return Json(res, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //
        // GET: /IEM_STATE/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /IEM_STATE/Create

        //public ActionResult Create()
        //{
        //    return View();
        //}

        //
        // POST: /IEM_STATE/Create

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
        // GET: /IEM_STATE/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /IEM_STATE/Edit/5

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
        // GET: /IEM_STATE/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /IEM_STATE/Delete/5

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
