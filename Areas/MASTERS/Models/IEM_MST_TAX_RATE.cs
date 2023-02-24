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
    public class IEM_MST_TAX_RATE : Iiem_mst_Tax_rate
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
        public IEnumerable<iem_mst_tax_rate> gettaxrate()
        {
            try
            {
                List<iem_mst_tax_rate> objOrgType = new List<iem_mst_tax_rate>();
                iem_mst_tax_rate objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tax_rate", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_tax_rate();
                    objModel.taxrate_gid = Convert.ToInt32(dt.Rows[i]["taxrate_gid"].ToString());
                    objModel.taxrate_tax_gid = Convert.ToString(dt.Rows[i]["taxrate_tax_gid"].ToString());
                    objModel.taxrate_taxsubtype_flag = Convert.ToString(dt.Rows[i]["taxrate_taxsubtype_flag"].ToString());
                    objModel.taxrate_taxsubtype_gid = Convert.ToString(dt.Rows[i]["taxrate_taxsubtype_gid"].ToString());
                    objModel.taxrate_rate = Convert.ToString(dt.Rows[i]["taxrate_rate"].ToString());

                    objModel.taxrate_change_flag = Convert.ToString(dt.Rows[i]["taxrate_change_flag"].ToString());
                    objModel.taxrate_period_from = Convert.ToString(dt.Rows[i]["taxrate_period_from"].ToString());
                    objModel.taxrate_period_to = Convert.ToString(dt.Rows[i]["taxrate_period_to"].ToString());
                    objModel.taxrate_active_flag = Convert.ToString(dt.Rows[i]["taxrate_active_flag"].ToString());
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
        public IEnumerable<iem_mst_tax_rate> gettaxrateid(string filter_name, string filter_name1 )
        {
            try
            {
                List<iem_mst_tax_rate> objOrgType = new List<iem_mst_tax_rate>();
                iem_mst_tax_rate objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tax_rate", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.Parameters.AddWithValue("@taxrate_tax_gid", filter_name);
                cmd.Parameters.AddWithValue("@taxrate_taxsubtype_gid", filter_name1);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_tax_rate();
                    objModel.taxrate_gid = Convert.ToInt32(dt.Rows[i]["taxrate_gid"].ToString());
                    objModel.taxrate_tax_gid = Convert.ToString(dt.Rows[i]["taxrate_tax_gid"].ToString());
                    objModel.taxrate_taxsubtype_flag = Convert.ToString(dt.Rows[i]["taxrate_taxsubtype_flag"].ToString());
                    objModel.taxrate_taxsubtype_gid = Convert.ToString(dt.Rows[i]["taxrate_taxsubtype_gid"].ToString());
                    objModel.taxrate_rate = Convert.ToString(dt.Rows[i]["taxrate_rate"].ToString());

                    objModel.taxrate_change_flag = Convert.ToString(dt.Rows[i]["taxrate_change_flag"].ToString());
                    objModel.taxrate_period_from = Convert.ToString(dt.Rows[i]["taxrate_period_from"].ToString());
                    objModel.taxrate_period_to = Convert.ToString(dt.Rows[i]["taxrate_period_to"].ToString());
                    objModel.taxrate_active_flag = Convert.ToString(dt.Rows[i]["taxrate_active_flag"].ToString());
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
        public iem_mst_tax_rate gettaxratebyid(int taxId)
        {
            try
            {
                List<iem_mst_tax_rate> objOrgType = new List<iem_mst_tax_rate>();
                iem_mst_tax_rate objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tax_rate", con);
                cmd.Parameters.AddWithValue("@action", "selectby_Id");
                cmd.Parameters.AddWithValue("@gid", taxId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objModel = new iem_mst_tax_rate()
                {
                    taxrate_gid = Convert.ToInt32(dt.Rows[0]["taxrate_gid"].ToString()),
                    taxrate_tax_gid = Convert.ToString(dt.Rows[0]["taxnameid"].ToString()),
                    taxrate_taxsubtype_flag = Convert.ToString(dt.Rows[0]["taxrate_taxsubtype_flag"].ToString()),
                    taxrate_taxsubtype_gid = Convert.ToString(dt.Rows[0]["taxsubnameid"].ToString()),
                    taxrate_rate = Convert.ToString(dt.Rows[0]["taxrate_rate"].ToString()),

                    taxrate_change_flag = Convert.ToString(dt.Rows[0]["taxrate_change_flag"].ToString()),
                    taxrate_period_from = Convert.ToString(dt.Rows[0]["taxrate_period_from"].ToString()),
                    taxrate_period_to = Convert.ToString(dt.Rows[0]["taxrate_period_to"].ToString()),
                    taxrate_active_flag = Convert.ToString(dt.Rows[0]["taxrate_active_flag"].ToString()),
                };
                return objModel;
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
        public IEnumerable<Gettaxratetaxname> Gettaxname()
        {
            try
            {
                List<Gettaxratetaxname> objCountrytype = new List<Gettaxratetaxname>();
                Gettaxratetaxname objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tax", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new Gettaxratetaxname();
                    objModel.tax_gid = Convert.ToInt32(dt.Rows[i]["tax_gid"].ToString());
                    objModel.tax_name = Convert.ToString(dt.Rows[i]["tax_name"].ToString());

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
        public IEnumerable<Gettaxratetaxsubname> Gettaxsubname()
        {
            try
            {
                List<Gettaxratetaxsubname> objCountrytype = new List<Gettaxratetaxsubname>();
                Gettaxratetaxsubname objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tax_sub_type", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new Gettaxratetaxsubname();
                    objModel.taxsub_gid = Convert.ToInt32(dt.Rows[i]["taxsubtype_gid"].ToString());
                    objModel.taxsub_name = Convert.ToString(dt.Rows[i]["taxsubtype_name"].ToString());

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
        public IEnumerable<Gettaxratetaxsubname> Gettaxsubname_id(int id)
        {
            try
            {
                List<Gettaxratetaxsubname> objCountrytype = new List<Gettaxratetaxsubname>();
                Gettaxratetaxsubname objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tax_sub_type", con);
                cmd.Parameters.AddWithValue("@action", "selectByIdTax");
                cmd.Parameters.AddWithValue("@gid", id);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new Gettaxratetaxsubname();
                    objModel.taxsub_gid = Convert.ToInt32(dt.Rows[i]["taxsubtype_gid"].ToString());
                    objModel.taxsub_name = Convert.ToString(dt.Rows[i]["taxsubtype_name"].ToString());

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
        public string Inserttaxrate(iem_mst_tax_rate taxcat)
        {
            string result;
            try
            {
                //string[] date1 = taxcat.taxrate_period_from.Split('/');
                //string curdate1 = date1[1].ToString() + "-" + date1[0].ToString() + "-" + date1[2].ToString();
                //string[] date2 = taxcat.taxrate_period_to.Split('/');
                //string curdate2 = date2[1].ToString() + "-" + date2[0].ToString() + "-" + date2[2].ToString();
                CommonIUD comm = new CommonIUD();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tax_rate", con);
                GetCon();
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.AddWithValue("@taxrate_tax_gid", taxcat.taxrate_tax_gid);
                cmd.Parameters.AddWithValue("@taxrate_periodfrom", cmn.convertoDateTimeString(taxcat.taxrate_period_from));
                cmd.Parameters.AddWithValue("@taxrate_periodto", cmn.convertoDateTimeString(taxcat.taxrate_period_to));
                cmd.Parameters.AddWithValue("@taxrate_taxsubtype_gid", taxcat.taxrate_taxsubtype_gid);
              //  cmd.Parameters.AddWithValue("@taxrate_rate", taxcat.taxrate_rate);
               
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
	               {
                        {"taxrate_tax_gid",taxcat.taxrate_tax_gid.ToString()},
                        {"taxrate_taxsubtype_flag",taxcat.taxrate_taxsubtype_flag.ToString() },                      
	                    {"taxrate_taxsubtype_gid", taxcat.taxrate_taxsubtype_gid.ToString()},                        
	                    {"taxrate_rate", taxcat.taxrate_rate.ToString ()},  
                        {"taxrate_change_flag",taxcat.taxrate_change_flag},
                        {"taxrate_period_from",cmn.convertoDateTimeString(taxcat.taxrate_period_from) },                      
	                    {"taxrate_period_to", cmn.convertoDateTimeString(taxcat.taxrate_period_to)},                        
	                    {"taxrate_active_flag", taxcat.taxrate_active_flag.ToString ()},   
                        {"taxrate_insert_by", taxcat.taxrate_insert_by.ToString ()},
                        {"taxrate_insert_date","sysdatetime()"}                        
                  };
                    string tname = "iem_mst_ttaxrate";
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

        }
        public string Updattaxrate(iem_mst_tax_rate taxcat)
        {
            string result;
            try
            { 
                //string curdate1 = "";
                //string curdate2 = "";
                //if (taxcat.taxrate_period_from.Contains("/") || taxcat.taxrate_period_to.Contains("/"))
                //{
                //    string[] date1 = taxcat.taxrate_period_from.Split('/');
                //    curdate1 = date1[1].ToString() + "-" + date1[0].ToString() + "-" + date1[2].ToString();
                //    string[] date2 = taxcat.taxrate_period_to.Split('/');
                //    curdate2 = date2[1].ToString() + "-" + date2[0].ToString() + "-" + date2[2].ToString();
                //}
                //else
                //{
                //    curdate1 = taxcat.taxrate_period_from;
                //    curdate2 = taxcat.taxrate_period_to;
                //}
                
                

                CommonIUD comm = new CommonIUD();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tax_rate", con);
                GetCon();
                
                cmd.Parameters.AddWithValue("@action", "update");
                cmd.Parameters.AddWithValue("@taxrate_tax_gid", taxcat.taxrate_tax_gid);
                cmd.Parameters.AddWithValue("@taxrate_periodfrom", cmn.convertoDateTimeString(taxcat.taxrate_period_from));
                cmd.Parameters.AddWithValue("@taxrate_periodto", cmn.convertoDateTimeString(taxcat.taxrate_period_to));
                cmd.Parameters.AddWithValue("@taxrate_taxsubtype_gid", taxcat.taxrate_taxsubtype_gid);
                cmd.Parameters.AddWithValue("@taxrate_ratedecimal", taxcat.taxrate_rate);
               
                //cmd.Parameters.AddWithValue("@taxrate_rate", taxcat.taxrate_rate);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    //string[] date1 = taxcat.taxrate_period_from.Split('-');
                    //string curdate1 = date1[1].ToString() + "-" + date1[0].ToString() + "-" + date1[2].ToString();
                    //string[] date2 = taxcat.taxrate_period_to.Split('-');
                    //string curdate2 = date2[1].ToString() + "-" + date2[0].ToString() + "-" + date2[2].ToString();

                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes = new string[,]
                 
                   {
                           {"taxrate_tax_gid",taxcat.taxrate_tax_gid.ToString()},
                        {"taxrate_taxsubtype_flag",taxcat.taxrate_taxsubtype_flag.ToString() },                      
                        {"taxrate_taxsubtype_gid", taxcat.taxrate_taxsubtype_gid.ToString()},                        
                        {"taxrate_rate", taxcat.taxrate_rate.ToString ()},  
                        {"taxrate_change_flag",taxcat.taxrate_change_flag},
                        {"taxrate_period_from", cmn.convertoDateTimeString(taxcat.taxrate_period_from)},                      
                        {"taxrate_period_to", cmn.convertoDateTimeString(taxcat.taxrate_period_to)},                        
                        {"taxrate_active_flag", taxcat.taxrate_active_flag.ToString ()},   
                        {"taxrate_insert_by", taxcat.taxrate_insert_by.ToString ()},
                        {"taxrate_insert_date","sysdatetime()"}                     
                  };
                    string[,] whrcol = new string[,]
                     {
                          {"taxrate_gid", taxcat.taxrate_gid.ToString()}
            
                     };
                    string tblname = "iem_mst_ttaxrate";

                    string updcomm = delecomm.UpdateCommon(codes, whrcol, tblname);
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
        //public string Updattaxrate(iem_mst_tax_rate taxcat)
        //{
        //    string result;
        //    try
        //    {
        //        string[] date1 = taxcat.taxrate_period_from.Split('-');
        //        string curdate1 = date1[1].ToString() + "-" + date1[0].ToString() + "-" + date1[2].ToString();
        //        string[] date2 = taxcat.taxrate_period_to.Split('-');
        //        string curdate2 = date2[1].ToString() + "-" + date2[0].ToString() + "-" + date2[2].ToString();
        //        CommonIUD comm = new CommonIUD();
        //        SqlCommand cmd = new SqlCommand("pr_iem_mst_tax_rate", con);
        //        GetCon();
        //        cmd.Parameters.AddWithValue("@action", "update");
        //        cmd.Parameters.AddWithValue("@taxrate_tax_gid", taxcat.taxrate_tax_gid);
        //        cmd.Parameters.AddWithValue("@taxrate_periodfrom", curdate1);
        //        cmd.Parameters.AddWithValue("@taxrate_periodto", curdate2);
        //        cmd.Parameters.AddWithValue("@taxrate_rate", taxcat.taxrate_rate);
        //        cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.ExecuteNonQuery();
        //        string res1 = cmd.Parameters["@res"].Value.ToString();
        //        if (res1 == "Not There")
        //        {
        //            string[,] codes = new string[,]
        //           {
        //                {"taxrate_tax_gid",taxcat.taxrate_tax_gid.ToString()},
        //                {"taxrate_taxsubtype_flag",taxcat.taxrate_taxsubtype_flag.ToString() },                      
        //                {"taxrate_taxsubtype_gid", taxcat.taxrate_taxsubtype_gid.ToString()},                        
        //                {"taxrate_rate", taxcat.taxrate_rate.ToString ()},  
        //                {"taxrate_change_flag",taxcat.taxrate_change_flag},
        //                {"taxrate_period_from",curdate1 },                      
        //                {"taxrate_period_to", curdate2},                        
        //                {"taxrate_active_flag", taxcat.taxrate_active_flag.ToString ()},   
        //                {"taxrate_insert_by", taxcat.taxrate_insert_by.ToString ()},
        //                {"taxrate_insert_date","sysdatetime()"}                        
        //          };
        //            string tname = "iem_mst_ttaxrate";
        //            string insertcommend = comm.InsertCommon(codes, tname);
        //            result = "success";
        //        }
        //        else
        //        {
        //            result = "Duplicate record !";
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        result = ex.Message;
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return result;

        //}
        public void Deletetaxrate(int taxId)
        {
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {

                string[,] codes = new string[,]
	       {
                 {"taxrate_isremoved", "Y"}
	            
           };
                string[,] whrcol = new string[,]
	        {
                {"taxrate_gid", taxId.ToString ()}
            };
                string tblname = "iem_mst_ttaxrate";


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

        }
    }
}