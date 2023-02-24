using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
//using IEM.Helper;

namespace IEM.Areas.MASTERS.Models
{
    public class TransferModel:TransferRepository
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        //proLib nprop = new proLib();
        Common.CmnFunctions cmnfun = new Common.CmnFunctions();
        string month;
        string result;
        public TransferModel()
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
        public IEnumerable<TransferDataModel> SelectEmployee()
        {
            try
            {
                List<TransferDataModel> trn = new List<TransferDataModel>();
                TransferDataModel objModel;
                cmd = new SqlCommand("pr_iem_mst_transferowner", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTEMPLOYEE";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
              
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new TransferDataModel();
                    objModel.EmployeeId = Convert.ToInt32(row["employee_gid"].ToString());
                    objModel.EmployeeName = row["employee_name"].ToString();
                    trn.Add(objModel);
                }
                return trn;
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

        public DataTable SelectOwnership(string modulename)
        {
            try
            {
                cmd = new SqlCommand("pr_iem_mst_transferowner", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@MODULENAMEBY", SqlDbType.VarChar).Value = modulename;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTOWNERSHIP";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
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

        public IEnumerable<TransferDataModel> SelectModule()
        {
            try
            {
                List<TransferDataModel> emp = new List<TransferDataModel>();
                TransferDataModel objModel;
                cmd = new SqlCommand("pr_iem_mst_transferowner", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTMODULE";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
              
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new TransferDataModel();
                    objModel.ModuleId = Convert.ToInt32(row["module_gid"].ToString());
                    objModel.ModuleName = row["module_name"].ToString();
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
        public string InsertOwner(TransferDataModel trn)
        {
            try
            {
                string[] mdate = trn.TransferDate.Split('-');
                string tdate = mdate[1].ToString() + "-" + mdate[0].ToString() + "-" + mdate[2].ToString();
                cmd = new SqlCommand("pr_iem_mst_transferowner", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EMPLOYEEFROM", SqlDbType.Int).Value = trn.TranferFrom;
                cmd.Parameters.Add("@EMPLOYEETO", SqlDbType.Int).Value = trn.TransferTo;
                cmd.Parameters.Add("@MODULEID", SqlDbType.Int).Value = trn.ModuleId;
                cmd.Parameters.Add("@REMARK", SqlDbType.VarChar).Value = trn.Remarks;
                cmd.Parameters.Add("@TRANSFERDATEFROM", SqlDbType.VarChar).Value = tdate;
                cmd.Parameters.Add("@INSERTEDBY", SqlDbType.Int).Value =Convert.ToInt32(cmnfun.GetLoginUserGid());
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "INSERTOWNER";
                result = cmd.ExecuteNonQuery().ToString();
                if(result=="1")
                {
                    result = "Successfully Saved";
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
        public string InsertOwnerDetails(string[] SelectedOwner)
        {
            try
            {
                con.Open();
                foreach (string id in SelectedOwner)
                {
                    string strID = "";
                    if(id !=null)
                    {
                        strID = id;
                    }
                    cmd = new SqlCommand("pr_iem_mst_transferowner", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    if(strID!="" && strID!=null)
                    {
                        cmd.Parameters.Add("@OWNERID", SqlDbType.Int).Value = Convert.ToInt32(strID);
                    }
                    else
                    {
                        cmd.Parameters.Add("@OWNERID", SqlDbType.Int).Value = 0;
                    }
                    cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "INSERTOWNERDET";
                    result = cmd.ExecuteNonQuery().ToString();
                }
                if (result == "1")
                {
                    result = "Successfully Saved";
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


        public IEnumerable<TransferDataModel> Select()
        {
            try
            {
                List<TransferDataModel> trn = new List<TransferDataModel>();
                TransferDataModel objModel;
                cmd = new SqlCommand("pr_iem_mst_transferowner", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECT";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new TransferDataModel();
                    objModel.TranferId = Convert.ToInt32(row["transferown_gid"].ToString());
                    objModel.TranferFrom = row["employee_name"].ToString();
                    objModel.TransferTo = row["transferown_to"].ToString();
                    objModel.ModuleName = row["module_name"].ToString();
                    objModel.TransferDate = row["transferown_insert_date"].ToString();
                    trn.Add(objModel);
                }
                return trn;
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


        public IEnumerable<TransferDataModel> Search(string TransferDateFrom = null, string TransferDateTo = null, string TransferEmployeeId = null, string TransferEmployeeTOId = null, string TransferModuleId = null)
        {
            try
            {
                
                List<TransferDataModel> trn = new List<TransferDataModel>();
                TransferDataModel objModel;
                cmd = new SqlCommand("pr_iem_mst_transferowner", con);
                cmd.Parameters.AddWithValue("@ACTION", "SEARCH");
                cmd.Parameters.AddWithValue("@EMPLOYEEFROM", TransferEmployeeId);
                cmd.Parameters.AddWithValue("@EMPLOYEETO", TransferEmployeeTOId);
                cmd.Parameters.AddWithValue("@MODULEID", TransferModuleId);
                //cmd.Parameters.AddWithValue("@TRANSFERDATEFROM", nprop.ConvertDateMaster(TransferDateFrom));
                //cmd.Parameters.AddWithValue("@TRANSFERDATETO", nprop.ConvertDateMaster(TransferDateTo));
                cmd.CommandType = CommandType.StoredProcedure;
              
                //    cmd.Parameters.Add("@EMPLOYEEFROM", SqlDbType.Int).Value = Convert.ToInt16(TransferEmployeeId);
               
                //    cmd.Parameters.Add("@EMPLOYEETO", SqlDbType.Int).Value = Convert.ToInt16(TransferEmployeeTOId);
               
                //    cmd.Parameters.Add("@MODULEID", SqlDbType.Int).Value = Convert.ToInt16(TransferModuleId);
                
                //cmd.Parameters.Add("@TRANSFERDATEFROM", SqlDbType.VarChar).Value = TransferDateFrom;
                //cmd.Parameters.Add("@TRANSFERDATETO", SqlDbType.VarChar).Value = TransferDateTo;
                //cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SEARCH";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new TransferDataModel();
                    objModel.TranferId = Convert.ToInt32(row["transferown_gid"].ToString());
                    objModel.TranferFrom = row["employee_name"].ToString();
                    objModel.TransferTo = row["transferown_to"].ToString();
                    objModel.ModuleName = row["module_name"].ToString();
                    objModel.TransferDate = row["transferown_insert_date"].ToString();
                    trn.Add(objModel);
                }
                return trn;
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
        public IEnumerable<TransferDataModel> SelectedId(int id)
        {
            try
            {
                List<TransferDataModel> trn = new List<TransferDataModel>();
                TransferDataModel objModel;
                cmd = new SqlCommand("pr_iem_mst_transferowner", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TRANSFERID", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTEDID";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new TransferDataModel();
                    objModel.TranferFrom = row["transferown_from"].ToString();
                    objModel.TransferTo = row["transferown_to"].ToString();
                    objModel.ModuleId =   Convert.ToInt16(row["transferown_module"].ToString());
                    objModel.TransferDate = row["transferown_insert_date"].ToString();
                    objModel.Remarks = row["transferown_remark"].ToString();
                    trn.Add(objModel);
                }
                return trn;
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
        public DataTable Selectedownerid(int id)
        {
            try
            {
                cmd = new SqlCommand("pr_iem_mst_transferowner", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TRANSFERID", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTEDOWNERSHIP";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
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
        public string EditOwner(TransferDataModel trn)
        {
            try
            {
                string[] mdate = trn.TransferDate.Split('-');
                string tdate = mdate[1].ToString() + "-" + mdate[0].ToString() + "-" + mdate[2].ToString();
                cmd = new SqlCommand("pr_iem_mst_transferowner", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EMPLOYEEFROM", SqlDbType.Int).Value = trn.TranferFrom;
                cmd.Parameters.Add("@EMPLOYEETO", SqlDbType.Int).Value = trn.TransferTo;
                cmd.Parameters.Add("@MODULEID", SqlDbType.Int).Value = trn.ModuleId;
                cmd.Parameters.Add("@REMARK", SqlDbType.VarChar).Value = trn.Remarks;
                cmd.Parameters.Add("@TRANSFERID", SqlDbType.VarChar).Value = trn.TranferId;
                cmd.Parameters.Add("@TRANSFERDATEFROM", SqlDbType.VarChar).Value = tdate;
                cmd.Parameters.Add("@INSERTEDBY", SqlDbType.Int).Value = Convert.ToInt32(cmnfun.GetLoginUserGid());
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "UPDATEOWNER";
                result = cmd.ExecuteNonQuery().ToString();
                if (result == "1")
                {
                    result = "Successfully Saved";
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
        public string EditOwnerDetails(string[] SelectedOwner,int transid)
        {
            try
            {
                con.Open();
                foreach (string id in SelectedOwner)
                {
                    string strID = "";
                    if (id != null)
                    {
                        strID = id;
                    }
                    cmd = new SqlCommand("pr_iem_mst_transferowner", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@TRANSFERID", SqlDbType.Int).Value = transid;
                    if (id != "" && id != null)
                    {
                        cmd.Parameters.Add("@OWNERID", SqlDbType.Int).Value = Convert.ToInt32(strID);
                    }
                    else
                    {
                        cmd.Parameters.Add("@OWNERID", SqlDbType.Int).Value = 0;
                    }

                    cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "INSERTOWNERDET";
                    result = cmd.ExecuteNonQuery().ToString();
                }
                if (result == "1")
                {
                    result = "Successfully Saved";
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

        public string Delete(int tranid)
        {
            try
            {
                con.Open();
                cmd = new SqlCommand("pr_iem_mst_transferowner", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TRANSFERID", SqlDbType.VarChar).Value = tranid;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "DELETEOWNER";
                result = cmd.ExecuteNonQuery().ToString();
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


        public string DeleteOwner(int tranid)
        {
            try
            {
                
                cmd = new SqlCommand("pr_iem_mst_transferowner", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TRANSFERID", SqlDbType.VarChar).Value = tranid;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "OWNERDELETE";
                result = cmd.ExecuteNonQuery().ToString();
                if (result == "2")
                {
                    result = "Successfully Saved";
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

        //added by Dhasarathan 14-11-2016 
        public DataTable SelectEmployeeQueue(int empfromid, int emptoid, int transfertype, int refflag, int subDocType, int ecfstatus)
        {
            try
            {
                DataTable dt = new DataTable();



                cmd = new SqlCommand("ecf_iem_mst_Empowner", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EMPLOYEEFROM", SqlDbType.Int).Value = empfromid;
                cmd.Parameters.Add("@EMPLOYEETO", SqlDbType.Int).Value = emptoid;
                cmd.Parameters.Add("@subDocType", SqlDbType.Int).Value = subDocType;
                cmd.Parameters.Add("@transfertype", SqlDbType.Int).Value = transfertype;
                cmd.Parameters.Add("@queuerefflag", SqlDbType.Int).Value = refflag;
                cmd.Parameters.Add("@status", SqlDbType.Int).Value = ecfstatus;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SelectOwnerShip";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
            catch (Exception)
            {

                throw;
            }


        }

        public string TransferOwnerShip(int empfromid, int emptoid, int transfertype, int refflag, int refgid, int queuegid)
        {
            try
            {
                DataTable dt = new DataTable();



                cmd = new SqlCommand("iem_mst_ownershipUpdate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@EMPLOYEEFROM", SqlDbType.Int).Value = empfromid;
                cmd.Parameters.Add("@EMPLOYEETO", SqlDbType.Int).Value = emptoid;
                cmd.Parameters.Add("@transfertype", SqlDbType.Int).Value = transfertype;
                cmd.Parameters.Add("@refgid", SqlDbType.Int).Value = refgid;
                cmd.Parameters.Add("@queuegid", SqlDbType.Int).Value = queuegid;
                cmd.Parameters.Add("@queuerefflag", SqlDbType.Int).Value = refflag;
                cmd.Parameters.Add("@INSERTEDBY", SqlDbType.Int).Value = Convert.ToInt16(cmnfun.GetLoginUserGid());
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "TransferOwnerShip";
                result = cmd.ExecuteNonQuery().ToString();
                if (result == "2")
                {
                    result = "Successfully Saved";
                }
                return result;
            }
            catch (Exception)
            {

                throw;
            }


        }

        public DataTable SelectEmployeeDetails(string txt)
        {
            try
            {
                DataTable dt = new DataTable();
                cmd = new SqlCommand("ecf_iem_mst_Empowner", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@txt", SqlDbType.VarChar).Value = txt;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTEMPLOYEE";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        public DataTable GetEcfSubDocType()
        {
            try
            {
                DataTable dt = new DataTable();
                cmd = new SqlCommand("GetEcfSubDocType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);

                return dt;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }

        //added by Dhasarathan 
    }
}