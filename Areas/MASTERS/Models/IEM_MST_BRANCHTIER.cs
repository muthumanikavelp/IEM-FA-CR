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
    public class IEM_MST_BRANCHTIER :Iiem_mst_branchtier
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions cmnfun=new CmnFunctions ();
        private void GetCon()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }
        public IEnumerable<iem_mst_branchtier> getbranchtier()
        {
            try
            {
                List<iem_mst_branchtier> objOrgType = new List<iem_mst_branchtier>();
                iem_mst_branchtier objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_branchtier", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_branchtier();
                    objModel.branchtier_gid = Convert.ToInt32(dt.Rows[i]["branchtier_gid"].ToString());
                    objModel.branchtier_name = Convert.ToString(dt.Rows[i]["branchtier_name"].ToString());
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

        public IEnumerable<iem_mst_branchtier> getbranchtier(string filter_name)
        {
            try
            {
                List<iem_mst_branchtier> objOrgType = new List<iem_mst_branchtier>();
                iem_mst_branchtier objModel;

                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_branchtier", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.Parameters.AddWithValue("@branchtier_name", filter_name);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);

                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_branchtier();
                    objModel.branchtier_gid = Convert.ToInt32(dt.Rows[i]["branchtier_gid"].ToString());
                    objModel.branchtier_name = Convert.ToString(dt.Rows[i]["branchtier_name"].ToString());
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

        public iem_mst_branchtier GetbranchtierById(int ClassificationId)
        {
            try
            {
                List<iem_mst_branchtier> objOrgType = new List<iem_mst_branchtier>();
                iem_mst_branchtier objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_branchtier", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", ClassificationId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new iem_mst_branchtier()
                {
                    branchtier_gid = Convert.ToInt32(dt.Rows[0]["branchtier_gid"].ToString()),
                    branchtier_name = Convert.ToString(dt.Rows[0]["branchtier_name"].ToString()),

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

        public string Insertbranchtier(iem_mst_branchtier Classifications)
        {
          string result;

            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_branchtier", con);
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.AddWithValue("@branchtier_name", Classifications.branchtier_name);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                result = cmd.Parameters["@res"].Value.ToString();
                CommonIUD comm = new CommonIUD();
                if (result == "Not There")
                {
                    string[,] codes = new string[,]
	                {
                        {"branchtier_name",Classifications.branchtier_name},       
                        {"branchtier_insert_by",cmnfun.GetLoginUserGid().ToString()},
                        {"branchtier_insert_date",comm.GetCurrentDate()}	   
                    };

                  
                    string insertcommend = comm.InsertCommon(codes, "iem_mst_tbranchtier");

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

        public string Deletebranchtier(int ClassificationId)
        {
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {

                string[,] codes = new string[,]
	            {
                    {"branchtier_isremoved", "Y"}
                };
                string[,] whrcol = new string[,]
	            {
                    {"branchtier_gid", ClassificationId.ToString ()},
                    {"branchtier_isremoved", "N"}
                };

                string deletetbl = delecomm.DeleteCommon(codes, whrcol, "iem_mst_tbranchtier");
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

        public string Updatebranchtier(iem_mst_branchtier Classifications)
        {
            string result;

            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_branchtier", con);
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.AddWithValue("@gid", Classifications.branchtier_gid);
                cmd.Parameters.AddWithValue("@branchtier_name", Classifications.branchtier_name);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                result = cmd.Parameters["@res"].Value.ToString();

                if (result == "Not There")
                {
                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes = new string[,]
	                {
                        {"branchtier_name", Classifications.branchtier_name},
                        {"branchtier_update_by",cmnfun.GetLoginUserGid().ToString()},
                        {"branchtier_update_date", delecomm.GetCurrentDate()}
	                };
                    string[,] whrcol = new string[,]
	                {
                        {"branchtier_gid", Classifications.branchtier_gid.ToString () },
                        {"branchtier_isremoved", "N" }
                    };

                    string updcomm = delecomm.UpdateCommon(codes, whrcol, "iem_mst_tbranchtier");
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
    }
}