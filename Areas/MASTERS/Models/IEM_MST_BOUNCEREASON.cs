using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using IEM.Areas.MASTERS.Models;
using System.Web.Mvc;
using System.Web.Helpers;
using IEM.Common;


namespace IEM.Areas.MASTERS.Models
{
    public class IEM_MST_BOUNCEREASON : Iiem_mst_bounce
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();

        private void GetCon()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }


        public IEnumerable<iem_mst_bouncereason> getbounce()
        {
            try
            {
                List<iem_mst_bouncereason> objOrgType = new List<iem_mst_bouncereason>();
                iem_mst_bouncereason objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_bouncereason", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_bouncereason();
                    objModel.bouncereason_gid = Convert.ToInt32(dt.Rows[i]["bouncereason_gid"].ToString());
                    objModel.bouncereason_code = Convert.ToString(dt.Rows[i]["bouncereason_code"].ToString());
                    objModel.bouncereason_name = Convert.ToString(dt.Rows[i]["bouncereason_name"].ToString());
                    objModel.bouncereason_type = Convert.ToString(dt.Rows[i]["bouncereason_type"].ToString());
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
            // throw new NotImplementedException();
        }

        public IEnumerable<iem_mst_bouncereason> getbounce(string filter_code,string filter_name)
        {
            try
            {
                List<iem_mst_bouncereason> objOrgType = new List<iem_mst_bouncereason>();
                iem_mst_bouncereason objModel;

                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_bouncereason", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.Parameters.AddWithValue("@bouncereason_code", filter_code);
                cmd.Parameters.AddWithValue("@bouncereason_name", filter_name);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_bouncereason();
                    objModel.bouncereason_gid = Convert.ToInt32(dt.Rows[i]["bouncereason_gid"].ToString());
                    objModel.bouncereason_code = Convert.ToString(dt.Rows[i]["bouncereason_code"].ToString());
                    objModel.bouncereason_name = Convert.ToString(dt.Rows[i]["bouncereason_name"].ToString());
                    objModel.bouncereason_type = Convert.ToString(dt.Rows[i]["bouncereason_type"].ToString());
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

        public iem_mst_bouncereason GetBounceById(int BounceId)
        {
            try
            {
                List<iem_mst_bouncereason> objOrgType = new List<iem_mst_bouncereason>();
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_bouncereason", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", BounceId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new iem_mst_bouncereason()
                {
                    bouncereason_gid = Convert.ToInt32(dt.Rows[0]["bouncereason_gid"].ToString()),
                    bouncereason_name = Convert.ToString(dt.Rows[0]["bouncereason_name"].ToString()),
                    bouncereason_code = Convert.ToString(dt.Rows[0]["bouncereason_code"].ToString()),
                    bouncereason_type = Convert.ToString(dt.Rows[0]["bouncereason_type"].ToString()),
                };

                return model;
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

        public string InsertBounce(iem_mst_bouncereason Bounce)
        {
            string result;

            try
            {
                SqlCommand cmd = new SqlCommand("pr_iem_mst_bouncereason", con);
                GetCon();
                cmd.Parameters.AddWithValue("@action", "InExist");
                cmd.Parameters.AddWithValue("@bouncereason_code", Bounce.bouncereason_code);
                cmd.Parameters.AddWithValue("@bouncereason_type", Bounce.bouncereason_type);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();

                if (res1 == "Not There")
                {
                    CommonIUD comm = new CommonIUD();
                    string[,] codes = new string[,]            
	                {
                        {"bouncereason_code	",Bounce.bouncereason_code},
                        {"bouncereason_name	",Bounce.bouncereason_name},
                        {"bouncereason_type	",Bounce.bouncereason_type},
                        {"bouncereason_insert_by	",Convert.ToString (Bounce.bouncereason_insert_by)},
                        {"bouncereason_insert_date	",comm.GetCurrentDate()}
                    };
                    string insertcommend = comm.InsertCommon(codes, "iem_mst_tbouncereason");
                    result = "success";
                }
                else
                {
                    result = "Duplicate record !";
                }
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

        public void DeleteBounce(int BounceId, int updateperson)
        {

            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {

                string[,] codes = new string[,]
	       {
                 {"bouncereason_isremoved", "Y"}
	            
           };
                string[,] whrcol = new string[,]
	        {
                {"bouncereason_gid", BounceId.ToString ()}
            };
                string tblname = "iem_mst_tbouncereason";


                string deletetbl = delecomm.DeleteCommon(codes, whrcol, tblname);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
            //throw new NotImplementedException();
        }

        public string UpdateBounce(iem_mst_bouncereason Bounce)
        {
            string result;

            try
            {
                SqlCommand cmd = new SqlCommand("pr_iem_mst_bouncereason", con);
                GetCon();
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.AddWithValue("@gid", Bounce.bouncereason_gid);
                cmd.Parameters.AddWithValue("@bouncereason_code", Bounce.bouncereason_code);
                cmd.Parameters.AddWithValue("@bouncereason_type", Bounce.bouncereason_type);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();

                if (res1 == "Not There")
                {
                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes = new string[,]
	                  {
                          {"bouncereason_code", Bounce.bouncereason_code},
                          {"bouncereason_name", Bounce.bouncereason_name},
                          {"bouncereason_type", Bounce.bouncereason_type},
                          {"bouncereason_update_by", Bounce.bouncereason_update_by.ToString ()},
                          {"bouncereason_update_date", delecomm.GetCurrentDate()}
	                  };
                    string[,] whrcol = new string[,]
	                 {
                          {"bouncereason_gid", Bounce.bouncereason_gid.ToString ()},
                          {"bouncereason_isremoved", "N"},
                     };

                    string updcomm = delecomm.UpdateCommon(codes, whrcol, "iem_mst_tbouncereason");
                    result = "success";
                }
                else
                {
                    result = "Duplicate record !";
                }
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
    }
}