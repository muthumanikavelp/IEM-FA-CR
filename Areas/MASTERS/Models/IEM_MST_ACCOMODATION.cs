using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using IEM.Common;
using System.Configuration;
namespace IEM.Areas.MASTERS.Models
{
    public class IEM_MST_ACCOMODATION : Iiem_mst_accomodationtype
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions cmn = new CmnFunctions();
        private void GetCon()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }
        public IEnumerable<iem_mst_accomodationtype> GetAccomodationtype()
        {
            try
            {
                List<iem_mst_accomodationtype> objOrgType = new List<iem_mst_accomodationtype>();
                iem_mst_accomodationtype objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_accommodationtype", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_accomodationtype();
                    objModel.accommodationtype_gid = Convert.ToInt32(dt.Rows[i]["accommodationtype_gid"].ToString());
                    objModel.accommodationtype_name = Convert.ToString(dt.Rows[i]["accommodationtype_name"].ToString());
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
        public IEnumerable<iem_mst_accomodationtype> GetAccomodationtype(string Accomodationtype)
        {
            try
            {
                List<iem_mst_accomodationtype> objOrgType = new List<iem_mst_accomodationtype>();
                iem_mst_accomodationtype objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_accommodationtype", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.Parameters.AddWithValue("@accommodationtype_name", Accomodationtype);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_accomodationtype();
                    objModel.accommodationtype_gid = Convert.ToInt32(dt.Rows[i]["accommodationtype_gid"].ToString());
                    objModel.accommodationtype_name = Convert.ToString(dt.Rows[i]["accommodationtype_name"].ToString());
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

        public iem_mst_accomodationtype GetAccomodationtypeById(int AccomodationtypeId)
        {
          
            try
            {
                List<iem_mst_accomodationtype> objOrgType = new List<iem_mst_accomodationtype>();
                //iem_mst_accomodationtype objModel;
                var objmodel=new iem_mst_accomodationtype();
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_accommodationtype", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", AccomodationtypeId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
               objmodel = new iem_mst_accomodationtype()
                {                   
                   accommodationtype_gid = Convert.ToInt32(dt.Rows[0]["accommodationtype_gid"].ToString()),
                   accommodationtype_name = Convert.ToString(dt.Rows[0]["accommodationtype_name"].ToString()),                    
                };
               return objmodel;
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

        public string Insertaccomodation(iem_mst_accomodationtype Accomodationtype)
        {
            string result=string.Empty;
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_accommodationtype", con);
                cmd.Parameters.AddWithValue("@action", "InsertExist");
                cmd.Parameters.AddWithValue("@accommodationtype_name", Accomodationtype.accommodationtype_name);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                result = cmd.Parameters["@res"].Value.ToString();
                CommonIUD comm = new CommonIUD();
                if (result == "Not There")
                {
                    string[,] codes = new string[,]
	                {
                        {"accommodationtype_name",Accomodationtype.accommodationtype_name},       
                        {"accommodationtype_insert_by",cmn.GetLoginUserGid().ToString ()},
                        {"accommodationtype_insert_date",comm.GetCurrentDate()}	   
                    };


                    string insertcommend = comm.InsertCommon(codes, "iem_mst_taccommodationtype");

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
            } return result;
        }

        public string DeleteAuditsubgroup(int AccomodationtypeId)
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
                    {"accommodationtype_gid", AccomodationtypeId.ToString ()},
                    {"accommodationtype_isremoved", "N"}
                };

                string deletetbl = delecomm.DeleteCommon(codes, whrcol, "iem_mst_taccommodationtype");
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

        public string UpdateAccomodationtype(iem_mst_accomodationtype UpdateAccomodationtype)
        {
            string result;

            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_accommodationtype", con);
                cmd.Parameters.AddWithValue("@action", "UpdateExist");
                cmd.Parameters.AddWithValue("@gid", UpdateAccomodationtype.accommodationtype_gid);
                cmd.Parameters.AddWithValue("@accommodationtype_name", UpdateAccomodationtype.accommodationtype_name);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                result = cmd.Parameters["@res"].Value.ToString();

                if (result == "Not There")
                {
                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes = new string[,]
	                {
                        {"accommodationtype_name",UpdateAccomodationtype.accommodationtype_name},       
                        {"accommodationtype_update_by",cmn.GetLoginUserGid().ToString ()},
                        {"accommodationtype_update_date",delecomm.GetCurrentDate()}	  
	                };
                    string[,] whrcol = new string[,]
	                {
                        {"accommodationtype_gid", UpdateAccomodationtype.accommodationtype_gid.ToString () },
                        {"accommodationtype_isremoved", "N" }
                    };

                    string updcomm = delecomm.UpdateCommon(codes, whrcol, "iem_mst_taccommodationtype");
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