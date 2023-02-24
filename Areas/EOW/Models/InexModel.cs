using IEM.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace IEM.Areas.EOW.Models
{
    public class InexModel:InexRepository
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        Common.CmnFunctions cmnfun = new Common.CmnFunctions();
        string month;
        string result;
        System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
        public InexModel()
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

            }
            finally
            {
                con.Close();
            }
            return dt;
        }
        public IEnumerable<InexDataModel> SelectDocType()
        {
            try
            {
                List<InexDataModel> emp = new List<InexDataModel>();
                InexDataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTDOCTYPE";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new InexDataModel();
                    objModel.Docgid = Convert.ToInt32(row["doctype_gid"].ToString());
                    objModel.Docname = row["doctype_code"].ToString();
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
        public IEnumerable<InexDataModel> SelectDocSubType()
        {
            try
            {
                List<InexDataModel> emp = new List<InexDataModel>();
                InexDataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTSUBDOCTYPE";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new InexDataModel();
                    objModel.DocSubgid = Convert.ToInt32(row["docsubtype_gid"].ToString());
                    objModel.DocSubname = row["docsubtype_code"].ToString();
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

        public IEnumerable<InexDataModel> SelectStatus()
        {
            try
            {
                List<InexDataModel> emp = new List<InexDataModel>();
                InexDataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTSTATUS";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new InexDataModel();
                    objModel.StatusGid = Convert.ToInt32(row["ecfstatus_gid"].ToString());
                    objModel.statusname = row["ecfstatus_name"].ToString();
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

        public IEnumerable<InexDataModel> Search()
        {
            try
            {
                
                List<InexDataModel> emp = new List<InexDataModel>();
                InexDataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_innexsubmission", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "DISPLAYGRID";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new InexDataModel();
                    objModel.ECFId = Convert.ToInt16(row["ecf_gid"].ToString());
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
                    objModel.ECFAmount = row["ecf_amount"].ToString();
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
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        public IEnumerable<InexDataModel> Search(string EcfDateFrom = null, string EcfDateTo = null, string DocType = null, string docsubtype = null, string Code = null, string Name = null, string ECFClaimMonth = null, string queryempsup = null, string ECFDespatchDateTo = null, string Suppliername = null, string ecfnumber = null, string ecfamount = null, string Ecfstatus = null, string ECFMode = null, string command = null)
        {
            try
            {
                //if (ECFClaimMonth != null && ECFClaimMonth != "")
                //{
                //    InexDataModel bat = new InexDataModel();
                //    string[] arr = Regex.Split(ECFClaimMonth, "-");
                //    month = arr[1];
                //    string year = arr[2];
                //}
                List<InexDataModel> emp = new List<InexDataModel>();
                InexDataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_innexsubmission", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (EcfDateFrom != "" && EcfDateFrom!=null)
                {
                    cmd.Parameters.Add("@ecf_dateFrom", SqlDbType.VarChar).Value = cmnfun.convertoDateTimeString(EcfDateFrom.ToString());
                }
                if (EcfDateTo != "" && EcfDateTo!=null)
                {
                    cmd.Parameters.Add("@ecf_dateTo", SqlDbType.VarChar).Value = cmnfun.convertoDateTimeString(EcfDateTo.ToString());
                }
                if (DocType != "" || DocType == null)
                {
                    cmd.Parameters.Add("@doctype_name", SqlDbType.VarChar).Value = DocType;
                }
                else
                {
                    cmd.Parameters.Add("@doctype_name", SqlDbType.Int).Value = 0;
                }
                if (docsubtype != "" || docsubtype == null)
                {
                    cmd.Parameters.Add("@docsubtype_name", SqlDbType.Int).Value = Convert.ToInt16(docsubtype);
                }
                else
                {
                    cmd.Parameters.Add("@docsubtype_name", SqlDbType.Int).Value = 0;
                }
                if (queryempsup == "Employee")
                {
                    cmd.Parameters.Add("@ecf_supplier_employee", SqlDbType.VarChar).Value = "E";
                }
                if (queryempsup == "Supplier")
                {
                    cmd.Parameters.Add("@ecf_supplier_employee", SqlDbType.VarChar).Value = "S";
                }
                cmd.Parameters.Add("@ecf_no", SqlDbType.VarChar).Value = ecfnumber;
                cmd.Parameters.Add("@ecf_create_mode", SqlDbType.VarChar).Value = ECFMode;
                cmd.Parameters.Add("@ecf_amount", SqlDbType.VarChar).Value = ecfamount;
                cmd.Parameters.Add("@employee_code", SqlDbType.VarChar).Value = Code;
                cmd.Parameters.Add("@employee_name", SqlDbType.VarChar).Value = Name;
               
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
               
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SEARCH";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new InexDataModel();
                    objModel.ECFId = Convert.ToInt16(row["ecf_gid"].ToString());
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
                    objModel.ECFAmount = row["ecf_amount"].ToString();
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
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        public IEnumerable<InexDataModel> SelectView(int id)
        {
            try
            {
                List<InexDataModel> emp = new List<InexDataModel>();
                InexDataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_innexsubmission", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ecf_gid", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTGRID";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new InexDataModel();
                    objModel.ECFId = Convert.ToInt16(row["ecf_gid"].ToString());
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
                    objModel.ECFAmount = row["ecf_amount"].ToString();
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
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        public string UpdateInex(int id)
        {
            try
            {
                cmd = new SqlCommand("pr_iem_trn_innexsubmission", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ecf_gid", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@CANCELBY", SqlDbType.Int).Value = Convert.ToInt32(cmnfun.GetLoginUserGid());
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "UPDATEINEX";
                result = cmd.ExecuteNonQuery().ToString();
                if(result == "2")
                {
                    result = "inex succesfully";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return result;
        }
        public string RejectInex(int id)
        {
            try
            {
                cmd = new SqlCommand("pr_iem_trn_innexsubmission", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ecf_gid", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@CANCELBY", SqlDbType.Int).Value = Convert.ToInt32(cmnfun.GetLoginUserGid());
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "REJECTINEX";
                result = cmd.ExecuteNonQuery().ToString();
                if (result == "2")
                {
                    result = "Reject succesfully";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return result;
        }
        public DataTable getecfstatusname(int ecfstatusgid)
        {
            DataTable dt = new DataTable();
            try
            {
                List<InexDataModel> emp = new List<InexDataModel>();
                InexDataModel objModel;
                GetConnection();
                cmd = new SqlCommand("pr_iem_trn_ECFReport", con);
                cmd.Parameters.AddWithValue("@GID", ecfstatusgid);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTECFSTATUSNAME";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
            }
            catch(Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return dt;
        }
        public IEnumerable<InexDataModel> SelectDocSubType(int gid)
        {
            try
            {
                List<InexDataModel> objCountrytype = new List<InexDataModel>();
                InexDataModel objModel;
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
                    objModel = new InexDataModel();
                    objModel.DocSubgid = Convert.ToInt32(dt.Rows[i]["docsubtype_gid"].ToString());
                    //objModel.doctype_code = Convert.ToString(dt.Rows[i]["doctype_code"].ToString());
                    objModel.DocSubname = Convert.ToString(dt.Rows[i]["docsubtype_name"].ToString());
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

    }
}