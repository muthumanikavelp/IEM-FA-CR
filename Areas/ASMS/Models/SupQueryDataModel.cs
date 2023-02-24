using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IEM.Areas.ASMS.Models;
using System.Data;
using System.Data.SqlClient;
using IEM.Common;
using System.Configuration;
namespace IEM.Areas.ASMS.Models
{
    public class SupQueryDataModel:SupQueryRepository
    {
        SqlCommand cmd;
        SqlConnection con = new SqlConnection();
        SqlDataAdapter da;
        ErrorLog objErrorLog = new ErrorLog();
        CommonIUD objCommonIUD = new CommonIUD();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        public void getconnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }
        public DataSet  GetSupplierQuery()
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_asms_trn_SupplierQuery", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getsupplierquery";
                da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return ds;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        public DataSet GetSupplierQuerysearch(string CreatedDateFrom, string CreatedDateTo, int deptgid, String supcode,String supname,String suppanno)
        {
            DataSet ds = new DataSet();
            getconnection();
            cmd = new SqlCommand("pr_asms_trn_SupplierQuery", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
            cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getsupplierquerysearch";
            cmd.Parameters.Add("@supplierCreationDateFrom", SqlDbType.VarChar).Value = CreatedDateFrom;
            cmd.Parameters.Add("@supplierCreationDateTo", SqlDbType.VarChar).Value = CreatedDateTo;
            cmd.Parameters.Add("@deptgid", SqlDbType.Int).Value = deptgid;
            cmd.Parameters.Add("@suppliercode", SqlDbType.VarChar).Value = supcode.Trim();
            cmd.Parameters.Add("@suppliername", SqlDbType.VarChar).Value =supname.Trim();
            cmd.Parameters.Add("@supplierPanno", SqlDbType.VarChar).Value = suppanno.Trim();
            da = new SqlDataAdapter(cmd);
            cmd.CommandTimeout = 0;
            da.Fill(ds);
            return ds;
        }
        public DataSet GetSupplierQueryReport(string CreatedDateFrom, string CreatedDateTo, int deptgid, string supname, string suppanno) 
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_asms_trn_SupplierQuery", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getsupplierqueryfull";
                cmd.Parameters.Add("@supplierCreationDateFrom", SqlDbType.VarChar).Value = CreatedDateFrom;
                cmd.Parameters.Add("@supplierCreationDateTo", SqlDbType.VarChar).Value = CreatedDateTo;
                cmd.Parameters.Add("@deptgid", SqlDbType.Int).Value = deptgid;
                cmd.Parameters.Add("@suppliername", SqlDbType.VarChar).Value = supname.Trim();
                cmd.Parameters.Add("@supplierpanno", SqlDbType.VarChar).Value = suppanno.Trim();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return ds;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }

        public DataSet GetDeptList()
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_asms_trn_SupplierQuery", con);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getDeptList"; 
                cmd.CommandType = CommandType.StoredProcedure;         
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return ds;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }



    }
}