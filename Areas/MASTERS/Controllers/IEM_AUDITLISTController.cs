using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Areas.MASTERS.Models;
using System.Web.Mvc;
using System.Data;
using IEM.App_Start;
namespace IEM.Areas.MASTERS.Controllers
{

    [NoDirectAccess]
    public class IEM_AUDITLISTController : Controller
    {
        //
        // GET: /MASTERS/IEM_AUDITLIST/

        private Iiem_mst_auditlist auditlist;

        public IEM_AUDITLISTController() :
            this(new IEM_MST_AUDITLIST()) { }

        public IEM_AUDITLISTController(Iiem_mst_auditlist obj)
        {
            auditlist = obj;
        }

        public ActionResult Index()
        {
            List<iem_mst_auditlist> g = new List<iem_mst_auditlist>();
            g = auditlist.GetAuditList().ToList();
            iem_mst_auditlist Typemodel = new iem_mst_auditlist();
            Typemodel.selctgroupname = new SelectList(auditlist.selctgroupname().ToList(), "groupname", "groupname");
            Typemodel.selectsubgroupname = new SelectList(auditlist.selectsubgroupname().ToList(), "subgroupname", "subgroupname");
            ViewBag.groupname = Typemodel;
            ViewBag.subgroupname = Typemodel;
            if (g.Count == 0)
            {
                ViewBag.Message = "No record";
            }
            return View(g);
        }
        [HttpPost]
        public ActionResult Index(string filter_auditlistname = "", string Group = "", string SubGroupname = "", string command = null)
        {

            List<iem_mst_auditlist> records = new List<iem_mst_auditlist>();
            Session["records"] = "";
            iem_mst_auditlist Typemodel = new iem_mst_auditlist();
            Typemodel.selctgroupname = new SelectList(auditlist.selctgroupname().ToList(), "groupname", "groupname");
            Typemodel.selectsubgroupname = new SelectList(auditlist.selectsubgroupname().ToList(), "subgroupname", "subgroupname");
            ViewBag.groupname = Typemodel;
            ViewBag.subgroupname = Typemodel;
            if (command == "Search" || command == "Refresh")
            {
                records = auditlist.GetAuditList(filter_auditlistname, Group, SubGroupname).ToList();
                Session["records"] = records;
                @ViewBag.auditlistname = filter_auditlistname;
                @ViewBag.selctgroupname = Group;
                @ViewBag.selectsubgroupname = SubGroupname;
                if (records.Count == 0)
                {
                    ViewBag.Message = "No records found";
                }
            }
            if (command == "Clear")
            {
                records = auditlist.GetAuditList().ToList();
            }
            return View(records);
        }
        [HttpGet]
        public PartialViewResult Create()
        {
            iem_mst_auditlist Typemodel = new iem_mst_auditlist();
            Typemodel.selctgroupname = new SelectList(auditlist.selctgroupname().ToList(), "groupnamegid", "groupname");
            Typemodel.selectsubgroupname = new SelectList(auditlist.selectsubgroupname().ToList(), "subgroupnamegid", "subgroupname");
            ViewBag.groupname = Typemodel;
            ViewBag.subgroupname = Typemodel;
            return PartialView(Typemodel);
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
            iem_mst_auditlist Typemodel = new iem_mst_auditlist();
            Typemodel = auditlist.GetAuditListById(id);
            Typemodel.selctgroupname = new SelectList(auditlist.selctgroupname().ToList(), "groupnamegid", "groupname", Typemodel.auditgroup_gid);
            Typemodel.selectsubgroupname = new SelectList(auditlist.selectsubgroupname().ToList(), "subgroupnamegid", "subgroupname", Typemodel.auditsubgroup_gid);
           
            return PartialView(Typemodel);
        }
        [HttpPost]
        public JsonResult InsertAuditlist(iem_mst_auditlist insertauditlist)
        {
          
            string res=string.Empty;
            int count = 0;
            DataTable getcount = new DataTable();  
            try
            {
                getcount = auditlist.getauditlistorder();
                foreach (DataRow row in getcount.Rows)
                {
                    if(row["Count"]!=DBNull.Value)
                    {
                        count = int.Parse(getcount.Rows[0]["Count"].ToString());
                        count = count + 1;
                    }
                    else
                    {
                        count = count + 1;
                    }
                }               
            }
            catch(Exception ex)
            {

            }
            finally
            {

            }
           
            insertauditlist.auditlist_order = count;
            res = auditlist.InsertAuditList(insertauditlist);
            return Json(res,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateAuditgroup(iem_mst_auditlist TypeModel)
        {
            string result = "";

            try
            {
                if (ModelState.IsValid)
                {

                    //TypeModel.bank_update_by = int.Parse(com.GetLoginUserGid().ToString());
                    result = auditlist.UpdateAuditGroup(TypeModel);
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
        public JsonResult Dele(iem_mst_auditlist Dele)
        {
            string result = string.Empty;
            result = auditlist.DeleteAuditList(Dele.auditlist_gid);
            if (result == "success") RedirectToAction("Index");
            return Json(result, JsonRequestBehavior.AllowGet);
        }

       
    }
}
