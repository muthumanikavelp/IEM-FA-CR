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
    public class IEM_MST_COURIER : Iiem_mst_courier
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
        public IEnumerable<iem_mst_courier> getcourier()
        {
            try
            {
                List<iem_mst_courier> objOrgType = new List<iem_mst_courier>();
                iem_mst_courier objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_courier", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_courier();
                    objModel.courier_gid= Convert.ToInt32(dt.Rows[i]["courier_gid"].ToString());                   
                    objModel.courier_name = Convert.ToString(dt.Rows[i]["courier_name"].ToString());
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

        public IEnumerable<iem_mst_courier> getcourier(string filter_name)
        {
            try
            {
                List<iem_mst_courier> objOrgType = new List<iem_mst_courier>();
                iem_mst_courier objModel;
                
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_courier", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.Parameters.AddWithValue("@courier_name", filter_name);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);

                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_courier();
                    objModel.courier_gid = Convert.ToInt32(dt.Rows[i]["courier_gid"].ToString());
                    objModel.courier_name = Convert.ToString(dt.Rows[i]["courier_name"].ToString());
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

        public iem_mst_courier GetCourierById(int ClassificationId)
        {

            try
            {
                List<iem_mst_courier> objOrgType = new List<iem_mst_courier>();
                iem_mst_bank objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_courier", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", ClassificationId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new iem_mst_courier()
                {
                    courier_gid= Convert.ToInt32(dt.Rows[0]["courier_gid"].ToString()),
                    courier_name = Convert.ToString(dt.Rows[0]["courier_name"].ToString()),
                   
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


            throw new NotImplementedException();
        }

        public string InsertCourier(iem_mst_courier Classifications)
        {
            string result;

            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_courier", con);
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.AddWithValue("@courier_name", Classifications.courier_name);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                result = cmd.Parameters["@res"].Value.ToString();
                CommonIUD comm = new CommonIUD();
                if (result == "Not There")
                {
                    string[,] codes = new string[,]
	                {
                        {"courier_name",Classifications.courier_name},       
                        {"courier_insert_by",Convert.ToString (Classifications.courier_insert_by)},
                        {"courier_insert_date",comm.GetCurrentDate()}	   
                    };

                  
                    string insertcommend = comm.InsertCommon(codes, "iem_mst_tcourier");

                    result = "success";
                }
                else
                {
                    result= "Duplicate record !";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }


            return result;
        }

       

        public string UpdateCourier(iem_mst_courier Classifications)
        {
            string result;

            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_courier", con);
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.AddWithValue("@gid", Classifications.courier_gid);
                cmd.Parameters.AddWithValue("@courier_name", Classifications.courier_name);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                result = cmd.Parameters["@res"].Value.ToString();

                if (result == "Not There")
                {
                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes = new string[,]
	                {
                        {"courier_name", Classifications.courier_name},
                        {"courier_update_by", Classifications.courier_update_by.ToString ()},
                        {"courier_update_date", delecomm.GetCurrentDate()}
	                };
                    string[,] whrcol = new string[,]
	                {
                        {"courier_gid", Classifications.courier_gid.ToString () },
                        {"courier_isremoved", "N" }
                    };

                    string updcomm = delecomm.UpdateCommon(codes, whrcol, "iem_mst_tcourier");
                    result = "success";
                }
                else
                {
                    result = "Duplicate record !";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }

            return result;
        }


        public string DeleteCourier(int ClassificationId)
        {
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {

                string[,] codes = new string[,]
	            {
                    {"courier_isremoved", "Y"}
                };
                string[,] whrcol = new string[,]
	            {
                    {"courier_gid", ClassificationId.ToString ()},
                    {"courier_isremoved", "N"}
                };

                string deletetbl = delecomm.DeleteCommon(codes, whrcol, "iem_mst_tcourier");
                return deletetbl;
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
    }
}