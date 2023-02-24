using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.MASTERS.Models;
using System.Data;
using IEM.App_Start;
namespace IEM.Areas.MASTERS.Controllers
{
    [NoDirectAccess]
    public class IEM_AUDITSUBGROUPController : Controller
    {

        private Iiem_mast_auditsubgroup auditsubgroup;
        public IEM_AUDITSUBGROUPController():
        this(new IEM_MST_AUDITSUBGROUP()) { }

        public IEM_AUDITSUBGROUPController(Iiem_mast_auditsubgroup obj)
        {
            auditsubgroup = obj;
        }

        public ActionResult Index()
        {
            List<iem_mst_auditsubgroup> g = new List<iem_mst_auditsubgroup>();
            iem_mst_auditsubgroup Typemodel = new iem_mst_auditsubgroup();
            Typemodel.selctgroupname = new SelectList(auditsubgroup.selctgroupname().ToList(), "groupname", "groupname");
            ViewBag.SelectGroup = Typemodel;
            g = auditsubgroup.GetAuditsubgroup().ToList();
            if(g.Count==0)
            {
                ViewBag.Message = "No Record";
            }
            return View(g);
        }
        [HttpPost]
        public ActionResult Index(string filter_subgroupname, string filter_auditgroupname,string command=null)
        {
            List<iem_mst_auditsubgroup> g = new List<iem_mst_auditsubgroup>();
            iem_mst_auditsubgroup Typemodel = new iem_mst_auditsubgroup();
            Typemodel.selctgroupname = new SelectList(auditsubgroup.selctgroupname().ToList(), "groupname", "groupname");
            ViewBag.SelectGroup = Typemodel;
            if(command=="Search" || command=="Refresh")
            {

                ViewBag.selctgroupname = filter_auditgroupname;
                ViewBag.filter_subgroupname = filter_subgroupname;
                g = auditsubgroup.GetAuditsubgroup(filter_subgroupname, filter_auditgroupname).ToList();
                if (g.Count == 0)
                {
                    ViewBag.Message = "No Record Found";
                }
            }
            else
            {
                g = auditsubgroup.GetAuditsubgroup().ToList();
            }
          
           
            return View(g);
        }
        [HttpGet]
        public PartialViewResult Create()
        {
            iem_mst_auditsubgroup TypeModel = new iem_mst_auditsubgroup();
            TypeModel.selctgroupname = new SelectList(auditsubgroup.selctgroupname().ToList(), "groupnamegid", "groupname");
            ViewBag.SelectGroup = TypeModel;
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
            iem_mst_auditsubgroup TypeModel = auditsubgroup.GetAuditsubgroupById(id);
            TypeModel.selctgroupname = new SelectList(auditsubgroup.selctgroupname().ToList(), "groupnamegid", "groupname", TypeModel.auditgroup_gid);            
            return PartialView(TypeModel);
        }
        [HttpPost]
        public JsonResult UpdateAuditSubgroup(iem_mst_auditsubgroup TypeModel)
        {
            string result = "";
            try
            {
                if (ModelState.IsValid)
                {
                    TypeModel.auditgroup_name = auditsubgroup.getauditgroupname(TypeModel.auditsubgroup_auditgroup_gid);
                    result = auditsubgroup.UpdateAuditsubgroup(TypeModel);
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
        public JsonResult Insertauditsubgroup(iem_mst_auditsubgroup TypeModel)
        {
            string result = string.Empty;
            string auditgroupname = string.Empty;
            string count = string.Empty;
            int incre=0;

            try
            {
                if (ModelState.IsValid)
                {
                    TypeModel.auditgroup_name = auditsubgroup.getauditgroupname(TypeModel.auditsubgroup_auditgroup_gid);
                    count = auditsubgroup.gotcount();
                    if(count=="")
                    {
                        incre = incre + 1;
                    }
                    else
                    {
                        incre = int.Parse(count);
                        incre = incre + 1;
                    }
                    TypeModel.auditsubgroup_order = incre;
                    result = auditsubgroup.InsertAuditsubgroup(TypeModel);                   
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
        public JsonResult DeleteAuditsubgroup(iem_mst_auditsubgroup TypeModel)
        {
            string lsresult = string.Empty;
            try
            {
                lsresult = auditsubgroup.DeleteAuditsubgroup(TypeModel.auditsubgroup_gid);                
                return Json(lsresult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }      

       
    }
}
