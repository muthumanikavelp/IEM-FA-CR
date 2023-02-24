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
    public class IEM_MST_FINYEAR : Iiem_mst_finyear
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions com = new CmnFunctions();

        private void GetCon()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }
        public IEnumerable<iem_mst_finayear> GetFinyear()
        {
            try
            {
                List<iem_mst_finayear> objOrgType = new List<iem_mst_finayear>();
                iem_mst_finayear objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_finyear", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_finayear();
                    objModel.finyear_gid = Convert.ToInt32(dt.Rows[i]["finyear_gid"].ToString());
                    objModel.finyear_code = Convert.ToString(dt.Rows[i]["finyear_code"].ToString());
                    objModel.finyear_start_date = Convert.ToString(dt.Rows[i]["finyear_start_date"].ToString());
                    objModel.finyear_end_date = Convert.ToString(dt.Rows[i]["finyear_end_date"].ToString());
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

        public IEnumerable<iem_mst_finayear> GetFinyear(string finyearcode, string periodfrom, string periodto)
        {
            try
            {
                List<iem_mst_finayear> objOrgType = new List<iem_mst_finayear>();
                iem_mst_finayear objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_finyear", con);
                cmd.Parameters.AddWithValue("@action", "Search");
                cmd.Parameters.AddWithValue("@finyear_code", finyearcode);
                if (periodfrom != "")
                {
                    cmd.Parameters.AddWithValue("@finyear_start_date",com.convertoDateTimeString(periodfrom));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@finyear_start_date", periodfrom);
                }
                if (periodto != "")
                {
                    cmd.Parameters.AddWithValue("@finyear_end_date", com.convertoDateTimeString(periodto));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@finyear_end_date", periodto);
                }
               
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_finayear();
                    objModel.finyear_gid = Convert.ToInt32(dt.Rows[i]["finyear_gid"].ToString());
                    objModel.finyear_code = Convert.ToString(dt.Rows[i]["finyear_code"].ToString());
                    objModel.finyear_start_date = Convert.ToString(dt.Rows[i]["finyear_start_date"].ToString());
                    objModel.finyear_end_date = Convert.ToString(dt.Rows[i]["finyear_end_date"].ToString());
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

        public iem_mst_finayear GetFinyearById(int FinId)
        {
            try
            {
                List<iem_mst_finayear> objOrgType = new List<iem_mst_finayear>();
                iem_mst_finayear objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_finyear", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", FinId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new iem_mst_finayear()
                {
                    finyear_gid = Convert.ToInt32(dt.Rows[0]["finyear_gid"].ToString()),
                    finyear_code = Convert.ToString(dt.Rows[0]["finyear_code"].ToString()),
                    finyear_start_date = Convert.ToString(dt.Rows[0]["finyear_start_date"].ToString()),
                    finyear_end_date = Convert.ToString(dt.Rows[0]["finyear_end_date"].ToString()),
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

        public string InsertFinyear(iem_mst_finayear Finyear)
        {
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_finyear", con);
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.AddWithValue("@finyear_code", Finyear.finyear_code);
                cmd.Parameters.AddWithValue("@finyear_start_date", com.convertoDateTimeString(Finyear.finyear_start_date));
                cmd.Parameters.AddWithValue("@finyear_end_date", com.convertoDateTimeString(Finyear.finyear_end_date));
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["@res"].Value.ToString();

                if (result == "Not There")
                {
                    CommonIUD comm = new CommonIUD();
                    string[,] codes = new string[,]            
	                {
                        {"finyear_code",Finyear.finyear_code },
                        {"finyear_start_date",com.convertoDateTimeString(Finyear.finyear_start_date)},
                        {"finyear_end_date",com.convertoDateTimeString(Finyear.finyear_end_date)},                       
                        {"finyear_insert_by",com.GetLoginUserGid().ToString ()},
                        {"finyear_insert_date",comm.GetCurrentDate()}
                    };
                    string tname = "iem_mst_tfinyear";
                    string insertcommend = comm.InsertCommon(codes, tname);
                }
                else
                {
                    return "Duplicate Record";
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

        public string DeleteFinyear(int FinyearId)
        {
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {

                string[,] codes = new string[,]
	       {
                 {"finyear_isremoved", "Y"}
	            
           };
                string[,] whrcol = new string[,]
	        {
                {"finyear_gid", FinyearId.ToString ()}
            };
                string tblname = "iem_mst_tfinyear";


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

        public string UpdateFinyear(iem_mst_finayear UpdateFinyearmodel)
        {
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_finyear", con);
                cmd.Parameters.AddWithValue("@action", "UpdateExist");
                cmd.Parameters.AddWithValue("@finyear_code", UpdateFinyearmodel.finyear_code);
                cmd.Parameters.AddWithValue("@gid", UpdateFinyearmodel.finyear_gid);
                cmd.Parameters.AddWithValue("@finyear_start_date", com.convertoDateTimeString(UpdateFinyearmodel.finyear_start_date));
                cmd.Parameters.AddWithValue("@finyear_end_date", com.convertoDateTimeString(UpdateFinyearmodel.finyear_end_date));
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["@res"].Value.ToString();

                if (result == "Not There")
                {
                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes = new string[,]
	                  {
                        {"finyear_code",UpdateFinyearmodel.finyear_code },
                        {"finyear_start_date",com.convertoDateTimeString(UpdateFinyearmodel.finyear_start_date)},
                        {"finyear_end_date",com.convertoDateTimeString(UpdateFinyearmodel.finyear_end_date)},                       
                        {"finyear_insert_by",com.GetLoginUserGid().ToString ()},
                        {"finyear_insert_date",delecomm.GetCurrentDate()}
                    };
                    string[,] whrcol = new string[,]
	                 {
                          {"finyear_gid", UpdateFinyearmodel.finyear_gid.ToString ()},
                          {"finyear_isremoved", "N"}
                     };

                    string updcomm = delecomm.UpdateCommon(codes, whrcol, "iem_mst_tfinyear");
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