using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using IEM.Common;
using System.Data;
using System.Web.Providers.Entities;

namespace IEM.Areas.IFAMS.Models
{
    public class Ifams_Modelx : ifamsIRepositoryx
    {
        SqlConnection confa = new SqlConnection();
        DataModel objidm = new DataModel();
        SqlCommand cmdfa = new SqlCommand();
        SqlDataAdapter dafa = new SqlDataAdapter();
        CmnFunctions comuidfa = new CmnFunctions();
        CommonIUD commfa = new CommonIUD();
        ErrorLog objErrorLog = new ErrorLog();
        //DataSet dsfa;
        //DataTable dtfa;
        CmnFunctions objCmnFunctions = new CmnFunctions();
        CommonIUD objCommonIUD = new CommonIUD();

        private void GetConnection()
        {
            if (confa.State == ConnectionState.Closed)
            {
                confa.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                confa.Open();
            }

        }
        public List<Ifams_Propertyx.ImpairmentsModel> GetImpDetail(string StatusFlage)
        {
            List<Ifams_Propertyx.ImpairmentsModel> Model = new List<Ifams_Propertyx.ImpairmentsModel>();
            Ifams_Propertyx.ImpairmentsModel obj = new Ifams_Propertyx.ImpairmentsModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_imrheader", confa);
                cmd.Parameters.AddWithValue("@statu_flage", Convert.ToInt32(GetStatusforch(StatusFlage)));
                cmd.Parameters.AddWithValue("@Impair", "ImpairMaker");
                cmd.CommandType = CommandType.StoredProcedure;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new Ifams_Propertyx.ImpairmentsModel();
                    obj._gid = Convert.ToInt32(row["imrheader_gid"].ToString());
                    obj._imp_number = row["imrheader_imr_number"].ToString();
                    obj._no_records = Convert.ToInt32(row["imrheader_nof_records"].ToString());
                    obj._imp_date = row["timrheader_process_date"].ToString();
                    obj.impstatus = Convert.ToString(row["status_name"].ToString());
                    obj._upld_fname = row["attachment_filename"].ToString();
                    Model.Add(obj);
                }


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                GetOffConnection();

            }

