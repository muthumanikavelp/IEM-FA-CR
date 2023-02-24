using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using IEM.Common;
namespace IEM.Areas.EOW.Models
{
    public class HolidayMasterModel : HolidayRepository
    {
        ErrorLog objErrorLog = new ErrorLog();
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        DataTable data = new DataTable();
        Common.CmnFunctions cmn = new Common.CmnFunctions();
        SupClassificationModel sub = new SupClassificationModel();
        
        string result;
        public HolidayMasterModel()
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

        public IEnumerable<SupClassificationModel> GetClassification()
        {
            throw new NotImplementedException();
        }
        public DataTable SelectStates()
        {
            try
            {
                SupClassificationModel obj1 = new SupClassificationModel();
                cmd = new SqlCommand("pr_iem_mst_tholiday", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTSTATES";
                da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 0;
                da.Fill(data);
                return data;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return data;
            }
        }
        public string AddHoliday(SupClassificationModel sub)
        {
            try
            {
                cmd = new SqlCommand("pr_iem_mst_tholiday", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@HOLIDAYNAME", SqlDbType.VarChar).Value = sub.HolidayName;
                cmd.Parameters.Add("@HOLIDAYDATE", SqlDbType.DateTime).Value = cmn.convertoDateTime(sub.HolidayDate);
                cmd.Parameters.Add("@NATIONALHOLIDAY", SqlDbType.Char).Value = sub.NationalHoliday;
                cmd.Parameters.Add("@STATEHOLIDAY", SqlDbType.Char).Value = sub.StateHoliday;
                cmd.Parameters.Add("@CUTOFF", SqlDbType.Char).Value = sub.Cutoff;
                cmd.Parameters.Add("@HOLIDAYSTATE_STATE_GID", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@INSERTEDBY", SqlDbType.Int).Value = Convert.ToInt32(cmn.GetLoginUserGid());
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "ADDHOLIDAY";
                result = (string)cmd.ExecuteScalar();
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
            return result;
        }

        public string AddHolidayState(SupClassificationModel sub, string[] SelectedState)
        {
            try
            {
                con.Open();
                foreach (string selctedstate in SelectedState)
                {
                    cmd = new SqlCommand("pr_iem_mst_tholiday", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@HOLIDAYSTATE_STATE_GID", SqlDbType.Int).Value =Convert.ToInt16(selctedstate);
                    cmd.Parameters.Add("@INSERTEDBY", SqlDbType.Int).Value = Convert.ToInt32(cmn.GetLoginUserGid());
                    cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "ADDHOLIDAYSTATE";
                    result = (string)cmd.ExecuteScalar();
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
            return result;

        }

        public IEnumerable<SupClassificationModel> SelectHoliday()
        {
            List<SupClassificationModel> sub = new List<SupClassificationModel>();
            try
            {
              
                SupClassificationModel objModel;
                cmd = new SqlCommand("pr_iem_mst_tholiday", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTHOLIDAY";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new SupClassificationModel();
                    objModel.Holiday_gid = Convert.ToInt32(row["holiday_gid"].ToString());
                    objModel.HolidayDate = row["holiday_date"].ToString();
                    objModel.HolidayName = row["holiday_name"].ToString();
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
        public DataTable ViewHoliday(int id)
        {
            try { 
            SupClassificationModel obj1 = new SupClassificationModel();
            cmd = new SqlCommand("pr_iem_mst_tholiday", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@HOLIDAYGID", SqlDbType.VarChar).Value = id;
            cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "VIEWHOLIDAY";
            da = new SqlDataAdapter(cmd);
            da.SelectCommand.CommandTimeout = 0;
            dt = new DataTable();
            da.Fill(dt);
            return dt;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return dt;
            }
        } 
        
        public string EditHoliday(SupClassificationModel sub,int holidaygid)
        {
            try
            {
                cmd = new SqlCommand("pr_iem_mst_tholiday", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@HOLIDAYGID", SqlDbType.VarChar).Value = holidaygid;
                cmd.Parameters.Add("@HOLIDAYNAME", SqlDbType.VarChar).Value = sub.HolidayName;
                cmd.Parameters.Add("@HOLIDAYDATE", SqlDbType.DateTime).Value =cmn.convertoDateTimeString(sub.HolidayDate.ToString());
                cmd.Parameters.Add("@NATIONALHOLIDAY", SqlDbType.Char).Value = sub.NationalHoliday;
                cmd.Parameters.Add("@STATEHOLIDAY", SqlDbType.Char).Value = sub.StateHoliday;
                cmd.Parameters.Add("@CUTOFF", SqlDbType.Char).Value = sub.Cutoff;
                cmd.Parameters.Add("@HOLIDAYSTATE_STATE_GID", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@UPDATEDBY", SqlDbType.Int).Value = Convert.ToInt32(cmn.GetLoginUserGid());
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "EDITHOLIDAY";
                result = (string)cmd.ExecuteScalar();
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
            return result;
        }
        public string EditHolidayState(SupClassificationModel sub, string[] SelectedState,int holidaygid)
        {
            try
            {
                con.Open();
                foreach (string selctedstate in SelectedState)
                {
                    cmd = new SqlCommand("pr_iem_mst_tholiday", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@HOLIDAYSTATE_HOLIDAY_GID", SqlDbType.Int).Value = holidaygid;
                    cmd.Parameters.Add("@HOLIDAYSTATE_STATE_GID", SqlDbType.Int).Value = Convert.ToInt16(selctedstate);
                    cmd.Parameters.Add("@UPDATEDBY", SqlDbType.Int).Value = Convert.ToInt32(cmn.GetLoginUserGid());
                    cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "EDITHOLIDAYSTATE";
                    result = (string)cmd.ExecuteScalar();
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
            return result;

        }
        public string DeleteHoliday(SupClassificationModel sub, int holidaygid)
        {
            try
            {
                cmd = new SqlCommand("pr_iem_mst_tholiday", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@HOLIDAYGID", SqlDbType.VarChar).Value = holidaygid;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "DELETEHOLIDAY";
                result = (string)cmd.ExecuteScalar();
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
            return result;
        }
        public string DeleteHolidayState(SupClassificationModel sub,int holidaygid)
        {
            try
            {
                con.Open();
                cmd = new SqlCommand("pr_iem_mst_tholiday", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@HOLIDAYGID", SqlDbType.VarChar).Value = holidaygid;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "DELETEHOLIDAYSTATE";
                result = (string)cmd.ExecuteScalar();
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
            return result;
        }
    }
}