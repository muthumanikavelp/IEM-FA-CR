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
    namespace IEM.Areas.MASTERS.Models
    {

        public class IEM_MST_EXPENSESUBCATEGORY : Iiem_mst_expensesubcategory
        {
            ErrorLog objErrorLog = new ErrorLog();
            SqlConnection con = new SqlConnection();
            SqlCommand cmd = new SqlCommand();
            SqlDataAdapter da = new SqlDataAdapter();
            string result;
            private void GetCon()
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                    con.Open();
                }
            }
            public IEnumerable<iem_mst_expensesubcategory> getexpsubcategory1(string expcat_code, string expcat_name, string naturegid, string expcatgid)
            {
                try
                {
                    List<iem_mst_expensesubcategory> objOrgType = new List<iem_mst_expensesubcategory>();
                    iem_mst_expensesubcategory objModel;
                    GetCon();
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("pr_iem_mst_expsubcat", con);
                    if (naturegid != "0" && expcatgid != "0")
                    {
                        cmd.Parameters.AddWithValue("@action", "selectwithexpcat");
                        cmd.Parameters.AddWithValue("@expsubcat_expcat_gid", expcatgid);
                        cmd.Parameters.AddWithValue("@expsubcat_gid", naturegid);
                    }

                    else if (naturegid != "0" && expcatgid == "0")
                    {
                        cmd.Parameters.AddWithValue("@action", "selectwithnature");
                        cmd.Parameters.AddWithValue("@expsubcat_gid", naturegid);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@action", "select");
                    }

                    cmd.Parameters.AddWithValue("@expsubcat_code", expcat_code);
                    cmd.Parameters.AddWithValue("@expsubname_name", expcat_name);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objModel = new iem_mst_expensesubcategory();
                        objModel.expsubcat_gid = Convert.ToInt32(dt.Rows[i]["expsubcat_gid"].ToString());
                        objModel.expsubcat_code = Convert.ToString(dt.Rows[i]["expsubcat_code"].ToString());
                        objModel.expsubcat_name = Convert.ToString(dt.Rows[i]["expsubcat_name"].ToString());
                        // objModel.expsubcat_help = Convert.ToString(dt.Rows[i]["expsubcat_help"].ToString());
                        objModel.expsubcat_active = Convert.ToString(dt.Rows[i]["expsubcat_active"].ToString());
                        objModel.expnature_name = Convert.ToString(dt.Rows[i]["expnature_name"].ToString());
                        objModel.expcat_name = Convert.ToString(dt.Rows[i]["expcat_name"].ToString());
                        objModel.GlNo = Convert.ToString(dt.Rows[i]["expcat_gl_no"].ToString());
                        objModel.hsn_code = Convert.ToString(dt.Rows[i]["hsn_code"].ToString());
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
            public IEnumerable<iem_mst_expensesubcategory> getexpsubcategory()
            {
                try
                {
                    List<iem_mst_expensesubcategory> objOrgType = new List<iem_mst_expensesubcategory>();
                    iem_mst_expensesubcategory objModel;
                    GetCon();
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("pr_iem_mst_expsubcat", con);
                    cmd.Parameters.AddWithValue("@action", "select");
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objModel = new iem_mst_expensesubcategory();
                        objModel.expsubcat_gid = Convert.ToInt32(dt.Rows[i]["expsubcat_gid"].ToString());
                        objModel.expsubcat_code = Convert.ToString(dt.Rows[i]["expsubcat_code"].ToString());
                        objModel.expsubcat_name = Convert.ToString(dt.Rows[i]["expsubcat_name"].ToString());
                        objModel.expsubcat_active = Convert.ToString(dt.Rows[i]["expsubcat_active"].ToString());
                        objModel.expnature_name = Convert.ToString(dt.Rows[i]["expnature_name"].ToString());
                        objModel.expcat_name = Convert.ToString(dt.Rows[i]["expcat_name"].ToString());
                        objModel.GlNo = Convert.ToString(dt.Rows[i]["expcat_gl_no"].ToString());
                        objModel.hsn_code = Convert.ToString(dt.Rows[i]["hsn_code"].ToString());
                        objModel.hsn_desc = Convert.ToString(dt.Rows[i]["hsndesc"].ToString());
                      
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
            public iem_mst_expensesubcategory GetexpsubcategoryById(int ExpsubId)
            {
                try
                {
                    List<iem_mst_expensesubcategory> objOrgType = new List<iem_mst_expensesubcategory>();
                    iem_mst_expensesubcategory objModel;
                    GetCon();
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("pr_iem_mst_expsubcat", con);
                    cmd.Parameters.AddWithValue("@action", "selectById");
                    cmd.Parameters.AddWithValue("@expsubcat_gid", ExpsubId);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    objModel = new iem_mst_expensesubcategory()
                    {
                        expsubcat_gid = Convert.ToInt32(dt.Rows[0]["expsubcat_gid"].ToString()),
                        expsubcat_code = Convert.ToString(dt.Rows[0]["expsubcat_code"].ToString()),
                        expsubcat_name = Convert.ToString(dt.Rows[0]["expsubcat_name"].ToString()),
                        //expsubcat_help = Convert.ToString(dt.Rows[0]["expnature_name"].ToString()),
                        expsubcat_active = Convert.ToString(dt.Rows[0]["expsubcat_active"].ToString()),
                        expnature_gid = Convert.ToInt32(dt.Rows[0]["expnature_gid"].ToString()),
                        expcat_name = Convert.ToString(dt.Rows[0]["expcat_name"].ToString()),
                        expcat_gid = Convert.ToInt32(dt.Rows[0]["expcat_gid"].ToString()),
                        hsn_code = Convert.ToString(dt.Rows[0]["hsn_code"].ToString()),
                        Hsn_gid = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[0]["hsn_gid"].ToString()) ? "0" : dt.Rows[0]["hsn_gid"].ToString()),
                        hsn_desc = Convert.ToString(dt.Rows[0]["hsndesc"].ToString()),
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

            public string Insertexpsubcategory(iem_mst_expensesubcategory Expsubcate, string hsn)
            {

                try
                {
                    int res;
                    string help = "";
                    CommonIUD comm = new CommonIUD();
                    SqlCommand cmd = new SqlCommand("pr_iem_mst_expsubcat", con);
                    GetCon();
                    cmd.Parameters.AddWithValue("@action", "insExist");
                    cmd.Parameters.AddWithValue("@expsubcat_expcat_gid", Expsubcate.expsubcat_expcat_gid);
                    cmd.Parameters.AddWithValue("@expsubcat_code", Expsubcate.expsubcat_code);
                    cmd.Parameters.AddWithValue("@expsubname_name", Expsubcate.expsubcat_name);
                    cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                    string res1 = cmd.Parameters["@res"].Value.ToString();
                    if (res1 == "Not There")
                    {
                        help = Convert.ToString(Expsubcate.expsubcat_help);
                        if (help == null)
                        {
                            help = "";
                        }
                  //      string[,] codes = new string[,]
                  // {
                  //      {"expsubcat_code",Expsubcate.expsubcat_code},
                  //      {"expsubcat_name",Expsubcate.expsubcat_name.ToString() },                      
                  //      {"expsubcat_help", help},                        
                  //      {"expsubcat_active", Expsubcate.expsubcat_active.ToString ()},
                  //      {"expsubcat_expnature_gid", Expsubcate.expsubcat_expnature_gid.ToString ()},
                  //      {"expsubcat_expcat_gid", Expsubcate.expsubcat_expcat_gid.ToString ()},
                  //      {"expsubcat_insert_by", Expsubcate.expsubcat_insert_by.ToString ()},  
                  //      {"expsubcat_insert_date","sysdatetime()"},  
                  //      {"expsubcat_hsngid",Expsubcate.Hsn_gid.ToString()}
                  //};
                  //      string tname = "iem_mst_texpsubcat";
                  //      string insertcommend = comm.InsertCommon(codes, tname);
                  //      result = "Success";

                        GetCon();
                        cmd = new SqlCommand("PR_FA_SET_EXPENSE", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@expsubcat_code", SqlDbType.VarChar).Value = Expsubcate.expsubcat_code;
                        cmd.Parameters.Add("@expsubcat_name", SqlDbType.VarChar).Value = Expsubcate.expsubcat_name.ToString();
                        cmd.Parameters.Add("@expsubcat_help", SqlDbType.VarChar).Value = help;
                        cmd.Parameters.Add("@expsubcat_active", SqlDbType.VarChar).Value =  Expsubcate.expsubcat_active.ToString ();
                        cmd.Parameters.Add("@expsubcat_expnature_gid", SqlDbType.VarChar).Value = Expsubcate.expsubcat_expnature_gid.ToString();
                        cmd.Parameters.Add("@expsubcat_expcat_gid", SqlDbType.VarChar).Value = Expsubcate.expsubcat_expcat_gid.ToString();
                        cmd.Parameters.Add("@expsubcat_insert_by", SqlDbType.VarChar).Value = Expsubcate.expsubcat_insert_by.ToString();
                        cmd.Parameters.Add("@expsubcat_insert_date", SqlDbType.VarChar).Value = "sysdatetime()";
                        cmd.Parameters.Add("@expsubcat_hsngid", SqlDbType.VarChar).Value = hsn.ToString();
                        cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "create";
                        res = cmd.ExecuteNonQuery();

                        if (res == -1)
                        {
                            result = "success";
                        }

                    }
                    else
                    {
                        result = "Duplicate Record! : Expense Sub Category Code And Name Already Exits";
                    }

                }
                catch (Exception ex)
                {
                    objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                }
                finally
                {
                    con.Close();
                }
                return result;
            }
            public string Deletexpsubcategory(int ExpCatId)
            {
                CommonIUD delecomm = new CommonIUD();
                string col_value = string.Empty;
                string col_temp = string.Empty;
                int res;
                try
                {

           //         string[,] codes = new string[,]
           //{
           //      {"expsubcat_isremoved", "Y"}
	            
           //};
           //         string[,] whrcol = new string[,]
           // {
           //     {"expsubcat_gid", ExpCatId.ToString ()}
           // };
           //         string tblname = "iem_mst_texpsubcat";


           //         string deletetbl = delecomm.DeleteCommon(codes, whrcol, tblname);
           //         result = "Success";


                    GetCon();
                    cmd = new SqlCommand("PR_FA_SET_EXPENSE", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@expenseid", SqlDbType.VarChar).Value = ExpCatId;
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "delete";
                    res = cmd.ExecuteNonQuery();

                    if (res == -1)
                        result = "success";

                }
                catch (Exception ex)
                {
                    objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
                return result;
            }

            public string Updatexpsubcategory(iem_mst_expensesubcategory ExpsubCat, string hsn)
            {
                try
                {
                    //CommonIUD commname = new CommonIUD();
                    //SqlCommand cmdname = new SqlCommand("pr_iem_mst_expsubcat", con);
                    //GetCon();
                    //cmdname.Parameters.AddWithValue("@action", "Existname");
                    //cmdname.Parameters.AddWithValue("@expsubcat_gid", ExpsubCat.expsubcat_gid);
                    //cmdname.Parameters.AddWithValue("@expsubcat_code", ExpsubCat.expsubcat_code);
                    //cmdname.Parameters.AddWithValue("@expsubname_name", ExpsubCat.expsubcat_name);
                    //cmdname.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                    //cmdname.CommandType = CommandType.StoredProcedure;
                    //cmdname.ExecuteNonQuery();
                    //string rescmdname = cmdname.Parameters["@res"].Value.ToString();
                    //if (rescmdname == "Not There")
                    //{
                    //CommonIUD commcode = new CommonIUD();
                    //SqlCommand cmdcode = new SqlCommand("pr_iem_mst_expsubcat", con);
                    //GetCon();
                    //cmdcode.Parameters.AddWithValue("@action", "Existcode");
                    //cmdcode.Parameters.AddWithValue("@expsubcat_gid", ExpsubCat.expsubcat_gid);
                    //cmdcode.Parameters.AddWithValue("@expsubcat_code", ExpsubCat.expsubcat_code);
                    //cmdcode.Parameters.AddWithValue("@expsubname_name", ExpsubCat.expsubcat_name);
                    //cmdcode.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                    //cmdcode.CommandType = CommandType.StoredProcedure;
                    //cmdcode.ExecuteNonQuery();
                    //string rescmdcode = cmdcode.Parameters["@res"].Value.ToString();
                    //if (rescmdcode == "Not There")
                    //{
                    int res;
                    CommonIUD comm = new CommonIUD();
                    SqlCommand cmdcodename = new SqlCommand("pr_iem_mst_expsubcat", con);
                    GetCon();
                    cmdcodename.Parameters.AddWithValue("@action", "Exist");
                    cmdcodename.Parameters.AddWithValue("@expsubcat_gid", ExpsubCat.expsubcat_gid);
                    cmdcodename.Parameters.AddWithValue("@expsubcat_expcat_gid", ExpsubCat.selectedexpcat_gid);
                    cmdcodename.Parameters.AddWithValue("@expsubcat_code", ExpsubCat.expsubcat_code);
                    cmdcodename.Parameters.AddWithValue("@expsubname_name", ExpsubCat.expsubcat_name);
                    cmdcodename.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                    cmdcodename.CommandType = CommandType.StoredProcedure;
                    cmdcodename.ExecuteNonQuery();
                    string rescmdcodename = cmdcodename.Parameters["@res"].Value.ToString();
                    if (rescmdcodename == "Not There")
                    {
                        //CommonIUD delecomm = new CommonIUD();
                  //      string[,] codes = new string[,]
                  // {
                  //      {"expsubcat_code",ExpsubCat.expsubcat_code},
                  //      {"expsubcat_name",ExpsubCat.expsubcat_name.ToString() },	                    
                  //      {"expsubcat_active", ExpsubCat.expsubcat_active.ToString ()},
                  //      {"expsubcat_expnature_gid",ExpsubCat.selectedexpsubcat_expnature_gid.ToString ()},
                  //      {"expsubcat_expcat_gid",ExpsubCat.selectedexpcat_gid.ToString ()},
                  //      {"expsubcat_update_by",ExpsubCat.expsubcat_update_by.ToString ()},  
                  //      {"expsubcat_update_date", "sysdatetime()"},
                  //      {"expsubcat_hsngid",ExpsubCat.Hsn_gid.ToString()}
                  //};
                  //      string[,] whrcol = new string[,]
                  //   {
                  //        {"expsubcat_gid", ExpsubCat.expsubcat_gid.ToString ()}
            
                  //   };
                  //      string tblname = "iem_mst_texpsubcat";

                  //      string updcomm = delecomm.UpdateCommon(codes, whrcol, tblname);

                        GetCon();
                        cmd = new SqlCommand("PR_FA_SET_EXPENSE", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@expenseid", SqlDbType.VarChar).Value = ExpsubCat.expsubcat_gid;
                        cmd.Parameters.Add("@expsubcat_code", SqlDbType.VarChar).Value = ExpsubCat.expsubcat_code;
                        cmd.Parameters.Add("@expsubcat_name", SqlDbType.VarChar).Value = ExpsubCat.expsubcat_name.ToString();
                        cmd.Parameters.Add("@expsubcat_active", SqlDbType.VarChar).Value = ExpsubCat.expsubcat_active.ToString();
                        cmd.Parameters.Add("@expsubcat_expnature_gid", SqlDbType.VarChar).Value = ExpsubCat.selectedexpsubcat_expnature_gid.ToString();
                        cmd.Parameters.Add("@expsubcat_expcat_gid", SqlDbType.VarChar).Value = ExpsubCat.selectedexpcat_gid.ToString();
                        cmd.Parameters.Add("@expsubcat_insert_by", SqlDbType.VarChar).Value = ExpsubCat.expsubcat_update_by.ToString();
                        cmd.Parameters.Add("@expsubcat_insert_date", SqlDbType.VarChar).Value = "sysdatetime()";
                        cmd.Parameters.Add("@expsubcat_hsngid", SqlDbType.VarChar).Value = hsn.ToString();
                        cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "update";
                        res = cmd.ExecuteNonQuery();

                        if (res == -1)
                        {
                            result = "success";
                        }


                      
                    }
                    else
                    {
                        result = "Duplicate Record! : Expense Sub Category Code And Name Already Exits";
                    }
                    //}
                    //}
                    //else
                    //{
                    //    result = " Duplicate Record! : Expense Sub Category Code Already Exits";
                    //}

                }
                catch (Exception ex)
                {
                    objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                }
                finally
                {

                }
                return result;
            }

            public IEnumerable<Getexpnature> Getexpnature()
            {
                try
                {
                    List<Getexpnature> objCountrytype = new List<Getexpnature>();
                    Getexpnature objModel;
                    GetCon();
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("pr_iem_mst_expnature", con);
                    cmd.Parameters.AddWithValue("@action", "select");
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objModel = new Getexpnature();
                        objModel.expnature_gid = Convert.ToInt32(dt.Rows[i]["expnature_gid"].ToString());
                        objModel.expnature_name = Convert.ToString(dt.Rows[i]["expnature_name"].ToString());

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
                //  throw new NotImplementedException();
            }

            public GetexpnatureById GetexpnatureById(int ExpNatureId)
            {
                try
                {
                    List<GetexpnatureById> objOrgType = new List<GetexpnatureById>();
                    GetexpnatureById objModel;
                    GetCon();
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("pr_iem_mst_gl", con);
                    cmd.Parameters.AddWithValue("@action", "selectById");
                    cmd.Parameters.AddWithValue("@gid", ExpNatureId);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    objModel = new GetexpnatureById()
                    {
                        expnature_gid = Convert.ToInt32(dt.Rows[0]["expnature_gid"].ToString()),
                        expnature_name = Convert.ToString(dt.Rows[0]["expnature_name"].ToString()),

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


            public IEnumerable<Getexpcat> Getexpcat()
            {
                try
                {
                    List<Getexpcat> objCountrytype = new List<Getexpcat>();
                    Getexpcat objModel;
                    GetCon();
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("pr_iem_mst_expcat", con);
                    cmd.Parameters.AddWithValue("@action", "select");
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objModel = new Getexpcat();
                        objModel.expcat_gid = Convert.ToInt32(dt.Rows[i]["expcat_gid"].ToString());
                        objModel.expcat_name = Convert.ToString(dt.Rows[i]["expcat_name"].ToString());

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
            public IEnumerable<GetexpcatById> GetexpcatBy_Id(int ExpCatId)
            {
                try
                {
                    List<GetexpcatById> objOrgType = new List<GetexpcatById>();
                    GetexpcatById objModel;
                    GetCon();
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("pr_iem_mst_expcat", con);
                    cmd.Parameters.AddWithValue("@action", "selectBy_Id");
                    cmd.Parameters.AddWithValue("@gid", ExpCatId);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            objModel = new GetexpcatById();
                            objModel.expcat_gid = Convert.ToInt32(dt.Rows[i]["expcat_gid"].ToString());
                            objModel.expcat_name = Convert.ToString(dt.Rows[i]["expcat_name"].ToString());
                            objOrgType.Add(objModel);

                        }
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
                //  throw new NotImplementedException();
            }
            public GetexpcatById GetexpcatById(int ExpCatId)
            {
                try
                {
                    List<GetexpcatById> objOrgType = new List<GetexpcatById>();
                    GetexpcatById objModel;
                    GetCon();
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("pr_iem_mst_expcat", con);
                    cmd.Parameters.AddWithValue("@action", "selectById");
                    cmd.Parameters.AddWithValue("@gid", ExpCatId);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    objModel = new GetexpcatById()
                    {
                        expcat_gid = Convert.ToInt32(dt.Rows[0]["expcat_gid"].ToString()),
                        expcat_name = Convert.ToString(dt.Rows[0]["expcat_name"].ToString()),

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
                //  throw new NotImplementedException();
            }
            #region // add by sugumar HSN code
            public IEnumerable<GetHsncode> GetHsncode()
            {
                try
                {
                    List<GetHsncode> objhsttype = new List<GetHsncode>();
                    GetHsncode objModel;
                    GetCon();
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("pr_iem_mst_expnature", con);
                    cmd.Parameters.AddWithValue("@action", "Gethsnlist");
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objModel = new GetHsncode();
                        objModel.Hsn_gid = Convert.ToInt32(dt.Rows[i]["hsn_gid"].ToString());
                        objModel.hsn_code = Convert.ToString(dt.Rows[i]["hsn_code"].ToString());
                        objModel.hsn_desc = dt.Rows[i]["hsn_desc"].ToString();

                        objhsttype.Add(objModel);
                    }

                    return objhsttype;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
                //  throw new NotImplementedException();
            }
            #endregion

            #region // add by sugumar for HSN Desc
            public string Gethsndesc(string Exphsn)
            {
                string hsn_desc = string.Empty;
                try
                {
                    GetCon();
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("pr_ifams_Gethsncode", con);
                    cmd.Parameters.AddWithValue("@action", "Getdesc");
                    cmd.Parameters.AddWithValue("@hsn_id", Exphsn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            hsn_desc = hsn_desc + dt.Rows[i]["hsn_description"].ToString();
                        }
                    }
                    
                }
                   
                catch (Exception ex)
                {

                }
                return hsn_desc;
            }
            #endregion


            public List<SelectListItem> GetAllHsn()
            {
                try
                {
                    List<SelectListItem> items = new List<SelectListItem>();
                    GetCon();
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("pr_ifams_Gethsncode", con);
                    cmd.Parameters.AddWithValue("@action", "getcombo");
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        items.Add(new SelectListItem
                        {
                            Text = sdr["hsn_code"].ToString(),
                            Value = sdr["hsn_gid"].ToString(),

                        });
                    }
                    return items;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
                //  throw new NotImplementedException();
            }




            #region //Multiple hsn code select
            public List<SelectListItem> GetHsnforexpense(string id)
            {
                try
                {
                    List<SelectListItem> items = new List<SelectListItem>();
                    GetCon();
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("pr_ifams_Gethsncode", con);
                    cmd.Parameters.AddWithValue("@action", "gethsnexpense");
                    cmd.Parameters.AddWithValue("@hsn_id", id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataReader sdr = cmd.ExecuteReader();
                    while (sdr.Read())
                    {
                        items.Add(new SelectListItem
                        {
                            Text = sdr["hsn_code"].ToString(),
                            Value = sdr["hsn_gid"].ToString(),

                        });
                    }
                    return items;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
                //  throw new NotImplementedException();
            }
            #endregion

            public string GethsndescEXPENSE(string Exphsn)
            {
                string hsn_desc = string.Empty;
                try
                {
                    GetCon();
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("pr_ifams_Gethsncode", con);
                    cmd.Parameters.AddWithValue("@action", "MULTIHSNDESCEXPENSE");
                    cmd.Parameters.AddWithValue("@hsn_id", Exphsn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows.Count > 0)
                        {
                            hsn_desc = hsn_desc + dt.Rows[i]["hsn_description"].ToString();
                        }
                    }

                }

                catch (Exception ex)
                {

                }
                return hsn_desc;
            }

        }
    }
}