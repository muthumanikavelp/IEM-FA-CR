using IEM.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IEM.Areas.FLEXIBUY.Models
{
    public class PrintDataModel:PrintRepository
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();

        CmnFunctions objCmnFunctions = new CmnFunctions();
        CommonIUD objCommonIUD = new CommonIUD();
        ErrorLog objErrorLog = new ErrorLog();
       
        private void GetConnection()
        {
            try
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                    con.Open();
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

        }

        public IEnumerable<CbfPrintEntity> GetCBFSummary(string CBFNumber = null) 
        {
            List<CbfPrintEntity> objCBFSummary = new List<CbfPrintEntity>();
            try
            {
                CbfPrintEntity objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (CBFNumber == null || CBFNumber == "")
                {
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getcbfsummary";
                }
                else
                {
                    cmd.Parameters.Add("@cbfnumber", SqlDbType.VarChar).Value = CBFNumber;
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getcbfsummarybyid";
                }
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new CbfPrintEntity();
                    objModel.CBfGid = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["cbfheader_gid"].ToString()) ? "0" : dt.Rows[i]["cbfheader_gid"].ToString());
                    objModel.CBfMode = string.IsNullOrEmpty(dt.Rows[i]["cbfheader_mode"].ToString()) ? "" : dt.Rows[i]["cbfheader_mode"].ToString();
                    objModel.CBfNumber = string.IsNullOrEmpty(dt.Rows[i]["cbfheader_cbfno"].ToString()) ? "" : dt.Rows[i]["cbfheader_cbfno"].ToString();
                    objModel.CBfDate = string.IsNullOrEmpty(dt.Rows[i]["cbfheader_date"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["cbfheader_date"].ToString());
                    objModel.Department = string.IsNullOrEmpty(dt.Rows[i]["Dept"].ToString()) ? "" : dt.Rows[i]["Dept"].ToString();
                    objModel.BranchCode = string.IsNullOrEmpty(dt.Rows[i]["branch_code"].ToString()) ? "" : dt.Rows[i]["branch_code"].ToString();
                    objModel.BranchName = string.IsNullOrEmpty(dt.Rows[i]["branch_name"].ToString()) ? "" : dt.Rows[i]["branch_name"].ToString();
                    objModel.City = string.IsNullOrEmpty(dt.Rows[i]["city_name"].ToString()) ? "" : dt.Rows[i]["city_name"].ToString();
                    objModel.Justification = string.IsNullOrEmpty(dt.Rows[i]["cbfheader_remarks"].ToString()) ? "" : dt.Rows[i]["cbfheader_remarks"].ToString();
                    objModel.RaiserName = string.IsNullOrEmpty(dt.Rows[i]["Initiator"].ToString()) ? "" : dt.Rows[i]["Initiator"].ToString();
                    objModel.BudgetedFlag = string.IsNullOrEmpty(dt.Rows[i]["BudgetedFlag"].ToString()) ? "" : dt.Rows[i]["BudgetedFlag"].ToString();
                    objModel.CBFHeaderAmount = string.IsNullOrEmpty(dt.Rows[i]["cbfheader_cbfamt"].ToString()) ? "" : objCmnFunctions.GetINRAmount(dt.Rows[i]["cbfheader_cbfamt"].ToString());
                    objModel.CBFHeaderDevAmount = string.IsNullOrEmpty(dt.Rows[i]["cbfheader_Devi_amt"].ToString()) ? "" : objCmnFunctions.GetINRAmount(dt.Rows[i]["cbfheader_Devi_amt"].ToString());
                    objCBFSummary.Add(objModel);
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
            return objCBFSummary;
        }


        public CbfPrintEntity GetCBFData(string CBFNumber = null)
        {
            CbfPrintEntity objModel = new CbfPrintEntity(); ;
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@cbfnumber", SqlDbType.VarChar).Value = CBFNumber;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getcbfsummarybyid";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new CbfPrintEntity();
                    objModel.CBfGid = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["cbfheader_gid"].ToString()) ? "0" : dt.Rows[i]["cbfheader_gid"].ToString());
                    objModel.CBfMode = string.IsNullOrEmpty(dt.Rows[i]["cbfheader_mode"].ToString()) ? "" : dt.Rows[i]["cbfheader_mode"].ToString();
                    objModel.CBfNumber = string.IsNullOrEmpty(dt.Rows[i]["cbfheader_cbfno"].ToString()) ? "" : dt.Rows[i]["cbfheader_cbfno"].ToString();
                    objModel.CBfDate = string.IsNullOrEmpty(dt.Rows[i]["cbfheader_date"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["cbfheader_date"].ToString());
                    objModel.Department = string.IsNullOrEmpty(dt.Rows[i]["Dept"].ToString()) ? "" : dt.Rows[i]["Dept"].ToString();
                    objModel.BranchCode = string.IsNullOrEmpty(dt.Rows[i]["branch_code"].ToString()) ? "" : dt.Rows[i]["branch_code"].ToString();
                    objModel.BranchName = string.IsNullOrEmpty(dt.Rows[i]["branch_name"].ToString()) ? "" : dt.Rows[i]["branch_name"].ToString();
                    objModel.BranchType = string.IsNullOrEmpty(dt.Rows[i]["BranchType"].ToString()) ? "" : dt.Rows[i]["BranchType"].ToString();
                    objModel.City = string.IsNullOrEmpty(dt.Rows[i]["city_name"].ToString()) ? "" : dt.Rows[i]["city_name"].ToString();
                    objModel.CityClass = string.IsNullOrEmpty(dt.Rows[i]["cityclass_name"].ToString()) ? "" : dt.Rows[i]["cityclass_name"].ToString();
                    objModel.Justification = string.IsNullOrEmpty(dt.Rows[i]["cbfheader_remarks"].ToString()) ? "" : dt.Rows[i]["cbfheader_remarks"].ToString();
                    objModel.RaiserName = string.IsNullOrEmpty(dt.Rows[i]["Initiator"].ToString()) ? "" : dt.Rows[i]["Initiator"].ToString();
                    objModel.BudgetedFlag = string.IsNullOrEmpty(dt.Rows[i]["BudgetedFlag"].ToString()) ? "" : dt.Rows[i]["BudgetedFlag"].ToString();
                    objModel.CBFHeaderAmount = string.IsNullOrEmpty(dt.Rows[i]["cbfheader_cbfamt"].ToString()) ? "" : objCmnFunctions.GetINRAmount(dt.Rows[i]["cbfheader_cbfamt"].ToString());
                    objModel.CBFHeaderDevAmount = string.IsNullOrEmpty(dt.Rows[i]["cbfheader_Devi_amt"].ToString()) ? "" : objCmnFunctions.GetINRAmount(dt.Rows[i]["cbfheader_Devi_amt"].ToString());
                    objModel.CBFHeaderAmountInWords = string.IsNullOrEmpty(dt.Rows[i]["CBFHeaderAmountInWords"].ToString()) ? "" : dt.Rows[i]["CBFHeaderAmountInWords"].ToString();
                    if (objModel.CBFHeaderAmountInWords != "")
                    {
                        int amnt = Convert.ToInt32(objModel.CBFHeaderAmountInWords);
                        objModel.CBFHeaderAmountInWords = objCmnFunctions.ConvertNumbertoWords(amnt);
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
            }
            return objModel;
        }

        public IEnumerable<CbfPrintEntity> GetCBFDetailsList(string CBFNumber = null)
        {
            List<CbfPrintEntity> objCBFSummary = new List<CbfPrintEntity>();
            try
            {
                CbfPrintEntity objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@cbfnumber", SqlDbType.VarChar).Value = CBFNumber;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getcbfdetails";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new CbfPrintEntity();
                    objModel.ProductCode = string.IsNullOrEmpty(dt.Rows[i]["prodservice_code"].ToString()) ? "" : dt.Rows[i]["prodservice_code"].ToString();
                    objModel.ProductName = string.IsNullOrEmpty(dt.Rows[i]["prodservice_name"].ToString()) ? "" : dt.Rows[i]["prodservice_name"].ToString();
                    objModel.ProductDescription = string.IsNullOrEmpty(dt.Rows[i]["prodservice_description"].ToString()) ? "" : dt.Rows[i]["prodservice_description"].ToString();
                    objModel.UOM = string.IsNullOrEmpty(dt.Rows[i]["uom_name"].ToString()) ? "" : dt.Rows[i]["uom_name"].ToString();
                    objModel.Qty = string.IsNullOrEmpty(dt.Rows[i]["cbfdetails_qty"].ToString()) ? "" : dt.Rows[i]["cbfdetails_qty"].ToString();
                    objModel.UnitAmount = string.IsNullOrEmpty(dt.Rows[i]["cbfdetails_unitprice"].ToString()) ? "" : objCmnFunctions.GetINRAmount(dt.Rows[i]["cbfdetails_unitprice"].ToString());
                    objModel.TotalAmount = string.IsNullOrEmpty(dt.Rows[i]["cbfdetails_totalamt"].ToString()) ? "" : objCmnFunctions.GetINRAmount(dt.Rows[i]["cbfdetails_totalamt"].ToString());
                    objModel.ChartOfAccount = string.IsNullOrEmpty(dt.Rows[i]["cbfdetails_chartofacc"].ToString()) ? "" : dt.Rows[i]["cbfdetails_chartofacc"].ToString();
                    objModel.FCCC = string.IsNullOrEmpty(dt.Rows[i]["cbfdetails_fccc"].ToString()) ? "" : dt.Rows[i]["cbfdetails_fccc"].ToString();
                    objModel.BudgetLine = string.IsNullOrEmpty(dt.Rows[i]["cbfdetails_budgetline"].ToString()) ? "" : dt.Rows[i]["cbfdetails_budgetline"].ToString();
                    objCBFSummary.Add(objModel);
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
            return objCBFSummary;
        }

        public IEnumerable<ApprovalHistoryForPrint> GetApproverList(string CBFNumber = null,string ActionFor = null)
        {
            List<ApprovalHistoryForPrint> objCBFSummary = new List<ApprovalHistoryForPrint>();
            try
            {
                ApprovalHistoryForPrint objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@cbfnumber", SqlDbType.VarChar).Value = CBFNumber;
                cmd.Parameters.Add("@refname", SqlDbType.VarChar).Value = ActionFor;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getapprovalhistoryprint";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                  
                    if (dt.Rows[i]["queue_prev_gid"].ToString().Trim() == "0")
                    {
                        objModel = new ApprovalHistoryForPrint();
                        objModel.ApprovalStage = "Raiser";
                        objModel.ApproverName = string.IsNullOrEmpty(dt.Rows[i]["queue_from_name"].ToString()) ? "" : dt.Rows[i]["queue_from_name"].ToString();
                        objModel.ApprovalDate = string.IsNullOrEmpty(dt.Rows[i]["queue_date"].ToString()) ? "" : dt.Rows[i]["queue_date"].ToString();
                        objModel.Remarks = "";
                        objModel.EmpCode = string.IsNullOrEmpty(dt.Rows[i]["MakerID"].ToString()) ? "" : dt.Rows[i]["MakerID"].ToString();
                        objCBFSummary.Add(objModel);
                    }
                    objModel = new ApprovalHistoryForPrint();
                    objModel.ApproverName = string.IsNullOrEmpty(dt.Rows[i]["action_by_name"].ToString()) ? "" : dt.Rows[i]["action_by_name"].ToString();
                    objModel.ApprovalDate = string.IsNullOrEmpty(dt.Rows[i]["queue_action_date"].ToString()) ? "" : dt.Rows[i]["queue_action_date"].ToString();
                    objModel.ApprovalStage = string.IsNullOrEmpty(dt.Rows[i]["queue_to_name"].ToString()) ? "" : dt.Rows[i]["queue_to_name"].ToString();
                    objModel.Remarks = string.IsNullOrEmpty(dt.Rows[i]["queue_action_remark"].ToString()) ? "" : dt.Rows[i]["queue_action_remark"].ToString();
                    objModel.EmpCode = string.IsNullOrEmpty(dt.Rows[i]["ApproverID"].ToString()) ? "" : dt.Rows[i]["ApproverID"].ToString();
                    objCBFSummary.Add(objModel);
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
            return objCBFSummary;
        }

        public IEnumerable<POPrintEntity> GetPOSummary(string PONumber = null)  
        {
            List<POPrintEntity> objPOSummary = new List<POPrintEntity>(); 
            try
            {
                POPrintEntity objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (PONumber == null || PONumber == "")
                {
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getposummary";
                }
                else
                { 
                    cmd.Parameters.Add("@cbfnumber", SqlDbType.VarChar).Value = PONumber;
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getposummarybyid";
                }
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new POPrintEntity();
                    objModel.POGid = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["POGid"].ToString()) ? "0" : dt.Rows[i]["POGid"].ToString());
                    objModel.PONumber = string.IsNullOrEmpty(dt.Rows[i]["PONumber"].ToString()) ? "" : dt.Rows[i]["PONumber"].ToString();
                    objModel.PODate = string.IsNullOrEmpty(dt.Rows[i]["PODate"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["PODate"].ToString());
                    objModel.POAmount = string.IsNullOrEmpty(dt.Rows[i]["POAmount"].ToString()) ? "" : objCmnFunctions.GetINRAmount(dt.Rows[i]["POAmount"].ToString());
                    objModel.VendorName = string.IsNullOrEmpty(dt.Rows[i]["VendorName"].ToString()) ? "" : dt.Rows[i]["VendorName"].ToString();
                    objModel.RequestFor = string.IsNullOrEmpty(dt.Rows[i]["RequestFor"].ToString()) ? "" : dt.Rows[i]["RequestFor"].ToString();
                  
                    objPOSummary.Add(objModel);
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
            return objPOSummary;
        }
        public IEnumerable<WOPrintEntity> GetWOSummary(string WONumber = null)  
        {
            List<WOPrintEntity> objWOSummary = new List<WOPrintEntity>(); 
            try
            {
                WOPrintEntity objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (WONumber == null || WONumber == "")
                {
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getwosummary";
                }
                else
                {
                    cmd.Parameters.Add("@cbfnumber", SqlDbType.VarChar).Value = WONumber;
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getwosummarybyid";
                }
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new WOPrintEntity();
                    objModel.WOGid = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["WOGid"].ToString()) ? "0" : dt.Rows[i]["WOGid"].ToString());
                    objModel.WONumber = string.IsNullOrEmpty(dt.Rows[i]["WONumber"].ToString()) ? "" : dt.Rows[i]["WONumber"].ToString();
                    objModel.WODate = string.IsNullOrEmpty(dt.Rows[i]["WODate"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["WODate"].ToString());
                    objModel.WOAmount = string.IsNullOrEmpty(dt.Rows[i]["WOAmount"].ToString()) ? "" : objCmnFunctions.GetINRAmount(dt.Rows[i]["WOAmount"].ToString());
                    objModel.VendorName = string.IsNullOrEmpty(dt.Rows[i]["VendorName"].ToString()) ? "" : dt.Rows[i]["VendorName"].ToString();
                    objModel.RequestFor = string.IsNullOrEmpty(dt.Rows[i]["RequestFor"].ToString()) ? "" : dt.Rows[i]["RequestFor"].ToString();
                    objWOSummary.Add(objModel);
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
            return objWOSummary;
        }

        public POPrintEntity GetPOData(string PONumber = null) 
        {
            POPrintEntity objModel = new POPrintEntity(); ;
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@cbfnumber", SqlDbType.VarChar).Value = PONumber;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getposummarybyid";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new POPrintEntity();
                    objModel.POGid = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["POGid"].ToString()) ? "0" : dt.Rows[i]["POGid"].ToString());
                    objModel.PONumber = string.IsNullOrEmpty(dt.Rows[i]["PONumber"].ToString()) ? "" : dt.Rows[i]["PONumber"].ToString();
                    objModel.POAmount = string.IsNullOrEmpty(dt.Rows[i]["POAmount"].ToString()) ? "" : objCmnFunctions.GetINRAmount(dt.Rows[i]["POAmount"].ToString());
                    objModel.PODate = string.IsNullOrEmpty(dt.Rows[i]["PODate"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["PODate"].ToString());
                    objModel.RaiserName = string.IsNullOrEmpty(dt.Rows[i]["Raiser"].ToString()) ? "" : dt.Rows[i]["Raiser"].ToString();
                    objModel.VendorName = string.IsNullOrEmpty(dt.Rows[i]["VendorName"].ToString()) ? "" : dt.Rows[i]["VendorName"].ToString();
                    objModel.VendorNotes = string.IsNullOrEmpty(dt.Rows[i]["VendorNote"].ToString()) ? "" : dt.Rows[i]["VendorNote"].ToString();
                    objModel.VendorAddress = string.IsNullOrEmpty(dt.Rows[i]["VendorAddress"].ToString()) ? "" : dt.Rows[i]["VendorAddress"].ToString();
                    objModel.VendorCity = string.IsNullOrEmpty(dt.Rows[i]["VendorCity"].ToString()) ? "" : dt.Rows[i]["VendorCity"].ToString();
                    objModel.VendorState = string.IsNullOrEmpty(dt.Rows[i]["VendorState"].ToString()) ? "" : dt.Rows[i]["VendorState"].ToString();
                    objModel.VendorCountry = string.IsNullOrEmpty(dt.Rows[i]["VendorCountry"].ToString()) ? "" : dt.Rows[i]["VendorCountry"].ToString();
                    objModel.VendorPinCode = string.IsNullOrEmpty(dt.Rows[i]["VendorPinCode"].ToString()) ? "" : dt.Rows[i]["VendorPinCode"].ToString();
                    objModel.VendorPhone = string.IsNullOrEmpty(dt.Rows[i]["VendorPhone"].ToString()) ? "" : dt.Rows[i]["VendorPhone"].ToString();
                    objModel.VendorFax = string.IsNullOrEmpty(dt.Rows[i]["VendorFax"].ToString()) ? "" : dt.Rows[i]["VendorFax"].ToString();
                    objModel.RequestFor = string.IsNullOrEmpty(dt.Rows[i]["RequestFor"].ToString()) ? "" : dt.Rows[i]["RequestFor"].ToString();
                    objModel.POAmountInWords = string.IsNullOrEmpty(dt.Rows[i]["POAmountWithoutDecimal"].ToString()) ? "" : dt.Rows[i]["POAmountWithoutDecimal"].ToString();
                    objModel.TermAndCondn = string.IsNullOrEmpty(dt.Rows[i]["TermsAndCond"].ToString()) ? "" : dt.Rows[i]["TermsAndCond"].ToString();
                    objModel.AdditionalTermsAndCondition = string.IsNullOrEmpty(dt.Rows[i]["AdditionalTermsAndCondition"].ToString()) ? "" : dt.Rows[i]["AdditionalTermsAndCondition"].ToString();
                    objModel.deptname = string.IsNullOrEmpty(dt.Rows[i]["employee_dept_name"].ToString()) ? "" : dt.Rows[i]["employee_dept_name"].ToString();
                    if (objModel.POAmountInWords != "")
                    {
                        decimal amnt = Convert.ToDecimal(objModel.POAmount);
                        objModel.POAmountInWords = objCmnFunctions.ConvertDecimaltoWords(amnt);
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
            }
            return objModel;
        }

        public IEnumerable<POPrintEntity> GetPODetailsList(string PONumber = null)  
        {
            List<POPrintEntity> objPOSummary = new List<POPrintEntity>();
            try
            {
                POPrintEntity objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@cbfnumber", SqlDbType.VarChar).Value = PONumber;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getpodetails";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new POPrintEntity();
                    objModel.ProductCode = string.IsNullOrEmpty(dt.Rows[i]["ProductCode"].ToString()) ? "" : dt.Rows[i]["ProductCode"].ToString();
                    objModel.ProductName = string.IsNullOrEmpty(dt.Rows[i]["ProductName"].ToString()) ? "" : dt.Rows[i]["ProductName"].ToString();
                    objModel.ProductDescription = string.IsNullOrEmpty(dt.Rows[i]["ProductDescription"].ToString()) ? "" : dt.Rows[i]["ProductDescription"].ToString();
                    objModel.UOM = string.IsNullOrEmpty(dt.Rows[i]["UOM"].ToString()) ? "" : dt.Rows[i]["UOM"].ToString();
                    objModel.Qty = string.IsNullOrEmpty(dt.Rows[i]["Quantity"].ToString()) ? "" : dt.Rows[i]["Quantity"].ToString();
                    objModel.UnitAmount = string.IsNullOrEmpty(dt.Rows[i]["UnitPrice"].ToString()) ? "" :objCmnFunctions.GetINRAmount( dt.Rows[i]["UnitPrice"].ToString());
                    objModel.BaseAmount = string.IsNullOrEmpty(dt.Rows[i]["BaseAmount"].ToString()) ? "" : objCmnFunctions.GetINRAmount(dt.Rows[i]["BaseAmount"].ToString());
                    objModel.Discount = string.IsNullOrEmpty(dt.Rows[i]["Discount"].ToString()) ? "" : dt.Rows[i]["Discount"].ToString();
                    objModel.POCBFNumber = string.IsNullOrEmpty(dt.Rows[i]["CBFNumber"].ToString()) ? "" : dt.Rows[i]["CBFNumber"].ToString();
                    objModel.Tax = string.IsNullOrEmpty(dt.Rows[i]["Tax1"].ToString()) ? "" : dt.Rows[i]["Tax1"].ToString();
                    objModel.OtherCharge = string.IsNullOrEmpty(dt.Rows[i]["Tax2"].ToString()) ? "" : dt.Rows[i]["Tax2"].ToString();
                    objModel.fccc = string.IsNullOrEmpty(dt.Rows[i]["fccc"].ToString()) ? "" : dt.Rows[i]["fccc"].ToString();
                    objModel.PoVersion = string.IsNullOrEmpty(dt.Rows[i]["poversion"].ToString()) ? "0" : dt.Rows[i]["poversion"].ToString();
                    objModel.AmendmentDate = string.IsNullOrEmpty(dt.Rows[i]["amended"].ToString()) ? "" : dt.Rows[i]["amended"].ToString();
                    objModel.cbfmode = string.IsNullOrEmpty(dt.Rows[i]["cbfheader_mode"].ToString()) ? "" : dt.Rows[i]["cbfheader_mode"].ToString();
                    objModel.cbfbranch = string.IsNullOrEmpty(dt.Rows[i]["branch"].ToString()) ? "" : dt.Rows[i]["branch"].ToString();
                    objModel.parno = string.IsNullOrEmpty(dt.Rows[i]["parheader_refno"].ToString()) ? "" : dt.Rows[i]["parheader_refno"].ToString();
                    objModel.prno = string.IsNullOrEmpty(dt.Rows[i]["prheader_refno"].ToString()) ? "" : dt.Rows[i]["prheader_refno"].ToString();
                    objModel.gstvalue = string.IsNullOrEmpty(dt.Rows[i]["gstvalue"].ToString()) ? "" : dt.Rows[i]["gstvalue"].ToString();
                    objPOSummary.Add(objModel);
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
            return objPOSummary;
        }
        public WOPrintEntity GetWOData(string WONumber = null)  
        {
            WOPrintEntity objModel = new WOPrintEntity(); ;
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@cbfnumber", SqlDbType.VarChar).Value = WONumber;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getwosummarybyid";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new WOPrintEntity();
                    objModel.WOGid = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["WOGid"].ToString()) ? "0" : dt.Rows[i]["WOGid"].ToString());
                    objModel.WONumber = string.IsNullOrEmpty(dt.Rows[i]["WONumber"].ToString()) ? "" : dt.Rows[i]["WONumber"].ToString();
                    objModel.WOAmount = string.IsNullOrEmpty(dt.Rows[i]["WOAmount"].ToString()) ? "" : objCmnFunctions.GetINRAmount(dt.Rows[i]["WOAmount"].ToString());
                    objModel.WODate = string.IsNullOrEmpty(dt.Rows[i]["WODate"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["WODate"].ToString());
                    objModel.RaiserName = string.IsNullOrEmpty(dt.Rows[i]["Raiser"].ToString()) ? "" : dt.Rows[i]["Raiser"].ToString();
                    objModel.VendorName = string.IsNullOrEmpty(dt.Rows[i]["VendorName"].ToString()) ? "" : dt.Rows[i]["VendorName"].ToString();
                    objModel.VendorNotes = string.IsNullOrEmpty(dt.Rows[i]["VendorNote"].ToString()) ? "" : dt.Rows[i]["VendorNote"].ToString();
                    objModel.RequestFor = string.IsNullOrEmpty(dt.Rows[i]["RequestFor"].ToString()) ? "" : dt.Rows[i]["RequestFor"].ToString();
                    objModel.VendorAddress = string.IsNullOrEmpty(dt.Rows[i]["VendorAddress"].ToString()) ? "" : dt.Rows[i]["VendorAddress"].ToString();
                    objModel.VendorCity = string.IsNullOrEmpty(dt.Rows[i]["VendorCity"].ToString()) ? "" : dt.Rows[i]["VendorCity"].ToString();
                    objModel.VendorState = string.IsNullOrEmpty(dt.Rows[i]["VendorState"].ToString()) ? "" : dt.Rows[i]["VendorState"].ToString();
                    objModel.VendorCountry = string.IsNullOrEmpty(dt.Rows[i]["VendorCountry"].ToString()) ? "" : dt.Rows[i]["VendorCountry"].ToString();
                    objModel.VendorPinCode = string.IsNullOrEmpty(dt.Rows[i]["VendorPinCode"].ToString()) ? "" : dt.Rows[i]["VendorPinCode"].ToString();
                    objModel.VendorPhone = string.IsNullOrEmpty(dt.Rows[i]["VendorPhone"].ToString()) ? "" : dt.Rows[i]["VendorPhone"].ToString();
                    objModel.VendorFax = string.IsNullOrEmpty(dt.Rows[i]["VendorFax"].ToString()) ? "" : dt.Rows[i]["VendorFax"].ToString();
                    objModel.WOAmountInWords = string.IsNullOrEmpty(dt.Rows[i]["WOAmountWithoutDecimal"].ToString()) ? "" : dt.Rows[i]["WOAmountWithoutDecimal"].ToString();
                    objModel.TermAndCondn = string.IsNullOrEmpty(dt.Rows[i]["TermsAndCond"].ToString()) ? "" : dt.Rows[i]["TermsAndCond"].ToString();
                    objModel.AdditionalTermsAndCondition = string.IsNullOrEmpty(dt.Rows[i]["AdditionalTermsAndCondition"].ToString()) ? "" : dt.Rows[i]["AdditionalTermsAndCondition"].ToString();
                    objModel.deptname = string.IsNullOrEmpty(dt.Rows[i]["employee_dept_name"].ToString()) ? "" : dt.Rows[i]["employee_dept_name"].ToString();
                    objModel.WoVersion = string.IsNullOrEmpty(dt.Rows[i]["WoVersion"].ToString()) ? "0" : dt.Rows[i]["WoVersion"].ToString();
                    objModel.WoAmendmentDate = string.IsNullOrEmpty(dt.Rows[i]["Amendmentdate"].ToString()) ? "" : dt.Rows[i]["Amendmentdate"].ToString();

                    if (objModel.WOAmountInWords != "")
                    {
                        decimal amnt = Convert.ToDecimal(objModel.WOAmount);
                        objModel.WOAmountInWords = objCmnFunctions.ConvertDecimaltoWords(amnt);
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
            }
            return objModel;
        }

        public IEnumerable<WOPrintEntity> GetWODetailsList(string WONumber = null) 
        {
            List<WOPrintEntity> objWOSummary = new List<WOPrintEntity>(); 
            try
            {
                WOPrintEntity objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@cbfnumber", SqlDbType.VarChar).Value = WONumber;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getwodetails";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new WOPrintEntity();
                    objModel.ProductCode = string.IsNullOrEmpty(dt.Rows[i]["ProductCode"].ToString()) ? "" : dt.Rows[i]["ProductCode"].ToString();
                    objModel.ProductName = string.IsNullOrEmpty(dt.Rows[i]["ProductName"].ToString()) ? "" : dt.Rows[i]["ProductName"].ToString();
                    objModel.ProductDescription = string.IsNullOrEmpty(dt.Rows[i]["ProductDescription"].ToString()) ? "" : dt.Rows[i]["ProductDescription"].ToString();
                    objModel.ServiceMonth = string.IsNullOrEmpty(dt.Rows[i]["ServiceMonth"].ToString()) ? "" : dt.Rows[i]["ServiceMonth"].ToString();
                    objModel.Percentage = string.IsNullOrEmpty(dt.Rows[i]["Percentage"].ToString()) ? "" : dt.Rows[i]["Percentage"].ToString();
                    objModel.TotalAmount = string.IsNullOrEmpty(dt.Rows[i]["TotalAmount"].ToString()) ? "" : objCmnFunctions.GetINRAmount(dt.Rows[i]["TotalAmount"].ToString());
                    objModel.UnitPrice = string.IsNullOrEmpty(dt.Rows[i]["UnitPrice"].ToString()) ? "" : objCmnFunctions.GetINRAmount(dt.Rows[i]["UnitPrice"].ToString());
                    objModel.Qty = string.IsNullOrEmpty(dt.Rows[i]["Qty"].ToString()) ? "" : dt.Rows[i]["Qty"].ToString();
                    objModel.WOCBFNumber = string.IsNullOrEmpty(dt.Rows[i]["CBFNumber"].ToString()) ? "" : dt.Rows[i]["CBFNumber"].ToString();
                    objModel.fccc = string.IsNullOrEmpty(dt.Rows[i]["fccc"].ToString()) ? "" : dt.Rows[i]["fccc"].ToString();

                    objModel.cbfmode = string.IsNullOrEmpty(dt.Rows[i]["cbfheader_mode"].ToString()) ? "" : dt.Rows[i]["cbfheader_mode"].ToString();
                    objModel.cbfbranch = string.IsNullOrEmpty(dt.Rows[i]["branch"].ToString()) ? "" : dt.Rows[i]["branch"].ToString();
                    objModel.parno = string.IsNullOrEmpty(dt.Rows[i]["parheader_refno"].ToString()) ? "" : dt.Rows[i]["parheader_refno"].ToString();
                    objModel.prno = string.IsNullOrEmpty(dt.Rows[i]["prheader_refno"].ToString()) ? "" : dt.Rows[i]["prheader_refno"].ToString();
                    objModel.UOM = string.IsNullOrEmpty(dt.Rows[i]["UOM"].ToString()) ? "" : dt.Rows[i]["UOM"].ToString();
                    objWOSummary.Add(objModel);
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
            return objWOSummary;
        }
        public int GetBranchIsSingle(int pogid = 0)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = pogid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "checkissinglebranch";
                int result = Convert.ToInt32(cmd.ExecuteScalar());
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
        public IEnumerable<ShipmentDetails> GetShipmentDetails(int refgid = 0)
        {
            List<ShipmentDetails> objWOSummary = new List<ShipmentDetails>();
            try
            {
                ShipmentDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = refgid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getshipmentdetails";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ShipmentDetails();
                    objModel.BranchName = string.IsNullOrEmpty(dt.Rows[i]["BranchName"].ToString()) ? "" : dt.Rows[i]["BranchName"].ToString();
                    objModel.BranchAddress = string.IsNullOrEmpty(dt.Rows[i]["BranchAddress"].ToString()) ? "" : dt.Rows[i]["BranchAddress"].ToString();
                    objModel.ShipmentProdCode = string.IsNullOrEmpty(dt.Rows[i]["ProductCode"].ToString()) ? "" : dt.Rows[i]["ProductCode"].ToString();
                    objModel.ShipmentprodName = string.IsNullOrEmpty(dt.Rows[i]["ProductName"].ToString()) ? "" : dt.Rows[i]["ProductName"].ToString();
                    objModel.ShippedQty = string.IsNullOrEmpty(dt.Rows[i]["ShippedQty"].ToString()) ? "" : dt.Rows[i]["ShippedQty"].ToString();
                    objModel.ShipmentType = string.IsNullOrEmpty(dt.Rows[i]["ShipmentType"].ToString()) ? "" : dt.Rows[i]["ShipmentType"].ToString();
                    objWOSummary.Add(objModel);
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
            return objWOSummary;
        }
        public DataTable GetTC()
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getTC";
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
        public IEnumerable<ShipmentDetails> GetShipmentDetailsWO(int refgid = 0) 
        {
            List<ShipmentDetails> objWOSummary = new List<ShipmentDetails>();
            try
            {
                ShipmentDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = refgid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getshipmentdetailswo";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ShipmentDetails();
                    objModel.BranchName = string.IsNullOrEmpty(dt.Rows[i]["BranchName"].ToString()) ? "" : dt.Rows[i]["BranchName"].ToString();
                    objModel.BranchAddress = string.IsNullOrEmpty(dt.Rows[i]["BranchAddress"].ToString()) ? "" : dt.Rows[i]["BranchAddress"].ToString();
                    objModel.ShipmentProdCode = string.IsNullOrEmpty(dt.Rows[i]["ProductCode"].ToString()) ? "" : dt.Rows[i]["ProductCode"].ToString();
                    objModel.ShipmentprodName = string.IsNullOrEmpty(dt.Rows[i]["ProductName"].ToString()) ? "" : dt.Rows[i]["ProductName"].ToString();
                    objModel.ShippedQty = string.IsNullOrEmpty(dt.Rows[i]["ShippedQty"].ToString()) ? "" : dt.Rows[i]["ShippedQty"].ToString();
                    objModel.ShipmentType = string.IsNullOrEmpty(dt.Rows[i]["ShipmentType"].ToString()) ? "" : dt.Rows[i]["ShipmentType"].ToString();
                    objWOSummary.Add(objModel);
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
            return objWOSummary;
        }
        public DataSet GetDocSummary()
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getapproversdocs";
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
        public DataSet GetQueryScreenData() 
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getqueryresult";
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
        public DataSet GetVendorSelectionData() 
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "vendorselectionsummary";
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
        public DataSet VendorSelectionOpexData() 
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "vendorselectionopex";
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
        public DataSet VendorSelectionOpexDetailsSummary(string docgid = null, string doctype = null)
        {
            DataSet ds = new DataSet();
            try
            {
                DataTable dt = new DataTable();
                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(docgid)?"0":docgid);
                cmd.Parameters.Add("@refname", SqlDbType.VarChar).Value = string.IsNullOrEmpty(doctype) ? "" : doctype;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getprheaderdetailsopex";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                string obfnumber = objCmnFunctions.GetSequenceNoFb("OBF", "", dt.Rows[0]["OBFRequestFor"].ToString());
                string[,] codes2 = new string[,]
                {
                    {"cbfheader_cbfno", obfnumber},
                    {"cbfheader_date","sysdatetime()"},
                    {"cbfheader_mode",dt.Rows[0]["OBFMode"].ToString()},
                    {"cbfheader_cbfobf_flag", "O"},
                    {"cbfheader_prpar_gid", dt.Rows[0]["PRGid"].ToString()},
                    {"cbfheader_cbfamt",dt.Rows[0]["OBFAmount"].ToString()},
                    {"cbfheader_rasier_gid",Convert.ToString(HttpContext.Current.Session["employee_gid"])},
                    {"cbfheader_requestfor_gid",dt.Rows[0]["OBFRequestForGid"].ToString()},
                    {"cbfheader_isbudgeted","Y"},
                    {"cbfheader_status","5"},
                    {"cbfheader_closed","N"},
                    {"cbfheader_iscancelled","N"},
                    {"cbfheader_isremoved","N"}
                 
                };
                string tbname = "fb_trn_tcbfheader";
                string query_result = objCommonIUD.InsertCommon(codes2, tbname);

                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(docgid) ? "0" : docgid);
                cmd.Parameters.Add("@cbfnumber", SqlDbType.VarChar).Value = string.IsNullOrEmpty(obfnumber) ? "" : obfnumber;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getobfheadergid";
                string cbfheadergid = Convert.ToString(cmd.ExecuteScalar());
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string[,] codes3 = new string[,]
                        {
                            {"cbfheader_cbfobf_flag", "O"},
                            {"cbfdetails_cbfhead_gid",cbfheadergid},
                            {"cbfdetails_parprdesc",dt.Rows[i]["ProdServDesc"].ToString()},
                            {"cbfdetails_prodservgrp_gid",dt.Rows[i]["ProdGroup"].ToString()},
                            {"cbfdetails_prpardel_gid",dt.Rows[i]["PRDetailsGid"].ToString()},
                            {"cbfdetails_vendor_gid", "0"},
                            {"cbfdetails_prod_gid", dt.Rows[i]["ProdServGid"].ToString()},
                          //  {"cbfheader_cbfamt",dt.Rows[i]["OBFAmount"].ToString()},
                            {"cbfdetails_uom_gid",dt.Rows[i]["UOM"].ToString()},
                            {"cbfdetails_qty",dt.Rows[i]["PrDetQty"].ToString()},
                            {"cbfdetails_unitprice",dt.Rows[i]["Rate"].ToString()},
                            {"cbfdetails_totalamt",dt.Rows[i]["TotalAmnt"].ToString()},
                            {"cbfdetails_isremoved","N"},
                        };
                    string tbname2 = "fb_trn_tcbfdetails";
                    string query_result2 = objCommonIUD.InsertCommon(codes3, tbname2);
                }
                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(docgid) ? "0" : docgid);
                cmd.Parameters.Add("@cbfnumber", SqlDbType.VarChar).Value = string.IsNullOrEmpty(obfnumber) ? "" : obfnumber;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "vendorselectionopexfull";
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
        public DataSet GetCCAutoComplete(string searchtext = null)
        {
            DataSet ds = new DataSet(); 
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SearchText", SqlDbType.VarChar).Value = searchtext;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getccautocomplete";
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
        public DataSet GetOBFDetailsById(string docgid = null)
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(docgid) ? "0" : docgid);
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getobfdetailsbyid";
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
        public int UpdateOBFDetails(OBFDetails objOBFDetails)
        {
            DataSet ds = new DataSet();
            try
            {
                string[,] codes2 = new string[,]
                {
                    {"cbfdetails_fccc",string.IsNullOrEmpty(objOBFDetails.CC)?"":objOBFDetails.CC},
                    {"cbfdetails_budgetline",string.IsNullOrEmpty(objOBFDetails.BudgetLine.ToString())?"0":objOBFDetails.BudgetLine.ToString()},
                    {"cbfdetails_vendor_gid",string.IsNullOrEmpty(objOBFDetails.SupplierGid.ToString())?"0":objOBFDetails.SupplierGid.ToString()},
                };
                string[,] whrcol = new string[,]
	            {
                    {"cbfdetails_gid", string.IsNullOrEmpty(objOBFDetails.OBFDetailGid.ToString())?"0":objOBFDetails.OBFDetailGid.ToString()},
                    {"cbfdetails_cbfhead_gid", string.IsNullOrEmpty(objOBFDetails.OBFGid.ToString())?"0":objOBFDetails.OBFGid.ToString()}
                };

                string tbname = "fb_trn_tcbfdetails";
                string query_result = objCommonIUD.UpdateCommon(codes2, whrcol, tbname);

                string[,] codes1 = new string[,]
                {
                    {"cbfheader_remarks",string.IsNullOrEmpty(objOBFDetails.OBFJustification)?"":objOBFDetails.OBFJustification},
                    {"cbfheader_desc",string.IsNullOrEmpty(objOBFDetails.OBFDescription)?"":objOBFDetails.OBFDescription},
                    {"cbfheader_requesttype",string.IsNullOrEmpty(objOBFDetails.OBFITType)?"":objOBFDetails.OBFITType},
                };
                string[,] whrcol1 = new string[,]
	            {
                    {"cbfheader_gid", string.IsNullOrEmpty(objOBFDetails.OBFGid.ToString())?"0":objOBFDetails.OBFGid.ToString()}
                };

                string tbname1 = "fb_trn_tcbfheader";
                string query_result1 = objCommonIUD.UpdateCommon(codes1, whrcol1, tbname1);

               return objOBFDetails.OBFGid;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 1;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        public DataSet GetOBFDetailsAll(int docgid = 0) 
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = docgid;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getobfdetailsall";
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
        public int SubmitOBFDetails(OBFDetails objOBFDetails) 
        {
            DataSet ds = new DataSet();
            try
            {
                string[,] codes1 = new string[,]
                {
                    {"cbfheader_remarks",string.IsNullOrEmpty(objOBFDetails.OBFJustification)?"":objOBFDetails.OBFJustification},
                    {"cbfheader_desc",string.IsNullOrEmpty(objOBFDetails.OBFDescription.ToString())?"":objOBFDetails.OBFDescription.ToString()},
                    {"cbfheader_requesttype",string.IsNullOrEmpty(objOBFDetails.OBFITType)?"":objOBFDetails.OBFITType},
                };
                string[,] whrcol1 = new string[,]
	            {
                    {"cbfheader_gid", string.IsNullOrEmpty(objOBFDetails.OBFGid.ToString())?"0":objOBFDetails.OBFGid.ToString()}
                };

                string tbname1 = "fb_trn_tcbfheader";
                string query_result1 = objCommonIUD.UpdateCommon(codes1, whrcol1, tbname1);

                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(objOBFDetails.OBFGid);
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updatevendorgid";
                cmd.ExecuteNonQuery();

                return objOBFDetails.OBFGid;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 1;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        public DataSet VendorSelectionOpexDetailsSummary1(string docgid = null) 
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = docgid;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getobfdetailsallold";
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
        public DataSet VendorSelectionOpexDetailsSummaryOld(string docgid = null) 
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(docgid)?"0":docgid);
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "vendorselectionopexfullnew";
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
        public DataSet GetHeaderDetailsPAR(string docgid = null)
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(docgid) ? "0" : docgid);
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getheaderdetailspar";
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
        public DataSet GenerateOBFDetails(OBFDetails objOBFDetails)
        {
            DataSet ds = new DataSet();
            try
            {
                DataTable dt = new DataTable();
                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(objOBFDetails.PARGid) ? "0" : objOBFDetails.PARGid);
                cmd.Parameters.Add("@refname", SqlDbType.VarChar).Value = string.IsNullOrEmpty(objOBFDetails.OBFRequestFor) ? "" : objOBFDetails.OBFRequestFor;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getheaderdetailspar1";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                string obfnumber = objCmnFunctions.GetSequenceNoFb("OBF", "", objOBFDetails.OBFRequestFor);
                string[,] codes2 = new string[,]
                {
                    {"cbfheader_cbfno", obfnumber},
                    {"cbfheader_date","sysdatetime()"},
                    {"cbfheader_mode",dt.Rows[0]["OBFMode"].ToString()},
                    {"cbfheader_cbfobf_flag", "O"},
                    {"cbfheader_prpar_gid", dt.Rows[0]["PARGid"].ToString()},
                    {"cbfheader_cbfamt",dt.Rows[0]["OBFAmount"].ToString()},
                    {"cbfheader_rasier_gid",Convert.ToString(HttpContext.Current.Session["employee_gid"])},
                    {"cbfheader_requestfor_gid",dt.Rows[0]["OBFRequestForGid"].ToString()},
                    {"cbfheader_isbudgeted","Y"},
                    {"cbfheader_requesttype",string.IsNullOrEmpty(objOBFDetails.OBFITType)?"":objOBFDetails.OBFITType},
                    {"cbfheader_status","5"},
                    {"cbfheader_closed","N"},
                    {"cbfheader_iscancelled","N"},
                    {"cbfheader_isremoved","N"}
                 
                };
                string tbname = "fb_trn_tcbfheader";
                string query_result = objCommonIUD.InsertCommon(codes2, tbname);

                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(objOBFDetails.PARGid) ? "0" : objOBFDetails.PARGid);
                cmd.Parameters.Add("@cbfnumber", SqlDbType.VarChar).Value = string.IsNullOrEmpty(obfnumber) ? "" : obfnumber;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getobfheadergid";
                string cbfheadergid = Convert.ToString(cmd.ExecuteScalar());
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string[,] codes3 = new string[,]
                        {
                            {"cbfheader_cbfobf_flag", "O"},
                            {"cbfdetails_cbfhead_gid",cbfheadergid},
                            {"cbfdetails_parprdesc",dt.Rows[i]["ProdServDesc"].ToString()},
                            {"cbfdetails_prodservgrp_gid",dt.Rows[i]["ProdGroup"].ToString()},
                            {"cbfdetails_prpardel_gid",dt.Rows[i]["PARDetailsGid"].ToString()},
                            {"cbfdetails_vendor_gid", "0"},
                            {"cbfdetails_prod_gid", dt.Rows[i]["ProdServGid"].ToString()},
                          //  {"cbfheader_cbfamt",dt.Rows[i]["OBFAmount"].ToString()},
                            {"cbfdetails_uom_gid",dt.Rows[i]["UOM"].ToString()},
                            {"cbfdetails_qty",dt.Rows[i]["ParDetQty"].ToString()},
                            {"cbfdetails_unitprice",dt.Rows[i]["TotalAmnt"].ToString()},
                            {"cbfdetails_totalamt",dt.Rows[i]["TotalAmnt"].ToString()},
                            {"cbfdetails_isremoved","N"},
                        };
                    string tbname2 = "fb_trn_tcbfdetails";
                    string query_result2 = objCommonIUD.InsertCommon(codes3, tbname2);
                }
                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(objOBFDetails.PARGid) ? "0" : objOBFDetails.PARGid);
                cmd.Parameters.Add("@cbfnumber", SqlDbType.VarChar).Value = string.IsNullOrEmpty(obfnumber) ? "" : obfnumber;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "vendorselectionopexfullPAR";
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
        public DataSet VendorSelectionOpexDetailsSummaryOldPAR(string docgid = null) 
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(docgid) ? "0" : docgid);
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "vendorselectionopexfullnewPAR";
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
        public DataSet GetOBFDetailsByIdPAR(string docgid = null) 
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(docgid) ? "0" : docgid);
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getobfdetailsbyidpar";
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
        public int UpdateOBFDetailsPAR(OBFDetails objOBFDetails) 
        {
            DataSet ds = new DataSet();
            try
            {
                string[,] codes2 = new string[,]
                {
                    {"cbfdetails_parprdesc",string.IsNullOrEmpty(objOBFDetails.OBFProductDesc)?"":objOBFDetails.OBFProductDesc},
                    {"cbfdetails_prodservgrp_gid",string.IsNullOrEmpty(objOBFDetails.OBFProductGroupGid)?"0":objOBFDetails.OBFProductGroupGid},
                    {"cbfdetails_prod_gid",string.IsNullOrEmpty(objOBFDetails.OBFProductGid)?"0":objOBFDetails.OBFProductGid},
                    {"cbfdetails_fccc",string.IsNullOrEmpty(objOBFDetails.CC)?"":objOBFDetails.CC},
                    {"cbfdetails_budgetline",string.IsNullOrEmpty(objOBFDetails.BudgetLine.ToString())?"0":objOBFDetails.BudgetLine.ToString()},
                    {"cbfdetails_vendor_gid",string.IsNullOrEmpty(objOBFDetails.SupplierGid.ToString())?"0":objOBFDetails.SupplierGid.ToString()},
                };
                string[,] whrcol = new string[,]
	            {
                    {"cbfdetails_gid", string.IsNullOrEmpty(objOBFDetails.OBFDetailGid.ToString())?"0":objOBFDetails.OBFDetailGid.ToString()},
                    {"cbfdetails_cbfhead_gid", string.IsNullOrEmpty(objOBFDetails.OBFGid.ToString())?"0":objOBFDetails.OBFGid.ToString()}
                };

                string tbname = "fb_trn_tcbfdetails";
                string query_result = objCommonIUD.UpdateCommon(codes2, whrcol, tbname);

                string[,] codes1 = new string[,]
                {
                    {"cbfheader_remarks",string.IsNullOrEmpty(objOBFDetails.OBFJustification)?"":objOBFDetails.OBFJustification},
                    {"cbfheader_desc",string.IsNullOrEmpty(objOBFDetails.OBFDescription)?"":objOBFDetails.OBFDescription}
                };
                string[,] whrcol1 = new string[,]
	            {
                    {"cbfheader_gid", string.IsNullOrEmpty(objOBFDetails.OBFGid.ToString())?"0":objOBFDetails.OBFGid.ToString()}
                };

                string tbname1 = "fb_trn_tcbfheader";
                string query_result1 = objCommonIUD.UpdateCommon(codes1, whrcol1, tbname1);

                return objOBFDetails.OBFGid;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 1;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        public DataSet GetOBFDetailsAllPAR(int docgid = 0) 
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = docgid;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getobfdetailsallpar";
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
        public int SubmitOBFDetailsPAR(OBFDetails objOBFDetails) 
        {
            DataSet ds = new DataSet();
            try
            {
                string[,] codes1 = new string[,]
                {
                    {"cbfheader_remarks",string.IsNullOrEmpty(objOBFDetails.OBFJustification)?"":objOBFDetails.OBFJustification},
                    {"cbfheader_desc",string.IsNullOrEmpty(objOBFDetails.OBFDescription.ToString())?"":objOBFDetails.OBFDescription.ToString()}
                };
                string[,] whrcol1 = new string[,]
	            {
                    {"cbfheader_gid", string.IsNullOrEmpty(objOBFDetails.OBFGid.ToString())?"0":objOBFDetails.OBFGid.ToString()}
                };

                string tbname1 = "fb_trn_tcbfheader";
                string query_result1 = objCommonIUD.UpdateCommon(codes1, whrcol1, tbname1);

                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(objOBFDetails.OBFGid);
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updatevendorgid";
                cmd.ExecuteNonQuery();

                return objOBFDetails.OBFGid;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 1;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }
        public int CheckProductExists(int obfgid = 0)
        {
            try
            {
                int result = 0;
                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = obfgid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "checkproductdetails";
                result = Convert.ToInt32(cmd.ExecuteScalar());
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
        public DataSet GetPORptData(string cbfnumber = null, string ponumber = null, string podate = null, string poenddate = null) 
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                if (cbfnumber != null && cbfnumber != "null" && cbfnumber != "" && cbfnumber != "undefined")
                    cmd.Parameters.Add("@cbfnumber", SqlDbType.VarChar).Value = cbfnumber;
                if (ponumber != null && ponumber != "null" && ponumber != "" && ponumber != "undefined")
                    cmd.Parameters.Add("@ponumber", SqlDbType.VarChar).Value = ponumber;
                if (podate != null && podate != "null" && podate != "" && podate != "undefined")
                    cmd.Parameters.Add("@pofromdate", SqlDbType.VarChar).Value = podate;
                if (poenddate != null && poenddate != "null" && poenddate != "" && poenddate != "undefined")
                    cmd.Parameters.Add("@poenddate", SqlDbType.VarChar).Value = poenddate;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "poreportdata";
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
        public DataSet GetPORptDataFull(string cbfnumber = null, string ponumber = null, string podate = null, string poenddate = null) 
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_printing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                if(cbfnumber !=null && cbfnumber !="null" && cbfnumber !="" && cbfnumber !="undefined")
                    cmd.Parameters.Add("@cbfnumber", SqlDbType.VarChar).Value = cbfnumber;
                if (ponumber != null && ponumber != "null" && ponumber != "" && ponumber != "undefined")
                    cmd.Parameters.Add("@ponumber", SqlDbType.VarChar).Value = ponumber;
                if (podate != null && podate != "null" && podate != "" && podate != "undefined")
                    cmd.Parameters.Add("@pofromdate", SqlDbType.VarChar).Value = podate;
                if (poenddate != null && poenddate != "null" && poenddate != "" && poenddate != "undefined")
                    cmd.Parameters.Add("@poenddate", SqlDbType.VarChar).Value = poenddate;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "poreportdatafull";
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
        public DataSet PendingForConfirmationData()
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_PendingForGRNConfirmation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getgrnconfirmationpending";
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
    }
}