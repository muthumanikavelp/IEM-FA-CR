using IEM.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Configuration;

namespace IEM.Areas.IFAMS.Models
{
    public class IfamsAssetCCDataModel : AssetCCRepository
    {
        SqlConnection con = new SqlConnection();
        // IfamsAssetCCDataModel objdm = new IfamsAssetCCDataModel();
        DataModel objidm = new DataModel();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions objcmnfunction = new CmnFunctions();
        CommonIUD objcmnIUD = new CommonIUD();
        ErrorLog objErrLog = new ErrorLog();
        private void GetConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }

        public string GetStatusExcel(string assetdata, string valid, string action)
        {
            string resultacc = string.Empty;

            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_saleasset", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@salechkdata", SqlDbType.VarChar).Value = valid.ToString();
                cmd.Parameters.Add("@saleassetdata", SqlDbType.VarChar).Value = assetdata.ToString();
                cmd.Parameters.Add("@saleresult", SqlDbType.VarChar).Value = action;
                resultacc = (string)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return resultacc;
        }

        //public string getTheUser(string groupCode)
        //{
        //    string success =   string.Empty  ;
        //    try
        //    {
        //        GetConnection();
        //        cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@querytype", "group");
        //        cmd.Parameters.AddWithValue("@usergroup", groupCode);
        //        DataTable dt = new DataTable();
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        int userid = objcmnfunction.GetLoginUserGid();
        //        if (dt.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                if (dt.Rows[i]["roleemployee_employee_gid"].ToString() == userid.ToString())
        //                {
        //                    success = "success";
        //                }
        //            }
        //        }
        //        return success;
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //        return   string.Empty  ;
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //}

        //public IEnumerable<AssetCCModel> GetAccDetail(string id)
        //{
        //    try
        //    {
        //        List<AssetCCModel> objModel = new List<AssetCCModel>();
        //        AssetCCModel obj = new AssetCCModel();
        //        GetConnection();
        //        DataTable dt = new DataTable();
        //        cmd = new SqlCommand("pr_ifams_trn_ValueReduction", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@action", "selectacc");
        //        cmd.Parameters.AddWithValue("@assetid", id);
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            obj = new AssetCCModel();
        //            DataTable dt1 = new DataTable();
        //            cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
        //            cmd.CommandType = CommandType.StoredProcedure;


        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //        return   string.Empty  ;
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        // }


        public string UpdateACC(DataTable exldata, string filename)
        {
            try
            {
                string branchlegacycode = string.Empty;
                string oldassetid = string.Empty;
                string oracleCCcode = string.Empty;
                string oracleBScode = string.Empty;
                string oldbranchlegacycode = string.Empty;
                string oldoracleCCcode = string.Empty;
                string oldoracleBScode = string.Empty;
                List<AssetCCModel> accList = new List<AssetCCModel>();
                AssetCCModel acc;
                GetConnection();
                DataTable dt = new DataTable();
                dt = (DataTable)exldata;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    acc = new AssetCCModel();
                    acc.assetdetdetid = dt.Rows[i]["Asset id"].ToString().Trim();
                    acc.assetdetcode = dt.Rows[i]["Old Asset Code"].ToString().Trim();
                    acc.assetdetnewcode = dt.Rows[i]["New Asset Code"].ToString().Trim();
                    string reason = dt.Rows[i]["Remarks"].ToString().Trim();
                    string[] assetdetArr = { };
                    string detid = acc.assetdetdetid;
                    //int index = detid.IndexOf("-");
                    //string assetcode = detid.Remove(index);
                    string[] strArray = detid.Split('-');
                    //for (int j = 0; j < strArray.Length; j++)
                    //{
                    //   // acc = new AssetCCModel();
                    //   // acc.assetdetArr[j] = strArray[j].ToString();
                    //    assetdetArr[j] = strArray[j].ToString();
                    //}

                    string assetid = ACCGetid(dt.Rows[i]["Asset id"].ToString());
                    oldassetid = assetid;
                    string newassetcod = acc.assetdetnewcode.ToString();
                    //string a = strArray[0];
                    //string b = strArray[2];
                    acc.assetdetbrnchid = ACCGetbranch(dt.Rows[i]["Asset id"].ToString());
                    acc.branchlegacy = ACCGetLegacy(acc.assetdetbrnchid.ToString());

                    //if (acc.assetdetcode != acc.assetdetnewcode)
                    //{
                    //    string[,] codes = new string[,]
                    //    {
                    //        {"assetdetails_asset_code",GetAssetcodeid(acc.assetdetnewcode.ToString())},
                    //        {"assetdetails_update_by", Convert.ToString(objcmnfunction.GetLoginUserGid())},
                    //        {"assetdetails_update_date", Convert.ToString("SYSDATETIME()")}
                    //    };
                    //    string[,] whrcol = new string[,]
                    //    {
                    //        {"assetdetails_gid", assetid.ToString()}
                    //    };
                    //    string tblnam = "fa_trn_tassetdetails";
                    //    string updcom = objcmnIUD.UpdateCommon(codes, whrcol, tblnam);
                    //}
                    ////acc.assetdetcode == acc.assetdetnewcode &&
                    //if ( acc.assetdetnewcode != strArray[0])
                    //{
                    //    string newassetdetid = objcmnfunction.generateAssetid(b, a);
                    //    string[,] codes = new string[,]
                    //    {
                    //        {"assetdetails_assetdet_id", newassetdetid.ToString()},
                    //        {"assetdetails_update_by", Convert.ToString(objcmnfunction.GetLoginUserGid())},
                    //        {"assetdetails_update_date", Convert.ToString("SYSDATETIME()")}
                    //    };
                    //    string[,] whrcol = new string[,]
                    //    {
                    //         {"assetdetails_gid", assetid.ToString()}
                    //    };
                    //    string tblnam = "fa_trn_tassetdetails";
                    //    string updcom = objcmnIUD.UpdateCommon(codes, whrcol, tblnam);
                    //}

                    //cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = assetid.ToString();
                    //cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "takenby";
                    //string takenby = Convert.ToString(cmd.ExecuteScalar());

                    //if (takenby == "14")
                    //{
                    //    takenby = "14";
                    //}
                    //if (takenby == "21")
                    //{
                    //    takenby = "21";
                    //}


                    if (acc.assetdetcode != acc.assetdetnewcode && acc.assetdetnewcode != strArray[0])
                    {
                        string[,] codes = new string[,]
                        {
                            {"assetdetails_isremoved","N"},
                            //{"assetdetails_takenby","12"},
                            {"assetdetails_takenby","26"},
                          //  {"assetdetails_active_status","N"},
                            {"assetdetails_assetcode_changestatus","N"},
                            {"assetdetails_assetid_source",ACCGetsource("ASSET CODE CHANGE").ToString()},
                            {"assetdetails_update_by", Convert.ToString(objcmnfunction.GetLoginUserGid())},
                            {"assetdetails_update_date", Convert.ToString("SYSDATETIME()")}
                        };
                        string[,] whercol = new string[,]
                        {
                             {"assetdetails_gid", assetid.ToString()}
                        };
                        string tblnam = "fa_trn_tassetdetails";
                        string updcom = objcmnIUD.UpdateCommon(codes, whercol, tblnam);

                        string newassetdetid = objcmnfunction.generateAssetid(acc.branchlegacy.ToString(), newassetcod);

                        DataTable dt1 = new DataTable();
                        dt1 = GetAssetdettable(assetid.ToString());
                        for (int k = 0; k < dt1.Rows.Count; k++)
                        {
                            string[,] code = new string[,]
                            {    
                                    {"assetdetails_entity_gid",dt1.Rows[k]["assetdetails_entity_gid"].ToString()},
                                    {"assetdetails_assetdet_id",newassetdetid.ToString()},                                
                                    {"assetdetails_physical_assetid",dt1.Rows[k]["assetdetails_physical_assetid"].ToString()},
                                    {"assetdetails_qty",dt1.Rows[k]["assetdetails_qty"].ToString()},
                                    {"assetdetails_barcode",dt1.Rows[k]["assetdetails_barcode"].ToString()},
                                    {"assetdetails_asset_groupid",dt1.Rows[k]["assetdetails_asset_groupid"].ToString()},
                                    {"assetdetails_asset_code",GetAssetcodeid(newassetcod.ToString())},
                                    {"assetdetails_asset_value",dt1.Rows[k]["assetdetails_asset_value"].ToString()},
                                    {"assetdetails_asset_description",dt1.Rows[k]["assetdetails_asset_description"].ToString()},
                                    {"assetdetails_cap_date",string.IsNullOrEmpty(dt1.Rows[k]["assetdetails_cap_date"].ToString()) ?   string.Empty   : convertoDate(dt1.Rows[k]["assetdetails_cap_date"].ToString())},
                                    {"assetdetails_po_number",dt1.Rows[k]["assetdetails_po_number"].ToString()},
                                    {"assetdetails_tfr_status",dt1.Rows[k]["assetdetails_tfr_status"].ToString()},
                                    {"assetdetails_tfr_date",string.IsNullOrEmpty(dt1.Rows[k]["assetdetails_tfr_date"].ToString()) ?   string.Empty   : convertoDate(dt1.Rows[k]["assetdetails_tfr_date"].ToString()) },
                                    {"assetdetails_tfr_id",dt1.Rows[k]["assetdetails_tfr_id"].ToString()},
                                    {"assetdetails_recon_reference",dt1.Rows[k]["assetdetails_recon_reference"].ToString()},
                                    {"assetdetails_active_status","Y"},
                                    {"assetdetails_sale_status","N"},
                                    {"assetdetails_sale_date",string.IsNullOrEmpty(dt1.Rows[k]["assetdetails_sale_date"].ToString()) ?   string.Empty   : convertoDate(dt1.Rows[k]["assetdetails_sale_date"].ToString())},
                                    {"assetdetails_not_5kcase",dt1.Rows[k]["assetdetails_not_5kcase"].ToString()},
                                    {"assetdetails_trf_value",dt1.Rows[k]["assetdetails_asset_value"].ToString()},
                                    {"assetdetails_trf_reason",dt1.Rows[k]["assetdetails_trf_reason"].ToString()},
                                    {"assetdetails_asset_owner",dt1.Rows[k]["assetdetails_asset_owner"].ToString()},
                                    {"assetdetails_trn_date",string.IsNullOrEmpty(dt1.Rows[k]["assetdetails_trn_date"].ToString()) ?   string.Empty   : convertoDate(dt1.Rows[k]["assetdetails_trn_date"].ToString())},
                                    {"assetdetails_upld_status",dt1.Rows[k]["assetdetails_upld_status"].ToString()},
                                    {"assetdetails_upld_ref",dt1.Rows[k]["assetdetails_upld_ref"].ToString()},
                                    {"assetdetails_reduced_value",string.IsNullOrEmpty(dt1.Rows[k]["assetdetails_reduced_value"].ToString()) ?  "0"   : dt1.Rows[k]["assetdetails_reduced_value"].ToString()},
                                    {"assetdetails_lease_startdate",string.IsNullOrEmpty(convertoDate(dt1.Rows[k]["assetdetails_lease_startdate"].ToString())) ?   string.Empty   : convertoDate(dt1.Rows[k]["assetdetails_lease_startdate"].ToString()) },
                                    {"assetdetails_lease_enddate",string.IsNullOrEmpty(dt1.Rows[k]["assetdetails_lease_enddate"].ToString()) ?   string.Empty   : convertoDate(dt1.Rows[k]["assetdetails_lease_enddate"].ToString())},
                                    {"assetdetails_dep_onhold",dt1.Rows[k]["assetdetails_dep_onhold"].ToString()},
                                    {"assetdetails_spoke_asset",dt1.Rows[k]["assetdetails_spoke_asset"].ToString()},
                                    {"assetdetails_impair_asset",dt1.Rows[k]["assetdetails_impair_asset"].ToString()},
                                    {"assetdetails_dep_rate",dt1.Rows[k]["assetdetails_dep_rate"].ToString()},
                                    {"assetdetails_asset_serialno",dt1.Rows[k]["assetdetails_asset_serialno"].ToString()},                               
                                    {"assetdetails_assetcode_changestatus","N"},
                                    {"assetdetails_assetcode_changedate",string.IsNullOrEmpty(dt1.Rows[k]["assetdetails_assetcode_changedate"].ToString()) ?   string.Empty   :  convertoDate(dt1.Rows[k]["assetdetails_assetcode_changedate"].ToString())},
                                    {"assetdetails_assetcode_changeid",dt1.Rows[k]["assetdetails_assetcode_changeid"].ToString()},
                                    {"assetdetails_physical_idrelease",dt1.Rows[k]["assetdetails_physical_idrelease"].ToString()},
                                    {"assetdetails_isremoved","N"},
                                    {"assetdetails_branch_gid",dt1.Rows[k]["assetdetails_branch_gid"].ToString()},
                                    {"assetdetails_addate",string.Empty},
                                    {"assetdetails_trf_from",dt1.Rows[k]["assetdetails_physical_assetid"].ToString()},
                                    {"assetdetails_insert_by",Convert.ToString(objcmnfunction.GetLoginUserGid())},
                                    {"assetdetails_insert_date",Convert.ToString("SYSDATETIME()")},
                                    {"assetdetails_takenby","14"},
                                    {"assetdetails_trf_to",string.Empty },
                                    {"assetdetails_assetid_source","2"},
                                    {"assetdetails_capnew_date",string.IsNullOrEmpty(dt1.Rows[k]["assetdetails_capnew_date"].ToString()) ?   string.Empty   : convertoDate(dt1.Rows[k]["assetdetails_capnew_date"].ToString())},
                                    {"assetdetails_capold_date",string.IsNullOrEmpty(dt1.Rows[k]["assetdetails_capold_date"].ToString()) ?   string.Empty   : convertoDate(dt1.Rows[k]["assetdetails_capold_date"].ToString())},
                                    {"assetdetails_status",string.IsNullOrEmpty(dt1.Rows[k]["assetdetails_status"].ToString()) ?  "0"   : dt1.Rows[k]["assetdetails_status"].ToString()},                                
                                    {"assetdetails_cbfnum",dt1.Rows[k]["assetdetails_cbfnum"].ToString()},
                                    {"assetdetails_cbf_gid",dt1.Rows[k]["assetdetails_cbf_gid"].ToString()},
                                    {"assetdetails_ecfnum",dt1.Rows[k]["assetdetails_ecfnum"].ToString()},
                                    {"assetdetails_ecf_gid",dt1.Rows[k]["assetdetails_ecf_gid"].ToString()}
                                    ,{"assetdetails_imp_date",string.IsNullOrEmpty(dt1.Rows[k]["assetdetails_imp_date"].ToString()) ?   string.Empty   :  convertoDate(dt1.Rows[k]["assetdetails_imp_date"].ToString())}
                                    ,{"assetdetails_invoice_gid",string.IsNullOrEmpty(dt1.Rows[k]["assetdetails_invoice_gid"].ToString()) ?   string.Empty   : dt1.Rows[k]["assetdetails_invoice_gid"].ToString()}
                                    ,{"assetdetails_inwdetailgid",string.IsNullOrEmpty(dt1.Rows[k]["assetdetails_inwdetailgid"].ToString()) ?   string.Empty   : dt1.Rows[k]["assetdetails_inwdetailgid"].ToString()}
                                    ,{"assetdetails_inwheadergid",string.IsNullOrEmpty(dt1.Rows[k]["assetdetails_inwheadergid"].ToString()) ?   string.Empty   : dt1.Rows[k]["assetdetails_inwheadergid"].ToString()}
                                    ,{"assetdetails_ponum",string.IsNullOrEmpty(dt1.Rows[k]["assetdetails_ponum"].ToString()) ?   string.Empty   : dt1.Rows[k]["assetdetails_ponum"].ToString()}
                                    ,{"assetdetails_sale_id",string.IsNullOrEmpty(dt1.Rows[k]["assetdetails_sale_id"].ToString()) ?   string.Empty   : dt1.Rows[k]["assetdetails_sale_id"].ToString()}
                                    ,{"assetdetails_wrt_date",string.IsNullOrEmpty(dt1.Rows[k]["assetdetails_wrt_date"].ToString()) ?   string.Empty   : convertoDate( dt1.Rows[k]["assetdetails_wrt_date"].ToString())}
                                    ,{"assetdetails_wrt_id",string.IsNullOrEmpty(dt1.Rows[k]["assetdetails_wrt_id"].ToString()) ?   string.Empty   : dt1.Rows[k]["assetdetails_wrt_id"].ToString()}
                                    ,{"assetdetails_wrt_status",string.IsNullOrEmpty(dt1.Rows[k]["assetdetails_wrt_status"].ToString()) ? "N" : dt1.Rows[k]["assetdetails_wrt_status"].ToString()}
};
                            string insertcom = objcmnIUD.InsertCommon(code, "fa_trn_tassetdetails");

                            //Asset Code Change
                            string[,] code1 = new string[,]
                            {
                                {"assetcodechange_old_assetid",dt1.Rows[k]["assetdetails_assetdet_id"].ToString()},
                                {"assetcodechange_new_assetid",newassetdetid.ToString()},
                                {"assetcodechange_asset_value",dt1.Rows[k]["assetdetails_asset_value"].ToString()},
                                {"assetcodechange_new_asset_code",GetAssetcodeid(newassetcod.ToString())},
                                {"assetcodechange_reason",reason.ToString()},
                                {"assetcodechange_process_date",Convert.ToString("SYSDATETIME()")},
                                {"assetcodechange_assetcode_changedate",Convert.ToString("SYSDATETIME()")},
                                {"assetcodechange_insert_by",Convert.ToString(objcmnfunction.GetLoginUserGid())},
                                {"assetcodechange_insert_date",Convert.ToString("SYSDATETIME()")},
                                {"assetcodechange_isremoved","N"}

                            };
                            string insertcomn = objcmnIUD.InsertCommon(code1, "fa_trn_tassetcodechange");

                            //DataTable dtnewas = new DataTable();
                            //cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                            //cmd.CommandType = CommandType.StoredProcedure;
                            //cmd.Parameters.AddWithValue("@action", "newsplitdet");
                            //cmd.Parameters.AddWithValue("@assetdetid", id.ToString());
                            //da = new SqlDataAdapter(cmd);
                            //da.Fill(dtnewas);
                            //foreach (DataRow row in dtnewas.Rows)
                            //{
                            //newasset = newassetdetid.ToString();

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
                            Int64 grpid = 0;
                            Int32 newassetid = 0;
                            // Int32 assetdetid = obj.assetdetgid;
                            decimal sumassetdep = 0;
                            //decimal linewdv = 0;
                            decimal rectificationamt = 0;
                            // decimal linePL = 0;




                            //sumof dep for old id

                            //dep

                            DataTable dtdep = new DataTable();
                            cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@action", "Totdep");
                            cmd.Parameters.AddWithValue("@assetgid", assetid);
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dtdep);
                            foreach (DataRow rowdep in dtdep.Rows)
                            {
                                sumassetdep = Convert.ToDecimal(rowdep["depreciation_amount"].ToString());
                            }
                            //decimal sumassetdep = Convert.ToDecimal(tmm.queue_from.ToString());



                            DataTable dtwdv = new DataTable();
                            cmd = new SqlCommand("pr_ifams_trn_ValueReduction", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@action", "WDV");
                            cmd.Parameters.AddWithValue("@assetdetDetid", newassetdetid.ToString());
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
                                grpid = Convert.ToInt64(string.IsNullOrEmpty(row1["assetdetails_asset_groupid"].ToString()) ? "0" : row1["assetdetails_asset_groupid"]);
                                newassetid = Convert.ToInt32(row1["assetdetails_gid"].ToString());
                                // CanDepreciateFully = Convert.ToString(string.IsNullOrEmpty(row1["h_ficrefnumber"].ToString()) ? "0" : row1["h_ficrefnumber"]);
                            }




                            //depupdate
                            string[,] coldepupdate = new string[,]
                            {
                                {"depreciation_assetdet_gid", newassetid.ToString()},
                            };

                            string[,] depnewwhrcol = new string[,]
	                        {
                                    {"depreciation_assetdet_gid",assetid.ToString()} , 
                                   
                            };
                            string depupdatetbname = "fa_trn_tdepreciation";
                            string depnewupdcomm = objcmnIUD.UpdateCommon(coldepupdate, depnewwhrcol, depupdatetbname);





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


                            rectificationamt = depamt - sumassetdep;


                            string[,] codesdep = new string[,]
                    {
                        //{"depreciation_amount", depamt.ToString()},
                        {"depreciation_amount", rectificationamt.ToString()},
                        {"depreciation_assetdet_gid",newassetid.ToString()},
                        {"depreciation_month", Convert.ToString("SYSDATETIME()")},
                        {"depreciation_asset_groupid", grpid.ToString()},
                        {"depreciation_asset_owner", "F"},
                        {"depreciation_type","A"},
                        {"depreciation_insert_by",Convert.ToString(objcmnfunction.GetLoginUserGid())},
                        {"depreciation_insert_date", Convert.ToString("SYSDATETIME()")}
                    };
                            string insertcmn = objcmnIUD.InsertCommon(codesdep, "fa_trn_tdepreciation");





                            //Location table entry

                            DataTable dtloc = new DataTable();
                            cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@branchgid", dt1.Rows[k]["assetdetails_branch_gid"].ToString());
                            cmd.Parameters.AddWithValue("@assetdetid", newassetdetid.ToString());
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
                            cmd.Parameters.AddWithValue("@assetdetid", newassetdetid.ToString());
                            cmd.Parameters.AddWithValue("@action", "assetcodechangecc");
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
                                {"assetlocation_asset_id",newassetdetid.ToString()},
                                {"assetlocation_location_code",branchlegacycode.ToString()},
                                {"assetlocation_location_ccfc",oraclebscccode},
                                {"assetlocation_location_fc",oracleBScode},
                                {"assetlocation_location_cc",oracleCCcode},
                                {"assetlocation_ratio","100.00"},
                                {"assetlocation_isremoved","N"},
                                {"assetlocation_insert_by",Convert.ToString(objcmnfunction.GetLoginUserGid())},
                                {"assetlocation_insert_date",Convert.ToString("SYSDATETIME()")}
                            };

                            string insertlocationcmn = objcmnIUD.InsertCommon(codeloc, "fa_trn_tassetlocation");


                            //old asset bscc oracle

                            DataTable dtoloc = new DataTable();
                            cmd = new SqlCommand("pr_ifams_trn_SplitMaker", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@branchgid", dt1.Rows[k]["assetdetails_branch_gid"].ToString());
                            cmd.Parameters.AddWithValue("@assetid", assetid.ToString());
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
                            cmd.Parameters.AddWithValue("@assetid", assetid.ToString());
                            cmd.Parameters.AddWithValue("@action", "assetcodechangeccgid");
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dtocc);
                            for (int l = 0; l < dtocc.Rows.Count; l++)
                            {
                                oldoracleCCcode = dtocc.Rows[l]["cc_code"].ToString();
                            }

                            string oldoraclebscccode = string.Empty;
                            oldoraclebscccode = oldoracleCCcode + oldoracleBScode;



                            //ENTRY FOR GL UPLOAD
                            cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "UploadDetails";
                            cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = newassetid.ToString();
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


                            cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "UploadDetails";
                            cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = assetid.ToString();
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
                            string oldratio = string.Empty;
                            DataTable oldUPLOADDATATBL = new DataTable();
                            da.Fill(oldUPLOADDATATBL);
                            if (oldUPLOADDATATBL.Rows.Count > 0)
                                foreach (DataRow rowdep in UPLOADDATATBL.Rows)
                                {
                                    oldglCode = oldUPLOADDATATBL.Rows[0]["asset_glcode"].ToString();
                                    oldresdepglCode = oldUPLOADDATATBL.Rows[0]["asset_dep_reservglcode"].ToString();
                                    olddepglCode = oldUPLOADDATATBL.Rows[0]["asset_dep_glcode"].ToString();
                                    oldbranch = oldUPLOADDATATBL.Rows[0]["branch_code"].ToString();
                                    oldcat = oldUPLOADDATATBL.Rows[0]["assetcategory_name"].ToString();
                                    if (oldcat.Length > 16)
                                        oldcat = oldcat.Substring(0, 16);
                                    oldsubcat = oldUPLOADDATATBL.Rows[0]["asset_code"].ToString();
                                    oldasset_id = oldUPLOADDATATBL.Rows[0]["assetdetails_assetdet_id"].ToString();
                                }


                            string[,] glCoulmsD = new string[,]
                            {                                
                                {"tran_date",objidm.convertoDate(DateTime.Today.ToShortDateString()).ToString()},
                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
                                // {"tran_desc", asset_id.ToString() + "|" + inwno + "SALE OF ASSET" + "|" + "Inward Reference number" },
                                {"tran_desc", newassetdetid.ToString() + "|"  + "ASSET CODE CHANGE"},
                                {"tran_gl_no",glCode},
                                {"tran_amount",AssetValue.ToString()},
                                {"tran_acc_mode","D".ToString()},
                                {"tran_mult","-1"},  
                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"] },
                                // {"tran_fc_code",BS},oracleBScode
                                {"tran_fc_code",oracleBScode},
                                //{"tran_cc_code",CC}, oracleCCcode  
                                {"tran_cc_code",oracleCCcode},
                                {"tran_ou_code",branch},
                                {"tran_ref_flag",objidm.GetRef("ACC")},
                                {"tran_ref_gid",  newassetid.ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                                {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                {"tran_expense_category","1"},
                                {"tran_primary_branch_code",branch.ToString()},                          
                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                                //{"tran_maker_id",objcmnfunction.GetLoginUserGid().ToString()},
                                {"tran_checker_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                {"tran_emp_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                            string insertforglD = objcmnIUD.InsertCommon(glCoulmsD, "fa_trn_ttran");
                            string[,] glCoulmsC = new string[,]
                            {                                
                                {"tran_date",objidm.convertoDate(DateTime.Today.ToShortDateString()).ToString()},
                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_desc",acc.assetdetdetid.ToString() + "|" + "ASSET CODE CHANGE" },
                                {"tran_gl_no",oldglCode},
                                {"tran_amount",AssetValue.ToString()},
                                {"tran_acc_mode","C".ToString()},
                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                {"tran_mult","1"},  
                                // {"tran_fc_code",oldBS},oldoracleBScode
                                {"tran_fc_code",oldoracleBScode},
                                //{"tran_cc_code",oldCC },
                                {"tran_cc_code",oldoracleCCcode},  
                                {"tran_ou_code",oldbranch},
                                {"tran_ref_flag",objidm.GetRef("ACC")},
                                {"tran_ref_gid",  assetid.ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                                {"tran_main_cat",cat},
                                {"tran_sub_cat",oldsubcat},
                                {"tran_expense_category","1"},
                                {"tran_primary_branch_code",oldbranch.ToString()},                         
                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_checker_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                {"tran_emp_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                            string insertforglC = objcmnIUD.InsertCommon(glCoulmsC, "fa_trn_ttran");

                            //decimal sumassetdep = 0;
                            DataTable dtdeptrn = new DataTable();
                            cmd = new SqlCommand("pr_ifams_trn_ValueReduction", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@action", "Totdep");
                            cmd.Parameters.AddWithValue("@assetdetDetid", newassetdetid);
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dtdeptrn);
                            foreach (DataRow rowdep in dtdeptrn.Rows)
                            {
                                sumassetdep = Convert.ToDecimal(rowdep["depreciation_amount"].ToString());
                            }

                            //sumassetdep = Convert.ToDecimal(obj.assetdep.ToString());


                            string[,] glCoulmsDepD = new string[,]
                            {                             
                                {"tran_date",objidm.convertoDate(DateTime.Today.ToShortDateString()).ToString()},
                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_gl_no",oldresdepglCode},
                                {"tran_desc", acc.assetdetdetid.ToString() + "|" +"ASSET CODE CHANGE" },
                                //{"tran_amount",depamt.ToString()}, Muthu
                                {"tran_amount",sumassetdep.ToString()},
                                {"tran_acc_mode","D".ToString()},
                                {"tran_mult","-1"},  
                                {"tran_fc_code",oldoracleBScode },
                                {"tran_cc_code",oldoracleCCcode },
                                {"tran_ou_code",oldbranch},
                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                {"tran_ref_flag",objidm.GetRef("ACC")},
                                {"tran_ref_gid",assetid.ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                                {"tran_main_cat",cat},
                                {"tran_sub_cat",oldsubcat},
                                {"tran_expense_category","1"},
                                {"tran_primary_branch_code",oldbranch.ToString()}, 
                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_checker_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                {"tran_emp_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                            string insertforglDepD = objcmnIUD.InsertCommon(glCoulmsDepD, "fa_trn_ttran");





                            string[,] glCoulmsDepC = new string[,]
                            {                                
                                {"tran_date",objidm.convertoDate(DateTime.Today.ToShortDateString()).ToString()},
                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_desc",newassetdetid.ToString() + "|" +"ASSET CODE CHANGE"},
                                {"tran_gl_no",resdepglCode},
                                // {"tran_amount",depamt.ToString()},Muthu
                                {"tran_amount",sumassetdep.ToString()},
                                {"tran_acc_mode","C".ToString()},
                                {"tran_mult","1"},  
                                {"tran_fc_code",oldoracleBScode },
                                {"tran_cc_code",oldoracleCCcode },  
                                // {"tran_product_code","999".ToString()},
                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                {"tran_ou_code",branch},
                                {"tran_ref_flag",objidm.GetRef("ACC")},
                                {"tran_ref_gid", newassetid.ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                                {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                {"tran_expense_category","1"},
                                {"tran_primary_branch_code",branch.ToString()},
                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_checker_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                {"tran_emp_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                            string insertforglDepC = objcmnIUD.InsertCommon(glCoulmsDepC, "fa_trn_ttran");
                            string[,] glCoulmsDepD1 = new string[,]
                            {                             
                                {"tran_date",objidm.convertoDate(DateTime.Today.ToShortDateString()).ToString()},
                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_gl_no",depglCode},
                                {"tran_desc",newassetdetid.ToString() + "|" + "ASSET CODE CHANGE" },
                                //{"tran_amount",depamt.ToString()},Muthu
                                {"tran_amount",sumassetdep.ToString()},
                                {"tran_acc_mode","D".ToString()},
                                {"tran_mult","-1"},  
                                {"tran_fc_code",oldoracleBScode },
                                {"tran_cc_code",oldoracleCCcode },
                                {"tran_ou_code",branch},
                                // {"tran_product_code","999".ToString()},
                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                {"tran_ref_flag",objidm.GetRef("ACC")},
                                {"tran_ref_gid",newassetid.ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                                {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                {"tran_expense_category","1"},
                                {"tran_primary_branch_code",branch.ToString()}, 
                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_checker_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                {"tran_emp_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                            string insertforglDepD1 = objcmnIUD.InsertCommon(glCoulmsDepD1, "fa_trn_ttran");
                            string[,] glCoulmsDepC1 = new string[,]
                            {     
                                {"tran_date",objidm.convertoDate(DateTime.Today.ToShortDateString()).ToString()},
                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_desc", acc.assetdetdetid.ToString() + "|" + "ASSET CODE CHANGE" },
                                {"tran_gl_no",olddepglCode},
                               // {"tran_amount",depamt.ToString()},Muthu
                                {"tran_amount",sumassetdep.ToString()},
                                {"tran_acc_mode","C".ToString()},
                                {"tran_mult","1"},  
                                {"tran_fc_code",oldoracleBScode },
                                {"tran_cc_code",oldoracleCCcode },  
                               // {"tran_product_code","999".ToString()},
                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                {"tran_ou_code",oldbranch},
                                {"tran_ref_flag",objidm.GetRef("ACC")},
                                {"tran_ref_gid", assetid.ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                              {"tran_main_cat",cat},
                                {"tran_sub_cat",oldsubcat},
                              {"tran_expense_category","1"},
                                {"tran_primary_branch_code",branch.ToString()},
                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                                 {"tran_checker_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                 {"tran_emp_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                 {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                            string insertforglDepC1 = objcmnIUD.InsertCommon(glCoulmsDepC1, "fa_trn_ttran");


                            //tran table insert for ACC


                            if (rectificationamt != 0)
                            {
                                if (rectificationamt < 0)
                                {
                                    rectificationamt = Math.Abs(rectificationamt);


                                    string[,] glCoulmsACCDepC = new string[,]
                            {                                
                                {"tran_date",objidm.convertoDate(DateTime.Today.ToShortDateString()).ToString()},
                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_desc",newassetdetid.ToString() + "|" + "ASSET CODE CHANGE" },
                                {"tran_gl_no",depglCode},
                                // {"tran_amount",depamt.ToString()},Muthu
                                // {"tran_amount", (depamt - sumassetdep).ToString()}, -- 2019-22-07
                                {"tran_amount", rectificationamt.ToString()},
                                {"tran_acc_mode","C".ToString()},
                                {"tran_mult","1"},  
                                {"tran_fc_code",BS },
                                {"tran_cc_code",CC },  
                                //{"tran_product_code","999".ToString()},
                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                {"tran_ou_code",branch},
                                {"tran_ref_flag",objidm.GetRef("ACCDEP")},
                                {"tran_ref_gid", newassetid.ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                                {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                {"tran_expense_category","10"},
                                {"tran_primary_branch_code",branch.ToString()},
                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_checker_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                {"tran_emp_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                                    string insertforACCglDep = objcmnIUD.InsertCommon(glCoulmsACCDepC, "fa_trn_ttran");



                                    string[,] glCoulmsACCDepD = new string[,]
                            {                                
                                {"tran_date",objidm.convertoDate(DateTime.Today.ToShortDateString()).ToString()},
                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_desc",newassetdetid.ToString() + "|" +  "ASSET CODE CHANGE"},
                                {"tran_gl_no",resdepglCode},
                                //{"tran_amount",depamt.ToString()},Muthu
                                //{"tran_amount", (depamt - sumassetdep).ToString()}, 2019-07-22
                                {"tran_amount", rectificationamt.ToString()},
                                {"tran_acc_mode","D".ToString()},
                                {"tran_mult","1"},  
                                {"tran_fc_code",BS },
                                {"tran_cc_code",CC },  
                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                {"tran_ou_code",branch},
                                {"tran_ref_flag",objidm.GetRef("ACCDEP")},
                                {"tran_ref_gid", newassetid.ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                                {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                {"tran_expense_category","10"},
                                {"tran_primary_branch_code",branch.ToString()},
                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_checker_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                {"tran_emp_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                                    string insertforACCglDepD = objcmnIUD.InsertCommon(glCoulmsACCDepD, "fa_trn_ttran");
                                }

                                else
                                {
                                    string[,] glCoulmsACCDepC = new string[,]
                            {                                
                                {"tran_date",objidm.convertoDate(DateTime.Today.ToShortDateString()).ToString()},
                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_desc",newassetdetid.ToString() + "|" +"ASSET CODE CHANGE" },
                                {"tran_gl_no",resdepglCode},
                                // {"tran_amount",depamt.ToString()},Muthu
                                // {"tran_amount", (depamt - sumassetdep).ToString()},
                                {"tran_amount", rectificationamt.ToString()}, //22-07-2019 
                                {"tran_acc_mode","C".ToString()},
                                {"tran_mult","1"},  
                                {"tran_fc_code",BS },
                                {"tran_cc_code",CC },  
                                //{"tran_product_code","999".ToString()},
                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                {"tran_ou_code",branch},
                                {"tran_ref_flag",objidm.GetRef("ACCDEP")},
                                {"tran_ref_gid", newassetid.ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                                {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                {"tran_expense_category","10"},
                                {"tran_primary_branch_code",branch.ToString()},
                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_checker_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                {"tran_emp_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                                    string insertforACCglDep = objcmnIUD.InsertCommon(glCoulmsACCDepC, "fa_trn_ttran");



                                    string[,] glCoulmsACCDepD = new string[,]
                            {                                
                                {"tran_date",objidm.convertoDate(DateTime.Today.ToShortDateString()).ToString()},
                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_desc",newassetdetid.ToString() + "|" +  "ASSET CODE CHANGE" },
                                {"tran_gl_no",depglCode},
                                // {"tran_amount",depamt.ToString()},Muthu
                                // {"tran_amount", (depamt - sumassetdep).ToString()}, //2019-07-22 As per EPU request we will change the depamt as rectification amount for both HFC & FIC

                                {"tran_acc_mode","D".ToString()},
                                {"tran_mult","1"},  
                                {"tran_fc_code",BS },
                                {"tran_cc_code",CC },  
                                {"tran_product_code", ConfigurationManager.AppSettings["Productcode"]},
                                {"tran_ou_code",branch},
                                {"tran_ref_flag",objidm.GetRef("ACCDEP")},
                                {"tran_ref_gid", newassetid.ToString()},
                                {"tran_upload_gid","0".ToString()},
                                {"tran_isremoved","N"},
                                {"tran_main_cat",cat},
                                {"tran_sub_cat",subcat},
                                {"tran_expense_category","10"},
                                {"tran_primary_branch_code",branch.ToString()},
                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
                                {"tran_checker_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                {"tran_emp_id",Convert.ToString(HttpContext.Current.Session["employee_code"])},
                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
                            };
                                    string insertforACCglDepD = objcmnIUD.InsertCommon(glCoulmsACCDepD, "fa_trn_ttran");
                                }
                            }

                        }
                    }


                }
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_ValueReduction", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "deletedepreciation");
                cmd.Parameters.AddWithValue("@assetdetDetid", oldassetid);
                cmd.ExecuteNonQuery();
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            return "success";
        }

        public string ACCGetid(string assetid)
        {
            string result = string.Empty;
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_AssetCodeChange", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@assetid", assetid);
                cmd.Parameters.AddWithValue("@action", "AssetId");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result = dt.Rows[0]["assetdetails_gid"].ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                objErrLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return result;
            }
            finally
            {
                con.Close();
            }
        }

        public string ACCGetbranch(string assetid)
        {
            string result = string.Empty;
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_AssetCodeChange", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@assetid", assetid);
                cmd.Parameters.AddWithValue("@action", "AssetId");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result = dt.Rows[0]["assetdetails_branch_gid"].ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                objErrLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return result;
            }
            finally
            {
                con.Close();
            }
        }

        public string ACCGetLegacy(string branchid)
        {
            string result = string.Empty;
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_AssetCodeChange", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@branchid", branchid);
                cmd.Parameters.AddWithValue("@action", "legacy");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result = dt.Rows[0]["branch_code"].ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                objErrLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return result;
            }
            finally
            {
                con.Close();
            }
        }


        public DataTable GetAssetdettable(string assetid)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                //DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_AssetCodeChange", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@assetid", assetid);
                cmd.Parameters.AddWithValue("@action", "assetdettab");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                objErrLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return dt;
            }
            finally
            {
                con.Close();
            }
        }



        public string GetAssetcodeid(string assetcode)
        {
            string result = string.Empty;
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_AssetCodeChange", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@assetcode", assetcode);
                cmd.Parameters.AddWithValue("@action", "AssetCode");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result = dt.Rows[0]["asset_gid"].ToString();
                }
                return result;
            }
            catch (Exception ex)
            {
                objErrLog.WriteErrorLog(ex.Message.ToString(), ex.ToString()); return result;
            }
            finally
            {
                con.Close();
            }
        }


        public string ACCGetsource(string source)
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
                objErrLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return result;
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
                objErrLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return string.Empty;
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
    }
}