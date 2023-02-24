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

namespace IEM.Areas.EOW.Models
{
    public class DeclarationNote : DeclarationNoteRepository
    {
        ErrorLog objErrorLog = new ErrorLog();
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions cmnfun = new CmnFunctions();
        private void GetCon()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }
        public IEnumerable<EOW_DeclarationNote> DisplayGrid()
        {
            List<EOW_DeclarationNote> objOrgType = new List<EOW_DeclarationNote>();
            try
            {
              
                EOW_DeclarationNote objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_declartionnoote", con);
                cmd.Parameters.AddWithValue("@action", "DisplayGrid");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                DataSet ds = new DataSet();
                dt.TableName = "DeclarationNote";
                ds.Tables.Add(dt);
                HttpContext.Current.Session["ExcelExportDeclarationNote"] = ds;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new EOW_DeclarationNote();
                    objModel.declnote_gid = Convert.ToInt32(dt.Rows[i]["Declaration Note Gid"].ToString());
                    objModel.declnote_name = Convert.ToString(dt.Rows[i]["Declartion Note Name"].ToString());
                    objModel.doctype_name = Convert.ToString(dt.Rows[i]["Doc Type"].ToString());
                    objModel.docsubtype_name = Convert.ToString(dt.Rows[i]["Doc SubType"].ToString());
                    //objModel.declnote_at = Convert.ToString(dt.Rows[i]["Declartion NoteAt"].ToString());
                    objModel.declnote_desc = Convert.ToString(dt.Rows[i]["Description"].ToString());
                    objModel.declnote_active = Convert.ToString(dt.Rows[i]["Active"].ToString());
                    objModel.declnote_periodfrom = Convert.ToString(dt.Rows[i]["Period From"].ToString());
                    objModel.declnote_periodto = Convert.ToString(dt.Rows[i]["Period To"].ToString());
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

        public IEnumerable<EOW_DeclarationNote> DisplayGrid(string delnotename, string docsubtype, string periodfrom, string periodto)
        {
            List<EOW_DeclarationNote> objOrgType = new List<EOW_DeclarationNote>();
            try
            {
             
                EOW_DeclarationNote objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_declartionnoote", con);
                cmd.Parameters.AddWithValue("@action", "SearchReport");
                cmd.Parameters.AddWithValue("@declnote_name", delnotename);
                cmd.Parameters.AddWithValue("@doctype_code", docsubtype);
                if(periodfrom!="")
                {
                    cmd.Parameters.AddWithValue("@periodfromdate",cmnfun.convertoDateTimeString(periodfrom));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@periodfromdate", periodfrom);
                }
                if(periodto!="")
                {
                    cmd.Parameters.AddWithValue("@periodtodate", cmnfun.convertoDateTimeString(periodto));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@periodtodate", periodto);
                }
                
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new EOW_DeclarationNote();
                    objModel.declnote_gid = Convert.ToInt32(dt.Rows[i]["declnote_gid"].ToString());
                    objModel.declnote_name = Convert.ToString(dt.Rows[i]["Declartion Note Name"].ToString());
                    objModel.doctype_name = Convert.ToString(dt.Rows[i]["Doc Type"].ToString());
                    objModel.docsubtype_name = Convert.ToString(dt.Rows[i]["Doc SubType"].ToString());
                    //objModel.declnote_at = Convert.ToString(dt.Rows[i]["Declartion NoteAt"].ToString());
                    objModel.declnote_desc = Convert.ToString(dt.Rows[i]["Description"].ToString());
                    objModel.declnote_active = Convert.ToString(dt.Rows[i]["Active"].ToString());
                    objModel.declnote_periodfrom = Convert.ToString(dt.Rows[i]["PeriodFrom"].ToString());
                    objModel.declnote_periodto = Convert.ToString(dt.Rows[i]["PeriodTo"].ToString());
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

        public IEnumerable<GetDoctype> GetDoctype()
        {
            List<GetDoctype> objCountrytype = new List<GetDoctype>();
            try
            {
               
                GetDoctype objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_declartionnoote", con);
                cmd.Parameters.AddWithValue("@action", "DocType");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new GetDoctype();
                    objModel.doctypegid = Convert.ToInt32(dt.Rows[i]["doctype_gid"].ToString());
                    objModel.doctypecode = Convert.ToString(dt.Rows[i]["doctype_code"].ToString());
                    objModel.doctypename = Convert.ToString(dt.Rows[i]["doctype_name"].ToString());
                    objCountrytype.Add(objModel);
                }
                return objCountrytype;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objCountrytype;
            }
            finally
            {
                con.Close();
            }
        }

        public IEnumerable<GetDocsubtype> GetDocsubtype(int doctypegid)
        {
            List<GetDocsubtype> objCountrytype = new List<GetDocsubtype>();
            //GetDocsubtype objModel;
            try
            {
                
               GetDocsubtype objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_declartionnoote", con);
                cmd.Parameters.AddWithValue("@action", "DocSubType");
                cmd.Parameters.AddWithValue("@gid",doctypegid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new GetDocsubtype();
                    objModel.docsubtypegid = Convert.ToInt32(dt.Rows[i]["docsubtype_gid"].ToString());
                    //objModel.doctype_code = Convert.ToString(dt.Rows[i]["doctype_code"].ToString());
                    objModel.docsubtypename = Convert.ToString(dt.Rows[i]["docsubtype_name"].ToString());
                    objCountrytype.Add(objModel);
                }
                return objCountrytype;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objCountrytype;
            }
            finally
            {
                con.Close();
            }
        }


        public string InsertDecNote(EOW_DeclarationNote decnote)
        {
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_declartionnoote", con);
                cmd.Parameters.AddWithValue("@action", "InsertExist");
                cmd.Parameters.AddWithValue("@doctype_code", decnote.doctype_code);
                cmd.Parameters.AddWithValue("@docsubtype_name", decnote.docsubtype_name);
                cmd.Parameters.AddWithValue("@periodfromdate", cmnfun.convertoDateTimeString(decnote.declnote_periodfrom));
                cmd.Parameters.AddWithValue("@periodtodate", cmnfun.convertoDateTimeString(decnote.declnote_periodto));
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["@res"].Value.ToString();
                if (result == "Not There")
                {
                    CommonIUD comm = new CommonIUD();
                    string[,] codes = new string[,]            
	                {
                        {"declnote_name",decnote.declnote_name},
                        {"declnote_docsubtype_gid",decnote.docsubtype_gid.ToString ()},
                        //{"declnote_at",Convert.ToString (decnote.declnote_at)},
                         {"declnote_onsubmission",Convert.ToString (decnote.declnote_onsubmission)},
                         {"declnote_approval",Convert.ToString (decnote.declnote_approval)},
                        {"declnote_desc",Convert.ToString (decnote.declnote_desc)},
                        {"declnote_active",decnote.declnote_active },
                        {"declnote_periodfrom",cmnfun.convertoDateTimeString (decnote.declnote_periodfrom)},
                        {"declnote_periodto",cmnfun.convertoDateTimeString (decnote.declnote_periodto)},
                        {"declnote_insert_by",cmnfun.GetLoginUserGid().ToString ()},
                        {"declnote_insert_date",comm.GetCurrentDate()}
                    };
                    string tname = "iem_mst_tdeclnote";
                    string insertcommend = comm.InsertCommon(codes, tname);
                    return insertcommend;
                }
                else
                {
                    return "Duplicate record !";
                }
                //return "Duplicate record !";
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
            return "success";
        }

        public string UpdateDecnote(EOW_DeclarationNote decnote)
        {
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_declartionnoote", con);
                cmd.Parameters.AddWithValue("@action", "UpdateExist");
                cmd.Parameters.AddWithValue("@doctype_code", decnote.doctype_code);
                cmd.Parameters.AddWithValue("@docsubtype_name", decnote.docsubtype_name);
                cmd.Parameters.AddWithValue("@periodfromdate", cmnfun.convertoDateTimeString(decnote.declnote_periodfrom));
                cmd.Parameters.AddWithValue("@periodtodate", cmnfun.convertoDateTimeString(decnote.declnote_periodto));
                cmd.Parameters.AddWithValue("@gid", decnote.declnote_gid);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["@res"].Value.ToString();
                if (result == "Not There")
                {
                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes = new string[,]
	                  {
                        {"declnote_name",decnote.declnote_name},
                        {"declnote_docsubtype_gid",decnote.docsubtype_gid.ToString ()},                    
                        {"declnote_onsubmission",Convert.ToString (decnote.declnote_onsubmission)},
                        {"declnote_approval",Convert.ToString (decnote.declnote_approval)},
                        {"declnote_desc",Convert.ToString (decnote.declnote_desc)},
                        {"declnote_active",decnote.declnote_active },
                        {"declnote_periodfrom",cmnfun.convertoDateTimeString (decnote.declnote_periodfrom)},
                        {"declnote_periodto",cmnfun.convertoDateTimeString (decnote.declnote_periodto)},
                        {"declnote_update_by",cmnfun.GetLoginUserGid().ToString ()},
                        {"declnote_update_date",delecomm.GetCurrentDate()}
	                  };
                    string[,] whrcol = new string[,]
	                 {
                          {"declnote_gid", decnote.declnote_gid.ToString()},                         
                     };

                    string updcomm = delecomm.UpdateCommon(codes, whrcol, "iem_mst_tdeclnote");
                }
                else
                {
                    return "Duplicate record !";
                }
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

            return "success";
        }

        public string DeleteNote(int declnotegid)
        {
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {

                string[,] codes = new string[,]
	       {
                 {"declnote_isremoved", "Y"}
	            
           };
                string[,] whrcol = new string[,]
	        {
                {"declnote_gid", declnotegid.ToString ()}
            };
                string tblname = "iem_mst_tdeclnote";

                string deletetbl = delecomm.DeleteCommon(codes, whrcol, tblname);
                return deletetbl;
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


        public EOW_DeclarationNote DisplayGridById(int id)
        {
            EOW_DeclarationNote objModel = new EOW_DeclarationNote();
            try
            {
                List<EOW_DeclarationNote> objOrgType = new List<EOW_DeclarationNote>();
              
                GetCon();
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_declartionnoote", con);
                cmd.Parameters.AddWithValue("@action", "DisplayGridById");
                cmd.Parameters.AddWithValue("@gid", id);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new EOW_DeclarationNote()
                {
                   // objModel = new EOW_DeclarationNote();
                    declnote_gid = Convert.ToInt32(dt.Rows[0]["declnote_gid"].ToString()),
                    declnote_name = Convert.ToString(dt.Rows[0]["Declartion Note Name"].ToString()),
                    doctype_name = Convert.ToString(dt.Rows[0]["Doc Type"].ToString()),
                    docsubtype_name = Convert.ToString(dt.Rows[0]["Doc SubType"].ToString()),
                    declnote_onsubmission = Convert.ToString(dt.Rows[0]["declnote_onsubmission"].ToString()),
                    declnote_approval = Convert.ToString(dt.Rows[0]["declnote_approval"].ToString()),
                    //declnote_at = Convert.ToString(dt.Rows[0]["Declartion NoteAt"].ToString()),
                    declnote_desc = Convert.ToString(dt.Rows[0]["Description"].ToString()),
                    declnote_active = Convert.ToString(dt.Rows[0]["Active"].ToString()),
                    declnote_periodfrom = Convert.ToString(dt.Rows[0]["PeriodFrom"].ToString()),
                    declnote_periodto = Convert.ToString(dt.Rows[0]["PeriodTo"].ToString()),
                    selecteddoctype_gid = Convert.ToInt32(dt.Rows[0]["doctype_gid"].ToString()),
                    selecteddocsubtype_gid = Convert.ToInt32(dt.Rows[0]["docsubtype_gid"].ToString())
                    //objOrgType.Add(objModel);
                };
                return model;

                //return objOrgType;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objModel;
            }
            finally
            {
                con.Close();
            }
        }


        public DataTable getdoctypecode(int doctypegid)
        {
            DataTable dt=new DataTable ();
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_declartionnoote", con);
                SqlDataAdapter da = new SqlDataAdapter();
                cmd.Parameters.AddWithValue("@action", "DocTypeById");
                cmd.Parameters.AddWithValue("@gid", doctypegid);              
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                da=new SqlDataAdapter (cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return dt;
            }
            finally
            {
                con.Close();
            }
            return dt;
        }

        public DataTable getdocsubtypename(int doctypegid)
        {
            DataTable dt = new DataTable();
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_declartionnoote", con);
                SqlDataAdapter da = new SqlDataAdapter();
                cmd.Parameters.AddWithValue("@action", "DocSubTypeById");
                cmd.Parameters.AddWithValue("@gid", doctypegid);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return dt;
            }
            finally
            {
                con.Close();
            }
            return dt;
        }
    }
}