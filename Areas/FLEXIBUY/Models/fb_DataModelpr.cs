using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IEM.Common;
using System.Data;
using IEM.Areas.FLEXIBUY.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Globalization;

namespace IEM.Areas.FLEXIBUY.Models
{
    public class fb_DataModelpr : Irepositorypr
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        DataTable dt;
        CommonIUD objcommon = new CommonIUD();
        ErrorLog objErrorLog = new ErrorLog();
        public void getconnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }
        public List<poRaising> getpoSummary()
        {
            DataTable objtable = new DataTable();
            List<poRaising> objraiser = new List<poRaising>();
            poRaising objDetails;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_poheader", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "select");
                cmd.Parameters.AddWithValue("@logingid", objCmnFunctions.GetLoginUserGid().ToString());
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objDetails = new poRaising();
                    objDetails.slno = i + 1;
                    objDetails.poDetgid = Convert.ToInt32(objtable.Rows[i]["poheader_gid"]);
                    objDetails.poDate = Convert.ToString(objtable.Rows[i]["poheader_date"]);
                    objDetails.poRefNo = objtable.Rows[i]["poheader_pono"].ToString();
                    objDetails.vendorName = objtable.Rows[i]["supplierheader_name"].ToString();
                    objDetails.status = objtable.Rows[i]["status_name"].ToString();
                    objDetails.poAmount = objtable.Rows[i]["poheader_over_total"].ToString();
                    objDetails.poVersion = objtable.Rows[i]["poheader_version"].ToString();
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
                con.Close();
            }
        }
        public List<poraiser> SCNWODetails(int woheadgid)
        {
            DataTable objtable = new DataTable();
            List<poraiser> objraiserWO = new List<poraiser>();
            poraiser objDetails;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_SCNWORelease", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "SCNWODetails");
                cmd.Parameters.AddWithValue("@WOheadgid", woheadgid.ToString());
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objDetails = new poraiser();
                    objDetails.poheadGid = Convert.ToInt32(objtable.Rows[i]["poheader_gid"]);
                    objDetails.prodservicegroup = Convert.ToString(objtable.Rows[i]["productgroupname"]);
                    objDetails.cbfno = objtable.Rows[i]["cbfheader_cbfno"].ToString();
                    objDetails.prodservicecode = objtable.Rows[i]["prodservice_code"].ToString();
                    objDetails.prodservicename = objtable.Rows[i]["prodservice_name"].ToString();
                    objDetails.prodservicedesc = objtable.Rows[i]["podetails_desc"].ToString();
                    //objDetails.quantity = Convert.ToInt32(objtable.Rows[i]["podetails_qty"]);
                    //objDetails.unitprice = Convert.ToInt32(objtable.Rows[i]["podetails_unitprice"]);
                    objDetails.total = Convert.ToInt32(objtable.Rows[i]["podetails_total"]);
                    //  objDetails.servicemonth = objtable.Rows[i]["podetails_serv_month"].ToString();
                    objraiserWO.Add(objDetails);
                }
                return objraiserWO;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objraiserWO;
            }
            finally
            {
                con.Close();
            }
        }
        public List<poRaising> SCNWORelease(string request)
        {
            DataTable objtable = new DataTable();
            List<poRaising> objraiserWO = new List<poRaising>();
            poRaising objDetails;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_SCNWORelease", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "select");
                // cmd.Parameters.AddWithValue("@logingid", objCmnFunctions.GetLoginUserGid().ToString());
                cmd.Parameters.AddWithValue("@requestfor", request.ToString());
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objDetails = new poRaising();
                    objDetails.poDetgid = Convert.ToInt32(objtable.Rows[i]["poheader_gid"]);
                    objDetails.poDate = Convert.ToString(objtable.Rows[i]["poheader_date"]);
                    objDetails.poRefNo = objtable.Rows[i]["poheader_pono"].ToString();
                    objDetails.vendorName = objtable.Rows[i]["supplierheader_name"].ToString();
                    objDetails.status = objtable.Rows[i]["status_name"].ToString();
                    objDetails.poAmount = objtable.Rows[i]["poheader_over_total"].ToString();
                    objraiserWO.Add(objDetails);
                }
                return objraiserWO;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objraiserWO;
            }
            finally
            {
                con.Close();
            }
        }
        public List<poraiser> SCNWOReleaseHead(int woheadgid)
        {
            DataTable objtable = new DataTable();
            List<poraiser> objraiserWO = new List<poraiser>();
            poraiser objDetails;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_SCNWORelease", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "SCNReleaseHeader");
                // cmd.Parameters.AddWithValue("@logingid", objCmnFunctions.GetLoginUserGid().ToString());
                cmd.Parameters.AddWithValue("@WOheadgid", woheadgid.ToString());
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objDetails = new poraiser();
                    objDetails.podetGid = Convert.ToInt32(objtable.Rows[i]["poheader_gid"]);
                    objDetails.podate = Convert.ToString(objtable.Rows[i]["poheader_date"]);
                    objDetails.porefno = objtable.Rows[i]["poheader_pono"].ToString();
                    objDetails.tempName = objtable.Rows[i]["emp"].ToString();
                    objDetails.department = objtable.Rows[i]["requestfor_name"].ToString();
                    objDetails.vendorName = objtable.Rows[i]["supplierheader_name"].ToString();
                    objDetails.itType = objtable.Rows[i]["poheader_ittype"].ToString();
                    objDetails.vendorNote = objtable.Rows[i]["poheader_vendor_note"].ToString();
                    objraiserWO.Add(objDetails);
                }
                return objraiserWO;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objraiserWO;
            }
            finally
            {
                con.Close();
            }
        }
        public List<vendorselection> getvendorselection()
        {
            List<vendorselection> lstvendor = new List<vendorselection>();
            vendorselection ObjVendor;
            DataTable objtable = new DataTable();
            try
            {
                getconnection();
                cmd = new SqlCommand("select supplierheader_gid,supplierheader_name,supplierheader_suppliercode from asms_trn_tsupplierheader where supplierheader_status='APPROVED'  and supplierheader_isremoved='N' order by supplierheader_gid", con);
                da = new SqlDataAdapter(cmd);
                DataTable ObjDt = new DataTable();
                da.Fill(ObjDt);
                if (ObjDt.Rows.Count > 0)
                {
                    for (int i = 0; i < ObjDt.Rows.Count; i++)
                    {
                        ObjVendor = new vendorselection();
                        ObjVendor.slno = i + 1;
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
                con.Close();
            }
        }
        public string GetGroupForUser()
        {
            string result = string.Empty;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_checkUserForGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "select");
                cmd.Parameters.AddWithValue("@logingid", Convert.ToInt32(objCmnFunctions.GetLoginUserGid().ToString()));
                result = (string)cmd.ExecuteScalar();
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
        public string GetGroupForUserPO() 
        {
            string result = string.Empty;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_checkUserForGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectforPO");
                cmd.Parameters.AddWithValue("@logingid", Convert.ToInt32(objCmnFunctions.GetLoginUserGid().ToString()));
                result = (string)cmd.ExecuteScalar();
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
        public string GetGroupForUserWO() 
        {
            string result = string.Empty;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_checkUserForGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectforWO");
                cmd.Parameters.AddWithValue("@logingid", Convert.ToInt32(objCmnFunctions.GetLoginUserGid().ToString()));
                result = (string)cmd.ExecuteScalar();
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
        public string GetReqGroup()
        {
            string result = string.Empty;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_checkUserForGroup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectforgroup");
                cmd.Parameters.AddWithValue("@logingid", Convert.ToInt32(objCmnFunctions.GetLoginUserGid().ToString()));
                result = (string)cmd.ExecuteScalar();
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

        public List<poRaising> getStatus()
        {
            DataTable objtable = new DataTable();
            List<poRaising> objraiser = new List<poRaising>();
            poRaising objDetails;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_poheader", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "status");
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                foreach (DataRow row in objtable.Rows)
                {
                    objDetails = new poRaising();
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
                con.Close();
            }
        }

        public string getStatusName2(int statusgid)
        {
            try
            {
                getconnection();
                poRaising objstatus = new poRaising();
                cmd = new SqlCommand("pr_fb_trn_poheader", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@statusgid", statusgid);
                cmd.Parameters.AddWithValue("@actionName", "statusname");
                objstatus.statusname = Convert.ToString(cmd.ExecuteScalar());
                return objstatus.statusname;
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
        public string Getchckdetails(string statusgid)
        {
            try
            {
                string status = "";
                DataTable objtable = new DataTable();
                getconnection();
                poRaising objstatus = new poRaising();
                cmd = new SqlCommand("pr_fb_trn_poheader", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@statusgid", statusgid);
                cmd.Parameters.AddWithValue("@actionName", "chckvalue");
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                if (objtable.Rows.Count > 0)
                {
                    status = "AlReady_Exists";
                }
                else
                {
                    status = "Not_There";
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
                con.Close();
            }
        }
        public string getStatusName1(int statusgid)
        {
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_poheader", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@statusgid", statusgid);
                cmd.Parameters.AddWithValue("@actionName", "statusname");
                return Convert.ToString(cmd.ExecuteScalar());
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

                return objcommon.ProcedureCommon(parameter, "pr_fb_trn_poheader");
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

        public List<cbfselection> getcbfheader(string cbfmode)
        {
            DataTable objtable = new DataTable();
            List<cbfselection> objselection = new List<cbfselection>();
            cbfselection objDetails;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_cbfselection", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "select");
                cmd.Parameters.AddWithValue("@requestforname", cbfmode.ToString());
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objDetails = new cbfselection();
                    objDetails.slno = i + 1;
                    objDetails.cbfheadgid = Convert.ToInt64(objtable.Rows[i]["cbfheader_gid"]);
                    objDetails.cbfmode = Convert.ToString(objtable.Rows[i]["cbfheader_mode"]);
                    objDetails.cbfno = Convert.ToString(objtable.Rows[i]["cbfheader_cbfno"]);
                    objDetails.cbfdate = objtable.Rows[i]["startdate"].ToString();
                    objDetails.cbfEnddate = objtable.Rows[i]["enddate"].ToString();
                    objDetails.department = objtable.Rows[i]["requestfor_name"].ToString();
                    objDetails.finconBudget = objtable.Rows[i]["cbfheader_isbudgeted"].ToString();
                    objDetails.cbfamount = Convert.ToDecimal(objtable.Rows[i]["cbfheader_cbfamt"].ToString());
                    objDetails.projectowner = objtable.Rows[i]["projectowner_name"].ToString();
                    objDetails.deviationamount = Convert.ToDecimal(objtable.Rows[i]["cbfheader_Devi_amt"].ToString());
                    objDetails.status = objtable.Rows[i]["status_name"].ToString();
                    objDetails.description = objtable.Rows[i]["cbfheader_desc"].ToString();
                    objDetails.regularlater = objtable.Rows[i]["cbfregularlater"].ToString();
                    objselection.Add(objDetails);
                }
                return objselection;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objselection;
            }
            finally
            {
                con.Close();
            }
        }
        public List<cbfselection> getrequestfor()
        {
            DataTable objtable = new DataTable();
            List<cbfselection> objlist = new List<cbfselection>();
            cbfselection objrequest;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_cbfselection", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectlist");
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                foreach (DataRow row in objtable.Rows)
                {
                    objrequest = new cbfselection();
                    objrequest.requestforgid = Convert.ToInt32(row["requestfor_gid"]);
                    objrequest.requestforname = row["requestfor_name"].ToString();
                    objlist.Add(objrequest);
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
                con.Close();
            }
        }
        public string getrequestName(int requestforgid)
        {
            try
            {
                getconnection();
                string[,] parameter = new string[,]
                {
                    {"@requestforgid",requestforgid.ToString()},
                    {"@actionName","requestname"},
                };

                return objcommon.ProcedureCommon(parameter, "pr_fb_trn_cbfselection");
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

        public DataTable getTable(cbfselection objcbf)
        {
            DataTable dt = new DataTable();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_cbfselection", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectdetail");
                cmd.Parameters.AddWithValue("@cbfheadgid", objcbf.cbfheadgid);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (!dt.Columns.Contains("cbftotal_amount"))
                    dt.Columns.Add("cbftotal_amount");
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {

                        dt.Rows[i]["cbftotal_amount"] = Convert.ToDecimal(Convert.ToDecimal(dt.Rows[i]["cbfdetails_qty"]) * Convert.ToDecimal(dt.Rows[i]["cbfdetails_unitprice"]));

                    }
                }
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
        public List<cbfdetail> getcbflist(DataTable dt)
        {
            cbfdetail objcbfdetails;
            List<cbfdetail> objlist = new List<cbfdetail>();
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objcbfdetails = new cbfdetail();
                    objcbfdetails.slno = i + 1;
                    objcbfdetails.cbfheadgid = Convert.ToInt64(dt.Rows[i]["cbfdetails_cbfhead_gid"].ToString());
                    // objcbfdetails.cbfheadAmount = Convert.ToDecimal(rows["cbfheader_cbfamt"].ToString());
                    objcbfdetails.cbfdetailgid = Convert.ToInt64(dt.Rows[i]["cbfdetails_gid"].ToString());
                    objcbfdetails.cbfno = dt.Rows[i]["cbfheader_cbfno"].ToString();
                    objcbfdetails.productCode = dt.Rows[i]["prodservice_code"].ToString();
                    objcbfdetails.productName = dt.Rows[i]["prodservicename"].ToString();
                    objcbfdetails.productDesc = dt.Rows[i]["prodservice_description"].ToString();
                    objcbfdetails.uom = dt.Rows[i]["uom_code"].ToString();
                    objcbfdetails.quantity = Convert.ToInt32(dt.Rows[i]["cbfdetails_qty"].ToString());
                    objcbfdetails.unitPrice = Convert.ToDecimal(dt.Rows[i]["cbfdetails_unitprice"].ToString());
                    //  objcbfdetails.totalAmount = Convert.ToDecimal(dt.Rows[i]["cbftotal_amount"].ToString());
                    objcbfdetails.totalAmount = Convert.ToDecimal(dt.Rows[i]["cbftotal_amount"].ToString());
                    objcbfdetails.vendorName = dt.Rows[i]["supplierheader_name"].ToString();
                    objlist.Add(objcbfdetails);
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
                con.Close();
            }
        }
        public DataTable getcbfdetails(string cbfno)
        {
            DataTable objtable = new DataTable();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_cbfselection", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectforpo");
                cmd.Parameters.AddWithValue("@cbfdetailgid", cbfno);
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                //if (!objtable.Columns.Contains("cbftotal_amount"))
                //    objtable.Columns.Add("cbftotal_amount");
                if (objtable.Rows.Count > 0)
                {
                    for (int i = 0; i < objtable.Rows.Count; i++)
                    {

                        objtable.Rows[i]["cbfdetails_totalamt"] = Convert.ToDecimal(Convert.ToDecimal(objtable.Rows[i]["cbfdetails_qty"]) * Convert.ToDecimal(objtable.Rows[i]["cbfdetails_unitprice"]));

                    }
                }
                return objtable;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objtable;
            }
            finally
            {
                con.Close();
            }
        }
        public List<poraiser> getpolist(DataTable dt)
        {
            List<poraiser> objlist = new List<poraiser>();
            poraiser objraiser;
            try
            {
                getconnection();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objraiser = new poraiser();
                    objraiser.prodservicegroup = dt.Rows[i]["productgroup"].ToString();
                    objraiser.prodservicecode = dt.Rows[i]["prodservice_code"].ToString();
                    objraiser.prodservicename = dt.Rows[i]["prodservicename"].ToString();
                    objraiser.prodservicedesc = dt.Rows[i]["prodservice_description"].ToString();
                    objraiser.prodservicegid = Convert.ToInt32(dt.Rows[i]["prodservice_gid"].ToString());
                    objraiser.uomgid = Convert.ToInt32(dt.Rows[i]["uom_gid"].ToString());
                    objraiser.vendorgid = Convert.ToInt32(dt.Rows[i]["supplierheader_gid"].ToString());
                    objraiser.requestforgid = Convert.ToInt32(dt.Rows[i]["requestfor_gid"].ToString());
                    objraiser.cbfdetailsgid = Convert.ToInt32(dt.Rows[i]["cbfdetails_gid"].ToString());
                    objraiser.units = dt.Rows[i]["uom_code"].ToString();
                    objraiser.actQuantity = Convert.ToDecimal(dt.Rows[i]["cbfdetails_qty"]);
                    objraiser.quantity = Convert.ToDecimal(dt.Rows[i]["cbfdetails_dumqty"]);
                    objraiser.unitprice = Convert.ToDecimal(dt.Rows[i]["cbfdetails_unitprice"].ToString());
                    //objraiser.baseamount = Convert.ToDecimal(dt.Rows[i]["cbftotal_amount"].ToString());
                    objraiser.baseamount = Convert.ToDecimal(dt.Rows[i]["cbfdetails_totalamt"].ToString());
                    // objraiser.unutilizedAmount = Convert.ToDecimal(dt.Rows[i]["unutilized_amount"].ToString());
                    objraiser.total = objraiser.baseamount;
                    objraiser.department = dt.Rows[i]["requestfor_name"].ToString();
                    objraiser.vendorName = dt.Rows[i]["supplierheader_name"].ToString();
                    //objraiser.devamount = Convert.ToDecimal(dt.Rows[i]["cbfheader_Devi_amt"]);
                    objraiser.cbfheadAmount = Convert.ToDecimal(dt.Rows[i]["cbfheader_cbfamt"]);
                    if (dt.Columns.Count > 20)
                    {
                        if (dt.Rows[i]["discount"].ToString() == null || dt.Rows[i]["discount"].ToString() == "")
                        {
                            objraiser.discount = 0;
                            dt.Rows[i]["discount"] = objraiser.discount;
                        }
                        else
                        {
                            objraiser.discount = (Convert.ToDecimal(dt.Rows[i]["discount"].ToString()));
                        }

                        if (dt.Rows[i]["tax1"].ToString() == null || dt.Rows[i]["tax1"].ToString() == "")
                        {
                            objraiser.tax1 = 0;
                            dt.Rows[i]["tax1"] = objraiser.tax1;
                        }
                        else
                        {
                            objraiser.tax1 = (Convert.ToDecimal(dt.Rows[i]["tax1"].ToString()));
                        }
                        if (dt.Rows[i]["tax2"].ToString() == null || dt.Rows[i]["tax2"].ToString() == "")
                        {
                            objraiser.tax2 = 0;
                            dt.Rows[i]["tax2"] = objraiser.tax2;
                        }
                        else
                        {
                            objraiser.tax2 = (Convert.ToDecimal(dt.Rows[i]["tax2"].ToString()));
                        }
                        if (dt.Rows[i]["tax3"].ToString() == null || dt.Rows[i]["tax3"].ToString() == "")
                        {
                            objraiser.tax3 = 0;
                            dt.Rows[i]["tax3"] = objraiser.tax3;
                        }
                        else
                        {
                            objraiser.tax3 = (Convert.ToDecimal(dt.Rows[i]["tax3"].ToString()));
                        }
                        //if (dt.Rows[i]["total"].ToString() == null || dt.Rows[i]["total"].ToString() == "")
                        //{
                        //    objraiser.total = 0;

                        //}
                        //else
                        //{
                        objraiser.total = (Convert.ToDecimal(dt.Rows[i]["total"].ToString()));
                    }
                    objlist.Add(objraiser);
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
                con.Close();
            }
        }

        //projectowner List
        public List<poraiser> GetProjectOwnerList()
        {
            DataTable objTable = new DataTable();
            List<poraiser> objlist = new List<poraiser>();
            poraiser objProject;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_cbfselection", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "projmanagerlist");
                da = new SqlDataAdapter(cmd);
                da.Fill(objTable);
                foreach (DataRow row in objTable.Rows)
                {
                    objProject = new poraiser();
                    objProject.projmanagergid = Convert.ToInt32(row["projectowner_gid"]);
                    objProject.projmanagername = row["projectowner_name"].ToString();
                    objlist.Add(objProject);
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
                con.Close();
            }
        }

        public string GetLoginUser()
        {
            poraiser objraiser = new poraiser();
            DataTable dt = new DataTable();
            try
            {

                dt = objCmnFunctions.GetLoginUserDetails(objCmnFunctions.GetLoginUserGid());
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objraiser.raisedby = dt.Rows[i]["employee_code"].ToString() + "-" + dt.Rows[i]["employee_name"].ToString();
                }
                return objraiser.raisedby;
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
        public List<shipment> getshipmentType()
        {
            DataTable objtable = new DataTable();
            List<shipment> objlist = new List<shipment>();
            shipment objship;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_cbfselection", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectship");
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                foreach (DataRow row in objtable.Rows)
                {
                    objship = new shipment();
                    objship.shipmentgid = row["shipment_gid"].ToString();
                    objship.shipmentName = row["shipment_type"].ToString();
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
                con.Close();
            }
        }

        public List<shipment> getbranchdetails()
        {

            DataTable objtable = new DataTable();
            List<shipment> objlist = new List<shipment>();
            shipment objship;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_cbfselection", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectbranch");
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
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
                con.Close();
            }
        }

        public List<shipment> getbranchdetailsForCwip()
        {
            DataTable objtable = new DataTable();
            List<shipment> objlist = new List<shipment>();
            shipment objship;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_grnSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "branchforcwip");
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
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
                    objship.empgid = Convert.ToInt32(objtable.Rows[i]["inchargegid"].ToString());
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
                con.Close();
            }
        }
        public List<shipment> GetShipment(DataTable dt)
        {

            shipment objShipment;
            List<shipment> lLstShipment = new List<shipment>();
            try
            {
                //int count = 0;
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objShipment = new shipment()
                        {
                            SrNo = Convert.ToInt32(dt.Rows[i]["Slno"].ToString()),
                            branchgid = Convert.ToInt32(dt.Rows[i]["branchgid"].ToString()),
                            shipmentgid = dt.Rows[i]["shipmenttype"].ToString(),
                            cbfdetGid = Convert.ToInt32(dt.Rows[i]["cbfdetgid"]),
                            branchName = dt.Rows[i]["branchName"].ToString(),
                            address = dt.Rows[i]["address"].ToString(),
                            location = dt.Rows[i]["location"].ToString(),
                            inchargeID = dt.Rows[i]["inchargeID"].ToString(),
                            branchCode = dt.Rows[i]["branchCode"].ToString(),
                            shippedqty = Convert.ToDecimal(dt.Rows[i]["Quantity"].ToString())
                        };
                        lLstShipment.Add(objShipment);

                    }
                }
                return lLstShipment;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return lLstShipment;
            }
            finally
            {
                con.Close();
            }
        }

        //Getting the shipment type from database for checking validation
        public string GetShipmentStatus(string validate, string action)
        {
            string result = "";
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_shipmentexcelvalidate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@chkdata", SqlDbType.VarChar).Value = validate.ToString();
                cmd.Parameters.Add("@result", SqlDbType.VarChar).Value = action;
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
        public List<BulkUpload> GetUploadSummary(DataTable objTable)
        {
            BulkUpload objBulk;
            List<BulkUpload> lLstBulkUpload = new List<BulkUpload>();
            try
            {
                getconnection();
                if (objTable.Rows.Count > 0)
                {
                    for (int i = 0; i < objTable.Rows.Count; i++)
                    {
                        objBulk = new BulkUpload()
                        {
                            slNo = objTable.Rows[i]["SNo"].ToString(),
                            shipmentType = objTable.Rows[i]["Shipment Type"].ToString(),
                            branchCode = objTable.Rows[i]["Branch Code"].ToString(),
                            quantity = Convert.ToInt32(objTable.Rows[i]["Quantity"].ToString()),
                        };
                        lLstBulkUpload.Add(objBulk);
                    }
                }
                return lLstBulkUpload;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return lLstBulkUpload;
            }
            finally
            {
                con.Close();
            }
        }
        public List<shipment> ExcelToShipment(DataTable dt)
        {
            shipment objShipment;
            int count = 0;
            List<shipment> lLstShipment = new List<shipment>();
            try
            {
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objShipment = new shipment();
                        count = Convert.ToInt32(HttpContext.Current.Session["count"]);
                        objShipment.SrNo = count + 1;
                        if (dt.Rows[i]["ShipmentType"].ToString() == "DIRECT")
                            objShipment.shipmentgid = "1";
                        if (dt.Rows[i]["ShipmentType"].ToString() == "CWIP")
                            objShipment.shipmentgid = "2";
                        if (dt.Rows[i]["ShipmentType"].ToString() != "DIRECT" && dt.Rows[i]["ShipmentType"].ToString() != "CWIP")
                            objShipment.shipmentgid = dt.Rows[i]["ShipmentType"].ToString();
                        objShipment.cbfdetGid = Convert.ToInt32(dt.Rows[i]["Slno"]);
                        //dt.Rows[i]["Slno"] = count + 1;
                        objShipment.branchCode = dt.Rows[i]["BranchCode"].ToString();
                        DataTable objTable = GetBranchRecords(objShipment.branchCode);
                        if (objTable.Rows.Count > 0)
                        {
                            for (int j = 0; j < objTable.Rows.Count; j++)
                            {

                                objShipment.branchgid = Convert.ToInt32(objTable.Rows[j]["branch_gid"].ToString());
                                //HttpContext.Current.Session["branchgid"] = objShipment.branchgid;
                                objShipment.branchName = objTable.Rows[j]["branch_name"].ToString();
                                objShipment.address = objTable.Rows[j]["branchaddress"].ToString();
                                objShipment.location = objTable.Rows[j]["branch_location_name"].ToString();
                                // objShipment.inchargeID = objTable.Rows[j]["branch_incharge"].ToString();
                                HttpContext.Current.Session["count"] = objShipment.SrNo;
                            }
                        }
                        objShipment.shippedqty = Convert.ToDecimal(dt.Rows[i]["Quantity"].ToString());
                        lLstShipment.Add(objShipment);
                    }
                }
                return lLstShipment;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return lLstShipment;
            }
            finally
            {
                con.Close();
            }
        }
        public DataTable ShipmentFinalTable(DataTable dt)
        {
            try
            {
                shipment objShipment;
                int count = 0;
                if (!dt.Columns.Contains("branchName") && !dt.Columns.Contains("address") && !dt.Columns.Contains("location") && !dt.Columns.Contains("inchargeID"))
                {
                    dt.Columns.Add("branchName");
                    dt.Columns.Add("address");
                    dt.Columns.Add("location");
                    dt.Columns.Add("inchargeID");
                }
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objShipment = new shipment();
                        count = Convert.ToInt32(HttpContext.Current.Session["count"]);
                        objShipment.SrNo = count + 1;
                        if (dt.Rows[i]["ShipmentType"].ToString() == "DIRECT")
                        {
                            objShipment.shipmentgid = "1";
                            dt.Rows[i]["ShipmentType"] = "1";
                        }
                        if (dt.Rows[i]["ShipmentType"].ToString() == "CWIP")
                        {
                            objShipment.shipmentgid = "2";
                            dt.Rows[i]["ShipmentType"] = "2";
                        }
                        if (dt.Rows[i]["ShipmentType"].ToString() != "DIRECT" && dt.Rows[i]["ShipmentType"].ToString() != "CWIP")
                            objShipment.shipmentgid = dt.Rows[i]["ShipmentType"].ToString();
                        objShipment.cbfdetGid = Convert.ToInt32(dt.Rows[i]["Slno"]);
                        dt.Rows[i]["Slno"] = count + 1;
                        objShipment.branchCode = dt.Rows[i]["BranchCode"].ToString();
                        DataTable objTable = GetBranchRecords(objShipment.branchCode);
                        if (objTable.Rows.Count > 0)
                        {
                            for (int j = 0; j < objTable.Rows.Count; j++)
                            {
                                dt.Rows[i]["branchgid"] = Convert.ToInt32(objTable.Rows[j]["branch_gid"].ToString());
                                //HttpContext.Current.Session["branchgid"] = objShipment.branchgid;
                                dt.Rows[i]["branchName"] = objTable.Rows[j]["branch_name"].ToString();
                                dt.Rows[i]["address"] = objTable.Rows[j]["branchaddress"].ToString();
                                dt.Rows[i]["location"] = objTable.Rows[j]["branch_location_name"].ToString();
                                // dt.Rows[i]["inchargeID"] = objTable.Rows[j]["branch_incharge"].ToString();
                                HttpContext.Current.Session["count"] = objShipment.SrNo;
                            }
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return dt;
            }
        }

        public DataTable GetBranchRecords(string branchCode)
        {
            DataTable objTable = new DataTable();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_cbfselection", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@branchCode", SqlDbType.VarChar).Value = branchCode;
                cmd.Parameters.Add("@actionName", SqlDbType.VarChar).Value = "branchdetails";
                da = new SqlDataAdapter(cmd);
                da.Fill(objTable);
                return objTable;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objTable;
            }
        }

        public string InsertPoTemplate(PoTemplate objTemplate)
        {
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_mst_termsandconditions", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@templateName", objTemplate.templateName);
                cmd.Parameters.AddWithValue("@actionName", "exist");
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 64);
                cmd.Parameters["@res"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res = Convert.ToString(cmd.Parameters["@res"].Value.ToString());
                if (res == "Not there")
                {
                    string[,] codes = new string[,]
	               {
                         
	                       {"termandcond_name",objTemplate.templateName.ToString()},
                           {"termandcond_content",objTemplate.templateContent.ToString()},
                           {"termandcond_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                           {"termandcond_insert_date",DateTime.Now.ToString("dd/MMM/yyyy")}
                   };

                    string tablename = "fb_mst_ttermandcond";
                    objTemplate.result = objcommon.InsertCommon(codes, tablename);
                }
                return objTemplate.result;
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
        public List<poraiser> GetTemplateListWO()
        {
            DataTable objTable = new DataTable();
            List<poraiser> objlist = new List<poraiser>();
            poraiser objTemplate;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_mst_termsandconditions", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectlist");
                da = new SqlDataAdapter(cmd);
                da.Fill(objTable);
                foreach (DataRow row in objTable.Rows)
                {
                    objTemplate = new poraiser();
                    objTemplate.templateGid = Convert.ToInt32(row["termandcond_gid"]);
                    objTemplate.tempName = row["termandcond_name"].ToString();
                    objlist.Add(objTemplate);
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
                con.Close();
            }
        }
        public List<poraiser> GetTemplateList()
        {
            DataTable objTable = new DataTable();
            List<poraiser> objlist = new List<poraiser>();
            poraiser objTemplate;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_mst_termsandconditions", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectlistPO");
                da = new SqlDataAdapter(cmd);
                da.Fill(objTable);
                foreach (DataRow row in objTable.Rows)
                {
                    objTemplate = new poraiser();
                    objTemplate.templateGid = Convert.ToInt32(row["termandcond_gid"]);
                    objTemplate.tempName = row["termandcond_name"].ToString();
                    objlist.Add(objTemplate);
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
                con.Close();
            }
        }
        public string GetTemplateContent(int id)
        {
            try
            {
                getconnection();
                string[,] parameter = new string[,]
                {
                    {"@tempgid",id.ToString()},
                    {"@actionName","selectTemplate"}
                };
                return objcommon.ProcedureCommon(parameter, "pr_fb_mst_termsandconditions");
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
        public string GetTemplateName(int id)
        {
            try
            {
                getconnection();
                string[,] parameter = new string[,]
                {
                    {"@tempgid",id.ToString()},
                    {"@actionName","selectTempName"}
                };
                return objcommon.ProcedureCommon(parameter, "pr_fb_mst_termsandconditions");
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
        public string InsertPo(poraiser objpoheader, DataTable objPoTable, DataTable objShipTable, DataTable objShipExcelTable, DataTable objExShip)
        {
            try
            {
                getconnection();
                // int shipmentgid = 0;
                if (objPoTable.Rows.Count == 0)
                {
                    return objpoheader.result = "There is no line item to save or submit";
                }
                if (objShipTable.Rows.Count == 0 && objShipExcelTable.Rows.Count == 0 && objExShip.Rows.Count == 0)
                {
                    return objpoheader.result = "Please Add Shipment Details";
                }
                int b = 0;
                cmd = new SqlCommand("pr_fb_trn_poinsert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@poheader_pono", objCmnFunctions.GetSequenceNoFb("PO", "", objpoheader.department));
                cmd.Parameters.AddWithValue("@poheader_date", objCmnFunctions.convertoDateTime(objpoheader.podate));
                cmd.Parameters.AddWithValue("@poheader_enddate", objCmnFunctions.convertoDateTime(objpoheader.poenddate));
                cmd.Parameters.AddWithValue("@poheader_raiser_gid", objCmnFunctions.GetLoginUserGid().ToString());
                cmd.Parameters.AddWithValue("@poheader_projmanager", objpoheader.projmanagergid.ToString());
                cmd.Parameters.AddWithValue("@poheader_requestfor", Convert.ToInt32(objpoheader.requestforgid).ToString());
                if (objpoheader.itType == "Application")
                    cmd.Parameters.AddWithValue("@poheader_ittype", "A");
                if (objpoheader.itType == "Infrastructure")
                    cmd.Parameters.AddWithValue("@poheader_ittype", "I");
                cmd.Parameters.AddWithValue("@poheader_vendor_gid", Convert.ToInt32(objpoheader.vendorgid.ToString()));
                cmd.Parameters.AddWithValue("@poheader_vendor_note", objpoheader.vendorNote);
                cmd.Parameters.AddWithValue("@poheader_over_total", objpoheader.overTotal);
                cmd.Parameters.AddWithValue("@poheader_type", "P");
                if (objpoheader.actionName == "Submit")
                {
                    cmd.Parameters.AddWithValue("@poheader_status", "2");

                }
                else
                {
                    cmd.Parameters.AddWithValue("@poheader_status", "1");
                }
                cmd.Parameters.AddWithValue("@poheader_termcond_gid", objpoheader.templateGid);
                cmd.Parameters.AddWithValue("@poheader_add_termandcond", objpoheader.additionTemplate);
                cmd.Parameters.AddWithValue("@actionName", "insertpoheader");
                cmd.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;

                int a = cmd.ExecuteNonQuery();
                int poheadGid = Convert.ToInt32(cmd.Parameters["@result"].Value);
                if (a == 1)
                {
                    if (objPoTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < objPoTable.Rows.Count; i++)
                        {
                            cmd = new SqlCommand("pr_fb_trn_poinsert", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@podetails_prodservice_gid", Convert.ToInt32(objPoTable.Rows[i]["prodservice_gid"].ToString()));
                            cmd.Parameters.AddWithValue("@podetails_desc", objPoTable.Rows[i]["prodservice_description"].ToString());
                            cmd.Parameters.AddWithValue("@podetails_uom_gid", Convert.ToInt32(objPoTable.Rows[i]["uom_gid"].ToString()));
                            if (objPoTable.Columns.Count > 20)
                            {
                                cmd.Parameters.AddWithValue("@podetails_qty", Convert.ToInt32(objPoTable.Rows[i]["cbfdetails_dumqty"].ToString()));
                                cmd.Parameters.AddWithValue("@podetails_unitprice", Convert.ToDecimal(objPoTable.Rows[i]["cbfdetails_unitprice"].ToString()));
                                cmd.Parameters.AddWithValue("@podetails_base_amount", Convert.ToDecimal(objPoTable.Rows[i]["cbfdetails_totalamt"].ToString()));
                                //cmd.Parameters.AddWithValue("@podetails_total", Convert.ToDecimal(objPoTable.Rows[i]["total"].ToString()));
                                cmd.Parameters.AddWithValue("@podetails_discount", Convert.ToDecimal(objPoTable.Rows[i]["discount"].ToString()));
                                cmd.Parameters.AddWithValue("@podetails_tax1", Convert.ToDecimal(objPoTable.Rows[i]["tax1"].ToString()));
                                cmd.Parameters.AddWithValue("@podetails_tax2", Convert.ToDecimal(objPoTable.Rows[i]["tax2"].ToString()));
                                cmd.Parameters.AddWithValue("@podetails_tax3", Convert.ToDecimal(string.IsNullOrEmpty(objPoTable.Rows[i]["tax3"].ToString()) ? "0" : objPoTable.Rows[i]["tax3"].ToString()));
                                cmd.Parameters.AddWithValue("@podetails_total", Convert.ToDecimal(objPoTable.Rows[i]["total"].ToString()));
                                cmd.Parameters.AddWithValue("@podetails_cbfdet_gid", Convert.ToInt32(objPoTable.Rows[i]["cbfdetails_gid"].ToString()));
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@podetails_qty", Convert.ToInt32(objPoTable.Rows[i]["cbfdetails_qty"].ToString()));
                                cmd.Parameters.AddWithValue("@podetails_unitprice", Convert.ToDecimal(objPoTable.Rows[i]["cbfdetails_unitprice"].ToString()));
                                cmd.Parameters.AddWithValue("@podetails_base_amount", Convert.ToDecimal(objPoTable.Rows[i]["cbfdetails_totalamt"].ToString()));
                                cmd.Parameters.AddWithValue("@podetails_discount", "0");
                                cmd.Parameters.AddWithValue("@podetails_tax1", "0");
                                cmd.Parameters.AddWithValue("@podetails_tax2", "0");
                                cmd.Parameters.AddWithValue("@podetails_tax3", "0");
                                cmd.Parameters.AddWithValue("@podetails_total", Convert.ToDecimal(objPoTable.Rows[i]["cbfdetails_totalamt"].ToString()));
                                cmd.Parameters.AddWithValue("@podetails_cbfdet_gid", Convert.ToInt32(objPoTable.Rows[i]["cbfdetails_gid"].ToString()));
                            }
                            cmd.Parameters.AddWithValue("@actionName", "insertpodetails");
                            b = cmd.ExecuteNonQuery();
                        }
                        if (!objPoTable.Columns.Contains("cbfdetails_remqty"))
                            objPoTable.Columns.Add("cbfdetails_remqty");
                        for (int j = 0; j < objPoTable.Rows.Count; j++)
                        {

                            objPoTable.Rows[j]["cbfdetails_remqty"] = Convert.ToInt32(Convert.ToInt32(objPoTable.Rows[j]["cbfdetails_qty"]) - Convert.ToInt32(objPoTable.Rows[j]["cbfdetails_dumqty"]));
                            cmd = new SqlCommand("pr_fb_trn_poinsert", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@cbfdetails_gid", Convert.ToInt32(objPoTable.Rows[j]["cbfdetails_gid"]));
                            if (Convert.ToInt32(objPoTable.Rows[j]["cbfdetails_remqty"]) == 0)
                            {
                                cmd.Parameters.AddWithValue("@cbfdetails_flag", "C");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@cbfdetails_flag", "P");
                            }
                            cmd.Parameters.AddWithValue("@actionName", "updateflagcbf");
                            int result = cmd.ExecuteNonQuery();
                        }
                    }
                }

                if (b == 1)
                {
                    if (objExShip.Rows.Count > 0)
                    {
                        for (int ex = 0; ex < objExShip.Rows.Count; ex++)
                        {
                            cmd = new SqlCommand("pr_fb_trn_poinsert", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@podetails_cbfdet_gid", Convert.ToInt32(objExShip.Rows[ex]["cbfdetgid"].ToString()));
                            cmd.Parameters.AddWithValue("@poshipment_type_gid", Convert.ToInt32(objExShip.Rows[ex]["shipmenttype"].ToString()));
                            cmd.Parameters.AddWithValue("@poshipment_branch_gid", Convert.ToInt32(objExShip.Rows[ex]["branchgid"].ToString()));
                            cmd.Parameters.AddWithValue("@poshipment_empgid", Convert.ToInt32(objExShip.Rows[ex]["empgid"].ToString()));
                            cmd.Parameters.AddWithValue("@poshipment_qty", Convert.ToDecimal(objExShip.Rows[ex]["Quantity"].ToString()));
                            cmd.Parameters.AddWithValue("@actionName", "insertshipment");
                            objpoheader.result = Convert.ToString(cmd.ExecuteNonQuery());
                        }
                    }
                    else
                    {
                        if (objShipTable.Rows.Count > 0)
                        {
                            for (int s = 0; s < objShipTable.Rows.Count; s++)
                            {
                                cmd = new SqlCommand("pr_fb_trn_poinsert", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@podetails_cbfdet_gid", Convert.ToInt32(objShipTable.Rows[s]["cbfdetgid"].ToString()));
                                cmd.Parameters.AddWithValue("@poshipment_type_gid", Convert.ToInt32(objShipTable.Rows[s]["shipmenttype"].ToString()));
                                cmd.Parameters.AddWithValue("@poshipment_branch_gid", Convert.ToInt32(objShipTable.Rows[s]["branchgid"].ToString()));
                                cmd.Parameters.AddWithValue("@poshipment_empgid", Convert.ToInt32(objShipTable.Rows[s]["empgid"].ToString()));
                                cmd.Parameters.AddWithValue("@poshipment_qty", Convert.ToDecimal(objShipTable.Rows[s]["Quantity"].ToString()));
                                cmd.Parameters.AddWithValue("@actionName", "insertshipment");
                                objpoheader.result = Convert.ToString(cmd.ExecuteNonQuery());

                            }
                        }
                        if (objShipExcelTable.Rows.Count > 0)
                        {
                            for (int x = 0; x < objShipExcelTable.Rows.Count; x++)
                            {
                                //objShipment=new shipment();
                                cmd = new SqlCommand("pr_fb_trn_poinsert", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@poshipment_podet_gid", Convert.ToInt32(objShipExcelTable.Rows[x]["cbfdetgid"].ToString()));
                                //if (objShipExcelTable.Rows[x]["ShipmentType"].ToString() == "DIRECT")
                                //    shipmentgid = 2;
                                //if (objShipExcelTable.Rows[x]["ShipmentType"].ToString() == "CWIP")
                                //    shipmentgid = 3;
                                cmd.Parameters.AddWithValue("@poshipment_type_gid", objShipExcelTable.Rows[x]["ShipmentType"].ToString());
                                cmd.Parameters.AddWithValue("@poshipment_branch_gid", Convert.ToInt32(objShipExcelTable.Rows[x]["branchgid"].ToString()));
                                cmd.Parameters.AddWithValue("@poshipment_empgid", Convert.ToInt32(objShipExcelTable.Rows[x]["empgid"].ToString()));
                                cmd.Parameters.AddWithValue("@poshipment_qty", Convert.ToDecimal(objShipExcelTable.Rows[x]["Quantity"].ToString()));
                                cmd.Parameters.AddWithValue("@actionName", "insertshipment");
                                objpoheader.result = Convert.ToString(cmd.ExecuteNonQuery());
                            }
                        }
                    }
                    if (objpoheader.actionName == "Submit")
                    {
                        cmd = new SqlCommand("pr_fb_trn_apprqueuePO", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@refgid", poheadGid);
                        cmd.Parameters.AddWithValue("@empgid", objCmnFunctions.GetLoginUserGid());
                        cmd.Parameters.Add("@requesttype", "PO");
                        objpoheader.result = Convert.ToString(cmd.ExecuteNonQuery());
                    }
                    //HttpContext.Current.Session["poqueuegid"] = "";
                    //cmd = new SqlCommand("pr_fb_Getqueuegidformail", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@actionName", SqlDbType.VarChar).Value = "mailsubmissionforpo";
                    //cmd.Parameters.Add("@queue_refgid", SqlDbType.Int).Value = poheadGid;
                    //string queuegid = cmd.ExecuteScalar().ToString();
                    //if(queuegid!=null || queuegid!="")
                    //HttpContext.Current.Session["poqueuegid"] = queuegid;
                }
                if (objpoheader.result != "")
                {
                    objpoheader.result = "Inserted Successfully";
                }
                return objpoheader.result;
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
        public List<ViewBOQ> GetAttachmentDetails(int cbfgid)
        {

            DataTable objTable = new DataTable();
            List<ViewBOQ> objlist = new List<ViewBOQ>();
            ViewBOQ objBoq;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_cbfselection", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@cbfdetailgid", cbfgid);
                cmd.Parameters.AddWithValue("@actionName", "selectBoq");
                da = new SqlDataAdapter(cmd);
                da.Fill(objTable);
                for (int i = 0; i < objTable.Rows.Count; i++)
                {
                    objBoq = new ViewBOQ()
                    {
                        poFileName = objTable.Rows[i]["attachment_filename"].ToString(),
                        poDescription = objTable.Rows[i]["attachment_desc"].ToString(),
                        attachmentDate = objTable.Rows[i]["attachdate"].ToString()
                    };
                    objlist.Add(objBoq);
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
                con.Close();
            }
        }
        public DataTable GetPoDetails(int poheadGid)
        {
            DataTable objtable = new DataTable();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_poheader", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectpodetails");
                cmd.Parameters.AddWithValue("@poheadgid", poheadGid);
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                if (!objtable.Columns.Contains("cbfheader_amount"))
                    objtable.Columns.Add("cbfheader_amount");
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    cmd = new SqlCommand("pr_fb_trn_poheader", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@actionName", "selectpoamount");
                    cmd.Parameters.AddWithValue("@poheadgid", poheadGid);
                    objtable.Rows[i]["cbfheader_amount"] = cmd.ExecuteScalar();
                }
                return objtable;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objtable;
            }
            finally
            {
                con.Close();
            }
        }
        //public DataTable GetPoDetails(int poheadGid)
        //{
        //    try
        //    {
        //        getconnection();
        //        DataTable objtable = new DataTable();
        //        if (!objtable.Columns.Contains("pototal_amount") && !objtable.Columns.Contains("pototal_remaining") && !objtable.Columns.Contains("pogrand_total")
        //            && !objtable.Columns.Contains("pototal"))
        //        {
        //            objtable.Columns.Add("pototal_amount");
        //            objtable.Columns.Add("pototal_remaining");
        //            objtable.Columns.Add("pogrand_total");
        //            objtable.Columns.Add("pototal");
        //        }
        //        cmd = new SqlCommand("pr_fb_trn_poheader", con);
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        cmd.Parameters.AddWithValue("@actionName", "selectpodetails");
        //        cmd.Parameters.AddWithValue("@poheadgid", poheadGid);

        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(objtable);
        //        for (int i = 0; i < objtable.Rows.Count;i++)
        //        {
        //            cmd = new SqlCommand("pr_fb_trn_poheader", con);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.AddWithValue("@actionName", "selectpoamount");
        //            cmd.Parameters.AddWithValue("@poheadgid", poheadGid);
        //            objtable.Rows[i]["pototal_amount"] = cmd.ExecuteScalar();
        //            objtable.Rows[i]["pototal"]=  Convert.ToDecimal(Convert.ToDecimal(objtable.Rows[i]["pototal_amount"])+Convert.ToDecimal(objtable.Rows[i]["podetails_total"]));
        //            objtable.Rows[i]["pogrand_total"] = Convert.ToDecimal(Convert.ToDecimal(objtable.Rows[i]["cbfheader_cbfamt"]) + Convert.ToDecimal(objtable.Rows[i]["cbfheader_Devi_amt"]));
        //            objtable.Rows[i]["pototal_remaining"] = Convert.ToDecimal(Convert.ToDecimal(objtable.Rows[i]["pogrand_total"]) - Convert.ToDecimal(objtable.Rows[i]["pototal"]));
        //        }
        //            return objtable;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //}

        public List<poraiser> GetPoDetailsList(DataTable dt)
        {
            List<poraiser> objlist = new List<poraiser>();
            poraiser objraiser;
            try
            {
                getconnection();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objraiser = new poraiser();
                    objraiser.podetGid = Convert.ToInt32(dt.Rows[i]["podetails_gid"].ToString());
                    objraiser.poheadGid = Convert.ToInt32(dt.Rows[i]["poheader_gid"].ToString());
                    objraiser.porefno = dt.Rows[i]["poheader_pono"].ToString();
                    objraiser.podate = dt.Rows[i]["poheader_date"].ToString();
                    objraiser.poenddate = dt.Rows[i]["poheader_enddate"].ToString();
                    // objraiser.cbfheadAmount = Convert.ToDecimal(dt.Rows[i]["cbfheader_cbfamt"].ToString());
                    objraiser.devamount = Convert.ToDecimal(dt.Rows[i]["cbfheader_Devi_amt"].ToString());
                    objraiser.cbfdetailsQty = Convert.ToDecimal(dt.Rows[i]["cbfdetails_qty"].ToString());
                    objraiser.projmanagergid = Convert.ToInt32(dt.Rows[i]["poheader_projectmanager"].ToString());
                    objraiser.vendorName = GetVendorName(Convert.ToInt32(dt.Rows[i]["poheader_vendor_gid"].ToString()));
                    objraiser.vendorgid = Convert.ToInt32(dt.Rows[i]["poheader_vendor_gid"].ToString());
                    objraiser.itType = dt.Rows[i]["poheader_ittype"].ToString();
                    objraiser.department = getrequestName(Convert.ToInt32(dt.Rows[i]["poheader_requestfor"].ToString()));
                    objraiser.vendorNote = dt.Rows[i]["poheader_vendor_note"].ToString();
                    objraiser.templateGid = Convert.ToInt32(dt.Rows[i]["poheader_termcond_gid"].ToString());
                    objraiser.tempName = GetTemplateContent(objraiser.templateGid);
                    objraiser.templname = GetTemplateName(objraiser.templateGid);
                    objraiser.additionTemplate = dt.Rows[i]["poheader_add_termandcond"].ToString();
                    objraiser.prodservicegroup = dt.Rows[i]["productgroupname"].ToString();
                    objraiser.prodservicecode = dt.Rows[i]["prodservice_code"].ToString();
                    objraiser.prodservicename = dt.Rows[i]["prodservice_name"].ToString();
                    objraiser.prodservicedesc = dt.Rows[i]["podetails_desc"].ToString();
                    objraiser.units = dt.Rows[i]["uom_code"].ToString();
                    objraiser.quantity = Convert.ToDecimal(dt.Rows[i]["podetails_qty"]);
                    objraiser.actQuantity = Convert.ToDecimal(dt.Rows[i]["actQuantity"]);
                    objraiser.unitprice = Convert.ToDecimal(dt.Rows[i]["podetails_unitprice"].ToString());
                    objraiser.discount = Convert.ToDecimal(dt.Rows[i]["podetails_discount"].ToString());
                    objraiser.baseamount = Convert.ToDecimal(dt.Rows[i]["podetails_base_amt"].ToString());
                    objraiser.tax1 = Convert.ToDecimal(dt.Rows[i]["podetails_tax1"].ToString());
                    objraiser.tax2 = Convert.ToDecimal(dt.Rows[i]["podetails_tax2"].ToString());
                    objraiser.tax3 = Convert.ToDecimal(dt.Rows[i]["podetails_tax3"].ToString());
                    objraiser.total = Convert.ToDecimal(dt.Rows[i]["podetails_total"].ToString());
                    objraiser.cbfheadAmount = Convert.ToDecimal(dt.Rows[i]["cbfheader_cbfamt"]);
                    //objraiser.poRemaingAmount = Convert.ToDecimal(dt.Rows[i]["pototal_remaining"]);
                    objraiser.cbfno = dt.Rows[i]["cbfheader_cbfno"].ToString();
                    objlist.Add(objraiser);
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
                con.Close();
            }
        }
        public string GetVendorName(int vendorGid)
        {
            try
            {
                getconnection();
                string[,] parameter = new string[,]
                {
                    {"@vendorgid",vendorGid.ToString()},
                    {"@actionName","vendorname"},
                };
                return objcommon.ProcedureCommon(parameter, "pr_fb_trn_poheader");
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
        public string GetProjectManager(int projmanagergid)
        {
            try
            {
                getconnection();
                string[,] parameter = new string[,]
                {
                    {"@projmanagergid",projmanagergid.ToString()},
                    {"@actionName","projmanagername"},
                };
                return objcommon.ProcedureCommon(parameter, "pr_fb_trn_poheader");
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
        public DataTable GetShipTable(int poDetGid)
        {
            DataTable objtable = new DataTable();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_poheader", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectshipment");
                cmd.Parameters.AddWithValue("@podetailgid", poDetGid);
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                return objtable;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objtable;
            }
            finally
            {
                con.Close();
            }
        }
        public List<shipment> GetShipmentDetails(DataTable dt)
        {
            List<shipment> lstShipment = new List<shipment>();
            shipment objShipment;
            try
            {
                getconnection();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["deleteflag"].ToString() == "")
                    {
                        objShipment = new shipment();
                        objShipment.SrNo = i + 1;
                        objShipment.shipmentgid = dt.Rows[i]["ShipmentType"].ToString();
                        objShipment.shipGid = Convert.ToInt32(dt.Rows[i]["poshipment_podet_gid"].ToString());
                        objShipment.branchgid = Convert.ToInt32(dt.Rows[i]["poshipment_branch_gid"].ToString());
                        objShipment.branchName = dt.Rows[i]["branch_name"].ToString();
                        objShipment.address = dt.Rows[i]["branch_address"].ToString();
                        objShipment.location = dt.Rows[i]["branch_location_name"].ToString();
                        objShipment.inchargeID = dt.Rows[i]["branch_incharge"].ToString();
                        objShipment.branchCode = dt.Rows[i]["BranchCode"].ToString();
                        objShipment.shippedqty = Convert.ToDecimal(dt.Rows[i]["Quantity"].ToString());
                        lstShipment.Add(objShipment);
                    }
                }
                return lstShipment;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return lstShipment;
            }
            finally
            {
                con.Close();
            }
        }
        public DataTable ShipmentFinalEditTable(DataTable dt)
        {
            try
            {
                shipment objShipment;
                int count = 0;
                if (dt.Columns.Count == 9)
                {
                    dt.Columns.Add("branch_name");
                    dt.Columns.Add("branch_address");
                    dt.Columns.Add("branch_location_name");
                    dt.Columns.Add("branch_incharge");

                }
                if (dt.Rows.Count > 0)
                {


                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objShipment = new shipment();
                        count = Convert.ToInt32(HttpContext.Current.Session["count"]);
                        objShipment.SrNo = count + 1;
                        if (dt.Rows[i]["ShipmentType"].ToString() == "DIRECT")
                            objShipment.shipmentgid = "1";
                        dt.Rows[i]["ShipmentType"] = "1";
                        if (dt.Rows[i]["ShipmentType"].ToString() == "CWIP")
                            objShipment.shipmentgid = "2";
                        dt.Rows[i]["ShipmentType"] = "2";
                        if (dt.Rows[i]["ShipmentType"].ToString() != "DIRECT" && dt.Rows[i]["ShipmentType"].ToString() != "CWIP")
                            objShipment.shipmentgid = dt.Rows[i]["ShipmentType"].ToString();
                        // objShipment.cbfdetGid = Convert.ToInt32(dt.Rows[i]["Slno"]);
                        dt.Rows[i]["Slno"] = count + 1;
                        dt.Rows[i]["poshipment_gid"] = 0;
                        objShipment.branchCode = dt.Rows[i]["BranchCode"].ToString();
                        DataTable objTable = GetBranchRecords(objShipment.branchCode);
                        if (objTable.Rows.Count > 0)
                        {
                            for (int j = 0; j < objTable.Rows.Count; j++)
                            {

                                dt.Rows[i]["poshipment_branch_gid"] = Convert.ToInt32(objTable.Rows[j]["branch_gid"].ToString());
                                //HttpContext.Current.Session["branchgid"] = objShipment.branchgid;
                                dt.Rows[i]["branch_name"] = objTable.Rows[j]["branch_name"].ToString();
                                dt.Rows[i]["branch_address"] = objTable.Rows[j]["branchaddress"].ToString();
                                dt.Rows[i]["branch_location_name"] = objTable.Rows[j]["branch_location_name"].ToString();
                                //dt.Rows[i]["branch_incharge"] = objTable.Rows[j]["branch_incharge"].ToString();
                                HttpContext.Current.Session["count"] = objShipment.SrNo;
                            }
                        }
                    }
                }
                return dt;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return dt;
            }
        }
        public List<shipment> ExcelShipmentEdit(DataTable dt)
        {
            List<shipment> lLstShipment = new List<shipment>();
            try
            {
                shipment objShipment;
                int count = 0;

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objShipment = new shipment();
                        //count = Convert.ToInt32(HttpContext.Current.Session["count"]);
                        //objShipment.SrNo = count + 1;
                        if (dt.Rows[i]["ShipmentType"].ToString() == "DIRECT")
                            objShipment.shipmentgid = "1";
                        if (dt.Rows[i]["ShipmentType"].ToString() == "CWIP")
                            objShipment.shipmentgid = "2";
                        if (dt.Rows[i]["ShipmentType"].ToString() != "DIRECT" && dt.Rows[i]["ShipmentType"].ToString() != "CWIP")
                            objShipment.shipmentgid = dt.Rows[i]["ShipmentType"].ToString();
                        //objShipment.cbfdetGid = Convert.ToInt32(dt.Rows[i]["Slno"]);
                        //dt.Rows[i]["Slno"] = count + 1;
                        objShipment.branchCode = dt.Rows[i]["BranchCode"].ToString();
                        DataTable objTable = GetBranchRecords(objShipment.branchCode);
                        if (objTable.Rows.Count > 0)
                        {
                            for (int j = 0; j < objTable.Rows.Count; j++)
                            {

                                objShipment.branchgid = Convert.ToInt32(objTable.Rows[j]["branch_gid"].ToString());
                                //HttpContext.Current.Session["branchgid"] = objShipment.branchgid;
                                objShipment.branchName = objTable.Rows[j]["branch_name"].ToString();
                                objShipment.address = objTable.Rows[j]["branchaddress"].ToString();
                                objShipment.location = objTable.Rows[j]["branch_location_name"].ToString();
                                // objShipment.inchargeID = objTable.Rows[j]["branch_incharge"].ToString();
                                // HttpContext.Current.Session["count"] = objShipment.SrNo;
                            }
                        }
                        objShipment.shippedqty = Convert.ToDecimal(dt.Rows[i]["Quantity"].ToString());
                        lLstShipment.Add(objShipment);
                    }
                }
                return lLstShipment;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return lLstShipment;
            }
            finally
            {
                con.Close();
            }
        }
        public string UpdatePo(poraiser objpoheader, DataTable objPoTable, DataTable objShipTable, DataTable objShipExcelTable, DataTable objExShip)
        {
            try
            {
                if (objPoTable.Rows.Count == 0)
                {
                    objpoheader.result = "There is no line item to Update";
                    return objpoheader.result;
                }
                //if(objShipTable.Rows.Count==0 && objShipExcelTable.Rows.Count==0 && objExShip.Rows.Count==0)
                //{
                //    objpoheader.result = "Please Add Shipment Details";
                //    return objpoheader.result;
                //}
                getconnection();
                // int shipmentgid = 0;
                int b = 0;
                cmd = new SqlCommand("pr_fb_trn_poinsert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@poheader_pono", objCmnFunctions.GetSequenceNoFb("PO", "", objpoheader.department));
                cmd.Parameters.AddWithValue("@poheader_date", objCmnFunctions.convertoDateTime(objpoheader.podate));
                cmd.Parameters.AddWithValue("@poheader_enddate", objCmnFunctions.convertoDateTime(objpoheader.poenddate));
                //cmd.Parameters.AddWithValue("@poheader_raiser_gid", objCmnFunctions.GetLoginUserGid().ToString());
                cmd.Parameters.AddWithValue("@poheader_projmanager", objpoheader.projmanagergid.ToString());
                //    cmd.Parameters.AddWithValue("@poheader_requestfor", Convert.ToInt32(objpoheader.requestforgid).ToString());
                if (objpoheader.itType == "Application")
                    cmd.Parameters.AddWithValue("@poheader_ittype", "A");
                if (objpoheader.itType == "Infrastructure")
                    cmd.Parameters.AddWithValue("@poheader_ittype", "I");
                // cmd.Parameters.AddWithValue("@poheader_vendor_gid", Convert.ToInt32(objpoheader.vendorgid.ToString()));
                cmd.Parameters.AddWithValue("@poheader_vendor_note", objpoheader.vendorNote);
                cmd.Parameters.AddWithValue("@poheader_over_total", objpoheader.overTotal);
                if (objpoheader.actionName == "amend")
                {
                    cmd.Parameters.AddWithValue("@poheader_amendmentdate", Convert.ToDateTime(DateTime.Now.ToString()));
                    cmd.Parameters.AddWithValue("@poheader_status", "2");
                }
                else
                {
                    //cmd.Parameters.AddWithValue("@poheader_type", "P");
                    if (objpoheader.actionName == "Submit")
                    {
                        cmd.Parameters.AddWithValue("@poheader_status", "2");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@poheader_status", "1");
                    }
                }
                cmd.Parameters.AddWithValue("@poheader_termcond_gid", objpoheader.templateGid);
                cmd.Parameters.AddWithValue("@poheader_add_termandcond", objpoheader.additionTemplate);
                cmd.Parameters.AddWithValue("@poheader_gid", objpoheader.poheadGid);
                cmd.Parameters.AddWithValue("@poheader_vendor_gid", objpoheader.vendorgid);
                cmd.Parameters.AddWithValue("@actionName", "updatepoheader");
                int a = cmd.ExecuteNonQuery();
                if (a >= 1)
                {
                    if (objPoTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < objPoTable.Rows.Count; i++)
                        {
                            cmd = new SqlCommand("pr_fb_trn_poinsert", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            //cmd.Parameters.AddWithValue("@podetails_prodservice_gid", Convert.ToInt32(objPoTable.Rows[i]["prodservice_gid"].ToString()));
                            cmd.Parameters.AddWithValue("@podetails_desc", objPoTable.Rows[i]["podetails_desc"].ToString());
                            //cmd.Parameters.AddWithValue("@podetails_uom_gid", Convert.ToInt32(objPoTable.Rows[i]["uom_gid"].ToString()));
                            cmd.Parameters.AddWithValue("@podetails_qty", Convert.ToInt32(objPoTable.Rows[i]["podetails_qty"].ToString()));
                            cmd.Parameters.AddWithValue("@podetails_unitprice", Convert.ToDecimal(objPoTable.Rows[i]["podetails_unitprice"].ToString()));
                            cmd.Parameters.AddWithValue("@podetails_base_amount", Convert.ToDecimal(objPoTable.Rows[i]["podetails_base_amt"].ToString()));
                            //cmd.Parameters.AddWithValue("@podetails_total", Convert.ToDecimal(objPoTable.Rows[i]["total"].ToString()));
                            cmd.Parameters.AddWithValue("@podetails_discount", Convert.ToDecimal(objPoTable.Rows[i]["podetails_discount"].ToString()));
                            cmd.Parameters.AddWithValue("@podetails_tax1", Convert.ToDecimal(objPoTable.Rows[i]["podetails_tax1"].ToString()));
                            cmd.Parameters.AddWithValue("@podetails_tax2", Convert.ToDecimal(objPoTable.Rows[i]["podetails_tax2"].ToString()));
                            cmd.Parameters.AddWithValue("@podetails_tax3", Convert.ToDecimal(string.IsNullOrEmpty(objPoTable.Rows[i]["podetails_tax3"].ToString()) ? "0" : objPoTable.Rows[i]["podetails_tax3"].ToString()));
                            cmd.Parameters.AddWithValue("@podetails_total", Convert.ToDecimal(objPoTable.Rows[i]["podetails_total"].ToString()));

                            cmd.Parameters.AddWithValue("@podetails_gid", Convert.ToInt32(objPoTable.Rows[i]["podetails_gid"].ToString()));
                            //else
                            //{
                            //    cmd.Parameters.AddWithValue("@podetails_qty", Convert.ToInt32(objPoTable.Rows[i]["cbfdetails_qty"].ToString()));
                            //    cmd.Parameters.AddWithValue("@podetails_unitprice", Convert.ToDecimal(objPoTable.Rows[i]["cbfdetails_unitprice"].ToString()));
                            //    cmd.Parameters.AddWithValue("@podetails_base_amount", Convert.ToDecimal(objPoTable.Rows[i]["cbfdetails_totalamt"].ToString()));
                            //    cmd.Parameters.AddWithValue("@podetails_discount", "0");
                            //    cmd.Parameters.AddWithValue("@podetails_tax1", "0");
                            //    cmd.Parameters.AddWithValue("@podetails_tax2", "0");
                            //    cmd.Parameters.AddWithValue("@podetails_tax3", "0");
                            //    cmd.Parameters.AddWithValue("@podetails_total", Convert.ToDecimal(objPoTable.Rows[i]["cbfdetails_totalamt"].ToString()));
                            //    cmd.Parameters.AddWithValue("@podetails_gid", Convert.ToInt32(objPoTable.Rows[i]["podetails_gid"].ToString()));
                            //}
                            cmd.Parameters.AddWithValue("@actionName", "updatepodetails");
                            b = cmd.ExecuteNonQuery();

                        }
                    }
                    if (!objPoTable.Columns.Contains("cbfdetails_remqty"))
                        objPoTable.Columns.Add("cbfdetails_remqty");
                    for (int j = 0; j < objPoTable.Rows.Count; j++)
                    {

                        objPoTable.Rows[j]["cbfdetails_remqty"] = Convert.ToInt32(Convert.ToInt32(objPoTable.Rows[j]["cbfdetails_qty"]) - Convert.ToInt32(objPoTable.Rows[j]["podetails_qty"]));
                        cmd = new SqlCommand("pr_fb_trn_poinsert", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@cbfdetails_gid", Convert.ToInt32(objPoTable.Rows[j]["podetails_cbfdet_gid"]));
                        if (Convert.ToInt32(objPoTable.Rows[j]["cbfdetails_remqty"]) == 0)
                        {
                            cmd.Parameters.AddWithValue("@cbfdetails_flag", "C");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@cbfdetails_flag", "P");
                        }
                        cmd.Parameters.AddWithValue("@actionName", "updateflagcbf");
                        int result = cmd.ExecuteNonQuery();
                    }
                }
                if (b == 1)
                {
                    if (objExShip.Rows.Count > 0)
                    {
                        for (int ex = 0; ex < objExShip.Rows.Count; ex++)
                        {
                            if (Convert.ToInt32(objExShip.Rows[ex]["poshipment_gid"].ToString()) > 0)
                            {

                                if ((objExShip.Rows[ex]["deleteflag"].ToString()) == "Y")
                                {
                                    cmd = new SqlCommand("pr_fb_trn_poinsert", con);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@poshipment_gid", Convert.ToInt32(objExShip.Rows[ex]["poshipment_gid"].ToString()));
                                    cmd.Parameters.AddWithValue("@deleteflag", objExShip.Rows[ex]["deleteflag"].ToString());
                                    cmd.Parameters.AddWithValue("@actionName", "deleteshipment");
                                    objpoheader.result = Convert.ToString(cmd.ExecuteNonQuery());
                                }
                                else
                                {
                                    cmd = new SqlCommand("pr_fb_trn_poinsert", con);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    //   cmd.Parameters.AddWithValue("@podetails_cbfdet_gid", Convert.ToInt32(objExShip.Rows[ex]["cbfdetgid"].ToString()));
                                    cmd.Parameters.AddWithValue("@poshipment_type_gid", Convert.ToInt32(objExShip.Rows[ex]["ShipmentType"].ToString()));
                                    cmd.Parameters.AddWithValue("@poshipment_branch_gid", Convert.ToInt32(objExShip.Rows[ex]["poshipment_branch_gid"].ToString()));
                                    cmd.Parameters.AddWithValue("@poshipment_empgid", Convert.ToInt32(objExShip.Rows[ex]["employeegid"].ToString()));
                                    cmd.Parameters.AddWithValue("@poshipment_qty", Convert.ToDecimal(objExShip.Rows[ex]["Quantity"].ToString()));
                                    cmd.Parameters.AddWithValue("@poshipment_gid", Convert.ToInt32(objExShip.Rows[ex]["poshipment_gid"].ToString()));

                                    cmd.Parameters.AddWithValue("@actionName", "updateshipment");
                                    objpoheader.result = Convert.ToString(cmd.ExecuteNonQuery());
                                }
                            }
                            //else if ((objExShip.Rows[ex]["deleteflag"].ToString()) == "Y")
                            //{
                            //        cmd = new SqlCommand("pr_fb_trn_poinsert", con);
                            //        cmd.CommandType = CommandType.StoredProcedure;
                            //        cmd.Parameters.AddWithValue("@poshipment_gid", Convert.ToInt32(objExShip.Rows[ex]["poshipment_gid"].ToString()));
                            //        cmd.Parameters.AddWithValue("@actionName", "deleteshipment");
                            //        objpoheader.result = Convert.ToString(cmd.ExecuteNonQuery());

                            //}
                            else
                            {

                                if ((objExShip.Rows[ex]["deleteflag"].ToString()) != "Y")
                                {
                                    cmd = new SqlCommand("pr_fb_trn_poinsert", con);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    //   cmd.Parameters.AddWithValue("@podetails_cbfdet_gid", Convert.ToInt32(objExShip.Rows[ex]["cbfdetgid"].ToString()));
                                    cmd.Parameters.AddWithValue("@poshipment_type_gid", Convert.ToInt32(objExShip.Rows[ex]["ShipmentType"].ToString()));
                                    cmd.Parameters.AddWithValue("@poshipment_branch_gid", Convert.ToInt32(objExShip.Rows[ex]["poshipment_branch_gid"].ToString()));
                                    cmd.Parameters.AddWithValue("@poshipment_qty", Convert.ToDecimal(objExShip.Rows[ex]["Quantity"].ToString()));
                                    cmd.Parameters.AddWithValue("@poshipment_empgid", Convert.ToInt32(objExShip.Rows[ex]["employeegid"].ToString()));
                                    cmd.Parameters.AddWithValue("@poshipment_podet_gid", Convert.ToInt32(objExShip.Rows[ex]["poshipment_podet_gid"].ToString()));
                                    cmd.Parameters.AddWithValue("@actionName", "insertshipment");
                                    objpoheader.result = Convert.ToString(cmd.ExecuteNonQuery());
                                }
                            }
                        }
                    }

                    if (objShipTable.Rows.Count > 0)
                    {
                        for (int s = 0; s < objShipTable.Rows.Count; s++)
                        {
                            if (Convert.ToInt32(objShipTable.Rows[s]["poshipment_gid"].ToString()) > 0)
                            {


                                if ((objShipTable.Rows[s]["deleteflag"].ToString()) == "Y")
                                {
                                    cmd = new SqlCommand("pr_fb_trn_poinsert", con);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@poshipment_gid", Convert.ToInt32(objShipTable.Rows[s]["poshipment_gid"].ToString()));
                                    cmd.Parameters.AddWithValue("@actionName", "deleteshipment");
                                    objpoheader.result = Convert.ToString(cmd.ExecuteNonQuery());
                                }
                                else
                                {
                                    //   cmd.Parameters.AddWithValue("@podetails_cbfdet_gid", Convert.ToInt32(objExShip.Rows[ex]["cbfdetgid"].ToString()));
                                    cmd = new SqlCommand("pr_fb_trn_poinsert", con);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@poshipment_type_gid", Convert.ToInt32(objShipTable.Rows[s]["ShipmentType"].ToString()));
                                    cmd.Parameters.AddWithValue("@poshipment_branch_gid", Convert.ToInt32(objShipTable.Rows[s]["poshipment_branch_gid"].ToString()));
                                    cmd.Parameters.AddWithValue("@poshipment_qty", Convert.ToDecimal(objShipTable.Rows[s]["Quantity"].ToString()));
                                    cmd.Parameters.AddWithValue("@poshipment_empgid", Convert.ToInt32(objShipTable.Rows[s]["employeegid"].ToString()));
                                    cmd.Parameters.AddWithValue("@poshipment_gid", Convert.ToInt32(objShipTable.Rows[s]["poshipment_gid"].ToString()));
                                    cmd.Parameters.AddWithValue("@actionName", "updateshipment");
                                    objpoheader.result = Convert.ToString(cmd.ExecuteNonQuery());
                                }
                            }
                            else
                            {
                                //   cmd.Parameters.AddWithValue("@podetails_cbfdet_gid", Convert.ToInt32(objExShip.Rows[ex]["cbfdetgid"].ToString()));
                                cmd = new SqlCommand("pr_fb_trn_poinsert", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@poshipment_type_gid", Convert.ToInt32(objShipTable.Rows[s]["ShipmentType"].ToString()));
                                cmd.Parameters.AddWithValue("@poshipment_branch_gid", Convert.ToInt32(objShipTable.Rows[s]["poshipment_branch_gid"].ToString()));
                                cmd.Parameters.AddWithValue("@poshipment_qty", Convert.ToDecimal(objShipTable.Rows[s]["Quantity"].ToString()));
                                cmd.Parameters.AddWithValue("@poshipment_empgid", Convert.ToInt32(objShipTable.Rows[s]["employeegid"].ToString()));
                                cmd.Parameters.AddWithValue("@poshipment_podet_gid", Convert.ToInt32(objShipTable.Rows[s]["poshipment_podet_gid"].ToString()));
                                cmd.Parameters.AddWithValue("@actionName", "insertshipment");
                                objpoheader.result = Convert.ToString(cmd.ExecuteNonQuery());
                            }
                        }


                        if (objShipExcelTable.Rows.Count > 0)
                        {
                            for (int x = 0; x < objShipExcelTable.Rows.Count; x++)
                            {
                                if (Convert.ToInt32(objShipExcelTable.Rows[x]["poshipment_gid"].ToString()) > 0)
                                {


                                    if ((objShipExcelTable.Rows[x]["deleteflag"].ToString()) == "Y")
                                    {
                                        cmd = new SqlCommand("pr_fb_trn_poinsert", con);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@poshipment_gid", Convert.ToInt32(objShipExcelTable.Rows[x]["poshipment_gid"].ToString()));
                                        cmd.Parameters.AddWithValue("@actionName", "deleteshipment");
                                        objpoheader.result = Convert.ToString(cmd.ExecuteNonQuery());
                                    }
                                    else
                                    {
                                        //   cmd.Parameters.AddWithValue("@podetails_cbfdet_gid", Convert.ToInt32(objExShip.Rows[ex]["cbfdetgid"].ToString()));
                                        cmd = new SqlCommand("pr_fb_trn_poinsert", con);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@poshipment_type_gid", Convert.ToInt32(objShipExcelTable.Rows[x]["ShipmentType"].ToString()));
                                        cmd.Parameters.AddWithValue("@poshipment_branch_gid", Convert.ToInt32(objShipExcelTable.Rows[x]["poshipment_branch_gid"].ToString()));
                                        cmd.Parameters.AddWithValue("@poshipment_qty", Convert.ToDecimal(objShipExcelTable.Rows[x]["Quantity"].ToString()));
                                        cmd.Parameters.AddWithValue("@poshipment_empgid", Convert.ToInt32(objShipExcelTable.Rows[x]["employeegid"].ToString()));
                                        cmd.Parameters.AddWithValue("@poshipment_podet_gid", Convert.ToInt32(objShipExcelTable.Rows[x]["poshipment_gid"].ToString()));
                                        cmd.Parameters.AddWithValue("@actionName", "updateshipment");
                                        objpoheader.result = Convert.ToString(cmd.ExecuteNonQuery());
                                    }
                                }
                                else
                                {
                                    cmd = new SqlCommand("pr_fb_trn_poinsert", con);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    //   cmd.Parameters.AddWithValue("@podetails_cbfdet_gid", Convert.ToInt32(objExShip.Rows[ex]["cbfdetgid"].ToString()));
                                    cmd.Parameters.AddWithValue("@poshipment_type_gid", Convert.ToInt32(objShipExcelTable.Rows[x]["ShipmentType"].ToString()));
                                    cmd.Parameters.AddWithValue("@poshipment_branch_gid", Convert.ToInt32(objShipExcelTable.Rows[x]["poshipment_branch_gid"].ToString()));
                                    cmd.Parameters.AddWithValue("@poshipment_empgid", Convert.ToInt32(objShipExcelTable.Rows[x]["employeegid"].ToString()));
                                    cmd.Parameters.AddWithValue("@poshipment_qty", Convert.ToDecimal(objShipExcelTable.Rows[x]["Quantity"].ToString()));
                                    cmd.Parameters.AddWithValue("@poshipment_podet_gid", Convert.ToInt32(objShipExcelTable.Rows[x]["poshipment_podet_gid"].ToString()));
                                    cmd.Parameters.AddWithValue("@actionName", "insertshipment");
                                    objpoheader.result = Convert.ToString(cmd.ExecuteNonQuery());
                                }
                            }
                        }
                        //if(objdelShip.Rows.Count>0)
                        //{
                        //    for(int s=0;s<objdelShip.Rows.Count;s++)
                        //    {
                        //        cmd = new SqlCommand("pr_fb_trn_poinsert", con);
                        //        cmd.CommandType = CommandType.StoredProcedure;
                        //        cmd.Parameters.AddWithValue("@poshipment_gid", Convert.ToInt32(objShipExcelTable.Rows[s]["poshipment_gid"].ToString()));
                        //        cmd.Parameters.AddWithValue("@actionName", "deleteshipment");
                        //        objpoheader.result = Convert.ToString(cmd.ExecuteNonQuery());
                        //    }
                        //}
                    }
                }


                if (objpoheader.actionName == "Submit" || objpoheader.actionName == "amend")
                {
                    cmd = new SqlCommand("pr_fb_trn_apprqueuePO", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@refgid", objpoheader.poheadGid);
                    cmd.Parameters.AddWithValue("@empgid", objCmnFunctions.GetLoginUserGid());
                    cmd.Parameters.AddWithValue("@requesttype", "PO");
                    objpoheader.result = Convert.ToString(cmd.ExecuteNonQuery());
                }
                if (HttpContext.Current.Session["ArrayList"] != null)
                {
                    ArrayList lalistpodetail = (ArrayList)HttpContext.Current.Session["ArrayList"];
                    if (lalistpodetail.Count > 0)
                        for (int j = 0; j < lalistpodetail.Count; j++)
                        {
                            DeletePoDetails(Convert.ToInt32(lalistpodetail[j]));
                        }
                }
                if (HttpContext.Current.Session["shiparraylist"] != null)
                {
                    ArrayList lalistshipdetail = (ArrayList)HttpContext.Current.Session["shiparraylist"];
                    if (lalistshipdetail.Count > 0)
                        for (int j = 0; j < lalistshipdetail.Count; j++)
                        {
                            DeleteShipdetails(Convert.ToInt32(lalistshipdetail[j]));
                        }
                }
                if (objpoheader.result != "")
                {
                    objpoheader.result = "Updated Successfully";
                }
                return objpoheader.result;
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

        public void DeleteShipdetails(int podetailgid)
        {
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_poinsert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@poshipment_podet_gid", podetailgid);
                cmd.Parameters.AddWithValue("@actionName", "deleteshipment");
                int res = cmd.ExecuteNonQuery();
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

        public string DeletePoSummaryDetails(poraiser obj)
        {
            try
            {

                string[,] wherecond = new string[,]
	        {
                {"poheader_gid", obj.poheadGid.ToString ()}
            };
                string[,] column = new string[,]
	       {
                 {"poheader_isremoved", "Y"}
	            
           };
                string tblname = "fb_trn_tpoheader";
                obj.result = objcommon.DeleteCommon(column, wherecond, tblname);
                if (obj.result == "1")
                {
                    string[,] wherecond1 = new string[,]
	        {
                {"podetails_pohead_gid", obj.poheadGid.ToString ()}
            };
                    string[,] column1 = new string[,]
	       {
                 {"podetails_isremoved", "Y"}
	            
           };
                    string tblname1 = "fb_trn_tpodetails";
                    obj.result = objcommon.DeleteCommon(column1, wherecond1, tblname1);
                }

                return obj.result;
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
        public string CancelPoSummaryDetails(poraiser obj)
        {
            try
            {
                getconnection();

                string[,] wherecond = new string[,]
	        {
                {"poheader_gid", obj.poheadGid.ToString ()}
            };
                string[,] column = new string[,]
	       {
                 {"poheader_iscancelled", "M"},
                 {"poheader_status","7"}
	            
           };
                string tblname = "fb_trn_tpoheader";
                obj.result = objcommon.DeleteCommon(column, wherecond, tblname);
                if (obj.result == "Sucess")
                {
                    string[,] columnval = new string[,]
	       {
                 {"pocancellation_pohead_gid",obj.poheadGid.ToString()},
                 {"pocancellation_mak_gid",objCmnFunctions.GetLoginUserGid().ToString()},
                 {"pocancellation_mak_remarks",(string.IsNullOrEmpty(obj.remarks.ToString()) ? string.Empty :obj.remarks.ToString())},
                 {"pocancellation_mak_date",DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss")}
           };
                    string tblname1 = "fb_trn_tpocancellation";
                    obj.result = objcommon.InsertCommon(columnval, tblname1);

                    DataTable objtable = new DataTable();
                    cmd = new SqlCommand("pr_fb_trn_poinsert", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@poheader_gid", obj.poheadGid.ToString());
                    cmd.Parameters.AddWithValue("@actionName", "clearcbfFlag");
                    da = new SqlDataAdapter(cmd);
                    da.Fill(objtable);
                    if (objtable.Rows.Count > 0)
                    {
                        if (!objtable.Columns.Contains("resultqty"))
                            objtable.Columns.Add("resultqty");
                        for (int j = 0; j < objtable.Rows.Count; j++)
                        {
                            objtable.Rows[j]["resultqty"] = Convert.ToInt32(Convert.ToInt32(objtable.Rows[j]["cbfdetails_qty"]) - Convert.ToInt32(objtable.Rows[j]["poqty"]));
                            if (Convert.ToInt32(objtable.Rows[j]["resultqty"]) == 0)
                            {
                                cmd = new SqlCommand("pr_fb_trn_poinsert", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@actionName", "cbfFlag");
                                cmd.Parameters.AddWithValue("@cbfdetails_gid", Convert.ToInt32(objtable.Rows[j]["cbfdetails_gid"]));
                                int result = cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    int queue_type = Convert.ToInt32(ConfigurationManager.AppSettings["pocancelrolegid"].ToString());
                    int queue_ref_flag = Convert.ToInt32(ConfigurationManager.AppSettings["porefgid"].ToString());
                    string[,] codes = new string[,]
                                {
                                {"queue_date", "sysdatetime()"},
                                {"queue_ref_flag",queue_ref_flag.ToString()},
                                {"queue_ref_gid",obj.poheadGid.ToString()},
                                {"queue_from",objCmnFunctions.GetLoginUserGid().ToString()},
                                {"queue_to_type", "G"},
                                {"queue_to", queue_type.ToString()},
                                {"queue_action_for","A"},
                                {"queue_prev_gid",""}
                                };
                    string tname = "iem_trn_tqueue";
                    string queue_result = objcommon.InsertCommon(codes, tname);
                }
                return obj.result;
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
        public List<poRaising> GetCancelPoSummaryDetails()
        {
            DataTable objtable = new DataTable();
            List<poRaising> objraiser = new List<poRaising>();
            poRaising objDetails;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_pocancellation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "PoCancellationSummary");
                cmd.Parameters.AddWithValue("@logingid", objCmnFunctions.GetLoginUserGid().ToString());
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objDetails = new poRaising();
                    objDetails.slno = i + 1;
                    objDetails.poDetgid = Convert.ToInt32(objtable.Rows[i]["poheader_gid"].ToString());
                    objDetails.poCancelGid = Convert.ToInt32(objtable.Rows[i]["pocancellation_gid"].ToString());
                    objDetails.poDate = objtable.Rows[i]["poheader_date"].ToString();
                    objDetails.remarks = objtable.Rows[i]["pocancellation_mak_remarks"].ToString();
                    objDetails.poRefNo = objtable.Rows[i]["poheader_pono"].ToString();
                    objDetails.vendorName = objtable.Rows[i]["supplierheader_name"].ToString();
                    objDetails.status = objtable.Rows[i]["status_name"].ToString();
                    objDetails.poAmount = objtable.Rows[i]["poheader_over_total"].ToString();
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
                con.Close();
            }
        }
        public int PoCancelApprove(poRaising obj)
        {
            try
            {
                getconnection();
                string queue_prevgid = string.Empty;
                cmd = new SqlCommand("pr_fb_trn_pocancellation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pocancellation_chk_gid", objCmnFunctions.GetLoginUserGid().ToString());
                if (obj.viewCancel == "rejected")
                {
                    cmd.Parameters.AddWithValue("@pocancellation_chk_status", "R");
                    cmd.Parameters.AddWithValue("@pocancellation_isremoved", "Y");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@pocancellation_chk_status", "A");
                    cmd.Parameters.AddWithValue("@pocancellation_isremoved", "N");
                }
                //cmd.Parameters.AddWithValue("@actionName", "PoCancellationchecker");
                cmd.Parameters.AddWithValue("@pocancellation_chk_remarks", (string.IsNullOrEmpty(obj.remarks) ? string.Empty : obj.remarks.ToString()));
                //   cmd.Parameters.AddWithValue("@pocancellation_chk_remarks", obj.remarks);
                cmd.Parameters.AddWithValue("@pocancellation_gid", obj.poCancelGid);
                cmd.Parameters.AddWithValue("@pocancellation_pohead_gid", obj.poheadGid);
                cmd.Parameters.AddWithValue("@actionName", "PoCancellationchecker");
                obj.result = cmd.ExecuteNonQuery();
                if (obj.result == 2 && string.IsNullOrEmpty(obj.viewCancel) == true)
                {

                    int porefgid = Convert.ToInt32(ConfigurationManager.AppSettings["porefgid"].ToString());
                    string[,] wherecond = new string[,]
	              {
                         {"queue_ref_gid",obj.poheadGid.ToString()},
                         {"queue_ref_flag",porefgid.ToString()},
                         {"queue_isremoved","N"}
                      };
                    string[,] column = new string[,]
	             {
                    {"queue_isremoved", "Y"},
                    {"queue_action_date","sysdatetime()"},
                    {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                    {"queue_action_for","A"},
                    {"queue_action_status","1"},
                    {"queue_action_remark",(string.IsNullOrEmpty(obj.remarks) ? string.Empty : obj.remarks.ToString())},
                 };
                    string tblname = "iem_trn_tqueue";
                    string result = objcommon.UpdateCommon(column, wherecond, tblname);
                    //if (result == "Success")
                    //{
                    //    cmd = new SqlCommand("pr_fb_trn_POApprovalQueue", con);
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Parameters.AddWithValue("@refgid", obj.poheadGid.ToString());
                    //    cmd.Parameters.AddWithValue("@actionName", "queueprev");
                    //    queue_prevgid = (string)cmd.ExecuteScalar();
                    //    string[,] wherecond12 = new string[,]
                    //  {
                    //     {"queue_ref_gid",obj.poheadGid.ToString()},
                    //     {"queue_ref_flag",porefgid.ToString()}
                    //  };
                    //    string[,] column12 = new string[,]
                    //{
                    //{"queue_prev_gid",queue_prevgid.ToString()}
                    //};
                    //    string tblname12 = "iem_trn_tqueue";
                    //    result = objcommon.UpdateCommon(column12, wherecond12, tblname12);
                    //}
                }


                if (obj.result == 2 && obj.viewCancel == "rejected")
                {
                    int porefgid = Convert.ToInt32(ConfigurationManager.AppSettings["porefgid"].ToString());
                    string[,] wherecond = new string[,]
	              {
                         {"queue_ref_gid",obj.poheadGid.ToString()},
                         {"queue_ref_flag",porefgid.ToString()}
                      };
                    string[,] column = new string[,]
	             {
                    {"queue_action_date","sysdatetime()"},
                    {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                    {"queue_action_status","2"},
                    {"queue_action_for","R"},
                    {"queue_action_remark",(string.IsNullOrEmpty(obj.remarks) ? string.Empty : obj.remarks.ToString())},
                 };
                    string tblname = "iem_trn_tqueue";
                    string result = objcommon.UpdateCommon(column, wherecond, tblname);

                }
                return obj.result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj.result;
            }
            finally
            {
                con.Close();
            }
        }
        public void DeletePoDetails(int poDetGid)
        {
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_poinsert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "clearFlag");
                cmd.Parameters.AddWithValue("@podetails_gid", poDetGid);
                int a = cmd.ExecuteNonQuery();
                string[,] wherecond = new string[,]
	        {
                {"podetails_gid", poDetGid.ToString ()}
            };
                string[,] column = new string[,]
	       {
                 {"podetails_isremoved", "Y"}
	            
           };
                string tblname = "fb_trn_tpodetails";
                string result = objcommon.DeleteCommon(column, wherecond, tblname);
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

        public List<poRaising> GetPoCheckerSummary()
        {
            DataTable objtable = new DataTable();
            List<poRaising> objraiser = new List<poRaising>();
            poRaising objDetails;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_poheader", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employeeegid", objCmnFunctions.GetLoginUserGid().ToString());
                cmd.Parameters.AddWithValue("@actionName", "selectforpochecker");
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objDetails = new poRaising();
                    objDetails.slno = i + 1;
                    objDetails.poDetgid = Convert.ToInt32(objtable.Rows[i]["poheader_gid"].ToString());
                    //objDetails.poCancelGid = Convert.ToInt32(objtable.Rows[i]["pocancellation_gid"].ToString());
                    objDetails.poDate = objtable.Rows[i]["poheader_date"].ToString();
                    //objDetails.remarks = objtable.Rows[i]["pocancellation_mak_remarks"].ToString();
                    objDetails.poRefNo = objtable.Rows[i]["poheader_pono"].ToString();
                    objDetails.vendorName = objtable.Rows[i]["supplierheader_name"].ToString();
                    objDetails.status = objtable.Rows[i]["status_name"].ToString();
                    objDetails.poAmount = objtable.Rows[i]["poheader_over_total"].ToString();
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
                con.Close();
            }
        }

        public string PoCheckerApprove(poRaising obj)
        {
            try
            {
                string result = string.Empty;
                int qTo = 0;
                int a = 0;
                int queue_prevgid = 0;
                getconnection();
                if (obj.viewCancel == "rejected")
                {

                    string[,] codw = new string[,]
            {
                {"queue_action_for","R"},
                {"queue_action_status","2"},
                {"queue_isremoved","Y"},
                {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                {"queue_action_date","sysdatetime()"},
                {"queue_action_remark",obj.remarks}
            };
                    string[,] codewhe = new string[,]
            {
                {"queue_ref_gid",obj.poheadGid.ToString()},
                {"queue_ref_flag","8"},
                {"queue_isremoved","N"}
            };

                    string tblname = "iem_trn_tqueue";
                    string updatecon = objcommon.UpdateCommon(codw, codewhe, tblname);
                    //cmd = new SqlCommand("pr_fb_trn_apprqueuePO", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@refgid", obj.poheadGid);
                    ////int rolegid= Convert.ToInt32(ConfigurationManager.AppSettings["pocancelrolegid"].ToString());
                    ////int porefFlag = Convert.ToInt32(ConfigurationManager.AppSettings["porefgid"].ToString());
                    //cmd.Parameters.AddWithValue("@empgid", objCmnFunctions.GetLoginUserGid().ToString());
                    //if (obj.viewCancel == "rejected")
                    //{
                    //    cmd.Parameters.AddWithValue("@actionstatus", "2");
                    //}
                    //cmd.Parameters.AddWithValue("@actionremark ", (string.IsNullOrEmpty(obj.remarks) ? string.Empty : obj.remarks.ToString()));
                    //cmd.Parameters.AddWithValue("@requesttype", "PO");
                    //obj.result = cmd.ExecuteNonQuery();


                }
                if (obj.result != null && obj.viewCancel == "rejected")
                {
                    string[,] wherecond = new string[,]
	              {
                {"poheader_gid", obj.poheadGid.ToString ()}
                  };
                    string[,] column = new string[,]
	             {
                      {"poheader_status", "6"},
                      {"poheader_currentapprovalstage","0"}
                 };
                    string tblname = "fb_trn_tpoheader";
                    result = objcommon.UpdateCommon(column, wherecond, tblname);
                }
                else
                {
                    string[,] wherecond = new string[,]
	              {
                {"poheader_gid", obj.poheadGid.ToString ()}
                  };
                    string[,] column = new string[,]
	             {
                      {"poheader_status", "4"}
                 };
                    string tblname = "fb_trn_tpoheader";
                    result = objcommon.UpdateCommon(column, wherecond, tblname);
                    if (result == "Success")
                    {
                        //cmd = new SqlCommand("pr_fb_trn_POApprovalQueue", con);
                        //cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("@refgid", obj.poheadGid);
                        //cmd.Parameters.AddWithValue("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                        //cmd.Parameters.AddWithValue("@actionremark", (string.IsNullOrEmpty(obj.remarks) ? string.Empty : obj.remarks.ToString()));
                        //cmd.Parameters.AddWithValue("@request_type", "PO");
                        //cmd.Parameters.AddWithValue("@actionName", "updatepoqueue");
                        //a = cmd.ExecuteNonQuery();
                        int porefFlag = Convert.ToInt32(ConfigurationManager.AppSettings["porefgid"].ToString());
                        string[,] wherecol = new string[,]
	              {
                              {"queue_ref_gid", obj.poheadGid.ToString ()},
                              {"queue_ref_flag",porefFlag.ToString()},
                              {"queue_isremoved","N"}   
                  };
                        string[,] columnNames = new string[,]
	             {
                      {"queue_action_date", "sysdatetime()"},
                      {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                      {"queue_action_status","1"},
                      {"queue_action_remark",Convert.ToString(string.IsNullOrEmpty(obj.remarks) ? string.Empty : obj.remarks)},
                      {"queue_prev_gid",queue_prevgid.ToString()},
                      {"queue_isremoved","Y"}
                 };
                        string tbl = "iem_trn_tqueue";
                        result = objcommon.UpdateCommon(columnNames, wherecol, tbl);

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
                con.Close();
            }
        }
        //Po Closure maker Summary
        public List<poRaising> GetPoClosureSummary()
        {
            DataTable objtable = new DataTable();
            List<poRaising> objraiser = new List<poRaising>();
            poRaising objDetails;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_poclosure", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "select");
                cmd.Parameters.AddWithValue("@logingid", objCmnFunctions.GetLoginUserGid().ToString());
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objDetails = new poRaising();
                    objDetails.slno = i + 1;
                    objDetails.poDetgid = Convert.ToInt32(objtable.Rows[i]["poheader_gid"].ToString());
                    //objDetails.poCancelGid = Convert.ToInt32(objtable.Rows[i]["pocancellation_gid"].ToString());
                    objDetails.poDate = objtable.Rows[i]["poheader_date"].ToString();
                    //objDetails.remarks = objtable.Rows[i]["pocancellation_mak_remarks"].ToString();
                    objDetails.poRefNo = objtable.Rows[i]["poheader_pono"].ToString();
                    objDetails.vendorName = objtable.Rows[i]["supplierheader_name"].ToString();
                    objDetails.status = objtable.Rows[i]["status_name"].ToString();
                    objDetails.poAmount = objtable.Rows[i]["poheader_over_total"].ToString();
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
                con.Close();
            }
        }
        //PO Maker Closure Insertion 
        public string ClosurePoSummaryDetails(poraiser obj)
        {
            try
            {
                getconnection();

                string[,] wherecond = new string[,]
	        {
                {"poheader_gid", obj.poheadGid.ToString ()}
            };
                string[,] column = new string[,]
	       {
                 {"poheader_isclosed", "M"}
	            
           };
                string tblname = "fb_trn_tpoheader";
                obj.result = objcommon.UpdateCommon(column, wherecond, tblname);
                if (obj.result == "Success")
                {
                    string[,] columnval = new string[,]
	       {
                 {"poclosure_pohead_gid",obj.poheadGid.ToString()},
                 {"poclosure_mak_gid",objCmnFunctions.GetLoginUserGid().ToString()},
                 {"poclosure_mak_remarks",(string.IsNullOrEmpty(obj.remarks.ToString()) ? "" :obj.remarks.ToString())},
                 {"poclosure_mak_date","sysdatetime()"}
           };
                    string tblname1 = "fb_trn_tpoclosure";
                    obj.result = objcommon.InsertCommon(columnval, tblname1);

                    if (obj.result == "success")
                    {
                        int queue_type = Convert.ToInt32(ConfigurationManager.AppSettings["poclosurerolegid"].ToString());
                        int queue_ref_flag = Convert.ToInt32(ConfigurationManager.AppSettings["porefgid"].ToString());
                        string[,] codes = new string[,]
                                {
                                {"queue_date", "sysdatetime()"},
                                {"queue_ref_flag",queue_ref_flag.ToString()},
                                {"queue_ref_gid",obj.poheadGid.ToString()},
                                {"queue_from",objCmnFunctions.GetLoginUserGid().ToString()},
                                {"queue_to_type", "G"},
                                {"queue_to", queue_type.ToString()},
                                {"queue_action_for","A"}
                               
                                };
                        string tname = "iem_trn_tqueue";
                        string queue_result = objcommon.InsertCommon(codes, tname);
                    }
                }
                return obj.result;

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
        //PO Checker Closure Insertion 
        public string ClosurePoSummaryInsert(poraiser obj)
        {
            try
            {
                getconnection();
                string queue_prevgid = string.Empty;
                string result = string.Empty;
                string[,] wherecond = new string[,]
	        {
                {"poheader_gid", obj.poheadGid.ToString ()}
            };
                if (obj.viewfor == "reject")
                {
                    string[,] column = new string[,]
	       {
                 {"poheader_isclosed", "N"},
                    {"poheader_status","5"}
           };
                    string tblname = "fb_trn_tpoheader";
                    obj.result = objcommon.UpdateCommon(column, wherecond, tblname);
                }
                else
                {
                    string[,] column = new string[,]
	       {
                 {"poheader_isclosed", "C"}
                
           };
                    string tblname = "fb_trn_tpoheader";
                    obj.result = objcommon.UpdateCommon(column, wherecond, tblname);
                }
                if (obj.result == "Success" && obj.viewfor == "reject")
                {
                    string[,] columnval = new string[,]
	       {
                 {"poclosure_pohead_gid",obj.poheadGid.ToString()},
                 {"poclosure_chk_gid",objCmnFunctions.GetLoginUserGid().ToString()},
                 {"poclosure_chk_remarks",(string.IsNullOrEmpty(obj.remarks.ToString()) ? string.Empty :obj.remarks.ToString())},
                 {"poclosure_chk_status","R"},
                 {"poclosure_chk_date","sysdatetime()"}
           };
                    string[,] wherecolumn = new string[,]
                    {
                      {"poclosure_gid",obj.poClosureGid.ToString()}

                    };
                    string tblname1 = "fb_trn_tpoclosure";
                    obj.result = objcommon.UpdateCommon(columnval, wherecolumn, tblname1);
                    if (obj.result == "Success")
                    {
                        int porefFlag = Convert.ToInt32(ConfigurationManager.AppSettings["porefgid"].ToString());
                        string[,] wherecond1 = new string[,]
	              {
                         {"queue_ref_gid",obj.poheadGid.ToString()},
                         {"queue_ref_flag",porefFlag.ToString()},
                        
                  };
                        string[,] column = new string[,]
	             {
                         {"queue_action_date","sysdatetime()"},
                         {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                         {"queue_action_status","2"},
                         {"queue_action_for","R"},
                         {"queue_action_remark",(string.IsNullOrEmpty(obj.remarks) ? string.Empty :obj.remarks.ToString())}
                 };
                        string tblname = "iem_trn_tqueue";
                        result = objcommon.UpdateCommon(column, wherecond1, tblname);
                    }
                }

                else if (obj.result == "Success")
                {
                    string[,] columnval = new string[,]
	       {
                 {"poclosure_pohead_gid",obj.poheadGid.ToString()},
                 {"poclosure_chk_gid",objCmnFunctions.GetLoginUserGid().ToString()},
                 {"poclosure_chk_remarks",(string.IsNullOrEmpty(obj.remarks) ? string.Empty :obj.remarks.ToString())},
                 {"poclosure_chk_status","A"},
                 {"poclosure_chk_date","sysdatetime()"}
           };
                    string[,] wherecolumn = new string[,]
                    {
                      {"poclosure_gid",obj.poClosureGid.ToString()}

                    };
                    string tblname1 = "fb_trn_tpoclosure";
                    obj.result = objcommon.UpdateCommon(columnval, wherecolumn, tblname1);
                    if (obj.result == "Success")
                    {
                        int porefFlag = Convert.ToInt32(ConfigurationManager.AppSettings["porefgid"].ToString());
                        int poclosuregid = Convert.ToInt32(ConfigurationManager.AppSettings["poclosurerolegid"].ToString());
                        string[,] wherecond1 = new string[,]
	                  {
                         {"queue_ref_gid",obj.poheadGid.ToString()},
                         {"queue_ref_flag",porefFlag.ToString()},
                         {"queue_isremoved","N"},
                         {"queue_to",poclosuregid.ToString()}
                         
                      };
                        string[,] columnvalues = new string[,]
	             {
                         {"queue_isremoved", "Y"},
                         {"queue_action_date","sysdatetime()"},
                         {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                         {"queue_action_status","1"},
                         {"queue_action_for","A"},
                         {"queue_prev_gid","0"},
                         {"queue_action_remark",(string.IsNullOrEmpty(obj.remarks) ? string.Empty :obj.remarks.ToString())}
                 };
                        string tableName = "iem_trn_tqueue";
                        result = objcommon.UpdateCommon(columnvalues, wherecond1, tableName);

                        //       if (result == "Success")
                        //       {
                        //           cmd = new SqlCommand("pr_fb_trn_POApprovalQueue", con);
                        //           cmd.CommandType = CommandType.StoredProcedure;
                        //           cmd.Parameters.AddWithValue("@actionName", "queueprev");
                        //           queue_prevgid = (string)cmd.ExecuteScalar();

                        //           string[,] wherecol = new string[,]
                        //     {
                        //        {"queue_ref_gid",obj.poheadGid.ToString()},
                        //        {"queue_ref_flag",porefFlag.ToString()},

                        //     };
                        //           string[,] columns = new string[,]
                        //{
                        //    {"queue_prev_gid",queue_prevgid.ToString()}
                        //};
                        //           string tblName = "iem_trn_tqueue";
                        //           result = objcommon.UpdateCommon(columns, wherecol, tblName);
                        //       }
                    }
                }
                return obj.result;


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
        //Po Closure Checker Summary
        public List<poRaising> GetPoClosureCheckerSummary()
        {
            DataTable objtable = new DataTable();
            List<poRaising> objraiser = new List<poRaising>();
            poRaising objDetails;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_poclosure", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectforchecker");
                cmd.Parameters.AddWithValue("@logingid", objCmnFunctions.GetLoginUserGid().ToString());
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objDetails = new poRaising();
                    objDetails.slno = i + 1;
                    objDetails.poDetgid = Convert.ToInt32(objtable.Rows[i]["poheader_gid"].ToString());
                    //objDetails.poCancelGid = Convert.ToInt32(objtable.Rows[i]["pocancellation_gid"].ToString());
                    objDetails.poDate = objtable.Rows[i]["poheader_date"].ToString();
                    //objDetails.remarks = objtable.Rows[i]["pocancellation_mak_remarks"].ToString();
                    objDetails.poRefNo = objtable.Rows[i]["poheader_pono"].ToString();
                    objDetails.remarks = objtable.Rows[i]["poclosure_mak_remarks"].ToString();
                    objDetails.vendorName = objtable.Rows[i]["supplierheader_name"].ToString();
                    objDetails.status = objtable.Rows[i]["status_name"].ToString();
                    objDetails.poAmount = objtable.Rows[i]["poheader_over_total"].ToString();
                    objDetails.poclosureGid = Convert.ToInt32(objtable.Rows[i]["poclosure_gid"].ToString());
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
                con.Close();
            }
        }
        public string GetPoClosureExcelValid(string validate, string pono, string action)
        {
            string result = "";
            try
            {
                getconnection();
                DataTable objtable = new DataTable();


                if (pono != null && pono != "")
                {
                    cmd = new SqlCommand("pr_fb_trn_poClosureExceValidate", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@chkdate", SqlDbType.VarChar).Value = validate.ToString();
                    cmd.Parameters.Add("@chkdata", SqlDbType.VarChar).Value = pono.ToString();
                    cmd.Parameters.Add("@result", SqlDbType.VarChar).Value = action;
                    result = (string)cmd.ExecuteScalar();
                }
                else
                {
                    cmd = new SqlCommand("pr_fb_trn_poClosureExceValidate", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@chkdata", SqlDbType.VarChar).Value = validate.ToString();
                    //cmd.Parameters.Add("@chkdate", SqlDbType.VarChar).Value = validate.ToString();
                    cmd.Parameters.Add("@result", SqlDbType.VarChar).Value = action;
                    result = (string)cmd.ExecuteScalar();
                }
                if (result == "Exists")
                {
                    if (pono != null && pono != "")
                    {
                        cmd = new SqlCommand("pr_fb_trn_poClosureExceValidate", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@result", SqlDbType.VarChar).Value = "checkforclosuredate";
                        cmd.Parameters.Add("@chkdate", SqlDbType.VarChar).Value = validate.ToString();
                        cmd.Parameters.Add("@chkdata", SqlDbType.VarChar).Value = pono.ToString();
                        da = new SqlDataAdapter(cmd);
                        da.Fill(objtable);
                        string status = ClosureValidate(objtable);
                        if (status == "N")
                        {
                            result = "Exists";
                        }
                        else
                        {
                            result = "notexists";
                        }
                        //if (objtable.Rows.Count == 0)
                        //{
                        //    cmd.Parameters.Add("@res", SqlDbType.VarChar, 64);
                        //    cmd.Parameters["@res"].Direction = ParameterDirection.Output;
                        //    result = Convert.ToString(cmd.Parameters["@res"].Value.ToString());
                        //}
                    }
                    else
                    {
                        cmd = new SqlCommand("pr_fb_trn_poClosureExceValidate", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@result", SqlDbType.VarChar).Value = "checkforclosure";
                        cmd.Parameters.Add("@chkdata", SqlDbType.VarChar).Value = validate.ToString();
                        da = new SqlDataAdapter(cmd);
                        da.Fill(objtable);
                        string status = ClosureValidate(objtable);
                        if (status == "N")
                        {
                            result = "Exists";
                        }
                        else
                        {
                            result = "notexists";
                        }
                        //if (objtable.Rows.Count == 0)
                        //{
                        //    cmd.Parameters.Add("@res", SqlDbType.VarChar, 64);
                        //    cmd.Parameters["@res"].Direction = ParameterDirection.Output;
                        //   // result = cmd.ExecuteNonQuery().ToString();
                        //}
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
                con.Close();
            }

        }
        public string ClosureValidate(DataTable objtable)
        {
            try
            {
                string result = string.Empty;
                if (objtable.Rows.Count > 0)
                {
                    for (int i = 0; i < objtable.Rows.Count; i++)
                    {
                        if (Convert.ToInt32(objtable.Rows[i]["podetails_qty"]) == Convert.ToInt32(objtable.Rows[i]["grnreleaseforpo_released_qty"]))
                        {
                            result = "Y";

                        }
                        else
                        {
                            result = "N";
                            return result;
                        }
                    }
                    return result;
                }
                else
                {
                    return result = "N";
                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }

        }
        public string BulkPoInsert(DataTable dt, int validRecords, string fileName)
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
                    if (!dt.Columns.Contains("poheadgid"))
                        dt.Columns.Add("poheadgid");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cmd = new SqlCommand("pr_fb_trn_poclosure", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@pono", dt.Rows[i]["PONo"].ToString());
                        cmd.Parameters.AddWithValue("@poheaddate", objCmnFunctions.convertoDateTimeString(dt.Rows[i]["PoDate"].ToString()));
                        cmd.Parameters.AddWithValue("@actionName", "selectpoheadgid");
                        dt.Rows[i]["poheadgid"] = cmd.ExecuteScalar();

                        string[,] columnval = new string[,]
	               {
                        {"poclosure_pohead_gid",dt.Rows[i]["poheadgid"].ToString()},
                         {"poclosure_mak_gid",objCmnFunctions.GetLoginUserGid().ToString()},
                        {"poclosure_mak_remarks",dt.Rows[i]["Remarks"].ToString()},
                        {"poclosure_mak_date",DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss")}
                  };

                        string tblname1 = "fb_trn_tpoclosure";
                        result = objcommon.InsertCommon(columnval, tblname1);
                        if (result == "success")
                        {

                            string[,] columnval1 = new string[,]
	                    {
                             {"poheader_isclosed","M"},
                        };
                            string[,] wherecolumn = new string[,]
                        {
                      {"poheader_gid",dt.Rows[i]["poheadgid"].ToString()}

                        };
                            string tblname = "fb_trn_tpoheader";
                            result = objcommon.UpdateCommon(columnval1, wherecolumn, tblname);
                            HttpContext.Current.Session["maindatatable"] = null;
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
        }


        //WO Summary
        public List<WoSummary> GetWoSummary()
        {
            List<WoSummary> objraiser = new List<WoSummary>();
            try
            {
                getconnection();
                DataTable objtable = new DataTable();
                WoSummary objDetails;
                cmd = new SqlCommand("pr_fb_trn_woheader", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "select");
                cmd.Parameters.AddWithValue("@logingid", objCmnFunctions.GetLoginUserGid().ToString());
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objDetails = new WoSummary();
                    objDetails.slno = Convert.ToInt32(objtable.Rows[i]["slno"]);
                    objDetails.woDetgid = Convert.ToInt32(objtable.Rows[i]["poheader_gid"]);
                    objDetails.woDate = Convert.ToString(objtable.Rows[i]["poheader_date"]);
                    objDetails.woRefNo = objtable.Rows[i]["poheader_pono"].ToString();
                    objDetails.vendorName = objtable.Rows[i]["supplierheader_name"].ToString();
                    objDetails.status = objtable.Rows[i]["status_name"].ToString();
                    objDetails.woAmount = objtable.Rows[i]["poheader_over_total"].ToString();
                    objDetails.Version = Convert.ToInt32(objtable.Rows[i]["poheader_version"].ToString());
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
                con.Close();
            }
        }

        //objfselection

        public List<obfselection> getobfheader(string requestGroup)
        {
            List<obfselection> objselection = new List<obfselection>();
            try
            {
                getconnection();
                DataTable objtable = new DataTable();

                obfselection objDetails;
                cmd = new SqlCommand("pr_fb_trn_obfselection", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "select");
                cmd.Parameters.AddWithValue("@requestfor_name", requestGroup);
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objDetails = new obfselection();
                    objDetails.slno = i + 1;
                    objDetails.obfheadgid = Convert.ToInt64(objtable.Rows[i]["cbfheader_gid"]);
                    objDetails.obfmode = Convert.ToString(objtable.Rows[i]["cbfheader_mode"]);
                    objDetails.obfno = Convert.ToString(objtable.Rows[i]["cbfheader_cbfno"]);
                    objDetails.obfdate = objtable.Rows[i]["startdate"].ToString();
                    // objDetails.obfEnddate = objtable.Rows[i]["enddate"].ToString();
                    objDetails.department = objtable.Rows[i]["requestfor_name"].ToString();
                    objDetails.finconBudget = objtable.Rows[i]["cbfheader_isbudgeted"].ToString();
                    objDetails.obfamount = Convert.ToDecimal(objtable.Rows[i]["cbfheader_cbfamt"].ToString());
                    // objDetails.projectowner = objtable.Rows[i]["projectowner_name"].ToString();
                    //objDetails.deviationamount = Convert.ToDecimal(objtable.Rows[i]["cbfheader_Devi_amt"].ToString());
                    // objDetails.status = objtable.Rows[i]["status_name"].ToString();
                    objDetails.description = objtable.Rows[i]["cbfheader_desc"].ToString();
                    objselection.Add(objDetails);
                }
                return objselection;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objselection;
            }
            finally
            {
                con.Close();
            }
        }
        public List<obfdetail> getobflist(DataTable dt)
        {
            List<obfdetail> objlist = new List<obfdetail>();
            try
            {
                obfdetail objcbfdetails;
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    objcbfdetails = new obfdetail();
                    objcbfdetails.slno = i + 1;
                    objcbfdetails.obfheadgid = Convert.ToInt64(dt.Rows[i]["cbfdetails_cbfhead_gid"].ToString());
                    // objcbfdetails.cbfheadAmount = Convert.ToDecimal(rows["cbfheader_cbfamt"].ToString());
                    objcbfdetails.obfdetailgid = Convert.ToInt64(dt.Rows[i]["cbfdetails_gid"].ToString());
                    objcbfdetails.obfno = dt.Rows[i]["cbfheader_cbfno"].ToString();
                    objcbfdetails.serviceCode = dt.Rows[i]["prodservice_code"].ToString();
                    objcbfdetails.serviceName = dt.Rows[i]["prodservicename"].ToString();
                    objcbfdetails.serviceDesc = dt.Rows[i]["prodservice_description"].ToString();
                    //  objcbfdetails.uom = dt.Rows[i]["uom_code"].ToString();
                    objcbfdetails.obfheadAmount = Convert.ToDecimal(dt.Rows[i]["cbfdetails_totalamt"].ToString());
                    objcbfdetails.vendorName = dt.Rows[i]["supplierheader_name"].ToString();
                    objlist.Add(objcbfdetails);
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
                con.Close();
            }
        }
        public DataTable getObfTable(obfselection objcbf)
        {
            DataTable dt = new DataTable();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_obfselection", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectdetails");
                cmd.Parameters.AddWithValue("@obfheadgid", objcbf.obfheadgid);
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
        public DataTable getobfdetails(string obfdetgid)
        {
            DataTable objtable = new DataTable();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_obfselection", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectforwo");
                cmd.Parameters.AddWithValue("@obfdetgid", obfdetgid);
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                //if (!objtable.Columns.Contains("cbftotal_amount"))
                //    objtable.Columns.Add("cbftotal_amount");
                return objtable;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objtable;
            }
            finally
            {
                con.Close();
            }
        }
        public List<woraiser> GetWoList(DataTable dt)
        {
            List<woraiser> objlist = new List<woraiser>();
            try
            {
                getconnection();
                woraiser objraiser;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objraiser = new woraiser();
                    objraiser.serviceCode = dt.Rows[i]["prodservice_code"].ToString();
                    objraiser.serviceName = dt.Rows[i]["prodservicename"].ToString();
                    objraiser.serviceDesc = dt.Rows[i]["prodservice_description"].ToString();
                    objraiser.prodservicegid = Convert.ToInt32(dt.Rows[i]["prodservice_gid"].ToString());
                    // objraiser.uomgid = Convert.ToInt32(dt.Rows[i]["uom_gid"].ToString());
                    objraiser.vendorgid = Convert.ToInt32(dt.Rows[i]["supplierheader_gid"].ToString());
                    objraiser.requestforgid = Convert.ToInt32(dt.Rows[i]["requestfor_gid"].ToString());
                    objraiser.obfdetailsgid = Convert.ToInt32(dt.Rows[i]["cbfdetails_gid"].ToString());
                    //  objraiser.units = dt.Rows[i]["uom_code"].ToString();
                    objraiser.department = dt.Rows[i]["requestfor_name"].ToString();
                    objraiser.vendorName = dt.Rows[i]["supplierheader_name"].ToString();
                    objraiser.total = Convert.ToDecimal(dt.Rows[i]["cbfdetails_totalamt"].ToString());
                    objlist.Add(objraiser);
                }
                return objlist;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objlist;
            }
        }
        public List<woraiser> GetMonth()
        {
            List<woraiser> objMonth = new List<woraiser>();
            //objMonth.Add(new woraiser { monthId = "0", monthName = "-- Select --", });
            objMonth.Add(new woraiser { monthId = 1, monthName = "Jan", });
            objMonth.Add(new woraiser { monthId = 2, monthName = "Feb", });
            objMonth.Add(new woraiser { monthId = 3, monthName = "Mar", });
            objMonth.Add(new woraiser { monthId = 4, monthName = "Apr", });
            objMonth.Add(new woraiser { monthId = 5, monthName = "May", });
            objMonth.Add(new woraiser { monthId = 6, monthName = "Jun", });
            objMonth.Add(new woraiser { monthId = 7, monthName = "Jul", });
            objMonth.Add(new woraiser { monthId = 8, monthName = "Aug", });
            objMonth.Add(new woraiser { monthId = 9, monthName = "Sep", });
            objMonth.Add(new woraiser { monthId = 10, monthName = "Oct", });
            objMonth.Add(new woraiser { monthId = 11, monthName = "Nov", });
            objMonth.Add(new woraiser { monthId = 12, monthName = "Dec", });
            return objMonth;
        }
        public List<woraiser> GetMonth1()
        {
            List<woraiser> objMonth = new List<woraiser>();
            //objMonth.Add(new woraiser { monthId = "0", monthName = "-- Select --", });
            objMonth.Add(new woraiser { monthId1 = 1, monthName1 = "Jan", });
            objMonth.Add(new woraiser { monthId1 = 2, monthName1 = "Feb", });
            objMonth.Add(new woraiser { monthId1 = 3, monthName1 = "Mar", });
            objMonth.Add(new woraiser { monthId1 = 4, monthName1 = "Apr", });
            objMonth.Add(new woraiser { monthId1 = 5, monthName1 = "May", });
            objMonth.Add(new woraiser { monthId1 = 6, monthName1 = "Jun", });
            objMonth.Add(new woraiser { monthId1 = 7, monthName1 = "Jul", });
            objMonth.Add(new woraiser { monthId1 = 8, monthName1 = "Aug", });
            objMonth.Add(new woraiser { monthId1 = 9, monthName1 = "Sep", });
            objMonth.Add(new woraiser { monthId1 = 10, monthName1 = "Oct", });
            objMonth.Add(new woraiser { monthId1 = 11, monthName1 = "Nov", });
            objMonth.Add(new woraiser { monthId1 = 12, monthName1 = "Dec", });
            return objMonth;
        }
        public List<woraiser> GetFreqList()
        {
            List<woraiser> objworaiser = new List<woraiser>();
            try
            {
                getconnection();
                DataTable objtable = new DataTable();
                woraiser objDetails;
                cmd = new SqlCommand("pr_fb_trn_frequency", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectfreq");
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                foreach (DataRow row in objtable.Rows)
                {
                    objDetails = new woraiser();
                    objDetails.freqId = Convert.ToInt32(row["frequency_gid"]);
                    objDetails.freqName = row["frequency_name"].ToString();
                    objworaiser.Add(objDetails);
                }
                return objworaiser;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objworaiser;
            }
        }
        public int GetFreqMonth(int freqId)
        {
            try
            {
                getconnection();

                string[,] parameter = new string[,]
                {
                    {"@freq_gid",freqId.ToString()},
                    {"@actionName","freqName"},
                };

                return Convert.ToInt32(objcommon.ProcedureCommon(parameter, "pr_fb_trn_frequency"));
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
        public string GetMonthName(int monthId)
        {
            string monthName = string.Empty;
            try
            {
                switch (monthId)
                {
                    case 1:
                        monthName = "Jan";
                        break;
                    case 2:
                        monthName = "Feb";
                        break;
                    case 3:
                        monthName = "Mar";
                        break;
                    case 4:
                        monthName = "Apr";
                        break;
                    case 5:
                        monthName = "May";
                        break;
                    case 6:
                        monthName = "Jun";
                        break;
                    case 7:
                        monthName = "Jul";
                        break;
                    case 8:
                        monthName = "Aug";
                        break;
                    case 9:
                        monthName = "Sep";
                        break;
                    case 10:
                        monthName = "Oct";
                        break;
                    case 11:
                        monthName = "Nov";
                        break;
                    case 12:
                        monthName = "Dec";
                        break;
                }
                return monthName;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }

        }
        public int GetMonthId(string monthName)
        {
            int monthId = 0;
            try
            {
                switch (monthName)
                {
                    case "Jan":
                        monthId = 1;
                        break;
                    case "Feb":
                        monthId = 2;
                        break;
                    case "Mar":
                        monthId = 3;
                        break;
                    case "Apr":
                        monthId = 4;
                        break;
                    case "May":
                        monthId = 5;
                        break;
                    case "Jun":
                        monthId = 6;
                        break;
                    case "Jul":
                        monthId = 7;
                        break;
                    case "Aug":
                        monthId = 8;
                        break;
                    case "Sep":
                        monthId = 9;
                        break;
                    case "Oct":
                        monthId = 10;
                        break;
                    case "Nov":
                        monthId = 11;
                        break;
                    case "Dec":
                        monthId = 12;
                        break;
                }
                return monthId;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }

        }
        public List<poRaising> GetAmendSummary()
        {
            List<poRaising> objraiser = new List<poRaising>();
            try
            {
                getconnection();
                DataTable objtable = new DataTable();
                poRaising objDetails;
                cmd = new SqlCommand("pr_fb_trn_poheader", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectforamend");
                cmd.Parameters.AddWithValue("@logingid", objCmnFunctions.GetLoginUserGid().ToString());
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objDetails = new poRaising();
                    objDetails.slno = i + 1;
                    objDetails.poDetgid = Convert.ToInt32(objtable.Rows[i]["poheader_gid"]);
                    objDetails.poDate = Convert.ToString(objtable.Rows[i]["poheader_date"]);
                    objDetails.poRefNo = objtable.Rows[i]["poheader_pono"].ToString();
                    objDetails.vendorName = objtable.Rows[i]["supplierheader_name"].ToString();
                    //objDetails.status = objtable.Rows[i]["status_name"].ToString();
                    objDetails.poAmount = objtable.Rows[i]["poheader_over_total"].ToString();
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
                con.Close();
            }
        }
        public List<woraiser> GetWoSplitList(DataTable dt, int monthQ, int monthId, int monthId1, int freqId)
        {
            List<woraiser> objlist = new List<woraiser>();
            try
            {
                getconnection();

                woraiser objraiser;
                string finalMonthS = string.Empty;
                int remMonth1 = 0;
                int finalMonth = 0;
                int toMonth = 0;
                DataTable objTable = new DataTable();
                if (objTable.Columns.Count == 0)
                {
                    objTable.Columns.Add("prodservice_code");
                    objTable.Columns.Add("prodservicename");
                    objTable.Columns.Add("prodservice_description");
                    objTable.Columns.Add("prodservice_gid");
                    // objTable.Columns.Add("uom_gid");
                    objTable.Columns.Add("supplierheader_gid");
                    objTable.Columns.Add("cbfdetails_gid");
                    objTable.Columns.Add("serviceMonth");
                    // objTable.Columns.Add("uom_code");
                    objTable.Columns.Add("cbfdetails_totalamt");
                }
                for (int j = 0; j < dt.Rows.Count; j++)
                {

                    for (int i = 1; i <= monthQ; i++)
                    {
                        objraiser = new woraiser();
                        objraiser.serviceCode = dt.Rows[j]["prodservice_code"].ToString();
                        objraiser.serviceName = dt.Rows[j]["prodservicename"].ToString();
                        objraiser.serviceDesc = dt.Rows[j]["prodservice_description"].ToString();
                        objraiser.prodservicegid = Convert.ToInt32(dt.Rows[j]["prodservice_gid"].ToString());
                        //objraiser.uomgid = Convert.ToInt32(dt.Rows[j]["uom_gid"].ToString());
                        objraiser.vendorgid = Convert.ToInt32(dt.Rows[j]["supplierheader_gid"].ToString());
                        // objraiser.requestforgid = Convert.ToInt32(dt.Rows[i]["requestfor_gid"].ToString());
                        objraiser.obfdetailsgid = Convert.ToInt32(dt.Rows[j]["cbfdetails_gid"].ToString());

                        string fromMonth = GetMonthName(monthId).ToString();
                        monthId = (Convert.ToInt32(monthId) + Convert.ToInt32(freqId));

                        if (monthId > monthId1)
                        {
                            monthId = monthId - freqId;
                            remMonth1 = monthId1 - monthId;
                            toMonth = monthId + remMonth1;

                        }
                        else
                        {
                            toMonth = monthId - 1;


                        }
                        finalMonthS = GetMonthName(toMonth).ToString();
                        objraiser.serviceMonth = fromMonth + "-" + finalMonthS;

                        // objraiser.units = dt.Rows[j]["uom_code"].ToString();
                        objraiser.overTotal = Convert.ToDecimal(dt.Rows[j]["cbfdetails_totalamt"].ToString());
                        //objraiser.overTotal = 
                        objlist.Add(objraiser);
                        objTable.Rows.Add(objraiser.serviceCode, objraiser.serviceName, objraiser.serviceDesc, objraiser.prodservicegid
                            , objraiser.vendorgid, objraiser.obfdetailsgid, objraiser.serviceMonth,
                            objraiser.overTotal);
                    }
                }

                HttpContext.Current.Session["WoSplitTable"] = objTable;
                return objlist;
            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objlist;
            }
        }
        public List<woraiser> GetWoTempList(DataTable dt)
        {
            List<woraiser> objlist = new List<woraiser>();
            try
            {
                getconnection();

                woraiser objraiser;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objraiser = new woraiser();
                    objraiser.serviceCode = dt.Rows[i]["prodservice_code"].ToString();
                    objraiser.serviceName = dt.Rows[i]["prodservicename"].ToString();
                    objraiser.serviceDesc = dt.Rows[i]["prodservice_description"].ToString();
                    objraiser.prodservicegid = Convert.ToInt32(dt.Rows[i]["prodservice_gid"].ToString());
                    //objraiser.uomgid = Convert.ToInt32(dt.Rows[i]["uom_gid"].ToString());
                    objraiser.vendorgid = Convert.ToInt32(dt.Rows[i]["supplierheader_gid"].ToString());
                    // objraiser.requestforgid = Convert.ToInt32(dt.Rows[i]["requestfor_gid"].ToString());
                    objraiser.obfdetailsgid = Convert.ToInt32(dt.Rows[i]["cbfdetails_gid"].ToString());
                    objraiser.serviceMonth = dt.Rows[i]["serviceMonth"].ToString();
                    //objraiser.percentage = Convert.ToDecimal(dt.Rows[i]["percentage"].ToString());
                    // objraiser.units = dt.Rows[i]["uom_code"].ToString();
                    //objraiser.total = Convert.ToDecimal(dt.Rows[i]["cbfdetails_totalamt"].ToString());
                    objraiser.grandTotal = Convert.ToDecimal(dt.Rows[i]["cbfdetails_totalamt"]);
                    // objraiser.percentage = Convert.ToDecimal(dt.Rows[i]["percentage"].ToString());
                    // objraiser.total = Convert.ToDecimal(dt.Rows[i]["total"].ToString());
                    if (dt.Columns.Count > 8)
                    {
                        if (dt.Rows[i]["percentage"].ToString() == null || dt.Rows[i]["percentage"].ToString() == "")
                        {
                            objraiser.percentage = 0;
                        }
                        else
                        {
                            objraiser.percentage = (Convert.ToDecimal(dt.Rows[i]["percentage"].ToString()));
                        }
                        if (dt.Rows[i]["total"].ToString() == null || dt.Rows[i]["total"].ToString() == "")
                        {
                            objraiser.total = 0;
                        }
                        else
                        {
                            objraiser.total = (Convert.ToDecimal(dt.Rows[i]["total"].ToString()));
                        }
                    }
                    objlist.Add(objraiser);
                }
                return objlist;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objlist;
            }
        }
        public string InsertWo(woraiser objwoheader, DataTable objWoTable)
        {
            try
            {
                getconnection();
                // int shipmentgid = 0;
                if (objWoTable.Rows.Count == 0)
                {
                    return objwoheader.result = "There is no line item to save or submit";
                }
                int b = 0;
                cmd = new SqlCommand("pr_fb_trn_poinsert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@poheader_pono", objCmnFunctions.GetSequenceNoFb("WO", "", objwoheader.department));
                cmd.Parameters.AddWithValue("@poheader_date", objCmnFunctions.convertoDateTime(objwoheader.wodate));
                //   cmd.Parameters.AddWithValue("@poheader_enddate", objCmnFunctions.convertoDateTime(objwoheader.woenddate));
                cmd.Parameters.AddWithValue("@poheader_raiser_gid", objCmnFunctions.GetLoginUserGid().ToString());
                //  cmd.Parameters.AddWithValue("@poheader_projmanager", objwoheader.projmanagergid.ToString());
                cmd.Parameters.AddWithValue("@poheader_requestfor", Convert.ToInt32(objwoheader.requestforgid).ToString());
                if (objwoheader.itType == "Application")
                    cmd.Parameters.AddWithValue("@poheader_ittype", "A");
                if (objwoheader.itType == "Infrastructure")
                    cmd.Parameters.AddWithValue("@poheader_ittype", "I");
                cmd.Parameters.AddWithValue("@poheader_vendor_gid", Convert.ToInt32(objwoheader.vendorgid.ToString()));
                cmd.Parameters.AddWithValue("@poheader_vendor_note", objwoheader.vendorNote);
                cmd.Parameters.AddWithValue("@poheader_over_total", objwoheader.overTotal);
                cmd.Parameters.AddWithValue("@poheader_type", "W");
                if (objwoheader.actionName == "Submit")
                {
                    cmd.Parameters.AddWithValue("@poheader_status", "2");

                }
                else
                {
                    cmd.Parameters.AddWithValue("@poheader_status", "1");
                }
                cmd.Parameters.AddWithValue("@poheader_termcond_gid", objwoheader.templateGid);
                cmd.Parameters.AddWithValue("@poheader_add_termandcond", objwoheader.additionTemplate);
                cmd.Parameters.AddWithValue("@poheader_from_month", objwoheader.monthName);
                cmd.Parameters.AddWithValue("@poheader_to_month", objwoheader.monthName1);
                cmd.Parameters.AddWithValue("@poheader_frequency_gid", objwoheader.freqId);
                cmd.Parameters.AddWithValue("@actionName", "insertwoheader");
                cmd.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;

                int a = cmd.ExecuteNonQuery();
                int woheadGid = Convert.ToInt32(cmd.Parameters["@result"].Value);
                if (a == 1)
                {
                    if (objWoTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < objWoTable.Rows.Count; i++)
                        {
                            cmd = new SqlCommand("pr_fb_trn_poinsert", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@podetails_prodservice_gid", Convert.ToInt32(objWoTable.Rows[i]["prodservice_gid"].ToString()));
                            cmd.Parameters.AddWithValue("@podetails_desc", objWoTable.Rows[i]["prodservice_description"].ToString());
                            // cmd.Parameters.AddWithValue("@podetails_uom_gid", Convert.ToInt32(objWoTable.Rows[i]["uom_gid"].ToString()));
                            cmd.Parameters.AddWithValue("@podetails_total", Convert.ToDecimal(objWoTable.Rows[i]["total"].ToString()));
                            cmd.Parameters.AddWithValue("@podetails_cbfdet_gid", Convert.ToInt32(objWoTable.Rows[i]["cbfdetails_gid"].ToString()));
                            cmd.Parameters.AddWithValue("@podetails_percentage", Convert.ToDecimal(objWoTable.Rows[i]["percentage"].ToString()));
                            cmd.Parameters.AddWithValue("@podetails_serv_month ", objWoTable.Rows[i]["serviceMonth"].ToString());
                            cmd.Parameters.AddWithValue("@actionName", "insertwodetails");
                            b = cmd.ExecuteNonQuery();
                        }

                    }
                    if (objwoheader.actionName == "Submit")
                    {
                        cmd = new SqlCommand("pr_fb_trn_apprqueueWO", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@refgid", woheadGid);
                        cmd.Parameters.AddWithValue("@empgid", objCmnFunctions.GetLoginUserGid());
                        cmd.Parameters.AddWithValue("@requesttype", "WO");
                        objwoheader.result = Convert.ToString(cmd.ExecuteNonQuery());
                    }
                }
                if (objwoheader.result != "")
                {
                    objwoheader.result = "Inserted Successfully";
                }
                return objwoheader.result;
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
        public DataTable GetWoDetails(int woheadGid)
        {
            DataTable objtable = new DataTable();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_woheader", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectwodetails");
                cmd.Parameters.AddWithValue("@poheadgid", woheadGid);
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                //if (!objtable.Columns.Contains("cbfheader_amount"))
                //    objtable.Columns.Add("cbfheader_amount");
                //for (int i = 0; i < objtable.Rows.Count; i++)
                //{
                //    cmd = new SqlCommand("pr_fb_trn_poheader", con);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.AddWithValue("@actionName", "selectpoamount");
                //    cmd.Parameters.AddWithValue("@poheadgid", woheadGid);
                //    objtable.Rows[i]["cbfheader_amount"] = cmd.ExecuteScalar();
                //}
                return objtable;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objtable;
            }
            finally
            {
                con.Close();
            }
        }
        public List<woraiser> GetWoEditList(DataTable dt)
        {
            List<woraiser> objlist = new List<woraiser>();
            try
            {
                getconnection();
                woraiser objraiser;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objraiser = new woraiser();
                    objraiser.podetailsgid = Convert.ToInt32(dt.Rows[i]["podetails_gid"].ToString());
                    objraiser.worefno = dt.Rows[i]["poheader_pono"].ToString();
                    objraiser.wodate = dt.Rows[i]["poheader_date"].ToString();
                    // objraiser.woenddate = dt.Rows[i]["poheader_enddate"].ToString();
                    objraiser.serviceCode = dt.Rows[i]["prodservice_code"].ToString();
                    objraiser.serviceName = dt.Rows[i]["prodservice_name"].ToString();
                    objraiser.serviceDesc = dt.Rows[i]["podetails_desc"].ToString();
                    objraiser.prodservicegid = Convert.ToInt32(dt.Rows[i]["prodservice_gid"].ToString());
                    //objraiser.uomgid = Convert.ToInt32(dt.Rows[i]["uom_gid"].ToString());
                    objraiser.vendorgid = Convert.ToInt32(dt.Rows[i]["supplierheader_gid"].ToString());
                    //objraiser.requestforgid = Convert.ToInt32(dt.Rows[i]["requestfor_gid"].ToString());
                    //objraiser.obfdetailsgid = Convert.ToInt32(dt.Rows[i]["cbfdetails_gid"].ToString());
                    // objraiser.units = dt.Rows[i]["uom_code"].ToString();
                    objraiser.department = getrequestName(Convert.ToInt32(dt.Rows[i]["poheader_requestfor"].ToString()));
                    objraiser.vendorName = dt.Rows[i]["supplierheader_name"].ToString();
                    objraiser.total = Convert.ToDecimal(dt.Rows[i]["podetails_total"].ToString());
                    objraiser.overTotal = Convert.ToDecimal(dt.Rows[i]["cbfdetails_totalamt"].ToString());
                    objraiser.itType = dt.Rows[i]["poheader_ittype"].ToString();
                    objraiser.vendorNote = dt.Rows[i]["poheader_vendor_note"].ToString();
                    objraiser.freqId = Convert.ToInt32(dt.Rows[i]["frequency_gid"].ToString());
                    objraiser.monthName = dt.Rows[i]["poheader_from_month"].ToString();
                    objraiser.monthName1 = dt.Rows[i]["poheader_to_month"].ToString();
                    objraiser.percentage = Convert.ToDecimal(dt.Rows[i]["podetails_perce"].ToString());
                    objraiser.serviceMonth = dt.Rows[i]["podetails_serv_month"].ToString();
                    //objraiser.projmanagergid = Convert.ToInt32(dt.Rows[i]["poheader_projectmanager"].ToString());
                    objraiser.templateGid = Convert.ToInt32(dt.Rows[i]["poheader_termcond_gid"].ToString());
                    objraiser.tempName = GetTemplateContent(objraiser.templateGid);
                    objraiser.templname = GetTemplateName(objraiser.templateGid);
                    objraiser.additionTemplate = dt.Rows[i]["poheader_add_termandcond"].ToString();
                    objlist.Add(objraiser);
                }
                return objlist;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objlist;
            }
        }
        public List<woraiser> GetWoEditTempList(DataTable dt)
        {
            List<woraiser> objlist = new List<woraiser>();
            try
            {
                getconnection();
                woraiser objraiser;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objraiser = new woraiser();
                    objraiser.podetailsgid = Convert.ToInt32(dt.Rows[i]["podetails_gid"].ToString());
                    objraiser.serviceCode = dt.Rows[i]["prodservice_code"].ToString();
                    objraiser.serviceName = dt.Rows[i]["prodservice_name"].ToString();
                    objraiser.serviceDesc = dt.Rows[i]["podetails_desc"].ToString();
                    objraiser.prodservicegid = Convert.ToInt32(dt.Rows[i]["prodservice_gid"].ToString());
                    //objraiser.uomgid = Convert.ToInt32(dt.Rows[i]["uom_gid"].ToString());
                    //objraiser.vendorgid = Convert.ToInt32(dt.Rows[i]["supplierheader_gid"].ToString());
                    // objraiser.requestforgid = Convert.ToInt32(dt.Rows[i]["requestfor_gid"].ToString());
                    objraiser.podetailsgid = Convert.ToInt32(dt.Rows[i]["podetails_gid"].ToString());
                    objraiser.serviceMonth = dt.Rows[i]["podetails_serv_month"].ToString();
                    //objraiser.percentage = Convert.ToDecimal(dt.Rows[i]["percentage"].ToString());
                    //objraiser.units = dt.Rows[i]["uom_code"].ToString();
                    //objraiser.total = Convert.ToDecimal(dt.Rows[i]["cbfdetails_totalamt"].ToString());
                    objraiser.grandTotal = Convert.ToDecimal(dt.Rows[i]["cbfdetails_totalamt"]);
                    objraiser.percentage = Convert.ToDecimal(dt.Rows[i]["podetails_perce"]);
                    objraiser.total = Convert.ToDecimal(dt.Rows[i]["podetails_total"]);
                    objlist.Add(objraiser);
                }
                return objlist;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objlist;
            }
        }
        public string UpdateWo(woraiser objwoheader, DataTable objWoTable)
        {
            try
            {
                getconnection();

                int b = 0;
                cmd = new SqlCommand("pr_fb_trn_poinsert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //  cmd.Parameters.AddWithValue("@poheader_pono", objCmnFunctions.GetSequenceNoFb("WO", "", objwoheader.department));
                //  cmd.Parameters.AddWithValue("@poheader_date", objCmnFunctions.convertoDateTime(objwoheader.wodate));
                // cmd.Parameters.AddWithValue("@poheader_enddate", objCmnFunctions.convertoDateTime(objwoheader.woenddate));
                cmd.Parameters.AddWithValue("@poheader_raiser_gid", objCmnFunctions.GetLoginUserGid().ToString());
                // cmd.Parameters.AddWithValue("@poheader_projmanager", objwoheader.projmanagergid.ToString());
                // cmd.Parameters.AddWithValue("@poheader_requestfor", Convert.ToInt32(objwoheader.requestforgid).ToString());
                if (objwoheader.itType == "Application")
                    cmd.Parameters.AddWithValue("@poheader_ittype", "A");
                if (objwoheader.itType == "Infrastructure")
                    cmd.Parameters.AddWithValue("@poheader_ittype", "I");
                //cmd.Parameters.AddWithValue("@poheader_vendor_gid", Convert.ToInt32(objwoheader.vendorgid.ToString()));
                cmd.Parameters.AddWithValue("@poheader_vendor_note", objwoheader.vendorNote);
                cmd.Parameters.AddWithValue("@poheader_over_total", objwoheader.overTotal);
                cmd.Parameters.AddWithValue("@poheader_gid", objwoheader.woheadGid);
                // cmd.Parameters.AddWithValue("@poheader_type", "W");
                if (objwoheader.actionName == "amend")
                {
                    cmd.Parameters.AddWithValue("@poheader_amendmentdate", objCmnFunctions.convertoDateTimeString(DateTime.Now.ToString()));
                    cmd.Parameters.AddWithValue("@poheader_status", "2");
                }
                else
                {
                    //cmd.Parameters.AddWithValue("@poheader_type", "P");
                    if (objwoheader.actionName == "Submit")
                    {
                        cmd.Parameters.AddWithValue("@poheader_status", "2");
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@poheader_status", "1");
                    }
                }
                cmd.Parameters.AddWithValue("@poheader_termcond_gid", objwoheader.templateGid);
                cmd.Parameters.AddWithValue("@poheader_add_termandcond", objwoheader.additionTemplate);
                //  cmd.Parameters.AddWithValue("@poheader_from_month", objwoheader.monthName);
                //  cmd.Parameters.AddWithValue("@poheader_to_month", objwoheader.monthName1);
                // cmd.Parameters.AddWithValue("@poheader_frequency_gid", objwoheader.freqId);
                cmd.Parameters.AddWithValue("@actionName", "updatewoheader");
                //   cmd.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;

                int a = cmd.ExecuteNonQuery();
                // int poheadGid = Convert.ToInt32(cmd.Parameters["@result"].Value);
                if (a == 2 || a == 1)
                {
                    if (objWoTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < objWoTable.Rows.Count; i++)
                        {
                            cmd = new SqlCommand("pr_fb_trn_poinsert", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            // cmd.Parameters.AddWithValue("@podetails_prodservice_gid", Convert.ToInt32(objWoTable.Rows[i]["prodservice_gid"].ToString()));
                            cmd.Parameters.AddWithValue("@podetails_desc", objWoTable.Rows[i]["podetails_desc"].ToString());
                            // cmd.Parameters.AddWithValue("@podetails_uom_gid", Convert.ToInt32(objWoTable.Rows[i]["uom_gid"].ToString()));
                            if (objWoTable.Columns.Contains("total"))
                            {
                                if (objWoTable.Rows[i]["total"].ToString() != "" || objWoTable.Rows[i]["total"].ToString() != null)
                                {
                                    cmd.Parameters.AddWithValue("@podetails_total", Convert.ToDecimal(objWoTable.Rows[i]["total"].ToString()));
                                }

                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@podetails_total", Convert.ToDecimal(objWoTable.Rows[i]["podetails_total"].ToString()));
                            }
                            if (objWoTable.Columns.Contains("percentage"))
                            {
                                if (objWoTable.Rows[i]["percentage"].ToString() != "" || objWoTable.Rows[i]["percentage"].ToString() != null)
                                {
                                    cmd.Parameters.AddWithValue("@podetails_total", Convert.ToDecimal(objWoTable.Rows[i]["percentage"].ToString()));
                                }

                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@podetails_percentage", Convert.ToDecimal(objWoTable.Rows[i]["podetails_perce"].ToString()));
                            }
                            cmd.Parameters.AddWithValue("@podetails_gid", Convert.ToInt32(objWoTable.Rows[i]["podetails_gid"].ToString()));
                            //cmd.Parameters.AddWithValue("@podetails_serv_month ", objWoTable.Rows[i]["podetails_serv_month"].ToString());
                            cmd.Parameters.AddWithValue("@actionName", "updatewodetails");
                            b = cmd.ExecuteNonQuery();
                        }

                    }

                    if (objwoheader.actionName == "Submit" || objwoheader.actionName == "amend")
                    {
                        cmd = new SqlCommand("pr_fb_trn_apprqueueWO", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@refgid", objwoheader.woheadGid);
                        cmd.Parameters.AddWithValue("@empgid", objCmnFunctions.GetLoginUserGid());
                        cmd.Parameters.AddWithValue("@requesttype", "WO");
                        objwoheader.result = Convert.ToString(cmd.ExecuteNonQuery());
                    }
                }
                if (objwoheader.result != "" && objwoheader.result != null)
                {
                    objwoheader.result = "Updated Successfully";
                }
                return objwoheader.result;
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

        //Wo Checker Summary
        public List<WoSummary> GetWoCheckerSummary()
        {
            DataTable objtable = new DataTable();
            List<WoSummary> objraiser = new List<WoSummary>();
            WoSummary objDetails;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_woheader", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@employgid", objCmnFunctions.GetLoginUserGid().ToString());
                cmd.Parameters.AddWithValue("@actionName", "selectforwochecker");
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    if (objCmnFunctions.GetLoginUserGid().ToString() != objtable.Rows[i]["poheader_raisor_gid"])
                    {
                        objDetails = new WoSummary();
                        objDetails.slno = i + 1;
                        objDetails.woDetgid = Convert.ToInt32(objtable.Rows[i]["poheader_gid"].ToString());
                        //objDetails.poCancelGid = Convert.ToInt32(objtable.Rows[i]["pocancellation_gid"].ToString());
                        objDetails.woDate = objtable.Rows[i]["poheader_date"].ToString();
                        //objDetails.remarks = objtable.Rows[i]["pocancellation_mak_remarks"].ToString();
                        objDetails.woRefNo = objtable.Rows[i]["poheader_pono"].ToString();
                        objDetails.vendorName = objtable.Rows[i]["supplierheader_name"].ToString();
                        objDetails.status = objtable.Rows[i]["status_name"].ToString();
                        objDetails.woAmount = objtable.Rows[i]["poheader_over_total"].ToString();
                        objraiser.Add(objDetails);
                    }

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
                con.Close();
            }
        }

        public string DeleteWoSummaryDetails(woraiser obj)
        {
            try
            {

                string[,] wherecond = new string[,]
	        {
                {"poheader_gid", obj.woheadGid.ToString ()},
                {"poheader_type","W"}
            };
                string[,] column = new string[,]
	       {
                 {"poheader_isremoved", "Y"}
	            
           };
                string tblname = "fb_trn_tpoheader";
                obj.result = objcommon.DeleteCommon(column, wherecond, tblname);
                if (obj.result == "1")
                {
                    string[,] wherecond1 = new string[,]
	        {
                {"podetails_pohead_gid", obj.woheadGid.ToString ()}
            };
                    string[,] column1 = new string[,]
	       {
                 {"podetails_isremoved", "Y"}
	            
           };
                    string tblname1 = "fb_trn_tpodetails";
                    obj.result = objcommon.DeleteCommon(column1, wherecond1, tblname1);
                }

                return obj.result;
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
        public string CancelWoSummaryDetails(woraiser obj)
        {
            try
            {
                getconnection();
                string[,] wherecond = new string[,]
	        {
                {"poheader_gid", obj.woheadGid.ToString ()},
                {"poheader_type","W"}
            };
                string[,] column = new string[,]
	       {
                 {"poheader_iscancelled", "M"}
	            
           };
                string tblname = "fb_trn_tpoheader";
                obj.result = objcommon.DeleteCommon(column, wherecond, tblname);
                if (obj.result == "Sucess")
                {
                    string[,] columnval = new string[,]
	       {
                 {"pocancellation_pohead_gid",obj.woheadGid.ToString()},
                 {"pocancellation_mak_gid",objCmnFunctions.GetLoginUserGid().ToString()},
                 {"pocancellation_mak_remarks",(string.IsNullOrEmpty(obj.remarks.ToString()) ? string.Empty :obj.remarks.ToString())},
                 {"pocancellation_mak_date",DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss")}
           };
                    string tblname1 = "fb_trn_tpocancellation";
                    obj.result = objcommon.InsertCommon(columnval, tblname1);

                    if (obj.result == "success")
                    {
                        int queue_type = Convert.ToInt32(ConfigurationManager.AppSettings["wocancelrolegid"].ToString());
                        int queue_ref_flag = Convert.ToInt32(ConfigurationManager.AppSettings["worefFlag"].ToString());
                        string[,] codes = new string[,]
                                {
                                {"queue_date", "sysdatetime()"},
                                {"queue_ref_flag",queue_ref_flag.ToString()},
                                {"queue_ref_gid",obj.woheadGid.ToString()},
                                {"queue_prev_gid","0"},
                                {"queue_from",objCmnFunctions.GetLoginUserGid().ToString()},
                                {"queue_action_for","A"},
                                {"queue_to_type", "G"},
                                {"queue_to", queue_type.ToString()},
                                };
                        string tname = "iem_trn_tqueue";
                        string queue_result = objcommon.InsertCommon(codes, tname);
                    }
                }
                return obj.result;
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

        public List<WoSummary> GetCancelWoSummaryDetails()
        {
            List<WoSummary> objraiser = new List<WoSummary>();
            try
            {
                getconnection();
                DataTable objtable = new DataTable();

                WoSummary objDetails;
                cmd = new SqlCommand("pr_fb_trn_pocancellation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "WoCancellationSummary");
                cmd.Parameters.AddWithValue("@logingid", objCmnFunctions.GetLoginUserGid());
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objDetails = new WoSummary();
                    objDetails.slno = i + 1;
                    objDetails.woDetgid = Convert.ToInt32(objtable.Rows[i]["poheader_gid"].ToString());
                    objDetails.woCancelGid = Convert.ToInt32(objtable.Rows[i]["pocancellation_gid"].ToString());
                    objDetails.woDate = objtable.Rows[i]["poheader_date"].ToString();
                    objDetails.remarks = objtable.Rows[i]["pocancellation_mak_remarks"].ToString();
                    objDetails.woRefNo = objtable.Rows[i]["poheader_pono"].ToString();
                    objDetails.vendorName = objtable.Rows[i]["supplierheader_name"].ToString();
                    objDetails.status = objtable.Rows[i]["status_name"].ToString();
                    objDetails.woAmount = objtable.Rows[i]["poheader_over_total"].ToString();
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
                con.Close();
            }
        }

        //Wo Closure Maker Summary

        public List<WoSummary> GetWoClosureSummary()
        {
            List<WoSummary> objraiser = new List<WoSummary>();
            try
            {
                getconnection();
                DataTable objtable = new DataTable();
                WoSummary objDetails;
                cmd = new SqlCommand("pr_fb_trn_woclosure", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "select");
                cmd.Parameters.AddWithValue("@logingid", objCmnFunctions.GetLoginUserGid().ToString());
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objDetails = new WoSummary();
                    objDetails.slno = i + 1;
                    objDetails.woDetgid = Convert.ToInt32(objtable.Rows[i]["poheader_gid"].ToString());
                    //objDetails.poCancelGid = Convert.ToInt32(objtable.Rows[i]["pocancellation_gid"].ToString());
                    objDetails.woDate = objtable.Rows[i]["poheader_date"].ToString();
                    //objDetails.remarks = objtable.Rows[i]["pocancellation_mak_remarks"].ToString();
                    objDetails.woRefNo = objtable.Rows[i]["poheader_pono"].ToString();
                    objDetails.vendorName = objtable.Rows[i]["supplierheader_name"].ToString();
                    objDetails.status = objtable.Rows[i]["status_name"].ToString();
                    objDetails.woAmount = objtable.Rows[i]["poheader_over_total"].ToString();
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
                con.Close();
            }
        }

        public string GetWoClosureExcelValid(string pono, string podate, string action)
        {
            string result = "";
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_woClosureExcelValidate", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@chkdata", SqlDbType.VarChar).Value = pono.ToString();
                cmd.Parameters.Add("@chkdate", SqlDbType.VarChar).Value = podate.ToString();
                cmd.Parameters.Add("@result", SqlDbType.VarChar).Value = action;
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
        public string BulkWoInsert(DataTable dt, int validRecords, string fileName)
        {
            try
            {
                getconnection();
                string result = string.Empty;
                if (dt.Rows.Count > 0)
                {
                    if (!dt.Columns.Contains("woheadgid"))
                        dt.Columns.Add("woheadgid");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cmd = new SqlCommand("pr_fb_trn_woclosure", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@pono", dt.Rows[i]["WONo"].ToString());
                        cmd.Parameters.AddWithValue("@poheaddate", objCmnFunctions.convertoDateTimeString(dt.Rows[i]["WoDate"].ToString()));
                        cmd.Parameters.AddWithValue("@actionName", "selectpoheadgid");
                        dt.Rows[i]["woheadgid"] = cmd.ExecuteScalar();

                        string[,] columnval = new string[,]
	               {
                        {"poclosure_pohead_gid",dt.Rows[i]["woheadgid"].ToString()},
                         {"poclosure_mak_gid",objCmnFunctions.GetLoginUserGid().ToString()},
                        {"poclosure_mak_remarks",dt.Rows[i]["Remarks"].ToString()},
                        {"poclosure_mak_date",DateTime.Now.ToString("dd/MMM/yyyy HH:mm:ss")}
                  };

                        string tblname1 = "fb_trn_tpoclosure";
                        result = objcommon.InsertCommon(columnval, tblname1);
                        if (result == "success")
                        {

                            string[,] columnval1 = new string[,]
	                    {
                             {"poheader_isclosed","M"},
                        };
                            string[,] wherecolumn = new string[,]
                        {
                      {"poheader_gid",dt.Rows[i]["woheadgid"].ToString()}

                        };
                            string tblname = "fb_trn_tpoheader";
                            result = objcommon.UpdateCommon(columnval1, wherecolumn, tblname);
                            HttpContext.Current.Session["maindatatable"] = null;
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
                con.Close();
            }
        }
        public string WoCheckerApprove(WoSummary obj)
        {
            try
            {
                string result = string.Empty;
                int b = 0;
                int queue_prevgid = 0;
                getconnection();
                if (obj.viewCancel == "rejected")
                {
                    //cmd = new SqlCommand("pr_fb_trn_apprqueueWO", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.AddWithValue("@refgid", obj.woheadGid);
                    //cmd.Parameters.AddWithValue("@empgid", objCmnFunctions.GetLoginUserGid().ToString());
                    //if (obj.viewCancel == "rejected")
                    //{
                    //    cmd.Parameters.AddWithValue("@actionstatus", "0");
                    //}
                    //cmd.Parameters.AddWithValue("@actionremark ", Convert.ToString(string.IsNullOrEmpty(obj.remarks) ? string.Empty : obj.remarks));
                    //cmd.Parameters.AddWithValue("@requesttype", "PO");
                    //obj.result = cmd.ExecuteNonQuery();
                    int worefFlag = Convert.ToInt32(ConfigurationManager.AppSettings["worefFlag"].ToString());
                    string[,] codw = new string[,]
            {
                {"queue_action_for","R"},
                {"queue_action_status","2"},
                {"queue_isremoved","Y"},
                {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                {"queue_action_date","sysdatetime()"},
                {"queue_action_remark",obj.remarks}
            };
                    string[,] codewhe = new string[,]
            {
                {"queue_ref_gid",obj.woheadGid.ToString()},
                {"queue_ref_flag",worefFlag.ToString()},
                {"queue_isremoved","N"}
            };

                    string tblname = "iem_trn_tqueue";
                    string updatecon = objcommon.UpdateCommon(codw, codewhe, tblname);
                }
                if (obj.result != null && obj.viewCancel == "rejected")
                {
                    string[,] wherecond = new string[,]
	              {
                {"poheader_gid", obj.woheadGid.ToString ()},
                {"poheader_type","W"}
                  };
                    string[,] column = new string[,]
	             {
                      {"poheader_status", "6"},
                      {"poheader_currentapprovalstage","0"}
                 };
                    string tblname = "fb_trn_tpoheader";
                    result = objcommon.UpdateCommon(column, wherecond, tblname);
                }

                else
                {
                    string[,] wherecond = new string[,]
	              {
                {"poheader_gid", obj.woheadGid.ToString ()},
                {"poheader_type","W"}
                  };
                    string[,] column = new string[,]
	             {
                      {"poheader_status", "4"}
                 };
                    string tblname = "fb_trn_tpoheader";
                    result = objcommon.UpdateCommon(column, wherecond, tblname);
                    if (result == "Success")
                    {
                        //cmd = new SqlCommand("pr_fb_trn_POApprovalQueue", con);
                        //cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("@refgid", obj.woheadGid);
                        //cmd.Parameters.AddWithValue("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                        //cmd.Parameters.AddWithValue("@actionremark", (string.IsNullOrEmpty(obj.remarks) ? string.Empty : obj.remarks.ToString()));
                        //cmd.Parameters.AddWithValue("@request_type", "WO");
                        //cmd.Parameters.AddWithValue("@actionName", "updatepoqueue");
                        //b = cmd.ExecuteNonQuery();
                        int worefFlag = Convert.ToInt32(ConfigurationManager.AppSettings["worefFlag"].ToString());
                        string[,] wherecol = new string[,]
	              {
                              {"queue_ref_gid", obj.woheadGid.ToString ()},
                              {"queue_ref_flag",worefFlag.ToString()},
                              {"queue_isremoved","N"}
                  };
                        string[,] columnNames = new string[,]
	             {
                      {"queue_action_date", "sysdatetime()"},
                      {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                      {"queue_action_status","1"},
                      {"queue_action_remark",Convert.ToString(string.IsNullOrEmpty(obj.remarks) ? string.Empty : obj.remarks)},
                      {"queue_prev_gid",queue_prevgid.ToString()},
                      {"queue_isremoved","Y"}
                 };
                        string tbl = "iem_trn_tqueue";
                        result = objcommon.UpdateCommon(columnNames, wherecol, tbl);
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
                con.Close();
            }
        }
        public string ClosureWoSummaryDetails(woraiser obj)
        {
            try
            {
                getconnection();

                string[,] wherecond = new string[,]
	        {
                {"poheader_gid", obj.woheadGid.ToString ()}
            };
                string[,] column = new string[,]
	       {
                 {"poheader_isclosed", "M"}
	            
           };
                string tblname = "fb_trn_tpoheader";
                obj.result = objcommon.UpdateCommon(column, wherecond, tblname);
                if (obj.result == "Success")
                {
                    string[,] columnval = new string[,]
	       {
                 {"poclosure_pohead_gid",obj.woheadGid.ToString()},
                 {"poclosure_mak_gid",objCmnFunctions.GetLoginUserGid().ToString()},
                 {"poclosure_mak_remarks",(string.IsNullOrEmpty(obj.remarks.ToString()) ? "" :obj.remarks.ToString())},
                 {"poclosure_mak_date","sysdatetime()"}
           };
                    string tblname1 = "fb_trn_tpoclosure";
                    obj.result = objcommon.InsertCommon(columnval, tblname1);
                    if (obj.result == "success")
                    {
                        int queue_type = Convert.ToInt32(ConfigurationManager.AppSettings["woclosurerolegroup"].ToString());
                        int queue_ref_flag = Convert.ToInt32(ConfigurationManager.AppSettings["worefFlag"].ToString());
                        string[,] codes = new string[,]
                                {
                                {"queue_date", "sysdatetime()"},
                                {"queue_ref_flag",queue_ref_flag.ToString()},
                                {"queue_ref_gid",obj.woheadGid.ToString()},
                                {"queue_prev_gid","0"},
                                {"queue_action_for","A"},
                                {"queue_from",objCmnFunctions.GetLoginUserGid().ToString()},
                                {"queue_to_type", "G"},
                                {"queue_to", queue_type.ToString()}
                                };
                        string tname = "iem_trn_tqueue";
                        string queue_result = objcommon.InsertCommon(codes, tname);
                    }
                }
                return obj.result;

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
        //Get Wo Closure Checker Summary
        public List<WoSummary> GetWoClosureCheckerSummary()
        {
            List<WoSummary> objraiser = new List<WoSummary>();
            try
            {
                getconnection();
                DataTable objtable = new DataTable();
                WoSummary objDetails;
                cmd = new SqlCommand("pr_fb_trn_woclosure", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectforchecker");
                cmd.Parameters.AddWithValue("@logingid", objCmnFunctions.GetLoginUserGid());
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objDetails = new WoSummary();
                    objDetails.slno = i + 1;
                    objDetails.woDetgid = Convert.ToInt32(objtable.Rows[i]["poheader_gid"].ToString());
                    //objDetails.poCancelGid = Convert.ToInt32(objtable.Rows[i]["pocancellation_gid"].ToString());
                    objDetails.woDate = objtable.Rows[i]["poheader_date"].ToString();
                    //objDetails.remarks = objtable.Rows[i]["pocancellation_mak_remarks"].ToString();
                    objDetails.woRefNo = objtable.Rows[i]["poheader_pono"].ToString();
                    objDetails.remarks = objtable.Rows[i]["poclosure_mak_remarks"].ToString();
                    objDetails.vendorName = objtable.Rows[i]["supplierheader_name"].ToString();
                    objDetails.status = objtable.Rows[i]["status_name"].ToString();
                    objDetails.woAmount = objtable.Rows[i]["poheader_over_total"].ToString();
                    objDetails.woclosureGid = Convert.ToInt32(objtable.Rows[i]["poclosure_gid"].ToString());
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
                con.Close();
            }
        }

        //WO Checker Closure Insertion 
        public string ClosureWoSummaryInsert(woraiser obj)
        {
            try
            {
                getconnection();
                string[,] wherecond = new string[,]
	        {
                {"poheader_gid", obj.woheadGid.ToString ()},
                {"poheader_type","W"}
            };
                if (obj.viewfor == "reject")
                {
                    string[,] column = new string[,]
	       {
                 {"poheader_isclosed", "N"},
                    {"poheader_status","6"}
           };
                    string tblname = "fb_trn_tpoheader";
                    obj.result = objcommon.UpdateCommon(column, wherecond, tblname);
                }
                else
                {
                    string[,] column = new string[,]
	       {
                 {"poheader_isclosed", "C"}
                
           };
                    string tblname = "fb_trn_tpoheader";
                    obj.result = objcommon.UpdateCommon(column, wherecond, tblname);
                }
                if (obj.result == "Success" && obj.viewfor == "reject")
                {
                    string[,] columnval = new string[,]
	       {
                 {"poclosure_pohead_gid",obj.woheadGid.ToString()},
                 {"poclosure_chk_gid",objCmnFunctions.GetLoginUserGid().ToString()},
                 {"poclosure_chk_remarks",(string.IsNullOrEmpty(obj.remarks.ToString()) ? string.Empty :obj.remarks.ToString())},
                 {"poclosure_chk_status","R"},
                 {"poclosure_chk_date","sysdatetime()"}
           };
                    string[,] wherecolumn = new string[,]
                    {
                      {"poclosure_gid",obj.woClosureGid.ToString()},
                      {"poclosure_isremoved","N"}

                    };
                    string tblname1 = "fb_trn_tpoclosure";
                    obj.result = objcommon.UpdateCommon(columnval, wherecolumn, tblname1);
                    if (obj.result == "Success")
                    {
                        int worefFlag = Convert.ToInt32(ConfigurationManager.AppSettings["worefFlag"].ToString());
                        string[,] wherecond1 = new string[,]
	              {
                         {"queue_ref_gid",obj.woheadGid.ToString()},
                         {"queue_ref_flag",worefFlag.ToString()},
                         {"queue_isremoved","N"}
                        
                  };
                        string[,] column = new string[,]
	             {
                         {"queue_isremoved","Y"},
                         {"queue_action_date","sysdatetime()"},
                         {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                         {"queue_action_status","2"},
                         {"queue_action_for","R"},
                         {"queue_action_remark",(string.IsNullOrEmpty(obj.remarks) ? string.Empty :obj.remarks.ToString())}
                 };
                        string tblname = "iem_trn_tqueue";
                        string result = objcommon.UpdateCommon(column, wherecond, tblname);
                    }
                }

                else if (obj.result == "Success")
                {
                    string[,] columnval = new string[,]
	       {
                 {"poclosure_pohead_gid",obj.woheadGid.ToString()},
                 {"poclosure_chk_gid",objCmnFunctions.GetLoginUserGid().ToString()},
                 {"poclosure_chk_remarks",(string.IsNullOrEmpty(obj.remarks) ? string.Empty :obj.remarks.ToString())},
                 {"poclosure_chk_status","A"},
                 {"poclosure_chk_date","sysdatetime()"}
           };
                    string[,] wherecolumn = new string[,]
                    {
                      {"poclosure_gid",obj.woClosureGid.ToString()},
                        {"poclosure_isremoved","N"}

                    };
                    string tblname1 = "fb_trn_tpoclosure";
                    obj.result = objcommon.UpdateCommon(columnval, wherecolumn, tblname1);
                    if (obj.result == "Success")
                    {
                        int worefFlag = Convert.ToInt32(ConfigurationManager.AppSettings["worefFlag"].ToString());
                        string[,] wherecond1 = new string[,]
	              {
                         {"queue_ref_gid",obj.woheadGid.ToString()},
                         {"queue_ref_flag",worefFlag.ToString()},
                         {"queue_isremoved","N"}
                        
                  };
                        string[,] column = new string[,]
	             {
                         {"queue_isremoved","Y"},
                         {"queue_action_date","sysdatetime()"},
                         {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                         {"queue_action_status","1"},
                         {"queue_action_remark",(string.IsNullOrEmpty(obj.remarks) ? string.Empty :obj.remarks.ToString())}
                 };
                        string tblname = "iem_trn_tqueue";
                        string result = objcommon.UpdateCommon(column, wherecond1, tblname);
                    }
                }
                return obj.result;
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
        public int WoCancelApprove(WoSummary obj)
        {
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_pocancellation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pocancellation_chk_gid", objCmnFunctions.GetLoginUserGid().ToString());
                if (obj.viewCancel == "rejected")
                {
                    cmd.Parameters.AddWithValue("@pocancellation_chk_status", "R");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@pocancellation_chk_status", "A");
                }
                //cmd.Parameters.AddWithValue("@actionName", "PoCancellationchecker");
                cmd.Parameters.AddWithValue("@pocancellation_chk_remarks", (string.IsNullOrEmpty(obj.remarks) ? string.Empty : obj.remarks.ToString()));
                //   cmd.Parameters.AddWithValue("@pocancellation_chk_remarks", obj.remarks);
                cmd.Parameters.AddWithValue("@pocancellation_gid", obj.woCancelGid);
                cmd.Parameters.AddWithValue("@pocancellation_pohead_gid", obj.woheadGid);
                cmd.Parameters.AddWithValue("@actionName", "WoCancellationchecker");
                obj.result = cmd.ExecuteNonQuery();
                //return obj.result;

                if (obj.result > 0 && obj.viewCancel == "")
                {
                    int worefFlag = Convert.ToInt32(ConfigurationManager.AppSettings["worefFlag"].ToString());
                    string[,] wherecond = new string[,]
	              {
                         {"queue_ref_gid",obj.woheadGid.ToString()},
                         {"queue_ref_flag",worefFlag.ToString()},
                         {"queue_isremoved", "N"}
                      };
                    string[,] column = new string[,]
	             {
                    {"queue_isremoved", "Y"},
                    {"queue_action_date","sysdatetime()"},
                    {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                    {"queue_action_for","A"},
                    {"queue_action_status","1"},
                    {"queue_action_remark",(string.IsNullOrEmpty(obj.remarks) ? string.Empty : obj.remarks.ToString())},
                 };
                    string tblname = "iem_trn_tqueue";
                    string result = objcommon.UpdateCommon(column, wherecond, tblname);

                }
                if (obj.result > 0 && obj.viewCancel == "rejected")
                {
                    int worefFlag = Convert.ToInt32(ConfigurationManager.AppSettings["worefFlag"].ToString());
                    string[,] wherecond = new string[,]
	              {
                         {"queue_ref_gid",obj.woheadGid.ToString()},
                         {"queue_ref_flag",worefFlag.ToString()},
                         {"queue_isremoved", "N"}
                      };
                    string[,] column = new string[,]
	             {
                    {"queue_isremoved", "Y"},
                    {"queue_action_date","sysdatetime()"},
                    {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                    {"queue_action_status","2"},
                    {"queue_action_for","R"},
                    {"queue_action_remark",(string.IsNullOrEmpty(obj.remarks) ? string.Empty : obj.remarks.ToString())},
                 };
                    string tblname = "iem_trn_tqueue";
                    string result = objcommon.UpdateCommon(column, wherecond, tblname);
                }
                return obj.result;
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
        public List<WoSummary> GetWoAmendSummary()
        {
            List<WoSummary> objraiser = new List<WoSummary>();
            try
            {
                getconnection();
                DataTable objtable = new DataTable();
                WoSummary objDetails;
                cmd = new SqlCommand("pr_fb_trn_woheader", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectforamend");
                cmd.Parameters.AddWithValue("@logingid", objCmnFunctions.GetLoginUserGid().ToString());
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objDetails = new WoSummary();
                    objDetails.slno = i + 1;
                    objDetails.woDetgid = Convert.ToInt32(objtable.Rows[i]["poheader_gid"]);
                    objDetails.woDate = Convert.ToString(objtable.Rows[i]["poheader_date"]);
                    objDetails.woRefNo = objtable.Rows[i]["poheader_pono"].ToString();
                    objDetails.vendorName = objtable.Rows[i]["supplierheader_name"].ToString();
                    //objDetails.status = objtable.Rows[i]["status_name"].ToString();
                    objDetails.woAmount = objtable.Rows[i]["poheader_over_total"].ToString();
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
                con.Close();
            }
        }

        public List<poRaising> getgrnReleaseForPO(string group)
        {
            List<poRaising> objraiser = new List<poRaising>();
            try
            {
                getconnection();
                DataTable objtable = new DataTable();
                poRaising objDetails;
                cmd = new SqlCommand("pr_fb_trn_poheader", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "GRNReleaseforPO");
                cmd.Parameters.AddWithValue("@logingid", objCmnFunctions.GetLoginUserGid().ToString());
                cmd.Parameters.AddWithValue("@requestfor", group.ToString());
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objDetails = new poRaising();
                    objDetails.slno = i + 1;
                    objDetails.poDetgid = Convert.ToInt32(objtable.Rows[i]["poheader_gid"]);
                    objDetails.poDate = Convert.ToString(objtable.Rows[i]["poheader_date"]);
                    objDetails.poRefNo = objtable.Rows[i]["poheader_pono"].ToString();
                    objDetails.vendorName = objtable.Rows[i]["supplierheader_name"].ToString();
                    objDetails.status = objtable.Rows[i]["status_name"].ToString();
                    objDetails.poAmount = objtable.Rows[i]["poheader_over_total"].ToString();
                    objDetails.itType = objtable.Rows[i]["poheader_ittype"].ToString();
                    objDetails.poVersion = objtable.Rows[i]["poheader_version"].ToString();
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
                con.Close();
            }
        }
        public List<GRNInward> GetgrnSummary()
        {
            List<GRNInward> objraiser = new List<GRNInward>();
            try
            {
                getconnection();
                DataTable objtable = new DataTable();
                GRNInward objDetails;
                cmd = new SqlCommand("pr_fb_trn_grnSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectforGRN");
                cmd.Parameters.AddWithValue("@logingid", objCmnFunctions.GetLoginUserGid().ToString());
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objDetails = new GRNInward();
                    objDetails.slno = i + 1;
                    objDetails.grnheadgid = Convert.ToInt32(objtable.Rows[i]["grninwrdheader_gid"]);
                    objDetails.grnDate = Convert.ToString(objtable.Rows[i]["grninward_date"]);
                    objDetails.grnRefNo = Convert.ToString(objtable.Rows[i]["grninwrdheader_refno"]);
                    objDetails.poRefNo = objtable.Rows[i]["poheader_pono"].ToString();
                    objDetails.vendorName = objtable.Rows[i]["supplierheader_name"].ToString();
                    objDetails.grnStatus = objtable.Rows[i]["status_name"].ToString();
                    objDetails.poAmount = objtable.Rows[i]["poheader_over_total"].ToString();
                    objDetails.productName = objtable.Rows[i]["prodservice_name"].ToString();
                    objDetails.poVersion = objtable.Rows[i]["poheader_version"].ToString();
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
                con.Close();
            }
        }
        public List<GRNInward> GetscnSummary()
        {
            List<GRNInward> objraiser = new List<GRNInward>();
            try
            {
                getconnection();
                DataTable objtable = new DataTable();
                GRNInward objDetails;
                cmd = new SqlCommand("pr_fb_trn_grnSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectforSCN");
                cmd.Parameters.AddWithValue("@logingid", objCmnFunctions.GetLoginUserGid().ToString());
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objDetails = new GRNInward();
                    objDetails.slno = i + 1;
                    objDetails.grnheadgid = Convert.ToInt32(objtable.Rows[i]["grninwrdheader_gid"]);
                    objDetails.grnDate = Convert.ToString(objtable.Rows[i]["grninward_date"]);
                    objDetails.grnRefNo = Convert.ToString(objtable.Rows[i]["grninwrdheader_refno"]);
                    objDetails.poRefNo = objtable.Rows[i]["poheader_pono"].ToString();
                    objDetails.vendorName = objtable.Rows[i]["supplierheader_name"].ToString();
                    objDetails.grnStatus = objtable.Rows[i]["status_name"].ToString();
                    objDetails.poAmount = objtable.Rows[i]["poheader_over_total"].ToString();
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
                con.Close();
            }
        }
        public List<GRNInward> getStatusForGRN()
        {
            List<GRNInward> objraiser = new List<GRNInward>();
            try
            {
                getconnection();
                DataTable objtable = new DataTable();
                GRNInward objDetails;
                cmd = new SqlCommand("pr_fb_trn_grnSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "statusforGRN");
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                foreach (DataRow row in objtable.Rows)
                {
                    objDetails = new GRNInward();
                    objDetails.statusgid = Convert.ToInt32(row["status_gid"]);
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
                con.Close();
            }
        }
        public List<shipment> getbranchdetailsGrn(poraiser podetails)
        {
            List<shipment> objlist = new List<shipment>();
            try
            {
                getconnection();
                DataTable objtable = new DataTable();
                shipment objship;
                cmd = new SqlCommand("pr_fb_trn_poheader", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "ShipmentPO");
                cmd.Parameters.AddWithValue("@podetailgid", podetails.podetGid);
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objship = new shipment();
                    objship.SrNo = i + 1;
                    objship.branchedtype = objtable.Rows[i]["shipment_gid"].ToString();
                    objship.branchName = objtable.Rows[i]["branch_name"].ToString();
                    objship.address = objtable.Rows[i]["branchaddress"].ToString();
                    objship.location = objtable.Rows[i]["branch_location_name"].ToString();
                    objship.inchargeID = objtable.Rows[i]["employee_name"].ToString();
                    objship.totalqty = Convert.ToDecimal(objtable.Rows[i]["poshipment_qty"].ToString());
                    objship.releasedqty = objtable.Rows[i]["releasedqty"].ToString();
                    objship.balancedqty = Convert.ToDecimal(objtable.Rows[i]["balacned"].ToString());
                    objship.cbfdetGid = Convert.ToInt32(objtable.Rows[i]["podetails_gid"].ToString());
                    objship.shipmentgid = objtable.Rows[i]["poshipment_gid"].ToString();
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
                con.Close();
            }
        }
        public List<GRNInward> GetSelectionForPo()
        {
            List<GRNInward> objraiser = new List<GRNInward>();
            try
            {
                getconnection();
                DataTable objtable = new DataTable();

                GRNInward objDetails;
                cmd = new SqlCommand("pr_fb_trn_grnSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectforAddGrn");
                cmd.Parameters.AddWithValue("@logingid", objCmnFunctions.GetLoginUserGid().ToString());
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objDetails = new GRNInward();
                    objDetails.slno = i + 1;
                    objDetails.podetailsGid = Convert.ToString(objtable.Rows[i]["grnreleaseforpo_gid"]);
                    objDetails.poheadGid = Convert.ToInt32(objtable.Rows[i]["poheader_gid"]);
                    objDetails.poDate = Convert.ToString(objtable.Rows[i]["poheader_date"]);
                    objDetails.poRefNo = objtable.Rows[i]["poheader_pono"].ToString();
                    objDetails.vendorName = objtable.Rows[i]["supplierheader_name"].ToString();
                    objDetails.grnStatus = objtable.Rows[i]["status_name"].ToString();
                    objDetails.poAmount = objtable.Rows[i]["poheader_over_total"].ToString();
                    objDetails.quantBalanced = Convert.ToDecimal(objtable.Rows[i]["balanced_qty"].ToString());
                    objDetails.grnType = objtable.Rows[i]["grnreleaseforpo_branch_type"].ToString();
                    objDetails.productName = objtable.Rows[i]["prodservice_name"].ToString();
                    objDetails.poVersion = objtable.Rows[i]["poheader_version"].ToString();
                    objDetails.Description = objtable.Rows[i]["podetails_desc"].ToString();
                    objDetails.UnitPrice = objtable.Rows[i]["podetails_unitprice"].ToString();
                    objDetails.branchCode = Convert.ToString(objtable.Rows[i]["branch_code"]);
                    objDetails.branchname = Convert.ToString(objtable.Rows[i]["branch_name"]);
                    objDetails.assetDescr = Convert.ToString(objtable.Rows[i]["asset_description"]);
                    objDetails.PoDetailsAmount = Convert.ToString(objtable.Rows[i]["podetails_total"]);
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
                con.Close();
            }
        }
        public List<GRNInward> GetSelectionForWo()
        {
            List<GRNInward> objraiser = new List<GRNInward>();
            try
            {
                getconnection();
                DataTable objtable = new DataTable();

                GRNInward objDetails;
                cmd = new SqlCommand("pr_fb_trn_grnSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectforAddSCN");
                cmd.Parameters.AddWithValue("@logingid", objCmnFunctions.GetLoginUserGid().ToString());
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objDetails = new GRNInward();
                    objDetails.slno = i + 1;
                    objDetails.podetailsGid = Convert.ToString(objtable.Rows[i]["grnreleaseforpo_gid"]);
                    objDetails.poheadGid = Convert.ToInt32(objtable.Rows[i]["poheader_gid"]);
                    objDetails.poDate = Convert.ToString(objtable.Rows[i]["poheader_date"]);
                    objDetails.poRefNo = objtable.Rows[i]["poheader_pono"].ToString();
                    objDetails.vendorName = objtable.Rows[i]["supplierheader_name"].ToString();
                    objDetails.grnStatus = objtable.Rows[i]["status_name"].ToString();
                    objDetails.poAmount = objtable.Rows[i]["poheader_over_total"].ToString();
                    objDetails.quantBalanced = Convert.ToDecimal(objtable.Rows[i]["balanced_qty"].ToString());
                    objDetails.grnType = objtable.Rows[i]["grnreleaseforpo_branch_type"].ToString();
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
                con.Close();
            }
        }
        public List<GRNInward> GetSelectionForPoCwip(string group)
        {
            List<GRNInward> objraiser = new List<GRNInward>();
            try
            {
                getconnection();
                DataTable objtable = new DataTable();

                GRNInward objDetails;
                cmd = new SqlCommand("pr_fb_trn_grnSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectforgrnCwip");
                cmd.Parameters.AddWithValue("@logingid", objCmnFunctions.GetLoginUserGid().ToString());
                cmd.Parameters.AddWithValue("@requestforname", group.ToString());
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objDetails = new GRNInward();
                    objDetails.slno = i + 1;
                    objDetails.poheadGid = Convert.ToInt32(objtable.Rows[i]["poheader_gid"]);
                    objDetails.poDate = Convert.ToString(objtable.Rows[i]["poheader_date"]);
                    objDetails.poRefNo = objtable.Rows[i]["poheader_pono"].ToString();
                    objDetails.vendorName = objtable.Rows[i]["supplierheader_name"].ToString();
                    objDetails.grnStatus = objtable.Rows[i]["status_name"].ToString();
                    objDetails.poAmount = objtable.Rows[i]["poheader_over_total"].ToString();
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
                con.Close();
            }
        }

        public List<poraiser> GetPoDetailsListGRN(DataTable dt)
        {
            List<poraiser> objlist = new List<poraiser>();
            try
            {
                getconnection();

                poraiser objraiser;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objraiser = new poraiser();
                    objraiser.podetGid = Convert.ToInt32(dt.Rows[i]["podetails_gid"].ToString());
                    objraiser.poheadGid = Convert.ToInt32(dt.Rows[i]["poheader_gid"].ToString());
                    objraiser.porefno = dt.Rows[i]["poheader_pono"].ToString();
                    objraiser.podate = dt.Rows[i]["poheader_date"].ToString();
                    objraiser.poenddate = dt.Rows[i]["poheader_enddate"].ToString();
                    // objraiser.cbfheadAmount = Convert.ToDecimal(dt.Rows[i]["cbfheader_cbfamt"].ToString());
                    objraiser.devamount = Convert.ToDecimal(dt.Rows[i]["cbfheader_Devi_amt"].ToString());
                    objraiser.cbfdetailsQty = Convert.ToDecimal(dt.Rows[i]["cbfdetails_qty"].ToString());
                    objraiser.projmanagergid = Convert.ToInt32(dt.Rows[i]["poheader_projectmanager"].ToString());
                    objraiser.vendorName = GetVendorName(Convert.ToInt32(dt.Rows[i]["poheader_vendor_gid"].ToString()));
                    objraiser.itType = dt.Rows[i]["poheader_ittype"].ToString();
                    objraiser.department = getrequestName(Convert.ToInt32(dt.Rows[i]["poheader_requestfor"].ToString()));
                    objraiser.vendorNote = dt.Rows[i]["poheader_vendor_note"].ToString();
                    objraiser.templateGid = Convert.ToInt32(dt.Rows[i]["poheader_termcond_gid"].ToString());
                    objraiser.tempName = GetTemplateContent(objraiser.templateGid);
                    objraiser.templname = GetTemplateName(objraiser.templateGid);
                    objraiser.additionTemplate = dt.Rows[i]["poheader_add_termandcond"].ToString();
                    objraiser.prodservicegroup = dt.Rows[i]["productgroupname"].ToString();
                    objraiser.prodservicecode = dt.Rows[i]["prodservice_code"].ToString();
                    objraiser.prodservicename = dt.Rows[i]["prodservice_name"].ToString();
                    objraiser.prodservicedesc = dt.Rows[i]["podetails_desc"].ToString();
                    objraiser.units = dt.Rows[i]["uom_code"].ToString();
                    objraiser.quantity = Convert.ToDecimal(dt.Rows[i]["podetails_qty"]);
                    objraiser.actQuantity = Convert.ToDecimal(dt.Rows[i]["actQuantity"]);
                    objraiser.unitprice = Convert.ToDecimal(dt.Rows[i]["podetails_unitprice"].ToString());
                    objraiser.discount = Convert.ToDecimal(dt.Rows[i]["podetails_discount"].ToString());
                    objraiser.baseamount = Convert.ToDecimal(dt.Rows[i]["podetails_base_amt"].ToString());
                    objraiser.tax1 = Convert.ToDecimal(dt.Rows[i]["podetails_tax1"].ToString());
                    objraiser.tax2 = Convert.ToDecimal(dt.Rows[i]["podetails_tax2"].ToString());
                    objraiser.tax3 = Convert.ToDecimal(dt.Rows[i]["podetails_tax3"].ToString());
                    objraiser.total = Convert.ToDecimal(dt.Rows[i]["podetails_total"].ToString());
                    objraiser.cbfno = dt.Rows[i]["cbfheader_cbfno"].ToString();
                    objlist.Add(objraiser);
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
                con.Close();
            }
        }
        public List<GRNInward> GetPoDetailsForGRN(int poheadGid, string branchType)
        {
            List<GRNInward> objInward = new List<GRNInward>();
            try
            {
                getconnection();
                DataTable objtable = new DataTable();

                GRNInward objInwd;
                cmd = new SqlCommand("pr_fb_trn_grnSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectgrnpo");
                cmd.Parameters.AddWithValue("@poheadgid", poheadGid);
                cmd.Parameters.AddWithValue("@logingid", Convert.ToInt32(objCmnFunctions.GetLoginUserGid().ToString()));
                cmd.Parameters.AddWithValue("@releaseType", branchType);
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                HttpContext.Current.Session["grnTable"] = objtable;
                if (objtable.Rows.Count > 0)
                {
                    for (int i = 0; i < objtable.Rows.Count; i++)
                    {
                        objInwd = new GRNInward();
                        objInwd.slno = i + 1;
                        objInwd.podetailsGid = objtable.Rows[i]["podetails_gid"].ToString();
                        objInwd.poRefNo = objtable.Rows[i]["poheader_pono"].ToString();
                        objInwd.productGroup = objtable.Rows[i]["productgroup"].ToString();
                        objInwd.productCode = objtable.Rows[i]["prodservice_code"].ToString();
                        objInwd.productName = objtable.Rows[i]["productName"].ToString();
                        objInwd.uomcode = objtable.Rows[i]["uom_code"].ToString();
                        objInwd.vendorName = objtable.Rows[i]["supplierheader_name"].ToString();
                        objInwd.quantity = Convert.ToDecimal(objtable.Rows[i]["poshipment_qty"].ToString());
                        objInwd.quantReleased = Convert.ToDecimal(objtable.Rows[i]["grnreleaseforpo_released_qty"].ToString());
                        objInwd.quantBalanced = Convert.ToDecimal(objtable.Rows[i]["grnreleaseforpo_balanceqty"].ToString());
                        //objInwd.quantReceived = objInwd.quantReleased;
                        objInwd.quantReceived = Convert.ToDecimal(string.IsNullOrEmpty(objtable.Rows[i]["received_qty"].ToString()) ? "0" : objtable.Rows[i]["received_qty"].ToString());
                        objInwd.grnReleaseGid = Convert.ToInt32(objtable.Rows[i]["grnreleaseforpo_gid"].ToString());
                        objInwd.poreleasedDate = objtable.Rows[i]["grnreleaseforpo_released_date"].ToString();
                        objInward.Add(objInwd);
                    }
                }
                return objInward;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objInward;
            }
        }
        public List<GRNInward> GetPoDetailsForGRNCwip(int poheadGid)
        {
            List<GRNInward> objInward = new List<GRNInward>();
            try
            {
                getconnection();
                DataTable objtable = new DataTable();

                GRNInward objInwd;
                cmd = new SqlCommand("pr_fb_trn_grnSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectgrnpocwip");
                cmd.Parameters.AddWithValue("@poheadgid", poheadGid);
                cmd.Parameters.AddWithValue("@logingid", objCmnFunctions.GetLoginUserGid().ToString());
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                HttpContext.Current.Session["grnTable"] = objtable;
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objInwd = new GRNInward();
                    objInwd.slno = i + 1;
                    objInwd.podetailsGid = objtable.Rows[i]["podetails_gid"].ToString();
                    objInwd.poRefNo = objtable.Rows[i]["poheader_pono"].ToString();
                    objInwd.productGroup = objtable.Rows[i]["productgroup"].ToString();
                    objInwd.productCode = objtable.Rows[i]["prodservice_code"].ToString();
                    objInwd.productName = objtable.Rows[i]["productName"].ToString();
                    objInwd.uomcode = objtable.Rows[i]["uom_code"].ToString();
                    objInwd.vendorName = objtable.Rows[i]["supplierheader_name"].ToString();
                    objInwd.quantity = Convert.ToDecimal(objtable.Rows[i]["poshipment_qty"].ToString());
                    objInwd.quantReleased = Convert.ToDecimal(objtable.Rows[i]["grnreleaseforpo_released_qty"].ToString());
                    objInwd.grnReleaseGid = Convert.ToInt32(objtable.Rows[i]["grnreleaseforpo_gid"].ToString());
                    objInwd.poreleasedDate = objtable.Rows[i]["grnreleaseforpo_released_date"].ToString();
                    objInward.Add(objInwd);
                }
                return objInward;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objInward;
            }
        }
        public List<GRNInward> GetPoDetailsTemp(DataTable dt)
        {
            List<GRNInward> objInward = new List<GRNInward>();
            try
            {
                GRNInward objInwd;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objInwd = new GRNInward();
                    objInwd.slno = i + 1;
                    objInwd.podetailsGid = dt.Rows[i]["podetails_gid"].ToString();
                    objInwd.productGroup = dt.Rows[i]["productgroup"].ToString();
                    objInwd.productCode = dt.Rows[i]["prodservice_code"].ToString();
                    objInwd.productName = dt.Rows[i]["productName"].ToString();
                    objInwd.uomcode = dt.Rows[i]["uom_code"].ToString();
                    objInwd.vendorName = dt.Rows[i]["supplierheader_name"].ToString();
                    objInwd.quantity = Convert.ToDecimal(dt.Rows[i]["poshipment_qty"].ToString());
                    objInwd.quantReleased = Convert.ToDecimal(dt.Rows[i]["grnreleaseforpo_released_qty"].ToString());
                    objInwd.grnReleaseGid = Convert.ToInt32(dt.Rows[i]["grnreleaseforpo_gid"].ToString());
                    objInwd.poreleasedDate = dt.Rows[i]["grnreleaseforpo_released_date"].ToString();
                    objInwd.quantBalanced = Convert.ToDecimal(dt.Rows[i]["grnreleaseforpo_balanceqty"].ToString());
                    if (dt.Rows[i]["received_qty"].ToString() == "" || dt.Rows[i]["received_qty"].ToString() == null)
                    {
                        objInwd.quantReceived = Convert.ToDecimal(dt.Rows[i]["grnreleaseforpo_released_qty"].ToString());
                    }
                    else
                    {
                        objInwd.quantReceived = Convert.ToDecimal(dt.Rows[i]["received_qty"].ToString());
                    }
                    if (dt.Rows[i]["received_date"].ToString() == "" || dt.Rows[i]["received_date"].ToString() == null)
                    {
                        objInwd.receivedDate = "";
                    }
                    else
                    {
                        objInwd.receivedDate = dt.Rows[i]["received_date"].ToString();
                    }
                    objInward.Add(objInwd);
                }
                return objInward;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objInward;
            }
        }
        public DataTable DtTablegrn(shipment prdtgid)
        {
            DataTable dt = new DataTable();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_poheader", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "Released grn");
                cmd.Parameters.AddWithValue("@poshipmentgid", prdtgid.shipGid);
                cmd.Parameters.AddWithValue("@podetailgid", prdtgid.cbfdetGid);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
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
        public List<shipment> GetGRNDetailsForSave(DataTable dt)
        {
            List<shipment> objlist = new List<shipment>();
            try
            {
                getconnection();
                DataTable objtable = new DataTable();
                shipment objship;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objship = new shipment();
                    objship.SrNo = i + 1;
                    objship.branchedtype = dt.Rows[i]["shipment_gid"].ToString();
                    objship.branchName = dt.Rows[i]["branch_name"].ToString();
                    objship.address = dt.Rows[i]["branchaddress"].ToString();
                    objship.location = dt.Rows[i]["branch_location_name"].ToString();
                    objship.inchargeID = dt.Rows[i]["employee_name"].ToString();
                    objship.totalqty = Convert.ToDecimal(dt.Rows[i]["poshipment_qty"].ToString());
                    //objship.releasedqty = Convert.ToInt32(dt.Rows[i]["releasedqty"].ToString());
                    objship.balancedqty = Convert.ToDecimal(dt.Rows[i]["balacned"].ToString());
                    objship.releasedqty = dt.Rows[i]["balacned"].ToString();
                    objship.cbfdetGid = Convert.ToInt32(dt.Rows[i]["podetails_gid"].ToString());
                    objship.shipmentgid = dt.Rows[i]["poshipment_gid"].ToString();
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
                con.Close();
            }
        }
        public List<shipment> GetGRNDetailsForTempSave(DataTable dt)
        {
            List<shipment> objlist = new List<shipment>();
            try
            {
                getconnection();
                DataTable objtable = new DataTable();
                shipment objship;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objship = new shipment();
                    objship.SrNo = i + 1;
                    objship.branchedtype = dt.Rows[i]["shipment_gid"].ToString();
                    objship.branchName = dt.Rows[i]["branch_name"].ToString();
                    objship.address = dt.Rows[i]["branchaddress"].ToString();
                    objship.location = dt.Rows[i]["branch_location_name"].ToString();
                    objship.inchargeID = dt.Rows[i]["employee_name"].ToString();
                    objship.totalqty = Convert.ToDecimal(dt.Rows[i]["poshipment_qty"].ToString());
                    objship.releasedqty = dt.Rows[i]["releasedqty"].ToString();
                    objship.balancedqty = Convert.ToDecimal(dt.Rows[i]["balacned"].ToString());
                    objship.cbfdetGid = Convert.ToInt32(dt.Rows[i]["podetails_gid"].ToString());
                    objship.shipmentgid = dt.Rows[i]["poshipment_gid"].ToString();
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
                con.Close();
            }
        }
        public string InsertGrn(GRNInward objgrnheader, DataTable objTable, DataTable dtAttach)
        {
            try
            {
                getconnection();
                int a = 0;
                int b = 0;
                int received_qty = 1;
                int receivedQty = 0;
                int releasedQty = 0;
                string relpodet = "";
                DataTable objTablerel = new DataTable();
                if (objTable.Rows.Count > 0)
                {
                    for (int i = 0; i < objTable.Rows.Count; i++)
                    {
                        cmd = new SqlCommand("pr_fb_trn_grnSummary", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@actionName", "selecttype");
                        cmd.Parameters.AddWithValue("@grnreleaseforpo_gid", objTable.Rows[i]["grnreleaseforpo_gid"].ToString());
                        //objgrnheader.grnType = (string)cmd.ExecuteScalar();
                        da = new SqlDataAdapter(cmd);
                        da.Fill(objTablerel);
                        relpodet = objTable.Rows[i]["grnreleaseforpo_gid"].ToString();
                        if (objTablerel.Rows.Count > 0)
                        {
                            objgrnheader.grnType = objTablerel.Rows[0]["grnreleaseforpo_branch_type"].ToString();
                            objgrnheader.quantReleased = Convert.ToDecimal(objTablerel.Rows[0]["grnreleaseforpo_balanceqty"].ToString());
                        }
                        if (objgrnheader.grnType == "C")
                        {
                            objgrnheader.grnType = "C";
                        }
                        else
                        {
                            objgrnheader.grnType = "D";
                        }
                    }
                }
                cmd = new SqlCommand("pr_fb_trn_grnInsert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@grninwrdheader_refno", objCmnFunctions.GetSequenceNoFb("GRN", "", ""));
                cmd.Parameters.AddWithValue("@grninwrdheader_grndatetime", objCmnFunctions.convertoDateTime(objgrnheader.grnDate));
                cmd.Parameters.AddWithValue("@grninwardheader_poheader", objgrnheader.poheadGid);
                cmd.Parameters.AddWithValue("@grninwrdheader_rasiergid", objCmnFunctions.GetLoginUserGid().ToString());
                cmd.Parameters.AddWithValue("@grninwrdheader_dcno", objgrnheader.grnDcNo);
                cmd.Parameters.AddWithValue("@grninwrdheader_invoiceno", objgrnheader.grnInvoiceNo);
                cmd.Parameters.AddWithValue("@grninwrdheader_remarks", objgrnheader.grnRemarks);
                cmd.Parameters.AddWithValue("@grninwrdheader_branch_type", objgrnheader.grnType);

                if (objgrnheader.actionName == "Submit")
                {
                    cmd.Parameters.AddWithValue("@grninwrdheader_status", "9");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@grninwrdheader_status", "1");
                }
                cmd.Parameters.AddWithValue("@actionName", "insertgrnheader");
                cmd.Parameters.Add("@result", SqlDbType.Int).Direction = ParameterDirection.Output;
                a = cmd.ExecuteNonQuery();
                int grnheadGid = Convert.ToInt32(cmd.Parameters["@result"].Value);
                if (a == 1)
                {
                    if (objTable.Rows.Count > 0)
                    {
                        for (int i = 0; i < objTable.Rows.Count; i++)
                        {
                            decimal cnt = Convert.ToDecimal(objTable.Rows[i]["received_qty"].ToString());
                            string prodservicecode = objTable.Rows[i]["prodservice_code"].ToString();
                            DataTable dttype = new DataTable();
                            cmd = new SqlCommand("pr_fb_trn_grnintchek", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@prodservicecode", prodservicecode);
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dttype);
                            var values = cnt.ToString(CultureInfo.InvariantCulture).Split('.');
                            //int firstValue = int.Parse(values[0]);
                            //int secondValue = int.Parse(values[1]);
                            string type = dttype.Rows[0]["Type"].ToString();
                           
                            if (b == 0)
                            {
                                if ("int" == dttype.Rows[0]["Type"].ToString())
                                {
                                    for (int k = 0; k < cnt; k++)
                                    {
                                        cmd = new SqlCommand("pr_fb_trn_grnInsert", con);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@grninwrddet_grnreleforpo_gid", objTable.Rows[i]["grnreleaseforpo_gid"]);
                                        cmd.Parameters.AddWithValue("@grninwrddet_reced_qty", received_qty);
                                        cmd.Parameters.AddWithValue("@grninwrddet_reced_date", objCmnFunctions.convertoDateTime(objTable.Rows[i]["received_date"].ToString()));
                                        cmd.Parameters.AddWithValue("@actionName", "insertgrndetails");
                                        b = cmd.ExecuteNonQuery();
                                        if (b == 1)
                                        {

                                        }
                                    }
                                }
                                else
                                {
                                    if ("Decimal" == dttype.Rows[0]["Type"].ToString())
                                    {
                                        decimal qty = Convert.ToDecimal(objTable.Rows[i]["received_qty"].ToString());
                                        cmd = new SqlCommand("pr_fb_trn_grnInsert", con);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@grninwrddet_grnreleforpo_gid", objTable.Rows[i]["grnreleaseforpo_gid"]);
                                        cmd.Parameters.AddWithValue("@grninwrddet_reced_qty", qty);
                                        cmd.Parameters.AddWithValue("@grninwrddet_reced_date", objCmnFunctions.convertoDateTime(objTable.Rows[i]["received_date"].ToString()));
                                        cmd.Parameters.AddWithValue("@actionName", "insertgrndetails");
                                        b = cmd.ExecuteNonQuery();
                                    }
                                }

                                string[,] column = new string[,]
                        {
                            {"grnreleaseforpo_balanceqty", Convert.ToString(Convert.ToDecimal(objgrnheader.quantReleased)- Convert.ToDecimal(cnt))}
                        };
                                string[,] wherecol = new string[,]
                        {
                            {"grnreleaseforpo_gid",relpodet}
                        };
                                string tblname = "fb_trn_tgrnreleaseforpo";
                                string upre = objcommon.UpdateCommon(column, wherecol, tblname);
                            }

                        }
                    }
                    if (dtAttach != null)
                    {
                        if (dtAttach.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtAttach.Rows.Count; i++)
                            {
                                getconnection();
                                cmd = new SqlCommand("pr_fb_trn_grnInsert", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@grninwrdheader_gid", SqlDbType.Int).Value = grnheadGid;
                                cmd.Parameters.Add("@attachment_ref_flag", SqlDbType.TinyInt).Value = 11;
                                cmd.Parameters.Add("@attachment_attachmenttype_gid", SqlDbType.SmallInt).Value = 4;
                                cmd.Parameters.Add("@attachment_filename", SqlDbType.VarChar, 256).Value = dtAttach.Rows[i]["Documnet_Name"].ToString();
                                cmd.Parameters.Add("@attachment_desc", SqlDbType.VarChar).Value = dtAttach.Rows[i]["Document_des"].ToString();
                                cmd.Parameters.Add("@attachment_date", SqlDbType.VarChar).Value = objCmnFunctions.convertoDateTimeString(dtAttach.Rows[i]["Attachment_Date"].ToString());
                                cmd.Parameters.Add("@attachment_by", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                                cmd.Parameters.Add("@actionName", SqlDbType.VarChar).Value = "grnattachment";
                                int a2 = cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    //if (objgrnheader.grnType == "D" && objgrnheader.actionName == "Submit")
                    //{
                    //    cmd = new SqlCommand("pr_fb_trn_grnInsert", con);
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Parameters.AddWithValue("@logingid", objCmnFunctions.GetLoginUserGid().ToString());
                    //    cmd.Parameters.AddWithValue("@actionName", "grnsupervisor");
                    //    int queue_to = (int)cmd.ExecuteScalar();
                    //    string[,] codesIN = new string[,]
                    //                  {
                    //                       {"queue_date","sysdatetime()"},
                    //                       {"queue_ref_flag", "11"},
                    //                       {"queue_ref_gid",grnheadGid.ToString() },
                    //                       {"queue_ref_status", "0"},
                    //                       {"queue_from",objCmnFunctions.GetLoginUserGid().ToString() } ,
                    //                       {"queue_to_type","I"},
                    //                       {"queue_to", queue_to.ToString()},
                    //                       {"queue_action_for", "A"}

                    //                 };
                    //    string tnameIN = "iem_trn_tqueue";
                    //    string insertcommendecf = objcommon.InsertCommon(codesIN, tnameIN);
                    //}
                }
                if (b == 1)
                {
                    objgrnheader.result = "Inserted Successfully";
                }
                //else if (b == 2)
                //{
                //    objgrnheader.result = "Received Qty not valid !";
                //}
                return objgrnheader.result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
        }
        #region Modified
        public List<GRNInward> GetGRNDetails(int grnheadGid)
        {
            List<GRNInward> objInward = new List<GRNInward>();
            try
            {
                getconnection();
                DataTable objtable = new DataTable();

                GRNInward objInwd;
                cmd = new SqlCommand("pr_fb_trn_grnSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectgrndetcwip");
                cmd.Parameters.AddWithValue("@grninwrdheadgid", grnheadGid);
                //cmd.Parameters.AddWithValue("@logingid", objCmnFunctions.GetLoginUserGid().ToString());
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                HttpContext.Current.Session["grnTable"] = objtable;
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objInwd = new GRNInward();
                    objInwd.slno = i + 1;
                    objInwd.podetailsGid = objtable.Rows[i]["podetails_gid"].ToString();
                    objInwd.poRefNo = objtable.Rows[i]["poheader_pono"].ToString();
                    objInwd.productGroup = objtable.Rows[i]["productgroup"].ToString();
                    objInwd.productCode = objtable.Rows[i]["prodservice_code"].ToString();
                    objInwd.productName = objtable.Rows[i]["productName"].ToString();
                    objInwd.uomcode = objtable.Rows[i]["uom_code"].ToString();
                    objInwd.vendorName = objtable.Rows[i]["supplierheader_name"].ToString();
                    objInwd.quantity = Convert.ToDecimal(objtable.Rows[i]["poshipment_qty"].ToString());
                    objInwd.quantReleased = Convert.ToDecimal(objtable.Rows[i]["grnreleaseforpo_released_qty"].ToString());
                    objInwd.quantReceived = Convert.ToDecimal(objtable.Rows[i]["grninwrddet_reced_qty"].ToString());
                    objInwd.receivedDate = objtable.Rows[i]["grninwrddet_reced_date"].ToString();
                    objInwd.grnReleaseGid = Convert.ToInt32(objtable.Rows[i]["grnreleaseforpo_gid"].ToString());
                    objInwd.grnRefNo = objtable.Rows[i]["grninwrdheader_refno"].ToString();
                    objInwd.grnDcNo = objtable.Rows[i]["grninwrdheader_dcno"].ToString();
                    objInwd.grnInvoiceNo = objtable.Rows[i]["grninwrdheader_invoiceno"].ToString();
                    objInwd.grnRemarks = objtable.Rows[i]["grninwrdheader_remarks"].ToString();
                    objInwd.grndetgid = Convert.ToInt32(objtable.Rows[i]["grninwrddet_gid"].ToString());
                    objInwd.poshipmentGid = Convert.ToInt32(objtable.Rows[i]["poshipment_gid"].ToString());
                    objInwd.poreleasedDate = objtable.Rows[i]["grnreleaseforpo_released_date"].ToString();
                    objInwd.IsSerial = objtable.Rows[i]["IsSerial"].ToString();

                    objInward.Add(objInwd);
                }
                return objInward;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objInward;
            }
        }
        #endregion
        public List<GRNInward> GetGRNDetailsForCwip(int grnheadGid)
        {
            List<GRNInward> objInward = new List<GRNInward>();
            try
            {
                getconnection();
                DataTable objtable = new DataTable();

                GRNInward objInwd;
                cmd = new SqlCommand("pr_fb_trn_grnSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectgrndet");
                cmd.Parameters.AddWithValue("@grninwrdheadgid", grnheadGid);
                //cmd.Parameters.AddWithValue("@logingid", objCmnFunctions.GetLoginUserGid().ToString());
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                HttpContext.Current.Session["grnTable"] = objtable;
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objInwd = new GRNInward();
                    objInwd.slno = i + 1;
                    objInwd.podetailsGid = objtable.Rows[i]["podetails_gid"].ToString();
                    objInwd.poRefNo = objtable.Rows[i]["poheader_pono"].ToString();
                    objInwd.productGroup = objtable.Rows[i]["productgroup"].ToString();
                    objInwd.productCode = objtable.Rows[i]["prodservice_code"].ToString();
                    objInwd.productName = objtable.Rows[i]["productName"].ToString();
                    objInwd.uomcode = objtable.Rows[i]["uom_code"].ToString();
                    objInwd.vendorName = objtable.Rows[i]["supplierheader_name"].ToString();
                    objInwd.quantity = Convert.ToDecimal(objtable.Rows[i]["poshipment_qty"].ToString());
                    objInwd.quantReleased = Convert.ToDecimal(objtable.Rows[i]["grnreleaseforpo_released_qty"].ToString());
                    objInwd.quantReceived = Convert.ToDecimal(objtable.Rows[i]["grninwrddet_reced_qty"].ToString());
                    objInwd.receivedDate = objtable.Rows[i]["grninwrddet_reced_date"].ToString();
                    objInwd.quantBalanced = Convert.ToDecimal(objtable.Rows[i]["grnreleaseforpo_balanceqty"].ToString());
                    objInwd.grnReleaseGid = Convert.ToInt32(objtable.Rows[i]["grnreleaseforpo_gid"].ToString());
                    objInwd.grnRefNo = objtable.Rows[i]["grninwrdheader_refno"].ToString();
                    objInwd.grnDcNo = objtable.Rows[i]["grninwrdheader_dcno"].ToString();
                    objInwd.grnInvoiceNo = objtable.Rows[i]["grninwrdheader_invoiceno"].ToString();
                    objInwd.grnRemarks = objtable.Rows[i]["grninwrdheader_remarks"].ToString();
                    // objInwd.grndetgid = Convert.ToInt32(objtable.Rows[i]["grninwrddet_gid"].ToString());
                    objInwd.poshipmentGid = Convert.ToInt32(objtable.Rows[i]["poshipment_gid"].ToString());
                    objInwd.poreleasedDate = objtable.Rows[i]["grnreleaseforpo_released_date"].ToString();
                    objInward.Add(objInwd);
                }
                return objInward;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objInward;
            }
        }
        public string UpdateGrn(GRNInward objgrnheader, DataTable objTable, DataTable dtAttach)
        {
            try
            {
                getconnection();
                int a = 0;
                int b = 0;
                int recd_qty = 1;

                if (objTable.Rows.Count > 0)
                {
                    for (int i = 0; i < objTable.Rows.Count; i++)
                    {
                        cmd = new SqlCommand("pr_fb_trn_grnSummary", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@actionName", "selecttype");
                        cmd.Parameters.AddWithValue("@grnreleaseforpo_gid", objTable.Rows[i]["grnreleaseforpo_gid"].ToString());
                        objgrnheader.grnType = (string)cmd.ExecuteScalar();
                        if (objgrnheader.grnType == "C")
                        {
                            objgrnheader.grnType = "C";
                        }
                        else
                        {
                            objgrnheader.grnType = "D";
                        }
                    }
                }
                cmd = new SqlCommand("pr_fb_trn_grnInsert", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@grninwrdheader_dcno", objgrnheader.grnDcNo);
                cmd.Parameters.AddWithValue("@grninwrdheader_invoiceno", objgrnheader.grnInvoiceNo);
                cmd.Parameters.AddWithValue("@grninwrdheader_remarks", objgrnheader.grnRemarks);
                cmd.Parameters.AddWithValue("@grninwrdheader_gid", objgrnheader.grnheadgid);
                if (objgrnheader.actionName == "Submit")
                {
                    cmd.Parameters.AddWithValue("@grninwrdheader_status", "9");
                }
                else
                {
                    cmd.Parameters.AddWithValue("@grninwrdheader_status", "1");
                }
                cmd.Parameters.AddWithValue("@actionName", "updategrnheader");
                a = cmd.ExecuteNonQuery();
                if (a == 1)
                {
                    if (objTable.Rows.Count > 0)
                    {

                        for (int i = 0; i < objTable.Rows.Count; i++)
                        {
                           //int cnt = Convert.ToInt32(objTable.Rows[i]["grninwrddet_reced_qty"].ToString());
                            decimal cnt = Convert.ToDecimal(objTable.Rows[i]["received_qty"].ToString());
                            string prodservicecode = objTable.Rows[i]["prodservice_code"].ToString();
                            DataTable dttype = new DataTable();
                            cmd = new SqlCommand("pr_fb_trn_grnintchek", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@prodservicecode", prodservicecode);
                            da = new SqlDataAdapter(cmd);
                            da.Fill(dttype);
                            var values = cnt.ToString(CultureInfo.InvariantCulture).Split('.');
                            int firstValue = int.Parse(values[0]);
                            int secondValue = int.Parse(values[1]);
                            string type = dttype.Rows[0]["Type"].ToString();

                            if ("int" == dttype.Rows[0]["Type"].ToString())
                            {
                                for (int k = 0; k < cnt; k++)
                                {
                                    cmd = new SqlCommand("pr_fb_trn_grnInsert", con);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    if (objTable.Columns.Contains("grninwrddet_reced_qty") && objTable.Columns.Contains("grninwrddet_reced_date"))
                                    {
                                        cmd.Parameters.AddWithValue("@grninwrddet_reced_qty", recd_qty);
                                        cmd.Parameters.AddWithValue("@grninwrddet_reced_date", objCmnFunctions.convertoDateTime(objTable.Rows[i]["grninwrddet_reced_date"].ToString()));
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@grninwrddet_reced_qty", recd_qty);
                                        cmd.Parameters.AddWithValue("@grninwrddet_reced_date", objCmnFunctions.convertoDateTime(objTable.Rows[i]["received_date"].ToString()));
                                    }
                                    //cmd.Parameters.AddWithValue("@grninwrddet_gid", objgrnheader.grndetgid);
                                    cmd.Parameters.AddWithValue("@grninwrdheader_gid", objgrnheader.grnheadgid);
                                    cmd.Parameters.AddWithValue("@actionName", "updategrndetails");
                                    b = cmd.ExecuteNonQuery();
                                }
                            }
                            else
                            {
                                if ("Decimal" == dttype.Rows[0]["Type"].ToString())
                                {
                                    decimal qty = Convert.ToDecimal(objTable.Rows[i]["received_qty"].ToString());
                                    cmd = new SqlCommand("pr_fb_trn_grnInsert", con);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    if (objTable.Columns.Contains("grninwrddet_reced_qty") && objTable.Columns.Contains("grninwrddet_reced_date"))
                                    {
                                        cmd.Parameters.AddWithValue("@grninwrddet_reced_qty", qty);
                                        cmd.Parameters.AddWithValue("@grninwrddet_reced_date", objCmnFunctions.convertoDateTime(objTable.Rows[i]["grninwrddet_reced_date"].ToString()));
                                    }
                                    else
                                    {
                                        cmd.Parameters.AddWithValue("@grninwrddet_reced_qty", qty);
                                        cmd.Parameters.AddWithValue("@grninwrddet_reced_date", objCmnFunctions.convertoDateTime(objTable.Rows[i]["received_date"].ToString()));
                                    }
                                    //cmd.Parameters.AddWithValue("@grninwrddet_gid", objgrnheader.grndetgid);
                                    cmd.Parameters.AddWithValue("@grninwrdheader_gid", objgrnheader.grnheadgid);
                                    cmd.Parameters.AddWithValue("@actionName", "updategrndetails");
                                    b = cmd.ExecuteNonQuery();
                                }
                            }

                           
                        }
                    }
                    if (dtAttach != null)
                    {
                        if (dtAttach.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtAttach.Rows.Count; i++)
                            {
                                getconnection();
                                cmd = new SqlCommand("pr_fb_trn_grnInsert", con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add("@grninwrdheader_gid", SqlDbType.Int).Value = objgrnheader.grnheadgid;
                                cmd.Parameters.Add("@attachment_ref_flag", SqlDbType.TinyInt).Value = 11;
                                cmd.Parameters.Add("@attachment_attachmenttype_gid", SqlDbType.SmallInt).Value = 4;
                                cmd.Parameters.Add("@attachment_filename", SqlDbType.VarChar, 256).Value = dtAttach.Rows[i]["Documnet_Name"].ToString();
                                cmd.Parameters.Add("@attachment_desc", SqlDbType.VarChar).Value = dtAttach.Rows[i]["Document_des"].ToString();
                                cmd.Parameters.Add("@attachment_date", SqlDbType.VarChar).Value = objCmnFunctions.convertoDateTimeString(dtAttach.Rows[i]["Attachment_Date"].ToString());
                                cmd.Parameters.Add("@attachment_by", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                                cmd.Parameters.Add("@actionName", SqlDbType.VarChar).Value = "grnattachment";
                                int a2 = cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    //if (objgrnheader.grnType == "D" && objgrnheader.actionName == "Submit")
                    //{
                    //    cmd = new SqlCommand("pr_fb_trn_grnInsert", con);
                    //    cmd.CommandType = CommandType.StoredProcedure;
                    //    cmd.Parameters.AddWithValue("@logingid", objCmnFunctions.GetLoginUserGid().ToString());
                    //    cmd.Parameters.AddWithValue("@actionName", "grnsupervisor");
                    //    int queue_to = (int)cmd.ExecuteScalar();
                    //    string[,] codesIN = new string[,]
                    //                  {
                    //                       {"queue_date","sysdatetime()"},
                    //                       {"queue_ref_flag", "11"},
                    //                       {"queue_ref_gid",objgrnheader.grnheadgid.ToString() },
                    //                       {"queue_ref_status", "0"},
                    //                       {"queue_from",objCmnFunctions.GetLoginUserGid().ToString() } ,
                    //                       {"queue_to_type","I"},
                    //                       {"queue_to", queue_to.ToString()},
                    //                       {"queue_action_for", "A"}

                    //                 };
                    //    string tnameIN = "iem_trn_tqueue";
                    //    string insertcommendecf = objcommon.InsertCommon(codesIN, tnameIN);
                    //}
                }
                if (a == 1)
                {
                    objgrnheader.result = "Inserted Successfully";
                }
                return objgrnheader.result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
        }
        public string InsertWORelease(string ReleaseHeadGid)
        {
            string result = string.Empty;
            try
            {
                DataTable objtable = new DataTable();
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_SCNWORelease", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "GetWOdetails");
                cmd.Parameters.AddWithValue("@WOheadgid", ReleaseHeadGid);
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                if (objtable.Rows.Count > 0)
                {
                    for (int i = 0; i < objtable.Rows.Count; i++)
                    {
                        DataTable objtableship = new DataTable();
                        getconnection();
                        cmd = new SqlCommand("pr_fb_trn_SCNWORelease", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@actionName", "GetWOshipdetails");
                        cmd.Parameters.AddWithValue("@WOdetailgid", objtable.Rows[i]["podetails_gid"].ToString());
                        da = new SqlDataAdapter(cmd);
                        da.Fill(objtableship);
                        if (objtableship.Rows.Count > 0)
                        {
                            string[,] columnval = new string[,]
           {
                 {"grnreleaseforpo_podet_gid",objtable.Rows[i]["podetails_gid"].ToString()},
                 {"grnreleaseforpo_released_qty",objtableship.Rows[0]["poshipment_qty"].ToString()},
                 {"grnreleaseforpo_balanceqty",objtableship.Rows[0]["poshipment_qty"].ToString()},
                 {"grnreleaseforpo_released_date","sysdatetime()"},
                 {"grnreleaseforpo_releasedby",objCmnFunctions.GetLoginUserGid().ToString()},
                 {"grnreleaseforpo_isremoved","N"},
                 {"grnreleaseforpo_poshipment_gid",objtableship.Rows[0]["poshipment_gid"].ToString()},
                 {"grnreleaseforpo_branch_type","D"}
           };
                            string tblname1 = "fb_trn_tgrnreleaseforpo";
                            result = objcommon.InsertCommon(columnval, tblname1);
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
        }
        public string InsertRelease(DataTable objTable)
        {
            string result = string.Empty;
            try
            {
                for (int i = 0; i < objTable.Rows.Count; i++)
                {
                    getconnection();
                    cmd = new SqlCommand("pr_fb_trn_poheader", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@actionName", "shiptype");
                    cmd.Parameters.AddWithValue("@shipmentgid", objTable.Rows[i]["shipment_gid"].ToString());
                    result = (string)cmd.ExecuteScalar();
                    if (result == "CWIP")
                    {
                        string[,] columnval = new string[,]
           {
                 {"grnreleaseforpo_podet_gid",objTable.Rows[i]["podetails_gid"].ToString()},
                 {"grnreleaseforpo_released_qty",objTable.Rows[i]["releasedqty"].ToString()},
                 {"grnreleaseforpo_released_date","sysdatetime()"},
                 {"grnreleaseforpo_releasedby",objCmnFunctions.GetLoginUserGid().ToString()},
                 {"grnreleaseforpo_balanceqty",objTable.Rows[i]["releasedqty"].ToString()},
                 {"grnreleaseforpo_poshipment_gid",objTable.Rows[i]["poshipment_gid"].ToString()},
                 {"grnreleaseforpo_branch_type","C"}
           };
                        string tblname1 = "fb_trn_tgrnreleaseforpo";
                        result = objcommon.InsertCommon(columnval, tblname1);
                    }
                    else
                    {
                        string[,] columnval = new string[,]
           {
                 {"grnreleaseforpo_podet_gid",objTable.Rows[i]["podetails_gid"].ToString()},
                 {"grnreleaseforpo_released_qty",objTable.Rows[i]["releasedqty"].ToString()},
                 {"grnreleaseforpo_released_date","sysdatetime()"},
                 {"grnreleaseforpo_releasedby",objCmnFunctions.GetLoginUserGid().ToString()},
                 {"grnreleaseforpo_balanceqty",objTable.Rows[i]["releasedqty"].ToString()},
                 {"grnreleaseforpo_poshipment_gid",objTable.Rows[i]["poshipment_gid"].ToString()},
                 {"grnreleaseforpo_branch_type","D"}
           };
                        string tblname1 = "fb_trn_tgrnreleaseforpo";
                        result = objcommon.InsertCommon(columnval, tblname1);
                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
        }
        public List<GRNInward> GetApprovedPo()
        {
            List<GRNInward> objraiser = new List<GRNInward>();
            try
            {
                DataTable objtable = new DataTable();

                GRNInward objCwip;
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_grncwipsummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "select");
                cmd.Parameters.AddWithValue("@logingid", objCmnFunctions.GetLoginUserGid().ToString());
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                if (objtable.Rows.Count > 0)
                {
                    for (int i = 0; i < objtable.Rows.Count; i++)
                    {

                        objCwip = new GRNInward();
                        objCwip.slno = i + 1;
                        objCwip.poheadGid = Convert.ToInt32(objtable.Rows[i]["poheader_gid"]);
                        objCwip.podetailsGid = objtable.Rows[i]["podetails_gid"].ToString();
                        objCwip.poRefNo = objtable.Rows[i]["poheader_pono"].ToString();
                        objCwip.poDate = objtable.Rows[i]["poheader_date"].ToString();
                        objCwip.vendorName = objtable.Rows[i]["supplierheader_name"].ToString();
                        objCwip.poAmount = objtable.Rows[i]["poheader_over_total"].ToString();
                        objraiser.Add(objCwip);
                    }
                }
                return objraiser;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objraiser;
            }
        }

        //GRN Release For Cwip Summary

        public List<GRNInward> GetgrnSummaryForCwip()
        {
            List<GRNInward> objraiser = new List<GRNInward>();
            try
            {
                getconnection();
                DataTable objtable = new DataTable();

                GRNInward objDetails;
                cmd = new SqlCommand("pr_fb_trn_grnSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "grnsummaryforcwip");
                cmd.Parameters.AddWithValue("@logingid", objCmnFunctions.GetLoginUserGid().ToString());
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objDetails = new GRNInward();
                    objDetails.slno = i + 1;
                    objDetails.grnheadgid = Convert.ToInt32(objtable.Rows[i]["grninwrdheader_gid"]);
                    objDetails.grnDate = Convert.ToString(objtable.Rows[i]["grninward_date"]);
                    objDetails.grnRefNo = Convert.ToString(objtable.Rows[i]["grninwrdheader_refno"]);
                    objDetails.poRefNo = objtable.Rows[i]["poheader_pono"].ToString();
                    objDetails.vendorName = objtable.Rows[i]["supplierheader_name"].ToString();
                    objDetails.grnStatus = objtable.Rows[i]["status_name"].ToString();
                    objDetails.poAmount = objtable.Rows[i]["poheader_over_total"].ToString();
                    objDetails.poVersion = objtable.Rows[i]["poheader_version"].ToString();
                    //objDetails.branchCode = objtable.Rows[i]["branch_code"].ToString();
                    objraiser.Add(objDetails);
                }
                return objraiser;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objraiser;
            }
        }

        //GRN Release for CWIP Summary Details

        #region Modified
        public List<GRNInward> GetGrnDetailsTemp(DataTable dt)
        {
            List<GRNInward> objInward = new List<GRNInward>();
            try
            {
                GRNInward objInwd;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objInwd = new GRNInward();
                    objInwd.slno = i + 1;
                    objInwd.podetailsGid = dt.Rows[i]["podetails_gid"].ToString();
                    objInwd.poRefNo = dt.Rows[i]["poheader_pono"].ToString();
                    objInwd.productGroup = dt.Rows[i]["productgroup"].ToString();
                    objInwd.productCode = dt.Rows[i]["prodservice_code"].ToString();
                    objInwd.productName = dt.Rows[i]["productName"].ToString();
                    objInwd.uomcode = dt.Rows[i]["uom_code"].ToString();
                    objInwd.vendorName = dt.Rows[i]["supplierheader_name"].ToString();
                    objInwd.quantity = Convert.ToDecimal(dt.Rows[i]["poshipment_qty"].ToString());
                    objInwd.quantReleased = Convert.ToDecimal(dt.Rows[i]["grnreleaseforpo_released_qty"].ToString());
                    objInwd.quantReceived = Convert.ToDecimal(dt.Rows[i]["grninwrddet_reced_qty"].ToString());
                    objInwd.receivedDate = dt.Rows[i]["grninwrddet_reced_date"].ToString();
                    objInwd.grnReleaseGid = Convert.ToInt32(dt.Rows[i]["grnreleaseforpo_gid"].ToString());
                    objInwd.grnRefNo = dt.Rows[i]["grninwrdheader_refno"].ToString();
                    objInwd.grnDcNo = dt.Rows[i]["grninwrdheader_dcno"].ToString();
                    objInwd.grnInvoiceNo = dt.Rows[i]["grninwrdheader_invoiceno"].ToString();
                    objInwd.grnRemarks = dt.Rows[i]["grninwrdheader_remarks"].ToString();
                    objInwd.grndetgid = Convert.ToInt32(dt.Rows[i]["grninwrddet_gid"].ToString());
                    objInwd.poshipmentGid = Convert.ToInt32(dt.Rows[i]["poshipment_gid"].ToString());
                    objInwd.IsSerial = dt.Rows[i]["IsSerial"].ToString();
                    if (dt.Rows[i]["assetSlno"].ToString() != "" && dt.Rows[i]["assetSlno"].ToString() != null)
                    {
                        objInwd.assetSlno = dt.Rows[i]["assetSlno"].ToString();
                    }
                    objInward.Add(objInwd);
                }
                return objInward;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objInward;
            }
        }
        #endregion
        public DataTable GetBranchDetailsForGRN(string detgid, shipment objdetails)
        {
            DataTable objDt = new DataTable();
            try
            {
                if (!objDt.Columns.Contains("grninwddetgid") && !objDt.Columns.Contains("BranchName") && !objDt.Columns.Contains("Address") && !objDt.Columns.Contains("Location")
                    && !objDt.Columns.Contains("InchargeID") && !objDt.Columns.Contains("releasedQty") && !objDt.Columns.Contains("branchgid") && !objDt.Columns.Contains("poshipmentgid") && !objDt.Columns.Contains("empgid"))
                {
                    objDt.Columns.Add("grninwddetgid");
                    objDt.Columns.Add("BranchName");
                    objDt.Columns.Add("Address");
                    objDt.Columns.Add("Location");
                    objDt.Columns.Add("InchargeID");
                    objDt.Columns.Add("releasedQty");
                    objDt.Columns.Add("branchgid");
                    objDt.Columns.Add("poshipmentgid");
                    objDt.Columns.Add("empgid");
                }
                objDt.Rows.Add(detgid, objdetails.branchName, objdetails.address, objdetails.location,
                    objdetails.inchargeID, objdetails.releasedqty, objdetails.branchgid, objdetails.poshipmentgid,  objdetails.empgid);
                return objDt;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objDt;
            }
        }
        public List<shipment> GetBranchListForGRN(DataTable objtable)
        {
            List<shipment> objship = new List<shipment>();
            try
            {
                shipment obj;
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    obj = new shipment();
                    obj.SrNo = i + 1;
                    obj.branchName = objtable.Rows[i]["BranchName"].ToString();
                    obj.address = objtable.Rows[i]["Address"].ToString();
                    obj.location = objtable.Rows[i]["Location"].ToString();
                    obj.inchargeID = objtable.Rows[i]["InchargeID"].ToString();
                    obj.releasedqty = objtable.Rows[i]["releasedQty"].ToString();
                    obj.branchgid = Convert.ToInt32(objtable.Rows[i]["branchgid"].ToString());
                    obj.grndetGid = objtable.Rows[i]["grninwddetgid"].ToString();
                    obj.empgid = Convert.ToInt32(objtable.Rows[i]["empgid"].ToString());
                    objship.Add(obj);

                }
                return objship;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objship;
            }
        }
        public string InsertCwipRelease(DataTable objtable, GRNInward objshipment, DataTable dt)
        {
            try
            {
                string result = string.Empty;
                if (objtable.Rows.Count > 0)
                {
                    for (int i = 0; i < objtable.Rows.Count; i++)
                    {
                        string[,] columnval = new string[,]
                         {
                             {"grncwiprelease_grnheadgid",objshipment.grnheadgid.ToString()},
                             {"grncwiprelease_poshipmentgid",objtable.Rows[i]["poshipmentgid"].ToString()},
                             {"grncwiprelease_releasedqty",objtable.Rows[i]["releasedQty"].ToString()},
                             {"grncwiprelease_grninwddetgid",objtable.Rows[i]["grninwddetgid"].ToString()},
                             {"grncwiprelease_branchgid",objtable.Rows[i]["branchgid"].ToString()},
                             {"grncwiprelease_empgid",objtable.Rows[i]["empgid"].ToString()}
                         };
                        string tblname1 = "fb_trn_tgrncwiprelease";
                        result = objcommon.InsertCommon(columnval, tblname1);

                    }

                }
                if (result == "success")
                {
                    if (dt.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt.Rows.Count; j++)
                        {

                            string[,] column = new string[,]
                        {
                            {"grninwrddet_assetsrlno",dt.Rows[j]["assetSlno"].ToString()}
                        };
                            string[,] wherecol = new string[,]
                        {
                            {"grninwrddet_gid",dt.Rows[j]["grninwrddet_gid"].ToString()}
                        };
                            string tblname = "fb_trn_tgrninwrddet";
                            result = objcommon.UpdateCommon(column, wherecol, tblname);
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
        }


        public List<woraiser> PopulateAuditTrailWO(woraiser pat)
        {
            List<woraiser> objDashBoard = new List<woraiser>();
            try
            {

                woraiser objModel;
                int liRaiserGid = 0;
                int liCnt = 0;
                getconnection();
                DataTable dt = new DataTable();
                DataTable dtr = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_audittrail", con);
                cmd.Parameters.AddWithValue("@gid", pat.gid);
                cmd.Parameters.AddWithValue("@refflag", Convert.ToInt32(pat.ref_flag));
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
                            objModel = new woraiser();
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

                        objModel = new woraiser();
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
                            && queue_type != "R")
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
                con.Close();
            }
        }


        //public List<poraiser> PopulateAuditTrail(poraiser pat)
        //{
        //    try
        //    {
        //        List<poraiser> objDashBoard = new List<poraiser>();
        //        poraiser objModel;
        //        getconnection();
        //        DataTable dt = new DataTable();
        //        DataTable dtr = new DataTable();
        //        string streject = "";
        //        string strejectnew = "";
        //        string status = "";

        //        cmd = new SqlCommand("pr_fb_trn_audittaril", con);
        //        cmd.Parameters.AddWithValue("@gid", pat.gid);
        //        cmd.Parameters.AddWithValue("@refflag", pat.ref_flag);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        if (dt.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                string actions = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                string pergid = Convert.ToString(dt.Rows[i]["queue_prev_gid"].ToString());
        //                if (pergid == "0")
        //                {
        //                    streject = Convert.ToString(dt.Rows[i]["queue_from"].ToString());
        //                    string empcodnamer = Getempname(streject);
        //                    string[] datar;
        //                    datar = empcodnamer.Split(',');
        //                    objModel = new poraiser();
        //                    objModel.employee_code = datar[0].ToString() + "" + datar[1].ToString();
        //                    // objModel.employee_name = datar[1].ToString();
        //                  //  objModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());
        //                    objModel.action_status = "Submitted";
        //                    objModel.action_date = Convert.ToString(dt.Rows[i]["queue_date"].ToString());
        //                    objModel.queue_to_type = "PO Maker";
        //                    objDashBoard.Add(objModel);


        //                    if (actions != "")
        //                    {
        //                        string empid = Convert.ToString(dt.Rows[i]["queue_action_by"].ToString());
        //                        if (empid != "")
        //                        {
        //                            objModel = new poraiser();
        //                            string empcodname = Getempname(empid);
        //                            string[] data;
        //                            data = empcodname.Split(',');
        //                            objModel.employee_code = data[0].ToString() + "-" + data[1].ToString();
        //                            objModel.action_date = Convert.ToString(dt.Rows[i]["queue_action_date"].ToString());
        //                            objModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());
        //                            status = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                            objModel.action_status = GetQueueStatusHistry(status);
        //                            string queue_type = Convert.ToString(dt.Rows[i]["queue_to_type"].ToString());
        //                            string queue_to = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
        //                            objModel.queue_to_type = GetQueueRole(queue_type, "8", queue_to);
        //                            objDashBoard.Add(objModel);
        //                        }
        //                    }
        //                }
        //                else
        //                {

        //                    string empid = "";
        //                    string queue_type = Convert.ToString(dt.Rows[i]["queue_to_type"].ToString());
        //                    string queue_to = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
        //                    string actionstt = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                    if (actionstt != "")
        //                    {
        //                        objModel = new poraiser();
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
        //                            objModel.employee_code = data[0].ToString() + "-" + data[1].ToString();
        //                            objModel.action_date = Convert.ToString(dt.Rows[i]["queue_action_date"].ToString());
        //                            objModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());
        //                            status = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                            objModel.action_status = GetQueueStatusHistry(status);
        //                            objModel.queue_to_type = GetQueueRole(queue_type, "8", queue_to);
        //                            objDashBoard.Add(objModel);
        //                        }
        //                        // objModel.employee_name = Convert.ToString(dt.Rows[i]["employee_name"].ToString());

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
        //        con.Close();
        //    }
        //}

        //public List<poraiser> PopulateAuditTrail(poraiser pat)
        //{
        //    try
        //    {
        //        List<poraiser> objDashBoard = new List<poraiser>();
        //        poraiser objModel;
        //        getconnection();
        //        DataTable dt = new DataTable();
        //        DataTable dtr = new DataTable();
        //        string streject = "";
        //        string strejectnew = "";
        //        string status = "";
        //        string actionfor = "";
        //        string queue_action = "";

        //        cmd = new SqlCommand("pr_fb_trn_audittaril", con);
        //        cmd.Parameters.AddWithValue("@gid", pat.gid);
        //        cmd.Parameters.AddWithValue("@refflag", pat.ref_flag);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        if (dt.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dt.Rows.Count; i++)
        //            {
        //                string actions = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                string pergid = Convert.ToString(dt.Rows[i]["queue_prev_gid"].ToString());
        //                if (pergid == "0")
        //                {
        //                    streject = Convert.ToString(dt.Rows[i]["queue_from"].ToString());
        //                    string empcodnamer = Getempname(streject);
        //                    string[] datar;
        //                    datar = empcodnamer.Split(',');
        //                    objModel = new poraiser();
        //                    objModel.employee_code = datar[0].ToString() + "" + datar[1].ToString();
        //                    // objModel.employee_name = datar[1].ToString();
        //                    //  objModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());
        //                    objModel.action_status = "Submitted";
        //                    objModel.action_date = Convert.ToString(dt.Rows[i]["queue_date"].ToString());
        //                    objModel.queue_to_type = "PO Maker";
        //                    //  objDashBoard.Add(objModel);
        //                    if (i != 0)
        //                    {
        //                        actionfor = Convert.ToString(dt.Rows[i]["queue_action_for"].ToString());
        //                        queue_action = dt.Rows[i]["queue_action_status"].ToString();
        //                        if (actionfor == "R" && queue_action == "2")
        //                        {
        //                            objModel.action_status = "Rejected";
        //                        }
        //                        if (i > 0)
        //                        {
        //                            actionfor = Convert.ToString(dt.Rows[i - 1]["queue_action_for"].ToString());
        //                        }

        //                        //string ref_no = Convert.ToString(dt.Rows[i]["queue_ref_status"].ToString());
        //                        if (actionfor == "R")
        //                        {
        //                            objModel.action_status = "ReSubmitted";
        //                        }
        //                    }
        //                    objDashBoard.Add(objModel);
        //                    if (actions != "")
        //                    {
        //                        string empid = Convert.ToString(dt.Rows[i]["queue_action_by"].ToString());
        //                        if (empid != "")
        //                        {
        //                            objModel = new poraiser();
        //                            string empcodname = Getempname(empid);
        //                            string[] data;
        //                            data = empcodname.Split(',');
        //                            objModel.employee_code = data[0].ToString() + "-" + data[1].ToString();
        //                            objModel.action_date = Convert.ToString(dt.Rows[i]["queue_action_date"].ToString());
        //                            objModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());
        //                            status = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                            objModel.action_status = GetQueueStatusHistry(status);
        //                            string queue_type = Convert.ToString(dt.Rows[i]["queue_to_type"].ToString());
        //                            string queue_to = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
        //                            objModel.queue_to_type = GetQueueRole(queue_type, "8", queue_to);
        //                            objDashBoard.Add(objModel);
        //                        }
        //                    }
        //                }
        //                else
        //                {

        //                    string empid = "";
        //                    string queue_type = Convert.ToString(dt.Rows[i]["queue_to_type"].ToString());
        //                    string queue_to = Convert.ToString(dt.Rows[i]["queue_to"].ToString());
        //                    string actionstt = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                    if (actionstt != "")
        //                    {
        //                        objModel = new poraiser();
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
        //                            objModel.employee_code = data[0].ToString() + "-" + data[1].ToString();
        //                            objModel.action_date = Convert.ToString(dt.Rows[i]["queue_action_date"].ToString());
        //                            objModel.action_remarks = Convert.ToString(dt.Rows[i]["queue_action_remark"].ToString());
        //                            status = Convert.ToString(dt.Rows[i]["queue_action_status"].ToString());
        //                            objModel.action_status = GetQueueStatusHistry(status);
        //                            objModel.queue_to_type = GetQueueRole(queue_type, "8", queue_to);
        //                            actionfor = Convert.ToString(dt.Rows[i]["queue_action_for"].ToString());
        //                            if (actionfor == "R")
        //                            {
        //                                objModel.action_status = "Rejected";
        //                            }
        //                            objDashBoard.Add(objModel);
        //                        }
        //                        // objModel.employee_name = Convert.ToString(dt.Rows[i]["employee_name"].ToString());

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
        //        con.Close();
        //    }
        //}
        public List<poraiser> PopulateAuditTrail(poraiser pat)
        {
            List<poraiser> objDashBoard = new List<poraiser>();
            try
            {

                poraiser objModel;
                int liRaiserGid = 0;
                int liCnt = 0;
                getconnection();
                DataTable dt = new DataTable();
                DataTable dtr = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_audittrail", con);
                cmd.Parameters.AddWithValue("@gid", pat.gid);
                cmd.Parameters.AddWithValue("@refflag", Convert.ToInt32(pat.ref_flag));
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
                            objModel = new poraiser();
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

                        objModel = new poraiser();
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
                            && queue_type != "R")
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
                con.Close();
            }
        }
        public string GetEmployeeHierarchy(int raisergid = 0, string titlename = null, int titlerefgid = 0)
        {
            try
            {
                string lsResult = string.Empty;
                getconnection();
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
                getconnection();
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
        public string Getempname(string empgid)
        {
            string status = "";
            try
            {
                getconnection();
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
                return status;
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
        }

        public string GetQueueRole(string type, string flag, string gid)
        {
            try
            {
                string queuetype = string.Empty;

                if (type.Contains("G"))
                {
                    getconnection();
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
                if (type.Contains("R"))
                {
                    getconnection();
                    DataTable dt = new DataTable();
                    cmd = new SqlCommand("pr_fb_trn_audittaril", con);
                    cmd.Parameters.AddWithValue("@gid", gid);
                    cmd.Parameters.AddWithValue("@refflag", "R");
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        queuetype = Convert.ToString(dt.Rows[0]["role_name"].ToString());
                    }
                }
                if (type.Contains("L"))
                {
                    getconnection();
                    DataTable dt = new DataTable();
                    cmd = new SqlCommand("pr_fb_trn_audittaril", con);
                    cmd.Parameters.AddWithValue("@gid", gid);
                    cmd.Parameters.AddWithValue("@refflag", "L");
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        queuetype = Convert.ToString(dt.Rows[0]["grade_name"].ToString());
                    }
                }
                if (type.Contains("D"))
                {
                    getconnection();
                    DataTable dt = new DataTable();
                    cmd = new SqlCommand("pr_fb_trn_audittaril", con);
                    cmd.Parameters.AddWithValue("@gid", gid);
                    cmd.Parameters.AddWithValue("@refflag", "D");
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        queuetype = Convert.ToString(dt.Rows[0]["designation_name"].ToString());
                    }
                }
                if (type.Contains("I"))
                {
                    getconnection();
                    DataTable dt = new DataTable();
                    cmd = new SqlCommand("pr_fb_trn_audittaril", con);
                    cmd.Parameters.AddWithValue("@gid", gid);
                    cmd.Parameters.AddWithValue("@refflag", "I");
                    cmd.CommandType = CommandType.StoredProcedure;
                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
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
        }

        public GRNInward Attachmentname(GRNInward filename)
        {
            GRNInward obj = new GRNInward();
            try
            {
                int count = 0;
                int count1 = 0;
                if (HttpContext.Current.Session["count"] != null)
                {
                    count = (int)HttpContext.Current.Session["count"];
                }
                if (HttpContext.Current.Session["count1"] != null)
                {
                    count1 = (int)HttpContext.Current.Session["count1"];
                }

                if (filename.attach_id == "IEM.Areas.FLEXIBUY.Models.GRNInward")
                {
                    filename.attach_id = "";
                }

                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();

                Attachment obj_getprdetails;
                obj.attachment = new List<Attachment>();

                if (filename.attach_id != "IEM.Areas.FLEXIBUY.Models.GRNInward" || filename.attach_id != "" || filename.attach_id != null)
                {
                    filename.attachment = Getattachment(filename.attach_id, "11", "4");
                }
                if (filename.attachment != null)
                {
                    if (filename.attachment.Count > 0)
                    {
                        dt2.Columns.Add("Sno", typeof(string));
                        dt2.Columns.Add("Documnet_Name", typeof(string));
                        dt2.Columns.Add("Attachment_Date", typeof(string));
                        dt2.Columns.Add("Document_des", typeof(string));
                        for (int i = 0; i < filename.attachment.Count(); i++)
                        {
                            var row = dt2.NewRow();
                            row["Sno"] = i + 1;
                            row["Documnet_Name"] = filename.attachment[i].fileName;
                            row["Attachment_Date"] = filename.attachment[i].attachedDate;
                            row["Document_des"] = filename.attachment[i].description;
                            dt2.Rows.Add(row);
                        }
                    }
                }
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        obj_getprdetails = new Attachment()
                        {
                            attachGid = dt2.Rows[i]["Sno"].ToString(),
                            attachedDate = dt2.Rows[i]["Attachment_Date"].ToString(),
                            fileName = dt2.Rows[i]["Documnet_Name"].ToString(),
                            description = dt2.Rows[i]["Document_des"].ToString(),
                            employee_gid = dt2.Rows[i]["Document_des"].ToString(),

                        };
                        obj.attachment.Add(obj_getprdetails);
                    }
                    System.Web.HttpContext.Current.Session["AccessToken"] = dt2;
                }
                if (filename.attach_id == null || filename.attach_id == "")
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
                        if ((filename.attachdate != "" && filename.attachment1 != "" && filename.attachdes != "")
                            && (filename.attachdate != null && filename.attachment1 != null && filename.attachdes != null))
                        {
                            HttpContext.Current.Session["count"] = count + 1;
                            var row = dt.NewRow();
                            row["Sno"] = HttpContext.Current.Session["count"];
                            row["Documnet_Name"] = filename.attachment1;
                            row["Attachment_Date"] = filename.attachdate.ToString();
                            row["Document_des"] = filename.attachdes;
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

                //else
                //{
                //    if (HttpContext.Current.Session["AccessTokenheader"] == ""
                //           || HttpContext.Current.Session["AccessTokenheader"] == null && HttpContext.Current.Session["viewfor"] != null)
                //    {
                //        HttpContext.Current.Session["AccessTokenheader"] = HttpContext.Current.Session["cbfAttachmentDetails"];
                //    }
                //    if (HttpContext.Current.Session["AccessTokenheader"] != ""
                //        && HttpContext.Current.Session["AccessTokenheader"] != null)
                //    {
                //        dt2 = (DataTable)HttpContext.Current.Session["AccessTokenheader"];
                //    }
                //    else
                //    {
                //        dt2.Columns.Add("Sno", typeof(string));
                //        dt2.Columns.Add("prdetails", typeof(int));
                //        dt2.Columns.Add("Documnet_Name", typeof(string));
                //        dt2.Columns.Add("Attachment_Date", typeof(string));
                //        dt2.Columns.Add("Document_des", typeof(string));
                //        dt2.Columns.Add("cbfdetails_gid", typeof(string));
                //    }
                //    if ((filename.attachdate != "" && filename.attachment1 != "" && filename.attachdes != "")
                //        && (filename.attachdate != null && filename.attachment1 != null && filename.attachdes != null))
                //    {
                //        string number = filename.attach_id;
                //        string[] numberpar = number.Split('_');
                //        if (numberpar.Length > 1)
                //        {
                //            if (HttpContext.Current.Session["sno"] != null)
                //            {
                //                int sno = Convert.ToInt32(HttpContext.Current.Session["sno"]) + 1;
                //                filename.attach_id = sno.ToString();
                //                HttpContext.Current.Session["sno"] = sno;
                //            }
                //            else
                //            {
                //                int sno = Convert.ToInt32(HttpContext.Current.Session["sno"]) + 1;
                //                filename.attach_id = sno.ToString();
                //            }
                //        }
                //        HttpContext.Current.Session["count1"] = count1 + 1;
                //        var row = dt2.NewRow();
                //        row["Sno"] = count1 + 1;
                //        row["prdetails"] = filename.attach_id;
                //        row["Documnet_Name"] = filename.attachment1;
                //        row["Attachment_Date"] = filename.attachdate.ToString();
                //        row["Document_des"] = filename.attachdes;
                //        if (dt2.Columns.Contains("cbfdetails_gid"))
                //            row["cbfdetails_gid"] = filename.selectedValue;
                //        // row["cbfpar"] = filename.selectedValue;
                //        dt2.Rows.Add(row);
                //    }


                //    if (dt2.Rows.Count > 0)
                //    {
                //        string number = filename.attach_id;
                //        string[] numberpar = number.Split('_');
                //        if (numberpar.Length > 1)
                //        {
                //            filename.attach_id = numberpar[1].ToString();
                //        }
                //        DataView dv = new DataView();
                //        dt2.DefaultView.RowFilter = "prdetails = " + filename.attach_id + "";
                //        dv = dt2.DefaultView;
                //        for (int i = 0; i < dv.Count; i++)
                //        {
                //            obj_getprdetails = new Attachment()
                //            {
                //                attachGid = dv[i]["Sno"].ToString(),
                //                attachedDate = dv[i]["Attachment_Date"].ToString(),
                //                fileName = dv[i]["Documnet_Name"].ToString(),
                //                description = dv[i]["Document_des"].ToString(),
                //            };

                //            obj.attachment.Add(obj_getprdetails);
                //        }
                //    }
                //    System.Web.HttpContext.Current.Session["AccessTokenheader"] = dt2;
                //}

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


        public grnconfirmationdetails Attachmentname(grnconfirmationdetails filename)
        {
            grnconfirmationdetails obj = new grnconfirmationdetails();
            try
            {
                int count = 0;
                int count1 = 0;
                if (HttpContext.Current.Session["count"] != null)
                {
                    count = (int)HttpContext.Current.Session["count"];
                }
                if (HttpContext.Current.Session["count1"] != null)
                {
                    count1 = (int)HttpContext.Current.Session["count1"];
                }

                if (filename.attach_id == "IEM.Areas.FLEXIBUY.Models.GRNInward")
                {
                    filename.attach_id = "";
                }

                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();

                Attachment obj_getprdetails;
                obj.attachment = new List<Attachment>();

                if (filename.attach_id != "IEM.Areas.FLEXIBUY.Models.GRNInward" || filename.attach_id != "" || filename.attach_id != null)
                {
                    filename.attachment = Getattachment(filename.attach_id, "11", "4");
                }
                if (filename.attachment != null)
                {
                    if (filename.attachment.Count > 0)
                    {
                        dt2.Columns.Add("Sno", typeof(string));
                        dt2.Columns.Add("Documnet_Name", typeof(string));
                        dt2.Columns.Add("Attachment_Date", typeof(string));
                        dt2.Columns.Add("Document_des", typeof(string));
                        for (int i = 0; i < filename.attachment.Count(); i++)
                        {
                            var row = dt2.NewRow();
                            row["Sno"] = i + 1;
                            row["Documnet_Name"] = filename.attachment[i].fileName;
                            row["Attachment_Date"] = filename.attachment[i].attachedDate;
                            row["Document_des"] = filename.attachment[i].description;
                            dt2.Rows.Add(row);
                        }
                    }
                }
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        obj_getprdetails = new Attachment()
                        {
                            attachGid = dt2.Rows[i]["Sno"].ToString(),
                            attachedDate = dt2.Rows[i]["Attachment_Date"].ToString(),
                            fileName = dt2.Rows[i]["Documnet_Name"].ToString(),
                            description = dt2.Rows[i]["Document_des"].ToString(),
                            employee_gid = dt2.Rows[i]["Document_des"].ToString(),

                        };
                        obj.attachment.Add(obj_getprdetails);
                    }
                    System.Web.HttpContext.Current.Session["AccessToken"] = dt2;
                }
                if (filename.attach_id == null || filename.attach_id == "")
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
                        if ((filename.attachdate != "" && filename.attachment1 != "" && filename.attachdes != "")
                            && (filename.attachdate != null && filename.attachment1 != null && filename.attachdes != null))
                        {
                            HttpContext.Current.Session["count"] = count + 1;
                            var row = dt.NewRow();
                            row["Sno"] = HttpContext.Current.Session["count"];
                            row["Documnet_Name"] = filename.attachment1;
                            row["Attachment_Date"] = filename.attachdate.ToString();
                            row["Document_des"] = filename.attachdes;
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

                //else
                //{
                //    if (HttpContext.Current.Session["AccessTokenheader"] == ""
                //           || HttpContext.Current.Session["AccessTokenheader"] == null && HttpContext.Current.Session["viewfor"] != null)
                //    {
                //        HttpContext.Current.Session["AccessTokenheader"] = HttpContext.Current.Session["cbfAttachmentDetails"];
                //    }
                //    if (HttpContext.Current.Session["AccessTokenheader"] != ""
                //        && HttpContext.Current.Session["AccessTokenheader"] != null)
                //    {
                //        dt2 = (DataTable)HttpContext.Current.Session["AccessTokenheader"];
                //    }
                //    else
                //    {
                //        dt2.Columns.Add("Sno", typeof(string));
                //        dt2.Columns.Add("prdetails", typeof(int));
                //        dt2.Columns.Add("Documnet_Name", typeof(string));
                //        dt2.Columns.Add("Attachment_Date", typeof(string));
                //        dt2.Columns.Add("Document_des", typeof(string));
                //        dt2.Columns.Add("cbfdetails_gid", typeof(string));
                //    }
                //    if ((filename.attachdate != "" && filename.attachment1 != "" && filename.attachdes != "")
                //        && (filename.attachdate != null && filename.attachment1 != null && filename.attachdes != null))
                //    {
                //        string number = filename.attach_id;
                //        string[] numberpar = number.Split('_');
                //        if (numberpar.Length > 1)
                //        {
                //            if (HttpContext.Current.Session["sno"] != null)
                //            {
                //                int sno = Convert.ToInt32(HttpContext.Current.Session["sno"]) + 1;
                //                filename.attach_id = sno.ToString();
                //                HttpContext.Current.Session["sno"] = sno;
                //            }
                //            else
                //            {
                //                int sno = Convert.ToInt32(HttpContext.Current.Session["sno"]) + 1;
                //                filename.attach_id = sno.ToString();
                //            }
                //        }
                //        HttpContext.Current.Session["count1"] = count1 + 1;
                //        var row = dt2.NewRow();
                //        row["Sno"] = count1 + 1;
                //        row["prdetails"] = filename.attach_id;
                //        row["Documnet_Name"] = filename.attachment1;
                //        row["Attachment_Date"] = filename.attachdate.ToString();
                //        row["Document_des"] = filename.attachdes;
                //        if (dt2.Columns.Contains("cbfdetails_gid"))
                //            row["cbfdetails_gid"] = filename.selectedValue;
                //        // row["cbfpar"] = filename.selectedValue;
                //        dt2.Rows.Add(row);
                //    }


                //    if (dt2.Rows.Count > 0)
                //    {
                //        string number = filename.attach_id;
                //        string[] numberpar = number.Split('_');
                //        if (numberpar.Length > 1)
                //        {
                //            filename.attach_id = numberpar[1].ToString();
                //        }
                //        DataView dv = new DataView();
                //        dt2.DefaultView.RowFilter = "prdetails = " + filename.attach_id + "";
                //        dv = dt2.DefaultView;
                //        for (int i = 0; i < dv.Count; i++)
                //        {
                //            obj_getprdetails = new Attachment()
                //            {
                //                attachGid = dv[i]["Sno"].ToString(),
                //                attachedDate = dv[i]["Attachment_Date"].ToString(),
                //                fileName = dv[i]["Documnet_Name"].ToString(),
                //                description = dv[i]["Document_des"].ToString(),
                //            };

                //            obj.attachment.Add(obj_getprdetails);
                //        }
                //    }
                //    System.Web.HttpContext.Current.Session["AccessTokenheader"] = dt2;
                //}

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
        public List<cbfselection> getcbfheader()
        {
            throw new NotImplementedException();
        }

        public List<obfselection> getobfheader()
        {
            throw new NotImplementedException();
        }


        public List<woraiser> GetWoSplitList(DataTable dt, int monthId, int monthId1)
        {
            throw new NotImplementedException();
        }
        public string GetDelmatemployee(poRaising objapprove)
        {
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
            int podelmat_result = 0;
            int porefFlag = 0;

            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_GetPOqueueapp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pogid", objapprove.poheadGid);
                da = new SqlDataAdapter(cmd);
                DataTable dtcmd = new DataTable();
                da.Fill(dtcmd);
                if (dtcmd.Rows.Count > 0)
                {
                    queuengid = dtcmd.Rows[0]["queuegid"].ToString();
                    //queuento = dtcmd.Rows[0]["queueto"].ToString();.
                    //queuento = "6133";
                    queuento = objCmnFunctions.GetLoginUserGid().ToString();



                    cmd = new SqlCommand("pr_podelmat", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@poheader_gid", objapprove.poheadGid);
                    cmd.Parameters.AddWithValue("@queue_gid", queuengid);
                    cmd.Parameters.AddWithValue("@po_approver_gid", queuento);

                    cmd.Parameters.Add("@podelmat_result", SqlDbType.Int, 32);
                    cmd.Parameters["@podelmat_result"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@po_final_flag", SqlDbType.Char, 1);
                    cmd.Parameters["@po_final_flag"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@po_next_delmatmatrix_gid", SqlDbType.Int, 64);
                    cmd.Parameters["@po_next_delmatmatrix_gid"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@po_next_queue_to_type", SqlDbType.Char, 1);
                    cmd.Parameters["@po_next_queue_to_type"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@po_next_queue_to_gid", SqlDbType.Int, 64);
                    cmd.Parameters["@po_next_queue_to_gid"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@po_err_output", SqlDbType.VarChar, 10000);
                    cmd.Parameters["@po_err_output"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@po_sql_output", SqlDbType.VarChar, 10000);
                    cmd.Parameters["@po_sql_output"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@po_next_queue_to_additional_flag", SqlDbType.VarChar, 10000);
                    cmd.Parameters["@po_next_queue_to_additional_flag"].Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    string finalfalg = Convert.ToString(cmd.Parameters["@po_final_flag"].Value);
                    string nqueuety = Convert.ToString(cmd.Parameters["@po_next_queue_to_type"].Value);
                    string nqueuet = Convert.ToString(cmd.Parameters["@po_next_queue_to_gid"].Value);
                    string ndelgid = Convert.ToString(cmd.Parameters["@po_next_delmatmatrix_gid"].Value);
                    string po_erroutput = Convert.ToString(cmd.Parameters["@po_err_output"].Value);
                    string po_sql_output = Convert.ToString(cmd.Parameters["@po_sql_output"].Value);
                    string additional_flag = Convert.ToString(cmd.Parameters["@po_next_queue_to_additional_flag"].Value);
                    podelmat_result = Convert.ToInt32(cmd.Parameters["@podelmat_result"].Value);


                    porefFlag = Convert.ToInt32(ConfigurationManager.AppSettings["porefgid"].ToString());

                    if (podelmat_result > 0)
                    {
                        if (objapprove.delmatview != "delmat")
                        {
                            PoCheckerApprove(objapprove);
                        }
                        if (finalfalg == "Y")
                        {
                            string[,] codes1 = new string[,]            
                       {
                           {"poheader_status","5"}
                           

                       };
                            string[,] whrcol = new string[,]
                        {
                          
                             {"poheader_gid",objapprove.poheadGid.ToString()}

                        };
                            string tname1 = "fb_trn_tpoheader";
                            string updatequery = objcommon.UpdateCommon(codes1, whrcol, tname1);

                            if (updatequery == "Success")
                            {
                                //cmd = new SqlCommand("pr_fb_trn_POApprovalQueue", con);
                                //cmd.CommandType = CommandType.StoredProcedure;
                                //cmd.Parameters.AddWithValue("@refgid", objapprove.poheadGid);
                                //cmd.Parameters.AddWithValue("@request_type", "PO");
                                //cmd.Parameters.AddWithValue("@actionName", "queueprev");
                                //int poqueue_prevgid = (int)cmd.ExecuteScalar();
                                string[,] codesq = new string[,]
                                    {
                                         {"queue_action_date","sysdatetime()"},
                                         {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString() },
                                         {"queue_action_remark",Convert.ToString(string.IsNullOrEmpty(objapprove.remarks) ? "" :objapprove.remarks)},
                                         {"queue_action_status","1" },
                                         {"queue_isremoved","Y" },
                                         {"queue_delmat_flag","F"}
                                     };
                                string[,] whreq = new string[,]
                                    {
                                     
                                        {"queue_ref_gid",objapprove.poheadGid.ToString()},
                                        {"queue_ref_flag",porefFlag.ToString()},
                                        {"queue_isremoved","N" }
                                   };
                                string tnameq = "iem_trn_tqueue";
                                string insertcommendq = objcommon.UpdateCommon(codesq, whreq, tnameq);
                                resultone = insertcommendq;
                            }

                        }

                        else
                        {
                            //getconnection();
                            //cmd = new SqlCommand("pr_fb_trn_POApprovalQueue", con);
                            //cmd.CommandType = CommandType.StoredProcedure;
                            //cmd.Parameters.AddWithValue("@refgid", objapprove.poheadGid);
                            //cmd.Parameters.AddWithValue("@request_type", "PO");
                            //cmd.Parameters.AddWithValue("@actionName", "queueprev");
                            //int poqueue_prevgid = (int)cmd.ExecuteScalar();
                            string[,] colum = new string[,]
                                    {
                                         {"queue_isremoved","Y" }
                                    };
                            string[,] wherecolumn = new string[,]
                                    {
                                     
                                        {"queue_ref_gid",objapprove.poheadGid.ToString()},
                                         {"queue_isremoved","N" },
                                         {"queue_prev_gid","0"},
                                         {"queue_ref_flag",porefFlag.ToString()}
                                   };
                            string tblname = "iem_trn_tqueue";
                            string updatequeue = objcommon.UpdateCommon(colum, wherecolumn, tblname);

                            if (objapprove.delmatview == "delmat")
                            {
                                string[,] codesq = new string[,]
                                    {
                                         {"queue_action_date","sysdatetime()"},
                                         {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString() },
                                         {"queue_action_status","1" },
                                         {"queue_action_remark",Convert.ToString(string.IsNullOrEmpty(objapprove.remarks) ? "" :objapprove.remarks)},
                                         {"queue_isremoved","Y" }
                                     };
                                string[,] whreq = new string[,]
                                    {
                                     
                                         {"queue_ref_gid",objapprove.poheadGid.ToString()},
                                         {"queue_isremoved","N" },
                                         {"queue_ref_flag",porefFlag.ToString()}
                                   };
                                string tnameq = "iem_trn_tqueue";
                                string insertcommendq = objcommon.UpdateCommon(codesq, whreq, tnameq);
                            }
                            if (additional_flag == "")
                            {
                                additional_flag = "N";
                            }

                            string[,] codesIN = new string[,]
                                      {
                                           {"queue_date","sysdatetime()"},
                                           {"queue_ref_flag", "8"},
                                           {"queue_ref_gid",objapprove.poheadGid.ToString() },
                                           {"queue_ref_status", "0"},
                                           {"queue_from",objCmnFunctions.GetLoginUserGid().ToString() } ,
                                           {"queue_to_type", nqueuety.ToString()},
                                           {"queue_to", nqueuet.ToString()},
                                           {"queue_action_for", "A"},
                                           {"queue_delmat_flag","D"},
                                           {"queue_prev_gid",Convert.ToString(GetPOQueueGid(objapprove.poheadGid.ToString()))},
                                           {"Additional_flag",additional_flag}
                                     };
                            string tnameIN = "iem_trn_tqueue";

                            string insertcommendecf = objcommon.InsertCommon(codesIN, tnameIN);
                            resultone = insertcommendecf;
                        }
                    }
                    else
                    {
                        resultone = po_erroutput;
                    }

                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }
            return resultone;
        }

        public int GetPOQueueGid(string poheadGid)
        {
            int prrvgid = 0;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_GetPOqueuegid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pogid", poheadGid.ToString());
                prrvgid = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                prrvgid = 0;
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            return prrvgid;
        }

        public int GetWOQueueGid(string woheadGid)
        {
            int prrvgid = 0;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_GetWOqueuegid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@wogid", woheadGid.ToString());
                prrvgid = (int)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                prrvgid = 0;
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            finally
            {
                con.Close();
            }
            return prrvgid;
        }
        public string GetDelmatemployeeForWo(WoSummary objapprove)
        {
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
            int worefFlag = 0;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_GetWOqueueapp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@wogid", objapprove.woheadGid);
                da = new SqlDataAdapter(cmd);
                DataTable dtcmd = new DataTable();
                da.Fill(dtcmd);
                if (dtcmd.Rows.Count > 0)
                {
                    queuengid = dtcmd.Rows[0]["queuegid"].ToString();
                    //queuento = dtcmd.Rows[0]["queueto"].ToString();.
                    queuento = objCmnFunctions.GetLoginUserGid().ToString();



                    cmd = new SqlCommand("pr_podelmat", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@poheader_gid", objapprove.woheadGid);
                    cmd.Parameters.AddWithValue("@queue_gid", queuengid);
                    cmd.Parameters.AddWithValue("@po_approver_gid", queuento);



                    cmd.Parameters.Add("@po_final_flag", SqlDbType.Char, 1);
                    cmd.Parameters["@po_final_flag"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@po_next_delmatmatrix_gid", SqlDbType.Int, 64);
                    cmd.Parameters["@po_next_delmatmatrix_gid"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@po_next_queue_to_type", SqlDbType.Char, 1);
                    cmd.Parameters["@po_next_queue_to_type"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@po_next_queue_to_gid", SqlDbType.Int, 64);
                    cmd.Parameters["@po_next_queue_to_gid"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@po_err_output", SqlDbType.VarChar, 10000);
                    cmd.Parameters["@po_err_output"].Direction = ParameterDirection.Output;

                    cmd.Parameters.Add("@po_sql_output", SqlDbType.VarChar, 10000);
                    cmd.Parameters["@po_sql_output"].Direction = ParameterDirection.Output;


                    cmd.ExecuteNonQuery();

                    string finalfalg = Convert.ToString(cmd.Parameters["@po_final_flag"].Value);
                    string nqueuety = Convert.ToString(cmd.Parameters["@po_next_queue_to_type"].Value);
                    string nqueuet = Convert.ToString(cmd.Parameters["@po_next_queue_to_gid"].Value);
                    string ndelgid = Convert.ToString(cmd.Parameters["@po_next_delmatmatrix_gid"].Value);
                    string po_erroutput = Convert.ToString(cmd.Parameters["@po_err_output"].Value);
                    string po_sql_output = Convert.ToString(cmd.Parameters["@po_sql_output"].Value);


                    worefFlag = Convert.ToInt32(ConfigurationManager.AppSettings["worefFlag"].ToString());

                    if (po_erroutput == "")
                    {
                        if (objapprove.delmatView != "delmat")
                        {
                            WoCheckerApprove(objapprove);
                        }
                        if (finalfalg == "Y")
                        {
                            string[,] codes1 = new string[,]            
                       {
                           {"poheader_status","5"},

                       };
                            string[,] whrcol = new string[,]
                        {
                          
                             {"poheader_gid",objapprove.woheadGid.ToString()}

                        };
                            string tname1 = "fb_trn_tpoheader";
                            string updatequery = objcommon.UpdateCommon(codes1, whrcol, tname1);
                            string[,] codesq = new string[,]
                                    {
                                         {"queue_action_date","sysdatetime()"},
                                         {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString() },
                                         {"queue_action_remark",Convert.ToString(string.IsNullOrEmpty(objapprove.remarks) ? "" :objapprove.remarks)},
                                         {"queue_action_status","1" },
                                         {"queue_isremoved","Y" },
                                         {"queue_delmat_flag","F"}
                                     };
                            string[,] whreq = new string[,]
                                    {
                                     
                                        {"queue_ref_gid",objapprove.woheadGid.ToString()},
                                        {"queue_ref_flag",worefFlag.ToString()},
                                         {"queue_isremoved","N" }
                                   };
                            string tnameq = "iem_trn_tqueue";
                            string insertcommendq = objcommon.UpdateCommon(codesq, whreq, tnameq);
                            resultone = insertcommendq;


                        }

                        else
                        {
                            if (objapprove.delmatView == "delmat")
                            {
                                string[,] codesq = new string[,]
                                    {
                                         {"queue_action_date","sysdatetime()"},
                                         {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString() },
                                         {"queue_action_remark",Convert.ToString(string.IsNullOrEmpty(objapprove.remarks) ? "" :objapprove.remarks)},
                                         {"queue_action_status","1" },
                                         {"queue_isremoved","Y" }
                                     };
                                string[,] whreq = new string[,]
                                    {
                                     
                                        {"queue_ref_gid",objapprove.woheadGid.ToString()},
                                        {"queue_ref_flag",worefFlag.ToString()},
                                        {"queue_isremoved","N" }
                                   };
                                string tnameq = "iem_trn_tqueue";
                                string insertcommendq = objcommon.UpdateCommon(codesq, whreq, tnameq);
                            }
                            string[,] codesIN = new string[,]
                                      {
                                           {"queue_date","sysdatetime()"},
                                           {"queue_ref_flag", "10"},
                                           {"queue_ref_gid",objapprove.woheadGid.ToString() },
                                           {"queue_ref_status", "0"},
                                           {"queue_from",objCmnFunctions.GetLoginUserGid().ToString() } ,
                                           {"queue_to_type", nqueuety.ToString()},
                                           {"queue_to", nqueuet.ToString()},
                                           {"queue_action_for", "A"},
                                           {"queue_prev_gid",Convert.ToString(GetWOQueueGid(objapprove.woheadGid.ToString()))},
                                           {"queue_delmat_flag","D"}
                                     };
                            string tnameIN = "iem_trn_tqueue";

                            string insertcommendecf = objcommon.InsertCommon(codesIN, tnameIN);
                            resultone = insertcommendecf;
                        }
                    }

                    if (po_erroutput != "")
                    {
                        resultone = po_erroutput;
                    }

                }
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "";
            }

            return resultone;
        }
        //public CbfSumEntity GetFccc()
        //{
        //    try
        //    {
        //        CbfSumEntity obj = new CbfSumEntity();
        //        FcccMaster obj_getvalues;
        //        obj.searchFccc = new List<FcccMaster>();
        //        getconnection();

        //        DataTable dt = new DataTable();
        //        cmd = new SqlCommand("pr_fb_mst_tfccc", con);
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        cmd.Parameters.AddWithValue("@action", "selectforficc");
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        foreach (DataRow row in dt.Rows)
        //        {
        //            obj_getvalues = new FcccMaster()
        //            {
        //                fcccCode = row["fccc_gid"].ToString(),
        //                fcccGid = row["fccc_code"].ToString(),
        //                fcccName = row["fccc_name"].ToString(),

        //            };
        //            obj.searchFccc.Add(obj_getvalues);
        //        }
        //        return obj;
        //    }

        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    finally
        //    {
        //        con.Close();
        //    }
        //}
        public List<Attachment> Getattachment(string id, string refe, string type)
        {
            CbfSumEntity ob = new CbfSumEntity();
            try
            {
                getconnection();

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
                return ob.attachment;
            }

        }

        public List<EmployeeSearch> GetEmployeeDetails()
        {
            List<EmployeeSearch> obj = new List<EmployeeSearch>();
            try
            {
                getconnection();
                DataTable objTable = new DataTable();

                EmployeeSearch objproject;
                cmd = new SqlCommand("pr_fb_iem_mst_employee", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "Select");
                da = new SqlDataAdapter(cmd);
                da.Fill(objTable);
                for (int i = 0; i < objTable.Rows.Count; i++)
                {
                    objproject = new EmployeeSearch();
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
                con.Close();
            }
        }
        public int GetemployId(string inchargeId)
        {
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_poheader", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "getempid");
                cmd.Parameters.AddWithValue("@employcode", inchargeId);
                int employid = (int)cmd.ExecuteScalar();
                return employid;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
        }
        public int GetBranchId(string branchCode)
        {
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_poheader", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "getbranchgid");
                cmd.Parameters.AddWithValue("@branchcode", branchCode);
                int branchgid = (int)cmd.ExecuteScalar();
                return branchgid;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
        }
        public DataSet GetGRNSummaryNew() 
        {
            DataSet ds = new DataSet(); 
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_grnSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("@actionName", "selectforGRNNew");
                cmd.Parameters.AddWithValue("@logingid", objCmnFunctions.GetLoginUserGid().ToString());
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
            }
        }
        public DataSet GetSCNSummaryNew() 
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_grnSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("@actionName", "selectforSCN");
                cmd.Parameters.AddWithValue("@logingid", objCmnFunctions.GetLoginUserGid().ToString());
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
            }
        }
        public string GrnReleaseFull(int pogid = 0)
        {
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_grnreleasefull", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.AddWithValue("@POGID", pogid);
                cmd.Parameters.AddWithValue("@EMPGID", objCmnFunctions.GetLoginUserGid().ToString());
                string result = Convert.ToString(cmd.ExecuteScalar());
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "0";
            }
            finally
            {
                con.Close();
            }
        }
        public string Getgrntype(string prodservicecode)
        {
            try
            {
                getconnection();
                DataTable dttype = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_grnintchek", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prodservicecode", prodservicecode);
                da = new SqlDataAdapter(cmd);
                da.Fill(dttype);
                string type = null;
                if (dttype.Rows.Count > 0)
                {
                    type = dttype.Rows[0]["Type"].ToString();
                }
                return type;
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

        public int CreatePOAttachment(Attachments POAttachment)
        {
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_poheader", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (POAttachment.AttachmentFor == "PO")
                {
                    cmd.Parameters.Add("@actionName", SqlDbType.VarChar).Value = "getrefflagPO";
                }

                string refflag = Convert.ToString(cmd.ExecuteScalar());
                string attachmenttype = "0";
                string[,] codes2 = new string[,] 
                   {
                     {"attachment_ref_flag",string.IsNullOrEmpty(refflag)?"0":refflag},
                     {"attachment_ref_gid",string.IsNullOrEmpty(POAttachment.AttachmentRefGid.ToString())?"0":POAttachment.AttachmentRefGid.ToString()},
                     {"attachment_filename",string.IsNullOrEmpty(POAttachment.AttachedActualFileName)?"":POAttachment.AttachedActualFileName},
                     {"attachment_desc",string.IsNullOrEmpty(POAttachment.AttachDescription)?"":POAttachment.AttachDescription},
                     {"attachment_attachmenttype_gid",string.IsNullOrEmpty(attachmenttype)?"0":attachmenttype},
                     {"attachment_date","sysdatetime()" },
                     {"attachment_by",objCmnFunctions.GetLoginUserGid().ToString()},
                     {"attachment_isremoved","N" },
                     {"attachment_tempfilename",string.IsNullOrEmpty(POAttachment.TempFileName)?"":POAttachment.TempFileName},
                   };
                string insertcommand2 = objcommon.InsertCommon(codes2, "iem_trn_tattachment");

                return 1;
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

        public int DeletePOAttachment(string id)
        {
            try
            {
                string[,] codw2 = new string[,] 
                   {
                     {"attachment_isremoved","Y" }
                   };
                string[,] codewhe2 = new string[,]
                {
                    {"attachment_gid",id}
                };
                string updatecon2 = objcommon.UpdateCommon(codw2, codewhe2, "iem_trn_tattachment");
                return 1;
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
    }
}


