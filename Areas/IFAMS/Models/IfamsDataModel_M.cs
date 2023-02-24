using IEM.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.IO;
namespace IEM.Areas.IFAMS.Models
{
    public class IfamsDataModel_M : IfamsRepository_M
    {
        SqlConnection con = new SqlConnection();
        SqlTransaction tran = null;
        DataModel objidm = new DataModel();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        CommonIUD objCommonIUD = new CommonIUD();
        ErrorLog objErrorLog = new ErrorLog();
        string[,] result;
        private void GetConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }

        public string getTheUser(string groupCode)
        {
            string success = "";
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
                return "";
            }
            finally
            {
                con.Close();
            }
        }

        public IEnumerable<SaleMakerModel> GetSoaDetailDraft(string id)  //Get Draft Data
        {
            List<SaleMakerModel> objModel = new List<SaleMakerModel>();
            try
            {

                SaleMakerModel obj = new SaleMakerModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "draft");
                cmd.Parameters.AddWithValue("@assetid", id);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                // for(int i=0; i < dt.Rows.Count; i++)
                foreach (DataRow row in dt.Rows)
                {
                    obj = new SaleMakerModel();
                    DataTable dt1 = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = row["soaheader_sale_status"].ToString();
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "statusname";
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt1);
                    foreach (DataRow row1 in dt1.Rows)
                        obj.soaStatus = row1["status_name"].ToString();
                    obj.soaTnorecords = Convert.ToInt32(row["soaheader_no_records"].ToString());
                    obj.soaGid = Convert.ToInt32(row["soaheader_gid"].ToString());
                    obj.soaSalenumber = row["soaheader_sale_number"].ToString();
                    obj.soaSalevalue = Convert.ToDecimal(row["soaheader_sale_value"].ToString());
                    obj.soaSaledate = row["soaheader_sale_date"].ToString();

                    obj.soaUploadfilename = row["soaheader_upld_fname"].ToString();
                    objModel.Add(obj);
                }
                return objModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objModel;
                //throw ex;
            }
            finally
            {
                con.Close();
            }
        }


        //Dashboard Reject click Get Reject Data


        public IEnumerable<SaleMakerModel> GetSoaDetailReject(string id)  //Get Reject Data
        {
            List<SaleMakerModel> objModel = new List<SaleMakerModel>();
            try
            {

                SaleMakerModel obj = new SaleMakerModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "Rejected");
                cmd.Parameters.AddWithValue("@assetid", id);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                // for(int i=0; i < dt.Rows.Count; i++)
                foreach (DataRow row in dt.Rows)
                {
                    obj = new SaleMakerModel();
                    DataTable dt1 = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = row["soaheader_sale_status"].ToString();
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "statusname";
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt1);
                    foreach (DataRow row1 in dt1.Rows)
                        obj.soaStatus = row1["status_name"].ToString();
                    obj.soaTnorecords = Convert.ToInt32(row["soaheader_no_records"].ToString());
                    obj.soaGid = Convert.ToInt32(row["soaheader_gid"].ToString());
                    obj.soaSalenumber = row["soaheader_sale_number"].ToString();
                    obj.soaSalevalue = Convert.ToDecimal(row["soaheader_sale_value"].ToString());
                    obj.soaSaledate = row["soaheader_sale_date"].ToString();

                    obj.soaUploadfilename = row["soaheader_upld_fname"].ToString();
                    objModel.Add(obj);
                }
                return objModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objModel;
                //throw ex;
            }
            finally
            {
                con.Close();
            }
        }



        //Dashboard link inprocess


        public IEnumerable<SaleMakerModel> GetSoaDetailSummary(string id)
        {
            List<SaleMakerModel> objModel = new List<SaleMakerModel>();
            try
            {

                SaleMakerModel obj = new SaleMakerModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_get_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "DashboardInproces");
                cmd.Parameters.AddWithValue("@assetid", id);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                // for(int i=0; i < dt.Rows.Count; i++)
                foreach (DataRow row in dt.Rows)
                {
                    obj = new SaleMakerModel();
                    DataTable dt1 = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = row["soaheader_sale_status"].ToString();
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "statusname";
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt1);
                    foreach (DataRow row1 in dt1.Rows)
                        obj.soaStatus = row1["status_name"].ToString();
                    obj.soaTnorecords = Convert.ToInt32(row["soaheader_no_records"].ToString());
                    obj.soaGid = Convert.ToInt32(row["soaheader_gid"].ToString());
                    obj.soaSalenumber = row["soaheader_sale_number"].ToString();
                    obj.soaSalevalue = Convert.ToDecimal(row["soaheader_sale_value"].ToString());
                    obj.soaSaledate = row["soaheader_sale_date"].ToString();

                    obj.soaUploadfilename = row["soaheader_upld_fname"].ToString();
                    objModel.Add(obj);
                }
                return objModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objModel;
                //throw ex;
            }
            finally
            {
                con.Close();
            }
        }


        public IEnumerable<SaleMakerModel> GetSoaDetail(string id)
        {
            List<SaleMakerModel> objModel = new List<SaleMakerModel>();
            try
            {
                SaleMakerModel obj = new SaleMakerModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "selects");
                cmd.Parameters.AddWithValue("@assetid", id);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                // for(int i=0; i < dt.Rows.Count; i++)
                foreach (DataRow row in dt.Rows)
                {
                    obj = new SaleMakerModel();
                    DataTable dt1 = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = row["soaheader_sale_status"].ToString();
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "statusname";
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt1);
                    foreach (DataRow row1 in dt1.Rows)
                        obj.soaStatus = row1["status_name"].ToString();
                    obj.soaTnorecords = Convert.ToInt32(row["soaheader_no_records"].ToString());
                    obj.soaGid = Convert.ToInt32(row["soaheader_gid"].ToString());
                    obj.soaSalenumber = row["soaheader_sale_number"].ToString();
                    obj.soaSalevalue = Convert.ToDecimal(row["soaheader_sale_value"].ToString());
                    obj.soaSaledate = row["soaheader_sale_date"].ToString();

                    obj.soaUploadfilename = row["soaheader_upld_fname"].ToString();
                    objModel.Add(obj);
                }
                return objModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objModel;
                // throw ex;
            }
            finally
            {
                con.Close();
            }
        }



        // Get Maker Queue
        public IEnumerable<SaleMakerModel> GetSoaDetailMaker(string id)
        {
            List<SaleMakerModel> objModel = new List<SaleMakerModel>();
            try
            {

                SaleMakerModel obj = new SaleMakerModel();
                GetConnection();
                DataTable dt = new DataTable();
                //cmd = new SqlCommand("pr_ifams_get_SaleMaker", con);
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "FAMaker");
                cmd.Parameters.AddWithValue("@assetid", id);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    obj = new SaleMakerModel();
                    DataTable dt1 = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = row["soaheader_sale_status"].ToString();
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "statusname";
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt1);
                    foreach (DataRow row1 in dt1.Rows)
                        obj.soaStatus = row1["status_name"].ToString();
                    obj.soaTnorecords = Convert.ToInt32(row["soaheader_no_records"].ToString());
                    obj.soaGid = Convert.ToInt32(row["soaheader_gid"].ToString());
                    obj.soaSalenumber = row["soaheader_sale_number"].ToString();
                    obj.soaSalevalue = Convert.ToDecimal(row["soaheader_sale_value"].ToString());
                    obj.soaSaledate = row["soaheader_sale_date"].ToString();

                    obj.soaUploadfilename = row["soaheader_upld_fname"].ToString();
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



        public IEnumerable<SaleMakerModel> GetSoaDetailMakerWFAP(string id)
        {
            List<SaleMakerModel> objModel = new List<SaleMakerModel>();
            try
            {

                SaleMakerModel obj = new SaleMakerModel();
                GetConnection();
                DataTable dt = new DataTable();
                //cmd = new SqlCommand("pr_ifams_get_SaleMaker", con);
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "FAMakerwaitingfor");
                cmd.Parameters.AddWithValue("@assetid", id);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    obj = new SaleMakerModel();
                    DataTable dt1 = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = row["soaheader_sale_status"].ToString();
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "statusname";
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt1);
                    foreach (DataRow row1 in dt1.Rows)
                        obj.soaStatus = row1["status_name"].ToString();
                    obj.soaTnorecords = Convert.ToInt32(row["soaheader_no_records"].ToString());
                    obj.soaGid = Convert.ToInt32(row["soaheader_gid"].ToString());
                    obj.soaSalenumber = row["soaheader_sale_number"].ToString();
                    obj.soaSalevalue = Convert.ToDecimal(row["soaheader_sale_value"].ToString());
                    obj.soaSaledate = row["soaheader_sale_date"].ToString();

                    obj.soaUploadfilename = row["soaheader_upld_fname"].ToString();
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



        public IEnumerable<SaleMakerModel> GetSoaDetailCheckerApproved(string id)
        {
            List<SaleMakerModel> objModel = new List<SaleMakerModel>();
            try
            {

                SaleMakerModel obj = new SaleMakerModel();
                GetConnection();
                DataTable dt = new DataTable();
                //cmd = new SqlCommand("pr_ifams_get_SaleMaker", con);
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "FACHKAPPROVED");
                cmd.Parameters.AddWithValue("@assetid", id);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    obj = new SaleMakerModel();
                    DataTable dt1 = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = row["soaheader_sale_status"].ToString();
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "statusname";
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt1);
                    foreach (DataRow row1 in dt1.Rows)
                        obj.soaStatus = row1["status_name"].ToString();
                    obj.soaTnorecords = Convert.ToInt32(row["soaheader_no_records"].ToString());
                    obj.soaGid = Convert.ToInt32(row["soaheader_gid"].ToString());
                    obj.soaSalenumber = row["soaheader_sale_number"].ToString();
                    obj.soaSalevalue = Convert.ToDecimal(row["soaheader_sale_value"].ToString());
                    obj.soaSaledate = row["soaheader_sale_date"].ToString();

                    obj.soaUploadfilename = row["soaheader_upld_fname"].ToString();
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

        public string GetUserType(string id)
        {

            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_get_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Salegid", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@empgid", SqlDbType.VarChar).Value = Convert.ToString(objCmnFunctions.GetLoginUserGid().ToString());
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "UserType";
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

        public IEnumerable<SaleMakerModel> GetSaleDetailSummary(string id)
        {
            List<SaleMakerModel> objModel = new List<SaleMakerModel>();
            try
            {

                SaleMakerModel obj = new SaleMakerModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_Get_formyapproval", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prToaEmployeeGid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@prToaDocType", SqlDbType.VarChar).Value = "SOA";
                cmd.Parameters.Add("@prToaAction", SqlDbType.VarChar).Value = "ForMyApproval";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new SaleMakerModel();
                    obj.soaStatus = GetStatus(row["doc_status_gid"].ToString());
                    obj.soaGid = Convert.ToInt32(row["queue_ref_gid"].ToString());
                    obj.soaSalenumber = row["doc_no"].ToString();

                    DateTime dateformat = Convert.ToDateTime(row["doc_date"].ToString());
                    obj.soaSaledate = dateformat.ToString("dd-MM-yyy");
                    //  obj.soaSaledate = Convert.ToString(row["doc_date"].ToString());
                    obj.soaNorecords = Convert.ToInt32(row["doc_noof_records"].ToString());
                    obj.soaSalevalue = Convert.ToDecimal(row["doc_amount"].ToString());
                    obj.soaUploadfilename = row["doc_file_name"].ToString();

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


        public string GetStatus(string Id)
        {
            string status = string.Empty;
            try
            {
                GetConnection();
                DataTable dt1 = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = Id;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "statusname";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt1);
                if (dt1.Rows.Count > 0)
                {
                    status = dt1.Rows[0]["status_name"].ToString();
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

        public string GetSaleStatusexcel(string assetdata, string valid, string action)
        {
            string result = "";
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_saleasset", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@salechkdata", SqlDbType.VarChar).Value = valid.ToString();
                cmd.Parameters.Add("@saleassetdata", SqlDbType.VarChar).Value = assetdata.ToString();
                cmd.Parameters.Add("@saleresult", SqlDbType.VarChar).Value = action;
                result = (string)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
                // throw ex;
            }
            finally
            {
                con.Close();
            }
            return result;
        }


        public string InsertSale(DataTable assetdata, string filename)
        {
            try
            {
                List<SaleMakerModel> smmList = new List<SaleMakerModel>();
                SaleMakerModel smm;
                string sastatus = "";
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = "DRAFT";
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "status";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    sastatus = row["status_flag"].ToString();
                }
                string soanumber = objCmnFunctions.GetSequenceNoFam("SOA");
                smm = new SaleMakerModel();

                DataTable dtA = new DataTable();
                dtA = (DataTable)assetdata;
                for (int j = 0; j < dtA.Rows.Count; j++)
                {
                    smm.assetdetDetid = dtA.Rows[j]["Asset Id"].ToString().Trim();
                    smm.soaSalevalue = Convert.ToDecimal(dtA.Rows[j]["Cheque Split Amount"].ToString().Trim());

                }


                for (int i = 0; i == 0; i++)
                {

                    DataTable dtAsset = new DataTable();
                    dtAsset = (DataTable)assetdata;
                    for (int j = 0; j < dtAsset.Rows.Count; j++)
                    {
                        smm.assetdetDetid = dtA.Rows[j]["Asset Id"].ToString().Trim();
                        smm.soaSalevalue = Convert.ToDecimal(dtA.Rows[j]["Cheque Split Amount"].ToString().Trim());
                    }
                    smm.soaLocgid = Convert.ToInt32(SALocationdetail(smm.assetdetDetid).ToString());
                    smm.soaCheckerid = SAGetCheckerDetails(objCmnFunctions.GetLoginUserGid());
                    smm.soaUploadfilename = filename.ToString();
                    smm.soaSalenumber = soanumber;
                    string[,] codes = new string[,]
                    {                            
                             {"soaheader_sale_number",smm.soaSalenumber.ToString()},
                             //{"soaheader_sale_value",smm.soaSalevalue.ToString()},
                             {"soaheader_loc_gid" , smm.soaLocgid.ToString()},
                             {"soaheader_maker_id",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                             {"soaheader_maker_date",Convert.ToString("SYSDATETIME()")},
                             //{"soaheader_checker_id", smm.soaCheckerid.ToString()},
                             {"soaheader_upld_date",Convert.ToString(objCommonIUD.GetCurrentDate())},
                             {"soaheader_sale_date",Convert.ToString("SYSDATETIME()")},
                            // {"soaheader_checker_date",Convert.ToString(objCommonIUD.GetCurrentDate())},
                             {"soaheader_upld_fname",smm.soaUploadfilename.ToString()},
                             {"soaheader_isremoved","N"},
                             {"soaheader_no_records",Convert.ToString(assetdata.Rows.Count)},
                             {"soaheader_sale_status", sastatus.ToString()},
                             //{"soaheader_job_code",smm.soaSalenumber.ToString()},
                    };
                    string tblname = "fa_trn_tsoaheader";
                    string instcomm = objCommonIUD.InsertCommon(codes, tblname);

                    DataTable sadetaildt1 = new DataTable();
                    sadetaildt1 = SAGetheaderDetails(soanumber);
                    for (int r = 0; r < sadetaildt1.Rows.Count; r++)
                    {
                        smm.soaGid = Convert.ToInt32(sadetaildt1.Rows[r]["soaheader_gid"].ToString().Trim());
                    }
                    string[,] pays = new string[,]
                     {
                         {"soapayment_soaheader_gid", smm.soaGid.ToString()},
                         {"soapayment_sale_no", smm.soaSalenumber.ToString()},
                         {"soapayment_isremoved","N"},
                     };
                    string tblnamepay = "fa_trn_tsoapayment";
                    string instcommn = objCommonIUD.InsertCommon(pays, tblnamepay);
                }
                DataTable dtsm = new DataTable();
                dtsm = (DataTable)assetdata;
                int sum = 0;
                for (int i = 0; i < dtsm.Rows.Count; i++)
                {
                    smm = new SaleMakerModel();
                    smm.assetdetDetid = dtsm.Rows[i]["Asset Id"].ToString().Trim();
                    smm.soadetSalevalue = Convert.ToDecimal(dtsm.Rows[i]["Cheque Split Amount"].ToString().Trim());
                    smm.soadetinwno = dtsm.Rows[i]["Inward No"].ToString();
                    smm.hsn_gid = GetHsnid(dtsm.Rows[i]["HSN Code"].ToString());
                    DataTable sadetaildt1 = new DataTable();
                    sadetaildt1 = SAGetheaderDetails(soanumber);
                    for (int r = 0; r < sadetaildt1.Rows.Count; r++)
                    {
                        smm.soaGid = Convert.ToInt32(sadetaildt1.Rows[r]["soaheader_gid"].ToString().Trim());
                        //smm.assetdetAssetvalue = Convert.ToInt32(sadetaildt1.Rows[i]["assetdetails_asset_value"].ToString().Trim());
                        //smm.assetdetLocationcode = Convert.ToInt32(sadetaildt1.Rows[i]["assetdetails_locationcode"].ToString().Trim());
                    }


                    sum += Convert.ToInt32(smm.soadetSalevalue);

                    string[,] saVal = new string[,]
                    {
                      {"soaheader_sale_value", sum.ToString()},
                    };

                    string[,] whrcolSA = new string[,]
                     {  
                         {"soaheader_gid", smm.soaGid.ToString()}, 
                     };
                    string tblnamesa = "fa_trn_tsoaheader";
                    string updcomnSalv = objCommonIUD.UpdateCommon(saVal, whrcolSA, tblnamesa);

                    cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = smm.assetdetDetid.ToString();
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "takenby";
                    smm.takenby = Convert.ToString(cmd.ExecuteScalar());

                    if (smm.takenby == "14")
                    {
                        smm.takenby = "9";
                    }
                    if (smm.takenby == "21")
                    {
                        smm.takenby = "24";
                    }

                    string[,] codesass = new string[,]
                     {
                        // {"assetdetails_sale_id", smm.soaGid.ToString()},
                        // {"assetdetails_isremoved", "Y"},
                        //{"assetdetails_active_status", "N"},
                       // {"assetdetails_takenfor_soa", "Y"}
                       {"assetdetails_takenby", smm.takenby}
                     };
                    string[,] whrcolass = new string[,]
                     {  
                         {"assetdetails_assetdet_id", smm.assetdetDetid.ToString()}, 
                     };
                    string tblnameass = "fa_trn_tassetdetails";
                    string updcomnass = objCommonIUD.UpdateCommon(codesass, whrcolass, tblnameass);


                    smm.assetdetDetid = smm.assetdetDetid.ToString();
                    DataTable dtassetgid = new DataTable();
                    dtassetgid = SAassetgid(smm.assetdetDetid);

                    for (int j = 0; j < dtassetgid.Rows.Count; j++)
                    {
                        smm.soadetassetdetgid = Convert.ToInt32(dtassetgid.Rows[j]["assetdetails_gid"].ToString());
                    }


                    string[,] codedet = new string[,]
                    {
                        {"soadetail_soahead_gid",smm.soaGid.ToString()},
                       // {"soadetail_assetdet_value",smm.assetdetAssetvalue.ToString()},
                        {"soadetail_inw_no",smm.soadetinwno},
                        {"soadetail_sale_value",smm.soadetSalevalue.ToString().Trim()},
                        {"soadetail_isremoved","N"},
                        {"soadetail_assetdet_gid",smm.soadetassetdetgid.ToString()},
                        {"soadetail_hsn_gid",smm.hsn_gid.ToString()},
                    };

                    string tblname = "fa_trn_tsoadetail";
                    string instComm = objCommonIUD.InsertCommon(codedet, tblname);

                    string soagid = Convert.ToString(smm.soaGid);
                    DataTable dtAssetdetail = new DataTable();
                    dtAssetdetail = SAassetdetails(soagid);
                    for (int a = 0; a < dtAssetdetail.Rows.Count; a++)
                    {

                        smm.assetdetLocationcode = Convert.ToInt32(dtAssetdetail.Rows[a]["assetdetails_branch_gid"].ToString().Trim());
                        smm.assetdetAssetvalue = Convert.ToDecimal(dtAssetdetail.Rows[a]["assetdetails_asset_value"].ToString().Trim());
                        //smm.assetdetgid = Convert.ToInt32(dtAssetdetail.Rows[a]["assetdetails_gid"].ToString().Trim());

                    }
                    string[,] codes = new string[,]
                     {
                        //{"soadetail_soahead_gid",smm.soaGid.ToString()},
                        {"soadetail_assetdet_value",smm.assetdetAssetvalue.ToString()},
                        {"soadetail_loc_gid",smm.assetdetLocationcode.ToString()},
                         //{"soadetail_sale_date",Convert.ToString("SYSDATETIME()")},
                        //{"soadetail_sale_value",smm.soadetSalevalue.ToString().Trim()},
                        //{"soadetail_assetdet_gid",smm.assetdetgid.ToString()},
                       // {"soadetail_isremoved","N"},
                     };
                    string[,] whrcol = new string[,]
                    {
                        {"soadetail_gid",smm.soadetgid.ToString()},
                        {"soadetail_soahead_gid",smm.soapaySoaheadergid.ToString()},
                    };
                    string tblname1 = "fa_trn_tsoadetail";
                    string updcomn = objCommonIUD.UpdateCommon(codes, whrcol, tblname1);

                    //DataTable sadetaildt2 = new DataTable();
                    ////sadetaildts2 = (DataTable)assetdata;
                    //sadetaildt2 = SAGetheaderDetails(soanumber);
                    //for (int r = 0; r < sadetaildt2.Rows.Count; r++)
                    //{

                    //    smm.soaGid = Convert.ToInt32(sadetaildt2.Rows[r]["soaheader_gid"].ToString().Trim());
                    //    smm.soaSalenumber = sadetaildt2.Rows[r]["soaheader_sale_number"].ToString();
                    //}
                    //string[,] pays = new string[,]
                    // {
                    //     {"soapayment_soaheader_gid", smm.soaGid.ToString()},
                    //     {"soapayment_sale_no", smm.soaSalenumber.ToString()},
                    //     {"soapayment_isremoved","N"},
                    // };
                    //string tblnamepay = "fa_trn_tsoapayment";
                    //string instcomm = objCommonIUD.InsertCommon(pays, tblnamepay);

                }
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
                //throw ex;
            }
            finally
            {
                con.Close();
            }
            return "success";
        }

        public int SAGetCheckerDetails(int samakerid)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_sereview", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = samakerid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getsuperlist";
                int result = (int)cmd.ExecuteScalar();
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
                //throw ex;
            }
            finally
            {

            }
        }

        public DataTable SAGetheaderDetails(string saheaderid)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@soaheader_sale_number", SqlDbType.VarChar).Value = saheaderid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "saheadergid";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public DataTable SAassetdetails(string soagid)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@assetdetsaleid", SqlDbType.VarChar).Value = soagid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "assetdetsaleid";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public DataTable SAassetgid(string assetdetid)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@assetdetid", assetdetid);
                cmd.Parameters.AddWithValue("@action", "assetgid");
                // cmd.Parameters["@val"].Direction = ParameterDirection.Output;

                //string value = cmd.Parameters["@val"].Value.ToString();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }

        }

        public DataTable SApaymentGetDetails(string soanumber)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@soaheader_sale_number", SqlDbType.VarChar).Value = soanumber;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "saheadergid";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public DataTable SAGetDetails(string saassetid)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = saassetid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "saassetid";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public string SAGetstus(SaleMakerModel status, string id)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@assetid", id);
                cmd.Parameters.AddWithValue("@action", "status");
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    status.soaStatus = dt.Rows[0]["soaheader_sale_status"].ToString();
                }
                return status.soaStatus;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
                //throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        public string[,] GetVendor(string vendorcode, int saleid)
        {
            try
            {

                SaleMakerModel obj = new SaleMakerModel();
                GetConnection();
                string IsGstCharged = obj.gstcharged;

                cmd = new SqlCommand("pr_ifams_trn_tsalemakergst", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetCustomerdetails";
                cmd.Parameters.Add("@Salegid", SqlDbType.Int).Value = saleid;
                cmd.Parameters.Add("@IsGstCharged", SqlDbType.VarChar).Value = IsGstCharged;
                cmd.Parameters.Add("@Customercode", SqlDbType.VarChar).Value = vendorcode;
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row1 in dt.Rows)
                    {
                        obj.Address = row1["customer_address"].ToString();
                        obj.State = row1["state_name"].ToString();
                        obj.District = row1["district_name"].ToString();
                        obj.Pincode = row1["pincode_code"].ToString();
                        obj.customer_gid = Convert.ToInt32(row1["customer_gid"].ToString());
                        obj.state_gid = Convert.ToInt32(row1["state_gid"].ToString());
                        obj.saleamt = Convert.ToDecimal(row1["saleamount"].ToString());
                        obj.taxamount = Convert.ToDecimal(row1["taxamount"].ToString());
                        obj.gstnumber = row1["gstnumber"].ToString();
                    }
                }
                result = new string[,]
                    {
                        {"address",obj.Address.ToString()},
                        {"state", obj.State.ToString()},                     
                        {"district", obj.District.ToString()},                    
                        {"pincode", obj.Pincode},
                        {"customer_gid", obj.customer_gid.ToString()},
                        {"state_gid", obj.state_gid.ToString()}, 
                        {"saleamt",obj.saleamt.ToString()},
                        {"taxamount",obj.taxamount.ToString()},
                        {"gstnumber",obj.gstnumber.ToString()}

                    };
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                // return "";
                //throw ex;
            }
            finally
            {
                con.Close();
            }
            return result;
        }





        //public SaleMakerModel GetSaleDetail(string salegid)
        //{
        //    List<SaleMakerModel> objModel = new List<SaleMakerModel>();
        //    try
        //    {
        //        SaleMakerModel obj = new SaleMakerModel();
        //        GetConnection();
        //        DataTable dt = new DataTable();
        //        cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "SASelectView";
        //        cmd.Parameters.Add("@Salegid", SqlDbType.VarChar).Value = salegid;
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        obj.soaSalenumber = salegid;
        //        decimal str = 0;
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            obj = new SaleMakerModel();
        //            //obj.soaSalenumber = row["soaheader_sale_number"].ToString();
        //            obj.assetdetDetid = row["assetdetails_assetdet_id"].ToString();
        //            obj.assetcategory = Convert.ToString(string.IsNullOrEmpty(row["assetcategory_name"].ToString()) ? "0" : row["assetcategory_name"]);
        //            obj.assetsubcategory = Convert.ToString(string.IsNullOrEmpty(row["asset_description"].ToString()) ? "0" : row["asset_description"]);
        //            obj.soadetSalevalue = Convert.ToDecimal(row["soadetail_sale_value"].ToString());
        //            obj.assetdetCode = SAassetccode(row["assetdetails_asset_code"].ToString());
        //            obj.assetdetDescription = row["asset_description"].ToString();
        //            //obj.soadetSalevalue = Convert.ToInt32(row["soadetail_sale_value"].ToString());
        //            obj.saleamt = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_assetdet_value"].ToString()) ? "0" : row["soadetail_assetdet_value"]);
        //            obj.soadetVatvalue = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_vat_value"].ToString()) ? "0" : row["soadetail_vat_value"]);
        //            obj.soadetWdvalue = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_wd_value"].ToString()) ? "0" : row["soadetail_wd_value"]);
        //            obj.soadetProfitloss = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetial_pl_value"].ToString()) ? "0" : row["soadetial_pl_value"]);
        //            //obj.soaRectifAmt = objidm.getRetificationAmount(row["assetdetails_gid"].ToString()); //Muthu 20-09-2016
        //            obj.soaRectifAmt = getRetificationAmountsale(row["assetdetails_gid"].ToString());
        //            // obj.soadetIvvalue = 0;                    
        //            objModel.Add(obj);
        //            str = str + obj.soadetSalevalue;
        //        }
        //        obj.TModel = objModel.ToList();
        //        obj.soaSalevalue = str;
        //        return obj;
        //    }
        //    catch (Exception ex)
        //    {
        //        //objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //        //return objModel;
        //        throw ex;
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //}

        public string SAassetccode(string assetc)
        {
            GetConnection();
            DataTable dt = new DataTable();
            cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@action", "assetcode");
            cmd.Parameters.AddWithValue("@assetcode", assetc);
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt.Rows[0][0].ToString();
        }
        public SaleMakerModel GetchqSalDetail(string salegid)
        {
            try
            {
                List<SaleMakerModel> objModelch = new List<SaleMakerModel>();
                SaleMakerModel obj = new SaleMakerModel();
                GetConnection();
                // string sastatus = "";
                string statusname = "";

                DataTable dt1 = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "SAchqval");
                cmd.Parameters.AddWithValue("@Salegid", salegid);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt1);
                obj.soaGid = Convert.ToInt32(salegid);
                foreach (DataRow row in dt1.Rows)
                {
                    string statu = obj.soaStatus = row["soaheader_sale_status"].ToString();
                    //getstatus from status table
                    DataTable dt2 = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "statusstr");
                    cmd.Parameters.AddWithValue("@SAstatus", statu);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt2);
                    foreach (DataRow row2 in dt2.Rows)
                        statusname = obj.statname = row2["status_name"].ToString();
                    //obj.soaStatus = row1["status_name"].ToString();
                    obj.statname = statusname.ToString();
                    obj.soapaySaleno = row["soapayment_sale_no"].ToString();
                    obj.soapayChqno = row["soapayment_chq_no"].ToString();
                    obj.soapaychqdat = convertoDate(row["soapayment_sale_chq_date"].ToString());
                    //obj.soapaychqdat = convertoDate(string.IsNullOrEmpty(row["soapayment_sale_chq_date"].ToString()) ? SYSDATETIME() : row["soapayment_sale_chq_date"]);
                    obj.soapayAmount = row["soapayment_amount"].ToString();
                    obj.soapayRealizationdate = row["soapayment_realizationdate"].ToString();
                    obj.soaVendorname = row["soaheader_vendor_name"].ToString();
                    obj.soavendoradd = row["soaheader_vendor_address"].ToString();
                    obj.soaTnorecords = Convert.ToInt32(row["soaheader_no_records"].ToString());
                    obj.soaVatamt = row["soaheader_vat_amount"].ToString();
                    obj.vatstate = Getvatstate(row["soaheader_vat_percentage"].ToString());
                    obj.soaSaledate = convertoDate(row["soaheader_sale_date"].ToString());
                    obj.soaWtaxamt = Math.Round(Convert.ToDecimal(string.IsNullOrEmpty(row["soaheader_sale_value_wtax"].ToString()) ? "0" : row["soaheader_sale_value_wtax"]), 2);
                    obj.customer_gid = Convert.ToInt32(string.IsNullOrEmpty(row["customer_gid"].ToString()) ? "0" : row["customer_gid"].ToString());
                    obj.customer_name = Convert.ToString(row["customer_name"].ToString());
                    obj.customer_code = Convert.ToString(row["customer_code"].ToString());
                    obj.Address = Convert.ToString(row["Address"].ToString());
                    obj.State = Convert.ToString(row["State"].ToString());
                    obj.District = Convert.ToString(row["District"].ToString());
                    obj.Pincode = Convert.ToString(row["Pincode"].ToString());
                    obj.taxamount = Math.Round(Convert.ToDecimal(string.IsNullOrEmpty(row["soaheader_gst_amount"].ToString()) ? "0" : row["soaheader_gst_amount"]), 2);
                    obj.gstnumber = Convert.ToString(row["soaheader_gstnumber"].ToString());
                    obj.gstcharged = Convert.ToString(string.IsNullOrEmpty(row["soaheader_isgstcharged"].ToString()) ? "N" : row["soaheader_isgstcharged"].ToString());
                    objModelch.Add(obj);

                }

                return obj;
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

        public string Getvatstate(string taxsubtygid)
        {
            string tsubtyid = "";
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@taxsubtygid", taxsubtygid);
                cmd.Parameters.AddWithValue("@action", "vatup");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    tsubtyid = row["taxsubtype_name"].ToString();
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
                //throw ex;
            }
            finally { con.Close(); }
            return tsubtyid;
        }

        public IEnumerable<SaleMakerModel> GetSADetail(string id)
        {
            List<SaleMakerModel> objModel = new List<SaleMakerModel>();
            try
            {
                SaleMakerModel obj = new SaleMakerModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "SASearch";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new SaleMakerModel();
                    obj.soaSalenumber = row["soaheader_sale_number"].ToString();
                    obj.soaSalevalue = Convert.ToDecimal(row["soaheader_sale_value"].ToString());
                    obj.soaSaledate = row["soaheader_sale_date"].ToString();
                    obj.soaStatus = row["soaheader_sale_status"].ToString();
                    objModel.Add(obj);
                }
                return objModel;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objModel;
                //throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public void AbortSale(Models.SaleMakerModel status, string id)
        {
            try
            {
                GetConnection();
                string[,] codes2 = new string[,]
                     {
                        {"soaheader_isremoved", "Y"},

                     };
                string[,] whrcol1 = new string[,]
                     {  
                         {" soaheader_gid", id.ToString()}, 
                     };
                string tblname = "fa_trn_tsoaheader";
                string updcommn = objCommonIUD.UpdateCommon(codes2, whrcol1, tblname);

                string[,] code = new string[,]
                {
                    {"soadetail_isremoved","Y"}
                };
                string[,] wcol = new string[,]
                {
                    {"soadetail_soahead_gid" , id.ToString()},
                    {"soadetail_isremoved","N"}
                };
                string tblname1 = "fa_trn_tsoadetail";
                string updcommn1 = objCommonIUD.UpdateCommon(code, wcol, tblname1);

                DataTable dt1 = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Salegid", id.ToString());
                cmd.Parameters.AddWithValue("@action", "updatasset");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt1);
                foreach (DataRow row1 in dt1.Rows)
                {
                    cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = row1["soadetail_assetdet_gid"].ToString();
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "takenbyid";
                    string takenby = Convert.ToString(cmd.ExecuteScalar());

                    if (takenby == "9")
                    {
                        takenby = "14";
                    }
                    if (takenby == "24")
                    {
                        takenby = "21";
                    }
                    string[,] cola = new string[,]
                        {                          
                           {"assetdetails_takenby",takenby}
                        };

                    string[,] whercola = new string[,]
                        {
                            {"assetdetails_gid", row1["soadetail_assetdet_gid"].ToString() },
                        };
                    string updcm = objCommonIUD.UpdateCommon(cola, whercola, "fa_trn_tassetdetails");
                }
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

        }



        public string subSApaymentdetail(Models.SaleMakerModel status)
        {
            try
            {
                GetConnection();

                decimal rectificationamt = 0;
                decimal sumassetdep = 0;
                string soaGid = status.soaGid.ToString();



                SaleMakerModel obj = new SaleMakerModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "SASelectView";
                cmd.Parameters.Add("@Salegid", SqlDbType.VarChar).Value = soaGid;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                // obj.soaSalenumber = soaGid;
                // decimal str = 0;
                foreach (DataRow row in dt.Rows)
                {
                    obj = new SaleMakerModel();
                    obj.assetdetgid = Convert.ToInt32(string.IsNullOrEmpty(row["assetdetails_gid"].ToString()) ? "0" : row["assetdetails_gid"]);
                    // obj.assetdetgroupid = Convert.ToInt64(string.IsNullOrEmpty(row["assetdetails_asset_groupid"].ToString()) ? "0" : row["assetdetails_asset_groupid"]);
                    obj.assetdetgroupid = Convert.ToString(string.IsNullOrEmpty(row["assetdetails_asset_groupid"].ToString()) ? "0" : row["assetdetails_asset_groupid"]);
                    obj.takenby = Convert.ToString(string.IsNullOrEmpty(row["assetdetails_takenby"].ToString()) ? "0" : row["assetdetails_takenby"]);


                    DataTable dtdep = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "Totdep");
                    cmd.Parameters.AddWithValue("@assetgid", obj.assetdetgid);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dtdep);
                    foreach (DataRow rowdep in dtdep.Rows)
                    {
                        status.assetdep = rowdep["depreciation_amount"].ToString();
                    }

                    sumassetdep = Convert.ToDecimal(status.assetdep.ToString());


                    // SALE DATE TAKEN PURPOSE
                    DataTable dtsd = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "SALEDETAILS";
                    cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = obj.assetdetgid.ToString();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dtsd);
                    foreach (DataRow rowdtsd in dtsd.Rows)
                    {
                        obj.soaSaledate = rowdtsd["soaheader_sale_date"].ToString();

                    }

                    string soacaptilisationdat = Convert.ToString("SYSDATETIME()");
                    DateTime soacaptilisationda = DateTime.Now;
                    DateTime dtTillDate = Convert.ToDateTime(obj.soaSaledate.ToString()); ;
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
                    Int32 assetdetid = obj.assetdetgid;
                    // decimal sumassetdep = 0;
                    //decimal linewdv = 0;
                    //decimal rectificationamt = 0;
                    // decimal linePL = 0;


                    DataTable dtwdv = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
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
                                                                    dDepDevRate), 2);





                    rectificationamt = depamt - sumassetdep;

                    //DataTable dtgroup = new DataTable();
                    //cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@action", "Groupid");
                    //cmd.Parameters.AddWithValue("@assetgid", obj.assetdetgid);
                    //da = new SqlDataAdapter(cmd);
                    //da.Fill(dtgroup);
                    //foreach (DataRow rowgroup in dtgroup.Rows)
                    //{
                    //    status.assetdetgroupid = Convert.ToInt64(rowgroup["assetdetails_asset_groupid"].ToString());
                    //}
                    //if (sDepType == "SLM" || sDepType == "")
                    //{
                    //    sDepType = "S";
                    //}
                    //else if (sDepType == "LPM")
                    //{
                    //    sDepType = "L";
                    //}
                    //string[,] codesdep = new string[,]
                    //{
                    //    {"depreciation_amount", rectificationamt.ToString()},
                    //    {"depreciation_assetdet_gid", obj.assetdetgid.ToString()},
                    //    {"depreciation_month", Convert.ToString("SYSDATETIME()")},
                    //    {"depreciation_asset_groupid", obj.assetdetgroupid.ToString()},
                    //    {"depreciation_asset_owner", "F"},
                    //    {"depreciation_type",sDepType.ToString()},
                    //    {"depreciation_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                    //    {"depreciation_insert_date", Convert.ToString("SYSDATETIME()")}
                    //};
                    //string insertcmn = objCommonIUD.InsertCommon(codesdep, "fa_trn_tdepreciation");

                }

                string[,] codes1 = new string[,]
                     {
                        {"soapayment_chq_no", status.soapayChqno.ToString()},
                        {"soapayment_amount", status.soapayAmount.ToString()},
                        {"soapayment_update_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                        {"soapayment_update_date",Convert.ToString("SYSDATETIME()")}
                         
                       // {"soapayment_realizationdate",convertoDate(status.soapayRealizationdate.ToString())},
                     };
                string[,] whrcol = new string[,]
                     {  
                         {"soapayment_soaheader_gid", status.soaGid.ToString()}, 
                     };
                string tblname = "fa_trn_tsoapayment";
                string updcomn = objCommonIUD.UpdateCommon(codes1, whrcol, tblname);

                //soaheader
                DataTable dt1 = new DataTable();
                string wait = "WAITING FOR APPROVAL";
                cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = wait.ToString();
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "status";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt1);
                foreach (DataRow row1 in dt1.Rows)
                    status.soaStatus = row1["status_flag"].ToString();
                string[,] codesHead = new string[,]
                     {
                         {"soaheader_sale_date",convertoDate(status.soaSaledate.ToString())},
                         {"soaheader_vat_amount", status.soaVatamt.ToString()},
                       //  {"soaheader_vat_percentage", status.soaVatper.ToString()},
                         {"soaheader_vendor_name", status.soaVendorname.ToString()},
                         {"soaheader_vendor_address", status.soavendoradd.ToString() },
                         {"soaheader_sale_status",   status.soaStatus .ToString()},
                         {"soaheader_update_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                         {"soaheader_update_date",Convert.ToString("SYSDATETIME()")}
                         
                     };
                string[,] whrcolHead = new string[,]
                     {  
                         {" soaheader_gid", status.soaGid.ToString()}, 
                     };
                string tblname1 = "fa_trn_tsoaHeader";
                string updcommn = objCommonIUD.UpdateCommon(codesHead, whrcolHead, tblname1);

                //queue
                DataTable dt2 = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = "SOACHK";
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "groupname";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt2);
                string grpid = "";
                foreach (DataRow row1 in dt2.Rows)
                    grpid = row1["rolegroup_gid"].ToString();
                string[,] col = new string[,]
	                        {
                                     {"queue_date",Convert.ToString(convertoDate(objCommonIUD.GetCurrentDate()))},
                                     {"queue_ref_flag",objidm.GetRef("SOA").ToString()},
                                     {"queue_ref_gid",  status.soaGid.ToString()},
                                     {"queue_action_for", "A"},
                                     {"queue_from",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                                     {"queue_to_type","G" },
                                     {"queue_to",grpid},
                                     {"queue_prev_gid","0"},
                                     {"queue_action_status","0"},
                                     {"queue_isremoved","N"}                                     
	                      };
                string inst = objCommonIUD.InsertCommon(col, "iem_trn_tqueue");
                ////soadetail
                //string[,] codesdet = new string[,]
                //     {
                //        {"soadet"},
                //     };
                //string[,] whrcoldet = new string[,]
                //     {  
                //         {" soadetail_soahead_gid", status.soaGid.ToString()}, 
                //     };
                //string tblname2 = "fa_trn_tsoadetail";
                //string updcommnsoadet = objCommonIUD.UpdateCommon(codesdet, whrcoldet, tblname2);
                // string view = "asdf";
                return "success";

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
                //throw ex;
            }
            finally { con.Close(); }
        }


        public string subSApaymentdetailup(Models.SaleMakerModel status)
        {
            try
            {
                int soaTnorecord = Convert.ToInt32(status.soaTnorecords.ToString());
                string soaGid = status.soaGid.ToString();

                SaleMakerModel obj = new SaleMakerModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "SASelectView";
                cmd.Parameters.Add("@Salegid", SqlDbType.VarChar).Value = soaGid;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                obj.soaSalenumber = soaGid;
                // decimal str = 0;
                foreach (DataRow row in dt.Rows)
                {
                    obj = new SaleMakerModel();
                    //obj.soaSalenumber = row["soaheader_sale_number"].ToString();
                    obj.assetdetgid = Convert.ToInt32(row["assetdetails_gid"].ToString());
                    obj.assetdetDetid = row["assetdetails_assetdet_id"].ToString();
                    obj.assetcategory = Convert.ToString(string.IsNullOrEmpty(row["assetcategory_name"].ToString()) ? "0" : row["assetcategory_name"]);
                    obj.assetsubcategory = Convert.ToString(string.IsNullOrEmpty(row["asset_description"].ToString()) ? "0" : row["asset_description"]);
                    obj.soadetSalevalue = Convert.ToDecimal(row["soadetail_sale_value"].ToString());
                    obj.assetdetCode = SAassetccode(row["assetdetails_asset_code"].ToString());
                    obj.assetdetDescription = row["asset_description"].ToString();
                    obj.assetdettrfval = Convert.ToDecimal(row["assetdetails_trf_value"].ToString());
                    obj.assetdetAssetvalue = Convert.ToDecimal(row["assetdetails_asset_value"].ToString());
                    //obj.assetdetDescription = row["assetdetails_asset_description"].ToString();
                    //obj.soadetSalevalue = Convert.ToInt32(row["soadetail_sale_value"].ToString());
                    //  obj.soaSalevalue = Convert.ToDecimal(string.IsNullOrEmpty(row["assetdetails_asset_value"].ToString()) ? "0" : row["assetdetails_asset_value"]);
                    obj.saleamt = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_assetdet_value"].ToString()) ? "0" : row["soadetail_assetdet_value"]);
                    obj.soadetVatvalue = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_vat_value"].ToString()) ? "0" : row["soadetail_vat_value"]);
                    //   obj.soadetWdvalue = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_wd_value"].ToString()) ? "0" : row["soadetail_wd_value"]);
                    //obj.soadetWdvalue = Convert.ToDecimal(string.IsNullOrEmpty(row["assetdetails_asset_value"].ToString()) ? "0" : row["assetdetails_asset_value"]);
                    obj.soadetProfitloss = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetial_pl_value"].ToString()) ? "0" : row["soadetial_pl_value"]);
                    //obj.assetdetgroupid = Convert.ToInt64(string.IsNullOrEmpty(row["assetdetails_asset_groupid"].ToString()) ? "0" : row["assetdetails_asset_groupid"]);
                    obj.assetdetgroupid = Convert.ToString(string.IsNullOrEmpty(row["assetdetails_asset_groupid"].ToString()) ? "0" : row["assetdetails_asset_groupid"]);
                    // obj.soadetIvvalue = 0;                    

                    //str = str + obj.soadetSalevalue;


                    // int chspamt = Convert.ToInt32(status.soadetSalevalue.ToString());
                    decimal chspamt = Convert.ToDecimal(obj.soadetSalevalue.ToString());
                    string vadid = Convert.ToString(status.vatid);
                    decimal vatpercen = Convert.ToDecimal(Vatcalculation(vadid).ToString());
                    decimal linesalamt = (chspamt * 100) / (100 + Math.Round(vatpercen, 2));
                    string linevatamt = Convert.ToString(Math.Round(chspamt, 2) - Math.Round(linesalamt, 2));

                    //decimal linewdval =  Convert.ToDecimal(obj.soadetWdvalue.ToString());

                    //decimal lineplvalue = Math.Round(linesalamt) - Math.Round(linewdval);


                    //// SALE DATE TAKEN PURPOSE

                    obj.soaSaledate = status.soaSaledate;

                    ////WDValue
                    string soacaptilisationdat = Convert.ToString("SYSDATETIME()");
                    DateTime soacaptilisationda = DateTime.Now;
                    DateTime dtTillDate = Convert.ToDateTime(obj.soaSaledate.ToString());
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
                    Int32 assetdetid = obj.assetdetgid;
                    // decimal sumassetdep = 0;
                    decimal linewdv = 0;
                    //decimal rectificationamt = 0;
                    decimal linePL = 0;


                    DataTable dt1 = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "WDV");
                    cmd.Parameters.AddWithValue("@assetid", assetdetid);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt1);

                    //DateTime dtLeaseEnd1 =
                    foreach (DataRow row1 in dt1.Rows)
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
                                                                    dDepDevRate), 2);
                    //DataTable dtdep = new DataTable();
                    //cmd = new SqlCommand("pr_ifams_trn_SaleMaker",con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@action", "Totdep");
                    //cmd.Parameters.AddWithValue("@assetgid", obj.assetdetgid);
                    //da = new SqlDataAdapter(cmd);
                    //da.Fill(dtdep);
                    //foreach (DataRow rowdep in dtdep.Rows)
                    //{
                    //    obj.assetdep = rowdep["depreciation_amount"].ToString();
                    //}

                    // sumassetdep = Convert.ToDecimal(obj.assetdep.ToString());

                    //if (sumassetdep > 0)
                    //{
                    // linewdv = depamt + (obj.assetdetAssetvalue - obj.assetdettrfval);
                    linewdv = obj.assetdetAssetvalue - depamt;
                    //rectificationamt = depamt - sumassetdep;
                    linePL = Math.Round(linesalamt, 2) - Math.Round(linewdv, 2);
                    //linePL = (linesalamt  + linewdv) - obj.assetdetAssetvalue;
                    // }                   


                    //     string[,] codesdep = new string[,]
                    //{
                    //    {"depreciation_amount", rectificationamt.ToString()},
                    //    {"depreciation_assetdet_gid", obj.assetdetgid.ToString()},
                    //    {"depreciation_month", Convert.ToString("SYSDATETIME()")},
                    //    {"depreciation_asset_groupid", obj.assetdetgroupid.ToString()},
                    //    {"depreciation_asset_owner", "F"},
                    //    {"depreciation_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                    //    {"depreciation_insert_date", Convert.ToString("SYSDATETIME()")}
                    //};
                    //string insertcmn = objCommonIUD.InsertCommon(codesdep, "fa_trn_tdepreciation");


                    string[,] codes = new string[,]
                    {
 /*salevalue for particular lineitem in xlsheet*/
                        {"soadetail_assetdet_value", linesalamt.ToString()},
                        {"soadetail_vat_value", linevatamt.ToString()},
                     // {"soadetail_cap_date", soacaptilisationda.ToString()},
                     //  {"soadetail_deptill_date", dtTillDate.ToString()},
                        //{"soadetail_wd_value", linewdv.ToString()},
                        {"soadetail_wd_value", linewdv.ToString()},
                        //{"soadetial_pl_value", lineplvalue.ToString()}
                         {"soadetial_pl_value", linePL.ToString()},
                         {"soadetail_update_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                         {"soadetail_update_date",Convert.ToString("SYSDATETIME()")}
                    };
                    string[,] whrcol = new string[,]
                    {
                        {"soadetail_assetdet_gid", obj.assetdetgid.ToString()}
                    };
                    string tblname = "fa_trn_tsoadetail";
                    string updcoomn = objCommonIUD.UpdateCommon(codes, whrcol, tblname);

                    //save time
                    string[,] codes1 = new string[,]
                         {
                            {"soapayment_chq_no", status.soapayChqno.ToString()},
                            {"soapayment_sale_chq_date", convertoDate(status.soapaychqdat.ToString())},
                            {"soapayment_amount", status.soapayAmount.ToString()},
                            {"soapayment_update_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                            {"soapayment_update_date",Convert.ToString("SYSDATETIME()")}
                            
                            //{"soapayment_realizationdate",convertoDate(status.soapayRealizationdate.ToString())},
                         };
                    string[,] whrcol1 = new string[,]
                         {  
                             {"soapayment_soaheader_gid", status.soaGid.ToString()}, 
                         };
                    string tblname1 = "fa_trn_tsoapayment";
                    string updcomn = objCommonIUD.UpdateCommon(codes1, whrcol1, tblname1);

                    //    //soaheader
                    //    DataTable dt1 = new DataTable();
                    //    string wait = "WAITING FOR APPROVAL";
                    //    cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = wait.ToString();
                    //    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "status";
                    //    da = new SqlDataAdapter(cmd);
                    //    da.Fill(dt1);
                    //    foreach (DataRow row1 in dt1.Rows)
                    //        status.soaStatus = row1["status_flag"].ToString();
                    string[,] codesHead = new string[,]
                         {
                             {"soaheader_sale_date",convertoDate( status.soaSaledate.ToString())},
                             {"soaheader_vat_amount", status.soaVatamt.ToString()},
                            {"soaheader_vat_percentage", status.soaVatper.ToString()},//vatpercen -- Muthu
                            //{"soaheader_vat_percentage", vatpercen.ToString()},
                             {"soaheader_vendor_name", status.soaVendorname.ToString()},
                             {"soaheader_vendor_address", status.soavendoradd.ToString()},
                             {"soaheader_sale_value_wtax", status.soaWtaxamt.ToString()}
                            // {"soaheader_sale_status",   status.soaStatus .ToString()}

                         };
                    string[,] whrcolHead = new string[,]
                         {  
                             {" soaheader_gid", status.soaGid.ToString()}, 
                         };
                    string tblname11 = "fa_trn_tsoaHeader";
                    string updcommn = objCommonIUD.UpdateCommon(codesHead, whrcolHead, tblname11);
                    //}
                    //queue
                    //DataTable dt2 = new DataTable();
                    //cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = "SOACHK";
                    //cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "groupname";
                    //da = new SqlDataAdapter(cmd);
                    //da.Fill(dt2);
                    //string grpid = "";
                    //foreach (DataRow row1 in dt2.Rows)
                    //    grpid = row1["rolegroup_gid"].ToString();
                    //string[,] col = new string[,]
                    //            {
                    //                     {"queue_date",Convert.ToString(objCommonIUD.GetCurrentDate())},
                    //                     {"queue_ref_flag",objidm.GetRef("SOA").ToString()},
                    //                     {"queue_ref_gid",  status.soaGid.ToString()},
                    //                     {"queue_action_for", "A"},
                    //                     {"queue_from",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                    //                     {"queue_to_type","G" },
                    //                     {"queue_to",grpid},
                    //                     {"queue_prev_gid","0"},
                    //                     {"queue_action_status","0"},
                    //                     {"queue_isremoved","N"}                                     
                    //          };
                    //string inst = objCommonIUD.InsertCommon(col, "iem_trn_tqueue");
                    ////soadetail
                    //string[,] codesdet = new string[,]
                    //     {
                    //        {"soadet"},
                    //     };
                    //string[,] whrcoldet = new string[,]
                    //     {  
                    //         {" soadetail_soahead_gid", status.soaGid.ToString()}, 
                    //     };
                    //string tblname2 = "fa_trn_tsoadetail";
                    //string updcommnsoadet = objCommonIUD.UpdateCommon(codesdet, whrcoldet, tblname2);
                    // string view = "asdf";
                }
                return "success";

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
                //throw ex;
            }
            finally { con.Close(); }
        }


        public string GetSalNo(Models.SaleMakerModel status, int id)
        {
            try
            {
                GetConnection();
                status.soaGid = id;
                string sagetgid = "select soaheader_sale_number from fa_trn_tsoaheader where soaheader_gid = '" + status.soaGid + "'";
                cmd = new SqlCommand(sagetgid, con);
                cmd.CommandType = CommandType.Text;
                cmd.ExecuteNonQuery();
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    status.soaStatus = dt.Rows[0]["soaheader_sale_number"].ToString();
                }
                return status.soaStatus;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
                //throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public string SALocationdetail(string id)
        {
            string result = "";
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@assetdetid", SqlDbType.VarChar).Value = id.ToString();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "SALocation";
                DataTable dt = new DataTable();
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
                // return "";
                //throw ex;
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public string saveSaattachment(SaleMakerModel samodel)
        {
            try
            {
                string[,] colu = new string[,]
                {
                    {"attachment_ref_gid",samodel.soaGid.ToString()},
                    {"attachment_ref_flag",objidm.GetRef("SOA")},
                    {"attachment_filename", samodel.attach_file.ToString()},
                    {"attachment_attachmenttype_gid",objidm.GetAttachType()},
                    {"attachment_desc",samodel.attach_desc.ToString()},
                    {"attachment_date",Convert.ToString(objCommonIUD.GetCurrentDate())},
                    {"attachment_by",objCmnFunctions.GetLoginUserGid().ToString()},
                    {"attachment_isremoved","N"},
                };
                string sainst = objCommonIUD.InsertCommon(colu, "iem_trn_tattachment");
                return "success";
            }
            catch (Exception e)
            {
                objErrorLog.WriteErrorLog(e.Message.ToString(), e.ToString());
                return "";
            }
            finally
            {

            }
        }

        public string Deleteattachsub(int subattachid)
        {
            string result;
            CommonIUD delcomm = new CommonIUD();
            string colvalue = string.Empty;
            string coltemp = string.Empty;
            try
            {
                string[,] codes = new string[,]
                {
                    {"attachment_isremoved", "Y"}
                };
                string[,] whrcol = new string[,]
                {
                    {"attachment_gid", subattachid.ToString()}
                };
                string deltbl = delcomm.DeleteCommon(codes, whrcol, "iem_trn_tattachment");
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

            }
        }

        //public List<SaleMakerModel> saPopulateAuditTrail(SaleMakerModel sapat)
        //{
        //    List<SaleMakerModel> objSAAuditTrail = new List<SaleMakerModel>();
        //    try
        //    {
        //        SaleMakerModel objSAModel;
        //        GetConnection();
        //        DataTable dt = new DataTable();
        //        string safstEmp = "";
        //        string sastatus = "";
        //        cmd = new SqlCommand("pr_ifams_trn_audittaril", con);
        //        cmd.Parameters.AddWithValue("@gid", sapat.soaGid);
        //        cmd.Parameters.AddWithValue("@refflag", "SOA");
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        if (dt.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                string sapergid = Convert.ToString(dt.Rows[i]["queue_prev_gid"].ToString());
        //                if (sapergid == "0")
        //                {
        //                    safstEmp = Convert.ToString(dt.Rows[i]["queue_from"].ToString());
        //                    string saempcodenamer = objidm.Getempname(safstEmp);
        //                    string[] sadatar;
        //                    sadatar = saempcodenamer.Split(',');
        //                    objSAModel = new SaleMakerModel();
        //                    objSAModel.employee_code = sadatar[0].ToString() + " - " + sadatar[1].ToString();
        //                    objSAModel.action_status = "Submitted";
        //                    objSAModel.action_date = Convert.ToString(convertoDate(dt.Rows[i]["queue_date"].ToString()));
        //                    objSAModel.queue_to_type = "SOA Maker";
        //                    objSAModel.action_remarks = "";
        //                    objSAAuditTrail.Add(objSAModel);
        //                    string saactions = Convert.ToString(dt.Rows[i]["queue_action_by"].ToString());
        //                    if (saactions == "")
        //                    {
        //                        string saqueue_type = Convert.ToString(dt.Rows[i]["queue_to_type"].ToString());
        //                        string saqueue_to = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
        //                        string actionssa = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                        if (actionssa != "")
        //                        {
        //                            objSAModel = new SaleMakerModel();
        //                            objSAModel.employee_code = "";
        //                            objSAModel.action_date = Convert.ToString(dt.Rows[i]["queue_action_date"].ToString());
        //                            sastatus = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                            objSAModel.action_status = objidm.GetQueueStatusHistry(sastatus);
        //                            objSAModel.queue_to_type = objidm.GetQueueRole(saqueue_type, saqueue_to);
        //                            objSAModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());
        //                            objSAAuditTrail.Add(objSAModel);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        string saempid = "'";
        //                        string saqueue_type = Convert.ToString(dt.Rows[i]["queue_to_type"].ToString());
        //                        string saqueue_to = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
        //                        string actionsa = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                        if (actionsa != "")
        //                        {
        //                            objSAModel = new SaleMakerModel();
        //                            if (saqueue_type == "G" || saqueue_type == "R" || saqueue_type == "L" || saqueue_type == "D" ||saqueue_type =="E")
        //                            {
        //                                saempid = Convert.ToString(dt.Rows[i]["queue_action_by"].ToString());
        //                            }
        //                            if (saempid != "")
        //                            {
        //                                string saempcodname = objidm.Getempname(saempid);
        //                                string[] sadata;
        //                                sadata = saempcodname.Split(',');
        //                                 objSAModel.employee_code = sadata[0].ToString() + " - " + sadata[1].ToString();
        //                            }
        //                            objSAModel.action_date = Convert.ToString(dt.Rows[i]["queue_action_date"].ToString());
        //                            sastatus = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                            objSAModel.action_status = objidm.GetQueueStatusHistry(sastatus);
        //                            objSAModel.queue_to_type = objidm.GetQueueRole(saqueue_type, saqueue_to);
        //                            objSAModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());
        //                            objSAAuditTrail.Add(objSAModel);
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
        //    return objSAAuditTrail;
        //}


        public List<SaleMakerModel> saPopulateAuditTrail(SaleMakerModel sapat)
        {
            List<SaleMakerModel> objSAAuditTrail = new List<SaleMakerModel>();
            try
            {
                SaleMakerModel objModel;
                int liRaiserGid = 0;
                int liCnt = 0;
                GetConnection();
                DataTable dt = new DataTable();
                DataTable dtr = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_audittaril", con);
                cmd.Parameters.AddWithValue("@gid", sapat.soaGid);
                cmd.Parameters.AddWithValue("@refflag", "SOA");
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
                            objModel = new SaleMakerModel();
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

                            objSAAuditTrail.Add(objModel);
                            liCnt = liCnt + 1;
                        }

                        string actions = Convert.ToString(dt.Rows[i]["queue_action_for_name"].ToString());
                        string empid = Convert.ToString(dt.Rows[i]["queue_action_by"].ToString());
                        string queue_type = Convert.ToString(dt.Rows[i]["queue_to_type"].ToString());
                        string queue_to = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
                        string delgation_gid = Convert.ToString(dt.Rows[i]["queue_delegation_gid"].ToString());

                        objModel = new SaleMakerModel();
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
                            string isfinalapprover = CheckFinalApproverTag(sapat.soaGid, refflagnew, objModel.queue_gid);
                            if (isfinalapprover == "Y")
                                objModel.action_status = "Final Approver";
                        }
                        objModel.action_date = Convert.ToString(dt.Rows[i]["queue_action_date"].ToString());
                        objModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());

                        objSAAuditTrail.Add(objModel);
                    }
                }
                return objSAAuditTrail;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objSAAuditTrail;
            }
            finally
            {
                con.Close();
            }
        }


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


        public IEnumerable<SaleMakerModel> GetSADetailChecker(string id)
        {
            List<SaleMakerModel> objMod = new List<SaleMakerModel>();
            try
            {
                SaleMakerModel obj = new SaleMakerModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "SelectSAChecker";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new SaleMakerModel();
                    DataTable dt1 = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = row["soaheader_sale_status"].ToString();
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "statusname";
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt1);
                    foreach (DataRow row1 in dt1.Rows)
                        obj.soaStatus = row1["status_name"].ToString();
                    obj.soaGid = Convert.ToInt32(row["soaheader_gid"].ToString());
                    obj.soaSalenumber = row["soaheader_sale_number"].ToString();
                    obj.soaSalevalue = Convert.ToDecimal(row["soaheader_sale_value"].ToString());
                    obj.soaSaledate = row["soaheader_sale_date"].ToString();

                    obj.soaUploadfilename = row["soaheader_upld_fname"].ToString();
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

        //public string SAChkApprovalStatus(string id, string chstatus, string Remarks)
        //{
        //    try
        //    {
        //        GetConnection();
        //        if (chstatus == "REJECTED")
        //        {
        //            string[,] col = new string[,]
        //            {
        //                {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
        //                {"queue_action_status","0"},
        //                //{"queue_action_status","2"},
        //                {"queue_action_remark", Remarks.ToString()},
        //                {"queue_action_date",objCommonIUD.GetCurrentDate()},
        //                {"queue_isremoved","Y"},
        //            };
        //            string[,] whercolu = new string[,]
        //            {
        //                {"queue_ref_gid", id.Trim()},
        //                //{"queue_ref_flag", id.Trim()},
        //                {"queue_ref_flag",objidm.GetRef("SOA").ToString()},
        //            };
        //            string updcmmn = objCommonIUD.UpdateCommon(col, whercolu, "iem_trn_tqueue");

        //            DataTable dt = new DataTable();
        //            string rejct = "REJECTED";
        //            cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = rejct.ToString();
        //            cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "status";
        //            da = new SqlDataAdapter(cmd);
        //            da.Fill(dt);
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                string[,] col1 = new string[,]
        //                {
        //                {"soaheader_sale_status", row["status_flag"].ToString()},
        //                {"soaheader_process_date", objCommonIUD.GetCurrentDate()}
        //                //{"soaheader_isremoved", "Y"},
        //                };

        //                string[,] whercol1 = new string[,]
        //                {
        //                    {"soaheader_gid", id.ToString()},
        //                };
        //                string updcmm = objCommonIUD.UpdateCommon(col1, whercol1, "fa_trn_tsoaheader");


        //                string[,] colsod = new string[,]
        //                {
        //                    {"soadetail_isremoved", "Y"}
        //                };

        //                string[,] whercolsod = new string[,]
        //                {
        //                    {"soadetail_soahead_gid", id.ToString()},
        //                };
        //                string updcomm = objCommonIUD.UpdateCommon(colsod, whercolsod, "fa_trn_tsoadetail");


        //            }

        //            DataTable dt1 = new DataTable();
        //            cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@Salegid", id);
        //            cmd.Parameters.AddWithValue("@action", "updatasset");
        //            da = new SqlDataAdapter(cmd);
        //            da.Fill(dt1);
        //            foreach (DataRow row1 in dt1.Rows)
        //            {
        //                cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = row1["soadetail_assetdet_gid"].ToString();
        //                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "takenbyid";
        //                string takenby = Convert.ToString(cmd.ExecuteScalar());

        //                if (takenby == "9")
        //                {
        //                    takenby = "14";
        //                }
        //                if (takenby == "24")
        //                {
        //                    takenby = "21";
        //                }
        //                string[,] cola = new string[,]
        //                {
        //                   // {"assetdetails_takenfor_soa","N"}
        //                   {"assetdetails_takenby",takenby}
        //                };

        //                string[,] whercola = new string[,]
        //                {
        //                    {"assetdetails_gid", row1["soadetail_assetdet_gid"].ToString() },
        //                };
        //                string updcm = objCommonIUD.UpdateCommon(cola, whercola, "fa_trn_tassetdetails");
        //            }
        //        }
        //        else
        //        {

        //            decimal sumassetdep = 0;
        //            decimal rectificationamt = 0;
        //            SaleMakerModel obj = new SaleMakerModel();
        //            DataTable dtdep1 = new DataTable();
        //            cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "SASelectView";
        //            cmd.Parameters.Add("@Salegid", SqlDbType.VarChar).Value = id;
        //            da = new SqlDataAdapter(cmd);
        //            da.Fill(dtdep1);
        //            // obj.soaSalenumber = soaGid;
        //            //  decimal str = 0;
        //            foreach (DataRow row in dtdep1.Rows)
        //            {
        //                obj = new SaleMakerModel();
        //                obj.assetdetgid = Convert.ToInt32(string.IsNullOrEmpty(row["assetdetails_gid"].ToString()) ? "0" : row["assetdetails_gid"]);
        //                //  obj.assetdetgroupid = Convert.ToInt64(string.IsNullOrEmpty(row["assetdetails_asset_groupid"].ToString()) ? "0" : row["assetdetails_asset_groupid"]);
        //                obj.assetdetgroupid = Convert.ToString(string.IsNullOrEmpty(row["assetdetails_asset_groupid"].ToString()) ? "0" : row["assetdetails_asset_groupid"]);


        //                DataTable dtdep = new DataTable();
        //                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddWithValue("@action", "Totdep");
        //                cmd.Parameters.AddWithValue("@assetgid", obj.assetdetgid);
        //                da = new SqlDataAdapter(cmd);
        //                da.Fill(dtdep);
        //                foreach (DataRow rowdep in dtdep.Rows)
        //                {
        //                    obj.assetdep = rowdep["depreciation_amount"].ToString();
        //                }

        //                sumassetdep = Convert.ToDecimal(obj.assetdep.ToString());


        //                // SALE DATE TAKEN PURPOSE
        //                DataTable dtsd = new DataTable();
        //                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "SALEDETAILS";
        //                cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = obj.assetdetgid.ToString();
        //                da = new SqlDataAdapter(cmd);
        //                da.Fill(dtsd);
        //                foreach (DataRow rowdtsd in dtsd.Rows)
        //                {
        //                    obj.soaSaledate = rowdtsd["soaheader_sale_date"].ToString();
        //                }



        //                string soacaptilisationdat = Convert.ToString("SYSDATETIME()");
        //                DateTime soacaptilisationda = DateTime.Now;
        //                DateTime dtTillDate = Convert.ToDateTime(obj.soaSaledate.ToString());
        //                decimal AssetValue = 0;
        //                string sAssetCode = "";
        //                string cNot5000Case = "";
        //                string sBranch1 = "";
        //                string sBranch2 = "";
        //                DateTime dtBranchLaunch = DateTime.Now;
        //                DateTime dtLeaseStart = DateTime.Now;
        //                DateTime dtLeaseEnd = DateTime.Now;
        //                string sDepType = "";
        //                decimal dDepRate = 0;
        //                string sAssetGroup = "";
        //                string sPONumber = "";
        //                string sFICCRef = "";
        //                string sAsset_GroupId = "";
        //                decimal dTransfer_value = 0;
        //                string CanDepreciateFully = "";
        //                Int32 dDepDevRate = 0;
        //                Int32 assetdetid = obj.assetdetgid;
        //                // decimal sumassetdep = 0;
        //                //decimal linewdv = 0;
        //                //decimal rectificationamt = 0;
        //                // decimal linePL = 0;


        //                DataTable dtwdv = new DataTable();
        //                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddWithValue("@action", "WDV");
        //                cmd.Parameters.AddWithValue("@assetid", assetdetid);
        //                da = new SqlDataAdapter(cmd);
        //                da.Fill(dtwdv);

        //                //DateTime dtLeaseEnd1 =
        //                foreach (DataRow row1 in dtwdv.Rows)
        //                {
        //                    dtBranchLaunch = Convert.ToDateTime(string.IsNullOrEmpty(row1["branch_start_date"].ToString()) ? "0" : row1["branch_start_date"]);
        //                    soacaptilisationda = Convert.ToDateTime(string.IsNullOrEmpty(row1["assetdetails_cap_date"].ToString()) ? "01-01-1900" : row1["assetdetails_cap_date"]);
        //                    AssetValue = Convert.ToDecimal(string.IsNullOrEmpty(row1["assetdetails_asset_value"].ToString()) ? "0" : row1["assetdetails_asset_value"]);
        //                    sAssetCode = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_asset_code"].ToString()) ? "0" : row1["assetdetails_asset_code"]);
        //                    cNot5000Case = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_not_5kcase"].ToString()) ? "0" : row1["assetdetails_not_5kcase"]);
        //                    //sBranch1 = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_not_5kcase"].ToString()) ? "0" : row1["assetdetails_not_5kcase"]);
        //                    dtLeaseStart = Convert.ToDateTime(string.IsNullOrEmpty(row1["assetdetails_lease_startdate"].ToString()) ? "01-01-1900" : row1["assetdetails_lease_startdate"]);
        //                    dtLeaseEnd = Convert.ToDateTime(string.IsNullOrEmpty(row1["assetdetails_lease_enddate"].ToString()) ? "01-01-1900" : row1["assetdetails_lease_enddate"]);
        //                    sDepType = Convert.ToString(string.IsNullOrEmpty(row1["asset_dep_type"].ToString()) ? "0" : row1["asset_dep_type"]);
        //                    dDepRate = Convert.ToDecimal(string.IsNullOrEmpty(row1["asset_dep_rate"].ToString()) ? "0" : row1["asset_dep_rate"]);
        //                    sAssetGroup = Convert.ToString(string.IsNullOrEmpty(row1["h_asst_groupid_no"].ToString()) ? "0" : row1["h_asst_groupid_no"]);
        //                    sPONumber = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_po_number"].ToString()) ? "0" : row1["assetdetails_po_number"]);
        //                    sFICCRef = Convert.ToString(string.IsNullOrEmpty(row1["h_ficrefnumber"].ToString()) ? "0" : row1["h_ficrefnumber"]);
        //                    sAsset_GroupId = Convert.ToString(string.IsNullOrEmpty(row1["assetdetails_asset_groupid"].ToString()) ? "0" : row1["assetdetails_asset_groupid"]);
        //                    dTransfer_value = Convert.ToDecimal(string.IsNullOrEmpty(row1["assetdetails_trf_value"].ToString()) ? "0" : row1["assetdetails_trf_value"]);
        //                    // CanDepreciateFully = Convert.ToString(string.IsNullOrEmpty(row1["h_ficrefnumber"].ToString()) ? "0" : row1["h_ficrefnumber"]);
        //                }

        //                decimal depamt = Math.Round(objidm.GetTotalDep(soacaptilisationda, dtTillDate,
        //                                                                AssetValue, sAssetCode,
        //                                                                cNot5000Case, sBranch1, sBranch2,
        //                                                                dtBranchLaunch, dtLeaseStart, dtLeaseEnd,
        //                                                                sDepType, dDepRate,
        //                                                                sAssetGroup,
        //                                                                sPONumber,
        //                                                                sFICCRef,
        //                                                                sAsset_GroupId,
        //                                                                dTransfer_value,
        //                                                                CanDepreciateFully,
        //                                                                dDepDevRate),2);





        //                rectificationamt = depamt - sumassetdep;

        //                //DataTable dtgroup = new DataTable();
        //                //cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
        //                //cmd.CommandType = CommandType.StoredProcedure;
        //                //cmd.Parameters.AddWithValue("@action", "Groupid");
        //                //cmd.Parameters.AddWithValue("@assetgid", obj.assetdetgid);
        //                //da = new SqlDataAdapter(cmd);
        //                //da.Fill(dtgroup);
        //                //foreach (DataRow rowgroup in dtgroup.Rows)
        //                //{
        //                //    status.assetdetgroupid = Convert.ToInt64(rowgroup["assetdetails_asset_groupid"].ToString());
        //                //}

        //                if (rectificationamt != 0)
        //                {
        //                    string[,] codesdep = new string[,]
        //            {
        //                {"depreciation_amount", rectificationamt.ToString()},
        //                {"depreciation_assetdet_gid", obj.assetdetgid.ToString()},
        //                {"depreciation_month", Convert.ToString("SYSDATETIME()")},
        //                {"depreciation_asset_groupid", obj.assetdetgroupid.ToString()},
        //                {"depreciation_asset_owner", "F"},
        //                {"depreciation_type","S"},
        //                {"depreciation_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
        //                {"depreciation_insert_date", Convert.ToString("SYSDATETIME()")}
        //            };
        //                    string insertcmn = objCommonIUD.InsertCommon(codesdep, "fa_trn_tdepreciation");

        //                }
        //                //ENTRY FOR GL UPLOAD

        //                string glCode = "";
        //                string resdepglCode = "";
        //                string depglCode = "";
        //                string branch = "";
        //                string cat = "";
        //                string subcat = "";
        //                string asset_id = "";
        //                string BS = "";
        //                string CC = "";
        //                string ratio = "";
        //                DataTable UPLOADDATATBL = new DataTable();

        //                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "SALEDETAILS";
        //                cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = obj.assetdetgid.ToString();
        //                da = new SqlDataAdapter(cmd);
        //                string sale_value = "";
        //                string maker_id = "";
        //                string sale_date = "";
        //                string chq_no = "";
        //                string chq_date = "";
        //                string chq_amt = "";
        //                string ven_name = "";
        //                string inv = "";
        //                string soapayment_bank_gl = "";
        //                string soadetail_vat_value = "";
        //                string soadetial_pl_value = "";
        //                string saleno = "";
        //                string inwno = "";

        //                decimal soadetialplvalue = 0;
        //                UPLOADDATATBL = new DataTable();
        //                da.Fill(UPLOADDATATBL);
        //                if (UPLOADDATATBL.Rows.Count > 0)
        //                    foreach (DataRow rowdep in UPLOADDATATBL.Rows)
        //                    {
        //                        saleno = UPLOADDATATBL.Rows[0]["soaheader_sale_number"].ToString();
        //                        sale_value = UPLOADDATATBL.Rows[0]["soadetail_sale_value"].ToString();
        //                        maker_id = UPLOADDATATBL.Rows[0]["soaheader_maker_id"].ToString();
        //                        sale_date = UPLOADDATATBL.Rows[0]["soaheader_sale_date"].ToString();
        //                        chq_no = UPLOADDATATBL.Rows[0]["soapayment_chq_no"].ToString();
        //                        chq_date = UPLOADDATATBL.Rows[0]["soapayment_sale_chq_date"].ToString();
        //                        ven_name = UPLOADDATATBL.Rows[0]["soaheader_vendor_name"].ToString();
        //                        chq_amt = UPLOADDATATBL.Rows[0]["soapayment_amount"].ToString();
        //                        soadetail_vat_value = UPLOADDATATBL.Rows[0]["soadetail_vat_value"].ToString();
        //                        soadetial_pl_value = UPLOADDATATBL.Rows[0]["soadetial_pl_value"].ToString();
        //                        soapayment_bank_gl = UPLOADDATATBL.Rows[0]["soapayment_bank_gl"].ToString();
        //                        inwno = UPLOADDATATBL.Rows[0]["soadetail_inw_no"].ToString();
        //                    }


        //                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "UploadDetails";
        //                cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = obj.assetdetgid.ToString();
        //                da = new SqlDataAdapter(cmd);
        //                UPLOADDATATBL = new DataTable();
        //                da.Fill(UPLOADDATATBL);
        //                if (UPLOADDATATBL.Rows.Count > 0)
        //                    foreach (DataRow rowdep in UPLOADDATATBL.Rows)
        //                    {
        //                        glCode = UPLOADDATATBL.Rows[0]["asset_glcode"].ToString();
        //                        resdepglCode = UPLOADDATATBL.Rows[0]["asset_dep_reservglcode"].ToString();
        //                        depglCode = UPLOADDATATBL.Rows[0]["asset_dep_glcode"].ToString();
        //                        branch = UPLOADDATATBL.Rows[0]["branch_code"].ToString();
        //                        cat = UPLOADDATATBL.Rows[0]["assetcategory_name"].ToString();
        //                        if (cat.Length > 16)
        //                            cat = cat.Substring(0, 16);
        //                        subcat = UPLOADDATATBL.Rows[0]["asset_code"].ToString();
        //                        asset_id = UPLOADDATATBL.Rows[0]["assetdetails_assetdet_id"].ToString();
        //                    }

        //                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "BSCC_upload_details";
        //                cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = subcat;
        //                cmd.Parameters.Add("@fccc", SqlDbType.VarChar).Value = branch;
        //                da = new SqlDataAdapter(cmd);
        //                UPLOADDATATBL = new DataTable();
        //                da.Fill(UPLOADDATATBL);
        //                if (UPLOADDATATBL.Rows.Count > 0)
        //                    foreach (DataRow rowdep in UPLOADDATATBL.Rows)
        //                    {
        //                        BS = UPLOADDATATBL.Rows[0]["depreciationbscc_bs"].ToString();
        //                        CC = UPLOADDATATBL.Rows[0]["depreciationbscc_cc_oracle"].ToString();
        //                    }
        //                // bank gl missing doubt

        //                DataTable datamaker_id = objCmnFunctions.GetLoginUserDetails(Convert.ToInt32 (maker_id));

        //                if (chq_amt != "0")
        //                {
        //                    string[,] glCoulmsD = new string[,]
        //                                {                                
        //                                //{"tran_date",convertoDate(sale_date).ToString()/*Convert.ToString("SYSDATETIME()").ToString()*/},
        //                                {"tran_date",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_desc", asset_id.ToString() + "|" + inwno + "SALE OF ASSET" + "|" + "Inward Reference number" },
        //                                //{"tran_gl_no",soapayment_bank_gl},114070001
        //                                {"tran_gl_no","114070001"},
        //                               // {"tran_amount",chq_amt.ToString()},sale_value
        //                                {"tran_amount",sale_value.ToString()},
        //                                {"tran_acc_mode","D".ToString()},
        //                                {"tran_mult","-1"},  
        //                                {"tran_fc_code",BS},
        //                                {"tran_cc_code",CC}, 
        //                                {"tran_product_code","500".ToString()},
        //                                {"tran_ou_code",branch.ToString()},
        //                                {"tran_ref_flag",objidm.GetRef("SOA")},
        //                                {"tran_ref_gid",  obj.assetdetgid.ToString()},
        //                                {"tran_upload_gid","0".ToString()},
        //                                {"tran_isremoved","N"},
        //                                {"tran_main_cat",cat},
        //                                {"tran_sub_cat",subcat},
        //                                {"tran_expense_category","7"},
        //                                {"tran_primary_branch_code",branch.ToString()},                                
        //                             //   {"tran_invoice_no",inv.ToString()},  
        //                             //   {"tran_vendor_code","1"},
        //                             //   {"tran_vendor_name",ven_name},
        //                            //    {"tran_cheque_no",chq_no},
        //                              //  {"tran_cheque_date",convertoDate(chq_date).ToString()/*Convert.ToString("SYSDATETIME()").ToString()*/},
        //                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
        //                                {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
        //                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
        //                                };
        //                    string insertforglD = objCommonIUD.InsertCommon(glCoulmsD, "fa_trn_ttran");
        //                }
        //                //if (rectificationamt != 0)
        //                //{
        //                    DataTable dtdeptrn = new DataTable();
        //                    cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
        //                    cmd.CommandType = CommandType.StoredProcedure;
        //                    cmd.Parameters.AddWithValue("@action", "Totdep");
        //                    cmd.Parameters.AddWithValue("@assetgid", obj.assetdetgid);
        //                    da = new SqlDataAdapter(cmd);
        //                    da.Fill(dtdeptrn);
        //                    foreach (DataRow rowdep in dtdeptrn.Rows)
        //                    {
        //                        obj.assetdep = rowdep["depreciation_amount"].ToString();
        //                    }

        //                    sumassetdep = Convert.ToDecimal(obj.assetdep.ToString());



        //                    string[,] glCoulmsC = new string[,]
        //                                {                                
        //                                //{"tran_date",convertoDate(sale_date).ToString()},

        //                                {"tran_date",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
        //                               // {"tran_desc","SALE OF ASSET - " + asset_id.ToString() },
        //                               {"tran_desc", asset_id.ToString() + "|" + inwno + "SALE OF ASSET" + "|" + "Inward Reference number" },
        //                                {"tran_gl_no",resdepglCode},
        //                                {"tran_amount",sumassetdep.ToString()},
        //                                {"tran_acc_mode","D".ToString()},
        //                                {"tran_mult","1"},  
        //                                {"tran_fc_code",BS},
        //                                {"tran_cc_code",CC},  
        //                                {"tran_product_code","500".ToString()},
        //                                {"tran_ou_code",branch.ToString()},
        //                                {"tran_ref_flag",objidm.GetRef("SOA")},
        //                                {"tran_ref_gid",  obj.assetdetgid.ToString()},
        //                                {"tran_upload_gid","0".ToString()},
        //                                {"tran_isremoved","N"},
        //                             {"tran_main_cat",cat},
        //                                {"tran_sub_cat",subcat},
        //                               {"tran_expense_category","7"},
        //                                {"tran_primary_branch_code",branch.ToString()},
        //                             //  {"tran_invoice_no",inv.ToString()},  
        //                              //  {"tran_vendor_code","1"},
        //                             //   {"tran_vendor_name",ven_name},
        //                             //   {"tran_cheque_no",chq_no},
        //                             //   {"tran_cheque_date",convertoDate(chq_date).ToString()},
        //                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
        //                                {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
        //                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
        //                                };
        //                    string insertforglC = objCommonIUD.InsertCommon(glCoulmsC, "fa_trn_ttran");
        //               // }
        //                if (AssetValue != 0)
        //                {
        //                    string[,] glCoulmsDepD = new string[,]
        //                                {                             
        //                                //{"tran_date",convertoDate(sale_date).ToString()},
        //                                {"tran_date",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_gl_no",glCode},
        //                               // {"tran_desc","SALE OF ASSET - " + asset_id.ToString() },
        //                               {"tran_desc", asset_id.ToString() + "|" + inwno + "SALE OF ASSET" + "|" + "Inward Reference number" },
        //                                {"tran_amount",AssetValue.ToString()},
        //                                {"tran_acc_mode","C".ToString()},
        //                                {"tran_mult","-1"},  
        //                                {"tran_fc_code",BS},
        //                                {"tran_cc_code",CC},  
        //                                {"tran_product_code","500".ToString()},
        //                                {"tran_ou_code",branch.ToString()},
        //                                {"tran_ref_flag",objidm.GetRef("SOA")},
        //                                {"tran_ref_gid",obj.assetdetgid.ToString()},
        //                                {"tran_upload_gid","0".ToString()},
        //                                {"tran_isremoved","N"},
        //                             {"tran_main_cat",cat},
        //                                {"tran_sub_cat",subcat},
        //                                 {"tran_invoice_no",inv.ToString()}, 
        //                                {"tran_expense_category","7"},
        //                             //    {"tran_vendor_code","1"},
        //                             //   {"tran_vendor_name",ven_name},
        //                             //   {"tran_cheque_no",chq_no},
        //                             //   {"tran_cheque_date",convertoDate(chq_date).ToString()},
        //                                {"tran_primary_branch_code",branch.ToString()},                               
        //                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
        //                                {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
        //                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
        //                                };
        //                    string insertforglDepD = objCommonIUD.InsertCommon(glCoulmsDepD, "fa_trn_ttran");
        //                }
        //                string c_D = "";
        //                if (Convert.ToDecimal(soadetial_pl_value) > 0)
        //                    c_D = "c";
        //                else
        //                    c_D = "D";


        //                if (Convert.ToDecimal(soadetial_pl_value) < 0 && c_D == "D")
        //                {


        //                    soadetial_pl_value = Convert.ToString(Math.Abs(Convert.ToDecimal(soadetial_pl_value)));
        //                    //;
        //                //     //Asset Value()A-Depreciation(B)+VAT(C)-Sale Value(D),
        //                //    soadetialplvalue = Convert.ToDecimal(AssetValue) - Convert.ToDecimal(depamt) + Convert.ToDecimal(soadetail_vat_value) - Convert.ToDecimal(sale_value);
        //                }

        //                if (soadetial_pl_value != "0")
        //                {
        //                    string[,] glCoulmsDepC = new string[,]
        //                                {                                
        //                                //{"tran_date",convertoDate(sale_date).ToString()},
        //                                {"tran_date",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
        //                               // {"tran_desc","SALE OF ASSET - " + asset_id.ToString() },
        //                               {"tran_desc", asset_id.ToString() + "|" + inwno + "SALE OF ASSET" + "|" + "Inward Reference number" },
        //                                {"tran_gl_no","441400001"},
        //                                {"tran_amount",soadetial_pl_value.ToString()},
        //                               // {"tran_amount",soadetialplvalue.ToString()},
        //                                {"tran_acc_mode",c_D.ToString()},
        //                                {"tran_mult","1"},  
        //                                {"tran_fc_code",BS},
        //                                {"tran_cc_code",CC},     
        //                                {"tran_product_code","500".ToString()},
        //                                {"tran_ou_code",branch.ToString()},
        //                                {"tran_ref_flag",objidm.GetRef("SOA")},
        //                                {"tran_ref_gid",obj.assetdetgid.ToString()},
        //                                {"tran_upload_gid","0".ToString()},
        //                                {"tran_isremoved","N"},
        //                             {"tran_main_cat",cat},
        //                                {"tran_sub_cat",subcat},
        //                                {"tran_expense_category","7"},
        //                               //  {"tran_vendor_code","1"},
        //                              //  {"tran_vendor_name",ven_name},
        //                              //  {"tran_cheque_no",chq_no},
        //                               // {"tran_cheque_date",convertoDate(chq_date).ToString()},
        //                                {"tran_primary_branch_code",branch.ToString()},                               
        //                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
        //                                {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
        //                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
        //                                };
        //                    string insertforglDepC = objCommonIUD.InsertCommon(glCoulmsDepC, "fa_trn_ttran");
        //                }
        //                if (soadetail_vat_value != "0")
        //                {
        //                    string[,] glCoulmsDepC1 = new string[,]
        //                                {                                
        //                                //{"tran_date",convertoDate(sale_date).ToString()},
        //                                {"tran_date",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
        //                               // {"tran_desc","SALE OF ASSET - " + asset_id.ToString() },
        //                               {"tran_desc", asset_id.ToString() + "|" + inwno + "SALE OF ASSET" + "|" + "Inward Reference number" },
        //                                {"tran_gl_no","211100101"},
        //                                {"tran_amount",soadetail_vat_value.ToString()},
        //                                {"tran_acc_mode","C".ToString()},
        //                                {"tran_mult","1"},  
        //                                {"tran_fc_code",BS},
        //                                {"tran_cc_code",CC},     
        //                                {"tran_product_code","500".ToString()},
        //                                {"tran_ou_code",branch.ToString()},
        //                                {"tran_ref_flag",objidm.GetRef("SOA")},
        //                                {"tran_ref_gid",obj.assetdetgid.ToString()},
        //                                {"tran_upload_gid","0".ToString()},
        //                                {"tran_isremoved","N"},
        //                             {"tran_main_cat",cat},
        //                                {"tran_sub_cat",subcat},
        //                               {"tran_expense_category","7"},
        //                               //  {"tran_vendor_code","1"},
        //                               // {"tran_vendor_name",ven_name},
        //                               // {"tran_cheque_no",chq_no},
        //                               // {"tran_cheque_date",convertoDate(chq_date).ToString()},
        //                                {"tran_primary_branch_code",branch.ToString()},                               
        //                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
        //                                {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
        //                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
        //                                };
        //                    string insertforglDepC1 = objCommonIUD.InsertCommon(glCoulmsDepC1, "fa_trn_ttran");
        //                }



        //                //tran table entry for saledepreciation refflag(SDEP)

        //                if (rectificationamt != 0)
        //                {
        //                    if (rectificationamt < 0)
        //                    {
        //                        rectificationamt = Math.Abs(rectificationamt);



        //                        string[,] glCoulmssaledepC = new string[,]
        //                                {                                
        //                                //{"tran_date",convertoDate(sale_date).ToString()},
        //                                {"tran_date",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
        //                             //   {"tran_desc","DEP FOR ASSET SALE - FOR THE ASSET :  " + asset_id.ToString() },
        //                                {"tran_desc", asset_id.ToString() + "|" + inwno + "SALE OF ASSET" + "|" + "Inward Reference number" },
        //                                {"tran_gl_no",depglCode},
        //                                {"tran_amount",rectificationamt.ToString()},
        //                                {"tran_acc_mode","C".ToString()},
        //                                {"tran_mult","1"},  
        //                                {"tran_fc_code",BS},
        //                                {"tran_cc_code",CC},  
        //                                {"tran_product_code","500".ToString()},
        //                                {"tran_ou_code",branch.ToString()},
        //                                {"tran_ref_flag",objidm.GetRef("SDEP")},
        //                                {"tran_ref_gid",  obj.assetdetgid.ToString()},
        //                                {"tran_upload_gid","0".ToString()},
        //                                {"tran_isremoved","N"},
        //                                {"tran_main_cat",cat},
        //                                {"tran_sub_cat",subcat},
        //                                {"tran_expense_category","10"},
        //                                {"tran_primary_branch_code",branch.ToString()},
        //                             //  {"tran_invoice_no",inv.ToString()},  
        //                             //   {"tran_vendor_code","1"},
        //                             //   {"tran_vendor_name",ven_name},
        //                              //  {"tran_cheque_no",chq_no},
        //                              //  {"tran_cheque_date",convertoDate(chq_date).ToString()},
        //                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
        //                                {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
        //                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
        //                                };
        //                        string insertforsaledepglC = objCommonIUD.InsertCommon(glCoulmssaledepC, "fa_trn_ttran");


        //                        string[,] glCoulmssaledepD = new string[,]
        //                                {                                
        //                                //{"tran_date",convertoDate(sale_date).ToString()},
        //                                {"tran_date",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
        //                               // {"tran_desc","DEP FOR ASSET SALE - FOR THE ASSET :  " + asset_id.ToString() },
        //                               {"tran_desc", asset_id.ToString() + "|" + inwno + "SALE OF ASSET" + "|" + "Inward Reference number" },
        //                                {"tran_gl_no",resdepglCode},
        //                                {"tran_amount",rectificationamt.ToString()},
        //                                {"tran_acc_mode","D".ToString()},
        //                                {"tran_mult","1"},  
        //                                {"tran_fc_code",BS},
        //                                {"tran_cc_code",CC},  
        //                                {"tran_product_code","500".ToString()},
        //                                {"tran_ou_code",branch.ToString()},
        //                                {"tran_ref_flag",objidm.GetRef("SDEP")},
        //                                {"tran_ref_gid",  obj.assetdetgid.ToString()},
        //                                {"tran_upload_gid","0".ToString()},
        //                                {"tran_isremoved","N"},
        //                             {"tran_main_cat",cat},
        //                                {"tran_sub_cat",subcat},
        //                                     {"tran_expense_category","10"},
        //                                {"tran_primary_branch_code",branch.ToString()},
        //                               //{"tran_invoice_no",inv.ToString()},  
        //                              //  {"tran_vendor_code","1"},
        //                              //  {"tran_vendor_name",ven_name},
        //                              //  {"tran_cheque_no",chq_no},
        //                               // {"tran_cheque_date",convertoDate(chq_date).ToString()},
        //                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
        //                                {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
        //                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
        //                                };
        //                        string insertforsaledepglD = objCommonIUD.InsertCommon(glCoulmssaledepD, "fa_trn_ttran");
        //                    }

        //                    else
        //                    {
        //                        string[,] glCoulmssaledepC = new string[,]
        //                                {                                
        //                                //{"tran_date",convertoDate(sale_date).ToString()},
        //                                {"tran_date",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
        //                             //   {"tran_desc","DEP FOR ASSET SALE - FOR THE ASSET :  " + asset_id.ToString() },
        //                                {"tran_desc", asset_id.ToString() + "|" + inwno + "SALE OF ASSET" + "|" + "Inward Reference number" },
        //                                {"tran_gl_no",resdepglCode},
        //                                {"tran_amount",rectificationamt.ToString()},
        //                                {"tran_acc_mode","C".ToString()},
        //                                {"tran_mult","1"},  
        //                                {"tran_fc_code",BS},
        //                                {"tran_cc_code",CC},  
        //                                {"tran_product_code","500".ToString()},
        //                                {"tran_ou_code",branch.ToString()},
        //                                {"tran_ref_flag",objidm.GetRef("SDEP")},
        //                                {"tran_ref_gid",  obj.assetdetgid.ToString()},
        //                                {"tran_upload_gid","0".ToString()},
        //                                {"tran_isremoved","N"},
        //                             {"tran_main_cat",cat},
        //                                {"tran_sub_cat",subcat},
        //                                     {"tran_expense_category","10"},
        //                                {"tran_primary_branch_code",branch.ToString()},
        //                             //  {"tran_invoice_no",inv.ToString()},  
        //                             //   {"tran_vendor_code","1"},
        //                             //   {"tran_vendor_name",ven_name},
        //                              //  {"tran_cheque_no",chq_no},
        //                              //  {"tran_cheque_date",convertoDate(chq_date).ToString()},
        //                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
        //                                {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
        //                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
        //                                };
        //                        string insertforsaledepglC = objCommonIUD.InsertCommon(glCoulmssaledepC, "fa_trn_ttran");


        //                        string[,] glCoulmssaledepD = new string[,]
        //                                {                                
        //                                //{"tran_date",convertoDate(sale_date).ToString()},
        //                                {"tran_date",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_val_date",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_payment_date",Convert.ToString("SYSDATETIME()").ToString()},
        //                               // {"tran_desc","DEP FOR ASSET SALE - FOR THE ASSET :  " + asset_id.ToString() },
        //                               {"tran_desc", asset_id.ToString() + "|" + inwno + "SALE OF ASSET" + "|" + "Inward Reference number" },
        //                                {"tran_gl_no",depglCode},
        //                                {"tran_amount",rectificationamt.ToString()},
        //                                {"tran_acc_mode","D".ToString()},
        //                                {"tran_mult","1"},  
        //                                {"tran_fc_code",BS},
        //                                {"tran_cc_code",CC},  
        //                                {"tran_product_code","500".ToString()},
        //                                {"tran_ou_code",branch.ToString()},
        //                                {"tran_ref_flag",objidm.GetRef("SDEP")},
        //                                {"tran_ref_gid",  obj.assetdetgid.ToString()},
        //                                {"tran_upload_gid","0".ToString()},
        //                                {"tran_isremoved","N"},
        //                             {"tran_main_cat",cat},
        //                                {"tran_sub_cat",subcat},
        //                                     {"tran_expense_category","10"},
        //                                {"tran_primary_branch_code",branch.ToString()},
        //                               //{"tran_invoice_no",inv.ToString()},  
        //                              //  {"tran_vendor_code","1"},
        //                              //  {"tran_vendor_name",ven_name},
        //                              //  {"tran_cheque_no",chq_no},
        //                               // {"tran_cheque_date",convertoDate(chq_date).ToString()},
        //                                {"tran_expense_month",Convert.ToString("SYSDATETIME()").ToString()},
        //                                {"tran_checker_id",datamaker_id.Rows[0]["employee_code"].ToString()},
        //                                {"tran_emp_id",datamaker_id.Rows[0]["employee_code"].ToString()},
        //                                {"tran_authoriser_id",Convert.ToString(HttpContext.Current.Session["employee_code"])}
        //                                };
        //                        string insertforsaledepglD = objCommonIUD.InsertCommon(glCoulmssaledepD, "fa_trn_ttran");
        //                    }

        //                }
        //            }
        //            string[,] col = new string[,]
        //            {
        //                {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
        //                {"queue_action_status","1"},
        //                {"queue_action_remark", Remarks.ToString()},
        //                {"queue_action_date",objCommonIUD.GetCurrentDate()},
        //                {"queue_isremoved","Y"},
        //            };
        //            string[,] whercolu = new string[,]
        //            {
        //                {"queue_ref_gid", id.Trim()},
        //                //{"queue_ref_flag", id.Trim()},
        //                {"queue_ref_flag",objidm.GetRef("SOA").ToString()},
        //            };
        //            string updcmmn = objCommonIUD.UpdateCommon(col, whercolu, "iem_trn_tqueue");

        //            //soaheader
        //            string status = "";
        //            DataTable dt1 = new DataTable();
        //            string rejct = "APPROVED";
        //            cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = rejct.ToString();
        //            cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "status";
        //            da = new SqlDataAdapter(cmd);
        //            da.Fill(dt1);
        //            foreach (DataRow row1 in dt1.Rows)
        //            {
        //                status = row1["status_flag"].ToString();
        //            }

        //            string[,] col1 = new string[,]
        //            {
        //                {"soaheader_checker_id", objCmnFunctions.GetLoginUserGid().ToString()},
        //                {"soaheader_checker_date", objCommonIUD.GetCurrentDate()},
        //                //{"soaheader_isremoved","Y"},
        //                {"soaheader_sale_status", status.ToString()},
        //                {"soaheader_process_date", objCommonIUD.GetCurrentDate()}
        //            };
        //            string[,] whrcol = new string[,]
        //            {
        //                {"soaheader_gid", id.Trim()},
        //            };
        //            string updcmn = objCommonIUD.UpdateCommon(col1, whrcol, "fa_trn_tsoaheader");

        //            DataTable dt = new DataTable();
        //            cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@soahgid", id);
        //            cmd.Parameters.AddWithValue("@action", "updateasset");
        //            da = new SqlDataAdapter(cmd);
        //            dt = new DataTable();
        //            da.Fill(dt);
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = row["assetdetails_gid"].ToString();
        //                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "takenbyid";
        //                string takenby = Convert.ToString(cmd.ExecuteScalar());

        //                if (takenby == "9")
        //                {
        //                    takenby = "4";
        //                }
        //                if (takenby == "24")
        //                {
        //                    takenby = "22";
        //                }
        //                string[,] col2 = new string[,]
        //            {
        //                //{"assetdetails_isremoved","Y"},
        //              //  {"assetdetails_active_status","N"},
        //                {"assetdetails_sale_status","Y"},
        //                {"assetdetails_sale_date", Convert.ToString(objCommonIUD.GetCurrentDate())},
        //                {"assetdetails_takenby",takenby}
        //            };
        //                string[,] whrcol2 = new string[,]
        //            {
        //                {"assetdetails_gid", row["assetdetails_gid"].ToString()},
        //            };
        //                string updcm = objCommonIUD.UpdateCommon(col2, whrcol2, "fa_trn_tassetdetails");
        //            }
        //        }
        //        return chstatus;
        //    }
        //    catch (Exception ex)
        //    {
        //        objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //        return "";
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }

        //}


        public string SAChkApprovalStatus(string id, string chstatus, string Remarks)
        {
            string catchresult = string.Empty;
            Int64 loginid = Convert.ToInt64(objCmnFunctions.GetLoginUserGid());
            try
            {
                GetConnection();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_SAChkApprovalStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Salegid", SqlDbType.BigInt).Value = Convert.ToInt64(id.ToString());
                cmd.Parameters.Add("@Chkstatus", SqlDbType.VarChar, 16).Value = Convert.ToString(chstatus.ToString());
                cmd.Parameters.Add("@Remarks", SqlDbType.VarChar, -1).Value = Convert.ToString(Remarks.ToString());
                cmd.Parameters.Add("@Logingid", SqlDbType.BigInt).Value = Convert.ToInt64(loginid.ToString());


                //Add the output parameter to the command object
                SqlParameter outPutParameter = new SqlParameter();
                outPutParameter.ParameterName = "@Result";
                outPutParameter.SqlDbType = System.Data.SqlDbType.VarChar;
                outPutParameter.Size = 16;
                outPutParameter.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(outPutParameter);

                //Open the connection and execute the query
                //con.Open();
                cmd.ExecuteNonQuery();

                //Retrieve the value of the output parameter
                catchresult = outPutParameter.Value.ToString();

            }
            catch (Exception ex)
            {

                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

                con.Close();

            }
            return catchresult;
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

        public IEnumerable<VatModel> Getvat()
        {
            List<VatModel> objvatper = new List<VatModel>();
            try
            {
                VatModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "vat");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new VatModel();
                    objModel.vatid = dt.Rows[i]["taxsubtype_gid"].ToString();
                    objModel.vatstate = dt.Rows[i]["taxsubtype_name"].ToString();
                    objvatper.Add(objModel);
                }
                return objvatper;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objvatper;
            }
            finally
            {
                con.Close();
            }
        }

        public string Vatcalculation(string id)
        {
            string result = "";
            // List<SaleMakerModel> objModel = new List<SaleMakerModel>();
            try
            {
                // SaleMakerModel obj;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "vatval");
                cmd.Parameters.AddWithValue("@vatgid", id);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    // obj = new SaleMakerModel();
                    result = dt.Rows[i]["taxrate_rate"].ToString();
                    //objModel.Add(obj);
                }
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return result;
            }
            finally
            {
                con.Close();
            }
        }

        public string GetGrpCountSA(string sagrpId)
        {
            string result = "";
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_saleasset", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@saleresult", SqlDbType.VarChar).Value = "Groupcount";
                cmd.Parameters.Add("@salechkdata", SqlDbType.VarChar).Value = sagrpId;
                DataTable dt = new DataTable();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                sagrpId = "0";
                if (dt.Rows.Count > 0)
                {
                    sagrpId = dt.Rows[0]["goaheader_asset_count"].ToString();
                }
                return sagrpId;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return result;
            }
            finally
            {
                con.Close();
            }

        }

        public List<SaleMakerModel> SAAutosaleno(string term, string status, string loginid)
        {
            List<SaleMakerModel> Model = new List<SaleMakerModel>();
            SaleMakerModel obj = new SaleMakerModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "SAAutoSaleno";
                cmd.Parameters.Add("@saleno", SqlDbType.VarChar).Value = term;
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@Makerid", loginid);
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new SaleMakerModel();
                    obj.soaSalenumber = row["soaheader_sale_number"].ToString();
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

        public List<SaleMakerModel> SAAutosalenochk(string term, string loginid)
        {
            List<SaleMakerModel> Model = new List<SaleMakerModel>();
            SaleMakerModel obj = new SaleMakerModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "SAAutoSalenochk";
                cmd.Parameters.Add("@saleno", SqlDbType.VarChar).Value = term;
                //cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@Makerid", loginid);
                dt.Load(cmd.ExecuteReader());
                foreach (DataRow row in dt.Rows)
                {
                    obj = new SaleMakerModel();
                    obj.soaSalenumber = row["soaheader_sale_number"].ToString();
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


        public string RevokeSaleDetail(string soahgid)
        {
            List<SaleMakerModel> Model = new List<SaleMakerModel>();
            SaleMakerModel obj = new SaleMakerModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "soaheaderdet");
                cmd.Parameters.AddWithValue("@soahgid", soahgid);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj = new SaleMakerModel();
                    obj.assetdetgid = Convert.ToInt32(row["assetdetails_gid"].ToString());
                    obj.takenby = row["assetdetails_takenby"].ToString();
                    if (obj.takenby == "4")
                    {
                        obj.takenby = "14";
                    }
                    else if (obj.takenby == "22")
                    {
                        obj.takenby = "21";
                    }

                    string[,] codepay = new string[,]
                    {
                        {"soapayment_isremoved","Y"},
                        {"soapayment_update_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                        {"soapayment_update_date",Convert.ToString("SYSDATETIME()")}
                    };
                    string[,] whrpay = new string[,]
                    {
                        {"soapayment_soaheader_gid", soahgid},
                        {"soapayment_isremoved", "N"}
                    };

                    string updcm = objCommonIUD.UpdateCommon(codepay, whrpay, "fa_trn_tsoapayment");

                    string[,] codesoah = new string[,]
                    {
                        {"soaheader_isremoved","Y"},
                        //{"soaheader_inw_no",""},
                        {"soaheader_update_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                        {"soaheader_update_date",Convert.ToString("SYSDATETIME()")}
                    };

                    string[,] whersoah = new string[,]
                    {
                        {"soaheader_gid",soahgid},
                        {"soaheader_isremoved","N"}
                    };

                    string updcomn = objCommonIUD.UpdateCommon(codesoah, whersoah, "fa_trn_tsoaheader");

                    string[,] codeassdet = new string[,]
                    {
                       // {"assetdetails_active_status","Y"},
                        {"assetdetails_sale_status","N"},
                        {"assetdetails_sale_date",""},
                        {"assetdetails_takenby",obj.takenby.ToString()}
                    };
                    string[,] whrassdet = new string[,]
                    {
                        {"assetdetails_gid", obj.assetdetgid.ToString()},
                      //  {"assetdetails_active_status","N"},
                        {"assetdetails_sale_status","Y"},
                        {"assetdetails_isremoved","N"}
                    };
                    string uptcmmn = objCommonIUD.UpdateCommon(codeassdet, whrassdet, "fa_trn_tassetdetails");

                    cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "deletedepreciation");
                    cmd.Parameters.AddWithValue("@assetid", obj.assetdetgid.ToString());
                    cmd.ExecuteNonQuery();

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
            return "Success";
        }


        public string Getempcode(string empgid)
        {
            string empid = "";
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




        public decimal getRetificationAmountsale(string assetId)
        {
            GetConnection();
            decimal rectificationamt = 0;
            string soaSaledate = "";
            try
            {


                DataTable dtwdv = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "WDV");
                cmd.Parameters.AddWithValue("@assetid", assetId);
                da = new SqlDataAdapter(cmd);
                da.Fill(dtwdv);


                // SALE DATE TAKEN PURPOSE
                DataTable dtsd = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "SALEDETAILS";
                cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = assetId.ToString();
                da = new SqlDataAdapter(cmd);
                da.Fill(dtsd);
                foreach (DataRow rowdtsd in dtsd.Rows)
                {
                    soaSaledate = rowdtsd["soaheader_sale_date"].ToString();
                }

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
                DateTime dtTillDate = Convert.ToDateTime(soaSaledate.ToString());
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




        public SaleMakerModel GetSaleDetail(string salegid)
        {
            List<HsnData> objhsn = new List<HsnData>();
            List<SaleMakerModel> objModel = new List<SaleMakerModel>();
            try
            {
                //bharathidhasan kumar1
                SaleMakerModel obj = new SaleMakerModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "SASelectView";
                cmd.Parameters.Add("@Salegid", SqlDbType.VarChar).Value = salegid;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                obj.soaSalenumber = salegid;
                decimal str = 0;
                foreach (DataRow row in dt.Rows)
                {
                    obj = new SaleMakerModel();
                    //obj.soaSalenumber = row["soaheader_sale_number"].ToString();
                    obj.soaGid = Convert.ToInt32(salegid.ToString());
                    obj.assetdetDetid = row["assetdetails_assetdet_id"].ToString();
                    obj.soadetgid = Convert.ToInt32(row["soadetail_gid"].ToString());
                    obj.assetcategory = Convert.ToString(string.IsNullOrEmpty(row["assetcategory_name"].ToString()) ? "0" : row["assetcategory_name"]);
                    obj.assetsubcategory = Convert.ToString(string.IsNullOrEmpty(row["asset_description"].ToString()) ? "0" : row["asset_description"]);
                    obj.soadetSalevalue = Convert.ToDecimal(row["soadetail_sale_value"].ToString());
                    obj.assetdetCode = SAassetccode(row["assetdetails_asset_code"].ToString());
                    obj.assetdetDescription = row["asset_description"].ToString();
                    //obj.soadetSalevalue = Convert.ToInt32(row["soadetail_sale_value"].ToString());
                    obj.saleamt = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_assetdet_value"].ToString()) ? "0" : row["soadetail_assetdet_value"]);
                    obj.soadetVatvalue = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_vat_value"].ToString()) ? "0" : row["soadetail_vat_value"]);
                    obj.soadetWdvalue = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_wd_value"].ToString()) ? "0" : row["soadetail_wd_value"]);
                    obj.soadetProfitloss = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetial_pl_value"].ToString()) ? "0" : row["soadetial_pl_value"]);
                    //obj.soaRectifAmt = objidm.getRetificationAmount(row["assetdetails_gid"].ToString()); //Muthu 20-09-2016
                    obj.soaRectifAmt = getRetificationAmountsale(row["assetdetails_gid"].ToString());
                    obj.hsn_code = Convert.ToString(string.IsNullOrEmpty(row["hsn_code"].ToString()) ? "0" : row["hsn_code"]);
                    obj.hsn_gid = Convert.ToInt32(string.IsNullOrEmpty(row["soadetail_hsn_gid"].ToString()) ? 0 : row["soadetail_hsn_gid"]);
                    obj.branch_name = Convert.ToString(string.IsNullOrEmpty(row["branch_name"].ToString()) ? "0" : row["branch_name"]);
                    obj.cgst_amount = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_cgst_amount"].ToString()) ? "0" : row["soadetail_cgst_amount"]);
                    obj.sgst_amount = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_sgst_amount"].ToString()) ? "0" : row["soadetail_sgst_amount"]);
                    obj.igst_amount = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_igst_amount"].ToString()) ? "0" : row["soadetail_igst_amount"]);
                    obj.cgcess_amount = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_cgcess_amount"].ToString()) ? "0" : row["soadetail_cgcess_amount"]);
                    obj.sgcess_amount = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_sgcess_amount"].ToString()) ? "0" : row["soadetail_sgcess_amount"]);
                    obj.igcess_amount = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_igcess_amount"].ToString()) ? "0" : row["soadetail_igcess_amount"]);
                    obj.assetgid = Convert.ToInt32(string.IsNullOrEmpty(row["asset_gid"].ToString()) ? "0" : row["asset_gid"]);
                    obj.assetdetgid = Convert.ToInt32(string.IsNullOrEmpty(row["assetdetails_gid"].ToString()) ? "0" : row["assetdetails_gid"]);
                    obj.HsnDetails = new SelectList(GetHsnDetails(obj.assetgid, obj.soadetgid).ToList(), "hsn_gid", "hsn_code");
                    obj.provider_stategid = Convert.ToInt32(string.IsNullOrEmpty(row["provider_stategid"].ToString()) ? 0 : row["provider_stategid"]);
                    obj.provider_statename = Convert.ToString(string.IsNullOrEmpty(row["provider_statename"].ToString()) ? "0" : row["provider_statename"]);
                    obj.provider_gstnumber = Convert.ToString(string.IsNullOrEmpty(row["provider_gstin"].ToString()) ? "0" : row["provider_gstin"]);
                    objModel.Add(obj);
                    str = str + obj.soadetSalevalue;
                }
                obj.TModel = objModel.ToList();
                obj.soaSalevalue = str;
                return obj;
            }
            catch (Exception ex)
            {
                //objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                //return objModel;
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }


        public List<HsnData> GetHsnDetails(int assetgid, int soadetgid)
        {
            List<HsnData> objhsndata = new List<HsnData>();
            try
            {
                HsnData objhsndetail;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Hsndata";
                cmd.Parameters.Add("@assetgid", SqlDbType.Int).Value = assetgid;
                cmd.Parameters.Add("@soahgid", SqlDbType.Int).Value = soadetgid;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objhsndetail = new HsnData();
                    objhsndetail.hsn_gid = Convert.ToInt32(dt.Rows[i]["hsn_gid"].ToString());
                    objhsndetail.hsn_code = Convert.ToString(dt.Rows[i]["hsn_code"].ToString());
                    objhsndata.Add(objhsndetail);
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
            return objhsndata;
        }

        public List<Salemakermodelgst> gstsalemakerdetails(string pincode, string salegid, string hsndesc)
        {
            List<Salemakermodelgst> objModel = new List<Salemakermodelgst>();
            try
            {
                Salemakermodelgst obj = new Salemakermodelgst();
                GetConnection();
                DataTable dt = new DataTable();

                cmd = new SqlCommand("pr_ifams_trn_tsalemakergst", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Salegid", SqlDbType.Int).Value = Convert.ToInt32(salegid);
                cmd.Parameters.Add("@ReciverPincode", SqlDbType.VarChar).Value = pincode;
                cmd.Parameters.Add("@HsnDetails", SqlDbType.VarChar).Value = hsndesc;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetGstdetails";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                //obj.soaSalenumber = salegid;
                //decimal str = 0;
                foreach (DataRow row in dt.Rows)
                {
                    obj = new Salemakermodelgst();
                    //obj.soaSalenumber = row["soaheader_sale_number"].ToString();
                    obj.hsn_gid = Convert.ToInt32(row["hsn_gid"].ToString().Trim());
                    obj.hsn_code = Convert.ToString(string.IsNullOrEmpty(row["hsn_code"].ToString()) ? "0" : row["hsn_code"]);
                    obj.hsn_description = Convert.ToString(string.IsNullOrEmpty(row["hsn_description"].ToString()) ? "0" : row["hsn_description"]);
                    obj.taxsubtype = Convert.ToString(row["TaxsubtypeName"].ToString());
                    obj.taxrate = Convert.ToDecimal(string.IsNullOrEmpty(row["rate"].ToString()) ? "0" : row["rate"]);
                    obj.amount = Convert.ToDecimal(string.IsNullOrEmpty(row["amount"].ToString()) ? "0" : row["amount"]);
                    //obj.soadetSalevalue = Convert.ToInt32(row["soadetail_sale_value"].ToString());
                    obj.taxamount = Convert.ToDecimal(string.IsNullOrEmpty(row["Taxamt"].ToString()) ? "0" : row["Taxamt"]);
                    objModel.Add(obj);
                    //str = str + obj.soadetSalevalue;
                }

                //obj.TModel = objModel.ToList();
                //obj.soaSalevalue = str;
                return objModel;
            }
            catch (Exception ex)
            {
                //objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                //return objModel;
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }


        public string assetsalepaymentdetailsupdate(Models.SaleMakerModel status)
        {
            try
            {
                //int soaTnorecord = Convert.ToInt32(status.soaTnorecords.ToString());
                string soaGid = status.soaGid.ToString();
                string pincode = status.Pincode.ToString();

                SaleMakerModel obj = new SaleMakerModel();
                GetConnection();
                DataSet ds = new DataSet();
                cmd = new SqlCommand("pr_ifams_trn_tsalemakergst", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Salegid", SqlDbType.VarChar).Value = soaGid;
                cmd.Parameters.Add("@ReciverPincode", SqlDbType.VarChar).Value = pincode;
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                obj.soaSalenumber = soaGid;
                // decimal str = 0;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    obj = new SaleMakerModel();
                    //obj.soaSalenumber = row["soaheader_sale_number"].ToString();
                    obj.assetdetgid = Convert.ToInt32(row["assetdetails_gid"].ToString());
                    obj.assetdetDetid = row["assetdetails_assetdet_id"].ToString();
                    obj.assetcategory = Convert.ToString(string.IsNullOrEmpty(row["assetcategory_name"].ToString()) ? "0" : row["assetcategory_name"]);
                    obj.assetsubcategory = Convert.ToString(string.IsNullOrEmpty(row["asset_description"].ToString()) ? "0" : row["asset_description"]);
                    obj.soadetSalevalue = Convert.ToDecimal(row["soadetail_sale_value"].ToString());
                    obj.assetdetCode = SAassetccode(row["assetdetails_asset_code"].ToString());
                    obj.assetdetDescription = row["asset_description"].ToString();
                    //obj.soadetSalevalue = Convert.ToInt32(row["soadetail_sale_value"].ToString());
                    obj.saleamt = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_assetdet_value"].ToString()) ? "0" : row["soadetail_assetdet_value"]);
                    obj.soadetVatvalue = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_vat_value"].ToString()) ? "0" : row["soadetail_vat_value"]);
                    obj.soadetWdvalue = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_wd_value"].ToString()) ? "0" : row["soadetail_wd_value"]);
                    obj.soadetProfitloss = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetial_pl_value"].ToString()) ? "0" : row["soadetial_pl_value"]);
                    //obj.soaRectifAmt = objidm.getRetificationAmount(row["assetdetails_gid"].ToString()); //Muthu 20-09-2016
                    obj.soaRectifAmt = getRetificationAmountsale(row["assetdetails_gid"].ToString());
                    obj.hsn_code = Convert.ToString(string.IsNullOrEmpty(row["hsn_code"].ToString()) ? "0" : row["hsn_code"]);
                    obj.branch_name = Convert.ToString(string.IsNullOrEmpty(row["branch_name"].ToString()) ? "0" : row["branch_name"]);
                    obj.cgst_amount = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_cgst_amount"].ToString()) ? "0" : row["soadetail_cgst_amount"]);
                    obj.sgst_amount = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_sgst_amount"].ToString()) ? "0" : row["soadetail_sgst_amount"]);
                    obj.igst_amount = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_igst_amount"].ToString()) ? "0" : row["soadetail_igst_amount"]);
                    obj.cgcess_amount = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_cgcess_amount"].ToString()) ? "0" : row["soadetail_cgcess_amount"]);
                    obj.sgcess_amount = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_sgcess_amount"].ToString()) ? "0" : row["soadetail_sgcess_amount"]);
                    obj.igcess_amount = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_igcess_amount"].ToString()) ? "0" : row["soadetail_igcess_amount"]);


                    // int chspamt = Convert.ToInt32(status.soadetSalevalue.ToString());
                    decimal chspamt = Convert.ToDecimal(obj.soadetSalevalue.ToString());
                    decimal cgst_amt = Convert.ToDecimal(obj.cgst_amount.ToString());
                    decimal sgst_amt = Convert.ToDecimal(obj.sgst_amount.ToString());
                    decimal igst_amt = Convert.ToDecimal(obj.igst_amount.ToString());
                    decimal cgcess_amt = Convert.ToDecimal(obj.cgcess_amount.ToString());
                    decimal sgcess_amt = Convert.ToDecimal(obj.sgcess_amount.ToString());
                    decimal igcess_amt = Convert.ToDecimal(obj.igcess_amount.ToString());


                    obj.soaSaledate = status.soaSaledate;

                    ////WDValue
                    string soacaptilisationdat = Convert.ToString("SYSDATETIME()");
                    DateTime soacaptilisationda = DateTime.Now;
                    DateTime dtTillDate = Convert.ToDateTime(obj.soaSaledate.ToString());
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
                    Int32 assetdetid = obj.assetdetgid;
                    // decimal sumassetdep = 0;
                    decimal linewdv = 0;
                    //decimal rectificationamt = 0;
                    decimal linePL = 0;


                    DataTable dt1 = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "WDV");
                    cmd.Parameters.AddWithValue("@assetid", assetdetid);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt1);

                    //DateTime dtLeaseEnd1 =
                    foreach (DataRow row1 in dt1.Rows)
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
                                                                    dDepDevRate), 2);

                    linewdv = obj.assetdetAssetvalue - depamt;

                    //linePL = Math.Round(linesalamt, 2) - Math.Round(linewdv, 2);


                    string[,] codes = new string[,]
                    { 
                        {"soadetail_cgst_amount ", cgst_amt.ToString()},
                        {"soadetail_sgst_amount ", sgst_amt.ToString()},
                        {"soadetail_igst_amount ", igst_amt.ToString()},
                        {"soadetail_cgcess_amount  ", cgcess_amt.ToString()},
                        {"soadetail_sgcess_amount  ",sgcess_amt.ToString()},
                        {"soadetail_igcess_amount ", igcess_amt.ToString()},
                        {"soadetail_wd_value", linewdv.ToString()},
                        //{"soadetial_pl_value", lineplvalue.ToString()}
                         {"soadetial_pl_value", linePL.ToString()},
                         {"soadetail_update_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                         {"soadetail_update_date",Convert.ToString("SYSDATETIME()")}
                    };
                    string[,] whrcol = new string[,]
                    {
                        {"soadetail_assetdet_gid", obj.assetdetgid.ToString()}
                    };
                    string tblname = "fa_trn_tsoadetail";
                    string updcoomn = objCommonIUD.UpdateCommon(codes, whrcol, tblname);

                    string[,] codes1 = new string[,]
                         {
                            {"soapayment_chq_no", status.soapayChqno.ToString()},
                            {"soapayment_sale_chq_date", convertoDate(status.soapaychqdat.ToString())},
                            {"soapayment_amount", status.soapayAmount.ToString()},
                            {"soapayment_update_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                            {"soapayment_update_date",Convert.ToString("SYSDATETIME()")}
                            
                            //{"soapayment_realizationdate",convertoDate(status.soapayRealizationdate.ToString())},
                         };
                    string[,] whrcol1 = new string[,]
                         {  
                             {"soapayment_soaheader_gid", status.soaGid.ToString()}, 
                         };
                    string tblname1 = "fa_trn_tsoapayment";
                    string updcomn = objCommonIUD.UpdateCommon(codes1, whrcol1, tblname1);

                    string[,] codesHead = new string[,]
                         {
                             {"soaheader_sale_date",convertoDate( status.soaSaledate.ToString())},
                            // {"soaheader_vat_amount", status.soaVatamt.ToString()},
                            //{"soaheader_vat_percentage", status.soaVatper.ToString()},//vatpercen -- Muthu
                            //{"soaheader_vat_percentage", vatpercen.ToString()},
                             {"soaheader_vendor_name", status.soaVendorname.ToString()},
                             {"soaheader_vendor_address", status.Address.ToString()},
                             {"soaheader_sale_value_wtax", status.soaWtaxamt.ToString()}
                            // {"soaheader_sale_status",   status.soaStatus .ToString()}

                         };
                    string[,] whrcolHead = new string[,]
                         {  
                             {"soaheader_gid", status.soaGid.ToString()}, 
                         };
                    string tblname11 = "fa_trn_tsoaHeader";
                    string updcommn = objCommonIUD.UpdateCommon(codesHead, whrcolHead, tblname11);


                }
                return "success";

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
                //throw ex;
            }
            finally { con.Close(); }
        }


        public string assetdetailupdate(Models.SaleMakerModel status)
        {
            try
            {
                int soaTnorecord = Convert.ToInt32(status.soaTnorecords.ToString());
                string soaGid = status.soaGid.ToString();
                string pincode = status.Pincode.ToString();
                string customercode = status.customer_code.ToString();
                int contactgid = status.contact_gid;
                string IsGstCharged = status.gstcharged;

                SaleMakerModel obj = new SaleMakerModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_tsalemakergst", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Salegid", SqlDbType.VarChar).Value = soaGid;
                cmd.Parameters.Add("@ReciverPincode", SqlDbType.VarChar).Value = pincode;
                cmd.Parameters.Add("@IsGstCharged", SqlDbType.VarChar).Value = IsGstCharged;
                cmd.Parameters.Add("@HsnDetails", SqlDbType.VarChar).Value = status.hsn_description;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetAssetdetails";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                obj.soaSalenumber = soaGid;
                // decimal str = 0;
                foreach (DataRow row in dt.Rows)
                {
                    obj = new SaleMakerModel();
                    //obj.soaSalenumber = row["soaheader_sale_number"].ToString();
                    obj.assetdetgid = Convert.ToInt32(row["assetdetails_gid"].ToString());
                    obj.assetdetDetid = row["assetdetails_assetdet_id"].ToString();
                    obj.assetcategory = Convert.ToString(string.IsNullOrEmpty(row["assetcategory_name"].ToString()) ? "0" : row["assetcategory_name"]);
                    obj.assetsubcategory = Convert.ToString(string.IsNullOrEmpty(row["asset_description"].ToString()) ? "0" : row["asset_description"]);
                    obj.soadetSalevalue = Convert.ToDecimal(row["soadetail_sale_value"].ToString());
                    obj.assetdetCode = SAassetccode(row["assetdetails_asset_code"].ToString());
                    obj.assetdetDescription = row["asset_description"].ToString();
                    obj.assetdettrfval = Convert.ToDecimal(row["assetdetails_trf_value"].ToString());
                    obj.assetdetAssetvalue = Convert.ToDecimal(row["assetdetails_asset_value"].ToString());
                    //obj.assetdetDescription = row["assetdetails_asset_description"].ToString();
                    //obj.soadetSalevalue = Convert.ToInt32(row["soadetail_sale_value"].ToString());
                    //  obj.soaSalevalue = Convert.ToDecimal(string.IsNullOrEmpty(row["assetdetails_asset_value"].ToString()) ? "0" : row["assetdetails_asset_value"]);
                    obj.saleamt = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_assetdet_value"].ToString()) ? "0" : row["soadetail_assetdet_value"]);
                    obj.soadetVatvalue = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_vat_value"].ToString()) ? "0" : row["soadetail_vat_value"]);
                    //   obj.soadetWdvalue = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_wd_value"].ToString()) ? "0" : row["soadetail_wd_value"]);
                    //obj.soadetWdvalue = Convert.ToDecimal(string.IsNullOrEmpty(row["assetdetails_asset_value"].ToString()) ? "0" : row["assetdetails_asset_value"]);
                    obj.soadetProfitloss = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetial_pl_value"].ToString()) ? "0" : row["soadetial_pl_value"]);
                    //obj.assetdetgroupid = Convert.ToInt64(string.IsNullOrEmpty(row["assetdetails_asset_groupid"].ToString()) ? "0" : row["assetdetails_asset_groupid"]);
                    obj.assetdetgroupid = Convert.ToString(string.IsNullOrEmpty(row["assetdetails_asset_groupid"].ToString()) ? "0" : row["assetdetails_asset_groupid"]);
                    // obj.soadetIvvalue = 0;                    

                    //str = str + obj.soadetSalevalue;


                    obj.hsn_code = Convert.ToString(string.IsNullOrEmpty(row["hsn_code"].ToString()) ? "0" : row["hsn_code"]);
                    obj.branch_name = Convert.ToString(string.IsNullOrEmpty(row["branch_name"].ToString()) ? "0" : row["branch_name"]);
                    obj.cgst_amount = Convert.ToDecimal(string.IsNullOrEmpty(row["cgst_amount"].ToString()) ? "0" : row["cgst_amount"]);
                    obj.sgst_amount = Convert.ToDecimal(string.IsNullOrEmpty(row["sgst_amount"].ToString()) ? "0" : row["sgst_amount"]);
                    obj.igst_amount = Convert.ToDecimal(string.IsNullOrEmpty(row["igst_amount"].ToString()) ? "0" : row["igst_amount"]);
                    obj.cgst_rate = Convert.ToDecimal(string.IsNullOrEmpty(row["cgst_rate"].ToString()) ? "0" : row["cgst_rate"]);
                    obj.sgst_rate = Convert.ToDecimal(string.IsNullOrEmpty(row["sgst_rate"].ToString()) ? "0" : row["sgst_rate"]);
                    obj.igst_rate = Convert.ToDecimal(string.IsNullOrEmpty(row["igst_rate"].ToString()) ? "0" : row["igst_rate"]);
                    obj.cgcess_amount = Convert.ToDecimal(string.IsNullOrEmpty(row["cgcess_amount"].ToString()) ? "0" : row["cgcess_amount"]);
                    obj.sgcess_amount = Convert.ToDecimal(string.IsNullOrEmpty(row["sgcess_amount"].ToString()) ? "0" : row["sgcess_amount"]);
                    obj.igcess_amount = Convert.ToDecimal(string.IsNullOrEmpty(row["igcess_amount"].ToString()) ? "0" : row["igcess_amount"]);
                    obj.cgcess_rate = Convert.ToDecimal(string.IsNullOrEmpty(row["cgcess_rate"].ToString()) ? "0" : row["cgcess_rate"]);
                    obj.sgcess_rate = Convert.ToDecimal(string.IsNullOrEmpty(row["sgcess_rate"].ToString()) ? "0" : row["sgcess_rate"]);
                    obj.igcess_rate = Convert.ToDecimal(string.IsNullOrEmpty(row["igcess_rate"].ToString()) ? "0" : row["igcess_rate"]);




                    // int chspamt = Convert.ToInt32(status.soadetSalevalue.ToString());
                    decimal chspamt = Convert.ToDecimal(obj.soadetSalevalue.ToString());
                    string vadid = Convert.ToString(status.vatid);
                    decimal vatpercen = Convert.ToDecimal(Vatcalculation(vadid).ToString());
                    // bharathi calculation needed//
                    //decimal linesalamt = (chspamt * 100) / (100 + Math.Round(vatpercen, 2));          
                    //string linevatamt = Convert.ToString(Math.Round(chspamt, 2) - Math.Round(linesalamt, 2));
                    decimal gsttaxamount = 0.00M;
                    gsttaxamount = obj.cgst_amount + obj.sgst_amount + obj.igst_amount + obj.cgcess_amount + obj.sgcess_amount + obj.igcess_amount;

                    decimal linesalamt = Convert.ToDecimal(Math.Round(chspamt, 2) - Math.Round(gsttaxamount, 2));

                    //decimal linewdval =  Convert.ToDecimal(obj.soadetWdvalue.ToString());

                    //decimal lineplvalue = Math.Round(linesalamt) - Math.Round(linewdval);


                    //// SALE DATE TAKEN PURPOSE

                    obj.soaSaledate = status.soaSaledate;

                    ////WDValue
                    string soacaptilisationdat = Convert.ToString("SYSDATETIME()");
                    DateTime soacaptilisationda = DateTime.Now;
                    DateTime dtTillDate = Convert.ToDateTime(obj.soaSaledate.ToString());
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
                    Int32 assetdetid = obj.assetdetgid;
                    // decimal sumassetdep = 0;
                    decimal linewdv = 0;
                    //decimal rectificationamt = 0;
                    decimal linePL = 0;


                    DataTable dt1 = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "WDV");
                    cmd.Parameters.AddWithValue("@assetid", assetdetid);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt1);

                    //DateTime dtLeaseEnd1 =
                    foreach (DataRow row1 in dt1.Rows)
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
                                                                    dDepDevRate), 2);
                    //DataTable dtdep = new DataTable();
                    //cmd = new SqlCommand("pr_ifams_trn_SaleMaker",con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@action", "Totdep");
                    //cmd.Parameters.AddWithValue("@assetgid", obj.assetdetgid);
                    //da = new SqlDataAdapter(cmd);
                    //da.Fill(dtdep);
                    //foreach (DataRow rowdep in dtdep.Rows)
                    //{
                    //    obj.assetdep = rowdep["depreciation_amount"].ToString();
                    //}

                    // sumassetdep = Convert.ToDecimal(obj.assetdep.ToString());

                    //if (sumassetdep > 0)
                    //{
                    // linewdv = depamt + (obj.assetdetAssetvalue - obj.assetdettrfval);
                    linewdv = obj.assetdetAssetvalue - depamt;
                    //rectificationamt = depamt - sumassetdep;
                    linePL = Math.Round(linesalamt, 2) - Math.Round(linewdv, 2);
                    //linePL = (linesalamt  + linewdv) - obj.assetdetAssetvalue;
                    // }                   


                    //     string[,] codesdep = new string[,]
                    //{
                    //    {"depreciation_amount", rectificationamt.ToString()},
                    //    {"depreciation_assetdet_gid", obj.assetdetgid.ToString()},
                    //    {"depreciation_month", Convert.ToString("SYSDATETIME()")},
                    //    {"depreciation_asset_groupid", obj.assetdetgroupid.ToString()},
                    //    {"depreciation_asset_owner", "F"},
                    //    {"depreciation_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                    //    {"depreciation_insert_date", Convert.ToString("SYSDATETIME()")}
                    //};
                    //string insertcmn = objCommonIUD.InsertCommon(codesdep, "fa_trn_tdepreciation");


                    string[,] codes = new string[,]
                    {
 /*salevalue for particular lineitem in xlsheet*/
                        {"soadetail_assetdet_value", linesalamt.ToString()},
                         {"soadetail_cgst_amount", obj.cgst_amount.ToString()},
                        {"soadetail_sgst_amount ", obj.sgst_amount.ToString()},
                        {"soadetail_igst_amount ", obj.igst_amount.ToString()},
                        {"soadetail_cgst_rate",obj.cgst_rate.ToString()},
                        {"soadetail_sgst_rate",obj.sgst_rate.ToString()},
                        {"soadetail_igst_rate",obj.igst_rate.ToString()}, 
                        {"soadetail_cgcess_amount ", obj.cgcess_amount.ToString()},
                        {"soadetail_sgcess_amount ", obj.sgcess_amount.ToString()},
                        {"soadetail_igcess_amount", obj.igcess_amount.ToString()},
                        {"soadetail_cgcess_rate",obj.cgcess_rate.ToString()},
                        {"soadetail_sgcess_rate",obj.sgcess_rate.ToString()},
                        {"soadetail_igcess_rate",obj.igcess_rate.ToString()}, 
                        //{"soadetail_vat_value", linevatamt.ToString()},
                     // {"soadetail_cap_date", soacaptilisationda.ToString()},
                     //  {"soadetail_deptill_date", dtTillDate.ToString()},
                        //{"soadetail_wd_value", linewdv.ToString()},
                        {"soadetail_wd_value", linewdv.ToString()},
                        //{"soadetial_pl_value", lineplvalue.ToString()}
                         {"soadetial_pl_value", linePL.ToString()},
                         {"soadetail_update_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                         {"soadetail_update_date",Convert.ToString("SYSDATETIME()")}
                    };
                    string[,] whrcol = new string[,]
                    {
                        {"soadetail_assetdet_gid", obj.assetdetgid.ToString()}
                    };
                    string tblname = "fa_trn_tsoadetail";
                    string updcoomn = objCommonIUD.UpdateCommon(codes, whrcol, tblname);

                    //save time
                    string[,] codes1 = new string[,]
                         {
                            {"soapayment_chq_no", status.soapayChqno.ToString()},
                            {"soapayment_sale_chq_date", convertoDate(status.soapaychqdat.ToString())},
                            {"soapayment_amount", status.soapayAmount.ToString()},
                            {"soapayment_update_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                            {"soapayment_update_date",Convert.ToString("SYSDATETIME()")}
                            
                            //{"soapayment_realizationdate",convertoDate(status.soapayRealizationdate.ToString())},
                         };
                    string[,] whrcol1 = new string[,]
                         {  
                             {"soapayment_soaheader_gid", status.soaGid.ToString()}, 
                         };
                    string tblname1 = "fa_trn_tsoapayment";
                    string updcomn = objCommonIUD.UpdateCommon(codes1, whrcol1, tblname1);

                    //    //soaheader
                    //    DataTable dt1 = new DataTable();
                    //    string wait = "WAITING FOR APPROVAL";
                    //    cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = wait.ToString();
                    //    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "status";
                    //    da = new SqlDataAdapter(cmd);
                    //    da.Fill(dt1);
                    //    foreach (DataRow row1 in dt1.Rows)
                    //        status.soaStatus = row1["status_flag"].ToString();

                    /* K.bharathidasan
                     * 
                    string[,] codesHead = new string[,]
                         {
                             {"soaheader_sale_date",convertoDate( status.soaSaledate.ToString())},
                             {"soaheader_vat_amount", status.soaVatamt.ToString()},
                            {"soaheader_vat_percentage", status.soaVatper.ToString()},//vatpercen -- Muthu
                            //{"soaheader_vat_percentage", vatpercen.ToString()},
                             {"soaheader_vendor_name", status.soaVendorname.ToString()},
                             {"soaheader_vendor_address", status.soavendoradd.ToString()},
                             {"soaheader_sale_value_wtax", status.soaWtaxamt.ToString()}
                            // {"soaheader_sale_status",   status.soaStatus .ToString()}

                         };
                    string[,] whrcolHead = new string[,]
                         {  
                             {" soaheader_gid", status.soaGid.ToString()}, 
                         };
                    string tblname11 = "fa_trn_tsoaHeader";
                    string updcommn = objCommonIUD.UpdateCommon(codesHead, whrcolHead, tblname11);


                    */

                    //}
                    //queue
                    //DataTable dt2 = new DataTable();
                    //cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = "SOACHK";
                    //cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "groupname";
                    //da = new SqlDataAdapter(cmd);
                    //da.Fill(dt2);
                    //string grpid = "";
                    //foreach (DataRow row1 in dt2.Rows)
                    //    grpid = row1["rolegroup_gid"].ToString();
                    //string[,] col = new string[,]
                    //            {
                    //                     {"queue_date",Convert.ToString(objCommonIUD.GetCurrentDate())},
                    //                     {"queue_ref_flag",objidm.GetRef("SOA").ToString()},
                    //                     {"queue_ref_gid",  status.soaGid.ToString()},
                    //                     {"queue_action_for", "A"},
                    //                     {"queue_from",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                    //                     {"queue_to_type","G" },
                    //                     {"queue_to",grpid},
                    //                     {"queue_prev_gid","0"},
                    //                     {"queue_action_status","0"},
                    //                     {"queue_isremoved","N"}                                     
                    //          };
                    //string inst = objCommonIUD.InsertCommon(col, "iem_trn_tqueue");
                    ////soadetail
                    //string[,] codesdet = new string[,]
                    //     {
                    //        {"soadet"},
                    //     };
                    //string[,] whrcoldet = new string[,]
                    //     {  
                    //         {" soadetail_soahead_gid", status.soaGid.ToString()}, 
                    //     };
                    //string tblname2 = "fa_trn_tsoadetail";
                    //string updcommnsoadet = objCommonIUD.UpdateCommon(codesdet, whrcoldet, tblname2);
                    // string view = "asdf";
                }


                DataTable dt2 = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_tsalemakergst", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Salegid", SqlDbType.VarChar).Value = soaGid;
                cmd.Parameters.Add("@Customergid", SqlDbType.VarChar).Value = customercode;
                cmd.Parameters.Add("@Contactgid", SqlDbType.Int).Value = contactgid;
                cmd.Parameters.Add("@IsGstCharged", SqlDbType.VarChar).Value = IsGstCharged;
                cmd.Parameters.Add("@HsnDetails", SqlDbType.VarChar).Value = status.hsn_description;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetContactdetails";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt2);
                obj.soaSalenumber = soaGid;
                // decimal str = 0;
                foreach (DataRow row in dt2.Rows)
                {
                    obj = new SaleMakerModel();
                    obj.cgst_rate = Convert.ToDecimal(string.IsNullOrEmpty(row["cgstpercentage"].ToString()) ? "0" : row["cgstpercentage"]);
                    obj.sgst_rate = Convert.ToDecimal(string.IsNullOrEmpty(row["sgstpercentage"].ToString()) ? "0" : row["sgstpercentage"]);
                    obj.igst_rate = Convert.ToDecimal(string.IsNullOrEmpty(row["igstpercentage"].ToString()) ? "0" : row["igstpercentage"]);
                    obj.cgcess_rate = Convert.ToDecimal(string.IsNullOrEmpty(row["cgcesspercentage"].ToString()) ? "0" : row["cgcesspercentage"]);
                    obj.sgcess_rate = Convert.ToDecimal(string.IsNullOrEmpty(row["sgcesspercentage"].ToString()) ? "0" : row["sgcesspercentage"]);
                    obj.igcess_rate = Convert.ToDecimal(string.IsNullOrEmpty(row["igcesspercentage"].ToString()) ? "0" : row["igcesspercentage"]);
                    obj.taxamount = Convert.ToDecimal(string.IsNullOrEmpty(row["taxamount"].ToString()) ? "0" : row["taxamount"]);
                    obj.customer_gid = Convert.ToInt32(string.IsNullOrEmpty(row["customer_gid"].ToString()) ? "0" : row["customer_gid"]);
                    obj.contact_gid = Convert.ToInt32(string.IsNullOrEmpty(row["contactgid"].ToString()) ? "0" : row["contactgid"]);
                    obj.customer_name = Convert.ToString(string.IsNullOrEmpty(row["customer_name"].ToString()));
                    obj.Address = Convert.ToString(string.IsNullOrEmpty(row["customer_address"].ToString()));
                    obj.gstnumber = Convert.ToString(row["gstnumber"].ToString());
                    string[,] codesHead = new string[,]
                         {
                             {"soaheader_sale_date",convertoDate( status.soaSaledate.ToString())},
                            // {"soaheader_vat_amount", status.soaVatamt.ToString()},
                            //{"soaheader_vat_percentage", status.soaVatper.ToString()},//vatpercen -- Muthu
                            //{"soaheader_vat_percentage", vatpercen.ToString()},
                            {"soaheader_gst_amount",obj.taxamount.ToString()},
                            {"soaheader_cgst_percentage",obj.cgst_rate.ToString()},
                            {"soaheader_sgst_percentage",obj.sgst_rate.ToString()},
                            {"soaheader_igst_percentage",obj.igst_rate.ToString()},
                            {"soaheader_cgcess_percentage",obj.cgst_rate.ToString()},
                            {"soaheader_sgcess_percentage",obj.sgst_rate.ToString()},
                            {"soaheader_igcess_percentage",obj.igst_rate.ToString()},
                             {"soaheader_vendor_name",obj.customer_name.ToString()},
                             {"soaheader_vendor_address", obj.Address.ToString()},
                             {"soaheader_sale_value_wtax", status.soaWtaxamt.ToString()},
                             {"soaheader_customergid",obj.customer_gid.ToString()},
                             {"soaheader_contact_gid",obj.contact_gid.ToString()},
                             {"soaheader_gstnumber",obj.gstnumber.ToString()},
                            {"soaheader_isgstcharged", status.gstcharged.ToString()}

                         };
                    string[,] whrcolHead = new string[,]
                         {  
                             {" soaheader_gid", status.soaGid.ToString()}, 
                         };
                    string tblname11 = "fa_trn_tsoaHeader";
                    string updcommn = objCommonIUD.UpdateCommon(codesHead, whrcolHead, tblname11);


                }



                return "success";

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
                //throw ex;
            }
            finally { con.Close(); }
        }

        public string assetdetailssave(Models.SaleMakerModel status)
        {
            try
            {
                GetConnection();

                decimal rectificationamt = 0;
                decimal sumassetdep = 0;
                string soaGid = status.soaGid.ToString();



                SaleMakerModel obj = new SaleMakerModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "SASelectView";
                cmd.Parameters.Add("@Salegid", SqlDbType.VarChar).Value = soaGid;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                // obj.soaSalenumber = soaGid;
                // decimal str = 0;
                foreach (DataRow row in dt.Rows)
                {
                    obj = new SaleMakerModel();
                    obj.assetdetgid = Convert.ToInt32(string.IsNullOrEmpty(row["assetdetails_gid"].ToString()) ? "0" : row["assetdetails_gid"]);
                    // obj.assetdetgroupid = Convert.ToInt64(string.IsNullOrEmpty(row["assetdetails_asset_groupid"].ToString()) ? "0" : row["assetdetails_asset_groupid"]);
                    obj.assetdetgroupid = Convert.ToString(string.IsNullOrEmpty(row["assetdetails_asset_groupid"].ToString()) ? "0" : row["assetdetails_asset_groupid"]);
                    obj.takenby = Convert.ToString(string.IsNullOrEmpty(row["assetdetails_takenby"].ToString()) ? "0" : row["assetdetails_takenby"]);


                    DataTable dtdep = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@action", "Totdep");
                    cmd.Parameters.AddWithValue("@assetgid", obj.assetdetgid);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dtdep);
                    foreach (DataRow rowdep in dtdep.Rows)
                    {
                        status.assetdep = rowdep["depreciation_amount"].ToString();
                    }

                    sumassetdep = Convert.ToDecimal(status.assetdep.ToString());


                    // SALE DATE TAKEN PURPOSE
                    DataTable dtsd = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_TransferMaker", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "SALEDETAILS";
                    cmd.Parameters.Add("@assetid", SqlDbType.VarChar).Value = obj.assetdetgid.ToString();
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dtsd);
                    foreach (DataRow rowdtsd in dtsd.Rows)
                    {
                        obj.soaSaledate = rowdtsd["soaheader_sale_date"].ToString();

                    }

                    string soacaptilisationdat = Convert.ToString("SYSDATETIME()");
                    DateTime soacaptilisationda = DateTime.Now;
                    DateTime dtTillDate = Convert.ToDateTime(obj.soaSaledate.ToString()); ;
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
                    Int32 assetdetid = obj.assetdetgid;
                    // decimal sumassetdep = 0;
                    //decimal linewdv = 0;
                    //decimal rectificationamt = 0;
                    // decimal linePL = 0;


                    DataTable dtwdv = new DataTable();
                    cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
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
                                                                    dDepDevRate), 2);





                    rectificationamt = depamt - sumassetdep;

                    //DataTable dtgroup = new DataTable();
                    //cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@action", "Groupid");
                    //cmd.Parameters.AddWithValue("@assetgid", obj.assetdetgid);
                    //da = new SqlDataAdapter(cmd);
                    //da.Fill(dtgroup);
                    //foreach (DataRow rowgroup in dtgroup.Rows)
                    //{
                    //    status.assetdetgroupid = Convert.ToInt64(rowgroup["assetdetails_asset_groupid"].ToString());
                    //}
                    //if (sDepType == "SLM" || sDepType == "")
                    //{
                    //    sDepType = "S";
                    //}
                    //else if (sDepType == "LPM")
                    //{
                    //    sDepType = "L";
                    //}
                    //string[,] codesdep = new string[,]
                    //{
                    //    {"depreciation_amount", rectificationamt.ToString()},
                    //    {"depreciation_assetdet_gid", obj.assetdetgid.ToString()},
                    //    {"depreciation_month", Convert.ToString("SYSDATETIME()")},
                    //    {"depreciation_asset_groupid", obj.assetdetgroupid.ToString()},
                    //    {"depreciation_asset_owner", "F"},
                    //    {"depreciation_type",sDepType.ToString()},
                    //    {"depreciation_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                    //    {"depreciation_insert_date", Convert.ToString("SYSDATETIME()")}
                    //};
                    //string insertcmn = objCommonIUD.InsertCommon(codesdep, "fa_trn_tdepreciation");

                }

                string[,] codes1 = new string[,]
                     {
                        {"soapayment_chq_no", status.soapayChqno.ToString()},
                        {"soapayment_amount", status.soapayAmount.ToString()},
                        {"soapayment_update_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                        {"soapayment_update_date",Convert.ToString("SYSDATETIME()")}
                         
                       // {"soapayment_realizationdate",convertoDate(status.soapayRealizationdate.ToString())},
                     };
                string[,] whrcol = new string[,]
                     {  
                         {"soapayment_soaheader_gid", status.soaGid.ToString()}, 
                     };
                string tblname = "fa_trn_tsoapayment";
                string updcomn = objCommonIUD.UpdateCommon(codes1, whrcol, tblname);

                //soaheader
                DataTable dt1 = new DataTable();
                string wait = "WAITING FOR APPROVAL";
                cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = wait.ToString();
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "status";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt1);
                foreach (DataRow row1 in dt1.Rows)
                    status.soaStatus = row1["status_flag"].ToString();
                string[,] codesHead = new string[,]
                     {
                         {"soaheader_sale_date",convertoDate(status.soaSaledate.ToString())},
                         //{"soaheader_vat_amount", status.soaVatamt.ToString()},
                       //  {"soaheader_vat_percentage", status.soaVatper.ToString()},
                         {"soaheader_vendor_name", status.soaVendorname.ToString()},
                         {"soaheader_vendor_address", status.soavendoradd.ToString() },
                         {"soaheader_sale_status",   status.soaStatus .ToString()},
                         {"soaheader_update_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                         {"soaheader_update_date",Convert.ToString("SYSDATETIME()")}
                         
                     };
                string[,] whrcolHead = new string[,]
                     {  
                         {" soaheader_gid", status.soaGid.ToString()}, 
                     };
                string tblname1 = "fa_trn_tsoaHeader";
                string updcommn = objCommonIUD.UpdateCommon(codesHead, whrcolHead, tblname1);

                //queue
                DataTable dt2 = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_getflowdetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@usergroup", SqlDbType.VarChar).Value = "SOACHK";
                cmd.Parameters.Add("@querytype", SqlDbType.VarChar).Value = "groupname";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt2);
                string grpid = "";
                foreach (DataRow row1 in dt2.Rows)
                    grpid = row1["rolegroup_gid"].ToString();
                string[,] col = new string[,]
	                        {
                                     {"queue_date",Convert.ToString(convertoDate(objCommonIUD.GetCurrentDate()))},
                                     {"queue_ref_flag",objidm.GetRef("SOA").ToString()},
                                     {"queue_ref_gid",  status.soaGid.ToString()},
                                     {"queue_action_for", "A"},
                                     {"queue_from",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                                     {"queue_to_type","G" },
                                     {"queue_to",grpid},
                                     {"queue_prev_gid","0"},
                                     {"queue_action_status","0"},
                                     {"queue_isremoved","N"}                                     
	                      };
                string inst = objCommonIUD.InsertCommon(col, "iem_trn_tqueue");
                ////soadetail
                //string[,] codesdet = new string[,]
                //     {
                //        {"soadet"},
                //     };
                //string[,] whrcoldet = new string[,]
                //     {  
                //         {" soadetail_soahead_gid", status.soaGid.ToString()}, 
                //     };
                //string tblname2 = "fa_trn_tsoadetail";
                //string updcommnsoadet = objCommonIUD.UpdateCommon(codesdet, whrcoldet, tblname2);
                // string view = "asdf";
                return "success";

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
                //throw ex;
            }
            finally { con.Close(); }
        }

        public SaleMakerModel GetSaleDetailFORREJECT(string salegid)
        {
            List<SaleMakerModel> objModel = new List<SaleMakerModel>();
            try
            {
                SaleMakerModel obj = new SaleMakerModel();
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "SASelectViewReject";
                cmd.Parameters.Add("@Salegid", SqlDbType.VarChar).Value = salegid;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                obj.soaSalenumber = salegid;
                decimal str = 0;
                foreach (DataRow row in dt.Rows)
                {
                    obj = new SaleMakerModel();
                    //obj.soaSalenumber = row["soaheader_sale_number"].ToString();
                    obj.assetdetDetid = row["assetdetails_assetdet_id"].ToString();
                    obj.soadetgid = Convert.ToInt32(row["soadetail_gid"].ToString());
                    obj.assetcategory = Convert.ToString(string.IsNullOrEmpty(row["assetcategory_name"].ToString()) ? "0" : row["assetcategory_name"]);
                    obj.assetsubcategory = Convert.ToString(string.IsNullOrEmpty(row["asset_description"].ToString()) ? "0" : row["asset_description"]);
                    obj.soadetSalevalue = Convert.ToDecimal(row["soadetail_sale_value"].ToString());
                    obj.assetdetCode = SAassetccode(row["assetdetails_asset_code"].ToString());
                    obj.assetdetDescription = row["asset_description"].ToString();
                    //obj.soadetSalevalue = Convert.ToInt32(row["soadetail_sale_value"].ToString());
                    obj.saleamt = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_assetdet_value"].ToString()) ? "0" : row["soadetail_assetdet_value"]);
                    obj.soadetVatvalue = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_vat_value"].ToString()) ? "0" : row["soadetail_vat_value"]);
                    obj.soadetWdvalue = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_wd_value"].ToString()) ? "0" : row["soadetail_wd_value"]);
                    obj.soadetProfitloss = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetial_pl_value"].ToString()) ? "0" : row["soadetial_pl_value"]);
                    //obj.soaRectifAmt = objidm.getRetificationAmount(row["assetdetails_gid"].ToString()); //Muthu 20-09-2016
                    obj.soaRectifAmt = getRetificationAmountsale(row["assetdetails_gid"].ToString());
                    obj.hsn_code = Convert.ToString(string.IsNullOrEmpty(row["hsn_code"].ToString()) ? "0" : row["hsn_code"]);
                    obj.hsn_gid = Convert.ToInt32(string.IsNullOrEmpty(row["soadetail_hsn_gid"].ToString()) ? 0 : row["soadetail_hsn_gid"]);
                    obj.branch_name = Convert.ToString(string.IsNullOrEmpty(row["branch_name"].ToString()) ? "0" : row["branch_name"]);
                    obj.cgst_amount = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_cgst_amount"].ToString()) ? "0" : row["soadetail_cgst_amount"]);
                    obj.sgst_amount = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_sgst_amount"].ToString()) ? "0" : row["soadetail_sgst_amount"]);
                    obj.igst_amount = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_igst_amount"].ToString()) ? "0" : row["soadetail_igst_amount"]);
                    obj.cgcess_amount = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_cgcess_amount"].ToString()) ? "0" : row["soadetail_cgcess_amount"]);
                    obj.sgcess_amount = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_sgcess_amount"].ToString()) ? "0" : row["soadetail_sgcess_amount"]);
                    obj.igcess_amount = Convert.ToDecimal(string.IsNullOrEmpty(row["soadetail_igcess_amount"].ToString()) ? "0" : row["soadetail_igcess_amount"]);
                    obj.assetgid = Convert.ToInt32(string.IsNullOrEmpty(row["asset_gid"].ToString()) ? "0" : row["asset_gid"]);
                    obj.assetdetgid = Convert.ToInt32(string.IsNullOrEmpty(row["assetdetails_gid"].ToString()) ? "0" : row["assetdetails_gid"]);
                    obj.HsnDetails = new SelectList(GetHsnDetails(obj.assetgid, obj.soadetgid).ToList(), "hsn_gid", "hsn_code");

                    objModel.Add(obj);
                    str = str + obj.soadetSalevalue;
                }
                obj.TModel = objModel.ToList();
                obj.soaSalevalue = str;
                return obj;
            }
            catch (Exception ex)
            {
                //objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                //return objModel;
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }


        public IEnumerable<VatModel> GetState()
        {
            List<VatModel> objvatper = new List<VatModel>();
            try
            {
                VatModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_GstSaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@action", "state");
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new VatModel();
                    objModel.vatid = dt.Rows[i]["state_gid"].ToString();
                    objModel.vatstate = dt.Rows[i]["state_name"].ToString();
                    objvatper.Add(objModel);
                }
                return objvatper;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objvatper;
            }
            finally
            {
                con.Close();
            }
        }

        public List<SearchCustomer> GetCustomerList()
        {
            List<SearchCustomer> objSearchEmployee = new List<SearchCustomer>();
            try
            {

                SearchCustomer objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_iem_mst_tcustomerdetail", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getcustomerlist";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SearchCustomer();
                    objModel.customer_gid = Convert.ToInt32(dt.Rows[i]["customer_gid"].ToString());
                    objModel.customer_code = Convert.ToString(dt.Rows[i]["customer_code"].ToString());
                    objModel.customer_name = Convert.ToString(dt.Rows[i]["customer_name"].ToString());
                    objModel.customer_address = Convert.ToString(dt.Rows[i]["customer_address"].ToString());
                    objModel.state_name = Convert.ToString(dt.Rows[i]["state_name"].ToString());
                    objModel.pincode_code = Convert.ToString(dt.Rows[i]["pincode_code"].ToString());
                    objModel.Gstin_number = Convert.ToString(dt.Rows[i]["Gstin_number"].ToString());
                    objModel.state_gid = Convert.ToInt32(dt.Rows[i]["state_gid"].ToString());
                    objModel.pincode_gid = Convert.ToInt32(dt.Rows[i]["pincode_gid"].ToString());
                    objModel.district_gid = Convert.ToInt32(dt.Rows[i]["district_gid"].ToString());
                    objModel.district_name = Convert.ToString(dt.Rows[i]["district_name"].ToString());
                    objModel.contact_gid = Convert.ToInt32(dt.Rows[i]["customercontact_gid"].ToString());
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

        public DataTable GetGstCustomerDetails(SaleMakerModel salemodel)
        {
            GetConnection();
            cmd = new SqlCommand("pr_ifams_trn_tsalemakergst", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetCustomerdetails";
            cmd.Parameters.Add("@Salegid", SqlDbType.Int).Value = salemodel.soaGid;
            cmd.Parameters.Add("@IsGstCharged", SqlDbType.VarChar).Value = salemodel.gstcharged;
            cmd.Parameters.Add("@Customercode", SqlDbType.VarChar).Value = salemodel.customer_code;
            cmd.Parameters.Add("@ReciverPincode", SqlDbType.VarChar).Value = salemodel.Pincode;
            cmd.Parameters.Add("@HsnDetails", SqlDbType.VarChar).Value = salemodel.hsn_description;
            DataTable dt = new DataTable();
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
                return dt;
            else
                return null;
        }

        public string UpdateSOAHsndetail(SaleMakerModel saledetail)
        {
            try
            {

                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_tsalemakergst", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@HsnDetails", SqlDbType.VarChar).Value = saledetail.hsn_description;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "UpdateHsn";
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
            return "success";
        }



        public IEnumerable<SearchCustomer> SelectCustomerSearch(string CustomerCode, string CustomerName)
        {
            List<SearchCustomer> customer = new List<SearchCustomer>();
            try
            {
                DataTable dt = new DataTable();
                SearchCustomer objModel;
                GetConnection();
                cmd = new SqlCommand("pr_ifams_trn_tautosearchgst", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@pcustomercode", SqlDbType.VarChar).Value = CustomerCode;
                cmd.Parameters.Add("@pcustomername", SqlDbType.VarChar).Value = CustomerName;
                //cmd.Parameters.Add("@DEPARTMENT", SqlDbType.VarChar).Value = EmployeeName;
                cmd.Parameters.Add("@paction", SqlDbType.VarChar).Value = "customersearch";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new SearchCustomer();
                    objModel.customer_gid = Convert.ToInt32(row["customer_gid"].ToString());
                    objModel.customer_code = row["customer_code"].ToString();
                    objModel.customer_name = row["customer_name"].ToString();
                    objModel.pincode_code = row["pincode_code"].ToString();
                    objModel.state_name = row["state_name"].ToString();

                    customer.Add(objModel);
                }
                return customer;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return customer;
            }
            finally
            {
                con.Close();
            }
        }

        public int GetHsnid(string hsn_code)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_ifams_trn_SaleMaker", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = hsn_code;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetHsnid";
                int result = (int)cmd.ExecuteScalar();
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
                //throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public DataTable AssetSaveDetails(Models.SaleMakerModel status)
        {
            string Result = string.Empty;
            DataTable dt = new DataTable();
            try
            {

                GetConnection();
                cmd = new SqlCommand("pr_Set_Asset_SaleDetailsApproved", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@prSoaNumber", SqlDbType.VarChar).Value = status.soaSalenumber;
                cmd.Parameters.Add("@prSoaStatus", SqlDbType.VarChar).Value = status.soaStatus;
                cmd.Parameters.Add("@prSoaRemarks", SqlDbType.VarChar).Value = status.action_remark;
                cmd.Parameters.Add("@prSoaLogginId", SqlDbType.Int).Value = Convert.ToString(objCmnFunctions.GetLoginUserGid());
                cmd.Parameters.Add("@prSoaPayChequeNo", SqlDbType.VarChar).Value = status.soapayChqno;
                cmd.Parameters.Add("@prSoaPayAmount", SqlDbType.VarChar).Value = status.soapayAmount;
                cmd.Parameters.Add("@prSoaAction", SqlDbType.VarChar).Value = status.action_status;
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

                //throw ex;
            }
            finally { con.Close(); }
            return dt;
        }

    }
}


