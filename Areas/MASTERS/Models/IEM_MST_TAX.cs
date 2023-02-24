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
    public class IEM_MST_TAX : Iiem_mst_Tax
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
        public IEnumerable<iem_mst_tax> gettax()
        {
            try
            {
                List<iem_mst_tax> objOrgType = new List<iem_mst_tax>();
                iem_mst_tax objModel;
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
                    objModel = new iem_mst_tax();
                    objModel.tax_gid = Convert.ToInt32(dt.Rows[i]["tax_gid"].ToString());
                    objModel.tax_code = Convert.ToString(dt.Rows[i]["tax_code"].ToString());
                    objModel.tax_name = Convert.ToString(dt.Rows[i]["tax_name"].ToString());
                    objModel.tax_parent_flag = Convert.ToString(dt.Rows[i]["tax_parent_flag"].ToString());
                    objModel.tax_parent_name = Convert.ToString(dt.Rows[i]["tax_parent_gid"].ToString());
                    objModel.tax_withhold_flag = Convert.ToString(dt.Rows[i]["tax_withhold_flag"].ToString());
                    objModel.tax_payable_flag = Convert.ToString(dt.Rows[i]["tax_payable_flag"].ToString());
                    objModel.tax_receivable_flag = Convert.ToString(dt.Rows[i]["tax_receivable_flag"].ToString());
                    objModel.tax_inputcredit_flag = Convert.ToString(dt.Rows[i]["tax_inputcredit_flag"].ToString());
                    objModel.tax_reverse_flag = Convert.ToString(dt.Rows[i]["tax_reverse_flag"].ToString());
                    objModel.tax_active = Convert.ToString(dt.Rows[i]["tax_active_flag"].ToString());
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
        public IEnumerable<iem_mst_tax> gettaxid(string filter_name, string filter_name1)
        {
            try
            {
                List<iem_mst_tax> objOrgType = new List<iem_mst_tax>();
                iem_mst_tax objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tax", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.Parameters.AddWithValue("@tax_code", filter_name);
                cmd.Parameters.AddWithValue("@tax_name", filter_name1);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_tax();
                    objModel.tax_gid = Convert.ToInt32(dt.Rows[i]["tax_gid"].ToString());
                    objModel.tax_code = Convert.ToString(dt.Rows[i]["tax_code"].ToString());
                    objModel.tax_name = Convert.ToString(dt.Rows[i]["tax_name"].ToString());
                    objModel.tax_parent_flag = Convert.ToString(dt.Rows[i]["tax_parent_flag"].ToString());
                    objModel.tax_parent_name = Convert.ToString(dt.Rows[i]["tax_parent_gid"].ToString());
                    objModel.tax_withhold_flag = Convert.ToString(dt.Rows[i]["tax_withhold_flag"].ToString());
                    objModel.tax_payable_flag = Convert.ToString(dt.Rows[i]["tax_payable_flag"].ToString());
                    objModel.tax_receivable_flag = Convert.ToString(dt.Rows[i]["tax_receivable_flag"].ToString());
                    objModel.tax_inputcredit_flag = Convert.ToString(dt.Rows[i]["tax_inputcredit_flag"].ToString());
                    objModel.tax_reverse_flag = Convert.ToString(dt.Rows[i]["tax_reverse_flag"].ToString());
                    objModel.tax_active = Convert.ToString(dt.Rows[i]["tax_active_flag"].ToString());
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
        public IEnumerable<Gettax> Gettaxvl()
        {
            try
            {
                List<Gettax> objCountrytype = new List<Gettax>();
                Gettax objModel;
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
                    objModel = new Gettax();
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
        public IEnumerable<Gettax> Gettaxvl_id(int taxId)
        {
            try
            {
                List<Gettax> objCountrytype = new List<Gettax>();
                Gettax objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tax", con);
                cmd.Parameters.AddWithValue("@action", "selectBy_Id");
                cmd.Parameters.AddWithValue("@gid", taxId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new Gettax();
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
        public iem_mst_tax GettaxById(int taxId)
        {
            try
            {
                List<iem_mst_tax> objOrgType = new List<iem_mst_tax>();
                iem_mst_tax objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tax", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", taxId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objModel = new iem_mst_tax()
                {
                    tax_gid = Convert.ToInt32(dt.Rows[0]["tax_gid"].ToString()),
                    tax_code = Convert.ToString(dt.Rows[0]["tax_code"].ToString()),
                    tax_name = Convert.ToString(dt.Rows[0]["tax_name"].ToString()),
                    tax_parent_flag = Convert.ToString(dt.Rows[0]["tax_parent_flag"].ToString()),
                    tax_parent_name = Convert.ToString(dt.Rows[0]["tax_parent_gid"].ToString()),
                    tax_withhold_flag = Convert.ToString(dt.Rows[0]["tax_withhold_flag"].ToString()),
                    tax_payable_flag = Convert.ToString(dt.Rows[0]["tax_payable_flag"].ToString()),
                    tax_receivable_flag = Convert.ToString(dt.Rows[0]["tax_receivable_flag"].ToString()),
                    tax_inputcredit_flag = Convert.ToString(dt.Rows[0]["tax_inputcredit_flag"].ToString()),
                    tax_reverse_flag = Convert.ToString(dt.Rows[0]["tax_reverse_flag"].ToString()),
                    tax_active = Convert.ToString(dt.Rows[0]["tax_active_flag"].ToString()),

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

        public string Inserttax(iem_mst_tax taxcat)
        {
            string result;
            try
            {

                CommonIUD comm = new CommonIUD();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tax", con);
                GetCon();
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.AddWithValue("@tax_code", taxcat.tax_code);
                cmd.Parameters.AddWithValue("@tax_name", taxcat.tax_name);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
	               {
                        {"tax_code",taxcat.tax_code},
                        {"tax_name",taxcat.tax_name.ToString() },                      
	                    {"tax_parent_flag", taxcat.tax_parent_flag},                        
	                    {"tax_parent_gid", taxcat.tax_parent_name1.ToString ()},
                        {"tax_withhold_flag", taxcat.tax_withhold_flag.ToString ()},
                        {"tax_payable_flag", taxcat.tax_payable_flag.ToString ()},
                        {"tax_receivable_flag", taxcat.tax_receivable_flag.ToString ()},  
                         {"tax_inputcredit_flag", taxcat.tax_inputcredit_flag.ToString ()},  
                          {"tax_reverse_flag", taxcat.tax_reverse_flag.ToString ()},  
                           {"tax_active_flag", taxcat.tax_active.ToString ()},
                            {"tax_insert_by", taxcat.tax_insert_by.ToString ()},
                        {"tax_insert_date","sysdatetime()"}                        
                  };
                    string tname = "iem_mst_ttax";
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
        public string Deletetax(int taxId)
        {
            string result;
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;

            try
            {
                CommonIUD comm = new CommonIUD();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tax", con);
                GetCon();
                cmd.Parameters.AddWithValue("@action", "Delete");
                cmd.Parameters.AddWithValue("@gid", taxId);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    con.Close();

                    CommonIUD comm1 = new CommonIUD();
                    SqlCommand cmd1 = new SqlCommand("pr_iem_mst_tax", con);
                    GetCon();
                    cmd1.Parameters.AddWithValue("@action", "Delete_subtype");
                    cmd1.Parameters.AddWithValue("@gid", taxId);
                    cmd1.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.ExecuteNonQuery();
                    string res2 = cmd1.Parameters["@res"].Value.ToString();
                    if (res2 == "Not There")
                    {
                        string[,] codes = new string[,]
	                {
                         {"tax_isremoved", "Y"}
	            
                         };
                        string[,] whrcol = new string[,]
	                    {
                          {"tax_gid", taxId.ToString ()}
                        };
                        string tblname = "iem_mst_ttax";


                        string deletetbl = delecomm.DeleteCommon(codes, whrcol, tblname);
                        result = "success";
                    }
                    else
                    {
                        result = "Can Not Delete this, Its Based On Some Other category";
                    }
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
        //public void Updattax(iem_mst_tax taxcat)
        //{
        //    try
        //    {
        //        CommonIUD comm = new CommonIUD();
        //        SqlCommand cmd = new SqlCommand("pr_iem_mst_tax", con);
        //        GetCon();
        //        cmd.Parameters.AddWithValue("@action", "Exist");
        //        cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.ExecuteNonQuery();
        //        string res1 = cmd.Parameters["@res"].Value.ToString();
        //        if (res1 == "Not There")
        //        {
        //            CommonIUD delecomm = new CommonIUD();
        //            string[,] codes = new string[,]

        //           {
        //                  {"tax_code",taxcat.tax_code},
        //                {"tax_name",taxcat.tax_name.ToString() },                      
        //                {"tax_parent_flag", taxcat.tax_parent_flag},                        
        //                {"tax_parent_gid", taxcat.tax_parent_name1.ToString ()},
        //                {"tax_withhold_flag", taxcat.tax_withhold_flag.ToString ()},
        //                {"tax_payable_flag", taxcat.tax_payable_flag.ToString ()},
        //                {"tax_receivable_flag", taxcat.tax_receivable_flag.ToString ()},  
        //                 {"tax_inputcredit_flag", taxcat.tax_inputcredit_flag.ToString ()},  
        //                  {"tax_reverse_flag", taxcat.tax_reverse_flag.ToString ()},  
        //                   {"tax_active_flag", taxcat.tax_active.ToString ()},
        //                    {"tax_update_by", taxcat.tax_insert_by.ToString ()},
        //                {"tax_update_date","sysdatetime()"}                     
        //          };
        //            string[,] whrcol = new string[,]
        //             {
        //                  {"tax_gid", taxcat.tax_gid.ToString()}

        //             };
        //            string tblname = "iem_mst_ttax";

        //            string updcomm = delecomm.UpdateCommon(codes, whrcol, tblname);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {

        //    }

        //}
        public string Updattax(iem_mst_tax taxcat)
        {
            string result;
            try
            {
                CommonIUD comm = new CommonIUD();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tax", con);
                GetCon();
                cmd.Parameters.AddWithValue("@action", "exist");
                cmd.Parameters.AddWithValue("@gid", taxcat.tax_gid);
                cmd.Parameters.AddWithValue("@tax_name", taxcat.tax_name);
                cmd.Parameters.AddWithValue("@tax_code", taxcat.tax_code);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes = new string[,]
	                  {
                          {"tax_code",taxcat.tax_code},
                      {"tax_name",taxcat.tax_name.ToString() },                      
                     {"tax_parent_flag", taxcat.tax_parent_flag},                        
                      {"tax_parent_gid", taxcat.tax_parent_name1.ToString ()},
                    {"tax_withhold_flag", taxcat.tax_withhold_flag.ToString ()},
                      {"tax_payable_flag", taxcat.tax_payable_flag.ToString ()},
                      {"tax_receivable_flag", taxcat.tax_receivable_flag.ToString ()},  
                     {"tax_inputcredit_flag", taxcat.tax_inputcredit_flag.ToString ()},  
                       {"tax_reverse_flag", taxcat.tax_reverse_flag.ToString ()},  
                          {"tax_active_flag", taxcat.tax_active.ToString ()},
                          {"tax_update_by", taxcat.tax_insert_by.ToString ()},
                       {"tax_update_date","sysdatetime()"}    
	                  };
                    string[,] whrcol = new string[,]
	                 {
                          {"tax_gid", taxcat.tax_gid.ToString ()},
                            // {"tax_isremoved", "N"}
                     };
                    string tblname = "iem_mst_ttax";

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
    }
}