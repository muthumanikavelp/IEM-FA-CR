using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Mvc;
using IEM.Common;
using System.Collections;

namespace IEM.Areas.FLEXIBUY.Models
{
    public class prsummodel : IRepositoryKIR
    {
        CommonIUD objuid = new CommonIUD();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlCommand cmd1 = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        SqlDataAdapter da1 = new SqlDataAdapter();
        ErrorLog objErrorLog = new ErrorLog();

        public void getconnection()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                conn.Open();
            }
        }

        public PrSumEntity GetPrSumry()
        {
            PrSumEntity prsument = new PrSumEntity();
            try
            {
                getconnection();
                PrsummaryModel prsummod;
                prsument.lstprSum = new List<PrsummaryModel>();

                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_prsummarylist", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@employee_gid", SqlDbType.BigInt).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Prsummary";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        prsummod = new PrsummaryModel();
                        prsummod.srNo = i + 1;
                        prsummod.Prgid = dt.Rows[i]["prheader_gid"].ToString();
                        prsummod.prRefNo = dt.Rows[i]["prheader_refno"].ToString();
                        prsummod.prDate = dt.Rows[i]["prheader_date"].ToString();
                        prsummod.prBranch = dt.Rows[i]["branch"].ToString();
                        prsummod.prDept = dt.Rows[i]["employee_dept_name"].ToString();
                        prsummod.prDesc = dt.Rows[i]["prheader_desc"].ToString();
                        prsummod.prReqFor = dt.Rows[i]["requestfor_name"].ToString();
                        prsummod.prStatus = dt.Rows[i]["status_name"].ToString();
                        prsument.lstprSum.Add(prsummod);
                    }
                }
                return prsument;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return prsument;
            }
            finally
            {
                conn.Close();
            }


        }

        public IEnumerable<flexibuydashboard> doctypedata()
        {

            List<flexibuydashboard> objparenttax = new List<flexibuydashboard>();
            try
            {
                objparenttax.Add(new flexibuydashboard { DocTypeIdd = "0", DocTypeName = "-- Select --", });
                objparenttax.Add(new flexibuydashboard { DocTypeIdd = "1", DocTypeName = "PAR", });
                objparenttax.Add(new flexibuydashboard { DocTypeIdd = "2", DocTypeName = "PR", });
                objparenttax.Add(new flexibuydashboard { DocTypeIdd = "3", DocTypeName = "CBF", });
                objparenttax.Add(new flexibuydashboard { DocTypeIdd = "4", DocTypeName = "PO", });
                objparenttax.Add(new flexibuydashboard { DocTypeIdd = "5", DocTypeName = "WO", });
                //  objparenttax.Add(new flexibuydashboard { DocTypeIdd = "6", DocTypeName = "GRN", });
                return objparenttax;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objparenttax;
            }
            finally
            {
                conn.Close();
            }

        }
        public IEnumerable<flexibuydashboard> GetStatusType()
        {
            List<flexibuydashboard> objparenttax = new List<flexibuydashboard>();
            try
            {
                objparenttax.Add(new flexibuydashboard { StatusTypeId = "0", StatusTypeName = "-- Select --", });
                objparenttax.Add(new flexibuydashboard { StatusTypeId = "1", StatusTypeName = "Draft", });
                //objparenttax.Add(new dashboard { StatusTypeId = "2", StatusTypeName = "Pending Approval-Checker", });
                //objparenttax.Add(new dashboard { StatusTypeId = "3", StatusTypeName = "Pending Approval-Supervisor", });
                //objparenttax.Add(new dashboard { StatusTypeId = "4", StatusTypeName = "Pending Approval-Delmat", });
                objparenttax.Add(new flexibuydashboard { StatusTypeId = "2", StatusTypeName = "Inprocess", });
                objparenttax.Add(new flexibuydashboard { StatusTypeId = "3", StatusTypeName = "Approved", });
                objparenttax.Add(new flexibuydashboard { StatusTypeId = "4", StatusTypeName = "Rejected", });
                //objparenttax.Add(new dashboard { StatusTypeId = "7", StatusTypeName = "Pending Approval-PIP Input", });
                //objparenttax.Add(new dashboard { StatusTypeId = "8", StatusTypeName = "Pending Approval-Cancellation", });
                return objparenttax;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objparenttax;
            }
            finally
            {
                conn.Close();
            }

        }
        public IEnumerable<flexibuydashboard> doctypedataapp()
        {

            List<flexibuydashboard> objparenttax = new List<flexibuydashboard>();
            try
            {
                objparenttax.Add(new flexibuydashboard { DocTypeAppId = "0", DocTypeAppName = "-- Select --", });
                objparenttax.Add(new flexibuydashboard { DocTypeAppId = "1", DocTypeAppName = "PAR", });
                objparenttax.Add(new flexibuydashboard { DocTypeAppId = "2", DocTypeAppName = "PR", });
                objparenttax.Add(new flexibuydashboard { DocTypeAppId = "3", DocTypeAppName = "CBF", });
                objparenttax.Add(new flexibuydashboard { DocTypeAppId = "4", DocTypeAppName = "PO", });
                objparenttax.Add(new flexibuydashboard { DocTypeAppId = "5", DocTypeAppName = "WO", });
                //  objparenttax.Add(new flexibuydashboard { DocTypeAppId = "6", DocTypeAppName = "GRN", });
                return objparenttax;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objparenttax;
            }
            finally
            {
                conn.Close();
            }

        }
        public IEnumerable<flexibuydashboard> GetStatusTypeapp()
        {
            List<flexibuydashboard> objparenttax = new List<flexibuydashboard>();
            try
            {
                objparenttax.Add(new flexibuydashboard { StatusTypeAppId = "0", StatusTypeAppName = "-- Select --", });
                objparenttax.Add(new flexibuydashboard { StatusTypeAppId = "1", StatusTypeAppName = "Inprocess", });
                //objparenttax.Add(new dashboard { StatusTypeAppId = "2", StatusTypeAppName = "Concurrent Approval", });
                return objparenttax;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objparenttax;
            }
            finally
            {
                conn.Close();
            }

        }

        //public IEnumerable<flexibuydashboard> getdashboard()
        //{
        //    try
        //    {

        //        getconnection();
        //        List<flexibuydashboard> prdashboard = new List<flexibuydashboard>();
        //        flexibuydashboard dash;
        //        DataTable dt = new DataTable();
        //        cmd = new SqlCommand("pr_fb_trn_tflexidashboard", conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@employee_gid", SqlDbType.BigInt).Value = objCmnFunctions.GetLoginUserGid();
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        if (dt.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                dash = new flexibuydashboard();
        //                dash.Requesttype = Convert.ToString(dt.Rows[i]["Category"].ToString());
        //                dash.draft = Convert.ToInt32(dt.Rows[i]["Draft"].ToString());
        //                dash.inprocess = Convert.ToInt32(dt.Rows[i]["Inprocess"].ToString());
        //                dash.approval = Convert.ToInt32(dt.Rows[i]["Approval"].ToString());
        //                dash.reject = Convert.ToInt32(dt.Rows[i]["Rejected"].ToString());
        //                dash.formyapproval = Convert.ToInt32(dt.Rows[i]["ForApproval"].ToString());
        //                prdashboard.Add(dash);

        //            }
        //        }

        //        return prdashboard;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}
        public IEnumerable<flexibuydashboard> getdashboard()
        {
            List<flexibuydashboard> prdashboard = new List<flexibuydashboard>();
            try
            {
                getconnection();
                string IsDelegateUser = "";
                string raisermode = "";
                Int32 RaierGid = 0;
                if (HttpContext.Current.Session["Proxyemployee_gid"] != null)
                {
                    raisermode = "P";
                    RaierGid = Convert.ToInt32(HttpContext.Current.Session["Proxyemployee_gid"]);
                }
                else
                {
                    raisermode = "S";
                }
                int appcount = 0;

                flexibuydashboard dash;
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                DataSet ds = new DataSet();
                DataTable dt_isdelegate = new DataTable();
                int employee_gid = objCmnFunctions.GetLoginUserGid();

                cmd = new SqlCommand("pr_fb_dashboardnew", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = employee_gid;
                cmd.CommandTimeout = 0;
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds != null && ds.Tables.Count != 0)
                {
                    dt = ds.Tables[0];
                    dt1 = ds.Tables[1];
                    dt2 = ds.Tables[2]; 
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if ((string.IsNullOrEmpty(dt.Rows[i]["CATEGORY"].ToString()) ? "" : dt.Rows[i]["CATEGORY"].ToString()) != "GRN")
                            {
                                dash = new flexibuydashboard();
                                dash.Requesttype = Convert.ToString(dt.Rows[i]["CATEGORY"].ToString());
                                dash.draft = Convert.ToInt32(dt.Rows[i]["DRAFT"].ToString());
                                dash.inprocess = Convert.ToInt32(dt.Rows[i]["INPROCESS"].ToString());
                                dash.approval = Convert.ToInt32(dt.Rows[i]["APPROVED"].ToString());
                                dash.reject = Convert.ToInt32(dt.Rows[i]["REJECTED"].ToString());
                                string doctype = Convert.ToString(dt.Rows[i]["CATEGORY"].ToString());
                                if(dt2.Rows.Count==0)
                                {
                                    dash.formyapproval = Convert.ToInt32(dt1.Rows[i]["FORMYAPPROVAL"].ToString());
                                }
                                else if (dt2.Rows.Count>0 && raisermode != "P")
                                {
                                    appcount = GetForMyapprval(objCmnFunctions.GetLoginUserGid().ToString(), doctype);
                                    dash.formyapproval = appcount;
                                } 
                                prdashboard.Add(dash);
                            }

                        }
                    }
                }
                //if (dt.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        dash = new flexibuydashboard();
                //        dash.Requesttype = Convert.ToString(dt.Rows[i]["Category"].ToString());
                //        dash.draft = Convert.ToInt32(dt.Rows[i]["Draft"].ToString());
                //        dash.inprocess = Convert.ToInt32(dt.Rows[i]["Inprocess"].ToString());
                //        dash.approval = Convert.ToInt32(dt.Rows[i]["Approval"].ToString());
                //        dash.reject = Convert.ToInt32(dt.Rows[i]["Rejected"].ToString());
                //        dash.formyapproval = Convert.ToInt32(dt.Rows[i]["ForApproval"].ToString());

                //        /* code to retrieve the records in queue */
                //        SqlCommand cmdMyqueue = new SqlCommand("pr_fb_inprocess", conn);
                //        cmdMyqueue.CommandType = CommandType.StoredProcedure;
                //        cmdMyqueue.Parameters.Add("@emp_gid", SqlDbType.Int).Value = employee_gid;
                //        cmdMyqueue.Parameters.Add("@output_type", SqlDbType.Char).Value = "S";
                //        cmdMyqueue.Parameters.Add("@queue_ref_name", SqlDbType.VarChar).Value = dash.Requesttype;
                //        SqlDataAdapter daMyqueue = new SqlDataAdapter(cmdMyqueue);
                //        DataTable dtMyqueue = new DataTable();
                //        daMyqueue.Fill(dtMyqueue);

                //        if (dtMyqueue.Rows.Count > 0)
                //        {
                //            dash.formyapproval = Convert.ToInt32(dtMyqueue.Rows[0]["cnt"].ToString());
                //        }
                //        else
                //            dash.formyapproval = 0;

                //        dtMyqueue.Clear();
                //        dtMyqueue.Dispose();
                //        cmdMyqueue.Dispose();
                //        daMyqueue.Dispose();

                //        prdashboard.Add(dash);

                //    }
                //}

                return prdashboard;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return prdashboard;
            }
            finally
            {
                conn.Close();
            }
        }

        public int GetForMyapprval(string userlognid, string doctype)
        {
            int delant = 0;
            string delegateByID = "";
            try
            {
                //bharathi1
                //Hashtable emplist = new Hashtable();
                Hashtable deptlist = new Hashtable();
                int deptlistid = 0;
                int emplistid = 0;
                string IsAllDept;
                //emplist.Add(emplistid, userlognid);
                getconnection();
                DataTable dtdel = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_NatureofExpenses", conn);
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
                // for check having all type of department. K.Bharathidhasan.
                if (deptlist.ContainsValue("1"))
                {
                    IsAllDept = "Y";
                }
                else
                {
                    IsAllDept = "N";
                }
                int delegatesuser;
                int delegatedeptid = 0;
                int currentuser;

                currentuser = Convert.ToInt32(userlognid.ToString().Trim());
                getconnection();
                DataTable dtco = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_NatureofExpenses", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@para1", SqlDbType.Int).Value = currentuser;
                cmd.Parameters.Add("@doctype", SqlDbType.VarChar).Value = doctype;
                //cmd.Parameters.Add("@delegateById", SqlDbType.VarChar).Value = delegateByID;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "queueformyapprvolcount";
                da = new SqlDataAdapter(cmd);
                da.Fill(dtco);

                Boolean hierhy = false;
                if (dtco.Rows.Count > 0)
                {
                    if (doctype == "CBF")
                    {
                        for (int i = 0; i < dtco.Rows.Count; i++)
                        {
                            hierhy = false;
                            if (dtco.Rows[i]["ref_name"].ToString() == "CBF")
                            {
                                delant = delant + Convert.ToInt32(dtco.Rows[i]["FORMYAPPROVAL"].ToString());
                            }
                        }
                    }
                    else if (doctype == "PAR")
                    {
                        for (int i = 0; i < dtco.Rows.Count; i++)
                        {
                            hierhy = false;
                            if (dtco.Rows[i]["ref_name"].ToString() == "PAR")
                            {
                                delant = delant + Convert.ToInt32(dtco.Rows[i]["FORMYAPPROVAL"].ToString());
                            }
                        }
                    }
                    else if (doctype == "PR")
                    {
                        for (int i = 0; i < dtco.Rows.Count; i++)
                        {
                            hierhy = false;
                            if (dtco.Rows[i]["ref_name"].ToString() == "PR")
                            {
                                delant = delant + Convert.ToInt32(dtco.Rows[i]["FORMYAPPROVAL"].ToString());
                            }
                        }
                    }
                    else if (doctype == "PO")
                    {
                        for (int i = 0; i < dtco.Rows.Count; i++)
                        {
                            hierhy = false;
                            if (dtco.Rows[i]["ref_name"].ToString() == "PO")
                            {
                                delant = delant + Convert.ToInt32(dtco.Rows[i]["FORMYAPPROVAL"].ToString());
                            }
                        }
                    }
                    else if (doctype == "WO")
                    {
                        for (int i = 0; i < dtco.Rows.Count; i++)
                        {
                            hierhy = false;
                            if (dtco.Rows[i]["ref_name"].ToString() == "WO")
                            {
                                delant = delant + Convert.ToInt32(dtco.Rows[i]["FORMYAPPROVAL"].ToString());
                            }
                        }
                    }
                }
                if (dtdel.Rows.Count > 0)
                {
                    if (IsAllDept == "Y")
                    {
                        for (int tr = 0; tr < 1; tr++)
                        {

                            delegatesuser = Convert.ToInt32(dtdel.Rows[tr]["delegate_by"].ToString().Trim());
                            delegatedeptid = 1;
                            getconnection();
                            DataTable dtco1 = new DataTable();
                            cmd = new SqlCommand("pr_fb_mst_NatureofExpenses", conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@para1", SqlDbType.Int).Value = currentuser;
                            cmd.Parameters.Add("@doctype", SqlDbType.VarChar).Value = doctype;
                            cmd.Parameters.Add("@delegateById", SqlDbType.VarChar).Value = delegatesuser;
                            cmd.Parameters.Add("@delegatedeptId", SqlDbType.VarChar).Value = delegatedeptid;
                            cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "qdelegatecount";
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dtco1);

                            //Boolean hierhy = false;
                            if (doctype == "CBF")
                            {
                                for (int i = 0; i < dtco1.Rows.Count; i++)
                                {
                                    hierhy = false;
                                    if (dtco1.Rows[i]["ref_name"].ToString() == "CBF")
                                    {
                                        delant = delant + Convert.ToInt32(dtco1.Rows[i]["FORMYAPPROVAL"].ToString());
                                    }
                                }
                            }
                            else if (doctype == "PAR")
                            {
                                for (int i = 0; i < dtco1.Rows.Count; i++)
                                {
                                    hierhy = false;
                                    if (dtco1.Rows[i]["ref_name"].ToString() == "PAR")
                                    {
                                        delant = delant + Convert.ToInt32(dtco1.Rows[i]["FORMYAPPROVAL"].ToString());
                                    }
                                }
                            }
                            else if (doctype == "PR")
                            {
                                for (int i = 0; i < dtco1.Rows.Count; i++)
                                {
                                    hierhy = false;
                                    if (dtco1.Rows[i]["ref_name"].ToString() == "PR")
                                    {
                                        delant = delant + Convert.ToInt32(dtco1.Rows[i]["FORMYAPPROVAL"].ToString());
                                    }
                                }
                            }
                            else if (doctype == "PO")
                            {
                                for (int i = 0; i < dtco1.Rows.Count; i++)
                                {
                                    hierhy = false;
                                    if (dtco1.Rows[i]["ref_name"].ToString() == "PO")
                                    {
                                        delant = delant + Convert.ToInt32(dtco1.Rows[i]["FORMYAPPROVAL"].ToString());
                                    }
                                }
                            }
                            else if (doctype == "WO")
                            {
                                for (int i = 0; i < dtco1.Rows.Count; i++)
                                {
                                    hierhy = false;
                                    if (dtco1.Rows[i]["ref_name"].ToString() == "WO")
                                    {
                                        delant = delant + Convert.ToInt32(dtco1.Rows[i]["FORMYAPPROVAL"].ToString());
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
                            getconnection();
                            DataTable dtco1 = new DataTable();
                            cmd = new SqlCommand("pr_fb_mst_NatureofExpenses", conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@para1", SqlDbType.Int).Value = currentuser;
                            cmd.Parameters.Add("@doctype", SqlDbType.VarChar).Value = doctype;
                            cmd.Parameters.Add("@delegateById", SqlDbType.VarChar).Value = delegatesuser;
                            cmd.Parameters.Add("@delegatedeptId", SqlDbType.VarChar).Value = delegatedeptid;
                            cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "qdelegatecount";
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dtco1);

                            //Boolean hierhy = false;
                            if (doctype == "CBF")
                            {
                                for (int i = 0; i < dtco1.Rows.Count; i++)
                                {
                                    hierhy = false;
                                    if (dtco1.Rows[i]["ref_name"].ToString() == "CBF")
                                    {
                                        delant = delant + Convert.ToInt32(dtco1.Rows[i]["FORMYAPPROVAL"].ToString());
                                    }
                                }
                            }
                            else if (doctype == "PAR")
                            {
                                for (int i = 0; i < dtco1.Rows.Count; i++)
                                {
                                    hierhy = false;
                                    if (dtco1.Rows[i]["ref_name"].ToString() == "PAR")
                                    {
                                        delant = delant + Convert.ToInt32(dtco1.Rows[i]["FORMYAPPROVAL"].ToString());
                                    }
                                }
                            }
                            else if (doctype == "PR")
                            {
                                for (int i = 0; i < dtco1.Rows.Count; i++)
                                {
                                    hierhy = false;
                                    if (dtco1.Rows[i]["ref_name"].ToString() == "PR")
                                    {
                                        delant = delant + Convert.ToInt32(dtco1.Rows[i]["FORMYAPPROVAL"].ToString());
                                    }
                                }
                            }
                            else if (doctype == "PO")
                            {
                                for (int i = 0; i < dtco1.Rows.Count; i++)
                                {
                                    hierhy = false;
                                    if (dtco1.Rows[i]["ref_name"].ToString() == "PO")
                                    {
                                        delant = delant + Convert.ToInt32(dtco1.Rows[i]["FORMYAPPROVAL"].ToString());
                                    }
                                }
                            }
                            else if (doctype == "WO")
                            {
                                for (int i = 0; i < dtco1.Rows.Count; i++)
                                {
                                    hierhy = false;
                                    if (dtco1.Rows[i]["ref_name"].ToString() == "WO")
                                    {
                                        delant = delant + Convert.ToInt32(dtco1.Rows[i]["FORMYAPPROVAL"].ToString());
                                    }
                                }
                            }
                        }
                    }
                }
                return delant;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                conn.Close();
                da.Dispose();
            }
        }






        public IEnumerable<flexibuydashboard> getMyRequest()
        {
            List<flexibuydashboard> lstReqGrid = new List<flexibuydashboard>();
            try
            {
                getconnection();

                flexibuydashboard dash;
                DataTable dt = new DataTable();
                cmd = new SqlCommand("[pr_fb_trn_tdashboardmyrequestgrid]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EMPLOYEEGID", SqlDbType.BigInt).Value = objCmnFunctions.GetLoginUserGid();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dash = new flexibuydashboard();
                        dash.dsno = i + 1;
                        dash.category = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCCATEGORY"].ToString()) ? "" : dt.Rows[i]["DOCCATEGORY"].ToString());
                        dash.dgid = Convert.ToInt32(dt.Rows[i]["GID"].ToString());
                        dash.docNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCNO"].ToString()) ? "" : dt.Rows[i]["DOCNO"].ToString());
                        dash.ddate = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCDATE"].ToString()) ? "" : dt.Rows[i]["DOCDATE"].ToString());
                        dash.amount = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCAMOUNT"].ToString()) ? "0" : dt.Rows[i]["DOCAMOUNT"].ToString());
                        dash.status = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCSTATUS"].ToString()) ? "" : dt.Rows[i]["DOCSTATUS"].ToString());
                        dash.raiser = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["RAISER"].ToString()) ? "" : dt.Rows[i]["RAISER"].ToString());
                        dash.description = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DESCR"].ToString()) ? "" : dt.Rows[i]["DESCR"].ToString());
                        dash.requestfor = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["requestfor_name"].ToString()) ? "" : dt.Rows[i]["requestfor_name"].ToString());
                        lstReqGrid.Add(dash);
                    }
                }
                return lstReqGrid;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return lstReqGrid;
            }
            finally
            {
                conn.Close();
            }
        }
        public IEnumerable<flexibuydashboard> getMyRequestquery()
        {
            List<flexibuydashboard> lstReqGrid = new List<flexibuydashboard>();
            try
            {
                getconnection();

                flexibuydashboard dash;
                DataTable dt = new DataTable();
                cmd = new SqlCommand("[pr_fb_trn_tquery]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EMPLOYEEGID", SqlDbType.BigInt).Value = objCmnFunctions.GetLoginUserGid();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dash = new flexibuydashboard();
                        dash.dsno = i + 1;
                        dash.category = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCCATEGORY"].ToString()) ? "" : dt.Rows[i]["DOCCATEGORY"].ToString());
                        dash.dgid = Convert.ToInt32(dt.Rows[i]["GID"].ToString());
                        dash.docNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCNO"].ToString()) ? "" : dt.Rows[i]["DOCNO"].ToString());
                        dash.ddate = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCDATE"].ToString()) ? "" : dt.Rows[i]["DOCDATE"].ToString());
                        dash.amount = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCAMOUNT"].ToString()) ? "" : dt.Rows[i]["DOCAMOUNT"].ToString());
                        dash.status = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCSTATUS"].ToString()) ? "" : dt.Rows[i]["DOCSTATUS"].ToString());
                        dash.raiser = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["RAISER"].ToString()) ? "" : dt.Rows[i]["RAISER"].ToString());
                        dash.description = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DESCR"].ToString()) ? "" : dt.Rows[i]["DESCR"].ToString());
                        dash.requestfor = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["requestfor_name"].ToString()) ? "" : dt.Rows[i]["requestfor_name"].ToString());
                        lstReqGrid.Add(dash);
                    }
                }
                return lstReqGrid;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return lstReqGrid;
            }
            finally
            {
                conn.Close();
            }
        }
        public IEnumerable<flexibuydashboard> getMyRequest(string date, string DocNo, string Amount, string Category, string Status)
        {
            List<flexibuydashboard> lstReqGrid = new List<flexibuydashboard>();
            try
            {
                getconnection();
                flexibuydashboard dash;
                DataTable dt = new DataTable();
                cmd = new SqlCommand("[pr_fb_trn_tdashboardmyrequestgrid]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EMPLOYEEGID", SqlDbType.BigInt).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@DOCDATE", SqlDbType.VarChar).Value = date;
                cmd.Parameters.Add("@DOCNO", SqlDbType.VarChar).Value = DocNo;
                cmd.Parameters.Add("@DOCAMOUNT", SqlDbType.VarChar).Value = Amount;

                if (Category == "-- Select --")
                {
                    Category = "";
                }
                if (Status == "-- Select --")
                {
                    Status = "";
                }
                cmd.Parameters.Add("@DOCCATEGORY", SqlDbType.VarChar).Value = Category;
                cmd.Parameters.Add("@DOCSTATUS", SqlDbType.VarChar).Value = Status;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dash = new flexibuydashboard();
                        dash.dsno = i + 1;
                        dash.category = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCCATEGORY"].ToString()) ? "" : dt.Rows[i]["DOCCATEGORY"].ToString());
                        dash.dgid = Convert.ToInt32(dt.Rows[i]["GID"].ToString());
                        dash.docNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCNO"].ToString()) ? "" : dt.Rows[i]["DOCNO"].ToString());
                        dash.ddate = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCDATE"].ToString()) ? "" : dt.Rows[i]["DOCDATE"].ToString());
                        dash.amount = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCAMOUNT"].ToString()) ? "" : dt.Rows[i]["DOCAMOUNT"].ToString());
                        dash.status = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCSTATUS"].ToString()) ? "" : dt.Rows[i]["DOCSTATUS"].ToString());
                        dash.raiser = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["RAISER"].ToString()) ? "" : dt.Rows[i]["RAISER"].ToString());
                        dash.description = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DESCR"].ToString()) ? "" : dt.Rows[i]["DESCR"].ToString());
                        dash.requestfor = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["requestfor_name"].ToString()) ? "" : dt.Rows[i]["requestfor_name"].ToString());
                        lstReqGrid.Add(dash);
                    }
                }
                return lstReqGrid;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return lstReqGrid;
            }
            finally
            {
                conn.Close();
            }
        }

        //public IEnumerable<flexibuydashboard> getMyApproval()
        //{
        //    try
        //    {

        //        getconnection();
        //        List<flexibuydashboard> lstReqGrid = new List<flexibuydashboard>();
        //        flexibuydashboard dash;
        //        DataTable dt = new DataTable();
        //        cmd = new SqlCommand("[pr_fb_trn_tdashboardmyapprovalgrid]", conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@EMPLOYEEGID", SqlDbType.BigInt).Value = objCmnFunctions.GetLoginUserGid();
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        if (dt.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                dash = new flexibuydashboard();
        //                dash.dsno = i + 1;
        //                dash.category = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCCATEGORY"].ToString()) ? "" : dt.Rows[i]["DOCCATEGORY"].ToString());
        //                dash.dgid = Convert.ToInt32(dt.Rows[i]["GID"].ToString());
        //                dash.docNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCNO"].ToString()) ? "" : dt.Rows[i]["DOCNO"].ToString());
        //                dash.ddate = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCDATE"].ToString()) ? "" : dt.Rows[i]["DOCDATE"].ToString());
        //                dash.amount = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCAMOUNT"].ToString()) ? "" : dt.Rows[i]["DOCAMOUNT"].ToString());
        //                dash.status = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCSTATUS"].ToString()) ? "" : dt.Rows[i]["DOCSTATUS"].ToString());
        //                dash.statusId = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["STATUSGID"].ToString()) ? "" : dt.Rows[i]["STATUSGID"].ToString());
        //                dash.raiser = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["RAISER"].ToString()) ? "" : dt.Rows[i]["RAISER"].ToString());
        //                dash.description = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DESCR"].ToString()) ? "" : dt.Rows[i]["DESCR"].ToString());
        //                lstReqGrid.Add(dash);
        //            }
        //        }
        //        return lstReqGrid;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}

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
                cmd = new SqlCommand("pr_fb_mst_NatureofExpenses", conn);
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
                cmd = new SqlCommand("pr_fb_inprocess", conn);
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
                            cmd = new SqlCommand("pr_fb_inprocess", conn);
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
                            cmd = new SqlCommand("pr_fb_inprocess", conn);
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
                conn.Close();
            }
        }



        public IEnumerable<flexibuydashboard> getMyApproval()
        {
            List<flexibuydashboard> lstReqGrid = new List<flexibuydashboard>();
            try
            {

                getconnection();
                flexibuydashboard dash;
                DataTable dt = new DataTable();

                cmd = new SqlCommand("pr_fb_inprocess", conn);
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
                    }
                }

                return lstReqGrid;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return lstReqGrid;
            }
            finally
            {
                conn.Close();
            }
        }
        //public IEnumerable<flexibuydashboard> getMyApproval(string date, string DocNo, string Amount, string Category, string Status)
        //{
        //    try
        //    {

        //        getconnection();
        //        List<flexibuydashboard> lstReqGrid = new List<flexibuydashboard>();
        //        flexibuydashboard dash;
        //        DataTable dt = new DataTable();
        //        cmd = new SqlCommand("[pr_fb_trn_tdashboardmyapprovalgrid]", conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@EMPLOYEEGID", SqlDbType.BigInt).Value = objCmnFunctions.GetLoginUserGid();
        //        cmd.Parameters.Add("@DOCDATE", SqlDbType.VarChar).Value = date;
        //        cmd.Parameters.Add("@DOCNO", SqlDbType.VarChar).Value = DocNo;
        //        cmd.Parameters.Add("@DOCAMOUNT", SqlDbType.VarChar).Value = Amount;

        //        if (Category == "-- Select --")
        //        {
        //            Category = "";
        //        }
        //        if (Status == "-- Select --")
        //        {
        //            Status = "";
        //        }
        //        cmd.Parameters.Add("@DOCCATEGORY", SqlDbType.VarChar).Value = Category;
        //        cmd.Parameters.Add("@DOCSTATUS", SqlDbType.VarChar).Value = Status;
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        if (dt.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                dash = new flexibuydashboard();
        //                dash.dsno = i + 1;
        //                dash.category = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCCATEGORY"].ToString()) ? "" : dt.Rows[i]["DOCCATEGORY"].ToString());
        //                dash.dgid = Convert.ToInt32(dt.Rows[i]["GID"].ToString());
        //                dash.docNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCNO"].ToString()) ? "" : dt.Rows[i]["DOCNO"].ToString());
        //                dash.ddate = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCDATE"].ToString()) ? "" : dt.Rows[i]["DOCDATE"].ToString());
        //                dash.amount = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCAMOUNT"].ToString()) ? "" : dt.Rows[i]["DOCAMOUNT"].ToString());
        //                dash.status = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCSTATUS"].ToString()) ? "" : dt.Rows[i]["DOCSTATUS"].ToString());
        //                dash.raiser = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["RAISER"].ToString()) ? "" : dt.Rows[i]["RAISER"].ToString());
        //                dash.description = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DESCR"].ToString()) ? "" : dt.Rows[i]["DESCR"].ToString());
        //                lstReqGrid.Add(dash);
        //            }
        //        }
        //        return lstReqGrid;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}

        public IEnumerable<flexibuydashboard> getMyApproval(string date, string DocNo, string Amount, string Category, string Status)
        {
            List<flexibuydashboard> lstReqGrid = new List<flexibuydashboard>();
            try
            {

                getconnection();
                flexibuydashboard dash;
                DataTable dt = new DataTable();

                if (Category == "-- Select --") Category = "";
                if (Status == "-- Select --") Status = "";
                if (Amount == "") Amount = "0";

                cmd = new SqlCommand("pr_fb_inprocess", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@emp_gid", SqlDbType.BigInt).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@output_type", SqlDbType.Char).Value = "D";

                if (date == "" || date == null)
                    cmd.Parameters.Add("@queue_doc_date", SqlDbType.Date).Value = null;
                else
                    cmd.Parameters.Add("@queue_doc_date", SqlDbType.Date).Value = Convert.ToDateTime(date);

                cmd.Parameters.Add("@queue_doc_no", SqlDbType.VarChar).Value = DocNo;
                cmd.Parameters.Add("@queue_doc_amount", SqlDbType.Decimal).Value = Convert.ToDecimal(Amount);

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
                        dash.statusId = "";
                        dash.raiser = Convert.ToString(dt.Rows[i]["doc_raiser_name"].ToString());
                        dash.description = Convert.ToString(dt.Rows[i]["doc_desc"].ToString());
                        lstReqGrid.Add(dash);
                    }
                }

                return lstReqGrid;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return lstReqGrid;
            }
            finally
            {
                conn.Close();
            }
        }
        public IEnumerable<flexibuydashboard> getDashDetailsPR(string type, string process)
        {
            List<flexibuydashboard> lstReqGrid = new List<flexibuydashboard>();
            try
            {
                getconnection();

                flexibuydashboard dash;
                DataTable dt = new DataTable();
                cmd = new SqlCommand("[pr_fb_trn_tdashboardPR]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EMPLOYEEGID", SqlDbType.BigInt).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@TYPE", SqlDbType.VarChar).Value = type;
                cmd.Parameters.Add("@PROCESS", SqlDbType.VarChar).Value = process;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dash = new flexibuydashboard();
                        dash.prsrNo = i + 1;
                        dash.prgid = dt.Rows[i]["prheader_gid"].ToString();
                        dash.prRefNo = dt.Rows[i]["prheader_refno"].ToString();
                        dash.prDate = dt.Rows[i]["prheader_date"].ToString();
                        dash.prBranch = dt.Rows[i]["branch"].ToString();
                        dash.prDept = dt.Rows[i]["employee_dept_name"].ToString();
                        dash.prDesc = dt.Rows[i]["prheader_desc"].ToString();
                        dash.prReqFor = dt.Rows[i]["requestfor_name"].ToString();
                        dash.prStatus = dt.Rows[i]["status_name"].ToString();
                        lstReqGrid.Add(dash);
                    }
                }
                return lstReqGrid;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return lstReqGrid;
            }
            finally
            {
                conn.Close();
            }
        }

        public IEnumerable<flexibuydashboard> getDashDetailsPAR(string type, string process)
        {
            List<flexibuydashboard> lstReqGrid = new List<flexibuydashboard>();
            try
            {
                getconnection();

                flexibuydashboard dash;
                DataTable dt = new DataTable();
                cmd = new SqlCommand("[pr_fb_trn_tdashboardPR]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EMPLOYEEGID", SqlDbType.BigInt).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@TYPE", SqlDbType.VarChar).Value = type;
                cmd.Parameters.Add("@PROCESS", SqlDbType.VarChar).Value = process;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dash = new flexibuydashboard();
                        dash.parHeader_Gid = dt.Rows[i]["parheader_gid"].ToString();
                        dash.parNo = dt.Rows[i]["parheader_refno"].ToString();
                        dash.parDate = dt.Rows[i]["parheader_date"].ToString();
                        dash.parDescription = dt.Rows[i]["parheader_desc"].ToString();
                        dash.parStatus = dt.Rows[i]["status_name"].ToString();
                        lstReqGrid.Add(dash);
                    }
                }
                return lstReqGrid;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return lstReqGrid;
            }
            finally
            {
                conn.Close();
            }
        }

        public IEnumerable<flexibuydashboard> getDashDetailsCBF(string type, string process)
        {
            List<flexibuydashboard> lstReqGrid = new List<flexibuydashboard>();
            try
            {
                getconnection();

                flexibuydashboard dash;
                DataTable dt = new DataTable();
                cmd = new SqlCommand("[pr_fb_trn_tdashboardPR]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EMPLOYEEGID", SqlDbType.BigInt).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@TYPE", SqlDbType.VarChar).Value = type;
                cmd.Parameters.Add("@PROCESS", SqlDbType.VarChar).Value = process;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dash = new flexibuydashboard();
                        dash.cbfGid = dt.Rows[i]["cbfheader_gid"].ToString();
                        dash.cbfNo = dt.Rows[i]["cbfheader_cbfno"].ToString();
                        dash.cbfDate = (dt.Rows[i]["cbfheader_date"].ToString());
                        dash.cbfEnddate = (dt.Rows[i]["cbfheader_enddate"].ToString());
                        dash.cbfProjectOwner = dt.Rows[i]["cbfheader_projectowner"].ToString();
                        dash.cbfDevi_Amo = Convert.ToDecimal(dt.Rows[i]["cbfheader_Devi_amt"].ToString());
                        dash.cbfAmo = Convert.ToDecimal(dt.Rows[i]["cbfheader_cbfamt"].ToString());
                        dash.cbfDescription = dt.Rows[i]["cbfheader_desc"].ToString();
                        dash.fincon_Bugt = dt.Rows[i]["cbfheader_isbudgeted"].ToString();
                        dash.cbfRequestfor = dt.Rows[i]["cbfheader_department"].ToString();
                        dash.cbfStatus = dt.Rows[i]["cbfheader_status"].ToString();
                        dash.cbfMode = dt.Rows[i]["cbfheader_mode"].ToString();
                        lstReqGrid.Add(dash);
                    }
                }
                return lstReqGrid;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return lstReqGrid;
            }
            finally
            {
                conn.Close();
            }
        }

        public IEnumerable<flexibuydashboard> getDashDetailsPO(string type, string process)
        {
            List<flexibuydashboard> lstReqGrid = new List<flexibuydashboard>();
            try
            {
                getconnection();

                flexibuydashboard dash;
                DataTable dt = new DataTable();
                cmd = new SqlCommand("[pr_fb_trn_tdashboardPR]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EMPLOYEEGID", SqlDbType.BigInt).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@TYPE", SqlDbType.VarChar).Value = type;
                cmd.Parameters.Add("@PROCESS", SqlDbType.VarChar).Value = process;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dash = new flexibuydashboard();
                        dash.poslno = i + 1;
                        dash.poDetgid = Convert.ToInt32(dt.Rows[i]["poheader_gid"]);
                        dash.poDate = Convert.ToString(dt.Rows[i]["poheader_date"]);
                        dash.poRefNo = dt.Rows[i]["poheader_pono"].ToString();
                        dash.povendorName = dt.Rows[i]["supplierheader_name"].ToString();
                        dash.postatus = dt.Rows[i]["status_name"].ToString();
                        dash.poAmount = dt.Rows[i]["poheader_over_total"].ToString();
                        dash.poVersion = dt.Rows[i]["poheader_version"].ToString();
                        lstReqGrid.Add(dash);
                    }
                }
                return lstReqGrid;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return lstReqGrid;
            }
            finally
            {
                conn.Close();
            }
        }

        public IEnumerable<flexibuydashboard> getDashDetailsWO(string type, string process)
        {
            List<flexibuydashboard> lstReqGrid = new List<flexibuydashboard>();
            try
            {
                getconnection();

                flexibuydashboard dash;
                DataTable dt = new DataTable();
                cmd = new SqlCommand("[pr_fb_trn_tdashboardPR]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EMPLOYEEGID", SqlDbType.BigInt).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@TYPE", SqlDbType.VarChar).Value = type;
                cmd.Parameters.Add("@PROCESS", SqlDbType.VarChar).Value = process;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dash = new flexibuydashboard();
                        dash.woslno = i + 1;
                        dash.woDetgid = Convert.ToInt32(dt.Rows[i]["poheader_gid"]);
                        dash.woDate = Convert.ToString(dt.Rows[i]["poheader_date"]);
                        dash.woRefNo = dt.Rows[i]["poheader_pono"].ToString();
                        dash.wovendorName = dt.Rows[i]["supplierheader_name"].ToString();
                        dash.wostatus = dt.Rows[i]["status_name"].ToString();
                        dash.woAmount = dt.Rows[i]["poheader_over_total"].ToString();
                        lstReqGrid.Add(dash);
                    }
                }
                return lstReqGrid;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return lstReqGrid;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<flexibuydashboard> getpodashboard()
        {

            List<flexibuydashboard> prdashboard = new List<flexibuydashboard>();
            try
            {
                getconnection();
                flexibuydashboard dash;
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_tflexidashboarddraft", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@employee_gid", SqlDbType.BigInt).Value = objCmnFunctions.GetLoginUserGid();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dash = new flexibuydashboard();
                        dash.refno = dt.Rows[i]["prheader_refno"].ToString();
                        dash.date = dt.Rows[i]["prheader_date"].ToString();
                        dash.branch = dt.Rows[i]["branch"].ToString();
                        dash.department = dt.Rows[i]["employee_dept_name"].ToString();
                        dash.requestfor = dt.Rows[i]["requestfor_name"].ToString();
                        dash.description = dt.Rows[i]["prheader_desc"].ToString();

                        prdashboard.Add(dash);

                    }
                }

                return prdashboard;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return prdashboard;
            }
            finally
            {
                conn.Close();
            }


        }


        public List<flexibuydashboard> getwodashboard()
        {
            List<flexibuydashboard> prdashboard = new List<flexibuydashboard>();
            try
            {
                getconnection();
                flexibuydashboard dash;
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_twodashboard", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dash = new flexibuydashboard();
                        dash.Requesttype = dt.Rows[i]["Requesttype"].ToString();
                        dash.draft = Convert.ToInt32(dt.Rows[i]["Draft"].ToString());
                        dash.inprocess = Convert.ToInt32(dt.Rows[i]["Inprocess"].ToString());
                        dash.approval = Convert.ToInt32(dt.Rows[i]["Approval"].ToString());
                        dash.reject = Convert.ToInt32(dt.Rows[i]["Rejected"].ToString());
                        dash.formyapproval = Convert.ToInt32(dt.Rows[i]["ForApproval"].ToString());

                        prdashboard.Add(dash);

                    }
                }

                return prdashboard;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return prdashboard;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<flexibuydashboard> getcbfdashboard()
        {
            List<flexibuydashboard> prdashboard = new List<flexibuydashboard>();
            try
            {
                getconnection();
                flexibuydashboard dash;
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_tcbfdashboard", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dash = new flexibuydashboard();
                        dash.Requesttype = dt.Rows[i]["Requesttype"].ToString();
                        dash.draft = Convert.ToInt32(dt.Rows[i]["Draft"].ToString());
                        dash.inprocess = Convert.ToInt32(dt.Rows[i]["Inprocess"].ToString());
                        dash.approval = Convert.ToInt32(dt.Rows[i]["Approval"].ToString());
                        dash.reject = Convert.ToInt32(dt.Rows[i]["Rejected"].ToString());
                        dash.formyapproval = Convert.ToInt32(dt.Rows[i]["ForApproval"].ToString());

                        prdashboard.Add(dash);

                    }
                }

                return prdashboard;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return prdashboard;
            }
            finally
            {
                conn.Close();
            }
        }
        public List<grnconfirmation> getscnconfirmation()
        {
            List<grnconfirmation> grnconfirm = new List<grnconfirmation>();
            try
            {
                getconnection();

                grnconfirmation grncfm;
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_GrnConfirmation", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectSCN");
                cmd.Parameters.AddWithValue("@logingid", Convert.ToInt32(objCmnFunctions.GetLoginUserGid().ToString()));
                cmd.CommandTimeout = 0;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        grncfm = new grnconfirmation();
                        grncfm.slno = i + 1;
                        grncfm.grnheadgid = Convert.ToInt32(dt.Rows[i]["grninwrdheader_gid"]);
                        grncfm.grndate = dt.Rows[i]["grninwrdheader_grndatetime"].ToString();
                        grncfm.grnrefno = dt.Rows[i]["grninwrdheader_refno"].ToString();
                        grncfm.poworefno = dt.Rows[i]["poheader_pono"].ToString();
                        grncfm.vendorname = dt.Rows[i]["supplierheader_name"].ToString();
                        //grncfm.powoamount = Convert.ToDouble(dt.Rows[i]["poheader_over_total"].ToString());
                        grncfm.grnQuantity = Convert.ToDecimal(dt.Rows[i]["grninwrddet_reced_qty"].ToString());
                        grnconfirm.Add(grncfm);
                    }
                }

                return grnconfirm;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return grnconfirm;
            }
            finally
            {
                conn.Close();
            }

        }
        public List<grnconfirmation> getgrnconfirmation()
        {
            List<grnconfirmation> grnconfirm = new List<grnconfirmation>();
            try
            {
                getconnection();

                grnconfirmation grncfm;
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_GrnConfirmation", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "select");
                cmd.Parameters.AddWithValue("@logingid", Convert.ToInt32(objCmnFunctions.GetLoginUserGid().ToString()));
                cmd.CommandTimeout = 0;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        grncfm = new grnconfirmation();
                        grncfm.slno = i + 1;
                        grncfm.grnheadgid = Convert.ToInt32(dt.Rows[i]["grninwrdheader_gid"]);
                        grncfm.grndate = dt.Rows[i]["grninwrdheader_grndatetime"].ToString();
                        grncfm.grnrefno = dt.Rows[i]["grninwrdheader_refno"].ToString();
                        grncfm.poworefno = dt.Rows[i]["poheader_pono"].ToString();
                        grncfm.vendorname = dt.Rows[i]["supplierheader_name"].ToString();
                        //grncfm.powoamount = Convert.ToDouble(dt.Rows[i]["poheader_over_total"].ToString());
                        grncfm.grnQuantity = Convert.ToDecimal(dt.Rows[i]["grninwrddet_reced_qty"].ToString());
                        grncfm.poVersion = dt.Rows[i]["poheader_version"].ToString();
                        grnconfirm.Add(grncfm);
                    }
                }

                return grnconfirm;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return grnconfirm;
            }
            finally
            {
                conn.Close();
            }

        }
        //public List<grnconfirmation> getgrnconfirmation1(string _pono)
        //{

        //    getconnection();
        //    List<grnconfirmation> grnconfirm = new List<grnconfirmation>();
        //    grnconfirmation grncfm;
        //    DataTable dt = new DataTable();
        //    cmd = new SqlCommand("fb_trn_tgrnconfirmation", conn);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    da = new SqlDataAdapter(cmd);
        //    da.Fill(dt);

        //    if (dt.Rows.Count > 0)
        //    {

        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            grncfm = new grnconfirmation();
        //            grncfm.grndate = dt.Rows[i]["grninwrdheader_grndatetime"].ToString();
        //            grncfm.grnrefno = dt.Rows[i]["grninwrdheader_refno"].ToString();
        //            grncfm.poworefno = dt.Rows[i]["poheader_pono"].ToString();
        //            grncfm.vendorname = dt.Rows[i]["supplierheader_name"].ToString();
        //            grncfm.powoamount = Convert.ToDouble(dt.Rows[i]["poheader_over_total"].ToString());

        //            grnconfirm.Add(grncfm);
        //        }
        //    }

        //    return grnconfirm;
        //}

        public List<grnconfirmationdetails> getgrnconfirmationdetails(grnconfirmation gr, int grnheadgid)
        {
            List<grnconfirmationdetails> confirmdetails = new List<grnconfirmationdetails>();
            try
            {
                getconnection();

                grnconfirmationdetails gcd;

                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_GrnConfirmationdetails", conn);
                cmd.Parameters.Add("@grninwardheader_gid", SqlDbType.BigInt).Value = gr.grnheadgid;
                cmd.Parameters.Add("@logingid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@actionName", SqlDbType.VarChar).Value = "selectdetails";
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                HttpContext.Current.Session["grndetails"] = dt;
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        // int cnt = Convert.ToInt32(dt.Rows[i]["grninwrddet_reced_qty"].ToString());
                        //for (int k = 0; k < cnt; k++)
                        // {
                        gcd = new grnconfirmationdetails();
                        gcd.slno = i + 1;
                        gcd.grndetGid = Convert.ToInt32(dt.Rows[i]["grninwrddet_gid"].ToString());
                        gcd.grnrefno = dt.Rows[i]["grninwrdheader_refno"].ToString();
                        gcd.grndate = dt.Rows[i]["grninwrdheader_grndatetime"].ToString();
                        gcd.vendorname = dt.Rows[i]["supplierheader_name"].ToString();
                        gcd.poworefno = dt.Rows[i]["poheader_pono"].ToString();
                        gcd.dcno = dt.Rows[i]["grninwrdheader_dcno"].ToString();
                        gcd.invoiceno = dt.Rows[i]["grninwrdheader_invoiceno"].ToString();
                        gcd.productgroup = dt.Rows[i]["productGroup"].ToString();
                        gcd.productcode = dt.Rows[i]["prodservice_code"].ToString();
                        gcd.productname = dt.Rows[i]["prodservice_name"].ToString();
                        gcd.uomcode = dt.Rows[i]["uom_code"].ToString();
                        gcd.receivedqty = Convert.ToDecimal(dt.Rows[i]["grninwrddet_reced_qty"].ToString());
                        // gcd.receivedqty = 1;
                        gcd.receiveddate = dt.Rows[i]["grninwrddet_reced_date"].ToString();
                        gcd.manfName = dt.Rows[i]["grninwrddet_mft_name"].ToString();
                        gcd.assetSlno = dt.Rows[i]["grninwrddet_assetsrlno"].ToString();
                        gcd.IsSerial = dt.Rows[i]["IsSerial"].ToString();
                        gcd.IsBranch = dt.Rows[i]["branch_flag"].ToString();
                        gcd.GRNType = dt.Rows[i]["GRNType"].ToString();
                        gcd.GRNdes = dt.Rows[i]["grninwrdheader_remarks"].ToString();
                        //gcd.putToUseDate = dt.Rows[i]["grninwrddet_puttousedatetime"].ToString();
                        if (dt.Rows[i]["grninwrddet_puttousedatetime"].ToString() == "" || dt.Rows[i]["grninwrddet_puttousedatetime"].ToString() == null)
                        {
                            if (dt.Rows[i]["branch_flag"].ToString() == "B" && dt.Rows[i]["GRNType"].ToString() == "D")
                            {
                                gcd.putToUseDate = dt.Rows[i]["grninwrddet_reced_date"].ToString();
                            }
                            else
                            {
                                // gcd.putToUseDate = dt.Rows[i]["today_date"].ToString();
                                gcd.putToUseDate = "";
                            }
                        }
                        else
                        {
                            gcd.putToUseDate = dt.Rows[i]["grninwrddet_puttousedatetime"].ToString();
                        }
                        //gcd.putToUseDate = dt.Rows[i]["grninwrddet_reced_date"].ToString();
                        confirmdetails.Add(gcd);
                        // }//
                        //  HttpContext.Current.Session["grndetails"] = confirmdetails;
                    }

                }

                return confirmdetails;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return confirmdetails;
            }
            finally
            {
                conn.Close();
            }

        }

        public List<grnconfirmationdetails> getscnconfirmationdetails(grnconfirmation gr, int grnheadgid)
        {
            List<grnconfirmationdetails> confirmdetails = new List<grnconfirmationdetails>();
            try
            {
                getconnection();

                grnconfirmationdetails gcd;

                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_ScnConfirmationdetails", conn);
                cmd.Parameters.Add("@grninwardheader_gid", SqlDbType.BigInt).Value = gr.grnheadgid;
                cmd.Parameters.Add("@logingid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@actionName", SqlDbType.VarChar).Value = "selectdetails";
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                HttpContext.Current.Session["grndetails"] = dt;
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        // int cnt = Convert.ToInt32(dt.Rows[i]["grninwrddet_reced_qty"].ToString());
                        //for (int k = 0; k < cnt; k++)
                        // {
                        gcd = new grnconfirmationdetails();
                        gcd.slno = i + 1;
                        gcd.grndetGid = Convert.ToInt32(dt.Rows[i]["grninwrddet_gid"].ToString());
                        gcd.grnrefno = dt.Rows[i]["grninwrdheader_refno"].ToString();
                        gcd.grndate = dt.Rows[i]["grninwrdheader_grndatetime"].ToString();
                        gcd.vendorname = dt.Rows[i]["supplierheader_name"].ToString();
                        gcd.poworefno = dt.Rows[i]["poheader_pono"].ToString();
                        gcd.dcno = dt.Rows[i]["grninwrdheader_dcno"].ToString();
                        gcd.invoiceno = dt.Rows[i]["grninwrdheader_invoiceno"].ToString();
                        gcd.productgroup = dt.Rows[i]["productGroup"].ToString();
                        gcd.productcode = dt.Rows[i]["prodservice_code"].ToString();
                        gcd.productname = dt.Rows[i]["prodservice_name"].ToString();
                        gcd.uomcode = dt.Rows[i]["uom_code"].ToString();
                        gcd.receivedqty = Convert.ToDecimal(dt.Rows[i]["grninwrddet_reced_qty"].ToString());
                        //  gcd.receivedqty = 1;
                        gcd.receiveddate = dt.Rows[i]["grninwrddet_reced_date"].ToString();
                        gcd.manfName = dt.Rows[i]["grninwrddet_mft_name"].ToString();
                        gcd.assetSlno = dt.Rows[i]["grninwrddet_assetsrlno"].ToString();
                        //gcd.putToUseDate = dt.Rows[i]["grninwrddet_puttousedatetime"].ToString();
                        gcd.putToUseDate = dt.Rows[i]["grninwrddet_reced_date"].ToString();
                        confirmdetails.Add(gcd);
                        // }//
                        //  HttpContext.Current.Session["grndetails"] = confirmdetails;
                    }

                }

                return confirmdetails;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return confirmdetails;
            }
            finally
            {
                conn.Close();
            }

        }
        public PrSumEntity GetPIPSumry()
        {
            PrSumEntity prsument = new PrSumEntity();
            try
            {
                getconnection();


                PrsummaryModel prsummod;
                prsument.lstprSum = new List<PrsummaryModel>();

                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_prsummarylist", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@employee_gid", SqlDbType.BigInt).Value = objCmnFunctions.GetLoginUserGid();
                // cmd.Parameters.Add("@prheader_gid", SqlDbType.BigInt).Value = 0;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "pip";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        prsummod = new PrsummaryModel();
                        prsummod.srNo = i + 1;
                        prsummod.Prgid = dt.Rows[i]["prheader_gid"].ToString();
                        prsummod.prRefNo = dt.Rows[i]["prheader_refno"].ToString();
                        prsummod.prDate = dt.Rows[i]["prheader_date"].ToString();
                        prsummod.prBranch = dt.Rows[i]["branch"].ToString();
                        prsummod.prDept = dt.Rows[i]["employee_dept_name"].ToString();
                        prsummod.prDesc = dt.Rows[i]["prheader_desc"].ToString();
                        prsummod.prReqFor = dt.Rows[i]["requestfor_name"].ToString();
                        prsummod.prStatus = dt.Rows[i]["status_name"].ToString();
                        prsument.lstprSum.Add(prsummod);
                    }
                }
                return prsument;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return prsument;
            }
            finally
            {
                conn.Close();
            }


        }


        public PrSumEntity GetPrsupervisorSumry()
        {
            PrSumEntity prsument = new PrSumEntity();
            try
            {
                getconnection();

                PrsummaryModel prsummod;
                prsument.lstprSum = new List<PrsummaryModel>();

                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_prsummarylist", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@employee_gid", SqlDbType.BigInt).Value = objCmnFunctions.GetLoginUserGid();
                // cmd.Parameters.Add("@prheader_gid", SqlDbType.BigInt).Value = 0;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "prsupervisor";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        prsummod = new PrsummaryModel();
                        prsummod.srNo = i + 1;
                        prsummod.Prgid = dt.Rows[i]["prheader_gid"].ToString();
                        prsummod.prRefNo = dt.Rows[i]["prheader_refno"].ToString();
                        prsummod.prDate = dt.Rows[i]["prheader_date"].ToString();
                        prsummod.prBranch = dt.Rows[i]["branch"].ToString();
                        prsummod.prDept = dt.Rows[i]["employee_dept_name"].ToString();
                        prsummod.prDesc = dt.Rows[i]["prheader_desc"].ToString();
                        prsummod.prReqFor = dt.Rows[i]["requestfor_name"].ToString();
                        prsummod.prStatus = dt.Rows[i]["status_name"].ToString();
                        prsument.lstprSum.Add(prsummod);
                    }
                }
                return prsument;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return prsument;
            }
            finally
            {
                conn.Close();
            }

        }

        public List<PrSumEntity> GetBranchList()
        {
            List<PrSumEntity> prsumentbranch = new List<PrSumEntity>();
            try
            {
                getconnection();
                PrSumEntity prBranchlist;

                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_bracnhcode", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        prBranchlist = new PrSumEntity();
                        prBranchlist.branchGid = Convert.ToInt32(dt.Rows[i]["branch_gid"].ToString());
                        prBranchlist.branchName = dt.Rows[i]["branch_code"].ToString();
                        prsumentbranch.Add(prBranchlist);
                    }
                }
                return prsumentbranch;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return prsumentbranch;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<PrSumEntity> GetStatusList()
        {
            List<PrSumEntity> prsumentstatus = new List<PrSumEntity>();
            try
            {
                getconnection();

                PrSumEntity prStatuslist;

                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_status", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        prStatuslist = new PrSumEntity();

                        if (dt.Rows[i]["status_gid"].ToString() == "2")
                        {
                            prStatuslist.statusGid = Convert.ToInt32(dt.Rows[i]["status_gid"].ToString());
                            prStatuslist.statusName = dt.Rows[i]["status_name"].ToString();
                        }
                        if (dt.Rows[i]["status_gid"].ToString() == "1" || dt.Rows[i]["status_gid"].ToString() == "5" || dt.Rows[i]["status_gid"].ToString() == "6")
                        {
                            prStatuslist.statusGid = Convert.ToInt32(dt.Rows[i]["status_gid"].ToString());
                            prStatuslist.statusName = dt.Rows[i]["status_name"].ToString();
                        }
                        prsumentstatus.Add(prStatuslist);
                    }
                }
                return prsumentstatus;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return prsumentstatus;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<PrSumEntity> GetRequestForList()
        {

            List<PrSumEntity> prsumentrequestfor = new List<PrSumEntity>();
            try
            {
                getconnection();
                PrSumEntity prRequestforlist;

                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_trequestfor", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        prRequestforlist = new PrSumEntity();
                        prRequestforlist.requestForGid = Convert.ToInt32(dt.Rows[i]["requestfor_gid"].ToString());
                        prRequestforlist.requestForName = dt.Rows[i]["requestfor_name"].ToString();
                        prsumentrequestfor.Add(prRequestforlist);
                    }
                }
                return prsumentrequestfor;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return prsumentrequestfor;
            }
            finally
            {
                conn.Close();
            }

        }

        public List<PrSumEntity> GetFcccList()
        {
            List<PrSumEntity> prsumentfccc = new List<PrSumEntity>();
            try
            {
                getconnection();
                PrSumEntity prfccclist;

                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_tfccc", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        prfccclist = new PrSumEntity();
                        prfccclist.fcccGid = Convert.ToInt32(dt.Rows[i]["fccc_gid"].ToString());
                        prfccclist.fcccName = dt.Rows[i]["fccc_code"].ToString();
                        prsumentfccc.Add(prfccclist);
                    }
                }
                return prsumentfccc;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return prsumentfccc;
            }
            finally
            {
                conn.Close();
            }

        }

        public PrSumEntity GetProdServDetails(int id)
        {
            PrSumEntity prprodserv = new PrSumEntity();
            try
            {
                getconnection();
                Product prprod;
                prprodserv.lstproduct = new List<Product>();

                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_prodservicelist", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prodservicegid", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@actionName", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@prodservicecode", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@prodserviceflag", SqlDbType.VarChar).Value = "";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["prodservice_flag"].ToString() == "P")
                        {
                            prprod = new Product();

                            prprod.srNo = i + 1;

                            prprod.product_gid = dt.Rows[i]["prodservice_gid"].ToString();
                            prprod.product_Code = dt.Rows[i]["prodservice_code"].ToString();
                            prprod.product_Name = dt.Rows[i]["prodservice_name"].ToString();
                            prprod.product_Description = dt.Rows[i]["prodservice_description"].ToString();
                            prprodserv.lstproduct.Add(prprod);
                        }
                        if (dt.Rows[i]["prodservice_flag"].ToString() == "S")
                        {
                            prprod = new Product();
                            prprod.srNo = i + 1;
                            prprod.product_gid = dt.Rows[i]["prodservice_gid"].ToString();
                            prprod.service_Code = dt.Rows[i]["prodservice_code"].ToString();
                            prprod.service_Name = dt.Rows[i]["prodservice_name"].ToString();
                            prprod.service_Description = dt.Rows[i]["prodservice_description"].ToString();
                            prprodserv.lstproduct.Add(prprod);
                        }
                    }
                }
                return prprodserv;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return prprodserv;
            }
            finally
            {
                conn.Close();
            }

        }

        public List<PrSumEntity> GetProductGroupList(string radio1)
        {
            List<PrSumEntity> prprodservgroup = new List<PrSumEntity>();
            try
            {
                getconnection();

                PrSumEntity prprodservgrp;

                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_productgroup", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prodserviceflag", SqlDbType.VarChar).Value = radio1;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        prprodservgrp = new PrSumEntity();
                        prprodservgrp.productGroupGid = Convert.ToInt32(dt.Rows[i]["prodservice_gid"].ToString());
                        prprodservgrp.productGroupName = dt.Rows[i]["prodservice_name"].ToString();
                        prprodservgroup.Add(prprodservgrp);


                    }

                }
                return prprodservgroup;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return prprodservgroup;
            }
            finally
            {
                conn.Close();
            }

        }

        public List<Product> GetSelectedProduct(Product code)
        {
            List<Product> selectedDetails = new List<Product>();
            try
            {
                getconnection();
                Product prprodservgrp;

                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_prodservicelist", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prodservicegid", SqlDbType.Int).Value = 0;
                if (code.product_Code != null)
                {
                    cmd.Parameters.Add("@prodservicecode", SqlDbType.VarChar).Value = code.product_Code;
                }
                else
                {
                    cmd.Parameters.Add("@prodservicecode", SqlDbType.VarChar).Value = 0;
                }
                cmd.Parameters.Add("@actionName", SqlDbType.VarChar).Value = "selectBycode";
                cmd.Parameters.Add("@prodserviceflag", SqlDbType.VarChar).Value = "P";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        prprodservgrp = new Product();
                        prprodservgrp.product_Code = dt.Rows[i]["prodservice_code"].ToString();
                        prprodservgrp.product_Name = dt.Rows[i]["prodservice_name"].ToString();
                        prprodservgrp.product_Description = dt.Rows[i]["prodservice_description"].ToString();
                        selectedDetails.Add(prprodservgrp);

                    }

                }

                return selectedDetails;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return selectedDetails;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<Product> GetSelectedService(Product code)
        {
            List<Product> selectedDetails1 = new List<Product>();
            try
            {
                getconnection();

                Product prprodservgrp1;

                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_prodservicelist", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prodservicegid", SqlDbType.Int).Value = 0;
                if (code.service_Code != null)
                {
                    cmd.Parameters.Add("@prodservicecode", SqlDbType.VarChar).Value = code.service_Code;
                }
                else
                {
                    cmd.Parameters.Add("@prodservicecode", SqlDbType.VarChar).Value = 0;
                }
                cmd.Parameters.Add("@actionName", SqlDbType.VarChar).Value = "selectBycode";
                cmd.Parameters.Add("@prodserviceflag", SqlDbType.VarChar).Value = "S";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        prprodservgrp1 = new Product();
                        prprodservgrp1.service_Code = dt.Rows[i]["prodservice_code"].ToString();
                        prprodservgrp1.service_Name = dt.Rows[i]["prodservice_name"].ToString();
                        prprodservgrp1.service_Description = dt.Rows[i]["prodservice_description"].ToString();
                        selectedDetails1.Add(prprodservgrp1);
                    }

                }

                return selectedDetails1;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return selectedDetails1;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<Product> GetService(DataTable dt)
        {
            List<Product> selectedDetails1 = new List<Product>();
            try
            {
                Product prprodservgrp1;

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {



                        prprodservgrp1 = new Product();
                        prprodservgrp1.service_Code = dt.Rows[i]["Service_Code"].ToString();
                        prprodservgrp1.service_Name = dt.Rows[i]["Service_Name"].ToString();
                        prprodservgrp1.service_Description = dt.Rows[i]["Service_Description"].ToString();
                        prprodservgrp1.prodservgrp_Gid = dt.Rows[i]["service_Group"].ToString();
                        prprodservgrp1.product_gid = dt.Rows[i]["product_gid"].ToString();
                        selectedDetails1.Add(prprodservgrp1);

                    }
                }
                return selectedDetails1;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return selectedDetails1;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<Product> GetProduct(DataTable dt)
        {
            List<Product> selectedDetails1 = new List<Product>();
            try
            {
                Product prprodprodgrp1;

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        prprodprodgrp1 = new Product();
                        prprodprodgrp1.srNo = Convert.ToInt32(dt.Rows[i]["Srno"].ToString());
                        prprodprodgrp1.product_Code = dt.Rows[i]["product_Code"].ToString();
                        prprodprodgrp1.product_Name = dt.Rows[i]["product_Name"].ToString();
                        prprodprodgrp1.product_Description = dt.Rows[i]["product_Description"].ToString();
                        prprodprodgrp1.product_Qty = Convert.ToDecimal(dt.Rows[i]["product_Qty"].ToString());
                        prprodprodgrp1.prodservgrp_Gid = dt.Rows[i]["product_Group"].ToString();
                        prprodprodgrp1.productUnit_Gid = dt.Rows[i]["product_Unit"].ToString();
                        prprodprodgrp1.product_gid = dt.Rows[i]["product_gid"].ToString();
                        selectedDetails1.Add(prprodprodgrp1);

                    }
                }


                return selectedDetails1;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return selectedDetails1;
            }
            finally
            {
                conn.Close();
            }
        }


        public string GeneartePrRefNo(int id, string prefix)
        {
            try
            {
                getconnection();

                PrSumEntity prsument = new PrSumEntity();
                cmd = new SqlCommand("pr_Generate_Refno", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prheader_requestgid", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@prefix", SqlDbType.VarChar).Value = prefix;
                cmd.Parameters.Add("@prheader_refno", SqlDbType.VarChar, 30);
                cmd.Parameters["@prheader_refno"].Direction = ParameterDirection.Output;
                cmd.ExecuteReader();
                string refno = cmd.Parameters["@prheader_refno"].Value.ToString();
                return refno;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                conn.Close();
            }
        }


        public PrSumEntity ViewPrDetails(PrHeader pr1, string action)
        {
            PrSumEntity pr = new PrSumEntity();
            try
            {
                getconnection();
                DataTable dt = new DataTable();
                List<Product> prdet = new List<Product>();
                Product prprodservgrpprdet;

                cmd = new SqlCommand("pr_fb_trn_viewprraisingdetails", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prheader_refno", SqlDbType.VarChar).Value = string.IsNullOrEmpty(pr1.prGid) ? pr1.prRefNo : pr1.prGid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = action;
                cmd.Parameters.Add("@remarks", SqlDbType.VarChar).Value = pr1.prRemarks;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        PrHeader prhead = new PrHeader();
                        prprodservgrpprdet = new Product();
                        prhead.prGid = dt.Rows[i]["prheader_gid"].ToString();
                        prhead.prRefNo = dt.Rows[i]["prheader_refno"].ToString();
                        prhead.prDate = Convert.ToDateTime(dt.Rows[i]["prheader_date"]).ToShortDateString();
                        prhead.prBranchType = dt.Rows[i]["prheader_branchsingle"].ToString();
                        prhead.prBranch = dt.Rows[i]["branch"].ToString();
                        prhead.prDesc = dt.Rows[i]["prheader_desc"].ToString();
                        prhead.prExpense = dt.Rows[i]["prheader_Expensetype"].ToString();
                        prhead.prFcc = dt.Rows[i]["fccc"].ToString();
                        prhead.prRaisedBy = dt.Rows[i]["raisedby"].ToString();
                        prhead.prReqFor = dt.Rows[i]["requestfor"].ToString();

                        pr.requestForGid = Convert.ToInt32(dt.Rows[i]["prheader_requestforgid"].ToString());
                        pr.statusGid = Convert.ToInt32(dt.Rows[i]["prheader_status"].ToString());
                        pr.branchGid = Convert.ToInt32(dt.Rows[i]["prheader_branchgid"].ToString());
                        pr.fcccGid = Convert.ToInt32(dt.Rows[i]["prheader_fccc"].ToString());

                        if (action == "Clone")
                        {
                            //prhead.newPrRefNo = GeneartePrRefNo(pr.requestForGid, "PR");
                            prhead.newPrRefNo = objCmnFunctions.GetSequenceNoFb("PR", Convert.ToString(pr.requestForGid), "");
                        }

                        prprodservgrpprdet.srNo = Convert.ToInt32(dt.Rows[i]["prdetails_gid"].ToString());

                        prprodservgrpprdet.prodservgrp_Gid = dt.Rows[i]["prodservice_gid"].ToString();
                        prprodservgrpprdet.productUnit_Gid = dt.Rows[i]["uom_gid"].ToString();

                        prprodservgrpprdet.prodserv_Type = dt.Rows[i]["prodservice_flag"].ToString();
                        if (prprodservgrpprdet.prodserv_Type == "P")
                        {
                            prprodservgrpprdet.product_gid = dt.Rows[i]["prdetails_prodservicegid"].ToString();
                            prprodservgrpprdet.product_Group = dt.Rows[i]["product_group"].ToString();
                            prprodservgrpprdet.product_Code = dt.Rows[i]["prodservice_code"].ToString();
                            prprodservgrpprdet.product_Name = dt.Rows[i]["prodservice_name"].ToString();
                            prprodservgrpprdet.product_Description = dt.Rows[i]["prdetails_prodservicedesc"].ToString();
                            prprodservgrpprdet.product_Unit = dt.Rows[i]["unit"].ToString();
                            prprodservgrpprdet.product_Qty = Convert.ToDecimal(dt.Rows[i]["prdetails_qty"].ToString());
                        }
                        if (prprodservgrpprdet.prodserv_Type == "S")
                        {
                            prprodservgrpprdet.product_gid = dt.Rows[i]["prdetails_prodservicegid"].ToString();
                            prprodservgrpprdet.service_Group = dt.Rows[i]["product_group"].ToString();
                            prprodservgrpprdet.service_Code = dt.Rows[i]["prodservice_code"].ToString();
                            prprodservgrpprdet.service_Name = dt.Rows[i]["prodservice_name"].ToString();
                            prprodservgrpprdet.service_Description = dt.Rows[i]["prdetails_prodservicedesc"].ToString();
                        }
                        prprodservgrpprdet.product_cost = Convert.ToString(dt.Rows[i]["pipinput_costestimation"]);
                        prprodservgrpprdet.product_rate = Convert.ToString(dt.Rows[i]["pipinput_rate"]);
                        prdet.Add(prprodservgrpprdet);

                        pr.product = prprodservgrpprdet;
                        pr.lstproduct = prdet;

                        pr.prHead = prhead;

                    }


                }
                else
                {
                    if (action == "Delete")
                    {
                        pr.lstproduct = new List<Product>();
                    }
                }

                return pr;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return pr;
            }
            finally
            {
                conn.Close();
            }
        }

        public string rejectprsup(PrHeader objpr)
        {
            try
            {
                string data = "";
                string insertcommend = "";
                CommonIUD objcommuid = new CommonIUD();
                string[,] codes1 = new string[,]            
	                {
                        {"prheader_status","6"},
                       
                    };
                string[,] whrcol = new string[,]
	                 {
                         // {"prheader_refno",prrefno.ToString()},prheader_gid
                         {"prheader_gid",objpr.prGid.ToString()}
                                                    
                     };
                string tname1 = "fb_trn_tprheader";
                insertcommend = objcommuid.UpdateCommon(codes1, whrcol, tname1);
                if (insertcommend != "Success")
                {
                    insertcommend = "0";
                }


                string[,] codw = new string[,]
            {
                {"queue_action_for","R"},
                {"queue_action_status","2"},
                {"queue_isremoved","Y"},
                {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                {"queue_action_date","sysdatetime()"},
                {"queue_action_remark",objpr.prRemarks}
            };
                string[,] codewhe = new string[,]
            {
                {"queue_ref_gid",objpr.prGid.ToString()},
                {"queue_ref_flag","5"},
                {"queue_isremoved","N"}
            };

                string tblname = "iem_trn_tqueue";

                string updatecon = objcommuid.UpdateCommon(codw, codewhe, tblname);

                return insertcommend;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                conn.Close();
            }

        }

        public string approvesupp(PrHeader objpr)
        {
            try
            {
                string data = "";
                string insertcommend = "";
                CommonIUD objcommuid = new CommonIUD();
                string[,] codes1 = new string[,]            
	                {
                        {"prheader_status","4"},
                        {"prheader_curapprstage","3"}
                       
                    };
                string[,] whrcol = new string[,]
	                 {
                         // {"prheader_refno",prrefno.ToString()}
                          {"prheader_gid",objpr.prGid.ToString()}
                          
                     };
                string tname1 = "fb_trn_tprheader";
                insertcommend = objcommuid.UpdateCommon(codes1, whrcol, tname1);
                if (insertcommend == "Success")
                {
                    string remarks = string.IsNullOrEmpty(objpr.prRemarks) ? string.Empty : objpr.prRemarks.ToString();
                    string[,] codes2 = new string[,]            
	                {
                        {"queue_action_status","1"},
                        {"queue_isremoved","N"},
                        {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                        {"queue_action_date","sysdatetime()"},
                        {"queue_action_remark",remarks}
                    };
                    string[,] whrcolmn = new string[,]
	                 {
                         // {"prheader_refno",prrefno.ToString()}
                          {"queue_ref_gid",objpr.prGid.ToString()},
                          {"queue_ref_flag","5"},
                          {"queue_isremoved","N"}
                     };
                    string tname2 = "iem_trn_tqueue";
                    insertcommend = objcommuid.UpdateCommon(codes2, whrcolmn, tname2);
                }
                if (insertcommend != "Success")
                {
                    insertcommend = "0";
                }
                return insertcommend;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                conn.Close();
            }

        }

        public PrSumEntity supervisorViewPrDetails(PrHeader pr1, string action)
        {
            PrSumEntity pr = new PrSumEntity();
            try
            {
                getconnection();
                DataTable dt = new DataTable();
                List<Product> prdet = new List<Product>();
                Product prprodservgrpprdet;
                string branchname = string.Empty;
                branchname = getbranchname(Convert.ToInt32(pr1.prGid));
                string fcccname = string.Empty;
                fcccname = getfccname(Convert.ToInt32(pr1.prGid));
                cmd = new SqlCommand("pr_fb_trn_viewprraisingdetails", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prheader_refno", SqlDbType.VarChar).Value = pr1.prGid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = action;
                cmd.Parameters.Add("@remarks", SqlDbType.VarChar).Value = pr1.prRemarks;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        PrHeader prhead = new PrHeader();
                        prprodservgrpprdet = new Product();

                        //prhead.prRefNo = pr1.prRefNo;
                        prhead.prRefNo = dt.Rows[i]["prheader_refno"].ToString();
                        prhead.prDate = Convert.ToDateTime(dt.Rows[i]["prheader_date"]).ToShortDateString();
                        prhead.prBranchType = dt.Rows[i]["prheader_branchsingle"].ToString();
                        // prhead.prBranch = dt.Rows[i]["branch"].ToString();
                        prhead.prBranch = branchname;
                        prhead.prDesc = dt.Rows[i]["prheader_desc"].ToString();
                        prhead.prExpense = dt.Rows[i]["prheader_Expensetype"].ToString();
                        // prhead.prFcc = dt.Rows[i]["fccc"].ToString();
                        prhead.prFcc = fcccname;
                        prhead.prRaisedBy = dt.Rows[i]["raisedby"].ToString();
                        prhead.prReqFor = dt.Rows[i]["requestfor"].ToString();


                        pr.requestForGid = Convert.ToInt32(dt.Rows[i]["prheader_requestforgid"].ToString());
                        pr.statusGid = Convert.ToInt32(dt.Rows[i]["prheader_status"].ToString());
                        pr.branchGid = Convert.ToInt32(dt.Rows[i]["prheader_branchgid"].ToString());
                        pr.fcccGid = Convert.ToInt32(dt.Rows[i]["prheader_fccc"].ToString());

                        if (action == "Clone")
                        {
                            //prhead.newPrRefNo = GeneartePrRefNo(pr.requestForGid, "PR");
                            prhead.newPrRefNo = objCmnFunctions.GetSequenceNoFb("PR", Convert.ToString(pr.requestForGid), "");
                        }

                        prprodservgrpprdet.srNo = Convert.ToInt32(dt.Rows[i]["prdetails_gid"].ToString());

                        prprodservgrpprdet.prodservgrp_Gid = dt.Rows[i]["prodservice_gid"].ToString();
                        prprodservgrpprdet.productUnit_Gid = dt.Rows[i]["uom_gid"].ToString();

                        prprodservgrpprdet.prodserv_Type = dt.Rows[i]["prodservice_flag"].ToString();
                        if (prprodservgrpprdet.prodserv_Type == "P")
                        {
                            prprodservgrpprdet.product_gid = dt.Rows[i]["prdetails_prodservicegid"].ToString();
                            prprodservgrpprdet.product_Group = dt.Rows[i]["product_group"].ToString();
                            prprodservgrpprdet.product_Code = dt.Rows[i]["prodservice_code"].ToString();
                            prprodservgrpprdet.product_Name = dt.Rows[i]["prodservice_name"].ToString();
                            prprodservgrpprdet.product_Description = dt.Rows[i]["prdetails_prodservicedesc"].ToString();
                            prprodservgrpprdet.product_Unit = dt.Rows[i]["unit"].ToString();
                            prprodservgrpprdet.product_Qty = Convert.ToDecimal(dt.Rows[i]["prdetails_qty"].ToString());
                            prprodservgrpprdet.product_rate = dt.Rows[i]["pipinput_rate"].ToString();
                            prprodservgrpprdet.product_cost = dt.Rows[i]["pipinput_costestimation"].ToString();
                            prprodservgrpprdet.prDet_Gid = Convert.ToInt32(dt.Rows[i]["prdetails_gid"].ToString());
                        }
                        if (prprodservgrpprdet.prodserv_Type == "S")
                        {
                            prprodservgrpprdet.product_gid = dt.Rows[i]["prdetails_prodservicegid"].ToString();
                            prprodservgrpprdet.service_Group = dt.Rows[i]["product_group"].ToString();
                            prprodservgrpprdet.service_Code = dt.Rows[i]["prodservice_code"].ToString();
                            prprodservgrpprdet.service_Name = dt.Rows[i]["prodservice_name"].ToString();
                            prprodservgrpprdet.service_Description = dt.Rows[i]["prdetails_prodservicedesc"].ToString();
                            prprodservgrpprdet.product_rate = dt.Rows[i]["pipinput_rate"].ToString();
                            prprodservgrpprdet.product_cost = dt.Rows[i]["pipinput_costestimation"].ToString();
                            prprodservgrpprdet.prDet_Gid = Convert.ToInt32(dt.Rows[i]["prdetails_gid"].ToString());
                        }


                        prdet.Add(prprodservgrpprdet);

                        pr.product = prprodservgrpprdet;
                        pr.lstproduct = prdet;

                        pr.prHead = prhead;

                    }


                }
                else
                {
                    if (action == "Delete")
                    {
                        pr.lstproduct = new List<Product>();
                    }
                }

                return pr;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return pr;
            }
            finally
            {
                conn.Close();
            }
        }

        public string getbranchname(int prgid)
        {
            string branchname = string.Empty;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_getbranchname", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prgid", prgid);
                branchname = (string)cmd.ExecuteScalar();
                return branchname;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                conn.Close();
            }

        }


        public string getfccname(int prgid)
        {
            string fccname = string.Empty;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_getfccname", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prgid", prgid);
                fccname = (string)cmd.ExecuteScalar();
                return fccname;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                conn.Close();
            }

        }
        public string GetStatusName(int statusgid)
        {
            try
            {
                getconnection();

                PrSumEntity objstatus = new PrSumEntity();
                cmd = new SqlCommand("pr_fb_trn_getstatus_branch_requestfor", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@requestforgid", 0);
                cmd.Parameters.AddWithValue("@statusgid", statusgid);
                cmd.Parameters.AddWithValue("@branchgid", 0);
                cmd.Parameters.AddWithValue("@prdate", DBNull.Value);
                cmd.Parameters.AddWithValue("@prrefno", DBNull.Value);
                objstatus.statusName = Convert.ToString(cmd.ExecuteScalar());
                return objstatus.statusName;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                conn.Close();
            }
        }

        public string GetRequestforName(int Requestforgid)
        {
            try
            {
                getconnection();
                PrSumEntity objRequestfor = new PrSumEntity();
                cmd = new SqlCommand("pr_fb_trn_getstatus_branch_requestfor", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@requestforgid", Requestforgid);
                cmd.Parameters.AddWithValue("@statusgid", 0);
                cmd.Parameters.AddWithValue("@branchgid", 0);
                cmd.Parameters.AddWithValue("@prdate", DBNull.Value);
                cmd.Parameters.AddWithValue("@prrefno", DBNull.Value);
                objRequestfor.requestForName = Convert.ToString(cmd.ExecuteScalar());
                return objRequestfor.requestForName;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                conn.Close();
            }
        }

        public string GetBranchName(int Branchgid)
        {
            try
            {
                getconnection();
                PrSumEntity objBranch = new PrSumEntity();
                cmd = new SqlCommand("pr_fb_trn_getstatus_branch_requestfor", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@requestforgid", 0);
                cmd.Parameters.AddWithValue("@statusgid", 0);
                cmd.Parameters.AddWithValue("@branchgid", Branchgid);
                cmd.Parameters.AddWithValue("@prdate", DBNull.Value);
                cmd.Parameters.AddWithValue("@prrefno", DBNull.Value);
                objBranch.branchName = Convert.ToString(cmd.ExecuteScalar());
                return objBranch.branchName;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                conn.Close();
            }
        }

        public PrSumEntity CheckExistancyPrRefNo(string prrefno)
        {
            PrSumEntity objprrefno = new PrSumEntity();
            try
            {
                getconnection();
                PrsummaryModel prsummod1;
                objprrefno.lstprSum = new List<PrsummaryModel>();

                DataTable dt = new DataTable();

                cmd = new SqlCommand("pr_fb_trn_getstatus_branch_requestfor", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@requestforgid", 0);
                cmd.Parameters.AddWithValue("@statusgid", 0);
                cmd.Parameters.AddWithValue("@branchgid", 0);
                cmd.Parameters.AddWithValue("@prdate", DBNull.Value);
                cmd.Parameters.AddWithValue("@prrefno", prrefno);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        prsummod1 = new PrsummaryModel();
                        prsummod1.srNo = i + 1;
                        prsummod1.prRefNo = dt.Rows[i]["prheader_refno"].ToString();
                        prsummod1.prDate = Convert.ToDateTime(dt.Rows[i]["prheader_date"].ToString()).ToShortDateString();
                        prsummod1.prBranch = dt.Rows[i]["branch"].ToString();
                        prsummod1.prDept = dt.Rows[i]["employee_dept_name"].ToString();
                        prsummod1.prDesc = dt.Rows[i]["prheader_desc"].ToString();
                        prsummod1.prReqFor = dt.Rows[i]["requestfor_name"].ToString();
                        prsummod1.prStatus = dt.Rows[i]["status_name"].ToString();
                        objprrefno.lstprSum.Add(prsummod1);
                    }
                }
                return objprrefno;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objprrefno;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<grnconfirmation> chechgrnprefno(string grnprnrefno)
        {
            List<grnconfirmation> grnconfirm = new List<grnconfirmation>();
            try
            {
                getconnection();

                grnconfirmation grncfm;
                DataTable dt = new DataTable();
                cmd = new SqlCommand("fb_trn_tgrnconfirmation", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prrefno", grnprnrefno);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        grncfm = new grnconfirmation();
                        grncfm.grndate = dt.Rows[i]["grninwrdheader_grndatetime"].ToString();
                        grncfm.grnrefno = dt.Rows[i]["grninwrdheader_refno"].ToString();
                        grncfm.poworefno = dt.Rows[i]["poheader_pono"].ToString();
                        grncfm.vendorname = dt.Rows[i]["supplierheader_name"].ToString();
                        grncfm.powoamount = Convert.ToDouble(dt.Rows[i]["poheader_over_total"].ToString());

                        grnconfirm.Add(grncfm);
                    }
                }

                return grnconfirm;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return grnconfirm;
            }
            finally
            {
                conn.Close();
            }
        }


        //public string grnsubmit(string grnheadergid)
        //{
        //    string data = "";



        //}
        public PrSumEntity ClonePrDetails(string prrefno)
        {
            PrSumEntity pr = new PrSumEntity();
            try
            {
                getconnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_viewprraisingdetails", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prheader_refno", SqlDbType.VarChar).Value = prrefno;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                return pr;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return pr;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<PrSumEntity> GetUomList()
        {
            List<PrSumEntity> prsumentuom = new List<PrSumEntity>();
            try
            {
                getconnection();
                PrSumEntity prUomlist;

                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_uom", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        prUomlist = new PrSumEntity();
                        prUomlist.uomGid = Convert.ToInt32(dt.Rows[i]["uom_gid"].ToString());
                        prUomlist.uomCode = dt.Rows[i]["uom_code"].ToString();
                        prsumentuom.Add(prUomlist);
                    }
                }
                return prsumentuom;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return prsumentuom;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<PrDetails> ViewPipPrDetails(string prrefno)
        {
            List<PrDetails> prdet = new List<PrDetails>();
            try
            {
                getconnection();
                DataTable dt = new DataTable();
                PrDetails prprodservgrpprdet;

                cmd = new SqlCommand("pr_fb_trn_viewprraisingdetails", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prheader_refno", SqlDbType.VarChar).Value = prrefno;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "View";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        prprodservgrpprdet = new PrDetails();

                        prprodservgrpprdet.srNo = i + 1;

                        prprodservgrpprdet.prDet_Gid = Convert.ToInt32(dt.Rows[i]["prdetails_gid"].ToString());
                        prprodservgrpprdet.prDet_Product_Group = dt.Rows[i]["product_group"].ToString();
                        prprodservgrpprdet.prDet_Product_Code = dt.Rows[i]["prodservice_code"].ToString();
                        prprodservgrpprdet.prDet_Product_Name = dt.Rows[i]["prodservice_name"].ToString();
                        prprodservgrpprdet.prDet_Unit = dt.Rows[i]["unit"].ToString();
                        prprodservgrpprdet.prDet_Department = dt.Rows[i]["department"].ToString();
                        prprodservgrpprdet.prDet_Qty = Convert.ToInt32(dt.Rows[i]["prdetails_qty"].ToString());
                        prprodservgrpprdet.prDet_Product_Description = dt.Rows[i]["prdetails_prodservicedesc"].ToString();
                        if (dt.Rows[i]["pipinput_rate"].ToString() != null && dt.Rows[i]["pipinput_rate"].ToString() != "")
                        {
                            prprodservgrpprdet.prDet_Rate = Convert.ToDecimal(dt.Rows[i]["pipinput_rate"].ToString());
                        }
                        else
                        {
                            prprodservgrpprdet.prDet_Rate = 0;
                        }
                        if (dt.Rows[i]["pipinput_costestimation"].ToString() != null && dt.Rows[i]["pipinput_costestimation"].ToString() != "")
                        {
                            prprodservgrpprdet.prDet_CostEstimation = Convert.ToDecimal(dt.Rows[i]["pipinput_costestimation"].ToString());
                        }
                        else
                        {
                            prprodservgrpprdet.prDet_CostEstimation = 0;
                        }


                        prdet.Add(prprodservgrpprdet);

                    }

                }

                return prdet;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return prdet;
            }
            finally
            {
                conn.Close();
            }
        }

        public PrSumEntity PipUpdateEstimate(string refgid, string prrefno, List<PrDetails> prd)
        {
            PrSumEntity obj = new PrSumEntity();

            decimal prheaderAmount = Convert.ToDecimal("0.00");
            try
            {
                getconnection();
                for (int i = 0; i < prd.Count; i++)
                {
                    prheaderAmount += prd[i].prDet_CostEstimation;
                    cmd = new SqlCommand("pr_fb_trn_pipinputsonpr", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@prdetgid", SqlDbType.Int).Value = prd[i].prDet_Gid;
                    cmd.Parameters.Add("@prgid", SqlDbType.VarChar).Value = refgid;
                    cmd.Parameters.Add("@productcode", SqlDbType.VarChar).Value = prd[i].prDet_Product_Code;
                    cmd.Parameters.Add("@rate", SqlDbType.Int).Value = prd[i].prDet_Rate;
                    cmd.Parameters.Add("@quantity", SqlDbType.Int).Value = prd[i].prDet_Qty;
                    cmd.Parameters.Add("@costestimation", SqlDbType.Decimal).Value = prd[i].prDet_CostEstimation;
                    cmd.ExecuteNonQuery();

                }

                string[,] update = new string[,]
                {
                    {"prheader_pramount",prheaderAmount.ToString()}
                };
                string[,] wupdate = new string[,]
                {
                    {"prheader_gid",refgid.ToString()}
                };

                string tablname = "fb_trn_tprheader";
                string updateresult = objuid.UpdateCommon(update, wupdate, tablname);


                return obj;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }
            finally
            {
                conn.Close();
            }

        }

        public PrSumEntity InsertAttachment(string prrefno, string process, List<Attachment> attach)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                getconnection();
                if (process.ToString() == "PIP")
                {
                    // DataTable dt = (DataTable)HttpContext.Current.Session["prheader_AccessTokenheader1"];
                    DataTable dt = (DataTable)HttpContext.Current.Session["AccessTokenheader1"];
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        cmd = new SqlCommand("pr_fb_trn_prpip_attachment", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@refno", SqlDbType.VarChar).Value = dt.Rows[i]["prdetails"].ToString();
                        cmd.Parameters.Add("@process", SqlDbType.VarChar).Value = process;
                        cmd.Parameters.Add("@documentname", SqlDbType.VarChar).Value = dt.Rows[i]["Documnet_Name"].ToString();
                        //  cmd.Parameters.Add("@attachdate", SqlDbType.VarChar).Value = objCmnFunctions.convertoDateTime(attach[i].attachedDate);
                        cmd.Parameters.Add("@attachdate", SqlDbType.VarChar).Value = DateTime.Now.ToString("dd/MMM/yyyy");
                        cmd.Parameters.Add("@attachtype", SqlDbType.VarChar).Value = "Quotation";
                        cmd.Parameters.Add("@attachdesc", SqlDbType.VarChar).Value = dt.Rows[i]["Document_des"].ToString();
                        cmd.Parameters.Add("@attachby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                        cmd.ExecuteNonQuery();


                    }


                }
                else
                {
                    for (int i = 0; i < attach.Count; i++)
                    {

                        cmd = new SqlCommand("pr_fb_trn_prpip_attachment", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@refno", SqlDbType.VarChar).Value = prrefno;
                        cmd.Parameters.Add("@process", SqlDbType.VarChar).Value = process;
                        cmd.Parameters.Add("@documentname", SqlDbType.VarChar).Value = attach[i].fileName;
                        // cmd.Parameters.Add("@attachdate", SqlDbType.VarChar).Value = objCmnFunctions.convertoDateTime(attach[i].attachedDate);
                        cmd.Parameters.Add("@attachdate", SqlDbType.VarChar).Value = DateTime.Now.ToString("dd/MMM/yyyy");
                        cmd.Parameters.Add("@attachtype", SqlDbType.VarChar).Value = "boqattachment";
                        cmd.Parameters.Add("@attachdesc", SqlDbType.VarChar).Value = attach[i].description;
                        cmd.Parameters.Add("@attachby", SqlDbType.Int).Value = 1;
                        cmd.ExecuteNonQuery();


                    }
                }
                return obj;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }
            finally
            {
                conn.Close();
            }
        }

        public PrSumEntity InsertPrheader(PrHeader prh, string button)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                getconnection();
                PrDetails prdet = new PrDetails();
                cmd = new SqlCommand("pr_fb_trn_prheaderinsert", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (prh.prRefNo != "" && prh.prRefNo != null)
                {
                    cmd.Parameters.Add("@prheader_refno", SqlDbType.VarChar).Value = prh.prRefNo;
                }
                else
                {
                    cmd.Parameters.Add("@prheader_refno", SqlDbType.VarChar).Value = objCmnFunctions.GetSequenceNoFb("PR", prh.prReqFor, "");
                }
                cmd.Parameters.Add("@prheader_date", SqlDbType.DateTime).Value = prh.prDate;
                cmd.Parameters.Add("@prheader_raisedgid", SqlDbType.Int).Value = Convert.ToInt32(objCmnFunctions.GetLoginUserGid());
                cmd.Parameters.Add("@prheader_fcccgid", SqlDbType.Int).Value = Convert.ToInt32(prh.prFcc);
                cmd.Parameters.Add("@prheader_branchgid", SqlDbType.Int).Value = Convert.ToInt32(prh.prBranch);
                cmd.Parameters.Add("@prheader_branchsingle", SqlDbType.Char).Value = prh.prBranchType;
                cmd.Parameters.Add("@prheader_expensetype", SqlDbType.Char).Value = prh.prExpense;
                cmd.Parameters.Add("@prheader_requestgid", SqlDbType.VarChar).Value = Convert.ToInt32(prh.prReqFor);
                cmd.Parameters.Add("@prheader_curapprstage", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@prheader_desc ", SqlDbType.VarChar).Value = prh.prDesc;
                if (button == "Save")
                {
                    cmd.Parameters.Add("@prheader_status ", SqlDbType.VarChar).Value = "Draft";
                }
                else
                {
                    cmd.Parameters.Add("@prheader_status ", SqlDbType.VarChar).Value = "Pending Approval-PIP Input";
                }
                cmd.Parameters.Add("@prheader_lockedby", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@prheader_gid", SqlDbType.Int);
                cmd.Parameters["@prheader_gid"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                prdet.prDet_PrHeader_Gid = Convert.ToInt32(cmd.Parameters["@prheader_gid"].Value);

                obj.prDet = prdet;
                return obj;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }
            finally
            {
                conn.Close();
            }
        }

        public PrSumEntity InsertPrDetails(List<Product> productLst, int prheadergid)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                getconnection();
                PrHeader pr = new PrHeader();
                for (int i = 0; i < productLst.Count; i++)
                {

                    cmd = new SqlCommand("pr_fb_trn_prdetailsinsert", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@prheader_gid", SqlDbType.Int).Value = prheadergid;
                    cmd.Parameters.Add("@prdetails_prodservgrp_gid", SqlDbType.Int).Value = Convert.ToInt32(productLst[i].prodservgrp_Gid);
                    // if (productLst[i].product_Code != null && productLst[i].service_Code == null)
                    if (productLst[i].product_Code != null)
                    {
                        cmd.Parameters.Add("@prdetail_productservice_gid", SqlDbType.VarChar).Value = string.IsNullOrEmpty(productLst[i].product_gid) ? HttpContext.Current.Session["Productgid"] : productLst[i].product_gid;
                        cmd.Parameters.Add("@prdetails_prodservcode", SqlDbType.VarChar).Value = productLst[i].product_Code;
                        cmd.Parameters.Add("@prdetails_prodservicedesc", SqlDbType.VarChar).Value = productLst[i].product_Description;
                        cmd.Parameters.Add("@prdetails_uom_code", SqlDbType.Int).Value = Convert.ToInt32(productLst[i].productUnit_Gid);
                        cmd.Parameters.Add("@prdetails_qty", SqlDbType.Decimal).Value = productLst[i].product_Qty;
                    }
                    else if (productLst[i].service_Code != null)
                    {
                        cmd.Parameters.Add("@prdetail_productservice_gid", SqlDbType.VarChar).Value = string.IsNullOrEmpty(productLst[i].product_gid) ? HttpContext.Current.Session["Productgid"] : productLst[i].product_gid;
                        cmd.Parameters.Add("@prdetails_prodservcode", SqlDbType.VarChar).Value = productLst[i].service_Code;
                        cmd.Parameters.Add("@prdetails_prodservicedesc", SqlDbType.VarChar).Value = productLst[i].service_Description;
                        cmd.Parameters.Add("@prdetails_uom_code", SqlDbType.Int).Value = 5;
                        cmd.Parameters.Add("@prdetails_qty", SqlDbType.Int).Value = 1;
                    }
                    cmd.Parameters.Add("@prheader_refno", SqlDbType.VarChar, 30);
                    cmd.Parameters["@prheader_refno"].Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();

                    pr.prRefNo = Convert.ToString(cmd.Parameters["@prheader_refno"].Value);
                }
                obj.prHead = pr;

                return obj;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }
            finally
            {
                conn.Close();
            }
        }

        public PrSumEntity EditPrheader(PrHeader prh)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                getconnection();
                PrDetails prdet = new PrDetails();

                cmd = new SqlCommand("pr_fb_trn_prheaderupdate", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prheader_refno", SqlDbType.VarChar).Value = prh.prRefNo;
                cmd.Parameters.Add("@prheader_date", SqlDbType.DateTime).Value = prh.prDate;
                cmd.Parameters.Add("@prheader_raisedgid", SqlDbType.Int).Value = Convert.ToInt32(objCmnFunctions.GetLoginUserGid());
                cmd.Parameters.Add("@prheader_fcccgid", SqlDbType.Int).Value = Convert.ToInt32(prh.prFcc);
                cmd.Parameters.Add("@prheader_branchgid", SqlDbType.Int).Value = Convert.ToInt32(prh.prBranch);
                cmd.Parameters.Add("@prheader_branchsingle", SqlDbType.Char).Value = prh.prBranchType;
                cmd.Parameters.Add("@prheader_expensetype", SqlDbType.Char).Value = prh.prExpense;
                cmd.Parameters.Add("@prheader_requestgid", SqlDbType.VarChar).Value = Convert.ToInt32(prh.prReqFor);
                cmd.Parameters.Add("@prheader_desc ", SqlDbType.VarChar).Value = prh.prDesc;
                cmd.Parameters.Add("@prheader_status ", SqlDbType.VarChar).Value = 7;
                cmd.Parameters.Add("@prheader_lockedby", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@prheader_gid", SqlDbType.Int);
                cmd.Parameters["@prheader_gid"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                prdet.prDet_PrHeader_Gid = Convert.ToInt32(cmd.Parameters["@prheader_gid"].Value);

                obj.prDet = prdet;
                return obj;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }
            finally
            {
                conn.Close();
            }
        }

        public PrSumEntity EditPrheader1(PrHeader prh)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                getconnection();

                PrDetails prdet = new PrDetails();

                cmd = new SqlCommand("pr_fb_trn_prheaderupdate", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prheader_refno", SqlDbType.VarChar).Value = prh.prRefNo;
                cmd.Parameters.Add("@prheader_date", SqlDbType.DateTime).Value = prh.prDate;
                cmd.Parameters.Add("@prheader_raisedgid", SqlDbType.Int).Value = Convert.ToInt32(objCmnFunctions.GetLoginUserGid());
                cmd.Parameters.Add("@prheader_fcccgid", SqlDbType.Int).Value = Convert.ToInt32(prh.prFcc);
                cmd.Parameters.Add("@prheader_branchgid", SqlDbType.Int).Value = Convert.ToInt32(prh.prBranch);
                cmd.Parameters.Add("@prheader_branchsingle", SqlDbType.Char).Value = prh.prBranchType;
                cmd.Parameters.Add("@prheader_expensetype", SqlDbType.Char).Value = prh.prExpense;
                cmd.Parameters.Add("@prheader_requestgid", SqlDbType.VarChar).Value = Convert.ToInt32(prh.prReqFor);
                cmd.Parameters.Add("@prheader_desc ", SqlDbType.VarChar).Value = prh.prDesc;
                cmd.Parameters.Add("@prheader_status ", SqlDbType.VarChar).Value = 1;
                cmd.Parameters.Add("@prheader_lockedby", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@prheader_gid", SqlDbType.Int);
                cmd.Parameters["@prheader_gid"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                prdet.prDet_PrHeader_Gid = Convert.ToInt32(cmd.Parameters["@prheader_gid"].Value);

                obj.prDet = prdet;
                return obj;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }
            finally
            {
                conn.Close();
            }
        }

        public PrSumEntity pipheader(PrHeader prh)
        {
            int empid = 0;
            PrSumEntity obj = new PrSumEntity();
            try
            {
                getconnection();

                PrDetails prdet = new PrDetails();

                cmd = new SqlCommand("pr_fb_trn_pipheaderupdate", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prheader_refno", SqlDbType.VarChar).Value = prh.prRefNo;
                cmd.Parameters.Add("@prheader_date", SqlDbType.DateTime).Value = prh.prDate;
                cmd.Parameters.Add("@prheader_raisedgid", SqlDbType.Int).Value = Convert.ToInt32(objCmnFunctions.GetLoginUserGid());
                cmd.Parameters.Add("@prheader_fcccgid", SqlDbType.Int).Value = Convert.ToInt32(prh.prFcc);
                cmd.Parameters.Add("@prheader_branchgid", SqlDbType.Int).Value = Convert.ToInt32(prh.prBranch);
                cmd.Parameters.Add("@prheader_branchsingle", SqlDbType.Char).Value = prh.prBranchType;
                cmd.Parameters.Add("@prheader_expensetype", SqlDbType.Char).Value = prh.prExpense;
                cmd.Parameters.Add("@prheader_requestgid", SqlDbType.VarChar).Value = Convert.ToInt32(prh.prReqFor);
                cmd.Parameters.Add("@prheader_desc ", SqlDbType.VarChar).Value = prh.prDesc;
                cmd.Parameters.Add("@prheader_remarks ", SqlDbType.VarChar).Value = string.IsNullOrEmpty(prh.prRemarks) ? string.Empty : prh.prRemarks;
                cmd.Parameters.Add("@prheader_status ", SqlDbType.VarChar).Value = 3;
                cmd.Parameters.Add("@prheader_curapprstage", SqlDbType.Int).Value = 2;
                cmd.Parameters.Add("@prheader_lockedby", SqlDbType.Int).Value = 1;

                cmd.Parameters.Add("@prheader_gid", SqlDbType.Int);
                cmd.Parameters["@prheader_gid"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                prdet.prDet_PrHeader_Gid = Convert.ToInt32(cmd.Parameters["@prheader_gid"].Value);

                obj.prDet = prdet;
                return obj;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }
            finally
            {
                conn.Close();
            }
        }

        public PrSumEntity supervisorheader(PrHeader prh)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                getconnection();

                PrDetails prdet = new PrDetails();

                cmd = new SqlCommand("pr_fb_trn_prheaderupdate", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prheader_refno", SqlDbType.VarChar).Value = prh.prRefNo;
                cmd.Parameters.Add("@prheader_date", SqlDbType.DateTime).Value = prh.prDate;
                cmd.Parameters.Add("@prheader_raisedgid", SqlDbType.Int).Value = Convert.ToInt32(2);
                cmd.Parameters.Add("@prheader_fcccgid", SqlDbType.Int).Value = Convert.ToInt32(prh.prFcc);
                cmd.Parameters.Add("@prheader_branchgid", SqlDbType.Int).Value = Convert.ToInt32(prh.prBranch);
                cmd.Parameters.Add("@prheader_branchsingle", SqlDbType.Char).Value = prh.prBranchType;
                cmd.Parameters.Add("@prheader_expensetype", SqlDbType.Char).Value = prh.prExpense;
                cmd.Parameters.Add("@prheader_requestgid", SqlDbType.VarChar).Value = Convert.ToInt32(prh.prReqFor);
                cmd.Parameters.Add("@prheader_desc ", SqlDbType.VarChar).Value = prh.prDesc;
                cmd.Parameters.Add("@prheader_status ", SqlDbType.VarChar).Value = 6;
                cmd.Parameters.Add("@prheader_lockedby", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@prheader_gid", SqlDbType.Int);
                cmd.Parameters["@prheader_gid"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();

                prdet.prDet_PrHeader_Gid = Convert.ToInt32(cmd.Parameters["@prheader_gid"].Value);

                obj.prDet = prdet;
                return obj;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }
            finally
            {
                conn.Close();
            }
        }
        public PrSumEntity EditPrDetails(List<Product> productLst, int prheadergid)
        {
            int Result = 0;
            PrSumEntity obj = new PrSumEntity();
            try
            {
                getconnection();

                for (int i = 0; i < productLst.Count; i++)
                {
                    cmd = new SqlCommand("pr_fb_trn_prdetailsupdate", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@prheader_gid", SqlDbType.Int).Value = prheadergid;
                    cmd.Parameters.Add("@prdetails_prodservgrp_gid", SqlDbType.VarChar).Value = productLst[i].prodservgrp_Gid;
                    if (productLst[i].product_Code != null && productLst[i].service_Code == null)
                    {

                        cmd.Parameters.Add("@prdetails_prodservcode ", SqlDbType.VarChar).Value = productLst[i].product_Code;
                        cmd.Parameters.Add("@prdetails_prodservicedesc ", SqlDbType.VarChar).Value = productLst[i].product_Description;
                        cmd.Parameters.Add("@prdetails_uom_code", SqlDbType.Int).Value = Convert.ToInt32(productLst[i].productUnit_Gid);
                        cmd.Parameters.Add("@prdetails_qty", SqlDbType.Decimal).Value = productLst[i].product_Qty;
                    }
                    else
                    {
                        cmd.Parameters.Add("@prdetails_prodservcode ", SqlDbType.VarChar).Value = productLst[i].service_Code;
                        cmd.Parameters.Add("@prdetails_prodservicedesc ", SqlDbType.VarChar).Value = productLst[i].service_Description;
                        cmd.Parameters.Add("@prdetails_uom_code", SqlDbType.Int).Value = 1;
                        cmd.Parameters.Add("@prdetails_qty", SqlDbType.Int).Value = 0;
                    }
                    Result = cmd.ExecuteNonQuery();
                }
                return obj;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }
            finally
            {
                conn.Close();
            }
        }

        //public PrSumEntity EditsavePrDetails(List<Product> productLst, int prheadergid)
        //{

        //    try
        //    {

        //        getconnection();

        //        PrSumEntity obj = new PrSumEntity();
        //        for (int i = 0; i < productLst.Count; i++)
        //        {
        //            cmd = new SqlCommand("pr_fb_trn_prdetailsupdate", conn);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add("@prheader_gid", SqlDbType.Int).Value = prheadergid;
        //            cmd.Parameters.Add("@prdetails_prodservgrp_gid", SqlDbType.Int).Value = productLst[i].prodservgrp_Gid;
        //            if (productLst[i].product_Code != null && productLst[i].service_Code == null)
        //            {

        //                cmd.Parameters.Add("@prdetails_prodservcode ", SqlDbType.VarChar).Value = productLst[i].product_Code;
        //                cmd.Parameters.Add("@prdetails_prodservicedesc ", SqlDbType.VarChar).Value = productLst[i].product_Description;
        //                cmd.Parameters.Add("@prdetails_uom_code", SqlDbType.Int).Value = productLst[i].productUnit_Gid;
        //                cmd.Parameters.Add("@prdetails_qty", SqlDbType.Int).Value = productLst[i].product_Qty;
        //            }
        //            else
        //            {
        //                cmd.Parameters.Add("@prdetails_prodservcode ", SqlDbType.VarChar).Value = productLst[i].service_Code;
        //                cmd.Parameters.Add("@prdetails_prodservicedesc ", SqlDbType.VarChar).Value = productLst[i].service_Description;
        //                cmd.Parameters.Add("@prdetails_uom_code", SqlDbType.Int).Value = 1;
        //                cmd.Parameters.Add("@prdetails_qty", SqlDbType.Int).Value = 0;
        //            }
        //            cmd.ExecuteNonQuery();
        //        }
        //        return obj;
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}


        public string convertoDate(string date1)
        {
            try
            {
                string date2 = date1;
                string convdate = string.Empty;
                DateTime convdate2 = Convert.ToDateTime(date1);
                string format = "yyyy/MM/dd";
                convdate = convdate2.ToString(format);
                return convdate;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                conn.Close();
            }

        }


        public PrSumEntity PRAttachmentname(PrSumEntity filename)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();
                Attachment obj_getprdetails;

                obj.attachment = new List<Attachment>();


                if (filename.attachName == null || filename.attachName == "")
                {
                    if (HttpContext.Current.Session["prheader_AccessToken"] != "" && HttpContext.Current.Session["prheader_AccessToken"] != null)
                    {
                        dt = (DataTable)HttpContext.Current.Session["prheader_AccessToken"];
                    }
                    else
                    {

                        dt.Columns.Add("Documnet_Name", typeof(string));
                        dt.Columns.Add("Attachment_Date", typeof(string));
                        dt.Columns.Add("Document_des", typeof(string));
                    }
                    if ((filename.attachmentDate != "" && filename.attachName != "" && filename.attachmentDesc != "") && (filename.attachmentDate != null && filename.attachName != null && filename.attachmentDesc != null))
                    {
                        dt.Rows.Add(filename.attachName, convertoDate(filename.attachmentDate.ToString()), filename.attachmentDesc);

                    }

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            obj_getprdetails = new Attachment();

                            obj_getprdetails.attachedDate = dt.Rows[i]["Attachment_Date"].ToString();
                            obj_getprdetails.fileName = dt.Rows[i]["Documnet_Name"].ToString();
                            obj_getprdetails.description = dt.Rows[i]["Document_des"].ToString();
                            obj.attachment.Add(obj_getprdetails);
                        }
                    }
                    System.Web.HttpContext.Current.Session["prheader_AccessToken"] = dt;
                }
                else
                {
                    if (HttpContext.Current.Session["prheader_AccessTokenheader"] != "" && HttpContext.Current.Session["prheader_AccessTokenheader"] != null)
                    {
                        dt2 = (DataTable)HttpContext.Current.Session["prheader_AccessTokenheader"];
                    }
                    else
                    {

                        dt2.Columns.Add("prdetails", typeof(string));
                        dt2.Columns.Add("Documnet_Name", typeof(string));
                        dt2.Columns.Add("Attachment_Date", typeof(string));
                        dt2.Columns.Add("Document_des", typeof(string));
                    }
                    if ((filename.attachmentDate != "" && filename.attachName != "" && filename.attachmentDesc != "") && (filename.attachmentDate != null && filename.attachName != null && filename.attachmentDesc != null))
                    {

                        dt2.Rows.Add(filename.documentName, filename.attachName, convertoDate(filename.attachmentDate.ToString()), filename.attachmentDesc);
                    }

                    if (dt2.Rows.Count > 0)
                    {

                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            obj_getprdetails = new Attachment();
                            obj_getprdetails.attachGid = Convert.ToString(i + 1);
                            //obj_getprdetails.attachedDate = dt2.Rows[i]["Attachment_Date"].ToString();
                            //obj_getprdetails.fileName = dt2.Rows[i]["Documnet_Name"].ToString();
                            //obj_getprdetails.description = dt2.Rows[i]["Document_des"].ToString();
                            obj_getprdetails.attachedDate = string.IsNullOrEmpty(dt2.Rows[i]["Attachment_Date"].ToString()) ? dt2.Rows[i]["AttachmentDate"].ToString() : dt2.Rows[i]["Attachment_Date"].ToString();
                            obj_getprdetails.fileName = string.IsNullOrEmpty(dt2.Rows[i]["Documnet_Name"].ToString()) ? dt2.Rows[i]["Filename"].ToString() : dt2.Rows[i]["Documnet_Name"].ToString();
                            obj_getprdetails.description = string.IsNullOrEmpty(dt2.Rows[i]["Document_des"].ToString()) ? dt2.Rows[i]["AttachmentDescription"].ToString() : dt2.Rows[i]["Document_des"].ToString();
                            obj.attachment.Add(obj_getprdetails);
                        }
                    }

                    System.Web.HttpContext.Current.Session["prheader_AccessTokenheader"] = dt2;
                }

                return obj;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }
            finally
            {
                conn.Close();
            }
        }

        public PrSumEntity PIPAttachmentname(PrSumEntity filename, int row)
        {
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();
            PrSumEntity obj = new PrSumEntity();
            dt2.Clear();
            Attachment obj_getprdetails;

            obj.attachment = new List<Attachment>();

            HttpContext.Current.Session["row"] = row;


            try
            {

                if (filename.attachName == null || filename.attachName == "")
                {
                    if (HttpContext.Current.Session["AccessToken1"] != "" && HttpContext.Current.Session["AccessToken1"] != null)
                    {
                        dt = (DataTable)HttpContext.Current.Session["AccessToken1"];
                    }
                    else
                    {
                        dt.Columns.Add("Documnet_Name", typeof(string));
                        dt.Columns.Add("Attachment_Date", typeof(string));
                        dt.Columns.Add("Document_des", typeof(string));
                    }
                    if ((filename.attachmentDate != "" && filename.attachName != "" && filename.attachmentDesc != "") && (filename.attachmentDate != null && filename.attachName != null && filename.attachmentDesc != null))
                    {
                        dt.Rows.Add(filename.attachName, convertoDate(filename.attachmentDate.ToString()), filename.attachmentDesc);

                    }

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            obj_getprdetails = new Attachment();
                            obj_getprdetails.attachedDate = dt.Rows[i]["Attachment_Date"].ToString();
                            obj_getprdetails.fileName = dt.Rows[i]["Documnet_Name"].ToString();
                            obj_getprdetails.description = dt.Rows[i]["Document_des"].ToString();
                            //  obj_getprdetails.attachedDate = string.IsNullOrEmpty(dt2.Rows[i]["Attachment_Date"].ToString()) ? dt2.Rows[i]["AttachmentDate"].ToString() : dt2.Rows[i]["Attachment_Date"].ToString();
                            //obj_getprdetails.fileName = string.IsNullOrEmpty(dt2.Rows[i]["Documnet_Name"].ToString()) ? dt2.Rows[i]["Filename"].ToString() : dt2.Rows[i]["Documnet_Name"].ToString();
                            //obj_getprdetails.description = string.IsNullOrEmpty(dt2.Rows[i]["Document_des"].ToString()) ? dt2.Rows[i]["AttachmentDescription"].ToString() : dt2.Rows[i]["Document_des"].ToString();
                            obj_getprdetails.attachGid = Convert.ToString(i + 1);
                            obj.attachment.Add(obj_getprdetails);
                        }
                    }
                    System.Web.HttpContext.Current.Session["AccessToken1"] = dt;
                }
                else
                {
                    dt2.Clear();

                    if (HttpContext.Current.Session["AccessTokenheader1"] != "" && HttpContext.Current.Session["AccessTokenheader1"] != null)
                    {
                        dt2 = (DataTable)HttpContext.Current.Session["AccessTokenheader1"];
                        if (dt2.Columns.Count == 6)
                        {
                            dt2 = new DataTable();
                            dt2.Columns.Add("prdetails", typeof(string));
                            dt2.Columns.Add("Documnet_Name", typeof(string));
                            dt2.Columns.Add("Attachment_Date", typeof(string));
                            dt2.Columns.Add("Document_des", typeof(string));
                        }
                    }
                    else
                    {
                        dt2.Columns.Add("prdetails", typeof(string));
                        dt2.Columns.Add("Documnet_Name", typeof(string));
                        dt2.Columns.Add("Attachment_Date", typeof(string));
                        dt2.Columns.Add("Document_des", typeof(string));


                    }
                    if ((filename.attachmentDate != "" && filename.attachName != "" && filename.attachmentDesc != "") && (filename.attachmentDate != null && filename.attachName != null && filename.attachmentDesc != null))
                    {
                        dt2.Rows.Add(row, filename.attachName, convertoDate(filename.attachmentDate.ToString()), filename.attachmentDesc);

                    }

                    if (dt2.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            obj_getprdetails = new Attachment();
                            obj_getprdetails.attachGid = dt2.Rows[i]["prdetails"].ToString();
                            obj_getprdetails.attachedDate = string.IsNullOrEmpty(dt2.Rows[i]["Attachment_Date"].ToString()) ? dt2.Rows[i]["AttachmentDate"].ToString() : dt2.Rows[i]["Attachment_Date"].ToString();
                            obj_getprdetails.fileName = string.IsNullOrEmpty(dt2.Rows[i]["Documnet_Name"].ToString()) ? dt2.Rows[i]["Filename"].ToString() : dt2.Rows[i]["Documnet_Name"].ToString();
                            obj_getprdetails.description = string.IsNullOrEmpty(dt2.Rows[i]["Document_des"].ToString()) ? dt2.Rows[i]["AttachmentDescription"].ToString() : dt2.Rows[i]["Document_des"].ToString();
                            obj.attachment.Add(obj_getprdetails);
                        }
                    }
                    System.Web.HttpContext.Current.Session["AccessTokenheader1"] = dt2;
                }

                return obj;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }
            finally
            {
                conn.Close();
            }

        }



        public List<Attachment> getattachmentdetails(string id, string process)
        {
            List<Attachment> attachment = new List<Attachment>();
            try
            {
                getconnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_prpip_viewattachment", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@refno", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@process", SqlDbType.VarChar).Value = process;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                HttpContext.Current.Session["editprattach"] = dt;
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Attachment attach = new Attachment();
                        attach.attachGid = Convert.ToString(dt.Rows[i]["attachment_gid"].ToString());
                        attach.fileName = dt.Rows[i]["attachment_filename"].ToString();
                        attach.FileTempName = dt.Rows[i]["attachment_tempfilename"].ToString();
                        attach.attachedDate = dt.Rows[i]["attachment_date"].ToString();
                        attach.description = dt.Rows[i]["attachment_desc"].ToString();
                        attach.attachedDate = DateTime.Now.ToString("dd/MM/yyyy");
                        attachment.Add(attach);
                    }
                }
                return attachment;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return attachment;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<Attachment> EditPRAttachmentList(DataTable dt)
        {
            List<Attachment> lstattach = new List<Attachment>();
            try
            {
                PrSumEntity obj = new PrSumEntity();
                Attachment obj_getattach;
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        obj_getattach = new Attachment();
                        //obj_getattach.fileName = dt.Rows[i]["Filename"].ToString();
                        //obj_getattach.attachedDate = dt.Rows[i]["AttachmentDate"].ToString();
                        //obj_getattach.description = dt.Rows[i]["AttachmentDescription"].ToString();
                        obj_getattach.fileName = dt.Rows[i]["Documnet_Name"].ToString();
                        obj_getattach.attachedDate = dt.Rows[i]["Attachment_Date"].ToString();
                        obj_getattach.description = dt.Rows[i]["Document_des"].ToString();
                        lstattach.Add(obj_getattach);
                    }
                }
                else
                {
                    lstattach = new List<Attachment>();
                }
                return lstattach;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return lstattach;
            }
            finally
            {
                conn.Close();
            }

        }

        public PrSumEntity DeleteAttachment(DataTable dt)
        {
            PrSumEntity obj = new PrSumEntity();

            try
            {
                getconnection();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string[,] wherecond = new string[,]
	                    {
                            {"attachment_filename", dt.Rows[i]["Documnet_Name"].ToString()}
                        };
                    string[,] column = new string[,]
	                   {
                             {"attachment_isremoved", "Y"}
	            
                       };
                    string tblname = "iem_trn_tattachment";
                    string deletetbl = objuid.DeleteCommon(column, wherecond, tblname);
                }
                return obj;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }
            finally
            {
                conn.Close();
            }
        }

        public PrSumEntity DeleteProductServiceDetails(DataTable dt)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                getconnection();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string[,] wherecond = new string[,]
	        {
                {"prdetails_gid", dt.Rows[i]["detail_gid"].ToString()}
            };
                    string[,] column = new string[,]
	       {
                 {"prdetails_isremoved", "Y"}
	            
           };
                    string tblname = "fb_trn_tprdetails";
                    string deletetbl = objuid.DeleteCommon(column, wherecond, tblname);
                }
                return obj;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }
            finally
            {
                conn.Close();
            }
        }
        public PrSumEntity viewqotation(string id)
        {
            PrSumEntity objsum = new PrSumEntity();
            try
            {
                getconnection();

                objsum.attachment = new List<Attachment>();
                Attachment obj_getprdetails = new Attachment();
                DataTable dt2 = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_prviewqutation", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@prdetails_gid", id);
                cmd.Parameters.AddWithValue("@prflag", 5);
                cmd.Parameters.AddWithValue("@prtype", 2);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt2);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        obj_getprdetails = new Attachment();
                        {
                            obj_getprdetails.attachedDate = dt2.Rows[i]["attachment_date"].ToString();
                            obj_getprdetails.fileName = dt2.Rows[i]["attachment_filename"].ToString();
                            obj_getprdetails.description = dt2.Rows[i]["attachment_desc"].ToString();
                            objsum.attachment.Add(obj_getprdetails);
                        }
                    }
                }


                return objsum;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objsum;
            }
            finally
            {
                conn.Close();
            }
        }

        public string insertgrncfmdetails(grnconfirmationdetails objdetails, DataTable dt)
        {
            string result = "";
            try
            {

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dt.Rows[i]["IsSerial"].ToString();
                    //if (dt.Rows[i]["grninwrddet_mft_name"].ToString() != "" && dt.Rows[i]["grninwrddet_assetsrlno"].ToString() != "" && dt.Rows[i]["grninwrddet_puttousedatetime"].ToString() != "")   
                    if (dt.Rows[i]["grninwrddet_puttousedatetime"].ToString() != "")
                    {
                        string[,] codes = new string[,]
                                {
                               
                                {"grninwrddet_mft_name", dt.Rows[i]["grninwrddet_mft_name"].ToString()},
                                {"grninwrddet_assetsrlno",dt.Rows[i]["grninwrddet_assetsrlno"].ToString()},
                                {"grninwrddet_puttousedatetime",objCmnFunctions.convertoDateTimeString(dt.Rows[i]["grninwrddet_puttousedatetime"].ToString())},
                                };
                        string[,] whrcol = new string[,]
                     {
                          {"grninwrddet_gid",objdetails.grndetGid.ToString()}
                          
                     };
                        string tname = "fb_trn_tgrninwrddet";
                        result = objuid.UpdateCommon(codes, whrcol, tname);
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
                conn.Close();
            }

        }

        #region Modified
        public List<grnconfirmationdetails> rebindconfirmdetails(DataTable dt)
        {
            List<grnconfirmationdetails> objdetlist = new List<grnconfirmationdetails>();
            try
            {
                grnconfirmationdetails objdet;

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objdet = new grnconfirmationdetails();
                        objdet.slno = i + 1;
                        objdet.grndetGid = Convert.ToInt32(dt.Rows[i]["grninwrddet_gid"].ToString());
                        //objdet.grnrefno = dt.Rows[i]["grninwrdheader_refno"].ToString();
                        //objdet.grndate = dt.Rows[i]["grninwrdheader_grndatetime"].ToString();
                        //objdet.vendorname = dt.Rows[i]["supplierheader_name"].ToString();
                        //objdet.poworefno = dt.Rows[i]["poheader_pono"].ToString();
                        //objdet.dcno = dt.Rows[i]["grninwrdheader_dcno"].ToString();
                        //objdet.invoiceno = dt.Rows[i]["grninwrdheader_invoiceno"].ToString();
                        objdet.productgroup = dt.Rows[i]["productGroup"].ToString();
                        objdet.productcode = dt.Rows[i]["prodservice_code"].ToString();
                        objdet.productname = dt.Rows[i]["prodservice_name"].ToString();
                        objdet.uomcode = dt.Rows[i]["uom_code"].ToString();
                        objdet.receivedqty = Convert.ToDecimal(dt.Rows[i]["grninwrddet_reced_qty"].ToString());
                        objdet.receiveddate = dt.Rows[i]["grninwrddet_reced_date"].ToString();
                        objdet.manfName = dt.Rows[i]["grninwrddet_mft_name"].ToString();
                        objdet.assetSlno = dt.Rows[i]["grninwrddet_assetsrlno"].ToString();
                        objdet.IsSerial = dt.Rows[i]["IsSerial"].ToString();
                        objdet.IsBranch = dt.Rows[i]["branch_flag"].ToString();
                        objdet.GRNType = dt.Rows[i]["GRNType"].ToString();
                        if (dt.Rows[i]["grninwrddet_puttousedatetime"].ToString() == "" || dt.Rows[i]["grninwrddet_puttousedatetime"].ToString() == null)
                        {
                            if (dt.Rows[i]["branch_flag"].ToString() == "B" && dt.Rows[i]["GRNType"].ToString() == "D")
                                objdet.putToUseDate = dt.Rows[i]["grninwrddet_reced_date"].ToString();
                            else
                                objdet.putToUseDate = "";
                        }
                        else
                        {
                            objdet.putToUseDate = dt.Rows[i]["grninwrddet_puttousedatetime"].ToString();
                        }
                        //if (dt.Rows[i]["grninwrddet_puttousedatetime"].ToString() == "" || dt.Rows[i]["grninwrddet_puttousedatetime"].ToString() == null)
                        //{
                        //    objdet.putToUseDate = dt.Rows[i]["grninwrddet_reced_date"].ToString();
                        //}
                        //else
                        //{
                        //    objdet.putToUseDate = dt.Rows[i]["grninwrddet_puttousedatetime"].ToString();
                        //}
                        objdetlist.Add(objdet);
                    }


                }
                return objdetlist;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objdetlist;
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion
        public string grnapprove(grnconfirmation grheadid)
        {
            try
            {
                string insertcommend = "";
                //int queue_prev = 0;
                CommonIUD objcommuid = new CommonIUD();
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_GrnConfirmationdetails", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "branchcwip");
                cmd.Parameters.AddWithValue("@grninwardheader_gid", grheadid.grnheadgid.ToString());
                string branchType = (string)cmd.ExecuteScalar();
                if (branchType == "C")
                {
                    if (HttpContext.Current.Session["grndetails"] != null)
                    {
                        DataTable dt = new DataTable();
                        dt = (DataTable)HttpContext.Current.Session["grndetails"];
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string[,] columnvalues = new string[,]
	                   {
                         {"grncwiprelease_confirmtag","Y"},
                       };

                                string[,] wherecol = new string[,]
	                  {
                          {"grncwiprelease_grninwddetgid",dt.Rows[i]["grninwrddet_gid"].ToString()},
                          {"grncwiprelease_isremoved","N"}
                          
                      };
                                string tname = "fb_trn_tgrncwiprelease";
                                insertcommend = objuid.UpdateCommon(columnvalues, wherecol, tname);
                            }
                        }
                    }
                    if (insertcommend == "Success")
                    {
                        if (branchType == "C")
                        {
                            cmd = new SqlCommand("pr_fb_trn_GrnConfirmationdetails", conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@actionName", "cwiptag");
                            cmd.Parameters.AddWithValue("@grninwardheader_gid", grheadid.grnheadgid.ToString());
                            int count = (int)cmd.ExecuteScalar();

                            if (count == 0)
                            {
                                string[,] codes1 = new string[,]
	                 {
                           {"grnconfrm_status","5"},
                           {"grnconfrm_remarks",grheadid.remarks == null ? "" : grheadid.remarks},
                           {"grnconfrm_date","SYSDATETIME()"},
                           {"grnconfrm_confirmby",Convert.ToString (objCmnFunctions.GetLoginUserGid())},
                           {"grnconfrm_isremoved","N"},
                           {"grnconfrm_grninwrdheader_gid",Convert.ToInt32(grheadid.grnheadgid).ToString()}
                     };
                                string tname1 = "fb_trn_tgrnconfrm";
                                insertcommend = objuid.InsertCommon(codes1, tname1);

                                string[,] codes2 = new string[,]
	                 {
                         {"grninwrdheader_status","5"},
                     };

                                string[,] whrcol = new string[,]
	                 {
                          {"grninwrdheader_gid",Convert.ToInt32(grheadid.grnheadgid).ToString()},
                          {"grninwrdheader_isremoved","N"}
                          
                     };
                                string tname = "fb_trn_tgrninwrdheader";
                                insertcommend = objuid.UpdateCommon(codes2, whrcol, tname);
                            }
                        }
                    }
                }
                else if (branchType == "D")
                {
                    string[,] codes1 = new string[,]
	                 {
                           {"grnconfrm_status","5"},
                           {"grnconfrm_remarks",grheadid.remarks == null ? "" : grheadid.remarks},
                           {"grnconfrm_date","SYSDATETIME()"},
                           {"grnconfrm_confirmby",Convert.ToString (objCmnFunctions.GetLoginUserGid())},
                           {"grnconfrm_isremoved","N"},
                           {"grnconfrm_grninwrdheader_gid",Convert.ToInt32(grheadid.grnheadgid).ToString()}
                     };
                    string tname1 = "fb_trn_tgrnconfrm";
                    insertcommend = objuid.InsertCommon(codes1, tname1);
                    if (insertcommend == "success")
                    {
                        string[,] columns = new string[,]
	                 {
                         {"grninwrdheader_status","5"},
                     };

                        string[,] whrcolm = new string[,]
	                 {
                          {"grninwrdheader_gid",Convert.ToInt32(grheadid.grnheadgid).ToString()},
                          {"grninwrdheader_isremoved","N"}
                          
                     };
                        string tbname = "fb_trn_tgrninwrdheader";
                        insertcommend = objuid.UpdateCommon(columns, whrcolm, tbname);
                    }
                }
                else
                {
                    insertcommend = branchType;
                }
                //if (insertcommend == "success")
                //{
                //    if (branchType == "C")
                //    {
                //        cmd = new SqlCommand("pr_fb_trn_GrnConfirmationdetails", conn);
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Parameters.AddWithValue("@actionName", "cwiptag");
                //        cmd.Parameters.AddWithValue("@grninwardheader_gid", grheadid.grnheadgid.ToString());
                //        int count = (int)cmd.ExecuteScalar();

                //        if (count == 0)
                //        {
                //            string[,] codes2 = new string[,]
                //     {
                //         {"grninwrdheader_status","5"},
                //     };

                //            string[,] whrcol = new string[,]
                //     {
                //          {"grninwrdheader_gid",Convert.ToInt32(grheadid.grnheadgid).ToString()},
                //          {"grninwrdheader_isremoved","N"}

                //     };
                //            string tname = "fb_trn_tgrninwrdheader";
                //            insertcommend = objuid.UpdateCommon(codes2, whrcol, tname);
                //       }
                //    }
                //    else
                //    {
                //        string[,] columns = new string[,]
                //     {
                //         {"grninwrdheader_status","5"},
                //     };

                //        string[,] whrcolm = new string[,]
                //     {
                //          {"grninwrdheader_gid",Convert.ToInt32(grheadid.grnheadgid).ToString()},
                //          {"grninwrdheader_isremoved","N"}

                //     };
                //        string tbname = "fb_trn_tgrninwrdheader";
                //        insertcommend = objuid.UpdateCommon(columns, whrcolm, tbname);
                //    }
                //    //if (insertcommend != "success")
                //    //{
                //    //    insertcommend = "0";
                //    //}

                //    //string[,] codesq = new string[,]
                //    //                    {
                //    //                         {"queue_action_date","sysdatetime()"},
                //    //                         {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString() },
                //    //                         {"queue_action_remark",grheadid.remarks.ToString()},
                //    //                         {"queue_action_status","1" },
                //    //                         {"queue_prev_gid",queue_prev.ToString()},
                //    //                         {"queue_isremoved","Y" }
                //    //                     };
                //    //string[,] whreq = new string[,]
                //    //                    {

                //    //                        {"queue_ref_gid",grheadid.grnheadgid.ToString()},
                //    //                        {"queue_ref_flag","11"},
                //    //                        {"queue_isremoved","N" }
                //    //                   };
                //    //string tnameq = "iem_trn_tqueue";
                //    //string insertcommendq = objuid.UpdateCommon(codesq, whreq, tnameq);
                //}
                return insertcommend;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                conn.Close();
            }
        }
        public string scnapprove(grnconfirmation grheadid)
        {
            try
            {
                string insertcommend = "";

                //int queue_prev = 0;
                CommonIUD objcommuid = new CommonIUD();
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_GrnConfirmationdetails", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "SCNbranchcwip");
                cmd.Parameters.AddWithValue("@grninwardheader_gid", grheadid.grnheadgid.ToString());
                string branchType = (string)cmd.ExecuteScalar();
                if (branchType == "1")
                {
                    string[,] codes1 = new string[,]
	                 {
                           {"grnconfrm_status","5"},
                           {"grnconfrm_remarks",(string.IsNullOrEmpty(grheadid.remarks)?"":grheadid.remarks)},
                           {"grnconfrm_date","SYSDATETIME()"},
                           {"grnconfrm_confirmby",Convert.ToString (objCmnFunctions.GetLoginUserGid())},
                           {"grnconfrm_isremoved","N"},
                           {"grnconfrm_grninwrdheader_gid",Convert.ToInt32(grheadid.grnheadgid).ToString()}
                     };
                    string tname1 = "fb_trn_tgrnconfrm";
                    insertcommend = objuid.InsertCommon(codes1, tname1);
                    if (insertcommend == "success")
                    {
                        string[,] columns = new string[,]
	                 {
                         {"grninwrdheader_status","5"},
                     };

                        string[,] whrcolm = new string[,]
	                 {
                          {"grninwrdheader_gid",Convert.ToInt32(grheadid.grnheadgid).ToString()},
                          {"grninwrdheader_isremoved","N"}
                          
                     };
                        string tbname = "fb_trn_tgrninwrdheader";
                        insertcommend = objuid.UpdateCommon(columns, whrcolm, tbname);
                    }
                }
                else
                {
                    insertcommend = branchType;
                }
                return insertcommend;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                conn.Close();
            }
        }

        #region "Delmate"
        public string Getemployeedetails(string prhgid)
        {
            string resultone = string.Empty;
            string queue_toG = string.Empty;
            string queue_toD = string.Empty;
            int queue_toR = 0;
            string queue_totype = string.Empty;
            string queue_tobranch = string.Empty;
            string queue_todept = string.Empty;
            string delmattype = string.Empty;
            decimal delamount;
            string delmatgid = string.Empty;
            string Expenses = string.Empty;
            int branchgid = 0;
            string queuengid = string.Empty;
            string queuento = string.Empty;
            decimal pramount;
            string branchType = string.Empty;
            try
            {
                getconnection();

                //cmd = new SqlCommand("pr_fb_getqueuegid", conn);pr_fb_getbranch
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Prgid", prhgid);
                //DataTable dt = new DataTable();
                //da = new SqlDataAdapter(cmd);
                //da.Fill(dt);

                cmd = new SqlCommand("pr_fb_getbranch", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Prgid", prhgid);
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {


                    branchgid = Convert.ToInt32(dt.Rows[0]["Branchgid"].ToString());
                    Expenses = dt.Rows[0]["Expensetype"].ToString();
                    pramount = Convert.ToDecimal(string.IsNullOrEmpty(dt.Rows[0]["pramount"].ToString()) ? "0.00" : dt.Rows[0]["pramount"].ToString());
                    branchType = dt.Rows[0]["branchtype"].ToString();
                    cmd = new SqlCommand("pr_getdelmat", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@delmattype_gid", 4);
                    cmd.Parameters.AddWithValue("@delmattype_branch ", branchType);
                    cmd.Parameters.AddWithValue("@delmattype_exp", Expenses);
                    cmd.Parameters.AddWithValue("@delmat_amount", pramount);

                    //var Resultgid = cmd.Parameters.Add("@delmat_gid", SqlDbType.Int);
                    //Resultgid.Direction = ParameterDirection.Output;
                    //Resultgid.Size = 64;
                    cmd.Parameters.Add("@delmat_gid", SqlDbType.Int, 64);
                    cmd.Parameters["@delmat_gid"].Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@queue_to_type", SqlDbType.Char, 1);
                    cmd.Parameters["@queue_to_type"].Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@queue_to_gid", SqlDbType.Int, 64);
                    cmd.Parameters["@queue_to_gid"].Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    delmatgid = Convert.ToString(cmd.Parameters["@delmat_gid"].Value);
                    string queuety = Convert.ToString(cmd.Parameters["@queue_to_type"].Value);
                    string queuet = Convert.ToString(cmd.Parameters["@queue_to_gid"].Value);

                    if (delmatgid != "0")
                    {
                        string[,] codesq = new string[,]
                                    {
                                         {"queue_action_date","sysdatetime()"},
                                         {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString() },
                                         {"queue_action_status","1" },
                                         {"queue_isremoved","Y" }
                                     };
                        string[,] whreq = new string[,]
                                    {
                                       // {"queue_ref_gid",getempid(refno).ToString() }
                                        {"queue_ref_gid",prhgid},
                                         {"queue_isremoved","N" }
                                   };
                        string tnameq = "iem_trn_tqueue";
                        string insertcommendq = objuid.UpdateCommon(codesq, whreq, tnameq);

                        string[,] codesIN = new string[,]
                                      {
                                           {"queue_date","sysdatetime()"},
                                           {"queue_ref_flag", "5"},
                                           {"queue_ref_gid",prhgid },
                                           {"queue_ref_status", "0"},
                                           {"queue_from",objCmnFunctions.GetLoginUserGid().ToString() } ,
                                           {"queue_to_type", queuety.ToString()},
                                           {"queue_to", queuet.ToString()},
                                           {"queue_action_for", "A"}

                                     };
                        string tnameIN = "iem_trn_tqueue";

                        string insertcommendecf = objuid.InsertCommon(codesIN, tnameIN);
                        resultone = insertcommendecf;
                    }
                }
                return resultone;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        #region "uueue"

        public string Insertqueue(int prheadergid, string queueTo, PrHeader objpr)
        {

            try
            {
                getconnection();
                string result = string.Empty;
                string flag = string.Empty;
                int qTo = 0;
                int prevGid = 0;
                int actionstatus = 0;
                Hashtable Togetlist = new Hashtable();
                Togetlist = (Hashtable)HttpContext.Current.Session["Queue_delegateslist"];

                if (Togetlist.ContainsKey(queueTo))
                {
                    string empgid = Togetlist[queueTo].ToString();
                }

                cmd = new SqlCommand("pr_fb_trn_GetRequestGroup", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                if (queueTo == "PIP")
                {
                    cmd.Parameters.AddWithValue("@group", queueTo);
                    cmd.Parameters.AddWithValue("@actionName", "select");
                    qTo = Convert.ToInt32(cmd.ExecuteScalar());
                    flag = "G";
                }
                else if (queueTo == "IT")
                {
                    cmd.Parameters.AddWithValue("@group", queueTo);
                    cmd.Parameters.AddWithValue("@actionName", "select");
                    qTo = Convert.ToInt32(cmd.ExecuteScalar());
                    flag = "G";
                }
                else
                {
                    flag = "I";
                    cmd = new SqlCommand("pr_fb_GetSupervisor", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@prgid", prheadergid.ToString());
                    qTo = (int)cmd.ExecuteScalar();

                    string[,] codeW = new string[,]
                   {
                       {"queue_action_date","sysdatetime()"},
                       {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                       {"queue_action_status","1"},
                       {"queue_action_remark",string.IsNullOrEmpty(objpr.prRemarks)? string.Empty:objpr.prRemarks.ToString()},
                       {"queue_isremoved","Y"}
                   };

                    string[,] codewh = new string[,]
                   {
                       {"queue_ref_gid",prheadergid.ToString()},
                       {"queue_ref_flag","5"},
                       {"queue_isremoved","N"}
                   };

                    string twhname = "iem_trn_tqueue";

                    result = objuid.UpdateCommon(codeW, codewh, twhname);
                    if (result == "Success")
                    {

                        cmd = new SqlCommand("pr_fb_trn_PRApprovalQueue", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@actionName", "updatequeue");
                        cmd.Parameters.AddWithValue("@refgid", prheadergid.ToString());
                        prevGid = (int)cmd.ExecuteScalar();

                        string[,] codesIN = new string[,]
                                              {
                                                   {"queue_date","sysdatetime()"},
                                                   {"queue_ref_flag", "5"},
                                                   {"queue_ref_gid",prheadergid.ToString() },
                                                   {"queue_ref_status", "1"},
                                                   {"queue_from",objCmnFunctions.GetLoginUserGid().ToString() },
                                                   {"queue_to_type", flag},
                                                   {"queue_to", qTo.ToString()},
                                                   {"queue_prev_gid",prevGid.ToString()},
                                                   {"queue_action_status",actionstatus.ToString()},
                                                   {"queue_action_for", "A"}
                                             };
                        string tnameIN = "iem_trn_tqueue";

                        result = objuid.InsertCommon(codesIN, tnameIN);
                    }

                }

                if (queueTo == "PIP" || queueTo == "IT")
                {
                    string[,] codesIN = new string[,]
                                              {
                                                   {"queue_date","sysdatetime()"},
                                                   {"queue_ref_flag", "5"},
                                                   {"queue_ref_gid",prheadergid.ToString() },
                                                   {"queue_ref_status", "1"},
                                                   {"queue_from",objCmnFunctions.GetLoginUserGid().ToString() },
                                                   {"queue_to_type", flag},
                                                   {"queue_to", qTo.ToString()},
                                                   {"queue_prev_gid","0"},
                                                   //{"queue_action_status","0"},
                                                   {"queue_action_for", "A"}
                                             };
                    string tnameIN = "iem_trn_tqueue";

                    result = objuid.InsertCommon(codesIN, tnameIN);
                }
                HttpContext.Current.Session["PRqueuegid"] = null;
                cmd = new SqlCommand("pr_fb_trn_GetRequestGroup", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@actionName", SqlDbType.VarChar).Value = "mailsubmissiononpr";
                cmd.Parameters.Add("@queue_gid", SqlDbType.Int).Value = prheadergid;
                string a2 = cmd.ExecuteScalar().ToString();
                HttpContext.Current.Session["PRqueuegid"] = a2;
                return result;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                conn.Close();
            }

        }

        #endregion

        public string Getpipemp()
        {
            DataTable dtgetemp = new DataTable();
            string empid = objCmnFunctions.GetLoginUserGid().ToString();
            string status = string.Empty;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_Getpipgroup", conn);
                cmd.Parameters.AddWithValue("@logingid", Convert.ToInt32(objCmnFunctions.GetLoginUserGid().ToString()));
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dtgetemp);
                if (dtgetemp.Rows.Count > 0)
                {
                    for (int i = 0; i < dtgetemp.Rows.Count; i++)
                    {
                        if (empid == dtgetemp.Rows[i][0].ToString())
                        {
                            status = "Y";
                        }
                    }
                }
                return status;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                conn.Close();
            }

        }

        public int getempid(string refno)
        {
            getconnection();
            int empid = 0;
            try
            {
                cmd = new SqlCommand("select prheader_gid from fb_trn_tprheader where prheader_refno='" + refno + "' and prheader_isremoved='N'", conn);
                empid = (int)cmd.ExecuteScalar();
                return empid;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }

        //public List<prsupervisior> GetdelmateApprovalqueue()
        //{
        //    // Pr_fb_GetEmployeeDetails

        //    string depid = string.Empty;
        //    string desgid = string.Empty;
        //    string gradegid = string.Empty;
        //    string rolgid = string.Empty;
        //    string prheadergid = string.Empty;

        //    prsupervisior objpr = new prsupervisior();
        //    List<prsupervisior> objjj = new List<prsupervisior>();
        //    try
        //    {
        //        getconnection();

        //        string action = "I,L,D,R,G";
        //        string[] act = action.Split(',');
        //        cmd = new SqlCommand("Pr_fb_GetEmployeeDetails", conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@EMPID", objCmnFunctions.GetLoginUserGid().ToString());
        //        DataTable dtempdet = new DataTable();
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dtempdet);
        //        if (dtempdet.Rows.Count > 0)
        //        {
        //            //depid = dtempdet.Rows[0]["Deptid"].ToString();
        //            //desgid = dtempdet.Rows[0]["Desnid"].ToString();
        //            //gradegid = dtempdet.Rows[0]["Gradeid"].ToString();
        //            //rolgid = dtempdet.Rows[0]["Rolegid"].ToString();

        //            for (int j = 0; j < dtempdet.Columns.Count; j++)
        //            {
        //                DataTable dtprlist = new DataTable();
        //                cmd = new SqlCommand("pr_fb_getdelmatflowgid", conn);
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddWithValue("@empgid", dtempdet.Rows[0][j].ToString());
        //                cmd.Parameters.AddWithValue("@action", act[j].ToString());
        //                da = new SqlDataAdapter(cmd);
        //                da.Fill(dtprlist);
        //                if (dtprlist.Rows.Count > 0)
        //                {
        //                    for (int k = 0; k < dtprlist.Rows.Count; k++)
        //                    {
        //                        if (prheadergid == string.Empty)
        //                        {
        //                            prheadergid = dtprlist.Rows[k]["queue_ref_gid"].ToString();
        //                        }
        //                        else
        //                        {
        //                            prheadergid += "," + dtprlist.Rows[k]["queue_ref_gid"].ToString();
        //                        }
        //                    }

        //                }
        //            }

        //        }



        //        cmd = new SqlCommand("pr_fb_getApprovalQueue", conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@empgid", objCmnFunctions.GetLoginUserGid().ToString());
        //        cmd.Parameters.AddWithValue("@Refflag", 5);
        //        DataTable dt = new DataTable();
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);

        //        if (dt.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                prsupervisior objpr1 = new prsupervisior();
        //                objpr1.prgid = Convert.ToInt32(dt.Rows[i]["prheader_gid"].ToString());
        //                objpr1.prRefNo = string.IsNullOrEmpty(dt.Rows[i]["prheader_refno"].ToString()) ? string.Empty : dt.Rows[i]["prheader_refno"].ToString();
        //                objpr1.prDate = string.IsNullOrEmpty(dt.Rows[i]["prheader_date"].ToString()) ? string.Empty : dt.Rows[i]["prheader_date"].ToString();
        //                objpr1.prBranch = string.IsNullOrEmpty(dt.Rows[i]["branch_name"].ToString()) ? string.Empty : dt.Rows[i]["branch_name"].ToString();
        //                objpr1.prDesc = string.IsNullOrEmpty(dt.Rows[i]["prheader_desc"].ToString()) ? string.Empty : dt.Rows[i]["prheader_desc"].ToString();
        //                objpr1.prReqFor = string.IsNullOrEmpty(dt.Rows[i]["prheader_refno"].ToString()) ? "0" : dt.Rows[i]["prheader_refno"].ToString();
        //                objpr1.prStatus = string.IsNullOrEmpty(dt.Rows[i]["prheader_status"].ToString()) ? "0" : dt.Rows[i]["prheader_status"].ToString();
        //                objjj.Add(objpr1);
        //            }


        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return objjj;
        //}

        //public string Getdelmatgid()
        //{
        //    try
        //    {

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        public List<prsupervisior> GetdelmateApprovalqueue()
        {
            // Pr_fb_GetEmployeeDetails

            string depid = string.Empty;
            string desgid = string.Empty;
            string gradegid = string.Empty;
            string rolgid = string.Empty;
            string prheadergid = string.Empty;
            string queue_type = string.Empty;

            prsupervisior objpr = new prsupervisior();
            List<prsupervisior> objjj = new List<prsupervisior>();
            try
            {
                getconnection();

                // string action = "I,L,D,R,G";
                //string[] act = action.Split(',');
                cmd = new SqlCommand("Pr_fb_GetEmployeeDetails", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@EMPID", objCmnFunctions.GetLoginUserGid().ToString());
                DataTable dtempdet = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dtempdet);

                if (dtempdet.Rows.Count > 0)
                {
                    cmd = new SqlCommand("Pr_fb_GetEmployeeDetails", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@actionName", "selectqueuetype");
                    queue_type = (string)cmd.ExecuteScalar();

                    for (int j = 0; j < dtempdet.Columns.Count; j++)
                    {
                        DataTable dtprlist = new DataTable();
                        cmd = new SqlCommand("pr_fb_getdelmatflowgid", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@empgid", dtempdet.Rows[0][j].ToString());
                        //cmd.Parameters.AddWithValue("@action", string.IsNullOrEmpty(act[j].ToString()) ? string.Empty : act[j].ToString());
                        cmd.Parameters.AddWithValue("@action", string.IsNullOrEmpty(queue_type.ToString()) ? string.Empty : queue_type.ToString());
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dtprlist);
                        if (dtprlist.Rows.Count > 0)
                        {
                            for (int k = 0; k < dtprlist.Rows.Count; k++)
                            {
                                if (prheadergid == string.Empty)
                                {
                                    prheadergid = dtprlist.Rows[k]["queue_ref_gid"].ToString();
                                }
                                else
                                {
                                    prheadergid += "," + dtprlist.Rows[k]["queue_ref_gid"].ToString();
                                }
                            }

                        }


                        string sqlquery = "select pr.prheader_gid,pr.prheader_refno ,convert(nvarchar(15),pr.prheader_date,103) as prheader_date,br.branch_code,br.branch_name ,pr.prheader_desc,pr.prheader_refno,'Inprocess' as prheader_status from fb_trn_tprheader as pr inner join iem_mst_tbranch as br on pr.prheader_branchgid=br.branch_gid and br.branch_isremoved='N'  where   pr.prheader_status=4 and pr.prheader_gid in (" + prheadergid + ") ";

                        //cmd = new SqlCommand("pr_fb_getApprovalQueue", conn);
                        //cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("@empgid", objCmnFunctions.GetLoginUserGid().ToString());
                        //cmd.Parameters.AddWithValue("@Refflag", 5);

                        cmd = new SqlCommand(sqlquery, conn);
                        DataTable dt = new DataTable();
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                prsupervisior objpr1 = new prsupervisior();
                                objpr1.prgid = Convert.ToInt32(dt.Rows[i]["prheader_gid"].ToString());
                                objpr1.prRefNo = string.IsNullOrEmpty(dt.Rows[i]["prheader_refno"].ToString()) ? string.Empty : dt.Rows[i]["prheader_refno"].ToString();
                                objpr1.prDate = string.IsNullOrEmpty(dt.Rows[i]["prheader_date"].ToString()) ? string.Empty : dt.Rows[i]["prheader_date"].ToString();
                                objpr1.prBranch = string.IsNullOrEmpty(dt.Rows[i]["branch_name"].ToString()) ? string.Empty : dt.Rows[i]["branch_name"].ToString();
                                objpr1.prDesc = string.IsNullOrEmpty(dt.Rows[i]["prheader_desc"].ToString()) ? string.Empty : dt.Rows[i]["prheader_desc"].ToString();
                                objpr1.prReqFor = string.IsNullOrEmpty(dt.Rows[i]["prheader_refno"].ToString()) ? "0" : dt.Rows[i]["prheader_refno"].ToString();
                                objpr1.prStatus = string.IsNullOrEmpty(dt.Rows[i]["prheader_status"].ToString()) ? "0" : dt.Rows[i]["prheader_status"].ToString();
                                objjj.Add(objpr1);
                            }
                        }
                    }
                }

                return objjj;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objjj;
            }
            finally
            {
                conn.Close();
            }

        }

        //public List<PrSumEntity> PopulateAuditTrail(PrSumEntity pat)
        //{
        //    try
        //    {
        //        List<PrSumEntity> objDashBoard = new List<PrSumEntity>();
        //        PrSumEntity objModel;
        //        getconnection();
        //        DataTable dt = new DataTable();
        //        DataTable dtr = new DataTable();
        //        string streject = "";
        //        string strejectnew = "";
        //        string status = "";
        //        string actionfor = "";
        //        cmd = new SqlCommand("pr_fb_trn_audittaril", conn);
        //        cmd.Parameters.AddWithValue("@gid", pat.gid);
        //        cmd.Parameters.AddWithValue("@refflag", pat.ref_flag);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        if (dt.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                string pergid = Convert.ToString(dt.Rows[i]["queue_prev_gid"].ToString());
        //                if (pergid == "0")
        //                {
        //                    streject = Convert.ToString(dt.Rows[i]["queue_from"].ToString());
        //                    string empcodnamer = Getempname(streject);
        //                    string[] datar;
        //                    datar = empcodnamer.Split(',');
        //                    objModel = new PrSumEntity();
        //                    objModel.employee_code = datar[0].ToString() + "-" + datar[1].ToString();
        //                    status = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                    objModel.action_status = GetQueueStatusHistry(status);
        //                    objModel.action_status = "Submitted";
        //                    if (pat.ref_flag == "5")
        //                    {
        //                      //  objModel.action_remarks = Convert.ToString(dt.Rows[i]["prheader_remarks"].ToString());
        //                        objModel.action_date = Convert.ToString(dt.Rows[i]["queue_date"].ToString());
        //                        objModel.queue_to_type = "PR Raiser";
        //                    }

        //                    actionfor = Convert.ToString(dt.Rows[i]["queue_action_for"].ToString());
        //                    if (actionfor == "R")
        //                    {
        //                        objModel.action_status = "Rejected";
        //                    }
        //                    if (i > 0)
        //                    {
        //                        actionfor = Convert.ToString(dt.Rows[i - 1]["queue_action_for"].ToString());
        //                    }

        //                    //string ref_no = Convert.ToString(dt.Rows[i]["queue_ref_status"].ToString());
        //                    if (actionfor == "R")
        //                    {
        //                        objModel.action_status = "ReSubmitted";
        //                    }
        //                    objDashBoard.Add(objModel);

        //                    string actions = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                    //if (actions != "")
        //                    //{
        //                        string empid = Convert.ToString(dt.Rows[i]["queue_action_by"].ToString());
        //                        string queue_type = Convert.ToString(dt.Rows[i]["queue_to_type"].ToString());
        //                        string queue_to = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
        //                        if (objModel.action_status == "Submitted" && empid == "" && (queue_type == "I" || queue_type == "E"))
        //                        {
        //                            empid = queue_to;
        //                        }
        //                        //else if (objModel.action_status == "Submitted" && empid == "" && (queue_type == "I" || queue_type == "E"))
        //                        //{
        //                        //    empid = queue_to;
        //                        //}
        //                        if (empid != "")
        //                        {
        //                            string empcodname = Getempname(empid);
        //                            string[] data;
        //                            data = empcodname.Split(',');
        //                            objModel = new PrSumEntity();
        //                            objModel.employee_code = data[0].ToString() + "-" + data[1].ToString();
        //                            objModel.action_date = Convert.ToString(dt.Rows[i]["queue_action_date"].ToString());
        //                            objModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());
        //                            status = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                            objModel.action_status = GetQueueStatusHistry(status);
        //                            objModel.queue_to_type = GetQueueRole(queue_type, "5", queue_to);
        //                            objDashBoard.Add(objModel);
        //                        }

        //                //}
        //                }
        //                else
        //                {
        //                    string queue_type = Convert.ToString(dt.Rows[i]["queue_to_type"].ToString());
        //                    string queue_to = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
        //                    string actionstt = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                    if (actionstt != "")
        //                    {
        //                        string empid = "";
        //                        objModel = new PrSumEntity();
        //                        if (queue_type == "I" || queue_type == "E")
        //                        {
        //                            empid = Convert.ToString(dt.Rows[i]["queue_to"].ToString());

        //                        }
        //                        else if (queue_type == "G" || queue_type == "R" || queue_type == "L" || queue_type == "D")
        //                        {
        //                            empid = Convert.ToString(dt.Rows[i]["queue_action_by"].ToString());
        //                        }
        //                        if (empid != "")
        //                        {
        //                            string empcodname = Getempname(empid);
        //                            string[] data;
        //                            data = empcodname.Split(',');
        //                            objModel.employee_code = data[0].ToString() + "-" + data[1].ToString();
        //                            objModel.action_date = Convert.ToString(dt.Rows[i]["queue_action_date"].ToString());
        //                            objModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());
        //                            status = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                            objModel.action_status = GetQueueStatusHistry(status);
        //                            objModel.queue_to_type = GetQueueRole(queue_type, "5", queue_to);
        //                            actionfor = Convert.ToString(dt.Rows[i]["queue_action_for"].ToString());
        //                            if (actionfor == "R")
        //                            {
        //                                objModel.action_status = "Rejected";
        //                            }
        //                            objDashBoard.Add(objModel);
        //                        }
        //                        else
        //                        {
        //                            objModel.employee_code = "";
        //                            objModel.action_date = Convert.ToString(dt.Rows[i]["queue_action_date"].ToString());
        //                            objModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());
        //                            status = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                            objModel.action_status = GetQueueStatusHistry(status);
        //                            objModel.queue_to_type = GetQueueRole(queue_type, "5", queue_to);
        //                            actionfor = Convert.ToString(dt.Rows[i]["queue_action_for"].ToString());
        //                            if (actionfor == "R")
        //                            {
        //                                objModel.action_status = "Rejected";
        //                            }
        //                            objDashBoard.Add(objModel);
        //                        }



        //                    }
        //                }

        //            }
        //        }
        //        return objDashBoard;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        conn.Close();
        //    }
        //}

        public List<PrSumEntity> PopulateAuditTrail(PrSumEntity pat)
        {
            List<PrSumEntity> objDashBoard = new List<PrSumEntity>();
            try
            {
                PrSumEntity objModel;
                int liRaiserGid = 0;
                int liCnt = 0;
                getconnection();
                DataTable dt = new DataTable();
                DataTable dtr = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_audittrail", conn);
                cmd.Parameters.AddWithValue("@gid", pat.gid);
                cmd.Parameters.AddWithValue("@refflag", Convert.ToInt32(pat.ref_flag));
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string pergid = Convert.ToString(dt.Rows[i]["queue_prev_gid"].ToString());
                        if (pergid == "0")
                        {
                            objModel = new PrSumEntity();
                            objModel.queue_gid = Convert.ToInt32(dt.Rows[i]["queue_gid"].ToString());
                            objModel.employee_code = Convert.ToString(dt.Rows[i]["queue_from_name"].ToString());
                            objModel.action_date = Convert.ToString(dt.Rows[i]["queue_date"].ToString());
                            liRaiserGid = Convert.ToInt32(dt.Rows[i]["queue_from"].ToString());
                            objModel.queue_to_type = "Raiser";
                            if (liCnt == 0)
                            {
                                objModel.action_status = "Submitted";
                            }
                            else
                            {
                                objModel.action_status = "ReSubmitted";
                            }

                            objDashBoard.Add(objModel);
                            liCnt = liCnt + 1;
                        }

                        string actions = Convert.ToString(dt.Rows[i]["queue_action_for_name"].ToString());
                        string empid = Convert.ToString(dt.Rows[i]["queue_action_by"].ToString());
                        string queue_type = Convert.ToString(dt.Rows[i]["queue_to_type"].ToString());
                        string queue_to = Convert.ToString(dt.Rows[i]["queue_to"].ToString());

                        objModel = new PrSumEntity();
                        objModel.queue_gid = Convert.ToInt32(dt.Rows[i]["queue_gid"].ToString());
                        objModel.action_status = Convert.ToString(dt.Rows[i]["queue_action_status_name"].ToString());
                        objModel.queue_to_type = Convert.ToString(dt.Rows[i]["queue_to_name"].ToString());
                        //if (dt.Rows[i]["Additional_flag"].ToString() == "N" && queue_type !="G")
                        //{
                        //    string empgid = Convert.ToString(GetEmployeeHierarchy(liRaiserGid, Convert.ToString(dt.Rows[i]["queue_to_type_name"].ToString()), Convert.ToInt32(dt.Rows[i]["queue_to"].ToString())));
                        //    string EmployeeDetails = Getempname(empgid); 
                        //    string[] data;
                        //    data = EmployeeDetails.Split(',');
                        //    objModel.employee_code = data[0].ToString() + "-" + data[1].ToString();
                        //}
                        //else
                        //{
                        //    objModel.employee_code = Convert.ToString(dt.Rows[i]["action_by_name"].ToString());
                        //}

                        //by K.Bharathidassan //
                        string employeegid = Convert.ToString(dt.Rows[i]["queue_action_by"].ToString());
                        string empiddel = Convert.ToString(dt.Rows[i]["queue_delegation_gid"].ToString());
                        string empcodname = Getempname(employeegid);
                        string[] data1;
                        data1 = empcodname.Split(',');


                        if (dt.Rows[i]["Additional_flag"].ToString() == "N"
                            && queue_type != "G"
                            && queue_type != "I"
                            && queue_type != "E"
                            && objModel.action_status != "Approved"
                            && objModel.action_status != "Rejected")
                        {
                            string empgid = Convert.ToString(GetEmployeeHierarchy(liRaiserGid, Convert.ToString(dt.Rows[i]["queue_to_type_name"].ToString()), Convert.ToInt32(dt.Rows[i]["queue_to"].ToString())));
                            string EmployeeDetails = Getempname(empgid);
                            string[] data;
                            data = EmployeeDetails.Split(',');
                            objModel.employee_code = data[0].ToString() + "-" + data[1].ToString();
                        }
                        else
                        {
                            if (queue_type == "I" || queue_type == "E")
                            {
                                if (empid == empiddel || empiddel == "0")
                                {
                                    objModel.employee_code = GetEmployeeName(Convert.ToInt32(dt.Rows[i]["queue_to"].ToString()));
                                }
                                else
                                {
                                    string empcodnamedel = Getempname(empiddel);
                                    string[] datadel;
                                    datadel = empcodnamedel.Split(',');
                                    objModel.employee_code = datadel[0].ToString() + "-" + datadel[1].ToString() + " instead of " + data1[0].ToString() + "-" + data1[1].ToString();
                                }
                            }
                            else
                            {
                                if (data1.Length > 1)
                                {
                                    if (empid == empiddel || empiddel == "0")
                                    {
                                        // objModel.employee_code = data[0].ToString() + "-" + data[1].ToString();
                                        objModel.employee_code = Convert.ToString(dt.Rows[i]["action_by_name"].ToString());
                                    }
                                    else
                                    {
                                        string empcodnamedel = Getempname(empiddel);
                                        string[] datadel;
                                        datadel = empcodnamedel.Split(',');
                                        objModel.employee_code = datadel[0].ToString() + "-" + datadel[1].ToString() + " instead of " + data1[0].ToString() + "-" + data1[1].ToString();
                                    }
                                    //objModel.empname = Gethrsdesi(data[0].ToString());
                                }
                            }
                            // objModel.employee_code = Convert.ToString(dt.Rows[i]["action_by_name"].ToString());
                        }
                        objModel.action_date = Convert.ToString(dt.Rows[i]["queue_action_date"].ToString());
                        objModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());

                        objDashBoard.Add(objModel);

                    }
                    //if (objDashBoard.Count > 0)
                    //{
                    //    objDashBoard = objDashBoard.OrderByDescending(s => s.queue_gid).ToList();
                    //}
                }
                return objDashBoard;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objDashBoard;
            }
            finally
            {
                conn.Close();
            }
        }
        public string GetEmployeeHierarchy(int raisergid = 0, string titlename = null, int titlerefgid = 0)
        {
            try
            {
                string lsResult = string.Empty;
                getconnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_chk_employee_hierarchy", conn);
                cmd.Parameters.AddWithValue("@employee_gid", raisergid);
                cmd.Parameters.AddWithValue("@title_name", titlename);
                cmd.Parameters.AddWithValue("@title_value_gid", titlerefgid);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@title_employee_gid", SqlDbType.Int, 5);
                cmd.Parameters["@title_employee_gid"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                lsResult = Convert.ToString(cmd.Parameters["@title_employee_gid"].Value.ToString());

                return lsResult;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                conn.Close();
            }
        }
        public string GetEmployeeName(int empgid = 0)
        {
            string EmpName = "";
            try
            {
                getconnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_iem_LoginDetails", conn);
                cmd.Parameters.AddWithValue("@empgid", empgid);
                cmd.Parameters.AddWithValue("@action", "employeenameforaudittrial");
                cmd.CommandType = CommandType.StoredProcedure;
                EmpName = Convert.ToString(cmd.ExecuteScalar());
                return EmpName;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                conn.Close();
            }
        }
        public string Getempname(string empgid)
        {
            string status = "";
            try
            {
                getconnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_audittaril", conn);
                cmd.Parameters.AddWithValue("@gid", empgid);
                cmd.Parameters.AddWithValue("@refflag", "E");
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    string raisername = Convert.ToString(dt.Rows[0]["employee_code"].ToString());
                    string raisercode = Convert.ToString(dt.Rows[0]["employee_name"].ToString());
                    status = raisername + "," + raisercode;
                }
                return status;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                conn.Close();
            }
        }

        public string GetQueueStatusHistry(string Queue)
        {
            try
            {
                string Status = string.Empty;

                if (Queue.Contains("1"))
                {
                    Status = "Approved";
                }
                if (Queue.Contains("2"))
                {
                    Status = "Rejected";
                }
                if (Queue.Contains("0"))
                {
                    Status = "Pending";
                }
                return Status;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                conn.Close();
            }
        }

        public string GetQueueRole(string type, string flag, string queue)
        {
            try
            {
                string queuetype = string.Empty;
                if (flag == "5")
                {
                    if (type.Contains("I"))
                    {
                        queuetype = "PR Supervisor";
                    }
                }
                if (type.Contains("G") || flag == "10")
                {
                    getconnection();
                    DataTable dt = new DataTable();
                    cmd = new SqlCommand("pr_fb_trn_audittaril", conn);
                    cmd.Parameters.AddWithValue("@gid", queue);
                    cmd.Parameters.AddWithValue("@refflag", "G");
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        queuetype = Convert.ToString(dt.Rows[0]["rolegroup_name"].ToString());
                    }
                }
                if (type.Contains("R"))
                {
                    getconnection();
                    DataTable dt = new DataTable();
                    cmd = new SqlCommand("pr_fb_trn_audittaril", conn);
                    cmd.Parameters.AddWithValue("@gid", queue);
                    cmd.Parameters.AddWithValue("@refflag", "R");
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        queuetype = Convert.ToString(dt.Rows[0]["role_name"].ToString());
                    }
                }
                if (type.Contains("L"))
                {
                    getconnection();
                    DataTable dt = new DataTable();
                    cmd = new SqlCommand("pr_fb_trn_audittaril", conn);
                    cmd.Parameters.AddWithValue("@gid", queue);
                    cmd.Parameters.AddWithValue("@refflag", "L");
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        queuetype = Convert.ToString(dt.Rows[0]["grade_name"].ToString());
                    }
                }
                if (type.Contains("D"))
                {
                    getconnection();
                    DataTable dt = new DataTable();
                    cmd = new SqlCommand("pr_fb_trn_audittaril", conn);
                    cmd.Parameters.AddWithValue("@gid", queue);
                    cmd.Parameters.AddWithValue("@refflag", "D");
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        queuetype = Convert.ToString(dt.Rows[0]["designation_name"].ToString());
                    }
                }
                if (type.Contains("I"))
                {
                    getconnection();
                    DataTable dt = new DataTable();
                    cmd = new SqlCommand("pr_fb_trn_audittaril", conn);
                    cmd.Parameters.AddWithValue("@gid", queue);
                    cmd.Parameters.AddWithValue("@refflag", "I");
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        queuetype = Convert.ToString(dt.Rows[0]["designation_name"].ToString());
                    }
                }
                return queuetype;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                conn.Close();
            }
        }


        #region "DelmateFlow"

        public string GetDelmatemployee(PrHeader objpr)
        {
            string resultone = string.Empty;
            string queue_toG = string.Empty;
            string queue_toD = string.Empty;
            int queue_toR = 0;
            string queue_totype = string.Empty;
            string queue_tobranch = string.Empty;
            string queue_todept = string.Empty;
            string delmattype = string.Empty;
            decimal delamount;
            string delmatgid = string.Empty;
            string Expenses = string.Empty;
            int branchgid = 0;
            string queuengid = string.Empty;
            string queuento = string.Empty;
            int prdelmat_result = 0;
            decimal pramount;

            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_getqueuegidemp", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prgid", objpr.prGid);
                da = new SqlDataAdapter(cmd);
                DataTable dtcmd = new DataTable();
                da.Fill(dtcmd);
                if (dtcmd.Rows.Count > 0)
                {
                    queuengid = dtcmd.Rows[0]["queuegid"].ToString();
                    //queuento = dtcmd.Rows[0]["queueto"].ToString();.
                    queuento = objCmnFunctions.GetLoginUserGid().ToString();



                    cmd = new SqlCommand("pr_prdelmat", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@prheader_gid", objpr.prGid);
                    cmd.Parameters.AddWithValue("@queue_gid", queuengid);
                    cmd.Parameters.AddWithValue("@pr_approver_gid", queuento);


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

                    string finalfalg = Convert.ToString(cmd.Parameters["@pr_final_flag"].Value);
                    string nqueuety = Convert.ToString(cmd.Parameters["@pr_next_queue_to_type"].Value);
                    string nqueuet = Convert.ToString(cmd.Parameters["@pr_next_queue_to_gid"].Value);
                    string ndelgid = Convert.ToString(cmd.Parameters["@pr_next_delmatmatrix_gid"].Value);
                    string pr_erroutput = Convert.ToString(cmd.Parameters["@pr_err_output"].Value);
                    string pr_sql_output = Convert.ToString(cmd.Parameters["@pr_sql_output"].Value);
                    string additional_flag = Convert.ToString(cmd.Parameters["@pr_next_queue_to_additional_flag"].Value);
                    prdelmat_result = Convert.ToInt32(cmd.Parameters["@prdelmat_result"].Value);

                    if (prdelmat_result > 0)
                    {
                        if (finalfalg == "N")
                        {
                            string[,] codesq = new string[,]
                                    {
                                         {"queue_action_date","sysdatetime()"},
                                         {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString() },
                                         {"queue_action_remark",objpr.prRemarks.ToString()},
                                         {"queue_action_status","1"},
                                         {"queue_isremoved","Y" }
                                     };
                            string[,] whreq = new string[,]
                                    {
                                     
                                        {"queue_ref_gid",objpr.prGid.ToString()},
                                        {"queue_ref_flag","5"},
                                        {"queue_isremoved","N" }
                                   };
                            string tnameq = "iem_trn_tqueue";
                            string insertcommendq = objuid.UpdateCommon(codesq, whreq, tnameq);

                            string[,] codesIN = new string[,]
                                      {
                                           {"queue_date","sysdatetime()"},
                                           {"queue_ref_flag", "5"},
                                           {"queue_ref_gid",objpr.prGid.ToString()},
                                           {"queue_ref_status", "0"},
                                           {"queue_from",objCmnFunctions.GetLoginUserGid().ToString() } ,
                                           {"queue_to_type", nqueuety.ToString()},
                                           {"queue_to", nqueuet.ToString()},
                                           {"queue_action_for", "A"},
                                           {"queue_prev_gid",Convert.ToString(getprrpvgid(objpr.prGid))},
                                           {"queue_delmat_flag","D"},
                                           {"Additional_flag",additional_flag.ToString()}

                                     };
                            string tnameIN = "iem_trn_tqueue";

                            string insertcommendecf = objuid.InsertCommon(codesIN, tnameIN);
                            resultone = insertcommendecf;
                        }

                        else
                        {
                            string[,] codes1 = new string[,]            
                       {
                           {"prheader_status","5"},
                           {"prheader_curapprstage","0"},

                       };
                            string[,] whrcol = new string[,]
                        {
                          
                             {"prheader_gid",objpr.prGid.ToString()}

                        };
                            string tname1 = "fb_trn_tprheader";
                            string updatequery = objuid.UpdateCommon(codes1, whrcol, tname1);
                            string[,] codesq = new string[,]
                                    {
                                         {"queue_action_date","sysdatetime()"},
                                         {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString() },
                                         {"queue_action_status","1" },
                                         {"queue_action_remark",objpr.prRemarks.ToString()},
                                         {"queue_isremoved","Y" },
                                         {"queue_delmat_flag","F"}
                                     };
                            string[,] whreq = new string[,]
                                    {
                                     
                                        {"queue_ref_gid",objpr.prGid.ToString()},
                                        {"queue_ref_flag","5"},
                                         {"queue_isremoved","N" }
                                   };
                            string tnameq = "iem_trn_tqueue";
                            string insertcommendq = objuid.UpdateCommon(codesq, whreq, tnameq);
                            resultone = insertcommendq;
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
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion
        public PrSumEntity Attachmentname(PrSumEntity filename)
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                int count = 0;
                int count1 = 0;
                if (HttpContext.Current.Session["count"] != null)
                {
                    count = (int)HttpContext.Current.Session["count"];
                }
                if (HttpContext.Current.Session["count1"] != null)
                {
                    count1 = (int)HttpContext.Current.Session["count1"];
                }
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();

                Attachment obj_getprdetails;
                obj.attachment = new List<Attachment>();
                if (filename.attachid == null || filename.attachid == "")
                {
                    if (HttpContext.Current.Session["AccessToken1"] != ""
                         && HttpContext.Current.Session["AccessToken1"] == null && HttpContext.Current.Session["viewfor"] != null)
                    {
                        HttpContext.Current.Session["AccessToken1"] = HttpContext.Current.Session["cbfAttachment"];
                    }
                    if (HttpContext.Current.Session["AccessToken1"] != ""
                        && HttpContext.Current.Session["AccessToken1"] != null)
                    {
                        dt = (DataTable)HttpContext.Current.Session["AccessToken1"];
                    }
                    else
                    {
                        dt.Columns.Add("Sno", typeof(string));
                        dt.Columns.Add("Documnet_Name", typeof(string));
                        dt.Columns.Add("Attachment_Date", typeof(string));
                        dt.Columns.Add("Document_des", typeof(string));

                    }


                    if (filename != null)
                    {
                        if ((filename.attachmentDate != "" && filename.attachment1 != "" && filename.attachmentDesc != "")
                            && (filename.attachmentDate != null && filename.attachment1 != null && filename.attachmentDesc != null))
                        {
                            HttpContext.Current.Session["count"] = count + 1;
                            var row = dt.NewRow();
                            row["Sno"] = HttpContext.Current.Session["count"];
                            row["Documnet_Name"] = filename.attachment1;
                            row["Attachment_Date"] = filename.attachmentDate.ToString();
                            row["Document_des"] = filename.attachmentDesc;
                            if (dt2.Columns.Contains("cbfheader_gid"))
                                row["cbfheader_gid"] = "1";
                            dt.Rows.Add(row);

                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            obj_getprdetails = new Attachment()
                            {
                                attachGid = dt.Rows[i]["Sno"].ToString(),
                                attachedDate = dt.Rows[i]["Attachment_Date"].ToString(),
                                fileName = dt.Rows[i]["Documnet_Name"].ToString(),
                                description = dt.Rows[i]["Document_des"].ToString(),
                                employee_gid = dt.Rows[i]["Document_des"].ToString(),

                            };
                            obj.attachment.Add(obj_getprdetails);
                        }
                    }
                    System.Web.HttpContext.Current.Session["AccessToken1"] = dt;
                }

                else
                {
                    if (filename.attachid != "IEM.Areas.FLEXIBUY.Models.CbfSumEntity" || filename.attachid != "" || filename.attachid != null)
                    {

                        filename.attachment = Getattachment(filename.attachid, "4", "3");
                    }
                    if (filename.attachment != null)
                    {
                        if (filename.attachment.Count > 0)
                        {
                            dt1.Columns.Add("Sno", typeof(string));
                            dt1.Columns.Add("Documnet_Name", typeof(string));
                            dt1.Columns.Add("Attachment_Date", typeof(string));
                            dt1.Columns.Add("Document_des", typeof(string));
                            for (int i = 0; i < filename.attachment.Count(); i++)
                            {
                                var row = dt1.NewRow();
                                row["Sno"] = i + 1;
                                row["Documnet_Name"] = filename.attachment[i].fileName;
                                row["Attachment_Date"] = filename.attachment[i].attachedDate;
                                row["Document_des"] = filename.attachment[i].description;
                                dt1.Rows.Add(row);
                            }
                        }
                    }
                    if (HttpContext.Current.Session["AccessTokenheader1"] == ""
                           || HttpContext.Current.Session["AccessTokenheader1"] == null && HttpContext.Current.Session["viewfor"] != null)
                    {
                        HttpContext.Current.Session["AccessTokenheader1"] = HttpContext.Current.Session["cbfAttachmentDetails1"];
                    }
                    if (HttpContext.Current.Session["AccessTokenheader1"] != ""
                        && HttpContext.Current.Session["AccessTokenheader1"] != null)
                    {
                        dt2 = (DataTable)HttpContext.Current.Session["AccessTokenheader1"];
                    }
                    else
                    {
                        dt2.Columns.Add("Sno", typeof(string));
                        dt2.Columns.Add("prdetails", typeof(int));
                        dt2.Columns.Add("Documnet_Name", typeof(string));
                        dt2.Columns.Add("Attachment_Date", typeof(string));
                        dt2.Columns.Add("Document_des", typeof(string));
                        dt2.Columns.Add("cbfdetails_gid", typeof(string));
                    }
                    if ((filename.attachmentDate != "" && filename.attachment1 != "" && filename.attachmentDesc != "")
                        && (filename.attachmentDate != null && filename.attachment1 != null && filename.attachmentDesc != null))
                    {
                        string number = filename.attachid;
                        string[] numberpar = number.Split('_');
                        if (numberpar.Length > 1)
                        {
                            if (HttpContext.Current.Session["sno"] != null)
                            {
                                int sno = Convert.ToInt32(HttpContext.Current.Session["sno"]) + 1;
                                filename.attachid = sno.ToString();
                                HttpContext.Current.Session["sno"] = sno;
                            }
                            else
                            {
                                int sno = Convert.ToInt32(HttpContext.Current.Session["sno"]) + 1;
                                filename.attachid = sno.ToString();
                            }
                        }
                        HttpContext.Current.Session["count1"] = count1 + 1;
                        var row = dt2.NewRow();
                        row["Sno"] = count1 + 1;
                        row["prdetails"] = filename.attachid;
                        row["Documnet_Name"] = filename.attachment1;
                        row["Attachment_Date"] = filename.attachmentDate.ToString();
                        row["Document_des"] = filename.attachmentDesc;
                        if (dt2.Columns.Contains("cbfdetails_gid"))
                            row["cbfdetails_gid"] = filename.selectedValue;
                        // row["cbfpar"] = filename.selectedValue;
                        dt2.Rows.Add(row);
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        string number = filename.attachid;
                        string[] numberpar = number.Split('_');
                        if (numberpar.Length > 1)
                        {
                            filename.attachid = numberpar[1].ToString();
                        }


                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            obj_getprdetails = new Attachment()
                            {
                                attachGid = dt1.Rows[i]["Sno"].ToString(),
                                attachedDate = dt1.Rows[i]["Attachment_Date"].ToString(),
                                fileName = dt1.Rows[i]["Documnet_Name"].ToString(),
                                description = dt1.Rows[i]["Document_des"].ToString(),
                            };

                            obj.attachment.Add(obj_getprdetails);
                        }
                        dt2.Clear();
                    }
                    if (dt2.Rows.Count > 0)
                    {
                        string number = filename.attachid;
                        string[] numberpar = number.Split('_');
                        if (numberpar.Length > 1)
                        {
                            filename.attachid = numberpar[1].ToString();
                        }
                        DataView dv = new DataView();
                        dt2.DefaultView.RowFilter = "prdetails = " + filename.attachid + "";
                        dv = dt2.DefaultView;
                        for (int i = 0; i < dv.Count; i++)
                        {
                            obj_getprdetails = new Attachment()
                            {
                                attachGid = dv[i]["Sno"].ToString(),
                                attachedDate = dv[i]["Attachment_Date"].ToString(),
                                fileName = dv[i]["Documnet_Name"].ToString(),
                                description = dv[i]["Document_des"].ToString(),
                            };

                            obj.attachment.Add(obj_getprdetails);
                        }
                    }
                    System.Web.HttpContext.Current.Session["AccessTokenheader1"] = dt2;
                }

                return obj;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }
            finally
            {
                conn.Close();
            }
        }


        public int Getbranchgid()
        {
            int empbrgid = 0;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_getbranchgid", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@empid", objCmnFunctions.GetLoginUserGid().ToString());
                empbrgid = (int)cmd.ExecuteScalar();
                return empbrgid;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }

        public int Getfcccgid()
        {
            int empfcccgid = 0;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_getfcccgid", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@empid", objCmnFunctions.GetLoginUserGid().ToString());
                empfcccgid = (int)cmd.ExecuteScalar();
                return empfcccgid;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                conn.Close();
            }

        }
        public List<Attachment> Getattachment(string id, string refe, string type)
        {
            CbfSumEntity ob = new CbfSumEntity();
            DataTable dt = new DataTable();
            List<Attachment> obj_ = new List<Attachment>();
            try
            {
                getconnection();
                Attachment newob = new Attachment();
                cmd = new SqlCommand("attachment_get", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@gid", id);
                cmd.Parameters.AddWithValue("@ref", refe);
                cmd.Parameters.AddWithValue("@type", type);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        newob = new Attachment();
                        newob.attachedDate = dt.Rows[i]["attachment_date"].ToString();
                        newob.fileName = dt.Rows[i]["attachment_filename"].ToString();
                        newob.description = dt.Rows[i]["attachment_desc"].ToString();
                        newob.employee_gid = dt.Rows[i]["attachment_by"].ToString();
                        obj_.Add(newob);
                    }
                }
                ob.attachment = obj_;
                return ob.attachment;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                ob.attachment = obj_;
                return ob.attachment;
            }
            finally
            {
                conn.Close();
            }
        }

        public int getprrpvgid(string prheadergid)
        {
            int prrvgid = 0;
            try
            {
                cmd = new SqlCommand("pr_fb_getprrrgid", conn);
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
                conn.Close();
            }

        }
        public int IsExistingApprover(int refgid, string refflag)
        {
            try
            {
                int liResult = 0;
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_ChkIsFinalApprover", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@refgid", SqlDbType.Int).Value = Convert.ToInt32(refgid);
                cmd.Parameters.Add("@refflagname", SqlDbType.VarChar).Value = Convert.ToString(refflag);
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "isexistingapprover";
                liResult = Convert.ToInt32(cmd.ExecuteScalar());
                return liResult;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 2;
            }
            finally
            {
                conn.Close();
            }
        }
        public int RejectPIPInputsOnPR(PrHeader prheader)
        {
            try
            {
                string insertcommend = "";
                CommonIUD objcommuid = new CommonIUD();
                string[,] codes1 = new string[,]            
	                {
                        {"prheader_status","6"},
                       
                    };
                string[,] whrcol = new string[,]
	                 {
                         // {"prheader_refno",prrefno.ToString()},prheader_gid
                         {"prheader_gid",prheader.prGid.ToString()}
                                                    
                     };
                string tname1 = "fb_trn_tprheader";
                insertcommend = objcommuid.UpdateCommon(codes1, whrcol, tname1);
                if (insertcommend != "Success")
                {
                    insertcommend = "0";
                }


                string[,] codw = new string[,]
            {
                {"queue_action_for","R"},
                {"queue_action_status","2"},
                {"queue_isremoved","Y"},
                {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                {"queue_action_date","sysdatetime()"},
                {"queue_action_remark",prheader.prRemarks}
            };
                string[,] codewhe = new string[,]
            {
                {"queue_ref_gid",prheader.prGid.ToString()},
                {"queue_ref_flag","5"},
                 {"queue_isremoved","N"}
            };

                string tblname = "iem_trn_tqueue";

                string updatecon = objcommuid.UpdateCommon(codw, codewhe, tblname);
                return 1;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }


        public IEnumerable<flexibuydashboard> getMyRequest1(string date, string DocNo, string Amount, string Category, string Status, string Raiser)
        {
            List<flexibuydashboard> lstReqGrid = new List<flexibuydashboard>();
            try
            {

                getconnection();

                flexibuydashboard dash;
                DataTable dt = new DataTable();
                cmd = new SqlCommand("[pr_fb_trn_tdashboardmyrequestgrid]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EMPLOYEEGID", SqlDbType.BigInt).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@DOCDATE", SqlDbType.VarChar).Value = date;
                cmd.Parameters.Add("@DOCNO", SqlDbType.VarChar).Value = DocNo;
                cmd.Parameters.Add("@DOCAMOUNT", SqlDbType.VarChar).Value = Amount;
                cmd.Parameters.Add("@DOCRAISER", SqlDbType.VarChar).Value = Raiser;

                if (Category == "-- Select --")
                {
                    Category = "";
                }
                if (Status == "-- Select --")
                {
                    Status = "";
                }
                cmd.Parameters.Add("@DOCCATEGORY", SqlDbType.VarChar).Value = Category;
                cmd.Parameters.Add("@DOCSTATUS", SqlDbType.VarChar).Value = Status;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dash = new flexibuydashboard();
                        dash.dsno = i + 1;
                        dash.category = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCCATEGORY"].ToString()) ? "" : dt.Rows[i]["DOCCATEGORY"].ToString());
                        dash.dgid = Convert.ToInt32(dt.Rows[i]["GID"].ToString());
                        dash.docNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCNO"].ToString()) ? "" : dt.Rows[i]["DOCNO"].ToString());
                        dash.ddate = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCDATE"].ToString()) ? "" : dt.Rows[i]["DOCDATE"].ToString());
                        dash.amount = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCAMOUNT"].ToString()) ? "" : dt.Rows[i]["DOCAMOUNT"].ToString());
                        dash.status = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCSTATUS"].ToString()) ? "" : dt.Rows[i]["DOCSTATUS"].ToString());
                        dash.raiser = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["RAISER"].ToString()) ? "" : dt.Rows[i]["RAISER"].ToString());
                        dash.description = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DESCR"].ToString()) ? "" : dt.Rows[i]["DESCR"].ToString());
                        lstReqGrid.Add(dash);
                    }
                }
                return lstReqGrid;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return lstReqGrid;
            }
            finally
            {
                conn.Close();
            }

        }

        public int InsertPRAttachmentEditNew(PrSumEntity objPRSumEntity, int refgid = 0, string attachmenttype = "")
        {
            PrSumEntity obj = new PrSumEntity();
            try
            {
                getconnection();
                if (refgid != 0)
                {
                    cmd = new SqlCommand("pr_fb_trn_prpip_attachment", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@refgid", SqlDbType.Int).Value = refgid;
                    cmd.Parameters.Add("@process", SqlDbType.VarChar).Value = objPRSumEntity.MyFile;
                    cmd.Parameters.Add("@documentname", SqlDbType.VarChar).Value = objPRSumEntity.attachment1;
                    cmd.Parameters.Add("@attachdate", SqlDbType.VarChar).Value = DateTime.Now.ToString("dd/MMM/yyyy");
                    cmd.Parameters.Add("@attachtype", SqlDbType.VarChar).Value = attachmenttype;
                    cmd.Parameters.Add("@attachdesc", SqlDbType.VarChar).Value = objPRSumEntity.attachmentDesc;
                    cmd.Parameters.Add("@attachby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    if (objPRSumEntity.MyFile.ToString() == "PIP")
                    {
                        cmd = new SqlCommand("pr_fb_trn_prpip_attachment", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@refno", SqlDbType.VarChar).Value = objPRSumEntity.prRefNo.ToString().Trim();
                        cmd.Parameters.Add("@process", SqlDbType.VarChar).Value = objPRSumEntity.MyFile;
                        cmd.Parameters.Add("@documentname", SqlDbType.VarChar).Value = objPRSumEntity.attachment1;
                        cmd.Parameters.Add("@attachdate", SqlDbType.VarChar).Value = DateTime.Now.ToString("dd/MMM/yyyy");
                        cmd.Parameters.Add("@attachtype", SqlDbType.VarChar).Value = "Quotation";
                        cmd.Parameters.Add("@attachdesc", SqlDbType.VarChar).Value = objPRSumEntity.attachmentDesc;
                        cmd.Parameters.Add("@attachby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        cmd = new SqlCommand("pr_fb_trn_prpip_attachment", conn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@refno", SqlDbType.VarChar).Value = objPRSumEntity.prRefNo.ToString().Trim();
                        cmd.Parameters.Add("@process", SqlDbType.VarChar).Value = objPRSumEntity.MyFile;
                        cmd.Parameters.Add("@documentname", SqlDbType.VarChar).Value = objPRSumEntity.attachment1;
                        cmd.Parameters.Add("@attachdate", SqlDbType.VarChar).Value = DateTime.Now.ToString("dd/MMM/yyyy");
                        cmd.Parameters.Add("@attachtype", SqlDbType.VarChar).Value = "boqattachment";
                        cmd.Parameters.Add("@attachdesc", SqlDbType.VarChar).Value = objPRSumEntity.attachmentDesc;
                        cmd.Parameters.Add("@attachby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                        cmd.ExecuteNonQuery();
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }
        public IEnumerable<flexibuydashboard> getMyRequestqueryExcel(string date, string DocNo, string Amount, string Category, string Status, string Raiser)
        {
            List<flexibuydashboard> lstReqGrid = new List<flexibuydashboard>();
            try
            {

                getconnection();

                flexibuydashboard dash;
                DataTable dt = new DataTable();
                cmd = new SqlCommand("[pr_fb_trn_tquery]", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EMPLOYEEGID", SqlDbType.BigInt).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@DOCDATE", SqlDbType.VarChar).Value = date;
                cmd.Parameters.Add("@DOCNO", SqlDbType.VarChar).Value = DocNo;
                cmd.Parameters.Add("@DOCAMOUNT", SqlDbType.VarChar).Value = Amount;
                cmd.Parameters.Add("@DOCRAISER", SqlDbType.VarChar).Value = Raiser;

                if (Category == "-- Select --")
                {
                    Category = "";
                }
                if (Status == "-- Select --")
                {
                    Status = "";
                }
                cmd.Parameters.Add("@DOCCATEGORY", SqlDbType.VarChar).Value = Category;
                cmd.Parameters.Add("@DOCSTATUS", SqlDbType.VarChar).Value = Status;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dash = new flexibuydashboard();
                        dash.dsno = i + 1;
                        dash.category = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCCATEGORY"].ToString()) ? "" : dt.Rows[i]["DOCCATEGORY"].ToString());
                        dash.dgid = Convert.ToInt32(dt.Rows[i]["GID"].ToString());
                        dash.docNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCNO"].ToString()) ? "" : dt.Rows[i]["DOCNO"].ToString());
                        dash.ddate = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCDATE"].ToString()) ? "" : dt.Rows[i]["DOCDATE"].ToString());
                        dash.amount = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCAMOUNT"].ToString()) ? "" : dt.Rows[i]["DOCAMOUNT"].ToString());
                        dash.status = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DOCSTATUS"].ToString()) ? "" : dt.Rows[i]["DOCSTATUS"].ToString());
                        dash.raiser = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["RAISER"].ToString()) ? "" : dt.Rows[i]["RAISER"].ToString());
                        dash.description = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DESCR"].ToString()) ? "" : dt.Rows[i]["DESCR"].ToString());
                        lstReqGrid.Add(dash);
                    }
                }
                return lstReqGrid;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return lstReqGrid;
            }
            finally
            {
                conn.Close();
            }

        }
        public int DeletePREditAttachment(int id)
        {
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_attachmentall", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "deletepredit";
                cmd.ExecuteNonQuery();
                return 1;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }
        public PrSumEntity ViewInlineAttachmentsPR(int id = 0, string process = "")
        {
            PrSumEntity objsum = new PrSumEntity();
            try
            {
                objsum.attachment = new List<Attachment>();
                Attachment obj_getprdetails = new Attachment();
                DataTable dt2 = new DataTable();
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_attachmentall", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@process", SqlDbType.VarChar).Value = process;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "viewparattachment";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt2);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        obj_getprdetails = new Attachment();
                        {
                            obj_getprdetails.attachedDate = dt2.Rows[i]["attachment_date"].ToString();
                            obj_getprdetails.fileName = dt2.Rows[i]["attachment_filename"].ToString();
                            obj_getprdetails.description = dt2.Rows[i]["attachment_desc"].ToString();
                            objsum.attachment.Add(obj_getprdetails);
                        }
                    }
                }


                return objsum;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objsum;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<CBFPRPARDetails> GetCBFReportTreeView(string CBFDateFrom, string CBFDateTo)
        {
            List<flexibuydashboard> lstReqGrid = new List<flexibuydashboard>();
            List<CBFPRPARDetails> objCBFDetails = new List<CBFPRPARDetails>();
            try
            {

                getconnection();

                CBFPRPARDetails CBFDetails;
                PODetails PODetails;
                ECFDetails ECFDetails;

                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                string Empid = null;
                if (!string.IsNullOrEmpty(CBFDateFrom) && !string.IsNullOrEmpty(CBFDateTo))
                {
                    cmd = new SqlCommand("pr_fb_getCbfDetails", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = CBFDateFrom;
                    cmd.Parameters.Add("@Todate", SqlDbType.DateTime).Value = CBFDateTo;
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetCbfDetails";
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            CBFDetails = new CBFPRPARDetails();

                            CBFDetails.cbfGid = Convert.ToInt32(dt.Rows[i]["cbfheader_gid"]);
                            CBFDetails.cbfNo = Convert.ToString(dt.Rows[i]["cbfheader_cbfno"]);
                            CBFDetails.PRNo = Convert.ToString(dt.Rows[i]["PR/PAR"]);
                            CBFDetails.PRPARAmount = Convert.ToString(dt.Rows[i]["PR/PAR Amount"]);
                            CBFDetails.cbfAmount = Convert.ToDecimal(dt.Rows[i]["cbfheader_cbfamt"]);
                            CBFDetails.cbfDate = Convert.ToString(dt.Rows[i]["cbfheader_date"]);
                            CBFDetails.Status = Convert.ToString(dt.Rows[i]["status_name"]);
                            getconnection();
                            DataTable dtsub = new DataTable();
                            cmd = new SqlCommand("pr_fb_getCbfDetails", conn);//pr_fb_getPODetails
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@cbfheader_gid", SqlDbType.Int).Value = Convert.ToInt32(dt.Rows[i]["cbfheader_gid"]);
                            cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetPODetails";
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dtsub);
                            List<PODetails> objPODetails = new List<PODetails>();
                            // List<customerdetails> objcustomerdetails = new List<customerdetails>();
                            for (int ij = 0; ij < dtsub.Rows.Count; ij++)
                            {
                                PODetails = new PODetails();
                                if (!string.IsNullOrEmpty(Convert.ToString(dtsub.Rows[ij]["poheader_gid"])))
                                {
                                    PODetails.poheaderGid = Convert.ToInt32(dtsub.Rows[ij]["poheader_gid"]);
                                    PODetails.Pono = Convert.ToString(dtsub.Rows[ij]["poheader_pono"]);
                                    PODetails.POAmount = Convert.ToString(dtsub.Rows[ij]["poheader_over_total"]);
                                    PODetails.POBranch = Convert.ToString(dtsub.Rows[ij]["branch_name"]);
                                    PODetails.PoStatus = Convert.ToString(dtsub.Rows[ij]["status_name"]);

                                    PODetails.GRNNumber = Convert.ToString(dtsub.Rows[ij]["grninwrdheader_refno"]);
                                    PODetails.ProductCode = Convert.ToString(dtsub.Rows[ij]["prodservice_code"]);
                                    PODetails.ProductName = Convert.ToString(dtsub.Rows[ij]["prodservice_name"]);
                                    PODetails.ProductDesc = Convert.ToString(dtsub.Rows[ij]["prodservice_description"]);
                                    PODetails.ExpCode = Convert.ToString(dtsub.Rows[ij]["expcat_code"]);

                                    PODetails.ExpSubCode = Convert.ToString(dtsub.Rows[ij]["expsubcat_code"]);
                                    PODetails.ProjectManager = Convert.ToString(dtsub.Rows[ij]["Project Manager"]);
                                    PODetails.UOM = Convert.ToString(dtsub.Rows[ij]["uom_code"]);
                                    PODetails.Qty = Convert.ToDecimal(dtsub.Rows[ij]["podetails_qty"]);
                                    PODetails.UnitAmount = Convert.ToDecimal(dtsub.Rows[ij]["podetails_unitprice"]);
                                    PODetails.CapexBudgetLineNo = Convert.ToString(dtsub.Rows[ij]["cbfdetails_budgetline"]);
                                    PODetails.CC = Convert.ToString(dtsub.Rows[ij]["cbfdetails_fccc"]);

                                }

                                //objOrderdetails.Add(Orderdt);
                                List<ECFDetails> objECFDetails = new List<ECFDetails>();
                                if (!string.IsNullOrEmpty(Convert.ToString(dtsub.Rows[ij]["poheader_gid"])))
                                {
                                    getconnection();
                                    DataTable dtsub1 = new DataTable();
                                    cmd = new SqlCommand("pr_fb_getCbfDetails", conn);//pr_fb_GetECFDetails
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@Poheader_gid", SqlDbType.Int).Value = Convert.ToInt32(dtsub.Rows[ij]["poheader_gid"]);
                                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetECFDetails";
                                    da = new SqlDataAdapter(cmd);
                                    da.Fill(dtsub1);


                                    if (dtsub1.Rows.Count > 0)
                                    {
                                        for (int k = 0; k < dtsub1.Rows.Count; k++)
                                        {
                                            ECFDetails = new ECFDetails();
                                            if (!string.IsNullOrEmpty(Convert.ToString(dtsub1.Rows[k]["ecf_gid"])))
                                            {
                                                ECFDetails.EcfGid = Convert.ToInt32(dtsub1.Rows[k]["ecf_gid"]);
                                                ECFDetails.ECFNo = Convert.ToString(dtsub1.Rows[k]["ecf_no"]);
                                                ECFDetails.EcfAmount = Convert.ToDecimal(dtsub1.Rows[k]["ecf_amount"]);
                                                ECFDetails.ECFDate = Convert.ToString(dtsub1.Rows[k]["ecf_date"]);
                                                ECFDetails.EcfDesc = Convert.ToString(dtsub1.Rows[k]["ecf_description"]);
                                                ECFDetails.InvoiceNo = Convert.ToString(dtsub1.Rows[k]["invoice_no"]);
                                                ECFDetails.InvoiceAmount = Convert.ToDecimal(dtsub1.Rows[k]["invoice_amount"]);
                                                ECFDetails.InvDesc = Convert.ToString(dtsub1.Rows[k]["invoice_desc"]);
                                                ECFDetails.InvoiceServiceMonth = Convert.ToString(dtsub1.Rows[k]["invoice_service_month"]);
                                                ECFDetails.RaiserID = Convert.ToString(dtsub1.Rows[k]["Raiser"]);
                                                ECFDetails.RaiserDepartment = Convert.ToString(dtsub1.Rows[k]["RaiserDept"]);
                                                ECFDetails.EcfStatus = Convert.ToString(dtsub1.Rows[k]["ecfstatus_name"]);
                                            }


                                            //  objcustomerdetails.Add(customerdt);


                                            //getconnection();
                                            //DataTable dtsub2 = new DataTable();
                                            //cmd = new SqlCommand("TestTreelist_order", conn);
                                            //cmd.CommandType = CommandType.StoredProcedure;
                                            //cmd.Parameters.Add("@para3", SqlDbType.VarChar).Value = dtsub1.Rows[k]["Customerid"].ToString();
                                            //cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Invoicedetails";
                                            //da = new SqlDataAdapter(cmd);
                                            //da.Fill(dtsub2);

                                            //List<invoicedetails> objinvoicedetails = new List<invoicedetails>();
                                            //for (int l=0;l< dtsub2 .Rows .Count ;l++)
                                            //{
                                            //    invoicedt = new invoicedetails();
                                            //    invoicedt.InvCustid = Convert.ToString(dtsub2.Rows[l]["customerid"].ToString());
                                            //    invoicedt.ItemNo = Convert.ToString(dtsub2.Rows[l]["ItemNo"].ToString());
                                            //    invoicedt.ItemName = Convert.ToString(dtsub2.Rows[l]["ItemName"].ToString());
                                            //    invoicedt.Qty = Convert.ToString(dtsub2.Rows[l]["Qty"].ToString());
                                            //    invoicedt.Price = Convert.ToString(dtsub2.Rows[l]["Price"].ToString());
                                            //    invoicedt.Invoiceid = Convert.ToString(dtsub2.Rows[l]["Invoiceid"].ToString());

                                            //    objinvoicedetails.Add(invoicedt);

                                            //}

                                            //customerdt.Invoicedt = objinvoicedetails;
                                            if (ECFDetails.EcfGid > 0)
                                            {
                                                objECFDetails.Add(ECFDetails);
                                            }

                                        }
                                    }
                                }
                                PODetails.ECFDetails = objECFDetails;
                                if (PODetails.poheaderGid > 0)
                                {
                                    objPODetails.Add(PODetails);
                                }
                            }

                            //objCBFDetails. = empdt;
                            //dash.orderDetails = objOrderdetails;
                            CBFDetails.PoDetails = objPODetails;
                            objCBFDetails.Add(CBFDetails);

                        }
                    }
                }

                return objCBFDetails;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objCBFDetails;
            }
            finally
            {
                conn.Close();
            }
        }

        public DataSet GetCBFUtilReports(string CBFDateFrom, string CBFDateTo, string CBFNO, string PONO)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_getCBFUtilReportNew", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = CBFDateFrom;
                cmd.Parameters.Add("@Todate", SqlDbType.DateTime).Value = CBFDateTo;
                if (string.IsNullOrEmpty(CBFNO))
                {
                    CBFNO = null;
                }
                if (string.IsNullOrEmpty(PONO))
                {
                    PONO = null;
                }
                cmd.Parameters.Add("@CBFNO", SqlDbType.VarChar).Value = CBFNO;
                cmd.Parameters.Add("@PONO", SqlDbType.VarChar).Value = PONO;

                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                ds.Tables.Add(dt);
                return ds;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return ds;
            }
            finally
            {
                conn.Close();
            }
        }

        public DataSet GetCBFUtilReports(string CBFDateFrom, string CBFDateTo)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_getCBFUtilReport", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@FromDate", SqlDbType.DateTime).Value = CBFDateFrom;
                cmd.Parameters.Add("@Todate", SqlDbType.DateTime).Value = CBFDateTo;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                ds.Tables.Add(dt);
                return ds;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return ds;
            }
            finally
            {
                conn.Close();
            }
        }

        public DataSet Getapprovaldetails(string doctype)
        {
            List<flexibuydashboard> lstReqGrid = new List<flexibuydashboard>();
            DataSet ds = new DataSet();
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
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_NatureofExpenses", conn);
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

                cmd = new SqlCommand("pr_fb_inprocess", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@emp_gid", SqlDbType.BigInt).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@output_type", SqlDbType.Char).Value = "D";
                cmd.Parameters.Add("@queue_ref_name", SqlDbType.Char).Value = doctype;

                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dtdel.Rows.Count > 0)
                {
                    if (IsAdmDept == "Y")
                    {
                        for (int tr = 0; tr < 1; tr++)
                        {
                            delegatesuser = Convert.ToInt32(dtdel.Rows[tr]["delegate_by"].ToString().Trim());
                            delegatedeptid = 1; //to Set All Department 



                            cmd = new SqlCommand("pr_fb_inprocess", conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@emp_gid", SqlDbType.BigInt).Value = delegatesuser;
                            cmd.Parameters.Add("@delegate_by", SqlDbType.BigInt).Value = objCmnFunctions.GetLoginUserGid();
                            cmd.Parameters.Add("@delegatedeptId", SqlDbType.BigInt).Value = delegatedeptid;
                            cmd.Parameters.Add("@output_type", SqlDbType.Char).Value = "T";
                            cmd.Parameters.Add("@queue_ref_name", SqlDbType.Char).Value = doctype;

                            da = new SqlDataAdapter(cmd);
                            da.Fill(dt1);
                        }
                    }
                    else
                    {

                        for (int tr = 0; tr < dtdel.Rows.Count; tr++)
                        {
                            delegatesuser = Convert.ToInt32(dtdel.Rows[tr]["delegate_by"].ToString().Trim());
                            delegatedeptid = Convert.ToInt32(dtdel.Rows[tr]["delegate_deptid"].ToString().Trim());



                            cmd = new SqlCommand("pr_fb_inprocess", conn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@emp_gid", SqlDbType.BigInt).Value = delegatesuser;
                            cmd.Parameters.Add("@delegate_by", SqlDbType.BigInt).Value = objCmnFunctions.GetLoginUserGid();
                            cmd.Parameters.Add("@delegatedeptId", SqlDbType.BigInt).Value = delegatedeptid;
                            cmd.Parameters.Add("@output_type", SqlDbType.Char).Value = "T";
                            cmd.Parameters.Add("@queue_ref_name", SqlDbType.Char).Value = doctype;

                            da = new SqlDataAdapter(cmd);
                            da.Fill(dt1);

                        }
                    }
                }
                dt.Merge(dt1);
                ds.Tables.Add(dt);
                return ds;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return ds;
            }
            finally
            {
                conn.Close();
            }
        }

    }
}
