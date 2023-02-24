using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using IEM.Common;
using System.Configuration;
using System.Text.RegularExpressions;

namespace IEM.Areas.EOW.Models
{
    public class RetentiorealeaseReport : RetentiorealeaseReportepository
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();

        CmnFunctions objCmnFunctions = new CmnFunctions();
        CommonIUD objCommonIUD = new CommonIUD();
        CmnFunctions com = new CmnFunctions();

        private void GetConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }
        public IEnumerable<Eow_RetentionRealeaseReport> RetentioRealeaseReport(string retentionfromdate, string retentiontodate, string suppcode, string suppname, string invoiceno, string ecfno)
        {
            try
            {
                List<Eow_RetentionRealeaseReport> objOrgType = new List<Eow_RetentionRealeaseReport>();
                GetConnection();
                Eow_RetentionRealeaseReport objModel;
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_trn_RetentionRelease", con);
                cmd.Parameters.AddWithValue("@ACTION", "RetentionReleaseReportonly");
                if (retentionfromdate != "")
                {
                    cmd.Parameters.AddWithValue("@retentionrelease_from_date", com.convertoDateTimeString(retentionfromdate));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@retentionrelease_from_date", retentionfromdate);
                }
                cmd.Parameters.AddWithValue("@supplierheader_suppliercode", suppcode);
                if (retentiontodate != "")
                {
                    cmd.Parameters.AddWithValue("@retentionrelease_to_date", com.convertoDateTimeString(retentiontodate));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@retentionrelease_to_date", retentiontodate);
                }
                cmd.Parameters.AddWithValue("@supplierheader_name", suppname);               
                cmd.Parameters.AddWithValue("@invoice_no", invoiceno);
                cmd.Parameters.AddWithValue("@ecf_no", ecfno);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //objModel = new Eow_RetentionRealeaseReport();
                    //objModel.retentionrelease_gid = Convert.ToInt32(dt.Rows[i]["retentionrelease_gid"].ToString());
                    //objModel.retentionrelease_date = Convert.ToString(dt.Rows[i]["Retention Date"].ToString());
                    //objModel.supplierheader_suppliercode = Convert.ToString(dt.Rows[i]["Suppiler Code"].ToString());
                    //objModel.supplierheader_name = Convert.ToString(dt.Rows[i]["Supplier Name"].ToString());
                    //objModel.retentionrelease_amount = Convert.ToDecimal(dt.Rows[i]["Retention Release Amount"].ToString());
                    //objModel.retentionrelease_on = Convert.ToString(dt.Rows[i]["Retention Release Date"].ToString());
                    //objModel.invoice_no = Convert.ToString(dt.Rows[i]["Invoice No"].ToString());
                    //objModel.ecf_no = Convert.ToString(dt.Rows[i]["ECF No"].ToString());
                    //objModel.employee_name = Convert.ToString(dt.Rows[i]["Retention Release By"].ToString());
                    //objModel.retentionrelease_cancel_by = Convert.ToString(dt.Rows[i]["Cancel By"].ToString());
                    //objModel.retentionrelease_cancel_date = Convert.ToString(dt.Rows[i]["Cancel Date"].ToString());
                    //objOrgType.Add(objModel);
                    objModel = new Eow_RetentionRealeaseReport();
                    // objModel.retentionrelease_gid = Convert.ToInt32(dt.Rows[i]["retentionrelease_gid"].ToString());
                    objModel.retentionrelease_date = Convert.ToString(dt.Rows[i]["Retention Date"].ToString());
                    objModel.supplierheader_suppliercode = Convert.ToString(dt.Rows[i]["Suppiler Code"].ToString());
                    objModel.supplierheader_name = Convert.ToString(dt.Rows[i]["Supplier Name"].ToString());
                    objModel.retention_amount = Convert.ToDecimal(dt.Rows[i]["Retention Amount"].ToString());
                    objModel.retention_releaseamount = Convert.ToDecimal(dt.Rows[i]["Release Amount"].ToString());
                    //objModel.retention_exception = Convert.ToDecimal(dt.Rows[i]["Cancel Amount"].ToString());
                    objModel.employee_name = Convert.ToString(dt.Rows[i]["Retention Release By"].ToString());
                    objModel.retentionrelease_on = Convert.ToString(dt.Rows[i]["Retention Release Date"].ToString());
                    objModel.invoice_no = Convert.ToString(dt.Rows[i]["Invoice No"].ToString());
                    objModel.ecf_no = Convert.ToString(dt.Rows[i]["ECF No"].ToString());
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

        public IEnumerable<Eow_RetentionRealeaseReport> RetentioRealeaseReport()
        {
            try
            {
                
                List<Eow_RetentionRealeaseReport> objOrgType = new List<Eow_RetentionRealeaseReport>();
                GetConnection();
                Eow_RetentionRealeaseReport objModel;
                SqlCommand cmd = new SqlCommand("pr_iem_trn_RetentionRelease", con);
                cmd.Parameters.AddWithValue("@ACTION", "SelecrReleaseReportonly");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                foreach(DataRow row in dt.Rows)
                {
                    objModel = new Eow_RetentionRealeaseReport();
                    if (row["invoice_gid"].ToString() != null && row["invoice_gid"].ToString() != "")
                    {
                        objModel.invoice_gid = Convert.ToInt32(row["invoice_gid"].ToString());
                    }
                    //objModel.retentionrelease_date = row["retention_date"].ToString();
                    objModel.supplierheader_suppliercode =row["supplierheader_suppliercode"].ToString();
                    objModel.supplierheader_name = row["supplierheader_name"].ToString();
                    objModel.retention_amount = Convert.ToDecimal(row["invoice_retention_amount"].ToString());
                    objModel.retention_releaseamount = Convert.ToDecimal(row["retentionrelease_amount"].ToString());
                    objModel.employee_name = row["employee_name"].ToString();
                    objModel.retentionrelease_on = row["retentionrelease_on"].ToString();
                    objModel.invoice_no =row["invoice_no"].ToString();
                    objModel.ecf_no = row["ecf_no"].ToString();
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

        public IEnumerable<Eow_RetentionRealeaseReport> RetentioRealeaseReportById(string reportMode, string retentionfromdate, string retentiontodate)
        {
            List<Eow_RetentionRealeaseReport> objOrgType = new List<Eow_RetentionRealeaseReport>();
            Eow_RetentionRealeaseReport objModel;
            try
            {
                if (reportMode =="Amount Released")
                {
                   
                    GetConnection();
                   // Eow_RetentionRealeaseReport objModel;
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("pr_iem_trn_RetentionRelease", con);
                    cmd.Parameters.AddWithValue("@action", "ReleaseReport");
                    //if (retentionfromdate != "")
                    //{
                    //    cmd.Parameters.AddWithValue("@retentionrelease_from_date", com.convertoDateTimeString(retentionfromdate));
                    //}
                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@retentionrelease_from_date", retentionfromdate);
                    //}
                    //cmd.Parameters.AddWithValue("@supplierheader_suppliercode", suppcode);
                    //if (retentiontodate != "")
                    //{
                    //    cmd.Parameters.AddWithValue("@retentionrelease_to_date", com.convertoDateTimeString(retentiontodate));
                    //}
                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@retentionrelease_to_date", retentiontodate);                    //}
                    
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objModel = new Eow_RetentionRealeaseReport();
                        // objModel.retentionrelease_gid = Convert.ToInt32(dt.Rows[i]["retentionrelease_gid"].ToString());
                        objModel.retentionrelease_date = Convert.ToString(dt.Rows[i]["Retention Date"].ToString());
                        objModel.supplierheader_suppliercode = Convert.ToString(dt.Rows[i]["Suppiler Code"].ToString());
                        objModel.supplierheader_name = Convert.ToString(dt.Rows[i]["Supplier Name"].ToString());
                        objModel.retention_amount = Convert.ToDecimal(dt.Rows[i]["Retention Amount"].ToString());
                        objModel.retention_releaseamount = Convert.ToDecimal(dt.Rows[i]["Release Amount"].ToString());
                        objModel.retention_exception = Convert.ToDecimal(dt.Rows[i]["Cancel Amount"].ToString());
                        objModel.employee_name = Convert.ToString(dt.Rows[i]["Retention Release By"].ToString());
                        objModel.retentionrelease_on = Convert.ToString(dt.Rows[i]["Retention Release Date"].ToString());
                        objModel.invoice_no = Convert.ToString(dt.Rows[i]["Invoice No"].ToString());
                        objModel.ecf_no = Convert.ToString(dt.Rows[i]["ECF No"].ToString());
                        objOrgType.Add(objModel);
                    }
                    return objOrgType;
                }
                else if (reportMode == "Release Cancelled")
                {
                   
                    GetConnection();                   
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("pr_iem_trn_RetentionRelease", con);
                    cmd.Parameters.AddWithValue("@action", "CancelReport");
                    //if (retentionfromdate != "")
                    //{
                    //    cmd.Parameters.AddWithValue("@retentionrelease_from_date", com.convertoDateTimeString(retentionfromdate));
                    //}
                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@retentionrelease_from_date", retentionfromdate);
                    //}
                    //cmd.Parameters.AddWithValue("@supplierheader_suppliercode", suppcode);
                    //if (retentiontodate != "")
                    //{
                    //    cmd.Parameters.AddWithValue("@retentionrelease_to_date", com.convertoDateTimeString(retentiontodate));
                    //}
                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@retentionrelease_to_date", retentiontodate);                    //}

                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objModel = new Eow_RetentionRealeaseReport();
                        // objModel.retentionrelease_gid = Convert.ToInt32(dt.Rows[i]["retentionrelease_gid"].ToString());
                        objModel.retentionrelease_date = Convert.ToString(dt.Rows[i]["Retention Date"].ToString());
                        objModel.supplierheader_suppliercode = Convert.ToString(dt.Rows[i]["Suppiler Code"].ToString());
                        objModel.supplierheader_name = Convert.ToString(dt.Rows[i]["Supplier Name"].ToString());
                        objModel.retention_amount = Convert.ToDecimal(dt.Rows[i]["Retention Amount"].ToString());                        
                        objModel.retention_exception = Convert.ToDecimal(dt.Rows[i]["Cancel Amount"].ToString());
                        objModel.employee_name = Convert.ToString(dt.Rows[i]["Retention Cancel By"].ToString());
                        objModel.retentionrelease_cancel_date = Convert.ToString(dt.Rows[i]["Retention Cancel Date"].ToString());
                        objModel.invoice_no = Convert.ToString(dt.Rows[i]["Invoice No"].ToString());
                        objModel.ecf_no = Convert.ToString(dt.Rows[i]["ECF No"].ToString());
                        objOrgType.Add(objModel);
                    }
                    return objOrgType;
                }
                else if (reportMode == "Expiry Date Extended")
                {

                    GetConnection();
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("pr_iem_trn_RetentionRelease", con);
                    cmd.Parameters.AddWithValue("@action", "ExtentedReport");
                    //if (retentionfromdate != "")
                    //{
                    //    cmd.Parameters.AddWithValue("@retentionrelease_from_date", com.convertoDateTimeString(retentionfromdate));
                    //}
                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@retentionrelease_from_date", retentionfromdate);
                    //}
                    //cmd.Parameters.AddWithValue("@supplierheader_suppliercode", suppcode);
                    //if (retentiontodate != "")
                    //{
                    //    cmd.Parameters.AddWithValue("@retentionrelease_to_date", com.convertoDateTimeString(retentiontodate));
                    //}
                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@retentionrelease_to_date", retentiontodate);                    //}

                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objModel = new Eow_RetentionRealeaseReport();                        
                        objModel.retentionrelease_date = Convert.ToString(dt.Rows[i]["Retention Date"].ToString());
                        objModel.supplierheader_suppliercode = Convert.ToString(dt.Rows[i]["Suppiler Code"].ToString());
                        objModel.supplierheader_name = Convert.ToString(dt.Rows[i]["Supplier Name"].ToString());
                        objModel.retention_expiry = Convert.ToString(dt.Rows[i]["Extented Date"].ToString());
                        objModel.employee_name = Convert.ToString(dt.Rows[i]["Extend By"].ToString());
                        objModel.invoice_no = Convert.ToString(dt.Rows[i]["Invoice No"].ToString());
                        objModel.ecf_no = Convert.ToString(dt.Rows[i]["ECF No"].ToString());
                        objOrgType.Add(objModel);
                    }
                    return objOrgType;
                }
                else if (reportMode == "Forfeiture")
                {

                    GetConnection();
                    DataTable dt = new DataTable();
                    SqlCommand cmd = new SqlCommand("pr_iem_trn_RetentionRelease", con);
                    cmd.Parameters.AddWithValue("@action", "ForfeitureReport");
                    //if (retentionfromdate != "")
                    //{
                    //    cmd.Parameters.AddWithValue("@retentionrelease_from_date", com.convertoDateTimeString(retentionfromdate));
                    //}
                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@retentionrelease_from_date", retentionfromdate);
                    //}
                    //cmd.Parameters.AddWithValue("@supplierheader_suppliercode", suppcode);
                    //if (retentiontodate != "")
                    //{
                    //    cmd.Parameters.AddWithValue("@retentionrelease_to_date", com.convertoDateTimeString(retentiontodate));
                    //}
                    //else
                    //{
                    //    cmd.Parameters.AddWithValue("@retentionrelease_to_date", retentiontodate);                    //}

                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objModel = new Eow_RetentionRealeaseReport();
                        objModel.retentionrelease_date = Convert.ToString(dt.Rows[i]["Retention Date"].ToString());
                        objModel.supplierheader_suppliercode = Convert.ToString(dt.Rows[i]["Suppiler Code"].ToString());
                        objModel.supplierheader_name = Convert.ToString(dt.Rows[i]["Supplier Name"].ToString());
                        //objModel.retention_expiry = Convert.ToString(dt.Rows[i]["Extented Date"].ToString());
                        objModel.employee_name = Convert.ToString(dt.Rows[i]["Forfeiture By"].ToString());
                        objModel.invoice_no = Convert.ToString(dt.Rows[i]["Invoice No"].ToString());
                        objModel.ecf_no = Convert.ToString(dt.Rows[i]["ECF No"].ToString());
                        objOrgType.Add(objModel);
                    }
                    return objOrgType;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return objOrgType;
        }
        public IEnumerable<Eow_RetentionRealeaseReport> RetentioRealeaseView( int invoice_gid)
        {
            List<Eow_RetentionRealeaseReport> objOrgType = new List<Eow_RetentionRealeaseReport>();
            Eow_RetentionRealeaseReport objModel;
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_trn_RetentionRelease", con);
                cmd.Parameters.AddWithValue("@invoice_gid", invoice_gid);
                cmd.Parameters.AddWithValue("@action", "SelectReportView");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
               foreach(DataRow row in dt.Rows)
                {
                    objModel = new Eow_RetentionRealeaseReport();
                    objModel.retentionrelease_date = row["retention_date"].ToString();
                    objModel.retention_amount =Convert.ToDecimal(row["retention_amount"].ToString());
                    objModel.retention_releaseamount =Convert.ToDecimal(row["retention_releaseamount"].ToString());
                    objModel.retention_exception =Convert.ToDecimal(row["retention_exception"].ToString());
                    objModel.retention_expiry = row["retention_expiry"].ToString();
                    objModel.RetentionStatus = row["retention_status"].ToString();
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