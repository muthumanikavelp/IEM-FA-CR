using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using IEM.Areas.MASTERS.Models;
using System.Web.Mvc;
using System.Web.Helpers;
using System;
using IEM.Common;
namespace IEM.Areas.MASTERS.Models
{
    public class IEM_MST_REGION : Iiem_mst_region
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

        public IEnumerable<iem_mst_region> getregion()
        {

            try
            {
                List<iem_mst_region> objOrgType = new List<iem_mst_region>();
                iem_mst_region objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_region", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_region();
                    objModel.region_gid = Convert.ToInt32(dt.Rows[i]["region_gid"].ToString());
                    objModel.region_name = Convert.ToString(dt.Rows[i]["region_name"].ToString());
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

        public IEnumerable<iem_mst_region> getregion(string filter_name)
        {

            try
            {
                List<iem_mst_region> objOrgType = new List<iem_mst_region>();
                iem_mst_region objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_region", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.Parameters.AddWithValue("@region_name", filter_name);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_region();
                    objModel.region_gid = Convert.ToInt32(dt.Rows[i]["region_gid"].ToString());
                    objModel.region_name = Convert.ToString(dt.Rows[i]["region_name"].ToString());
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

        public iem_mst_region GetRegionById(int RegionId)
        {
            try
            {
                List<iem_mst_region> objOrgType = new List<iem_mst_region>();
                iem_mst_region objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_region", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", RegionId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new iem_mst_region()
                {
                    region_gid = Convert.ToInt32(dt.Rows[0]["region_gid"].ToString()),
                    region_name = Convert.ToString(dt.Rows[0]["region_name"].ToString()),

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

        public string InsertRegion(iem_mst_region Region)
        {
            string result = string.Empty;
            try
            {
                GetCon();

                SqlCommand cmd = new SqlCommand("pr_iem_mst_region", con);
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.AddWithValue("@region_name", Region.region_name);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();

                if (res1 == "Not There")
                {
                    CommonIUD comm = new CommonIUD();

                    string[,] codes = new string[,]            
	                {
                        {"region_name",Region.region_name},
                        {"region_insert_by",Convert.ToString (Region.region_insert_by)},
                        {"region_insert_date",comm.GetCurrentDate()}
                    };

                    string insertcommend = comm.InsertCommon(codes, "iem_mst_tregion");
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

        //public void DeleteRegion(int RegionId, int updateperson)
        //{
        //    //throw new NotImplementedException();
        //    CommonIUD delecomm = new CommonIUD();
        //    string col_value = string.Empty;
        //    string col_temp = string.Empty;

        //    try
        //    {
        //        string[,] codes = new string[,]
        //        {
        //            {"region_isremoved", "Y"}
        //        };

        //        string[,] whrcol = new string[,]
        //        {
        //            {"region_gid", RegionId.ToString ()},
        //            {"region_isremoved", "N"}
        //        };
        //        string deletetbl = delecomm.DeleteCommon(codes, whrcol, "iem_mst_tregion");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public string DeleteRegion(int RegionId)
        {
            string result;
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;

            try
            {

                CommonIUD comm1 = new CommonIUD();
                SqlCommand cmd1 = new SqlCommand("pr_iem_mst_region", con);
                GetCon();
                cmd1.Parameters.AddWithValue("@action", "Delete_state");
                cmd1.Parameters.AddWithValue("@gid", RegionId);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd1.ExecuteNonQuery();
                string res = cmd1.Parameters["@res"].Value.ToString();
                if (res == "Not There")
                {
                    string[,] codes = new string[,]
                    {
                         {"region_isremoved", "Y"}
	            
                         };
                    string[,] whrcol = new string[,]
                        {
                          {"region_gid", RegionId.ToString ()},
                           //{"region_isremoved", "N"}
                        };
                    string tblname = "iem_mst_tregion";


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
        public string UpdateRegion(iem_mst_region Region)
        {
            string result = string.Empty;

            try
            {
                GetCon();

                SqlCommand cmd = new SqlCommand("pr_iem_mst_region", con);
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.AddWithValue("@gid", Region.region_gid);
                cmd.Parameters.AddWithValue("@region_name", Region.region_name);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();

                if (res1 == "Not There")
                {
                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes = new string[,]
	                {
                        {"region_name", Region.region_name},
                        {"region_update_by", Region.region_update_by.ToString ()},
                        {"region_update_date",delecomm.GetCurrentDate()}
	                };
                    string[,] whrcol = new string[,]
	                {
                        {"region_gid", Region.region_gid.ToString ()}
                    };

                    string updcomm = delecomm.UpdateCommon(codes, whrcol, "iem_mst_tregion");
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
    }
}