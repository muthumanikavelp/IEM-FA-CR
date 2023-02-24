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
    public class IEM_MST_BRANCHTYPEController : Controller
    {
        //
        // GET: /MASTERS/IEM_MST_BRANCHTYPE/
        private Iiem_mst_branchtype branchtype;
        CmnFunctions com = new CmnFunctions();

        public IEM_MST_BRANCHTYPEController() :
            this(new IEM_MST_BRANCHTYPE()) { }

        public IEM_MST_BRANCHTYPEController(Iiem_mst_branchtype Objist) 
        {
            branchtype = Objist;
        }
        public ActionResult Index()
        {
            List<iem_mst_branchtype> g = new List<iem_mst_branchtype>();
            g = branchtype.getbranchtype().ToList();
            return View(g);
        }
        [HttpPost]
        public ActionResult Index(string filter_branchtype = "", string command = null)
        {

            List<iem_mst_branchtype> records = new List<iem_mst_branchtype>();
            Session["records"] = "";
            if (command == "Search" || command == "Refresh")
            {
                records = branchtype.getbranchtype(filter_branchtype).ToList();
                Session["records"] = records;
                @ViewBag.branchtype = filter_branchtype;               
                if (records.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }
            if (command == "Clear")
            {
                records = branchtype.getbranchtype().ToList();
            }
            return View(records);
        }
        [HttpGet]
        public PartialViewResult Create()
        {
            iem_mst_branchtype TypeModel = new iem_mst_branchtype();
            return PartialView(TypeModel);
        }
        [HttpPost]
        public JsonResult Create(iem_mst_branchtype TypeModel)
        {
            string result = "";

            try
            {
                if (ModelState.IsValid)
                {
                   // TypeModel.bank_insert_by = int.Parse(com.GetLoginUserGid().ToString());
                    result = branchtype.Insertbranchtype(TypeModel);

                    if (result == "success") RedirectToAction("Index");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
            iem_mst_branchtype TypeModel = branchtype.GetbranchtypeById(id);
            return PartialView(TypeModel);
        }
        [HttpPost]
        public JsonResult UpdateBRanchtypeDetails(iem_mst_branchtype TypeModel)
        {
            string result = "";

            try
            {
                if (ModelState.IsValid)
                {

                   // TypeModel.bank_update_by = int.Parse(com.GetLoginUserGid().ToString());
                    result = branchtype.Updatebranchtype(TypeModel);
                    if (result == "success") RedirectToAction("Index");
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteBranchtype(iem_mst_branchtype TypeModel)
        {
            string lsresult = string.Empty;
            try
            {
                //var ids = TypeModel.bank_gid;
                lsresult = branchtype.Deletebranchtype(TypeModel.branchtype_gid);
                ViewBag.viewfor = "delete";
                return Json(lsresult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }      
       
    }
}
