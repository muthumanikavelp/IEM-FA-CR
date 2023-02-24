using IEM.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using IEM.Helper;
namespace IEM.Areas.FLEXIBUY.Models
{
    public class PARDataModel:PARRepository
    {
        SqlCommand cmd;
        SqlConnection con = new SqlConnection();
        SqlDataAdapter da;
        ErrorLog objErrorLog = new ErrorLog();
        CommonIUD objCommonIUD = new CommonIUD();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        dbLib db = new dbLib();
        proLib plib = new proLib();
        public void getconnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }
        public DataSet GetPARHeaderDetails() 
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_parnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getparheaderdetails";
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
        public DataSet GetRequestFor() 
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_parnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetRequestFor";
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
        public DataSet GetPeriodList() 
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_parnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetFinancialYears";
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
        public int AddPARParentDetailsList(PAREntity PARParentDetailsInsert)
        {
            try
            {
                string parheadergid = ""; 
                int result = 0;
                string parnum = objCmnFunctions.GetSequenceNoFb("PAR", "", "");
                string[,] codes2 = new string[,]
                {
                    {"parheader_refno", parnum },
                    {"parheader_date","sysdatetime()"},
                    {"parheader_raiser_gid",objCmnFunctions.GetLoginUserGid().ToString()},
                    {"parheader_Contigencypercent", string.IsNullOrEmpty(PARParentDetailsInsert.PARContigency)?"0":PARParentDetailsInsert.PARContigency},
                    {"parheader_AmtWithContigency", string.IsNullOrEmpty(PARParentDetailsInsert.PARContigencyAmount)?"0": PARParentDetailsInsert.PARContigencyAmount.Replace(",","")},
                    {"parheader_isbudgeted",string.IsNullOrEmpty(PARParentDetailsInsert.PARBudgetedFlag)?"":PARParentDetailsInsert.PARBudgetedFlag},
                    {"parheader_status","1"},
                    {"parheader_locked","N"},
                    {"parheader_amt","0"},
                    {"parheader_desc",string.IsNullOrEmpty(PARParentDetailsInsert.PARDescription)?"":PARParentDetailsInsert.PARDescription},
                    {"parheader_isremoved","N"}
                };
                string tbname = "fb_trn_tparheader";
                string query_result = objCommonIUD.InsertCommon(codes2, tbname);

                if (query_result.ToLower() == "success")
                {

                    getconnection();
                    cmd = new SqlCommand("pr_fb_trn_parnew", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                    cmd.Parameters.Add("@parnumber", SqlDbType.VarChar).Value = parnum;
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getparheadergid";
                    parheadergid = Convert.ToString(cmd.ExecuteScalar());

                    string[,] codes = new string[,] 
                {
                    {"pardetails_parhead_gid", string.IsNullOrEmpty(parheadergid)?"0":parheadergid},
                    {"pardetails_expensetype", string.IsNullOrEmpty(PARParentDetailsInsert.PARExpenseType.ToString())?"0":PARParentDetailsInsert.PARExpenseType.ToString()},
                    {"pardetails_requestfor_gid", string.IsNullOrEmpty(PARParentDetailsInsert.RequestForGid.ToString())?"0":PARParentDetailsInsert.RequestForGid.ToString()},
                    {"pardetails_isbudgeted", string.IsNullOrEmpty(PARParentDetailsInsert.PARBudgetedFlag)?"":PARParentDetailsInsert.PARBudgetedFlag},
                    {"pardetails_year", string.IsNullOrEmpty(PARParentDetailsInsert.PARDetailPeriod)?"":PARParentDetailsInsert.PARDetailPeriod},
                    {"pardetails_amt",string.IsNullOrEmpty(PARParentDetailsInsert.PARDetailAmount)?"0":PARParentDetailsInsert.PARDetailAmount},
                    {"pardetails_remarks",string.IsNullOrEmpty(PARParentDetailsInsert.PARDetailDescription)?"":PARParentDetailsInsert.PARDetailDescription},
                    {"pardetails_desc",string.IsNullOrEmpty(PARParentDetailsInsert.PARDetailDescription.ToString())?"":PARParentDetailsInsert.PARDetailDescription.ToString()},
                    {"pardetails_isremoved","N"}
                };
                    string tbname1 = "fb_trn_tpardetails";
                    string query_result1 = objCommonIUD.InsertCommon(codes, tbname1);
                }

                result = Convert.ToInt32(string.IsNullOrEmpty(parheadergid) ? "0" : parheadergid);

                getconnection();
                cmd = new SqlCommand("pr_fb_trn_parnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@pargid", SqlDbType.Int).Value = result;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updatepartotalamnt";
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
        public DataSet GetPARDetailsAll(int pargid = 0) 
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_parnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@pargid", SqlDbType.Int).Value = pargid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getpardetailsfull";
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
        public int AddPARParentDetailsListNew(PAREntity PARParentDetailsInsert) 
        {
            try
            {
                string parheadergid = string.IsNullOrEmpty(PARParentDetailsInsert.PARGid.ToString()) ? "0" : PARParentDetailsInsert.PARGid.ToString();
                int result = 0;
                string[,] codes2 = new string[,]
                {
                    {"parheader_Contigencypercent", string.IsNullOrEmpty(PARParentDetailsInsert.PARContigency)?"0":PARParentDetailsInsert.PARContigency},
                    {"parheader_AmtWithContigency", string.IsNullOrEmpty(PARParentDetailsInsert.PARContigencyAmount)?"0": PARParentDetailsInsert.PARContigencyAmount.Replace(",","")},
                    {"parheader_isbudgeted",string.IsNullOrEmpty(PARParentDetailsInsert.PARBudgetedFlag)?"":PARParentDetailsInsert.PARBudgetedFlag},
                    {"parheader_status","1"},
                    {"parheader_desc",string.IsNullOrEmpty(PARParentDetailsInsert.PARDescription)?"":PARParentDetailsInsert.PARDescription},
                };
                string[,] whrcol = new string[,]
	            {
                    {"parheader_gid", string.IsNullOrEmpty(PARParentDetailsInsert.PARGid.ToString())?"0":PARParentDetailsInsert.PARGid.ToString()}
                };
                string tbname = "fb_trn_tparheader";
                string query_result = objCommonIUD.UpdateCommon(codes2, whrcol, tbname);

                if (query_result.ToLower() == "success")
                {

                    string[,] codes = new string[,] 
                {
                    {"pardetails_parhead_gid", string.IsNullOrEmpty(parheadergid)?"0":parheadergid},
                    {"pardetails_expensetype", string.IsNullOrEmpty(PARParentDetailsInsert.PARExpenseType.ToString())?"0":PARParentDetailsInsert.PARExpenseType.ToString()},
                    {"pardetails_requestfor_gid", string.IsNullOrEmpty(PARParentDetailsInsert.RequestForGid.ToString())?"0":PARParentDetailsInsert.RequestForGid.ToString()},
                    {"pardetails_isbudgeted", string.IsNullOrEmpty(PARParentDetailsInsert.PARBudgetedFlag)?"":PARParentDetailsInsert.PARBudgetedFlag},
                    {"pardetails_year", string.IsNullOrEmpty(PARParentDetailsInsert.PARDetailPeriod)?"":PARParentDetailsInsert.PARDetailPeriod},
                    {"pardetails_amt",string.IsNullOrEmpty(PARParentDetailsInsert.PARDetailAmount)?"0":PARParentDetailsInsert.PARDetailAmount},
                    {"pardetails_remarks",string.IsNullOrEmpty(PARParentDetailsInsert.PARDetailDescription)?"":PARParentDetailsInsert.PARDetailDescription},
                    {"pardetails_desc",string.IsNullOrEmpty(PARParentDetailsInsert.PARDetailDescription.ToString())?"":PARParentDetailsInsert.PARDetailDescription.ToString()},
                    {"pardetails_isremoved","N"}
                };
                    string tbname1 = "fb_trn_tpardetails";
                    string query_result1 = objCommonIUD.InsertCommon(codes, tbname1);
                }

                result = Convert.ToInt32(string.IsNullOrEmpty(parheadergid) ? "0" : parheadergid);

                getconnection();
                cmd = new SqlCommand("pr_fb_trn_parnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@pargid", SqlDbType.Int).Value = result;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updatepartotalamnt";
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
        public DataSet GetPARDetailParent(int pardetgid = 0)
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_parnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@pargid", SqlDbType.Int).Value = pardetgid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getpardetailsbyid";
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
        public int AddPARApprover(PAREntity PARParentDetailsInsert) 
        {
            try
            {
                string parheadergid = string.IsNullOrEmpty(PARParentDetailsInsert.PARGid.ToString()) ? "0" : PARParentDetailsInsert.PARGid.ToString();
                int result = 0;
                string[,] codes2 = new string[,]
                {
                    {"parapprover_parheader_gid", string.IsNullOrEmpty(parheadergid)?"0":parheadergid},
                    {"parapprover_emp_gid", string.IsNullOrEmpty(PARParentDetailsInsert.PARApproverGid.ToString())?"0": PARParentDetailsInsert.PARApproverGid.ToString()},
                    {"parapprover_approval_date",string.IsNullOrEmpty(PARParentDetailsInsert.PARApproverDate)?"":objCmnFunctions.convertoDateTimeString(PARParentDetailsInsert.PARApproverDate)},
                    {"parapprover_isremoved","N"}
                };
                string tbname = "fb_trn_tparapprover";
                string query_result = objCommonIUD.InsertCommon(codes2, tbname);

                if (query_result.ToLower() == "success")
                {
                    result = Convert.ToInt32(string.IsNullOrEmpty(parheadergid) ? "0" : parheadergid);
                    getconnection();
                    cmd = new SqlCommand("pr_fb_trn_parnew", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pargid", SqlDbType.Int).Value = result;
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updateparfinalapprovaldate";
                    cmd.ExecuteNonQuery();
                }
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
        public DataSet GetPARApprover(int pargid = 0) 
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_parnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@pargid", SqlDbType.Int).Value = pargid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getparapproverdetails";
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
        public int CreatePARAttachment(Attachments PARParentDetailsInsert) 
        {
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_parnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                string attachmenttype = "0";
                if (PARParentDetailsInsert.AttachmentFor == "PARDET")
                {
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getrefflagPARDET";
                    attachmenttype = "2";
                }
                else
                {
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getrefflagPAR";
                    attachmenttype = "3";
                }
                string refflag = Convert.ToString(cmd.ExecuteScalar());

                string[,] codes2 = new string[,] 
                   {
                     {"attachment_ref_flag",string.IsNullOrEmpty(refflag)?"0":refflag},
                     {"attachment_ref_gid",string.IsNullOrEmpty(PARParentDetailsInsert.AttachmentRefGid.ToString())?"0":PARParentDetailsInsert.AttachmentRefGid.ToString()},
                     {"attachment_filename",string.IsNullOrEmpty(PARParentDetailsInsert.AttachedActualFileName)?"":PARParentDetailsInsert.AttachedActualFileName},
                     {"attachment_desc",string.IsNullOrEmpty(PARParentDetailsInsert.AttachDescription)?"":PARParentDetailsInsert.AttachDescription},
                     {"attachment_attachmenttype_gid",string.IsNullOrEmpty(attachmenttype)?"0":attachmenttype},
                     {"attachment_date","sysdatetime()" },
                     {"attachment_by",objCmnFunctions.GetLoginUserGid().ToString()},
                     {"attachment_isremoved","N" },
                     {"attachment_tempfilename",string.IsNullOrEmpty(PARParentDetailsInsert.TempFileName)?"":PARParentDetailsInsert.TempFileName},
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
        public DataSet GetPARAttachments(string attachmentfor = "", int refgid = 0)
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_parnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SearchText", SqlDbType.VarChar).Value = string.IsNullOrEmpty(attachmentfor) ? "" : attachmentfor;
                cmd.Parameters.Add("@pargid", SqlDbType.Int).Value = refgid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getattachmentspar";
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
        public int UpdatePARParentList(PAREntity PARParentDetailsInsert) 
        {
            try
            {
                string parheadergid = string.IsNullOrEmpty(PARParentDetailsInsert.PARGid.ToString()) ? "0" : PARParentDetailsInsert.PARGid.ToString();
                int result = 0;
                string[,] codes2 = new string[,]
                {
                    {"parheader_Contigencypercent", string.IsNullOrEmpty(PARParentDetailsInsert.PARContigency)?"0":PARParentDetailsInsert.PARContigency},
                    {"parheader_AmtWithContigency", string.IsNullOrEmpty(PARParentDetailsInsert.PARContigencyAmount)?"0": PARParentDetailsInsert.PARContigencyAmount.Replace(",","")},
                    {"parheader_isbudgeted",string.IsNullOrEmpty(PARParentDetailsInsert.PARBudgetedFlag)?"":PARParentDetailsInsert.PARBudgetedFlag},
                    {"parheader_status","1"},
                    {"parheader_desc",string.IsNullOrEmpty(PARParentDetailsInsert.PARDescription)?"":PARParentDetailsInsert.PARDescription},
                };
                string[,] whrcol = new string[,]
	            {
                    {"parheader_gid", string.IsNullOrEmpty(PARParentDetailsInsert.PARGid.ToString())?"0":PARParentDetailsInsert.PARGid.ToString()}
                };
                string tbname = "fb_trn_tparheader";
                string query_result = objCommonIUD.UpdateCommon(codes2, whrcol, tbname);

                if (query_result.ToLower() == "success")
                {
                    string[,] codes = new string[,] 
                {
                    {"pardetails_parhead_gid", string.IsNullOrEmpty(parheadergid)?"0":parheadergid},
                    {"pardetails_expensetype", string.IsNullOrEmpty(PARParentDetailsInsert.PARExpenseType.ToString())?"0":PARParentDetailsInsert.PARExpenseType.ToString()},
                    {"pardetails_requestfor_gid", string.IsNullOrEmpty(PARParentDetailsInsert.RequestForGid.ToString())?"0":PARParentDetailsInsert.RequestForGid.ToString()},
                    {"pardetails_isbudgeted", string.IsNullOrEmpty(PARParentDetailsInsert.PARBudgetedFlag)?"":PARParentDetailsInsert.PARBudgetedFlag},
                    {"pardetails_year", string.IsNullOrEmpty(PARParentDetailsInsert.PARDetailPeriod)?"":PARParentDetailsInsert.PARDetailPeriod},
                    {"pardetails_amt",string.IsNullOrEmpty(PARParentDetailsInsert.PARDetailAmount)?"0":PARParentDetailsInsert.PARDetailAmount},
                    {"pardetails_remarks",string.IsNullOrEmpty(PARParentDetailsInsert.PARDetailDescription)?"":PARParentDetailsInsert.PARDetailDescription},
                    {"pardetails_desc",string.IsNullOrEmpty(PARParentDetailsInsert.PARDetailDescription.ToString())?"":PARParentDetailsInsert.PARDetailDescription.ToString()},
                    {"pardetails_isremoved","N"}
                };
                string[,] whrcol2 = new string[,]
	            {
                    {"pardetails_parhead_gid", string.IsNullOrEmpty(PARParentDetailsInsert.PARGid.ToString())?"0":PARParentDetailsInsert.PARGid.ToString()},
                    {"pardetails_gid", string.IsNullOrEmpty(PARParentDetailsInsert.PARDetailGid.ToString())?"0":PARParentDetailsInsert.PARDetailGid.ToString()}
                };
                    string tbname1 = "fb_trn_tpardetails";
                    string query_result2 = objCommonIUD.UpdateCommon(codes, whrcol2, tbname1);
                }

                result = Convert.ToInt32(string.IsNullOrEmpty(parheadergid) ? "0" : parheadergid);

                getconnection();
                cmd = new SqlCommand("pr_fb_trn_parnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@pargid", SqlDbType.Int).Value = result;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updatepartotalamnt";
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
        public int DeletePARParentDetails(int refgid = 0, string deletefor = null)
        {
            DataSet ds = new DataSet(); 
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_parnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@pardetailgid", SqlDbType.Int).Value = refgid;
                cmd.Parameters.Add("@DeleteFor", SqlDbType.VarChar).Value = deletefor;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "deletepardetails";
                int result = Convert.ToInt32(cmd.ExecuteScalar());

                if (deletefor == "child" || deletefor == "approver")
                {
                    getconnection();
                    cmd = new SqlCommand("pr_fb_trn_parnew", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@pargid", SqlDbType.Int).Value = result;
                    if (deletefor == "child")
                         cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updatepartotalamnt";
                    else
                         cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updateparfinalapprovaldate";
                    cmd.ExecuteNonQuery();
                 }

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
        public int DeletePARAttachment(Attachments PARParentDetailsInsert)
        { 
            try
            {
                string[,] codw2 = new string[,] 
                   {
                     {"attachment_isremoved","Y" }
                   };
                string[,] codewhe2 = new string[,]
                { 
                    {"attachment_gid",PARParentDetailsInsert.AttachmentID.ToString()}
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
        public int SaveAsDraft(PAREntity PARParentDetailsInsert)
        {
            try
            {
                string parheadergid = string.IsNullOrEmpty(PARParentDetailsInsert.PARGid.ToString()) ? "0" : PARParentDetailsInsert.PARGid.ToString();
                int result = 0;
                string[,] codes2 = new string[,]
                {
                    {"parheader_Contigencypercent", string.IsNullOrEmpty(PARParentDetailsInsert.PARContigency)?"0":PARParentDetailsInsert.PARContigency},
                    {"parheader_AmtWithContigency", string.IsNullOrEmpty(PARParentDetailsInsert.PARContigencyAmount)?"0": PARParentDetailsInsert.PARContigencyAmount.Replace(",","")},
                    {"parheader_isbudgeted",string.IsNullOrEmpty(PARParentDetailsInsert.PARBudgetedFlag)?"":PARParentDetailsInsert.PARBudgetedFlag},
                    {"parheader_status","1"},
                    {"parheader_desc",string.IsNullOrEmpty(PARParentDetailsInsert.PARDescription)?"":PARParentDetailsInsert.PARDescription},
                };
                string[,] whrcol = new string[,]
	            {
                    {"parheader_gid", string.IsNullOrEmpty(PARParentDetailsInsert.PARGid.ToString())?"0":PARParentDetailsInsert.PARGid.ToString()}
                };
                string tbname = "fb_trn_tparheader";
                string query_result = objCommonIUD.UpdateCommon(codes2, whrcol, tbname);

                result = Convert.ToInt32(string.IsNullOrEmpty(parheadergid) ? "0" : parheadergid);

                getconnection();
                cmd = new SqlCommand("pr_fb_trn_parnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@pargid", SqlDbType.Int).Value = result;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updatepartotalamnt";
                cmd.ExecuteNonQuery();

                if (query_result.ToLower() == "success" && PARParentDetailsInsert.DeleteFor == "submit")
                {
                    getconnection();
                    cmd = new SqlCommand("pr_fb_trn_parnew", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                    cmd.Parameters.Add("@pargid", SqlDbType.Int).Value = result;
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "submitraiser";
                    cmd.ExecuteNonQuery();
                }
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
        public int CheckFinalApprover(int pargid = 0) 
        {
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_parnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@pargid", SqlDbType.Int).Value = pargid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "parapprovervalidation";
                int result = Convert.ToInt32(cmd.ExecuteScalar());
                if (result == 0)
                    result = 404;
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
        public string SubmitApprover(PAREntity PARParentDetailsInsert)
        { 
            try
            {
                string parheadergid = "";
                string result = "0";

                parheadergid = string.IsNullOrEmpty(PARParentDetailsInsert.PARGid.ToString()) ? "0" : PARParentDetailsInsert.PARGid.ToString();
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_parnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@pargid", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(parheadergid) ? "0" : parheadergid);
                cmd.Parameters.Add("@remarks", SqlDbType.VarChar).Value = string.IsNullOrEmpty(PARParentDetailsInsert.PARApprovalRemarks) ? "" : PARParentDetailsInsert.PARApprovalRemarks;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "submitchecker";
                cmd.ExecuteNonQuery();
                result = "SUCCESS";
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "0";
            }
            finally
            {
                con.Close();
            }
        }
        public int RejectApprover(PAREntity PARParentDetailsInsert)
        {
            try 
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_parnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getrefflagPAR";
                int refflag = Convert.ToInt32(cmd.ExecuteScalar());

                string[,] codw = new string[,]
                {
                    {"queue_action_for","R"},
                    {"queue_action_status","2"},
                    {"queue_isremoved","Y"},
                    {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                    {"queue_action_date","sysdatetime()"},
                    {"queue_action_remark",PARParentDetailsInsert.PARApprovalRemarks}
                };
                string[,] codewhe = new string[,]
                {
                    {"queue_ref_gid",PARParentDetailsInsert.PARGid.ToString()},
                    {"queue_ref_flag",refflag.ToString()},
                    {"queue_isremoved","N"}
                };

                string tblname = "iem_trn_tqueue";
                string updatecon = objCommonIUD.UpdateCommon(codw, codewhe, tblname);

                string[,] codw2 = new string[,]
                {
                    {"parheader_status","6"}
                };
                string[,] codewhe2 = new string[,]
                {
                    {"parheader_gid",PARParentDetailsInsert.PARGid.ToString()}
                };

                string tblname2 = "fb_trn_tparheader";
                string updatecon2 = objCommonIUD.UpdateCommon(codw2, codewhe2, tblname2);

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
    }
}