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
    public class IEM_MST_TAXSUBTYPE : Iiem_mst_Tax_subtype
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
        public IEnumerable<iem_mst_tax_subtype> NatureofExpensesdataother()
        {
            List<iem_mst_tax_subtype> objNatureofExpenses = new List<iem_mst_tax_subtype>();
            try
            {
                iem_mst_tax_subtype objModel;
                GetCon();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Other";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_tax_subtype();
                    objModel.NatureofExpensesId = Convert.ToInt32(dt.Rows[i]["expnature_gid"].ToString());
                    objModel.NatureofExpensesName = Convert.ToString(dt.Rows[i]["expnature_name"].ToString());
                    objNatureofExpenses.Add(objModel);
                }
                return objNatureofExpenses;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        public IEnumerable<iem_mst_tax_subtype> ExpenseCategorydata(int id)
        {
            List<iem_mst_tax_subtype> objExpenseCategory = new List<iem_mst_tax_subtype>();
            try
            {
                iem_mst_tax_subtype objModel;
                GetCon();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id1", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Category";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_tax_subtype();
                    objModel.ExpenseCategoryId = Convert.ToInt32(dt.Rows[i]["expcat_gid"].ToString());
                    objModel.ExpenseCategoryName = Convert.ToString(dt.Rows[i]["expcat_name"].ToString());
                    objExpenseCategory.Add(objModel);
                }
                return objExpenseCategory;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        public IEnumerable<iem_mst_tax_subtype> SubCategorydata(int id)
        {
            List<iem_mst_tax_subtype> objSubCategory = new List<iem_mst_tax_subtype>();
            try
            {
                iem_mst_tax_subtype objModel;
                GetCon();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id1", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "SubCategory";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_tax_subtype();
                    objModel.SubCategoryId = Convert.ToInt32(dt.Rows[i]["expsubcat_gid"].ToString());
                    objModel.SubCategoryName = Convert.ToString(dt.Rows[i]["expsubcat_name"].ToString());
                    objModel.GLCode = Convert.ToString(dt.Rows[i]["expcat_gl_no"].ToString());
                    objSubCategory.Add(objModel);
                }
                return objSubCategory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        public IEnumerable<iem_mst_tax_subtype> gettaxsubtype()
        {
            try
            {
                List<iem_mst_tax_subtype> objOrgType = new List<iem_mst_tax_subtype>();
                iem_mst_tax_subtype objModel;
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
                    objModel = new iem_mst_tax_subtype();
                    objModel.taxsubtype_gid = Convert.ToInt32(dt.Rows[i]["taxsubtype_gid"].ToString());
                    objModel.taxsubtype_code = Convert.ToString(dt.Rows[i]["taxsubtype_code"].ToString());
                    objModel.taxsubtype_name = Convert.ToString(dt.Rows[i]["taxsubtype_name"].ToString());
                    objModel.taxsubtype_parent_name = Convert.ToString(dt.Rows[i]["taxsubtype_tax_gid"].ToString());
                    objModel.taxsubtype_active_flag = Convert.ToString(dt.Rows[i]["taxsubtype_active_flag"].ToString());
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
        public IEnumerable<iem_mst_tax_subtype> gettaxsubtypeid(string filter_name, string filter_name1)
        {
            try
            {
                List<iem_mst_tax_subtype> objOrgType = new List<iem_mst_tax_subtype>();
                iem_mst_tax_subtype objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tax_sub_type", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.Parameters.AddWithValue("@taxsubtype_code", filter_name);
                cmd.Parameters.AddWithValue("@taxsubtype_name", filter_name1);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_tax_subtype();
                    objModel.taxsubtype_gid = Convert.ToInt32(dt.Rows[i]["taxsubtype_gid"].ToString());
                    objModel.taxsubtype_code = Convert.ToString(dt.Rows[i]["taxsubtype_code"].ToString());
                    objModel.taxsubtype_name = Convert.ToString(dt.Rows[i]["taxsubtype_name"].ToString());
                    objModel.taxsubtype_parent_name = Convert.ToString(dt.Rows[i]["taxsubtype_tax_gid"].ToString());
                    objModel.taxsubtype_active_flag = Convert.ToString(dt.Rows[i]["taxsubtype_active_flag"].ToString());
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
        public IEnumerable<Gettaxsub> Gettaxsubvl()
        {
            try
            {
                List<Gettaxsub> objCountrytype = new List<Gettaxsub>();
                Gettaxsub objModel;
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
                    objModel = new Gettaxsub();
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
        public iem_mst_tax_subtype GettaxsubById(int taxId)
        {
            try
            {
                List<iem_mst_tax_subtype> objOrgType = new List<iem_mst_tax_subtype>();
                iem_mst_tax_subtype objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tax_sub_type", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", taxId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objModel = new iem_mst_tax_subtype()
                {
                    taxsubtype_gid = Convert.ToInt32(dt.Rows[0]["taxsubtype_gid"].ToString()),
                    taxsubtype_code = Convert.ToString(dt.Rows[0]["taxsubtype_code"].ToString()),
                    taxsubtype_name = Convert.ToString(dt.Rows[0]["taxsubtype_name"].ToString()),
                    taxsubtype_parent_name = Convert.ToString(dt.Rows[0]["taxsubtype_tax_gid"].ToString()),
                    taxsubtype_active_flag = Convert.ToString(dt.Rows[0]["taxsubtype_active_flag"].ToString()),
                    GLCode = Convert.ToString(dt.Rows[0]["taxsubtype_gl_no"].ToString()),
                    //NatureofExpensesId = Convert.ToInt32(dt.Rows[0]["expnature_gid"].ToString()),
                    //NatureofExpensesName = Convert.ToString(dt.Rows[0]["expnature_name"].ToString()),
                    //ExpenseCategoryId = Convert.ToInt32(dt.Rows[0]["expcat_gid"].ToString()),
                    //ExpenseCategoryName = Convert.ToString(dt.Rows[0]["expcat_name"].ToString()),
                    //SubCategoryId = Convert.ToInt32(dt.Rows[0]["expsubcat_gid"].ToString()),
                    //SubCategoryName = Convert.ToString(dt.Rows[0]["expsubcat_name"].ToString()),

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
        // Code added by sabari for validate GL
        public string CheckValidGL(string GLCode)
        {
            EOW.Models.EOW_DataModel objEOW_DataModel = new EOW.Models.EOW_DataModel();
            return objEOW_DataModel.GetStatusexcel(GLCode, "", "", "GLCode");
        }

        public string Inserttaxsub(iem_mst_tax_subtype taxcat)
        {
            string result;
            string status;
            try
            {
                
                status = CheckValidGL(taxcat.GLCode.ToString());

                if (status == "notexists")
                {
                    result = "Invalid GL Code";
                    return result;
                }
                else
                {

                CommonIUD comm = new CommonIUD();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tax_sub_type", con);
                GetCon();
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.AddWithValue("@taxsubtype_code", taxcat.taxsubtype_code);
                cmd.Parameters.AddWithValue("@taxsubtype_name", taxcat.taxsubtype_name);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
	               {
                        {"taxsubtype_code",taxcat.taxsubtype_code},
                        {"taxsubtype_name",taxcat.taxsubtype_name.ToString() },                      
	                    {"taxsubtype_tax_gid", taxcat.taxsubtype_parent_name},                        
	                    {"taxsubtype_active_flag", taxcat.taxsubtype_active_flag.ToString ()}, 
                        {"taxsubtype_gl_no", taxcat.GLCode.ToString ()}, 
                         
                        {"taxsubtype_insert_by", taxcat.taxsubtype_insert_by.ToString ()},
                        {"taxsubtype_insert_date","sysdatetime()"}                        
                  };
                        string tname = "iem_mst_ttaxsubtype";
                        string insertcommend = comm.InsertCommon(codes, tname);
                        result = "success";
                    }
                    else
                    {
                        result = "Duplicate record !";
                    }
                    return result;
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


        }
       

        public string Updattaxsub(iem_mst_tax_subtype taxcat)
        {
            string result;
            string status;
            try
            {
                status = CheckValidGL(taxcat.GLCode.ToString());

                if (status == "notexists")
                {
                    result = "Invalid GL Code";
                    return result;
                }
                else { 
                CommonIUD comm = new CommonIUD();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tax_sub_type", con);
                GetCon();
                cmd.Parameters.AddWithValue("@action", "exist");
                cmd.Parameters.AddWithValue("@gid", taxcat.taxsubtype_gid);
                cmd.Parameters.AddWithValue("@taxsubtype_name", taxcat.taxsubtype_name);
                cmd.Parameters.AddWithValue("@taxsubtype_code", taxcat.taxsubtype_code);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes = new string[,]
	                  {
                          {"taxsubtype_code", taxcat.taxsubtype_code},
                          {"taxsubtype_name", taxcat.taxsubtype_name},
                          {"taxsubtype_tax_gid", taxcat.taxsubtype_parent_name.ToString ()},
                          {"taxsubtype_active_flag", taxcat.taxsubtype_active_flag.ToString()},
                          {"taxsubtype_gl_no", taxcat.GLCode.ToString ()}, 
                          {"taxsubtype_update_by", taxcat.taxsubtype_update_by.ToString ()},
                          {"taxsubtype_update_date", delecomm.GetCurrentDate()}
	                  };
                    string[,] whrcol = new string[,]
	                 {
                          {"taxsubtype_gid", taxcat.taxsubtype_gid.ToString ()},
                             {"taxsubtype_isremoved", "N"}
                     };
                    string tblname = "iem_mst_ttaxsubtype";

                    string updcomm = delecomm.UpdateCommon(codes, whrcol, tblname);
                    result = "success";
                }
                else
                {
                    result = "Duplicate record !";
                }
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

       
        public string Deletetaxsub(int taxId)
        {
            string result;
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;

            try
            {
                CommonIUD comm1 = new CommonIUD();
                SqlCommand cmd1 = new SqlCommand("pr_iem_mst_tax_sub_type", con);
                GetCon();
                cmd1.Parameters.AddWithValue("@action", "Delete_taxrate");
                cmd1.Parameters.AddWithValue("@gid", taxId);
                cmd1.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.ExecuteNonQuery();
                string res2 = cmd1.Parameters["@res"].Value.ToString();
                if (res2 == "Not There")
                {
                    string[,] codes = new string[,]
	                {
                         {"taxsubtype_isremoved", "Y"}
	            
                         };
                    string[,] whrcol = new string[,]
	                    {
                          {"taxsubtype_gid", taxId.ToString ()}
                        };
                    string tblname = "iem_mst_ttaxsubtype";


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
        public IEnumerable<iem_mst_tax_subtype> GetGlNumber(string glnumber)
        {
            List<iem_mst_tax_subtype> objNatureofExpenses = new List<iem_mst_tax_subtype>();
            try
            {
                iem_mst_tax_subtype objModel;
                GetCon();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = glnumber;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "AUTOCOMPLETEGLNO";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new iem_mst_tax_subtype();
                    objModel.GLId = Convert.ToInt32(row["gl_gid"].ToString());
                    objModel.GLCode = row["gl_no"].ToString();
                    objNatureofExpenses.Add(objModel);
                }
                return objNatureofExpenses;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
            //throw new NotImplementedException();
        }
    }
}