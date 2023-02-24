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
    public class PrintModel : PrintRepository
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
        printdatamodel objModel;
        System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
        int CentralId;
        string result;
        public PrintModel()
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
        public DataTable GetSupplieinvoice(string ecfid)
        {
            List<EOW_SupplierModelgrid> objExpense = new List<EOW_SupplierModelgrid>();
            try
            {
                DataTable dt = new DataTable();
                GetConnection();
                cmd = new SqlCommand("pr_eow_trn_Print", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ecfid", SqlDbType.VarChar).Value = ecfid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetInvoiceDetails";
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
        public DataTable GetprintExpensesupplier(string ecfid)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_eow_trn_Print", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ecfid", SqlDbType.VarChar).Value = ecfid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetExpenseDetailssupplier";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                //if (Convert.ToString(dt.Rows[i]["ecfdebitline_category_type"].ToString()) == "A")
                //{
                //    objModel.NatureofExpensesName = "";
                //    objModel.ExpenseCategoryName = Convert.ToString(dt.Rows[i]["assetcategory_name"].ToString());
                //    objModel.SubCategoryName = Convert.ToString(dt.Rows[i]["asset_description"].ToString());
                //}
                //else
                //{
                //    objModel.NatureofExpensesName = Convert.ToString(dt.Rows[i]["expnature_name"].ToString());
                //    objModel.ExpenseCategoryName = Convert.ToString(dt.Rows[i]["expcat_name"].ToString());
                //    objModel.SubCategoryName = Convert.ToString(dt.Rows[i]["expsubcat_name"].ToString());
                //}
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
        public DataTable GetEmployeeelist(string ecfgid)
        {
            DataTable objNatureofExpenses = new DataTable();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = ecfgid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetEmployeeelist";
                da = new SqlDataAdapter(cmd);
                da.Fill(objNatureofExpenses);
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
        public DataTable GetprintPayment(string ecfgid)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_eow_trn_Print", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ecfid", SqlDbType.VarChar).Value = ecfgid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetPaymenDetails";
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
        public DataTable SelectAdvanceecf(string Empcode)
        {
            try
            {
                DataTable dt = new DataTable();
                GetConnection();
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "SELECT_ADVANCE");
                cmd.Parameters.AddWithValue("@gid", Empcode);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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
        public DataTable SelectAdvanceprint(string Empcode)
        {
            try
            {
                DataTable dt = new DataTable();
                GetConnection();
                SqlCommand cmd = new SqlCommand("pr_eow_trn_Raisingarf", con);
                cmd.Parameters.AddWithValue("@ACTION", "SELECT_ADVANCEprint");
                cmd.Parameters.AddWithValue("@gid", Empcode);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //objModel = new EOW_arfraising();
                //objModel.ecfarf_ecf_gid = Convert.ToInt32(dt.Rows[i]["ecf_gid"].ToString());
                //objModel.supplierheader_suppliercode = Convert.ToString(dt.Rows[i]["supplierheader_suppliercode"]);
                //objModel.supplierheader_name = Convert.ToString(dt.Rows[i]["supplierheader_name"].ToString());
                //string po_no = Convert.ToString(dt.Rows[i]["ecfarf_po_no"].ToString());
                //if (po_no == "")
                //{
                //    po_no = "-";
                //}
                //objModel.ecfarf_po_no = po_no;
                //string cbf_no = Convert.ToString(dt.Rows[i]["ecfarf_cbf_no"].ToString());
                //if (cbf_no == "")
                //{
                //    cbf_no = "-";
                //}
                //objModel.ecfarf_cbf_no = cbf_no;
                //string ecf_date = Convert.ToString(dt.Rows[i]["invoice_date"].ToString());
                //if (ecf_date == "")
                //{
                //    ecf_date = "-";
                //}
                //objModel.ecf_date = ecf_date;
                //string ecf_no = Convert.ToString(dt.Rows[i]["invoice_no"].ToString());
                //if (ecf_no == "")
                //{
                //    ecf_no = "-";
                //}
                //objModel.ecf_no = ecf_no;
                //objNatureofExpenses.Add(objModel);
                //}
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
        public string togetPaymentAdjst(string id)
        {
            try
            {
                string objExpenseCategory = "";
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_iem_trn_print", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ECFGID", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "PaymentAdjst";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objExpenseCategory = Convert.ToString(dt.Rows[i]["ecfcreditline_pay_mode"].ToString());
                    if (objExpenseCategory == "")
                    {
                        objExpenseCategory = "No";
                    }
                    else
                    {
                        objExpenseCategory = "Yes";
                    }
                }

                return objExpenseCategory;
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
        public string togetpowonodetails(string id)
        {
            string objmainpo = "";
            try
            {
                string objExpenseCategory = "";
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_iem_trn_print", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ECFGID", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "powono";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    objExpenseCategory = Convert.ToString(dt.Rows[0]["poheader_pono"].ToString());
                    string[] objExpense = objExpenseCategory.Split(',');
                    for (int i = 0; i < objExpense.Length; i++)
                    {
                        if (objExpense[i].ToString() != "")
                        {
                            if (objmainpo == "")
                            {
                                objmainpo = objExpense[i].ToString();
                            }
                            else
                            {
                                objmainpo = objmainpo + "," + objExpense[i].ToString();
                            }
                        }
                    }
                }
                return objmainpo;
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
        public string togetoppcapdetails(string id)
        {
            try
            {
                string objExpenseCategory = "";
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_iem_trn_print", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ECFGID", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "OpexCapex";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows.Count == 1)
                    {
                        objExpenseCategory = Convert.ToString(dt.Rows[0]["OpexCapex"].ToString().ToUpper());
                        if (objExpenseCategory == "O")
                        {
                            objExpenseCategory = "Opex";
                        }
                        else if (objExpenseCategory == "C")
                        {
                            objExpenseCategory = "Capex";
                        }
                        else
                        {
                            objExpenseCategory = "";
                        }
                    }
                    else
                    {
                        objExpenseCategory = "Both [Opex/Capex]";
                    }
                }
                else
                {
                    objExpenseCategory = "";
                }

                return objExpenseCategory;
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
        public string toInvoiceType(string doctypeid, string doctype)
        {
            try
            {
                string objExpenseCategory = "-";
                if (doctypeid == "1")
                {
                    objExpenseCategory = "Non Travel";
                }
                else if (doctypeid == "2")
                {
                    objExpenseCategory = "LCF";
                }
                else if (doctypeid == "3")
                {
                    objExpenseCategory = "Travel";
                }
                else if (doctypeid == "4")
                {
                    if (doctype == "P")
                    {
                        objExpenseCategory = "PO";
                    }
                    else if (doctype == "W")
                    {
                        objExpenseCategory = "WO";
                    }
                    else if (doctype == "N" || doctype == "U")
                    {
                        objExpenseCategory = "Non PO";
                    }
                }
                else if (doctypeid == "5")
                {
                    objExpenseCategory = "DSA";
                }
                return objExpenseCategory;
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
        public printdatamodel SelectEmployeeSearch(string ECFgid, string ecftype)
        {
            try
            {
                dt = new DataTable();
                cmd = new SqlCommand("pr_iem_trn_print", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ECFGID", SqlDbType.VarChar).Value = ECFgid;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTEMPLOYEE";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new printdatamodel();
                    objModel.EmployeeId = row["employee_code"].ToString();
                    objModel.EmployeeName = row["employee_name"].ToString();
                    objModel.Title = row["employee_iem_designation"].ToString();
                    objModel.Ecf_Date = row["ecf_date"].ToString();
                    objModel.Department = row["employee_dept_name"].ToString();
                    objModel.LocationCode = row["LOCATIONCODE"].ToString();
                    objModel.Ecf_Gid = ECFgid.ToString();
                    objModel.Ecf_No = row["ecf_no"].ToString();
                    objModel.Ecf_Amount = cmnfun.GetINRAmount(row["ecf_amount"].ToString());

                    string splitamt = row["ecf_amount"].ToString();
                    //string[] splitamtwords = splitamt.Split('.');
                    //string paisa = "";
                    //if (splitamtwords[1].ToString() != "00")
                    //{
                    //    paisa = " AND " + cmnfun.ConvertNumbertoWords(Convert.ToInt32(splitamtwords[1].ToString())) + " PAISA ";
                    //}
                    //string main = cmnfun.ConvertNumbertoWords(Convert.ToInt32(splitamtwords[0].ToString()));

                    objModel.Ecf_Amountinword = cmnfun.ConvertDecimaltoWords(Convert.ToDecimal(splitamt.ToString()));
                    objModel.Ecf_dcsc = row["ecf_description"].ToString();

                    if (row["invoice_amort_flag"].ToString() == "Y")
                    {
                        objModel.Ecf_Amort = "Yes";
                    }
                    else
                    {
                        objModel.Ecf_Amort = "No";
                    }
                    if (row["ecf_currency_code"].ToString() == "INR")
                    {
                        objModel.Ecf_forex = "No";
                    }
                    else
                    {
                        objModel.Ecf_forex = "Yes";
                    }

                    if (row["ecf_po_type"].ToString() == "U")
                    {
                        objModel.Ecf_Utility = "Yes";
                    }
                    else
                    {
                        objModel.Ecf_Utility = "No";
                    }

                    objModel.Ecf_PaymentAdjst = togetPaymentAdjst(ECFgid);

                    objModel.Invoice_type = toInvoiceType(row["ecf_docsubtype_gid"].ToString(), row["ecf_po_type"].ToString());

                    if (objModel.Invoice_type == "PO" || objModel.Invoice_type == "WO")
                    {
                        objModel.Expense_type = togetoppcapdetails(ECFgid);
                        objModel.Invoice_powono = togetpowonodetails(ECFgid);
                        if (objModel.Expense_type == "Capex")
                        {
                            objModel.Header = " ASSET CAPITALISATION FORM ";
                        }
                        else if (objModel.Expense_type == "Both [Opex/Capex]")
                        {
                            objModel.Header = " EXPENSE CLAIM FORM / ASSET CAPITALISATION FORM ";
                        }
                        else
                        {
                            objModel.Header = " EXPENSE CLAIM FORM ";
                        }
                    }
                    else
                    {
                        objModel.Expense_type = "Claim";
                        objModel.Invoice_powono = "-";
                        objModel.Header = " EXPENSE CLAIM FORM ";
                    }
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
        public printdatamodel SelectPrintGeneral(string ECFgid, string ECFtype)
        {
            try
            {
                dt = new DataTable();
                cmd = new SqlCommand("pr_iem_trn_print", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ECFGID", SqlDbType.VarChar).Value = ECFgid;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTEMPLOYEE";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new printdatamodel();
                    objModel.EmployeeId = row["employee_code"].ToString();
                    objModel.EmployeeName = row["employee_name"].ToString();
                    objModel.Title = row["employee_iem_designation"].ToString();
                    objModel.Ecf_Date = row["ecf_date"].ToString();
                    objModel.Department = row["employee_dept_name"].ToString();
                    objModel.LocationCode = row["LOCATIONCODE"].ToString();
                    objModel.Invoice_powono = "-";
                    objModel.Ecf_Gid = ECFgid.ToString();
                    objModel.Ecf_No = row["ecf_no"].ToString();
                    objModel.Ecf_Amount = cmnfun.GetINRAmount(row["ecf_amount"].ToString());
                    objModel.ecfdocsubtype = row["ecf_docsubtype_gid"].ToString();
                    string splitamt = row["ecf_amount"].ToString();
                    //string[] splitamtwords = splitamt.Split('.');
                    //string paisa = "";
                    //if (splitamtwords[1].ToString() != "00")
                    //{
                    //    paisa = " AND " + cmnfun.ConvertNumbertoWords(Convert.ToInt32(splitamtwords[1].ToString())) + " PAISA ";
                    //}
                    //string main = cmnfun.ConvertNumbertoWords(Convert.ToInt32(splitamtwords[0].ToString()));
                    //objModel.Ecf_Amountinword = main + paisa;

                    objModel.Ecf_Amountinword = cmnfun.ConvertDecimaltoWords(Convert.ToDecimal(splitamt.ToString()));
                    objModel.Ecf_dcsc = row["ecf_description"].ToString();

                    if (row["invoice_amort_flag"].ToString() == "Y")
                    {
                        objModel.Ecf_Amort = "Yes";
                    }
                    else
                    {
                        objModel.Ecf_Amort = "No";
                    }
                    if (row["ecf_currency_code"].ToString() == "INR")
                    {
                        objModel.Ecf_forex = "No";
                    }
                    else
                    {
                        objModel.Ecf_forex = "Yes";
                    }

                    if (row["ecf_po_type"].ToString() == "U")
                    {
                        objModel.Ecf_Utility = "Yes";
                    }
                    else
                    {
                        objModel.Ecf_Utility = "No";
                    }
                    objModel.Ecf_PaymentAdjst = togetPaymentAdjst(ECFgid);
                    objModel.Invoice_type = toInvoiceType(row["ecf_docsubtype_gid"].ToString(), row["ecf_po_type"].ToString());

                    if (objModel.Invoice_type == "PO" || objModel.Invoice_type == "WO")
                    {
                        objModel.Expense_type = togetoppcapdetails(ECFgid);
                        objModel.Invoice_powono = togetpowonodetails(ECFgid);
                        if (objModel.Expense_type == "Capex")
                        {
                            objModel.Header = " ASSET CAPITALISATION FORM ";
                        }
                        else if (objModel.Expense_type == "Both [Opex/Capex]")
                        {
                            objModel.Header = " EXPENSE CLAIM FORM / ASSET CAPITALISATION FORM ";
                        }
                        else
                        {
                            objModel.Header = " EXPENSE CLAIM FORM ";
                        }
                    }
                    else
                    {
                        objModel.Expense_type = "Claim";
                        objModel.Invoice_powono = "-";
                        objModel.Header = " EXPENSE CLAIM FORM ";
                    }
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
        public printdatamodel SelectPrintArf(string ECFgid, string ECFtype)
        {

            try
            {
                dt = new DataTable();
                cmd = new SqlCommand("pr_iem_trn_print", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ECFGID", SqlDbType.VarChar).Value = ECFgid;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTEMPLOYEE";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {

                    objModel = new printdatamodel();
                    objModel.EmployeeId = row["employee_code"].ToString();
                    objModel.EmployeeName = row["employee_name"].ToString();
                    objModel.Title = row["employee_iem_designation"].ToString();
                    objModel.Ecf_Date = row["ecf_date"].ToString();
                    objModel.Department = row["employee_dept_name"].ToString();
                    objModel.LocationCode = row["LOCATIONCODE"].ToString();
                    objModel.Ecf_No = row["ecf_no"].ToString();
                    objModel.Ecf_Gid = ECFgid.ToString();
                    objModel.Ecf_Amount = cmnfun.GetINRAmount(row["ecf_amount"].ToString());
                    string splitamt = row["ecf_amount"].ToString();
                    //string[] splitamtwords = splitamt.Split('.');
                    //string paisa = "";
                    //if (splitamtwords[1].ToString() != "00")
                    //{
                    //    paisa = " AND " + cmnfun.ConvertNumbertoWords(Convert.ToInt32(splitamtwords[1].ToString())) + " PAISA ";
                    //}
                    ////decimal amountword = Math.Round(decimal.Parse(splitamtwords[0].ToString()));
                    //string main = cmnfun.ConvertNumbertoWords(Convert.ToInt32(splitamtwords[0].ToString()));
                    //objModel.Ecf_Amountinword = main + paisa;
                    objModel.Ecf_Amountinword = cmnfun.ConvertDecimaltoWords(Convert.ToDecimal(splitamt.ToString()));
                    objModel.Ecf_dcsc = row["ecf_description"].ToString();
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

        #region ToGet Declaration from database 07/06/2018
        public printdatamodel ToGetDeclaration(string ecfgid)
        {
            printdatamodel objModel1 = new printdatamodel();
            try
            {
                DataTable dtdec = new DataTable();
                cmd = new SqlCommand("pr_iem_trn_print", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ECFGID", ecfgid);
                cmd.Parameters.AddWithValue("@ACTION", "GetDeclaration");
                da = new SqlDataAdapter(cmd);
                da.Fill(dtdec);
                if (dtdec.Rows.Count > 0)
                {
                    objModel1.dclnotesub = dtdec.Rows[0]["declnote_onsubmission"].ToString();
                    objModel1.dclnoteapp = dtdec.Rows[0]["declnote_approval"].ToString();
                }
                return objModel1;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objModel1;

            }
            finally
            {
                con.Close();
            }
        }

        #endregion
    }
}