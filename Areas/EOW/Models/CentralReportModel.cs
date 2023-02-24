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
    public class CentralReportModel : CentralReportRepository
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
        int EmployeeFromId, EmployeeToId;
        string result;
        public CentralReportModel()
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
        public IEnumerable<CentraldataModel> MISReportLoad(string InvoiceDate, string invoiceAmount, string ECFNo, string ECFDate, string Status)
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {
                CentraldataModel objModel;
                cmd = new SqlCommand("Pr_iem_trn_tCentralReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (ECFDate != "")
                {
                    cmd.Parameters.Add("@EcfDate", SqlDbType.VarChar).Value = ECFDate.ToString();
                }
                if (InvoiceDate != "")
                {
                    cmd.Parameters.Add("@InvoiceDate", SqlDbType.VarChar).Value = InvoiceDate.ToString();
                }
                cmd.Parameters.Add("@InvoiceAmount", SqlDbType.VarChar).Value = invoiceAmount;
                cmd.Parameters.Add("@EcfNo", SqlDbType.VarChar).Value = ECFNo;
                cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = Status;
                cmd.Parameters.Add("@Action", SqlDbType.VarChar).Value = "MISReport";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.ReceivedDate = row["Invoice received at CPPU on"].ToString();
                    objModel.EmployeeDepartment = row["Department"].ToString();
                    objModel.SupplierName = row["Vendor Name"].ToString();
                    objModel.InvoiceNo = row["Invoice No"].ToString();
                    objModel.InvoiceDate = row["Invoice Date"].ToString();
                    objModel.InvoiceAmount = row["Invoice Amount"].ToString();
                    objModel.EcfNo = row["ECF Number"].ToString();
                    objModel.EcfDate = row["ECF Date"].ToString();
                    objModel.ecfstatus = row["Status"].ToString();
                    objModel.ReturnedDate = row["Invoice Returned Date"].ToString();
                    objModel.Remarks = row["Return Reason"].ToString();
                    objModel.HoldRemarks = row["Hold Reason"].ToString();
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
        public IEnumerable<CentraldataModel> Collection_MISReport(string InvoiceDate, string invoiceAmount, string ECFNo, string ECFDate, string Status)
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {
                CentraldataModel objModel;
                cmd = new SqlCommand("Pr_iem_trn_tCentralReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (ECFDate != "")
                {
                    cmd.Parameters.Add("@EcfDate", SqlDbType.VarChar).Value = ECFDate.ToString();
                }
                if (InvoiceDate != "")
                {
                    cmd.Parameters.Add("@InvoiceDate", SqlDbType.VarChar).Value = InvoiceDate.ToString();
                }
                cmd.Parameters.Add("@InvoiceAmount", SqlDbType.VarChar).Value = invoiceAmount;
                cmd.Parameters.Add("@EcfNo", SqlDbType.VarChar).Value = ECFNo;
                cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = Status;
                cmd.Parameters.Add("@Action", SqlDbType.VarChar).Value = "Collection_MISReport";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.ReceivedDate = row["Invoice received at CPPU on"].ToString();

                    objModel.RaiserCode = row["Raiser Id"].ToString();
                    objModel.Docno = row["EPU Code"].ToString();

                    objModel.EmployeeDepartment = row["Department"].ToString();
                    objModel.SupplierName = row["Vendor Name"].ToString();
                    objModel.InvoiceNo = row["Invoice No"].ToString();
                    objModel.InvoiceDate = row["Invoice Date"].ToString();
                    objModel.InvoiceAmount = row["Invoice Amount"].ToString();
                    objModel.EcfNo = row["ECF Number"].ToString();
                    objModel.EcfDate = row["ECF Date"].ToString();
                    objModel.ecfstatus = row["Status"].ToString();
                    objModel.ReturnedDate = row["Invoice Returned Date"].ToString();
                    objModel.Remarks = row["Return Reason"].ToString();
                    objModel.HoldRemarks = row["Hold Reason"].ToString();
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
        public IEnumerable<CentraldataModel> MIS_DepartmentwiseAndVendorwise_Report(string InvoiceDate, string invoiceAmount, string ECFNo, string ECFDate, string Status)
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {
                CentraldataModel objModel;
                cmd = new SqlCommand("Pr_iem_trn_tCentralReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (ECFDate != "")
                {
                    cmd.Parameters.Add("@EcfDate", SqlDbType.VarChar).Value = ECFDate.ToString();
                }
                if (InvoiceDate != "")
                {
                    cmd.Parameters.Add("@InvoiceDate", SqlDbType.VarChar).Value = InvoiceDate.ToString();
                }
                cmd.Parameters.Add("@InvoiceAmount", SqlDbType.VarChar).Value = invoiceAmount;
                cmd.Parameters.Add("@EcfNo", SqlDbType.VarChar).Value = ECFNo;
                cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = Status;
                cmd.Parameters.Add("@Action", SqlDbType.VarChar).Value = "MIS_Departmentwise&Vendorwise_Report";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.ReceivedDate = row["Invoice received at CPPU on"].ToString();

                    objModel.DocDate = row["Date When Invoice processed/Returned By CPPU"].ToString();
                    objModel.PONo = row["PO/WONO"].ToString();
                    objModel.DateEcfSentToEPUbyCPPU = row["Date Ecf Sent To EPU by CPPU"].ToString();
                    objModel.FinalApproverDate = row["Final Approver Date"].ToString();
                    objModel.EPURejectDate = row["EPU Reject Date"].ToString();
                    objModel.EPUPaidDate = row["EPU Paid Date"].ToString();

                    objModel.EmployeeDepartment = row["Department"].ToString();
                    objModel.SupplierName = row["Vendor Name"].ToString();
                    objModel.InvoiceNo = row["Invoice No"].ToString();
                    objModel.InvoiceDate = row["Invoice Date"].ToString();
                    objModel.InvoiceAmount = row["Invoice Amount"].ToString();
                    objModel.EcfNo = row["ECF Number"].ToString();

                    objModel.ecfstatus = row["Status"].ToString();
                    objModel.ReturnedDate = row["Invoice Returned Date"].ToString();
                    objModel.Remarks = row["Return Reason"].ToString();
                    objModel.HoldRemarks = row["Hold Reason"].ToString();
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
        public IEnumerable<CentraldataModel> InvoiceMIS_VendorwiseAndPOWOwise_Report(string InvoiceDate, string invoiceAmount, string ECFNo, string ECFDate, string Status)
        {
            List<CentraldataModel> emp = new List<CentraldataModel>();
            try
            {
                CentraldataModel objModel;
                cmd = new SqlCommand("Pr_iem_trn_tCentralReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (ECFDate != "")
                {
                    cmd.Parameters.Add("@EcfDate", SqlDbType.VarChar).Value = ECFDate.ToString();
                }
                if (InvoiceDate != "")
                {
                    cmd.Parameters.Add("@InvoiceDate", SqlDbType.VarChar).Value = InvoiceDate.ToString();
                }
                cmd.Parameters.Add("@InvoiceAmount", SqlDbType.VarChar).Value = invoiceAmount;
                cmd.Parameters.Add("@EcfNo", SqlDbType.VarChar).Value = ECFNo;
                cmd.Parameters.Add("@Status", SqlDbType.VarChar).Value = Status;
                cmd.Parameters.Add("@Action", SqlDbType.VarChar).Value = "InvoiceMIS_Vendorwise&PO/WOwise_Report";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new CentraldataModel();
                    objModel.ReceivedDate = row["Invoice received at CPPU on"].ToString();
                    objModel.PONo = row["PO/WONO"].ToString();
                    objModel.POWOBalance = row["PO/WO Balance"].ToString();
                    objModel.CBFNo = row["CBFNo"].ToString();
                    objModel.POWODate = row["PO/WO Date"].ToString();
                    objModel.POWOAmount = row["PO/WO Amount"].ToString();

                    objModel.EmployeeDepartment = row["Department"].ToString();
                    objModel.SupplierName = row["Vendor Name"].ToString();
                    objModel.InvoiceNo = row["Invoice No"].ToString();
                    objModel.InvoiceDate = row["Invoice Date"].ToString();
                    objModel.InvoiceAmount = row["Invoice Amount"].ToString();
                    objModel.EcfNo = row["ECF Number"].ToString();

                    objModel.ecfstatus = row["Status"].ToString();
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
        public IEnumerable<SelectCourier> ChangePettyReport(string ARFFrom, string ARFTo, string ARFAmount, string ARFNo, string RaiserCode, string RaiserName, string BranchCode)
        {
            List<SelectCourier> emp = new List<SelectCourier>();
            try
            {
                SelectCourier objModel;
                cmd = new SqlCommand("Pr_iem_trn_tCentralReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (ARFFrom != "")
                {
                    cmd.Parameters.Add("@EcfDate", SqlDbType.VarChar).Value = ARFFrom.ToString();
                }
                if (ARFTo != "")
                {
                    cmd.Parameters.Add("@InvoiceDate", SqlDbType.VarChar).Value = ARFTo.ToString();
                }
                cmd.Parameters.Add("@InvoiceAmount", SqlDbType.VarChar).Value = ARFAmount;
                cmd.Parameters.Add("@EcfNo", SqlDbType.VarChar).Value = ARFNo;
                cmd.Parameters.Add("@RaiserCode", SqlDbType.VarChar).Value = RaiserCode;
                cmd.Parameters.Add("@RaiserName", SqlDbType.VarChar).Value = RaiserName;
                cmd.Parameters.Add("@Employee_Code", SqlDbType.VarChar).Value = BranchCode;
                cmd.Parameters.Add("@Action", SqlDbType.VarChar).Value = "ChangePettyCash";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new SelectCourier();
                    objModel.ECF_Id = Convert.ToInt32(row["Ecf Gid"].ToString());
                    objModel.Raiser_Id = Convert.ToInt32(row["Raiser"].ToString());
                    objModel.ARF_Date_To = row["Ecf Date"].ToString();
                    objModel.ARF_No = row["Ecf No"].ToString();
                    objModel.ARF_Amount = row["Arf Amount"].ToString();
                    objModel.OutStandingAmount = row["Arf Exception"].ToString();
                    objModel.RaiserCode = row["Employee Code"].ToString();
                    objModel.RaiserName = row["Employee Name"].ToString();

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
        public IEnumerable<SelectCourier> ChangePettyReportPopup(string Ecfid)
        {
            List<SelectCourier> emp = new List<SelectCourier>();
            try
            {
                SelectCourier objModel;
                cmd = new SqlCommand("Pr_iem_trn_tCentralReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EcfGid", SqlDbType.Int).Value = Ecfid;
                cmd.Parameters.Add("@Action", SqlDbType.VarChar).Value = "ChangePettyCashPopup";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new SelectCourier();
                    objModel.ECF_Id = Convert.ToInt32(row["ecf_gid"].ToString());
                    objModel.ARF_Date_To = row["ecf_date"].ToString();
                    objModel.ARF_No = row["ecf_no"].ToString();
                    objModel.ARF_Amount = row["ecfarf_amount"].ToString();
                    objModel.OutStandingAmount = row["ecfarf_exception"].ToString();
                    objModel.RaiserCode = row["employee_code"].ToString();
                    objModel.RaiserName = row["employee_name"].ToString();
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
        public string AddPettyTransfer(SelectCourier cen)
        {
            try
            {
                cmd = new SqlCommand("Pr_iem_trn_tCentralReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RaiserCode", SqlDbType.Int).Value = cen.TransferFrom;
                cmd.Parameters.Add("@Employee_Code", SqlDbType.Int).Value = cen.Raiser_Id;
                cmd.Parameters.Add("@Action", SqlDbType.VarChar).Value = "Raiserid";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    EmployeeFromId = Convert.ToInt32(dt.Rows[0][0].ToString());
                    EmployeeToId = Convert.ToInt32(dt.Rows[0][1].ToString());

                    string[,] codes = new string[,]            
	                {
                         {"pettytransfer_ecf_gid",cen.ECF_Id.ToString()},
                         {"pettytransfer_date",cmnfun.convertoDateTimeString(cen.ARF_Date_From.ToString())},
                         {"pettytransfer_from",EmployeeFromId.ToString()},
                         {"pettytransfer_to",EmployeeToId.ToString()},
                         {"pettytransfer_remarks",cen.Remark.ToString()},
                         {"pettytransfer_insert_by",Convert.ToString (cmnfun.GetLoginUserGid())},
                         {"pettytransfer_insert_date","sysdatetime()"}
                    };
                    string tname = "iem_trn_tpettytransfer";
                    result = comm.InsertCommon(codes, tname);
                    if (result == "success")
                    {
                        result = "sub";
                    }

                    string[,] codes1 = new string[,]            
	                    {
                             {"ecf_raiser",EmployeeToId.ToString()},
                             {"ecf_employee_gid",EmployeeToId.ToString()},
                        
                        };
                    string[,] whrcol = new string[,]
	                     {
                              {"ecf_gid", cen.ECF_Id.ToString()}
                          
                         };
                    string tname1 = "iem_trn_tecf";
                    result = comm.UpdateCommon(codes1, whrcol, tname1);
                }
                else
                {
                    result = "invalidEmployee";
                }
            }
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
        public IEnumerable<SelectCourier> AutofilterEmployeeTo(string EmployeeName, string EmployeeCode)
        {
            List<SelectCourier> emp = new List<SelectCourier>();
            try
            {
                dt = new DataTable();
                SelectCourier objModel;
                cmd = new SqlCommand("Pr_iem_trn_tCentralReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Employee_Code", SqlDbType.VarChar).Value = EmployeeCode;
                cmd.Parameters.Add("@RaiserName", SqlDbType.VarChar).Value = EmployeeName;
                cmd.Parameters.Add("@Action", SqlDbType.VarChar).Value = "PettyCashChangeEmployee";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new SelectCourier();

                    objModel.RaiserCode = row["Employee"].ToString();
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
        public IEnumerable<SelectCourier> ChangePettyLogPopup(string Ecfid)
        {
            List<SelectCourier> emp = new List<SelectCourier>();
            try
            {
                SelectCourier objModel;
                cmd = new SqlCommand("Pr_iem_trn_tCentralReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EcfGid", SqlDbType.Int).Value = Ecfid;
                cmd.Parameters.Add("@Action", SqlDbType.VarChar).Value = "PettyCashChangeLog";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new SelectCourier();
                    objModel.ARF_Date_From = row["pettytransfer_date"].ToString();
                    objModel.TransferFrom = row["PettyFrom"].ToString();
                    objModel.RaiserName = row["PettyTo"].ToString();
                    objModel.Remark = row["pettytransfer_remarks"].ToString();

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
        public IEnumerable<SelectCourier> BranchCodeAutocomplete(string BranchCode)
        {
            List<SelectCourier> emp = new List<SelectCourier>();
            try
            {
                SelectCourier objModel;
                cmd = new SqlCommand("Pr_iem_trn_tCentralReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Employee_Code", SqlDbType.VarChar).Value = BranchCode;
                cmd.Parameters.Add("@Action", SqlDbType.VarChar).Value = "BranchCodeAutocomplete";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new SelectCourier();
                    objModel.BranchCode = row["branch_code"].ToString();
                    objModel.BranchName = row["branch_name"].ToString();
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
        public DataTable ExcelExportPettyCashChange(string ARFFrom, string ARFTo, string ARFAmount, string ARFNo, string RaiserCode, string RaiserName, string BranchCode)
        {

            try
            {

                cmd = new SqlCommand("Pr_iem_trn_tCentralReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (!string.IsNullOrEmpty(ARFFrom))
                {
                    cmd.Parameters.Add("@EcfDate", SqlDbType.VarChar).Value = ARFFrom.ToString();
                }
                if (!string.IsNullOrEmpty(ARFTo))
                {
                    cmd.Parameters.Add("@InvoiceDate", SqlDbType.VarChar).Value = ARFTo.ToString();
                }
                cmd.Parameters.Add("@InvoiceAmount", SqlDbType.VarChar).Value = ARFAmount;
                cmd.Parameters.Add("@EcfNo", SqlDbType.VarChar).Value = ARFNo;
                cmd.Parameters.Add("@RaiserCode", SqlDbType.VarChar).Value = RaiserCode;
                cmd.Parameters.Add("@RaiserName", SqlDbType.VarChar).Value = RaiserName;
                cmd.Parameters.Add("@Employee_Code", SqlDbType.VarChar).Value = BranchCode;
                cmd.Parameters.Add("@Action", SqlDbType.VarChar).Value = "ChangePettyCash";
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
    }
}