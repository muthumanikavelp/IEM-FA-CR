using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;


namespace IEM.Areas.MASTERS.Models
{

    public class MyProfileModel : MyprofileRepository
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        Common.CmnFunctions cmnfun = new Common.CmnFunctions();
        string month;
        string result;
        public MyProfileModel()
        {
            GetConnection();
        }
        private void GetConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }
        public IEnumerable<MyProfileDataModel> SelectEmployeeDetails()
        {
            try
            {
                List<MyProfileDataModel> emp = new List<MyProfileDataModel>();
                MyProfileDataModel objModel;
                cmd = new SqlCommand("pr_iem_mst_temployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EMPLOYEEID", SqlDbType.Int).Value = cmnfun.GetLoginUserGid();
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTEMPLOYEE";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new MyProfileDataModel();

                    objModel.employee_ou_name = row["ou_name"].ToString();
                    objModel.employee_product_name = row["product_name"].ToString();
                    objModel.employee_cc_name = row["cc_name"].ToString();
                    objModel.employee_fc_name = row["fc_name"].ToString();
                    objModel.employee_gid = row["employee_gid"].ToString();
                    objModel.employee_code = row["employee_code"].ToString();
                    objModel.employee_name = row["employee_name"].ToString();
                    objModel.employee_gender = row["employee_gender"].ToString();
                    objModel.employee_status = row["employee_status"].ToString();
                    objModel.employee_dob = row["employee_dob"].ToString();
                    objModel.employee_doj = row["employee_doj"].ToString();
                    objModel.employee_addr1 = row["employee_addr1"].ToString();
                    objModel.employee_city_name = row["employee_city_name"].ToString();
                    objModel.employee_personal_email = row["employee_personal_email"].ToString();
                    objModel.employee_office_email = row["employee_office_email"].ToString();
                    objModel.employee_card_no = row["employee_card_no"].ToString();
                    objModel.employee_iem_designation = row["employee_iem_designation"].ToString();
                    objModel.employee_grade_code = row["employee_grade_code"].ToString();
                    objModel.employee_dept_name = row["employee_dept_name"].ToString();
                    objModel.employee_fccc_code = row["employee_fccc_code"].ToString();
                    objModel.employee_supervisor = row["Supervisor"].ToString();
                    objModel.employee_era_acc_no = row["employee_era_acc_no"].ToString();
                    objModel.employee_era_ifsc_code = row["employee_era_ifsc_code"].ToString();
                    objModel.employee_era_bank_code = row["employee_era_bank_code"].ToString();
                    objModel.hris_designation = row["employee_hris_designation"].ToString();
                    objModel.branch_flag = row["branch_flag"].ToString();

                    
                    objModel.employee_function_name = row["employee_functionname"].ToString();
                    emp.Add(objModel);
                }
                return emp;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        public IEnumerable<MyProfileDataModel> GetMenu()
        {
            try
            {
                IList<MyProfileDataModel> mmList = new List<MyProfileDataModel>();
                MyProfileDataModel objModel;
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_iem_mst_temployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTTREEVIEW";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new MyProfileDataModel();
                    objModel.Id = Convert.ToInt32(dt.Rows[i]["employee_gid"].ToString());
                    objModel.Name = dt.Rows[i]["employee_name"].ToString();
                    //objModel.url = dt.Rows[i]["menu_link"].ToString();
                    objModel.ParentId = Convert.ToInt32(dt.Rows[i]["employee_supervisor"].ToString());
                    //objModel.SortOrder = Convert.ToInt32(dt.Rows[i]["menu_displayorder"].ToString());
                    mmList.Add(objModel);
                }

                return mmList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public MyProfileDataModel menu(int id)
        {
            try
            {
                List<menu> menu = new List<menu>();
                List<menu> all = new List<menu>();
                MyProfileDataModel obj = new MyProfileDataModel();
                obj.menu = new List<menu>();
                //obj.RoleList = new List<MyProfileDataModel>();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_iem_mst_temployee", con);
                cmd.Parameters.AddWithValue("@EMPLOYEEID", SqlDbType.Int).Value = cmnfun.GetLoginUserGid();
                cmd.Parameters.AddWithValue("@ACTION", "SELECTTREEVIEW");
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                menu mn;
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    mn = new menu();

                    mn.menu_gid = Convert.ToInt32(dt.Rows[i]["employee_gid"].ToString());

                    mn.menu_name = dt.Rows[i]["employee_name"].ToString();

                    mn.menu_parent_gid = Convert.ToInt32(dt.Rows[i]["employee_supervisor"].ToString());

                    //mn.menu_link = dt.Rows[i]["menu_link"].ToString();
                    obj.menu.Add(mn);
                }
                all = obj.menu.OrderBy(a => a.menu_parent_gid).ToList();

                //all = obj.OrderBy(a => a.menu_parent_gid).ToList();
                //obj.RoleList = getrole().ToList();

                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<MyProfileDataModel> GetHierarchy()
        {
            try
            {
                List<MyProfileDataModel> lst = new List<MyProfileDataModel>();
                MyProfileDataModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_iem_GetHierarchy", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = cmnfun.GetLoginUserGid();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "gethierarchy";
                da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 0;
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new MyProfileDataModel();
                    objModel.employee_code = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["employee"].ToString()) ? "" : dt.Rows[i]["employee"].ToString());
                    // objModel.employee_name = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["empname"].ToString()) ? "" : dt.Rows[i]["empname"].ToString());
                    // objModel.employee_status = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["Category"].ToString()) ? "" : dt.Rows[i]["Category"].ToString());
                    objModel.Id = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["emplevel"].ToString()) ? "0" : dt.Rows[i]["emplevel"].ToString());
                    objModel.ParentId = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["parent"].ToString()) ? "0" : dt.Rows[i]["parent"].ToString());
                    lst.Add(objModel);
                }

                return lst;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
    }
}