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
    public class RetentionInvoiceReport : RetentionInvoiceReportRepository
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        ErrorLog objErrorLog = new ErrorLog();
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
        public IEnumerable<EOW_RetentionInvoiceReport> RetentioinInvoiceReport()
        {
            List<EOW_RetentionInvoiceReport> objOrgType = new List<EOW_RetentionInvoiceReport>();
            try
            {
              
                GetConnection();
                EOW_RetentionInvoiceReport objModel;
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_trn_RetentionInvoiceReport", con);
                cmd.Parameters.AddWithValue("@action", "RetentionInvoiceReport");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new EOW_RetentionInvoiceReport();
                    objModel.invoice_date = Convert.ToString(dt.Rows[i]["Invoice Date"].ToString());
                    objModel.supplierheader_suppliercode = Convert.ToString(dt.Rows[i]["Supplier Code"].ToString());
                    objModel.supplierheader_name = Convert.ToString(dt.Rows[i]["Supplier Name"].ToString());                  
                    objModel.invoice_no = Convert.ToString(dt.Rows[i]["Invoice No"].ToString());
                    objModel.ecf_no = Convert.ToString(dt.Rows[i]["ECF No"].ToString());
                    objModel.employee_name = Convert.ToString(dt.Rows[i]["Employee Name"].ToString());
                    objModel.invoice_type = Convert.ToString(dt.Rows[i]["Invoice Type"].ToString());
                    objModel.invoice_service_month = Convert.ToString(dt.Rows[i]["Service Month"].ToString());
                    objModel.Description = Convert.ToString(dt.Rows[i]["Description"].ToString());
                    objModel.invoice_amount = Convert.ToDecimal(dt.Rows[i]["Invoice Amount"].ToString());
                    objModel.invoice_wotax_amount = Convert.ToDecimal(dt.Rows[i]["Tax Amount"].ToString());
                    objModel.invoice_payment_nett = Convert.ToString(dt.Rows[i]["Net Payment"].ToString());
                    objModel.invoice_dedup_no = Convert.ToString(dt.Rows[i]["Dedup No"].ToString());
                    objModel.invoice_dedup_status = Convert.ToInt32(dt.Rows[i]["Dedup Status"].ToString());
                    objModel.invoice_provision_flag = Convert.ToString(dt.Rows[i]["Provision Flag"].ToString());
                    objModel.invoice_retention_flag = Convert.ToString(dt.Rows[i]["Retention Flag"].ToString());
                    objModel.invoice_retention_rate = Convert.ToDecimal(dt.Rows[i]["Retention Rate"].ToString());
                    objModel.invoice_retention_amount = Convert.ToDecimal(dt.Rows[i]["Retention Amount"].ToString());
                    objModel.invoice_retention_exception = Convert.ToDecimal(dt.Rows[i]["Exception Amount"].ToString());
                    objModel.invoice_retention_curr_status = Convert.ToInt32(dt.Rows[i]["Retention Status"].ToString());
                    objModel.invoice_retention_releaseon = Convert.ToString(dt.Rows[i]["Retention Release Date"].ToString());
                    if (objModel.invoice_type.Contains("S"))
                    {
                        objModel.invoice_type = Convert.ToString(ConfigurationManager.AppSettings["S"].ToString());
                    }
                    else
                    {
                        objModel.invoice_type = Convert.ToString(ConfigurationManager.AppSettings["E"].ToString());
                    }

                    objOrgType.Add(objModel);
                }
                return objOrgType;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objOrgType;
            }
            finally
            {
                con.Close();
            }
        }

        public IEnumerable<EOW_RetentionInvoiceReport> RetentioinInvoiceReport(string retentionfromdate, string retentiontodate, string suppcode, string suppname, string invoiceno, string ecfno)
        {
            List<EOW_RetentionInvoiceReport> objOrgType = new List<EOW_RetentionInvoiceReport>();
            try
            {
               
                GetConnection();
                EOW_RetentionInvoiceReport objModel;
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_trn_RetentionInvoiceReport", con);
                cmd.Parameters.AddWithValue("@action", "RetentionInvoiceReport");
                if (retentionfromdate != "")
                {
                    cmd.Parameters.AddWithValue("@invoice_from_date", com.convertoDateTimeString(retentionfromdate));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@invoice_from_date", retentionfromdate);
                }
                cmd.Parameters.AddWithValue("@supplierheader_suppliercode", suppcode);
                if (retentiontodate != "")
                {
                    cmd.Parameters.AddWithValue("@invoice_to_date", com.convertoDateTimeString(retentiontodate));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@invoice_to_date", retentiontodate);
                }
                cmd.Parameters.AddWithValue("@supplierheader_name", suppname);
                //cmd.Parameters.AddWithValue("@retentionrelease_amount", amount);
                cmd.Parameters.AddWithValue("@invoice_no", invoiceno);
                cmd.Parameters.AddWithValue("@ecf_no", ecfno);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new EOW_RetentionInvoiceReport();
                    objModel.invoice_date = Convert.ToString(dt.Rows[i]["Invoice Date"].ToString());
                    objModel.supplierheader_suppliercode = Convert.ToString(dt.Rows[i]["Supplier Code"].ToString());
                    objModel.supplierheader_name = Convert.ToString(dt.Rows[i]["Supplier Name"].ToString());
                    objModel.invoice_no = Convert.ToString(dt.Rows[i]["Invoice No"].ToString());
                    objModel.ecf_no = Convert.ToString(dt.Rows[i]["ECF No"].ToString());
                    objModel.employee_name = Convert.ToString(dt.Rows[i]["Employee Name"].ToString());
                    objModel.invoice_type = Convert.ToString(dt.Rows[i]["Invoice Type"].ToString());
                    objModel.invoice_service_month = Convert.ToString(dt.Rows[i]["Service Month"].ToString());
                    objModel.Description = Convert.ToString(dt.Rows[i]["Description"].ToString());
                    objModel.invoice_amount = Convert.ToDecimal(dt.Rows[i]["Invoice Amount"].ToString());
                    objModel.invoice_wotax_amount = Convert.ToDecimal(dt.Rows[i]["Tax Amount"].ToString());
                    objModel.invoice_payment_nett = Convert.ToString(dt.Rows[i]["Net Payment"].ToString());
                    objModel.invoice_dedup_no = Convert.ToString(dt.Rows[i]["Dedup No"].ToString());
                    objModel.invoice_dedup_status = Convert.ToInt32(dt.Rows[i]["Dedup Status"].ToString());
                    objModel.invoice_provision_flag = Convert.ToString(dt.Rows[i]["Provision Flag"].ToString());
                    objModel.invoice_retention_flag = Convert.ToString(dt.Rows[i]["Retention Flag"].ToString());
                    objModel.invoice_retention_rate = Convert.ToDecimal(dt.Rows[i]["Retention Rate"].ToString());
                    objModel.invoice_retention_amount = Convert.ToDecimal(dt.Rows[i]["Retention Amount"].ToString());
                    objModel.invoice_retention_exception = Convert.ToDecimal(dt.Rows[i]["Exception Amount"].ToString());
                    objModel.invoice_retention_curr_status = Convert.ToInt32(dt.Rows[i]["Retention Status"].ToString());
                    objModel.invoice_retention_releaseon = Convert.ToString(dt.Rows[i]["Retention Release Date"].ToString());
                    if (objModel.invoice_type.Contains("S"))
                    {
                        objModel.invoice_type = Convert.ToString(ConfigurationManager.AppSettings["S"].ToString());
                    }
                    else
                    {
                        objModel.invoice_type = Convert.ToString(ConfigurationManager.AppSettings["E"].ToString());
                    }

                    objOrgType.Add(objModel);
                }
                return objOrgType;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objOrgType;
            }
            finally
            {
                con.Close();
            }
        }
    }
}