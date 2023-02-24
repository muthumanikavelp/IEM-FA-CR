using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.IO;
using System.Globalization;
using System.Text.RegularExpressions;
using IEM.Models;
using Microsoft.Win32;
using System.Text;
using System.Runtime.InteropServices;

namespace IEM.Common
{

    public class CmnFunctions
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da;
        DataTable dt;
        ErrorLog objErrorLog = new ErrorLog();
        public void getconnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }
        public string Sorter(string columnName, string columnHeader, WebGrid grid)
        {
            return string.Format("{0} {1}", columnHeader, grid.SortColumn == columnName ?
                grid.SortDirection == SortDirection.Ascending ? "▲" :
                "▼" : string.Empty);
        }
        public int GetLoginUserGid()
        {
            try
            {
                int UserGid = 0;
                UserGid = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                return UserGid;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                Emp.SessionStatus = 0;
                IEMDataModel objModel = new IEMDataModel();
                objModel.insertloginattempt(Emp.IPAddress, Emp.empcodeST, "Session Expired", Emp.BrowserName);
                objModel.UpdateLoginFlag(Emp.empgidST, 0);
                HttpContext.Current.Response.RedirectToRoute("Default");
                throw ex;
            }
 
            // return 3;
        }
        public DataTable GetLoginUserDetails(int usergid)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            getconnection();
            string lsQry = "select employee_code,employee_name from iem_mst_temployee where employee_gid=" + usergid + " and employee_isremoved='N' ";
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = lsQry;
            da.SelectCommand = cmd;
            da.Fill(dt);
            return dt;
        }
        public string GetEmployeeName(int usergid)
        {
            string lsParameter = "";
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter();
            getconnection();
            string lsQry = "select employee_name from iem_mst_temployee where employee_gid=" + usergid + " and employee_isremoved='N' ";
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = lsQry;
            lsParameter = Convert.ToString(cmd.ExecuteScalar());
            return lsParameter;
        }
        public int GetLoginProxyUserGid()
        {
            int ProxyUserGid;
            ProxyUserGid = Convert.ToInt32(HttpContext.Current.Session["Proxyemployee_gid"]);
            return ProxyUserGid;
            //return 2;
        }
        public DateTime convertoDateTime(string inputdate)
        {
            try
            {
                string lsInputDate = inputdate;
                string convertdate = string.Empty;
                DateTime outputDate = Convert.ToDateTime(lsInputDate.ToString());
                string format = "dd/MMM/yyyy";
                convertdate = outputDate.ToString(format);
                outputDate = Convert.ToDateTime(convertdate);
                return outputDate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string convertoDateTimeString(string inputdate)
        {
            try
            {
                string lsDate="";
                if (inputdate.Trim() != "") //Pandiaraj 03-10-18
                {
                    inputdate = inputdate.Split(' ')[0].ToString();
                    lsDate = inputdate;
                    /*
                    if (inputdate.Length == 10)
                    {
                        if (inputdate.Split('-').Length > 1)
                        {
                            if (inputdate.Split('-')[2].Length == 4)
                            { 
                                lsDate = inputdate.Split('-')[2].ToString() + '-' + inputdate.Split('-')[1].ToString() + '-' + inputdate.Split('-')[0].ToString();
                            }
                        }
                    }
                    */
                    DateTime outputDate = Convert.ToDateTime(lsDate.ToString());
                    lsDate = outputDate.ToString("dd/MMM/yyyy");
                }//Pandiaraj 03-10-18
                return lsDate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string convertoDateTimeStringasms(string inputdate)
        {
            try
            {
                string lsDate="";
                inputdate = inputdate.Split(' ')[0].ToString();
                lsDate = inputdate;
                /*
                if (inputdate.Length == 10)
                {
                    if (inputdate.Split('-').Length > 1)
                    {
                        if (inputdate.Split('-')[2].Length == 4)
                        { 
                            lsDate = inputdate.Split('-')[2].ToString() + '-' + inputdate.Split('-')[1].ToString() + '-' + inputdate.Split('-')[0].ToString();
                        }
                    }
                }
                */
                DateTime outputDate = Convert.ToDateTime(lsDate.ToString());
                lsDate = outputDate.ToString("dd/MMM/yyyy");
                return lsDate;
            }
            catch (Exception ex)
            {
                return lsDate;
            }
        }

        public string convertotime(string inputdate)
        {
            try
            {
                string lsDate;
                DateTime outputDate = Convert.ToDateTime(inputdate);

                lsDate = outputDate.ToShortTimeString();
                return lsDate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string ddMMyyyyString(string inputdate)
        {
            try
            {
                string lsInputDate = inputdate;
                string convertdate = string.Empty;
                DateTime outputDate = Convert.ToDateTime(inputdate);
                string format = "dd/MM/yyyy";
                convertdate = outputDate.ToString(format);
                return convertdate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string yyMMddString(string inputdate)
        {
            try
            {
                string lsInputDate = inputdate;
                string convertdate = string.Empty;
                DateTime outputDate = Convert.ToDateTime(inputdate);
                string format = "yyMMdd";
                convertdate = outputDate.ToString(format);
                return convertdate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Decimal convertoDecimal(string inputstring)
        {
            try
            {
                decimal lsOutput = Math.Round(Convert.ToDecimal(inputstring), 2);
                return lsOutput;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetSequenceNo(string SequenceCode, string SupName = null)
        {
            try
            {
                string result = "";
                int lsSequence = 0;
                string lsQry = "";
                string lsCurrentDate = "";
                DataTable dt = new DataTable();

                getconnection();

                lsQry = "select CAST(GETDATE() as DATE) as 'getdate'";

                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = lsQry;

                lsCurrentDate = Convert.ToString(cmd.ExecuteScalar());
                lsCurrentDate = convertoDateTimeString(lsCurrentDate);

                lsQry = "";
                lsQry += "Select sequence_no,CAST(sequence_date as DATE) as 'sequence_date',sequence_code ";
                lsQry += "from iem_mst_tsequence where sequence_code = '" + SequenceCode + "'";

                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = lsQry;

                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    if (SequenceCode == "SUP")
                    {
                        lsSequence = Convert.ToInt32(dt.Rows[0]["sequence_no"].ToString()) + 1;
                        lsQry = "update iem_mst_tsequence set sequence_no='" + lsSequence + "' ";
                        lsQry += " where sequence_code='" + SequenceCode + "'";
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = lsQry;
                        cmd.ExecuteNonQuery();
                        if (SupName != null)
                        {
                            result = "SUP" + lsSequence.ToString("000000");
                        }
                    }
                    else
                    {
                        if (convertoDateTimeString(dt.Rows[0]["sequence_date"].ToString()) != lsCurrentDate)
                        {
                            lsSequence = 1;
                            lsQry = "update iem_mst_tsequence set sequence_no='" + lsSequence + "', sequence_date='" + lsCurrentDate + "' ";
                            lsQry += " where sequence_code='" + SequenceCode + "'";
                        }
                        else
                        {
                            lsSequence = Convert.ToInt32(dt.Rows[0]["sequence_no"].ToString()) + 1;
                            lsQry = "update iem_mst_tsequence set sequence_no='" + lsSequence + "' ";
                            lsQry += " where sequence_code='" + SequenceCode + "'";
                        }
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = lsQry;
                        cmd.ExecuteNonQuery();
                        if (SupName != null)
                        {
                            if (SupName.Trim().Length > 2)
                            {
                                result = SupName.Trim().ToUpper().Substring(0, 2) + yyMMddString(lsCurrentDate) + lsSequence.ToString("0000");
                            }
                            else
                            {
                                result = SequenceCode + yyMMddString(lsCurrentDate) + lsSequence.ToString("0000");
                            }
                        }
                        else
                        {
                            result = SequenceCode + yyMMddString(lsCurrentDate) + lsSequence.ToString("0000");
                        }
                    }


                }

                return result;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }


        public void GetAllFiles(int gid, string filepath)
        {
            try
            {

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetRemovecommas(string inpustring)
        {
            try
            {
                string Isretrunstr;
                Isretrunstr = inpustring.Replace(",", "");
                return Isretrunstr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string Getreplacesinglequotes(string inpustring)
        {
            try
            {
                string Isretrunstr;
                Isretrunstr = inpustring.Replace("'", "''");
                return Isretrunstr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string Getspecialcharacters(string inpustring)
        {
            try
            {
                string Isretrunstr;
                Isretrunstr = Regex.Replace(inpustring, "[^a-zA-Z0-9]+", "");
                return Isretrunstr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetINRAmount(string Amount)
        {
            try
            {
                //decimal amoumnt = 0;
                //amoumnt = Convert.ToDecimal(Amount);
                //CultureInfo cuInfo = new CultureInfo("hi-IN");
                //string Isretrunstr;
                //Isretrunstr = (amoumnt.ToString("C", cuInfo)).Remove(0, 2).Trim();
                //return Isretrunstr;
                decimal amoumnt = 0;
                amoumnt = Convert.ToDecimal(Amount.Trim());
                CultureInfo cuInfo = new CultureInfo("hi-IN");
                string Isretrunstr;
                //Isretrunstr = (amoumnt.ToString("C", cuInfo)).Remove(0, 1).Trim();
                //12-04-18
                Isretrunstr = (amoumnt.ToString("C", cuInfo)).Trim();
                var charstoremove = new string[] { "₹", " ", "रु", "$" };
                foreach (var c in charstoremove)
                {
                    Isretrunstr = Isretrunstr.Replace(c, string.Empty).Trim();
                }
                //12-04-18
                return Isretrunstr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetMailtables(string Mailtable, string module, string request)
        {
            try
            {
                string Status = string.Empty;
                if (module == "ASMS")
                {
                    if (Mailtable.Contains("ONBOARDING"))
                    {
                        Status = "iem_asms_mailview,queue_gid";
                    }
                    else if (Mailtable.Contains("ACTIVATION"))
                    {
                        Status = "iem_asms_mailview,queue_gid";
                    }
                    else if (Mailtable.Contains("DEACTIVATION"))
                    {
                        Status = "iem_asms_mailview,queue_gid";
                    }
                    else if (Mailtable.Contains("MODIFICATION"))
                    {
                        Status = "iem_asms_mailview,queue_gid";
                    }
                    else if (Mailtable.Contains("RENEWAL"))
                    {
                        Status = "iem_asms_mailview,queue_gid";
                    }
                }
                if (module == "EOW")
                {
                    if (Mailtable.Equals("REGULAR"))
                    {
                        Status = "iem_eow_employee_mailview,queue_gid";
                    }
                    else if (Mailtable.Equals("LOCALCOVEYANCE"))
                    {
                        Status = "iem_eow_employee_mailview,queue_gid";
                    }
                    else if (Mailtable.Equals("TRAVEL"))
                    {
                        Status = "iem_eow_employee_mailview,queue_gid";
                    }
                    else if (Mailtable.Equals("SUPPLIERINVOICE"))
                    {
                        Status = "iem_eow_supplier_mailview,queue_gid";
                    }
                    else if (Mailtable.Equals("SUPPLIERINVOICEDSA"))
                    {
                        Status = "iem_eow_supplier_mailview,queue_gid";
                    }
                    else if (Mailtable.Equals("ADVANCEREQUISITIONEMPLOYEE"))
                    {
                        Status = "iem_eow_employee_mailview,queue_gid";
                    }
                    else if (Mailtable.Equals("ADVANCEREQUISITIONSUPPLIER"))
                    {
                        Status = "iem_eow_supplier_mailview,queue_gid";
                    }
                    else if (Mailtable.Equals("PETTYCASH"))
                    {
                        Status = "iem_eow_employee_mailview,queue_gid";
                    }

                    else if (Mailtable.Equals("INSURANCE")) //rAMYA -- 14-05-2019
                    {
                        Status = "iem_eow_supplier_mailview,queue_gid";
                    }
                    else if (Mailtable.Equals("INSURANCEADVANCE"))
                    {
                        Status = "iem_eow_supplier_mailview,queue_gid";
                    }
                    //else if (Mailtable.Equals("ADVANCEREQUISITIONSUPPLIER"))
                    //{
                    //    Status = "iem_eow_supplier_mailview,queue_gid";
                    //}
                }
                if (module == "FB")
                {
                    if (Mailtable.Contains("PAR"))
                    {
                        Status = "iem_fb_Par_mailview,queue_gid";
                    }
                    else if (Mailtable.Contains("PR"))
                    {
                        Status = "iem_fb_Pr_mailview,queue_gid";
                    }
                    else if (Mailtable.Contains("CBF"))
                    {
                        Status = "iem_fb_cbf_mailview,queue_gid";
                    }
                    else if (Mailtable.Contains("PO"))
                    {
                        Status = "iem_fb_Po_mailview,queue_gid";
                    }
                    else if (Mailtable.Contains("WO"))
                    {
                        Status = "iem_fb_wo_mailview,queue_gid";
                    }
                }
                if (module == "FS")
                {
                    if (Mailtable.Contains("PAYMENT ADVICE EMAIL ALERT"))
                    {
                        Status = "VW_FS_PaymentAdviceMail,PvId";
                    }
                    else if (Mailtable.Contains("PETTY CASH EMAIL ALERT"))
                    {
                        Status = "VW_FS_PettyCashMail,EcfId";
                    }
                    else if (Mailtable.Contains("EFT REJECTION EMAIL ALERT"))
                    {
                        Status = "VW_FS_EFTRejectionMail,PvId";
                    }
                    else if (Mailtable.Contains("INEX SUBMISSION"))
                    {
                        Status = "VW_FS_InexedDocument,ECFId";
                    }
                    else if (Mailtable.Contains("PPX O/S - GENTLE REMINDER"))
                    {
                        Status = "VW_FS_ARFMailAlert,ECFId";
                    }
                    else if (Mailtable.Contains("PPX O/S - REMINDER 1"))
                    {
                        Status = "VW_FS_ARFMailAlert,ECFId";
                    }
                    else if (Mailtable.Contains("PPX O/S - REMINDER 2"))
                    {
                        Status = "VW_FS_ARFMailAlert,ECFId";
                    }
                    else if (Mailtable.Contains("PHYSICAL RECEIPTING"))
                    {
                        Status = "VW_FS_PhysicalReceipting,ecf_gid";
                    }
                    else if (Mailtable.Contains("PREPAID ADJUSTMENT - VENDOR CLAIM"))
                    {
                        Status = "VW_FS_PrepaidLiquidation,ECFId";
                    }
                    else if (Mailtable.Contains("PREPAID ADJUSTMENT - EMPLOYEE CLAIM"))
                    {
                        Status = "VW_FS_PrepaidLiquidation,ECFId";
                    }
                    else if (Mailtable.Contains("PAYMENT ALERT VENDOR CLAIM"))
                    {
                        Status = "VW_FS_EFTERAStatusUpdate,PvId";
                    }
                    else if (Mailtable.Contains("PAYMENT ALERT EMPLOYEE CLAIM"))
                    {
                        Status = "VW_FS_EFTERAStatusUpdate,PvId";
                    }
                    else if (Mailtable.Contains("CHEQUE DISPATCH"))
                    {
                        Status = "VW_FS_CHQDespatch,PvId";
                    }
                }
                return Status;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetTitleValues(string Titleid)
        {
            try
            {
                string Status = string.Empty;

                if (Titleid == "1")
                {
                    Status = "E";
                }
                else if (Titleid == "2")
                {
                    Status = "G";
                }
                else if (Titleid == "3")
                {
                    Status = "D";
                }
                else if (Titleid == "4")
                {
                    Status = "R";
                }
                return Status;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetTsearchValues(string Titleid)
        {
            try
            {
                string Status = string.Empty;

                if (Titleid == "1")
                {
                    Status = "D";
                }
                else if (Titleid == "2")
                {
                    Status = "A";
                }
                else if (Titleid == "3")
                {
                    Status = "R";
                }
                else if (Titleid == "4")
                {
                    Status = "C";
                }
                return Status;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetQueueStatusHistry(string Queue)
        {
            try
            {
                string Status = string.Empty;

                if (Queue == "0")
                {
                    Status = "Raiser";
                    return Status;
                }
                else if (Queue == "64")
                {
                    Status = "Hold Release";
                    return Status;
                }
                else if (Queue == "16")
                {
                    Status = "Hold";
                    return Status;
                }
                else if (Queue == "1")
                {
                    Status = "Approved";
                    return Status;
                }
                else if (Queue == "32")
                {
                    Status = "Inprocess";
                    return Status;
                }
                else if (Queue == "2")
                {
                    Status = "Rejected";
                    return Status;
                }
                else if (Queue == "10" || Queue == "20")
                {
                    Status = "Rejected";
                    return Status;
                }
                else if (Queue == "4")
                {
                    Status = "Cancelled";
                    return Status;
                }
                else if (Queue == "7")
                {
                    Status = "Concurrent Approved Rejected";
                    return Status;
                }
                else if (Queue == "8")
                {
                    //Status = "Concurrent Approval soughted";
                    Status = "Sent for Concurrent approval";
                    return Status;
                }
                else if (Queue == "9")
                {
                    Status = "Concurrent Approved";
                    return Status;
                }
                else if (Queue == "11")
                {
                    Status = "Concurrent Not Applicable";
                    return Status;
                }
                return Status;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetQueueStatusapp(string Queue)
        {
            try
            {
                string Status = string.Empty;
                if (Queue == "0")
                {
                    Status = "Draft";
                }
                else if (Queue == "32")
                {
                    Status = "Inprocess";
                }
                else if (Queue == "2")
                {
                    Status = "Rejected";
                }
                else if (Queue == "1")
                {
                    Status = "Approved";
                }
                else if (Queue == "65536")
                {
                    Status = "Paid";
                }
                else if (Queue == "2048")
                {
                    Status = "EPU Rejected";
                }
                else
                {
                    Status = "EPU Inprocess";
                }
                return Status;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetQueueStatusappser(string Queue)
        {
            try
            {
                string Status = string.Empty;
                if (Queue == "1")
                {
                    Status = "0";
                }
                else if (Queue == "5")
                {
                    Status = "32";
                }
                else if (Queue == "3")
                {
                    Status = "2";
                }
                else if (Queue == "7")
                {
                    Status = "1";
                }
                else if (Queue == "6")
                {
                    Status = "65536";
                }
                return Status;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetQueueStatus(string Queue)
        {
            try
            {
                string Status = string.Empty;
                if (Queue.Contains("D"))
                {
                    Status = "Draft";
                }
                else if (Queue.Contains("A"))
                {
                    Status = "Pending Approval";
                }
                else if (Queue.Contains("R"))
                {
                    Status = "Rejected";
                }
                else if (Queue.Contains("C"))
                {
                    Status = "Concurrent Approval";
                }
                return Status;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetDocType(string Doctype)
        {
            try
            {
                string Status = string.Empty;
                if (Doctype.ToString() == "1")
                {
                    Status = "ECRegular";
                }
                else if (Doctype.ToString() == "2")
                {
                    Status = "ECLocal";
                }
                else if (Doctype.Contains("3"))
                {
                    Status = "ECTravel";
                }
                else if (Doctype.Contains("4"))
                {
                    Status = "EcfSupplier";
                }
                else if (Doctype.Contains("5"))
                {
                    Status = "EcfSupplierDSA";
                }
                else if (Doctype.Contains("8"))
                {
                    Status = "ECRegular";
                }
                else if (Doctype.Contains("6") || Doctype.Contains("7"))
                {
                    Status = "Arf";
                }
                else if (Doctype.ToString() == "11")
                {
                    Status = "Insurance";
                }
                else if (Doctype.ToString() == "12")
                {
                    Status = "Insuranceadvance";
                }
                return Status;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetSubDocType(string Doctype)
        {
            try
            {
                string Status = string.Empty;
                if (Doctype.Equals("1"))
                {
                    Status = "REGULAR";
                }
                else if (Doctype.Equals("2"))
                {
                    Status = "LOCALCOVEYANCE";
                }
                else if (Doctype.Equals("3"))
                {
                    Status = "TRAVEL";
                }
                else if (Doctype.Equals("4"))
                {
                    Status = "SUPPLIERINVOICE";
                }
                else if (Doctype.Equals("5"))
                {
                    Status = "SUPPLIERINVOICEDSA";
                }
                else if (Doctype.Equals("6"))
                {
                    Status = "ADVANCEREQUISITIONEMPLOYEE";
                }
                else if (Doctype.Equals("7"))
                {
                    Status = "ADVANCEREQUISITIONSUPPLIER";
                }
                else if (Doctype.Equals("8"))
                {
                    Status = "PETTYCASH";
                }
                else if (Doctype.ToString() == "11")
                {
                    Status = "INSURANCE";
                }
                else if (Doctype.ToString() == "12")
                {
                    Status = "INSURANCEADVANCE";
                }
                return Status;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetSubDocTypedash(string Doctype)
        {
            try
            {
                string Status = string.Empty;
                if (Doctype.Equals("1"))
                {
                    Status = "General Claims";
                }
                else if (Doctype.Equals("2"))
                {
                    Status = "Bulk Conveyance";
                }
                else if (Doctype.Equals("3"))
                {
                    Status = "Travel Claim";
                }
                else if (Doctype.Equals("4"))
                {
                    Status = "Regular Invoice";
                }
                else if (Doctype.Equals("5"))
                {
                    Status = "DSA Suppliers";
                }
                else if (Doctype.Equals("6"))
                {
                    Status = "Employee Advance";
                }
                else if (Doctype.Equals("7"))
                {
                    Status = "Supplier Advance";
                }
                else if (Doctype.Equals("8"))
                {
                    Status = "Petty Cash";
                }
                else if (Doctype.ToString() == "11")
                {
                    Status = "INSURANCE";
                }
                else if (Doctype.ToString() == "12")
                {
                    Status = "INSURANCEADVANCE";
                }
                return Status;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetContentType(string filename)
        {
            try
            {
                string contentType = string.Empty;
                if (filename.Contains(".pdf"))
                {
                    contentType = "application/pdf";
                }
                else if ((filename.Contains(".doc")) || (filename.Contains(".docx")))
                {
                    contentType = "application/docx";
                }
                else if (filename.Contains(".xls"))
                {
                    contentType = "application/vnd.ms-excel";
                }
                else if ((filename.Contains(".jpg")) || (filename.Contains(".jpeg")) || (filename.Contains(".jpe")))
                {
                    contentType = "image/jpeg";
                }
                else if ((filename.Contains(".tif")) || (filename.Contains(".tiff")))
                {
                    contentType = "image/tiff";
                }
                else if (filename.Contains(".bmp"))
                {
                    contentType = "image/bmp";
                }
                else if (filename.Contains(".gif"))
                {
                    contentType = "image/gif";
                }
                else if (filename.Contains(".txt"))
                {
                    contentType = "text/plain";
                }
                else if (filename.Contains(".txt"))
                {
                    contentType = "text/plain";
                }
                else if ((filename.Contains(".htm")) || (filename.Contains(".html")) || (filename.Contains(".stm")))
                {
                    contentType = "text/html";
                }
                else if ((filename.Contains(".htm")) || (filename.Contains(".html")) || (filename.Contains(".stm")))
                {
                    contentType = "text/html";
                }
                return contentType;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetSequenceNoFb(string SequenceCode, string Parameter, string Parameter1)
        {
            try
            {
                string result = "";
                int lsSequence = 0;
                string lsQry = "";
                string lsQry1 = "";
                string lsCurrentDate = "";
                string lsParameter = "";
                DataTable dt = new DataTable();

                getconnection();
                lsQry = "select CAST(GETDATE() as DATE) as 'getdate'";
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = lsQry;
                lsCurrentDate = Convert.ToString(cmd.ExecuteScalar());
                lsCurrentDate = convertoDateTimeString(lsCurrentDate);

                if (Parameter != "" && Parameter != "0")
                {
                    lsQry1 = "select requestfor_name from iem_mst_trequestfor where (requestfor_gid='" + Parameter + "' )and requestfor_isremoved='N'";

                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = lsQry1;
                    lsParameter = Convert.ToString(cmd.ExecuteScalar());
                }
                else if (Parameter1 != "")
                {
                    lsQry1 = "select requestfor_name from iem_mst_trequestfor where (requestfor_name='" + Parameter1 + "' )and requestfor_isremoved='N'";

                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = lsQry1;
                    lsParameter = Convert.ToString(cmd.ExecuteScalar());
                }



                lsQry = "select  sequenceno_no,CAST(sequenceno_date as DATE) as 'sequence_date',sequenceno_code,sequenceno_parameter1  from fb_mst_tsequenceno";
                lsQry += " where sequenceno_code = '" + SequenceCode + "' and sequenceno_parameter1='" + lsParameter + "' ";
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = lsQry;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    if (convertoDateTimeString(dt.Rows[0]["sequence_date"].ToString()) != lsCurrentDate)
                    {
                        lsSequence = 1;
                        lsQry = "update fb_mst_tsequenceno set sequenceno_no='" + lsSequence + "', sequenceno_date='" + lsCurrentDate + "' ";
                        lsQry += " where sequenceno_code='" + SequenceCode + "' and sequenceno_parameter1='" + lsParameter + "'";
                    }
                    else
                    {
                        lsSequence = Convert.ToInt32(dt.Rows[0]["sequenceno_no"].ToString()) + 1;
                        lsQry = "update fb_mst_tsequenceno set sequenceno_no='" + lsSequence + "' ";
                        lsQry += " where sequenceno_code='" + SequenceCode + "' and sequenceno_parameter1='" + lsParameter + "'";
                    }
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = lsQry;
                    cmd.ExecuteNonQuery();
                    result = SequenceCode + lsParameter + yyMMddString(lsCurrentDate) + lsSequence.ToString("0000");
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GETprno(string Prdetails)
        {
            try
            {
                string lsQry = "";
                getconnection();
                string lsParameter = "";
                lsQry = "select prdetails_prheader_gid from fb_trn_tprdetails where prdetails_gid='" + Prdetails.ToString() + "'";
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = lsQry;
                lsParameter = Convert.ToString(cmd.ExecuteScalar());
                return lsParameter;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetSequenceNoFam(string SequenceCode)
        {
            try
            {
                string result = "";
                int lsSequence = 0;
                string lsQry = "";
                string lsQry1 = "";
                string lsCurrentDate = "";
                DataTable dt = new DataTable();
                CommonIUD objIUD = new CommonIUD();
                getconnection();
                lsQry = "select CAST(GETDATE() as DATE) as 'getdate'";
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = lsQry;
                lsCurrentDate = Convert.ToString(cmd.ExecuteScalar());
                lsCurrentDate = convertoDateTimeString(lsCurrentDate);

                lsQry = "select  sequenceno_no,CAST(sequenceno_date as DATE) as 'sequence_date',sequenceno_code  from fa_mst_tsequenceno";
                lsQry += " where sequenceno_code = '" + SequenceCode + "'";
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = lsQry;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    if (convertoDateTimeString(dt.Rows[0]["sequence_date"].ToString()) != lsCurrentDate)
                    {
                        lsSequence = 1;
                        lsQry1 = "update fa_mst_tsequenceno set sequenceno_no='" + lsSequence + "', sequenceno_date='" + lsCurrentDate + "' ";
                        lsQry1 += " where sequenceno_code='" + SequenceCode + "'";
                    }
                    else
                    {
                        lsSequence = Convert.ToInt32(dt.Rows[0]["sequenceno_no"].ToString()) + 1;
                        lsQry1 = "update fa_mst_tsequenceno set sequenceno_no='" + lsSequence + "' ";
                        lsQry1 += " where sequenceno_code='" + SequenceCode + "'";
                    }
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = lsQry1;
                    cmd.ExecuteNonQuery();
                    result = SequenceCode + yyMMddString(lsCurrentDate) + lsSequence.ToString("0000");
                }
                else
                {

                    lsSequence = 0;
                    string[,] codes = new string[,]
	                   { 
                             {"sequenceno_no",lsSequence.ToString()},
                             {"sequenceno_date",lsCurrentDate.ToString()},
                             {"sequenceno_code",SequenceCode.ToString()}
                       };
                    string tblname = "fa_mst_tsequenceno";
                    string instComm = objIUD.InsertCommon(codes, tblname);

                    result = SequenceCode + yyMMddString(lsCurrentDate) + lsSequence.ToString("0000");
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string generateAssetid(string locCode, string assetCode)
        {
            try
            {
                getconnection();

                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@fccc", SqlDbType.VarChar).Value = locCode;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "locid";
                string locCode1 = "";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                    locCode1 = dt.Rows[0]["branch_gid"].ToString();
                string asstCode1 = "";
                string result = "";
                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@fccc", SqlDbType.VarChar).Value = assetCode;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "assetcode";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                    asstCode1 = dt.Rows[0]["asset_gid"].ToString();
                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@headergid", SqlDbType.VarChar).Value = locCode1;
                cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = asstCode1;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "Exist";
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (locCode.Length <= 4)
                {
                    if (res1 == "Not There")
                    {
                        int serialNo = 1;
                        result = ConfigurationManager.AppSettings["Entity"] + assetCode + "-" + Convert.ToInt32(locCode).ToString("0000") + "0000-" + serialNo.ToString("00000");
                    }
                    else
                    {
                        string lsQry = "select  MAX(convert(int,substring(assetdetails_assetdet_id,17,len(assetdetails_assetdet_id)))) FROM fa_trn_tassetdetails ";
                        lsQry += " where assetdetails_asset_code='" + asstCode1 + "' and ";
                        lsQry += " assetdetails_isremoved='N' and assetdetails_branch_gid ='" + locCode1 + "'";
                        cmd = new SqlCommand(lsQry, con);
                        cmd.CommandType = CommandType.Text;
                        int serialNo = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
                        result = ConfigurationManager.AppSettings["Entity"] + assetCode + "-" + Convert.ToInt32(locCode).ToString("0000") + "0000-" + serialNo.ToString("00000");
                    }
                }
                else
                {
                    if (res1 == "Not There")
                    {
                        int serialNo = 1;
                        result = ConfigurationManager.AppSettings["Entity"] + assetCode + "-" + Convert.ToInt32(locCode).ToString("00000000") + "-" + serialNo.ToString("00000");
                    }
                    else
                    {
                        string lsQry = "select  MAX(convert(int,substring(assetdetails_assetdet_id,17,len(assetdetails_assetdet_id)))) FROM fa_trn_tassetdetails ";
                        lsQry += " where assetdetails_asset_code='" + asstCode1 + "' and ";
                        lsQry += " assetdetails_isremoved='N' and assetdetails_branch_gid ='" + locCode1 + "'";
                        cmd = new SqlCommand(lsQry, con);
                        cmd.CommandType = CommandType.Text;
                        int serialNo = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
                        result = ConfigurationManager.AppSettings["Entity"] + assetCode + "-" + Convert.ToInt32(locCode).ToString("00000000") + "-" + serialNo.ToString("00000");
                    }
                }

                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public string yyMMddStringfor(string inputdate)
        {
            try
            {
                string lsInputDate = inputdate;
                string convertdate = string.Empty;
                DateTime outputDate = Convert.ToDateTime(inputdate);
                // string format = "dd-MM-yyyy";
                string format = "dd/MM/yyyy";
                convertdate = outputDate.ToString(format);
                return convertdate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string ConvertDecimaltoWords(decimal inputNumber = 0)
        {
            var values = inputNumber.ToString(CultureInfo.InvariantCulture).Split('.');
            int firstValue = int.Parse(values[0]);
            int secondValue = 0;
            if (values.Length > 1)
            {
                secondValue = int.Parse(values[1]);
            }
            string BeforeDecimal = ConvertNumbertoWords(firstValue) + " Rupees ";
            string AfterDecimal = "";
            if (secondValue > 0)
            {
                AfterDecimal = ConvertNumbertoWords(secondValue) + " Paise";
            }

            return BeforeDecimal.ToString() + AfterDecimal.ToString().TrimEnd() + " only.";
        }

        public string ConvertNumbertoWords(int inputNumber)
        {

            int inputNo = inputNumber;

            if (inputNo == 0)
                return "Zero";

            int[] numbers = new int[4];
            int first = 0;
            int u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (inputNo < 0)
            {
                sb.Append("Minus ");
                inputNo = -inputNo;
            }

            string[] words0 = {"" ,"One ", "Two ", "Three ", "Four ",
            "Five " ,"Six ", "Seven ", "Eight ", "Nine "};
            string[] words1 = {"Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ",
            "Fifteen ","Sixteen ","Seventeen ","Eighteen ", "Nineteen "};
            string[] words2 = {"Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ",
            "Seventy ","Eighty ", "Ninety "};
            string[] words3 = { "Thousand ", "Lakh ", "Crore " };

            numbers[0] = inputNo % 1000; // units
            numbers[1] = inputNo / 1000;
            numbers[2] = inputNo / 100000;
            numbers[1] = numbers[1] - 100 * numbers[2]; // thousands
            numbers[3] = inputNo / 10000000; // crores
            numbers[2] = numbers[2] - 100 * numbers[3]; // lakhs

            for (int i = 3; i > 0; i--)
            {
                if (numbers[i] != 0)
                {
                    first = i;
                    break;
                }
            }
            for (int i = first; i >= 0; i--)
            {
                if (numbers[i] == 0) continue;
                u = numbers[i] % 10; // ones
                t = numbers[i] / 10;
                h = numbers[i] / 100; // hundreds
                t = t - 10 * h; // tens
                if (h > 0) sb.Append(words0[h] + "Hundred ");
                if (u > 0 || t > 0)
                {
                    if (h > 0 || i == 0) sb.Append("and ");
                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }
                if (i != 0) sb.Append(words3[i - 1]);
            }
            return sb.ToString().TrimEnd();
        }

        public string ConvertNumbertoWords1(int number)
        {
            if (number == 0)
                return "ZERO";
            if (number < 0)
                return "minus " + ConvertNumbertoWords(Math.Abs(number));
            string words = "";
            if ((number / 1000000) > 0)
            {
                words += ConvertNumbertoWords(number / 1000000) + " MILLION ";
                number %= 1000000;
            }
            if ((number / 1000) > 0)
            {
                words += ConvertNumbertoWords(number / 1000) + " THOUSAND  ";
                number %= 1000;
            }
            if ((number / 100) > 0)
            {
                words += ConvertNumbertoWords(number / 100) + " HUNDRED ";
                number %= 100;
            }
            if (number > 0)
            {
                if (words != "")
                    words += "AND ";
                var unitsMap = new[] { "ZERO", "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE", "TEN", "ELEVEN",
                    "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN" };
                var tensMap = new[] { "ZERO", "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }
            }
            return words;
        }

        public string GetTicketNo(string SequenceCode, string RefName = null)
        {
            try
            {
                string result = "";
                int lsSequence = 0;
                string lsQry = "";
                string lsCurrentDate = "";
                DataTable dt = new DataTable();

                getconnection();

                lsQry = "select CAST(GETDATE() as DATE) as 'getdate'";

                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = lsQry;

                lsCurrentDate = Convert.ToString(cmd.ExecuteScalar());
                lsCurrentDate = convertoDateTimeString(lsCurrentDate);

                lsQry = "";
                lsQry += "Select sequence_no,CAST(sequence_date as DATE) as 'sequence_date',sequence_code ";
                lsQry += "from iem_mst_tsequence where sequence_code = '" + SequenceCode + "'";

                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = lsQry;

                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    if (convertoDateTimeString(dt.Rows[0]["sequence_date"].ToString()) != lsCurrentDate)
                    {
                        lsSequence = 1;
                        lsQry = "update iem_mst_tsequence set sequence_no='" + lsSequence + "', sequence_date='" + lsCurrentDate + "' ";
                        lsQry += " where sequence_code='" + SequenceCode + "'";
                    }
                    else
                    {
                        lsSequence = Convert.ToInt32(dt.Rows[0]["sequence_no"].ToString()) + 1;
                        lsQry = "update iem_mst_tsequence set sequence_no='" + lsSequence + "' ";
                        lsQry += " where sequence_code='" + SequenceCode + "'";
                    }
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = lsQry;
                    cmd.ExecuteNonQuery();
                    result = SequenceCode + Convert.ToString(string.IsNullOrEmpty(RefName) ? "" : RefName.ToString().ToUpper()) + yyMMddString(lsCurrentDate) + lsSequence.ToString("0000");
                }

                return result;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public string ChangeAmountFormat(string value = null)
        {
            try
            {
                decimal output = Convert.ToDecimal(value);
                CultureInfo india = new CultureInfo("hi-IN");
                string amnt = String.Format(india, "{0:c}", output);
                amnt = amnt.Remove(0, 2);
                return amnt;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public string IEMAttachmentPath()
        {
            string x = "";
            try
            {
                x = System.Configuration.ConfigurationManager.AppSettings["IEMFileUpload"].ToString();
            }
            catch { x = ""; }
            return (x == null || x.Trim() == "") ? "" : x;
        }

        public DataTable GetQueryOutput(string qry)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                getconnection();
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = qry;
                cmd.CommandTimeout = 0;
                da.SelectCommand = cmd;
                da.Fill(dt);
                return dt;





            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable GetSqlQueryOutput(string qry)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                getconnection();
                cmd = new SqlCommand("pr_execSqlQuery", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@sqlquery", SqlDbType.VarChar).Value = qry;
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string lsDate { get; set; }

        public string authorize(string MenuId)
        {
            string Result = string.Empty;
            DataTable dt = new DataTable();
            dt = HttpContext.Current.Session["Auth"] as DataTable;
            int i = 0;
            for (i = 0; i < dt.Rows.Count; i++)
            {

                if (Convert.ToInt32(dt.Rows[i]["menu_gid"]) == Convert.ToInt32(MenuId))
                {
                    Result = "Success";
                    break;
                }
                else
                {
                    Result = "Fail";
                }
            }

            return Result;
        }


        /*FOR ECFDebitline Deletion*/
        public void ECFDebtDel(Int64 ECFGid, Int64 invoiceGid, Int64 ProviderLocation, Int64 ReceiverLocation)
        {
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_eow_com_ecfdebitlinDeletion", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ecf_gid", SqlDbType.BigInt).Value = ECFGid;
                cmd.Parameters.Add("@invoice_gid", SqlDbType.BigInt).Value = invoiceGid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "DelDebitline";
                cmd.Parameters.Add("@ploc", SqlDbType.BigInt).Value = ProviderLocation;
                cmd.Parameters.Add("@rloc", SqlDbType.BigInt).Value = ReceiverLocation;
                cmd.Parameters.AddWithValue("@Userid", Convert.ToInt64(GetLoginUserGid()));
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw ex;
            }
            finally { con.Close(); }


        }

        //for Attachemnet -------------Pandiaraj 01-11-2019
        [DllImport(@"urlmon.dll", CharSet = CharSet.Auto)]
        private extern static System.UInt32 FindMimeFromData(
                System.UInt32 pBC,
                [MarshalAs(UnmanagedType.LPStr)] System.String pwzUrl,
                [MarshalAs(UnmanagedType.LPArray)] byte[] pBuffer,
                System.UInt32 cbSize,
                [MarshalAs(UnmanagedType.LPStr)] System.String pwzMimeProposed,
                System.UInt32 dwMimeFlags,
                out System.UInt32 ppwzMimeOut,
                System.UInt32 dwReserverd
        );
        public static bool GetMimeTypeFromAttachment(byte[] abytBuffer, string Attached_list, string file_format)
        {
            bool sMimeType = false;

            if (abytBuffer.Length > 0)
            {
                try
                {
                    UInt32 unMimeType;

                    FindMimeFromData(0, null, abytBuffer, 256, null, 0, out unMimeType, 0);

                    IntPtr pMimeType = new IntPtr(unMimeType);
                    string sMimeTypeFromFile = Marshal.PtrToStringUni(pMimeType);
                    //string[] Attachment_list = Attached_list.Split(',');
                    Marshal.FreeCoTaskMem(pMimeType);
                    sMimeType = !(Attached_list.Contains(sMimeTypeFromFile));
                    if (sMimeTypeFromFile == "application/x-zip-compressed" && (file_format == ".docx" || file_format == ".xlsx" || file_format == ".doc" || file_format == ".xls"))
                    {
                        sMimeType = false;
                    }
                    //for (int i = 0; i < Attachment_list.Count(); i++)
                    //{

                    //  //  sMimeType = sMimeTypeFromFile.Contains(Attachment_list[i].ToString());
                    //    //if (!String.IsNullOrEmpty(sMimeTypeFromFile) && sMimeTypeFromFile != Attachment_list[i].ToString())
                    //    //{ 
                    //    //    sMimeType = true;
                    //    //}
                    //} 
                }
                catch { }
            }

            return sMimeType;
        }
        //for Attachemnet -------------Pandiaraj 01-11-2019
    }
}