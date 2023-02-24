using IEM.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace IEM.Areas.EOW.Models
{
    public class ModeofTravelModel:ModeOftravelRepository
    {
        ErrorLog objErrorLog = new ErrorLog();
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        DataTable data = new DataTable();
        Common.CmnFunctions cmnfun = new Common.CmnFunctions();

        string result;
        public ModeofTravelModel()
        {
            GetConnection();
        }
        private void GetConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }
        public string SubmitMode(ModeOfTravelEntity mode)
        {
            try{
                SqlCommand cmd = new SqlCommand("pr_iem_mst_ttransport", con);
                cmd.Parameters.AddWithValue("@ACTION", "IfCheckDuplicateAdd");
                cmd.Parameters.AddWithValue("@TransportName", mode.ModeOfTravel);
                cmd.CommandType = CommandType.StoredProcedure;
                result = (string)cmd.ExecuteScalar();
                if (result == "Not There")
                {
                    CommonIUD comm = new CommonIUD();
                    string[,] codes = new string[,]            
	                {
                        {"transport_name",mode.ModeOfTravel.ToString()},
                        {"transport_flag",mode.ModeFlag.ToString()},
                        {"transport_insert_by",Convert.ToString (cmnfun.GetLoginUserGid())},
                        {"transport_insert_date","sysdatetime()"}
                    };
                    string tname = "iem_mst_ttransport";
                    result = comm.InsertCommon(codes, tname);
                    if (result == "success")
                    {
                        result = "sub";
                    }
                }
                else
                {
                    return "duplicate";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally{
                con.Close();
            }

            return result;
        }
        public IEnumerable<ModeOfTravelEntity> SelectTravelMode()
        {
            List<ModeOfTravelEntity> sub = new List<ModeOfTravelEntity>();
            try
            {
              
                ModeOfTravelEntity objModel;
                cmd = new SqlCommand("pr_iem_mst_ttransport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTTRAVELMODEL";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new ModeOfTravelEntity(){
                    ModeId = Convert.ToInt32(row["transport_gid"].ToString()),
                    ModeOfTravel = row["transport_name"].ToString(),
                };
                    sub.Add(objModel);
                }
                return sub;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return sub;
            }
            finally
            {
                con.Close();
            }
        }
        public IEnumerable<ModeOfTravelEntity> SelectTravelMode(string modename)
        {
            List<ModeOfTravelEntity> sub = new List<ModeOfTravelEntity>();
            try
            {
              
                ModeOfTravelEntity objModel;
                cmd = new SqlCommand("pr_iem_mst_ttransport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TransportName", SqlDbType.VarChar).Value = modename;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTTRAVELMODEL";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new ModeOfTravelEntity(){
                        ModeId = Convert.ToInt32(row["transport_gid"].ToString()),
                        ModeOfTravel = row["transport_name"].ToString(),
                    };
                    sub.Add(objModel);
                }
                return sub;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return sub;
            }
            finally{
                con.Close();
            }
        }
        IEnumerable<ModeOfTravelEntity>EditMode(int id)
        {
            List<ModeOfTravelEntity> sub = new List<ModeOfTravelEntity>();
            try
            {
              
                ModeOfTravelEntity objModel;
                cmd = new SqlCommand("pr_iem_mst_ttransport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TransportId", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTTRAVELMODELVIEW";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new ModeOfTravelEntity()
                    {
                        ModeId = Convert.ToInt32(row["transport_gid"].ToString()),
                        ModeOfTravel = row["transport_name"].ToString(),
                        ModeFlag = row["transport_flag"].ToString()
                    };
                    sub.Add(objModel);
                }
                return sub;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return sub;
            }
            finally
            {
                con.Close();
            }
        }
        public string SubmitEditMode(ModeOfTravelEntity mode)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("pr_iem_mst_ttransport", con);
                cmd.Parameters.AddWithValue("@action", "IfCheckDuplicate");
                cmd.Parameters.AddWithValue("@TransportId", mode.ModeId);
                cmd.Parameters.AddWithValue("@TransportName", mode.ModeOfTravel);
                cmd.CommandType = CommandType.StoredProcedure;
                result = (string)cmd.ExecuteScalar();
                if (result == "Not There")
                {
                    CommonIUD comm = new CommonIUD();
                    string[,] codes = new string[,]            
	                {
                        {"transport_name",mode.ModeOfTravel.ToString()},
                        {"transport_flag",mode.ModeFlag.ToString()},
                        {"transport_update_by",Convert.ToString (cmnfun.GetLoginUserGid())},
                        {"transport_update_date","sysdatetime()"}
                    };
                    string[,] whrcol = new string[,]
	                 {
                          {"transport_gid", mode.ModeId.ToString()},
                          {"transport_isremoved", "N"}
                     };
                    string tname = "iem_mst_ttransport";
                    result = comm.UpdateCommon(codes,whrcol,tname);
                    result = "sub";
                }
                else
                {
                    return "duplicate";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally{
                con.Close();
            }

            return result;
        }

        IEnumerable<ModeOfTravelEntity> ModeOftravelRepository.EditMode(int id)
        {
            List<ModeOfTravelEntity> sub = new List<ModeOfTravelEntity>();
            try
            {
                
                ModeOfTravelEntity objModel;
                cmd = new SqlCommand("pr_iem_mst_ttransport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TransportId", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTTRAVELMODELVIEW";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new ModeOfTravelEntity();
                    objModel.ModeId = Convert.ToInt32(row["transport_gid"].ToString());
                    objModel.ModeOfTravel = row["transport_name"].ToString();
                    objModel.ModeFlag = row["transport_flag"].ToString();
                    sub.Add(objModel);
                }
                return sub;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return sub;
            }
            finally{
                con.Close();
            }
        }
        public string DeleteMode(int id)
        {
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {

                string[,] codes = new string[,]
	       {
                 {"transport_isremoved", "Y"}
	            
           };
                string[,] whrcol = new string[,]
	        {
                {"transport_gid",id.ToString ()}
            };
                string tblname = "iem_mst_ttransport";
                result = delecomm.DeleteCommon(codes, whrcol, tblname);
                if(result=="Sucess")
                {
                    result = "Deleted";
                }
                else
                {
                    result = result;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally{
                con.Close();
            }
            return result;
        }
        public string DeleteTravel(int id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("pr_iem_mst_ttransport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ACTION", "IfDeleteCheck");
                cmd.Parameters.AddWithValue("@TransportId", SqlDbType.Int).Value = id;
                result = (string)cmd.ExecuteScalar();

                // CommonIUD delecomm = new CommonIUD();
                // string col_value = string.Empty;
                // string col_temp = string.Empty;
                // try
                // {

                //     string[,] codes = new string[,]
                //{
                //      {"transportclass_isremoved", "Y"}

                //};
                //     string[,] whrcol = new string[,]
                // {
                //     {"transportclass_transport_gid",id.ToString ()}
                // };
                //     string tblname = "iem_mst_ttransportclass";
                //     result = delecomm.DeleteCommon(codes, whrcol, tblname);
                // }
                // catch (Exception ex)
                // {
                //     throw ex;
                // }
                // finally
                // {
                //     con.Close();
                // }
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
        }
    }
}