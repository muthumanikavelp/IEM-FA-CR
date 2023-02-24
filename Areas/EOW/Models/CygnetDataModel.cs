using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using IEM.Common;
using System.Configuration;

namespace IEM.Areas.EOW.Models
{
    public class CygnetDataModel : CygnetSearch_IRepository
    {
        ErrorLog objErrorLog = new ErrorLog();
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        Common.CmnFunctions cmnfun = new Common.CmnFunctions();
        public IEnumerable<CygnetSearchModel> SelectInvoiceSearch(CygnetSearchModel data)
        {
            List<CygnetSearchModel> Inv = new List<CygnetSearchModel>();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();

                CygnetSearchModel objModel;
                cmd = new SqlCommand("SP_EOW_Get_CygnetBySearch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PanNo", SqlDbType.VarChar).Value = data.Cygnet_SupplierPanNo;
                cmd.Parameters.Add("@SupplierName", SqlDbType.VarChar).Value = data.Cygnet_SupplierName;
                cmd.Parameters.Add("@GSTN", SqlDbType.VarChar).Value = data.Cygnet_Supplier_GSTNNo; 
                cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar).Value = data.Cygnet_InvoiceNo;
                if (data.Cygnet_InvoiceFromDate != "" && data.Cygnet_InvoiceFromDate!=null)
                {
                    cmd.Parameters.Add("@InvoiceFromDate", SqlDbType.VarChar).Value = cmnfun.convertoDateTimeString(data.Cygnet_InvoiceFromDate).ToString(); 
                }
                else
                {
                    cmd.Parameters.Add("@InvoiceFromDate", SqlDbType.VarChar).Value = data.Cygnet_InvoiceFromDate;
                }

                if (data.Cygnet_InvoiceToDate != "" && data.Cygnet_InvoiceToDate != null)
                {
                    cmd.Parameters.Add("@InvoiceToDate", SqlDbType.VarChar).Value = cmnfun.convertoDateTimeString(data.Cygnet_InvoiceToDate).ToString();
                }
                else
                {
                    cmd.Parameters.Add("@InvoiceToDate", SqlDbType.VarChar).Value = data.Cygnet_InvoiceToDate; 
                }
                
                cmd.Parameters.Add("@Action", SqlDbType.VarChar).Value = "SearchInvoice";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CygnetSearchModel();
                    objModel.Cygnet_InvoiceNo = row["Cygnet_Invoice_No"].ToString();
                    objModel.Cygnet_InvoiceDate = row["Cygnet_Invoice_Date"].ToString();
                    objModel.Cygnet_TaxableAmt = row["Cygnet_Taxable_Amount"].ToString();
                    objModel.Cygnet_CGSTAmt = row["Cygnet_CGST_Amount"].ToString();
                    objModel.Cygnet_SGSTAmt = row["Cygnet_SGST_Amount"].ToString();
                    objModel.Cygnet_IGSTAmt = row["Cygnet_IGST_Amount"].ToString();
                    objModel.Cygnet_InvoiceAmt = row["Cygnet_Invoice_Amount"].ToString();
                    objModel.Cygnet_Gid = Convert.ToInt64(row["Cygnet_gid"]);
                    objModel.Cygnet_InvoiceType = row["Cygnet_InvoiceType"].ToString();
                    objModel.InvoiceSupplier_gid = row["InvoiceSupplier_gid"].ToString();
                    objModel.Cygnet_Supplier = row["Cygnet_Supplier"].ToString();
                    objModel.Cygnet_Provider = row["Cygnet_Provider_Location_Name"].ToString();
                    objModel.Cygnet_Receiver = row["Cygnet_Receiver_Location_Name"].ToString();
                    objModel.Cygnet_Provider_GSTN = row["Cygnet_Supplier_GSTN_No"].ToString();
                    objModel.Cygnet_Receiver_GSTN = row["Cygnet_Receiver_GSTN_No"].ToString();
                    //objModel.PO_Nos = row["PO_Nos"].ToString();
                    Inv.Add(objModel);
                }
                return Inv;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return Inv;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        private void GetConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }
        public DataSet InsertECFInvoiceFromCygnet(string Cygnet_Gids, String LoginUser_Gid, string ecf_gid)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string Cygnetgids = Cygnet_Gids.Substring(0, Cygnet_Gids.Length - 1);
            try
            { 
                GetConnection();
                cmd = new SqlCommand("SP_EOW_Set_ECFInvoiceFromCygnet", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Cygnet_Gids", SqlDbType.VarChar).Value = Cygnetgids;
                cmd.Parameters.Add("@LoginUser_Gid", SqlDbType.VarChar).Value = LoginUser_Gid;
                cmd.Parameters.Add("@ecf_gid", SqlDbType.VarChar).Value = ecf_gid;
                cmd.Parameters.Add("@Action", SqlDbType.VarChar).Value = "Insert";
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
        public DataSet SetInvoiceMismatch(string Cygnet_Gids, String LoginUser_Gid)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            string Cygnetgids = Cygnet_Gids.Substring(0, Cygnet_Gids.Length - 1);
            try
            {
                GetConnection();
                cmd = new SqlCommand("SP_EOW_SET_InvoiceMismatch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Cygnet_Gids", SqlDbType.VarChar).Value = Cygnetgids;
                cmd.Parameters.Add("@LoginUser_Gid", SqlDbType.VarChar).Value = LoginUser_Gid; 
                cmd.Parameters.Add("@Action", SqlDbType.VarChar).Value = "Update";
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
    }
}