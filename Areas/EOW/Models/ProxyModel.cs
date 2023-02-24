using IEM.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;


namespace IEM.Areas.EOW.Models
{
    public class ProxyModel : ProxyRepository
    {
        ErrorLog objErrorLog = new ErrorLog();
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        DataTable data = new DataTable();
        SupEmployeeRole emp = new SupEmployeeRole();
        CmnFunctions cmnfun = new CmnFunctions();
        CommonIUD comm = new CommonIUD();
        System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
        string result;
        public ProxyModel()
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
        
        public IEnumerable<ProxyDataModel> SelectEmployee()
        {
            List<ProxyDataModel> emp = new List<ProxyDataModel>();
            try
            {
            
                ProxyDataModel objModel;
                cmd = new SqlCommand("pr_iem_mst_tproxy", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supervisor_id", SqlDbType.Int).Value = cmnfun.GetLoginUserGid();
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "EMPLOYEESEARCH";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new ProxyDataModel();
                    objModel.EmployeeId = Convert.ToInt32(row["employee_gid"].ToString());
                    objModel.RaiserName = row["employee_name"].ToString();
                    objModel.RaiserCode = row["employee_code"].ToString();
                    objModel.EmployeeGrade = row["employee_grade_code"].ToString();
                    objModel.EmployeeMobile = row["employee_mobile_no"].ToString();
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

        public IEnumerable<ProxyDataModel> SelectEmployeeSearch(string EmployeeName, string EmployeeCode)
        {
            List<ProxyDataModel> emp = new List<ProxyDataModel>();
            try
            {
              
                ProxyDataModel objModel;
                cmd = new SqlCommand("pr_iem_mst_tproxy", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RAISERNAME", SqlDbType.VarChar).Value = EmployeeName;
                cmd.Parameters.Add("@RAISERCODE", SqlDbType.VarChar).Value = EmployeeCode;
                cmd.Parameters.Add("@supervisor_id", SqlDbType.Int).Value = cmnfun.GetLoginUserGid();
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "EMPLOYEESEARCH";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new ProxyDataModel();
                    objModel.EmployeeId = Convert.ToInt32(row["employee_gid"].ToString());
                    objModel.RaiserName = row["employee_name"].ToString();
                    objModel.RaiserCode = row["employee_code"].ToString();
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
        public string SaveProxy(ProxyDataModel proxy)
        {
            try
            {
                if(proxy.proxy_remark==null)
                {
                    proxy.proxy_remark="";
                }
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tproxy", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@proxy_to", SqlDbType.Int).Value = proxy.proxy_to;
                cmd.Parameters.Add("@proxy_period_from", SqlDbType.VarChar).Value = cmnfun.convertoDateTimeString(proxy.proxy_period_from);
                cmd.Parameters.Add("@proxy_period_to", SqlDbType.VarChar).Value = cmnfun.convertoDateTimeString(proxy.proxy_period_to);
                cmd.Parameters.Add("@proxy_remark", SqlDbType.VarChar).Value = proxy.proxy_remark;
                cmd.Parameters.Add("@proxy_active", SqlDbType.Char).Value = proxy.proxy_active;
                cmd.Parameters.AddWithValue("@proxy_by", SqlDbType.Int).Value = cmnfun.GetLoginUserGid();
                //cmd.Parameters.AddWithValue("@ACTION", SqlDbType.VarChar).Value = "ADDPROXY";
                //result = (string)cmd.ExecuteScalar();
                //if (result == "Not There")
                //{
                    CommonIUD comm = new CommonIUD();
                    string[,] codes = new string[,]            
	                {
                        {"proxy_by",Convert.ToString (cmnfun.GetLoginUserGid())},
                        {"proxy_to",proxy.proxy_to},
                        {"proxy_period_from",cmnfun.convertoDateTimeString(proxy.proxy_period_from)},
                        {"proxy_period_to",cmnfun.convertoDateTimeString(proxy.proxy_period_to)},
                        {"proxy_active",proxy.proxy_active},
                        {"proxy_insert_by",Convert.ToString (cmnfun.GetLoginUserGid())},
                        {"proxy_remark",proxy.proxy_remark},
                        {"proxy_insert_date","sysdatetime()"}
                    };
                    string tname = "iem_mst_tproxy";
                    result = comm.InsertCommon(codes, tname);
                    if(result=="success")
                    {
                        result = "Successfully Saved";
                    }
                //}
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
        public IEnumerable<ProxyDataModel> SelectProxy()
        {
            List<ProxyDataModel> emp = new List<ProxyDataModel>();
            try
            {
               
                ProxyDataModel objModel;
                cmd = new SqlCommand("pr_iem_mst_tproxy", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@proxy_by", SqlDbType.VarChar).Value = cmnfun.GetLoginUserGid();
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTPROXY";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                DataSet ds = new DataSet();
                dt.TableName = "Proxy";
                ds.Tables.Add(dt);
                HttpContext.Current.Session["ExcelExportProxy"] = ds;
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new ProxyDataModel();
                    objModel.proxy_gid = Convert.ToInt32(row["Proxy Gid"].ToString());
                    objModel.proxy_period_from = row["Proxy Period From"].ToString();
                    objModel.proxy_period_to = row["proxy_period_to"].ToString();
                    objModel.proxy_to = row["Proxy To"].ToString();
                    objModel.proxy_active = row["Proxy Active"].ToString();
                    objModel.ProxyGrade = row["Grade"].ToString();
                    objModel.EmployeeName = row["Employee Name"].ToString();
                    objModel.EmployeeCode = row["Employee Code"].ToString();
                    objModel.MobileNo = row["Mobile No"].ToString();
                    objModel.proxy_remark = row["Proxy Remark"].ToString();
                    objModel.proxy_by = row["Proxy By"].ToString();
                    objModel.RaiserName = row["Proxy By"].ToString();
                    objModel.EmployeeCode = cmnfun.GetLoginUserGid().ToString();
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
        public IEnumerable<ProxyDataModel> SelectEditProxy(int id)
        {
            List<ProxyDataModel> emp = new List<ProxyDataModel>();
            try
            {
              
                ProxyDataModel objModel;
                cmd = new SqlCommand("pr_iem_mst_tproxy", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@proxy_gid", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@proxy_by", SqlDbType.VarChar).Value = cmnfun.GetLoginUserGid();
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTEDITPROXY";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new ProxyDataModel();
                    objModel.proxy_gid = Convert.ToInt32(row["proxy_gid"].ToString());
                    objModel.proxy_period_from = row["proxy_period_from"].ToString();
                    objModel.proxy_period_to = row["proxy_period_to"].ToString();
                    objModel.proxy_to = row["proxy_to"].ToString();
                    objModel.proxy_active = row["proxy_active"].ToString();
                    objModel.ProxyGrade = row["Grade"].ToString();
                    objModel.EmployeeName = row["EmployeeName"].ToString();
                    objModel.EmployeeCode = row["EmployeeCode"].ToString();
                    objModel.MobileNo = row["MobileNo"].ToString();
                    objModel.proxy_remark = row["proxy_remark"].ToString();
                    objModel.proxy_by = row["proxy_by"].ToString();
                    objModel.RaiserName = row["Proxyby"].ToString();
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
        public string EditProxy(ProxyDataModel proxy)
        {
            try
            {
                if (proxy.proxy_remark == null)
                {
                    proxy.proxy_remark = "";
                }
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tproxy", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@proxy_to", SqlDbType.Int).Value = proxy.proxy_to;
                cmd.Parameters.Add("@proxy_period_from", SqlDbType.VarChar).Value = cmnfun.convertoDateTimeString(proxy.proxy_period_from);
                cmd.Parameters.Add("@proxy_period_to", SqlDbType.VarChar).Value = cmnfun.convertoDateTimeString(proxy.proxy_period_to);
                cmd.Parameters.Add("@proxy_remark", SqlDbType.VarChar).Value = proxy.proxy_remark;
                cmd.Parameters.Add("@proxy_active", SqlDbType.Char).Value = proxy.proxy_active;
                cmd.Parameters.AddWithValue("@proxy_by", SqlDbType.Int).Value = cmnfun.GetLoginUserGid();
                //if (result == "Not There")
                //{
                    CommonIUD comm = new CommonIUD();
                    string[,] codes = new string[,]            
	                {
                        {"proxy_by",Convert.ToString (cmnfun.GetLoginUserGid())},
                        {"proxy_to",proxy.proxy_to},
                        {"proxy_period_from",cmnfun.convertoDateTimeString(proxy.proxy_period_from)},
                        {"proxy_period_to",cmnfun.convertoDateTimeString(proxy.proxy_period_to)},
                        {"proxy_active",proxy.proxy_active},
                        {"proxy_insert_by",Convert.ToString (cmnfun.GetLoginUserGid())},
                        {"proxy_remark",proxy.proxy_remark},
                        {"proxy_insert_date","sysdatetime()"}
                    };
                    string[,] whrcol = new string[,]
	                 {
                          {"proxy_gid", proxy.proxy_gid.ToString()},
                          {"proxy_isremoved", "N"}
                     };
                    string tname = "iem_mst_tproxy";
                    result = comm.UpdateCommon(codes, whrcol, tname);
                    if (result == "Success")
                    {
                        result = "Successfully Saved";
                    }
                //}
                //else
                //{
                //    return "duplicate";
                //}
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

            return result;
        }
        public string DeleteProxy(ProxyDataModel proxy)
        {
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {
                string[,] codes = new string[,]
	        {
                 {"proxy_isremoved", "Y"}
	            
            };
                string[,] whrcol = new string[,]
	        {
                {"proxy_gid",proxy.proxy_gid.ToString ()}
            };
                string tblname = "iem_mst_tproxy";
                result = delecomm.DeleteCommon(codes, whrcol, tblname);
                if (result == "Sucess")
                {
                    result = "Deleted";
                }
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
            return result;
        }
        public IEnumerable<ProxyDataModel> SearchProxy(string DateFrom, string DateTo, string ProxyTo)
        {
            List<ProxyDataModel> emp = new List<ProxyDataModel>();
            try
            {
               
                ProxyDataModel objModel;
                cmd = new SqlCommand("pr_iem_mst_tproxy", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@employee", SqlDbType.VarChar).Value = ProxyTo;
                if (DateFrom!="")
                {
                    cmd.Parameters.Add("@proxy_period_from", SqlDbType.VarChar).Value = cmnfun.convertoDateTimeString(DateFrom);
                }
                if (DateTo!="")
                {
                    cmd.Parameters.Add("@proxy_period_to", SqlDbType.VarChar).Value = cmnfun.convertoDateTimeString(DateTo);
                }
                cmd.Parameters.Add("@proxy_by", SqlDbType.VarChar).Value = cmnfun.GetLoginUserGid();
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SEARCH";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new ProxyDataModel();
                    objModel.proxy_gid = Convert.ToInt32(row["proxy_gid"].ToString());
                    objModel.proxy_period_from = row["proxy_period_from"].ToString();
                    objModel.proxy_period_to = row["proxy_period_to"].ToString();
                    objModel.EmployeeName = row["EmployeeName"].ToString();
                    objModel.proxy_remark = row["proxy_remark"].ToString();
                    objModel.proxy_by = row["proxy_by"].ToString();
                    objModel.RaiserName = row["Proxyby"].ToString();
                    objModel.EmployeeCode =cmnfun.GetLoginUserGid().ToString();
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
    }
}