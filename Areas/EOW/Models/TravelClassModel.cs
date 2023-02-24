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
    public class TravelClassModel:TravelClassRepository
    {
        ErrorLog objErrorLog = new ErrorLog();
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        DataTable data = new DataTable();
        Common.CmnFunctions cmnfun = new Common.CmnFunctions();
        string result;
        public TravelClassModel()
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

        public IEnumerable<TravelClassEntity> SelectModeOfTravel()
        {
            List<TravelClassEntity> emp = new List<TravelClassEntity>();
            try
            {
               
                TravelClassEntity objModel;
                cmd = new SqlCommand("pr_iem_mst_ttransportclass", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTMODEOFTRAVEL";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new TravelClassEntity(){
                    TravelModeId = Convert.ToInt32(row["transport_gid"].ToString()),
                    TravelMode = row["transport_name"].ToString(),
                };
                    emp.Add(objModel);
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally{
                con.Close();
            } 
        }
        public IEnumerable<TravelClassEntity> SelectTravelSearch()
        {
            List<TravelClassEntity> emp = new List<TravelClassEntity>();
            try
            {
               
                TravelClassEntity objModel;
                cmd = new SqlCommand("pr_iem_mst_ttransportclass", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTTRAVELMODEL";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new TravelClassEntity(){
                    TravelId = Convert.ToInt32(row["transportclass_gid"].ToString()),
                    TravelClass = row["transportclass_name"].ToString(),
                    TravelMode = row["transportclass_transport_name"].ToString(),
                };
                    emp.Add(objModel);
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally{
                con.Close();
            }
        }

        public IEnumerable<TravelClassEntity> SelectTravelSearch(string Travelclass, string travelmode)
        {
            List<TravelClassEntity> emp = new List<TravelClassEntity>();
            try
            {
              
                TravelClassEntity objModel;
                cmd = new SqlCommand("pr_iem_mst_ttransportclass", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TransportClassName", SqlDbType.VarChar).Value = Travelclass;
                cmd.Parameters.Add("@TransportId", SqlDbType.VarChar).Value = travelmode;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTTRAVELMODEL";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new TravelClassEntity(){
                        TravelId = Convert.ToInt32(row["transportclass_gid"].ToString()),
                        TravelClass = row["transportclass_name"].ToString(),
                        TravelMode = row["transportclass_transport_name"].ToString(),
                    };
                    emp.Add(objModel);
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally{
                con.Close();
            } 
        }
        public string SubmitTravel(TravelClassEntity mode)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("pr_iem_mst_ttransportclass", con);
                cmd.Parameters.AddWithValue("@ACTION", "IfCheckDuplicateAdd");
                cmd.Parameters.AddWithValue("@TransportClassName", mode.TravelClass);
                cmd.Parameters.AddWithValue("@TansportModeId", mode.TravelModeId);
                cmd.CommandType = CommandType.StoredProcedure;
                result = (string)cmd.ExecuteScalar();
                if (result == "Not There")
                {
                    CommonIUD comm = new CommonIUD();
                    string[,] codes = new string[,]            
	                {
                        {"transportclass_transport_gid",mode.TravelModeId.ToString()},
                        {"transportclass_name",mode.TravelClass.ToString()},
                        {"transportclass_transport_name",mode.TravelMode.ToString()},
                        {"transportclass_insert_by",Convert.ToString (cmnfun.GetLoginUserGid())},
                        {"transportclass_insert_date","sysdatetime()"}
                    };
                    string tname = "iem_mst_ttransportclass";
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
        IEnumerable<TravelClassEntity> TravelClassRepository.EditTravel(int id)
        {
            List<TravelClassEntity> sub = new List<TravelClassEntity>();
            try
            {
              
                TravelClassEntity objModel;
                cmd = new SqlCommand("pr_iem_mst_ttransportclass", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TransportId", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTEDIT";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new TravelClassEntity(){
                        TravelId = Convert.ToInt32(row["transportclass_gid"].ToString()),
                        TravelClass = row["transportclass_name"].ToString(),
                        TravelMode = row["transportclass_transport_name"].ToString(),
                        TravelModeId = Convert.ToInt32(row["transportclass_transport_gid"].ToString()),
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

        string TravelClassRepository.SubmitEditTravel(TravelClassEntity mode)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("pr_iem_mst_ttransportclass", con);
                cmd.Parameters.AddWithValue("@action", "IfCheckDuplicate");
                cmd.Parameters.AddWithValue("@TransportClassName", mode.TravelClass);
                cmd.Parameters.AddWithValue("@TansportModeId", mode.TravelModeId);
                cmd.Parameters.AddWithValue("@TransportId", mode.TravelId);
                cmd.CommandType = CommandType.StoredProcedure;
                result = (string)cmd.ExecuteScalar();
                if (result == "Not There")
                {
                    CommonIUD comm = new CommonIUD();
                    string[,] codes = new string[,]            
	                {
                        {"transportclass_name",mode.TravelClass.ToString()},
                        {"transportclass_transport_name",mode.TravelMode.ToString()},
                        {"transportclass_transport_gid",mode.TravelModeId.ToString()},
                        {"transportclass_update_by",Convert.ToString (cmnfun.GetLoginUserGid())},
                        {"transportclass_update_date","sysdatetime()"}
                    };
                    string[,] whrcol = new string[,]
	                 {
                          {"transportclass_gid", mode.TravelId.ToString()},
                          {"transportclass_isremoved", "N"}
                     };
                    string tname = "iem_mst_ttransportclass";
                    result = comm.UpdateCommon(codes, whrcol, tname);
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
        public string DeleteTravel(int id)
        {
            try
            {
                CommonIUD delecomm = new CommonIUD();
                string col_value = string.Empty;
                string col_temp = string.Empty;
                try
                {

                    string[,] codes = new string[,]
	            {
                 {"transportclass_isremoved", "Y"}
	            
                };
                    string[,] whrcol = new string[,]
	            {
                {"transportclass_gid",id.ToString ()}
                };
                    string tblname = "iem_mst_ttransportclass";
                    result = delecomm.DeleteCommon(codes, whrcol, tblname);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
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