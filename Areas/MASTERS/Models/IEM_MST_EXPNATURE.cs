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
    public class IEM_MST_EXPNATURE : Iiem_mst_expnature
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
        public IEnumerable<iem_mst_expnature> getexpnature()
        {
            try
            {
                List<iem_mst_expnature> objOrgType = new List<iem_mst_expnature>();
                iem_mst_expnature objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_expnature", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_expnature();
                    objModel.expnature_gid = Convert.ToInt32(dt.Rows[i]["expnature_gid"].ToString());
                    objModel.expnature_name = Convert.ToString(dt.Rows[i]["expnature_name"].ToString());
                    objModel.expnature_active = Convert.ToString(dt.Rows[i]["expnature_active"].ToString());
                    objModel.expnature_istravel = Convert.ToString(dt.Rows[i]["expnature_istravel"].ToString());
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

        public IEnumerable<iem_mst_expnature> getexpnature(string filter_name)
        {
            try
            {
                List<iem_mst_expnature> objOrgType = new List<iem_mst_expnature>();
                iem_mst_expnature objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_expnature", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.Parameters.AddWithValue("@expnature_name", filter_name);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_expnature();
                    objModel.expnature_gid = Convert.ToInt32(dt.Rows[i]["expnature_gid"].ToString());
                    objModel.expnature_name = Convert.ToString(dt.Rows[i]["expnature_name"].ToString());
                    objModel.expnature_active = Convert.ToString(dt.Rows[i]["expnature_active"].ToString());
                    objModel.expnature_istravel = Convert.ToString(dt.Rows[i]["expnature_istravel"].ToString());
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

        public iem_mst_expnature GetexpnatureById(int expnatureId)
        {
            try
            {
                List<iem_mst_expnature> objOrgType = new List<iem_mst_expnature>();
                iem_mst_expnature objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_expnature", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", expnatureId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objModel = new iem_mst_expnature()
                {
                    expnature_gid = Convert.ToInt32(dt.Rows[0]["expnature_gid"].ToString()),
                    expnature_name = Convert.ToString(dt.Rows[0]["expnature_name"].ToString()),
                    expnature_active = Convert.ToString(dt.Rows[0]["expnature_active"].ToString()),
                    expnature_istravel = Convert.ToString(dt.Rows[0]["expnature_istravel"].ToString()),
                    //expnature_isothertravel = Convert.ToString(dt.Rows[0]["expnature_isother_travel"].ToString()),
                };
                return objModel;

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
            //throw new NotImplementedException();
        }

        public string Insertexpnature(iem_mst_expnature expnature)
        {
            string result;

            try
            {
                SqlCommand cmd = new SqlCommand("pr_iem_mst_expnature", con);
                GetCon();
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.AddWithValue("@expnature_name", expnature.expnature_name);
                cmd.Parameters.AddWithValue("@expnature_active", expnature.expnature_active);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();

                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]            
	                {
                        {"expnature_name",expnature.expnature_name},
                        {"expnature_active",expnature.expnature_active},
                          {"expnature_istravel",expnature.expnature_istravel},
                        //{"expnature_isother_travel",expnature.expnature_isothertravel},
                        {"expnature_insert_by",Convert.ToString (expnature.expnature_insert_by)},
                        {"expnature_insert_date","sysdatetime()"}
                    };

                    CommonIUD comm = new CommonIUD();
                    string insertcommend = comm.InsertCommon(codes, "iem_mst_texpnature");
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

     

        public string Updateexpnature(iem_mst_expnature expnature)
        {
            string result;

            try
            {
                SqlCommand cmd = new SqlCommand("pr_iem_mst_expnature", con);
                GetCon();
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.AddWithValue("@gid", expnature.expnature_gid);
                cmd.Parameters.AddWithValue("@expnature_name", expnature.expnature_name);
               
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes = new string[,]
	                {
                        {"expnature_name", expnature.expnature_name},                         
                        {"expnature_active", expnature.expnature_active},
                          {"expnature_istravel",expnature.expnature_istravel},
                     //   {"expnature_isother_travel",expnature.expnature_isothertravel},
                        {"expnature_update_by", expnature.expnature_update_by.ToString()},
                        {"expnature_update_date", "sysdatetime()"}
	                };
                    string[,] whrcol = new string[,]
	                {
                        {"expnature_gid", expnature.expnature_gid.ToString ()}
                    };

                    string updcomm = delecomm.UpdateCommon(codes, whrcol, "iem_mst_texpnature");
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


        public string Deleteexpnature(int expnatureId)
        {
            string result ;
            try
            {
                GetCon();
                DataTable getserilno = new DataTable();
                SqlCommand cmd = new SqlCommand("select * from iem_mst_texpcat  where expcat_expnature_gid=" + expnatureId + " and expcat_isremoved='N'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(getserilno);
                if (getserilno.Rows.Count == 0)
                {
                    CommonIUD delecomm = new CommonIUD();
                    string col_value = string.Empty;
                    string col_temp = string.Empty;

                    string[,] codes = new string[,]
	            {
                    {"expnature_isremoved", "Y"}
	            };

                    string[,] whrcol = new string[,]
	            {
                    {"expnature_gid", expnatureId.ToString ()}
                };

                    string deletetbl = delecomm.DeleteCommon(codes, whrcol, "iem_mst_texpnature");
                    result = deletetbl;

                }
                else
                {
                    result = "Can Not Delete this, Its Based On Some Other category";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally{
                con.Close();
            }
            return result;
            // throw new NotImplementedException();
        }
    }
}