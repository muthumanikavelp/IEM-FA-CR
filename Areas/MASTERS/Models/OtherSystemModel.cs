using IEM.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IEM.Areas.MASTERS.Models
{
    public class OtherSystemModel : othersystemintegrationRepository
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        CommonIUD comm = new CommonIUD();
        ErrorLog objErrorLog = new ErrorLog();
        Common.CmnFunctions cmnfun = new Common.CmnFunctions();
        string month;
        string result;
        string[,] codes;
        public OtherSystemModel()
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
        public DataTable BranchLoad()
        {
            dt = new DataTable();
            try
            {
                cmd = new SqlCommand("pr_iem_mst_tbranch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "BRANCHLOAD";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return dt;
            }
            finally
            {
                con.Close();
            }
        }
        public IEnumerable<othersystemdatamodel> SelectFC()
        {
            List<othersystemdatamodel> emp = new List<othersystemdatamodel>();
            try
            {
                othersystemdatamodel objModel;
                cmd = new SqlCommand("pr_iem_mst_tbranch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "FCLOAD";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new othersystemdatamodel();
                    objModel.Fc_Code = row["fc_code"].ToString();
                    objModel.Fc_Name = row["fc_name"].ToString();
                    emp.Add(objModel);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return emp;
        }
        public IEnumerable<othersystemdatamodel> SelectCC()
        {
            List<othersystemdatamodel> emp = new List<othersystemdatamodel>();
            try
            {
                othersystemdatamodel objModel;
                cmd = new SqlCommand("pr_iem_mst_tbranch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "CCLOAD";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new othersystemdatamodel();
                    objModel.Cc_Code = row["cc_code"].ToString();
                    objModel.Cc_Name = row["cc_name"].ToString();
                    emp.Add(objModel);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return emp;
        }

        public IEnumerable<othersystemdatamodel> Product()
        {
            List<othersystemdatamodel> emp = new List<othersystemdatamodel>();
            try
            {
                othersystemdatamodel objModel;
                cmd = new SqlCommand("pr_iem_mst_tbranch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "PRODUCTLOAD";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new othersystemdatamodel();
                    objModel.productCode = row["product_code"].ToString();
                    objModel.ProductName = row["product_name"].ToString();
                    emp.Add(objModel);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return emp;
        }
        public IEnumerable<othersystemdatamodel> GL()
        {
            List<othersystemdatamodel> emp = new List<othersystemdatamodel>();
            try
            {
                othersystemdatamodel objModel;
                cmd = new SqlCommand("pr_iem_mst_tbranch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "GLLOAD";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new othersystemdatamodel();
                    objModel.gl_no = row["gl_no"].ToString();
                    objModel.gl_name = row["gl_name"].ToString();
                    objModel.gl_company_code = row["gl_company_code"].ToString();
                    objModel.gl_business_segment = row["gl_business_segment"].ToString();
                    objModel.gl_location_code = row["gl_location_code"].ToString();
                    objModel.gl_product_code = row["gl_product_code"].ToString();
                    emp.Add(objModel);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return emp;
        }

        public IEnumerable<othersystemdatamodel> Employee()
        {
            List<othersystemdatamodel> emp = new List<othersystemdatamodel>();
            try
            {
                othersystemdatamodel objModel;
                cmd = new SqlCommand("pr_iem_mst_tbranch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "EMPLOYEELOAD";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new othersystemdatamodel();
                    objModel.employee_code = row["employee_code"].ToString();
                    objModel.employee_name = row["employee_name"].ToString();
                    objModel.employee_dob = row["employee_dob"].ToString();
                    objModel.employee_iem_designation = row["employee_iem_designation"].ToString();
                    objModel.employee_grade_code = row["employee_grade_code"].ToString();
                    objModel.employee_dept_name = row["employee_dept_name"].ToString();
                    emp.Add(objModel);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return emp;
        }
    }
}