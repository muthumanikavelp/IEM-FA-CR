using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using IEM.Areas.MASTERS.Models.IEM;
using System.Web.Mvc;
using System.Web.Helpers;
using IEM.Common;


namespace IEM.Areas.MASTERS.Models
{

    public class GetPincodeModel : Gstpincode
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions com = new CmnFunctions();
        private Gstpincode Res; 
        private void GetCon()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }

        public IEnumerable<EntityGSTPincode> GetDistrict()
        {
            //  throw new NotImplementedException();
            try
            {
                List<EntityGSTPincode> objOrgType = new List<EntityGSTPincode>();
                EntityGSTPincode objModel;
               
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("GST_MST_DISTRICT_SL", con);
               // cmd.Parameters.AddWithValue("@district_state_gid" );  
                cmd.Parameters.AddWithValue("@StatementType", "SEL");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new EntityGSTPincode();
                    objModel.Pincode_district_gid = Convert.ToInt32(dt.Rows[i]["district_gid"].ToString());
                    //  objModel.Currencycode = Convert.ToString(dt.Rows[i]["district_code"].ToString());
                    objModel.Pincode_district_name = Convert.ToString(dt.Rows[i]["district_name"].ToString());
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

        public IEnumerable<EntityGSTPincode> GetState()
        {
            //  throw new NotImplementedException();
            try
            {
                List<EntityGSTPincode> objOrgType = new List<EntityGSTPincode>();
                EntityGSTPincode objModel;
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
                    objModel = new EntityGSTPincode();
                    objModel.Pincode_state_gid = Convert.ToInt32(dt.Rows[i]["state_gid"].ToString());
                    //  objModel.Currencycode = Convert.ToString(dt.Rows[i]["district_code"].ToString());
                    objModel.Pincode_state_name = Convert.ToString(dt.Rows[i]["state_name"].ToString());
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


        public EntityGSTPincode GetDistrictById(int Disid)
        {
            //  throw new NotImplementedException();
            try
            {
                List<EntityGSTPincode> objOrgType = new List<EntityGSTPincode>();
                EntityGSTPincode objModel;
                GetCon();
              
                
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("GST_MST_DISTRICT_SL", con);
                cmd.Parameters.AddWithValue("@StatementType", "SID");
                cmd.Parameters.AddWithValue("@district_gid", Disid);
               
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt); 
                objModel = new EntityGSTPincode()
                {
                         
                    Pincode_district_gid = Convert.ToInt32(dt.Rows[0]["district_gid"].ToString()),
                    //  objModel.Currencycode = Convert.ToString(dt.Rows[i]["district_code"].ToString());
                    Pincode_district_name = Convert.ToString(dt.Rows[0]["district_name"].ToString()),
                       
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

        public EntityGSTPincode GetStateById(int Disid)
        {
            //  throw new NotImplementedException();
            try
            {
                List<EntityGSTPincode> objOrgType = new List<EntityGSTPincode>();
                EntityGSTPincode objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("GST_MST_State_SL", con);
                cmd.Parameters.AddWithValue("@StatementType", "SID");
                cmd.Parameters.AddWithValue("@state_gid", Disid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objModel = new EntityGSTPincode()
                {

                    Pincode_state_gid = Convert.ToInt32(dt.Rows[0]["state_gid"].ToString()),
                    //  objModel.Currencycode = Convert.ToString(dt.Rows[i]["district_code"].ToString());
                    Pincode_state_name = Convert.ToString(dt.Rows[0]["state_name"].ToString()),
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


        public IEnumerable<EntityGSTPincode> getPincode()
        {
            try
            {
                List<EntityGSTPincode> objOrgType = new List<EntityGSTPincode>();
                EntityGSTPincode objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("GST_MST_PINCODE_SL", con);
                cmd.Parameters.AddWithValue("@StatementType", "SEL");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new EntityGSTPincode();
                    objModel.Pincode_gid = Convert.ToInt32(dt.Rows[i]["Pincode_gid"]);
                    objModel.Pincode_code = dt.Rows[i]["Pincode_code"].ToString();
                    objModel.Pincode_state_name =dt.Rows[i]["state_name"].ToString();
                    objModel.Pincode_state_gid = Convert.ToInt32(dt.Rows[i]["district_state_gid"]);
                    objModel.Pincode_district_name = dt.Rows[i]["district_name"].ToString();
                    objModel.Pincode_district_gid = Convert.ToInt32(dt.Rows[i]["Pincode_district_gid"]);
                    objModel.Pincode_status = dt.Rows[i]["pincode_status"].ToString();
                    //objModel.Pincode_isremoved = dt.Rows[0]["pincode_isremoved"].ToString();
                    //objModel.Pincode_insertby = dt.Rows[0]["pincode_insertby"].ToString();
                    //objModel.Pincode_updateby = dt.Rows[0]["pincode_updateby"].ToString();
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



       
        public IEnumerable<EntityGSTPincode> getPincode_list(string Pincode_code)
        {
            //  throw new NotImplementedException();

            try
            {
                List<EntityGSTPincode> objOrgType = new List<EntityGSTPincode>();
                EntityGSTPincode objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("GST_MST_PINCODE_SL", con);
                cmd.Parameters.AddWithValue("@StatementType", "SPIN");
                cmd.Parameters.AddWithValue("@Pincode_code", Pincode_code); 
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new EntityGSTPincode();
                    objModel.Pincode_gid = Convert.ToInt32(dt.Rows[i]["Pincode_gid"]);
                    objModel.Pincode_code = dt.Rows[i]["Pincode_code"].ToString();
                    objModel.Pincode_state_name = dt.Rows[i]["state_name"].ToString();
                    objModel.Pincode_state_gid = Convert.ToInt32(dt.Rows[i]["district_state_gid"]);
                    objModel.Pincode_district_name = dt.Rows[i]["district_name"].ToString();
                    objModel.Pincode_district_gid = Convert.ToInt32(dt.Rows[i]["Pincode_district_gid"]);
                    objModel.Pincode_status = dt.Rows[i]["pincode_status"].ToString();
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

        public string Insertpincode(EntityGSTPincode Classifications)
        {
            string result;
            // throw new NotImplementedException();
            try
            {
                CommonIUD comm = new CommonIUD();
                SqlCommand cmd = new SqlCommand("GST_MST_PINCODE_IU", con);
                GetCon();

                cmd.Parameters.AddWithValue("@StatementType", "E");
                cmd.Parameters.AddWithValue("@pincode_gid", "0");
                cmd.Parameters.AddWithValue("@pincode_code", Classifications.Pincode_code);
                cmd.Parameters.AddWithValue("@pincode_district_gid", Classifications.Pincode_district_gid);
                cmd.Parameters.AddWithValue("@Pincode_state_gid", Classifications.Pincode_state_gid);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    CommonIUD comms = new CommonIUD();
                    SqlCommand cmds = new SqlCommand("GST_MST_PINCODE_IU", con);
                    GetCon();
                    cmds.Parameters.AddWithValue("@StatementType", "I");
                    cmds.Parameters.AddWithValue("@pincode_gid", "0");
                    cmds.Parameters.AddWithValue("@pincode_code", Classifications.Pincode_code);
                    cmds.Parameters.AddWithValue("@pincode_district_gid", Classifications.Pincode_district_gid);
                    cmds.Parameters.AddWithValue("@Pincode_state_gid", Classifications.Pincode_state_gid);
                    cmds.Parameters.AddWithValue("@pincode_status", Classifications.Pincode_status);
                    cmds.Parameters.AddWithValue("@pincode_isremoved", "N");
                    cmds.Parameters.AddWithValue("@pincode_insertby", int.Parse(com.GetLoginUserGid().ToString()));

                    cmds.CommandType = CommandType.StoredProcedure;
                    cmds.ExecuteNonQuery();
                    result = "success";
                }
                else
                {
                    result = "Duplicate record !";
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

        public EntityGSTPincode getPincodeId(int PinId)
        {

            try
            {
                List<EntityGSTPincode> objOrgType = new List<EntityGSTPincode>();
                EntityGSTPincode objModel = new EntityGSTPincode();
                GetCon();
                DataTable dt = new DataTable();

                SqlCommand cmd = new SqlCommand("GST_MST_PINCODE_SL", con);
                cmd.Parameters.AddWithValue("@StatementType", "SID1");
                cmd.Parameters.AddWithValue("@pincode_gid", PinId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);            
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel.Pincode_gid = Convert.ToInt32(dt.Rows[i]["Pincode_gid"]);
                    objModel.Pincode_code = dt.Rows[i]["Pincode_code"].ToString();
                    objModel.Pincode_state_name = dt.Rows[i]["state_name"].ToString();
                    objModel.Pincode_state_gid = Convert.ToInt32(dt.Rows[i]["district_state_gid"]);
                    objModel.selectedstate_gid = Convert.ToInt32(dt.Rows[i]["district_state_gid"]);
                    objModel.Pincode_district_name = dt.Rows[i]["district_name"].ToString();
                    objModel.Pincode_district_gid = Convert.ToInt32(dt.Rows[i]["Pincode_district_gid"]);
                    objModel.selecteddistrict_gid = Convert.ToInt32(dt.Rows[i]["Pincode_district_gid"]);
                    objModel.Pincode_status = dt.Rows[i]["pincode_status"].ToString();
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

        public string Updatepincode(EntityGSTPincode Classifications)
        {
            string result;
            // throw new NotImplementedException();
            try
            {
                CommonIUD comm = new CommonIUD();
                SqlCommand cmd = new SqlCommand("GST_MST_PINCODE_IU", con);
                GetCon();
                 cmd.Parameters.AddWithValue("@StatementType", "E");
                 cmd.Parameters.AddWithValue("@pincode_gid", Classifications.Pincode_gid);
                cmd.Parameters.AddWithValue("@pincode_code", Classifications.Pincode_code); 
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    CommonIUD comms = new CommonIUD();
                    SqlCommand cmds = new SqlCommand("GST_MST_PINCODE_IU", con);
                    GetCon();
                    cmds.Parameters.AddWithValue("@StatementType", "U");
                    cmds.Parameters.AddWithValue("@pincode_gid", Classifications.Pincode_gid);
                    cmds.Parameters.AddWithValue("@pincode_code", Classifications.Pincode_code);
                    cmds.Parameters.AddWithValue("@pincode_district_gid", Classifications.selecteddistrict_gid);
                    cmds.Parameters.AddWithValue("@Pincode_state_gid", Classifications.selectedstate_gid);
                    cmds.Parameters.AddWithValue("@pincode_status", Classifications.Pincode_status);
                    cmds.Parameters.AddWithValue("@pincode_isremoved", "N");
                    cmds.Parameters.AddWithValue("@pincode_insertby", int.Parse(com.GetLoginUserGid().ToString()));

                    cmds.CommandType = CommandType.StoredProcedure;
                    cmds.ExecuteNonQuery();
                    result = "success";
                }
                else
                {
                    result = "Duplicate record !";
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

        public string Deletepincode(int PinId)
        {
            string result;
            // throw new NotImplementedException();
            try
            {
                CommonIUD comm = new CommonIUD();
                SqlCommand cmd = new SqlCommand("GST_MST_PINCODE_IU", con);
                GetCon();
                cmd.Parameters.AddWithValue("@StatementType", "D");
                cmd.Parameters.AddWithValue("@pincode_gid", PinId);
                cmd.Parameters.AddWithValue("@pincode_updateby", int.Parse(com.GetLoginUserGid().ToString()));
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

        public IEnumerable<EntityGSTPincode> GetDistrictByStateId(int stateid)
        {
            //  throw new NotImplementedException();

            try
            {
                List<EntityGSTPincode> objOrgType = new List<EntityGSTPincode>();
                EntityGSTPincode objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("GST_MST_DISTRICT_SL", con);
                cmd.Parameters.AddWithValue("@StatementType", "STATEID");
                cmd.Parameters.AddWithValue("@district_state_gid", stateid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new EntityGSTPincode();
                    objModel.Pincode_district_gid = Convert.ToInt32(dt.Rows[i]["district_gid"]);
                    objModel.Pincode_district_name = dt.Rows[i]["district_name"].ToString();                    
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

     
    }


}