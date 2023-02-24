using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using IEM.Areas.EOW.Models;
using System.Reflection;
using IEM.Helper;
using IEM.App_Start;
namespace IEM.Areas.EOW.Controllers
{

    [NoDirectAccess]
    public class RoleEmployeeController : Controller
    {
        string Result;
        DataTable dt = new DataTable();
        int id;
        int flag = 0;
        string name;
        SupClassificationModel sub = new SupClassificationModel();
        CentralModel cen = new CentralModel();
        private RoleEmployee objModel;
        proLib plib = new proLib();
        dbLib dblib = new dbLib();

        public RoleEmployeeController()
            : this(new RoleEmployeeModel())
        {

        }
        public RoleEmployeeController(RoleEmployee objM)
        {
            objModel = objM;
        }
        public ActionResult Index()
        {
            try
            {
                List<SupClassificationModel> obj = new List<SupClassificationModel>();
                SupClassificationModel sub = new SupClassificationModel();
                if (Session["RoleDetails"] != null)
                {
                    obj = (List<SupClassificationModel>)Session["RoleDetails"];
                }
                else
                {
                    obj = objModel.SelectRole().ToList();
                }
                if (obj.Count == 0)
                {
                    ViewBag.alert = "No Records";
                    ViewBag.message = "No Records";
                }
                return View(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult Clear()
        {
            Session["RoleDetails"] = null;
            return RedirectToAction("Index", "RoleEmployee");
        }
        [HttpPost]
        public ActionResult Index(string filter_role = "", string filter_group = "")
        {
            try
            {
                List<SupClassificationModel> obj = new List<SupClassificationModel>();
                SupClassificationModel sub = new SupClassificationModel();
                obj = objModel.SelectRole(filter_role, filter_group).ToList();
                Session["RoleDetails"] = obj;
                @ViewBag.filter_role = filter_role;
                @ViewBag.filter_group = filter_group;
                if (obj.Count == 0)
                {
                    ViewBag.alert = "No Records";
                    ViewBag.message = "No Record's Found !";
                }
                return View(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public ActionResult EmployeeMapping(string[] list)
        {
            try
            {
                if (list != null)
                {
                    string arr = list[0].ToString();
                    string[] split = arr.Split(',');
                    id = Convert.ToInt16(split[0]);
                    Session["Roleid"] = id;
                    name = split[1].ToString();
                    Session["RoleName"] = split[2].ToString();
                    Array.Clear(list, 0, list.Length);
                }
                List<SupClassificationModel> obj = new List<SupClassificationModel>();
                SupClassificationModel sub1;
                DataTable dt1 = objModel.SelectRoleName(Convert.ToInt16(Session["Roleid"]));
                if (dt1.Rows.Count > 0)
                {
                    foreach (DataRow row in dt1.Rows)
                    {
                        Session["RoleName"] = row["role_name"].ToString();
                        sub1 = new SupClassificationModel();
                        sub1.Employeeid = Convert.ToInt32(row["employee_gid"].ToString());//convert.toint16 update
                        sub1.EmployeeCode = row["employee_code"].ToString();
                        sub1.EmployeeName = row["employee_name"].ToString();
                        sub1.Department = row["employee_dept_name"].ToString();
                        sub1.Designation = row["employee_iem_designation"].ToString();
                        obj.Add(sub1);
                    }
                }
                else
                {
                    dt = objModel.RoleName(Convert.ToInt16(Session["Roleid"]));
                    if (dt.Rows.Count > 0)
                    {
                        Session["RoleName"] = dt.Rows[0][0].ToString();
                    }
                }
                if (obj.Count == 0)
                {
                    ViewBag.message = "No Records";
                }
                return View(obj);
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
            dtc.Columns.Add("Role");
            dtc.Columns.Add("Role Group");



            DataTable dt = (DataTable)Session["Roleemployeeexcel"];
            //if (_downloadingData != null)
            string attachment = "attachment; filename=RoleEmloyee.xls";
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

        [HttpGet]
        public PartialViewResult EmployeeRole()
        {

            return PartialView();
        }
        public JsonResult EmploeeRoleAdd(SupClassificationModel sub)
        {
            try
            {
                string result = string.Empty;
                sub.Roleid = Convert.ToInt32(Session["Roleid"]); //convert.ToInt16 kavitha
                dt = objModel.SelectEmployeeId(sub);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString() != "INVALID")
                    {
                        sub.Employeeid = Convert.ToInt32(dt.Rows[0]["employee_gid"].ToString());////convert.ToInt16 kavitha
                        Result = objModel.EmployeeRoleSubmit(sub);
                        if (Result == "sub")
                        {
                            result = "Successfully Submited";
                        }

                        else if (Result == "duplicate")
                        {
                            result = "You Can't Add Duplicate Record";
                        }
                        else if (Result == "Invalid Employee Code")
                        {
                            result = "Invalid Employee Code";
                        }
                        else
                        {
                            result = Result;
                        }
                    }
                    else
                    {
                        result = "Invalid Employee Code";
                    }
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult EmployeeRoleDelete(SupClassificationModel sub)
        {
            try
            {
                String result = String.Empty;
                result = objModel.DeleteEmployeeRole(sub.Employeeid, (Convert.ToInt16(Session["Roleid"])));
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        [HttpGet]
        public PartialViewResult EmployeeAddSearch(string listfor)
        {
            try
            {
                List<CentraldataModel> obj = new List<CentraldataModel>();
                CentraldataModel Getvaluesearchvalue = new CentraldataModel();
                if (listfor == "search")
                {
                    if (Session["SearchEmployeedata"] != null)
                    {
                        Getvaluesearchvalue = (CentraldataModel)Session["SerchViewbag"];
                        @ViewBag.EmployeeName = Getvaluesearchvalue.RaiserName;
                        @ViewBag.EmployeeCode = Getvaluesearchvalue.RaiserCode;
                        obj = (List<CentraldataModel>)Session["SearchEmployeedata"];
                    }
                }
                else
                {
                    Session["SearchEmployeedata"] = null;
                    Session["SerchViewbag"] = null;
                    obj = cen.SelectEmployee().ToList();
                }
                return PartialView(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public JsonResult EmployeeSearchrolmap(string RaiserName = "", string RaiserCode = "")
        {
            List<CentraldataModel> recordsearch = new List<CentraldataModel>();
            CentraldataModel emp = new CentraldataModel();
            if (RaiserName != "" || RaiserCode != "")
            {
                @ViewBag.EmployeeName = RaiserName;
                @ViewBag.EmployeeCode = RaiserCode;
                emp.RaiserCode = RaiserCode;
                emp.RaiserName = RaiserName;
                Session["SerchViewbag"] = emp;
                recordsearch = cen.SelectEmployeeSearch(RaiserName, RaiserCode).ToList();
                Session["SearchEmployeedata"] = recordsearch;
            }
            else
            {
                Session["SerchViewbag"] = null;
                Session["SerchViewbag"] = null;
                recordsearch = cen.SelectEmployeeSearch(RaiserName, RaiserCode).ToList();
            }

            return Json(recordsearch, JsonRequestBehavior.AllowGet);
        }

        //Employee Auto Complete
        [HttpPost]
        public JsonResult GetAutoCompleteEmployeeAdd(string txt)
        {
            try { return Json(GetAutoCompleteEmployeeAdd(txt, plib.LoginUserId), JsonRequestBehavior.AllowGet); }
            catch { return null; }
        }

        //Employee Auto Complete
        public List<string> GetAutoCompleteEmployeeAdd(string txt, string _UsrId)
        {
            try
            {
                List<string> result = new List<string>();
                DataSet ds = dblib.GetAutoComplete(txt, "13", _UsrId);
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        result.Add(string.Format("{0}~{1}", dr[0], dr[1]));
                    }
                }
                ds.Dispose();
                return result;
            }
            catch { return null; }
        }
    }
}
