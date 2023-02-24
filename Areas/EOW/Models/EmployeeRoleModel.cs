using IEM.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IEM.Areas.EOW.Models
{
    public class EmployeeRoleModel:EmployeeRole
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        DataTable data = new DataTable();
        SupEmployeeRole emp = new SupEmployeeRole();
        CmnFunctions cmnfun = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        string result;
        public EmployeeRoleModel()
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


        public IEnumerable<SupEmployeeRole> SelectEmployee()
        {
            List<SupEmployeeRole> emp = new List<SupEmployeeRole>();
            try
            {
               
                SupEmployeeRole objModel;
                cmd = new SqlCommand("pr_iem_mst_temployeerole", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTEMPLOYEE";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                HttpContext.Current.Session["Emproleexcel"] = dt;
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new SupEmployeeRole();
                    objModel.EmployeeId = Convert.ToInt32(row["employee_gid"].ToString());
                    objModel.EmployeeCode = row["employee_code"].ToString();
                    objModel.EmployeeName = row["employee_name"].ToString();
                    emp.Add(objModel);
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            } 
        }
        public IEnumerable<SupEmployeeRole> SelectEmployee(string code, string name)
        {
            List<SupEmployeeRole> sub = new List<SupEmployeeRole>();
            try
            {
                
                SupEmployeeRole objModel;
                cmd = new SqlCommand("pr_iem_mst_temployeerole", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EMPLOYEECODE", SqlDbType.VarChar).Value = code;
                cmd.Parameters.Add("@EMPLOYEENAME", SqlDbType.VarChar).Value = name;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTEMPLOYEE";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                HttpContext.Current.Session["Emproleexcel"] = dt;
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new SupEmployeeRole();
                    objModel.EmployeeId = Convert.ToInt32(row["employee_gid"].ToString());
                    objModel.EmployeeCode = row["employee_code"].ToString();
                    objModel.EmployeeName = row["employee_name"].ToString();
                    sub.Add(objModel);
                }
                return sub;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return sub;
            }
            finally
            {
                con.Close();
            } 
        }
        public IEnumerable<SupEmployeeRole> SelectRole()
        {
            List<SupEmployeeRole> sub = new List<SupEmployeeRole>();
            try
            {
               
                SupEmployeeRole objModel;
                cmd = new SqlCommand("pr_iem_mst_temployeerole", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTROLE";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new SupEmployeeRole();
                    objModel.Roleid = Convert.ToInt32(row["role_gid"].ToString());
                    objModel.Role = row["role_name"].ToString();
                    
                    sub.Add(objModel);
                }
                return sub;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return sub;
            }
            finally
            {
                con.Close();
            } 
        }
        public DataTable SelectEmployeeName(int id)
        {
            try
            {
                cmd = new SqlCommand("pr_iem_mst_temployeerole", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ROLEEMPLOYEEGID", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@ROLE", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTEMPLOYEENAME";
                da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 0;
                dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return dt;
            }
        }

        public DataTable SelectEmployeeNamebyRole(int id,string role)
        {
            try
            {
                cmd = new SqlCommand("pr_iem_mst_temployeerole", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ROLEEMPLOYEEGID", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@ROLE", SqlDbType.VarChar).Value = role;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTEMPLOYEENAME";
                da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 0;
                dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return dt;
            }
        }
        public IEnumerable<SupEmployeeRole> SelectRoleDetails(int rolegid)
        {
            List<SupEmployeeRole> sub = new List<SupEmployeeRole>();
            try
            {
             
                SupEmployeeRole objModel;
                cmd = new SqlCommand("pr_iem_mst_temployeerole", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ROLEGID", SqlDbType.VarChar).Value = rolegid;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTROLLDETAILS";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new SupEmployeeRole();
                    objModel.Roleid = Convert.ToInt32(row["role_gid"].ToString());
                    objModel.Role = row["role_name"].ToString();
                    objModel.RoleGroup = row["rolegroup_name"].ToString();
                    sub.Add(objModel);
                }
                return sub;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return sub;
            }
            finally
            {
                con.Close();
            } 
        }
        public DataTable SelectRoleById(int id)
        {
            try
            {
                cmd = new SqlCommand("pr_iem_mst_temployeerole", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ROLEGID", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTROLEBYID";
                da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 0;
                dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return dt;
            }
        }
        public string RoleEmployeeSubmit(SupEmployeeRole sub,string[] uncheck,string[] check)
        {
            try
            {
                foreach (string selctedstate in uncheck)
                {
                    SqlCommand cmd = new SqlCommand("pr_iem_mst_temployeerole", con);
                    cmd.Parameters.AddWithValue("@ACTION", "UPDATEROLE");
                    cmd.Parameters.AddWithValue("@ROLEEMPLOYEEGID", sub.EmployeeId);
                    cmd.Parameters.AddWithValue("@UNCHECKEDROLE", selctedstate);
                    cmd.CommandType = CommandType.StoredProcedure;
                    result = (string)cmd.ExecuteScalar();
                }
                foreach (string checkid in check)
                {
                    SqlCommand cmd = new SqlCommand("pr_iem_mst_temployeerole", con);
                    cmd.Parameters.AddWithValue("@ACTION",SqlDbType.VarChar).Value= "UPDATE";
                    cmd.Parameters.AddWithValue("@ROLEEMPLOYEEGID",SqlDbType.Int).Value= sub.EmployeeId;
                    cmd.Parameters.AddWithValue("@INSERTBY",SqlDbType.Int).Value= cmnfun.GetLoginUserGid();
                    cmd.Parameters.AddWithValue("@ROLEGID",SqlDbType.Int).Value= checkid;
                    cmd.CommandType = CommandType.StoredProcedure;
                    result = (string)cmd.ExecuteScalar();
                }
               
               
                if (result == "sub" || result==null)
                {
                    result = "sub";
                }
                if(result=="ins")
                {
                    result = "sub";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
            }

            return result;
        }
        public string DeleteRoleEmployee(int id)
        {
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {

                string[,] codes = new string[,]
	       {
                 {"roleemployee_isremoved", "Y"},
                 {"roleemployee_update_by",cmnfun.GetLoginUserGid().ToString()},
                 {"roleemployee_update_date", "sysdatetime()"}
	            
           };
                string[,] whrcol = new string[,]
	        {
                {"roleemployee_role_gid",id.ToString ()}
            };
                string tblname = "iem_mst_troleemployee";
                result = delecomm.DeleteCommon(codes, whrcol, tblname);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
            }
            return result;
        }
        public DataTable RoleList()
        {
            try
            {
                cmd = new SqlCommand("pr_iem_mst_temployeerole", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTROLE";
                da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 0;
                dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return dt;
            }
        }

        public DataTable SelectedRole(int id)
        {
            try
            {
                cmd = new SqlCommand("pr_iem_mst_temployeerole", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ROLEEMPLOYEEGID", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTEDROLEID";
                da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 0;
                dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return dt;
            }
        }
    }
}