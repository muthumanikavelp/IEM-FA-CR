using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using System.Configuration;
using System.Data.SqlClient;
using IEM.Common;

namespace IEM.Areas.MASTERS.Models
{
    
    public class IEM_MST_AUDITGROUP : Iiem_mst_auditgroup
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions cmnfun = new CmnFunctions();
        private void GetCon()
        {
            if(con.State==ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }
        public IEnumerable<iem_mst_auditgroup> GetAuditGroup()
        {
            List<iem_mst_auditgroup> objmodel = new List<iem_mst_auditgroup>();
            iem_mst_auditgroup obj;
            DataTable dt = new DataTable();
           try
           {
               GetCon();
               cmd = new SqlCommand("pr_iem_mst_auditgroup", con);
               cmd.Parameters.AddWithValue("@action", "select");
               cmd.CommandType = CommandType.StoredProcedure;
               da = new SqlDataAdapter(cmd);
               da.Fill(dt);
               foreach(DataRow row in dt.Rows)
               {
                   obj = new iem_mst_auditgroup()
                   {
                       auditgroup_gid = Convert.ToInt32(row["auditgroup_gid"].ToString()),
                       auditgroup_name = row["auditgroup_name"].ToString(),
                       doccatname = row["doccat_name"].ToString(),
                       doctype_name = row["doctype_name"].ToString(),
                       docsubtypename = row["docsubtype_name"].ToString(),
                
                };
                   objmodel.Add(obj);
               }              

           }
            catch(Exception ex)
           {

           }
            finally
           {
               con.Close();
           }
           return objmodel;
        }

        public IEnumerable<iem_mst_auditgroup> GetAuditGroup(string Auditgrouname, string doctype, string docsubtype, string doccat)
        {
            List<iem_mst_auditgroup> objmodel = new List<iem_mst_auditgroup>();
            iem_mst_auditgroup obj;
            DataTable dt = new DataTable();
            try
            {
                GetCon();
                cmd = new SqlCommand("pr_iem_mst_auditgroup", con);
                cmd.Parameters.AddWithValue("@action", "Search");
                cmd.Parameters.AddWithValue("@auditgroup_name", Auditgrouname);
                cmd.Parameters.AddWithValue("@doctype_name", doctype);
                cmd.Parameters.AddWithValue("@docsubtype_name", docsubtype);
                cmd.Parameters.AddWithValue("@doccat_name", doccat);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new iem_mst_auditgroup()
                    {
                        auditgroup_gid = Convert.ToInt32(row["auditgroup_gid"].ToString()),
                        auditgroup_name = row["auditgroup_name"].ToString(),
                        doccatname = row["doccat_name"].ToString(),
                        doctype_name = row["doctype_name"].ToString(),
                        docsubtypename = row["docsubtype_name"].ToString(),

                    };
                    objmodel.Add(obj);
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return objmodel;
        }

        public iem_mst_auditgroup GetAuditgroupById(int AuditgroupGid)
        {
            List<iem_mst_auditgroup> obj = new List<iem_mst_auditgroup>();
            iem_mst_auditgroup obj1;
            var model = new iem_mst_auditgroup();
            try
            {
                GetCon();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_iem_mst_auditgroup", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", AuditgroupGid);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                model = new iem_mst_auditgroup()
                {
                    auditgroup_gid = Convert.ToInt32(dt.Rows[0]["auditgroup_gid"].ToString()),
                    auditgroup_name = dt.Rows[0]["auditgroup_name"].ToString(),
                   // doccatname = dt.Rows[0]["doccat_name"].ToString(),
                   // doctype_name = dt.Rows[0]["doctype_name"].ToString(), 
                   // docsubtypename = dt.Rows[0]["docsubtype_name"].ToString(),
                    doctype_gid = Convert.ToInt32(dt.Rows[0]["doctype_gid"].ToString()),
                    docsubtype_gid = Convert.ToInt32(dt.Rows[0]["docsubtype_gid"].ToString()),
                    doccat_gid = Convert.ToInt32(dt.Rows[0]["doccat_gid"].ToString()),
                };
                
            }
            catch(Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return model;
        }

        public string InsertAuditGroup(iem_mst_auditgroup Auditgroup)
        {
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_auditgroup", con);
                cmd.Parameters.AddWithValue("@action", "InsertExist");
                cmd.Parameters.AddWithValue("@auditgroup_name", Auditgroup.auditgroup_name);              
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["@res"].Value.ToString();
                if (result == "Not There")
                {
                    CommonIUD comm = new CommonIUD();
                    string[,] codes = new string[,]            
	                {
                        {"auditgroup_name",Auditgroup.auditgroup_name},
                        {"auditgroup_doccat_gid",Convert.ToString (Auditgroup.auditgroup_doccat_gid)},
                        {"auditgroup_doctype_gid",Convert.ToString (Auditgroup.auditgroup_doctype_gid)},
                        {"auditgroup_docsubtype_gid",Convert.ToString (Auditgroup.auditgroup_docsubtype_gid)},
                        {"auditgroup_insert_by",cmnfun.GetLoginUserGid().ToString ()},
                        {"auditgroup_insert_date",comm.GetCurrentDate()}
                    };
                    string tname = "iem_mst_tauditgroup";
                    string insertcommend = comm.InsertCommon(codes, tname);
                }
                else
                {
                    return "Duplicate record !";
                }
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

        public string DeleteAuditGroup(int AuditGroupGid)
        {
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {

                string[,] codes = new string[,]
	       {
                 {"auditgroup_isremoved", "Y"}
	            
           };
                string[,] whrcol = new string[,]
	        {
                {"auditgroup_gid", AuditGroupGid.ToString ()}
            };
                string tblname = "iem_mst_tauditgroup";


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

        public string UpdateAuditGroup(iem_mst_auditgroup UpdateAudditgroupmodel)
        {
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_auditgroup", con);
                cmd.Parameters.AddWithValue("@action", "UpdateExist");
                cmd.Parameters.AddWithValue("@auditgroup_name", UpdateAudditgroupmodel.auditgroup_name);
                cmd.Parameters.AddWithValue("@gid", UpdateAudditgroupmodel.auditgroup_gid);               
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["@res"].Value.ToString();
                if (result == "Not There")
                {
                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes = new string[,]
	                  {
                        {"auditgroup_name",UpdateAudditgroupmodel.auditgroup_name},
                        {"auditgroup_doccat_gid",Convert.ToString (UpdateAudditgroupmodel.auditgroup_doccat_gid)},
                        {"auditgroup_doctype_gid",Convert.ToString (UpdateAudditgroupmodel.auditgroup_doctype_gid)},
                        {"auditgroup_docsubtype_gid",Convert.ToString (UpdateAudditgroupmodel.auditgroup_docsubtype_gid)},
                        {"auditgroup_update_by",cmnfun.GetLoginUserGid().ToString ()},
                        {"auditgroup_update_date",delecomm.GetCurrentDate()}
	                  };
                    string[,] whrcol = new string[,]
	                 {
                          {"auditgroup_gid", UpdateAudditgroupmodel.auditgroup_gid.ToString ()},
                          {"auditgroup_isremoved", "N"}
                     };

                    string updcomm = delecomm.UpdateCommon(codes, whrcol, "iem_mst_tauditgroup");
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

        public IEnumerable<doctype> doctype()
        {
            List<doctype> getdoc = new List<doctype>();
            doctype obj;
            DataTable dt = new DataTable();
            try
            {
                GetCon();
                cmd = new SqlCommand("pr_iem_mst_auditgroup", con);
                cmd.Parameters.AddWithValue("@action", "SelectDocType");
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new doctype()
                    {
                        doctypegid = Convert.ToInt32(row["doctype_gid"].ToString()),
                        doctypename = row["doctype_name"].ToString(),
                    };
                    getdoc.Add(obj);
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return getdoc;

          
        }

        public IEnumerable<docsubtype> docsubtype()
        {
            List<docsubtype> getdoc = new List<docsubtype>();
            docsubtype obj;
            DataTable dt = new DataTable();
            try
            {
                GetCon();
                cmd = new SqlCommand("pr_iem_mst_auditgroup", con);
                cmd.Parameters.AddWithValue("@action", "SelectDocSubType");
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new docsubtype()
                    {
                        docsubtypegid = Convert.ToInt32(row["docsubtype_gid"].ToString()),
                        docsubtypename = row["docsubtype_name"].ToString(),
                    };
                    getdoc.Add(obj);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return getdoc;
        }

        public IEnumerable<doccat> doccat()
        {
            List<doccat> getdoc = new List<doccat>();
            doccat obj;
            DataTable dt = new DataTable();
            try
            {
                GetCon();
                cmd = new SqlCommand("pr_iem_mst_auditgroup", con);
                cmd.Parameters.AddWithValue("@action", "SelectDocCat");
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new doccat()
                    {
                        doccatgid = Convert.ToInt32(row["doccat_gid"].ToString()),
                        doccatname = row["doccat_name"].ToString(),
                    };
                    getdoc.Add(obj);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return getdoc;
        }
    }
}