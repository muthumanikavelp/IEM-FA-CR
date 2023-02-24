using IEM.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IEM.Areas.ASMS.Models
{
    public class Adhoc_model : IAdhocrepository
    {

        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        ErrorLog objErrorLog = new ErrorLog();
        CmnFunctions com = new CmnFunctions();
        private void GetCon()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }

        public IEnumerable<adhoc_model> getadhoclist()
        {
            try
            {
                List<adhoc_model> objOrgType = new List<adhoc_model>();
                adhoc_model objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("[PR_IEM_MST_ADHOC]", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new adhoc_model();
                    objModel.supplierheader_gid = Convert.ToInt64(dt.Rows[i]["supplierheader_gid"].ToString());
                    objModel.Audit_suppliercode = Convert.ToString(dt.Rows[i]["supplierheader_suppliercode"].ToString());
                    objModel.Audit_suppliername = Convert.ToString(dt.Rows[i]["supplierheader_name"].ToString());
                    objModel.statename = Convert.ToString(dt.Rows[i]["state_name"].ToString());
                    objModel.Audit_suppliergstin = Convert.ToString(dt.Rows[i]["suppliergst_gstin"].ToString());
                    objOrgType.Add(objModel);
                }

                return objOrgType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }


            //  throw new NotImplementedException();
        }


        public IEnumerable<adhoc_model> getadhoclist(string code, string name)
        {
            try
            {
                List<adhoc_model> objOrgType = new List<adhoc_model>();
                adhoc_model objModel;

                GetCon();
                DataTable dt = new DataTable();

                SqlCommand cmd = new SqlCommand("PR_IEM_MST_ADHOC", con);
                cmd.Parameters.AddWithValue("@action", "SELECTFILTER");
                cmd.Parameters.AddWithValue("@suppliercode", code);
                cmd.Parameters.AddWithValue("@suppliername", name);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new adhoc_model();
                    objModel.supplierheader_gid = Convert.ToInt64(dt.Rows[i]["supplierheader_gid"].ToString());
                    objModel.Audit_suppliercode = Convert.ToString(dt.Rows[i]["supplierheader_suppliercode"].ToString());
                    objModel.Audit_suppliername = Convert.ToString(dt.Rows[i]["supplierheader_name"].ToString());
                    objModel.statename = Convert.ToString(dt.Rows[i]["state_name"].ToString());
                    objModel.Audit_suppliergstin = Convert.ToString(dt.Rows[i]["suppliergst_gstin"].ToString());
                    objOrgType.Add(objModel);
                }

                return objOrgType;
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


        public adhoc_model GetAdhocById(int ClassificationId)
        {
            try
            {
                List<adhoc_model> objOrgType = new List<adhoc_model>();
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("PR_IEM_MST_ADHOC", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@supplierheader_gid", ClassificationId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new adhoc_model()
                 {
                     supplierheader_gid = Convert.ToInt64(dt.Rows[0]["supplierheader_gid"].ToString()),
                     Audit_suppliercode = Convert.ToString(dt.Rows[0]["supplierheader_suppliercode"].ToString()),
                     Audit_suppliername = Convert.ToString(dt.Rows[0]["supplierheader_name"].ToString()),
                     statename = Convert.ToString(dt.Rows[0]["state_name"].ToString()),
                     Audit_suppliergstin = Convert.ToString(dt.Rows[0]["suppliergst_gstin"].ToString()),
                     gststatecode = Convert.ToString(dt.Rows[0]["state_gst_code"].ToString()),
                     Audit_stategid = Convert.ToInt64(dt.Rows[0]["state_gid"].ToString())
                 };
                return model;

                //return objOrgType;
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
        public string Deleteadhoc(string id)
        {
            string dresult = string.Empty;
            int res = 0;
            try
            {
                GetCon();
                cmd = new SqlCommand("PR_IEM_MST_ADHOC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@suppliercode", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@user", SqlDbType.Int).Value = com.GetLoginUserGid().ToString();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "delete";
                res = cmd.ExecuteNonQuery();

                if (res == -1)
                    dresult = "success";

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return dresult;
        }

        public string getstategstcode(string ClassificationId)
        {
            try
            {
                DataTable dt = new DataTable();
                string s = string.Empty;
                GetCon();
                cmd = new SqlCommand("PR_IEM_MST_ADHOC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@state_gid", SqlDbType.Int).Value = ClassificationId;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "gstcode";
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                s = Convert.ToString(dt.Rows[0]["state_gst_code"].ToString());
                return s;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            // throw new NotImplementedException();
        }

        public string Updateadhoc(adhoc_model Classifications)
        {
            string result = "";
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("PR_IEM_MST_ADHOC", con);
                cmd.Parameters.AddWithValue("@action", "update");
                cmd.Parameters.AddWithValue("@supplierheader_gid", Classifications.supplierheader_gid);
                cmd.Parameters.AddWithValue("@suppliercode", Classifications.Audit_suppliercode);
                cmd.Parameters.AddWithValue("@suppliername", Classifications.Audit_suppliername);
                cmd.Parameters.AddWithValue("@suppliergstin", Classifications.Audit_suppliergstin);
                cmd.Parameters.AddWithValue("@state_gid", Classifications.Audit_stategid);
                cmd.Parameters.AddWithValue("@user", Classifications.Audit_updateby);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                result="success";
             

               
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                con.Close();
            }

            return result;
        }

        public IEnumerable<adhoc_model> GetAdhochistoryById(int id)
        {
            try
            {
                List<adhoc_model> objOrgType = new List<adhoc_model>();
                adhoc_model objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("[PR_IEM_MST_ADHOC]", con);
                cmd.Parameters.AddWithValue("@action", "selecthistory");
                cmd.Parameters.AddWithValue("@supplierheader_gid", id);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new adhoc_model();
                    objModel.supplierheader_gid = Convert.ToInt64(dt.Rows[i]["supplierheader_gid"].ToString());
                    objModel.Audit_suppliercode = Convert.ToString(dt.Rows[i]["supplierheader_suppliercode"].ToString());
                    objModel.Audit_suppliername = Convert.ToString(dt.Rows[i]["Audit_suppliername"].ToString());
                    objModel.statename = Convert.ToString(dt.Rows[i]["state_name"].ToString());
                    objModel.Audit_suppliergstin = Convert.ToString(dt.Rows[i]["suppliergst_gstin"].ToString());
                    objModel.user = Convert.ToString(dt.Rows[i]["Audit_updateby"].ToString());
                    objModel.audit_type = Convert.ToString(dt.Rows[i]["audit_type"].ToString());
                    objOrgType.Add(objModel);
                }

                return objOrgType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }


            //  throw new NotImplementedException();
        }





    }
}