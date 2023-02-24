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
    public class IEM_MST_COUNTRY : Iiem_mst_country
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

        public IEnumerable<iem_mst_country> getcountry()
        {
          //  throw new NotImplementedException();

            try
            {
                List<iem_mst_country> objOrgType = new List<iem_mst_country>();
                iem_mst_country objModel;
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
                    objModel = new iem_mst_country();
                    objModel.country_gid = Convert.ToInt32(dt.Rows[i]["country_gid"].ToString());
                    objModel.country_code = Convert.ToString(dt.Rows[i]["country_code"].ToString());
                    objModel.country_name = Convert.ToString(dt.Rows[i]["country_name"].ToString());
                    objModel.currency_name = Convert.ToString(dt.Rows[i]["currency_name"].ToString());
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
        public IEnumerable<iem_mst_country> getcountry1(string country_code, string country_name, string currency_name)
        {
            //  throw new NotImplementedException();

            try
            {
                List<iem_mst_country> objOrgType = new List<iem_mst_country>();
                iem_mst_country objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_country", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.Parameters.AddWithValue("@country_code", country_code);
                cmd.Parameters.AddWithValue("@country_name", country_name);
                cmd.Parameters.AddWithValue("@currency_name", currency_name);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_country();
                    objModel.country_gid = Convert.ToInt32(dt.Rows[i]["country_gid"].ToString());
                    objModel.country_code = Convert.ToString(dt.Rows[i]["country_code"].ToString());
                    objModel.country_name = Convert.ToString(dt.Rows[i]["country_name"].ToString());
                    objModel.currency_name = Convert.ToString(dt.Rows[i]["currency_name"].ToString());
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
        public iem_mst_country GetCountryId(int CityId)
        {

            try
            {
                List<iem_mst_country> objOrgType = new List<iem_mst_country>();
                iem_mst_country objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_country", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", CityId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objModel = new iem_mst_country()
                {
                    country_gid = Convert.ToInt32(dt.Rows[0]["country_gid"].ToString()),
                    country_code = Convert.ToString(dt.Rows[0]["country_code"].ToString()),
                    country_name = Convert.ToString(dt.Rows[0]["country_name"].ToString()),
                    selectedcurrency_gid = Convert.ToInt32(dt.Rows[0]["country_currency_gid"].ToString()),
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
      

        public string Insertcountry(iem_mst_country Classifications)
        {
            string result;
           // throw new NotImplementedException();
            try
            {
                CommonIUD comm = new CommonIUD();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_country", con);
                GetCon();
                cmd.Parameters.AddWithValue("@action", "InExist");
                cmd.Parameters.AddWithValue("@gid", "0");
                cmd.Parameters.AddWithValue("@country_code", Classifications.country_code);
                cmd.Parameters.AddWithValue("@country_name", Classifications.country_name);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
	               {
                        {"country_code",Classifications.country_code},
                        {"country_name",Classifications.country_name.ToString() },
	                    {"country_currency_gid", Classifications.currency_gid.ToString ()},
	                    {"country_currency_code", Classifications.currency_code},
                        {"country_insert_by", Classifications.country_insert_by.ToString ()},
                        {"country_insert_date", comm.GetCurrentDate()} 

                  };
                    string tname = "iem_mst_tcountry";
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
           
        }

       

       

        public IEnumerable<Getcurrency> GetCurrency()
        {
          //  throw new NotImplementedException();
            try
            {
                List<Getcurrency> objOrgType = new List<Getcurrency>();
                Getcurrency objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_currency", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new Getcurrency();
                    objModel.Currencygid = Convert.ToInt32(dt.Rows[i]["currency_gid"].ToString());
                    objModel.Currencycode = Convert.ToString(dt.Rows[i]["currency_code"].ToString());
                    objModel.Currencyname = Convert.ToString(dt.Rows[i]["currency_name"].ToString());
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

        public GetcurrencyById GetCurrencyById(int Curency)
        {
          //  throw new NotImplementedException();
            try
            {
                List<GetcurrencyById> objOrgType = new List<GetcurrencyById>();
                GetcurrencyById objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_currency", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", Curency);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objModel = new GetcurrencyById()
                {
                    Currencygid = Convert.ToInt32(dt.Rows[0]["currency_gid"].ToString()),
                    Currencyname= Convert.ToString(dt.Rows[0]["currency_name"].ToString()),
                    Currencycode = Convert.ToString(dt.Rows[0]["currency_code"].ToString()),
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


       

        public string UpdateCountry(iem_mst_country Classifications)
        {

            try
            {
                CommonIUD comm = new CommonIUD();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_country", con);
                GetCon();
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.AddWithValue("@gid", Classifications.country_gid);
                cmd.Parameters.AddWithValue("@country_code", Classifications.country_code);
                cmd.Parameters.AddWithValue("@country_name", Classifications.country_name);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes = new string[,]
	                  {
                          {"country_code", Classifications.country_code},
                          {"country_name", Classifications.country_name},
                          {"country_currency_gid", Classifications.selectedcurrency_gid.ToString ()},
                          {"country_update_by", Classifications.country_update_by.ToString ()},
                          {"country_update_date", delecomm.GetCurrentDate()}
	                  };
                    string[,] whrcol = new string[,]
	                 {
                          {"country_gid", Classifications.country_gid.ToString ()}
            
                     };
                    string tblname = "iem_mst_tcountry";

                    string updcomm = delecomm.UpdateCommon(codes, whrcol, tblname);
                }
                else
                {
                    return "Duplicate Record !";
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
            return "success";
            // throw new NotImplementedException();
        }


        //public string Deletecountry(int CityId)
        //{
        //    CommonIUD delecomm = new CommonIUD();
        //    string col_value = string.Empty;
        //    string col_temp = string.Empty;
        //    try
        //    {

        //        string[,] codes = new string[,]
        //   {
        //         {"country_isremoved", "Y"}
	            
        //   };
        //        string[,] whrcol = new string[,]
        //    {
        //        {"country_gid", CityId.ToString ()}
        //    };
        //        string tblname = "iem_mst_tcountry";


        //        string deletetbl = delecomm.DeleteCommon(codes, whrcol, tblname);
        //        return deletetbl;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {

        //    }
            
        //    //throw new NotImplementedException();
        //}
        public string Deletecountry(int CityId)
        {
            string result;
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;

            try
            {

                CommonIUD comm1 = new CommonIUD();
                SqlCommand cmd1 = new SqlCommand("pr_iem_mst_country", con);
                GetCon();
                cmd1.Parameters.AddWithValue("@action", "Delete_state");
                cmd1.Parameters.AddWithValue("@gid", CityId);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd1.ExecuteNonQuery();
                string res2 = cmd1.Parameters["@res"].Value.ToString();
                if (res2 == "Not There")
                {
                    string[,] codes = new string[,]
	                {
                         {"country_isremoved", "Y"}
	            
                         };
                    string[,] whrcol = new string[,]
	                    {
                          {"country_gid", CityId.ToString ()}
                        };
                    string tblname = "iem_mst_tcountry";


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
    }
}