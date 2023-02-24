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
    public class IEM_MST_BRANCHTYPE : Iiem_mst_branchtype
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions cmf = new CmnFunctions();
        private void GetCon()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }
        public IEnumerable<iem_mst_branchtype> getbranchtype()
        {
            try
            {
                List<iem_mst_branchtype> objOrgType = new List<iem_mst_branchtype>();
                iem_mst_branchtype objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_branchtype", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_branchtype();
                    objModel.branchtype_gid = Convert.ToInt32(dt.Rows[i]["branchtype_gid"].ToString());
                    objModel.branchtype_name = Convert.ToString(dt.Rows[i]["branchtype_name"].ToString());
                    objModel.branchtype_active = Convert.ToString(dt.Rows[i]["branchtype_active"].ToString());
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

        public IEnumerable<iem_mst_branchtype> getbranchtype(string filter_code)
        {
            try
            {
                List<iem_mst_branchtype> objOrgType = new List<iem_mst_branchtype>();
                iem_mst_branchtype objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_branchtype", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.Parameters.AddWithValue("@branchtype_name", filter_code);               
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_branchtype();
                    objModel.branchtype_gid = Convert.ToInt32(dt.Rows[i]["branchtype_gid"].ToString());
                    objModel.branchtype_name = Convert.ToString(dt.Rows[i]["branchtype_name"].ToString());
                    objModel.branchtype_active = Convert.ToString(dt.Rows[i]["branchtype_active"].ToString()); 
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

        public iem_mst_branchtype GetbranchtypeById(int ClassificationId)
        {
            try
            {
                List<iem_mst_branchtype> objOrgType = new List<iem_mst_branchtype>();
                iem_mst_branchtype objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_branchtype", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", ClassificationId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new iem_mst_branchtype()
                {
                    branchtype_gid = Convert.ToInt32(dt.Rows[0]["branchtype_gid"].ToString()),
                    branchtype_name = Convert.ToString(dt.Rows[0]["branchtype_name"].ToString()),
                    branchtype_active = Convert.ToString(dt.Rows[0]["branchtype_active"].ToString()),
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

        public string Insertbranchtype(iem_mst_branchtype Classifications)
        {
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_branchtype", con);
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.AddWithValue("@branchtype_name", Classifications.branchtype_name);
                //cmd.Parameters.AddWithValue("@branchtype_active", Classifications.branchtype_active);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["@res"].Value.ToString();

                if (result == "Not There")
                {
                    CommonIUD comm = new CommonIUD();
                    string[,] codes = new string[,]            
	                {
                        {"branchtype_name",Classifications.branchtype_name },
                        {"branchtype_active",Classifications.branchtype_active},
                        {"branchtype_insert_by",cmf.GetLoginUserGid().ToString ()},
                        {"branchtype_insert_date",comm.GetCurrentDate()}
                    };
                    string tname = "iem_mst_tbranchtype";
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

        public string Deletebranchtype(int ClassificationId)
        {
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {

                string[,] codes = new string[,]
	       {
                 {"branchtype_isremoved", "Y"}
	            
           };
                string[,] whrcol = new string[,]
	        {
                {"branchtype_gid", ClassificationId.ToString ()}
            };
                string tblname = "iem_mst_tbranchtype";


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

        public string Updatebranchtype(iem_mst_branchtype Classifications)
        {
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_branchtype", con);
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.AddWithValue("@gid", Classifications.branchtype_gid);
                cmd.Parameters.AddWithValue("@branchtype_name", Classifications.branchtype_name);                
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["@res"].Value.ToString();

                if (result == "Not There")
                {
                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes = new string[,]
	                  {
                          {"branchtype_name", Classifications.branchtype_name},
                          {"branchtype_active", Classifications.branchtype_active},
                          {"branchtype_update_by", Classifications.branchtype_update_by.ToString ()},
                          {"branchtype_update_date", delecomm.GetCurrentDate()}
	                  };
                    string[,] whrcol = new string[,]
	                 {
                          {"branchtype_gid", Classifications.branchtype_gid.ToString ()},
                          {"branchtype_isremoved", "N"}
                     };

                    string updcomm = delecomm.UpdateCommon(codes, whrcol, "iem_mst_tbranchtype");
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