using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using IEM.Common;

namespace IEM.Areas.EOW.Models
{
    public class ECFModel : ECFRepository
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        ErrorLog objErrorLog = new ErrorLog();
        Common.CmnFunctions cmnfun = new Common.CmnFunctions();
        SupClassificationModel sub = new SupClassificationModel();
        string month;
        string result;
        public ECFModel()
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

        public IEnumerable<ECFDataModel> SelectDocType()
        {
            List<ECFDataModel> emp = new List<ECFDataModel>();
            try
            {

                ECFDataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTDOCTYPE";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new ECFDataModel();
                    objModel.Docgid = Convert.ToInt32(row["doctype_gid"].ToString());
                    objModel.Docname = row["doctype_code"].ToString();
                    emp.Add(objModel);
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            }
        }
        //public IEnumerable<ECFDataModel> SelectDocSubType()
        //{
        //    //try
        //    //{
        //    //    List<ECFDataModel> emp = new List<ECFDataModel>();
        //    //    ECFDataModel objModel;
        //    //    cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
        //    //    cmd.CommandType = CommandType.StoredProcedure;
        //    //    cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTSUBDOCTYPE";
        //    //    da = new SqlDataAdapter(cmd);
        //    //    dt = new DataTable();
        //    //    da.Fill(dt);
        //    //    foreach (DataRow row in dt.Rows)
        //    //    {
        //    //        objModel = new ECFDataModel();
        //    //        objModel.DocSubgid = Convert.ToInt32(row["docsubtype_gid"].ToString());
        //    //        objModel.DocSubname = row["docsubtype_code"].ToString();
        //    //        emp.Add(objModel);
        //    //    }
        //    //    return emp;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    throw ex;
        //    //}
        //    //finally
        //    //{
        //    //    con.Close();
        //    //}

        //}

