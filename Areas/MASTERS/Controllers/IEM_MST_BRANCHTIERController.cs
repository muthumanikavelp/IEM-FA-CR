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
    public class IEM_MST_BRANCHTIERController : Controller
    {
        //
        // GET: /MASTERS/IEM_MST_BRANCHTIER/
           private Iiem_mst_branchtier branchtier;
         CmnFunctions com = new CmnFunctions();
         public IEM_MST_BRANCHTIERController() :
             this(new IEM_MST_BRANCHTIER()) { }

         public IEM_MST_BRANCHTIERController(Iiem_mst_branchtier Objist) 
        {
            branchtier = Objist;
        }
        public ActionResult Index()
        {
            List<iem_mst_branchtier> cour = new List<iem_mst_branchtier>();
            cour = branchtier.getbranchtier().ToList();
            if (cour.Count == 0)
            {
                ViewBag.records = "No Records Found";
            }
            return View(cour);
        }
        [HttpPost]
        public ActionResult Index(string filter_name = null, string command = null)
        {
            List<iem_mst_branchtier> cour = new List<iem_mst_branchtier>();
            if (command == "Search" || command == "Refresh")
            {
                cour = branchtier.getbranchtier(filter_name).ToList();
                @ViewBag.filter_name = filter_name;
                if (cour.Count == 0)
                {
                    ViewBag.records = "No Records Found";
                }
            }
            if (command == "Clear")
            {
                cour = branchtier.getbranchtier().ToList();
            }
            return View(cour);
        }
        [HttpGet]
        public PartialViewResult Create()
        {
            iem_mst_branchtier TypeModel = new iem_mst_branchtier();
            return PartialView(TypeModel);
        }
        [HttpPost]
        public JsonResult CreateBranchtier(iem_mst_branchtier TypeModel)
        {
            string result = "";

            try
            {
                if (ModelState.IsValid)
                {
                    //TypeModel.courier_insert_by = int.Parse(com.GetLoginUserGid().ToString());
                    result = branchtier.Insertbranchtier(TypeModel);
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
            iem_mst_branchtier TypeModel = branchtier.GetbranchtierById(id);
            return PartialView(TypeModel);
        }       
        [HttpPost]
        public JsonResult UpdateBranchtier(iem_mst_branchtier TypeModel)
        {
            string result = "";

            try
            {
                if (ModelState.IsValid)
                {
                    //TypeModel.courier_update_by = int.Parse(com.GetLoginUserGid().ToString());
                    result = branchtier.Updatebranchtier(TypeModel);
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
        public JsonResult DeleteBranchtier(iem_mst_branchtier TypeModel)
        {
            string res = string.Empty;
            try
            {
               // var ids = TypeModel.courier_gid;
                res = branchtier.Deletebranchtier(TypeModel.branchtier_gid);
                return Json(res, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        
    }
}
