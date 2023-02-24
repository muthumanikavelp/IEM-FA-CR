using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using IEM.Common;
using System.Configuration;
using IEM.Models;

namespace IEM.Areas.MASTERS.Models
{
    public class EmpReleaseDataModel:EmployeeReleaseRepository
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        CmnFunctions objCmnFunctions = new CmnFunctions(); 
        private void GetConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }

        public IEnumerable<EmployeeRelease> GetEmployeeDetails(int LoginUserGid)
        {
 	        try
            {
                List<EmployeeRelease> objReleaseEmployee = new List<EmployeeRelease>(); 
                EmployeeRelease objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_iem_LoginDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = LoginUserGid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getreleaseemployeelist";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new EmployeeRelease();
                    objModel._EmployeeGid = Convert.ToInt32(dt.Rows[i]["employee_gid"].ToString());
                    objModel._EmployeeCode = Convert.ToString(dt.Rows[i]["employee_code"].ToString());
                    objModel._EmployeeName = Convert.ToString(dt.Rows[i]["employee_name"].ToString());
                    objReleaseEmployee.Add(objModel);
                }

                return objReleaseEmployee; 
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

        public void UpdateEmployeeDetails(EmployeeRelease objEmpRelease)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_iem_LoginDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (string.IsNullOrEmpty(objEmpRelease._EmployeeCode))
                {
                    cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Emp.empgidST;
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "releaseemployeegid";
                }
                else
                {
                    cmd.Parameters.Add("@empcode", SqlDbType.VarChar).Value = objEmpRelease._EmployeeCode;
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "releaseemployee";
                }
               
                cmd.ExecuteNonQuery();
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
}
}