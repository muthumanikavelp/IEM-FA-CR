using IEM.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace IEM.Areas.FLEXIBUY.Models
{
    public class WODataModel:WORepository
    {
        SqlCommand cmd;
        SqlConnection con = new SqlConnection();
        SqlDataAdapter da;
        ErrorLog objErrorLog = new ErrorLog();
        CommonIUD objCommonIUD = new CommonIUD();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        public void getconnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }
        public DataSet GetWODetails(int cbfdetailsgid = 0) 
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@cbfdetailsgid", SqlDbType.Int).Value = cbfdetailsgid;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getwodetails";
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
        public int AddParentWODetails(WOEntity objWoEntity)
        {
            try
            {
                string woheadergid = "";
                int result = 0;
                decimal poheaderamnt = Convert.ToDecimal(string.IsNullOrEmpty(objWoEntity.WOUtilAmount) ? "0" : objWoEntity.WOUtilAmount) +
                                       Convert.ToDecimal(string.IsNullOrEmpty(objWoEntity.WODetailTotalAmount) ? "0" : objWoEntity.WODetailTotalAmount); 
                string[,] codes2 = new string[,]
                {
                    {"poheader_pono",  objCmnFunctions.GetSequenceNoFb("WO", "", objWoEntity.WORequestFor)},
                    {"poheader_date","sysdatetime()"},
                    {"poheader_raisor_gid",objCmnFunctions.GetLoginUserGid().ToString()},
                    {"poheader_requestfor", string.IsNullOrEmpty(objWoEntity.WORequestForGid.ToString())?"0":objWoEntity.WORequestForGid.ToString()},
                    {"poheader_vendor_gid", string.IsNullOrEmpty(objWoEntity.WOVendorGid.ToString())?"0":objWoEntity.WOVendorGid.ToString()},
                    {"poheader_vendor_note",string.IsNullOrEmpty(objWoEntity.WODescription)?"":objWoEntity.WODescription},
                    {"poheader_over_total",string.IsNullOrEmpty(poheaderamnt.ToString())?"0":poheaderamnt.ToString()},
                    {"poheader_ittype",string.IsNullOrEmpty(objWoEntity.WOITType)?"":objWoEntity.WOITType},
                    {"poheader_type","W"},
                    {"poheader_status","1"},
                    {"poheader_isclosed","N"},
                    {"poheader_iscancelled","N"},
                    {"poheader_termcond_gid",string.IsNullOrEmpty(objWoEntity.TermCondGid.ToString())?"0":objWoEntity.TermCondGid.ToString()},
                    {"poheader_add_termandcond",string.IsNullOrEmpty(objWoEntity.AdditionalTermCondContent)?"":objWoEntity.AdditionalTermCondContent},
                    {"poheader_currentapprovalstage","1"},
                    {"poheader_isremoved","N"}
                };
                string tbname = "fb_trn_tpoheader";
                string query_result = objCommonIUD.InsertCommon(codes2, tbname);

                if (query_result.ToLower() == "success")
                {
                  
                    getconnection();
                    cmd = new SqlCommand("pr_fb_trn_wonew", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getwoheadergid";
                    woheadergid = Convert.ToString(cmd.ExecuteScalar());

              string[,] codes = new string[,] 
                {
                    {"podetails_pohead_gid", string.IsNullOrEmpty(woheadergid)?"0":woheadergid},
                    {"podetails_prodservice_gid", string.IsNullOrEmpty(objWoEntity.WOProductGid.ToString())?"0":objWoEntity.WOProductGid.ToString()},
                    {"podetails_desc", string.IsNullOrEmpty(objWoEntity.WODetailDescription)?"":objWoEntity.WODetailDescription},
                    {"podetails_serv_month", string.IsNullOrEmpty(objWoEntity.WODetailServiceMonth)?"":objWoEntity.WODetailServiceMonth},
                    {"podetails_qty", string.IsNullOrEmpty(objWoEntity.WODetailQty)?"0":objWoEntity.WODetailQty},
                    {"podetails_unitprice",string.IsNullOrEmpty(objWoEntity.WODetailUnitAmount)?"0":objWoEntity.WODetailUnitAmount},
                    {"podetails_base_amt",string.IsNullOrEmpty(objWoEntity.WODetailTotalAmount)?"0":objWoEntity.WODetailTotalAmount},
                    {"podetails_total",string.IsNullOrEmpty(objWoEntity.WODetailTotalAmount)?"0":objWoEntity.WODetailTotalAmount},
                    {"podetails_cbfdet_gid",string.IsNullOrEmpty(objWoEntity.CBFDetailGid.ToString())?"0":objWoEntity.CBFDetailGid.ToString()},
                    {"podetails_isremoved","N"}
                };
                    string tbname1 = "fb_trn_tpodetails";
                    string query_result1 = objCommonIUD.InsertCommon(codes, tbname1);
                }

                result = Convert.ToInt32(string.IsNullOrEmpty(woheadergid) ? "0" : woheadergid);

                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@wogid", SqlDbType.Int).Value = result;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updatewototalamnt";
                cmd.ExecuteNonQuery();

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
        public DataSet GetWODetailsParent(int poheadergid = 0) 
        {
            DataSet ds = new DataSet(); 
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@wogid", SqlDbType.Int).Value = poheadergid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getwodetailsparent";
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
        public int AddParentWODetails1(WOEntity objWoEntity) 
        {
            try
            {
                string woheadergid = "";
                int result = 0;
                 decimal poheaderamnt = Convert.ToDecimal(string.IsNullOrEmpty(objWoEntity.WOUtilAmount) ? "0" : objWoEntity.WOUtilAmount) +
                                       Convert.ToDecimal(string.IsNullOrEmpty(objWoEntity.WODetailTotalAmount) ? "0" : objWoEntity.WODetailTotalAmount); 
                string[,] codes2 = new string[,]
                {
                    {"poheader_vendor_note",string.IsNullOrEmpty(objWoEntity.WODescription)?"":objWoEntity.WODescription},
                    {"poheader_over_total",string.IsNullOrEmpty(poheaderamnt.ToString())?"0":poheaderamnt.ToString()},
                    {"poheader_termcond_gid",string.IsNullOrEmpty(objWoEntity.TermCondGid.ToString())?"0":objWoEntity.TermCondGid.ToString()},
                    {"poheader_add_termandcond",string.IsNullOrEmpty(objWoEntity.AdditionalTermCondContent)?"":objWoEntity.AdditionalTermCondContent},
                    {"poheader_ittype",string.IsNullOrEmpty(objWoEntity.WOITType)?"":objWoEntity.WOITType}
                };
                string[,] whrcol = new string[,]
	            {
                    {"poheader_gid", string.IsNullOrEmpty(objWoEntity.WOGid.ToString())?"0":objWoEntity.WOGid.ToString()}
                };
              
                string tbname = "fb_trn_tpoheader";
                string query_result = objCommonIUD.UpdateCommon(codes2, whrcol, tbname);
                if (query_result.ToLower() == "success")
                {
                woheadergid = string.IsNullOrEmpty(objWoEntity.WOGid.ToString()) ? "0" : objWoEntity.WOGid.ToString();

                string[,] codes = new string[,] 
                {
                    {"podetails_pohead_gid", string.IsNullOrEmpty(woheadergid)?"0":woheadergid},
                    {"podetails_prodservice_gid", string.IsNullOrEmpty(objWoEntity.WOProductGid.ToString())?"0":objWoEntity.WOProductGid.ToString()},
                    {"podetails_desc", string.IsNullOrEmpty(objWoEntity.WODetailDescription)?"":objWoEntity.WODetailDescription},
                    {"podetails_serv_month", string.IsNullOrEmpty(objWoEntity.WODetailServiceMonth)?"":objWoEntity.WODetailServiceMonth},
                    {"podetails_qty", string.IsNullOrEmpty(objWoEntity.WODetailQty)?"0":objWoEntity.WODetailQty},
                    {"podetails_unitprice",string.IsNullOrEmpty(objWoEntity.WODetailUnitAmount)?"0":objWoEntity.WODetailUnitAmount},
                    {"podetails_base_amt",string.IsNullOrEmpty(objWoEntity.WODetailTotalAmount)?"0":objWoEntity.WODetailTotalAmount},
                    {"podetails_total",string.IsNullOrEmpty(objWoEntity.WODetailTotalAmount)?"0":objWoEntity.WODetailTotalAmount},
                    {"podetails_cbfdet_gid",string.IsNullOrEmpty(objWoEntity.CBFDetailGid.ToString())?"0":objWoEntity.CBFDetailGid.ToString()},
                    {"podetails_uom_gid", string.IsNullOrEmpty(objWoEntity.WODetailUOM)?"0":objWoEntity.WODetailUOM},
                    {"podetails_tax1",string.IsNullOrEmpty(objWoEntity.WODetailTaxAmount)?"0":objWoEntity.WODetailTaxAmount},
                    {"podetails_txt_percent",string.IsNullOrEmpty(objWoEntity.WODetailTax)?"0":objWoEntity.WODetailTax},
                    {"podetails_tax3",string.IsNullOrEmpty(objWoEntity.WODetailOthers)?"0":objWoEntity.WODetailOthers},
                    {"podetails_discount",string.IsNullOrEmpty(objWoEntity.WODetailDiscount)?"0":objWoEntity.WODetailDiscount},
                    {"podetails_isremoved","N"}
                };
                    string tbname1 = "fb_trn_tpodetails";
                    string query_result1 = objCommonIUD.InsertCommon(codes, tbname1);
                }
                result = Convert.ToInt32(string.IsNullOrEmpty(woheadergid) ? "0" : woheadergid);

                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@wogid", SqlDbType.Int).Value = result;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updatewototalamnt";
                cmd.ExecuteNonQuery();

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
        public DataSet GetWODetailsParentForEdit(int wogid = 0)  
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@wodetailgid", SqlDbType.Int).Value = wogid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getwodetailbyid";
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
        public int UpdateParentWODetails(WOEntity objWoEntity)
        {
            try  
            {
                string woheadergid = "";
                int result = 0;
                string[,] codes2 = new string[,]
                {
                    {"poheader_vendor_note",string.IsNullOrEmpty(objWoEntity.WODescription)?"":objWoEntity.WODescription},
                    {"poheader_over_total",string.IsNullOrEmpty(objWoEntity.WOAmount)?"0":objWoEntity.WOAmount},
                    {"poheader_termcond_gid",string.IsNullOrEmpty(objWoEntity.TermCondGid.ToString())?"0":objWoEntity.TermCondGid.ToString()},
                    {"poheader_add_termandcond",string.IsNullOrEmpty(objWoEntity.AdditionalTermCondContent)?"":objWoEntity.AdditionalTermCondContent},
                    {"poheader_ittype",string.IsNullOrEmpty(objWoEntity.WOITType)?"":objWoEntity.WOITType}
                };
                string[,] whrcol = new string[,]
	            {
                    {"poheader_gid", string.IsNullOrEmpty(objWoEntity.WOGid.ToString())?"0":objWoEntity.WOGid.ToString()}
                };

                string tblname = "fb_trn_tpoheader";
                string query_result = objCommonIUD.UpdateCommon(codes2, whrcol, tblname);
                if (query_result.ToLower() == "success") 
                {
                    woheadergid = string.IsNullOrEmpty(objWoEntity.WOGid.ToString()) ? "0" : objWoEntity.WOGid.ToString();

                string[,] codes = new string[,] 
                {
                    {"podetails_prodservice_gid", string.IsNullOrEmpty(objWoEntity.WOProductGid.ToString())?"0":objWoEntity.WOProductGid.ToString()},
                    {"podetails_desc", string.IsNullOrEmpty(objWoEntity.WODetailDescription)?"":objWoEntity.WODetailDescription},
                    {"podetails_serv_month", string.IsNullOrEmpty(objWoEntity.WODetailServiceMonth)?"":objWoEntity.WODetailServiceMonth},
                    {"podetails_qty", string.IsNullOrEmpty(objWoEntity.WODetailQty)?"0":objWoEntity.WODetailQty},
                    {"podetails_unitprice",string.IsNullOrEmpty(objWoEntity.WODetailUnitAmount)?"0":objWoEntity.WODetailUnitAmount},
                    {"podetails_base_amt",string.IsNullOrEmpty(objWoEntity.WODetailTotalAmount)?"0":objWoEntity.WODetailTotalAmount},
                    {"podetails_total",string.IsNullOrEmpty(objWoEntity.WODetailTotalAmount)?"0":objWoEntity.WODetailTotalAmount},
                    {"podetails_uom_gid", string.IsNullOrEmpty(objWoEntity.WODetailUOM)?"0":objWoEntity.WODetailUOM},
                    {"podetails_tax1",string.IsNullOrEmpty(objWoEntity.WODetailTaxAmount)?"0":objWoEntity.WODetailTaxAmount},
                    {"podetails_txt_percent",string.IsNullOrEmpty(objWoEntity.WODetailTax)?"0":objWoEntity.WODetailTax},
                    {"podetails_tax3",string.IsNullOrEmpty(objWoEntity.WODetailOthers)?"0":objWoEntity.WODetailOthers},
                    {"podetails_discount",string.IsNullOrEmpty(objWoEntity.WODetailDiscount)?"0":objWoEntity.WODetailDiscount}
                };
                string[,] whrcol1 = new string[,]
	            {
                    {"podetails_gid", string.IsNullOrEmpty(objWoEntity.WODetailParentGid.ToString())?"0":objWoEntity.WODetailParentGid.ToString()}
                };
                    string tblname1 = "fb_trn_tpodetails";
                    string query_result1 = objCommonIUD.UpdateCommon(codes, whrcol1, tblname1);
                }
                result = Convert.ToInt32(string.IsNullOrEmpty(woheadergid) ? "0" : woheadergid);

                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@wogid", SqlDbType.Int).Value = result;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updatewototalamnt";
                cmd.ExecuteNonQuery();

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
        public DataSet GetProductGroup(string flag = null) 
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prodserviceflag", SqlDbType.Char).Value = string.IsNullOrEmpty(flag) ? "" : flag;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getproductgroup";
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
        public DataSet GetProductByGroup(string flag = null, int ProdGroupGid = 0)
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prodserviceflag", SqlDbType.Char).Value = string.IsNullOrEmpty(flag) ? "" : flag;
                cmd.Parameters.Add("@prodgid", SqlDbType.Int).Value = ProdGroupGid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getproductbygroup";
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
        public DataSet GetWODetailsChild(int wodetailsgid = 0)
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@wodetailgid", SqlDbType.Int).Value = wodetailsgid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getwochilddetailNew";
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

        public DataSet GetProductDesc(int prodgid = 0) 
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prodgid", SqlDbType.Int).Value = prodgid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getproductdesc";
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
        public int AddChildWODetails(WOEntity objWoEntity)
        {
            try 
            {
              string woheadergid = string.IsNullOrEmpty(objWoEntity.WODetailParentGid.ToString()) ? "0" : objWoEntity.WODetailParentGid.ToString();

              string[,] codes = new string[,] 
                {
                    {"podetails_pohead_gid", string.IsNullOrEmpty(objWoEntity.WOGid.ToString())?"0":objWoEntity.WOGid.ToString()},
                    {"podetails_prodservice_gid", string.IsNullOrEmpty(objWoEntity.WOProductGid.ToString())?"0":objWoEntity.WOProductGid.ToString()},
                    {"podetails_desc", string.IsNullOrEmpty(objWoEntity.WODetailDescription)?"":objWoEntity.WODetailDescription},
                    {"podetails_serv_month", string.IsNullOrEmpty(objWoEntity.WODetailServiceMonth)?"":objWoEntity.WODetailServiceMonth},
                    {"podetails_qty", string.IsNullOrEmpty(objWoEntity.WODetailQty)?"0":objWoEntity.WODetailQty},
                    {"podetails_perce", string.IsNullOrEmpty(objWoEntity.WODetailPercentage)?"0":objWoEntity.WODetailPercentage},
                    {"podetails_unitprice",string.IsNullOrEmpty(objWoEntity.WODetailUnitAmount)?"0":objWoEntity.WODetailUnitAmount},
                    {"podetails_base_amt",string.IsNullOrEmpty(objWoEntity.WODetailTotalAmount)?"0":objWoEntity.WODetailTotalAmount},
                    {"podetails_total",string.IsNullOrEmpty(objWoEntity.WODetailTotalAmount)?"0":objWoEntity.WODetailTotalAmount},
                    {"podetails_cbfdet_gid",string.IsNullOrEmpty(objWoEntity.CBFDetailGid.ToString())?"0":objWoEntity.CBFDetailGid.ToString()},
                    {"podetails_parent_gid",string.IsNullOrEmpty(objWoEntity.WODetailParentGid.ToString()) ? "0" : objWoEntity.WODetailParentGid.ToString()},
                    {"podetails_uom_gid", string.IsNullOrEmpty(objWoEntity.WODetailUOM)?"0":objWoEntity.WODetailUOM},
                    {"podetails_tax1",string.IsNullOrEmpty(objWoEntity.WODetailTaxAmount)?"0":objWoEntity.WODetailTaxAmount},
                    {"podetails_txt_percent",string.IsNullOrEmpty(objWoEntity.WODetailTax)?"0":objWoEntity.WODetailTax},
                    {"podetails_tax3",string.IsNullOrEmpty(objWoEntity.WODetailOthers)?"0":objWoEntity.WODetailOthers},
                    {"podetails_discount",string.IsNullOrEmpty(objWoEntity.WODetailDiscount)?"0":objWoEntity.WODetailDiscount},
                    {"podetails_isremoved","N"}
                };
             
               string tbname1 = "fb_trn_tpodetails";
               string query_result1 = objCommonIUD.InsertCommon(codes, tbname1);
               int result = Convert.ToInt32(string.IsNullOrEmpty(woheadergid) ? "0" : woheadergid);

               getconnection();
               cmd = new SqlCommand("pr_fb_trn_wonew", con);
               cmd.CommandType = CommandType.StoredProcedure;
               cmd.Parameters.Add("@wodetailgid", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(woheadergid) ? "0" : woheadergid);
               cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updatechildshipment";
               cmd.ExecuteNonQuery();
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
        public int UpdateChildWODetails(WOEntity objWoEntity)
        { 
            try
            {
                string woheadergid = string.IsNullOrEmpty(objWoEntity.WODetailParentGid.ToString()) ? "0" : objWoEntity.WODetailParentGid.ToString();

                string[,] codes = new string[,] 
                {
                    {"podetails_prodservice_gid", string.IsNullOrEmpty(objWoEntity.WOProductGid.ToString())?"0":objWoEntity.WOProductGid.ToString()},
                    {"podetails_desc", string.IsNullOrEmpty(objWoEntity.WODetailDescription)?"":objWoEntity.WODetailDescription},
                    {"podetails_serv_month", string.IsNullOrEmpty(objWoEntity.WODetailServiceMonth)?"":objWoEntity.WODetailServiceMonth},
                    {"podetails_qty", string.IsNullOrEmpty(objWoEntity.WODetailQty)?"0":objWoEntity.WODetailQty},
                    {"podetails_perce", string.IsNullOrEmpty(objWoEntity.WODetailPercentage)?"0":objWoEntity.WODetailPercentage},
                    {"podetails_unitprice",string.IsNullOrEmpty(objWoEntity.WODetailUnitAmount)?"0":objWoEntity.WODetailUnitAmount},
                    {"podetails_base_amt",string.IsNullOrEmpty(objWoEntity.WODetailTotalAmount)?"0":objWoEntity.WODetailTotalAmount},
                    {"podetails_total",string.IsNullOrEmpty(objWoEntity.WODetailTotalAmount)?"0":objWoEntity.WODetailTotalAmount},
                    {"podetails_uom_gid", string.IsNullOrEmpty(objWoEntity.WODetailUOM)?"0":objWoEntity.WODetailUOM},
                    {"podetails_tax1",string.IsNullOrEmpty(objWoEntity.WODetailTaxAmount)?"0":objWoEntity.WODetailTaxAmount},
                    {"podetails_txt_percent",string.IsNullOrEmpty(objWoEntity.WODetailTax)?"0":objWoEntity.WODetailTax},
                    {"podetails_tax3",string.IsNullOrEmpty(objWoEntity.WODetailOthers)?"0":objWoEntity.WODetailOthers},
                    {"podetails_discount",string.IsNullOrEmpty(objWoEntity.WODetailDiscount)?"0":objWoEntity.WODetailDiscount}
                };
                string[,] whrcol = new string[,] 
	            {
                    {"podetails_gid", string.IsNullOrEmpty(objWoEntity.WODetailGid.ToString())?"0":objWoEntity.WODetailGid.ToString()}
                };
                string tblname = "fb_trn_tpodetails";
                string query_result1 = objCommonIUD.UpdateCommon(codes, whrcol, tblname);

                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@wodetailgid", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(objWoEntity.WODetailGid.ToString()) ? "0" : objWoEntity.WODetailGid.ToString());
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updateshipmentqty1";
                cmd.ExecuteNonQuery();

                int result = Convert.ToInt32(string.IsNullOrEmpty(woheadergid) ? "0" : woheadergid);
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
        public DataSet GetBranchAutoComplete(string searchtext = null)
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SearchText", SqlDbType.VarChar).Value = searchtext;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getbranchautocomplete";
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
        public DataSet GetEmployeeComplete(string searchtext = null) 
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SearchText", SqlDbType.VarChar).Value = searchtext;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getemployeeautocomplete";
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
        public int ADDWOShipmentDetails(WOEntity objWoEntity) 
        {
            try
            {
                string wodetailgid = string.IsNullOrEmpty(objWoEntity.WODetailGid.ToString()) ? "0" : objWoEntity.WODetailGid.ToString();
                 
                string[,] codes = new string[,] 
                {
                    {"poshipment_podet_gid", string.IsNullOrEmpty(objWoEntity.WODetailGid.ToString())?"0":objWoEntity.WODetailGid.ToString()},
                    {"poshipment_type_gid", string.IsNullOrEmpty(objWoEntity.WOShipmentTypeGid.ToString())?"0":objWoEntity.WOShipmentTypeGid.ToString()},
                    {"poshipment_branch_gid", string.IsNullOrEmpty(objWoEntity.WOBranchGid.ToString())?"0":objWoEntity.WOBranchGid.ToString()},
                    {"poshipment_empgid", string.IsNullOrEmpty(objWoEntity.WOBranchInchargeGid.ToString())?"0":objWoEntity.WOBranchInchargeGid.ToString()},
                    {"poshipment_qty", string.IsNullOrEmpty(objWoEntity.WOShipmentQty)?"0":objWoEntity.WOShipmentQty},
                    {"poshipment_isremoved","N"}
                };

                string tbname1 = "fb_trn_tposhipment";
                string query_result1 = objCommonIUD.InsertCommon(codes, tbname1);
                int result = Convert.ToInt32(string.IsNullOrEmpty(wodetailgid) ? "0" : wodetailgid);
                if (Convert.ToInt32(objWoEntity.WOShipmentQty) == 1)
                {
                    getconnection();
                    cmd = new SqlCommand("pr_fb_trn_wonew", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@wodetailgid", SqlDbType.Int).Value = result;
                    cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(objWoEntity.WOBranchInchargeGid.ToString()) ? "0" : objWoEntity.WOBranchInchargeGid.ToString());
                    cmd.Parameters.Add("@branchgid", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(objWoEntity.WOBranchGid.ToString()) ? "0" : objWoEntity.WOBranchGid.ToString());
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updateshipmentqty";
                    cmd.ExecuteNonQuery();
                }
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
        public DataSet GetWOShipmentDetails(int wodetailsgid = 0, int Vendorid=0)
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@wodetailgid", SqlDbType.Int).Value = wodetailsgid;
                cmd.Parameters.Add("@VendorId", SqlDbType.Int).Value = Vendorid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getwoshipmentdetails";
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
        public DataSet DeleteWoParentDetails(int refgid = 0,string deletefor = null)
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@wodetailgid", SqlDbType.Int).Value = refgid;
                cmd.Parameters.Add("@DeleteFor", SqlDbType.VarChar).Value = deletefor;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "deletewodetails";
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
        public string SubmitRaiser(WOEntity objWoEntity) 
        {
            try
            {
                string errormsg = "";
                int result = 0;
                result = Convert.ToInt32(string.IsNullOrEmpty(objWoEntity.WOGid.ToString()) ? "0" : objWoEntity.WOGid.ToString());

                //Update Queue
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@wogid", SqlDbType.Int).Value = result;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "SubmitRaiserWO";
                cmd.Parameters.Add("@errormsg", SqlDbType.VarChar, 150).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@clear", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                var errormsgs = Convert.ToString(cmd.Parameters["@errormsg"].Value);
                var clear = Convert.ToString(cmd.Parameters["@clear"].Value);
                errormsg = errormsgs.ToString();
                if (clear == "1")
                {
                    
                string[,] codes2 = new string[,]
                {
                    {"poheader_vendor_note",string.IsNullOrEmpty(objWoEntity.WODescription)?"":objWoEntity.WODescription},
                  //  {"poheader_over_total",string.IsNullOrEmpty(objWoEntity.WOAmount)?"0":objWoEntity.WOAmount},
                    {"poheader_termcond_gid",string.IsNullOrEmpty(objWoEntity.TermCondGid.ToString())?"0":objWoEntity.TermCondGid.ToString()},
                    {"poheader_add_termandcond",string.IsNullOrEmpty(objWoEntity.AdditionalTermCondContent)?"":objWoEntity.AdditionalTermCondContent},
                    {"poheader_ittype",string.IsNullOrEmpty(objWoEntity.WOITType)?"":objWoEntity.WOITType},
                    {"poheader_vendor_gid", string.IsNullOrEmpty(objWoEntity.WOVendorGid.ToString())?"0":objWoEntity.WOVendorGid.ToString()},
                    {"poheader_status","2"},
                    {"Poheader_VendorContact_gid",string.IsNullOrEmpty(objWoEntity.vendAddressId.ToString())?"0":objWoEntity.vendAddressId.ToString()}
                };
                string[,] whrcol = new string[,]
	            {
                    {"poheader_gid", string.IsNullOrEmpty(objWoEntity.WOGid.ToString())?"0":objWoEntity.WOGid.ToString()}
                };
                string tnameq = "fb_trn_tpoheader";
                string insertcommendq = objCommonIUD.UpdateCommon(codes2, whrcol, tnameq);

               // errormsgs = 
              //  errormsg = errormsgs.ToString(); 
                errormsg = clear;
                  
                }
               else
                {
                    errormsg = errormsgs.ToString();
                }
                return errormsg;
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
        public int SaveAsDraft(WOEntity objWoEntity)
        { 
            try
            {
                int result = 0;
                string[,] codes2 = new string[,]
                {
                    {"poheader_vendor_note",string.IsNullOrEmpty(objWoEntity.WODescription)?"":objWoEntity.WODescription},
                   // {"poheader_over_total",string.IsNullOrEmpty(objWoEntity.WOAmount)?"0":objWoEntity.WOAmount},
                    {"poheader_termcond_gid",string.IsNullOrEmpty(objWoEntity.TermCondGid.ToString())?"0":objWoEntity.TermCondGid.ToString()},
                    {"poheader_add_termandcond",string.IsNullOrEmpty(objWoEntity.AdditionalTermCondContent)?"":objWoEntity.AdditionalTermCondContent},
                    {"poheader_ittype",string.IsNullOrEmpty(objWoEntity.WOITType)?"":objWoEntity.WOITType},
                    {"poheader_vendor_gid", string.IsNullOrEmpty(objWoEntity.WOVendorGid.ToString())?"0":objWoEntity.WOVendorGid.ToString()},
                    {"poheader_status","1"},
                };
                string[,] whrcol = new string[,]
	            {
                    {"poheader_gid", string.IsNullOrEmpty(objWoEntity.WOGid.ToString())?"0":objWoEntity.WOGid.ToString()}
                };
                string tnameq = "fb_trn_tpoheader";
                string insertcommendq = objCommonIUD.UpdateCommon(codes2, whrcol, tnameq);
                result = Convert.ToInt32(string.IsNullOrEmpty(objWoEntity.WOGid.ToString()) ? "0" : objWoEntity.WOGid.ToString());

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
        public string GetDelmatemployeeForWo(WOEntity ObjWOEntity) 
        {
            string resultone = string.Empty;
            string queue_toG = string.Empty;
            string queue_toD = string.Empty;
            string queue_totype = string.Empty;
            string queue_tobranch = string.Empty;
            string queue_todept = string.Empty;
            string delmattype = string.Empty;
            string delmatgid = string.Empty;
            string Expenses = string.Empty;
            string queuengid = string.Empty;
            string queuento = string.Empty;
            int worefFlag = 0;
            string empgid = string.Empty;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_GetWOqueueapp", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@wogid", ObjWOEntity.WOGid);
                da = new SqlDataAdapter(cmd);
                DataTable dtcmd = new DataTable();
                da.Fill(dtcmd);
                if (dtcmd.Rows.Count > 0)
                {
                    queuengid = dtcmd.Rows[0]["queuegid"].ToString();
                    //queuento = dtcmd.Rows[0]["queueto"].ToString();.
                    queuento = objCmnFunctions.GetLoginUserGid().ToString();
                    Hashtable Togetlist = new Hashtable();
                    Togetlist = (Hashtable)HttpContext.Current.Session["Queue_delegateslist"];

                    if (Togetlist.ContainsKey(queuengid))
                    {
                        empgid = Togetlist[queuengid].ToString();
                    }

                    if (empgid == "")
                    {
                        empgid = queuento.ToString();
                    }
                    
                    cmd = new SqlCommand("pr_podelmat", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@poheader_gid", ObjWOEntity.WOGid);
                    cmd.Parameters.AddWithValue("@queue_gid", queuengid);
                    //cmd.Parameters.AddWithValue("@po_approver_gid", queuento);
                    cmd.Parameters.AddWithValue("@po_approver_gid", Convert.ToInt32(empgid.ToString()));

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
                        if (ObjWOEntity.WOApprovedBy != "delmat")
                        {
                            WoCheckerApprove(ObjWOEntity);
                        }
                        if (finalfalg == "Y")
                        {
                            string[,] codes1 = new string[,]            
                               {
                                   {"poheader_status","5"},

                               };
                            string[,] whrcol = new string[,]
                                {
                          
                                     {"poheader_gid",ObjWOEntity.WOGid.ToString()}

                                };
                            string tname1 = "fb_trn_tpoheader";
                            string updatequery = objCommonIUD.UpdateCommon(codes1, whrcol, tname1);
                            string[,] codesq = new string[,]
                                    {
                                         {"queue_action_date","sysdatetime()"},
                                         //{"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString() },
                                         {"queue_action_by",empgid.ToString() },
                                         {"queue_delegation_gid",((Convert.ToInt32(empgid.ToString())== Convert.ToInt32(queuento.ToString()))? 0: objCmnFunctions.GetLoginUserGid()).ToString() },
                                         {"queue_action_remark",Convert.ToString(string.IsNullOrEmpty(ObjWOEntity.WOApprovalRemarks) ? "" :ObjWOEntity.WOApprovalRemarks)},
                                         {"queue_action_status","1" },
                                         {"queue_isremoved","Y" },
                                         {"queue_delmat_flag","F"}
                                     };
                            string[,] whreq = new string[,]
                                    {
                                        {"queue_ref_gid",ObjWOEntity.WOGid.ToString()},
                                        {"queue_ref_flag",worefFlag.ToString()},
                                        {"queue_isremoved","N" }
                                   };
                            string tnameq = "iem_trn_tqueue";
                            string insertcommendq = objCommonIUD.UpdateCommon(codesq, whreq, tnameq);
                            resultone = insertcommendq;


                        }

                        else
                        {
                            if (ObjWOEntity.WOApprovedBy == "delmat")
                            {
                                string[,] codesq = new string[,]
                                    {
                                         {"queue_action_date","sysdatetime()"},
                                         //{"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString() },
                                         {"queue_action_by",empgid.ToString() },
                                         {"queue_delegation_gid",((Convert.ToInt32(empgid.ToString())== Convert.ToInt32(queuento.ToString()))? 0: objCmnFunctions.GetLoginUserGid()).ToString()},
                                         {"queue_action_remark",Convert.ToString(string.IsNullOrEmpty(ObjWOEntity.WOApprovalRemarks) ? "" :ObjWOEntity.WOApprovalRemarks)},
                                         {"queue_action_status","1" },
                                         {"queue_isremoved","Y" }
                                     };
                                string[,] whreq = new string[,]
                                    {
                                     
                                        {"queue_ref_gid",ObjWOEntity.WOGid.ToString()},
                                        {"queue_ref_flag",worefFlag.ToString()},
                                        {"queue_isremoved","N" }
                                   };
                                string tnameq = "iem_trn_tqueue";
                                string insertcommendq = objCommonIUD.UpdateCommon(codesq, whreq, tnameq);
                            }
                            string[,] codesIN = new string[,]
                                      {
                                           {"queue_date","sysdatetime()"},
                                           {"queue_ref_flag", "10"},
                                           {"queue_ref_gid",ObjWOEntity.WOGid.ToString() },
                                           {"queue_ref_status", "0"},
                                           {"queue_from",objCmnFunctions.GetLoginUserGid().ToString() } ,
                                           {"queue_to_type", nqueuety.ToString()},
                                           {"queue_to", nqueuet.ToString()},
                                           {"queue_action_for", "A"},
                                           {"queue_prev_gid",Convert.ToString(GetWOQueueGid(ObjWOEntity.WOGid.ToString()))},
                                           {"queue_delmat_flag","D"}
                                     };
                            string tnameIN = "iem_trn_tqueue";

                            string insertcommendecf = objCommonIUD.InsertCommon(codesIN, tnameIN);
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
        public string WoCheckerApprove(WOEntity objWOEntity) 
        {
            try
            {
                string result = string.Empty;
                int b = 0;
                int queue_prevgid = 0;
                getconnection();
              
                string[,] wherecond = new string[,]
	            {
                    {"poheader_gid", objWOEntity.WOGid.ToString ()}
                };
                string[,] column = new string[,]
	            {
                    {"poheader_status", "4"}
                };
                string tblname = "fb_trn_tpoheader";
                result = objCommonIUD.UpdateCommon(column, wherecond, tblname);
                if (result == "Success")
                {
                int worefFlag = Convert.ToInt32(ConfigurationManager.AppSettings["worefFlag"].ToString());
                string[,] wherecol = new string[,]
	            {
                    {"queue_ref_gid", objWOEntity.WOGid.ToString ()},
                    {"queue_ref_flag",worefFlag.ToString()},
                    {"queue_isremoved","N"}
                };
                string[,] columnNames = new string[,]
	            {
                    {"queue_action_date", "sysdatetime()"},
                    {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                    {"queue_action_status","1"},
                    {"queue_action_remark",Convert.ToString(string.IsNullOrEmpty(objWOEntity.WOApprovalRemarks) ? string.Empty : objWOEntity.WOApprovalRemarks)},
                    {"queue_prev_gid",queue_prevgid.ToString()},
                    {"queue_isremoved","Y"}
                };
                    string tbl = "iem_trn_tqueue";
                    result = objCommonIUD.UpdateCommon(columnNames, wherecol, tbl);
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
        public int GetWOQueueGid(string woheadGid = "0")
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
        public string RejectWO(WOEntity objWOEntity)
        {
            try
            {
                int worefFlag = Convert.ToInt32(ConfigurationManager.AppSettings["worefFlag"].ToString());
                string[,] codw = new string[,]
                {
                    {"queue_action_for","R"},
                    {"queue_action_status","2"},
                    {"queue_isremoved","Y"},
                    {"queue_action_by",objCmnFunctions.GetLoginUserGid().ToString()},
                    {"queue_action_date","sysdatetime()"},
                    {"queue_action_remark",(string.IsNullOrEmpty(objWOEntity.WOApprovalRemarks)?"":objWOEntity.WOApprovalRemarks)}
                };
                string[,] codewhe = new string[,]
                {
                    {"queue_ref_gid",objWOEntity.WOGid.ToString()},
                    {"queue_ref_flag",worefFlag.ToString()},
                    {"queue_isremoved","N"}
                };

                string tblname = "iem_trn_tqueue";
                string updatecon = objCommonIUD.UpdateCommon(codw, codewhe, tblname);

                string[,] codw2 = new string[,]
                {
                    {"poheader_status","6"}
                };
                string[,] codewhe2 = new string[,]
                {
                    {"poheader_gid",objWOEntity.WOGid.ToString()},
                    {"poheader_type","W"}
                };

                string tblname2 = "fb_trn_tpoheader";
                string updatecon2 = objCommonIUD.UpdateCommon(codw2, codewhe2, tblname2);

                return "1";
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
        public DataSet GetSCNInwardDetails(int WOGid = 0)
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@wodetailgid", SqlDbType.Int).Value = WOGid;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getscninwardheader";
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
        public DataSet GetWOSummary()
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                int empid = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getwosummaryforscn";
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
        public string AddInwardDetails(SCNInward objSCNInward)
        {
            try
            {
                string scninwardgid = "";
                string SCNInwardNumber = objCmnFunctions.GetSequenceNo("SCN");
                string[,] codes2 = new string[,]
                { 
                    {"grninwrdheader_refno", SCNInwardNumber},
                    {"grninwrdheader_grndatetime","sysdatetime()"},
                    {"grninwrdheader_rasiergid",objCmnFunctions.GetLoginUserGid().ToString()},
                    {"grninwardheader_poheader", string.IsNullOrEmpty(objSCNInward.WOGid.ToString())?"0":objSCNInward.WOGid.ToString()},
                    {"grninwrdheader_dcno", string.IsNullOrEmpty(objSCNInward.SCNDocNumber.ToString())?"":objSCNInward.SCNDocNumber.ToString()},
                    {"grninwrdheader_invoiceno",string.IsNullOrEmpty(objSCNInward.SCNInvoiceNumber)?"":objSCNInward.SCNInvoiceNumber},
                    {"grninwrdheader_remarks",string.IsNullOrEmpty(objSCNInward.SCNDescription)?"0":objSCNInward.SCNDescription},
                    {"grninwrdheader_branch_type","D"},
                    {"grninwrdheader_status","9"},
                    {"grninwrdheader_isremoved","N"}
                };
                string tbname = "fb_trn_tgrninwrdheader";
                string query_result = objCommonIUD.InsertCommon(codes2, tbname);

                if (query_result.ToLower() == "success")
                {
                    getconnection();
                    cmd = new SqlCommand("pr_fb_trn_wonew", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                    cmd.Parameters.Add("@wodetailgid", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(objSCNInward.WOGid.ToString()) ? "0" : objSCNInward.WOGid.ToString());
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getgrnheadergid";
                    scninwardgid = Convert.ToString(cmd.ExecuteScalar());
                    for (int i = 0; i < objSCNInward.SelectedItems.Length; i++)
                    {
                        string[,] codes = new string[,] 
                        {
                            {"grninwrddet_grnreleforpo_gid", objSCNInward.SelectedItems[i].ToString()},
                            {"grninwrddet_reced_qty", objSCNInward.ReceivedQty[i].ToString()},
                            {"grninwrddet_reced_date", "sysdatetime()"},
                            {"grninwrddet_grninwrdhead_gid", string.IsNullOrEmpty(scninwardgid)?"0":scninwardgid},
                            {"grninwrddet_isremoved","N"}
                        };
                        string tbname1 = "fb_trn_tgrninwrddet";
                        string query_result1 = objCommonIUD.InsertCommon(codes, tbname1);

                        if(query_result1 =="success"){
                            getconnection();
                            cmd = new SqlCommand("pr_fb_trn_wonew", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@wodetailgid", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(objSCNInward.SelectedItems[i].ToString()) ? "0" : objSCNInward.SelectedItems[i].ToString());
                            cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updatebalanceqty";
                            cmd.ExecuteNonQuery();
                        }
                    }

                }
                return SCNInwardNumber;
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
        public List<AuditTrailWO> PopulateAuditTrail(int refgid = 0,string refname = null)
        {
            List<AuditTrailWO> objDashBoard = new List<AuditTrailWO>();
            try
            {
                AuditTrailWO objModel;
                int liRaiserGid = 0;
                int liCnt = 0;
                getconnection();
                DataTable dt = new DataTable();
                DataTable dtr = new DataTable();
                cmd = new SqlCommand("pr_fb_trn_audittrail", con);
                cmd.Parameters.AddWithValue("@gid", refgid);
                cmd.Parameters.AddWithValue("@refname", refname);
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
                                objModel = new AuditTrailWO();
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

                            objModel = new AuditTrailWO();
                            objModel.queue_gid = Convert.ToInt32(dt.Rows[i]["queue_gid"].ToString());
                            objModel.action_status = Convert.ToString(dt.Rows[i]["queue_action_status_name"].ToString());
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
                                string isfinalapprover = CheckFinalApproverTag(refgid, refflagnew, objModel.queue_gid);
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
        public string ChkWOShipment(int wodetgid = 0)
        {
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@wodetailgid", SqlDbType.Int).Value = wodetgid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "chkwoshipment";
                string result = Convert.ToString(cmd.ExecuteScalar());
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "404";
            }
            finally
            {
                con.Close();
            }
        }
        public DataSet ValidateWOShipment(int wogid = 0)
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@wogid", SqlDbType.Int).Value = wogid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "validateshipment";
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
        public DataSet GetVendorNameAutoComplete(string searchtext = null)
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@SearchText", SqlDbType.VarChar).Value = searchtext;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getvendorautocomplete";
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
        public DataSet GetTermCondContent(int termsandcondgid = 0)
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prodgid", SqlDbType.Int).Value = termsandcondgid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "gettermsandcondbyid";
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
        public int DeleteWo(int wogid = 0)
        {
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@wogid", SqlDbType.Int).Value = wogid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "deletewo";
                cmd.ExecuteNonQuery();
                return wogid;
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
        public DataSet GetSCNInwardDetailsView(int scngid = 0) 
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@wogid", SqlDbType.Int).Value = scngid;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "viewscn";
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
        public int InsertWOHeader(int cbfdetgid = 0) 
        {
            try
            {
                DataTable dt=new DataTable();
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@cbfdetailsgid", SqlDbType.Int).Value = cbfdetgid;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getcbfdetailsforwoheader";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                string woheadergid = "";
                int result = 0;
                string[,] codes2 = new string[,]
                {
                    {"poheader_pono",  objCmnFunctions.GetSequenceNoFb("WO", "", dt.Rows[0]["WORequestFor"].ToString())},
                    {"poheader_date","sysdatetime()"},
                    {"poheader_raisor_gid",objCmnFunctions.GetLoginUserGid().ToString()},
                    {"poheader_requestfor", string.IsNullOrEmpty(dt.Rows[0]["WORequestForGid"].ToString())?"0":dt.Rows[0]["WORequestForGid"].ToString()},
                    {"poheader_vendor_gid", string.IsNullOrEmpty(dt.Rows[0]["WOVendorGid"].ToString())?"0":dt.Rows[0]["WOVendorGid"].ToString()},
                    {"poheader_vendor_note",""},
                    {"poheader_over_total","0"},
                    {"poheader_ittype",""},
                    {"poheader_type","W"},
                    {"poheader_status","1"},
                    {"poheader_isclosed","N"},
                    {"poheader_iscancelled","N"},
                    {"poheader_termcond_gid",string.IsNullOrEmpty(dt.Rows[0]["TermCondGid"].ToString())?"0":dt.Rows[0]["TermCondGid"].ToString()},
                    {"poheader_add_termandcond",""},
                    {"poheader_currentapprovalstage","1"},
                    {"poheader_isremoved","N"},
                    {"poheader_version","0"}
                };
                string tbname = "fb_trn_tpoheader";
                string query_result = objCommonIUD.InsertCommon(codes2, tbname);

                if (query_result.ToLower() == "success")
                {

                    getconnection();
                    cmd = new SqlCommand("pr_fb_trn_wonew", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getwoheadergid";
                    woheadergid = Convert.ToString(cmd.ExecuteScalar());
                }

                result = Convert.ToInt32(string.IsNullOrEmpty(woheadergid) ? "0" : woheadergid);
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
        public void InsertWODetails(int cbfdetgid = 0, int wogid = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@cbfdetailsgid", SqlDbType.Int).Value = cbfdetgid;
                cmd.Parameters.Add("@wogid", SqlDbType.Int).Value = wogid;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getcbfdetailsforwodetails";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                 string[,] codes = new string[,] 
                {
                    {"podetails_pohead_gid", string.IsNullOrEmpty(wogid.ToString())?"0":wogid.ToString()},
                    {"podetails_prodservice_gid", string.IsNullOrEmpty(dt.Rows[0]["WOProductGid"].ToString())?"0":dt.Rows[0]["WOProductGid"].ToString()},
                    {"podetails_desc",  string.IsNullOrEmpty(dt.Rows[0]["CBFDescription"].ToString())?"":dt.Rows[0]["CBFDescription"].ToString()},
                    {"podetails_serv_month", ""},
                    {"podetails_qty",  string.IsNullOrEmpty(dt.Rows[0]["CBFQuantity"].ToString())?"0":dt.Rows[0]["CBFQuantity"].ToString()},
                    {"podetails_uom_gid",  string.IsNullOrEmpty(dt.Rows[0]["CBFUOM"].ToString())?"0":dt.Rows[0]["CBFUOM"].ToString()},
                    {"podetails_unitprice", string.IsNullOrEmpty(dt.Rows[0]["CBFUnitPrice"].ToString())?"0":dt.Rows[0]["CBFUnitPrice"].ToString()},
                    {"podetails_base_amt", string.IsNullOrEmpty(dt.Rows[0]["CBFTotalAmount"].ToString())?"0":dt.Rows[0]["CBFTotalAmount"].ToString()},
                    {"podetails_total", string.IsNullOrEmpty(dt.Rows[0]["CBFTotalAmount"].ToString())?"0":dt.Rows[0]["CBFTotalAmount"].ToString()},
                    {"podetails_cbfdet_gid",string.IsNullOrEmpty(cbfdetgid.ToString())?"0":cbfdetgid.ToString()},
                    {"podetails_tax1","0"},
                    {"podetails_discount","0"},
                    {"podetails_tax3","0"},
                    {"podetails_txt_percent","0"},
                    {"podetails_isremoved","N"}
                };
                    string tbname1 = "fb_trn_tpodetails";
                    string query_result1 = objCommonIUD.InsertCommon(codes, tbname1);
              
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
        public void UpdateWOHeaderAmount(int wogid = 0)
        {
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@wogid", SqlDbType.Int).Value = wogid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updatewototalamnt";
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
        public string ChkWOSplitExists(int wodetgid = 0) 
        {
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@wodetailgid", SqlDbType.Int).Value = wodetgid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "chkwosplitexists";
                string result = Convert.ToString(cmd.ExecuteScalar());
                return result;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return "404";
            }
            finally
            {
                con.Close();
            }
        }
        public DataSet GetWOAmendment() 
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getwoamendmentsummary";
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
        public DataTable DoAmend(int wogid = 0)
        {
            DataTable dt = new DataTable();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_WOAmendment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@UID", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]); ;
                cmd.Parameters.Add("@POGId", SqlDbType.Int).Value = wogid;
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
        public string CheckFinalApproverTag(int ref_gid = 0,int ref_flag = 0,int queue_gid = 0)  
        { 
            try
            {
                getconnection();
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
        public DataSet GetPOSummary() 
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_grnnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getwosummaryforgrn";
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
        public DataSet GetGRNInwardDetails(int WOGid = 0)
        {
            DataSet ds = new DataSet();
            try 
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_grnnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@wodetailgid", SqlDbType.Int).Value = WOGid;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getgrninwardheader";
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
        public DataSet GetGRNInwardDetailsView(int scngid = 0)
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_grnnew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@wogid", SqlDbType.Int).Value = scngid;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "viewgrn";
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
        public string AddInwardDetailsGRN(SCNInward objSCNInward)
        {
            try 
            {
                string scninwardgid = "";
                string SCNInwardNumber = objCmnFunctions.GetSequenceNo("GRN");
                string[,] codes2 = new string[,]
                { 
                    {"grninwrdheader_refno", SCNInwardNumber},
                    {"grninwrdheader_grndatetime","sysdatetime()"},
                    {"grninwrdheader_rasiergid",objCmnFunctions.GetLoginUserGid().ToString()},
                    {"grninwardheader_poheader", string.IsNullOrEmpty(objSCNInward.WOGid.ToString())?"0":objSCNInward.WOGid.ToString()},
                    {"grninwrdheader_dcno", string.IsNullOrEmpty(objSCNInward.SCNDocNumber.ToString())?"":objSCNInward.SCNDocNumber.ToString()},
                    {"grninwrdheader_invoiceno",string.IsNullOrEmpty(objSCNInward.SCNInvoiceNumber)?"":objSCNInward.SCNInvoiceNumber},
                    {"grninwrdheader_remarks",string.IsNullOrEmpty(objSCNInward.SCNDescription)?"0":objSCNInward.SCNDescription},
                    {"grninwrdheader_branch_type",string.IsNullOrEmpty(objSCNInward.SCNBranchType)?"":objSCNInward.SCNBranchType},
                    {"grninwrdheader_status","9"},
                    {"grninwrdheader_isremoved","N"}
                };
                string tbname = "fb_trn_tgrninwrdheader";
                string query_result = objCommonIUD.InsertCommon(codes2, tbname);

                if (query_result.ToLower() == "success")
                {
                    getconnection();
                    cmd = new SqlCommand("pr_fb_trn_wonew", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                    cmd.Parameters.Add("@wodetailgid", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(objSCNInward.WOGid.ToString()) ? "0" : objSCNInward.WOGid.ToString());
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getgrnheadergid";
                    scninwardgid = Convert.ToString(cmd.ExecuteScalar());
                    for (int i = 0; i < objSCNInward.SelectedItems.Length; i++)
                    {
                        int rowlength = Convert.ToInt32(objSCNInward.ReceivedRows[i].ToString());
                        if (objSCNInward.IsAssetSerial[i].ToString() == "Y")
                        {
                            rowlength = Convert.ToInt32(Math.Round(Convert.ToDecimal(objSCNInward.ReceivedQty[i].ToString())));
                        }
                        string query_result1 = "";
                       
                        decimal qty = Convert.ToDecimal(objSCNInward.ReceivedQty[i].ToString()) / Convert.ToDecimal(rowlength);
                        for (int j = 0; j < rowlength; j++)
                        {
                            string[,] codes = new string[,] 
                                {
                                    {"grninwrddet_grnreleforpo_gid", objSCNInward.SelectedItems[i].ToString()},
                                    {"grninwrddet_reced_qty", qty.ToString()},
                                    {"grninwrddet_reced_date", "sysdatetime()"},
                                    {"grninwrddet_grninwrdhead_gid", string.IsNullOrEmpty(scninwardgid)?"0":scninwardgid},
                                    {"grninwrddet_isremoved","N"}
                                };
                            string tbname1 = "fb_trn_tgrninwrddet";
                            query_result1 = objCommonIUD.InsertCommon(codes, tbname1);

                        }
                      
                        if (query_result1 == "success")
                        {
                            getconnection();
                            cmd = new SqlCommand("pr_fb_trn_wonew", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@wodetailgid", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(objSCNInward.SelectedItems[i].ToString()) ? "0" : objSCNInward.SelectedItems[i].ToString());
                            cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updatebalanceqty";
                            cmd.ExecuteNonQuery();
                        }
                    }

                }
                return SCNInwardNumber;
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

        //Dhasarathan 
        public DataSet GetWOAttachments(int wogid = 0)
        {
            DataSet ds = new DataSet();
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@wogid", SqlDbType.Int).Value = wogid;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["employee_gid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getWoAttachment";
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

        public int CreateWOAttachment(Attachments WOAttachment)
        {
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_trn_wonew", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getrefflagWO";
                string refflag = Convert.ToString(cmd.ExecuteScalar());
                string attachmenttype = "0";
                string[,] codes2 = new string[,] 
                   {
                     {"attachment_ref_flag",string.IsNullOrEmpty(refflag)?"0":refflag},
                     {"attachment_ref_gid",string.IsNullOrEmpty(WOAttachment.AttachmentRefGid.ToString())?"0":WOAttachment.AttachmentRefGid.ToString()},
                     {"attachment_filename",string.IsNullOrEmpty(WOAttachment.AttachedActualFileName)?"":WOAttachment.AttachedActualFileName},
                     {"attachment_desc",string.IsNullOrEmpty(WOAttachment.AttachDescription)?"":WOAttachment.AttachDescription},
                     {"attachment_attachmenttype_gid",string.IsNullOrEmpty(attachmenttype)?"0":attachmenttype},
                     {"attachment_date","sysdatetime()" },
                     {"attachment_by",objCmnFunctions.GetLoginUserGid().ToString()},
                     {"attachment_isremoved","N" },
                     {"attachment_tempfilename",string.IsNullOrEmpty(WOAttachment.TempFileName)?"":WOAttachment.TempFileName},
                   };
                string insertcommand2 = objCommonIUD.InsertCommon(codes2, "iem_trn_tattachment");

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

        public int DeleteWOAttachment(string id)
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
                string updatecon2 = objCommonIUD.UpdateCommon(codw2, codewhe2, "iem_trn_tattachment");
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
        //Dhasarathan 


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
    }
}