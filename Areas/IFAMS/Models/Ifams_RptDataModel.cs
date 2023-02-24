using IEM.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IEM.Areas.IFAMS.Models
{
    public class Ifams_RptDataModel:Ifams_RptRepository
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();

        CmnFunctions objCmnFunctions = new CmnFunctions();
        CommonIUD objCommonIUD = new CommonIUD();
        ErrorLog objErrorLog = new ErrorLog();
        private void GetConnection()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                    con.Open();
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

        }

        public DataSet GetSplitSummary() 
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_rpt_SplitReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getsplitreport";
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return ds;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        public DataSet GetMergeSummary() 
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_rpt_SplitReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getmergereport";
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return ds;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        public DataSet GetAssetDetails(string LocationCode = null, string capdate = null)
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_rpt_SplitReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@locationcode", SqlDbType.VarChar).Value = LocationCode;
                cmd.Parameters.Add("@capdate", SqlDbType.VarChar).Value = capdate;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getassetdetailsbylocation";
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return ds;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        public DataSet GetReductionSummary() 
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_rpt_SplitReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "reductionreport";
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return ds;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }

        public DataTable GetBatchtally(string DateFrom)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                cmd = new SqlCommand("PR_Get_Batchtally", con);
                cmd.Parameters.Add("@date", SqlDbType.VarChar).Value = DateFrom.ToString();
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return dt;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }


        public DataSet GetEntries(string ECFNO)
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("Pr_IEM_EntriesRpt", con);
                cmd.Parameters.Add("@ECFNO", SqlDbType.VarChar).Value = ECFNO.ToString();
                cmd.Parameters.Add("@Userid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return ds;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }


        #region Control Report

        public DataSet TrantoOgl(string DateFrom, string todate)
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("Pr_ControlRpt_Tran_to_Ogl", con);
                cmd.Parameters.AddWithValue("@StartDate", DateFrom);
                cmd.Parameters.AddWithValue("@EndDate", todate);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return ds;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }

        public DataSet BasetoTran(string DateFrom, string todate)
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ControlRpt_PaymentToTran", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", DateFrom);
                cmd.Parameters.AddWithValue("@EndDate", todate);
                da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return ds;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        #endregion


        #region FAControl Report
        public DataTable FAExport(string datefrom, string dateto)
        {
            DataTable dt = new DataTable();
            Ifams_RptEntity obj = new Ifams_RptEntity();
            List<Ifams_RptEntity> Objlist = new List<Ifams_RptEntity>();
            try
            {
                GetConnection();
                cmd = new SqlCommand("Pr_FA_ControlRpt", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DateFrom", datefrom);
                cmd.Parameters.AddWithValue("@DateTo", dateto);
                da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.Fill(dt);
                return dt;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw ex;
            }
        }
        #endregion

        #region //FAControlReport for Tran to OGL
        public DataSet FATrantoOgl(string datefrom, string dateto)
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("Pr_FAControlRpt_Tran_to_Ogl", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StartDate", datefrom);
                cmd.Parameters.AddWithValue("@EndDate", dateto);
                da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.Fill(ds);
                return ds;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw ex;
            }

        }
        #endregion


        #region //Central ECF Report
        public DataSet CentralECFReport(string datefrom, string dateto, string dep, Int64 user)
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("Pr_CentralECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", datefrom);
                cmd.Parameters.AddWithValue("@ToDate", dateto);
                cmd.Parameters.AddWithValue("@Department", dep);
                cmd.Parameters.AddWithValue("@Userid", user);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Search";
                da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.Fill(ds);
                return ds;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw ex;
            }

        }

        public List<Ifams_RptEntity> GetCTECFReport(string datefrom, string dateto, string dep, Int64 user)
        {
            DataTable dt = new DataTable();
            List<Ifams_RptEntity> Model = new List<Ifams_RptEntity>();
            Ifams_RptEntity obj = new Ifams_RptEntity();
            try
            {
                GetConnection();
                cmd = new SqlCommand("Pr_CentralECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Search";
                cmd.Parameters.AddWithValue("@FromDate", datefrom);
                cmd.Parameters.AddWithValue("@ToDate", dateto);
                cmd.Parameters.AddWithValue("@Department", dep);
                cmd.Parameters.AddWithValue("@Userid", user);
                da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {

                    obj = new Ifams_RptEntity();
                    obj.ReceivedDate = row["ReceivedDate"].ToString();
                    obj.Name = row["Name"].ToString();
                    obj.InvoiceAmount = Convert.ToDecimal(row["InvoiceAmount"]);
                    obj.InvoiceDate = row["InvoiceDate"].ToString();
                    obj.InvoiceDesc = row["InvoiceDesc"].ToString();
                    obj.InvoiceNo = row["InvoiceNo"].ToString();
                    obj.PaymentDate = row["PaymentDate"].ToString();
                    obj.poheader_pono = row["poheader_pono"].ToString();
                    obj.Raiser = row["Raiser"].ToString();
                    obj.ECFNo = row["ECFNo"].ToString();
                    obj.InvoiceNo = row["InvoiceNo"].ToString();
                    obj.ECFStatus = row["ECFStatus"].ToString();
                    obj.Queue = row["Queue"].ToString();
                    obj.Aging = row["Aging"].ToString();
                    obj.queue_date = row["queue_date"].ToString();
                    obj.Status = row["Status"].ToString();
                    obj.queue_action_Date = row["queue_action_Date"].ToString();
                    obj.CTECFStatus = row["CTECFStatus"].ToString();
                    obj.Department = row["Department"].ToString();
                    Model.Add(obj);
                }
                return Model;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public List<Ifams_RptEntity> Autodepsummary(string term, Int64 user)
        {
            List<Ifams_RptEntity> Model = new List<Ifams_RptEntity>();
            Ifams_RptEntity obj = new Ifams_RptEntity();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("Pr_CentralECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "AutoDepid";
                cmd.Parameters.Add("@Department", SqlDbType.VarChar).Value = term.Trim();
                cmd.Parameters.AddWithValue("@Userid", user);
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new Ifams_RptEntity();
                    obj.Depname = row["Depname"].ToString();
                    Model.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                con.Close();

            }

            return Model;
        }
        #endregion
    }
}