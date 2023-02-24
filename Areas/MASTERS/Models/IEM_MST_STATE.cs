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
    public class IEM_MST_STATE : Iiem_mst_state
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
        public IEnumerable<iem_mst_state> getstate()
        {
            try
            {
                List<iem_mst_state> objOrgType = new List<iem_mst_state>();
                iem_mst_state objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_state", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_state();
                    objModel.state_gid = Convert.ToInt32(dt.Rows[i]["state_gid"].ToString());
                    objModel.state_code= Convert.ToString(dt.Rows[i]["state_code"].ToString());
                    objModel.state_name= Convert.ToString(dt.Rows[i]["state_name"].ToString());
                    objModel.region_name = Convert.ToString(dt.Rows[i]["region_name"].ToString());
                    objModel.country_name= Convert.ToString(dt.Rows[i]["country_name"].ToString());                   
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
        public IEnumerable<iem_mst_state> getstateid(string state_code, string state_name)
        {
            try
            {
                List<iem_mst_state> objOrgType = new List<iem_mst_state>();
                iem_mst_state objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_state", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.Parameters.AddWithValue("@state_code", state_code);
                cmd.Parameters.AddWithValue("@state_name", state_name);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_state();
                    objModel.state_gid = Convert.ToInt32(dt.Rows[i]["state_gid"].ToString());
                    objModel.state_code = Convert.ToString(dt.Rows[i]["state_code"].ToString());
                    objModel.state_name = Convert.ToString(dt.Rows[i]["state_name"].ToString());
                    objModel.region_name = Convert.ToString(dt.Rows[i]["region_name"].ToString());
                    objModel.country_name = Convert.ToString(dt.Rows[i]["country_name"].ToString());
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
        public iem_mst_state GetStateId(int StateId)
        {

            try
            {
                List<iem_mst_state> objOrgType = new List<iem_mst_state>();
                iem_mst_state objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_state", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@state_gid", StateId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objModel = new iem_mst_state()
                {
                    state_gid= Convert.ToInt32(dt.Rows[0]["state_gid"].ToString()),
                    state_code = Convert.ToString(dt.Rows[0]["state_code"].ToString()),
                    state_name = Convert.ToString(dt.Rows[0]["state_name"].ToString()),
                    region_name = Convert.ToString(dt.Rows[0]["region_name"].ToString()),
                    country_name = Convert.ToString(dt.Rows[0]["country_name"].ToString()),
                    selectedregion_gid = Convert.ToInt32(dt.Rows[0]["region_gid"].ToString()),
                    selectedcountry_gid = Convert.ToInt32(dt.Rows[0]["country_gid"].ToString()),
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
            // throw new NotImplementedException();
        }

        public string InsertState(iem_mst_state State)
        {
            string result;
            try
            {
                CommonIUD comm = new CommonIUD();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_state", con);
                GetCon();
                cmd.Parameters.AddWithValue("@action", "SelExist");              
                cmd.Parameters.AddWithValue("@state_code", State.state_code);
                cmd.Parameters.AddWithValue("@state_name", State.state_name);        
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
	               {
                        {"state_code",State.state_code},
                        {"state_name",State.state_name.ToString() },
	                    {"state_region_gid", State.region_gid.ToString ()},
                        {"state_region_name", State.region_name.ToString ()},
                        {"state_country_gid",State.country_gid.ToString ()},   
                        {"state_country_code",State.country_code.ToString ()}, 
                        {"state_insert_by	",State.state_insert_by .ToString ()},
                        {"state_insert_date", comm.GetCurrentDate()} 

                  };
                    string tname = "iem_mst_tstate";
                    string insertcommend = comm.InsertCommon(codes, tname);
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
            //throw new NotImplementedException();
        }

        

       
        public IEnumerable<Getregion> Getregion()
        {
            try
            {
                List<Getregion> objregiontype = new List<Getregion>();
                Getregion objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_region", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new Getregion();
                    objModel.regiongid = Convert.ToInt32(dt.Rows[i]["region_gid"].ToString());                    
                    objModel.regionname = Convert.ToString(dt.Rows[i]["region_name"].ToString());
                    objregiontype.Add(objModel);
                }

                return objregiontype;
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

        public GetregionById GetregionById(int Region)
        {
            try
            {
                List<GetregionById> objOrgType = new List<GetregionById>();
                GetregionById objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_region", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", Region);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objModel = new GetregionById()
                {
                    regiongid = Convert.ToInt32(dt.Rows[0]["region_gid"].ToString()),
                    regionname = Convert.ToString(dt.Rows[0]["region_name"].ToString()),
                   // regioncode = Convert.ToString(dt.Rows[0]["region_code"].ToString()),
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
            // throw new NotImplementedException();
        }

        public IEnumerable<Getcountry> Getcountry()
        {
            try
            {
                List<Getcountry> objCountrytype = new List<Getcountry>();
                Getcountry objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_country", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new Getcountry();
                    objModel.countrygid = Convert.ToInt32(dt.Rows[i]["country_gid"].ToString());
                    objModel.countrycode = Convert.ToString(dt.Rows[i]["country_code"].ToString());
                    objModel.countryname = Convert.ToString(dt.Rows[i]["country_name"].ToString());
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
            // throw new NotImplementedException();
        }

        public GetcountryById GetcountryById(int Country)
        {
            try
            {
                List<GetcountryById> objOrgType = new List<GetcountryById>();
                GetcountryById objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_country", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", Country);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objModel = new GetcountryById()
                {
                    countrygid = Convert.ToInt32(dt.Rows[0]["country_gid"].ToString()),
                    countryname = Convert.ToString(dt.Rows[0]["country_name"].ToString()),
                    countrycode = Convert.ToString(dt.Rows[0]["country_code"].ToString()),
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
            // throw new NotImplementedException();
        }      

        //public string DeleteState(int StateId)
        //{

        //    CommonIUD delecomm = new CommonIUD();
        //    string col_value = string.Empty;
        //    string col_temp = string.Empty;
        //    try
        //    {

        //        string[,] codes = new string[,]
        //   {
        //         {"state_isremoved", "Y"}
	            
        //   };
        //        string[,] whrcol = new string[,]
        //    {
        //        {"state_gid", StateId.ToString ()}
        //    };
        //        string tblname = "iem_mst_tstate";


        //        string deletetbl = delecomm.DeleteCommon(codes, whrcol, tblname);
        //        return deletetbl;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    //throw new NotImplementedException();
        //}
        public string DeleteState(int StateId)
        {
            string result;
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;

            try
            {

                CommonIUD comm1 = new CommonIUD();
                SqlCommand cmd1 = new SqlCommand("pr_iem_mst_state", con);
                GetCon();
                cmd1.Parameters.AddWithValue("@action", "Delete_city");
                cmd1.Parameters.AddWithValue("@gid", StateId);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd1.ExecuteNonQuery();
                string res2 = cmd1.Parameters["@res"].Value.ToString();
                if (res2 == "Not There")
                {
                    string[,] codes = new string[,]
	                {
                         {"state_isremoved", "Y"}
	            
                         };
                    string[,] whrcol = new string[,]
	                    {
                          {"state_gid", StateId.ToString ()}
                        };
                    string tblname = "iem_mst_tstate";


                    string deletetbl = delecomm.DeleteCommon(codes, whrcol, tblname);
                    result = "success";
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
            finally
            {
                con.Close();
            }

        }
        public string UpdateState(iem_mst_state State)
        {
            try
            {
                CommonIUD comm = new CommonIUD();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_state", con);
                GetCon();
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.AddWithValue("@gid", State.state_gid);
                cmd.Parameters.AddWithValue("@state_name", State.state_name);
                cmd.Parameters.AddWithValue("@state_code", State.state_code);    
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes = new string[,]
	                  {
                          {"state_code", State.state_code},
                          {"state_name", State.state_name},
                          {"state_region_gid", State.selectedregion_gid.ToString ()},
                          {"state_region_name", State.region_name.ToString()},
                          {"state_country_gid", State.selectedcountry_gid.ToString ()},
                          {"state_country_code", State.country_code},
                          {"state_update_by", State.state_update_by.ToString ()},
                          {"state_update_date", delecomm.GetCurrentDate()}
	                  };
                    string[,] whrcol = new string[,]
	                 {
                          {"state_gid", State.state_gid.ToString ()}
            
                     };
                    string tblname = "iem_mst_tstate";

                    string updcomm = delecomm.UpdateCommon(codes, whrcol, tblname);
                }
                else
                {
                    return "Duplicate Record !";
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return "success";
            // throw new NotImplementedException();
        }
    }
}