        public IEnumerable<ECFDataModel> SelectStatus()
        {
            List<ECFDataModel> emp = new List<ECFDataModel>();
            try
            {

                ECFDataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTSTATUS";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new ECFDataModel();
                    objModel.StatusGid = Convert.ToInt32(row["ecfstatus_gid"].ToString());
                    objModel.statusname = row["ecfstatus_name"].ToString();
                    emp.Add(objModel);
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            }
        }
        public IEnumerable<ECFDataModel> SearchTEST()
        {
            List<ECFDataModel> emp = new List<ECFDataModel>();
            try
            {

                ECFDataModel objModel;
                cmd = new SqlCommand("pr_eow_trn_CTPrint", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SEARCH";
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "iem_mst_temployee";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new ECFDataModel();
                    objModel.QueueId = Convert.ToInt32(row["employee_gid"].ToString());
                    objModel.SupplierorEmployeename = row["employee_code"].ToString();
                    objModel.ECFNo = row["employee_name"].ToString();
                    objModel.ECFDate = row["employee_iem_designation"].ToString();
                    objModel.SupplierorEmployee = row["employee_grade_code"].ToString();
                    objModel.DocTypeName = row["employee_dept_name"].ToString();
                    objModel.DocSubTypeName = row["employee_branch_code"].ToString();
                    objModel.CreateMode = row["employee_fc_code"].ToString();
                    objModel.ClaimMonth = row["employee_cc_code"].ToString();
                    objModel.ECFAmount = cmnfun.GetINRAmount(row["ecf_amount"].ToString());
                    objModel.Despatchdate = row["employee_ou_code"].ToString();
                    objModel.ECFStatus = row["employee_supervisor"].ToString();
                    emp.Add(objModel);
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            }
        }
        public DataTable SearchNewecfinvoicetext()
        {
            dt = new DataTable();
            try
            {
                cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "QUERYEXPORT";
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
            }
        }
        public DataTable SearchNew()
        {
            dt = new DataTable();
            try
            {
                cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "QUERYSEARCHBYNew";
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
            }
        }
        //public IEnumerable<ECFDataModel> Search()
        //{
        //    List<ECFDataModel> emp = new List<ECFDataModel>();
        //    try
        //    {

        //        ECFDataModel objModel;
        //        cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        //cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SEARCH";
        //        //cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "QUERYSEARCHBYSAM";
        //        cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "QUERYSEARCHBYNew";
        //        da = new SqlDataAdapter(cmd);
        //        dt = new DataTable();
        //        da.Fill(dt);
        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                string print = "No";
        //                objModel = new ECFDataModel();
        //                if (row["QueueId"].ToString() != "")
        //                {
        //                    objModel.QueueId = Convert.ToInt32(row["QueueId"].ToString());
        //                }

        //                objModel.ECFId = Convert.ToInt32(row["ECFId"].ToString());
        //                objModel.ECFNo = row["ECFNo"].ToString();
        //                objModel.ECFDate = row["ECFDate"].ToString();
        //                objModel.SupplierorEmployee = row["SupplierorEmployee"].ToString();
        //                if (objModel.SupplierorEmployee == "E" || objModel.SupplierorEmployee == "P")
        //                {
        //                    objModel.SupplierorEmployeename = row["SupplierorEmployeename"].ToString();
        //                    objModel.ECFRaiser = row["ECFRaiser"].ToString();
        //                }
        //                if (objModel.SupplierorEmployee == "S")
        //                {
        //                    objModel.SupplierorEmployeename = row["SupplierorEmployeename"].ToString();
        //                    objModel.ECFRaiser = row["ECFRaiser"].ToString();
        //                }
        //                //objModel.DocTypeName = row["doctype_code"].ToString();
        //                objModel.DocSubTypeName = row["DocSubTypeName"].ToString();
        //                //objModel.CreateMode = row["ecf_create_mode"].ToString();
        //                ///objModel.ClaimMonth = row["ecf_claim_month"].ToString();
        //                objModel.ECFAmount = cmnfun.GetINRAmount(row["ECFAmount"].ToString());
        //                //objModel.Despatchdate = row["ecf_despatch_date"].ToString();
        //                //objModel.CourierName = row["ecf_courier_name"].ToString();
        //                //objModel.AWBNo = row["ecf_no"].ToString();
        //                objModel.ECFStatus = row["ECFStatus"].ToString();
        //                //objModel.CancelBy = row["ecf_cancel_by"].ToString();
        //                //objModel.CancelDate = row["ecf_cancel_date"].ToString();
        //                if (row["Reason"].ToString() == "Y")
        //                {
        //                    print = "Yes";
        //                }
        //                objModel.Reason = print;
        //                emp.Add(objModel);
        //            }
        //        }
        //        return emp;
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //        return emp;
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //}
        public IEnumerable<ECFDataModel> Searchponumberbased()
        {
            List<ECFDataModel> emp = new List<ECFDataModel>();
            try
            {

                ECFDataModel objModel;
                cmd = new SqlCommand("pr_eow_sup_getpodetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SEARCH";
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "Getecfinvoivedtls";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        objModel = new ECFDataModel();
                        if (row["QUEUE_GID"].ToString() != "")
                        {
                            objModel.QueueId = Convert.ToInt32(row["QUEUE_GID"].ToString());
                        }

                        objModel.ECFId = Convert.ToInt32(row["ecf_gid"].ToString());
                        objModel.ECFNo = row["ecf_no"].ToString();
                        objModel.ECFDate = row["ecf_date"].ToString();
                        objModel.SupplierorEmployee = row["ecf_supplier_employee"].ToString();
                        if (objModel.SupplierorEmployee == "E" || objModel.SupplierorEmployee == "P")
                        {
                            objModel.SupplierorEmployeename = row["EmployeeName"].ToString();
                            objModel.ECFRaiser = row["Raiser"].ToString();
                        }
                        if (objModel.SupplierorEmployee == "S")
                        {
                            objModel.SupplierorEmployeename = row["supplierheader_name"].ToString();
                            objModel.ECFRaiser = row["Raiser"].ToString();
                        }
                        objModel.DocSubTypeName = row["poheader_pono"].ToString();
                        objModel.ECFAmount = cmnfun.GetINRAmount(row["ecf_amount"].ToString());
                        objModel.AWBNo = row["ecf_no"].ToString();
                        objModel.ECFStatus = row["ecfstatus_name"].ToString();
                        emp.Add(objModel);
                    }
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            }
        }
        public IEnumerable<ECFDataModel> DespatchReport()
        {
            List<ECFDataModel> emp = new List<ECFDataModel>();
            try
            {

                ECFDataModel objModel;
                cmd = new SqlCommand("pr_eow_trn_CTPrint", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SEARCH";
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "DespatchReport";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    string print = "No";
                    objModel = new ECFDataModel();
                    objModel.QueueId = Convert.ToInt32(row["QUEUE_GID"].ToString());
                    objModel.ECFId = Convert.ToInt32(row["ecf_gid"].ToString());
                    objModel.ECFNo = row["ecf_no"].ToString();
                    objModel.ECFDate = row["ecf_date"].ToString();
                    objModel.SupplierorEmployee = row["ecf_supplier_employee"].ToString();
                    if (objModel.SupplierorEmployee == "E")
                    {
                        objModel.SupplierorEmployeename = row["EmployeeName"].ToString();
                        objModel.ECFRaiser = row["Raiser"].ToString();
                    }
                    if (objModel.SupplierorEmployee == "S")
                    {
                        objModel.SupplierorEmployeename = row["supplierheader_name"].ToString();
                        objModel.ECFRaiser = row["Raiser"].ToString();
                    }
                    objModel.DocTypeName = row["doctype_code"].ToString();
                    objModel.DocSubTypeName = row["docsubtype_name"].ToString();
                    objModel.CreateMode = row["ecf_create_mode"].ToString();
                    objModel.ClaimMonth = row["ecf_claim_month"].ToString();
                    //objModel.ECFAmount = row["ecf_amount"].ToString();
                    objModel.ECFAmount = cmnfun.GetINRAmount(row["ecf_amount"].ToString());
                    objModel.Despatchdate = row["ecf_despatch_date"].ToString();
                    objModel.CourierName = row["ecf_courier_name"].ToString();
                    objModel.AWBNo = row["ecf_awb_no"].ToString();
                    objModel.ECFStatus = row["ecfstatus_name"].ToString();
                    objModel.CancelBy = row["ecf_cancel_by"].ToString();
                    objModel.CancelDate = row["ecf_cancel_date"].ToString();
                    if (row["ecf_printflag"].ToString() == "Y")
                    {
                        print = "Yes";
                    }
                    objModel.Reason = print;

                    emp.Add(objModel);
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            }
        }
        public IEnumerable<ECFDataModel> AllPrintdata()
        {
            List<ECFDataModel> emp = new List<ECFDataModel>();
            try
            {

                ECFDataModel objModel;
                cmd = new SqlCommand("pr_eow_trn_CTPrint", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SEARCH";
                cmd.Parameters.Add("@ecf_gid", SqlDbType.Int).Value = cmnfun.GetLoginUserGid();
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "QUERYAllPrint";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    string print = "No";
                    objModel = new ECFDataModel();
                    objModel.QueueId = Convert.ToInt32(row["QUEUE_GID"].ToString());
                    objModel.ECFId = Convert.ToInt32(row["ecf_gid"].ToString());
                    objModel.ECFNo = row["ecf_no"].ToString();
                    objModel.ECFDate = row["ecf_date"].ToString();
                    objModel.SupplierorEmployee = row["ecf_supplier_employee"].ToString();
                    if (objModel.SupplierorEmployee == "E")
                    {
                        objModel.SupplierorEmployeename = row["EmployeeName"].ToString();
                        objModel.ECFRaiser = row["Raiser"].ToString();
                    }
                    if (objModel.SupplierorEmployee == "S")
                    {
                        objModel.SupplierorEmployeename = row["supplierheader_name"].ToString();
                        objModel.ECFRaiser = row["Raiser"].ToString();
                    }
                    objModel.DocTypeName = row["doctype_code"].ToString();
                    objModel.DocSubTypeName = row["docsubtype_name"].ToString();
                    objModel.CreateMode = row["ecf_create_mode"].ToString();
                    objModel.ClaimMonth = row["ecf_claim_month"].ToString();
                    //objModel.ECFAmount = row["ecf_amount"].ToString();

                    objModel.ECFAmount = cmnfun.GetINRAmount(row["ecf_amount"].ToString());
                    objModel.Despatchdate = row["ecf_despatch_date"].ToString();
                    objModel.CourierName = row["ecf_courier_name"].ToString();
                    objModel.AWBNo = row["ecf_no"].ToString();
                    objModel.ECFStatus = row["ecfstatus_name"].ToString();
                    objModel.CancelBy = row["ecf_cancel_by"].ToString();
                    objModel.CancelDate = row["ecf_cancel_date"].ToString();
                    if (row["ecf_printflag"].ToString() == "Y")
                    {
                        print = "Yes";
                    }
                    objModel.Reason = print;
                    emp.Add(objModel);
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            }
        }
        public IEnumerable<ECFDataModel> ctdatdPrintdata(string ecffromdate, string ecfTodate, string ecfno, string printstatus, string searchtype)
        {
            List<ECFDataModel> emp = new List<ECFDataModel>();
            try
            {

                ECFDataModel objModel;
                cmd = new SqlCommand("pr_eow_trn_CTPrint", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SEARCH";
                cmd.Parameters.Add("@ecf_gid", SqlDbType.Int).Value = cmnfun.GetLoginUserGid();
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "QUERYAllPrintget";
                cmd.Parameters.Add("@EcfFromDate", SqlDbType.VarChar).Value = ecffromdate;
                cmd.Parameters.Add("@EcfToDate", SqlDbType.VarChar).Value = ecfTodate;
                cmd.Parameters.Add("@EcfNo", SqlDbType.VarChar).Value = ecfno;
                cmd.Parameters.Add("@printstatus", SqlDbType.VarChar).Value = printstatus;
                cmd.Parameters.Add("@SearchType", SqlDbType.VarChar).Value = searchtype;
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    string print = "No";
                    objModel = new ECFDataModel();
                    objModel.QueueId = Convert.ToInt32(row["QUEUE_GID"].ToString());
                    if (objModel.QueueId > 0) {
                        objModel.ECFId = Convert.ToInt32(row["ecf_gid"].ToString());
                        objModel.ECFNo = row["ecf_no"].ToString();
                        objModel.ECFDate = row["ecf_date"].ToString();
                        objModel.SupplierorEmployee = row["ecf_supplier_employee"].ToString();
                        if (objModel.SupplierorEmployee == "E")
                        {
                            objModel.SupplierorEmployeename = row["EmployeeName"].ToString();
                            objModel.ECFRaiser = row["Raiser"].ToString();
                        }
                        if (objModel.SupplierorEmployee == "S")
                        {
                            objModel.SupplierorEmployeename = row["supplierheader_name"].ToString();
                            objModel.ECFRaiser = row["Raiser"].ToString();
                        }
                        objModel.DocTypeName = row["doctype_code"].ToString();
                        objModel.DocSubTypeName = row["docsubtype_name"].ToString();
                        objModel.CreateMode = row["ecf_create_mode"].ToString();
                        objModel.ClaimMonth = row["ecf_claim_month"].ToString();
                        //objModel.ECFAmount = row["ecf_amount"].ToString();

                        objModel.ECFAmount = cmnfun.GetINRAmount(row["ecf_amount"].ToString());
                        objModel.Despatchdate = row["ecf_despatch_date"].ToString();
                        objModel.CourierName = row["ecf_courier_name"].ToString();
                        objModel.AWBNo = row["ecf_no"].ToString();
                        objModel.ECFStatus = row["ecfstatus_name"].ToString();
                        objModel.CancelBy = row["ecf_cancel_by"].ToString();
                        objModel.CancelDate = row["ecf_cancel_date"].ToString();
                        if (row["ecf_printflag"].ToString() == "Y")
                        {
                            print = "Yes";
                        }
                        objModel.Reason = print;
                        emp.Add(objModel);
                    }
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            }
        }
        public IEnumerable<ECFDataModel> CTPrintdata()
        {
            List<ECFDataModel> emp = new List<ECFDataModel>();
            try
            {
                ECFDataModel objModel;
                cmd = new SqlCommand("pr_eow_trn_CTPrint", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "QUERYCTPrint";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new ECFDataModel();
                    objModel.QueueId = Convert.ToInt32(row["QUEUE_GID"].ToString());
                    objModel.ECFId = Convert.ToInt32(row["ecf_gid"].ToString());
                    objModel.ECFNo = row["ecf_no"].ToString();
                    objModel.ECFDate = row["ecf_date"].ToString();
                    objModel.SupplierorEmployee = row["ecf_supplier_employee"].ToString();
                    if (objModel.SupplierorEmployee == "E")
                    {
                        objModel.SupplierorEmployeename = row["EmployeeName"].ToString();
                        objModel.ECFRaiser = row["Raiser"].ToString();
                    }
                    if (objModel.SupplierorEmployee == "S")
                    {
                        objModel.SupplierorEmployeename = row["supplierheader_name"].ToString();
                        objModel.ECFRaiser = row["Raiser"].ToString();
                    }
                    objModel.ECFAmount = cmnfun.GetINRAmount(row["ecf_amount"].ToString());

                    objModel.DocTypeName = row["invoice_no"].ToString();
                    objModel.DocSubTypeName = row["invoice_date"].ToString();
                    objModel.CreateMode = row["invoice_amount"].ToString();

                    emp.Add(objModel);
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            }
        }

        public IEnumerable<ECFDataModel> Search(string EcfDateFrom = null, string EcfDateTo = null, string DocType = null, string docsubtype = null, string Code = null, string Name = null, string ECFClaimMonth = null, string queryempsup = null, string ECFDespatchDateTo = null, string ecfnumber = null, string ecfamount = null, string Ecfstatus = null, string ECFMode = null, string command = null,string Suppliercode = null,string Suppliername = null)
        {
            List<ECFDataModel> emp = new List<ECFDataModel>();
            try
            {
                ECFDataModel objModel;
                cmd = new SqlCommand("pr_eow_trn_getECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@doctype_name", DocType);
                cmd.Parameters.AddWithValue("@docsubtype_name", docsubtype);
                cmd.Parameters.AddWithValue("@ecfstatus_name", Ecfstatus);
                cmd.Parameters.AddWithValue("@ecf_no", ecfnumber);
                cmd.Parameters.AddWithValue("@ecf_amount", ecfamount);
                cmd.Parameters.AddWithValue("@ecf_create_mode", ECFMode);

                cmd.Parameters.AddWithValue("@employee_code", Code);
                cmd.Parameters.AddWithValue("@supplierheader_code", Suppliercode);
           
               
                if (Name != "")
                {

                    cmd.Parameters.AddWithValue("@employee_name", Name);
                }
                else if (Suppliername != "")
                {
                    cmd.Parameters.AddWithValue("@supplierheader_name", Suppliername);
                }


                cmd.Parameters.AddWithValue("@ecf_supplier_employee", queryempsup);
                // cmd.Parameters.AddWithValue("@ecf_courier_name", ECFStatus);
                if (EcfDateFrom != "")
                {
                    cmd.Parameters.AddWithValue("@ecf_dateFrom", cmnfun.convertoDateTimeString(EcfDateFrom).ToString());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ecf_dateFrom", EcfDateFrom);
                }
                if (EcfDateTo != "")
                {
                    cmd.Parameters.AddWithValue("@ecf_dateTo", cmnfun.convertoDateTimeString(EcfDateTo).ToString());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ecf_dateTo", EcfDateTo);
                }
                if (ECFClaimMonth != "")
                {
                    //string datefor = cmnfun.yyMMddStringfor(ECFClaimMonth).ToString();
                    //string[] arr = Regex.Split(datefor, "-");
                    //month = arr[1];
                    //cmd.Parameters.AddWithValue("@ecf_claim_month", month);
                    cmd.Parameters.AddWithValue("@ecf_claim_month", ECFClaimMonth);
                }

                // cmd.Parameters.AddWithValue("@ecf_claim_month",month);
                // cmd.Parameters.AddWithValue("@ecf_dateTo", EcfDateTo);
                //cmd.Parameters.AddWithValue("@emplogin", cmnfun.GetLoginUserGid().ToString());
                //cmd.Parameters.AddWithValue("@ACTION", "QuerysearchSam");
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.CommandTimeout = 0;
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {

                    objModel = new ECFDataModel();
                    if (row["QUEUE_GID"].ToString() != null && row["QUEUE_GID"].ToString()!="")
                    {
                        objModel.QueueId = Convert.ToInt32(row["QUEUE_GID"].ToString());
                    }
                    objModel.ECFId = Convert.ToInt32(row["ecf_gid"].ToString());
                    objModel.ECFNo = row["ecf_no"].ToString();
                    objModel.ECFDate = row["ecf_date"].ToString();
                    objModel.SupplierorEmployee = row["ecf_supplier_employee"].ToString();
                    if (objModel.SupplierorEmployee == "E")
                    {
                        objModel.SupplierorEmployeename = row["EmployeeName"].ToString();
                        objModel.ECFRaiser = row["Raiser"].ToString();
                    }
                    if (objModel.SupplierorEmployee == "S" || objModel.SupplierorEmployee == "I")
                    {
                        objModel.SupplierorEmployeename = row["supplierheader_name"].ToString();
                        objModel.ECFRaiser = row["Raiser"].ToString();
                    }
                    objModel.DocTypeName = row["doctype_code"].ToString();
                    objModel.DocSubTypeName = row["docsubtype_name"].ToString();
                    objModel.CreateMode = row["ecf_create_mode"].ToString();
                    objModel.ClaimMonth = row["ecf_claim_month"].ToString();
                    objModel.ECFAmount = cmnfun.GetINRAmount(row["ecf_amount"].ToString());
                    objModel.Despatchdate = row["ecf_despatch_date"].ToString();
                    objModel.CourierName = row["ecf_courier_name"].ToString();
                    objModel.AWBNo = row["ecf_no"].ToString();
                    objModel.ECFStatus = row["ecfstatus_name"].ToString();
                    objModel.CancelBy = row["ecf_cancel_by"].ToString();
                    objModel.CancelDate = row["ecf_cancel_date"].ToString();
                    if (row["ecf_raiser"].ToString() != null && row["ecf_raiser"].ToString()!="")
                    {
                        objModel.ECFRaiserid = Convert.ToInt32(row["ecf_raiser"].ToString());
                    }
                    objModel.PrintStatus = row["PrintStatus"].ToString();

                    emp.Add(objModel);
                }
                return emp;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            }
        }
        public IEnumerable<ECFDataModel> View(int id)
        {
            List<ECFDataModel> emp = new List<ECFDataModel>();
            try
            {

                ECFDataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ecf_gid", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "VIEW";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new ECFDataModel();
                    objModel.ECFId = Convert.ToInt32(row["ecf_gid"].ToString());
                    objModel.ECFNo = row["ecf_no"].ToString();
                    objModel.ECFDate = row["ecf_date"].ToString();
                    objModel.SupplierorEmployee = row["ecf_supplier_employee"].ToString();
                    if (objModel.SupplierorEmployee == "E")
                    {
                        objModel.SupplierorEmployeename = row["employee_name"].ToString();
                        objModel.ECFRaiser = row["EmployeeName"].ToString();
                    }
                    if (objModel.SupplierorEmployee == "S")
                    {
                        objModel.SupplierorEmployeename = row["supplierheader_name"].ToString();
                        objModel.ECFRaiser = row["EmployeeName"].ToString();
                    }
                    objModel.DocTypeName = row["doctype_name"].ToString();
                    objModel.DocSubTypeName = row["docsubtype_code"].ToString();
                    objModel.CreateMode = row["ecf_create_mode"].ToString();
                    objModel.ClaimMonth = row["ecf_claim_month"].ToString();
                    objModel.ECFAmount = row["ecf_amount"].ToString();
                    objModel.Despatchdate = row["ecf_despatch_date"].ToString();
                    objModel.CourierName = row["ecf_courier_name"].ToString();
                    objModel.AWBNo = row["ecf_awb_no"].ToString();
                    objModel.ECFStatus = row["ecfstatus_name"].ToString();
                    objModel.CancelBy = row["ecf_cancel_by"].ToString();
                    objModel.CancelDate = row["ecf_cancel_date"].ToString();

                    objModel.CurrencyCode = row["ecf_currency_code"].ToString();
                    objModel.CurrencyAmount = row["ecf_currency_amount"].ToString();
                    objModel.CurrencyRate = row["ecf_currency_rate"].ToString();
                    objModel.ReducedAmount = row["ecf_reduced_amount"].ToString();
                    objModel.ProcessedAmount = row["ecf_processed_amount"].ToString();
                    objModel.EcfRemark = row["ecf_remark"].ToString();
                    emp.Add(objModel);
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            }
        }
        public IEnumerable<ECFDataModel> Viewwithqueue(int id)
        {
            List<ECFDataModel> emp = new List<ECFDataModel>();
            try
            {

                ECFDataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ecf_gid", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "VIEWwithqueue";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new ECFDataModel();
                    objModel.QueueId = id;
                    objModel.ECFId = Convert.ToInt32(row["ecf_gid"].ToString());
                    objModel.ECFNo = row["ecf_no"].ToString();
                    objModel.ECFDate = row["ecf_date"].ToString();
                    objModel.SupplierorEmployee = row["ecf_supplier_employee"].ToString();
                    if (objModel.SupplierorEmployee == "E")
                    {
                        objModel.SupplierorEmployeename = row["employee_name"].ToString();
                        objModel.ECFRaiser = row["EmployeeName"].ToString();
                    }
                    else if (objModel.SupplierorEmployee == "S")
                    {
                        objModel.SupplierorEmployeename = row["supplierheader_name"].ToString();
                        objModel.ECFRaiser = row["EmployeeName"].ToString();
                    }
                    objModel.DocTypeName = row["doctype_name"].ToString();
                    objModel.DocSubTypeName = row["docsubtype_code"].ToString();
                    objModel.CreateMode = row["ecf_create_mode"].ToString();
                    objModel.ClaimMonth = row["ecf_claim_month"].ToString();
                    objModel.ECFAmount = row["ecf_amount"].ToString();
                    objModel.Despatchdate = row["ecf_despatch_date"].ToString();
                    objModel.CourierName = row["ecf_courier_name"].ToString();
                    objModel.AWBNo = row["ecf_awb_no"].ToString();
                    objModel.ECFStatus = row["ecfstatus"].ToString();
                    objModel.CancelBy = row["ecf_cancel_by"].ToString();
                    objModel.CancelDate = row["ecf_cancel_date"].ToString();

                    objModel.CurrencyCode = row["ecf_currency_code"].ToString();
                    objModel.CurrencyAmount = row["ecf_currency_amount"].ToString();
                    objModel.CurrencyRate = row["ecf_currency_rate"].ToString();
                    objModel.ReducedAmount = row["ecf_reduced_amount"].ToString();
                    objModel.ProcessedAmount = row["ecf_processed_amount"].ToString();
                    objModel.EcfRemark = row["ecf_remark"].ToString();
                    emp.Add(objModel);
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            }
        }
        public IEnumerable<ECFDataModel> CancellationSearch()
        {
            List<ECFDataModel> emp = new List<ECFDataModel>();
            try
            {

                ECFDataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                //cmd.Parameters.AddWithValue("@emplogin",cmnfun.GetLoginUserGid().ToString());
                cmd.Parameters.AddWithValue("@emplogin", cmnfun.GetLoginUserGid().ToString());
                cmd.Parameters.AddWithValue("@ACTION", "SELECTGRIDCANCELLATION");
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "CANCELLATIONSEARCH";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new ECFDataModel();
                    objModel.ECFId = Convert.ToInt32(row["ecf_gid"].ToString());
                    objModel.ECFNo = row["ecf_no"].ToString();
                    objModel.ECFDate = row["ecf_date"].ToString();
                    objModel.SupplierorEmployee = row["ecf_supplier_employee"].ToString();
                    if (objModel.SupplierorEmployee == "E")
                    {
                        objModel.SupplierorEmployeename = row["employee_name"].ToString();
                        objModel.ECFRaiser = row["ecfraiser"].ToString();
                    }
                    else if (objModel.SupplierorEmployee == "S")
                    {
                        objModel.SupplierorEmployeename = row["supplierheader_name"].ToString();
                        objModel.ECFRaiser = row["ecfraiser"].ToString();
                    }
                    //objModel.DocTypeName = row["doctype_code"].ToString();
                    objModel.DocSubTypeName = row["docsubtype_name"].ToString();
                    objModel.CreateMode = row["ecf_create_mode"].ToString();
                    objModel.ClaimMonth = row["ecf_claim_month"].ToString();
                    //objModel.ECFAmount = row["ecf_amount"].ToString();
                    objModel.ECFAmount = cmnfun.GetINRAmount(row["ecf_amount"].ToString());

                    //objModel.Despatchdate = row["ecf_despatch_date"].ToString();
                    //objModel.CourierName = row["ecf_courier_name"].ToString();
                    //objModel.AWBNo = row["ecf_awb_no"].ToString();
                    objModel.ECFStatus = row["ECF Status"].ToString();
                    //objModel.CancelBy = row["ecf_cancel_by"].ToString();
                    //objModel.CancelDate = row["ecf_cancel_date"].ToString();
                    emp.Add(objModel);
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            }
        }
        public IEnumerable<ECFDataModel> CancellationSearch(string EcfDateFrom, string EcfDateTo, string DocType, string DocSubType, string Code, string Name, string ECFClaimMonth, string SupplierorEmployee, string ECFDespatchDateTo, string Suppliername, string ecfmode, string ECFStatus, string ecfamount)
        {
            List<ECFDataModel> emp = new List<ECFDataModel>();
            try
            {
                //if (ECFClaimMonth != null && ECFClaimMonth != "")
                //{
                //    ECFDataModel bat = new ECFDataModel();
                //    string[] arr = Regex.Split(ECFClaimMonth, "-");
                //    month = arr[1];
                //    string year = arr[2];
                //}

                ECFDataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@doctype_name", DocType);
                cmd.Parameters.AddWithValue("@docsubtype_name", DocSubType);
                cmd.Parameters.AddWithValue("@ecf_amount", ecfamount);
                cmd.Parameters.AddWithValue("@ecf_create_mode", ecfmode);
                //cmd.Parameters.AddWithValue("@ecf_no", ecfnumbercancel);

                if (SupplierorEmployee == "E" && Name != "")
                {

                    cmd.Parameters.AddWithValue("@employee_name", Name);
                }
                else if (SupplierorEmployee == "S" && Name != "")
                {
                    cmd.Parameters.AddWithValue("@supplierheader_name", Name);
                }

                // cmd.Parameters.AddWithValue("@employee_name",Name);
                // cmd.Parameters.AddWithValue("@supplierheader_name", Name);
                cmd.Parameters.AddWithValue("@ecf_supplier_employee", SupplierorEmployee);
                if (EcfDateFrom != "")
                {
                    cmd.Parameters.AddWithValue("@ecf_dateFrom", cmnfun.convertoDateTimeString(EcfDateFrom).ToString());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ecf_dateFrom", EcfDateFrom);
                }
                if (EcfDateTo != "")
                {
                    cmd.Parameters.AddWithValue("@ecf_dateTo", cmnfun.convertoDateTimeString(EcfDateTo).ToString());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ecf_dateTo", EcfDateTo);
                }
                if (ECFClaimMonth != "")
                {
                    string datefor = cmnfun.yyMMddStringfor(ECFClaimMonth).ToString();
                    string[] arr = Regex.Split(datefor, "-");
                    month = arr[1];
                    cmd.Parameters.AddWithValue("@ecf_claim_month", month);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ecf_claim_month", ECFClaimMonth);
                }
                // cmd.Parameters.AddWithValue("@ecf_dateTo", EcfDateTo);
                cmd.Parameters.AddWithValue("@emplogin", cmnfun.GetLoginUserGid().ToString());
                cmd.Parameters.AddWithValue("@ACTION", "CANCELLATIONSEARCHBYSam");
                cmd.CommandType = CommandType.StoredProcedure;

                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new ECFDataModel();
                    objModel.ECFId = Convert.ToInt32(row["ecf_gid"].ToString());
                    objModel.ECFNo = row["ecf_no"].ToString();
                    objModel.ECFDate = row["ecf_date"].ToString();
                    objModel.SupplierorEmployee = row["ecf_supplier_employee"].ToString();
                    if (objModel.SupplierorEmployee == "E")
                    {
                        objModel.SupplierorEmployeename = row["employee_name"].ToString();
                        objModel.ECFRaiser = row["EmployeeName"].ToString();
                        //objModel.ECFRaiser = row["employee_name"].ToString();
                    }
                    if (objModel.SupplierorEmployee == "S")
                    {
                        objModel.SupplierorEmployeename = row["supplierheader_name"].ToString();
                        objModel.ECFRaiser = row["EmployeeName"].ToString();
                        // objModel.ECFRaiser = row["employee_name"].ToString();
                    }
                    //objModel.ECFRaiser = row["ecfstatus_name"].ToString();
                    objModel.DocTypeName = row["doctype_code"].ToString();
                    objModel.DocSubTypeName = row["docsubtype_name"].ToString();
                    objModel.CreateMode = row["ecf_create_mode"].ToString();
                    objModel.ClaimMonth = row["ecf_claim_month"].ToString();
                    objModel.ECFAmount = cmnfun.GetINRAmount(row["ecf_amount"].ToString());
                    objModel.Despatchdate = row["ecf_despatch_date"].ToString();
                    objModel.CourierName = row["ecf_courier_name"].ToString();
                    objModel.AWBNo = row["ecf_awb_no"].ToString();
                    objModel.ECFStatus = row["ECF Status"].ToString();
                    //objModel.CancelBy = row["ecf_cancel_by"].ToString();
                    // objModel.CancelDate = row["ecf_cancel_date"].ToString();
                    emp.Add(objModel);
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            }
        }
        public string UpdateCancellation(int id)
        {
            string delant = "0";
            try
            {
                //SqlCommand cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@ecf_gid", SqlDbType.Int).Value = id;
                //cmd.Parameters.AddWithValue("@CANCELBY", SqlDbType.VarChar).Value = cmnfun.GetLoginUserGid().ToString();
                //cmd.Parameters.AddWithValue("@ACTION", SqlDbType.VarChar).Value = "UPDATECANCELLATION";
                //result = Convert.ToString(cmd.ExecuteNonQuery());
                //if (result == "1")
                //{
                //    result = "UpdatedCancel";
                //}
                //return result;

                DataTable dt = new DataTable();
                GetConnection();
                cmd = new SqlCommand("pr_eow_com_ecfdraftdelete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ecf_gid", SqlDbType.VarChar).Value = id.ToString();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "ctdraftppxselect";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string ppxamt = Convert.ToString(dt.Rows[i]["ecfcreditline_amount"].ToString());
                    string ppxamtgid = Convert.ToString(dt.Rows[i]["ecfarf_gid"].ToString());

                    GetConnection();
                    cmd = new SqlCommand("pr_eow_com_ecfdraftdelete", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ppx_amt", SqlDbType.VarChar).Value = ppxamt;
                    cmd.Parameters.Add("@ppx_gid", SqlDbType.VarChar).Value = ppxamtgid;
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "ctdraftppxupdate";
                    int k = cmd.ExecuteNonQuery();
                }

                GetConnection();
                cmd = new SqlCommand("pr_eow_com_ecfdraftdelete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ecf_gid", SqlDbType.VarChar).Value = id.ToString();
                cmd.Parameters.Add("@employee_gid", SqlDbType.VarChar).Value = cmnfun.GetLoginUserGid().ToString();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "ctdraftdeleteecf";
                int j = cmd.ExecuteNonQuery();
                delant = "UpdatedCancel";
                return delant;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
        }
        public string Delete(ECFDataModel obj, int id)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@queue_gid", SqlDbType.Int).Value = id;
                cmd.Parameters.AddWithValue("@CANCELBY", SqlDbType.VarChar).Value = cmnfun.GetLoginUserGid().ToString();
                cmd.Parameters.Add("@action_remark", SqlDbType.VarChar).Value = cmnfun.Getreplacesinglequotes(obj.EcfRemark);
                cmd.Parameters.AddWithValue("@ACTION", SqlDbType.VarChar).Value = "ECFDELETETION";
                result = Convert.ToString(cmd.ExecuteNonQuery());
                //if (result == "2")
                //{
                result = "Deleted";
                //}
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
        }
        public IEnumerable<ECFDataModel> SelectDespatch(int id)
        {
            List<ECFDataModel> emp = new List<ECFDataModel>();
            try
            {

                ECFDataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", "SELECTGRIDESPATCH");
                cmd.Parameters.AddWithValue("@emplogin", cmnfun.GetLoginUserGid().ToString());
                //cmd.Parameters.AddWithValue("@emplogin", cmnfun.GetLoginUserGid().ToString());
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new ECFDataModel();
                    objModel.ECFId = Convert.ToInt32(row["ecf_gid"].ToString());
                    objModel.ECFNo = row["ecf_no"].ToString();
                    objModel.ECFDate = row["ecf_date"].ToString();
                    objModel.SupplierorEmployee = row["ecf_supplier_employee"].ToString();
                    if (objModel.SupplierorEmployee == "E")
                    {
                        objModel.SupplierorEmployeename = row["employee_name"].ToString();
                        objModel.ECFRaiser = row["EmployeeName"].ToString();
                    }
                    if (objModel.SupplierorEmployee == "S")
                    {
                        objModel.SupplierorEmployeename = row["supplierheader_name"].ToString();
                        objModel.ECFRaiser = row["EmployeeName"].ToString();
                    }
                    objModel.DocTypeName = row["doctype_code"].ToString();
                    objModel.DocSubTypeName = row["docsubtype_name"].ToString();
                    objModel.CreateMode = row["ecf_create_mode"].ToString();
                    objModel.ClaimMonth = row["ecf_claim_month"].ToString();
                    objModel.ECFAmount = cmnfun.GetINRAmount(row["ecf_amount"].ToString());
                    objModel.Despatchdate = row["ecf_despatch_date"].ToString();
                    objModel.CourierName = row["ecf_courier_name"].ToString();
                    objModel.AWBNo = row["ecf_awb_no"].ToString();
                    objModel.ECFStatus = row["ecfstatus_name"].ToString();
                    // objModel.CancelBy = row["ecf_cancel_by"].ToString();
                    //objModel.CancelDate = row["ecf_cancel_date"].ToString();
                    emp.Add(objModel);
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            }
        }
        public IEnumerable<ECFDataModel> SelectDespatchByEcfno(int id)
        {
            List<ECFDataModel> emp = new List<ECFDataModel>();
            try
            {

                ECFDataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ecf_gid", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTDESPATCHBYECFNO";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new ECFDataModel();
                    objModel.ECFId = Convert.ToInt32(row["ecf_gid"].ToString());
                    objModel.ECFNo = row["ecf_no"].ToString();
                    objModel.ECFDate = row["ecf_date"].ToString();
                    objModel.SupplierorEmployee = row["ecf_supplier_employee"].ToString();
                    if (objModel.SupplierorEmployee == "E")
                    {
                        objModel.SupplierorEmployeename = row["employee_name"].ToString();
                        objModel.ECFRaiser = row["EmployeeName"].ToString();
                    }
                    if (objModel.SupplierorEmployee == "S")
                    {
                        objModel.SupplierorEmployeename = row["supplierheader_name"].ToString();
                        objModel.ECFRaiser = row["EmployeeName"].ToString();
                    }
                    objModel.DocTypeName = row["doctype_name"].ToString();
                    objModel.DocSubTypeName = row["docsubtype_code"].ToString();
                    objModel.CreateMode = row["ecf_create_mode"].ToString();
                    objModel.ClaimMonth = row["ecf_claim_month"].ToString();
                    objModel.ECFAmount = cmnfun.GetINRAmount(row["ecf_amount"].ToString());
                    objModel.Despatchdate = row["ecf_despatch_date"].ToString();
                    objModel.CourierName = row["ecf_courier_name"].ToString();
                    objModel.AWBNo = row["ecf_awb_no"].ToString();
                    objModel.ECFStatus = row["ecfstatus_name"].ToString();
                    objModel.CancelBy = row["ecf_cancel_by"].ToString();
                    objModel.CancelDate = row["ecf_cancel_date"].ToString();
                    emp.Add(objModel);
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            }
        }
        public string IfCheck(ECFDataModel ecf)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                cmd.Parameters.AddWithValue("@ACTION", "CheckValidEcfNo");
                cmd.Parameters.AddWithValue("@ECFNO", SqlDbType.VarChar).Value = ecf.ECFNo;
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                // result = cmd.ExecuteNonQuery().ToString();

                if (dt.Rows.Count > 0)
                {
                    result = "VALID";
                }
                else
                {
                    result = "INVALID";
                }
                return result;
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
        }

        public string DespatchUpdate(ECFDataModel ecf, string[] ecfids)
        {
            try
            {
                foreach (string id in ecfids)
                {
                    GetConnection();
                    SqlCommand cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                    cmd.Parameters.AddWithValue("@ACTION", SqlDbType.VarChar).Value = "DESPATCHUPDATE";
                    if (ecf.Despatchdate != null)
                    {
                        cmd.Parameters.AddWithValue("@ecf_despatch_dateFrom", SqlDbType.VarChar).Value = cmnfun.convertoDateTimeString(ecf.Despatchdate.ToString());
                    }
                    else
                    {
                        ecf.Despatchdate = "";
                    }
                    cmd.Parameters.AddWithValue("@ecf_courier_name", SqlDbType.VarChar).Value = ecf.CourierName;
                    cmd.Parameters.AddWithValue("@ecf_awb_no", SqlDbType.VarChar).Value = ecf.AWBNo;
                    cmd.Parameters.AddWithValue("@REASON", SqlDbType.VarChar).Value = ecf.Reason;
                    cmd.Parameters.AddWithValue("@ecf_gid", SqlDbType.Int).Value = id;
                    cmd.CommandType = CommandType.StoredProcedure;
                    result = cmd.ExecuteNonQuery().ToString();
                    if (result == "1")
                    {
                        result = "Successfully Despatched";
                    }
                }
                return result;
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
        }
        public IEnumerable<ECFDataModel> SelectDespatchByEcfNumber(string id)
        {
            List<ECFDataModel> emp = new List<ECFDataModel>();
            try
            {

                ECFDataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ECFNO", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTDESPATCHBYECFNO";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new ECFDataModel();
                    objModel.ECFId = Convert.ToInt32(row["ecf_gid"].ToString());
                    objModel.ECFNo = row["ecf_no"].ToString();
                    objModel.ECFDate = row["ecf_date"].ToString();
                    objModel.SupplierorEmployee = row["ecf_supplier_employee"].ToString();
                    if (objModel.SupplierorEmployee == "E")
                    {
                        objModel.SupplierorEmployeename = row["employee_name"].ToString();
                        objModel.ECFRaiser = row["EmployeeName"].ToString();
                    }
                    if (objModel.SupplierorEmployee == "S")
                    {
                        objModel.SupplierorEmployeename = row["supplierheader_name"].ToString();
                        objModel.ECFRaiser = row["EmployeeName"].ToString();
                    }
                    objModel.DocTypeName = row["doctype_name"].ToString();
                    objModel.DocSubTypeName = row["docsubtype_code"].ToString();
                    objModel.CreateMode = row["ecf_create_mode"].ToString();
                    objModel.ClaimMonth = row["ecf_claim_month"].ToString();
                    objModel.ECFAmount = cmnfun.GetINRAmount(row["ecf_amount"].ToString());
                    objModel.Despatchdate = row["ecf_despatch_date"].ToString();
                    objModel.CourierName = row["ecf_courier_name"].ToString();
                    objModel.AWBNo = row["ecf_awb_no"].ToString();
                    objModel.ECFStatus = row["ecfstatus_name"].ToString();
                    objModel.CancelBy = row["ecf_cancel_by"].ToString();
                    objModel.CancelDate = row["ecf_cancel_date"].ToString();
                    emp.Add(objModel);
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            }
        }
        public IEnumerable<ECFDataModel> HoldReport(string EcfDateFrom, string DocType, string Code, string amount)
        {
            List<ECFDataModel> emp = new List<ECFDataModel>();
            try
            {


                ECFDataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ecf_dateFrom", SqlDbType.VarChar).Value = EcfDateFrom;
                if (DocType != "" || DocType == null)
                {
                    cmd.Parameters.Add("@doctype_name", SqlDbType.Int).Value = Convert.ToInt32(DocType);
                }
                cmd.Parameters.Add("@employee_code", SqlDbType.VarChar).Value = Code;
                cmd.Parameters.Add("@ecf_claim_month", SqlDbType.VarChar).Value = month;
                cmd.Parameters.Add("@ecf_amount", SqlDbType.VarChar).Value = amount;
                cmd.Parameters.Add("@emplogin", SqlDbType.VarChar).Value = cmnfun.GetLoginUserGid().ToString();
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "HOLDREPORTSEARCH";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new ECFDataModel();
                    if (row["ecfhold_queue_gid"].ToString() != "")
                    {
                        objModel.QueueId = Convert.ToInt32(row["ecfhold_queue_gid"].ToString());
                    }
                    objModel.ECFId = Convert.ToInt32(row["ecf_gid"].ToString());
                    objModel.ECFNo = row["ecf_no"].ToString();
                    objModel.ECFDate = row["ecf_date"].ToString();
                    objModel.SupplierorEmployee = row["ecf_supplier_employee"].ToString();
                    if (objModel.SupplierorEmployee == "E")
                    {
                        objModel.SupplierorEmployeename = row["employee_name"].ToString();
                        objModel.ECFRaiser = row["EmployeeName"].ToString();
                    }
                    if (objModel.SupplierorEmployee == "S")
                    {
                        objModel.SupplierorEmployeename = row["supplierheader_name"].ToString();
                        objModel.ECFRaiser = row["EmployeeName"].ToString();
                    }
                    objModel.ECFRaiser = row["ecfstatus_name"].ToString();
                    objModel.DocTypeName = row["doctype_name"].ToString();
                    objModel.DocSubTypeName = row["docsubtype_code"].ToString();
                    objModel.CreateMode = row["ecf_create_mode"].ToString();
                    objModel.ClaimMonth = row["ecf_claim_month"].ToString();
                    objModel.ECFAmount = cmnfun.GetINRAmount(row["ecf_amount"].ToString());
                    objModel.Despatchdate = row["ecf_despatch_date"].ToString();
                    objModel.CourierName = row["ecf_courier_name"].ToString();
                    objModel.AWBNo = row["ecf_no"].ToString();
                    objModel.ECFStatus = row["ecfstatus_name"].ToString();
                    objModel.CancelBy = row["ecf_cancel_by"].ToString();
                    objModel.CancelDate = row["ecf_cancel_date"].ToString();
                    emp.Add(objModel);
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            }
        }
        public IEnumerable<ECFDataModel> HoldDetails()
        {
            List<ECFDataModel> emp = new List<ECFDataModel>();
            try
            {

                ECFDataModel objModel;
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                string j = cmnfun.GetLoginUserGid().ToString();
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "HOLDREPORTSEARCH";
                cmd.Parameters.Add("@emplogin", SqlDbType.VarChar).Value = cmnfun.GetLoginUserGid().ToString();
                da = new SqlDataAdapter(cmd);
                //string holdrep = "SELECT t1.*,EmployeeName=(SELECT employee_name FROM iem_mst_temployee WHERE iem_mst_temployee.employee_gid=t1.ecf_raiser) FROM	(SELECT distinct ecfhold_by, ecf_gid,ecfhold_queue_gid,CONVERT(VARCHAR,ecf_date,105)AS ecf_date,ecf_supplier_employee,supplierheader_name,	employee_name,doctype_name,docsubtype_name,docsubtype_code,ecf_no,ecf_raiser,ecf_create_mode,CONVERT(VARCHAR,ecf_claim_month,105)AS ecf_claim_month,ecf_amount,CONVERT(VARCHAR,ecf_despatch_date,105)AS ecf_despatch_date,ecf_courier_name,ecf_awb_no,ecfstatus_name,ecf_cancel_by,CONVERT(VARCHAR,ecf_cancel_date,105)AS ecf_cancel_date FROM iem_trn_tecf LEFT JOIN iem_mst_tdoctype ON iem_trn_tecf.ecf_doctype_gid=doctype_gid LEFT JOIN iem_mst_tdocsubtype ON iem_trn_tecf.ecf_docsubtype_gid=docsubtype_gid LEFT JOIN iem_mst_temployee ON iem_mst_temployee.employee_gid=iem_trn_tecf.ecf_employee_gid LEFT JOIN asms_tmp_tsupplierheader ON asms_tmp_tsupplierheader.supplierheader_gid=iem_trn_tecf.ecf_supplier_gid LEFT JOIN Iem_mst_tecfstatus ON Iem_mst_tecfstatus.ecfstatus_gid=ecf_status LEFT JOIN iem_trn_tecfhold ON iem_trn_tecf.ecf_gid=iem_trn_tecfhold.ecfhold_ecf_gid LEFT JOIN iem_trn_tqueue AS e ON e.queue_ref_gid=iem_trn_tecf.ecf_gid WHERE ecf_isremoved='N' and ecfhold_by=" + cmnfun.GetLoginUserGid().ToString() + " )t1";           
                //GetConnection();
                //DataTable dt = new DataTable();
                //cmd = new SqlCommand(holdrep, con);
                //cmd.CommandType = CommandType.Text;
                //da = new SqlDataAdapter(cmd); 
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new ECFDataModel();
                    if (row["ecfhold_queue_gid"].ToString() != "")
                    {
                        objModel.QueueId = Convert.ToInt32(row["ecfhold_queue_gid"].ToString());
                    }
                    objModel.ECFId = Convert.ToInt32(row["ecf_gid"].ToString());
                    objModel.ECFNo = row["ecf_no"].ToString();
                    objModel.ECFDate = row["ecf_date"].ToString();
                    objModel.SupplierorEmployee = row["ecf_supplier_employee"].ToString();
                    if (objModel.SupplierorEmployee == "E")
                    {
                        objModel.SupplierorEmployeename = row["employee_name"].ToString();
                        objModel.ECFRaiser = row["EmployeeName"].ToString();
                    }
                    if (objModel.SupplierorEmployee == "S")
                    {
                        objModel.SupplierorEmployeename = row["supplierheader_name"].ToString();
                        objModel.ECFRaiser = row["EmployeeName"].ToString();
                    }
                    objModel.DocTypeName = row["doctype_name"].ToString();
                    objModel.DocSubTypeName = row["docsubtype_name"].ToString();
                    objModel.CreateMode = row["ecf_create_mode"].ToString();
                    objModel.ClaimMonth = row["ecf_claim_month"].ToString();
                    objModel.ECFAmount = cmnfun.GetINRAmount(row["ecf_amount"].ToString());
                    objModel.Despatchdate = row["ecf_despatch_date"].ToString();
                    objModel.CourierName = row["ecf_courier_name"].ToString();
                    objModel.AWBNo = row["ecf_no"].ToString();
                    objModel.ECFStatus = row["ecfstatus_name"].ToString();
                    objModel.CancelBy = row["ecf_cancel_by"].ToString();
                    objModel.CancelDate = row["ecf_cancel_date"].ToString();
                    emp.Add(objModel);
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            }
        }
        public string Update(int id, string remark)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ecf_gid", SqlDbType.Int).Value = id;
                cmd.Parameters.AddWithValue("@CANCELBY", SqlDbType.Int).Value = cmnfun.GetLoginUserGid();
                cmd.Parameters.AddWithValue("@action_remark", SqlDbType.VarChar).Value = remark;
                cmd.Parameters.AddWithValue("@ACTION", SqlDbType.VarChar).Value = "UPDATEHOLDRELEASE";
                result = cmd.ExecuteNonQuery().ToString();
                if (result == "4")
                {
                    result = "Successfully Released";
                }
                return result;
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
            //throw new NotImplementedException();
        }


        public IEnumerable<SelectDocSubType> SelectDocSubType(int gid)
        {
            List<SelectDocSubType> objCountrytype = new List<SelectDocSubType>();
            try
            {

                SelectDocSubType objModel;
                //GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                cmd.Parameters.AddWithValue("@ACTION", "DocSubType");
                cmd.Parameters.AddWithValue("@GID", gid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SelectDocSubType();
                    objModel.DocSubgid = Convert.ToInt32(dt.Rows[i]["docsubtype_gid"].ToString());
                    //objModel.doctype_code = Convert.ToString(dt.Rows[i]["doctype_code"].ToString());
                    objModel.DocSubname = Convert.ToString(dt.Rows[i]["docsubtype_name"].ToString());
                    objCountrytype.Add(objModel);
                }
                return objCountrytype;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objCountrytype;
            }
            finally
            {
                con.Close();
            }
        }


        public IEnumerable<SelectCourier> SelectCourier()
        {
            List<SelectCourier> objCountrytype = new List<SelectCourier>();
            try
            {

                SelectCourier objModel;
                //GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                cmd.Parameters.AddWithValue("@ACTION", "SelectCourier");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SelectCourier();
                    objModel.courierid = Convert.ToInt32(dt.Rows[i]["courier_gid"].ToString());
                    objModel.couriername = Convert.ToString(dt.Rows[i]["courier_name"].ToString());
                    objCountrytype.Add(objModel);
                }
                return objCountrytype;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objCountrytype;
            }
            finally
            {
                con.Close();
            }
        }


        public IEnumerable<ECFDataModel> DespatchSearch()
        {
            List<ECFDataModel> emp = new List<ECFDataModel>();
            try
            {

                ECFDataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                //cmd.Parameters.AddWithValue("@emplogin",cmnfun.GetLoginUserGid().ToString());

                cmd.Parameters.AddWithValue("@emplogin", cmnfun.GetLoginUserGid().ToString());
                cmd.Parameters.AddWithValue("@ACTION", "SELECTGRIDESPATCH");
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "CANCELLATIONSEARCH";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new ECFDataModel();
                    objModel.ECFId = Convert.ToInt32(row["ecf_gid"].ToString());
                    objModel.ECFNo = row["ecf_no"].ToString();
                    objModel.ECFDate = row["ecf_date"].ToString();
                    objModel.SupplierorEmployee = row["ecf_supplier_employee"].ToString();
                    if (objModel.SupplierorEmployee == "E")
                    {
                        objModel.SupplierorEmployeename = row["employee_name"].ToString();
                        objModel.ECFRaiser = row["EmployeeName"].ToString();
                        //objModel.ECFRaiser = row["employee_name"].ToString();
                    }
                    if (objModel.SupplierorEmployee == "S")
                    {
                        objModel.SupplierorEmployeename = row["supplierheader_name"].ToString();
                        objModel.ECFRaiser = row["EmployeeName"].ToString();
                        //objModel.ECFRaiser = row["employee_name"].ToString();
                    }
                    objModel.DocTypeName = row["doctype_code"].ToString();
                    objModel.DocSubTypeName = row["docsubtype_name"].ToString();
                    objModel.CreateMode = row["ecf_create_mode"].ToString();
                    objModel.ClaimMonth = row["ecf_claim_month"].ToString();
                    objModel.ECFAmount = cmnfun.GetINRAmount(row["ecf_amount"].ToString());
                    objModel.Despatchdate = row["ecf_despatch_date"].ToString();
                    objModel.CourierName = row["ecf_courier_name"].ToString();
                    objModel.AWBNo = row["ecf_awb_no"].ToString();
                    objModel.ECFStatus = row["ECF Status"].ToString();
                    objModel.CancelBy = row["ecf_cancel_by"].ToString();
                    objModel.CancelDate = row["ecf_cancel_date"].ToString();
                    emp.Add(objModel);
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            }
        }

        public IEnumerable<ECFDataModel> DespatchSearch(string EcfDateFrom, string EcfDateTo, string DocType, string DocSubType, string Code, string Name, string ECFClaimMonth, string suporemp, string ECFDespatchDateTo, string Suppliername, string ecfamount, string ecfnumber, string ECFStatus, string ecfmode)
        {
            List<ECFDataModel> emp = new List<ECFDataModel>();
            try
            {
                //if (ECFClaimMonth != null && ECFClaimMonth != "")
                //{
                //    ECFDataModel bat = new ECFDataModel();
                //    string[] arr = Regex.Split(ECFClaimMonth, "-");
                //    month = arr[1];
                //    string year = arr[2];
                //}

                ECFDataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@doctype_name", DocType);
                cmd.Parameters.AddWithValue("@docsubtype_name", DocSubType);
                cmd.Parameters.AddWithValue("@ecf_no", ecfnumber);
                cmd.Parameters.AddWithValue("@ecf_create_mode", ecfmode);
                cmd.Parameters.AddWithValue("@ecf_amount", ecfamount);
                if (suporemp == "E" && Name != "")
                {

                    cmd.Parameters.AddWithValue("@employee_name", Name);
                }
                else if (suporemp == "S" && Name != "")
                {
                    cmd.Parameters.AddWithValue("@supplierheader_name", Name);
                }
                //cmd.Parameters.AddWithValue("@supplierheader_name", Name);
                cmd.Parameters.AddWithValue("@ecf_supplier_employee", Suppliername);
                if (EcfDateFrom != "")
                {
                    cmd.Parameters.AddWithValue("@ecf_dateFrom", cmnfun.convertoDateTimeString(EcfDateFrom).ToString());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ecf_dateFrom", EcfDateFrom);
                }
                if (EcfDateTo != "")
                {
                    cmd.Parameters.AddWithValue("@ecf_dateTo", cmnfun.convertoDateTimeString(EcfDateTo).ToString());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ecf_dateTo", EcfDateTo);
                }
                if (ECFClaimMonth != "")
                {
                    string datefor = cmnfun.yyMMddStringfor(ECFClaimMonth).ToString();
                    string[] arr = Regex.Split(datefor, "-");
                    month = arr[1];
                    cmd.Parameters.AddWithValue("@ecf_claim_month", month);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ecf_claim_month", ECFClaimMonth);
                }
                // cmd.Parameters.AddWithValue("@ecf_dateTo", EcfDateTo);
                cmd.Parameters.AddWithValue("@emplogin", cmnfun.GetLoginUserGid().ToString());
                cmd.Parameters.AddWithValue("@ACTION", "DESPATCHSEARCHBYSam");
                cmd.CommandType = CommandType.StoredProcedure;

                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new ECFDataModel();
                    objModel.ECFId = Convert.ToInt32(row["ecf_gid"].ToString());
                    objModel.ECFNo = row["ecf_no"].ToString();
                    objModel.ECFDate = row["ecf_date"].ToString();
                    objModel.SupplierorEmployee = row["ecf_supplier_employee"].ToString();
                    if (objModel.SupplierorEmployee == "E")
                    {
                        objModel.SupplierorEmployeename = row["employee_name"].ToString();
                        objModel.ECFRaiser = row["EmployeeName"].ToString();
                        //objModel.ECFRaiser = row["employee_name"].ToString();
                    }
                    if (objModel.SupplierorEmployee == "S")
                    {
                        objModel.SupplierorEmployeename = row["supplierheader_name"].ToString();
                        objModel.ECFRaiser = row["EmployeeName"].ToString();
                        //objModel.ECFRaiser = row["employee_name"].ToString();
                    }
                    //objModel.ECFRaiser = row["ecfstatus_name"].ToString();
                    objModel.DocTypeName = row["doctype_code"].ToString();
                    objModel.DocSubTypeName = row["docsubtype_name"].ToString();
                    objModel.CreateMode = row["ecf_create_mode"].ToString();
                    objModel.ClaimMonth = row["ecf_claim_month"].ToString();
                    objModel.ECFAmount = cmnfun.GetINRAmount(row["ecf_amount"].ToString());
                    objModel.Despatchdate = row["ecf_despatch_date"].ToString();
                    objModel.CourierName = row["ecf_courier_name"].ToString();
                    //objModel.AWBNo = row["ecf_awb_no"].ToString();
                    objModel.ECFStatus = row["ECF Status"].ToString();
                    // objModel.CancelBy = row["ecf_cancel_by"].ToString();
                    //objModel.CancelDate = row["ecf_cancel_date"].ToString();
                    emp.Add(objModel);
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            }
        }


        public IEnumerable<ECFDataModel> DeletionSearch()
        {
            List<ECFDataModel> emp = new List<ECFDataModel>();
            try
            {

                ECFDataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                //cmd.Parameters.AddWithValue("@emplogin",cmnfun.GetLoginUserGid().ToString());

                cmd.Parameters.AddWithValue("@emplogin", cmnfun.GetLoginUserGid().ToString());
                cmd.Parameters.AddWithValue("@ACTION", "SELECTGRIDELETION");
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "CANCELLATIONSEARCH";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new ECFDataModel();
                    objModel.ECFId = Convert.ToInt32(row["ecf_gid"].ToString());
                    objModel.QueueId = Convert.ToInt32(row["queue_gid"].ToString());
                    objModel.ECFNo = row["ecf_no"].ToString();
                    objModel.ECFDate = row["ecf_date"].ToString();
                    objModel.SupplierorEmployee = row["ecf_supplier_employee"].ToString();
                    if (objModel.SupplierorEmployee == "E")
                    {
                        objModel.SupplierorEmployeename = row["employee_name"].ToString();
                        objModel.ECFRaiser = row["ecfraiser"].ToString();
                    }
                    else if (objModel.SupplierorEmployee == "S")
                    {
                        objModel.SupplierorEmployeename = row["supplierheader_name"].ToString();
                        objModel.ECFRaiser = row["ecfraiser"].ToString();
                    }
                    //objModel.DocTypeName = row["doctype_code"].ToString();
                    objModel.DocSubTypeName = row["docsubtype_name"].ToString();
                    objModel.CreateMode = row["ecf_create_mode"].ToString();
                    objModel.ClaimMonth = row["ecf_claim_month"].ToString();
                    //objModel.ECFAmount = row["ecf_amount"].ToString();
                    objModel.ECFAmount = cmnfun.GetINRAmount(row["ecf_amount"].ToString());
                    //objModel.Despatchdate = row["ecf_despatch_date"].ToString();
                    //objModel.CourierName = row["ecf_courier_name"].ToString();
                    //objModel.AWBNo = row["ecf_awb_no"].ToString();
                    objModel.ECFStatus = row["ECF Status"].ToString();
                    //objModel.CancelBy = row["ecf_cancel_by"].ToString();
                    //objModel.CancelDate = row["ecf_cancel_date"].ToString();
                    emp.Add(objModel);
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            }
        }

        public IEnumerable<ECFDataModel> DeletionSearch(string EcfDateFrom, string EcfDateTo, string DocType, string DocSubType, string Code, string Name, string ECFClaimMonth, string SupplierorEmployee, string ECFDespatchDateTo, string Suppliername, string ecfnumber, string ecfmode, string ECFStatus, string ecfamount)
        {
            List<ECFDataModel> emp = new List<ECFDataModel>();
            try
            {


                ECFDataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@doctype_name", DocType);
                //cmd.Parameters.AddWithValue("@docsubtype_name", DocSubType);
                //cmd.Parameters.AddWithValue("@ecf_supplier_employee", Suppliername);
                //cmd.Parameters.AddWithValue("@ecf_no", ecfnumber);
                //cmd.Parameters.AddWithValue("@ecf_amount", ecfamount);
                //cmd.Parameters.AddWithValue("@ecf_create_mode", ecfmode);

                //if (SupplierorEmployee == "E" && Name != "")
                //{

                //    cmd.Parameters.AddWithValue("@employee_name", Name);
                //}
                //else if (SupplierorEmployee == "S" && Name != "")
                //{
                //    cmd.Parameters.AddWithValue("@supplierheader_name", Name);
                //}

                cmd.Parameters.AddWithValue("@doctype_name", DocType);
                cmd.Parameters.AddWithValue("@docsubtype_name", DocSubType);
                cmd.Parameters.AddWithValue("@ecf_amount", ecfamount);
                cmd.Parameters.AddWithValue("@ecf_create_mode", ecfmode);
                cmd.Parameters.AddWithValue("@ecf_no", ecfnumber);

                if (SupplierorEmployee == "E" && Name != "")
                {

                    cmd.Parameters.AddWithValue("@employee_name", Name);
                }
                else if (SupplierorEmployee == "S" && Name != "")
                {
                    cmd.Parameters.AddWithValue("@supplierheader_name", Name);
                }

                // cmd.Parameters.AddWithValue("@employee_name",Name);
                // cmd.Parameters.AddWithValue("@supplierheader_name", Name);
                cmd.Parameters.AddWithValue("@ecf_supplier_employee", SupplierorEmployee);

                if (EcfDateFrom != "")
                {
                    cmd.Parameters.AddWithValue("@ecf_dateFrom", cmnfun.convertoDateTimeString(EcfDateFrom).ToString());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ecf_dateFrom", EcfDateFrom);
                }
                if (EcfDateTo != "")
                {
                    cmd.Parameters.AddWithValue("@ecf_dateTo", cmnfun.convertoDateTimeString(EcfDateTo).ToString());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ecf_dateTo", EcfDateTo);
                }
                if (ECFClaimMonth != "")
                {
                    string datefor = cmnfun.yyMMddStringfor(ECFClaimMonth).ToString();
                    string[] arr = Regex.Split(datefor, "-");
                    month = arr[1];
                    cmd.Parameters.AddWithValue("@ecf_claim_month", month);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ecf_claim_month", ECFClaimMonth);
                }
                // cmd.Parameters.AddWithValue("@ecf_dateTo", EcfDateTo);
                cmd.Parameters.AddWithValue("@emplogin", cmnfun.GetLoginUserGid().ToString());
                cmd.Parameters.AddWithValue("@ACTION", "DELETIONSEARCHBYSam");
                cmd.CommandType = CommandType.StoredProcedure;
                // cmd.CommandType = CommandType.StoredProcedure;               
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new ECFDataModel();
                    objModel.ECFId = Convert.ToInt32(row["ecf_gid"].ToString());
                    objModel.QueueId = Convert.ToInt32(row["queue_gid"].ToString());
                    objModel.ECFNo = row["ecf_no"].ToString();
                    objModel.ECFDate = row["ecf_date"].ToString();
                    objModel.SupplierorEmployee = row["ecf_supplier_employee"].ToString();
                    if (objModel.SupplierorEmployee == "E")
                    {
                        objModel.SupplierorEmployeename = row["employee_name"].ToString();
                        objModel.ECFRaiser = row["EmployeeName"].ToString();
                        //objModel.ECFRaiser = row["employee_name"].ToString();
                    }
                    if (objModel.SupplierorEmployee == "S")
                    {
                        objModel.SupplierorEmployeename = row["supplierheader_name"].ToString();
                        objModel.ECFRaiser = row["EmployeeName"].ToString();
                        // objModel.ECFRaiser = row["employee_name"].ToString();
                    }
                    objModel.ECFRaiser = row["ecfstatus_name"].ToString();
                    objModel.DocTypeName = row["doctype_code"].ToString();
                    objModel.DocSubTypeName = row["docsubtype_name"].ToString();
                    objModel.CreateMode = row["ecf_create_mode"].ToString();
                    objModel.ClaimMonth = row["ecf_claim_month"].ToString();
                    objModel.ECFAmount = cmnfun.GetINRAmount(row["ecf_amount"].ToString());
                    objModel.Despatchdate = row["ecf_despatch_date"].ToString();
                    objModel.CourierName = row["ecf_courier_name"].ToString();
                    objModel.AWBNo = row["ecf_awb_no"].ToString();
                    objModel.ECFStatus = row["ECF Status"].ToString();
                    objModel.CancelBy = row["ecf_cancel_by"].ToString();
                    objModel.CancelDate = row["ecf_cancel_date"].ToString();
                    emp.Add(objModel);
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            }
        }


        public DataTable getdoctypecode(int id)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                cmd = new SqlCommand("select doctype_code from iem_mst_tdoctype where doctype_gid='" + id + "'", con);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                //return dt;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return dt;
            }
            finally
            {
                con.Close();
            }
            return dt;
        }
        public DataTable getdocsubtypecode(int id)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                cmd = new SqlCommand("select docsubtype_name from iem_mst_tdocsubtype where docsubtype_gid='" + id + "'", con);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                //return dt;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return dt;
            }
            finally
            {
                con.Close();
            }
            return dt;
        }

        public DataTable getecfnumbergid(string ecfno)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                cmd = new SqlCommand("select ecf_gid from iem_trn_tecf where ecf_no= '" + ecfno + "' and  ecf_isremoved='N'", con);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                //return dt;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return dt;
            }
            finally
            {
                con.Close();
            }
            return dt;
        }


        public IEnumerable<ECFDataModel> SelectDocsubType()
        {
            List<ECFDataModel> emp = new List<ECFDataModel>();
            try
            {

                ECFDataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "Dummyfunction";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new ECFDataModel();
                    objModel.Docgid = Convert.ToInt32(row["doctype_gid"].ToString());
                    objModel.Docname = row["doctype_code"].ToString();
                    emp.Add(objModel);
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            }
        }
        public IEnumerable<ECFDataModel> ReportSearch()
        {
            List<ECFDataModel> emp = new List<ECFDataModel>();
            try
            {

                ECFDataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "REPORT";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string print = "No";
                        objModel = new ECFDataModel();
                        if (row["QUEUE_GID"].ToString() != "")
                        {
                            objModel.QueueId = Convert.ToInt32(row["QUEUE_GID"].ToString());
                        }

                        objModel.ECFId = Convert.ToInt32(row["ecf_gid"].ToString());
                        objModel.ECFNo = row["ecf_no"].ToString();
                        objModel.ECFDate = row["ecf_date"].ToString();
                        objModel.SupplierorEmployee = row["ecf_supplier_employee"].ToString();
                        if (objModel.SupplierorEmployee == "E" || objModel.SupplierorEmployee == "P")
                        {
                            objModel.SupplierorEmployeename = row["EmployeeName"].ToString();
                            objModel.ECFRaiser = row["Raiser"].ToString();
                        }
                        if (objModel.SupplierorEmployee == "S")
                        {
                            objModel.SupplierorEmployeename = row["supplierheader_name"].ToString();
                            objModel.ECFRaiser = row["Raiser"].ToString();
                        }
                        objModel.DocTypeName = row["doctype_code"].ToString();
                        objModel.DocSubTypeName = row["docsubtype_name"].ToString();
                        objModel.CreateMode = row["ecf_create_mode"].ToString();
                        objModel.ClaimMonth = row["ecf_claim_month"].ToString();
                        objModel.ECFAmount = cmnfun.GetINRAmount(row["ecf_amount"].ToString());
                        objModel.Despatchdate = row["ecf_despatch_date"].ToString();
                        objModel.CourierName = row["ecf_courier_name"].ToString();
                        objModel.AWBNo = row["ecf_no"].ToString();
                        objModel.ECFStatus = row["ecfstatus_name"].ToString();
                        objModel.CancelBy = row["ecf_cancel_by"].ToString();
                        objModel.CancelDate = row["ecf_cancel_date"].ToString();
                        objModel.CurrentuserApprovestatus = cmnfun.GetQueueStatusHistry(row["queue_action_status"].ToString());
                        if (row["ecf_printflag"].ToString() == "Y")
                        {
                            print = "Yes";
                        }
                        objModel.Reason = print;
                        emp.Add(objModel);
                    }
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            }
        }
        DataTable ECFRepository.DespatchIndexExportExcel()
        {
            try
            {
                cmd = new SqlCommand("pr_report", con);
                cmd.Parameters.AddWithValue("@emplogin", cmnfun.GetLoginUserGid().ToString());
                cmd.Parameters.AddWithValue("@OPERATION", "DESPATCHINDEXEXPORT");
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
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
            }
        }
        public DataTable DespatchReportExportExcel()
        {
            try
            {
                cmd = new SqlCommand("pr_report", con);
                cmd.Parameters.AddWithValue("@emplogin", cmnfun.GetLoginUserGid().ToString());
                cmd.Parameters.AddWithValue("@OPERATION", "DESPATCHREPORTEXPORT");
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
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
            }
        }
        public DataTable POWOReportExportExcel()
        {
            try
            {
                cmd = new SqlCommand("pr_report", con);
                cmd.Parameters.AddWithValue("@emplogin", cmnfun.GetLoginUserGid().ToString());
                cmd.Parameters.AddWithValue("@OPERATION", "POWOBASEDREPOTEXPORT");
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
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
            }
        }
        public IEnumerable<ECFDataModel> myapprovedlistSearch()
        {
            List<ECFDataModel> emp = new List<ECFDataModel>();
            try
            {

                ECFDataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emplogin", cmnfun.GetLoginUserGid().ToString());
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "approvedlist";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.Add("Queue Action Status Name", typeof(System.String));
                    foreach (DataRow row in dt.Rows)
                    {
                        objModel = new ECFDataModel();
                        if (row["QUEUE_GID"].ToString() != "")
                        {
                            objModel.QueueId = Convert.ToInt32(row["QUEUE_GID"].ToString());
                        }

                        objModel.ECFId = Convert.ToInt32(row["ECFGid"].ToString());
                        objModel.ECFNo = row["ECF NO"].ToString();
                        objModel.ECFDate = row["ECF Date"].ToString();
                        objModel.SupplierorEmployee = row["Supplier/Employee Flag"].ToString();
                        if (objModel.SupplierorEmployee == "E" || objModel.SupplierorEmployee == "P")
                        {
                            objModel.SupplierorEmployeename = row["Employee Name"].ToString();
                            objModel.ECFRaiser = row["Raiser Name"].ToString();
                        }
                        if (objModel.SupplierorEmployee == "S")
                        {
                            objModel.SupplierorEmployeename = row["Supplier Name"].ToString();
                            objModel.ECFRaiser = row["Raiser Name"].ToString();
                        }

                        objModel.DocSubTypeName = row["DOC Type"].ToString();
                        //objModel.CreateMode = row["ecf_create_mode"].ToString();
                        ///objModel.ClaimMonth = row["ecf_claim_month"].ToString();
                        objModel.ECFAmount = cmnfun.GetINRAmount(row["ECF Amount"].ToString());
                        row["Queue Action Status Name"] = cmnfun.GetQueueStatusHistry(row["Queue Action Status"].ToString());
                        objModel.DocTypeName = cmnfun.GetQueueStatusHistry(row["Queue Action Status"].ToString());
                        objModel.Despatchdate = row["Queue Action Remark"].ToString();
                        //objModel.CourierName = row["Raiser Name"].ToString();
                        //objModel.AWBNo = row["ecf_no"].ToString();
                        objModel.ECFStatus = row["ECF Status"].ToString();
                        //objModel.CancelBy = row["ecf_cancel_by"].ToString();
                        //objModel.CancelDate = row["ecf_cancel_date"].ToString();
                        objModel.Reason = row["Queue Action Date"].ToString();
                        emp.Add(objModel);
                    }
                }
                return emp;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            }
        }
        public DataTable myapprovedlistSearchexcel()
        {
            dt = new DataTable();
            try
            {
                cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emplogin", cmnfun.GetLoginUserGid().ToString());
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "approvedlist";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    dt.Columns.Add("Queue Action Status Name", typeof(System.String));
                    foreach (DataRow row in dt.Rows)
                    {
                        row["Queue Action Status Name"] = cmnfun.GetQueueStatusHistry(row["Queue Action Status"].ToString());
                    }
                }
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
            }
        }
        public IEnumerable<ECFDataModel> EcfReport(string EcfDateFrom = null, string EcfDateTo = null, string EcfNo = null, string EcfAmount = null, string InvoiceDateFrom = null, string InvoiceDateTo = null, string InvoiceNo = null, string InvoiceAmount = null, string EcfMode = null, string Ecfstatus = null,string suppliercode = null,string suppliername = null, string command = null)
        {
            List<ECFDataModel> emp = new List<ECFDataModel>();
            try
            {
                ECFDataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ecf_no", EcfNo);
                cmd.Parameters.AddWithValue("@ecf_amount", EcfAmount);
                cmd.Parameters.AddWithValue("@invoiceno", InvoiceNo);
                cmd.Parameters.AddWithValue("@invoiceamount", InvoiceAmount);
                cmd.Parameters.AddWithValue("@ecf_create_mode", EcfMode);
                cmd.Parameters.AddWithValue("@ecfstatus_name", Ecfstatus);
                cmd.Parameters.AddWithValue("@supplierheader_code", suppliercode);
                cmd.Parameters.AddWithValue("@supplierheader_name", suppliername);

                if (InvoiceDateFrom != "")
                {
                    cmd.Parameters.AddWithValue("@invoicefromdate", cmnfun.convertoDateTimeString(InvoiceDateFrom).ToString());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@invoicefromdate", InvoiceDateFrom);
                }

                if (InvoiceDateTo != "")
                {
                    cmd.Parameters.AddWithValue("@invoicetodate", cmnfun.convertoDateTimeString(InvoiceDateTo).ToString());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@invoicetodate", InvoiceDateTo);
                }
               

                if (EcfDateFrom != "")
                {
                    cmd.Parameters.AddWithValue("@ecf_dateFrom", cmnfun.convertoDateTimeString(EcfDateFrom).ToString());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ecf_dateFrom", EcfDateFrom);
                }

                if (EcfDateTo != "")
                {
                    cmd.Parameters.AddWithValue("@ecf_dateTo", cmnfun.convertoDateTimeString(EcfDateTo).ToString());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ecf_dateTo", EcfDateTo);
                }
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "QUERYEXPORT";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        objModel = new ECFDataModel();
                        objModel.ECFNo = row["ECF Ref No"].ToString();
                        objModel.ECFDate = row["ECF Date"].ToString();
                        objModel.SupplierorEmployee = row["Vendor Name"].ToString();
                        objModel.InvoiceNo = row["Invoice No"].ToString();
                        objModel.InvoiceDate = row["Invoice Date"].ToString();
                        objModel.InvoiceAmt = row["Invoice Amt"].ToString();
                        objModel.OracleLocationCode = row["Oracle Location Code"].ToString();
                        objModel.ECFAmount = cmnfun.GetINRAmount(row["ECF Amount"].ToString());
                        objModel.BusinessSegment = row["Business Segment"].ToString();
                        objModel.CostCentre = row["Cost Centre"].ToString();
                        objModel.ProductCode = row["Product Code"].ToString();
                        objModel.GLcode = row["GL code"].ToString();
                        objModel.WorkorderNo = row["Work order No"].ToString();
                        objModel.Category = row["Category"].ToString();
                        objModel.SubCategory = row["Sub Category"].ToString();
                        objModel.DebitLineAmount = row["Debit Line Amount"].ToString();
                        objModel.ECFdescription = row["ECF description"].ToString();
                        objModel.FinalStatus = row["Final Status"].ToString();
                        objModel.InvoiceDescription = row["Invoice Description"].ToString();
                        objModel.PayMode = row["Pay Mode"].ToString();
                        objModel.ExpenseNature = row["Expense Nature"].ToString();
                        objModel.ECFdescription = row["ECF description"].ToString();
                        objModel.RaiserCode = row["Raiser Code"].ToString();
                        objModel.RaiserDepartment = row["Raiser Department"].ToString();
                        objModel.ECFStatus = row["Ecf Status"].ToString();
                        objModel.CapitalizationDate = row["Capitalization Date"].ToString();
                        objModel.TAT = row["TAT"].ToString();

                        objModel.NetAmount = row["Net Amount"].ToString();
                        objModel.PaidDate = row["Paid Date"].ToString();
                        objModel.ServiceTax = row["Service Tax"].ToString();
                        objModel.ValueAddedTax = row["Value Added Tax"].ToString();
                        objModel.TaxDeductedatSource = row["Tax Deducted at Source"].ToString();
                        objModel.WorksContractTax = row["Works Contract Tax"].ToString();
                        objModel.Prepaid = row["Prepaid"].ToString();
                        objModel.Suspense = row["Suspense"].ToString();
                        objModel.Creditnote = row["Credit note"].ToString();
                        objModel.Document = row["Document"].ToString();
                        objModel.InvoiceServiceMonth = row["Invoice Service Month"].ToString();
                        objModel.EcfDespatchDate = row["Ecf Despatch Date"].ToString();
                        emp.Add(objModel);
                    }
                }
                return emp;
            }   
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return emp;
            }
            finally
            {
                con.Close();
            }
        }
        
        //public DataTable EcfReport()
        //{
        //    List<ECFDataModel> emp = new List<ECFDataModel>();
        //    try
        //    {
        //        //ECFDataModel objModel;
        //        cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "ECFReportExcelExport";
        //        da = new SqlDataAdapter(cmd);
        //        dt = new DataTable();
        //        da.Fill(dt);
        //        //if (dt.Rows.Count > 0)
        //        //{
        //        //    foreach (DataRow row in dt.Rows)
        //        //    {
        //        //        objModel = new ECFDataModel();
        //        //        objModel.ECFNo = row["ECF Ref No"].ToString();
        //        //        objModel.ECFDate = row["ECF Date"].ToString();
        //        //        objModel.SupplierorEmployee = row["Vendor Name"].ToString();
        //        //        objModel.InvoiceNo = row["Invoice No"].ToString();
        //        //        objModel.InvoiceDate = row["Invoice Date"].ToString();
        //        //        objModel.InvoiceAmt = row["Invoice Amt"].ToString();
        //        //        objModel.OracleLocationCode = row["Oracle Location Code"].ToString();
        //        //        objModel.ECFAmount = cmnfun.GetINRAmount(row["ECF Amount"].ToString());
        //        //        objModel.BusinessSegment = row["Business Segment"].ToString();
        //        //        objModel.CostCentre = row["Cost Centre"].ToString();
        //        //        objModel.ProductCode = row["Product Code"].ToString();
        //        //        objModel.GLcode = row["GL code"].ToString();
        //        //        objModel.WorkorderNo = row["Work order No"].ToString();
        //        //        objModel.Category = row["Category"].ToString();
        //        //        objModel.SubCategory = row["Sub Category"].ToString();
        //        //        objModel.DebitLineAmount = row["Debit Line Amount"].ToString();
        //        //        objModel.ECFdescription = row["ECF description"].ToString();
        //        //        objModel.FinalStatus = row["Final Status"].ToString();
        //        //        objModel.InvoiceDescription = row["Invoice Description"].ToString();
        //        //        objModel.PayMode = row["Pay Mode"].ToString();
        //        //        objModel.ExpenseNature = row["Expense Nature"].ToString();
        //        //        objModel.ECFdescription = row["ECF description"].ToString();
        //        //        objModel.RaiserCode = row["Raiser Code"].ToString();
        //        //        objModel.RaiserDepartment = row["Raiser Department"].ToString();
        //        //        objModel.ECFStatus = row["Ecf Status"].ToString();
        //        //        objModel.CapitalizationDate = row["Capitalization Date"].ToString();
        //        //        objModel.TAT = row["TAT"].ToString();

        //        //        objModel.NetAmount = row["Net Amount"].ToString();
        //        //        objModel.PaidDate = row["Paid Date"].ToString();
        //        //        objModel.ServiceTax = row["Service Tax"].ToString();
        //        //        objModel.ValueAddedTax = row["Value Added Tax"].ToString();
        //        //        objModel.TaxDeductedatSource = row["Tax Deducted at Source"].ToString();
        //        //        objModel.WorksContractTax = row["Works Contract Tax"].ToString();
        //        //        objModel.Prepaid = row["Prepaid"].ToString();
        //        //        objModel.Suspense = row["Suspense"].ToString();
        //        //        objModel.Creditnote = row["Credit note"].ToString();

        //        //        emp.Add(objModel);
        //        //    }
        //        //}
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //        return dt;
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //}
        //correction by dhasarathan
        public DataTable EcfReport(string EcfDateFrom = null, string EcfDateTo = null, string DocType = null, string docsubtype = null, string Code = null, string Name = null, string ECFClaimMonth = null, string queryempsup = null, string ECFDespatchDateTo = null, string ecfnumber = null, string ecfamount = null, string Ecfstatus = null, string ECFMode = null, string command = null, string Suppliercode = null, string Suppliername = null)
        {

            try
            {

                cmd = new SqlCommand("pr_eow_trn_getECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@doctype_name", DocType);
                cmd.Parameters.AddWithValue("@docsubtype_name", docsubtype);
                cmd.Parameters.AddWithValue("@ecfstatus_name", Ecfstatus);
                cmd.Parameters.AddWithValue("@ecf_no", ecfnumber);
                cmd.Parameters.AddWithValue("@ecf_amount", ecfamount);
                cmd.Parameters.AddWithValue("@ecf_create_mode", ECFMode);
                cmd.Parameters.AddWithValue("@employee_code", Code);
                cmd.Parameters.AddWithValue("@supplierheader_code", Suppliercode);


                if (Name != "")
                {

                    cmd.Parameters.AddWithValue("@employee_name", Name);
                }
                else if (Suppliername != "")
                {
                    cmd.Parameters.AddWithValue("@supplierheader_name", Suppliername);
                }


                cmd.Parameters.AddWithValue("@ecf_supplier_employee", queryempsup);
                // cmd.Parameters.AddWithValue("@ecf_courier_name", ECFStatus);
                if (EcfDateFrom != "")
                {
                    cmd.Parameters.AddWithValue("@ecf_dateFrom", cmnfun.convertoDateTimeString(EcfDateFrom).ToString());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ecf_dateFrom", EcfDateFrom);
                }
                if (EcfDateTo != "")
                {
                    cmd.Parameters.AddWithValue("@ecf_dateTo", cmnfun.convertoDateTimeString(EcfDateTo).ToString());
                }
                else
                {
                    cmd.Parameters.AddWithValue("@ecf_dateTo", EcfDateTo);
                }
                if (ECFClaimMonth != "")
                {
                    //string datefor = cmnfun.yyMMddStringfor(ECFClaimMonth).ToString();
                    //string[] arr = Regex.Split(datefor, "-");
                    //month = arr[1];
                    //cmd.Parameters.AddWithValue("@ecf_claim_month", month);

                    cmd.Parameters.AddWithValue("@ecf_claim_month", ECFClaimMonth);
                }

                // cmd.Parameters.AddWithValue("@ecf_claim_month",month);
                // cmd.Parameters.AddWithValue("@ecf_dateTo", EcfDateTo);
                //cmd.Parameters.AddWithValue("@emplogin", cmnfun.GetLoginUserGid().ToString());
                //cmd.Parameters.AddWithValue("@ACTION", "QuerysearchSam");
                DataTable dt = new DataTable();
                cmd.CommandTimeout = 0;
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
            }
        }

    }
}