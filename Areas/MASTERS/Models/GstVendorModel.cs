using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Net;
using System.Web;
using IEM.Areas.MASTERS.Models.IEM;
using System.Web.Mvc;
using System.Web.Helpers;
using IEM.Common;
using System.Configuration;

namespace IEM.Areas.MASTERS.Models
{
    public class GstVendorModel : GstVendor
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions com = new CmnFunctions();
        string Ischecker = "";
        private void GetCon()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();                 
            }
        }
        
         
        //public IEnumerable<EntityGstvendor> getvendor()
        //{
        //    try
        //    {
        //        List<EntityGstvendor> objOrgType = new List<EntityGstvendor>();
        //        EntityGstvendor objModel;
        //        GetCon();
        //        DataTable dt = new DataTable();
        //        dt.Columns.Clear();
        //        dt.Rows.Clear();
        //        dt.Columns.Add("Gstapplicable");
        //        dt.Columns.Add("State");
        //        dt.Columns.Add("Gsttin");
        //        dt.Columns.Add("Businessvertical");
        //        dt.Columns.Add("Status");
        //        DataRow dr = dt.NewRow();
        //        dr["Gstapplicable"] = "Yes-Combo";
        //        dr["State"] = "Tamil Nadu";
        //        dr["Gsttin"] = "TIN001";
        //        dr["Businessvertical"] = "Business";
        //        dr["Status"] = "Active";
        //        dt.Rows.Add(dr);

        //        objModel = new EntityGstvendor();
        //        objModel.suppliergst_app = dt.Rows[0]["Gstapplicable"].ToString();
        //        objModel.suppliergst_app  = dt.Rows[0]["State"].ToString();
        //        objModel.suppliergst_tin = dt.Rows[0]["Gsttin"].ToString();
        //        objModel.suppliergst_vertical = dt.Rows[0]["Businessvertical"].ToString();                
        //        objModel.suppliergst_status = dt.Rows[0]["Status"].ToString();
        //        objOrgType.Add(objModel);
        //        return objOrgType;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //}

        public IEnumerable<EntityGstvendor> GetState()
        {
            //  throw new NotImplementedException();
            try
            {
                List<EntityGstvendor> objOrgType = new List<EntityGstvendor>();
                EntityGstvendor objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("GST_MST_STATE_SL", con);
                cmd.Parameters.AddWithValue("@StatementType", "SEL");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new EntityGstvendor();
                    objModel.suppliergst_stateid = Convert.ToInt32(dt.Rows[i]["state_gid"].ToString());                    
                    objModel.suppliergst_state = Convert.ToString(dt.Rows[i]["state_name"].ToString());
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

        public EntityGstvendor GetStateById(int Disid)
        {
            //  throw new NotImplementedException();
            try
            {
                List<EntityGstvendor> objOrgType = new List<EntityGstvendor>();
                EntityGstvendor objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("GST_MST_State_SEL", con);
                cmd.Parameters.AddWithValue("@StatementType", "SID");
                cmd.Parameters.AddWithValue("@state_gid", Disid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objModel = new EntityGstvendor()
                {

                    suppliergst_stateid = Convert.ToInt32(dt.Rows[0]["state_gid"].ToString()),
                    //  objModel.Currencycode = Convert.ToString(dt.Rows[i]["district_code"].ToString());
                     suppliergst_state= Convert.ToString(dt.Rows[0]["state_name"].ToString()),
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
        }

        public string Insertvendor(EntityGstvendor Classifications)
        {
            string result;
            // throw new NotImplementedException();
            //try
            //{
            //    CommonIUD comm = new CommonIUD();
            //    SqlCommand cmd = new SqlCommand("GST_MST_TMP_SUPPLIER_IU", con);
            //    GetCon();
            //    cmd.Parameters.AddWithValue("@StatementType", "I");
               
            //    cmd.Parameters.AddWithValue("@suppliergst_gid ", "0");
            //    cmd.Parameters.AddWithValue("@suppliergst_gst_app", Classifications.suppliergst_app);
            //    cmd.Parameters.AddWithValue("@suppliergst_gstin", Classifications.suppliergst_tin);
            //    cmd.Parameters.AddWithValue("@suppliergst_state_gid",Convert.ToInt32(Classifications.suppliergst_stateid));
            //    cmd.Parameters.AddWithValue("@suppliergst_status", Classifications.suppliergst_status);
            //    cmd.Parameters.AddWithValue("@suppliergst_isremoved","N");
            //    cmd.Parameters.AddWithValue("@suppliergst_insertby", int.Parse(com.GetLoginUserGid().ToString()));
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.ExecuteNonQuery();
            //    result = "success";
            //    return result;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    con.Close();
            //}
            try
            {
                CommonIUD commn = new CommonIUD();
                SqlCommand cmdn = new SqlCommand("GST_MST_FICCBRANCH_IU", con);
                GetCon();
                string res5 = "YES";
                string res1 = "";
                if (Classifications.suppliergst_app == "Yes")
                {
                    string ss = Classifications.suppliergst_tin.Substring(0, 2);// Classifications.suppliergst_tin.Split(new char[] { ' ' }, 2);
                    cmdn.Parameters.AddWithValue("@StatementType", "C");
                    cmdn.Parameters.AddWithValue("@state_code", ss);
                    cmdn.Parameters.AddWithValue("@branchgst_state_gid", Convert.ToInt32(Classifications.suppliergst_stateid));
                    cmdn.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                    cmdn.CommandType = CommandType.StoredProcedure;
                    cmdn.ExecuteNonQuery();
                    res5 = cmdn.Parameters["@res"].Value.ToString();
                }
                else
                {
                    Classifications.suppliergst_tin = "";
                    Classifications.suppliergst_vertical = "";
                }
                if (res5 == "YES")
                {
                     
                        CommonIUD comms = new CommonIUD();
                        SqlCommand cmds = new SqlCommand("GST_MST_FICCBRANCH_IU", con);
                        GetCon();
                        cmds.Parameters.AddWithValue("@StatementType", "E");
                        cmds.Parameters.AddWithValue("@branchgst_gid ", "0");
                        cmds.Parameters.AddWithValue("@branchgst_gstin", Classifications.suppliergst_tin);
                        cmds.Parameters.AddWithValue("@branchgst_state_gid", Convert.ToInt32(Classifications.suppliergst_stateid));
                        cmds.Parameters.AddWithValue("@branchgst_vertical", Classifications.suppliergst_vertical);
                        cmds.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                        cmds.CommandType = CommandType.StoredProcedure;
                        cmds.ExecuteNonQuery();
                        res1 = cmds.Parameters["@res"].Value.ToString();                    
                        if (res1 == "Not There")
                        {
                            CommonIUD comm = new CommonIUD();
                            SqlCommand cmdt = new SqlCommand("GST_MST_FICCBRANCH_IU", con);
                            GetCon();
                            cmdt.Parameters.AddWithValue("@StatementType", "I");
                            cmdt.Parameters.AddWithValue("@branchgst_gid ", "0");
                            cmdt.Parameters.AddWithValue("@branchgst_gst_app", Classifications.suppliergst_app);
                            cmdt.Parameters.AddWithValue("@branchgst_gstin", Classifications.suppliergst_tin);
                            cmdt.Parameters.AddWithValue("@branchgst_vertical", Classifications.suppliergst_vertical);
                            cmdt.Parameters.AddWithValue("@branchgst_state_gid", Convert.ToInt32(Classifications.suppliergst_stateid));
                            cmdt.Parameters.AddWithValue("@branch_status", Classifications.suppliergst_status);
                            cmdt.Parameters.AddWithValue("@branchgst_isremoved", "N");
                            cmdt.Parameters.AddWithValue("@branchgst_insertby", int.Parse(com.GetLoginUserGid().ToString()));
                            cmdt.CommandType = CommandType.StoredProcedure;
                            cmdt.ExecuteNonQuery();
                            result = "success";
                        }
                        else
                        {
                            result = res1;
                        }
                }
                else
                {
                    result = "State GST Code InCorrect";
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

        public IEnumerable<EntityGstvendor> getmaker()
        {    
            try
            {
                List<EntityGstvendor> objOrgType = new List<EntityGstvendor>();
                EntityGstvendor objModel;
                GetCon();
                DataTable dt = new DataTable();

                SqlCommand cmd = new SqlCommand("GST_MST_FICCBRANCH_SL", con);
                cmd.Parameters.AddWithValue("@StatementType", "ROLE");
                cmd.Parameters.AddWithValue("@UId ", int.Parse(com.GetLoginUserGid().ToString()));
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new EntityGstvendor();                   
                    objModel.IsChecker = dt.Rows[i]["IsChecker"].ToString();                    
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
        
        public IEnumerable<EntityGstvendor> getvendor()
        {           

            try
            {
                List<EntityGstvendor> objOrgType = new List<EntityGstvendor>();
                EntityGstvendor objModel; 
                GetCon();
                DataTable dt = new DataTable();

                SqlCommand cmd = new SqlCommand("GST_MST_FICCBRANCH_SL", con);
                cmd.Parameters.AddWithValue("@StatementType", "SEL");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new EntityGstvendor();
                    objModel.suppliergst_gid = Convert.ToInt32(dt.Rows[i]["branchgst_gid"]);
                    objModel.suppliergst_app = dt.Rows[i]["applicable"].ToString();
                    objModel.suppliergst_stateid = Convert.ToInt32(dt.Rows[i]["branchgst_state_gid"]);
                    objModel.suppliergst_state = dt.Rows[i]["state_name"].ToString();
                    objModel.suppliergst_tin = dt.Rows[i]["branchgst_gstin"].ToString();
                    objModel.suppliergst_vertical = dt.Rows[i]["branchgst_vertical"].ToString();
                    objModel.suppliergst_status = dt.Rows[i]["branch_status"].ToString();
                    objModel.approved_status = dt.Rows[i]["approved_status"].ToString();
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


        public EntityGstvendor getVendorid(int id)
        {

            try
            {
                List<EntityGstvendor> objOrgType = new List<EntityGstvendor>();
                EntityGstvendor objModel = new EntityGstvendor();
                GetCon();
                DataTable dt = new DataTable();

                SqlCommand cmd = new SqlCommand("GST_MST_FICCBRANCH_SL", con);
                cmd.Parameters.AddWithValue("@StatementType", "SID");
                cmd.Parameters.AddWithValue("@branchgst_gid", id);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel.suppliergst_gid = Convert.ToInt32(dt.Rows[i]["branchgst_gid"]);
                    objModel.suppliergst_app = dt.Rows[i]["branchgst_gst_app"].ToString();
                    //objModel.suppliergst_app = dt.Rows[i]["applicable"].ToString();
                    objModel.suppliergst_stateid = Convert.ToInt32(dt.Rows[i]["branchgst_state_gid"]);
                    objModel.suppliergst_state = dt.Rows[i]["state_name"].ToString();
                    objModel.suppliergst_tin = dt.Rows[i]["branchgst_gstin"].ToString();
                    objModel.suppliergst_vertical = dt.Rows[i]["branchgst_vertical"].ToString();
                    objModel.suppliergst_status = dt.Rows[i]["branch_status"].ToString();
                    objModel.selectedstate_gid = Convert.ToInt32(dt.Rows[i]["branchgst_state_gid"]);
                }

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
            // throw new NotImplementedException();
        }

        public string Updatevendor(EntityGstvendor Classifications)
        {
            string result;

            try
            {
                CommonIUD commn = new CommonIUD();
                SqlCommand cmdn = new SqlCommand("GST_MST_FICCBRANCH_IU", con);
                GetCon();
                string res5 = "YES";
                string res1 = "";
                if (Classifications.suppliergst_app == "Y")
                {
                    string ss = Classifications.suppliergst_tin.Substring(0, 2);
                    cmdn.Parameters.AddWithValue("@StatementType", "C");
                    cmdn.Parameters.AddWithValue("@state_code", ss);
                    cmdn.Parameters.AddWithValue("@branchgst_state_gid", Convert.ToInt32(Classifications.suppliergst_stateid));
                    cmdn.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                    cmdn.CommandType = CommandType.StoredProcedure;
                    cmdn.ExecuteNonQuery();
                    res5 = cmdn.Parameters["@res"].Value.ToString();
                }
                else
                {
                    Classifications.suppliergst_tin = "";
                    Classifications.suppliergst_vertical = "";
                }
                if (res5 == "YES")
                {
                CommonIUD comms = new CommonIUD();
                SqlCommand cmds = new SqlCommand("GST_MST_FICCBRANCH_IU", con);
                GetCon();
                        
                    cmds.Parameters.AddWithValue("@StatementType", "E");
                    cmds.Parameters.AddWithValue("@branchgst_gid ", Classifications.suppliergst_gid);
                    cmds.Parameters.AddWithValue("@branchgst_gstin", Classifications.suppliergst_tin);
                    cmds.Parameters.AddWithValue("@branchgst_state_gid", Convert.ToInt32(Classifications.suppliergst_stateid));
                    cmds.Parameters.AddWithValue("@branchgst_vertical", Classifications.suppliergst_vertical);
                    cmds.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                    cmds.CommandType = CommandType.StoredProcedure;
                    cmds.ExecuteNonQuery();
                    res1 = cmds.Parameters["@res"].Value.ToString();                        
                    if (res1 == "Not There")
                    {
                        CommonIUD comm = new CommonIUD();
                        SqlCommand cmda = new SqlCommand("GST_MST_FICCBRANCH_IU", con);
                        GetCon();
                        cmda.Parameters.AddWithValue("@StatementType", "U");
                        cmda.Parameters.AddWithValue("@branchgst_gid", Classifications.suppliergst_gid);
                        cmda.Parameters.AddWithValue("@branchgst_gst_app", Classifications.suppliergst_app);
                        cmda.Parameters.AddWithValue("@branchgst_gstin", Classifications.suppliergst_tin);
                        cmda.Parameters.AddWithValue("@branchgst_vertical", Classifications.suppliergst_vertical);
                        cmda.Parameters.AddWithValue("@branchgst_state_gid", Convert.ToInt32(Classifications.suppliergst_stateid));
                        cmda.Parameters.AddWithValue("@branch_status", Classifications.suppliergst_status);
                        cmda.Parameters.AddWithValue("@branchgst_isremoved", "N");
                        cmda.Parameters.AddWithValue("@branchgst_updateby", int.Parse(com.GetLoginUserGid().ToString()));
                        cmda.Parameters.AddWithValue("@approved_status", Classifications.IsChecker);
                        cmda.CommandType = CommandType.StoredProcedure;
                        cmda.ExecuteNonQuery();
                        result = "success";
                    }
                    else
                    {
                        result = res1;
                    }
            }
            else
            {
                result = "State GST Code InCorrect";
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

        public string DeleteVendor(int PinId)
        {
            string result;
            // throw new NotImplementedException();
            try
            {
                CommonIUD comm = new CommonIUD();
                SqlCommand cmd = new SqlCommand("GST_MST_FICCBRANCH_IU", con);
                GetCon();
                cmd.Parameters.AddWithValue("@StatementType", "D");
                cmd.Parameters.AddWithValue("@branchgst_gid", PinId);
                cmd.Parameters.AddWithValue("@branchgst_updateby", int.Parse(com.GetLoginUserGid().ToString())); 
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                result = "success";
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
    }
}