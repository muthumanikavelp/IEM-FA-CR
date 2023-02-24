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
    public class IEM_MST_AUDITLIST : Iiem_mst_auditlist
    {
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
        public IEnumerable<iem_mst_auditlist> GetAuditList()
        {
            try
            {
                List<iem_mst_auditlist> objOrgType = new List<iem_mst_auditlist>();
                iem_mst_auditlist objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_auditlist", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_auditlist();
                    objModel.auditlist_gid = Convert.ToInt32(dt.Rows[i]["auditlist_gid"].ToString());
                    objModel.auditlist_name = Convert.ToString(dt.Rows[i]["auditlist_name"].ToString());
                    objModel.auditlist_template = Convert.ToString(dt.Rows[i]["auditlist_template"].ToString());
                    objModel.auditgroup_name= Convert.ToString(dt.Rows[i]["auditgroup_name"].ToString());
                    objModel.auditsubgroup_name = Convert.ToString(dt.Rows[i]["auditsubgroup_name"].ToString());
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

        public IEnumerable<iem_mst_auditlist> GetAuditList(string AuditListame, string auditgroupname, string auditsubname)
        {
            try
            {
                List<iem_mst_auditlist> objOrgType = new List<iem_mst_auditlist>();
                iem_mst_auditlist objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_auditlist", con);
                cmd.Parameters.AddWithValue("@action", "Search");
                cmd.Parameters.AddWithValue("@auditlist_name", AuditListame);
                cmd.Parameters.AddWithValue("@auditgroup_name", auditgroupname);
                cmd.Parameters.AddWithValue("@auditsubgroup_name", auditsubname);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_auditlist();
                    objModel.auditlist_gid = Convert.ToInt32(dt.Rows[i]["auditlist_gid"].ToString());
                    objModel.auditlist_name = Convert.ToString(dt.Rows[i]["auditlist_name"].ToString());
                    objModel.auditlist_template = Convert.ToString(dt.Rows[i]["auditlist_template"].ToString());
                    objModel.auditgroup_name = Convert.ToString(dt.Rows[i]["auditgroup_name"].ToString());
                    objModel.auditsubgroup_name = Convert.ToString(dt.Rows[i]["auditsubgroup_name"].ToString());
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

        public iem_mst_auditlist GetAuditListById(int AuditListGid)
        {
            try
            {
                List<iem_mst_auditlist> objOrgType = new List<iem_mst_auditlist>();
                iem_mst_auditlist objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_auditlist", con);
                cmd.Parameters.AddWithValue("@gid", AuditListGid);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                var model=new iem_mst_auditlist()
                {
                    
                    auditlist_gid = Convert.ToInt32(dt.Rows[0]["auditlist_gid"].ToString()),
                    auditlist_name = Convert.ToString(dt.Rows[0]["auditlist_name"].ToString()),
                    auditlist_template = Convert.ToString(dt.Rows[0]["auditlist_template"].ToString()),
                    auditgroup_gid = Convert.ToInt32(dt.Rows[0]["auditgroup_gid"].ToString()),
                    auditsubgroup_gid = Convert.ToInt32(dt.Rows[0]["auditsubgroup_gid"].ToString()),                    
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

        public string InsertAuditList(iem_mst_auditlist AuditList)
        {
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_auditlist", con);
                cmd.Parameters.AddWithValue("@action", "InsertExist");
                cmd.Parameters.AddWithValue("@auditlist_name", AuditList.auditlist_name);                
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["@res"].Value.ToString();

                if (result == "Not There")
                {
                    CommonIUD comm = new CommonIUD();
                    string[,] codes = new string[,]            
	                {
                        {"auditlist_auditsubgroup_gid",Convert.ToString(AuditList.auditlist_auditsubgroup_gid)},
                        {"auditlist_name", AuditList.auditlist_name},
                        {"auditlist_template",AuditList.auditlist_template },
                        {"auditlist_order",Convert.ToString(AuditList.auditlist_order)},
                        {"auditlist_auditgroup_gid",Convert.ToString(AuditList.auditlist_auditgroup_gid)},
                        {"auditlist_insert_by",cmnfun.GetLoginUserGid().ToString ()},
                        {"auditlist_insert_date",comm.GetCurrentDate()}
                    };
                    string tname = "iem_mst_tauditlist";
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

        public string DeleteAuditList(int AuditGroupGid)
        {
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {

                string[,] codes = new string[,]
	       {
                 {"auditlist_isremoved", "Y"}
	            
           };
                string[,] whrcol = new string[,]
	        {
                {"auditlist_gid", AuditGroupGid.ToString ()}
            };
                string tblname = "iem_mst_tauditlist";


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

        public string UpdateAuditGroup(iem_mst_auditlist UpdateAudditListmodel)
        {
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_auditlist", con);
                cmd.Parameters.AddWithValue("@action", "UpdateExist");
                cmd.Parameters.AddWithValue("@auditlist_name", UpdateAudditListmodel.auditlist_name);
                cmd.Parameters.AddWithValue("@gid", UpdateAudditListmodel.auditlist_gid);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["@res"].Value.ToString();  
                if (result == "Not There")
                {
                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes = new string[,]
	                  {
                        {"auditlist_auditsubgroup_gid",Convert.ToString(UpdateAudditListmodel.auditlist_auditsubgroup_gid)},
                        {"auditlist_name", UpdateAudditListmodel.auditlist_name},
                        {"auditlist_template",UpdateAudditListmodel.auditlist_template },
                        {"auditlist_order",Convert.ToString(UpdateAudditListmodel.auditlist_order)},
                        {"auditlist_auditgroup_gid",Convert.ToString(UpdateAudditListmodel.auditlist_auditgroup_gid)},
                        {"auditlist_insert_by",cmnfun.GetLoginUserGid().ToString ()},
                        {"auditlist_insert_date",delecomm.GetCurrentDate()}
	                  };
                    string[,] whrcol = new string[,]
	                 {
                          {"auditlist_gid", UpdateAudditListmodel.auditlist_gid.ToString ()},
                          {"auditlist_isremoved", "N"}
                     };

                    string updcomm = delecomm.UpdateCommon(codes, whrcol, "iem_mst_tauditlist");
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

        public IEnumerable<selctgroupname> selctgroupname()
        {
            List<selctgroupname> obj = new List<selctgroupname>();
            selctgroupname getobj;
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_auditlist", con);
                cmd.Parameters.AddWithValue("@action", "selectgroupname");               
                //cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    getobj = new selctgroupname();
                    getobj.groupnamegid = Convert.ToInt32(dt.Rows[i]["auditgroup_gid"].ToString());
                    getobj.groupname = Convert.ToString(dt.Rows[i]["auditgroup_name"].ToString());
                    obj.Add(getobj);
                }
               
            }
            catch(Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return obj;
        }

        public IEnumerable<selectsubgroupname> selectsubgroupname()
        {
            List<selectsubgroupname> obj = new List<selectsubgroupname>();
            selectsubgroupname getobj;
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_auditlist", con);
                cmd.Parameters.AddWithValue("@action", "selectsubgroupname");
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    getobj = new selectsubgroupname();
                    getobj.subgroupnamegid = Convert.ToInt32(dt.Rows[i]["auditsubgroup_gid"].ToString());
                    getobj.subgroupname = Convert.ToString(dt.Rows[i]["auditsubgroup_name"].ToString());
                    obj.Add(getobj);
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return obj;
        }

        public DataTable getauditlistorder()
        {
            DataTable getorder = new DataTable();
            try
            {
                GetCon();
                cmd = new SqlCommand("select MAX(auditlist_order) as Count from iem_mst_tauditlist", con);
                cmd.ExecuteNonQuery();
                da = new SqlDataAdapter(cmd);
                da.Fill(getorder);
            }
            catch(Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return getorder;
        }
    }
}