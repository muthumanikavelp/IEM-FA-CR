using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using IEM.Common;
using System.Configuration;
using System.Web.Providers.Entities;

namespace IEM.Areas.IFAMS.Models
{
    public class sectormodel : sectorrep
    {
        SqlConnection con = new SqlConnection();
        // ReportDataModel_M objrdm = new ReportDataModel_M();
        DataModel objidm = new DataModel();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions objcmnfunc = new CmnFunctions();
        CommonIUD objcmnIUD = new CommonIUD();
        ErrorLog objerrlog = new ErrorLog();
        string duplicateasset = "";
        private void GetConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }

        public IEnumerable<sectorreport> Getsectorreport()
        {


            List<sectorreport> sectorreports = new List<sectorreport>();
            sectorreport obj = new sectorreport();
            try
            {
                GetConnection();
                cmd = new SqlCommand("FS_Get_Sectorwisereport", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@action", "select");
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                HttpContext.Current.Session["sectorreport"] = dt;
                foreach (DataRow row in dt.Rows)
                {
                    obj = new sectorreport();
                    obj.branch_code = row["branch_code"].ToString();
                    obj.employee_code = row["employee_code"].ToString();
                    obj.employee_name = row["employee_name"].ToString();
                    obj.payrunvoucheramount = row["payrunvoucher_amount"].ToString();
                    obj.payrunvoucherpvno = row["payrunvoucher_pvno"].ToString();
                    obj.supplierheader_name = row["supplierheader_name"].ToString();
                    obj.chqno = row["chq_no"].ToString();


                    sectorreports.Add(obj);
                }
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return sectorreports;
        }

        public IEnumerable<sectorreport> Getsectorwisereport()
        {


            List<sectorreport> sectorreports = new List<sectorreport>();
            sectorreport obj = new sectorreport();
            try
            {
                GetConnection();
                cmd = new SqlCommand("FS_GET_EMPSEC", con);
                cmd.CommandType = CommandType.StoredProcedure;

                //cmd.Parameters.AddWithValue("@action", "select");
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                HttpContext.Current.Session["sectorwisereport"] = dt;
                foreach (DataRow row in dt.Rows)
                {
                    obj = new sectorreport();
                    obj.branch_code = row["branch_code"].ToString();
                    obj.employee_code = row["employee_code"].ToString();
                    obj.employee_name = row["employee_name"].ToString();
                    obj.payrunvoucheramount = row["payrunvoucher_amount"].ToString();
                    obj.payrunvoucherpvno = row["payrunvoucher_pvno"].ToString();
                    obj.supplierheader_name = row["supplierheader_name"].ToString();
                    obj.chqno = row["chq_no"].ToString();


                    sectorreports.Add(obj);
                }
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return sectorreports;
        }

        public IEnumerable<sectorreport> Getsectorexcelreport()
        {


            List<sectorreport> sectorreports = new List<sectorreport>();
            sectorreport obj = new sectorreport();
            try
            {
                GetConnection();
                cmd = new SqlCommand("FS_Get_Sectorwisereport", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@action", "excel");
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    obj = new sectorreport();
                    obj.ecfno = row["ecf_no"].ToString();
                    obj.employee_code = row["employee_code"].ToString();
                    obj.employee_name = row["employee_name"].ToString();
                    obj.supplierheader_name = row["supplierheader_name"].ToString();
                    obj.grossamount = row["invoice_amount"].ToString();
                    obj.tdsamount = row["creditline_amount"].ToString();
                    obj.netamount = row["NetAmount"].ToString();
                    obj.chqno = row["chq_no"].ToString();
                    obj.payrunvoucherpvno = row["payrunvoucher_pvno"].ToString();
                    obj.location = row["branch_name"].ToString();
                    obj.awbno = row["paymentdespatch_awb_no"].ToString();
                    obj.sendto = row["paymentdespatch_addr"].ToString();
                    sectorreports.Add(obj);
                }
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return sectorreports;
        }

       
        public IEnumerable<tdsreport> Gettdsreport(string fromdate,string todate)
        {


            List<tdsreport> tdsreports = new List<tdsreport>();
            tdsreport obj = new tdsreport();
            try
            {
                GetConnection();
                cmd = new SqlCommand("FS_Get_tdsreport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fromdate", fromdate);
                cmd.Parameters.AddWithValue("@todate", todate);
                cmd.Parameters.AddWithValue("@action", "tdsreport");
                DataTable dt = new DataTable();
                
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                HttpContext.Current.Session["tdsexcel"] = dt;
                foreach (DataRow row in dt.Rows)
                {
                    obj = new tdsreport();
                    obj.ecfno = row["ECF No"].ToString();
                    obj.employeecode = row["raiser"].ToString();
                    obj.employeename = row["Emp Name"].ToString();
                    obj.suppliername = row["Supplier Name"].ToString();
                    obj.invoiceamount = row["Invoice Amount"].ToString();
                    obj.taxableamount = row["Tax Amount"].ToString();
                    obj.glno = row["Creditline Glno"].ToString();
                    obj.ecfclaimmonth = row["Ecf Claim Month"].ToString();
                    obj.mode = row["MODE"].ToString();
                    obj.finyear = row["FINYEAR"].ToString();

                    tdsreports.Add(obj);

                }
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return tdsreports;
        }


        public IEnumerable<wctreport> getwctrep(string fromdate, string todate)
        {


            List<wctreport> wctreports = new List<wctreport>();
            wctreport obj = new wctreport();
            try
            {
                GetConnection();
                cmd = new SqlCommand("FS_Get_tdsreport", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@action", "wctreport");
                cmd.Parameters.AddWithValue("@fromdate", fromdate);
                cmd.Parameters.AddWithValue("@todate", todate);
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                HttpContext.Current.Session["wctexcel"] = dt;
                foreach (DataRow row in dt.Rows)
                {
                    obj = new wctreport();
                    obj.ecfno = row["Ecf No"].ToString();
                    obj.employeecode = row["raiser"].ToString();
                    obj.employeename = row["Employee Name"].ToString();
                    obj.suppliername = row["Supplier Name"].ToString();
                    obj.invoiceamount = row["Invoice Amount"].ToString();
                    obj.taxableamount = row["Tax Amount"].ToString();
                    obj.glno = row["Creditline Glno"].ToString();
                    obj.ecfclaimmonth = row["Ecf Claimmonth"].ToString();
                    obj.modes = row["MODE"].ToString();
                    obj.finyear = row["FINYEAR"].ToString();

                    wctreports.Add(obj);

                }
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return wctreports;
        }


        public IEnumerable<servicetareport> getservicerep(string fromdate, string todate)
        {
            List<servicetareport> servicereports = new List<servicetareport>();
            servicetareport obj = new servicetareport();
            try
            {
                GetConnection();
                cmd = new SqlCommand("FS_Get_tdsreport", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@action", "servicetaxreport");
                cmd.Parameters.AddWithValue("@fromdate", fromdate);
                cmd.Parameters.AddWithValue("@todate", todate);
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                HttpContext.Current.Session["serviceexcel"] = dt;
                foreach (DataRow row in dt.Rows)
                {
                    obj = new servicetareport();
                    obj.ecfno = row["ECF NO"].ToString();
                    obj.panno = row["PAN NO"].ToString();
                    obj.vendorcode = row["Vendor Code"].ToString();
                    obj.vendorname = row["Vendor Name"].ToString();
                    obj.empcode = row["EMP CODE"].ToString();
                    obj.raiser = row["RAISER"].ToString();
                    obj.invoiceno = row["Invoice_No"].ToString();
                    obj.invoicedate = row["Invoice Date"].ToString();
                    obj.billamount = row["Bill Amount"].ToString();
                    obj.cbsamount = row["CBS Amount"].ToString();
                    obj.bscc = row["BSCC"].ToString();
                    obj.branchname = row["Branch"].ToString();
                    obj.expcode = row["Exp Code"].ToString();
                    obj.subcode = row["Sub Code"].ToString();
                    obj.narration = row["Narration"].ToString();
                    obj.expmonth = row["Exp Month"].ToString();
                    obj.billstartdate = row["Bill Start Date"].ToString();
                    obj.billenddate = row["Bill End Date"].ToString();
                    obj.productcode = row["Product Code"].ToString();
                    obj.staxamount = row["Stax Amount"].ToString();
                    obj.servicetax = row["Service Tax"].ToString();
                    obj.taxableamount = row["Service Taxable Amt"].ToString();

                    servicereports.Add(obj);
                    
                }
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return servicereports;
            }
            finally
            {
                con.Close();
            }
            return servicereports;
        }

        public IEnumerable<valreport> getvalrep(string fromdate, string todate)
        {
            List<valreport> valreports = new List<valreport>();
            valreport obj = new valreport();
            try
            {
                GetConnection();
                cmd = new SqlCommand("FS_Get_tdsreport", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@action", "VALreport");
                cmd.Parameters.AddWithValue("@fromdate", fromdate);
                cmd.Parameters.AddWithValue("@todate", todate);
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                HttpContext.Current.Session["valexcel"] = dt;
                foreach (DataRow row in dt.Rows)
                {
                    obj = new valreport();
                    obj.ecfno = row["ECF NO"].ToString();
                    obj.ecfstatus = row["ECF STATUS"].ToString();
                    obj.empcode = row["EMP CODE"].ToString();
                    obj.empname = row["EMPLOYEE NAME"].ToString();
                    obj.vendorcode = row["VENODR CODE"].ToString();
                    obj.vendorname = row["VENDOR NAME"].ToString();
                    obj.ecfprosamt = row["ECF PROCESSAMOUNT"].ToString();
                    obj.ecfamount = row["ECF AMOUNT"].ToString();
                    obj.paymenttranamt = row["PAYMENTTRAN AMOUNT"].ToString();
                    obj.ecfnet = row["ECF NET"].ToString();
                    obj.prundate = row["PAURUNVOUCHER DATE"].ToString();
                    obj.TDS  = row["TDS"].ToString();
                    obj.WCT = row["WCT"].ToString();
                    obj.ptranpaymode = row["PAYMODE"].ToString();
                    obj.chqno = row["CHQ NO"].ToString();
                    obj.pvno = row["PVNO"].ToString();
                    obj.pvouchernet = row["PAYRUNVOUCHER NET"].ToString();
                    obj.vclaim = row["VOCHER CLAIM TYPE"].ToString();
                    obj.chqdate = row["CHQ DATE"].ToString();
                    obj.vmemo = row["VOUCHER MEMO"].ToString();
                    obj.paybankgl = row["PAYBANK GL"].ToString();
                    obj.remarks = row["REMARKS"].ToString();
                    obj.oraclebatch = row["OracleBatchNumber"].ToString();

                    valreports.Add(obj);

                }
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return valreports;
            }
            finally
            {
                con.Close();
            }
            return valreports;
        }

    }
}

