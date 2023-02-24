using IEM.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using IEM.Helper;

namespace IEM.Areas.FLEXIBUY.Models
{
    public class PRNewDataModel : PRNewRepository
    {
        SqlCommand cmd;
        SqlConnection con = new SqlConnection();
        SqlDataAdapter da;
        ErrorLog objErrorLog = new ErrorLog();
        CommonIUD objCommonIUD = new CommonIUD();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        public void getconnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }
        public DataSet GetPRHeaderDetails()
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_prnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getprdetails";
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
        public DataSet GetUOM()
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_prnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getuom";
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
        public int AddPRParentDetailsList(PRNewEntity PRParentDetailsInsert)
        {
            try
            {
                string prheadergid = "";
                int result = 0;

                string[,] codes2 = new string[,]
                {
                    {"prheader_refno",  objCmnFunctions.GetSequenceNoFb("PR", "", PRParentDetailsInsert.RequestForName)},
                    {"prheader_date","sysdatetime()"},
                    {"prheader_rasiergid",objCmnFunctions.GetLoginUserGid().ToString()},
                    {"prheader_fccc", string.IsNullOrEmpty(PRParentDetailsInsert.PRFCCCGid.ToString())?"0":PRParentDetailsInsert.PRFCCCGid.ToString()},
                    {"prheader_branchgid", string.IsNullOrEmpty(PRParentDetailsInsert.PRBranchGid.ToString())?"0":PRParentDetailsInsert.PRBranchGid.ToString()},
                    {"prheader_branchsingle",string.IsNullOrEmpty(PRParentDetailsInsert.PRBranchType)?"":PRParentDetailsInsert.PRBranchType},
                    {"prheader_Expensetype",string.IsNullOrEmpty(PRParentDetailsInsert.PRExpenseFlag)?"":PRParentDetailsInsert.PRExpenseFlag},
                    {"prheader_requestforgid",string.IsNullOrEmpty(PRParentDetailsInsert.RequestForValue.ToString())?"0":PRParentDetailsInsert.RequestForValue.ToString()},
                    {"prheader_status","1"},
                    {"prheader_iscancelled","N"},
                    {"prheader_locked","N"},
                    {"prheader_pramount","0"},
                    {"prheader_desc",string.IsNullOrEmpty(PRParentDetailsInsert.PRDescription)?"":PRParentDetailsInsert.PRDescription},
                    {"prheader_remarks",string.IsNullOrEmpty(PRParentDetailsInsert.PRDescription)?"":PRParentDetailsInsert.PRDescription},
                    {"prheader_curapprstage","1"},
                    {"prheader_isremoved","N"}
                };
                string tbname = "fb_trn_tprheader";
                string query_result = objCommonIUD.InsertCommon(codes2, tbname);

                if (query_result.ToLower() == "success")
                {

                    getconnection();
                    cmd = new SqlCommand("pr_fb_trn_prnew", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getprheadergid";
                    prheadergid = Convert.ToString(cmd.ExecuteScalar());

                    string[,] codes = new string[,] 
                {
                    {"prdetails_prheader_gid", string.IsNullOrEmpty(prheadergid)?"0":prheadergid},
                    {"prdetails_prodservicegid", string.IsNullOrEmpty(PRParentDetailsInsert.ChildProductGid.ToString())?"0":PRParentDetailsInsert.ChildProductGid.ToString()},
                    {"prdetails_prodservicedesc", string.IsNullOrEmpty(PRParentDetailsInsert.PRDetailDescription)?"":PRParentDetailsInsert.PRDetailDescription},
                    {"prdetails_uom_code", string.IsNullOrEmpty(PRParentDetailsInsert.PRDetailUOM.ToString())?"0":PRParentDetailsInsert.PRDetailUOM.ToString()},
                    {"prdetails_qty", string.IsNullOrEmpty(PRParentDetailsInsert.PRDetailQty)?"0":PRParentDetailsInsert.PRDetailQty},
                    {"pipinput_rate",string.IsNullOrEmpty(PRParentDetailsInsert.PRDetailUnitAmount)?"0":PRParentDetailsInsert.PRDetailUnitAmount},
                    {"pipinput_costestimation",string.IsNullOrEmpty(PRParentDetailsInsert.PRDetailTotalAmount)?"0":PRParentDetailsInsert.PRDetailTotalAmount},
                    {"prdetails_prodservgrp_gid",string.IsNullOrEmpty(PRParentDetailsInsert.ChildProductGroupGid.ToString())?"0":PRParentDetailsInsert.ChildProductGroupGid.ToString()},
                    {"prdetails_isremoved","N"}
                };
                    string tbname1 = "fb_trn_tprdetails";
                    string query_result1 = objCommonIUD.InsertCommon(codes, tbname1);
                }

                result = Convert.ToInt32(string.IsNullOrEmpty(prheadergid) ? "0" : prheadergid);

                getconnection();
                cmd = new SqlCommand("pr_fb_trn_prnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prgid", SqlDbType.Int).Value = result;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updateprtotalamnt";
                cmd.ExecuteNonQuery();

                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                con.Close();
            }
        }
        public DataSet GetPRDetailsAll(int prgid = 0)
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_prnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prgid", SqlDbType.Int).Value = prgid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getprdetailsall";
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
        public DataSet GetPRDetailParent(int prdetgid = 0)
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_prnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prgid", SqlDbType.Int).Value = prdetgid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getprdetailsbyid";
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
        public int AddPRParentDetailsListNew(PRNewEntity PRParentDetailsInsert)
        {
            try
            {
                string prheadergid = "";
                int result = 0;

                string[,] codes2 = new string[,]
                {
                    {"prheader_fccc", string.IsNullOrEmpty(PRParentDetailsInsert.PRFCCCGid.ToString())?"0":PRParentDetailsInsert.PRFCCCGid.ToString()},
                    {"prheader_branchgid", string.IsNullOrEmpty(PRParentDetailsInsert.PRBranchGid.ToString())?"0":PRParentDetailsInsert.PRBranchGid.ToString()},
                    {"prheader_branchsingle",string.IsNullOrEmpty(PRParentDetailsInsert.PRBranchType)?"":PRParentDetailsInsert.PRBranchType},
                    {"prheader_Expensetype",string.IsNullOrEmpty(PRParentDetailsInsert.PRExpenseFlag)?"":PRParentDetailsInsert.PRExpenseFlag},
                    {"prheader_requestforgid",string.IsNullOrEmpty(PRParentDetailsInsert.RequestForValue.ToString())?"0":PRParentDetailsInsert.RequestForValue.ToString()},
                    {"prheader_desc",string.IsNullOrEmpty(PRParentDetailsInsert.PRDescription)?"":PRParentDetailsInsert.PRDescription},
                    {"prheader_remarks",string.IsNullOrEmpty(PRParentDetailsInsert.PRDescription)?"":PRParentDetailsInsert.PRDescription},
                };
                string[,] whrcol = new string[,]
	            {
                    {"prheader_gid", string.IsNullOrEmpty(PRParentDetailsInsert.PRGid.ToString())?"0":PRParentDetailsInsert.PRGid.ToString()}
                };
                string tbname = "fb_trn_tprheader";
                string query_result = objCommonIUD.UpdateCommon(codes2, whrcol, tbname);

                prheadergid = string.IsNullOrEmpty(PRParentDetailsInsert.PRGid.ToString()) ? "0" : PRParentDetailsInsert.PRGid.ToString();

                if (query_result.ToLower() == "success")
                {
                    string[,] codes = new string[,] 
                {
                    {"prdetails_prheader_gid", string.IsNullOrEmpty(prheadergid)?"0":prheadergid},
                    {"prdetails_prodservicegid", string.IsNullOrEmpty(PRParentDetailsInsert.ChildProductGid.ToString())?"0":PRParentDetailsInsert.ChildProductGid.ToString()},
                    {"prdetails_prodservicedesc", string.IsNullOrEmpty(PRParentDetailsInsert.PRDetailDescription)?"":PRParentDetailsInsert.PRDetailDescription},
                    {"prdetails_uom_code", string.IsNullOrEmpty(PRParentDetailsInsert.PRDetailUOM.ToString())?"0":PRParentDetailsInsert.PRDetailUOM.ToString()},
                    {"prdetails_qty", string.IsNullOrEmpty(PRParentDetailsInsert.PRDetailQty)?"0":PRParentDetailsInsert.PRDetailQty},
                    {"pipinput_rate",string.IsNullOrEmpty(PRParentDetailsInsert.PRDetailUnitAmount)?"0":PRParentDetailsInsert.PRDetailUnitAmount},
                    {"pipinput_costestimation",string.IsNullOrEmpty(PRParentDetailsInsert.PRDetailTotalAmount)?"0":PRParentDetailsInsert.PRDetailTotalAmount},
                    {"prdetails_prodservgrp_gid",string.IsNullOrEmpty(PRParentDetailsInsert.ChildProductGroupGid.ToString())?"0":PRParentDetailsInsert.ChildProductGroupGid.ToString()},
                    {"prdetails_isremoved","N"}
                };
                    string tbname1 = "fb_trn_tprdetails";
                    string query_result1 = objCommonIUD.InsertCommon(codes, tbname1);
                }

                result = Convert.ToInt32(string.IsNullOrEmpty(prheadergid) ? "0" : prheadergid);

                getconnection();
                cmd = new SqlCommand("pr_fb_trn_prnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prgid", SqlDbType.Int).Value = result;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updateprtotalamnt";
                cmd.ExecuteNonQuery();

                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                con.Close();
            }
        }
        public int UpdatePRParentList(PRNewEntity PRParentDetailsInsert)
        {
            try
            {
                string prheadergid = "";
                int result = 0;

                string[,] codes2 = new string[,]
                {
                    {"prheader_fccc", string.IsNullOrEmpty(PRParentDetailsInsert.PRFCCCGid.ToString())?"0":PRParentDetailsInsert.PRFCCCGid.ToString()},
                    {"prheader_branchgid", string.IsNullOrEmpty(PRParentDetailsInsert.PRBranchGid.ToString())?"0":PRParentDetailsInsert.PRBranchGid.ToString()},
                    {"prheader_branchsingle",string.IsNullOrEmpty(PRParentDetailsInsert.PRBranchType)?"":PRParentDetailsInsert.PRBranchType},
                    {"prheader_Expensetype",string.IsNullOrEmpty(PRParentDetailsInsert.PRExpenseFlag)?"":PRParentDetailsInsert.PRExpenseFlag},
                    {"prheader_requestforgid",string.IsNullOrEmpty(PRParentDetailsInsert.RequestForValue.ToString())?"0":PRParentDetailsInsert.RequestForValue.ToString()},
                    {"prheader_desc",string.IsNullOrEmpty(PRParentDetailsInsert.PRDescription)?"":PRParentDetailsInsert.PRDescription},
                    {"prheader_remarks",string.IsNullOrEmpty(PRParentDetailsInsert.PRDescription)?"":PRParentDetailsInsert.PRDescription},
                };
                string[,] whrcol = new string[,]
	            {
                    {"prheader_gid", string.IsNullOrEmpty(PRParentDetailsInsert.PRGid.ToString())?"0":PRParentDetailsInsert.PRGid.ToString()}
                };
                string tbname = "fb_trn_tprheader";
                string query_result = objCommonIUD.UpdateCommon(codes2, whrcol, tbname);

                prheadergid = string.IsNullOrEmpty(PRParentDetailsInsert.PRGid.ToString()) ? "0" : PRParentDetailsInsert.PRGid.ToString();

                if (query_result.ToLower() == "success")
                {
                    string[,] codes = new string[,] 
                    {
                        {"prdetails_prheader_gid", string.IsNullOrEmpty(prheadergid)?"0":prheadergid},
                        {"prdetails_prodservicegid", string.IsNullOrEmpty(PRParentDetailsInsert.ChildProductGid.ToString())?"0":PRParentDetailsInsert.ChildProductGid.ToString()},
                        {"prdetails_prodservicedesc", string.IsNullOrEmpty(PRParentDetailsInsert.PRDetailDescription)?"":PRParentDetailsInsert.PRDetailDescription},
                        {"prdetails_uom_code", string.IsNullOrEmpty(PRParentDetailsInsert.PRDetailUOM.ToString())?"0":PRParentDetailsInsert.PRDetailUOM.ToString()},
                        {"prdetails_qty", string.IsNullOrEmpty(PRParentDetailsInsert.PRDetailQty)?"0":PRParentDetailsInsert.PRDetailQty},
                        {"pipinput_rate",string.IsNullOrEmpty(PRParentDetailsInsert.PRDetailUnitAmount)?"0":PRParentDetailsInsert.PRDetailUnitAmount},
                        {"pipinput_costestimation",string.IsNullOrEmpty(PRParentDetailsInsert.PRDetailTotalAmount)?"0":PRParentDetailsInsert.PRDetailTotalAmount},
                        {"prdetails_prodservgrp_gid",string.IsNullOrEmpty(PRParentDetailsInsert.ChildProductGroupGid.ToString())?"0":PRParentDetailsInsert.ChildProductGroupGid.ToString()}
                    };
                    string[,] whrcol1 = new string[,]
	                {
                        {"prdetails_prheader_gid", string.IsNullOrEmpty(PRParentDetailsInsert.PRGid.ToString())?"0":PRParentDetailsInsert.PRGid.ToString()},
                        {"prdetails_gid", string.IsNullOrEmpty(PRParentDetailsInsert.PRDetailGid.ToString())?"0":PRParentDetailsInsert.PRDetailGid.ToString()}
                    };
                    string tbname1 = "fb_trn_tprdetails";
                    string query_result1 = objCommonIUD.UpdateCommon(codes, whrcol1, tbname1);
                }

                result = Convert.ToInt32(string.IsNullOrEmpty(prheadergid) ? "0" : prheadergid);

                getconnection();
                cmd = new SqlCommand("pr_fb_trn_prnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prgid", SqlDbType.Int).Value = result;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updateprtotalamnt";
                cmd.ExecuteNonQuery();

                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                con.Close();
            }
        }
        public int DeletePRParentDetails(int refgid = 0, string deletefor = null)
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_prnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prdetailgid", SqlDbType.Int).Value = refgid;
                cmd.Parameters.Add("@DeleteFor", SqlDbType.VarChar).Value = deletefor;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "deleteprdetails";
                int result = Convert.ToInt32(cmd.ExecuteScalar());
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                con.Close();
            }
        }
        public int SaveAsDraft(PRNewEntity PRParentDetailsInsert)
        {
            try
            {
                string prheadergid = "";
                int result = 0;

                string[,] codes2 = new string[,]
                {
                    {"prheader_fccc", string.IsNullOrEmpty(PRParentDetailsInsert.PRFCCCGid.ToString())?"0":PRParentDetailsInsert.PRFCCCGid.ToString()},
                    {"prheader_branchgid", string.IsNullOrEmpty(PRParentDetailsInsert.PRBranchGid.ToString())?"0":PRParentDetailsInsert.PRBranchGid.ToString()},
                    {"prheader_branchsingle",string.IsNullOrEmpty(PRParentDetailsInsert.PRBranchType)?"":PRParentDetailsInsert.PRBranchType},
                    {"prheader_Expensetype",string.IsNullOrEmpty(PRParentDetailsInsert.PRExpenseFlag)?"":PRParentDetailsInsert.PRExpenseFlag},
                    {"prheader_requestforgid",string.IsNullOrEmpty(PRParentDetailsInsert.RequestForValue.ToString())?"0":PRParentDetailsInsert.RequestForValue.ToString()},
                    {"prheader_desc",string.IsNullOrEmpty(PRParentDetailsInsert.PRDescription)?"":PRParentDetailsInsert.PRDescription},
                    {"prheader_remarks",string.IsNullOrEmpty(PRParentDetailsInsert.PRDescription)?"":PRParentDetailsInsert.PRDescription},
                    {"prheader_status","1"},
                    {"prheader_curapprstage","1"}
                };
                string[,] whrcol = new string[,]
	            {
                    {"prheader_gid", string.IsNullOrEmpty(PRParentDetailsInsert.PRGid.ToString())?"0":PRParentDetailsInsert.PRGid.ToString()}
                };
                string tbname = "fb_trn_tprheader";
                string query_result = objCommonIUD.UpdateCommon(codes2, whrcol, tbname);

                prheadergid = string.IsNullOrEmpty(PRParentDetailsInsert.PRGid.ToString()) ? "0" : PRParentDetailsInsert.PRGid.ToString();

                if (query_result.ToLower() == "success" && PRParentDetailsInsert.RequestForName == "submit")
                {
                    getconnection();
                    cmd = new SqlCommand("pr_fb_trn_prnew", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                    cmd.Parameters.Add("@prgid", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(prheadergid) ? "0" : prheadergid);
                    cmd.Parameters.Add("@prdetailgid", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(PRParentDetailsInsert.RequestForValue.ToString()) ? "0" : PRParentDetailsInsert.RequestForValue.ToString());
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "submitraiser";
                    cmd.ExecuteNonQuery();
                }

                result = Convert.ToInt32(string.IsNullOrEmpty(prheadergid) ? "0" : prheadergid);
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                con.Close();
            }
        }
        public string SubmitApprover(PRNewEntity PRParentDetailsInsert)
        {
            try
            {
                string prheadergid = "";
                string result = "0";

                prheadergid = string.IsNullOrEmpty(PRParentDetailsInsert.PRGid.ToString()) ? "0" : PRParentDetailsInsert.PRGid.ToString();
                
                //objErrorLog.WriteErrorLog("001", prheadergid);
                //objErrorLog.WriteErrorLog("002", PRParentDetailsInsert.PRApprovedBy);

                if (PRParentDetailsInsert.PRApprovedBy == "pipinputs")
                {
                   // objErrorLog.WriteErrorLog("003", HttpContext.Current.Session["employee_gid"].ToString());

                    getconnection();
                    cmd = new SqlCommand("pr_fb_trn_prnew", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                    cmd.Parameters.Add("@prgid", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(prheadergid) ? "0" : prheadergid);
                    cmd.Parameters.Add("@remarks", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PRParentDetailsInsert.PRApprovalRemarks) ? "" : PRParentDetailsInsert.PRApprovalRemarks;
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "submitpipinputs";
                    cmd.ExecuteNonQuery();
                    result = "SUCCESS";

                }
                else
                {
                    if (PRParentDetailsInsert.PRApprovedBy == "supervisor")
                    {
                        //objErrorLog.WriteErrorLog("004", HttpContext.Current.Session["employee_gid"].ToString());
                        getconnection();
                        cmd = new SqlCommand("pr_fb_trn_prnew", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                        cmd.Parameters.Add("@prgid", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(prheadergid) ? "0" : prheadergid);
                        cmd.Parameters.Add("@remarks", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PRParentDetailsInsert.PRApprovalRemarks) ? "" : PRParentDetailsInsert.PRApprovalRemarks;
                        cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "submitsupervisor";
                        cmd.ExecuteNonQuery();
                    }
                    result = GetDelmatemployee(PRParentDetailsInsert);
                }
                //objErrorLog.WriteErrorLog("005", result);

                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.InnerException.ToString());
                return "0";
            }
            finally
            {
                con.Close();
            }
        }
        public string GetDelmatemployee(PRNewEntity PRParentDetailsInsert)
        {
            string resultone = string.Empty;
            string queue_toG = string.Empty;
            string queue_toD = string.Empty;

            string queue_totype = string.Empty;
            string queue_tobranch = string.Empty;
            string queue_todept = string.Empty;
            string delmattype = string.Empty;

            string delmatgid = string.Empty;
            string Expenses = string.Empty;

            string queuengid = string.Empty;
            string queuento = string.Empty;
            int prdelmat_result = 0;
            string pr_erroutput = "";
            string empgid = string.Empty;
            string finalfalg = "";
            string nqueuety = "";
            string nqueuet = "";
            string ndelgid = "";
            string pr_sql_output = "";
            string additional_flag = "";
            try
            {
                getMyApprovaldetails("PR");
               // objErrorLog.WriteErrorLog("006", PRParentDetailsInsert.PRGid.ToString());
                getconnection();
                cmd = new SqlCommand("pr_fb_getqueuegidemp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prgid", PRParentDetailsInsert.PRGid);
                da = new SqlDataAdapter(cmd);
                DataTable dtcmd = new DataTable();

               // objErrorLog.WriteErrorLog("007selva", "db.fill");
                da.Fill(dtcmd);

               // objErrorLog.WriteErrorLog("0010selva", "dtcmd.table");

                if (dtcmd.Rows.Count > 0)
                {

                    queuengid = dtcmd.Rows[0]["queuegid"].ToString();

                    queuento = objCmnFunctions.GetLoginUserGid().ToString();

                   // objErrorLog.WriteErrorLog("008selva", queuento);

                    Hashtable Togetlist = new Hashtable(); 
                   // objErrorLog.WriteErrorLog("0011selva", Togetlist.ToString());

                    Togetlist = (Hashtable)HttpContext.Current.Session["Queue_delegateslist"];

                   // objErrorLog.WriteErrorLog("009selva", Togetlist.ToString());

                    if (Togetlist.ContainsKey(queuengid))
                    {
                        empgid = Togetlist[queuengid].ToString();
                       // objErrorLog.WriteErrorLog("008", empgid);
                    }
                    if (empgid == "")
                    {
                        empgid = queuento.ToString();
                    }
                    try
                    {
                        cmd = new SqlCommand("pr_prdelmat", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@prheader_gid", PRParentDetailsInsert.PRGid);

                       // objErrorLog.WriteErrorLog("109", PRParentDetailsInsert.PRGid.ToString());

                        cmd.Parameters.AddWithValue("@queue_gid", queuengid);

                       // objErrorLog.WriteErrorLog("110", queuengid);

                        cmd.Parameters.AddWithValue("@pr_approver_gid", Convert.ToInt32(empgid.ToString()));

                       // objErrorLog.WriteErrorLog("111", empgid.ToString());

                        cmd.Parameters.Add("@prdelmat_result", SqlDbType.Int, 32);
                        cmd.Parameters["@prdelmat_result"].Direction = ParameterDirection.Output;

                        cmd.Parameters.Add("@pr_final_flag", SqlDbType.Char, 1);
                        cmd.Parameters["@pr_final_flag"].Direction = ParameterDirection.Output;

                        cmd.Parameters.Add("@pr_next_delmatmatrix_gid", SqlDbType.Int, 64);
                        cmd.Parameters["@pr_next_delmatmatrix_gid"].Direction = ParameterDirection.Output;

                        cmd.Parameters.Add("@pr_next_queue_to_type", SqlDbType.Char, 1);
                        cmd.Parameters["@pr_next_queue_to_type"].Direction = ParameterDirection.Output;

                        cmd.Parameters.Add("@pr_next_queue_to_gid", SqlDbType.Int, 64);
                        cmd.Parameters["@pr_next_queue_to_gid"].Direction = ParameterDirection.Output;

                        cmd.Parameters.Add("@pr_err_output", SqlDbType.VarChar, 10000);
                        cmd.Parameters["@pr_err_output"].Direction = ParameterDirection.Output;

                        cmd.Parameters.Add("@pr_sql_output", SqlDbType.VarChar, 10000);
                        cmd.Parameters["@pr_sql_output"].Direction = ParameterDirection.Output;

                        cmd.Parameters.Add("@pr_next_queue_to_additional_flag", SqlDbType.VarChar, 10000);
                        cmd.Parameters["@pr_next_queue_to_additional_flag"].Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();



                        finalfalg = Convert.ToString(cmd.Parameters["@pr_final_flag"].Value);



                        nqueuety = Convert.ToString(cmd.Parameters["@pr_next_queue_to_type"].Value);



                        nqueuet = Convert.ToString(cmd.Parameters["@pr_next_queue_to_gid"].Value);
                        ndelgid = Convert.ToString(cmd.Parameters["@pr_next_delmatmatrix_gid"].Value);
                        pr_erroutput = Convert.ToString(cmd.Parameters["@pr_err_output"].Value);
                        pr_sql_output = Convert.ToString(cmd.Parameters["@pr_sql_output"].Value);
                        additional_flag = Convert.ToString(cmd.Parameters["@pr_next_queue_to_additional_flag"].Value);


                        //objErrorLog.WriteErrorLog("112", finalfalg + "-" + nqueuety + "-" + nqueuet + "-" + ndelgid + "-" + pr_erroutput + "-" + pr_sql_output + "-" + additional_flag);

                        if (cmd.Parameters["@prdelmat_result"].Value != null)
                            prdelmat_result = Convert.ToInt32(cmd.Parameters["@prdelmat_result"].Value);



                    }
                    catch (Exception ex)
                    {
                       objErrorLog.WriteErrorLog("568 " + ex.Message.ToString(), ex.InnerException.ToString());
                        if (pr_erroutput != "")
                            return pr_erroutput;
                        else
                            return "";
                    }


                    if (prdelmat_result > 0)
                    {
                        if (finalfalg == "N")
                        {                            //---

                            try
                            {
                                string[,] codesq = new string[,]
                                    {
                                         {"queue_action_date","sysdatetime()"},                                                                            
                                         {"queue_action_by",empgid.ToString() },                                         
                                         {"queue_delegation_gid",((Convert.ToInt32(empgid.ToString())== Convert.ToInt32(queuento.ToString()))? 0: objCmnFunctions.GetLoginUserGid()).ToString()},
                                         {"queue_action_remark",string.IsNullOrEmpty(PRParentDetailsInsert.PRApprovalRemarks)?"":PRParentDetailsInsert.PRApprovalRemarks},
                                         {"queue_action_status","1"},
                                         {"queue_isremoved","Y" }

                                         
                                     };
                               // objErrorLog.WriteErrorLog("114", empgid.ToString());
                                string[,] whreq = new string[,]
                                    {
                                     
                                        {"queue_ref_gid",PRParentDetailsInsert.PRGid.ToString()},
                                        {"queue_ref_flag","5"},
                                        {"queue_isremoved","N" }
                                   };

                                //objErrorLog.WriteErrorLog("115", PRParentDetailsInsert.PRGid.ToString());
                                string tnameq = "iem_trn_tqueue";
                                string insertcommendq = objCommonIUD.UpdateCommon(codesq, whreq, tnameq);

                            }
                            catch (Exception ex)
                            {
                                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.InnerException.ToString());
                                if (pr_erroutput != "")
                                    return pr_erroutput;
                                else
                                    return "";
                            }

                            //--2


                            try
                            {
                                //---
                                string[,] codesIN = new string[,]
                                      {
                                           {"queue_date","sysdatetime()"},
                                           {"queue_ref_flag", "5"},
                                           {"queue_ref_gid",PRParentDetailsInsert.PRGid.ToString()},
                                           {"queue_ref_status", "0"},
                                          
                                           {"queue_from",((empgid.ToString()== queuento.ToString())? objCmnFunctions.GetLoginUserGid().ToString(): empgid.ToString()).ToString()},
                                           {"queue_to_type", nqueuety.ToString()},
                                           {"queue_to", nqueuet.ToString()},
                                           {"queue_action_for", "A"},
                                           {"queue_prev_gid",Convert.ToString(getprrpvgid(PRParentDetailsInsert.PRGid.ToString()))},
                                           {"queue_delmat_flag","D"},
                                           {"Additional_flag",additional_flag.ToString()}

                                     };

                                //objErrorLog.WriteErrorLog("115", PRParentDetailsInsert.PRGid.ToString());
                                //objErrorLog.WriteErrorLog("116", empgid.ToString());
                                //objErrorLog.WriteErrorLog("117", nqueuety.ToString());
                                //objErrorLog.WriteErrorLog("118", nqueuet.ToString());
                                //objErrorLog.WriteErrorLog("119", PRParentDetailsInsert.PRGid.ToString());
                                //objErrorLog.WriteErrorLog("120", additional_flag.ToString());

                                string tnameIN = "iem_trn_tqueue";

                                string insertcommendecf = objCommonIUD.InsertCommon(codesIN, tnameIN);
                                resultone = insertcommendecf;
                                //---
                            }
                            catch (Exception ex)
                            {
                                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.InnerException.ToString());
                                if (pr_erroutput != "")
                                    return pr_erroutput;
                                else
                                    return "";
                            }

                        }

                        else
                        {
                            //--

                            try
                            {
                                string[,] codes1 = new string[,]            
                       {
                           {"prheader_status","5"},
                           {"prheader_curapprstage","0"},

                       };
                                string[,] whrcol = new string[,]
                        {
                          
                             {"prheader_gid",PRParentDetailsInsert.PRGid.ToString()}

                        };
                                string tname1 = "fb_trn_tprheader";
                                string updatequery = objCommonIUD.UpdateCommon(codes1, whrcol, tname1);

                                //---
                            }
                            catch (Exception ex)
                            {
                                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.InnerException.ToString());
                                if (pr_erroutput != "")
                                    return pr_erroutput;
                                else
                                    return "";
                            }



                            try
                            {

                                //---

                                string[,] codesq = new string[,]
                                    {
                                         {"queue_action_date","sysdatetime()"},
                                         {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString() },
                                         {"queue_delegation_gid",((Convert.ToInt32(empgid.ToString())== Convert.ToInt32(queuento.ToString()))? 0: objCmnFunctions.GetLoginUserGid()).ToString()},
                                         {"queue_action_status","1" },
                                         {"queue_action_remark",string.IsNullOrEmpty(PRParentDetailsInsert.PRApprovalRemarks)?"":PRParentDetailsInsert.PRApprovalRemarks},
                                         {"queue_isremoved","Y" },
                                         {"queue_delmat_flag","F"}
                                     };

                                // objErrorLog.WriteErrorLog("121", empgid.ToString());
                                string[,] whreq = new string[,]
                                    {
                                     
                                        {"queue_ref_gid",PRParentDetailsInsert.PRGid.ToString()},
                                        {"queue_ref_flag","5"},
                                         {"queue_isremoved","N" }
                                   };

                                //objErrorLog.WriteErrorLog("121", PRParentDetailsInsert.PRGid.ToString());

                                string tnameq = "iem_trn_tqueue";
                                string insertcommendq = objCommonIUD.UpdateCommon(codesq, whreq, tnameq);
                                resultone = insertcommendq;

                                //----
                            }
                            catch (Exception ex)
                            {
                                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.InnerException.ToString());
                                if (pr_erroutput != "")
                                    return pr_erroutput;
                                else
                                    return "";
                            }
                        }
                    }
                    else
                    {
                        resultone = pr_erroutput;
                    }
                }
                return resultone;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.InnerException.ToString());
                if (pr_erroutput != "")
                    return pr_erroutput;
                else
                    return "";
            }
            finally
            {
                con.Close();
            }

        }
        public int getprrpvgid(string prheadergid)
        {
            int prrvgid = 0;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_getprrrgid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prgid", prheadergid.ToString());
                prrvgid = (int)cmd.ExecuteScalar();
                return prrvgid;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                con.Close();
            }

        }
        public int RejectApprover(PRNewEntity PRParentDetailsInsert)
        {
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_prnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prgid", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(PRParentDetailsInsert.PRGid.ToString()) ? "0" : PRParentDetailsInsert.PRGid.ToString());
                cmd.Parameters.Add("@SearchText", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PRParentDetailsInsert.PRApprovedBy) ? "" : PRParentDetailsInsert.PRApprovedBy;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getrefflag";
                int refflag = Convert.ToInt32(cmd.ExecuteScalar());

                string[,] codw = new string[,]
                {
                    {"queue_action_for","R"},
                    {"queue_action_status","2"},
                    {"queue_isremoved","Y"},
                    {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                    {"queue_action_date","sysdatetime()"},
                    {"queue_action_remark",PRParentDetailsInsert.PRApprovalRemarks}
                };
                string[,] codewhe = new string[,]
                {
                    {"queue_ref_gid",PRParentDetailsInsert.PRGid.ToString()},
                    {"queue_ref_flag",refflag.ToString()},
                    {"queue_isremoved","N"}
                };

                string tblname = "iem_trn_tqueue";
                string updatecon = objCommonIUD.UpdateCommon(codw, codewhe, tblname);

                string[,] codw2 = new string[,]
                {
                    {"prheader_status","6"},
                    {"prheader_pramount","0"}
                };
                string[,] codewhe2 = new string[,]
                {
                    {"prheader_gid",PRParentDetailsInsert.PRGid.ToString()}
                };

                string tblname2 = "fb_trn_tprheader";
                string updatecon2 = objCommonIUD.UpdateCommon(codw2, codewhe2, tblname2);

                string[,] codw3 = new string[,]
                {
                    {"pipinput_costestimation","0"},
                    {"pipinput_rate","0"}
                };
                string[,] codewhe3 = new string[,]
                {
                    {"prdetails_prheader_gid",PRParentDetailsInsert.PRGid.ToString()}
                };
                string tblname3 = "fb_trn_tprdetails";
                string updatecon3 = objCommonIUD.UpdateCommon(codw3, codewhe3, tblname3);
                return 1;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                con.Close();
            }
        }
        public int CreatePRAttachment(Attachments PRParentDetailsInsert)
        {
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_prnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                string attachmenttype = "0";
                if (PRParentDetailsInsert.AttachmentFor == "PRDET")
                {
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getrefflagPRDET";
                    attachmenttype = "2";
                }
                else
                {
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getrefflagPR";
                    attachmenttype = "3";
                }
                string refflag = Convert.ToString(cmd.ExecuteScalar());

                string[,] codes2 = new string[,] 
                   {
                     {"attachment_ref_flag",string.IsNullOrEmpty(refflag)?"0":refflag},
                     {"attachment_ref_gid",string.IsNullOrEmpty(PRParentDetailsInsert.AttachmentRefGid.ToString())?"0":PRParentDetailsInsert.AttachmentRefGid.ToString()},
                     {"attachment_filename",string.IsNullOrEmpty(PRParentDetailsInsert.AttachedActualFileName)?"":PRParentDetailsInsert.AttachedActualFileName},
                     {"attachment_desc",string.IsNullOrEmpty(PRParentDetailsInsert.AttachDescription)?"":PRParentDetailsInsert.AttachDescription},
                     {"attachment_attachmenttype_gid",string.IsNullOrEmpty(attachmenttype)?"0":attachmenttype},
                     {"attachment_date","sysdatetime()" },
                     {"attachment_by",objCmnFunctions.GetLoginUserGid().ToString()},
                     {"attachment_isremoved","N" },
                     {"attachment_tempfilename",string.IsNullOrEmpty(PRParentDetailsInsert.TempFileName)?"":PRParentDetailsInsert.TempFileName},
                   };
                string insertcommand2 = objCommonIUD.InsertCommon(codes2, "iem_trn_tattachment");

                return 1;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                con.Close();
            }
        }
        public DataSet GetPRAttachments(string attachmentfor = "", int refgid = 0)
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_prnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SearchText", SqlDbType.VarChar).Value = string.IsNullOrEmpty(attachmentfor) ? "" : attachmentfor;
                cmd.Parameters.Add("@prgid", SqlDbType.Int).Value = refgid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getattachmentspr";
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
        public int DeletePRAttachment(Attachments PRParentDetailsInsert)
        {
            try
            {
                string[,] codw2 = new string[,] 
                   {
                     {"attachment_isremoved","Y" }
                   };
                string[,] codewhe2 = new string[,]
                {
                    {"attachment_gid",PRParentDetailsInsert.AttachmentID.ToString()}
                };
                string updatecon2 = objCommonIUD.UpdateCommon(codw2, codewhe2, "iem_trn_tattachment");
                return 1;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                con.Close();
            }
        }
        public DataTable DoClone(int prgid = 0)
        {
            DataTable dt = new DataTable();
            try
            {
                string prreqfor = "";

                getconnection();
                cmd = new SqlCommand("pr_fb_trn_prnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prgid", SqlDbType.Int).Value = prgid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getrequestfor";
                prreqfor = Convert.ToString(cmd.ExecuteScalar());

                string PRNumber = objCmnFunctions.GetSequenceNoFb("PR", "", string.IsNullOrEmpty(prreqfor) ? "" : prreqfor);

                cmd = new SqlCommand("pr_fb_trn_prnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@prgid", SqlDbType.Int).Value = prgid;
                cmd.Parameters.Add("@SearchText", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PRNumber) ? "" : PRNumber;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "doclone";
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
        public int CheckAmount(int prgid = 0)
        {
            try
            {
                int result = 0;
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_prnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prgid", SqlDbType.Int).Value = prgid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "checkamnt";
                result = Convert.ToInt32(cmd.ExecuteScalar());
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                con.Close();
            }
        }

        public IEnumerable<flexibuydashboard> getMyApprovaldetails(string doctype)
        {
            List<flexibuydashboard> lstReqGrid = new List<flexibuydashboard>();
            try
            {

                //bharathi2
                Hashtable queuelist = new Hashtable();
                //Hashtable emplist = new Hashtable();
                Hashtable deptlist = new Hashtable();
                int delegatesuser;
                int delegatedeptid;
                int deptlistid = 0;
                int emplistid = 0;
                string IsAdmDept;
                //emplist.Add(emplistid, objCmnFunctions.GetLoginUserGid());
                getconnection();
                DataTable dtdel = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_NatureofExpenses", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@para1", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid().ToString();
                cmd.Parameters.Add("@doctype", SqlDbType.VarChar).Value = doctype;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Getdelegateuser";
                da = new SqlDataAdapter(cmd);
                da.Fill(dtdel);
                if (dtdel.Rows.Count > 0)
                {
                    for (int TR = 0; TR < dtdel.Rows.Count; TR++)
                    {
                        if (deptlist.Count == 0)
                        {
                            deptlist.Add(deptlistid, Convert.ToString(dtdel.Rows[TR]["delegate_deptid"].ToString()));
                        }
                        else
                        {
                            if (!deptlist.ContainsValue(Convert.ToString(dtdel.Rows[TR]["delegate_deptid"].ToString())))
                            {
                                deptlistid++;
                                deptlist.Add(deptlistid, Convert.ToString(dtdel.Rows[TR]["delegate_deptid"].ToString()));
                            }
                        }
                    }
                }

                if (deptlist.ContainsValue("1"))
                {
                    IsAdmDept = "Y";
                }
                else
                {
                    IsAdmDept = "N";
                }

                //getconnection();
                flexibuydashboard dash;
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_inprocess", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@emp_gid", SqlDbType.BigInt).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@output_type", SqlDbType.Char).Value = "D";

                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dash = new flexibuydashboard();
                        dash.dsno = i + 1;
                        dash.category = Convert.ToString(dt.Rows[i]["queue_ref_name"].ToString());
                        dash.dgid = Convert.ToInt32(dt.Rows[i]["queue_ref_gid"].ToString());
                        dash.docNo = Convert.ToString(dt.Rows[i]["doc_no"].ToString());
                        dash.ddate = Convert.ToString(dt.Rows[i]["doc_date"].ToString());
                        dash.amount = Convert.ToString(dt.Rows[i]["doc_amount"].ToString());
                        dash.status = "Inprocess";
                        dash.statusId = dash.statusId = dt.Rows[i]["doc_status_gid"].ToString();
                        dash.raiser = Convert.ToString(dt.Rows[i]["doc_raiser_name"].ToString());
                        dash.description = Convert.ToString(dt.Rows[i]["doc_desc"].ToString());
                        lstReqGrid.Add(dash);
                        if (!queuelist.ContainsKey(dt.Rows[i]["queue_gid"].ToString()))
                        {
                            queuelist.Add(dt.Rows[i]["queue_gid"].ToString(), objCmnFunctions.GetLoginUserGid());
                        }
                    }
                }

                if (dtdel.Rows.Count > 0)
                {
                    if (IsAdmDept == "Y")
                    {
                        for (int tr = 0; tr < 1; tr++)
                        {
                            delegatesuser = Convert.ToInt32(dtdel.Rows[tr]["delegate_by"].ToString().Trim());
                            delegatedeptid = 1; //to Set All Department 


                            DataTable dt1 = new DataTable();
                            cmd = new SqlCommand("pr_fb_inprocess", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@emp_gid", SqlDbType.BigInt).Value = delegatesuser;
                            cmd.Parameters.Add("@delegate_by", SqlDbType.BigInt).Value = objCmnFunctions.GetLoginUserGid();
                            cmd.Parameters.Add("@delegatedeptId", SqlDbType.BigInt).Value = delegatedeptid;
                            cmd.Parameters.Add("@output_type", SqlDbType.Char).Value = "T";

                            da = new SqlDataAdapter(cmd);
                            da.Fill(dt1);
                            if (dt1.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt1.Rows.Count; i++)
                                {
                                    dash = new flexibuydashboard();
                                    dash.dsno = i + 1;
                                    dash.category = Convert.ToString(dt1.Rows[i]["queue_ref_name"].ToString());
                                    dash.dgid = Convert.ToInt32(dt1.Rows[i]["queue_ref_gid"].ToString());
                                    dash.docNo = Convert.ToString(dt1.Rows[i]["doc_no"].ToString());
                                    dash.ddate = Convert.ToString(dt1.Rows[i]["doc_date"].ToString());
                                    dash.amount = Convert.ToString(dt1.Rows[i]["doc_amount"].ToString());
                                    dash.status = "Inprocess";
                                    dash.statusId = dash.statusId = dt1.Rows[i]["doc_status_gid"].ToString();
                                    dash.raiser = Convert.ToString(dt1.Rows[i]["doc_raiser_name"].ToString());
                                    dash.description = Convert.ToString(dt1.Rows[i]["doc_desc"].ToString());
                                    lstReqGrid.Add(dash);
                                    if (!queuelist.ContainsKey(dt1.Rows[i]["queue_gid"].ToString()))
                                    {
                                        queuelist.Add(dt1.Rows[i]["queue_gid"].ToString(), delegatesuser);
                                    }
                                }
                            }



                        }
                    }
                    else
                    {

                        for (int tr = 0; tr < dtdel.Rows.Count; tr++)
                        {
                            delegatesuser = Convert.ToInt32(dtdel.Rows[tr]["delegate_by"].ToString().Trim());
                            delegatedeptid = Convert.ToInt32(dtdel.Rows[tr]["delegate_deptid"].ToString().Trim());


                            DataTable dt1 = new DataTable();
                            cmd = new SqlCommand("pr_fb_inprocess", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@emp_gid", SqlDbType.BigInt).Value = delegatesuser;
                            cmd.Parameters.Add("@delegate_by", SqlDbType.BigInt).Value = objCmnFunctions.GetLoginUserGid();
                            cmd.Parameters.Add("@delegatedeptId", SqlDbType.BigInt).Value = delegatedeptid;
                            cmd.Parameters.Add("@output_type", SqlDbType.Char).Value = "T";

                            da = new SqlDataAdapter(cmd);
                            da.Fill(dt1);
                            if (dt1.Rows.Count > 0)
                            {
                                for (int i = 0; i < dt1.Rows.Count; i++)
                                {
                                    dash = new flexibuydashboard();
                                    dash.dsno = i + 1;
                                    dash.category = Convert.ToString(dt1.Rows[i]["queue_ref_name"].ToString());
                                    dash.dgid = Convert.ToInt32(dt1.Rows[i]["queue_ref_gid"].ToString());
                                    dash.docNo = Convert.ToString(dt1.Rows[i]["doc_no"].ToString());
                                    dash.ddate = Convert.ToString(dt1.Rows[i]["doc_date"].ToString());
                                    dash.amount = Convert.ToString(dt1.Rows[i]["doc_amount"].ToString());
                                    dash.status = "Inprocess";
                                    dash.statusId = dash.statusId = dt1.Rows[i]["doc_status_gid"].ToString();
                                    dash.raiser = Convert.ToString(dt1.Rows[i]["doc_raiser_name"].ToString());
                                    dash.description = Convert.ToString(dt1.Rows[i]["doc_desc"].ToString());
                                    lstReqGrid.Add(dash);
                                    if (!queuelist.ContainsKey(dt1.Rows[i]["queue_gid"].ToString()))
                                    {
                                        queuelist.Add(dt1.Rows[i]["queue_gid"].ToString(), delegatesuser);
                                    }
                                }
                            }


                        }
                    }
                }


                HttpContext.Current.Session["Queue_delegateslist"] = queuelist;
                return lstReqGrid;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return lstReqGrid;
            }
            finally
            {
                con.Close();
            }
        }


    }
}