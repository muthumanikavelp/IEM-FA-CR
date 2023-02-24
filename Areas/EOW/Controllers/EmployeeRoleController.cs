using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using IEM.Areas.EOW.Models;
using System.Reflection;
using IEM.App_Start;
namespace IEM.Areas.EOW.Controllers
{

    [NoDirectAccess]
    public class EmployeeRoleController : Controller
    {
        string Result;
        DataTable dt = new DataTable();
        DataTable data;
        int id,flag=0;
        string name;
        SupEmployeeRole sub = new SupEmployeeRole();
        private EmployeeRole objModel;
        public EmployeeRoleController()
            : this(new EmployeeRoleModel())
        {

        }
        public EmployeeRoleController(EmployeeRole objM)
        {
            objModel = objM;
        }
        public ActionResult Index()
        {
            try
            {
                List<SupEmployeeRole> obj = new List<SupEmployeeRole>();
                SupEmployeeRole emp = new SupEmployeeRole();
                if (Session["EmployeeDetails"] != null)
                {
                    obj = (List<SupEmployeeRole>)Session["EmployeeDetails"];
                }
                else
                {
                    obj = objModel.SelectEmployee().ToList();
                }
                if (obj.Count == 0)
                {
                    ViewBag.alert = "No Records";
                    ViewBag.Message = "No Records";
                }
                return View(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult ClearEmployee()
        {
            Session["EmployeeDetails"] = null;
            return RedirectToAction("Index", "EmployeeRole");
        }
        [HttpPost]
        public ActionResult Index(string filter_code = "", string filter_name = "")
        {
            try
            {
                List<SupEmployeeRole> obj = new List<SupEmployeeRole>();
                SupEmployeeRole sub = new SupEmployeeRole();
                obj = objModel.SelectEmployee(filter_code, filter_name).ToList();
                Session["EmployeeDetails"] = obj;
                if (obj.Count == 0)
                {
                    ViewBag.alert = "No Records";
                    ViewBag.Message = "No Record's Found !";
                }
                @ViewBag.filter_code = filter_code;
                @ViewBag.filter_name = filter_name;
                return View(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult RoleMapping(int id)
        {
            try
            {
                Session["ID"] = id;
                SupEmployeeRole objStudentModel = new SupEmployeeRole();
                dt = new DataTable();
                dt = objModel.SelectedRole(id);

                List<SelectListItem> names = new List<SelectListItem>();
                objStudentModel.lstSelectedRoleGid = new string[dt.Rows.Count];
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objStudentModel.lstSelectedRoleGid[i] = string.IsNullOrEmpty(dt.Rows[i]["roleemployee_role_gid"].ToString()) ? "" : dt.Rows[i]["roleemployee_role_gid"].ToString();
                    }
                }
                data = new DataTable();
                data = objModel.RoleList();
                if (data.Rows.Count > 0)
                {
                    foreach (DataRow row1 in data.Rows)
                    {
                        names.Add(new SelectListItem { Text = row1["role_name"].ToString(), Value = row1["role_gid"].ToString() });
                        objStudentModel.RoleMapping = names;

                    }
                }
                return PartialView(objStudentModel);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //[HttpPost]
        //public ActionResult RoleMapping(string filter_role="")
        //{
        //    List<SupEmployeeRole> obj = new List<SupEmployeeRole>();
        //    SupEmployeeRole sub1;
        //    DataTable dt1 = objModel.SelectEmployeeNamebyRole(Convert.ToInt16(Session["EmployeeId"]),filter_role);
        //    if (dt1.Rows.Count == 0)
        //    {
        //        ViewBag.Message = "No Record's Found !";
        //    }
        //    if (dt1.Rows.Count > 0)
        //    {
        //        foreach (DataRow row in dt1.Rows)
        //        {

        //            sub1 = new SupEmployeeRole();
        //            sub1.Roleid = Convert.ToInt16(row["role_gid"].ToString());
        //            sub1.Role = row["role_name"].ToString();
        //            sub1.RoleGroup = row["rolegroup_name"].ToString();
        //            obj.Add(sub1);
        //        }
        //    }
        //    ViewBag.filter_role = filter_role;
        //    return View(obj);
        //}
        public PartialViewResult RoleEmployeeAdd()
        {
            try
            {
                List<SupEmployeeRole> Rolelist = new List<SupEmployeeRole>();
                {
                    Rolelist = objModel.SelectRole().ToList();
                };
                List<SelectListItem> role = new List<SelectListItem>();
                foreach (var item in Rolelist)
                {
                    role.Add(new SelectListItem
                    {
                        Text = item.Role,
                        Value = item.Roleid.ToString()
                    });
                }
                ViewBag.Rolelist = role;
                return PartialView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //---------------------------------------Coded By Subburaj 11.05.2016----------------------//

        public ActionResult excelexport()
        {

            DataTable dtc = new DataTable();
            dtc.Columns.Add("Employee Code");
            dtc.Columns.Add("Employee Name");



            DataTable dt = (DataTable)Session["Emproleexcel"];
            //if (_downloadingData != null)
            string attachment = "attachment; filename=Employee Role.xls";
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
                        if (k != 0 && k != 4)
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



        //--------------------------------------------End--------------------------------------------//
        public JsonResult RoleEmployeeAdding(SupEmployeeRole sub, string[] uncheckedlist, string[] CheckedList)
        {
            try
            {
                String result = String.Empty;
                sub.EmployeeId = Convert.ToInt32(Session["ID"]);
                //sub.Roleid = sub.Roleid;
                Result = objModel.RoleEmployeeSubmit(sub, uncheckedlist, CheckedList);

                if (Result == "sub")
                {
                    result = "Record Inserted Successfully";
                }
                else if (Result == "duplicate")
                {
                    result = " Duplicate Record";
                }
                else
                {
                    result = Result;
                }
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PartialViewResult Delete(int id)
        {
            try
            {
                Session["id"] = id;
                List<SupEmployeeRole> Rolelist = new List<SupEmployeeRole>();
                {
                    Rolelist = objModel.SelectRole().ToList();
                };
                List<SelectListItem> role = new List<SelectListItem>();
                foreach (var item in Rolelist)
                {
                    bool selected = false;
                    if (item.Roleid.ToString() == id.ToString())
                    {
                        selected = true;
                    }
                    role.Add(new SelectListItem
                    {
                        Text = item.Role,
                        Value = item.Roleid.ToString(),
                        Selected = selected
                    });
                }
                ViewBag.Rolelist = role;
                return PartialView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult RoleEmployeeDelete()
        {
            try
            {
                String result = String.Empty;
                objModel.DeleteRoleEmployee(Convert.ToInt16(Session["id"]));
                result = "1";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
