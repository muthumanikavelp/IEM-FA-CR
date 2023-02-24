using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using IEM.Common;
using System.Collections;
namespace IEM.Areas.FLEXIBUY.Models
{
    public class CbfSumModel : IrepositoryAn
    {

        CmnFunctions objCmnFunctions = new CmnFunctions();
        CommonIUD objCommonIUD = new CommonIUD();
        SqlConnection objConn = new SqlConnection();
        SqlCommand cmdQry = new SqlCommand();
        ErrorLog objErrorLog = new ErrorLog();
        SqlDataAdapter objDataAdapter = new SqlDataAdapter();

        public void getconnection()
        {
            if (objConn.State == ConnectionState.Closed)
            {
                objConn.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                objConn.Open();
            }
        }
        public string GetGroupForUser()
        {
            string result = string.Empty;
            try
            {
                getconnection();
                cmdQry = new SqlCommand("pr_fb_trn_checkUserForGroup", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@actionName", "select");
                cmdQry.Parameters.AddWithValue("@logingid", Convert.ToInt32(objCmnFunctions.GetLoginUserGid().ToString()));
                result = (string)cmdQry.ExecuteScalar();

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return result;
        }
        public string GetGroupForUserCBF()
        { 
            string result = string.Empty;
            try
            {
                getconnection();
                cmdQry = new SqlCommand("pr_fb_trn_checkUserForGroup", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@actionName", "selectforCBF");
                cmdQry.Parameters.AddWithValue("@logingid", Convert.ToInt32(objCmnFunctions.GetLoginUserGid().ToString()));
                result = (string)cmdQry.ExecuteScalar();

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return result;
        }
        public string GetReqGroup()
        {
            string result = string.Empty;
            try
            {
                getconnection();
                cmdQry = new SqlCommand("pr_fb_trn_checkUserForGroup", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@actionName", "selectforgroup");
                cmdQry.Parameters.AddWithValue("@logingid", Convert.ToInt32(objCmnFunctions.GetLoginUserGid().ToString()));
                result = (string)cmdQry.ExecuteScalar();
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
        }
     
        public CbfSumEntity Getcbfaiseredit(string lsnumber)
        {

            // List<int> prdetails =new List<int>();
            string prdetails = string.Empty;
            getconnection();
            CbfSumEntity objsum = new CbfSumEntity();

            try
            {
                CbfRaiseHeader getvalues = new CbfRaiseHeader();
                DataTable dtCbfHeader = new DataTable();
                CbfDetails getdetailsvalue;
                objsum.cbfDetails = new List<CbfDetails>();
                objsum.cbfPrheader = new List<CbfPrHeader>();
                objsum.cbfPrdetarils = new List<CbfPrDetails>();
                CbfPrHeader getprheader;
                CbfPrDetails getprdetails;

                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.BigInt).Value = lsnumber;
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectcbfedit";
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dtCbfHeader);

                if (dtCbfHeader.Rows.Count > 0)
                {
                    getvalues = new CbfRaiseHeader()
                    {
                        cbfNo = dtCbfHeader.Rows[0]["cbfheader_cbfno"].ToString(),
                        cbfDate = dtCbfHeader.Rows[0]["cbfheader_date"].ToString(),
                        cbfEnddate = dtCbfHeader.Rows[0]["cbfheader_enddate"].ToString(),
                        cbfAmt = Convert.ToDecimal(dtCbfHeader.Rows[0]["cbfheader_cbfamt"].ToString()),
                        deviationAmt = Convert.ToDecimal(dtCbfHeader.Rows[0]["cbfheader_Devi_amt"].ToString()),
                        description = dtCbfHeader.Rows[0]["cbfheader_desc"].ToString(),
                        justfication = dtCbfHeader.Rows[0]["cbfheader_remarks"].ToString(),
                        requeestForGid = Convert.ToInt32(dtCbfHeader.Rows[0]["cbfheader_requestfor_gid"].ToString()),
                        branchcodeGid = Convert.ToInt32(dtCbfHeader.Rows[0]["cbfheader_branch_gid"].ToString()),
                        branchType = dtCbfHeader.Rows[0]["cbfheader_isbranchsingle"].ToString(),
                        budgeted = dtCbfHeader.Rows[0]["cbfheader_isbudgeted"].ToString(),
                        cbfMode = dtCbfHeader.Rows[0]["cbfheader_mode"].ToString(),
                        cbfType = dtCbfHeader.Rows[0]["cbfheader_approvaltype"].ToString(),
                        projectOwnerGid = Convert.ToInt32(dtCbfHeader.Rows[0]["cbfheader_projectowner"].ToString()),
                        requestType = dtCbfHeader.Rows[0]["cbfheader_requesttype"].ToString(),
                        BudgetownerGid = Convert.ToInt32(dtCbfHeader.Rows[0]["cbfheader_budgetowner_gid"].ToString())
                    };

                    objsum.cbdRasieobj = getvalues;
                    HttpContext.Current.Session["cbfdeailsdt"] = dtCbfHeader;
                    for (int i = 0; i < dtCbfHeader.Rows.Count; i++)
                    {
                        getdetailsvalue = new CbfDetails();

                        getdetailsvalue.productCode = dtCbfHeader.Rows[i]["prodservice_code"].ToString();
                        getdetailsvalue.cbfDetGid = Convert.ToInt32(dtCbfHeader.Rows[i]["cbfdetails_gid"].ToString());
                        getdetailsvalue.productName = dtCbfHeader.Rows[i]["prodservice_name"].ToString();
                        getdetailsvalue.description = dtCbfHeader.Rows[i]["prodservice_description"].ToString();
                        getdetailsvalue.description1 = dtCbfHeader.Rows[i]["cbfdetails_desc"].ToString();
                        getdetailsvalue.att_identify = dtCbfHeader.Rows[i]["cbfdetails_gid"].ToString();
                        getdetailsvalue.qty = Convert.ToInt32(dtCbfHeader.Rows[i]["prdetails_qty"].ToString());
                        getdetailsvalue.unitAmt = Convert.ToDecimal(dtCbfHeader.Rows[i]["pipinput_rate"].ToString());
                        getdetailsvalue.total = Convert.ToDecimal(dtCbfHeader.Rows[i]["pipinput_costestimation"].ToString());
                        getdetailsvalue.remarks = dtCbfHeader.Rows[i]["remarks"].ToString();
                        getdetailsvalue.chartOfAccount = dtCbfHeader.Rows[i]["assetcategory_glno"].ToString();
                        getdetailsvalue.fccc = Convert.ToInt32(dtCbfHeader.Rows[i]["fccc"].ToString());
                        getdetailsvalue.budgetLine = Convert.ToInt32(dtCbfHeader.Rows[i]["budgetline"].ToString());
                        getdetailsvalue.prdetailsGid = Convert.ToInt64(dtCbfHeader.Rows[i]["prdetails_gid"].ToString());
                        if (getvalues.cbfMode == "PR")
                        {
                            getdetailsvalue.prheaderGid = Convert.ToInt32(objCmnFunctions.GETprno(dtCbfHeader.Rows[i]["prdetails_gid"].ToString()));
                            getdetailsvalue.uom = dtCbfHeader.Rows[i]["uom_code"].ToString();
                        }
                        if (getvalues.cbfMode == "PAR")
                        {
                            getdetailsvalue.productgid = dtCbfHeader.Rows[i]["cbfdetails_prod_gid"].ToString();
                            getdetailsvalue.productGroupId = Convert.ToInt32(dtCbfHeader.Rows[i]["prodservice_gid"].ToString());
                            getdetailsvalue.uomGid = Convert.ToInt32(dtCbfHeader.Rows[i]["uom_gid"].ToString());
                        }
                        getdetailsvalue.attch_Gid = Convert.ToInt32(dtCbfHeader.Rows[i]["cbfdetails_gid"].ToString());


                        objsum.cbfDetails.Add(getdetailsvalue);

                        if (prdetails.ToString() == "")
                        {
                            //prdetails.Add(0);
                            prdetails = getdetailsvalue.prdetailsGid.ToString();
                        }
                        else
                        {
                            //prdetails.Add(Convert.ToInt32(getdetailsvalue.prdetailsGid)); 

                            prdetails = prdetails + "," + getdetailsvalue.prdetailsGid.ToString();
                        }


                    }

                    HttpContext.Current.Session["cbfdeails"] = objsum.cbfDetails;

                }
                if (getvalues.cbfMode == "PAR")
                {
                    string pardetailsGid = prdetails.ToString();
                    cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                    cmdQry.CommandType = CommandType.StoredProcedure;
                    cmdQry.Parameters.Add("@pardetailsgid", SqlDbType.VarChar).Value = prdetails.ToString();
                    cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "cbfedit";
                    objDataAdapter = new SqlDataAdapter(cmdQry);

                    //string lsQry=
                    //string lsQry = "select pardr.pardetails_gid,pardr.pardetails_expensetype,pardr.pardetails_isbudgeted,rq.requestfor_name,pardr.pardetails_amt,pardr.pardetails_desc,pardr.pardetails_remarks";
                    //lsQry = lsQry + " ,pardr.pardetails_year, parhr.parheader_gid,parhr.parheader_refno,convert(varchar(20),parhr.parheader_date,105)as parheader_date,parhr.parheader_enddate,parhr.parheader_desc,parhr.parheader_amt,";
                    //lsQry = lsQry + " (pardr.pardetails_amt-SUM(cbfhr.cbfheader_cbfamt)) as cbf_balancedamount,SUM(cbfhr.cbfheader_cbfamt) as cbf_utilizedamount from  fb_trn_tcbfdetails cbfdr inner join fb_trn_tpardetails pardr on ";
                    //lsQry = lsQry + " cbfdr.cbfdetails_prpardel_gid=pardr.pardetails_gid inner join fb_trn_tcbfheader cbfhr on cbfhr.cbfheader_gid=cbfdr.cbfdetails_cbfhead_gid";
                    //lsQry = lsQry + " inner join fb_trn_tparheader parhr on parhr.parheader_gid=pardr.pardetails_parhead_gid inner join iem_mst_trequestfor rq on rq.requestfor_gid=pardr.pardetails_requestfor_gid where cbfhr.cbfheader_mode='PAR'";
                    //lsQry = lsQry + " and cbfhr.cbfheader_isremoved='N' and cbfhr.cbfheader_iscancelled='N' and cbfdr.cbfdetails_isremoved='N' and pardr.pardetails_gid in ('" + prdetails + "') and pardetails_isremoved='N'";
                    //lsQry = lsQry + " and parhr.parheader_isremoved='N' group by parhr.parheader_gid,parhr.parheader_refno, parheader_date,parhr.parheader_enddate,parhr.parheader_desc,parhr.parheader_amt, pardr.pardetails_gid,";
                    //lsQry = lsQry + " pardr.pardetails_expensetype,pardr.pardetails_isbudgeted,rq.requestfor_name,pardr.pardetails_amt,pardr.pardetails_desc,pardr.pardetails_remarks,pardr.pardetails_year";
                    CbfParHeader obj_getvalues;
                    CbfParDetails getpardetails;
                    objsum.cbfParheader = new List<CbfParHeader>();
                    objsum.cbfPardetailslst = new List<CbfParDetails>();
                    DataTable dt2 = new DataTable();
                    //cmdQry = new SqlCommand(lsQry, objConn);
                    //cmdQry.CommandType = CommandType.Text;
                    //objDataAdapter = new SqlDataAdapter(cmdQry);
                    objDataAdapter.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        //int balancedAmt=Convert.ToInt32(dt2.Rows[0]["parheader_amt"]);
                        //int utilizedAmount=0;
                        //for (int i = 0; i < dt2.Rows.Count; i++)
                        //{
                        //utilizedAmount += Convert.ToInt32(dt2.Rows[i]["utilised_amt"]);
                        obj_getvalues = new CbfParHeader()
                        {
                            parGid = Convert.ToInt64(dt2.Rows[0]["parheader_gid"].ToString()),
                            parNo = dt2.Rows[0]["parheader_refno"].ToString(),
                            parDate = dt2.Rows[0]["parheader_date"].ToString(),
                            parAmt = Convert.ToDecimal(dt2.Rows[0]["parheader_amt"].ToString()),
                            parDesc = dt2.Rows[0]["parheader_desc"].ToString(),
                            //balancedAmt = (dt2.Rows[0]["cbf_balancedamount"].ToString() != "" ? Convert.ToDecimal(dt2.Rows[0]["cbf_balancedamount"].ToString()) : 0),
                            //balancedAmt= Convert.ToDecimal(balancedAmt-Convert.ToInt32(dt2.Rows[i]["utilised_amt"])),
                            balancedAmt = Convert.ToDecimal(dt2.Rows[0]["PARBAlanceamt"]),
                            //utilizedAmt = (dt2.Rows[0]["cbf_utilizedamount"].ToString() != "" ? Convert.ToDecimal(dt2.Rows[0]["cbf_utilizedamount"].ToString()) : 0),
                            //utilizedAmt = (dt2.Rows[0]["cbfheader_cbfamt"].ToString() != "" ? Convert.ToDecimal(dt2.Rows[0]["cbfheader_cbfamt"].ToString()) : 0)
                            utilizedAmt = Convert.ToDecimal(dt2.Rows[0]["utilised_amt"])
                        };
                        // obj_getvalues.utilizedAmt = utilizedAmount;
                        objsum.cbfParheader.Add(obj_getvalues);
                        //}
                    }

                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        CbfSumEntity objcbfsum = new CbfSumEntity();
                        objcbfsum.Budgeted = dt2.Rows[i]["pardetails_gid"].ToString();
                        getpardetails = new CbfParDetails()
                        {
                            pardet_Gid = Convert.ToInt64(dt2.Rows[i]["pardetails_gid"].ToString()),
                            par_Expense = dt2.Rows[i]["pardetails_expensetype"].ToString(),
                            par_Requestfor = dt2.Rows[i]["requestfor_name"].ToString(),
                            par_Budget = dt2.Rows[i]["pardetails_isbudgeted"].ToString(),
                            description = dt2.Rows[i]["pardetails_desc"].ToString(),
                            year = dt2.Rows[i]["pardetails_year"].ToString(),
                            lin_Amt = Convert.ToDecimal(dt2.Rows[i]["pardetails_amt"].ToString()),
                            //  ulis_Amt = Convert.ToDecimal(dt2.Rows[i]["cbf_utilizedamount"].ToString()),
                            ulis_Amt = Convert.ToDecimal(dt2.Rows[i]["utilised_amt"].ToString()),
                            c_FwdAmount = Convert.ToDecimal(dt2.Rows[i]["TransferIn"].ToString()),
                            b_FwdAmount = Convert.ToDecimal(dt2.Rows[i]["TransferOut"].ToString()),
                            balance = Convert.ToDecimal(dt2.Rows[i]["PARDetailBalanceAmount"].ToString()),
                            // getpardetails.b_fwdamount = dt.Rows[i]["pardetails_year"].ToString();
                            // lin_Amt = Convert.ToDecimal(dt2.Rows[i]["pardetails_amt"].ToString()),
                            // getpardetails.ulis_amt=dt.Rows[i][""].ToString();
                            //getpardetails.c_fwdamount=dt.Rows[i][""].ToString();
                            //  getpardetails.balance=dt.Rows[i][""].ToString();
                            remarks = dt2.Rows[i]["pardetails_remarks"].ToString(),
                        };
                        objsum.cbfPardetailslst.Add(getpardetails);
                    }


                }
                else if (getvalues.cbfMode == "PR")
                {
                    string lsQrypr = "SELECT  prhr.prheader_gid,prhr.prheader_refno,prhr.prheader_date,prhr.prheader_desc,b.branch_gid,b.branch_name,d.employee_dept_name,r.requestfor_name";
                    lsQrypr = lsQrypr + ",prdr.prdetails_gid,prdr.pipinput_costestimation,prdr.pipinput_rate,prdr.pipinput_remarks,pod.prodservice_code,pod.prodservice_name,prdr.prdetails_qty";
                    lsQrypr = lsQrypr + ",pod.prodservice_code,uom.uom_code ,prdr.prdetails_prodservicedesc,podgrp.prodservice_name prdetails_prodservicegid,pod.prodservice_description,prdr.prdetails_prheader_gid  FROM fb_trn_tprdetails prdr";
                    lsQrypr = lsQrypr + " left join fb_trn_tprheader prhr on prdr.prdetails_prheader_gid=prhr.prheader_gid";
                    lsQrypr = lsQrypr + " inner join fb_mst_tprodservice pod on pod.prodservice_gid=prdr.prdetails_prodservicegid inner join iem_mst_tuom uom on uom.uom_gid=prdr.prdetails_uom_code";
                    lsQrypr = lsQrypr + " left join iem_mst_tbranch b on b.branch_gid=prhr.prheader_branchgid left join iem_mst_temployee d on  d.employee_gid=prhr.prheader_rasiergid ";
                    lsQrypr = lsQrypr + " join  iem_mst_trequestfor r on r.requestfor_gid=prhr.prheader_requestforgid  left join fb_mst_tprodservice podgrp on podgrp.prodservice_gid=prdr.prdetails_prodservgrp_gid";
                    lsQrypr = lsQrypr + " where prdetails_isremoved='N' and prhr.prheader_isremoved='N'";
                    lsQrypr = lsQrypr + " and prhr.prheader_iscancelled='N' and prhr.prheader_status=5 and prdetails_gid in (" + prdetails + ")";
                    DataTable dt2 = new DataTable();
                    cmdQry = new SqlCommand(lsQrypr, objConn);
                    cmdQry.CommandType = CommandType.Text;
                    objDataAdapter = new SqlDataAdapter(cmdQry);
                    objDataAdapter.Fill(dt2);

                    if (dt2.Rows.Count > 0)
                    {
                        getprheader = new CbfPrHeader()
                        {
                            prhed_Gid = Convert.ToInt64(dt2.Rows[0]["prheader_gid"].ToString()),
                            prNo = dt2.Rows[0]["prheader_refno"].ToString(),
                            branch_Gid = Convert.ToInt32(string.IsNullOrEmpty(dt2.Rows[0]["branch_gid"].ToString()) ? "0" : dt2.Rows[0]["branch_gid"].ToString()),
                            branch = dt2.Rows[0]["branch_name"].ToString(),
                            dept = dt2.Rows[0]["employee_dept_name"].ToString(),
                            description = dt2.Rows[0]["prheader_desc"].ToString(),
                            prRequestFor = dt2.Rows[0]["requestfor_name"].ToString(),
                            prDate = convertoDate(dt2.Rows[0]["prheader_date"].ToString()),

                        };
                        objsum.cbfPrheader.Add(getprheader);
                    }
                    if (dt2.Rows.Count > 0)
                    {
                        System.Web.HttpContext.Current.Session["AccessTokenprdetails"] = dt2;
                        DataView dv = new DataView();
                        dt2.DefaultView.RowFilter = "prheader_gid = " + dt2.Rows[0]["prheader_gid"].ToString() + "";
                        dv = dt2.DefaultView;

                        if (dv.Count > 0)
                        {
                            for (int j = 0; j < dv.Count; j++)
                            {
                                getprdetails = new CbfPrDetails()
                                {
                                    prdetGid = Convert.ToInt32(dv[j]["prdetails_gid"].ToString()),
                                    productCode = dv[j]["prodservice_code"].ToString(),
                                    productGroup = dv[j]["prdetails_prodservicegid"].ToString(),
                                    productName = dv[j]["prodservice_name"].ToString(),
                                    productDescription = dv[j]["prodservice_description"].ToString(),
                                    unit = dv[j]["uom_code"].ToString(),
                                    qty = Convert.ToInt32(dv[j]["prdetails_qty"].ToString()),
                                    rate = Convert.ToDecimal(dv[j]["pipinput_rate"].ToString()),
                                    costestimation = Convert.ToDecimal(dv[j]["pipinput_costestimation"].ToString()),
                                };

                                objsum.cbfPrdetarils.Add(getprdetails);
                            }

                        }
                    }

                }
                objsum.attachment = new List<Attachment>();
                objsum.attachmentCbfdetails = new List<Attachment>();
                DataSet dsCbfdetatails = new DataSet();
                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.BigInt).Value = lsnumber;
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "boqattachmentfor";
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dsCbfdetatails);
                if (dsCbfdetatails.Tables[0].Rows.Count > 0 || dsCbfdetatails.Tables[0].Rows.Count != null)
                {
                    for (int i = 0; i < dsCbfdetatails.Tables[0].Rows.Count; i++)
                    {
                        Attachment attachment1 = new Attachment()
                        {
                            fileName = dsCbfdetatails.Tables[0].Rows[i]["Documnet_Name"].ToString(),
                            attachedDate = dsCbfdetatails.Tables[0].Rows[i]["Attachment_Date"].ToString(),
                            description = dsCbfdetatails.Tables[0].Rows[i]["Document_des"].ToString(),
                            attachGid = dsCbfdetatails.Tables[0].Rows[i]["Sno"].ToString(),
                        };
                        objsum.attachment.Add(attachment1);
                    }
                }
                if (dsCbfdetatails.Tables[1].Rows.Count > 0 || dsCbfdetatails.Tables[1].Rows.Count != null)
                {
                    for (int i = 0; i < dsCbfdetatails.Tables[1].Rows.Count; i++)
                    {
                        Attachment attachment1 = new Attachment()
                        {
                            fileName = dsCbfdetatails.Tables[1].Rows[i]["Documnet_Name"].ToString(),
                            attachedDate = dsCbfdetatails.Tables[1].Rows[i]["Attachment_Date"].ToString(),
                            description = dsCbfdetatails.Tables[1].Rows[i]["Document_des"].ToString(),
                            attachGid = dsCbfdetatails.Tables[1].Rows[i]["Sno"].ToString(),
                        };
                        objsum.attachmentCbfdetails.Add(attachment1);
                    }
                }
                HttpContext.Current.Session["cbfdeailsedit"] = objsum;
                HttpContext.Current.Session["cbfAttachment"] = dsCbfdetatails.Tables[0];
                HttpContext.Current.Session["cbfAttachmentDetails"] = dsCbfdetatails.Tables[1];
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return objsum;

        }
        public string getStatusName(int status_gid)
        {
            try
            {
                getconnection();

                string[,] parameter = new string[,]
                {
                    {"@statusgid",status_gid.ToString()},
                    {"@actionName","statusname"},
                };

                return objCommonIUD.ProcedureCommon(parameter, "pr_fb_trn_poheader");
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
        }

        public List<CbfSumEntity> getStatus()
        {
            List<CbfSumEntity> objraiser = new List<CbfSumEntity>();
            try
            {
                getconnection();
                DataTable objtable = new DataTable();

                CbfSumEntity objDetails;
                cmdQry = new SqlCommand("pr_fb_trn_poheader", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@actionName", "status");
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(objtable);
                foreach (DataRow row in objtable.Rows)
                {
                    objDetails = new CbfSumEntity();
                    //objDetails.statusname = row["status_name"].ToString();
                    objDetails.statusgid = Convert.ToInt32(row["status_flag"]);
                    objDetails.statusname = row["status_name"].ToString();
                    objraiser.Add(objDetails);
                }
                return objraiser;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objraiser;
            }
            finally
            {
                objConn.Close();
            }

        }


        public CbfSumEntity GetCbfSumry()
        {
            CbfSumEntity obj = new CbfSumEntity();
            try
            {
                getconnection();

                CbfSummarymodel obj_getvalues;
                obj.cbfSum = new List<CbfSummarymodel>();
                DataTable dt = new DataTable();

                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.BigInt).Value = 0;
                cmdQry.Parameters.Add("@cbfheader_rasier_gid", SqlDbType.BigInt).Value = objCmnFunctions.GetLoginUserGid();
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        obj_getvalues = new CbfSummarymodel()
                        {
                            cbfGid = Convert.ToInt32(dt.Rows[i]["cbfheader_gid"].ToString()),
                            cbfNo = dt.Rows[i]["cbfheader_cbfno"].ToString(),
                            cbfDate = (dt.Rows[i]["cbfheader_date"].ToString()),
                            cbfEnddate = (dt.Rows[i]["cbfheader_enddate"].ToString()),
                            cbfProjectOwner = dt.Rows[i]["cbfheader_projectowner"].ToString(),
                            cbfDevi_Amo = Convert.ToDecimal(dt.Rows[i]["cbfheader_Devi_amt"].ToString()),
                            cbfAmo = Convert.ToDecimal(dt.Rows[i]["cbfheader_cbfamt"].ToString()),
                            cbfDescription = dt.Rows[i]["cbfheader_desc"].ToString(),
                            fincon_Bugt = dt.Rows[i]["cbfheader_isbudgeted"].ToString(),
                            cbfRequestfor = dt.Rows[i]["cbfheader_department"].ToString(),
                            cbfStatus = dt.Rows[i]["cbfheader_status"].ToString(),
                            cbfMode = dt.Rows[i]["cbfheader_mode"].ToString()
                        };

                        obj.cbfSum.Add(obj_getvalues);
                    }
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
                objConn.Close();
            }
        }

        public string convertoDate(string lsdate1)
        {
            try
            {
                string date2 = lsdate1;
                string convdate = string.Empty;
                DateTime convdate2 = Convert.ToDateTime(lsdate1);
                string format = "yyyy/MM/dd";
                convdate = convdate2.ToString(format);
                return convdate;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }

        }
        public string GetStatusName(int lnstatus_gid)
        {
            try
            {
                getconnection();
                string[,] lsParameter = new string[,] {
                    {"@requestgid",lnstatus_gid.ToString()},
                    {"@action","statusname"},
                };
                return objCommonIUD.ProcedureCommon(lsParameter, "pr_fb_trn_cbfsummary");
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw ex;
            }

            finally
            {
                objConn.Close();
            }
        }
        public List<CbfRaiseHeader> GetList()
        {
            List<CbfRaiseHeader> objproduct = new List<CbfRaiseHeader>();
            try
            {
                CbfRaiseHeader objraiser;
                getconnection();

                DataTable dt = new DataTable();
                cmdQry = new SqlCommand("pr_fb_mst_cbfprojectowner", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@action", "Select");
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    objraiser = new CbfRaiseHeader()
                    {
                        projectOwnerGid = Convert.ToInt32(row["projectowner_gid"].ToString()),
                        projectOwnerName = row["projectowner_name"].ToString(),

                    };
                    objproduct.Add(objraiser);
                }

                return objproduct;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objproduct;
            }

            finally
            {
                objConn.Close();
            }
        }

        public List<CbfRaiseHeader> GetBranchCode()
        {
            List<CbfRaiseHeader> objproduct = new List<CbfRaiseHeader>();
            try
            {

                CbfRaiseHeader obj;
                getconnection();
                DataTable dt = new DataTable();

                cmdQry = new SqlCommand("pr_fb_mst_bracnhcode", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@action", "Select");
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    obj = new CbfRaiseHeader()
                    {
                        branchcodeGid = Convert.ToInt32(row["branch_gid"].ToString()),
                        branchcodeName = row["branch_code"].ToString(),
                    };

                    objproduct.Add(obj);
                }

                return objproduct;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objproduct;
            }

            finally
            {
                objConn.Close();
            }
        }
        public List<CbfRaiseHeader> GetList1()
        {
            List<CbfRaiseHeader> objproduct = new List<CbfRaiseHeader>();
            try
            {
                CbfRaiseHeader obj;
                getconnection();
                DataTable dt = new DataTable();

                cmdQry = new SqlCommand("pr_fb_mst_trequestfor", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@action", "Select");
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    obj = new CbfRaiseHeader()
                    {
                        requeestForGid = Convert.ToInt32(row["requestfor_gid"].ToString()),
                        requestForName = row["requestfor_name"].ToString(),
                    };
                    objproduct.Add(obj);
                }
                return objproduct;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objproduct;
            }

            finally
            {
                objConn.Close();
            }
        }



        public CbfSumEntity GetPrHeader(int lnPrHeaderGid)
        {
            CbfSumEntity obj = new CbfSumEntity();
            try
            {
                
                CbfPrHeader obj_getvalues;
                obj.cbfPrheader = new List<CbfPrHeader>();
                getconnection();
                DataTable dt = new DataTable();

                cmdQry = new SqlCommand("pr_fb_trn_prsummary", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                if (lnPrHeaderGid != 0)
                {
                    cmdQry.Parameters.AddWithValue("@prheader_requestforgid", lnPrHeaderGid);
                }

                cmdQry.Parameters.AddWithValue("@action", "select");
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    obj_getvalues = new CbfPrHeader()
                    {
                        prhed_Gid = Convert.ToInt64(row["prheader_gid"].ToString()),
                        prNo = row["prheader_refno"].ToString(),
                        branch_Gid = Convert.ToInt32(row["branch_gid"].ToString()),
                        branch = row["branch_name"].ToString(),
                        dept = row["employee_dept_name"].ToString(),
                        description = row["prheader_desc"].ToString(),
                        prRequestFor = row["requestfor_name"].ToString(),
                        prDate = row["prheader_date"].ToString(),
                    };
                    obj.cbfPrheader.Add(obj_getvalues);
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
                objConn.Close();
            }
        }
        public CbfSumEntity GetParHeader(int cbfgid = 0, string isBudgeted = "Y")
        {
            CbfSumEntity obj = new CbfSumEntity();
            try
            {
                obj.result = GetReqGroup();

                CbfParHeader obj_getvalues;
                obj.cbfParheader = new List<CbfParHeader>();
                getconnection();

                DataTable dt = new DataTable();
                cmdQry = new SqlCommand("pr_fb_trn_prsummary", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@cbfheadergid", cbfgid);
                cmdQry.Parameters.AddWithValue("@isbudgeted", isBudgeted);
                cmdQry.Parameters.AddWithValue("@action", "parheaderselect");
                cmdQry.Parameters.AddWithValue("@requestforname", string.IsNullOrEmpty(obj.result.ToString()) ? "0" : obj.result.ToString());
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        obj_getvalues = new CbfParHeader()
                        {
                            parGid = Convert.ToInt64(row["parheader_gid"].ToString()),
                            parNo = row["parheader_refno"].ToString(),
                            parDate = row["parheader_date"].ToString(),
                            // par_Budget = row["par_Budget"].ToString(),
                            //  parAmt = Convert.ToDecimal(row["parheader_amt"].ToString()),
                            parAmt = Convert.ToDecimal(row["pardetails_amt"].ToString()),
                            parDesc = row["parheader_desc"].ToString(),
                            balancedAmt = (row["cbf_balancedamount"].ToString() != "" ? Convert.ToDecimal(row["cbf_balancedamount"].ToString()) : 0),
                            utilizedAmt = (row["cbf_utilizedamount"].ToString() != "" ? Convert.ToDecimal(row["cbf_utilizedamount"].ToString()) : 0),
                            par_Budget = row["parheader_isbudgeted"].ToString()
                        };

                        obj.cbfParheader.Add(obj_getvalues);
                    }
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
                objConn.Close();
            }
           

        }
        //public CbfSumEntity GetParHeader(CbfParHeader objpar)
        //{
        //    CbfSumEntity obj = new CbfSumEntity();
        //    obj.result = GetReqGroup();
        //    CbfParHeader obj_getvalues;
        //    obj.cbfParheader = new List<CbfParHeader>();
        //    getconnection();

        //    DataTable dt = new DataTable();
        //    cmdQry = new SqlCommand("pr_fb_trn_prsummary", objConn);
        //    cmdQry.CommandType = CommandType.StoredProcedure;
        //    cmdQry.Parameters.AddWithValue("@action", "parheaderselect");
        //    //cmdQry.Parameters.AddWithValue("@pardetails_isbudgeted", budgeted.ToString());
        //    cmdQry.Parameters.AddWithValue("@requestforname", obj.result.ToString());
        //    cmdQry.Parameters.AddWithValue("@pardetails_isbudgeted", objpar.par_Budget);
        //    objDataAdapter = new SqlDataAdapter(cmdQry);
        //    objDataAdapter.Fill(dt);

        //    foreach (DataRow row in dt.Rows)
        //    {
        //        obj_getvalues = new CbfParHeader()
        //        {
        //            parGid = Convert.ToInt64(row["parheader_gid"].ToString()),
        //            parNo = row["parheader_refno"].ToString(),
        //            parDate = row["parheader_date"].ToString(),
        //            //  parAmt = Convert.ToDecimal(row["parheader_amt"].ToString()),
        //            parAmt = Convert.ToDecimal(row["pardetails_amt"].ToString()),
        //            parDesc = row["parheader_desc"].ToString(),
        //            balancedAmt = (row["cbf_balancedamount"].ToString() != "" ? Convert.ToDecimal(row["cbf_balancedamount"].ToString()) : 0),
        //            utilizedAmt = (row["cbf_utilizedamount"].ToString() != "" ? Convert.ToDecimal(row["cbf_utilizedamount"].ToString()) : 0),
        //        };

        //        obj.cbfParheader.Add(obj_getvalues);
        //    }
        //    return obj;

        //}
        public CbfSumEntity GetPrDetail(CbfPrHeader PrHeGid)
        {
            CbfSumEntity obj = new CbfSumEntity();
            try
            {
               
                CbfPrDetails obj_getprdetails;
                obj.cbfPrdetarils = new List<CbfPrDetails>();
                getconnection();
                DataTable dt = new DataTable();

                cmdQry = new SqlCommand("pr_fb_trn_prsummary", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@action", "prdetailselect");
                cmdQry.Parameters.AddWithValue("@prheader_gid", PrHeGid.prhed_Gid);
                // cmd.Parameters.AddWithValue("@prheader_gid", 0);
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);

                foreach (DataRow rows in dt.Rows)
                {
                    obj_getprdetails = new CbfPrDetails
                    {
                        prheaderGid = Convert.ToInt32(PrHeGid.prhed_Gid),
                        prdetGid = Convert.ToInt32(rows["prdetails_gid"].ToString()),
                        productCode = rows["prodservice_code"].ToString(),
                        productGroup = rows["prodservice_description"].ToString(),
                        productName = rows["prodservice_name"].ToString(),
                        productDescription = rows["prodservice_description"].ToString(),
                        unit = rows["uom_code"].ToString(),
                        qty = Convert.ToInt32(rows["prdetails_qty"].ToString()),
                        rate = Convert.ToDecimal(rows["pipinput_rate"].ToString()),
                        costestimation = Convert.ToDecimal(rows["pipinput_costestimation"].ToString()),
                    };
                    obj.cbfPrdetarils.Add(obj_getprdetails);
                }
                return (obj);
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }

            finally
            {
                objConn.Close();
            }
        }
        public CbfSumEntity GetPrHeaderopex(int lnPrHeaderGid)
        {
            CbfSumEntity obj = new CbfSumEntity();
            try
            {
                
                CbfPrHeader obj_getvalues;
                obj.cbfPrheader = new List<CbfPrHeader>();
                getconnection();
                DataTable dt = new DataTable();

                cmdQry = new SqlCommand("pr_fb_trn_topexprdetails", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                if (lnPrHeaderGid != 0)
                {
                    cmdQry.Parameters.AddWithValue("@prheader_requestforgid", lnPrHeaderGid);
                }
                //cmdQry.Parameters.AddWithValue("@requestvalue", reqvl);
                cmdQry.Parameters.AddWithValue("@action", "Prheaderopex");
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    obj_getvalues = new CbfPrHeader()
                    {
                        prhed_Gid = Convert.ToInt64(row["prheader_gid"].ToString()),
                        prNo = row["prheader_refno"].ToString(),
                        branch = row["branch_name"].ToString(),
                        dept = row["employee_dept_name"].ToString(),
                        description = row["prheader_desc"].ToString(),
                        prRequestFor = row["requestfor_name"].ToString(),
                        prDate = row["prheader_date"].ToString(),
                    };
                    obj.cbfPrheader.Add(obj_getvalues);
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
                objConn.Close();
            }
        }
        public CbfSumEntity GetParHeaderopex(CbfPrHeader cbfhead)
        {
            CbfSumEntity obj = new CbfSumEntity();
            try
            {
                CbfParHeader obj_getvalues;
                obj.cbfParheader = new List<CbfParHeader>();
                getconnection();

                DataTable dt = new DataTable();
                cmdQry = new SqlCommand("pr_fb_trn_topexprdetails", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@budget", cbfhead.dept);
                cmdQry.Parameters.AddWithValue("@requestvalue", cbfhead.branch);
                cmdQry.Parameters.AddWithValue("@action", "parheader");
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    obj_getvalues = new CbfParHeader()
                    {
                        parGid = Convert.ToInt64(row["parheader_gid"].ToString()),
                        parNo = row["parheader_refno"].ToString(),
                        parDate = row["parheader_date"].ToString(),
                        parAmt = Convert.ToDecimal(row["parheader_amt"].ToString()),
                        parDesc = row["parheader_desc"].ToString(),
                    };

                    obj.cbfParheader.Add(obj_getvalues);
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
                objConn.Close();
            }
          

        }
        public CbfSumEntity GetPrDetailopex(CbfPrHeader PrHeGid)
        {
            CbfSumEntity obj = new CbfSumEntity();
            try
            {
             
                CbfPrDetails obj_getprdetails;
                obj.cbfPrdetarils = new List<CbfPrDetails>();
                getconnection();
                DataTable dt = new DataTable();

                cmdQry = new SqlCommand("pr_fb_trn_topexprdetails", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@action", "Prdetailsopex");
                cmdQry.Parameters.AddWithValue("@prheader_gid", PrHeGid.prhed_Gid);
                // cmd.Parameters.AddWithValue("@prheader_gid", 0);
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);

                foreach (DataRow rows in dt.Rows)
                {
                    obj_getprdetails = new CbfPrDetails
                    {
                        prheaderGid = Convert.ToInt32(PrHeGid.prhed_Gid),
                        prdetGid = Convert.ToInt32(rows["prdetails_gid"].ToString()),
                        productCode = rows["prodservice_code"].ToString(),
                        productGroup = rows["product"].ToString(),
                        productName = rows["prodservice_name"].ToString(),
                        productDescription = rows["prodservice_description"].ToString(),
                        rate = Convert.ToDecimal(rows["pipinput_rate"].ToString()),
                        costestimation = Convert.ToDecimal(rows["pipinput_costestimation"].ToString()),
                    };
                    obj.cbfPrdetarils.Add(obj_getprdetails);
                }
                return (obj);
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }

            finally
            {
                objConn.Close();
            }
        }
        public CbfSumEntity GetPrDetailEdit(CbfPrHeader PrHeGid)
        {
            CbfSumEntity obj = new CbfSumEntity();
            try
            {
               
                CbfPrDetails obj_getprdetails;
                obj.cbfPrdetarils = new List<CbfPrDetails>();
                getconnection();
                DataTable dt = new DataTable();

                cmdQry = new SqlCommand("pr_fb_trn_prsummary", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@action", "prdetailselectedit");
                cmdQry.Parameters.AddWithValue("@prheader_gid", PrHeGid.prhed_Gid);
                cmdQry.Parameters.AddWithValue("@cbfheader_gid", HttpContext.Current.Session["cbfgid"]);
                // cmd.Parameters.AddWithValue("@prheader_gid", 0);
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);

                foreach (DataRow rows in dt.Rows)
                {
                    obj_getprdetails = new CbfPrDetails
                    {
                        prdetGid = Convert.ToInt32(rows["prdetails_gid"].ToString()),
                        productCode = rows["prodservice_code"].ToString(),
                        productGroup = rows["prodservice_description"].ToString(),
                        productName = rows["prodservice_name"].ToString(),
                        productDescription = rows["prodservice_description"].ToString(),
                        unit = rows["uom_code"].ToString(),
                        qty = Convert.ToInt32(rows["prdetails_qty"].ToString()),
                        rate = Convert.ToDecimal(rows["pipinput_rate"].ToString()),
                        costestimation = Convert.ToDecimal(rows["pipinput_costestimation"].ToString()),
                    };
                    obj.cbfPrdetarils.Add(obj_getprdetails);
                }
                return (obj);
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }

            finally
            {
                objConn.Close();
            }
        }
        public string GetBranchNameForEdit(int branchid)
        {
            try
            {
                getconnection();
                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@branchid", branchid);
                cmdQry.Parameters.AddWithValue("@action", "getbranch");
                string cbfbranch = (string)cmdQry.ExecuteScalar();
                return cbfbranch;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }

        }
        public string Rquestvalue(string reqvl)
        {
            try
            {
                getconnection();
                cmdQry = new SqlCommand("pr_fb_trn_topexprdetails", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@requestvalue", reqvl);
                cmdQry.Parameters.AddWithValue("@action", "Requestvalue");
                cmdQry.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmdQry);
                da = new SqlDataAdapter(cmdQry);
                DataTable dt = new DataTable();
                da.Fill(dt);
                string res1 = "";
                if (dt.Rows.Count > 0)
                {
                    res1 = dt.Rows[0]["requestfor_gid"].ToString();
                }
                return res1;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }

        }
        //public GetBranchDetailsForCbf
        public string InsertCbfHeader(CbfRaiseHeader cbfheader, DataTable dt)
        {
            try
            {
                getconnection();
                int pardetails = 0;
                if (dt.Rows.Count < 0)
                {
                    return "Please Enter Any Cbf Details Line";
                }
                DataTable dt2 = (DataTable)System.Web.HttpContext.Current.Session["AccessTokenheader"];
                DataTable dtattachment = (DataTable)System.Web.HttpContext.Current.Session["AccessToken"];
                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;

                cmdQry.Parameters.Add("@cbfheader_cbfno", SqlDbType.VarChar).Value = objCmnFunctions.GetSequenceNoFb("CBF", cbfheader.requestFor, "");
                cmdQry.Parameters.Add("@cbfheader_date", SqlDbType.Date).Value = convertoDate(cbfheader.cbfDate);
                cmdQry.Parameters.Add("@cbfheader_enddate", SqlDbType.Date).Value = convertoDate(cbfheader.cbfEnddate);
                cmdQry.Parameters.Add("@cbfheader_projectowner", SqlDbType.Int).Value = cbfheader.projectOwner;
                cmdQry.Parameters.Add("@cbfheader_bracnh_gid", SqlDbType.Int).Value = cbfheader.branchCode;
                cmdQry.Parameters.Add("@cbfheader_mode", SqlDbType.VarChar).Value = cbfheader.cbfMode;
                cmdQry.Parameters.Add("@cbfheader_approvaltype", SqlDbType.VarChar).Value = cbfheader.cbfType;
                cmdQry.Parameters.Add("@cbfheader_isbudgeted", SqlDbType.VarChar).Value = cbfheader.budgeted;
                cmdQry.Parameters.Add("@cbfheader_Devi_amt", SqlDbType.Decimal).Value = cbfheader.deviationAmt;
                cmdQry.Parameters.Add("@cbfheader_cbfamt", SqlDbType.Decimal).Value = cbfheader.cbfAmt;
                cmdQry.Parameters.Add("@cbfheader_desc", SqlDbType.VarChar).Value = cbfheader.description;
                cmdQry.Parameters.Add("@cbfheader_requestfor_gid", SqlDbType.VarChar, 50).Value = cbfheader.requestFor;

                if (Convert.ToInt32(cbfheader.requestFor) == 3)
                {
                    cmdQry.Parameters.Add("@cbfheader_requesttype", SqlDbType.VarChar).Value = cbfheader.requestType;
                }
                cmdQry.Parameters.Add("@cbfheader_remarks", SqlDbType.VarChar).Value = cbfheader.justfication;
                cmdQry.Parameters.Add("@cbfheader_isbranchsingle", SqlDbType.VarChar).Value = cbfheader.branchType;
                // cmdQry.Parameters.Add("@cbfheader_prpar_gid", SqlDbType.VarChar).Value = cbfheader.cbfNo;
                cmdQry.Parameters.Add("@cbfheader_prpar_gid", SqlDbType.VarChar).Value = cbfheader.ParPRGid;
                cmdQry.Parameters.Add("@cbfheader_cbfobf_flag", SqlDbType.VarChar).Value = "C";
                cmdQry.Parameters.Add("@cbfheader_status", SqlDbType.VarChar).Value = "1";
                cmdQry.Parameters.Add("@cbfheader_rasier_gid", SqlDbType.Int).Value = Convert.ToInt32(objCmnFunctions.GetLoginUserGid());
                cmdQry.Parameters.Add("@cbfheader_budgetowner_gid", SqlDbType.Int).Value = Convert.ToInt32(cbfheader.BudgetownerGid);
                //cmdQry.Parameters.Add("@cbfheader_submit_date", SqlDbType.SmallDateTime).Value = DateTime.Now.ToString();
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "insert";
                cmdQry.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;
                string date = cmdQry.Parameters.ToString();
                int a = cmdQry.ExecuteNonQuery();
                int yourResult = Convert.ToInt32(cmdQry.Parameters["@result"].Value);
                if (a == 1)
                {
                    if (dtattachment != null)
                    {
                        if (dtattachment.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtattachment.Rows.Count; i++)
                            {
                                getconnection();
                                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                                cmdQry.CommandType = CommandType.StoredProcedure;
                                cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.Int).Value = yourResult;
                                cmdQry.Parameters.Add("@attachment_ref_flag", SqlDbType.TinyInt).Value = 2;
                                cmdQry.Parameters.Add("@attachment_attachmenttype_gid", SqlDbType.SmallInt).Value = 3;
                                cmdQry.Parameters.Add("@attachment_filename", SqlDbType.VarChar, 256).Value = dtattachment.Rows[i]["Documnet_Name"].ToString();
                                cmdQry.Parameters.Add("@attachment_desc", SqlDbType.VarChar).Value = dtattachment.Rows[i]["Document_des"].ToString();
                                cmdQry.Parameters.Add("@attachment_date", SqlDbType.Date).Value = convertoDate(dtattachment.Rows[i]["Attachment_Date"].ToString());
                                cmdQry.Parameters.Add("@attachment_by", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "attachment";
                                int a2 = cmdQry.ExecuteNonQuery();
                                if (a2 != 1)
                                {
                                    return "Not Inserted CBF Header Attachment";
                                }
                            }
                        }
                    }
                    if (cbfheader.cbfMode == "PAR")
                    {
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Columns.Contains("myref"))
                                {
                                    if (dt.Rows[i]["myref"].ToString() == "1")
                                    {

                                        cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                                        cmdQry.CommandType = CommandType.StoredProcedure;
                                        cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.Int).Value = Convert.ToInt32(yourResult);
                                        cmdQry.Parameters.Add("@cbfdetails_prodgroup_gid", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["productgid"].ToString()) ? "0" : dt.Rows[i]["productgid"].ToString());
                                        cmdQry.Parameters.Add("@cbfdetails_prodservice_gid", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["productgroup"].ToString()) ? "0" : dt.Rows[i]["productgroup"].ToString());
                                        cmdQry.Parameters.Add("@cbfdetails_parprdesc", SqlDbType.VarChar).Value = dt.Rows[i]["productdescription"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_desc", SqlDbType.VarChar).Value = dt.Rows[i]["remarks"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_uom_gid", SqlDbType.Int).Value = Convert.ToInt32(dt.Rows[i]["uom"].ToString());
                                        cmdQry.Parameters.Add("@cbfdetails_qty", SqlDbType.Int).Value = Convert.ToInt32(dt.Rows[i]["qty"].ToString());
                                        cmdQry.Parameters.Add("@cbfdetails_unitprice", SqlDbType.Decimal).Value = Convert.ToDecimal(dt.Rows[i]["unitamount"].ToString());
                                        cmdQry.Parameters.Add("@cbfdetails_totalamt", SqlDbType.Decimal).Value = Convert.ToDecimal(dt.Rows[i]["total"].ToString());
                                        cmdQry.Parameters.Add("@cbfdetails_remarks", SqlDbType.VarChar).Value = dt.Rows[i]["remarks"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_chartofacc", SqlDbType.VarChar).Value = dt.Rows[i]["chartofaccount"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_FCCC", SqlDbType.Int).Value = Convert.ToInt32(dt.Rows[i]["fccc"].ToString());
                                        cmdQry.Parameters.Add("@cbfdetails_budgetline", SqlDbType.Int).Value = Convert.ToInt32(dt.Rows[i]["budgetline"].ToString());
                                        cmdQry.Parameters.Add("@cbfdetails_prpardel_gid", SqlDbType.Int).Value = Convert.ToInt32(dt.Rows[i]["prdetails_gid"].ToString());
                                        cmdQry.Parameters.Add("@cbfheader_status", SqlDbType.VarChar).Value = "1";
                                        cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "insertdetails";
                                        cmdQry.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;
                                        int a1 = cmdQry.ExecuteNonQuery();
                                        pardetails = Convert.ToInt32(cmdQry.Parameters["@result"].Value);
                                        if (a1 != 1)
                                        {
                                            return "Not Inserted CBF Details";
                                        }
                                        //if (dt.Rows[i]["BOQ"].ToString() != null)
                                        //{

                                            cmdQry = new SqlCommand("pr_fb_trn_attachmentall", objConn);
                                            cmdQry.CommandType = CommandType.StoredProcedure;
                                            cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectcbfdetailsgid";
                                            cmdQry.Parameters.Add("@cbfheadgid", SqlDbType.Int).Value = yourResult;
                                            cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                                            cmdQry.Parameters.Add("@process", SqlDbType.VarChar).Value = "CBFTEMP";
                                            objDataAdapter = new SqlDataAdapter(cmdQry);
                                            DataTable objTable = new DataTable();
                                            string gid = "";
                                            objDataAdapter.Fill(objTable);
                                            //for (int git = 0; git < objTable.Rows.Count; git++)
                                            //{
                                            //    gid = objTable.Rows[git]["cbfdetails_gid"].ToString();
                                            //}
                                            string refflaggid = "";
                                            string cbfdetailsflag = ""; 
                                            if (objTable.Rows.Count > 0)
                                            {
                                                gid = objTable.Rows[0]["cbfdetails_gid"].ToString();
                                                refflaggid = objTable.Rows[0]["ref_flag"].ToString();
                                                cbfdetailsflag = objTable.Rows[0]["cbf_ref_flag"].ToString();
                                            }
                                            string[,] codes = new string[,]
	                                     {
                                             {"attachment_ref_gid", gid},
                                             {"attachment_ref_flag", cbfdetailsflag}
	                                        
                                         };
                                            string[,] whrcol = new string[,]
	                                        {
                                                {"attachment_ref_flag", refflaggid},
                                                {"attachment_ref_gid", Convert.ToString(i + 1)},
                                                {"attachment_by", objCmnFunctions.GetLoginUserGid().ToString()},
                                                {"attachment_attachmenttype_gid", "2"}
                                            };
                                            objCommonIUD.UpdateCommon(codes, whrcol, "iem_trn_tattachment");


                                      //  }

                                    }
                                    //if (dt2 != null)
                                    //{

                                    //    if (dt2.Rows.Count > 0)
                                    //    {
                                    //        for (int ij = 0; ij < dt2.Rows.Count; ij++)
                                    //        {
                                    //            if (dt2.Rows[ij]["prdetails"].ToString() == dt.Rows[i]["sno"].ToString())
                                    //            {
                                    //                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                                    //                cmdQry.CommandType = CommandType.StoredProcedure;
                                    //                cmdQry.Parameters.Add("@attachment_ref_flag", SqlDbType.Int).Value = 3;
                                    //                cmdQry.Parameters.Add("@attachment_attachmenttype_gid", SqlDbType.VarChar).Value = 3;
                                    //                cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.Int).Value = pardetails;
                                    //                cmdQry.Parameters.Add("@attachment_filename", SqlDbType.VarChar).Value = dt2.Rows[ij]["Documnet_Name"].ToString();
                                    //                cmdQry.Parameters.Add("@attachment_desc", SqlDbType.VarChar).Value = dt2.Rows[ij]["Document_des"].ToString();
                                    //                cmdQry.Parameters.Add("@attachment_date", SqlDbType.Date).Value = convertoDate(dt2.Rows[ij]["Attachment_Date"].ToString());
                                    //                cmdQry.Parameters.Add("@attachment_by", SqlDbType.Int).Value = 2;
                                    //                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "boqattachforpardetails";
                                    //                int a2 = cmdQry.ExecuteNonQuery();
                                    //                if (a2 != 1)
                                    //                {
                                    //                    return "Not Inserted CBF Details Attachment";
                                    //                }
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                }
                                else
                                {
                                    return "Please Save Any Cbf Details Line Item";
                                }

                            }

                        }
                        else
                        {
                            return "Please Save Any Cbf Details Line";
                        }
                    }
                    else if (cbfheader.cbfMode == "PR")
                    {
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Columns.Contains("myref"))
                                {
                                    if (dt.Rows[i]["myref"].ToString() == "1")
                                    {

                                        cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                                        cmdQry.CommandType = CommandType.StoredProcedure;
                                        cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.Int).Value = yourResult;
                                        cmdQry.Parameters.Add("@cbfdetails_prodgroup_gid", SqlDbType.Int).Value = dt.Rows[i]["prodservice_gid"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_prodservice_gid", SqlDbType.Int).Value = dt.Rows[i]["prodservice_gid"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_parprdesc", SqlDbType.VarChar).Value = dt.Rows[i]["prodservice_description"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_desc", SqlDbType.VarChar).Value = dt.Rows[i]["remarks"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_uom_gid", SqlDbType.Int).Value = dt.Rows[i]["uom_gid"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_qty", SqlDbType.Int).Value = dt.Rows[i]["prdetails_qty"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_unitprice", SqlDbType.Decimal).Value = dt.Rows[i]["pipinput_rate"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_totalamt", SqlDbType.Decimal).Value = dt.Rows[i]["pipinput_costestimation"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_remarks", SqlDbType.VarChar).Value = dt.Rows[i]["remarks"].ToString();
                                        if (dt.Rows[i]["assetcategory_glno"].ToString() != "")
                                        {
                                            cmdQry.Parameters.Add("@cbfdetails_chartofacc", SqlDbType.VarChar).Value = dt.Rows[i]["assetcategory_glno"].ToString();
                                        }
                                        else
                                        {
                                            cmdQry.Parameters.Add("@cbfdetails_chartofacc", SqlDbType.VarChar).Value = dt.Rows[i]["expcat_gl_no"].ToString();
                                        }
                                        cmdQry.Parameters.Add("@cbfdetails_FCCC", SqlDbType.Int).Value = dt.Rows[i]["fccc"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_budgetline", SqlDbType.Int).Value = dt.Rows[i]["budgetline"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_prpardel_gid", SqlDbType.Int).Value = dt.Rows[i]["prdetails_gid"].ToString();
                                        cmdQry.Parameters.Add("@cbfheader_status", SqlDbType.VarChar).Value = "1";
                                        cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "insertdetails";
                                        int a1 = cmdQry.ExecuteNonQuery();
                                        if (a1 != 1)
                                        {
                                            return "Not Inserted CBF Details";
                                        }

                                        cmdQry = new SqlCommand("pr_fb_trn_attachmentall", objConn);
                                        cmdQry.CommandType = CommandType.StoredProcedure;
                                        cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectcbfdetailsgid";
                                        cmdQry.Parameters.Add("@cbfheadgid", SqlDbType.Int).Value = yourResult;
                                        cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                                        cmdQry.Parameters.Add("@process", SqlDbType.VarChar).Value = "CBFTEMP";
                                        objDataAdapter = new SqlDataAdapter(cmdQry);
                                        DataTable objTable = new DataTable();
                                        string gid = "";
                                        objDataAdapter.Fill(objTable);
                                        //for (int git = 0; git < objTable.Rows.Count; git++)
                                        //{
                                        //    gid = objTable.Rows[git]["cbfdetails_gid"].ToString();
                                        //}
                                        string refflaggid = "";
                                        string cbfdetailsflag = "";
                                        if (objTable.Rows.Count > 0)
                                        {
                                            gid = objTable.Rows[0]["cbfdetails_gid"].ToString();
                                            refflaggid = objTable.Rows[0]["ref_flag"].ToString();
                                            cbfdetailsflag = objTable.Rows[0]["cbf_ref_flag"].ToString();
                                        }
                                        string[,] codes = new string[,]
	                                     {
                                             {"attachment_ref_gid", gid},
                                             {"attachment_ref_flag", cbfdetailsflag}
	                                        
                                         };
                                        string[,] whrcol = new string[,]
	                                        {
                                                {"attachment_ref_flag", refflaggid},
                                                {"attachment_ref_gid", Convert.ToString(i + 1)},
                                                {"attachment_by", objCmnFunctions.GetLoginUserGid().ToString()},
                                                {"attachment_attachmenttype_gid", "2"}
                                            };
                                        objCommonIUD.UpdateCommon(codes, whrcol, "iem_trn_tattachment");



                                    }
                                }
                                else
                                {
                                    return "Please Save Any Cbf Details Line";
                                }

                            }
                            //if (dt2 != null)
                            //{
                            //    if (dt2.Rows.Count > 0)
                            //    {
                            //        for (int ij = 0; ij < dt2.Rows.Count; ij++)
                            //        {
                            //            cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                            //            cmdQry.CommandType = CommandType.StoredProcedure;
                            //            cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.Int).Value = yourResult;
                            //            cmdQry.Parameters.Add("@attachment_ref_flag", SqlDbType.Int).Value = 3;
                            //            cmdQry.Parameters.Add("@attachment_attachmenttype_gid", SqlDbType.VarChar).Value = 3;
                            //            cmdQry.Parameters.Add("@prdetailsgid", SqlDbType.Int).Value = dt2.Rows[ij]["prdetails"].ToString();
                            //            cmdQry.Parameters.Add("@attachment_filename", SqlDbType.VarChar).Value = dt2.Rows[ij]["Documnet_Name"].ToString();
                            //            cmdQry.Parameters.Add("@attachment_desc", SqlDbType.VarChar).Value = dt2.Rows[ij]["Document_des"].ToString();
                            //            cmdQry.Parameters.Add("@attachment_date", SqlDbType.Date).Value = convertoDate(dt2.Rows[ij]["Attachment_Date"].ToString());
                            //            cmdQry.Parameters.Add("@attachment_by", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                            //            cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "cbfdetailsattachment";
                            //            int a2 = cmdQry.ExecuteNonQuery();
                            //            if (a2 != 1)
                            //            {
                            //                return "Not Inserted CBF Details Attachment";
                            //            }
                            //        }
                            //    }
                            //}

                        }
                    }
                }
                return "Inserted Successfully";
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }

        }
        public string InsertCbfHeader1(CbfRaiseHeader cbfheader, DataTable dt)
        {
            try
            {
                int pardetails = 0;
                int action_status = 0;
                getconnection();
                if (dt.Rows.Count < 0)
                {
                    return "Please Enter Any Cbf Details Line";
                }
                DataTable dt2 = (DataTable)System.Web.HttpContext.Current.Session["AccessTokenheader"];
                DataTable dtattachment = (DataTable)System.Web.HttpContext.Current.Session["AccessToken"];
                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;

                cmdQry.Parameters.Add("@cbfheader_cbfno", SqlDbType.VarChar).Value = objCmnFunctions.GetSequenceNoFb("CBF", cbfheader.requestFor, "");
                cmdQry.Parameters.Add("@cbfheader_date", SqlDbType.Date).Value = convertoDate(cbfheader.cbfDate);
                cmdQry.Parameters.Add("@cbfheader_enddate", SqlDbType.Date).Value = convertoDate(cbfheader.cbfEnddate);
                cmdQry.Parameters.Add("@cbfheader_projectowner", SqlDbType.Int).Value = cbfheader.projectOwner;
                cmdQry.Parameters.Add("@cbfheader_bracnh_gid", SqlDbType.Int).Value = cbfheader.branchCode;
                cmdQry.Parameters.Add("@cbfheader_mode", SqlDbType.VarChar).Value = cbfheader.cbfMode;
                cmdQry.Parameters.Add("@cbfheader_approvaltype", SqlDbType.VarChar).Value = cbfheader.cbfType;
                cmdQry.Parameters.Add("@cbfheader_isbudgeted", SqlDbType.VarChar).Value = cbfheader.budgeted;
                cmdQry.Parameters.Add("@cbfheader_Devi_amt", SqlDbType.Decimal).Value = cbfheader.deviationAmt;
                cmdQry.Parameters.Add("@cbfheader_cbfamt", SqlDbType.Decimal).Value = cbfheader.cbfAmt;
                cmdQry.Parameters.Add("@cbfheader_desc", SqlDbType.VarChar).Value = cbfheader.description;
                cmdQry.Parameters.Add("@cbfheader_requestfor_gid", SqlDbType.VarChar, 50).Value = cbfheader.requestFor;
                if (Convert.ToInt32(cbfheader.requestFor) == 3)
                {
                    cmdQry.Parameters.Add("@cbfheader_requesttype", SqlDbType.VarChar).Value = cbfheader.requestType;
                }
                cmdQry.Parameters.Add("@cbfheader_remarks", SqlDbType.VarChar).Value = cbfheader.justfication;
                cmdQry.Parameters.Add("@cbfheader_isbranchsingle", SqlDbType.VarChar).Value = cbfheader.branchType;
                // cmdQry.Parameters.Add("@cbfheader_prpar_gid", SqlDbType.VarChar).Value = cbfheader.cbfNo;
                cmdQry.Parameters.Add("@cbfheader_prpar_gid", SqlDbType.VarChar).Value = cbfheader.ParPRGid;
                cmdQry.Parameters.Add("@cbfheader_cbfobf_flag", SqlDbType.VarChar).Value = "C";
                cmdQry.Parameters.Add("@cbfheader_status", SqlDbType.VarChar).Value = "2";
                cmdQry.Parameters.Add("@cbfheader_rasier_gid", SqlDbType.Int).Value = Convert.ToInt32(objCmnFunctions.GetLoginUserGid());
                cmdQry.Parameters.Add("@cbfheader_budgetowner_gid", SqlDbType.Int).Value = Convert.ToInt32(cbfheader.BudgetownerGid);
                //cmdQry.Parameters.Add("@cbfheader_submit_date", SqlDbType.SmallDateTime).Value = DateTime.Now.ToString();
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "insert";
                cmdQry.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;

                string date = cmdQry.Parameters.ToString();
                int a = cmdQry.ExecuteNonQuery();
                int yourResult = Convert.ToInt32(cmdQry.Parameters["@result"].Value);
                if (a == 1)
                {
                    if (dtattachment != null)
                    {
                        if (dtattachment.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtattachment.Rows.Count; i++)
                            {
                                getconnection();
                                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                                cmdQry.CommandType = CommandType.StoredProcedure;
                                cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.Int).Value = yourResult;
                                cmdQry.Parameters.Add("@attachment_ref_flag", SqlDbType.TinyInt).Value = 2;
                                cmdQry.Parameters.Add("@attachment_attachmenttype_gid", SqlDbType.SmallInt).Value = 3;
                                cmdQry.Parameters.Add("@attachment_filename", SqlDbType.VarChar, 256).Value = dtattachment.Rows[i]["Documnet_Name"].ToString();
                                cmdQry.Parameters.Add("@attachment_desc", SqlDbType.VarChar).Value = dtattachment.Rows[i]["Document_des"].ToString();
                                cmdQry.Parameters.Add("@attachment_date", SqlDbType.Date).Value = convertoDate(dtattachment.Rows[i]["Attachment_Date"].ToString());
                                cmdQry.Parameters.Add("@attachment_by", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "attachment";
                                int a2 = cmdQry.ExecuteNonQuery();
                                if (a2 != 1)
                                {
                                    return "Not Inserted CBF Header Attachment";
                                }
                            }
                        }
                    }
                    int a1 = 0;
                    if (cbfheader.cbfMode == "PAR")
                    {
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Columns.Contains("myref"))
                                {
                                    if (dt.Rows[i]["myref"].ToString() == "1")
                                    {

                                        cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                                        cmdQry.CommandType = CommandType.StoredProcedure;
                                        cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.Int).Value = yourResult;
                                        cmdQry.Parameters.Add("@cbfdetails_prodgroup_gid", SqlDbType.Int).Value = dt.Rows[i]["productgid"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_prodservice_gid", SqlDbType.Int).Value = dt.Rows[i]["productgroup"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_parprdesc", SqlDbType.VarChar).Value = dt.Rows[i]["productdescription"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_desc", SqlDbType.VarChar).Value = dt.Rows[i]["remarks"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_uom_gid", SqlDbType.Int).Value = dt.Rows[i]["uom"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_qty", SqlDbType.Int).Value = dt.Rows[i]["qty"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_unitprice", SqlDbType.Decimal).Value = dt.Rows[i]["unitamount"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_totalamt", SqlDbType.Decimal).Value = dt.Rows[i]["total"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_remarks", SqlDbType.VarChar).Value = dt.Rows[i]["remarks"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_chartofacc", SqlDbType.VarChar).Value = dt.Rows[i]["chartofaccount"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_FCCC", SqlDbType.Int).Value = dt.Rows[i]["fccc"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_budgetline", SqlDbType.Int).Value = dt.Rows[i]["budgetline"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_prpardel_gid", SqlDbType.Int).Value = dt.Rows[i]["prdetails_gid"].ToString();
                                        cmdQry.Parameters.Add("@cbfheader_status", SqlDbType.VarChar).Value = "1";
                                        cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "insertdetails";
                                        cmdQry.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;
                                        a1 = cmdQry.ExecuteNonQuery();
                                        pardetails = Convert.ToInt32(cmdQry.Parameters["@result"].Value);

                                        if (a1 != 1)
                                        {
                                            return "Not Inserted CBF Details";
                                        }
                                        cmdQry = new SqlCommand("pr_fb_trn_attachmentall", objConn);
                                        cmdQry.CommandType = CommandType.StoredProcedure;
                                        cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectcbfdetailsgid";
                                        cmdQry.Parameters.Add("@cbfheadgid", SqlDbType.Int).Value = yourResult;
                                        cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                                        cmdQry.Parameters.Add("@process", SqlDbType.VarChar).Value = "CBFTEMP";
                                        objDataAdapter = new SqlDataAdapter(cmdQry);
                                        DataTable objTable = new DataTable();
                                        string gid = "";
                                        objDataAdapter.Fill(objTable);
                                        //for (int git = 0; git < objTable.Rows.Count; git++)
                                        //{
                                        //    gid = objTable.Rows[git]["cbfdetails_gid"].ToString();
                                        //}
                                        string refflaggid = "";
                                        string cbfdetailsflag = "";
                                        if (objTable.Rows.Count > 0)
                                        {
                                            gid = objTable.Rows[0]["cbfdetails_gid"].ToString();
                                            refflaggid = objTable.Rows[0]["ref_flag"].ToString();
                                            cbfdetailsflag = objTable.Rows[0]["cbf_ref_flag"].ToString();
                                        }
                                        string[,] codes = new string[,]
	                                     {
                                             {"attachment_ref_gid", gid},
                                             {"attachment_ref_flag", cbfdetailsflag}
	                                        
                                         };
                                        string[,] whrcol = new string[,]
	                                        {
                                                {"attachment_ref_flag", refflaggid},
                                                {"attachment_ref_gid", Convert.ToString(i + 1)},
                                                {"attachment_by", objCmnFunctions.GetLoginUserGid().ToString()},
                                                {"attachment_attachmenttype_gid", "2"}
                                            };
                                        objCommonIUD.UpdateCommon(codes, whrcol, "iem_trn_tattachment");


                                    }
                                    //if (dt2 != null)
                                    //{

                                    //    if (dt2.Rows.Count > 0)
                                    //    {
                                    //        for (int ij = 0; ij < dt2.Rows.Count; ij++)
                                    //        {
                                    //            if (dt2.Rows[ij]["prdetails"].ToString() == dt.Rows[i]["sno"].ToString())
                                    //            {
                                    //                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                                    //                cmdQry.CommandType = CommandType.StoredProcedure;
                                    //                cmdQry.Parameters.Add("@attachment_ref_flag", SqlDbType.Int).Value = 3;
                                    //                cmdQry.Parameters.Add("@attachment_attachmenttype_gid", SqlDbType.VarChar).Value = 3;
                                    //                cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.Int).Value = pardetails;
                                    //                cmdQry.Parameters.Add("@attachment_filename", SqlDbType.VarChar).Value = dt2.Rows[ij]["Documnet_Name"].ToString();
                                    //                cmdQry.Parameters.Add("@attachment_desc", SqlDbType.VarChar).Value = dt2.Rows[ij]["Document_des"].ToString();
                                    //                cmdQry.Parameters.Add("@attachment_date", SqlDbType.Date).Value = convertoDate(dt2.Rows[ij]["Attachment_Date"].ToString());
                                    //                cmdQry.Parameters.Add("@attachment_by", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                                    //                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "boqattachforpardetails";
                                    //                int a2 = cmdQry.ExecuteNonQuery();
                                    //                if (a2 != 1)
                                    //                {
                                    //                    return "Not Inserted CBF Details Attachment";
                                    //                }
                                    //            }
                                    //        }
                                    //    }
                                    //}
                                }
                                else
                                {
                                    return "Please Select Any CBF Details";
                                }

                            }

                        }
                        else
                        {
                            return "Please Save Any Cbf Details Line";
                        }
                    }
                    else if (cbfheader.cbfMode == "PR")
                    {
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Columns.Contains("myref"))
                                {
                                    if (dt.Rows[i]["myref"].ToString() == "1")
                                    {

                                        cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                                        cmdQry.CommandType = CommandType.StoredProcedure;
                                        cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.Int).Value = yourResult;
                                        cmdQry.Parameters.Add("@cbfdetails_prodgroup_gid", SqlDbType.Int).Value = dt.Rows[i]["prodservice_gid"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_prodservice_gid", SqlDbType.Int).Value = dt.Rows[i]["prodservice_gid"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_parprdesc", SqlDbType.VarChar).Value = dt.Rows[i]["prodservice_description"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_desc", SqlDbType.VarChar).Value = dt.Rows[i]["remarks"].ToString();
                                        if (dt.Rows[i]["uom_gid"].ToString() != "")
                                        {
                                            cmdQry.Parameters.Add("@cbfdetails_uom_gid", SqlDbType.Int).Value = dt.Rows[i]["uom_gid"].ToString();
                                        }
                                        else
                                        {
                                            cmdQry.Parameters.Add("@cbfdetails_uom_gid", SqlDbType.Int).Value = "0";
                                        }
                                        cmdQry.Parameters.Add("@cbfdetails_qty", SqlDbType.Int).Value = dt.Rows[i]["prdetails_qty"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_unitprice", SqlDbType.Decimal).Value = dt.Rows[i]["pipinput_rate"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_totalamt", SqlDbType.Decimal).Value = dt.Rows[i]["pipinput_costestimation"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_remarks", SqlDbType.VarChar).Value = dt.Rows[i]["remarks"].ToString();
                                        if (dt.Rows[i]["assetcategory_glno"].ToString() != "")
                                        {
                                            cmdQry.Parameters.Add("@cbfdetails_chartofacc", SqlDbType.VarChar).Value = dt.Rows[i]["assetcategory_glno"].ToString();
                                        }
                                        else
                                        {
                                            cmdQry.Parameters.Add("@cbfdetails_chartofacc", SqlDbType.VarChar).Value = dt.Rows[i]["expcat_gl_no"].ToString();
                                        }
                                        cmdQry.Parameters.Add("@cbfdetails_FCCC", SqlDbType.Int).Value = dt.Rows[i]["fccc"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_budgetline", SqlDbType.Int).Value = dt.Rows[i]["budgetline"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_prpardel_gid", SqlDbType.Int).Value = dt.Rows[i]["prdetails_gid"].ToString();
                                        cmdQry.Parameters.Add("@cbfheader_status", SqlDbType.VarChar).Value = "1";
                                        cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "insertdetails";
                                        a1 = cmdQry.ExecuteNonQuery();
                                        if (a1 != 1)
                                        {
                                            return "Not Inserted CBF Details";
                                        }
                                        cmdQry = new SqlCommand("pr_fb_trn_attachmentall", objConn);
                                        cmdQry.CommandType = CommandType.StoredProcedure;
                                        cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectcbfdetailsgid";
                                        cmdQry.Parameters.Add("@cbfheadgid", SqlDbType.Int).Value = yourResult;
                                        cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                                        cmdQry.Parameters.Add("@process", SqlDbType.VarChar).Value = "CBFTEMP";
                                        objDataAdapter = new SqlDataAdapter(cmdQry);
                                        DataTable objTable = new DataTable();
                                        string gid = "";
                                        objDataAdapter.Fill(objTable);
                                        //for (int git = 0; git < objTable.Rows.Count; git++)
                                        //{
                                        //    gid = objTable.Rows[git]["cbfdetails_gid"].ToString();
                                        //}
                                        string refflaggid = "";
                                        string cbfdetailsflag = "";
                                        if (objTable.Rows.Count > 0)
                                        {
                                            gid = objTable.Rows[0]["cbfdetails_gid"].ToString();
                                            refflaggid = objTable.Rows[0]["ref_flag"].ToString();
                                            cbfdetailsflag = objTable.Rows[0]["cbf_ref_flag"].ToString();
                                        }
                                        string[,] codes = new string[,]
	                                     {
                                             {"attachment_ref_gid", gid},
                                             {"attachment_ref_flag", cbfdetailsflag}
	                                        
                                         };
                                        string[,] whrcol = new string[,]
	                                        {
                                                {"attachment_ref_flag", refflaggid},
                                                {"attachment_ref_gid", Convert.ToString(i + 1)},
                                                {"attachment_by", objCmnFunctions.GetLoginUserGid().ToString()},
                                                {"attachment_attachmenttype_gid", "2"}
                                            };
                                        objCommonIUD.UpdateCommon(codes, whrcol, "iem_trn_tattachment");



                                    }
                                }
                                else
                                {
                                    return "Please Select Any CBF Details Line";
                                }


                            }
                            //if (dt2 != null)
                            //{
                            //    if (dt2.Rows.Count > 0)
                            //    {
                            //        for (int ij = 0; ij < dt2.Rows.Count; ij++)
                            //        {
                            //            cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                            //            cmdQry.CommandType = CommandType.StoredProcedure;
                            //            cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.Int).Value = yourResult;
                            //            cmdQry.Parameters.Add("@attachment_ref_flag", SqlDbType.Int).Value = 3;
                            //            cmdQry.Parameters.Add("@attachment_attachmenttype_gid", SqlDbType.VarChar).Value = 3;
                            //            cmdQry.Parameters.Add("@prdetailsgid", SqlDbType.Int).Value = dt2.Rows[ij]["prdetails"].ToString();
                            //            cmdQry.Parameters.Add("@attachment_filename", SqlDbType.VarChar).Value = dt2.Rows[ij]["Documnet_Name"].ToString();
                            //            cmdQry.Parameters.Add("@attachment_desc", SqlDbType.VarChar).Value = dt2.Rows[ij]["Document_des"].ToString();
                            //            cmdQry.Parameters.Add("@attachment_date", SqlDbType.Date).Value = convertoDate(dt2.Rows[ij]["Attachment_Date"].ToString());
                            //            cmdQry.Parameters.Add("@attachment_by", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                            //            cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "cbfdetailsattachment";
                            //            int a2 = cmdQry.ExecuteNonQuery();
                            //            if (a2 != 1)
                            //            {
                            //                return "Not Inserted CBF Details Attachment";
                            //            }
                            //        }
                            //    }
                            //}
                            

                        }
                    }
                    if (cbfheader.BudgetownerGid.ToString() != null || cbfheader.BudgetownerGid.ToString() != "")
                    {

                        string[,] codes = new string[,]
                                {
                                {"queue_date", "sysdatetime()"},
                                {"queue_ref_flag","2"},
                                {"queue_ref_gid",yourResult.ToString()},
                                {"queue_from",objCmnFunctions.GetLoginUserGid().ToString()},
                                {"queue_to_type", "I"},
                                {"queue_to", cbfheader.BudgetownerGid.ToString()},
                                {"queue_action_for","A"},
                                {"queue_prev_gid",action_status.ToString()}
                                };
                        string tname = "iem_trn_tqueue";
                        string result = objCommonIUD.InsertCommon(codes, tname);

                        if (result == "success")
                        {

                            HttpContext.Current.Session["queuegid"] = null;
                            cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                            cmdQry.CommandType = CommandType.StoredProcedure;
                            cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "mailsubmission";
                            cmdQry.Parameters.Add("@queue_gid", SqlDbType.Int).Value = yourResult;
                            string a2 = cmdQry.ExecuteScalar().ToString();
                            HttpContext.Current.Session["queuegid"] = a2;
                            string[,] codes1 = new string[,]
                            {
                                {"cbfheader_currentapprovalstage","1"}
                               
                            };
                            string[,] wherecolumn = new string[,]
                            {
                                {"cbfheader_gid",yourResult.ToString()},
                                {"cbfheader_isremoved","N"}
                            };
                            string tname1 = "fb_trn_tcbfheader";
                            string result1 = objCommonIUD.UpdateCommon(codes1, wherecolumn, tname1);
                        }
                    }
                    //cmdQry = new SqlCommand("pr_fb_trn_ApprovalQuenu", objConn);
                    //cmdQry.CommandType = CommandType.StoredProcedure;
                    //cmdQry.Parameters.Add("@refgid", SqlDbType.Int).Value = yourResult;
                    //cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                    //cmdQry.Parameters.Add("@requesttype", SqlDbType.VarChar).Value = "CBF";
                    //int a3 = cmdQry.ExecuteNonQuery();



                }
                else
                {
                    return "Not Inserted";
                }
                return "Inserted Successfully";

            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }

        }
        public string InsertCbfHeaderopex(CbfRaiseHeader cbfheader, DataTable dt)
        {
            try
            {
                int pardetails = 0;
                getconnection();

                if (dt.Rows.Count < 0)
                {
                    if (dt.Columns.Contains("vendor_gid"))
                    {
                        return "Please Enter Any OBF Details Line";
                    }
                    else
                    {
                        return "Please Enter Any OBF Details Line";
                    }
                }

                cmdQry = new SqlCommand("pr_fb_trn_obfinsert", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;

                cmdQry.Parameters.Add("@cbfheader_cbfno", SqlDbType.VarChar).Value = objCmnFunctions.GetSequenceNoFb("OBF", cbfheader.requestFor, "");
                cmdQry.Parameters.Add("@cbfheader_date", SqlDbType.Date).Value = convertoDate(cbfheader.cbfDate);

                cmdQry.Parameters.Add("@cbfheader_mode", SqlDbType.VarChar).Value = cbfheader.cbfMode;
                cmdQry.Parameters.Add("@cbfheader_isbudgeted", SqlDbType.VarChar).Value = cbfheader.budgeted;
                cmdQry.Parameters.Add("@cbfheader_cbfamt", SqlDbType.Decimal).Value = cbfheader.cbfAmt;
                cmdQry.Parameters.Add("@cbfheader_desc", SqlDbType.VarChar).Value = cbfheader.description;
                cmdQry.Parameters.Add("@cbfheader_requestfor_gid", SqlDbType.VarChar, 50).Value = cbfheader.requestFor;
                if (Convert.ToInt32(cbfheader.requestFor) == 3)
                {
                    cmdQry.Parameters.Add("@cbfheader_requesttype", SqlDbType.VarChar).Value = cbfheader.requestType;
                }
                cmdQry.Parameters.Add("@cbfheader_remarks", SqlDbType.VarChar).Value = cbfheader.justfication;
                cmdQry.Parameters.Add("@cbfheader_prpar_gid", SqlDbType.VarChar).Value = cbfheader.ParPRGid;
                cmdQry.Parameters.Add("@cbfheader_cbfobf_flag", SqlDbType.VarChar).Value = "O";
                cmdQry.Parameters.Add("@cbfheader_status", SqlDbType.VarChar).Value = "5";
                cmdQry.Parameters.Add("@cbfheader_rasier_gid", SqlDbType.Int).Value = Convert.ToInt32(objCmnFunctions.GetLoginUserGid());
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "insert";
                cmdQry.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;

                string date = cmdQry.Parameters.ToString();
                int a = cmdQry.ExecuteNonQuery();
                int yourResult = Convert.ToInt32(cmdQry.Parameters["@result"].Value);
                if (a == 1)
                {
                    int a1 = 0;
                    if (cbfheader.cbfMode == "PAR")
                    {
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Columns.Contains("Vendorgid"))
                                {


                                    cmdQry = new SqlCommand("pr_fb_trn_obfinsert", objConn);
                                    cmdQry.CommandType = CommandType.StoredProcedure;
                                    cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.Int).Value = yourResult;
                                    cmdQry.Parameters.Add("@cbfdetails_prodgroup_gid", SqlDbType.Int).Value = dt.Rows[i]["productgid"].ToString();
                                    cmdQry.Parameters.Add("@cbfdetails_prodservice_gid", SqlDbType.Int).Value = dt.Rows[i]["productgroup"].ToString();
                                    cmdQry.Parameters.Add("@cbfdetails_parprdesc", SqlDbType.VarChar).Value = dt.Rows[i]["productdescription"].ToString();

                                    cmdQry.Parameters.Add("@cbfdetails_unitprice", SqlDbType.Decimal).Value = dt.Rows[i]["unitamount"].ToString();
                                    cmdQry.Parameters.Add("@cbfdetails_totalamt", SqlDbType.Decimal).Value = dt.Rows[i]["total"].ToString();
                                    cmdQry.Parameters.Add("@cbfdetails_prpardel_gid", SqlDbType.Int).Value = Convert.ToInt32(dt.Rows[i]["prdetails_gid"].ToString());
                                    cmdQry.Parameters.Add("@cbfheader_vendorgid", SqlDbType.Int).Value = dt.Rows[i]["Vendorgid"].ToString();
                                    cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "insertdetails";
                                    cmdQry.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;
                                    a1 = cmdQry.ExecuteNonQuery();
                                    if (a1 != 1)
                                    {
                                        return "Not Inserted OBF Details";
                                    }


                                }
                                else
                                {
                                    return "Please Select Any OBF Details";
                                }

                            }

                        }
                        else
                        {
                            return "Please Save Any OBF Details Line";
                        }
                    }
                    else if (cbfheader.cbfMode == "PR")
                    {
                        if (dt.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                if (dt.Columns.Contains("Vendorgid"))
                                {


                                    cmdQry = new SqlCommand("pr_fb_trn_obfinsert", objConn);
                                    cmdQry.CommandType = CommandType.StoredProcedure;
                                    cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.Int).Value = yourResult;
                                    cmdQry.Parameters.Add("@cbfdetails_prodgroup_gid", SqlDbType.Int).Value = dt.Rows[i]["productgroupgid"].ToString();
                                    cmdQry.Parameters.Add("@cbfdetails_prodservice_gid", SqlDbType.Int).Value = dt.Rows[i]["product"].ToString();
                                    cmdQry.Parameters.Add("@cbfdetails_qty", SqlDbType.Int).Value = dt.Rows[i]["prdetails_qty"].ToString();

                                    cmdQry.Parameters.Add("@cbfdetails_parprdesc", SqlDbType.VarChar).Value = dt.Rows[i]["prodservice_description"].ToString();
                                    cmdQry.Parameters.Add("@cbfdetails_unitprice", SqlDbType.Decimal).Value = dt.Rows[i]["pipinput_rate"].ToString();
                                    cmdQry.Parameters.Add("@cbfdetails_totalamt", SqlDbType.Decimal).Value = dt.Rows[i]["pipinput_costestimation"].ToString();
                                    cmdQry.Parameters.Add("@cbfdetails_prpardel_gid", SqlDbType.Int).Value = dt.Rows[i]["prdetails_gid"].ToString();
                                    cmdQry.Parameters.Add("@cbfheader_vendorgid", SqlDbType.Int).Value = dt.Rows[i]["Vendorgid"].ToString();
                                    cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "insertdetails";
                                    a1 = cmdQry.ExecuteNonQuery();
                                    if (a1 != 1)
                                    {
                                        return "Not Inserted OBF Details";
                                    }

                                }
                                else
                                {
                                    return "Please Select Any OBF Details Line";
                                }


                            }


                        }
                    }
                    //int a3 = cmdQry.ExecuteNonQuery();



                }
                else
                {
                    return "Not Inserted";
                }
                return "Inserted Successfully";

            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }

        }

        public DataTable DtTable(CbfDetails prdtgid)
        {
            DataTable dt = new DataTable();
            try
            {
                getconnection();
             
                cmdQry = new SqlCommand("pr_fb_trn_prsummary", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@action", "cbfdetails");
                cmdQry.Parameters.AddWithValue("@prdetails_gid", prdtgid.prdetailsGid);
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return dt;
            }
            finally
            {
                objConn.Close();
            }
        }
        public DataTable DtTableOPex(CbfDetails prdtgid)
        {
            DataTable dt = new DataTable();
            try
            {
                getconnection();
               
                cmdQry = new SqlCommand("pr_fb_trn_topexprdetails", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@action", "cbfprdetails");
                cmdQry.Parameters.AddWithValue("@prdetails_gid", prdtgid.prdetailsGid);
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);
                return dt;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return dt;
            }
            finally
            {
                objConn.Close();
            }
        }

        public CbfSumEntity GetCbfDetails(DataTable dt)
        {
            CbfSumEntity obj = new CbfSumEntity();
            try
            {
                getconnection();
               
                CbfDetails obj_getcbfdetails;
                obj.cbfDetails = new List<CbfDetails>();
                foreach (DataRow rows in dt.Rows)
                {
                    obj_getcbfdetails = new CbfDetails();
                    if (dt.Columns.Contains("prheader_gid"))
                    {
                        if (rows["prheader_gid"].ToString() != "")
                            obj_getcbfdetails.prheaderGid = Convert.ToInt32(rows["prheader_gid"].ToString());
                        else
                            obj_getcbfdetails.prheaderGid = Convert.ToInt32(objCmnFunctions.GETprno(rows["prdetails_gid"].ToString()));
                    }
                    else
                    {
                        obj_getcbfdetails.prheaderGid = Convert.ToInt32(objCmnFunctions.GETprno(rows["prdetails_gid"].ToString()));
                    }
                    obj_getcbfdetails.prdetailsGid = Convert.ToInt32(rows["prdetails_gid"].ToString());
                    obj_getcbfdetails.productCode = rows["prodservice_code"].ToString();
                    obj_getcbfdetails.productName = rows["prodservice_name"].ToString();
                    obj_getcbfdetails.description = rows["prodservice_description"].ToString();
                    obj_getcbfdetails.uom = rows["uom_code"].ToString();
                    obj_getcbfdetails.qty = Convert.ToInt32(rows["prdetails_qty"].ToString());
                    obj_getcbfdetails.unitAmt = Convert.ToDecimal(rows["pipinput_rate"].ToString());
                    if (rows["assetcategory_glno"].ToString() != "")
                    {
                        obj_getcbfdetails.chartOfAccount = (rows["assetcategory_glno"].ToString());
                    }
                    else if (rows["expcat_gl_no"].ToString() != "")
                    {
                        obj_getcbfdetails.chartOfAccount = (rows["expcat_gl_no"].ToString());
                    }

                    obj_getcbfdetails.total = Convert.ToDecimal(rows["pipinput_costestimation"].ToString());
                    if (dt.Columns.Count > 13)
                    {
                        obj_getcbfdetails.remarks = (rows["remarks"].ToString());
                        if (rows["fccc"].ToString() == "")
                        {
                            obj_getcbfdetails.fccc = 0;
                        }
                        else
                        {
                            obj_getcbfdetails.fccc = Convert.ToInt32(rows["fccc"].ToString());
                        }

                        if (rows["budgetline"].ToString() == "")
                        {
                            obj_getcbfdetails.budgetLine = 0;
                        }
                        else
                        {
                            obj_getcbfdetails.budgetLine = Convert.ToInt32(rows["budgetline"].ToString());
                        }
                        if (dt.Columns.Contains("cbfdetails_gid"))
                        {
                            if (rows["cbfdetails_gid"].ToString() != "")
                                obj_getcbfdetails.cbfDetGid = Convert.ToInt32(rows["cbfdetails_gid"].ToString());
                        }


                    }
                    obj.cbfDetails.Add(obj_getcbfdetails);
                }
                return (obj);

            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }
            finally
            {
                objConn.Close();
            }
        }
        public CbfSumEntity GetCbfDetailsOpex(DataTable dt)
        {
            CbfSumEntity obj = new CbfSumEntity();
            try
            {
                getconnection();
               
                CbfDetails obj_getcbfdetails;
                obj.cbfDetails = new List<CbfDetails>();
                //if (dt.Rows.Count > 0)
                //{
                //    if (!dt.Columns.Contains("budgetline") && !dt.Columns.Contains("fccc"))
                //    {
                //        dt.Columns.Add("budgetline");
                //        dt.Columns.Add("fccc");
                //    }
                //}
                foreach (DataRow rows in dt.Rows)
                {
                    obj_getcbfdetails = new CbfDetails();
                    if (dt.Columns.Contains("prheader_gid"))
                    {
                        if (rows["prheader_gid"].ToString() != "")
                            obj_getcbfdetails.prheaderGid = Convert.ToInt32(rows["prheader_gid"].ToString());
                        else
                            obj_getcbfdetails.prheaderGid = Convert.ToInt32(objCmnFunctions.GETprno(rows["prdetails_gid"].ToString()));
                    }
                    else
                    {
                        obj_getcbfdetails.prheaderGid = Convert.ToInt32(objCmnFunctions.GETprno(rows["prdetails_gid"].ToString()));
                    }
                    obj_getcbfdetails.prdetailsGid = Convert.ToInt32(rows["prdetails_gid"].ToString());
                    obj_getcbfdetails.productCode = rows["prodservice_code"].ToString();
                    obj_getcbfdetails.qty = Convert.ToInt32(rows["prdetails_qty"].ToString());
                    obj_getcbfdetails.productName = rows["prodservice_name"].ToString();
                    obj_getcbfdetails.description = rows["prodservice_description"].ToString();
                    obj_getcbfdetails.unitAmt = Convert.ToDecimal(rows["pipinput_rate"].ToString());
                    obj_getcbfdetails.total = Convert.ToDecimal(rows["pipinput_costestimation"].ToString());
                    if (rows["assetcategory_glno"].ToString() != "")
                    {
                        obj_getcbfdetails.chartOfAccount = (rows["assetcategory_glno"].ToString());
                    }
                    else if (rows["expcat_gl_no"].ToString() != "")
                    {
                        obj_getcbfdetails.chartOfAccount = (rows["expcat_gl_no"].ToString());
                    }
                  
                    if (dt.Columns.Count > 14)
                    {
                        if (rows["budgetline"].ToString() == "")
                        {
                            obj_getcbfdetails.budgetLine = 0;
                        }
                        else
                        {
                            obj_getcbfdetails.budgetLine = Convert.ToInt32(rows["budgetline"].ToString());
                        }
                        if (rows["fccc"].ToString() == "")
                        {
                            obj_getcbfdetails.fccc = 0;
                        }
                        else
                        {
                            obj_getcbfdetails.fccc = Convert.ToInt32(rows["fccc"].ToString());
                        }

                        obj_getcbfdetails.vendorselection = (rows["Vendorname"].ToString());
                        if (rows["Vendorgid"].ToString() == null || rows["Vendorgid"].ToString() == "")
                            obj_getcbfdetails.vendorgid = 0;
                        else
                            obj_getcbfdetails.vendorgid = Convert.ToInt32(rows["Vendorgid"].ToString());
                    }
                    obj.cbfDetails.Add(obj_getcbfdetails);
                }
                return (obj);

            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }
            finally
            {
                objConn.Close();
            }
        }
        public CbfSumEntity CbfSave(DataTable dt, int i)
        {
            CbfSumEntity objsum = new CbfSumEntity();
            try
            {
                getconnection();
                if (dt.Columns.Count > 0)
                {
                    cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                    cmdQry.CommandType = CommandType.StoredProcedure;
                    cmdQry.Parameters.Add("@cbfdetails_cbfheader_gid", SqlDbType.Int).Value = dt.Rows[i]["myref"].ToString();
                    cmdQry.Parameters.Add("@cbfdetails_parprdesc_gid", SqlDbType.Int).Value = dt.Rows[i]["prdetails_gid"].ToString();
                    cmdQry.Parameters.Add("@cbfdetails_prodservice_gid", SqlDbType.Int).Value = dt.Rows[i]["prodservice_gid"].ToString();
                    cmdQry.Parameters.Add("@cbfdetails_parprdesc", SqlDbType.VarChar).Value = dt.Rows[i]["prodservice_description"].ToString();
                    cmdQry.Parameters.Add("@cbfdetails_desc", SqlDbType.VarChar).Value = dt.Rows[i]["remarks"].ToString();
                    cmdQry.Parameters.Add("@cbfdetails_uom_gid", SqlDbType.Int).Value = dt.Rows[i]["uom_gid"].ToString();
                    cmdQry.Parameters.Add("@cbfdetails_qty", SqlDbType.Int).Value = dt.Rows[i]["prdetails_qty"].ToString();
                    cmdQry.Parameters.Add("@cbfdetails_unitprice", SqlDbType.Decimal).Value = dt.Rows[i]["pipinput_rate"].ToString();
                    cmdQry.Parameters.Add("@cbfdetails_totalamt", SqlDbType.Decimal).Value = dt.Rows[i]["pipinput_costestimation"].ToString();
                    cmdQry.Parameters.Add("@cbfdetails_remarks", SqlDbType.VarChar).Value = dt.Rows[i]["remarks"].ToString();
                    if (dt.Rows[i]["assetcategory_glno"].ToString() != "")
                    {
                        cmdQry.Parameters.Add("@cbfdetails_chartofacc", SqlDbType.VarChar).Value = dt.Rows[i]["assetcategory_glno"].ToString();
                    }
                    else
                    {
                        cmdQry.Parameters.Add("@cbfdetails_chartofacc", SqlDbType.VarChar).Value = dt.Rows[i]["expcat_gl_no"].ToString();
                    }
                    cmdQry.Parameters.Add("@cbfdetails_FCCC", SqlDbType.Int).Value = dt.Rows[i]["fccc"].ToString();
                    cmdQry.Parameters.Add("@cbfdetails_budgetline", SqlDbType.Int).Value = dt.Rows[i]["budgetline"].ToString();
                    cmdQry.Parameters.Add("@cbfdetails_prpardel_gid", SqlDbType.Int).Value = dt.Rows[i]["prdetails_gid"].ToString();
                    cmdQry.Parameters.Add("@cbfheader_cbfobf_flag", SqlDbType.VarChar).Value = "C";
                    cmdQry.Parameters.Add("@cbfheader_status", SqlDbType.VarChar).Value = "1";
                    cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "savecbfdetails";
                    int a1 = cmdQry.ExecuteNonQuery();
                }
                getconnection();
                CbfRaiseHeader getvalues = new CbfRaiseHeader();
                DataTable dtCbfHeader = new DataTable();
                CbfDetails getdetailsvalue;
                objsum.cbfDetails = new List<CbfDetails>();
                objsum.cbfPrheader = new List<CbfPrHeader>();
                objsum.cbfPrdetarils = new List<CbfPrDetails>();

                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.BigInt).Value = HttpContext.Current.Session["cbfgid"];
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectcbfedit";
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dtCbfHeader);
                for (int i1 = 0; i1 < dtCbfHeader.Rows.Count; i1++)
                {
                    getdetailsvalue = new CbfDetails()
                    {
                        productCode = dtCbfHeader.Rows[i1]["prodservice_code"].ToString(),
                        cbfDetGid = Convert.ToInt32(dtCbfHeader.Rows[i1]["cbfdetails_gid"].ToString()),
                        productName = dtCbfHeader.Rows[i1]["prodservice_name"].ToString(),
                        description = dtCbfHeader.Rows[i1]["prodservice_description"].ToString(),
                        description1 = dtCbfHeader.Rows[i1]["cbfdetails_desc"].ToString(),
                        uom = dtCbfHeader.Rows[i1]["uom_code"].ToString(),
                        qty = Convert.ToInt32(dtCbfHeader.Rows[i1]["cbfdetails_qty"].ToString()),
                        unitAmt = Convert.ToDecimal(dtCbfHeader.Rows[i1]["cbfdetails_unitprice"].ToString()),
                        total = Convert.ToDecimal(dtCbfHeader.Rows[i1]["cbfdetails_totalamt"].ToString()),
                        remarks = dtCbfHeader.Rows[i1]["cbfdetails_remarks"].ToString(),
                        chartOfAccount = dtCbfHeader.Rows[i1]["cbfdetails_chartofacc"].ToString(),
                        fccc = Convert.ToInt32(dtCbfHeader.Rows[i1]["cbfdetails_FCCC"].ToString()),
                        budgetLine = Convert.ToInt32(dtCbfHeader.Rows[i1]["cbfdetails_budgetline"].ToString()),
                        prdetailsGid = Convert.ToInt64(dtCbfHeader.Rows[i1]["cbfdetails_prpardel_gid"].ToString()),
                        attch_Gid = Convert.ToInt16(i1 + 1),
                    };

                    objsum.cbfDetails.Add(getdetailsvalue);
                }


                return objsum;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objsum;
            }
            finally
            {
                objConn.Close();
            }


        }

        public CbfSumEntity GetCbfParDetails(DataTable dt)
        {
            CbfSumEntity obj = new CbfSumEntity();
            try
            {
              
                CbfDetails obj_getcbfpardetails;
                obj.cbfDetails = new List<CbfDetails>();
                foreach (DataRow rows in dt.Rows)
                {
                    obj_getcbfpardetails = new CbfDetails();
                    if (dt.Columns.Contains("sno"))
                    {
                        if (rows["sno"].ToString() != "" && rows["sno"].ToString() != null)
                        {

                            obj_getcbfpardetails.attch_Gid = Convert.ToInt32(rows["sno"].ToString());

                            obj_getcbfpardetails.prdetailsGid = Convert.ToInt32(rows["prdetails_gid"].ToString());
                            obj_getcbfpardetails.productGroupId = Convert.ToInt32(rows["productgroup"].ToString());
                            obj_getcbfpardetails.productCode = rows["productcode"].ToString();
                            obj_getcbfpardetails.productName = rows["productname"].ToString();
                            obj_getcbfpardetails.description = rows["productdescription"].ToString();
                            obj_getcbfpardetails.uomGid = Convert.ToInt32(rows["uom"].ToString());
                            obj_getcbfpardetails.qty = Convert.ToInt32(rows["qty"].ToString());
                            obj_getcbfpardetails.unitAmt = Convert.ToDecimal(rows["unitamount"].ToString());
                            obj_getcbfpardetails.chartOfAccount = (rows["chartofaccount"].ToString());
                            obj_getcbfpardetails.total = Convert.ToDecimal(rows["total"].ToString());
                            obj_getcbfpardetails.remarks = (rows["remarks"].ToString());
                            if (rows["fccc"].ToString() == "")
                            {
                                obj_getcbfpardetails.fccc = 0;
                            }
                            else
                            {
                                obj_getcbfpardetails.fccc = Convert.ToInt32(rows["fccc"].ToString());
                            }

                            if (rows["budgetline"].ToString() == "")
                            {
                                obj_getcbfpardetails.budgetLine = 0;
                            }
                            else
                            {

                                obj_getcbfpardetails.budgetLine = Convert.ToInt32(rows["budgetline"].ToString());
                            }

                            //obj_getcbfpardetails.att_identify = (rows["BOQ"].ToString());
                        }
                        else
                        {
                            obj_getcbfpardetails.productCode = rows["prodservice_code"].ToString();
                            obj_getcbfpardetails.cbfDetGid = Convert.ToInt32(rows["cbfdetails_gid"].ToString());
                            obj_getcbfpardetails.productName = rows["prodservice_name"].ToString();
                            obj_getcbfpardetails.description = rows["prodservice_description"].ToString();
                            obj_getcbfpardetails.description1 = rows["cbfdetails_desc"].ToString();

                            obj_getcbfpardetails.qty = Convert.ToInt32(rows["prdetails_qty"].ToString());
                            obj_getcbfpardetails.unitAmt = Convert.ToDecimal(rows["pipinput_rate"].ToString());
                            obj_getcbfpardetails.total = Convert.ToDecimal(rows["pipinput_costestimation"].ToString());
                            obj_getcbfpardetails.remarks = rows["remarks"].ToString();
                            obj_getcbfpardetails.chartOfAccount = rows["assetcategory_glno"].ToString();
                            obj_getcbfpardetails.fccc = Convert.ToInt32(rows["fccc"].ToString());
                            obj_getcbfpardetails.budgetLine = Convert.ToInt32(rows["budgetline"].ToString());
                            obj_getcbfpardetails.prdetailsGid = Convert.ToInt64(rows["prdetails_gid"].ToString());

                            obj_getcbfpardetails.productGroupId = Convert.ToInt32(rows["prodservice_gid"].ToString());
                            obj_getcbfpardetails.uomGid = Convert.ToInt32(rows["uom_gid"].ToString());
                            obj_getcbfpardetails.attch_Gid = Convert.ToInt32(rows["cbfdetails_gid"].ToString());
                            //obj_getcbfpardetails.att_identify = rows["BOQ"].ToString();
                        }
                    }
                    else
                    {
                        obj_getcbfpardetails.productCode = rows["prodservice_code"].ToString();
                        obj_getcbfpardetails.cbfDetGid = Convert.ToInt32(rows["cbfdetails_gid"].ToString());
                        obj_getcbfpardetails.productName = rows["prodservice_name"].ToString();
                        obj_getcbfpardetails.description = rows["prodservice_description"].ToString();
                        obj_getcbfpardetails.description1 = rows["cbfdetails_desc"].ToString();

                        obj_getcbfpardetails.qty = Convert.ToInt32(rows["prdetails_qty"].ToString());
                        obj_getcbfpardetails.unitAmt = Convert.ToDecimal(rows["pipinput_rate"].ToString());
                        obj_getcbfpardetails.total = Convert.ToDecimal(rows["pipinput_costestimation"].ToString());
                        obj_getcbfpardetails.remarks = rows["remarks"].ToString();
                        obj_getcbfpardetails.chartOfAccount = rows["assetcategory_glno"].ToString();
                        obj_getcbfpardetails.fccc = Convert.ToInt32(rows["fccc"].ToString());
                        obj_getcbfpardetails.budgetLine = Convert.ToInt32(rows["budgetline"].ToString());
                        obj_getcbfpardetails.prdetailsGid = Convert.ToInt64(rows["prdetails_gid"].ToString());

                        obj_getcbfpardetails.productGroupId = Convert.ToInt32(rows["prodservice_gid"].ToString());
                        obj_getcbfpardetails.uomGid = Convert.ToInt32(rows["uom_gid"].ToString());
                        obj_getcbfpardetails.attch_Gid = Convert.ToInt32(rows["cbfdetails_gid"].ToString());
                        //obj_getcbfpardetails.att_identify = rows["BOQ"].ToString();
                    }

                    obj.cbfDetails.Add(obj_getcbfpardetails);
                }

                return (obj);
            }


            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }
            finally
            {
                objConn.Close();
            }
        }

        public CbfSumEntity GetCbfParDetailsOpex(DataTable dt)
        {
            CbfSumEntity obj = new CbfSumEntity();
            try
            {
            
                CbfDetails obj_getcbfpardetails;
                obj.cbfDetails = new List<CbfDetails>();
                foreach (DataRow rows in dt.Rows)
                {
                    obj_getcbfpardetails = new CbfDetails();
                    if (dt.Columns.Contains("sno"))
                    {
                        if (rows["sno"].ToString() != "" && rows["sno"].ToString() != null)
                        {

                            obj_getcbfpardetails.attch_Gid = Convert.ToInt32(rows["sno"].ToString());

                            obj_getcbfpardetails.prdetailsGid = Convert.ToInt32(rows["prdetails_gid"].ToString());
                            obj_getcbfpardetails.productGroupId = Convert.ToInt32(rows["productgroup"].ToString());
                            obj_getcbfpardetails.productCode = rows["productcode"].ToString();
                            obj_getcbfpardetails.productName = rows["productname"].ToString();
                            obj_getcbfpardetails.description = rows["productdescription"].ToString();
                            obj_getcbfpardetails.qty = Convert.ToInt32(rows["qty"].ToString());
                            obj_getcbfpardetails.unitAmt = Convert.ToDecimal(rows["unitamount"].ToString());
                            obj_getcbfpardetails.total = Convert.ToDecimal(rows["total"].ToString());
                            obj_getcbfpardetails.vendorgid = Convert.ToInt32(rows["vendorgid"].ToString());
                            obj_getcbfpardetails.vendorselection = rows["Vendorname"].ToString();

                        }

                        obj.cbfDetails.Add(obj_getcbfpardetails);
                    }


                }
                return (obj);
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }
            finally
            {
                objConn.Close();
            }
        }


        public CbfSumEntity GetFccc()
        {
            CbfSumEntity obj = new CbfSumEntity();
            try
            {
              
                FcccMaster obj_getvalues;
                obj.searchFccc = new List<FcccMaster>();
                getconnection();

                DataTable dt = new DataTable();
                cmdQry = new SqlCommand("pr_fb_mst_tfccc", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;

                cmdQry.Parameters.AddWithValue("@action", "selectforficc");
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj_getvalues = new FcccMaster()
                    {
                        fcccCode = row["fccc_gid"].ToString(),
                        fcccGid = row["fccc_code"].ToString(),
                        fcccName = row["fccc_name"].ToString(),

                    };
                    obj.searchFccc.Add(obj_getvalues);
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
                objConn.Close();
            }
        }

        public CbfSumEntity GetFcccSerach(FcccMaster fc)
        {
            CbfSumEntity obj = new CbfSumEntity();
            try
            {
               
                FcccMaster obj_getvalues;
                obj.searchFccc = new List<FcccMaster>();
                getconnection();

                DataTable dt = new DataTable();
                cmdQry = new SqlCommand("pr_fb_mst_tfccc", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@fccccode", fc.fcccGid);
                cmdQry.Parameters.AddWithValue("@fccc_name", fc.fcccName);
                cmdQry.Parameters.AddWithValue("@action", "Search");
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj_getvalues = new FcccMaster()
                    {
                        fcccCode = row["fccc_gid"].ToString(),
                        fcccGid = row["fccc_code"].ToString(),
                        fcccName = row["fccc_name"].ToString(),
                    };
                    obj.searchFccc.Add(obj_getvalues);
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
                objConn.Close();
            }
        }

        public string GetFcccSerachText(string lsFccc)
        {
            try
            {

                CbfSumEntity obj = new CbfSumEntity();
                FcccMaster obj_getvalues;
                obj.searchFccc = new List<FcccMaster>();
                getconnection();
                string fccccode = "";
                DataTable dt = new DataTable();
                cmdQry = new SqlCommand("pr_fb_mst_tfccc", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@fccccode", lsFccc);
                cmdQry.Parameters.AddWithValue("@action", "change");
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    fccccode = dt.Rows[0]["fccc_code"].ToString();
                }
                return fccccode;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
        }


        public string GeneartePrRefno(int id, string prefix)
        {
            try
            {
                getconnection();
                CbfRaiseHeader prsument = new CbfRaiseHeader();
                cmdQry = new SqlCommand("pr_Generate_Refno", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@prheader_requestgid", SqlDbType.BigInt).Value = id;
                cmdQry.Parameters.Add("@prefix", SqlDbType.VarChar).Value = prefix;
                cmdQry.Parameters.Add("@prheader_refno", SqlDbType.VarChar, 30);
                cmdQry.Parameters["@prheader_refno"].Direction = ParameterDirection.Output;
                cmdQry.ExecuteReader();
                string lsrefno = cmdQry.Parameters["@prheader_refno"].Value.ToString();
                return lsrefno;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
        }
        public CbfSumEntity AttachmentnameInline(CbfSumEntity filename)
        {
            CbfSumEntity obj = new CbfSumEntity();
            try
            {
                int count = 0;
                int count1 = 0;
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
               
                Attachment obj_getprdetails;


                if (HttpContext.Current.Session["countinline"] != null)
                {
                    count = (int)HttpContext.Current.Session["countinline"];
                }
                if (HttpContext.Current.Session["count1"] != null)
                {
                    count1 = (int)HttpContext.Current.Session["count1"];
                }

                if (filename.amoun == "IEM.Areas.FLEXIBUY.Models.CbfSumEntity" || filename.amoun == "Attach_1")
                {
                    filename.amoun = "";
                }
                if (filename.amoun != "IEM.Areas.FLEXIBUY.Models.CbfSumEntity" || filename.amoun != "" || filename.amoun != null)
                {

                    filename.attachment = Getattachment(filename.amoun, (string)HttpContext.Current.Session["inline"], "2");
                }
                if (filename.attachment != null)
                {
                    if (filename.attachment.Count > 0)
                    {
                        dt1.Columns.Add("Sno", typeof(string));
                        dt1.Columns.Add("Documnet_Name", typeof(string));
                        dt1.Columns.Add("Attachment_Date", typeof(string));
                        dt1.Columns.Add("Document_des", typeof(string));
                        for (int i = 0; i < filename.attachment.Count(); i++)
                        {
                            var row = dt1.NewRow();
                            row["Sno"] = i + 1;
                            row["Documnet_Name"] = filename.attachment[i].fileName;
                            row["Attachment_Date"] = filename.attachment[i].attachedDate;
                            row["Document_des"] = filename.attachment[i].description;
                            dt1.Rows.Add(row);
                        }
                    }
                }
                obj.attachment = new List<Attachment>();
                if (filename.amoun == null || filename.amoun == "")
                {
                    if (HttpContext.Current.Session["AccessTokeninline"] != ""
                         && HttpContext.Current.Session["AccessTokeninline"] == null && HttpContext.Current.Session["viewfor"] != null)
                    {
                        HttpContext.Current.Session["AccessTokeninline"] = HttpContext.Current.Session["cbfAttachment"];
                    }
                    if (HttpContext.Current.Session["AccessTokeninline"] != ""
                        && HttpContext.Current.Session["AccessTokeninline"] != null)
                    {
                        dt = (DataTable)HttpContext.Current.Session["AccessTokeninline"];
                        if (dt1.Rows.Count > 0)
                        {
                            dt = dt1;
                        }
                    }
                    else
                    {
                        dt.Columns.Add("Sno", typeof(string));
                        dt.Columns.Add("Documnet_Name", typeof(string));
                        dt.Columns.Add("Attachment_Date", typeof(string));
                        dt.Columns.Add("Document_des", typeof(string));

                    }
                    if (filename != null)
                    {
                        if ((filename.attachmentDate != "" && filename.attachment1 != "" && filename.des != "")
                            && (filename.attachmentDate != null && filename.attachment1 != null && filename.des != null))
                        {
                            HttpContext.Current.Session["countinline"] = count + 1;
                            var row = dt.NewRow();
                            row["Sno"] = HttpContext.Current.Session["countinline"];
                            row["Documnet_Name"] = filename.attachment1;
                            row["Attachment_Date"] = filename.attachmentDate.ToString();
                            row["Document_des"] = filename.des;
                            if (dt2.Columns.Contains("cbfheader_gid"))
                                row["cbfheader_gid"] = "1";
                            dt.Rows.Add(row);

                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            obj_getprdetails = new Attachment()
                            {
                                attachGid = dt.Rows[i]["Sno"].ToString(),
                                attachedDate = dt.Rows[i]["Attachment_Date"].ToString(),
                                fileName = dt.Rows[i]["Documnet_Name"].ToString(),
                                description = dt.Rows[i]["Document_des"].ToString(),
                                employee_gid = dt.Rows[i]["Document_des"].ToString(),

                            };
                            obj.attachment.Add(obj_getprdetails);
                        }
                    }
                    System.Web.HttpContext.Current.Session["AccessTokeninline"] = dt;
                }

                else
                {
                    if (HttpContext.Current.Session["AccessTokenheaderinline"] == ""
                           || HttpContext.Current.Session["AccessTokenheaderinline"] == null && HttpContext.Current.Session["viewfor"] != null)
                    {
                        HttpContext.Current.Session["AccessTokenheaderinline"] = HttpContext.Current.Session["cbfAttachmentDetailsinline"];
                    }
                    if (HttpContext.Current.Session["AccessTokenheaderinline"] != ""
                        && HttpContext.Current.Session["AccessTokenheaderinline"] != null)
                    {
                        dt2 = (DataTable)HttpContext.Current.Session["AccessTokenheaderinline"];
                    }
                    else
                    {
                        dt2.Columns.Add("Sno", typeof(string));
                        dt2.Columns.Add("prdetails", typeof(int));
                        dt2.Columns.Add("Documnet_Name", typeof(string));
                        dt2.Columns.Add("Attachment_Date", typeof(string));
                        dt2.Columns.Add("Document_des", typeof(string));
                        dt2.Columns.Add("cbfdetails_gid", typeof(string));
                    }
                    if ((filename.attachmentDate != "" && filename.attachment1 != "" && filename.des != "")
                        && (filename.attachmentDate != null && filename.attachment1 != null && filename.des != null))
                    {
                        string number = filename.amoun;
                        string[] numberpar = number.Split('_');
                        if (numberpar.Length > 1)
                        {
                            if (HttpContext.Current.Session["sno"] != null)
                            {
                                int sno = Convert.ToInt32(HttpContext.Current.Session["sno"]) + 1;
                                filename.amoun = sno.ToString();
                                HttpContext.Current.Session["sno"] = sno;
                            }
                            else
                            {
                                int sno = Convert.ToInt32(HttpContext.Current.Session["sno"]) + 1;
                                filename.amoun = sno.ToString();
                            }
                        }
                        HttpContext.Current.Session["count1inline"] = count1 + 1;
                        var row = dt2.NewRow();
                        row["Sno"] = count1 + 1;
                        row["prdetails"] = filename.amoun;
                        row["Documnet_Name"] = filename.attachment1;
                        row["Attachment_Date"] = filename.attachmentDate.ToString();
                        row["Document_des"] = filename.des;
                        if (dt2.Columns.Contains("cbfdetails_gid"))
                            row["cbfdetails_gid"] = filename.selectedValue;
                        // row["cbfpar"] = filename.selectedValue;
                        dt2.Rows.Add(row);
                    }
                    if (dt1.Rows.Count > 0)
                    {
                        string number = filename.amoun;
                        string[] numberpar = number.Split('_');
                        if (numberpar.Length > 1)
                        {
                            filename.amoun = numberpar[1].ToString();
                        }


                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            obj_getprdetails = new Attachment()
                            {
                                attachGid = dt1.Rows[i]["Sno"].ToString(),
                                attachedDate = dt1.Rows[i]["Attachment_Date"].ToString(),
                                fileName = dt1.Rows[i]["Documnet_Name"].ToString(),
                                description = dt1.Rows[i]["Document_des"].ToString(),
                            };

                            obj.attachment.Add(obj_getprdetails);
                        }
                        dt2.Clear();
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        string number = filename.amoun;
                        string[] numberpar = number.Split('_');
                        if (numberpar.Length > 1)
                        {
                            filename.amoun = numberpar[1].ToString();
                        }
                        DataView dv = new DataView();
                        dt2.DefaultView.RowFilter = "prdetails = " + filename.amoun + "";
                        dv = dt2.DefaultView;
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            obj_getprdetails = new Attachment()
                            {
                                attachGid = dt2.Rows[i]["Sno"].ToString(),
                                attachedDate = dt2.Rows[i]["Attachment_Date"].ToString(),
                                fileName = dt2.Rows[i]["Documnet_Name"].ToString(),
                                description = dt2.Rows[i]["Document_des"].ToString(),
                            };

                            obj.attachment.Add(obj_getprdetails);
                        }
                        dt1.Clear();
                    }
                    System.Web.HttpContext.Current.Session["AccessTokenheaderinline"] = dt2;
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
                objConn.Close();
            }
        }
        public CbfSumEntity Attachmentname(CbfSumEntity filename)
        {
            CbfSumEntity obj = new CbfSumEntity();
            try
            {
                int count = 0;
                int count1 = 0;
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
              
                Attachment obj_getprdetails;
                if (HttpContext.Current.Session["count"] != null)
                {
                    count = (int)HttpContext.Current.Session["count"];
                }
                if (HttpContext.Current.Session["count1"] != null)
                {
                    count1 = (int)HttpContext.Current.Session["count1"];
                }

                if (filename.amoun == "IEM.Areas.FLEXIBUY.Models.CbfSumEntity" || filename.amoun == "Attach_1")
                {
                    filename.amoun = "";
                }
                if (filename.amoun != "IEM.Areas.FLEXIBUY.Models.CbfSumEntity" || filename.amoun != "" || filename.amoun != null)
                {

                    filename.attachment = Getattachment(filename.amoun, (string)HttpContext.Current.Session["mode"], "3");
                }
                if (filename.attachment != null)
                {
                    if (filename.attachment.Count > 0)
                    {
                        dt1.Columns.Add("Sno", typeof(string));
                        dt1.Columns.Add("Documnet_Name", typeof(string));
                        dt1.Columns.Add("Attachment_Date", typeof(string));
                        dt1.Columns.Add("Document_des", typeof(string));
                        for (int i = 0; i < filename.attachment.Count(); i++)
                        {
                            var row = dt1.NewRow();
                            row["Sno"] = i + 1;
                            row["Documnet_Name"] = filename.attachment[i].fileName;
                            row["Attachment_Date"] = filename.attachment[i].attachedDate;
                            row["Document_des"] = filename.attachment[i].description;
                            dt1.Rows.Add(row);
                        }
                        HttpContext.Current.Session["attachmentdetails"] = "1";
                    }
                }
                obj.attachment = new List<Attachment>();
                if (filename.amoun == null || filename.amoun == "")
                {
                    if (HttpContext.Current.Session["AccessToken"] != ""
                         && HttpContext.Current.Session["AccessToken"] == null && HttpContext.Current.Session["viewfor"] != null)
                    {
                        HttpContext.Current.Session["AccessToken"] = HttpContext.Current.Session["cbfAttachment"];
                    }
                    if (HttpContext.Current.Session["AccessToken"] != ""
                        && HttpContext.Current.Session["AccessToken"] != null)
                    {
                        dt = (DataTable)HttpContext.Current.Session["AccessToken"];
                        if (dt1.Rows.Count > 0)
                        {
                            dt = dt1;
                        }
                    }
                    else
                    {
                        dt.Columns.Add("Sno", typeof(string));
                        dt.Columns.Add("Documnet_Name", typeof(string));
                        dt.Columns.Add("Attachment_Date", typeof(string));
                        dt.Columns.Add("Document_des", typeof(string));

                    }
                    if (filename != null)
                    {
                        if ((filename.attachmentDate != "" && filename.attachment1 != "" && filename.des != "")
                            && (filename.attachmentDate != null && filename.attachment1 != null && filename.des != null))
                        {
                            HttpContext.Current.Session["count"] = count + 1;
                            var row = dt.NewRow();
                            row["Sno"] = HttpContext.Current.Session["count"];
                            row["Documnet_Name"] = filename.attachment1;
                            row["Attachment_Date"] = filename.attachmentDate.ToString();
                            row["Document_des"] = filename.des;
                            if (dt2.Columns.Contains("cbfheader_gid"))
                                row["cbfheader_gid"] = "1";
                            dt.Rows.Add(row);

                        }
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            obj_getprdetails = new Attachment()
                            {
                                attachGid = dt.Rows[i]["Sno"].ToString(),
                                attachedDate = dt.Rows[i]["Attachment_Date"].ToString(),
                                fileName = dt.Rows[i]["Documnet_Name"].ToString(),
                                description = dt.Rows[i]["Document_des"].ToString(),
                                employee_gid = dt.Rows[i]["Document_des"].ToString(),

                            };
                            obj.attachment.Add(obj_getprdetails);
                        }
                    }
                    System.Web.HttpContext.Current.Session["AccessToken"] = dt;
                }

                else
                {
                    if (HttpContext.Current.Session["AccessTokenheader"] == ""
                           || HttpContext.Current.Session["AccessTokenheader"] == null && HttpContext.Current.Session["viewfor"] != null)
                    {
                        HttpContext.Current.Session["AccessTokenheader"] = HttpContext.Current.Session["cbfAttachmentDetails"];
                    }
                    if (HttpContext.Current.Session["AccessTokenheader"] != ""
                        && HttpContext.Current.Session["AccessTokenheader"] != null)
                    {
                        dt2 = (DataTable)HttpContext.Current.Session["AccessTokenheader"];
                    }
                    else
                    {
                        dt2.Columns.Add("Sno", typeof(string));
                        dt2.Columns.Add("prdetails", typeof(int));
                        dt2.Columns.Add("Documnet_Name", typeof(string));
                        dt2.Columns.Add("Attachment_Date", typeof(string));
                        dt2.Columns.Add("Document_des", typeof(string));
                        dt2.Columns.Add("cbfdetails_gid", typeof(string));
                    }
                    if ((filename.attachmentDate != "" && filename.attachment1 != "" && filename.des != "")
                        && (filename.attachmentDate != null && filename.attachment1 != null && filename.des != null))
                    {
                        string number = filename.amoun;
                        string[] numberpar = number.Split('_');
                        if (numberpar.Length > 1)
                        {
                            if (HttpContext.Current.Session["sno"] != null)
                            {
                                int sno = Convert.ToInt32(HttpContext.Current.Session["sno"]) + 1;
                                filename.amoun = sno.ToString();
                                HttpContext.Current.Session["sno"] = sno;
                            }
                            else
                            {
                                int sno = Convert.ToInt32(HttpContext.Current.Session["sno"]) + 1;
                                filename.amoun = sno.ToString();
                            }
                        }
                        HttpContext.Current.Session["count1"] = count1 + 1;
                        var row = dt2.NewRow();
                        row["Sno"] = count1 + 1;
                        row["prdetails"] = filename.amoun;
                        row["Documnet_Name"] = filename.attachment1;
                        row["Attachment_Date"] = filename.attachmentDate.ToString();
                        row["Document_des"] = filename.des;
                        if (dt2.Columns.Contains("cbfdetails_gid"))
                            row["cbfdetails_gid"] = Convert.ToString(filename.selectedValue);
                        // row["cbfpar"] = filename.selectedValue;
                        dt2.Rows.Add(row);
                    }

                    if (dt1.Rows.Count > 0)
                    {
                        string number = filename.amoun;
                        string[] numberpar = number.Split('_');
                        if (numberpar.Length > 1)
                        {
                            filename.amoun = numberpar[1].ToString();
                        }


                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            obj_getprdetails = new Attachment()
                            {
                                attachGid = dt1.Rows[i]["Sno"].ToString(),
                                attachedDate = dt1.Rows[i]["Attachment_Date"].ToString(),
                                fileName = dt1.Rows[i]["Documnet_Name"].ToString(),
                                description = dt1.Rows[i]["Document_des"].ToString(),
                            };

                            obj.attachment.Add(obj_getprdetails);
                        }
                        dt2.Clear();
                    }
                    if (dt2.Rows.Count > 0)
                    {
                        string number = filename.amoun;
                        string[] numberpar = number.Split('_');
                        if (numberpar.Length > 1)
                        {
                            filename.amoun = numberpar[1].ToString();
                        }
                        DataView dv = new DataView();
                        dt2.DefaultView.RowFilter = "prdetails = " + filename.amoun + "";
                        dv = dt2.DefaultView;
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            obj_getprdetails = new Attachment()
                            {
                                attachGid = dt2.Rows[i]["Sno"].ToString(),
                                attachedDate = dt2.Rows[i]["Attachment_Date"].ToString(),
                                fileName = dt2.Rows[i]["Documnet_Name"].ToString(),
                                description = dt2.Rows[i]["Document_des"].ToString(),
                            };

                            obj.attachment.Add(obj_getprdetails);
                        }
                    }
                    System.Web.HttpContext.Current.Session["AccessTokenheader"] = dt2;
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
                objConn.Close();
            }
        }

        public CbfSumEntity AttachmnetDetails(CbfSumEntity filename)
        {
            CbfSumEntity objsum = new CbfSumEntity();
            try
            {
                getconnection();
              
                objsum.attachment = new List<Attachment>();
                objsum.attachmentCbfdetails = new List<Attachment>();
                DataSet dsCbfdetatails = new DataSet();
                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.BigInt).Value = HttpContext.Current.Session["cbfgid"].ToString();
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "boqattachmentfor";
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dsCbfdetatails);
                if (dsCbfdetatails.Tables[0].Rows.Count > 0 || dsCbfdetatails.Tables[0].Rows.Count != null)
                {
                    for (int i = 0; i < dsCbfdetatails.Tables[0].Rows.Count; i++)
                    {
                        Attachment attachment1 = new Attachment()
                        {
                            fileName = dsCbfdetatails.Tables[0].Rows[i]["attachment_filename"].ToString(),
                            attachedDate = dsCbfdetatails.Tables[0].Rows[i]["attachment_date"].ToString(),
                            description = dsCbfdetatails.Tables[0].Rows[i]["attachment_desc"].ToString(),
                            attachGid = dsCbfdetatails.Tables[0].Rows[i]["attachment_gid"].ToString(),
                        };
                        objsum.attachment.Add(attachment1);
                    }
                }
                if (HttpContext.Current.Session["cbfdetails"].ToString() != "")
                {
                    if (dsCbfdetatails.Tables[1].Rows.Count > 0 && dsCbfdetatails.Tables[1].Rows.Count != null)
                    {
                        DataView ObjDv = new DataView();

                        dsCbfdetatails.Tables[1].DefaultView.RowFilter = "attachment_ref_gid=" + HttpContext.Current.Session["cbfdetails"].ToString() + "";
                        ObjDv = dsCbfdetatails.Tables[1].DefaultView;
                        if (ObjDv.Count > 0)
                        {
                            for (int i = 0; i < ObjDv.Count; i++)
                            {
                                Attachment attachment1 = new Attachment()
                                {
                                    fileName = ObjDv[i]["attachment_filename"].ToString(),
                                    attachedDate = ObjDv[i]["attachment_date"].ToString(),
                                    description = ObjDv[i]["attachment_desc"].ToString(),
                                    attachGid = ObjDv[i]["attachment_gid"].ToString(),
                                };
                                objsum.attachmentCbfdetails.Add(attachment1);
                            }
                        }
                    }
                }
                //  HttpContext.Current.Session["cbfdeailsedit"] = objsum;
                return objsum;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objsum;
            }
            finally
            {
                objConn.Close();
            }
        }

        public CbfSumEntity MyAttachmnetCbfDetails(string CbfDeeatailsId)
        {
            CbfSumEntity objsum = new CbfSumEntity();
            try
            {
                getconnection();
                
                objsum.attachment = new List<Attachment>();
                objsum.attachmentCbfdetails = new List<Attachment>();
                DataSet dsCbfdetatails = new DataSet();
                SqlCommand cmd = new SqlCommand("select * from iem_trn_tattachment where attachment_ref_gid = " + CbfDeeatailsId, objConn);
                cmd.CommandType = CommandType.Text;
                DataTable dtcbfattach = new DataTable();
                objDataAdapter = new SqlDataAdapter(cmd);
                objDataAdapter.Fill(dsCbfdetatails);
                if (dsCbfdetatails.Tables[0].Rows.Count > 0 || dsCbfdetatails.Tables[0].Rows.Count != null)
                {
                    for (int i = 0; i < dsCbfdetatails.Tables[0].Rows.Count; i++)
                    {
                        Attachment attachment1 = new Attachment()
                        {
                            fileName = dsCbfdetatails.Tables[0].Rows[i]["attachment_filename"].ToString(),
                            attachedDate = dsCbfdetatails.Tables[0].Rows[i]["attachment_date"].ToString(),
                            description = dsCbfdetatails.Tables[0].Rows[i]["attachment_desc"].ToString(),
                            attachGid = dsCbfdetatails.Tables[0].Rows[i]["attachment_gid"].ToString(),
                        };
                        objsum.attachment.Add(attachment1);
                    }
                }

                return objsum;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objsum;
            }
            finally
            {
                objConn.Close();
            }
        }

        public CbfSumEntity parattachment(int lnPardetails_gid)
        {
            CbfSumEntity objsum = new CbfSumEntity();
            try
            {
                getconnection();
             
                Attachment obj_getprdetails;
                objsum.attachment = new List<Attachment>();
                SqlCommand cmd = new SqlCommand("select 0 + ROW_NUMBER() OVER (ORDER BY attachment_gid) AS Sno,attachment_gid,attachment_filename,attachment_desc,attachment_date from iem_trn_tattachment where attachment_ref_gid='" + lnPardetails_gid + "' and attachment_ref_flag=12", objConn);
                cmd.CommandType = CommandType.Text;
                DataTable dtcbfattach = new DataTable();
                objDataAdapter = new SqlDataAdapter(cmd);
                objDataAdapter.Fill(dtcbfattach);
                if (dtcbfattach.Rows.Count > 0)
                {
                    for (int i = 0; i < dtcbfattach.Rows.Count; i++)
                    {
                        obj_getprdetails = new Attachment();
                        obj_getprdetails.fileName = dtcbfattach.Rows[i]["attachment_filename"].ToString();
                        obj_getprdetails.attachedDate = dtcbfattach.Rows[i]["attachment_date"].ToString();
                        obj_getprdetails.description = dtcbfattach.Rows[i]["attachment_desc"].ToString();
                        obj_getprdetails.attachGid = dtcbfattach.Rows[i]["Sno"].ToString();

                        objsum.attachment.Add(obj_getprdetails);
                    }
                }
                return objsum;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objsum;
            }
            finally
            {
                objConn.Close();
            }
        }
        public CbfSumEntity CbfPardetails(CbfParHeader par)
        {
            CbfSumEntity obj = new CbfSumEntity();
            try
            {
                getconnection();
             
                CbfParDetails getpardetails;
                obj.cbfPardetailslst = new List<CbfParDetails>();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_fb_trn_prsummary", objConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@parheader_gid", par.parGid);
                cmd.Parameters.AddWithValue("@prheader_requestforgid", par.par_Requestfor);
                cmd.Parameters.AddWithValue("@pardetails_isbudgeted", par.par_Budget);
                cmd.Parameters.AddWithValue("@action", "pardetilaselect");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                HttpContext.Current.Session["pardetails"] = dt;
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        getpardetails = new CbfParDetails()
                        {
                            pardet_Gid = Convert.ToInt64(dt.Rows[i]["pardetails_gid"].ToString()),
                            par_Expense = dt.Rows[i]["pardetails_expensetype"].ToString(),
                            par_Requestfor = dt.Rows[i]["requestfor_name"].ToString(),
                            par_Budget = dt.Rows[i]["pardetails_isbudgeted"].ToString(),
                            description = dt.Rows[i]["pardetails_desc"].ToString(),
                            year = dt.Rows[i]["pardetails_year"].ToString(),
                            b_FwdAmount = Convert.ToDecimal(dt.Rows[i]["TransferOut"].ToString()),
                            lin_Amt = Convert.ToDecimal(dt.Rows[i]["pardetails_amt"].ToString()),
                            ulis_Amt = Convert.ToDecimal(dt.Rows[i]["Untilized"].ToString()),
                            // c_FwdAmount = Convert.ToDecimal(dt.Rows[i]["balanced"].ToString()),
                            c_FwdAmount = Convert.ToDecimal(dt.Rows[i]["TransferIn"].ToString()),
                            balance = Convert.ToDecimal(dt.Rows[i]["balanced"].ToString()),
                            remarks = dt.Rows[i]["pardetails_remarks"].ToString(),
                        };
                        obj.cbfPardetailslst.Add(getpardetails);
                        System.Web.HttpContext.Current.Session["pardetails_gid"] = dt.Rows[i]["pardetails_gid"].ToString();
                    }
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
                objConn.Close();
            }
        }
        public CbfSumEntity CbfPardetailsedit(CbfParHeader par)
        {
            CbfSumEntity obj = new CbfSumEntity();
            try
            {
                getconnection();
              
                CbfParDetails getpardetails;
                obj.cbfPardetailslst = new List<CbfParDetails>();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_fb_trn_prsummary", objConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@parheader_gid", par.parGid);
                cmd.Parameters.AddWithValue("@prheader_requestforgid", par.par_Requestfor);
                cmd.Parameters.AddWithValue("@pardetails_isbudgeted", par.par_Budget);
                cmd.Parameters.AddWithValue("@cbfheader_gid", HttpContext.Current.Session["cbfgid"]);
                cmd.Parameters.AddWithValue("@action", "pardetailsedit");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        getpardetails = new CbfParDetails()
                        {
                            pardet_Gid = Convert.ToInt64(dt.Rows[i]["pardetails_gid"].ToString()),
                            par_Expense = dt.Rows[i]["pardetails_expensetype"].ToString(),
                            par_Requestfor = dt.Rows[i]["requestfor_name"].ToString(),
                            par_Budget = dt.Rows[i]["pardetails_isbudgeted"].ToString(),
                            description = dt.Rows[i]["pardetails_desc"].ToString(),
                            year = dt.Rows[i]["pardetails_year"].ToString(),
                            b_FwdAmount = Convert.ToDecimal(dt.Rows[i]["TransferOut"].ToString()),
                            lin_Amt = Convert.ToDecimal(dt.Rows[i]["pardetails_amt"].ToString()),
                            ulis_Amt = Convert.ToDecimal(dt.Rows[i]["Untilized"].ToString()),
                            // c_FwdAmount = Convert.ToDecimal(dt.Rows[i]["balanced"].ToString()),
                            c_FwdAmount = Convert.ToDecimal(dt.Rows[i]["TransferIn"].ToString()),
                            balance = Convert.ToDecimal(dt.Rows[i]["balanced"].ToString()),
                            remarks = dt.Rows[i]["pardetails_remarks"].ToString(),
                        };
                        obj.cbfPardetailslst.Add(getpardetails);
                    }
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
                objConn.Close();
            }
        }
        public string VendorPardetails_id(string lsdate1)
        {
            try
            {
                string lineamt="";
                getconnection();
                CbfSumEntity obj = new CbfSumEntity();
                CbfParDetails getpardetails;
                obj.cbfPardetailslst = new List<CbfParDetails>();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_fb_trn_topexprdetails", objConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pardetails_gid", lsdate1);
                cmd.Parameters.AddWithValue("@action", "pardetailsopex_id");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    lineamt = Convert.ToString(dt.Rows[0]["pardetails_amt"]);                          
                }
                return lineamt;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
        }
        public CbfSumEntity VendorPardetails(CbfParHeader par)
        {
            CbfSumEntity obj = new CbfSumEntity();
            try
            {
                getconnection();
               
                CbfParDetails getpardetails;
                obj.cbfPardetailslst = new List<CbfParDetails>();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_fb_trn_topexprdetails", objConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@parheader_gid", par.parGid);
                cmd.Parameters.AddWithValue("@prheader_requestforgid", par.par_Requestfor);
                cmd.Parameters.AddWithValue("@pardetails_isbudgeted", par.par_Budget);
                cmd.Parameters.AddWithValue("@action", "pardetailsopex");
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        getpardetails = new CbfParDetails()
                        {
                            pardet_Gid = Convert.ToInt64(dt.Rows[i]["pardetails_gid"].ToString()),
                            par_Expense = dt.Rows[i]["pardetails_expensetype"].ToString(),
                            par_Requestfor = dt.Rows[i]["requestfor_name"].ToString(),
                            par_Budget = dt.Rows[i]["pardetails_isbudgeted"].ToString(),
                            description = dt.Rows[i]["pardetails_desc"].ToString(),
                            year = dt.Rows[i]["pardetails_year"].ToString(),
                            // getpardetails.b_fwdamount = dt.Rows[i]["pardetails_year"].ToString();
                            lin_Amt = Convert.ToDecimal(dt.Rows[i]["pardetails_amt"].ToString()),
                            balance=Convert.ToInt32(dt.Rows[i]["balanced"].ToString()),
                            ulis_Amt=Convert.ToDecimal(dt.Rows[i]["Untilized"].ToString()),
                            // getpardetails.ulis_amt=dt.Rows[i][""].ToString();
                            //getpardetails.c_fwdamount=dt.Rows[i][""].ToString();
                            //  getpardetails.balance=dt.Rows[i][""].ToString();
                            remarks = dt.Rows[i]["pardetails_remarks"].ToString(),
                        };
                        obj.cbfPardetailslst.Add(getpardetails);
                    }
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
                objConn.Close();
            }
        }
        public List<CbfDetails> Getproductlist()
        {
            List<CbfDetails> prprodservgroup = new List<CbfDetails>();
            try
            {
                getconnection();
              
                CbfDetails prprodservgrp;
                cmdQry = new SqlCommand("pr_fb_mst_productgroup", objConn);
                DataTable dt = new DataTable();

                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@prodserviceflag", SqlDbType.VarChar).Value = "Product";
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        prprodservgrp = new CbfDetails();
                        prprodservgrp.productGroupId = Convert.ToInt32(dt.Rows[i]["prodservice_gid"].ToString());
                        prprodservgrp.productGroupIdName = dt.Rows[i]["prodservice_name"].ToString();
                        prprodservgroup.Add(prprodservgrp);


                    }

                }
                return prprodservgroup;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return prprodservgroup;
            }
            finally
            {
                objConn.Close();
            }

        }
        public List<CbfDetails> GetServicelist()
        {
            List<CbfDetails> prprodservgroup = new List<CbfDetails>();
            try
            {
                getconnection();
               
                CbfDetails prprodservgrp;
                cmdQry = new SqlCommand("pr_fb_mst_productgroup", objConn);
                DataTable dt = new DataTable();

                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@prodserviceflag", SqlDbType.VarChar).Value = "Service";
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        prprodservgrp = new CbfDetails();
                        prprodservgrp.productGroupId = Convert.ToInt32(dt.Rows[i]["prodservice_gid"].ToString());
                        prprodservgrp.productGroupIdName = dt.Rows[i]["prodservice_name"].ToString();
                        prprodservgroup.Add(prprodservgrp);


                    }

                }
                return prprodservgroup;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return prprodservgroup;
            }
            finally
            {
                objConn.Close();
            }

        }
        public List<CbfDetails> GetUomList()
        {
            List<CbfDetails> prprodservgroup = new List<CbfDetails>();
            try
            {
                getconnection();
                
                CbfDetails prprodservgrp;
                cmdQry = new SqlCommand("pr_fb_mst_uom", objConn);
                DataTable dt = new DataTable();

                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "Select";
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        prprodservgrp = new CbfDetails();
                        prprodservgrp.uomGid = Convert.ToInt32(dt.Rows[i]["uom_gid"].ToString());
                        prprodservgrp.uomCode = dt.Rows[i]["uom_code"].ToString();
                        prprodservgroup.Add(prprodservgrp);


                    }

                }
                return prprodservgroup;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return prprodservgroup;
            }
            finally
            {
                objConn.Close();
            }

        }


        public CbfSumEntity GetProdServDetails(int id)
        {
            CbfSumEntity prprodserv = new CbfSumEntity();
            try
            {
                getconnection();
                prprodserv.cbfDetails = new List<CbfDetails>();
                CbfDetails prprod;
                DataTable dt = new DataTable();

                cmdQry = new SqlCommand("pr_fb_mst_prodservicelist", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@prodservicegid", SqlDbType.Int).Value = id;
                cmdQry.Parameters.Add("@actionName", SqlDbType.VarChar, 80).Value = "Productgl";
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["prodservice_flag"].ToString() == "P")
                        {
                            prprod = new CbfDetails()
                            {
                                productgid = dt.Rows[i]["prodservice_gid"].ToString(),
                                productCode = dt.Rows[i]["prodservice_code"].ToString(),
                                productName = dt.Rows[i]["prodservice_name"].ToString(),
                                description = dt.Rows[i]["prodservice_description"].ToString(),
                                chartOfAccount = ((dt.Rows[i]["assetcategory_glno"].ToString() == "" || dt.Rows[i]["assetcategory_glno"].ToString() == null) ? dt.Rows[i]["expcat_gl_no"].ToString() : dt.Rows[i]["assetcategory_glno"].ToString()),

                            };
                            prprodserv.cbfDetails.Add(prprod);
                        }
                        else
                        {
                            prprod = new CbfDetails()
                            {
                                productgid = dt.Rows[i]["prodservice_gid"].ToString(),
                                productCode = dt.Rows[i]["prodservice_code"].ToString(),
                                productName = dt.Rows[i]["prodservice_name"].ToString(),
                                description = dt.Rows[i]["prodservice_description"].ToString(),
                                chartOfAccount = ((dt.Rows[i]["assetcategory_glno"].ToString() == "" || dt.Rows[i]["assetcategory_glno"].ToString() == null) ? dt.Rows[i]["expcat_gl_no"].ToString() : dt.Rows[i]["assetcategory_glno"].ToString()),

                            };
                            prprodserv.cbfDetails.Add(prprod);

                        }
                    }
                }
                return prprodserv;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return prprodserv;
            }
            finally
            {
                objConn.Close();
            }
        }

        public string UpdateCfHeaderSave(CbfRaiseHeader cbfheader)
        {
            try
            {
                int pardetails = 0;
                getconnection();
                DataTable dt2 = (DataTable)System.Web.HttpContext.Current.Session["AccessTokenheader"];
                DataTable dtattachment = (DataTable)System.Web.HttpContext.Current.Session["AccessToken"];
                DataTable dtcbfdetailsgid = new DataTable();
                if (HttpContext.Current.Session["da"] == null)
                {
                    HttpContext.Current.Session["da"] = HttpContext.Current.Session["cbfdeailsdt"];
                }
                dtcbfdetailsgid = (DataTable)HttpContext.Current.Session["da"];
                if (dtcbfdetailsgid.Rows.Count < 0)
                {
                    return "Please Save Any Cbf Details Item";
                }

                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@cbfheader_date", SqlDbType.Date).Value = convertoDate(cbfheader.cbfDate);
                cmdQry.Parameters.Add("@cbfheader_enddate", SqlDbType.Date).Value = convertoDate(cbfheader.cbfEnddate);
                cmdQry.Parameters.Add("@cbfheader_projectowner", SqlDbType.Int).Value = cbfheader.projectOwner;
                cmdQry.Parameters.Add("@cbfheader_bracnh_gid", SqlDbType.Int).Value = cbfheader.branchCode;
                cmdQry.Parameters.Add("@cbfheader_mode", SqlDbType.VarChar).Value = cbfheader.cbfMode;
                cmdQry.Parameters.Add("@cbfheader_approvaltype", SqlDbType.VarChar).Value = cbfheader.cbfType;
                cmdQry.Parameters.Add("@cbfheader_isbudgeted", SqlDbType.VarChar).Value = cbfheader.budgeted;
                cmdQry.Parameters.Add("@cbfheader_Devi_amt", SqlDbType.Decimal).Value = cbfheader.deviationAmt;
                cmdQry.Parameters.Add("@cbfheader_cbfamt", SqlDbType.Decimal).Value = cbfheader.cbfAmt;
                cmdQry.Parameters.Add("@cbfheader_desc", SqlDbType.VarChar).Value = cbfheader.description;
                cmdQry.Parameters.Add("@cbfheader_requestfor_gid", SqlDbType.Int).Value = cbfheader.requestFor;
                if (Convert.ToInt32(cbfheader.requestFor) == 3)
                {
                    cmdQry.Parameters.Add("@cbfheader_requesttype", SqlDbType.VarChar).Value = cbfheader.requestType;
                }
                cmdQry.Parameters.Add("@cbfheader_remarks", SqlDbType.VarChar).Value = cbfheader.justfication;
                cmdQry.Parameters.Add("@cbfheader_prpar_gid", SqlDbType.VarChar).Value = cbfheader.ParPRGid;
                cmdQry.Parameters.Add("@cbfheader_isbranchsingle", SqlDbType.VarChar).Value = cbfheader.branchType;
                cmdQry.Parameters.Add("@cbfheader_cbfobf_flag", SqlDbType.VarChar).Value = "C";
                cmdQry.Parameters.Add("@cbfheader_status", SqlDbType.VarChar).Value = "1";
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "cbfheaderupdate";
                cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.VarChar).Value = HttpContext.Current.Session["cbfgid"];
                cmdQry.Parameters.Add("@cbfheader_rasier_gid", SqlDbType.Int).Value = Convert.ToInt32(objCmnFunctions.GetLoginUserGid());
                cmdQry.Parameters.Add("@cbfheader_budgetowner_gid", SqlDbType.Int).Value = Convert.ToInt32(cbfheader.BudgetownerGid);
                int a = cmdQry.ExecuteNonQuery();
                string insertcommend;
                if (a == 1)
                {
                    if (dtattachment != null)
                    {
                        if (dtattachment.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtattachment.Rows.Count; i++)
                            {
                                if (dtattachment.Rows[i]["cbfheader_gid"].ToString() == "" || dtattachment.Rows[i]["cbfheader_gid"].ToString() == "1" || dtattachment.Rows[i]["cbfheader_gid"].ToString() == null)
                                {
                                    string[,] codes = new string[,]
                                {
                                {"attachment_ref_flag", "2"},
                                {"attachment_ref_gid", HttpContext.Current.Session["cbfgid"].ToString()},
                                {"attachment_attachmenttype_gid","3"},
                                {"attachment_date",objCmnFunctions.convertoDateTimeString(dtattachment.Rows[i]["Attachment_Date"].ToString())},
                                {"attachment_filename", dtattachment.Rows[i]["Documnet_Name"].ToString()},
                                {"attachment_desc",  dtattachment.Rows[i]["Document_des"].ToString()},
                                {"attachment_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                                };
                                    string tname = "iem_trn_tattachment";
                                    insertcommend = objCommonIUD.InsertCommon(codes, tname);
                                    if (insertcommend != "success")
                                    {
                                        return "Not Inserted CBF Header Attachment";
                                    }

                                }
                            }
                        }
                    }

                    if (dtcbfdetailsgid != null)
                    {
                        if (dtcbfdetailsgid.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtcbfdetailsgid.Rows.Count; i++)
                            {
                                if (dtcbfdetailsgid.Rows[i]["cbfdetails_gid"].ToString() == "")
                                {
                                    if (cbfheader.cbfMode == "PAR")
                                    {
                                        if (dtcbfdetailsgid.Rows[i]["myref"].ToString() == "1")
                                        {

                                            cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                                            cmdQry.CommandType = CommandType.StoredProcedure;
                                            cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.Int).Value = HttpContext.Current.Session["cbfgid"];
                                            cmdQry.Parameters.Add("@cbfdetails_prodgroup_gid", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["productgroup"].ToString();
                                            cmdQry.Parameters.Add("@cbfdetails_prodservice_gid", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["productgroup"].ToString();
                                            cmdQry.Parameters.Add("@cbfdetails_parprdesc", SqlDbType.VarChar).Value = dtcbfdetailsgid.Rows[i]["productdescription"].ToString();
                                            cmdQry.Parameters.Add("@cbfdetails_desc", SqlDbType.VarChar).Value = dtcbfdetailsgid.Rows[i]["remarks"].ToString();
                                            cmdQry.Parameters.Add("@cbfdetails_uom_gid", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["uom"].ToString();
                                            cmdQry.Parameters.Add("@cbfdetails_qty", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["qty"].ToString();
                                            cmdQry.Parameters.Add("@cbfdetails_unitprice", SqlDbType.Decimal).Value = dtcbfdetailsgid.Rows[i]["unitamount"].ToString();
                                            cmdQry.Parameters.Add("@cbfdetails_totalamt", SqlDbType.Decimal).Value = dtcbfdetailsgid.Rows[i]["total"].ToString();
                                            cmdQry.Parameters.Add("@cbfdetails_remarks", SqlDbType.VarChar).Value = dtcbfdetailsgid.Rows[i]["remarks"].ToString();
                                            cmdQry.Parameters.Add("@cbfdetails_chartofacc", SqlDbType.VarChar).Value = dtcbfdetailsgid.Rows[i]["chartofaccount"].ToString();
                                            cmdQry.Parameters.Add("@cbfdetails_FCCC", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["fccc"].ToString();
                                            cmdQry.Parameters.Add("@cbfdetails_budgetline", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["budgetline"].ToString();
                                            cmdQry.Parameters.Add("@cbfdetails_prpardel_gid", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["prdetails_gid"].ToString();

                                            cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "insertdetails";
                                            cmdQry.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;
                                            int a1 = cmdQry.ExecuteNonQuery();
                                            pardetails = Convert.ToInt32(cmdQry.Parameters["@result"].Value);
                                            
                                            if (a1 != 1)
                                            {
                                                return "Not Inserted CBF Details";
                                            }
                                            if (HttpContext.Current.Session["cbfgid"] != null)
                                            {
                                                int cbfheadgidforattachment = Convert.ToInt32(HttpContext.Current.Session["cbfgid"]);
                                                cmdQry = new SqlCommand("pr_fb_trn_attachmentall", objConn);
                                                cmdQry.CommandType = CommandType.StoredProcedure;
                                                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectcbfdetailsgid";
                                                cmdQry.Parameters.Add("@cbfheadgid", SqlDbType.Int).Value = cbfheadgidforattachment;
                                                cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                                                cmdQry.Parameters.Add("@process", SqlDbType.VarChar).Value = "CBFTEMP";
                                                objDataAdapter = new SqlDataAdapter(cmdQry);
                                                DataTable objTable = new DataTable();
                                                string gid = "";
                                                objDataAdapter.Fill(objTable);
                                                //for (int git = 0; git < objTable.Rows.Count; git++)
                                                //{
                                                //    gid = objTable.Rows[git]["cbfdetails_gid"].ToString();
                                                //}
                                                string refflaggid = "";
                                                string cbfdetailsflag = "";
                                                if (objTable.Rows.Count > 0)
                                                {
                                                    gid = objTable.Rows[0]["cbfdetails_gid"].ToString();
                                                    refflaggid = objTable.Rows[0]["ref_flag"].ToString();
                                                    cbfdetailsflag = objTable.Rows[0]["cbf_ref_flag"].ToString();
                                                }
                                                string[,] codes = new string[,]
	                                             {
                                                     {"attachment_ref_gid", gid},
                                                     {"attachment_ref_flag", cbfdetailsflag}
	                                        
                                                 };
                                                string[,] whrcol = new string[,]
	                                            {
                                                    {"attachment_ref_flag", refflaggid},
                                                    {"attachment_ref_gid", Convert.ToString(i + 1)},
                                                    {"attachment_by", objCmnFunctions.GetLoginUserGid().ToString()},
                                                    {"attachment_attachmenttype_gid", "2"}
                                                };
                                                objCommonIUD.UpdateCommon(codes, whrcol, "iem_trn_tattachment");
                                            }
                                          
                                           
                                        }
                                        //if (dt2 != null)
                                        //{

                                        //    if (dt2.Rows.Count > 0)
                                        //    {
                                        //        for (int ij = 0; ij < dt2.Rows.Count; ij++)
                                        //        {
                                        //            if (dt2.Rows[ij]["cbfdetails_gid"].ToString() == "1" && dtcbfdetailsgid.Rows[i]["sno"].ToString() == dt2.Rows[ij]["prdetails"].ToString())
                                        //            {
                                        //                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                                        //                cmdQry.CommandType = CommandType.StoredProcedure;
                                        //                cmdQry.Parameters.Add("@attachment_ref_flag", SqlDbType.Int).Value = 3;
                                        //                cmdQry.Parameters.Add("@attachment_attachmenttype_gid", SqlDbType.VarChar).Value = 3;
                                        //                cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.Int).Value = pardetails;
                                        //                cmdQry.Parameters.Add("@attachment_filename", SqlDbType.VarChar).Value = dt2.Rows[ij]["Documnet_Name"].ToString();
                                        //                cmdQry.Parameters.Add("@attachment_desc", SqlDbType.VarChar).Value = dt2.Rows[ij]["Document_des"].ToString();
                                        //                cmdQry.Parameters.Add("@attachment_date", SqlDbType.Date).Value = convertoDate(dt2.Rows[ij]["Attachment_Date"].ToString());
                                        //                cmdQry.Parameters.Add("@attachment_by", SqlDbType.Int).Value = 2;
                                        //                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "boqattachforpardetails";
                                        //                int a2 = cmdQry.ExecuteNonQuery();
                                        //                if (a2 != 1)
                                        //                {
                                        //                    return "Not Inserted CBF Details Attachment";
                                        //                }
                                        //            }
                                        //        }
                                        //    }
                                        //}
                                    }
                                    else
                                    {
                                        cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                                        cmdQry.CommandType = CommandType.StoredProcedure;
                                        cmdQry.Parameters.Add("@cbfdetails_cbfheader_gid", SqlDbType.Int).Value = HttpContext.Current.Session["cbfgid"];
                                        cmdQry.Parameters.Add("@cbfdetails_parprdesc_gid", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["prdetails_gid"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_prodservice_gid", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["prodservice_gid"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_parprdesc", SqlDbType.VarChar).Value = dtcbfdetailsgid.Rows[i]["prodservice_description"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_desc", SqlDbType.VarChar).Value = dtcbfdetailsgid.Rows[i]["remarks"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_uom_gid", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["uom_gid"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_qty", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["prdetails_qty"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_unitprice", SqlDbType.Decimal).Value = dtcbfdetailsgid.Rows[i]["pipinput_rate"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_totalamt", SqlDbType.Decimal).Value = dtcbfdetailsgid.Rows[i]["pipinput_costestimation"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_remarks", SqlDbType.VarChar).Value = dtcbfdetailsgid.Rows[i]["remarks"].ToString();
                                        if (dtcbfdetailsgid.Rows[i]["assetcategory_glno"].ToString() != "")
                                        {
                                            cmdQry.Parameters.Add("@cbfdetails_chartofacc", SqlDbType.VarChar).Value = dtcbfdetailsgid.Rows[i]["assetcategory_glno"].ToString();
                                        }
                                        else
                                        {
                                            cmdQry.Parameters.Add("@cbfdetails_chartofacc", SqlDbType.VarChar).Value = dtcbfdetailsgid.Rows[i]["expcat_gl_no"].ToString();
                                        }
                                        cmdQry.Parameters.Add("@cbfdetails_FCCC", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["fccc"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_budgetline", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["budgetline"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_prpardel_gid", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["prdetails_gid"].ToString();
                                        cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "savecbfdetails";
                                        int a1 = cmdQry.ExecuteNonQuery();
                                        if (a1 != 1)
                                        {
                                            return "Not Inserted CBF Details";
                                        }
                                        if (HttpContext.Current.Session["cbfgid"] != null)
                                        {
                                            int cbfheadgidforattachment = Convert.ToInt32(HttpContext.Current.Session["cbfgid"]);
                                            cmdQry = new SqlCommand("pr_fb_trn_attachmentall", objConn);
                                            cmdQry.CommandType = CommandType.StoredProcedure;
                                            cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectcbfdetailsgid";
                                            cmdQry.Parameters.Add("@cbfheadgid", SqlDbType.Int).Value = cbfheadgidforattachment;
                                            cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                                            cmdQry.Parameters.Add("@process", SqlDbType.VarChar).Value = "CBFTEMP";
                                            objDataAdapter = new SqlDataAdapter(cmdQry);
                                            DataTable objTable = new DataTable();
                                            string gid = "";
                                            objDataAdapter.Fill(objTable);
                                            //for (int git = 0; git < objTable.Rows.Count; git++)
                                            //{
                                            //    gid = objTable.Rows[git]["cbfdetails_gid"].ToString();
                                            //}
                                            string refflaggid = "";
                                            string cbfdetailsflag = "";
                                            if (objTable.Rows.Count > 0)
                                            {
                                                gid = objTable.Rows[0]["cbfdetails_gid"].ToString();
                                                refflaggid = objTable.Rows[0]["ref_flag"].ToString();
                                                cbfdetailsflag = objTable.Rows[0]["cbf_ref_flag"].ToString();
                                            }
                                            string[,] codes = new string[,]
	                                             {
                                                     {"attachment_ref_gid", gid},
                                                     {"attachment_ref_flag", cbfdetailsflag}
	                                        
                                                 };
                                            string[,] whrcol = new string[,]
	                                            {
                                                    {"attachment_ref_flag", refflaggid},
                                                    {"attachment_ref_gid", Convert.ToString(i + 1)},
                                                    {"attachment_by", objCmnFunctions.GetLoginUserGid().ToString()},
                                                    {"attachment_attachmenttype_gid", "2"}
                                                };
                                            objCommonIUD.UpdateCommon(codes, whrcol, "iem_trn_tattachment");
                                        }
                                        //if (dt2 != null)
                                        //{
                                        //    if (dt2.Rows.Count > 0)
                                        //    {
                                        //        for (int ji = 0; ji < dt2.Rows.Count; ji++)
                                        //        {
                                        //            if (dt2.Rows[ji]["cbfdetails_gid"].ToString() == "1" && dtcbfdetailsgid.Rows[i]["prdetails_gid"].ToString() == dt2.Rows[ji]["prdetails"].ToString())
                                        //            {
                                        //                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                                        //                cmdQry.CommandType = CommandType.StoredProcedure;
                                        //                cmdQry.Parameters.Add("@attachment_ref_flag", SqlDbType.Int).Value = 3;
                                        //                cmdQry.Parameters.Add("@attachment_attachmenttype_gid", SqlDbType.VarChar).Value = 3;
                                        //                cmdQry.Parameters.Add("@prdetailsgid", SqlDbType.Int).Value = dt2.Rows[ji]["prdetails"].ToString();
                                        //                cmdQry.Parameters.Add("@attachment_filename", SqlDbType.VarChar).Value = dt2.Rows[ji]["Documnet_Name"].ToString();
                                        //                cmdQry.Parameters.Add("@attachment_desc", SqlDbType.VarChar).Value = dt2.Rows[ji]["Document_des"].ToString();
                                        //                cmdQry.Parameters.Add("@attachment_date", SqlDbType.Date).Value = convertoDate(dt2.Rows[ji]["Attachment_Date"].ToString());
                                        //                cmdQry.Parameters.Add("@attachment_by", SqlDbType.Int).Value = 2;
                                        //                cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.VarChar).Value = HttpContext.Current.Session["cbfgid"];
                                        //                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "cbfdetailsattachmentedit";
                                        //                int a2 = cmdQry.ExecuteNonQuery();
                                        //                if (a2 != 1)
                                        //                {
                                        //                    return "Not Inserted CBF Details Attachment";
                                        //                }
                                        //            }
                                        //        }
                                        //    }
                                        //}
                                    }
                                }
                                else
                                {
                                    cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                                    cmdQry.CommandType = CommandType.StoredProcedure;
                                    cmdQry.Parameters.Add("@cbfdetails_qty", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["prdetails_qty"].ToString();
                                    cmdQry.Parameters.Add("@cbfdetails_unitprice", SqlDbType.Decimal).Value = dtcbfdetailsgid.Rows[i]["pipinput_rate"].ToString();
                                    cmdQry.Parameters.Add("@cbfdetails_totalamt", SqlDbType.Decimal).Value = dtcbfdetailsgid.Rows[i]["pipinput_costestimation"].ToString();
                                    cmdQry.Parameters.Add("@cbfdetails_remarks", SqlDbType.VarChar).Value = dtcbfdetailsgid.Rows[i]["remarks"].ToString();
                                    cmdQry.Parameters.Add("@cbfdetails_FCCC", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["fccc"].ToString();
                                    cmdQry.Parameters.Add("@cbfdetails_budgetline", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["budgetline"].ToString();
                                    cmdQry.Parameters.Add("@cbfdetails_uom_gid", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["uom_gid"].ToString();
                                    cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "updateforpr";
                                    cmdQry.Parameters.Add("@cbfdetails_gid", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["cbfdetails_gid"].ToString();
                                    int a1 = cmdQry.ExecuteNonQuery();
                                    if (a1 != 1)
                                    {
                                        return "Not Inserted CBF Details";
                                    }
                                    if (HttpContext.Current.Session["cbfgid"] != null)
                                    {
                                        int cbfheadgidforattachment = Convert.ToInt32(HttpContext.Current.Session["cbfgid"]);
                                        cmdQry = new SqlCommand("pr_fb_trn_attachmentall", objConn);
                                        cmdQry.CommandType = CommandType.StoredProcedure;
                                        cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectcbfdetailsgid";
                                        cmdQry.Parameters.Add("@cbfheadgid", SqlDbType.Int).Value = cbfheadgidforattachment;
                                        cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                                        cmdQry.Parameters.Add("@process", SqlDbType.VarChar).Value = "CBFTEMP";
                                        objDataAdapter = new SqlDataAdapter(cmdQry);
                                        DataTable objTable = new DataTable();
                                        string gid = "";
                                        objDataAdapter.Fill(objTable);
                                        //for (int git = 0; git < objTable.Rows.Count; git++)
                                        //{
                                        //    gid = objTable.Rows[git]["cbfdetails_gid"].ToString();
                                        //}
                                        string refflaggid = "";
                                        string cbfdetailsflag = "";
                                        if (objTable.Rows.Count > 0)
                                        {
                                            gid = objTable.Rows[0]["cbfdetails_gid"].ToString();
                                            refflaggid = objTable.Rows[0]["ref_flag"].ToString();
                                            cbfdetailsflag = objTable.Rows[0]["cbf_ref_flag"].ToString();
                                        }
                                        string[,] codes = new string[,]
	                                             {
                                                     {"attachment_ref_gid", gid},
                                                     {"attachment_ref_flag", cbfdetailsflag}
	                                        
                                                 };
                                        string[,] whrcol = new string[,]
	                                            {
                                                    {"attachment_ref_flag", refflaggid},
                                                    {"attachment_ref_gid", Convert.ToString(i + 1)},
                                                    {"attachment_by", objCmnFunctions.GetLoginUserGid().ToString()},
                                                    {"attachment_attachmenttype_gid", "2"}
                                                };
                                        objCommonIUD.UpdateCommon(codes, whrcol, "iem_trn_tattachment");
                                    }
                                //    if (dt2 != null)
                                //    {
                                //        if (dt2.Rows.Count > 0)
                                //        {
                                //            for (int ji = 0; ji < dt2.Rows.Count; ji++)
                                //            {
                                //                if (dt2.Rows[ji]["cbfdetails_gid"].ToString() == "2" && dtcbfdetailsgid.Rows[i]["cbfdetails_gid"].ToString() == dt2.Rows[ji]["prdetails"].ToString())
                                //                {

                                //                    string[,] codes = new string[,]
                                //{
                                //{"attachment_ref_flag", "3"},
                                //{"attachment_ref_gid", dt2.Rows[ji]["prdetails"].ToString()},
                                //{"attachment_attachmenttype_gid","3"},
                                //{"attachment_date",dt2.Rows[ji]["Attachment_Date"].ToString()},
                                //{"attachment_filename", dt2.Rows[ji]["Documnet_Name"].ToString()},
                                //{"attachment_desc",  dt2.Rows[ji]["Document_des"].ToString()},
                                //{"attachment_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                                //};
                                //                    string tname = "iem_trn_tattachment";
                                //                    insertcommend = objCommonIUD.InsertCommon(codes, tname);
                                //                    if (insertcommend != "success")
                                //                    {
                                //                        return "Not Inserted CBF Details Attachment";
                                //                    }
                                //                }

                                //            }
                                //        }
                                //    }
                                }
                                if (HttpContext.Current.Session["ArrayList"] != null)
                                {
                                    ArrayList lalistCbfDelete = (ArrayList)HttpContext.Current.Session["ArrayList"];
                                    if (lalistCbfDelete.Count > 0)
                                        for (int j = 0; j < lalistCbfDelete.Count; j++)
                                        {
                                            DeleteProductServiceDetails(lalistCbfDelete[j].ToString());
                                        }

                                }
                                if (HttpContext.Current.Session["Arraylistattachment"] != null)
                                {
                                    ArrayList lalistCbfDelete = (ArrayList)HttpContext.Current.Session["Arraylistattachment"];
                                    if (lalistCbfDelete.Count > 0)
                                        for (int j = 0; j < lalistCbfDelete.Count; j++)
                                        {
                                            DeleteAttachment(lalistCbfDelete[j].ToString());
                                        }

                                }

                            }
                        }
                        else
                        {
                            return "Please Save Any Cbf Details Line";
                        }
                    }

                }
                else
                {
                    return "Not Inserted";
                }
                return "Inserted Successfully";

            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
        }
        public string UpdateCfHeaderSubmit(CbfRaiseHeader cbfheader)
        {
            int cbfprevgid = 0;
            try
            {
                DataTable dtcbfdetailsgid = new DataTable();
                if (HttpContext.Current.Session["da"] == null)
                {
                    HttpContext.Current.Session["da"] = HttpContext.Current.Session["cbfdeailsdt"];
                }
                dtcbfdetailsgid = (DataTable)HttpContext.Current.Session["da"];
                if (dtcbfdetailsgid.Rows.Count < 0)
                {
                    return "Please Save Any Cbf Details Item";
                }

                int pardetails = 0;
                getconnection();
                DataTable dt2 = (DataTable)System.Web.HttpContext.Current.Session["AccessTokenheader"];
                DataTable dtattachment = (DataTable)System.Web.HttpContext.Current.Session["AccessToken"];

                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@cbfheader_date", SqlDbType.Date).Value = convertoDate(cbfheader.cbfDate);
                cmdQry.Parameters.Add("@cbfheader_enddate", SqlDbType.Date).Value = convertoDate(cbfheader.cbfEnddate);
                cmdQry.Parameters.Add("@cbfheader_projectowner", SqlDbType.Int).Value = cbfheader.projectOwner;
                cmdQry.Parameters.Add("@cbfheader_bracnh_gid", SqlDbType.Int).Value = cbfheader.branchCode;
                cmdQry.Parameters.Add("@cbfheader_mode", SqlDbType.VarChar).Value = cbfheader.cbfMode;
                cmdQry.Parameters.Add("@cbfheader_approvaltype", SqlDbType.VarChar).Value = cbfheader.cbfType;
                cmdQry.Parameters.Add("@cbfheader_isbudgeted", SqlDbType.VarChar).Value = cbfheader.budgeted;
                cmdQry.Parameters.Add("@cbfheader_Devi_amt", SqlDbType.Decimal).Value = cbfheader.deviationAmt;
                cmdQry.Parameters.Add("@cbfheader_cbfamt", SqlDbType.Decimal).Value = cbfheader.cbfAmt;
                cmdQry.Parameters.Add("@cbfheader_desc", SqlDbType.VarChar).Value = cbfheader.description;
                cmdQry.Parameters.Add("@cbfheader_requestfor_gid", SqlDbType.Int).Value = cbfheader.requestFor;
                if (Convert.ToInt32(cbfheader.requestFor) == 3)
                {
                    cmdQry.Parameters.Add("@cbfheader_requesttype", SqlDbType.VarChar).Value = cbfheader.requestType;
                }
                cmdQry.Parameters.Add("@cbfheader_remarks", SqlDbType.VarChar).Value = cbfheader.justfication;
                cmdQry.Parameters.Add("@cbfheader_prpar_gid", SqlDbType.VarChar).Value = cbfheader.ParPRGid;
                cmdQry.Parameters.Add("@cbfheader_isbranchsingle", SqlDbType.VarChar).Value = cbfheader.branchType;
                cmdQry.Parameters.Add("@cbfheader_cbfobf_flag", SqlDbType.VarChar).Value = "C";
                cmdQry.Parameters.Add("@cbfheader_status", SqlDbType.VarChar).Value = "2";
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "cbfheaderupdate";
                cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.VarChar).Value = HttpContext.Current.Session["cbfgid"];
                cmdQry.Parameters.Add("@cbfheader_rasier_gid", SqlDbType.Int).Value = Convert.ToInt32(objCmnFunctions.GetLoginUserGid());
                cmdQry.Parameters.Add("@cbfheader_budgetowner_gid", SqlDbType.Int).Value = Convert.ToInt32(cbfheader.BudgetownerGid);
                int a = cmdQry.ExecuteNonQuery();

                string insertcommend;
                if (a == 1)
                {
                    if (dtattachment != null)
                    {
                        if (dtattachment.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtattachment.Rows.Count; i++)
                            {
                                if (dtattachment.Rows[i]["cbfheader_gid"].ToString() == "" || dtattachment.Rows[i]["cbfheader_gid"].ToString() == "1" || dtattachment.Rows[i]["cbfheader_gid"].ToString() == null)
                                {
                                    string[,] codes = new string[,]
                                {
                                {"attachment_ref_flag", "2"},
                                {"attachment_ref_gid", HttpContext.Current.Session["cbfgid"].ToString()},
                                {"attachment_attachmenttype_gid","3"},
                                {"attachment_date",dtattachment.Rows[i]["Attachment_Date"].ToString()},
                                {"attachment_filename", dtattachment.Rows[i]["Documnet_Name"].ToString()},
                                {"attachment_desc",  dtattachment.Rows[i]["Document_des"].ToString()},
                                {"attachment_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                                };
                                    string tname = "iem_trn_tattachment";
                                    insertcommend = objCommonIUD.InsertCommon(codes, tname);
                                    if (insertcommend != "success")
                                    {
                                        return "Not Inserted CBF Header Attachment";
                                    }
                                }
                            }
                        }
                    }

                    if (dtcbfdetailsgid != null)
                    {
                        if (dtcbfdetailsgid.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtcbfdetailsgid.Rows.Count; i++)
                            {
                                if (dtcbfdetailsgid.Rows[i]["cbfdetails_gid"].ToString() == "")
                                {
                                    if (cbfheader.cbfMode == "PAR")
                                    {
                                        if (dtcbfdetailsgid.Rows[i]["myref"].ToString() == "1")
                                        {

                                            cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                                            cmdQry.CommandType = CommandType.StoredProcedure;
                                            cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.Int).Value = HttpContext.Current.Session["cbfgid"];
                                            cmdQry.Parameters.Add("@cbfdetails_prodgroup_gid", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["productgroup"].ToString();
                                            cmdQry.Parameters.Add("@cbfdetails_prodservice_gid", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["productgroup"].ToString();
                                            cmdQry.Parameters.Add("@cbfdetails_parprdesc", SqlDbType.VarChar).Value = dtcbfdetailsgid.Rows[i]["productdescription"].ToString();
                                            cmdQry.Parameters.Add("@cbfdetails_desc", SqlDbType.VarChar).Value = dtcbfdetailsgid.Rows[i]["remarks"].ToString();
                                            cmdQry.Parameters.Add("@cbfdetails_uom_gid", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["uom"].ToString();
                                            cmdQry.Parameters.Add("@cbfdetails_qty", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["qty"].ToString();
                                            cmdQry.Parameters.Add("@cbfdetails_unitprice", SqlDbType.Decimal).Value = dtcbfdetailsgid.Rows[i]["unitamount"].ToString();
                                            cmdQry.Parameters.Add("@cbfdetails_totalamt", SqlDbType.Decimal).Value = dtcbfdetailsgid.Rows[i]["total"].ToString();
                                            cmdQry.Parameters.Add("@cbfdetails_remarks", SqlDbType.VarChar).Value = dtcbfdetailsgid.Rows[i]["remarks"].ToString();
                                            cmdQry.Parameters.Add("@cbfdetails_chartofacc", SqlDbType.VarChar).Value = dtcbfdetailsgid.Rows[i]["chartofaccount"].ToString();
                                            cmdQry.Parameters.Add("@cbfdetails_FCCC", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["fccc"].ToString();
                                            cmdQry.Parameters.Add("@cbfdetails_budgetline", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["budgetline"].ToString();
                                            cmdQry.Parameters.Add("@cbfdetails_prpardel_gid", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["prdetails_gid"].ToString();
                                            cmdQry.Parameters.Add("@cbfheader_status", SqlDbType.VarChar).Value = "1";
                                            cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "insertdetails";
                                            cmdQry.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;
                                            int a1 = cmdQry.ExecuteNonQuery();
                                            pardetails = Convert.ToInt32(cmdQry.Parameters["@result"].Value);
                                            if (a1 != 1)
                                            {
                                                return "Not Inserted CBF Details";
                                            }
                                            if (HttpContext.Current.Session["cbfgid"] != null)
                                            {
                                                int cbfheadgidforattachment = Convert.ToInt32(HttpContext.Current.Session["cbfgid"]);
                                                cmdQry = new SqlCommand("pr_fb_trn_attachmentall", objConn);
                                                cmdQry.CommandType = CommandType.StoredProcedure;
                                                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectcbfdetailsgid";
                                                cmdQry.Parameters.Add("@cbfheadgid", SqlDbType.Int).Value = cbfheadgidforattachment;
                                                cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                                                cmdQry.Parameters.Add("@process", SqlDbType.VarChar).Value = "CBFTEMP";
                                                objDataAdapter = new SqlDataAdapter(cmdQry);
                                                DataTable objTable = new DataTable();
                                                string gid = "";
                                                objDataAdapter.Fill(objTable);
                                                //for (int git = 0; git < objTable.Rows.Count; git++)
                                                //{
                                                //    gid = objTable.Rows[git]["cbfdetails_gid"].ToString();
                                                //}
                                                string refflaggid = "";
                                                string cbfdetailsflag = "";
                                                if (objTable.Rows.Count > 0)
                                                {
                                                    gid = objTable.Rows[0]["cbfdetails_gid"].ToString();
                                                    refflaggid = objTable.Rows[0]["ref_flag"].ToString();
                                                    cbfdetailsflag = objTable.Rows[0]["cbf_ref_flag"].ToString();
                                                }
                                                string[,] codes = new string[,]
	                                             {
                                                     {"attachment_ref_gid", gid},
                                                     {"attachment_ref_flag", cbfdetailsflag}
	                                        
                                                 };
                                                string[,] whrcol = new string[,]
	                                            {
                                                    {"attachment_ref_flag", refflaggid},
                                                    {"attachment_ref_gid", Convert.ToString(i + 1)},
                                                    {"attachment_by", objCmnFunctions.GetLoginUserGid().ToString()},
                                                    {"attachment_attachmenttype_gid", "2"}
                                                };
                                                objCommonIUD.UpdateCommon(codes, whrcol, "iem_trn_tattachment");
                                            }
                                        }
                                        //if (dt2 != null)
                                        //{

                                        //    if (dt2.Rows.Count > 0)
                                        //    {
                                        //        for (int ij = 0; ij < dt2.Rows.Count; ij++)
                                        //        {
                                        //            if (dt2.Rows[ij]["cbfdetails_gid"].ToString() == "1" && dtcbfdetailsgid.Rows[i]["sno"].ToString() == dt2.Rows[ij]["prdetails"].ToString())
                                        //            {
                                        //                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                                        //                cmdQry.CommandType = CommandType.StoredProcedure;
                                        //                cmdQry.Parameters.Add("@attachment_ref_flag", SqlDbType.Int).Value = 3;
                                        //                cmdQry.Parameters.Add("@attachment_attachmenttype_gid", SqlDbType.VarChar).Value = 3;
                                        //                cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.Int).Value = pardetails;
                                        //                cmdQry.Parameters.Add("@attachment_filename", SqlDbType.VarChar).Value = dt2.Rows[ij]["Documnet_Name"].ToString();
                                        //                cmdQry.Parameters.Add("@attachment_desc", SqlDbType.VarChar).Value = dt2.Rows[ij]["Document_des"].ToString();
                                        //                cmdQry.Parameters.Add("@attachment_date", SqlDbType.Date).Value = convertoDate(dt2.Rows[ij]["Attachment_Date"].ToString());
                                        //                cmdQry.Parameters.Add("@attachment_by", SqlDbType.Int).Value = 2;
                                        //                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "boqattachforpardetails";
                                        //                int a2 = cmdQry.ExecuteNonQuery();
                                        //                if (a2 != 1)
                                        //                {
                                        //                    return "Not Inserted CBF Details Attachment";
                                        //                }
                                        //            }
                                        //        }
                                        //    }
                                        //}
                                    }
                                    else
                                    {
                                        cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                                        cmdQry.CommandType = CommandType.StoredProcedure;
                                        cmdQry.Parameters.Add("@cbfdetails_cbfheader_gid", SqlDbType.Int).Value = HttpContext.Current.Session["cbfgid"];
                                        cmdQry.Parameters.Add("@cbfdetails_parprdesc_gid", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["prdetails_gid"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_prodservice_gid", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["prodservice_gid"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_parprdesc", SqlDbType.VarChar).Value = dtcbfdetailsgid.Rows[i]["prodservice_description"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_desc", SqlDbType.VarChar).Value = dtcbfdetailsgid.Rows[i]["remarks"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_uom_gid", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["uom_gid"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_qty", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["prdetails_qty"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_unitprice", SqlDbType.Decimal).Value = dtcbfdetailsgid.Rows[i]["pipinput_rate"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_totalamt", SqlDbType.Decimal).Value = dtcbfdetailsgid.Rows[i]["pipinput_costestimation"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_remarks", SqlDbType.VarChar).Value = dtcbfdetailsgid.Rows[i]["remarks"].ToString();
                                        if (dtcbfdetailsgid.Rows[i]["assetcategory_glno"].ToString() != "")
                                        {
                                            cmdQry.Parameters.Add("@cbfdetails_chartofacc", SqlDbType.VarChar).Value = dtcbfdetailsgid.Rows[i]["assetcategory_glno"].ToString();
                                        }
                                        else
                                        {
                                            cmdQry.Parameters.Add("@cbfdetails_chartofacc", SqlDbType.VarChar).Value = dtcbfdetailsgid.Rows[i]["expcat_gl_no"].ToString();
                                        }
                                        cmdQry.Parameters.Add("@cbfdetails_FCCC", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["fccc"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_budgetline", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["budgetline"].ToString();
                                        cmdQry.Parameters.Add("@cbfdetails_prpardel_gid", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["prdetails_gid"].ToString();
                                        cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "savecbfdetails";
                                        int a1 = cmdQry.ExecuteNonQuery();
                                        if (a1 != 1)
                                        {
                                            return "Not Inserted CBF Details";
                                        }
                                        if (HttpContext.Current.Session["cbfgid"] != null)
                                        {
                                            int cbfheadgidforattachment = Convert.ToInt32(HttpContext.Current.Session["cbfgid"]);
                                            cmdQry = new SqlCommand("pr_fb_trn_attachmentall", objConn);
                                            cmdQry.CommandType = CommandType.StoredProcedure;
                                            cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectcbfdetailsgid";
                                            cmdQry.Parameters.Add("@cbfheadgid", SqlDbType.Int).Value = cbfheadgidforattachment;
                                            cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                                            cmdQry.Parameters.Add("@process", SqlDbType.VarChar).Value = "CBFTEMP";
                                            objDataAdapter = new SqlDataAdapter(cmdQry);
                                            DataTable objTable = new DataTable();
                                            string gid = "";
                                            objDataAdapter.Fill(objTable);
                                            //for (int git = 0; git < objTable.Rows.Count; git++)
                                            //{
                                            //    gid = objTable.Rows[git]["cbfdetails_gid"].ToString();
                                            //}
                                            string refflaggid = "";
                                            string cbfdetailsflag = "";
                                            if (objTable.Rows.Count > 0)
                                            {
                                                gid = objTable.Rows[0]["cbfdetails_gid"].ToString();
                                                refflaggid = objTable.Rows[0]["ref_flag"].ToString();
                                                cbfdetailsflag = objTable.Rows[0]["cbf_ref_flag"].ToString();
                                            }
                                            string[,] codes = new string[,]
	                                             {
                                                     {"attachment_ref_gid", gid},
                                                     {"attachment_ref_flag", cbfdetailsflag}
	                                        
                                                 };
                                            string[,] whrcol = new string[,]
	                                            {
                                                    {"attachment_ref_flag", refflaggid},
                                                    {"attachment_ref_gid", Convert.ToString(i + 1)},
                                                    {"attachment_by", objCmnFunctions.GetLoginUserGid().ToString()},
                                                    {"attachment_attachmenttype_gid", "2"}
                                                };
                                            objCommonIUD.UpdateCommon(codes, whrcol, "iem_trn_tattachment");
                                        }
                                        //if (dt2 != null)
                                        //{
                                        //    if (dt2.Rows.Count > 0)
                                        //    {
                                        //        for (int ji = 0; ji < dt2.Rows.Count; ji++)
                                        //        {
                                        //            if (dt2.Rows[ji]["cbfdetails_gid"].ToString() == "1" && dtcbfdetailsgid.Rows[i]["prdetails_gid"].ToString() == dt2.Rows[ji]["prdetails"].ToString())
                                        //            {
                                        //                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                                        //                cmdQry.CommandType = CommandType.StoredProcedure;
                                        //                cmdQry.Parameters.Add("@attachment_ref_flag", SqlDbType.Int).Value = 3;
                                        //                cmdQry.Parameters.Add("@attachment_attachmenttype_gid", SqlDbType.VarChar).Value = 3;
                                        //                cmdQry.Parameters.Add("@prdetailsgid", SqlDbType.Int).Value = dt2.Rows[ji]["prdetails"].ToString();
                                        //                cmdQry.Parameters.Add("@attachment_filename", SqlDbType.VarChar).Value = dt2.Rows[ji]["Documnet_Name"].ToString();
                                        //                cmdQry.Parameters.Add("@attachment_desc", SqlDbType.VarChar).Value = dt2.Rows[ji]["Document_des"].ToString();
                                        //                cmdQry.Parameters.Add("@attachment_date", SqlDbType.Date).Value = convertoDate(dt2.Rows[ji]["Attachment_Date"].ToString());
                                        //                cmdQry.Parameters.Add("@attachment_by", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                                        //                cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.VarChar).Value = HttpContext.Current.Session["cbfgid"];
                                        //                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "cbfdetailsattachmentedit";
                                        //                int a2 = cmdQry.ExecuteNonQuery();
                                        //                if (a2 != 1)
                                        //                {
                                        //                    return "Not Inserted CBF Details Attachment";
                                        //                }
                                        //            }
                                        //        }
                                        //    }
                                        //}
                                    }
                                }
                                else
                                {
                                    cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                                    cmdQry.CommandType = CommandType.StoredProcedure;
                                    cmdQry.Parameters.Add("@cbfdetails_qty", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["prdetails_qty"].ToString();
                                    cmdQry.Parameters.Add("@cbfdetails_unitprice", SqlDbType.Decimal).Value = dtcbfdetailsgid.Rows[i]["pipinput_rate"].ToString();
                                    cmdQry.Parameters.Add("@cbfdetails_totalamt", SqlDbType.Decimal).Value = dtcbfdetailsgid.Rows[i]["pipinput_costestimation"].ToString();
                                    cmdQry.Parameters.Add("@cbfdetails_remarks", SqlDbType.VarChar).Value = dtcbfdetailsgid.Rows[i]["remarks"].ToString();
                                    cmdQry.Parameters.Add("@cbfdetails_FCCC", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["fccc"].ToString();
                                    cmdQry.Parameters.Add("@cbfdetails_budgetline", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["budgetline"].ToString();
                                    cmdQry.Parameters.Add("@cbfdetails_uom_gid", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["uom_gid"].ToString();
                                    cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "updateforpr";
                                    cmdQry.Parameters.Add("@cbfdetails_gid", SqlDbType.Int).Value = dtcbfdetailsgid.Rows[i]["cbfdetails_gid"].ToString();
                                    int a1 = cmdQry.ExecuteNonQuery();
                                    if (a1 != 1)
                                    {
                                        return "Not Inserted CBF Details";
                                    }
                                    if (HttpContext.Current.Session["cbfgid"] != null)
                                    {
                                        int cbfheadgidforattachment = Convert.ToInt32(HttpContext.Current.Session["cbfgid"]);
                                        cmdQry = new SqlCommand("pr_fb_trn_attachmentall", objConn);
                                        cmdQry.CommandType = CommandType.StoredProcedure;
                                        cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectcbfdetailsgid";
                                        cmdQry.Parameters.Add("@cbfheadgid", SqlDbType.Int).Value = cbfheadgidforattachment;
                                        cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                                        cmdQry.Parameters.Add("@process", SqlDbType.VarChar).Value = "CBFTEMP";
                                        objDataAdapter = new SqlDataAdapter(cmdQry);
                                        DataTable objTable = new DataTable();
                                        string gid = "";
                                        objDataAdapter.Fill(objTable);
                                        //for (int git = 0; git < objTable.Rows.Count; git++)
                                        //{
                                        //    gid = objTable.Rows[git]["cbfdetails_gid"].ToString();
                                        //}
                                        string refflaggid = "";
                                        string cbfdetailsflag = "";
                                        if (objTable.Rows.Count > 0)
                                        {
                                            gid = objTable.Rows[0]["cbfdetails_gid"].ToString();
                                            refflaggid = objTable.Rows[0]["ref_flag"].ToString();
                                            cbfdetailsflag = objTable.Rows[0]["cbf_ref_flag"].ToString();
                                        }
                                        string[,] codes = new string[,]
	                                             {
                                                     {"attachment_ref_gid", gid},
                                                     {"attachment_ref_flag", cbfdetailsflag}
	                                        
                                                 };
                                        string[,] whrcol = new string[,]
	                                            {
                                                    {"attachment_ref_flag", refflaggid},
                                                    {"attachment_ref_gid", Convert.ToString(i + 1)},
                                                    {"attachment_by", objCmnFunctions.GetLoginUserGid().ToString()},
                                                    {"attachment_attachmenttype_gid", "2"}
                                                };
                                        objCommonIUD.UpdateCommon(codes, whrcol, "iem_trn_tattachment");
                                    }

                                //    if (dt2 != null)
                                //    {
                                //        if (dt2.Rows.Count > 0)
                                //        {
                                //            for (int ji = 0; ji < dt2.Rows.Count; ji++)
                                //            {
                                //                if (dt2.Rows[ji]["cbfdetails_gid"].ToString() == "2" && dtcbfdetailsgid.Rows[i]["cbfdetails_gid"].ToString() == dt2.Rows[ji]["prdetails"].ToString())
                                //                {

                                //                    string[,] codes = new string[,]
                                //{
                                //{"attachment_ref_flag", "3"},
                                //{"attachment_ref_gid", dt2.Rows[ji]["prdetails"].ToString()},
                                //{"attachment_attachmenttype_gid","3"},
                                //{"attachment_date",dt2.Rows[ji]["Attachment_Date"].ToString()},
                                //{"attachment_filename", dt2.Rows[ji]["Documnet_Name"].ToString()},
                                //{"attachment_desc",  dt2.Rows[ji]["Document_des"].ToString()},
                                //{"attachment_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                                //};
                                //                    string tname = "iem_trn_tattachment";
                                //                    insertcommend = objCommonIUD.InsertCommon(codes, tname);
                                //                    if (insertcommend != "success")
                                //                    {
                                //                        return "Not Inserted CBF Details Attachment";
                                //                    }

                                //                }
                                //            }
                                //        }
                                //    }
                                }
                                if (HttpContext.Current.Session["ArrayList"] != null)
                                {
                                    ArrayList lalistCbfDelete = (ArrayList)HttpContext.Current.Session["ArrayList"];
                                    if (lalistCbfDelete.Count > 0)
                                        for (int j = 0; j < lalistCbfDelete.Count; j++)
                                        {
                                            DeleteProductServiceDetails(lalistCbfDelete[i].ToString());
                                        }

                                }
                                if (HttpContext.Current.Session["Arraylistattachment"] != null)
                                {
                                    ArrayList lalistCbfDelete = (ArrayList)HttpContext.Current.Session["Arraylistattachment"];
                                    if (lalistCbfDelete.Count > 0)
                                        for (int j = 0; j < lalistCbfDelete.Count; j++)
                                        {
                                            DeleteAttachment(lalistCbfDelete[i].ToString());
                                        }

                                }

                            }

                        }

                        else
                        {
                            return "Please Save Any Cbf Details Line";
                        }
                    }
                    if (cbfheader.BudgetownerGid.ToString() != null || cbfheader.BudgetownerGid.ToString() != "")
                    {

                        string[,] codes = new string[,]
                                {
                                {"queue_date", "sysdatetime()"},
                                {"queue_ref_flag","2"},
                                {"queue_ref_gid", HttpContext.Current.Session["cbfgid"].ToString()},
                                {"queue_from",objCmnFunctions.GetLoginUserGid().ToString()},
                                {"queue_to_type", "I"},
                                {"queue_to", cbfheader.BudgetownerGid.ToString()},
                                {"queue_action_for","A"},
                                {"queue_prev_gid",cbfprevgid.ToString()}

                                };
                        string tname = "iem_trn_tqueue";
                        string result = objCommonIUD.InsertCommon(codes, tname);

                        if (result == "success")
                        {
                            string[,] codes1 = new string[,]
                            {
                                {"cbfheader_currentapprovalstage","1"}
                               
                            };
                            string[,] wherecolumn = new string[,]
                            {
                                {"cbfheader_gid", HttpContext.Current.Session["cbfgid"].ToString()},
                                {"cbfheader_isremoved","N"}
                            };
                            string tname1 = "fb_trn_tcbfheader";
                            string result1 = objCommonIUD.UpdateCommon(codes1, wherecolumn, tname1);
                        }
                    }
                    //cmdQry = new SqlCommand("pr_fb_trn_ApprovalQuenu", objConn);
                    //cmdQry.CommandType = CommandType.StoredProcedure;
                    //cmdQry.Parameters.Add("@refgid", SqlDbType.Int).Value = HttpContext.Current.Session["cbfgid"];
                    //cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                    //cmdQry.Parameters.Add("@requesttype", SqlDbType.VarChar).Value = "CBF";
                    //int a3 = cmdQry.ExecuteNonQuery();
                }
                else
                {
                    return "Not Inserted";
                }
                return "Inserted Successfully";

            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }

        }
        public CbfSumEntity GetCbfSumryChecker()
        {
            CbfSumEntity objSumEntity = new CbfSumEntity();
            try
            {
                getconnection();
               
                CbfCheckerSummary obj_getvalues;
                objSumEntity.cbfCheckersumamry = new List<CbfCheckerSummary>();
                DataTable dt = new DataTable();

                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.BigInt).Value = 0;
                cmdQry.Parameters.Add("@employee_gid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "cbfcheckersummary";
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        obj_getvalues = new CbfCheckerSummary()
                        {
                            cbFGidChecker = Convert.ToInt32(dt.Rows[i]["cbfheader_gid"].ToString()),
                            cbfNoChecker = dt.Rows[i]["cbfheader_cbfno"].ToString(),
                            cbfDateChecker = (dt.Rows[i]["cbfheader_date"].ToString()),
                            cbfEndDateChecker = (dt.Rows[i]["cbfheader_enddate"].ToString()),
                            cbfProjectOwnerChecker = dt.Rows[i]["cbfheader_projectowner"].ToString(),
                            cbfDevi_AmoChecker = Convert.ToDecimal(dt.Rows[i]["cbfheader_Devi_amt"].ToString()),
                            cbfAmoChecker = Convert.ToDecimal(dt.Rows[i]["cbfheader_cbfamt"].ToString()),
                            cbfDescriptionChecker = dt.Rows[i]["cbfheader_desc"].ToString(),
                            fincon_Bugchecker = dt.Rows[i]["cbfheader_isbudgeted"].ToString(),
                            cbfrequestForChecker = dt.Rows[i]["cbfheader_department"].ToString(),
                            cbfStatusChecker = dt.Rows[i]["cbfheader_status"].ToString(),
                            cbfModeChecker = dt.Rows[i]["cbfheader_mode"].ToString()
                        };

                        objSumEntity.cbfCheckersumamry.Add(obj_getvalues);
                    }
                }
                return objSumEntity;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objSumEntity;
            }
            finally
            {
                objConn.Close();
            }
        }
        public CbfSumEntity GetCbfcancellationSummary()
        {
            CbfSumEntity objSumEntity = new CbfSumEntity();
            try
            {
                getconnection();
               
                CbfCheckerSummary obj_getvalues;
                objSumEntity.cbfCheckersumamry = new List<CbfCheckerSummary>();
                DataTable dt = new DataTable();

                cmdQry = new SqlCommand("pr_fb_trn_tcbfcancellation", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "CbfcancellationSummary";
                cmdQry.Parameters.Add("@logingid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid().ToString();
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        obj_getvalues = new CbfCheckerSummary()
                        {
                            cbFGidChecker = Convert.ToInt32(dt.Rows[i]["cbfheader_gid"].ToString()),
                            cbfNoChecker = dt.Rows[i]["cbfheader_cbfno"].ToString(),
                            cbfDateChecker = (dt.Rows[i]["cbfheader_date"].ToString()),
                            cbfEndDateChecker = (dt.Rows[i]["cbfheader_enddate"].ToString()),
                            cbfProjectOwnerChecker = dt.Rows[i]["cbfheader_projectowner"].ToString(),
                            cbfDevi_AmoChecker = Convert.ToDecimal(dt.Rows[i]["cbfheader_Devi_amt"].ToString()),
                            cbfAmoChecker = Convert.ToDecimal(dt.Rows[i]["cbfheader_cbfamt"].ToString()),
                            cbfDescriptionChecker = dt.Rows[i]["cbfheader_desc"].ToString(),
                            fincon_Bugchecker = dt.Rows[i]["cbfheader_isbudgeted"].ToString(),
                            cbfrequestForChecker = dt.Rows[i]["cbfheader_department"].ToString(),
                            cbfModeChecker = dt.Rows[i]["cbfheader_mode"].ToString(),
                            Cbfcancellationgid = dt.Rows[i]["cbfcancellation_gid"].ToString(),
                            cbfCancellationRemarks = dt.Rows[i]["cbfcancellation_mak_remarks"].ToString(),
                        };

                        objSumEntity.cbfCheckersumamry.Add(obj_getvalues);
                    }
                }
                return objSumEntity;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objSumEntity;
            }
            finally
            {
                objConn.Close();
            }
        }

        public CbfSumEntity GetCbfClosureChkSummary()
        {
            CbfSumEntity objSumEntity = new CbfSumEntity();
            try
            {
                getconnection();
              
                CbfCheckerSummary obj_getvalues;
                objSumEntity.cbfCheckersumamry = new List<CbfCheckerSummary>();
                DataTable dt = new DataTable();

                cmdQry = new SqlCommand("pr_fb_trn_tcbfcancellation", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "cbfclosurechksummary";
                cmdQry.Parameters.Add("@logingid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid().ToString();
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        obj_getvalues = new CbfCheckerSummary()
                        {
                            cbFGidChecker = Convert.ToInt32(dt.Rows[i]["cbfheader_gid"].ToString()),
                            cbfNoChecker = dt.Rows[i]["cbfheader_cbfno"].ToString(),
                            cbfDateChecker = (dt.Rows[i]["cbfheader_date"].ToString()),
                            cbfEndDateChecker = (dt.Rows[i]["cbfheader_enddate"].ToString()),
                            cbfProjectOwnerChecker = dt.Rows[i]["cbfheader_projectowner"].ToString(),
                            cbfDevi_AmoChecker = Convert.ToDecimal(dt.Rows[i]["cbfheader_Devi_amt"].ToString()),
                            cbfAmoChecker = Convert.ToDecimal(dt.Rows[i]["cbfheader_cbfamt"].ToString()),
                            cbfDescriptionChecker = dt.Rows[i]["cbfheader_desc"].ToString(),
                            fincon_Bugchecker = dt.Rows[i]["cbfheader_isbudgeted"].ToString(),
                            cbfrequestForChecker = dt.Rows[i]["cbfheader_department"].ToString(),
                            cbfModeChecker = dt.Rows[i]["cbfheader_mode"].ToString(),
                            Cbfcancellationgid = dt.Rows[i]["cbfclosure_gid"].ToString(),
                            cbfclosuredate = dt.Rows[i]["cbfclosure_date"].ToString(),
                            cbfclosureremarks = dt.Rows[i]["cbfclosure_remarks"].ToString(),
                        };

                        objSumEntity.cbfCheckersumamry.Add(obj_getvalues);
                    }
                }
                return objSumEntity;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objSumEntity;
            }
            finally
            {
                objConn.Close();
            }
        }
        public CbfSumEntity GetCbfReopenChkSummary()
        {
            CbfSumEntity objSumEntity = new CbfSumEntity();
            try
            {
                getconnection();
             
                CbfCheckerSummary obj_getvalues;
                objSumEntity.cbfCheckersumamry = new List<CbfCheckerSummary>();
                DataTable dt = new DataTable();

                cmdQry = new SqlCommand("pr_fb_trn_tcbfcancellation", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "CbfReopenchecker";
                cmdQry.Parameters.Add("@logingid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid().ToString();
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        obj_getvalues = new CbfCheckerSummary()
                        {
                            cbFGidChecker = Convert.ToInt32(dt.Rows[i]["cbfheader_gid"].ToString()),
                            cbfNoChecker = dt.Rows[i]["cbfheader_cbfno"].ToString(),
                            cbfDateChecker = (dt.Rows[i]["cbfheader_date"].ToString()),
                            cbfEndDateChecker = (dt.Rows[i]["cbfheader_enddate"].ToString()),
                            cbfProjectOwnerChecker = dt.Rows[i]["cbfheader_projectowner"].ToString(),
                            cbfDevi_AmoChecker = Convert.ToDecimal(dt.Rows[i]["cbfheader_Devi_amt"].ToString()),
                            cbfAmoChecker = Convert.ToDecimal(dt.Rows[i]["cbfheader_cbfamt"].ToString()),
                            cbfDescriptionChecker = dt.Rows[i]["cbfheader_desc"].ToString(),
                            fincon_Bugchecker = dt.Rows[i]["cbfheader_isbudgeted"].ToString(),
                            cbfrequestForChecker = dt.Rows[i]["cbfheader_department"].ToString(),
                            cbfModeChecker = dt.Rows[i]["cbfheader_mode"].ToString(),
                            Cbfcancellationgid = dt.Rows[i]["cbfclosure_gid"].ToString(),
                            cbfclosureremarks = dt.Rows[i]["cbfclosure_reopenremarks"].ToString(),
                            cbfclosuredate = dt.Rows[i]["cbfclosure_reopen_date"].ToString(),
                        };

                        objSumEntity.cbfCheckersumamry.Add(obj_getvalues);
                    }
                }
                return objSumEntity;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objSumEntity;
            }
            finally
            {
                objConn.Close();
            }
        }
        public CbfSumEntity GetCbfReopenSummary()
        {
            CbfSumEntity objSumEntity = new CbfSumEntity();
            try
            {
                getconnection();
            
                CbfCheckerSummary obj_getvalues;
                objSumEntity.cbfCheckersumamry = new List<CbfCheckerSummary>();
                DataTable dt = new DataTable();

                cmdQry = new SqlCommand("pr_fb_trn_tcbfcancellation", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "CbfReopenmaker";
                cmdQry.Parameters.Add("@logingid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid().ToString();
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        obj_getvalues = new CbfCheckerSummary()
                        {
                            cbFGidChecker = Convert.ToInt32(dt.Rows[i]["cbfheader_gid"].ToString()),
                            cbfNoChecker = dt.Rows[i]["cbfheader_cbfno"].ToString(),
                            cbfDateChecker = (dt.Rows[i]["cbfheader_date"].ToString()),
                            cbfEndDateChecker = (dt.Rows[i]["cbfheader_enddate"].ToString()),
                            cbfProjectOwnerChecker = dt.Rows[i]["cbfheader_projectowner"].ToString(),
                            cbfDevi_AmoChecker = Convert.ToDecimal(dt.Rows[i]["cbfheader_Devi_amt"].ToString()),
                            cbfAmoChecker = Convert.ToDecimal(dt.Rows[i]["cbfheader_cbfamt"].ToString()),
                            cbfDescriptionChecker = dt.Rows[i]["cbfheader_desc"].ToString(),
                            fincon_Bugchecker = dt.Rows[i]["cbfheader_isbudgeted"].ToString(),
                            cbfrequestForChecker = dt.Rows[i]["cbfheader_department"].ToString(),
                            cbfModeChecker = dt.Rows[i]["cbfheader_mode"].ToString(),
                            Cbfcancellationgid = dt.Rows[i]["cbfclosure_gid"].ToString(),
                        };

                        objSumEntity.cbfCheckersumamry.Add(obj_getvalues);
                    }
                }
                return objSumEntity;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objSumEntity;
            }
            finally
            {
                objConn.Close();
            }
        }


        public CbfSumEntity GetCbfApprovalSummary()
        {
            CbfSumEntity objSumEntity = new CbfSumEntity();
            try
            {
                getconnection();
                
                CbfCheckerSummary obj_getvalues;
                objSumEntity.cbfCheckersumamry = new List<CbfCheckerSummary>();
                DataTable dt = new DataTable();

                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.BigInt).Value = 0;
                cmdQry.Parameters.Add("@employee_gid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "cbfapprovalsummary";
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        obj_getvalues = new CbfCheckerSummary()
                        {
                            cbFGidChecker = Convert.ToInt32(dt.Rows[i]["cbfheader_gid"].ToString()),
                            cbfNoChecker = dt.Rows[i]["cbfheader_cbfno"].ToString(),
                            cbfDateChecker = (dt.Rows[i]["cbfheader_date"].ToString()),
                            cbfEndDateChecker = (dt.Rows[i]["cbfheader_enddate"].ToString()),
                            cbfProjectOwnerChecker = dt.Rows[i]["cbfheader_projectowner"].ToString(),
                            cbfDevi_AmoChecker = Convert.ToDecimal(dt.Rows[i]["cbfheader_Devi_amt"].ToString()),
                            cbfAmoChecker = Convert.ToDecimal(dt.Rows[i]["cbfheader_cbfamt"].ToString()),
                            cbfDescriptionChecker = dt.Rows[i]["cbfheader_desc"].ToString(),
                            fincon_Bugchecker = dt.Rows[i]["cbfheader_isbudgeted"].ToString(),
                            cbfrequestForChecker = dt.Rows[i]["cbfheader_department"].ToString(),
                            cbfStatusChecker = dt.Rows[i]["cbfheader_status"].ToString(),
                            cbfModeChecker = dt.Rows[i]["cbfheader_mode"].ToString()
                        };

                        objSumEntity.cbfCheckersumamry.Add(obj_getvalues);
                    }
                }
                return objSumEntity;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objSumEntity;
            }
            finally
            {
                objConn.Close();
            }
        }
        public CbfSumEntity Getcbfchecker(string lsnumber)
        {
            CbfSumEntity objsum = new CbfSumEntity();
            try
            {
                getconnection();
             
                CbfCheckerHeader objgetvalues = new CbfCheckerHeader();
                DataTable dtCbfdetatails = new DataTable();
                DataSet dsCbfdetatails = new DataSet();
                CbfCheckerDetails getdetailsvalue;
                objsum.cbfCheckerDetails = new List<CbfCheckerDetails>();
                objsum.attachment = new List<Attachment>();
                objsum.attachmentCbfdetails = new List<Attachment>();
                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.BigInt).Value = lsnumber;
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectcbfeditvendor";
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dtCbfdetatails);

                if (dtCbfdetatails.Rows.Count > 0)
                {
                    objgetvalues = new CbfCheckerHeader()
                    {
                        cbfGidChecker = dtCbfdetatails.Rows[0]["cbfheader_gid"].ToString(),
                        cbfNoChecker = dtCbfdetatails.Rows[0]["cbfheader_cbfno"].ToString(),
                        cbfDateChecker = dtCbfdetatails.Rows[0]["cbfheader_date"].ToString(),
                        cbfEndDateChecker = dtCbfdetatails.Rows[0]["cbfheader_enddate"].ToString(),
                        cbfAmtChecker = Convert.ToDecimal(dtCbfdetatails.Rows[0]["cbfheader_cbfamt"].ToString()),
                        deviationAmtChecker = Convert.ToDecimal(dtCbfdetatails.Rows[0]["cbfheader_Devi_amt"].ToString()),
                        descriptionChecker = dtCbfdetatails.Rows[0]["cbfheader_desc"].ToString(),
                        justficationChecker = dtCbfdetatails.Rows[0]["cbfheader_remarks"].ToString(),
                        requeestForGidChecker = Convert.ToInt32(dtCbfdetatails.Rows[0]["cbfheader_requestfor_gid"].ToString()),
                        branchCodeGidChecker = Convert.ToInt32(dtCbfdetatails.Rows[0]["cbfheader_branch_gid"].ToString()),
                        branchTypeChecker = dtCbfdetatails.Rows[0]["cbfheader_isbranchsingle"].ToString(),
                        budgetedchecker = dtCbfdetatails.Rows[0]["cbfheader_isbudgeted"].ToString(),
                        cbfModeChecker = dtCbfdetatails.Rows[0]["cbfheader_mode"].ToString(),
                        cbfTypeChecker = dtCbfdetatails.Rows[0]["cbfheader_approvaltype"].ToString(),
                        projectOwnerGidChecker = Convert.ToInt32(dtCbfdetatails.Rows[0]["cbfheader_projectowner"].ToString()),
                        requestTypeChecker = dtCbfdetatails.Rows[0]["cbfheader_requesttype"].ToString(),
                        raiser_code = dtCbfdetatails.Rows[0]["empcode"].ToString(),
                        raiser_name = dtCbfdetatails.Rows[0]["empname"].ToString()

                    };

                    objsum.cbfCheckerHeader = objgetvalues;

                    for (int i = 0; i < dtCbfdetatails.Rows.Count; i++)
                    {
                        getdetailsvalue = new CbfCheckerDetails()
                        {


                            productCodeChecker = dtCbfdetatails.Rows[i]["prodservice_code"].ToString(),
                            cbfDetailGidChecker = Convert.ToInt32(dtCbfdetatails.Rows[i]["cbfdetails_gid"].ToString()),
                            productNameChecker = dtCbfdetatails.Rows[i]["prodservice_name"].ToString(),
                            descriptionChecker = dtCbfdetatails.Rows[i]["prodservice_description"].ToString(),
                            description1Checker = dtCbfdetatails.Rows[i]["cbfdetails_desc"].ToString(),
                            uomChecker = dtCbfdetatails.Rows[i]["uom_code"].ToString(),
                            qtyChecker = Convert.ToDecimal(dtCbfdetatails.Rows[i]["prdetails_qty"].ToString()),
                            unit_AmtChecker = Convert.ToDecimal(dtCbfdetatails.Rows[i]["pipinput_rate"].ToString()),
                            totalChecker = Convert.ToDecimal(dtCbfdetatails.Rows[i]["pipinput_costestimation"].ToString()),
                            remarksChecker = dtCbfdetatails.Rows[i]["remarks"].ToString(),
                            chartOfAccountChecker = dtCbfdetatails.Rows[i]["assetcategory_glno"].ToString(),
                            fcccChecker = Convert.ToInt32(dtCbfdetatails.Rows[i]["fccc"].ToString()),
                            budgetLineChecker = Convert.ToInt32(dtCbfdetatails.Rows[i]["budgetline"].ToString()),
                            prdetailsGidChecker = Convert.ToInt64(dtCbfdetatails.Rows[i]["prdetails_gid"].ToString())
                        };

                        objsum.cbfCheckerDetails.Add(getdetailsvalue);
                    }
                    HttpContext.Current.Session["Cbfdetailsvendor"] = dtCbfdetatails;
                    cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                    cmdQry.CommandType = CommandType.StoredProcedure;
                    cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.BigInt).Value = lsnumber;
                    cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "boqattachmentfor";
                    objDataAdapter = new SqlDataAdapter(cmdQry);
                    objDataAdapter.Fill(dsCbfdetatails);
                    if (dsCbfdetatails.Tables[0].Rows.Count > 0 || dsCbfdetatails.Tables[0].Rows.Count != null)
                    {
                        for (int i = 0; i < dsCbfdetatails.Tables[0].Rows.Count; i++)
                        {
                            Attachment attachment1 = new Attachment()
                            {
                                attachGid = dsCbfdetatails.Tables[0].Rows[i]["Sno"].ToString(),
                                fileName = dsCbfdetatails.Tables[0].Rows[i]["Documnet_Name"].ToString(),
                                attachedDate = dsCbfdetatails.Tables[0].Rows[i]["Attachment_Date"].ToString(),
                                description = dsCbfdetatails.Tables[0].Rows[i]["Document_des"].ToString(),

                            };
                            objsum.attachment.Add(attachment1);
                        }
                    }
                    if (dsCbfdetatails.Tables[1].Rows.Count > 0 || dsCbfdetatails.Tables[1].Rows.Count != null)
                    {
                        for (int i = 0; i < dsCbfdetatails.Tables[1].Rows.Count; i++)
                        {
                            Attachment attachment1 = new Attachment()
                            {
                                attachGid = dsCbfdetatails.Tables[0].Rows[i]["Sno"].ToString(),
                                fileName = dsCbfdetatails.Tables[1].Rows[i]["Documnet_Name"].ToString(),
                                attachedDate = dsCbfdetatails.Tables[1].Rows[i]["Attachment_Date"].ToString(),
                                description = dsCbfdetatails.Tables[1].Rows[i]["Document_des"].ToString(),

                            };
                            objsum.attachmentCbfdetails.Add(attachment1);
                        }
                    }
                }
                return objsum;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objsum;
            }
            finally
            {
                objConn.Close();
            }
        }

        public CbfSumEntity GetCbfPardetails(CbfDetails objCbfdetailsSavePar)
        {
            CbfSumEntity objsum = new CbfSumEntity();
            try
            {
                getconnection();
             
                CbfRaiseHeader getvalues = new CbfRaiseHeader(); ;
                DataTable dtCbfHeader = new DataTable();
                CbfDetails getdetailsvalue;
                objsum.cbfDetails = new List<CbfDetails>();
                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@cbfdetails_cbfheader_gid", SqlDbType.Int).Value = HttpContext.Current.Session["cbfgid"].ToString();
                cmdQry.Parameters.Add("@cbfdetails_prodservice_gid", SqlDbType.Int).Value = objCbfdetailsSavePar.productGroupId;
                cmdQry.Parameters.Add("@cbfdetails_parprdesc", SqlDbType.VarChar).Value = objCbfdetailsSavePar.description;
                cmdQry.Parameters.Add("@cbfdetails_uom_gid", SqlDbType.Int).Value = objCbfdetailsSavePar.uomGid;
                cmdQry.Parameters.Add("@cbfdetails_qty", SqlDbType.Int).Value = objCbfdetailsSavePar.qty;
                cmdQry.Parameters.Add("@cbfdetails_unitprice", SqlDbType.Decimal).Value = objCbfdetailsSavePar.unitAmt;
                cmdQry.Parameters.Add("@cbfdetails_totalamt", SqlDbType.Decimal).Value = objCbfdetailsSavePar.total;
                cmdQry.Parameters.Add("@cbfdetails_remarks", SqlDbType.VarChar).Value = objCbfdetailsSavePar.remarks;
                cmdQry.Parameters.Add("@cbfdetails_chartofacc", SqlDbType.VarChar).Value = objCbfdetailsSavePar.chartOfAccount;
                cmdQry.Parameters.Add("@cbfdetails_FCCC", SqlDbType.Int).Value = objCbfdetailsSavePar.fccc;
                cmdQry.Parameters.Add("@cbfdetails_budgetline", SqlDbType.Int).Value = objCbfdetailsSavePar.budgetLine;
                cmdQry.Parameters.Add("@cbfdetails_prpardel_gid", SqlDbType.Int).Value = objCbfdetailsSavePar.prdetailsGid;
                cmdQry.Parameters.Add("@cbfheader_status", SqlDbType.VarChar).Value = "1";
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "savecbfpardetails";
                int a1 = cmdQry.ExecuteNonQuery();

                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.BigInt).Value = HttpContext.Current.Session["cbfgid"].ToString();
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectcbfedit";
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dtCbfHeader);
                HttpContext.Current.Session["cbfdeailsdt"] = dtCbfHeader;
                for (int i = 0; i < dtCbfHeader.Rows.Count; i++)
                {
                    getdetailsvalue = new CbfDetails()
                    {
                        productCode = dtCbfHeader.Rows[i]["prodservice_code"].ToString(),
                        cbfDetGid = Convert.ToInt32(dtCbfHeader.Rows[i]["cbfdetails_gid"].ToString()),
                        productName = dtCbfHeader.Rows[i]["prodservice_name"].ToString(),
                        description = dtCbfHeader.Rows[i]["prodservice_description"].ToString(),
                        description1 = dtCbfHeader.Rows[i]["cbfdetails_desc"].ToString(),
                        uom = dtCbfHeader.Rows[i]["uom_code"].ToString(),
                        qty = Convert.ToInt32(dtCbfHeader.Rows[i]["cbfdetails_qty"].ToString()),
                        unitAmt = Convert.ToDecimal(dtCbfHeader.Rows[i]["cbfdetails_unitprice"].ToString()),
                        total = Convert.ToDecimal(dtCbfHeader.Rows[i]["cbfdetails_totalamt"].ToString()),
                        remarks = dtCbfHeader.Rows[i]["cbfdetails_remarks"].ToString(),
                        chartOfAccount = dtCbfHeader.Rows[i]["cbfdetails_chartofacc"].ToString(),
                        fccc = Convert.ToInt32(dtCbfHeader.Rows[i]["cbfdetails_FCCC"].ToString()),
                        budgetLine = Convert.ToInt32(dtCbfHeader.Rows[i]["cbfdetails_budgetline"].ToString()),
                        prdetailsGid = Convert.ToInt64(dtCbfHeader.Rows[i]["cbfdetails_prpardel_gid"].ToString())
                    };

                    objsum.cbfDetails.Add(getdetailsvalue);

                }
                HttpContext.Current.Session["cbfdeails"] = objsum.cbfDetails;

                return objsum;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objsum;
            }
            finally
            {
                objConn.Close();
            }

        }

        public void DeleteProductServiceDetails(string objDelete)
        {
            try
            {

                string[,] wherecond = new string[,]
	        {
                {"cbfdetails_gid", objDelete.ToString()}
            };
                string[,] column = new string[,]
	       {
                 {"cbfdetails_isremoved", "Y"}
	            
           };
                string tblname = "fb_trn_tcbfdetails";
                string deletetbl = objCommonIUD.DeleteCommon(column, wherecond, tblname);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
             
            }
            finally
            {
                objConn.Close();
            }
        }
        public void DeletePARdetailsLine(string objDelete)
        {
            try
            {

                string[,] wherecond = new string[,]
	        {
                {"pardetails_gid", objDelete.ToString()}
            };
                string[,] column = new string[,]
	       {
                 {"pardetails_isremoved", "Y"}
	            
           };
                string tblname = "fb_trn_tpardetails";
                string deletetbl = objCommonIUD.DeleteCommon(column, wherecond, tblname);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
              
            }
            finally
            {
                objConn.Close();
            }
        }
        public void DeletePARapproval(string objDelete)
        {
            try
            {

                string[,] wherecond = new string[,]
	        {
                {"parapprover_gid", objDelete.ToString()}
            };
                string[,] column = new string[,]
	       {
                 {"parapprover_isremoved", "Y"}
	            
           };
                string tblname = "fb_trn_tparapprover";
                string deletetbl = objCommonIUD.DeleteCommon(column, wherecond, tblname);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            
            }
            finally
            {
                objConn.Close();
            }
        }
        public void DeleteAttachment(string objDelete)
        {
            try
            {
                string[,] wherecond;

                wherecond = new string[,]
	        {
              {"attachment_gid", objDelete.ToString()}
            };

                string[,] column = new string[,]
	       {
                 {"attachment_isremoved", "Y"}
	            
           };
                string tblname = "iem_trn_tattachment";
                string deletetbl = objCommonIUD.DeleteCommon(column, wherecond, tblname);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
               
            }
            finally
            {
                objConn.Close();
            }
        }
        public void InsertConversation(RaiserTicket objTicket)
        {
            try
            {
                string date = DateTime.Now.ToString();
                string insertcommend = "";
                string[,] codes = new string[,]
                                {
                                {"conversation_fromid", objTicket.fromID.ToString()},
                                {"conversation_from_date", "sysdatetime()"},
                                {"conversation_message",objTicket.message.ToString()},
                                {"conversation_toid",objTicket.toID.ToString()},
                                {"conversation_module_name","CBF"},
                                {"conversation_ref_gid",HttpContext.Current.Session["Cbfnochecker"].ToString()},
                                };
                string tname = "fb_trn_tconversation";
                insertcommend = objCommonIUD.InsertCommon(codes, tname);

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
               
            }
            finally
            {
                objConn.Close();
            }
        }
        public IEnumerable<RaiserTicket> SelectDate(RaiserTicket objTicket)
        {
            List<RaiserTicket> SelectDate12 = new List<RaiserTicket>();
            try
            {
                getconnection();

                string strquey = "select * from fb_trn_tconversation where  (conversation_fromid='" + objTicket.fromID + "' or conversation_fromid='" + objTicket.toID + "')  and (conversation_toid='" + objTicket.toID + "' or conversation_toid='" + objTicket.fromID + "') order by conversation_gid ";
                cmdQry = new SqlCommand(strquey, objConn);
                cmdQry.CommandType = CommandType.Text;
                objDataAdapter = new SqlDataAdapter(cmdQry);
                DataTable dtq = new DataTable();
                objDataAdapter.Fill(dtq);

              
                RaiserTicket raiser;
                if (dtq.Rows.Count > 0)
                {
                    for (int i = 0; i < dtq.Rows.Count; i++)
                    {
                        raiser = new RaiserTicket();
                        raiser.Fromemp = objCmnFunctions.GetEmployeeName(Convert.ToInt32(dtq.Rows[i]["conversation_fromid"]));
                        raiser.message = dtq.Rows[i]["conversation_message"].ToString();
                        raiser.date = objCmnFunctions.convertoDateTimeString(dtq.Rows[i]["conversation_from_date"].ToString());
                        raiser.time = objCmnFunctions.convertotime(dtq.Rows[i]["conversation_from_date"].ToString());
                        SelectDate12.Add(raiser);
                    }
                }

                return SelectDate12;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return SelectDate12;
            }
            finally
            {
                objConn.Close();
            }
        }
        public List<CbfCheckerHeader> GetSupervisor()
        {
            List<CbfCheckerHeader> objproduct = new List<CbfCheckerHeader>();
            try
            {
            
                CbfCheckerHeader objraiser;
                getconnection();

                DataTable dt = new DataTable();
                cmdQry = new SqlCommand("pr_fb_mst_superviosr", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    objraiser = new CbfCheckerHeader()
                    {
                        supervisorGidChecker = Convert.ToInt32(row["roleemployee_employee_gid"].ToString()),
                        SupervisornameChecker = row["employee_name"].ToString(),

                    };
                    objproduct.Add(objraiser);
                }

                return objproduct;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objproduct;
            }
            finally
            {
                objConn.Close();
            }
        }
        //public string QueueProcess(quenue queue)
        //{
        //    try
        //    {
        //        string insertcommend = "";
        //        getconnection();
        //        string result = "";
        //        int queue_gid = 0;

        //        if (queue.actionstatus == "supervisor")
        //        {
        //            cmdQry = new SqlCommand("pr_fb_trn_CbfApprovalQueue", objConn);
        //            cmdQry.CommandType = CommandType.StoredProcedure;
        //            // string SRT = HttpContext.Current.Session["Cbfnochecker"].ToString();
        //            cmdQry.Parameters.Add("@refgid", SqlDbType.Int).Value = HttpContext.Current.Session["Cbfnochecker"];
        //            cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
        //            cmdQry.Parameters.Add("@actionremark", SqlDbType.VarChar).Value = queue.actionremark;
        //            cmdQry.Parameters.Add("@actionName", SqlDbType.VarChar).Value = "cbfqueuegid";
        //            int a1 = cmdQry.ExecuteNonQuery();
        //        }
        //        if (queue.actionstatus == "checker")
        //        {
        //            cmdQry = new SqlCommand("pr_fb_trn_CbfApprovalQueue", objConn);
        //            cmdQry.CommandType = CommandType.StoredProcedure;
        //            // string SRT = HttpContext.Current.Session["Cbfnochecker"].ToString();
        //            cmdQry.Parameters.Add("@refgid", SqlDbType.Int).Value = HttpContext.Current.Session["Cbfnochecker"];
        //            cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
        //            cmdQry.Parameters.Add("@actionremark", SqlDbType.VarChar).Value = queue.actionremark;
        //            cmdQry.Parameters.Add("@actionName", SqlDbType.VarChar).Value = "insertcbfchecker";
        //            int a1 = cmdQry.ExecuteNonQuery();
        //        }


        //        cmdQry = new SqlCommand("pr_fb_trn_ApprovalQuenu", objConn);
        //        cmdQry.CommandType = CommandType.StoredProcedure;
        //        string SRT = HttpContext.Current.Session["Cbfnochecker"].ToString();
        //        cmdQry.Parameters.Add("@refgid", SqlDbType.Int).Value = HttpContext.Current.Session["Cbfnochecker"];
        //        cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
        //        cmdQry.Parameters.Add("@requesttype", SqlDbType.VarChar).Value = "CBF";
        //        cmdQry.Parameters.Add("@actionremark", SqlDbType.VarChar).Value = queue.actionremark;
        //        //if (queue.queueto != null)
        //        //{

        //        //    cmdQry.Parameters.Add("@queueto", SqlDbType.Int).Value = queue.queueto;
        //        //}
        //        // cmdQry.Parameters.Add("@skipflag", SqlDbType.Int).Value = queue.skipflag;
        //        int a3 = cmdQry.ExecuteNonQuery();


        //        if (a3 != -1)
        //        {
        //            //cmdQry = new SqlCommand("pr_fb_trn_CbfApprovalQueue", objConn);
        //            //cmdQry.CommandType = CommandType.StoredProcedure;
        //            //cmdQry.Parameters.Add("@refgid", SqlDbType.Int).Value = HttpContext.Current.Session["Cbfnochecker"];
        //            //cmdQry.Parameters.Add("@actionName", SqlDbType.VarChar).Value = "getqueuegid";
        //            //queue_gid = (int)cmdQry.ExecuteScalar();

        //            //if (queue.actionstatus == "supervisor")
        //            //{
        //            //    string[,] codesq = new string[,]
        //            //                {

        //            //                     {"queue_isremoved","N" }
        //            //                 };
        //            //    string[,] whreq = new string[,]
        //            //                {

        //            //                    {"queue_ref_gid",HttpContext.Current.Session["Cbfnochecker"].ToString()},
        //            //                    {"queue_gid",queue_gid.ToString()},
        //            //                    {"queue_isremoved","Y" }
        //            //               };
        //            //    string tnameq = "iem_trn_tqueue";
        //            //    string insertcommendq = objCommonIUD.UpdateCommon(codesq, whreq, tnameq);
        //            //}
        //            string[,] codes1 = new string[,]            
        //            {
        //                {"cbfheader_status",queue.cbfstatus},
                       
        //            };
        //            string[,] whrcol = new string[,]
        //             {
        //                  {"cbfheader_gid",HttpContext.Current.Session["Cbfnochecker"].ToString()}
                          
        //             };
        //            string tname1 = "fb_trn_tcbfheader";
        //            insertcommend = objCommonIUD.UpdateCommon(codes1, whrcol, tname1);
        //            if (insertcommend == "Success" && queue.cbfstatus == "5")
        //            {
        //                int queue_type = Convert.ToInt32(ConfigurationManager.AppSettings["cbfreopenmakergid"].ToString());
        //                int queue_ref_flag = Convert.ToInt32(ConfigurationManager.AppSettings["cbfrefFlag"].ToString());
        //                string[,] code = new string[,]
        //                        {
        //                        {"queue_date", "sysdatetime()"},
        //                        {"queue_ref_flag",queue_ref_flag.ToString()},
        //                        {"queue_ref_gid",HttpContext.Current.Session["Cbfnochecker"].ToString()},
        //                        {"queue_from",objCmnFunctions.GetLoginUserGid().ToString()},
        //                        {"queue_to_type", "G"},
        //                        {"queue_to", queue_type.ToString()}
        //                        };
        //                string tbname = "iem_trn_tqueue";
        //                result = objCommonIUD.InsertCommon(code, tbname);
        //            }
        //        }
        //        return a3.ToString();


        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //        return "";
        //    }
        //    finally
        //    {
        //        objConn.Close();
        //    }
        //}
        public string QueueProcess(quenue queue)
        {
            try
            {
                string insertcommend = "";
                getconnection();
                string result = "";
                int queue_ref_flag = 0;
                int a1 = 0;
                int queue_gid = 0;

                if (queue.actionstatus == "supervisor")
                {
                    cmdQry = new SqlCommand("pr_fb_trn_CbfApprovalQueue", objConn);
                    cmdQry.CommandType = CommandType.StoredProcedure;
                    // string SRT = HttpContext.Current.Session["Cbfnochecker"].ToString();
                    cmdQry.Parameters.Add("@refgid", SqlDbType.Int).Value = HttpContext.Current.Session["Cbfnochecker"];
                    cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                    cmdQry.Parameters.Add("@actionremark", SqlDbType.VarChar).Value = queue.actionremark;
                    cmdQry.Parameters.Add("@actionName", SqlDbType.VarChar).Value = "cbfqueuegid";
                    a1 = cmdQry.ExecuteNonQuery();
                }
                if (queue.actionstatus == "checker")
                {
                    cmdQry = new SqlCommand("pr_fb_trn_CbfApprovalQueue", objConn);
                    cmdQry.CommandType = CommandType.StoredProcedure;
                    // string SRT = HttpContext.Current.Session["Cbfnochecker"].ToString();
                    cmdQry.Parameters.Add("@refgid", SqlDbType.Int).Value = HttpContext.Current.Session["Cbfnochecker"];
                    cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                    cmdQry.Parameters.Add("@actionremark", SqlDbType.VarChar).Value = queue.actionremark;
                    cmdQry.Parameters.Add("@actionName", SqlDbType.VarChar).Value = "insertcbfchecker";
                    a1 = cmdQry.ExecuteNonQuery();

                    cmdQry = new SqlCommand("pr_fb_trn_ApprovalQuenu", objConn);
                    cmdQry.CommandType = CommandType.StoredProcedure;
                    string SRT = HttpContext.Current.Session["Cbfnochecker"].ToString();
                    cmdQry.Parameters.Add("@refgid", SqlDbType.Int).Value = HttpContext.Current.Session["Cbfnochecker"];
                    cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                    cmdQry.Parameters.Add("@requesttype", SqlDbType.VarChar).Value = "CBF";
                    cmdQry.Parameters.Add("@actionremark", SqlDbType.VarChar).Value = queue.actionremark;

                    a1 = cmdQry.ExecuteNonQuery();
                }

                //if (queue.queueto != null)
                //{

                //    cmdQry.Parameters.Add("@queueto", SqlDbType.Int).Value = queue.queueto;
                //}
                // cmdQry.Parameters.Add("@skipflag", SqlDbType.Int).Value = queue.skipflag;
              


                if (a1 != -1)
                {
                    //cmdQry = new SqlCommand("pr_fb_trn_CbfApprovalQueue", objConn);
                    //cmdQry.CommandType = CommandType.StoredProcedure;
                    //cmdQry.Parameters.Add("@refgid", SqlDbType.Int).Value = HttpContext.Current.Session["Cbfnochecker"];
                    //cmdQry.Parameters.Add("@actionName", SqlDbType.VarChar).Value = "getqueuegid";
                    //queue_gid = (int)cmdQry.ExecuteScalar();
                    queue_ref_flag = Convert.ToInt32(ConfigurationManager.AppSettings["cbfrefFlag"].ToString());
                    if (queue.actionstatus == "supervisor")
                    {
                        string[,] codesq = new string[,]
                                    {

                                         {"queue_isremoved","Y" }
                                     };
                        string[,] whreq = new string[,]
                                    {

                                        {"queue_ref_gid",HttpContext.Current.Session["Cbfnochecker"].ToString()},
                                        {"queue_gid",queue_gid.ToString()},
                                        {"queue_ref_flag",Convert.ToString(queue_ref_flag)},
                                        {"queue_isremoved","N" }
                                   };
                        string tnameq = "iem_trn_tqueue";
                        string insertcommendq = objCommonIUD.UpdateCommon(codesq, whreq, tnameq);
                    }
                    string[,] codes1 = new string[,]            
	                {
                        {"cbfheader_status",queue.cbfstatus},
                       
                    };
                    string[,] whrcol = new string[,]
	                 {
                          {"cbfheader_gid",HttpContext.Current.Session["Cbfnochecker"].ToString()}
                          
                     };
                    string tname1 = "fb_trn_tcbfheader";
                    insertcommend = objCommonIUD.UpdateCommon(codes1, whrcol, tname1);
                    if (insertcommend == "Success" && queue.cbfstatus == "5")
                    {
                        int queue_type = Convert.ToInt32(ConfigurationManager.AppSettings["cbfreopenmakergid"].ToString());
                       // int queue_ref_flag = Convert.ToInt32(ConfigurationManager.AppSettings["cbfrefFlag"].ToString());
                        string[,] code = new string[,]
                                {
                                {"queue_date", "sysdatetime()"},
                                {"queue_ref_flag",queue_ref_flag.ToString()},
                                {"queue_ref_gid",HttpContext.Current.Session["Cbfnochecker"].ToString()},
                                {"queue_from",objCmnFunctions.GetLoginUserGid().ToString()},
                                {"queue_to_type", "G"},
                                {"queue_to", queue_type.ToString()}
                                };
                        string tbname = "iem_trn_tqueue";
                        result = objCommonIUD.InsertCommon(code, tbname);
                    }
                }
                return a1.ToString();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
        }
        public string QueueProcessreject(quenue queue)
        {
            try
            {
                string insertcommendq = string.Empty;
                //getconnection();
                //string result = "";
                //cmdQry = new SqlCommand("pr_fb_trn_ApprovalQuenu", objConn);
                //cmdQry.CommandType = CommandType.StoredProcedure;
                //cmdQry.Parameters.Add("@refgid", SqlDbType.Int).Value = HttpContext.Current.Session["Cbfnochecker"];
                //cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                //cmdQry.Parameters.Add("@requesttype", SqlDbType.VarChar).Value = "CBF";
                //cmdQry.Parameters.Add("@actionremark", SqlDbType.VarChar).Value = queue.actionremark;
                //if (queue.queueto != null)
                //{

                //    cmdQry.Parameters.Add("@queueto", SqlDbType.Int).Value = queue.queueto;
                //}
                //cmdQry.Parameters.Add("@actionstatus", SqlDbType.Int).Value = 2;
                //cmdQry.Parameters.Add("@skipflag", SqlDbType.Int).Value = queue.skipflag;
                //int a3 = cmdQry.ExecuteNonQuery();

                //return a3.ToString();
                string[,] codesq = new string[,]
                                    {
                                         {"queue_action_date","sysdatetime()"},
                                         {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString() },
                                         {"queue_action_status","2" },
                                         {"queue_action_remark",queue.actionremark},
                                         {"queue_isremoved","Y" },
                                         {"queue_action_for","R"}
                                     };
                string[,] whreq = new string[,]
                                    {
                                     
                                        {"queue_ref_gid",HttpContext.Current.Session["Cbfnochecker"].ToString()},
                                        {"queue_ref_flag","2"},
                                        {"queue_isremoved","N" }
                                   };
                string tnameq = "iem_trn_tqueue";
                insertcommendq = objCommonIUD.UpdateCommon(codesq, whreq, tnameq);
                if (insertcommendq == "Success")
                {
                    string[,] column = new string[,]
                                    {
                                         {"cbfheader_status","6"},
                                          {"cbfheader_closed","N"},
                                          {"cbfheader_iscancelled","N"}
                                     };
                    string[,] wherecol = new string[,]
                                    {
                                     
                                        {"cbfheader_gid",HttpContext.Current.Session["Cbfnochecker"].ToString()}
                                    };
                    string tblname = "fb_trn_tcbfheader";
                    insertcommendq = objCommonIUD.UpdateCommon(column, wherecol, tblname);
                }
                return insertcommendq;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
        }
        public string Cancel(CbfRaiseHeader cbfheader)
        {
            try
            {
                string date = DateTime.Now.ToString();
                string insertcommend = "";
                string[,] codes = new string[,]
                                {
                                {"cbfcancellation_cbfhead_gid",HttpContext.Current.Session["cbfgid"].ToString()},
                                {"cbfcancellation_mak_gid", objCmnFunctions.GetLoginUserGid().ToString()},
                                {"cbfcancellation_mak_remarks",cbfheader.justfication.ToString()},
                                {"cbfcancellation_mak_date","sysdatetime()"},
                                };
                string tname = "fb_trn_tcbfcancellation";
                insertcommend = objCommonIUD.InsertCommon(codes, tname);
                if (insertcommend == "success")
                {
                    string[,] codes1 = new string[,]            
	                {
                        {"cbfheader_iscancelled","M"},
                        {"cbfheader_status","7"},
                    };
                    string[,] whrcol = new string[,]
	                 {
                          {"cbfheader_gid",HttpContext.Current.Session["cbfgid"].ToString()}
                          
                     };
                    string tname1 = "fb_trn_tcbfheader";
                    insertcommend = objCommonIUD.UpdateCommon(codes1, whrcol, tname1);

                    //praveen
                    if (insertcommend == "Success")
                    {
                        int queue_type = Convert.ToInt32(ConfigurationManager.AppSettings["cbfcancelgroupgid"].ToString());
                        int queue_ref_flag = Convert.ToInt32(ConfigurationManager.AppSettings["cbfrefFlag"].ToString());
                        string[,] code = new string[,]
                                {
                                {"queue_date", "sysdatetime()"},
                                {"queue_ref_flag",queue_ref_flag.ToString()},
                                {"queue_ref_gid",HttpContext.Current.Session["cbfgid"].ToString()},
                                {"queue_from",objCmnFunctions.GetLoginUserGid().ToString()},
                                {"queue_to_type", "G"},
                                {"queue_action_for", "A"},
                                {"queue_to", queue_type.ToString()},
                                };
                        string tbname = "iem_trn_tqueue";
                        string queue_result = objCommonIUD.InsertCommon(code, tbname);
                    }
                }
                return insertcommend;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }

        }
        public string Closure(CbfRaiseHeader cbfheader)
        {
            try
            {
                string date = DateTime.Now.ToString();
                string insertcommend = "";
                string[,] codes = new string[,]
                                {
                                {"cbfclosure_cbfheader_gid",HttpContext.Current.Session["Cbfnochecker"].ToString()},
                                {"cbfclosure_raiser_gid", objCmnFunctions.GetLoginUserGid().ToString()},
                                {"cbfclosure_date",objCmnFunctions.convertoDateTimeString(cbfheader.cbfDate.ToString())},
                                {"cbfclosure_remarks",cbfheader.justfication.ToString()},
                                };
                string tname = "fb_trn_tcbfclosure";
                insertcommend = objCommonIUD.InsertCommon(codes, tname);
                if (insertcommend == "success")
                {
                    string[,] codes1 = new string[,]            
	                {
                        {"cbfheader_closed","M"},
                       
                    };
                    string[,] whrcol = new string[,]
	                 {
                          {"cbfheader_gid",HttpContext.Current.Session["Cbfnochecker"].ToString()}
                          
                     };
                    string tname1 = "fb_trn_tcbfheader";
                    insertcommend = objCommonIUD.UpdateCommon(codes1, whrcol, tname1);
                    if (insertcommend == "Success")
                    {
                        int queue_type = Convert.ToInt32(ConfigurationManager.AppSettings["cbfclosuregroupid"].ToString());
                        int queue_ref_flag = Convert.ToInt32(ConfigurationManager.AppSettings["cbfrefFlag"].ToString());
                        string[,] column = new string[,]
	             {
                
                                {"queue_date", "sysdatetime()"},
                                {"queue_ref_flag",queue_ref_flag.ToString()},
                                {"queue_ref_gid", HttpContext.Current.Session["Cbfnochecker"].ToString()},
                                {"queue_from",objCmnFunctions.GetLoginUserGid().ToString()},
                                {"queue_action_for","A"},
                                {"queue_to_type", "G"},
                                {"queue_to", queue_type.ToString()}
                 };
                        string tblname = "iem_trn_tqueue";
                        string result = objCommonIUD.InsertCommon(column, tblname);
                    }
                }
                return insertcommend;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }

        }

        public string Reopen(CbfRaiseHeader cbfheader)
        {
            try
            {
                string date = DateTime.Now.ToString();
                string insertcommend = "";
                string[,] codes = new string[,]
                                {
                                {"cbfclosure_cbfheader_gid",HttpContext.Current.Session["Cbfnochecker"].ToString()},
                                {"cbfclosure_reopenmak_gid", objCmnFunctions.GetLoginUserGid().ToString()},
                                {"cbfclosure_reopen_date",objCmnFunctions.convertoDateTimeString(cbfheader.cbfDate.ToString())},
                                {"cbfclosure_reopenremarks",cbfheader.justfication.ToString()},
                                {"cbfclosure_status","O"},
                                };

                string[,] whrcol1 = new string[,]
	                 {
                          {"cbfclosure_gid",HttpContext.Current.Session["Cancellationgid"] .ToString()},
                           {"cbfclosure_cbfheader_gid",HttpContext.Current.Session["Cbfnochecker"].ToString()},
                          
                     };
                string tname = "fb_trn_tcbfclosure";
                insertcommend = objCommonIUD.UpdateCommon(codes, whrcol1, tname);
                if (insertcommend == "Success")
                {
                    string[,] codes1 = new string[,]            
	                {
                        {"cbfheader_closed","R"},
                       
                    };
                    string[,] whrcol = new string[,]
	                 {
                          {"cbfheader_gid",HttpContext.Current.Session["Cbfnochecker"].ToString()}
                          
                     };
                    string tname1 = "fb_trn_tcbfheader";
                    insertcommend = objCommonIUD.UpdateCommon(codes1, whrcol, tname1);
                    if (insertcommend == "Success")
                    {
                        int queue_type = Convert.ToInt32(ConfigurationManager.AppSettings["cbfreclosuregid"].ToString());
                        int queue_ref_flag = Convert.ToInt32(ConfigurationManager.AppSettings["cbfrefFlag"].ToString());
                        string[,] codes2 = new string[,]
                                {
                                {"queue_date", "sysdatetime()"},
                                {"queue_ref_flag",queue_ref_flag.ToString()},
                                {"queue_ref_gid",HttpContext.Current.Session["Cbfnochecker"].ToString()},
                                {"queue_from",objCmnFunctions.GetLoginUserGid().ToString()},
                                {"queue_to_type", "G"},
                                {"queue_to", queue_type.ToString()},
                                {"queue_action_for","A"}
                                };
                        string tbname = "iem_trn_tqueue";
                        string queue_result = objCommonIUD.InsertCommon(codes2, tbname);
                    }

                }
                return insertcommend;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }

        }

        public string Delete(CbfRaiseHeader cbfheader)
        {
            try
            {
                string deletetbl = "";
                getconnection();

                string Strquery = "select * from fb_trn_tcbfheader inner join fb_trn_tcbfdetails on ";
                Strquery += " cbfheader_gid=cbfdetails_cbfhead_gid inner join";
                Strquery += " fb_trn_tpodetails on podetails_cbfdet_gid=cbfdetails_gid where cbfheader_gid='" + HttpContext.Current.Session["cbfgid"].ToString() + "'";

                cmdQry = new SqlCommand(Strquery, objConn);
                cmdQry.CommandType = CommandType.Text;
                objDataAdapter = new SqlDataAdapter(cmdQry);
                DataTable Objdt = new DataTable();
                objDataAdapter.Fill(Objdt);
                if (Objdt.Rows.Count > 0)
                {
                    deletetbl = "Already Used";
                }
                else
                {
                    string[,] wherecond = new string[,]
	        {
                {"cbfheader_gid", HttpContext.Current.Session["cbfgid"].ToString()}
            };
                    string[,] column = new string[,]
	       {
                 {"cbfheader_isremoved", "Y"}
	            
           };
                    string tblname = "fb_trn_tcbfheader";
                    deletetbl = objCommonIUD.DeleteCommon(column, wherecond, tblname);
                }

                return deletetbl;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
        }
        public string Approvalcancellation(string remarks)
        {
            try
            {
                getconnection();
                string result = "";
                cmdQry = new SqlCommand("pr_fb_trn_tcbfcancellation", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@cbfcancellation_chk_gid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmdQry.Parameters.Add("@cbfcancellation_chk_status", SqlDbType.VarChar).Value = "A";
                cmdQry.Parameters.Add("@cbfcancellation_chk_remarks", SqlDbType.VarChar).Value = remarks;
                cmdQry.Parameters.Add("@cbfcancellation_gid", SqlDbType.Int).Value = HttpContext.Current.Session["Cancellationgid"];
                cmdQry.Parameters.Add("@cbfcancellation_cbfhead_gid", SqlDbType.Int).Value = HttpContext.Current.Session["Cbfnochecker"];
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "CBFCancellationchecker";
                int a3 = cmdQry.ExecuteNonQuery();
                if (a3 == 2)
                {
                    int cbfrefFlag = Convert.ToInt32(ConfigurationManager.AppSettings["cbfrefFlag"].ToString());
                    string[,] wherecond = new string[,]
	              {
                         {"queue_ref_gid",HttpContext.Current.Session["Cbfnochecker"].ToString()},
                         {"queue_ref_flag",cbfrefFlag.ToString()},
                         {"queue_isremoved","N"}
                      };
                    string[,] column = new string[,]
	             {
                    {"queue_isremoved", "Y"},
                    {"queue_action_date","sysdatetime()"},
                    {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                    {"queue_action_for","A"},
                    {"queue_action_status","1"},
                    {"queue_action_remark",(string.IsNullOrEmpty(remarks) ? string.Empty : remarks.ToString())},
                 };
                    string tblname = "iem_trn_tqueue";
                    result = objCommonIUD.UpdateCommon(column, wherecond, tblname);
                }
                return a3.ToString();
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }

        }
        public string Rejectcancellation(string remarks)
        {
            try
            {
                getconnection();
                string result = "";
                cmdQry = new SqlCommand("pr_fb_trn_tcbfcancellation", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@cbfcancellation_chk_gid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmdQry.Parameters.Add("@cbfcancellation_chk_status", SqlDbType.VarChar).Value = "R";
                cmdQry.Parameters.Add("@cbfcancellation_chk_remarks", SqlDbType.VarChar).Value = remarks;
                cmdQry.Parameters.Add("@cbfcancellation_gid", SqlDbType.VarChar).Value = HttpContext.Current.Session["Cancellationgid"];
                cmdQry.Parameters.Add("@cbfcancellation_cbfhead_gid", SqlDbType.VarChar).Value = HttpContext.Current.Session["Cbfnochecker"];
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "CBFCancellationchecker";
                int a3 = cmdQry.ExecuteNonQuery();
                if (a3 == 2)
                {
                 //   int cbfrefFlag = Convert.ToInt32(ConfigurationManager.AppSettings["cbfrefFlag"].ToString());
                 //   string[,] wherecond = new string[,]
                 // {
                 //        {"queue_ref_gid",HttpContext.Current.Session["Cbfnochecker"].ToString()},
                 //        {"queue_ref_flag",cbfrefFlag.ToString()},
                 //        {"queue_isremoved","N"}
                 //     };
                 //   string[,] column = new string[,]
                 //{
                
                 //   {"queue_action_date","sysdatetime()"},
                 //   {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                 //   {"queue_action_for","R"},
                 //   {"queue_action_status","2"},
                 //   {"queue_action_remark",(string.IsNullOrEmpty(remarks) ? string.Empty : remarks.ToString())},
                 //};
                 //   string tblname = "iem_trn_tqueue";
                 //   result = objCommonIUD.UpdateCommon(column, wherecond, tblname);
                    getconnection();
                    cmdQry = new SqlCommand("pr_fb_trn_rejectupdates", objConn);
                    cmdQry.CommandType = CommandType.StoredProcedure;
                    cmdQry.Parameters.Add("@actionby", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid();
                    cmdQry.Parameters.Add("@refname", SqlDbType.VarChar).Value = "CBF";
                    cmdQry.Parameters.Add("@actionremark", SqlDbType.VarChar).Value = remarks;
                    cmdQry.Parameters.Add("@refgid", SqlDbType.VarChar).Value = HttpContext.Current.Session["Cbfnochecker"];
                    cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "cbfcancel";
                    cmdQry.ExecuteNonQuery();
                }
                return a3.ToString();
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }

        }

        //-----Closure Summary---//
        public List<CbfCheckerSummary> GetCbfClosureSummary()
        {
            List<CbfCheckerSummary> ObjClosure = new List<CbfCheckerSummary>();
            try
            {
                getconnection();

                CbfCheckerSummary obj_getvalues;
             
                DataTable dt = new DataTable();

                cmdQry = new SqlCommand("pr_fb_trn_tcbfcancellation", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "Cbfclosuresummary";
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        obj_getvalues = new CbfCheckerSummary()
                        {
                            cbFGidChecker = Convert.ToInt32(dt.Rows[i]["cbfheader_gid"].ToString()),
                            cbfNoChecker = dt.Rows[i]["cbfheader_cbfno"].ToString(),
                            cbfDateChecker = (dt.Rows[i]["cbfheader_date"].ToString()),
                            cbfEndDateChecker = (dt.Rows[i]["cbfheader_enddate"].ToString()),
                            cbfProjectOwnerChecker = dt.Rows[i]["cbfheader_projectowner"].ToString(),
                            cbfDevi_AmoChecker = Convert.ToDecimal(dt.Rows[i]["cbfheader_Devi_amt"].ToString()),
                            cbfAmoChecker = Convert.ToDecimal(dt.Rows[i]["cbfheader_cbfamt"].ToString()),
                            cbfDescriptionChecker = dt.Rows[i]["cbfheader_desc"].ToString(),
                            fincon_Bugchecker = dt.Rows[i]["cbfheader_isbudgeted"].ToString(),
                            cbfrequestForChecker = dt.Rows[i]["cbfheader_department"].ToString(),
                            cbfModeChecker = dt.Rows[i]["cbfheader_mode"].ToString(),

                        };

                        ObjClosure.Add(obj_getvalues);
                    }
                }
                return ObjClosure;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return ObjClosure;
            }
            finally
            {
                objConn.Close();
            }
        }
        public CbfSumEntity GetCbfClosureSummary1()
        {
            CbfSumEntity objSumEntity = new CbfSumEntity();

            List<CbfCheckerSummary> ObjClosure = new List<CbfCheckerSummary>();
            try
            {
                getconnection();

                CbfCheckerSummary obj_getvalues;

                DataTable dt = new DataTable();

                cmdQry = new SqlCommand("pr_fb_trn_tcbfcancellation", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "Cbfclosuresummary";
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        obj_getvalues = new CbfCheckerSummary()
                        {
                            cbFGidChecker = Convert.ToInt32(dt.Rows[i]["cbfheader_gid"].ToString()),
                            cbfNoChecker = dt.Rows[i]["cbfheader_cbfno"].ToString(),
                            cbfDateChecker = (dt.Rows[i]["cbfheader_date"].ToString()),
                            cbfEndDateChecker = (dt.Rows[i]["cbfheader_enddate"].ToString()),
                            cbfProjectOwnerChecker = dt.Rows[i]["cbfheader_projectowner"].ToString(),
                            cbfDevi_AmoChecker = Convert.ToDecimal(dt.Rows[i]["cbfheader_Devi_amt"].ToString()),
                            cbfAmoChecker = Convert.ToDecimal(dt.Rows[i]["cbfheader_cbfamt"].ToString()),
                            cbfDescriptionChecker = dt.Rows[i]["cbfheader_desc"].ToString(),
                            fincon_Bugchecker = dt.Rows[i]["cbfheader_isbudgeted"].ToString(),
                            cbfrequestForChecker = dt.Rows[i]["cbfheader_department"].ToString(),
                            cbfModeChecker = dt.Rows[i]["cbfheader_mode"].ToString(),

                        };

                        ObjClosure.Add(obj_getvalues);
                    }
                }
                objSumEntity.GetCbfClosureSummary = ObjClosure;
                return objSumEntity;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objSumEntity;
            }
            finally
            {
                objConn.Close();
            }
        }
        public string Approvalclosure(string remarks)
        {
            try
            {
                getconnection();
                string result = "";
                cmdQry = new SqlCommand("pr_fb_trn_tcbfcancellation", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@cbfclosure_chk_gid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmdQry.Parameters.Add("@cbfclosure_chk_status", SqlDbType.VarChar).Value = "A";
                cmdQry.Parameters.Add("@cbfclosure_chk_remarks", SqlDbType.VarChar).Value = remarks;
                cmdQry.Parameters.Add("@cbfclosure_gid", SqlDbType.Int).Value = HttpContext.Current.Session["Cancellationgid"];
                cmdQry.Parameters.Add("@cbfclosure_cbfhead_gid", SqlDbType.Int).Value = HttpContext.Current.Session["Cbfnochecker"];
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "CBFclosurechecker";
                int a3 = cmdQry.ExecuteNonQuery();
                if (a3 == 2)
                {
                    int cbfrefFlag = Convert.ToInt32(ConfigurationManager.AppSettings["cbfrefFlag"].ToString());
                    string[,] wherecond = new string[,]
	              {
                         {"queue_ref_gid",HttpContext.Current.Session["Cbfnochecker"].ToString()},
                         {"queue_ref_flag",cbfrefFlag.ToString()},
                         {"queue_isremoved","N"}
                      };
                    string[,] column = new string[,]
	             {
                    {"queue_isremoved", "Y"},
                    {"queue_action_date","sysdatetime()"},
                    {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                    {"queue_action_for","A"},
                    {"queue_action_status","1"},
                    {"queue_action_remark",(string.IsNullOrEmpty(remarks) ? string.Empty : remarks.ToString())}
                 };
                    string tblname = "iem_trn_tqueue";
                    result = objCommonIUD.UpdateCommon(column, wherecond, tblname);
                    //if (result == "Success")
                    //{
                    //    int queue_type = Convert.ToInt32(ConfigurationManager.AppSettings["cbfreopenmakergid"].ToString());
                    //    int queue_ref_flag = Convert.ToInt32(ConfigurationManager.AppSettings["cbfrefFlag"].ToString());
                    //    string[,] codes2 = new string[,]
                    //            {
                    //            {"queue_date", "sysdatetime()"},
                    //            {"queue_ref_flag",queue_ref_flag.ToString()},
                    //            {"queue_ref_gid",HttpContext.Current.Session["Cbfnochecker"].ToString()},
                    //            {"queue_from",objCmnFunctions.GetLoginUserGid().ToString()},
                    //            {"queue_to_type", "G"},
                    //            {"queue_to", queue_type.ToString()},
                    //            {"queue_action_for","A"}
                    //            };
                    //    string tbname = "iem_trn_tqueue";
                    //    string queue_result = objCommonIUD.InsertCommon(codes2, tbname);
                    //}
                }
                return a3.ToString();
            }


            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }

        }
        public string Rejectclosure(string remarks)
        {
            try
            {
                getconnection();
                string result = "";
                cmdQry = new SqlCommand("pr_fb_trn_tcbfcancellation", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@cbfclosure_chk_gid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmdQry.Parameters.Add("@cbfclosure_chk_status", SqlDbType.VarChar).Value = "R";
                cmdQry.Parameters.Add("@cbfclosure_chk_remarks", SqlDbType.VarChar).Value = remarks;
                cmdQry.Parameters.Add("@cbfclosure_gid", SqlDbType.VarChar).Value = HttpContext.Current.Session["Cancellationgid"];
                cmdQry.Parameters.Add("@cbfclosure_cbfhead_gid", SqlDbType.VarChar).Value = HttpContext.Current.Session["Cbfnochecker"];
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "CBFclosurechecker";
                int a3 = cmdQry.ExecuteNonQuery();
                if (a3 == 2)
                {
                 //   int cbfrefFlag = Convert.ToInt32(ConfigurationManager.AppSettings["cbfrefFlag"].ToString());
                 //   string[,] wherecond = new string[,]
                 // {
                 //        {"queue_ref_gid",HttpContext.Current.Session["Cbfnochecker"].ToString()},
                 //        {"queue_ref_flag",cbfrefFlag.ToString()}
                 //     };
                 //   string[,] column = new string[,]
                 //{
                
                 //   {"queue_action_date","sysdatetime()"},
                 //   {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                 //   {"queue_action_for","R"},
                 //   {"queue_action_status","0"},
                 //   {"queue_action_remark",(string.IsNullOrEmpty(remarks) ? string.Empty : remarks.ToString())},
                 //};
                 //   string tblname = "iem_trn_tqueue";
                 //   result = objCommonIUD.UpdateCommon(column, wherecond, tblname);
                    getconnection();
                    cmdQry = new SqlCommand("pr_fb_trn_rejectupdates", objConn);
                    cmdQry.CommandType = CommandType.StoredProcedure;
                    cmdQry.Parameters.Add("@actionby", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid();
                    cmdQry.Parameters.Add("@refname", SqlDbType.VarChar).Value = "CBF";
                    cmdQry.Parameters.Add("@actionremark", SqlDbType.VarChar).Value = remarks;
                    cmdQry.Parameters.Add("@refgid", SqlDbType.VarChar).Value = HttpContext.Current.Session["Cbfnochecker"];
                    cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "cbfcancel";
                    cmdQry.ExecuteNonQuery();
                }
                return a3.ToString();
            }


            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }

        }
        public string ApprovalReopen(string remarks)
        {
            try
            {
                getconnection();
                string result = "";
                cmdQry = new SqlCommand("pr_fb_trn_tcbfcancellation", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@cbfclosure_chk_gid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmdQry.Parameters.Add("@cbfclosure_chk_status", SqlDbType.VarChar).Value = "A";
                cmdQry.Parameters.Add("@cbfclosure_chk_remarks", SqlDbType.VarChar).Value = remarks;
                cmdQry.Parameters.Add("@cbfclosure_gid", SqlDbType.Int).Value = HttpContext.Current.Session["Cancellationgid"];
                cmdQry.Parameters.Add("@cbfclosure_cbfhead_gid", SqlDbType.Int).Value = HttpContext.Current.Session["Cbfnochecker"];
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "CBFreopenchecker";
                int a3 = cmdQry.ExecuteNonQuery();
                if (a3 == 2)
                {
                    int cbfrefFlag = Convert.ToInt32(ConfigurationManager.AppSettings["cbfrefFlag"].ToString());
                    string[,] wherecond = new string[,]
	              {
                         {"queue_ref_gid",HttpContext.Current.Session["Cbfnochecker"].ToString()},
                         {"queue_ref_flag",cbfrefFlag.ToString()},
                         {"queue_isremoved","N"}
                      };
                    string[,] column = new string[,]
	             {
                    {"queue_isremoved", "Y"},
                    {"queue_action_date","sysdatetime()"},
                    {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                    {"queue_action_for","A"},
                    {"queue_action_status","1"},
                    {"queue_action_remark",(string.IsNullOrEmpty(remarks) ? string.Empty : remarks.ToString())},
                 };
                    string tblname = "iem_trn_tqueue";
                    result = objCommonIUD.UpdateCommon(column, wherecond, tblname);
                }
                return a3.ToString();
            }


            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }

        }
        public string Rejectreopen(string remarks)
        {
            try
            {
                getconnection();
                string result = "";
                cmdQry = new SqlCommand("pr_fb_trn_tcbfcancellation", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@cbfclosure_chk_gid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmdQry.Parameters.Add("@cbfclosure_chk_status", SqlDbType.VarChar).Value = "R";
                cmdQry.Parameters.Add("@cbfclosure_chk_remarks", SqlDbType.VarChar).Value = remarks;
                cmdQry.Parameters.Add("@cbfclosure_gid", SqlDbType.VarChar).Value = HttpContext.Current.Session["Cancellationgid"];
                cmdQry.Parameters.Add("@cbfclosure_cbfhead_gid", SqlDbType.VarChar).Value = HttpContext.Current.Session["Cbfnochecker"];
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "CBFreopenchecker";
                int a3 = cmdQry.ExecuteNonQuery();
                if (a3 == 2)
                {
                    int cbfrefFlag = Convert.ToInt32(ConfigurationManager.AppSettings["cbfrefFlag"].ToString());
                    string[,] wherecond = new string[,]
	              {
                         {"queue_ref_gid",HttpContext.Current.Session["Cbfnochecker"].ToString()},
                         {"queue_ref_flag",cbfrefFlag.ToString()}
                      };
                    string[,] column = new string[,]
	             {
                   
                    {"queue_action_date","sysdatetime()"},
                    {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                    {"queue_action_for","R"},
                    {"queue_action_status","0"},
                    {"queue_action_remark",(string.IsNullOrEmpty(remarks) ? string.Empty : remarks.ToString())},
                 };
                    string tblname = "iem_trn_tqueue";
                    result = objCommonIUD.UpdateCommon(column, wherecond, tblname);
                }
                return a3.ToString();
            }


            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }

        }
        public string GetCBFClosureExcelValid(string validate, string action)
        {
            string result = "";
            try
            {
                getconnection();
                cmdQry = new SqlCommand("pr_fb_trn_CBFClosureExceValidate", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@chkdata", SqlDbType.VarChar).Value = validate.ToString();
                cmdQry.Parameters.Add("@result", SqlDbType.VarChar).Value = action;
                result = (string)cmdQry.ExecuteScalar();
                return result;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
            
        }
        public string BulkCBFInsert(DataTable dt, int validRecords, string fileName)
        {
            try
            {
                getconnection();
                string result = string.Empty;
                //cmd = new SqlCommand("pr_fb_trn_poclosure", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@filename", fileName.ToString());
                //cmd.Parameters.AddWithValue("@validrecords",validRecords);
                //cmd.Parameters.AddWithValue("@actionName", "bulkinsert");
                //cmd.Parameters.Add("@result", SqlDbType.Int);
                //cmd.Parameters["@result"].Direction = ParameterDirection.Output;
                //int a =cmd.ExecuteNonQuery();
                //string res = Convert.ToString(cmd.Parameters["@result"].Value.ToString());
                //if (res != "")
                //{
                if (dt.Rows.Count > 0)
                {
                    if (!dt.Columns.Contains("cbfheadgid"))
                        dt.Columns.Add("cbfheadgid");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cmdQry = new SqlCommand("pr_fb_trn_tcbfcancellation", objConn);
                        cmdQry.CommandType = CommandType.StoredProcedure;
                        cmdQry.Parameters.AddWithValue("@cbfheader_no", dt.Rows[i]["CBFNo"].ToString());
                        cmdQry.Parameters.AddWithValue("@cbfheader_date", objCmnFunctions.convertoDateTimeString(dt.Rows[i]["CBFDate"].ToString()));
                        cmdQry.Parameters.AddWithValue("@action", "selectcbfheadgid");
                        dt.Rows[i]["cbfheadgid"] = cmdQry.ExecuteScalar();

                        string[,] columnval = new string[,]
	               {
                        {"cbfclosure_cbfheader_gid",dt.Rows[i]["cbfheadgid"].ToString()},
                         {"cbfclosure_raiser_gid",objCmnFunctions.GetLoginUserGid().ToString()},
                        {"cbfclosure_remarks",dt.Rows[i]["Remarks"].ToString()},
                        {"cbfclosure_date",DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss")}
                  };

                        string tblname1 = "fb_trn_tcbfclosure";
                        result = objCommonIUD.InsertCommon(columnval, tblname1);
                        if (result == "success")
                        {

                            string[,] columnval1 = new string[,]
	          {
                 {"cbfheader_closed","M"},
           };
                            string[,] wherecolumn = new string[,]
                    {
                      {"cbfheader_gid",dt.Rows[i]["cbfheadgid"].ToString()}

                    };
                            string tblname = "fb_trn_tcbfheader";
                            result = objCommonIUD.UpdateCommon(columnval1, wherecolumn, tblname);
                        }
                    }
                }
                return result;
                HttpContext.Current.Session["maindatatable"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
        }

        public string GetCBFReopenExcelValid(string validate, string action)
        {
            string result = "";
            try
            {
                getconnection();
                cmdQry = new SqlCommand("pr_fb_trn_CBFReopenExceValidate", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@chkdata", SqlDbType.VarChar).Value = validate.ToString();
                cmdQry.Parameters.Add("@result", SqlDbType.VarChar).Value = action;
                result = (string)cmdQry.ExecuteScalar();
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
          
        }
        public string BulkReopenInsert(DataTable dt, int validRecords, string fileName)
        {
            try
            {
                getconnection();
                string result = string.Empty;

                if (dt.Rows.Count > 0)
                {
                    if (!dt.Columns.Contains("cbfheadgid"))
                    {
                        dt.Columns.Add("cbfheadgid");
                        dt.Columns.Add("cbfclosergid");
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cmdQry = new SqlCommand("pr_fb_trn_tcbfcancellation", objConn);
                        cmdQry.CommandType = CommandType.StoredProcedure;
                        cmdQry.Parameters.AddWithValue("@cbfheader_no", dt.Rows[i]["CBFNo"].ToString());
                        cmdQry.Parameters.AddWithValue("@cbfheader_date", objCmnFunctions.convertoDateTimeString(dt.Rows[i]["CBFDate"].ToString()));
                        cmdQry.Parameters.AddWithValue("@action", "selectcbfreopenheadgid");
                        SqlDataReader dr = cmdQry.ExecuteReader();
                        while (dr.Read())
                        {
                            dt.Rows[i]["cbfheadgid"] = dr["cbfheader_gid"].ToString();
                            dt.Rows[i]["cbfclosergid"] = dr["cbfclosure_gid"].ToString();

                        }

                        string[,] columnval = new string[,]
                          {
                                {"cbfclosure_cbfheader_gid",dt.Rows[i]["cbfheadgid"].ToString()},
                                {"cbfclosure_reopenmak_gid",objCmnFunctions.GetLoginUserGid().ToString()},
                                {"cbfclosure_reopen_date",DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss")},
                                {"cbfclosure_reopenremarks",dt.Rows[i]["Remarks"].ToString()},
                                {"cbfclosure_status","O"},
                                };

                        string[,] whrcol1 = new string[,]
	                 {
                          {"cbfclosure_gid",dt.Rows[i]["cbfclosergid"].ToString()},
                           {"cbfclosure_cbfheader_gid",dt.Rows[i]["cbfheadgid"].ToString()},
                          
                     };
                        string tname = "fb_trn_tcbfclosure";
                        result = objCommonIUD.UpdateCommon(columnval, whrcol1, tname);


                        if (result == "Success")
                        {

                            string[,] columnval1 = new string[,]
	          {
                 {"cbfheader_closed","R"},
           };
                            string[,] wherecolumn = new string[,]
                    {
                      {"cbfheader_gid",dt.Rows[i]["cbfheadgid"].ToString()}

                    };
                            string tblname = "fb_trn_tcbfheader";
                            result = objCommonIUD.UpdateCommon(columnval1, wherecolumn, tblname);
                        }
                    }
                }
                return result;
                HttpContext.Current.Session["maindatatable"] = null;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
        }
        public List<Attachment> attachment(string Cbfheader_gid)
        {
            List<Attachment> lstattach = new List<Attachment>();
            try
            {
              
                DataTable dsCbfdetatails = new DataTable();
                cmdQry = new SqlCommand("pr_fb_trn_attachment", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.BigInt).Value = Cbfheader_gid;
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dsCbfdetatails);
                for (int i = 0; i < dsCbfdetatails.Rows.Count; i++)
                {
                    Attachment attachment1 = new Attachment()
                    {
                        fileName = dsCbfdetatails.Rows[i]["attachment_filename"].ToString(),
                        attachedDate = dsCbfdetatails.Rows[i]["attachment_date"].ToString(),
                        description = dsCbfdetatails.Rows[i]["attachment_desc"].ToString(),
                        employee_gid = dsCbfdetatails.Rows[i]["employee_gid"].ToString(),
                        employee_name = dsCbfdetatails.Rows[i]["employee_by"].ToString(),
                        attachment_Gid = dsCbfdetatails.Rows[i]["attachment_gid"].ToString(),
                    };
                    lstattach.Add(attachment1);

                }

                return lstattach;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return lstattach;
            }
            finally
            {
                objConn.Close();
            }

        }
        public CbfSumEntity GetParDetailsForPAR(string lsParheader_gid)
        {
            CbfSumEntity obj = new CbfSumEntity();
            try
            {
                getconnection();
         
                ParDetails obj_getvalues;
                obj.lListParDetails = new List<ParDetails>();
                DataTable dt = new DataTable();

                cmdQry = new SqlCommand("pr_fb_trn_parheader", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@parheader_gid", SqlDbType.BigInt).Value = 0;
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    int cntiden = 30;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string year = dt.Rows[i]["pardetails_year"].ToString();
                        string[] year1 = null;
                        if (year != null)
                        {
                            year1 = year.Split('-');
                        }
                        cntiden++;
                        obj_getvalues = new ParDetails();
                        obj_getvalues.att_identify = Convert.ToString(cntiden);
                        obj_getvalues.ParGid = Convert.ToInt32(dt.Rows[i]["pardetails_gid"].ToString());
                        obj_getvalues.ParDetails_Gid = Convert.ToInt32(dt.Rows[i]["pardetails_gid"].ToString());
                        obj_getvalues.Expense = dt.Rows[i]["pardetails_expensetype"].ToString();
                        obj_getvalues.Department = dt.Rows[i]["pardetails_requestfor_gid"].ToString();
                        obj_getvalues.Budgeted = dt.Rows[i]["pardetails_isbudgeted"].ToString();
                        obj_getvalues.Description = dt.Rows[i]["pardetails_desc"].ToString();
                        if (year1.Length > 0)
                        {
                            obj_getvalues.FromYear = year1[0].ToString();
                            obj_getvalues.ToYear = year1[1].ToString();
                        }
                        else
                        {
                            obj_getvalues.FromYear = year1[0].ToString();
                            obj_getvalues.ToYear = year1[1].ToString();
                        }
                        obj_getvalues.ParAmount = Convert.ToInt32(dt.Rows[i]["pardetails_amt"].ToString());
                        obj_getvalues.Remarks = dt.Rows[i]["pardetails_remarks"].ToString();



                        obj.lListParDetails.Add(obj_getvalues);
                    }
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
                objConn.Close();
            }
        }

        public CbfSumEntity GetParDetailsSave(DataTable dt)
        {
            CbfSumEntity obj = new CbfSumEntity();
            try
            {
                getconnection();
              
                ParDetails obj_getvalues;
                obj.lListParDetails = new List<ParDetails>();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        obj_getvalues = new ParDetails();
                        {
                            if (dt.Columns.Contains("sno"))
                            {
                                if (dt.Rows[i]["sno"].ToString() != "" && dt.Rows[i]["sno"].ToString() != null)
                                {

                                    obj_getvalues.ParDetails_Gid = Convert.ToInt32(dt.Rows[i]["sno"].ToString());
                                    obj_getvalues.capexId = dt.Rows[i]["Expense"].ToString();
                                    obj_getvalues.Department = dt.Rows[i]["department"].ToString();
                                    obj_getvalues.budjectedid = dt.Rows[i]["bugdect"].ToString();
                                    obj_getvalues.Description = dt.Rows[i]["Description"].ToString();
                                    obj_getvalues.FromYear = dt.Rows[i]["fromyear"].ToString();
                                    obj_getvalues.ToYear = dt.Rows[i]["toyear"].ToString();
                                    obj_getvalues.ParAmount = Convert.ToInt64(dt.Rows[i]["Amount"].ToString());
                                    obj_getvalues.Remarks = dt.Rows[i]["remarks"].ToString();
                                    obj_getvalues.att_identify = dt.Rows[i]["BOQ"].ToString();
                                }
                                else
                                {
                                    obj_getvalues.ParGid = Convert.ToInt32(dt.Rows[i]["pardetails_gid"].ToString());
                                    obj_getvalues.ParDetails_Gid = Convert.ToInt32(dt.Rows[i]["pardetails_gid"].ToString());
                                    obj_getvalues.capexId = dt.Rows[i]["Expense"].ToString();
                                    obj_getvalues.Department = dt.Rows[i]["department"].ToString();
                                    obj_getvalues.budjectedid = dt.Rows[i]["bugdect"].ToString();
                                    obj_getvalues.Description = dt.Rows[i]["Description"].ToString();
                                    string split = dt.Rows[i]["fromyear"].ToString();
                                    string[] split1 = split.Split('-');
                                    if (split1.Length > 1)
                                    {
                                        obj_getvalues.FromYear = split1[0].ToString();
                                        dt.Rows[i]["fromyear"] = split1[0].ToString();
                                    }
                                    else
                                    {
                                        obj_getvalues.FromYear = dt.Rows[i]["fromyear"].ToString();
                                    }
                                    string splitto = dt.Rows[i]["toyear"].ToString();
                                    string[] splitto1 = splitto.Split('-');
                                    if (splitto1.Length > 1)
                                    {
                                        obj_getvalues.ToYear = splitto1[1].ToString();
                                        dt.Rows[i]["toyear"] = splitto1[1].ToString();
                                    }
                                    else
                                    {
                                        obj_getvalues.ToYear = dt.Rows[i]["toyear"].ToString();
                                    }
                                    obj_getvalues.ParAmount = Convert.ToInt32(dt.Rows[i]["Amount"].ToString());
                                    obj_getvalues.Remarks = dt.Rows[i]["remarks"].ToString();
                                    obj_getvalues.att_identify = dt.Rows[i]["BOQ"].ToString();
                                }
                            }
                            else
                            {
                                obj_getvalues.ParDetails_Gid = Convert.ToInt32(dt.Rows[i]["pardetails_gid"].ToString());
                                obj_getvalues.ParGid = Convert.ToInt32(dt.Rows[i]["pardetails_gid"].ToString());
                                obj_getvalues.capexId = dt.Rows[i]["Expense"].ToString();
                                obj_getvalues.Department = dt.Rows[i]["department"].ToString();
                                obj_getvalues.budjectedid = dt.Rows[i]["bugdect"].ToString();
                                obj_getvalues.Description = dt.Rows[i]["Description"].ToString();
                                string split = dt.Rows[i]["fromyear"].ToString();
                                string[] split1 = split.Split('-');
                                if (split1.Length > 1)
                                {
                                    obj_getvalues.FromYear = split1[0].ToString();
                                    dt.Rows[i]["fromyear"] = split1[0].ToString();
                                }
                                else
                                {
                                    obj_getvalues.FromYear = dt.Rows[i]["fromyear"].ToString();
                                }
                                string splitto = dt.Rows[i]["toyear"].ToString();
                                string[] splitto1 = splitto.Split('-');
                                if (splitto1.Length > 1)
                                {
                                    obj_getvalues.ToYear = splitto1[1].ToString();
                                    dt.Rows[i]["toyear"] = splitto1[1].ToString();
                                }
                                else
                                {
                                    obj_getvalues.ToYear = dt.Rows[i]["toyear"].ToString();
                                }
                                obj_getvalues.ParAmount = Convert.ToInt32(dt.Rows[i]["Amount"].ToString());
                                obj_getvalues.Remarks = dt.Rows[i]["remarks"].ToString();
                                obj_getvalues.att_identify = dt.Rows[i]["BOQ"].ToString();
                            }

                        }

                        obj.lListParDetails.Add(obj_getvalues);
                    }
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
                objConn.Close();
            }
        }
        public CbfSumEntity GetParSummary()
        {
            CbfSumEntity obj = new CbfSumEntity();
            try
            {
                DataTable dt = new DataTable();
                getconnection();
              
                Parheader obj_getvalues;
                obj.ListParHeader = new List<Parheader>();
                cmdQry = new SqlCommand("pr_fb_trn_parsummary", objConn);
                cmdQry.Parameters.Add("@employee_gid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmdQry.CommandType = CommandType.StoredProcedure;
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        obj_getvalues = new Parheader()
                      {
                          ParHeader_Gid = dt.Rows[i]["parheader_gid"].ToString(),
                          ParNo = dt.Rows[i]["parheader_refno"].ToString(),
                          ParDate = dt.Rows[i]["parheader_date"].ToString(),
                          pardescription = dt.Rows[i]["parheader_desc"].ToString(),
                          status = dt.Rows[i]["status_name"].ToString(),
                      };
                        obj.ListParHeader.Add(obj_getvalues);
                    }
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
                objConn.Close();
            }
        }
        public Employee_gid GetEmployeeDetails(string code)
        {
            List<Employee_gid> objOrgType = new List<Employee_gid>();
            Employee_gid objModel=new Employee_gid();
            try
            {              
                getconnection();
                DataTable objTable = new DataTable();
                SqlCommand cmdQry = new SqlCommand("pr_fb_iem_mst_employee", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@actionName", "selectby_code");
                cmdQry.Parameters.AddWithValue("@employee_code", code);
                SqlDataAdapter objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(objTable);
                if (objTable.Rows.Count > 0)
                {
                    objModel = new Employee_gid()
                    {
                        employeeGid = Convert.ToInt32(objTable.Rows[0]["employee_gid"]),
                        empCode = objTable.Rows[0]["employee_code"].ToString(),
                        empName = objTable.Rows[0]["employee_name"].ToString(),
                        empdesignation = objTable.Rows[0]["employee_iem_designation"].ToString(),
                    };
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
                objConn.Close();
            }
           
        }
        //public List<Employee_gid> GetEmployeeDetails( string code)
        //{
        //    List<Employee_gid> obj = new List<Employee_gid>();
        //    try
        //    {
        //        getconnection();
        //        DataTable objTable = new DataTable();

        //        Employee_gid objproject;
        //        cmdQry = new SqlCommand("pr_fb_iem_mst_employee", objConn);
        //        cmdQry.CommandType = CommandType.StoredProcedure;
        //        cmdQry.Parameters.AddWithValue("@actionName", "selectby_code");
        //        cmdQry.Parameters.AddWithValue("@employee_code", code);
        //        objDataAdapter = new SqlDataAdapter(cmdQry);
        //        objDataAdapter.Fill(objTable);
        //        for (int i = 0; i < objTable.Rows.Count; i++)
        //        {
        //            objproject = new Employee_gid();
        //            objproject.slNo = i + 1;
        //            objproject.employeeGid = Convert.ToInt32(objTable.Rows[i]["employee_gid"]);
        //            objproject.empCode = objTable.Rows[i]["employee_code"].ToString();
        //            objproject.empName = objTable.Rows[i]["employee_name"].ToString();
        //            objproject.empdesignation = objTable.Rows[i]["employee_iem_designation"].ToString();
        //            obj.Add(objproject);
        //        }

        //        return obj;
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //        return obj;
        //    }
        //    finally
        //    {
        //        objConn.Close();
        //    }
        //}
        public List<Employee_gid> GetEmployeeDetails()
        {
            List<Employee_gid> obj = new List<Employee_gid>();
            try
            {
                getconnection();
                DataTable objTable = new DataTable();
               
                Employee_gid objproject;
                cmdQry = new SqlCommand("pr_fb_iem_mst_employee", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@actionName", "Select");
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(objTable);
                for (int i = 0; i < objTable.Rows.Count; i++)
                {
                    objproject = new Employee_gid();
                    objproject.slNo = i + 1;
                    objproject.employeeGid = Convert.ToInt32(objTable.Rows[i]["employee_gid"]);
                    objproject.empCode = objTable.Rows[i]["employee_code"].ToString();
                    objproject.empName = objTable.Rows[i]["employee_name"].ToString();
                    objproject.empdesignation = objTable.Rows[i]["employee_iem_designation"].ToString();
                    obj.Add(objproject);
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
                objConn.Close();
            }
        }
        public CbfSumEntity SaveEmployee_details(DataTable dt, List<CbfSumEntity> lst = null)
        {
            CbfSumEntity obj = new CbfSumEntity();
            try
            {
               
                Employee_gid obj_getvalues;
                obj.Employeedetails = new List<Employee_gid>();
                if (lst != null)
                {
                    if (lst.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            obj_getvalues = new Employee_gid();
                            obj_getvalues.slNo = i + 1;
                            obj_getvalues.employeeGid = Convert.ToInt32(lst[i].employeeGid);
                            obj_getvalues.empCode = lst[i].employee_code;
                            obj_getvalues.empName = lst[i].employee_name;
                            obj_getvalues.empdesignation = lst[i].empdesignation;
                            obj_getvalues.Approvaldate = lst[i].Approvaldate;
                            obj.Employeedetails.Add(obj_getvalues);
                        }
                    }
                }
                else
                {
                    if (dt.Rows.Count > 0)
                    {

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            obj_getvalues = new Employee_gid();
                            obj_getvalues.slNo = i + 1;
                            obj_getvalues.employeeGid = Convert.ToInt32(dt.Rows[i]["employee_gid"]);
                            obj_getvalues.empCode = dt.Rows[i]["employee_code"].ToString();
                            obj_getvalues.empName = dt.Rows[i]["employee_name"].ToString();
                            obj_getvalues.empdesignation = dt.Rows[i]["employee_designation"].ToString();
                            obj_getvalues.Approvaldate = dt.Rows[i]["Approvaldate"].ToString();
                            obj.Employeedetails.Add(obj_getvalues);
                        }
                    }
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
                objConn.Close();
            }

        }
        public string Insertparheader(Parheader objParheader, DataTable objDt, DataTable objapproval)
        {
            string year = "";
            string yearto = "";
            string period = "";
            try
            {
                getconnection();
                int pardetails = 0;
                if (objDt.Rows.Count < 0)
                {
                    return "Please Enter Any Par Details Line";
                }
                else
                {
                    int minVal = Convert.ToInt32(objDt.Compute("min(fromyear)", string.Empty));
                    int MaxVal = Convert.ToInt32(objDt.Compute("max(toyear)", string.Empty));
                    year = minVal.ToString();
                    yearto = MaxVal.ToString();
                    period = year + "-" + yearto;
                }
                if (objapproval.Rows.Count < 0)
                {
                    return "Please Enter Any Par Approval Details";
                }
                //  DataTable dt2 = (DataTable)System.Web.HttpContext.Current.Session["AccessTokenInline"];
                DataTable dtattachment = (DataTable)System.Web.HttpContext.Current.Session["AccessToken"];
                cmdQry = new SqlCommand("pr_fb_parprocess", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;

                cmdQry.Parameters.Add("@parheader_refno", SqlDbType.VarChar).Value = objCmnFunctions.GetSequenceNoFb("PAR", "", "");
                //  cmdQry.Parameters.Add("@parheader_date", SqlDbType.Date).Value = convertoDate(objParheader.ParDate.ToString());
                cmdQry.Parameters.Add("@parheader_date", SqlDbType.SmallDateTime).Value = DateTime.Now.ToString();
                cmdQry.Parameters.Add("@parheader_amt", SqlDbType.Decimal).Value = Convert.ToDecimal(objParheader.ParAmount);
                cmdQry.Parameters.Add("@parheader_enddate", SqlDbType.Date).Value = convertoDate(objParheader.FinalApprovalDate.ToString());
                cmdQry.Parameters.Add("@parheader_period", SqlDbType.VarChar).Value = period;
                cmdQry.Parameters.Add("@parheader_desc", SqlDbType.VarChar).Value = objParheader.pardescription;
                cmdQry.Parameters.Add("@parheader_Contigencypercent", SqlDbType.VarChar).Value = objParheader.contigency;
                cmdQry.Parameters.Add("@parheader_AmtWithContigency", SqlDbType.VarChar).Value = objParheader.contigencyamount;
                cmdQry.Parameters.Add("@parheader_isbudgeted", SqlDbType.VarChar).Value = objParheader.budgeted;
                cmdQry.Parameters.Add("@parheader_status", SqlDbType.Decimal).Value = 1;
                cmdQry.Parameters.Add("@parheader_raiser_gid", SqlDbType.Decimal).Value = objCmnFunctions.GetLoginUserGid();
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "Parheader";
                cmdQry.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;
                int a = cmdQry.ExecuteNonQuery();
                int yourResult = Convert.ToInt32(cmdQry.Parameters["@result"].Value);
                if (a == 1)
                {
                    if (dtattachment != null)
                    {
                        if (dtattachment.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtattachment.Rows.Count; i++)
                            {
                                getconnection();
                                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                                cmdQry.CommandType = CommandType.StoredProcedure;
                                cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.Int).Value = yourResult;
                                cmdQry.Parameters.Add("@attachment_ref_flag", SqlDbType.TinyInt).Value = 9;
                                cmdQry.Parameters.Add("@attachment_attachmenttype_gid", SqlDbType.SmallInt).Value = 3;
                                cmdQry.Parameters.Add("@attachment_filename", SqlDbType.VarChar, 256).Value = dtattachment.Rows[i]["Documnet_Name"].ToString();
                                cmdQry.Parameters.Add("@attachment_desc", SqlDbType.VarChar).Value = dtattachment.Rows[i]["Document_des"].ToString();
                                cmdQry.Parameters.Add("@attachment_date", SqlDbType.Date).Value = convertoDate(dtattachment.Rows[i]["Attachment_Date"].ToString());
                                cmdQry.Parameters.Add("@attachment_by", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "attachment";
                                int a2 = cmdQry.ExecuteNonQuery();
                                if (a2 != 1)
                                {
                                    return "Not Inserted PAR Header Attachment";
                                }
                            }
                        }
                    }
                    if (objDt.Rows.Count > 0)
                    {
                        for (int i = 0; i < objDt.Rows.Count; i++)
                        {
                            getconnection();
                            cmdQry = new SqlCommand("pr_fb_parprocess", objConn);
                            cmdQry.CommandType = CommandType.StoredProcedure;
                            cmdQry.Parameters.Add("@parheader_gid", SqlDbType.Int).Value = yourResult;
                            cmdQry.Parameters.Add("@pardetails_expensetype", SqlDbType.VarChar).Value = objDt.Rows[i]["Expense"].ToString();
                            cmdQry.Parameters.Add("@pardetails_requestfor_gid", SqlDbType.Int).Value = objDt.Rows[i]["department"].ToString();
                            cmdQry.Parameters.Add("@pardetails_isbudgeted", SqlDbType.Char).Value = objParheader.budgeted;
                            cmdQry.Parameters.Add("@pardetails_desc", SqlDbType.VarChar).Value = objDt.Rows[i]["Description"].ToString();
                            cmdQry.Parameters.Add("@pardetails_year", SqlDbType.VarChar).Value = objDt.Rows[i]["fromyear"].ToString() + "-" + objDt.Rows[i]["toyear"].ToString();
                            cmdQry.Parameters.Add("@pardetails_amt", SqlDbType.Decimal).Value = (objDt.Rows[i]["Amount"].ToString());
                            cmdQry.Parameters.Add("@pardetails_remarks", SqlDbType.VarChar).Value = objDt.Rows[i]["remarks"].ToString();
                            cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "Pardetails";
                            int a2 = cmdQry.ExecuteNonQuery();
                            if (a2 == 1)
                            {
                                cmdQry = new SqlCommand("pr_fb_trn_attachmentall", objConn);
                                cmdQry.CommandType = CommandType.StoredProcedure;
                                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectpardetailsgid";
                                cmdQry.Parameters.Add("@cbfheadgid", SqlDbType.Int).Value = yourResult;
                                cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                                cmdQry.Parameters.Add("@process", SqlDbType.VarChar).Value = "PARTEMP";
                                objDataAdapter = new SqlDataAdapter(cmdQry);
                                DataTable objTable = new DataTable();
                                string gid = "";
                                objDataAdapter.Fill(objTable);
                                //for (int git = 0; git < objTable.Rows.Count; git++)
                                //{
                                //    gid = objTable.Rows[git]["cbfdetails_gid"].ToString();
                                //}
                                string refflaggid = "";
                                string cbfdetailsflag = "";
                                if (objTable.Rows.Count > 0)
                                {
                                    gid = objTable.Rows[0]["pardetails_gid"].ToString();
                                    refflaggid = objTable.Rows[0]["ref_flag"].ToString();
                                    cbfdetailsflag = objTable.Rows[0]["par_ref_flag"].ToString();
                                }
                                string[,] codes = new string[,]
	                                     {
                                             {"attachment_ref_gid", gid},
                                             {"attachment_ref_flag", cbfdetailsflag}
	                                        
                                         };
                                string[,] whrcol = new string[,]
	                                        {
                                                {"attachment_ref_flag", refflaggid},
                                                {"attachment_ref_gid", Convert.ToString(i + 1)},
                                                {"attachment_by", objCmnFunctions.GetLoginUserGid().ToString()},
                                                {"attachment_attachmenttype_gid", "2"}
                                            };
                                objCommonIUD.UpdateCommon(codes, whrcol, "iem_trn_tattachment");
                                //if (objDt.Rows[i]["BOQ"].ToString() != null)
                                //{

                                //    cmdQry = new SqlCommand("pr_fb_parprocess", objConn);
                                //    cmdQry.CommandType = CommandType.StoredProcedure;
                                //    cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectpardetailid";
                                //    cmdQry.Parameters.Add("@parheader_gid", SqlDbType.Int).Value = yourResult;
                                //    objDataAdapter = new SqlDataAdapter(cmdQry);
                                //    DataTable objTable = new DataTable();
                                //    string gid = "";
                                //    objDataAdapter.Fill(objTable);
                                //    for (int git = 0; git < objTable.Rows.Count; git++)
                                //    {
                                //        gid = objTable.Rows[git]["pardetails_gid"].ToString();
                                //    }
                                //    //getconnection();
                                //    //cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                                //    //cmdQry.CommandType = CommandType.StoredProcedure;
                                //    //cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.Int).Value = gid;
                                //    //cmdQry.Parameters.Add("@attachment_ref_flag", SqlDbType.TinyInt).Value = 12;
                                //    //cmdQry.Parameters.Add("@attachment_attachmenttype_gid", SqlDbType.SmallInt).Value = 2;
                                //    //cmdQry.Parameters.Add("@attachment_filename", SqlDbType.VarChar, 256).Value = dt2.Rows[i]["Documnet_Name"].ToString();
                                //    //cmdQry.Parameters.Add("@attachment_desc", SqlDbType.VarChar).Value = dt2.Rows[i]["Document_des"].ToString();
                                //    //cmdQry.Parameters.Add("@attachment_date", SqlDbType.Date).Value = convertoDate(dt2.Rows[i]["Attachment_Date"].ToString());
                                //    //cmdQry.Parameters.Add("@attachment_by", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                                //    //cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "attachment";
                                //    string[,] codes = new string[,]
                                //         {
                                //             {"attachment_ref_gid", gid},
                                //             {"attachment_attachmenttype_gid", "2"}
                                //         };
                                //    string[,] whrcol = new string[,]
                                //            {
                                //                {"attachment_ref_flag", "12"},
                                //                {"attachment_ref_gid", "932600"},
                                //                {"attachment_by", objCmnFunctions.GetLoginUserGid().ToString()},
                                //                {"attachment_isremoved", "N"},
                                //                {"attachment_attachmenttype_gid", objDt.Rows[i]["BOQ"].ToString()}
                                //            };
                                //    objCommonIUD.UpdateCommon(codes, whrcol, "iem_trn_tattachment");
                                //    if (a2 != 1)
                                //    {
                                //        return "Not Inserted PAR Details Attachment";
                                //    }
                                //}

                            }
                            if (a2 != 1)
                            {
                                return "Not Inserted PAR Details Line";
                            }

                        }

                    }
                    else
                    {
                        return "Please Enter PAR Details Line Item";
                    }
                    if (objapproval.Rows.Count > 0)
                    {
                        for (int i = 0; i < objapproval.Rows.Count; i++)
                        {
                            getconnection();
                            cmdQry = new SqlCommand("pr_fb_parprocess", objConn);
                            cmdQry.CommandType = CommandType.StoredProcedure;
                            cmdQry.Parameters.Add("@parheader_gid", SqlDbType.Int).Value = yourResult;
                            cmdQry.Parameters.Add("@parapprover_emp_gid", SqlDbType.Int).Value = objapproval.Rows[i]["employee_gid"].ToString();
                            cmdQry.Parameters.Add("@parapprover_approval_date", SqlDbType.Date).Value = objapproval.Rows[i]["Approvaldate"].ToString();
                            cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "Parapproval";
                            int a2 = cmdQry.ExecuteNonQuery();
                            if (a2 != 1)
                            {
                                return "Not Inserted PAR Details Line";
                            }
                        }

                    }

                }
                else
                {
                    return "Not Inserted PAR Header Line";
                }
                return "Inserted Successfully";

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
            
        }
        public string InsertparheaderSubmit(Parheader objParheader, DataTable objDt, DataTable objapproval)
        {
            string year = "";
            string yearto = "";
            string period = "";
            int prev_gid = 0;
            try
            {
                getconnection();
                int pardetails = 0;
                if (objDt.Rows.Count < 0)
                {
                    return "Please Enter Any Par Details Line";
                }
                else
                {
                    int minVal = Convert.ToInt32(objDt.Compute("min(fromyear)", string.Empty));
                    int MaxVal = Convert.ToInt32(objDt.Compute("max(toyear)", string.Empty));
                    year = minVal.ToString();
                    yearto = MaxVal.ToString();
                    period = year + "-" + yearto;
                }
                if (objapproval != null)
                {
                    if (objapproval.Rows.Count < 1)
                    {
                        return "Please Enter Any Par Approval Details";
                    }
                }
                else
                {
                    if (HttpContext.Current.Session["paredit_Approval"] != null)
                    {
                        List<CbfSumEntity> lst = new List<CbfSumEntity>();
                        lst = (List<CbfSumEntity>)HttpContext.Current.Session["paredit_Approval"];
                        if (lst.Count < 1)
                        {
                            return "Please Enter Any Par Approval Details";
                        }

                    }
                }

                //DataTable dt2 = (DataTable)System.Web.HttpContext.Current.Session["AccessTokenInline"];
                DataTable dtattachment = (DataTable)System.Web.HttpContext.Current.Session["AccessToken"];
                cmdQry = new SqlCommand("pr_fb_parprocess", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;

                cmdQry.Parameters.Add("@parheader_refno", SqlDbType.VarChar).Value = objCmnFunctions.GetSequenceNoFb("PAR", "", "");
                //cmdQry.Parameters.Add("@parheader_date", SqlDbType.Date).Value = convertoDate(objParheader.ParDate.ToString());
                cmdQry.Parameters.Add("@parheader_date", SqlDbType.SmallDateTime).Value = DateTime.Now.ToString();
                cmdQry.Parameters.Add("@parheader_amt", SqlDbType.Decimal).Value = Convert.ToDecimal(objParheader.ParAmount);
                cmdQry.Parameters.Add("@parheader_enddate", SqlDbType.Date).Value = convertoDate(objParheader.FinalApprovalDate.ToString());
                cmdQry.Parameters.Add("@parheader_period", SqlDbType.VarChar).Value = period;
                cmdQry.Parameters.Add("@parheader_desc", SqlDbType.VarChar).Value = objParheader.pardescription;
                cmdQry.Parameters.Add("@parheader_Contigencypercent", SqlDbType.VarChar).Value = objParheader.contigency;
                cmdQry.Parameters.Add("@parheader_AmtWithContigency", SqlDbType.VarChar).Value = objParheader.contigencyamount;
                cmdQry.Parameters.Add("@parheader_isbudgeted", SqlDbType.VarChar).Value = objParheader.budgeted;
                cmdQry.Parameters.Add("@parheader_status", SqlDbType.Decimal).Value = 2;
                cmdQry.Parameters.Add("@parheader_raiser_gid", SqlDbType.Decimal).Value = objCmnFunctions.GetLoginUserGid();
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "Parheader";
                cmdQry.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;
                int a = cmdQry.ExecuteNonQuery();
                int yourResult = Convert.ToInt32(cmdQry.Parameters["@result"].Value);
                if (a == 1)
                {
                    if (dtattachment != null)
                    {
                        if (dtattachment.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtattachment.Rows.Count; i++)
                            {
                                getconnection();
                                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                                cmdQry.CommandType = CommandType.StoredProcedure;
                                cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.Int).Value = yourResult;
                                cmdQry.Parameters.Add("@attachment_ref_flag", SqlDbType.TinyInt).Value = 9;
                                cmdQry.Parameters.Add("@attachment_attachmenttype_gid", SqlDbType.SmallInt).Value = 3;
                                cmdQry.Parameters.Add("@attachment_filename", SqlDbType.VarChar, 256).Value = dtattachment.Rows[i]["Documnet_Name"].ToString();
                                cmdQry.Parameters.Add("@attachment_desc", SqlDbType.VarChar).Value = dtattachment.Rows[i]["Document_des"].ToString();
                                cmdQry.Parameters.Add("@attachment_date", SqlDbType.Date).Value = convertoDate(dtattachment.Rows[i]["Attachment_Date"].ToString());
                                cmdQry.Parameters.Add("@attachment_by", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "attachment";
                                int a2 = cmdQry.ExecuteNonQuery();
                                if (a2 != 1)
                                {
                                    return "Not Inserted PAR Header Attachment";
                                }
                            }
                        }
                    }
                    if (objDt.Rows.Count > 0)
                    {
                        for (int i = 0; i < objDt.Rows.Count; i++)
                        {
                            getconnection();
                            cmdQry = new SqlCommand("pr_fb_parprocess", objConn);
                            cmdQry.CommandType = CommandType.StoredProcedure;
                            cmdQry.Parameters.Add("@parheader_gid", SqlDbType.Int).Value = yourResult;
                            cmdQry.Parameters.Add("@pardetails_expensetype", SqlDbType.VarChar).Value = objDt.Rows[i]["Expense"].ToString();
                            cmdQry.Parameters.Add("@pardetails_requestfor_gid", SqlDbType.Int).Value = objDt.Rows[i]["department"].ToString();
                            cmdQry.Parameters.Add("@pardetails_isbudgeted", SqlDbType.Char).Value = objParheader.budgeted;
                            cmdQry.Parameters.Add("@pardetails_desc", SqlDbType.VarChar).Value = objDt.Rows[i]["Description"].ToString();
                            cmdQry.Parameters.Add("@pardetails_year", SqlDbType.VarChar).Value = objDt.Rows[i]["fromyear"].ToString() + "-" + objDt.Rows[i]["toyear"].ToString();
                            cmdQry.Parameters.Add("@pardetails_amt", SqlDbType.Decimal).Value = (objDt.Rows[i]["Amount"].ToString());
                            cmdQry.Parameters.Add("@pardetails_remarks", SqlDbType.VarChar).Value = objDt.Rows[i]["remarks"].ToString();
                            cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "Pardetails";
                            int a2 = cmdQry.ExecuteNonQuery();
                            if (a2 == 1)
                            {
                                cmdQry = new SqlCommand("pr_fb_trn_attachmentall", objConn);
                                cmdQry.CommandType = CommandType.StoredProcedure;
                                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectpardetailsgid";
                                cmdQry.Parameters.Add("@cbfheadgid", SqlDbType.Int).Value = yourResult;
                                cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                                cmdQry.Parameters.Add("@process", SqlDbType.VarChar).Value = "PARTEMP";
                                objDataAdapter = new SqlDataAdapter(cmdQry);
                                DataTable objTable = new DataTable();
                                string gid = "";
                                objDataAdapter.Fill(objTable);
                                //for (int git = 0; git < objTable.Rows.Count; git++)
                                //{
                                //    gid = objTable.Rows[git]["cbfdetails_gid"].ToString();
                                //}
                                string refflaggid = "";
                                string cbfdetailsflag = "";
                                if (objTable.Rows.Count > 0)
                                {
                                    gid = objTable.Rows[0]["pardetails_gid"].ToString();
                                    refflaggid = objTable.Rows[0]["ref_flag"].ToString();
                                    cbfdetailsflag = objTable.Rows[0]["par_ref_flag"].ToString();
                                }
                                string[,] codes = new string[,]
	                                     {
                                             {"attachment_ref_gid", gid},
                                             {"attachment_ref_flag", cbfdetailsflag}
	                                        
                                         };
                                string[,] whrcol = new string[,]
	                                        {
                                                {"attachment_ref_flag", refflaggid},
                                                {"attachment_ref_gid", Convert.ToString(i + 1)},
                                                {"attachment_by", objCmnFunctions.GetLoginUserGid().ToString()},
                                                {"attachment_attachmenttype_gid", "2"}
                                            };
                                objCommonIUD.UpdateCommon(codes, whrcol, "iem_trn_tattachment");
                                //if (objDt.Rows[i]["BOQ"].ToString() != null)
                                //{

                                //    cmdQry = new SqlCommand("pr_fb_parprocess", objConn);
                                //    cmdQry.CommandType = CommandType.StoredProcedure;
                                //    cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectpardetailid";
                                //    cmdQry.Parameters.Add("@parheader_gid", SqlDbType.Int).Value = yourResult;
                                //    objDataAdapter = new SqlDataAdapter(cmdQry);
                                //    DataTable objTable = new DataTable();
                                //    string gid = "";
                                //    objDataAdapter.Fill(objTable);
                                //    for (int git = 0; git < objTable.Rows.Count; git++)
                                //    {
                                //        gid = objTable.Rows[git]["pardetails_gid"].ToString();
                                //    }
                                //    string[,] codes = new string[,]
                                //         {
                                //             {"attachment_ref_gid", gid},
                                //             {"attachment_attachmenttype_gid", "2"}
                                //         };
                                //    string[,] whrcol = new string[,]
                                //            {
                                //                {"attachment_ref_flag", "12"},
                                //                {"attachment_ref_gid", "932600"},
                                //                {"attachment_by", objCmnFunctions.GetLoginUserGid().ToString()},
                                //                {"attachment_isremoved", "N"},
                                //                {"attachment_attachmenttype_gid", objDt.Rows[i]["BOQ"].ToString()}
                                //            };
                                //    objCommonIUD.UpdateCommon(codes, whrcol, "iem_trn_tattachment");
                                //    if (a2 != 1)
                                //    {
                                //        return "Not Inserted PAR Details Attachment";
                                //    }
                                //}
                            }
                            if (a2 != 1)
                            {
                                return "Not Inserted PAR Details Line";
                            }
                        }
                    }
                    else
                    {
                        return "Please Enter PAR Details Line Item";
                    }
                    //if (objapproval.Rows.Count > 0)
                    //{
                    //    for (int i = 0; i < objapproval.Rows.Count; i++)
                    //    {
                    //        getconnection();
                    //        cmdQry = new SqlCommand("pr_fb_parprocess", objConn);
                    //        cmdQry.CommandType = CommandType.StoredProcedure;
                    //        cmdQry.Parameters.Add("@parheader_gid", SqlDbType.Int).Value = yourResult;
                    //        cmdQry.Parameters.Add("@parapprover_emp_gid", SqlDbType.Int).Value = objapproval.Rows[i]["employee_gid"].ToString();
                    //        cmdQry.Parameters.Add("@parapprover_approval_date", SqlDbType.Date).Value = objapproval.Rows[i]["Approvaldate"].ToString();
                    //        cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "Parapproval";
                    //        int a2 = cmdQry.ExecuteNonQuery();
                    //        if (a2 != 1)
                    //        {
                    //            return "Not Inserted PAR Details Line";
                    //        }
                    //    }

                    //}
                    if (objapproval != null)
                    {
                        if (objapproval.Rows.Count > 0)
                        {
                            for (int kkk = 0; kkk < objapproval.Rows.Count; kkk++)
                            {
                                if (objapproval.Rows[kkk]["employee_gid"].ToString() != "" || objapproval.Rows[kkk]["employee_gid"].ToString() != null)
                                {
                                    getconnection();
                                    cmdQry = new SqlCommand("pr_fb_parprocess", objConn);
                                    cmdQry.CommandType = CommandType.StoredProcedure;
                                    cmdQry.Parameters.Add("@parheader_gid", SqlDbType.Int).Value = yourResult;
                                    cmdQry.Parameters.Add("@parapprover_emp_gid", SqlDbType.Int).Value = objapproval.Rows[kkk]["employee_gid"].ToString();
                                    cmdQry.Parameters.Add("@parapprover_approval_date", SqlDbType.Date).Value = objapproval.Rows[kkk]["Approvaldate"].ToString();
                                    cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "Parapproval";
                                    int a2 = cmdQry.ExecuteNonQuery();
                                    if (a2 != 1)
                                    {
                                        return "Not Inserted PAR Details Line";
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (HttpContext.Current.Session["paredit_Approval"] != null)
                        {
                            List<CbfSumEntity> lst = new List<CbfSumEntity>();
                            lst = (List<CbfSumEntity>)HttpContext.Current.Session["paredit_Approval"];

                            for (int i = 0; i < lst.Count; i++)
                            {
                                //if (objapproval.Columns.Contains("parapprover_gid"))
                                //{
                                //    if (objapproval.Rows[i]["parapprover_gid"].ToString() != "" || objapproval.Rows[i]["parapprover_gid"].ToString() != null)
                                //    {

                                if (lst[i].approvalgid.ToString() == "")
                                {
                                    getconnection();
                                    cmdQry = new SqlCommand("pr_fb_parprocess", objConn);
                                    cmdQry.CommandType = CommandType.StoredProcedure;
                                    cmdQry.Parameters.Add("@parheader_gid", SqlDbType.Int).Value = yourResult;
                                    cmdQry.Parameters.Add("@parapprover_emp_gid", SqlDbType.Int).Value = lst[i].employeeGid.ToString();
                                    cmdQry.Parameters.Add("@parapprover_approval_date", SqlDbType.Date).Value = lst[i].Approvaldate.ToString();
                                    cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "Parapproval";
                                    int a2 = cmdQry.ExecuteNonQuery();
                                    if (a2 != 1)
                                    {
                                        return "Not Inserted PAR Details Line";
                                    }
                                }
                                //    }
                                //}
                            }


                        }
                    }

                    getconnection();
                    cmdQry = new SqlCommand("pr_fb_parprocess", objConn);
                    cmdQry.CommandType = CommandType.StoredProcedure;
                    cmdQry.Parameters.Add("@parheader_gid", SqlDbType.Int).Value = yourResult;
                    cmdQry.Parameters.Add("@employee_gid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                    cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "Parchecekr";

                    int a4 = cmdQry.ExecuteNonQuery();

                    if (objParheader.status == "submit")
                    {
                        DataTable dt = new DataTable();
                        cmdQry = new SqlCommand("pr_fb_trn_parqueue", objConn);
                        cmdQry.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(cmdQry);
                        da.Fill(dt);
                        //cmdQry.Parameters.AddWithValue("@action", "selectrole");
                        // int roledf_gid = (int)cmdQry.ExecuteScalar();

                        string[,] codesIN = new string[,]
                                      {
                                           {"queue_date","sysdatetime()"},
                                           {"queue_ref_flag", "9"},
                                           {"queue_ref_gid",yourResult.ToString() },
                                           {"queue_ref_status", "0"},
                                           {"queue_from",objCmnFunctions.GetLoginUserGid().ToString() } ,
                                           {"queue_to_type", "R"},
                                           {"queue_to", dt.Rows[0][0].ToString()},
                                           {"queue_action_for", "A"},
                                           {"queue_prev_gid",prev_gid.ToString()}
                                     };
                        string tnameIN = "iem_trn_tqueue";
                        string insertcommendecf = objCommonIUD.InsertCommon(codesIN, tnameIN);
                    }
                }
                else
                {
                    return "Not Inserted PAR Header Line";
                }
                return "Inserted Successfully";
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
           

        }
        public string updateparheader(Parheader objParheader, DataTable objDt, DataTable dt, DataTable objapproval)
        {
            string year = "";
            string yearto = "";
            string period = "";
            getconnection();
            int pardetails = 0;
            try
            {

                if (objDt != null)
                {
                    if (objDt.Rows.Count < 0)
                    {
                        return "Please Enter Any Par Details Line";
                    }
                    else
                    {
                        int minVal = Convert.ToInt32(objDt.Compute("min(fromyear)", string.Empty));
                        int MaxVal = Convert.ToInt32(objDt.Compute("max(toyear)", string.Empty));
                        year = minVal.ToString();
                        yearto = MaxVal.ToString();
                        period = year + "-" + yearto;
                    }
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    period = dt.Rows[0]["parheader_period"].ToString();
                }
                //if (objapproval != null)
                //{
                //    if (objapproval.Rows.Count < 0)
                //    {
                //        return "Please Enter Any Par Approval Details";
                //    }
                //}
                if (HttpContext.Current.Session["paredit_Approval"] != null)
                {
                    List<CbfSumEntity> lst = new List<CbfSumEntity>();
                    lst = (List<CbfSumEntity>)HttpContext.Current.Session["paredit_Approval"];
                    if (lst.Count < 1)
                    {
                        return "Please Enter Any Par Approval Details";
                    }

                }
                DataTable dt2 = (DataTable)System.Web.HttpContext.Current.Session["AccessTokenInline"];
                DataTable dtattachment = (DataTable)System.Web.HttpContext.Current.Session["AccessToken"];
                cmdQry = new SqlCommand("pr_fb_parprocess", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;

                cmdQry.Parameters.Add("@parheader_refno", SqlDbType.VarChar).Value = objCmnFunctions.GetSequenceNoFb("PAR", "", "");
                //cmdQry.Parameters.Add("@parheader_date", SqlDbType.Date).Value = convertoDate(objParheader.ParDate.ToString());
                cmdQry.Parameters.Add("@parheader_date", SqlDbType.SmallDateTime).Value = DateTime.Now.ToString();
                cmdQry.Parameters.Add("@parheader_amt", SqlDbType.Decimal).Value = Convert.ToDecimal(objParheader.ParAmount);
                cmdQry.Parameters.Add("@parheader_enddate", SqlDbType.Date).Value = convertoDate(objParheader.FinalApprovalDate.ToString());
                cmdQry.Parameters.Add("@parheader_period", SqlDbType.VarChar).Value = period;
                //if (objDt != null)
                //{
                //    cmdQry.Parameters.Add("@parheader_period", SqlDbType.VarChar).Value = period;
                //}
                cmdQry.Parameters.Add("@parheader_desc", SqlDbType.VarChar).Value = objParheader.pardescription;
                cmdQry.Parameters.Add("@parheader_Contigencypercent", SqlDbType.VarChar).Value = objParheader.contigency;
                cmdQry.Parameters.Add("@parheader_AmtWithContigency", SqlDbType.VarChar).Value = objParheader.contigencyamount;
                cmdQry.Parameters.Add("@parheader_isbudgeted", SqlDbType.VarChar).Value = objParheader.budgeted;
                cmdQry.Parameters.Add("@parheader_status", SqlDbType.Decimal).Value = 1;
                cmdQry.Parameters.Add("@parheader_gid", SqlDbType.VarChar).Value = HttpContext.Current.Session["parheadergid"];
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "Updateheader";
                int a = cmdQry.ExecuteNonQuery();
                int yourResult = (int)HttpContext.Current.Session["parheadergid"];
                if (a == 1)
                {
                    //if (dtattachment != null)
                    //{
                    //    if (dtattachment.Rows.Count > 0)
                    //    {
                    //        for (int i = 0; i < dtattachment.Rows.Count; i++)
                    //        {
                    //            getconnection();
                    //            cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                    //            cmdQry.CommandType = CommandType.StoredProcedure;
                    //            cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.Int).Value = yourResult;
                    //            cmdQry.Parameters.Add("@attachment_ref_flag", SqlDbType.TinyInt).Value = 9;
                    //            cmdQry.Parameters.Add("@attachment_attachmenttype_gid", SqlDbType.SmallInt).Value = 3;
                    //            cmdQry.Parameters.Add("@attachment_filename", SqlDbType.VarChar, 256).Value = dtattachment.Rows[i]["Documnet_Name"].ToString();
                    //            cmdQry.Parameters.Add("@attachment_desc", SqlDbType.VarChar).Value = dtattachment.Rows[i]["Document_des"].ToString();
                    //            cmdQry.Parameters.Add("@attachment_date", SqlDbType.Date).Value = convertoDate(dtattachment.Rows[i]["Attachment_Date"].ToString());
                    //            cmdQry.Parameters.Add("@attachment_by", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                    //            cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "attachment";
                    //            int a2 = cmdQry.ExecuteNonQuery();
                    //            if (a2 != 1)
                    //            {
                    //                return "Not Inserted PAR Header Attachment";
                    //            }
                    //        }
                    //    }
                    //}
                    if (objDt != null)
                    {
                        if (objDt.Rows.Count > 0)
                        {
                            for (int i = 0; i < objDt.Rows.Count; i++)
                            {
                                if (objDt.Columns.Contains("sno"))
                                {
                                    string da = objDt.Rows[i]["sno"].ToString();
                                    if (objDt.Rows[i]["sno"].ToString() != null && objDt.Rows[i]["sno"].ToString() != "")
                                    {
                                        getconnection();
                                        cmdQry = new SqlCommand("pr_fb_parprocess", objConn);
                                        cmdQry.CommandType = CommandType.StoredProcedure;
                                        cmdQry.Parameters.Add("@parheader_gid", SqlDbType.Int).Value = yourResult;
                                        cmdQry.Parameters.Add("@pardetails_expensetype", SqlDbType.VarChar).Value = objDt.Rows[i]["Expense"].ToString();
                                        cmdQry.Parameters.Add("@pardetails_requestfor_gid", SqlDbType.Int).Value = objDt.Rows[i]["department"].ToString();
                                        cmdQry.Parameters.Add("@pardetails_isbudgeted", SqlDbType.Char).Value = objParheader.budgeted;
                                        cmdQry.Parameters.Add("@pardetails_desc", SqlDbType.VarChar).Value = objDt.Rows[i]["Description"].ToString();
                                        cmdQry.Parameters.Add("@pardetails_year", SqlDbType.VarChar).Value = objDt.Rows[i]["fromyear"].ToString() + "-" + objDt.Rows[i]["toyear"].ToString();
                                        cmdQry.Parameters.Add("@pardetails_amt", SqlDbType.Decimal).Value = (objDt.Rows[i]["Amount"].ToString());
                                        cmdQry.Parameters.Add("@pardetails_remarks", SqlDbType.VarChar).Value = objDt.Rows[i]["remarks"].ToString();
                                        cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "Pardetails";
                                        int a2 = cmdQry.ExecuteNonQuery();
                                        if (a2 == 1)
                                        {
                                            cmdQry = new SqlCommand("pr_fb_trn_attachmentall", objConn);
                                            cmdQry.CommandType = CommandType.StoredProcedure;
                                            cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectpardetailsgid";
                                            cmdQry.Parameters.Add("@cbfheadgid", SqlDbType.Int).Value = yourResult;
                                            cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                                            cmdQry.Parameters.Add("@process", SqlDbType.VarChar).Value = "PARTEMP";
                                            objDataAdapter = new SqlDataAdapter(cmdQry);
                                            DataTable objTable = new DataTable();
                                            string gid = "";
                                            objDataAdapter.Fill(objTable);
                                            //for (int git = 0; git < objTable.Rows.Count; git++)
                                            //{
                                            //    gid = objTable.Rows[git]["cbfdetails_gid"].ToString();
                                            //}
                                            string refflaggid = "";
                                            string cbfdetailsflag = "";
                                            if (objTable.Rows.Count > 0)
                                            {
                                                gid = objTable.Rows[0]["pardetails_gid"].ToString();
                                                refflaggid = objTable.Rows[0]["ref_flag"].ToString();
                                                cbfdetailsflag = objTable.Rows[0]["par_ref_flag"].ToString();
                                            }
                                            string[,] codes = new string[,]
	                                     {
                                             {"attachment_ref_gid", gid},
                                             {"attachment_ref_flag", cbfdetailsflag}
	                                        
                                         };
                                            string[,] whrcol = new string[,]
	                                        {
                                                {"attachment_ref_flag", refflaggid},
                                                {"attachment_ref_gid", Convert.ToString(i + 1)},
                                                {"attachment_by", objCmnFunctions.GetLoginUserGid().ToString()},
                                                {"attachment_attachmenttype_gid", "2"}
                                            };
                                            objCommonIUD.UpdateCommon(codes, whrcol, "iem_trn_tattachment");
                                            //if (dt2 != null)
                                            //{
                                            //    if (dt2.Rows.Count > 0)
                                            //    {
                                            //        for (int j = 0; j < dt2.Rows.Count; j++)
                                            //        {
                                            //            cmdQry = new SqlCommand("pr_fb_parprocess", objConn);
                                            //            cmdQry.CommandType = CommandType.StoredProcedure;
                                            //            cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectpardetailid";
                                            //            cmdQry.Parameters.Add("@parheader_gid", SqlDbType.Int).Value = yourResult;
                                            //            objDataAdapter = new SqlDataAdapter(cmdQry);
                                            //            DataTable objTable = new DataTable();
                                            //            string gid = "";
                                            //            objDataAdapter.Fill(objTable);
                                            //            for (int git = 0; git < objTable.Rows.Count; git++)
                                            //            {
                                            //                gid = objTable.Rows[i]["pardetails_gid"].ToString();
                                            //            }
                                            //            getconnection();
                                            //            cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                                            //            cmdQry.CommandType = CommandType.StoredProcedure;
                                            //            cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.Int).Value = gid;
                                            //            cmdQry.Parameters.Add("@attachment_ref_flag", SqlDbType.TinyInt).Value = 12;
                                            //            cmdQry.Parameters.Add("@attachment_attachmenttype_gid", SqlDbType.SmallInt).Value = 2;
                                            //            cmdQry.Parameters.Add("@attachment_filename", SqlDbType.VarChar, 256).Value = dt2.Rows[i]["Documnet_Name"].ToString();
                                            //            cmdQry.Parameters.Add("@attachment_desc", SqlDbType.VarChar).Value = dt2.Rows[i]["Document_des"].ToString();
                                            //            cmdQry.Parameters.Add("@attachment_date", SqlDbType.Date).Value = convertoDate(dt2.Rows[i]["Attachment_Date"].ToString());
                                            //            cmdQry.Parameters.Add("@attachment_by", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                                            //            cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "attachment";
                                            //            int a3 = cmdQry.ExecuteNonQuery();
                                            //            if (a3 != 1)
                                            //            {
                                            //                return "Not Inserted PAR Details Attachment";
                                            //            }
                                            //        }
                                            //    }
                                            //}

                                        }
                                        if (a2 != 1)
                                        {
                                            return "Not Inserted PAR Details Line";
                                        }
                                    }
                                }
                                else
                                {
                                    getconnection();
                                    cmdQry = new SqlCommand("pr_fb_parprocess", objConn);
                                    cmdQry.CommandType = CommandType.StoredProcedure;
                                    cmdQry.Parameters.Add("@parheader_gid", SqlDbType.Int).Value = yourResult;
                                    cmdQry.Parameters.Add("@pardetails_gid", SqlDbType.Int).Value = objDt.Rows[i]["pardetails_gid"].ToString();
                                    cmdQry.Parameters.Add("@pardetails_expensetype", SqlDbType.VarChar).Value = objDt.Rows[i]["Expense"].ToString();
                                    cmdQry.Parameters.Add("@pardetails_requestfor_gid", SqlDbType.Int).Value = objDt.Rows[i]["department"].ToString();
                                    cmdQry.Parameters.Add("@pardetails_isbudgeted", SqlDbType.Char).Value = objParheader.budgeted;
                                    cmdQry.Parameters.Add("@pardetails_desc", SqlDbType.VarChar).Value = objDt.Rows[i]["Description"].ToString();
                                    cmdQry.Parameters.Add("@pardetails_year", SqlDbType.VarChar).Value = objDt.Rows[i]["fromyear"].ToString() + "-" + objDt.Rows[i]["toyear"].ToString();
                                    cmdQry.Parameters.Add("@pardetails_amt", SqlDbType.Decimal).Value = (objDt.Rows[i]["Amount"].ToString());
                                    cmdQry.Parameters.Add("@pardetails_remarks", SqlDbType.VarChar).Value = objDt.Rows[i]["remarks"].ToString();
                                    cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "updatedetails";
                                    int a2 = cmdQry.ExecuteNonQuery();
                                    if (a2 == 1)
                                    {
                                        if (dt2 != null)
                                        {
                                            if (dt2.Rows.Count > 0)
                                            {
                                                for (int j = 0; j < dt2.Rows.Count; j++)
                                                {
                                                    getconnection();
                                                    cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                                                    cmdQry.CommandType = CommandType.StoredProcedure;
                                                    cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.Int).Value = objDt.Rows[i]["pardetails_gid"].ToString();
                                                    cmdQry.Parameters.Add("@attachment_ref_flag", SqlDbType.TinyInt).Value = 12;
                                                    cmdQry.Parameters.Add("@attachment_attachmenttype_gid", SqlDbType.SmallInt).Value = 2;
                                                    cmdQry.Parameters.Add("@attachment_filename", SqlDbType.VarChar, 256).Value = dt2.Rows[i]["Documnet_Name"].ToString();
                                                    cmdQry.Parameters.Add("@attachment_desc", SqlDbType.VarChar).Value = dt2.Rows[i]["Document_des"].ToString();
                                                    cmdQry.Parameters.Add("@attachment_date", SqlDbType.Date).Value = convertoDate(dt2.Rows[i]["Attachment_Date"].ToString());
                                                    cmdQry.Parameters.Add("@attachment_by", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                                                    cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "attachment";
                                                    int a3 = cmdQry.ExecuteNonQuery();
                                                    if (a3 != 1)
                                                    {
                                                        return "Not Inserted PAR Details Attachment";
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    if (a2 != 1)
                                    {
                                        return "Not Inserted PAR Details Line";
                                    }

                                }

                            }

                        }
                        else
                        {
                            return "Please Enter PAR Details Line Item";
                        }
                    }


                    if (HttpContext.Current.Session["paredit_Approval"] != null)
                    {
                        List<CbfSumEntity> lst = new List<CbfSumEntity>();
                        lst = (List<CbfSumEntity>)HttpContext.Current.Session["paredit_Approval"];

                        for (int i = 0; i < lst.Count; i++)
                        {
                            //if (objapproval.Columns.Contains("parapprover_gid"))
                            //{
                            //    if (objapproval.Rows[i]["parapprover_gid"].ToString() != "" || objapproval.Rows[i]["parapprover_gid"].ToString() != null)
                            //    {

                            if (lst[i].approvalgid.ToString() == "")
                            {
                                getconnection();
                                cmdQry = new SqlCommand("pr_fb_parprocess", objConn);
                                cmdQry.CommandType = CommandType.StoredProcedure;
                                cmdQry.Parameters.Add("@parheader_gid", SqlDbType.Int).Value = yourResult;
                                cmdQry.Parameters.Add("@parapprover_emp_gid", SqlDbType.Int).Value = lst[i].employeeGid.ToString();
                                cmdQry.Parameters.Add("@parapprover_approval_date", SqlDbType.Date).Value = lst[i].Approvaldate.ToString();
                                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "Parapproval";
                                int a2 = cmdQry.ExecuteNonQuery();
                                if (a2 != 1)
                                {
                                    return "Not Inserted PAR Details Line";
                                }
                            }
                            //    }
                            //}
                        }


                    }
                    if (HttpContext.Current.Session["par_ArrayList"] != null)
                    {
                        ArrayList lalistCbfDelete = (ArrayList)HttpContext.Current.Session["par_ArrayList"];
                        if (lalistCbfDelete.Count > 0)
                            for (int j = 0; j < lalistCbfDelete.Count; j++)
                            {
                                DeletePARapproval(lalistCbfDelete[j].ToString());
                            }

                    }
                    if (HttpContext.Current.Session["par_ArrayListpar"] != null)
                    {
                        ArrayList lalistCbfDelete = (ArrayList)HttpContext.Current.Session["par_ArrayListpar"];
                        if (lalistCbfDelete.Count > 0)
                            for (int j = 0; j < lalistCbfDelete.Count; j++)
                            {
                                DeletePARdetailsLine(lalistCbfDelete[j].ToString());
                            }

                    }
                    if (objParheader.ApproverDeleteIDs != null && objParheader.ApproverDeleteIDs != "")
                    {
                        string[] DeleteIDs = objParheader.ApproverDeleteIDs.Split(',');
                        for (int kk = 0; kk < DeleteIDs.Length; kk++)
                        {
                            string[,] codes = new string[,]
                           {
                                {"parapprover_isremoved", "Y"}
                           };
                            string[,] whrcol = new string[,]
	                        {
                                {"parapprover_gid", DeleteIDs[kk]}
                            };
                            string insertcommand = objCommonIUD.UpdateCommon(codes, whrcol, "fb_trn_tparapprover");
                        }
                    }
                }
                else
                {
                    return "Not Inserted PAR Header Line";
                }
                return "Inserted Successfully";

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
        }
        public string updateparheaderSubmit(Parheader objParheader, DataTable objDt, DataTable dt, DataTable objapproval)
        {
            string year = "";
            string yearto = "";
            string period = "";
            int prev_gid = 0;
            try
            {
                getconnection();
                int pardetails = 0;
                if (objDt != null)
                {
                    if (objDt.Rows.Count < 0)
                    {
                        return "Please Enter Any Par Details Line";
                    }
                    else
                    {
                        int minVal = Convert.ToInt32(objDt.Compute("min(fromyear)", string.Empty));
                        int MaxVal = Convert.ToInt32(objDt.Compute("max(toyear)", string.Empty));
                        year = minVal.ToString();
                        yearto = MaxVal.ToString();
                        period = year + "-" + yearto;
                    }
                }
                if (dt != null && dt.Rows.Count > 0)
                {
                    period = dt.Rows[0]["parheader_period"].ToString();
                }
                //if (objapproval != null)
                //{
                //    if (objapproval.Rows.Count < 0)
                //    {
                //        return "Please Enter Any Par Approval Details";
                //    }
                //}
                if (HttpContext.Current.Session["paredit_Approval"] != null)
                {
                    List<CbfSumEntity> lst = new List<CbfSumEntity>();
                    lst = (List<CbfSumEntity>)HttpContext.Current.Session["paredit_Approval"];
                    if (lst.Count < 1)
                    {
                        return "Please Enter Any Par Approval Details";
                    }

                }
                DataTable dt2 = (DataTable)System.Web.HttpContext.Current.Session["AccessTokenheader"];
                DataTable dtattachment = (DataTable)System.Web.HttpContext.Current.Session["AccessToken"];
                cmdQry = new SqlCommand("pr_fb_parprocess", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;

                cmdQry.Parameters.Add("@parheader_refno", SqlDbType.VarChar).Value = objCmnFunctions.GetSequenceNoFb("PAR", "", "");
                //cmdQry.Parameters.Add("@parheader_date", SqlDbType.Date).Value = convertoDate(objParheader.ParDate.ToString());
                //cmdQry.Parameters.Add("@parheader_date", SqlDbType.VarChar).Value = "sysdatetime()";
                cmdQry.Parameters.Add("@parheader_date", SqlDbType.SmallDateTime).Value = DateTime.Now.ToString();
                cmdQry.Parameters.Add("@parheader_amt", SqlDbType.Decimal).Value = Convert.ToDecimal(objParheader.ParAmount);
                cmdQry.Parameters.Add("@parheader_enddate", SqlDbType.Date).Value = convertoDate(objParheader.FinalApprovalDate.ToString());
                //if (year != "" && yearto != "")
                //{
                //    period = year + "-" + yearto;
                //    cmdQry.Parameters.Add("@parheader_period", SqlDbType.VarChar).Value = period;
                //}
                cmdQry.Parameters.Add("@parheader_period", SqlDbType.VarChar).Value = period;
                cmdQry.Parameters.Add("@parheader_desc", SqlDbType.VarChar).Value = objParheader.pardescription;
                cmdQry.Parameters.Add("@parheader_Contigencypercent", SqlDbType.VarChar).Value = objParheader.contigency;
                cmdQry.Parameters.Add("@parheader_AmtWithContigency", SqlDbType.VarChar).Value = objParheader.contigencyamount;
                cmdQry.Parameters.Add("@parheader_isbudgeted", SqlDbType.VarChar).Value = objParheader.budgeted;
                cmdQry.Parameters.Add("@parheader_status", SqlDbType.Decimal).Value = 2;
                cmdQry.Parameters.Add("@parheader_gid", SqlDbType.VarChar).Value = HttpContext.Current.Session["parheadergid"];
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "Updateheader";
                int a = cmdQry.ExecuteNonQuery();
                int yourResult = (int)HttpContext.Current.Session["parheadergid"];
                if (a == 1)
                {
                    //if (dtattachment != null)
                    //{
                    //    if (dtattachment.Rows.Count > 0)
                    //    {
                    //        for (int i = 0; i < dtattachment.Rows.Count; i++)
                    //        {
                    //            getconnection();
                    //            cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                    //            cmdQry.CommandType = CommandType.StoredProcedure;
                    //            cmdQry.Parameters.Add("@cbfheader_gid", SqlDbType.Int).Value = yourResult;
                    //            cmdQry.Parameters.Add("@attachment_ref_flag", SqlDbType.TinyInt).Value = 9;
                    //            cmdQry.Parameters.Add("@attachment_attachmenttype_gid", SqlDbType.SmallInt).Value = 3;
                    //            cmdQry.Parameters.Add("@attachment_filename", SqlDbType.VarChar, 256).Value = dtattachment.Rows[i]["Documnet_Name"].ToString();
                    //            cmdQry.Parameters.Add("@attachment_desc", SqlDbType.VarChar).Value = dtattachment.Rows[i]["Document_des"].ToString();
                    //            cmdQry.Parameters.Add("@attachment_date", SqlDbType.Date).Value = convertoDate(dtattachment.Rows[i]["Attachment_Date"].ToString());
                    //            cmdQry.Parameters.Add("@attachment_by", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                    //            cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "attachment";
                    //            int a2 = cmdQry.ExecuteNonQuery();
                    //            if (a2 != 1)
                    //            {
                    //                return "Not Inserted PAR Header Attachment";
                    //            }
                    //        }
                    //    }
                    //}
                    if (objDt != null)
                    {
                        if (objDt.Rows.Count > 0)
                        {
                            for (int i = 0; i < objDt.Rows.Count; i++)
                            {
                                if (objDt.Columns.Contains("sno"))
                                {
                                    if (objDt.Rows[i]["sno"].ToString() != null || objDt.Rows[i]["sno"].ToString() != "")
                                    {
                                        getconnection();
                                        cmdQry = new SqlCommand("pr_fb_parprocess", objConn);
                                        cmdQry.CommandType = CommandType.StoredProcedure;
                                        cmdQry.Parameters.Add("@parheader_gid", SqlDbType.Int).Value = yourResult;
                                        cmdQry.Parameters.Add("@pardetails_expensetype", SqlDbType.VarChar).Value = objDt.Rows[i]["Expense"].ToString();
                                        cmdQry.Parameters.Add("@pardetails_requestfor_gid", SqlDbType.Int).Value = objDt.Rows[i]["department"].ToString();
                                        cmdQry.Parameters.Add("@pardetails_isbudgeted", SqlDbType.Char).Value = objParheader.budgeted;
                                        cmdQry.Parameters.Add("@pardetails_desc", SqlDbType.VarChar).Value = objDt.Rows[i]["Description"].ToString();
                                        cmdQry.Parameters.Add("@pardetails_year", SqlDbType.VarChar).Value = objDt.Rows[i]["fromyear"].ToString() + "-" + objDt.Rows[i]["toyear"].ToString();
                                        cmdQry.Parameters.Add("@pardetails_amt", SqlDbType.Decimal).Value = (objDt.Rows[i]["Amount"].ToString());
                                        cmdQry.Parameters.Add("@pardetails_remarks", SqlDbType.VarChar).Value = objDt.Rows[i]["remarks"].ToString();
                                        cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "Pardetails";
                                        int a2 = cmdQry.ExecuteNonQuery();
                                        if (a2 != 1)
                                        {
                                            return "Not Inserted PAR Details Line";
                                        }
                                        cmdQry = new SqlCommand("pr_fb_trn_attachmentall", objConn);
                                        cmdQry.CommandType = CommandType.StoredProcedure;
                                        cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectpardetailsgid";
                                        cmdQry.Parameters.Add("@cbfheadgid", SqlDbType.Int).Value = yourResult;
                                        cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                                        cmdQry.Parameters.Add("@process", SqlDbType.VarChar).Value = "PARTEMP";
                                        objDataAdapter = new SqlDataAdapter(cmdQry);
                                        DataTable objTable = new DataTable();
                                        string gid = "";
                                        objDataAdapter.Fill(objTable);
                                        //for (int git = 0; git < objTable.Rows.Count; git++)
                                        //{
                                        //    gid = objTable.Rows[git]["cbfdetails_gid"].ToString();
                                        //}
                                        string refflaggid = "";
                                        string cbfdetailsflag = "";
                                        if (objTable.Rows.Count > 0)
                                        {
                                            gid = objTable.Rows[0]["pardetails_gid"].ToString();
                                            refflaggid = objTable.Rows[0]["ref_flag"].ToString();
                                            cbfdetailsflag = objTable.Rows[0]["par_ref_flag"].ToString();
                                        }
                                        string[,] codes = new string[,]
	                                     {
                                             {"attachment_ref_gid", gid},
                                             {"attachment_ref_flag", cbfdetailsflag}
	                                        
                                         };
                                        string[,] whrcol = new string[,]
	                                        {
                                                {"attachment_ref_flag", refflaggid},
                                                {"attachment_ref_gid", Convert.ToString(i + 1)},
                                                {"attachment_by", objCmnFunctions.GetLoginUserGid().ToString()},
                                                {"attachment_attachmenttype_gid", "2"}
                                            };
                                        objCommonIUD.UpdateCommon(codes, whrcol, "iem_trn_tattachment");
                                    }
                                }
                                else
                                {
                                    getconnection();
                                    cmdQry = new SqlCommand("pr_fb_parprocess", objConn);
                                    cmdQry.CommandType = CommandType.StoredProcedure;
                                    cmdQry.Parameters.Add("@parheader_gid", SqlDbType.Int).Value = yourResult;
                                    cmdQry.Parameters.Add("@pardetails_gid", SqlDbType.Int).Value = objDt.Rows[i]["pardetails_gid"].ToString();
                                    cmdQry.Parameters.Add("@pardetails_expensetype", SqlDbType.VarChar).Value = objDt.Rows[i]["Expense"].ToString();
                                    cmdQry.Parameters.Add("@pardetails_requestfor_gid", SqlDbType.Int).Value = objDt.Rows[i]["department"].ToString();
                                    cmdQry.Parameters.Add("@pardetails_isbudgeted", SqlDbType.Char).Value = objParheader.budgeted;
                                    cmdQry.Parameters.Add("@pardetails_desc", SqlDbType.VarChar).Value = objDt.Rows[i]["Description"].ToString();
                                    cmdQry.Parameters.Add("@pardetails_year", SqlDbType.VarChar).Value = objDt.Rows[i]["fromyear"].ToString() + "-" + objDt.Rows[i]["toyear"].ToString();
                                    cmdQry.Parameters.Add("@pardetails_amt", SqlDbType.Decimal).Value = (objDt.Rows[i]["Amount"].ToString());
                                    cmdQry.Parameters.Add("@pardetails_remarks", SqlDbType.VarChar).Value = objDt.Rows[i]["remarks"].ToString();
                                    cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "updatedetails";
                                    int a2 = cmdQry.ExecuteNonQuery();
                                    if (a2 != 1)
                                    {
                                        return "Not Inserted PAR Details Line";
                                    }
                                    cmdQry = new SqlCommand("pr_fb_trn_attachmentall", objConn);
                                    cmdQry.CommandType = CommandType.StoredProcedure;
                                    cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectpardetailsgid";
                                    cmdQry.Parameters.Add("@cbfheadgid", SqlDbType.Int).Value = yourResult;
                                    cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                                    cmdQry.Parameters.Add("@process", SqlDbType.VarChar).Value = "PARTEMP";
                                    objDataAdapter = new SqlDataAdapter(cmdQry);
                                    DataTable objTable = new DataTable();
                                    string gid = "";
                                    objDataAdapter.Fill(objTable);
                                    //for (int git = 0; git < objTable.Rows.Count; git++)
                                    //{
                                    //    gid = objTable.Rows[git]["cbfdetails_gid"].ToString();
                                    //}
                                    string refflaggid = "";
                                    string cbfdetailsflag = "";
                                    if (objTable.Rows.Count > 0)
                                    {
                                        gid = objTable.Rows[0]["pardetails_gid"].ToString();
                                        refflaggid = objTable.Rows[0]["ref_flag"].ToString();
                                        cbfdetailsflag = objTable.Rows[0]["par_ref_flag"].ToString();
                                    }
                                    string[,] codes = new string[,]
	                                     {
                                             {"attachment_ref_gid", gid},
                                             {"attachment_ref_flag", cbfdetailsflag}
	                                        
                                         };
                                    string[,] whrcol = new string[,]
	                                        {
                                                {"attachment_ref_flag", refflaggid},
                                                {"attachment_ref_gid", Convert.ToString(i + 1)},
                                                {"attachment_by", objCmnFunctions.GetLoginUserGid().ToString()},
                                                {"attachment_attachmenttype_gid", "2"}
                                            };
                                    objCommonIUD.UpdateCommon(codes, whrcol, "iem_trn_tattachment");
                                }

                            }

                        }
                        else
                        {
                            return "Please Enter PAR Details Line Item";
                        }
                    }
                    //if (objapproval != null)
                    //{
                    //    //if (objapproval.Rows.Count > 0)
                    //{
                    //    for (int i = 0; i < objapproval.Rows.Count; i++)
                    //    {
                    //        if (objapproval.Rows[i]["parapprover_gid"].ToString() != "" || objapproval.Rows[i]["parapprover_gid"].ToString() != null)
                    //        {
                    //            getconnection();
                    //            cmdQry = new SqlCommand("pr_fb_parprocess", objConn);
                    //            cmdQry.CommandType = CommandType.StoredProcedure;
                    //            cmdQry.Parameters.Add("@parheader_gid", SqlDbType.Int).Value = yourResult;
                    //            cmdQry.Parameters.Add("@parapprover_emp_gid", SqlDbType.Int).Value = objapproval.Rows[i]["employee_gid"].ToString();
                    //            cmdQry.Parameters.Add("@parapprover_approval_date", SqlDbType.Date).Value = objapproval.Rows[i]["Approvaldate"].ToString();
                    //            cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "Parapproval";
                    //            int a2 = cmdQry.ExecuteNonQuery();
                    //            if (a2 != 1)
                    //            {
                    //                return "Not Inserted PAR Details Line";
                    //            }
                    //        }
                    //    }

                    //}
                    if (HttpContext.Current.Session["paredit_Approval"] != null)
                    {
                        List<CbfSumEntity> lst = new List<CbfSumEntity>();
                        lst = (List<CbfSumEntity>)HttpContext.Current.Session["paredit_Approval"];

                        for (int i = 0; i < lst.Count; i++)
                        {
                            //if (objapproval.Columns.Contains("parapprover_gid"))
                            //{
                            //    if (objapproval.Rows[i]["parapprover_gid"].ToString() != "" || objapproval.Rows[i]["parapprover_gid"].ToString() != null)
                            //    {

                            if (lst[i].approvalgid.ToString() == "")
                            {
                                getconnection();
                                cmdQry = new SqlCommand("pr_fb_parprocess", objConn);
                                cmdQry.CommandType = CommandType.StoredProcedure;
                                cmdQry.Parameters.Add("@parheader_gid", SqlDbType.Int).Value = yourResult;
                                cmdQry.Parameters.Add("@parapprover_emp_gid", SqlDbType.Int).Value = lst[i].employeeGid.ToString();
                                cmdQry.Parameters.Add("@parapprover_approval_date", SqlDbType.Date).Value = lst[i].Approvaldate.ToString();
                                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "Parapproval";
                                int a2 = cmdQry.ExecuteNonQuery();
                                if (a2 != 1)
                                {
                                    return "Not Inserted PAR Details Line";
                                }
                            }
                            //    }
                            //}
                        }
                    }
                    //}

                    if (HttpContext.Current.Session["par_ArrayList"] != null)
                    {
                        ArrayList lalistCbfDelete = (ArrayList)HttpContext.Current.Session["par_ArrayList"];
                        if (lalistCbfDelete.Count > 0)
                            for (int j = 0; j < lalistCbfDelete.Count; j++)
                            {
                                DeletePARapproval(lalistCbfDelete[j].ToString());
                            }

                    }
                    if (HttpContext.Current.Session["par_ArrayListpar"] != null)
                    {
                        ArrayList lalistCbfDelete = (ArrayList)HttpContext.Current.Session["par_ArrayListpar"];
                        if (lalistCbfDelete.Count > 0)
                            for (int j = 0; j < lalistCbfDelete.Count; j++)
                            {
                                DeletePARdetailsLine(lalistCbfDelete[j].ToString());
                            }

                    }

                    getconnection();
                    cmdQry = new SqlCommand("pr_fb_parprocess", objConn);
                    cmdQry.CommandType = CommandType.StoredProcedure;
                    cmdQry.Parameters.Add("@parheader_gid", SqlDbType.Int).Value = yourResult;
                    cmdQry.Parameters.Add("@employee_gid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                    cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "Parchecekr";

                    int a4 = cmdQry.ExecuteNonQuery();

                    if (objParheader.status == "submit")
                    {
                        //cmdQry = new SqlCommand("pr_fb_parprocess", objConn);
                        //cmdQry.CommandType = CommandType.StoredProcedure;
                        //cmdQry.Parameters.AddWithValue("@action", "selectrolegid");
                        //string role_gid = (string)cmdQry.ExecuteScalar();

                        //string[,] codesIN = new string[,]
                        //                  {
                        //                       {"queue_date","sysdatetime()"},
                        //                       {"queue_ref_flag", "9"},
                        //                       {"queue_ref_gid",yourResult.ToString() },
                        //                       {"queue_ref_status", "0"},
                        //                       {"queue_from",objCmnFunctions.GetLoginUserGid().ToString() } ,
                        //                       {"queue_to_type", "R"},
                        //                       {"queue_to", role_gid.ToString()},
                        //                       {"queue_action_for", "A"}
                        //                 };
                        //string tnameIN = "iem_trn_tqueue";
                        //string insertcommendecf = objCommonIUD.InsertCommon(codesIN, tnameIN);
                        DataTable dt1 = new DataTable();
                        cmdQry = new SqlCommand("pr_fb_trn_parqueue", objConn);
                        cmdQry.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(cmdQry);
                        da.Fill(dt1);
                        //cmdQry.Parameters.AddWithValue("@action", "selectrole");
                        // int roledf_gid = (int)cmdQry.ExecuteScalar();

                        string[,] codesIN = new string[,]
                                      {
                                           {"queue_date","sysdatetime()"},
                                           {"queue_ref_flag", "9"},
                                           {"queue_ref_gid",yourResult.ToString() },
                                           {"queue_ref_status", "0"},
                                           {"queue_from",objCmnFunctions.GetLoginUserGid().ToString() } ,
                                           {"queue_to_type", "R"},
                                           {"queue_prev_gid",prev_gid.ToString()},
                                           {"queue_to", dt1.Rows[0][0].ToString()},
                                           {"queue_action_for", "A"}
                                     };
                        string tnameIN = "iem_trn_tqueue";
                        string insertcommendecf = objCommonIUD.InsertCommon(codesIN, tnameIN);
                    }
                    if (objParheader.ApproverDeleteIDs != null && objParheader.ApproverDeleteIDs != "")
                    {
                        string[] DeleteIDs = objParheader.ApproverDeleteIDs.Split(',');
                        for (int kk = 0; kk < DeleteIDs.Length; kk++)
                        {
                            string[,] codes = new string[,]
                           {
                                {"parapprover_isremoved", "Y"}
                           };
                            string[,] whrcol = new string[,]
	                        {
                                {"parapprover_gid", DeleteIDs[kk]}
                            };
                            string insertcommand = objCommonIUD.UpdateCommon(codes, whrcol, "fb_trn_tparapprover");
                        }
                    }

                }
                else
                {
                    return "Not Inserted PAR Header Line";
                }
                return "Inserted Successfully";
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
           

        }
        public CbfSumEntity getParEdit(string lsparheader_gid)
        {
            CbfSumEntity obj = new CbfSumEntity();
            try
            {
                getconnection();
              
                DataSet ds = new DataSet();
                cmdQry = new SqlCommand("pr_fb_trn_pardetailsedit", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@parheader_gid", lsparheader_gid);
                cmdQry.Parameters.AddWithValue("@action", "parheaderdetails");
                objDataAdapter = new SqlDataAdapter(cmdQry);
                Parheader obj_getvalues;
                ParDetails obj_getpardetails;
                obj.ParheaderObj = new Parheader();
                obj.lListParDetails = new List<ParDetails>();
                objDataAdapter.Fill(ds);
                HttpContext.Current.Session["pardetails"] = ds.Tables[0];
                Employee_gid obj_getemployee;
                if (ds.Tables[0].Rows.Count > 0)
                {

                    obj_getvalues = new Parheader()
                    {
                        ParHeader_Gid = ds.Tables[0].Rows[0]["parheader_gid"].ToString(),
                        ParNo = ds.Tables[0].Rows[0]["parheader_refno"].ToString(),
                        ParDate = ds.Tables[0].Rows[0]["parheader_date"].ToString(),
                        pardescription = ds.Tables[0].Rows[0]["parheader_desc"].ToString(),
                        ParAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["parheader_amt"].ToString()) ,
                        FinalApprovalDate = ds.Tables[0].Rows[0]["parheader_enddate"].ToString(),
                        period = ds.Tables[0].Rows[0]["parheader_period"].ToString(),
                        contigency = ds.Tables[0].Rows[0]["parheader_Contigencypercent"].ToString(),
                        contigencyamount = Convert.ToString(ds.Tables[0].Rows[0]["parheader_AmtWithContigency"].ToString()),
                        budgeted = ds.Tables[0].Rows[0]["parheader_isbudgeted"].ToString()
                    };
                    obj.ParheaderObj = (obj_getvalues);

                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        obj_getpardetails = new ParDetails();

                        obj_getpardetails.ParDetails_Gid = Convert.ToInt32(ds.Tables[1].Rows[i]["pardetails_gid"].ToString());
                        obj_getpardetails.ParGid = Convert.ToInt32(ds.Tables[1].Rows[i]["pardetails_gid"].ToString());
                        obj_getpardetails.capexId = ds.Tables[1].Rows[i]["Expense"].ToString();
                        obj_getpardetails.Department = ds.Tables[1].Rows[i]["department"].ToString();
                        obj_getpardetails.budjectedid = ds.Tables[1].Rows[i]["bugdect"].ToString();
                        obj_getpardetails.Description = ds.Tables[1].Rows[i]["Description"].ToString();
                        obj_getpardetails.att_identify = ds.Tables[1].Rows[i]["pardetails_gid"].ToString();
                        string split = ds.Tables[1].Rows[i]["fromyear"].ToString();
                        string[] split1 = split.Split('-');
                        if (split1.Length > 0)
                        {
                            obj_getpardetails.FromYear = split1[0].ToString();
                        }
                        else
                        {
                            obj_getpardetails.FromYear = ds.Tables[1].Rows[i]["fromyear"].ToString();
                        }
                        string splitto = ds.Tables[1].Rows[i]["fromyear"].ToString();
                        string[] splitto1 = splitto.Split('-');
                        if (splitto1.Length > 0)
                        {
                            obj_getpardetails.ToYear = splitto1[1].ToString();
                        }
                        else
                        {
                            obj_getpardetails.ToYear = ds.Tables[1].Rows[i]["toyear"].ToString();
                        }
                        obj_getpardetails.ParAmount = Convert.ToInt32(ds.Tables[1].Rows[i]["Amount"].ToString());
                        obj_getpardetails.Remarks = ds.Tables[1].Rows[i]["remarks"].ToString();

                        obj.lListParDetails.Add(obj_getpardetails);

                    }
                }
                HttpContext.Current.Session["pardetailsgid"] = ds.Tables[1];
                obj.attachment = new List<Attachment>();
                obj.attachmentCbfdetails = new List<Attachment>();
                if (ds.Tables[3].Rows.Count > 0 || ds.Tables[3].Rows.Count != null)
                {
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        Attachment attachment1 = new Attachment()
                        {
                            fileName = ds.Tables[3].Rows[i]["Documnet_Name"].ToString(),
                            attachedDate = ds.Tables[3].Rows[i]["Attachment_Date"].ToString(),
                            description = ds.Tables[3].Rows[i]["Document_des"].ToString(),
                            attachGid = ds.Tables[3].Rows[i]["Sno"].ToString(),
                            //FileTempName = ds.Tables[3].Rows[i]["FileTempName"].ToString()
                        };
                        obj.attachment.Add(attachment1);
                    }
                }
                if (ds.Tables[2].Rows.Count > 0 || ds.Tables[2].Rows.Count != null)
                {
                    for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                    {
                        Attachment attachment1 = new Attachment()
                        {
                            fileName = ds.Tables[4].Rows[i]["Documnet_Name"].ToString(),
                            attachedDate = ds.Tables[4].Rows[i]["Attachment_Date"].ToString(),
                            description = ds.Tables[4].Rows[i]["Document_des"].ToString(),
                            attachGid = ds.Tables[4].Rows[i]["Sno"].ToString(),
                        };
                        obj.attachmentCbfdetails.Add(attachment1);
                    }
                }
                obj.Employeedetails = new List<Employee_gid>();
                if (ds.Tables[4].Rows.Count > 0 || ds.Tables[4].Rows.Count != null)
                {
                    List<CbfSumEntity> LST = new List<CbfSumEntity>();
                    CbfSumEntity objModel;
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        objModel = new CbfSumEntity();
                        objModel.slNo = i + 1;
                        objModel.approvalgid = ds.Tables[2].Rows[i]["parapprover_gid"].ToString();
                        objModel.employeeGid = Convert.ToInt32(ds.Tables[2].Rows[i]["employee_gid"]);
                        objModel.empCode = ds.Tables[2].Rows[i]["employee_code"].ToString();
                        objModel.empName = ds.Tables[2].Rows[i]["employee_name"].ToString();
                        objModel.empdesignation = ds.Tables[2].Rows[i]["employee_designation"].ToString();
                        objModel.Approvaldate = ds.Tables[2].Rows[i]["Approvaldate"].ToString();
                        LST.Add(objModel);
                    }
                    HttpContext.Current.Session["paredit_Approval"] = LST;
                }
                HttpContext.Current.Session["approvals"] = ds.Tables[2];
                return obj;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }
            finally
            {
                objConn.Close();
            }
        }
        public string DeletePAR()
        {
            try
            {
                string deletetbl = "";
                getconnection();
                DataTable dt = new DataTable();
                cmdQry = new SqlCommand("pr_fb_trn_parDeleteCheck", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@actionName", "deletecheck");
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["parheader_gid"].ToString() == HttpContext.Current.Session["parheadergid"].ToString())
                        {
                            string[,] wherecond = new string[,]
	                            {
                                    {"parheader_gid", HttpContext.Current.Session["parheadergid"].ToString()}
                                };
                            string[,] column = new string[,]
	                           {
                                     {"parheader_isremoved", "Y"}
	            
                               };
                            string tblname = "fb_trn_tparheader";
                            deletetbl = objCommonIUD.DeleteCommon(column, wherecond, tblname);
                        }

                    }
                }
                //string Strquery = "select * from fb_trn_parheader inner join fb_trn_tpardetails on ";
                //Strquery += " parheader_gid=pardetails_parhead_gid in where cbfheader_gid='" + HttpContext.Current.Session["parheadergid"].ToString() + "'";

                //cmdQry = new SqlCommand(Strquery, objConn);
                //cmdQry.CommandType = CommandType.Text;
                //objDataAdapter = new SqlDataAdapter(cmdQry);
                //DataTable Objdt = new DataTable();
                //objDataAdapter.Fill(Objdt);
                //if (Objdt.Rows.Count > 0)
                //{
                //    deletetbl = "Already Used";
                //}
                //else
                //{

                //}

                return deletetbl;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
        }
        public CbfSumEntity GetParchecker()
        {
            CbfSumEntity obj = new CbfSumEntity();
            try
            {
                DataTable dt = new DataTable();
                getconnection();
             
                Parheader obj_getvalues;
                obj.ListParHeader = new List<Parheader>();
                cmdQry = new SqlCommand("pr_fb_trn_parsummary", objConn);
                cmdQry.Parameters.AddWithValue("@action", "parchecker");
                cmdQry.Parameters.AddWithValue("@employee_gid", objCmnFunctions.GetLoginUserGid());
                cmdQry.CommandType = CommandType.StoredProcedure;
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        obj_getvalues = new Parheader()
                        {
                            ParHeader_Gid = dt.Rows[i]["parheader_gid"].ToString(),
                            ParNo = dt.Rows[i]["parheader_refno"].ToString(),
                            ParDate = dt.Rows[i]["parheader_date"].ToString(),
                            pardescription = dt.Rows[i]["parheader_desc"].ToString(),

                        };
                        obj.ListParHeader.Add(obj_getvalues);
                    }
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
                objConn.Close();
            }
        }
        public string parcheckerapproval(string remarks)
        {
            try
            {
                getconnection();
                string result = "";
                cmdQry = new SqlCommand("pr_fb_trn_parsummary", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@employee_gid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmdQry.Parameters.Add("@parchecker_remarks", SqlDbType.VarChar).Value = remarks;
                cmdQry.Parameters.Add("@parchecker_status", SqlDbType.VarChar).Value = "A";
                cmdQry.Parameters.Add("@parheader_gid", SqlDbType.Int).Value = HttpContext.Current.Session["parheadergid"];

                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "parcheckerapproval";
                int a3 = cmdQry.ExecuteNonQuery();

                if (a3 == 1)
                {
                    string[,] codesq = new string[,]
                                    {
                                         {"queue_action_date","sysdatetime()"},
                                         {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString() },
                                         {"queue_action_status","1" },
                                         {"queue_isremoved","Y" },
                                         {"queue_prev_gid","0"},
                                         {"queue_action_remark",remarks.ToString()}
                                     };
                    string[,] whreq = new string[,]
                                    {
                                     
                                         {"queue_ref_gid", Convert.ToString(HttpContext.Current.Session["parheadergid"])},
                                         {"queue_isremoved","N" },
                                         {"queue_ref_flag","9"}
                                   };
                    string tnameq = "iem_trn_tqueue";
                    string insertcommendq = objCommonIUD.UpdateCommon(codesq, whreq, tnameq);

                }
                return a3.ToString();
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }

        }
        public string parcheckerReject(string remarks)
        {
            try
            {
                getconnection();
                string result = "";
                cmdQry = new SqlCommand("pr_fb_trn_parsummary", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@employee_gid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmdQry.Parameters.Add("@parchecker_remarks", SqlDbType.VarChar).Value = remarks;
                cmdQry.Parameters.Add("@parchecker_status", SqlDbType.VarChar).Value = "R";
                cmdQry.Parameters.Add("@parheader_gid", SqlDbType.Int).Value = HttpContext.Current.Session["parheadergid"];

                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "parcheckerapproval";
                int a3 = cmdQry.ExecuteNonQuery();
                if (a3 == 1)
                {
                    string[,] columnNames = new string[,]
                                    {
                                         {"queue_action_date","sysdatetime()"},
                                         {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                                         {"queue_action_status","0"},
                                         {"queue_action_for","R"},
                                         {"queue_isremoved","Y" },
                                         {"queue_prev_gid","0"},
                                         {"queue_action_remark",remarks.ToString()}
                                     };
                    string[,] whreq = new string[,]
                                    {
                                     
                                         {"queue_ref_gid", Convert.ToString(HttpContext.Current.Session["parheadergid"])},
                                         {"queue_isremoved","N" },
                                         {"queue_ref_flag","9"}
                                   };
                    string tnameq = "iem_trn_tqueue";
                    string insertcommendq = objCommonIUD.UpdateCommon(columnNames, whreq, tnameq);
                }
                return a3.ToString();
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }

        }
        public CbfSumEntity GetCBFSummaryHeader()
        {
            CbfSumEntity objSumEntity = new CbfSumEntity();
            try
            {
                getconnection();
              
                CbfCheckerSummary obj_getvalues;
                objSumEntity.cbfCheckersumamry = new List<CbfCheckerSummary>();
                DataTable dt = new DataTable();

                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "cbfvendorselection";
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        obj_getvalues = new CbfCheckerSummary()
                        {
                            cbFGidChecker = Convert.ToInt32(dt.Rows[i]["cbfheader_gid"].ToString()),
                            cbfNoChecker = dt.Rows[i]["cbfheader_cbfno"].ToString(),
                            cbfDateChecker = (dt.Rows[i]["cbfheader_date"].ToString()),
                            cbfEndDateChecker = (dt.Rows[i]["cbfheader_enddate"].ToString()),
                            cbfProjectOwnerChecker = dt.Rows[i]["cbfheader_projectowner"].ToString(),
                            cbfDevi_AmoChecker = Convert.ToDecimal(dt.Rows[i]["cbfheader_Devi_amt"].ToString()),
                            cbfAmoChecker = Convert.ToDecimal(dt.Rows[i]["cbfheader_cbfamt"].ToString()),
                            cbfDescriptionChecker = dt.Rows[i]["cbfheader_desc"].ToString(),
                            fincon_Bugchecker = dt.Rows[i]["cbfheader_isbudgeted"].ToString(),
                            cbfrequestForChecker = dt.Rows[i]["cbfheader_department"].ToString(),
                            cbfStatusChecker = dt.Rows[i]["cbfheader_status"].ToString(),
                            cbfModeChecker = dt.Rows[i]["cbfheader_mode"].ToString()
                        };

                        objSumEntity.cbfCheckersumamry.Add(obj_getvalues);
                    }
                }
                return objSumEntity;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objSumEntity;
            }
            finally
            {
                objConn.Close();
            }
        }
        public CbfSumEntity GetUltilizedamount(CbfSumEntity objCrheader)
        {
            CbfSumEntity objchecker = objCrheader;
            try
            {
                getconnection();
                DataTable dt = new DataTable();
               
                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "cbfutilizedamoun";
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);
                CbfUtilizedAmount Unlitzed = new CbfUtilizedAmount();
                if (dt.Rows.Count > 0)
                {

                    Unlitzed.cbfAmount = Convert.ToDecimal(dt.Rows[0]["cbfheader_cbfamt"].ToString());
                    Unlitzed.cbfBalance = Convert.ToDecimal(dt.Rows[0]["Balacnceamount"].ToString());
                    Unlitzed.cbfUtilized = Convert.ToDecimal(dt.Rows[0]["Utilizedamount"].ToString());

                }
                objchecker.Utilized = Unlitzed;
                return objchecker;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objchecker;
            }
            finally
            {
                objConn.Close();
            }
        }
        public List<vendorselection> getvendorselection()
        {
            List<vendorselection> lstvendor = new List<vendorselection>();
            try
            {
                getconnection();
                
                vendorselection ObjVendor;
                cmdQry = new SqlCommand("select supplierheader_gid,supplierheader_name,supplierheader_suppliercode from asms_trn_tsupplierheader where supplierheader_status='APPROVED'  and supplierheader_isremoved='N' order by supplierheader_gid", objConn);
                objDataAdapter = new SqlDataAdapter(cmdQry);
                DataTable ObjDt = new DataTable();
                objDataAdapter.Fill(ObjDt);
                if (ObjDt.Rows.Count > 0)
                {
                    for (int i = 0; i < ObjDt.Rows.Count; i++)
                    {
                        ObjVendor = new vendorselection();
                        ObjVendor.vendorgid = Convert.ToInt32(ObjDt.Rows[i]["supplierheader_gid"].ToString());
                        ObjVendor.vendorname = ObjDt.Rows[i]["supplierheader_name"].ToString();
                        ObjVendor.vendorcode = ObjDt.Rows[i]["supplierheader_suppliercode"].ToString();
                        lstvendor.Add(ObjVendor);
                    }
                }
                return lstvendor;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return lstvendor;
            }
            finally
            {
                objConn.Close();
            }
        }
        public CbfSumEntity Getvendorgridcopex(DataTable dt)
        {
            CbfSumEntity obj = new CbfSumEntity();
            try
            {
                getconnection();
             
                CbfCheckerDetails obj_getcbfdetails;
                obj.cbfCheckerDetails = new List<CbfCheckerDetails>();
                foreach (DataRow rows in dt.Rows)
                {
                    obj_getcbfdetails = new CbfCheckerDetails()
                    {
                        productCodeChecker = rows["prodservice_code"].ToString(),
                        cbfDetailGidChecker = Convert.ToInt32(rows["cbfdetails_gid"].ToString()),
                        productNameChecker = rows["prodservice_name"].ToString(),
                        descriptionChecker = rows["prodservice_description"].ToString(),
                        description1Checker = rows["cbfdetails_desc"].ToString(),
                        uomChecker = rows["uom_code"].ToString(),
                        qtyChecker = Convert.ToDecimal(rows["prdetails_qty"].ToString()),
                        unit_AmtChecker = Convert.ToDecimal(rows["pipinput_rate"].ToString()),
                        totalChecker = Convert.ToDecimal(rows["pipinput_costestimation"].ToString()),
                        remarksChecker = rows["remarks"].ToString(),
                        chartOfAccountChecker = rows["assetcategory_glno"].ToString(),
                        fcccChecker = Convert.ToInt32(rows["fccc"].ToString()),
                        budgetLineChecker = Convert.ToInt32(rows["budgetline"].ToString()),
                        prdetailsGidChecker = Convert.ToInt64(rows["prdetails_gid"].ToString()),
                        vendorgid = ((rows["vendorgid"].ToString() != "" && rows["vendorgid"].ToString() != null) ? Convert.ToInt32(rows["vendorgid"].ToString()) : 0),
                        vendorselection = ((rows["vendorname"].ToString() != "" && rows["vendorname"].ToString() != null) ? rows["vendorname"].ToString() : ""),

                    };
                    obj.cbfCheckerDetails.Add(obj_getcbfdetails);
                }
                return (obj);

            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;  
            }
            finally
            {
                objConn.Close();
            }
        }
        public string vendorselectionsave()
        {
            try
            {
                DataTable ObjDt = new DataTable();
                ObjDt= (DataTable)HttpContext.Current.Session["vendorselection"];
                string cbfheadergid = Convert.ToString(HttpContext.Current.Session["cbfheadergid"]);
                string insertcommend = "";
                if (ObjDt !=null)
                {
                    for (int i = 0; i < ObjDt.Rows.Count; i++)
                    {

                        string[,] codes1 = new string[,]            
	                {
                        {"cbfdetails_vendor_gid",string.IsNullOrEmpty(ObjDt.Rows[i]["vendorgid"].ToString())?"0":ObjDt.Rows[i]["vendorgid"].ToString()},
                       
                    };
                        string[,] whrcol = new string[,]
	                 {
                          {"cbfdetails_gid",ObjDt.Rows[i]["cbfdetails_gid"].ToString()}
                          
                     };
                        string tname1 = "fb_trn_tcbfdetails";
                        insertcommend = objCommonIUD.UpdateCommon(codes1, whrcol, tname1);


                    }

                }
                if (cbfheadergid !=null)
                {

                    string[,] codescbfh = new string[,]
                    {
       
                       {"cbfheader_IsVendorApprovedAt","sysdatetime()" },
                       {"cbfheader_IsvendorapprovedBy",objCmnFunctions.GetLoginUserGid().ToString()},
                       {"cbfheader_Isvendorapproved","Y" }

                     };
                  string[,] whrecbfh = new string[,]
                        {
                                     
                          {"cbfheader_gid",cbfheadergid},       
                          {"cbfheader_isremoved","N" } 
                         };

                    string tnamecbfh = "fb_trn_tcbfheader";
                      insertcommend = objCommonIUD.UpdateCommon(codescbfh, whrecbfh, tnamecbfh);
                }

                    if (insertcommend == "Success")
                    {
                        return "Inserted Successfully";
                    }
                    else
                    {
                        return "Not Inserted Successfully";
                    }

                
                //else
                //{
                //    return "Please Select Any Line Item";
                //}
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
          

        }
        public List<CbfRaiseHeader> GetBudgetowner()
        {
            List<CbfRaiseHeader> objproduct = new List<CbfRaiseHeader>();
            try
            {
             
                CbfRaiseHeader objraiser;
                getconnection();
                DataTable dt = new DataTable();
                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@employee_gid", objCmnFunctions.GetLoginUserGid().ToString());
                cmdQry.Parameters.AddWithValue("@action", "selectbudgetowner");
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    objraiser = new CbfRaiseHeader()
                    {
                        BudgetownerGid = Convert.ToInt32(row["budgetowner_employeegid"].ToString()),
                        BudgetOwner_Name = row["budgetowner_name"].ToString(),

                    };
                    objproduct.Add(objraiser);
                }

                return objproduct;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objproduct;
            }
            finally
            {
                objConn.Close();
            }
          
        }
        public int GetBudgetownerGid(string lsNumber)
        {
            try
            {
                getconnection();
                cmdQry = new SqlCommand("pr_fb_trn_cbfsummary", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@employee_gid", Convert.ToInt32(lsNumber));
                cmdQry.Parameters.AddWithValue("@action", "selectbudgetownerid");
                int budgetownerid = Convert.ToInt32(cmdQry.ExecuteScalar());
                return budgetownerid;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                objConn.Close();
            }
        }
        public List<ParDetails> GetCapex()
        {
            try
            {
                CbfSumEntity oBjOpex = new CbfSumEntity();
                oBjOpex.lListParDetails = new List<ParDetails>();
                oBjOpex.lListParDetails.Add(new ParDetails { capexId = "C", capexname = "Capex", });
                oBjOpex.lListParDetails.Add(new ParDetails { capexId = "O", capexname = "Opex", });
                return oBjOpex.lListParDetails;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                List<ParDetails> lst = new List<ParDetails>();
                return lst;
            }
            finally
            {
                objConn.Close();
            }
        }
        public List<ParDetails> GetBudjected()
        {
            try
            {
                CbfSumEntity oBjOpex = new CbfSumEntity();
                oBjOpex.lListParDetails = new List<ParDetails>();
                oBjOpex.lListParDetails.Add(new ParDetails { budjectedid = "Y", budjectedname = "Yes", });
                oBjOpex.lListParDetails.Add(new ParDetails { budjectedid = "N", budjectedname = "No", });
                return oBjOpex.lListParDetails;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                List<ParDetails> lst = new List<ParDetails>();
                return lst;
            }
            finally
            {
                objConn.Close();
            }

        }

        public List<Attachment> EditcbfAttachmentList(DataTable dt)
        {
            List<Attachment> lstattach = new List<Attachment>();
            try
            {
                PrSumEntity obj = new PrSumEntity();
                Attachment obj_getattach;
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        obj_getattach = new Attachment();
                        obj_getattach.fileName = dt.Rows[i]["Documnet_Name"].ToString();
                        obj_getattach.attachedDate = dt.Rows[i]["Attachment_Date"].ToString();
                        obj_getattach.description = dt.Rows[i]["Document_des"].ToString();
                        lstattach.Add(obj_getattach);
                    }
                }
                else
                {
                    lstattach = new List<Attachment>();
                }
                return lstattach;


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return lstattach;
            }
            finally
            {
                objConn.Close();
            }


        }

        public CbfSumEntity cbfAttachmentname(CbfSumEntity filename)
        {
            CbfSumEntity obj = new CbfSumEntity();
            try
            {
                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();
               
                Attachment obj_getprdetails;
                obj.attachment = new List<Attachment>();
                if (filename.attachment == null || filename.attachment1 == "")
                {
                    if (HttpContext.Current.Session["prheader_AccessToken"] != "" && HttpContext.Current.Session["prheader_AccessToken"] != null)
                    {
                        dt = (DataTable)HttpContext.Current.Session["prheader_AccessToken"];
                    }
                    else
                    {
                        dt.Columns.Add("Documnet_Name", typeof(string));
                        dt.Columns.Add("Attachment_Date", typeof(string));
                        dt.Columns.Add("Document_des", typeof(string));
                    }
                    if ((filename.attachmentDate != "" && filename.attachment1 != "" && filename.des != "") && (filename.attachmentDate != null && filename.attachment1 != null && filename.des != null))
                    {
                        dt.Rows.Add(filename.attachment, convertoDate(filename.attachmentDate.ToString()), filename.des);
                    }

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            obj_getprdetails = new Attachment();
                            obj_getprdetails.attachedDate = dt.Rows[i]["Attachment_Date"].ToString();
                            obj_getprdetails.fileName = dt.Rows[i]["Documnet_Name"].ToString();
                            obj_getprdetails.description = dt.Rows[i]["Document_des"].ToString();
                            obj.attachment.Add(obj_getprdetails);
                        }
                    }
                    System.Web.HttpContext.Current.Session["prheader_AccessToken"] = dt;
                }
                else
                {
                    if (HttpContext.Current.Session["prheader_AccessTokenheader"] != "" && HttpContext.Current.Session["prheader_AccessTokenheader"] != null)
                    {
                        dt2 = (DataTable)HttpContext.Current.Session["prheader_AccessTokenheader"];
                    }
                    else
                    {
                        dt2.Columns.Add("prdetails", typeof(string));
                        dt2.Columns.Add("Documnet_Name", typeof(string));
                        dt2.Columns.Add("Attachment_Date", typeof(string));
                        dt2.Columns.Add("Document_des", typeof(string));
                    }
                    if ((filename.attachmentDate != "" && filename.attachment1 != "" && filename.des != "") && (filename.attachmentDate != null && filename.attachment1 != null && filename.des != null))
                    {
                        dt2.Rows.Add(filename.attachment, filename.attachment1, convertoDate(filename.attachmentDate.ToString()), filename.des);
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            obj_getprdetails = new Attachment();
                            obj_getprdetails.attachGid = Convert.ToString(i + 1);
                            obj_getprdetails.attachedDate = string.IsNullOrEmpty(dt2.Rows[i]["Attachment_Date"].ToString()) ? dt2.Rows[i]["AttachmentDate"].ToString() : dt2.Rows[i]["Attachment_Date"].ToString();
                            obj_getprdetails.fileName = string.IsNullOrEmpty(dt2.Rows[i]["Documnet_Name"].ToString()) ? dt2.Rows[i]["Filename"].ToString() : dt2.Rows[i]["Documnet_Name"].ToString();
                            obj_getprdetails.description = string.IsNullOrEmpty(dt2.Rows[i]["Document_des"].ToString()) ? dt2.Rows[i]["AttachmentDescription"].ToString() : dt2.Rows[i]["Document_des"].ToString();
                            obj.attachment.Add(obj_getprdetails);
                        }
                    }
                    System.Web.HttpContext.Current.Session["prheader_AccessTokenheader"] = dt2;
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
                objConn.Close();
            }
        }


        //-----------09-09-2015 START -----------------
        //public List<CbfSumEntity> PopulateAuditTrail(CbfSumEntity pat)
        //{
        //    try
        //    {
        //        List<CbfSumEntity> objDashBoard = new List<CbfSumEntity>();
        //        CbfSumEntity objModel;
        //        getconnection();
        //        DataTable dt = new DataTable();
        //        DataTable dtr = new DataTable();
        //        string streject = "";
        //        string strejectnew = "";
        //        string status = "";
        //        string str = "";


        //        cmdQry = new SqlCommand("pr_fb_trn_audittaril", objConn);
        //        cmdQry.Parameters.AddWithValue("@gid", pat.gid);
        //        cmdQry.Parameters.AddWithValue("@refflag", pat.ref_flag);
        //        cmdQry.CommandType = CommandType.StoredProcedure;
        //        objDataAdapter = new SqlDataAdapter(cmdQry);
        //        objDataAdapter.Fill(dt);
        //        if (dt.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                objModel = new CbfSumEntity();
        //                string employee_code = string.Empty;
        //                string pergid = Convert.ToString(dt.Rows[i]["queue_prev_gid"].ToString());
        //                //string cbfstatus = Convert.ToString(dt.Rows[i]["cbfheader_status"].ToString());
        //                //if (pat.ref_flag == "2")
        //                //{

        //                //    if (cbfstatus == "2")
        //                //    {
        //                //    string raiser = Convert.ToString(dt.Rows[i]["cbfheader_rasier_gid"].ToString());
        //                //    string empcodnamer = Getempname(raiser);
        //                //    string[] datar;
        //                //    datar = empcodnamer.Split(',');
        //                //    objModel = new CbfSumEntity();
        //                //    objModel.employee_code = datar[0].ToString() + "-" + datar[1].ToString();
        //                //    objModel.action_status = "Submitted";
        //                //    objModel.action_remarks = Convert.ToString(dt.Rows[i]["cbfheader_remarks"].ToString());
        //                //    objModel.action_date = Convert.ToString(dt.Rows[i]["cbfheader_date"].ToString());
        //                //    objModel.queue_to_type = "CBF Raiser";
        //                //    objDashBoard.Add(objModel);

        //                //      }
        //                //}
        //                //if (pat.ref_flag == "9")
        //                //{
        //                //    string parstatus = Convert.ToString(dt.Rows[i]["parheader_status"].ToString());
        //                //    if (parstatus == "2")
        //                //    {
        //                //    string raiser = Convert.ToString(dt.Rows[i]["parheader_raiser"].ToString());
        //                //    string empcodnamer = Getempname(raiser);
        //                //    string[] datar;
        //                //    datar = empcodnamer.Split(',');
        //                //    objModel = new CbfSumEntity();
        //                //    objModel.employee_code = datar[0].ToString() + "-" + datar[1].ToString();
        //                //    objModel.action_status = "Submitted";
        //                //    objModel.action_remarks = Convert.ToString(dt.Rows[i]["parheader_remarks"].ToString());
        //                //    objModel.action_date = Convert.ToString(dt.Rows[i]["parheader_date"].ToString());
        //                //    objModel.queue_to_type = "PAR Raiser";
        //                //    objDashBoard.Add(objModel);

        //                //     }
        //                //}
        //                if (pergid == "0")
        //                {
        //                    //if (cbfstatus != "2")
        //                    //{
        //                    streject = Convert.ToString(dt.Rows[i]["queue_from"].ToString());
        //                    string empcodnamer = Getempname(streject);
        //                    string[] datar;
        //                    datar = empcodnamer.Split(',');
        //                    if (i == 0)
        //                    {
        //                        HttpContext.Current.Session["empcode"] = datar[0].ToString();
        //                        HttpContext.Current.Session["empname"] = datar[1].ToString();
        //                    }
        //                    objModel = new CbfSumEntity();
        //                    objModel.employee_code = datar[0].ToString() + "-" + datar[1].ToString();
        //                    objModel.action_status = "Submitted";
        //                    if (pat.ref_flag == "9")
        //                    {
        //                        objModel.action_remarks = "";
        //                        objModel.action_date = Convert.ToString(dt.Rows[i]["queue_date"].ToString());
        //                        objModel.queue_to_type = "PAR Raiser";
        //                    }
        //                    if (pat.ref_flag == "2")
        //                    {
        //                        objModel.action_remarks = Convert.ToString(dt.Rows[i]["cbfheader_remarks"].ToString());
        //                        objModel.action_date = Convert.ToString(dt.Rows[i]["queue_date"].ToString());
        //                        objModel.queue_to_type = "CBF Raiser";
        //                    }
        //                    string actionfor = "";
        //                    if (i > 0)
        //                    {
        //                        actionfor = Convert.ToString(dt.Rows[i - 1]["queue_action_for"].ToString());
        //                    }

        //                    //string ref_no = Convert.ToString(dt.Rows[i]["queue_ref_status"].ToString());
        //                    if (actionfor == "R")
        //                    {
        //                        objModel.action_status = "ReSubmitted";
        //                    }

        //                    objDashBoard.Add(objModel);
        //                    //}
        //                    string actions = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                    if (actions != "")
        //                    {

        //                        string empid = Convert.ToString(dt.Rows[i]["queue_action_by"].ToString());
        //                        if (objModel.action_status == "Submitted" && empid == "")
        //                        {
        //                            empid = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
        //                        }
        //                        if (empid != "")
        //                        {
        //                            string empcodname = Getempname(empid);
        //                            string[] data;
        //                            data = empcodname.Split(',');
        //                            objModel = new CbfSumEntity();
        //                            objModel.employee_code = data[0].ToString() + "-" + data[1].ToString();
        //                        }
        //                        else
        //                        {
        //                            //objModel = new CbfSumEntity();
        //                            objModel.employee_code = "";
        //                        }
        //                        objModel.action_date = Convert.ToString(dt.Rows[i]["queue_action_date"].ToString());
        //                        objModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());
        //                        status = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                        objModel.action_status = GetQueueStatusHistry(status);
        //                        string actionfor1 = Convert.ToString(dt.Rows[i]["queue_action_for"].ToString());
        //                        if (actionfor1 == "R")
        //                        {
        //                            objModel.action_status = "Rejected";
        //                        }
        //                        if (pat.ref_flag == "2")
        //                        {
        //                            objModel.queue_to_type = "CBF Checker";
        //                        }
        //                        if (pat.ref_flag == "9")
        //                        {
        //                            string queue_type = Convert.ToString(dt.Rows[i]["queue_to_type"].ToString());
        //                            string queue_to = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
        //                            objModel.queue_to_type = GetQueueRole(queue_type, "12", queue_to);
        //                        }
        //                        if (objModel.queue_to_type == "PAR Checker" && objModel.action_status == "Pending")
        //                        {
        //                            objModel.employee_code = "";
        //                        }

        //                        objDashBoard.Add(objModel);
        //                    }
        //                }
        //                else
        //                {

        //                    string queue_type = Convert.ToString(dt.Rows[i]["queue_to_type"].ToString());
        //                    string queue_to = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
        //                    string actionstt = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                    if (actionstt != "")
        //                    {
        //                        string empid = "";
        //                        objModel = new CbfSumEntity();
        //                        if (queue_type == "I" || queue_type == "E")
        //                        {
        //                            empid = Convert.ToString(dt.Rows[i]["queue_to"].ToString());

        //                        }
        //                        else if (queue_type == "G" || queue_type == "R" || queue_type == "L" || queue_type == "D")
        //                        {
        //                            empid = Convert.ToString(dt.Rows[i]["queue_action_by"].ToString());
        //                        }
        //                        if (empid != "")
        //                        {
        //                            string empcodname = Getempname(empid);
        //                            string[] data;
        //                            data = empcodname.Split(',');
        //                            objModel = new CbfSumEntity();
        //                            objModel.employee_code = data[0].ToString() + "-" + data[1].ToString();
        //                            employee_code = objModel.employee_code;
        //                        }
        //                        else
        //                        {
        //                            objModel.employee_code = "";
        //                            if (pat.ref_flag == "9")
        //                            {
        //                                string raiser_gid = Convert.ToString(dt.Rows[i]["parheader_raiser_gid"].ToString());
        //                                string empcodname1 = Getempname(raiser_gid);
        //                                string[] data;
        //                                data = empcodname1.Split(',');
        //                                objModel = new CbfSumEntity();

        //                                objModel.employee_code = data[0].ToString() + "-" + data[1].ToString();
        //                                employee_code = objModel.employee_code;
        //                                objModel.action_date = Convert.ToString(dt.Rows[i]["queue_date"].ToString());
        //                                objModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());
        //                                status = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                                objModel.action_status = GetQueueStatusHistry(status);
        //                                string actionfor = "";
        //                                if (i > 0)
        //                                {
        //                                    actionfor = Convert.ToString(dt.Rows[i - 1]["queue_action_for"].ToString());
        //                                }
        //                                //string ref_no = Convert.ToString(dt.Rows[i]["queue_ref_status"].ToString());
        //                                if (actionfor == "R")
        //                                {
        //                                    objModel.action_status = "ReSubmitted";
        //                                }

        //                                objModel.queue_to_type = "PAR Raiser";
        //                                string actionfor1 = Convert.ToString(dt.Rows[i]["queue_action_for"].ToString());
        //                                if (actionfor1 == "R")
        //                                {
        //                                    objModel.action_status = "Rejected";
        //                                }
        //                                objDashBoard.Add(objModel);
        //                            }
        //                        }
        //                        objModel = new CbfSumEntity();
        //                        objModel.employee_code = employee_code;

        //                        objModel.action_date = Convert.ToString(dt.Rows[i]["queue_action_date"].ToString());
        //                        objModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());
        //                        status = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                        objModel.action_status = GetQueueStatusHistry(status);
        //                        string actionfor11 = Convert.ToString(dt.Rows[i]["queue_action_for"].ToString());
        //                        if (actionfor11 == "R")
        //                        {
        //                            objModel.action_status = "Rejected";
        //                        }
        //                        objModel.queue_to_type = GetQueueRole(queue_type, "2", queue_to);
        //                        if (objModel.queue_to_type == "PAR Checker" && objModel.action_status == "Pending")
        //                        {
        //                            objModel.employee_code = "";
        //                        }
        //                        objDashBoard.Add(objModel);

        //                    }
        //                }

        //            }
        //        }
        //        return objDashBoard;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        objConn.Close();
        //    }
        //}
        //-----------09-09-2015 END -----------------
        public List<CbfSumEntity> PopulateAuditTrail(CbfSumEntity pat,string audittrialfor = null)
        {
            List<CbfSumEntity> objDashBoard = new List<CbfSumEntity>();
            try
            {
               
                CbfSumEntity objModel;
                int liRaiserGid = 0;
                int liCnt = 0;
                getconnection();
                DataTable dt = new DataTable();
                DataTable dtr = new DataTable();
                cmdQry = new SqlCommand("pr_fb_trn_audittrail", objConn);
                cmdQry.Parameters.AddWithValue("@gid", pat.gid);
                cmdQry.Parameters.AddWithValue("@refflag", Convert.ToInt32(pat.ref_flag));
                cmdQry.CommandType = CommandType.StoredProcedure;
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (audittrialfor == "PAR")
                        {
                            objModel = new CbfSumEntity();
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

                            string actions = Convert.ToString(dt.Rows[i]["queue_action_for_name"].ToString());
                            string empid = Convert.ToString(dt.Rows[i]["queue_action_by"].ToString());
                            string queue_type = Convert.ToString(dt.Rows[i]["queue_to_type"].ToString());
                            string queue_to = Convert.ToString(dt.Rows[i]["queue_to"].ToString());

                            objModel = new CbfSumEntity();
                            objModel.queue_gid = Convert.ToInt32(dt.Rows[i]["queue_gid"].ToString());
                            if (actions == "Rejected")
                            {
                                objModel.action_status = actions;
                            }
                            else
                            {
                                objModel.action_status = Convert.ToString(dt.Rows[i]["queue_action_status_name"].ToString());
                            }
                            objModel.queue_to_type = Convert.ToString(dt.Rows[i]["queue_to_name"].ToString());
                            //if (dt.Rows[i]["Additional_flag"].ToString() == "N" && queue_type !="G")
                            //{
                            //    string empgid = Convert.ToString(GetEmployeeHierarchy(liRaiserGid, Convert.ToString(dt.Rows[i]["queue_to_type_name"].ToString()), Convert.ToInt32(dt.Rows[i]["queue_to"].ToString())));
                            //    string EmployeeDetails = Getempname(empgid); 
                            //    string[] data;
                            //    data = EmployeeDetails.Split(',');
                            //    objModel.employee_code = data[0].ToString() + "-" + data[1].ToString();
                            //}
                            //else
                            //{
                            //    objModel.employee_code = Convert.ToString(dt.Rows[i]["action_by_name"].ToString());
                            //}
                            if (dt.Rows[i]["Additional_flag"].ToString() == "N"
                                && queue_type != "G"
                                && queue_type != "R"
                                && queue_type != "I"
                                && queue_type != "E"
                                && objModel.action_status !="Approved"
                                && objModel.action_status !="Rejected")
                            {
                                string empgid = Convert.ToString(GetEmployeeHierarchy(liRaiserGid, Convert.ToString(dt.Rows[i]["queue_to_type_name"].ToString()), Convert.ToInt32(dt.Rows[i]["queue_to"].ToString())));
                                string EmployeeDetails = Getempname(empgid);
                                string[] data;
                                data = EmployeeDetails.Split(',');
                                objModel.employee_code = data[0].ToString() + "-" + data[1].ToString();
                            }
                            else
                            {
                                if (queue_type == "I" || queue_type == "E")
                                    objModel.employee_code = GetEmployeeName(Convert.ToInt32(dt.Rows[i]["queue_to"].ToString()));
                                else
                                    objModel.employee_code = Convert.ToString(dt.Rows[i]["action_by_name"].ToString());
                            }
                            objModel.action_date = Convert.ToString(dt.Rows[i]["queue_action_date"].ToString());
                            objModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());

                            objDashBoard.Add(objModel);
                        }
                        else
                        {
                            string pergid = Convert.ToString(dt.Rows[i]["queue_prev_gid"].ToString());
                            if (pergid == "0")
                            {
                                objModel = new CbfSumEntity();
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

                            objModel = new CbfSumEntity();
                            objModel.queue_gid = Convert.ToInt32(dt.Rows[i]["queue_gid"].ToString());
                            objModel.action_status = Convert.ToString(dt.Rows[i]["queue_action_status_name"].ToString());
                            objModel.queue_to_type = Convert.ToString(dt.Rows[i]["queue_to_name"].ToString());
                            //if (dt.Rows[i]["Additional_flag"].ToString() == "N" && queue_type !="G")
                            //{
                            //    string empgid = Convert.ToString(GetEmployeeHierarchy(liRaiserGid, Convert.ToString(dt.Rows[i]["queue_to_type_name"].ToString()), Convert.ToInt32(dt.Rows[i]["queue_to"].ToString())));
                            //    string EmployeeDetails = Getempname(empgid); 
                            //    string[] data;
                            //    data = EmployeeDetails.Split(',');
                            //    objModel.employee_code = data[0].ToString() + "-" + data[1].ToString();
                            //}
                            //else
                            //{
                            //    objModel.employee_code = Convert.ToString(dt.Rows[i]["action_by_name"].ToString());
                            //}
                            if (dt.Rows[i]["Additional_flag"].ToString() == "N"
                                && queue_type != "G"
                                && queue_type != "I"
                                && queue_type != "E"
                                && objModel.action_status !="Approved"
                                && objModel.action_status != "Rejected")
                            {
                                string empgid = Convert.ToString(GetEmployeeHierarchy(liRaiserGid, Convert.ToString(dt.Rows[i]["queue_to_type_name"].ToString()), Convert.ToInt32(dt.Rows[i]["queue_to"].ToString())));
                                string EmployeeDetails = Getempname(empgid);
                                string[] data;
                                data = EmployeeDetails.Split(',');
                                objModel.employee_code = data[0].ToString() + "-" + data[1].ToString();
                            }
                            else
                            {
                                if (queue_type == "I" || queue_type == "E")
                                    objModel.employee_code = GetEmployeeName(Convert.ToInt32(dt.Rows[i]["queue_to"].ToString()));
                                else
                                    objModel.employee_code = Convert.ToString(dt.Rows[i]["action_by_name"].ToString());
                            }
                            objModel.action_date = Convert.ToString(dt.Rows[i]["queue_action_date"].ToString());
                            objModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());

                            objDashBoard.Add(objModel);
                        }
                       

                    }
                    //if (objDashBoard.Count > 0)
                    //{
                    //    objDashBoard = objDashBoard.OrderByDescending(s => s.queue_gid).ToList();
                    //}
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
                objConn.Close();
            }
        }
        public string GetEmployeeHierarchy(int raisergid = 0, string titlename = null, int titlerefgid = 0)
        {
            try
            {
                string lsResult = string.Empty;
                getconnection();
                DataTable dt = new DataTable();
                cmdQry = new SqlCommand("pr_chk_employee_hierarchy", objConn);
                cmdQry.Parameters.AddWithValue("@employee_gid", raisergid);
                cmdQry.Parameters.AddWithValue("@title_name", titlename);
                cmdQry.Parameters.AddWithValue("@title_value_gid", titlerefgid);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@title_employee_gid", SqlDbType.Int, 5);
                cmdQry.Parameters["@title_employee_gid"].Direction = ParameterDirection.Output;
                cmdQry.ExecuteNonQuery();
                lsResult = Convert.ToString(cmdQry.Parameters["@title_employee_gid"].Value.ToString());

                return lsResult;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
        }
        public string GetEmployeeName(int empgid = 0)
        {
            string EmpName = "";
            try
            {
                getconnection();
                DataTable dt = new DataTable();
                cmdQry = new SqlCommand("pr_iem_LoginDetails", objConn);
                cmdQry.Parameters.AddWithValue("@empgid", empgid);
                cmdQry.Parameters.AddWithValue("@action", "employeenameforaudittrial");
                cmdQry.CommandType = CommandType.StoredProcedure;
                EmpName = Convert.ToString(cmdQry.ExecuteScalar());
                return EmpName;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
        }
        public List<GRNInward> PopulateAuditTrailGRN(GRNInward pat)
        {
            List<GRNInward> objDashBoard = new List<GRNInward>();
            try
            {
                
                GRNInward objModel;
                getconnection();
                DataTable dt = new DataTable();
                DataTable dtr = new DataTable();
                string streject = "";
                string strejectnew = "";
                string status = "";
                string str = "";

                cmdQry = new SqlCommand("pr_fb_trn_audittaril", objConn);
                cmdQry.Parameters.AddWithValue("@gid", pat.gid);
                cmdQry.Parameters.AddWithValue("@refflag", pat.ref_flag);
                cmdQry.CommandType = CommandType.StoredProcedure;
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string pergid = Convert.ToString(dt.Rows[i]["queue_prev_gid"].ToString());
                        if (pergid == "0")
                        {
                            streject = Convert.ToString(dt.Rows[i]["queue_from"].ToString());
                            string empcodnamer = Getempname(streject);
                            string[] datar;
                            datar = empcodnamer.Split(',');
                            objModel = new GRNInward();
                            objModel.employee_code = datar[0].ToString() + "-" + datar[1].ToString();
                            objModel.action_remarks = Convert.ToString(dt.Rows[i]["grninwrdheader_remarks"].ToString());
                            status = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
                            objModel.action_status = GetQueueStatusHistry(status);
                            objModel.action_status = "Submitted";
                            if (pat.ref_flag == "12")
                            {
                                objModel.action_date = Convert.ToString(dt.Rows[i]["grninwrdheader_grndatetime"].ToString());
                                objModel.queue_to_type = "GRN Raiser";
                            }
                            objDashBoard.Add(objModel);

                            string actions = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
                            if (actions != "0")
                            {

                                string empid = Convert.ToString(dt.Rows[i]["queue_action_by"].ToString());
                                if (objModel.action_status == "Submitted" && empid == "")
                                {
                                    empid = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
                                }
                                if (empid != "")
                                {
                                    string empcodname = Getempname(empid);
                                    string[] data;
                                    data = empcodname.Split(',');
                                    objModel.employee_code = data[0].ToString() + "-" + data[1].ToString();
                                }
                                else
                                {
                                    objModel.employee_code = "";

                                }
                                objModel = new GRNInward();

                                objModel.action_date = Convert.ToString(dt.Rows[i]["queue_action_date"].ToString());
                                objModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());
                                status = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
                                objModel.action_status = GetQueueStatusHistry(status);
                                if (pat.ref_flag == "12")
                                {
                                    string queue_type = Convert.ToString(dt.Rows[i]["queue_to_type"].ToString());
                                    string queue_to = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
                                    objModel.queue_to_type = GetQueueRole(queue_type, "12", queue_to);
                                }
                                objDashBoard.Add(objModel);

                            }
                        }
                        else
                        {
                            string empid = "";
                            string queue_type = Convert.ToString(dt.Rows[i]["queue_to_type"].ToString());
                            string queue_to = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
                            string actionstt = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
                            if (actionstt != "")
                            {
                                objModel = new GRNInward();
                                if (queue_type == "I" || queue_type == "E")
                                {
                                    empid = Convert.ToString(dt.Rows[i]["queue_to"].ToString());

                                }
                                else if (queue_type == "G" || queue_type == "R" || queue_type == "L" || queue_type == "D")
                                {
                                    empid = Convert.ToString(dt.Rows[i]["queue_action_by"].ToString());
                                }
                                if (empid != "")
                                {
                                    string empcodname = Getempname(empid);
                                    string[] data;
                                    data = empcodname.Split(',');
                                    objModel.employee_code = data[0].ToString() + "-" + data[1].ToString();
                                }
                                else
                                {
                                    objModel.employee_code = "";

                                }
                                objModel.action_date = Convert.ToString(dt.Rows[i]["queue_action_date"].ToString());
                                objModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());
                                status = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
                                objModel.action_status = GetQueueStatusHistry(status);
                                objModel.queue_to_type = GetQueueRole(queue_type, "12", queue_to);
                                objDashBoard.Add(objModel);

                            }

                        }
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
                objConn.Close();
            }
        }
        public string attach_parinline_cbf(CbfSumEntity parcat)
        {
            string result;
            try
            {
                parcat.Attachment_date = objCmnFunctions.convertoDateTimeString(parcat.Attachment_date).ToString();
                string[,] codes = new string[,]
	               {
                        {"attachment_ref_flag","2"},
                        {"attachment_ref_gid","22270" },                      
	                    {"attachment_attachmenttype_gid", parcat.Budgeted},                        
	                    {"attachment_by", objCmnFunctions.GetLoginUserGid().ToString()},
                        {"attachment_date",objCmnFunctions.convertoDateTimeString(parcat.Attachment_date).ToString()},
                        {"attachment_desc", parcat.des},
                        {"attachment_filename", parcat.attachment1},  
                         {"attachment_isremoved", "N"}                                           
                  };
                string tname = "iem_trn_tattachment";
                string insertcommend = objCommonIUD.InsertCommon(codes, tname);
                result = "success";

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
            return result;

        }
        //public string GetEmployeeName(int empgid = 0)
        //{
        //    string EmpName = "";
        //    try
        //    {
        //        getconnection();
        //        DataTable dt = new DataTable();
        //        cmdQry = new SqlCommand("pr_iem_LoginDetails", objConn);
        //        cmdQry.Parameters.AddWithValue("@empgid", empgid);
        //        cmdQry.Parameters.AddWithValue("@action", "employeenameforaudittrial");
        //        cmdQry.CommandType = CommandType.StoredProcedure;
        //        EmpName = Convert.ToString(cmdQry.ExecuteScalar());
        //        return EmpName;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        objConn.Close();
        //    }
        //}
        public string Getempname(string empgid)
        {
            string status = "";
            try
            {
                getconnection();
                DataTable dt = new DataTable();
                cmdQry = new SqlCommand("pr_fb_trn_audittaril", objConn);
                cmdQry.Parameters.AddWithValue("@gid", empgid);
                cmdQry.Parameters.AddWithValue("@refflag", "E");
                cmdQry.CommandType = CommandType.StoredProcedure;
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    string raisername = Convert.ToString(dt.Rows[0]["employee_code"].ToString());
                    string raisercode = Convert.ToString(dt.Rows[0]["employee_name"].ToString());
                    status = raisername + "," + raisercode;
                }
                return status;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
        }
        public string GetQueueStatusHistry(string Queue)
        {
            try
            {
                string Status = string.Empty;

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
                return Status;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
        }

        public string GetQueueRole(string type, string flag, string queue)
        {
            try
            {
                string queuetype = string.Empty;
                if (flag == "2")
                {
                    if (type.Contains("I"))
                    {
                        queuetype = "CBF Supervisor";
                    }
                    if (type.Contains("E"))
                    {
                        queuetype = "CBF Checker";
                    }
                }
                if (flag == "9")
                {
                    if (type.Contains("G"))
                    {
                        queuetype = "PAR Checker";
                    }

                }

                if (type.Contains("G"))
                {
                    getconnection();
                    DataTable dt = new DataTable();
                    cmdQry = new SqlCommand("pr_fb_trn_audittaril", objConn);
                    cmdQry.Parameters.AddWithValue("@gid", queue);
                    cmdQry.Parameters.AddWithValue("@refflag", "G");
                    cmdQry.CommandType = CommandType.StoredProcedure;
                    objDataAdapter = new SqlDataAdapter(cmdQry);
                    objDataAdapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        queuetype = Convert.ToString(dt.Rows[0]["rolegroup_name"].ToString());
                    }
                }
                if (type.Contains("R"))
                {
                    getconnection();
                    DataTable dt = new DataTable();
                    cmdQry = new SqlCommand("pr_fb_trn_audittaril", objConn);
                    cmdQry.Parameters.AddWithValue("@gid", queue);
                    cmdQry.Parameters.AddWithValue("@refflag", "R");
                    cmdQry.CommandType = CommandType.StoredProcedure;
                    objDataAdapter = new SqlDataAdapter(cmdQry);
                    objDataAdapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        queuetype = Convert.ToString(dt.Rows[0]["role_name"].ToString());
                    }
                }
                if (type.Contains("L"))
                {
                    getconnection();
                    DataTable dt = new DataTable();
                    cmdQry = new SqlCommand("pr_fb_trn_audittaril", objConn);
                    cmdQry.Parameters.AddWithValue("@gid", queue);
                    cmdQry.Parameters.AddWithValue("@refflag", "L");
                    cmdQry.CommandType = CommandType.StoredProcedure;
                    objDataAdapter = new SqlDataAdapter(cmdQry);
                    objDataAdapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        queuetype = Convert.ToString(dt.Rows[0]["grade_name"].ToString());
                    }
                }
                if (type.Contains("D"))
                {
                    getconnection();
                    DataTable dt = new DataTable();
                    cmdQry = new SqlCommand("pr_fb_trn_audittaril", objConn);
                    cmdQry.Parameters.AddWithValue("@gid", queue);
                    cmdQry.Parameters.AddWithValue("@refflag", "D");
                    cmdQry.CommandType = CommandType.StoredProcedure;
                    objDataAdapter = new SqlDataAdapter(cmdQry);
                    objDataAdapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        queuetype = Convert.ToString(dt.Rows[0]["designation_name"].ToString());
                    }
                }
                if (type.Contains("I"))
                {
                    getconnection();
                    DataTable dt = new DataTable();
                    cmdQry = new SqlCommand("pr_fb_trn_audittaril", objConn);
                    cmdQry.Parameters.AddWithValue("@gid", queue);
                    cmdQry.Parameters.AddWithValue("@refflag", "I");
                    cmdQry.CommandType = CommandType.StoredProcedure;
                    objDataAdapter = new SqlDataAdapter(cmdQry);
                    objDataAdapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        queuetype = Convert.ToString(dt.Rows[0]["designation_name"].ToString());
                    }
                }

                return queuetype;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
        }

        public int getAttachBy(CbfSumEntity att)
        {
            try
            {
                DataTable dt = new DataTable();
                int attachBy = 0;
                cmdQry = new SqlCommand("pr_fb_trn_audittaril", objConn);
                cmdQry.Parameters.AddWithValue("@gid", att.gid);
                cmdQry.Parameters.AddWithValue("@refflag", "attachmentby");
                cmdQry.CommandType = CommandType.StoredProcedure;
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    attachBy = Convert.ToInt32(dt.Rows[0]["attachment_by"].ToString());
                }
                return attachBy;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                objConn.Close();
            }
           
        }


        public int Getbranchgid()
        {
            try
            {
                int empbrgid = 0;
                getconnection();
                cmdQry = new SqlCommand("pr_fb_getbranchgid", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@empid", objCmnFunctions.GetLoginUserGid().ToString());
                empbrgid = (int)cmdQry.ExecuteScalar();
                return empbrgid;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                objConn.Close();
            }
          
        }

        #region "DelmateFlow"

        public string GetDelmatemployee(string cbfgid, string status, quenue objque, string remarks)
        {
            int isparapprover = 0;
            int parapprover = 0;
        Exitlabel:
            string resultone = string.Empty;
            string queue_toG = string.Empty;
            string queue_toD = string.Empty;
            int queue_toR = 0;
            string queue_totype = string.Empty;
            string queue_tobranch = string.Empty;
            string queue_todept = string.Empty;
            string delmattype = string.Empty;
            decimal delamount;
            string delmatgid = string.Empty;
            string Expenses = string.Empty;
            int branchgid = 0;
            string queuengid = string.Empty;
            string queuento = string.Empty;
            decimal pramount;
            int cbfdelmat_result = 0;

            try
            {
                int cbfextraflag = 0;
                if (parapprover != 0)
                {
                    cbfextraflag = 1;
                }
                getconnection();
                cmdQry = new SqlCommand("pr_fb_GetCbfqueueapp", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@cbfgid", cbfgid);
                cmdQry.Parameters.AddWithValue("@cbfextraflag", cbfextraflag);
                objDataAdapter = new SqlDataAdapter(cmdQry);
                DataTable dtcmd = new DataTable();
                objDataAdapter.Fill(dtcmd);
                if (dtcmd.Rows.Count > 0)
                {
                    queuengid = dtcmd.Rows[0]["queuegid"].ToString();
                    //queuengid = "255";
                    //queuento = dtcmd.Rows[0]["queueto"].ToString();.
                    if (parapprover != 0)
                    {
                        queuento = Convert.ToString(parapprover);
                    }
                    else
                    {
                        queuento = objCmnFunctions.GetLoginUserGid().ToString();
                    }
                                       
                    cmdQry = new SqlCommand("pr_cbfdelmat", objConn);
                    cmdQry.CommandType = CommandType.StoredProcedure;
                    cmdQry.Parameters.AddWithValue("@cbfheader_gid", cbfgid);
                    cmdQry.Parameters.AddWithValue("@queue_gid", queuengid);
                    cmdQry.Parameters.AddWithValue("@cbf_approver_gid", queuento);

                    cmdQry.Parameters.Add("@cbfdelmat_result", SqlDbType.Int, 32);
                    cmdQry.Parameters["@cbfdelmat_result"].Direction = ParameterDirection.Output;

                    cmdQry.Parameters.Add("@cbf_final_flag", SqlDbType.Char, 1);
                    cmdQry.Parameters["@cbf_final_flag"].Direction = ParameterDirection.Output;

                    cmdQry.Parameters.Add("@cbf_next_delmatmatrix_gid", SqlDbType.Int, 64);
                    cmdQry.Parameters["@cbf_next_delmatmatrix_gid"].Direction = ParameterDirection.Output;

                    cmdQry.Parameters.Add("@cbf_next_queue_to_type", SqlDbType.Char, 1);
                    cmdQry.Parameters["@cbf_next_queue_to_type"].Direction = ParameterDirection.Output;

                    cmdQry.Parameters.Add("@cbf_next_queue_to_gid", SqlDbType.Int, 64);
                    cmdQry.Parameters["@cbf_next_queue_to_gid"].Direction = ParameterDirection.Output;

                    cmdQry.Parameters.Add("@cbf_err_output", SqlDbType.VarChar, 10000);
                    cmdQry.Parameters["@cbf_err_output"].Direction = ParameterDirection.Output;

                    cmdQry.Parameters.Add("@cbf_sql_output", SqlDbType.VarChar, 10000);
                    cmdQry.Parameters["@cbf_sql_output"].Direction = ParameterDirection.Output;

                    cmdQry.Parameters.Add("@cbf_next_queue_to_additional_flag", SqlDbType.VarChar, 10000);
                    cmdQry.Parameters["@cbf_next_queue_to_additional_flag"].Direction = ParameterDirection.Output;

                    cmdQry.ExecuteNonQuery();

                    string finalfalg = Convert.ToString(cmdQry.Parameters["@cbf_final_flag"].Value);
                    string nqueuety = Convert.ToString(cmdQry.Parameters["@cbf_next_queue_to_type"].Value);
                    string nqueuet = Convert.ToString(cmdQry.Parameters["@cbf_next_queue_to_gid"].Value);
                    string ndelgid = Convert.ToString(cmdQry.Parameters["@cbf_next_delmatmatrix_gid"].Value);
                    string cbf_erroutput = Convert.ToString(cmdQry.Parameters["@cbf_err_output"].Value);
                    string cbf_sql_output = Convert.ToString(cmdQry.Parameters["@cbf_sql_output"].Value);
                    string additional_flag = Convert.ToString(cmdQry.Parameters["@cbf_next_queue_to_additional_flag"].Value);
                    cbfdelmat_result = Convert.ToInt32(cmdQry.Parameters["@cbfdelmat_result"].Value);

                    if (cbfdelmat_result > 0)
                    {
                        if (status == "supervisor")
                        {
                            QueueProcess(objque);
                            string[,] codesIN = new string[,]
                                      {
                                           {"queue_date","sysdatetime()"},
                                           {"queue_ref_flag", "2"},
                                           {"queue_ref_gid",cbfgid },
                                           {"queue_ref_status", "0"},
                                           {"queue_from",objCmnFunctions.GetLoginUserGid().ToString() } ,
                                           {"queue_to_type", nqueuety.ToString()},
                                           {"queue_to", nqueuet.ToString()},
                                           {"queue_action_for", "A"},
                                           {"queue_prev_gid",Convert.ToString(GetQueueGid(cbfgid))},
                                           {"queue_delmat_flag","D"},
                                           {"Additional_flag",additional_flag.ToString()}
                                     };
                            string tnameIN = "iem_trn_tqueue";

                            string insertcommendecf = objCommonIUD.InsertCommon(codesIN, tnameIN);
                            resultone = insertcommendecf;
                        }
                        //string[,] column = new string[,]
                        //             {
                        //                 {"queue_isremoved","Y" }
                        //             };
                        //string[,] wherecol = new string[,]
                        //            {
                        //                {"queue_ref_gid",cbfgid},
                        //                {"queue_ref_flag","2"},
                        //                {"queue_isremoved","N" },
                        //            };
                        //string tblname = "iem_trn_tqueue";
                        //string updatecom = objCommonIUD.UpdateCommon(column, wherecol, tblname);
                        if (finalfalg == "N")
                        {
                                                      
                            if (status == "delmat")
                            {
                               
                               
                                if (parapprover !=0)
                                {
                                    string[,] codesq1 = new string[,]
                                            {
                                                 {"queue_action_date","sysdatetime()"},
                                                 {"queue_action_by", Convert.ToString(parapprover)},
                                                 {"queue_action_status","1" },
                                                 {"queue_action_remark","PAR APPROVER"},
                                                 {"queue_isremoved","Y" }
                                             };
                                    string[,] whreq1 = new string[,]
                                            {
                                     
                                                {"queue_ref_gid",cbfgid},
                                                {"queue_ref_flag","2"},
                                                {"queue_isremoved","N" },
                                                {"queue_delmat_flag","D"}
                                           };

                                    string tnameq1 = "iem_trn_tqueue";
                                    string insertcommendq1 = objCommonIUD.UpdateCommon(codesq1, whreq1, tnameq1);
                                    string[,] codesIN = new string[,]
                                      {
                                           {"queue_date","sysdatetime()"},
                                           {"queue_ref_flag", "2"},
                                           {"queue_ref_gid",cbfgid },
                                           {"queue_ref_status", "0"},
                                           {"queue_from",Convert.ToString(parapprover)},
                                           {"queue_to_type", nqueuety.ToString()},
                                           {"queue_to", nqueuet.ToString()},
                                           {"queue_action_for", "A"},
                                           {"queue_prev_gid",Convert.ToString(GetQueueGid(cbfgid))},
                                           {"queue_delmat_flag","D"},
                                           {"Additional_flag",additional_flag.ToString()}
                                     };
                                    string tnameIN = "iem_trn_tqueue";

                                    string insertcommendecf = objCommonIUD.InsertCommon(codesIN, tnameIN);
                                    resultone = insertcommendecf;
                                   
                                    getconnection();
                                    cmdQry = new SqlCommand("pr_fb_trn_ChkIsFinalApprover", objConn);
                                    cmdQry.CommandType = CommandType.StoredProcedure;
                                    cmdQry.Parameters.AddWithValue("@refgid", cbfgid);
                                    cmdQry.Parameters.AddWithValue("@queuetotype", Convert.ToString(nqueuety));
                                    cmdQry.Parameters.AddWithValue("@queueto", Convert.ToInt32(nqueuet));
                                    cmdQry.Parameters.AddWithValue("@action", "isparapprover");
                                    parapprover = Convert.ToInt32(cmdQry.ExecuteScalar());
                                    if (parapprover != 0)
                                    {
                                        //string[,] codesq = new string[,]
                                        //    {
                                        //         {"queue_action_date","sysdatetime()"},
                                        //         {"queue_action_by", Convert.ToString(parapprover)},
                                        //         {"queue_action_status","1" },
                                        //         {"queue_action_remark","PAR APPROVER"},
                                        //         {"queue_isremoved","Y" }
                                        //     };
                                        //string[,] whreq = new string[,]
                                        //    {
                                     
                                        //        {"queue_ref_gid",cbfgid},
                                        //        {"queue_ref_flag","2"},
                                        //        {"queue_isremoved","N" },
                                        //        {"queue_delmat_flag","D"}
                                        //   };

                                        //string tnameq = "iem_trn_tqueue";
                                        //string insertcommendq = objCommonIUD.UpdateCommon(codesq, whreq, tnameq);

                                        goto Exitlabel;
                                    }
                                }
                                else
                                {
                                    string[,] codesq = new string[,]
                                    {
                                         {"queue_action_date","sysdatetime()"},
                                         {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString() },
                                         {"queue_action_status","1" },
                                         {"queue_action_remark",(string.IsNullOrEmpty(remarks.ToString()) ? string.Empty :remarks.ToString())},
                                         {"queue_isremoved","Y" }
                                     };
                                    string[,] whreq = new string[,]
                                    {
                                     
                                        {"queue_ref_gid",cbfgid},
                                        {"queue_ref_flag","2"},
                                         {"queue_isremoved","N" },
                                         {"queue_delmat_flag","D"}
                                   };
                                    string tnameq = "iem_trn_tqueue";
                                    string insertcommendq = objCommonIUD.UpdateCommon(codesq, whreq, tnameq);

                                    string[,] codesIN = new string[,]
                                      {
                                           {"queue_date","sysdatetime()"},
                                           {"queue_ref_flag", "2"},
                                           {"queue_ref_gid",cbfgid },
                                           {"queue_ref_status", "0"},
                                           {"queue_from",objCmnFunctions.GetLoginUserGid().ToString() } ,
                                           {"queue_to_type", nqueuety.ToString()},
                                           {"queue_to", nqueuet.ToString()},
                                           {"queue_action_for", "A"},
                                           {"queue_prev_gid",Convert.ToString(GetQueueGid(cbfgid))},
                                           {"queue_delmat_flag","D"},
                                           {"Additional_flag",additional_flag.ToString()}
                                     };
                                    string tnameIN = "iem_trn_tqueue";

                                    string insertcommendecf = objCommonIUD.InsertCommon(codesIN, tnameIN);
                                    resultone = insertcommendecf;

                                    getconnection();
                                    cmdQry = new SqlCommand("pr_fb_trn_ChkIsFinalApprover", objConn);
                                    cmdQry.CommandType = CommandType.StoredProcedure;
                                    cmdQry.Parameters.AddWithValue("@refgid", cbfgid);
                                    cmdQry.Parameters.AddWithValue("@queuetotype", Convert.ToString(nqueuety));
                                    cmdQry.Parameters.AddWithValue("@queueto", Convert.ToInt32(nqueuet));
                                    cmdQry.Parameters.AddWithValue("@action", "isparapprover");
                                    parapprover = Convert.ToInt32(cmdQry.ExecuteScalar());
                                    if (parapprover != 0)
                                    {
                                       
                                        goto Exitlabel;
                                    }
                                }
                                
                               
                            }
                            


                        }

                        else
                        {
                            string[,] codes1 = new string[,]            
                       {
                           {"cbfheader_status","5"},

                       };
                            string[,] whrcol = new string[,]
                        {
                          
                             {"cbfheader_gid",cbfgid.ToString()}

                        };
                            string tname1 = "fb_trn_tcbfheader";
                            string updatequery = objCommonIUD.UpdateCommon(codes1, whrcol, tname1);
                            string[,] codesq = new string[,]
                                    {
                                         {"queue_action_date","sysdatetime()"},
                                         {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString() },
                                         {"queue_action_status","1" },
                                         {"queue_action_remark",(string.IsNullOrEmpty(remarks.ToString()) ? string.Empty :remarks.ToString())},
                                         {"queue_isremoved","Y"},
                                         {"queue_delmat_flag","F"}
                                     };
                            string[,] whreq = new string[,]
                                    {
                                     
                                         {"queue_ref_gid",cbfgid.ToString()},
                                         {"queue_isremoved","N" },
                                         {"queue_delmat_flag","D"},
                                         {"queue_ref_flag","2"}
                                   };
                            string tnameq = "iem_trn_tqueue";
                            string insertcommendq = objCommonIUD.UpdateCommon(codesq, whreq, tnameq);
                            resultone = insertcommendq;
                        }
                    }
                    else
                    {
                        resultone = cbf_erroutput;
                    }

                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            finally
            {
                objConn.Close();
            }
            return resultone;
        }
        public int GetQueueGid(string cbfheadgid)
        {
            try
            {
                int prrvgid = 0;
                getconnection();
                cmdQry = new SqlCommand("pr_fb_GetCbfqueuegid", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@cbfgid", cbfheadgid.ToString());
                prrvgid = (int)cmdQry.ExecuteScalar();
                return prrvgid;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                objConn.Close();
            }
         
        }
        #endregion

        public List<Attachment> Getattachment(string id, string refe, string type)
        {
            List<Attachment> obj_ = new List<Attachment>();
            CbfSumEntity ob = new CbfSumEntity();
            try
            {
                getconnection();
                DataTable dt = new DataTable();
                Attachment newob = new Attachment();
                cmdQry = new SqlCommand("attachment_get", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@gid", id);
                cmdQry.Parameters.AddWithValue("@ref", refe);
                cmdQry.Parameters.AddWithValue("@type", type);
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);
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
                ob.attachment = obj_;
                return ob.attachment;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                ob.attachment = obj_;
                return ob.attachment;
            }
            finally
            {
                objConn.Close();
            }

        }
        public List<Attachment> Getattachment_val(CbfSumEntity dateval)
        {
            CbfSumEntity ob = new CbfSumEntity();
            DataTable dt = new DataTable();
            List<Attachment> obj_ = new List<Attachment>();
            try
            {
                getconnection();
             
                Attachment newob = new Attachment();
                cmdQry = new SqlCommand("attachment_get_all", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@gid", dateval.cbfGid);
                cmdQry.Parameters.AddWithValue("@ref", dateval.cbfref);
                cmdQry.Parameters.AddWithValue("@type", dateval.boqfile);
                cmdQry.Parameters.AddWithValue("@attachment_date", dateval.attachmentDate);
                cmdQry.Parameters.AddWithValue("@attachment_by", objCmnFunctions.GetLoginUserGid().ToString());
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);
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
                ob.attachment = obj_;
                return ob.attachment;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                ob.attachment = obj_;
                return ob.attachment;
            }
            finally
            {
                objConn.Close();
            }

        }
        public DataSet getdelmatfinalapprover(int delmattype, string delmatname, decimal tamount)
        {
            int approverid = 0;
            string result = string.Empty;
            DataSet dspp = new DataSet();
            try
            {
                getconnection();
                // cmdQry = new SqlCommand("pr_fb_getdelmatfinalid", objConn);pr_fb_getdelmatffid
                cmdQry = new SqlCommand("pr_fb_getdelmatffid", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@delmattypegid", delmattype);
                cmdQry.Parameters.AddWithValue("@delmatname", delmatname);
                cmdQry.Parameters.AddWithValue("@totalparamount", tamount);
                SqlDataAdapter da = new SqlDataAdapter(cmdQry);

                da.Fill(dspp);
                //result = Convert.ToString(cmdQry.ExecuteScalar());
                //approverid = Convert.ToInt32(string.IsNullOrEmpty(result) ? "0" : result);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                objConn.Close();
            }
            return dspp;
        }
        public List<shipment> getbranchdetails()
        {
            List<shipment> objlist = new List<shipment>();
            try
            {
                getconnection();
                DataTable objtable = new DataTable();
                shipment objship;
                cmdQry = new SqlCommand("pr_fb_trn_cbfselection", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@actionName", "selectbranch");
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(objtable);
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objship = new shipment();
                    objship.SrNo = i + 1;
                    objship.branchgid = Convert.ToInt32(objtable.Rows[i]["branch_gid"]);
                    objship.branchName = objtable.Rows[i]["branch_name"].ToString();
                    objship.address = objtable.Rows[i]["branchaddress"].ToString();
                    objship.location = objtable.Rows[i]["branch_location_name"].ToString();
                    objship.branchCode = objtable.Rows[i]["branch_code"].ToString();
                    objship.inchargeID = objtable.Rows[i]["branch_incharge"].ToString();
                    objlist.Add(objship);
                }
                return objlist;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objlist;
            }
            finally
            {
                objConn.Close();
            }
        }
        public string attach_parinline(ParDetails parcat)
        {
            try
            {
                string result;
                parcat.Attachment_date = objCmnFunctions.convertoDateTimeString(parcat.Attachment_date).ToString();
                string[,] codes = new string[,]
	               {
                        {"attachment_ref_flag","12"},
                        {"attachment_ref_gid","932600" },                      
	                    {"attachment_attachmenttype_gid", parcat.Budgeted},                        
	                    {"attachment_by", objCmnFunctions.GetLoginUserGid().ToString()},
                        {"attachment_date",objCmnFunctions.convertoDateTimeString(parcat.Attachment_date).ToString()},
                        {"attachment_desc", parcat.Description},
                        {"attachment_filename", parcat.Attachment},  
                         {"attachment_isremoved", "N"}                                           
                  };
                string tname = "iem_trn_tattachment";
                string insertcommend = objCommonIUD.InsertCommon(codes, tname);
                result = "success";
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
           

        }
        public string attach_parinlineEdit(ParDetails parcat)
        {
            try
            {
                string result;
                parcat.Attachment_date = objCmnFunctions.convertoDateTimeString(parcat.Attachment_date).ToString();
                string[,] codes = new string[,]
	               {
                        {"attachment_ref_flag","12"},
                        {"attachment_ref_gid", parcat.att_identify},                      
	                    {"attachment_attachmenttype_gid", "2"},                        
	                    {"attachment_by", objCmnFunctions.GetLoginUserGid().ToString()},
                        {"attachment_date",objCmnFunctions.convertoDateTimeString(parcat.Attachment_date).ToString()},
                        {"attachment_desc", parcat.Description},
                        {"attachment_filename", parcat.Attachment},  
                         {"attachment_isremoved", "N"}                                           
                  };
                string tname = "iem_trn_tattachment";
                string insertcommend = objCommonIUD.InsertCommon(codes, tname);
                result = "success";
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
        }
        public string attach_cbfinlineEdit(CbfSumEntity parcat)
        {
            try
            {
                string result;
                parcat.Attachment_date = objCmnFunctions.convertoDateTimeString(parcat.Attachment_date).ToString();
                string[,] codes = new string[,]
	               {
                        {"attachment_ref_flag","2"},
                        {"attachment_ref_gid", parcat.Budgeted},                      
	                    {"attachment_attachmenttype_gid", "2"},                        
	                    {"attachment_by", objCmnFunctions.GetLoginUserGid().ToString()},
                        {"attachment_date",objCmnFunctions.convertoDateTimeString(parcat.Attachment_date).ToString()},
                        {"attachment_desc", parcat.des},
                        {"attachment_filename", parcat.attachment1},  
                         {"attachment_isremoved", "N"}                                           
                  };
                string tname = "iem_trn_tattachment";
                string insertcommend = objCommonIUD.InsertCommon(codes, tname);
                result = "success";
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
        }

        public string Deleteattach(int attachId)
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
                string tblname = "iem_trn_tattachment";


                string deletetbl = delecomm.DeleteCommon(codes, whrcol, tblname);
                result = "success";

                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }

        }

        public DataTable dtgetemployeeilist(int pargid)
        {
            DataTable dt = new DataTable();
            try
            {
                getconnection();
                cmdQry = new SqlCommand("pr_fb_getparapproverids", objConn);
                cmdQry.Parameters.AddWithValue("@pargid", pargid);
                cmdQry.CommandType = CommandType.StoredProcedure;
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);
                return dt;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return dt;
            }
            finally
            {
                objConn.Close();
            }
           
        }

        public void deletetempstoredattach(string att)
        {
            try
            {
                getconnection();
                cmdQry = new SqlCommand("attachment_get_all", objConn);

                cmdQry.Parameters.AddWithValue("@gid", att);

                cmdQry.Parameters.AddWithValue("@attachfor", "deleteifexist");

                cmdQry.CommandType = CommandType.StoredProcedure;

                cmdQry.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                objConn.Close();
            }
           

        }
        public List<shipment> getbranchForLoginUser()
        {
            List<shipment> objlist = new List<shipment>();
            try
            {
                getconnection();
                DataTable objtable = new DataTable();
             
                shipment objship;
                cmdQry = new SqlCommand("pr_fb_trn_cbfselection", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@actionName", "selectbranchforloginuser");
                cmdQry.Parameters.AddWithValue("@empgid", Convert.ToInt32(objCmnFunctions.GetLoginUserGid().ToString()));
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(objtable);
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objship = new shipment();
                    objship.SrNo = i + 1;
                    objship.branchgid = Convert.ToInt32(objtable.Rows[i]["branch_gid"]);
                    objship.branchName = objtable.Rows[i]["branch_name"].ToString();
                    objship.address = objtable.Rows[i]["branchaddress"].ToString();
                    objship.location = objtable.Rows[i]["branch_location_name"].ToString();
                    objship.branchCode = objtable.Rows[i]["branch_code"].ToString();
                    objship.inchargeID = objtable.Rows[i]["branch_incharge"].ToString();
                    objlist.Add(objship);
                }
                return objlist;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objlist; 
            }
            finally
            {
                objConn.Close();
            }
        }


        public IEnumerable<partransfer> getpartransfersummary()
        {
            List<partransfer> partfr = new List<partransfer>();
            try
            {
                getconnection();
             
                partransfer tranfr;
                DataTable dt = new DataTable();
                cmdQry = new SqlCommand("pr_fb_trn_tpartransfer", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@action", "parheadertransferselect");
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tranfr = new partransfer();
                    tranfr.parheadergid = Convert.ToInt32(dt.Rows[i]["parheader_gid"].ToString());
                    tranfr.parheaderrefno = dt.Rows[i]["parheader_refno"].ToString();
                    tranfr.parheaderdate = dt.Rows[i]["parheader_date"].ToString();
                    tranfr.parheaderamount = dt.Rows[i]["pardetails_amt"].ToString();
                    tranfr.parutilizeramount = dt.Rows[i]["cbf_utilizedamount"].ToString();
                    tranfr.parbalencedamount = dt.Rows[i]["cbf_balancedamount"].ToString();
                    tranfr.parheaderdescription = dt.Rows[i]["parheader_desc"].ToString();
                    partfr.Add(tranfr);
                }
                return partfr;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return partfr;
            }
            finally
            {
                objConn.Close();
            }

        }

        public IEnumerable<pardetailtransfer> getpardetailamountsummary(string pardetails_gid)
        {
            List<pardetailtransfer> partfr = new List<pardetailtransfer>();
            try
            {
                getconnection();
            
                pardetailtransfer tranfr;
                DataTable dt = new DataTable();
                cmdQry = new SqlCommand("pr_fb_trn_tpartransfer", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@action", "pardetailstransferselect");
                cmdQry.Parameters.AddWithValue("@pardetails_gid", pardetails_gid);
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);
                HttpContext.Current.Session["dttransferamount"] = dt;
                int toatal = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tranfr = new pardetailtransfer();
                    tranfr.pardetails_parhead_gid = Convert.ToInt32(dt.Rows[i]["parheader_gid"].ToString());
                    tranfr.pardetailsgid = Convert.ToInt32(dt.Rows[i]["pardetails_gid"].ToString());
                    tranfr.pardetailsexpensetype = dt.Rows[i]["pardetails_expensetype"].ToString();
                    tranfr.pardetails_yearfrom = dt.Rows[i]["YearFrom"].ToString();
                    tranfr.pardetails_yearto = dt.Rows[i]["YearTo"].ToString();
                    tranfr.pardetailsrequestfor = dt.Rows[i]["requestfor_name"].ToString();
                    tranfr.pardetails_desc = dt.Rows[i]["pardetails_desc"].ToString();
                    tranfr.pardetailstransferinamount = 0;
                    tranfr.pardetailslineamount = Convert.ToDecimal(dt.Rows[i]["pardetails_amt"].ToString());
                    tranfr.pardetailsutilizedamount = Convert.ToDecimal(dt.Rows[i]["Untilized"].ToString());
                    tranfr.pardetailstransferoutamount = 0;
                    tranfr.pardetailsbalencedamount = Convert.ToDecimal(dt.Rows[i]["balanced"].ToString());
                    tranfr.pardetailstransferamount = 0;
                    tranfr.totallineamount += tranfr.pardetailslineamount;
                    tranfr.totalutilizedamount += tranfr.pardetailsutilizedamount;
                    tranfr.toatalbalencedamount += tranfr.toatalbalencedamount;
                    //tranfr.pardetails_remarks = dt.Rows[i]["pardetails_remarks"].ToString();

                    //tranfr.parheaderenddate = dt.Rows[i]["parheader_enddate"].ToString();
                    //tranfr.parheaderperiod = dt.Rows[i]["parheader_period"].ToString();
                    //tranfr.parheaderdescription = dt.Rows[i]["parheader_desc"].ToString();
                    partfr.Add(tranfr);
                }
                return partfr;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return partfr;
            }
            finally
            {
                objConn.Close();
            }
        }
        public IEnumerable<pardetailtransfer> getpardetailtransfersummary_report()
        {
            List<pardetailtransfer> partfr = new List<pardetailtransfer>();
            try
            {
                getconnection();

                pardetailtransfer tranfr;
                DataTable dt = new DataTable();
                cmdQry = new SqlCommand("pr_fb_trn_tpartransfer", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@action", "parheaderdetailstransferselect_report");             
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);                
                
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tranfr = new pardetailtransfer();
                    tranfr.parheader_refno = Convert.ToString(dt.Rows[i]["parheader_refno"].ToString());
                    tranfr.pardetailsgid = Convert.ToInt32(dt.Rows[i]["parheader_gid"].ToString());                   
                    tranfr.pardetailsrequestfor = dt.Rows[i]["requestfor_name"].ToString();
                    tranfr.pardetails_desc = dt.Rows[i]["pardetails_desc"].ToString();                   
                    tranfr.pardetailslineamount = Convert.ToDecimal(dt.Rows[i]["pardetails_amt"].ToString());
                    tranfr.pardetailsutilizedamount = Convert.ToDecimal(dt.Rows[i]["Untilized"].ToString());                   
                    tranfr.pardetailsbalencedamount = Convert.ToDecimal(dt.Rows[i]["balanced"].ToString()); 
                    tranfr.pardetailstransferinamount = Convert.ToDecimal(dt.Rows[i]["TransferIn"].ToString());
                    tranfr.pardetailstransferoutamount = Convert.ToDecimal(dt.Rows[i]["TransferOut"].ToString());
                    
                    partfr.Add(tranfr);
                }
                return partfr;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return partfr;
            }
            finally
            {
                objConn.Close();
            }
        }
        public IEnumerable<pardetailtransfer> getpardetailtransfersummary(string parheadid)
        {
            List<pardetailtransfer> partfr = new List<pardetailtransfer>();
            try
            {
                getconnection();
              
                pardetailtransfer tranfr;
                DataTable dt = new DataTable();
                cmdQry = new SqlCommand("pr_fb_trn_tpartransfer", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.AddWithValue("@action", "parheaderdetailstransferselect");
                cmdQry.Parameters.AddWithValue("@parheadergid", parheadid);
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);
                HttpContext.Current.Session["dttransferamount"] = dt;
                int toatal = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tranfr = new pardetailtransfer();
                    tranfr.pardetails_parhead_gid = Convert.ToInt32(dt.Rows[i]["parheader_gid"].ToString());
                    tranfr.pardetailsgid = Convert.ToInt32(dt.Rows[i]["pardetails_gid"].ToString());
                    tranfr.pardetailsexpensetype = dt.Rows[i]["pardetails_expensetype"].ToString();
                    tranfr.pardetails_yearfrom = dt.Rows[i]["YearFrom"].ToString();
                    tranfr.pardetails_yearto = dt.Rows[i]["YearTo"].ToString();
                    tranfr.pardetailsrequestfor = dt.Rows[i]["requestfor_name"].ToString();
                    tranfr.pardetails_desc = dt.Rows[i]["pardetails_desc"].ToString();
                    tranfr.pardetailstransferinamount = 0;
                    tranfr.pardetailslineamount = Convert.ToDecimal(dt.Rows[i]["pardetails_amt"].ToString());
                    tranfr.pardetailsutilizedamount = Convert.ToDecimal(dt.Rows[i]["Untilized"].ToString());
                    tranfr.pardetailstransferoutamount = 0;
                    tranfr.pardetailsbalencedamount = Convert.ToDecimal(dt.Rows[i]["balanced"].ToString());
                    tranfr.pardetailstransferamount = 0;
                    tranfr.totallineamount += tranfr.pardetailslineamount;
                    tranfr.totalutilizedamount += tranfr.pardetailsutilizedamount;
                    tranfr.toatalbalencedamount += tranfr.toatalbalencedamount;
                    //tranfr.pardetails_remarks = dt.Rows[i]["pardetails_remarks"].ToString();

                    //tranfr.parheaderenddate = dt.Rows[i]["parheader_enddate"].ToString();
                    //tranfr.parheaderperiod = dt.Rows[i]["parheader_period"].ToString();
                    //tranfr.parheaderdescription = dt.Rows[i]["parheader_desc"].ToString();
                    DataTable dttrans = new DataTable();
                    cmdQry = new SqlCommand("pr_fb_trn_tpartransfer", objConn);
                    cmdQry.CommandType = CommandType.StoredProcedure;
                    cmdQry.Parameters.AddWithValue("@action", "pargettranse");
                    cmdQry.Parameters.AddWithValue("@pardetails_gid", Convert.ToInt32(dt.Rows[i]["pardetails_gid"].ToString()));
                    objDataAdapter = new SqlDataAdapter(cmdQry);
                    objDataAdapter.Fill(dttrans);
                    if (dttrans.Rows.Count > 0)
                    {
                        tranfr.pardetailstransferinamount = Convert.ToDecimal(dttrans.Rows[0]["pardetails_transferinamount"].ToString());
                        tranfr.pardetailstransferoutamount = Convert.ToDecimal(dttrans.Rows[0]["pardetails_transferoutamount"].ToString());
                    }
                    partfr.Add(tranfr);
                }
                return partfr;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return partfr;
            }
            finally
            {
                objConn.Close();
            }
        }

        public IEnumerable<pardetailtransfer> getpardetailswithamount(DataTable dt, string viewfor)
        {
            List<pardetailtransfer> partfr = new List<pardetailtransfer>();
            try
            {
                int res;
               
                pardetailtransfer tranfr;
                getconnection();
                if (viewfor == "save")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        tranfr = new pardetailtransfer();
                        tranfr.pardetailsgid = Convert.ToInt32(dt.Rows[i]["pardetails_gid"].ToString());
                        tranfr.pardetailsexpensetype = dt.Rows[i]["pardetails_expensetype"].ToString();
                        tranfr.pardetails_yearfrom = dt.Rows[i]["YearFrom"].ToString();
                        tranfr.pardetails_yearto = dt.Rows[i]["YearTo"].ToString();
                        tranfr.pardetailsrequestfor = dt.Rows[i]["requestfor_name"].ToString();
                        tranfr.pardetails_desc = dt.Rows[i]["pardetails_desc"].ToString();

                        if (string.IsNullOrEmpty(dt.Rows[i]["TransferIn"].ToString()))
                        {
                            tranfr.pardetailstransferinamount = 0;
                        }
                        else
                        {
                            tranfr.pardetailstransferinamount = Convert.ToDecimal(dt.Rows[i]["TransferIn"].ToString());
                        }

                        tranfr.pardetailslineamount = Convert.ToDecimal(dt.Rows[i]["pardetails_amt"].ToString());
                        tranfr.pardetailsutilizedamount = Convert.ToDecimal(dt.Rows[i]["Untilized"].ToString());
                        if (string.IsNullOrEmpty(dt.Rows[i]["TransferOut"].ToString()))
                        {
                            tranfr.pardetailstransferoutamount = 0;
                        }
                        else
                        {
                            tranfr.pardetailstransferoutamount = Convert.ToDecimal(dt.Rows[i]["TransferOut"].ToString());
                        }

                        tranfr.pardetailsbalencedamount = Convert.ToDecimal(dt.Rows[i]["balanced"].ToString());
                        tranfr.pardetailstransferamount = 0;
                        tranfr.totallineamount += tranfr.pardetailslineamount;
                        tranfr.totalutilizedamount += tranfr.pardetailsutilizedamount;
                        tranfr.toatalbalencedamount += tranfr.toatalbalencedamount;
                        //tranfr.pardetails_remarks = dt.Rows[i]["pardetails_remarks"].ToString();
                        //tranfr.parheaderenddate = dt.Rows[i]["parheader_enddate"].ToString();
                        //tranfr.parheaderperiod = dt.Rows[i]["parheader_period"].ToString();
                        //tranfr.parheaderdescription = dt.Rows[i]["parheader_desc"].ToString();
                        partfr.Add(tranfr);
                    }
                }
                if (viewfor == "update")
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        tranfr = new pardetailtransfer();
                        tranfr.pardetailsgid = Convert.ToInt32(dt.Rows[i]["pardetails_gid"].ToString());
                        tranfr.pardetailsexpensetype = dt.Rows[i]["pardetails_expensetype"].ToString();
                        tranfr.pardetails_yearfrom = dt.Rows[i]["YearFrom"].ToString();
                        tranfr.pardetails_yearto = dt.Rows[i]["YearTo"].ToString();
                        tranfr.pardetailsrequestfor = dt.Rows[i]["requestfor_name"].ToString();
                        tranfr.pardetails_desc = dt.Rows[i]["pardetails_desc"].ToString();

                        if (string.IsNullOrEmpty(dt.Rows[i]["TransferIn"].ToString()))
                        {
                            tranfr.pardetailstransferinamount = 0;
                        }
                        else
                        {
                            tranfr.pardetailstransferinamount = Convert.ToDecimal(dt.Rows[i]["TransferIn"].ToString());
                        }

                        tranfr.pardetailslineamount = Convert.ToDecimal(dt.Rows[i]["pardetails_amt"].ToString());
                        tranfr.pardetailsutilizedamount = Convert.ToDecimal(dt.Rows[i]["Untilized"].ToString());
                        if (string.IsNullOrEmpty(dt.Rows[i]["TransferOut"].ToString()))
                        {
                            tranfr.pardetailstransferoutamount = 0;
                        }
                        else
                        {
                            tranfr.pardetailstransferoutamount = Convert.ToDecimal(dt.Rows[i]["TransferOut"].ToString());
                        }

                        tranfr.pardetailsbalencedamount = Convert.ToDecimal(dt.Rows[i]["balanced"].ToString());
                        partfr.Add(tranfr);
                        cmdQry = new SqlCommand("pr_fb_trn_tpartransfer", objConn);
                        cmdQry.CommandType = CommandType.StoredProcedure;
                        cmdQry.Parameters.AddWithValue("@action", "parheaderdetailstransferupdate");
                        cmdQry.Parameters.AddWithValue("@pardetails_gid", tranfr.pardetailsgid);
                        cmdQry.Parameters.AddWithValue("@pardetails_transferinamount", tranfr.pardetailstransferinamount);
                        cmdQry.Parameters.AddWithValue("@pardetails_transferoutamount", tranfr.pardetailstransferoutamount);
                        res = cmdQry.ExecuteNonQuery();

                    }

                }
                return partfr;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return partfr;
            }
            finally
            {
                objConn.Close();
            }
        }
        public int GetQueueGidForMail(int refgid = 0, string ref_name = "")
        {
            try 
            {
                int result = 0;
                getconnection();
                cmdQry = new SqlCommand("pr_fb_trn_MailProcedure", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@refgid", SqlDbType.Int).Value = Convert.ToInt32(refgid);
                cmdQry.Parameters.Add("@refname", SqlDbType.VarChar).Value = ref_name.ToUpper();
                cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "getqueuegid";
                result = Convert.ToInt32(cmdQry.ExecuteScalar());
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                objConn.Close();
            }
        }
        public int GetRefGidForMail(string ref_name="")
        {
            try
            {
                int result = 0;
                getconnection();
                cmdQry = new SqlCommand("pr_fb_trn_MailProcedure", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@refname", SqlDbType.VarChar).Value = ref_name.ToUpper();
                cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "getrefgid";
                result = Convert.ToInt32(cmdQry.ExecuteScalar());
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                objConn.Close();
            }
        }
        // Correction sugumar 10-18-2016
        public int GetRefGidForMailCbfheader(string ref_name = "", string Cbfheadergid="")
        {
            try
            {
                int result = 0;
                getconnection();
                cmdQry = new SqlCommand("pr_fb_trn_MailProcedure", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@refname", SqlDbType.VarChar).Value = ref_name.ToUpper();
                cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = Cbfheadergid;
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "getrefgid";
                result = Convert.ToInt32(cmdQry.ExecuteScalar());
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {
                objConn.Close();
            }
        }
        public  string GetRequestStatus(int refgid = 0, string refname = "")
        {
            try
            {
                string result = "";
                getconnection();
                cmdQry = new SqlCommand("pr_fb_trn_MailProcedure", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@refgid", SqlDbType.Int).Value = Convert.ToInt32(refgid);
                cmdQry.Parameters.Add("@refname", SqlDbType.VarChar).Value = refname.ToUpper();
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "getreqstatus";
                result = Convert.ToString(cmdQry.ExecuteScalar());
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
        }
        public string GetNextApproverDetails(int refgid = 0, string refname = "")
        {
            try
            {
                string result = "";
                int liRaiserGid = 0;
                int liCnt = 0;
                DataTable dt=new DataTable();
                getconnection();
                cmdQry = new SqlCommand("pr_fb_trn_MailProcedure", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@refgid", SqlDbType.Int).Value = Convert.ToInt32(refgid);
                cmdQry.Parameters.Add("@refname", SqlDbType.VarChar).Value = refname.ToUpper();
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "getnextapproverdetail";
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    string actions = Convert.ToString(dt.Rows[0]["queue_action_for_name"].ToString());
                    string empid = Convert.ToString(string.IsNullOrEmpty(dt.Rows[0]["queue_action_by"].ToString())?"":dt.Rows[0]["queue_action_by"].ToString());
                    string empname ="";
                    if(empid ==""){
                    string queue_type = Convert.ToString(dt.Rows[0]["queue_to_type"].ToString());
                    string queue_to = Convert.ToString(dt.Rows[0]["queue_to"].ToString());

                    string queue_to_type = Convert.ToString(dt.Rows[0]["queue_to_name"].ToString());
                    string action_status = Convert.ToString(dt.Rows[0]["queue_action_status_name"].ToString());
                    if (dt.Rows[0]["Additional_flag"].ToString() == "N"
                        && queue_type != "G"
                        && queue_type != "R"
                        && queue_type != "I"
                        && queue_type != "E"
                        && action_status !="Approved"
                        && action_status !="Rejected")
                    {
                        string empgid = Convert.ToString(GetEmployeeHierarchy(liRaiserGid, Convert.ToString(dt.Rows[0]["queue_to_type_name"].ToString()), Convert.ToInt32(dt.Rows[0]["queue_to"].ToString())));
                        if(empgid =="0" || empgid ==""){
                            empname ="";
                        }
                        else{
                                empname = GetEmployeeName(Convert.ToInt32(empgid)); 
                        }
                    }
                    else
                    {
                        if (queue_type == "I" || queue_type == "E")
                            empname = GetEmployeeName(Convert.ToInt32(dt.Rows[0]["queue_to"].ToString()));
                        else
                            empname = "";
                    }
                    if(empname !=""){
                            result = "Next Approver : " + empname;
                    }
                    else{
                        result = "Next Approval Stage : " + queue_to_type;
                    }
                              
                    }
                }
                          
                      return result;
                       
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            finally
            {
                objConn.Close();
            }
        }

        public List<Attachment> getattachmentdetails(string id, string process)
        {
            List<Attachment> attachment = new List<Attachment>();
            try
            {
                getconnection();
                DataTable dt = new DataTable();
                cmdQry = new SqlCommand("pr_fb_trn_attachmentall", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(id);
                cmdQry.Parameters.Add("@process", SqlDbType.VarChar).Value = process.ToUpper().Trim();
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "viewparattachment";
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Attachment attach = new Attachment();
                        attach.attachGid = Convert.ToString(dt.Rows[i]["attachment_gid"].ToString());
                        attach.fileName = dt.Rows[i]["attachment_filename"].ToString();
                        attach.attachedDate = dt.Rows[i]["attachment_date"].ToString();
                        attach.description = dt.Rows[i]["attachment_desc"].ToString();
                        attach.attachedDate = DateTime.Now.ToString("dd/MM/yyyy");
                        attach.FileTempName = dt.Rows[i]["FileTempName"].ToString();
                        attachment.Add(attach);
                    }
                }
                return attachment;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return attachment;
            }
            finally
            {
                objConn.Close();
            }
        }
        public List<CbfSumEntity> GetCBFAttachmentTemp(int rownum = 0, int cbfdetid = 0)
        {
            List<CbfSumEntity> attachment = new List<CbfSumEntity>();
            try
            {
                getconnection();
                DataTable dt = new DataTable();
                cmdQry = new SqlCommand("pr_fb_trn_attachmentall", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                if (cbfdetid != 0)
                {
                    cmdQry.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(cbfdetid);
                    cmdQry.Parameters.Add("@process", SqlDbType.VarChar).Value = "CBFDET";
                }
                else
                {
                    cmdQry.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(rownum);
                    cmdQry.Parameters.Add("@process", SqlDbType.VarChar).Value = "CBFTEMP";
                }
                               
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "viewcbftempattachment";
                cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(objCmnFunctions.GetLoginUserGid());
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        CbfSumEntity attach = new CbfSumEntity();
                        attach._CBFAttachmentID = Convert.ToInt32(dt.Rows[i]["attachment_gid"].ToString());
                        attach._CBFAttachmentFileName = dt.Rows[i]["attachment_filename"].ToString();
                        attach._CBFAttachmentDate = dt.Rows[i]["attachment_date"].ToString();
                        attach._CBFAttachmentDescription = dt.Rows[i]["attachment_desc"].ToString();
                        attachment.Add(attach);
                    }
                }
                return attachment;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return attachment;
            }
            finally
            {
                objConn.Close();
            }
        }
        public void DeleteCBFAttachment(int refgid = 0)
        {
            try
            {
                getconnection();
                DataTable dt = new DataTable();
                cmdQry = new SqlCommand("pr_fb_trn_attachmentall", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(refgid);
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "deletecbftempattachment";
                cmdQry.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                objConn.Close();
            }
        }
        public void InsertCBFAttachment(CbfSumEntity objCBFSumEntity)
        {
            try
            {
                getconnection();
                DataTable dt = new DataTable();
                cmdQry = new SqlCommand("pr_fb_trn_attachmentall", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@process", SqlDbType.VarChar).Value = objCBFSumEntity._CBFAttachmentFor;
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectrefgid";
                int refflag = Convert.ToInt32(cmdQry.ExecuteScalar());
                string[,] codes = new string[,]
	               {
                        {"attachment_ref_flag",Convert.ToString(refflag)},
                        {"attachment_ref_gid", Convert.ToString(objCBFSumEntity._CBFAttachmentID)},                      
	                    {"attachment_attachmenttype_gid", "2"},                        
	                    {"attachment_by", objCmnFunctions.GetLoginUserGid().ToString()},
                        {"attachment_date",objCmnFunctions.convertoDateTimeString(objCBFSumEntity._CBFAttachmentDate).ToString()},
                        {"attachment_desc", Convert.ToString(objCBFSumEntity._CBFAttachmentDescription)},
                        {"attachment_filename", Convert.ToString(objCBFSumEntity._CBFAttachmentFileName)},  
                        {"attachment_isremoved", "N"}                                           
                  };
                string tname = "iem_trn_tattachment";
                string insertcommend = objCommonIUD.InsertCommon(codes, tname);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                objConn.Close();
            }
        }
        public List<CbfSumEntity> GetPROpexAttachment(int prdetid = 0) 
        {
            List<CbfSumEntity> attachment = new List<CbfSumEntity>();
            try
            {
                getconnection();
                DataTable dt = new DataTable();
                cmdQry = new SqlCommand("pr_fb_trn_attachmentall", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(prdetid);
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "viewprattachments";
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        CbfSumEntity attach = new CbfSumEntity();
                        attach._CBFAttachmentID = Convert.ToInt32(dt.Rows[i]["attachment_gid"].ToString());
                        attach._CBFAttachmentFileName = dt.Rows[i]["attachment_filename"].ToString();
                        attach._CBFAttachmentDate = dt.Rows[i]["attachment_date"].ToString();
                        attach._CBFAttachmentDescription = dt.Rows[i]["attachment_desc"].ToString();
                        attachment.Add(attach);
                    }
                }
                return attachment;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return attachment;
            }
            finally
            {
                objConn.Close();
            }
        }
        public List<CbfSumEntity> GetPARAttachmentTemp(int rownum = 0, int cbfdetid = 0) 
        {
            List<CbfSumEntity> attachment = new List<CbfSumEntity>();
            try
            {
                getconnection();
                DataTable dt = new DataTable();
                cmdQry = new SqlCommand("pr_fb_trn_attachmentall", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                if (cbfdetid != 0)
                {
                    cmdQry.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(cbfdetid);
                    cmdQry.Parameters.Add("@process", SqlDbType.VarChar).Value = "PARDET";
                }
                else
                {
                    cmdQry.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(rownum);
                    cmdQry.Parameters.Add("@process", SqlDbType.VarChar).Value = "PARTEMP";
                }

                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "viewcbftempattachment";
                cmdQry.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(objCmnFunctions.GetLoginUserGid());
                objDataAdapter = new SqlDataAdapter(cmdQry);
                objDataAdapter.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        CbfSumEntity attach = new CbfSumEntity();
                        attach._CBFAttachmentID = Convert.ToInt32(dt.Rows[i]["attachment_gid"].ToString());
                        attach._CBFAttachmentFileName = dt.Rows[i]["attachment_filename"].ToString();
                        attach._CBFAttachmentDate = dt.Rows[i]["attachment_date"].ToString();
                        attach._CBFAttachmentDescription = dt.Rows[i]["attachment_desc"].ToString();
                        attachment.Add(attach);
                    }
                }
                return attachment;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return attachment;
            }
            finally
            {
                objConn.Close();
            }
        }
        public void DeletePARAttachment(int refgid = 0)
        {
            try
            {
                getconnection();
                DataTable dt = new DataTable();
                cmdQry = new SqlCommand("pr_fb_trn_attachmentall", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@id", SqlDbType.Int).Value = Convert.ToInt32(refgid);
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "deletecbftempattachment";
                cmdQry.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                objConn.Close();
            }
        }
        public void InsertPARAttachment(CbfSumEntity objCBFSumEntity)
        {
            try 
            {
                getconnection();
                DataTable dt = new DataTable();
                cmdQry = new SqlCommand("pr_fb_trn_attachmentall", objConn);
                cmdQry.CommandType = CommandType.StoredProcedure;
                cmdQry.Parameters.Add("@process", SqlDbType.VarChar).Value = objCBFSumEntity._CBFAttachmentFor;
                cmdQry.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectrefgid";
                int refflag = Convert.ToInt32(cmdQry.ExecuteScalar());
                string[,] codes = new string[,]
	               {
                        {"attachment_ref_flag",Convert.ToString(refflag)},
                        {"attachment_ref_gid", Convert.ToString(objCBFSumEntity._CBFAttachmentID)},                      
	                    {"attachment_attachmenttype_gid", "2"},                        
	                    {"attachment_by", objCmnFunctions.GetLoginUserGid().ToString()},
                        {"attachment_date",objCmnFunctions.convertoDateTimeString(objCBFSumEntity._CBFAttachmentDate).ToString()},
                        {"attachment_desc", Convert.ToString(objCBFSumEntity._CBFAttachmentDescription)},
                        {"attachment_filename", Convert.ToString(objCBFSumEntity._CBFAttachmentFileName)},  
                        {"attachment_isremoved", "N"}                                           
                  };
                string tname = "iem_trn_tattachment";
                string insertcommend = objCommonIUD.InsertCommon(codes, tname);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                objConn.Close();
            }
        }
    }
}
