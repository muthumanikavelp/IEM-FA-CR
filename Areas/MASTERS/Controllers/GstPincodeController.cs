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
    public class GstPincodeController : Controller
    {
        private Gstpincode Res; 
        CmnFunctions com = new CmnFunctions();

        public GstPincodeController() :
            this(new GetPincodeModel()) { }

        public GstPincodeController(Gstpincode Objist)
        {
            Res = Objist;
        }
        [HttpGet]
        public ActionResult Index()
        {
            List<EntityGSTPincode> records = new List<EntityGSTPincode>();            
            records.Clear();
        
            records = Res.getPincode().ToList();
            if (records.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            return View(records);
        }
        [HttpPost]
        public ActionResult Index(string pincode = null, string command = null)
        {
            List<EntityGSTPincode> records = new List<EntityGSTPincode>();

            Session["records"] = "";
            if (command == "Search" || command == "Refresh")
            {
                records = Res.getPincode_list(pincode).ToList();
                @ViewBag.filter = pincode; 
                if (records.Count == 0)
                {
                    ViewBag.records = "No Records Found";
                }
            }
            else if (command == "Clear")
            {
                records = Res.getPincode().ToList();
            }
            return View(records);
        }


        [HttpGet]         
        public PartialViewResult Create_Pincode()
        {
            EntityGSTPincode TypeModel = new EntityGSTPincode();
            TypeModel.GetDistrict = new SelectList(Res.GetDistrict(), "Pincode_district_gid", "Pincode_district_name");          
            TypeModel.GetState = new SelectList(Res.GetState(), "Pincode_state_gid", "Pincode_state_name");            
            return PartialView(TypeModel);
        }
       
        
        [HttpPost]
        public JsonResult CreatePincode(EntityGSTPincode pinmodel)
        {
            try
            {
                string dn = "";
                //if (ModelState.IsValid)
                //{
                    EntityGSTPincode getdis = Res.GetDistrictById(pinmodel.Pincode_district_gid);
                    EntityGSTPincode getstate = Res.GetStateById(pinmodel.Pincode_state_gid);
                   // pinmodel.selecteddistrict_gid = getdis.selecteddistrict_gid;  
                    dn = Res.Insertpincode(pinmodel);
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
        public PartialViewResult Edit_Pincode(int id, string viewfor)
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

            EntityGSTPincode TypeModel = Res.getPincodeId(id);
            TypeModel.GetState = new SelectList(Res.GetState(), "Pincode_state_gid", "Pincode_state_name", TypeModel.selectedstate_gid);
            EntityGSTPincode sss = new EntityGSTPincode();
            sss.selectedstate_gid = TypeModel.selectedstate_gid;
            TypeModel.GetDistrict = new SelectList(Res.GetDistrict(), "Pincode_district_gid", "Pincode_district_name", TypeModel.selecteddistrict_gid);
           
            return PartialView(TypeModel);

        }

         [HttpPost]
         public JsonResult Editpincode(EntityGSTPincode pinmodel)
         {
             string result = string.Empty;
             try
             {
                 if (ModelState.IsValid)
                 {
                     result = Res.Updatepincode(pinmodel);
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

            EntityGSTPincode TypeModel = Res.getPincodeId(id);
            TypeModel.GetDistrict = new SelectList(Res.GetDistrict(), "Pincode_district_gid", "Pincode_district_name", TypeModel.selecteddistrict_gid);
            return PartialView(TypeModel);
        }

       
        [HttpPost]
        public JsonResult DeletePincode(EntityGSTPincode pincode)
        {
            string result = string.Empty;
            try
            {
                var ids = pincode.Pincode_gid;
                result = Res.Deletepincode(ids);

                return Json(result, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //Get district by selected dropdown

        [HttpPost]
        public JsonResult GetDistictby_State(EntityGSTPincode stateid)
        {
            try
            {

                int id = stateid.selectedstate_gid;
                return Json(Res.GetDistrictByStateId(id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
