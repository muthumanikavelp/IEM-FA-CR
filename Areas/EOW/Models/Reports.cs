using IEM.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace IEM.Areas.EOW.Models
{
    public class Reports : ReportRepository
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        DataTable data = new DataTable();
        SupEmployeeRole emp = new SupEmployeeRole();
        CmnFunctions cmnfun = new CmnFunctions();
        CommonIUD comm = new CommonIUD();
        ErrorLog objErrorLog = new ErrorLog();

        System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
        string result;
        public Reports()
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

        public DataSet GetEmpTravelDetails(string Fromdate, string Todate)
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_employee_travelreportbydate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                //cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@pr_FromDate", SqlDbType.DateTime).Value = Fromdate;
                cmd.Parameters.Add("@pr_ToDate", SqlDbType.DateTime).Value = Todate;
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
        public DataSet GetEmpTravelDetails_Download(string Fromdate, string Todate)
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_employee_travelreportbydate_Download", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                //cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@pr_FromDate", SqlDbType.DateTime).Value = Fromdate;
                cmd.Parameters.Add("@pr_ToDate", SqlDbType.DateTime).Value = Todate;
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