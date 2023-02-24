using IEM.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IEM.Areas.MASTERS.Models
{
    public class ForMailDataModel : ForMailRepository
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();

        CmnFunctions objCmnFunctions = new CmnFunctions();
        CommonIUD objCommonIUD = new CommonIUD();

        private void GetConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }
        public string selectdocsubtype(string queuegid, string type)
        {
            string Emp_Msg = "";
            try
            {
                DataTable dt = new DataTable();
                GetConnection();
                cmd = new SqlCommand("pr_eow_com_ecfdetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ecf_gid", SqlDbType.VarChar).Value = queuegid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "TYPEDL";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Emp_Msg = Convert.ToString(dt.Rows[0]["ecf_docsubtype_gid"].ToString());
                }
                return Emp_Msg;
            }
            catch (Exception ex)
            {
                return "1";
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        public DataTable USERMailIDTofs(string ecfgid, string type,string PVId)
        {
            DataTable dtResult = new DataTable();
            string str = "";
            try
            {
                DataSet dtemp = new DataSet();
                GetConnection();
                dtResult = new DataTable();
                if (type == "Payment Alert Employee Claim" || type == "Payment Alert Vendor Claim")
                {
                    str = "select distinct case when  paymenttrans_paymode='EFT' then (select top 1 supplierheader_emailid from asms_trn_tsupplierheader where supplierheader_gid in(";
                    str += "select payrunvoucher_bill_to_gid from iem_trn_tpayrunvoucher where payrunvoucher_gid='" + PVId + "' ))";
                    str += "when paymenttrans_paymode='ERA' then (select top 1 employee_office_email from iem_mst_temployee e ";
                    str += "inner join iem_trn_tecf a on e.employee_gid=a.ecf_raiser and a.ecf_isremoved='N'";
                    str += "Inner JOin iem_trn_tpaymenttrans on a.ecf_gid=paymenttrans_ecf_gid and a.ecf_isremoved='N'";
                    str += "where paymenttrans_payrunvoucher_gid='" + PVId + "')";
                    str += " else '' end as employee_office_email from iem_trn_tpaymenttrans where paymenttrans_payrunvoucher_gid='" + PVId + "'";
                    str += "union all";
                    str += " ";
                    str += "select distinct employee_office_email";
                    str += " from iem_trn_tecf as a";
                    str += " inner join iem_mst_temployee as b on b.employee_gid=a.ecf_raiser";
                    str += " where employee_isremoved='N' and ecf_gid in (select paymenttrans_ecf_gid from iem_trn_tpaymenttrans where paymenttrans_payrunvoucher_gid='" + PVId + "')";
                    cmd = new SqlCommand(str, con);
                    cmd.CommandType = CommandType.Text;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dtResult);
                    return dtResult;
                }
                else
                {
                    str = "";
                    str += "select employee_office_email,employee_code";
                    str += " from iem_trn_tecf as a";
                    str += " inner join iem_mst_temployee as b on b.employee_gid=a.ecf_raiser";
                    str += " where employee_isremoved='N' and ecf_gid='" + ecfgid + "'";
                    cmd = new SqlCommand(str, con);
                    cmd.CommandType = CommandType.Text;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dtResult);
                    return dtResult;
                }
            }
            catch (Exception ex)
            {
                return dtResult;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        public DataTable USERMailIDTocentralteamall(string gid)
        {
            DataTable dtResult = new DataTable();
            try
            {
                GetConnection();
                dtResult = new DataTable();
                cmd = new SqlCommand("pr_eow_trn_teowmail", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ecfgid", SqlDbType.VarChar).Value = gid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "centralteamall";
                da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                return dtResult;
            }
            catch (Exception ex)
            {
                return dtResult;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        public DataTable USERMailIDTocentralteam(string gid)
        {
            DataTable dtResult = new DataTable();
            try
            {
                GetConnection();
                dtResult = new DataTable();
                cmd = new SqlCommand("pr_eow_trn_teowmail", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ecfgid", SqlDbType.VarChar).Value = gid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "centralteammkr";
                da = new SqlDataAdapter(cmd);
                da.Fill(dtResult);
                return dtResult;
            }
            catch (Exception ex)
            {
                return dtResult;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        public DataTable USERMailIDTo(string Module, string TransactionType, string gid, string request, string isfinalapproval = null)
        {
            try
            {
                string tableparamater = "";
                DataTable dt = new DataTable();
                DataTable dtResult = new DataTable();
                string str = ""; string queuerefflg = "";
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
                            //  int queueactionby = Convert.ToInt32(dt.Rows[0]["queue_action_by"].ToString()); 
                            if (isfinalapproval == "Y" || request == "R")
                            {
                                DataSet dtemp1 = new DataSet();
                                dtResult = new DataTable();
                                GetConnection();
                                //str = "";
                                //str += " select employee_office_email,employee_code from iem_mst_temployee where employee_gid in(";
                                //str += " select supplierheader_insert_by from asms_tmp_tsupplierheader where  supplierheader_gid=" + queuerefgid + ") ";
                                //str += " union all ";
                                //str += " select employee_office_email,employee_code from iem_mst_temployee where employee_gid in(";
                                //str += " select supplierheader_owner_gid from asms_tmp_tsupplierheader where  supplierheader_gid=" + queuerefgid + ") ";
                                //cmd = new SqlCommand(str, con);
                                //cmd.CommandType = CommandType.Text;
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
                                    //if (dtemp.Tables[0].Rows.Count > 0)
                                    //{
                                    //    for (int j = 0; j < dtemp.Tables[0].Rows.Count; j++)
                                    //    {
                                    //        if (mailtoid == "")
                                    //        {
                                    //            mailtoid = Convert.ToString(dtemp.Tables[0].Rows[j]["employee_office_email"].ToString());
                                    //        }
                                    //        else
                                    //        {
                                    //            mailtoid += "," + Convert.ToString(dtemp.Tables[0].Rows[j]["employee_office_email"].ToString());
                                    //        }

                                    //    }
                                    //    //mailtoid = Convert.ToString(dtemp.Tables[0].Rows[0]["employee_office_email"].ToString());
                                    //}
                                    ////if (dtemp.Tables[1].Rows.Count > 0)
                                    ////{
                                    ////    mailfromid = Convert.ToString(dtemp.Tables[1].Rows[0]["employee_office_email"].ToString());
                                    ////}
                                    //// HttpContext.Current.Session["USERMailIDFrom"] = mailfromid.ToString();
                                    //// HttpContext.Current.Session["USERMailIDTo"] = mailtoid.ToString();
                                    //Emp_Msg = mailtoid.ToString();
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
                                    //str = "";
                                    //str += "select  distinct(a.employee_office_email) as 'employee_office_email' from iem_mst_temployee as a";
                                    //str += " inner join iem_mst_troleemployee as b on a.employee_gid=b.roleemployee_employee_gid and b.roleemployee_isremoved='N'";
                                    //str += " inner join iem_mst_trole as c on b.roleemployee_role_gid=c.role_gid and c.role_isremoved='N'";
                                    //str += "  inner join iem_mst_trolegroup as d on d.rolegroup_gid=c.role_rolegroup_gid and d.rolegroup_isremoved='N'";
                                    //str += " where  d.rolegroup_gid='" + queueto + "' and a.employee_isremoved='N' ;";
                                    // str += " select employee_office_email from iem_mst_temployee where employee_isremoved='N' and employee_gid='" + queuefrom + "'";
                                    cmd = new SqlCommand(str, con);
                                    cmd.CommandType = CommandType.Text;
                                    da = new SqlDataAdapter(cmd);
                                    da.Fill(dtResult);
                                    //if (dtrole.Tables[0].Rows.Count > 0)
                                    //{
                                    //    for (int i = 0; i < dtrole.Tables[0].Rows.Count; i++)
                                    //    {
                                    //        if (dtrole.Tables[0].Rows[i]["employee_office_email"].ToString() != "")
                                    //        {
                                    //            if (mailtoid == "")
                                    //            {
                                    //                mailtoid = Convert.ToString(dtrole.Tables[0].Rows[i]["employee_office_email"].ToString());
                                    //            }
                                    //            else
                                    //            {
                                    //                mailtoid +=  "," + Convert.ToString(dtrole.Tables[0].Rows[i]["employee_office_email"].ToString());
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                    //if (dtrole.Tables[1].Rows.Count > 0)
                                    //{
                                    //    mailfromid = Convert.ToString(dtrole.Tables[1].Rows[0]["employee_office_email"].ToString());
                                    //}
                                    //HttpContext.Current.Session["USERMailIDFrom"] = mailfromid.ToString();
                                    // HttpContext.Current.Session["USERMailIDTo"] = mailtoid.ToString();
                                    // Emp_Msg = mailtoid.ToString();
                                    return dtResult;
                                }
                            }

                        }
                        if (Module == "FB")
                        {
                            string flg = Convert.ToString(dt.Rows[0]["queue_to_type"].ToString());
                            string queueto = Convert.ToString(dt.Rows[0]["queue_to"].ToString());
                            string queuefrom = Convert.ToString(dt.Rows[0]["queue_from"].ToString());
                            int queuerefgid = Convert.ToInt32(dt.Rows[0]["queue_ref_gid"].ToString());
                            queuerefflg = Convert.ToString(dt.Rows[0]["queue_ref_flag"].ToString());
                            int queueactionby = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[0]["queue_action_by"].ToString()) ? "0" : dt.Rows[0]["queue_action_by"].ToString());
                            if (request == "R")// Reject mail to raiser
                            {
                                DataSet dtemp1 = new DataSet();
                                dtResult = new DataTable();
                                GetConnection();

                                cmd = new SqlCommand("pr_fb_trn_MailProcedure", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@refgid", SqlDbType.Int).Value = Convert.ToInt32(queuerefgid);
                                cmd.Parameters.Add("@refname", SqlDbType.VarChar).Value = TransactionType.ToUpper().Trim();
                                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getfinalapprovalmailids";
                                da = new SqlDataAdapter(cmd);
                                da.Fill(dtResult);

                                return dtResult;
                            }
                            else
                            {
                                string titlename = "";

                                switch (flg)
                                {
                                    case "I":
                                        titlename = "Employee";
                                        break;
                                    case "E":
                                        titlename = "Employee";
                                        break;
                                    case "L":
                                        titlename = "Grade";
                                        break;
                                    case "D":
                                        titlename = "Designation";
                                        break;
                                    case "R":
                                        titlename = "Role";
                                        break;
                                    case "G":
                                        titlename = "Role Group";
                                        break;
                                }

                                GetConnection();
                                cmd = new SqlCommand("pr_chk_employee_hierarchy", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@employee_gid", queuefrom);
                                cmd.Parameters.AddWithValue("@title_name", titlename);
                                cmd.Parameters.AddWithValue("@title_value_gid", queueto);
                                cmd.Parameters.AddWithValue("@chk_employee_gid", 0);
                                cmd.Parameters.Add("@title_employee_gid",SqlDbType.Int,32);
                                cmd.Parameters["@title_employee_gid"].Direction = ParameterDirection.Output;
                                cmd.Parameters.Add("@pr_err_output", SqlDbType.VarChar, 1000);
                                cmd.Parameters["@pr_err_output"].Direction = ParameterDirection.Output;
                                cmd.ExecuteNonQuery();

                                if (flg == "I" || flg == "E")
                                {
                                    DataSet dtemp = new DataSet();
                                    GetConnection();
                                    dtResult = new DataTable();
                                    str = "";
                                    str += "select employee_office_email,employee_code,employee_gid from iem_mst_temployee where employee_isremoved='N' and employee_gid='" + queueto + "';";

                                    cmd = new SqlCommand(str, con);
                                    cmd.CommandType = CommandType.Text;
                                    da = new SqlDataAdapter(cmd);
                                    da.Fill(dtResult);
                                    //if (dtResult.Rows.Count > 0)
                                    //{
                                    //    if (dtResult.Rows[0]["employee_gid"].ToString() == queueto)
                                    //    {
                                    //        dtResult.Clear();
                                    //        return dtResult;
                                    //    }
                                    //    else
                                    //    {
                                    //        return dtResult;
                                    //    }
                                    //}
                                    
                                }
                                else if (flg == "G")
                                {
                                    queueto = Convert.ToString(cmd.Parameters["@title_employee_gid"].Value);
                                    DataSet dtrole = new DataSet();
                                    dtResult = new DataTable();
                                    GetConnection();
                                    str = "";
                                    str += " select employee_office_email,employee_code from iem_mst_temployee";
                                    str += " inner join iem_mst_troleemployee on roleemployee_employee_gid=employee_gid";
                                    str += " inner join iem_mst_trole on role_gid=roleemployee_role_gid";
                                    str += " inner join iem_mst_trolegroup on rolegroup_gid=role_rolegroup_gid";
                                    str += " where rolegroup_gid=" + queueto + " and employee_isremoved='N' and roleemployee_isremoved='N'";
                                    str += " and role_isremoved='N' and rolegroup_isremoved='N' ";
                                    str += " group by employee_office_email,employee_code";

                                    cmd = new SqlCommand(str, con);
                                    cmd.CommandType = CommandType.Text;
                                    da = new SqlDataAdapter(cmd);
                                    da.Fill(dtResult);
                                    return dtResult;
                                }
                                else if (flg == "R")
                                {
                                    if (queuerefflg == "9" && queueto == "16") // PAR Checker queue
                                    {
                                        //queueto = Convert.ToString(cmd.Parameters["@title_employee_gid"].Value);
                                        DataSet dtrole = new DataSet();
                                        dtResult = new DataTable();
                                        GetConnection();
                                        str = "";
                                        str += " select employee_office_email,employee_code from iem_mst_temployee";
                                        str += " inner join iem_mst_troleemployee on roleemployee_employee_gid=employee_gid";
                                        str += " inner join iem_mst_trole on role_gid=roleemployee_role_gid";
                                        str += " inner join iem_mst_trolegroup on rolegroup_gid=role_rolegroup_gid";
                                        str += " where role_gid=" + queueto + " and employee_isremoved='N' and roleemployee_isremoved='N'";
                                        str += " and role_isremoved='N' and rolegroup_isremoved='N' ";
                                        str += " group by employee_office_email,employee_code";
                                    }
                                    else if (queuerefflg == "8" && queueto == "22") // PO Checker queue
                                    {
                                        DataSet dtrole = new DataSet();
                                        dtResult = new DataTable();
                                        GetConnection();
                                        str = "";
                                        str += " select employee_office_email,employee_code from iem_mst_temployee";
                                        str += " inner join iem_mst_troleemployee on roleemployee_employee_gid=employee_gid";
                                        str += " inner join iem_mst_trole on role_gid=roleemployee_role_gid";
                                        str += " inner join iem_mst_trolegroup on rolegroup_gid=role_rolegroup_gid";
                                        str += " where role_gid=" + queueto + " and employee_isremoved='N' and roleemployee_isremoved='N'";
                                        str += " and role_isremoved='N' and rolegroup_isremoved='N' ";
                                        str += " group by employee_office_email,employee_code";
                                    }
                                    else if (queuerefflg == "10" && queueto == "27") // WO Checker queue
                                    {
                                        DataSet dtrole = new DataSet();
                                        dtResult = new DataTable();
                                        GetConnection();
                                        str = "";
                                        str += " select employee_office_email,employee_code from iem_mst_temployee";
                                        str += " inner join iem_mst_troleemployee on roleemployee_employee_gid=employee_gid";
                                        str += " inner join iem_mst_trole on role_gid=roleemployee_role_gid";
                                        str += " inner join iem_mst_trolegroup on rolegroup_gid=role_rolegroup_gid";
                                        str += " where role_gid=" + queueto + " and employee_isremoved='N' and roleemployee_isremoved='N'";
                                        str += " and role_isremoved='N' and rolegroup_isremoved='N' ";
                                        str += " group by employee_office_email,employee_code";
                                    }
                                    else
                                    {
                                        queueto = Convert.ToString(cmd.Parameters["@title_employee_gid"].Value);
                                        DataSet dtrole = new DataSet();
                                        dtResult = new DataTable();
                                        GetConnection();
                                        str = "";
                                        str += " select employee_office_email,employee_code from iem_mst_temployee";
                                        str += " inner join iem_mst_troleemployee on roleemployee_employee_gid=employee_gid";
                                        str += " inner join iem_mst_trole on role_gid=roleemployee_role_gid";
                                        str += " inner join iem_mst_trolegroup on rolegroup_gid=role_rolegroup_gid";
                                        str += " where role_gid=" + queueto + " and employee_isremoved='N' and roleemployee_isremoved='N'";
                                        str += " and role_isremoved='N' and rolegroup_isremoved='N' ";
                                        str += " group by employee_office_email,employee_code";
                                    }

                                    cmd = new SqlCommand(str, con);
                                    cmd.CommandType = CommandType.Text;
                                    da = new SqlDataAdapter(cmd);
                                    da.Fill(dtResult);
                                    return dtResult;
                                }
                                else if (flg == "L")
                                {
                                    queueto = Convert.ToString(cmd.Parameters["@title_employee_gid"].Value);
                                    DataSet dtrole = new DataSet();
                                    dtResult = new DataTable();
                                    GetConnection();
                                    str = "";
                                    str += " select employee_office_email,employee_code from iem_mst_temployee";
                                    str += " inner join iem_mst_tgrade on grade_gid=employee_grade_gid ";
                                    str += " where grade_gid=" + queueto + " and employee_isremoved='N' and grade_isremoved='N' ";
                                    str += " group by employee_office_email,employee_code";

                                    cmd = new SqlCommand(str, con);
                                    cmd.CommandType = CommandType.Text;
                                    da = new SqlDataAdapter(cmd);
                                    da.Fill(dtResult);
                                    return dtResult;
                                }
                                else if (flg == "D")
                                {
                                    queueto = Convert.ToString(cmd.Parameters["@title_employee_gid"].Value);
                                    DataSet dtrole = new DataSet();
                                    dtResult = new DataTable();
                                    GetConnection();
                                    str = "";
                                    str += " select employee_office_email,employee_code from iem_mst_temployee";
                                    str += " inner join iem_mst_tdesignation on designation_gid=employee_iem_designation_gid ";
                                    str += " where designation_gid=" + queueto + " and employee_isremoved='N' and designation_isremoved='N' ";
                                    str += " group by employee_office_email,employee_code";

                                    cmd = new SqlCommand(str, con);
                                    cmd.CommandType = CommandType.Text;
                                    da = new SqlDataAdapter(cmd);
                                    da.Fill(dtResult);
                                    return dtResult;
                                }

                            }
                        }
                        if (Module == "EOW")
                        {
                            string flg = Convert.ToString(dt.Rows[0]["queue_to_type"].ToString());
                            string queueto = Convert.ToString(dt.Rows[0]["queue_to"].ToString());
                            string queuefrom = Convert.ToString(dt.Rows[0]["queue_from"].ToString());
                            string raiser = Convert.ToString(dt.Rows[0]["ecf_raiser"].ToString());
                            string Additionalflag = Convert.ToString(dt.Rows[0]["Additional_flag"].ToString());
                            int queuerefgid = Convert.ToInt32(dt.Rows[0]["queue_ref_gid"].ToString());
                            int queueactionby = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[0]["queue_action_by"].ToString()) ? "0" : dt.Rows[0]["queue_action_by"].ToString());
                            if (queueactionby != 0)
                            {
                                DataSet dtemp1 = new DataSet();
                                dtResult = new DataTable();
                                GetConnection();

                                cmd = new SqlCommand("pr_fb_trn_MailProcedure", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@refgid", SqlDbType.Int).Value = Convert.ToInt32(queuerefgid);
                                cmd.Parameters.Add("@refname", SqlDbType.VarChar).Value = TransactionType.ToUpper().Trim();
                                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getfinalapprovalmailids";
                                da = new SqlDataAdapter(cmd);
                                da.Fill(dtResult);

                                return dtResult;
                            }
                            else
                            {

                                if (flg == "E")
                                {
                                    DataSet dtemp = new DataSet();
                                    GetConnection();
                                    dtResult = new DataTable();
                                    //str = "";
                                    //str += "select employee_office_email,employee_code from iem_mst_temployee where employee_isremoved='N' and employee_gid='" + queueto + "';";

                                    //cmd = new SqlCommand(str, con);
                                    //cmd.CommandType = CommandType.Text;
                                    //da = new SqlDataAdapter(cmd);
                                    //da.Fill(dtResult);

                                    cmd = new SqlCommand("pr_eow_trn_teowmail", con);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@ecf_raiser", SqlDbType.VarChar).Value = queueto;
                                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "employeebasedmail";
                                    da = new SqlDataAdapter(cmd);
                                    da.Fill(dtResult);

                                    return dtResult;
                                }
                                else if (flg == "R")
                                {
                                    if (Additionalflag == "Y")
                                    {
                                        DataSet dtrole = new DataSet();
                                        dtResult = new DataTable();
                                        GetConnection();
                                        //str = "";
                                        //str += " select employee_office_email,employee_code from iem_mst_temployee";
                                        //str += " inner join iem_mst_troleemployee on roleemployee_employee_gid=employee_gid";
                                        //str += " inner join iem_mst_trole on role_gid=roleemployee_role_gid";
                                        //str += " inner join iem_mst_trolegroup on rolegroup_gid=role_rolegroup_gid";
                                        //str += " where rolegroup_gid=" + queueto + " and employee_isremoved='N' and roleemployee_isremoved='N'";
                                        //str += " and role_isremoved='N' and rolegroup_isremoved='N' ";
                                        //str += " group by employee_office_email,employee_code";

                                        //cmd = new SqlCommand(str, con);
                                        //cmd.CommandType = CommandType.Text;
                                        //da = new SqlDataAdapter(cmd);
                                        //da.Fill(dtResult);

                                        cmd = new SqlCommand("pr_eow_trn_teowmail", con);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@ecfgid", SqlDbType.VarChar).Value = gid;
                                        cmd.Parameters.Add("@ecf_raiser", SqlDbType.VarChar).Value = queueto;
                                        cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "rolebasedmail";
                                        da = new SqlDataAdapter(cmd);
                                        da.Fill(dtResult);
                                        return dtResult;
                                    }
                                    else
                                    {
                                        string getempgid = Getempheryname(raiser, "Role Group", queueto);
                                        if (getempgid != "0")
                                        {
                                            DataSet dtemp = new DataSet();
                                            GetConnection();
                                            dtResult = new DataTable();
                                            //str = "";
                                            //str += "select employee_office_email,employee_code from iem_mst_temployee where employee_isremoved='N' and employee_gid='" + getempgid + "';";

                                            //cmd = new SqlCommand(str, con);
                                            //cmd.CommandType = CommandType.Text;
                                            //da = new SqlDataAdapter(cmd);
                                            //da.Fill(dtResult);

                                            cmd = new SqlCommand("pr_eow_trn_teowmail", con);
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.Add("@ecf_raiser", SqlDbType.VarChar).Value = getempgid;
                                            cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "employeebasedmail";
                                            da = new SqlDataAdapter(cmd);
                                            da.Fill(dtResult);
                                            return dtResult;
                                        }
                                    }
                                }

                                else if (flg == "G")
                                {
                                    if (Additionalflag == "Y")
                                    {
                                        DataSet dtrole = new DataSet();
                                        dtResult = new DataTable();
                                        GetConnection();
                                        //str = "";
                                        //str += " select employee_office_email,employee_code from iem_mst_temployee";
                                        //str += " inner join iem_mst_tgrade on grade_gid=employee_grade_gid ";
                                        //str += " where grade_gid=" + queueto + " and employee_isremoved='N' and grade_isremoved='N' ";
                                        //str += " group by employee_office_email,employee_code";

                                        //cmd = new SqlCommand(str, con);
                                        //cmd.CommandType = CommandType.Text;
                                        //da = new SqlDataAdapter(cmd);
                                        //da.Fill(dtResult);


                                        cmd = new SqlCommand("pr_eow_trn_teowmail", con);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@ecf_raiser", SqlDbType.VarChar).Value = queueto;
                                        cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "gradebasedmail";
                                        da = new SqlDataAdapter(cmd);
                                        da.Fill(dtResult);
                                        return dtResult;
                                    }
                                    else
                                    {
                                        string getempgid = Getempheryname(raiser, "Grade", queueto);
                                        if (getempgid != "0")
                                        {
                                            DataSet dtemp = new DataSet();
                                            GetConnection();
                                            dtResult = new DataTable();
                                            //str = "";
                                            //str += "select employee_office_email,employee_code from iem_mst_temployee where employee_isremoved='N' and employee_gid='" + getempgid + "';";

                                            //cmd = new SqlCommand(str, con);
                                            //cmd.CommandType = CommandType.Text;
                                            //da = new SqlDataAdapter(cmd);
                                            //da.Fill(dtResult);

                                            cmd = new SqlCommand("pr_eow_trn_teowmail", con);
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.Add("@ecf_raiser", SqlDbType.VarChar).Value = getempgid;
                                            cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "employeebasedmail";
                                            da = new SqlDataAdapter(cmd);
                                            da.Fill(dtResult);

                                            return dtResult;
                                        }
                                    }
                                }
                                else if (flg == "D")
                                {
                                    if (Additionalflag == "Y")
                                    {
                                        DataSet dtrole = new DataSet();
                                        dtResult = new DataTable();
                                        GetConnection();
                                        //str = "";
                                        //str += " select employee_office_email,employee_code from iem_mst_temployee";
                                        //str += " inner join iem_mst_tdesignation on designation_gid=employee_iem_designation_gid ";
                                        //str += " where designation_gid=" + queueto + " and employee_isremoved='N' and designation_isremoved='N' ";
                                        //str += " group by employee_office_email,employee_code";

                                        //cmd = new SqlCommand(str, con);
                                        //cmd.CommandType = CommandType.Text;
                                        //da = new SqlDataAdapter(cmd);
                                        //da.Fill(dtResult);

                                        cmd = new SqlCommand("pr_eow_trn_teowmail", con);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@ecf_raiser", SqlDbType.VarChar).Value = queueto;
                                        cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "dedcbasedmail";
                                        da = new SqlDataAdapter(cmd);
                                        da.Fill(dtResult);
                                        return dtResult;
                                    }
                                    else
                                    {
                                        string getempgid = Getempheryname(raiser, "Designation", queueto);
                                        if (getempgid != "0")
                                        {
                                            DataSet dtemp = new DataSet();
                                            GetConnection();
                                            dtResult = new DataTable();
                                            //str = "";
                                            //str += "select employee_office_email,employee_code from iem_mst_temployee where employee_isremoved='N' and employee_gid='" + getempgid + "';";

                                            //cmd = new SqlCommand(str, con);
                                            //cmd.CommandType = CommandType.Text;
                                            //da = new SqlDataAdapter(cmd);
                                            //da.Fill(dtResult);

                                            cmd = new SqlCommand("pr_eow_trn_teowmail", con);
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.Add("@ecf_raiser", SqlDbType.VarChar).Value = getempgid;
                                            cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "employeebasedmail";
                                            da = new SqlDataAdapter(cmd);
                                            da.Fill(dtResult);
                                            return dtResult;
                                        }
                                    }
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

        public string Getempheryname(string empgid, string titlevalue, string titlegid)
        {
            string status = "";
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_chk_employee_hierarchy", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@employee_gid", SqlDbType.Int).Value = Convert.ToInt32(empgid.ToString());
                cmd.Parameters.Add("@title_value_gid", SqlDbType.Int).Value = Convert.ToInt32(titlegid.ToString());
                cmd.Parameters.Add("@title_name", SqlDbType.VarChar).Value = titlevalue.ToString();

                cmd.Parameters.Add("@title_employee_gid", SqlDbType.Int, 64);
                cmd.Parameters["@title_employee_gid"].Direction = ParameterDirection.Output;

                cmd.Parameters.Add("@pr_err_output", SqlDbType.VarChar, 10000);
                cmd.Parameters["@pr_err_output"].Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                var result = Convert.ToString(cmd.Parameters["@title_employee_gid"].Value);
                var sqlerrors = Convert.ToString(cmd.Parameters["@pr_err_output"].Value);
                status = result.ToString();
                return status;
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

        public IEnumerable<ForMailEntity> SelectSupplierSearch(string SupplierName, string SupplierCode)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                List<ForMailEntity> emp = new List<ForMailEntity>();
                ForMailEntity objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SUPLIERNAME", SqlDbType.VarChar).Value = SupplierName;
                cmd.Parameters.Add("@SUPPLIERCODE", SqlDbType.VarChar).Value = SupplierCode;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTSUPPLIERDETAILSSEARCH";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new ForMailEntity();
                    objModel.employeeGid = Convert.ToInt32(row["supplierheader_gid"].ToString());
                    objModel.empCode = row["supplierheader_suppliercode"].ToString();
                    objModel.empName = row["supplierheader_name"].ToString();
                    emp.Add(objModel);
                }
                return emp;
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

        public string selectfcccval(string fc, string cc)
        {
            string status = "";
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_iem_trn_mailckeck", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@modulecode", SqlDbType.VarChar).Value = fc.ToString();
                cmd.Parameters.Add("@mailtypename", SqlDbType.VarChar).Value = cc.ToString();
                cmd.Parameters.Add("@Result", SqlDbType.VarChar).Value = "Empfccc";
                status = (string)cmd.ExecuteScalar();
                return status;
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

        public IEnumerable<ForMailEntity> GetTables()
        {
            List<ForMailEntity> objNatureofExpenses = new List<ForMailEntity>();
            ForMailEntity objModel;

            GetConnection();
            DataTable dt = new DataTable();
            string str = "";
            str += " SELECT distinct TABLE_NAME";
            str += " FROM INFORMATION_SCHEMA.COLUMNS";
            str += " WHERE TABLE_NAME like 'iem_mst_t%' or ";
            str += " TABLE_NAME like 'asms_mst_t%' or";
            str += " TABLE_NAME like 'fb_mst_t%'";
            cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objModel = new ForMailEntity();
                objModel.TablesId = Convert.ToString(dt.Rows[i]["TABLE_NAME"].ToString());
                objModel.TablesName = Convert.ToString(dt.Rows[i]["TABLE_NAME"].ToString());
                objNatureofExpenses.Add(objModel);
            }
            return objNatureofExpenses;
        }

        public string selectpolicy(string Gid)
        {
            try
            {
                string insertcommend = "";
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("select expsubcat_help from iem_mst_texpsubcat where expsubcat_isremoved='N' and expsubcat_gid='" + Gid + "'", con);
                cmd.CommandType = CommandType.Text;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    insertcommend = Convert.ToString(dt.Rows[0]["expsubcat_help"].ToString());
                }
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

        public IEnumerable<ForMailEntity> SelectMailalertbyid(string idnew)
        {
            try
            {
                List<ForMailEntity> objExpenseCategory = new List<ForMailEntity>();
                ForMailEntity objModel;
                GetConnection();
                DataTable dt = new DataTable();
                string str = "";
                str += " select b.mailtype_gid,b.mailtype_table_aname,a.mailtemplate_template_name,b.mailtype_modulecode,a.mailtemplate_mailtype_gid,a.mailtemplate_mailtype_name";
                str += " ,a.mailtemplate_trigger_on,a.mailtemplate_trigger_mode,a.mailtemplate_trigger_days,";
                str += " a.mailtemplate_to,a.mailtemplate_cc,a.mailtemplate_bcc,b.mailtype_gid,a.mailtemplate_subject,a.mailtemplate_attachment";
                str += " ,a.mailtemplate_include_approval,a.mailtemplate_workflow,a.mailtemplate_audience,a.mailtemplate_content,a.mailtemplate_sign";
                str += " from iem_mst_tmailtemplate as a";
                str += " inner join iem_mst_tmailtype as b on a.mailtemplate_mailtype_gid=b.mailtype_gid and b.mailtype_isremoved='N'";
                str += " where  a.mailtemplate_gid='" + idnew.ToString() + "' and a.mailtemplate_isremoved='N' ";
                cmd = new SqlCommand(str, con);
                cmd.CommandType = CommandType.Text;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ForMailEntity();
                    objModel.TemplateName = Convert.ToString(dt.Rows[i]["mailtemplate_template_name"].ToString());
                    objModel.ModuleName = Convert.ToString(dt.Rows[i]["mailtype_modulecode"].ToString());
                    objModel.TransactionTypeId = Convert.ToString(dt.Rows[i]["mailtemplate_mailtype_gid"].ToString());
                    objModel.TransactionTypeName = Convert.ToString(dt.Rows[i]["mailtemplate_mailtype_name"].ToString());
                    objModel.TriggerTypeName = Convert.ToString(dt.Rows[i]["mailtemplate_trigger_on"].ToString());
                    objModel.WorkFlowId = Convert.ToString(dt.Rows[i]["mailtemplate_workflow"].ToString());
                    objModel.ToGetTypeName = Convert.ToString(dt.Rows[i]["mailtemplate_mailtype_name"].ToString());
                    objModel.TablesName = Convert.ToString(dt.Rows[i]["mailtype_gid"].ToString());
                    objModel.TablescolName = Convert.ToString(dt.Rows[i]["mailtype_table_aname"].ToString());
                    objModel.Triggernodays = Convert.ToString(dt.Rows[i]["mailtemplate_trigger_days"].ToString());
                    objModel.TriggerTypeFreName = Convert.ToString(dt.Rows[i]["mailtemplate_trigger_mode"].ToString());
                    objModel.AudienceName = Convert.ToString(dt.Rows[i]["mailtemplate_audience"].ToString());
                    objModel.Includeflg = Convert.ToString(dt.Rows[i]["mailtemplate_include_approval"].ToString());
                    objModel.AttachmentName = Convert.ToString(dt.Rows[i]["mailtemplate_attachment"].ToString());
                    objModel.Subject = Convert.ToString(dt.Rows[i]["mailtemplate_subject"].ToString());
                    objModel.Content = Convert.ToString(dt.Rows[i]["mailtemplate_content"].ToString());
                    objModel.TOid = Convert.ToString(dt.Rows[i]["mailtemplate_to"].ToString());
                    objModel.BCCid = Convert.ToString(dt.Rows[i]["mailtemplate_bcc"].ToString());
                    objModel.CCid = Convert.ToString(dt.Rows[i]["mailtemplate_cc"].ToString());
                    objModel.Signature = Convert.ToString(dt.Rows[i]["mailtemplate_sign"].ToString());
                    objExpenseCategory.Add(objModel);
                }

                return objExpenseCategory;
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

        public string selectfccc(string fc, string cc)
        {
            string Emp_Msg = "";
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("select fccc_name from iem_mst_tfccc where fccc_fc_code='" + fc + "' and fccc_cc_code='" + cc + "' and fccc_isremoved='N'", con);
                cmd.CommandType = CommandType.Text;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Emp_Msg = Convert.ToString(dt.Rows[0]["fccc_name"].ToString());
                }
                return Emp_Msg;
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

        public IEnumerable<ForMailEntity> getuserfunction(string transactiontype, string modulename)
        {
            //List<ForMailEntity> objExpenseCategory = new List<ForMailEntity>();
            //ForMailEntity objModel;
            //GetConnection();
            //DataTable dt = new DataTable();
            //cmd = new SqlCommand("select distinct UPPER(approvalstage_name) AS  approvalstage_name from asms_trn_tapprovalstage where approvalstage_for='" + id + "'", con);
            //cmd.CommandType = CommandType.Text;
            //da = new SqlDataAdapter(cmd);
            //da.Fill(dt);
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    objModel = new ForMailEntity();
            //    objModel.ToGetTypeId = Convert.ToString(dt.Rows[i]["approvalstage_name"].ToString());
            //    objModel.ToGetTypeName = Convert.ToString(dt.Rows[i]["approvalstage_name"].ToString());
            //    objExpenseCategory.Add(objModel);
            //}

            //return objExpenseCategory;
            List<ForMailEntity> objExpenseCategory = new List<ForMailEntity>();
            ForMailEntity objModel;
            GetConnection();
            DataTable dt = new DataTable();
            string lsQry = "select workflow_name ";
            lsQry += "from iem_mst_tworkflow ";
            lsQry += "where upper(workflow_requesttype)='" + transactiontype.ToUpper().Trim() + "' ";
            lsQry += "and upper(workflow_module)='" + modulename.ToUpper().Trim() + "' ";
            cmd = new SqlCommand(lsQry, con);
            cmd.CommandType = CommandType.Text;
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objModel = new ForMailEntity();
                objModel.ToGetTypeId = Convert.ToString(dt.Rows[i]["workflow_name"].ToString());
                objModel.ToGetTypeName = Convert.ToString(dt.Rows[i]["workflow_name"].ToString());
                objExpenseCategory.Add(objModel);
            }
            return objExpenseCategory;
        }

        public IEnumerable<ForMailEntity> GetTablescol(string tblname)
        {
            List<ForMailEntity> objNatureofExpenses = new List<ForMailEntity>();
            ForMailEntity objModel;

            GetConnection();
            DataTable dt = new DataTable();
            string str = "";
            //str += " SELECT  a.storagecolumn_Name,a.storagecolumn_fieldname FROM iem_mst_trptstoragecolumn as a ";
            //str += " inner join iem_mst_trptstorage as b on b.rptstorage_gid=a.storagecolumn_rptstorageid";
            //str += " where a.storagecolumn_ispartindisplay='Y' and  a.storagecolumn_isremoved='N'";
            //str += " and b.rptstorage_Name='" + tblname + "'";
            str += " select a.mailfield_gid,UPPER(mailfield_name) as mailfield_name,mailfield_field_name from iem_mst_tmailfield as a ";
            str += " inner join iem_mst_tmailtype as b on b.mailtype_gid=a.mailfield_mailtype_gid ";
            str += " where b.mailtype_gid='" + tblname + "' and a.mailfield_isremoved='N' AND B.mailtype_isremoved='N'";
            cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objModel = new ForMailEntity();
                objModel.TablesId = Convert.ToString(dt.Rows[i]["mailfield_gid"].ToString());
                objModel.TablesName = Convert.ToString(dt.Rows[i]["mailfield_name"].ToString());
                objNatureofExpenses.Add(objModel);
            }
            return objNatureofExpenses;
        }

        public IEnumerable<ForMailEntity> GetModuleType()
        {
            List<ForMailEntity> objNatureofExpenses = new List<ForMailEntity>();
            ForMailEntity objModel;
            objNatureofExpenses.Add(new ForMailEntity { ModuleId = "0", ModuleName = "-- Select --", });
            GetConnection();
            DataTable dt = new DataTable();
            string str = "";
            str += "  select module_code,module_name from iem_mst_tmodule";
            str += " where module_isremoved='N'";
            cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objModel = new ForMailEntity();
                objModel.ModuleId = Convert.ToString(dt.Rows[i]["module_code"].ToString());
                objModel.ModuleName = Convert.ToString(dt.Rows[i]["module_code"].ToString());
                objNatureofExpenses.Add(objModel);
            }
            return objNatureofExpenses;
        }

        public IEnumerable<ForMailEntity> GetTransactionType(string modulename)
        {
            List<ForMailEntity> objNatureofExpenses = new List<ForMailEntity>();
            ForMailEntity objModel;
            GetConnection();
            DataTable dt = new DataTable();
            string str = "";
            //str += " select distinct approvalstage_for from asms_trn_tapprovalstage ";
            //str += " where approvalstage_modulename='" + modulename + "'";
            str += " SELECT mailtype_gid,mailtype_code,UPPER(mailtype_name) as mailtype_name FROM iem_mst_tmailtype";
            str += " WHERE mailtype_modulecode='" + modulename + "' AND mailtype_isremoved='N'";
            cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objModel = new ForMailEntity();
                objModel.TransactionTypeId = Convert.ToString(dt.Rows[i]["mailtype_gid"].ToString());
                objModel.TransactionTypeName = Convert.ToString(dt.Rows[i]["mailtype_name"].ToString());
                objNatureofExpenses.Add(objModel);
            }
            return objNatureofExpenses;
        }

        public IEnumerable<ForMailEntity> getuserdata(string id)
        {
            List<ForMailEntity> objExpenseCategory = new List<ForMailEntity>();
            ForMailEntity objModel;
            GetConnection();
            DataTable dt = new DataTable();
            //cmd = new SqlCommand("SELECT  rptstorage_Name,rptstorage_Tablename FROM iem_mst_trptstorage where rptstorage_Modulecode='" + id + "'", con);
            cmd = new SqlCommand("SELECT mailtype_gid,mailtype_table_name,mailtype_table_aname FROM iem_mst_tmailtype WHERE mailtype_gid='" + id + "' AND mailtype_isremoved='N'", con);
            cmd.CommandType = CommandType.Text;
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objModel = new ForMailEntity();
                objModel.TablescolId = Convert.ToString(dt.Rows[i]["mailtype_gid"].ToString());
                objModel.TablescolName = Convert.ToString(dt.Rows[i]["mailtype_table_aname"].ToString());
                objExpenseCategory.Add(objModel);
            }

            return objExpenseCategory;
        }


        public string Insertmailtemplate(ForMailEntity Mailalertobj)
        {
            string Triggernodays = "";
            string AttachmentName = "";
            string Includeflg = "";
            string Emp_MsgretnBCC = "";
            string Emp_MsgretnCC = "";
            string Emp_Msg = "";
            string result = "";
            string Emp_Msgretn = "";
            string stagesapp = "";
            try
            {
                //status = GetStatusexcel(EmployeeeExpense.Exp_FC.ToString(), "FunctionCode");
                //if (status == "notexists")
                //{
                //    Emp_Msgretn = "Invalid Function Code";
                //    return Emp_Msgretn;
                //}
                if (Mailalertobj.ModuleName == "FS")
                {
                    if (Mailalertobj.WorkFlowId == null)
                    {
                        Mailalertobj.WorkFlowId = "0";
                    }
                }
                if (Mailalertobj.ModuleName == "EOW")
                {
                    if (Mailalertobj.WorkFlowId == null)
                    {
                        Mailalertobj.WorkFlowId = "0";
                    }
                }
                GetConnection();
                cmd = new SqlCommand("pr_iem_trn_mailckeck", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@modulecode", SqlDbType.VarChar).Value = Mailalertobj.ModuleName.ToString();
                cmd.Parameters.Add("@mailtypename", SqlDbType.VarChar).Value = Mailalertobj.TransactionTypeName.ToString();
                cmd.Parameters.Add("@workflow", SqlDbType.Int).Value = Convert.ToInt32(Mailalertobj.WorkFlowId.ToString());
                cmd.Parameters.Add("@triggeron", SqlDbType.VarChar).Value = Mailalertobj.TriggerTypeName.ToString();
                cmd.Parameters.Add("@Result", SqlDbType.VarChar).Value = "Maildup";
                result = (string)cmd.ExecuteScalar();
                if (result == "notexists")
                {
                    if (Mailalertobj.Triggernodays == null)
                    {
                        Triggernodays = "";
                    }
                    else
                    {
                        Triggernodays = Mailalertobj.Triggernodays;
                    }
                    if (Mailalertobj.AttachmentName == "Yes")
                    {
                        AttachmentName = "Y";
                    }
                    else
                    {
                        AttachmentName = "N";
                    }
                    if (Mailalertobj.Includeflg == "Yes")
                    {
                        Includeflg = "Y";
                    }
                    else
                    {
                        Includeflg = "N";
                    }
                    if (Mailalertobj.CCid == null)
                    {
                        Emp_MsgretnCC = "";
                    }
                    else
                    {
                        Emp_MsgretnCC = Mailalertobj.CCid;
                    }
                    if (Mailalertobj.BCCid == null)
                    {
                        Emp_MsgretnBCC = "";
                    }
                    else
                    {
                        Emp_MsgretnBCC = Mailalertobj.BCCid;
                    }
                    if (Mailalertobj.WorkFlowId == null)
                    {
                        stagesapp = "";
                    }
                    else
                    {
                        stagesapp = Mailalertobj.WorkFlowId;
                    }
                    string[,] codes = new string[,]
	               {
        {"mailtemplate_mailtype_gid",Mailalertobj. TransactionTypeId },
        {"mailtemplate_mailtype_name", Mailalertobj.TransactionTypeName},
        {"mailtemplate_workflow", stagesapp},
        {"mailtemplate_template_name", Mailalertobj.TemplateName},
        {"mailtemplate_trigger_on",Mailalertobj.TriggerTypeName },
        {"mailtemplate_trigger_mode", Mailalertobj.TriggerTypeFreName},
        {"mailtemplate_trigger_days",Triggernodays },
	    {"mailtemplate_audience", Mailalertobj.AudienceName},
        {"mailtemplate_to",Mailalertobj.TOid},
        {"mailtemplate_cc",Emp_MsgretnCC.ToString() },
        {"mailtemplate_bcc", Emp_MsgretnBCC.ToString()},
        {"mailtemplate_mailtable_name", Mailalertobj.TablesName},
        {"mailtemplate_include_approval", Includeflg},
        {"mailtemplate_subject",Mailalertobj.Subject },
        {"mailtemplate_attachment", AttachmentName}   ,
        {"mailtemplate_content",  objCmnFunctions.Getreplacesinglequotes(Mailalertobj.Content)}  ,
        {"mailtemplate_sign", Mailalertobj.Signature} ,
        {"mailtemplate_insert_by", objCmnFunctions.GetLoginUserGid().ToString()}, 
        {"mailtemplate_insert_date", "sysdatetime()"},
        {"mailtemplate_isremoved", "N"},
                  };
                    string tname = "iem_mst_tmailtemplate";

                    string insertcommend = objCommonIUD.InsertCommon(codes, tname);
                    Emp_Msgretn = "Success";

                    GetConnection();
                    DataTable dt = new DataTable();
                    cmd = new SqlCommand("select MAX(mailtemplate_gid) as mailtemplate_gid  from iem_mst_tmailtemplate where mailtemplate_insert_by='" + objCmnFunctions.GetLoginUserGid().ToString() + "'", con);
                    cmd.CommandType = CommandType.Text;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        Emp_Msg = Convert.ToString(dt.Rows[0]["mailtemplate_gid"].ToString());
                        string[] words = Mailalertobj.TablescolName.Split(',');
                        foreach (string word in words)
                        {
                            string[,] codes1 = new string[,]
	                         {
                                {"mailtemplatefield_mailtemplate_gid",Emp_Msg.ToString() },
                                {"mailtemplatefield_mailfield_gid", word.ToString()}, 
                                {"mailtemplatefield_isremoved", "N"},
                             };
                            string tname1 = "iem_mst_tmailtemplatefield";

                            string insertcommend1 = objCommonIUD.InsertCommon(codes1, tname1);
                        }
                        Emp_Msg = "Success";
                        //  string[,] codes2 = new string[,]
                        // {
                        //      {"mailtemplateaddr_mailtemplate_gid",Mailalertobj. TransactionTypeId },
                        //      {"mailtemplateaddr_mode", Mailalertobj.TransactionTypeName},
                        //      {"mailtemplateaddr_type", Mailalertobj.TemplateName},
                        //      {"mailtemplateaddr_addr",Mailalertobj.TriggerTypeName },
                        //      {"mailtemplateaddr_audience", Mailalertobj.TriggerTypeFreName}, 
                        //};
                        //  string tname2 = "iem_mst_tmailtemplateaddr";

                        //  string insertcommend2 = objCommonIUD.InsertCommon(codes2, tname2);
                    }
                }
                else
                {
                    Emp_Msg = "Template Already Created For This Transaction Type";
                }

                return Emp_Msg;
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

        public string Updatetemplate(ForMailEntity Mailalertobj, string gid)
        {
            string Triggernodays = "";
            string AttachmentName = "";
            string Includeflg = "";
            string Emp_MsgretnBCC = "";
            string Emp_MsgretnCC = "";
            string Emp_Msg = "";
            string result = "";
            string Emp_Msgretn = "";
            try
            {
                //status = GetStatusexcel(EmployeeeExpense.Exp_FC.ToString(), "FunctionCode");
                //if (status == "notexists")
                //{
                //    Emp_Msgretn = "Invalid Function Code";
                //    return Emp_Msgretn;
                //}

                if (Mailalertobj.Triggernodays == null)
                {
                    Triggernodays = "";
                }
                else
                {
                    Triggernodays = Mailalertobj.Triggernodays;
                }
                if (Mailalertobj.AttachmentName == "Yes")
                {
                    AttachmentName = "Y";
                }
                else
                {
                    AttachmentName = "N";
                }
                if (Mailalertobj.Includeflg == "Yes")
                {
                    Includeflg = "Y";
                }
                else
                {
                    Includeflg = "N";
                }
                if (Mailalertobj.CCid == null)
                {
                    Emp_MsgretnCC = "";
                }
                else
                {
                    Emp_MsgretnCC = Mailalertobj.CCid;
                }
                if (Mailalertobj.BCCid == null)
                {
                    Emp_MsgretnBCC = "";
                }
                else
                {
                    Emp_MsgretnBCC = Mailalertobj.BCCid;
                }

                string[,] codes = new string[,]
	               {
                        {"mailtemplate_mailtype_gid",Mailalertobj. TransactionTypeId },
                        {"mailtemplate_mailtype_name", Mailalertobj.TransactionTypeName},
                        {"mailtemplate_template_name", Mailalertobj.TemplateName},
                        {"mailtemplate_trigger_on",Mailalertobj.TriggerTypeName },
                        {"mailtemplate_trigger_mode", Mailalertobj.TriggerTypeFreName},
                        {"mailtemplate_trigger_days",Triggernodays },
	                    {"mailtemplate_audience", Mailalertobj.AudienceName},
                        {"mailtemplate_to",Mailalertobj.TOid},
                        {"mailtemplate_workflow",Mailalertobj.WorkFlowId},
                        {"mailtemplate_cc",Emp_MsgretnCC.ToString() },
                        {"mailtemplate_bcc", Emp_MsgretnBCC.ToString()},
                        {"mailtemplate_mailtable_name", Mailalertobj.TablesName},
                        {"mailtemplate_include_approval", Includeflg},
                        {"mailtemplate_subject",Mailalertobj.Subject },
                        {"mailtemplate_attachment", AttachmentName}   ,
                        {"mailtemplate_content", Mailalertobj.Content}  ,
                        {"mailtemplate_sign", Mailalertobj.Signature} ,
                        {"mailtemplate_update_by", objCmnFunctions.GetLoginUserGid().ToString()}, 
                        {"mailtemplate_update_date", "sysdatetime()"},
                  };
                string[,] whcos = new string[,]
	               {
                        {"mailtemplate_gid",gid }
                  };
                string tname = "iem_mst_tmailtemplate";

                string insertcommend = objCommonIUD.UpdateCommon(codes, whcos, tname);
                Emp_Msgretn = "Success";

                string[,] codesfst = new string[,]
	               {                          
                           {"mailtemplatefield_isremoved","Y" }
                   };
                string[,] whcosfrst = new string[,]
	               {
                         {"mailtemplatefield_mailtemplate_gid",gid.ToString() }   
                  };
                string tnamefrst = "iem_mst_tmailtemplatefield";

                string insertcommendfrst = objCommonIUD.UpdateCommon(codesfst, whcosfrst, tnamefrst);

                string[] words = Mailalertobj.TablescolName.Split(',');
                foreach (string word in words)
                {
                    GetConnection();
                    cmd = new SqlCommand("pr_iem_trn_mailckeck", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@modulecode", SqlDbType.VarChar).Value = gid.ToString();
                    cmd.Parameters.Add("@mailtypename", SqlDbType.VarChar).Value = word.ToString();
                    cmd.Parameters.Add("@Result", SqlDbType.VarChar).Value = "Mailfields";
                    result = (string)cmd.ExecuteScalar();
                    if (result == "Exists")
                    {
                        string[,] codes1 = new string[,]
	                       {                          
                                   {"mailtemplatefield_isremoved","N" }
                           };
                        string[,] whcos1 = new string[,]
	                       {
                                 {"mailtemplatefield_mailtemplate_gid",gid.ToString() },
                                {"mailtemplatefield_mailfield_gid", word.ToString()},    
                          };
                        string tname1 = "iem_mst_tmailtemplatefield";

                        string insertcommend1 = objCommonIUD.UpdateCommon(codes1, whcos1, tname1);
                    }
                    else
                    {
                        string[,] codes1 = new string[,]
	                         {
                                {"mailtemplatefield_mailtemplate_gid",gid.ToString() },
                                {"mailtemplatefield_mailfield_gid", word.ToString()}, 
                                {"mailtemplatefield_isremoved","N" },
                             };
                        string tname1 = "iem_mst_tmailtemplatefield";

                        string insertcommend1 = objCommonIUD.InsertCommon(codes1, tname1);
                    }
                }
                Emp_Msg = "Success";
                //  string[,] codes2 = new string[,]
                // {
                //      {"mailtemplateaddr_mailtemplate_gid",Mailalertobj. TransactionTypeId },
                //      {"mailtemplateaddr_mode", Mailalertobj.TransactionTypeName},
                //      {"mailtemplateaddr_type", Mailalertobj.TemplateName},
                //      {"mailtemplateaddr_addr",Mailalertobj.TriggerTypeName },
                //      {"mailtemplateaddr_audience", Mailalertobj.TriggerTypeFreName}, 
                //};
                //  string tname2 = "iem_mst_tmailtemplateaddr";

                //  string insertcommend2 = objCommonIUD.InsertCommon(codes2, tname2);                    

                return Emp_Msg;
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

        public IEnumerable<ForMailEntity> SelectMailtemplate(string Module, string TransactionType, string gid, string request, string workflow)
        {
            List<ForMailEntity> objNatureofExpenses = new List<ForMailEntity>();
            ForMailEntity objModel;
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
                objModel = new ForMailEntity();
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
                objModel.AttachmentFlag = string.IsNullOrEmpty(dt.Rows[0]["mailtemplate_attachment"].ToString()) ? "" : dt.Rows[0]["mailtemplate_attachment"].ToString();
                objNatureofExpenses.Add(objModel);
            }

            return objNatureofExpenses;
        }

        public IEnumerable<ForMailEntity> Getmailselectdfield(string id)
        {
            List<ForMailEntity> objExpenseCategory = new List<ForMailEntity>();
            ForMailEntity objModel;
            GetConnection();
            DataTable dt = new DataTable();
            string str = "";
            str += "select upper(c.mailfield_name) as mailfield_name,c.mailfield_field_name from iem_mst_tmailtemplate as a";
            str += " inner join iem_mst_tmailtemplatefield as b on a.mailtemplate_gid=b.mailtemplatefield_mailtemplate_gid";
            str += " and b.mailtemplatefield_isremoved='N'";
            str += " inner join iem_mst_tmailfield as c on b.mailtemplatefield_mailfield_gid=c.mailfield_gid";
            str += " and c.mailfield_isremoved='N' and a.mailtemplate_gid='" + id + "'";
            str += "order by c.mailfield_orderby asc";
            cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objModel = new ForMailEntity();
                objModel.ToGetTypeId = Convert.ToString(dt.Rows[i]["mailfield_name"].ToString());
                objModel.ToGetTypeName = Convert.ToString(dt.Rows[i]["mailfield_field_name"].ToString());
                objExpenseCategory.Add(objModel);
            }

            return objExpenseCategory;
        }

        public DataTable Selectbyid(string Module, string TransactionType, string gid, string request)
        {
            //string mailtoid = "";
            //string mailfromid = "";
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
                //    if (dt.Rows.Count > 0)
                //    {
                //        if (Module == "ASMS")
                //        {
                //            string flg = Convert.ToString(dt.Rows[0]["queue_to_type"].ToString());
                //            string queueto = Convert.ToString(dt.Rows[0]["queue_to"].ToString());
                //            string queuefrom = Convert.ToString(dt.Rows[0]["queue_from"].ToString());
                //            if (flg == "I")
                //            {
                //                DataSet dtemp = new DataSet();
                //                GetConnection();
                //                str = "";
                //                str += "select employee_office_email from iem_mst_temployee where employee_isremoved='N' and employee_gid='" + queueto + "';";
                //                str += " select employee_office_email from iem_mst_temployee where employee_isremoved='N' and employee_gid='" + queuefrom + "'";
                //                cmd = new SqlCommand(str, con);
                //                cmd.CommandType = CommandType.Text;
                //                da = new SqlDataAdapter(cmd);
                //                da.Fill(dtemp);
                //                if (dtemp.Tables[0].Rows.Count > 0)
                //                {
                //                    mailtoid = Convert.ToString(dtemp.Tables[0].Rows[0]["employee_office_email"].ToString());
                //                }
                //                //if (dtemp.Tables[1].Rows.Count > 0)
                //                //{
                //                //    mailfromid = Convert.ToString(dtemp.Tables[1].Rows[0]["employee_office_email"].ToString());
                //                //}
                //                HttpContext.Current.Session["USERMailIDFrom"] = mailfromid.ToString();
                //                HttpContext.Current.Session["USERMailIDTo"] = mailtoid.ToString();
                //            }
                //            else if (flg == "G")
                //            {
                //                DataSet dtrole = new DataSet();
                //                GetConnection();
                //                str = "";
                //                str += " select employee_office_email from iem_mst_temployee";
                //                str += " inner join iem_mst_troleemployee on roleemployee_employee_gid=employee_gid";
                //                str += " inner join iem_mst_trole on role_gid=roleemployee_role_gid";
                //                str += " inner join iem_mst_trolegroup on rolegroup_gid=role_rolegroup_gid";
                //                str += " where rolegroup_gid=" + queueto + " and employee_isremoved='N' ";
                //                str += " group by employee_office_email";   
                //                //str = "";
                //                //str += "select a.employee_office_email from iem_mst_temployee as a";
                //                //str += " inner join iem_mst_troleemployee as b on a.employee_gid=b.roleemployee_employee_gid and b.roleemployee_isremoved='N'";
                //                //str += " inner join iem_mst_trole as c on b.roleemployee_role_gid=c.role_gid and c.role_isremoved='N'";
                //                //str += " where a.employee_isremoved='N' and c.role_rolegroup_gid='" + queueto + "';";
                //                //str += " select employee_office_email from iem_mst_temployee where employee_isremoved='N' and employee_gid='" + queuefrom + "'";
                //                cmd = new SqlCommand(str, con);
                //                cmd.CommandType = CommandType.Text;
                //                da = new SqlDataAdapter(cmd);
                //                da.Fill(dtrole);
                //                if (dtrole.Tables[0].Rows.Count > 0)
                //                {
                //                    for (int i = 0; i < dtrole.Tables[0].Rows.Count; i++)
                //                    {
                //                        if (dtrole.Tables[0].Rows[i]["employee_office_email"].ToString() != "")
                //                        {
                //                            if (mailtoid == "")
                //                            {
                //                                mailtoid = Convert.ToString(dtrole.Tables[0].Rows[i]["employee_office_email"].ToString());
                //                            }
                //                            else
                //                            {
                //                                mailtoid = mailtoid + "," + Convert.ToString(dtrole.Tables[0].Rows[i]["employee_office_email"].ToString());
                //                            }
                //                        }
                //                    }
                //                }
                //                //if (dtrole.Tables[1].Rows.Count > 0)
                //                //{
                //                //    mailfromid = Convert.ToString(dtrole.Tables[1].Rows[0]["employee_office_email"].ToString());
                //                //}
                //                HttpContext.Current.Session["USERMailIDFrom"] = mailfromid.ToString();
                //                HttpContext.Current.Session["USERMailIDTo"] = mailtoid.ToString();
                //            }
                //        }
                //    }
            }
            return dt;
        }

        public IEnumerable<ForMailEntity> gettemplatedetails()
        {
            List<ForMailEntity> objExpenseCategory = new List<ForMailEntity>();
            ForMailEntity objModel;
            GetConnection();
            DataTable dt = new DataTable();
            cmd = new SqlCommand("pr_iem_mst_mailtemplate", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getmailtemplatedetailsgrid";
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objModel = new ForMailEntity();
                objModel.TemplateId = Convert.ToString(dt.Rows[i]["mailtemplate_gid"].ToString());
                objModel.TemplateName = Convert.ToString(dt.Rows[i]["mailtemplate_template_name"].ToString());
                objModel.ModuleName = Convert.ToString(dt.Rows[i]["mailtype_modulecode"].ToString());
                objModel.TransactionTypeName = Convert.ToString(dt.Rows[i]["mailtype_name"].ToString());
                string status = Convert.ToString(dt.Rows[i]["mailtemplate_trigger_on"].ToString());
                if (status == "A")
                {
                    status = "On Approval";
                }
                else if (status == "S")
                {
                    status = "On Submission";
                }
                else
                {
                    status = "On Rejection";
                }
                objModel.TriggerTypeName = status;
                objModel.WorkFlowName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["workflow_name"].ToString()) ? "" : dt.Rows[i]["workflow_name"].ToString());
                objExpenseCategory.Add(objModel);
            }

            return objExpenseCategory;
        }

        public DataTable selectmailfields(string gid)
        {
            string str = "";
            DataTable dt = new DataTable();
            GetConnection();
            str += " select mailtemplatefield_gid,mailtemplatefield_mailfield_gid from iem_mst_tmailtemplatefield where  mailtemplatefield_mailtemplate_gid='" + gid + "' and mailtemplatefield_isremoved='N'";
            cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        public string Deletetemplate(string template)
        {
            string delant = "0";
            try
            {
                string[,] codes = new string[,]
	               {
         {"mailtemplate_isremoved","Y" }
                  };
                string[,] whcos = new string[,]
	               {
        {"mailtemplate_gid",template }
                  };
                string tname = "iem_mst_tmailtemplate";

                string insertcommend = objCommonIUD.UpdateCommon(codes, whcos, tname);

                string[,] codes1 = new string[,]
	               {
        {"mailtemplatefield_isremoved","Y" }
                  };
                string[,] whcos1 = new string[,]
	               {
        {"mailtemplatefield_mailtemplate_gid",template }
                  };
                string tname1 = "iem_mst_tmailtemplatefield";

                string insertcommend1 = objCommonIUD.UpdateCommon(codes1, whcos1, tname1);

                return delant;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public string mailsentmessage(string mailtemplategid, string refflag, string refgid, string refstatus, string mailalertdate, string to, string cc, string bcc, string subject, string content, string scheduledate, string sendflag, string senddate, string isremoved)
        {
            string Emp_Msg = "";
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

        public string USERMailIDFrom(string Module, string TransactionType, string gid, string request)
        {
            string Emp_Msg = "";
            try
            {
                // Emp_Msg = HttpContext.Current.Session["USERMailIDFrom"].ToString();
                //return Emp_Msg;
                string mailtoid = "";
                string mailfromid = "";
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
                    if (dt.Rows.Count > 0)
                    {
                        if (Module == "ASMS")
                        {
                            string flg = Convert.ToString(dt.Rows[0]["queue_to_type"].ToString());
                            string queueto = Convert.ToString(dt.Rows[0]["queue_to"].ToString());
                            string queuefrom = Convert.ToString(dt.Rows[0]["queue_from"].ToString());
                            if (flg == "I")
                            {
                                DataSet dtemp = new DataSet();
                                GetConnection();
                                str = "";
                                str += "select employee_office_email from iem_mst_temployee where employee_isremoved='N' and employee_gid='" + queueto + "';";
                                str += " select employee_office_email from iem_mst_temployee where employee_isremoved='N' and employee_gid='" + queuefrom + "'";
                                cmd = new SqlCommand(str, con);
                                cmd.CommandType = CommandType.Text;
                                da = new SqlDataAdapter(cmd);
                                da.Fill(dtemp);
                                if (dtemp.Tables[0].Rows.Count > 0)
                                {
                                    mailfromid = Convert.ToString(dtemp.Tables[0].Rows[0]["employee_office_email"].ToString());
                                }
                                //if (dtemp.tables[0].rows.count > 0)
                                //{
                                //    mailfromid = convert.tostring(dtemp.tables[1].rows[0]["employee_office_email"].tostring());
                                //}
                                Emp_Msg = mailfromid.ToString();
                                return Emp_Msg;
                                //HttpContext.Current.Session["USERMailIDTo"] = mailtoid.ToString();
                            }
                            else if (flg == "G")
                            {
                                DataSet dtrole = new DataSet();
                                GetConnection();
                                str = "";
                                str += " select employee_office_email from iem_mst_temployee";
                                str += " inner join iem_mst_troleemployee on roleemployee_employee_gid=employee_gid";
                                str += " inner join iem_mst_trole on role_gid=roleemployee_role_gid";
                                str += " inner join iem_mst_trolegroup on rolegroup_gid=role_rolegroup_gid";
                                str += " where rolegroup_gid=" + queueto + " and employee_isremoved='N' and roleemployee_isremoved='N' ";
                                str += " group by employee_office_email";

                                //str += "select a.employee_office_email from iem_mst_temployee as a";
                                //str += " inner join iem_mst_troleemployee as b on a.employee_gid=b.roleemployee_employee_gid and b.roleemployee_isremoved='N'";
                                //str += " inner join iem_mst_trole as c on b.roleemployee_role_gid=c.role_gid and c.role_isremoved='N'";
                                //str += " where a.employee_isremoved='N' and c.role_rolegroup_gid='" + queueto + "';";
                                //str += " select employee_office_email from iem_mst_temployee where employee_isremoved='N' and employee_gid='" + queuefrom + "'";
                                cmd = new SqlCommand(str, con);
                                cmd.CommandType = CommandType.Text;
                                da = new SqlDataAdapter(cmd);
                                da.Fill(dtrole);
                                if (dtrole.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < dtrole.Tables[0].Rows.Count; i++)
                                    {
                                        if (dtrole.Tables[0].Rows[i]["employee_office_email"].ToString() != "")
                                        {
                                            if (mailtoid == "")
                                            {
                                                mailtoid = Convert.ToString(dtrole.Tables[0].Rows[i]["employee_office_email"].ToString());
                                            }
                                            else
                                            {
                                                mailtoid = mailtoid + "," + Convert.ToString(dtrole.Tables[0].Rows[i]["employee_office_email"].ToString());
                                            }
                                        }
                                    }
                                }
                                //if (dtrole.Tables[1].Rows.Count > 0)
                                //{
                                //    mailfromid = Convert.ToString(dtrole.Tables[1].Rows[0]["employee_office_email"].ToString());
                                //}
                                //HttpContext.Current.Session["USERMailIDFrom"] = mailfromid;
                                //HttpContext.Current.Session["USERMailIDTo"] = mailtoid;
                                Emp_Msg = mailfromid.ToString();
                                return Emp_Msg;
                            }
                        }
                    }
                }
                return Emp_Msg;
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

        public IEnumerable<ForMailEntity> GetWorkFlow(string transactiontype, string modulename)
        {
            List<ForMailEntity> objExpenseCategory = new List<ForMailEntity>();
            ForMailEntity objModel;
            GetConnection();
            DataTable dt = new DataTable();
            string lsQry = "select workflow_level,workflow_name ";
            lsQry += "from iem_mst_tworkflow ";
            lsQry += "where upper(workflow_requesttype)='" + transactiontype.ToUpper().Trim() + "' ";
            lsQry += "and upper(workflow_module)='" + modulename.ToUpper().Trim() + "' ";
            cmd = new SqlCommand(lsQry, con);
            cmd.CommandType = CommandType.Text;
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objModel = new ForMailEntity();
                objModel.WorkFlowId = Convert.ToString(dt.Rows[i]["workflow_level"].ToString());
                objModel.WorkFlowName = Convert.ToString(dt.Rows[i]["workflow_name"].ToString());
                objExpenseCategory.Add(objModel);
            }

            return objExpenseCategory;
        }


        //public string USERMailIDTo(string Module, string TransactionType, string gid, string request)
        //{
        //    throw new NotImplementedException();
        //}

        public IEnumerable<ForMailEntity> getloginemployeedetails()
        {
            List<ForMailEntity> objExpenseCategory = new List<ForMailEntity>();
            ForMailEntity objModel;
            GetConnection();
            DataTable dt = new DataTable();
            cmd = new SqlCommand("pr_iem_mst_mailtemplate", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
            cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getloginemployeedetails";
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                objModel = new ForMailEntity();
                objModel.employee = Convert.ToString(dt.Rows[i]["employee"].ToString());
                objModel.empdept = Convert.ToString(dt.Rows[i]["employee_dept_name"].ToString());
                objExpenseCategory.Add(objModel);
            }

            return objExpenseCategory;
        }

        public string SelectMailIdsForRoleGroup(string rolegroupname, string queuegid, string workflow, string modulename, string requesttype)
        {
            try
            {
                string str = "";
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
                //if (dtemp.tables[0].rows.count > 0)
                //{
                //    mailfromid = convert.tostring(dtemp.tables[1].rows[0]["employee_office_email"].tostring());
                //}
                return mailtoids.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
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
        public string CheckIsFinalApproverFB(string gid)
        {
            try
            {
                string NextApproverIsMandatory = "N";
                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_MailProcedure", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@queuegid", SqlDbType.Int).Value = Convert.ToInt32(gid);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "checkisfinalapprover";
                NextApproverIsMandatory = Convert.ToString(cmd.ExecuteScalar());

                return NextApproverIsMandatory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetCurrApprStage(string queuegid = "", string refname = "")
        {
            try
            {
                string CurApprStage = "";
                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_MailProcedure", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@queuegid", SqlDbType.Int).Value = Convert.ToInt32(queuegid);
                cmd.Parameters.Add("@refname", SqlDbType.VarChar).Value = refname.ToUpper();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "checkisdelmatapprover";
                CurApprStage = Convert.ToString(cmd.ExecuteScalar());

                return CurApprStage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public string GetWorkFlowFB(string queuegid = "")
        {
            try
            {
                string CurApprStage = "";
                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_MailProcedure", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@queuegid", SqlDbType.Int).Value = Convert.ToInt32(queuegid);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getworkflow";
                CurApprStage = Convert.ToString(cmd.ExecuteScalar());

                return CurApprStage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //for mail for payment alert for employee and supplier
        public DataTable EftErapaymentAlert(string pvId)
        {
            GetConnection();
            cmd = new SqlCommand("PR_FS_EftEraStatusUpdate", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@PVId", SqlDbType.VarChar).Value = pvId;
            da = new SqlDataAdapter(cmd);
            cmd.CommandTimeout = 0;
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt;
        }

        //mail for cheque status update
        public DataTable ChequeStatusUpdateDetails(string pvId)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("PR_FS_CHEQUE_DISPATCH_MAIL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PVId", SqlDbType.VarChar).Value = pvId;
                da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }

        public DataTable GetBounchresone(string Ecfgid)
        {
            try
            {
            GetConnection();
            cmd = new SqlCommand("FS_Get_InexAttachmentReason_det", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ECFGId", SqlDbType.VarChar).Value = Ecfgid;
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            return dt;

            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }

        public DataTable Getaddress()
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("FS_Getaddress_det", con);
                cmd.CommandType = CommandType.StoredProcedure;
                DataTable dt = new DataTable();
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
                da.Dispose();
            }
        }
    }
}