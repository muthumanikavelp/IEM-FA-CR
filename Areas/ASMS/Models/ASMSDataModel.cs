using System;
using System.IO;
using System.Web;
using IEM.Common;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Globalization;
using System.Collections;

namespace IEM.Areas.ASMS.Models
{
    public class ASMSDataModel : ASMSIRepository
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        CommonIUD objCommonIUD = new CommonIUD();

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

        //public DataTable GetReviwerDetails(int usergid)
        //{
        //    DataTable dt = new DataTable();
        //    SqlDataAdapter da = new SqlDataAdapter();
        //    GetConnection();
        //    string lsQry = "select employee_code,employee_name from iem_mst_temployee where employee_gid=" + usergid + " and employee_isremoved='N' ";
        //    cmd.Connection = con;
        //    cmd.CommandType = CommandType.Text;
        //    cmd.CommandText = lsQry;
        //    da.SelectCommand = cmd;
        //    da.Fill(dt);
        //    return dt;
        //}

        public IEnumerable<SEModel> GetSEModelPending(string filter, int id)
        {
            List<SEModel> objreview = new List<SEModel>();
            try
            {
                
                List<SEModel> objreviewComp = new List<SEModel>();
                List<SEModel> objreviewdifference = new List<SEModel>();
                SEModel objModel;
                GetConnection();
                DataTable dt = new DataTable();

                cmd = new SqlCommand("pr_asms_trn_sereview", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = filter;
                cmd.Parameters.Add("@reviewer", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectPending";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);


                for (int m = 0; m < dt.Rows.Count; m++)
                {
                    DataTable dtsere = new DataTable();
                    cmd = new SqlCommand("select sereview_year from asms_trn_tsereview where sereview_supplierheader_gid='" + dt.Rows[m]["supplierheader_gid"] + "' and sereview_isremoved='N'", con);
                    cmd.CommandType = CommandType.Text;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dtsere);

                    ArrayList al = new ArrayList();
                    ArrayList al1 = new ArrayList();
                    if (dtsere.Rows.Count > 0)
                    {
                        for (int ij = 0; ij < dtsere.Rows.Count; ij++)
                        {
                            al1.Add(Convert.ToInt32(dtsere.Rows[ij]["sereview_year"]));
                        }
                    }
                    int yearDiff = Convert.ToInt32(dt.Rows[m]["yeardif"].ToString());
                    if (dtsere.Rows.Count > 0)
                    {

                        for (int j = 0; j < yearDiff; j++)
                        {
                            for (int ij = 0; ij < dtsere.Rows.Count; ij++)
                            {
                                if (Convert.ToInt32(dtsere.Rows[ij]["sereview_year"]) != j + Convert.ToInt32(dt.Rows[m]["sereview_reviewdon"].ToString()))
                                {
                                    if (!al.Contains(j + Convert.ToInt32(dt.Rows[m]["sereview_reviewdon"].ToString())))
                                    {
                                        if (!al1.Contains(j + Convert.ToInt32(dt.Rows[m]["sereview_reviewdon"].ToString())))
                                        {
                                            al.Add(j + Convert.ToInt32(dt.Rows[m]["sereview_reviewdon"].ToString()));
                                            objModel = new SEModel();
                                            objModel.seId = Convert.ToInt32(dt.Rows[m]["supplierheader_gid"].ToString());
                                            objModel.seSupCode = Convert.ToString(dt.Rows[m]["supplierheader_suppliercode"].ToString());
                                            objModel.seSupName = Convert.ToString(dt.Rows[m]["supplierheader_name"].ToString());
                                            objModel.seYear = j + Convert.ToInt32(dt.Rows[m]["sereview_reviewdon"].ToString());
                                            objModel.seStatus = Convert.ToString(dt.Rows[m]["supplierheader_status"].ToString());
                                            objreview.Add(objModel);
                                        }
                                    }

                                }

                            }

                        }
                    }
                    else
                    {
                        for (int j = 0; j <= yearDiff; j++)
                        {
                            objModel = new SEModel();
                            objModel.seId = Convert.ToInt32(dt.Rows[m]["supplierheader_gid"].ToString());
                            objModel.seSupCode = Convert.ToString(dt.Rows[m]["supplierheader_suppliercode"].ToString());
                            objModel.seSupName = Convert.ToString(dt.Rows[m]["supplierheader_name"].ToString());
                            objModel.seYear = j + Convert.ToInt32(dt.Rows[m]["sereview_reviewdon"].ToString());
                            objModel.seStatus = Convert.ToString(dt.Rows[m]["supplierheader_status"].ToString());
                            objreview.Add(objModel);
                        }
                    }

                }
                return objreview;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objreview;
            }
            finally
            {
                con.Close();
            }
          
        }

        public SEModel GetSEModelPendingById(int reviewID)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_sereview", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectPending";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var objreview = new SEModel()
                {
                    seId = Convert.ToInt32(dt.Rows[0]["supplierheader_gid"].ToString()),
                    seSupName = Convert.ToString(dt.Rows[0]["supplierheader_name"].ToString()),
                    seStatus = "Approved"
                };
                return objreview;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                SEModel objMod = new SEModel();
                return objMod; 
            }
            finally
            {
                con.Close();
            }
        }

        public string GetSupbyId(int supID)
        {
            try
            {
                GetConnection();
                string[,] parameter = new string[,]
                {
                    {"@id",supID.ToString()},                   
                    {"@action","selectsupname"},
                };
                return objCommonIUD.ProcedureCommon(parameter, "pr_asms_trn_sereview");
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

        public string GetLoginUserById(int gid)
        {
            try
            {
                GetConnection();
                string[,] parameter = new string[,]
                {
                    {"@reviewer",gid.ToString()},
                    {"@action","reviewer"},
                };
                return objCommonIUD.ProcedureCommon(parameter, "pr_asms_trn_sereview");
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

        public IEnumerable<SEModel> GetSEModelCompleted(string filter, int id)
        {
            List<SEModel> objreview = new List<SEModel>();
            try
            {
               
                SEModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_sereview", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = filter;
                cmd.Parameters.Add("@reviewer", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectComplete";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SEModel();
                    objModel.sNo = i + 1;
                    objModel.seId = Convert.ToInt32(dt.Rows[i]["supplierheader_gid"].ToString());
                    objModel.seSupCode = Convert.ToString(dt.Rows[i]["supplierheader_suppliercode"].ToString());
                    objModel.seSupName = Convert.ToString(dt.Rows[i]["supplierheader_name"].ToString());
                    objModel.seOverAllRating = Convert.ToString(dt.Rows[i]["sereview_overallrating"].ToString());
                    objModel.seReviewYear = Convert.ToString(dt.Rows[i]["sereview_reviewdon"].ToString());
                    objModel.seStatus = Convert.ToString(dt.Rows[i]["supplierheader_status"].ToString());
                    objreview.Add(objModel);
                }
                return objreview;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objreview;
            }
            finally
            {
                con.Close();
            }
        }

        public SEModel GetSEModelCompletedById(int revirewid)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_sereview", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectComplete";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var objreview = new SEModel()
                {
                    seId = Convert.ToInt32(dt.Rows[0]["supplierheader_gid"].ToString()),
                    seStatus = "Approved",
                    seSupName = Convert.ToString(dt.Rows[0]["supplierheader_name"].ToString()),
                };
                return objreview;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                SEModel objMod = new SEModel();
                return objMod;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable GetReviwerDetails(int revirewid)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
               
                cmd = new SqlCommand("pr_asms_trn_sereview", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = revirewid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getsuperlist";
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

        public IEnumerable<SERate> Getscorebygrp(int id)
        {
            List<SERate> objSESubCategory = new List<SERate>();
            try
            {
                
                SERate objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_SESubCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@Catname", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@Catid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@rateid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@rategroupid", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@queid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectrate";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SERate();
                    objModel.seRateID = Convert.ToInt32(dt.Rows[i]["serate_gid"].ToString());
                    objModel.seRatescore = Convert.ToString(dt.Rows[i]["serate_score"].ToString());
                    objModel.seRateName = Convert.ToString(dt.Rows[i]["serate_name"].ToString());
                    objSESubCategory.Add(objModel);
                }

                return objSESubCategory;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objSESubCategory;
            }
            finally
            {
                con.Close();
            }
        }

        public IEnumerable<SERate> GetSErate()
        {
            List<SERate> objSESubCategory = new List<SERate>();
            try
            {
              
                SERate objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_SESubCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@Catname", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@Catid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@rateid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@rategroupid", SqlDbType.VarChar).Value = 0;
                cmd.Parameters.Add("@queid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectrategrp";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SERate();
                    objModel.seRateID = Convert.ToInt32(dt.Rows[i]["serate_gid"].ToString());
                    objModel.seRateName = Convert.ToString(dt.Rows[i]["serate_name"].ToString());
                    objSESubCategory.Add(objModel);
                }
                return objSESubCategory;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objSESubCategory;
            }
            finally
            {
                con.Close();
            }
        }

        public IEnumerable<SERate> GetSEratedrop()
        {
            List<SERate> objSESubCategory = new List<SERate>();
            try
            {
              
                SERate objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_SESubCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "drop";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SERate();
                    objModel.seRateID = Convert.ToInt32(dt.Rows[i]["serate_gid"].ToString());
                    objModel.seRateName = Convert.ToString(dt.Rows[i]["serate_name"].ToString());
                    objSESubCategory.Add(objModel);
                }
                return objSESubCategory;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objSESubCategory;
            }
            finally
            {
                con.Close();
            }
        }

        public string InsertReview(SEModel model)
        {
            try
            {
                string comments = model.seReviewerComments.ToLower();
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_sereview", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@supid", SqlDbType.Int).Value = model.seSupID;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.ExecuteNonQuery();
                string[,] codes = new string[,]
	                   {
                            {"sereview_supplierheader_gid",Convert.ToString(model.seSupID) },
	                        {"sereview_reviewedby",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                            {"sereview_reviewdon",Convert.ToString(objCommonIUD.GetCurrentDate()) },
                            {"sereview_overallscore",model.seOverallScore},
                            {"sereview_overallrating",model.seOverAllRating },
	                        {"sereview_coordinator", Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                            {"sereview_year",Convert.ToString(model.seYear)},
                            {"sereview_comments",CultureInfo.CurrentCulture.TextInfo.ToTitleCase(comments.ToString())},                           

                      };
                string tname = "asms_trn_tsereview";
                string insertcommend = objCommonIUD.InsertCommon(codes, tname);
                return null;
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

        public string GetChildRate(int questiongid, int rategid)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_mst_SESubCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@questionchilgid", SqlDbType.Int).Value = questiongid;
                cmd.Parameters.Add("@rategid", SqlDbType.Int).Value = rategid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetChildRate";
                string result = Convert.ToString(cmd.ExecuteScalar());
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

        public string GetStatusexcel(string supdata, string valid, string action)
        {
            string result = "";
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_excelvalidation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@chkdata", SqlDbType.VarChar).Value = valid.ToString();
                cmd.Parameters.Add("@supdata", SqlDbType.VarChar).Value = supdata.ToString();
                cmd.Parameters.Add("@Result", SqlDbType.VarChar).Value = action;
                result = (string)cmd.ExecuteScalar();
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

        public string UpdateOwner(DataTable datatable)
        {
            try
            {
                List<SEModel> objList = new List<SEModel>();
                SEModel objModel;
                DataTable err1 = new DataTable();
                err1 = (DataTable)datatable;
                for (int i = 0; i < err1.Rows.Count; i++)
                {
                    objModel = new SEModel();

                    objModel.seSupCode = err1.Rows[i]["Supplier Code"].ToString().Trim();
                    //    objModel.seSupName = err1.Rows[i]["Supplier Name"].ToString().Trim();
                    objModel.ownerCode = err1.Rows[i]["Existing Owner Code"].ToString().Trim();
                    //   objModel.ownerName = err1.Rows[i]["Existing Owner Name"].ToString().Trim();
                    objModel.ownerCodeNew = err1.Rows[i]["New Owner Code"].ToString().Trim();
                    //    objModel.ownerNameNew = err1.Rows[i]["New Owner Name"].ToString().Trim();

                    string newOnwer = "select employee_gid from iem_mst_temployee where employee_code='" + objModel.ownerCodeNew + "' and employee_isremoved='N'";
                    GetConnection();
                    cmd = new SqlCommand(newOnwer, con);
                    cmd.CommandType = CommandType.Text;
                    int newOnwergid = Convert.ToInt32(cmd.ExecuteScalar());
                    string oldOwner = "select employee_gid from iem_mst_temployee where employee_code='" + objModel.ownerCode + "'  and employee_isremoved='N'";
                    GetConnection();
                    cmd = new SqlCommand(oldOwner, con);
                    cmd.CommandType = CommandType.Text;
                    int oldOwnergid = Convert.ToInt32(cmd.ExecuteScalar());
                    string gid = "select supplierheader_gid from asms_tmp_tsupplierheader where supplierheader_owner_gid=" + oldOwnergid + " and  supplierheader_suppliercode='" + objModel.seSupCode + "'  and supplierheader_isremoved='N'";
                    GetConnection();
                    cmd = new SqlCommand(gid, con);
                    cmd.CommandType = CommandType.Text;
                    int supgid = Convert.ToInt32(cmd.ExecuteScalar());
                    string[,] codes = new string[,]
	                {
                             {"supplierheader_owner_gid",newOnwergid.ToString()},
                             {"supplierheader_update_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                             {"supplierheader_update_date",Convert.ToString(objCommonIUD.GetCurrentDate())}
	                };
                    string[,] whrcol = new string[,]
	                {                          
                           {"supplierheader_owner_gid", oldOwnergid.ToString()} ,
                          // {"supplierheader_name",  objModel.seSupName .ToString()} ,
                           {"supplierheader_suppliercode",  objModel.seSupCode .ToString()} ,
                           {"supplierheader_isremoved", "N"} ,                           
                    };

                    string updateQueue = "update iem_trn_tqueue set queue_from=" + newOnwergid ;
                    updateQueue += " where queue_ref_flag=6 and queue_ref_gid in(select supplierheader_gid from ";
                    updateQueue += " asms_tmp_tsupplierheader where supplierheader_suppliercode='" + objModel.seSupCode.ToString() + "') ";
                    updateQueue += " and queue_from=" + oldOwnergid + " and queue_isremoved='Y'";
                    GetConnection();
                    cmd = new SqlCommand(updateQueue, con);
                    cmd.CommandType = CommandType.Text;
                    cmd.ExecuteNonQuery();

                    string tblname = "asms_trn_tsupplierheader";
                    string tblname1 = "asms_tmp_tsupplierheader";
                    string updcomm = objCommonIUD.UpdateCommon(codes, whrcol, tblname);
                    string updcomm1 = objCommonIUD.UpdateCommon(codes, whrcol, tblname1);
                    //history of updation
                    string[,] colval = new string[,]
               {
                        {"ownerupdate_beforeupdate_ownergid",oldOwnergid.ToString()},
                        {"ownerupdate_afterupdate_ownergid",newOnwergid.ToString()},
                        {"ownerupdate_supplierheader_code",  objModel.seSupCode .ToString()} ,
                        {"ownerupdate_insertby",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                        {"ownerupdate_insert_date","sysdatetime()"}
                };
                    string tblename = "asms_trn_townerupdate";
                    string insertcomm = objCommonIUD.InsertCommon(colval, tblename);
                }

                return "Success";
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

        public IEnumerable<SupplierHeader> GetStatusGridSupplierDetails()
        {
            List<SupplierHeader> objSupHeaderDetails = new List<SupplierHeader>();
            try
            {
                
                SupplierHeader objModel;
             //   string fsqry = "select supplierheader_gid,supplierheader_suppliercode,supplierheader_name,supplierheader_owner_gid,employee_code,employee_name from asms_trn_tsupplierheader join iem_mst_temployee on employee_gid=supplierheader_owner_gid where supplierheader_status='APPROVED' and supplierheader_isremoved='N'";
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_iem_mst_OwnerUpdation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getsupplierdetailsforownerupdation";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierHeader();
                    objModel._HeaderID = Convert.ToInt32(dt.Rows[i]["supplierheader_gid"].ToString());
                    objModel._SupplierCode = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_suppliercode"].ToString()) ? "" : dt.Rows[i]["supplierheader_suppliercode"].ToString());
                    objModel._SupplierName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_name"].ToString()) ? "" : dt.Rows[i]["supplierheader_name"].ToString());
                    objModel._OwnerId = Convert.ToInt32(dt.Rows[i]["supplierheader_owner_gid"].ToString());
                    objModel._OwnerCode = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["employee_code"].ToString()) ? "" : dt.Rows[i]["employee_code"].ToString());
                    objModel._OwnerName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["employee_name"].ToString()) ? "" : dt.Rows[i]["employee_name"].ToString());
                    objSupHeaderDetails.Add(objModel);
                }
                return objSupHeaderDetails;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objSupHeaderDetails;
            }
            finally
            {
                con.Close();
            }
        }

        public List<SearchEmployee> GetEmployeeList()
        {
            List<SearchEmployee> objSearchEmployee = new List<SearchEmployee>();
            try
            {
                
                SearchEmployee objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getemployeelist";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SearchEmployee();
                    objModel._EmployeeGid = Convert.ToInt32(dt.Rows[i]["employee_gid"].ToString());
                    objModel._EmployeeCode = Convert.ToString(dt.Rows[i]["employee_code"].ToString());
                    objModel._EmployeeName = Convert.ToString(dt.Rows[i]["employee_name"].ToString());
                    objSearchEmployee.Add(objModel);
                }

                return objSearchEmployee;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objSearchEmployee;
            }
            finally
            {
                con.Close();
            }
        }

        public string UpdateOnwer(SupplierHeader model)
        {
            try
            {
                string oldOwner = "select supplierheader_owner_gid from asms_tmp_tsupplierheader where supplierheader_gid=" + model._HeaderID;
                GetConnection();
                cmd = new SqlCommand(oldOwner, con);
                cmd.CommandType = CommandType.Text;
                int oldOwnergid = Convert.ToInt32(cmd.ExecuteScalar());
                string supCode = "select supplierheader_suppliercode from asms_tmp_tsupplierheader where supplierheader_gid=" + model._HeaderID;
                GetConnection();
                cmd = new SqlCommand(supCode, con);
                cmd.CommandType = CommandType.Text;
                string supCode1 = Convert.ToString(cmd.ExecuteScalar());

                string[,] codes = new string[,]
	                {
                             {"supplierheader_owner_gid",model._OwnerId.ToString()},
                             {"supplierheader_update_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                             {"supplierheader_update_date","sysdatetime()"}
	                };
                string[,] whrcol = new string[,]
	                {
                           {"supplierheader_suppliercode", supCode1.ToString()} , 
                           {"supplierheader_isremoved", "N"} ,                           
                    };
                string tblname = "asms_trn_tsupplierheader";
                string updcomm = objCommonIUD.UpdateCommon(codes, whrcol, tblname);
                string tblname1 = "asms_tmp_tsupplierheader";
                string updcomm1 = objCommonIUD.UpdateCommon(codes, whrcol, tblname1);
                //history of updation
                string[,] colval = new string[,]
               {
                        {"ownerupdate_beforeupdate_ownergid",oldOwnergid.ToString()},
                        {"ownerupdate_afterupdate_ownergid",model._OwnerId.ToString()},
                        {"ownerupdate_supplierheader_code",  supCode1 .ToString()} ,
                        {"ownerupdate_insertby",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                        {"ownerupdate_insert_date","sysdatetime()"}
                };
                string tblename = "asms_trn_townerupdate";
                string insertcomm = objCommonIUD.InsertCommon(colval, tblename);

                string updateQueue = "update iem_trn_tqueue set queue_from=" + model._OwnerId.ToString() + " ";
                updateQueue += " where queue_ref_flag=6 and queue_ref_gid in(select supplierheader_gid from ";
                updateQueue += " asms_tmp_tsupplierheader where supplierheader_owner_gid=" + oldOwnergid + ") ";
                updateQueue += " and queue_from=" + oldOwnergid + " and queue_isremoved='Y'";
                GetConnection();
                cmd = new SqlCommand(updateQueue, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();

                return "success";
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
        public string convertoDate(string date1)
        {
            try
            {
                string date2 = date1;
                string convdate = string.Empty;
                DateTime convdate2 = Convert.ToDateTime(date2);
                string format = "dd/MMM/yyyy";
                convdate = convdate2.ToString(format);
                return convdate;
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
        public List<ReportModel> GetReport(string ownerName
            , string ownerCode
            , string vendorName
            , string vendorCode
            , string contractType
              , string categoryName
              , string contractEndDate
              , string serviceTypeName
              , string employeeCode
              , string employeeName
               , string city
              , string state
            , string search
             )
        {
            List<ReportModel> objq = new List<ReportModel>();
            ReportModel reportModel = new ReportModel();
            DataTable dtmodel = new DataTable();
            try
            {

                string ownername = string.IsNullOrEmpty(ownerName) ? string.Empty : ownerName;
                string ownercode = string.IsNullOrEmpty(ownerCode) ? string.Empty : ownerCode;
                string empcode = string.IsNullOrEmpty(employeeCode) ? string.Empty : employeeCode;
                string empname = string.IsNullOrEmpty(employeeName) ? string.Empty : employeeName;
                string vendorcode = string.IsNullOrEmpty(vendorCode) ? string.Empty : vendorCode;
                string vendorname = string.IsNullOrEmpty(vendorName) ? string.Empty : vendorName;
                string contractenddate = string.IsNullOrEmpty(contractEndDate) ? string.Empty : convertoDate(contractEndDate);

                if (categoryName == "0")
                {
                    categoryName = string.Empty;
                }
                if (city == "0")
                {
                    city = string.Empty;
                }
                if (serviceTypeName == "0")
                {
                    serviceTypeName = string.Empty;
                }
                if (state == "0")
                {
                    state = string.Empty;
                }
                if (contractType == "0")
                {
                    contractType = string.Empty;
                }
                string categoryname = string.IsNullOrEmpty(categoryName) ? string.Empty : categoryName;
                string cityname = string.IsNullOrEmpty(city) ? string.Empty : city;
                string statename = string.IsNullOrEmpty(state) ? string.Empty : state;
                string servicetypename = string.IsNullOrEmpty(serviceTypeName) ? string.Empty : serviceTypeName;
                string contracttype = string.IsNullOrEmpty(contractType) ? string.Empty : contractType;

                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@vendorCode", vendorcode);
                cmd.Parameters.AddWithValue("@vendorName", vendorname);
                cmd.Parameters.AddWithValue("@ownerCode", ownercode);
                cmd.Parameters.AddWithValue("@ownerName", ownername);
                cmd.Parameters.AddWithValue("@employeeCode", empcode);
                cmd.Parameters.AddWithValue("@employeeName ", empname);
                cmd.Parameters.AddWithValue("@categoryName", categoryname);
                cmd.Parameters.AddWithValue("@city", cityname);
                cmd.Parameters.AddWithValue("@state", statename);
                cmd.Parameters.AddWithValue("@contractType", contracttype);
                cmd.Parameters.AddWithValue("@serviceTypeName", servicetypename);
                cmd.Parameters.AddWithValue("@contractEndDate", contractenddate);
                if (vendorcode == string.Empty
                    && vendorname == string.Empty
                    && ownercode == string.Empty
                    && ownername == string.Empty
                    && empcode == string.Empty
                    && empname == string.Empty
                    && categoryname == string.Empty
                    && cityname == string.Empty
                    && statename == string.Empty
                    && servicetypename == string.Empty
                    && contracttype == string.Empty
                    && contractenddate == string.Empty)
                {
                    cmd.Parameters.AddWithValue("@search", "all");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@search", "certain");
                }

                da = new SqlDataAdapter(cmd);
                da.Fill(dtmodel);
                if (dtmodel.Rows.Count > 0)
                {
                    HttpContext.Current.Session["exceldownload"] = dtmodel;
                    for (int i = 0; i < dtmodel.Rows.Count; i++)
                    {
                        reportModel = new ReportModel();
                        reportModel.ownerDepartment = dtmodel.Rows[i]["OwnerDepartment"].ToString();
                        reportModel.ownerName = dtmodel.Rows[i]["OwnerName"].ToString();
                        reportModel.ownerEmployeeCode = dtmodel.Rows[i]["OwnerEmployeeCode"].ToString();
                        reportModel.ownerEmailID = dtmodel.Rows[i]["OwnerEmailID"].ToString();
                        reportModel.vendorName = dtmodel.Rows[i]["VendorName"].ToString();
                        reportModel.vendorCode = dtmodel.Rows[i]["VendorCode"].ToString();
                        reportModel.vendorEmailID = dtmodel.Rows[i]["VendorEmailID"].ToString();
                        reportModel.vendorAddress = dtmodel.Rows[i]["VendorAddress"].ToString();
                        reportModel.vendorCoordinatorName = dtmodel.Rows[i]["VendorCoordinatorName"].ToString();
                        reportModel.vendorEmpanelmentDate = dtmodel.Rows[i]["VendorEmpanelmentDate"].ToString();
                        reportModel.vendorState = dtmodel.Rows[i]["VendorState"].ToString();
                        reportModel.vendorCity = dtmodel.Rows[i]["VendorCity"].ToString();
                        reportModel.typeofAgreement = dtmodel.Rows[i]["AgreementType"].ToString();
                        reportModel.vendorOfficeNumber = dtmodel.Rows[i]["VendorOfficeNumber"].ToString();
                        reportModel.vendorWebsiteAddress = dtmodel.Rows[i]["VendorWebsiteAddress"].ToString();
                        reportModel.agreementEndDate = dtmodel.Rows[i]["AgreementEndDate"].ToString();
                        reportModel.finalvendorStatus = dtmodel.Rows[i]["FinalVendorStatus"].ToString();
                        reportModel.vendorValidityDate = dtmodel.Rows[i]["VendorValidityDate"].ToString();
                        reportModel.pANNumber = dtmodel.Rows[i]["PANNumber"].ToString();
                        reportModel.procurementName = dtmodel.Rows[i]["ProcurementCoordinatorName"].ToString();
                        reportModel.procurementEmployeeCode = dtmodel.Rows[i]["ProcurementCoordinatorEmployeeCode"].ToString();
                        reportModel.procurementEmailID = dtmodel.Rows[i]["ProcurementCoordinatorEmailID"].ToString();
                        reportModel.remarks = dtmodel.Rows[i]["Remarks"].ToString();
                        reportModel.coordinatorDepartment = dtmodel.Rows[i]["CoordinatorDepartment"].ToString();
                        reportModel.coordinatorName = dtmodel.Rows[i]["CoordinatorName"].ToString();
                        reportModel.coordinatorcode = dtmodel.Rows[i]["CoordinatorEmployeeCode"].ToString();
                        reportModel.coordinatorEmailID = dtmodel.Rows[i]["CoordinatorEmailID"].ToString();
                        reportModel.categoryName = dtmodel.Rows[i]["VendorCategory"].ToString();
                        reportModel.natureofServicesCategory = dtmodel.Rows[i]["ServicesCategory"].ToString();

                        objq.Add(reportModel);
                    }
                }
                else
                {
                    HttpContext.Current.Session["exceldownload"] = dtmodel;
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
            return objq;
        }
        public IEnumerable<ReportModel> Getcategory()
        {
            List<ReportModel> objcategory = new List<ReportModel>();
            try
            {
                ReportModel objModel;

                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("select suppliercategory_gid,suppliercategory_name from asms_mst_tsuppliercategory where suppliercategory_isremoved='N' order by suppliercategory_name desc", con);
                cmd.CommandType = CommandType.Text;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objcategory.Add(new ReportModel { categoryID = 0, categoryName = "-- Select --", });
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ReportModel();
                    objModel.categoryID = Convert.ToInt32(dt.Rows[i]["suppliercategory_gid"].ToString());
                    objModel.categoryName = Convert.ToString(dt.Rows[i]["suppliercategory_name"].ToString());
                    objcategory.Add(objModel);
                }
                return objcategory;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objcategory;
            }
            finally
            {
                con.Close();
            }
           
        }
        public IEnumerable<ReportModel> Getservicetype()
        {
            List<ReportModel> objcategory = new List<ReportModel>();
            try
            {
                ReportModel objModel;

                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("select servicetype_gid,servicetype_name FROM asms_mst_tservicetype  where servicetype_isremoved='N' order by servicetype_name desc", con);
                cmd.CommandType = CommandType.Text;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objcategory.Add(new ReportModel { serviceId = 0, natureofServicesCategory = "-- Select --", });
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ReportModel();
                    objModel.serviceId = Convert.ToInt32(dt.Rows[i]["servicetype_gid"].ToString());
                    objModel.natureofServicesCategory = Convert.ToString(dt.Rows[i]["servicetype_name"].ToString());
                    objcategory.Add(objModel);
                }
                return objcategory;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objcategory;
            }
            finally
            {
                con.Close();
            }
           
        }
        public IEnumerable<ReportModel> Getstate()
        {
            List<ReportModel> objcategory = new List<ReportModel>();
            try
            {
                ReportModel objModel;

                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("select state_gid,state_name FROM iem_mst_tstate  where state_isremoved='N' order by state_name desc", con);
                cmd.CommandType = CommandType.Text;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objcategory.Add(new ReportModel { stateID = 0, vendorState = "-- Select --", });
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ReportModel();
                    objModel.stateID = Convert.ToInt32(dt.Rows[i]["state_gid"].ToString());
                    objModel.vendorState = Convert.ToString(dt.Rows[i]["state_name"].ToString());
                    objcategory.Add(objModel);
                }
                return objcategory;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objcategory;
            }
            finally
            {
                con.Close();
            }
           
        }
        public IEnumerable<ReportModel> Getcity()
        {
            List<ReportModel> objcategory = new List<ReportModel>();
            try
            {
                ReportModel objModel;

                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("select city_gid,city_name FROM iem_mst_tcity  where city_isremoved='N'  order by city_name desc", con);
                cmd.CommandType = CommandType.Text;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objcategory.Add(new ReportModel { cityID = 0, vendorCity = "-- Select --", });
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ReportModel();
                    objModel.cityID = Convert.ToInt32(dt.Rows[i]["city_gid"].ToString());
                    objModel.vendorCity = Convert.ToString(dt.Rows[i]["city_name"].ToString());
                    objcategory.Add(objModel);
                }
                return objcategory;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objcategory;
            }
            finally
            {
                con.Close();
            }
          
        }
        public IEnumerable<ReportModel> Getcontract()
        {
            List<ReportModel> objcategory = new List<ReportModel>();
            try
            {
                ReportModel objModel;

                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("   select  contacttype_gid,contacttype_name from asms_mst_tcontacttype where contacttype_isremoved='N' order by contacttype_name desc ", con);
                cmd.CommandType = CommandType.Text;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objcategory.Add(new ReportModel { AgreementId = 0, typeofAgreement = "-- Select --", });
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ReportModel();
                    objModel.AgreementId = Convert.ToInt32(dt.Rows[i]["contacttype_gid"].ToString());
                    objModel.typeofAgreement = Convert.ToString(dt.Rows[i]["contacttype_name"].ToString());
                    objcategory.Add(objModel);
                }
                return objcategory;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objcategory;
            }
            finally
            {
                con.Close();
            }
           
        }

    }
}