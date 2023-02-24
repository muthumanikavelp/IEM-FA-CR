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
    public class IEM_MST_CITYCLASS : Iiem_mst_cityclass
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

        public IEnumerable<iem_mst_cityclass> getcity()
        {
            try
            {
                List<iem_mst_cityclass> objOrgType = new List<iem_mst_cityclass>();
                iem_mst_cityclass objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_cityclass", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_cityclass();
                    objModel.cityclass_gid = Convert.ToInt32(dt.Rows[i]["cityclass_gid"].ToString());
                    objModel.cityclass_code = Convert.ToString(dt.Rows[i]["cityclass_code"].ToString());
                    objModel.cityclass_name = Convert.ToString(dt.Rows[i]["cityclass_name"].ToString());
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

        public IEnumerable<iem_mst_cityclass> getcity(string filter_code, string filter_name)
        {
            try
            {
                List<iem_mst_cityclass> objOrgType = new List<iem_mst_cityclass>();
                iem_mst_cityclass objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_cityclass", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.Parameters.AddWithValue("@cityclass_code", filter_code);
                cmd.Parameters.AddWithValue("@cityclass_name", filter_name);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_cityclass();
                    objModel.cityclass_gid = Convert.ToInt32(dt.Rows[i]["cityclass_gid"].ToString());
                    objModel.cityclass_code = Convert.ToString(dt.Rows[i]["cityclass_code"].ToString());
                    objModel.cityclass_name = Convert.ToString(dt.Rows[i]["cityclass_name"].ToString());
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

        public iem_mst_cityclass GetCityById(int CityId)
        {
            try
            {
                List<iem_mst_cityclass> objOrgType = new List<iem_mst_cityclass>();
                iem_mst_cityclass objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_cityclass", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", CityId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new iem_mst_cityclass()
                {
                    cityclass_gid = Convert.ToInt32(dt.Rows[0]["cityclass_gid"].ToString()),
                    cityclass_name = Convert.ToString(dt.Rows[0]["cityclass_name"].ToString()),
                    cityclass_code = Convert.ToString(dt.Rows[0]["cityclass_code"].ToString()),
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

        public string InsertCity(iem_mst_cityclass Classifications)
        {
            string result = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand("pr_iem_mst_cityclass", con);
                GetCon();
                cmd.Parameters.AddWithValue("@action", "ExistforInsert");
                cmd.Parameters.AddWithValue("@cityclass_code", Classifications.cityclass_code);
                cmd.Parameters.AddWithValue("@cityclass_name", Classifications.cityclass_name);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();

                if (res1 == "Not There")
                {
                    CommonIUD comm = new CommonIUD();

                    string[,] codes = new string[,]            
	                {
                        {"cityclass_code",Classifications.cityclass_code },
                        {"cityclass_name",Classifications.cityclass_name},
                        {"cityclass_insert_by",Convert.ToString (Classifications.cityclass_insert_by)},
                        {"cityclass_insert_date",comm.GetCurrentDate()}
                    };

                    string insertcommend = comm.InsertCommon(codes, "iem_mst_tcityclass");
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

        public string DeleteCity(int CityId, int updateperson)
        {
            string result = "";
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {
                CommonIUD comm1 = new CommonIUD();
                SqlCommand cmd1 = new SqlCommand("pr_iem_mst_cityclass", con);
                GetCon();
                cmd1.Parameters.AddWithValue("@action", "Delete_class");
                cmd1.Parameters.AddWithValue("@gid", CityId);
                cmd1.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.ExecuteNonQuery();
                string res2 = cmd1.Parameters["@res"].Value.ToString();
                if (res2 == "Not There")
                {
                    string[,] codes = new string[,]
	           {
                     {"cityclass_isremoved", "Y"}
               };

                    string[,] whrcol = new string[,]
	            {
                    {"cityclass_gid", CityId.ToString ()},
                    {"cityclass_isremoved", "N"}
                };
                    string deletetbl = delecomm.DeleteCommon(codes, whrcol, "iem_mst_tcityclass");
                    result = "Record deleted successfully !";
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

            // throw new NotImplementedException();
        }

        public string UpdateCity(iem_mst_cityclass Classifications)
        {
            string result = string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand("pr_iem_mst_cityclass", con);
                GetCon();
                cmd.Parameters.AddWithValue("@action", "Exist");
                //cmd.Parameters.Add(new SqlParameter("@action", SqlDbType.VarChar, 50));
                cmd.Parameters.AddWithValue("@gid", Classifications.cityclass_gid);
                cmd.Parameters.AddWithValue("@cityclass_code", Classifications.cityclass_code);
                cmd.Parameters.AddWithValue("@cityclass_name", Classifications.cityclass_name);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();

                if (res1 == "Not There")
                {
                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes = new string[,]
	                {
                        {"cityclass_code", Classifications.cityclass_code},
                        {"cityclass_name", Classifications.cityclass_name},
                        {"cityclass_update_by", Classifications.cityclass_update_by.ToString ()},
                        {"cityclass_update_date", delecomm.GetCurrentDate()}
	                };

                    string[,] whrcol = new string[,]
	                {
                        {"cityclass_gid", Classifications.cityclass_gid.ToString ()},
                        {"cityclass_isremoved", "N"}
                    };

                    string updcomm = delecomm.UpdateCommon(codes, whrcol, "iem_mst_tcityclass");
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