using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Drawing;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using System.IO;
using IEM.Common;
using System.Net;
using System.IO.IsolatedStorage;
using System.Globalization;
using System.Text.RegularExpressions;
namespace IEM.Areas.IFAMS.Models
{
    public class DataModel : IRepository
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        CommonIUD objCommonIUD = new CommonIUD();
        SqlTransaction Tran = null;

        public void GetConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }
        public IEnumerable<TransferMakerModel> GetGlNumber(string glnumber)
        {
            List<TransferMakerModel> objNatureofExpenses = new List<TransferMakerModel>();
            try
            {
                TransferMakerModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_eow_mst_NatureofExpenses", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@para1", SqlDbType.VarChar).Value = glnumber;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "AUTOCOMPLETEGLNO";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new TransferMakerModel();
                    objModel.GLId = Convert.ToInt32(row["gl_gid"].ToString());
                    objModel.GLCode = row["gl_no"].ToString();
                    objNatureofExpenses.Add(objModel);
                }
                return objNatureofExpenses;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
            //throw new NotImplementedException();
        }
        public string getTheUser(string groupCode)
        {

            string success = string.Empty;
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "group";
                cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = groupCode;
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                int userid = objCmnFunctions.GetLoginUserGid();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["roleemployee_employee_gid"].ToString() == userid.ToString())
                        {
                            success = "success";
                        }
                    }
                }
                return success;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return string.Empty;
            }
            finally
            {
                con.Close();
            }
        }

        public string updateFCCC(TransferMakerModel fc)
        {
            try
            {

                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = fc._asset_id;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "assetdetails";
                fc._asset_id = Convert.ToString(cmd.ExecuteScalar());
                fc._fccc_gid = Getfccc(fc._fccc).ToString();
                string[,] codes = new string[,]
	                { 
                             {"toadetail_fccc_gid",fc._fccc_gid.ToString()}};
                string[,] where = new string[,]
	                { 
                             {"toadetail_assetdet_gid",fc._asset_id.ToString()}};
                objCommonIUD.UpdateCommon(codes, where, "fa_trn_ttoadetail");
                return fc.ToString();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return fc.ToString();
            }
            finally
            {
                con.Close();
            }
        }

        public string Getststus(TransferMakerModel status, string id)
        {
            try
            {

                GetConnection();
                status._toa_number = id;

                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "TRFstatus";
                cmd.Parameters.Add("@headergid", SqlDbType.VarChar).Value = status._toa_number;
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    status._tfr_status = dt.Rows[0]["toaheader_tfr_status"].ToString();
                }
                DataTable dt1 = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = status._tfr_status;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "statusname";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt1);
                foreach (DataRow row1 in dt1.Rows)
                    status._tfr_status = row1["status_name"].ToString();
                return status._tfr_status;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return status._tfr_status;
            }
            finally
            {
                con.Close();
            }
        }

        public string Getwrtststus(WriteOffModel status, string id)
        {
            try
            {

                GetConnection();
                status._woa_number = id;

                cmd = new SqlCommand("pr_ifams_trn_woaMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "WRTstatus";
                cmd.Parameters.Add("@headergid", SqlDbType.VarChar).Value = status._woa_number;
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    status._woa_status = dt.Rows[0]["woaheader_woa_status"].ToString();
                }
                DataTable dt1 = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = status._woa_status;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "statusname";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt1);
                foreach (DataRow row1 in dt1.Rows)
                    status._woa_status = row1["status_name"].ToString();
                return status._woa_status;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return status._woa_status;
            }
            finally
            {
                con.Close();
            }
        }

        public string GetGrpCount(string grpId)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_exceldataverification", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Result", SqlDbType.VarChar).Value = "Groupcount";
                cmd.Parameters.Add("@chkdata", SqlDbType.VarChar).Value = grpId;
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                grpId = "0";
                if (dt.Rows.Count > 0)
                {
                    grpId = dt.Rows[0]["goaheader_asset_count"].ToString();
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {


            }
            return grpId;
        }



        public DataTable updateTrfStatus(TransferMakerModel transferMakerModel) // Transfer Raiser Submit
        {
            string Result = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_Set_Asset_TransferDetailsApproved", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prToaNumber", SqlDbType.VarChar).Value = transferMakerModel._toa_number;
                cmd.Parameters.Add("@prToaStatus", SqlDbType.VarChar).Value = transferMakerModel._tfr_status;
                cmd.Parameters.Add("@prToaRemarks", SqlDbType.VarChar).Value = transferMakerModel.actionRemarks;
                cmd.Parameters.Add("@prToaReceiverId", SqlDbType.Int).Value = transferMakerModel.receiverId;
                cmd.Parameters.Add("@prToReceiverBranchcodename", SqlDbType.VarChar).Value = transferMakerModel.receiverBranchCode;
                cmd.Parameters.Add("@prToaLogginId", SqlDbType.Int).Value = Convert.ToString(objCmnFunctions.GetLoginUserGid());
                cmd.Parameters.Add("@prToaAction", SqlDbType.VarChar).Value = transferMakerModel.commandType;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return dt;
        }


        public IEnumerable<TransferMakerModel> GetTrfDetailSummary(string id)
        {
            List<TransferMakerModel> objModel = new List<TransferMakerModel>();
            try
            {

                TransferMakerModel obj = new TransferMakerModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_Get_formyapproval", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prToaEmployeeGid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@prToaDocType", SqlDbType.VarChar).Value = "TOA";
                cmd.Parameters.Add("@prToaAction", SqlDbType.VarChar).Value = "ForMyApproval";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new TransferMakerModel();
                    obj._tfr_status = Getststus(obj, row["queue_ref_gid"].ToString());
                    obj._gid = Convert.ToInt32(row["queue_ref_gid"].ToString());
                    obj._toa_number = row["doc_no"].ToString();


                    DateTime dateformat = Convert.ToDateTime(row["doc_date"].ToString());
                    obj._tfr_date = dateformat.ToString("dd-MM-yyy");


                    //obj._tfr_date = Convert.ToString(row["doc_date"].ToString());
                    obj._no_records = Convert.ToInt64(row["doc_noof_records"].ToString());
                    obj._upld_fname = row["doc_file_name"].ToString();
                    objModel.Add(obj);
                }
                return objModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return objModel;
            }
            finally
            {
                con.Close();
            }
        }





        public List<TransferMakerModel> verifylogedindetails(string headergid)
        {
            List<TransferMakerModel> objl = new List<TransferMakerModel>();
            TransferMakerModel obji;
            DataTable dtGetDeetails = new DataTable();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_verifylogedinstatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@empgid", Convert.ToString(objCmnFunctions.GetLoginUserGid()));
                cmd.Parameters.AddWithValue("@process", "toa");
                cmd.Parameters.AddWithValue("@headergid", headergid);
                da = new SqlDataAdapter(cmd);
                da.Fill(dtGetDeetails);
                if (dtGetDeetails.Rows.Count > 0)
                {
                    for (int i = 0; i < dtGetDeetails.Rows.Count; i++)
                    {
                        obji = new TransferMakerModel();
                        obji.loged_empgid = Convert.ToInt32(dtGetDeetails.Rows[i]["empgid"].ToString());
                        obji.loged_empcode = dtGetDeetails.Rows[i]["employeecode"].ToString();
                        obji.loged_empname = dtGetDeetails.Rows[i]["employeename"].ToString();
                        obji.loged_date = dtGetDeetails.Rows[i]["logeddate"].ToString();
                        objl.Add(obji);
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return objl;
        }



        public int UpdatelogedinStatus(string headergid)
        {
            int insertcommand = 0;
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_verifylogedinstatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@empgid", Convert.ToString(objCmnFunctions.GetLoginUserGid()));
                cmd.Parameters.AddWithValue("@process", "UpdateStatus");
                cmd.Parameters.AddWithValue("@headergid", headergid);
                da = new SqlDataAdapter(cmd);
                insertcommand = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return insertcommand;

        }


        //public void updateTrfStatus(TransferMakerModel status, string id)
        //{
        //    try
        //    {
        //        GetConnection();
        //        status._toa_number = id;
        //        //string _toa_number = string.Empty;
        //        string grpid = string.Empty;
        //        DataTable dt = new DataTable();
        //        DataTable dt1 = new DataTable();
        //        string wait = "WAITING FOR APPROVAL";
        //        cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = wait.ToString();
        //        cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "status";
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt1);
        //        foreach (DataRow row1 in dt1.Rows)
        //            status._tfr_status = row1["status_flag"].ToString();
        //        dt = GetheaderDetails(status._toa_number);
        //        if (dt.Rows.Count > 0)
        //        {
        //            id = dt.Rows[0]["toaheader_gid"].ToString();
        //        }
        //        string[,] codes = new string[,]
        //            { 
        //                     {"toaheader_tfr_status",status._tfr_status}
        //            };
        //        string[,] where = new string[,]
        //            { 
        //                     {"toaheader_gid",id.ToString()}
        //            };
        //        objCommonIUD.UpdateCommon(codes, where, "fa_trn_ttoaheader");

        //        DataTable dt2 = new DataTable();
        //        cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = "TOACHK";
        //        cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "groupname";
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt2);
        //        foreach (DataRow row1 in dt2.Rows)
        //            grpid = row1["rolegroup_gid"].ToString();
        //        string[,] col = new string[,]
        //                    {
        //                             {"queue_date",Convert.ToString(objCommonIUD.GetCurrentDate())},
        //                             {"queue_ref_flag",GetRef("TOA").ToString()},
        //                             {"queue_ref_gid", dt.Rows[0]["toaheader_gid"].ToString()},
        //                             {"queue_action_for", "A"},
        //                             {"queue_from",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
        //                             {"queue_to_type","G" },
        //                             {"queue_to",grpid},
        //                             {"queue_prev_gid","0"},
        //                             {"queue_action_status","0"},
        //                             {"queue_isremoved","N"}                                     
        //                  };
        //        string inst = objCommonIUD.InsertCommon(col, "iem_trn_tqueue");
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //}

        //public void updateWrtStatus(WriteOffModel status, string id)
        //{
        //    try
        //    {
        //        GetConnection();
        //        status._woa_number = id;
        //        string grpid = string.Empty;
        //        DataTable dt = new DataTable();
        //        DataTable dt1 = new DataTable();
        //        string wait = "WAITING FOR APPROVAL";
        //        cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = wait.ToString();
        //        cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "status";
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt1);
        //        foreach (DataRow row1 in dt1.Rows)
        //            status._woa_status = row1["status_flag"].ToString();
        //        dt = GetwoaheaderDetails(status._woa_number);
        //        if (dt.Rows.Count > 0)
        //        {
        //            id = dt.Rows[0]["woaheader_gid"].ToString();
        //        }
        //        string[,] codes = new string[,]
        //            { 
        //                     {"woaheader_woa_status",status._woa_status}
        //            };
        //        string[,] where = new string[,]
        //            { 
        //                     {"woaheader_gid",id.ToString()}
        //            };
        //        objCommonIUD.UpdateCommon(codes, where, "fa_trn_twoaheader");

        //        DataTable dt2 = new DataTable();
        //        cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = "WOACHK";
        //        cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "groupname";
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt2);
        //        foreach (DataRow row1 in dt2.Rows)
        //            grpid = row1["rolegroup_gid"].ToString();
        //        string[,] col = new string[,]
        //                    {
        //                             {"queue_date",Convert.ToString(objCommonIUD.GetCurrentDate())},
        //                             {"queue_ref_flag",GetRef("WOA").ToString()},
        //                             {"queue_ref_gid", dt.Rows[0]["woaheader_gid"].ToString()},
        //                             {"queue_action_for", "A"},
        //                             {"queue_from",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
        //                             {"queue_to_type","G" },
        //                             {"queue_to",grpid},
        //                             {"queue_prev_gid","0"},
        //                             {"queue_action_status","0"},
        //                             {"queue_isremoved","N"}                                     
        //                  };
        //        string inst = objCommonIUD.InsertCommon(col, "iem_trn_tqueue");
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //}


        // Write-Off Submit
        public DataTable updateWrtStatus(WriteOffModel assetwriteoffDetails) // Write off Submit
        {
            string Result = string.Empty;
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_Set_Asset_WriteoffDetailsApproved", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prWoaNumber", SqlDbType.VarChar).Value = assetwriteoffDetails._woa_number;
                cmd.Parameters.Add("@prWoaStatus", SqlDbType.VarChar).Value = assetwriteoffDetails._woa_status;
                cmd.Parameters.Add("@prWoaRemarks", SqlDbType.VarChar).Value = assetwriteoffDetails.actionremarks;
                cmd.Parameters.Add("@prWoaLogginId", SqlDbType.Int).Value = Convert.ToString(objCmnFunctions.GetLoginUserGid());
                cmd.Parameters.Add("@prWoaAction", SqlDbType.VarChar).Value = assetwriteoffDetails.commandType;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return dt;
        }


        // For Write off Supervisior

        public IEnumerable<WriteOffModel> GetWrtDetailforSupervisior(string id)
        {
            List<WriteOffModel> objModel = new List<WriteOffModel>();
            try
            {

                WriteOffModel obj = new WriteOffModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_Get_formyapproval", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prToaEmployeeGid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@prToaDocType", SqlDbType.VarChar).Value = "WOA";
                cmd.Parameters.Add("@prToaAction", SqlDbType.VarChar).Value = "ForMyApproval";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {

                    obj = new WriteOffModel();
                    obj._woa_status = Getwrtststus(obj, row["queue_ref_gid"].ToString());
                    obj._gid = Convert.ToInt32(row["queue_ref_gid"].ToString());
                    obj._woa_number = row["doc_no"].ToString();
                    DateTime dateformat = Convert.ToDateTime(row["doc_date"].ToString());
                    obj._woa_date = dateformat.ToString("dd-MM-yyy");
                    //   obj._woa_date = Convert.ToString(row["doc_date"].ToString());
                    obj._no_records = Convert.ToInt64(row["doc_noof_records"].ToString());
                    obj._upld_fname = row["doc_file_name"].ToString();
                    objModel.Add(obj);
                }
                return objModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return objModel;
            }
            finally
            {
                con.Close();
            }
        }



        public IEnumerable<TransferMakerModel> GetTrfDetailFull(string id)
        {
            List<TransferMakerModel> objModel = new List<TransferMakerModel>();
            try
            {

                TransferMakerModel obj = new TransferMakerModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "SelectSum";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new TransferMakerModel();
                    DataTable dt1 = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = row["toaheader_tfr_status"].ToString();
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "statusname";
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt1);
                    foreach (DataRow row1 in dt1.Rows)
                        obj._tfr_status = row1["status_name"].ToString();
                    obj._gid = Convert.ToInt32(row["toaheader_gid"].ToString());
                    obj._toa_number = row["toaheader_toa_number"].ToString();
                    obj._tfr_date = Convert.ToString(row["toaheader_tfr_date"].ToString());
                    obj._no_records = Convert.ToInt64(row["toaheader_no_records"].ToString());
                    obj._upld_fname = row["toaheader_upld_fname"].ToString();
                    objModel.Add(obj);
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

        //selvachanges
        public IEnumerable<TransferMakerModel> GetTrfDetailDraft(string id) //TOA For Draft
        {
            List<TransferMakerModel> objModel = new List<TransferMakerModel>();
            try
            {

                TransferMakerModel obj = new TransferMakerModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = id;
                //cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "SelectSumDRAFT";

                cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = Convert.ToString(objCmnFunctions.GetLoginUserGid().ToString());
                cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = string.IsNullOrEmpty(id) ? null : id.ToString();
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "SelectSumDRAFT";

                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {

                    obj = new TransferMakerModel();
                    obj._tfr_status = "DRAFT";
                    obj._gid = Convert.ToInt32(row["toaheader_gid"].ToString());
                    obj._toa_number = row["toaheader_toa_number"].ToString();
                    obj._tfr_date = Convert.ToString(row["toaheader_tfr_date"].ToString());
                    obj._no_records = Convert.ToInt64(row["toaheader_no_records"].ToString());
                    obj._upld_fname = row["toaheader_upld_fname"].ToString();
                    objModel.Add(obj);
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


        //FOR TOA GET REJECT DATA


        public IEnumerable<TransferMakerModel> GetTrfDetailRejected(string id) //TOA For rejected data
        {
            List<TransferMakerModel> objModel = new List<TransferMakerModel>();
            try
            {

                TransferMakerModel obj = new TransferMakerModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = id;
                //cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "SELECTREJECT";
                cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = Convert.ToString(objCmnFunctions.GetLoginUserGid().ToString());
                cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = string.IsNullOrEmpty(id) ? null : id.ToString();
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "SELECTREJECT";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {

                    obj = new TransferMakerModel();
                    obj._tfr_status = "REJECTED";
                    obj._gid = Convert.ToInt32(row["toaheader_gid"].ToString());
                    obj._toa_number = row["toaheader_toa_number"].ToString();
                    obj._tfr_date = Convert.ToString(row["toaheader_tfr_date"].ToString());
                    obj._no_records = Convert.ToInt64(row["toaheader_no_records"].ToString());
                    obj._upld_fname = row["toaheader_upld_fname"].ToString();
                    objModel.Add(obj);
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


        //Get Waiting For Approvarl
        public IEnumerable<TransferMakerModel> GetTrfDetailWFAP(string id)
        {
            List<TransferMakerModel> objModel = new List<TransferMakerModel>();
            try
            {

                TransferMakerModel obj = new TransferMakerModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_get_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "SELECTWaitingForApproval";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new TransferMakerModel();
                    obj._tfr_status = "WAITING FOR APPROVAL";
                    obj._gid = Convert.ToInt32(row["toaheader_gid"].ToString());
                    obj._toa_number = row["toaheader_toa_number"].ToString();
                    obj._tfr_date = Convert.ToString(row["toaheader_tfr_date"].ToString());
                    obj._no_records = Convert.ToInt64(row["toaheader_no_records"].ToString());
                    obj._upld_fname = row["toaheader_upld_fname"].ToString();
                    objModel.Add(obj);
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

        public IEnumerable<TransferMakerModel> GetTransferDetail(string Transferno)
        {
            List<TransferMakerModel> objModel = new List<TransferMakerModel>();
            try
            {

                TransferMakerModel obj = new TransferMakerModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "SelectView";
                cmd.Parameters.Add("@Transferno", SqlDbType.VarChar).Value = Transferno;
                cmd.Transaction = Tran;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                obj._toa_number = Transferno;
                foreach (DataRow row in dt.Rows)
                {
                    obj = new TransferMakerModel();
                    obj._gid = Convert.ToInt32(row["assetgid"].ToString());
                    obj._asset_id = row["assetid"].ToString();
                    obj._new_asset_id = row["newassetid"].ToString();
                    obj._AssetCode = row["assetcode"].ToString();
                    obj._AssetDesp = row["assetdesc"].ToString();
                    obj._loc_from = row["oldloc"].ToString();
                    obj._loc_to = row["newloc"].ToString();
                    // obj._fccc = row["FC"].ToString();
                    obj.queue_from = row["CC"].ToString();
                    obj._AssetCatCode = row["assetcat"].ToString();
                    obj._tfr_date = row["toadetail_tfr_date"].ToString();
                    obj._tfr_reason = row["toadetail_tfr_reason"].ToString();
                    obj._inw_no = row["toadetail_inw_no"].ToString();
                    obj.rectiAmt = getRetificationAmount(obj._gid.ToString(), obj._tfr_date.ToString());
                    objModel.Add(obj);
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


        public TransferMakerModel GetRaiserDetail(string raiserId, string toa_number)
        {
            TransferMakerModel lstObject = new TransferMakerModel();
            DataTable dt = new DataTable();
            //string employeeGid,employeeCode, employeeName, branchCode, branchName = string.Empty;
            //int employeeGid=0;

            try
            {
                cmd = new SqlCommand("pr_ifams_get_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@querytype", "GETRaiserDetail");
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(raiserId.ToString()));
                cmd.Parameters.AddWithValue("@Transferno", toa_number);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    lstObject.raiserId = Convert.ToInt32(dt.Rows[0]["employee_gid"]);
                    lstObject.raiserCode = Convert.ToString(dt.Rows[0]["employee_code"].ToString());
                    lstObject.raiserName = Convert.ToString(dt.Rows[0]["employee_name"].ToString());
                    lstObject.raiserBranchCode = Convert.ToString(dt.Rows[0]["branch_code"].ToString());
                    lstObject.raiserBranchName = Convert.ToString(dt.Rows[0]["branch_name"].ToString());
                    lstObject.receiverId = dt.Rows[0]["receiverid"].ToString();
                    lstObject.receiverCode = dt.Rows[0]["receiverCode"].ToString();
                    lstObject.receiverName = dt.Rows[0]["receivername"].ToString();
                    lstObject.receiverBranchName = dt.Rows[0]["receiverbranch"].ToString();
                    lstObject.receiverbranchid = Convert.ToInt32(dt.Rows[0]["branch_gid"]);
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
            return lstObject;
        }

        //For Transfer


        public string GetUserTypeTransfer(string id)
        {

            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_get_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@headergid", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = Convert.ToString(objCmnFunctions.GetLoginUserGid().ToString());
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "UserType";
                string result = (string)cmd.ExecuteScalar();
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


        //For Writeoff

        public string GetUserType(string id)
        {

            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_get_woaMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@headergid", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@empgid", SqlDbType.VarChar).Value = Convert.ToString(objCmnFunctions.GetLoginUserGid().ToString());
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "UserType";
                string result = (string)cmd.ExecuteScalar();
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

        /*22-11-2017 - Rollback purpose*/
        public IEnumerable<TransferMakerModel> GetTransferDetailnew(string Transferno)
        {
            List<TransferMakerModel> objModel = new List<TransferMakerModel>();
            try
            {

                TransferMakerModel obj = new TransferMakerModel();
                // GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "SelectView";
                cmd.Parameters.Add("@Transferno", SqlDbType.VarChar).Value = Transferno;
                cmd.Transaction = Tran;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                obj._toa_number = Transferno;
                foreach (DataRow row in dt.Rows)
                {
                    obj = new TransferMakerModel();
                    obj._gid = Convert.ToInt32(row["assetgid"].ToString());
                    obj._asset_id = row["assetid"].ToString();
                    obj._new_asset_id = row["newassetid"].ToString();
                    obj._AssetCode = row["assetcode"].ToString();
                    obj._AssetDesp = row["assetdesc"].ToString();
                    obj._loc_from = row["oldloc"].ToString();
                    obj._loc_to = row["newloc"].ToString();
                    // obj._fccc = row["FC"].ToString();
                    obj.queue_from = row["CC"].ToString();
                    obj._AssetCatCode = row["assetcat"].ToString();
                    obj._tfr_date = row["toadetail_tfr_date"].ToString();
                    obj._tfr_reason = row["toadetail_tfr_reason"].ToString();
                    obj._inw_no = row["toadetail_inw_no"].ToString();
                    obj.rectiAmt = getRetificationAmount(obj._gid.ToString(), obj._tfr_date.ToString());
                    objModel.Add(obj);
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
                // con.Close();
            }
        }


        public IEnumerable<WriteOffModel> GetWriteOffDetail(string Transferno)
        {
            List<WriteOffModel> objModel = new List<WriteOffModel>();
            try
            {
                WriteOffModel obj = new WriteOffModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_woaMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "SelectView";
                cmd.Parameters.Add("@Wrtno", SqlDbType.VarChar).Value = Transferno;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                obj._woa_number = Transferno;
                foreach (DataRow row in dt.Rows)
                {
                    obj = new WriteOffModel();
                    obj._assetdet_gid = row["assetdetails_gid"].ToString();
                    obj._gid = Convert.ToInt32(row["gid"].ToString());
                    obj._asset_id = row["assetid"].ToString();
                    obj._AssetCode = row["assetcode"].ToString();
                    obj._AssetDesp = row["assetdes"].ToString();
                    obj._loc = row["loc"].ToString();
                    obj._fccc = row["FC"].ToString();
                    obj.queue_from = row["cc_code"].ToString();
                    obj._asset_value = row["value"].ToString();
                    obj._wdv_value = Convert.ToDecimal(row["woadetail_wdv_value"].ToString());
                    obj._AssetCatCode = row["assetcat"].ToString();
                    obj._woa_reason = row["reason"].ToString();
                    obj._woa_date = row["woadate"].ToString();
                    obj._rectif_amount = Convert.ToString(getRetificationAmount(obj._assetdet_gid.ToString(), obj._woa_date.ToString()));
                    objModel.Add(obj);
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

        public void AbortTransfer(Models.TransferMakerModel tmm, string id)
        {

            try
            {
                GetConnection();
                tmm._toa_number = id;
                string toa_id = string.Empty;
                GetConnection();
                DataTable dt = new DataTable();
                dt = GetheaderDetails(tmm._toa_number);
                if (dt.Rows.Count > 0)
                {
                    toa_id = dt.Rows[0]["toaheader_gid"].ToString();
                }
                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "abort";
                cmd.Parameters.Add("@headergid", SqlDbType.VarChar).Value = toa_id;
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    string[,] ASScodes1 = new string[,]
	                        {                                                      
                                {"assetdetails_takenby","14"},                             
                                {"assetdetails_update_by",objCmnFunctions.GetLoginUserGid().ToString()},    
                                {"assetdetails_update_date",Convert.ToString(objCommonIUD.GetCurrentDate())},               
	                       };
                    string[,] ASSwhrcol = new string[,]
	                        {
                                {"assetdetails_gid", row["toadetail_assetdet_gid"].ToString()},
                                {"assetdetails_isremoved", "N"} ,   
                            };
                    string asstupdcomm = objCommonIUD.UpdateCommon(ASScodes1, ASSwhrcol, "fa_trn_tassetdetails");
                }
                string[,] codes1 = new string[,]
	                {
                           {"toaheader_isremoved","Y"} , 
                           {"toaheader_update_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},     
                           {"toaheader_update_date",Convert.ToString(objCommonIUD.GetCurrentDate())}                                      
	                };
                string[,] whrcol = new string[,]
	                {
                           {"toaheader_toa_number", tmm._toa_number .ToString()},
                           {"toaheader_isremoved", "N"}
                    };
                string updcomm = objCommonIUD.UpdateCommon(codes1, whrcol, "fa_trn_ttoaheader");
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }

        }

        public void AbortWriteoff(string id)
        {

            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                dt = GetwoaheaderDetails(id);
                if (dt.Rows.Count > 0)
                {
                    id = dt.Rows[0]["woaheader_gid"].ToString();
                }
                cmd = new SqlCommand("pr_ifams_trn_woaMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "abort";
                cmd.Parameters.Add("@headergid", SqlDbType.VarChar).Value = id;
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = row["woadetail_assetdet_gid"].ToString();
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "takenbyid";
                    string takenby = Convert.ToString(cmd.ExecuteScalar());

                    if (takenby == "11")
                    {
                        takenby = "14";
                    }
                    if (takenby == "23")
                    {
                        takenby = "21";
                    }
                    string[,] ASScodes1 = new string[,]
	                        {                                                      
                                {"assetdetails_takenby",takenby},                             
                                {"assetdetails_update_by",objCmnFunctions.GetLoginUserGid().ToString()},    
                                {"assetdetails_update_date",Convert.ToString(objCommonIUD.GetCurrentDate())},               
	                       };
                    string[,] ASSwhrcol = new string[,]
	                        {
                                 {"assetdetails_gid", row["woadetail_assetdet_gid"].ToString()},
                                 {"assetdetails_isremoved", "N"} ,   
                            };
                    string asstupdcomm = objCommonIUD.UpdateCommon(ASScodes1, ASSwhrcol, "fa_trn_tassetdetails");

                }
                string[,] codes1 = new string[,]
	                {
                           {"woaheader_isremoved","Y"}     ,
                           {"woaheader_update_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},     
                           {"woaheader_update_date",Convert.ToString(objCommonIUD.GetCurrentDate())},                                    
	                };
                string[,] whrcol = new string[,]
	                {
                           {"woaheader_gid",id .ToString()},
                           {"woaheader_isremoved", "N"}
                    };
                string updcomm = objCommonIUD.UpdateCommon(codes1, whrcol, "fa_trn_twoaheader");
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }

        }

        public string GetTrfNo(string id)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "toanumber";
                cmd.Parameters.Add("@headergid", SqlDbType.VarChar).Value = id;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    id = dt.Rows[0]["toaheader_toa_number"].ToString();
                }
                return id;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return string.Empty;
            }
            finally
            {
                con.Close();
            }
        }

        public string GetWrtNo(string id)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_woaMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "woanumber";
                cmd.Parameters.Add("@headergid", SqlDbType.VarChar).Value = id;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    id = dt.Rows[0]["woaheader_woa_number"].ToString();
                }
                return id;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return string.Empty;
            }
            finally
            {
                con.Close();
            }
        }

        public string GetStatusexcel(string assetdata, string valid, string action)
        {
            string result = string.Empty;
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_exceldataverification", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@chkdata", SqlDbType.VarChar).Value = valid.ToString();
                cmd.Parameters.Add("@assetdata", SqlDbType.VarChar).Value = assetdata.ToString();
                cmd.Parameters.Add("@Result", SqlDbType.VarChar).Value = action;
                result = (string)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return string.Empty;
            }
            finally
            {
                con.Close();
            }
            return result;
        }


        //For Upload Excel file
        public string ImportAssetDetails(DataTable assetData, string fileName, string assettype)
        {
            string Emp_Msg = "";
            String xmlstr = "";
            try
            {
                assetData.TableName = "AssetDetails";
                xmlstr = ConvertDatatableToXML(assetData);
                string modifiedXml = Regex.Replace(xmlstr,
                                                    @"<Date_x0028_dd-mm-yyyy_x0029_>(?<year>\d{4})-(?<month>\d{2})-(?<date>\d{2}).*?</Date_x0028_dd-mm-yyyy_x0029_>",
                                                    @"<Date_x0028_dd-mm-yyyy_x0029_>${month}/${date}/${year}</Date_x0028_dd-mm-yyyy_x0029_>",
                                                    RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                xmlstr = Regex.Replace(modifiedXml,
                                @"<Date_x0028_dd-mm-yyyy_x0029_>(?<year>\d{4})-(?<month>\d{2})-(?<date>\d{2}).*?</Date_x0028_dd-mm-yyyy_x0029_>",
                                @"<Date_x0028_dd-mm-yyyy_x0029_>${month}/${date}/${year}</Date_x0028_dd-mm-yyyy_x0029_>",
                                RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                DataTable dt = new DataTable();
                GetConnection();
                cmd = new SqlCommand("pr_Set_ImportFAAssetDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("@puploadfilename", fileName.Trim());
                cmd.Parameters.AddWithValue("@paction", "I");
                cmd.Parameters.AddWithValue("@pAssetType", assettype);
                cmd.Parameters.AddWithValue("@puploadby", Convert.ToInt32(objCmnFunctions.GetLoginUserGid().ToString()));
                cmd.Parameters.AddWithValue("@pXmlData", xmlstr);
                cmd.Parameters.Add("@pmsg", SqlDbType.VarChar, 10000);
                cmd.Parameters["@pmsg"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                Emp_Msg = Convert.ToString(cmd.Parameters["@pmsg"].Value);
                return Emp_Msg;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return string.Empty;
            }
        }



        public string ConvertDatatableToXML(DataTable dt)
        {
            MemoryStream str = new MemoryStream();
            dt.WriteXml(str, true);
            str.Seek(0, SeekOrigin.Begin);
            StreamReader sr = new StreamReader(str);
            string xmlstr;
            xmlstr = sr.ReadToEnd();
            return (xmlstr);
        }
        //public string InsertTransfer(DataTable assetdata, string filename)
        //{
        //    try
        //    {
        //        GetConnection();
        //        List<TransferMakerModel> tmmList = new List<TransferMakerModel>();
        //        TransferMakerModel tmm;

        //        string status = string.Empty;
        //        string hsncode = string.Empty;
        //        DataTable dt = new DataTable();
        //        cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = "DRAFT";
        //        cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "status";
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            status = row["status_flag"].ToString();
        //        }
        //        string toanumber = objCmnFunctions.GetSequenceNoFam("TOA");
        //        for (int i = 0; i == 0; i++)
        //        {
        //            tmm = new TransferMakerModel();

        //            tmm._upld_fname = filename.ToString();
        //            tmm._toa_number = toanumber;
        //            string[,] codes = new string[,]
        //            { 
        //                     {"toaheader_toa_number",tmm._toa_number.ToString()},
        //                     {"toaheader_maker_id",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
        //                     {"toaheader_maker_date",Convert.ToString(objCommonIUD.GetCurrentDate())},                            
        //                     {"toaheader_upld_date",Convert.ToString(objCommonIUD.GetCurrentDate())},
        //                     {"toaheader_tfr_date",Convert.ToString(objCommonIUD.GetCurrentDate())},                            
        //                     {"toaheader_upld_fname",tmm._upld_fname.ToString()},
        //                     {"toaheader_isremoved","N"},
        //                     {"toaheader_no_records",Convert.ToString(assetdata.Rows.Count)},
        //                     {"toaheader_tfr_status",status.ToString()},
        //                     {"toaheader_jobcode",tmm._toa_number.ToString()},
        //                     {"toaheader_insert_by",objCmnFunctions.GetLoginUserGid().ToString()},
        //                     {"toaheader_insert_date",objCommonIUD.GetCurrentDate()} 
        //            };
        //            string instComm = objCommonIUD.InsertCommon(codes, "fa_trn_ttoaheader");
        //        }
        //        DataTable detaildt1 = new DataTable();
        //        detaildt1 = GetheaderDetails(toanumber);
        //        string _toa_number = string.Empty;
        //        for (int r = 0; r < detaildt1.Rows.Count; r++)
        //        {
        //            _toa_number = detaildt1.Rows[0]["toaheader_gid"].ToString().Trim();
        //        }

        //        DataTable dttm = new DataTable();
        //        dttm = (DataTable)assetdata;
        //        for (int i = 0; i < dttm.Rows.Count; i++)
        //        {
        //            GetConnection();
        //            tmm = new TransferMakerModel();
        //            tmm._asset_id = dttm.Rows[i]["Asset ID"].ToString().Trim();
        //            tmm._fccc = dttm.Rows[i]["CC"].ToString().Trim();
        //            tmm._loc_from = dttm.Rows[i]["Current Branch"].ToString().Trim();
        //            tmm._loc_to = dttm.Rows[i]["New Branch"].ToString().Trim();
        //            tmm._tfr_date = convertoDate(dttm.Rows[i]["Date of Transfer(dd-mm-yyyy)"].ToString().Trim());
        //            tmm._tfr_reason = dttm.Rows[i]["Reason"].ToString().Trim();
        //            tmm._inw_no = dttm.Rows[i]["Inward No"].ToString().Trim();
        //            tmm._fccc_gid = Getfccc(tmm._fccc).ToString();
        //            hsncode=dttm.Rows[i]["Hsn Code"].ToString().Trim();
        //            DataTable detaildt2 = new DataTable();
        //            detaildt2 = GetDetails(tmm._asset_id);
        //            for (int r = 0; r < detaildt2.Rows.Count; r++)
        //            {
        //                tmm._asset_value = detaildt2.Rows[r]["assetdetails_asset_value"].ToString().Trim();
        //                tmm._AssetCode = detaildt2.Rows[r]["asset_code"].ToString().Trim();
        //                tmm._gid = Convert.ToInt32(detaildt2.Rows[r]["assetdetails_asset_code"].ToString().Trim());
        //                tmm._assetdet_gid = detaildt2.Rows[r]["assetdetails_gid"].ToString().Trim();
        //            }
        //            tmm._loc_to = GetlocDetails(tmm._loc_to).ToString();
        //            tmm._loc_from = GetlocDetails(tmm._loc_from).ToString();
        //            tmm.hsnid = Convert.ToInt32(GethsnDetails(hsncode));
        //            string[,] codes = new string[,]
        //            {
        //                     {"toadetail_toahead_gid",_toa_number.ToString()},
        //                     {"toadetail_assetdet_gid",tmm._assetdet_gid.ToString()},
        //                     {"toadetail_asset_value", tmm._asset_value.ToString()},
        //                     {"toadetail_tfr_value", tmm._asset_value.ToString()},
        //                     {"toadetail_tfr_date",tmm._tfr_date},                          
        //                     {"toadetail_tfr_reason",tmm._tfr_reason.ToString()},
        //                     {"toadetail_rectif_amount","0"},
        //                     {"toadetail_value_date",Convert.ToString(objCommonIUD.GetCurrentDate())},
        //                     {"toadetail_fccc_gid",tmm._fccc_gid.ToString()},
        //                     {"toadetail_loc_from",tmm._loc_from.ToString()},
        //                     {"toadetail_loc_to",tmm._loc_to.ToString()},
        //                     {"toadetail_inw_no",tmm._inw_no.ToString()},
        //                     {"toadetail_isremoved","N"},
        //                     {"toadetail_insert_by",objCmnFunctions.GetLoginUserGid().ToString()},
        //                     {"toadetail_insert_date",objCommonIUD.GetCurrentDate()},
        //                     {"toadetail_hsngid",tmm.hsnid.ToString()}
        //            };
        //            string tblname = "fa_trn_ttoadetail";
        //            string instComm = objCommonIUD.InsertCommon(codes, tblname);

        //            string[,] ASScodes1 = new string[,]
        //                    {                                                      
        //                        {"assetdetails_takenby","10"},                           
        //                        {"assetdetails_update_by",objCmnFunctions.GetLoginUserGid().ToString()},    
        //                        {"assetdetails_update_date",Convert.ToString(objCommonIUD.GetCurrentDate())},               
        //                   };
        //            string[,] ASSwhrcol = new string[,]
        //                    {
        //                            {"assetdetails_gid", tmm._assetdet_gid.ToString()},
        //                            {"assetdetails_isremoved", "N"} ,   
        //                    };
        //            string ASStblname1 = "fa_trn_tassetdetails";
        //            string ASSupdcomm = objCommonIUD.UpdateCommon(ASScodes1, ASSwhrcol, ASStblname1);
        //        }


        //        return "Success";
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //        return string.Empty;
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //}

        public int GetCheckerDetails(int makerid)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_sereview", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = makerid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getsuperlist";
                int result = (int)cmd.ExecuteScalar();
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {

            }
        }

        public DataTable GetDetails(string assetid)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();

                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = assetid;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "assetid";
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

            }
        }

        public int Getfccc(string fccc)
        {
            try
            {
                GetConnection();
                int res;
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@fccc", SqlDbType.VarChar).Value = fccc;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "fccc";
                res = Convert.ToInt32(cmd.ExecuteScalar());
                return res;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {

            }
        }

        public DataTable GetheaderDetails(string headerid)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();

                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@headergid", SqlDbType.VarChar).Value = headerid;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "headergid";
                //string result = (string)cmd.ExecuteScalar();
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

            }
        }

        /*22-11-2017 Roll back purpose*/
        public DataTable GetheaderDetailsnew(string headerid)
        {
            DataTable dt = new DataTable();
            try
            {
                // GetConnection();

                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@headergid", SqlDbType.VarChar).Value = headerid;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "headergid";
                //string result = (string)cmd.ExecuteScalar();
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

            }
        }

        public DataTable GetwoaheaderDetails(string headergid)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();

                cmd = new SqlCommand("pr_ifams_trn_woaMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@headergid", SqlDbType.VarChar).Value = headergid;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "headergid";
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

            }
        }

        public int GetlocDetails(string loc)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@fccc", SqlDbType.VarChar).Value = loc;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "locid";
                int result = (int)cmd.ExecuteScalar();
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {

            }
        }

        public string getfilename(string toanumber)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@headergid", SqlDbType.VarChar).Value = toanumber;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "filename";
                string result = (string)cmd.ExecuteScalar();
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return string.Empty;
            }
            finally
            {

            }

        }

        public string GetRef(string ref_name)
        {
            try
            {
                da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "refno";
                cmd.Parameters.Add("@headergid", SqlDbType.VarChar).Value = ref_name.ToString();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                { ref_name = dt.Rows[0][0].ToString(); }
                return ref_name;
            }
            catch (Exception e)
            {
                objErrorLog.WriteErrorLog(e.Message.ToString(), e.ToString());
                return string.Empty;
            }
            finally { }
        }

        public string GetAttachType()
        {
            try
            {
                GetConnection();
                da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                string attachtype = string.Empty;
                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "attachtype";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                    attachtype = dt.Rows[0][0].ToString();
                return attachtype;
            }
            catch (Exception e)
            {
                objErrorLog.WriteErrorLog(e.Message.ToString(), e.ToString());
                return string.Empty;
            }
            finally { }
        }

        public string save_attachment(TransferMakerModel model) //FA Attachment File Save Transfer(s)
        {

            try
            {
                string[,] col = new string[,]
	                {
                             {"attachment_ref_gid",model._gid.ToString()},
                             {"attachment_ref_flag",GetRef("TOA")},
                             {"attachment_filename", model._attach_file.ToString()},
                             {"attachment_attachmenttype_gid", GetAttachType().ToString()},
                             {"attachment_desc",model._attach_desc.ToString()},                            
                             {"attachment_date",Convert.ToString(objCommonIUD.GetCurrentDate())},
                             {"attachment_by",objCmnFunctions.GetLoginUserGid().ToString()},
                             {"attachment_isremoved","N"},
                           
	                };
                string inst = objCommonIUD.InsertCommon(col, "iem_trn_tattachment");
                return "success";
            }
            catch (Exception e)
            {
                objErrorLog.WriteErrorLog(e.Message.ToString(), e.ToString());
                return string.Empty;
            }
            finally
            {

            }
        }

        public string save_attachment(WriteOffModel model)
        {

            try
            {
                string[,] col = new string[,]
	                {
                             {"attachment_ref_gid",model._gid.ToString()},
                             {"attachment_ref_flag",GetRef("WOA")},
                             {"attachment_filename", model._attach_file.ToString()},
                             {"attachment_attachmenttype_gid", GetAttachType().ToString()},
                             {"attachment_desc",model._attach_desc.ToString()},                            
                             {"attachment_date",Convert.ToString(objCommonIUD.GetCurrentDate())},
                             {"attachment_by",objCmnFunctions.GetLoginUserGid().ToString()},
                             {"attachment_isremoved","N"},
                           
	                };
                string inst = objCommonIUD.InsertCommon(col, "iem_trn_tattachment");
                return "success";
            }
            catch (Exception e)
            {
                objErrorLog.WriteErrorLog(e.Message.ToString(), e.ToString());
                return string.Empty;
            }
            finally
            {

            }
        }

        public string Delete_attach(int attachId)
        {
            string result;
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {
                string[,] codes = new string[,]
	                {
                         {"attachment_isremoved", "Y"}	            
                    };
                string[,] whrcol = new string[,]
	                    {
                          {"attachment_gid", attachId.ToString ()}
                        };
                string deletetbl = delecomm.DeleteCommon(codes, whrcol, "iem_trn_tattachment");
                result = "success";
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return string.Empty;
            }
            finally
            {

            }

        }

        public List<Attachment> Getattachment(string id, string refe, string type)
        {
            TransferMakerModel ob = new TransferMakerModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                List<Attachment> obj_ = new List<Attachment>();
                Attachment newob = new Attachment();
                cmd = new SqlCommand("attachment_get", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@gid", id);
                cmd.Parameters.AddWithValue("@ref", refe);
                cmd.Parameters.AddWithValue("@type", type);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        newob = new Attachment();
                        newob.attachGid = dt.Rows[i]["attachment_gid"].ToString();
                        newob.attachedDate = dt.Rows[i]["attachment_date"].ToString();
                        newob.fileName = dt.Rows[i]["attachment_filename"].ToString();
                        newob.description = dt.Rows[i]["attachment_desc"].ToString();
                        newob.employee_gid = dt.Rows[i]["attachment_by"].ToString();
                        obj_.Add(newob);
                    }
                }
                ob._attach_list = obj_;
                return ob._attach_list;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return ob._attach_list;
            }

        }

        public IEnumerable<TransferMakerModel> GetTrfDetailforChecker(string id)
        {
            List<TransferMakerModel> objModel = new List<TransferMakerModel>();
            try
            {

                TransferMakerModel obj = new TransferMakerModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                // cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "SelectCheckerSumm";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new TransferMakerModel();
                    obj._tfr_status = Getststus(obj, row["toaheader_gid"].ToString());
                    obj._gid = Convert.ToInt32(row["toaheader_gid"].ToString());
                    obj._toa_number = row["toaheader_toa_number"].ToString();
                    obj._tfr_date = Convert.ToString(row["toaheader_tfr_date"].ToString());
                    obj._no_records = Convert.ToInt64(row["toaheader_no_records"].ToString());
                    obj._upld_fname = row["toaheader_upld_fname"].ToString();
                    objModel.Add(obj);
                }
                return objModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return objModel;
            }
            finally
            {
                con.Close();
            }
        }

        public IEnumerable<WriteOffModel> GetWrtDetailforChecker(string id)
        {
            List<WriteOffModel> objModel = new List<WriteOffModel>();
            try
            {
                WriteOffModel obj = new WriteOffModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_woaMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "SELECTSearchDropdown";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new WriteOffModel();
                    obj._woa_status = Getwrtststus(obj, row["woaheader_gid"].ToString());
                    obj._gid = Convert.ToInt32(row["woaheader_gid"].ToString());
                    obj._woa_number = row["woaheader_woa_number"].ToString();
                    obj._woa_date = Convert.ToString(row["woaheader_trans_date"].ToString());
                    obj._no_records = Convert.ToInt64(row["woaheader_nof_records"].ToString());
                    obj._upld_fname = row["woaheader_upld_fname"].ToString();
                    objModel.Add(obj);
                }
                return objModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return objModel;
            }
            finally
            {
                con.Close();
            }
        }

        public TransferMakerModel GetFccc()
        {
            TransferMakerModel obj = new TransferMakerModel();
            try
            {

                FcccMaster obj_getvalues;
                obj.fcccList = new List<FcccMaster>();
                GetConnection();

                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_tfccc", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@action", "selectforficc");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj_getvalues = new FcccMaster()
                    {
                        fcccCode = row["fccc_gid"].ToString(),
                        fcccGid = row["fccc_code"].ToString(),
                        fcccName = row["fccc_name"].ToString(),

                    };
                    obj.fcccList.Add(obj_getvalues);
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
            return obj;
        }

        public TransferMakerModel GetFcccSerach(FcccMaster fc)
        {
            TransferMakerModel obj = new TransferMakerModel();
            try
            {

                FcccMaster obj_getvalues;
                obj.fcccList = new List<FcccMaster>();
                GetConnection();

                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_tfccc", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fccccode", fc.fcccGid);
                cmd.Parameters.AddWithValue("@fccc_name", fc.fcccName);
                cmd.Parameters.AddWithValue("@action", "Search");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj_getvalues = new FcccMaster()
                    {
                        fcccCode = row["fccc_gid"].ToString(),
                        fcccGid = row["fccc_code"].ToString(),
                        fcccName = row["fccc_name"].ToString(),
                    };
                    obj.fcccList.Add(obj_getvalues);
                }


            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            finally
            {
                con.Close();
            } return obj;
        }

        public string GetFcccSerachText(string lsFccc)
        {
            string fccccode = string.Empty;
            try
            {
                GetConnection();

                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_tfccc", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fccccode", lsFccc);
                cmd.Parameters.AddWithValue("@action", "change");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    fccccode = dt.Rows[0]["fccc_code"].ToString();
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
            return fccccode;
        }
        public string ApprovalStatus(string id, string status, string remarks)
        {
            string Result = string.Empty;
            try
            {
                GetConnection();

                DataTable dt = new DataTable();
                cmd = new SqlCommand("Pr_fa_set_TransferChkrApprove", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TOANumber", id.ToString());
                cmd.Parameters.AddWithValue("@Remarks", remarks.ToString());
                //cmd.Parameters.AddWithValue("@status",status.ToString());
                cmd.Parameters.AddWithValue("@action", status.ToString());
                cmd.Parameters.AddWithValue("@Userid ", objCmnFunctions.GetLoginUserGid().ToString());
                //cmd.Parameters.AddWithValue("@newBranchcode", );
                cmd.CommandTimeout = 0;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Result = dt.Rows[0][0].ToString();
                }
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                Result = ex.Message.ToString();
            }

            finally
            {
                con.Close();
            }
            return Result;
        }
        #region
        /* public string ApprovalStatus(string id, string status, string remarks)
        {
            try
            {
                string _toa_number = string.Empty;
                string _toa_maker = string.Empty;
                string oracleCCcode = string.Empty;
                string oracleBScode = string.Empty;
                string branchlegacycode = string.Empty;
                string oldoracleCCcode = string.Empty;
                string oldoracleBScode = string.Empty;
                string oldbranchlegacycode = string.Empty;
                TransferMakerModel tmm = new TransferMakerModel();
                tmm.TModel = GetTransferDetail(id).ToList();
                DataTable detaildt1 = new DataTable();
                detaildt1 = GetheaderDetails(id);
                for (int r = 0; r < detaildt1.Rows.Count; r++)
                {
                    _toa_number = detaildt1.Rows[0]["toaheader_gid"].ToString().Trim();
                    _toa_maker = detaildt1.Rows[0]["toaheader_maker_id"].ToString().Trim();
                }
                //cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@headergid", SqlDbType.VarChar).Value = id;
                //cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "toanumber";
                //string transfer_number = Convert.ToString(cmd.ExecuteScalar());
                DataTable dt2 = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@headergid", SqlDbType.VarChar).Value = "TOA".Trim();
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "refno";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt2);
                string refflag = string.Empty;
                foreach (DataRow row in dt2.Rows)
                {
                    refflag = row["ref_flag"].ToString();
                }
                if (status == "REJECTED")
                {
                    cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "abort";
                    cmd.Parameters.Add("@headergid", SqlDbType.VarChar).Value = _toa_number;
                    da = new SqlDataAdapter(cmd);
                    dt2 = new DataTable();
                    da.Fill(dt2);
                    foreach (DataRow row in dt2.Rows)
                    {
                        string[,] ASScodes1 = new string[,]
	                        {
                                
                                {"assetdetails_update_by",objCmnFunctions.GetLoginUserGid().ToString()},                      
                                {"assetdetails_update_date",objCommonIUD.GetCurrentDate()}, 
                                {"assetdetails_takenby", "14"} ,   
                               
	                       };
                        string[,] ASSwhrcol = new string[,]
	                        {
                                    {"assetdetails_gid", row["toadetail_assetdet_gid"].ToString()}, 
                                    {"assetdetails_isremoved", "N"} ,   
                            };
                        string assupdcmd = objCommonIUD.UpdateCommon(ASScodes1, ASSwhrcol, "fa_trn_tassetdetails");
                    }
                    string[,] colm = new string[,]
                    {
                             {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                             {"queue_action_status","2"},
                             {"queue_action_date",objCommonIUD.GetCurrentDate()}   ,
                             {"queue_isremoved","Y"},  
                              {"queue_action_remark",remarks},
                                              
                    };
                    string[,] whrecol = new string[,]
                    {
                              {"queue_ref_gid", _toa_number} ,    
                              {"queue_ref_flag", refflag}                                           
                    };
                    string updcmd = objCommonIUD.UpdateCommon(colm, whrecol, "iem_trn_tqueue");
                }
                else
                {

                    string tfrdate = "";

                    if (tmm.TModel.Count > 0)
                    {
                        for (int i = 0; i < tmm.TModel.Count; i++)
                        {
                            tmm._new_asset_id = objCmnFunctions.generateAssetid(tmm.TModel[i]._loc_to, tmm.TModel[i]._AssetCode);
                            cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@fccc", SqlDbType.VarChar).Value = tmm.TModel[i]._loc_to;
                            cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "locid";
                            string locCode1 = string.Empty;
                            da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            da.Fill(dt);
                            //GET ASSET DETAILS OF PREVIOUS ASSET
                            if (dt.Rows.Count > 0)
                                locCode1 = dt.Rows[0]["branch_gid"].ToString();
                            dt = new DataTable();
                            cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = tmm.TModel[i]._asset_id.ToString();
                            cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "newassetidinsert";
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dt);
                            cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = tmm.TModel[i]._asset_id.ToString();
                            cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "assetdetails";
                            string _asset_gid_ = Convert.ToString(cmd.ExecuteScalar());


                            foreach (DataRow row in dt.Rows)
                            {

                                tfrdate = row["assetdetails_tfr_date"].ToString();
                                // string oldassetgid = row["assetdetails_gid"].ToString();
                                //INSERT NEW ASSET IN ASSET DETAILS TABLE
                                string[,] col = new string[,]{	                            
                                     {"assetdetails_assetdet_id",string.IsNullOrEmpty(tmm._new_asset_id.ToString())? string.Empty :tmm._new_asset_id.ToString()}
                                    ,{"assetdetails_entity_gid","1"}
                                    ,{"assetdetails_physical_assetid", Convert.ToInt32(string.IsNullOrEmpty(row["assetdetails_physical_assetid"].ToString()) ? "0" : row["assetdetails_physical_assetid"]).ToString()}
                                    ,{"assetdetails_qty","1"}
                                    ,{"assetdetails_barcode",row["assetdetails_barcode"].ToString()}
                                    ,{"assetdetails_asset_groupid", string.IsNullOrEmpty(row["assetdetails_asset_groupid"].ToString())? string.Empty :row["assetdetails_asset_groupid"].ToString()}
                                    ,{"assetdetails_asset_code",Convert.ToInt32(string.IsNullOrEmpty(row["assetdetails_asset_code"].ToString()) ? "0" : row["assetdetails_asset_code"]).ToString()}
                                    ,{"assetdetails_asset_value",Convert.ToDecimal(string.IsNullOrEmpty(row["assetdetails_asset_value"].ToString()) ? "0.00" : row["assetdetails_asset_value"]).ToString()}                                
                                    ,{"assetdetails_asset_description",row["assetdetails_asset_description"].ToString()}
                                    ,{"assetdetails_cap_date",string.IsNullOrEmpty(row["assetdetails_cap_date"].ToString()) ? string.Empty : convertoDate (row["assetdetails_cap_date"].ToString())}
                                    ,{"assetdetails_po_number",row["assetdetails_po_number"].ToString()}
                                    ,{"assetdetails_tfr_status","N"}
                                    ,{"assetdetails_recon_reference",row["assetdetails_recon_reference"].ToString()}
                                    ,{"assetdetails_active_status","Y"}
                                    ,{"assetdetails_sale_status","N"}
                                    ,{"assetdetails_not_5kcase",row["assetdetails_not_5kcase"].ToString()}
                                    ,{"assetdetails_trf_value",Convert.ToDecimal(string.IsNullOrEmpty(row["assetdetails_trf_value"].ToString()) ? row["assetdetails_asset_value"] : row["assetdetails_trf_value"]).ToString()}
                                    ,{"assetdetails_asset_owner",string.IsNullOrEmpty(row["assetdetails_asset_owner"].ToString()) ? "F" : row["assetdetails_asset_owner"].ToString()}
                                    ,{"assetdetails_trn_date",string.IsNullOrEmpty(row["assetdetails_trn_date"].ToString()) ? string.Empty : convertoDate (row["assetdetails_trn_date"].ToString())}
                                    ,{"assetdetails_reduced_value", string.IsNullOrEmpty(row["assetdetails_reduced_value"].ToString()) ? "0" : row["assetdetails_reduced_value"].ToString()}
                                    ,{"assetdetails_lease_startdate", string.IsNullOrEmpty(row["assetdetails_lease_startdate"].ToString()) ? string.Empty : convertoDate (row["assetdetails_lease_startdate"].ToString())}
                                    ,{"assetdetails_lease_enddate",string.IsNullOrEmpty(row["assetdetails_lease_enddate"].ToString()) ? string.Empty : convertoDate (row["assetdetails_lease_enddate"].ToString())}
                                    ,{"assetdetails_dep_onhold",row["assetdetails_dep_onhold"].ToString()}
                                    ,{"assetdetails_spoke_asset",row["assetdetails_spoke_asset"].ToString()}
                                    ,{"assetdetails_impair_asset",row["assetdetails_impair_asset"].ToString()}
                                    ,{"assetdetails_dep_rate",string.IsNullOrEmpty(row["assetdetails_dep_rate"].ToString()) ? "0" : row["assetdetails_dep_rate"].ToString()}
                                    ,{"assetdetails_asset_serialno",row["assetdetails_asset_serialno"].ToString()}
                                    ,{"assetdetails_physical_idrelease",row["assetdetails_physical_idrelease"].ToString()}                            
                                    ,{"assetdetails_addate",objCommonIUD.GetCurrentDate()}
                                    ,{"assetdetails_branch_gid",locCode1.ToString()}
                                    ,{"assetdetails_isremoved","N"}                                      
                                    ,{"assetdetails_insert_by",objCmnFunctions.GetLoginUserGid().ToString()}                   
                                    ,{"assetdetails_insert_date",objCommonIUD.GetCurrentDate()}
                                    ,{"assetdetails_assetid_source","3"}                                    
                                    ,{"assetdetails_takenby","14"}  
                                    ,{"assetdetails_trf_from",string.IsNullOrEmpty(_asset_gid_.ToString())? string.Empty :_asset_gid_.ToString()} 
                                    ,{"assetdetails_assetcode_changedate",string.IsNullOrEmpty(row["assetdetails_assetcode_changedate"].ToString()) ? string.Empty : convertoDate (row["assetdetails_assetcode_changedate"].ToString())}
                                    ,{"assetdetails_assetcode_changeid",string.IsNullOrEmpty(row["assetdetails_assetcode_changeid"].ToString()) ? string.Empty : row["assetdetails_assetcode_changeid"].ToString()}
                                    ,{"assetdetails_assetcode_changestatus",string.IsNullOrEmpty(row["assetdetails_assetcode_changestatus"].ToString()) ? "N" : row["assetdetails_assetcode_changestatus"].ToString()}
                                    ,{"assetdetails_capnew_date",string.IsNullOrEmpty(row["assetdetails_capnew_date"].ToString()) ? string.Empty :convertoDate (  row["assetdetails_capnew_date"].ToString())}
                                    ,{"assetdetails_capold_date",string.IsNullOrEmpty(row["assetdetails_capold_date"].ToString()) ? string.Empty :convertoDate (  row["assetdetails_capold_date"].ToString())}
                                    ,{"assetdetails_cbf_gid",string.IsNullOrEmpty(row["assetdetails_cbf_gid"].ToString()) ? string.Empty : row["assetdetails_cbf_gid"].ToString()}
                                    ,{"assetdetails_cbfnum",string.IsNullOrEmpty(row["assetdetails_cbfnum"].ToString()) ? string.Empty : row["assetdetails_cbfnum"].ToString()}
                                    ,{"assetdetails_ecf_gid",string.IsNullOrEmpty(row["assetdetails_ecf_gid"].ToString()) ? string.Empty : row["assetdetails_ecf_gid"].ToString()}
                                    ,{"assetdetails_ecfnum",string.IsNullOrEmpty(row["assetdetails_ecfnum"].ToString()) ? string.Empty : row["assetdetails_ecfnum"].ToString()}
                                    ,{"assetdetails_imp_date",string.IsNullOrEmpty(row["assetdetails_imp_date"].ToString()) ? string.Empty : convertoDate (row["assetdetails_imp_date"].ToString())}
                                    ,{"assetdetails_invoice_gid",string.IsNullOrEmpty(row["assetdetails_invoice_gid"].ToString()) ? string.Empty : row["assetdetails_invoice_gid"].ToString()}
                                    ,{"assetdetails_inwdetailgid",string.IsNullOrEmpty(row["assetdetails_inwdetailgid"].ToString()) ? string.Empty : row["assetdetails_inwdetailgid"].ToString()}
                                    ,{"assetdetails_inwheadergid",string.IsNullOrEmpty(row["assetdetails_inwheadergid"].ToString()) ? string.Empty : row["assetdetails_inwheadergid"].ToString()}
                                    ,{"assetdetails_ponum",string.IsNullOrEmpty(row["assetdetails_ponum"].ToString()) ? string.Empty : row["assetdetails_ponum"].ToString()}
                                    ,{"assetdetails_sale_date",string.IsNullOrEmpty(row["assetdetails_sale_date"].ToString()) ? string.Empty :convertoDate ( row["assetdetails_sale_date"].ToString())}
                                    ,{"assetdetails_sale_id",string.IsNullOrEmpty(row["assetdetails_sale_id"].ToString()) ? string.Empty : row["assetdetails_sale_id"].ToString()}
                                    ,{"assetdetails_status",string.IsNullOrEmpty(row["assetdetails_status"].ToString()) ? "0" : row["assetdetails_status"].ToString()}
                                    ,{"assetdetails_tfr_date",string.IsNullOrEmpty(row["assetdetails_tfr_date"].ToString()) ? string.Empty :convertoDate( row["assetdetails_tfr_date"].ToString())}
                                    ,{"assetdetails_tfr_id",string.IsNullOrEmpty(row["assetdetails_tfr_id"].ToString()) ? string.Empty : row["assetdetails_tfr_id"].ToString()}
                                    ,{"assetdetails_trf_reason",string.IsNullOrEmpty(row["assetdetails_trf_reason"].ToString()) ? string.Empty : row["assetdetails_trf_reason"].ToString()}
                                    ,{"assetdetails_trf_to",string.Empty}
                                    ,{"assetdetails_update_by",string.Empty}
                                    ,{"assetdetails_update_date",string.Empty}
                                    ,{"assetdetails_upld_ref",string.Empty}
                                    ,{"assetdetails_upld_status",string.IsNullOrEmpty(row["assetdetails_upld_status"].ToString()) ? "N" : row["assetdetails_upld_status"].ToString()}
                                    ,{"assetdetails_wrt_date",string.IsNullOrEmpty(row["assetdetails_wrt_date"].ToString()) ? string.Empty :convertoDate ( row["assetdetails_wrt_date"].ToString())}
                                    ,{"assetdetails_wrt_id",string.IsNullOrEmpty(row["assetdetails_wrt_id"].ToString()) ? string.Empty : row["assetdetails_wrt_id"].ToString()}
                                    ,{"assetdetails_wrt_status",string.IsNullOrEmpty(row["assetdetails_wrt_status"].ToString()) ? "N" : row["assetdetails_wrt_status"].ToString()}
	                         };
                                string inst = objCommonIUD.InsertCommon(col, "fa_trn_tassetdetails");
                            }

                            DataTable detaildt2 = new DataTable();
                            detaildt2 = GetDetails(tmm.TModel[i]._asset_id.ToString());
                            for (int r = 0; r < detaildt2.Rows.Count; r++)
                            {
                                tmm._assetdet_gid = detaildt2.Rows[r]["assetdetails_gid"].ToString().Trim();
                            }


                            string newassetgid = string.Empty;
                            //DataTable detaildtnew = new DataTable();
                            //cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                            //cmd.CommandType = CommandType.StoredProcedure;
                            //cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = tmm._new_asset_id.ToString();//assetdetid
                            //cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "@assetid";
                            //da = new SqlDataAdapter(cmd);
                            //da.Fill(detaildtnew);
                            //for (int n = 0; n < detaildtnew.Rows.Count; n++ )
                            //{
                            //    newassetgid = detaildtnew.Rows[n]["assetdetails_gid"].ToString().Trim();
                            //}


                            DataTable detaildtnewgid = new DataTable();
                            detaildtnewgid = GetDetails(tmm._new_asset_id.ToString().ToString());
                            for (int rn = 0; rn < detaildt2.Rows.Count; rn++)
                            {
                                newassetgid = detaildtnewgid.Rows[rn]["assetdetails_gid"].ToString().Trim();
                            }


                            //dep

                            DataTable dtdep = new DataTable();
                            cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@action", "Totdep");
                            cmd.Parameters.AddWithValue("@assetgid", tmm._assetdet_gid);
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dtdep);
                            foreach (DataRow rowdep in dtdep.Rows)
                            {
                                tmm.queue_from = rowdep["depreciation_amount"].ToString();
                            }
                            decimal sumassetdep = Convert.ToDecimal(tmm.queue_from.ToString());


                            //depupdate
                            string[,] coldepupdate = new string[,]
                            {
                                {"depreciation_assetdet_gid", newassetgid},
                            };

                            string[,] depnewwhrcol = new string[,]
	                        {
                                    {"depreciation_assetdet_gid", tmm._assetdet_gid.ToString()} , 
                                   
                            };
                            string depupdatetbname = "fa_trn_tdepreciation";
                            string depnewupdcomm = objCommonIUD.UpdateCommon(coldepupdate, depnewwhrcol, depupdatetbname);


                            DataTable dtwdv = new DataTable();
                            cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@action", "WDV");
                            cmd.Parameters.AddWithValue("@assetid", tmm._assetdet_gid);
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dtwdv);

                            //Transfer Date purpose
                            DataTable dttdate = new DataTable();
                            cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@querytype", "TOAdate");
                            cmd.Parameters.AddWithValue("@assetid", tmm._assetdet_gid);
                            cmd.Parameters.AddWithValue("@Transferno", _toa_number);
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dttdate);
                            foreach (DataRow rowdat in dttdate.Rows)
                            {
                                tfrdate = Convert.ToString(rowdat["toadetail_tfr_date"].ToString());

                            }


                            //DateTime dtLeaseEnd1 =
                            DateTime dtBranchLaunch = Convert.ToDateTime("01-01-1900");
                            DateTime soacaptilisationda = Convert.ToDateTime("01-01-1900");
                            decimal AssetValue = 0;
                            string sAssetCode = string.Empty;
                            string cNot5000Case = string.Empty;
                            DateTime dtLeaseStart = Convert.ToDateTime("01-01-1900");
                            DateTime dtLeaseEnd = Convert.ToDateTime("01-01-1900");
                            string sDepType = string.Empty;
                            decimal dDepRate = 0;
                            string sAssetGroup = string.Empty;
                            //   DateTime dtTillDate = DateTime.Now;
                            DateTime dtTillDate = Convert.ToDateTime(tfrdate.ToString());
                            string sPONumber = string.Empty;
                            string sFICCRef = string.Empty;
                            string sAsset_GroupId = string.Empty;
                            decimal dTransfer_value = 0;
                            string CanDepreciateFully = string.Empty;
                            Int32 dDepDevRate = 0;
                            string sBranch1 = string.Empty;
                            string sBranch2 = string.Empty;
                            foreach (DataRow row1 in dtwdv.Rows)
                            {
                                dtBranchLaunch = Convert.ToDateTime(string.IsNullOrEmpty(row1["branch_start_date"].ToString()) ? "0" : row1["branch_start_date"]);
                                soacaptilisationda = Convert.ToDateTime(string.IsNullOrEmpty(row1["assetdetails_cap_date"].ToString()) ? "01-01-1900" : row1["assetdetails_cap_date"]);
                                AssetValue = Convert.ToDecimal(string.IsNullOrEmpty(row1["assetdetails_asset_value"].ToString()) ? "0" : row1["assetdetails_asset_value"]);
                                sAssetCode = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_asset_code"].ToString()) ? "0" : row1["assetdetails_asset_code"]);
                                cNot5000Case = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_not_5kcase"].ToString()) ? "0" : row1["assetdetails_not_5kcase"]);
                                dtLeaseStart = Convert.ToDateTime(string.IsNullOrEmpty(row1["assetdetails_lease_startdate"].ToString()) ? "01-01-1900" : row1["assetdetails_lease_startdate"]);
                                dtLeaseEnd = Convert.ToDateTime(string.IsNullOrEmpty(row1["assetdetails_lease_enddate"].ToString()) ? "01-01-1900" : row1["assetdetails_lease_enddate"]);
                                sDepType = Convert.ToString(string.IsNullOrEmpty(row1["asset_dep_type"].ToString()) ? "0" : row1["asset_dep_type"]);
                                dDepRate = Convert.ToDecimal(string.IsNullOrEmpty(row1["asset_dep_rate"].ToString()) ? "0" : row1["asset_dep_rate"]);
                                sAssetGroup = Convert.ToString(string.IsNullOrEmpty(row1["h_asst_groupid_no"].ToString()) ? "0" : row1["h_asst_groupid_no"]);
                                sPONumber = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_po_number"].ToString()) ? "0" : row1["assetdetails_po_number"]);
                                sFICCRef = Convert.ToString(string.IsNullOrEmpty(row1["h_ficrefnumber"].ToString()) ? "0" : row1["h_ficrefnumber"]);
                                sAsset_GroupId = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_asset_groupid"].ToString()) ? "0" : row1["assetdetails_asset_groupid"]);
                                dTransfer_value = Convert.ToDecimal(string.IsNullOrEmpty(row1["assetdetails_trf_value"].ToString()) ? "0" : row1["assetdetails_trf_value"]);
                            }

                            decimal depamt = Math.Round(GetTotalDep(soacaptilisationda, dtTillDate,
                                            AssetValue, sAssetCode,
                                         cNot5000Case, sBranch1, sBranch2,
                                         dtBranchLaunch, dtLeaseStart, dtLeaseEnd,
                                         sDepType, dDepRate,
                                         sAssetGroup,
                                         sPONumber,
                                         sFICCRef,
                                         sAsset_GroupId,
                                         dTransfer_value,
                                         CanDepreciateFully,
                                         dDepDevRate), 2);
                            decimal rectificationamt = depamt - sumassetdep;





                            //UPDATE THE NEW ASSET ID

                            string[,] codes = new string[,]
	                            { 
                                    {"toadetail_new_assetdetgid", Convert.ToString(newassetgid)},
                                    {"toadetail_update_by",objCmnFunctions.GetLoginUserGid().ToString()},                      
                                    {"toadetail_update_date",objCommonIUD.GetCurrentDate()}, 
	                            };
                            string[,] whreCol = new string[,]
	                            { 
                                    {"toadetail_assetdet_gid", tmm._assetdet_gid.ToString()},
                                    {"toadetail_toahead_gid", _toa_number.ToString()}
	                            };
                            string upd = objCommonIUD.UpdateCommon(codes, whreCol, "fa_trn_ttoadetail");

                            //Location table entry








                            DataTable dtloc = new DataTable();
                            cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@branchgid", locCode1.ToString());
                            cmd.Parameters.AddWithValue("@assetdetid", tmm._new_asset_id.ToString());
                            cmd.Parameters.AddWithValue("@action", "Branchlegacy");
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dtloc);
                            for (int l = 0; l < dtloc.Rows.Count; l++)
                            {
                                branchlegacycode = dtloc.Rows[l]["branch_legacy_code"].ToString();
                                oracleBScode = dtloc.Rows[l]["branch_businesssegement"].ToString();
                            }

                            DataTable dtcc = new DataTable();
                            cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@assetdetid", tmm._new_asset_id.ToString());
                            cmd.Parameters.AddWithValue("@action", "transfercc");
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dtcc);
                            for (int l = 0; l < dtcc.Rows.Count; l++)
                            {
                                oracleCCcode = dtcc.Rows[l]["cc_code"].ToString();
                            }

                            string oraclebscccode = string.Empty;
                            oraclebscccode = oracleCCcode + oracleBScode;
                            string[,] codeloc = new string[,]
                            {
                                {"assetlocation_asset_id",tmm._new_asset_id.ToString()},
                                {"assetlocation_location_code",branchlegacycode.ToString()},
                                {"assetlocation_location_ccfc",oraclebscccode.ToString()},
                                {"assetlocation_location_fc",oracleBScode.ToString()},
                                {"assetlocation_location_cc",oracleCCcode.ToString()},
                                {"assetlocation_ratio","100.00"},
                                {"assetlocation_isremoved","N"},
                                {"assetlocation_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                                {"assetlocation_insert_date",Convert.ToString(convertoDate (DateTime.Today.ToShortDateString()))}
                            };

                            string insertlocationcmn = objCommonIUD.InsertCommon(codeloc, "fa_trn_tassetlocation");






                            // if (rectificationamt >= 0)
                            //{
                            string[,] codesdep = new string[,]
                            {
                                {"depreciation_amount", rectificationamt.ToString()},
                              //  {"depreciation_assetdet_gid", tmm._assetdet_gid.ToString()}, Muthu newassetgid
                                {"depreciation_assetdet_gid", newassetgid.ToString()},
                                {"depreciation_month", Convert.ToString(convertoDate (DateTime.Today.ToShortDateString()))},
                                {"depreciation_asset_groupid", sAsset_GroupId.ToString()},
                                {"depreciation_asset_owner", "F"},
                                {"depreciation_type","T"},
                                //{"depreciation_type","TDEP"},
                                {"depreciation_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                                {"depreciation_insert_date", Convert.ToString(convertoDate (DateTime.Today.ToShortDateString()))}
                            };
                            string insertcmn = objCommonIUD.InsertCommon(codesdep, "fa_trn_tdepreciation");
                            // }
                            //DISABLE THE PREVOIUS ASSET
                            GetConnection();
                            cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "tfr_date";
                            cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = tmm._assetdet_gid.ToString();
                            string trf_dat = Convert.ToString(cmd.ExecuteScalar());


                            cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@fccc", SqlDbType.VarChar).Value = tmm.TModel[i]._loc_from;
                            cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "locid";
                            string locCode2 = string.Empty;
                            da = new SqlDataAdapter(cmd);
                            dt = new DataTable();
                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                                locCode2 = dt.Rows[0]["branch_gid"].ToString();
                            cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = tmm._new_asset_id;
                            cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "assetdetails";
                            _asset_gid_ = Convert.ToString(cmd.ExecuteScalar());
                            string[,] ASScodes1 = new string[,]
	                        {
                                {"assetdetails_tfr_status","Y"},
                                {"assetdetails_active_status","N"},
                                {"assetdetails_tfr_date",convertoDate( trf_dat)},                               
                                {"assetdetails_trf_to",_asset_gid_.ToString()},                                
                                {"assetdetails_trf_reason",tmm.TModel[i]._tfr_reason.ToString()}, 
                                {"assetdetails_tfr_id",_toa_number.ToString()},                       
                                {"assetdetails_reduced_value","0"},                             
                                {"assetdetails_branch_gid",locCode2.ToString()},    
                              //  {"assetdetails_isremoved","Y"} , 
                                {"assetdetails_update_by",objCmnFunctions.GetLoginUserGid().ToString()},                      
                                {"assetdetails_update_date",objCommonIUD.GetCurrentDate()}, 
                                {"assetdetails_takenby", "13"} , 
	                       };
                            string[,] ASSwhrcol = new string[,]
	                        {
                                    {"assetdetails_gid", tmm._assetdet_gid.ToString()} , 
                                    {"assetdetails_isremoved", "N"} ,   
                            };
                            string ASStblname1 = "fa_trn_tassetdetails";
                            string ASSupdcomm = objCommonIUD.UpdateCommon(ASScodes1, ASSwhrcol, ASStblname1);
                            //ENTRY FOR GL UPLOAD
                            cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "UploadDetails";
                            cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = _asset_gid_.ToString();
                            da = new SqlDataAdapter(cmd);
                            string glCode = string.Empty;
                            string resdepglCode = string.Empty;
                            string depglCode = string.Empty;
                            string branch = string.Empty;
                            string cat = string.Empty;
                            string subcat = string.Empty;
                            string asset_id = string.Empty;
                            string BS = string.Empty;
                            string CC = string.Empty;
                            DataTable UPLOADDATATBL = new DataTable();
                            da.Fill(UPLOADDATATBL);
                            if (UPLOADDATATBL.Rows.Count > 0)
                                foreach (DataRow rowdep in UPLOADDATATBL.Rows)
                                {
                                    glCode = UPLOADDATATBL.Rows[0]["asset_glcode"].ToString();
                                    resdepglCode = UPLOADDATATBL.Rows[0]["asset_dep_reservglcode"].ToString();
                                    depglCode = UPLOADDATATBL.Rows[0]["asset_dep_glcode"].ToString();
                                    branch = UPLOADDATATBL.Rows[0]["branch_code"].ToString();
                                    cat = UPLOADDATATBL.Rows[0]["assetcategory_name"].ToString();
                                    if (cat.Length > 16)
                                        cat = cat.Substring(0, 16);
                                    subcat = UPLOADDATATBL.Rows[0]["asset_code"].ToString();
                                    asset_id = UPLOADDATATBL.Rows[0]["assetdetails_assetdet_id"].ToString();
                                }
                            DataTable detaildt3 = new DataTable();
                            detaildt3 = GetDetails(tmm._new_asset_id.ToString());
                            for (int r = 0; r < detaildt3.Rows.Count; r++)
                            {
                                tmm._new_assetdetgid = detaildt3.Rows[r]["assetdetails_gid"].ToString().Trim();
                            }





                            cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "BSCC_upload_details";
                            cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = subcat;
                            cmd.Parameters.Add("@fccc", SqlDbType.VarChar).Value = branch;
                            da = new SqlDataAdapter(cmd);
                            UPLOADDATATBL = new DataTable();
                            da.Fill(UPLOADDATATBL);
                            if (UPLOADDATATBL.Rows.Count > 0)
                                foreach (DataRow rowdep in UPLOADDATATBL.Rows)
                                {
                                    BS = UPLOADDATATBL.Rows[0]["depreciationbscc_bs"].ToString();
                                    CC = UPLOADDATATBL.Rows[0]["depreciationbscc_cc_oracle"].ToString();
                                }



                            //Muthu


                            //ENTRY FOR GL UPLOAD
                            cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "UploadDetails";
                            cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = tmm._assetdet_gid.ToString();
                            da = new SqlDataAdapter(cmd);
                            string oldglCode = string.Empty;
                            string oldresdepglCode = string.Empty;
                            string olddepglCode = string.Empty;
                            string oldbranch = string.Empty;
                            string oldcat = string.Empty;
                            string oldsubcat = string.Empty;
                            string oldasset_id = string.Empty;
                            string oldBS = string.Empty;
                            string oldCC = string.Empty;
                            string oldbranch_gid = string.Empty;
                            DataTable oldUPLOADDATATBL = new DataTable();
                            da.Fill(oldUPLOADDATATBL);
                            if (oldUPLOADDATATBL.Rows.Count > 0)
                                foreach (DataRow rowdep in oldUPLOADDATATBL.Rows)
                                {
                                    oldglCode = oldUPLOADDATATBL.Rows[0]["asset_glcode"].ToString();
                                    oldresdepglCode = oldUPLOADDATATBL.Rows[0]["asset_dep_reservglcode"].ToString();
                                    olddepglCode = oldUPLOADDATATBL.Rows[0]["asset_dep_glcode"].ToString();
                                    oldbranch = oldUPLOADDATATBL.Rows[0]["branch_code"].ToString();
                                    oldbranch_gid = oldUPLOADDATATBL.Rows[0]["branch_gid"].ToString();
                                    oldcat = oldUPLOADDATATBL.Rows[0]["assetcategory_name"].ToString();
                                    if (oldcat.Length > 16)
                                        oldcat = oldcat.Substring(0, 16);
                                    oldsubcat = oldUPLOADDATATBL.Rows[0]["asset_code"].ToString();
                                    oldasset_id = oldUPLOADDATATBL.Rows[0]["assetdetails_assetdet_id"].ToString();
                                }



                            cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "BSCC_upload_details";
                            cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = oldsubcat;
                            cmd.Parameters.Add("@fccc", SqlDbType.VarChar).Value = oldbranch;
                            da = new SqlDataAdapter(cmd);
                            oldUPLOADDATATBL = new DataTable();
                            da.Fill(oldUPLOADDATATBL);
                            if (oldUPLOADDATATBL.Rows.Count > 0)
                                foreach (DataRow rowdep in oldUPLOADDATATBL.Rows)
                                {
                                    oldBS = oldUPLOADDATATBL.Rows[0]["depreciationbscc_bs"].ToString();
                                    oldCC = oldUPLOADDATATBL.Rows[0]["depreciationbscc_cc_oracle"].ToString();
                                }





                            //old id asset gl bscc 

                            DataTable dtoloc = new DataTable();
                            cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@branchgid", oldbranch_gid.ToString());
                            cmd.Parameters.AddWithValue("@assetid", tmm._assetdet_gid.ToString());
                            cmd.Parameters.AddWithValue("@action", "Branchlegacygid");
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dtoloc);
                            for (int l = 0; l < dtoloc.Rows.Count; l++)
                            {
                                oldbranchlegacycode = dtoloc.Rows[l]["branch_legacy_code"].ToString();
                                oldoracleBScode = dtoloc.Rows[l]["branch_businesssegement"].ToString();
                            }



                            DataTable dtocc = new DataTable();
                            cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@assetid", tmm._assetdet_gid.ToString());
                            cmd.Parameters.AddWithValue("@action", "transferccgid");
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dtocc);
                            for (int l = 0; l < dtocc.Rows.Count; l++)
                            {
                                oldoracleCCcode = dtocc.Rows[l]["cc_code"].ToString();
                            }

                            DataTable datamaker_id = objCmnFunctions.GetLoginUserDetails(Convert.ToInt32(_toa_maker));

                            string[,] glCoulmsD = new string[,]
                            {                                
                                {"tran_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_val_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_payment_date",convertoDate (DateTime.Today.ToShortDateString())},
                               // {"tran_desc",tmm._new_asset_id + " || TRANSFER OF ASSET - " + asset_id.ToString() + " under request no " + id },
                               {"tran_desc",tmm.TModel[i]._asset_id.ToString() + " | TRANSFER OF ASSET - " + tmm._new_asset_id.ToString() + " | " + tmm.TModel[i]._inw_no },
                                {"tran_gl_no",glCode},
                                {"tran_amount",AssetValue.ToString()},
                                {"tran_acc_mode","D".ToString()},
                                {"tran_mult","-1"},  
                               // {"tran_fc_code",BS},oracleBScode
                               {"tran_fc_code",oracleBScode},
                                //{"tran_cc_code",CC},    
                                {"tran_cc_code",oracleCCcode},
                                {"tran_ou_code",branch},
                                {"tran_product_code","500".ToString()},
                                {"tran_ref_flag",GetRef("TOA")},
                                {"tran_ref_gid",  tmm._new_assetdetgid .ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                            {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                 {"tran_expense_category","8"},
                                {"tran_primary_branch_code",branch.ToString()},                               
                           //     {"tran_invoice_no",_toa_number.ToString()},                               
                                {"tran_expense_month",convertoDate (DateTime.Today.ToShortDateString())},
                               // {"tran_maker_id",_toa_maker.ToString()},
                              
                               {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                {"tran_checker_id",Getempcode(_toa_maker.ToString())},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                            string insertforglD = objCommonIUD.InsertCommon(glCoulmsD, "fa_trn_ttran");
                            string[,] glCoulmsC = new string[,]
                            {                                
                                {"tran_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_val_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_payment_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_desc",tmm.TModel[i]._asset_id.ToString() + " | TRANSFER OF ASSET - " + tmm._new_asset_id.ToString() + " | " + tmm.TModel[i]._inw_no },
                                {"tran_gl_no",oldglCode},
                                {"tran_amount",AssetValue.ToString()},
                                {"tran_acc_mode","C".ToString()},
                                {"tran_mult","1"},  
                              //  {"tran_fc_code",oldBS},
                                {"tran_fc_code",oracleBScode},
                                //{"tran_cc_code",oldCC},
                                {"tran_cc_code",oracleCCcode},
                                {"tran_product_code","500".ToString()},
                                {"tran_ou_code",oldbranch},
                                {"tran_ref_flag",GetRef("TOA")},
                                {"tran_ref_gid",  tmm._new_assetdetgid.ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                            {"tran_main_cat",cat},
                                {"tran_sub_cat",oldsubcat},
                                 {"tran_expense_category","8"},
                                {"tran_primary_branch_code",oldbranch.ToString()},                               
                            //    {"tran_invoice_no",_toa_number.ToString()},                               
                                {"tran_expense_month",convertoDate (DateTime.Today.ToShortDateString())},
                              //  {"tran_maker_id",_toa_maker.ToString()},
                                 {"tran_checker_id",Getempcode(_toa_maker).ToString()},
                                 {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                  {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                            string insertforglC = objCommonIUD.InsertCommon(glCoulmsC, "fa_trn_ttran");
                            //if (rectificationamt >= 0)
                            //{
                            string[,] glCoulmsDepD = new string[,]
                            {                             
                                {"tran_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_val_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_payment_date",convertoDate (DateTime.Today.ToShortDateString())},
                                // {"tran_gl_no",glCode},Muthu
                                {"tran_gl_no",resdepglCode},
                               {"tran_desc",tmm.TModel[i]._asset_id.ToString() + " | TRANSFER OF ASSET - " + tmm._new_asset_id.ToString() + " | " + tmm.TModel[i]._inw_no },
                                // {"tran_amount",rectificationamt.ToString()},Muthu
                                {"tran_amount",sumassetdep.ToString()},
                                {"tran_acc_mode","C".ToString()},
                                {"tran_mult","-1"},  
                                {"tran_product_code","500".ToString()},
                                {"tran_fc_code",oracleBScode},
                                {"tran_cc_code",oracleCCcode},
                                {"tran_ou_code",branch},
                                {"tran_ref_flag",GetRef("TOA")},
                                {"tran_ref_gid",tmm._assetdet_gid.ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                            {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                   {"tran_expense_category","8"},
                                {"tran_primary_branch_code",branch.ToString()},                                
                                //    {"tran_invoice_no",_toa_number.Trim().ToString()},                                
                                {"tran_expense_month",convertoDate (DateTime.Today.ToShortDateString())},
                                // {"tran_maker_id",objCmnFunctions.GetLoginUserGid().ToString()},
                                {"tran_checker_id",Getempcode(_toa_maker.ToString())},
                                {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                            string insertforglDepD = objCommonIUD.InsertCommon(glCoulmsDepD, "fa_trn_ttran");
                            string[,] glCoulmsDepC = new string[,]
                            {                                
                                {"tran_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_val_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_payment_date",convertoDate (DateTime.Today.ToShortDateString())},
                               {"tran_desc",tmm.TModel[i]._asset_id.ToString() + " | TRANSFER OF ASSET - " + tmm._new_asset_id.ToString() + " | " + tmm.TModel[i]._inw_no },
                                //{"tran_gl_no",glCode},  resdepglCode Muthu
                                {"tran_gl_no",oldresdepglCode},
                                // {"tran_amount",rectificationamt.ToString()}, sumassetdep
                                {"tran_amount",sumassetdep.ToString()},
                                {"tran_acc_mode","D".ToString()},
                                {"tran_mult","1"},  
                                {"tran_fc_code",oracleBScode},
                                {"tran_product_code","500".ToString()},
                                {"tran_cc_code",oracleCCcode},                                
                                {"tran_ou_code",oldbranch},
                                {"tran_ref_flag",GetRef("TOA")},
                                {"tran_ref_gid", tmm._assetdet_gid.ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                            {"tran_main_cat",cat},
                                {"tran_sub_cat",oldsubcat},
                                {"tran_expense_category","8"},
                                {"tran_primary_branch_code",oldbranch.ToString()},
                                //   {"tran_invoice_no",_toa_number.ToString()},                               
                                {"tran_expense_month",convertoDate (DateTime.Today.ToShortDateString())},
                                // {"tran_maker_id",objCmnFunctions.GetLoginUserGid().ToString()},
                                {"tran_checker_id",Getempcode(_toa_maker.ToString())},
                                {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                            string insertforglDepC = objCommonIUD.InsertCommon(glCoulmsDepC, "fa_trn_ttran");
                            //}


                            //tran table insert for transfer

                            if (rectificationamt != 0)
                            {
                                if (rectificationamt < 0)
                                {
                                    rectificationamt = Math.Abs(rectificationamt);


                                    string[,] glCoulmstoaDepC = new string[,]
                                        {                                
                                        {"tran_date",convertoDate (DateTime.Today.ToShortDateString())},
                                        {"tran_val_date",convertoDate (DateTime.Today.ToShortDateString())},
                                        {"tran_payment_date",convertoDate (DateTime.Today.ToShortDateString())},
                                        {"tran_desc",asset_id.ToString() +" | DEP FOR ASSET TRANSFER " },
                                        //{"tran_gl_no",glCode},  resdepglCode Muthu
                                        {"tran_gl_no",depglCode},
                                        // {"tran_amount",rectificationamt.ToString()}, sumassetdep
                                        {"tran_amount",rectificationamt.ToString()},
                                        {"tran_acc_mode","C".ToString()},
                                        {"tran_mult","1"},  
                                        {"tran_fc_code",oldBS},
                                        {"tran_product_code","500".ToString()},
                                        {"tran_cc_code",oldCC},                                
                                        {"tran_ou_code",oldbranch},
                                        {"tran_ref_flag",GetRef("TDEP")},
                                        {"tran_ref_gid", tmm._assetdet_gid.ToString()},
                                        {"tran_upload_gid","0".ToString()},
                                        {"tran_isremoved","N"},
                                    {"tran_main_cat",cat},
                                        {"tran_sub_cat",oldsubcat},
                                        {"tran_expense_category","10"},
                                        {"tran_primary_branch_code",oldbranch.ToString()},
                                        // {"tran_invoice_no",_toa_number.ToString()},                               
                                        {"tran_expense_month",convertoDate (DateTime.Today.ToShortDateString())},
                                        // {"tran_maker_id",objCmnFunctions.GetLoginUserGid().ToString()},
                                        {"tran_checker_id",Getempcode(_toa_maker.ToString())},
                                        {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                        {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                                    string insertfortoaglDepC = objCommonIUD.InsertCommon(glCoulmstoaDepC, "fa_trn_ttran");


                                    string[,] glCoulmstoaDepD = new string[,]
                            {                                
                                    {"tran_date",convertoDate (DateTime.Today.ToShortDateString())},
                                    {"tran_val_date",convertoDate (DateTime.Today.ToShortDateString())},
                                    {"tran_payment_date",convertoDate (DateTime.Today.ToShortDateString())},
                                    {"tran_desc",asset_id.ToString() +" | DEP FOR ASSET TRANSFER " },
                                    //{"tran_gl_no",glCode},  resdepglCode Muthu
                                    {"tran_gl_no",resdepglCode},
                                    // {"tran_amount",rectificationamt.ToString()}, sumassetdep
                                    {"tran_amount",rectificationamt.ToString()},
                                    {"tran_acc_mode","D".ToString()},
                                    {"tran_mult","1"},  
                                    {"tran_fc_code",oldBS},
                                    {"tran_product_code","500".ToString()},
                                    {"tran_cc_code",oldCC},                                
                                    {"tran_ou_code",oldbranch},
                                    {"tran_ref_flag",GetRef("TDEP")},
                                    {"tran_ref_gid", tmm._assetdet_gid.ToString()},
                                    {"tran_upload_gid","0".ToString()},
                                    {"tran_isremoved","N"},
                                {"tran_main_cat",cat},
                                    {"tran_sub_cat",oldsubcat},
                                     {"tran_expense_category","10"},
                                    {"tran_primary_branch_code",oldbranch.ToString()},
                                    // {"tran_invoice_no",_toa_number.ToString()},                               
                                    {"tran_expense_month",convertoDate (DateTime.Today.ToShortDateString())},
                                    // {"tran_maker_id",objCmnFunctions.GetLoginUserGid().ToString()},
                                    {"tran_checker_id",Getempcode(_toa_maker.ToString())},
                                    {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                    {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                                    string insertfortoaglDepD = objCommonIUD.InsertCommon(glCoulmstoaDepD, "fa_trn_ttran");
                                }

                                else
                                {
                                    string[,] glCoulmstoaDepC = new string[,]
                            {                                
                                {"tran_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_val_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_payment_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_desc",asset_id.ToString() +" | DEP FOR ASSET TRANSFER "},
                                //{"tran_gl_no",glCode},  resdepglCode Muthu
                                {"tran_gl_no",resdepglCode},
                               // {"tran_amount",rectificationamt.ToString()}, sumassetdep
                               {"tran_amount",rectificationamt.ToString()},
                                {"tran_acc_mode","C".ToString()},
                                {"tran_mult","1"},  
                                {"tran_fc_code",oldBS},
                                {"tran_product_code","500".ToString()},
                                {"tran_cc_code",oldCC},                                
                                {"tran_ou_code",oldbranch},
                                {"tran_ref_flag",GetRef("TDEP")},
                                {"tran_ref_gid", tmm._assetdet_gid.ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                            {"tran_main_cat",cat},
                                {"tran_sub_cat",oldsubcat},
                                 {"tran_expense_category","10"},
                                {"tran_primary_branch_code",oldbranch.ToString()},
                            //    {"tran_invoice_no",_toa_number.ToString()},                               
                                {"tran_expense_month",convertoDate (DateTime.Today.ToShortDateString())},
                               // {"tran_maker_id",objCmnFunctions.GetLoginUserGid().ToString()},
                                 {"tran_checker_id",Getempcode(_toa_maker.ToString())},
                                 {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                  {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                                    string insertfortoaglDepC = objCommonIUD.InsertCommon(glCoulmstoaDepC, "fa_trn_ttran");


                                    string[,] glCoulmstoaDepD = new string[,]
                            {                                
                                {"tran_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_val_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_payment_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_desc",asset_id.ToString() +" | DEP FOR ASSET TRANSFER " },
                                //{"tran_gl_no",glCode},  resdepglCode Muthu
                                {"tran_gl_no",depglCode},
                               // {"tran_amount",rectificationamt.ToString()}, sumassetdep
                               {"tran_amount",rectificationamt.ToString()},
                                {"tran_acc_mode","D".ToString()},
                                {"tran_mult","1"},  
                                {"tran_fc_code",oldBS},
                                {"tran_product_code","500".ToString()},
                                {"tran_cc_code",oldCC},                                
                                {"tran_ou_code",oldbranch},
                                {"tran_ref_flag",GetRef("TDEP")},
                                {"tran_ref_gid", tmm._assetdet_gid.ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                            {"tran_main_cat",cat},
                                {"tran_sub_cat",oldsubcat},
                                 {"tran_expense_category","10"},
                                {"tran_primary_branch_code",oldbranch.ToString()},
                               // {"tran_invoice_no",_toa_number.ToString()},                               
                                {"tran_expense_month",convertoDate (DateTime.Today.ToShortDateString())},
                               // {"tran_maker_id",objCmnFunctions.GetLoginUserGid().ToString()},
                                 {"tran_checker_id",Getempcode(_toa_maker.ToString())},
                                 {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                  {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                                    string insertfortoaglDepD = objCommonIUD.InsertCommon(glCoulmstoaDepD, "fa_trn_ttran");
                                }

                            }


                        }
                    }


                    string[,] colm = new string[,]
                    {
                             {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                             {"queue_action_status","1"},
                             {"queue_action_date",objCommonIUD.GetCurrentDate()}   ,
                             {"queue_isremoved","Y"},
                             {"queue_action_remark",remarks},
                                              
                    };
                    string[,] whrecol = new string[,]
                    {
                              {"queue_ref_gid", _toa_number.Trim()} ,    
                              {"queue_ref_flag", refflag.Trim()}                                           
                    };
                    string updcmd = objCommonIUD.UpdateCommon(colm, whrecol, "iem_trn_tqueue");
                }

                DataTable dt1 = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = status.Trim();
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "status";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt1);
                foreach (DataRow row in dt1.Rows)
                {
                    status = row["status_flag"].ToString();
                }
                TransferMakerModel obj = new TransferMakerModel();
                string[,] codes1 = new string[,]
                    {
                             {"toaheader_tfr_status",status},
                             {"toaheader_checker_id",objCmnFunctions.GetLoginUserGid().ToString()},
                             {"toaheader_checker_date",objCommonIUD.GetCurrentDate()}  ,
                             {"toaheader_update_by",objCmnFunctions.GetLoginUserGid().ToString()},
                             {"toaheader_update_date",objCommonIUD.GetCurrentDate()} ,
                             {"toaheader_remarks",remarks.ToString()} ,
                             {"toaheader_process_date",objCommonIUD.GetCurrentDate()}
                    };
                string[,] whrcol = new string[,]
                    {
                              {"toaheader_toa_number", id.Trim()}                                                                              
                    };
                string updcomm = objCommonIUD.UpdateCommon(codes1, whrcol, "fa_trn_ttoaheader");


                return status;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return string.Empty;
            }
            finally
            {
                con.Close();
            }
        }*/
        #endregion




        public List<WriteOffModel> GetWoaDetail(int id)
        {
            List<WriteOffModel> wmm = new List<WriteOffModel>();
            try
            {

                WriteOffModel obj = new WriteOffModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_woaMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "SelectSum";


                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new WriteOffModel();
                    DataTable dt1 = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = row["woaheader_woa_status"].ToString();
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "statusname";
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt1);
                    foreach (DataRow row1 in dt1.Rows)
                        obj._woa_status = row1["status_name"].ToString();
                    obj._gid = Convert.ToInt32(row["woaheader_gid"].ToString());
                    obj._woa_number = row["woaheader_woa_number"].ToString();
                    obj._woa_date = Convert.ToString(row["woaheader_trans_date"].ToString());
                    obj._no_records = Convert.ToInt64(row["woaheader_nof_records"].ToString());
                    obj._upld_fname = row["woaheader_upld_fname"].ToString();
                    wmm.Add(obj);
                }
                return wmm;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return wmm;
            }
            finally
            {
                con.Close();
            }
        }

        public List<WriteOffModel> GetWoaDetailDraft(int id)
        {
            List<WriteOffModel> wmm = new List<WriteOffModel>();
            try
            {

                WriteOffModel obj = new WriteOffModel();
                GetConnection();
                DataTable dt = new DataTable();
                //cmd = new SqlCommand("pr_ifams_trn_woaMaker", con);
                cmd = new SqlCommand("pr_ifams_get_woaMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "SelectSumDRAFT";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new WriteOffModel();
                    obj._woa_status = "DRAFT";
                    obj._gid = Convert.ToInt32(row["woaheader_gid"].ToString());
                    obj._woa_number = row["woaheader_woa_number"].ToString();
                    obj._woa_date = Convert.ToString(row["woaheader_trans_date"].ToString());
                    obj._no_records = Convert.ToInt64(row["woaheader_nof_records"].ToString());
                    obj._upld_fname = row["woaheader_upld_fname"].ToString();
                    wmm.Add(obj);
                }
                return wmm;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return wmm;
            }
            finally
            {
                con.Close();
            }
        }


        //Reject Dashboard Click


        public List<WriteOffModel> GetWoaDetailReject(int id)
        {
            List<WriteOffModel> wmm = new List<WriteOffModel>();
            try
            {

                WriteOffModel obj = new WriteOffModel();
                GetConnection();
                DataTable dt = new DataTable();
                //cmd = new SqlCommand("pr_ifams_trn_woaMaker", con);
                cmd = new SqlCommand("pr_ifams_get_woaMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "SELECTREJECTED";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new WriteOffModel();
                    obj._woa_status = "REJECTED";
                    obj._gid = Convert.ToInt32(row["woaheader_gid"].ToString());
                    obj._woa_number = row["woaheader_woa_number"].ToString();
                    obj._woa_date = Convert.ToString(row["woaheader_trans_date"].ToString());
                    obj._no_records = Convert.ToInt64(row["woaheader_nof_records"].ToString());
                    obj._upld_fname = row["woaheader_upld_fname"].ToString();
                    wmm.Add(obj);
                }
                return wmm;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return wmm;
            }
            finally
            {
                con.Close();
            }
        }





        //public List<TransferMakerModel> PopulateAuditTrail(TransferMakerModel pat)
        //{
        //    List<TransferMakerModel> objAuditTrail = new List<TransferMakerModel>();
        //    try
        //    {
        //        TransferMakerModel objModel;
        //        GetConnection();
        //        DataTable dt = new DataTable();
        //        string fstEmp = string.Empty;
        //        string status = string.Empty;
        //        cmd = new SqlCommand("pr_ifams_trn_audittaril", con);
        //        cmd.Parameters.AddWithValue("@gid", pat.gid);
        //        cmd.Parameters.AddWithValue("@refflag", "TOA");
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        if (dt.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                string pergid = Convert.ToString(dt.Rows[i]["queue_prev_gid"].ToString());
        //                if (pergid == "0")
        //                {
        //                    fstEmp = Convert.ToString(dt.Rows[i]["queue_from"].ToString());
        //                    string empcodnamer = Getempname(fstEmp);
        //                    string[] datar;
        //                    datar = empcodnamer.Split(',');
        //                    objModel = new TransferMakerModel();
        //                    objModel.employee_code = datar[0].ToString() + " - " + datar[1].ToString();
        //                    objModel.action_status = "Submitted";
        //                    objModel.action_date = Convert.ToString(dt.Rows[i]["queue_date"].ToString());
        //                    objModel.queue_to_type = "TOA Maker";
        //                    objModel.action_remarks = string.Empty;
        //                    objAuditTrail.Add(objModel);
        //                    string actions = Convert.ToString(dt.Rows[i]["queue_action_by"].ToString());
        //                    if (actions == string.Empty)
        //                    {
        //                        string queue_type = Convert.ToString(dt.Rows[i]["queue_to_type"].ToString());
        //                        string queue_to = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
        //                        string actionstt = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());

        //                        if (actionstt != string.Empty)
        //                        {
        //                            objModel = new TransferMakerModel();
        //                            objModel.employee_code = string.Empty;
        //                            objModel.action_date = Convert.ToString(dt.Rows[i]["queue_action_date"].ToString());
        //                            status = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                            objModel.action_status = GetQueueStatusHistry(status);
        //                            objModel.queue_to_type = GetQueueRole(queue_type, queue_to);
        //                            objModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());
        //                            objAuditTrail.Add(objModel);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        string empid = string.Empty;
        //                        string queue_type = Convert.ToString(dt.Rows[i]["queue_to_type"].ToString());
        //                        string queue_to = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
        //                        string actionstt = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                        if (actionstt != string.Empty)
        //                        {
        //                            objModel = new TransferMakerModel();
        //                            //if (queue_type == "I" || queue_type == "E")
        //                            //{
        //                            //    empid = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
        //                            //}
        //                            // else 
        //                            if (queue_type == "G" || queue_type == "R" || queue_type == "L" || queue_type == "D")
        //                            {
        //                                empid = Convert.ToString(dt.Rows[i]["queue_action_by"].ToString());
        //                            }
        //                            if (empid != string.Empty)
        //                            {
        //                                string empcodname = Getempname(empid);
        //                                string[] data;
        //                                data = empcodname.Split(',');
        //                                objModel.employee_code = data[0].ToString() + " - " + data[1].ToString();
        //                            }
        //                            objModel.action_date = Convert.ToString(dt.Rows[i]["queue_action_date"].ToString());
        //                            status = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                            objModel.action_status = GetQueueStatusHistry(status);
        //                            objModel.queue_to_type = GetQueueRole(queue_type, queue_to);
        //                            objModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());
        //                            objAuditTrail.Add(objModel);
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return objAuditTrail;
        //}


        //Audit Trail For TOA
        public List<TransferMakerModel> PopulateAuditTrail(TransferMakerModel pat)
        {
            List<TransferMakerModel> objDashBoard = new List<TransferMakerModel>();
            try
            {
                TransferMakerModel objModel;
                int liRaiserGid = 0;
                int liCnt = 0;
                GetConnection();
                DataTable dt = new DataTable();
                DataTable dtr = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_audittaril", con);
                cmd.Parameters.AddWithValue("@gid", pat.gid);
                cmd.Parameters.AddWithValue("@refflag", "TOA");
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        string pergid = Convert.ToString(dt.Rows[i]["queue_prev_gid"].ToString());
                        if (pergid == "0")
                        {
                            objModel = new TransferMakerModel();
                            objModel.queue_gid = Convert.ToInt32(dt.Rows[i]["queue_gid"].ToString());
                            objModel.employee_code = Convert.ToString(dt.Rows[i]["queue_from_name"].ToString());
                            objModel.action_date = Convert.ToString(dt.Rows[i]["queue_date"].ToString());
                            liRaiserGid = Convert.ToInt32(dt.Rows[i]["queue_from"].ToString());
                            objModel.queue_to_type = "Raiser";
                            if (liCnt == 0)
                            {
                                objModel.action_status = "Submitted";
                            }
                            else
                            {
                                objModel.action_status = "ReSubmitted";
                            }

                            objDashBoard.Add(objModel);
                            liCnt = liCnt + 1;
                        }

                        string actions = Convert.ToString(dt.Rows[i]["queue_action_for_name"].ToString());
                        string empid = Convert.ToString(dt.Rows[i]["queue_action_by"].ToString());
                        string queue_type = Convert.ToString(dt.Rows[i]["queue_to_type"].ToString());
                        string queue_to = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
                        string delgation_gid = Convert.ToString(dt.Rows[i]["queue_delegation_gid"].ToString());

                        objModel = new TransferMakerModel();
                        objModel.queue_gid = Convert.ToInt32(dt.Rows[i]["queue_gid"].ToString());
                        //objModel.action_status = Convert.ToString(dt.Rows[i]["queue_action_status_name"].ToString());
                        //queue_action_status
                        objModel.action_status = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
                        objModel.queue_to_type = Convert.ToString(dt.Rows[i]["queue_to_name"].ToString());

                        string[] data, data1;
                        string employeedetails, delegatedetails;
                        employeedetails = Getempname(empid);
                        data = employeedetails.Split(',');

                        if (dt.Rows[i]["Additional_flag"].ToString() == "N"
                            && queue_type != "G"
                            && queue_type != "I"
                            && queue_type != "E"
                            && objModel.action_status != "Approved"
                            && objModel.action_status != "Rejected")
                        {
                            string empgid = Convert.ToString(GetEmployeeHierarchy(liRaiserGid, Convert.ToString(dt.Rows[i]["queue_to_type_name"].ToString()), Convert.ToInt32(dt.Rows[i]["queue_to"].ToString())));
                            string EmployeeDetails = GetEmployeeName(Convert.ToInt32(empgid));
                            objModel.employee_code = EmployeeDetails;
                        }
                        else
                        {
                            if (queue_type == "I" || queue_type == "E")
                            {
                                if (empid == delgation_gid || delgation_gid == "0")
                                {
                                    objModel.employee_code = GetEmployeeName(Convert.ToInt32(dt.Rows[i]["queue_to"].ToString()));
                                }
                                else
                                {
                                    delegatedetails = Getempname(delgation_gid);
                                    data1 = delegatedetails.Split(',');
                                    objModel.employee_code = data1[0].ToString() + "-" + data1[1].ToString() + " instead of " + data[0].ToString() + "-" + data[1].ToString();
                                }
                            }
                            else
                            {
                                if (empid == delgation_gid || delgation_gid == "0")
                                {
                                    objModel.employee_code = Convert.ToString(dt.Rows[i]["action_by_name"].ToString());
                                }
                                else
                                {
                                    delegatedetails = Getempname(delgation_gid);
                                    data1 = delegatedetails.Split(',');
                                    objModel.employee_code = data1[0].ToString() + "-" + data1[1].ToString() + " instead of " + data[0].ToString() + "-" + data[1].ToString();
                                }
                            }
                        }
                        if ((i + 1) == dt.Rows.Count)
                        {
                            int refflagnew = Convert.ToInt32(dt.Rows[i]["queue_ref_flag"].ToString());
                            string isfinalapprover = CheckFinalApproverTag(pat.gid, refflagnew, objModel.queue_gid);
                            if (isfinalapprover == "Y")
                                objModel.action_status = "Final Approver";
                        }
                        objModel.action_date = Convert.ToString(dt.Rows[i]["queue_action_date"].ToString());
                        objModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());

                        objDashBoard.Add(objModel);
                    }
                }
                return objDashBoard;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objDashBoard;
            }
            finally
            {
                con.Close();
            }
        }

        //FOR TOA
        public string GetEmployeeHierarchy(int raisergid = 0, string titlename = null, int titlerefgid = 0)
        {
            try
            {
                string lsResult = string.Empty;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_chk_employee_hierarchy", con);
                cmd.Parameters.AddWithValue("@employee_gid", raisergid);
                cmd.Parameters.AddWithValue("@title_name", titlename);
                cmd.Parameters.AddWithValue("@title_value_gid", titlerefgid);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@title_employee_gid", SqlDbType.Int, 5);
                cmd.Parameters["@title_employee_gid"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                lsResult = Convert.ToString(cmd.Parameters["@title_employee_gid"].Value.ToString());

                return lsResult;
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


        // For TOA
        public string GetEmployeeName(int empgid = 0)
        {
            string EmpName = "";
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_iem_LoginDetails", con);
                cmd.Parameters.AddWithValue("@empgid", empgid);
                cmd.Parameters.AddWithValue("@action", "employeenameforaudittrial");
                cmd.CommandType = CommandType.StoredProcedure;
                EmpName = Convert.ToString(cmd.ExecuteScalar());
                return EmpName;
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

        //For TOA
        public string CheckFinalApproverTag(int ref_gid = 0, int ref_flag = 0, int queue_gid = 0)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@wogid", SqlDbType.Int).Value = ref_gid;
                cmd.Parameters.Add("@wodetailgid", SqlDbType.Int).Value = ref_flag;
                cmd.Parameters.Add("@cbfdetailsgid", SqlDbType.Int).Value = queue_gid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "checkfinalapprovaltag";
                string result = Convert.ToString(cmd.ExecuteScalar());
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "N";
            }
            finally
            {
                con.Close();
            }
        }


        //public List<WriteOffModel> PopulateAuditTrail(WriteOffModel pat)
        //{
        //    List<WriteOffModel> objAuditTrail = new List<WriteOffModel>();
        //    try
        //    {
        //        WriteOffModel objModel;
        //        GetConnection();
        //        DataTable dt = new DataTable();
        //        string fstEmp = string.Empty;
        //        string status = string.Empty;
        //        cmd = new SqlCommand("pr_ifams_trn_audittaril", con);
        //        cmd.Parameters.AddWithValue("@gid", pat.gid);
        //        cmd.Parameters.AddWithValue("@refflag", "WOA");
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        if (dt.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                string pergid = Convert.ToString(dt.Rows[i]["queue_prev_gid"].ToString());
        //                if (pergid == "0")
        //                {
        //                    fstEmp = Convert.ToString(dt.Rows[i]["queue_from"].ToString());
        //                    string empcodnamer = Getempname(fstEmp);
        //                    string[] datar;
        //                    datar = empcodnamer.Split(',');
        //                    objModel = new WriteOffModel();
        //                    objModel.employee_code = datar[0].ToString() + " - " + datar[1].ToString();
        //                    objModel.action_status = "Submitted";
        //                    objModel.action_date = Convert.ToString(dt.Rows[i]["queue_date"].ToString());
        //                    objModel.queue_to_type = "WOA Maker";
        //                    objModel.action_remarks = string.Empty;
        //                    objAuditTrail.Add(objModel);
        //                    string actions = Convert.ToString(dt.Rows[i]["queue_action_by"].ToString());
        //                    if (actions == string.Empty)
        //                    {
        //                        string queue_type = Convert.ToString(dt.Rows[i]["queue_to_type"].ToString());
        //                        string queue_to = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
        //                        string actionstt = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                        if (actionstt != string.Empty)
        //                        {
        //                            objModel = new WriteOffModel();
        //                            objModel.employee_code = string.Empty;
        //                            objModel.action_date = Convert.ToString(dt.Rows[i]["queue_action_date"].ToString());
        //                            status = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                            objModel.action_status = GetQueueStatusHistry(status);
        //                            objModel.queue_to_type = GetQueueRole(queue_type, queue_to);
        //                            objModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());
        //                            objAuditTrail.Add(objModel);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        string empid = string.Empty;
        //                        string queue_type = Convert.ToString(dt.Rows[i]["queue_to_type"].ToString());
        //                        string queue_to = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
        //                        string actionstt = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                        if (actionstt != string.Empty)
        //                        {
        //                            objModel = new WriteOffModel();
        //                            if (queue_type == "G" || queue_type == "R" || queue_type == "L" || queue_type == "D")
        //                            {
        //                                empid = Convert.ToString(dt.Rows[i]["queue_action_by"].ToString());
        //                            }
        //                            if (empid != string.Empty)
        //                            {
        //                                string empcodname = Getempname(empid);
        //                                string[] data;
        //                                data = empcodname.Split(',');
        //                                objModel.employee_code = data[0].ToString() + " - " + data[1].ToString();
        //                            }
        //                            objModel.action_date = Convert.ToString(dt.Rows[i]["queue_action_date"].ToString());
        //                            status = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                            objModel.action_status = GetQueueStatusHistry(status);
        //                            objModel.queue_to_type = GetQueueRole(queue_type, queue_to);
        //                            objModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());
        //                            objAuditTrail.Add(objModel);
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return objAuditTrail;
        //}




        //Audit Trail For WOA

        public List<WriteOffModel> PopulateAuditTrail(WriteOffModel pat)
        {
            List<WriteOffModel> objDashBoard = new List<WriteOffModel>();
            try
            {
                WriteOffModel objModel;
                int liRaiserGid = 0;
                int liCnt = 0;
                GetConnection();
                DataTable dt = new DataTable();
                DataTable dtr = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_audittaril", con);
                cmd.Parameters.AddWithValue("@gid", pat.gid);
                cmd.Parameters.AddWithValue("@refflag", "WOA");
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        string pergid = Convert.ToString(dt.Rows[i]["queue_prev_gid"].ToString());
                        if (pergid == "0")
                        {
                            objModel = new WriteOffModel();
                            objModel.queue_gid = Convert.ToInt32(dt.Rows[i]["queue_gid"].ToString());
                            objModel.employee_code = Convert.ToString(dt.Rows[i]["queue_from_name"].ToString());
                            objModel.action_date = Convert.ToString(dt.Rows[i]["queue_date"].ToString());
                            liRaiserGid = Convert.ToInt32(dt.Rows[i]["queue_from"].ToString());
                            objModel.queue_to_type = "Raiser";
                            if (liCnt == 0)
                            {
                                objModel.action_status = "Submitted";
                            }
                            else
                            {
                                objModel.action_status = "ReSubmitted";
                            }

                            objDashBoard.Add(objModel);
                            liCnt = liCnt + 1;
                        }

                        string actions = Convert.ToString(dt.Rows[i]["queue_action_for_name"].ToString());
                        string empid = Convert.ToString(dt.Rows[i]["queue_action_by"].ToString());
                        string queue_type = Convert.ToString(dt.Rows[i]["queue_to_type"].ToString());
                        string queue_to = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
                        string delgation_gid = Convert.ToString(dt.Rows[i]["queue_delegation_gid"].ToString());

                        objModel = new WriteOffModel();
                        objModel.queue_gid = Convert.ToInt32(dt.Rows[i]["queue_gid"].ToString());
                        //objModel.action_status = Convert.ToString(dt.Rows[i]["queue_action_status_name"].ToString());
                        //queue_action_status
                        objModel.action_status = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
                        objModel.queue_to_type = Convert.ToString(dt.Rows[i]["queue_to_name"].ToString());

                        string[] data, data1;
                        string employeedetails, delegatedetails;
                        employeedetails = Getempname(empid);
                        data = employeedetails.Split(',');

                        if (dt.Rows[i]["Additional_flag"].ToString() == "N"
                            && queue_type != "G"
                            && queue_type != "I"
                            && queue_type != "E"
                            && objModel.action_status != "Approved"
                            && objModel.action_status != "Rejected")
                        {
                            string empgid = Convert.ToString(GetEmployeeHierarchy(liRaiserGid, Convert.ToString(dt.Rows[i]["queue_to_type_name"].ToString()), Convert.ToInt32(dt.Rows[i]["queue_to"].ToString())));
                            string EmployeeDetails = GetEmployeeName(Convert.ToInt32(empgid));
                            objModel.employee_code = EmployeeDetails;
                        }
                        else
                        {
                            if (queue_type == "I" || queue_type == "E")
                            {
                                if (empid == delgation_gid || delgation_gid == "0")
                                {
                                    objModel.employee_code = GetEmployeeName(Convert.ToInt32(dt.Rows[i]["queue_to"].ToString()));
                                }
                                else
                                {
                                    delegatedetails = Getempname(delgation_gid);
                                    data1 = delegatedetails.Split(',');
                                    objModel.employee_code = data1[0].ToString() + "-" + data1[1].ToString() + " instead of " + data[0].ToString() + "-" + data[1].ToString();
                                }
                            }
                            else
                            {
                                if (empid == delgation_gid || delgation_gid == "0")
                                {
                                    objModel.employee_code = Convert.ToString(dt.Rows[i]["action_by_name"].ToString());
                                }
                                else
                                {
                                    delegatedetails = Getempname(delgation_gid);
                                    data1 = delegatedetails.Split(',');
                                    objModel.employee_code = data1[0].ToString() + "-" + data1[1].ToString() + " instead of " + data[0].ToString() + "-" + data[1].ToString();
                                }
                            }
                        }
                        if ((i + 1) == dt.Rows.Count)
                        {
                            int refflagnew = Convert.ToInt32(dt.Rows[i]["queue_ref_flag"].ToString());
                            string isfinalapprover = CheckFinalApproverTag(pat.gid, refflagnew, objModel.queue_gid);
                            if (isfinalapprover == "Y")
                                objModel.action_status = "Final Approver";
                        }
                        objModel.action_date = Convert.ToString(dt.Rows[i]["queue_action_date"].ToString());
                        objModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());

                        objDashBoard.Add(objModel);
                    }
                }
                return objDashBoard;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objDashBoard;
            }
            finally
            {
                con.Close();
            }
        }



        public string Getempname(string empgid)
        {
            string status = string.Empty;
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_audittaril", con);
                cmd.Parameters.AddWithValue("@gid", empgid);
                cmd.Parameters.AddWithValue("@refflag", "E");
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    string raisername = Convert.ToString(dt.Rows[0]["employee_code"].ToString());
                    string raisercode = Convert.ToString(dt.Rows[0]["employee_name"].ToString());
                    status = raisername + "," + raisercode;
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
            return status;
        }

        public string GetQueueStatusHistry(string Queue)
        {
            string Status = string.Empty;
            try
            {
                if (Queue.Contains("1"))
                {
                    Status = "Approved";
                }
                if (Queue.Contains("2"))
                {
                    Status = "Rejected";
                }
                if (Queue.Contains("0"))
                {
                    Status = "Pending";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Status;
        }

        public string GetQueueRole(string type, string gid)
        {
            string queuetype = string.Empty;
            try
            {
                if (type.Contains("G"))
                {
                    GetConnection();
                    DataTable dt = new DataTable();
                    cmd = new SqlCommand("pr_fb_trn_audittaril", con);
                    cmd.Parameters.AddWithValue("@gid", gid);
                    cmd.Parameters.AddWithValue("@refflag", "G");
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        queuetype = Convert.ToString(dt.Rows[0]["rolegroup_name"].ToString());
                    }
                }
                //if (type.Contains("R"))
                //{
                //    GetConnection();
                //    DataTable dt = new DataTable();
                //    cmd = new SqlCommand("pr_fb_trn_audittaril", con);
                //    cmd.Parameters.AddWithValue("@gid", gid);
                //    cmd.Parameters.AddWithValue("@refflag", "R");
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    da = new SqlDataAdapter(cmd);
                //    da.Fill(dt);
                //    if (dt.Rows.Count > 0)
                //    {
                //        queuetype = Convert.ToString(dt.Rows[0]["role_name"].ToString());
                //    }
                //}
                //if (type.Contains("L"))
                //{
                //    GetConnection();
                //    DataTable dt = new DataTable();
                //    cmd = new SqlCommand("pr_fb_trn_audittaril", con);
                //    cmd.Parameters.AddWithValue("@gid", gid);
                //    cmd.Parameters.AddWithValue("@refflag", "L");
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    da = new SqlDataAdapter(cmd);
                //    da.Fill(dt);
                //    if (dt.Rows.Count > 0)
                //    {
                //        queuetype = Convert.ToString(dt.Rows[0]["grade_name"].ToString());
                //    }
                //}
                //if (type.Contains("D"))
                //{
                //    GetConnection();
                //    DataTable dt = new DataTable();
                //    cmd = new SqlCommand("pr_fb_trn_audittaril", con);
                //    cmd.Parameters.AddWithValue("@gid", gid);
                //    cmd.Parameters.AddWithValue("@refflag", "D");
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    da = new SqlDataAdapter(cmd);
                //    da.Fill(dt);
                //    if (dt.Rows.Count > 0)
                //    {
                //        queuetype = Convert.ToString(dt.Rows[0]["designation_name"].ToString());
                //    }
                //}
                //if (type.Contains("I"))
                //{
                //    GetConnection();
                //    DataTable dt = new DataTable();
                //    cmd = new SqlCommand("pr_fb_trn_audittaril", con);
                //    cmd.Parameters.AddWithValue("@gid", gid);
                //    cmd.Parameters.AddWithValue("@refflag", "I");
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    da = new SqlDataAdapter(cmd);
                //    da.Fill(dt);
                //    if (dt.Rows.Count > 0)
                //    {
                //        queuetype = Convert.ToString(dt.Rows[0]["designation_name"].ToString());
                //    }
                //}               
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return queuetype;
        }

        public List<TransferMakerModel> verifylogeddetails(string headergid)
        {
            List<TransferMakerModel> objl = new List<TransferMakerModel>();
            TransferMakerModel obji;
            DataTable dtGetDeetails = new DataTable();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_verifylogedstatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@empgid", Convert.ToString(objCmnFunctions.GetLoginUserGid()));
                cmd.Parameters.AddWithValue("@process", "toa");
                cmd.Parameters.AddWithValue("@headergid", headergid);
                da = new SqlDataAdapter(cmd);
                da.Fill(dtGetDeetails);
                if (dtGetDeetails.Rows.Count > 0)
                {
                    for (int i = 0; i < dtGetDeetails.Rows.Count; i++)
                    {
                        obji = new TransferMakerModel();
                        obji.loged_empgid = Convert.ToInt32(dtGetDeetails.Rows[i]["empgid"].ToString());
                        obji.loged_empcode = dtGetDeetails.Rows[i]["employeecode"].ToString();
                        obji.loged_empname = dtGetDeetails.Rows[i]["employeename"].ToString();
                        obji.loged_date = dtGetDeetails.Rows[i]["logeddate"].ToString();
                        objl.Add(obji);
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return objl;
        }

        public List<WriteOffModel> verifyWoalogeddetails(string headergid)
        {
            List<WriteOffModel> objl = new List<WriteOffModel>();
            WriteOffModel obji;
            DataTable dtGetDeetails = new DataTable();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_verifylogedstatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@empgid", Convert.ToString(objCmnFunctions.GetLoginUserGid()));
                cmd.Parameters.AddWithValue("@process", "woa");
                cmd.Parameters.AddWithValue("@headergid", headergid);
                da = new SqlDataAdapter(cmd);
                da.Fill(dtGetDeetails);
                if (dtGetDeetails.Rows.Count > 0)
                {
                    for (int i = 0; i < dtGetDeetails.Rows.Count; i++)
                    {
                        obji = new WriteOffModel();
                        obji.loged_empgid = Convert.ToInt32(dtGetDeetails.Rows[i]["empgid"].ToString());
                        obji.loged_empcode = dtGetDeetails.Rows[i]["employeecode"].ToString();
                        obji.loged_empname = dtGetDeetails.Rows[i]["employeename"].ToString();
                        obji.loged_date = dtGetDeetails.Rows[i]["logeddate"].ToString();
                        objl.Add(obji);
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return objl;
        }

        public string Updatelogedstatus(string headergid)
        {
            string insertcommand = string.Empty;
            try
            {
                string[,] codes = new string[,]
                   {
                     {"toaheader_loged_status","Y" },
                     {"toaheader_loged_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"toaheader_loged_date",convertoDate (DateTime.Today.ToShortDateString()) }
                   };
                string[,] whrcol = new string[,]
	                 {
                          {"toaheader_gid",  headergid},
                          {"toaheader_isremoved", "N"}
                     };

                insertcommand = objCommonIUD.UpdateCommon(codes, whrcol, "fa_trn_ttoaheader");
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return insertcommand;

        }

        public string changlogedstatus(string toaheaderid)
        {
            string changelog = string.Empty;
            try
            {
                string[,] codes = new string[,]
                   {
                     {"toaheader_loged_status","N" },
                     {"toaheader_loged_by",string.Empty },
                     {"toaheader_loged_date",convertoDate (DateTime.Today.ToShortDateString()) }
                   };
                string[,] whrcol = new string[,]
	                 {
                          {"toaheader_gid",  toaheaderid},
                          {"toaheader_isremoved", "N"}
                     };

                changelog = objCommonIUD.UpdateCommon(codes, whrcol, "fa_trn_ttoaheader");
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return changelog;
        }

        public string convertoDate(string date1)
        {
            try
            {
                string date2 = string.IsNullOrEmpty(date1) ? "01-01-1900" : date1 == "0" ? "01-01-1900" : date1 == "undefined" ? "01-01-1900" : date1;
                string convdate = string.Empty;
                DateTime convdate2 = Convert.ToDateTime(date2);
                string format = "dd/MMM/yyyy";
                convdate = convdate2.ToString(format);
                return convdate;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return string.Empty;
            }
        }

        public string InsertWriteOff(DataTable assetdata, string filename)
        {
            try
            {
                GetConnection();
                List<WriteOffModel> wmList = new List<WriteOffModel>();
                WriteOffModel wm;
                string status = string.Empty;
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = "DRAFT";
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "status";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    status = row["status_flag"].ToString();
                }
                string woanumber = objCmnFunctions.GetSequenceNoFam("WOA");
                for (int i = 0; i == 0; i++)
                {
                    wm = new WriteOffModel();
                    wm._upld_fname = filename.ToString();
                    wm._woa_number = woanumber;
                    string[,] codes = new string[,]
	                { 
                             {"woaheader_woa_number",wm._woa_number.ToString()},
                             {"woaheader_maker_id",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                             {"woaheader_maker_date",Convert.ToString(objCommonIUD.GetCurrentDate())}, 
                             {"woaheader_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                             {"woaheader_insert_date",Convert.ToString(objCommonIUD.GetCurrentDate())}, 
                             {"woaheader_upld_date",Convert.ToString(objCommonIUD.GetCurrentDate())},
                             {"woaheader_trans_date",Convert.ToString(objCommonIUD.GetCurrentDate())},                            
                             {"woaheader_upld_fname",wm._upld_fname.ToString()},
                             {"woaheader_isremoved","N"},
                             {"woaheader_nof_records",Convert.ToString(assetdata.Rows.Count)},
                             {"woaheader_woa_status",status.ToString()},
                             {"woaheader_jobcode",wm._woa_number.ToString()},
	                };
                    string instComm = objCommonIUD.InsertCommon(codes, "fa_trn_twoaheader");
                }
                DataTable detaildt1 = new DataTable();
                detaildt1 = GetwoaheaderDetails(woanumber);
                string _woa_number = string.Empty;
                for (int r = 0; r < detaildt1.Rows.Count; r++)
                {
                    _woa_number = detaildt1.Rows[0]["woaheader_gid"].ToString().Trim();
                }
                DataTable dttm = new DataTable();
                dttm = (DataTable)assetdata;
                for (int i = 0; i < dttm.Rows.Count; i++)
                {
                    GetConnection();
                    wm = new WriteOffModel();
                    wm._asset_id = dttm.Rows[i]["Asset ID"].ToString().Trim();
                    wm._fccc = dttm.Rows[i]["CC"].ToString().Trim();
                    wm._loc = dttm.Rows[i]["Branch"].ToString().Trim();
                    wm._woa_reason = dttm.Rows[i]["Reason"].ToString().Trim();
                    wm._inw_no = dttm.Rows[i]["Inward No"].ToString().Trim();
                    wm._woa_date = dttm.Rows[i]["Write-Off Date(dd-mm-yyyy)"].ToString().Trim();
                    wm._fccc = Getfccc(wm._fccc).ToString();
                    DataTable detaildt2 = new DataTable();
                    detaildt2 = GetDetails(wm._asset_id);
                    for (int r = 0; r < detaildt2.Rows.Count; r++)
                    {
                        wm._asset_value = detaildt2.Rows[r]["assetdetails_asset_value"].ToString().Trim();
                        wm._AssetCode = detaildt2.Rows[r]["asset_code"].ToString().Trim();
                        wm._gid = Convert.ToInt32(detaildt2.Rows[r]["assetdetails_asset_code"].ToString().Trim());
                        wm._assetdet_gid = detaildt2.Rows[r]["assetdetails_gid"].ToString().Trim();
                    }
                    wm._loc = GetlocDetails(wm._loc).ToString();

                    //dep

                    DataTable dtdep = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "Totdep");
                    cmd.Parameters.AddWithValue("@assetgid", wm._assetdet_gid);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dtdep);
                    foreach (DataRow rowdep in dtdep.Rows)
                    {
                        wm.queue_from = rowdep["depreciation_amount"].ToString();
                    }
                    decimal sumassetdep = Convert.ToDecimal(wm.queue_from.ToString());
                    DataTable dtwdv = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "WDV");
                    cmd.Parameters.AddWithValue("@assetid", wm._assetdet_gid);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dtwdv);

                    //DateTime dtLeaseEnd1 =
                    DateTime dtBranchLaunch = Convert.ToDateTime("01-01-1900");
                    DateTime soacaptilisationda = Convert.ToDateTime("01-01-1900");
                    decimal AssetValue = 0;
                    string sAssetCode = string.Empty;
                    string cNot5000Case = string.Empty;
                    DateTime dtLeaseStart = Convert.ToDateTime("01-01-1900");
                    DateTime dtLeaseEnd = Convert.ToDateTime("01-01-1900");
                    string sDepType = string.Empty;
                    decimal dDepRate = 0;
                    string sAssetGroup = string.Empty;
                    //  DateTime dtTillDate = DateTime.Now;
                    DateTime dtTillDate = Convert.ToDateTime(wm._woa_date.ToString());
                    string sPONumber = string.Empty;
                    string sFICCRef = string.Empty;
                    string sAsset_GroupId = string.Empty;
                    decimal dTransfer_value = 0;
                    string CanDepreciateFully = string.Empty;
                    Int32 dDepDevRate = 0;
                    string sBranch1 = string.Empty;
                    string sBranch2 = string.Empty;
                    foreach (DataRow row1 in dtwdv.Rows)
                    {
                        dtBranchLaunch = Convert.ToDateTime(string.IsNullOrEmpty(row1["branch_start_date"].ToString()) ? "0" : row1["branch_start_date"]);
                        soacaptilisationda = Convert.ToDateTime(string.IsNullOrEmpty(row1["assetdetails_cap_date"].ToString()) ? "01-01-1900" : row1["assetdetails_cap_date"]);
                        AssetValue = Convert.ToDecimal(string.IsNullOrEmpty(row1["assetdetails_asset_value"].ToString()) ? "0" : row1["assetdetails_asset_value"]);
                        sAssetCode = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_asset_code"].ToString()) ? "0" : row1["assetdetails_asset_code"]);
                        cNot5000Case = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_not_5kcase"].ToString()) ? "0" : row1["assetdetails_not_5kcase"]);
                        dtLeaseStart = Convert.ToDateTime(string.IsNullOrEmpty(row1["assetdetails_lease_startdate"].ToString()) ? "01-01-1900" : row1["assetdetails_lease_startdate"]);
                        dtLeaseEnd = Convert.ToDateTime(string.IsNullOrEmpty(row1["assetdetails_lease_enddate"].ToString()) ? "01-01-1900" : row1["assetdetails_lease_enddate"]);
                        sDepType = Convert.ToString(string.IsNullOrEmpty(row1["asset_dep_type"].ToString()) ? "0" : row1["asset_dep_type"]);
                        dDepRate = Convert.ToDecimal(string.IsNullOrEmpty(row1["asset_dep_rate"].ToString()) ? "0" : row1["asset_dep_rate"]);
                        sAssetGroup = Convert.ToString(string.IsNullOrEmpty(row1["h_asst_groupid_no"].ToString()) ? "0" : row1["h_asst_groupid_no"]);
                        sPONumber = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_po_number"].ToString()) ? "0" : row1["assetdetails_po_number"]);
                        sFICCRef = Convert.ToString(string.IsNullOrEmpty(row1["h_ficrefnumber"].ToString()) ? "0" : row1["h_ficrefnumber"]);
                        sAsset_GroupId = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_asset_groupid"].ToString()) ? "0" : row1["assetdetails_asset_groupid"]);
                        dTransfer_value = Convert.ToDecimal(string.IsNullOrEmpty(row1["assetdetails_trf_value"].ToString()) ? "0" : row1["assetdetails_trf_value"]);
                    }

                    decimal depamt = Math.Round(GetTotalDep(soacaptilisationda, dtTillDate,
                                    AssetValue, sAssetCode,
                                 cNot5000Case, sBranch1, sBranch2,
                                 dtBranchLaunch, dtLeaseStart, dtLeaseEnd,
                                 sDepType, dDepRate,
                                 sAssetGroup,
                                 sPONumber,
                                 sFICCRef,
                                 sAsset_GroupId,
                                 dTransfer_value,
                                 CanDepreciateFully,
                                 dDepDevRate), 2);
                    decimal rectificationamt = depamt - sumassetdep;
                    decimal wdv = Convert.ToDecimal(wm._asset_value) - depamt;
                    //decimal wdv = Convert.ToDecimal(wm._asset_value) - sumassetdep;  Muthu


                    string[,] codes = new string[,]
	                {
                             {"woadetail_header_gid",_woa_number.ToString()},
                             {"woadetail_assetdet_gid",wm._assetdet_gid.ToString()},
                             {"woadetail_asset_value", wm._asset_value.ToString()},
                             {"woadetail_wdv_value", wdv.ToString()},
                             {"woadetail_woa_date",convertoDate(wm._woa_date).ToString()},                          
                             {"woadetail_reason",wm._woa_reason.ToString()},
                             {"woadetail_rectif_amount",rectificationamt.ToString()},
                             {"woadetail_value_date",Convert.ToString(objCommonIUD.GetCurrentDate())},
                             {"woadetail_fccc_gid",wm._fccc.ToString()},
                             {"woadetail_loc_gid",wm._loc.ToString()},  
                             {"woadetail_inw_no", wm._inw_no.ToString()},
                             {"woadetail_jobcode",woanumber.ToString()},
                             {"woadetail_isremoved","N"},
                             {"woadetail_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                             {"woadetail_insert_date",Convert.ToString(objCommonIUD.GetCurrentDate())},
	                };
                    string tblname = "fa_trn_twoadetail";
                    string instComm = objCommonIUD.InsertCommon(codes, tblname);



                    string[,] ASScodes1 = new string[,]
	                        {                                                      
                                {"assetdetails_takenby","11"},                              
                                {"assetdetails_update_by",objCmnFunctions.GetLoginUserGid().ToString()},    
                                {"assetdetails_update_date",Convert.ToString(objCommonIUD.GetCurrentDate())},               
	                       };
                    string[,] ASSwhrcol = new string[,]
	                        {
                                {"assetdetails_gid", wm._assetdet_gid.ToString()},
                                {"assetdetails_isremoved", "N"} ,   
                            };
                    string ASStblname1 = "fa_trn_tassetdetails";
                    string ASSupdcomm = objCommonIUD.UpdateCommon(ASScodes1, ASSwhrcol, ASStblname1);
                }


                return "Success";
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return string.Empty;
            }
            finally
            {
                con.Close();
            }
        }
        public string AppRejStatus(string id, string status, string remarks)
        {
            string Result = string.Empty;
            try
            {
                GetConnection();

                DataTable dt = new DataTable();
                cmd = new SqlCommand("Pr_fa_set_WOAChkrApprove", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@WOANumber", id.ToString());
                cmd.Parameters.AddWithValue("@Remarks", remarks.ToString());
                //cmd.Parameters.AddWithValue("@status",status.ToString());
                cmd.Parameters.AddWithValue("@action", status.ToString());
                cmd.Parameters.AddWithValue("@Userid ", objCmnFunctions.GetLoginUserGid().ToString());
                //cmd.Parameters.AddWithValue("@newBranchcode", );
                cmd.CommandTimeout = 0;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    Result = dt.Rows[0][0].ToString();
                }
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                Result = ex.Message.ToString();
            }

            finally
            {
                con.Close();
            }
            return Result;
        }
        #region
        /*public string AppRejStatus(string id, string status, string remarks)
        {
            try
            {
                //get details
                string _woa_number = string.Empty;
                string _woa_MAKER = string.Empty;
                string amt = string.Empty;
                DataTable detaildt1 = new DataTable();
                detaildt1 = GetwoaheaderDetails(id);
                for (int r = 0; r < detaildt1.Rows.Count; r++)
                {
                    _woa_number = detaildt1.Rows[0]["woaheader_gid"].ToString().Trim();
                    // _woa_MAKER = detaildt1.Rows[0]["woaheader_gid"].ToString().Trim();
                    _woa_MAKER = detaildt1.Rows[0]["employee_gid"].ToString().Trim();
                }
                DataTable dt2 = new DataTable();
                cmd = new SqlCommand("[pr_ifams_trn_TransferMaker]", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@headergid", SqlDbType.VarChar).Value = "WOA".Trim();
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "refno";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt2);
                string refflag = string.Empty;
                foreach (DataRow row in dt2.Rows)
                {
                    refflag = row["ref_flag"].ToString();
                }

                if (status == "REJECTED")
                {
                    cmd = new SqlCommand("pr_ifams_trn_woaMaker", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "abort";
                    cmd.Parameters.Add("@headergid", SqlDbType.VarChar).Value = _woa_number;
                    da = new SqlDataAdapter(cmd);
                    dt2 = new DataTable();
                    da.Fill(dt2);
                    foreach (DataRow row in dt2.Rows)
                    {
                        cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = row["woadetail_assetdet_gid"].ToString();
                        cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "takenbyid";
                        string takenby = Convert.ToString(cmd.ExecuteScalar());

                        if (takenby == "11")
                        {
                            takenby = "14";
                        }
                        if (takenby == "23")
                        {
                            takenby = "21";
                        }

                        string[,] ASScodes1 = new string[,]
	                        {
                                
                                {"assetdetails_update_by",objCmnFunctions.GetLoginUserGid().ToString()},                      
                                {"assetdetails_update_date",objCommonIUD.GetCurrentDate()}, 
                                {"assetdetails_takenby", takenby} ,              
	                       };
                        string[,] ASSwhrcol = new string[,]
	                        {
                                    {"assetdetails_gid", row["woadetail_assetdet_gid"].ToString()}, 
                                    {"assetdetails_isremoved", "N"} ,   
                            };
                        string assupdcmd = objCommonIUD.UpdateCommon(ASScodes1, ASSwhrcol, "fa_trn_tassetdetails");
                    }
                    //update rej in queue
                    string[,] colm = new string[,]
                    {
                             {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                             {"queue_action_status","2"},
                             {"queue_action_date",objCommonIUD.GetCurrentDate()}   ,
                             {"queue_isremoved","Y"},  
                             {"queue_action_remark",remarks},
                                              
                    };
                    string[,] whrecol = new string[,]
                    {
                              {"queue_ref_gid", _woa_number.Trim()} ,    
                              {"queue_ref_flag", refflag.Trim()}                                           
                    };
                    string updcmd = objCommonIUD.UpdateCommon(colm, whrecol, "iem_trn_tqueue");
                }

                else
                {
                    //update approve in asset
                    cmd = new SqlCommand("pr_ifams_trn_woaMaker", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "abort";
                    cmd.Parameters.Add("@headergid", SqlDbType.VarChar).Value = _woa_number;
                    da = new SqlDataAdapter(cmd);
                    dt2 = new DataTable();
                    da.Fill(dt2);
                    foreach (DataRow row in dt2.Rows)
                    {

                        //dep

                        DataTable dtdep = new DataTable();
                        cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "Totdep");
                        cmd.Parameters.AddWithValue("@assetgid", row["woadetail_assetdet_gid"]);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dtdep);
                        foreach (DataRow rowdep in dtdep.Rows)
                        {
                            amt = rowdep["depreciation_amount"].ToString();
                        }
                        decimal sumassetdep = Convert.ToDecimal(amt.ToString());
                        DataTable dtwdv = new DataTable();
                        cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "WDV");
                        cmd.Parameters.AddWithValue("@assetid", row["woadetail_assetdet_gid"].ToString());
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dtwdv);

                        //DateTime dtLeaseEnd1 =
                        DateTime dtBranchLaunch = Convert.ToDateTime("01-01-1900");
                        DateTime soacaptilisationda = Convert.ToDateTime("01-01-1900");
                        decimal AssetValue = 0;
                        string sAssetCode = string.Empty;
                        string cNot5000Case = string.Empty;
                        DateTime dtLeaseStart = Convert.ToDateTime("01-01-1900");
                        DateTime dtLeaseEnd = Convert.ToDateTime("01-01-1900");
                        string sDepType = string.Empty;
                        decimal dDepRate = 0;
                        string sAssetGroup = string.Empty;
                        // DateTime dtTillDate = DateTime.Now;
                        DateTime dtTillDate = Convert.ToDateTime(row["woadetail_woa_date"].ToString());
                        string sPONumber = string.Empty;
                        string sFICCRef = string.Empty;
                        string sAsset_GroupId = string.Empty;
                        decimal dTransfer_value = 0;
                        string CanDepreciateFully = string.Empty;
                        Int32 dDepDevRate = 0;
                        string sBranch1 = string.Empty;
                        string sBranch2 = string.Empty;
                        foreach (DataRow row1 in dtwdv.Rows)
                        {
                            dtBranchLaunch = Convert.ToDateTime(string.IsNullOrEmpty(row1["branch_start_date"].ToString()) ? "0" : row1["branch_start_date"]);
                            soacaptilisationda = Convert.ToDateTime(string.IsNullOrEmpty(row1["assetdetails_cap_date"].ToString()) ? "01-01-1900" : row1["assetdetails_cap_date"]);
                            AssetValue = Convert.ToDecimal(string.IsNullOrEmpty(row1["assetdetails_asset_value"].ToString()) ? "0" : row1["assetdetails_asset_value"]);
                            sAssetCode = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_asset_code"].ToString()) ? "0" : row1["assetdetails_asset_code"]);
                            cNot5000Case = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_not_5kcase"].ToString()) ? "0" : row1["assetdetails_not_5kcase"]);
                            dtLeaseStart = Convert.ToDateTime(string.IsNullOrEmpty(row1["assetdetails_lease_startdate"].ToString()) ? "01-01-1900" : row1["assetdetails_lease_startdate"]);
                            dtLeaseEnd = Convert.ToDateTime(string.IsNullOrEmpty(row1["assetdetails_lease_enddate"].ToString()) ? "01-01-1900" : row1["assetdetails_lease_enddate"]);
                            sDepType = Convert.ToString(string.IsNullOrEmpty(row1["asset_dep_type"].ToString()) ? "0" : row1["asset_dep_type"]);
                            dDepRate = Convert.ToDecimal(string.IsNullOrEmpty(row1["asset_dep_rate"].ToString()) ? "0" : row1["asset_dep_rate"]);
                            sAssetGroup = Convert.ToString(string.IsNullOrEmpty(row1["h_asst_groupid_no"].ToString()) ? "0" : row1["h_asst_groupid_no"]);
                            sPONumber = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_po_number"].ToString()) ? "0" : row1["assetdetails_po_number"]);
                            sFICCRef = Convert.ToString(string.IsNullOrEmpty(row1["h_ficrefnumber"].ToString()) ? "0" : row1["h_ficrefnumber"]);
                            sAsset_GroupId = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_asset_groupid"].ToString()) ? "0" : row1["assetdetails_asset_groupid"]);
                            dTransfer_value = Convert.ToDecimal(string.IsNullOrEmpty(row1["assetdetails_trf_value"].ToString()) ? "0" : row1["assetdetails_trf_value"]);
                        }

                        decimal depamt = Math.Round(GetTotalDep(soacaptilisationda, dtTillDate,
                                        AssetValue, sAssetCode,
                                     cNot5000Case, sBranch1, sBranch2,
                                     dtBranchLaunch, dtLeaseStart, dtLeaseEnd,
                                     sDepType, dDepRate,
                                     sAssetGroup,
                                     sPONumber,
                                     sFICCRef,
                                     sAsset_GroupId,
                                     dTransfer_value,
                                     CanDepreciateFully,
                                     dDepDevRate), 2);
                        decimal rectificationamt = depamt - sumassetdep;

                        //if (sDepType == "SLM" || sDepType == string.Empty)
                        //{

                        //    sDepType = "S";
                        //}
                        //else if (sDepType == "LPM")
                        //{
                        //    sDepType = "L";
                        //}
                        //if (rectificationamt > 0)
                        //{
                        string[,] codesdep = new string[,]
                            {
                                {"depreciation_amount", rectificationamt.ToString()},
                                {"depreciation_assetdet_gid", row["woadetail_assetdet_gid"].ToString()},
                                {"depreciation_month", Convert.ToString(convertoDate (DateTime.Today.ToShortDateString()))},
                                {"depreciation_asset_groupid", sAsset_GroupId.ToString()},
                                {"depreciation_asset_owner", "F"},
                                {"depreciation_type","W".ToString()},
                                {"depreciation_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                                {"depreciation_insert_date", Convert.ToString(convertoDate (DateTime.Today.ToShortDateString()))}
                            };
                        string insertcmn = objCommonIUD.InsertCommon(codesdep, "fa_trn_tdepreciation");
                        //}
                        //ENTRY FOR GL UPLOAD
                        cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "UploadDetails";
                        cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = row["woadetail_assetdet_gid"].ToString();
                        da = new SqlDataAdapter(cmd);
                        string glCode = string.Empty;
                        string resdepglCode = string.Empty;
                        string depglCode = string.Empty;
                        string branch = string.Empty;
                        string cat = string.Empty;
                        string subcat = string.Empty;
                        string asset_id = string.Empty;
                        string BS = string.Empty;
                        string CC = string.Empty;
                        string ratio = string.Empty;
                        DataTable UPLOADDATATBL = new DataTable();
                        da.Fill(UPLOADDATATBL);
                        if (UPLOADDATATBL.Rows.Count > 0)
                            foreach (DataRow rowdep in UPLOADDATATBL.Rows)
                            {
                                glCode = UPLOADDATATBL.Rows[0]["asset_glcode"].ToString();
                                resdepglCode = UPLOADDATATBL.Rows[0]["asset_dep_reservglcode"].ToString();
                                depglCode = UPLOADDATATBL.Rows[0]["asset_dep_glcode"].ToString();
                                branch = UPLOADDATATBL.Rows[0]["branch_code"].ToString();
                                cat = UPLOADDATATBL.Rows[0]["assetcategory_name"].ToString();
                                if (cat.Length > 16)
                                    cat = cat.Substring(0, 16);
                                subcat = UPLOADDATATBL.Rows[0]["asset_code"].ToString();
                                asset_id = UPLOADDATATBL.Rows[0]["assetdetails_assetdet_id"].ToString();
                            }

                        cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "BSCC_upload_details";
                        cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = subcat;
                        cmd.Parameters.Add("@fccc", SqlDbType.VarChar).Value = branch;
                        da = new SqlDataAdapter(cmd);
                        UPLOADDATATBL = new DataTable();
                        da.Fill(UPLOADDATATBL);
                        if (UPLOADDATATBL.Rows.Count > 0)
                            foreach (DataRow rowdep in UPLOADDATATBL.Rows)
                            {
                                BS = UPLOADDATATBL.Rows[0]["depreciationbscc_bs"].ToString();
                                CC = UPLOADDATATBL.Rows[0]["depreciationbscc_cc_oracle"].ToString();
                            }
                        //if (rectificationamt != 0)
                        //{

                        DataTable datamaker_id = objCmnFunctions.GetLoginUserDetails(Convert.ToInt32(_woa_MAKER));
                        DataTable dtdeptrn = new DataTable();
                        cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "Totdep");
                        cmd.Parameters.AddWithValue("@assetgid", row["woadetail_assetdet_gid"]);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dtdeptrn);
                        foreach (DataRow rowdep in dtdeptrn.Rows)
                        {
                            amt = rowdep["depreciation_amount"].ToString();
                        }
                        sumassetdep = Convert.ToDecimal(amt.ToString());
                        string inw_no = row["woadetail_inw_no"].ToString();
                        string[,] glCoulmsDepD = new string[,]
                            {                                
                                {"tran_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_val_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_payment_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_gl_no",resdepglCode},
                                {"tran_desc",asset_id.ToString() + " | WRITTEN-OFF OF ASSET | " + inw_no},
                                //{"tran_amount",( rectificationamt).ToString()},sumassetdep
                                {"tran_amount",(sumassetdep).ToString()},
                                {"tran_acc_mode","D".ToString()},
                                {"tran_mult","-1"},  
                                {"tran_fc_code",BS},
                                {"tran_cc_code",CC},                                
                                {"tran_ou_code",branch},
                                {"tran_product_code","500".ToString()},
                                {"tran_ref_flag",GetRef("WOA")},
                                {"tran_ref_gid",row["woadetail_assetdet_gid"].ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                            {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                {"tran_expense_category","9"},
                                {"tran_primary_branch_code",branch.ToString()},  
                                // {"tran_invoice_no",_woa_number.Trim().ToString()},                              
                                {"tran_expense_month",convertoDate (DateTime.Today.ToShortDateString())},
                                // {"tran_maker_id",objCmnFunctions.GetLoginUserGid().ToString()},
                                {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                        string insertforglDepD = objCommonIUD.InsertCommon(glCoulmsDepD, "fa_trn_ttran");
                        //}
                        if (AssetValue - rectificationamt != 0)
                        {
                            string[,] glCoulmsD = new string[,]
                            {                                
                                {"tran_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_val_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_payment_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_gl_no","441510001"},
                                  {"tran_desc",asset_id.ToString() + " | WRITTEN-OFF OF ASSET | " + inw_no},
                                //  {"tran_amount",(AssetValue - rectificationamt).ToString()}, Muthu
                                {"tran_amount",(AssetValue - sumassetdep).ToString()},
                                {"tran_acc_mode","D".ToString()},
                                {"tran_mult","-1"},  
                                {"tran_fc_code",BS},
                                {"tran_cc_code",CC},                                
                                {"tran_ou_code",branch},
                                {"tran_ref_flag",GetRef("WOA")},
                                {"tran_ref_gid",row["woadetail_assetdet_gid"].ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_product_code","500".ToString()},
                                {"tran_isremoved","N"},
                            {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                {"tran_expense_category","9"},
                                {"tran_primary_branch_code",branch.ToString()},
                                {"tran_secondary_branch_code",string.Empty},
                                {"tran_related_account",string.Empty},
                                //    {"tran_invoice_no",_woa_number.Trim().ToString()},                               
                                {"tran_expense_month",convertoDate (DateTime.Today.ToShortDateString())},
                                //   {"tran_maker_id",_woa_MAKER.ToString()},
                                {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                            string insertforglD = objCommonIUD.InsertCommon(glCoulmsD, "fa_trn_ttran");

                        }

                        string[,] glCoulmsC = new string[,]
                                {                                
                                {"tran_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_val_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_payment_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_gl_no",glCode},
                                {"tran_desc",asset_id.ToString() + " | WRITTEN-OFF OF ASSET | " + inw_no},
                                {"tran_amount",AssetValue.ToString()},
                                {"tran_acc_mode","C".ToString()},
                                {"tran_mult","1"},  
                                {"tran_fc_code",BS},
                                {"tran_cc_code",CC},
                                {"tran_ou_code",branch},
                                {"tran_product_code","500".ToString()},
                                {"tran_ref_flag",GetRef("WOA")},
                                {"tran_ref_gid",row["woadetail_assetdet_gid"].ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                            {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                {"tran_expense_category","9"},
                                {"tran_primary_branch_code",branch.ToString()},
                                {"tran_secondary_branch_code",string.Empty},
                                {"tran_related_account",string.Empty},
                                // {"tran_invoice_no",_woa_number.Trim().ToString()},                              
                                {"tran_expense_month",convertoDate (DateTime.Today.ToShortDateString())},
                                //  {"tran_maker_id",_woa_MAKER.ToString()},
                                {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                        string insertforglC = objCommonIUD.InsertCommon(glCoulmsC, "fa_trn_ttran");



                        //tran table insert for writeoff
                        if (rectificationamt != 0)
                        {
                            if (rectificationamt < 0)
                            {
                                rectificationamt = Math.Abs(rectificationamt);


                                string[,] glCoulmsimpdepC = new string[,]
                            {                                
                                {"tran_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_val_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_payment_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_gl_no",resdepglCode},
                                {"tran_desc", asset_id.ToString() +" | DEP FOR ASSET WRITE-OFF " },
                              //  {"tran_amount",(AssetValue - rectificationamt).ToString()}, Muthu
                                {"tran_amount",rectificationamt.ToString()},
                                {"tran_acc_mode","D".ToString()},
                                {"tran_mult","-1"},  
                                {"tran_fc_code",BS},
                                {"tran_cc_code",CC},                                
                                {"tran_ou_code",branch},
                                {"tran_ref_flag",GetRef("WDEP")},
                                {"tran_ref_gid",row["woadetail_assetdet_gid"].ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_product_code","500".ToString()},
                                {"tran_isremoved","N"},
                            {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                {"tran_expense_category","10"},
                                {"tran_primary_branch_code",branch.ToString()},
                                {"tran_secondary_branch_code",string.Empty},
                                {"tran_related_account",string.Empty},
//{"tran_invoice_no",_woa_number.Trim().ToString()},                               
                                {"tran_expense_month",convertoDate (DateTime.Today.ToShortDateString())},
                             //   {"tran_maker_id",_woa_MAKER.ToString()},
                             {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                  {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                   {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                                string insertforimpdepglC = objCommonIUD.InsertCommon(glCoulmsimpdepC, "fa_trn_ttran");


                                string[,] glCoulmsimpdepD = new string[,]
                            {                                
                                {"tran_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_val_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_payment_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_gl_no",depglCode},
                                {"tran_desc",asset_id.ToString() +" | DEP FOR ASSET WRITE-OFF "},
                              //  {"tran_amount",(AssetValue - rectificationamt).ToString()}, Muthu
                                {"tran_amount",rectificationamt.ToString()},
                                {"tran_acc_mode","C".ToString()},
                                {"tran_mult","-1"},  
                                {"tran_fc_code",BS},
                                {"tran_cc_code",CC},                                
                                {"tran_ou_code",branch},
                                {"tran_ref_flag",GetRef("WDEP")},
                                {"tran_ref_gid",row["woadetail_assetdet_gid"].ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_product_code","500".ToString()},
                                {"tran_isremoved","N"},
                            {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                {"tran_expense_category","10"},
                                {"tran_primary_branch_code",branch.ToString()},
                                {"tran_secondary_branch_code",string.Empty},
                                {"tran_related_account",string.Empty},
                             //   {"tran_invoice_no",_woa_number.Trim().ToString()},                               
                                {"tran_expense_month",convertoDate (DateTime.Today.ToShortDateString())},
                             //   {"tran_maker_id",_woa_MAKER.ToString()},
                             {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                  {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                   {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                                string insertforimpdepglD = objCommonIUD.InsertCommon(glCoulmsimpdepD, "fa_trn_ttran");
                            }

                            else
                            {
                                string[,] glCoulmsimpdepC = new string[,]
                            {                                
                                {"tran_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_val_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_payment_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_gl_no",depglCode},
                                {"tran_desc",asset_id.ToString() +" | DEP FOR ASSET WRITE-OFF " },
                              //  {"tran_amount",(AssetValue - rectificationamt).ToString()}, Muthu
                                {"tran_amount",rectificationamt.ToString()},
                                {"tran_acc_mode","D".ToString()},
                                {"tran_mult","-1"},  
                                {"tran_fc_code",BS},
                                {"tran_cc_code",CC},                                
                                {"tran_ou_code",branch},
                                {"tran_ref_flag",GetRef("WDEP")},
                                {"tran_ref_gid",row["woadetail_assetdet_gid"].ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_product_code","500".ToString()},
                                {"tran_isremoved","N"},
                            {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                {"tran_expense_category","10"},
                                {"tran_primary_branch_code",branch.ToString()},
                                {"tran_secondary_branch_code",string.Empty},
                                {"tran_related_account",string.Empty},
//{"tran_invoice_no",_woa_number.Trim().ToString()},                               
                                {"tran_expense_month",convertoDate (DateTime.Today.ToShortDateString())},
                             //   {"tran_maker_id",_woa_MAKER.ToString()},
                             {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                  {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                   {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                                string insertforimpdepglC = objCommonIUD.InsertCommon(glCoulmsimpdepC, "fa_trn_ttran");


                                string[,] glCoulmsimpdepD = new string[,]
                            {                                
                                {"tran_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_val_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_payment_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_gl_no",resdepglCode},
                                {"tran_desc",asset_id.ToString() +" | DEP FOR ASSET WRITE-OFF "},
                              //  {"tran_amount",(AssetValue - rectificationamt).ToString()}, Muthu
                                {"tran_amount",rectificationamt.ToString()},
                                {"tran_acc_mode","C".ToString()},
                                {"tran_mult","-1"},  
                                {"tran_fc_code",BS},
                                {"tran_cc_code",CC},                                
                                {"tran_ou_code",branch},
                                {"tran_ref_flag",GetRef("WDEP")},
                                {"tran_ref_gid",row["woadetail_assetdet_gid"].ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_product_code","500".ToString()},
                                {"tran_isremoved","N"},
                            {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                               {"tran_expense_category","10"},
                                {"tran_primary_branch_code",branch.ToString()},
                                {"tran_secondary_branch_code",string.Empty},
                                {"tran_related_account",string.Empty},
                             //   {"tran_invoice_no",_woa_number.Trim().ToString()},                               
                                {"tran_expense_month",convertoDate (DateTime.Today.ToShortDateString())},
                             //   {"tran_maker_id",_woa_MAKER.ToString()},
                             {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                  {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                   {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                                string insertforimpdepglD = objCommonIUD.InsertCommon(glCoulmsimpdepD, "fa_trn_ttran");
                            }
                        }


                        cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = row["woadetail_assetdet_gid"].ToString();
                        cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "takenbyid";
                        string takenby = Convert.ToString(cmd.ExecuteScalar());

                        if (takenby == "11")
                        {
                            takenby = "5";
                        }
                        if (takenby == "25")
                        {
                            takenby = "23";
                        }

                        string[,] ASScodes1 = new string[,]
	                        {
                               // {"assetdetails_asset_value","0"}, 
                                {"assetdetails_active_status","N"}, 
                                {"assetdetails_update_by",objCmnFunctions.GetLoginUserGid().ToString()},                      
                                {"assetdetails_update_date",objCommonIUD.GetCurrentDate()}, 
                                {"assetdetails_isremoved", "Y"} ,  
                                {"assetdetails_takenby", takenby} ,
                                {"assetdetails_wrt_date", convertoDate(row["woadetail_woa_date"].ToString())},
	                       };
                        string[,] ASSwhrcol = new string[,]
	                        {
                                    {"assetdetails_gid", row["woadetail_assetdet_gid"].ToString()}, 
                                    {"assetdetails_isremoved", "N"} ,   
                            };
                        string assupdcmd = objCommonIUD.UpdateCommon(ASScodes1, ASSwhrcol, "fa_trn_tassetdetails");
                    }
                    //update approve in queue
                    string[,] colm = new string[,]
                    {
                             {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                             {"queue_action_status","1"},
                             {"queue_action_date",objCommonIUD.GetCurrentDate()},
                             {"queue_isremoved","Y"},
                             {"queue_action_remark",remarks},
                                              
                    };
                    string[,] whrecol = new string[,]
                    {
                              {"queue_ref_gid", _woa_number.Trim()} ,    
                              {"queue_ref_flag", refflag.Trim()}                                           
                    };
                    string updcmd = objCommonIUD.UpdateCommon(colm, whrecol, "iem_trn_tqueue");
                }

                DataTable dt1 = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = status.Trim();
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "status";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt1);
                string stt = string.Empty;
                foreach (DataRow row in dt1.Rows)
                {
                    stt = row["status_flag"].ToString();
                }
                TransferMakerModel obj = new TransferMakerModel();
                string[,] codes1 = new string[,]
                    {
                             {"woaheader_woa_status",stt},
                             {"woaheader_checker_id",objCmnFunctions.GetLoginUserGid().ToString()},
                             {"woaheader_checker_date",objCommonIUD.GetCurrentDate()},
                             {"woaheader_update_by",objCmnFunctions.GetLoginUserGid().ToString()},
                             {"woaheader_update_date",objCommonIUD.GetCurrentDate()},
                             {"woaheader_remarks ",remarks},
                             {"woaheader_process_date",objCommonIUD.GetCurrentDate()}
                    };
                string[,] whrcol = new string[,]
                    {
                              {"woaheader_gid", _woa_number.Trim()}                                                                              
                    };
                string updcomm = objCommonIUD.UpdateCommon(codes1, whrcol, "fa_trn_twoaheader");


                return status;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return string.Empty;
            }
            finally
            {
                con.Close();
            }
        }*/
        #endregion
        public List<BarcodeGenerationModel> GetDetailForBarcode(string loccode, string assetcode, string Dp_type, string capdate, string captodate)
        {
            List<BarcodeGenerationModel> objModel = new List<BarcodeGenerationModel>();
            try
            {

                BarcodeGenerationModel obj = new BarcodeGenerationModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_barcodegeneration", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "GETDATA";
                cmd.Parameters.Add("@BranchCode", SqlDbType.VarChar).Value = loccode;
                cmd.Parameters.Add("@Assetcode", SqlDbType.VarChar).Value = assetcode;
                cmd.Parameters.Add("@Dep_type", SqlDbType.VarChar).Value = Dp_type;
                cmd.Parameters.Add("@capFromdate", SqlDbType.VarChar).Value = capdate;
                cmd.Parameters.Add("@capTodate", SqlDbType.VarChar).Value = captodate;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new BarcodeGenerationModel();
                    obj._asset_id = row["assetdetails_assetdet_id"].ToString();
                    obj._asset_sno = row["assetdetails_asset_serialno"].ToString();
                    obj._assetCode = row["assetcategory_name"].ToString();
                    obj._assetcode_gid = row["asset_code"].ToString();
                    obj._assetDesp = row["asset_description"].ToString();
                    obj._assetdet_gid = row["assetdetails_gid"].ToString();
                    obj._cap_date = row["assetdetails_cap_date"].ToString();
                    //obj._cwip_ecf_number = row["cwip_ecf_number"].ToString();
                    obj._inv_no = row["requestfor_name"].ToString();
                    //obj._inv_no = row["invoice_no"].ToString();
                    obj._loc_gid = row["assetdetails_branch_gid"].ToString();
                    obj._loc = row["branch_code"].ToString();
                    objModel.Add(obj);
                }
                return objModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return objModel;
            }
            finally
            {
                con.Close();
            }
        }

        public string GenerateBarcode(List<string> id)
        {
            BarcodeGenerationModel bgm = new BarcodeGenerationModel();
            try
            {
                string barcode = string.Empty;
                for (int i = 0; i < id.Count; i++)
                {
                    GetConnection();
                    DataTable dt = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_barcodegeneration", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "GENERATIONDETAILS";
                    cmd.Parameters.Add("@assetid", SqlDbType.Int).Value = Convert.ToInt32(id[i]);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        bgm._assetCode = row["asset_code"].ToString();
                        bgm._assetcode_gid = row["assetdetails_asset_code"].ToString();
                        bgm._loc_gid = row["assetdetails_branch_gid"].ToString();
                    }

                    barcode = GetBarcodeSequence(bgm._assetCode, bgm._assetcode_gid);
                    string[,] valuescol = new string[,]
	                        { 
                                {"barcodedetials_assetdet_gid",id[i]},                             
                                {"barcodedetials_branch_gid", bgm._loc_gid},
                                {"barcodedetials_bar_no",barcode}, 
                                {"barcodedetials_bar_status","NOT PRINTED".ToString()},                             
                                {"barcodedetails_insert_by",objCmnFunctions.GetLoginUserGid().ToString()},
                                {"barcodedetails_insert_date",objCommonIUD.GetCurrentDate()}, 
                                {"barcodedetials_isreemoved","N".ToString()}                                
	                        };
                    string InsertComm = objCommonIUD.InsertCommon(valuescol, "fa_trn_tbarcodedetails");

                    GetConnection();
                    dt = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_barcodegeneration", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "BARCODEID";
                    cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = id[i].ToString();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        bgm._barcode = row["barcodedetails_gid"].ToString();
                    }

                    string[,] col = new string[,]
	                        { 
                                {"assetdetails_barcode", bgm._barcode.ToString()},                             
                                {"assetdetails_update_by",objCmnFunctions.GetLoginUserGid().ToString()},
                                {"assetdetails_update_date",objCommonIUD.GetCurrentDate()}                        
	                        };
                    string[,] Whrcol = new string[,]
	                        {                                                              
                                {"assetdetails_gid",id[i]}                             
	                        };
                    string updateComm = objCommonIUD.UpdateCommon(col, Whrcol, "fa_trn_tassetdetails");
                }


                return "Success";
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "Failed";
            }
            finally
            {
                con.Close();
            }
        }

        public string GetBarcodeSequence(string assetCode, string assetcode_gid)
        {
            string result = string.Empty;
            try
            {
                int lsSequence = 0;
                string lsQry = string.Empty;
                string lsQry1 = string.Empty;
                DataTable dt = new DataTable();
                CommonIUD objIUD = new CommonIUD();
                GetConnection();
                lsQry = "select  assetseqno_no,assetseqno_assetcode  from fa_trn_tassetseqno";
                lsQry += " where assetseqno_assetcode = '" + assetCode + "'";
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = lsQry;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    lsSequence = Convert.ToInt32(dt.Rows[0]["assetseqno_no"]) + 1;
                    string[,] codes = new string[,]
	                   { 
                             {"assetseqno_no",lsSequence.ToString()},                            
                       };
                    string[,] Whrcodes = new string[,]
	                   {                              
                             {"assetseqno_assetcode",assetCode.ToString()}
                       };
                    string instComm = objIUD.UpdateCommon(codes, Whrcodes, "fa_trn_tassetseqno");
                    result = assetCode + lsSequence.ToString("0000000");
                }
                else
                {
                    lsSequence = 1;
                    string[,] codes = new string[,]
	                   { 
                             {"assetseqno_no",lsSequence.ToString()},
                             {"assetseqno_asset_gid",assetcode_gid},
                             {"assetseqno_assetcode",assetCode.ToString()}
                       };
                    string instComm = objIUD.InsertCommon(codes, "fa_trn_tassetseqno");
                    result = assetCode + lsSequence.ToString("0000000");
                }
                //barcode(result);

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public string GetGroupIDSequence()
        {
            string result = string.Empty;
            try
            {
                int lsSequence = 0;
                string lsQry = string.Empty;
                DataTable dt = new DataTable();
                CommonIUD objIUD = new CommonIUD();
                GetConnection();
                lsQry = "select  sequenceno_no  from fa_mst_tsequenceno";
                lsQry += " where sequenceno_code = 'GROUP'";
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = lsQry;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                lsSequence = Convert.ToInt32(dt.Rows[0]["sequenceno_no"]) + 1;
                string[,] codes = new string[,]
	                   { 
                             {"sequenceno_no",lsSequence.ToString()}                            
                       };
                string[,] whrcodes = new string[,]
	                   { 
                             {"sequenceno_code","GROUP"}                            
                       };
                string Comm = objIUD.UpdateCommon(codes, whrcodes, "fa_mst_tsequenceno");
                result = lsSequence.ToString("0000000");

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public List<BarcodeGenerationModel> GetddlStatus()
        {
            List<BarcodeGenerationModel> Model = new List<BarcodeGenerationModel>();
            string status = string.Empty;
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_barcodegeneration", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "STATUS";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                BarcodeGenerationModel obj = new BarcodeGenerationModel();
                foreach (DataRow row in dt.Rows)
                {
                    obj = new BarcodeGenerationModel();
                    obj._fccc = row["barcodedetials_bar_status"].ToString();
                    Model.Add(obj);
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
            return Model;
        }

        public string GetddlSource()
        {
            string status = string.Empty;
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_barcodegeneration", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "SOURCE";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    status = row["assetsource_name"].ToString();
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
            return status;
        }


        public string Barcode(List<string> barcodeid)
        {
            string barcodepath = ConfigurationManager.AppSettings["Barcodepath"].ToString();
            BarcodeGenerationModel bgm = new BarcodeGenerationModel();

            string filename = string.Format("Barcode{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now);
            try
            {
                //using (MemoryStream ms = new MemoryStream())
                //{
                Document pdfdoc = new Document();
                iTextSharp.text.Rectangle pagesize = new iTextSharp.text.Rectangle(350, 160, 10, 0);
                pdfdoc = new Document(pagesize, 0, 0, 0, 0);
                FileStream stream = new FileStream(barcodepath + filename + ".pdf", FileMode.Create);
                PdfWriter writer = PdfWriter.GetInstance(pdfdoc, stream);
                PdfPTable tbl = new PdfPTable(3);
                tbl.SpacingBefore = 2;
                tbl.SpacingAfter = 2;
                float[] widths = {
       		                40f,
                            50f,
                            40f
        	                };
                tbl.SetWidths(widths);
                tbl.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                tbl.DefaultCell.HorizontalAlignment = Element.ALIGN_CENTER;
                tbl.DefaultCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                pdfdoc.Open();
                for (int i = 0; i < barcodeid.Count; i++)
                {
                    for (int j = 0; j < 1; j++)
                    {
                        bgm = new BarcodeGenerationModel();
                        GetConnection();
                        DataTable dt = new DataTable();
                        cmd = new SqlCommand("pr_ifams_trn_barcodegeneration", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "ASSETID";
                        cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = barcodeid[i].ToString();
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        foreach (DataRow row in dt.Rows)
                        {
                            bgm._barcode = row["barcodedetials_bar_no"].ToString();
                            bgm._barcode_status = row["barcodedetials_bar_status"].ToString();
                            bgm._asset_type = row["asset_type"].ToString();
                            if (bgm._asset_type == "M")
                            {
                                bgm._asset_type = "Moveable Asset";
                            }
                            else
                            {
                                bgm._asset_type = "Non-Moveable Asset";
                            }
                            bgm._asset_sno = row["assetdetails_asset_serialno"].ToString();
                            bgm._asset_id = row["assetdetails_assetdet_id"].ToString();
                            bgm._loc = row["branch_code"].ToString();
                        }
                        string strcode = bgm._barcode;
                        PdfContentByte pdtcb = writer.DirectContent;
                        Barcode128 code128 = new Barcode128();
                        code128.Code = strcode;
                        code128.Extended = true;
                        code128.ChecksumText = true;
                        code128.StartStopText = true;
                        code128.CodeType = iTextSharp.text.pdf.Barcode.CODE128;
                        code128.BarHeight = 30.00F;
                        code128.Size = 10;
                        code128.X = 0.8F;
                        code128.Baseline = 10.0F;
                        code128.TextAlignment = Element.ALIGN_CENTER;
                        iTextSharp.text.Image image128 = code128.CreateImageWithBarcode(pdtcb, null, null);
                        BaseFont bf = BaseFont.CreateFont(BaseFont.TIMES_BOLD, BaseFont.CP1252, false);
                        iTextSharp.text.Font tbf = new iTextSharp.text.Font(bf, 7, iTextSharp.text.Font.NORMAL, iTextSharp.text.BaseColor.BLACK);
                        string text = System.Environment.NewLine + bgm._asset_id + System.Environment.NewLine + bgm._asset_sno + System.Environment.NewLine + bgm._asset_type + System.Environment.NewLine + bgm._loc;
                        PdfPCell cell1 = new PdfPCell(new Phrase(System.Environment.NewLine, tbf));
                        cell1.Border = 0;
                        cell1.Padding = 15;
                        cell1.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell1.VerticalAlignment = Element.ALIGN_MIDDLE;
                        PdfPCell cell2 = new PdfPCell(new Phrase(text, tbf));
                        cell2.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell2.VerticalAlignment = Element.ALIGN_MIDDLE;
                        cell2.Border = 0;
                        cell2.Padding = 10;
                        tbl.AddCell(cell1); tbl.AddCell(cell1); tbl.AddCell(cell1);
                        tbl.AddCell(new Phrase(new Chunk(image128, 75, -30)));
                        tbl.AddCell(cell1); tbl.AddCell(cell1); tbl.AddCell(cell1);
                        tbl.AddCell(cell2); tbl.AddCell(cell1);
                    }
                    if (bgm._barcode_status == "NOT PRINTED")
                    {
                        string[,] col = new string[,]
                            { 
                                {"barcodedetials_bar_status", "PRINTED"},                             
                                {"barcodedetails_update_by",objCmnFunctions.GetLoginUserGid().ToString()},
                                {"barcodedetails_update_date",objCommonIUD.GetCurrentDate()}                        
                            };
                        string[,] Whrcol = new string[,]
                            {                                                              
                                {"barcodedetials_assetdet_gid",barcodeid[i]}                             
                            };
                        string updateComm = objCommonIUD.UpdateCommon(col, Whrcol, "fa_trn_tbarcodedetails");
                    }
                }
                if (tbl.Rows.Count != 0)
                {

                    string barcode = string.Empty;
                    string lpath = ConfigurationManager.AppSettings["Barcodepath"].ToString(); ;
                    string fileName = string.Empty;

                    pdfdoc.Open();
                    pdfdoc.Add(tbl);
                    pdfdoc.Close();
                    writer.Close();
                    barcode = lpath + filename + ".pdf";
                    string Filpath = barcode;
                    HttpContext.Current.Response.ClearHeaders();
                    HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + filename + ".pdf");
                    HttpContext.Current.Response.ContentType = "application/pdf";
                    HttpContext.Current.Response.Clear();

                    HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment");
                    HttpContext.Current.Response.TransmitFile(Filpath);
                    if (HttpContext.Current.Response.IsClientConnected) { HttpContext.Current.Response.End(); }



                    //string fileprintname = filename;
                    //HttpContext.Current.Response.ClearHeaders();
                    //HttpContext.Current.Response.ContentType = "pdf/application";
                    //HttpContext.Current.Response.AddHeader("content-disposition",
                    //"attachment;filename=" + fileprintname + ".PDF");
                    //HttpContext.Current.Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                    //HttpContext.Current.Response.End();
                }
                //   }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return filename;
        }

        public void DownLoad(string FName)
        {
            string path = FName;
            System.IO.FileInfo file = new System.IO.FileInfo(path);
            if (file.Exists)
            {
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + file.Name);
                HttpContext.Current.Response.AddHeader("Content-Length", file.Length.ToString());
                HttpContext.Current.Response.ContentType = "application/pdf"; // download […]
            }
        }

        //public void CopyStream(Stream stream, string destPath)
        //{
        //    using (var fileStream = new FileStream(destPath, FileMode.Create, FileAccess.Write))
        //    {
        //        stream.CopyTo(fileStream);
        //    }
        //}
        //async void client_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        //{

        //    byte[] buffer = new byte[e.Result.Length];
        //    await e.Result.ReadAsync(buffer, 0, buffer.Length);

        //    using (IsolatedStorageFile storageFile = IsolatedStorageFile.GetUserStoreForApplication())
        //    {
        //        using (IsolatedStorageFileStream stream = storageFile.OpenFile(HttpContext.Current.Session["filename"].ToString(), FileMode.Create))
        //        {
        //            await stream.WriteAsync(buffer, 0, buffer.Length);
        //        }
        //    }
        //}

        public List<BarcodeGenerationModel> GetDetailForBarcodePrintingSearch()
        {
            List<BarcodeGenerationModel> objModel = new List<BarcodeGenerationModel>();
            try
            {

                BarcodeGenerationModel obj = new BarcodeGenerationModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_barcodegeneration", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "BARCODEDETAILSearch";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new BarcodeGenerationModel();
                    obj._assetdet_gid = row["assetdetails_gid"].ToString();
                    obj._asset_id = row["assetdetails_assetdet_id"].ToString();
                    obj._asset_sno = row["assetdetails_asset_serialno"].ToString();
                    obj._assetCode = row["asset_code"].ToString();
                    obj._loc = row["branch_code"].ToString();
                    obj._awb_no = row["barcodedetials_awb_no"].ToString();
                    obj._despatch_date = row["barcodedetials_despatch_date"].ToString();
                    obj._courier_name = row["barcodedetials_couri_name"].ToString();
                    obj._barcode_status = row["barcodedetials_bar_status"].ToString();
                    obj._barcode = row["barcodedetials_bar_no"].ToString();
                    obj._source_name = row["assetsource_name"].ToString();
                    obj._asset_type = row["asset_type"].ToString();
                    obj._brname = row["branch_name"].ToString();
                    if (obj._asset_type == "M")
                    {
                        obj._asset_type = "MOVABLE";
                    }
                    else
                    {
                        obj._asset_type = "IMMOVABLE";
                    }
                    objModel.Add(obj);
                }
                return objModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return objModel;
            }
            finally
            {
                con.Close();
            }
        }

        public List<BarcodeGenerationModel> GetDetailForBarcodePrintingSum()
        {
            List<BarcodeGenerationModel> objModel = new List<BarcodeGenerationModel>();
            try
            {

                BarcodeGenerationModel obj = new BarcodeGenerationModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_barcodegeneration", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "BARCODEDETAILSum";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new BarcodeGenerationModel();
                    obj._assetdet_gid = row["assetdetails_gid"].ToString();
                    obj._asset_id = row["assetdetails_assetdet_id"].ToString();
                    obj._asset_sno = row["assetdetails_asset_serialno"].ToString();
                    obj._assetCode = row["asset_code"].ToString();
                    obj._assetdet_gid = row["assetdetails_gid"].ToString();
                    obj._loc_gid = row["assetdetails_branch_gid"].ToString();
                    obj._loc = row["branch_code"].ToString();
                    obj._awb_no = row["barcodedetials_awb_no"].ToString();
                    obj._despatch_date = row["barcodedetials_despatch_date"].ToString();
                    obj._courier_name = row["barcodedetials_couri_name"].ToString();
                    obj._barcode_status = row["barcodedetials_bar_status"].ToString();
                    obj._barcode = row["barcodedetials_bar_no"].ToString();
                    obj._source_name = row["assetsource_name"].ToString();
                    obj._brname = row["branch_name"].ToString();
                    obj._asset_type = row["asset_type"].ToString();
                    if (obj._asset_type == "M")
                    {
                        obj._asset_type = "MOVABLE";
                    }
                    else
                    {
                        obj._asset_type = "IMMOVABLE";
                    }
                    objModel.Add(obj);
                }
                return objModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return objModel;
            }
            finally
            {
                con.Close();
            }
        }

        public string Savedespatch(BarcodeGenerationModel model, string ids)
        {
            try
            {
                string[] values = ids.Split(',');

                foreach (string id in values)
                {
                    if (id != string.Empty)
                    {
                        string[,] col = new string[,]
                    { 
                        {"barcodedetials_awb_no",model._awb_no},
                        {"barcodedetials_couri_name",model._courier_name},
                        {"barcodedetials_despatch_date",objCmnFunctions.convertoDateTimeString(model._despatch_date)},
                        {"barcodedetials_bar_status ","DESPATCHED"}
                    };
                        string[,] Whcol = new string[,]
                    {    
                        {"barcodedetials_assetdet_gid", id}                                                   
                    };
                        string updateFunc = objCommonIUD.UpdateCommon(col, Whcol, "fa_trn_tbarcodedetails");
                    }
                }
                return "success";
            }
            catch (Exception e)
            {
                objErrorLog.WriteErrorLog(e.Message.ToString(), e.ToString());
                return string.Empty;
            }
            finally
            {

            }

        }

        public List<GroupModel> GetDetailToGroup()
        {
            List<GroupModel> objModel = new List<GroupModel>();
            try
            {

                GroupModel obj = new GroupModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_groupingasset", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "GROUPASSETSEARCH";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new GroupModel();
                    obj._assetdet_gid = row["assetdetails_gid"].ToString();
                    obj._asset_id = row["assetdetails_assetdet_id"].ToString();
                    obj._asset_cat = row["assetcategory_name"].ToString();
                    obj._asset_subcat = row["asset_code"].ToString();
                    obj._asset_subcat_name = row["asset_description"].ToString();
                    obj._loc = row["branch_code"].ToString();
                    obj._cap_date = row["assetdetails_cap_date"].ToString();
                    obj._qty = row["assetdetails_qty"].ToString();
                    obj._is5K = row["assetdetails_not_5kcase"].ToString();
                    obj._asset_value = Convert.ToDecimal(row["assetdetails_asset_value"].ToString());
                    objModel.Add(obj);
                }
                return objModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return objModel;
            }
            finally
            {
                con.Close();
            }
        }
        public List<GroupModel> GetDetailToGroupsearch(string location, string capdate)
        {
            List<GroupModel> objModel = new List<GroupModel>();
            try
            {

                GroupModel obj = new GroupModel();
                GetConnection();
                DataTable dt = new DataTable();
                //cmd = new SqlCommand("pr_ifams_trn_groupingasset", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "GROUPASSETSEARCH_loc";
                //cmd.Parameters.Add("@loc", SqlDbType.VarChar).Value = location;
                ////cmd.Parameters.Add("@assetdetails_cap_date", SqlDbType.VarChar).Value = capdate;
                cmd = new SqlCommand("pr_ifams_rpt_SplitReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@locationcode", SqlDbType.VarChar).Value = location;
                cmd.Parameters.Add("@capdate", SqlDbType.VarChar).Value = capdate;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getassetdetailsbylocation";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new GroupModel();
                    obj._assetdet_gid = row["assetdetails_gid"].ToString();
                    obj._asset_id = row["assetdetails_assetdet_id"].ToString();
                    obj._asset_cat = row["assetcategory_name"].ToString();
                    obj._asset_subcat = row["asset_code"].ToString();
                    obj._asset_subcat_name = row["asset_description"].ToString();
                    obj._loc = row["branch_code"].ToString();
                    obj._cap_date = row["assetdetails_cap_date"].ToString();
                    obj._qty = row["assetdetails_qty"].ToString();
                    obj._is5K = row["assetdetails_not_5kcase"].ToString();
                    obj._asset_value = Convert.ToDecimal(row["assetdetails_asset_value"].ToString());
                    objModel.Add(obj);
                }
                return objModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return objModel;
            }
            finally
            {
                con.Close();
            }
        }
        public string GenerateGoupId(string ids, string grpid)
        {
            string success = "success";
            List<GroupModel> objModel = new List<GroupModel>();
            try
            {
                DataTable dt = new DataTable();
                GroupModel obj = new GroupModel();
                string[] values = ids.Split(',');

                for (int i = 0; i < values.Length; i++)
                {
                    string id1 = values[i];
                    if (id1 != string.Empty)
                        for (int length = (values.Length) - 1; length >= 0; length--)
                        {
                            string id2 = values[length];
                            if (id2 != string.Empty)
                                if (success == "success")
                                {
                                    string loc1 = Getlocation(id2);
                                    string loc2 = Getlocation(id1);
                                    if (loc1 == loc2)
                                        success = "success";
                                    else
                                        success = "location";
                                }
                        }
                }
                int value_len = 0;
                if (values.Length == 1) { value_len = 2; }
                else { value_len = values.Length; }
                if (success == "success")
                {
                    string groupid = string.Empty;
                    if (grpid == null)
                    {
                        groupid = GetGroupIDSequence();
                        if (ids != null || ids != string.Empty)
                        {
                            string[,] Whcol = new string[,]
                            {    
                                {"goaheader_groupid",groupid},
                                {"goaheader_asset_count",(value_len-1).ToString()},  
                                {"goaheader_insert_by",objCmnFunctions.GetLoginUserGid().ToString()},
                                {"goaheader_insert_date",objCommonIUD.GetCurrentDate()} ,
                                {"goaheader_maker_id",objCmnFunctions.GetLoginUserGid().ToString()}
                            };
                            string ins = objCommonIUD.InsertCommon(Whcol, "fa_trn_tgoaheader");
                        }
                    }
                    else
                    {
                        groupid = grpid;
                        GetConnection();
                        dt = new DataTable();
                        cmd = new SqlCommand("pr_ifams_trn_groupingasset", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "VIEW";
                        cmd.Parameters.Add("@group", SqlDbType.VarChar).Value = groupid;
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            string LOCMAIN = dt.Rows[0]["branch_code"].ToString();
                            for (int i = 0; i < values.Length; i++)
                            {
                                string id1 = values[i];
                                if (id1 != string.Empty)
                                    if (success == "success")
                                    {
                                        string loc1 = Getlocationcode(Getlocation(id1));
                                        if (loc1 == LOCMAIN)
                                            success = "success";
                                        else
                                            success = "location";
                                    }
                            }
                        }
                        if (success == "success")
                        {
                            dt = new DataTable();
                            cmd = new SqlCommand("pr_ifams_trn_groupingasset", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "COUNT";
                            cmd.Parameters.Add("@group", SqlDbType.VarChar).Value = groupid;
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dt);
                            int value = 0;
                            if (dt.Rows.Count > 0)
                            {
                                value = Convert.ToInt32(dt.Rows[0]["goaheader_asset_count"]);
                                value = value + (value_len - 1);
                            }
                            string[,] colV = new string[,]
                            {                         
                               
                                {"goaheader_asset_count ",(value).ToString()}   
                            };
                            string[,] WHcol = new string[,]
                            {                         
                                {"goaheader_groupid ",groupid.ToString()},
                                 
                            };
                            string UPDATE = objCommonIUD.UpdateCommon(colV, WHcol, "fa_trn_tgoaheader");
                        }
                    }
                    if (success == "success")
                    {
                        obj._group_gid = GetGroupID(groupid);
                        dt = new DataTable();
                        cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = "WAITING FOR APPROVAL";
                        cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "status";
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        foreach (DataRow row in dt.Rows)
                        {
                            obj._asset_status = row["status_flag"].ToString();
                        }
                        for (int i = 0; i < values.Length; i++)
                        {
                            string id = values[i];
                            if (id != string.Empty)
                            {
                                string[,] col = new string[,]
                            {                         
                                {"goadetail_goaheader_gid ",obj._group_gid.ToString()},
                                {"goadetail_assetdet_gid ",id},
                                {"goadetail_insert_by ",objCmnFunctions.GetLoginUserGid().ToString()},
                                {"goadetail_status ", obj._asset_status},
                                {"goadetail_insert_date ",objCommonIUD.GetCurrentDate()}   
                            };
                                string ins = objCommonIUD.InsertCommon(col, "fa_trn_tgoadetail");
                                DataTable dt2 = new DataTable();
                                cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = "GOACHK";
                                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "groupname";
                                da = new SqlDataAdapter(cmd);
                                da.Fill(dt2);
                                string grp = string.Empty;
                                foreach (DataRow row1 in dt2.Rows)
                                    grp = row1["rolegroup_gid"].ToString();

                                string[,] colo = new string[,]
	                        {
                                     {"queue_date",Convert.ToString(objCommonIUD.GetCurrentDate())},
                                     {"queue_ref_flag",GetRef("GOA").ToString()},
                                     {"queue_ref_gid", id},
                                     {"queue_action_for", "A"},
                                     {"queue_from",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                                     {"queue_to_type","G" },
                                     {"queue_to",grp},
                                     {"queue_prev_gid","0"},
                                     {"queue_action_status","0"},
                                     {"queue_isremoved","N"}                                     
	                      };
                                string inst = objCommonIUD.InsertCommon(colo, "iem_trn_tqueue");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return "failed";
            }
            finally
            {
                con.Close();
            }
            return success;
        }

        public string Getlocation(string id)
        {
            GroupModel obj = new GroupModel();
            try
            {

                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_groupingasset", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "LOCATION";
                cmd.Parameters.Add("@group", SqlDbType.VarChar).Value = id;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                    obj._loc = dt.Rows[0]["assetdetails_branch_gid"].ToString();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return "failed";
            }
            finally
            {
                con.Close();
            }
            return obj._loc;

        }

        public string Getlocationcode(string id)
        {
            GroupModel obj = new GroupModel();
            try
            {

                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_groupingasset", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "LOCATIONcode";
                cmd.Parameters.Add("@group", SqlDbType.VarChar).Value = id;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                    obj._loc = dt.Rows[0]["branch_code"].ToString();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return "failed";
            }
            finally
            {
                con.Close();
            }
            return obj._loc;

        }

        public List<GroupModel> GetDetailForMkrSummary(string id)
        {
            List<GroupModel> objModel = new List<GroupModel>();
            try
            {
                GroupModel obj = new GroupModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_groupingasset", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "SUMMARYMKR";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new GroupModel();
                    obj._group_gid = row["goaheader_gid"].ToString();
                    obj._group_id = row["goaheader_groupid"].ToString();
                    obj._asset_count = row["goaheader_asset_count"].ToString();
                    objModel.Add(obj);
                }
                return objModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return objModel;
            }
            finally
            {
                con.Close();
            }
        }

        public List<GroupModel> GetDetailForChkSummary()
        {
            List<GroupModel> objModel = new List<GroupModel>();
            try
            {
                GroupModel obj = new GroupModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_groupingasset", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "SUMMARYCHK";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new GroupModel();
                    obj._group_gid = row["goaheader_gid"].ToString();
                    obj._group_id = row["goaheader_groupid"].ToString();
                    obj._asset_count = row["goaheader_asset_count"].ToString();
                    objModel.Add(obj);
                }
                return objModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return objModel;
            }
            finally
            {
                con.Close();
            }
        }

        public List<GroupModel> GetDetailForView(string groupid)
        {
            List<GroupModel> objModel = new List<GroupModel>();
            try
            {
                GroupModel obj = new GroupModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_groupingasset", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "VIEW";
                cmd.Parameters.Add("@group", SqlDbType.VarChar).Value = groupid;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new GroupModel();
                    DataTable dt1 = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = row["goadetail_status"].ToString();
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "statusname";
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt1);
                    foreach (DataRow row1 in dt1.Rows)
                        obj._asset_status = row1["status_name"].ToString();
                    obj._group_id = row["goadetail_goaheader_gid"].ToString();
                    obj._asset_id = row["assetdetails_assetdet_id"].ToString();
                    obj._asset_cat = row["assetcategory_name"].ToString();
                    obj._asset_subcat = row["asset_code"].ToString();
                    obj._asset_subcat_name = row["asset_description"].ToString();
                    obj._loc = row["branch_code"].ToString();
                    obj._cap_date = row["assetdetails_cap_date"].ToString();
                    obj._qty = row["assetdetails_qty"].ToString();
                    obj._is5K = row["assetdetails_not_5kcase"].ToString();
                    obj._asset_value = Convert.ToDecimal(row["assetdetails_asset_value"].ToString());
                    objModel.Add(obj);
                }
                return objModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return objModel;
            }
            finally
            {
                con.Close();
            }
        }

        public string GetGroupGid(string assetid)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_exceldataverification", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Result", SqlDbType.VarChar).Value = "GroupID";
                cmd.Parameters.Add("@chkdata", SqlDbType.VarChar).Value = assetid;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                    assetid = dt.Rows[0]["assetdetails_physical_assetid"].ToString();
                else
                    assetid = string.Empty;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return assetid;
        }

        public string GetGroupID(string groupid)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_groupingasset", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "getgroupgid";
                cmd.Parameters.Add("@group", SqlDbType.VarChar).Value = groupid;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                    groupid = dt.Rows[0]["goaheader_gid"].ToString();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return groupid;
        }

        public string ApproveRejectGrp(string no, string status, string remarks)
        {
            string id = GetGroupID(no);
            DataTable dt1 = new DataTable();
            string status_flag = string.Empty, GRPID = string.Empty;

            try
            {
                cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = status.Trim();
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "status";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt1);
                foreach (DataRow row in dt1.Rows)
                {
                    status_flag = row["status_flag"].ToString();
                }
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_groupingasset", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "details";
                cmd.Parameters.Add("@group", SqlDbType.VarChar).Value = id;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    string is5k = string.Empty;
                    decimal assetids = 0;
                    for (int i = 1; i < dt.Rows.Count; i++)
                    {
                        cmd = new SqlCommand("pr_ifams_trn_groupingasset", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "getallassetvalues";
                        cmd.Parameters.Add("@group", SqlDbType.VarChar).Value = dt.Rows[i]["goadetail_assetdet_gid"].ToString();

                        decimal sumassetvalue = Convert.ToDecimal(cmd.ExecuteScalar());
                        assetids = assetids + sumassetvalue;

                    }

                    #region/*5K Validation Remove for Client Request 31-10-2018 */
                    /* if (assetids > 5000)
                    {
                        is5k = "Y";
                    }
                    else
                    {
                        is5k = "N";
                    }*/
                    #endregion
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (status == "APPROVED")
                        {



                            DataTable dATAtABL = new DataTable();
                            cmd = new SqlCommand("pr_ifams_trn_groupingasset", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "GroupIDOF1STASSET";
                            cmd.Parameters.Add("@group", SqlDbType.VarChar).Value = id;
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dATAtABL);
                            if (dATAtABL.Rows.Count > 0)
                            { GRPID = dATAtABL.Rows[i]["assetdetails_asset_groupid"].ToString(); }
                            string[,] col = new string[,]
                            {                         
                                {"assetdetails_physical_assetid ",id},                                
                                {"assetdetails_update_by ",objCmnFunctions.GetLoginUserGid().ToString()},                                
                                {"assetdetails_update_date ",objCommonIUD.GetCurrentDate()}  ,
                                {"assetdetails_qty ","0"} ,
                                //{"assetdetails_not_5kcase",is5k}
                                {"assetdetails_not_5kcase","Y"}
                            };
                            string[,] Whrcol = new string[,]
                            {                               
                                {"assetdetails_gid",dt.Rows[i]["goadetail_assetdet_gid"].ToString()}   
                            };
                            string Update = objCommonIUD.UpdateCommon(col, Whrcol, "fa_trn_tassetdetails");
                            string[,] col1 = new string[,]
                            {                         
                                                            
                                {"assetdetails_qty ","1"}   
                            };
                            string[,] Whrcol1 = new string[,]
                            {                               
                                {"assetdetails_gid",dt.Rows[0]["goadetail_assetdet_gid"].ToString()}   
                            };
                            string Update11 = objCommonIUD.UpdateCommon(col1, Whrcol1, "fa_trn_tassetdetails");
                            string[,] colm = new string[,]
                                {
                                         {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                                         {"queue_action_status","1"},
                                         {"queue_action_date",objCommonIUD.GetCurrentDate()},
                                         {"queue_isremoved","Y"},
                                         {"queue_action_remark",remarks},    
                                };
                            string[,] whrecol = new string[,]
                                {
                                          {"queue_ref_gid", dt.Rows[i]["goadetail_assetdet_gid"].ToString().Trim()} ,    
                                          {"queue_ref_flag", GetRef("GOA").ToString()}                                           
                                };
                            string Update1 = objCommonIUD.UpdateCommon(colm, whrecol, "iem_trn_tqueue");
                        }
                        else
                        {
                            string[,] colm = new string[,]
                                {
                                         {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                                         {"queue_action_status","2"},
                                         {"queue_action_date",objCommonIUD.GetCurrentDate()},
                                         {"queue_isremoved","Y"},
                                           {"queue_action_remark",remarks}     
                                };
                            string[,] whrecol = new string[,]
                                {
                                          {"queue_ref_gid", dt.Rows[i]["goadetail_assetdet_gid"].ToString().Trim()} ,    
                                          {"queue_ref_flag", GetRef("GOA").ToString()}                                           
                                };
                            string Update1 = objCommonIUD.UpdateCommon(colm, whrecol, "iem_trn_tqueue");
                        }
                        string[,] col11 = new string[,]
                            {                         
                                {"goadetail_status",status_flag},                                
                                {"goadetail_update_by",objCmnFunctions.GetLoginUserGid().ToString()},                                
                                {"goadetail_update_date",objCommonIUD.GetCurrentDate()}   
                            };
                        string[,] Whrcol11 = new string[,]
                            {                               
                                {"goadetail_goaheader_gid",id.ToString()}  ,
                                {"goadetail_assetdet_gid",dt.Rows[i]["goadetail_assetdet_gid"].ToString()}
                            };
                        string Update2 = objCommonIUD.UpdateCommon(col11, Whrcol11, "fa_trn_tgoadetail");
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "Failed";
            }
            finally
            {
                con.Close();
            }
            return "Success";
        }

        public List<GroupModel> PopulateAuditTrail(GroupModel pat)
        {
            List<GroupModel> objAuditTrail = new List<GroupModel>();
            try
            {
                GroupModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                string fstEmp = string.Empty;
                string status = string.Empty;
                cmd = new SqlCommand("pr_ifams_trn_audittaril", con);
                cmd.Parameters.AddWithValue("@gid", pat.gid);
                cmd.Parameters.AddWithValue("@refflag", "GOA");
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string pergid = Convert.ToString(dt.Rows[i]["queue_prev_gid"].ToString());
                        if (pergid == "0")
                        {
                            fstEmp = Convert.ToString(dt.Rows[i]["queue_from"].ToString());
                            string empcodnamer = Getempname(fstEmp);
                            string[] datar;
                            datar = empcodnamer.Split(',');
                            objModel = new GroupModel();
                            objModel._employee_code = datar[0].ToString() + " - " + datar[1].ToString();
                            objModel._action_status = "Submitted";
                            objModel._action_date = Convert.ToString(dt.Rows[i]["queue_date"].ToString());
                            objModel._queue_to_type = "GOA Maker";
                            objModel._action_remarks = string.Empty;
                            objModel._asset_id = Convert.ToString(dt.Rows[i]["assetdetails_assetdet_id"].ToString());
                            objAuditTrail.Add(objModel);
                            string actions = Convert.ToString(dt.Rows[i]["queue_action_by"].ToString());
                            if (actions == string.Empty)
                            {
                                string queue_type = Convert.ToString(dt.Rows[i]["queue_to_type"].ToString());
                                string queue_to = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
                                string actionstt = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
                                if (actionstt != string.Empty)
                                {
                                    objModel = new GroupModel();
                                    objModel._employee_code = string.Empty;
                                    objModel._action_date = Convert.ToString(dt.Rows[i]["queue_action_date"].ToString());
                                    status = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
                                    objModel._action_status = GetQueueStatusHistry(status);
                                    objModel._queue_to_type = GetQueueRole(queue_type, queue_to);
                                    objModel._asset_id = Convert.ToString(dt.Rows[i]["assetdetails_assetdet_id"].ToString());
                                    objModel._action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());
                                    objAuditTrail.Add(objModel);
                                }
                            }
                            else
                            {
                                string empid = string.Empty;
                                string queue_type = Convert.ToString(dt.Rows[i]["queue_to_type"].ToString());
                                string queue_to = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
                                string actionstt = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
                                if (actionstt != string.Empty)
                                {
                                    objModel = new GroupModel();
                                    //if (queue_type == "I" || queue_type == "E")
                                    //{
                                    //    empid = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
                                    //}
                                    // else 
                                    if (queue_type == "G" || queue_type == "R" || queue_type == "L" || queue_type == "D")
                                    {
                                        empid = Convert.ToString(dt.Rows[i]["queue_action_by"].ToString());
                                    }
                                    if (empid != string.Empty)
                                    {
                                        string empcodname = Getempname(empid);
                                        string[] data;
                                        data = empcodname.Split(',');
                                        objModel._employee_code = data[0].ToString() + " - " + data[1].ToString();
                                    }
                                    objModel._action_date = Convert.ToString(dt.Rows[i]["queue_action_date"].ToString());
                                    status = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
                                    objModel._action_status = GetQueueStatusHistry(status);
                                    objModel._queue_to_type = GetQueueRole(queue_type, queue_to);
                                    objModel._action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());
                                    objModel._asset_id = Convert.ToString(dt.Rows[i]["assetdetails_assetdet_id"].ToString());
                                    objAuditTrail.Add(objModel);
                                }
                            }
                        }
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
            return objAuditTrail;
        }

        public string UpdateDepRateBulk(DataTable datatable, string filename)
        {
            try
            {
                List<ChangeDepreciationRate> objList = new List<ChangeDepreciationRate>();
                ChangeDepreciationRate objModel;
                DataTable err1 = new DataTable();
                err1 = (DataTable)datatable;
                for (int i = 0; i < err1.Rows.Count; i++)
                {
                    objModel = new ChangeDepreciationRate();
                    objModel._asset_id = err1.Rows[i]["Asset Id"].ToString().Trim();
                    objModel._dep_value = err1.Rows[i]["Depreciation Rate"].ToString().Trim();
                    string[,] codes = new string[,]
	                {
                            {"assetdetails_dep_rate", objModel._dep_value.ToString()},
                            {"assetdetails_update_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                            {"assetdetails_update_date",Convert.ToString(objCommonIUD.GetCurrentDate())}
                    };
                    string[,] whrcol = new string[,]
	                {
                            {"assetdetails_assetdet_id", objModel._asset_id .ToString()} ,  
                            {"assetdetails_isremoved", "N"}                            
                    };
                    string updcomm1 = objCommonIUD.UpdateCommon(codes, whrcol, "fa_trn_tassetdetails");
                    //history of updation
                    DataTable dt = GetDetails(objModel._asset_id.ToString());
                    objModel._gid = Convert.ToInt32(dt.Rows[0]["assetdetails_gid"]);
                    string[,] colval = new string[,]
                       {
                            {"changedeprate_assetdet_gid", objModel._gid.ToString()},
                            {"changedeprate_new_deprate", objModel._dep_value.ToString()},
                            {"changedeprate_upld_flag","Y"} ,
                            {"changedeprate_upld_fname",filename} ,
                            {"changedeprate_upld_date",convertoDate (DateTime.Today.ToShortDateString())},
                            {"changedepreate_upld_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                            {"changedeprate_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                            {"changedeprate_insert_date",convertoDate (DateTime.Today.ToShortDateString())},
                            {"changedeprate_isremoved","N"},
                        };
                    string insertcomm = objCommonIUD.InsertCommon(colval, "fa_trn_tchangedeprate");
                }
                return "Success";
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "Failed";
            }
            finally
            {
                con.Close();
            }

        }

        public string UpdateDepRateManul(ChangeDepreciationRate data)
        {
            try
            {
                string[,] codes = new string[,]
	                {
                             {"assetdetails_dep_rate", data._dep_value.ToString()},
                             {"assetdetails_update_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                             {"assetdetails_update_date",Convert.ToString(objCommonIUD.GetCurrentDate())}
	                };
                string[,] whrcol = new string[,]
	                {
                           {"assetdetails_assetdet_id", data._asset_id .ToString()} ,  
                           {"assetdetails_isremoved", "N"} ,
                    };
                string updcomm = objCommonIUD.UpdateCommon(codes, whrcol, "fa_trn_tassetdetails");
                DataTable dt = GetDetails(data._asset_id.ToString());
                data._gid = Convert.ToInt32(dt.Rows[0]["assetdetails_gid"]);
                string[,] colval = new string[,]
               {
                        {"changedeprate_assetdet_gid",data._gid.ToString()},
                        {"changedeprate_new_deprate", data._dep_value.ToString()},
                        {"changedeprate_upld_flag","N"} ,
                        {"changedeprate_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                        {"changedeprate_insert_date",convertoDate (DateTime.Today.ToShortDateString())},
                        {"changedeprate_isremoved","N"},
                };
                string insertcomm = objCommonIUD.InsertCommon(colval, "fa_trn_tchangedeprate");
                return "Success";
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "Failed";
            }
            finally
            {
                con.Close();
            }

        }

        public IEnumerable<ChangeDepreciationRate> GetAssetDetailHelp()
        {
            List<ChangeDepreciationRate> objMod = new List<ChangeDepreciationRate>();
            ChangeDepreciationRate obj = new ChangeDepreciationRate();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "Search");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new ChangeDepreciationRate();
                    obj._gid = Convert.ToInt32(row["assetdetails_gid"].ToString());
                    obj._asset_id = row["assetdetails_assetdet_id"].ToString();
                    obj._dep_value = row["assetdetails_dep_rate"].ToString();
                    objMod.Add(obj);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return objMod;
            }
            finally
            {
                con.Close();
            }
            return objMod;
        }

        public string Format(string date)
        {
            try
            {
                GetConnection();
                string date1 = objCmnFunctions.convertoDateTimeString(date);
                string[] array = date1.Split('-');
                date = array[1] + "-" + array[2];
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return date;
        }

        public Boolean DataSynchronise()
        {
            Boolean data = true;
            Depreciation dep = new Depreciation();
            try
            {
                if (!StartProcess("ASSET_SYNC"))
                {
                    data = false;
                }
                else
                {
                    GetConnection();
                    DataTable dt = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_depreciation", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@querytype", "ASSET_SYNC");
                    cmd.CommandTimeout = 1000;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        //for (int i = 0; i < dt.Rows.Count; i++)
                        //{
                        //    dep._branch = dt.Rows[i][string.Empty].ToString().Split('|');
                        //    dep._ratio = dt.Rows[i][string.Empty].ToString().Split('|');
                        //    dep._qty = Convert.ToInt32(dt.Rows[i][string.Empty]);
                        //    dep._bs = dt.Rows[i][string.Empty].ToString().Split('|');
                        //    dep._cc = dt.Rows[i][string.Empty].ToString().Split('|');
                        //    if (dep._qty == 0)
                        //        dep._qty = 1;
                        //    dep._asset_value = Convert.ToDecimal(dt.Rows[i][string.Empty]) / Convert.ToDecimal(dep._qty);
                        //    for (int j = 1; j < Convert.ToInt32(dep._qty); j++)
                        //    {

                        //    }
                        //    for (int temp = 1; temp < dep._branch.GetUpperBound(1); temp++)
                        //    {
                        //        string[,] colval = new string[,]
                        //                  {
                        //                  {string.Empty, dep._branch[temp]},
                        //                  {string.Empty, dep._ratio[temp]},
                        //                  {string.Empty, dep._asset_id},
                        //                  {string.Empty, dep._bs[temp]},
                        //                  {string.Empty,dep._cc[temp] },
                        //                   };
                        //        string insertcomm = objCommonIUD.InsertCommon(colval, "fa_trn_tassetlocation");
                        //    }
                        //}
                    }
                }
                EndProcess("ASSET_SYNC");
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return data;

        }

        public Boolean StartProcess(string type)
        {
            Boolean data = true;
            Depreciation dep = new Depreciation();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_depreciation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@querytype", "PROCESS_START");
                cmd.Parameters.AddWithValue("@process_name", "DEPR_RUN");
                cmd.CommandTimeout = 1000;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    string a = dt.Rows[0]["process_status"].ToString();
                    if (a == "S")
                    {
                        string[,] colval = new string[,]
                                          {                                          
                                          {"process_status", "R"},
                                          {"process_update_by", Convert.ToString(objCommonIUD.GetCurrentDate())} ,   
                                          {"process_update_date", Convert.ToString(objCommonIUD.GetCurrentDate())} , 
                                           };
                        string[,] Whrcolval = new string[,]
                                          {                                         
                                          {"process_name",type },
                                           };
                        string update = objCommonIUD.UpdateCommon(colval, Whrcolval, "fa_trn_tprocess");
                    }
                    else
                    {
                        data = false;
                    }
                }
                else
                {
                    data = false;
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
            return data;
        }

        public void EndProcess(string type)
        {
            try
            {
                GetConnection();
                string[,] colval = new string[,]
                                         {                                          
                                          {"process_status", "S"},
                                          {"process_update_by", Convert.ToString(objCommonIUD.GetCurrentDate())} ,   
                                          {"process_update_date", Convert.ToString(objCommonIUD.GetCurrentDate())} , 
                                          };
                string[,] Whrcolval = new string[,]
                                          {                                         
                                          {"process_name",type },
                                           };
                string update = objCommonIUD.UpdateCommon(colval, Whrcolval, "fa_trn_tprocess");
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }

        public Boolean DepreciationDone(string date, string runtype)
        {
            Boolean data = true;
            try
            {
                if (runtype == "Final")
                {
                    string date1 = objCmnFunctions.convertoDateTimeString(date);
                    GetConnection();
                    DataTable dt = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_depreciation", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@querytype", "DEP_DONE1");
                    cmd.Parameters.AddWithValue("@process_name", date1);
                    cmd.CommandTimeout = 0;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    int a = Convert.ToInt32(dt.Rows[0]["depreciation_assetdet_gid"].ToString());
                    if (a > 0)
                    {
                        return true;
                    }
                    else
                    {
                        data = false;
                    }
                    //dt = new DataTable();
                    //cmd = new SqlCommand("pr_ifams_trn_depreciation", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@querytype", "DEP_DONE2");
                    //cmd.Parameters.AddWithValue("@process_name", objCmnFunctions.convertoDateTimeString(date));
                    //da = new SqlDataAdapter(cmd);
                    //da.Fill(dt);
                    //int a1 = Convert.ToInt32(dt.Rows[0]["depreciation_assetdet_gid"].ToString());
                    //if (a1 > 0)
                    //{
                    //    //--------------------------- MESSAGE BOX ----------------------------------//
                    //    //------------------------ok dep done false ----------------------------------//
                    //    //------------------------cancel dep done true----------------------------------//
                    //    return true;
                    //}
                    //else
                    //{
                    //    data = false;
                    //}
                }
                if (runtype == "Pre")
                {
                    string date1 = objCmnFunctions.convertoDateTimeString(date);
                    GetConnection();
                    DataTable dt = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_depreciation", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@querytype", "PREDEP_DONE1");
                    cmd.Parameters.AddWithValue("@process_name", date1);
                    cmd.CommandTimeout = 1000;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    int a = Convert.ToInt32(dt.Rows[0]["depreciation_assetdet_gid"].ToString());
                    if (a > 0)
                    {
                        return true;
                    }
                    else
                    {
                        data = false;
                    }
                    //dt = new DataTable();
                    //cmd = new SqlCommand("pr_ifams_trn_depreciation", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@querytype", "DEP_DONE2");
                    //cmd.Parameters.AddWithValue("@process_name", objCmnFunctions.convertoDateTimeString(date));
                    //da = new SqlDataAdapter(cmd);
                    //da.Fill(dt);
                    //int a1 = Convert.ToInt32(dt.Rows[0]["depreciation_assetdet_gid"].ToString());
                    //if (a1 > 0)
                    //{
                    //    //--------------------------- MESSAGE BOX ----------------------------------//
                    //    //------------------------ok dep done false ----------------------------------//
                    //    //------------------------cancel dep done true----------------------------------//
                    //    return true;
                    //}
                    //else
                    //{
                    //    data = false;
                    //}
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
            return data;
        }

        public List<Depreciation> SelectDepDetails()
        {
            Depreciation objDep = new Depreciation();
            List<Depreciation> deplist = new List<Depreciation>();
            try
            {

                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_depreciation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@querytype", "DEP_RUN");
                cmd.CommandTimeout = 1000;
                da = new SqlDataAdapter(cmd);

                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        objDep = new Depreciation();
                        objDep._gid = Convert.ToInt32(row["assetdetails_gid"].ToString());
                        objDep._asset_id = row["assetdetails_assetdet_id"].ToString();
                        objDep._group_id = row["assetdetails_asset_groupid"].ToString();
                        objDep._asset_code = row["assetdetails_asset_code"].ToString();
                        objDep._asset_value = Convert.ToDecimal(string.IsNullOrEmpty(row["asset_value"].ToString()) ? "0" : row["asset_value"]);
                        objDep._Trf_value = Convert.ToDecimal(string.IsNullOrEmpty(row["transfer_value"].ToString()) ? "0" : row["transfer_value"]);
                        objDep._cap_date = row["assetdetails_cap_date"].ToString();
                        objDep._branchdate_start = row["branch_start_date"].ToString();
                        objDep._branch_leasedate_start = row["branch_lease_start_date"].ToString();
                        objDep._branch_leasedate_end = row["branch_lease_end_date"].ToString();
                        objDep._dep_type = row["asset_dep_type"].ToString();
                        objDep._dep_rate = Convert.ToDecimal(string.IsNullOrEmpty(row["asset_dep_rate"].ToString()) ? "0" : row["asset_dep_rate"]);
                        objDep._assetdet_dep_rate = Convert.ToDecimal(string.IsNullOrEmpty(row["assetdetails_dep_rate"].ToString()) ? "0" : row["assetdetails_dep_rate"]);
                        objDep._dep_full = row["can_depfully"].ToString();
                        objDep._po_number = row["assetdetails_po_number"].ToString();
                        //objDep._loc = row["location"].ToString();
                        objDep._Trf_date = row["assetdetails_tfr_date"].ToString();
                        objDep._Trf_id = row["assetdetails_tfr_id"].ToString();
                        objDep._Sale_status = row["assetdetails_sale_status"].ToString();
                        objDep._Sale_date = row["assetdetails_sale_date"].ToString();
                        objDep._Trf_status = row["assetdetails_tfr_status"].ToString();
                        objDep._group_gid = row["asset_assetgroup_gid"].ToString();
                        objDep._recon_ref = row["assetdetails_recon_reference"].ToString();
                        objDep._is_5k = row["assetdetails_not_5kcase"].ToString();
                        objDep._leasedate_start = row["assetdetails_lease_startdate"].ToString();
                        objDep._leasedate_end = row["assetdetails_lease_enddate"].ToString();
                        objDep._ast_dep_rate = Convert.ToDecimal(row["assetdetails_dep_rate"].ToString());
                        objDep._asset_code = row["assetcategory_name"].ToString();
                        objDep._ast_dep_rate = Convert.ToDecimal(row["assetdetails_dep_rate"].ToString());
                        objDep._AssetDesp = row["asset_description"].ToString();
                        objDep._asset_glcode = row["asset_glcode"].ToString();
                        objDep._dep_amt = Convert.ToDecimal(string.IsNullOrEmpty(row["depreciation_amount"].ToString()) ? "0" : row["depreciation_amount"]);
                        deplist.Add(objDep);
                    }
                }
                objDep._Dep_list = deplist;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return deplist;
        }

        public List<Depreciation> SelectPreRunDetails()
        {
            Depreciation objDep = new Depreciation();
            List<Depreciation> deplist = new List<Depreciation>();
            try
            {

                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_depreciation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@querytype", "PRE_DEP_RUN");
                cmd.CommandTimeout = 1000;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        objDep = new Depreciation();
                        objDep._gid = Convert.ToInt32(row["assetdetails_gid"].ToString());
                        objDep._asset_id = row["assetdetails_assetdet_id"].ToString();
                        objDep._group_id = row["assetdetails_asset_groupid"].ToString();
                        objDep._asset_code = row["assetdetails_asset_code"].ToString();
                        objDep._asset_value = Convert.ToDecimal(string.IsNullOrEmpty(row["asset_value"].ToString()) ? "0" : row["asset_value"]);
                        objDep._Trf_value = Convert.ToDecimal(string.IsNullOrEmpty(row["transfer_value"].ToString()) ? "0" : row["transfer_value"]);
                        objDep._cap_date = row["assetdetails_cap_date"].ToString();
                        objDep._branchdate_start = row["branch_start_date"].ToString();
                        objDep._branch_leasedate_start = row["branch_lease_start_date"].ToString();
                        objDep._branch_leasedate_end = row["branch_lease_end_date"].ToString();
                        objDep._dep_type = row["asset_dep_type"].ToString();
                        objDep._dep_rate = Convert.ToDecimal(string.IsNullOrEmpty(row["asset_dep_rate"].ToString()) ? "0" : row["asset_dep_rate"]);
                        objDep._assetdet_dep_rate = Convert.ToDecimal(string.IsNullOrEmpty(row["assetdetails_dep_rate"].ToString()) ? "0" : row["assetdetails_dep_rate"]);
                        objDep._dep_full = row["can_depfully"].ToString();
                        objDep._po_number = row["assetdetails_po_number"].ToString();
                        objDep._asset_status = row["excelvalidate_name"].ToString();
                        objDep._Trf_date = row["assetdetails_tfr_date"].ToString();
                        objDep._Trf_id = row["assetdetails_tfr_id"].ToString();
                        objDep._Sale_status = row["assetdetails_sale_status"].ToString();
                        objDep._Sale_date = row["assetdetails_sale_date"].ToString();
                        objDep._Trf_status = row["assetdetails_tfr_status"].ToString();
                        objDep._group_gid = row["asset_assetgroup_gid"].ToString();
                        objDep._recon_ref = row["assetdetails_recon_reference"].ToString();
                        objDep._is_5k = row["assetdetails_not_5kcase"].ToString();
                        objDep._leasedate_start = row["assetdetails_lease_startdate"].ToString();
                        objDep._leasedate_end = row["assetdetails_lease_enddate"].ToString();
                        objDep._ast_dep_rate = Convert.ToDecimal(row["assetdetails_dep_rate"].ToString());
                        objDep._asset_code = row["assetcategory_name"].ToString();
                        objDep._ast_dep_rate = Convert.ToDecimal(row["assetdetails_dep_rate"].ToString());
                        objDep._AssetDesp = row["asset_description"].ToString();
                        objDep._asset_glcode = row["asset_glcode"].ToString();
                        objDep._Imp_date = row["assetdetails_imp_date"].ToString();
                        objDep._Wrt_date = row["assetdetails_wrt_date"].ToString();
                        objDep._Split_date = row["assetsplitmerge_update_date"].ToString();
                        objDep._dep_amt = Convert.ToDecimal(string.IsNullOrEmpty(row["depreciation_amount"].ToString()) ? "0" : row["depreciation_amount"]);
                        deplist.Add(objDep);
                    }
                }
                objDep._Dep_list = deplist;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return deplist;
        }

        public DataTable SelectreportDetails(string date)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();

                cmd = new SqlCommand("pr_fa_far_prerpt", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@period_to", date);
                cmd.CommandTimeout = 1000;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                dt = new DataTable();
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return dt;
        }

        public DataTable GetSelectreportDetails()
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_depreciation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@querytype", "reportnew");
                cmd.CommandTimeout = 1000;
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string CurrentMonth = ds.Tables[0].Rows[0]["CurrentMonth"].ToString();
                    string lastMonth = ds.Tables[0].Rows[0]["lastMonth"].ToString();

                    ds.Tables[1].Columns[10].ColumnName = "Cumulative Depreciation for " + lastMonth;
                    ds.Tables[1].Columns[11].ColumnName = "WDV for " + lastMonth;

                    ds.Tables[1].Columns[19].ColumnName = "Depreciation For " + CurrentMonth;
                    ds.Tables[1].Columns[20].ColumnName = "Cumulative Depreciation as of " + CurrentMonth;
                    ds.Tables[1].Columns[21].ColumnName = "WDV as of " + CurrentMonth;
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
            return ds.Tables[1];
        }
        public List<Depreciation> SelectreportDetails(string report_type, string date)
        {
            Depreciation objDep = new Depreciation();
            List<Depreciation> deplist = new List<Depreciation>();
            try
            {

                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_depreciation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@querytype", report_type);
                cmd.Parameters.AddWithValue("@process_name", date);
                cmd.CommandTimeout = 1000;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        objDep = new Depreciation();
                        objDep._gid = Convert.ToInt32(row["assetdetails_gid"].ToString());
                        objDep._asset_id = row["assetdetails_assetdet_id"].ToString();
                        objDep._group_id = row["assetdetails_asset_groupid"].ToString();
                        objDep._asset_value = Convert.ToDecimal(string.IsNullOrEmpty(row["asset_value"].ToString()) ? "0" : row["asset_value"]);
                        objDep._Trf_value = Convert.ToDecimal(string.IsNullOrEmpty(row["transfer_value"].ToString()) ? "0" : row["transfer_value"]);
                        objDep._cap_date = row["assetdetails_cap_date"].ToString();
                        objDep._dep_type = row["asset_dep_type"].ToString();
                        objDep._dep_rate = Convert.ToDecimal(string.IsNullOrEmpty(row["asset_dep_rate"].ToString()) ? "0" : row["asset_dep_rate"]);
                        objDep._asset_code = row["assetcategory_name"].ToString();
                        objDep._AssetDesp = row["asset_description"].ToString();
                        objDep._Sale_status = row["excelvalidate_name"].ToString();
                        objDep._asset_glcode = row["asset_glcode"].ToString();
                        objDep._dep_amt = Convert.ToDecimal(string.IsNullOrEmpty(row["depreciation_amount"].ToString()) ? "0" : row["depreciation_amount"]);
                        objDep._dep_amtpre = Convert.ToDecimal(string.IsNullOrEmpty(row["depreciationpre_amount"].ToString()) ? "0" : row["depreciationpre_amount"]);
                        objDep._is_5k = string.IsNullOrEmpty(row["assetdetails_not_5kcase"].ToString()) ? "N" : row["assetdetails_not_5kcase"].ToString();
                        objDep._assetdet_dep_rate = Convert.ToDecimal(string.IsNullOrEmpty(row["assetdetails_dep_rate"].ToString()) ? "0" : row["assetdetails_dep_rate"]);
                        objDep._leasedate_end = row["assetdetails_lease_enddate"].ToString();
                        objDep._branchdate_start = "01-01-2015".ToString();
                        objDep._leasedate_start = row["assetdetails_lease_startdate"].ToString();
                        objDep._branch = string.Empty.ToString();
                        objDep._ratio = string.Empty.ToString();
                        objDep._Trf_date = row["assetdetails_tfr_date"].ToString();
                        objDep._Wrt_date = row["assetdetails_wrt_date"].ToString();
                        objDep._Sale_date = row["assetdetails_sale_date"].ToString();
                        objDep._Imp_date = row["assetdetails_imp_date"].ToString();
                        objDep._Split_date = row["assetsplitmerge_update_date"].ToString();
                        deplist.Add(objDep);
                    }
                }
                objDep._Dep_list = deplist;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return deplist;
        }

        public void InsertDep(string _asset_id, string _group_id, decimal CurrentDepAmt, string depdate, string deptype)
        {
            try
            {
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@querytype", "assetdetails");
                cmd.Parameters.AddWithValue("@assetid", _asset_id);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                    _asset_id = dt.Rows[0]["assetdetails_gid"].ToString();
                string[,] colval = new string[,]
                                          {                                         
                                            {"depreciation_assetdet_gid",_asset_id },
                                            {"depreciation_asset_groupid",_group_id },
                                            {"depreciation_amount",CurrentDepAmt.ToString() },
                                            {"depreciation_month",depdate },
                                            {"depreciation_asset_owner","F" },
                                            {"depreciation_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                                            {"depreciation_insert_date",Convert.ToString(objCommonIUD.GetCurrentDate())} , 
                                            {"depreciation_type","M"} ,
                                           };
                string insert = objCommonIUD.InsertCommon(colval, "fa_trn_tdepreciation");

                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "UploadDetails";
                cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = _asset_id.ToString();
                da = new SqlDataAdapter(cmd);
                string glCode = string.Empty;
                string resdepglCode = string.Empty;
                string depglCode = string.Empty;
                string branch = string.Empty;
                string cat = string.Empty;
                string subcat = string.Empty;
                string asset_id = string.Empty;
                string BS = string.Empty;
                string CC = string.Empty;
                string ratio = string.Empty;
                DataTable UPLOADDATATBL = new DataTable();
                da.Fill(UPLOADDATATBL);
                if (UPLOADDATATBL.Rows.Count > 0)
                    foreach (DataRow rowdep in UPLOADDATATBL.Rows)
                    {
                        glCode = UPLOADDATATBL.Rows[0]["asset_glcode"].ToString();
                        resdepglCode = UPLOADDATATBL.Rows[0]["asset_dep_reservglcode"].ToString();
                        depglCode = UPLOADDATATBL.Rows[0]["asset_dep_glcode"].ToString();
                        branch = UPLOADDATATBL.Rows[0]["branch_code"].ToString();
                        cat = UPLOADDATATBL.Rows[0]["assetcategory_name"].ToString();
                        if (cat.Length > 16)
                            cat = cat.Substring(0, 16);
                        subcat = UPLOADDATATBL.Rows[0]["asset_code"].ToString();
                        asset_id = UPLOADDATATBL.Rows[0]["assetdetails_assetdet_id"].ToString();
                    }

                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "BSCC_upload_details";
                cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = subcat;
                cmd.Parameters.Add("@fccc", SqlDbType.VarChar).Value = branch;
                da = new SqlDataAdapter(cmd);
                UPLOADDATATBL = new DataTable();
                da.Fill(UPLOADDATATBL);
                if (UPLOADDATATBL.Rows.Count > 0)
                    foreach (DataRow rowdep in UPLOADDATATBL.Rows)
                    {
                        BS = UPLOADDATATBL.Rows[0]["depreciationbscc_bs"].ToString();
                        CC = UPLOADDATATBL.Rows[0]["depreciationbscc_cc_oracle"].ToString();
                    }


                if (CurrentDepAmt != 0 || CurrentDepAmt < 0)
                {
                    string[,] glCoulmsDepD = new string[,]
                            {                             
                                {"tran_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_val_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_payment_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_gl_no",depglCode},
                                {"tran_desc","DEP FOR THE MONTH OF " + Format(depdate) + " - FOR THE ASSET :  " + asset_id.ToString()},
                                {"tran_amount",CurrentDepAmt.ToString()},
                                {"tran_acc_mode","D".ToString()},
                                {"tran_mult","-1"},  
                                {"tran_fc_code",BS},
                                {"tran_cc_code",CC}, 
                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                {"tran_ou_code",branch},
                                {"tran_ref_flag",GetRef("DEP")},
                                {"tran_ref_gid",_asset_id.ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                                {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                {"tran_expense_category","10"},
                                {"tran_primary_branch_code",branch.ToString()},
                                {"tran_secondary_branch_code",string.Empty},
                                {"tran_related_account",string.Empty},
                                {"tran_invoice_no",string.Empty},
                                {"tran_vendor_code",string.Empty},
                                {"tran_vendor_name",string.Empty},
                                {"tran_cheque_no",string.Empty},
                                {"tran_cheque_date",string.Empty},
                                {"tran_expense_month",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_maker_id",objCmnFunctions.GetLoginUserGid().ToString()},
                                {"tran_emp_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                {"tran_checker_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                    string insertforglDepD = objCommonIUD.InsertCommon(glCoulmsDepD, "fa_trn_ttran");
                }
                if (CurrentDepAmt != 0 || CurrentDepAmt < 0)
                {
                    string[,] glCoulmsDepC = new string[,]
                            {                                                                                          
                                {"tran_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_val_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_payment_date",convertoDate (DateTime.Today.ToShortDateString())},
                                {"tran_gl_no",resdepglCode},
                                {"tran_desc","DEP FOR THE MONTH OF " + Format(depdate)+ " - FOR THE ASSET :  " + asset_id.ToString()},
                                {"tran_amount",CurrentDepAmt.ToString()},
                                {"tran_acc_mode","C".ToString()},
                                {"tran_mult","1"},  
                                {"tran_fc_code",BS},
                                {"tran_cc_code",CC}, 
                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                {"tran_ou_code",branch},
                                {"tran_ref_flag",GetRef("DEP")},
                                // {"tran_ref_gid",_asset_id.ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                                {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                {"tran_expense_category","10"},
                                {"tran_primary_branch_code",branch.ToString()},
                                {"tran_secondary_branch_code",string.Empty},
                                {"tran_related_account",string.Empty},
                                {"tran_invoice_no",string.Empty},
                                {"tran_vendor_code",string.Empty},
                                {"tran_vendor_name",string.Empty},
                                {"tran_cheque_no",string.Empty},
                                {"tran_cheque_date",string.Empty},
                                {"tran_expense_month",convertoDate (DateTime.Today.ToShortDateString())},
                                //  {"tran_maker_id",objCmnFunctions.GetLoginUserGid().ToString()},
                                {"tran_emp_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                {"tran_checker_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                    string insertforglDepC = objCommonIUD.InsertCommon(glCoulmsDepC, "fa_trn_ttran");
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
        }

        public void InsertFromDepToPreDep()
        {
            try
            {

                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_depreciation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@querytype", "insert");
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }

        public void InsertFromDepToForeDep()
        {
            try
            {

                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_depreciation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@querytype", "insertforecast");
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }


        public void InsertPreDep(string _asset_id, string _group_id, decimal CurrentDepAmt, string depdate, string deptype)
        {
            try
            {
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@querytype", "assetdetails");
                cmd.Parameters.AddWithValue("@assetid", _asset_id);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                    _asset_id = dt.Rows[0]["assetdetails_gid"].ToString();
                string[,] colval = new string[,]
                                          {                                         
                                            {"depreciationpreliminaryrun_assetdet_gid",_asset_id },
                                            {"depreciationpreliminaryrun_asset_groupid",_group_id },
                                            {"depreciationpreliminaryrun_amount",CurrentDepAmt.ToString() },
                                            {"depreciationpreliminaryrun_month",depdate },
                                            {"depreciationpreliminaryrun_asset_owner","F" },
                                            {"depreciationpreliminaryrun_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                                            {"depreciationpreliminaryrun_insert_date",Convert.ToString(objCommonIUD.GetCurrentDate())} , 
                                            {"depreciationpreliminaryrun_type","M"} ,
                                           };
                string insert = objCommonIUD.InsertCommon(colval, "fa_trn_tdepreciationpreliminaryrun");
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }
        public void InsertForecasteDep(string _asset_id, string _group_id, decimal CurrentDepAmt, string depdate, string deptype)
        {
            try
            {
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@querytype", "assetdetails");
                cmd.Parameters.AddWithValue("@assetid", _asset_id);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                    _asset_id = dt.Rows[0]["assetdetails_gid"].ToString();
                depdate = "01-" + Format(depdate);
                string[,] colval = new string[,]
                                          {                                         
                                            {"depreciationforecast_assetdetail_gid",_asset_id },
                                            {"depreciationforecast_group_id",_group_id },
                                            {"depreciationforecast_amount",CurrentDepAmt.ToString() },
                                            {"depreciationforecast_dep_month",depdate },
                                            {"depreciationforecast_asset_owner","F" },
                                            {"depreciationforecast_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                                            {"depreciationforecast_insert_date",Convert.ToString(objCommonIUD.GetCurrentDate())} , 
                                            {"depreciationforecast_dep_type","M"} ,
                                           };
                string insert = objCommonIUD.InsertCommon(colval, "fa_trn_tdepreciationforecast");
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }
        public Boolean CanDepreciateToZero(string sAssetGroup, string sPONumber, string sFICCRefNumber)
        {
            decimal dTotValue = 0;
            int iTotQty = 0;
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                if (sFICCRefNumber == "0")
                {

                    dt = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_depreciation", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@querytype", "ponumb");
                    cmd.Parameters.AddWithValue("@ponumber", sPONumber);
                    cmd.Parameters.AddWithValue("@groupid", sAssetGroup);
                    cmd.CommandTimeout = 1000;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                else
                {
                    dt = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_depreciation", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@querytype", "ficcRefnum");
                    cmd.Parameters.AddWithValue("@ficcRefnumnber", sFICCRefNumber);
                    cmd.Parameters.AddWithValue("@groupid", sAssetGroup);
                    cmd.CommandTimeout = 1000;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                if (dt.Rows.Count > 0)
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dTotValue = dTotValue + Convert.ToDecimal(string.IsNullOrEmpty(dt.Rows[i]["h_purc_cost"].ToString()) ? "0" : dt.Rows[i]["h_purc_cost"]);
                        iTotQty = iTotQty + Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["Qty"].ToString()) ? "1" : dt.Rows[i]["Qty"].ToString());

                    }
                if (iTotQty == 0) iTotQty = 1;
                // if ((dTotValue / iTotQty) <= 5000)
                if ((dTotValue / iTotQty) < 0)
                    return true;
                else return false;
            }
            catch (Exception ex)
            {

                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return true;
            }
            finally
            {
                con.Close();
            }
        }

        public decimal GetTotalDep(DateTime dtCapitalisationDate, DateTime dtTillDate,
                                decimal dAssetValue, string sAssetCode,
                                 string cNot5000Case, string sBranch1, string sBranch2,
                                  DateTime dtBranchLaunch, DateTime dtLeaseStart, DateTime dtLeaseEnd,
                                 string sDepType, decimal dDepRate,
                                 string sAssetGroup = null,
                                 string sPONumber = null,
                                 string sFICCRef = null,
                                 string sAsset_GroupId = null,
                                 decimal dTransfer_value = 0,
                                 string CanDepreciateFully = null,
                                 int dDepDevRate = 0)
        {
            DateTime dtReference;
            Depreciation oodj = new Depreciation();

            int iLeaseMonths = 0;
            int iLeaseDays = 0;
            int iTotalDays = 0;
            decimal dDepRateDD = dDepRate;
            //string sQuery = "0";
            decimal value = 0;
            DateTime dtCalcLeaseEndDate = dtLeaseEnd;

            try
            {
                if (dtCapitalisationDate == Convert.ToDateTime("01-01-1900"))
                {
                    return 0;
                }
                else
                {
                    dtReference = dtCapitalisationDate;
                }

                switch (sDepType)
                {
                    case "SLM":
                        if (dtReference > Convert.ToDateTime("01-03-2013"))
                        {

                            dtReference = dtCapitalisationDate;
                        }
                        break;


                    case "LPM":
                        {
                            if (dtLeaseEnd == Convert.ToDateTime("01-01-1900")) return 0;

                            //iLeaseMonths = ((dtReference.Year - dtLeaseEnd.Year) * 12) + dtReference.Month - dtLeaseEnd.Month;

                            iLeaseMonths = ((dtLeaseStart.Year - dtLeaseEnd.Year) * 12) + dtLeaseStart.Month - dtLeaseEnd.Month;

                            if (iLeaseMonths < 0)
                            {
                                iLeaseMonths = (iLeaseMonths * -1) + 1;
                            }

                            /* changes done by Vijayavel J. starts */
                            if (dtReference > dtLeaseStart) dtLeaseStart = dtReference;
                            /* changes ends */

                            if (iLeaseMonths > 66)
                            {
                                iLeaseMonths = 66;
                                dtCalcLeaseEndDate = dtLeaseStart.AddMonths(iLeaseMonths);
                            }

                            /* changes done by Vijayavel J. starts */
                            if (dtCalcLeaseEndDate < dtLeaseStart)
                            {
                                dtCalcLeaseEndDate = dtLeaseStart.AddMonths(iLeaseMonths);
                            }

                            if (dtCalcLeaseEndDate > dtLeaseEnd)
                                dtCalcLeaseEndDate = dtLeaseEnd;
                            else
                            {
                                //if (dtCalcLeaseEndDate < dtTillDate && dtLeaseEnd > dtTillDate) dtCalcLeaseEndDate = dtLeaseEnd;
                            }
                            /* changes ends */

                            /* comment as per Mr.Arul instruction
                            double total1 = (dtCalcLeaseEndDate.Subtract(dtLeaseStart)).TotalDays + 1;
                            iLeaseDays = Convert.ToInt32(total1);
                             */

                            /*
                            double total1;

                            if (dtCalcLeaseEndDate > dtTillDate)
                            {
                                total1 = (dtCalcLeaseEndDate.Subtract(dtLeaseStart)).TotalDays + 1;
                                iLeaseDays = Convert.ToInt32(total1);
                            }
                            else
                            {
                                total1 = (dtTillDate.Subtract(dtLeaseStart)).TotalDays + 1;
                                iLeaseDays = Convert.ToInt32(total1);
                            }
                            */

                            double total1;
                            total1 = (dtCalcLeaseEndDate.Subtract(dtLeaseStart)).TotalDays + 1;
                            iLeaseDays = Convert.ToInt32(total1);
                            dtReference = dtLeaseStart;

                            if (iLeaseDays > 1980) iLeaseDays = 1980;

                            double rate = (100.0 / Convert.ToDouble(iLeaseDays)) * 365.0;
                            dDepRateDD = Convert.ToDecimal(rate);
                            break;
                        }
                }

                if (dtReference == Convert.ToDateTime("01-01-1900")) return 0;
                if (dtReference > dtTillDate)
                {
                    value = 0;
                    return value;
                }

                if (dtCalcLeaseEndDate < dtTillDate && dtCalcLeaseEndDate != Convert.ToDateTime("01-01-1900")) return dAssetValue;

                if (dDepDevRate != 0) dDepRateDD = dDepDevRate;

                //*********************************************************************************************

                double total = (dtTillDate.Subtract(dtReference)).TotalDays;
                iTotalDays = Convert.ToInt32(total);

                if (iTotalDays < 0)
                    iTotalDays = (iTotalDays * -1) + 1;
                else
                    iTotalDays = iTotalDays + 1;

                //*************************************************************************************

                decimal itotal = iTotalDays;
                value = dAssetValue * (dDepRateDD / 100) * (itotal / 365);

                // if (dAssetValue <= 5000)
                if (dAssetValue < 0)
                    switch (cNot5000Case)
                    {
                        case "Y": break;
                        case "N":
                            {
                                value = dAssetValue;
                                break;
                            }
                        case "":
                            if (CanDepreciateToZero(sAssetGroup, sPONumber, sFICCRef) && CanDepreciateFully == "Y")
                            {
                                string[,] colval = new string[,]
                                          {                                         
                                            {"assetdetails_not_5kcase","N" },
                                            };
                                string[,] whcolval = new string[,]
                                          {                                         
                                             {"assetdetails_asset_groupid",sAssetGroup} , 
                                           };
                                string update = objCommonIUD.UpdateCommon(colval, whcolval, "fa_trn_tassetdetails");
                            }
                            else
                            {
                                string[,] colval = new string[,]
                                          {                                         
                                            {"assetdetails_not_5kcase","Y" },
                                            };
                                string[,] whcolval = new string[,]
                                          {                                         
                                            {"assetdetails_asset_groupid",sAssetGroup},
                                            };
                                string update = objCommonIUD.UpdateCommon(colval, whcolval, "fa_trn_tassetdetails");

                            }
                            break;
                    }

                if ((value + (dAssetValue - dTransfer_value)) > dAssetValue)
                {
                    value = dTransfer_value;
                }

                // calculated value greater than asset value
                if (value > dAssetValue) value = dAssetValue;

                value = Math.Round(value, 2);

                /*
                    if (dAssetValue <= 5000)
                        switch (cNot5000Case)
                        {
                            case "Y": break;
                            case "N":
                                {
                                    value = dAssetValue;
                                    break;
                                }
                            case " ":
                                if (CanDepreciateToZero(sAssetGroup, sPONumber, sFICCRef) && CanDepreciateFully == "Y")
                                {
                                    string[,] colval = new string[,]
                                          {                                         
                                            {"assetdetails_not_5kcase","N" },
                                            };
                                    string[,] whcolval = new string[,]
                                          {                                         
                                             {"assetdetails_asset_groupid",sAssetGroup} , 
                                           };
                                    string update = objCommonIUD.UpdateCommon(colval, whcolval, "fa_trn_tassetdetails");
                                }
                                else
                                {
                                    string[,] colval = new string[,]
                                          {                                         
                                            {"assetdetails_not_5kcase","Y" },
                                            };
                                    string[,] whcolval = new string[,]
                                          {                                         
                                            {"assetdetails_asset_groupid",sAssetGroup},
                                            };
                                    string update = objCommonIUD.UpdateCommon(colval, whcolval, "fa_trn_tassetdetails");

                                }
                                break;
                        }

                 if ((value + (dAssetValue - dTransfer_value)) > dAssetValue)
                    {
                        value = dAssetValue - (dAssetValue - dTransfer_value);
                    }
                */
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return value;
        }

        public void UpdateLeaseDates(string assetid, string lease_start, string lease_end)
        {
            try
            {
                string[,] colval = new string[,]
                                          {                                         
                                            {"assetdetails_lease_startdate",lease_start },
                                             {"assetdetails_lease_enddate",lease_end },
                                            };
                string[,] whcolval = new string[,]
                                          {                                         
                                            {"assetdetails_assetdet_id",assetid},
                                            };
                string update = objCommonIUD.UpdateCommon(colval, whcolval, "fa_trn_tassetdetails");
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }


        }

        public void UpdateReversals()
        {

            string[,] colval = new string[,]
                                          {                                         
                                           
                                            };
            string[,] whcolval = new string[,]
                                          {                                         
                                           
                                            };
            string update = objCommonIUD.UpdateCommon(colval, whcolval, "fa_trn_tassetdetails");
        }

        public List<SaleInvoice> getApprovedSaleDetails()
        {
            SaleInvoice obj = new SaleInvoice();
            GetConnection();
            List<SaleInvoice> objlist = new List<SaleInvoice>();
            try
            {
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_saleinvoice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@querytype", "GETDETAIL");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        obj = new SaleInvoice();
                        obj._sale_no = i["soaheader_sale_number"].ToString();
                        obj._sale_gid = Convert.ToInt32(i["soaheader_gid"].ToString());
                        obj._sale_amt = i["soaheader_sale_value"].ToString();
                        obj._sale_date = i["soaheader_sale_date"].ToString();
                        obj._vendor_name = i["soaheader_vendor_name"].ToString();
                        obj._vendor_address = i["soaheader_vendor_address"].ToString();
                        objlist.Add(obj);
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
            return objlist;
        }

        public List<IFAMS.Models.Ifams_Propertyx.ImpairmentsModel> GetImpairStatus()
        {
            List<IEM.Areas.IFAMS.Models.Ifams_Propertyx.ImpairmentsModel> Model = new List<IEM.Areas.IFAMS.Models.Ifams_Propertyx.ImpairmentsModel>();
            IEM.Areas.IFAMS.Models.Ifams_Propertyx.ImpairmentsModel obj = new IEM.Areas.IFAMS.Models.Ifams_Propertyx.ImpairmentsModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_imrheader", con);
                cmd.Parameters.AddWithValue("@Status", "Status");
                cmd.CommandType = CommandType.StoredProcedure;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new IEM.Areas.IFAMS.Models.Ifams_Propertyx.ImpairmentsModel();
                    obj.status = row["status_name"].ToString();
                    Model.Add(obj);
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
            return Model;
        }

        public void GenerateInvoicePDF(string id)
        {
            try
            {
                string barcodepath = ConfigurationManager.AppSettings["Barcodepath"].ToString();
                SaleInvoice obj = new SaleInvoice();
                GetConnection();
                DataTable datatable = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_saleinvoice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@querytype", "GETDETAILfromID");
                cmd.Parameters.AddWithValue("@data", id);
                da = new SqlDataAdapter(cmd);
                da.Fill(datatable);
                if (datatable.Rows.Count > 0)
                {
                    obj._vendor_name = datatable.Rows[0]["soaheader_vendor_name"].ToString();
                    obj._vendor_address = datatable.Rows[0]["soaheader_vendor_address"].ToString();
                    obj._invoice_no = GetInvoiceSequence();
                }
                string[,] colval = new string[,]
                                          {                                         
                                            {"soaheader_inv_no", obj._invoice_no  },                                            
                                          };
                string[,] whcolval = new string[,]
                                          {                                         
                                            {"soaheader_gid",id},
                                          };
                string update = objCommonIUD.UpdateCommon(colval, whcolval, "fa_trn_tsoaheader");
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[6] {
                            new DataColumn("Sno", typeof(string)),
                            new DataColumn("Particulars", typeof(string)),
                            new DataColumn("Quantity", typeof(int)),
                            new DataColumn("Price", typeof(int)),
                             new DataColumn("VAT", typeof(int)),
                            new DataColumn("Total", typeof(int))});
                DataTable detailstable = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_saleinvoice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@querytype", "GETsoaDETAILfromID");
                cmd.Parameters.AddWithValue("@data", id);
                da = new SqlDataAdapter(cmd);
                da.Fill(detailstable);
                if (detailstable.Rows.Count > 0)
                {
                    int ii = 0;
                    foreach (DataRow i in detailstable.Rows)
                    {
                        SaleInvoice obj1 = new SaleInvoice();
                        obj1._vat_amt = i["soadetail_vat_value"].ToString();
                        if (obj1._vat_amt == string.Empty) obj1._vat_amt = "0";
                        obj1._sale_amt = i["soadetail_sale_value"].ToString();
                        obj1._particulars = i["asset_description"].ToString();
                        decimal Total = Convert.ToDecimal(obj1._sale_amt) - Convert.ToDecimal(obj1._vat_amt);
                        ii += 1;
                        dt.Rows.Add(ii, obj1._particulars, 1, Total, obj1._vat_amt, obj1._sale_amt);
                    }
                }

                using (StringWriter sw = new StringWriter())
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw))
                        {
                            StringBuilder sb = new StringBuilder();
                            sb.Append("<div  ><table width='100%' cellspacing='0' cellpadding='2'>");
                            sb.Append("<tr><td ></td></tr><tr><td>");
                            sb.Append(string.Empty);
                            sb.Append("</td><td align='right'> ");
                            sb.Append("<img height='25%' src='" + ConfigurationManager.AppSettings["ficc"].ToString() + "'/><table><tr><td align='center'> ");
                            sb.Append("<b>" + System.Configuration.ConfigurationManager.AppSettings["CompanyFullName"].ToString() + "</b>");
                            sb.Append("</td></tr></table></td></tr>");
                            sb.Append("</table><font size='.5'><table><tr><td td align='center'>Corporate Office: " + System.Configuration.ConfigurationManager.AppSettings["MAddress1"].ToString() + ", " + System.Configuration.ConfigurationManager.AppSettings["MAddress2"].ToString() + ", " + System.Configuration.ConfigurationManager.AppSettings["MArea"].ToString() + ", " + System.Configuration.ConfigurationManager.AppSettings["MCity"].ToString());
                            sb.Append("<br/>Regd Office: " + System.Configuration.ConfigurationManager.AppSettings["LAddress1"].ToString() + ", " + System.Configuration.ConfigurationManager.AppSettings["LAddress2"].ToString() + ", " + System.Configuration.ConfigurationManager.AppSettings["LArea"].ToString() + ", " + System.Configuration.ConfigurationManager.AppSettings["LCity"].ToString());
                            sb.Append("</td></tr><tr><td align='center' style='color:#000;height:10px;'>");
                            sb.Append("________________________________________________________________________________________________________________________________</td></tr></table></font></div>");
                            sb.Append("<table >");
                            sb.Append("<tr><td align = 'center'><b>INVOICE</b></td></tr></table><font size='.5'><table ><tr><td align='center' style='color:#000;height:10px;'>");
                            sb.Append("________________________________________________________________________________________________________________________________</td></tr></table>");
                            sb.Append("<table   width='100%' cellspacing='0' cellpadding='2'><tr><td><table  border='0'><tr><td><b>Invoice to:</b></td></tr><tr><td>");
                            sb.Append(obj._vendor_name);
                            sb.Append("</td></tr>");
                            sb.Append("<tr><td>");
                            sb.Append(obj._vendor_address);
                            sb.Append(" </td></tr></table></td>");
                            sb.Append("<td align = 'right'><table border='0'><tr><td><b>Invoice No: </b>");
                            sb.Append(obj._invoice_no);
                            sb.Append(" </td></tr>");
                            sb.Append("<tr><td align='right'><b>Invoice Date: </b>");
                            sb.Append(convertoDate(DateTime.Now.ToShortDateString()));
                            sb.Append("</td></tr>");
                            sb.Append("</table></td></tr></table><table><tr><td align='center' style='color:#000;height:10px;'>");
                            sb.Append("________________________________________________________________________________________________________________________________</td></tr></table>");

                            //Generate Invoice (Bill) Items Grid.
                            sb.Append("</font><font size='1'><b>Invoice Details</b><br/><br/></font><font size='.5'><table border = '.5'>");
                            sb.Append("<tr bgcolor='rgb(51, 122, 183)' style='color: #FFF;'>");
                            foreach (DataColumn column in dt.Columns)
                            {
                                sb.Append("<th ><b>");
                                sb.Append(column.ColumnName);
                                sb.Append("</b></th>");
                            }
                            sb.Append("</tr>");
                            foreach (DataRow row in dt.Rows)
                            {
                                sb.Append("<tr>");
                                foreach (DataColumn column in dt.Columns)
                                {
                                    sb.Append("<td>");
                                    sb.Append(row[column]);
                                    sb.Append("</td>");
                                }
                                sb.Append("</tr>");
                            }
                            sb.Append("<tr><td align = 'right' colspan = '");
                            sb.Append(dt.Columns.Count - 1);
                            sb.Append("'>Total</td>");
                            sb.Append("<td>");
                            sb.Append(dt.Compute("sum(Total)", string.Empty));
                            //=============convert number to words
                            string ConvertedNumber = objCmnFunctions.ConvertDecimaltoWords(Convert.ToDecimal(dt.Compute("sum(Total)", string.Empty)));
                            //=============
                            sb.Append("</td>");
                            sb.Append("</tr></table>");
                            sb.Append("Total Amount in words: <p><B> " + ConvertedNumber + " RUPEES ONLY</B></p>");
                            sb.Append("<table><tr><td align='center' style='color:#000;height:10px;'>");
                            sb.Append("________________________________________________________________________________________________________________________________</td></tr></table>");
                            sb.Append("<b>DISCLAIMER :</b>");
                            sb.Append("<p>This is Sale of used asset items from " + System.Configuration.ConfigurationManager.AppSettings["CompanyFullName"].ToString() + " on as is where is basis. The above sale is not of any product and is without any <br/>");
                            sb.Append("gurantee as to  quality or performance. The buyer agrees that the same has been bought after proper inspection.</p>");
                            sb.Append("<table><tr><td align='center' style='color:#000;height:10px;'>");
                            sb.Append("________________________________________________________________________________________________________________________________</td></tr></table>");

                            sb.Append("<table><tr><td><table  width='100%' cellspacing='0' cellpadding='2'>");
                            sb.Append("<tr><td ><b>E&OE</td></tr>");
                            sb.Append("<tr><td ><b>MVAT TIN:  27280753193V, wef " + convertoDate(DateTime.Now.ToShortDateString()) + "</b></td></tr>");
                            sb.Append("<tr><td ><b>CST TIN:  " + ConfigurationManager.AppSettings["GSTIN"] + ", wef " + convertoDate(DateTime.Now.ToShortDateString()) + "</b></td></tr>");
                            sb.Append("</table></td><td>");
                            sb.Append("<table  width='100%' cellspacing='0' cellpadding='2'>");
                            sb.Append("<tr><td align='right'><b>For " + System.Configuration.ConfigurationManager.AppSettings["CompanyFullName"].ToString() + "</b></td></tr>");
                            sb.Append("</table><br/><br/><br/><br/><br/>");
                            sb.Append("<table  width='100%' cellspacing='0' cellpadding='2'>");
                            sb.Append("<tr><td align='right'><b>Authorized Signatory</b></td></tr>");
                            sb.Append("</table></td></tr></table><br/>");
                            sb.Append("<p align = 'center'>We hereby certify that our registration certificate under the Maharastra Value Added Tax Act 2002 is in force on the date on which the sale of the goods specified in this invoice is made by us and that the transaction of sale covered by this invoice has been effected by us and it shall be accounted for in the turnover of sales while filing of return and the due tax, if any, payable on the sale has been paid or shall be paid.");
                            sb.Append(" </font>");
                            //Export HTML String as PDF.
                            StringReader sr = new StringReader(sb.ToString());
                            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                            FileStream stream = new FileStream(barcodepath + "Invoice_" + obj._invoice_no + ".pdf", FileMode.Create);
                            PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                            pdfDoc.Open();
                            htmlparser.Parse(sr);
                            pdfDoc.Close();
                            //string lpath = ConfigurationManager.AppSettings["Barcodepath"].ToString();
                            //string Filpath = lpath + "Invoice_" + obj._invoice_no + ".pdf";

                            string fileName = "Invoice_" + obj._invoice_no;
                            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".pdf");
                            HttpContext.Current.Response.ContentType = "application/pdf";
                            HttpContext.Current.Response.Clear();
                            //HttpContext.Current.Response.TransmitFile(Filpath);
                            HttpContext.Current.Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
                            HttpContext.Current.Response.End();
                            //
                        }
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
        }

        public string GetInvoiceSequence()
        {
            string result = string.Empty;
            try
            {

                int lsSequence = 0;
                string lsQry = string.Empty;
                DataTable dt = new DataTable();
                CommonIUD objIUD = new CommonIUD();
                GetConnection();
                lsQry = "select  sequenceno_no  from fa_mst_tsequenceno";
                lsQry += " where sequenceno_code = 'INV'";
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = lsQry;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                lsSequence = Convert.ToInt32(dt.Rows[0]["sequenceno_no"]) + 1;
                string[,] codes = new string[,]
	                   { 
                             {"sequenceno_no",lsSequence.ToString()}                            
                       };
                string[,] whrcodes = new string[,]
	                   { 
                             {"sequenceno_code","INV"}                            
                       };
                string Comm = objIUD.UpdateCommon(codes, whrcodes, "fa_mst_tsequenceno");
                result = "INV" + lsSequence.ToString("0000000");

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public List<TransferMakerModel> getassetcat()
        {
            TransferMakerModel obj = new TransferMakerModel();
            List<TransferMakerModel> objModel = new List<TransferMakerModel>();
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                GetConnection();
                cmd = new SqlCommand("pr_ifams_mst_assetmaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@get", "Summary");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new TransferMakerModel();
                    obj._gid = Convert.ToInt32(row["assetcategory_gid"].ToString());
                    obj._tfr_status = row["assetcategory_depglno"].ToString();
                    obj._AssetCatCode = row["assetcategory_glno"].ToString();
                    obj._toa_number = row["assetcategory_name"].ToString();
                    obj._tfr_date = row["assetcategory_glno"].ToString();
                    objModel.Add(obj);
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

        public string InsertCat(TransferMakerModel obj)
        {
            try
            {
                string CAT = obj._toa_number.ToLower();
                CAT = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(CAT.ToString());
                if (CAT.Contains("'") == true)
                {
                    CAT = CAT.Replace("'", "''");
                }
                GetConnection();
                cmd = new SqlCommand("pr_ifams_mst_assetmaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = CAT.ToString();
                cmd.Parameters.Add("@get", SqlDbType.VarChar).Value = "Exist";
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
	                   { 
                            {"assetcategory_depglno", obj._tfr_status.ToString()} ,
                            {"assetcategory_glno",obj._AssetCatCode.ToString()} , 
                            {"assetcategory_name",obj._toa_number.ToString()} ,
                            {"assetcategory_depresglno",obj._tfr_date.ToString()} ,
                            {"assetcategory_assset_clearing",obj.asset_clearence_gl.ToString()} 
                       };
                    string Comm = objCommonIUD.InsertCommon(codes, "iem_mst_tassetcategory");
                    return null;
                }
                else
                {
                    return res1;
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
            return string.Empty;

        }

        public string EditCat(TransferMakerModel obj)
        {
            try
            {
                string CAT = obj._toa_number.ToLower();
                CAT = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(CAT.ToString());
                if (CAT.Contains("'") == true)
                {
                    CAT = CAT.Replace("'", "''");
                }
                GetConnection();
                cmd = new SqlCommand("pr_ifams_mst_assetmaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = CAT.ToString();
                cmd.Parameters.Add("@get", SqlDbType.VarChar).Value = "Exist";
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
	                   { 
                            {"assetcategory_depglno", obj._tfr_status.ToString()} ,
                            {"assetcategory_glno",obj._AssetCatCode.ToString()} , 
                            {"assetcategory_name",obj._toa_number.ToString()} ,
                            {"assetcategory_depresglno",obj._tfr_date.ToString()},
                            {"assetcategory_assset_clearing",obj.asset_clearence_gl.ToString()} 
                       };
                    string[,] whrcodes = new string[,]
	                   { 
                             {"assetcategory_gid",obj._gid.ToString()}                            
                       };
                    string Comm = objCommonIUD.UpdateCommon(codes, whrcodes, "iem_mst_tassetcategory");
                    return res1;
                }
                else
                {
                    return "there";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "ERROR";
            }
            finally
            {
                con.Close();
            }
        }

        public TransferMakerModel GetCatById(int id)
        {
            TransferMakerModel obj = new TransferMakerModel();
            try
            {

                GetConnection();
                cmd = new SqlCommand("pr_ifams_mst_assetmaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@get", "EDITDETAIL");
                cmd.Parameters.AddWithValue("@id", id);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new TransferMakerModel();
                    obj._gid = Convert.ToInt32(row["assetcategory_gid"].ToString());
                    obj._tfr_status = row["assetcategory_depglno"].ToString();
                    obj._AssetCatCode = row["assetcategory_glno"].ToString();
                    obj._toa_number = row["assetcategory_name"].ToString();
                    obj._tfr_date = row["assetcategory_glno"].ToString();
                    obj.asset_clearence_gl = row["assetcategory_assset_clearing"].ToString();
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
            return obj;
        }

        public string Deletecat(int id)
        {
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_mst_assetmaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = string.Empty;
                cmd.Parameters.Add("@get", SqlDbType.VarChar).Value = "delete";
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
	               {
                         {"assetcategory_isremoved", "Y"}	            
                   };
                    string[,] whrcol = new string[,]
	                {
                        {"assetcategory_gid", id.ToString()}
                    };
                    string tblname = "iem_mst_tassetcategory";
                    string deletetbl = objCommonIUD.UpdateCommon(codes, whrcol, tblname);
                    return null;
                }
                else { return res1; }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return string.Empty;
            }
            finally
            {
                con.Close();
            }
        }

        public void InsertDepMonths(string selectedDate, string prevDate, string refe, string tillDate)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_depreciation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "deleteTmpDates";
                cmd.Parameters.Add("@process_name", SqlDbType.VarChar).Value = refe;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
                string[,] codes = new string[,]
	               {
                         {"depmonths_ref", refe},
	                     {"depmonths_ran_for", convertoDate(Convert.ToDateTime(selectedDate).ToShortDateString())},
                         {"depmonths_pre_month",convertoDate(Convert.ToDateTime(prevDate).ToShortDateString())},
                         {"depmonths_tilldate",convertoDate( Convert.ToDateTime(tillDate).ToShortDateString())}
                   };
                string deletetbl = objCommonIUD.InsertCommon(codes, "fa_tmp_tdepmonths");
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }

        public Depreciation selcetdates(string ref_flag)
        {
            Depreciation dep = new Depreciation();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_depreciation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "selectdepmonts";
                cmd.Parameters.Add("@groupid", SqlDbType.VarChar).Value = ref_flag;
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    dep = new Depreciation();
                    dep._current_month = row["depmonths_ran_for"].ToString();
                    dep._date = row["depmonths_pre_month"].ToString();
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
            return dep;
        }

        public string toGetFincialyear()
        {
            int CurrentYear = DateTime.Today.Year;
            int PreviousYear = DateTime.Today.Year - 1;
            int NextYear = DateTime.Today.Year + 1;
            string PreYear = PreviousYear.ToString();
            string NexYear = NextYear.ToString();
            string CurYear = CurrentYear.ToString();
            string FinYear = null;

            if (DateTime.Today.Month > 3)
                FinYear = CurYear + "-" + NexYear;
            else
                FinYear = PreYear + "-" + CurYear;
            return FinYear.Trim();
        }
        public string toGetFincialyear(DateTime date)
        {
            int CurrentYear = date.Year;
            int PreviousYear = date.Year - 1;
            int NextYear = date.Year + 1;
            string PreYear = PreviousYear.ToString();
            string NexYear = NextYear.ToString();
            string CurYear = CurrentYear.ToString();
            string FinYear = null;

            if (date.Month > 3)
                FinYear = CurYear + "-" + NexYear;
            else
                FinYear = PreYear + "-" + CurYear;
            return FinYear.Trim();

        }


        public List<Depreciation> GetdetailsforDepHold()
        {
            List<Depreciation> objModel = new List<Depreciation>();
            try
            {

                Depreciation obj = new Depreciation();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_tdephold", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@resultfor", SqlDbType.VarChar).Value = "FREE_ASSET";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new Depreciation();
                    DataTable dt1 = new DataTable();
                    obj._gid = Convert.ToInt32(row["_gid"].ToString());
                    obj._asset_id = row["_asset_id"].ToString();
                    obj._asset_code = Convert.ToString(row["_asset_code"].ToString());
                    obj._AssetDesp = Convert.ToString(row["_AssetDesp"].ToString());
                    obj._branch = row["_branch"].ToString();
                    obj._group_id = row["_group_id"].ToString();
                    obj._cap_date = row["_cap_date"].ToString();
                    obj._asset_value = Convert.ToDecimal(row["_asset_value"].ToString());
                    objModel.Add(obj);
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

        //public DataSet GetdetailsforDepHold()
        //{
        //    DataSet dt = new DataSet();
        //    try
        //    {
        //        GetConnection();
        //        cmd = new SqlCommand("pr_ifams_trn_tdephold", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@resultfor", SqlDbType.VarChar).Value = "FREE_ASSET";
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return dt;
        //}

        public List<Depreciation> GetdetailsforDepRelease()
        {
            List<Depreciation> objModel = new List<Depreciation>();
            try
            {
                Depreciation obj = new Depreciation();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_tdephold", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@resultfor", SqlDbType.VarChar).Value = "HELD_ASSET";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new Depreciation();

                    obj._gid = Convert.ToInt32(row["_gid"].ToString());
                    obj._asset_id = row["_asset_id"].ToString();
                    obj._asset_code = Convert.ToString(row["_asset_code"].ToString());
                    obj._AssetDesp = Convert.ToString(row["_AssetDesp"].ToString());
                    obj._branch = row["_branch"].ToString();
                    obj._group_id = row["_group_id"].ToString();
                    obj._cap_date = row["_cap_date"].ToString();
                    obj._asset_value = Convert.ToDecimal(row["_asset_value"].ToString());
                    objModel.Add(obj);
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


        //public DataSet GetdetailsforDepRelease()
        //{
        //    DataSet dt = new DataSet();
        //    try
        //    {
        //        GetConnection();
        //        cmd = new SqlCommand("pr_ifams_trn_tdephold", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@resultfor", SqlDbType.VarChar).Value = "HELD_ASSET";
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return dt;
        //}

        public void Insert_deponhold(string asset_gid)
        {
            try
            {
                string[,] col = new string[,]
                   {
                    {"deponhold_entitygid", "1"}
                    , {"deponhold_assetdet_gid", asset_gid} 
                    , {"deponhold_flag", "Y"}              
                    , {"deponhold_isremoved", "N"}
                    , {"deponhold_insert_by", Convert.ToString(objCmnFunctions.GetLoginUserGid()) }
                    , {"deponhold_insert_date",Convert.ToString(objCommonIUD.GetCurrentDate())} 
                  
                   };
                string inserttbl = objCommonIUD.InsertCommon(col, "fa_trn_tdeponhold");

                string[,] cols = new string[,]
                   {                               
                    {"assetdetails_dep_onhold", "Y"}    
                    ,{"assetdetails_update_by", Convert.ToString(objCmnFunctions.GetLoginUserGid()) }               
                    ,{"assetdetails_update_date",Convert.ToString(objCommonIUD.GetCurrentDate())} 
                    };
                string[,] whcol = new string[,]
                   {
                    {"assetdetails_gid", asset_gid}                   
                   };
                string updatetbl = objCommonIUD.UpdateCommon(cols, whcol, "fa_trn_tassetdetails");
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }
        public void Update_deponhold(string asset_gid)
        {
            try
            {
                string[,] codes = new string[,]
                   {                  
                     {"deponhold_flag", "N"}          
                    , {"deponhold_isremoved", "Y"}
                    , {"deponhold_update_by", Convert.ToString(objCmnFunctions.GetLoginUserGid()) }
                    , {"deponhold_update_date",Convert.ToString(objCommonIUD.GetCurrentDate())}                   
                   };

                string[,] codes2 = new string[,]
                   {                    
                     {"deponhold_assetdet_gid", asset_gid}
                   };
                string deletetbl = objCommonIUD.UpdateCommon(codes, codes2, "fa_trn_tdeponhold");
                string[,] cols = new string[,]
                   {                               
                    {"assetdetails_dep_onhold", "N"}                   
                    ,{"assetdetails_takenby", "14"}  
                    ,{"assetdetails_update_by", Convert.ToString(objCmnFunctions.GetLoginUserGid()) }               
                    ,{"assetdetails_update_date",Convert.ToString(objCommonIUD.GetCurrentDate())} 
                    };
                string[,] whcol = new string[,]
                   {
                    {"assetdetails_gid", asset_gid}                   
                   };
                string updatetbl = objCommonIUD.UpdateCommon(cols, whcol, "fa_trn_tassetdetails");
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
        }

        public DateTime getDepRateChangeDetail(string asset_gid)
        {

            DateTime date = Convert.ToDateTime("01-01-2001");
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_depreciation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "getratechangedetails";
                cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = asset_gid;
                cmd.CommandTimeout = 1000;
                date = Convert.ToDateTime(Convert.ToString(cmd.ExecuteScalar()));
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return date;
        }


        public List<SaleInvoice> getSaleDetails(string id)
        {
            SaleInvoice obj = new SaleInvoice();
            List<SaleInvoice> objlist = new List<SaleInvoice>();
            try
            {
                GetConnection();
                DataTable datatable = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_saleinvoice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@querytype", "GETDETAILfromID");
                cmd.Parameters.AddWithValue("@data", id);
                da = new SqlDataAdapter(cmd);
                da.Fill(datatable);
                if (datatable.Rows.Count > 0)
                {
                    obj._vendor_name = datatable.Rows[0]["soaheader_vendor_name"].ToString();
                    obj._vendor_address = datatable.Rows[0]["soaheader_vendor_address"].ToString();
                    obj._customer_name = datatable.Rows[0]["customer_name"].ToString();
                    obj._customer_address = datatable.Rows[0]["customer_address"].ToString();
                    obj._Gstin_number = datatable.Rows[0]["Gstin_number"].ToString();
                    obj._state_code = datatable.Rows[0]["state_code"].ToString();
                    obj._state_name = datatable.Rows[0]["state_name"].ToString();
                    obj.customer_city = datatable.Rows[0]["city_name"].ToString();
                    obj.customer_pincode = datatable.Rows[0]["pincode"].ToString();
                    obj.customer_district = datatable.Rows[0]["district_name"].ToString();


                    obj._invoice_no = GetInvoiceSequence();
                }
                string[,] colval = new string[,]
                                          {                                         
                                            {"soaheader_inv_no", obj._invoice_no  },                                            
                                          };
                string[,] whcolval = new string[,]
                                          {                                         
                                            {"soaheader_gid",id},
                                          };
                string update = objCommonIUD.UpdateCommon(colval, whcolval, "fa_trn_tsoaheader");
                objlist.Add(obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return objlist;
        }

        public List<SaleInvoice> getApprovedSale(string id)
        {
            SaleInvoice obj = new SaleInvoice();
            List<SaleInvoice> objlist = new List<SaleInvoice>();
            DataTable detailstable = new DataTable();
            try
            {
                cmd = new SqlCommand("pr_ifams_trn_saleinvoice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@querytype", "GETsoaDETAILfromID");
                cmd.Parameters.AddWithValue("@data", id);
                da = new SqlDataAdapter(cmd);
                da.Fill(detailstable);
                if (detailstable.Rows.Count > 0)
                {

                    foreach (DataRow i in detailstable.Rows)
                    {
                        SaleInvoice obj1 = new SaleInvoice();
                        obj1._vat_amt = string.Empty;//i["soadetail_vat_value"].ToString();
                        if (obj1._vat_amt == string.Empty) obj1._vat_amt = "0";
                        obj1._sale_amt = i["soadetail_sale_value"].ToString();
                        obj1._particulars = i["asset_description"].ToString();
                        obj1._vatpercentage = "0";//i["soaheader_vat_percentage"].ToString();
                        obj1._code = i["asset_code"].ToString();
                        obj1.total = Convert.ToDecimal(i["total"].ToString());
                        obj1._qty = i["qty"].ToString();
                        objlist.Add(obj1);
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
            return objlist;
        }
        public List<SaleInvoice> getFICCdetails(string id)
        {
            //SaleInvoice obj = new SaleInvoice();
            List<SaleInvoice> objFICClist = new List<SaleInvoice>();
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand("pr_ifams_trn_saleinvoice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@querytype", "GETFICCDetails");
                cmd.Parameters.AddWithValue("@data", id);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow F in dt.Rows)
                    {
                        SaleInvoice obj = new SaleInvoice();
                        obj._FiccGstin = F["branchgst_gstin"].ToString();
                        obj._Ficc_Branchaddress = F["branch_addr1"].ToString();
                        obj._Ficc_Location = F["branch_location_name"].ToString();
                        obj._supplier_statename = F["state_name"].ToString();
                        objFICClist.Add(obj);
                    }
                }
                else
                {
                    SaleInvoice obj = new SaleInvoice();
                    obj._FiccGstin = "0".ToString();
                    objFICClist.Add(obj);
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
            return objFICClist;
        }


        public List<SaleInvoice> GetTaxdetails(string id)
        {
            List<SaleInvoice> objtax = new List<SaleInvoice>();
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand("pr_ifams_trn_saleinvoice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@querytype", "GETTaxDetails");
                cmd.Parameters.AddWithValue("@data", id);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow tax in dt.Rows)
                    {
                        SaleInvoice obj = new SaleInvoice();
                        obj._hsn_code = Convert.ToString(tax["hsn_code"]);
                        obj._CGstamt = Convert.ToDecimal(tax["soadetail_cgst_amount"]);
                        obj._CGstrat = Convert.ToDecimal(tax["soadetail_cgst_rate"]);
                        obj._SGstamt = Convert.ToDecimal(tax["soadetail_sgst_amount"]);
                        obj._SGstrat = Convert.ToDecimal(tax["soadetail_sgst_rate"]);
                        obj._IGstamt = Convert.ToDecimal(tax["soadetail_igst_amount"]);
                        obj._IGstrat = Convert.ToDecimal(tax["soadetail_igst_rate"]);
                        obj._Cessamt = Convert.ToDecimal(tax["soadetail_ccess_amount"]);
                        obj._Cessrat = Convert.ToDecimal(tax["soadetail_ccess_rate"]);
                        obj._Ccessamt = Convert.ToDecimal(tax["soadetail_cgcess_amount"]);
                        obj._Ccessrat = Convert.ToDecimal(tax["soadetail_cgcess_rate"]);
                        obj._Scessamt = Convert.ToDecimal(tax["soadetail_sgcess_amount"]);
                        obj._Scessrat = Convert.ToDecimal(tax["soadetail_sgcess_rate"]);
                        obj._Icessamt = Convert.ToDecimal(tax["soadetail_igcess_amount"]);
                        obj._Icessrat = Convert.ToDecimal(tax["soadetail_igcess_rate"]);
                        obj._taxTotal = Convert.ToDecimal(tax["Totaltaxamount"]);
                        obj._taxRateTotal = Convert.ToDecimal(tax["Totaltaxrate"]);
                        obj._cessTotal = Convert.ToDecimal(tax["Totalcessamt"]);
                        obj._totalCessrate = Convert.ToDecimal(tax["Totalcessrate"]);
                        obj.tot_taxableamount = Convert.ToDecimal(tax["taxableamount"]);
                        objtax.Add(obj);
                    }
                }
                else
                {
                    SaleInvoice obj = new SaleInvoice();
                    obj._FiccGstin = "0".ToString();
                    objtax.Add(obj);
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
            return objtax;
        }


        public List<SaleInvoice> GetCessTaxdetails(string id)
        {
            List<SaleInvoice> objcesstax = new List<SaleInvoice>();
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand("pr_ifams_trn_saleinvoice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@querytype", "GETCessTaxDetails");
                cmd.Parameters.AddWithValue("@data", id);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow tax in dt.Rows)
                    {
                        SaleInvoice obj = new SaleInvoice();
                        obj._hsn_code = Convert.ToString(tax["hsn_code"]);
                        obj._Ccessamt = Convert.ToDecimal(tax["soadetail_cgcess_amount"]);
                        obj._Ccessrat = Convert.ToDecimal(tax["soadetail_cgcess_rate"]);
                        obj._Scessamt = Convert.ToDecimal(tax["soadetail_sgcess_amount"]);
                        obj._Scessrat = Convert.ToDecimal(tax["soadetail_sgcess_rate"]);
                        obj._Icessamt = Convert.ToDecimal(tax["soadetail_igcess_amount"]);
                        obj._Icessrat = Convert.ToDecimal(tax["soadetail_igcess_rate"]);
                        obj._cessTotal = Convert.ToDecimal(tax["Totalcessamt"]);
                        obj._totalCessrate = Convert.ToDecimal(tax["Totalcessrate"]);
                        objcesstax.Add(obj);
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
            return objcesstax;
        }

        public DataSet GetAssetDetails(string LocationCode = null, string capdate = null)
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_rpt_SplitReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@locationcode", SqlDbType.VarChar).Value = LocationCode;
                cmd.Parameters.Add("@capdate", SqlDbType.VarChar).Value = capdate;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getassetdetailsbylocation";
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

        public string Getempcode(string empgid)
        {
            string empid = string.Empty;
            GetConnection();
            DataTable dtdep = new DataTable();
            cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@action", "empcode");
            cmd.Parameters.AddWithValue("@empgid", empgid);
            da = new SqlDataAdapter(cmd);
            da.Fill(dtdep);
            foreach (DataRow rowdep in dtdep.Rows)
            {
                empid = rowdep["employee_code"].ToString();
            }

            return empid;
        }



        public decimal getRetificationAmount(string assetId)
        {
            GetConnection();
            decimal rectificationamt = 0;
            try
            {
                DataTable dtwdv = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "WDV");
                cmd.Parameters.AddWithValue("@assetid", assetId);
                da = new SqlDataAdapter(cmd);
                da.Fill(dtwdv);

                DateTime dtBranchLaunch = Convert.ToDateTime("01-01-1900");
                DateTime soacaptilisationda = Convert.ToDateTime("01-01-1900");
                decimal AssetValue = 0;
                string sAssetCode = string.Empty;
                string cNot5000Case = string.Empty;
                DateTime dtLeaseStart = Convert.ToDateTime("01-01-1900");
                DateTime dtLeaseEnd = Convert.ToDateTime("01-01-1900");
                string sDepType = string.Empty;
                decimal dDepRate = 0;
                string sAssetGroup = string.Empty;
                DateTime dtTillDate = DateTime.Now;
                string sPONumber = string.Empty;
                string sFICCRef = string.Empty;
                string sAsset_GroupId = string.Empty;
                decimal dTransfer_value = 0;
                string CanDepreciateFully = string.Empty;
                Int32 dDepDevRate = 0;
                string sBranch1 = string.Empty;
                string sBranch2 = string.Empty;
                foreach (DataRow row1 in dtwdv.Rows)
                {
                    dtBranchLaunch = Convert.ToDateTime(string.IsNullOrEmpty(row1["branch_start_date"].ToString()) ? "0" : row1["branch_start_date"]);
                    soacaptilisationda = Convert.ToDateTime(string.IsNullOrEmpty(row1["assetdetails_cap_date"].ToString()) ? "01-01-1900" : row1["assetdetails_cap_date"]);
                    AssetValue = Convert.ToDecimal(string.IsNullOrEmpty(row1["assetdetails_asset_value"].ToString()) ? "0" : row1["assetdetails_asset_value"]);
                    sAssetCode = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_asset_code"].ToString()) ? "0" : row1["assetdetails_asset_code"]);
                    cNot5000Case = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_not_5kcase"].ToString()) ? "0" : row1["assetdetails_not_5kcase"]);
                    dtLeaseStart = Convert.ToDateTime(string.IsNullOrEmpty(row1["assetdetails_lease_startdate"].ToString()) ? "01-01-1900" : row1["assetdetails_lease_startdate"]);
                    dtLeaseEnd = Convert.ToDateTime(string.IsNullOrEmpty(row1["assetdetails_lease_enddate"].ToString()) ? "01-01-1900" : row1["assetdetails_lease_enddate"]);
                    sDepType = Convert.ToString(string.IsNullOrEmpty(row1["asset_dep_type"].ToString()) ? "0" : row1["asset_dep_type"]);
                    dDepRate = Convert.ToDecimal(string.IsNullOrEmpty(row1["asset_dep_rate"].ToString()) ? "0" : row1["asset_dep_rate"]);
                    sAssetGroup = Convert.ToString(string.IsNullOrEmpty(row1["h_asst_groupid_no"].ToString()) ? "0" : row1["h_asst_groupid_no"]);
                    sPONumber = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_po_number"].ToString()) ? "0" : row1["assetdetails_po_number"]);
                    sFICCRef = Convert.ToString(string.IsNullOrEmpty(row1["h_ficrefnumber"].ToString()) ? "0" : row1["h_ficrefnumber"]);
                    sAsset_GroupId = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_asset_groupid"].ToString()) ? "0" : row1["assetdetails_asset_groupid"]);
                    dTransfer_value = Convert.ToDecimal(string.IsNullOrEmpty(row1["assetdetails_trf_value"].ToString()) ? "0" : row1["assetdetails_trf_value"]);
                }

                DataTable dtdep = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "Totdep");
                cmd.Parameters.AddWithValue("@assetgid", assetId);
                decimal sumassetdep = Convert.ToDecimal(cmd.ExecuteScalar());

                decimal depamt = Math.Round(GetTotalDep(soacaptilisationda, dtTillDate,
                                AssetValue, sAssetCode,
                             cNot5000Case, sBranch1, sBranch2,
                             dtBranchLaunch, dtLeaseStart, dtLeaseEnd,
                             sDepType, dDepRate,
                             sAssetGroup,
                             sPONumber,
                             sFICCRef,
                             sAsset_GroupId,
                             dTransfer_value,
                             CanDepreciateFully,
                             dDepDevRate), 2);
                rectificationamt = depamt - sumassetdep;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return rectificationamt;

        }


        public string SelectDepDetailsnInsert(DateTime tilldate, string deptype)
        {
            string success = "success";
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_depreciation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (deptype == "run")
                {
                    cmd.Parameters.AddWithValue("@querytype", "DepRunSelectNInsert");
                }
                else if (deptype == "prerun")
                {
                    cmd.Parameters.AddWithValue("@querytype", "PreRunSelectNInsert");
                }
                else if (deptype == "forerun")
                {
                    cmd.Parameters.AddWithValue("@querytype", "ForeRunSelectNInsert");
                }
                cmd.Parameters.AddWithValue("@process_name", convertoDate(tilldate.ToShortDateString()));
                cmd.Parameters.AddWithValue("@person", objCmnFunctions.GetLoginUserGid().ToString());
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();

                if (deptype == "run")
                {
                    cmd = new SqlCommand("pr_ifams_trn_depreciation", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@querytype", "insertTranGLUpload");
                    cmd.Parameters.AddWithValue("@person", Convert.ToString(HttpContext.Current.Session["employee_code"]));
                    cmd.CommandTimeout = 0;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                success = "failed" + ex.Message.ToString();
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return success;
        }

        public string UpdateNot5KCase()
        {
            string success = "success";
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_depreciation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@querytype", "UpdateNot5KCase");
                cmd.CommandTimeout = 0;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                success = "failed";
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return success;
        }
        public DataTable GetDetailsForBarcodePrint(string id)
        {
            DataTable dt = new DataTable(); try
            {

                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_barcodegeneration", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "ASSETID";
                cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = id.ToString();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    string barStatus = dt.Rows[0]["barcodedetials_bar_status"].ToString();
                    if (barStatus == "NOT PRINTED")
                    {
                        string[,] col = new string[,]
                            { 
                                {"barcodedetials_bar_status", "PRINTED"},                             
                                {"barcodedetails_update_by",objCmnFunctions.GetLoginUserGid().ToString()},
                                {"barcodedetails_update_date",objCommonIUD.GetCurrentDate()}                        
                            };
                        string[,] Whrcol = new string[,]
                            {                                                              
                                {"barcodedetials_assetdet_gid",id.ToString()}                             
                            };
                        string updateComm = objCommonIUD.UpdateCommon(col, Whrcol, "fa_trn_tbarcodedetails");
                    }
                }
            }
            catch (Exception ex)
            {
                dt = null;
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return dt;
        }

        public List<AssetReportModel> SelectFARreportDetails(string Selecteddate, string tilldate)
        {
            DataTable fatable = new DataTable();
            List<AssetReportModel> objmod = new List<AssetReportModel>();
            AssetReportModel obj = new AssetReportModel();
            try
            {
                GetConnection();
                int openmonth = Convert.ToDateTime(tilldate).Month - 1;
                string openyear = toGetFincialyear(Convert.ToDateTime(tilldate));
                string[] year = openyear.Split('-');
                int years = Convert.ToInt32(year[0]);
                cmd = new SqlCommand("pr_ifams_trn_depreciation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@querytype", "ForecastReport");
                cmd.Parameters.AddWithValue("@opening_till", year[0] + "-MAR-31");
                cmd.Parameters.AddWithValue("@period_from", convertoDate(Convert.ToDateTime(tilldate).ToShortDateString()));
                cmd.Parameters.AddWithValue("@period_to", convertoDate(Convert.ToDateTime(Selecteddate).ToShortDateString()));
                da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.Fill(fatable);
                for (int i = 0; i < fatable.Rows.Count; i++)
                {
                    obj = new AssetReportModel();
                    obj.acfnumber = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["ECF No"].ToString()) ? " - " : fatable.Rows[i]["ECF No"]);
                    obj.assetdetgroupid = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["assetdetails_asset_groupid"].ToString()) ? " 0 " : fatable.Rows[i]["assetdetails_asset_groupid"]);
                    obj.assetdetDetid = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["assetdetails_assetdet_id"].ToString()) ? " - " : fatable.Rows[i]["assetdetails_assetdet_id"]); //fatable.Rows[i]["assetdetails_assetdet_id"].ToString();
                    obj.assetdetqty = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["assetdetails_qty"].ToString()) ? " - " : fatable.Rows[i]["assetdetails_qty"]);// fatable.Rows[i]["assetdetails_qty"].ToString();
                    obj.assetdetCode = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["asset_code"].ToString()) ? " - " : fatable.Rows[i]["asset_code"]); //fatable.Rows[i]["asset_code"].ToString();
                    obj.assetdetDescription = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["asset_description"].ToString()) ? " - " : fatable.Rows[i]["asset_description"]); // fatable.Rows[i]["asset_description"].ToString();
                    obj.assetdetglcode = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["asset_glcode"].ToString()) ? " - " : fatable.Rows[i]["asset_glcode"]); //fatable.Rows[i]["asset_glcode"].ToString();
                    obj.deptype = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["asset_dep_type"].ToString()) ? " - " : fatable.Rows[i]["asset_dep_type"]); //fatable.Rows[i]["asset_dep_type"].ToString();
                    obj.deprate = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["asset_dep_rate"].ToString()) ? " 0.00 " : fatable.Rows[i]["asset_dep_rate"]); //fatable.Rows[i]["asset_dep_rate"].ToString();
                    obj.depgl = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["asset_dep_glcode"].ToString()) ? " - " : fatable.Rows[i]["asset_dep_glcode"]); //fatable.Rows[i]["asset_dep_glcode"].ToString();
                    obj.deppvgl = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["asset_dep_reservglcode"].ToString()) ? " - " : fatable.Rows[i]["asset_dep_reservglcode"]); //fatable.Rows[i]["asset_dep_reservglcode"].ToString();
                    obj.assetdetLocationcode = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["branch_code"].ToString()) ? " - " : fatable.Rows[i]["branch_code"]); //fatable.Rows[i]["branch_code"].ToString();
                    obj.branchlaunchdat = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["branch_start_date"].ToString()) ? " - " : fatable.Rows[i]["branch_start_date"]); //fatable.Rows[i]["branch_start_date"].ToString();
                    obj.assetleasedat = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["assetdetails_lease_startdate"].ToString()) ? " - " : fatable.Rows[i]["assetdetails_lease_startdate"]); //fatable.Rows[i]["assetdetails_lease_startdate"].ToString();
                    obj.assetleaseenddat = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["assetdetails_lease_enddate"].ToString()) ? " - " : fatable.Rows[i]["assetdetails_lease_enddate"]); //fatable.Rows[i]["assetdetails_lease_enddate"].ToString();
                    obj.assetcapdate = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["assetdetails_cap_date"].ToString()) ? " - " : fatable.Rows[i]["assetdetails_cap_date"]); //fatable.Rows[i]["assetdetails_cap_date"].ToString();
                    obj.assetdetAssetvalue = Convert.ToDecimal(string.IsNullOrEmpty(fatable.Rows[i]["Asset Base Value"].ToString()) ? " 0.00 " : fatable.Rows[i]["Asset Base Value"]); //Convert.ToDecimal(fatable.Rows[i]["Asset Base Value"].ToString());
                    obj.depapr = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["Apr"].ToString()) ? " 0 " : fatable.Rows[i]["Apr"]); //fatable.Rows[i]["Apr"].ToString();
                    obj.depmay = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["May"].ToString()) ? " 0 " : fatable.Rows[i]["May"]); //fatable.Rows[i]["May"].ToString();
                    obj.depjun = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["Jun"].ToString()) ? " 0 " : fatable.Rows[i]["Jun"]); //fatable.Rows[i]["Jun"].ToString();
                    obj.depjul = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["Jul"].ToString()) ? " 0 " : fatable.Rows[i]["Jul"]); //fatable.Rows[i]["Jul"].ToString();
                    obj.depaug = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["Aug"].ToString()) ? " 0 " : fatable.Rows[i]["Aug"]); //fatable.Rows[i]["Aug"].ToString();
                    obj.depsep = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["Sep"].ToString()) ? " 0 " : fatable.Rows[i]["Sep"]); //fatable.Rows[i]["Sep"].ToString();
                    obj.depoct = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["Oct"].ToString()) ? " 0 " : fatable.Rows[i]["Oct"]); //fatable.Rows[i]["Oct"].ToString();
                    obj.depnov = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["Nov"].ToString()) ? " 0 " : fatable.Rows[i]["Nov"]); //fatable.Rows[i]["Nov"].ToString();
                    obj.depdec = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["Dec"].ToString()) ? " 0 " : fatable.Rows[i]["Dec"]); //fatable.Rows[i]["Dec"].ToString();
                    obj.depjan = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["Jan"].ToString()) ? " 0 " : fatable.Rows[i]["Jan"]); //fatable.Rows[i]["Jan"].ToString();
                    obj.depfeb = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["Feb"].ToString()) ? " 0 " : fatable.Rows[i]["Feb"]); //fatable.Rows[i]["Feb"].ToString();
                    obj.depmar = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["Mar"].ToString()) ? " 0 " : fatable.Rows[i]["Mar"]); //fatable.Rows[i]["Mar"].ToString();
                    obj.ACCUMULATED_DEP = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["opening_amount"].ToString()) ? " 0.00 " : fatable.Rows[i]["opening_amount"]); //fatable.Rows[i]["opening_amount"].ToString();
                    obj.CUMMULATIVE_DEP = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["dep_current"].ToString()) ? " 0.00 " : fatable.Rows[i]["dep_current"]); //fatable.Rows[i]["dep_current"].ToString();
                    obj.deptot = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["dep_total"].ToString()) ? " 0.00 " : fatable.Rows[i]["dep_total"]); //fatable.Rows[i]["dep_total"].ToString();
                    obj.wdv = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["wdv"].ToString()) ? " 0.00 " : fatable.Rows[i]["wdv"]); //fatable.Rows[i]["wdv"].ToString();
                    obj.assettrffrm = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["assetdetails_trf_from"].ToString()) ? "-" : fatable.Rows[i]["assetdetails_trf_from"]);
                    obj.assetdettrfval = Convert.ToDecimal(string.IsNullOrEmpty(fatable.Rows[i]["Asset Base Value"].ToString()) ? " 0.00 " : fatable.Rows[i]["Asset Base Value"]); //Convert.ToDecimal(fatable.Rows[i]["Asset Base Value"].ToString());
                    obj.assetdetstaus = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["excelvalidate_name"].ToString()) ? " - " : fatable.Rows[i]["excelvalidate_name"]); //fatable.Rows[i]["excelvalidate_name"].ToString();
                    obj.assetdetstatus1 = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["asset_verifiable"].ToString()) ? " - " : fatable.Rows[i]["asset_verifiable"]); //fatable.Rows[i]["assetdetails_status"].ToString();
                    obj.assetdettrfdat = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["assetdetails_tfr_date"].ToString()) ? " - " : fatable.Rows[i]["assetdetails_tfr_date"]); //fatable.Rows[i]["assetdetails_tfr_date"].ToString();
                    obj.assetdetsaledat = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["assetdetails_sale_date"].ToString()) ? " - " : fatable.Rows[i]["assetdetails_sale_date"]); //fatable.Rows[i]["assetdetails_sale_date"].ToString();
                    obj.department = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["asset_classification"].ToString()) ? "-" : fatable.Rows[i]["asset_classification"]);
                    obj.bscc = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["depbscc"].ToString()) ? "-" : fatable.Rows[i]["depbscc"]);
                    obj.ponumb = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["PO No"].ToString()) ? " - " : fatable.Rows[i]["PO No"]); // fatable.Rows[i]["assetdetails_po_number"].ToString();
                    obj.cbfnumb = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["CBF No"].ToString()) ? "_" : fatable.Rows[i]["CBF No"]);
                    obj.vendornam = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["supplierheader_name"].ToString()) ? "_" : fatable.Rows[i]["supplierheader_name"]); //fatable.Rows[i]["VENDOR_NAME"].ToString();
                    obj.Naration = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["assetdetails_asset_description"].ToString()) ? " - " : fatable.Rows[i]["assetdetails_asset_description"]); //fatable.Rows[i]["assetdetails_asset_description"].ToString();
                    obj.Branch_Status = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["branch_active"].ToString()) ? " - " : fatable.Rows[i]["branch_active"]);
                    obj.BRName = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["branch_name"].ToString()) ? " - " : fatable.Rows[i]["branch_name"]);
                    obj.BRAddress = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["branch_addr1"].ToString()) ? " - " : fatable.Rows[i]["branch_addr1"]);
                    obj.SALE_VALUE = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["soadetail_sale_value"].ToString()) ? " 0.00 " : fatable.Rows[i]["soadetail_sale_value"]);
                    obj.VAT_VALUE = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["soadetail_vat_value"].ToString()) ? " 0.00 " : fatable.Rows[i]["soadetail_vat_value"]);
                    obj.NET_LOSS = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["soadetial_pl_value"].ToString()) ? " 0.00 " : fatable.Rows[i]["soadetial_pl_value"]);
                    obj.inwarddate = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["assetdetails_inwarddate"].ToString()) ? "-" : fatable.Rows[i]["assetdetails_inwarddate"].ToString());
                    objmod.Add(obj);
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
            return objmod;
        }
        public string SubmitDraftCWIP(DataTable assetdata)
        {
            string result = string.Empty;

            try
            {
                if (assetdata.Rows.Count > 0)
                {
                    GetConnection();
                    for (int i = 0; i < assetdata.Rows.Count; i++)
                    {

                        int res;
                        cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@fccc", SqlDbType.VarChar).Value = assetdata.Rows[i][2].ToString();
                        cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "assetcode";
                        res = Convert.ToInt32(cmd.ExecuteScalar());
                        string res1 = assetdata.Rows[i][3].ToString();
                        cmd = new SqlCommand("pr_ifams_rpt_SplitReport", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@locationcode", SqlDbType.VarChar).Value = res1;
                        cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "BS";
                        res1 = Convert.ToString(cmd.ExecuteScalar());

                        string[,] colval = new string[,]
                                          {                                             
                                {"cwip_remark", assetdata.Rows[i][5].ToString() },      
                                {"cwip_branch_gid", GetlocDetails(assetdata.Rows[i][3].ToString()).ToString()  },
                                {"cwip_asset_gid",res.ToString()  },      
                                {"cwip_cc", assetdata.Rows[i][4].ToString() },
                                {"cwip_bs", res1  },  
                                {"cwip_capitalisation_date", convertoDate( assetdata.Rows[i][6].ToString())  },  
                                {"cwip_serial_no", assetdata.Rows[i][8].ToString()  },      
                                {"cwip_manft_name",assetdata.Rows[i][7].ToString()  },
                                {"cwip_makerid",objCmnFunctions.GetLoginUserGid().ToString()  },
                                {"cwip_maker_date",convertoDate( objCommonIUD.GetCurrentDate())  },
                                {"cwip_status","2" },
                                          };
                        string[,] whcolval = new string[,]
                                          {                                         
                                            {"cwip_asset_id",assetdata.Rows[i][1].ToString()},
                                          };
                        result = objCommonIUD.UpdateCommon(colval, whcolval, "fa_trn_tcwip");
                    }
                    //Muthu hide 10-12-2016
                    //cmd = new SqlCommand("pr_ifams_trn_cwip", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@date", SqlDbType.VarChar).Value = convertoDate(objCommonIUD.GetCurrentDate());
                    //cmd.Parameters.Add("@person", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid().ToString();
                    //cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "get_grp_no";
                    //cmd.ExecuteNonQuery();
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
            return result;
        }

        public string CancelDraftCWIP()
        {
            string result = string.Empty;
            try
            {
                CWIPModel obj = new CWIPModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_cwip", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@date", SqlDbType.VarChar).Value = convertoDate(objCommonIUD.GetCurrentDate());
                cmd.Parameters.Add("@person", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid().ToString();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetDetailsToViewforCap";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string[,] colval = new string[,]
                                {                                             
                                {"cwip_remark", string.Empty },      
                                {"cwip_branch_gid", string.Empty  },
                                {"cwip_asset_gid",string.Empty  },      
                                {"cwip_cc", string.Empty },
                                {"cwip_bs", string.Empty  },  
                                {"cwip_capitalisation_date", string.Empty  },  
                                {"cwip_serial_no",string.Empty  },      
                                {"cwip_manft_name",string.Empty },
                                {"cwip_makerid",string.Empty  },
                                {"cwip_maker_date",string.Empty  },
                                {"cwip_status","0" },
                                };
                        string[,] whcolval = new string[,]
                                {                                         
                                {"cwip_gid",row["cwip_gid"].ToString()},
                                {"cwip_status","1" },
                                };


                        result = objCommonIUD.UpdateCommon(colval, whcolval, "fa_trn_tcwip");
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
            return result;
        }
        public IEnumerable<CWIPModel> GetCWIPCheckerList()
        {
            List<CWIPModel> objModel = new List<CWIPModel>();
            try
            {

                CWIPModel obj = new CWIPModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_cwip", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetAllCheckerCases";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new CWIPModel();

                    obj.cwip_gid = Convert.ToInt32(row["cwip_gid"].ToString());
                    obj.cwip_asset_id = row["cwip_asset_id"].ToString();
                    obj.cwip_group_id = Convert.ToString(row["cwip_group_id"].ToString());
                    obj.cwip_capitalisation_date = row["cwip_capitalisation_date"].ToString();
                    obj.cwip_description = Convert.ToString(row["cwip_remark"].ToString());
                    obj.cwip_supplier_name = Convert.ToString(row["cwip_supplier_name"].ToString());
                    obj.cwip_capitalisation_date = convertoDate(row["cwip_capitalisation_date"].ToString());
                    obj.cwip_asset_value = Convert.ToDecimal(row["cwip_asset_value"].ToString());
                    obj.cwip_cbf_number = Convert.ToString(row["cwip_cbf_number"].ToString());
                    obj.cwip_po_number = Convert.ToString(row["cwip_po_number"].ToString());
                    obj.cwip_ecf_number = Convert.ToString(row["cwip_ecf_number"].ToString());
                    obj.cwip_branch_code = Convert.ToString(row["branch_code"].ToString());
                    obj.cwip_asset_code = Convert.ToString(row["asset_code"].ToString());
                    obj.cwip_asset_descp = Convert.ToString(row["asset_description"].ToString());
                    obj.cwip_serial_number = Convert.ToString(row["aset_serial_no"].ToString());
                    objModel.Add(obj);
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
        public IEnumerable<CWIPModel> GetViewDetailsList()
        {
            List<CWIPModel> objModel = new List<CWIPModel>();
            try
            {

                CWIPModel obj = new CWIPModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_cwip", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@date", SqlDbType.VarChar).Value = convertoDate(objCommonIUD.GetCurrentDate());
                cmd.Parameters.Add("@person", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid().ToString();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetDetailsToViewforCap";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new CWIPModel();

                    obj.cwip_gid = Convert.ToInt32(row["cwip_gid"].ToString());
                    obj.cwip_asset_id = row["cwip_asset_id"].ToString();
                    obj.cwip_group_id = Convert.ToString(row["cwip_group_id"].ToString());
                    obj.cwip_capitalisation_date = row["cwip_capitalisation_date"].ToString();
                    obj.cwip_description = Convert.ToString(row["cwip_remark"].ToString());
                    obj.cwip_supplier_name = Convert.ToString(row["cwip_supplier_name"].ToString());
                    obj.cwip_capitalisation_date = row["cwip_capitalisation_date"].ToString();
                    obj.cwip_asset_value = Convert.ToDecimal(row["cwip_asset_value"].ToString());
                    obj.cwip_cbf_number = Convert.ToString(row["cwip_cbf_number"].ToString());
                    obj.cwip_po_number = Convert.ToString(row["cwip_po_number"].ToString());
                    obj.cwip_ecf_number = Convert.ToString(row["cwip_ecf_number"].ToString());
                    obj.cwip_branch_code = Convert.ToString(row["branch_code"].ToString());
                    obj.cwip_asset_code = Convert.ToString(row["asset_code"].ToString());
                    objModel.Add(obj);
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


        public string SubmitCapCWIP(string ids)
        {
            string result = string.Empty;
            try
            {
                CWIPModel obj = new CWIPModel();
                GetConnection();
                if (ids != null)
                {
                    string[] splitvalue = ids.Split(',');
                    for (int i = 0; i < splitvalue.Length; i++)
                    {
                        if (splitvalue[i] != null && splitvalue[i] != "" && splitvalue[i] != "undefined")
                        {
                            string[,] colval = new string[,]
                                {                                             
                                  {"cwip_status","2" },
                                };
                            string[,] whcolval = new string[,]
                                {                                         
                                {"cwip_gid",splitvalue[i].ToString()},
                                {"cwip_status","1" },
                                };
                            result = objCommonIUD.UpdateCommon(colval, whcolval, "fa_trn_tcwip");
                        }
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
            return result;
        }

        public string UnseletedCWIP(string ids)
        {
            string result = string.Empty;
            try
            {
                CWIPModel obj = new CWIPModel();
                GetConnection();
                if (ids != null)
                {
                    string[] splitvalue = ids.Split(',');
                    for (int i = 0; i < splitvalue.Length; i++)
                    {
                        if (splitvalue[i] != null && splitvalue[i] != "" && splitvalue[i] != "undefined")
                        {
                            string[,] colval = new string[,]
                                {                                             
                                  {"cwip_status","0" },
                                };
                            string[,] whcolval = new string[,]
                                {                                         
                                {"cwip_gid",splitvalue[i].ToString()},
                                {"cwip_status","1" },
                                };
                            result = objCommonIUD.UpdateCommon(colval, whcolval, "fa_trn_tcwip");
                        }
                        else
                        {
                            result = "Success";
                        }
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
            return result;
        }



        public IEnumerable<CWIPModel> GetCWIPMakerList()
        {
            List<CWIPModel> objModel = new List<CWIPModel>();
            try
            {

                CWIPModel obj = new CWIPModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_cwip", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@person", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid().ToString();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetAllRaisedCases";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new CWIPModel();
                    obj.cwip_status = row["status_name"].ToString();
                    obj.cwip_gid = Convert.ToInt32(row["cwip_gid"].ToString());
                    obj.cwip_asset_id = row["cwip_asset_id"].ToString();
                    obj.cwip_group_id = Convert.ToString(row["cwip_group_id"].ToString());
                    obj.cwip_capitalisation_date = row["cwip_capitalisation_date"].ToString();
                    obj.cwip_inward_date = row["cwip_inward_date"].ToString();
                    objModel.Add(obj);
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

        public CWIPModel GetCWIPViewDetails(int cwip_gid)
        {
            CWIPModel obj = new CWIPModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_cwip", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetDetailsToView";
                cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = cwip_gid.ToString();

                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new CWIPModel();
                    obj.cwip_status = row["status_name"].ToString();
                    obj.cwip_gid = Convert.ToInt32(row["cwip_gid"].ToString());
                    obj.cwip_asset_id = row["cwip_asset_id"].ToString();
                    obj.cwip_group_id = Convert.ToString(row["cwip_group_id"].ToString());
                    obj.cwip_inward_date = convertoDate(row["cwip_inward_date"].ToString());
                    obj.cwip_description = Convert.ToString(row["cwip_description"].ToString());
                    obj.cwip_asset_descp = Convert.ToString(row["asset_description"].ToString());
                    obj.cwip_supplier_name = Convert.ToString(row["supplierheader_name"].ToString());
                    obj.cwip_capitalisation_date = convertoDate(row["cwip_capitalisation_date"].ToString());
                    obj.cwip_serial_number = Convert.ToString(row["cwip_serial_no"].ToString());
                    obj.cwip_manft_name = Convert.ToString(row["cwip_manft_name"].ToString());
                    obj.cwip_depr_rate = Convert.ToDecimal(row["cwip_depr_rate"].ToString());
                    obj.cwip_asset_value = Convert.ToDecimal(row["cwip_asset_value"].ToString());
                    obj.cwip_cbf_number = Convert.ToString(row["cwip_cbf_number"].ToString());
                    obj.cwip_po_number = Convert.ToString(row["cwip_po_number"].ToString());
                    obj.cwip_ecf_number = Convert.ToString(row["cwip_ecf_number"].ToString());
                    obj.cwip_cc = Convert.ToString(row["cwip_cc"].ToString());
                    obj.cwip_bs = Convert.ToString(row["cwip_bs"].ToString());
                    obj.cwip_department = Convert.ToString(row["asset_classification"].ToString());
                    obj.cwip_asset_resglcode = Convert.ToString(row["asset_dep_reservglcode"].ToString());
                    obj.cwip_asset_glcode = Convert.ToString(row["asset_glcode"].ToString());
                    obj.cwip_asset_depglcode = Convert.ToString(row["asset_dep_glcode"].ToString());
                    obj.cwip_asset_cat = Convert.ToString(row["assetcategory_name"].ToString());
                    obj.cwip_branch_code = Convert.ToString(row["branch_code"].ToString());
                    obj.cwip_quantity = Convert.ToString(row["cwip_quantity"].ToString());
                    obj.cwip_asset_code = Convert.ToString(row["asset_code"].ToString());
                    obj.cwip_branch_start = convertoDate(Convert.ToString(row["branch_start_date"].ToString()));
                }
                return obj;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }
            finally
            {
                con.Close();
            }
        }
        public string SetCWIPDetails(CWIPModel obj)
        {
            string update = string.Empty;
            try
            {
                string[,] colval = new string[,]
                                          {                                         
                                            {"cwip_serial_no", obj.cwip_serial_number  },      
                                            {"cwip_manft_name", obj.cwip_manft_name  },
                                          };
                string[,] whcolval = new string[,]
                                          {                                         
                                            {"cwip_gid",obj.cwip_gid.ToString()},
                                          };
                update = objCommonIUD.UpdateCommon(colval, whcolval, "fa_trn_tcwip");
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            finally
            {
                con.Close();
            }
            return update;
        }
        public string Verify_details(string[] values)
        {

            string success = "SUCCESS";
            try
            {
                string id1 = values[0];
                if (id1 != string.Empty && id1 != "undefined ")
                    for (int length = (values.Length) - 1; length >= 0; length--)
                    {
                        string id2 = values[length];
                        if (id2 != string.Empty && id2 != "undefined ")
                            if (success == "SUCCESS")
                            {
                                CWIPModel obj1 = GetCWIPViewDetails(Convert.ToInt32(id1));
                                CWIPModel obj2 = GetCWIPViewDetails(Convert.ToInt32(id2));
                                if (obj1.cwip_capitalisation_date == obj2.cwip_capitalisation_date && obj1.cwip_branch_code == obj2.cwip_branch_code && obj1.cwip_asset_value == obj2.cwip_asset_value && obj1.cwip_asset_code == obj2.cwip_asset_code)
                                    success = "SUCCESS";
                                else if (string.IsNullOrEmpty(obj1.cwip_branch_start))
                                    success = "branch_date";
                                else
                                    success = "failed";
                            }
                    }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return success;

        }
        public string AppRejCapCWIP(string ids, string status)
        {
            string Result = string.Empty, dep = string.Empty, asset = string.Empty, cbf = string.Empty;

            try
            {
                //string[] values = ids.Split(',');
                //string id1 = values[0];
                //if (id1 != string.Empty && id1 != "undefined ")
                //    for (int length = (values.Length) - 1; length >= 0; length--)
                //    {
                //        string id2 = values[length];
                //        if (id2 != string.Empty && id2 != "undefined ")
                //            if (Result == "SUCCESS")
                //            {
                //                CWIPModel obj1 = GetCWIPViewDetails(Convert.ToInt32(id1));
                //                CWIPModel obj2 = GetCWIPViewDetails(Convert.ToInt32(id2));
                //                if (obj1.cwip_capitalisation_date == obj2.cwip_capitalisation_date && obj1.cwip_branch_code == obj2.cwip_branch_code && obj1.cwip_asset_value == obj2.cwip_asset_value && obj1.cwip_asset_code == obj2.cwip_asset_code)
                //                    Result = "SUCCESS";
                //                else if (string.IsNullOrEmpty(obj1.cwip_branch_start))
                //                    Result = "branch_date";
                //                else
                //                    Result = "failed";
                //            }
                //    }


                GetConnection();
                string[] idsArray = ids.Split(',');
                if (status == "APPROVED")
                {
                    cmd = new SqlCommand("pr_ifams_trn_cwip", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@date", SqlDbType.VarChar).Value = convertoDate(objCommonIUD.GetCurrentDate());
                    cmd.Parameters.Add("@person", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid().ToString();
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "get_grp_no";
                    cmd.ExecuteNonQuery();

                    if (idsArray.Length > 0)
                    {
                        cmd = new SqlCommand("pr_ifams_trn_cwip", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "gettranuploadgid");
                        string tran_upload_gid = Convert.ToString(cmd.ExecuteScalar());
                        for (int k = 0; k < idsArray.Length; k++)
                        {
                            GetConnection();
                            DataTable dt = new DataTable();
                            cmd = new SqlCommand("pr_ifams_trn_cwip", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = idsArray[k];
                            cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetDetailsToView";
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {

                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    string group_id = dt.Rows[i]["cwip_groupid"].ToString();
                                    string yn = "";
                                    string GL = string.Empty;
                                    //Muthu 07-12-2016
                                    DataTable dta = new DataTable();
                                    cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@assetsubgid", dt.Rows[i]["cwip_asset_gid"].ToString());
                                    cmd.Parameters.AddWithValue("@action", "SelectGLassetid");
                                    da = new SqlDataAdapter(cmd);
                                    da.Fill(dta);
                                    for (int j = 0; j < dta.Rows.Count; j++)
                                    {
                                        GL = dta.Rows[0]["asset_glcode"].ToString();
                                    }

                                    #region/*5K Validation Remove for Client Request 31-10-2018 */
                                    /*if (Convert.ToInt32(dt.Rows[i]["cwip_asset_value"]) > 5000 && GL != "121010002")
                                    {
                                        yn = "Y";
                                    }
                                    else
                                    {
                                        yn = "N";
                                    }*/
                                    #endregion
                                    string capdate = "";
                                    string LeaseStartDate = "";
                                    string LeaseEndDate = "";
                                    if (dt.Rows[i]["asset_dep_type"].ToString() == "LPM")
                                    {
                                        LeaseStartDate = dt.Rows[i]["branch_lease_start_date"].ToString();
                                        LeaseEndDate = dt.Rows[i]["branch_lease_end_date"].ToString();

                                        if (Convert.ToDateTime(dt.Rows[i]["cwip_capitalisation_date"].ToString()) < Convert.ToDateTime(dt.Rows[i]["branch_lease_start_date"].ToString()))
                                        {
                                            capdate = objCmnFunctions.convertoDateTimeString(LeaseStartDate);
                                        }
                                        else
                                        {
                                            capdate = objCmnFunctions.convertoDateTimeString(dt.Rows[i]["cwip_capitalisation_date"].ToString());
                                        }
                                    }
                                    else
                                    {
                                        //  if (Convert.ToDateTime(dt.Rows[i]["cwip_capitalisation_date"].ToString()) > Convert.ToDateTime(dt.Rows[i]["branch_start_date"].ToString()))
                                        if (Convert.ToDateTime(dt.Rows[i]["cwip_capitalisation_date"].ToString()) < Convert.ToDateTime(dt.Rows[i]["branch_start_date"].ToString()))
                                        {
                                            capdate = objCmnFunctions.convertoDateTimeString(dt.Rows[i]["branch_start_date"].ToString());
                                        }
                                        else
                                        {
                                            capdate = objCmnFunctions.convertoDateTimeString(dt.Rows[i]["cwip_capitalisation_date"].ToString());
                                        }
                                    }

                                    string ASSET_ID = objCmnFunctions.generateAssetid(dt.Rows[i]["branch_code"].ToString(), dt.Rows[i]["asset_code"].ToString());
                                    string[,] incodeinst = new string[,]
                                            {
                                                {"assetdetails_assetdet_id",ASSET_ID}                    
                                                ,{"assetdetails_qty","1"} 
                                                ,{"assetdetails_asset_serialno", dt.Rows[i]["cwip_serial_no"].ToString()}
                                                ,{"assetdetails_asset_code",dt.Rows[i]["asset_gid"].ToString()}
                                                ,{"assetdetails_asset_value",dt.Rows[i]["cwip_asset_value"].ToString()}
                                                ,{"assetdetails_asset_description",dt.Rows[i]["cwip_description"].ToString()}
                                                ,{"assetdetails_cap_date",capdate}
                                                ,{"assetdetails_po_number",string.Empty}
                                                ,{"assetdetails_tfr_status","N"}   
                                                ,{"assetdetails_active_status","Y"}
                                                ,{"assetdetails_sale_status","N"}    
                                                // ,{"assetdetails_not_5kcase",yn}
                                                ,{"assetdetails_not_5kcase","Y"}
                                                ,{"assetdetails_trf_value",dt.Rows[i]["cwip_asset_value"].ToString()} 
                                                ,{"assetdetails_asset_owner","F"}
                                                ,{"assetdetails_trn_date",capdate}
                                                ,{"assetdetails_impair_asset","N"}
                                                ,{"assetdetails_assetcode_changestatus","N"}
                                                ,{"assetdetails_isremoved","N"}
                                                ,{"assetdetails_dep_rate","0"}                                    
                                                ,{"assetdetails_branch_gid",dt.Rows[i]["branch_gid"].ToString()}
                                                ,{"assetdetails_addate",convertoDate (DateTime.Today.ToShortDateString())}
                                                ,{"assetdetails_insert_by",objCmnFunctions.GetLoginUserGid().ToString()}
                                                ,{"assetdetails_insert_date",convertoDate (DateTime.Today.ToShortDateString())}    
                                                ,{"assetdetails_takenby","14"}     
                                                ,{"assetdetails_assetid_source","1"}
                                                ,{"assetdetails_invoice_gid",""}
                                                ,{"assetdetails_ecf_gid",string.Empty}
                                                ,{"assetdetails_entity_gid","1"}
                                                ,{"assetdetails_asset_groupid",group_id.ToString()}  //10-03-2016 Cwip group id
                                               // ,{"assetdetails_asset_groupid",dt.Rows[i]["cwip_group_id"].ToString()}
                                                ,{"assetdetails_assetcode_changedate",string.Empty}
                                                ,{"assetdetails_assetcode_changeid",string.Empty}
                                                ,{"assetdetails_barcode",string.Empty}
                                                ,{"assetdetails_capnew_date",string.Empty}
                                                ,{"assetdetails_capold_date",string.Empty}
                                                ,{"assetdetails_cbf_gid",string.Empty}
                                                ,{"assetdetails_cbfnum",dt.Rows[i]["cwip_cbf_number"].ToString()}
                                                ,{"assetdetails_dep_onhold","N"}
                                                ,{"assetdetails_ecfnum",dt.Rows[i]["cwip_ecf_number"].ToString()}
                                                ,{"assetdetails_imp_date",string.Empty}
                                                ,{"assetdetails_inwdetailgid",string.Empty}
                                                ,{"assetdetails_inwheadergid",string.Empty}
                                               // ,{"assetdetails_lease_enddate",LeaseEndDate}
                                                ,{"assetdetails_lease_enddate", objCmnFunctions.convertoDateTimeString(string.IsNullOrEmpty(LeaseEndDate.ToString()) ? "01-01-1900" : LeaseEndDate)}
                                               // ,{"assetdetails_lease_startdate",LeaseStartDate}
                                                ,{"assetdetails_lease_startdate", objCmnFunctions.convertoDateTimeString(string.IsNullOrEmpty(LeaseStartDate.ToString()) ? "01-01-1900" : LeaseStartDate)}
                                                ,{"assetdetails_physical_assetid","0"}
                                                ,{"assetdetails_physical_idrelease",string.Empty}
                                                ,{"assetdetails_ponum",dt.Rows[i]["cwip_po_number"].ToString()}
                                                ,{"assetdetails_recon_reference",string.Empty}
                                                ,{"assetdetails_reduced_value","0"}
                                                ,{"assetdetails_sale_date",string.Empty}
                                                ,{"assetdetails_sale_id",string.Empty}
                                                ,{"assetdetails_spoke_asset","N"}
                                                ,{"assetdetails_status",string.Empty}
                                                ,{"assetdetails_tfr_date",string.Empty}
                                                ,{"assetdetails_tfr_id",string.Empty}
                                                ,{"assetdetails_trf_from",string.Empty}
                                                ,{"assetdetails_trf_reason",string.Empty}
                                                ,{"assetdetails_trf_to",string.Empty}
                                                ,{"assetdetails_update_by",string.Empty}
                                                ,{"assetdetails_update_date",string.Empty}
                                                ,{"assetdetails_upld_ref",string.Empty}
                                                ,{"assetdetails_upld_status","N"}
                                                ,{"assetdetails_wrt_date",string.Empty}
                                                ,{"assetdetails_wrt_id",string.Empty}
                                                ,{"assetdetails_wrt_status","N"}
                                                ,{"assetdetails_vendorname",dt.Rows[i]["supplierheader_name"].ToString()}
                                                ,{"assetdetails_inwarddate",convertoDate(dt.Rows[i]["cwip_inward_date"].ToString())}
                                              
                                            };
                                    Result = objCommonIUD.InsertCommon(incodeinst, "fa_trn_tassetdetails");

                                    string[,] incode = new string[,]
                                            {
                                            {"h_asst_groupid_no",group_id }
                                            ,{"h_asst_code",dt.Rows[i]["asset_code"].ToString()}
                                            ,{"h_asst_owner","F"}
                                            ,{"h_asst_desc",dt.Rows[i]["asset_description"].ToString()}                   
                                            ,{"h_emp_code",objCmnFunctions.GetLoginUserGid().ToString() }   
                                            ,{"h_capt_date",objCmnFunctions.convertoDateTimeString(dt.Rows[i]["cwip_capitalisation_date"].ToString())}
                                            ,{"h_purchase_date",objCmnFunctions.convertoDateTimeString(dt.Rows[i]["cwip_inward_date"].ToString())}
                                            ,{"h_purc_cost",dt.Rows[i]["cwip_asset_value"].ToString()}
                                            ,{"h_sales_tax_amt","0"}
                                            ,{"h_org_capt_amt",dt.Rows[i]["cwip_asset_value"].ToString()}
                                            ,{"h_cur_capt_amt",dt.Rows[i]["cwip_asset_value"].ToString()}
                                            ,{"h_org_qty_old","1"}
                                            ,{"h_org_qty","1"}
                                            ,{"h_cur_qty","1"}
                                            ,{"h_model_no",""}
                                            ,{"h_supplier_code",dt.Rows[i]["supplierheader_suppliercode"].ToString()}
                                            ,{"h_supplier_name",dt.Rows[i]["supplierheader_name"].ToString()}
                                            ,{"h_serial_no",dt.Rows[i]["cwip_serial_no"].ToString()}
                                            ,{"h_manft_name",dt.Rows[i]["cwip_manft_name"].ToString()}
                                            ,{"h_ecfno",dt.Rows[i]["cwip_ecf_number"].ToString()}
                                            ,{"h_cbf_no",dt.Rows[i]["cwip_cbf_number"].ToString()}
                                            ,{"h_status","Y"}
                                            ,{"h_location_code",dt.Rows[i]["branch_code"].ToString()}
                                            ,{"h_po_mast_po_no",dt.Rows[i]["cwip_po_number"].ToString()}                   
                                            ,{"h_glcode",dt.Rows[i]["asset_glcode"].ToString()}
                                            ,{"h_depr_glcode",dt.Rows[i]["asset_dep_glcode"].ToString()}
                                            ,{"h_depr_type",dt.Rows[i]["asset_dep_type"].ToString()}  
                                            ,{"h_narration",dt.Rows[i]["cwip_remark"].ToString()}  
                                            ,{"h_isremoved","N"}
                                            ,{"h_insert_by",objCmnFunctions.GetLoginUserGid().ToString()}
                                            ,{"h_insert_date",DateTime.Now.ToString("dd/MMM/yyyy")}
                                            };
                                    Result = objCommonIUD.InsertCommon(incode, "fa_trn_th");

                                    cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "assetdetails";
                                    cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = ASSET_ID;
                                    string ASSET_GID = Convert.ToString(cmd.ExecuteScalar());
                                    cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "UploadDetails";
                                    cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = ASSET_GID;
                                    da = new SqlDataAdapter(cmd);
                                    string glCode = "";
                                    string branch = "";
                                    string cat = "";
                                    string subcat = "";
                                    string asset_id = "";
                                    string BS = dt.Rows[i]["cwip_bs"].ToString();
                                    string CC = dt.Rows[i]["cwip_cc"].ToString();
                                    DataTable UPLOADDATATBL = new DataTable();
                                    da.Fill(UPLOADDATATBL);
                                    if (UPLOADDATATBL.Rows.Count > 0)
                                        foreach (DataRow rowdep in UPLOADDATATBL.Rows)
                                        {
                                            glCode = UPLOADDATATBL.Rows[0]["asset_glcode"].ToString();
                                            branch = UPLOADDATATBL.Rows[0]["branch_code"].ToString();
                                            cat = UPLOADDATATBL.Rows[0]["assetcategory_name"].ToString();
                                            if (cat.Length > 16)
                                                cat = cat.Substring(0, 16);
                                            subcat = UPLOADDATATBL.Rows[0]["asset_code"].ToString();
                                            asset_id = UPLOADDATATBL.Rows[0]["assetdetails_assetdet_id"].ToString();
                                        }
                                    string[,] glCoulmsD = new string[,]
                                            {                                
                                                {"tran_date",convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_val_date",convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_payment_date",convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_gl_no",dt.Rows[i]["asset_glcode"].ToString()},
                                                {"tran_desc",ASSET_ID.ToString() + " | CAPITALIZATION OF ASSET | " + dt.Rows[i]["cwip_ecf_number"].ToString() },
                                                {"tran_amount",dt.Rows[i]["cwip_asset_value"].ToString()},
                                                {"tran_acc_mode","D".ToString()},
                                                {"tran_mult","-1"},  
                                                {"tran_fc_code",BS},
                                                {"tran_cc_code",CC},                                
                                                {"tran_ou_code",branch},
                                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                                {"tran_ref_flag",GetRef("COA")},
                                                {"tran_ref_gid",ASSET_GID.ToString()},
                                                //{"tran_upload_gid",tran_upload_gid.ToString()},
                                                {"tran_upload_gid","0".ToString()},
                                                {"tran_isremoved","N"},
                                                {"tran_main_cat",cat},
                                                {"tran_sub_cat",subcat},
                                                {"tran_expense_category","1"},
                                                {"tran_primary_branch_code",branch},
                                                {"tran_secondary_branch_code",""},
                                                {"tran_related_account",""},
                                                {"tran_invoice_no",""},                               
                                                {"tran_expense_month",convertoDate (DateTime.Today.ToShortDateString())},
                                                //  {"tran_maker_id",dt.Rows[i]["cap_makerid"].ToString()},
                                                {"tran_checker_id",dt.Rows[i]["cwip_makerid"].ToString()},
                                                {"tran_emp_id",dt.Rows[i]["cwip_makerid"].ToString()},
                                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                                            };
                                    string insertforglD = objCommonIUD.InsertCommon(glCoulmsD, "fa_trn_ttran");

                                    string[,] glCoulmsC = new string[,]
                                        {                                
                                                {"tran_date",convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_val_date",convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_payment_date",convertoDate (DateTime.Today.ToShortDateString())},
                                                {"tran_gl_no",dt.Rows[i]["assetcategory_assset_clearing"].ToString()},
                                                {"tran_desc",ASSET_ID.ToString() + " | CAPITALIZATION OF ASSET | " + dt.Rows[i]["cwip_ecf_number"].ToString() },
                                                {"tran_amount",dt.Rows[i]["cwip_asset_value"].ToString()},
                                                {"tran_acc_mode","C".ToString()},
                                                {"tran_mult","1"},  
                                                {"tran_fc_code",BS},
                                                {"tran_cc_code",CC},                                
                                                {"tran_ou_code",branch},
                                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                                {"tran_ref_flag",GetRef("COA")},
                                             //   {"tran_ref_gid",dt.Rows[i]["cwip_assetdetails_gid"].ToString()},
                                                {"tran_ref_gid",ASSET_GID.ToString()},
                                              //  {"tran_upload_gid",tran_upload_gid.ToString()},
                                                {"tran_upload_gid","0".ToString()},
                                                {"tran_isremoved","N"},
                                                {"tran_main_cat",cat},
                                                {"tran_sub_cat",subcat},
                                                {"tran_expense_category","1"},
                                                {"tran_primary_branch_code",branch},
                                                {"tran_secondary_branch_code",""},
                                                {"tran_related_account",""},
                                                {"tran_invoice_no",""},                               
                                                {"tran_expense_month",convertoDate (DateTime.Today.ToShortDateString())},
                                                //  {"tran_maker_id",dt.Rows[i]["cap_makerid"].ToString()},
                                                {"tran_checker_id",dt.Rows[i]["cwip_makerid"].ToString()},
                                                {"tran_emp_id",dt.Rows[i]["cwip_makerid"].ToString()},
                                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                                        };
                                    string insertforglC = objCommonIUD.InsertCommon(glCoulmsC, "fa_trn_ttran");

                                    string[,] cval = new string[,]
                                        {
                                            {"c_br_code","2"},
                                            {"c_ecf_no",dt.Rows[i]["cwip_ecf_number"].ToString()},
                                            {"c_in_no",""},
                                            {"c_inv_srlno",dt.Rows[i]["cwip_serial_no"].ToString()},
                                            {"c_ASST_ID_NO",group_id},
                                            {"c_COST_CODE",CC},
                                            {"c_BUSS_CODE",BS},
                                            {"c_RATIO","100.00"},
                                            {"c_CCBS",CC + BS},
                                            {"c_loc",branch},
                                            {"c_isremoved","N"},
                                            {"c_insert_by",objCmnFunctions.GetLoginUserGid().ToString()},
                                            {"c_insert_date",DateTime.Now.ToString("dd/MMM/yyyy")}
                                        };
                                    Result = objCommonIUD.InsertCommon(cval, "fa_trn_tc");

                                    string[,] codeloc = new string[,]
                                        {
                                            {"assetlocation_asset_id",ASSET_ID.ToString()},
                                            {"assetlocation_location_code",dt.Rows[i]["branch_legacy_code"].ToString().ToString()},
                                            {"assetlocation_location_ccfc",CC + BS.ToString()},
                                            {"assetlocation_location_fc",BS.ToString()},
                                            {"assetlocation_location_cc",CC.ToString()},
                                            {"assetlocation_ratio","100.00"},
                                            {"assetlocation_isremoved","N"},
                                            {"assetlocation_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                                            {"assetlocation_insert_date",Convert.ToString(convertoDate (DateTime.Today.ToShortDateString()))}
                                        };
                                    Result = objCommonIUD.InsertCommon(codeloc, "fa_trn_tassetlocation");

                                    string[,] uptax = new string[,]
                                        {
                                            {"cwip_asset_status","N"},
                                            {"cwip_status","3"},
                                            {"cwip_assetdetails_gid",ASSET_GID},
                                            {"cwip_checkerid",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                                            {"cwip_checker_date",Convert.ToString(convertoDate (DateTime.Today.ToShortDateString()))}
                                        };
                                    string[,] upwhrtax = new string[,]
                                        {
                                        {"cwip_gid",idsArray[k]}
                                        };
                                    Result = objCommonIUD.UpdateCommon(uptax, upwhrtax, "fa_trn_tcwip");
                                }

                            }

                        }


                        cmd = new SqlCommand("pr_ifams_trn_cwip", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "InsertGprGLEnrties");
                        cmd.Parameters.AddWithValue("@person", Convert.ToString(HttpContext.Current.Session["employee_code"]));
                        cmd.Parameters.AddWithValue("@AssetIds", tran_upload_gid);
                        cmd.Parameters.AddWithValue("@code", GetRef("COA"));
                        cmd.ExecuteNonQuery();
                    }
                }
                else if (status == "REJECTED")
                {
                    if (idsArray.Length > 0)
                    {
                        for (int k = 0; k < idsArray.Length; k++)
                        {
                            string[,] uptax = new string[,]
                                        {
                                        {"cwip_status","0"},
                                        {"cwip_branch_gid",string.Empty},
                                        {"cwip_makerid",string.Empty},
                                        {"cwip_maker_date",string.Empty}, 
                                        {"cwip_bs",string.Empty},
                                        {"cwip_cc",string.Empty},
                                        {"cwip_capitalisation_date",string.Empty},
                                        {"cwip_groupid",string.Empty}, 
                                        };
                            string[,] upwhrtax = new string[,]
                                        {
                                        {"cwip_gid",idsArray[k]}
                                        };
                            Result = objCommonIUD.UpdateCommon(uptax, upwhrtax, "fa_trn_tcwip");
                        }
                    }


                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Result;
        }



        public IEnumerable<CWIPModel> GetReportDetails(CWIPModel model)
        {
            List<CWIPModel> objModel = new List<CWIPModel>();
            try
            {

                CWIPModel obj = new CWIPModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_cwip", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@person", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid().ToString();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "report";
                cmd.Parameters.Add("@AssetIds", SqlDbType.VarChar).Value = model.cwip_asset_id;
                cmd.Parameters.Add("@inwardDate", SqlDbType.VarChar).Value = model.cwip_inward_date;
                cmd.Parameters.Add("@CbfNo", SqlDbType.VarChar).Value = model.cwip_cbf_number;
                cmd.Parameters.Add("@PoNo", SqlDbType.VarChar).Value = model.cwip_po_number;
                cmd.Parameters.Add("@Code", SqlDbType.VarChar).Value = model.cwip_asset_code;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                HttpContext.Current.Session["reportSession"] = dt;
                foreach (DataRow row in dt.Rows)
                {
                    obj = new CWIPModel();
                    obj.cwip_asset_id = row["ASSET_ID"].ToString();
                    obj.cwip_group_id = Convert.ToString(row["GROUP_ID"].ToString());
                    obj.cwip_sale_date = convertoDate(row["INWARD_DATE"].ToString());
                    // obj.cwip_transfer_date = convertoDate(row["TRANSFER_DATE"].ToString());
                    //  obj.cwip_inward_date = convertoDate(row["SALE_DATE"].ToString());
                    // obj.cwip_asset_descp = Convert.ToString(row["DESCRIPTION"].ToString());
                    obj.cwip_supplier_name = Convert.ToString(row["VENDOR_NAME"].ToString());
                    obj.cwip_capitalisation_date = convertoDate(row["CAPITALISATION_DATE"].ToString());
                    obj.cwip_asset_type = Convert.ToString(row["TANGIBLE_STATUS"].ToString());
                    obj.cwip_manft_name = Convert.ToString(row["AGEING"].ToString());
                    //  obj.cwip_depr_rate = Convert.ToDecimal(string.IsNullOrEmpty(row["DEPR_RATE"].ToString()) ? "0" : row["DEPR_RATE"].ToString());
                    obj.cwip_asset_value = Convert.ToDecimal(row["ASSET_VALUE"].ToString());
                    // obj.cwip_wd_value = Convert.ToDecimal(row["WD_VALUE"].ToString());
                    // obj.cwip_transfer_value = Convert.ToDecimal(row["TRANSFER_VALUE"].ToString());
                    obj.cwip_cbf_number = Convert.ToString(row["CBF_NUMBER"].ToString());
                    // obj.cwip_transfer_from = Convert.ToString(row["TRANSFE_FROM"].ToString());
                    obj.cwip_po_number = Convert.ToString(row["PO_NUMBER"].ToString());
                    obj.cwip_ecf_number = Convert.ToString(row["ACF_NUMBER"].ToString());
                    // obj.cwip_cc = Convert.ToString(row["ASSET_BSCC"].ToString());
                    // obj.cwip_bs = Convert.ToString(row["UPLOAD_BSCC"].ToString());
                    // obj.cwip_asset_status = Convert.ToString(row["ASSET_STATUS"].ToString());
                    obj.cwip_department = Convert.ToString(row["DEPARTMENT"].ToString());
                    // obj.cwip_asset_resglcode = Convert.ToString(row["DEPR_PV_GL"].ToString());
                    // obj.cwip_asset_glcode = Convert.ToString(row["GLCODE"].ToString());
                    //obj.cwip_asset_depglcode = Convert.ToString(row["DEPR_GL"].ToString());
                    obj.cwip_branch_code = Convert.ToString(row["BRANCH_CODE"].ToString());
                    obj.cwip_quantity = Convert.ToString(row["QUANTITY"].ToString());
                    // obj.cwip_dep_type = Convert.ToString(row["DEPR_TYPE"].ToString());
                    obj.cwip_asset_code = Convert.ToString(row["ASSET_CODE"].ToString());
                    //obj.cwip_branch_start = convertoDate(Convert.ToString(row["BRANCH_LAUNCH_DATE"].ToString()));
                    //obj.cwip_lea_start = convertoDate(Convert.ToString(row["LEASE_START_DATE"].ToString()));
                    //obj.cwip_lea_end = convertoDate(Convert.ToString(row["LEASE_END_DATE"].ToString()));
                    //obj.cwip_reference_date = convertoDate(Convert.ToString(row["REFERENCE_DATE"].ToString()));
                    objModel.Add(obj);
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

        public IEnumerable<CWIPModel> GetCapReportDetails(CWIPModel model)
        {
            List<CWIPModel> objModel = new List<CWIPModel>();
            try
            {

                CWIPModel obj = new CWIPModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_cwip", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@person", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid().ToString();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "cap_cwip_report";
                cmd.Parameters.Add("@AssetIds", SqlDbType.VarChar).Value = model.cwip_asset_id;
                cmd.Parameters.Add("@inwardDate", SqlDbType.VarChar).Value = model.cwip_inward_date;
                cmd.Parameters.Add("@CbfNo", SqlDbType.VarChar).Value = model.cwip_cbf_number;
                cmd.Parameters.Add("@PoNo", SqlDbType.VarChar).Value = model.cwip_po_number;
                cmd.Parameters.Add("@Code", SqlDbType.VarChar).Value = model.cwip_asset_code;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                HttpContext.Current.Session["reportSessionCapCWIPreport"] = dt;
                foreach (DataRow row in dt.Rows)
                {
                    obj = new CWIPModel();
                    obj.cwip_asset_id = row["ASSET_ID"].ToString();
                    obj.cwip_group_id = Convert.ToString(row["GROUP_ID"].ToString());
                    obj.cwip_sale_date = convertoDate(row["INWARD_DATE"].ToString());
                    // obj.cwip_transfer_date = convertoDate(row["TRANSFER_DATE"].ToString());
                    //  obj.cwip_inward_date = convertoDate(row["SALE_DATE"].ToString());
                    // obj.cwip_asset_descp = Convert.ToString(row["DESCRIPTION"].ToString());
                    obj.cwip_supplier_name = Convert.ToString(row["VENDOR_NAME"].ToString());
                    obj.cwip_capitalisation_date = convertoDate(row["CAPITALISATION_DATE"].ToString());
                    obj.cwip_asset_type = Convert.ToString(row["TANGIBLE_STATUS"].ToString());
                    obj.cwip_manft_name = Convert.ToString(row["AGEING"].ToString());
                    //  obj.cwip_depr_rate = Convert.ToDecimal(string.IsNullOrEmpty(row["DEPR_RATE"].ToString()) ? "0" : row["DEPR_RATE"].ToString());
                    obj.cwip_asset_value = Convert.ToDecimal(row["ASSET_VALUE"].ToString());
                    // obj.cwip_wd_value = Convert.ToDecimal(row["WD_VALUE"].ToString());
                    // obj.cwip_transfer_value = Convert.ToDecimal(row["TRANSFER_VALUE"].ToString());
                    obj.cwip_cbf_number = Convert.ToString(row["CBF_NUMBER"].ToString());
                    // obj.cwip_transfer_from = Convert.ToString(row["TRANSFE_FROM"].ToString());
                    obj.cwip_po_number = Convert.ToString(row["PO_NUMBER"].ToString());
                    obj.cwip_ecf_number = Convert.ToString(row["ACF_NUMBER"].ToString());
                    // obj.cwip_cc = Convert.ToString(row["ASSET_BSCC"].ToString());
                    // obj.cwip_bs = Convert.ToString(row["UPLOAD_BSCC"].ToString());
                    // obj.cwip_asset_status = Convert.ToString(row["ASSET_STATUS"].ToString());
                    obj.cwip_department = Convert.ToString(row["DEPARTMENT"].ToString());
                    // obj.cwip_asset_resglcode = Convert.ToString(row["DEPR_PV_GL"].ToString());
                    // obj.cwip_asset_glcode = Convert.ToString(row["GLCODE"].ToString());
                    //obj.cwip_asset_depglcode = Convert.ToString(row["DEPR_GL"].ToString());
                    obj.cwip_branch_code = Convert.ToString(row["BRANCH_CODE"].ToString());
                    obj.cwip_quantity = Convert.ToString(row["QUANTITY"].ToString());
                    // obj.cwip_dep_type = Convert.ToString(row["DEPR_TYPE"].ToString());
                    obj.cwip_asset_code = Convert.ToString(row["ASSET_CODE"].ToString());
                    //obj.cwip_branch_start = convertoDate(Convert.ToString(row["BRANCH_LAUNCH_DATE"].ToString()));
                    //obj.cwip_lea_start = convertoDate(Convert.ToString(row["LEASE_START_DATE"].ToString()));
                    //obj.cwip_lea_end = convertoDate(Convert.ToString(row["LEASE_END_DATE"].ToString()));
                    //obj.cwip_reference_date = convertoDate(Convert.ToString(row["REFERENCE_DATE"].ToString()));
                    objModel.Add(obj);
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

        public DataTable GetReportDetailsreport(CWIPModel model)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_cwip", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@person", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid().ToString();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "report";
                cmd.Parameters.Add("@AssetIds", SqlDbType.VarChar).Value = model.cwip_asset_id;
                cmd.Parameters.Add("@inwardDate", SqlDbType.VarChar).Value = model.cwip_inward_date;
                cmd.Parameters.Add("@CbfNo", SqlDbType.VarChar).Value = model.cwip_cbf_number;
                cmd.Parameters.Add("@PoNo", SqlDbType.VarChar).Value = model.cwip_po_number;
                cmd.Parameters.Add("@Code", SqlDbType.VarChar).Value = model.cwip_asset_code;
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

        public decimal getRetificationAmount(string assetId, string depdate)
        {
            GetConnection();
            decimal rectificationamt = 0;
            try
            {
                DataTable dtwdv = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "WDV");
                cmd.Parameters.AddWithValue("@assetid", assetId);
                da = new SqlDataAdapter(cmd);
                da.Fill(dtwdv);

                DateTime dtBranchLaunch = Convert.ToDateTime("01-01-1900");
                DateTime soacaptilisationda = Convert.ToDateTime("01-01-1900");
                decimal AssetValue = 0;
                string sAssetCode = string.Empty;
                string cNot5000Case = string.Empty;
                DateTime dtLeaseStart = Convert.ToDateTime("01-01-1900");
                DateTime dtLeaseEnd = Convert.ToDateTime("01-01-1900");
                string sDepType = string.Empty;
                decimal dDepRate = 0;
                string sAssetGroup = string.Empty;
                //  DateTime dtTillDate = DateTime.Now;
                DateTime dtTillDate = Convert.ToDateTime(depdate.ToString());
                string sPONumber = string.Empty;
                string sFICCRef = string.Empty;
                string sAsset_GroupId = string.Empty;
                decimal dTransfer_value = 0;
                string CanDepreciateFully = string.Empty;
                Int32 dDepDevRate = 0;
                string sBranch1 = string.Empty;
                string sBranch2 = string.Empty;
                foreach (DataRow row1 in dtwdv.Rows)
                {
                    dtBranchLaunch = Convert.ToDateTime(string.IsNullOrEmpty(row1["branch_start_date"].ToString()) ? "0" : row1["branch_start_date"]);
                    soacaptilisationda = Convert.ToDateTime(string.IsNullOrEmpty(row1["assetdetails_cap_date"].ToString()) ? "01-01-1900" : row1["assetdetails_cap_date"]);
                    AssetValue = Convert.ToDecimal(string.IsNullOrEmpty(row1["assetdetails_asset_value"].ToString()) ? "0" : row1["assetdetails_asset_value"]);
                    sAssetCode = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_asset_code"].ToString()) ? "0" : row1["assetdetails_asset_code"]);
                    cNot5000Case = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_not_5kcase"].ToString()) ? "0" : row1["assetdetails_not_5kcase"]);
                    dtLeaseStart = Convert.ToDateTime(string.IsNullOrEmpty(row1["assetdetails_lease_startdate"].ToString()) ? "01-01-1900" : row1["assetdetails_lease_startdate"]);
                    dtLeaseEnd = Convert.ToDateTime(string.IsNullOrEmpty(row1["assetdetails_lease_enddate"].ToString()) ? "01-01-1900" : row1["assetdetails_lease_enddate"]);
                    sDepType = Convert.ToString(string.IsNullOrEmpty(row1["asset_dep_type"].ToString()) ? "0" : row1["asset_dep_type"]);
                    dDepRate = Convert.ToDecimal(string.IsNullOrEmpty(row1["asset_dep_rate"].ToString()) ? "0" : row1["asset_dep_rate"]);
                    sAssetGroup = Convert.ToString(string.IsNullOrEmpty(row1["h_asst_groupid_no"].ToString()) ? "0" : row1["h_asst_groupid_no"]);
                    sPONumber = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_po_number"].ToString()) ? "0" : row1["assetdetails_po_number"]);
                    sFICCRef = Convert.ToString(string.IsNullOrEmpty(row1["h_ficrefnumber"].ToString()) ? "0" : row1["h_ficrefnumber"]);
                    sAsset_GroupId = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_asset_groupid"].ToString()) ? "0" : row1["assetdetails_asset_groupid"]);
                    dTransfer_value = Convert.ToDecimal(string.IsNullOrEmpty(row1["assetdetails_trf_value"].ToString()) ? "0" : row1["assetdetails_trf_value"]);
                }

                DataTable dtdep = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "Totdep");
                cmd.Parameters.AddWithValue("@assetgid", assetId);
                decimal sumassetdep = Convert.ToDecimal(cmd.ExecuteScalar());

                decimal depamt = Math.Round(GetTotalDep(soacaptilisationda, dtTillDate,
                                AssetValue, sAssetCode,
                             cNot5000Case, sBranch1, sBranch2,
                             dtBranchLaunch, dtLeaseStart, dtLeaseEnd,
                             sDepType, dDepRate,
                             sAssetGroup,
                             sPONumber,
                             sFICCRef,
                             sAsset_GroupId,
                             dTransfer_value,
                             CanDepreciateFully,
                             dDepDevRate), 2);
                rectificationamt = depamt - sumassetdep;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return rectificationamt;

        }

        public int GethsnDetails(string hsncode)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@fccc", SqlDbType.VarChar).Value = hsncode;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "hsnid";
                int result = (int)cmd.ExecuteScalar();
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

        public void Savetransfergstdetails(string id)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_saletransfergst", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ptoanumber", SqlDbType.VarChar).Value = id.ToString();
                cmd.Parameters.Add("@paction", SqlDbType.VarChar).Value = "SAVE";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            finally
            {
                con.Close();
            }
        }

        public List<TransferMakerModel> GetGstDetails(string toanumber)
        {
            List<TransferMakerModel> objGstDet = new List<TransferMakerModel>();
            try
            {
                TransferMakerModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                string fstEmp = string.Empty;
                string status = string.Empty;
                cmd = new SqlCommand("pr_ifams_trn_saletransfergst", con);
                cmd.Parameters.AddWithValue("@ptoanumber", toanumber);
                cmd.Parameters.AddWithValue("@paction", "GET");
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objModel = new TransferMakerModel();
                        objModel.hsn_code = Convert.ToString(dt.Rows[i]["hsn_code"].ToString());
                        objModel.hsn_description = Convert.ToString(dt.Rows[i]["hsn_description"].ToString());
                        objModel.taxsubtype = Convert.ToString(dt.Rows[i]["taxsubtype"].ToString());
                        objModel.taxrate = Convert.ToDecimal(dt.Rows[i]["taxrate"].ToString());
                        objModel.taxamount = Convert.ToDecimal(dt.Rows[i]["amount"].ToString());
                        objGstDet.Add(objModel);
                        //objModel.gstlist.Add(objModel);
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
            return objGstDet;
        }

        public List<TransferInvoice> getApprovedtoaDetails()
        {
            TransferInvoice obj = new TransferInvoice();
            GetConnection();
            List<TransferInvoice> objlist = new List<TransferInvoice>();
            try
            {
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_traferinvoice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@querytype", "GETDETAIL");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow i in dt.Rows)
                    {
                        obj = new TransferInvoice();
                        obj._toa_no = i["toaheader_toa_number"].ToString();
                        obj._toa_gid = Convert.ToInt32(i["toaheader_gid"].ToString());
                        obj.recordcount = Convert.ToInt32(i["recordcount"].ToString());
                        obj._toa_date = i["toa_date"].ToString();
                        obj.toastatus = i["status"].ToString();
                        objlist.Add(obj);
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
            return objlist;
        }



        public List<TransferInvoice> gettoaDetails(string id)
        {
            TransferInvoice obj = new TransferInvoice();
            List<TransferInvoice> objlist = new List<TransferInvoice>();
            try
            {
                GetConnection();
                DataTable datatable = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_traferinvoice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@querytype", "GETDETAILfromID");
                cmd.Parameters.AddWithValue("@data", id);
                da = new SqlDataAdapter(cmd);
                da.Fill(datatable);
                if (datatable.Rows.Count > 0)
                {
                    obj._tbranchtoname = datatable.Rows[0]["branch_name"].ToString();
                    obj._tbranchToaddress = datatable.Rows[0]["branch_to"].ToString();
                    //obj._customer_name = datatable.Rows[0]["customer_name"].ToString();
                    //obj._customer_address = datatable.Rows[0]["customer_address"].ToString();
                    obj._tbranchTogstin = datatable.Rows[0]["branchto_gstin"].ToString();
                    obj._state_code = datatable.Rows[0]["state_code"].ToString();
                    obj._state_name = datatable.Rows[0]["state_name"].ToString();
                    obj._invoice_no = GetInvoiceSequence();
                }
                string[,] colval = new string[,]
                                          {                                         
                                            {"toaheader_inv_no", obj._invoice_no  },                                            
                                          };
                string[,] whcolval = new string[,]
                                          {                                         
                                            {"toaheader_gid",id},
                                          };
                string update = objCommonIUD.UpdateCommon(colval, whcolval, "fa_trn_ttoaheader");
                objlist.Add(obj);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return objlist;
        }

        public List<TransferInvoice> getApprovedtoa(string id)
        {
            TransferInvoice obj = new TransferInvoice();
            List<TransferInvoice> objlist = new List<TransferInvoice>();
            DataTable detailstable = new DataTable();
            try
            {
                cmd = new SqlCommand("pr_ifams_trn_traferinvoice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@querytype", "GETtoaDETAILfromID");
                cmd.Parameters.AddWithValue("@data", id);
                da = new SqlDataAdapter(cmd);
                da.Fill(detailstable);
                if (detailstable.Rows.Count > 0)
                {

                    foreach (DataRow i in detailstable.Rows)
                    {
                        TransferInvoice obj1 = new TransferInvoice();
                        obj1._vat_amt = string.Empty;//i["soadetail_vat_value"].ToString();
                        if (obj1._vat_amt == string.Empty) obj1._vat_amt = "0";
                        obj1._toa_amt = i["toadetail_toa_amount"].ToString();
                        obj1._particulars = i["asset_description"].ToString();
                        obj1._vatpercentage = "0";//i["soaheader_vat_percentage"].ToString();
                        obj1._code = i["asset_code"].ToString();
                        obj1.total = Convert.ToDecimal(i["total"].ToString());
                        obj1._qty = i["qty"].ToString();
                        objlist.Add(obj1);
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
            return objlist;
        }


        public List<TransferInvoice> getFICCdetailstoa(string id)
        {
            //SaleInvoice obj = new SaleInvoice();
            List<TransferInvoice> objFICClist = new List<TransferInvoice>();
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand("pr_ifams_trn_traferinvoice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@querytype", "GETFICCDetails");
                cmd.Parameters.AddWithValue("@data", id);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow F in dt.Rows)
                    {
                        TransferInvoice obj = new TransferInvoice();
                        obj._FiccGstin = F["branchFrom_gstin"].ToString();
                        obj._Ficc_Branchaddress = F["branch_From"].ToString();
                        obj._Ficc_Location = F["branch_location_name"].ToString();
                        obj._supplier_statename = F["state_name"].ToString();
                        objFICClist.Add(obj);
                    }
                }
                else
                {
                    TransferInvoice obj = new TransferInvoice();
                    obj._FiccGstin = "0".ToString();
                    objFICClist.Add(obj);
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
            return objFICClist;
        }


        public List<TransferInvoice> getTaxdetailstoa(string id)
        {
            List<TransferInvoice> objtax = new List<TransferInvoice>();
            DataTable dt = new DataTable();
            try
            {
                cmd = new SqlCommand("pr_ifams_trn_traferinvoice", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@querytype", "GETTaxDetails");
                cmd.Parameters.AddWithValue("@data", id);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow tax in dt.Rows)
                    {
                        TransferInvoice obj = new TransferInvoice();
                        obj._hsn_code = Convert.ToString(tax["hsn_code"]);
                        obj._CGstamt = Convert.ToDecimal(tax["transfergstdtl_cgst_amount"]);
                        obj._CGstrat = Convert.ToDecimal(tax["transfergstdtl_cgst_rate"]);
                        obj._SGstamt = Convert.ToDecimal(tax["transfergstdtl_sgst_amount"]);
                        obj._SGstrat = Convert.ToDecimal(tax["transfergstdtl_sgst_rate"]);
                        obj._IGstamt = Convert.ToDecimal(tax["transfergstdtl_igst_amount"]);
                        obj._IGstrat = Convert.ToDecimal(tax["transfergstdtl_igst_rate"]);
                        obj._Cessamt = Convert.ToDecimal(tax["transfergstdtl_ccess_amount"]);
                        obj._Cessrat = Convert.ToDecimal(tax["transfergstdtl_ccess_rate"]);
                        obj._Ccessamt = Convert.ToDecimal(tax["transfergstdtl_cgcess_amount"]);
                        obj._Ccessrat = Convert.ToDecimal(tax["transfergstdtl_cgcess_rate"]);
                        obj._Scessamt = Convert.ToDecimal(tax["transfergstdtl_sgcess_amount"]);
                        obj._Scessrat = Convert.ToDecimal(tax["transfergstdtl_sgcess_rate"]);
                        obj._Icessamt = Convert.ToDecimal(tax["transfergstdtl_igcess_amount"]);
                        obj._Icessrat = Convert.ToDecimal(tax["transfergstdtl_igcess_rate"]);
                        obj._taxTotal = Convert.ToDecimal(tax["Totaltaxamount"]);
                        obj._taxRateTotal = Convert.ToDecimal(tax["transfergstdtl_igst_rate"]);
                        obj._cessTotal = Convert.ToDecimal(tax["transfergstdtl_igcess_amount"]);
                        obj._totalCessrate = Convert.ToDecimal(tax["transfergstdtl_igcess_rate"]);
                        obj.tot_taxableamount = Convert.ToDecimal(tax["taxableamount"]);
                        objtax.Add(obj);
                    }
                }
                else
                {
                    TransferInvoice obj = new TransferInvoice();
                    obj._FiccGstin = "0".ToString();
                    objtax.Add(obj);
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
            return objtax;
        }


        //For Dashboard Link Get Inprocess data
        public IEnumerable<TransferMakerModel> GetTrfDtlInprocessSummary(string id)
        {
            List<TransferMakerModel> objModel = new List<TransferMakerModel>();
            try
            {

                TransferMakerModel obj = new TransferMakerModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_get_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = id;
                //cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "SELECTINPROCESS";

                cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = Convert.ToString(objCmnFunctions.GetLoginUserGid().ToString());
                cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = string.IsNullOrEmpty(id) ? null : id.ToString();
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "SELECTINPROCESS";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new TransferMakerModel();
                    obj._tfr_status = "INPROCESS";
                    obj._gid = Convert.ToInt32(row["toaheader_gid"].ToString());
                    obj._toa_number = row["toaheader_toa_number"].ToString();
                    obj._tfr_date = Convert.ToString(row["toaheader_tfr_date"].ToString());
                    obj._no_records = Convert.ToInt64(row["toaheader_no_records"].ToString());
                    obj._upld_fname = row["toaheader_upld_fname"].ToString();
                    objModel.Add(obj);
                }
                return objModel;


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return objModel;
            }
            finally
            {
                con.Close();
            }
        }



        //Dashboard Inprocess link



        public List<WriteOffModel> GetWRFDtlInprocessSummary(int id)
        {
            List<WriteOffModel> wmm = new List<WriteOffModel>();
            try
            {

                WriteOffModel obj = new WriteOffModel();
                GetConnection();
                DataTable dt = new DataTable();

                cmd = new SqlCommand("pr_ifams_get_woaMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "SelectINPROCESS";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new WriteOffModel();
                    obj._woa_status = "INPROCESS";
                    obj._gid = Convert.ToInt32(row["woaheader_gid"].ToString());
                    obj._woa_number = row["woaheader_woa_number"].ToString();
                    obj._woa_date = Convert.ToString(row["woaheader_trans_date"].ToString());
                    obj._no_records = Convert.ToInt64(row["woaheader_nof_records"].ToString());
                    obj._upld_fname = row["woaheader_upld_fname"].ToString();
                    wmm.Add(obj);
                }
                return wmm;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return wmm;
            }
            finally
            {
                con.Close();
            }
        }

    }

}
