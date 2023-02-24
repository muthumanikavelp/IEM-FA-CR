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
    public class IEM_MST_TIER : Iiem_mst_tier
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

        public IEnumerable<iem_mst_tier> gettier()
        {
            try
            {
                List<iem_mst_tier> objOrgType = new List<iem_mst_tier>();
                iem_mst_tier objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tier", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_tier();
                    objModel.tier_gid = Convert.ToInt32(dt.Rows[i]["tier_gid"].ToString());
                    objModel.tier_code = Convert.ToString(dt.Rows[i]["tier_code"].ToString());
                    objModel.tier_name = Convert.ToString(dt.Rows[i]["tier_name"].ToString());
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
            //throw new NotImplementedException();
        }

        public IEnumerable<iem_mst_tier> gettier(string filter_code, string filter_name)
        {
            try
            {
                List<iem_mst_tier> objOrgType = new List<iem_mst_tier>();
                iem_mst_tier objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tier", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.Parameters.AddWithValue("@tier_code", filter_code);
                cmd.Parameters.AddWithValue("@tier_name", filter_name);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_tier();
                    objModel.tier_gid = Convert.ToInt32(dt.Rows[i]["tier_gid"].ToString());
                    objModel.tier_code = Convert.ToString(dt.Rows[i]["tier_code"].ToString());
                    objModel.tier_name = Convert.ToString(dt.Rows[i]["tier_name"].ToString());
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
            //throw new NotImplementedException();
        }

        public iem_mst_tier GetTierById(int TierId)
        {
            try
            {
                List<iem_mst_tier> objOrgType = new List<iem_mst_tier>();
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tier", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", TierId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new iem_mst_tier()
                {
                    tier_gid = Convert.ToInt32(dt.Rows[0]["tier_gid"].ToString()),
                    tier_name = Convert.ToString(dt.Rows[0]["tier_name"].ToString()),
                    tier_code = Convert.ToString(dt.Rows[0]["tier_code"].ToString()),
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
            // throw new NotImplementedException();
        }

        public string InsertTier(iem_mst_tier Tier)
        {
            string result = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tier", con);
                GetCon();
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.AddWithValue("@tier_code", Tier.tier_code);
                cmd.Parameters.AddWithValue("@tier_name", Tier.tier_name);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                CommonIUD comm = new CommonIUD();
                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]            
	                {
                        {"tier_code",Tier.tier_code },
                        {"tier_name",Tier.tier_name},
                        {"tier_insert_by",Convert.ToString (Tier.tier_insert_by)},
                        {"tier_insert_date",comm.GetCurrentDate()}
                    };


                    string insertcommend = comm.InsertCommon(codes, "iem_mst_ttier");
                    result = "success";
                }
                else
                {
                    result = "Duplicate record !";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            finally
            {
                con.Close();
            }

            return result;
        }



        public string UpdateTier(iem_mst_tier Tier)
        {
            string result = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tier", con);
                GetCon();
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.AddWithValue("@gid", Tier.tier_gid);
                cmd.Parameters.AddWithValue("@tier_code", Tier.tier_code);
                cmd.Parameters.AddWithValue("@tier_name", Tier.tier_name);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                CommonIUD delecomm = new CommonIUD();
                if (res1 == "Not There")
                {

                    string[,] codes = new string[,]
	                {
                        {"tier_code", Tier.tier_code},
                        {"tier_name", Tier.tier_name},
                        {"tier_update_by", Tier.tier_update_by.ToString ()},
                        {"tier_update_date", delecomm.GetCurrentDate()}
	                };

                    string[,] whrcol = new string[,]
	                {
                        {"tier_gid", Tier.tier_gid.ToString ()}
                    };


                    string updcomm = delecomm.UpdateCommon(codes, whrcol, "iem_mst_ttier");

                    result = "success";
                }
                else
                {
                    result = "Duplicate record !";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            finally
            {
                con.Close();
            }

            return result;
        }


        //public string DeleteTier(int TierId)
        //{
        //    CommonIUD delecomm = new CommonIUD();
        //    string col_value = string.Empty;
        //    string col_temp = string.Empty;
        //    try
        //    {

        //        string[,] codes = new string[,]
        //   {
        //         {"tier_isremoved", "Y"}

        //   };
        //        string[,] whrcol = new string[,]
        //    {
        //        {"tier_gid", TierId.ToString ()},
        //        {"tier_isremoved", "N"}
        //    };

        //        string deletetbl = delecomm.DeleteCommon(codes, whrcol, "iem_mst_ttier");
        //        return deletetbl;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    //throw new NotImplementedException();
        //}
        public string DeleteTier(int TierId)
        {
            string result;
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;

            try
            {

                CommonIUD comm1 = new CommonIUD();
                SqlCommand cmd1 = new SqlCommand("pr_iem_mst_tier", con);
                GetCon();
                cmd1.Parameters.AddWithValue("@action", "Delete_city");
                cmd1.Parameters.AddWithValue("@gid", TierId);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd1.ExecuteNonQuery();
                string res2 = cmd1.Parameters["@res"].Value.ToString();
                if (res2 == "Not There")
                {
                    string[,] codes = new string[,]
	                {
                         {"tier_isremoved", "Y"}
	            
                         };
                    string[,] whrcol = new string[,]
	                    {
                          {"tier_gid", TierId.ToString ()}
                        };
                    string tblname = "iem_mst_ttier";


                    string deletetbl = delecomm.DeleteCommon(codes, whrcol, tblname);
                    result = "success";
                }
                else
                {
                    result = "Can Not Delete this, Its Based On Some Other category";
                }

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