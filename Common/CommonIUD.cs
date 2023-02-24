using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;

namespace IEM.Common
{
    public class CommonIUD
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd, cmd1;
        SqlDataAdapter da;
        DataTable dt;
        ErrorLog objErrorLog = new ErrorLog();

        public void getconnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }

        public string GetCurrentDate()
        {
            try
            {
                getconnection();
                cmd = new SqlCommand("select sysdatetime()", con);
                cmd.CommandType = CommandType.Text;
                return Convert.ToDateTime(cmd.ExecuteScalar()).ToString("dd/MMM/yyyy");
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

        public string InsertCommon(string[,] colnamval, string tblname)
        {
            string col_value = string.Empty;
            string col_temp = string.Empty;
            string result = string.Empty;
            getconnection();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                for (int i = 0; i < colnamval.Length / 2; i++)
                {
                    if (colnamval[i, 0].ToString().Trim() != "1")
                    {
                        if (col_temp != string.Empty) col_temp += ",";
                        col_temp += colnamval[i, 0].ToString().Replace("'", "");

                        if (col_value != string.Empty) col_value += ",";

                        if (!string.IsNullOrEmpty(Convert.ToString(colnamval[i, 1])))
                        {
                            if (colnamval[i, 1].ToString().ToUpper() != "SYSDATETIME()")
                                //col_value += "'" + colnamval[i, 1].ToString() + "'";
                                col_value += "'" + colnamval[i, 1].ToString().Replace("'", "''") + "'";
                            else
                                col_value += "sysdatetime()";
                        }
                        else
                        {
                            col_value += "'" + colnamval[i, 1].ToString() + "'";
                        }
                    }

                }

                cmd = new SqlCommand("pr_insert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@columnList", col_temp);
                cmd.Parameters.AddWithValue("@tableSchema", "dbo");
                cmd.Parameters.AddWithValue("@tableName", tblname);
                cmd.Parameters.AddWithValue("@columnListvalue", col_value);
                tran.Commit();
                cmd.ExecuteNonQuery();
                result = "success";
            }
            catch (Exception ex)
            {
                result = ex.Message;
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                try
                {
                    tran.Rollback();
                }
                catch (SqlException exRollback)
                {
                    result = exRollback.Message;
                    objErrorLog.WriteErrorLog(exRollback.Message.ToString(), exRollback.ToString());
                }
            }

            finally
            {
                con.Close();
            }

            return result;
        }

        public string UpdateCommon(string[,] colnamval, string[,] WhrCondcolnamval, string tblname)
        {
            string col_namelist = string.Empty;
            string col_valuelist = string.Empty;
            string result = string.Empty;
            getconnection();
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                for (int i = 0; i < colnamval.Length / 2; i++)
                {
                    if (col_namelist != string.Empty) col_namelist += ",";

                    col_namelist += colnamval[i, 0].ToString().Replace("'", "");

                    if (!string.IsNullOrEmpty(Convert.ToString(colnamval[i, 1])))
                    {
                        if (colnamval[i, 1].ToString().ToUpper() != "SYSDATETIME()")
                            // col_namelist += "='" + colnamval[i, 1].ToString() + "'";
                            col_namelist += "='" + colnamval[i, 1].ToString().Replace("'", "''") + "'";
                        else
                            col_namelist += "=sysdatetime()";
                    }
                    else
                    {
                        if (colnamval[i, 0].ToString() == "ecf_delmat_amount")
                        {
                            if (colnamval[i, 1].ToString() == "")
                            {
                                col_namelist += "='" + 0 + "'";
                            }
                        }
                        else
                        {
                            col_namelist += "='" + colnamval[i, 1].ToString() + "'";
                        }
                    }
                }

                for (int i = 0; i < WhrCondcolnamval.Length / 2; i++)
                {
                    if (col_valuelist != string.Empty) col_valuelist += " AND ";

                    col_valuelist += WhrCondcolnamval[i, 0].ToString().Replace("'", "");
                    col_valuelist += "='" + WhrCondcolnamval[i, 1].ToString() + "'";
                }

                cmd = new SqlCommand("pr_update", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tableSchema", "");
                cmd.Parameters.AddWithValue("@tableName", tblname);
                cmd.Parameters.AddWithValue("@columnList", col_namelist);
                cmd.Parameters.AddWithValue("@columnListvalue", col_valuelist);
                tran.Commit();
                cmd.ExecuteNonQuery();
                result = "Success";
            }

            catch (Exception ex)
            {
                result = ex.Message;
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                try
                {
                    tran.Rollback();
                }
                catch (Exception exRollback)
                {
                    result = exRollback.Message;
                    objErrorLog.WriteErrorLog(exRollback.Message.ToString(), exRollback.ToString());
                }
                throw;
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public string DeleteCommon(string[,] colnamval, string[,] wherecondition, string tblname)
        {
            string col_namelist = string.Empty;
            string col_valuelist = string.Empty;
            string col = string.Empty, condition = string.Empty;
            string result;
            try
            {
                for (int i = 0; i < (colnamval.Length / 2); i++)
                {
                    if (col != string.Empty) col += ",";

                    col += colnamval[i, 0].ToString();
                    col += "='" + colnamval[i, 1].ToString() + "'";
                    col_namelist += colnamval[i, 0].ToString().Replace("'", "");
                    if (colnamval[i, 1].ToString().ToUpper() != "SYSDATETIME()")
                        col_namelist += "='" + colnamval[i, 1].ToString() + "'";
                    else
                        col_namelist += "=sysdatetime()";
                }

                for (int i = 0; i < (wherecondition.Length / 2); i++)
                {
                    if (condition != string.Empty) condition += " and ";

                    condition += wherecondition[i, 0].ToString();
                    condition += "='" + wherecondition[i, 1].ToString() + "'";
                }

                getconnection();

                cmd = new SqlCommand("pr_delete", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tableSchema", "dbo");
                cmd.Parameters.AddWithValue("@columnList", col);
                cmd.Parameters.AddWithValue("@tableName", tblname);
                cmd.Parameters.AddWithValue("@columnListvalue", condition);
                cmd.ExecuteNonQuery();
                result = "Sucess";
            }
            catch (Exception ex)
            {
                result = ex.Message;
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public string ProcedureCommon(string[,] pr_paraval, string pr_name)
        {
            string pr_parameter = string.Empty;
            string pr_value = string.Empty;
            string result = string.Empty;

            try
            {
                getconnection();
                cmd = new SqlCommand(pr_name, con);
                cmd.CommandType = CommandType.StoredProcedure;

                for (int i = 0; i < pr_paraval.Length / 2; i++)
                {
                    cmd.Parameters.AddWithValue(pr_paraval[i, 0], pr_paraval[i, 1]);
                }

                return cmd.ExecuteScalar().ToString();
            }
            catch (Exception ex)
            {
                result = ex.Message;
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            finally
            {
                con.Close();
            }

            return result;
        }
        public string SupplierLocked(string supp_gid, string logid)
        {
            string locresult = string.Empty;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_iem_supplierloged", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@supp_gid", supp_gid);
                cmd.Parameters.AddWithValue("@logid", logid);
                da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    locresult = "This Supplier Locked by :" + ds.Tables[0].Rows[0]["employee_code"].ToString() + "-" + ds.Tables[0].Rows[0]["employee_name"].ToString();
                }
                else
                {
                    string[,] lupcon = new string[,]
                    {
                        {"supplierheader_logedby",logid.ToString()},
                        {"supplierheader_logeddate",DateTime.Now.ToString("dd/MMM/yyyy")},
                        {"supplierheader_logedstatus","Y"}
                 
                    };

                    string[,] wlupcon = new string[,]
                    {
                        {"supplierheader_gid",supp_gid.ToString()}
                    };

                    string tblupname = "asms_tmp_tsupplierheader";

                    locresult = UpdateCommon(lupcon, wlupcon, tblupname);
                }


            }
            catch (Exception ex)
            {

            }
            return locresult;
        }

        public string InsertCommon_FA(string[,] colnamval, string tblname)
        {
            string col_value = string.Empty;
            string col_temp = string.Empty;
            string result = string.Empty;
            //getconnection();
           // SqlTransaction tran = con.BeginTransaction();
            try
            {
                for (int i = 0; i < colnamval.Length / 2; i++)
                {
                    if (colnamval[i, 0].ToString().Trim() != "1")
                    {
                        if (col_temp != string.Empty) col_temp += ",";
                        col_temp += colnamval[i, 0].ToString().Replace("'", "");

                        if (col_value != string.Empty) col_value += ",";

                        if (colnamval[i, 1].ToString().ToUpper() != "SYSDATETIME()")
                            col_value += "'" + colnamval[i, 1].ToString() + "'";
                        else
                            col_value += "sysdatetime()";
                    }

                }

                cmd = new SqlCommand("pr_insert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@columnList", col_temp);
                cmd.Parameters.AddWithValue("@tableSchema", "dbo");
                cmd.Parameters.AddWithValue("@tableName", tblname);
                cmd.Parameters.AddWithValue("@columnListvalue", col_value);
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
                result = "success";
            }
            catch (Exception ex)
            {
                result = ex.Message;
                //try
                //{
                //    tran.Rollback();
                //}
                //catch (SqlException exRollback)
                //{
                //    result = exRollback.Message;
                //}
                throw;
            }

            return result;
        }

        public string UpdateCommon_FA(string[,] colnamval, string[,] WhrCondcolnamval, string tblname)
        {
            string col_namelist = string.Empty;
            string col_valuelist = string.Empty;
            string result = string.Empty;
            //getconnection();
           // SqlTransaction tran = con.BeginTransaction();
            try
            {
                for (int i = 0; i < colnamval.Length / 2; i++)
                {
                    if (col_namelist != string.Empty) col_namelist += ",";

                    col_namelist += colnamval[i, 0].ToString().Replace("'", "");

                    if (colnamval[i, 1].ToString().ToUpper() != "SYSDATETIME()")
                        col_namelist += "='" + colnamval[i, 1].ToString() + "'";
                    else
                        col_namelist += "=sysdatetime()";
                }

                for (int i = 0; i < WhrCondcolnamval.Length / 2; i++)
                {
                    if (col_valuelist != string.Empty) col_valuelist += " AND ";

                    col_valuelist += WhrCondcolnamval[i, 0].ToString().Replace("'", "");
                    col_valuelist += "='" + WhrCondcolnamval[i, 1].ToString() + "'";
                }

                cmd = new SqlCommand("pr_update", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@tableSchema", "");
                cmd.Parameters.AddWithValue("@tableName", tblname);
                cmd.Parameters.AddWithValue("@columnList", col_namelist);
                cmd.Parameters.AddWithValue("@columnListvalue", col_valuelist);
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
                result = "Success";
            }

            catch (Exception ex)
            {
                result = ex.Message;
                //try
                //{
                //    tran.Rollback();
                //}
                //catch (Exception exRollback)
                //{
                //    result = exRollback.Message;
                //}
                throw;
            }
            //finally
            //{
            //    con.Close();
            //}
            return result;
        }


    }
}