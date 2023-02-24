using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using IEM.Common;
using IEM.Areas.MASTERS.Controllers;

namespace IEM.Models
{
    public class MailApproveDataModel : MailApproveRepository
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        ForMailController mailsender = new ForMailController();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        CommonIUD objCommonIUD = new CommonIUD();
        ErrorLog objErrorLog = new ErrorLog();
        private void GetConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }
        public DataTable IsQueueClosed(int queuegid)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_iem_mst_mailtemplate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@queuegid", SqlDbType.Int).Value = Convert.ToInt32(queuegid);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "isqueueclosed";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string CheckSupplierIsLocked(string suppliercode, int empgid)
        {
            try
            {
                GetConnection();
                string result = "error";
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_iem_mst_mailtemplate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@suppliercode", SqlDbType.VarChar).Value = suppliercode.ToUpper().Trim();
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(empgid);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "issupplierlocked";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    var LockedBy = dt.Rows[0]["locked_by"].ToString();
                    var LockedDate = dt.Rows[0]["supplierheader_logeddate"].ToString();
                    result = "This Supplier is Locked by " + LockedBy + " From " + LockedDate;
                }
                else
                {
                    result = "approve";
                }
                return result.ToString();
            }
            catch (Exception ex)
            {
                return "error";
            }
        }


        public string IsExistingApprover(int suppliergid, int empgid)
        {
            try
            {
                GetConnection();
                string result = "error";
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_iem_mst_mailtemplate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.VarChar).Value = suppliergid;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(empgid);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "isexistingapprover";
                result = Convert.ToString(cmd.ExecuteScalar());

                return result.ToString();
            }
            catch (Exception ex)
            {
                return "error";
            }
        }

        public DataTable GetNextApprover(string supcode)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_iem_mst_mailtemplate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@suppliercode", SqlDbType.VarChar).Value = Convert.ToString(supcode).ToUpper().Trim();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getnextapprover";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }




        public DataTable GetMailDetailsApprove()
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_iem_mst_mailtemplate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@suppliercode", SqlDbType.VarChar).Value = Convert.ToString(HttpContext.Current.Session["SupplierCodeApprove"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getemaildetailsapprove";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
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

        public void SubmitToNextQueueMail(string queueto = "", string requesttype = "", string remarks = "", int actionstatus = 1, int skipflag = 0)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_SubmitApprovalMail", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@refgid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierGidApprove"]);
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["EmployeeGidApprove"]);
                cmd.Parameters.Add("@requesttype", SqlDbType.VarChar).Value = Convert.ToString(requesttype);
                cmd.Parameters.Add("@actionremark", SqlDbType.VarChar).Value = Convert.ToString(remarks);
                cmd.Parameters.Add("@queueto", SqlDbType.VarChar).Value = Convert.ToString(queueto);
                cmd.Parameters.Add("@actionstatus", SqlDbType.Int).Value = actionstatus;
                cmd.Parameters.Add("@skipflag", SqlDbType.Int).Value = skipflag;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "asms";
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetSupIdForMail()
        {
            try
            {
                GetConnection();
                string result = "";
                int EmpGid = Convert.ToInt32(HttpContext.Current.Session["EmployeeGidApprove"]);
                int SupGid = Convert.ToInt32(HttpContext.Current.Session["SupplierGidApprove"]);
                cmd = new SqlCommand("pr_iem_mst_mailtemplate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = SupGid;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = EmpGid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getqueuegid";
                result = Convert.ToString(cmd.ExecuteScalar());
                return result;
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


        public IEnumerable<MailApproveEntity> SelectMailtemplate(string Module, string TransactionType, string gid, string request, string workflow)
        {
            try
            {
                List<MailApproveEntity> objNatureofExpenses = new List<MailApproveEntity>();
                MailApproveEntity objModel;
                var workflowtype = Convert.ToInt32(workflow);
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_iem_mst_mailtemplate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@modulename", SqlDbType.VarChar).Value = Module.ToUpper().Trim();
                cmd.Parameters.Add("@requesttype", SqlDbType.VarChar).Value = TransactionType.ToUpper().Trim();
                cmd.Parameters.Add("@workflow", SqlDbType.Int).Value = Convert.ToInt32(workflow.ToString());
                cmd.Parameters.Add("@approvalstatus", SqlDbType.VarChar).Value = request.ToUpper().Trim();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getmailtemplatedetails";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    objModel = new MailApproveEntity();
                    objModel.TemplateName = Convert.ToString(dt.Rows[0]["mailtemplate_template_name"].ToString());
                    objModel.TemplateId = Convert.ToString(dt.Rows[0]["mailtemplate_gid"].ToString());
                    objModel.TransactionTypeName = Convert.ToString(dt.Rows[0]["mailtemplate_mailtype_name"].ToString());
                    objModel.WorkFlowId = Convert.ToString(dt.Rows[0]["workflow_level"].ToString());
                    objModel.WorkFlowName = Convert.ToString(dt.Rows[0]["workflow_name"].ToString());
                    objModel.IsFinalApprover = Convert.ToString(dt.Rows[0]["isfinalapproval"].ToString());
                    objModel.TriggerTypeName = Convert.ToString(dt.Rows[0]["mailtemplate_trigger_on"].ToString());
                    objModel.Subject = Convert.ToString(dt.Rows[0]["mailtemplate_subject"].ToString());
                    objModel.Content = Convert.ToString(dt.Rows[0]["mailtemplate_content"].ToString());
                    objModel.Signature = Convert.ToString(dt.Rows[0]["mailtemplate_sign"].ToString());
                    objModel.Includeflg = Convert.ToString(dt.Rows[0]["mailtemplate_include_approval"].ToString());
                    objModel.TOid = Convert.ToString(dt.Rows[0]["mailtemplate_to"].ToString());
                    objModel.CCid = Convert.ToString(dt.Rows[0]["mailtemplate_cc"].ToString());
                    objModel.BCCid = Convert.ToString(dt.Rows[0]["mailtemplate_bcc"].ToString());
                    objNatureofExpenses.Add(objModel);
                }

                return objNatureofExpenses;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string SelectMailIdsForRoleGroup(string rolegroupname, string queuegid, string workflow, string modulename, string requesttype)
        {
            try
            {
                string mailtoids = "";
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_iem_mst_mailtemplate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@queuegid", SqlDbType.Int).Value = Convert.ToInt32(queuegid);
                cmd.Parameters.Add("@rolegroupname", SqlDbType.VarChar).Value = rolegroupname.ToUpper().Trim();
                cmd.Parameters.Add("@workflow", SqlDbType.Int).Value = Convert.ToInt32(workflow);
                cmd.Parameters.Add("@requesttype", SqlDbType.VarChar).Value = requesttype.ToUpper().Trim();
                cmd.Parameters.Add("@modulename", SqlDbType.VarChar).Value = modulename.ToUpper().Trim();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getrolegroupmailids";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (mailtoids == "")
                        {
                            mailtoids = Convert.ToString(dt.Rows[i]["employee_office_email"].ToString());
                        }
                        else
                        {
                            mailtoids += "," + Convert.ToString(dt.Rows[i]["employee_office_email"].ToString());
                        }
                    }

                }

                return mailtoids.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string mailsentmessage(string mailtemplategid, string refflag, string refgid, string refstatus, string mailalertdate, string to, string cc, string bcc, string subject, string content, string scheduledate, string sendflag, string senddate, string isremoved)
        {
            try
            {
                string[,] codes = new string[,]
	                   {
                        {"mailalert_mailtemplate_gid",mailtemplategid.ToString()},
	                    {"mailalert_ref_flag", refflag.ToString()},
                        {"mailalert_ref_gid",refgid.ToString() },
	                    {"mailalert_ref_status", refstatus.ToString()},
                        {"mailalert_date",mailalertdate.ToString() },
	                    {"mailalert_to", to},
                        {"mailalert_cc",cc},
	                    {"mailalert_bcc", bcc},
                        {"mailalert_subject", subject},
                        {"mailalert_content", content.ToString() },      
                        {"mailalert_schedule_date", scheduledate},
                        {"mailalert_send_flag", sendflag},
                        {"mailalert_send_date", senddate },  
                        {"mailalert_isremoved", "N" },
                  };
                string tname = "iem_mst_tmailalert";
                string insertcommend = objCommonIUD.InsertCommon(codes, tname);
                return insertcommend;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }


        public IEnumerable<MailApproveEntity> Getmailselectdfield(string id)
        {
            try
            {
                List<MailApproveEntity> objExpenseCategory = new List<MailApproveEntity>();
                MailApproveEntity objModel;
                GetConnection();
                DataTable dt = new DataTable();
                string str = "";
                str += "select upper(c.mailfield_name) as mailfield_name,c.mailfield_field_name from iem_mst_tmailtemplate as a";
                str += " inner join iem_mst_tmailtemplatefield as b on a.mailtemplate_gid=b.mailtemplatefield_mailtemplate_gid";
                str += " and b.mailtemplatefield_isremoved='N'";
                str += " inner join iem_mst_tmailfield as c on b.mailtemplatefield_mailfield_gid=c.mailfield_gid";
                str += " and c.mailfield_isremoved='N' and a.mailtemplate_gid='" + id + "'";
                cmd = new SqlCommand(str, con);
                cmd.CommandType = CommandType.Text;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new MailApproveEntity();
                    objModel.ToGetTypeId = Convert.ToString(dt.Rows[i]["mailfield_name"].ToString());
                    objModel.ToGetTypeName = Convert.ToString(dt.Rows[i]["mailfield_field_name"].ToString());
                    objExpenseCategory.Add(objModel);
                }

                return objExpenseCategory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable Selectbyid(string Module, string TransactionType, string gid, string request)
        {
            try
            {

                string tableparamater = "";
                DataTable dt = new DataTable();
                string str = "";
                tableparamater = objCmnFunctions.GetMailtables(TransactionType, Module, request);
                if (tableparamater != "")
                {
                    string[] values = tableparamater.Split(',');

                    GetConnection();
                    str += "select * from " + values[0].ToString() + " where " + values[1].ToString() + "='" + gid + "'";
                    cmd = new SqlCommand(str, con);
                    cmd.CommandType = CommandType.Text;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataTable USERMailIDTo(string Module, string TransactionType, string gid, string request, string isfinalapproval = null)
        {
            try
            {
                string tableparamater = "";
                DataTable dt = new DataTable();
                DataTable dtResult = new DataTable();
                string str = "";
                tableparamater = objCmnFunctions.GetMailtables(TransactionType, Module, request);
                if (tableparamater != "")
                {
                    string[] values = tableparamater.Split(',');

                    GetConnection();
                    str += "select * from " + values[0].ToString() + " where " + values[1].ToString() + "='" + gid + "'";
                    cmd = new SqlCommand(str, con);
                    cmd.CommandType = CommandType.Text;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    isfinalapproval = string.IsNullOrEmpty(isfinalapproval) ? "Y" : isfinalapproval.ToString().Trim();
                    if (dt.Rows.Count > 0)
                    {
                        if (Module == "ASMS")
                        {
                            string flg = Convert.ToString(dt.Rows[0]["queue_to_type"].ToString());
                            string queueto = Convert.ToString(dt.Rows[0]["queue_to"].ToString());
                            string queuefrom = Convert.ToString(dt.Rows[0]["queue_from"].ToString());
                            int queuerefgid = Convert.ToInt32(dt.Rows[0]["queue_ref_gid"].ToString());
                            if (isfinalapproval == "Y" || request == "R")
                            {
                                DataSet dtemp1 = new DataSet();
                                dtResult = new DataTable();
                                GetConnection();
                                cmd = new SqlCommand("pr_iem_mst_mailtemplate", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt32(queuerefgid);
                                cmd.Parameters.Add("@requesttype", SqlDbType.VarChar).Value = TransactionType.ToUpper().Trim();
                                cmd.Parameters.Add("@approvalstatus", SqlDbType.VarChar).Value = request.ToUpper().Trim();
                                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getfinalapprovalmailids";
                                da = new SqlDataAdapter(cmd);
                                da.Fill(dtResult);

                                return dtResult;
                            }
                            else
                            {
                                if (flg == "I")
                                {
                                    DataSet dtemp = new DataSet();
                                    GetConnection();
                                    dtResult = new DataTable();
                                    str = "";
                                    str += "select employee_office_email,employee_code from iem_mst_temployee where employee_isremoved='N' and employee_gid='" + queueto + "';";

                                    cmd = new SqlCommand(str, con);
                                    cmd.CommandType = CommandType.Text;
                                    da = new SqlDataAdapter(cmd);
                                    da.Fill(dtResult);
                                    return dtResult;
                                }
                                else if (flg == "G")
                                {
                                    DataSet dtrole = new DataSet();
                                    dtResult = new DataTable();
                                    GetConnection();
                                    str = "";
                                    str += " select employee_office_email,employee_code from iem_mst_temployee";
                                    str += " inner join iem_mst_troleemployee on roleemployee_employee_gid=employee_gid";
                                    str += " inner join iem_mst_trole on role_gid=roleemployee_role_gid";
                                    str += " inner join iem_mst_trolegroup on rolegroup_gid=role_rolegroup_gid";
                                    str += " where rolegroup_gid=" + queueto + " and employee_isremoved='N' and roleemployee_isremoved='N'";
                                    str += " group by employee_office_email,employee_code";

                                    cmd = new SqlCommand(str, con);
                                    cmd.CommandType = CommandType.Text;
                                    da = new SqlDataAdapter(cmd);
                                    da.Fill(dtResult);
                                    return dtResult;
                                }
                            }

                        }
                    }
                }
                return dtResult;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        public DataTable GetNextApproverLink(string module, string transactiontype, string gid)
        {
            try
            {
                DataTable dt = new DataTable();
                string IsNextApproverLinkAvailable = "N";
                GetConnection();
                cmd = new SqlCommand("pr_iem_mst_mailtemplate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@queuegid", SqlDbType.Int).Value = Convert.ToInt32(gid);
                cmd.Parameters.Add("@requesttype", SqlDbType.VarChar).Value = transactiontype.ToUpper().Trim();
                cmd.Parameters.Add("@modulename", SqlDbType.VarChar).Value = module.ToUpper().Trim();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "nextapproverlink";
                IsNextApproverLinkAvailable = Convert.ToString(cmd.ExecuteScalar());

                if (IsNextApproverLinkAvailable != "N")
                {
                    GetConnection();
                    DataTable dt1 = new DataTable();
                    string lsQry = "select employee_gid,employee_code,employee_name from iem_mst_temployee";
                    lsQry += " inner join iem_mst_troleemployee on roleemployee_employee_gid=employee_gid";
                    lsQry += " inner join iem_mst_trole on role_gid=roleemployee_role_gid";
                    lsQry += " inner join iem_mst_trolegroup on rolegroup_gid=role_rolegroup_gid ";
                    lsQry += " where upper(replace(rolegroup_name,' ',''))=upper(replace('" + IsNextApproverLinkAvailable + "',' ',''))";
                    lsQry += " and rolegroup_isremoved='N' and roleemployee_isremoved='N' and role_isremoved='N'";
                    lsQry += " and employee_isremoved='N' group by employee_gid,employee_code,employee_name";

                    cmd = new SqlCommand(lsQry, con);
                    cmd.CommandType = CommandType.Text;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt1);

                    return dt1;
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetNextApproverLink1(string module, string transactiontype, string gid)
        {
            try
            {
                string IsNextApproverLinkAvailable = "N";
                GetConnection();
                cmd = new SqlCommand("pr_iem_mst_mailtemplate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@queuegid", SqlDbType.Int).Value = Convert.ToInt32(gid);
                cmd.Parameters.Add("@requesttype", SqlDbType.VarChar).Value = transactiontype.ToUpper().Trim();
                cmd.Parameters.Add("@modulename", SqlDbType.VarChar).Value = module.ToUpper().Trim();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "nextapproverlink";
                IsNextApproverLinkAvailable = Convert.ToString(cmd.ExecuteScalar());

                return IsNextApproverLinkAvailable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string GetNextApproverIsMandatory(string TransactionType, string Module, string queuegid)
        {
            try
            {
                string NextApproverIsMandatory = "Y";
                GetConnection();
                cmd = new SqlCommand("pr_iem_mst_mailtemplate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@queuegid", SqlDbType.Int).Value = Convert.ToInt32(queuegid);
                cmd.Parameters.Add("@requesttype", SqlDbType.VarChar).Value = TransactionType.ToUpper().Trim();
                cmd.Parameters.Add("@modulename", SqlDbType.VarChar).Value = Module.ToUpper().Trim();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetNextApproverIsMandatory";
                NextApproverIsMandatory = Convert.ToString(cmd.ExecuteScalar());

                return NextApproverIsMandatory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetCurrentApprovalStage(string TransactionType, string queuegid)
        {
            try
            {
                string CurrentApprovalStage = "Y";
                GetConnection();
                cmd = new SqlCommand("pr_iem_mst_mailtemplate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@queuegid", SqlDbType.Int).Value = Convert.ToInt32(queuegid);
                cmd.Parameters.Add("@requesttype", SqlDbType.VarChar).Value = TransactionType.ToUpper().Trim();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetCurrentApprovalStage";
                CurrentApprovalStage = Convert.ToString(cmd.ExecuteScalar());

                return CurrentApprovalStage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string alreadyapproveby(string Gid)
        {
            string msg = "";
            try
            {
                string queueaction = "";
                DataTable dt = new DataTable();
                GetConnection();
                cmd = new SqlCommand("pr_eow_com_queuedetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@queue_gid", SqlDbType.VarChar).Value = Gid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getqueuemail";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    queueaction = Convert.ToString(dt.Rows[0]["queue_action_status"].ToString());
                    if (queueaction == "0")
                    {
                        msg = "";
                    }
                    else
                    {
                        string queueactions = Convert.ToString(dt.Rows[0]["queue_action_by"].ToString());
                        DataTable dtempsupd = new DataTable();
                        GetConnection();
                        cmd = new SqlCommand("pr_eow_com_empdetails", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@employee_gid", SqlDbType.Int).Value = queueactions;
                        cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "localempdetails";
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dtempsupd);
                        if (dtempsupd.Rows.Count > 0)
                        {
                            msg = "Approval Already Done by " + Convert.ToString(dtempsupd.Rows[0]["employee_code"].ToString()) + "-" + Convert.ToString(dtempsupd.Rows[0]["employee_name"].ToString());
                        }
                    }
                }
                return msg;
            }
            catch (Exception ex)
            {
                return msg;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        public IEnumerable<MailApprovaleowModel> Getecfqueuedetails(string empcode, string queuegid)
        {
            List<MailApprovaleowModel> objparent = new List<MailApprovaleowModel>();
            try
            {
                MailApprovaleowModel objModel;
                GetConnection();
                DataSet dt = new DataSet();
                cmd = new SqlCommand("pr_eow_com_ecfdetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@employee_gid", SqlDbType.VarChar).Value = empcode;
                cmd.Parameters.Add("@ecf_gid", SqlDbType.VarChar).Value = queuegid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Approvermail";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objModel = new MailApprovaleowModel();
                if (dt.Tables[0].Rows.Count > 0)
                {
                    objModel._EcfID = Convert.ToString(dt.Tables[0].Rows[0]["ecf_gid"].ToString());
                }
                else
                {
                    objModel._EcfID = "0";
                }
                if (dt.Tables[1].Rows.Count > 0)
                {
                    objModel._ApproveID = Convert.ToString(dt.Tables[1].Rows[0]["employee_gid"].ToString());

                }
                else
                {
                    objModel._ApproveID = "0";
                }
                objparent.Add(objModel);
                return objparent;
            }
            catch (Exception ex)
            {
                return objparent;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        public string Insertapprovalmail(string ecfgid, string empgid, string queuegid, string approvalsrequest)
        {
            string message = "";
            int EMPdelmattype = Convert.ToInt32(ConfigurationManager.AppSettings["EMPdelmattype"].ToString());
            int SUPdelmattype = Convert.ToInt32(ConfigurationManager.AppSettings["SUPdelmattype"].ToString());
            int EcfInprocess = Convert.ToInt32(ConfigurationManager.AppSettings["EcfInprocess"].ToString());
            int EcfApproved = Convert.ToInt32(ConfigurationManager.AppSettings["EcfApproved"].ToString());
            int ecfHold = Convert.ToInt32(ConfigurationManager.AppSettings["EcfHold"].ToString());
            int EcfConcurrentApproval = Convert.ToInt32(ConfigurationManager.AppSettings["EcfConcurrentApproval"].ToString());
            int EcfRejected = Convert.ToInt32(ConfigurationManager.AppSettings["EcfRejected"].ToString());
            int centralckeckerreject = Convert.ToInt32(ConfigurationManager.AppSettings["EcfCentralreject"].ToString());
            int centralmaker = Convert.ToInt32(ConfigurationManager.AppSettings["Centralmaker"].ToString());
            try
            {
                if (approvalsrequest == "0")
                {
                    string doctyoeandmode = "";
                    DataTable dtrejectcon = new DataTable();
                    GetConnection();
                    cmd = new SqlCommand("pr_eow_com_ecfdetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ecf_gid", SqlDbType.VarChar).Value = ecfgid;
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "TYPEDL";
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dtrejectcon);
                    if (dtrejectcon.Rows.Count > 0)
                    {
                        doctyoeandmode = Convert.ToString(dtrejectcon.Rows[0]["ecf_create_mode"].ToString());

                        if (doctyoeandmode == "S")
                        {
                            string[,] codes = new string[,]
	               {
                       {"ecf_all_status",EcfRejected.ToString() },
                        {"ecf_status",EcfRejected.ToString() },
                  };

                            string[,] whre = new string[,]
	               {
                         {"ecf_gid",ecfgid }
                  };
                            string tname = "iem_trn_tecf";
                            string insertcommend = objCommonIUD.UpdateCommon(codes, whre, tname);

                            string[,] codesq = new string[,]
	               {
                      {"queue_isremoved","Y"},
                      {"queue_action_date","sysdatetime()"},
                      {"queue_action_by",empgid.ToString() },
                      {"queue_action_status",EcfRejected.ToString() },
                      {"queue_action_remark","Reject By Mail"}
                  };
                            string[,] whreq = new string[,]
	               {
                          {"queue_gid",queuegid.ToString() }
                  };
                            string tnameq = "iem_trn_tqueue";
                            string insertcommendq = objCommonIUD.UpdateCommon(codesq, whreq, tnameq);

                            string Emp_Msgsuper = "";
                            DataTable dtempsup = new DataTable();
                            GetConnection();
                            cmd = new SqlCommand("pr_eow_com_queuedetails", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@ref_gid", SqlDbType.VarChar).Value = ecfgid;
                            cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getqueuefromreject";
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dtempsup);
                            if (dtempsup.Rows.Count > 0)
                            {
                                Emp_Msgsuper = Convert.ToString(dtempsup.Rows[0]["queue_from"].ToString());

                                string[,] codesIN = new string[,]
	               {
        {"queue_date","sysdatetime()"},
	    {"queue_ref_flag", "1"},
        {"queue_ref_gid",ecfgid },
	    {"queue_ref_status", EcfRejected.ToString()},
        {"queue_from",empgid },
	    {"queue_to_type", "E"},
        {"queue_to", Emp_Msgsuper.ToString()},
	    {"queue_action_for", "R"},  
        {"queue_prev_gid", queuegid.ToString()}
                  };
                                string tnameIN = "iem_trn_tqueue";

                                string insertcommendecf = objCommonIUD.InsertCommon(codesIN, tnameIN);

                            }
                            string queue_gid = "";
                            GetConnection();
                            DataTable dtempsupnew = new DataTable();
                            cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = empgid;
                            cmd.Parameters.Add("@para2", SqlDbType.VarChar).Value = ecfgid;
                            cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetMaxqueuegid";
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dtempsupnew);
                            if (dtempsupnew.Rows.Count > 0)
                            {
                                queue_gid = Convert.ToString(dtempsupnew.Rows[0]["queue_gid"].ToString());
                            }

                            string mail = queue_gid.ToString();
                            GetConnection();
                            DataTable dtdoctype = new DataTable();
                            cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = mail;
                            cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Getdocsubtype";
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dtdoctype);
                            if (dtdoctype.Rows.Count > 0)
                            {
                                string doctypeid = Convert.ToString(dtdoctype.Rows[0]["docsubtype_gid"].ToString());
                                doctypeid = objCmnFunctions.GetSubDocType(doctypeid);
                                mailsender.sendusermail("EOW", doctypeid, mail, "R", "0");
                            }
                            message = "Sucess";
                            return message;
                        }
                        else
                        {
                            string[,] codes = new string[,]
	               {
                       {"ecf_all_status",centralckeckerreject.ToString() },
                       {"ecf_status",centralckeckerreject.ToString() },
                  };

                            string[,] whre = new string[,]
	               {
                         {"ecf_gid",ecfgid }
                  };
                            string tname = "iem_trn_tecf";
                            string insertcommend = objCommonIUD.UpdateCommon(codes, whre, tname);

                            string Emp_Msgsuper = "";
                            DataTable dtempsup = new DataTable();
                            GetConnection();
                            cmd = new SqlCommand("pr_eow_com_queuedetails", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@ref_gid", SqlDbType.VarChar).Value = ecfgid;
                            cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getqueuefromreject";
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dtempsup);

                            if (dtempsup.Rows.Count > 0)
                            {
                                Emp_Msgsuper = Convert.ToString(dtempsup.Rows[0]["queue_from"].ToString());

                                string[,] codesq = new string[,]
	               {
                      {"queue_prev_gid", "-1"},
                      {"queue_isremoved","Y"},
                      {"queue_action_date","sysdatetime()"},
                      {"queue_action_by",empgid.ToString() },
                      {"queue_action_status",centralckeckerreject.ToString() },
                      {"queue_action_remark","Reject By Mail" }
                  };
                                string[,] whreq = new string[,]
	               {
                          {"queue_gid",queuegid.ToString() }
                  };
                                string tnameq = "iem_trn_tqueue";
                                string insertcommendq = objCommonIUD.UpdateCommon(codesq, whreq, tnameq);

                                string[,] codesIN = new string[,]
	               {
        {"queue_date","sysdatetime()"},
	    {"queue_ref_flag", "1"},
        {"queue_ref_gid",ecfgid },
	    {"queue_ref_status", centralckeckerreject.ToString()},
        {"queue_from",empgid },
	    {"queue_to_type", "R"},
        {"queue_to", centralmaker.ToString()},
	    {"queue_action_for", "R"},  
        {"queue_prev_gid", queuegid.ToString()}
                  };
                                string tnameIN = "iem_trn_tqueue";

                                string insertcommendecf = objCommonIUD.InsertCommon(codesIN, tnameIN);
                            }

                            string[,] codesqctct = new string[,]
	                           {
                      {"centralinward_status","10" }
                              };
                            string[,] whreqctct = new string[,]
	                           {
                    {"centralinward_ecf_gid",ecfgid }
                              };
                            string tnameqctc = "iem_trn_tcentralinward";
                            string insertcommendqc = objCommonIUD.UpdateCommon(codesqctct, whreqctct, tnameqctc);

                            string queue_gid = "";
                            GetConnection();
                            DataTable dtempsupnew = new DataTable();
                            cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = Emp_Msgsuper.ToString();
                            cmd.Parameters.Add("@para2", SqlDbType.VarChar).Value = ecfgid;
                            cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetMaxqueuegid";
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dtempsupnew);
                            if (dtempsupnew.Rows.Count > 0)
                            {
                                queue_gid = Convert.ToString(dtempsupnew.Rows[0]["queue_gid"].ToString());
                            }

                            string mail = queue_gid.ToString();
                            GetConnection();
                            DataTable dtdoctype = new DataTable();
                            cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = mail;
                            cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Getdocsubtype";
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dtdoctype);
                            if (dtdoctype.Rows.Count > 0)
                            {
                                string doctypeid = Convert.ToString(dtdoctype.Rows[0]["docsubtype_gid"].ToString());
                                doctypeid = objCmnFunctions.GetSubDocType(doctypeid);
                                mailsender.sendusermail("EOW", doctypeid, mail, "R", "0");
                            }
                            string approverrcodename = "";
                            DataTable dtempsupd = new DataTable();
                            GetConnection();
                            cmd = new SqlCommand("pr_eow_com_empdetails", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@employee_gid", SqlDbType.Int).Value = empgid;
                            cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "localempdetails";
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dtempsupd);
                            if (dtempsupd.Rows.Count > 0)
                            {
                                approverrcodename = Convert.ToString(dtempsupd.Rows[0]["employee_code"].ToString()) + "-" + Convert.ToString(dtempsupd.Rows[0]["employee_name"].ToString());
                            }

                            message = "Rejected Done by " + approverrcodename;
                            return message;
                        }
                    }
                }
                else
                {
                    GetConnection();
                    cmd = new SqlCommand("pr_ecfdelmat", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ecf_gid", SqlDbType.Int).Value = Convert.ToInt32(ecfgid.ToString());
                    cmd.Parameters.Add("@queue_gid", SqlDbType.Int).Value = Convert.ToInt32(queuegid.ToString());
                    cmd.Parameters.Add("@ecf_approver_gid", SqlDbType.Int).Value = Convert.ToInt32(empgid.ToString());

                    cmd.Parameters.Add("@ecf_next_queue_to_gid", SqlDbType.Int, 64);
                    cmd.Parameters["@ecf_next_queue_to_gid"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@ecf_next_queue_to_type", SqlDbType.Char, 1);
                    cmd.Parameters["@ecf_next_queue_to_type"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@ecf_next_queue_to_additional_flag", SqlDbType.Char, 1);
                    cmd.Parameters["@ecf_next_queue_to_additional_flag"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@ecf_final_flag", SqlDbType.Char, 1, "N");
                    cmd.Parameters["@ecf_final_flag"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@ecfdelmat_result", SqlDbType.Int, 32);
                    cmd.Parameters["@ecfdelmat_result"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@ecf_err_output", SqlDbType.VarChar, 10000);
                    cmd.Parameters["@ecf_err_output"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@ecf_sql_output", SqlDbType.VarChar, 10000);
                    cmd.Parameters["@ecf_sql_output"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    var result = Convert.ToString(cmd.Parameters["@ecf_next_queue_to_gid"].Value);
                    var Flag = Convert.ToString(cmd.Parameters["@ecf_next_queue_to_type"].Value);
                    var Additionalflagnew = Convert.ToString(cmd.Parameters["@ecf_next_queue_to_additional_flag"].Value);
                    var finalapprver = Convert.ToString(cmd.Parameters["@ecf_final_flag"].Value);
                    var demmatresult = Convert.ToString(cmd.Parameters["@ecfdelmat_result"].Value);
                    var sqlerrors = Convert.ToString(cmd.Parameters["@ecf_err_output"].Value);
                    var ecferrors = Convert.ToString(cmd.Parameters["@ecf_sql_output"].Value);
                    string Ecfdesignation = Convert.ToString(ConfigurationManager.AppSettings["Ecfdesignation"].ToString());
                    string Additionalflagnewq = "N";
                    string titlevalues = "";
                    titlevalues = Flag.ToString();
                    string remarks = "";

                    //For ceo claims approved directly by head hr
                    string Emp_designation = "";
                    cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = Convert.ToString(ecfgid);
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetEmpSupperdesc";
                    da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        Emp_designation = dt.Rows[0]["employee_iem_designation"].ToString();
                    }
                    else
                    {
                        Emp_designation = "0";
                    }

                    if ((Emp_designation == "0" || Emp_designation == "" || Emp_designation == null || Emp_designation == " "))
                    {
                        message = "Invalid Approver.";
                        return message;
                    }
                    if (Emp_designation == Ecfdesignation)
                    {
                        demmatresult = "1";
                        result = "0";
                        Flag = "E";
                        finalapprver = "Y";
                    }
                    if (Emp_designation.Trim() == "President")
                    {
                        demmatresult = "1";
                        result = "0";
                        Flag = "E";
                        finalapprver = "Y";
                    }
                    if (result == "0" || result == "" || result == " " || result == null || Emp_designation.Trim() == "President")
                    {
                        objErrorLog.WriteErrorLog("Employee supervisor is empty : Emp_designation=" + Emp_designation.Trim() + "ecfgid =" + ecfgid + "", "Ecfdesignation =" + Ecfdesignation + "Final approver =" + finalapprver + "demmatresult=" + demmatresult);
                    }


                    if (demmatresult != "0" && demmatresult != "")
                    {
                        if (finalapprver != "Y" && (Emp_designation.Trim() != "President"))
                        {
                            if (Additionalflagnew != null)
                            {
                                if (Additionalflagnew.ToString() == "Y")
                                {
                                    Additionalflagnewq = "Y";
                                }
                                else
                                {
                                    Additionalflagnewq = "N";
                                }
                            }

                            string[,] codesq = new string[,]
	                             {
                                      {"queue_isremoved","Y"},
                                      {"queue_action_date","sysdatetime()"},
                                      {"queue_action_by",empgid.ToString() },
                                      {"queue_action_status",EcfApproved.ToString() },
                                      {"queue_action_remark", remarks}
                                  };
                            string[,] whreq = new string[,]
	                             {
                                     {"queue_gid",queuegid.ToString() }
                                };
                            string tnameq = "iem_trn_tqueue";
                            string insertcommendq = objCommonIUD.UpdateCommon(codesq, whreq, tnameq);

                            string[,] codesIN = new string[,]
	                               {
                                        {"queue_date","sysdatetime()"},
	                                    {"queue_ref_flag", "1"},
                                        {"queue_ref_gid",ecfgid },
	                                    {"queue_ref_status", EcfInprocess.ToString()},
                                        {"queue_from",empgid },
	                                    {"queue_to_type", titlevalues.ToString()},
                                        {"queue_to", result.ToString()},
	                                    {"queue_action_for", "A"}, 
                                        {"Additional_flag", Additionalflagnewq.ToString()}, 
                                        {"queue_prev_gid", queuegid.ToString()}
                                  };
                            string tnameIN = "iem_trn_tqueue";

                            string insertcommendecf = objCommonIUD.InsertCommon(codesIN, tnameIN);

                            string[,] codes = new string[,]
                                    {
                                      {"ecf_all_status",EcfApproved.ToString() }
                                    };

                            string[,] whre = new string[,]
                                   {
                                     {"ecf_gid",ecfgid }
                                  };
                            string tname = "iem_trn_tecf";
                            string insertcommend = objCommonIUD.UpdateCommon(codes, whre, tname);
                        }
                        else
                        {
                            string[,] codesq = new string[,]
	                             {
                                      {"queue_isremoved","Y"},
                                      {"queue_action_date","sysdatetime()"},
                                      {"queue_action_by",empgid.ToString() },
                                      {"queue_action_status",EcfApproved.ToString() },
                                      {"queue_ref_status",EcfApproved.ToString() },
                                      {"queue_action_remark", remarks}
                                  };
                            string[,] whreq = new string[,]
	                             {
                                     {"queue_gid",queuegid.ToString() }
                                };
                            string tnameq = "iem_trn_tqueue";
                            string insertcommendq = objCommonIUD.UpdateCommon(codesq, whreq, tnameq);

                            string[,] codes = new string[,]
                                    {
                                      {"ecf_status",EcfApproved.ToString() },
                                      {"ecf_all_status",EcfApproved.ToString() }
                                    };

                            string[,] whre = new string[,]
                                   {
                                     {"ecf_gid",ecfgid }
                                  };
                            string tname
                                = "iem_trn_tecf";
                            string insertcommend = objCommonIUD.UpdateCommon(codes, whre, tname);

                            string[,] codesIN = new string[,]
	                               {
                                        {"queue_date","sysdatetime()"},
	                                    {"queue_ref_flag", "1"},
                                        {"queue_ref_gid",ecfgid },
	                                    {"queue_ref_status", "64"},
                                        {"queue_from",empgid },
	                                    {"queue_to_type","U"},
                                        {"queue_to", "43"},
	                                    {"queue_action_for", "A"}, 
                                        {"Additional_flag", Additionalflagnewq.ToString()}, 
                                        {"queue_prev_gid", queuegid.ToString()}
                                  };
                            string tnameIN = "iem_trn_tqueue";

                            string insertcommendecf = objCommonIUD.InsertCommon(codesIN, tnameIN);

                            GetConnection();
                            cmd = new SqlCommand("pr_eow_com_eowtofsmoveddata", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@ecf_gid", SqlDbType.VarChar).Value = ecfgid;
                            cmd.Parameters.Add("@employee_gid", SqlDbType.VarChar).Value = empgid;
                            cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "INSETINTO";
                            int i = cmd.ExecuteNonQuery();

                        }

                        string queue_gid = "";
                        GetConnection();
                        DataTable dtempsupnew = new DataTable();
                        cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = empgid;
                        cmd.Parameters.Add("@para2", SqlDbType.VarChar).Value = ecfgid;
                        cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetMaxqueuegid";
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dtempsupnew);
                        if (dtempsupnew.Rows.Count > 0)
                        {
                            queue_gid = Convert.ToString(dtempsupnew.Rows[0]["queue_gid"].ToString());
                        }

                        string mail = queue_gid.ToString();
                        GetConnection();
                        DataTable dtdoctype = new DataTable();
                        cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = mail;
                        cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Getdocsubtype";
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dtdoctype);
                        if (dtdoctype.Rows.Count > 0)
                        {
                            string doctypeid = Convert.ToString(dtdoctype.Rows[0]["docsubtype_gid"].ToString());
                            doctypeid = objCmnFunctions.GetSubDocType(doctypeid);
                            mailsender.sendusermail("EOW", doctypeid, mail, "A", "0");
                        }

                        string approverrcodename = "";
                        DataTable dtempsup = new DataTable();
                        GetConnection();
                        cmd = new SqlCommand("pr_eow_com_empdetails", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@employee_gid", SqlDbType.Int).Value = empgid;
                        cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "localempdetails";
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dtempsup);
                        if (dtempsup.Rows.Count > 0)
                        {
                            approverrcodename = Convert.ToString(dtempsup.Rows[0]["employee_code"].ToString()) + "-" + Convert.ToString(dtempsup.Rows[0]["employee_name"].ToString());
                        }

                        message = "Approved Done by " + approverrcodename;
                        return message;
                    }
                    else
                    {
                        message = sqlerrors;
                        return message;
                    }

                }
                return message;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}