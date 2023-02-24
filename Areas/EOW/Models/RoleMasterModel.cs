using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using IEM.Common;
namespace IEM.Areas.EOW.Models
{
    public class RoleMasterModel:RollRepository
    {
        ErrorLog objErrorLog = new ErrorLog();
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        DataTable data = new DataTable();
        SupClassificationModel sub = new SupClassificationModel();
        string result;
        public RoleMasterModel()
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
        public DataTable selectchildnodevalue(int parent_gid)
        {
            DataTable DT = new DataTable();
            try { 
            cmd = new SqlCommand("Pr_iem_mst_tRole", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PARENT_GID", SqlDbType.VarChar).Value = parent_gid;
            cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTCHILENODEVALUE";
            da = new SqlDataAdapter(cmd);
            da.SelectCommand.CommandTimeout = 0;
         
            da.Fill(DT);
            return DT;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return DT;
            }
        }
        public DataTable FindParenNode()
        {
            try { 
            cmd = new SqlCommand("Pr_iem_mst_tRole", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "FINDPARENTNODE";
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
    }
}