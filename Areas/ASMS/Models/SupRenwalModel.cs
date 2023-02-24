using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using IEM.Common;
using System.Data;
using System.Web.Providers.Entities;
//using System.DirectoryServices;


namespace IEM.Areas.ASMS.Models
{

    public class SupRenwalModel : IRepositoryRenewal
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions comuid = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        CommonIUD comm = new CommonIUD();
        DataSet Getds;
        DataTable ndt;
        CmnFunctions objCmnFunctions = new CmnFunctions();
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

        public IEnumerable<SupOrganizationType> GetOrganization()
        {
            DataTable dt;
            SqlDataAdapter Sa;
            List<SupOrganizationType> objOrgan = new List<SupOrganizationType>();
            SupOrganizationType objorg;
            try
            {
                cmd = new SqlCommand("Pro_GetOrganizationType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                dt = new DataTable();
                Sa = new SqlDataAdapter(cmd);
                Sa.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objorg = new SupOrganizationType();
                    objorg._OrganizationTypeID = Convert.ToInt32(dt.Rows[i]["OrganizationType_gid"].ToString());
                    objorg._OrganizationName = dt.Rows[i]["OrganizationType_Name"].ToString();
                    objOrgan.Add(objorg);
                }
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return objOrgan;
        }

        public IEnumerable<SupplierContractRenewal> GetfullRenewal()
        {
            DataTable dt;
            SqlDataAdapter Sa;
            GetConnection();
            List<SupplierContractRenewal> objOrgan = new List<SupplierContractRenewal>();
            SupplierContractRenewal objorg;
            try
            {
                cmd = new SqlCommand("GetSuppRenewalheader", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Supp_Code", string.Empty);
                cmd.Parameters.AddWithValue("@Supp_Name", string.Empty);
                cmd.Parameters.AddWithValue("@ContractFrom", string.Empty);
                cmd.Parameters.AddWithValue("@ContractTo", string.Empty);
                cmd.Parameters.AddWithValue("@Supp_gid", string.Empty);
                cmd.Parameters.AddWithValue("@type", "full");
                dt = new DataTable();
                Sa = new SqlDataAdapter(cmd);
                Sa.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objorg = new SupplierContractRenewal();
                    objorg.SupplierheadGid = dt.Rows[i]["SupplierheadGid"].ToString();
                    objorg.SupplierCode = dt.Rows[i]["SupplierCode"].ToString();
                    objorg.SupplierName = dt.Rows[i]["SupplierName"].ToString();
                    objorg.SupplierClassfication = dt.Rows[i]["classificationName"].ToString();
                    objorg.RquestType = dt.Rows[i]["ReqType"].ToString();
                    objorg.RquestStatus = dt.Rows[i]["ReqStatus"].ToString();
                    objorg.SupplierStatus = dt.Rows[i]["Currstatus"].ToString();
                    objorg.Cont_startDate = dt.Rows[i]["startDate"].ToString();
                    objorg.Cont_EndDate = dt.Rows[i]["EndDate"].ToString();
                    objorg.ActivityCount = dt.Rows[i]["ActivityCount"].ToString();
                    objOrgan.Add(objorg);
                }
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return objOrgan;
        }

        public IEnumerable<SupplierContractRenewal> GetSearchRenewal(string txtSuppliercode, string txtSupplierName, string txtContStartDate, string txtContEndDate, string ddlContractdays)
        {
            DataTable dt;

            SqlDataAdapter Sa;
            string SupplierHeadergid = string.Empty;

            GetConnection();
            List<SupplierContractRenewal> objOrgan = new List<SupplierContractRenewal>();
            SupplierContractRenewal objorg;
            try
            {

                if (ddlContractdays != "0" && ddlContractdays != string.Empty)
                {
                    SupplierHeadergid = getdaysandgid(ddlContractdays);
                }
                string cfrom = string.IsNullOrEmpty(txtContStartDate) ? string.Empty : convertoDate(txtContStartDate);
                string cto = string.IsNullOrEmpty(txtContEndDate) ? string.Empty : convertoDate(txtContEndDate);
                string sqlquery = "SELECT SH.SupplierHeader_gid as 'SupplierHeadGid',SH.supplierheader_suppliercode as 'SupplierCode'";
                sqlquery += ",SH.supplierheader_name as 'SupplierName' ,SH.supplierheader_suppliertype as 'SupplierType'";
                sqlquery += ",SC.suppliercategory_name as 'classificationName' ,SH.supplierheader_requesttype as 'ReqType' ";
                sqlquery += ",SH.supplierheader_requeststatus as 'ReqStatus',SH.supplierheader_status as 'Currstatus' ";
                sqlquery += ",convert(varchar(10),SH.supplierheader_contractfrom,103) as 'startDate',convert(varchar(10),SH.supplierheader_contractto,103) as 'EndDate'";
                sqlquery += ",'1' as 'ActivityCount' FROM asms_trn_tsupplierheader AS SH  ";
                sqlquery += " LEFT JOIN asms_mst_tsuppliercategory AS SC ON SH.supplierheader_suppliercategory_gid=SC.suppliercategory_gid  AND SC.suppliercategory_isremoved='N'  ";
                //sqlquery += "WHERE (SH.supplierheader_suppliercode='" + txtSuppliercode + "'   OR  SH.supplierheader_name='" + txtSupplierName + "'   OR ";
                //sqlquery += "SH.SupplierHeader_ContractFrom='" + cfrom + "' OR SH.SupplierHeader_ContractTo='" + cto + "'  ";
                sqlquery += "WHERE 1=1";
                if (txtSuppliercode != string.Empty)
                {
                    sqlquery += "AND SH.supplierheader_suppliercode LIKE '" + txtSuppliercode.ToUpper() + '%' + "' ";
                }
                if (txtSupplierName != string.Empty)
                {
                    sqlquery += "AND SH.supplierheader_name LIKE '" + txtSupplierName.ToUpper() + '%' + "' ";
                }
                if (txtContStartDate != string.Empty)
                {
                    sqlquery += "AND SH.SupplierHeader_ContractFrom='" + cfrom + "' ";
                }
                if (txtContEndDate != string.Empty)
                {
                    sqlquery += "AND SH.SupplierHeader_ContractTo='" + cto + "' ";
                }
                if (SupplierHeadergid != string.Empty)
                {
                    sqlquery += " AND SH.SupplierHeader_gid IN (" + SupplierHeadergid + ") ";
                }

                sqlquery += " AND  SH.supplierheader_isremoved='N'  ";

                cmd = new SqlCommand(sqlquery, con);


                //cmd = new SqlCommand("GetSuppRenewalheader", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Supp_Code", string.IsNullOrEmpty(txtSuppliercode) ? string.Empty : txtSuppliercode);
                //cmd.Parameters.AddWithValue("@Supp_Name", string.IsNullOrEmpty(txtSupplierName) ? string.Empty : txtSupplierName);
                ////cmd.Parameters.AddWithValue("@ContractFrom", string.IsNullOrEmpty(txtContStartDate) ? string.Empty : txtContStartDate);
                //cmd.Parameters.AddWithValue("@ContractFrom", string.IsNullOrEmpty(txtContStartDate) ? string.Empty : convertoDate(txtContStartDate));
                ////cmd.Parameters.AddWithValue("@ContractTo", string.IsNullOrEmpty(txtContEndDate) ? string.Empty : txtContEndDate);
                //cmd.Parameters.AddWithValue("@ContractTo", string.IsNullOrEmpty(txtContEndDate) ? string.Empty : convertoDate(txtContEndDate));
                //cmd.Parameters.AddWithValue("@Supp_gid", string.IsNullOrEmpty(SupplierHeadergid) ? string.Empty : SupplierHeadergid.Replace(",","','"));
                //cmd.Parameters.AddWithValue("@type", "search");
                dt = new DataTable();
                Sa = new SqlDataAdapter(cmd);
                Sa.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objorg = new SupplierContractRenewal();
                        objorg.SupplierheadGid = dt.Rows[i]["SupplierHeadGid"].ToString();
                        objorg.SupplierCode = dt.Rows[i]["SupplierCode"].ToString();
                        objorg.SupplierName = dt.Rows[i]["SupplierName"].ToString();
                        objorg.SupplierClassfication = dt.Rows[i]["classificationName"].ToString();
                        objorg.RquestType = dt.Rows[i]["ReqType"].ToString();
                        objorg.RquestStatus = dt.Rows[i]["ReqStatus"].ToString();
                        objorg.SupplierStatus = dt.Rows[i]["Currstatus"].ToString();
                        objorg.Cont_startDate = dt.Rows[i]["startDate"].ToString();
                        objorg.Cont_EndDate = dt.Rows[i]["EndDate"].ToString();
                        objorg.ActivityCount = dt.Rows[i]["ActivityCount"].ToString();
                        objOrgan.Add(objorg);
                    }
                }

            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return objOrgan;
        }

        private string getdaysandgid(string contdays)
        {
            string Suppliergid = string.Empty;
            DataTable dtgetcount;
            try
            {
                dtgetcount = new DataTable();
                SqlCommand cmdcount = new SqlCommand("pr_asms_getdayscount", con);
                cmdcount.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sacount = new SqlDataAdapter(cmdcount);
                sacount.Fill(dtgetcount);
                if (dtgetcount.Rows.Count > 0)
                {
                    switch (Convert.ToInt32(contdays))
                    {
                        case 1:
                            for (int i = 0; i < dtgetcount.Rows.Count; i++)
                            {
                                int j = Convert.ToInt32(dtgetcount.Rows[i]["Countdays"].ToString());
                                if (j >= 0 && j <= 30)
                                {
                                    if (Suppliergid == string.Empty)
                                    {
                                        Suppliergid += dtgetcount.Rows[i]["supplierheader_gid"].ToString();
                                    }
                                    else
                                    {
                                        Suppliergid += "," + dtgetcount.Rows[i]["supplierheader_gid"].ToString();
                                    }

                                }
                            }
                            break;
                        case 2:
                            for (int i = 0; i < dtgetcount.Rows.Count; i++)
                            {
                                int j = Convert.ToInt32(dtgetcount.Rows[i]["Countdays"].ToString());
                                if (j >= 31 && j <= 60)
                                {
                                    if (Suppliergid == string.Empty)
                                    {
                                        Suppliergid = dtgetcount.Rows[i]["supplierheader_gid"].ToString();
                                    }
                                    else
                                    {
                                        Suppliergid += "," + dtgetcount.Rows[i]["supplierheader_gid"].ToString();
                                    }

                                }
                            }
                            break;
                        case 3:
                            for (int i = 0; i < dtgetcount.Rows.Count; i++)
                            {
                                int j = Convert.ToInt32(dtgetcount.Rows[i]["Countdays"].ToString());
                                if (j >= 61 && j <= 90)
                                {
                                    if (Suppliergid == string.Empty)
                                    {
                                        Suppliergid = dtgetcount.Rows[i]["supplierheader_gid"].ToString();
                                    }
                                    else
                                    {
                                        Suppliergid += "," + dtgetcount.Rows[i]["supplierheader_gid"].ToString();
                                    }
                                }
                            }
                            break;
                        case 4:
                            for (int i = 0; i < dtgetcount.Rows.Count; i++)
                            {
                                int j = Convert.ToInt32(dtgetcount.Rows[i]["Countdays"].ToString());
                                if (j >= 91 && j <= 120)
                                {
                                    if (Suppliergid == string.Empty)
                                    {
                                        Suppliergid = dtgetcount.Rows[i]["supplierheader_gid"].ToString();
                                    }
                                    else
                                    {
                                        Suppliergid += "," + dtgetcount.Rows[i]["supplierheader_gid"].ToString();
                                    }
                                }
                            }
                            break;
                        case 5:
                            for (int i = 0; i < dtgetcount.Rows.Count; i++)
                            {
                                int j = Convert.ToInt32(dtgetcount.Rows[i]["Countdays"].ToString());
                                if (j <= 0)
                                {
                                    if (Suppliergid == string.Empty)
                                    {
                                        Suppliergid = dtgetcount.Rows[i]["supplierheader_gid"].ToString();
                                    }
                                    else
                                    {
                                        Suppliergid += "," + dtgetcount.Rows[i]["supplierheader_gid"].ToString();
                                    }
                                }
                            }
                            break;

                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return Suppliergid;
        }
        public IEnumerable<SupplierContractRenewal> InsertRenewal(SupplierContractRenewal obj)
        {


            DataTable dttemp = new DataTable();
            string updcomm = string.Empty;
            List<SupplierContractRenewal> objrene = new List<SupplierContractRenewal>();
            string attresult = string.Empty;
            try
            {



                #region "Insert renewal
                string[,] codes = new string[,]
                   {
        {"renewal_supplierheader_gid",obj.SupplierheadGid.ToString() },
        {"renewal_contractstartdate_old",convertoDate(obj.Cont_EndDate.ToString())},
        {"renewal_contractstartdate_new",convertoDate(obj.Cont_startDate.ToString())},
        {"renewal_contractenddate_old", convertoDate(obj.Cont_EndDate.ToString())},
        {"renewal_contractenddate_new",convertoDate(obj.Cont_EndDateNew.ToString())},
        {"renewal_renewedby",Convert.ToString(comuid.GetLoginUserGid())},
        {"renewal_renewedon",convertoDate(DateTime.Now.ToString())},
        {"renewal_description",string.IsNullOrEmpty(obj.Description) ? string.Empty : obj.Description.ToString() },
        {"renewal_isremoved", "N"}
        //,
        //{"renewal_AttachFname",obj.filename.ToString()}
       
       
                  };
                string tname = "asms_trn_tRenewal";
                string insertcommend = comm.InsertCommon(codes, tname);
                #endregion

                #region
                //cmd = new SqlCommand("pr_asms_GetRenewalGid", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //da = new SqlDataAdapter(cmd);
                //DataTable dtgetgid = new DataTable();
                //da.Fill
                string[,] getrgid = new string[,]
                {
                    {"@SupplierHeadergid",obj.SupplierheadGid.ToString()}
                };

                string maxrgid = comm.ProcedureCommon(getrgid, "pr_asms_GetRenewalGid");
                #endregion

                #region "AttachInsert"
                if (maxrgid != string.Empty && obj.filename != null)
                {
                    string[,] Att = new string[,]
                {
                    {"renewalattachment_renewal_gid",maxrgid},
                    {"renewalattachment_filename",obj.filename.ToString()},
                    {"renewalattachment_insert_by",Convert.ToString(comuid.GetLoginUserGid())},
                    {"renewalattachment_insert_date",DateTime.Now.ToString("dd-MMM-yyyy")},
                    {"renewalattachment_update_by",Convert.ToString(comuid.GetLoginUserGid())},
                    {"renewalattachment_update_date",DateTime.Now.ToString("dd-MMM-yyyy")},
                    {"renewalattachment_isremoved","N"}
                };

                    attresult = comm.InsertCommon(Att, "asms_trn_trenewalattachment");
                }

                #endregion



                #region "update renewal

                if (insertcommend == "success")
                {


                    string[,] ucodes = new string[,]
	{
         {"supplierheader_contractfrom", convertoDate(obj.Cont_startDate.ToString())},
         {"supplierheader_contractto",convertoDate(obj.Cont_EndDateNew.ToString())}
	};
                    string[,] whrcos = new string[,]
	{
         {"supplierheader_gid", obj.SupplierheadGid.ToString()}
            
    };
                    string tblname = "asms_trn_tsupplierheader";

                    updcomm = comm.UpdateCommon(ucodes, whrcos, tblname);
                }
                #endregion

                if (updcomm == "Success")
                {
                    dttemp.Columns.Add("SupplierCode", typeof(string));
                    dttemp.Columns.Add("SupplierName", typeof(string));
                    dttemp.Columns.Add("ContractStartDate", typeof(string));
                    dttemp.Columns.Add("ContractEndDate", typeof(string));
                    dttemp.Columns.Add("RenewalDate", typeof(string));
                    dttemp.Columns.Add("Description", typeof(string));
                    dttemp.Columns.Add("AttachFName", typeof(string));

                    dttemp.Rows.Add(
                        obj.SupplierCode.ToString()
                        , obj.SupplierName.ToString()
                        , obj.Cont_startDate.ToString()
                        , obj.Cont_EndDate.ToString()
                        , obj.Cont_EndDateNew.ToString()
                        , obj.Description.ToString()
                        , string.IsNullOrEmpty(obj.filename) ? string.Empty : obj.filename.ToString()
                                            );


                    for (int i = 0; i < dttemp.Rows.Count; i++)
                    {
                        SupplierContractRenewal objvar = new SupplierContractRenewal();
                        objvar.SupplierCode = dttemp.Rows[i]["SupplierCode"].ToString();
                        objvar.SupplierName = dttemp.Rows[i]["SupplierName"].ToString();
                        objvar.Cont_startDate = dttemp.Rows[i]["ContractStartDate"].ToString();
                        objvar.Cont_EndDate = dttemp.Rows[i]["ContractEndDate"].ToString();
                        objvar.Cont_EndDateNew = dttemp.Rows[i]["RenewalDate"].ToString();
                        objvar.Description = dttemp.Rows[i]["Description"].ToString();
                        objvar.filename = dttemp.Rows[i]["AttachFName"].ToString();
                        objrene.Add(objvar);
                    }
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return objrene;
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
        }

        public IEnumerable<SupplierTriggerRenewal> GetTriggerDetails(SupplierTriggerRenewal obj)
        {
            // CommonIUD comm = new CommonIUD();
            List<SupplierTriggerRenewal> GetTriobj = new List<SupplierTriggerRenewal>();
            string insertcommend = string.Empty;
            try
            {
                if (obj.status != "yes")
                {
                    #region "Insert Triggerrenewal
                    string[,] codes = new string[,]
                   {
     
        {"renewaltrigger_triggerbefore",obj.Trigger_before.ToString()},
        {"renewaltrigger_subject",obj.subject.ToString()},
        {"renewaltrigger_message",obj.Message.ToString()},
        {"renewaltrigger_insert_by",Convert.ToString(comuid.GetLoginUserGid())},
        {"renewaltrigger_insert_date",DateTime.Now.ToString("dd/MMM/yyyy")},
        {"renewaltrigger_update_by",Convert.ToString(comuid.GetLoginUserGid())},
        {"renewaltrigger_update_date",DateTime.Now.ToString("dd/MMM/yyyy")},
        {"renewaltrigger_isremoved","N"}
      
                  };
                    string tname = "asms_mst_trenewaltrigger";
                    insertcommend = comm.InsertCommon(codes, tname);
                    #endregion
                }

                if (insertcommend == "success" || obj.status == "yes")
                {
                    GetConnection();
                    cmd = new SqlCommand("asms_pr_suppGetTriggerDetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter(cmd);
                    ndt = new DataTable();
                    da.Fill(ndt);
                    for (int i = 0; i < ndt.Rows.Count; i++)
                    {
                        obj = new SupplierTriggerRenewal();
                        obj.Slno = ndt.Rows[i][0].ToString();
                        obj.Trigger_before = ndt.Rows[i][1].ToString();
                        obj.subject = ndt.Rows[i][2].ToString();
                        obj.Message = ndt.Rows[i][3].ToString();
                        obj.InsertDate = DateTime.Now.ToString();
                        obj.Insertby = Convert.ToString(comuid.GetLoginUserGid());
                        GetTriobj.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return GetTriobj;
        }



        public string UpdateSupplierTrigger(SupplierTriggerRenewal objtrig)
        {

            // CommonIUD comm = new CommonIUD();
            string updcomm = string.Empty;
            try
            {

                string[,] uTcodes = new string[,]
	{
         {"renewaltrigger_triggerbefore", objtrig.Trigger_before.ToString()},
         {"renewaltrigger_subject",objtrig.subject.ToString()},
         {"renewaltrigger_message",objtrig.Message.ToString()},
      
	};
                string[,] Twhrcos = new string[,]
	{
         {"renewaltrigger_gid", objtrig.Slno.ToString()}
            
    };
                string tblname = "asms_mst_trenewaltrigger";

                updcomm = comm.UpdateCommon(uTcodes, Twhrcos, tblname);


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                updcomm = ex.Message.ToString();
            }



            return updcomm;
        }

        public string DeleteSupplierTrigger(SupplierTriggerRenewal objtrig)
        {

            //  CommonIUD comm = new CommonIUD();
            string decomm = string.Empty;
            try
            {

                string[,] DTcodes = new string[,]
	{
       {"renewaltrigger_isremoved","Y"}
      
	};
                string[,] Dwhrcos = new string[,]
	{
         {"renewaltrigger_gid", objtrig.Slno.ToString()}
            
    };
                string tblname = "asms_mst_trenewaltrigger";

                decomm = comm.UpdateCommon(DTcodes, Dwhrcos, tblname);


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                decomm = ex.Message.ToString();
            }



            return decomm;
        }

        public DataSet GetActivatequeue(string txtSuppliercode, string txtSupplierName, string ddlSupplierStatus, string ddlSup_Clasification, string ddlRequestType, string ddlRequestStatus)
        {
            Getds = new DataSet();
            // SupplierActivation objAct = new SupplierActivation();
            try
            {
                // var Sstatus = (ddlSupplierStatus == "0" ? DBNull.Value.ToString() : ddlSupplierStatus);
                var Sstatus = string.IsNullOrEmpty(ddlSupplierStatus) ? string.Empty : ddlSupplierStatus;
                var Classification = (ddlSup_Clasification == "0" ? DBNull.Value.ToString() : ddlSup_Clasification);
                var Rtype = (ddlRequestType == "0" ? DBNull.Value.ToString() : ddlRequestType);
                var Rstatus = (ddlRequestStatus == "0" ? DBNull.Value.ToString() : ddlRequestStatus);
                GetConnection();
                cmd = new SqlCommand("pr_asms_AGetSup_Activation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SupplierCode", string.IsNullOrEmpty(txtSuppliercode) ? string.Empty : txtSuppliercode.ToUpper());
                cmd.Parameters.AddWithValue("@SupplierName", string.IsNullOrEmpty(txtSupplierName) ? string.Empty : txtSupplierName.ToUpper());
                cmd.Parameters.AddWithValue("@SupplierStatus", Sstatus);
                cmd.Parameters.AddWithValue("@SupplierClassification", Classification);
                cmd.Parameters.AddWithValue("@RequestType", Rtype);
                cmd.Parameters.AddWithValue("@RequestStatus", Rstatus);
                if (txtSuppliercode == string.Empty && txtSupplierName == string.Empty && Sstatus == string.Empty && Classification == string.Empty && Rtype == string.Empty && Rstatus == string.Empty)
                {
                    cmd.Parameters.AddWithValue("@Type", "full");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Type", "Search");
                }
                cmd.Parameters.AddWithValue("@Ownergid", objCmnFunctions.GetLoginUserGid());

                da = new SqlDataAdapter(cmd);
                da.Fill(Getds);
                //for(int i=0;i<ndt.Rows.Count;i++)
                //{
                //    objAct.SupplierCode = ndt.Rows[i][""].ToString();
                //    objAct.SupplierName = ndt.Rows[i][""].ToString();
                //    objAct.SupplierClassification = ndt.Rows[i][""].ToString();
                //    objAct.SupplierCode = ndt.Rows[i][""].ToString();
                //    objAct.SupplierCode = ndt.Rows[i][""].ToString();
                //    objAct.SupplierCode = ndt.Rows[i][""].ToString();
                //    objAct.SupplierCode = ndt.Rows[i][""].ToString();
                //}

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return Getds;

        }

        public DataSet GetDeActivatequeue(string txtSuppliercode, string txtSupplierName, string ddlSupplierStatus, string ddlSup_Clasification, string ddlRequestType, string ddlRequestStatus)
        {
            Getds = new DataSet();
            //  SupplierDeActvation objAct = new SupplierDeActvation();
            try
            {
                // var Sstatus = (ddlSupplierStatus == "0" ? DBNull.Value.ToString() : ddlSupplierStatus);
                string Sstatus = string.IsNullOrEmpty(ddlSupplierStatus) ? string.Empty : ddlSupplierStatus;
                //var Classification = (ddlSup_Clasification == "0" ? DBNull.Value.ToString() : ddlSup_Clasification);
                //var Rtype = (ddlRequestType == "0" ? DBNull.Value.ToString() : ddlRequestType);
                //var Rstatus = (ddlRequestStatus == "0" ? DBNull.Value.ToString() : ddlRequestStatus);
                string Classification =string.IsNullOrEmpty(ddlSup_Clasification) ? string.Empty : ddlSup_Clasification.ToString();
                string Rtype = string.IsNullOrEmpty(ddlRequestType) ? string.Empty : ddlRequestType.ToString();
                string Rstatus =string.IsNullOrEmpty(ddlRequestStatus) ? string.Empty : ddlRequestStatus.ToString();
                GetConnection();
                cmd = new SqlCommand("pr_asms_AGetSup_INActivation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SupplierCode", string.IsNullOrEmpty(txtSuppliercode) ? string.Empty : txtSuppliercode.ToUpper());
                cmd.Parameters.AddWithValue("@SupplierName", string.IsNullOrEmpty(txtSupplierName) ? string.Empty : txtSupplierName.ToUpper());
                cmd.Parameters.AddWithValue("@SupplierStatus", Sstatus.ToUpper());
                cmd.Parameters.AddWithValue("@SupplierClassification", Classification);
                cmd.Parameters.AddWithValue("@RequestType", Rtype.ToUpper());
                cmd.Parameters.AddWithValue("@RequestStatus", Rstatus);
                if (txtSuppliercode == string.Empty && txtSupplierName == string.Empty && Sstatus == string.Empty && Classification == string.Empty && Rtype == string.Empty && Rstatus == string.Empty)
                {
                    cmd.Parameters.AddWithValue("@Type", "full");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Type", "Search");
                }
                cmd.Parameters.AddWithValue("@Ownergid", objCmnFunctions.GetLoginUserGid());
                da = new SqlDataAdapter(cmd);
                da.Fill(Getds);
                //for(int i=0;i<ndt.Rows.Count;i++)
                //{
                //    objAct.SupplierCode = ndt.Rows[i][""].ToString();
                //    objAct.SupplierName = ndt.Rows[i][""].ToString();
                //    objAct.SupplierClassification = ndt.Rows[i][""].ToString();
                //    objAct.SupplierCode = ndt.Rows[i][""].ToString();
                //    objAct.SupplierCode = ndt.Rows[i][""].ToString();
                //    objAct.SupplierCode = ndt.Rows[i][""].ToString();
                //    objAct.SupplierCode = ndt.Rows[i][""].ToString();
                //}

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return Getds;

        }

        public DataSet Get_SupplierModification(string txtSuppliercode, string txtSupplierName, string ddlSupplierStatus, string ddlSup_Clasification, string ddlRequestType, string ddlRequestStatus)
        {
            Getds = new DataSet();
            //  SupplierDeActvation objAct = new SupplierDeActvation();
            try
            {
                var Sstatus = (ddlSupplierStatus == "0" ? DBNull.Value.ToString() : ddlSupplierStatus);
                // var Sstatus = DBNull.Value;
                var Classification = (ddlSup_Clasification == "0" ? DBNull.Value.ToString() : ddlSup_Clasification);
                var Rtype = (ddlRequestType == "0" ? DBNull.Value.ToString() : ddlRequestType);
                //var Rstatus = (ddlRequestStatus == "0" ? DBNull.Value.ToString() : ddlRequestStatus);
                var Rstatus = string.IsNullOrEmpty(ddlRequestStatus) ? string.Empty : ddlRequestStatus;
                GetConnection();
                cmd = new SqlCommand("pr_asms_AGetSup_SuppModification", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SupplierCode", string.IsNullOrEmpty(txtSuppliercode) ? string.Empty : txtSuppliercode.ToUpper());
                cmd.Parameters.AddWithValue("@SupplierName", string.IsNullOrEmpty(txtSupplierName) ? string.Empty : txtSupplierName.ToUpper());
                cmd.Parameters.AddWithValue("@SupplierStatus", Sstatus);
                cmd.Parameters.AddWithValue("@SupplierClassification", Classification);
                cmd.Parameters.AddWithValue("@RequestType", Rtype);
                cmd.Parameters.AddWithValue("@RequestStatus", Rstatus);
                if (txtSuppliercode == string.Empty && txtSupplierName == string.Empty && Sstatus == string.Empty && Classification == string.Empty && Rtype == string.Empty && Rstatus == string.Empty)
                {
                    cmd.Parameters.AddWithValue("@Type", "full");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Type", "Search");
                }

                da = new SqlDataAdapter(cmd);
                da.Fill(Getds);
                //for(int i=0;i<ndt.Rows.Count;i++)
                //{
                //    objAct.SupplierCode = ndt.Rows[i][""].ToString();
                //    objAct.SupplierName = ndt.Rows[i][""].ToString();
                //    objAct.SupplierClassification = ndt.Rows[i][""].ToString();
                //    objAct.SupplierCode = ndt.Rows[i][""].ToString();
                //    objAct.SupplierCode = ndt.Rows[i][""].ToString();
                //    objAct.SupplierCode = ndt.Rows[i][""].ToString();
                //    objAct.SupplierCode = ndt.Rows[i][""].ToString();
                //}

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return Getds;

        }
        public DataSet GetActive_emp(string id)
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_GetActive_emp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SupplierCode", id);
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return ds;
        }

        public DataSet GetActive_emptemp(string id, string type)
        {
            DataSet ds = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_GetActive_emptemp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SupplierCode", id);
                cmd.Parameters.AddWithValue("@CurrenStatus", type);
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return ds;
        }


        public string Update_SupplierActive(SupplierActivation uobj)
        {
            // CommonIUD comm = new CommonIUD();
            string result = string.Empty;
            int uResult = 0;
            int mainSupplierGid = 0;
            try
            {
                #region "UpdatesupplierHeader"
                string updcomm = string.Empty;

                string[,] uTcodes = new string[,]
	{
         {"supplierheader_status", "Approved"},
           
	};
                string[,] Twhrcos = new string[,]
	{
        // {"supplierheader_gid", uobj.Supplierheadergid.ToString()},
         {"supplierheader_suppliercode",uobj.SupplierCode.ToString()}
            
    };
                string tblname = "asms_trn_tsupplierheader";

                updcomm = comm.UpdateCommon(uTcodes, Twhrcos, tblname);
                #endregion

                mainSupplierGid = GetSuppliergid(uobj.SupplierCode.ToString());

                #region "InsertActivity"
                if (updcomm == "Success")
                {
                    string[,] codes = new string[,]
                   {
     
       // {"supplieractivation_supplierheader_gid",uobj.Supplierheadergid},
        {"supplieractivation_supplierheader_gid",Convert.ToString(mainSupplierGid)},
        {"supplieractivation_currentstatus","A"},
      //  {"supplieractivation_activationfrom",convertoDate(uobj.Fromdate.ToString())},
        {"supplieractivation_activationfrom",convertoDate(uobj.Fdate.ToString())},
        {"supplieractivation_activationto",convertoDate(uobj.Todate.ToString())},
        {"supplieractivation_activationreason",uobj.ActiveReason},
         {"supplieractivation_insert_by",Convert.ToString(comuid.GetLoginUserGid())},
          {"supplieractivation_insert_date",DateTime.Now.ToString("yyyy-MMM-dd")},
           {"supplieractivation_comments",uobj.comments},
           {"supplieractivation_isremoved","N"}
      
                  };
                    string tname = "asms_trn_tsupplieractivation";
                    result = comm.InsertCommon(codes, tname);
                }

                #endregion

                #region "temptabledisabled"

                if (result == "success")
                {
                    string[,] dcode = new string[,]
                    {
                        {"supplieractivation_isremoved","Y"}
                    };

                    string[,] wdcode = new string[,]
                    {
                        {"supplieractivation_supplierheader_gid",uobj.Supplierheadergid}
                    };
                    string tmpname = "asms_tmp_tsupplieractivation";
                    result = comm.UpdateCommon(dcode, wdcode, tmpname);
                }
                #endregion

                #region "SelectActivitygid"
                GetConnection();
                cmd = new SqlCommand("pr_asms_GetActivitygidnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                int active_gid = Convert.ToInt32(cmd.ExecuteScalar());
                #endregion

                //disable activity and attachment table
                //GetConnection();
                //cmd = new SqlCommand("pr_asms_Activitydisabled", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@SHeadergid", uobj.Supplierheadergid);
                //uResult = cmd.ExecuteNonQuery();

                //end

                #region "InsertAttachments"
                if (uobj.arrAttachfile != null)
                {
                    if (uobj.arrAttachfile.Length > 0)
                    {
                        for (int i = 0; i < uobj.arrAttachfile.Length; i++)
                        {
                            string[,] Acodes = new string[,]
                   {
     
      
        {"saattachment_supplieractivation_gid",active_gid.ToString()},
        {"saattachment_attachmentfor","A"},
        {"saattachment_filename",uobj.arrAttachfile[i].ToString()},
            {"saattachment_insert_by",Convert.ToString(comuid.GetLoginUserGid())},
          {"saattachment_insert_date",DateTime.Now.ToString("yyyy-MMM-dd")},
          {"saattachment_update_by",Convert.ToString(comuid.GetLoginUserGid())},
          {"saattachment_update_date",DateTime.Now.ToString("yyyy-MMM-dd")},
           {"saattachment_isremoved","N"}
      
                  };
                            string Atname = "asms_trn_tsaattachment";
                            result = comm.InsertCommon(Acodes, Atname);
                        }
                    }
                }

                if (result == "success")
                {
                    string[,] attTemp = new string[,]
                    {
                          {"saattachment_isremoved","Y"}
                    };

                    string[,] attwTemp = new string[,]
                    {
                        {"saattachment_supplieractivation_gid",active_gid.ToString()}
                    };
                    string attTemptbl = "asms_tmp_tsaattachment";

                    result = comm.UpdateCommon(attTemp, attwTemp, attTemptbl);
                }

            }
                #endregion

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                result = ex.Message;
            }
            return result;
        }


        public string DeUpdate_SupplierActive(SupplierDeActvation uobj)
        {
            // CommonIUD comm = new CommonIUD();
            string result = string.Empty;
            int uResult = 0;
            int mainSupplierGid = 0;
            try
            {
                #region "UpdatesupplierHeader"
                string updcomm = string.Empty;

                string[,] uTcodes = new string[,]
	{
         {"supplierheader_status", "InActive"},
           
	};
                string[,] Twhrcos = new string[,]
	{
         //{"supplierheader_gid", uobj.DSupplierheadergid.ToString()},
         {"supplierheader_suppliercode",uobj.DSupplierCode.ToString()}
            
    };
                string tblname = "asms_trn_tsupplierheader";

                updcomm = comm.UpdateCommon(uTcodes, Twhrcos, tblname);
                #endregion

                //disable activity and attachment table
                //GetConnection();
                //cmd = new SqlCommand("pr_asms_Activitydisabled", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@SHeadergid", uobj.DSupplierheadergid);
                //uResult = cmd.ExecuteNonQuery();

                //end
                mainSupplierGid = GetSuppliergid(uobj.DSupplierCode.ToString());


                #region "InsertActivity"
                if (updcomm == "Success")
                {
                    string[,] codes = new string[,]
                   {
     
        //{"supplieractivation_supplierheader_gid",uobj.DSupplierheadergid},
        {"supplieractivation_supplierheader_gid",Convert.ToString(mainSupplierGid)},
        {"supplieractivation_currentstatus","D"},
        //{"supplieractivation_activationfrom",convertoDate(uobj.Fromdate.ToString())},
        //{"supplieractivation_activationto",convertoDate(uobj.Todate.ToString())},
        {"supplieractivation_activationreason",uobj.DeActiveReason},
         {"supplieractivation_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
          {"supplieractivation_insert_date",DateTime.Now.ToString("yyyy-MMM-dd")},
           {"supplieractivation_comments",uobj.Decomments},
           {"supplieractivation_isremoved","N"}
      
                  };
                    string tname = "asms_trn_tsupplieractivation";
                    result = comm.InsertCommon(codes, tname);
                }

                #endregion

                #region "SelectActivitygid"
                GetConnection();
                cmd = new SqlCommand("pr_asms_GetActivitygidnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                int active_gid = Convert.ToInt32(cmd.ExecuteScalar());
                #endregion

                #region "InsertAttachments"
                if (uobj.arrDeAttachfile != null)
                {
                    if (uobj.arrDeAttachfile.Length > 0)
                    {
                        for (int i = 0; i < uobj.arrDeAttachfile.Length; i++)
                        {
                            string[,] Acodes = new string[,]
                   {
     
      
        {"saattachment_supplieractivation_gid",active_gid.ToString()},
        {"saattachment_attachmentfor","D"},
        {"saattachment_filename",uobj.arrDeAttachfile[i].ToString()},
            {"saattachment_insert_by",Convert.ToString(comuid.GetLoginUserGid())},
          {"saattachment_insert_date",DateTime.Now.ToString("yyyy-MMM-dd")},
          {"saattachment_update_by",Convert.ToString(comuid.GetLoginUserGid())},
          {"saattachment_update_date",DateTime.Now.ToString("yyyy-MMM-dd")},
           {"saattachment_isremoved","N"}
      
                  };
                            string Atname = "asms_trn_tsaattachment";
                            result = comm.InsertCommon(Acodes, Atname);
                        }
                    }
                }


            }
                #endregion

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                result = ex.Message;
            }
            return result;
        }

        public IEnumerable<SupplierActivation> GetEmployeeeAttachmentActivation(DataTable dtvalue)
        {
            List<SupplierActivation> iobj = new List<SupplierActivation>();
            int userid = Convert.ToInt32(comuid.GetLoginUserGid());
            try
            {
                if (dtvalue.Rows.Count > 0)
                {
                    for (int i = 0; i < dtvalue.Rows.Count; i++)
                    {
                        SupplierActivation obj = new SupplierActivation();
                        obj.Attachid = Convert.ToInt32(dtvalue.Rows[i]["Attachid"].ToString());
                        obj.AttachFileName = dtvalue.Rows[i]["AttachName"].ToString();
                        obj.AttachFileType = dtvalue.Rows[i]["AttachFileType"].ToString();
                        obj.Attachlength = dtvalue.Rows[i]["Attachlength"].ToString();
                        obj.Todate = DateTime.Now.ToString("dd-MM-yyyy");
                        obj.Insertby = userid.ToString();
                        iobj.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return iobj;
        }

        public IEnumerable<SupplierDeActvation> GetEmployeeeAttachmentDeActivation(DataTable dtvalue)
        {
            List<SupplierDeActvation> iobj = new List<SupplierDeActvation>();
            try
            {
                if (dtvalue.Rows.Count > 0)
                {
                    for (int i = 0; i < dtvalue.Rows.Count; i++)
                    {
                        SupplierDeActvation obj = new SupplierDeActvation();
                        obj.DAttachid = Convert.ToInt32(dtvalue.Rows[i]["Attachid"].ToString());
                        obj.DAttachFileName = dtvalue.Rows[i]["AttachName"].ToString();
                        obj.DAttachFileType = dtvalue.Rows[i]["AttachFileType"].ToString();
                        obj.DAttachlength = dtvalue.Rows[i]["Attachlength"].ToString();
                        obj.DTodate = DateTime.Now.ToString("dd-MM-yyyy");
                        obj.DInsertby = Convert.ToString(comuid.GetLoginUserGid());
                        iobj.Add(obj);
                    }
                }

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return iobj;
        }


        public DataTable GetTriggerEditDetails(int id)
        {
            DataTable dtGetDet = new DataTable(); ;
            try
            {
                GetConnection();
                cmd = new SqlCommand("asms_pr_suppGetTriggerEditDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Triggerid", id);
                da = new SqlDataAdapter(cmd);
                da.Fill(dtGetDet);

            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return dtGetDet;
        }


        #region "Activation Queue"
        public string Asms_ActivationQueue(SupplierActivation objAqq)
        {
            string result = string.Empty;
            DataSet dsGetRefgid = new DataSet();
            int uResult = 0;
            GetConnection();
            try
            {
                cmd = new SqlCommand("pr_asms_Getrefgids", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Employeegid", Convert.ToString(objCmnFunctions.GetLoginUserGid()));
                cmd.Parameters.AddWithValue("@Ref_Name", "ASMS");
                cmd.Parameters.AddWithValue("@RefTableName", "asms_trn_tsupplierheader");
                da = new SqlDataAdapter(cmd);
                da.Fill(dsGetRefgid);

                if (dsGetRefgid.Tables[0].Rows.Count > 0)
                {
                    //            string[,] aqcodes = new string[,]
                    //           {


                    //{"queue_date",DateTime.Now.ToString("dd-MMM-yyyy")},
                    //{"queue_ref_flag",dsGetRefgid.Tables[0].Rows[0]["ref_flag"].ToString()},
                    //{"queue_ref_gid",objAqq.Supplierheadergid},
                    // {"queue_ref_status","1"},
                    //  {"queue_from",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                    //  {"queue_to_type","I"},
                    //  {"queue_to",dsGetRefgid.Tables[1].Rows[0]["employee_supervisor"].ToString()},
                    //   {"queue_action_for","A"},
                    // //  {"queue_action_for",string.IsNullOrEmpty(objAqq.Approval) ? objAqq.Reject:objAqq.Approval},
                    //  // {"queue_action_date",DateTime.Now.ToString("dd-MMM-yyyy")},
                    // //  {"queue_action_by","N"},
                    ////   {"queue_action_status","N"},
                    //  // {"queue_action_remark","N"},
                    //  // {"queue_prev_gid","N"},
                    //   {"queue_isremoved","N"}

                    //          };
                    //            string aqtname = "iem_trn_tqueue";
                    //            result = comm.InsertCommon(aqcodes, aqtname);


                    //if (result == "success")
                    // {
                    string upqdcomm = string.Empty;

                    string[,] uTqcodes = new string[,]
    {
         {"supplierheader_requesttype", "ACTIVATION"},
         {"supplierheader_requeststatus", "WAITINGFORAPPROVAL"},
         {"supplierheader_update_by", Convert.ToString(objCmnFunctions.GetLoginUserGid())},
         {"supplierheader_update_date", DateTime.Now.ToString("dd-MMM-yyyy")}
         //,
       //  {"supplierheader_currentapprovalstage", "1"}
        
           
    };
                    string[,] Tqwhrcos = new string[,]
    {
        // {"supplierheader_gid", objAqq.Supplierheadergid},
         {"supplierheader_suppliercode",objAqq.SupplierCode}
            
    };
                    string tblname = "asms_tmp_tsupplierheader";

                    upqdcomm = comm.UpdateCommon(uTqcodes, Tqwhrcos, tblname);
                    if (upqdcomm == "Success")
                    {
                        result = upqdcomm;
                    }

                    // }
                    //disable activity and attachment table
                    GetConnection();
                    cmd = new SqlCommand("pr_asms_Activitydisabled", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SHeadergid", string.IsNullOrEmpty(objAqq.Supplierheadergid) ? objAqq.ActiheaderGid : objAqq.Supplierheadergid);
                    uResult = cmd.ExecuteNonQuery();

                    //end
                    string[,] codes = new string[,]
                   {
     
        {"supplieractivation_supplierheader_gid",string.IsNullOrEmpty(objAqq.Supplierheadergid)?objAqq.ActiheaderGid:objAqq.Supplierheadergid},
        {"supplieractivation_currentstatus","A"},
       // {"supplieractivation_activationfrom",convertoDate(objAqq.Fromdate.ToString())},
        {"supplieractivation_activationfrom",convertoDate(objAqq.Fdate.ToString())},
        {"supplieractivation_activationto",convertoDate(objAqq.Todate.ToString())},
        //kavitha for null passed in activate reason on 21-04-2017
        {"supplieractivation_activationreason",string.IsNullOrEmpty(objAqq.ActiveReason)? "" :objAqq.ActiveReason},
         {"supplieractivation_insert_by",Convert.ToString(comuid.GetLoginUserGid())},
          //{"supplieractivation_insert_date",DateTime.Now.ToString("yyyy-MMM-dd")},
           {"supplieractivation_insert_date","sysdatetime()"},
           {"supplieractivation_comments",objAqq.comments},
           {"supplieractivation_isremoved","N"}
      
                  };
                    string tname = "asms_tmp_tsupplieractivation";
                    result = comm.InsertCommon(codes, tname);
                }



        #endregion

                #region "SelectActivitygid"
                GetConnection();
                cmd = new SqlCommand("pr_asms_GetActivitygid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Employeegid", Convert.ToString(objCmnFunctions.GetLoginUserGid()));
                int active_gid = Convert.ToInt32(cmd.ExecuteScalar());
                #endregion

                #region "InsertAttachments"
                if (objAqq.arrAttachfile != null)
                {
                    if (objAqq.arrAttachfile.Length > 0)
                    {
                        for (int i = 0; i < objAqq.arrAttachfile.Length; i++)
                        {
                            string[,] Acodes = new string[,]
                   {
     
      
        {"saattachment_supplieractivation_gid",active_gid.ToString()},
        {"saattachment_attachmentfor","A"},
        {"saattachment_filename",objAqq.arrAttachfile[i].ToString()},
            {"saattachment_insert_by",Convert.ToString(comuid.GetLoginUserGid())},
        //  {"saattachment_insert_date",DateTime.Now.ToString("yyyy-MMM-dd")},
          {"saattachment_insert_date","sysdatetime()"},
       //   {"saattachment_update_by",Convert.ToString(comuid.GetLoginUserGid())},
        //  {"saattachment_update_date",DateTime.Now.ToString("yyyy-MMM-dd")},
           {"saattachment_isremoved","N"}
      
                  };
                            string Atname = "asms_tmp_tsaattachment";
                            result = comm.InsertCommon(Acodes, Atname);
                        }
                    }
                }



            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                result = ex.Message.ToString();
            }
            return result;
        }

        public string Asms_DeActivationQueue(SupplierDeActvation objDAqq)
        {
            string result = string.Empty;
            DataSet dsGetRefgid = new DataSet();
            int uResult = 0;
            GetConnection();
            try
            {
                cmd = new SqlCommand("pr_asms_Getrefgids", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Employeegid", Convert.ToString(objCmnFunctions.GetLoginUserGid()));
                cmd.Parameters.AddWithValue("@Ref_Name", "ASMS");
                cmd.Parameters.AddWithValue("@RefTableName", "asms_trn_tsupplierheader");
                da = new SqlDataAdapter(cmd);
                da.Fill(dsGetRefgid);

                if (dsGetRefgid.Tables[0].Rows.Count > 0)
                {
                    //                string[,] aqcodes = new string[,]
                    //               {


                    //    {"queue_date",DateTime.Now.ToString("dd-MMM-yyyy")},
                    //    {"queue_ref_flag",dsGetRefgid.Tables[0].Rows[0]["ref_flag"].ToString()},
                    //    {"queue_ref_gid",objDAqq.DSupplierheadergid},
                    //     {"queue_ref_status","1"},
                    //      {"queue_from",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                    //      {"queue_to_type","I"},
                    //      {"queue_to",dsGetRefgid.Tables[1].Rows[0]["employee_supervisor"].ToString()},
                    //       {"queue_action_for","D"},
                    //      // {"queue_action_date",DateTime.Now.ToString("dd-MMM-yyyy")},
                    //     //  {"queue_action_by","N"},
                    //    //   {"queue_action_status","N"},
                    //      // {"queue_action_remark","N"},
                    //      // {"queue_prev_gid","N"},
                    //       {"queue_isremoved","N"}

                    //              };
                    //                string aqtname = "iem_trn_tqueue";
                    //                result = comm.InsertCommon(aqcodes, aqtname);


                    //                if (result == "success")
                    //                {
                    string upqdcomm = string.Empty;

                    string[,] uTqcodes = new string[,]
    {
         {"supplierheader_requesttype", "DEACTIVATION"},
         {"supplierheader_requeststatus", "WAITINGFORAPPROVAL"},
         {"supplierheader_update_by", Convert.ToString(objCmnFunctions.GetLoginUserGid())},
       //  {"supplierheader_update_date", DateTime.Now.ToString("dd-MMM-yyyy")}
         {"supplierheader_update_date", "sysdatetime()"}
         //,
    //     {"supplierheader_currentapprovalstage", "1"}
        
           
    };
                    string[,] Tqwhrcos = new string[,]
    {
       //  {"supplierheader_gid", objDAqq.DSupplierheadergid},
         {"supplierheader_suppliercode",objDAqq.DSupplierCode},
          {"supplierheader_isremoved","N"}
            
    };
                    string tblname = "asms_tmp_tsupplierheader";

                    upqdcomm = comm.UpdateCommon(uTqcodes, Tqwhrcos, tblname);
                    if (upqdcomm == "Success")
                    {
                        result = upqdcomm;
                    }

                    //                }pr_asms_Activitydisabled

                    //disable activity and attachment table
                    GetConnection();
                    cmd = new SqlCommand("pr_asms_Activitydisabled", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SHeadergid", objDAqq.DSupplierheadergid);
                    uResult = cmd.ExecuteNonQuery();


                    string[,] codes = new string[,]
                   {
     
        {"supplieractivation_supplierheader_gid",objDAqq.DSupplierheadergid},
        {"supplieractivation_currentstatus","D"},
       // {"supplieractivation_activationfrom",convertoDate(objDAqq.Fromdate.ToString())},
       // {"supplieractivation_activationto",convertoDate(objDAqq.Todate.ToString())},
        {"supplieractivation_activationreason",objDAqq.DeActiveReason},
         {"supplieractivation_insert_by",Convert.ToString(comuid.GetLoginUserGid())},
         // {"supplieractivation_insert_date",DateTime.Now.ToString("yyyy-MMM-dd")},
          {"supplieractivation_insert_date","sysdatetime()"},
           {"supplieractivation_comments",objDAqq.Decomments},
           {"supplieractivation_isremoved","N"}
      
                  };
                    string tname = "asms_tmp_tsupplieractivation";
                    result = comm.InsertCommon(codes, tname);
                }



                #endregion

                #region "SelectActivitygid"
                GetConnection();
                cmd = new SqlCommand("pr_asms_GetActivitygid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Employeegid", Convert.ToString(objCmnFunctions.GetLoginUserGid()));
                int active_gid = Convert.ToInt32(cmd.ExecuteScalar());
                #endregion

                #region "InsertAttachments"
                if (objDAqq.arrDeAttachfile != null)
                {
                    if (objDAqq.arrDeAttachfile.Length > 0)
                    {
                        for (int i = 0; i < objDAqq.arrDeAttachfile.Length; i++)
                        {
                            string[,] Acodes = new string[,]
                   {
     
      
        {"saattachment_supplieractivation_gid",active_gid.ToString()},
        {"saattachment_attachmentfor","D"},
        {"saattachment_filename",objDAqq.arrDeAttachfile[i].ToString()},
            {"saattachment_insert_by",Convert.ToString(comuid.GetLoginUserGid())},
          //{"saattachment_insert_date",DateTime.Now.ToString("yyyy-MMM-dd")},
          {"saattachment_insert_date","sysdatetime()"},
          //{"saattachment_update_by",Convert.ToString(comuid.GetLoginUserGid())},
          //{"saattachment_update_date",DateTime.Now.ToString("yyyy-MMM-dd")},
           {"saattachment_isremoved","N"}
      
                  };
                            string Atname = "asms_tmp_tsaattachment";
                            result = comm.InsertCommon(Acodes, Atname);
                        }
                    }
                }



            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                result = ex.Message.ToString();
            }
            return result;
        }

        public List<SupplierActivation> GetSupHeaderDetailsDashboard(string requesttype, string requeststatus)
        {
            List<SupplierActivation> objSupHeaderDetails = new List<SupplierActivation>();
            try
            {
               
                SupplierActivation objModel;
                GetConnection();
                DataTable dt = new DataTable();
               
                cmd = new SqlCommand("pr_asms_GetDashboardDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(objCmnFunctions.GetLoginUserGid());

                if (requeststatus.ToUpper() == "WAITINGFORAPPROVAL")
                {
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getapproverdashboarddetails";
                }
                else
                {
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getraiserdashboarddetails";
                }
                if (requeststatus.ToUpper() == "INPROCESS")
                {
                    requeststatus = "WAITINGFORAPPROVAL";
                }
                cmd.Parameters.Add("@requesttype", SqlDbType.VarChar).Value = requesttype.ToUpper();
                cmd.Parameters.Add("@requeststatus", SqlDbType.VarChar).Value = requeststatus.ToUpper();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierActivation();
                    objModel.Supplierheadergid = dt.Rows[i]["supplierheader_gid"].ToString();
                    objModel.SupplierCode = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_suppliercode"].ToString()) ? "" : dt.Rows[i]["supplierheader_suppliercode"].ToString());
                    objModel.SupplierName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_name"].ToString()) ? "" : dt.Rows[i]["supplierheader_name"].ToString());
                    objModel.SupplierClassificationName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["suppliercategory_name"].ToString()) ? "" : dt.Rows[i]["suppliercategory_name"].ToString());
                    //objModel._CompanyRegNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_companyregno"].ToString()) ? "" : dt.Rows[i]["supplierheader_companyregno"].ToString());
                    //objModel._OwnerCode = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["employee_code"].ToString()) ? "" : dt.Rows[i]["employee_code"].ToString());
                    //objModel._OwnerName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["employee_name"].ToString()) ? "" : dt.Rows[i]["employee_name"].ToString());
                    //objModel._ContractFrom = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_contractfrom"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["supplierheader_contractfrom"].ToString()));
                    //objModel._ContractTo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_contractto"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["supplierheader_contractto"].ToString()));
                    //objModel._RenewalDate = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_renewaldate"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["supplierheader_renewaldate"].ToString()));
                    objModel.RequestTypeName = dt.Rows[i]["supplierheader_requesttype"].ToString();
                    objModel.RequestStatusName = dt.Rows[i]["supplierheader_requeststatus"].ToString();
                    objModel.SupplierStatusName = dt.Rows[i]["supplierheader_status"].ToString();
                    objSupHeaderDetails.Add(objModel);
                }
              
                // return objSupHeaderDetails;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return objSupHeaderDetails;
        }



        public List<SupplierDeActvation> GetSupHeaderDeDetailsDashboard(string requesttype, string requeststatus)
        {
            List<SupplierDeActvation> objSupHeaderDetails = new List<SupplierDeActvation>();
            try
            {
             
                SupplierDeActvation objModel;
                GetConnection();
                DataTable dt = new DataTable();
               
                cmd = new SqlCommand("pr_asms_GetDashboardDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(objCmnFunctions.GetLoginUserGid());

                if (requeststatus.ToUpper() == "WAITINGFORAPPROVAL")
                {
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getapproverdashboarddetails";
                }
                else
                {
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getraiserdashboarddetails";
                }
                if (requeststatus.ToUpper() == "INPROCESS")
                {
                    requeststatus = "WAITINGFORAPPROVAL";
                }
                cmd.Parameters.Add("@requesttype", SqlDbType.VarChar).Value = requesttype.ToUpper();
                cmd.Parameters.Add("@requeststatus", SqlDbType.VarChar).Value = requeststatus.ToUpper();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierDeActvation();
                    objModel.DSupplierheadergid = dt.Rows[i]["supplierheader_gid"].ToString();
                    objModel.DSupplierCode = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_suppliercode"].ToString()) ? "" : dt.Rows[i]["supplierheader_suppliercode"].ToString());
                    objModel.DSupplierName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_name"].ToString()) ? "" : dt.Rows[i]["supplierheader_name"].ToString());
                    objModel.DSupplierClassificationName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["suppliercategory_name"].ToString()) ? "" : dt.Rows[i]["suppliercategory_name"].ToString());
                    //objModel._CompanyRegNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_companyregno"].ToString()) ? "" : dt.Rows[i]["supplierheader_companyregno"].ToString());
                    //objModel._OwnerCode = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["employee_code"].ToString()) ? "" : dt.Rows[i]["employee_code"].ToString());
                    //objModel._OwnerName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["employee_name"].ToString()) ? "" : dt.Rows[i]["employee_name"].ToString());
                    //objModel._ContractFrom = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_contractfrom"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["supplierheader_contractfrom"].ToString()));
                    //objModel._ContractTo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_contractto"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["supplierheader_contractto"].ToString()));
                    //objModel._RenewalDate = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_renewaldate"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["supplierheader_renewaldate"].ToString()));
                    objModel.DRequestTypeName = dt.Rows[i]["supplierheader_requesttype"].ToString();
                    objModel.DRequestStatusName = dt.Rows[i]["supplierheader_requeststatus"].ToString();
                    objModel.DSupplierStatusName = dt.Rows[i]["supplierheader_status"].ToString();
                    objSupHeaderDetails.Add(objModel);
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
            return objSupHeaderDetails;
        }

        public List<SupplierActivation> GetSuppliercatogory(string value)
        {
            DataTable dtgetsc = new DataTable();
            List<SupplierActivation> losc = new List<SupplierActivation>();
            SupplierActivation objs = new SupplierActivation();
            try
            {
                cmd = new SqlCommand("pr_asms_mst_SupplierCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (value != string.Empty)
                {
                    cmd.Parameters.AddWithValue("@action", "Queueone");
                    cmd.Parameters.AddWithValue("@id", value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@action", "Queue");
                }

                GetConnection();
                da = new SqlDataAdapter(cmd);
                da.Fill(dtgetsc);
                if (dtgetsc.Rows.Count > 0)
                {
                    for (int j = 0; j < dtgetsc.Rows.Count; j++)
                    {
                        objs = new SupplierActivation();
                        objs.SupplierClassificationID = Convert.ToInt32(dtgetsc.Rows[j]["SupplierCategorygid"].ToString());
                        objs.SupplierClassificationName = dtgetsc.Rows[j]["SupplierCategoryName"].ToString();
                        losc.Add(objs);
                    }
                    //Session["category"] = losc;
                    //objActi.SupplierClassification = new SelectList(losc, "SupplierClassificationID", "SupplierClassificationName");
                    //objActi.SupplierActiveList = laobj.ToList();
                    //ViewBag.status = "yes";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                dtgetsc = null;
            }
            return losc;
        }



        public List<SupplierDeActvation> GetDeSuppliercatogory(string value)
        {
            DataTable dtgetsc = new DataTable();
            List<SupplierDeActvation> losc = new List<SupplierDeActvation>();
            SupplierDeActvation objs = new SupplierDeActvation();
            try
            {
                cmd = new SqlCommand("pr_asms_mst_SupplierCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (value != string.Empty)
                {
                    cmd.Parameters.AddWithValue("@action", "Queueone");
                    cmd.Parameters.AddWithValue("@id", value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@action", "Queue");
                }

                GetConnection();
                da = new SqlDataAdapter(cmd);
                da.Fill(dtgetsc);
                if (dtgetsc.Rows.Count > 0)
                {
                    for (int j = 0; j < dtgetsc.Rows.Count; j++)
                    {
                        objs = new SupplierDeActvation();
                        objs.DSupplierClassificationID = Convert.ToInt32(dtgetsc.Rows[j]["SupplierCategorygid"].ToString());
                        objs.DSupplierClassificationName = dtgetsc.Rows[j]["SupplierCategoryName"].ToString();
                        losc.Add(objs);
                    }
                    //Session["category"] = losc;
                    //objActi.SupplierClassification = new SelectList(losc, "SupplierClassificationID", "SupplierClassificationName");
                    //objActi.SupplierActiveList = laobj.ToList();
                    //ViewBag.status = "yes";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                dtgetsc = null;
            }
            return losc;
        }

        public List<SupplierActivation> GetEmployeelist()
        {
            DataTable dtemp = new DataTable();
            SupplierActivation objA = new SupplierActivation();
            List<SupplierActivation> objemp = new List<SupplierActivation>();
            try
            {
                cmd = new SqlCommand("pr_asms_mst_SupplierCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "QueueApproval");
                da = new SqlDataAdapter(cmd);
                da.Fill(dtemp);
                if (dtemp.Rows.Count > 0)
                {
                    for (int i = 0; i < dtemp.Rows.Count; i++)
                    {
                        objA = new SupplierActivation();
                        objA.Approverid = Convert.ToInt32(dtemp.Rows[i]["employee_gid"].ToString());
                        objA.ApproverName = dtemp.Rows[i]["employee_name"].ToString();
                        objemp.Add(objA);
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                objemp = null;
            }
            return objemp;
        }


        public List<SupplierDeActvation> GetDeEmployeelist()
        {
            DataTable dtemp = new DataTable();
            SupplierDeActvation objA = new SupplierDeActvation();
            List<SupplierDeActvation> objemp = new List<SupplierDeActvation>();
            try
            {
                cmd = new SqlCommand("pr_asms_mst_SupplierCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "QueueApproval");
                da = new SqlDataAdapter(cmd);
                da.Fill(dtemp);
                if (dtemp.Rows.Count > 0)
                {
                    for (int i = 0; i < dtemp.Rows.Count; i++)
                    {
                        objA = new SupplierDeActvation();
                        objA.DApproverid = Convert.ToInt32(dtemp.Rows[i]["employee_gid"].ToString());
                        objA.DApproverName = dtemp.Rows[i]["employee_name"].ToString();
                        objemp.Add(objA);
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                objemp = null;
            }
            return objemp;
        }
                #endregion



        public List<SupervisoryApproval> GetNextApprovalStage(string reqtype = null, int curlevel = 0)
        {
            List<SupervisoryApproval> objApproval = new List<SupervisoryApproval>();
            try
            {
                
                SupervisoryApproval objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetSupervisor", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@currentapprovalstage", SqlDbType.Int).Value = Convert.ToInt32(curlevel);
                cmd.Parameters.Add("@requesttype", SqlDbType.VarChar).Value = Convert.ToString(reqtype).ToUpper();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getnextapprovalstage";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupervisoryApproval();
                    objModel.Sup_ApprovalGroup = Convert.ToString(dt.Rows[i]["approvalstage_name"].ToString());
                    objModel.Sup_Currenlevel = Convert.ToInt32(dt.Rows[i]["approvalstage_level"].ToString());
                    objModel.QueueToType = Convert.ToString(dt.Rows[i]["approvalstage_type"].ToString());
                    objModel.ApprovalMandatory = Convert.ToString(dt.Rows[i]["approvalstage_ismandatory"].ToString());
                    objApproval.Add(objModel);
                }

              
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return objApproval;
        }
        public int SubmitToNextQueue(string queueto = null, string requesttype = null, string remarks = null, int actionstatus = 1, int skipflag = 0)
        {
            int Result = 0;
            try
            {
                GetConnection();
                // cmd = new SqlCommand("pr_asms_SubmitApproval", con);
                // cmd.CommandType = CommandType.StoredProcedure;
                // cmd.Parameters.Add("@refgid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                // cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                //// cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToString(objCmnFunctions.GetLoginUserGid());
                // if (queueto != null || requesttype != null && remarks != null)
                // {
                //     cmd.Parameters.Add("@requesttype", SqlDbType.VarChar).Value = Convert.ToString(requesttype.ToUpper());
                //     cmd.Parameters.Add("@actionremark", SqlDbType.VarChar).Value = Convert.ToString(remarks);
                //     cmd.Parameters.Add("@queueto", SqlDbType.VarChar).Value = Convert.ToString(queueto);
                // }
                // cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "submittonextapproval";
                cmd = new SqlCommand("pr_asms_SubmitApproval", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@refgid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["DSupplierHeaderGid"]);
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@requesttype", SqlDbType.VarChar).Value = Convert.ToString(requesttype.ToUpper());
                cmd.Parameters.Add("@actionremark", SqlDbType.VarChar).Value = Convert.ToString(remarks);
                cmd.Parameters.Add("@queueto", SqlDbType.VarChar).Value = Convert.ToString(queueto);
                cmd.Parameters.Add("@actionstatus", SqlDbType.Int).Value = actionstatus;
                cmd.Parameters.Add("@skipflag", SqlDbType.Int).Value = skipflag;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "submittonextapproval";
                //  cmd.ExecuteNonQuery();
                Result = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            { 
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Result;
        }


        public int ASubmitToNextQueue(string queueto = null, string requesttype = null, string remarks = null, int actionstatus = 1, int skipflag = 0)
        {
            int Result = 0;
            try
            {
                GetConnection();
                // cmd = new SqlCommand("pr_asms_SubmitApproval", con);
                // cmd.CommandType = CommandType.StoredProcedure;
                // cmd.Parameters.Add("@refgid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                // cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                //// cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToString(objCmnFunctions.GetLoginUserGid());
                // if (queueto != null || requesttype != null && remarks != null)
                // {
                //     cmd.Parameters.Add("@requesttype", SqlDbType.VarChar).Value = Convert.ToString(requesttype.ToUpper());
                //     cmd.Parameters.Add("@actionremark", SqlDbType.VarChar).Value = Convert.ToString(remarks);
                //     cmd.Parameters.Add("@queueto", SqlDbType.VarChar).Value = Convert.ToString(queueto);
                // }
                // cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "submittonextapproval";
                cmd = new SqlCommand("pr_asms_SubmitApproval", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@refgid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@requesttype", SqlDbType.VarChar).Value = Convert.ToString(requesttype.ToUpper());
                cmd.Parameters.Add("@actionremark", SqlDbType.VarChar).Value = Convert.ToString(remarks);
                cmd.Parameters.Add("@queueto", SqlDbType.VarChar).Value = Convert.ToString(queueto);
                cmd.Parameters.Add("@actionstatus", SqlDbType.Int).Value = actionstatus;
                cmd.Parameters.Add("@skipflag", SqlDbType.Int).Value = skipflag;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "submittonextapproval";
                //  cmd.ExecuteNonQuery();
                Result = cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return Result;
        }
        public DataTable Getempid(string empid)
        {
            DataTable dt = new DataTable();
            int result = 0;
            try
            {
                GetConnection();
                string lsQry = "select employee_gid,employee_name,employee_code,employee_dept_name,employee_gender ";
                lsQry += "from iem_mst_temployee where employee_code='" + empid + "' ";
                lsQry += "and employee_isremoved='N'";
                cmd = new SqlCommand(lsQry, con);
                SqlDataAdapter sa = new SqlDataAdapter(cmd);
                sa.Fill(dt);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                dt = null;
            }

            // result =(int) cmd.ExecuteScalar();
            //  result = Convert.ToInt32(dt.Rows[0]["employee_gid"].ToString());
            return dt;
        }

        public List<SupplierActivation> Sup_MoficationSummery()
        {
            GetConnection();
            List<SupplierActivation> objaa = new List<SupplierActivation>();
            DataTable dtheader = new DataTable();
            string clname = string.Empty;
            string old = string.Empty;
            string vnew = string.Empty;
            string[] query = null;
            string queryheader = "asms_trn_tvat/asms_tmp_tvat/vat_gid,vat_city,vat_rate,vat_update_date/vat_gid,vat_city,vat_rate,vat_update_date/1/vat_gid";
            query = queryheader.Split('/');
            cmd = new SqlCommand("pr_asms_CompareTables", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@table1", query[0].ToString());
            cmd.Parameters.AddWithValue("@table2", query[1].ToString());
            cmd.Parameters.AddWithValue("@T1ColumnList", query[2].ToString());
            cmd.Parameters.AddWithValue("@T2ColumnList", query[3].ToString());
            cmd.Parameters.AddWithValue("@Tblwherev", query[4].ToString());
            cmd.Parameters.AddWithValue("@Tblwherec", query[5].ToString());
            da = new SqlDataAdapter(cmd);
            da.Fill(dtheader);
            if (dtheader.Rows.Count > 0)
            {

                for (int i = 0; i < dtheader.Columns.Count; i++)
                {
                    if (dtheader.Rows[0][i].ToString() != dtheader.Rows[1][i].ToString())
                    {
                        clname = dtheader.Columns[i].ColumnName;
                        old = dtheader.Rows[0][i].ToString();
                        vnew = dtheader.Rows[1][i].ToString();
                    }
                }
            }
            return objaa;
        }
        public List<SupplierActivation> GetEmployeeListForApproval(string id)
        {
            List<SupplierActivation> objSearchEmployee = new List<SupplierActivation>();
            try
            {
                SupplierActivation objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt32(id);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "employeelistforapproval";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierActivation();
                    objModel.Approverid = Convert.ToInt32(dt.Rows[i]["employee_gid"].ToString());
                    objModel.Approver = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["employee_code"].ToString()) ? "" : dt.Rows[i]["employee_code"].ToString());
                    objModel.ApproverName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["employee_name"].ToString()) ? "" : dt.Rows[i]["employee_name"].ToString());
                    objSearchEmployee.Add(objModel);
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
            return objSearchEmployee;
        }


        public List<SupplierDeActvation> GetEmployeeDeListForApproval(string id)
        {
            List<SupplierDeActvation> objSearchEmployee = new List<SupplierDeActvation>();
            try
            {
             
                SupplierDeActvation objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt32(id);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "employeelistforapproval";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierDeActvation();
                    objModel.DApproverid = Convert.ToInt32(dt.Rows[i]["employee_gid"].ToString());
                    objModel.DApprovar = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["employee_code"].ToString()) ? "" : dt.Rows[i]["employee_code"].ToString());
                    objModel.DApproverName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["employee_name"].ToString()) ? "" : dt.Rows[i]["employee_name"].ToString());
                    objSearchEmployee.Add(objModel);
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
            return objSearchEmployee;
        }

        public DataSet GetpreSupplierDetails(string id, string a)
        {
            DataSet dspstatus = new DataSet();
            GetConnection();
            try
            {
                cmd = new SqlCommand("pr_asms_Getpresupplier", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Refgid", id);
                cmd.Parameters.AddWithValue("@Type", a);
                da = new SqlDataAdapter(cmd);
                da.Fill(dspstatus);
              
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return dspstatus;
        }

        public List<SupplierDeActvation> GetDSupplierApprovalHistory(string id, string action)
        {
            List<SupplierDeActvation> objh = new List<SupplierDeActvation>();
            DataSet dsGethistory = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_GetDashboardDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", id);
                cmd.Parameters.Add("@action", "getrequesthistory");
                da = new SqlDataAdapter(cmd);
                da.Fill(dsGethistory);
                if (dsGethistory.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsGethistory.Tables[0].Rows.Count; i++)
                    {
                        SupplierDeActvation obj = new SupplierDeActvation();
                        obj.DSupplierCode = dsGethistory.Tables[0].Rows[i]["supplierheader_suppliercode"].ToString();
                        obj.DSupplierName = dsGethistory.Tables[0].Rows[i]["supplierheader_name"].ToString();
                        obj.DApproverName = dsGethistory.Tables[0].Rows[i]["approvalstage_name"].ToString();
                        obj.Dreg_date = dsGethistory.Tables[0].Rows[i]["requesthistory_requestdate"].ToString();
                        obj.DRequestTypeName = dsGethistory.Tables[0].Rows[i]["requesthistory_requesttype"].ToString();
                        obj.DRequestStatusName = dsGethistory.Tables[0].Rows[i]["requesthistory_requeststatus"].ToString();
                        obj.Demployee = dsGethistory.Tables[0].Rows[i]["employee"].ToString();
                        objh.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return objh;
        }

        public List<SupplierActivation> GetSupplierApprovalHistory(string id, string action)
        {
            List<SupplierActivation> objh = new List<SupplierActivation>();
            DataSet dsGethistory = new DataSet();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_GetDashboardDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", id);
                cmd.Parameters.Add("@action", "getrequesthistory");
                da = new SqlDataAdapter(cmd);
                da.Fill(dsGethistory);
                if (dsGethistory.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsGethistory.Tables[0].Rows.Count; i++)
                    {
                        SupplierActivation obj = new SupplierActivation();
                        obj.SupplierCode = dsGethistory.Tables[0].Rows[i]["supplierheader_suppliercode"].ToString();
                        obj.SupplierName = dsGethistory.Tables[0].Rows[i]["supplierheader_name"].ToString();
                        obj.ApproverName = dsGethistory.Tables[0].Rows[i]["approvalstage_name"].ToString();
                        obj.reg_date = dsGethistory.Tables[0].Rows[i]["requesthistory_requestdate"].ToString();
                        obj.RequestTypeName = dsGethistory.Tables[0].Rows[i]["requesthistory_requesttype"].ToString();
                        obj.RequestStatusName = dsGethistory.Tables[0].Rows[i]["requesthistory_requeststatus"].ToString();
                        obj.employee = dsGethistory.Tables[0].Rows[i]["employee"].ToString();
                        objh.Add(obj);
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return objh;
        }

        public List<SupplierQuery> GetSupplierquery(string SupplierCode, string SupplierName, string SupplierStatus, string ContractFrom, string ContractTo, string Type
            , string categoryname
            , string organizationtypename
            , string servicetypename
            , string employeecode
            , string employeename
             ,string requeststatus
            ,string requesttype
            ,string department
            ,string actionfrom
            ,string actionto)
        {
            List<SupplierQuery> objq = new List<SupplierQuery>();
            SupplierQuery objqa = new SupplierQuery();
            DataTable dtgetq = new DataTable();
            // DataSet dtgetq = new DataSet();
            try
            {

                string socde = string.IsNullOrEmpty(SupplierCode) ? string.Empty : SupplierCode;
                string sname = string.IsNullOrEmpty(SupplierName) ? string.Empty : SupplierName;
                string cnfdate = string.IsNullOrEmpty(employeecode) ? string.Empty : employeecode;
                string cntdate = string.IsNullOrEmpty(employeename) ? string.Empty : employeename;

                string cservicetypename = "";
                string corganizationtypename = "";
                string csservicetypename = "";
                string csSupplierStatus = "";
                string rrequesttype = string.Empty;
                string rdepartment = string.Empty;
                string Acfdate = string.Empty;
                string AcTdate = string.Empty;
                if (categoryname == "0")
                {
                    categoryname = string.Empty;
                }
                if (organizationtypename == "0")
                {
                    organizationtypename = string.Empty;
                }
                if (servicetypename == "0")
                {
                    servicetypename = string.Empty;
                }
                if (SupplierStatus == "0")
                //    if (SupplierStatus == "0" || SupplierStatus == "INACTIVE")
                {
                    csSupplierStatus = string.Empty;
                }
                else
                {
                    csSupplierStatus = SupplierStatus;
                }
                if (requeststatus=="0")
                {
                    requeststatus = string.Empty;
                }

                rrequesttype = string.IsNullOrEmpty(requesttype) ? string.Empty : requesttype;
                rdepartment = string.IsNullOrEmpty(department) ? string.Empty : department;
                Acfdate = string.IsNullOrEmpty(actionfrom) ? string.Empty : convertoDate(actionfrom);
                AcTdate = string.IsNullOrEmpty(actionto) ? string.Empty : convertoDate(actionto);


                cservicetypename = string.IsNullOrEmpty(categoryname) ? string.Empty : categoryname;
                corganizationtypename = string.IsNullOrEmpty(organizationtypename) ? string.Empty : organizationtypename;
                csservicetypename = string.IsNullOrEmpty(servicetypename) ? string.Empty : servicetypename;

                GetConnection();
                cmd = new SqlCommand("pr_asms_GetSupplierquery", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SupplierCode", socde);
                //cmd.Parameters.AddWithValue("@SupplierCode", SupplierCode);
                cmd.Parameters.AddWithValue("@SupplierName", sname);
                cmd.Parameters.AddWithValue("@SupplierStatus", csSupplierStatus);
                cmd.Parameters.AddWithValue("@Category", cservicetypename);
                cmd.Parameters.AddWithValue("@Organizationtype", corganizationtypename);
                cmd.Parameters.AddWithValue("@Services", csservicetypename);
                cmd.Parameters.AddWithValue("@OwnerId", cnfdate);
                cmd.Parameters.AddWithValue("@Ownername", cntdate);

                cmd.Parameters.AddWithValue("@RequestStatus", requeststatus);
                cmd.Parameters.AddWithValue("@RequestType", rrequesttype);
                cmd.Parameters.AddWithValue("@Department", rdepartment);
                cmd.Parameters.AddWithValue("@ActionFromDate", Acfdate);
                cmd.Parameters.AddWithValue("@ActionToDate", AcTdate);

                if (cservicetypename == string.Empty && corganizationtypename == string.Empty && csservicetypename == string.Empty && socde == string.Empty && sname == string.Empty && SupplierStatus == string.Empty && cnfdate == string.Empty && cntdate == string.Empty && requeststatus == string.Empty && rrequesttype == string.Empty && rdepartment == string.Empty && Acfdate == string.Empty && AcTdate==string.Empty)
                {
                    cmd.Parameters.AddWithValue("@Type", "full");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@Type", "Search");
                }

                da = new SqlDataAdapter(cmd);
                da.Fill(dtgetq);
                if (dtgetq.Rows.Count > 0)
                {
                    HttpContext.Current.Session["exceldownload"] = dtgetq;
                    for (int i = 0; i < dtgetq.Rows.Count; i++)
                    {
                        objqa = new SupplierQuery();
                        objqa.qSupplierheadGid = dtgetq.Rows[i]["SupplierHeadergid"].ToString();
                        objqa.qSupplierCode = dtgetq.Rows[i]["SupplierCode"].ToString();
                        objqa.qSupplierName = dtgetq.Rows[i]["SupplierName"].ToString();
                        objqa.qSupplierStatus = dtgetq.Rows[i]["CurrentStatus"].ToString();
                        objqa.qCont_startDate = dtgetq.Rows[i]["ContractFrom"].ToString();
                        objqa.qCont_EndDateNew = dtgetq.Rows[i]["ContractTo"].ToString();

                        objqa.categoryname = dtgetq.Rows[i]["CategoryName"].ToString();
                        objqa.organizationtypename = dtgetq.Rows[i]["OrganizationType"].ToString();
                        objqa.servicetypename = dtgetq.Rows[i]["ServicetypeName"].ToString();
                        objqa.employeecode = dtgetq.Rows[i]["OwnerCode"].ToString();
                        objqa.employeename = dtgetq.Rows[i]["OwnerName"].ToString();

                        objqa.qDepartmentname = dtgetq.Rows[i]["Departmentname"].ToString();
                        objqa.qRequeststatus = dtgetq.Rows[i]["RequestStatus"].ToString();
                        objqa.qRequestTypeName = dtgetq.Rows[i]["RequestType"].ToString();

                        objq.Add(objqa);
                    }
                }
                else
                {
                    HttpContext.Current.Session["exceldownload"] = dtgetq;
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return objq;
        }
        public IEnumerable<supasmscategory> Getcategory()
        {
            List<supasmscategory> objcategory = new List<supasmscategory>();
            supasmscategory objModel;
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("select suppliercategory_gid,suppliercategory_name from asms_mst_tsuppliercategory where suppliercategory_isremoved='N'", con);
                cmd.CommandType = CommandType.Text;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objcategory.Add(new supasmscategory { categoryId = 0, categoryName = "-- Select --", });
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new supasmscategory();
                    objModel.categoryId = Convert.ToInt32(dt.Rows[i]["suppliercategory_gid"].ToString());
                    objModel.categoryName = Convert.ToString(dt.Rows[i]["suppliercategory_name"].ToString());
                    objcategory.Add(objModel);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
           
            return objcategory;
        }
        public IEnumerable<supasmsorganizationtype> Getorganizationtype()
        {
            List<supasmsorganizationtype> objcategory = new List<supasmsorganizationtype>();
            supasmsorganizationtype objModel;
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("select organizationtype_gid,organizationtype_name FROM asms_mst_torganizationtype  where organizationtype_isremoved='N'", con);
                cmd.CommandType = CommandType.Text;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objcategory.Add(new supasmsorganizationtype { organizationtypeId = 0, organizationtypeName = "-- Select --", });
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new supasmsorganizationtype();
                    objModel.organizationtypeId = Convert.ToInt32(dt.Rows[i]["organizationtype_gid"].ToString());
                    objModel.organizationtypeName = Convert.ToString(dt.Rows[i]["organizationtype_name"].ToString());
                    objcategory.Add(objModel);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
           
            return objcategory;
        }
        public IEnumerable<supasmsservicetype> Getservicetypey()
        {
            List<supasmsservicetype> objcategory = new List<supasmsservicetype>();
            supasmsservicetype objModel;
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("select servicetype_gid,servicetype_name FROM asms_mst_tservicetype  where servicetype_isremoved='N'", con);
                cmd.CommandType = CommandType.Text;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objcategory.Add(new supasmsservicetype { servicetypeId = 0, servicetypeName = "-- Select --", });
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new supasmsservicetype();
                    objModel.servicetypeId = Convert.ToInt32(dt.Rows[i]["servicetype_gid"].ToString());
                    objModel.servicetypeName = Convert.ToString(dt.Rows[i]["servicetype_name"].ToString());
                    objcategory.Add(objModel);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
           
            return objcategory;
        }
        public IEnumerable<supasmsStutas> GetStatussMode()
        {
            List<supasmsStutas> objparenttax = new List<supasmsStutas>();
           
            supasmsStutas objModel;
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("select supplierstatus_gid,supplierstatus_name from asms_mst_tsupplierstatus where supplierstatus_isremoved='N'  and supplierstatus_statusflow is not null", con);
                cmd.CommandType = CommandType.Text;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objparenttax.Add(new supasmsStutas { StutasId = "0", StutasName = "-- Select --", });
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new supasmsStutas();
                    objModel.StutasId = Convert.ToString(dt.Rows[i]["supplierstatus_name"].ToString());
                    objModel.StutasName = Convert.ToString(dt.Rows[i]["supplierstatus_name"].ToString());
                    objparenttax.Add(objModel);
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            
            return objparenttax;
        }
        public int GetSuppliergid(string SupplierCode)
        {
            int s_gid = 0;
            try
            {
                cmd = new SqlCommand("pr_asms_GetSupplierGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SupplierCode", SupplierCode);
                s_gid = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return s_gid;
        }

        public List<SupplierQuery> GetemployeeDepartment()
        {
            List<SupplierQuery> objdpt = new List<SupplierQuery>();
            DataTable dtdept = new DataTable();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_getemp_deptname", con);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dtdept);
                if (dtdept.Rows.Count > 0)
                {
                    for (int i = 0; dtdept.Rows.Count > 0; i++)
                    {
                        SupplierQuery objsq = new SupplierQuery();
                        objsq.qDepartmentid = dtdept.Rows[i]["department"].ToString();
                        objsq.qDepartmentname = dtdept.Rows[i]["department"].ToString();
                        objdpt.Add(objsq);
                    }

                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return objdpt;
        }

        public List<SupplierQuery> GetRequestType()
        {
            List<SupplierQuery> objtype = new List<SupplierQuery>();
            DataTable dttype = new DataTable();
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_GetRequestType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                da = new SqlDataAdapter(cmd);
                da.Fill(dttype);
                if (dttype.Rows.Count > 0)
                {
                    for (int j = 0; dttype.Rows.Count > 0; j++)
                    {
                        SupplierQuery obtype = new SupplierQuery();
                        obtype.qRequestTypeid = dttype.Rows[j]["RequestType"].ToString();
                        obtype.qRequestTypeName = dttype.Rows[j]["RequestType"].ToString();
                        objtype.Add(obtype);
                    }
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            return objtype;
        }
        public string GetSupIdForMail(int id)
        {
            try
            {
                GetConnection();

                int LOGINID = objCmnFunctions.GetLoginUserGid();
                int lsParameter = 0;

                string lsQry = "";
                lsQry += "SELECT max( queue_gid) AS queue_gid FROM iem_trn_tqueue ";
                lsQry += "INNER JOIN asms_trn_tsupplierheader ON asms_trn_tsupplierheader.supplierheader_gid=iem_trn_tqueue.queue_ref_gid";
                lsQry += " WHERE  queue_ref_gid='" + id.ToString() + "'  and  queue_ref_status=0 and queue_from='" + LOGINID.ToString() + "'";
                //cmd.Connection = con;
                //cmd.CommandText = lsQry;
                cmd = new SqlCommand(lsQry, con);
                lsParameter = (int)cmd.ExecuteScalar();
                con.Close();
                return lsParameter.ToString();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
           
        }
        public string GetTriggercount(string days)
        {
            string Result = string.Empty;
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_gettriggercount", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Triggerdays", Convert.ToInt32(string.IsNullOrEmpty(days) ? "0" : days));
                Result = Convert.ToString((int)cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                Result = ex.Message;
            }
            return Result;
        }

        public string ReleaseLog(string supplier_gid)
        {
            string releselog = string.Empty;
            try
            {
                string[,] rul = new string[,]
                {
                    {"supplierheader_logedstatus","N"},
                    {"supplierheader_logedby",string.Empty}
                };
                string[,] wrul = new string[,]
                {
                    {"supplierheader_gid",supplier_gid}
                };

                string tblname = "asms_tmp_tsupplierheader";
                releselog = comm.UpdateCommon(rul, wrul, tblname);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                releselog = ex.Message;
            }
            return releselog;
        }
        //public string emplogin(string empcode)
        //{
        //    string empna = "";
        //    string path = string.Empty;
        //    //string connection = "\\192.168.0.165";
        //    //DirectorySearcher dssearch = new DirectorySearcher(connection);
        //    //dssearch.Filter = "(sAMAccountName=" + empcode + ")";
        //    //SearchResult sresult = dssearch.FindOne();
        //    //DirectoryEntry dsresult = sresult.GetDirectoryEntry();

        //    //DataTable dt = new DataTable();
        //    path = "ldap://192.168.0.165";
        //    if (path != "")
        //    {
        //        // path = path + "," + empcode + "," + empname;

        //        DirectoryEntry De;
        //        string id = @"Gnsaindia\" + empcode;
        //        De = new DirectoryEntry(path, id, empcode);

        //        try
        //        {

        //            DirectorySearcher Ds = new DirectorySearcher(path);
        //            Ds.Filter = "(username=" + id + ")";
        //            SearchResultCollection sr = Ds.FindAll();
        //            //Ds.FindOne();
        //            empna = "success";
        //        }
        //        catch
        //        {
        //            empna = "Failed";
        //        }

        //        //DirectoryEntry De;

        //        //De = new DirectoryEntry(path, empcode, empname, AuthenticationTypes.Secure);

        //        //DirectorySearcher Ds = new DirectorySearcher(De);

        //        //Ds.FindOne();

        //        //if (DirectoryEntry.Exists(path))
        //        //{
        //        //    DirectorySearcher Ds = new DirectorySearcher(De);
        //        //    // Dim Ser As SearchResultCollection = Ds.FindAll()
        //        //    Ds.FindOne();

        //        //    empna = "success";



        //        //    //return true;
        //        //}
        //        //else
        //        //{
        //        //    empna = "Failed";


        //        //    //return false;
        //        //}


        //    }

        //    return empna;
        //}




        public DataSet GetpreSupplierDetails(string id)
        {
            throw new NotImplementedException();
        }

        //public DataTable getproxydetails(int deta)
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        IList<GetProxy> mmList = new List<GetProxy>();
        //        GetProxy objModel;
        //        GetConnection();
              
        //        cmd = new SqlCommand("pr_iem_Proxy", con);
        //        cmd.Parameters.AddWithValue("@action", "GetProxy");
        //        cmd.Parameters.AddWithValue("@gid", deta);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return dt;
        //}


        public DataTable getproxydetails(int deta)
        {
            throw new NotImplementedException();
        }
    }


}