            return Model;
        }


        public List<IFAMS.Models.Ifams_Propertyx.ImpairmentsModel> GetImpairStatus()
        {
            List<Ifams_Propertyx.ImpairmentsModel> Model = new List<Ifams_Propertyx.ImpairmentsModel>();
            Ifams_Propertyx.ImpairmentsModel obj = new Ifams_Propertyx.ImpairmentsModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_imrheader", confa);
                cmd.Parameters.AddWithValue("@Status", "Status");
                cmd.CommandType = CommandType.StoredProcedure;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new Ifams_Propertyx.ImpairmentsModel();
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

                GetOffConnection();

            }

            return Model;
        }

        public List<IFAMS.Models.Ifams_Propertyx.ImpairmentsModel> GetImpDetailChecker()
        {
            List<Ifams_Propertyx.ImpairmentsModel> Model = new List<Ifams_Propertyx.ImpairmentsModel>();
            Ifams_Propertyx.ImpairmentsModel obj = new Ifams_Propertyx.ImpairmentsModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_imrheaderch", confa);
                cmd.Parameters.AddWithValue("@statu_flage", Convert.ToInt32(GetStatusforch("WAITING FOR APPROVAL")));
                cmd.CommandType = CommandType.StoredProcedure;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new Ifams_Propertyx.ImpairmentsModel();
                    obj._gid = Convert.ToInt32(row["imrheader_gid"].ToString());
                    obj._imp_number = row["imrheader_imr_number"].ToString();
                    obj._no_records = Convert.ToInt32(row["imrheader_nof_records"].ToString());
                    obj._imp_date = row["timrheader_process_date"].ToString();
                    obj.impstatus = Convert.ToString(row["status_name"].ToString());
                    obj._upld_fname = row["attachment_filename"].ToString();
                    Model.Add(obj);
                }


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                GetOffConnection();

            }

            return Model;
        }

        public int GetStatusforch(string status)
        {
            DataTable dt = new DataTable();

            try
            {
                GetConnection();
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da;

                cmd = new SqlCommand("pr_ifams_trn_getdraftid", confa);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Draft", SqlDbType.VarChar).Value = status.ToString();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count != 0)
                {
                    return Convert.ToInt32(dt.Rows[0][0].ToString());
                }
                else
                {

                    return Convert.ToInt32(dt.Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                // GetOffConnection();
            }
            return Convert.ToInt32(dt.Rows[0][0].ToString());
        }




        public List<IFAMS.Models.Ifams_Propertyx.ImpairmentsModel> IOAuditTrial(int id)
        {


            string qdate = string.Empty;
            int qfrom = 0;
            string qactiondate = string.Empty;
            int qactionby = 0;
            string qstatus = string.Empty;
            string code = string.Empty;
            string name = string.Empty;
            string status = string.Empty;
            string Remarks = string.Empty;
            List<Ifams_Propertyx.ImpairmentsModel> Model = new List<Ifams_Propertyx.ImpairmentsModel>();
            Ifams_Propertyx.ImpairmentsModel obj = new Ifams_Propertyx.ImpairmentsModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_ioaaudittrial", confa);
                cmd.Parameters.AddWithValue("@Qdata", "Q");
                cmd.Parameters.AddWithValue("@queue_ref_flag", Convert.ToInt32(GetRef("IOA")));
                cmd.Parameters.AddWithValue("@queue_ref_gid", id);
                cmd.CommandType = CommandType.StoredProcedure;
                dt.Load(cmd.ExecuteReader());
                status = GetStatus(id);
                foreach (DataRow row in dt.Rows)
                {
                    qdate = convertoDate(row["queue_date"].ToString());
                    qfrom = Convert.ToInt32(row["queue_from"].ToString());
                    qactiondate = convertoDate(row["queue_action_date"].ToString());
                    Remarks = row["queue_action_remark"].ToString();

                    if (status.ToString() == "WAITING FOR APPROVAL")
                    {
                        obj.status = "WAITING FOR APPROVAL";
                    }
                    else
                    {
                        qactionby = Convert.ToInt32(row["queue_action_by"].ToString());
                        qstatus = Convert.ToString(row["queue_action_status"].ToString());
                    }
                }
                dt = GetEmp(qfrom);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new Ifams_Propertyx.ImpairmentsModel();
                    obj.remarks = string.Empty;
                    code = row["employee_code"].ToString();
                    name = row["employee_name"].ToString();
                    obj.empname_id = code + " - " + name;
                    obj.date = qdate;
                    obj.role = "IOA Maker";
                    if (status.ToString() == "DRAFT")
                    {
                        obj.status = "SUBMITED";
                        Model.Add(obj);
                    }
                    else if (status.ToString() == "WAITING FOR APPROVAL")
                    {
                        obj.status = "SUBMITED";
                        Model.Add(obj);
                    }
                    else if (status.ToString() == "APPROVED")
                    {

                        obj.status = "SUBMITED";
                        Model.Add(obj);
                    }
                    else if (status.ToString() == "REJECTED")
                    {
                        obj.status = "SUBMITED";
                        Model.Add(obj);
                    }
                }
                if (status.ToString() == "WAITING FOR APPROVAL")
                {
                    obj = new Ifams_Propertyx.ImpairmentsModel();
                    obj.role = "IOA Checker";
                    obj.status = "PENDING";
                    Model.Add(obj);
                }
                else
                {
                    dt = GetEmp(qactionby);
                    foreach (DataRow row in dt.Rows)
                    {
                        obj = new Ifams_Propertyx.ImpairmentsModel();
                        code = row["employee_code"].ToString();
                        name = row["employee_name"].ToString();
                        obj.remarks = Remarks.ToString();
                        obj.empname_id = code + " - " + name;
                        obj.date = qactiondate;
                        obj.role = "IOA Checker";
                        if (status.ToString() == "DRAFT")
                        {
                            obj.status = "PENDING";
                            Model.Add(obj);
                        }
                        else if (status.ToString() == "WAITING FOR APPROVAL")
                        {
                            obj.status = "WAITING FOR APPROVAL";
                            Model.Add(obj);
                        }
                        else if (status.ToString() == "APPROVED")
                        {
                            obj.status = "APPROVED";
                            Model.Add(obj);
                        }
                        else if (status.ToString() == "REJECTED")
                        {
                            obj.status = "REJECTED";
                            Model.Add(obj);
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
                GetOffConnection();
            }
            return Model;
        }

        public DataTable GetEmp(int EmpId)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_ioaaudittrial", confa);
                cmd.Parameters.AddWithValue("@Edata", "E");
                cmd.Parameters.AddWithValue("@EmpGid", EmpId);
                cmd.CommandType = CommandType.StoredProcedure;
                dt.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                GetOffConnection();
            }
            return dt;
        }

        public List<IFAMS.Models.Ifams_Propertyx.ImpairmentsModel> GetImpView(string id)
        {

            List<Ifams_Propertyx.ImpairmentsModel> Model = new List<Ifams_Propertyx.ImpairmentsModel>();
            Ifams_Propertyx.ImpairmentsModel obj = new Ifams_Propertyx.ImpairmentsModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_imrheaderview", confa);
                cmd.Parameters.AddWithValue("@imrheader_gid", Convert.ToInt32(id.ToString()));
                cmd.CommandType = CommandType.StoredProcedure;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new Ifams_Propertyx.ImpairmentsModel();
                    obj.impdet_gid = Convert.ToInt32(row["imrdetail_gid"].ToString());
                    obj.impheader_gid = Convert.ToInt32(row["imrheader_gid"].ToString());
                    obj._impasset_id = row["impasset_id"].ToString();
                    obj.imrdet_branch_id = row["imrdet_branch_id"].ToString();
                    obj.imrdet_department = row["asset_code"].ToString();
                    obj.imrheader_upld_date = row["asset_description"].ToString();
                    obj.imrheader_upld_fname = row["assetcategory_name"].ToString();
                    if (row["assetdetails_asset_value"].ToString() != string.Empty)
                    {
                        obj.imrdet_salevalue = Convert.ToDecimal(row["assetdetails_asset_value"].ToString());
                    }
                    obj.imrdet_date = convertoDate(row["imrdet_date"].ToString());
                    obj.impseqnumber = row["imrheader_imr_number"].ToString();
                   // obj._imp_recti_amt = objidm.getRetificationAmount(row["assetdetails_gid"].ToString());
                    obj._imp_recti_amt = objidm.getRetificationAmount(row["assetdetails_gid"].ToString(), obj.imrdet_date.ToString());
                    string seqno = Convert.ToString(row["imrheader_imr_number"].ToString());
                    HttpContext.Current.Session["ImpairNo"] = seqno;
                    Model.Add(obj);
                }
                //Session["ImpairNo"] = Model.impseqnumber.ToString();
                string seq = (string)HttpContext.Current.Session["ImpairNo"];
                GetOffConnection();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Model;
        }


        public string GetImapirStatusexcel(string assetdata, string valid, string action)
        {
            string result = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_saleasset", confa);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@salechkdata", SqlDbType.VarChar).Value = valid.ToString();
                cmd.Parameters.Add("@saleassetdata", SqlDbType.VarChar).Value = assetdata.ToString();
                cmd.Parameters.Add("@saleresult", SqlDbType.VarChar).Value = action;
                result = (string)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                GetOffConnection();
            }
            return result;
        }


        public string InsertImpair(DataTable assetdata, string filename)
        {
            string instcomm = string.Empty;
            try
            {
                Ifams_Propertyx.ImpairmentsModel impair;
                string impseqnumber = objCmnFunctions.GetSequenceNoFam("IOA");
                int LogUserId = objCmnFunctions.GetLoginUserGid();
                int NoOfRec = assetdata.Rows.Count;
                string Date = DateTime.Now.ToString();
                string IsRemoved = "N";
                DataTable dt = InsertImpairHeader(filename, Date, NoOfRec, impseqnumber, IsRemoved, LogUserId, "Y");
                int ImpairHeaderId = Convert.ToInt32(dt.Rows[0][0].ToString());
                impair = new Ifams_Propertyx.ImpairmentsModel();
                //for (int i = 0; i == 0; i++)
                //{
                DataTable dtA = new DataTable();
                dtA = (DataTable)assetdata;
                for (int j = 0; j < dtA.Rows.Count; j++)
                {
                    impair._impasset_id = dtA.Rows[j]["Asset Id"].ToString().Trim();
                    impair.imrdet_branch_id = dtA.Rows[j]["Branch"].ToString().Trim();
                    impair.imrdet_date = convertoDate(dtA.Rows[j]["Impairment Date"].ToString());
                    impair._impdet_inwno = dtA.Rows[j]["Inward No"].ToString();

                    ///////////////Lock Impairment
                    string Lockid = ImpairLock("Impairment in Process");
                    string[,] code = new string[,]
                    {                             
                             {"assetdetails_takenby",Lockid}                            
                    };
                    string[,] wher = new string[,]
                    { 
                           {"assetdetails_assetdet_id",dtA.Rows[j]["Asset Id"].ToString()} 
                    };
                    objCommonIUD.UpdateCommon(code, wher, "fa_trn_tassetdetails");

                    string[,] codes = new string[,]
                            {
                             {"imrheader_gid",ImpairHeaderId.ToString()},
                             {"impasset_id",impair._impasset_id.ToString()},
                             {"imrdet_branch_id ",impair.imrdet_branch_id.ToString()},                          
                             {"imrdet_date",convertoDate(impair.imrdet_date.ToString())},
                             {"impdetail_inw_no", impair._impdet_inwno.ToString()},
                             {"imrdetail_isremoved","N"}                          
                            };
                    string tblname = "fa_trn_timpdetail";
                    instcomm = objCommonIUD.InsertCommon(codes, tblname);
                }
                if (instcomm.ToString() == "success")
                {
                    for (int i = 0; i < dtA.Rows.Count; i++)
                    {
                        string[,] codes = new string[,]
                    {                             
                             {"assetdetails_takenby",ImpairLock("Impairment in Process")}                            
                    };
                        string[,] where = new string[,]
                    {                             
                             {"assetdetails_assetdet_id", dtA.Rows[i]["Asset Id"].ToString()}
                    };
                        objCommonIUD.UpdateCommon(codes, where, "fa_trn_tassetdetails");
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                GetOffConnection();
            }
            return instcomm.ToString();
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
                return string.Empty;
            }
        }


        public DataTable InsertImpairHeader(string FileName, string date, int NoOfRec, string SeqNo, string IsRemoved, int LogUserId, string UpFlag)
        {
            int DraftId = GetDraftId();
            DataTable dt = new DataTable();
            string[,] codes = new string[,]
                    {
                            
                             {"imrheader_imr_number",SeqNo},
                             {"imrheader_nof_records",NoOfRec.ToString()},
                             {"timrheader_process_date",convertoDate(date)},
                             {"imrheader_upld_flag",UpFlag},
                             {"imrheader_upld_date",convertoDate(date)},
                             {"imrheader_upld_fname",FileName},
                             {"imrheader_maker_id",LogUserId.ToString()},
                             {"imrheader_maker_date",convertoDate(date)},
                             {"imrheader_isremoved",IsRemoved},
                             {"status_flag",DraftId.ToString()},
                             {"attachment_filename",FileName.ToString()},
                    };
            string TblName = "fa_trn_timrheader";
            string instcomm = objCommonIUD.InsertCommon(codes, TblName);
            try
            {
                GetConnection();
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da;

                cmd = new SqlCommand("pr_ifams_trn_imrheaderid", confa);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@imrheader_imr_number", SqlDbType.VarChar).Value = SeqNo;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                GetOffConnection();
            }
            return dt;
        }



        public string ImpairLock(string Lock)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da;

                cmd = new SqlCommand("pr_ifams_trn_imrheader", confa);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ImpairLock", SqlDbType.VarChar).Value = "ImpairLock";
                cmd.Parameters.Add("@ExcelValidateName", SqlDbType.VarChar).Value = Lock;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count != 0)
                {
                    return dt.Rows[0]["excelvalidate_gid"].ToString();
                }
                else
                {
                    return dt.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                GetOffConnection();
            }
            return dt.Rows[0]["excelvalidate_gid"].ToString();
        }


        public int GetDraftId()
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da;
                cmd = new SqlCommand("pr_ifams_trn_getdraftid", confa);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Draft", SqlDbType.VarChar).Value = "DRAFT";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count != 0)
                {
                    return Convert.ToInt32(dt.Rows[0][0].ToString());
                }
                else
                {
                    return Convert.ToInt32(dt.Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                GetOffConnection();
            }
            return Convert.ToInt32(dt.Rows[0][0].ToString());
        }


        public string GetStatus(int ImpHeaderId)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da;

                cmd = new SqlCommand("pr_ifams_trn_getstatusimpairment", confa);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@imrheader_gid", SqlDbType.BigInt).Value = ImpHeaderId;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count != 0)
                {
                    return dt.Rows[0]["status_name"].ToString();
                }
                else
                {
                    return dt.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                GetOffConnection();
            }
            return dt.Rows[0]["status_name"].ToString();
        }

        public string UpdateMaker(int GidforMaker)
        {
            string ImpHeaderGid = string.Empty;
            string Msg = string.Empty;
            try
            {
                GetConnection();
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da;
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_getdraftid", confa);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Draft", SqlDbType.VarChar).Value = "WAITING FOR APPROVAL";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                int Idwaitapp = Convert.ToInt32(dt.Rows[0][0].ToString());

                ImpHeaderGid = WaitToApp(GidforMaker, Idwaitapp);
                Msg = InsertQMaker(ImpHeaderGid);

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                GetOffConnection();
            }
            return Msg.ToString();
        }

        public string InsertQMaker(string ImpHeaderGid)
        {
            string[,] col = new string[,]
	                        {
                                     {"queue_date",Convert.ToString(objCommonIUD.GetCurrentDate())},
                                     {"queue_ref_flag",GetRef("IOA").ToString()},
                                     {"queue_ref_gid", ImpHeaderGid},
                                     {"queue_action_for", "A"},
                                     {"queue_from",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                                     {"queue_to_type","G" },
                                     {"queue_to",RoleGroupGid()},
                                     {"queue_prev_gid","0"},
                                     {"queue_action_status","0"},
                                     {"queue_isremoved","N"}                                     
	                      };
            string inst = objCommonIUD.InsertCommon(col, "iem_trn_tqueue");
            return inst;
        }

        public string RoleGroupGid()
        {
            string RoleGroupGid = string.Empty;
            SqlCommand cmd;
            SqlDataAdapter da;
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_getflowdetails", confa);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = "IOAMKR";
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "groupname";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row1 in dt.Rows)
                    RoleGroupGid = row1["rolegroup_gid"].ToString();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                GetOffConnection();
            }
            return RoleGroupGid;
        }


        public string WaitToApp(int GidforMake, int statusflag)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da;
                cmd = new SqlCommand("pr_ifams_trn_getwaitid", confa);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@imrheader_gid", SqlDbType.BigInt).Value = GidforMake;
                cmd.Parameters.Add("@status_flag", SqlDbType.BigInt).Value = statusflag;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count != 0)
                {
                    return Convert.ToString(dt.Rows[0][0].ToString());
                }
                else
                {
                    return Convert.ToString(dt.Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                GetOffConnection();
            }
            return Convert.ToString(dt.Rows[0][0].ToString());
        }


        public string GetAssetId(string AssetID)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da;
                cmd = new SqlCommand("pr_ifams_trn_impdetailassetid", confa);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@impasset_id", SqlDbType.VarChar).Value = AssetID;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                GetOffConnection();
            }
            if (dt.Rows.Count != 0)
            {
                return dt.Rows[0][0].ToString();
            }
            else
            {
                return string.Empty;
            }
        }



        public DataTable SAGetheaderDetails(string saheaderid)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da;
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", confa);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@soaheader_sale_number", SqlDbType.VarChar).Value = saheaderid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "saheadergid";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
            }
            return dt;
        }

        public int SAGetCheckerDetails(int samakerid)
        {
            int result = 0;
            try
            {
                GetConnection();
                SqlCommand cmd = new SqlCommand();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_sereview", confa);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = samakerid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getsuperlist";
                result = (int)cmd.ExecuteScalar();
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
            return result;
        }


        public string SALocationdetail(string id)
        {
            //DataTable dt = new DataTable();
            string result = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da;
                DataTable dt = new DataTable();
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", confa);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@assetdetid", SqlDbType.VarChar).Value = id.ToString();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "SALocation";

                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result = dt.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                GetOffConnection();
            }
            return result;
        }



        public string getTheUser(string groupCode)
        {
            string success = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_getflowdetails", confa);
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
                GetOffConnection();
            }
        }



        public string UpdateCheckImpairStatus(int GidforApp, string Remarks)
        {
            string Msg = string.Empty;
            try
            {
                GetConnection();
                Ifams_Propertyx.ImpairmentsModel status = new Ifams_Propertyx.ImpairmentsModel();
                string Impseqno = (string)HttpContext.Current.Session["ImpairNo"];
                int AppStatusId = GetAppId();
                int LoguserID = objCmnFunctions.GetLoginUserGid();
                int RoleGid = Convert.ToInt32(RoleGroupGid());//28
                int RefFlagId = Convert.ToInt32(GetRef("IOA"));//16
                string ImpHeaderID = CheckerApp(GidforApp, AppStatusId);
                string[,] codes = new string[,]
                    { 
                             {"queue_action_by",LoguserID.ToString()},
                             {"queue_action_status","1"},
                             {"queue_action_date",Convert.ToString(objCommonIUD.GetCurrentDate())},
                             {"queue_action_remark",Remarks},
                             {"queue_isremoved","Y"}                            
                    };

                string[,] where = new string[,]
                    { 
                             {"queue_ref_gid",ImpHeaderID},
                             {"queue_ref_flag",Convert.ToString(RefFlagId)},
                             {"queue_to",Convert.ToString(RoleGid)}
                    };
                Msg = objCommonIUD.UpdateCommon(codes, where, "iem_trn_tqueue");
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_getimpassetId", confa);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@imrheader_gid", SqlDbType.BigInt).Value = Convert.ToInt32(ImpHeaderID);
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ///////////////Lock Impairment
                    GetConnection();
                    //depreciation calculation
                    decimal sumassetdep = 0;
                    decimal rectificationamt = 0;
                    string assetdep = "0";
                    int assetdetgid = Convert.ToInt32(dt.Rows[i]["assetdetails_gid"].ToString());
                    string assetdetgroupid = dt.Rows[i]["assetdetails_asset_groupid"].ToString();
                    string inw_no = dt.Rows[i]["impdetail_inw_no"].ToString();
                    IEM.Areas.IFAMS.Models.Ifams_Propertyx.ImpairmentsModel obj = new IEM.Areas.IFAMS.Models.Ifams_Propertyx.ImpairmentsModel();
                    DataTable dtdep = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_SaleMaker", confa);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "Totdep");
                    cmd.Parameters.AddWithValue("@assetgid", assetdetgid);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dtdep);
                    foreach (DataRow rowdep in dtdep.Rows)
                    {
                        assetdep = rowdep["depreciation_amount"].ToString();
                    }
                    sumassetdep = Convert.ToDecimal(assetdep.ToString());
                    string soacaptilisationdat = Convert.ToString("SYSDATETIME()");
                    DateTime soacaptilisationda = DateTime.Now;
                   // DateTime dtTillDate = DateTime.Now;
                    DateTime dtTillDate = Convert.ToDateTime(dt.Rows[i]["imrdet_date"].ToString());
                    decimal AssetValues = 0;
                    string sAssetCode = string.Empty;
                    string cNot5000Case = string.Empty;
                    string sBranch1 = string.Empty;
                    string sBranch2 = string.Empty;
                    DateTime dtBranchLaunch = DateTime.Now;
                    DateTime dtLeaseStart = DateTime.Now;
                    DateTime dtLeaseEnd = DateTime.Now;
                    string sDepType = string.Empty;
                    decimal dDepRate = 0;
                    string sAssetGroup = string.Empty;
                    string sPONumber = string.Empty;
                    string sFICCRef = string.Empty;
                    string sAsset_GroupId = string.Empty;
                    decimal dTransfer_value = 0;
                    string CanDepreciateFully = string.Empty;
                    Int32 dDepDevRate = 0;
                    Int32 assetdetid = assetdetgid;
                    // decimal sumassetdep = 0;
                    //decimal linewdv = 0;
                    //decimal rectificationamt = 0;
                    // decimal linePL = 0;
                    DataTable dtwdv = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_SaleMaker", confa);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "WDV");
                    cmd.Parameters.AddWithValue("@assetid", assetdetid);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dtwdv);
                    //DateTime dtLeaseEnd1 =
                    foreach (DataRow row1 in dtwdv.Rows)
                    {
                        dtBranchLaunch = Convert.ToDateTime(string.IsNullOrEmpty(row1["branch_start_date"].ToString()) ? "0" : row1["branch_start_date"]);
                        soacaptilisationda = Convert.ToDateTime(string.IsNullOrEmpty(row1["assetdetails_cap_date"].ToString()) ? "01-01-1900" : row1["assetdetails_cap_date"]);
                        AssetValues = Convert.ToDecimal(string.IsNullOrEmpty(row1["assetdetails_asset_value"].ToString()) ? "0" : row1["assetdetails_asset_value"]);
                        sAssetCode = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_asset_code"].ToString()) ? "0" : row1["assetdetails_asset_code"]);
                        cNot5000Case = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_not_5kcase"].ToString()) ? "0" : row1["assetdetails_not_5kcase"]);
                        //sBranch1 = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_not_5kcase"].ToString()) ? "0" : row1["assetdetails_not_5kcase"]);
                        dtLeaseStart = Convert.ToDateTime(string.IsNullOrEmpty(row1["assetdetails_lease_startdate"].ToString()) ? "01-01-1900" : row1["assetdetails_lease_startdate"]);
                        dtLeaseEnd = Convert.ToDateTime(string.IsNullOrEmpty(row1["assetdetails_lease_enddate"].ToString()) ? "01-01-1900" : row1["assetdetails_lease_enddate"]);
                        sDepType = Convert.ToString(string.IsNullOrEmpty(row1["asset_dep_type"].ToString()) ? "0" : row1["asset_dep_type"]);
                        dDepRate = Convert.ToDecimal(string.IsNullOrEmpty(row1["asset_dep_rate"].ToString()) ? "0" : row1["asset_dep_rate"]);
                        sAssetGroup = Convert.ToString(string.IsNullOrEmpty(row1["h_asst_groupid_no"].ToString()) ? "0" : row1["h_asst_groupid_no"]);
                        sPONumber = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_po_number"].ToString()) ? "0" : row1["assetdetails_po_number"]);
                        sFICCRef = Convert.ToString(string.IsNullOrEmpty(row1["h_ficrefnumber"].ToString()) ? "0" : row1["h_ficrefnumber"]);
                        sAsset_GroupId = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_asset_groupid"].ToString()) ? "0" : row1["assetdetails_asset_groupid"]);
                        dTransfer_value = Convert.ToDecimal(string.IsNullOrEmpty(row1["assetdetails_trf_value"].ToString()) ? "0" : row1["assetdetails_trf_value"]);
                        // CanDepreciateFully = Convert.ToString(string.IsNullOrEmpty(row1["h_ficrefnumber"].ToString()) ? "0" : row1["h_ficrefnumber"]);
                    }

                    decimal depamt = Math.Round(objidm.GetTotalDep(soacaptilisationda, dtTillDate,
                                                                    AssetValues, sAssetCode,
                                                                    cNot5000Case, sBranch1, sBranch2,
                                                                    dtBranchLaunch, dtLeaseStart, dtLeaseEnd,
                                                                    sDepType, dDepRate,
                                                                    sAssetGroup,
                                                                    sPONumber,
                                                                    sFICCRef,
                                                                    sAsset_GroupId,
                                                                    dTransfer_value,
                                                                    CanDepreciateFully,
                                                                    dDepDevRate),2);
                    rectificationamt = depamt - sumassetdep;
                    string[,] codesdep = new string[,]
                    {
                        {"depreciation_amount", rectificationamt.ToString()},
                        {"depreciation_assetdet_gid", assetdetgid.ToString()},
                        {"depreciation_month", Convert.ToString("SYSDATETIME()")},
                        {"depreciation_asset_groupid", assetdetgroupid.ToString()},
                        {"depreciation_asset_owner", "F"},
                        {"depreciation_type","I"},
                        {"depreciation_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                        {"depreciation_insert_date", Convert.ToString("SYSDATETIME()")}
                    };
                    string insertcmn = objCommonIUD.InsertCommon(codesdep, "fa_trn_tdepreciation");
                    //gl upload entry 
                    cmd = new SqlCommand("pr_ifams_trn_TransferMaker", confa);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "assetdetails";
                    cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = dt.Rows[i]["impasset_id"].ToString().ToString();
                    string gid = Convert.ToString(cmd.ExecuteScalar());

                    cmd = new SqlCommand("pr_ifams_trn_TransferMaker", confa);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "impairment";
                    cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = dt.Rows[i]["impasset_id"].ToString().ToString();
                    cmd.Parameters.Add("@headergid", SqlDbType.VarChar).Value = ImpHeaderID;
                    da = new SqlDataAdapter(cmd);
                    string maker_id = string.Empty;
                    string maker_date = string.Empty;
                    string inwNo = string.Empty;
                    DataTable UPLOADDATATBL = new DataTable();
                    da.Fill(UPLOADDATATBL);
                    if (UPLOADDATATBL.Rows.Count > 0)
                        foreach (DataRow rowdep in UPLOADDATATBL.Rows)
                        {
                            maker_id = UPLOADDATATBL.Rows[0]["imrheader_maker_id"].ToString();
                            maker_date = UPLOADDATATBL.Rows[0]["imrdet_date"].ToString();
                            inwNo = UPLOADDATATBL.Rows[0]["impdetail_inw_no"].ToString();
                        }
                    string Lockid = ImpairLock("Asset Impaired");
                    string[,] code = new string[,]
                    { 
                             {"assetdetails_active_status","N"},
                             {"assetdetails_isremoved","Y"},
                             {"assetdetails_takenby",Lockid},
                             {"assetdetails_imp_date",convertoDate(maker_date)},
                             {"assetdetails_impair_asset","Y"}
                    };
                    string[,] condition = new string[,]
                    { 
                             {"assetdetails_assetdet_id",dt.Rows[i]["impasset_id"].ToString()}   ,
                             {"assetdetails_gid",gid.ToString()} 
                    };
                    Msg = objCommonIUD.UpdateCommon(code, condition, "fa_trn_tassetdetails");
                    cmd = new SqlCommand("pr_ifams_trn_TransferMaker", confa);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "UploadDetails";
                    cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = gid;
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
                    UPLOADDATATBL = new DataTable();
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

                    cmd = new SqlCommand("pr_ifams_trn_TransferMaker", confa);
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

                    //if (rectificationamt != 0 || rectificationamt < 0)
                    //{

                    DataTable dtdeptrn = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_SaleMaker", confa);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "Totdep");
                    cmd.Parameters.AddWithValue("@assetgid", assetdetgid);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dtdeptrn);
                    foreach (DataRow rowdep in dtdeptrn.Rows)
                    {
                        assetdep = rowdep["depreciation_amount"].ToString();
                    }

                    sumassetdep = Convert.ToDecimal(assetdep.ToString());
                    DataTable datamaker_id = objCmnFunctions.GetLoginUserDetails(Convert.ToInt32(maker_id));

                    string[,] glCoulmsDepC = new string[,]
                            {                                
                               // {"tran_date",convertoDate(maker_date).ToString()},
                                {"tran_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_desc"," " + asset_id.ToString() + " | IMPAIRMENT OF ASSET  | " + inwNo },
                                {"tran_gl_no",resdepglCode},
                                //{"tran_amount",rectificationamt.ToString()},sumassetdep
                                {"tran_amount",sumassetdep.ToString()},
                                {"tran_acc_mode","D".ToString()},
                                {"tran_mult","1"},  
                                {"tran_fc_code",BS},
                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                {"tran_cc_code",CC},                                
                                {"tran_ou_code",branch},
                                {"tran_ref_flag",GetRef("IOA")},
                                {"tran_ref_gid", gid.ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                                {"tran_main_cat", string.Empty },//product categorycode
                                {"tran_sub_cat",subcat},
                                {"tran_expense_category","9"},
                                {"tran_primary_branch_code",branch.ToString()},
                             //   {"tran_invoice_no",ImpHeaderID.Trim().ToString()},                              
                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},                                
                                {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                                //  {"tran_maker_id",objCmnFunctions.GetLoginUserGid().ToString()},
                               

                            };
                    string insertforglDepC = objCommonIUD.InsertCommon(glCoulmsDepC, "fa_trn_ttran");

                    //}
                    if (AssetValues - rectificationamt != 0 || AssetValues - rectificationamt < 0)
                    {
                        string[,] glCoulmsD = new string[,]
                            {                                
                                //{"tran_date",convertoDate(maker_date).ToString()},
                                {"tran_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_val_date",convertoDate(maker_date).ToString()},
                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_gl_no","441100001"},
                                {"tran_desc"," " + asset_id.ToString() + " | IMPAIRMENT OF ASSET  | " + inwNo },
                                // {"tran_amount",(AssetValues - rectificationamt).ToString()}, Muthu
                                {"tran_amount",(AssetValues - sumassetdep).ToString()},
                                {"tran_acc_mode","D".ToString()},
                                {"tran_mult","-1"},  
                                {"tran_fc_code",BS},
                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                {"tran_cc_code",CC},
                                {"tran_ou_code",branch},
                                {"tran_ref_flag",GetRef("IOA")},
                                {"tran_ref_gid",gid},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                                {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                {"tran_expense_category","9"},
                                {"tran_primary_branch_code",branch.ToString()},                                
                                // {"tran_invoice_no",ImpHeaderID.Trim().ToString()},                               
                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                                //  {"tran_maker_id",maker_id.ToString()},
                                {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}

                            };
                        Msg = objCommonIUD.InsertCommon(glCoulmsD, "fa_trn_ttran");
                    }
                    string[,] glCoulmsC = new string[,]
                            {                                
                                //{"tran_date",convertoDate(maker_date).ToString()},
                            {"tran_date",Convert.ToString("SYSDATETIME()").ToString()},
                            {"tran_val_date",convertoDate(maker_date).ToString()},
                            {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
                            {"tran_gl_no",glCode},
                            {"tran_desc"," " + asset_id.ToString() + " | IMPAIRMENT OF ASSET  | " + inwNo },
                            {"tran_amount",AssetValues.ToString()},
                            {"tran_acc_mode","C".ToString()},
                            {"tran_mult","1"},  
                            {"tran_fc_code",BS},
                            {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                            {"tran_cc_code",CC},
                            {"tran_ou_code",branch},
                            {"tran_ref_flag",GetRef("IOA")},
                            {"tran_ref_gid",gid},
                            {"tran_upload_gid","0".ToString()},
                            {"tran_isremoved","N"},
                            {"tran_main_cat",cat},
                            {"tran_sub_cat",subcat},
                            {"tran_expense_category","9"},
                            {"tran_primary_branch_code",branch.ToString()},
                            // {"tran_invoice_no",ImpHeaderID.Trim().ToString()},                                
                            {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                            //  {"tran_maker_id",maker_id.ToString()},
                            {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                            {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                            {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}

                            };
                    Msg = objCommonIUD.InsertCommon(glCoulmsC, "fa_trn_ttran");


                    //tran table insert for impair
                    if (rectificationamt != 0)
                    {
                        if (rectificationamt < 0)
                        {
                            rectificationamt = Math.Abs(rectificationamt);



                            string[,] glCoulmsImpairDepC = new string[,]
                            {                                
                                //{"tran_date",convertoDate(maker_date).ToString()},
                                {"tran_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_desc"," " + asset_id.ToString() + " | DEPRECIATON IMPAIRMENT OF ASSET  "},
                                {"tran_gl_no",depglCode},
                                {"tran_amount",rectificationamt.ToString()},
                                //{"tran_amount",sumassetdep.ToString()},
                                {"tran_acc_mode","C".ToString()},
                                {"tran_mult","1"},  
                                {"tran_fc_code",BS},
                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                {"tran_cc_code",CC},                                
                                {"tran_ou_code",branch},
                                {"tran_ref_flag",GetRef("IDEP")},
                                {"tran_ref_gid", gid.ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                                {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                {"tran_expense_category","10"},
                                {"tran_primary_branch_code",branch.ToString()},
                                //  {"tran_invoice_no",ImpHeaderID.Trim().ToString()},                              
                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                                //  {"tran_maker_id",objCmnFunctions.GetLoginUserGid().ToString()},
                                {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                            string insertforimpairglDepC = objCommonIUD.InsertCommon(glCoulmsImpairDepC, "fa_trn_ttran");


                            string[,] glCoulmsImpairDepD = new string[,]
                            {                                
                                //{"tran_date",convertoDate(maker_date).ToString()},
                                {"tran_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_desc"," " + asset_id.ToString() + " | DEPRECIATON IMPAIRMENT OF ASSET  "},
                                {"tran_gl_no",resdepglCode},
                                {"tran_amount",rectificationamt.ToString()},
                                // {"tran_amount",sumassetdep.ToString()},
                                {"tran_acc_mode","D".ToString()},
                                {"tran_mult","1"},  
                                {"tran_fc_code",BS},
                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                {"tran_cc_code",CC},                                
                                {"tran_ou_code",branch},
                                {"tran_ref_flag",GetRef("IDEP")},
                                {"tran_ref_gid", gid.ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                                {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                {"tran_expense_category","10"},
                                {"tran_primary_branch_code",branch.ToString()},
                                // {"tran_invoice_no",ImpHeaderID.Trim().ToString()},                              
                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                                //  {"tran_maker_id",objCmnFunctions.GetLoginUserGid().ToString()},
                                {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                            string insertforimpairglDepD = objCommonIUD.InsertCommon(glCoulmsImpairDepD, "fa_trn_ttran");
                        }
                        else
                        {
                            string[,] glCoulmsImpairDepC = new string[,]
                            {                                
                                //{"tran_date",convertoDate(maker_date).ToString()},
                                {"tran_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_desc"," " + asset_id.ToString() + " | DEPRECIATON IMPAIRMENT OF ASSET  "},
                                {"tran_gl_no",resdepglCode},
                                {"tran_amount",rectificationamt.ToString()},
                                //{"tran_amount",sumassetdep.ToString()},
                                {"tran_acc_mode","C".ToString()},
                                {"tran_mult","1"},  
                                {"tran_fc_code",BS},
                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                {"tran_cc_code",CC},                                
                                {"tran_ou_code",branch},
                                {"tran_ref_flag",GetRef("IDEP")},
                                {"tran_ref_gid", gid.ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                                {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                {"tran_expense_category","10"},
                                {"tran_primary_branch_code",branch.ToString()},
                                //  {"tran_invoice_no",ImpHeaderID.Trim().ToString()},                              
                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                                //  {"tran_maker_id",objCmnFunctions.GetLoginUserGid().ToString()},
                                {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                            string insertforimpairglDepC = objCommonIUD.InsertCommon(glCoulmsImpairDepC, "fa_trn_ttran");


                            string[,] glCoulmsImpairDepD = new string[,]
                            {                                
                                //{"tran_date",convertoDate(maker_date).ToString()},
                                {"tran_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_desc"," " + asset_id.ToString() + " | DEPRECIATON IMPAIRMENT OF ASSET  "},
                                {"tran_gl_no",depglCode},
                                {"tran_amount",rectificationamt.ToString()},
                                // {"tran_amount",sumassetdep.ToString()},
                                {"tran_acc_mode","D".ToString()},
                                {"tran_mult","1"},  
                                {"tran_fc_code",BS},
                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                {"tran_cc_code",CC},                                
                                {"tran_ou_code",branch},
                                {"tran_ref_flag",GetRef("IDEP")},
                                {"tran_ref_gid", gid.ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                                {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                {"tran_expense_category","10"},
                                {"tran_primary_branch_code",branch.ToString()},
                                // {"tran_invoice_no",ImpHeaderID.Trim().ToString()},                              
                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                                //  {"tran_maker_id",objCmnFunctions.GetLoginUserGid().ToString()},
                                {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                            string insertforimpairglDepD = objCommonIUD.InsertCommon(glCoulmsImpairDepD, "fa_trn_ttran");
                        }
                    }
                    if (Msg == "success")
                        Msg = "Success";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                GetOffConnection();
            }
            return Msg.ToString();
        }


        public string UpdateCheckImpairStatusRej(int GidforApp, string Remarks)
        {
            string Msg = string.Empty;
            try
            {
                GetConnection();
                Ifams_Propertyx.ImpairmentsModel status = new Ifams_Propertyx.ImpairmentsModel();
                string Impseqno = (string)HttpContext.Current.Session["ImpairNo"];
                int AppStatusId = GetRejectId();
                int LoguserID = objCmnFunctions.GetLoginUserGid();
                int RoleGid = Convert.ToInt32(RoleGroupGid());//28
                int RefFlagId = Convert.ToInt32(GetRef("IOA"));//16
                string ImpHeaderID = CheckerApp(GidforApp, AppStatusId);

                string[,] codes = new string[,]
                    { 
                             {"queue_action_by",LoguserID.ToString()},
                             {"queue_action_status","0"},
                             {"queue_action_date",Convert.ToString(objCommonIUD.GetCurrentDate())},
                             {"queue_action_remark",Remarks},
                             {"queue_isremoved","Y"}
                    };
                string[,] where = new string[,]
                    { 
                             {"queue_ref_gid",ImpHeaderID},
                             {"queue_ref_flag",Convert.ToString(RefFlagId)},
                             {"queue_to",Convert.ToString(RoleGid)}
                    };
                Msg = objCommonIUD.UpdateCommon(codes, where, "iem_trn_tqueue");
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter();
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_getimpassetId", confa);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@imrheader_gid", SqlDbType.BigInt).Value = Convert.ToInt32(ImpHeaderID);
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ///////////////free Lock Impairment
                    string Lockid = ImpairLock("Free");
                    string[,] code = new string[,]
                    { 
                             {"assetdetails_active_status","N"},
                             {"assetdetails_isremoved","N"},
                               {"assetdetails_takenby",Lockid}
                             //{"imheader_Remarks",Remarks},                          
                    };
                    string[,] condition = new string[,]
                    { 
                             {"assetdetails_assetdet_id",dt.Rows[i]["impasset_id"].ToString()}                           
                    };
                    Msg = objCommonIUD.UpdateCommon(code, condition, "fa_trn_tassetdetails");
                    string[,] cod = new string[,]
                    {                             
                             {"imrdetail_isremoved","Y"}, 
                    };
                    string[,] cond = new string[,]
                    { 
                             {"impasset_id",dt.Rows[i]["impasset_id"].ToString()}                           
                    };
                    Msg = objCommonIUD.UpdateCommon(cod, cond, "fa_trn_timpdetail");
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                GetOffConnection();
            }

            return Msg.ToString();
        }
        public string DeleteImpair(string Id)
        {

            string Msg = string.Empty;
            try
            {
                string[,] codes = new string[,]
                    { 
                             {"imrheader_isremoved","Y"},                                                     
                    };
                string[,] where = new string[,]
                    {                             
                             {"imrheader_gid",Id}
                    };
                Msg = objCommonIUD.UpdateCommon(codes, where, "fa_trn_timrheader");
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_imrheaderdelete", confa);
                cmd.Parameters.AddWithValue("@imrheader_gid", Convert.ToInt32(Id.ToString()));
                cmd.CommandType = CommandType.StoredProcedure;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    //string[,] code = new string[,]
                    //{ 
                    //         {"assetdetails_takenby", string.Empty },
                    //};
                    //string[,] wh = new string[,]
                    //{ 
                    //         {"assetdetails_assetdet_id",row["impasset_id"].ToString()}
                    //};
                    //Msg = objCommonIUD.UpdateCommon(code, wh, "fa_trn_tassetdetails");
                    string Lockid = ImpairLock("Free");
                    string[,] code = new string[,]
                    {                             
                             {"assetdetails_takenby",Lockid}                            
                    };
                    string[,] wher = new string[,]
                    { 
                           {"assetdetails_assetdet_id",row["impasset_id"].ToString()} 
                    };
                    objCommonIUD.UpdateCommon(code, wher, "fa_trn_tassetdetails");
                    string[,] cod = new string[,]
                    {                             
                             {"imrdetail_isremoved","Y"}                            
                    };
                    string[,] whe = new string[,]
                    { 
                           {"impasset_id",row["impasset_id"].ToString()} 
                    };
                    objCommonIUD.UpdateCommon(cod, whe, "fa_trn_timpdetail");
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                GetOffConnection();
            }
            return Msg.ToString();
        }


        public string CheckerApp(int GidforMake, int statusflag)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da;

                cmd = new SqlCommand("pr_ifams_trn_getwaitid", confa);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@imrheader_gid", SqlDbType.BigInt).Value = GidforMake;
                cmd.Parameters.Add("@status_flag", SqlDbType.BigInt).Value = statusflag;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count != 0)
                {
                    return Convert.ToString(dt.Rows[0][0].ToString());
                }
                else
                {
                    return Convert.ToString(dt.Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                GetOffConnection();
            }
            return Convert.ToString(dt.Rows[0][0].ToString());
        }

        public int GetAppId()
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da;
                cmd = new SqlCommand("pr_ifams_trn_getdraftid", confa);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Draft", SqlDbType.VarChar).Value = "APPROVED";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count != 0)
                {
                    return Convert.ToInt32(dt.Rows[0][0].ToString());
                }
                else
                {
                    return Convert.ToInt32(dt.Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                GetOffConnection();
            }
            return Convert.ToInt32(dt.Rows[0][0].ToString());
        }



        public int GetRejectId()
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da;
                cmd = new SqlCommand("pr_ifams_trn_getdraftid", confa);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Draft", SqlDbType.VarChar).Value = "REJECTED";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count != 0)
                {
                    return Convert.ToInt32(dt.Rows[0][0].ToString());
                }
                else
                {
                    return Convert.ToInt32(dt.Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                GetOffConnection();
            }
            return Convert.ToInt32(dt.Rows[0][0].ToString());
        }


        //public DataTable GetheaderDetails(string headerid)
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        GetConnection();
        //        SqlDataAdapter da;
        //        SqlCommand cmd;
        //        cmd = new SqlCommand("pr_ifams_trn_TransferMaker", confa);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@headergid", SqlDbType.VarChar).Value = headerid;
        //        cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "headergid";
        //        //string result = (string)cmd.ExecuteScalar();
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);

        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //        return dt;
        //    }
        //    finally
        //    {
        //        GetOffConnection();
        //    }
        //}

        public string GetRef(string ref_name)
        {
            try
            {
                SqlDataAdapter da;
                SqlCommand cmd = new SqlCommand();
                DataTable dt = new DataTable();
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", confa);
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


        public List<Ifams_Propertyx.Attachment> Getattachment(string id, string refe, string type)
        {
            Ifams_Propertyx.ImpairmentsModel ob = new Ifams_Propertyx.ImpairmentsModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                List<Ifams_Propertyx.Attachment> obj_ = new List<Ifams_Propertyx.Attachment>();
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da;
                Ifams_Propertyx.Attachment newob = new Ifams_Propertyx.Attachment();
                cmd = new SqlCommand("attachment_get", confa);
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
                        newob = new Ifams_Propertyx.Attachment();
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

        public string save_attachment(Ifams_Propertyx.ImpairmentsModel model)
        {
            string inst = string.Empty;
            try
            {
                string[,] col = new string[,]
	                {
                             {"attachment_ref_gid",model._gid.ToString()},
                             {"attachment_ref_flag",GetRef("IOA")},
                             {"attachment_filename", model._attach_file.ToString()},
                             {"attachment_attachmenttype_gid", GetAttachType().ToString()},
                             {"attachment_desc",model._attach_desc.ToString()},                            
                             {"attachment_date",Convert.ToString(objCommonIUD.GetCurrentDate())},
                             {"attachment_by",objCmnFunctions.GetLoginUserGid().ToString()},
                             {"attachment_isremoved","N"},
                           
	                };
                inst = objCommonIUD.InsertCommon(col, "iem_trn_tattachment");
                //string[,] code = new string[,]
                //    { 
                //             {"attachment_filename",model._attach_file.ToString()},
                //    };
                //string[,] wh = new string[,]
                //    { 
                //             {"imrheader_gid",model._gid.ToString()}
                //    };
                //inst = objCommonIUD.UpdateCommon(code, wh, "fa_trn_timrheader");
            }
            catch (Exception e)
            {
                objErrorLog.WriteErrorLog(e.Message.ToString(), e.ToString());
                return string.Empty;
            }
            finally
            {

            }
            return inst.ToString();
        }

        public string GetAttachType()
        {
            try
            {
                GetConnection();
                SqlCommand cmd = new SqlCommand();
                SqlDataAdapter da;
                da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                string attachtype = string.Empty;
                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", confa);
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

        public string getfilename(string toanumber)
        {
            try
            {
                GetConnection();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", confa);
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
        public string RevokeImpair(string gid)
        {
            try
            {
                List<Ifams_Propertyx.ImpairmentsModel> list = new List<Ifams_Propertyx.ImpairmentsModel>();
                list = GetImpView(gid);
                for (int i = 0; i < list.Count(); i++)
                {
                    string[,] col = new string[,]
	                {                                      
                             {"assetdetails_update_date",Convert.ToString(objCommonIUD.GetCurrentDate())},
                             {"assetdetails_update_by",objCmnFunctions.GetLoginUserGid().ToString()},
                             {"assetdetails_isremoved","N"},   
                             {"assetdetails_takenby","21"}
	                };
                    string[,] whCol = new string[,]
	                {
                             {"assetdetails_assetdet_id",list[0]._impasset_id.ToString()},                            
	                };
                    string upt = objCommonIUD.UpdateCommon(col, whCol, "fa_trn_tassetdetails");


                    SqlCommand cmd = new SqlCommand("pr_ifams_trn_TransferMaker", confa);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "UploadDetails";
                    cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = gid;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
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
                    cmd = new SqlCommand("pr_ifams_trn_TransferMaker", confa);
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

                    //if (rectificationamt != 0 || rectificationamt < 0)
                    //{
                    GetConnection();
                    DataTable dtdeptrn = new DataTable();
                    cmd = new SqlCommand("pr_IFams_trn_TransferMaker", confa);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@querytype", "getDepforRevoke");
                    cmd.Parameters.AddWithValue("@headergid", gid);
                    cmd.Parameters.AddWithValue("@assetid", list[0]._impasset_id.ToString());
                    decimal rectificationamt = Convert.ToDecimal(cmd.ExecuteScalar());
                    string[,] glCoulmsDepC = new string[,]
                            {                                
                                // {"tran_date",convertoDate(maker_date).ToString()},
                                {"tran_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_desc"," " + asset_id.ToString() + " | REVOKE IMPAIRMENT OF ASSET  " },
                                {"tran_gl_no",resdepglCode},
                                //{"tran_amount",rectificationamt.ToString()},sumassetdep
                                {"tran_amount",rectificationamt.ToString()},
                                {"tran_acc_mode","C".ToString()},
                                {"tran_mult","1"},  
                                {"tran_fc_code",BS},
                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                {"tran_cc_code",CC},                                
                                {"tran_ou_code",branch},
                                {"tran_ref_flag",GetRef("IOAR")},
                                {"tran_ref_gid", gid.ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                                {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                {"tran_expense_category","9"},
                                {"tran_primary_branch_code",branch.ToString()},
                                //   {"tran_invoice_no",ImpHeaderID.Trim().ToString()},                              
                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                                
                                {"tran_checker_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                {"tran_emp_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                    string insertforglDepC = objCommonIUD.InsertCommon(glCoulmsDepC, "fa_trn_ttran");

                    //}
                    if (list[i].imrdet_salevalue - rectificationamt != 0 || list[i].imrdet_salevalue - rectificationamt < 0)
                    {
                        string[,] glCoulmsD = new string[,]
                            {                                
                                //{"tran_date",convertoDate(maker_date).ToString()},
                                {"tran_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_gl_no","441100001"},
                                {"tran_desc"," " + asset_id.ToString() + " | REVOKE IMPAIRMENT OF ASSET  " },
                                // {"tran_amount",(AssetValues - rectificationamt).ToString()}, Muthu
                                {"tran_amount",(list[i].imrdet_salevalue - rectificationamt).ToString()},
                                {"tran_acc_mode","C".ToString()},
                                {"tran_mult","-1"},  
                                {"tran_fc_code",BS},
                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                {"tran_cc_code",CC},
                                {"tran_ou_code",branch},
                                {"tran_ref_flag",GetRef("IOAR")},
                                {"tran_ref_gid",gid},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                                {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                {"tran_expense_category","9"},
                                {"tran_primary_branch_code",branch.ToString()},                                
                                // {"tran_invoice_no",ImpHeaderID.Trim().ToString()},                               
                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                                //  {"tran_maker_id",maker_id.ToString()},
                                {"tran_checker_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                {"tran_emp_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                        string Msg1 = objCommonIUD.InsertCommon(glCoulmsD, "fa_trn_ttran");
                    }
                    string[,] glCoulmsC = new string[,]
                            {                                
                                //{"tran_date",convertoDate(maker_date).ToString()},
                                {"tran_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_gl_no",glCode},
                                {"tran_desc"," " + asset_id.ToString() + " | REVOKE IMPAIRMENT OF ASSET  " },
                                {"tran_amount",list[i].imrdet_salevalue.ToString()},
                                {"tran_acc_mode","D".ToString()},
                                {"tran_mult","1"},  
                                {"tran_fc_code",BS},
                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                {"tran_cc_code",CC},
                                {"tran_ou_code",branch},
                                {"tran_ref_flag",GetRef("IOAR")},
                                {"tran_ref_gid",gid},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                                {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                {"tran_expense_category","9"},
                                {"tran_primary_branch_code",branch.ToString()},
                                // {"tran_invoice_no",ImpHeaderID.Trim().ToString()},                                
                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                                //  {"tran_maker_id",maker_id.ToString()},
                                {"tran_checker_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                {"tran_emp_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                    string Msg2 = objCommonIUD.InsertCommon(glCoulmsC, "fa_trn_ttran");

                }
                string[,] assetcol = new string[,]
	                {                                                 
                        {"imrheader_update_date",Convert.ToString(objCommonIUD.GetCurrentDate())},
                        {"imrheader_update_by",objCmnFunctions.GetLoginUserGid().ToString()},
                        {"imrheader_isremoved","Y"}                           
	                };
                string[,] assetwhCol = new string[,]
	                {
                        {"imrheader_gid",gid.ToString()}, 
	                };
                string upt1 = objCommonIUD.UpdateCommon(assetcol, assetwhCol, "fa_trn_timrheader");
                return "success";
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "error";
            }
            finally
            {

            }
        }

        private void GetOffConnection()
        {
            if (confa.State == ConnectionState.Closed)
            {
                confa.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                confa.Open();
            }
            if (confa.State == ConnectionState.Open)
            {
                //confa.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                confa.Close();
            }
        }


        public string Getempcode(string empgid)
        {
            string empid = string.Empty;
            GetConnection();
            DataTable dtdep = new DataTable();
            cmdfa = new SqlCommand("pr_ifams_trn_SaleMaker", confa);
            cmdfa.CommandType = CommandType.StoredProcedure;
            cmdfa.Parameters.AddWithValue("@action", "empcode");
            cmdfa.Parameters.AddWithValue("@empgid", empgid);
            dafa = new SqlDataAdapter(cmdfa);
            dafa.Fill(dtdep);
            foreach (DataRow rowdep in dtdep.Rows)
            {
                empid = rowdep["employee_code"].ToString();
            }
            return empid;
        }

    }
}