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
using System.Text.RegularExpressions;

namespace IEM.Areas.EOW.Models
{
    public class CentralModel : CentralRepository
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        DataTable data = new DataTable();
        SupEmployeeRole emp = new SupEmployeeRole();
        ErrorLog objErrorLog = new ErrorLog();
        CmnFunctions cmnfun = new CmnFunctions();
        CommonIUD comm = new CommonIUD();
        System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
        int CentralId;
        string result;
        public CentralModel()
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
        public IEnumerable<CentraldataModel> SelectSupplier()
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {
                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTSUPPLIERDETAILS";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.SupplierId = Convert.ToInt32(row["supplierheader_gid"].ToString());
                    objModel.SupplierCode = row["supplierheader_suppliercode"].ToString();
                    objModel.SupplierName = row["supplierheader_name"].ToString();
                    objModel.PanNo = row["supplierheader_panno"].ToString();
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
        public IEnumerable<CentraldataModel> SelectSupplierSearch(string SupplierName, string SupplierCode, string PanNo)
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {

                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SUPLIERNAME", SqlDbType.VarChar).Value = SupplierName;
                cmd.Parameters.Add("@SUPPLIERCODE", SqlDbType.VarChar).Value = SupplierCode;
                cmd.Parameters.Add("@PANNO", SqlDbType.VarChar).Value = PanNo;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTSUPPLIERDETAILSSEARCH";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.SupplierId = Convert.ToInt32(row["supplierheader_gid"].ToString());
                    objModel.SupplierCode = row["supplierheader_suppliercode"].ToString();
                    objModel.SupplierName = row["supplierheader_name"].ToString();
                    objModel.PanNo = row["supplierheader_panno"].ToString();
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
        public IEnumerable<CentraldataModel> SelectEmployee()
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {
                dt = new DataTable();
                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EMPGID", SqlDbType.Int).Value = cmnfun.GetLoginUserGid();
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTEMPLOYEEDETAILSSEARCH";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.EmployeeId = Convert.ToInt32(row["employee_gid"].ToString());
                    objModel.EmployeeDepartment = row["employee_dept_name"].ToString();
                    objModel.RaiserName = row["employee_name"].ToString();
                    objModel.RaiserCode = row["employee_code"].ToString();
                    objModel.empbranch = string.IsNullOrEmpty(row["employee_branch_gid"].ToString()) ? "" : row["employee_branch_gid"].ToString();
                    objModel.empfc = string.IsNullOrEmpty(row["employee_fc_code"].ToString()) ? "" : row["employee_fc_code"].ToString();
                    objModel.empcc = string.IsNullOrEmpty(row["employee_cc_code"].ToString()) ? "" : row["employee_cc_code"].ToString();
                    objModel.ecfselect = string.IsNullOrEmpty(row["employee_branch_code"].ToString()) ? "" : row["employee_branch_code"].ToString();

                    objModel.empfcname = string.IsNullOrEmpty(row["fc_name"].ToString()) ? "" : row["fc_name"].ToString();
                    objModel.empccname = string.IsNullOrEmpty(row["cc_name"].ToString()) ? "" : row["cc_name"].ToString();
                    objModel.empouname = string.IsNullOrEmpty(row["ou_name"].ToString()) ? "" : row["ou_name"].ToString();
                    objModel.empproductname = string.IsNullOrEmpty(row["product_name"].ToString()) ? "" : row["product_name"].ToString();
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
        public IEnumerable<CentraldataModel> SelectDepartment(string department)
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {
                dt = new DataTable();
                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DEPARTMENT", SqlDbType.VarChar).Value = department;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTEMPLOYEEDETAILSSEARCH";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.EmployeeId = Convert.ToInt32(row["employee_gid"].ToString());
                    objModel.EmployeeDepartment = row["employee_dept_name"].ToString();
                    objModel.RaiserName = row["employee_name"].ToString();
                    objModel.RaiserCode = row["employee_code"].ToString();
                    objModel.empbranch = row["employee_branch_gid"].ToString();
                    objModel.empfc = row["employee_fc_code"].ToString();
                    objModel.empcc = row["employee_cc_code"].ToString();
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
        public IEnumerable<CentraldataModel> SelectEmployeeDelegation()
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {
                dt = new DataTable();
                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EMPGID", SqlDbType.Int).Value = cmnfun.GetLoginUserGid();
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTEMPLOYEEDELEGATION";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.EmployeeId = Convert.ToInt32(row["employee_gid"].ToString());
                    objModel.EmployeeDepartment = row["employee_dept_name"].ToString();
                    objModel.RaiserName = row["employee_name"].ToString();
                    objModel.RaiserCode = row["employee_code"].ToString();
                    objModel.empbranch = string.IsNullOrEmpty(row["employee_branch_gid"].ToString()) ? "" : row["employee_branch_gid"].ToString();
                    objModel.empfc = string.IsNullOrEmpty(row["employee_fc_code"].ToString()) ? "" : row["employee_fc_code"].ToString();
                    objModel.empcc = string.IsNullOrEmpty(row["employee_cc_code"].ToString()) ? "" : row["employee_cc_code"].ToString();
                    objModel.ecfselect = string.IsNullOrEmpty(row["employee_branch_code"].ToString()) ? "" : row["employee_branch_code"].ToString();

                    objModel.empfcname = string.IsNullOrEmpty(row["fc_name"].ToString()) ? "" : row["fc_name"].ToString();
                    objModel.empccname = string.IsNullOrEmpty(row["cc_name"].ToString()) ? "" : row["cc_name"].ToString();
                    objModel.empouname = string.IsNullOrEmpty(row["ou_name"].ToString()) ? "" : row["ou_name"].ToString();
                    objModel.empproductname = string.IsNullOrEmpty(row["product_name"].ToString()) ? "" : row["product_name"].ToString();
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
        public IEnumerable<CentraldataModel> SelectEmployeeSearch(string EmployeeName, string EmployeeCode)
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {
                dt = new DataTable();
                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RAISERNAME", SqlDbType.VarChar).Value = EmployeeName;
                cmd.Parameters.Add("@RAISERCODE", SqlDbType.VarChar).Value = EmployeeCode;
                //cmd.Parameters.Add("@DEPARTMENT", SqlDbType.VarChar).Value = EmployeeName;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTEMPLOYEEDETAILSSEARCH";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.EmployeeId = Convert.ToInt32(row["employee_gid"].ToString());
                    objModel.EmployeeDepartment = row["employee_dept_name"].ToString();
                    objModel.RaiserName = row["employee_name"].ToString();
                    objModel.RaiserCode = row["employee_code"].ToString();
                    objModel.empbranch = row["employee_branch_gid"].ToString();
                    objModel.empfc = row["employee_fc_code"].ToString();
                    objModel.empcc = row["employee_cc_code"].ToString();
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
        public string convertoDateTimeString(string inputdate)
        {
            try
            {
                string lsInputDate = inputdate;
                string convertdate = string.Empty;
                DateTime outputDate = Convert.ToDateTime(lsInputDate.ToString());
                string format = "MM/dd/yyyy";
                convertdate = outputDate.ToString(format);
                return convertdate;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
        }
        public string AddSupplierDuplicateCheck(CentraldataModel cen)
        {
            GetConnection();

            try
            {
                cmd = new SqlCommand("pr_eow_trn_excelvalate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@chkdata", SqlDbType.VarChar).Value = cen.InvoiceNo.ToString();
                cmd.Parameters.Add("@chkdata1", SqlDbType.VarChar).Value = cen.SupplierId.ToString();
                //cmd.Parameters.Add("@centralinvoicedate", SqlDbType.VarChar).Value = cen.InvoiceDate.ToString();
                cmd.Parameters.Add("@chkdata3", SqlDbType.VarChar).Value = cen.InvoiceDate.ToString();
                cmd.Parameters.Add("@Result", SqlDbType.VarChar).Value = "DuplicateInvoice";
                result = (string)cmd.ExecuteScalar();
                if (result == "Exists")
                {
                    result = "DuplicateInvoice";
                }
                else
                {
                    cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@INVOICENO", SqlDbType.VarChar).Value = cen.InvoiceNo.ToString();
                    cmd.Parameters.Add("@SUPPLIERID", SqlDbType.VarChar).Value = cen.SupplierId.ToString();
                    cmd.Parameters.Add("@centralinvoicedate", SqlDbType.DateTime).Value = cen.InvoiceDate.ToString();
                    cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "DuplicateInvoice";
                    result = (string)cmd.ExecuteScalar();
                    if (result == "Exists")
                    {
                        result = "DuplicateInvoice";
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
        }
        public string AddSupplier(CentraldataModel cen)
        {
            try
            {
                if (cen.PONo == "-----Select-----")
                {
                    cen.PONo = "";
                }
                if (cen.PONo == null)
                {
                    cen.PONo = "";
                }

                if (cen.Remarks == null)
                {
                    cen.Remarks = "";
                }
                try
                {

                    GetConnection();
                    //cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@INVOICENO", SqlDbType.VarChar).Value = cen.InvoiceNo.ToString();
                    //cmd.Parameters.Add("@SUPPLIERID", SqlDbType.VarChar).Value = cen.SupplierId.ToString();
                    //cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "DuplicateInvoice";
                    //result = (string)cmd.ExecuteScalar();
                    //if (result == "Exists")
                    //{
                    //    result = "DuplicateInvoiceinward";
                    //    return result;
                    //}
                    //else
                    //{
                    string[,] codes = new string[,]            
	                {
                         {"centralinward_received_date",cmnfun.convertoDateTimeString(cen.ReceivedDate.ToString())},
                         {"centralinward_received_from",cen.EmployeeId.ToString()},
                         {"centralinward_supplier_gid",cen.SupplierId.ToString()},
                         {"centralinward_supplier_type",cen.SupplierType.ToString()},
                         {"centralinward_supplier_name",cen.SupplierName.ToString()},
                         {"centralinward_invoice_date",cmnfun.convertoDateTimeString(cen.InvoiceDate.ToString())},
                         {"centralinward_invoice_no",cen.InvoiceNo.ToString().ToUpper()},
                         {"centralinward_invoice_amount",cen.ECF_Amount.ToString()},
                         {"centralinward_po_no",cen.PONo.ToString()},
                         {"centralinward_remark",cen.Remarks.ToString()},

                         {"centralinward_currency",cen.Currencyrate.ToString()},
                         {"centralinward_exchangerate",cen.Exrate.ToString()},
                         {"centralinward_currencyamount",cen.Currencyamount.ToString()},
                         {"centralinward_ecfamount",cen.ECF_Amount.ToString()},
                         {"centralinward_po_type",cen.POtype.ToString()},

                         {"centralinward_status",(string)settingsReader.GetValue("CentralToBeRaised", typeof(String))},
                         {"centralinward_insert_by",Convert.ToString (cmnfun.GetLoginUserGid())},
                         {"centralinward_insert_date","sysdatetime()"}
                    };
                    string tname = "iem_trn_tcentralinward";
                    result = comm.InsertCommon(codes, tname);
                    if (result == "success")
                    {
                        result = "sub";
                    }
                }
                //}

                catch (Exception ex)
                {
                    return ex.Message.ToString();
                }
                finally
                {
                    con.Close();
                }
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
        }

        public IEnumerable<CentraldataModel> SelectCentralInward()
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {

                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SEARCHBYDATE";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.InwardId = Convert.ToInt32(row["centralinward_gid"].ToString());
                    objModel.ReceivedDate = row["centralinward_received_date"].ToString();
                    objModel.SupplierId = Convert.ToInt32(row["centralinward_supplier_gid"].ToString());
                    objModel.SupplierName = row["centralinward_supplier_name"].ToString();
                    objModel.SupplierCode = row["supplierheader_suppliercode"].ToString();
                    objModel.RaiserName = row["employee_name"].ToString();
                    objModel.InvoiceNo = row["centralinward_invoice_no"].ToString();
                    objModel.InvoiceDate = row["centralinward_invoice_date"].ToString();
                    objModel.PONo = row["centralinward_po_no"].ToString();
                    objModel.InvoiceAmount = cmnfun.GetINRAmount(row["centralinward_invoice_amount"].ToString());
                    objModel.EmployeeDepartment = row["employee_dept_name"].ToString();
                    objModel.SupplierCode = row["supplierheader_suppliercode"].ToString();
                    objModel.Remarks = row["centralinward_remark"].ToString();
                    objModel.Inwardedby = row["inwardedby"].ToString();
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
        public IEnumerable<CentraldataModel> SelectCentralHold()
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {

                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTCENTRALHOLD";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.HoldId = Convert.ToInt32(row["centralhold_gid"].ToString());
                    objModel.InwardId = Convert.ToInt32(row["centralinward_gid"].ToString());
                    objModel.ReceivedDate = row["centralinward_received_date"].ToString();
                    objModel.SupplierId = Convert.ToInt32(row["centralinward_supplier_gid"].ToString());
                    objModel.SupplierName = row["centralinward_supplier_name"].ToString();
                    objModel.RaiserName = row["employee_name"].ToString();
                    objModel.InvoiceNo = row["centralinward_invoice_no"].ToString();
                    objModel.InvoiceDate = row["centralinward_invoice_date"].ToString();
                    objModel.PONo = row["centralinward_po_no"].ToString();
                    //objModel.InvoiceAmount = row["centralinward_invoice_amount"].ToString();
                    objModel.InvoiceAmount = cmnfun.GetINRAmount(row["centralinward_invoice_amount"].ToString());
                    objModel.SupplierCode = row["supplierheader_suppliercode"].ToString();
                    objModel.EmployeeDepartment = row["employee_dept_name"].ToString();
                    objModel.Remarks = row["centralinward_remark"].ToString();
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

        public CentraldataModel SelectCentralInwardbyid(int Id)
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            CentraldataModel objModel = new CentraldataModel();
            try
            {

                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CENTRALINWARDGID", SqlDbType.VarChar).Value = Id;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTCENTRALINWARD";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    objModel = new CentraldataModel()
                    {
                        ReceivedDate = Convert.ToString(dt.Rows[0]["centralinward_received_date"].ToString()),
                        EmployeeId = Convert.ToInt32(dt.Rows[0]["centralinward_received_from"].ToString()),
                        RaiserCode = Convert.ToString(dt.Rows[0]["employee_code"].ToString()),
                        SupplierId = Convert.ToInt32(dt.Rows[0]["centralinward_supplier_gid"].ToString()),
                        SupplierCode = Convert.ToString(dt.Rows[0]["supplierheader_suppliercode"].ToString()),
                        SupplierName = Convert.ToString(dt.Rows[0]["centralinward_supplier_name"].ToString()),
                        SupplierType = Convert.ToChar(dt.Rows[0]["centralinward_supplier_type"].ToString()),
                        RaiserName = Convert.ToString(dt.Rows[0]["Raiser"].ToString()),
                        InvoiceNo = Convert.ToString(dt.Rows[0]["centralinward_invoice_no"].ToString()),
                        InvoiceDate = Convert.ToString(dt.Rows[0]["centralinward_invoice_date"].ToString()),
                        PONo = Convert.ToString(dt.Rows[0]["centralinward_po_no"].ToString()),
                        InvoiceAmount = cmnfun.GetINRAmount(Convert.ToString(dt.Rows[0]["centralinward_invoice_amount"].ToString())),
                        EmployeeDepartment = Convert.ToString(dt.Rows[0]["employee_dept_name"].ToString()),
                        Remarks = Convert.ToString(dt.Rows[0]["centralinward_remark"].ToString()),

                        CurrencyId = dt.Rows[0]["CURRENCY"].ToString(),
                        Exrate = Convert.ToString(dt.Rows[0]["centralinward_exchangerate"].ToString()),
                        Currencyamount = cmnfun.GetINRAmount(Convert.ToString(dt.Rows[0]["centralinward_currencyamount"].ToString())),
                        ECF_Amount = cmnfun.GetINRAmount(Convert.ToString(dt.Rows[0]["centralinward_ecfamount"].ToString())),
                    };

                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objModel;
            }
            finally
            {
                con.Close();
            }
            return objModel;
        }
        public CentraldataModel SelectCentralInwardEditbyid(int Id)
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            CentraldataModel objModel = new CentraldataModel();
            try
            {
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CENTRALINWARDGID", SqlDbType.VarChar).Value = Id;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTCENTRALINWARDEDIT";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    objModel = new CentraldataModel()
                    {
                        ReceivedDate = Convert.ToString(dt.Rows[0]["centralinward_received_date"].ToString()),
                        EmployeeId = Convert.ToInt32(dt.Rows[0]["centralinward_received_from"].ToString()),
                        RaiserCode = Convert.ToString(dt.Rows[0]["employee_code"].ToString()),
                        SupplierId = Convert.ToInt32(dt.Rows[0]["centralinward_supplier_gid"].ToString()),
                        SupplierCode = Convert.ToString(dt.Rows[0]["supplierheader_suppliercode"].ToString()),
                        SupplierName = Convert.ToString(dt.Rows[0]["centralinward_supplier_name"].ToString()),
                        SupplierType = Convert.ToChar(dt.Rows[0]["centralinward_supplier_type"].ToString()),
                        RaiserName = Convert.ToString(dt.Rows[0]["Raiser"].ToString()),
                        InvoiceNo = Convert.ToString(dt.Rows[0]["centralinward_invoice_no"].ToString()),
                        InvoiceDate = Convert.ToString(dt.Rows[0]["centralinward_invoice_date"].ToString()),
                        PONo = Convert.ToString(dt.Rows[0]["centralinward_po_no"].ToString()),
                        InvoiceAmount = cmnfun.GetINRAmount(Convert.ToString(dt.Rows[0]["centralinward_invoice_amount"].ToString())),
                        EmployeeDepartment = Convert.ToString(dt.Rows[0]["employee_dept_name"].ToString()),
                        Remarks = Convert.ToString(dt.Rows[0]["centralinward_remark"].ToString()),
                        CurrencyId = dt.Rows[0]["CURRENCY"].ToString(),
                        POtype = dt.Rows[0]["centralinward_po_type"].ToString(),
                        Exrate = Convert.ToString(dt.Rows[0]["centralinward_exchangerate"].ToString()),
                        Currencyamount = cmnfun.GetINRAmount(Convert.ToString(dt.Rows[0]["centralinward_currencyamount"].ToString())),
                        ECF_Amount = cmnfun.GetINRAmount(Convert.ToString(dt.Rows[0]["centralinward_ecfamount"].ToString())),
                    };

                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objModel;
            }
            finally
            {
                con.Close();
            }
            return objModel;
        }
        public CentraldataModel SelectCentralInwardbyidForMaker(int Id)
        {
            CentraldataModel objModel = new CentraldataModel();
            try
            {
                List<CentraldataModel> emp = new List<CentraldataModel>();
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@CENTRALINWARDGID", SqlDbType.VarChar).Value = Id;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTCENTRALINWARDBYIDFORMAKER";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {

                    objModel = new CentraldataModel();
                    objModel.ReceivedDate = row["centralinward_received_date"].ToString();
                    objModel.EmployeeId = Convert.ToInt32(row["centralinward_received_from"].ToString());
                    objModel.RaiserCode = row["employee_code"].ToString();
                    objModel.SupplierId = Convert.ToInt32(row["centralinward_supplier_gid"].ToString());
                    objModel.SupplierCode = row["supplierheader_suppliercode"].ToString();
                    objModel.SupplierName = row["centralinward_supplier_name"].ToString();
                    objModel.SupplierType = Convert.ToChar(row["centralinward_supplier_type"].ToString());
                    objModel.RaiserName = row["employee_name"].ToString();
                    objModel.InvoiceNo = row["centralinward_invoice_no"].ToString();
                    objModel.InvoiceDate = row["centralinward_invoice_date"].ToString();
                    objModel.PONo = row["centralinward_po_no"].ToString();
                    objModel.InvoiceAmount = cmnfun.GetINRAmount(row["centralinward_invoice_amount"].ToString());
                    objModel.EmployeeDepartment = row["employee_dept_name"].ToString();
                    objModel.Remarks = row["centralinward_remark"].ToString();

                    objModel.CurrencyId = dt.Rows[0]["CURRENCY"].ToString();
                    objModel.Exrate = row["centralinward_exchangerate"].ToString();
                    objModel.Currencyamount = cmnfun.GetINRAmount(row["centralinward_currencyamount"].ToString());
                    objModel.ECF_Amount = cmnfun.GetINRAmount(row["centralinward_ecfamount"].ToString());
                }
                return objModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objModel;
            }
            finally
            {
                con.Close();
            }
        }

        public string EditSupplier(CentraldataModel cen)
        {
            try
            {
                if (cen.PONo == "-----Select-----")
                {
                    cen.PONo = "";
                }
                if (cen.PONo == null)
                {
                    cen.PONo = "";
                }
                if (cen.Remarks == null)
                {
                    cen.Remarks = "";
                }
                if (cen.SupplierType == 'U')
                {
                    cen.SupplierId = 0;
                }
                CommonIUD comm = new CommonIUD();
                GetConnection();
                cmd = new SqlCommand("pr_eow_trn_excelvalate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@chkdata", SqlDbType.VarChar).Value = cen.InvoiceNo.ToString();
                cmd.Parameters.Add("@chkdata1", SqlDbType.VarChar).Value = cen.SupplierId.ToString();
                //cmd.Parameters.Add("@centralinvoicedate", SqlDbType.VarChar).Value = cen.InvoiceDate.ToString();
                cmd.Parameters.Add("@chkdata2", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@Result", SqlDbType.VarChar).Value = "DuplicateInvoice";
                result = (string)cmd.ExecuteScalar();

                if (result == "Exists")
                {
                    result = "DuplicateInvoice";
                    return result;
                }
                else
                {
                    GetConnection();
                    cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@INVOICENO", SqlDbType.VarChar).Value = cen.InvoiceNo.ToString();
                    cmd.Parameters.Add("@SUPPLIERID", SqlDbType.VarChar).Value = cen.SupplierId.ToString();
                    cmd.Parameters.Add("@CENTRALINWARDGID", SqlDbType.VarChar).Value = cen.InwardId.ToString();
                    cmd.Parameters.Add("@centralinvoicedate", SqlDbType.DateTime).Value = cen.InvoiceDate.ToString();
                    cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "DuplicateInvoiceedit";
                    result = (string)cmd.ExecuteScalar();
                    if (result == "Exists")
                    {
                        result = "DuplicateInvoiceinward";
                        return result;
                    }
                    else
                    {
                        string[,] codes = new string[,]            
	                {
                         {"centralinward_received_date",cmnfun.convertoDateTimeString(cen.ReceivedDate.ToString())},
                         {"centralinward_received_from",cen.EmployeeId.ToString()},
                         {"centralinward_supplier_gid",cen.SupplierId.ToString()},
                         {"centralinward_supplier_type",cen.SupplierType.ToString()},
                         {"centralinward_supplier_name",cen.SupplierName.ToString()},
                         {"centralinward_invoice_date",cmnfun.convertoDateTimeString(cen.InvoiceDate.ToString())},
                         {"centralinward_invoice_no",cen.InvoiceNo.ToString().ToUpper()},
                         {"centralinward_invoice_amount",cen.ECF_Amount.ToString()},
                         {"centralinward_po_no",cen.PONo.ToString()},
                         {"centralinward_remark",cen.Remarks.ToString()},

                         {"centralinward_currency",cen.Currencyrate.ToString()},
                         {"centralinward_exchangerate",cen.Exrate.ToString()},
                         {"centralinward_currencyamount",cen.Currencyamount.ToString()},
                         {"centralinward_ecfamount",cen.ECF_Amount.ToString()},
                         {"centralinward_po_type",cen.POtype.ToString()},

                         {"centralinward_status",(string)settingsReader.GetValue("CentralToBeRaised", typeof(String))},
                         {"centralinward_update_by",Convert.ToString (cmnfun.GetLoginUserGid())},
                         {"centralinward_update_date","sysdatetime()"}
                    };
                        string[,] whrcol = new string[,]
	                 {
                          {"centralinward_gid", cen.InwardId.ToString()},
                          {"centralinward_isremoved", "N"}
                     };
                        string tname = "iem_trn_tcentralinward";
                        result = comm.UpdateCommon(codes, whrcol, tname);
                        if (result == "Success")
                        {
                            result = "sub";
                        }
                    }
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
        public string DeleteSupplier(int id)
        {
            try
            {
                CommonIUD delecomm = new CommonIUD();
                string col_value = string.Empty;
                string col_temp = string.Empty;
                try
                {

                    string[,] codes = new string[,]
	       {
                 {"centralinward_isremoved", "Y"}
	            
           };
                    string[,] whrcol = new string[,]
	        {
                {"centralinward_gid",id.ToString ()}
            };
                    string tblname = "iem_trn_tcentralinward";
                    result = delecomm.DeleteCommon(codes, whrcol, tblname);
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
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
        }



        public IEnumerable<CentraldataModel> Search(string SupplierType, string RecivedDateFrom, string RecivedDateTo, string invoiceAmount,
            string InvoiceDate, string raisercode, string raisername, string suppliercode,
            string Suppliername, string Department, string InvoiceNo, string PONO)
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {

                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (RecivedDateFrom != "")
                {
                    cmd.Parameters.Add("@RECEIVEDDATEFROM", SqlDbType.VarChar).Value = cmnfun.convertoDateTimeString(RecivedDateFrom.ToString());
                }
                if (RecivedDateTo != "")
                {
                    cmd.Parameters.Add("@RECEIVEDDATETO", SqlDbType.VarChar).Value = cmnfun.convertoDateTimeString(RecivedDateTo.ToString());
                }
                cmd.Parameters.Add("@RAISERNAME", SqlDbType.VarChar).Value = raisername;
                cmd.Parameters.Add("@RAISERCODE", SqlDbType.VarChar).Value = raisercode;
                cmd.Parameters.Add("@INVOICENO", SqlDbType.VarChar).Value = InvoiceNo;
                if (InvoiceDate != "")
                {
                    cmd.Parameters.Add("@INVOICEDATE", SqlDbType.VarChar).Value = cmnfun.convertoDateTimeString(InvoiceDate.ToString()); ;
                }
                cmd.Parameters.Add("@INVOICEAMOUNT", SqlDbType.VarChar).Value = invoiceAmount;
                cmd.Parameters.Add("@DEPARTMENT", SqlDbType.VarChar).Value = Department;
                cmd.Parameters.Add("@PONO", SqlDbType.VarChar).Value = PONO;
                cmd.Parameters.Add("@SUPPLIERCODE", SqlDbType.VarChar).Value = suppliercode;
                cmd.Parameters.Add("@SUPLIERNAME", SqlDbType.VarChar).Value = Suppliername;
                if (SupplierType == "Approved")
                {
                    cmd.Parameters.Add("@SUPPLIERTYPE", SqlDbType.VarChar).Value = 'A';
                }
                if (SupplierType == "UnApproved")
                {
                    cmd.Parameters.Add("@SUPPLIERTYPE", SqlDbType.VarChar).Value = 'U';
                }
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SEARCH";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.InwardId = Convert.ToInt32(row["centralinward_gid"].ToString());
                    objModel.ReceivedDate = row["centralinward_received_date"].ToString();
                    objModel.SupplierId = Convert.ToInt32(row["centralinward_supplier_gid"].ToString());
                    objModel.SupplierName = row["centralinward_supplier_name"].ToString();
                    objModel.RaiserName = row["employee_name"].ToString();
                    objModel.InvoiceNo = row["centralinward_invoice_no"].ToString();
                    objModel.InvoiceDate = row["centralinward_invoice_date"].ToString();
                    objModel.PONo = row["centralinward_po_no"].ToString();
                    objModel.InvoiceAmount = cmnfun.GetINRAmount(row["centralinward_invoice_amount"].ToString());
                    objModel.SupplierCode = row["supplierheader_suppliercode"].ToString();
                    objModel.EmployeeDepartment = row["employee_dept_name"].ToString();
                    objModel.Remarks = row["centralinward_remark"].ToString();
                    objModel.Inwardedby = row["inwardedby"].ToString();
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
        public string UpdateCentralReturnback(string[] SelectedValues)
        {
            try
            {
                if (SelectedValues != null)
                {
                    foreach (string id in SelectedValues)
                    {
                        CentralId = Convert.ToInt32(id);

                        SqlCommand cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                        CommonIUD comm = new CommonIUD();
                        string[,] codes = new string[,]            
	                { 
                        {"centralinward_status",(string)settingsReader.GetValue("CentralReturn", typeof(String))}
                    };
                        string[,] whrcol = new string[,]
	                 {
                          {"centralinward_gid", CentralId.ToString()},
                     };
                        string tname = "iem_trn_tcentralinward";
                        result = comm.UpdateCommon(codes, whrcol, tname);
                        if (result == "Success")
                        {
                            result = "sub";
                        }
                    }
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

        public string AddReturn(string[] SelectedValues, string Reason)
        {
            try
            {
                if (SelectedValues != null)
                {
                    foreach (string id in SelectedValues)
                    {
                        CentralId = Convert.ToInt32(id);

                        CommonIUD comm = new CommonIUD();
                        string[,] codes = new string[,]            
	                {
                         {"centralreturn_remark",(Reason.ToString())},
                         {"centralreturn_centralinward_gid",(CentralId.ToString())},
                         {"centralreturn_return_by",Convert.ToString (cmnfun.GetLoginUserGid())},
                         {"centralreturn_return_date","sysdatetime()"}
                    };
                        string tname = "iem_trn_tcentralreturn";
                        result = comm.InsertCommon(codes, tname);
                        result = "sub";

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
        public string UpdateCentralHold(string[] SelectedValues)
        {
            try
            {
                if (SelectedValues != null)
                {
                    foreach (string id in SelectedValues)
                    {
                        CentralId = Convert.ToInt32(id);

                        SqlCommand cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                        CommonIUD comm = new CommonIUD();
                        string[,] codes = new string[,]            
	                { 
                        {"centralinward_status",(string)settingsReader.GetValue("CentralHold", typeof(String))}
                    };
                        string[,] whrcol = new string[,]
	                 {
                          {"centralinward_gid", CentralId.ToString()},
                     };
                        string tname = "iem_trn_tcentralinward";
                        result = comm.UpdateCommon(codes, whrcol, tname);
                        if (result == "Success")
                        {
                            result = "sub";
                        }
                    }
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
        public string AddHold(string[] SelectedValues, string Reason)
        {
            try
            {
                if (SelectedValues != null)
                {
                    //string values = SelectedValues.ToString();
                    //string[] arr = Regex.Split(SelectedValues.ToStrin, ",");
                    foreach (string id in SelectedValues)
                    {
                        CentralId = Convert.ToInt32(id);

                        CommonIUD comm = new CommonIUD();
                        string[,] codes = new string[,]            
	                {
                         {"centralhold_hold_remark",(Reason.ToString())},
                         {"centralhold_centralinward_gid",(CentralId.ToString())},
                         {"centralhold_hold_by",Convert.ToString (cmnfun.GetLoginUserGid())},
                         {"centralhold_hold_date","sysdatetime()"}
                    };
                        string tname = "iem_trn_tcentralhold";
                        result = comm.InsertCommon(codes, tname);
                        result = "sub";

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
        public string UpdateCentralHoldRelease(string[] SelectedValues)
        {
            try
            {
                if (SelectedValues != null)
                {
                    foreach (string id in SelectedValues)
                    {
                        CentralId = Convert.ToInt32(id);

                        SqlCommand cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                        CommonIUD comm = new CommonIUD();
                        string[,] codes = new string[,]            
	                { 
                        {"centralinward_status",(string)settingsReader.GetValue("CentralHoldRelease", typeof(String))}
                    };
                        string[,] whrcol = new string[,]
	                 {
                          {"centralinward_gid", CentralId.ToString()},                          
                     };
                        string tname = "iem_trn_tcentralinward";
                        result = comm.UpdateCommon(codes, whrcol, tname);
                        if (result == "Success")
                        {
                            result = "sub";
                        }
                    }
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
        public string UpdateHold(string[] SelectedValues, string Reason)
        {
            try
            {
                if (SelectedValues != null)
                {
                    con.Open();
                    foreach (string id in SelectedValues)
                    {
                        CentralId = Convert.ToInt32(id);
                        SqlCommand cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@REMARKS", SqlDbType.VarChar).Value = Reason;
                        cmd.Parameters.Add("@RELEASEBY", SqlDbType.VarChar).Value = cmnfun.GetLoginUserGid();
                        cmd.Parameters.Add("@CENTRALINWARDGID", SqlDbType.Int).Value = CentralId;
                        cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "UPDATEHOLDRELEASE";
                        result = cmd.ExecuteNonQuery().ToString();
                        if (result == "1")
                        {
                            result = "sub";
                        }
                        //    CommonIUD comm = new CommonIUD();
                        //    string[,] codes = new string[,]            
                        //{
                        //     {"centralhold_release_remark",Reason.ToString()},
                        //     {"centralhold_release_flag","Y"},
                        //     {"centralhold_release_by",Convert.ToString (cmnfun.GetLoginUserGid())},
                        //     {"centralhold_release_date","sysdatetime()"}
                        //};
                        //    string[,] whrcol = new string[,]
                        // {
                        //      {"centralhold_centralinward_gid", CentralId.ToString()},
                        //      
                        // };
                        //    string tname = "iem_trn_tcentralhold";
                        //    result = comm.UpdateCommon(codes, whrcol, tname);
                        //    if (result == "Success")
                        //    {
                        //        result = "sub";
                        //    }
                    }
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
        public IEnumerable<CentraldataModel> SearchHold(string SupplierType, string RecivedDateFrom, string RecivedDateTo, string invoiceAmount, string InvoiceDate, string raisercode, string raisername, string suppliercode, string Suppliername, string Department, string InvoiceNo, string PONO)
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {

                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (RecivedDateFrom != "")
                {
                    cmd.Parameters.Add("@RECEIVEDDATEFROM", SqlDbType.VarChar).Value = cmnfun.convertoDateTimeString(RecivedDateFrom.ToString());
                }
                if (RecivedDateTo != "")
                {
                    cmd.Parameters.Add("@RECEIVEDDATETO", SqlDbType.VarChar).Value = cmnfun.convertoDateTimeString(RecivedDateTo.ToString());
                }
                cmd.Parameters.Add("@RAISERNAME", SqlDbType.VarChar).Value = raisername;
                cmd.Parameters.Add("@RAISERCODE", SqlDbType.VarChar).Value = raisercode;
                cmd.Parameters.Add("@INVOICENO", SqlDbType.VarChar).Value = InvoiceNo;
                if (InvoiceDate != "")
                {
                    cmd.Parameters.Add("@INVOICEDATE", SqlDbType.VarChar).Value = cmnfun.convertoDateTimeString(InvoiceDate.ToString()); ;
                }
                cmd.Parameters.Add("@INVOICEAMOUNT", SqlDbType.VarChar).Value = invoiceAmount;
                cmd.Parameters.Add("@DEPARTMENT", SqlDbType.VarChar).Value = Department;
                cmd.Parameters.Add("@PONO", SqlDbType.VarChar).Value = PONO;
                cmd.Parameters.Add("@SUPPLIERCODE", SqlDbType.VarChar).Value = suppliercode;
                cmd.Parameters.Add("@SUPLIERNAME", SqlDbType.VarChar).Value = Suppliername;
                if (SupplierType == "Approved")
                {
                    cmd.Parameters.Add("@SUPPLIERTYPE", SqlDbType.VarChar).Value = 'A';
                }
                if (SupplierType == "UnApproved")
                {
                    cmd.Parameters.Add("@SUPPLIERTYPE", SqlDbType.VarChar).Value = 'U';
                }
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SEARCHHOLD";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.InwardId = Convert.ToInt32(row["centralinward_gid"].ToString());
                    objModel.ReceivedDate = row["centralinward_received_date"].ToString();
                    objModel.SupplierId = Convert.ToInt32(row["centralinward_supplier_gid"].ToString());
                    objModel.SupplierName = row["centralinward_supplier_name"].ToString();
                    objModel.RaiserName = row["employee_name"].ToString();
                    objModel.InvoiceNo = row["centralinward_invoice_no"].ToString();
                    objModel.InvoiceDate = row["centralinward_invoice_date"].ToString();
                    objModel.PONo = row["centralinward_po_no"].ToString();
                    objModel.InvoiceAmount = cmnfun.GetINRAmount(row["centralinward_invoice_amount"].ToString());
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

        public IEnumerable<CentraldataModel> SelectStatus()
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {

                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTSTATUS";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.StatusGid = Convert.ToInt32(row["centralstatus_status"].ToString());
                    objModel.statusname = row["centralstatus_name"].ToString();
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

        public IEnumerable<CentraldataModel> SearchReport(string SupplierType, string RecivedDateFrom, string RecivedDateTo, string invoiceAmount, string InvoiceDate, string raisercode, string raisername, string suppliercode, string Suppliername, string Department, string InvoiceNo, string PONO, string Rolelist)
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {

                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (RecivedDateFrom != "")
                {
                    cmd.Parameters.Add("@RECEIVEDDATEFROM", SqlDbType.VarChar).Value = cmnfun.convertoDateTimeString(RecivedDateFrom.ToString());
                }
                if (RecivedDateTo != "")
                {
                    cmd.Parameters.Add("@RECEIVEDDATETO", SqlDbType.VarChar).Value = cmnfun.convertoDateTimeString(RecivedDateTo.ToString());
                }
                cmd.Parameters.Add("@RAISERNAME", SqlDbType.VarChar).Value = raisername;
                cmd.Parameters.Add("@RAISERCODE", SqlDbType.VarChar).Value = raisercode;
                cmd.Parameters.Add("@INVOICENO", SqlDbType.VarChar).Value = InvoiceNo;
                if (InvoiceDate != "")
                {
                    cmd.Parameters.Add("@INVOICEDATE", SqlDbType.VarChar).Value = cmnfun.convertoDateTimeString(InvoiceDate.ToString()); ;
                }
                cmd.Parameters.Add("@INVOICEAMOUNT", SqlDbType.VarChar).Value = invoiceAmount;
                cmd.Parameters.Add("@DEPARTMENT", SqlDbType.VarChar).Value = Department;
                cmd.Parameters.Add("@PONO", SqlDbType.VarChar).Value = PONO;
                if (SupplierType == "Approved")
                {
                    cmd.Parameters.Add("@SUPPLIERTYPE", SqlDbType.VarChar).Value = 'A';
                }
                if (SupplierType == "UnApproved")
                {
                    cmd.Parameters.Add("@SUPPLIERTYPE", SqlDbType.VarChar).Value = 'U';
                }
                cmd.Parameters.Add("@SUPPLIERCODE", SqlDbType.VarChar).Value = suppliercode;
                cmd.Parameters.Add("@SUPLIERNAME", SqlDbType.VarChar).Value = Suppliername;
                if (Rolelist != "" && Rolelist != null)
                {
                    cmd.Parameters.Add("@STATUS", SqlDbType.Int).Value = Convert.ToInt32(Rolelist);
                }
                //if (Rolelist != "0" && Rolelist != "2" && Rolelist != "8")
                //{
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "CENTRALREPORT";
                //}
                //else
                //{
                //cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "CENTRALREPORT_Inward";
                //}
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.InwardId = Convert.ToInt32(row["centralinward_gid"].ToString());
                    if (row["QUEUE_GID"].ToString() != "" && row["QUEUE_GID"].ToString() != null)
                    {
                        objModel.queue_gid = Convert.ToInt32(row["QUEUE_GID"].ToString());
                    }
                    objModel.ReceivedDate = row["centralinward_received_date"].ToString();
                    objModel.SupplierId = Convert.ToInt32(row["centralinward_supplier_gid"].ToString());
                    objModel.SupplierName = row["centralinward_supplier_name"].ToString();
                    objModel.RaiserName = row["employee_name"].ToString();
                    objModel.InvoiceNo = row["centralinward_invoice_no"].ToString();
                    objModel.InvoiceDate = row["centralinward_invoice_date"].ToString();
                    objModel.PONo = row["centralinward_po_no"].ToString();
                    objModel.InvoiceAmount = cmnfun.GetINRAmount(row["centralinward_invoice_amount"].ToString());
                    objModel.EmployeeDepartment = row["employee_dept_name"].ToString();
                    objModel.EcfNo = row["ecf_no"].ToString();
                    objModel.Remarks = row["centralinward_remark"].ToString();
                    if (row["ecf_status"].ToString() == "1")
                    {
                        objModel.ecfstatus = "Approved";
                    }
                    else
                    {
                        objModel.ecfstatus = row["ECFstatus"].ToString();
                    }
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
        public void Insertapprovedactionreject(Approveraction EmployeeeExpense, string ecfgid, string invoicegid, string empgid, string ecftype, string queuegid, string queueamt)
        {
            string statuss = "";
            string delmattype = "";
            int EMPdelmattype = Convert.ToInt32(ConfigurationManager.AppSettings["EMPdelmattype"].ToString());
            int SUPdelmattype = Convert.ToInt32(ConfigurationManager.AppSettings["SUPdelmattype"].ToString());
            int EcfInprocess = Convert.ToInt32(ConfigurationManager.AppSettings["EcfInprocess"].ToString());
            int EcfApproved = Convert.ToInt32(ConfigurationManager.AppSettings["EcfApproved"].ToString());
            int ecfHold = Convert.ToInt32(ConfigurationManager.AppSettings["EcfHold"].ToString());
            int EcfConcurrentApproval = Convert.ToInt32(ConfigurationManager.AppSettings["EcfConcurrentApproval"].ToString());
            int EcfRejected = Convert.ToInt32(ConfigurationManager.AppSettings["EcfRejected"].ToString());
            try
            {
                string Emp_Msgsupermain = "";
                GetConnection();
                DataTable dtempsupfd = new DataTable();
                cmd = new SqlCommand("select ecf_raiser from iem_trn_tecf where ecf_isremoved='N' and ecf_gid='" + ecfgid + "'", con);
                cmd.CommandType = CommandType.Text;
                da = new SqlDataAdapter(cmd);
                da.Fill(dtempsupfd);
                if (dtempsupfd.Rows.Count > 0)
                {
                    Emp_Msgsupermain = Convert.ToString(dtempsupfd.Rows[0]["ecf_raiser"].ToString());
                }
                string queue_toG = "";
                string queue_toD = "";
                string queue_toR = "0";
                string queue_totype = "";
                string queue_tobranch = "";
                string queue_todept = "";
                GetConnection();
                DataSet dtempsup = new DataSet();
                string strr = "";
                strr = " SELECT employee_dept_gid,employee_iem_designation_gid,employee_grade_gid,employee_supervisor FROM iem_mst_temployee WHERE employee_gid='" + empgid + "' and employee_isremoved='N';";
                strr = strr + " select  b.ecf_docsubtype_gid,a.queue_to from  iem_trn_tqueue as a inner join iem_trn_tecf as b on a.queue_ref_gid=b.ecf_gid and b.ecf_isremoved='N' where a.queue_ref_gid='" + ecfgid + "' and queue_action_for='A'  and  a.queue_ref_flag='1' and a.queue_isremoved='N';";
                strr = strr + " SELECT b.branch_flag FROM iem_mst_temployee as a inner join iem_mst_tbranch as b on b.branch_gid=a.employee_branch_gid and b.branch_isremoved='N' WHERE employee_gid='" + Emp_Msgsupermain.ToString() + "' and employee_isremoved='N'";
                strr = strr + " SELECT employee_dept_gid,employee_iem_designation_gid,employee_grade_gid FROM iem_mst_temployee WHERE employee_gid='" + Emp_Msgsupermain + "' and employee_isremoved='N'";
                cmd = new SqlCommand(strr, con);
                cmd.CommandType = CommandType.Text;
                da = new SqlDataAdapter(cmd);
                da.Fill(dtempsup);
                if (dtempsup.Tables[0].Rows.Count > 0)
                {
                    queue_todept = Convert.ToString(dtempsup.Tables[3].Rows[0]["employee_dept_gid"].ToString());
                    queue_toG = Convert.ToString(dtempsup.Tables[0].Rows[0]["employee_grade_gid"].ToString());
                    queue_toD = Convert.ToString(dtempsup.Tables[0].Rows[0]["employee_iem_designation_gid"].ToString());
                    queue_totype = Convert.ToString(dtempsup.Tables[1].Rows[0]["ecf_docsubtype_gid"].ToString());
                    queue_tobranch = Convert.ToString(dtempsup.Tables[2].Rows[0]["branch_flag"].ToString());
                    result = Convert.ToString(dtempsup.Tables[1].Rows[0]["queue_to"].ToString());
                    if (queue_totype == "1" || queue_totype == "2" || queue_totype == "3" || queue_totype == "6" || queue_totype == "8")
                    {
                        queue_totype = "E";
                        statuss = "1";
                        delmattype = EMPdelmattype.ToString();
                    }
                    if (queue_totype == "4" || queue_totype == "5" || queue_totype == "7" || queue_totype == "9")
                    {
                        queue_totype = "S";
                        statuss = "1";
                        delmattype = SUPdelmattype.ToString();
                    }
                    if (result.ToString() != empgid.ToString())
                    {

                        string[,] codesq = new string[,]
	                             {
                                      {"queue_action_date","sysdatetime()"},
                                      {"queue_action_by",empgid.ToString() },
                                      {"queue_action_status","1"},
                                  };
                        string[,] whreq = new string[,]
	                             {
                                     {"queue_gid",queuegid.ToString() }
                                };
                        string tnameq = "iem_trn_tqueue";
                        string insertcommendq = comm.UpdateCommon(codesq, whreq, tnameq);

                        string[,] codesIN = new string[,]
	                               {
                                        {"queue_date","sysdatetime()"},
	                                    {"queue_ref_flag", statuss.ToString()},
                                        {"queue_ref_gid",ecfgid },
	                                    {"queue_ref_status", EcfInprocess.ToString()},
                                        {"queue_from",empgid },
	                                    {"queue_to_type", "S"},
                                        {"queue_to", result.ToString()},
	                                    {"queue_action_for", "A"}, 
                                        {"queue_action_status", "0"}, 
                                        {"Additional_flag", "N"}, 
                                        {"queue_prev_gid", queuegid.ToString()}
                                  };
                        string tnameIN = "iem_trn_tqueue";

                        string insertcommendecf = comm.InsertCommon(codesIN, tnameIN);

                        string[,] codes = new string[,]
                                    {
                                      {"ecf_all_status","0" }
                                    };

                        string[,] whre = new string[,]
                                   {
                                     {"ecf_gid",ecfgid }
                                  };
                        string tname = "iem_trn_tecf";
                        string insertcommend = comm.UpdateCommon(codes, whre, tname);
                    }
                    else
                    {
                        string[,] codesq = new string[,]
	                             {
                                      {"queue_action_date","sysdatetime()"},
                                      {"queue_action_by",empgid.ToString() },
                                      {"queue_action_status","0"},
                                      {"queue_ref_status",EcfApproved.ToString() },
                                  };
                        string[,] whreq = new string[,]
	                             {
                                     {"queue_gid",queuegid.ToString() }
                                };
                        string tnameq = "iem_trn_tqueue";
                        string insertcommendq = comm.UpdateCommon(codesq, whreq, tnameq);

                        string[,] codes = new string[,]
                                    {
                                      {"ecf_status","0"},
                                      {"ecf_all_status","0"}
                                    };

                        string[,] whre = new string[,]
                                   {
                                     {"ecf_gid",ecfgid }
                                  };
                        string tname = "iem_trn_tecf";
                        string insertcommend = comm.UpdateCommon(codes, whre, tname);

                    }
                }
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
        //public void Insertapprovedaction(Approveraction EmployeeeExpense, string ecfgid, string invoicegid, string empgid, string ecftype, string queuegid, string queueamt,string ckecef)
        //{
        //    string statuss = "";
        //    string delmattype = "";
        //    int EMPdelmattype = Convert.ToInt32(ConfigurationManager.AppSettings["EMPdelmattype"].ToString());
        //    int SUPdelmattype = Convert.ToInt32(ConfigurationManager.AppSettings["SUPdelmattype"].ToString());
        //    int EcfInprocess = Convert.ToInt32(ConfigurationManager.AppSettings["EcfInprocess"].ToString());
        //    int EcfApproved = Convert.ToInt32(ConfigurationManager.AppSettings["EcfApproved"].ToString());
        //    int ecfHold = Convert.ToInt32(ConfigurationManager.AppSettings["EcfHold"].ToString());
        //    int EcfConcurrentApproval = Convert.ToInt32(ConfigurationManager.AppSettings["EcfConcurrentApproval"].ToString());
        //    int EcfRejected = Convert.ToInt32(ConfigurationManager.AppSettings["EcfRejected"].ToString());
        //    try
        //    {
        //        if (EmployeeeExpense.status == "Approve")
        //        {
        //            string Emp_Msgsupermain = "";
        //            GetConnection();
        //            DataTable dtempsupfd = new DataTable();
        //            cmd = new SqlCommand("select ecf_raiser from iem_trn_tecf where ecf_isremoved='N' and ecf_gid='" + ecfgid + "'", con);
        //            cmd.CommandType = CommandType.Text;
        //            da = new SqlDataAdapter(cmd);
        //            da.Fill(dtempsupfd);
        //            if (dtempsupfd.Rows.Count > 0)
        //            {
        //                Emp_Msgsupermain = Convert.ToString(dtempsupfd.Rows[0]["ecf_raiser"].ToString());
        //            }
        //            string queue_toG = "";
        //            string queue_toD = "";
        //            string queue_toR = "0";
        //            string queue_totype = "";
        //            string queue_tobranch = "";
        //            string queue_todept = "";
        //            GetConnection();
        //            DataSet dtempsup = new DataSet();
        //            string strr = "";
        //            strr = " SELECT employee_dept_gid,employee_iem_designation_gid,employee_grade_gid,employee_supervisor FROM iem_mst_temployee WHERE employee_gid='" + empgid + "' and employee_isremoved='N';";
        //            strr = strr + " select ecf_docsubtype_gid from  iem_trn_tqueue as a inner join iem_trn_tecf as b on a.queue_ref_gid=b.ecf_gid and b.ecf_isremoved='N' where a.queue_gid='" + queuegid.ToString() + "' and  a.queue_ref_flag='1' and a.queue_isremoved='N';";
        //            strr = strr + " SELECT b.branch_flag FROM iem_mst_temployee as a inner join iem_mst_tbranch as b on b.branch_gid=a.employee_branch_gid and b.branch_isremoved='N' WHERE employee_gid='" + Emp_Msgsupermain.ToString() + "' and employee_isremoved='N'";
        //            strr = strr + " SELECT employee_dept_gid,employee_iem_designation_gid,employee_grade_gid FROM iem_mst_temployee WHERE employee_gid='" + Emp_Msgsupermain + "' and employee_isremoved='N'";
        //            cmd = new SqlCommand(strr, con);
        //            cmd.CommandType = CommandType.Text;
        //            da = new SqlDataAdapter(cmd);
        //            da.Fill(dtempsup);
        //            if (dtempsup.Tables[0].Rows.Count > 0)
        //            {
        //                queue_todept = Convert.ToString(dtempsup.Tables[3].Rows[0]["employee_dept_gid"].ToString());
        //                queue_toG = Convert.ToString(dtempsup.Tables[0].Rows[0]["employee_grade_gid"].ToString());
        //                queue_toD = Convert.ToString(dtempsup.Tables[0].Rows[0]["employee_iem_designation_gid"].ToString());
        //                queue_totype = Convert.ToString(dtempsup.Tables[1].Rows[0]["ecf_docsubtype_gid"].ToString());
        //                queue_tobranch = Convert.ToString(dtempsup.Tables[2].Rows[0]["branch_flag"].ToString());
        //                result = ckecef;                      
        //                if (queue_totype == "1" || queue_totype == "2" || queue_totype == "3" || queue_totype == "6" || queue_totype == "8")
        //                {
        //                    queue_totype = "E";
        //                    statuss = "1";
        //                    delmattype = EMPdelmattype.ToString();
        //                }
        //                if (queue_totype == "4" || queue_totype == "5" || queue_totype == "7" || queue_totype == "9")
        //                {
        //                    queue_totype = "S";
        //                    statuss = "1";
        //                    delmattype = SUPdelmattype.ToString();
        //                }                     
        //                if (result.ToString() != empgid.ToString())
        //                {

        //                    string[,] codesq = new string[,]
        //                    {
        //                        {"queue_action_date","sysdatetime()"},
        //                        {"queue_action_by",empgid.ToString() },
        //                        {"queue_action_status",EcfApproved.ToString() },
        //                    };
        //                    string[,] whreq = new string[,]
        //                    {
        //                        {"queue_gid",queuegid.ToString() }
        //                    };
        //                    string tnameq = "iem_trn_tqueue";
        //                    string insertcommendq = comm.UpdateCommon(codesq, whreq, tnameq);

        //                    string[,] codesIN = new string[,]
        //                    {
        //                        {"queue_date","sysdatetime()"},
        //                        {"queue_ref_flag", statuss.ToString()},
        //                        {"queue_ref_gid",ecfgid },
        //                        {"queue_ref_status", EcfInprocess.ToString()},
        //                        {"queue_from",empgid },
        //                        {"queue_to_type", "S"},
        //                        {"queue_to", result.ToString()},
        //                        {"queue_action_for", "A"}, 
        //                        {"queue_action_status", "1"}, 
        //                        {"Additional_flag", "N"}, 
        //                        {"queue_prev_gid", queuegid.ToString()}
        //                    };
        //                    string tnameIN = "iem_trn_tqueue";

        //                    string insertcommendecf = comm.InsertCommon(codesIN, tnameIN);

        //                    string[,] codes = new string[,]
        //                    {
        //                        {"ecf_all_status",EcfApproved.ToString() }
        //                    };
        //                    string[,] whre = new string[,]
        //                    {
        //                        {"ecf_gid",ecfgid }
        //                    };
        //                    string tname = "iem_trn_tecf";
        //                    string insertcommend = comm.UpdateCommon(codes, whre, tname);
        //                }
        //                else
        //                {
        //                    string[,] codesq = new string[,]
        //                         {
        //                              {"queue_action_date","sysdatetime()"},
        //                              {"queue_action_by",empgid.ToString() },
        //                              {"queue_action_status",EcfApproved.ToString() },
        //                              {"queue_ref_status",EcfApproved.ToString() },
        //                          };
        //                    string[,] whreq = new string[,]
        //                         {
        //                             {"queue_gid",queuegid.ToString() }
        //                        };
        //                    string tnameq = "iem_trn_tqueue";
        //                    string insertcommendq = comm.UpdateCommon(codesq, whreq, tnameq);

        //                    string[,] codes = new string[,]
        //                            {
        //                              {"ecf_status",EcfApproved.ToString() },
        //                              {"ecf_all_status",EcfApproved.ToString() }
        //                            };

        //                    string[,] whre = new string[,]
        //                           {
        //                             {"ecf_gid",ecfgid }
        //                          };
        //                    string tname = "iem_trn_tecf";
        //                    string insertcommend = comm.UpdateCommon(codes, whre, tname);
        //                }
        //            }
        //        }
        //        //------------------------------------------------------To Select Reject Status -------------
        //        else if (EmployeeeExpense.status == "Reject")
        //        {
        //            string[,] codes = new string[,]
        //           {
        //               {"ecf_all_status",EcfRejected.ToString() },
        //                {"ecf_status",EcfRejected.ToString() },
        //          };

        //            string[,] whre = new string[,]
        //           {
        //                 {"ecf_gid",ecfgid }
        //          };
        //            string tname = "iem_trn_tecf";
        //            string insertcommend = comm.UpdateCommon(codes, whre, tname);

        //            string[,] codesq = new string[,]
        //           {
        //              {"queue_action_date","sysdatetime()"},
        //              {"queue_action_by",empgid.ToString() },
        //              {"queue_action_status",EcfRejected.ToString() },
        //              {"queue_action_remark",EmployeeeExpense.Rejecthold.ToString() }
        //          };
        //            string[,] whreq = new string[,]
        //           {
        //                  {"queue_gid",queuegid.ToString() }
        //          };
        //            string tnameq = "iem_trn_tqueue";
        //            string insertcommendq = comm.UpdateCommon(codesq, whreq, tnameq);

        //            string Emp_Msgsuper = "";
        //            GetConnection();
        //            DataTable dtempsup = new DataTable();
        //            cmd = new SqlCommand("select queue_from from iem_trn_tqueue where queue_ref_gid='" + ecfgid + "' and  queue_prev_gid=0 and queue_ref_flag=1 and queue_isremoved='N'", con);
        //            cmd.CommandType = CommandType.Text;
        //            da = new SqlDataAdapter(cmd);
        //            da.Fill(dtempsup);
        //            if (dtempsup.Rows.Count > 0)
        //            {
        //                Emp_Msgsuper = Convert.ToString(dtempsup.Rows[0]["queue_from"].ToString());

        //                string[,] codesIN = new string[,]
        //           {
        //            {"queue_date","sysdatetime()"},
        //            {"queue_ref_flag", "1"},
        //            {"queue_ref_gid",ecfgid },
        //            {"queue_ref_status", EcfRejected.ToString()},
        //            {"queue_from",empgid },
        //            {"queue_to_type", "S"},
        //            {"queue_to", Emp_Msgsuper.ToString()},
        //            {"queue_action_for", "R"},  
        //            {"queue_action_status", "0"}, 
        //            {"queue_prev_gid", queuegid.ToString()}
        //          };
        //                string tnameIN = "iem_trn_tqueue";

        //                string insertcommendecf = comm.InsertCommon(codesIN, tnameIN);

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        con.Close();
        //        da.Dispose();
        //    }
        //}
        //public string EcfraiserReject(string[] SelectedValues)
        //{
        //    string resule = "";
        //    string selval = "";

        //    try
        //    {
        //        if (SelectedValues != null)
        //        {
        //            foreach (string id in SelectedValues)
        //            {
        //                if (selval == "")
        //                {
        //                    selval = id.ToString();
        //                }
        //                else
        //                {
        //                    selval = selval + "','" + id.ToString();
        //                }
        //            }
        //            if (selval != "")
        //            {
        //                Hashtable hstapp = new Hashtable();
        //                Hashtable hstunapp = new Hashtable();

        //                Hashtable hstappemp = new Hashtable();
        //                Hashtable hstunappemp = new Hashtable();
        //                GetConnection();
        //                DataTable dt = new DataTable();
        //                string str = "select c.employee_grade_code, b.ecf_supplier_gid as centralinward_supplier_gid,c.employee_name as centralinward_supplier_name,CONVERT(varchar(15),b.ecf_date,105) as centralinward_invoice_date,b.ecf_no as centralinward_invoice_no,b.ecf_amount as centralinward_invoice_amount,b.ecf_remark as centralinward_remark, (d.role_code+'-'+d.role_name) as rempname,c.employee_gid,c.employee_name,c.employee_code ,'N' AS Provision,'' as invoice_service_month,'N' as invoice_retention_flag,'' as invoice_retention_rate,'' as invoice_retention_amount,'' as invoice_retention_exception,'' as invoice_retention_releaseon  FROM iem_trn_tqueue as a  INNER JOIN iem_trn_tecf as b on a.queue_ref_gid=b.ecf_gid AND b.ecf_isremoved='N'  INNER JOIN iem_mst_temployee as c on c.employee_gid=a.queue_from AND c.employee_isremoved='N'   INNER JOIN iem_mst_trole as d on d.role_gid=a.queue_to AND d.role_isremoved='N'  INNER JOIN iem_mst_tdoctype AS e ON E.doctype_gid=B.ecf_doctype_gid  WHERE  a.queue_isremoved='N' AND a.queue_action_for='A' AND b.ecf_supplier_employee='S' AND a.queue_ref_flag=1  and b.ecf_all_status!=0 and ecf_gid=" + selval + "";
        //                cmd = new SqlCommand(str, con);
        //                cmd.CommandType = CommandType.Text;
        //                da = new SqlDataAdapter(cmd);
        //                da.Fill(dt);
        //                if (dt.Rows.Count > 0)
        //                {
        //                    if (resule == "")
        //                    {
        //                        HttpContext.Current.Session["CentralTables"] = dt;
        //                        resule = "Success";
        //                        return resule;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                resule = "select Same Supplier Name or Same Raiser Name";
        //                return resule;
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message.ToString();
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return resule;
        //}
        public IEnumerable<CentraldataModel> SearchMaker(string SupplierType, string RecivedDateFrom, string RecivedDateTo, string invoiceAmount, string InvoiceDate, string raisercode, string raisername, string suppliercode, string Suppliername, string Department, string InvoiceNo, string PONO)
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {

                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (RecivedDateFrom != "")
                {
                    cmd.Parameters.Add("@RECEIVEDDATEFROM", SqlDbType.VarChar).Value = cmnfun.convertoDateTimeString(RecivedDateFrom.ToString());
                }
                if (RecivedDateTo != "")
                {
                    cmd.Parameters.Add("@RECEIVEDDATETO", SqlDbType.VarChar).Value = cmnfun.convertoDateTimeString(RecivedDateTo.ToString());
                }
                cmd.Parameters.Add("@RAISERNAME", SqlDbType.VarChar).Value = raisername;
                cmd.Parameters.Add("@RAISERCODE", SqlDbType.VarChar).Value = raisercode;
                cmd.Parameters.Add("@INVOICENO", SqlDbType.VarChar).Value = InvoiceNo;
                if (InvoiceDate != "")
                {
                    cmd.Parameters.Add("@INVOICEDATE", SqlDbType.VarChar).Value = cmnfun.convertoDateTimeString(InvoiceDate.ToString()); ;
                }
                cmd.Parameters.Add("@INVOICEAMOUNT", SqlDbType.VarChar).Value = invoiceAmount;
                cmd.Parameters.Add("@DEPARTMENT", SqlDbType.VarChar).Value = Department;
                cmd.Parameters.Add("@PONO", SqlDbType.VarChar).Value = PONO;
                cmd.Parameters.Add("@SUPPLIERCODE", SqlDbType.VarChar).Value = suppliercode;
                cmd.Parameters.Add("@SUPLIERNAME", SqlDbType.VarChar).Value = Suppliername;
                if (SupplierType == "Approved")
                {
                    cmd.Parameters.Add("@SUPPLIERTYPE", SqlDbType.VarChar).Value = 'A';
                }
                if (SupplierType == "UnApproved")
                {
                    cmd.Parameters.Add("@SUPPLIERTYPE", SqlDbType.VarChar).Value = 'U';
                }
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "MAKERSEARCH";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.SupplierCode = row["supplierheader_suppliercode"].ToString();
                    objModel.InwardId = Convert.ToInt32(row["centralinward_gid"].ToString());
                    objModel.ReceivedDate = row["centralinward_received_date"].ToString();
                    objModel.SupplierId = Convert.ToInt32(row["centralinward_supplier_gid"].ToString());
                    objModel.SupplierName = row["centralinward_supplier_name"].ToString();
                    objModel.RaiserName = row["employee_name"].ToString();
                    objModel.InvoiceNo = row["centralinward_invoice_no"].ToString();
                    objModel.InvoiceDate = row["centralinward_invoice_date"].ToString();
                    objModel.PONo = row["centralinward_po_no"].ToString();
                    objModel.InvoiceAmount = cmnfun.GetINRAmount(row["centralinward_invoice_amount"].ToString());
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
        public string Ecfraiser(string[] SelectedValues)
        {
            string resule = "";
            string selval = "";

            try
            {
                HttpContext.Current.Session["CentralTables"] = null;
                if (SelectedValues != null)
                {
                    foreach (string id in SelectedValues)
                    {
                        if (selval == "")
                        {
                            if (id != "0")
                            {
                                selval = id.ToString();
                            }
                        }
                        else
                        {
                            if (id != "0")
                            {
                                selval = selval + "','" + id.ToString();
                            }
                        }
                    }
                    if (selval != "")
                    {
                        Hashtable hstapp = new Hashtable();
                        //Hashtable hstunapp = new Hashtable();

                        Hashtable hstappemp = new Hashtable();
                        //Hashtable hstunappemp = new Hashtable();
                        Hashtable hstappcurr = new Hashtable();
                        Hashtable hstinvtype = new Hashtable();

                        Boolean Approvedflg = false;
                        Boolean unApprovedflg = false;
                        GetConnection();
                        DataTable dt = new DataTable();
                        string str = "select a.centralinward_po_type,a.centralinward_currencyamount,c.currency_code,c.currency_name,centralinward_gid,ROW_NUMBER() over (ORDER BY a.centralinward_invoice_date) AS invoicegid,a.centralinward_supplier_type";
                        str += ",b.employee_grade_code,a.centralinward_supplier_gid";
                        str += ",a.centralinward_supplier_name";
                        str += ",CONVERT(varchar(15),a.centralinward_invoice_date,105) as centralinward_invoice_date";
                        str += ",a.centralinward_invoice_no";
                        str += ",a.centralinward_invoice_amount";
                        str += ",a.centralinward_po_no";
                        str += ",a.centralinward_remark";
                        str += ",b.employee_gid";
                        str += ",b.employee_name";
                        str += ",b.employee_code ";
                        str += ",'N' AS Provision";
                        str += ",'' as invoice_service_month,'N' as invoice_retention_flag,'' as invoice_retention_rate,'' as invoice_retention_amount,'' as invoice_retention_exception,'' as invoice_retention_releaseon";

                        str += ",a.centralinward_isgst,a.centralinward_receiver,isnull(x.suppliergst_gstin,'') as 'suppliergstin',a.centralinward_provider ";  //Pandiaraj 16-02-2019
                        str += " ,isnull(m.branchgst_gstin,'') as 'branchgstin' from iem_trn_tcentralinward as a";
                        str += " inner join iem_mst_temployee as b on a.centralinward_received_from=b.employee_gid";
                        str += " left join iem_mst_tcurrency as c ON c.currency_gid=a.centralinward_currency and c.currency_isremoved='N'";
                        str += " left join asms_trn_tsupplierGST x on x.suppliergst_supplierheader_gid=a.centralinward_supplier_gid and x.suppliergst_state_gid=a.centralinward_provider and x.suppliergst_isremoved='N'";
                        str += " left join iem_trn_tbranchGST m on m.branchgst_state_gid=a.centralinward_receiver and m.branchgst_isremoved='N'";

                        str += " where centralinward_gid in ('" + selval + "') and centralinward_isremoved='N'";
                        cmd = new SqlCommand(str, con);
                        cmd.CommandType = CommandType.Text;
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            int count = 0;
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                count++;
                                string suppliername = "";
                                string supplierid = "";
                                string empid = "";
                                string currency = "";
                                string potype = "";
                                string Approved = Convert.ToString(dt.Rows[i]["centralinward_supplier_type"].ToString());
                                if (Approved == "A")
                                {
                                    if (Approved == "A")
                                    {
                                        if (unApprovedflg == true)
                                        {
                                            resule = "select Same Supplier Name, Same Raiser Name and Same Currency";
                                            return resule;
                                        }
                                        else
                                        {
                                            Approvedflg = true;
                                            supplierid = Convert.ToString(dt.Rows[i]["centralinward_supplier_gid"].ToString());
                                            suppliername = Convert.ToString(dt.Rows[i]["centralinward_supplier_name"].ToString());
                                            empid = Convert.ToString(dt.Rows[i]["employee_gid"].ToString());
                                            currency = Convert.ToString(dt.Rows[i]["currency_code"].ToString());
                                            potype = Convert.ToString(dt.Rows[i]["centralinward_po_type"].ToString());
                                            if (hstapp.Count == 0)
                                            {
                                                hstapp.Add(count, supplierid);
                                            }
                                            else
                                            {
                                                if (hstapp.ContainsValue(supplierid.Trim()))
                                                {
                                                    hstapp.Add(count, supplierid);
                                                }
                                                else
                                                {
                                                    resule = "select Same Supplier Name,Raiser Name,Currency Code and Invoice Type";
                                                    return resule;
                                                }
                                            }
                                            if (hstappemp.Count == 0)
                                            {
                                                hstappemp.Add(count, empid);
                                            }
                                            else
                                            {
                                                if (hstappemp.ContainsValue(empid.Trim()))
                                                {
                                                    hstappemp.Add(count, empid);
                                                }
                                                else
                                                {
                                                    resule = "select Same Supplier Name,Raiser Name,Currency Code and Invoice Type";
                                                    return resule;
                                                }
                                            }
                                            if (hstappcurr.Count == 0)
                                            {
                                                hstappcurr.Add(count, currency);
                                            }
                                            else
                                            {
                                                if (hstappcurr.ContainsValue(currency.Trim()))
                                                {
                                                    hstappcurr.Add(count, currency);
                                                }
                                                else
                                                {
                                                    resule = "select Same Supplier Name,Raiser Name,Currency Code and Invoice Type";
                                                    return resule;
                                                }
                                            }
                                            if (hstinvtype.Count == 0)
                                            {
                                                hstinvtype.Add(count, potype);
                                            }
                                            else
                                            {
                                                if (hstinvtype.ContainsValue(potype.Trim()))
                                                {
                                                    hstinvtype.Add(count, potype);
                                                }
                                                else
                                                {
                                                    resule = "select Same Supplier Name,Raiser Name,Currency Code and Invoice Type";
                                                    return resule;
                                                }
                                            }
                                        }

                                        GetConnection();
                                        cmd = new SqlCommand("pr_eow_trn_excelvalate", con);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.Add("@chkdata", SqlDbType.VarChar).Value = Convert.ToString(dt.Rows[i]["centralinward_invoice_no"].ToString());
                                        cmd.Parameters.Add("@chkdata1", SqlDbType.VarChar).Value = Convert.ToString(dt.Rows[i]["centralinward_supplier_gid"].ToString());
                                        cmd.Parameters.Add("@Result", SqlDbType.VarChar).Value = "DuplicateInvoice";
                                        result = (string)cmd.ExecuteScalar();
                                        if (result == "Exists")
                                        {
                                            resule = "Selected Invoice Number Already Used in Another ECF";
                                            return resule;
                                        }
                                    }
                                    else if (Approved == "U")
                                    {
                                        //if (Approvedflg == true)
                                        //{
                                        //    resule = "select Same Supplier Name or Same Raiser Name";
                                        //    return resule;
                                        //}
                                        //else
                                        //{
                                        //    unApprovedflg = true;
                                        //    supplierid = Convert.ToString(dt.Rows[i]["centralinward_supplier_gid"].ToString());
                                        //    suppliername = Convert.ToString(dt.Rows[i]["centralinward_supplier_name"].ToString());
                                        //    empid = Convert.ToString(dt.Rows[i]["employee_gid"].ToString());

                                        //    if (hstunapp.Count == 0)
                                        //    {
                                        //        hstunapp.Add(count, suppliername);
                                        //    }
                                        //    else
                                        //    {
                                        //        if (hstunapp.ContainsValue(suppliername.Trim()))
                                        //        {
                                        //            hstunapp.Add(count, suppliername);
                                        //        }
                                        //        else
                                        //        {
                                        //            resule = "select Same Supplier Name or Same Raiser Name";
                                        //            return resule;
                                        //        }
                                        //    }
                                        //    if (hstunappemp.Count == 0)
                                        //    {
                                        //        hstunappemp.Add(count, empid);
                                        //    }
                                        //    else
                                        //    {
                                        //        if (hstunappemp.ContainsValue(empid.Trim()))
                                        //        {
                                        //            hstunappemp.Add(count, empid);
                                        //        }
                                        //        else
                                        //        {
                                        //            resule = "select Same Raiser Name or Same Raiser Name";
                                        //            return resule;
                                        //        }
                                        //    }
                                        //}

                                        resule = "Can Not Raise ECF For UnApproved Suppliers";
                                        return resule;

                                        //GetConnection();
                                        //cmd = new SqlCommand("pr_eow_trn_excelvalate", con);
                                        //cmd.CommandType = CommandType.StoredProcedure;
                                        //cmd.Parameters.Add("@chkdata", SqlDbType.VarChar).Value = Convert.ToString(dt.Rows[i]["centralinward_invoice_no"].ToString());
                                        //cmd.Parameters.Add("@chkdata1", SqlDbType.VarChar).Value = Convert.ToString(dt.Rows[i]["centralinward_supplier_gid"].ToString());
                                        //cmd.Parameters.Add("@Result", SqlDbType.VarChar).Value = "DuplicateInvoice";
                                        //result = (string)cmd.ExecuteScalar();
                                        //if (result == "Exists")
                                        //{
                                        //    resule = "Selected Invoice Number Already Used in Another ECF";
                                        //    return resule;
                                        //}
                                    }
                                }
                                else
                                {
                                    resule = "Central Team Can Only Raise Approved Supplier Invoices";
                                    return resule;
                                }
                            }
                            if (resule == "")
                            {
                                HttpContext.Current.Session["CentralTables"] = dt;
                                resule = "Success";
                                return resule;
                            }
                        }
                    }
                    else
                    {
                        resule = "select Same Supplier Name or Same Raiser Name";
                        return resule;
                    }
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
            return resule;
        }
        public string selectsuppcode(string EmployeeeGid)
        {
            string Emp_Msg = "";
            try
            {
                GetConnection();
                string str = "pr_eow_com_gettaxrate";
                DataTable dt = new DataTable();
                //str=" select supplierheader_suppliercode from asms_trn_tsupplierheader where supplierheader_gid='" + EmployeeeGid + "' ;
                cmd = new SqlCommand(str, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Invoice_gid", SqlDbType.VarChar).Value = EmployeeeGid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GETSUPPLIERDETAILS";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Emp_Msg = Convert.ToString(dt.Rows[0]["supplierheader_suppliercode"].ToString());
                }
                return Emp_Msg;
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
        public DataTable selectcurrency(string EmployeeeGid)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                string str = "pr_eow_com_gettaxrate";
                //str += " select A.currency_gid,A.currency_code,B.currencyrate_value from iem_mst_tcurrency AS A";
                //str += " INNER JOIN iem_mst_tcurrencyrate AS B ON B.currencyrate_currency_gid=A.currency_gid AND B.currencyrate_isremoved='N'";
                //str += " where A.currency_code='" + EmployeeeGid + "' and A.currency_isremoved='N'";
                cmd = new SqlCommand(str, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@currency_gid", SqlDbType.VarChar).Value = EmployeeeGid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GETCURRENCYDETAILS";
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
                da.Dispose();
            }
        }

        public IEnumerable<CentraldataModel> SelectrejectDetails(string txtdbdocdateReject, string txtdbdocnoReject, string txtdbdocamountReject)
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {
                dt = new DataTable();
                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_CentralTeamChecker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DOCDATE", SqlDbType.VarChar).Value = txtdbdocdateReject;
                cmd.Parameters.Add("@DOCAMOUNT", SqlDbType.VarChar).Value = txtdbdocamountReject;
                cmd.Parameters.Add("@DOCNO", SqlDbType.VarChar).Value = txtdbdocnoReject;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SEARCHCENTRALTEAM_reject";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.Docnogid = Convert.ToInt32(row["queue_gid"].ToString());
                    objModel.Docno = row["ecf_no"].ToString();
                    objModel.DocDate = row["queue_date"].ToString();
                    objModel.Docamount = cmnfun.GetINRAmount(row["ecf_amount"].ToString());
                    objModel.RaiserName = row["rempname"].ToString();
                    objModel.emporsupp = row["sempname"].ToString();
                    objModel.SupplierName = row["supplierheader_name"].ToString();
                    objModel.ecfstatus = "Rejected";
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
        public IEnumerable<CentraldataModel> SelectCentralCheckerDetails(string txtdbdocdate, string txtdbdocno, string txtdbdocamount)
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {
                dt = new DataTable();
                string ecfdate = "";
                if (txtdbdocdate != "")
                {
                    //ecfdate = cmnfun.convertoDateTimeString(txtdbdocdate);
                }

                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_CentralTeamChecker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@DOCDATE", SqlDbType.VarChar).Value = txtdbdocdate;
                cmd.Parameters.Add("@DOCAMOUNT", SqlDbType.VarChar).Value = txtdbdocamount;
                cmd.Parameters.Add("@DOCNO", SqlDbType.VarChar).Value = txtdbdocno;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SEARCHCENTRALTEAMCHECKER";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.Docnogid = Convert.ToInt32(row["queue_gid"].ToString());
                    objModel.Docno = row["ecf_no"].ToString();
                    objModel.DocDate = row["queue_date"].ToString();
                    objModel.Docamount = cmnfun.GetINRAmount(row["ecf_amount"].ToString());
                    objModel.RaiserName = row["rempname"].ToString();
                    objModel.emporsupp = row["sempname"].ToString();
                    objModel.SupplierName = row["supplierheader_name"].ToString();
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
        public IEnumerable<CentraldataModel> CentralCheckerDetails(string loginempcode)
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {
                dt = new DataTable();
                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_CentralTeamChecker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@LoginEmp_GID", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(loginempcode) ? "0" : loginempcode);
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "CENTRALTEAMCHECKER";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.Docnogid = Convert.ToInt32(row["queue_gid"].ToString());
                    objModel.Docno = row["ecf_no"].ToString();
                    objModel.DocDate = row["queue_date"].ToString();
                    objModel.Docamount = cmnfun.GetINRAmount(row["ecf_amount"].ToString());
                    objModel.RaiserName = row["rempname"].ToString();
                    objModel.emporsupp = row["sempname"].ToString();
                    objModel.SupplierName = row["supplierheader_name"].ToString();
                    objModel.ECFMaker = row["ECF Maker"].ToString();
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
        public IEnumerable<CentraldataModel> CentralRejectDetails()
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {
                dt = new DataTable();
                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_CentralTeamChecker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "CENTRALTEAM_REJECT";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    if (row["ecf_no"].ToString() != "")
                    {
                        objModel.InwardId = Convert.ToInt32(row["queue_gid"].ToString());
                        objModel.Docnogid = Convert.ToInt32(row["ecf_gid"].ToString());
                        objModel.Docno = row["ecf_no"].ToString();
                        objModel.DocDate = row["ecf_date"].ToString();
                        objModel.Docamount = cmnfun.GetINRAmount(row["ecf_amount"].ToString());
                        objModel.RaiserName = row["rempname"].ToString();
                        objModel.emporsupp = row["sempname"].ToString();
                        objModel.SupplierName = row["supplierheader_name"].ToString();

                        // Pandiaraj 12-10-2018

                        //if (row["ecf_status"].ToString() == "10")
                        //{
                        //    objModel.ecfstatus = "CT - Rejected";
                        //}
                        //else
                        //{
                        //    objModel.ecfstatus = "Raiser - Rejected";
                        //}

                        if (row["ecf_status"].ToString() == "10")
                        {
                            objModel.ecfstatus = "CT - Rejected";
                        }
                        else if (row["ecf_status"].ToString() == "20")
                        {
                            objModel.ecfstatus = "Raiser - Rejected";
                        }
                        else if (row["ecf_status"].ToString() == "32")
                        {
                            objModel.ecfstatus = "Inprocess";
                        }
                        else if (row["ecf_status"].ToString() == "0")
                        {
                            objModel.ecfstatus = "CT - Submission";
                        }
                        else if (row["ecf_status"].ToString() == "1")
                        {
                            objModel.ecfstatus = "Approved";
                        }
                        else if (row["ecf_status"].ToString() == "2")
                        {
                            objModel.ecfstatus = "CT - Hold";
                        }
                        else if (row["ecf_status"].ToString() == "3")
                        {
                            objModel.ecfstatus = "CT - Hold Released";
                        }
                        else if (row["ecf_status"].ToString() == "8")
                        {
                            objModel.ecfstatus = "CT - Return Back";
                        }

                        else if (row["ecf_status"].ToString() == "262144")
                        {
                            objModel.ecfstatus = "CT - Submitted";
                        }
                        else if (row["ecf_status"].ToString() == "524288")
                        {
                            objModel.ecfstatus = "CT – Raiser Reviewed";
                        }
                        // Pandiaraj 12-10-2018
                        objModel.ecfselect = "active";
                        objModel.ecfview = "notactive";
                    }
                    else
                    {
                        objModel.Docnogid = Convert.ToInt32(row["ecf_gid"].ToString());
                        objModel.Docno = "";
                        objModel.DocDate = row["ecf_date"].ToString();
                        objModel.Docamount = cmnfun.GetINRAmount(row["ecf_amount"].ToString());
                        objModel.RaiserName = row["rempname"].ToString();
                        objModel.emporsupp = row["sempname"].ToString();
                        objModel.SupplierName = row["supplierheader_name"].ToString();
                        objModel.ecfstatus = "CT Draft";

                        objModel.ecfselect = "active";
                        objModel.ecfview = "active";
                    }

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
        public IEnumerable<CentraldataModel> CentralMakerView(int id)
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {

                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinwardlogview", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SUPPLIERID", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "CENTRALLOGVIEW";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.DOT = row["DOT"].ToString();
                    objModel.MODE = row["MODE"].ToString();
                    objModel.Remarks = row["REMARKS"].ToString();
                    objModel.BY = Convert.ToInt32(row["TBY"].ToString());
                    objModel.EmployeeName = row["EMPLOYEENAME"].ToString();
                    objModel.EmployeeCode = row["EMPLOYEECODE"].ToString();
                    objModel.InvoiceNo = row["INVOICENO"].ToString();
                    objModel.InvoiceAmount = cmnfun.GetINRAmount(row["INVOICEAMOUNT"].ToString());
                    objModel.InvoiceDate = row["INVOICEDATE"].ToString();
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
        public IEnumerable<CentraldataModel> SearchByDate()
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {

                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SEARCHBYDATE";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.InwardId = Convert.ToInt32(row["centralinward_gid"].ToString());
                    objModel.ReceivedDate = row["centralinward_received_date"].ToString();
                    objModel.SupplierId = Convert.ToInt32(row["centralinward_supplier_gid"].ToString());
                    objModel.SupplierName = row["centralinward_supplier_name"].ToString();
                    objModel.RaiserName = row["employee_name"].ToString();
                    objModel.InvoiceNo = row["centralinward_invoice_no"].ToString();
                    objModel.InvoiceDate = row["centralinward_invoice_date"].ToString();
                    objModel.PONo = row["centralinward_po_no"].ToString();
                    objModel.InvoiceAmount = cmnfun.GetINRAmount(row["centralinward_invoice_amount"].ToString());
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
        public void Insertapprovedaction(Approveraction EmployeeeExpense, string ecfgid, string invoicegid, string empgid, string ecftype, string queuegid, string queueamt)
        {
            throw new NotImplementedException();
        }
        public IEnumerable<CentraldataModel> HoldSearchByDate()
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {

                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "HOLDSEARCHBYDATE";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.InwardId = Convert.ToInt32(row["centralinward_gid"].ToString());
                    objModel.ReceivedDate = row["centralinward_received_date"].ToString();
                    objModel.SupplierId = Convert.ToInt32(row["centralinward_supplier_gid"].ToString());
                    objModel.SupplierName = row["centralinward_supplier_name"].ToString();
                    objModel.RaiserName = row["employee_name"].ToString();
                    objModel.InvoiceNo = row["centralinward_invoice_no"].ToString();
                    objModel.InvoiceDate = row["centralinward_invoice_date"].ToString();
                    objModel.PONo = row["centralinward_po_no"].ToString();
                    objModel.InvoiceAmount = cmnfun.GetINRAmount(row["centralinward_invoice_amount"].ToString());
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

        public IEnumerable<CentraldataModel> MakerSearchByDate()
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {

                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "MAKERSEARCHBYDATE";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.InwardId = Convert.ToInt32(row["centralinward_gid"].ToString());
                    objModel.ReceivedDate = row["centralinward_received_date"].ToString();
                    objModel.SupplierId = Convert.ToInt32(row["centralinward_supplier_gid"].ToString());
                    objModel.SupplierName = row["centralinward_supplier_name"].ToString();
                    objModel.RaiserName = row["employee_name"].ToString();
                    objModel.InvoiceNo = row["centralinward_invoice_no"].ToString();
                    objModel.InvoiceDate = row["centralinward_invoice_date"].ToString();
                    objModel.PONo = row["centralinward_po_no"].ToString();
                    objModel.InvoiceAmount = cmnfun.GetINRAmount(row["centralinward_invoice_amount"].ToString());
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
        public DataTable SearchReportNew()
        {
            dt = new DataTable();
            try
            {
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTREPORTNew";
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
        public IEnumerable<CentraldataModel> SearchInwardReport(string ReceivedDateFrom, string ReceivedDateTo, string InvoiceDateFrom, string InvoiceDateTo, string Status, string Ecfno, string InvoiceNo, string searchtype)
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {

                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_tCentralInwardReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ReceivedDateFrom", SqlDbType.VarChar).Value = ReceivedDateFrom;
                cmd.Parameters.Add("@ReceivedDateTo", SqlDbType.VarChar).Value = ReceivedDateTo;
                cmd.Parameters.Add("@InvoiceDateFrom", SqlDbType.VarChar).Value = InvoiceDateFrom;
                cmd.Parameters.Add("@InvoiceDateTo", SqlDbType.VarChar).Value = InvoiceDateTo;
                cmd.Parameters.Add("@STATUS", SqlDbType.VarChar).Value = Status;
                cmd.Parameters.Add("@EcfNo", SqlDbType.VarChar).Value = Ecfno;
                cmd.Parameters.Add("@InvoiceNo", SqlDbType.VarChar).Value = InvoiceNo;
                cmd.Parameters.Add("@Searchtype", SqlDbType.VarChar).Value = searchtype;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "InwardSearchReport";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.InwardId = Convert.ToInt32(row["centralinward_gid"].ToString());

                    if (row["QUEUE_GID"].ToString() != "" && row["QUEUE_GID"].ToString() != null)
                    {
                        objModel.queue_gid = Convert.ToInt32(row["QUEUE_GID"].ToString());
                    }
                    else
                    {
                        objModel.queue_gid = 0;
                    }
                    objModel.ReceivedDate = row["centralinward_received_date"].ToString();
                    objModel.SupplierId = Convert.ToInt32(row["centralinward_supplier_gid"].ToString());
                    objModel.SupplierName = row["centralinward_supplier_name"].ToString();
                    objModel.RaiserName = row["employee_name"].ToString();
                    objModel.InvoiceNo = row["centralinward_invoice_no"].ToString();
                    objModel.InvoiceDate = row["centralinward_invoice_date"].ToString();
                    objModel.PONo = row["centralinward_po_no"].ToString();
                    //objModel.InvoiceAmount = row["centralinward_invoice_amount"].ToString();
                    objModel.InvoiceAmount = cmnfun.GetINRAmount(row["centralinward_invoice_amount"].ToString());
                    objModel.EmployeeDepartment = row["employee_dept_name"].ToString();
                    objModel.Remarks = row["centralinward_remark"].ToString();
                    objModel.PaidDate = row["centralinward_paid_date"].ToString(); //selva
                    objModel.Docno = row["ecf_no"].ToString();
                    objModel.empName = row["ctmkrname"].ToString();
                    objModel.maingid = row["ecf_gid"].ToString();
                    if (row["ecf_status"].ToString() == "1")
                    {
                        objModel.emporsupp = "1";
                        objModel.ecfstatus = "Approved";
                    }
                    else if (row["ecf_status"].ToString() == "2048")
                    {
                        objModel.emporsupp = "2048";
                        objModel.ecfstatus = "EPURejected";
                    }
                    else if (row["ecf_status"].ToString() == "65536")
                    {
                        objModel.emporsupp = "65536";
                        objModel.ecfstatus = "Paid";
                    }
                    else if (row["ecf_status"].ToString() == "512" || row["ecf_status"].ToString() == "1024"
                        || row["ecf_status"].ToString() == "16384" || row["ecf_status"].ToString() == "64"
                        || row["ecf_status"].ToString() == "128" || row["ecf_status"].ToString() == "256"
                        || row["ecf_status"].ToString() == "16")
                    {
                        objModel.emporsupp = row["ecf_status"].ToString();
                        objModel.ecfstatus = "EPUInproces";
                    }
                    else
                    {
                        if (row["ecf_gid"].ToString() != "" && row["ecf_no"].ToString() == "")
                        {
                            objModel.emporsupp = "262144";
                            objModel.ecfstatus = "CT Draft";
                        }
                        else
                        {
                            if (row["ecf_status"].ToString() == "4")
                            {
                                objModel.emporsupp = "1";
                                objModel.ecfstatus = "Approved";
                            }
                            else
                            {
                                objModel.emporsupp = row["centralinward_status"].ToString();
                                objModel.ecfstatus = row["ECFstatus"].ToString();
                            }
                        }
                    }
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
        public IEnumerable<CentraldataModel> SearchReportdashboard()
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {

                CentraldataModel objModel;
                cmd = new SqlCommand("pr_eow_trn_mailckeck", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Result", SqlDbType.VarChar).Value = "Getctmakerrpt";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.InwardId = Convert.ToInt32(row["centralinward_gid"].ToString());

                    if (row["QUEUE_GID"].ToString() != "" && row["QUEUE_GID"].ToString() != null)
                    {
                        objModel.queue_gid = Convert.ToInt32(row["QUEUE_GID"].ToString());
                    }
                    objModel.ReceivedDate = row["centralinward_received_date"].ToString();
                    objModel.SupplierId = Convert.ToInt32(row["centralinward_supplier_gid"].ToString());
                    objModel.SupplierName = row["centralinward_supplier_name"].ToString();
                    objModel.RaiserName = row["employee_name"].ToString();
                    objModel.InvoiceNo = row["centralinward_invoice_no"].ToString();
                    objModel.InvoiceDate = row["centralinward_invoice_date"].ToString();
                    objModel.PONo = row["centralinward_po_no"].ToString();
                    objModel.InvoiceAmount = cmnfun.GetINRAmount(row["centralinward_invoice_amount"].ToString());
                    string ecfstatus = row["ecf_status"].ToString();
                    if (ecfstatus == "1")
                    {
                        objModel.ecfstatus = "Approved";
                    }
                    else if (ecfstatus == "10")
                    {
                        objModel.ecfstatus = "Rejected";
                    }
                    else
                    {
                        objModel.ecfstatus = "Inprocess";
                    }
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
        public IEnumerable<CentraldataModel> SelectPONo(CentraldataModel delnote)
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {

                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SUPPLIERID", SqlDbType.VarChar).Value = delnote.SupplierId;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTPONOBYID";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.POid = Convert.ToInt32(row["poheader_gid"].ToString());
                    objModel.PONo = row["poheader_pono"].ToString();
                    objModel.pototal = row["poheader_over_total"].ToString();
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
        public IEnumerable<CentraldataModel> SelectPONo()
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {

                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTPONO";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.POid = Convert.ToInt32(row["poheader_gid"].ToString());
                    objModel.PONo = row["poheader_pono"].ToString();

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
        public IEnumerable<CentraldataModel> SelectPodetailswithoutarg(int SupId, string claimType)
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {

                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SUPPLIERID", SqlDbType.Int).Value = SupId;
                cmd.Parameters.Add("@POType", SqlDbType.VarChar).Value = claimType;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTPONOBYType";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.POid = Convert.ToInt32(row["poheader_gid"].ToString());
                    objModel.PONo = row["poheader_pono"].ToString();
                    objModel.pototal = row["poheader_over_total"].ToString();
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
        public IEnumerable<CentraldataModel> SelectPodetails(string EmployeeName, string EmployeeCode, int poid, string claimType)
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {

                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SUPPLIERID", SqlDbType.Int).Value = poid;
                cmd.Parameters.Add("@PONO", SqlDbType.VarChar).Value = EmployeeCode;
                cmd.Parameters.Add("@pototal", SqlDbType.VarChar).Value = EmployeeName;
                if (!string.IsNullOrEmpty(claimType))
                {
                    cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTPONOBYType";
                    cmd.Parameters.Add("@POType", SqlDbType.VarChar).Value = claimType;
                }
                else
                {
                    cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTPONOBYID";
                }//SELECTPONOBYType SELECTPONOBYID
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.POid = Convert.ToInt32(row["poheader_gid"].ToString());
                    objModel.PONo = row["poheader_pono"].ToString();
                    objModel.pototal = row["poheader_over_total"].ToString();
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


        public IEnumerable<CentraldataModel> SelectPONoByid(int gid)
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {

                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SUPPLIERID", SqlDbType.Int).Value = gid;
                //cmd.Parameters.Add("@PONO", SqlDbType.VarChar).Value = pono;
                //cmd.Parameters.Add("@pototal", SqlDbType.VarChar).Value = pototal;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTPONOBYID";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.POid = Convert.ToInt32(row["poheader_gid"].ToString());
                    objModel.PONo = row["poheader_pono"].ToString();
                    objModel.pototal = row["poheader_over_total"].ToString();
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
        public IEnumerable<CentraldataModel> SelectCentralMaker()
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {

                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTCENTRALINWARD";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.InwardId = Convert.ToInt32(row["centralinward_gid"].ToString());
                    objModel.ReceivedDate = row["centralinward_received_date"].ToString();
                    objModel.SupplierId = Convert.ToInt32(row["centralinward_supplier_gid"].ToString());
                    objModel.SupplierName = row["centralinward_supplier_name"].ToString();
                    objModel.SupplierCode = row["supplierheader_suppliercode"].ToString();
                    objModel.RaiserName = row["employee_name"].ToString();
                    objModel.InvoiceNo = row["centralinward_invoice_no"].ToString();
                    objModel.InvoiceDate = row["centralinward_invoice_date"].ToString();
                    objModel.PONo = row["centralinward_po_no"].ToString();
                    //objModel.InvoiceAmount = row["centralinward_invoice_amount"].ToString();
                    objModel.InvoiceAmount = cmnfun.GetINRAmount(row["centralinward_invoice_amount"].ToString());
                    objModel.EmployeeDepartment = row["employee_dept_name"].ToString();
                    objModel.Remarks = row["centralinward_remark"].ToString();
                    objModel.CurrencyName = row["currency_code"].ToString();
                    objModel.POtype = row["centralinward_po_type"].ToString();
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

        public string CheckPono(string PONO)
        {
            SqlCommand cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
            cmd.Parameters.AddWithValue("@PONO", SqlDbType.VarChar).Value = PONO;
            cmd.Parameters.AddWithValue("@ACTION", "CHECKPONO");
            cmd.CommandType = CommandType.StoredProcedure;
            result = (string)cmd.ExecuteScalar();
            return result;
        }
        public CentraldataModel SelectEmployeeSearch111(string EmployeeName, string EmployeeCode)
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            CentraldataModel objModel = new CentraldataModel();
            try
            {


                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RAISERNAME", SqlDbType.VarChar).Value = EmployeeName;
                cmd.Parameters.Add("@RAISERCODE", SqlDbType.VarChar).Value = EmployeeCode;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTEMPLOYEEDETAILSSEARCH";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.EmployeeId = Convert.ToInt32(row["employee_gid"].ToString());
                    objModel.EmployeeDepartment = row["employee_dept_name"].ToString();
                    objModel.RaiserName = row["employee_name"].ToString();
                    objModel.RaiserCode = row["employee_code"].ToString();
                    emp.Add(objModel);
                }
                return objModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objModel;
            }
            finally
            {
                con.Close();
            }
        }


        public IEnumerable<CentraldataModel> GetCurrency()
        {
            List<CentraldataModel> objNatureofExpenses = new List<CentraldataModel>();
            try
            {

                CentraldataModel objModel;
                objNatureofExpenses.Add(new CentraldataModel { CurrencyId = "0", CurrencyName = "-- Select --", });
                DataTable dt = new DataTable();
                GetConnection();
                cmd = new SqlCommand("pr_eow_com_gettaxrate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getcurrencys";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new CentraldataModel();
                    objModel.CurrencyId = Convert.ToString(dt.Rows[i]["currency_gid"].ToString());
                    objModel.CurrencyName = Convert.ToString(dt.Rows[i]["currency_code"].ToString());
                    objNatureofExpenses.Add(objModel);
                }
                return objNatureofExpenses;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objNatureofExpenses;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        public IEnumerable<CentraldataModel> AutofilterEmployee(string EmployeeName, string EmployeeCode)
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {
                dt = new DataTable();
                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RAISERNAME", SqlDbType.VarChar).Value = EmployeeName;
                cmd.Parameters.Add("@RAISERCODE", SqlDbType.VarChar).Value = EmployeeCode;
                //cmd.Parameters.Add("@DEPARTMENT", SqlDbType.VarChar).Value = EmployeeName;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "AutocompleteEmployee";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.EmployeeId = Convert.ToInt32(row["employee_gid"].ToString());
                    objModel.EmployeeDepartment = row["employee_dept_name"].ToString();
                    objModel.RaiserName = row["employee_name"].ToString();
                    objModel.RaiserCode = row["employee_code"].ToString();
                    objModel.empbranch = row["employee_branch_gid"].ToString();
                    objModel.empfc = row["employee_fc_code"].ToString();
                    objModel.empcc = row["employee_cc_code"].ToString();
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

        public IEnumerable<CentraldataModel> SelectEmployeeDelegationbyDept(string deptgid)
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {
                dt = new DataTable();
                CentraldataModel objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EMPGID", SqlDbType.Int).Value = cmnfun.GetLoginUserGid();
                cmd.Parameters.Add("@DEPARTMENT", SqlDbType.VarChar).Value = deptgid;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTEMPLOYEEDEPT";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.EmployeeId = Convert.ToInt32(row["employee_gid"].ToString());
                    objModel.EmployeeDepartment = row["employee_dept_name"].ToString();
                    objModel.RaiserName = row["employee_name"].ToString();
                    objModel.RaiserCode = row["employee_code"].ToString();
                    objModel.empbranch = string.IsNullOrEmpty(row["employee_branch_gid"].ToString()) ? "" : row["employee_branch_gid"].ToString();
                    objModel.empfc = string.IsNullOrEmpty(row["employee_fc_code"].ToString()) ? "" : row["employee_fc_code"].ToString();
                    objModel.empcc = string.IsNullOrEmpty(row["employee_cc_code"].ToString()) ? "" : row["employee_cc_code"].ToString();
                    objModel.ecfselect = string.IsNullOrEmpty(row["employee_branch_code"].ToString()) ? "" : row["employee_branch_code"].ToString();

                    objModel.empfcname = string.IsNullOrEmpty(row["fc_name"].ToString()) ? "" : row["fc_name"].ToString();
                    objModel.empccname = string.IsNullOrEmpty(row["cc_name"].ToString()) ? "" : row["cc_name"].ToString();
                    objModel.empouname = string.IsNullOrEmpty(row["ou_name"].ToString()) ? "" : row["ou_name"].ToString();
                    objModel.empproductname = string.IsNullOrEmpty(row["product_name"].ToString()) ? "" : row["product_name"].ToString();
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

        //--Pandiaraj 18-02-2019
        public DataSet forexcel(int uploadgid, string type)
        {
            DataSet ds = new DataSet();
            cmd = new SqlCommand("pr_Upload_BulkInvoice", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@statement", "A");
            cmd.Parameters.AddWithValue("@uploadgid", uploadgid);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            return ds;
        }
        public DataTable bulkinvoice(string invoicexml, string filename)
        {
            DataTable bdt = new DataTable();
            cmd = new SqlCommand("pr_Upload_BulkInvoice", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UPLOAD_FILENAME", filename.Trim());
            cmd.Parameters.AddWithValue("@statement", "I");
            cmd.Parameters.AddWithValue("@UPLOAD_BY", Convert.ToInt32(cmnfun.GetLoginUserGid().ToString()));
            cmd.Parameters.AddWithValue("@invoicexml", invoicexml);
            //  result = (string)cmd.ExecuteScalar();
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da = new SqlDataAdapter(cmd);
            da.Fill(bdt);
            return bdt;
        }

    }
}