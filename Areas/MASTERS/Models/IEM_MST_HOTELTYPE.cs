using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using IEM.Common;

namespace IEM.Areas.MASTERS.Models
{
    public class IEM_MST_HOTELTYPE : Iiem_mst_hotel
    {
        SqlCommand cmd = new SqlCommand();
        SqlConnection con = new SqlConnection();
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
        public IEnumerable<iem_mst_hotel> GetHotel()
        {
            List<iem_mst_hotel> objModel = new List<iem_mst_hotel>();
            try
            {
                
                iem_mst_hotel obj;
                DataTable dt = new DataTable();
                GetCon();
                cmd = new SqlCommand("pr_iem_mst_hoteltype", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow Rows in dt.Rows)
                {
                    obj = new iem_mst_hotel()
                    {
                        hoteltype_gid = Convert.ToInt32(Rows["hoteltype_gid"].ToString()),
                        hoteltype_name = Rows["hoteltype_name"].ToString(),
                    };
                    objModel.Add(obj);
                }
                //return objModel;
            }
               
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return objModel;
           // throw new NotImplementedException();
        }

        public IEnumerable<iem_mst_hotel> GetHotel(string HotelName)
        {
            List<iem_mst_hotel> objModel = new List<iem_mst_hotel>();
            try
            {
                iem_mst_hotel obj;
                DataTable dt = new DataTable();
                GetCon();
                cmd = new SqlCommand("pr_iem_mst_hoteltype", con);
                cmd.Parameters.AddWithValue("@action", "Serach");
                cmd.Parameters.AddWithValue("@hotelttype_name", HotelName);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow Rows in dt.Rows)
                {
                    obj = new iem_mst_hotel()
                    {
                        hoteltype_gid = Convert.ToInt32(Rows["hoteltype_gid"].ToString()),
                        hoteltype_name = Rows["hoteltype_name"].ToString(),
                    };
                    objModel.Add(obj);
                }
                //return objModel;
            }

            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return objModel;
        }

        public iem_mst_hotel GetHotelById(int HotelGid)
        {
            List<iem_mst_hotel> objModel = new List<iem_mst_hotel>();
            var Model = new iem_mst_hotel();
            try
            {
                iem_mst_hotel obj;
                DataTable dt = new DataTable();
                GetCon();
                cmd = new SqlCommand("pr_iem_mst_hoteltype", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@hoteltpe_gid", HotelGid);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);               
                Model = new iem_mst_hotel()
                {
                    hoteltype_gid = Convert.ToInt32(dt.Rows[0]["hoteltype_gid"].ToString()),
                    hoteltype_name = dt.Rows[0]["hoteltype_name"].ToString(),
                };
            }

            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return Model;
        }

        public string InsertHotel(iem_mst_hotel HotelModel)
        {
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_hoteltype", con);
                cmd.Parameters.AddWithValue("@action", "Insertexist");
                cmd.Parameters.AddWithValue("@hotelttype_name", HotelModel.hoteltype_name);               
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["@res"].Value.ToString();
                if (result == "Not There")
                {
                    CommonIUD comm = new CommonIUD();
                    string[,] codes = new string[,]            
	                {
                        {"hoteltype_name",HotelModel.hoteltype_name },                        
                        {"hoteltype_insert_by",cmnf.GetLoginUserGid().ToString ()},
                        {"hoteltype_insert_date",comm.GetCurrentDate()}
                    };
                    string tname = "iem_mst_thoteltype";
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

        public string DeleteHotel(int HotelGid)
        {

            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {

                string[,] codes = new string[,]
	       {
                 {"hoteltype_isremoved", "Y"}
	            
           };
                string[,] whrcol = new string[,]
	        {
                {"hoteltype_gid", HotelGid.ToString ()}
            };
                string tblname = "iem_mst_thoteltype";


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

        public string UpdateHotel(iem_mst_hotel Updatehotelmodel)
        {
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_hoteltype", con);
                cmd.Parameters.AddWithValue("@action", "Updateexist");
                cmd.Parameters.AddWithValue("@hoteltpe_gid", Updatehotelmodel.hoteltype_gid);
                cmd.Parameters.AddWithValue("@hotelttype_name", Updatehotelmodel.hoteltype_name);               
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["@res"].Value.ToString();
                if (result == "Not There")
                {
                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes = new string[,]
	                  {
                         {"hoteltype_name",Updatehotelmodel.hoteltype_name },                        
                        {"hoteltype_update_by",cmnf.GetLoginUserGid().ToString ()},
                        {"hoteltype_update_date",delecomm.GetCurrentDate()}
	                  };
                    string[,] whrcol = new string[,]
	                 {
                          {"hoteltype_gid", Updatehotelmodel.hoteltype_gid.ToString ()},
                          {"hoteltype_isremoved", "N"}
                     };

                    string updcomm = delecomm.UpdateCommon(codes, whrcol, "iem_mst_thoteltype");
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
    }
}