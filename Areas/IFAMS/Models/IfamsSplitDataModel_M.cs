using IEM.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IEM.Areas.IFAMS.Models
{
    public class IfamsSplitDataModel_M : IfamsSplitRepository_M
    {
        SqlConnection con = new SqlConnection();
        DataModel objidm = new DataModel();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions objcmnfun = new CmnFunctions();
        CommonIUD objcmnIUD = new CommonIUD();
        ErrorLog objErrorLog = new ErrorLog();
        private void GetConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }

        public IEnumerable<AssetSplitModel> GetAssetDetail(string assetdetid)
        {
            List<AssetSplitModel> objsplitM = new List<AssetSplitModel>();
            try
            {

                AssetSplitModel obj = new AssetSplitModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "assetdetselect");
                cmd.Parameters.AddWithValue("@assetdetid", assetdetid);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    //obj.locationId = Convert.ToInt32(row["assetdetails_locationcode"].ToString());
                    obj.locationId = Convert.ToInt32(row["assetdetails_branch_gid"].ToString());
                    obj.assetdetId = Convert.ToInt32(row["assetdetails_gid"].ToString());
                    obj.assetdetDetid = row["assetdetails_assetdet_id"].ToString();
                    obj.assetcategory = row["assetcategory_name"].ToString();
                    obj.assetsubcategory = row["asset_description"].ToString();
                    //obj.assetdetDescription = row["assetdetails_asset_description"].ToString();
                    obj.assetdetCode = SPAssetcode(row["assetdetails_asset_code"].ToString().Trim());
                    obj.assetdetAssetvalue = Convert.ToDecimal(row["assetdetails_asset_value"].ToString());
                    //obj.assetdetLocationcode = Convert.ToInt32(SPLocation(row["assetdetails_locationcode"].ToString().Trim()));
                    obj.assetdetLocationcode = Convert.ToInt32(SPLocation(row["assetdetails_branch_gid"].ToString().Trim()));
                    decimal assetdep = 0;
                    DataTable dtdep = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "Totdep");
                    cmd.Parameters.AddWithValue("@assetdetid", obj.assetdetId);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dtdep);
                    foreach (DataRow rowdep in dtdep.Rows)
                    {
                        assetdep = Convert.ToDecimal(rowdep["depreciation_amount"].ToString());
                    }
                    obj.Tdpreciation = assetdep;
                    objsplitM.Add(obj);
                }
                return objsplitM;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return objsplitM;
                //throw ex;
            }
            finally
            {
                con.Close();
            }

        }
        public string SPLocation(string locationcode)
        {
            string assetdetLocationcode = string.Empty;
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Location", locationcode);
                cmd.Parameters.AddWithValue("@action", "Location");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    assetdetLocationcode = row["branch_code"].ToString();

                }
                return assetdetLocationcode;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return assetdetLocationcode;
                //throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public string SPAssetcode(string assetcode)
        {
            try
            {
                List<AssetSplitModel> objModel = new List<AssetSplitModel>();
                AssetSplitModel obj = new AssetSplitModel();
                string assetcodes = " ";
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "assetcodeM");
                cmd.Parameters.AddWithValue("@assetcodes", assetcode);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    assetcodes = row["asset_code"].ToString();
                    //obj.assetdetDescription = row["asset_description"].ToString();
                    //objModel.Add(obj);
                }
                return assetcodes;
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


        public string GetAssetDetIDNew(string locid, string acode, Models.AssetSplitModel status, string splitamounts)
        {
            try
            {
                string locode = status.assetdetLocationcode.ToString();
                //string ff=status.splitamt.ToString()+","+status.splitamt1.ToString();
                var amount = splitamounts;
                string[] splitamount = amount.Split(',');
                AssetSplitModel asm = new AssetSplitModel();
                for (int j = 0; j < splitamount.Length; j++)
                {

                    string statu = "WAITING FOR APPROVAL";
                    status.statid = splGetStatus(statu);

                    //  if (j == 0)
                    // {
                    for (int i = j; i == j; i++)
                    {
                        //  asm.assetsmNassetdetId = objcmnfun.generateAssetid(locode, acode);
                        //asm.assetsmNassetdetId = objcmnfun.generateAssetid(locid, acode);
                        string newassetval = splitamount[i];
                        string[,] codes = new string[,]
                         {
                             //{"assetsplitmerge_new_assetid" , asm.assetsmNassetdetId.ToString()},
                             {"assetsplitmerge_new_assetvalue" , newassetval.ToString()},
                             {"assetsplitmerge_entry_by" ,Convert.ToString(objcmnfun.GetLoginUserGid())},
                             {"assetsplitmerge_entry_date" , Convert.ToString("SYSDATETIME()")},
                             {"assetsplitmerge_assetdet_gid" , status.assetdetId.ToString()},
                             {"assetsplitmerge_asset_value" , status.assetdetAssetvalue.ToString()},
                             {"assetsplitmerge_isremoved" , "N"},
                             {"assetsplitmerge_detail", "S"},
                             {"assetsplitmerge_status" , status.statid.ToString()},
                             {"assetsplitmerge_insert_by",Convert.ToString(objcmnfun.GetLoginUserGid())},
                             {"assetsplitmerge_insert_date",Convert.ToString("SYSDATETIME()")}

                        };
                        string tblname = "fa_trn_tassetsplitmerge";
                        string instcomm = objcmnIUD.InsertCommon(codes, tblname);
                    }
                    // }
                    //else
                    //{
                    //    for (int k = j; k == j; k++)
                    //    {

                    //        int assetid = status.assetdetId;
                    //       // asm.assetsmNassetdetId = splgenerateAssetid(locode, acode, assetid);
                    //       // asm.assetsmNassetdetId = splgenerateAssetid(locid, acode, assetid);
                    //        string newassetval1 = splitamount[j];
                    //        string[,] codes1 = new string[,]
                    //        {
                    //           // {"assetsplitmerge_new_assetid" , asm.assetsmNassetdetId.ToString()},
                    //            {"assetsplitmerge_new_assetvalue" , newassetval1.ToString()},
                    //            {"assetsplitmerge_entry_by" ,Convert.ToString(objcmnfun.GetLoginUserGid())},
                    //            {"assetsplitmerge_entry_date" , Convert.ToString("SYSDATETIME()")},
                    //            {"assetsplitmerge_assetdet_gid" , status.assetdetId.ToString()},
                    //            {"assetsplitmerge_asset_value" , status.assetdetAssetvalue.ToString()},
                    //            {"assetsplitmerge_isremoved" , "N"},
                    //            {"assetsplitmerge_detail", "S"},
                    //            {"assetsplitmerge_status" , status.statid.ToString()},
                    //            {"assetsplitmerge_insert_by",Convert.ToString(objcmnfun.GetLoginUserGid())},
                    //            {"assetsplitmerge_insert_date",Convert.ToString("SYSDATETIME()")}

                    //        };
                    //            string tblname1 = "fa_trn_tassetsplitmerge";
                    //            string instcommon = objcmnIUD.InsertCommon(codes1, tblname1);
                    //    }
                    //}
                }

                string[,] codes2 = new string[,]
                {             
                    {"assetdetails_takenby","16"} //split in proces
                };

                string[,] whrcol = new string[,]
                {
                    {"assetdetails_gid", status.assetdetId.ToString()}
                };
                string tblname2 = "fa_trn_tassetdetails";
                string updcomm = objcmnIUD.UpdateCommon(codes2, whrcol, tblname2);

                //quee
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = "SPLITCHK";
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "groupname";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                string grpid = string.Empty;
                foreach (DataRow row in dt.Rows)
                    grpid = row["rolegroup_gid"].ToString();
                string[,] col = new string[,]
                {
                                     {"queue_date",Convert.ToString(objcmnIUD.GetCurrentDate())},
                                     {"queue_ref_flag",objidm.GetRef("SPOA").ToString()},
                                     {"queue_ref_gid",  status.assetdetId.ToString()},
                                     {"queue_action_for", "A"},
                                     {"queue_from",Convert.ToString(objcmnfun.GetLoginUserGid())},
                                     {"queue_to_type","G" },
                                     {"queue_to",grpid},
                                     {"queue_prev_gid","0"},
                                     {"queue_action_status","0"},
                                     {"queue_isremoved","N"}     
                };

                string inst = objcmnIUD.InsertCommon(col, "iem_trn_tqueue");
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return string.Empty;
                // throw ex;
            }
            finally
            {
                con.Close();
            }
            return "success";
        }

        public string splgenerateAssetid(string locode, string acode, int assetid)
        {
            try
            {
                string result = string.Empty;
                GetConnection();
                if (locode.Length <= 4)
                {
                    string lsQry = "select  MAX(convert(int,substring(assetsplitmerge_new_assetid,17,len(assetsplitmerge_new_assetid)))) FROM fa_trn_tassetsplitmerge ";
                    lsQry += " where assetsplitmerge_assetdet_gid='" + assetid + "' and ";
                    lsQry += " assetsplitmerge_isremoved='N' and assetsplitmerge_status = (select status_flag from fa_mst_tstatus where status_name = 'WAITING FOR APPROVAL')";
                    cmd = new SqlCommand(lsQry, con);
                    cmd.CommandType = CommandType.Text;
                    int serialNo = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
                    result = ConfigurationManager.AppSettings["Entity"] + acode + "-" + Convert.ToInt32(locode).ToString("0000") + "0000-" + serialNo.ToString("00000");
                }
                else
                {
                    string lsQry = "select  MAX(convert(int,substring(assetsplitmerge_new_assetid,17,len(assetsplitmerge_new_assetid)))) FROM fa_trn_tassetsplitmerge ";
                    lsQry += " where assetsplitmerge_assetdet_gid='" + assetid + "' and ";
                    lsQry += " assetsplitmerge_isremoved='N' and assetsplitmerge_status = (select status_flag from fa_mst_tstatus where status_name = 'WAITING FOR APPROVAL')";
                    cmd = new SqlCommand(lsQry, con);
                    cmd.CommandType = CommandType.Text;
                    int serialNo = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
                    result = ConfigurationManager.AppSettings["Entity"] + acode + "-" + Convert.ToInt32(locode).ToString("00000000") + "-" + serialNo.ToString("00000");
                }
                return result;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return string.Empty;
                // throw e;
            }
            finally
            {
                con.Close();
            }
        }

        public string splGetStatus(string statu)
        {
            try
            {
                string result = string.Empty;
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@statusid", statu);
                cmd.Parameters.AddWithValue("@action", "status");
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    result = dt.Rows[0]["status_flag"].ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return string.Empty;
                //throw ex;
            }
            finally
            {
                con.Close();
            }
        }


        public string GetstatusNam(string statu)
        {
            try
            {
                string result = string.Empty;
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@statusid", statu);
                cmd.Parameters.AddWithValue("@action", "statu");
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    result = dt.Rows[0]["status_name"].ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return string.Empty;
                //throw ex;
            }
            finally
            {
                con.Close();
            }
        }


        public IEnumerable<AssetSplitModel> GetAssetDetailChk()
        {
            List<AssetSplitModel> objMod = new List<AssetSplitModel>();
            AssetSplitModel obj = new AssetSplitModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "SelectSPChk");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetSplitModel();
                    obj.assetdetId = Convert.ToInt32(row["assetdetails_gid"].ToString());
                    obj.assetdetDetid = row["assetdetails_assetdet_id"].ToString();
                    obj.assetcategory = row["assetcategory_name"].ToString();
                    obj.assetsubcategory = row["asset_description"].ToString();
                    //obj.assetdetDescription = row["assetdetails_asset_description"].ToString();
                    //obj.assetdetCode = SPAssetcode(row["assetdetails_asset_code"].ToString().Trim());
                    obj.assetdetCode = SPAssetcode(row["assetdetails_asset_code"].ToString());
                    obj.assetdetAssetvalue = Convert.ToDecimal(row["assetdetails_asset_value"].ToString());

                    decimal assetdep = 0;
                    DataTable dtdep = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "Totdep");
                    cmd.Parameters.AddWithValue("@assetdetid", obj.assetdetId);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dtdep);
                    foreach (DataRow rowdep in dtdep.Rows)
                    {
                        assetdep = Convert.ToDecimal(rowdep["depreciation_amount"].ToString());
                    }

                    obj.Tdpreciation = assetdep;

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


        public IEnumerable<AssetSplitModel> GetAssetDetailHp()
        {
            List<AssetSplitModel> objMod = new List<AssetSplitModel>();
            AssetSplitModel obj = new AssetSplitModel();
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
                    obj = new AssetSplitModel();
                    obj.assetdetId = Convert.ToInt32(row["assetdetails_gid"].ToString());
                    obj.assetdetDetid = row["assetdetails_assetdet_id"].ToString();
                    obj.assetdetDescription = row["assetdetails_asset_description"].ToString();
                    //obj.assetdetCode = SPAssetcode(row["assetdetails_asset_code"].ToString().Trim());
                    obj.assetdetCode = row["assetdetails_asset_code"].ToString().Trim();
                    obj.assetdetAssetvalue = Convert.ToDecimal(row["assetdetails_asset_value"].ToString());
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


        public IEnumerable<AssetSplitModel> GetSplitDetail(string id)
        {
            List<AssetSplitModel> objmod = new List<AssetSplitModel>();
            AssetSplitModel obj = new AssetSplitModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@assetdetid", id.ToString().Trim());
                cmd.Parameters.AddWithValue("@action", "Splitdet");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetSplitModel();
                    obj.assetdetId = Convert.ToInt32(row["assetdetails_gid"].ToString());
                    obj.assetdetDetid = row["assetdetails_assetdet_id"].ToString();
                    obj.assetsmNassetdetId = row["assetsplitmerge_new_assetid"].ToString();
                    obj.assetsmNassetVal = Convert.ToDecimal(row["assetsplitmerge_new_assetvalue"].ToString());
                    obj.assetcategory = row["assetcategory_name"].ToString();
                    obj.assetsubcategory = row["asset_description"].ToString();
                    // obj.assetdetDescription = row["assetdetails_asset_description"].ToString();
                    obj.assetdetCode = SPAssetcode(row["assetdetails_asset_code"].ToString());
                    obj.assetsmStatus = GetstatusNam(row["assetsplitmerge_status"].ToString());
                    obj.rectificationAmt = objidm.getRetificationAmount(row["assetdetails_gid"].ToString());
                    objmod.Add(obj);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return objmod;
            }
            finally
            {
                con.Close();
            }
            return objmod;
        }



        public IEnumerable<AssetSplitModel> GetMakSplitDetail(string id)
        {
            List<AssetSplitModel> objmod = new List<AssetSplitModel>();
            AssetSplitModel obj = new AssetSplitModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@assetdetid", id.ToString().Trim());
                cmd.Parameters.AddWithValue("@action", "Splitdetmak");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetSplitModel();
                    obj.assetdetId = Convert.ToInt32(row["assetdetails_gid"].ToString());
                    obj.assetdetDetid = row["assetdetails_assetdet_id"].ToString();
                    obj.assetsmNassetdetId = row["assetsplitmerge_new_assetid"].ToString();
                    obj.assetsmNassetVal = Convert.ToDecimal(row["assetsplitmerge_new_assetvalue"].ToString());
                    obj.assetcategory = row["assetcategory_name"].ToString();
                    obj.assetsubcategory = row["asset_description"].ToString();
                    // obj.assetdetDescription = row["assetdetails_asset_description"].ToString();
                    obj.assetdetCode = SPAssetcode(row["assetdetails_asset_code"].ToString());
                    obj.assetsmStatus = GetstatusNam(row["assetsplitmerge_status"].ToString());
                    objmod.Add(obj);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return objmod;
            }
            finally
            {
                con.Close();
            }
            return objmod;
        }

        public string AppRejAsset(string id, string chstatus, string Remarks)
        {
            try
            {
                string branchlegacycode = string.Empty;
                string oracleCCcode = string.Empty;
                string oracleBScode = string.Empty;
                string capdate = string.Empty;
                string oraclebscccode = string.Empty;
                string oraclebrlegacy = string.Empty;
                AssetSplitModel status = new AssetSplitModel();
                GetConnection();
                if (chstatus == "REJECTED")
                {

                    string statu = "REJECTED";
                    status.statid = splGetStatus(statu);
                    string[,] col = new string[,]
                    {
                       // {"assetsplitmerge_isremoved","Y"},
                        {"assetsplitmerge_status", status.statid.ToString()},
                        {"assetsplitmerge_update_by",Convert.ToString(objcmnfun.GetLoginUserGid())},
                        {"assetsplitmerge_update_date",Convert.ToString("SYSDATETIME()")}
                    };
                    string[,] whercol = new string[,]
                    {
                        {"assetsplitmerge_assetdet_gid",id.ToString()}
                    };
                    string updcmn = objcmnIUD.UpdateCommon(col, whercol, "fa_trn_tassetsplitmerge");

                    string[,] col1 = new string[,] 
                    {
                        //{"assetdetails_takenfor_split","N"}
                        {"assetdetails_takenby", "14"},
                        {"assetdetails_update_by",Convert.ToString(objcmnfun.GetLoginUserGid())},
                        {"assetdetails_update_date",Convert.ToString("SYSDATETIME()")}
                    };
                    string[,] whercol1 = new string[,]
                    {
                        {"assetdetails_gid",id.ToString()}
                    };
                    string updcmmn = objcmnIUD.UpdateCommon(col1, whercol1, "fa_trn_tassetdetails");

                    string[,] colu = new string[,]
                    {
                        {"queue_action_by",objcmnfun.GetLoginUserGid().ToString()},
                        {"queue_action_status","2"},
                        {"queue_action_remark", Remarks.ToString()},
                        {"queue_action_date",convertoDate(objcmnIUD.GetCurrentDate())},
                        {"queue_isremoved","Y"},
                    };
                    string[,] Whercolu = new string[,]
                    {
                        {"queue_ref_gid", id.Trim()},
                        {"queue_ref_flag",objidm.GetRef("SPOA").ToString()},
                        {"queue_action_status","0"}
                    };
                    string updcommn = objcmnIUD.UpdateCommon(colu, Whercolu, "iem_trn_tqueue");

                }
                else
                {
                    string statu = "WAITING FOR APPROVAL";
                    status.statid = splGetStatus(statu);
                    Int32 oldassetid = Convert.ToInt32(id.ToString());
                    GetConnection();
                    DataTable dt = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@assetdetid", id);
                    cmd.Parameters.AddWithValue("@action", "Selectassetsplitinsert");
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    //foreach (DataRow row in dt.Rows)
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string branchcode = SPLocation(dt.Rows[i]["assetdetails_branch_gid"].ToString());
                        string assetcode = SPAssetcode(dt.Rows[i]["assetdetails_asset_code"].ToString());
                        if (i == 0)
                        {
                            status.assetsmNassetdetId = objcmnfun.generateAssetid(branchcode, assetcode);
                        }
                        else
                        {
                            status.assetsmNassetdetId = splgenerateAssetid(branchcode, assetcode, oldassetid);
                        }
                        decimal a = 0;
                        a = Convert.ToInt32(dt.Rows[i]["assetsplitmerge_new_assetvalue"]);
                        string b = string.Empty;
                        string GL = string.Empty;
                       
                        //Muthu 07-12-2016
                        DataTable dta = new DataTable();
                        cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@assetcodes", assetcode);
                        cmd.Parameters.AddWithValue("@action", "SelectGL");
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dta);
                        for (int j = 0; j < dta.Rows.Count; j++ )
                        {
                            GL = dta.Rows[0]["asset_glcode"].ToString();
                        }

                            if (a <= 5000 || (a > 5000 && GL == "121010002"))
                            {
                                b = "N";
                            }
                            else { b = "Y"; }

                        string[,] codes = new string[,]
                         {
                             {"assetsplitmerge_new_assetid" , status.assetsmNassetdetId.ToString()},
                             {"assetsplitmerge_update_by",Convert.ToString(objcmnfun.GetLoginUserGid())},
                             {"assetsplitmerge_update_date",Convert.ToString("SYSDATETIME()")}
                         };
                        string[,] whercoll = new string[,]
                        {
                             {"assetsplitmerge_gid",dt.Rows[i]["assetsplitmerge_gid"].ToString()},
                             {"assetsplitmerge_assetdet_gid",id.ToString()},
                             {"assetsplitmerge_status", status.statid.ToString()}
                         };
                        string updcommn = objcmnIUD.UpdateCommon(codes, whercoll, "fa_trn_tassetsplitmerge");


                        capdate = dt.Rows[i]["assetdetails_cap_date"].ToString().ToString();

                        string[,] col1 = new string[,]
                        {
                            //{"assetdetails_assetdet_id",row["assetsplitmerge_new_assetid"].ToString()},
                            {"assetdetails_assetdet_id",status.assetsmNassetdetId.ToString()},
                                {"assetdetails_entity_gid","1"},
                                {"assetdetails_qty",dt.Rows[i]["assetdetails_qty"].ToString()},                        
                                {"assetdetails_asset_groupid",dt.Rows[i]["assetdetails_asset_groupid"].ToString()}, 
                                {"assetdetails_asset_code",dt.Rows[i]["assetdetails_asset_code"].ToString()},
                                {"assetdetails_asset_value",dt.Rows[i]["assetsplitmerge_new_assetvalue"].ToString()},
                                {"assetdetails_asset_description",dt.Rows[i]["assetdetails_asset_description"].ToString()},
                                {"assetdetails_cap_date",Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["assetdetails_cap_date"].ToString()) ?   string.Empty   : convertoDate(dt.Rows[i]["assetdetails_cap_date"].ToString()))},
                                {"assetdetails_po_number",dt.Rows[i]["assetdetails_po_number"].ToString()},                            
                                {"assetdetails_tfr_status","N"},
                                {"assetdetails_recon_reference",dt.Rows[i]["assetdetails_recon_reference"].ToString()},
                                {"assetdetails_active_status","Y"},
                                {"assetdetails_sale_status","N"},
                                {"assetdetails_not_5kcase", b.ToString()},
                                {"assetdetails_trf_value", Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["assetsplitmerge_new_assetvalue"].ToString()) ? "0.00" : dt.Rows[i]["assetsplitmerge_new_assetvalue"].ToString()) },
                                {"assetdetails_asset_owner",dt.Rows[i]["assetdetails_asset_owner"].ToString()},
                                {"assetdetails_trn_date",Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["assetdetails_trn_date"].ToString()) ?   string.Empty   : convertoDate(dt.Rows[i]["assetdetails_trn_date"].ToString()))},
                                {"assetdetails_reduced_value",Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["assetdetails_reduced_value"].ToString()) ? "0.00" : dt.Rows[i]["assetdetails_reduced_value"].ToString()) },
                                {"assetdetails_lease_startdate",Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["assetdetails_lease_startdate"].ToString()) ?   string.Empty   : convertoDate(dt.Rows[i]["assetdetails_lease_startdate"].ToString()))},
                                {"assetdetails_lease_enddate",Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["assetdetails_lease_enddate"].ToString()) ?   string.Empty   : convertoDate(dt.Rows[i]["assetdetails_lease_enddate"].ToString()))},
                                {"assetdetails_dep_onhold",dt.Rows[i]["assetdetails_dep_onhold"].ToString()},
                                {"assetdetails_spoke_asset",dt.Rows[i]["assetdetails_spoke_asset"].ToString()},
                                {"assetdetails_impair_asset",dt.Rows[i]["assetdetails_impair_asset"].ToString()},
                                {"assetdetails_dep_rate",dt.Rows[i]["assetdetails_dep_rate"].ToString()},  
                                {"assetdetails_asset_serialno",dt.Rows[i]["assetdetails_asset_serialno"].ToString()},
                                {"assetdetails_assetcode_changestatus","N"},                      
                                {"assetdetails_physical_idrelease",dt.Rows[i]["assetdetails_physical_idrelease"].ToString()},
                                {"assetdetails_isremoved","N"},                          
                                {"assetdetails_branch_gid",dt.Rows[i]["assetdetails_branch_gid"].ToString()},
                                {"assetdetails_insert_by",Convert.ToString(objcmnfun.GetLoginUserGid())},
                                {"assetdetails_insert_date","SYSDATETIME()"},          
                                {"assetdetails_assetid_source",Getsource("ASSET SPLITING").ToString()},    
                                {"assetdetails_addate","SYSDATETIME()"},
                                {"assetdetails_takenby","14"},
                                {"assetdetails_cbfnum",dt.Rows[i]["assetdetails_cbfnum"].ToString()},
                                {"assetdetails_cbf_gid",dt.Rows[i]["assetdetails_cbf_gid"].ToString()},
                                {"assetdetails_ecfnum",dt.Rows[i]["assetdetails_ecfnum"].ToString()},
                                {"assetdetails_ecf_gid",dt.Rows[i]["assetdetails_ecf_gid"].ToString()}
                                ,{"assetdetails_assetcode_changedate",Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["assetdetails_assetcode_changedate"].ToString()) ?   string.Empty   : convertoDate(dt.Rows[i]["assetdetails_assetcode_changedate"].ToString()))}
                                ,{"assetdetails_assetcode_changeid",dt.Rows[i]["assetdetails_assetcode_changeid"].ToString()}
                                ,{"assetdetails_capnew_date",Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["assetdetails_capnew_date"].ToString()) ?   string.Empty   : convertoDate(dt.Rows[i]["assetdetails_capnew_date"].ToString()))}
                                ,{"assetdetails_capold_date",dt.Rows[i]["assetdetails_capold_date"].ToString()}
                                ,{"assetdetails_invoice_gid",dt.Rows[i]["assetdetails_invoice_gid"].ToString()}
                                ,{"assetdetails_inwdetailgid",dt.Rows[i]["assetdetails_inwdetailgid"].ToString()}
                                ,{"assetdetails_inwheadergid",dt.Rows[i]["assetdetails_inwheadergid"].ToString()}                              
                                ,{"assetdetails_ponum",dt.Rows[i]["assetdetails_ponum"].ToString()}
                                ,{"assetdetails_sale_date",Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["assetdetails_sale_date"].ToString()) ?   string.Empty   : convertoDate(dt.Rows[i]["assetdetails_sale_date"].ToString()))}
                                ,{"assetdetails_sale_id",dt.Rows[i]["assetdetails_sale_id"].ToString()}
                                ,{"assetdetails_status",dt.Rows[i]["assetdetails_status"].ToString()}
                                ,{"assetdetails_tfr_date",Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["assetdetails_tfr_date"].ToString()) ?   string.Empty   : convertoDate(dt.Rows[i]["assetdetails_tfr_date"].ToString()))}
                                ,{"assetdetails_tfr_id",dt.Rows[i]["assetdetails_tfr_id"].ToString()}
                                ,{"assetdetails_trf_from",dt.Rows[i]["assetdetails_trf_from"].ToString()}
                                ,{"assetdetails_trf_reason",dt.Rows[i]["assetdetails_trf_reason"].ToString()}
                                ,{"assetdetails_trf_to",dt.Rows[i]["assetdetails_trf_to"].ToString()}
                                ,{"assetdetails_update_by",  string.Empty  }
                                ,{"assetdetails_update_date",  string.Empty  }
                                ,{"assetdetails_upld_ref",dt.Rows[i]["assetdetails_upld_ref"].ToString()}
                                ,{"assetdetails_upld_status",dt.Rows[i]["assetdetails_upld_status"].ToString()}
                                ,{"assetdetails_wrt_date",Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["assetdetails_wrt_date"].ToString()) ?   string.Empty   : convertoDate(dt.Rows[i]["assetdetails_wrt_date"].ToString()))}
                                ,{"assetdetails_wrt_id",dt.Rows[i]["assetdetails_wrt_id"].ToString()}
                                ,{"assetdetails_wrt_status",dt.Rows[i]["assetdetails_wrt_status"].ToString()}
                        };
                        string tblname = "fa_trn_tassetdetails";
                        string instcom = objcmnIUD.InsertCommon(col1, tblname);



                        //Location table entry


                        //DataTable dtloc = new DataTable();
                        //cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                        //cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("@branchgid", dt.Rows[i]["assetdetails_branch_gid"].ToString());
                        //cmd.Parameters.AddWithValue("@assetdetid", status.assetsmNassetdetId.ToString());
                        //cmd.Parameters.AddWithValue("@action", "Branchlegacy");
                        //da = new SqlDataAdapter(cmd);
                        //da.Fill(dtloc);
                        //for (int l = 0; l < dtloc.Rows.Count; l++)
                        //{
                        //    branchlegacycode = dtloc.Rows[l]["branch_legacy_code"].ToString();
                        //     oracleBScode = dtloc.Rows[l]["branch_businesssegement"].ToString();
                        //}


                        //DataTable dtcc = new DataTable();
                        //cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                        //cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("@assetdetid", status.assetsmNassetdetId.ToString());
                        //cmd.Parameters.AddWithValue("@action", "transfercc");
                        //da = new SqlDataAdapter(cmd);
                        //da.Fill(dtcc);
                        //for (int l = 0; l < dtcc.Rows.Count; l++)
                        //{
                        //    oracleCCcode = dtcc.Rows[l]["cc_code"].ToString();
                        //}

                        //string oraclebscccode =   string.Empty  ;
                        //oraclebscccode = oracleCCcode + oracleBScode;


                        DataTable dtoraclecc = new DataTable();
                        cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@assetid", id.ToString());
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dtoraclecc);
                        for (int l = 0; l < dtoraclecc.Rows.Count; l++)
                        {
                            oracleBScode = dtoraclecc.Rows[l]["assetlocation_location_fc"].ToString();
                            oracleCCcode = dtoraclecc.Rows[l]["assetlocation_location_cc"].ToString();
                            oraclebrlegacy = dtoraclecc.Rows[l]["assetlocation_location_code"].ToString();
                            oraclebscccode = dtoraclecc.Rows[l]["assetlocation_location_ccfc"].ToString();
                        }


                        //oraclebscccode = oracleCCcode + oracleBScode;
                        string[,] codeloc = new string[,]
                            {
                                {"assetlocation_asset_id",status.assetsmNassetdetId.ToString()},
                                {"assetlocation_location_code",branchlegacycode.ToString()},
                                {"assetlocation_location_ccfc",oraclebscccode.ToString()},
                                {"assetlocation_location_fc",oracleBScode.ToString()},
                                {"assetlocation_location_cc",oracleCCcode.ToString()},
                                {"assetlocation_ratio","100.00"},
                                {"assetlocation_isremoved","N"},
                                {"assetlocation_insert_by",Convert.ToString(objcmnfun.GetLoginUserGid())},
                                {"assetlocation_insert_date",Convert.ToString("SYSDATETIME()")}
                            };

                        string insertlocationcmn = objcmnIUD.InsertCommon(codeloc, "fa_trn_tassetlocation");


                        //Muthu 14-12-2016
                       // DataTable dtdep = new DataTable();
                        GetConnection();
                        cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "depreciationsplit");
                        cmd.Parameters.AddWithValue("@assetdetgid", id.ToString());
                        cmd.Parameters.AddWithValue("@newassetid", status.assetsmNassetdetId.ToString());
                        cmd.Parameters.AddWithValue("@newassetvalue", dt.Rows[i]["assetsplitmerge_new_assetvalue"].ToString());
                        cmd.ExecuteScalar();
                        //da = new SqlDataAdapter(cmd);
                       // da.Fill(dtdep);



                    }


                    string[,] col = new string[,]
                    {
                       // {"assetdetails_active_status","N"},                 
                        {"assetdetails_takenby", "18"},//Asset splited
                        {"assetdetails_update_by",Convert.ToString(objcmnfun.GetLoginUserGid())},
                        {"assetdetails_update_date",Convert.ToString("SYSDATETIME()")}
                    };
                    string[,] whercol = new string[,]
                    {
                        {"assetdetails_gid",id.ToString()}
                    };
                    string updcmn = objcmnIUD.UpdateCommon(col, whercol, "fa_trn_tassetdetails");

                    // string statu = "WAITING FOR APPROVAL";
                    status.statid = splGetStatus(statu);
                    string stat = "APPROVED";
                    status.statid = splGetStatus(stat);
                    string[,] col2 = new string[,]
                    {
                        
                        {"assetsplitmerge_status", status.statid.ToString()},
                        {"assetsplitmerge_insert_by",Convert.ToString(objcmnfun.GetLoginUserGid())},
                        {"assetsplitmerge_insert_date",Convert.ToString("SYSDATETIME()")}
                    };
                    string[,] whercol1 = new string[,]
                    {
                        {"assetsplitmerge_assetdet_gid",id.ToString()},
                        {"assetsplitmerge_status", splGetStatus(statu).ToString()}
                    };
                    string updcmmn = objcmnIUD.UpdateCommon(col2, whercol1, "fa_trn_tassetsplitmerge");

                    string[,] colu = new string[,]
                    {
                        {"queue_action_by",objcmnfun.GetLoginUserGid().ToString()},
                        {"queue_action_status","1"},
                        {"queue_action_remark", Remarks.ToString()},
                        {"queue_action_date",convertoDate(objcmnIUD.GetCurrentDate())},
                        {"queue_isremoved","Y"},
                    };
                    string[,] whercolu = new string[,]
                    {
                        {"queue_ref_gid", id.Trim()},
                        {"queue_ref_flag",objidm.GetRef("SPOA").ToString()},
                        {"queue_action_status","0"}
                    };
                    string updco = objcmnIUD.UpdateCommon(colu, whercolu, "iem_trn_tqueue");

                    string newasset = string.Empty;

                    DataTable dtnewas = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "newsplitdet");
                    cmd.Parameters.AddWithValue("@assetdetid", id.ToString());
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dtnewas);
                    foreach (DataRow row in dtnewas.Rows)
                    {
                        newasset = row["assetsplitmerge_new_assetid"].ToString();

                        string soacaptilisationdat = Convert.ToString("SYSDATETIME()");
                        DateTime soacaptilisationda = DateTime.Now;
                        DateTime dtTillDate = DateTime.Now;
                        decimal AssetValue = 0;
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
                        string grpid = "0";
                        Int32 newassetid = 0;
                        // Int32 assetdetid = obj.assetdetgid;
                        decimal sumassetdep = 0;
                        //decimal linewdv = 0;
                        decimal rectificationamt = 0;
                        // decimal linePL = 0;


                        DataTable dtwdv = new DataTable();
                        cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "WDV");
                        cmd.Parameters.AddWithValue("@assetdetid", newasset);
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dtwdv);


                        foreach (DataRow row1 in dtwdv.Rows)
                        {
                            dtBranchLaunch = Convert.ToDateTime(string.IsNullOrEmpty(row1["branch_start_date"].ToString()) ? "0" : row1["branch_start_date"]);
                            soacaptilisationda = Convert.ToDateTime(string.IsNullOrEmpty(row1["assetdetails_cap_date"].ToString()) ? "01-01-1900" : row1["assetdetails_cap_date"]);
                            AssetValue = Convert.ToDecimal(string.IsNullOrEmpty(row1["assetdetails_asset_value"].ToString()) ? "0" : row1["assetdetails_asset_value"]);
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
                            grpid = string.IsNullOrEmpty(row1["assetdetails_asset_groupid"].ToString()) ? "0" : row1["assetdetails_asset_groupid"].ToString();
                            newassetid = Convert.ToInt32(row1["assetdetails_gid"].ToString());
                            // CanDepreciateFully = Convert.ToString(string.IsNullOrEmpty(row1["h_ficrefnumber"].ToString()) ? "0" : row1["h_ficrefnumber"]);
                        }












                        //************** dep on Month start*******************************************



                        //decimal totalDepAmt = 0;
                        //decimal CurrentDepAmt = 0;
                        //decimal SameDepAmount = 0;
                        //string sAssetGroupId = string.Empty;
                        //string lsPreviousCapDate = string.Empty;
                        //decimal dAsssetValue = 0;
                        //string sTransferID = string.Empty;
                        //decimal dTransferValue = 0;
                        //string tilldate = string.Empty;
                        //List<Depreciation> _Dep_list = new List<Depreciation>();
                        //DateTime dtcheck = Convert.ToDateTime(capdate);
                        //tilldate = "01-" + objidm.Format(DateTime.Today.ToShortDateString());
                        //DateTime date1 = dtcheck;
                        //DateTime date2 = Convert.ToDateTime(tilldate);
                        //int iMonthCount = ((date1.Year - date2.Year) * 12) + date1.Month - (date2.Month-1);
                        //if (iMonthCount.ToString().Contains("-") == true)
                        //{
                        //    iMonthCount = (iMonthCount * -1) + 1;
                        //}
                        //else
                        //{
                        //    iMonthCount = (iMonthCount + 1);
                        //}
                        //// objidm.UpdateReversals();
                        //for (int i = 1; i <= iMonthCount; i++)
                        //{

                        //    DataTable dtdep = new DataTable();
                        //    cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                        //    cmd.CommandType = CommandType.StoredProcedure;
                        //    cmd.Parameters.AddWithValue("@action", "Totdep");
                        //    cmd.Parameters.AddWithValue("@assetgid", newassetid);
                        //    da = new SqlDataAdapter(cmd);
                        //    da.Fill(dtdep);
                        //    foreach (DataRow rowdep in dtdep.Rows)
                        //    {
                        //        sumassetdep = Convert.ToDecimal(rowdep["depreciation_amount"].ToString());
                        //    }
                        //    if (string.IsNullOrEmpty(soacaptilisationda.ToShortDateString()))
                        //    {
                        //        soacaptilisationda = Convert.ToDateTime("01-01-1900");
                        //    }
                        //    int lastday = DateTime.DaysInMonth(dtcheck.Year, dtcheck.Month);
                        //    DateTime daycount1 = Convert.ToDateTime(lastday + objidm.Format(dtcheck.ToString()));
                        //    tilldate = daycount1.ToString();
                        //    DateTime datecheckend = Convert.ToDateTime("01-" + objidm.Format(DateTime.Today.ToShortDateString()));
                        //    int lastday2 = DateTime.DaysInMonth(datecheckend.Year, datecheckend.Month);
                        //    DateTime daycount2 = Convert.ToDateTime(lastday2 + objidm.Format(DateTime.Today.ToShortDateString()));
                        //    if (daycount1 > daycount2)
                        //    {
                        //        tilldate = DateTime.Today.ToShortDateString();
                        //    }

                        //    totalDepAmt = objidm.GetTotalDep(Convert.ToDateTime(soacaptilisationda), Convert.ToDateTime(tilldate),
                        //                                AssetValue, sAssetCode, cNot5000Case,
                        //                                sBranch1, string.Empty, Convert.ToDateTime(dtBranchLaunch),
                        //                                Convert.ToDateTime(dtLeaseStart), Convert.ToDateTime(dtLeaseEnd)
                        //                                , sDepType, dDepRate, sAssetGroup, sPONumber
                        //                                , sFICCRef, sAsset_GroupId, dTransfer_value,
                        //                                  string.Empty, 0);


                        //    CurrentDepAmt = totalDepAmt - sumassetdep;
                        //    if (totalDepAmt > AssetValue)
                        //    {
                        //        CurrentDepAmt = AssetValue;
                        //    }
                        //    SameDepAmount = totalDepAmt;
                        //    sAssetGroupId = sAsset_GroupId;
                        //    dAsssetValue = AssetValue;
                        //    dTransferValue = dTransfer_value;
                        //    lsPreviousCapDate = soacaptilisationda.ToShortDateString();
                        //    if (Math.Round(CurrentDepAmt) > 0 && sumassetdep > 0)
                        //    {
                        //        CurrentDepAmt = Math.Round(CurrentDepAmt, 2);
                        //        string dte = objcmnfun.convertoDateTimeString(dtcheck.ToString());
                        //        //if (dep._Dep_list[loop]._dep_type == "SLM")
                        //        //{
                        //        //    dep._Dep_list[loop]._dep_type = "S";
                        //        //}
                        //        //if (dep._Dep_list[loop]._dep_type == "LPM")
                        //        //{
                        //        //    dep._Dep_list[loop]._dep_type = "L";
                        //        //}
                        //        objidm.InsertDep(newasset, sAsset_GroupId, CurrentDepAmt, dte, "S");
                        //        //ifr.InsertDep(dep._Dep_list[loop]._asset_id, dep._Dep_list[loop]._group_id, CurrentDepAmt, ifr.Format("01-12-2014"));
                        //    }

                        //    dtcheck = dtcheck.AddMonths(1);

                        //}

                        //END of Dep Monthentry


                        //    rectificationamt = depamt - sumassetdep;

                        //    if (sDepType == "SLM" || sDepType ==   string.Empty  )
                        //    {
                        //        sDepType = "S";
                        //    }
                        //    else if (sDepType == "LPM")
                        //    {
                        //        sDepType = "L";
                        //    }
                        //    string[,] codesdep = new string[,]
                        //{
                        //    {"depreciation_amount", depamt.ToString()},
                        //    {"depreciation_assetdet_gid",newassetid.ToString()},
                        //    {"depreciation_month", Convert.ToString("SYSDATETIME()")},
                        //    {"depreciation_asset_groupid", grpid.ToString()},
                        //    {"depreciation_asset_owner", "F"},
                        //    {"depreciation_type",sDepType.ToString()},
                        //    {"depreciation_insert_by",Convert.ToString(objcmnfun.GetLoginUserGid())},
                        //    {"depreciation_insert_date", Convert.ToString("SYSDATETIME()")}
                        //};
                        //    string insertcmn = objcmnIUD.InsertCommon(codesdep, "fa_trn_tdepreciation");




                        ////********************************ENTRY FOR GL UPLOAD**********************************************************
                        //cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                        //cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "UploadDetails";
                        //cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = newassetid.ToString();
                        //da = new SqlDataAdapter(cmd);
                        //string glCode =   string.Empty  ;
                        //string depglCode =   string.Empty  ;
                        //string resdepglCode =   string.Empty  ;
                        //string branch =   string.Empty  ;
                        //string cat =   string.Empty  ;
                        //string subcat =   string.Empty  ;
                        //string asset_id =   string.Empty  ;
                        //DataTable UPLOADDATATBL = new DataTable();
                        //da.Fill(UPLOADDATATBL);
                        //if (UPLOADDATATBL.Rows.Count > 0)
                        //    foreach (DataRow rowdep in UPLOADDATATBL.Rows)
                        //    {
                        //        glCode = UPLOADDATATBL.Rows[0]["asset_glcode"].ToString();
                        //        resdepglCode = UPLOADDATATBL.Rows[0]["asset_dep_reservglcode"].ToString();
                        //        depglCode = UPLOADDATATBL.Rows[0]["asset_dep_glcode"].ToString();
                        //        branch = UPLOADDATATBL.Rows[0]["branch_code"].ToString();
                        //        cat = UPLOADDATATBL.Rows[0]["assetcategory_name"].ToString();
                        //        if (cat.Length > 16)
                        //            cat = cat.Substring(0, 16);
                        //        subcat = UPLOADDATATBL.Rows[0]["asset_code"].ToString();
                        //        asset_id = UPLOADDATATBL.Rows[0]["assetdetails_assetdet_id"].ToString();
                        //    }
                        //string BS =   string.Empty  ;
                        //string CC =   string.Empty  ;
                        //cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                        //cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "BSCC_upload_details";
                        //cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = 42;
                        //cmd.Parameters.Add("@fccc", SqlDbType.VarChar).Value = branch;
                        //da = new SqlDataAdapter(cmd);
                        //UPLOADDATATBL = new DataTable();
                        //da.Fill(UPLOADDATATBL);
                        //if (UPLOADDATATBL.Rows.Count > 0)
                        //    foreach (DataRow rowdep in UPLOADDATATBL.Rows)
                        //    {
                        //        BS = UPLOADDATATBL.Rows[0]["depreciationbscc_bs"].ToString();
                        //        CC = UPLOADDATATBL.Rows[0]["depreciationbscc_cc_oracle"].ToString();
                        //    }
                        //string[,] glCoulmsD = new string[,]
                        //    {                                
                        //        {"tran_date",convertoDate(DateTime.Today.ToShortDateString()).ToString()},
                        //        {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                        //        {"tran_desc","SPLIT OF ASSET - " + asset_id.ToString() },
                        //        {"tran_gl_no",glCode},
                        //        {"tran_amount",AssetValue.ToString()},
                        //        {"tran_acc_mode","D".ToString()},
                        //        {"tran_mult","-1"},  
                        //        {"tran_fc_code",BS},
                        //        {"tran_cc_code",CC},  
                        //        {"tran_product_code","999".ToString()},
                        //        {"tran_ou_code",branch},
                        //        {"tran_ref_flag",objidm.GetRef("SPOA")},
                        //        {"tran_ref_gid", newassetid.ToString()},
                        //        {"tran_upload_gid","0".ToString()},
                        //        {"tran_isremoved","N"},
                        //        {"tran_main_cat","1"},
                        //        {"tran_sub_cat",subcat},
                        //        {"tran_expense_category",cat},
                        //        {"tran_primary_branch_code",branch.ToString()},                           
                        //        {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                        //        {"tran_maker_id",objcmnfun.GetLoginUserGid().ToString()},
                        //        {"tran_checker_id",objcmnfun.GetLoginUserGid().ToString()}
                        //    };
                        //string insertforglD = objcmnIUD.InsertCommon(glCoulmsD, "iem_trn_ttran");
                        //string[,] glCoulmsC = new string[,]
                        //    {                                
                        //        {"tran_date",convertoDate(DateTime.Today.ToShortDateString()).ToString()},
                        //        {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                        //        {"tran_desc","SPLIT OF ASSET - " + asset_id.ToString() },
                        //        {"tran_gl_no",glCode},
                        //        {"tran_amount",AssetValue.ToString()},
                        //        {"tran_acc_mode","C".ToString()},
                        //        {"tran_mult","1"},  
                        //        {"tran_fc_code",BS},
                        //        {"tran_cc_code",CC},                                
                        //        {"tran_ou_code",branch},
                        //        {"tran_product_code","999".ToString()},
                        //        {"tran_ref_flag",objidm.GetRef("SPOA")},
                        //        {"tran_ref_gid",  newassetid.ToString()},
                        //        {"tran_upload_gid","0".ToString()},
                        //        {"tran_isremoved","N"},
                        //        {"tran_main_cat","1"},
                        //        {"tran_sub_cat",subcat},
                        //        {"tran_expense_category",  string.Empty  .ToString()},
                        //        {"tran_primary_branch_code",branch.ToString()},                           
                        //        {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                        //        {"tran_maker_id",objcmnfun.GetLoginUserGid().ToString()},
                        //        {"tran_checker_id",objcmnfun.GetLoginUserGid().ToString()}
                        //    };
                        //string insertforglC = objcmnIUD.InsertCommon(glCoulmsC, "iem_trn_ttran");
                        //string[,] glCoulmsDepD = new string[,]
                        //    {                             
                        //        {"tran_date",convertoDate(DateTime.Today.ToShortDateString()).ToString()},
                        //        {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                        //        {"tran_gl_no",resdepglCode},
                        //        {"tran_desc","SPLIT OF ASSET - " + asset_id.ToString()},
                        //        {"tran_amount",depamt.ToString()},
                        //        {"tran_acc_mode","D".ToString()},
                        //        {"tran_mult","-1"},  
                        //        {"tran_fc_code",BS},
                        //        {"tran_cc_code",CC},
                        //        {"tran_ou_code",branch},
                        //        {"tran_product_code","999".ToString()},
                        //        {"tran_ref_flag",objidm.GetRef("DEP")},
                        //        {"tran_ref_gid",newassetid.ToString()},
                        //        {"tran_upload_gid","0".ToString()},
                        //        {"tran_isremoved","N"},
                        //        {"tran_main_cat","1"},
                        //        {"tran_sub_cat",subcat},
                        //        {"tran_expense_category",cat},
                        //        {"tran_primary_branch_code",branch.ToString()},                                 
                        //        {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                        //        {"tran_maker_id",objcmnfun.GetLoginUserGid().ToString()},
                        //        {"tran_checker_id",objcmnfun.GetLoginUserGid().ToString()}
                        //    };
                        //string insertforglDepD = objcmnIUD.InsertCommon(glCoulmsDepD, "iem_trn_ttran");
                        //string[,] glCoulmsDepC = new string[,]
                        //    {                                
                        //        {"tran_date",convertoDate(DateTime.Today.ToShortDateString()).ToString()},
                        //        {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                        //        {"tran_desc","SPLIT OF ASSET - " + asset_id.ToString()},
                        //        {"tran_gl_no",depglCode},
                        //        {"tran_amount",depamt.ToString()},
                        //        {"tran_acc_mode","C".ToString()},
                        //        {"tran_mult","1"},  
                        //        {"tran_fc_code",BS},
                        //        {"tran_cc_code",CC},      
                        //        {"tran_product_code","999".ToString()},
                        //        {"tran_ou_code",branch},
                        //        {"tran_ref_flag",objidm.GetRef("DEP")},
                        //        {"tran_ref_gid", newassetid.ToString()},
                        //        {"tran_upload_gid","0".ToString()},
                        //        {"tran_isremoved","N"},
                        //        {"tran_main_cat","1"},
                        //        {"tran_sub_cat",subcat},
                        //        {"tran_expense_category",cat},
                        //        {"tran_primary_branch_code",branch.ToString()},                                                        
                        //        {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                        //        {"tran_maker_id",objcmnfun.GetLoginUserGid().ToString()},
                        //        {"tran_checker_id",objcmnfun.GetLoginUserGid().ToString()}
                        //    };
                        //string insertforglDepC = objcmnIUD.InsertCommon(glCoulmsDepC, "iem_trn_ttran");



                        //tran table insert for transfer

                        if (rectificationamt < 0)
                        {
                            rectificationamt = Math.Abs(rectificationamt);
                        }


                    }
                    //GetConnection();
                    //cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@action", "deletedepreciation");
                    //cmd.Parameters.AddWithValue("@assetid", id.ToString());
                    //cmd.ExecuteNonQuery();

                    //   }
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
            return "success";
        }

        public string Getsource(string source)
        {
            string result = string.Empty;
            try
            {
                GetConnection();

                cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@sourceNam", source);
                cmd.Parameters.AddWithValue("@action", "source");
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result = dt.Rows[0]["assetsource_flag"].ToString();
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

        public IEnumerable<AssetSplitModel> GetSplit(string loginid)
        {
            List<AssetSplitModel> objmod = new List<AssetSplitModel>();
            AssetSplitModel obj = new AssetSplitModel();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Makerid", loginid);
                cmd.Parameters.AddWithValue("@action", "selectsplit");
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetSplitModel();
                    obj.assetdetId = Convert.ToInt32(row["assetdetails_gid"].ToString());
                    obj.assetdetDetid = row["assetdetails_assetdet_id"].ToString();
                    // obj.assetdetDescription = row["assetdetails_asset_description"].ToString();
                    obj.assetcategory = row["assetcategory_name"].ToString();
                    obj.assetsubcategory = row["asset_description"].ToString();
                    obj.assetdetCode = SPAssetcode(row["assetdetails_asset_code"].ToString().Trim());
                    obj.assetdetAssetvalue = Convert.ToDecimal(row["assetdetails_asset_value"].ToString());
                    //  obj.assetsmStatus = GetstatusNam(row["assetsplitmerge_status"].ToString());
                    objmod.Add(obj);

                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return objmod;
            }
            finally
            {
                con.Close();
            }
            return objmod;
        }

        public List<AssetSplitModel> spPopulateAuditTrail(AssetSplitModel sapat)
        {
            List<AssetSplitModel> objSAAuditTrail = new List<AssetSplitModel>();
            try
            {
                AssetSplitModel objSAModel;
                GetConnection();
                DataTable dt = new DataTable();
                string safstEmp = string.Empty;
                string sastatus = string.Empty;
                cmd = new SqlCommand("pr_ifams_trn_audittaril", con);
                cmd.Parameters.AddWithValue("@gid", sapat.assetdetId);
                cmd.Parameters.AddWithValue("@refflag", "SPOA");
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string sapergid = Convert.ToString(dt.Rows[i]["queue_prev_gid"].ToString());
                        if (sapergid == "0")
                        {
                            safstEmp = Convert.ToString(dt.Rows[i]["queue_from"].ToString());
                            string saempcodenamer = objidm.Getempname(safstEmp);
                            string[] sadatar;
                            sadatar = saempcodenamer.Split(',');
                            objSAModel = new AssetSplitModel();
                            objSAModel.employee_code = sadatar[0].ToString() + " - " + sadatar[1].ToString();
                            objSAModel.action_status = "Submitted";
                            objSAModel.action_date = Convert.ToString(convertoDate(dt.Rows[i]["queue_date"].ToString()));
                            objSAModel.queue_to_type = " SPLIT Maker";
                            objSAAuditTrail.Add(objSAModel);
                            string saactions = Convert.ToString(dt.Rows[i]["queue_action_by"].ToString());
                            if (saactions == string.Empty)
                            {
                                string saqueue_type = Convert.ToString(dt.Rows[i]["queue_to_type"].ToString());
                                string saqueue_to = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
                                string actionssa = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
                                if (actionssa != string.Empty)
                                {
                                    objSAModel = new AssetSplitModel();
                                    objSAModel.employee_code = string.Empty;
                                    objSAModel.action_date = Convert.ToString(convertoDate(dt.Rows[i]["queue_action_date"].ToString()));
                                    sastatus = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
                                    objSAModel.action_status = objidm.GetQueueStatusHistry(sastatus);
                                    objSAModel.queue_to_type = objidm.GetQueueRole(saqueue_type, saqueue_to);
                                    objSAModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());
                                    objSAAuditTrail.Add(objSAModel);
                                }
                            }
                            else
                            {
                                string saempid = "'";
                                string saqueue_type = Convert.ToString(dt.Rows[i]["queue_to_type"].ToString());
                                string saqueue_to = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
                                string actionsa = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
                                if (actionsa != string.Empty)
                                {
                                    objSAModel = new AssetSplitModel();
                                    if (saqueue_type == "G" || saqueue_type == "R" || saqueue_type == "L" || saqueue_type == "D")
                                    {
                                        saempid = Convert.ToString(dt.Rows[i]["queue_action_by"].ToString());
                                    }
                                    if (saempid != string.Empty)
                                    {
                                        string saempcodname = objidm.Getempname(saempid);
                                        string[] sadata;
                                        sadata = saempcodname.Split(',');
                                        objSAModel.employee_code = sadata[0].ToString() + " - " + sadata[1].ToString();
                                    }
                                    objSAModel.action_date = Convert.ToString(convertoDate(dt.Rows[i]["queue_action_date"].ToString()));
                                    sastatus = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
                                    objSAModel.action_status = objidm.GetQueueStatusHistry(sastatus);
                                    objSAModel.queue_to_type = objidm.GetQueueRole(saqueue_type, saqueue_to);
                                    objSAModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());
                                    objSAAuditTrail.Add(objSAModel);
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
            return objSAAuditTrail;
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


        public List<AssetSplitModel> SPAutoAssetsummary(string term)
        {
            List<AssetSplitModel> Model = new List<AssetSplitModel>();
            AssetSplitModel obj = new AssetSplitModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "AutoAssetdetDetid";
                cmd.Parameters.Add("@assetdetid", SqlDbType.VarChar).Value = term.Trim();
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetSplitModel();
                    obj.assetdetDetid = row["assetdetails_assetdet_id"].ToString();
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

        public List<AssetSplitModel> SPAutoAsset(string term, string loginid)
        {
            List<AssetSplitModel> Model = new List<AssetSplitModel>();
            AssetSplitModel obj = new AssetSplitModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "splitAutoAssetdetDetid";
                cmd.Parameters.Add("@assetdetid", SqlDbType.VarChar).Value = term;
                cmd.Parameters.AddWithValue("@Makerid", loginid);
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetSplitModel();
                    obj.assetdetDetid = row["assetdetails_assetdet_id"].ToString();
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

        public List<AssetSplitModel> SPAutoAssetChk(string term)
        {
            List<AssetSplitModel> Model = new List<AssetSplitModel>();
            AssetSplitModel obj = new AssetSplitModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "splitAutoAssetdetDetidChk";
                cmd.Parameters.Add("@assetdetid", SqlDbType.VarChar).Value = term;
                // cmd.Parameters.AddWithValue("@Makerid", loginid);
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetSplitModel();
                    obj.assetdetDetid = row["assetdetails_assetdet_id"].ToString();
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


    }
}