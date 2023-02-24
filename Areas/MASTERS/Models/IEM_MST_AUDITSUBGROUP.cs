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
    public class IEM_MST_AUDITSUBGROUP : Iiem_mast_auditsubgroup
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
        public IEnumerable<iem_mst_auditsubgroup> GetAuditsubgroup()
        {
            try
            {
                List<iem_mst_auditsubgroup> objOrgType = new List<iem_mst_auditsubgroup>();
                iem_mst_auditsubgroup objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_auditsubgroup", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_auditsubgroup();
                    objModel.auditsubgroup_gid= Convert.ToInt32(dt.Rows[i]["auditsubgroup_gid"].ToString());
                    objModel.auditsubgroup_name= Convert.ToString(dt.Rows[i]["auditsubgroup_name"].ToString());
                    objModel.auditgroup_name= Convert.ToString(dt.Rows[i]["auditgroup_name"].ToString());
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

        public IEnumerable<iem_mst_auditsubgroup> GetAuditsubgroup(string Auditsubgroupname, string auditgroupname)
        {
            try
            {
                List<iem_mst_auditsubgroup> objOrgType = new List<iem_mst_auditsubgroup>();
                iem_mst_auditsubgroup objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_auditsubgroup", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.Parameters.AddWithValue("@auditsubgroup_name", Auditsubgroupname);
                cmd.Parameters.AddWithValue("@auditgroup_name", auditgroupname);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_auditsubgroup();
                    objModel.auditsubgroup_gid = Convert.ToInt32(dt.Rows[i]["auditsubgroup_gid"].ToString());
                    objModel.auditsubgroup_name = Convert.ToString(dt.Rows[i]["auditsubgroup_name"].ToString());
                    objModel.auditgroup_name = Convert.ToString(dt.Rows[i]["auditgroup_name"].ToString());
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

        public iem_mst_auditsubgroup GetAuditsubgroupById(int AuditsubgroupId)
        {
            var obj = new iem_mst_auditsubgroup();
            try
            {
                List<iem_mst_auditsubgroup> objOrgType = new List<iem_mst_auditsubgroup>();
                iem_mst_auditsubgroup objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_auditsubgroup", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", AuditsubgroupId);                
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                obj=new iem_mst_auditsubgroup()
               {
                   
                   auditsubgroup_gid = Convert.ToInt32(dt.Rows[0]["auditsubgroup_gid"].ToString()),
                   auditsubgroup_name = Convert.ToString(dt.Rows[0]["auditsubgroup_name"].ToString()),
                   auditgroup_gid = Convert.ToInt32(dt.Rows[0]["auditgroup_gid"].ToString()),
                    
            };

                return obj;
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

        public string InsertAuditsubgroup(iem_mst_auditsubgroup Auditsubgroup)
        {
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_auditsubgroup", con);
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.AddWithValue("@auditsubgroup_name", Auditsubgroup.auditsubgroup_name);
                cmd.Parameters.AddWithValue("@auditgroup_name", Auditsubgroup.auditgroup_name);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["@res"].Value.ToString();

                if (result == "Not There")
                {
                    CommonIUD comm = new CommonIUD();
                    string[,] codes = new string[,]            
	                {
                        {"auditsubgroup_name",Auditsubgroup.auditsubgroup_name },
                        {"auditsubgroup_auditgroup_gid",Convert.ToString(Auditsubgroup.auditsubgroup_auditgroup_gid)},
                        {"auditsubgroup_order",Convert.ToString (Auditsubgroup.auditsubgroup_order)},
                        {"auditsubgroup_insert_by",cmn.GetLoginUserGid().ToString ()},
                        {"auditsubgroup_insert_date",comm.GetCurrentDate()}
                    };
                    string tname = "iem_mst_tauditsubgroup";
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

        public string DeleteAuditsubgroup(int FinyearId)
        {
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {

                string[,] codes = new string[,]
	       {
                 {"auditsubgroup_isremoved", "Y"}
	            
           };
                string[,] whrcol = new string[,]
	        {
                {"auditsubgroup_gid", FinyearId.ToString ()}
            };
                string tblname = "iem_mst_tauditsubgroup";


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

        public string UpdateAuditsubgroup(iem_mst_auditsubgroup UpdateAuditsubgroupmodel)
        {
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_auditsubgroup", con);
                cmd.Parameters.AddWithValue("@action", "UpdateExist");
                cmd.Parameters.AddWithValue("@auditsubgroup_name", UpdateAuditsubgroupmodel.auditsubgroup_name);
                cmd.Parameters.AddWithValue("@gid", UpdateAuditsubgroupmodel.auditsubgroup_gid);
                cmd.Parameters.AddWithValue("@auditgroup_name", UpdateAuditsubgroupmodel.auditgroup_name);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["@res"].Value.ToString();

                if (result == "Not There")
                {
                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes = new string[,]
	                  {
                        {"auditsubgroup_name",UpdateAuditsubgroupmodel.auditsubgroup_name },
                        {"auditsubgroup_auditgroup_gid",Convert.ToString(UpdateAuditsubgroupmodel.auditsubgroup_auditgroup_gid)},
                        {"auditsubgroup_order",Convert.ToString (UpdateAuditsubgroupmodel.auditsubgroup_order)},
                        {"auditsubgroup_insert_by",cmn.GetLoginUserGid().ToString ()},
                        {"auditsubgroup_insert_date",delecomm.GetCurrentDate()}
	                  };
                    string[,] whrcol = new string[,]
	                 {
                          {"auditsubgroup_gid", UpdateAuditsubgroupmodel.auditsubgroup_gid.ToString ()},
                          {"auditsubgroup_isremoved", "N"}
                     };

                    string updcomm = delecomm.UpdateCommon(codes, whrcol, "iem_mst_tauditsubgroup");
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
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return obj;
        }

        //public System.Data.DataTable getauditgroupname(int groupgid)
        //{
        //    throw new NotImplementedException();
        //}


        public string getauditgroupname(int groupgid)
        {
             string result=string.Empty;
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_auditsubgroup", con);
                cmd.Parameters.AddWithValue("@action", "Selectauditgroup");
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                result = cmd.Parameters["@res"].Value.ToString();
            }
            catch(Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public string gotcount()
        {
            string count = string.Empty;

            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_auditsubgroup", con);
                cmd.Parameters.AddWithValue("@action", "SelectCount");
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                count = cmd.Parameters["@res"].Value.ToString();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return count;
        }
    }
}