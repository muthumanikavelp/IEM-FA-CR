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
    public class IEM_MST_CITY : Iiem_mst_city
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
        public IEnumerable<iem_mst_city> getcity()
        {
            try
            {
                List<iem_mst_city> objOrgType = new List<iem_mst_city>();
                iem_mst_city objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_city", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_city();
                    objModel.city_gid= Convert.ToInt32(dt.Rows[i]["city_gid"].ToString());
                    objModel.city_code = Convert.ToString(dt.Rows[i]["city_code"].ToString());
                    objModel.city_name= Convert.ToString(dt.Rows[i]["city_name"].ToString());
                    objModel.city_pincode= Convert.ToInt32(dt.Rows[i]["city_pincode"].ToString());                  
                    objModel.state_name = Convert.ToString(dt.Rows[i]["state_name"].ToString());                   
                    objModel.region_name = Convert.ToString(dt.Rows[i]["region_name"].ToString());                   
                    objModel.country_name = Convert.ToString(dt.Rows[i]["country_name"].ToString());
                    objModel.cityclass_name= Convert.ToString(dt.Rows[i]["cityclass_name"].ToString());
                    objModel.tier_name = Convert.ToString(dt.Rows[i]["tier_name"].ToString()); 
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
        public IEnumerable<iem_mst_city> getcity_id(string city_code, string city_name, string city_pincode)
        {
            try
            {
                List<iem_mst_city> objOrgType = new List<iem_mst_city>();
                iem_mst_city objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_city", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.Parameters.AddWithValue("@city_code", city_code);
                cmd.Parameters.AddWithValue("@city_name", city_name);
                cmd.Parameters.AddWithValue("@city_pincode", city_pincode);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_city();
                    objModel.city_gid = Convert.ToInt32(dt.Rows[i]["city_gid"].ToString());
                    objModel.city_code = Convert.ToString(dt.Rows[i]["city_code"].ToString());
                    objModel.city_name = Convert.ToString(dt.Rows[i]["city_name"].ToString());
                    objModel.city_pincode = Convert.ToInt32(dt.Rows[i]["city_pincode"].ToString());
                    objModel.state_name = Convert.ToString(dt.Rows[i]["state_name"].ToString());
                    objModel.region_name = Convert.ToString(dt.Rows[i]["region_name"].ToString());
                    objModel.country_name = Convert.ToString(dt.Rows[i]["country_name"].ToString());
                    objModel.cityclass_name = Convert.ToString(dt.Rows[i]["cityclass_name"].ToString());
                    objModel.tier_name = Convert.ToString(dt.Rows[i]["tier_name"].ToString());
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
        public iem_mst_city GetcityId(int CityId)
        {
            iem_mst_city objModel = new iem_mst_city();
            List<iem_mst_city> objOrgType = new List<iem_mst_city>();
            try
            {


                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_city", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", CityId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                int x = 0;
                if (dt.Rows.Count > 0)
                {

                    bool cityclassgid = int.TryParse(dt.Rows[0]["cityclass_gid"].ToString().Trim(), out x);
                    bool tiergid = int.TryParse(dt.Rows[0]["tier_gid"].ToString().Trim().Trim(), out x);
                    objModel = new iem_mst_city()
                    {
                        city_gid = Convert.ToInt32(dt.Rows[0]["city_gid"].ToString().Trim()),
                        city_code = Convert.ToString(dt.Rows[0]["city_code"].ToString().Trim()),
                        city_name = Convert.ToString(dt.Rows[0]["city_name"].ToString().Trim()),
                        city_pincode = Convert.ToInt32(dt.Rows[0]["city_pincode"].ToString().Trim()),
                        selectedstate_gid = Convert.ToInt32(dt.Rows[0]["state_gid"].ToString().Trim()),
                        selectedregion_gid = Convert.ToInt32(dt.Rows[0]["region_gid"].ToString().Trim()),
                        selectedcountry_gid = Convert.ToInt32(dt.Rows[0]["country_gid"].ToString().Trim()),
                        selectedcityclass_gid = Convert.ToInt32(cityclassgid),
                        selectedtier_gid = Convert.ToInt32(tiergid)
                    };

                    return objModel;
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
            // throw new NotImplementedException();
            return objModel;
        }

        public string InsertCity(iem_mst_city City)
        {
            string result;
            try
            {
                CommonIUD comm = new CommonIUD();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_city", con);
                GetCon();
                cmd.Parameters.AddWithValue("@action", "InExist");
                cmd.Parameters.AddWithValue("@city_code", City.city_code);
                cmd.Parameters.AddWithValue("@citypincode", City.city_pincode);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
	               {
                        {"city_code",City.city_code},
                        {"city_name",City.city_name.ToString() },
	                    {"city_pincode", City.city_pincode.ToString ()},
	                    {"city_state_gid", City.state_gid.ToString ()},
                        {"city_state_code", City.state_code.ToString ()},
                         {"city_state_name", City.state_name.ToString ()},
                        {"city_region_gid", City.region_gid.ToString ()},
                         {"city_region_name", City.region_name},
                        {"city_country_gid", City.country_gid.ToString ()},
                        // {"city_country_code", City.country_code},
                        //{"city_country_name", City.country_name.ToString ()},
                          {"city_cityclass_gid", City.cityclass_gid.ToString ()},
                         {"city_cityclass_code", City.cityclass_code},
                        {"city_tier_gid	", City.tier_gid.ToString ()},
                        {"city_tier_code	", City.tier_code.ToString ()},
                        {"city_insert_by	", City.city_insert_by.ToString ()},
                        {"city_insert_date", comm.GetCurrentDate()} 

                  };
                    string tname = "iem_mst_tcity";
                    string insertcommend = comm.InsertCommon(codes, tname);
                    result = "success";
                }
                else
                {
                    result = "Duplicate record !";
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

                // throw new NotImplementedException();
            }
        

        public void DeleteCity(int CityId)
        {
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {

                string[,] codes = new string[,]
	       {
                 {"city_isremoved", "Y"}
	            
           };
                string[,] whrcol = new string[,]
	        {
                {"city_gid", CityId.ToString ()}
            };
                string tblname = "iem_mst_tcity";


                string deletetbl = delecomm.DeleteCommon(codes, whrcol, tblname);
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

        //public string UpdateCity(iem_mst_city City)
        //{
            



        //    // throw new NotImplementedException();
        //}

        public IEnumerable<Getstate> Getstate()
        {
            try
            {
                List<Getstate> objCountrytype = new List<Getstate>();
                Getstate objModel;
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
                    objModel = new Getstate();
                    objModel.stategid = Convert.ToInt32(dt.Rows[i]["state_gid"].ToString());
                    objModel.statecode = Convert.ToString(dt.Rows[i]["state_code"].ToString());
                    objModel.statename = Convert.ToString(dt.Rows[i]["state_name"].ToString());
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

        public GetstateById GetstateById(int State)
        {
            try
            {
                List<GetstateById> objOrgType = new List<GetstateById>();
                GetstateById objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_state", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@state_gid", State);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objModel = new GetstateById()
                {
                    stategid = Convert.ToInt32(dt.Rows[0]["state_gid"].ToString()),
                    statename = Convert.ToString(dt.Rows[0]["state_name"].ToString()),
                    statecode = Convert.ToString(dt.Rows[0]["state_code"].ToString()),
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
            // throw new 
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
            // throw new NotImplementedException();
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
            
        }
        public IEnumerable<GetregionById> GetregionBy_Id(int Region)
        {

            try
            {
                List<GetregionById> objOrgType = new List<GetregionById>();
                GetregionById objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_state", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@state_gid", Region);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objModel = new GetregionById();

                        objModel.regiongid = Convert.ToInt32(dt.Rows[0]["region_gid"].ToString());
                        objModel .regionname = Convert.ToString(dt.Rows[0]["region_name"].ToString());
                            // regioncode = Convert.ToString(dt.Rows[0]["region_code"].ToString()),
                        objOrgType.Add(objModel);
                    }
                }
                return objOrgType;


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
        public IEnumerable<GetregionById> GetcountryBy_Id(int Region)
        {

            try
            {
                List<GetregionById> objOrgType = new List<GetregionById>();
                GetregionById objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_state", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@state_gid", Region);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objModel = new GetregionById();

                        objModel.regiongid = Convert.ToInt32(dt.Rows[0]["country_gid"].ToString());
                        objModel.regionname = Convert.ToString(dt.Rows[0]["country_name"].ToString());
                        // regioncode = Convert.ToString(dt.Rows[0]["region_code"].ToString()),
                        objOrgType.Add(objModel);
                    }
                }
                return objOrgType;


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
                    objModel.countryname= Convert.ToString(dt.Rows[i]["country_name"].ToString());
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
                objModel = new GetcountryById();
                if (dt.Rows.Count > 0)
                {
                    objModel = new GetcountryById()
                    {
                        countrygid = Convert.ToInt32(dt.Rows[0]["country_gid"].ToString()),
                        countryname = Convert.ToString(dt.Rows[0]["country_name"].ToString()),
                        countrycode = Convert.ToString(dt.Rows[0]["country_code"].ToString()),
                    };
                   
                }
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
            // throw new 
            //throw new NotImplementedException();
        }

        public IEnumerable<Getcityclass> Getcityclass()
        {
            try
            {
                List<Getcityclass> objcityclasstype = new List<Getcityclass>();
                Getcityclass objModel;
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
                    objModel = new Getcityclass();
                    objModel.cityclassgid= Convert.ToInt32(dt.Rows[i]["cityclass_gid"].ToString());
                    objModel.cityclasscode = Convert.ToString(dt.Rows[i]["cityclass_code"].ToString());
                    objModel.cityclassname= Convert.ToString(dt.Rows[i]["cityclass_name"].ToString());
                    objcityclasstype.Add(objModel);
                }

                return objcityclasstype;
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

        public GetcityclassById GetcityclassById(int Cityclass)
        {
            try
            {
                List<GetcityclassById> objOrgType = new List<GetcityclassById>();
                GetcityclassById objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_cityclass", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", Cityclass);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objModel = new GetcityclassById()
                {
                    cityclassgid = Convert.ToInt32(dt.Rows[0]["cityclass_gid"].ToString()),
                    cityclassname = Convert.ToString(dt.Rows[0]["cityclass_name"].ToString()),
                    cityclasscode = Convert.ToString(dt.Rows[0]["cityclass_code"].ToString()),
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

        public IEnumerable<Gettier> Gettier()
        {
            try
            {
                List<Gettier> objtier = new List<Gettier>();
                Gettier objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tier", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new Gettier();
                    objModel.tiergid= Convert.ToInt32(dt.Rows[i]["tier_gid"].ToString());
                    objModel.tiercode= Convert.ToString(dt.Rows[i]["tier_code"].ToString());
                    objModel.tiername= Convert.ToString(dt.Rows[i]["tier_name"].ToString());
                    objtier.Add(objModel);
                }

                return objtier;
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

        public GettierById GettierById(int tier)
        {
            try
            {
                List<GettierById> objOrgType = new List<GettierById>();
                GettierById objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tier", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", tier);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objModel = new GettierById()
                {
                    tiergid = Convert.ToInt32(dt.Rows[0]["tier_gid"].ToString()),
                    tiername = Convert.ToString(dt.Rows[0]["tier_name"].ToString()),
                    tiercode = Convert.ToString(dt.Rows[0]["tier_code"].ToString()),
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
        
        


        string Iiem_mst_city.UpdateCity(iem_mst_city City)
        {
            string result = string.Empty;
            try
            {
                CommonIUD comm = new CommonIUD();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_city", con);
                GetCon();
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.AddWithValue("@gid", City.city_gid);
                cmd.Parameters.AddWithValue("@city_code", City.city_code);
                cmd.Parameters.AddWithValue("@citypincode", City.city_pincode);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Already Exist")
                {
                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes = new string[,]
	                  {
                        {"city_code",City.city_code},
                        {"city_name",City.city_name.ToString() },
	                    {"city_pincode", City.city_pincode.ToString ()},
	                    {"city_state_gid", City.selectedstate_gid.ToString ()},
                        {"city_state_code", City.state_code.ToString ()},
                        {"city_state_name", City.state_name.ToString ()},
                        {"city_region_gid", City.selectedregion_gid.ToString ()},
                         {"city_region_name", City.region_name},
                        {"city_country_gid", City.selectedcountry_gid.ToString ()},
                        {"city_country_code", City.country_code},
                        {"city_country_name", City.country_name.ToString ()},
                        {"city_cityclass_gid", City.selectedcityclass_gid.ToString ()},
                        {"city_cityclass_code", City.cityclass_code},
                        {"city_tier_gid", City.selectedtier_gid.ToString ()},
                        {"city_tier_code", City.tier_code.ToString ()},
                        {"city_update_by", City.city_update_by.ToString ()},
                        {"city_update_date", delecomm.GetCurrentDate()} 
	                  };
                    string[,] whrcol = new string[,]
	                 {
                          {"city_gid", City.city_gid.ToString ()}
            
                     };
                    string tblname = "iem_mst_tcity";

                    string updcomm = delecomm.UpdateCommon(codes, whrcol, tblname);

                }
                else
                {
                    return "No Record to updated !";
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return "success";
            // throw new NotImplementedException();
        }
    }
}