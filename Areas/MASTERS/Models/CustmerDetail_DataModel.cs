using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Globalization;
using IEM.Areas.MASTERS.Models;
using IEM.Common;

namespace IEM.Areas.MASTERS.Models
{
    public class CustmerDetail_DataModel : Iiem_mst_CustomerDetail 
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CommonIUD comm = new CommonIUD();
        CmnFunctions objCmnFunctions = new CmnFunctions();
        ErrorLog objErrorLog = new ErrorLog();
        public int pageMode = 0;

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
     
         

        

    

        public IEnumerable<CustomerdetailModel> GetScoreByGrp(int id)
        {
            List<CustomerdetailModel> objSESubCategory = new List<CustomerdetailModel>();
            try
            {

                CustomerdetailModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_SESubCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@Catname", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@Catid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@rateid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@rategroupid", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@queid", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectrate";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //objModel = new CustomerdetailModel();
                    //objModel.seRatescore = Convert.ToString(dt.Rows[i]["serate_score"].ToString());
                    //objModel.seRateName = Convert.ToString(dt.Rows[i]["serate_name"].ToString());
                    //objSESubCategory.Add(objModel);
                }

                return objSESubCategory;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objSESubCategory;
            }
            finally
            {
                con.Close();
            }
        }

         

      

        public string GetSPincodeID(int SECategoryId)
        {
            try
            {
                GetConnection();
                string[,] parameter = new string[,]
                {
                    {"@id",SECategoryId.ToString()},
                    {"@action","select1"},
                };
                return comm.ProcedureCommon(parameter, "pr_asms_mst_SECategory");
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
        

       

       

        //public List<CustomerdetailModel> GetPincode()
        //{
        //    List<CustomerdetailModel> objSupContact = new List<CustomerdetailModel>();
        //    try
        //    {

        //        CustomerdetailModel objModel;
        //        GetConnection();
        //        DataTable dt = new DataTable();
        //        cmd = new SqlCommand("GST_MST_PINCODE_SL", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        //cmd.Parameters.Add("@pincode_gid", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
        //        cmd.Parameters.Add("@StatementType", SqlDbType.VarChar).Value = "SEL";
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            objModel = new CustomerdetailModel();
        //            objModel.PincodeID = Convert.ToInt32(dt.Rows[i]["pincode_gid"].ToString());
        //            objModel.Pincode = Convert.ToString(dt.Rows[i]["pincode_code"].ToString());
        //            objSupContact.Add(objModel);
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
        //    return objSupContact;
        //}



        //public List<CustomerdetailModel> GetAllState()
        //{
        //    List<CustomerdetailModel> objSupContact = new List<CustomerdetailModel>();
        //    try
        //    {

        //        CustomerdetailModel objModel;
        //        GetConnection();
        //        DataTable dt = new DataTable();
        //        cmd = new SqlCommand("pr_asms_GetCSC", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getallstate";
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            objModel = new CustomerdetailModel();
        //            objModel.stateid = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["state_gid"].ToString()) ? "0" : dt.Rows[i]["state_gid"].ToString());
        //            objModel.statename = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["state_name"].ToString()) ? "" : dt.Rows[i]["state_name"].ToString());
        //            objSupContact.Add(objModel);
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
        //    return objSupContact;
        //}

        //public List<CustomerdetailModel> GetAllDistrict()
        //{
        //    List<CustomerdetailModel> objSupContact = new List<CustomerdetailModel>();
        //    try
        //    {

        //        CustomerdetailModel objModel;
        //        GetConnection();
        //        DataTable dt = new DataTable();
        //        cmd = new SqlCommand("GST_MST_DISTRICT_SL", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@StatementType", SqlDbType.VarChar).Value = "SEL";
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            objModel = new CustomerdetailModel();
        //            objModel.districtID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["district_gid"].ToString()) ? "" : dt.Rows[i]["district_gid"].ToString());
        //            objModel.districtname = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["district_name"].ToString()) ? "" : dt.Rows[i]["district_name"].ToString());
        //            objSupContact.Add(objModel);
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
        //    return objSupContact;
        //}


        public List<CustomerdetailModel> Getcitylist(int Pincodeid)
        {
            List<CustomerdetailModel> objSupContact = new List<CustomerdetailModel>();
            try
            {

                CustomerdetailModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("GST_MST_PINCODE_SL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@pincode_gid", SqlDbType.Int).Value = Pincodeid;
                cmd.Parameters.Add("@StatementType", SqlDbType.VarChar).Value = "SID";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new CustomerdetailModel();

                    objModel.stateid = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["district_state_gid"].ToString()) ? "0" : dt.Rows[i]["district_state_gid"].ToString());
                    objModel.statename = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["state_name"].ToString()) ? "" : dt.Rows[i]["state_name"].ToString());

                    objModel.districtID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["pincode_district_gid"].ToString()) ? "0" : dt.Rows[i]["pincode_district_gid"].ToString());
                    objModel.districtname = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["district_name"].ToString()) ? "" : dt.Rows[i]["district_name"].ToString());
                    objSupContact.Add(objModel);
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
            return objSupContact;
        }

        public string InsertCustomerDetail(CustomerdetailModel CustomerDetail)
        {
            string result;
            try
            {
                CommonIUD comm = new CommonIUD();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tcustomerdetail", con);
                GetConnection();
                cmd.Parameters.AddWithValue("@action", "InExist");
                cmd.Parameters.AddWithValue("@customer_code", CustomerDetail.CustomerCode);
                cmd.Parameters.AddWithValue("@pincode_gid", CustomerDetail.PincodeID);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
	               {
                        {"customer_code",CustomerDetail.CustomerCode.ToString()},
                        {"customer_name",CustomerDetail.CustomerName.ToString() },
	                    {"customer_address",CustomerDetail.CustomerAddress},
	                    {"state_gid", CustomerDetail.stateid.ToString()},
                        {"district_gid",CustomerDetail.districtID.ToString()},
                         {"pincode_gid",CustomerDetail.PincodeID.ToString()},
                        {"customerdetail_status","Y"},
                         {"customerdetail_isremoved","N"},                        
                        {"customer_insertby	", Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                        {"customer_insertdate", Convert.ToString(comm.GetCurrentDate()) } 

                  };
                    string tname = "iem_mst_tcustomerdetails";
                    string insertcommend = comm.InsertCommon(codes, tname);
                    result = "success";
                }
                else
                {
                    result = "Duplicate record !";
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }

            // throw new NotImplementedException();
        }

        public IEnumerable<CustomerdetailModel> GetCustomerDetail(string filter)
        {
            try
            {
                List<CustomerdetailModel> objOrgType = new List<CustomerdetailModel>();
                CustomerdetailModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tcustomerdetail", con);
                cmd.Parameters.AddWithValue("@action", "GetCustDet");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new CustomerdetailModel();
                    objModel.CustomerID = Convert.ToInt32(dt.Rows[i]["customerheader_gid"].ToString());
                    objModel.CustomerCode = Convert.ToString(dt.Rows[i]["customerheader_customercode"].ToString());
                    objModel.CustomerName = Convert.ToString(dt.Rows[i]["customerheader_name"].ToString());
                    objModel.Panno = Convert.ToString(dt.Rows[i]["customerheader_panno"].ToString());
                    objModel.customergst_gstin = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["customergst_gstin"].ToString()) ? "-" : dt.Rows[i]["customergst_gstin"].ToString()); 
                                        
                    objOrgType.Add(objModel);
                }

                return objOrgType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            // throw new NotImplementedException();
        }

        public List< CustomerdetailModel>  GetCustomerDetailById(string customer_code, string customer_name, string panno)
        {
            try
            {
                List<CustomerdetailModel> objOrgType = new List<CustomerdetailModel>();
                CustomerdetailModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tcustomerdetail", con);
                cmd.Parameters.AddWithValue("@action", "GetCustDetByID");
                cmd.Parameters.AddWithValue("@customer_code", customer_code);
                cmd.Parameters.AddWithValue("@customer_name", customer_name);
                cmd.Parameters.AddWithValue("@panno", panno);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new CustomerdetailModel();
                    objModel.CustomerID = Convert.ToInt32(dt.Rows[i]["customerheader_gid"].ToString());
                    objModel.CustomerCode = Convert.ToString(dt.Rows[i]["customerheader_customercode"].ToString());
                    objModel.CustomerName = Convert.ToString(dt.Rows[i]["customerheader_name"].ToString());
                    objModel.Panno = Convert.ToString(dt.Rows[i]["customerheader_panno"].ToString());
                    objModel.customergst_gstin = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["customergst_gstin"].ToString()) ? "-" : dt.Rows[i]["customergst_gstin"].ToString()); 
                    
                    objOrgType.Add(objModel);
                }

                return objOrgType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            // throw new NotImplementedException();
        }


        public CustomerdetailModel GetCustomerDetailId(int CustomerdetailID)
        {
            CustomerdetailModel objModel = new CustomerdetailModel();
            List<CustomerdetailModel> objOrgType = new List<CustomerdetailModel>();
            try
            {


                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tcustomerdetail", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", CustomerdetailID);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                int x = 0;
                if (dt.Rows.Count > 0)
                {

                    //bool pincode = int.TryParse(dt.Rows[0]["cityclass_gid"].ToString().Trim(), out x);
                    //bool tiergid = int.TryParse(dt.Rows[0]["tier_gid"].ToString().Trim().Trim(), out x);
                    objModel = new CustomerdetailModel()
                    {


                        CustomerID = Convert.ToInt32(dt.Rows[0]["customer_gid"].ToString().Trim()),
                        CustomerCode = Convert.ToString(dt.Rows[0]["customer_code"].ToString().Trim()),
                        CustomerName = Convert.ToString(dt.Rows[0]["customer_name"].ToString().Trim()),
                        CustomerAddress = Convert.ToString(dt.Rows[0]["customer_address"].ToString().Trim()),
                        selectedPincodeID = Convert.ToInt32(dt.Rows[0]["pincode_gid"].ToString().Trim()),
                        selectedStateID = Convert.ToInt32(dt.Rows[0]["state_gid"].ToString().Trim()),
                        selectedDistrictID = Convert.ToInt32(dt.Rows[0]["district_gid"].ToString().Trim()) 
                  };

                    return objModel;
                }


            }

            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {

                con.Close();

            }
            // throw new NotImplementedException();
            return objModel;
        }



        public string EditCustomerDetail(CustomerdetailModel customerdetail)
        {
            CommonIUD delecomm = new CommonIUD();
            try
            {
                string set = customerdetail.CustomerName.ToLower();
                if (set.Contains("'") == true)
                {
                    set = set.Replace("'", "''");
                }
                GetConnection();
                cmd = new SqlCommand("pr_iem_mst_tcustomerdetail", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@gid", SqlDbType.Int).Value = customerdetail.CustomerID;
                cmd.Parameters.Add("@customer_code", SqlDbType.VarChar).Value = customerdetail.CustomerCode;
                cmd.Parameters.Add("@customer_name", SqlDbType.VarChar).Value = set;
                cmd.Parameters.Add("@pincode_gid", SqlDbType.Int).Value = customerdetail.PincodeID;                
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Exist";
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();

                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
                    {
                        {"customer_code",customerdetail.CustomerCode.ToString()},
                        {"customer_name",customerdetail.CustomerName.ToString() },
	                    {"customer_address",customerdetail.CustomerAddress},
	                    {"state_gid", customerdetail.stateid.ToString()},
                        {"district_gid",customerdetail.districtID.ToString()},
                         {"pincode_gid",customerdetail.PincodeID.ToString()},
                        {"customerdetail_status","Y"},
                         {"customerdetail_isremoved","N"},    
                        {"customer_updateby",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },                                                                
                        {"customer_updatedate",Convert.ToString(comm.GetCurrentDate())}


                    };
                    string[,] whrcol = new string[,]
                    {
                         {"customer_gid",customerdetail.CustomerID.ToString()}
            
                    };
                    string tblname = "iem_mst_tcustomerdetails";

                    string updcomm = comm.UpdateCommon(codes, whrcol, tblname);
                    return null;
                }
                else { return res1; }
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


        public void DeleteCustomerDetail(int customerid)
        {
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {

                string[,] codes = new string[,]
	       {
                 {"customerdetail_isremoved", "Y"}
	            
           };
                string[,] whrcol = new string[,]
	        {
                {"customer_gid", customerid.ToString ()}
            };
                string tblname = "iem_mst_tcustomerdetails";


                string deletetbl = delecomm.DeleteCommon(codes, whrcol, tblname);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                con.Close();
            }
            //throw new NotImplementedException();
        }

        public DataTable GetCustomerDetailexcel()
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tcustomerdetail", con);
                cmd.Parameters.AddWithValue("@action", "select");                
                cmd.CommandType = CommandType.StoredProcedure;
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
                da.Dispose();
            }
        }

        public int CheckCustNameIsExists(string CustomerName, string PanNo)
        {
            try
            {
                int result = 0;
                GetConnection();
                cmd = new SqlCommand("pr_iem_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (HttpContext.Current.Session["CustomerHeaderGid"] != null)
                {
                    cmd.Parameters.Add("@customerheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["CustomerHeaderGid"]);
                }
                else
                {
                    cmd.Parameters.Add("@customerheadergid", SqlDbType.Int).Value = 0;
                }
                cmd.Parameters.Add("@custpanno", SqlDbType.VarChar).Value = PanNo;
                cmd.Parameters.Add("@custname", SqlDbType.VarChar).Value = CustomerName;
                cmd.Parameters.Add("@custnamefor", SqlDbType.VarChar).Value = "check";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getcustomernamesearch";
                result = Convert.ToInt32(cmd.ExecuteScalar());
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
        public int CheckCustPanNoIsExists(string CustomerPanNo)
        {
            try
            {
                int result = 0;
                GetConnection();
                cmd = new SqlCommand("pr_iem_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (HttpContext.Current.Session["CustomerHeaderGid"] != null)
                {
                    cmd.Parameters.Add("@customerheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["CustomerHeaderGid"]);
                }
                else
                {
                    cmd.Parameters.Add("@customerheadergid", SqlDbType.Int).Value = 0;
                }
                cmd.Parameters.Add("@custpanno", SqlDbType.VarChar).Value = CustomerPanNo;
                cmd.Parameters.Add("@custnamefor", SqlDbType.VarChar).Value = "check";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getcustomerpannosearch";
                result = Convert.ToInt32(cmd.ExecuteScalar());
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

        public void InsertCustomerHeader(CustomerDetailHeader custHeader)
        {
            try
            {
                string contractfrom = Convert.ToString(string.IsNullOrEmpty(custHeader._ContractFrom) ? "" : objCmnFunctions.convertoDateTimeStringasms(custHeader._ContractFrom));
                string contractto = Convert.ToString(string.IsNullOrEmpty(custHeader._ContractTo) ? "" : objCmnFunctions.convertoDateTimeStringasms(custHeader._ContractTo));
                string[,] codes = new string[,]
                   {
                     {"customerheader_customertype",Convert.ToString((custHeader._CustomerType)==null?"":custHeader._CustomerType) },
                     {"customerheader_customercode",Convert.ToString((custHeader._CustomerCode)==null?"":custHeader._CustomerCode) },
                     {"customerheader_name",Convert.ToString((custHeader._CustomerName)==null?"":custHeader._CustomerName) },
                     {"customerheader_panno",Convert.ToString((custHeader._PanNo)==null?"":custHeader._PanNo) },
                     //{"customerheader_organizationtype_gid",Convert.ToString(custHeader.selectedOrganizationID) },
                     //{"customerheader_servicetype_gid",Convert.ToString(custHeader.selectedServiceID) },
                     {"customerheader_companyregno",Convert.ToString((custHeader._CompanyRegNo)==null?"":custHeader._CompanyRegNo) },
                     {"customerheader_contractfrom",contractfrom },
                     {"customerheader_contractto",contractto },
                     {"customerheader_website",Convert.ToString((custHeader._website)==null?"":custHeader._website) },
                     {"customerheader_emailid",Convert.ToString((custHeader._EmailID)==null?"":custHeader._EmailID) },
                     //{"customerheader_approxspend",Convert.ToString(objCmnFunctions.convertoDecimal(custHeader._ApproxSpend)) },
                     //{"customerheader_actualspend",Convert.ToString(objCmnFunctions.convertoDecimal(custHeader._ActualSpend)) },
                     //{"customerheader_customercategory_gid",Convert.ToString(custHeader.SelectedCustomerCategoryID) },
                     {"customerheader_isactivecontract",Convert.ToString(custHeader._IsActiveContract) },
                     {"customerheader_reasonfornocontract",Convert.ToString((custHeader._ReasonForNoContract)==null?"":custHeader._ReasonForNoContract) },
                     //{"customerheader_requesttype",Convert.ToString(custHeader._RequestType) },
                     //{"customerheader_requeststatus",Convert.ToString(custHeader._Requeststatus) },
                     //{"customerheader_status",Convert.ToString(custHeader._CustomerStatus) },
                     //{"customerheader_owner_gid",Convert.ToString(custHeader._OwnerId) },
                     //{"customerheader_contacttype_gid",Convert.ToString(custHeader.SelectedCustomerContactTypeID) },
                     //{"customerheader_renewaldate",Convert.ToString(objCmnFunctions.convertoDateTimeString(custHeader._RenewalDate)) },
                     //{"customerheader_remarks",Convert.ToString((custHeader._CustomerTypeRemarks)==null?"":custHeader._CustomerTypeRemarks) },
                     {"customerheader_isremoved","N" },
                     {"customerheader_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"customerheader_insert_date","sysdatetime()" }
                   };
                
                string[,] codes2 = new string[,]
                   {
                     {"customerheader_customertype",Convert.ToString((custHeader._CustomerType)==null?"":custHeader._CustomerType) },
                     {"customerheader_customercode",Convert.ToString((custHeader._CustomerCode)==null?"":custHeader._CustomerCode) },
                     {"customerheader_name",Convert.ToString((custHeader._CustomerName)==null?"":custHeader._CustomerName) },
                     {"customerheader_panno",Convert.ToString((custHeader._PanNo)==null?"":custHeader._PanNo) },
                     //{"customerheader_organizationtype_gid",Convert.ToString(custHeader.selectedOrganizationID) },
                     //{"customerheader_servicetype_gid",Convert.ToString(custHeader.selectedServiceID) },
                     {"customerheader_companyregno",Convert.ToString((custHeader._CompanyRegNo)==null?"":custHeader._CompanyRegNo) },
                     {"customerheader_contractfrom",contractfrom },
                     {"customerheader_contractto",contractto },
                     {"customerheader_website",Convert.ToString((custHeader._website)==null?"":custHeader._website) },
                     {"customerheader_emailid",Convert.ToString((custHeader._EmailID)==null?"":custHeader._EmailID) },
                     //{"customerheader_approxspend",Convert.ToString(objCmnFunctions.convertoDecimal(custHeader._ApproxSpend)) },
                     //{"customerheader_actualspend",Convert.ToString(objCmnFunctions.convertoDecimal(custHeader._ActualSpend)) },
                     //{"customerheader_customercategory_gid",Convert.ToString(custHeader.SelectedCustomerCategoryID) },
                     {"customerheader_isactivecontract",Convert.ToString(custHeader._IsActiveContract) },
                     {"customerheader_reasonfornocontract",Convert.ToString((custHeader._ReasonForNoContract)==null?"":custHeader._ReasonForNoContract) },
                     //{"customerheader_requesttype",Convert.ToString(custHeader._RequestType) },
                     //{"customerheader_requeststatus",Convert.ToString(custHeader._Requeststatus) },
                     //{"customerheader_status",Convert.ToString(custHeader._CustomerStatus) },
                     //{"customerheader_owner_gid",Convert.ToString(custHeader._OwnerId) },
                     //{"customerheader_contacttype_gid",Convert.ToString(custHeader.SelectedCustomerContactTypeID) },
                     //{"customerheader_renewaldate",Convert.ToString(objCmnFunctions.convertoDateTimeString(custHeader._RenewalDate)) },
                     //{"customerheader_remarks",Convert.ToString((custHeader._CustomerTypeRemarks)==null?"":custHeader._CustomerTypeRemarks) },
                     {"customerheader_isremoved","N" },
                     {"customerheader_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"customerheader_insert_date","sysdatetime()" }
                   };
             
                string insertcommand1 = comm.InsertCommon(codes2, "iem_mst_tcustomerheader");
                string insertcommand = comm.InsertCommon(codes, "iem_mst_tcustomerheaderaudit");
                //}

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }

        public string UpdateCustomerHeader(CustomerDetailHeader custHeader)
        {
            string res = string.Empty;
            try
            {
                
                GetConnection();
                cmd = new SqlCommand("pr_iem_trn_tcustomerheader", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@custid", SqlDbType.Int).Value = custHeader._HeaderID;
                cmd.Parameters.Add("@custcode", SqlDbType.VarChar).Value = custHeader._CustomerCode;
                cmd.Parameters.Add("@custname", SqlDbType.VarChar).Value = custHeader._CustomerName;
                cmd.Parameters.Add("@custwebsite", SqlDbType.VarChar).Value = custHeader._website;
                cmd.Parameters.Add("@custemailid", SqlDbType.VarChar).Value = custHeader._EmailID;
                cmd.Parameters.Add("@custisactivecontract", SqlDbType.VarChar).Value = custHeader._IsActiveContract;
                cmd.Parameters.Add("@custreasonfornocontract", SqlDbType.VarChar).Value = custHeader._ReasonForNoContract;
                cmd.Parameters.Add("@custpanno", SqlDbType.VarChar).Value = custHeader._PanNo;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "edit";
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                res = cmd.Parameters["@res"].Value.ToString();

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();

            }
            return res;
        }
        
       
     public void DeleteCustomerMasterDetail(int customerID)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_iem_trn_tcustomerheader", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@custid", SqlDbType.Int).Value = customerID;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "delete";
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

     public void InsertCustomerHeaderDetails(CustomerDetailHeader custHeader)
     {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_iem_trn_tcustomerheader", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@custid", SqlDbType.Int).Value = custHeader._HeaderID;
                cmd.Parameters.Add("@custcode", SqlDbType.VarChar).Value = custHeader._CustomerCode;
                cmd.Parameters.Add("@custname", SqlDbType.VarChar).Value = custHeader._CustomerName;
                cmd.Parameters.Add("@custwebsite", SqlDbType.VarChar).Value = custHeader._website;
                cmd.Parameters.Add("@custemailid", SqlDbType.VarChar).Value = custHeader._EmailID;
                cmd.Parameters.Add("@custisactivecontract", SqlDbType.VarChar).Value = custHeader._IsActiveContract;
                cmd.Parameters.Add("@custreasonfornocontract", SqlDbType.VarChar).Value = custHeader._ReasonForNoContract;
                cmd.Parameters.Add("@custpanno", SqlDbType.VarChar).Value = custHeader._PanNo;
                cmd.Parameters.Add("@custcreatedby", SqlDbType.Int).Value = Convert.ToInt32(objCmnFunctions.GetLoginUserGid());
                cmd.Parameters.Add("@custcreatedon", SqlDbType.VarChar).Value = "sysdatetime()";
                
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Insert";
                
                cmd.ExecuteNonQuery();
                

            }
            catch(Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
     }

     public void InsertCustContactDetails(CustomerContactDetails CustContactDetails)
     {
         try
         {
             GetConnection();
             cmd = new SqlCommand("pr_iem_trn_tcustomercontact", con);
             cmd.CommandType = CommandType.StoredProcedure;
             //cmd.Parameters.Add("@id", SqlDbType.Int).Value = CustContactDetails._CustContactDetailsID;
             cmd.Parameters.Add("@customerheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["CustomerHeaderGid"]);
             cmd.Parameters.Add("@address1", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(CustContactDetails._Address1) ? "" : CustContactDetails._Address1);
             cmd.Parameters.Add("@address2", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(CustContactDetails._Address2) ? "" : CustContactDetails._Address2);
             cmd.Parameters.Add("@address3", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(CustContactDetails._Address3) ? "" : CustContactDetails._Address3);
             cmd.Parameters.Add("@addresstypegid", SqlDbType.Int).Value = Convert.ToInt32(CustContactDetails.selectedAddressTypeID);
             cmd.Parameters.Add("@countrygid", SqlDbType.Int).Value = Convert.ToInt32(CustContactDetails.selectedCountryID);
             cmd.Parameters.Add("@stategid", SqlDbType.Int).Value = Convert.ToInt32(CustContactDetails.selectedStateID);
             cmd.Parameters.Add("@citygid", SqlDbType.Int).Value = Convert.ToInt32(CustContactDetails.selectedCityID);
             cmd.Parameters.Add("@districtgid", SqlDbType.Int).Value = Convert.ToInt32(CustContactDetails.selectedDistrictID);
             cmd.Parameters.Add("@pincode", SqlDbType.Int).Value = Convert.ToInt32(CustContactDetails._PinCode);
             cmd.Parameters.Add("@phoneno", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(CustContactDetails._PhoneNo) ? "" : CustContactDetails._PhoneNo);
             cmd.Parameters.Add("@fax", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(CustContactDetails._Fax) ? "" : CustContactDetails._Fax);
             //cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
             cmd.Parameters.Add("@createdby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
             cmd.Parameters.Add("@createdon", SqlDbType.VarChar).Value = Convert.ToString(comm.GetCurrentDate());
             cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "Insert";
             //cmd.Parameters.Add("@res", SqlDbType.VarChar, 64);
             //cmd.Parameters["@res"].Direction = ParameterDirection.Output;
             cmd.ExecuteNonQuery();
             //string res = Convert.ToString(cmd.Parameters["@res"].Value.ToString());



         }
         catch (Exception ex)
         {
             objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
         }
         finally
         {
             con.Close();
             da.Dispose();
         }
     }
       

        public long GetCustomerHeaderGid(CustomerDetailHeader custHeader)
        {
            Int64 CustHeaderGid = 0;
            try
            {

                GetConnection();
                cmd = new SqlCommand("pr_iem_trn_tcustomerheader", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@custcode", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(custHeader._CustomerCode) ? "" : custHeader._CustomerCode);
                cmd.Parameters.Add("@custcreatedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getcustomergid";
                CustHeaderGid = Convert.ToInt64(cmd.ExecuteScalar());


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return CustHeaderGid;
        }

        public CustomerDetailHeader GetcustomerdetailHeader(string customer_gid)
        {
            try
            {
                CustomerDetailHeader cust_list = new CustomerDetailHeader();
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tcustomerdetailheader", con);
                cmd.Parameters.AddWithValue("@cust_gid", Convert.ToInt32(customer_gid));
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    cust_list._HeaderID = Convert.ToInt32(dt.Rows[i]["customerheader_gid"].ToString());
                    cust_list._CustomerCode = Convert.ToString(dt.Rows[i]["customerheader_customercode"].ToString());
                    cust_list._CustomerName = Convert.ToString(dt.Rows[i]["customerheader_name"].ToString());
                    cust_list._website = Convert.ToString(dt.Rows[i]["customerheader_website"].ToString());
                    cust_list._EmailID = Convert.ToString(dt.Rows[i]["customerheader_emailid"].ToString());
                    cust_list._PanNo = Convert.ToString(dt.Rows[i]["customerheader_panno"].ToString());
                    cust_list._IsActiveContract = Convert.ToString(dt.Rows[i]["customerheader_isactivecontract"].ToString());
                    cust_list._ReasonForNoContract = Convert.ToString(dt.Rows[i]["customerheader_reasonfornocontract"].ToString());

                }

                return cust_list;

            }
            catch(Exception EX)
            {
                throw EX;
            }
            finally 
                {
                    con.Close();
                }
        }

         

public List<CustomerContactDetails> GetAddressType()
        {
            List<CustomerContactDetails> objCustContact = new List<CustomerContactDetails>();
            try
            {

                CustomerContactDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getaddresstype";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new CustomerContactDetails();
                    objModel._AddressTypeID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["addresstype_gid"].ToString()) ? "0" : dt.Rows[i]["addresstype_gid"].ToString());
                    objModel._AddressTypeName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["addresstype_name"].ToString()) ? "" : dt.Rows[i]["addresstype_name"].ToString());
                    objCustContact.Add(objModel);
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
            return objCustContact;
        }
 
public List<CustomerContactDetails> GetPincode()
        {
            List<CustomerContactDetails> objCustContact = new List<CustomerContactDetails>();
            try
            {

                CustomerContactDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("GST_MST_PINCODE_SL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@pincode_gid", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@StatementType", SqlDbType.VarChar).Value = "SEL";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new CustomerContactDetails();
                    objModel.Pincodeid = Convert.ToInt32(dt.Rows[i]["pincode_gid"].ToString());
                    objModel.Pincode = Convert.ToString(dt.Rows[i]["pincode_code"].ToString());
                    objCustContact.Add(objModel);
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
            return objCustContact;
        }

 

  public List<CustomerContactDetails> GetCountry()
        {
            List<CustomerContactDetails> objCustContact = new List<CustomerContactDetails>();
            try
            {

                CustomerContactDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getcountry";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new CustomerContactDetails();
                    objModel._CountryID = Convert.ToInt32(dt.Rows[i]["country_gid"].ToString());
                    objModel._CountryName = Convert.ToString(dt.Rows[i]["country_name"].ToString());
                    objCustContact.Add(objModel);
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
            return objCustContact;
        }



  public List<CustomerContactDetails> GetAllState()
        {
            List<CustomerContactDetails> objCustContact = new List<CustomerContactDetails>();
            try
            {

                CustomerContactDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getallstate";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new CustomerContactDetails();
                    objModel._StateID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["state_gid"].ToString()) ? "0" : dt.Rows[i]["state_gid"].ToString());
                    objModel._StateName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["state_name"].ToString()) ? "" : dt.Rows[i]["state_name"].ToString());
                    objCustContact.Add(objModel);
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
            return objCustContact;
        }

  public List<CustomerContactDetails> GetAllCity()
        {
            List<CustomerContactDetails> objCustContact = new List<CustomerContactDetails>();
            try
            {

                CustomerContactDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getallcity";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new CustomerContactDetails();
                    objModel._CityID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["city_gid"].ToString()) ? "" : dt.Rows[i]["city_gid"].ToString());
                    objModel._CityName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["city_name"].ToString()) ? "" : dt.Rows[i]["city_name"].ToString());
                    objCustContact.Add(objModel);
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
            return objCustContact;
        }
         
 public List<CustomerContactDetails> GetAllDistrict()
        {
            List<CustomerContactDetails> objCustContact = new List<CustomerContactDetails>();
            try
            {

                CustomerContactDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("GST_MST_DISTRICT_SL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@StatementType", SqlDbType.VarChar).Value = "SEL";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new CustomerContactDetails();
                    objModel._DistrictID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["district_gid"].ToString()) ? "" : dt.Rows[i]["district_gid"].ToString());
                    objModel._DistrictName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["district_name"].ToString()) ? "" : dt.Rows[i]["district_name"].ToString());
                    objCustContact.Add(objModel);
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
            return objCustContact;
        }

 public IEnumerable<CustomerContactDetails> GetCustContactDetails()
 {
     List<CustomerContactDetails> objClient = new List<CustomerContactDetails>();
     try
     {

         CustomerContactDetails objModel;
         GetConnection();
         DataTable dt = new DataTable();
         cmd = new SqlCommand("pr_iem_trn_tcustomercontact", con);
         cmd.CommandType = CommandType.StoredProcedure;
         cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
         cmd.Parameters.Add("@dismode", SqlDbType.Int).Value = 1;
         cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
         cmd.Parameters.Add("@customerheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["CustomerHeaderGid"]);
         cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
         da = new SqlDataAdapter(cmd);
         da.Fill(dt);
         for (int i = 0; i < dt.Rows.Count; i++)
         {
             objModel = new CustomerContactDetails();
             objModel._CustContactDetailsID = Convert.ToInt32(dt.Rows[i]["customercontact_gid"].ToString());
             objModel.selectedAddressTypeID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["customercontact_addresstype_gid"].ToString()) ? "0" : dt.Rows[i]["customercontact_addresstype_gid"].ToString());
             objModel._AddressTypeName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["addresstype_name"].ToString()) ? "" : dt.Rows[i]["addresstype_name"].ToString());
             objModel._Address1 = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["customercontact_address1"].ToString()) ? "" : dt.Rows[i]["customercontact_address1"].ToString());
             objModel._Address2 = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["customercontact_address2"].ToString()) ? "" : dt.Rows[i]["customercontact_address2"].ToString());
             objModel._Address3 = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["customercontact_address3"].ToString()) ? "" : dt.Rows[i]["customercontact_address3"].ToString());
             objModel.selectedCountryID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["customercontact_country_gid"].ToString()) ? "0" : dt.Rows[i]["customercontact_country_gid"].ToString());
             objModel._CountryName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["country_name"].ToString()) ? "" : dt.Rows[i]["country_name"].ToString());
             objModel.selectedStateID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["customercontact_state_gid"].ToString()) ? "0" : dt.Rows[i]["customercontact_state_gid"].ToString());
             objModel._StateName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["state_name"].ToString()) ? "" : dt.Rows[i]["state_name"].ToString());
             objModel.selectedCityID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["customercontact_city_gid"].ToString()) ? "0" : dt.Rows[i]["customercontact_city_gid"].ToString());
             objModel._CityName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["city_name"].ToString()) ? "" : dt.Rows[i]["city_name"].ToString());
             objModel._PinCode = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["customercontact_pincode"].ToString()) ? "0" : dt.Rows[i]["customercontact_pincode"].ToString());
             objModel._DistrictID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["customercontact_district_gid"].ToString()) ? "0" : dt.Rows[i]["customercontact_district_gid"].ToString());
             objModel._DistrictName = string.IsNullOrEmpty(dt.Rows[i]["district_name"].ToString()) ? "0" : dt.Rows[i]["district_name"].ToString();
             objModel._PhoneNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["customercontact_phoneno"].ToString()) ? "" : dt.Rows[i]["customercontact_phoneno"].ToString());
             objModel._Fax = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["customercontact_fax"].ToString()) ? "" : dt.Rows[i]["customercontact_fax"].ToString());
             objClient.Add(objModel);
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
     return objClient;
 }


 

 public void DeleteCustContactDetails(int CustContactDetailsId)
 {
     try
    {
        GetConnection();
        cmd = new SqlCommand("pr_iem_trn_tcustomercontact", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.Add("@id", SqlDbType.Int).Value = CustContactDetailsId;
        cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
        cmd.Parameters.Add("@customerheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["CustomerHeaderGid"]);
        cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
        cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = comm.GetCurrentDate();
        cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "delete";
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

 public void UpdateCustContactDetails(CustomerContactDetails CustContactDetails)
 {
     try
     {
         GetConnection();
         cmd = new SqlCommand("pr_iem_trn_tcustomercontact", con);
         cmd.CommandType = CommandType.StoredProcedure;
         cmd.Parameters.Add("@id", SqlDbType.Int).Value = CustContactDetails._CustContactDetailsID;
         cmd.Parameters.Add("@customerheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["CustomerHeaderGid"]);
         cmd.Parameters.Add("@address1", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(CustContactDetails._Address1) ? "" : CustContactDetails._Address1);
         cmd.Parameters.Add("@address2", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(CustContactDetails._Address2) ? "" : CustContactDetails._Address2);
         cmd.Parameters.Add("@address3", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(CustContactDetails._Address3) ? "" : CustContactDetails._Address3);
         cmd.Parameters.Add("@addresstypegid", SqlDbType.Int).Value = Convert.ToInt32(CustContactDetails.selectedAddressTypeID);
         cmd.Parameters.Add("@countrygid", SqlDbType.Int).Value = Convert.ToInt32(CustContactDetails.selectedCountryID);
         cmd.Parameters.Add("@stategid", SqlDbType.Int).Value = Convert.ToInt32(CustContactDetails.selectedStateID);
         cmd.Parameters.Add("@citygid", SqlDbType.Int).Value = Convert.ToInt32(CustContactDetails.selectedCityID);
         cmd.Parameters.Add("@districtgid", SqlDbType.Int).Value = Convert.ToInt32(CustContactDetails.selectedDistrictID);
         cmd.Parameters.Add("@pincode", SqlDbType.Int).Value = Convert.ToInt32(CustContactDetails._PinCode);
         cmd.Parameters.Add("@phoneno", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(CustContactDetails._PhoneNo) ? "" : CustContactDetails._PhoneNo);
         cmd.Parameters.Add("@fax", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(CustContactDetails._Fax) ? "" : CustContactDetails._Fax);
         cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
         cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
         cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = Convert.ToString(comm.GetCurrentDate());
         cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "edit";
         cmd.Parameters.Add("@res", SqlDbType.VarChar, 64);
         cmd.Parameters["@res"].Direction = ParameterDirection.Output;
         cmd.ExecuteNonQuery();
         string res = Convert.ToString(cmd.Parameters["@res"].Value.ToString());
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

 public List<CustomerContactDetails> GetState(int CountryID)
 {
     List<CustomerContactDetails> objCustContact = new List<CustomerContactDetails>();
     try
     {

         CustomerContactDetails objModel;
         GetConnection();
         DataTable dt = new DataTable();
         cmd = new SqlCommand("pr_asms_GetCSC", con);
         cmd.CommandType = CommandType.StoredProcedure;
         cmd.Parameters.Add("@countryid", SqlDbType.Int).Value = CountryID;
         cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getstate";
         da = new SqlDataAdapter(cmd);
         da.Fill(dt);
         for (int i = 0; i < dt.Rows.Count; i++)
         {
             objModel = new CustomerContactDetails();
             objModel._StateID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["state_gid"].ToString()) ? "0" : dt.Rows[i]["state_gid"].ToString());
             objModel._StateName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["state_name"].ToString()) ? "" : dt.Rows[i]["state_name"].ToString());
             objCustContact.Add(objModel);
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
     return objCustContact;
 }

 public List<CustomerContactDetails> GetCity(int StateID, int CountryID)
 {
     List<CustomerContactDetails> objCustContact = new List<CustomerContactDetails>();
     try
     {

         CustomerContactDetails objModel;
         GetConnection();
         DataTable dt = new DataTable();
         cmd = new SqlCommand("pr_asms_GetCSC", con);
         cmd.CommandType = CommandType.StoredProcedure;
         cmd.Parameters.Add("@stateid", SqlDbType.Int).Value = StateID;
         cmd.Parameters.Add("@countryid", SqlDbType.Int).Value = CountryID;
         cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getallcity";
         da = new SqlDataAdapter(cmd);
         da.Fill(dt);
         for (int i = 0; i < dt.Rows.Count; i++)
         {
             objModel = new CustomerContactDetails();
             objModel._CityID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["city_gid"].ToString()) ? "" : dt.Rows[i]["city_gid"].ToString());
             objModel._CityName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["city_name"].ToString()) ? "" : dt.Rows[i]["city_name"].ToString());
             objCustContact.Add(objModel);
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
     return objCustContact;
 }


 public List<CustomerContactDetails> GetDistrictpincode(string Pincodeid)
 {
     List<CustomerContactDetails> objCustContact = new List<CustomerContactDetails>();
     try
     {

         CustomerContactDetails objModel;
         GetConnection();
         DataTable dt = new DataTable();
         cmd = new SqlCommand("GST_MST_PINCODE_SL", con);
         cmd.CommandType = CommandType.StoredProcedure;
         cmd.Parameters.Add("@pincode_code", SqlDbType.Int).Value = Pincodeid;
         cmd.Parameters.Add("@StatementType", SqlDbType.VarChar).Value = "BYPIN";
         da = new SqlDataAdapter(cmd);
         da.Fill(dt);
         for (int i = 0; i < dt.Rows.Count; i++)
         {
             objModel = new CustomerContactDetails();
             objModel._CountryID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["state_country_gid"].ToString()) ? "0" : dt.Rows[i]["state_country_gid"].ToString());
             objModel._CountryName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["country_name"].ToString()) ? "" : dt.Rows[i]["country_name"].ToString());

             objModel._StateID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["district_state_gid"].ToString()) ? "0" : dt.Rows[i]["district_state_gid"].ToString());
             objModel._StateName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["state_name"].ToString()) ? "" : dt.Rows[i]["state_name"].ToString());

             objModel._DistrictID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["pincode_district_gid"].ToString()) ? "0" : dt.Rows[i]["pincode_district_gid"].ToString());
             objModel._DistrictName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["district_name"].ToString()) ? "" : dt.Rows[i]["district_name"].ToString());
             objCustContact.Add(objModel);
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
     return objCustContact;
 }

 public List<CustomerContactDetails> Getcitylists(int Pincodeid)
 {
     List<CustomerContactDetails> objCustContact = new List<CustomerContactDetails>();
     try
     {

         CustomerContactDetails objModel;
         GetConnection();
         DataTable dt = new DataTable();
         cmd = new SqlCommand("PR_IEM_MST_TPINSELECT", con);
         cmd.CommandType = CommandType.StoredProcedure;
         cmd.Parameters.Add("@pincode_gid", SqlDbType.Int).Value = Pincodeid;
         cmd.Parameters.Add("@StatementType", SqlDbType.VarChar).Value = "BYPIN";
         da = new SqlDataAdapter(cmd);
         da.Fill(dt);
         for (int i = 0; i < dt.Rows.Count; i++)
         {
             objModel = new CustomerContactDetails();

             objModel._StateID  = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["district_state_gid"].ToString()) ? "0" : dt.Rows[i]["district_state_gid"].ToString());
             objModel._StateName= Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["state_name"].ToString()) ? "" : dt.Rows[i]["state_name"].ToString());

             objModel._DistrictID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["pincode_district_gid"].ToString()) ? "0" : dt.Rows[i]["pincode_district_gid"].ToString());
             objModel._DistrictName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["district_name"].ToString()) ? "" : dt.Rows[i]["district_name"].ToString());
             objModel._CountryID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["state_country_gid"].ToString()) ? "0" : dt.Rows[i]["state_country_gid"].ToString());
             objModel._CountryName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["country_name"].ToString()) ? "" : dt.Rows[i]["country_name"].ToString());
             objCustContact.Add(objModel);
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
     return objCustContact;
 }

        //bharathi gst

 public List<EntityGstCustomer> GetState()
 {
     //  throw new NotImplementedException();
     try
     {
         List<EntityGstCustomer> objOrgType = new List<EntityGstCustomer>();
         EntityGstCustomer objModel;
         GetConnection();
         DataTable dt = new DataTable();
         SqlCommand cmd = new SqlCommand("PR_IEM_TCUSTOMERGSTSELECT", con);
         cmd.Parameters.AddWithValue("@customergst_customerheader_gid", Convert.ToInt64(HttpContext.Current.Session["CustomerHeaderGid"]));
         cmd.Parameters.AddWithValue("@StatementType", "SUPSTATE");
         cmd.CommandType = CommandType.StoredProcedure;
         SqlDataAdapter da = new SqlDataAdapter(cmd);
         da = new SqlDataAdapter(cmd);
         da.Fill(dt);
         for (int i = 0; i < dt.Rows.Count; i++)
         {
             objModel = new EntityGstCustomer();
             objModel.customergst_stateid = Convert.ToInt32(dt.Rows[i]["state_gid"].ToString());
             objModel.customergst_state = Convert.ToString(dt.Rows[i]["state_name"].ToString());
             objOrgType.Add(objModel);
         }

         return objOrgType;
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

 public EntityGstCustomer GetStateById(int Disid)
 {
     //  throw new NotImplementedException();
     try
     {
         List<EntityGstCustomer> objOrgType = new List<EntityGstCustomer>();
         EntityGstCustomer objModel;
         GetConnection();
         DataTable dt = new DataTable();
         SqlCommand cmd = new SqlCommand("GST_MST_State_SEL", con);
         cmd.Parameters.AddWithValue("@StatementType", "SID");
         cmd.Parameters.AddWithValue("@state_gid", Disid);
         cmd.CommandType = CommandType.StoredProcedure;
         SqlDataAdapter da = new SqlDataAdapter(cmd);
         da = new SqlDataAdapter(cmd);
         da.Fill(dt);
         objModel = new EntityGstCustomer()
         {

             customergst_stateid = Convert.ToInt32(dt.Rows[0]["state_gid"].ToString()),
             //  objModel.Currencycode = Convert.ToString(dt.Rows[i]["district_code"].ToString());
             customergst_state = Convert.ToString(dt.Rows[0]["state_name"].ToString()),
         };
         return objModel;


         //return objOrgType;
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
 public string DeleteVendor(int PinId)
 {
     string result;
     // throw new NotImplementedException();
     try
     {
         CommonIUD comm = new CommonIUD();
         SqlCommand cmd = new SqlCommand("PR_IEM_TCUSTOMERGSTINSERT", con);
         GetConnection();
         cmd.Parameters.AddWithValue("@StatementType", "D");
         cmd.Parameters.AddWithValue("@customergst_gid", PinId);
         // cmd.Parameters.AddWithValue("@suppliergst_updateby", int.Parse(con.GetLoginUserGid().ToString()));
         cmd.CommandType = CommandType.StoredProcedure;
         cmd.ExecuteNonQuery();
         result = "success";
         return result;
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

 public string Insertcustomer(EntityGstCustomer Classifications)
 {
     string result;
     string res5 = "";
     // throw new NotImplementedException();
     try
     {

         CommonIUD commn = new CommonIUD();
         SqlCommand cmdn = new SqlCommand("PR_IEM_TCUSTOMERGSTINSERT", con);
         GetConnection();
         res5 = "YES";
         if (Classifications.customergst_app == "Yes")
         {
             string ss = Classifications.customergst_tin.Substring(0, 2);// Classifications.suppliergst_tin.Split(new char[] { ' ' }, 2);
             cmdn.Parameters.AddWithValue("@StatementType", "C");
             cmdn.Parameters.AddWithValue("@state_code", ss);
             cmdn.Parameters.AddWithValue("@customergst_state_gid", Convert.ToInt32(Classifications.customergst_stateid));
             cmdn.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
             cmdn.CommandType = CommandType.StoredProcedure;
             cmdn.ExecuteNonQuery();
             res5 = cmdn.Parameters["@res"].Value.ToString();
         }
         else
         {
             Int64 custid = GetCustomerheader();
             CommonIUD comm = new CommonIUD();
             SqlCommand cmd = new SqlCommand("PR_IEM_TCUSTOMERGSTINSERT", con);
             GetConnection();
             cmd.Parameters.AddWithValue("@StatementType", "I");
             cmd.Parameters.AddWithValue("@customergst_customerheader_gid", custid);
             cmd.Parameters.AddWithValue("@customergst_gid ", "0");
             cmd.Parameters.AddWithValue("@customergst_gst_app",'N');
             cmd.Parameters.AddWithValue("@customergst_gstin",'-');
             cmd.Parameters.AddWithValue("@customergst_vertical", Classifications.customergst_vertical);
             cmd.Parameters.AddWithValue("@customergst_state_gid", Convert.ToInt32(Classifications.customergst_stateid));
             cmd.Parameters.AddWithValue("@customergst_status", 'D');
             cmd.Parameters.AddWithValue("@customergst_isremoved", "D");
             cmd.Parameters.AddWithValue("@customergst_insertby", Convert.ToInt32(objCmnFunctions.GetLoginUserGid()));
             cmd.CommandType = CommandType.StoredProcedure;
             cmd.ExecuteNonQuery();
             res5 = "no";
             
         }
         if (res5 == "YES")
         {
             Int64 supid = GetCustomerheader();
             if (supid > 0)
             {
                 GetConnection();
                 CommonIUD comms = new CommonIUD();
                 SqlCommand cmds = new SqlCommand("PR_IEM_TCUSTOMERGSTINSERT", con);
                 cmds.Parameters.AddWithValue("@StatementType", "E");
                 cmds.Parameters.AddWithValue("@customergst_gid ", "0");
                 cmds.Parameters.AddWithValue("@customergst_customerheader_gid", supid);
                 cmds.Parameters.AddWithValue("@customergst_gstin", Classifications.customergst_tin);
                 cmds.Parameters.AddWithValue("@customergst_vertical", Classifications.customergst_vertical);
                 cmds.Parameters.AddWithValue("@customergst_state_gid", Convert.ToInt32(Classifications.customergst_stateid));
                 cmds.Parameters.AddWithValue("@customergst_updateby", Convert.ToInt32(objCmnFunctions.GetLoginUserGid()));
                 cmds.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                 cmds.CommandType = CommandType.StoredProcedure;
                 cmds.ExecuteNonQuery();
                 string res1 = cmds.Parameters["@res"].Value.ToString();
                 if (res1 == "Not There")
                 {
                     CommonIUD comm = new CommonIUD();
                     SqlCommand cmd = new SqlCommand("PR_IEM_TCUSTOMERGSTINSERT", con);
                     GetConnection();
                     cmd.Parameters.AddWithValue("@StatementType", "I");
                     cmd.Parameters.AddWithValue("@customergst_customerheader_gid", supid);
                     cmd.Parameters.AddWithValue("@customergst_gid ", "0");
                     cmd.Parameters.AddWithValue("@customergst_gst_app", Classifications.customergst_app);
                     cmd.Parameters.AddWithValue("@customergst_gstin", Classifications.customergst_tin);
                     cmd.Parameters.AddWithValue("@customergst_vertical", Classifications.customergst_vertical);
                     cmd.Parameters.AddWithValue("@customergst_state_gid", Convert.ToInt32(Classifications.customergst_stateid));
                     cmd.Parameters.AddWithValue("@customergst_status", Classifications.customergst_status );
                     cmd.Parameters.AddWithValue("@customergst_isremoved", "N");
                     cmd.Parameters.AddWithValue("@customergst_insertby", Convert.ToInt32(objCmnFunctions.GetLoginUserGid()));
                     cmd.CommandType = CommandType.StoredProcedure;
                     cmd.ExecuteNonQuery();
                     result = "success";
                 }
                 else
                 {
                     result = res1;
                 }
             }
             else
             {
                 result = "Session Expired! please select again Vendor";
             }
         }
         else
         {
             result = "State Code Incorrect";
         }
         return result;
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

 public List<EntityGstCustomer> getcustomer()
 {
     try
     {
         List<EntityGstCustomer> objOrgType = new List<EntityGstCustomer>();
         EntityGstCustomer objModel;
         GetConnection();
         DataTable dt = new DataTable();
         SqlCommand cmd = new SqlCommand("PR_IEM_TCUSTOMERGSTSELECT ", con);
         cmd.Parameters.AddWithValue("@customergst_customerheader_gid", Convert.ToInt64(HttpContext.Current.Session["CustomerHeaderGid"]));
         cmd.Parameters.AddWithValue("@StatementType", "SEL");
         cmd.CommandType = CommandType.StoredProcedure;
         SqlDataAdapter da = new SqlDataAdapter(cmd);
         da = new SqlDataAdapter(cmd);
         da.Fill(dt);
         for (int i = 0; i < dt.Rows.Count; i++)
         {
             objModel = new EntityGstCustomer();

             objModel.customergst_gid = Convert.ToInt32(dt.Rows[i]["customergst_gid"]);
             objModel.customergst_app = dt.Rows[i]["customergst_gst_app"].ToString();
             objModel.customergst_stateid = Convert.ToInt32(dt.Rows[i]["customergst_state_gid"]);
             objModel.customergst_state = dt.Rows[i]["state_name"].ToString();
             objModel.customergst_tin = dt.Rows[i]["customergst_gstin"].ToString();
             objModel.customergst_vertical = dt.Rows[i]["customergst_vertical"].ToString();
             objModel.customergst_status = dt.Rows[i]["customergst_status"].ToString();
             objOrgType.Add(objModel);
         }
         return objOrgType;
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


 public EntityGstCustomer getCustomerid(int id)
 {

     try
     {
         List<EntityGstCustomer> objOrgType = new List<EntityGstCustomer>();
         EntityGstCustomer objModel = new EntityGstCustomer();
         GetConnection();
         DataTable dt = new DataTable();

         SqlCommand cmd = new SqlCommand("PR_IEM_TCUSTOMERGSTSELECT", con);
         cmd.Parameters.AddWithValue("@StatementType", "SID");
         cmd.Parameters.AddWithValue("@customergst_gid", id);
         cmd.CommandType = CommandType.StoredProcedure;
         SqlDataAdapter da = new SqlDataAdapter(cmd);
         da = new SqlDataAdapter(cmd);
         da.Fill(dt);
         for (int i = 0; i < dt.Rows.Count; i++)
         {
             objModel.customergst_gid = Convert.ToInt32(dt.Rows[i]["customergst_gid"]);
             objModel.customergst_app = dt.Rows[i]["customergst_gst_app"].ToString();
             objModel.customergst_stateid = Convert.ToInt32(dt.Rows[i]["customergst_state_gid"]);
             objModel.customergst_state = dt.Rows[i]["state_name"].ToString();
             objModel.customergst_tin = dt.Rows[i]["customergst_gstin"].ToString();
             objModel.customergst_vertical = dt.Rows[i]["customergst_vertical"].ToString();
             objModel.customergst_status = dt.Rows[i]["customergst_status"].ToString();
             objModel.selectedstate_gid = Convert.ToInt32(dt.Rows[i]["customergst_state_gid"]);
         }

         return objModel;
     }
     catch (Exception ex)
     {
         throw ex;
     }
     finally
     {
         con.Close();
     }
     // throw new NotImplementedException();
 }

 public string Updatecustomer(EntityGstCustomer Classifications)
 {
     string result;
     // throw new NotImplementedException();            
     try
     {
         CommonIUD commn = new CommonIUD();
         SqlCommand cmdn = new SqlCommand("PR_IEM_TCUSTOMERGSTINSERT", con);
         GetConnection();
         string res5 = "YES";
         if (Classifications.customergst_app == "Y")
         {
             string ss = Classifications.customergst_tin.Substring(0, 2);// Classifications.suppliergst_tin.Split(new char[] { ' ' }, 2);
             cmdn.Parameters.AddWithValue("@StatementType", "C");
             cmdn.Parameters.AddWithValue("@state_code", ss);
             cmdn.Parameters.AddWithValue("@customergst_state_gid", Convert.ToInt32(Classifications.customergst_stateid));
             cmdn.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
             cmdn.CommandType = CommandType.StoredProcedure;
             cmdn.ExecuteNonQuery();
             res5 = cmdn.Parameters["@res"].Value.ToString();
         }
         else
         {
             Classifications.customergst_tin = "";
         }
         if (res5 == "YES")
         {
             Int64 supid = GetCustomerheader();
             if (supid > 0)
             {
                 CommonIUD comms = new CommonIUD();
                 SqlCommand cmds = new SqlCommand("PR_IEM_TCUSTOMERGSTINSERT", con);
                 GetConnection();
                 cmds.Parameters.AddWithValue("@StatementType", "E");
                 cmds.Parameters.AddWithValue("@customergst_gid ", Classifications.customergst_gid);
                 cmds.Parameters.AddWithValue("@customergst_gstin", Classifications.customergst_tin);
                 cmds.Parameters.AddWithValue("@customergst_customerheader_gid", Convert.ToInt64(HttpContext.Current.Session["CustomerHeaderGid"]));
                 cmds.Parameters.AddWithValue("@customergst_vertical", Classifications.customergst_vertical);
                 cmds.Parameters.AddWithValue("@customergst_state_gid", Convert.ToInt32(Classifications.customergst_stateid));
                 cmds.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                 cmds.CommandType = CommandType.StoredProcedure;
                 cmds.ExecuteNonQuery();
                 string res1 = cmds.Parameters["@res"].Value.ToString();
                 if (res1 == "Not There")
                 {
                     CommonIUD comm = new CommonIUD();
                     SqlCommand cmd = new SqlCommand("PR_IEM_TCUSTOMERGSTINSERT", con);
                     GetConnection();
                     cmd.Parameters.AddWithValue("@StatementType", "U");
                     cmd.Parameters.AddWithValue("@customergst_customerheader_gid", supid);
                     cmd.Parameters.AddWithValue("@customergst_gid", Classifications.customergst_gid);
                     cmd.Parameters.AddWithValue("@customergst_gst_app", Classifications.customergst_app);
                     cmd.Parameters.AddWithValue("@customergst_gstin", Classifications.customergst_tin);
                     cmd.Parameters.AddWithValue("@customergst_vertical", Classifications.customergst_vertical);
                     cmd.Parameters.AddWithValue("@customergst_state_gid", Convert.ToInt32(Classifications.customergst_stateid));
                     cmd.Parameters.AddWithValue("@customergst_status", Classifications.customergst_status);
                     cmd.Parameters.AddWithValue("@customergst_isremoved", "N");
                     cmd.Parameters.AddWithValue("@approved_status", Classifications.IsChecker);
                     cmd.Parameters.AddWithValue("@customergst_updateby", Convert.ToInt32(objCmnFunctions.GetLoginUserGid()));
                     cmd.CommandType = CommandType.StoredProcedure;
                     cmd.ExecuteNonQuery();
                     result = "success";
                 }
                 else
                 {
                     result = res1;
                 }
             }
             else
             {
                 result = "Session Expired! please select again Vendor";
             }
         }
         else
         {
             result = "State Code Incorrect";
         }
         return result;
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

 public long GetCustomerheader()
 {
     Int64 CustHeaderGid = 0;
     try
     {
         CustHeaderGid = Convert.ToInt64(HttpContext.Current.Session["CustomerHeaderGid"]);
     }
     catch (Exception ex)
     {
         objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
         throw ex;
     }
     finally
     {
         //con.Close();
     }
     return CustHeaderGid;
 }


 public string ApproveCustomerMasterDetail()
 {
     try
     {
         Int64 customergid = GetCustomerheader();
         GetConnection();
         CommonIUD comms = new CommonIUD();
         SqlCommand cmd = new SqlCommand("PR_IEM_TCUSTOMERGSTINSERT", con);
         cmd.CommandType = CommandType.StoredProcedure;
         cmd.Parameters.AddWithValue("@customergst_customerheader_gid", customergid);
         cmd.Parameters.AddWithValue("@StatementType","Approve");
         cmd.Parameters.AddWithValue("@customergst_updateby", Convert.ToInt32(objCmnFunctions.GetLoginUserGid()));
         cmd.ExecuteNonQuery();
     }
     catch(Exception ex)
     {
         objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
     }
     finally
     {
         con.Close();
     }
     return "success";
 }

    }
}