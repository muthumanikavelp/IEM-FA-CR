using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IEM.Common;
using IEM.Areas.MASTERS.Models;
using System.Web.Helpers;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;


namespace IEM.Areas.MASTERS.Models
{
    public class IEM_GST_TAX : iem_mst_tgsttax
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        string result;
        private void GetCon()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }
        public IEnumerable<Entity_taxgst> getgsttax(string sqlaction)
        {
            try
            {
                List<Entity_taxgst> objOrgType = new List<Entity_taxgst>();
                Entity_taxgst objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("iem_mst_tgetgsttax", con);
                cmd.Parameters.AddWithValue("@action", sqlaction);
                cmd.Parameters.AddWithValue("@userid", Convert.ToInt32(objCmnFunctions.GetLoginUserGid().ToString()));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new Entity_taxgst();
                    objModel.Taxgst_gid = Convert.ToInt16(dt.Rows[i]["Taxgst_gid"]);
                    objModel.Taxsubtypecode = dt.Rows[i]["taxsubtype_code"].ToString();
                    objModel.Taxhsncode = dt.Rows[i]["hsncode"].ToString();
                    objModel.Statename = dt.Rows[i]["state_code"].ToString();
                    objModel.Rate = Convert.ToDecimal(dt.Rows[i]["Rate"]);
                    objModel.Taxglno = dt.Rows[i]["gl_no"].ToString();
                    objModel.IpcdGLno = dt.Rows[i]["criditGL"].ToString();
                    objModel.RcGLno = dt.Rows[i]["ReveGL"].ToString();
                    objModel.Vwstatus = dt.Rows[i]["Status"].ToString();
                    objModel.Remarks = dt.Rows[i]["Remarks"].ToString();
                    objOrgType.Add(objModel);
                }
                return objOrgType;
            }
            catch (Exception ex)
            {
                ErrorLog objErrorLog = new ErrorLog();
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw ex;
            }
            finally
            {
                con.Close();
            }
            //throw new NotImplementedException();
        }
        public IEnumerable<Entity_taxgst> getgsttaxbyHSN(string sqlaction, string HSNCode)
        {
            try
            {
                List<Entity_taxgst> objOrgType = new List<Entity_taxgst>();
                Entity_taxgst objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("iem_mst_tgetgsttax", con);
                cmd.Parameters.AddWithValue("@action", sqlaction);
                cmd.Parameters.AddWithValue("@userid", Convert.ToInt32(objCmnFunctions.GetLoginUserGid().ToString()));
                cmd.Parameters.AddWithValue("@HSNCode", HSNCode);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new Entity_taxgst();
                    objModel.Taxgst_gid = Convert.ToInt16(dt.Rows[i]["Taxgst_gid"]);
                    objModel.Taxsubtypecode = dt.Rows[i]["taxsubtype_code"].ToString();
                    objModel.Taxhsncode = dt.Rows[i]["hsncode"].ToString();
                    objModel.Statename = dt.Rows[i]["state_code"].ToString();
                    objModel.Rate = Convert.ToDecimal(dt.Rows[i]["Rate"]);
                    objModel.Taxglno = dt.Rows[i]["gl_no"].ToString();
                    objModel.IpcdGLno = dt.Rows[i]["criditGL"].ToString();
                    objModel.RcGLno = dt.Rows[i]["ReveGL"].ToString();
                    objModel.Vwstatus = dt.Rows[i]["Status"].ToString();
                    objModel.Remarks = dt.Rows[i]["Remarks"].ToString();
                    objOrgType.Add(objModel);
                }
                return objOrgType;
            }
            catch (Exception ex)
            {
                ErrorLog objErrorLog = new ErrorLog();
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw ex;
            }
            finally
            {
                con.Close();
            }
            //throw new NotImplementedException();
        }
        public string GetRolegroup()
        {
            string result = string.Empty;
            try
            {
                GetCon();
                cmd = new SqlCommand("fb_taxgst_checkUserForGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", "selectTaxGstmak");
                cmd.Parameters.AddWithValue("@logingid", Convert.ToInt32(objCmnFunctions.GetLoginUserGid().ToString()));
                result = (string)cmd.ExecuteScalar();
                return result;
            }
            catch (Exception ex)
            {
                ErrorLog objErrorLog = new ErrorLog();
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
        }
        public IEnumerable<GetTaxsubtype> Gettaxsubtype()
        {
            try
            {
                List<GetTaxsubtype> objCountrytype = new List<GetTaxsubtype>();
                GetTaxsubtype objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("iem_mst_tgetgsttax", con);
                cmd.Parameters.AddWithValue("@action", "taxsubtype");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new GetTaxsubtype();
                    objModel.Taxsubtypeid = Convert.ToInt32(dt.Rows[i]["taxsubtype_gid"].ToString());
                    objModel.taxsubtype_code = Convert.ToString(dt.Rows[i]["taxsubtype_code"].ToString());

                    objCountrytype.Add(objModel);
                }

                return objCountrytype;
            }
            catch (Exception ex)
            {
                ErrorLog objErrorLog = new ErrorLog();
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw ex;
            }
            finally
            {
                con.Close();
            }
            //  throw new NotImplementedException();
        }
        public IEnumerable<GetHsncod> GetHsncode()
        {
            try
            {
                List<GetHsncod> objCountrytype = new List<GetHsncod>();
                GetHsncod objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("iem_mst_tgetgsttax", con);
                cmd.Parameters.AddWithValue("@action", "gethsncode");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new GetHsncod();
                    objModel.hsnid = Convert.ToInt32(dt.Rows[i]["hsn_gid"].ToString());
                    objModel.Hsn_code = Convert.ToString(dt.Rows[i]["hsn_code"].ToString());

                    objCountrytype.Add(objModel);
                }

                return objCountrytype;
            }
            catch (Exception ex)
            {
                ErrorLog objErrorLog = new ErrorLog();
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw ex;
            }
            finally
            {
                con.Close();
            }
            //  throw new NotImplementedException();
        }
        public IEnumerable<Getstatelist> GetStatecode()
        {
            try
            {
                List<Getstatelist> objCountrytype = new List<Getstatelist>();
                Getstatelist objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("iem_mst_tgetgsttax", con);
                cmd.Parameters.AddWithValue("@action", "getstate");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new Getstatelist();
                    objModel.stategid = Convert.ToInt32(dt.Rows[i]["state_gid"].ToString());
                    objModel.State_code = Convert.ToString(dt.Rows[i]["state_name"].ToString());

                    objCountrytype.Add(objModel);
                }

                return objCountrytype;
            }
            catch (Exception ex)
            {
                ErrorLog objErrorLog = new ErrorLog();
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw ex;
            }
            finally
            {
                con.Close();
            }
            //  throw new NotImplementedException();
        }
        public IEnumerable<Getgllist> GetGLdtl()
        {
            try
            {
                List<Getgllist> objCountrytype = new List<Getgllist>();
                Getgllist objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("iem_mst_tgetgsttax", con);
                cmd.Parameters.AddWithValue("@action", "getglcode");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new Getgllist();
                    objModel.Taxgl_id = Convert.ToInt32(dt.Rows[i]["gl_gid"].ToString());
                    objModel.Glno = Convert.ToString(dt.Rows[i]["gl_no"].ToString());

                    objCountrytype.Add(objModel);
                }

                return objCountrytype;
            }
            catch (Exception ex)
            {
                ErrorLog objErrorLog = new ErrorLog();
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw ex;
            }
            finally
            {
                con.Close();
            }
            //  throw new NotImplementedException();
        }
        public IEnumerable<GetglCr> GetCLtl()
        {
            try
            {
                List<GetglCr> objCountrytype = new List<GetglCr>();
                GetglCr objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("iem_mst_tgetgsttax", con);
                cmd.Parameters.AddWithValue("@action", "getglcode");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new GetglCr();
                    objModel.inputcredit_gid = Convert.ToInt32(dt.Rows[i]["gl_gid"].ToString());
                    objModel.CrGlno = Convert.ToString(dt.Rows[i]["gl_no"].ToString());

                    objCountrytype.Add(objModel);
                }

                return objCountrytype;
            }
            catch (Exception ex)
            {
                ErrorLog objErrorLog = new ErrorLog();
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw ex;
            }
            finally
            {
                con.Close();
            }
            //  throw new NotImplementedException();
        }
        public IEnumerable<GetglRe> GetRLdtl()
        {
            try
            {
                List<GetglRe> objCountrytype = new List<GetglRe>();
                GetglRe objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("iem_mst_tgetgsttax", con);
                cmd.Parameters.AddWithValue("@action", "getglcode");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new GetglRe();
                    objModel.reversecharge_glid = Convert.ToInt32(dt.Rows[i]["gl_gid"].ToString());
                    objModel.RcGlno = Convert.ToString(dt.Rows[i]["gl_no"].ToString());

                    objCountrytype.Add(objModel);
                }

                return objCountrytype;
            }
            catch (Exception ex)
            {
                ErrorLog objErrorLog = new ErrorLog();
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw ex;
            }
            finally
            {
                con.Close();
            }
            //  throw new NotImplementedException();
        }
        #region // add by sugumar for HSN Desc
        public string Gethsndesc(Entity_taxgst Exphsn)
        {
            string hsn_desc = string.Empty;
            try
            {
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_Gethsncode", con);
                cmd.Parameters.AddWithValue("@action", "Getdesc");
                cmd.Parameters.AddWithValue("@hsn_id", Exphsn.hsnid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    hsn_desc = dt.Rows[0]["hsn_description"].ToString();
                }

            }

            catch (Exception ex)
            {
                ErrorLog objErrorLog = new ErrorLog();
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return hsn_desc;
        }
        #endregion
        #region
        public string savegst(Entity_taxgst savegst)
        {
            string Msg = string.Empty;
            try
            {
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("iem_mst_SaveTax_sp", con);
                cmd.Parameters.AddWithValue("@action", "INSERT");
                cmd.Parameters.AddWithValue("@Taxsubtypeid", savegst.Taxsubtypeid);
                cmd.Parameters.AddWithValue("@hsnid", savegst.hsnid);
                cmd.Parameters.AddWithValue("@stategid", savegst.stategid);
                cmd.Parameters.AddWithValue("@Active", savegst.Active);
                cmd.Parameters.AddWithValue("@Rate", Convert.ToDecimal(savegst.Rate));
                cmd.Parameters.AddWithValue("@inputcredit", Convert.ToDecimal(savegst.inputcredit));
                cmd.Parameters.AddWithValue("@inputcredit_gid", savegst.inputcredit_gid);
                cmd.Parameters.AddWithValue("@effective_date", Convert.ToDateTime(savegst.effective_date));
                cmd.Parameters.AddWithValue("@reverse_charge", Convert.ToDecimal(savegst.reverse_charge));
                cmd.Parameters.AddWithValue("@reversecharge_glid", savegst.reversecharge_glid);
                cmd.Parameters.AddWithValue("@effective_todate", Convert.ToDateTime(savegst.effective_todate));
                cmd.Parameters.AddWithValue("@insertedby", Convert.ToInt32(objCmnFunctions.GetLoginUserGid().ToString()));
                cmd.Parameters.AddWithValue("@Status", Convert.ToInt32(savegst.Status));
                cmd.Parameters.AddWithValue("@Approvalstatus", Convert.ToInt32(savegst.Approvalstatus));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Msg = dt.Rows[0]["ErrorMessage"].ToString();
                }

            }

            catch (Exception ex)
            {
                ErrorLog objErrorLog = new ErrorLog();
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Msg;
        }
        #endregion

        #region
        public Entity_taxgst Geteditdel(int taxgstid)
        {
            try
            {
                List<Entity_taxgst> Objlist = new List<Entity_taxgst>();
                Entity_taxgst Objmodel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("iem_mst_tgetgsttax", con);
                cmd.Parameters.AddWithValue("@action", "getdedit");
                cmd.Parameters.AddWithValue("@Taxgst_gid", taxgstid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                Objmodel = new Entity_taxgst()
                {
                    Taxgst_gid = Convert.ToInt16(dt.Rows[0]["Taxgst_gid"]),
                    Taxsubtypecode = dt.Rows[0]["taxsubtype_code"].ToString(),
                    Taxhsncode = dt.Rows[0]["hsncode"].ToString(),
                    Statename = dt.Rows[0]["state_code"].ToString(),
                    Rate = Convert.ToDecimal(dt.Rows[0]["Rate"]),
                    Taxglno = dt.Rows[0]["gl_no"].ToString(),
                    Taxsubtypeid = Convert.ToInt32(dt.Rows[0]["Taxsubtypeid"].ToString()),
                    stategid = Convert.ToInt32(dt.Rows[0]["stategid"].ToString()),
                    hsnid = Convert.ToInt32(dt.Rows[0]["hsnid"].ToString()),
                    Active = (dt.Rows[0]["TaxgstActive"].ToString()),
                    Hsn_desc = dt.Rows[0]["Hsn_desc"].ToString(),
                    inputcredit = Convert.ToDecimal(dt.Rows[0]["inputcredit"]),
                    reverse_charge = Convert.ToDecimal(dt.Rows[0]["reverse_charge"]),
                    effective_date = Convert.ToString(dt.Rows[0]["effective_date"]),
                    effective_todate = Convert.ToString(dt.Rows[0]["effective_todate"]),
                    inputcredit_gid = Convert.ToInt32(dt.Rows[0]["inputcredit_gid"]),
                    reversecharge_glid = Convert.ToInt32(dt.Rows[0]["reversecharge_glid"]),

                };
                return Objmodel;
            }
            catch (Exception ex)
            {
                ErrorLog objErrorLog = new ErrorLog();
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw ex;
            }
        }
        #endregion
        #region
        public string Updategsttax(Entity_taxgst savegst)
        {
            string Msg = string.Empty;
            try
            {
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("iem_mst_SaveTax_sp", con);
                cmd.Parameters.AddWithValue("@action", "UPDATE");
                cmd.Parameters.AddWithValue("@Taxgst_gid", savegst.Taxgst_gid);
                cmd.Parameters.AddWithValue("@Taxsubtypeid", savegst.Taxsubtypeid);
                cmd.Parameters.AddWithValue("@hsnid", savegst.hsnid);
                cmd.Parameters.AddWithValue("@stategid", savegst.stategid);
                cmd.Parameters.AddWithValue("@Taxgl_id", savegst.Active);
                cmd.Parameters.AddWithValue("@Rate", Convert.ToDecimal(savegst.Rate));
                cmd.Parameters.AddWithValue("@inputcredit", Convert.ToDecimal(savegst.inputcredit));
                cmd.Parameters.AddWithValue("@inputcredit_gid", savegst.inputcredit_gid);
                cmd.Parameters.AddWithValue("@effective_date", Convert.ToDateTime(savegst.effective_date));
                cmd.Parameters.AddWithValue("@reverse_charge", Convert.ToDecimal(savegst.reverse_charge));
                cmd.Parameters.AddWithValue("@reversecharge_glid", savegst.reversecharge_glid);
                cmd.Parameters.AddWithValue("@effective_todate", Convert.ToDateTime(savegst.effective_todate));
                cmd.Parameters.AddWithValue("@Remarks", (savegst.Remarks));
                cmd.Parameters.AddWithValue("@Status", Convert.ToInt32(savegst.Status));
                cmd.Parameters.AddWithValue("@Approvalstatus", Convert.ToInt32(savegst.Approvalstatus));
                cmd.Parameters.AddWithValue("@insertedby", Convert.ToInt32(objCmnFunctions.GetLoginUserGid().ToString()));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Msg = dt.Rows[0]["ErrorMessage"].ToString();
                }

            }

            catch (Exception ex)
            {
                ErrorLog objErrorLog = new ErrorLog();
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Msg;
        }
        #endregion

        #region
        public string Deletegsttax(Entity_taxgst savegst)
        {
            string Msg = string.Empty;
            try
            {
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("iem_mst_SaveTax_sp", con);
                cmd.Parameters.AddWithValue("@action", "DELETE");
                cmd.Parameters.AddWithValue("@Taxgst_gid", savegst.Taxgst_gid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Msg = dt.Rows[0]["ErrorMessage"].ToString();
                }
                return Msg;
            }
            catch (Exception ex)
            {
                ErrorLog objErrorLog = new ErrorLog();
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw ex;
            }
        }
        #endregion
        //---pandiaraj
        #region
        public DataTable uploadGSTTAX(string XmlData, string filename)
        {
            DataTable dt = new DataTable();
            GetCon();
            cmd = new SqlCommand("pr_Upload_GST_TAX", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UPLOAD_FILENAME", filename.Trim());
            cmd.Parameters.AddWithValue("@statement", "I");
            cmd.Parameters.AddWithValue("@UPLOAD_BY", Convert.ToInt32(objCmnFunctions.GetLoginUserGid().ToString()));
            cmd.Parameters.AddWithValue("@XmlData", XmlData);

            //  result = (string)cmd.ExecuteScalar();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        public DataSet forexcel(int uploadgid, string type)
        {
            DataSet ds = new DataSet();
            GetCon();
            cmd = new SqlCommand("pr_Upload_GST_TAX", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@statement", type);
            cmd.Parameters.AddWithValue("@uploadgid", uploadgid);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            return ds;
        }

        public IEnumerable<Entity_taxgst> uploadlist()
        {
            try
            {
                List<Entity_taxgst> objOrgType = new List<Entity_taxgst>();
                Entity_taxgst objModel;
                GetCon();
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                cmd = new SqlCommand("pr_Upload_GST_TAX", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@XmlData", "");
                cmd.Parameters.AddWithValue("@statement", "L");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                dt = ds.Tables[0];
                //String roles = (String)HttpContext.Current.Session["rolegroup"];
                //if (roles == "TXGSTCHK")
                //{

                //}
                if (dt.Rows.Count > 0)
                //dt = dt.Select("UPLOAD_STATUS=1").CopyToDataTable();
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objModel = new Entity_taxgst();

                        objModel.upload_sno = Convert.ToInt16(dt.Rows[i]["sno"]);
                        objModel.uploadgid = Convert.ToInt16(dt.Rows[i]["upload_gid"]);
                        objModel.uploadno = dt.Rows[i]["upload_no"].ToString();
                        objModel.uploaddate = dt.Rows[i]["upload_date"].ToString();
                        objModel.upload_filename = dt.Rows[i]["upload_filename"].ToString();
                        objModel.upload_count = Convert.ToInt32(dt.Rows[i]["upload_records"]);
                        objModel.Status = dt.Rows[i]["UPLOAD_STATUS"].ToString().Trim();
                        objModel.Remarks = dt.Rows[i]["UPLOAD_REMARKS"].ToString().Trim();
                        objModel.Approvalstatus = dt.Rows[i]["STATUS_NAME"].ToString().Trim();
                        objOrgType.Add(objModel);
                    }
                }
                return objOrgType;
            }
            catch (Exception ex)
            {
                ErrorLog objErrorLog = new ErrorLog();
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw ex;
            }
            finally
            {
                con.Close();
            }
            //throw new NotImplementedException();
        }
        public string Updateuploadgsttax(Entity_taxgst savegst)
        {
            string Msg = string.Empty;
            try
            {
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_Upload_GST_TAX", con);
                cmd.Parameters.AddWithValue("@statement", "U");
                cmd.Parameters.AddWithValue("@uploadgid", savegst.uploadgid);
                cmd.Parameters.AddWithValue("@UPLOAD_REMARKS", (savegst.Remarks));
                cmd.Parameters.AddWithValue("@UPLOAD_STATUS", Convert.ToInt32(savegst.Status));
                cmd.Parameters.AddWithValue("@UPLOAD_APPROVED", Convert.ToInt32(savegst.Approvalstatus));
                cmd.Parameters.AddWithValue("@UPLOAD_LASTMODIFIED", Convert.ToInt32(objCmnFunctions.GetLoginUserGid().ToString()));
                //  cmd.Parameters.AddWithValue("@insertedby", Convert.ToInt32(objCmnFunctions.GetLoginUserGid().ToString()));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Msg = dt.Rows[0]["ErrorMessage"].ToString();
                }

            }

            catch (Exception ex)
            {
                ErrorLog objErrorLog = new ErrorLog();
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Msg;
        }

        public List<Entity_taxgst> historylist()
        {
            try
            {
                List<Entity_taxgst> objOrgType = new List<Entity_taxgst>();
                Entity_taxgst objModel;
                GetCon();
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                cmd = new SqlCommand("pr_Upload_GST_TAX", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@XmlData", "");
                cmd.Parameters.AddWithValue("@statement", "H");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new Entity_taxgst();
                    objModel.upload_sno = Convert.ToInt16(dt.Rows[i]["sno"]);
                    objModel.Rolename = dt.Rows[i]["Action_By"].ToString();
                    objModel.uploaddate = dt.Rows[i]["ActionDate"].ToString();
                    objModel.Statename = dt.Rows[i]["Emp_name"].ToString();
                    objModel.uploadno = dt.Rows[i]["UPLOAD_NO"].ToString();
                    objModel.upload_filename = dt.Rows[i]["UPLOAD_FILENAME"].ToString();
                    objModel.Status = dt.Rows[i]["Status"].ToString().Trim();
                    objModel.Remarks = dt.Rows[i]["Remarks"].ToString().Trim();
                    objOrgType.Add(objModel);
                }
                return objOrgType;
            }
            catch (Exception ex)
            {
                ErrorLog objErrorLog = new ErrorLog();
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw ex;
            }
            finally
            {
                con.Close();
            }
        #endregion
            //pandiraj
        }
        public IEnumerable<Entity_taxgst> GetAuditTrialList(int taxsubtypeid, int hsnid, int stateid)
        {
            try
            {
                List<Entity_taxgst> objOrgType = new List<Entity_taxgst>();
                Entity_taxgst objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tgetaudit", con);
                cmd.Parameters.AddWithValue("@ptaxsubtypeid", taxsubtypeid);
                cmd.Parameters.AddWithValue("@phsnid", hsnid);
                cmd.Parameters.AddWithValue("@pstateid", stateid);
                cmd.Parameters.AddWithValue("@paction", "GSTAudit");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new Entity_taxgst();
                    objModel.TaxSubtypeName = Convert.ToString(dt.Rows[i]["taxsubtype_name"].ToString());
                    objModel.hsnid = Convert.ToInt32(dt.Rows[i]["hsn_code"].ToString());
                    objModel.Hsn_desc = Convert.ToString(dt.Rows[i]["hsn_description"].ToString());
                    objModel.Statename = Convert.ToString(dt.Rows[i]["state_name"].ToString());
                    objModel.Rate = Convert.ToDecimal(dt.Rows[i]["Taxgst_Rate"].ToString());
                    objModel.inputcredit = Convert.ToDecimal(dt.Rows[i]["Taxgst_inputcredit"].ToString());
                    objModel.effective_date = Convert.ToString(dt.Rows[i]["effective_date"].ToString());
                    objModel.effective_todate = Convert.ToString(dt.Rows[i]["effective_todate"].ToString());
                    objModel.reverse_charge = Convert.ToDecimal(dt.Rows[i]["Taxgst_reverse_charge"].ToString());
                    objModel.IpcdGLno = Convert.ToString(dt.Rows[i]["gl_no"].ToString());
                    objModel.Remarks = Convert.ToString(dt.Rows[i]["Remarks"].ToString());
                    objModel.stategid = Convert.ToInt32(dt.Rows[i]["state_gid"].ToString());
                    objModel.Taxsubtypeid = Convert.ToInt32(dt.Rows[i]["taxsubtype_gid"].ToString());
                    //objModel.customergst_gstin = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["customergst_gstin"].ToString()) ? "-" : dt.Rows[i]["customergst_gstin"].ToString()); 

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
    }
}