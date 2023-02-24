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
    public class IEM_MST_ADVANCETYPE : Iiem_mst_advancetype
    {
        SqlConnection con = new SqlConnection();
        CmnFunctions cfunc = new CmnFunctions();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        int advancetype_gid;

        private void GetCon()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }
        
        public IEnumerable<iem_mst_advancetype> getadvancetypeGrid()
        {
            try
            {
                List<iem_mst_advancetype> objOrgType = new List<iem_mst_advancetype>();
                iem_mst_advancetype objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_advancetype", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_advancetype();
                    objModel.advancetype_gid = Convert.ToInt32(dt.Rows[i]["advancetype_gid"].ToString());
                    objModel.advancetype_name = Convert.ToString(dt.Rows[i]["advancetype_name"].ToString());
                    objModel.advancetype_gl_no = Convert.ToString(dt.Rows[i]["advancetype_gl_no"].ToString());
                    objModel.advancetype_active = Convert.ToString(dt.Rows[i]["advancetype_active"].ToString());
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

        public IEnumerable<iem_mst_advancetype> getadvancetype(string advanceypename, string advancetyp_gl_no)
        {
            try
            {
                List<iem_mst_advancetype> objOrgType = new List<iem_mst_advancetype>();
                iem_mst_advancetype objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_advancetype", con);
                cmd.Parameters.AddWithValue("@action", "Search");
                cmd.Parameters.AddWithValue("@advancetype_name", advanceypename);
                cmd.Parameters.AddWithValue("@advancetype_gl_no", advancetyp_gl_no);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_advancetype();
                    objModel.advancetype_gid = Convert.ToInt32(dt.Rows[i]["advancetype_gid"].ToString());
                    objModel.advancetype_name = Convert.ToString(dt.Rows[i]["advancetype_name"].ToString());
                    objModel.advancetype_gl_no = Convert.ToString(dt.Rows[i]["advancetype_gl_no"].ToString());
                    objModel.advancetype_active = Convert.ToString(dt.Rows[i]["advancetype_active"].ToString());
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

        public iem_mst_advancetype GetadvancetypeById(int advancetypegid)
        {
            try
            {
                List<iem_mst_advancetype> objOrgType = new List<iem_mst_advancetype>();
                iem_mst_advancetype objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_advancetype", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", advancetypegid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new iem_mst_advancetype()
                {
                    advancetype_gid = Convert.ToInt32(dt.Rows[0]["advancetype_gid"].ToString()),
                    advancetype_name = Convert.ToString(dt.Rows[0]["advancetype_name"].ToString()),
                    advancetype_gl_no = Convert.ToString(dt.Rows[0]["advancetype_gl_no"].ToString()),
                    advancesubtype_name = Convert.ToString(dt.Rows[0]["advancesubtype_name"].ToString()),
                    advancetype_gl_desc = Convert.ToString(dt.Rows[0]["advancetype_gl_desc"].ToString()),
                    advancetype_active = Convert.ToString(dt.Rows[0]["advancetype_active"].ToString()),
                    advancetype_help = Convert.ToString(dt.Rows[0]["advancetype_help"].ToString ()),
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

        public string InsertAdvanceType(iem_mst_advancetype Classifications)
        {
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_advancetype", con);               
                cmd.Parameters.AddWithValue("@advancetype_name", Classifications.advancetype_name);
                cmd.Parameters.AddWithValue("@advancetype_gl_no", Classifications.advancetype_gl_no);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@action", "InsertExist");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["@res"].Value.ToString();
                if (result == "Not There")
                {
                    if(Classifications.advancesubtype_name=="Employee")
                    {
                        advancetype_gid = 1;
                    }
                    if (Classifications.advancesubtype_name == "Supplier")
                    {
                        advancetype_gid = 2;
                    }
                    if (Classifications.advancesubtype_name == "Petty Cash")
                    {
                        advancetype_gid = 3;
                    }
                    CommonIUD comm = new CommonIUD();
                    string[,] codes = new string[,]            
	                {
                        {"advancetype_name",Classifications.advancetype_name },
                        {"advancetype_gl_no",Classifications.advancetype_gl_no},
                        {"advancetype_gl_desc",String.IsNullOrEmpty(Classifications.advancetype_gl_desc )?"":Classifications.advancetype_gl_desc.ToString()},
                        {"advancesubtype_name",Classifications.advancesubtype_name},
                        {"advancetype_active",Classifications.advancetype_active},
                        {"advancetype_help",Classifications.advancetype_help},
                        {"advancetype_insert_by",cfunc.GetLoginUserGid().ToString ()},
                        {"advancetype_insert_date",comm.GetCurrentDate()},
                        {"advancetype_arftype_gid",advancetype_gid.ToString()}
                    };
                    string tname = "iem_mst_tadvancetype";
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

        public string DeleteAdvanceType(int ClassificationId)
        {
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {

                string[,] codes = new string[,]
	       {
                 {"advancetype_isremoved", "Y"}
	            
           };
                string[,] whrcol = new string[,]
	        {
                {"advancetype_gid", ClassificationId.ToString ()}
            };
                string tblname = "iem_mst_tadvancetype";


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

        public string UpdateAdvanceType(iem_mst_advancetype Classifications)
        {
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_advancetype", con);
                cmd.Parameters.AddWithValue("@advancetype_name", Classifications.advancetype_name);
                cmd.Parameters.AddWithValue("@advancetype_gl_no", Classifications.advancetype_gl_no);
                cmd.Parameters.AddWithValue("@gid", Classifications.advancetype_gid);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.Parameters.AddWithValue("@action", "UpdateExist");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["@res"].Value.ToString();
                if (result == "Not There")
                {
                    if (Classifications.advancesubtype_name == "Employee")
                    {
                        advancetype_gid = 1;
                    }
                    if (Classifications.advancesubtype_name == "Supplier")
                    {
                        advancetype_gid = 2;
                    }
                    if (Classifications.advancesubtype_name == "Petty Cash")
                    {
                        advancetype_gid = 3;
                    }
                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes = new string[,]
	                  {
                        {"advancetype_name",Classifications.advancetype_name },
                        {"advancetype_gl_no",Classifications.advancetype_gl_no},
                        {"advancetype_active",Classifications.advancetype_active},
                        {"advancesubtype_name",Classifications.advancesubtype_name},
                        {"advancetype_gl_desc",String.IsNullOrEmpty(Classifications.advancetype_gl_desc )?"":Classifications.advancetype_gl_desc.ToString()},
                        {"advancetype_help",Classifications.advancetype_help},
                        {"advancetype_update_by",cfunc.GetLoginUserGid().ToString ()},
                        {"advancetype_update_date",delecomm.GetCurrentDate()},
                        {"advancetype_arftype_gid",advancetype_gid.ToString()}
	                  };
                    string[,] whrcol = new string[,]
	                 {
                          {"advancetype_gid", Classifications.advancetype_gid.ToString ()},
                          {"advancetype_isremoved", "N"}
                     };

                    string updcomm = delecomm.UpdateCommon(codes, whrcol, "iem_mst_tadvancetype");
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


        public IEnumerable<GetGL> GetGL()
        {
            try
            {
                List<GetGL> objCountrytype = new List<GetGL>();
                GetGL objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_advancetype", con);
                cmd.Parameters.AddWithValue("@action", "selectGlNo");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new GetGL();
                    objModel.gl_gid = Convert.ToInt32(dt.Rows[i]["gl_gid"].ToString());
                    objModel.gl_no = Convert.ToString(dt.Rows[i]["gl_no"].ToString());                   
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


        public DataTable getglno(int glgid)
        {
            DataTable dt = new DataTable();
            try
            {                
                GetCon();               
                SqlCommand cmd = new SqlCommand("pr_iem_mst_advancetype", con);
                cmd.Parameters.AddWithValue("@action", "selectGlNoById");
                cmd.Parameters.AddWithValue("@gid",glgid);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
                
            catch(Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return dt;
     }


        public DataTable getglgid(int glno)
        {
            DataTable dt = new DataTable();
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_advancetype", con);
                cmd.Parameters.AddWithValue("@action", "selectGlGid");
                cmd.Parameters.AddWithValue("@gid", glno);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return dt;
        }

        public DataTable getglnoByadvanctyid(int atypegid)
        {
            DataTable dt = new DataTable();
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_advancetype", con);
                cmd.Parameters.AddWithValue("@action", "selectGlGidByAdvanceTypeId");
                cmd.Parameters.AddWithValue("@gid", atypegid);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return dt;
        }
    }
}