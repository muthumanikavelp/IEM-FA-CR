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

    public class IEM_MST_CURRENCY : Iiem_mst_currency
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

        public IEnumerable<iem_mst_currency> getcurrency()
        {
            try
            {
                List<iem_mst_currency> objOrgType = new List<iem_mst_currency>();
                iem_mst_currency objModel;
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
                    objModel = new iem_mst_currency();
                    objModel.currency_gid = Convert.ToInt32(dt.Rows[i]["currency_gid"].ToString());
                    objModel.currency_code = Convert.ToString(dt.Rows[i]["currency_code"].ToString());
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

        public IEnumerable<iem_mst_currency> getcurrency(string filter_code, string filter_name)
        {
            try
            {
                List<iem_mst_currency> objOrgType = new List<iem_mst_currency>();
                iem_mst_currency objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_currency", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.Parameters.AddWithValue("@currency_code", filter_code);
                cmd.Parameters.AddWithValue("@currency_name", filter_name);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_currency();
                    objModel.currency_gid = Convert.ToInt32(dt.Rows[i]["currency_gid"].ToString());
                    objModel.currency_code = Convert.ToString(dt.Rows[i]["currency_code"].ToString());
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

        public iem_mst_currency GetCurrencyById(int currencyId)
        {
            try
            {
                List<iem_mst_currency> objOrgType = new List<iem_mst_currency>();
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_currency", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", currencyId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new iem_mst_currency()
                {
                    currency_gid = Convert.ToInt32(dt.Rows[0]["currency_gid"].ToString()),
                    currency_name = Convert.ToString(dt.Rows[0]["currency_name"].ToString()),
                    currency_code = Convert.ToString(dt.Rows[0]["currency_code"].ToString()),
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

        public string InsertCurrency(iem_mst_currency Currency)
        {
            string result;

            try
            {
                SqlCommand cmd = new SqlCommand("pr_iem_mst_currency", con);
                GetCon();
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.AddWithValue("@currency_code", Currency.currency_code);
                cmd.Parameters.AddWithValue("@currency_name", Currency.currency_name);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();

                if (res1 == "Not There")
                {
                    CommonIUD comm = new CommonIUD();
                    string[,] codes = new string[,]            
	                {
                        {"currency_code",Currency.currency_code },
                        {"currency_name",Currency.currency_name},
                        {"currency_insert_by",Convert.ToString (Currency.currency_insert_by)},
                        {"currency_insert_date",comm.GetCurrentDate()}
                    };
                    string insertcommend = comm.InsertCommon(codes, "iem_mst_tcurrency");
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



        public string UpdateCurrency(iem_mst_currency Currency)
        {
            string result;

            try
            {
                GetCon();

                SqlCommand cmd = new SqlCommand("pr_iem_mst_currency", con);
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.AddWithValue("@gid", Currency.currency_gid);
                cmd.Parameters.AddWithValue("@currency_code", Currency.currency_code);
                cmd.Parameters.AddWithValue("@currency_name", Currency.currency_name);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                CommonIUD delecomm = new CommonIUD();
                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
	                {
                        {"currency_code", Currency.currency_code},
                        {"currency_name", Currency.currency_name},
                        {"currency_update_by", Currency.currency_update_by.ToString ()},
                        {"currency_update_date", delecomm.GetCurrentDate()}
	                };
                    string[,] whrcol = new string[,]
	                {
                        {"currency_gid", Currency.currency_gid.ToString ()}
                    };


                    string updcomm = delecomm.UpdateCommon(codes, whrcol, "iem_mst_tcurrency");
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


        //public string DeleteCurrency(int currencyId)
        //{
        //    try
        //    {
        //        string[,] codes = new string[,]
        //        {
        //             {"currency_isremoved", "Y"}
        //        };

        //        string[,] whrcol = new string[,]
        //        {
        //            {"currency_gid", currencyId.ToString()},
        //             {"currency_isremoved", "N"}
        //        };

        //        CommonIUD delecomm = new CommonIUD();
        //        string deletetbl = delecomm.DeleteCommon(codes, whrcol, "iem_mst_tcurrency");
        //        return deletetbl;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    // throw new NotImplementedException();
        //}
        public string DeleteCurrency(int currencyId)
        {
            string result;
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;

            try
            {

                CommonIUD comm1 = new CommonIUD();
                SqlCommand cmd1 = new SqlCommand("pr_iem_mst_currency", con);
                GetCon();
                cmd1.Parameters.AddWithValue("@action", "Delete_country");
                cmd1.Parameters.AddWithValue("@gid", currencyId);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd1.ExecuteNonQuery();
                string res2 = cmd1.Parameters["@res"].Value.ToString();
                if (res2 == "Not There")
                {
                    string[,] codes = new string[,]
	                {
                         {"currency_isremoved", "Y"}
	            
                         };
                    string[,] whrcol = new string[,]
	                    {
                          {"currency_gid", currencyId.ToString ()}
                        };
                    string tblname = "iem_mst_tcurrency";


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


