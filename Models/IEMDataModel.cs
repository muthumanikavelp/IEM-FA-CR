using IEM.App_Start;
using IEM.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IEM.Models
{
    public class IEMDataModel:IEMRepository
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
        public IEnumerable<MenuModel> GetMenu()
        {
            try
            {
                IList<MenuModel> mmList = new List<MenuModel>();
                MenuModel objModel;
                getconnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_iem_GetMenu", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["employee_gid"]);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new MenuModel();
                    objModel.Id = Convert.ToInt32(dt.Rows[i]["menu_gid"].ToString());
                    objModel.Name = dt.Rows[i]["menu_name"].ToString();
                    objModel.url = dt.Rows[i]["menu_link"].ToString();
                    objModel.ParentId = Convert.ToInt32(dt.Rows[i]["menu_parent_gid"].ToString());
                    objModel.SortOrder = Convert.ToInt32(dt.Rows[i]["menu_displayorder"].ToString());
                    mmList.Add(objModel);
                }

                return mmList;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable GetLoginUserDetails(string empcode)
        {
            try
            {
                getconnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_iem_LoginDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empcode", SqlDbType.VarChar).Value = empcode.ToUpper().Trim();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getloginuserdetails";
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


        /*Authorize Purpose - 06-04-2018*/
        public string AuthorizeEntity(string empcode)
        {
            string Result = "Succcess";
            List<AuthorizeEntity> objModel = new List<AuthorizeEntity>();
            AuthorizeEntity obj = new AuthorizeEntity();
            try
            {
                getconnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_iem_LoginDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empcode", SqlDbType.VarChar).Value = empcode.ToUpper().Trim();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getusermenudetails";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                HttpContext.Current.Session["Auth"] = dt;
                //foreach(DataRow row in dt.Rows )
                // {
                //     obj = new AuthorizeEntity();
                //     obj.menu_gid = Convert.ToInt64(row["menu_gid"].ToString());
                //     objModel.Add(obj);
                // }
                // ArrayList myArrayList = new ArrayList();
                // myArrayList.Add(objModel);
                return Result;
            }
            catch (Exception ex)
            {
                //throw ex;
                return Result = ex.Message;
            }
            finally
            {
                con.Close();
            }

        }




        public void insertloginattempt(string ipaddr, string empcode, string loginstatus, string browser = null)
        {
            try
            {
                string[,] codes2 = new string[,]
                   {
                     {"loginattempt_ip",ipaddr },
                     {"loginattempt_employee_code",empcode },
                     {"loginattempt_status",loginstatus },
                     {"loginattempt_date","sysdatetime()" },
                     {"loginattempt_browser",browser}
                   
                   };
                string insertcommand2 = objCommonIUD.InsertCommon(codes2, "iem_trn_tloginattempt");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public int CheckDuplicateLogin(string empcode)
        {
            try
            {
                int logincount = 0; 
                getconnection();
                cmd = new SqlCommand("pr_iem_LoginDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empcode", SqlDbType.VarChar).Value = empcode.ToUpper().Trim();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "checkduplicatelogin";
                logincount = Convert.ToInt32(cmd.ExecuteScalar());
                return logincount; 
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
        
        public void UpdateLoginFlag(int empgid,int updateloginflagfor) 
        {
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_iem_LoginDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = empgid;
                cmd.Parameters.Add("@updateloginflagfor", SqlDbType.Int).Value = updateloginflagfor;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updateloginflag";
                cmd.ExecuteNonQuery();
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
        public IEnumerable<GetProxyDetails> GetProxyDetails(string iogincode)
        {
            try
            {
                List<GetProxyDetails> objCountrytype = new List<GetProxyDetails>();
                GetProxyDetails objModel;
                getconnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_iem_Proxy", con);
                cmd.Parameters.AddWithValue("@action", "GetProxy");
                cmd.Parameters.AddWithValue("@gid", iogincode);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new GetProxyDetails();
                    //objModel.employeegid = Convert.ToInt32(dt.Rows[i]["employee_code"].ToString());
                    objModel.employeegid = dt.Rows[i]["employee_code"].ToString();
                    objModel.empname = Convert.ToString(dt.Rows[i]["employee_name"].ToString());
                    objCountrytype.Add(objModel);
                }
                return objCountrytype;
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


        public DataTable getProxyId(string loginid)
        {
            try
            {
                getconnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_iem_Proxy", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@employeecode", SqlDbType.VarChar).Value = loginid.ToString();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetProxyID";
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
        
        public void InsertSessionTimedOutLog(string empcode)  
        {
            try
            {
                insertloginattempt(HttpContext.Current.Request.UserHostAddress, empcode, "Session Timed Out");
                HttpContext.Current.Response.Redirect("~/SessionTimeOut/Index");
            }
            catch (Exception ex)
            {
               
            }
            finally
            {
                con.Close();
            }
        }
        public DataSet GetTicketSummary()
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_iem_trn_raiseticket", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ticketfrom", SqlDbType.VarChar).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getticketsummary";
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

        public void InsertTicketComments(RaiseTicketEntity RaiseTic) 
        {
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_iem_trn_raiseticket", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ticketnumber", SqlDbType.VarChar).Value = RaiseTic.TicketNumber;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getticketraiser";
                RaiseTic.TicketToID=Convert.ToInt32(cmd.ExecuteScalar());
               
                string[,] codes2 = new string[,]
                   {
                     {"rt_number",RaiseTic.TicketNumber },
                     {"rt_content",string.IsNullOrEmpty(RaiseTic.TicketContent)?"":RaiseTic.TicketContent.Replace("'","''") },
                     {"rt_from",Convert.ToString(HttpContext.Current.Session["employee_gid"]) },
                     {"rt_to",Convert.ToString(RaiseTic.TicketToID) },
                     {"rt_deleteflag_from","N" },
                     {"rt_deleteflag_to","N" },
                     {"rt_date","sysdatetime()" },
                     {"rt_replyflag","Y" },
                     {"rt_parent_ticketno",RaiseTic.TicketNumber }
                   };
                string insertcommand2 = objCommonIUD.InsertCommon(codes2, "iem_trn_traiseticket");
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }
        public void InsertTicketCommentsNew(RaiseTicketEntity RaiseTic) 
        {
            try
            {
                getconnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_iem_trn_raiseticket", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@itemrefnum", SqlDbType.VarChar).Value = RaiseTic.ItemRefNumber;
                cmd.Parameters.Add("@refname", SqlDbType.VarChar).Value = RaiseTic.RefFlag;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getitemdetails";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    string[,] codes2 = new string[,]
                   {
                     {"rt_number", objCmnFunctions.GetTicketNo("TIC")},
                     {"rt_content",string.IsNullOrEmpty(RaiseTic.TicketContent)?"":RaiseTic.TicketContent.Replace("'","''") },
                     {"rt_from",Convert.ToString(HttpContext.Current.Session["employee_gid"]) },
                     {"rt_to",Convert.ToString(string.IsNullOrEmpty(dt.Rows[0]["TicketToID"].ToString())?"":dt.Rows[0]["TicketToID"].ToString()) },
                     {"rt_deleteflag_from","N" },
                     {"rt_deleteflag_to","N" },
                     {"rt_date","sysdatetime()" },
                     {"rt_replyflag","N" },
                     {"rt_parent_ticketno","" },
                     {"rt_ref_flag",Convert.ToString(string.IsNullOrEmpty(dt.Rows[0]["RefFlag"].ToString())?"":dt.Rows[0]["RefFlag"].ToString()) },
                     {"rt_item_gid",Convert.ToString(string.IsNullOrEmpty(dt.Rows[0]["ItemGid"].ToString())?"":dt.Rows[0]["ItemGid"].ToString()) },
                     {"rt_itemrefno",Convert.ToString(string.IsNullOrEmpty(RaiseTic.ItemRefNumber)?"": RaiseTic.ItemRefNumber) }
                   };
                    string insertcommand2 = objCommonIUD.InsertCommon(codes2, "iem_trn_traiseticket");
                }
                
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }
        public DataSet GetTicketSummarySingle(string refflag = null, string itemrefno = null) 
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_iem_trn_raiseticket", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@itemrefnum", SqlDbType.VarChar).Value = itemrefno;
                cmd.Parameters.Add("@refname", SqlDbType.VarChar).Value = refflag;
                cmd.Parameters.Add("@ticketfrom", SqlDbType.VarChar).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getticketsummarysingle";
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
        public IEnumerable<PolicyDataModel> GetPolicyFiles()
        {
            try
            {
                List<PolicyDataModel> objPolicy = new List<PolicyDataModel>();
                PolicyDataModel objModel;
                getconnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_report", con);
                cmd.Parameters.AddWithValue("@OPERATION", "GETPOLICY");
                
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new PolicyDataModel();
                    objModel.PolicyGid = Convert.ToInt32(dt.Rows[i]["policy_gid"].ToString());
                    objModel.PolicyName = Convert.ToString(dt.Rows[i]["policy_name"].ToString());
                    objPolicy.Add(objModel);
                }
                return objPolicy;
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
        public IEnumerable<PolicyDataModel> GetPolicyFilesView(int id)
        {
            try
            {
                List<PolicyDataModel> objPolicy = new List<PolicyDataModel>();
                PolicyDataModel objModel;
                getconnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_report", con);
                cmd.Parameters.AddWithValue("@OPERATION", "GETPOLICYVIEW");
                cmd.Parameters.Add("@POLICYID", SqlDbType.VarChar).Value = id;
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new PolicyDataModel();
                    objModel.PolicyGid = Convert.ToInt32(dt.Rows[i]["policy_gid"].ToString());
                    objModel.PolicyName = Convert.ToString(dt.Rows[i]["policy_name"].ToString());
                    objModel.DownloadPath = Convert.ToString(dt.Rows[i]["DownloadPath"].ToString());
                    objModel.FileName = Convert.ToString(dt.Rows[i]["policy_filename"].ToString());
                    objPolicy.Add(objModel);
                }
                return objPolicy;
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
        public string GetPreviousIPAddress(string empcode)
        {
            try
            {
                string result = "";
                getconnection();
                cmd = new SqlCommand("pr_iem_LoginDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empcode", SqlDbType.VarChar).Value = empcode.ToUpper().Trim();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getipaddress";
                result = string.IsNullOrEmpty(Convert.ToString(cmd.ExecuteScalar())) ? "" : Convert.ToString(cmd.ExecuteScalar());
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
                da.Dispose();
            }
        }
        public void ReleaseEmp(string empcode)
        {
            try
            {
                string result = "";
                getconnection();
                cmd = new SqlCommand("pr_iem_LoginDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empcode", SqlDbType.VarChar).Value = empcode.ToUpper().Trim();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "releaseemployee";
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        public string GetPreviousBrowser(string empcode) 
        {
            try
            {
                string result = "";
                getconnection();
                cmd = new SqlCommand("pr_iem_LoginDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empcode", SqlDbType.VarChar).Value = empcode.ToUpper().Trim();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getbrowsername";
                result = string.IsNullOrEmpty(Convert.ToString(cmd.ExecuteScalar())) ? "" : Convert.ToString(cmd.ExecuteScalar());
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
                da.Dispose();
            }
        }
        public void updateloginattempt(string empcode = null)
        {
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_iem_LoginDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empcode", SqlDbType.VarChar).Value = empcode.ToUpper().Trim();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updateloginattempt";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }
        public int GetSessionInterval(string empcode = null)
        {
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_iem_LoginDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empcode", SqlDbType.VarChar).Value = empcode.ToUpper().Trim();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getsessioninterval";
                int result = Convert.ToInt32(string.IsNullOrEmpty(Convert.ToString(cmd.ExecuteScalar())) ? "0" : Convert.ToString(cmd.ExecuteScalar()));
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
    }
}