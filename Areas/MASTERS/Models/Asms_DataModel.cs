using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Globalization;
using IEM.Areas.MASTERS.Models;
using IEM.Common;

namespace IEM.Areas.MASTERS.Models
{
    public class Asms_DataModel : Asms_IRepository
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CommonIUD comm = new CommonIUD();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();

        private void GetConnection()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                    con.Open();
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
        }

        public IEnumerable<CategoryModel> GetCategory(string filter)
        {
            List<CategoryModel> objCategory = new List<CategoryModel>();
            try
            {
                CategoryModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_SupplierCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = filter;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new CategoryModel();
                    objModel.sNo = i + 1;
                    objModel.categoryID = Convert.ToInt32(dt.Rows[i]["suppliercategory_gid"].ToString());
                    objModel.categoryName = Convert.ToString(dt.Rows[i]["suppliercategory_name"].ToString());
                    objCategory.Add(objModel);
                }

                return objCategory;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objCategory;
            }
            finally
            {
                con.Close();
            }
        }

        public string GetCategoryname(int CategoryId)
        {
            try
            {
                GetConnection();
                string[,] parameter = new string[,]
                {
                    {"@id",CategoryId.ToString()},
                    {"@action","select1"},
                };
                return comm.ProcedureCommon(parameter, "pr_asms_mst_SupplierCategory");
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
            }
        }

        public CategoryModel GetCategoryById(int CategoryId)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_SupplierCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = CategoryId;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new CategoryModel()
                {
                    categoryID = Convert.ToInt32(dt.Rows[0]["suppliercategory_gid"].ToString()),
                    categoryName = Convert.ToString(dt.Rows[0]["suppliercategory_Name"].ToString()),
                };
                return model;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                CategoryModel objMod = new CategoryModel();
                return objMod;
            }
            finally
            {
                con.Close();
            }
        }

        public string InsertCategory(CategoryModel Category)
        {
            try
            {
                string CAT = Category.categoryName.ToLower();
                CAT = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(CAT.ToString());
                if (CAT.Contains("'") == true)
                {
                    CAT = CAT.Replace("'", "''");
                }
                GetConnection();
                cmd = new SqlCommand("pr_asms_mst_SupplierCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = CAT.ToString();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Exist";
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
	                   {
                            {"suppliercategory_Name", CAT},
	                        {"suppliercategory_isRemoved", "N"},
                            {"suppliercategory_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                            {"suppliercategory_insert_date",Convert.ToString(comm.GetCurrentDate())}
                      };
                    string tname = "asms_mst_tsuppliercategory";
                    string insertcommend = comm.InsertCommon(codes, tname);
                    return null;
                }
                else { return res1; }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }

        public CategoryModel DeleteCategory(int Categoryid)
        {
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_prodservice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prodservicegid", Categoryid);
                cmd.Parameters.AddWithValue("@actionName", "Supplerclassfication");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new CategoryModel()
                {
                    categoryID = Convert.ToInt32(dt.Rows[0]["projectgid"])
                };
                return model;
                //string[,] codes = new string[,]
                //           {
                //                 {"suppliercategory_isRemoved", "Y"}	            
                //           };
                //string[,] whrcol = new string[,]
                //            {
                //                {"suppliercategory_gid", Categoryid.ToString()}
                //            };
                //string tblname = "asms_mst_tsuppliercategory";
                //string deletetbl = comm.DeleteCommon(codes, whrcol, tblname);

            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                CategoryModel objMod = new CategoryModel();
                return objMod;
            }
            finally
            {
                con.Close();
            }
        }

        public string EditCategory(CategoryModel Category)
        {
            try
            {
                string CAT = Category.categoryName.ToLower();
                if (CAT.Contains("'") == true)
                {
                    CAT = CAT.Replace("'", "''");
                }
                GetConnection();
                cmd = new SqlCommand("pr_asms_mst_SupplierCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = Category.categoryID;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = CAT;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Exist";
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();

                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
	                {
                         {"suppliercategory_name", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(CAT.ToString())},
                         {"suppliercategory_update_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                         {"suppliercategory_update_date",Convert.ToString(comm.GetCurrentDate())}
	                };
                    string[,] whrcol = new string[,]
	                {
                         {"suppliercategory_gid", Category.categoryID.ToString()}            
                    };
                    string tblname = "asms_mst_tsuppliercategory";
                    string updcomm = comm.UpdateCommon(codes, whrcol, tblname);
                    return null;
                }
                else { return res1; }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
            }
        }

        public IEnumerable<DocFrqModel> GetDocFrq(string filter)
        {
            List<DocFrqModel> objDocFrq = new List<DocFrqModel>();
            try
            {

                DocFrqModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_documentfrequency", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = filter;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new DocFrqModel();
                    objModel.sNo = i + 1;
                    objModel.docFrqID = Convert.ToInt32(dt.Rows[i]["frequency_gid"].ToString());
                    objModel.docFrqName = Convert.ToString(dt.Rows[i]["frequency_name"].ToString());
                    objModel.docFrqMonth = Convert.ToInt32(dt.Rows[i]["frequency_months"].ToString());
                    objDocFrq.Add(objModel);
                }
                return objDocFrq;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objDocFrq;
            }
            finally
            {
                con.Close();
            }
        }

        public IEnumerable<DocFrqModel> GetDocFrqedit()
        {
            List<DocFrqModel> objDocFrq = new List<DocFrqModel>();
            try
            {

                DocFrqModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_documentfrequency", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "normal";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new DocFrqModel();
                    objModel.docFrqID = Convert.ToInt32(dt.Rows[i]["frequency_gid"].ToString());
                    objModel.docFrqName = Convert.ToString(dt.Rows[i]["frq"].ToString());
                    objDocFrq.Add(objModel);
                }
                return objDocFrq;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objDocFrq;
            }
            finally
            {
                con.Close();
            }
        }

        public IEnumerable<DocFrqModel> GetDocFrq()
        {
            List<DocFrqModel> objDocFrq = new List<DocFrqModel>();
            try
            {
                DocFrqModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_documentfrequency", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "drop";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new DocFrqModel();
                    objModel.sNo = i + 1;
                    objModel.docFrqID = Convert.ToInt32(dt.Rows[i]["frequency_gid"].ToString());
                    objModel.docFrqName = Convert.ToString(dt.Rows[i]["frq"].ToString());
                    objDocFrq.Add(objModel);
                }
                return objDocFrq;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objDocFrq;
            }
            finally
            {
                con.Close();
            }
        }

        public DocFrqModel GetDocFrqById(int DocFrqId)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_documentfrequency", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = DocFrqId;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new DocFrqModel()
                {
                    docFrqID = Convert.ToInt32(dt.Rows[0]["frequency_gid"].ToString()),
                    docFrqName = Convert.ToString(dt.Rows[0]["frequency_name"].ToString()),
                    docFrqMonth = Convert.ToInt32(dt.Rows[0]["frequency_months"].ToString())
                };
                return model;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                DocFrqModel objMod = new DocFrqModel();
                return objMod;
            }
            finally
            {
                con.Close();
            }
        }

        public string InsertDocFrq(DocFrqModel DocFrq)
        {
            try
            {
                string frq = DocFrq.docFrqName.ToLower();
                if (frq.Contains("'") == true)
                {
                    frq = frq.Replace("'", "''");
                }
                GetConnection();
                cmd = new SqlCommand("pr_asms_mst_documentfrequency", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = frq;
                cmd.Parameters.Add("@month", SqlDbType.Int).Value = DocFrq.docFrqMonth;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Exist";
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();

                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
	               {
                       {"frequency_name",CultureInfo.CurrentCulture.TextInfo.ToTitleCase(frq.ToString()) },	          
                       {"frequency_months",Convert.ToString(DocFrq.docFrqMonth)},
                       {"frequency_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                       {"frequency_insert_date",Convert.ToString(comm.GetCurrentDate())}
                  };
                    string tname = "iem_mst_tfrequency";
                    string insertcommend = comm.InsertCommon(codes, tname);
                    return null;
                }
                else { return res1; }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }

        public string DeleteDocFrq(int DocFrqid)
        {
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_mst_documentfrequency", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "delete";
                cmd.Parameters.Add("@sub", SqlDbType.Int).Value = DocFrqid;
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
	                   {
                             {"frequency_isremoved", "Y"}
	                    };
                    string[,] whrcol = new string[,]
	                    {
                            {"frequency_gid", DocFrqid.ToString()}
                        };
                    string tblname = "iem_mst_tfrequency";
                    string deletetbl = comm.DeleteCommon(codes, whrcol, tblname);
                    return null;
                }
                else { return res1; }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
            }
        }

        //public string EditDocFrq(DocFrqModel DocFrq)
        //{
        //    try
        //    {
        //        string frq = DocFrq.docFrqName.ToLower();
        //        if (frq.Contains("'") == true)
        //        {
        //            frq = frq.Replace("'", "''");
        //        }
        //        GetConnection();
        //        cmd = new SqlCommand("pr_asms_mst_documentfrequency", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@id", SqlDbType.Int).Value = DocFrq.docFrqID;
        //        cmd.Parameters.Add("@month", SqlDbType.Int).Value = DocFrq.docFrqMonth;
        //        cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = frq;
        //        cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "delete";
        //        cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
        //        cmd.ExecuteNonQuery();
        //        string res1 = cmd.Parameters["@res"].Value.ToString();
        //        if (res1 == "Not There")
        //        {
        //            string[,] codes = new string[,]
        //            {
        //                 {"frequency_name", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(frq.ToString())},
        //                 {"frequency_months",Convert.ToString(DocFrq.docFrqMonth)},
        //                 {"frequency_update_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
        //                 {"frequency_update_date",Convert.ToString(comm.GetCurrentDate())}
        //            };
        //            string[,] whrcol = new string[,]
        //            {
        //             {"[frequency_gid]", DocFrq.docFrqID.ToString()}            
        //            };
        //            string tblname = "iem_mst_tfrequency";

        //            string updcomm = comm.UpdateCommon(codes, whrcol, tblname);
        //            return null;
        //        }
        //        else { return res1; }
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //        return "";
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //}
        public string EditDocFrq(DocFrqModel DocFrq)
        {
            try
            {
                string frq = DocFrq.docFrqName.ToLower();
                if (frq.Contains("'") == true)
                {
                    frq = frq.Replace("'", "''");
                }
                GetConnection();
                cmd = new SqlCommand("pr_asms_mst_documentfrequency", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = DocFrq.docFrqID;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = frq;
                cmd.Parameters.Add("@month", SqlDbType.Int).Value = DocFrq.docFrqMonth;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "exist";
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();

                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
	               {
                       {"frequency_name",CultureInfo.CurrentCulture.TextInfo.ToTitleCase(frq.ToString()) },	          
                       {"frequency_months",Convert.ToString(DocFrq.docFrqMonth)}
                  };
                    string[,] whrcol = new string[,]
	                 {         
                          {"frequency_gid", DocFrq.docFrqID.ToString ()}
                     };
                    string tname = "iem_mst_tfrequency";
                    string insertcommend = comm.UpdateCommon(codes, whrcol, tname);
                    return null;
                }
                else { return res1; }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        public IEnumerable<DocGrpModel> GetDocGrp(string filter)
        {
            List<DocGrpModel> objDocGrp = new List<DocGrpModel>();
            try
            {

                DocGrpModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_documentgroup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = filter;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new DocGrpModel();
                    objModel.sNo = i + 1;
                    objModel.docGrpID = Convert.ToInt32(dt.Rows[i]["documentgroup_gid"].ToString());
                    objModel.docGrpName = Convert.ToString(dt.Rows[i]["documentgroup_name"].ToString());
                    objDocGrp.Add(objModel);
                }

                return objDocGrp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objDocGrp;
            }
            finally
            {
                con.Close();
            }
        }

        public IEnumerable<DocGrpModel> GetDocGrp()
        {
            List<DocGrpModel> objDocGrp = new List<DocGrpModel>();
            try
            {

                DocGrpModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_documentgroup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "drop";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new DocGrpModel();
                    objModel.docGrpID = Convert.ToInt32(dt.Rows[i]["documentgroup_gid"].ToString());
                    objModel.docGrpName = Convert.ToString(dt.Rows[i]["documentgroup_name"].ToString());
                    objDocGrp.Add(objModel);
                }

                return objDocGrp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objDocGrp;
            }
            finally
            {
                con.Close();
            }
        }

        public IEnumerable<FrqModel> GetFrq()
        {
            List<FrqModel> objDocGrp = new List<FrqModel>();
            try
            {

                FrqModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_documentname", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "drop";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new FrqModel();
                    objModel.frqID = Convert.ToInt32(dt.Rows[i]["documentfrequency_gid"].ToString());
                    objModel.frqName = Convert.ToString(dt.Rows[i]["documentfrequency_name"].ToString());
                    objDocGrp.Add(objModel);
                }

                return objDocGrp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objDocGrp;
            }
            finally
            {
                con.Close();
            }
        }

        public DocGrpModel GetDocGrpById(int DocGrpId)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_documentgroup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = DocGrpId;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new DocGrpModel()
                {
                    docGrpID = Convert.ToInt32(dt.Rows[0]["documentgroup_gid"].ToString()),
                    docGrpName = Convert.ToString(dt.Rows[0]["documentgroup_name"].ToString()),
                };
                return model;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                DocGrpModel objMod = new DocGrpModel();
                return objMod;
            }
            finally
            {

            }
        }

        public string InsertDocGrp(DocGrpModel DocGrp)
        {
            try
            {
                string grp = DocGrp.docGrpName.ToLower();
                if (grp.Contains("'") == true)
                {
                    grp = grp.Replace("'", "''");
                }
                GetConnection();
                cmd = new SqlCommand("pr_asms_mst_documentgroup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = grp;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Exist";
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();

                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
	                {
                        {"documentgroup_name",CultureInfo.CurrentCulture.TextInfo.ToTitleCase(grp.ToString()) },
	                    {"documentgroup_isremoved", "N"},
                        {"documentgroup_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                        {"documentgroup_insert_date",Convert.ToString(comm.GetCurrentDate()) }
                    };
                    string tname = "asms_mst_tdocumentgroup";
                    string insertcommend = comm.InsertCommon(codes, tname);
                    return null;
                }
                else { return res1; }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }

        public string DeleteDocGrp(int DocGrpid)
        {
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_mst_documentgroup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "delete";
                cmd.Parameters.Add("@sub", SqlDbType.Int).Value = DocGrpid;
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
	               {
                         {"documentgroup_isremoved", "Y"}	            
                   };
                    string[,] whrcol = new string[,]
	                {
                        {"documentgroup_gid", DocGrpid.ToString()}
                    };
                    string tblname = "asms_mst_tdocumentgroup";
                    string deletetbl = comm.DeleteCommon(codes, whrcol, tblname);
                    return null;
                }
                else { return res1; }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
            }
        }

        public string EditDocGrp(DocGrpModel DocGrp)
        {
            try
            {
                string grp = DocGrp.docGrpName.ToLower();
                if (grp.Contains("'") == true)
                {
                    grp = grp.Replace("'", "''");
                }
                GetConnection();
                cmd = new SqlCommand("pr_asms_mst_documentgroup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = DocGrp.docGrpID;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = grp;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Exist";
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
	                    {
                             {"documentgroup_name", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(grp.ToString())},
                             {"documentgroup_update_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                             {"documentgroup_update_date",Convert.ToString(comm.GetCurrentDate())}
	                    };
                    string[,] whrcol = new string[,]
	                    {
                             {"[documentgroup_gid]", DocGrp.docGrpID.ToString()}            
                        };
                    string tblname = "asms_mst_tdocumentgroup";
                    string updcomm = comm.UpdateCommon(codes, whrcol, tblname);
                    return null;
                }
                else { return res1; }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
            }
        }

        public IEnumerable<DocNameModel> GetDocName(string filter)
        {
            List<DocNameModel> objDocName = new List<DocNameModel>();
            try
            {
                GetConnection();

                DocNameModel objModel;
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_documentname", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = filter;
                cmd.Parameters.Add("@Frqid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@Grpid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@Frqname", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@Grpname", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new DocNameModel();
                    objModel.sNo = i + 1;
                    objModel.docNameID = Convert.ToInt32(dt.Rows[i]["documentname_gid"].ToString());
                    objModel.docNameName = Convert.ToString(dt.Rows[i]["documentname_name"].ToString());
                    objModel.docGrpName = Convert.ToString(dt.Rows[i]["documentgroup_name"].ToString());
                    objModel.frqName = Convert.ToString(dt.Rows[i]["documentfrequency_name"].ToString());
                    objDocName.Add(objModel);
                }
                return objDocName;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objDocName;
            }
            finally
            {
                con.Close();
            }
        }

        public DocNameModel GetDocNameById(int DocNameId)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_documentname", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = DocNameId;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@Frqid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@Grpid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@Frqname", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@Grpname", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new DocNameModel()
                {
                    docGrpID = Convert.ToInt32(dt.Rows[0]["documentname_documentgroup_gid"].ToString()),
                    docNameID = Convert.ToInt32(dt.Rows[0]["documentname_gid"].ToString()),
                    docNameName = Convert.ToString(dt.Rows[0]["documentname_name"].ToString()),
                    docGrpName = Convert.ToString(dt.Rows[0]["documentgroup_name"].ToString()),
                    frqName = Convert.ToString(dt.Rows[0]["documentfrequency_name"].ToString()),
                    frqID = Convert.ToInt32(dt.Rows[0]["documentname_documentfrequency_gid"].ToString()),
                    docFrqID = Convert.ToString(dt.Rows[0]["documentname_frequency_gid"].ToString()),
                    docFrqName = Convert.ToString(dt.Rows[0]["frequency_name"].ToString()),
                    manOpt = Convert.ToString(dt.Rows[0]["documentname_mandatory_flag"].ToString()),
                    manDays = Convert.ToInt32(dt.Rows[0]["documentname_man_days"].ToString())
                };
                return model;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                DocNameModel objMod = new DocNameModel();
                return objMod;
            }
            finally
            {
                con.Close();
            }
        }

        public string InsertDocName(DocNameModel DocName)
        {
            try
            {
                string name = DocName.docNameName.ToLower();
                if (name.Contains("'") == true)
                {
                    name = name.Replace("'", "''");
                }
                GetConnection();
                cmd = new SqlCommand("pr_asms_mst_documentname", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@Frqid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@Grpid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = name;
                cmd.Parameters.Add("@frqname", SqlDbType.VarChar).Value = null;
                cmd.Parameters.Add("@Grpname", SqlDbType.VarChar).Value = null;
                cmd.Parameters.Add("@isremoved", SqlDbType.VarChar).Value = null;
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
	               {
                        {"documentname_name",CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToString()) },
                        {"documentname_documentgroup_gid",DocName.docGrpID.ToString() },	                   
                        {"documentname_documentfrequency_gid", DocName.frqID.ToString()},
	                    {"documentname_isremoved", "N"},
                        {"documentname_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                        {"documentname_insert_date",Convert.ToString(comm.GetCurrentDate())},
                        {"documentname_mandatory_flag", DocName.manOpt.ToString()},
                        {"documentname_man_days", DocName.manDays.ToString()},
                         {"documentname_frequency_gid", Convert.ToString(DocName.docFrqID)}
                  };
                    string tname = "asms_mst_tdocumentname";
                    string insertcommend = comm.InsertCommon(codes, tname);
                    return null;
                }
                else { return res1; }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }

        public DocNameModel DeleteDocName(int DocNameid)
        {
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_prodservice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prodservicegid", DocNameid);
                cmd.Parameters.AddWithValue("@actionName", "Docname");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new DocNameModel()
                {
                    docNameID = Convert.ToInt32(dt.Rows[0]["projectgid"])
                };
                return model;
                //string[,] codes = new string[,]
                //   {
                //         {"documentname_isremoved", "Y"}	            
                //   };
                //string[,] whrcol = new string[,]
                //    {
                //        {"documentname_gid", DocNameid.ToString()}
                //    };
                //string tblname = "[asms_mst_tdocumentname]";
                //string deletetbl = comm.DeleteCommon(codes, whrcol, tblname);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                DocNameModel objMod = new DocNameModel();
                return objMod;
            }
            finally
            {
                con.Close();
            }
        }

        public string EditDocName(DocNameModel DocName)
        {
            try
            {
                string name = DocName.docNameName.ToLower();
                if (name.Contains("'") == true)
                {
                    name = name.Replace("'", "''");
                }
                GetConnection();
                cmd = new SqlCommand("pr_asms_mst_documentname", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = DocName.docNameID;
                cmd.Parameters.Add("@Frqid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@Grpid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = name;
                cmd.Parameters.Add("@frqname", SqlDbType.VarChar).Value = null;
                cmd.Parameters.Add("@Grpname", SqlDbType.VarChar).Value = null;
                cmd.Parameters.Add("@isremoved", SqlDbType.VarChar).Value = null;
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
	            {
                      {"documentname_name", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToString())},
                      {"documentname_documentgroup_gid",DocName.docGrpID.ToString() },
	                  {"documentname_frequency_gid", DocName.docFrqID.ToString()},
                      {"documentname_mandatory_flag", DocName.manOpt.ToString()},
                      {"documentname_documentfrequency_gid", DocName.frqID.ToString()},
                      {"documentname_update_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                      {"documentname_update_date",Convert.ToString(comm.GetCurrentDate())},
                      {"documentname_man_days", DocName.manDays.ToString()}
	            };
                    string[,] whrcol = new string[,]
	            {
                     {"documentname_gid", DocName.docNameID.ToString()}
            
                };
                    string tblname = "asms_mst_tdocumentname";

                    string updcomm = comm.UpdateCommon(codes, whrcol, tblname);
                    return null;
                }
                else { return res1; }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
            }
        }

        public IEnumerable<OrgTypeModel> GetOrgType(string filter)
        {
            List<OrgTypeModel> objOrgType = new List<OrgTypeModel>();
            try
            {

                OrgTypeModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_OrganizationType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = filter;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new OrgTypeModel();
                    objModel.sNo = i + 1;
                    objModel.typeID = Convert.ToInt32(dt.Rows[i]["organizationtype_gid"].ToString());
                    objModel.typeName = Convert.ToString(dt.Rows[i]["organizationtype_name"].ToString());
                    objOrgType.Add(objModel);
                }

                return objOrgType;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objOrgType;
            }
            finally
            {
                con.Close();
            }
        }

        public OrgTypeModel GetOrgTypeById(int OrgTypeId)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_OrganizationType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = OrgTypeId;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new OrgTypeModel()
                {
                    typeID = Convert.ToInt32(dt.Rows[0]["organizationtype_gid"].ToString()),
                    typeName = Convert.ToString(dt.Rows[0]["organizationtype_name"].ToString()),
                };
                return model;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                OrgTypeModel objMod = new OrgTypeModel();
                return objMod;
            }
            finally
            {

            }
        }

        public string InsertOrgType(OrgTypeModel OrgTypes)
        {
            CommonIUD comm = new CommonIUD();
            try
            {
                string Org = OrgTypes.typeName.ToLower();
                if (Org.Contains("'") == true)
                {
                    Org = Org.Replace("'", "''");
                }
                GetConnection();
                cmd = new SqlCommand("pr_asms_mst_OrganizationType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = Org;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Exist";
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();

                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
	                           {
                                    {"organizationtype_name",CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Org.ToString())  },	                                
                                    {"organizationtype_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                                    {"organizationtype_insert_date",Convert.ToString(comm.GetCurrentDate())}
                              };
                    string tname = "asms_mst_torganizationtype";
                    string insertcommend = comm.InsertCommon(codes, tname);
                    return null;
                }
                else { return res1; }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }

        public OrgTypeModel DeleteOrgType(int OrgTypeId)
        {
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_prodservice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prodservicegid", OrgTypeId);
                cmd.Parameters.AddWithValue("@actionName", "SupplierOrg");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new OrgTypeModel()
                {
                    typeID = Convert.ToInt32(dt.Rows[0]["projectgid"])
                };
                return model;
                //     string[,] codes = new string[,]
                //{
                //      {"organizationtype_isremoved", "Y"}	            
                //};
                //     string[,] whrcol = new string[,]
                // {
                //     {"organizationtype_gid", OrgTypeId.ToString()}
                // };
                //     string tblname = "asms_mst_torganizationtype";
                //     string deletetbl = comm.DeleteCommon(codes, whrcol, tblname);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                OrgTypeModel objMod = new OrgTypeModel();
                return objMod;
            }
            finally
            {
                con.Close();
            }
        }

        public string UpdateOrgType(OrgTypeModel OrgTypes)
        {
            try
            {
                string Org = OrgTypes.typeName.ToLower();
                if (Org.Contains("'") == true)
                {
                    Org = Org.Replace("'", "''");
                }
                GetConnection();
                cmd = new SqlCommand("pr_asms_mst_OrganizationType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = OrgTypes.typeID;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = Org;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Exist";
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes = new string[,]
	                {
                         {"organizationtype_name", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Org.ToString()) },
                         {"organizationtype_update_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                         {"organizationtype_update_date",Convert.ToString(comm.GetCurrentDate())}
	                };
                    string[,] whrcol = new string[,]
	                {
                         {"organizationtype_gid", OrgTypes.typeID.ToString()}
            
                    };
                    string tblname = "asms_mst_torganizationtype";

                    string updcomm = comm.UpdateCommon(codes, whrcol, tblname);
                    return null;
                }
                else { return res1; }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
            }
        }

        public IEnumerable<SECategoryModel> GetSECategoryWithQues()
        {
            List<SECategoryModel> objSECategory = new List<SECategoryModel>();
            try
            {

                SECategoryModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_secategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "reviewSelect";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SECategoryModel();
                    objModel.sNo = i + 1;
                    objModel.seCategoryID = Convert.ToInt32(dt.Rows[i]["secategory_gid"].ToString());
                    objModel.seCategoryName = Convert.ToString(dt.Rows[i]["secategory_Name"].ToString());
                    objSECategory.Add(objModel);
                }

                return objSECategory;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objSECategory;
            }
            finally
            {
                con.Close();
            }
        }

        public IEnumerable<SECategoryModel> GetSECategory(string filter)
        {
            List<SECategoryModel> objSECategory = new List<SECategoryModel>();
            try
            {

                SECategoryModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_secategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = filter;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SECategoryModel();
                    objModel.sNo = i + 1;
                    objModel.seCategoryID = Convert.ToInt32(dt.Rows[i]["secategory_gid"].ToString());
                    objModel.seCategoryName = Convert.ToString(dt.Rows[i]["secategory_Name"].ToString());
                    objSECategory.Add(objModel);
                }

                return objSECategory;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objSECategory;
            }
            finally
            {
                con.Close();
            }
        }

        public IEnumerable<SECategoryModel> GetSECategory()
        {
            List<SECategoryModel> objSECategory = new List<SECategoryModel>();
            try
            {

                SECategoryModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_secategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "drop";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SECategoryModel();
                    objModel.seCategoryID = Convert.ToInt32(dt.Rows[i]["secategory_gid"].ToString());
                    objModel.seCategoryName = Convert.ToString(dt.Rows[i]["secategory_Name"].ToString());
                    objSECategory.Add(objModel);
                }

                return objSECategory;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objSECategory;
            }
            finally
            {
                con.Close();
            }
        }

        public SECategoryModel GetSECategoryById(int SECategoryId)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_SECategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = SECategoryId;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new SECategoryModel()
                {
                    seCategoryID = Convert.ToInt32(dt.Rows[0]["secategory_gid"].ToString()),
                    seCategoryName = Convert.ToString(dt.Rows[0]["secategory_Name"].ToString()),
                };
                return model;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                SECategoryModel objMod = new SECategoryModel();
                return objMod;
            }
            finally
            {
                con.Close();
            }
        }

        public string InsertSECategory(SECategoryModel SECategory)
        {
            CommonIUD comm = new CommonIUD();
            try
            {
                string cat = SECategory.seCategoryName.ToLower();
                if (cat.Contains("'") == true)
                {
                    cat = cat.Replace("'", "''");
                }
                GetConnection();
                cmd = new SqlCommand("pr_asms_mst_SECategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = cat;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Exist";
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();

                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
	                           {
                                    {"secategory_Name",CultureInfo.CurrentCulture.TextInfo.ToTitleCase(cat.ToString()) },
                                    {"secategory_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                                    {"secategory_insert_date",Convert.ToString(comm.GetCurrentDate())}
                              };
                    string tname = "asms_mst_tsecategory";
                    string insertcommend = comm.InsertCommon(codes, tname);
                    return null;
                }
                else { return res1; }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }

        public string DeleteSECategory(int SECategoryid)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_mst_SECategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = SECategoryid;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "delete";
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();

                if (res1 == "Not There")
                {
                    string col_value = string.Empty;
                    string col_temp = string.Empty;

                    string[,] codes = new string[,]
	                   {
                             {"secategory_isRemoved", "Y"}	            
                       };
                    string[,] whrcol = new string[,]
	                    {
                            {"secategory_gid", SECategoryid.ToString()}
                        };
                    string tblname = "asms_mst_tsecategory";
                    string deletetbl = comm.DeleteCommon(codes, whrcol, tblname);
                    return null;
                }
                else { return res1; }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
            }
        }

        public string EditSECategory(SECategoryModel SECategory)
        {
            try
            {
                string cat = SECategory.seCategoryName.ToLower();
                if (cat.Contains("'") == true)
                {
                    cat = cat.Replace("'", "''");
                }
                GetConnection();
                cmd = new SqlCommand("pr_asms_mst_SECategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = SECategory.seCategoryID;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = cat;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Exist";
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
	                {
                         {"secategory_Name",CultureInfo.CurrentCulture.TextInfo.ToTitleCase(cat.ToString()) },
                         {"secategory_update_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                         {"secategory_update_date",Convert.ToString(comm.GetCurrentDate())}
	                };
                    string[,] whrcol = new string[,]
	                {
                         {"secategory_gid", SECategory.seCategoryID.ToString()}            
                    };
                    string tblname = "asms_mst_tsecategory";
                    string updcomm = comm.UpdateCommon(codes, whrcol, tblname);
                    return null;
                }
                else { return res1; }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
            }
        }

        public IEnumerable<ServiceTypeModel> GetServiceType(string filter)
        {
            List<ServiceTypeModel> objServiceType = new List<ServiceTypeModel>();
            try
            {

                ServiceTypeModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_ServiceType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = filter;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ServiceTypeModel();
                    objModel.sNo = i + 1;
                    objModel.serviceID = Convert.ToInt32(dt.Rows[i]["servicetype_gid"].ToString());
                    objModel.serviceName = Convert.ToString(dt.Rows[i]["servicetype_name"].ToString());
                    objServiceType.Add(objModel);
                }

                return objServiceType;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objServiceType;
            }
            finally
            {
                con.Close();
            }
        }

        public ServiceTypeModel GetServiceTypeById(int ServiceTypeId)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_ServiceType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = ServiceTypeId;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new ServiceTypeModel()
                {
                    serviceID = Convert.ToInt32(dt.Rows[0]["servicetype_gid"].ToString()),
                    serviceName = Convert.ToString(dt.Rows[0]["servicetype_name"].ToString()),
                };
                return model;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                ServiceTypeModel objMod = new ServiceTypeModel();
                return objMod;
            }
            finally
            {
                con.Close();
            }
        }

        public string InsertServiceType(ServiceTypeModel ServiceTypes)
        {
            CommonIUD comm = new CommonIUD();
            try
            {
                string set = ServiceTypes.serviceName.ToLower();
                GetConnection();
                if (set.Contains("'") == true)
                {
                    set = set.Replace("'", "''");
                }
                cmd = new SqlCommand("pr_asms_mst_ServiceType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = set;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Exist";
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();

                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
	                       {
                            {"servicetype_name",CultureInfo.CurrentCulture.TextInfo.ToTitleCase(set.ToString())   },	                       
                            {"servicetype_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                            {"servicetype_insert_date",Convert.ToString(comm.GetCurrentDate())},
                            {"servicetype_isremoved","N"}
                         };
                    string tname = "[asms_mst_tservicetype]";
                    string insertcommend = comm.InsertCommon(codes, tname);
                    return null;
                }
                else { return res1; }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }

        public ServiceTypeModel DeleteServiceType(int Serviceid)
        {
            string col_value = string.Empty;
            string col_temp = string.Empty;
            string DelSergid = string.Empty;
            ServiceTypeModel TypeModel = new ServiceTypeModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_prodservice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prodservicegid", Serviceid);
                cmd.Parameters.AddWithValue("@actionName", "SupplierType");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new ServiceTypeModel()
                {
                    serviceID = Convert.ToInt32(dt.Rows[0]["projectgid"])
                };
                return model;
                // string[,] codes = new string[,]
                //{
                //      {"[servicetype_isremoved]", "Y"}	            
                //};
                // string[,] whrcol = new string[,]
                // {
                //     {"[servicetype_gid]", Serviceid.ToString()}
                // };
                // string tblname = "[asms_mst_tservicetype]";
                // string deletetbl = comm.DeleteCommon(codes, whrcol, tblname);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                ServiceTypeModel ObjeModel = new ServiceTypeModel();
                return ObjeModel;
            }
            finally
            {
                con.Close();
            }
        }

        public string EditServiceType(ServiceTypeModel ServiceType)
        {
            CommonIUD delecomm = new CommonIUD();
            try
            {
                string set = ServiceType.serviceName.ToLower();
                if (set.Contains("'") == true)
                {
                    set = set.Replace("'", "''");
                }
                GetConnection();
                cmd = new SqlCommand("pr_asms_mst_ServiceType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = ServiceType.serviceID;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = ServiceType.serviceName;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Exist";
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
	                    {
                             {"[servicetype_name]", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(ServiceType.serviceName .ToString())},
                             {"servicetype_update_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                             {"servicetype_update_date",Convert.ToString(comm.GetCurrentDate())}
	                    };
                    string[,] whrcol = new string[,]
	                    {
                             {"[servicetype_gid]", ServiceType.serviceID.ToString()}            
                        };
                    string tblname = "[asms_mst_tservicetype]";
                    string updcomm = comm.UpdateCommon(codes, whrcol, tblname);
                    return null;
                }
                else { return res1; }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
            }
        }

        public IEnumerable<SESubCategoryModel> GetSESubCategory(string filter)
        {
            List<SESubCategoryModel> objSESubCategory = new List<SESubCategoryModel>();
            try
            {
               
                SESubCategoryModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_SESubCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@Catid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = filter;
                cmd.Parameters.Add("@Catname", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SESubCategoryModel();
                    objModel.sNo = i + 1;
                    objModel.seSubCategoryID = Convert.ToInt32(dt.Rows[i]["sequestions_gid"].ToString());
                    objModel.seSubCategoryName = Convert.ToString(dt.Rows[i]["sequestions_question"].ToString());
                    objModel.seCategoryName = Convert.ToString(dt.Rows[i]["secategory_name"].ToString());
                    objModel.seRateName = Convert.ToString(dt.Rows[i]["serate_name"].ToString());
                    objSESubCategory.Add(objModel);
                }

                return objSESubCategory;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objSESubCategory;
            }
            finally
            {
                con.Close();
            }
        }

        public SESubCategoryModel GetSESubCategoryById(int SESubCategoryId)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_SESubCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = SESubCategoryId;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@Catname", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@Catid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@rateid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@rategroupid", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@queid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new SESubCategoryModel()
                {
                    seCategoryID = Convert.ToInt32(dt.Rows[0]["sequestions_secategory_gid"]),
                    seSubCategoryID = Convert.ToInt32(dt.Rows[0]["sequestions_gid"].ToString()),
                    seSubCategoryName = Convert.ToString(dt.Rows[0]["sequestions_question"].ToString()),
                    seCategoryName = Convert.ToString(dt.Rows[0]["secategory_name"].ToString()),
                    seRateName = Convert.ToString(dt.Rows[0]["serate_name"].ToString()),
                    seRateID = Convert.ToInt32(dt.Rows[0]["sequestions_groupid"]),

                };
                return model;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                SESubCategoryModel objMod = new SESubCategoryModel();
                return objMod; 
            }
            finally
            {
                con.Close();
            }
        }

        public string InsertSESubCategory(SESubCategoryModel SESubCategory)
        {
            try
            {
                string set = SESubCategory.seSubCategoryName.ToLower();
                if (set.Contains("'") == true)
                {
                    set = set.Replace("'", "''");
                }
                GetConnection();
                cmd = new SqlCommand("pr_asms_mst_SESubCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = set;
                cmd.Parameters.Add("@Catid", SqlDbType.VarChar).Value = SESubCategory.seCategoryID;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Exist";
                cmd.Parameters.Add("@rateid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@rategroupid", SqlDbType.VarChar).Value = 0;
                cmd.Parameters.Add("@queid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    CommonIUD INScomm = new CommonIUD();
                    string[,] codes = new string[,]
	               {
                        {"sequestions_question",CultureInfo.CurrentCulture.TextInfo.ToTitleCase(set.ToString()) },
                        {"sequestions_secategory_gid",SESubCategory.seCategoryID.ToString() },
	                    {"sequestions_groupid", SESubCategory.seRateID.ToString()},
                        {"sequestions_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },                                           
                        {"sequestions_insert_date",Convert.ToString(comm.GetCurrentDate())}
                  };
                    string tname = "[asms_mst_tsequestions]";
                    string insertcommend = comm.InsertCommon(codes, tname);
                    return null;
                }
                else { return res1; }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }

        public SESubCategoryModel DeleteSESubCategory(int SESubCategoryid)
        {
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_prodservice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prodservicegid", SESubCategoryid);
                cmd.Parameters.AddWithValue("@actionName", "SuppQuestion");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new SESubCategoryModel()
                {
                    seSubCategoryID = Convert.ToInt32(dt.Rows[0]["projectgid"])
                };
                return model;
                //CommonIUD delecomm = new CommonIUD();
                //string[,] codes = new string[,]
                //   {
                //         {"sequestions_isremoved", "Y"}	            
                //   };
                //string[,] whrcol = new string[,]
                //    {
                //        {"sequestions_gid", SESubCategoryid.ToString()}
                //    };
                //string tblname = "asms_mst_tsequestions";
                //string deletetbl = comm.DeleteCommon(codes, whrcol, tblname);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                SESubCategoryModel objMod = new SESubCategoryModel();
                return objMod;
            }
            finally
            {
                con.Close();
            }
        }

        public string EditSESubCategory(SESubCategoryModel SESubCategory)
        {
            CommonIUD delecomm = new CommonIUD();
            try
            {
                string set = SESubCategory.seSubCategoryName.ToLower();
                if (set.Contains("'") == true)
                {
                    set = set.Replace("'", "''");
                }
                GetConnection();
                cmd = new SqlCommand("pr_asms_mst_SESubCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = SESubCategory.seSubCategoryID;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = set;
                cmd.Parameters.Add("@Catid", SqlDbType.VarChar).Value = SESubCategory.seCategoryID;
                cmd.Parameters.Add("@rateid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@rategroupid", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@queid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Exist";
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();

                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
	                {
                        {"sequestions_question",CultureInfo.CurrentCulture.TextInfo.ToTitleCase(set.ToString())},
                        {"sequestions_secategory_gid",SESubCategory.seCategoryID.ToString() },
                        {"sequestions_groupid", SESubCategory.seRateID.ToString()},
                        {"sequestions_update_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },                                                                
                        {"sequestions_update_date",Convert.ToString(comm.GetCurrentDate())}


	                };
                    string[,] whrcol = new string[,]
	                {
                         {"sequestions_gid", SESubCategory.seSubCategoryID.ToString()}
            
                    };
                    string tblname = "asms_mst_tsequestions";

                    string updcomm = comm.UpdateCommon(codes, whrcol, tblname);
                    return null;
                }
                else { return res1; }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
            }
        }

        public IEnumerable<SESubCategoryModel> GetScoreByGrp(int id)
        {
            List<SESubCategoryModel> objSESubCategory = new List<SESubCategoryModel>();
            try
            {
               
                SESubCategoryModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_SESubCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@Catname", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@Catid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@rateid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@rategroupid", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@queid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectrate";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SESubCategoryModel();
                    objModel.seRatescore = Convert.ToString(dt.Rows[i]["serate_score"].ToString());
                    objModel.seRateName = Convert.ToString(dt.Rows[i]["serate_name"].ToString());
                    objSESubCategory.Add(objModel);
                }

                return objSESubCategory;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objSESubCategory; 
            }
            finally
            {
                con.Close();
            }
        }

        public IEnumerable<SErateModel> GetSErate()
        {
            List<SErateModel> objSESubCategory = new List<SErateModel>();
            try
            {
               
                SErateModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_SESubCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@Catname", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@Catid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@rateid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@rategroupid", SqlDbType.VarChar).Value = 0;
                cmd.Parameters.Add("@queid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectrategrp";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SErateModel();
                    objModel.seRateID = Convert.ToInt32(dt.Rows[i]["serate_gid"].ToString());
                    objModel.seRateName = Convert.ToString(dt.Rows[i]["serate_name"].ToString());
                    objSESubCategory.Add(objModel);
                }
                return objSESubCategory;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objSESubCategory; 
            }
            finally
            {
                con.Close();
            }
        }

        public IEnumerable<SErateModel> GetSEratedrop()
        {
            List<SErateModel> objSESubCategory = new List<SErateModel>();
            try
            {
               
                SErateModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_SESubCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "drop";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SErateModel();

                    objModel.seRateID = Convert.ToInt32(dt.Rows[i]["serate_gid"].ToString());
                    objModel.seRateName = Convert.ToString(dt.Rows[i]["serate_name"].ToString());
                    objSESubCategory.Add(objModel);
                }
                return objSESubCategory;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objSESubCategory; 
            }
            finally
            {
                con.Close();
            }
        }

        public string GetSECategoryId(int SECategoryId)
        {
            try
            {
                GetConnection();
                string[,] parameter = new string[,]
                {
                    {"@id",SECategoryId.ToString()},
                    {"@action","select1"},
                };
                return comm.ProcedureCommon(parameter, "pr_asms_mst_SECategory");
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return ""; 
            }
            finally
            {

            }
        }

        public string GetSErateById(int id)
        {
            try
            {
                GetConnection();
                string[,] parameter = new string[,]
                {
                    {"@rateid",id.ToString()},
                    {"@action","selectrategrp1"},
                };
                return comm.ProcedureCommon(parameter, "pr_asms_mst_SESubCategory");
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return ""; 
            }
            finally
            {
                con.Close();
            }
        }

        public string GetDocGrpID(int DocGrpID)
        {
            try
            {
                GetConnection();
                string[,] parameter = new string[,]
                {
                    {"@id",DocGrpID.ToString()},
                    {"@action","select1"},
                };
                return comm.ProcedureCommon(parameter, "pr_asms_mst_documentgroup");
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return ""; 
            }
            finally
            {
                con.Close();
            }
        }

        public string GetDocFrqID(int frqID)
        {
            try
            {
                GetConnection();
                string[,] parameter = new string[,]
                {
                    {"@id",frqID.ToString()},
                    {"@action","select1"},
                };
                return comm.ProcedureCommon(parameter, "pr_asms_mst_documentfrequency");
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return ""; 
            }
            finally
            {
                con.Close();
            }
        }

    }
}