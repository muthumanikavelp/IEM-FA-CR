using IEM.Areas.MASTERS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.App_Start;

namespace IEM.Areas.MASTERS.Controllers
{
    [NoDirectAccess]
    public class IEM_ROLEController : Controller
    {
        //
        // GET: /MASTERS/IEM_ROLE/

        private Iiem_mst_role role;


        public IEM_ROLEController()
            : this(new IEM_MST_ROLE())
        {
        }

        public IEM_ROLEController(Iiem_mst_role rol)
        {
            role = rol;
        }

        public ActionResult Index()
        {
            iem_mst_role typemodel = new iem_mst_role();

            //typemodel = role.getrole().ToList();
            typemodel = role.menu();

            if (typemodel.RoleList.Count == 0)
            {
                ViewBag.alert = "No Records";
            }
            return View(typemodel);
        }
        [HttpGet]
        public PartialViewResult Create()
        {

            iem_mst_role typemodel = new iem_mst_role();

            typemodel = role.menu();
            typemodel.Getrolegroup = new SelectList(role.Getrolegroupvl(), "rolegroup_gid", "rolegroup_name", 0);
            return PartialView(typemodel);
        }

        [HttpPost]
        public JsonResult Delete1(iem_mst_role custmodel)
        {
            string lsresult;
            try
            {
               lsresult= role.getroledelete(custmodel.role_gid);
               return Json(lsresult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public PartialViewResult Edit(int id, string viewfor)
        {
            iem_mst_role typemodel = new iem_mst_role();
            if (viewfor == "view")
            {
                ViewBag.viewfor = "view";
                typemodel = role.Iem_Mst_Role(id.ToString());
                typemodel.Getrolegroup = new SelectList(role.Getrolegroupvl(), "rolegroup_gid", "rolegroup_name", typemodel.role_rolegroup_gid);

            }
            else if (viewfor == "edit")
            {
                ViewBag.viewfor = "edit";
                typemodel = role.Iem_Mst_Role(id.ToString());
                typemodel.Getrolegroup = new SelectList(role.Getrolegroupvl(), "rolegroup_gid", "rolegroup_name", typemodel.role_rolegroup_gid);

            }
            return PartialView(typemodel);
        }
        
        [HttpPost]
        public JsonResult UpdateHolidayNewAdd(iem_mst_role Student, string[] Selectedrole, string[] unselectedrole)
        {
            String result = String.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    result = role.update(Student, Selectedrole, unselectedrole);
                    if (result == "INS")
                    {
                        result = "Updated Saved";
                    }

                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        [HttpPost]
        public JsonResult HolidayNewAdd(iem_mst_role student, string[] Selectedrole, string[] unselectedrole)
        {

            String result = String.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    result = role.AddHolidayState(student, Selectedrole, unselectedrole);
                    if (result == "EXISTS")
                    {
                        result = "Treeview Already Exists";
                    }
                   // result = role.AddHolidayState(student, Selectedrole, unselectedrole);
                    if (result == "INS")
                    {
                        result = "Successfully Saved";
                    }

                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult Index(string rolecode = "", string rolename = "", string rolegroupname = "")
        {
            iem_mst_role Searchobj = new iem_mst_role();
            List<iem_mst_role> searchrole = new List<iem_mst_role>();
            searchrole = role.searchrole(rolecode, rolename, rolegroupname).ToList();
            Searchobj.RoleList = searchrole;
            @ViewBag.role_code = rolecode;
            @ViewBag.role_name = rolename;
            @ViewBag.rolegroup_name = rolegroupname;

            if (searchrole.Count == 0)
            {
                ViewBag.alert = "No Records";
                ViewBag.searchrole = "No Records Found";
            }

            return View(Searchobj);
        }



        public ActionResult excelexport()
        {
           
            DataTable dtc = new DataTable();
            dtc.Columns.Add("Role Code");
            dtc.Columns.Add("Role Name");
            dtc.Columns.Add("Role Group");
            dtc.Columns.Add("Assigned To");
            




            DataTable dt = (DataTable)Session["Roleexport"];
            //if (_downloadingData != null)
            string attachment = "attachment; filename=TransferOwnerShipReport.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            string tab = "";
            string strquery = "";

            if (dt.Rows.Count > 0)
            {
                foreach (DataColumn dc in dtc.Columns)
                {
                    Response.Write(tab + dc.ColumnName);
                    tab = "\t";
                }
                Response.Write("\n");
                int k;
                foreach (DataRow dr in dt.Rows)
                {
                    tab = "";
                    for (k = 0; k < dt.Columns.Count; k++)
                    {
                        if(k != 0 && k != 4)
                        {
                        Response.Write(tab + dr[k].ToString());
                        tab = "\t";
                        }
                    }
                    Response.Write("\n");
                }
                Response.End();

            }
            return RedirectToAction("Index");



        }

    }
}
