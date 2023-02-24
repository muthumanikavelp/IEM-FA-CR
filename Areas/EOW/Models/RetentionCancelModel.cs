using IEM.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace IEM.Areas.EOW.Models
{
    public class RetentionCancelModel : RetentionCancel
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        DataTable data = new DataTable();
        SupEmployeeRole emp = new SupEmployeeRole();
        CmnFunctions cmnfun = new CmnFunctions();
        CommonIUD comm = new CommonIUD();
        System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
        string result;
        public RetentionCancelModel()
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
        public IEnumerable<RetentionCancelData> RetentionReleaseGrid()
        {
            try
            {
                List<RetentionCancelData> GridReceipt = new List<RetentionCancelData>();
                RetentionCancelData objModel;
                GetConnection();
                DataTable dt = new DataTable();

                SqlCommand cmd = new SqlCommand("pr_iem_trn_RetentionRelease", con);
                cmd.Parameters.AddWithValue("@ACTION", "SELECTCANCEL");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new RetentionCancelData();

                    objModel.invoice_gid = Convert.ToInt32(row["invoice_gid"].ToString());
                    objModel.invoice_ecf_gid = Convert.ToInt32(row["invoice_ecf_gid"].ToString());
                    objModel.ecf_date = row["ecf_date"].ToString();
                    objModel.ecf_no = row["ecf_no"].ToString();
                    objModel.ecf_suppliercode = row["supplierheader_suppliercode"].ToString();
                    objModel.ecf_suppliername = row["supplierheader_name"].ToString();
                    if (row["invoice_retention_amount"].ToString() != "" && row["invoice_retention_exception"].ToString() != "") 
                    {
                        objModel.invoice_retention_amount = Convert.ToDecimal(row["invoice_retention_amount"].ToString());
                        objModel.invoice_retention_exception = Convert.ToDecimal(row["invoice_retention_exception"].ToString());
                    }

                    objModel.invoice_no = row["invoice_no"].ToString();
                    objModel.invoice_amount = Convert.ToDouble(row["invoice_amount"].ToString());
                    objModel.Retention_Releasedate = row["retentionrelease_date"].ToString();
                    GridReceipt.Add(objModel);
                }
                return GridReceipt;
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
        public RetentionCancelData SelectById(int id)
        {
            try
            {
                List<RetentionCancelData> GridReceipt = new List<RetentionCancelData>();
                RetentionCancelData objModel;
                GetConnection();
                DataTable dt = new DataTable();

                SqlCommand cmd = new SqlCommand("pr_iem_trn_RetentionRelease", con);
                cmd.Parameters.Add("@invoice_gid", SqlDbType.Int).Value = id;
                cmd.Parameters.AddWithValue("@ACTION", "SELECTCANCEL_ID");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                
                    objModel = new RetentionCancelData();

                    objModel. invoice_gid = Convert.ToInt32(dt.Rows[0]["invoice_gid"].ToString());
                    objModel. invoice_ecf_gid = Convert.ToInt32(dt.Rows[0]["invoice_ecf_gid"].ToString());
                    objModel.ecf_date = dt.Rows[0]["ecf_date"].ToString();
                    objModel.ecf_no = dt.Rows[0]["ecf_no"].ToString();
                    objModel.ecf_suppliercode = dt.Rows[0]["supplierheader_suppliercode"].ToString();
                    objModel.ecf_suppliername = dt.Rows[0]["supplierheader_name"].ToString();
                    objModel.invoice_date = dt.Rows[0]["invoice_date"].ToString();
                    objModel.Retention_date = dt.Rows[0]["invoice_retention_releaseon"].ToString();
                    if (dt.Rows[0]["invoice_retention_amount"].ToString() != "" && dt.Rows[0]["invoice_retention_exception"].ToString()!="")
                    {
                        objModel.Retention_Amount = Convert.ToDouble(dt.Rows[0]["invoice_retention_amount"].ToString());
                        objModel.Balance_Amount = Convert.ToDouble(dt.Rows[0]["invoice_retention_exception"].ToString());
                    }
                    objModel.invoice_no = dt.Rows[0]["invoice_no"].ToString();
                    objModel.invoice_amount = Convert.ToDouble(dt.Rows[0]["invoice_amount"].ToString());
                    objModel.invoice_description = dt.Rows[0]["invoice_desc"].ToString();
                    objModel.invoice_retention_releaseon = dt.Rows[0]["invoice_retention_releaseon"].ToString();
                    objModel.Remarks = dt.Rows[0]["ecf_remark"].ToString();
                    objModel.ecf_amount = Convert.ToDouble(dt.Rows[0]["ecf_amount"].ToString());
                    GridReceipt.Add(objModel);
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


        public IEnumerable<RetentionCancelData> Search(string ReleaseDate = null, string ECFDate = null, string ECFNo = null, string InvoiceNo = null, string Suppliercode = null, string Suppliername = null, string extendeddate = null)
        {
            try
            {
                List<RetentionCancelData> GridReceipt = new List<RetentionCancelData>();
                RetentionCancelData objModel;
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_trn_RetentionRelease", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (ReleaseDate != "")
                {
                    cmd.Parameters.Add("@retentionrelease_date", SqlDbType.VarChar).Value =cmnfun.convertoDateTimeString(ReleaseDate);
                }
                if (ECFDate != "")
                {
                    cmd.Parameters.Add("@ecfdate", SqlDbType.VarChar).Value =cmnfun.convertoDateTimeString(ECFDate);
                }
                cmd.Parameters.Add("@ecf_no", SqlDbType.VarChar).Value = ECFNo;
                cmd.Parameters.Add("@invoice_no", SqlDbType.VarChar).Value = InvoiceNo;
                cmd.Parameters.Add("@supplierheader_suppliercode", SqlDbType.VarChar).Value = Suppliercode;
                cmd.Parameters.Add("@supplierheader_name", SqlDbType.VarChar).Value = Suppliername;
                cmd.Parameters.Add("@invoice_gid", SqlDbType.VarChar).Value = extendeddate;
                cmd.Parameters.AddWithValue("@ACTION", "SEARCH");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new RetentionCancelData()
                    {
                        invoice_gid = Convert.ToInt32(row["invoice_gid"].ToString()),
                        invoice_ecf_gid = Convert.ToInt32(row["invoice_ecf_gid"].ToString()),
                        ecf_date = row["ecf_date"].ToString(),
                        ecf_no = row["ecf_no"].ToString(),
                        ecf_suppliercode = row["supplierheader_suppliercode"].ToString(),
                        ecf_suppliername = row["supplierheader_name"].ToString(),
                        invoice_retention_amount = Convert.ToDecimal(row["invoice_retention_amount"].ToString()),
                        invoice_retention_exception = Convert.ToDecimal(row["invoice_retention_exception"].ToString()),
                        invoice_no = row["invoice_no"].ToString(),
                        invoice_amount = Convert.ToDouble(row["invoice_amount"].ToString()),
                        Retention_ReleaseAmount = row["invoice_retention_releaseon"].ToString(),
                    };
                    GridReceipt.Add(objModel);
                }

                return GridReceipt;
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
        public string CancelUpdate(RetentionCancelData ret)
        {
            SqlCommand cmd = new SqlCommand("pr_iem_trn_RetentionRelease", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@invoice_gid", SqlDbType.Int).Value = ret.invoice_gid;
            cmd.Parameters.Add("@cancel_by", SqlDbType.Int).Value = cmnfun.GetLoginUserGid().ToString();
            cmd.Parameters.Add("@cancel_date", SqlDbType.VarChar).Value = ret.CancelDate;
            cmd.Parameters.Add("@retention_rate", SqlDbType.VarChar).Value = ret.Retention_rate;
            cmd.Parameters.Add("@retention_amount", SqlDbType.VarChar).Value = ret.Retention_Amount;
            cmd.Parameters.Add("@retentionrelease_amount", SqlDbType.VarChar).Value = ret.release_amount;
            cmd.Parameters.Add("@retention_exception", SqlDbType.VarChar).Value = ret.BalanceAmount;
            cmd.Parameters.Add("@cancel_amount", SqlDbType.VarChar).Value = ret.CancelAmount;
            cmd.Parameters.AddWithValue("@ACTION", SqlDbType.VarChar).Value = "UPDATERETENTION";
            result = Convert.ToString(cmd.ExecuteNonQuery());
            result = "Canceled Successfully";
            return result;
        }
    }
}