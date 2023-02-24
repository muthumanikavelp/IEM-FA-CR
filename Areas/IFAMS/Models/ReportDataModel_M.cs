using IEM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace IEM.Areas.IFAMS.Models
{
    public class ReportDataModel_M : ReportRepository_M
    {
        SqlConnection con = new SqlConnection();
        // ReportDataModel_M objrdm = new ReportDataModel_M();
        DataModel objidm = new DataModel();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions objcmnfunc = new CmnFunctions();
        CommonIUD objcmnIUD = new CommonIUD();
        ErrorLog objerrlog = new ErrorLog();
        string duplicateasset = "";
        private void GetConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }

        public IEnumerable<AssetReportModel> GetSalereport()
        {
            List<AssetReportModel> objmod = new List<AssetReportModel>();
            AssetReportModel obj = new AssetReportModel();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //  cmd.Parameters.AddWithValue("@Makerid",loginid);
                cmd.Parameters.AddWithValue("@action", "SaleReport");
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetReportModel();
                    obj.soaProcessdate = row["soaheader_process_date"].ToString();
                    obj.soaSalenumber = row["soaheader_sale_number"].ToString();
                    obj.soainvno = row["soaheader_inv_no"].ToString();
                    obj.soaSalevalue = Convert.ToDecimal(row["soaheader_sale_value"].ToString());
                    obj.soaVatper = row["soaheader_vat_percentage"].ToString();
                    obj.soaVatamt = row["soaheader_vat_amount"].ToString();
                    obj.soaSaledate = row["soaheader_sale_date"].ToString();
                    obj.soaStatus = Getstatus(row["soaheader_sale_status"].ToString());
                    obj.soaMakerid = Getempid(row["soaheader_maker_id"].ToString());
                    obj.soaCheckerid = Getempid(row["soaheader_checker_id"].ToString());
                    obj.soaJobcode = Convert.ToInt32(row["soaheader_job_code"].ToString());
                    obj.assetdettrndate = row["assetdetails_trn_date"].ToString();
                    obj.assetdetDetid = row["assetdetails_assetdet_id"].ToString();
                    obj.assetdetAssetvalue = Convert.ToDecimal(row["assetdetails_asset_value"].ToString());
                    obj.soadetSalevalue = Convert.ToDecimal(row["soadetail_sale_value"].ToString());
                    obj.soadetVatvalue = Convert.ToDecimal(row["soadetail_vat_value"].ToString());
                    obj.soadetProfitloss = Convert.ToDecimal(row["soadetial_pl_value"].ToString());
                    obj.soadetrectamt = 0;//row["soaheader_sale_number"].ToString();
                    obj.assetdettaken = Gettakenby(row["assetdetails_takenby"].ToString());
                    obj.soapayGL = row["soapayment_bank_gl"].ToString();
                    obj.soapaymode = row["soapayment_pay_mode"].ToString();
                    obj.soapayChqno = row["soapayment_chq_no"].ToString();
                    obj.soapayAmount = row["soapayment_amount"].ToString();

                    objmod.Add(obj);
                }
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return objmod;
        }

        public IEnumerable<AssetReportModel> GetTransreport()
        {
            List<AssetReportModel> objmod = new List<AssetReportModel>();
            AssetReportModel obj = new AssetReportModel();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "TransReport");
                cmd.CommandTimeout = 0;
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                HttpContext.Current.Session["taexcel"] = dt;
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetReportModel();
                    //obj.toaProcessdate = Convert.ToString(string.IsNullOrEmpty(row["toaheader_process_date"].ToString()) ? "0" : row["toaheader_process_date"]);
                    obj.toaProcessdate = row["toaheader_process_date"].ToString();
                    obj.toaTrannumber = row["toaheader_toa_number"].ToString();
                    obj.assetdetgroupid = GetGroupid(row["assetdetails_asset_groupid"].ToString());
                    obj.assetdetDetid = row["assetdetails_assetdet_id"].ToString();
                    obj.assetdetAssetvalue = Convert.ToDecimal(row["assetdetails_asset_value"].ToString());
                    obj.assetdettrfval = Convert.ToDecimal(row["assetdetails_trf_value"].ToString());
                    obj.toadetnewassetdetid = row["toadetail_new_assetdetgid"].ToString();
                    obj.assetdettrndate = Convert.ToString(string.IsNullOrEmpty(row["assetdetails_tfr_date"].ToString()) ? "0" : row["assetdetails_tfr_date"]);
                    obj.toadetrectamt = Convert.ToDecimal(row["toadetail_rectif_amount"].ToString());
                    obj.toaStatus = Getstatus(row["toaheader_tfr_status"].ToString());
                    obj.toaJobcode = row["toaheader_jobcode"].ToString();
                    obj.toaUploasdate = Convert.ToString(string.IsNullOrEmpty(row["toaheader_upld_date"].ToString()) ? "0" : row["toaheader_upld_date"]);
                    obj.toatfrdate = Convert.ToString(string.IsNullOrEmpty(row["toaheader_tfr_date"].ToString()) ? "0" : row["toaheader_tfr_date"]);
                    obj.toatfrvaluedate = Convert.ToString(string.IsNullOrEmpty(row["toaheader_value_date"].ToString()) ? "0" : row["toaheader_value_date"]);
                    obj.toaMakerid = Getempid(row["toaheader_maker_id"].ToString());
                    obj.toaCheckerid = Getempid(row["toaheader_checker_id"].ToString());
                    objmod.Add(obj);
                }
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return (objmod);
        }


        public DataTable GetTransferangular()
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "TransReport");

                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }


            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return (dt);
        }
        public IEnumerable<AssetReportModel> GetWAreport()
        {
            List<AssetReportModel> objmod = new List<AssetReportModel>();
            AssetReportModel obj = new AssetReportModel();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "WriteReport");
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetReportModel();
                    // obj.woaProcessdate = Convert.ToString(string.IsNullOrEmpty(row["woaheader_process_date"].ToString()) ? "0" : row["woaheader_process_date"]);
                    obj.woaProcessdate = row["woaheader_process_date"].ToString();
                    obj.woaWOnumber = row["woaheader_woa_number"].ToString();
                    obj.woaStatus = Getstatus(row["woaheader_woa_status"].ToString());
                    obj.woaJobcode = row["woaheader_jobcode"].ToString();
                    obj.woatfrdate = Convert.ToString(string.IsNullOrEmpty(row["woaheader_trans_date"].ToString()) ? "0" : row["woaheader_trans_date"]);
                    obj.woatfrvaluedate = Convert.ToString(string.IsNullOrEmpty(row["woaheader_value_date"].ToString()) ? "0" : row["woaheader_value_date"]);
                    obj.woaMakerid = Getempid(row["woaheader_maker_id"].ToString());
                    obj.woaCheckerid = Getempid(row["woaheader_checker_id"].ToString());
                    obj.assetdetDetid = row["assetdetails_assetdet_id"].ToString();
                    obj.assetdetAssetvalue = Convert.ToDecimal(row["assetdetails_asset_value"].ToString());
                    obj.woadeptilldate = 0;
                    obj.assetdettaken = Gettakenby(row["assetdetails_takenby"].ToString());
                    obj.woaWOdate = Convert.ToString(string.IsNullOrEmpty(row["woadetail_woa_date"].ToString()) ? "0" : row["woadetail_woa_date"]);
                    obj.woadetrectamt = Convert.ToDecimal(row["woadetail_rectif_amount"].ToString());
                    objmod.Add(obj);
                }
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return (objmod);
        }

        public IEnumerable<AssetReportModel> GetACCreport()
        {
            List<AssetReportModel> objmod = new List<AssetReportModel>();
            AssetReportModel obj = new AssetReportModel();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "ACCReport");
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetReportModel();
                    obj.assetdetgroupid = GetGroupid(row["assetdetails_asset_groupid"].ToString());
                    obj.assetdetDetid = row["assetcodechange_old_assetid"].ToString();
                    obj.assetdetCode = row["oldasset_code"].ToString();
                    obj.accnewassetdetid = row["assetcodechange_new_assetid"].ToString();
                    obj.accnewassetcode = row["newasset_code"].ToString();
                    obj.assetdetAssetvalue = Convert.ToDecimal(row["assetcodechange_asset_value"].ToString());
                    obj.accupdateby = Getempid(row["assetcodechange_insert_by"].ToString());
                    obj.accupdatedate = Convert.ToString(string.IsNullOrEmpty(row["assetcodechange_insert_date"].ToString()) ? "0" : row["assetcodechange_insert_date"]);
                    obj.Reason = row["assetcodechange_reason"].ToString();

                    objmod.Add(obj);
                }
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return (objmod);
        }

        public IEnumerable<AssetReportModel> GetPIDreport()
        {
            List<AssetReportModel> objmod = new List<AssetReportModel>();
            AssetReportModel obj = new AssetReportModel();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "PIDReport");
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetReportModel();
                    obj.assetdetgroupid = row["assetdetails_asset_groupid"].ToString();
                    obj.assetdetDetid = row["assetdetails_assetdet_id"].ToString();
                    obj.goaphysicalID = row["goaheader_physicalid"].ToString();
                    obj.goaupdatedate = Convert.ToString(string.IsNullOrEmpty(row["goaheader_insert_date"].ToString()) ? "0" : row["goaheader_insert_date"]);
                    obj.goaupdateby = Getempid(row["goaheader_insert_by"].ToString());
                    obj.goastatus = Getstatus(row["goadetail_status"].ToString());
                    objmod.Add(obj);
                }
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return (objmod);
        }
        public string Getdatefyyyymmdd(string inputdate)
        {
            string lsInputDate = inputdate;
            string convertdate = string.Empty;
            DateTime outputDate = Convert.ToDateTime(inputdate);
            string format = "yyyy-MM-dd";
            convertdate = outputDate.ToString(format);
            return convertdate;
        }
        public DataTable GetFAreport(string depfd, string deptd, string dummy)
        {
            duplicateasset = "";
            DataTable dt = new DataTable();
            List<AssetReportModel> objmod = new List<AssetReportModel>();
            AssetReportModel obj = new AssetReportModel();
            AssetReportModel obj1 = new AssetReportModel();
            try
            {
                GetConnection();
                int set = 0;
                cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "FAReport");
                cmd.Parameters.AddWithValue("@FromDate", "FAReport");
                cmd.Parameters.AddWithValue("@ToDate", "FAReport");
                DataModel dm = new DataModel();
                string fin = dm.toGetFincialyear();
                string[] finYr = fin.Split('-');
                string curr0 = finYr[0] + "-04-01";
                string curr2 = finYr[1] + "-03-01";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                int samllcount = dt.Rows.Count;
                string years = "";
                int i = 1;
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetReportModel();
                    obj.inwarddate = " 0 ";
                    obj.assetdetgid = Convert.ToInt32(Convert.ToString(string.IsNullOrEmpty(row["assetgid"].ToString()) ? "0" : row["assetgid"]));
                    obj.assetdetgroupid = Convert.ToString(string.IsNullOrEmpty(row["GROUP_ID"].ToString()) ? " 0 " : row["GROUP_ID"]);
                    obj.assetdetCode = Convert.ToString(string.IsNullOrEmpty(row["ASSET_CODE"].ToString()) ? " 0 " : row["ASSET_CODE"]);
                    obj.assetdetDescription = Convert.ToString(string.IsNullOrEmpty(row["DESCRIPTION"].ToString()) ? " 0 " : row["DESCRIPTION"]);
                    obj.assetdetglcode = Convert.ToString(string.IsNullOrEmpty(row["GLCODE"].ToString()) ? " 0 " : row["GLCODE"]);
                    obj.deptype = Convert.ToString(string.IsNullOrEmpty(row["DEPR_TYPE"].ToString()) ? " 0 " : row["DEPR_TYPE"]);
                    obj.deprate = Convert.ToString(string.IsNullOrEmpty(row["DEPR_RATE"].ToString()) ? " 0 " : row["DEPR_RATE"]);
                    obj.depgl = Convert.ToString(string.IsNullOrEmpty(row["DEPR_GL"].ToString()) ? " 0 " : row["DEPR_GL"]);
                    obj.deppvgl = Convert.ToString(string.IsNullOrEmpty(row["DEPR_PV_GL"].ToString()) ? " 0 " : row["DEPR_PV_GL"]);
                    obj.assetdetLocationcode = Convert.ToString(string.IsNullOrEmpty(row["BRANCH_CODE"].ToString()) ? " 0 " : row["BRANCH_CODE"]);
                    obj.branchlaunchdat = Convert.ToString(string.IsNullOrEmpty(row["BRANCH_LAUNCH_DATE"].ToString()) ? " 0 " : row["BRANCH_LAUNCH_DATE"]);
                    obj.assetleasedat = Convert.ToString(string.IsNullOrEmpty(row["LEASE_START_DATE"].ToString()) ? " 0 " : row["LEASE_START_DATE"]);
                    obj.assetleaseenddat = Convert.ToString(string.IsNullOrEmpty(row["LEASE_END_DATE"].ToString()) ? " 0 " : row["LEASE_END_DATE"]);
                    obj.assetcapdate = Convert.ToString(string.IsNullOrEmpty(row["CAPITALISATION_DATE"].ToString()) ? " 0 " : row["CAPITALISATION_DATE"]);
                    obj.assetdetAssetvalue = Convert.ToDecimal(row["ASSET_VALUE"].ToString());


                    if (obj.assetdetgid != 0 && depfd != "" && depfd != "null" && depfd != "0" && deptd != "" && deptd != "null" && deptd != "0")
                    {
                        DateTime f = Convert.ToDateTime(depfd);
                        DateTime t = Convert.ToDateTime(deptd);
                        int a = (t.Year - f.Year);
                        int year = f.Year;
                        cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "getgrpcount");
                        cmd.Parameters.AddWithValue("@grpgid", obj.assetdetgroupid);
                        DataTable dtgetgrpcount = new DataTable();
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dtgetgrpcount);
                        int acount = Convert.ToInt32(dtgetgrpcount.Rows[0][0].ToString());
                        int bcount = Convert.ToInt32(dtgetgrpcount.Rows[1][0].ToString());
                        if (acount == bcount)
                        {
                            if ((duplicateasset.Contains(obj.assetdetgroupid) == false) || obj.assetdetgid == 0)
                            {
                                cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@action", "accelrateDepgrp");
                                cmd.Parameters.AddWithValue("@FromDate", f);
                                cmd.Parameters.AddWithValue("@grpgid", obj.assetdetgroupid);
                                obj.ACCUMULATED_DEP = Convert.ToString(cmd.ExecuteScalar());

                                cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@action", "cumDepgrp");
                                cmd.Parameters.AddWithValue("@FromDate", curr0);
                                cmd.Parameters.AddWithValue("@ToDate", curr2);
                                cmd.Parameters.AddWithValue("@grpgid", obj.assetdetgroupid);
                                obj.CUMMULATIVE_DEP = Convert.ToString(cmd.ExecuteScalar());

                                obj.assetdetDetid = "-";
                                obj.assetdetqty = Convert.ToString(string.IsNullOrEmpty(row["Count_of"].ToString()) ? " 0 " : row["Count_of"]);
                                duplicateasset += duplicateasset == "" ? obj.assetdetgroupid : ("," + obj.assetdetgroupid);
                                for (int yearCount = 0; yearCount <= a; yearCount++)
                                {
                                    set = (f.AddYears(yearCount)).Year;
                                    cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@action", "FAReportdep");
                                    cmd.Parameters.AddWithValue("@assetdetgrpid", obj.assetdetgroupid);
                                    cmd.Parameters.AddWithValue("@FromDate", f.Year.ToString());
                                    cmd.Parameters.AddWithValue("@ToDate", t.Year.ToString());
                                    cmd.Parameters.AddWithValue("@Year", set);
                                    DataTable dt1 = new DataTable();
                                    da = new SqlDataAdapter(cmd);
                                    da.Fill(dt1);
                                    years = obj.Year;
                                    if (dt1.Rows.Count > 0)
                                    {
                                        foreach (DataRow row1 in dt1.Rows)
                                        {
                                            obj1 = new AssetReportModel();
                                            obj1.inwarddate = " 0 ";
                                            if (years == "")
                                            {
                                                obj.Year = Convert.ToString(f.Year);
                                                obj.depjan = Convert.ToString(string.IsNullOrEmpty(row1[set + "-01-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-01-01 00:00:00.000"]); //row1["Jan"].ToString();
                                                obj.depfeb = Convert.ToString(string.IsNullOrEmpty(row1[set + "-02-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-02-01 00:00:00.000"]); //row1[""+set+"-02-01 00:00:00.000"].ToString();
                                                obj.depmar = Convert.ToString(string.IsNullOrEmpty(row1[set + "-03-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-03-01 00:00:00.000"]); //row1[""+set+"-02-01 00:00:00.000"].ToString();
                                                obj.depapr = Convert.ToString(string.IsNullOrEmpty(row1[set + "-04-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-04-01 00:00:00.000"]); //row1[""+set+"-04-01 00:00:00.000"].ToString();
                                                obj.depmay = Convert.ToString(string.IsNullOrEmpty(row1[set + "-05-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-05-01 00:00:00.000"]); //row1[""+set+"-05-01 00:00:00.000"].ToString();
                                                obj.depjun = Convert.ToString(string.IsNullOrEmpty(row1[set + "-06-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-06-01 00:00:00.000"]); //row1[""+set+"-06-01 00:00:00.000"].ToString();
                                                obj.depjul = Convert.ToString(string.IsNullOrEmpty(row1[set + "-07-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-07-01 00:00:00.000"]); //row1[""+set+"-07-01 00:00:00.000"].ToString();
                                                obj.depaug = Convert.ToString(string.IsNullOrEmpty(row1[set + "-08-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-08-01 00:00:00.000"]); //row1[""+set+"-08-01 00:00:00.000"].ToString();
                                                obj.depsep = Convert.ToString(string.IsNullOrEmpty(row1[set + "-09-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-09-01 00:00:00.000"]); //row1[""+set+"-09-01 00:00:00.000"].ToString();
                                                obj.depoct = Convert.ToString(string.IsNullOrEmpty(row1[set + "-10-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-10-01 00:00:00.000"]); //row1[""+set+"-10-01 00:00:00.000"].ToString();
                                                obj.depnov = Convert.ToString(string.IsNullOrEmpty(row1[set + "-11-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-11-01 00:00:00.000"]); //row1[""+set+"-11-01 00:00:00.000"].ToString();
                                                obj.depdec = Convert.ToString(string.IsNullOrEmpty(row1[set + "-12-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-12-01 00:00:00.000"]); //row1[""+set+"-12-01 00:00:00.000"].ToString();
                                            }
                                            else
                                            {
                                                obj.Year = years + ", " + set;
                                                obj.depjan = Convert.ToString(string.IsNullOrEmpty(row1[set + "-01-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-01-01 00:00:00.000"]); //row1["Jan"].ToString();
                                                obj.depfeb = Convert.ToString(string.IsNullOrEmpty(row1[set + "-02-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-02-01 00:00:00.000"]); //row1[""+set+"-02-01 00:00:00.000"].ToString();
                                                obj.depmar = Convert.ToString(string.IsNullOrEmpty(row1[set + "-03-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-03-01 00:00:00.000"]); //row1[""+set+"-02-01 00:00:00.000"].ToString();
                                                obj.depapr = Convert.ToString(string.IsNullOrEmpty(row1[set + "-04-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-04-01 00:00:00.000"]); //row1[""+set+"-04-01 00:00:00.000"].ToString();
                                                obj.depmay = Convert.ToString(string.IsNullOrEmpty(row1[set + "-05-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-05-01 00:00:00.000"]); //row1[""+set+"-05-01 00:00:00.000"].ToString();
                                                obj.depjun = Convert.ToString(string.IsNullOrEmpty(row1[set + "-06-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-06-01 00:00:00.000"]); //row1[""+set+"-06-01 00:00:00.000"].ToString();
                                                obj.depjul = Convert.ToString(string.IsNullOrEmpty(row1[set + "-07-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-07-01 00:00:00.000"]); //row1[""+set+"-07-01 00:00:00.000"].ToString();
                                                obj.depaug = Convert.ToString(string.IsNullOrEmpty(row1[set + "-08-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-08-01 00:00:00.000"]); //row1[""+set+"-08-01 00:00:00.000"].ToString();
                                                obj.depsep = Convert.ToString(string.IsNullOrEmpty(row1[set + "-09-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-09-01 00:00:00.000"]); //row1[""+set+"-09-01 00:00:00.000"].ToString();
                                                obj.depoct = Convert.ToString(string.IsNullOrEmpty(row1[set + "-10-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-10-01 00:00:00.000"]); //row1[""+set+"-10-01 00:00:00.000"].ToString();
                                                obj.depnov = Convert.ToString(string.IsNullOrEmpty(row1[set + "-11-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-11-01 00:00:00.000"]); //row1[""+set+"-11-01 00:00:00.000"].ToString();
                                                obj.depdec = Convert.ToString(string.IsNullOrEmpty(row1[set + "-12-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-12-01 00:00:00.000"]); //row1[""+set+"-12-01 00:00:00.000"].ToString();
                                            }
                                            HttpContext.Current.Session["obj1.Year"] = obj.Year;
                                            HttpContext.Current.Session["obj1.depjan"] = obj.depjan;
                                            HttpContext.Current.Session["obj1.depfeb"] = obj.depfeb;
                                            HttpContext.Current.Session["obj1.depmar"] = obj.depmar;
                                            HttpContext.Current.Session["obj1.depapr"] = obj.depapr;
                                            HttpContext.Current.Session["obj1.depmay"] = obj.depmay;
                                            HttpContext.Current.Session["obj1.depjun"] = obj.depjun;
                                            HttpContext.Current.Session["obj1.depjul"] = obj.depjul;
                                            HttpContext.Current.Session["obj1.depaug"] = obj.depaug;
                                            HttpContext.Current.Session["obj1.depsep"] = obj.depsep;
                                            HttpContext.Current.Session["obj1.depoct"] = obj.depoct;
                                            HttpContext.Current.Session["obj1.depnov"] = obj.depnov;
                                            HttpContext.Current.Session["obj1.depdec"] = obj.depdec;
                                        }
                                    }
                                    else
                                    {
                                        if (obj.Year != "" && obj.Year != null)
                                            obj.Year = years + ", " + set;
                                        else
                                            obj.Year = set.ToString();
                                        obj.depjan = " 0 ".ToString();
                                        obj.depfeb = " 0 ".ToString();
                                        obj.depmar = " 0 ".ToString();
                                        obj.depapr = " 0 ".ToString();
                                        obj.depmay = " 0 ".ToString();
                                        obj.depjun = " 0 ".ToString();
                                        obj.depjul = " 0 ".ToString();
                                        obj.depaug = " 0 ".ToString();
                                        obj.depsep = " 0 ".ToString();
                                        obj.depoct = " 0 ".ToString();
                                        obj.depnov = " 0 ".ToString();
                                        obj.depdec = " 0 ".ToString();
                                    }
                                }
                                obj.deptot = Convert.ToString(string.IsNullOrEmpty(row["TOTAL_DEPR"].ToString()) ? " 0 " : row["TOTAL_DEPR"]);
                                obj.wdv = Convert.ToString(string.IsNullOrEmpty(row["WDV"].ToString()) ? " 0 " : row["WDV"]);
                                obj.assettrffrm = Convert.ToString(string.IsNullOrEmpty(row["TRANSFER_FROM"].ToString()) ? " 0 " : row["TRANSFER_FROM"]);
                                obj.assetdettrfval = Math.Round(Convert.ToDecimal(row["TRANSFER_VALUE"].ToString()));
                                //  obj.assetdetstaus = Convert.ToString(string.IsNullOrEmpty(row["ASSET_STATUS"].ToString()) ? " 0 " : row["ASSET_STATUS"]);
                                obj.assetdettrfdat = Convert.ToString(string.IsNullOrEmpty(row["TRANSFER_DATE"].ToString()) ? " 0 " : row["TRANSFER_DATE"]);
                                obj.assetdetsaledat = Convert.ToString(string.IsNullOrEmpty(row["SALE_DATE"].ToString()) ? " 0 " : row["SALE_DATE"]);
                                // obj.department = Convert.ToString(string.IsNullOrEmpty(row["DEPARTMENT"].ToString()) ? " 0 " : row["DEPARTMENT"]);//row["DEPARTMENT"].ToString();
                                //  obj.assetdetspoke = Convert.ToString(string.IsNullOrEmpty(row["SPOKE"].ToString()) ? " 0 " : row["SPOKE"]);
                                obj.bscc = Convert.ToString(string.IsNullOrEmpty(row["ASSET_BSCC"].ToString()) ? " 0 " : row["ASSET_BSCC"]); //row["ASSET_BSCC"].ToString();
                                //  obj.upbscc = Convert.ToString(string.IsNullOrEmpty(row["UPLOAD_BSCC"].ToString()) ? " 0 " : row["UPLOAD_BSCC"]);//row["UPLOAD_BSCC"].ToString();
                                obj.ponumb = Convert.ToString(string.IsNullOrEmpty(row["PO_NUMBER"].ToString()) ? " 0 " : row["PO_NUMBER"]);// row["PO_NUMBER"].ToString();
                                obj.cbfnumb = Convert.ToString(string.IsNullOrEmpty(row["CBF_NUMBER"].ToString()) ? " 0 " : row["CBF_NUMBER"]);//row["CBF_NUMBER"].ToString();
                                obj.vendornam = Convert.ToString(string.IsNullOrEmpty(row["VENDOR_NAME"].ToString()) ? " 0 " : row["VENDOR_NAME"]); //row["VENDOR_NAME"].ToString();
                                obj.Naration = Convert.ToString(string.IsNullOrEmpty(row["NARRATION"].ToString()) ? " 0 " : row["NARRATION"]);
                                obj.CLASSIFICATION = Convert.ToString(string.IsNullOrEmpty(row["CLASSIFICATION"].ToString()) ? " 0 " : row["CLASSIFICATION"]);
                                obj.OFFICE = Convert.ToString(string.IsNullOrEmpty(row["OFFICE"].ToString()) ? " 0 " : row["OFFICE"]);
                                obj.Branch_Status = Convert.ToString(string.IsNullOrEmpty(row["Branch_Status"].ToString()) ? " 0 " : row["Branch_Status"]);
                                obj.branch_branchtype_name = Convert.ToString(string.IsNullOrEmpty(row["branch_branchtype_name"].ToString()) ? " 0 " : row["branch_branchtype_name"]);
                                obj.VERIFIABLE_STATUS = Convert.ToString(string.IsNullOrEmpty(row["VERIFIABLE_STATUS"].ToString()) ? " 0 " : row["VERIFIABLE_STATUS"]);
                                obj.ASSET_STATUS = Convert.ToString(string.IsNullOrEmpty(row["ASSET_STATUS"].ToString()) ? " 0 " : row["ASSET_STATUS"]);
                                obj.SALE_VALUE = Convert.ToString(string.IsNullOrEmpty(row["SALE_VALUE"].ToString()) ? " 0 " : row["SALE_VALUE"]);
                                obj.VAT_VALUE = Convert.ToString(string.IsNullOrEmpty(row["VAT_VALUE"].ToString()) ? " 0 " : row["VAT_VALUE"]);
                                obj.TOTAL_SALE_VALUE = Convert.ToString(string.IsNullOrEmpty(row["TOTAL_SALE_VALUE"].ToString()) ? " 0 " : row["TOTAL_SALE_VALUE"]);
                                obj.NET_LOSS = Convert.ToString(string.IsNullOrEmpty(row["NET_LOSS"].ToString()) ? " 0 " : row["NET_LOSS"]);


                                HttpContext.Current.Session["obj1.Year"] = null;
                                HttpContext.Current.Session["obj1.depjan"] = null;
                                HttpContext.Current.Session["obj1.depfeb"] = null;
                                HttpContext.Current.Session["obj1.depmar"] = null;
                                HttpContext.Current.Session["obj1.depapr"] = null;
                                HttpContext.Current.Session["obj1.depmay"] = null;
                                HttpContext.Current.Session["obj1.depjun"] = null;
                                HttpContext.Current.Session["obj1.depjul"] = null;
                                HttpContext.Current.Session["obj1.depaug"] = null;
                                HttpContext.Current.Session["obj1.depsep"] = null;
                                HttpContext.Current.Session["obj1.depoct"] = null;
                                HttpContext.Current.Session["obj1.depnov"] = null;
                                HttpContext.Current.Session["obj1.depdec"] = null;
                                years = "";
                                //objmod.Add(obj);
                                string[,] fareportinsert = new string[,] 
                                        {  
                                        {"SNo" , i.ToString()}   ,        
                                        {"GROUP_ID",obj.assetdetgroupid},
                                        {"ASSET_ID", obj.assetdetDetid },
                                        {"QUANTITY", obj.assetdetqty},
                                        {"ASSET_CODE", obj.assetdetCode},
                                        {"DESCRIPTION",obj.assetdetDescription},
                                        {"GLCODE", obj.assetdetglcode},
                                        {"DEPR_TYPE", obj.deptype},
                                        {"DEPR_RATE", obj.deprate },
                                        {"DEPR_GL",obj.depgl },
                                        {"DEPR_PV_GL",obj.deppvgl },
                                        {"BRANCH_CODE",obj.assetdetLocationcode},
                                        {"BRANCH_LAUNCH_DATE",obj.branchlaunchdat },
                                        {"LEASE_STAR_DATE",obj.assetleasedat},
                                        {"LEASE_END_DATE",obj.assetleaseenddat},
                                        {"CAPITALISATION_DATE",obj.assetcapdate },
                                        {"ASSET_VALUE",obj.assetdetAssetvalue.ToString() },
                                        {"YEARS", obj.Year},
                                        {"APR",obj.depapr},
                                        {"MAY",obj.depmay},
                                        {"JUN",obj.depjun},
                                        {"JUL",obj.depjul },
                                        {"AUG", obj.depaug},
                                        {"SEP", obj.depsep},
                                        {"OCT",obj.depoct },
                                        {"NOV",obj.depnov },
                                        {"DEC",obj.depdec },
                                        {"JAN",obj.depjan },
                                        {"FEB",obj.depfeb },
                                        {"MAR",obj.depmar},
                                        {"TOTAL_DEPR",obj.deptot},
                                        {"WDV",obj.wdv},
                                        {"TRANSFER_FROM",obj.assettrffrm },
                                        {"TRANSFER_VALUE", obj.assetdettrfval.ToString() },
                                      //  {"ASSET_STATUS", obj.assetdetstaus},
                                        {"TRANSFER_DATE", obj.assetdettrfdat},
                                        {"SALE_DATE", obj.assetdetsaledat },
                                       // {"DEPARTMENT",obj.department},
                                       // {"SPOKE", obj.assetdetspoke},
                                        {"ASSET_BSCC",  obj.bscc},
                                        //{"UPLOAD_BSCC", obj.upbscc },
                                        {"PO_NUMBER", obj.ponumb },
                                        {"CBF_NUMBER", obj.cbfnumb},
                                        {"VENDOR_NAME", obj.vendornam},
                                        {"NARRATION", obj.Naration},
                                        {"CLASSIFICATION", obj.CLASSIFICATION},
                                        {"OFFICE", obj.OFFICE},
                                        {"Branch_Status", obj.Branch_Status},
                                        {"branch_branchtype_name", obj.branch_branchtype_name},
                                        {"VERIFIABLE_STATUS", obj.VERIFIABLE_STATUS},
                                        {"ASSET_STATUS", obj.ASSET_STATUS},
                                        {"SALE_VALUE", obj.SALE_VALUE},
                                        {"VAT_VALUE", obj.VAT_VALUE},
                                        {"TOTAL_SALE_VALUE", obj.TOTAL_SALE_VALUE},
                                        {"NET_LOSS", obj.NET_LOSS} ,
                                        {"ACCUMULATED_DEP", obj.ACCUMULATED_DEP},
                                          {"CUMMULATIVE_DEP", obj.CUMMULATIVE_DEP}
                                        };
                                string insertcmn = objcmnIUD.InsertCommon(fareportinsert, "fa_tmp_fareport");

                            }
                        }
                        else
                        {


                            obj.assetdetDetid = Convert.ToString(string.IsNullOrEmpty(row["ASSET_ID"].ToString()) ? " 0 " : row["ASSET_ID"]);

                            cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@action", "accelrateDepgrp");
                            cmd.Parameters.AddWithValue("@FromDate", f);
                            cmd.Parameters.AddWithValue("@assetdetgid", obj.assetdetDetid);
                            obj.ACCUMULATED_DEP = Convert.ToString(cmd.ExecuteScalar());

                            cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@action", "cumDepgrp");

                            cmd.Parameters.AddWithValue("@FromDate", curr0);
                            cmd.Parameters.AddWithValue("@ToDate", curr2);
                            cmd.Parameters.AddWithValue("@assetdetgid", obj.assetdetDetid);
                            obj.CUMMULATIVE_DEP = Convert.ToString(cmd.ExecuteScalar());

                            obj.assetdetqty = "1";

                            for (int yearCount = 0; yearCount <= a; yearCount++)
                            {
                                set = (f.AddYears(yearCount)).Year;
                                cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@action", "FAReportdep");
                                cmd.Parameters.AddWithValue("@assetdetgrpid", obj.assetdetgroupid);
                                cmd.Parameters.AddWithValue("@FromDate", f.Year.ToString());
                                cmd.Parameters.AddWithValue("@ToDate", t.Year.ToString());
                                cmd.Parameters.AddWithValue("@Year", set);
                                DataTable dt1 = new DataTable();
                                da = new SqlDataAdapter(cmd);
                                da.Fill(dt1);
                                years = obj.Year;
                                if (dt1.Rows.Count > 0)
                                {
                                    foreach (DataRow row1 in dt1.Rows)
                                    {
                                        obj1 = new AssetReportModel();
                                        obj1.inwarddate = " 0 ";

                                        if (years == "")
                                        {
                                            obj.Year = Convert.ToString(f.Year);
                                            obj.depjan = Convert.ToString(string.IsNullOrEmpty(row1[set + "-01-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-01-01 00:00:00.000"]); //row1["Jan"].ToString();
                                            obj.depfeb = Convert.ToString(string.IsNullOrEmpty(row1[set + "-02-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-02-01 00:00:00.000"]); //row1[""+set+"-02-01 00:00:00.000"].ToString();
                                            obj.depmar = Convert.ToString(string.IsNullOrEmpty(row1[set + "-03-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-03-01 00:00:00.000"]); //row1[""+set+"-02-01 00:00:00.000"].ToString();
                                            obj.depapr = Convert.ToString(string.IsNullOrEmpty(row1[set + "-04-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-04-01 00:00:00.000"]); //row1[""+set+"-04-01 00:00:00.000"].ToString();
                                            obj.depmay = Convert.ToString(string.IsNullOrEmpty(row1[set + "-05-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-05-01 00:00:00.000"]); //row1[""+set+"-05-01 00:00:00.000"].ToString();
                                            obj.depjun = Convert.ToString(string.IsNullOrEmpty(row1[set + "-06-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-06-01 00:00:00.000"]); //row1[""+set+"-06-01 00:00:00.000"].ToString();
                                            obj.depjul = Convert.ToString(string.IsNullOrEmpty(row1[set + "-07-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-07-01 00:00:00.000"]); //row1[""+set+"-07-01 00:00:00.000"].ToString();
                                            obj.depaug = Convert.ToString(string.IsNullOrEmpty(row1[set + "-08-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-08-01 00:00:00.000"]); //row1[""+set+"-08-01 00:00:00.000"].ToString();
                                            obj.depsep = Convert.ToString(string.IsNullOrEmpty(row1[set + "-09-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-09-01 00:00:00.000"]); //row1[""+set+"-09-01 00:00:00.000"].ToString();
                                            obj.depoct = Convert.ToString(string.IsNullOrEmpty(row1[set + "-10-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-10-01 00:00:00.000"]); //row1[""+set+"-10-01 00:00:00.000"].ToString();
                                            obj.depnov = Convert.ToString(string.IsNullOrEmpty(row1[set + "-11-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-11-01 00:00:00.000"]); //row1[""+set+"-11-01 00:00:00.000"].ToString();
                                            obj.depdec = Convert.ToString(string.IsNullOrEmpty(row1[set + "-12-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-12-01 00:00:00.000"]); //row1[""+set+"-12-01 00:00:00.000"].ToString();
                                        }
                                        else
                                        {
                                            obj.Year = years + ", " + set;
                                            obj.depjan = Convert.ToString(string.IsNullOrEmpty(row1[set + "-01-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-01-01 00:00:00.000"]); //row1["Jan"].ToString();
                                            obj.depfeb = Convert.ToString(string.IsNullOrEmpty(row1[set + "-02-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-02-01 00:00:00.000"]); //row1[""+set+"-02-01 00:00:00.000"].ToString();
                                            obj.depmar = Convert.ToString(string.IsNullOrEmpty(row1[set + "-03-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-03-01 00:00:00.000"]); //row1[""+set+"-02-01 00:00:00.000"].ToString();
                                            obj.depapr = Convert.ToString(string.IsNullOrEmpty(row1[set + "-04-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-04-01 00:00:00.000"]); //row1[""+set+"-04-01 00:00:00.000"].ToString();
                                            obj.depmay = Convert.ToString(string.IsNullOrEmpty(row1[set + "-05-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-05-01 00:00:00.000"]); //row1[""+set+"-05-01 00:00:00.000"].ToString();
                                            obj.depjun = Convert.ToString(string.IsNullOrEmpty(row1[set + "-06-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-06-01 00:00:00.000"]); //row1[""+set+"-06-01 00:00:00.000"].ToString();
                                            obj.depjul = Convert.ToString(string.IsNullOrEmpty(row1[set + "-07-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-07-01 00:00:00.000"]); //row1[""+set+"-07-01 00:00:00.000"].ToString();
                                            obj.depaug = Convert.ToString(string.IsNullOrEmpty(row1[set + "-08-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-08-01 00:00:00.000"]); //row1[""+set+"-08-01 00:00:00.000"].ToString();
                                            obj.depsep = Convert.ToString(string.IsNullOrEmpty(row1[set + "-09-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-09-01 00:00:00.000"]); //row1[""+set+"-09-01 00:00:00.000"].ToString();
                                            obj.depoct = Convert.ToString(string.IsNullOrEmpty(row1[set + "-10-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-10-01 00:00:00.000"]); //row1[""+set+"-10-01 00:00:00.000"].ToString();
                                            obj.depnov = Convert.ToString(string.IsNullOrEmpty(row1[set + "-11-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-11-01 00:00:00.000"]); //row1[""+set+"-11-01 00:00:00.000"].ToString();
                                            obj.depdec = Convert.ToString(string.IsNullOrEmpty(row1[set + "-12-01 00:00:00.000"].ToString()) ? " 0 " : row1[set + "-12-01 00:00:00.000"]); //row1[""+set+"-12-01 00:00:00.000"].ToString();
                                        }
                                        HttpContext.Current.Session["obj1.Year"] = obj.Year;
                                        HttpContext.Current.Session["obj1.depjan"] = obj.depjan;
                                        HttpContext.Current.Session["obj1.depfeb"] = obj.depfeb;
                                        HttpContext.Current.Session["obj1.depmar"] = obj.depmar;
                                        HttpContext.Current.Session["obj1.depapr"] = obj.depapr;
                                        HttpContext.Current.Session["obj1.depmay"] = obj.depmay;
                                        HttpContext.Current.Session["obj1.depjun"] = obj.depjun;
                                        HttpContext.Current.Session["obj1.depjul"] = obj.depjul;
                                        HttpContext.Current.Session["obj1.depaug"] = obj.depaug;
                                        HttpContext.Current.Session["obj1.depsep"] = obj.depsep;
                                        HttpContext.Current.Session["obj1.depoct"] = obj.depoct;
                                        HttpContext.Current.Session["obj1.depnov"] = obj.depnov;
                                        HttpContext.Current.Session["obj1.depdec"] = obj.depdec;
                                    }
                                }
                                else
                                {
                                    if (obj.Year != "" && obj.Year != null)
                                        obj.Year = years + ", " + set;
                                    else
                                        obj.Year = set.ToString();
                                    obj.depjan = " 0 ".ToString();
                                    obj.depfeb = " 0 ".ToString();
                                    obj.depmar = " 0 ".ToString();
                                    obj.depapr = " 0 ".ToString();
                                    obj.depmay = " 0 ".ToString();
                                    obj.depjun = " 0 ".ToString();
                                    obj.depjul = " 0 ".ToString();
                                    obj.depaug = " 0 ".ToString();
                                    obj.depsep = " 0 ".ToString();
                                    obj.depoct = " 0 ".ToString();
                                    obj.depnov = " 0 ".ToString();
                                    obj.depdec = " 0 ".ToString();
                                }
                            }
                            obj.deptot = Convert.ToString(string.IsNullOrEmpty(row["TOTAL_DEPR"].ToString()) ? " 0 " : row["TOTAL_DEPR"]);
                            obj.wdv = Convert.ToString(string.IsNullOrEmpty(row["WDV"].ToString()) ? " 0 " : row["WDV"]);
                            obj.assettrffrm = Convert.ToString(string.IsNullOrEmpty(row["TRANSFER_FROM"].ToString()) ? " 0 " : row["TRANSFER_FROM"]);
                            obj.assetdettrfval = Math.Round(Convert.ToDecimal(row["TRANSFER_VALUE"].ToString()));
                            //  obj.assetdetstaus = Convert.ToString(string.IsNullOrEmpty(row["ASSET_STATUS"].ToString()) ? " 0 " : row["ASSET_STATUS"]);
                            obj.assetdettrfdat = Convert.ToString(string.IsNullOrEmpty(row["TRANSFER_DATE"].ToString()) ? " 0 " : row["TRANSFER_DATE"]);
                            obj.assetdetsaledat = Convert.ToString(string.IsNullOrEmpty(row["SALE_DATE"].ToString()) ? " 0 " : row["SALE_DATE"]);
                            // obj.department = Convert.ToString(string.IsNullOrEmpty(row["DEPARTMENT"].ToString()) ? " 0 " : row["DEPARTMENT"]);//row["DEPARTMENT"].ToString();
                            //  obj.assetdetspoke = Convert.ToString(string.IsNullOrEmpty(row["SPOKE"].ToString()) ? " 0 " : row["SPOKE"]);
                            obj.bscc = Convert.ToString(string.IsNullOrEmpty(row["ASSET_BSCC"].ToString()) ? " 0 " : row["ASSET_BSCC"]); //row["ASSET_BSCC"].ToString();
                            //  obj.upbscc = Convert.ToString(string.IsNullOrEmpty(row["UPLOAD_BSCC"].ToString()) ? " 0 " : row["UPLOAD_BSCC"]);//row["UPLOAD_BSCC"].ToString();
                            obj.ponumb = Convert.ToString(string.IsNullOrEmpty(row["PO_NUMBER"].ToString()) ? " 0 " : row["PO_NUMBER"]);// row["PO_NUMBER"].ToString();
                            obj.cbfnumb = Convert.ToString(string.IsNullOrEmpty(row["CBF_NUMBER"].ToString()) ? " 0 " : row["CBF_NUMBER"]);//row["CBF_NUMBER"].ToString();
                            obj.vendornam = Convert.ToString(string.IsNullOrEmpty(row["VENDOR_NAME"].ToString()) ? " 0 " : row["VENDOR_NAME"]); //row["VENDOR_NAME"].ToString();
                            obj.Naration = Convert.ToString(string.IsNullOrEmpty(row["NARRATION"].ToString()) ? " 0 " : row["NARRATION"]);
                            obj.CLASSIFICATION = Convert.ToString(string.IsNullOrEmpty(row["CLASSIFICATION"].ToString()) ? " 0 " : row["CLASSIFICATION"]);
                            obj.OFFICE = Convert.ToString(string.IsNullOrEmpty(row["OFFICE"].ToString()) ? " 0 " : row["OFFICE"]);
                            obj.Branch_Status = Convert.ToString(string.IsNullOrEmpty(row["Branch_Status"].ToString()) ? " 0 " : row["Branch_Status"]);
                            obj.branch_branchtype_name = Convert.ToString(string.IsNullOrEmpty(row["branch_branchtype_name"].ToString()) ? " 0 " : row["branch_branchtype_name"]);
                            obj.VERIFIABLE_STATUS = Convert.ToString(string.IsNullOrEmpty(row["VERIFIABLE_STATUS"].ToString()) ? " 0 " : row["VERIFIABLE_STATUS"]);
                            obj.ASSET_STATUS = Convert.ToString(string.IsNullOrEmpty(row["ASSET_STATUS"].ToString()) ? " 0 " : row["ASSET_STATUS"]);
                            obj.SALE_VALUE = Convert.ToString(string.IsNullOrEmpty(row["SALE_VALUE"].ToString()) ? " 0 " : row["SALE_VALUE"]);
                            obj.VAT_VALUE = Convert.ToString(string.IsNullOrEmpty(row["VAT_VALUE"].ToString()) ? " 0 " : row["VAT_VALUE"]);
                            obj.TOTAL_SALE_VALUE = Convert.ToString(string.IsNullOrEmpty(row["TOTAL_SALE_VALUE"].ToString()) ? " 0 " : row["TOTAL_SALE_VALUE"]);
                            obj.NET_LOSS = Convert.ToString(string.IsNullOrEmpty(row["NET_LOSS"].ToString()) ? " 0 " : row["NET_LOSS"]);

                            //objmod.Add(obj1);
                            HttpContext.Current.Session["obj1.Year"] = null;
                            HttpContext.Current.Session["obj1.depjan"] = null;
                            HttpContext.Current.Session["obj1.depfeb"] = null;
                            HttpContext.Current.Session["obj1.depmar"] = null;
                            HttpContext.Current.Session["obj1.depapr"] = null;
                            HttpContext.Current.Session["obj1.depmay"] = null;
                            HttpContext.Current.Session["obj1.depjun"] = null;
                            HttpContext.Current.Session["obj1.depjul"] = null;
                            HttpContext.Current.Session["obj1.depaug"] = null;
                            HttpContext.Current.Session["obj1.depsep"] = null;
                            HttpContext.Current.Session["obj1.depoct"] = null;
                            HttpContext.Current.Session["obj1.depnov"] = null;
                            HttpContext.Current.Session["obj1.depdec"] = null;
                            years = "";
                            string[,] fareportinsert = new string[,] 
                                        {  
                                        {"SNo" , i.ToString()}   ,        
                                        {"GROUP_ID",obj.assetdetgroupid},
                                        {"ASSET_ID", obj.assetdetDetid },
                                        {"QUANTITY", obj.assetdetqty},
                                        {"ASSET_CODE", obj.assetdetCode},
                                        {"DESCRIPTION",obj.assetdetDescription},
                                        {"GLCODE", obj.assetdetglcode},
                                        {"DEPR_TYPE", obj.deptype},
                                        {"DEPR_RATE", obj.deprate },
                                        {"DEPR_GL",obj.depgl },
                                        {"DEPR_PV_GL",obj.deppvgl },
                                        {"BRANCH_CODE",obj.assetdetLocationcode},
                                        {"BRANCH_LAUNCH_DATE",obj.branchlaunchdat },
                                        {"LEASE_STAR_DATE",obj.assetleasedat},
                                        {"LEASE_END_DATE",obj.assetleaseenddat},
                                        {"CAPITALISATION_DATE",obj.assetcapdate },
                                        {"ASSET_VALUE",obj.assetdetAssetvalue.ToString() },
                                        {"YEARS", obj.Year},
                                        {"APR",obj.depapr},
                                        {"MAY",obj.depmay},
                                        {"JUN",obj.depjun},
                                        {"JUL",obj.depjul },
                                        {"AUG", obj.depaug},
                                        {"SEP", obj.depsep},
                                        {"OCT",obj.depoct },
                                        {"NOV",obj.depnov },
                                        {"DEC",obj.depdec },
                                        {"JAN",obj.depjan },
                                        {"FEB",obj.depfeb },
                                        {"MAR",obj.depmar},
                                        {"TOTAL_DEPR",obj.deptot},
                                        {"WDV",obj.wdv},
                                        {"TRANSFER_FROM",obj.assettrffrm },
                                        {"TRANSFER_VALUE", obj.assetdettrfval.ToString() },
                                    //    {"ASSET_STATUS", obj.assetdetstaus},
                                        {"TRANSFER_DATE", obj.assetdettrfdat},
                                        {"SALE_DATE", obj.assetdetsaledat },
                                       // {"DEPARTMENT",obj.department},
                                      //  {"SPOKE", obj.assetdetspoke},
                                        {"ASSET_BSCC",  obj.bscc},
                                       // {"UPLOAD_BSCC", obj.upbscc },
                                        {"PO_NUMBER", obj.ponumb },
                                        {"CBF_NUMBER", obj.cbfnumb},
                                        {"VENDOR_NAME", obj.vendornam},
                                        {"NARRATION", obj.Naration},
                                        {"CLASSIFICATION", obj.CLASSIFICATION},
                                        {"OFFICE", obj.OFFICE},
                                        {"Branch_Status", obj.Branch_Status},
                                        {"branch_branchtype_name", obj.branch_branchtype_name},
                                        {"VERIFIABLE_STATUS", obj.VERIFIABLE_STATUS},
                                        {"ASSET_STATUS", obj.ASSET_STATUS},
                                        {"SALE_VALUE", obj.SALE_VALUE},
                                        {"VAT_VALUE", obj.VAT_VALUE},
                                        {"TOTAL_SALE_VALUE", obj.TOTAL_SALE_VALUE},
                                        {"NET_LOSS", obj.NET_LOSS},
                                          //{"ACCUMULATED_DEP", obj.ACCUMULATED_DEP},
                                          // {"CUMMULATIVE_DEP", obj.CUMMULATIVE_DEP}
                                        };
                            string insertcmn = objcmnIUD.InsertCommon(fareportinsert, "fa_tmp_fareport");

                        }
                    }
                    else
                    {
                        obj.Year = "-".ToString();
                        obj.depjan = " 0 ".ToString();
                        obj.depfeb = " 0 ".ToString();
                        obj.depmar = " 0 ".ToString();
                        obj.depapr = " 0 ".ToString();
                        obj.depmay = " 0 ".ToString();
                        obj.depjun = " 0 ".ToString();
                        obj.depjul = " 0 ".ToString();
                        obj.depaug = " 0 ".ToString();
                        obj.depsep = " 0 ".ToString();
                        obj.depoct = " 0 ".ToString();
                        obj.depnov = " 0 ".ToString();
                        obj.depdec = " 0 ".ToString();
                    }

                    i++;
                }
                DataTable fatable = new DataTable();
                if (i != 0)
                {

                    cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "fatable");
                    da = new SqlDataAdapter(cmd);
                    da.Fill(fatable);
                }
                return fatable;
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return dt;

            }
            finally
            {
                con.Close();
            }

        }
        public void deletefatemp()
        {
            GetConnection();
            cmd = new SqlCommand("pr_ifams_rpt_Report", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@action", "fatabledelete");
            cmd.ExecuteNonQuery();
        }

        public IEnumerable<AssetReportModel> getfatable(DataTable fatable)
        {
            List<AssetReportModel> objmod = new List<AssetReportModel>();
            AssetReportModel obj = new AssetReportModel();
            for (int i = 0; i < fatable.Rows.Count; i++)
            {
                obj.assetdetgroupid = fatable.Rows[i]["GROUP_ID"].ToString();
                obj.assetdetDetid = fatable.Rows[i]["ASSET_ID"].ToString();
                obj.assetdetqty = fatable.Rows[i]["QUANTITY"].ToString();
                obj.assetdetCode = fatable.Rows[i]["ASSET_CODE"].ToString();
                obj.assetdetDescription = fatable.Rows[i]["DESCRIPTION"].ToString();
                obj.assetdetglcode = fatable.Rows[i]["GLCODE"].ToString();
                obj.deptype = fatable.Rows[i]["DEPR_TYPE"].ToString();
                obj.deprate = fatable.Rows[i]["DEPR_RATE"].ToString();
                obj.depgl = fatable.Rows[i]["DEPR_GL"].ToString();
                obj.deppvgl = fatable.Rows[i]["DEPR_PV_GL"].ToString();
                obj.assetdetLocationcode = fatable.Rows[i]["BRANCH_CODE"].ToString();
                obj.branchlaunchdat = fatable.Rows[i]["BRANCH_LAUNCH_DATE"].ToString();
                obj.assetleasedat = fatable.Rows[i]["LEASE_STAR_DATE"].ToString();
                obj.assetleaseenddat = fatable.Rows[i]["LEASE_END_DATE"].ToString();
                obj.assetcapdate = fatable.Rows[i]["CAPITALISATION_DATE"].ToString();
                obj.assetdetAssetvalue = Convert.ToDecimal(fatable.Rows[i]["ASSET_VALUE"].ToString());
                obj.Year = fatable.Rows[i]["YEARS"].ToString();
                obj.depapr = fatable.Rows[i]["APR"].ToString();
                obj.depmay = fatable.Rows[i]["MAY"].ToString();
                obj.depjun = fatable.Rows[i]["JUN"].ToString();
                obj.depjul = fatable.Rows[i]["JUL"].ToString();
                obj.depaug = fatable.Rows[i]["AUG"].ToString();
                obj.depsep = fatable.Rows[i]["SEP"].ToString();
                obj.depoct = fatable.Rows[i]["OCT"].ToString();
                obj.depnov = fatable.Rows[i]["NOV"].ToString();
                obj.depdec = fatable.Rows[i]["DEC"].ToString();
                obj.depjan = fatable.Rows[i]["JAN"].ToString();
                obj.depfeb = fatable.Rows[i]["FEB"].ToString();
                obj.depmar = fatable.Rows[i]["MAR"].ToString();
                obj.deptot = fatable.Rows[i]["TOTAL_DEPR"].ToString();
                obj.wdv = fatable.Rows[i]["WDV"].ToString();
                obj.assettrffrm = fatable.Rows[i]["TRANSFER_FROM"].ToString();
                obj.assetdettrfval = Convert.ToDecimal(fatable.Rows[i]["TRANSFER_VALUE"].ToString());
                obj.assetdetstaus = fatable.Rows[i]["ASSET_STATUS"].ToString();
                obj.assetdettrfdat = fatable.Rows[i]["TRANSFER_DATE"].ToString();
                obj.assetdetsaledat = fatable.Rows[i]["SALE_DATE"].ToString();
                obj.department = fatable.Rows[i]["DEPARTMENT"].ToString();
                obj.assetdetspoke = fatable.Rows[i]["SPOKE"].ToString();
                obj.bscc = fatable.Rows[i]["ASSET_BSCC"].ToString();
                obj.upbscc = fatable.Rows[i]["UPLOAD_BSCC"].ToString();
                obj.ponumb = fatable.Rows[i]["PO_NUMBER"].ToString();
                obj.cbfnumb = fatable.Rows[i]["CBF_NUMBER"].ToString();
                obj.vendornam = fatable.Rows[i]["VENDOR_NAME"].ToString();
                obj.Naration = fatable.Rows[i]["NARRATION"].ToString();
                objmod.Add(obj);
            }

            return objmod;

        }

        public IEnumerable<AssetReportModel> GetFAreportDetails(string FINYEAR)
        {

            List<AssetReportModel> objmod = new List<AssetReportModel>();
            AssetReportModel obj = new AssetReportModel();
            AssetReportModel obj1 = new AssetReportModel();
            try
            {
                GetConnection();
                string[] set = FINYEAR.Split('-');
                string years0 = set[0];
                string years1 = set[1];
                cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "FAReport");
                cmd.Parameters.AddWithValue("@FromDate", years0);
                cmd.Parameters.AddWithValue("@ToDate", years1);
                cmd.CommandTimeout = 1000;
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                int samllcount = dt.Rows.Count;


                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetReportModel();
                    obj.inwarddate = " 0 ";
                    obj.assetdetgid = Convert.ToInt32(Convert.ToString(string.IsNullOrEmpty(row["assetgid"].ToString()) ? "0" : row["assetgid"]));
                    obj.assetdetgroupid = Convert.ToString(string.IsNullOrEmpty(row["GROUP_ID"].ToString()) ? " 0 " : row["GROUP_ID"]);
                    obj.assetdetCode = Convert.ToString(string.IsNullOrEmpty(row["ASSET_CODE"].ToString()) ? " 0 " : row["ASSET_CODE"]);
                    obj.assetdetDescription = Convert.ToString(string.IsNullOrEmpty(row["DESCRIPTION"].ToString()) ? " 0 " : row["DESCRIPTION"]);
                    obj.assetdetglcode = Convert.ToString(string.IsNullOrEmpty(row["GLCODE"].ToString()) ? " 0 " : row["GLCODE"]);
                    obj.deptype = Convert.ToString(string.IsNullOrEmpty(row["DEPR_TYPE"].ToString()) ? " 0 " : row["DEPR_TYPE"]);
                    obj.deprate = Convert.ToString(string.IsNullOrEmpty(row["DEPR_RATE"].ToString()) ? " 0 " : row["DEPR_RATE"]);
                    obj.depgl = Convert.ToString(string.IsNullOrEmpty(row["DEPR_GL"].ToString()) ? " 0 " : row["DEPR_GL"]);
                    obj.deppvgl = Convert.ToString(string.IsNullOrEmpty(row["DEPR_PV_GL"].ToString()) ? " 0 " : row["DEPR_PV_GL"]);
                    obj.assetdetLocationcode = Convert.ToString(string.IsNullOrEmpty(row["BRANCH_CODE"].ToString()) ? " 0 " : row["BRANCH_CODE"]);
                    obj.branchlaunchdat = Convert.ToString(string.IsNullOrEmpty(row["BRANCH_LAUNCH_DATE"].ToString()) ? " 0 " : row["BRANCH_LAUNCH_DATE"]);
                    obj.assetleasedat = Convert.ToString(string.IsNullOrEmpty(row["LEASE_START_DATE"].ToString()) ? " 0 " : row["LEASE_START_DATE"]);
                    obj.assetleaseenddat = Convert.ToString(string.IsNullOrEmpty(row["LEASE_END_DATE"].ToString()) ? " 0 " : row["LEASE_END_DATE"]);
                    obj.assetcapdate = Convert.ToString(string.IsNullOrEmpty(row["CAPITALISATION_DATE"].ToString()) ? " 0 " : row["CAPITALISATION_DATE"]);
                    obj.assetdetAssetvalue = Convert.ToDecimal(row["ASSET_VALUE"].ToString());

                    cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "getgrpcount");
                    cmd.Parameters.AddWithValue("@grpgid", obj.assetdetgroupid);
                    DataTable dtgetgrpcount = new DataTable();
                    cmd.CommandTimeout = 1000;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dtgetgrpcount);
                    int acount = Convert.ToInt32(dtgetgrpcount.Rows[0][0].ToString());
                    int bcount = Convert.ToInt32(dtgetgrpcount.Rows[1][0].ToString());
                    if (acount == bcount)
                    {
                        if ((duplicateasset.Contains(obj.assetdetgroupid) == false) || obj.assetdetgid == 0)
                        {
                            obj.assetdetDetid = "-";
                            obj.assetdetqty = Convert.ToString(string.IsNullOrEmpty(row["Count_of"].ToString()) ? " 0 " : row["Count_of"]);
                            duplicateasset += duplicateasset == "" ? obj.assetdetgroupid : ("," + obj.assetdetgroupid);

                            cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@action", "groupasset");
                            cmd.Parameters.AddWithValue("@Grpid", obj.assetdetgroupid);
                            cmd.Parameters.AddWithValue("@FromDate", years0);
                            cmd.Parameters.AddWithValue("@ToDate", years1);
                            cmd.CommandTimeout = 1000;
                            DataTable grpDT = new DataTable();
                            da = new SqlDataAdapter(cmd);
                            da.Fill(grpDT);
                            if (grpDT.Rows.Count > 0)
                            {
                                obj.depjan = Convert.ToString(string.IsNullOrEmpty(grpDT.Rows[0]["JAN_GRP"].ToString()) ? " 0 " : grpDT.Rows[0]["JAN_GRP"]);
                                obj.depfeb = Convert.ToString(string.IsNullOrEmpty(grpDT.Rows[0]["FEB_GRP"].ToString()) ? " 0 " : grpDT.Rows[0]["FEB_GRP"]);
                                obj.depmar = Convert.ToString(string.IsNullOrEmpty(grpDT.Rows[0]["MAR_GRP"].ToString()) ? " 0 " : grpDT.Rows[0]["MAR_GRP"]);
                                obj.depapr = Convert.ToString(string.IsNullOrEmpty(grpDT.Rows[0]["APR_GRP"].ToString()) ? " 0 " : grpDT.Rows[0]["APR_GRP"]);
                                obj.depmay = Convert.ToString(string.IsNullOrEmpty(grpDT.Rows[0]["MAY_GRP"].ToString()) ? " 0 " : grpDT.Rows[0]["MAY_GRP"]);
                                obj.depjun = Convert.ToString(string.IsNullOrEmpty(grpDT.Rows[0]["JUN_GRP"].ToString()) ? " 0 " : grpDT.Rows[0]["JUN_GRP"]);
                                obj.depjul = Convert.ToString(string.IsNullOrEmpty(grpDT.Rows[0]["JUL_GRP"].ToString()) ? " 0 " : grpDT.Rows[0]["JUL_GRP"]);
                                obj.depaug = Convert.ToString(string.IsNullOrEmpty(grpDT.Rows[0]["AUG_GRP"].ToString()) ? " 0 " : grpDT.Rows[0]["AUG_GRP"]);
                                obj.depsep = Convert.ToString(string.IsNullOrEmpty(grpDT.Rows[0]["SEP_GRP"].ToString()) ? " 0 " : grpDT.Rows[0]["SEP_GRP"]);
                                obj.depoct = Convert.ToString(string.IsNullOrEmpty(grpDT.Rows[0]["OCT_GRP"].ToString()) ? " 0 " : grpDT.Rows[0]["OCT_GRP"]);
                                obj.depnov = Convert.ToString(string.IsNullOrEmpty(grpDT.Rows[0]["NOV_GRP"].ToString()) ? " 0 " : grpDT.Rows[0]["NOV_GRP"]);
                                obj.depdec = Convert.ToString(string.IsNullOrEmpty(grpDT.Rows[0]["DEC_GRP"].ToString()) ? " 0 " : grpDT.Rows[0]["DEC_GRP"]);
                            }
                            obj.deptot = Convert.ToString(string.IsNullOrEmpty(row["TOTAL_DEPR"].ToString()) ? " 0 " : row["TOTAL_DEPR"]);
                            obj.wdv = Convert.ToString(string.IsNullOrEmpty(row["WDV"].ToString()) ? " 0 " : row["WDV"]);
                            obj.assettrffrm = Convert.ToString(string.IsNullOrEmpty(row["TRANSFER_FROM"].ToString()) ? " 0 " : row["TRANSFER_FROM"]);
                            obj.assetdettrfval = Math.Round(Convert.ToDecimal(row["TRANSFER_VALUE"].ToString()));
                            obj.assetdetstaus = Convert.ToString(string.IsNullOrEmpty(row["ASSET_STATUS"].ToString()) ? " 0 " : row["ASSET_STATUS"]);
                            obj.assetdettrfdat = Convert.ToString(string.IsNullOrEmpty(row["TRANSFER_DATE"].ToString()) ? " 0 " : row["TRANSFER_DATE"]);
                            obj.assetdetsaledat = Convert.ToString(string.IsNullOrEmpty(row["SALE_DATE"].ToString()) ? " 0 " : row["SALE_DATE"]);
                            obj.department = Convert.ToString(string.IsNullOrEmpty(row["DEPARTMENT"].ToString()) ? " 0 " : row["DEPARTMENT"]);
                            obj.assetdetspoke = Convert.ToString(string.IsNullOrEmpty(row["SPOKE"].ToString()) ? " 0 " : row["SPOKE"]);
                            obj.bscc = Convert.ToString(string.IsNullOrEmpty(row["ASSET_BSCC"].ToString()) ? " 0 " : row["ASSET_BSCC"]);
                            obj.upbscc = Convert.ToString(string.IsNullOrEmpty(row["UPLOAD_BSCC"].ToString()) ? " 0 " : row["UPLOAD_BSCC"]);
                            obj.ponumb = Convert.ToString(string.IsNullOrEmpty(row["PO_NUMBER"].ToString()) ? " 0 " : row["PO_NUMBER"]);
                            obj.cbfnumb = Convert.ToString(string.IsNullOrEmpty(row["CBF_NUMBER"].ToString()) ? " 0 " : row["CBF_NUMBER"]);
                            obj.vendornam = Convert.ToString(string.IsNullOrEmpty(row["VENDOR_NAME"].ToString()) ? " 0 " : row["VENDOR_NAME"]);
                            obj.Naration = Convert.ToString(string.IsNullOrEmpty(row["NARRATION"].ToString()) ? " 0 " : row["NARRATION"]);
                            objmod.Add(obj);
                        }
                    }
                    else
                    {
                        obj.assetdetDetid = Convert.ToString(string.IsNullOrEmpty(row["ASSET_ID"].ToString()) ? " 0 " : row["ASSET_ID"]);
                        obj.assetdetqty = "1";
                        cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@action", "singleasset");
                        cmd.Parameters.AddWithValue("@assetdetgid", obj.assetdetgid);
                        cmd.Parameters.AddWithValue("@FromDate", years0);
                        cmd.Parameters.AddWithValue("@ToDate", years1);
                        cmd.CommandTimeout = 1000;
                        DataTable grpDT = new DataTable();
                        da = new SqlDataAdapter(cmd);
                        da.Fill(grpDT);
                        if (grpDT.Rows.Count > 0)
                        {
                            obj.depjan = Convert.ToString(string.IsNullOrEmpty(grpDT.Rows[0]["JAN"].ToString()) ? " 0 " : grpDT.Rows[0]["JAN"]);
                            obj.depfeb = Convert.ToString(string.IsNullOrEmpty(grpDT.Rows[0]["FEB"].ToString()) ? " 0 " : grpDT.Rows[0]["FEB"]);
                            obj.depmar = Convert.ToString(string.IsNullOrEmpty(grpDT.Rows[0]["MAR"].ToString()) ? " 0 " : grpDT.Rows[0]["MAR"]);
                            obj.depapr = Convert.ToString(string.IsNullOrEmpty(grpDT.Rows[0]["APR"].ToString()) ? " 0 " : grpDT.Rows[0]["APR"]);
                            obj.depmay = Convert.ToString(string.IsNullOrEmpty(grpDT.Rows[0]["MAY"].ToString()) ? " 0 " : grpDT.Rows[0]["MAY"]);
                            obj.depjun = Convert.ToString(string.IsNullOrEmpty(grpDT.Rows[0]["JUN"].ToString()) ? " 0 " : grpDT.Rows[0]["JUN"]);
                            obj.depjul = Convert.ToString(string.IsNullOrEmpty(grpDT.Rows[0]["JUL"].ToString()) ? " 0 " : grpDT.Rows[0]["JUL"]);
                            obj.depaug = Convert.ToString(string.IsNullOrEmpty(grpDT.Rows[0]["AUG"].ToString()) ? " 0 " : grpDT.Rows[0]["AUG"]);
                            obj.depsep = Convert.ToString(string.IsNullOrEmpty(grpDT.Rows[0]["SEP"].ToString()) ? " 0 " : grpDT.Rows[0]["SEP"]);
                            obj.depoct = Convert.ToString(string.IsNullOrEmpty(grpDT.Rows[0]["OCT"].ToString()) ? " 0 " : grpDT.Rows[0]["OCT"]);
                            obj.depnov = Convert.ToString(string.IsNullOrEmpty(grpDT.Rows[0]["NOV"].ToString()) ? " 0 " : grpDT.Rows[0]["NOV"]);
                            obj.depdec = Convert.ToString(string.IsNullOrEmpty(grpDT.Rows[0]["DEC"].ToString()) ? " 0 " : grpDT.Rows[0]["DEC"]);
                        }

                        obj.deptot = Convert.ToString(string.IsNullOrEmpty(row["TOTAL_DEPR"].ToString()) ? " 0 " : row["TOTAL_DEPR"]);
                        obj.wdv = Convert.ToString(string.IsNullOrEmpty(row["WDV"].ToString()) ? " 0 " : row["WDV"]);
                        obj.assettrffrm = Convert.ToString(string.IsNullOrEmpty(row["TRANSFER_FROM"].ToString()) ? " 0 " : row["TRANSFER_FROM"]);
                        obj.assetdettrfval = Math.Round(Convert.ToDecimal(row["TRANSFER_VALUE"].ToString()));
                        obj.assetdetstaus = Convert.ToString(string.IsNullOrEmpty(row["ASSET_STATUS"].ToString()) ? " 0 " : row["ASSET_STATUS"]);
                        obj.assetdettrfdat = Convert.ToString(string.IsNullOrEmpty(row["TRANSFER_DATE"].ToString()) ? " 0 " : row["TRANSFER_DATE"]);
                        obj.assetdetsaledat = Convert.ToString(string.IsNullOrEmpty(row["SALE_DATE"].ToString()) ? " 0 " : row["SALE_DATE"]);
                        obj.department = Convert.ToString(string.IsNullOrEmpty(row["DEPARTMENT"].ToString()) ? " 0 " : row["DEPARTMENT"]);
                        obj.assetdetspoke = Convert.ToString(string.IsNullOrEmpty(row["SPOKE"].ToString()) ? " 0 " : row["SPOKE"]);
                        obj.bscc = Convert.ToString(string.IsNullOrEmpty(row["ASSET_BSCC"].ToString()) ? " 0 " : row["ASSET_BSCC"]);
                        obj.upbscc = Convert.ToString(string.IsNullOrEmpty(row["UPLOAD_BSCC"].ToString()) ? " 0 " : row["UPLOAD_BSCC"]);
                        obj.ponumb = Convert.ToString(string.IsNullOrEmpty(row["PO_NUMBER"].ToString()) ? " 0 " : row["PO_NUMBER"]);
                        obj.cbfnumb = Convert.ToString(string.IsNullOrEmpty(row["CBF_NUMBER"].ToString()) ? " 0 " : row["CBF_NUMBER"]);
                        obj.vendornam = Convert.ToString(string.IsNullOrEmpty(row["VENDOR_NAME"].ToString()) ? " 0 " : row["VENDOR_NAME"]);
                        obj.Naration = Convert.ToString(string.IsNullOrEmpty(row["NARRATION"].ToString()) ? " 0 " : row["NARRATION"]);
                        objmod.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return (objmod);
        }





        public DateTime converttoDategrid(string date1)
        {
            string lsInputDate = date1;
            DateTime outputDate = Convert.ToDateTime(lsInputDate.ToString());
            try
            {
                string convertdate = string.Empty;
                string format = "dd-mm-yyyy";
                convertdate = outputDate.ToString(format);
                outputDate = Convert.ToDateTime(convertdate);
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return outputDate;
        }

        public string Getstatus(string statusid)
        {
            try
            {
                string result = "";
                GetConnection();
                cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@statusid", statusid);
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
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
            }

        }

        public string Gettakenby(string excelvalname)
        {
            string result = "";
            try
            {
                GetConnection();

                cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@excelname", excelvalname);
                cmd.Parameters.AddWithValue("@action", "takenby");
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result = dt.Rows[0]["excelvalidate_name"].ToString();
                }
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public string Getempid(string empgid)
        {
            string result = "";
            try
            {
                GetConnection();

                cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@empgid", empgid);
                cmd.Parameters.AddWithValue("@action", "empname");
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result = dt.Rows[0]["employee_code"].ToString();
                }
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public string GetGroupid(string grpgid)
        {
            string result = "";
            try
            {
                GetConnection();

                cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@grpgid", grpgid);
                cmd.Parameters.AddWithValue("@action", "grpid");
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    result = dt.Rows[0]["employee_code"].ToString();
                }
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                con.Close();
            }
            return result;
        }


        public IEnumerable<AssetReportModel> FAAutobranch(string term)
        {
            List<AssetReportModel> Model = new List<AssetReportModel>();
            AssetReportModel obj = new AssetReportModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "FAAutobaranch";
                cmd.Parameters.Add("@branch", SqlDbType.VarChar).Value = term;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetReportModel();
                    obj.assetdetLocationcode = row["asset_code"].ToString();
                    Model.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                con.Close();

            }

            return Model;
        }


        public IEnumerable<AssetReportModel> Autoassetid(string term)
        {
            List<AssetReportModel> Model = new List<AssetReportModel>();
            AssetReportModel obj = new AssetReportModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "WOAAutoAssetdetDetid";
                cmd.Parameters.Add("@assetdetid", SqlDbType.VarChar).Value = term;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetReportModel();
                    obj.assetdetDetid = row["assetdetails_assetdet_id"].ToString();
                    Model.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                con.Close();

            }

            return Model;
        }

        public IEnumerable<AssetReportModel> WOAAutoserialno(string term)
        {
            List<AssetReportModel> Model = new List<AssetReportModel>();
            AssetReportModel obj = new AssetReportModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "WOA";
                cmd.Parameters.Add("@serailno", SqlDbType.VarChar).Value = term;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetReportModel();
                    obj.woaWOnumber = row["woaheader_woa_number"].ToString();
                    Model.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                con.Close();

            }

            return Model;
        }


        public IEnumerable<AssetReportModel> TOAAutoassetid(string term)
        {
            List<AssetReportModel> Model = new List<AssetReportModel>();
            AssetReportModel obj = new AssetReportModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "TOAAutoAssetdetDetid";
                cmd.Parameters.Add("@assetdetid", SqlDbType.VarChar).Value = term;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetReportModel();
                    obj.assetdetDetid = row["assetdetails_assetdet_id"].ToString();
                    Model.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                con.Close();

            }

            return Model;
        }

        public IEnumerable<AssetReportModel> TOAAutoserialno(string term)
        {
            List<AssetReportModel> Model = new List<AssetReportModel>();
            AssetReportModel obj = new AssetReportModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "TOA";
                cmd.Parameters.Add("@serailno", SqlDbType.VarChar).Value = term;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetReportModel();
                    obj.toaTrannumber = row["toaheader_toa_number"].ToString();
                    Model.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                con.Close();

            }

            return Model;
        }

        public IEnumerable<AssetReportModel> SOAAutoassetid(string term)
        {
            List<AssetReportModel> Model = new List<AssetReportModel>();
            AssetReportModel obj = new AssetReportModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "SOAAutoAssetdetDetid";
                cmd.Parameters.Add("@assetdetid", SqlDbType.VarChar).Value = term;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetReportModel();
                    obj.assetdetDetid = row["assetdetails_assetdet_id"].ToString();
                    Model.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                con.Close();

            }

            return Model;
        }


        public IEnumerable<AssetReportModel> SOAAutoserialno(string term)
        {
            List<AssetReportModel> Model = new List<AssetReportModel>();
            AssetReportModel obj = new AssetReportModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "SOA";
                cmd.Parameters.Add("@serailno", SqlDbType.VarChar).Value = term;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetReportModel();
                    obj.soaSalenumber = row["soaheader_sale_number"].ToString();
                    Model.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                con.Close();

            }

            return Model;
        }


        public IEnumerable<AssetReportModel> SOAAutocheqno(string term)
        {
            List<AssetReportModel> Model = new List<AssetReportModel>();
            AssetReportModel obj = new AssetReportModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "SOAAutocheqno";
                cmd.Parameters.Add("@cheqno", SqlDbType.VarChar).Value = term;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetReportModel();
                    obj.soapayChqno = row["soapayment_chq_no"].ToString();
                    Model.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                con.Close();

            }

            return Model;
        }



        public IEnumerable<AssetReportModel> SOAAutoinvoiceno(string term)
        {
            List<AssetReportModel> Model = new List<AssetReportModel>();
            AssetReportModel obj = new AssetReportModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "SOAAutoinvoiceno";
                cmd.Parameters.Add("@invno", SqlDbType.VarChar).Value = term;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetReportModel();
                    obj.soainvno = row["soaheader_inv_no"].ToString();
                    Model.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                con.Close();

            }

            return Model;
        }


        public IEnumerable<AssetReportModel> ACCAutoassetid(string term)
        {
            List<AssetReportModel> Model = new List<AssetReportModel>();
            AssetReportModel obj = new AssetReportModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "ACCAutoAssetdetDetid";
                cmd.Parameters.Add("@assetdetid", SqlDbType.VarChar).Value = term;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetReportModel();
                    obj.assetdetDetid = row["assetdetails_assetdet_id"].ToString();
                    Model.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                con.Close();

            }

            return Model;
        }


        public IEnumerable<AssetReportModel> ACCAutoOassetcode(string term)
        {
            List<AssetReportModel> Model = new List<AssetReportModel>();
            AssetReportModel obj = new AssetReportModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "ACCAutoAssetcodeold";
                cmd.Parameters.Add("@assetcode", SqlDbType.VarChar).Value = term;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetReportModel();
                    obj.accoldassetcode = row["assetcodechange_old_assetid"].ToString();
                    Model.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                con.Close();

            }

            return Model;
        }


        public IEnumerable<AssetReportModel> ACCAutoNassetcode(string term)
        {
            List<AssetReportModel> Model = new List<AssetReportModel>();
            AssetReportModel obj = new AssetReportModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "ACCAutoAssetcodenew";
                cmd.Parameters.Add("@assetcode", SqlDbType.VarChar).Value = term;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetReportModel();
                    obj.accnewassetcode = row["asset_code"].ToString();
                    Model.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                con.Close();

            }

            return Model;
        }


        public IEnumerable<AssetReportModel> ACCAutoGrpid(string term)
        {
            List<AssetReportModel> Model = new List<AssetReportModel>();
            AssetReportModel obj = new AssetReportModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "ACCAutoGrpid";
                cmd.Parameters.Add("@Grpid", SqlDbType.VarChar).Value = term;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetReportModel();
                    obj.assetdetgroupid = row["assetdetails_asset_groupid"].ToString();
                    Model.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                con.Close();

            }

            return Model;
        }

        public IEnumerable<AssetReportModel> PHYAutoassetid(string term)
        {
            List<AssetReportModel> Model = new List<AssetReportModel>();
            AssetReportModel obj = new AssetReportModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "PhyAutoassetid";
                cmd.Parameters.Add("@assetdetid", SqlDbType.VarChar).Value = term;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetReportModel();
                    obj.assetdetDetid = row["assetdetails_assetdet_id"].ToString();
                    Model.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                con.Close();

            }

            return Model;
        }

        public IEnumerable<AssetReportModel> Autophyid(string term)
        {
            List<AssetReportModel> Model = new List<AssetReportModel>();
            AssetReportModel obj = new AssetReportModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "AutoPHYid";
                cmd.Parameters.Add("@phyid", SqlDbType.VarChar).Value = term;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetReportModel();
                    obj.goagroupid = row["goaheader_groupid"].ToString();
                    Model.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                con.Close();

            }

            return Model;
        }


        public IEnumerable<AssetReportModel> PHYAutogrpid(string term)
        {
            List<AssetReportModel> Model = new List<AssetReportModel>();
            AssetReportModel obj = new AssetReportModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "PHYAutoGrpid";
                cmd.Parameters.Add("@Grpid", SqlDbType.VarChar).Value = term;
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetReportModel();
                    obj.assetdetgroupid = row["assetdetails_asset_groupid"].ToString();
                    Model.Add(obj);
                }

            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                con.Close();

            }

            return Model;
        }

        public string GetGRPSequence()
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
                lsQry = "select sequenceno_no from fa_mst_tsequenceno where sequenceno_code='GRP'";
                cmd.Connection = con;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = lsQry;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    lsSequence = Convert.ToInt32(dt.Rows[0]["sequenceno_no"]) + 1;
                }
                else
                {
                    lsSequence = 1;
                }
                result = "GRP" + lsSequence.ToString("000000");
                string[,] codes = new string[,]
	                   { 
                             {"sequenceno_no",lsSequence.ToString()},                            
                       };
                string[,] Whrcodes = new string[,]
	                   {                              
                             {"sequenceno_code" , "GRP"}
                       };
                string instComm = objIUD.UpdateCommon(codes, Whrcodes, "fa_mst_tsequenceno");
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public string GroupIDupdate()
        {
            List<AssetReportModel> Model = new List<AssetReportModel>();
            AssetReportModel obj = new AssetReportModel();
            try
            {

                GetConnection();

                string[,] col = new string[,]
	                   { 
                             {"sequenceno_no" , "0".ToString()},                            
                       };
                string[,] Whrcol = new string[,]
	                   {                              
                             {"sequenceno_code" , "GRP"}
                       };
                string instComm = objcmnIUD.UpdateCommon(col, Whrcol, "fa_mst_tsequenceno");

                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "UpGrpid";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                // dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new AssetReportModel();
                    obj.FAGroupid = GetGRPSequence();
                    obj.assetdetDetidarray = row["assetdetails_gid"].ToString().Split(',');
                    for (int i = 0; i < obj.assetdetDetidarray.Length; i++)
                    {
                        string[,] codes = new string[,]
                         {
                             {"assetdetails_asset_groupid", obj.FAGroupid.ToString()}
                         };
                        string[,] whrcol = new string[,]
                        {
                            {"assetdetails_gid", obj.assetdetDetidarray[i].ToString()}
                        };
                        string tblname = "fa_trn_tassetdetails";
                        string updcomm = objcmnIUD.UpdateCommon(codes, whrcol, tblname);


                        string[,] codesdep = new string[,]
                         {
                             {"depreciation_asset_groupid",  obj.FAGroupid.ToString() }
                         };
                        string[,] whrcoldep = new string[,]
                        {
                            {"depreciation_assetdet_gid", obj.assetdetDetidarray[i].ToString()}
                        };
                        string tblnamedep = "fa_trn_tdepreciation";
                        string updcommdep = objcmnIUD.UpdateCommon(codesdep, whrcoldep, tblnamedep);
                    }


                }

            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }

            return "Success";
        }


        public DataSet GetAssetClearingDetl()
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                SqlCommand cmd = new SqlCommand("pr_ifams_rpt_assetclearing", con);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return ds;
        }

        public DataSet GetFASummaryDetail()
        {
            DataSet ds = new DataSet();
            try
            {
                DataTable dt = new DataTable();
                DataModel mod = new DataModel();
                GetConnection();
                string finyear = mod.toGetFincialyear();
                string[] finyr = finyear.Split('-');
                SqlCommand cmd = new SqlCommand("pr_ifams_rpt_fasummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@finfrom", SqlDbType.VarChar).Value = finyr[0] + "-04-01";
                cmd.Parameters.Add("@finto", SqlDbType.VarChar).Value = finyr[1] + "-03-31";
                cmd.CommandTimeout = 0;
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return ds;
        }
        public DataTable getfadepDetailed(string fromdate, string todate, string grpid, string brcode, string glcode)
        {
            DataTable Mfatable = new DataTable();
            GetConnection();
            try
            {
                int openmonth = Convert.ToDateTime(fromdate).Month - 1;
                string openyear = objidm.toGetFincialyear(Convert.ToDateTime(fromdate));
                string[] year = openyear.Split('-');
                int years = Convert.ToInt32(year[0]);
                cmd = new SqlCommand("pr_ifams_rpt_fardetailed", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@opening_till", year[0] + "-MAR-" + "31");
                cmd.Parameters.AddWithValue("@period_from", convertoDate(Convert.ToDateTime(fromdate).ToShortDateString()));
                cmd.Parameters.AddWithValue("@period_to", convertoDate(Convert.ToDateTime(todate).ToShortDateString()));
                da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.Fill(Mfatable);
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return Mfatable;
        }

        public IEnumerable<AssetReportModel> getFATableDetailed(DataTable fatable)
        {
            List<AssetReportModel> objmod = new List<AssetReportModel>();
            AssetReportModel obj = new AssetReportModel();
            for (int i = 0; i < fatable.Rows.Count; i++)
            {
                obj = new AssetReportModel();
                obj.barCode = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["barcodedetials_bar_no"].ToString()) ? "" : fatable.Rows[i]["barcodedetials_bar_no"]);
                obj.assetSerialNo = Convert.ToString(string.IsNullOrEmpty(fatable.Rows[i]["assetdetails_asset_serialno"].ToString()) ? "" : fatable.Rows[i]["assetdetails_asset_serialno"]); //fatable.Rows[i]["assetdetails_assetdet_id"].ToString();
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

            return objmod;

        }

        public DataTable getfadep(string fromdate, string todate, string grpid, string brcode, string glcode)
        {
            DataTable Mfatable = new DataTable();
            GetConnection();
            //DataTable dt = new DataTable();
            //List<AssetReportModel> objmod = new List<AssetReportModel>();
            //AssetReportModel obj = new AssetReportModel();
            try
            {
                //GetConnection();
                //int set = 0;
                //cmd = new SqlCommand("pr_ifams_rpt_Report", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@action", "FAReportMuthu");
                ////cmd.Parameters.AddWithValue("@FromDate", "FAReport");
                ////cmd.Parameters.AddWithValue("@ToDate", "FAReport");

                //da = new SqlDataAdapter(cmd);
                //da.Fill(dt);
                ////int samllcount = dt.Rows.Count;
                ////string years = "";
                ////int i = 1;
                //foreach (DataRow row in dt.Rows)
                //{
                //    obj = new AssetReportModel();
                //    obj.inwarddate = " 0 ";
                //    obj.assetdetgid = Convert.ToInt32(Convert.ToString(string.IsNullOrEmpty(row["assetdetid"].ToString()) ? "0" : row["assetdetid"]));
                //    obj.assetdetgroupid = Convert.ToString(string.IsNullOrEmpty(row["group_id"].ToString()) ? " 0 " : row["group_id"]);
                //    //obj.assetdetCode = Convert.ToString(string.IsNullOrEmpty(row["ASSET_CODE"].ToString()) ? " 0 " : row["ASSET_CODE"]);
                //    obj.assetdetDescription = Convert.ToString(string.IsNullOrEmpty(row["DESCRIPTION"].ToString()) ? " 0 " : row["DESCRIPTION"]);
                //    obj.assetdetglcode = Convert.ToString(string.IsNullOrEmpty(row["GLCODE"].ToString()) ? " 0 " : row["GLCODE"]);
                //    obj.deptype = Convert.ToString(string.IsNullOrEmpty(row["DEPR_TYPE"].ToString()) ? " 0 " : row["DEPR_TYPE"]);
                //    obj.deprate = Convert.ToString(string.IsNullOrEmpty(row["DEPR_RATE"].ToString()) ? " 0 " : row["DEPR_RATE"]);
                //    obj.depgl = Convert.ToString(string.IsNullOrEmpty(row["DEPR_GL"].ToString()) ? " 0 " : row["DEPR_GL"]);
                //    obj.deppvgl = Convert.ToString(string.IsNullOrEmpty(row["DEPR_PV_GL"].ToString()) ? " 0 " : row["DEPR_PV_GL"]);
                //    obj.assetdetLocationcode = Convert.ToString(string.IsNullOrEmpty(row["BRANCH_CODE"].ToString()) ? " 0 " : row["BRANCH_CODE"]);
                //    obj.branchlaunchdat = Convert.ToString(string.IsNullOrEmpty(row["BRANCH_LAUNCH_DATE"].ToString()) ? " 0 " : row["BRANCH_LAUNCH_DATE"]);
                //    obj.assetleasedat = Convert.ToString(string.IsNullOrEmpty(row["LEASE_START_DATE"].ToString()) ? " 0 " : row["LEASE_START_DATE"]);
                //    obj.assetleaseenddat = Convert.ToString(string.IsNullOrEmpty(row["LEASE_END_DATE"].ToString()) ? " 0 " : row["LEASE_END_DATE"]);
                //    obj.assetcapdate = Convert.ToString(string.IsNullOrEmpty(row["CAPITALISATION_DATE"].ToString()) ? " 0 " : row["CAPITALISATION_DATE"]);
                //    obj.assetdetAssetvalue = Convert.ToDecimal(row["ASSET_VALUE"].ToString());
                int i;

                //if (i != 0)
                //{
                int openmonth = Convert.ToDateTime(fromdate).Month - 1;
                //int openyear = Convert.ToDateTime(fromdate).Year;
                string openyear = objidm.toGetFincialyear(Convert.ToDateTime(fromdate));
                string[] year = openyear.Split('-');
                int years = Convert.ToInt32(year[0]);
                cmd = new SqlCommand("pr_fa_far_rpt", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // cmd.Parameters.AddWithValue("@opening_till", convertoDate( DateTime.Today.ToShortDateString()));
                cmd.Parameters.AddWithValue("@opening_till", year[0] + "-MAR-" + "31");
                cmd.Parameters.AddWithValue("@period_from", convertoDate(Convert.ToDateTime(fromdate).ToShortDateString()));
                cmd.Parameters.AddWithValue("@period_to", convertoDate(Convert.ToDateTime(todate).ToShortDateString()));
                //cmd.Parameters.AddWithValue("@groupid", grpid.ToString());
                //cmd.Parameters.AddWithValue("@brcode", brcode.ToString());
                //cmd.Parameters.AddWithValue("@glcode", glcode.ToString());

                da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.Fill(Mfatable);
                //}


            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return Mfatable;
        }




        public IEnumerable<AssetReportModel> getFATableNew(DataTable fatable)
        {
            List<AssetReportModel> objmod = new List<AssetReportModel>();
            AssetReportModel obj = new AssetReportModel();
            for (int i = 0; i < fatable.Rows.Count; i++)
            {
                obj = new AssetReportModel(); //Convert.ToString(string.IsNullOrEmpty(row["group_id"].ToString()) ? " 0 " : row["group_id"]);

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
                //obj.upbscc = fatable.Rows[i]["UPLOAD_BSCC"].ToString();
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
                obj.taxable_amount = Convert.ToDecimal(string.IsNullOrEmpty(fatable.Rows[i]["Taxable_Amount"].ToString()) ? " 0.00 " : fatable.Rows[i]["Taxable_Amount"]);
                obj.cgst_amount = Convert.ToDecimal(string.IsNullOrEmpty(fatable.Rows[i]["soadetail_cgst_amount"].ToString()) ? " 0.00 " : fatable.Rows[i]["soadetail_cgst_amount"]);
                obj.sgst_amount = Convert.ToDecimal(string.IsNullOrEmpty(fatable.Rows[i]["soadetail_sgst_amount"].ToString()) ? " 0.00 " : fatable.Rows[i]["soadetail_sgst_amount"]);
                obj.igst_amount = Convert.ToDecimal(string.IsNullOrEmpty(fatable.Rows[i]["soadetail_igst_amount"].ToString()) ? " 0.00 " : fatable.Rows[i]["soadetail_igst_amount"]);
                obj.cgcess_amount = Convert.ToDecimal(string.IsNullOrEmpty(fatable.Rows[i]["soadetail_cgcess_amount"].ToString()) ? " 0.00 " : fatable.Rows[i]["soadetail_cgcess_amount"]);
                obj.sgcess_amount = Convert.ToDecimal(string.IsNullOrEmpty(fatable.Rows[i]["soadetail_sgcess_amount"].ToString()) ? " 0.00 " : fatable.Rows[i]["soadetail_sgcess_amount"]);
                obj.igcess_amount = Convert.ToDecimal(string.IsNullOrEmpty(fatable.Rows[i]["soadetail_igcess_amount"].ToString()) ? " 0.00 " : fatable.Rows[i]["soadetail_igcess_amount"]);


                objmod.Add(obj);
            }

            return objmod;

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
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
        }

        public DataTable SearchAssetClear(string DateFrom, string DateTo)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                cmd = new SqlCommand("Pr_ifams_rpt_AssetClearing_RPt", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DateFrom", DateFrom);
                cmd.Parameters.AddWithValue("@DateTo", DateTo);
                da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 0;
                da.Fill(dt);
            }
            catch (Exception ex)
            {
                objerrlog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return dt;
        }


    }
}