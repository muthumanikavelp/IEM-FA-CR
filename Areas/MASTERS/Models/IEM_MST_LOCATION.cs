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

    public class IEM_MST_LOCATION : Iiem_mst_location
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions cmnf = new CmnFunctions();
        private void GetCon()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }


        public IEnumerable<iem_mst_location> getlocation()
        {
            List<iem_mst_location> obj = new List<iem_mst_location>();
            iem_mst_location obj1;
            GetCon();
            DataTable dt = new DataTable();
            cmd = new SqlCommand("pr_iem_mst_location", con);
            cmd.Parameters.AddWithValue("@action","select");
            cmd.CommandType=CommandType.StoredProcedure;
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                obj1 = new iem_mst_location()
                {
                    location_gid = Convert.ToInt32(row["location_gid"].ToString()),
                    location_code = row["location_code"].ToString(),
                    location_name = row["location_name"].ToString(),
                    location_pincode = Convert.ToInt32(row["location_pincode"].ToString()),
                    location_city = row["city_name"].ToString(),
                    location_tier = row["tier_name"].ToString(),
                };
                obj.Add(obj1);
            }
            return obj;

        }

        public IEnumerable<iem_mst_location> getlocation(string locationcode, string locationname, string ciyname, string tiername)
        {
            List<iem_mst_location> obj = new List<iem_mst_location>();
            iem_mst_location obj1;
            GetCon();
            DataTable dt = new DataTable();
            cmd = new SqlCommand("pr_iem_mst_location", con);
            cmd.Parameters.AddWithValue("@action", "Search");
            cmd.Parameters.AddWithValue("@location_code", locationcode);
            cmd.Parameters.AddWithValue("@location_name", locationname);
            cmd.Parameters.AddWithValue("@city_name", ciyname);
            cmd.Parameters.AddWithValue("@tier_name", tiername);
            cmd.CommandType = CommandType.StoredProcedure;
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                obj1 = new iem_mst_location()
                {
                    location_gid = Convert.ToInt32(row["location_gid"].ToString()),
                    location_code = row["location_code"].ToString(),
                    location_name = row["location_name"].ToString(),
                    location_pincode = Convert.ToInt32(row["location_pincode"].ToString()),
                    location_city = row["city_name"].ToString(),
                    location_tier = row["tier_name"].ToString(),
                };
                obj.Add(obj1);
            }
            return obj;
        }

        public iem_mst_location GetlocationrById(int ClassificationId)
        {
            List<iem_mst_location> obj = new List<iem_mst_location>();
            iem_mst_location obj1;
            GetCon();
            DataTable dt = new DataTable();
            cmd = new SqlCommand("pr_iem_mst_location", con);
            cmd.Parameters.AddWithValue("@action", "selectById");
            cmd.Parameters.AddWithValue("@gid", ClassificationId);
            cmd.CommandType = CommandType.StoredProcedure;
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            var model = new iem_mst_location()
            {
                location_gid = Convert.ToInt32(dt.Rows[0]["location_gid"].ToString()),
                location_code = dt.Rows[0]["location_code"].ToString(),
                location_name = dt.Rows[0]["location_name"].ToString(),
                location_pincode = Convert.ToInt32(dt.Rows[0]["location_pincode"].ToString()),
                location_city = dt.Rows[0]["city_name"].ToString(),
                location_tier = dt.Rows[0]["tier_name"].ToString(),
                location_city_gid = Convert.ToInt32(dt.Rows[0]["city_gid"].ToString()),
                location_tier_gid = Convert.ToInt32(dt.Rows[0]["tier_gid"].ToString()),
            };
            return model;
        }

        public string Insertlocation(iem_mst_location Classifications)
        {
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_location", con);
                cmd.Parameters.AddWithValue("@action", "InsExist");
                cmd.Parameters.AddWithValue("@location_code", Classifications.location_code);
                cmd.Parameters.AddWithValue("@location_name", Classifications.location_name);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["@res"].Value.ToString();
                if (result == "Not There")
                {
                    CommonIUD comm = new CommonIUD();
                    string[,] codes = new string[,]            
	                {
                        {"location_code",Classifications.location_code },
                        {"location_name",Classifications.location_name},
                        {"location_pincode",Convert.ToString (Classifications.location_pincode)},
                        {"location_city_gid",Convert.ToString (Classifications.location_city_gid)},
                        {"location_tier_gid",Convert.ToString (Classifications.location_tier_gid)},
                        {"location_insert_by",cmnf.GetLoginUserGid().ToString ()},
                        {"location_insert_date",comm.GetCurrentDate()}
                    };
                    string tname = "iem_mst_tlocation";
                    string insertcommend = comm.InsertCommon(codes, tname);
                }
                else
                {
                    return "Duplicate record !";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
            finally
            {
                con.Close();
            }

            return "success";
        }

        public string Deletelocation(int ClassificationId)
        {
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {

                string[,] codes = new string[,]
	       {
                 {"location_isremoved", "Y"}
	            
           };
                string[,] whrcol = new string[,]
	        {
                {"location_gid", ClassificationId.ToString ()}
            };
                string tblname = "iem_mst_tlocation";


                string deletetbl = delecomm.DeleteCommon(codes, whrcol, tblname);
                return deletetbl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public string Updatelocation(iem_mst_location Classifications)
        {
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_location", con);
                cmd.Parameters.AddWithValue("@action", "UpdateExist");
                cmd.Parameters.AddWithValue("@gid", Classifications.location_gid);
                cmd.Parameters.AddWithValue("@location_code", Classifications.location_code);
                cmd.Parameters.AddWithValue("@location_name", Classifications.location_name);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                string result = cmd.Parameters["@res"].Value.ToString();

                if (result == "Not There")
                {
                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes = new string[,]
	                  {
                        {"location_code",Classifications.location_code },
                        {"location_name",Classifications.location_name},
                        {"location_pincode",Convert.ToString (Classifications.location_pincode)},
                        {"location_city_gid",Convert.ToString (Classifications.location_city_gid)},
                        {"location_tier_gid",Convert.ToString (Classifications.location_tier_gid)},
                        {"location_update_by",cmnf.GetLoginUserGid().ToString ()},
                        {"location_update_date",delecomm.GetCurrentDate()}
	                  };
                    string[,] whrcol = new string[,]
	                 {
                          {"location_gid", Classifications.location_gid.ToString ()},
                          {"location_isremoved", "N"}
                     };

                    string updcomm = delecomm.UpdateCommon(codes, whrcol, "iem_mst_tlocation");
                }
                else
                {
                    return "Duplicate record !";
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

            return "success";
        }


        public IEnumerable<GetCity> GetCity()
        {
            try
            {
                List<GetCity> objCountrytype = new List<GetCity>();
                GetCity objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_location", con);
                cmd.Parameters.AddWithValue("@action", "SelectCity");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new GetCity();
                    objModel.citygid = Convert.ToInt32(dt.Rows[i]["city_gid"].ToString());
                    objModel.cityname = Convert.ToString(dt.Rows[i]["city_name"].ToString());                    
                    objCountrytype.Add(objModel);
                }
                return objCountrytype;
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

        public IEnumerable<Gettier> Gettier()
        {
            try
            {
                List<Gettier> objCountrytype = new List<Gettier>();
                Gettier objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_location", con);
                cmd.Parameters.AddWithValue("@action", "SelectTier");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new Gettier();
                    objModel.tiergid = Convert.ToInt32(dt.Rows[i]["tier_gid"].ToString());
                    objModel.tiername = Convert.ToString(dt.Rows[i]["tier_name"].ToString());
                    objCountrytype.Add(objModel);
                }
                return objCountrytype;
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