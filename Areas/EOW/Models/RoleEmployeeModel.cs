using IEM.Areas.EOW.Models;
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
    public class RoleEmployeeModel : RoleEmployee
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        DataTable data = new DataTable();
        SupClassificationModel sub = new SupClassificationModel();
        CmnFunctions cmnfun = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        string result;
        public RoleEmployeeModel()
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

        public IEnumerable<SupClassificationModel> SelectRole(string role, string rolegroup)
        {
            List<SupClassificationModel> sub = new List<SupClassificationModel>();
            try
            {

                SupClassificationModel objModel;
                cmd = new SqlCommand("pr_iem_mst_troleemployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ROLE", SqlDbType.VarChar).Value = role;
                cmd.Parameters.Add("@ROLEGROUP", SqlDbType.VarChar).Value = rolegroup;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTROLE";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                HttpContext.Current.Session["Roleemployeeexcel"] = dt;
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new SupClassificationModel();
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
        public IEnumerable<SupClassificationModel> SelectRole()
        {
            List<SupClassificationModel> sub = new List<SupClassificationModel>();
            try
            {

                SupClassificationModel objModel;
                cmd = new SqlCommand("pr_iem_mst_troleemployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTROLE";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                HttpContext.Current.Session["Roleemployeeexcel"] = dt;
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new SupClassificationModel();
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
        public DataTable SelectRoleName(int id)
        {
            try
            {
                SupClassificationModel obj1 = new SupClassificationModel();
                cmd = new SqlCommand("pr_iem_mst_troleemployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ROLEGID", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTROLENAME";
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
        public DataTable RoleName(int id)
        {
            try
            {
                SupClassificationModel obj1 = new SupClassificationModel();
                cmd = new SqlCommand("pr_iem_mst_troleemployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ROLEGID", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "ROLENAME";
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
        public DataTable SelectRoleNamebyempname(int id, string empcode)
        {
            try
            {
                SupClassificationModel obj1 = new SupClassificationModel();
                cmd = new SqlCommand("pr_iem_mst_troleemployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ROLEGID", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@EMPLOYEECODE", SqlDbType.VarChar).Value = empcode;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTROLENAME";
                da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 0;
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return dt;
            }
        }
        public IEnumerable<SupClassificationModel> SelectEmployeeDetails(int code)
        {
            List<SupClassificationModel> sub = new List<SupClassificationModel>();
            try
            {

                SupClassificationModel objModel;
                cmd = new SqlCommand("pr_iem_mst_troleemployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EMPLOYEEGID", SqlDbType.VarChar).Value = code;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTEMPLOYEEDETAILS";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                HttpContext.Current.Session["Emproleexcel"] = dt;
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString() != "NOTEXISTS")
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            objModel = new SupClassificationModel();
                            objModel.Employeeid = Convert.ToInt32(row["employee_gid"].ToString());
                            objModel.EmployeeCode = row["employee_code"].ToString();
                            objModel.EmployeeName = row["employee_name"].ToString();
                            objModel.Department = row["employee_dept_name"].ToString();
                            objModel.Designation = row["employee_iem_designation"].ToString();
                            sub.Add(objModel);
                        }
                    }
                    else
                    {
                        objModel = new SupClassificationModel();
                        objModel.Error = "NOTEXISTS";
                        sub.Add(objModel);

                    }
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
        public IEnumerable<SupClassificationModel> SelectEditEmployeeDetails(string code)
        {
            List<SupClassificationModel> sub = new List<SupClassificationModel>();
            try
            {

                SupClassificationModel objModel;
                cmd = new SqlCommand("pr_iem_mst_troleemployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EMPLOYEECODE", SqlDbType.VarChar).Value = code;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTEMPLOYEEDETAILS";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0].ToString() != "NOTEXISTS")
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            objModel = new SupClassificationModel();
                            objModel.Employeeid = Convert.ToInt32(row["employee_gid"].ToString());
                            objModel.EmployeeCode = row["employee_code"].ToString();
                            objModel.EmployeeName = row["employee_name"].ToString();
                            objModel.Department = row["employee_dept_name"].ToString();
                            objModel.Designation = row["employee_iem_designation"].ToString();
                            sub.Add(objModel);
                        }
                    }
                    else
                    {
                        objModel = new SupClassificationModel();
                        objModel.Error = "NOTEXISTS";
                        sub.Add(objModel);
                    }

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
        public string EmployeeRoleSubmit(SupClassificationModel sub)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("pr_iem_mst_troleemployee", con);
                cmd.Parameters.AddWithValue("@action", "IfduplicateCheck");
                cmd.Parameters.AddWithValue("@EMPLOYEECODE", sub.EmployeeCode);
                cmd.Parameters.AddWithValue("@ROLEGID", sub.Roleid);
                cmd.CommandType = CommandType.StoredProcedure;
                result = (string)cmd.ExecuteScalar();
                if (result == "Not There")
                {
                    if (sub.Employeeid != cmnfun.GetLoginUserGid())
                    {
                        CommonIUD comm = new CommonIUD();
                        string[,] codes = new string[,]            
	                {
                         {"roleemployee_role_gid",sub.Roleid.ToString()},
                         {"roleemployee_employee_gid",sub.Employeeid.ToString()},
                        {"roleemployee_insert_by",Convert.ToString (cmnfun.GetLoginUserGid())},
                        {"roleemployee_insert_date","sysdatetime()"}
                    };
                        string tname = "iem_mst_troleemployee";
                        result = comm.InsertCommon(codes, tname);
                        if (result == "success")
                        {
                            result = "sub";
                        }
                    }
                    else
                    {
                        result= "Cannot map your Own Role Rights!!!";
                    }
                }
                else if (result == "Invalid Employee Code")
                {
                    result= "Invalid Employee Code";
                }
                else
                {
                    result = "duplicate";
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
        public string DeleteEmployeeRole(int id, int roleid)
        {
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {

                string[,] codes = new string[,]
	       {
                 {"roleemployee_isremoved", "Y"},
	            
           };
                string[,] codes1 = new string[,]
	       {
                 {"roleemployee_update_by",cmnfun.GetLoginUserGid().ToString()},
                 {"roleemployee_update_date","sysdatetime()"}
	            
           };

                string[,] whrcol = new string[,]
	        {
                {"roleemployee_employee_gid",id.ToString ()},
                {"roleemployee_role_gid",roleid.ToString ()}
            };
                string tblname = "iem_mst_troleemployee";
                result = delecomm.UpdateCommon(codes1, whrcol, tblname);
                result = delecomm.DeleteCommon(codes, whrcol, tblname);
                if (result == "Sucess")
                {
                    result = "1";
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
        public DataTable SelectEmployeeId(SupClassificationModel sub)
        {
            try
            {
                SupClassificationModel obj1 = new SupClassificationModel();
                cmd = new SqlCommand("pr_iem_mst_troleemployee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EMPLOYEECODE", sub.EmployeeCode);
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTEMPLOYEID";
                da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 0;
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return dt;
            }
        }
        public IEnumerable<CentraldataModel> SelectEmployee()
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {

                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_PouchInward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTEMPLOYEE";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.EmployeeId = Convert.ToInt32(row["employee_gid"].ToString());
                    objModel.RaiserName = row["employee_name"].ToString();
                    objModel.RaiserCode = row["employee_code"].ToString();
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
        public IEnumerable<CentraldataModel> SelectEmployee(string raisername, string raisercode)
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {

                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_PouchInward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RAISERNAME", SqlDbType.VarChar).Value = raisername;
                cmd.Parameters.Add("@RAISERCODE", SqlDbType.VarChar).Value = raisercode;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTEMPLOYEE";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.EmployeeId = Convert.ToInt32(row["employee_gid"].ToString());
                    objModel.RaiserName = row["employee_name"].ToString();
                    objModel.RaiserCode = row["employee_code"].ToString();
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

    }
}