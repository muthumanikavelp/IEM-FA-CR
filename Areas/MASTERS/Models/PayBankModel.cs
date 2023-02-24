using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using IEM.Common;

namespace IEM.Areas.MASTERS.Models
{
    public class PayBankModel:PayBankRepository
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        ErrorLog objErrorLog = new ErrorLog();
        CommonIUD comm = new CommonIUD();
        CmnFunctions cmnfun = new CmnFunctions();
        string result;
        public PayBankModel()
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
        public IEnumerable<PayBankDataModel> SelectPayBankGrid()
        {
            try
            {
                List<PayBankDataModel> emp = new List<PayBankDataModel>();
                PayBankDataModel objModel;
                cmd = new SqlCommand("pr_iem_mst_tPayBank", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTPAYBANK";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new PayBankDataModel();
                    objModel.PayBankGid = Convert.ToInt16(row["paybank_gid"].ToString());
                    objModel.BankName = row["bank_name"].ToString();
                    objModel.PayBankAcNo = row["paybank_acc_no"].ToString();
                    objModel.PayBankIfscCode = row["paybank_ifsc_code"].ToString();
                    objModel.PayBankBranchName = row["paybank_branch_name"].ToString();
                    objModel.PayBankGlNo = row["paybank_gl_no"].ToString();
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
        public IEnumerable<PayBankDataModel> SelectBank()
        {
            try
            {
                List<PayBankDataModel> emp = new List<PayBankDataModel>();
                PayBankDataModel objModel;
                cmd = new SqlCommand("pr_iem_mst_tPayBank", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTBANK";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new PayBankDataModel();
                    objModel.BankGid = Convert.ToInt16(row["bank_gid"].ToString());
                    objModel.BankName = row["bank_name"].ToString();
                    
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
        public IEnumerable<PayBankDataModel> SelectEmployeeSearch(string EmployeeName, string EmployeeCode)
        {
            List<PayBankDataModel> emp = new List<PayBankDataModel>();
            try
            {
                PayBankDataModel objModel;
                cmd = new SqlCommand("pr_iem_mst_tPayBank", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@RAISERNAME", SqlDbType.VarChar).Value = EmployeeName;
                //cmd.Parameters.Add("@RAISERCODE", SqlDbType.VarChar).Value = EmployeeCode;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTGLNO";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new PayBankDataModel();
                    objModel.Payglid = Convert.ToInt16(row["gl_gid"].ToString());
                    objModel.PayBankGlNo = row["gl_no"].ToString();
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
        public string InsertPayBank(PayBankDataModel pay)
        {
            try
            {
                if(pay.PayBankStatus == "Active")
                {
                    pay.PayBankStatus = "Y";
                }
                else
                {
                    pay.PayBankStatus = "N";
                }
                string[] bankcode=pay.BankName.Split('-');
                string bankcodevalue = bankcode[0].ToString();
                List<PayBankDataModel> emp = new List<PayBankDataModel>();
                PayBankDataModel objModel;
                cmd = new SqlCommand("pr_iem_mst_tPayBank", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PAYBANKCODE", SqlDbType.VarChar).Value = bankcodevalue;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "CHECKDUPLICATE";
                result = (string)cmd.ExecuteScalar();
                if (result == "Duplicate record")
                {
                    result = "DuplicateBranchCode";
                    return result;
                }
                else
                {
                    string[,] codes = new string[,]            
	                {   
                         {"paybank_bank_gid",pay.BankGid.ToString()},
                         {"paybank_bank_name",pay.BankName.ToString()},
                         {"paybank_bank_code",bankcodevalue.ToString()},
                         {"paybank_acc_no",pay.PayBankAcNo.ToString()},
                         {"paybank_ifsc_code",pay.PayBankIfscCode.ToString()},
                         {"paybank_branch_name",pay.PayBankBranchName.ToString()},
                         {"paybank_gl_no",pay.PayBankGlNo.ToString()},
                         {"paybank_period_from",cmnfun.convertoDateTimeString(pay.PeriodFrom).ToString()},
                         {"paybank_period_to",cmnfun.convertoDateTimeString(pay.PeriodTo).ToString()},
                         {"paybank_active",pay.PayBankStatus.ToString()},
                         {"paybank_insert_date","sysdatetime()"},
                         {"paybank_insert_by",Convert.ToString (cmnfun.GetLoginUserGid())},
                    };
                    string tname = "iem_mst_tpaybank";
                    result = comm.InsertCommon(codes, tname);
                    if (result == "success")
                    {
                        result = "suc";
                    }

                }
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
        public string UpdatePayBank(PayBankDataModel pay)
        {
            try
            {
                if (pay.PayBankStatus == "Active")
                {
                    pay.PayBankStatus = "Y";
                }
                else
                {
                    pay.PayBankStatus = "N";
                }
                string[] bankcode = pay.BankName.Split('-');
                string bankcodevalue = bankcode[0].ToString();
                List<PayBankDataModel> emp = new List<PayBankDataModel>();
                PayBankDataModel objModel;
                cmd = new SqlCommand("pr_iem_mst_tPayBank", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PAY_GID", SqlDbType.VarChar).Value = pay.PayBankGid;
                cmd.Parameters.Add("@PAYBANKCODE", SqlDbType.VarChar).Value = bankcodevalue;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "CHECKDUPLICATEFOREDIT";
                result = (string)cmd.ExecuteScalar();
                if (result == "Duplicate record")
                {
                    result = "DuplicateBranchCode";
                    return result;
                }
                else
                {
                    string[,] codes = new string[,]            
	                {   
                         {"paybank_bank_gid",pay.BankGid.ToString()},
                         {"paybank_bank_name",pay.BankName.ToString()},
                         {"paybank_bank_code",bankcodevalue.ToString()},
                         {"paybank_acc_no",pay.PayBankAcNo.ToString()},
                         {"paybank_ifsc_code",pay.PayBankIfscCode.ToString()},
                         {"paybank_branch_name",pay.PayBankBranchName.ToString()},
                         {"paybank_gl_no",pay.PayBankGlNo.ToString()},
                         {"paybank_period_from",cmnfun.convertoDateTimeString(pay.PeriodFrom).ToString()},
                         {"paybank_period_to",cmnfun.convertoDateTimeString(pay.PeriodTo).ToString()},
                         {"paybank_active",pay.PayBankStatus.ToString()},
                         {"paybank_insert_date","sysdatetime()"},
                         {"paybank_insert_by",Convert.ToString (cmnfun.GetLoginUserGid())},
                    };
                    string[,] whrcol = new string[,]
	                 {
                          {"paybank_gid", pay.PayBankGid.ToString()},
                          {"paybank_isremoved", "N"}
                     };
                    string tname = "iem_mst_tpaybank";
                    result = comm.UpdateCommon(codes, whrcol, tname);
                    if (result == "Success")
                    {
                        result = "sub";
                    }

                }
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
        public string DeletePayBank(int id)
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
                 {"paybank_isremoved", "Y"}
	            
           };
                    string[,] whrcol = new string[,]
	        {
                {"paybank_gid",id.ToString ()}
            };
                    string tblname = "iem_mst_tpaybank";
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
        public PayBankDataModel SelectEditPayBank(int pay_gid)
        {
            List<PayBankDataModel> emp = new List<PayBankDataModel>();
            PayBankDataModel objModel = new PayBankDataModel();
            try
            {
                cmd = new SqlCommand("pr_iem_mst_tPayBank", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@PAY_GID", SqlDbType.VarChar).Value = pay_gid;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "EDITSELECT";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new PayBankDataModel();
                    objModel.PayBankGid = Convert.ToInt16(row["paybank_gid"].ToString());
                    objModel.BankName = row["bank_name"].ToString();
                    objModel.PayBankAcNo = row["paybank_acc_no"].ToString();
                    objModel.PayBankIfscCode = row["paybank_ifsc_code"].ToString();
                    objModel.PayBankBranchName = row["paybank_branch_name"].ToString();
                    objModel.PayBankGlNo = row["paybank_gl_no"].ToString();
                    objModel.PayBankStatus = row["paybank_active"].ToString();
                    objModel.PeriodFrom = row["paybank_period_from"].ToString();
                    objModel.PeriodTo = row["paybank_period_to"].ToString();
                    emp.Add(objModel);
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
            return objModel;
        }
    }
}