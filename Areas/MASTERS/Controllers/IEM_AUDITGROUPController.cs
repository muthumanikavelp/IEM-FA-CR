using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.MASTERS.Models;
using System.Web.Mvc;
using IEM.App_Start;
namespace IEM.Areas.MASTERS.Controllers
{

     [NoDirectAccess]
    public class IEM_AUDITGROUPController : Controller
    {
        //
        // GET: /MASTERS/IEM_AUDITGROUP/
        private Iiem_mst_auditgroup auditgroup;
        public IEM_AUDITGROUPController() :
            this (new IEM_MST_AUDITGROUP()){ }

        public IEM_AUDITGROUPController(Iiem_mst_auditgroup objlist)
        {
            auditgroup = objlist;
        }
        public ActionResult Index()
        {
            iem_mst_auditgroup typemodel = new iem_mst_auditgroup();
            typemodel.doctype = new SelectList(auditgroup.doctype().ToList(), "doctypename", "doctypename");
            typemodel.docsubtype = new SelectList(auditgroup.docsubtype().ToList(), "docsubtypename", "docsubtypename");
            typemodel.doccat = new SelectList(auditgroup.doccat().ToList(), "doccatname", "doccatname");
            ViewBag.doctype = typemodel;
            ViewBag.docsubtype = typemodel;
            ViewBag.doccat = typemodel;
            List<iem_mst_auditgroup> rec = new List<iem_mst_auditgroup>();
            rec = auditgroup.GetAuditGroup().ToList();
            if (rec.Count == 0)
            {
                ViewBag.Message = "No Record";
            }
            return View(rec);
        }
        [HttpPost]
        public ActionResult Index(string filter_auditgroupname,string ADocType,string Adocsubtype,string Adoccat, string command=null)
        {
            List<iem_mst_auditgroup> rec = new List<iem_mst_auditgroup>();
          
            iem_mst_auditgroup typemodel = new iem_mst_auditgroup();
            typemodel.doctype = new SelectList(auditgroup.doctype().ToList(), "doctypename", "doctypename");
            typemodel.docsubtype = new SelectList(auditgroup.docsubtype().ToList(), "docsubtypename", "docsubtypename");
            typemodel.doccat = new SelectList(auditgroup.doccat().ToList(), "doccatname", "doccatname");
            ViewBag.doctype = typemodel;
            ViewBag.docsubtype = typemodel;
            ViewBag.doccat = typemodel;
            if(command=="Refresh" || command=="Search")
            {
                ViewBag.doccategory = Adoccat;
                ViewBag.docsubtype1 = Adocsubtype;
                ViewBag.doctype1 = ADocType;
                rec = auditgroup.GetAuditGroup(filter_auditgroupname, ADocType, Adocsubtype, Adoccat).ToList();
                if(rec.Count==0)
                {
                    ViewBag.Message = "No Record's Found";
                }
            }
            else
            {
                rec = auditgroup.GetAuditGroup().ToList();
            }

           
            return View(rec);
        }
        [HttpGet]
        public PartialViewResult Create()
        {
            iem_mst_auditgroup typemodel = new iem_mst_auditgroup();
            typemodel.doctype = new SelectList(auditgroup.doctype().ToList(), "doctypegid", "doctypename");
            typemodel.docsubtype = new SelectList(auditgroup.docsubtype().ToList(), "docsubtypegid", "docsubtypename");
            typemodel.doccat = new SelectList(auditgroup.doccat().ToList(), "doccatgid", "doccatname");
            ViewBag.doctype = typemodel;
            ViewBag.docsubtype = typemodel;
            ViewBag.doccat = typemodel;
            return PartialView(typemodel);
        }
        [HttpPost]
        public JsonResult InsertAuditgroup(iem_mst_auditgroup InsertAudit)
        {
            string res=string.Empty;
            res = auditgroup.InsertAuditGroup(InsertAudit);
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
           iem_mst_auditgroup typemodel = auditgroup.GetAuditgroupById(id);          
           typemodel.doctype = new SelectList(auditgroup.doctype().ToList(), "doctypegid", "doctypename", typemodel.doctype_gid);
           typemodel.docsubtype = new SelectList(auditgroup.docsubtype().ToList(), "docsubtypegid", "docsubtypename", typemodel.docsubtype_gid);
           typemodel.doccat = new SelectList(auditgroup.doccat().ToList(), "doccatgid", "doccatname", typemodel.doccat_gid);          
           return PartialView(typemodel);
        }
        [HttpPost]
        public JsonResult UpdateLocation(iem_mst_auditgroup updategroup)
        {
            string res = string.Empty;
            res = auditgroup.UpdateAuditGroup(updategroup);
            return Json(res,JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteAuditgroup(iem_mst_auditgroup delete)
        {
            string res = string.Empty;
            res = auditgroup.DeleteAuditGroup(delete.auditgroup_gid);
            return Json(res, JsonRequestBehavior.AllowGet);
        }
       
       
    }
}
