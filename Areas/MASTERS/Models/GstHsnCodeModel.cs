using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using IEM.Areas.MASTERS.Models.IEM;
using System.Web.Mvc;
using System.Web.Helpers;
using IEM.Common;

namespace IEM.Areas.MASTERS.Models
{
    public class GstHsnCodeModel : GstHSNCode
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions com = new CmnFunctions();

        private void GetCon()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }

        public string Inserthsncode(EntityGstHsn Classifications)
        {
            string result;
            // throw new NotImplementedException();
            try
            {
                CommonIUD comm = new CommonIUD();
                SqlCommand cmd = new SqlCommand("GST_MST_HSN_IU", con);
                GetCon();
                cmd.Parameters.AddWithValue("@StatementType", "I");
                cmd.Parameters.AddWithValue("@hsn_gid", "0");
                cmd.Parameters.AddWithValue("@hsn_code", Classifications.hsn_code);
                cmd.Parameters.AddWithValue("@hsn_description", Classifications.hsn_description);
                cmd.Parameters.AddWithValue("@hsn_fromdate", Classifications.hsn_fromdate);
                cmd.Parameters.AddWithValue("@hsn_todate", Classifications.hsn_todate);
                cmd.Parameters.AddWithValue("@hsn_status", Classifications.hsn_status);
                cmd.Parameters.AddWithValue("@hsn_insertby", int.Parse(com.GetLoginUserGid().ToString()));

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                result = "success";
                return result;
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
        public IEnumerable<EntityGstHsn> gethsncode()
        {
            try
            {
                List<EntityGstHsn> objOrgType = new List<EntityGstHsn>();
                EntityGstHsn objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("GST_MST_HSN_SL", con);
                cmd.Parameters.AddWithValue("@StatementType", "SEL");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new EntityGstHsn();
                    objModel.hsn_gid = Convert.ToInt32(dt.Rows[i]["hsn_gid"]);
                    objModel.hsn_code = dt.Rows[i]["hsn_code"].ToString();
                    objModel.hsn_description = dt.Rows[i]["hsn_description"].ToString();
                    objModel.hsn_creationdate = dt.Rows[i]["hsn_creation"].ToString();
                    objModel.hsn_updationdate = dt.Rows[i]["hsn_updation"].ToString();
                    if(!string.IsNullOrEmpty(dt.Rows[i]["hsn_fromdate"].ToString())){
                       objModel.hsn_fromdate = Convert.ToDateTime(dt.Rows[i]["hsn_fromdate"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[i]["hsn_todate"].ToString()))
                    {
                        objModel.hsn_todate = Convert.ToDateTime(dt.Rows[i]["hsn_todate"].ToString());
                    }
                    objModel.hsn_status = dt.Rows[i]["hsn_status"].ToString(); 
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

        public EntityGstHsn gethsncodeid(int hsnId)
        {

            try
            {
                List<EntityGstHsn> objOrgType = new List<EntityGstHsn>();
                EntityGstHsn objModel = new EntityGstHsn();
                GetCon();
                DataTable dt = new DataTable();

                SqlCommand cmd = new SqlCommand("GST_MST_HSN_SL", con);
                cmd.Parameters.AddWithValue("@StatementType", "SID");
                cmd.Parameters.AddWithValue("@hsn_gid", hsnId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel.hsn_gid = Convert.ToInt32(dt.Rows[i]["hsn_gid"]);
                    objModel.hsn_code = dt.Rows[i]["hsn_code"].ToString();
                    objModel.hsn_description = dt.Rows[i]["hsn_description"].ToString();
                    objModel.hsn_creationdate = dt.Rows[i]["hsn_insertdate"].ToString();
                    objModel.hsn_updationdate = dt.Rows[i]["hsn_updatedate"].ToString();
                    if (!string.IsNullOrEmpty(dt.Rows[i]["hsn_fromdate"].ToString()))
                    {
                        objModel.hsn_fromdate = Convert.ToDateTime(dt.Rows[i]["hsn_fromdate"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dt.Rows[i]["hsn_todate"].ToString()))
                    {
                        objModel.hsn_todate = Convert.ToDateTime(dt.Rows[i]["hsn_todate"].ToString());
                    }
                    objModel.hsn_status = dt.Rows[i]["hsn_status"].ToString(); 
                }

                return objModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            // throw new NotImplementedException();
        }

        public IEnumerable<EntityGstHsn> getHsncode_list(string Hsncode)
        {
            //  throw new NotImplementedException();

            try
            {
                List<EntityGstHsn> objOrgType = new List<EntityGstHsn>();
                EntityGstHsn objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("GST_MST_HSN_SL", con);
                cmd.Parameters.AddWithValue("@StatementType", "SCODE");
                cmd.Parameters.AddWithValue("@hsn_code", Hsncode);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new EntityGstHsn();
                    objModel.hsn_gid = Convert.ToInt32(dt.Rows[i]["hsn_gid"]);
                    objModel.hsn_code = dt.Rows[i]["hsn_code"].ToString();
                    objModel.hsn_description = dt.Rows[i]["hsn_description"].ToString();
                    objModel.hsn_creationdate = dt.Rows[i]["hsn_creation"].ToString();
                    objModel.hsn_updationdate = dt.Rows[i]["hsn_updation"].ToString();
                    objModel.hsn_fromdate = Convert.ToDateTime(dt.Rows[i]["hsn_fromdate"]);
                    objModel.hsn_todate = Convert.ToDateTime(dt.Rows[i]["hsn_todate"]); 
                    objModel.hsn_status = dt.Rows[i]["hsn_status"].ToString(); 
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

        public string Updatehsncode(EntityGstHsn Classifications)
        {
            string result;
            // throw new NotImplementedException();
            try
            {
                CommonIUD comm = new CommonIUD();
                SqlCommand cmd = new SqlCommand("GST_MST_HSN_IU", con);
                GetCon();

                cmd.Parameters.AddWithValue("@StatementType", "U");
                cmd.Parameters.AddWithValue("@hsn_gid", Classifications.hsn_gid);
                cmd.Parameters.AddWithValue("@hsn_code", Classifications.hsn_code);
                cmd.Parameters.AddWithValue("@hsn_description", Classifications.hsn_description);
                cmd.Parameters.AddWithValue("@hsn_addition", Classifications.hsn_addition);
                cmd.Parameters.AddWithValue("@hsn_status", Classifications.hsn_status);
                cmd.Parameters.AddWithValue("@hsn_fromdate", Classifications.hsn_fromdate);
                cmd.Parameters.AddWithValue("@hsn_todate", Classifications.hsn_todate);
                cmd.Parameters.AddWithValue("@hsn_updateby", int.Parse(com.GetLoginUserGid().ToString()));
                cmd.Parameters.AddWithValue("@hsn_isremoved", "N");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                result = "success";
                return result;
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

        public string Deletehsncode(int PinId)
        {
            string result;
            // throw new NotImplementedException();
            try
            {
                CommonIUD comm = new CommonIUD();
                SqlCommand cmd = new SqlCommand("GST_MST_HSN_IU", con);
                GetCon();
                cmd.Parameters.AddWithValue("@StatementType", "D"); 
                cmd.Parameters.AddWithValue("@hsn_gid", PinId);
                cmd.Parameters.AddWithValue("@hsn_updateby", int.Parse(com.GetLoginUserGid().ToString()));
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                result = "success";
                return result;
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