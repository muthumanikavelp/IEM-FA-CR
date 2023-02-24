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
    public class IfamsAssetVRDataModel_M : IfamsAssetVRRepository_M
    {
        SqlConnection con = new SqlConnection();
        DataModel objidm = new DataModel();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions objcmnfunc = new CmnFunctions();
        CommonIUD objcmnIUD = new CommonIUD();
        ErrorLog objErrorLog = new ErrorLog();
        private void GetConnection()
        {
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }

        public IEnumerable<AssetVRModel> GetMakerReduction(string usergid)
        {

            List<AssetVRModel> objvrModel = new List<AssetVRModel>();
            AssetVRModel obj = new AssetVRModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_ValueReduction", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "VRdetail");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetVRModel();
                    obj.assetdetId = Convert.ToInt32(row["assetreduction_id"].ToString());
                    obj.assetdetDetid = row["Oldassetdetails_gid"].ToString();
                    obj.assetreduval = Convert.ToDecimal(row["Original_Value"].ToString());
                    obj.assetnewval = row["New_Value"].ToString();
                    obj.assetreducedamt = row["Reduced_value"].ToString();
                    obj.assetrefno = row["assetreduction_reference_no"].ToString();
                    //obj.assetdetAssetvalue = Convert.ToInt32(row["assetreduction_reduced_date"].ToString());
                    obj.gid = Convert.ToInt32(row["assetdetails_gid"].ToString());                    
                    objvrModel.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw ex;

            }
            finally
            {
                con.Close();
            }
            return objvrModel;
        }

        public IEnumerable<AssetVRModel> GetVRDetail()
        {
            try
            {
                List<AssetVRModel> objvrModel = new List<AssetVRModel>();
                AssetVRModel obj = new AssetVRModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_ValueReduction", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "vrselect");
                //cmd.Parameters.AddWithValue("@Grpid", grpid);
                //cmd.Parameters.AddWithValue("@Assetid" , assetdetid);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                int f = dt.Rows.Count;
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetVRModel();
                    // obj.assetdetGroupid = Convert.ToInt32( "0".ToString());
                    // obj.assetdetGroupid = Convert.ToInt32(row["assetdetails_asset_groupid"].ToString());
                    obj.assetdetId = Convert.ToInt32(row["assetdetails_gid"].ToString());
                    obj.assetdetGroupid = Getgroup(row["assetdetails_asset_groupid"].ToString());
                    obj.assetdetDetid = row["assetdetails_assetdet_id"].ToString();
                    obj.assetcategory = row["assetcategory_name"].ToString();
                    obj.assetsubcategory = row["asset_description"].ToString();
                    obj.branchgid = row["assetdetails_branch_gid"].ToString();
                    obj.branchcode = row["branch_code"].ToString();
                    //obj.assetdetDescription = row["assetdetails_asset_description"].ToString();
                    obj.assetdetCode = VRAssetcode(row["assetdetails_asset_code"].ToString());
                    obj.assetdetAssetvalue = Convert.ToDecimal(row["assetdetails_asset_value"].ToString());
                    objvrModel.Add(obj);
                }
                return objvrModel;
            }
            catch (Exception ex)
            {
                //objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public IEnumerable<AssetVRModel> GetVRDetailsearch(string asset, string location)
        {
            try
            {
                List<AssetVRModel> objvrModel = new List<AssetVRModel>();
                AssetVRModel obj = new AssetVRModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_ValueReduction", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "vrselectsearch");
                cmd.Parameters.AddWithValue("@assetdetDetid", asset);
                cmd.Parameters.AddWithValue("@assetbranchcode", location);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                int f = dt.Rows.Count;
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetVRModel();
                    // obj.assetdetGroupid = Convert.ToInt32( "0".ToString());
                    // obj.assetdetGroupid = Convert.ToInt32(row["assetdetails_asset_groupid"].ToString());
                    obj.assetdetId = Convert.ToInt32(row["assetdetails_gid"].ToString());
                    obj.assetdetGroupid = Getgroup(row["assetdetails_asset_groupid"].ToString());
                    obj.assetdetDetid = row["assetdetails_assetdet_id"].ToString();
                    obj.assetcategory = row["assetcategory_name"].ToString();
                    obj.assetsubcategory = row["asset_description"].ToString();
                    obj.branchgid = row["assetdetails_branch_gid"].ToString();
                    obj.branchcode = row["branch_code"].ToString();
                    //obj.assetdetDescription = row["assetdetails_asset_description"].ToString();
                    obj.assetdetCode = VRAssetcode(row["assetdetails_asset_code"].ToString());
                    obj.assetdetAssetvalue = Convert.ToDecimal(row["assetdetails_asset_value"].ToString());
                    objvrModel.Add(obj);
                }
                return objvrModel;
            }
            catch (Exception ex)
            {
                //objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }


        public string VRAssetcode(string assetcode)
        {
            try
            {
                List<AssetVRModel> objModel = new List<AssetVRModel>();
                AssetVRModel obj = new AssetVRModel();
                string assetcodes = "";
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_ValueReduction", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "assetcode");
                cmd.Parameters.AddWithValue("@Assetcode", assetcode);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {

                    assetcodes = row["asset_code"].ToString();
                }
                return assetcodes;
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

        public string Getgroup(string groupid)
        {
            try
            {
                List<AssetVRModel> objModel = new List<AssetVRModel>();
                AssetVRModel obj = new AssetVRModel();
                string grpid = "";
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_ValueReduction", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "groupid");
                cmd.Parameters.AddWithValue("@groupid", groupid);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    grpid = row["goaheader_groupid"].ToString();
                }
                return grpid;
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

        public string Updateasset(Models.AssetVRModel status)
        {
            try
            {
                GetConnection();

                string[,] codes = new string[,]
                {
                    {"assetreduction_assetdet_id",status.assetdetId.ToString()},
                    {"assetreduction_asset_value",status.assetdetAssetvalue.ToString()},
                    {"assetreduction_new_assetvalue",(status.assetdetAssetvalue - status.assetreduval).ToString()},
                    {"assetreduction_reducedamount", status.assetreduval.ToString()},
                    {"assetreduction_reference_no", objcmnfunc.GetSequenceNoFam("AVR").ToString()},
                    {"assetreduction_reduced_date",Convert.ToString("SYSDATETIME()")},
                    {"assetreduction_isremoved","N"},
                    {"assetreduction_status",Getstatus("WAITING FOR APPROVAL").ToString()},
                    {"assetreduction_insert_by",Convert.ToString(objcmnfunc.GetLoginUserGid())},
                    {"assetreduction_insert_date",Convert.ToString("SYSDATETIME()")}
                };
                string tblname = "fa_trn_tassetreduction";
                string instcomm = objcmnIUD.InsertCommon(codes, tblname);
                string[,] codes1 = new string[,]
                {
                    //{"assetdetails_asset_value",status.assetnewval.ToString()},
                   // {"assetdetails_reduced_value",status.assetreduval.ToString()},
                    //{"assetdetails_reduced_value",newassetreduval.ToString()},
                   // {"assetdetails_trf_value",status.assetnewval.ToString()},
                    {"assetdetails_update_by",Convert.ToString(objcmnfunc.GetLoginUserGid())},
                    {"assetdetails_update_date",Convert.ToString(objcmnIUD.GetCurrentDate())},
                    {"assetdetails_takenby","20"}
                };
                string[,] wher = new string[,]
                {
                    {"assetdetails_gid",status.assetdetId.ToString()},
                    {"assetdetails_takenby","14"}
                    
                };
                string tblnam = "fa_trn_tassetdetails";
                string updatecmn = objcmnIUD.UpdateCommon(codes1, wher, tblnam);
                decimal oldassetreduval = 0;
                decimal newassetreduval = 0;
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_ValueReduction", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "assetdetgid");
                cmd.Parameters.AddWithValue("@Assetid", status.assetdetId.ToString());
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    oldassetreduval = Convert.ToDecimal(row["assetdetails_reduced_value"].ToString());
                }
                newassetreduval = oldassetreduval + status.assetreduval;


                //quee
                DataTable dtq = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = "AVRCHK";
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "groupname";
                da = new SqlDataAdapter(cmd);
                da.Fill(dtq);
                string grpid = "";
                foreach (DataRow row in dtq.Rows)
                    grpid = row["rolegroup_gid"].ToString();
                string[,] col = new string[,]
                {
                                     {"queue_date",Convert.ToString(objcmnIUD.GetCurrentDate())},
                                     {"queue_ref_flag",objidm.GetRef("AVR").ToString()},
                                     {"queue_ref_gid",  status.assetdetId.ToString()},
                                     {"queue_action_for", "A"},
                                     {"queue_from",Convert.ToString(objcmnfunc.GetLoginUserGid())},
                                     {"queue_to_type","G" },
                                     {"queue_to",grpid},
                                     {"queue_prev_gid","0"},
                                     {"queue_action_status","0"},
                                     {"queue_isremoved","N"}     
                };

                string inst = objcmnIUD.InsertCommon(col, "iem_trn_tqueue");


                

                //string newasset = "";
                //DataTable dtnewas = new DataTable();
                //     cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                //     cmd.CommandType = CommandType.StoredProcedure;
                //     cmd.Parameters.AddWithValue("@action", "newsplitdet");
                //     cmd.Parameters.AddWithValue("@assetdetid", id.ToString());
                //     da = new SqlDataAdapter(cmd);
                //     da.Fill(dtnewas);
                //     foreach (DataRow row in dtnewas.Rows)
                //     {
                //newasset = row["assetsplitmerge_new_assetid"].ToString();

                //        string soacaptilisationdat = Convert.ToString("SYSDATETIME()");
                //        DateTime soacaptilisationda = DateTime.Now;
                //        DateTime dtTillDate = DateTime.Now;
                //        decimal AssetValue = 0;
                //        string sAssetCode = "";
                //        string cNot5000Case = "";
                //        string sBranch1 = "";
                //        string sBranch2 = "";
                //        DateTime dtBranchLaunch = DateTime.Now;
                //        DateTime dtLeaseStart = DateTime.Now;
                //        DateTime dtLeaseEnd = DateTime.Now;
                //        string sDepType = "";
                //        decimal dDepRate = 0;
                //        string sAssetGroup = "";
                //        string sPONumber = "";
                //        string sFICCRef = "";
                //        string sAsset_GroupId = "";
                //        decimal dTransfer_value = 0;
                //        string CanDepreciateFully = "";
                //        Int32 dDepDevRate = 0;
                //        Int64 grpid = 0;
                //        Int32 newassetid = 0;
                //        // Int32 assetdetid = obj.assetdetgid;
                //        // decimal sumassetdep = 0;
                //        //decimal linewdv = 0;
                //        //decimal rectificationamt = 0;
                //        // decimal linePL = 0;


                //        DataTable dtwdv = new DataTable();
                //        cmd = new SqlCommand("pr_ifams_trn_ValueReduction", con);
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Parameters.AddWithValue("@action", "WDV");
                //        cmd.Parameters.AddWithValue("@Assetid", status.assetdetId.ToString());
                //        da = new SqlDataAdapter(cmd);
                //        da.Fill(dtwdv);


                //        foreach (DataRow row1 in dtwdv.Rows)
                //        {
                //            dtBranchLaunch = Convert.ToDateTime(string.IsNullOrEmpty(row1["branch_start_date"].ToString()) ? "0" : row1["branch_start_date"]);
                //            soacaptilisationda = Convert.ToDateTime(string.IsNullOrEmpty(row1["assetdetails_cap_date"].ToString()) ? "01-01-1900" : row1["assetdetails_cap_date"]);
                //            AssetValue = Convert.ToDecimal(string.IsNullOrEmpty(row1["assetdetails_asset_value"].ToString()) ? "0" : row1["assetdetails_asset_value"]);
                //            sAssetCode = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_asset_code"].ToString()) ? "0" : row1["assetdetails_asset_code"]);
                //            cNot5000Case = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_not_5kcase"].ToString()) ? "0" : row1["assetdetails_not_5kcase"]);
                //            //sBranch1 = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_not_5kcase"].ToString()) ? "0" : row1["assetdetails_not_5kcase"]);
                //            dtLeaseStart = Convert.ToDateTime(string.IsNullOrEmpty(row1["assetdetails_lease_startdate"].ToString()) ? "01-01-1900" : row1["assetdetails_lease_startdate"]);
                //            dtLeaseEnd = Convert.ToDateTime(string.IsNullOrEmpty(row1["assetdetails_lease_enddate"].ToString()) ? "01-01-1900" : row1["assetdetails_lease_enddate"]);
                //            sDepType = Convert.ToString(string.IsNullOrEmpty(row1["asset_dep_type"].ToString()) ? "0" : row1["asset_dep_type"]);
                //            dDepRate = Convert.ToDecimal(string.IsNullOrEmpty(row1["asset_dep_rate"].ToString()) ? "0" : row1["asset_dep_rate"]);
                //            sAssetGroup = Convert.ToString(string.IsNullOrEmpty(row1["h_asst_groupid_no"].ToString()) ? "0" : row1["h_asst_groupid_no"]);
                //            sPONumber = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_po_number"].ToString()) ? "0" : row1["assetdetails_po_number"]);
                //            sFICCRef = Convert.ToString(string.IsNullOrEmpty(row1["h_ficrefnumber"].ToString()) ? "0" : row1["h_ficrefnumber"]);
                //            sAsset_GroupId = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_asset_groupid"].ToString()) ? "0" : row1["assetdetails_asset_groupid"]);
                //            dTransfer_value = Convert.ToDecimal(string.IsNullOrEmpty(row1["assetdetails_trf_value"].ToString()) ? "0" : row1["assetdetails_trf_value"]);
                //            grpid = Convert.ToInt64(string.IsNullOrEmpty(row1["assetdetails_asset_groupid"].ToString()) ? "0" : row1["assetdetails_asset_groupid"]);
                //            newassetid = Convert.ToInt32(row1["assetdetails_gid"].ToString());
                //            // CanDepreciateFully = Convert.ToString(string.IsNullOrEmpty(row1["h_ficrefnumber"].ToString()) ? "0" : row1["h_ficrefnumber"]);
                //        }

                //        decimal depamt = Math.Round(objidm.GetTotalDep(soacaptilisationda, dtTillDate,
                //                        AssetValue, sAssetCode,
                //                     cNot5000Case, sBranch1, sBranch2,
                //                     dtBranchLaunch, dtLeaseStart, dtLeaseEnd,
                //                     sDepType, dDepRate,
                //                     sAssetGroup,
                //                     sPONumber,
                //                     sFICCRef,
                //                     sAsset_GroupId,
                //                     dTransfer_value,
                //                     CanDepreciateFully,
                //                     dDepDevRate));


                //        //rectificationamt = depamt - sumassetdep;

                //        if (sDepType == "SLM" || sDepType == "")
                //        {
                //            sDepType = "S";
                //        }
                //        else if (sDepType == "LPM")
                //        {
                //            sDepType = "L";
                //        }
                //        string[,] codesdep = new string[,]
                //{
                //    {"depreciation_amount", depamt.ToString()},
                //    {"depreciation_assetdet_gid",newassetid.ToString()},
                //    {"depreciation_month", Convert.ToString("SYSDATETIME()")},
                //    {"depreciation_asset_groupid", grpid.ToString()},
                //    {"depreciation_asset_owner", "F"},
                //    {"depreciation_type","R"},
                //    {"depreciation_insert_by",Convert.ToString(objcmnfunc.GetLoginUserGid())},
                //    {"depreciation_insert_date", Convert.ToString("SYSDATETIME()")}
                //};
                //        string insertcmn = objcmnIUD.InsertCommon(codesdep, "fa_trn_tdepreciation");

                //        //ENTRY FOR GL UPLOAD
                //        cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "UploadDetails";
                //        cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = newassetid.ToString();
                //        da = new SqlDataAdapter(cmd);
                //        string glCode = "";
                //        string branch = "";
                //        string cat = "";
                //        string subcat = "";
                //        string asset_id = "";
                //        DataTable UPLOADDATATBL = new DataTable();
                //        da.Fill(UPLOADDATATBL);
                //        if (UPLOADDATATBL.Rows.Count > 0)
                //            foreach (DataRow rowdep in UPLOADDATATBL.Rows)
                //            {
                //                glCode = UPLOADDATATBL.Rows[0]["asset_glcode"].ToString();
                //                branch = UPLOADDATATBL.Rows[0]["branch_code"].ToString();
                //                cat = UPLOADDATATBL.Rows[0]["assetcategory_name"].ToString();
                //                if (cat.Length > 16)
                //                cat = cat.Substring(0, 16);
                //                subcat = UPLOADDATATBL.Rows[0]["asset_code"].ToString();
                //                asset_id = UPLOADDATATBL.Rows[0]["assetdetails_assetdet_id"].ToString();
                //            }
                //        //DataTable detaildt3 = new DataTable();
                //        //detaildt3 = GetDetails(newassetid.ToString());
                //        //for (int r = 0; r < detaildt3.Rows.Count; r++)
                //        //{
                //        //   newassetid = detaildt3.Rows[r]["assetdetails_gid"].ToString().Trim();
                //        //}

                //        string BS = "";
                //        string CC = "";
                //        cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "BSCC_upload_details";
                //        cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = 42;
                //        cmd.Parameters.Add("@fccc", SqlDbType.VarChar).Value = branch;
                //        da = new SqlDataAdapter(cmd);
                //        UPLOADDATATBL = new DataTable();
                //        da.Fill(UPLOADDATATBL);
                //        if (UPLOADDATATBL.Rows.Count > 0)
                //            foreach (DataRow rowdep in UPLOADDATATBL.Rows)
                //            {
                //                BS = UPLOADDATATBL.Rows[0]["depreciationbscc_bs"].ToString();
                //                CC = UPLOADDATATBL.Rows[0]["depreciationbscc_cc_oracle"].ToString();
                //            }
                //        string[,] glCoulmsD = new string[,]
                //        {                                
                //            {"tran_date",objidm.convertoDate(DateTime.Today.ToShortDateString()).ToString()},
                //            {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                //            {"tran_desc","ASSET VALLUE REDUCTION - " + asset_id.ToString() },
                //            {"tran_gl_no",glCode},
                //            {"tran_amount",AssetValue.ToString()},
                //            {"tran_acc_mode","D".ToString()},
                //            {"tran_mult","-1"},  
                //           {"tran_fc_code",BS},
                //           {"tran_cc_code",CC}, 
                //            {"tran_product_code","999".ToString()},
                //            {"tran_ou_code",branch},
                //            {"tran_ref_flag",objidm.GetRef("AVR")},
                //            {"tran_ref_gid", newassetid.ToString() },
                //            {"tran_upload_gid","0".ToString()},
                //            {"tran_isremoved","N"},
                //            {"tran_main_cat","1"},
                //            {"tran_sub_cat",subcat},
                //             {"tran_expense_category","10"},
                //            {"tran_primary_branch_code",branch.ToString()}, 
                //            {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                //            {"tran_maker_id",objcmnfunc.GetLoginUserGid().ToString()},
                //             {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                //        };
                //        string insertforglD = objcmnIUD.InsertCommon(glCoulmsD, "fa_trn_ttran");
                //        string[,] glCoulmsC = new string[,]
                //        {                                
                //             {"tran_date",objidm.convertoDate(DateTime.Today.ToShortDateString()).ToString()},
                //            {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                //            {"tran_desc","ASSET VALLUE REDUCTION - " + asset_id.ToString() },
                //            {"tran_gl_no",glCode},
                //            {"tran_amount",AssetValue.ToString()},
                //            {"tran_acc_mode","C".ToString()},
                //            {"tran_mult","1"},  
                //           {"tran_fc_code",BS},
                //           {"tran_cc_code",CC},      
                //            {"tran_product_code","999".ToString()},
                //            {"tran_ou_code",branch},
                //            {"tran_ref_flag",objidm.GetRef("AVR")},
                //            {"tran_ref_gid", newassetid.ToString()},
                //            {"tran_upload_gid","0".ToString()},
                //            {"tran_isremoved","N"},
                //            {"tran_main_cat","1"},
                //            {"tran_sub_cat",subcat},
                //            {"tran_expense_category","".ToString()},
                //            {"tran_primary_branch_code",branch.ToString()},                         
                //            {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                //            {"tran_maker_id",objcmnfunc.GetLoginUserGid().ToString()},
                //             {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                //        };
                //        string insertforglC = objcmnIUD.InsertCommon(glCoulmsC, "fa_trn_ttran");
                //        string[,] glCoulmsDepD = new string[,]
                //        {                             
                //             {"tran_date",objidm.convertoDate(DateTime.Today.ToShortDateString()).ToString()},
                //            {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                //            {"tran_gl_no",glCode},
                //            {"tran_desc","DEP FOR THE MONTH OF " + objidm.Format(objidm.convertoDate(DateTime.Today.ToShortDateString()).ToString())  + " - FOR THE ASSET :  " + asset_id.ToString()},
                //            {"tran_amount",depamt.ToString()},
                //            {"tran_acc_mode","D".ToString()},
                //            {"tran_mult","-1"},  
                //           {"tran_fc_code",BS},
                //            {"tran_product_code","999".ToString()},
                //           {"tran_cc_code",CC},
                //            {"tran_ou_code",branch},
                //            {"tran_ref_flag",objidm.GetRef("DEP")},
                //            {"tran_ref_gid",newassetid.ToString()},
                //            {"tran_upload_gid","0".ToString()},
                //            {"tran_isremoved","N"},
                //            {"tran_main_cat","1"},
                //            {"tran_sub_cat",subcat},
                //             {"tran_expense_category","10"},
                //            {"tran_primary_branch_code",branch.ToString()},                                
                //            {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                //            {"tran_maker_id",objcmnfunc.GetLoginUserGid().ToString()},
                //             {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                //        };
                //        string insertforglDepD = objcmnIUD.InsertCommon(glCoulmsDepD, "fa_trn_ttran");
                //        string[,] glCoulmsDepC = new string[,]
                //        {                                
                //             {"tran_date",objidm.convertoDate(DateTime.Today.ToShortDateString()).ToString()},
                //            {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                //            {"tran_desc","DEP FOR THE MONTH OF " + objidm.Format(objidm.convertoDate(DateTime.Today.ToShortDateString()).ToString())  + " - FOR THE ASSET :  " + asset_id.ToString()},
                //            {"tran_gl_no",glCode},
                //            {"tran_amount",depamt.ToString()},
                //            {"tran_acc_mode","C".ToString()},
                //            {"tran_mult","1"},  
                //           {"tran_fc_code",BS},
                //           {"tran_cc_code",CC},                                
                //            {"tran_ou_code",branch},
                //            {"tran_product_code","999".ToString()},
                //            {"tran_ref_flag",objidm.GetRef("DEP")},
                //            {"tran_ref_gid", newassetid.ToString()},
                //            {"tran_upload_gid","0".ToString()},
                //            {"tran_isremoved","N"},
                //            {"tran_main_cat","1"},
                //            {"tran_sub_cat",subcat},
                //             {"tran_expense_category","10"},
                //            {"tran_primary_branch_code",branch.ToString()},                         
                //            {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                //            {"tran_maker_id",objcmnfunc.GetLoginUserGid().ToString()},
                //             {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                //        };
                //        string insertforglDepC = objcmnIUD.InsertCommon(glCoulmsDepC, "fa_trn_ttran");


                return "success";
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return "success";
            }
            finally
            {
                con.Close();
            }
        }

        //public string UpdateCommon(string[] codes1, string[] wher, string tblnam)
        //{

        //}

        public List<AssetVRModel> VRAutoBranch(string term)
        {
            List<AssetVRModel> Model = new List<AssetVRModel>();
            AssetVRModel obj = new AssetVRModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_ValueReduction", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "BranchCode";
                cmd.Parameters.Add("@assetbranchcode", SqlDbType.VarChar).Value = term;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetVRModel();
                    obj.branchcode = row["branch_code"].ToString();
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

        public List<AssetVRModel> VRAutoAsset(string term)
        {
            List<AssetVRModel> Model = new List<AssetVRModel>();
            AssetVRModel obj = new AssetVRModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_ValueReduction", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "AutoAssetdetDetid";
                cmd.Parameters.Add("@assetdetDetid", SqlDbType.VarChar).Value = term;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetVRModel();
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

        public string Getstatus(string statusname)
        {
            string status = "";
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_ValueReduction", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "getstatus");
                cmd.Parameters.AddWithValue("@status", statusname.ToString());
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    status = row["status_flag"].ToString();
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

        public IEnumerable<AssetVRModel> GetVRDetailsearch(string assetid)
        {
            List<AssetVRModel> Model = new List<AssetVRModel>();
            AssetVRModel obj = new AssetVRModel();
            try
            {
                GetConnection();
                //if(status == "DRAFT")
                //{ 
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_ValueReduction", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "searchdetailsumary");
                //  cmd.Parameters.AddWithValue("@assetdetDetid", assetid.ToString());
                // cmd.Parameters.AddWithValue("@status", status.ToString());
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetVRModel();

                    DataTable dt1 = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = row["assetreduction_status"].ToString();
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "statusname";
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt1);
                    foreach (DataRow row1 in dt1.Rows)
                        obj.avrstatus = row1["status_name"].ToString();
                    obj.assetdetDetid = row["Oldassetdetails_detid"].ToString();
                    obj.assetreduval = Convert.ToDecimal(row["Original_Value"].ToString());
                    obj.assetnewval = row["New_Value"].ToString();
                    obj.assetreducedamt = row["Reduced_value"].ToString();
                    obj.assetrefno = row["assetreduction_reference_no"].ToString();
                    obj.assetdetId = Convert.ToInt32(row["Oldassetdetails_gid"]);
                    obj.gid = Convert.ToInt32(row["assetdetails_gid"]);
                    Model.Add(obj);
                }
                //}
                //if (status == "APPROVED")
                //{
                //    DataTable dt = new DataTable();
                //    SqlCommand cmd = new SqlCommand("pr_ifams_trn_ValueReduction", con);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.AddWithValue("@action", "searchdetailsumaryApproved");
                //    cmd.Parameters.AddWithValue("@assetdetDetid", assetid.ToString());
                //    cmd.Parameters.AddWithValue("@status", status.ToString());
                //    dt.Load(cmd.ExecuteReader());
                //    foreach (DataRow row in dt.Rows)
                //    {
                //        obj = new AssetVRModel();
                //        obj.assetdetDetid = row["Oldassetdetails_gid"].ToString();
                //        obj.assetreduval = Convert.ToInt32(row["Original_Value"].ToString());
                //        obj.assetnewval = row["New_Value"].ToString();
                //        obj.assetreducedamt = row["Reduced_value"].ToString();
                //        obj.assetrefno = row["assetreduction_reference_no"].ToString();
                //        Model.Add(obj);
                //    }
                //}
                // if (status == "REJECTED")
                // {
                //     DataTable dt = new DataTable();
                //     SqlCommand cmd = new SqlCommand("pr_ifams_trn_ValueReduction", con);
                //     cmd.CommandType = CommandType.StoredProcedure;
                //     cmd.Parameters.AddWithValue("@action", "searchdetailsumaryRejected");
                //     cmd.Parameters.AddWithValue("@assetdetDetid", assetid.ToString());
                //     cmd.Parameters.AddWithValue("@status", status.ToString());
                //     dt.Load(cmd.ExecuteReader());
                //     foreach (DataRow row in dt.Rows)
                //     {
                //         obj = new AssetVRModel();
                //         obj.assetdetDetid = row["Oldassetdetails_gid"].ToString();
                //         obj.assetreduval = Convert.ToInt32(row["Original_Value"].ToString());
                //         obj.assetnewval = row["New_Value"].ToString();
                //         obj.assetreducedamt = row["Reduced_value"].ToString();
                //         obj.assetrefno = row["assetreduction_reference_no"].ToString();
                //         Model.Add(obj);
                //     }
                // }
                // if (status == "WAITING FOR APPROVAL")
                // {
                //     DataTable dt = new DataTable();
                //     SqlCommand cmd = new SqlCommand("pr_ifams_trn_ValueReduction", con);
                //     cmd.CommandType = CommandType.StoredProcedure;
                //     cmd.Parameters.AddWithValue("@action", "searchdetailsumaryWAITING");
                //     cmd.Parameters.AddWithValue("@assetdetDetid", assetid.ToString());
                //     cmd.Parameters.AddWithValue("@status", status.ToString());
                //     dt.Load(cmd.ExecuteReader());
                //     foreach (DataRow row in dt.Rows)
                //     {
                //         obj = new AssetVRModel();
                //         obj.assetdetDetid = row["Oldassetdetails_gid"].ToString();
                //         obj.assetreduval = Convert.ToInt32(row["Original_Value"].ToString());
                //         obj.assetnewval = row["New_Value"].ToString();
                //         obj.assetreducedamt = row["Reduced_value"].ToString();
                //         obj.assetrefno = row["assetreduction_reference_no"].ToString();
                //         Model.Add(obj);
                //     }
                // }

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

        public string AbortVR(string id)
        {
            try
            {
                GetConnection();
                string[,] codes2 = new string[,]
                     {
                        {"assetreduction_isremoved", "Y"},

                     };
                string[,] whrcol1 = new string[,]
                     {  
                         {" assetreduction_assetdet_gid", id.ToString()}, 
                     };
                string tblname = "fa_trn_tsoaheader";
                string updcommn = objcmnIUD.UpdateCommon(codes2, whrcol1, tblname);

                //DataTable dt1 = new DataTable();
                //cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Salegid", id.ToString());
                //cmd.Parameters.AddWithValue("@action", "updatasset");
                //da = new SqlDataAdapter(cmd);
                //da.Fill(dt1);
                //foreach (DataRow row1 in dt1.Rows)
                //{
                string[,] cola = new string[,]
                        {                          
                           {"assetdetails_takenby",Getstatus("FREE")}
                        };

                string[,] whercola = new string[,]
                        {
                            {"assetdetails_gid", id.ToString() },
                        };
                string updcm = objcmnIUD.UpdateCommon(cola, whercola, "fa_trn_tassetdetails");
                //}
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                //return "0";
                //throw ex;
            }
            finally
            {
                con.Close();
            }

            return "success";

        }



        //---------------checker----------
        public IEnumerable<AssetVRModel> GetCheckerReduction(string usergid)
        {

            List<AssetVRModel> objvrModel = new List<AssetVRModel>();
            AssetVRModel obj = new AssetVRModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_ValueReduction", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "chkVRdetail");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetVRModel();
                    obj.assetdetId = Convert.ToInt32(row["assetreduction_id"].ToString());
                    obj.assetdetDetid = row["Oldassetdetails_gid"].ToString();
                    obj.assetreduval = Convert.ToDecimal(row["Original_Value"].ToString());
                    obj.assetnewval = row["New_Value"].ToString();
                    obj.assetreducedamt = row["Reduced_value"].ToString();
                    obj.assetrefno = row["assetreduction_reference_no"].ToString();
                    //obj.assetdetAssetvalue = Convert.ToInt32(row["assetreduction_reduced_date"].ToString());
                    //obj.assetdetAssetvalue = Convert.ToInt32(row["assetreduction_status"].ToString());                    
                    objvrModel.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw ex;

            }
            finally
            {
                con.Close();
            }
            return objvrModel;
        }


        public IEnumerable<AssetVRModel> GetVRchkDetailsearch(string assetid)
        {
            List<AssetVRModel> Model = new List<AssetVRModel>();
            AssetVRModel obj = new AssetVRModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_ValueReduction", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "searchdetailsumary");
                cmd.Parameters.AddWithValue("@assetdetDetid", assetid.ToString());

                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetVRModel();
                    obj.assetdetId = Convert.ToInt32(row["Oldassetdetails_gid"].ToString());
                    obj.assetdetDetid = row["Oldassetdetails_detid"].ToString();
                    obj.assetreduval = Convert.ToInt32(row["Original_Value"].ToString());
                    obj.assetnewval = row["New_Value"].ToString();
                    obj.assetreducedamt = row["Reduced_value"].ToString();
                    obj.assetrefno = row["assetreduction_reference_no"].ToString();
                    obj.assetstatus = row["assetreduction_status"].ToString();
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

        public IEnumerable<AssetVRModel> GetVRDetail(string assetid)
        {
            List<AssetVRModel> Model = new List<AssetVRModel>();
            AssetVRModel obj = new AssetVRModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_ValueReduction", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "VRdetailsumary");
                cmd.Parameters.AddWithValue("@Assetid", assetid.ToString());

                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetVRModel();
                    obj.assetdetId = Convert.ToInt32(row["assetdetailsgid"].ToString());
                    obj.assetdetDetid = row["Oldassetdetails_gid"].ToString();
                    obj.assetreduval = Convert.ToDecimal(row["Original_Value"].ToString());
                    obj.assetnewval = row["New_Value"].ToString();
                    obj.assetreducedamt = row["Reduced_value"].ToString();
                    obj.assetrefno = row["assetreduction_reference_no"].ToString();
                    obj.assetstatus = row["assetreduction_status"].ToString();
                    obj.assetstatus = row["assetreduction_status"].ToString();
                    obj.assetRectifAmt = objidm.getRetificationAmount(row["assetreduction_assetdet_id"].ToString());
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

        //public string VRChkApprovalStatus(string id, string chstatus, string remarks)
        //{
        //    return "dsiugy";
        //}



        public string VRChkApprovalStatus(string assetgid, string chstatus, string remarks)
        {

            try
            {
                GetConnection();

                if (chstatus == "REJECTED")
                {

                    string[,] col = new string[,]
                    {
                       // {"assetsplitmerge_isremoved","Y"},
                        {"assetreduction_status", Getstatus("REJECTED").ToString()},
                        {"assetreduction_update_by",Convert.ToString(objcmnfunc.GetLoginUserGid())},
                        {"assetreduction_update_date",Convert.ToString("SYSDATETIME()")}
                    };
                    string[,] whercol = new string[,]
                    {
                        {"assetreduction_assetdet_id",assetgid.ToString()},
                        {"assetreduction_status", Getstatus("WAITING FOR APPROVAL")}
                    };
                    string updcmn = objcmnIUD.UpdateCommon(col, whercol, "fa_trn_tassetreduction");

                    string[,] col1 = new string[,] 
                    {
                        //{"assetdetails_takenfor_split","N"}
                        {"assetdetails_takenby", "14"},
                        {"assetdetails_update_by",Convert.ToString(objcmnfunc.GetLoginUserGid())},
                        {"assetdetails_update_date",Convert.ToString("SYSDATETIME()")}
                    };
                    string[,] whercol1 = new string[,]
                    {
                        {"assetdetails_gid",assetgid.ToString()},
                         {"assetdetails_takenby", "20"},
                    };
                    string updcmmn = objcmnIUD.UpdateCommon(col1, whercol1, "fa_trn_tassetdetails");

                    string[,] colu = new string[,]
                    {
                        {"queue_action_by",objcmnfunc.GetLoginUserGid().ToString()},
                        {"queue_action_status","2"},
                        {"queue_action_remark", remarks.ToString()},
                        {"queue_action_date",convertoDate(objcmnIUD.GetCurrentDate())},
                        {"queue_isremoved","Y"},
                    };
                    string[,] Whercolu = new string[,]
                    {
                        {"queue_ref_gid", assetgid.Trim()},
                        {"queue_ref_flag",objidm.GetRef("AVR").ToString()},
                        {"queue_action_status","0"}
                    };
                    string updcommn = objcmnIUD.UpdateCommon(colu, Whercolu, "iem_trn_tqueue");

                    return "success";
                }
                else
                {
                    //string[,] codes = new string[,]
                    //{
                    //    {"assetreduction_assetdet_id",status.assetdetId.ToString()},
                    //    {"assetreduction_asset_value",status.assetdetAssetvalue.ToString()},
                    //    {"assetreduction_new_assetvalue",status.assetreduval.ToString()},
                    //    {"assetreduction_reducedamount", (status.assetdetAssetvalue - status.assetreduval).ToString()},
                    //    {"assetreduction_reference_no", objcmnfunc.GetSequenceNoFam("AVR").ToString()},
                    //    {"assetreduction_reduced_date",Convert.ToString("SYSDATETIME()")},
                    //    {"assetreduction_isremoved","N"},
                    //    {"assetreduction_status",Getstatus("DRAFT").ToString()},
                    //    {"assetreduction_insert_by",Convert.ToString(objcmnfunc.GetLoginUserGid())},
                    //    {"assetreduction_insert_date",Convert.ToString("SYSDATETIME()")}
                    //};
                    //string tblname = "fa_trn_tassetreduction";
                    //string instcomm = objcmnIUD.InsertCommon(codes, tblname);




                    decimal oldassetreduval = 0;
                    decimal newassetreduval = 0;
                    decimal assetredcutionreducedamt = 0;
                    decimal assetnewval = 0;
                    string asset5kcase = "";
                    string assetcode = "";
                    DataTable dt = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_ValueReduction", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "assetdetgid");
                    cmd.Parameters.AddWithValue("@Assetid", assetgid.ToString());
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        oldassetreduval = Convert.ToDecimal(row["assetdetails_reduced_value"].ToString());
                        assetredcutionreducedamt = Convert.ToDecimal(row["assetreduction_reducedamount"].ToString());
                        assetnewval = Convert.ToDecimal(row["assetreduction_new_assetvalue"].ToString());
                        assetcode = row["assetdetails_asset_code"].ToString();
                    }
                    newassetreduval = oldassetreduval + assetredcutionreducedamt;

                    string GL = string.Empty;
                    //Muthu 10-12-2016
                    DataTable dta = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@assetcodes", assetcode.ToString());
                    cmd.Parameters.AddWithValue("@action", "SelectGL");
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dta);
                    for (int j = 0; j < dta.Rows.Count; j++)
                    {
                        GL = dta.Rows[0]["asset_glcode"].ToString();
                    }


                    #region /* Remove < 5000 Flag 31-10-2018 */
                    /*if (assetnewval <= 5000 || (assetnewval > 5000 && GL == "121010002"))
                    {
                        asset5kcase = "N";
                    }
                    else
                    {
                        asset5kcase = "Y";
                    }*/
                    asset5kcase = "Y";
                    #endregion

                    string[,] codes1 = new string[,]
                {
                    {"assetdetails_asset_value",assetnewval.ToString()},
                   // {"assetdetails_reduced_value",status.assetreduval.ToString()},
                    {"assetdetails_not_5kcase",asset5kcase.ToString()},
                    {"assetdetails_reduced_value",newassetreduval.ToString()},
                    {"assetdetails_trf_value",assetnewval.ToString()},
                    {"assetdetails_update_by",Convert.ToString(objcmnfunc.GetLoginUserGid())},
                    {"assetdetails_update_date",Convert.ToString(objcmnIUD.GetCurrentDate())},
                    {"assetdetails_takenby","14"}
                };
                    string[,] wher = new string[,]
                {
                    {"assetdetails_gid",assetgid.ToString()},
                    {"assetdetails_takenby","20"}
                    
                };
                    string tblnam = "fa_trn_tassetdetails";
                    string updatecmn = objcmnIUD.UpdateCommon(codes1, wher, tblnam);






                    string[,] col = new string[,]
                    {
                       // {"assetsplitmerge_isremoved","Y"},
                        {"assetreduction_status", Getstatus("APPROVED").ToString()},
                        {"assetreduction_update_by",Convert.ToString(objcmnfunc.GetLoginUserGid())},
                        {"assetreduction_update_date",Convert.ToString("SYSDATETIME()")}
                    };
                    string[,] whercol = new string[,]
                    {
                        {"assetreduction_assetdet_id",assetgid.ToString()},
                        {"assetreduction_status", Getstatus("WAITING FOR APPROVAL")}
                    };
                    string updcmn = objcmnIUD.UpdateCommon(col, whercol, "fa_trn_tassetreduction");





                    string[,] colu = new string[,]
                    {
                        {"queue_action_by",objcmnfunc.GetLoginUserGid().ToString()},
                        {"queue_action_status","1"},
                        {"queue_action_remark", remarks.ToString()},
                        {"queue_action_date",convertoDate(objcmnIUD.GetCurrentDate())},
                        {"queue_isremoved","Y"},
                    };
                    string[,] whercolu = new string[,]
                    {
                        {"queue_ref_gid", assetgid.Trim()},
                        {"queue_ref_flag",objidm.GetRef("AVR").ToString()},
                        {"queue_action_status","0"}
                    };
                    string updco = objcmnIUD.UpdateCommon(colu, whercolu, "iem_trn_tqueue");


                    //string newasset = "";
                    //DataTable dtnewas = new DataTable();
                    //     cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                    //     cmd.CommandType = CommandType.StoredProcedure;
                    //     cmd.Parameters.AddWithValue("@action", "newsplitdet");
                    //     cmd.Parameters.AddWithValue("@assetdetid", id.ToString());
                    //     da = new SqlDataAdapter(cmd);
                    //     da.Fill(dtnewas);
                    //     foreach (DataRow row in dtnewas.Rows)
                    //     {
                    //newasset = row["assetsplitmerge_new_assetid"].ToString();

                    string soacaptilisationdat = Convert.ToString("SYSDATETIME()");
                    DateTime soacaptilisationda = DateTime.Now;
                    DateTime dtTillDate = DateTime.Now;
                    decimal AssetValue = 0;
                    string sAssetCode = "";
                    string cNot5000Case = "";
                    string sBranch1 = "";
                    string sBranch2 = "";
                    DateTime dtBranchLaunch = DateTime.Now;
                    DateTime dtLeaseStart = DateTime.Now;
                    DateTime dtLeaseEnd = DateTime.Now;
                    string sDepType = "";
                    decimal dDepRate = 0;
                    string sAssetGroup = "";
                    string sPONumber = "";
                    string sFICCRef = "";
                    string sAsset_GroupId = "";
                    decimal dTransfer_value = 0;
                    string CanDepreciateFully = "";
                    Int32 dDepDevRate = 0;
                    string grpid = "";
                    Int32 newassetid = 0;
                    // Int32 assetdetid = obj.assetdetgid;
                     decimal sumassetdep = 0;
                    //decimal linewdv = 0;
                    decimal rectificationamt = 0;
                    // decimal linePL = 0;


                    DataTable dtwdv = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_ValueReduction", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "WDVgid");
                    cmd.Parameters.AddWithValue("@Assetid", assetgid.ToString());
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
                        grpid = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_asset_groupid"].ToString()) ? "0" : row1["assetdetails_asset_groupid"]);
                        newassetid = Convert.ToInt32(row1["assetdetails_gid"].ToString());
                        // CanDepreciateFully = Convert.ToString(string.IsNullOrEmpty(row1["h_ficrefnumber"].ToString()) ? "0" : row1["h_ficrefnumber"]);
                    }

                    decimal depamt = Math.Round(objidm.GetTotalDep(soacaptilisationda, dtTillDate,
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
                                 dDepDevRate),2);




                    DataTable dtdep = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "Totdep");
                    cmd.Parameters.AddWithValue("@assetgid", assetgid);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dtdep);
                    foreach (DataRow rowdep in dtdep.Rows)
                    {
                        sumassetdep = Convert.ToDecimal(rowdep["depreciation_amount"].ToString());
                    }

                    

                    rectificationamt = depamt - sumassetdep;

                    if (sDepType == "SLM" || sDepType == "")
                    {
                        sDepType = "S";
                    }
                    else if (sDepType == "LPM")
                    {
                        sDepType = "L";
                    }
                    string[,] codesdep = new string[,]
                    {
                        //{"depreciation_amount", depamt.ToString()},
                        {"depreciation_amount", rectificationamt.ToString()},
                        {"depreciation_assetdet_gid",newassetid.ToString()},
                        {"depreciation_month", Convert.ToString("SYSDATETIME()")},
                        {"depreciation_asset_groupid", grpid.ToString()},
                        {"depreciation_asset_owner", "F"},
                        {"depreciation_type","R"},
                        {"depreciation_insert_by",Convert.ToString(objcmnfunc.GetLoginUserGid())},
                        {"depreciation_insert_date", Convert.ToString("SYSDATETIME()")}
                    };
                    string insertcmn = objcmnIUD.InsertCommon(codesdep, "fa_trn_tdepreciation");

                    //ENTRY FOR GL UPLOAD
                    cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "UploadDetails";
                    cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = newassetid.ToString();
                    da = new SqlDataAdapter(cmd);
                    //string glCode = "";
                    //string branch = "";
                    //string cat = "";
                    //string subcat = "";
                    //string asset_id = "";
                    //DataTable UPLOADDATATBL = new DataTable();
                    //da.Fill(UPLOADDATATBL);
                    //if (UPLOADDATATBL.Rows.Count > 0)
                    //    foreach (DataRow rowdep in UPLOADDATATBL.Rows)
                    //    {
                    //        glCode = UPLOADDATATBL.Rows[0]["asset_glcode"].ToString();
                    //        branch = UPLOADDATATBL.Rows[0]["branch_code"].ToString();
                    //        cat = UPLOADDATATBL.Rows[0]["assetcategory_name"].ToString();
                    //        if (cat.Length > 16)
                    //            cat = cat.Substring(0, 16);
                    //        subcat = UPLOADDATATBL.Rows[0]["asset_code"].ToString();
                    //        asset_id = UPLOADDATATBL.Rows[0]["assetdetails_assetdet_id"].ToString();
                    //    }
                    ////DataTable detaildt3 = new DataTable();
                    ////detaildt3 = GetDetails(newassetid.ToString());
                    ////for (int r = 0; r < detaildt3.Rows.Count; r++)
                    ////{
                    ////   newassetid = detaildt3.Rows[r]["assetdetails_gid"].ToString().Trim();
                    ////}
                    string glCode = "";
                    string resdepglCode = "";
                    string depglCode = "";
                    string branch = "";
                    string cat = "";
                    string subcat = "";
                    string asset_id = "";
                    string BS = "";
                    string CC = "";
                    string ratio = "";
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
                    
                    cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "BSCC_upload_details";
                    cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = 42;
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
                    GetConnection();
                    cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "avr";
                    cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = newassetid;
                  
                    string maker = Convert.ToString(cmd.ExecuteScalar());
                    //string[,] glCoulmsD = new string[,]
                    //        {                                
                    //            {"tran_date",objidm.convertoDate(DateTime.Today.ToShortDateString()).ToString()},
                    //            {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                    //            {"tran_desc","ASSET VALLUE REDUCTION - " + asset_id.ToString() },
                    //            {"tran_gl_no",glCode},
                    //            {"tran_amount",AssetValue.ToString()},
                    //            {"tran_acc_mode","D".ToString()},
                    //            {"tran_mult","-1"},  
                    //           {"tran_fc_code",BS},
                    //           {"tran_cc_code",CC}, 
                    //            {"tran_product_code","999".ToString()},
                    //            {"tran_ou_code",branch},
                    //            {"tran_ref_flag",objidm.GetRef("AVR")},
                    //            {"tran_ref_gid", newassetid.ToString() },
                    //            {"tran_upload_gid","0".ToString()},
                    //            {"tran_isremoved","N"},
                    //            {"tran_main_cat","1"},
                    //            {"tran_sub_cat",subcat},
                    //             {"tran_expense_category","10"},
                    //            {"tran_primary_branch_code",branch.ToString()}, 
                    //            {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                    //           // {"tran_maker_id",objcmnfunc.GetLoginUserGid().ToString()},
                    //             {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                    //        };
                    //string insertforglD = objcmnIUD.InsertCommon(glCoulmsD, "fa_trn_ttran");
                    //string[,] glCoulmsC = new string[,]
                    //        {                                
                    //             {"tran_date",objidm.convertoDate(DateTime.Today.ToShortDateString()).ToString()},
                    //            {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                    //            {"tran_desc","ASSET VALLUE REDUCTION - " + asset_id.ToString() },
                    //            {"tran_gl_no",glCode},
                    //            {"tran_amount",AssetValue.ToString()},
                    //            {"tran_acc_mode","C".ToString()},
                    //            {"tran_mult","1"},  
                    //           {"tran_fc_code",BS},
                    //           {"tran_cc_code",CC},      
                    //            {"tran_product_code","999".ToString()},
                    //            {"tran_ou_code",branch},
                    //            {"tran_ref_flag",objidm.GetRef("AVR")},
                    //            {"tran_ref_gid", newassetid.ToString()},
                    //            {"tran_upload_gid","0".ToString()},
                    //            {"tran_isremoved","N"},
                    //            {"tran_main_cat","1"},
                    //            {"tran_sub_cat",subcat},
                    //            {"tran_expense_category","".ToString()},
                    //            {"tran_primary_branch_code",branch.ToString()},                         
                    //            {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                    //           // {"tran_maker_id",objcmnfunc.GetLoginUserGid().ToString()},
                    //             {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                    //        };
                    //string insertforglC = objcmnIUD.InsertCommon(glCoulmsC, "fa_trn_ttran");
                    //string[,] glCoulmsDepD = new string[,]
                    //        {                             
                    //             {"tran_date",objidm.convertoDate(DateTime.Today.ToShortDateString()).ToString()},
                    //            {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                    //            {"tran_gl_no",resdepglCode},
                    //            {"tran_desc","ASSET VALLUE REDUCTION - " + asset_id.ToString() },
                    //            {"tran_amount",depamt.ToString()},
                    //            {"tran_acc_mode","D".ToString()},
                    //            {"tran_mult","-1"},  
                    //           {"tran_fc_code",BS},
                    //            {"tran_product_code","999".ToString()},
                    //           {"tran_cc_code",CC},
                    //            {"tran_ou_code",branch},
                    //            {"tran_ref_flag",objidm.GetRef("AVR")},
                    //            {"tran_ref_gid",newassetid.ToString()},
                    //            {"tran_upload_gid","0".ToString()},
                    //            {"tran_isremoved","N"},
                    //            {"tran_main_cat","1"},
                    //            {"tran_sub_cat",subcat},
                    //             {"tran_expense_category","10"},
                    //            {"tran_primary_branch_code",branch.ToString()},                                
                    //            {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                    //          //  {"tran_maker_id",objcmnfunc.GetLoginUserGid().ToString()},
                    //             {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                    //        };
                    //string insertforglDepD = objcmnIUD.InsertCommon(glCoulmsDepD, "fa_trn_ttran");
                    //string[,] glCoulmsDepC = new string[,]
                    //        {                                
                    //             {"tran_date",objidm.convertoDate(DateTime.Today.ToShortDateString()).ToString()},
                    //            {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                    //            {"tran_desc","ASSET VALLUE REDUCTION - " + asset_id.ToString() },
                    //            {"tran_gl_no",resdepglCode},
                    //            {"tran_amount",depamt.ToString()},
                    //            {"tran_acc_mode","C".ToString()},
                    //            {"tran_mult","1"},  
                    //           {"tran_fc_code",BS},
                    //           {"tran_cc_code",CC},                                
                    //            {"tran_ou_code",branch},
                    //            {"tran_product_code","999".ToString()},
                    //            {"tran_ref_flag",objidm.GetRef("AVR")},
                    //            {"tran_ref_gid", newassetid.ToString()},
                    //            {"tran_upload_gid","0".ToString()},
                    //            {"tran_isremoved","N"},
                    //            {"tran_main_cat","1"},
                    //            {"tran_sub_cat",subcat},
                    //             {"tran_expense_category","10"},
                    //            {"tran_primary_branch_code",branch.ToString()},                         
                    //            {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                    //          //  {"tran_maker_id",objcmnfunc.GetLoginUserGid().ToString()},
                    //             {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                    //        };
                    //string insertforglDepC = objcmnIUD.InsertCommon(glCoulmsDepC, "fa_trn_ttran");




                    //tran table insert for transfer

                    //if(rectificationamt < 0)
                    //{
                    //    rectificationamt = Math.Abs(rectificationamt);
                    //}
                    if (rectificationamt < 0)
                    {
                        //depamt = Math.Abs(depamt);
                        rectificationamt = Math.Abs(rectificationamt);

                    string[,] glCoulmsVRDepc = new string[,]
                            {                             
                                {"tran_date",objidm.convertoDate(DateTime.Today.ToShortDateString()).ToString()},
                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_gl_no",depglCode},
                                {"tran_desc","ASSET VALLUE REDUCTION - " + asset_id.ToString() },
                                //{"tran_amount",depamt.ToString()}, /* Passed rectification amount in glupload - 08-08-2017 */
                                {"tran_amount",rectificationamt.ToString()},
                                {"tran_acc_mode","C".ToString()},
                                {"tran_mult","-1"},  
                                {"tran_fc_code",BS},
                                 {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                {"tran_cc_code",CC},
                                {"tran_ou_code",branch},
                                {"tran_ref_flag",objidm.GetRef("AVRDEP")},
                                {"tran_ref_gid",newassetid.ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                                {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                {"tran_expense_category","10"},
                                {"tran_primary_branch_code",branch.ToString()},                                
                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                                // {"tran_maker_id",objcmnfunc.GetLoginUserGid().ToString()},
                                {"tran_checker_id",maker.ToString()},
                                {"tran_emp_id",maker.ToString()},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                    string insertforVRglDepc = objcmnIUD.InsertCommon(glCoulmsVRDepc, "fa_trn_ttran");


                    string[,] glCoulmsVRDepD = new string[,]
                            {                             
                                {"tran_date",objidm.convertoDate(DateTime.Today.ToShortDateString()).ToString()},
                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_gl_no",resdepglCode},
                                {"tran_desc","ASSET VALLUE REDUCTION - " + asset_id.ToString() },
                                //{"tran_amount",depamt.ToString()},/* Passed rectification amount in glupload - 08-08-2017 */
                                {"tran_amount",rectificationamt.ToString()},
                                {"tran_acc_mode","D".ToString()},
                                {"tran_mult","-1"},  
                                {"tran_fc_code",BS},
                                {"tran_product_code",ConfigurationManager.AppSettings["Productcode"]},
                                {"tran_cc_code",CC},
                                {"tran_ou_code",branch},
                                {"tran_ref_flag",objidm.GetRef("AVRDEP")},
                                {"tran_ref_gid",newassetid.ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                                {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                {"tran_expense_category","10"},
                                {"tran_primary_branch_code",branch.ToString()},                                
                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                                //{"tran_maker_id",objcmnfunc.GetLoginUserGid().ToString()},
                                {"tran_checker_id",maker.ToString()},
                                {"tran_emp_id",maker.ToString()},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                    string insertforVRglDepD = objcmnIUD.InsertCommon(glCoulmsVRDepD, "fa_trn_ttran");
                    }
                    else
                    {
                        string[,] glCoulmsVRDepc = new string[,]
                            {                             
                                {"tran_date",objidm.convertoDate(DateTime.Today.ToShortDateString()).ToString()},
                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_gl_no",resdepglCode},
                                {"tran_desc","ASSET VALLUE REDUCTION - " + asset_id.ToString() },
                                //{"tran_amount",depamt.ToString()},/* Passed rectification amount in glupload - 08-08-2017 */
                                {"tran_amount",rectificationamt.ToString()},
                                {"tran_acc_mode","C".ToString()},
                                {"tran_mult","-1"},  
                                {"tran_fc_code",BS},
                                {"tran_product_code",ConfigurationManager.AppSettings["Productcode"]},
                                {"tran_cc_code",CC},
                                {"tran_ou_code",branch},
                                {"tran_ref_flag",objidm.GetRef("AVRDEP")},
                                {"tran_ref_gid",newassetid.ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                                {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                {"tran_expense_category","10"},
                                {"tran_primary_branch_code",branch.ToString()},                                
                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                                //{"tran_maker_id",objcmnfunc.GetLoginUserGid().ToString()},
                                {"tran_checker_id",maker.ToString()},
                                {"tran_emp_id",maker.ToString()},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                        string insertforVRglDepc = objcmnIUD.InsertCommon(glCoulmsVRDepc, "fa_trn_ttran");


                        string[,] glCoulmsVRDepD = new string[,]
                            {                             
                                {"tran_date",objidm.convertoDate(DateTime.Today.ToShortDateString()).ToString()},
                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_gl_no",depglCode},
                                {"tran_desc","ASSET VALLUE REDUCTION - " + asset_id.ToString() },
                                //{"tran_amount",depamt.ToString()},/* Passed rectification amount in glupload - 08-08-2017 */
                                {"tran_amount",rectificationamt.ToString()},
                                {"tran_acc_mode","D".ToString()},
                                {"tran_mult","-1"},  
                                {"tran_fc_code",BS},
                                {"tran_product_code",ConfigurationManager.AppSettings["Productcode"]},
                                {"tran_cc_code",CC},
                                {"tran_ou_code",branch},
                                {"tran_ref_flag",objidm.GetRef("AVRDEP")},
                                {"tran_ref_gid",newassetid.ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                                {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                {"tran_expense_category","10"},
                                {"tran_primary_branch_code",branch.ToString()},                                
                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                                //{"tran_maker_id",objcmnfunc.GetLoginUserGid().ToString()},
                                {"tran_checker_id",maker.ToString()},
                                {"tran_emp_id",maker.ToString()},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                        string insertforVRglDepD = objcmnIUD.InsertCommon(glCoulmsVRDepD, "fa_trn_ttran");
                    }

                    return "success";
                }
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return "success";
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
                string format = "yyyy/MMM/dd";
                convdate = convdate2.ToString(format);
                return convdate;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
        }

        public List<AssetVRModel> VRPopulateAuditTrail(AssetVRModel sapat)
        {
            List<AssetVRModel> objVRAuditTrail = new List<AssetVRModel>();
            try
            {
                AssetVRModel objVRModel;
                GetConnection();
                DataTable dt = new DataTable();
                string safstEmp = "";
                string sastatus = "";
                cmd = new SqlCommand("pr_ifams_trn_audittaril", con);
                cmd.Parameters.AddWithValue("@gid", sapat.assetdetId);
                cmd.Parameters.AddWithValue("@refflag", "AVR");
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
                            objVRModel = new AssetVRModel();
                            objVRModel.employee_code = sadatar[0].ToString() + " - " + sadatar[1].ToString();
                            objVRModel.action_status = "Submitted";
                            objVRModel.action_date = Convert.ToString(convertoDate(dt.Rows[i]["queue_date"].ToString()));
                            objVRModel.queue_to_type = "AVR Maker";
                            objVRAuditTrail.Add(objVRModel);
                            string saactions = Convert.ToString(dt.Rows[i]["queue_action_by"].ToString());
                            if (saactions == "")
                            {
                                string saqueue_type = Convert.ToString(dt.Rows[i]["queue_to_type"].ToString());
                                string saqueue_to = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
                                string actionssa = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
                                if (actionssa != "")
                                {
                                    objVRModel = new AssetVRModel();
                                    objVRModel.employee_code = "";
                                    objVRModel.action_date = Convert.ToString(convertoDate(dt.Rows[i]["queue_action_date"].ToString()));
                                    sastatus = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
                                    objVRModel.action_status = objidm.GetQueueStatusHistry(sastatus);
                                    objVRModel.queue_to_type = objidm.GetQueueRole(saqueue_type, saqueue_to);
                                    objVRModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());
                                    objVRAuditTrail.Add(objVRModel);
                                }
                            }
                            else
                            {
                                string saempid = "'";
                                string saqueue_type = Convert.ToString(dt.Rows[i]["queue_to_type"].ToString());
                                string saqueue_to = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
                                string actionsa = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
                                if (actionsa != "")
                                {
                                    objVRModel = new AssetVRModel();
                                    if (saqueue_type == "G" || saqueue_type == "R" || saqueue_type == "L" || saqueue_type == "D")
                                    {
                                        saempid = Convert.ToString(dt.Rows[i]["queue_action_by"].ToString());
                                    }
                                    if (saempid != "")
                                    {
                                        string saempcodname = objidm.Getempname(saempid);
                                        string[] sadata;
                                        sadata = saempcodname.Split(',');
                                        objVRModel.employee_code = sadata[0].ToString() + " - " + sadata[1].ToString();
                                    }
                                    objVRModel.action_date = Convert.ToString(convertoDate(dt.Rows[i]["queue_action_date"].ToString()));
                                    sastatus = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
                                    objVRModel.action_status = objidm.GetQueueStatusHistry(sastatus);
                                    objVRModel.queue_to_type = objidm.GetQueueRole(saqueue_type, saqueue_to);
                                    objVRModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());
                                    objVRAuditTrail.Add(objVRModel);
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
            return objVRAuditTrail;
        }


        public List<IFAMS.Models.AssetVRModel> GetVRStatus()
        {

            List<AssetVRModel> Model = new List<AssetVRModel>();
            AssetVRModel obj = new AssetVRModel();

            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_ValueReduction", con);
                cmd.Parameters.AddWithValue("@action", "Status");
                cmd.CommandType = CommandType.StoredProcedure;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetVRModel();
                    obj.avrstatus = row["status_name"].ToString();
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