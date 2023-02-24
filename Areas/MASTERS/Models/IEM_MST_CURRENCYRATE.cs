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
    public class IEM_MST_CURRENCYRATE : Iiem_mst_currencyrate
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
        public IEnumerable<iem_mst_currencyrate> getcurrencyRate()
        {
            try
            {
                List<iem_mst_currencyrate> objOrgType = new List<iem_mst_currencyrate>();
                iem_mst_currencyrate objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_currencyrate", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_currencyrate();
                    objModel.currencyrate_gid = Convert.ToInt32(dt.Rows[i]["currencyrate_gid"].ToString());
                    objModel.currency_name = Convert.ToString(dt.Rows[i]["Currency Name"].ToString());
                    objModel.currencyrate_value = Convert.ToDecimal(dt.Rows[i]["Currency Rate"].ToString());
                    objModel.currencyrate_period_from = Convert.ToString(dt.Rows[i]["Currency Period From"].ToString());
                    objModel.currencyrate_period_to = Convert.ToString(dt.Rows[i]["Currency Period To"].ToString());
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

        public IEnumerable<iem_mst_currencyrate> getcurrencyRate(string currencyname, string periodfrom, string periodto)
        {
            try
            {
                List<iem_mst_currencyrate> objOrgType = new List<iem_mst_currencyrate>();
                iem_mst_currencyrate objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_currencyrate", con);
                cmd.Parameters.AddWithValue("@action", "CurrencyRateSearch");
                cmd.Parameters.AddWithValue("@currency_name",currencyname);
                if (periodfrom!="")
                {
                    cmd.Parameters.AddWithValue("@currencyrate_period_from", com.convertoDateTimeString(periodfrom));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@currencyrate_period_from", periodfrom);
                }
                if (periodto != "")
                {
                    cmd.Parameters.AddWithValue("@currencyrate_period_to", com.convertoDateTimeString(periodto));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@currencyrate_period_to", periodto);
                }                    
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_currencyrate();
                    objModel.currencyrate_gid = Convert.ToInt32(dt.Rows[i]["currencyrate_gid"].ToString());
                    objModel.currency_name = Convert.ToString(dt.Rows[i]["Currency Name"].ToString());
                    objModel.currencyrate_value = Convert.ToDecimal(dt.Rows[i]["Currency Rate"].ToString());
                    objModel.currencyrate_period_from = Convert.ToString(dt.Rows[i]["Currency Period From"].ToString());
                    objModel.currencyrate_period_to = Convert.ToString(dt.Rows[i]["Currency Period To"].ToString());
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

        public iem_mst_currencyrate GetCurrencyRateById(int currencyId)
        {
            try
            {
                List<iem_mst_currencyrate> objOrgType = new List<iem_mst_currencyrate>();
                iem_mst_currencyrate objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_currencyrate", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", currencyId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new iem_mst_currencyrate()
                {
                    currencyrate_gid = Convert.ToInt32(dt.Rows[0]["currencyrate_gid"].ToString()),
                    currency_name = Convert.ToString(dt.Rows[0]["Currency Name"].ToString()),
                    currencyrate_value = Convert.ToDecimal(dt.Rows[0]["Currency Rate"].ToString()),
                    currencyrate_period_from = Convert.ToString(dt.Rows[0]["Currency Period From"].ToString()),
                    currencyrate_period_to = Convert.ToString(dt.Rows[0]["Currency Period To"].ToString()),
                    selectedcurrency_gid = Convert.ToInt32(dt.Rows[0]["currencyrate_currency_gid"])
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

        public string InsertCurrencyRate(iem_mst_currencyrate Currency)
        {
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_currencyrate", con);
                cmd.Parameters.AddWithValue("@action", "InsertExist");
                cmd.Parameters.AddWithValue("@currencyrate_period_from", com.convertoDateTimeString(Currency.currencyrate_period_from));
                cmd.Parameters.AddWithValue("@currencyrate_period_to", com.convertoDateTimeString(Currency.currencyrate_period_to));
                cmd.Parameters.AddWithValue("@gid", Currency.currency_gid);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["@res"].Value.ToString();

                if (result == "Not There")
                {
                    CommonIUD comm = new CommonIUD();
                    string[,] codes = new string[,]            
	                {
                        {"currencyrate_value",Currency.currencyrate_value.ToString() },
                        {"currencyrate_currency_gid",Currency.currency_gid.ToString ()},
                        {"currencyrate_period_from",com.convertoDateTimeString(Currency.currencyrate_period_from)},
                        {"currencyrate_period_to", com.convertoDateTimeString(Currency.currencyrate_period_to)},
                        {"currencyrate_insert_by",com.GetLoginUserGid().ToString ()},
                        {"currencyrate_insert_date",comm.GetCurrentDate()}
                    };
                    string tname = "iem_mst_tcurrencyrate";
                    string insertcommend = comm.InsertCommon(codes, tname);
                }
                else
                {
                    return "Duplicate";
                }
                //else if(result=="Already Exist")
                //{
                //    //update
                //    CommonIUD delecomm = new CommonIUD();
                //    string[,] codes = new string[,]
                //      {
                //          {"currencyrate_isremoved", "Y"}

                //      };
                //    string[,] whrcol = new string[,]
                //     {
                //          {"currencyrate_currency_gid", Currency.currency_gid.ToString ()},

                //     };

                //    string updcomm = delecomm.UpdateCommon(codes, whrcol, "iem_mst_tcurrencyrate");



                //    //insert
                //    CommonIUD comm = new CommonIUD();
                //    string[,] codes1 = new string[,]            
                //    {
                //        {"currencyrate_value",Convert.ToString (Currency.currencyrate_value) },
                //        {"currencyrate_currency_gid",Currency.currency_gid.ToString ()},
                //        {"currencyrate_period_from",com.convertoDateTimeString(Currency.currencyrate_period_from)},
                //        {"currencyrate_period_to", com.convertoDateTimeString(Currency.currencyrate_period_to)},
                //        {"currencyrate_insert_by",com.GetLoginUserGid().ToString ()},
                //        {"currencyrate_insert_date",comm.GetCurrentDate()}
                //    };
                //    string tname1 = "iem_mst_tcurrencyrate";
                //    string insertcommend = comm.InsertCommon(codes1, tname1);
                //}

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

        public string DeleteCurrencyRate(int currencyId)
        {
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {

                string[,] codes = new string[,]
	       {
                 {"currencyrate_isremoved", "Y"}
	            
           };
                string[,] whrcol = new string[,]
	        {
                {"currencyrate_gid", currencyId.ToString ()}
            };
                string tblname = "iem_mst_tcurrencyrate";

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

        public string UpdateCurrencyRate(iem_mst_currencyrate Currency)
        {
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_currencyrate", con);
                cmd.Parameters.AddWithValue("@action", "UpdateExist");
                cmd.Parameters.AddWithValue("@gid", Currency.currencyrate_gid);
                cmd.Parameters.AddWithValue("@currencyrate_gid", Currency.currency_gid);
                cmd.Parameters.AddWithValue("@currencyrate_period_from",com.convertoDateTimeString(Currency.currencyrate_period_from) );
                cmd.Parameters.AddWithValue("@currencyrate_period_to", com.convertoDateTimeString(Currency.currencyrate_period_to ));
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["@res"].Value.ToString();
                if (result == "Not There")
                {
                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes = new string[,]
	                  {
                        {"currencyrate_value",Convert.ToString (Currency.currencyrate_value) },
                        {"currencyrate_currency_gid",Currency.currency_gid.ToString ()},
                        {"currencyrate_period_from",com.convertoDateTimeString(Currency.currencyrate_period_from)},
                        {"currencyrate_period_to", com.convertoDateTimeString(Currency.currencyrate_period_to)},
                        {"currencyrate_update_by",com.GetLoginUserGid().ToString ()},
                        {"currencyrate_update_date",delecomm.GetCurrentDate()}
	                  };
                    string[,] whrcol = new string[,]
	                 {
                          {"currencyrate_gid", Currency.currencyrate_gid.ToString()},                         
                     };

                    string updcomm = delecomm.UpdateCommon(codes, whrcol, "iem_mst_tcurrencyrate");
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


        public IEnumerable<Getcurrency> Getcurrency()
        {

            try
            {
                List<Getcurrency> objCountrytype = new List<Getcurrency>();
                Getcurrency objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_currencyrate", con);
                cmd.Parameters.AddWithValue("@action", "SelectCurrency");
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
    }
}