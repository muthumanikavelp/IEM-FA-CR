using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using IEM.Common;
using System.Configuration;

namespace IEM.Models
{
    public class CygnetDataModel : CygnetSearch_IRepository
    {
        ErrorLog objErrorLog = new ErrorLog();
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();

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
                cmd.Parameters.Add("@InvoiceFromDate", SqlDbType.VarChar).Value = data.Cygnet_InvoiceFromDate;
                cmd.Parameters.Add("@InvoiceToDate", SqlDbType.VarChar).Value = data.Cygnet_InvoiceToDate; 
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
                    //objModel.Cygnet_HSNCode = row["Cygnet_HSN_Code"].ToString(); 
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
        public DataTable InsertEmployeeeBasicinvoiceupdate(string Cygnet_Gids, String LoginUser_Gid)
        { 
            DataTable dt = new DataTable();
            try
            { 
                GetConnection();
                cmd = new SqlCommand("SP_EOW_Set_ECFInvoiceFromCygnet", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Cygnet_Gids", SqlDbType.Int).Value = Cygnet_Gids;
                cmd.Parameters.Add("@LoginUser_Gid", SqlDbType.Int).Value = LoginUser_Gid; 
                cmd.Parameters.Add("@Action", SqlDbType.VarChar).Value = "Insert";
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
    }
}