using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using IEM.Common;

namespace IEM.Areas.ASMS.Models
{
    public class SupDataModel : IRepository
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();

        CmnFunctions objCmnFunctions = new CmnFunctions();
        CommonIUD objCommonIUD = new CommonIUD();
        ErrorLog objErrorLog = new ErrorLog();

        public int pageMode = 0;
        private void GetPageMode()
        {
            try
            {
                string queryname = "";
                pageMode = Convert.ToInt32(HttpContext.Current.Session["PageMode"] == null ? "0" : HttpContext.Current.Session["PageMode"].ToString());
                if (pageMode == 4 || pageMode == 6 || pageMode == 5)
                {
                    pageMode = 2;
                }
                 
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
        }
        private void GetConnection()
        {
            string queryname = "";
            try
            {
                pageMode = Convert.ToInt32(HttpContext.Current.Session["PageMode"] == null ? "0" : HttpContext.Current.Session["PageMode"].ToString());
                if (pageMode == 4 || pageMode == 6 || pageMode == 5)
                {
                    pageMode = 2;
                }
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

        public IEnumerable<DirectorModel> GetDirector()
        {
            List<DirectorModel> objDirector = new List<DirectorModel>();
            try
            {
                DirectorModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_tDirectors", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@createdby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new DirectorModel();
                    objModel._DirectorsID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["directors_gid"].ToString()) ? "0" : dt.Rows[i]["directors_gid"].ToString());
                    objModel._DirectorsName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["directors_name"].ToString()) ? "" : dt.Rows[i]["directors_name"].ToString());
                    objDirector.Add(objModel);
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
            return objDirector;
        }

        public DirectorModel GetDirectorById(int DirectorId)
        {
            throw new NotImplementedException();
        }

        public void InsertDirector(DirectorModel Directors)
        {

            try
            {
                //GetConnection();
                //cmd = new SqlCommand("asms_trn_Directors", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                //cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = Directors._DirectorsName;
                //cmd.Parameters.Add("@createdby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                //cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "insert";
                //cmd.ExecuteNonQuery();
                GetPageMode();
                string tname = "";
                if (pageMode == 1 || pageMode == 2 || pageMode == 3)
                {
                    tname = "asms_tmp_tdirectors";
                }
                else
                {
                    tname = "asms_trn_tdirectors";
                }


                if (HttpContext.Current.Session["SupplierHeaderGid"] != null && Convert.ToString(HttpContext.Current.Session["SupplierHeaderGid"]).Trim() != "")
                {
                    string[,] codes = new string[,]
                   {
                     {"directors_name",Convert.ToString(string.IsNullOrEmpty(Directors._DirectorsName)?"":Directors._DirectorsName) },
                     {"directors_supplierheader_gid", Convert.ToString(HttpContext.Current.Session["SupplierHeaderGid"]) },
                     {"directors_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"directors_insert_date","sysdatetime()" },
                    
                   };
                    string insertcommand = objCommonIUD.InsertCommon(codes, "asms_tmp_tdirectors");
                    GetConnection();
                    cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                    cmd.Parameters.Add("@insertby", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid();
                    cmd.Parameters.Add("@table", SqlDbType.VarChar).Value = "directors";
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "update";
                    int tmp_gid = Convert.ToInt32(cmd.ExecuteScalar());

                    cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                    string sup_trn_gid = Convert.ToString(cmd.ExecuteScalar());

                    string[,] codes2 = new string[,]
                   {
                     {"directors_name",Convert.ToString(string.IsNullOrEmpty(Directors._DirectorsName)?"":Directors._DirectorsName) },
                     {"directors_supplierheader_gid", sup_trn_gid},
                     {"directors_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"directors_insert_date","sysdatetime()" },
                     {"directors_isremoved","Y" },
                     {"directors_sup_id",tmp_gid.ToString()}
                   };
                    string insertcommand2 = objCommonIUD.InsertCommon(codes2, "asms_trn_tdirectors");
                }
                else if (HttpContext.Current.Session["SupplierHeaderGid"] == null)
                {
                    string[,] codes = new string[,]
                   {
                     {"directors_name",Convert.ToString(string.IsNullOrEmpty(Directors._DirectorsName)?"":Directors._DirectorsName) },
                     {"directors_supplierheader_gid", "0" },
                     {"directors_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"directors_insert_date","sysdatetime()" },
                    
                   };
                    string insertcommand = objCommonIUD.InsertCommon(codes, "asms_tmp_tdirectors");

                    GetConnection();
                    cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@supid", SqlDbType.Int).Value = "0";
                    cmd.Parameters.Add("@insertby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                    cmd.Parameters.Add("@table", SqlDbType.Int).Value = "directors";
                    cmd.Parameters.Add("@action", SqlDbType.Int).Value = "update";
                    int tmp_gid = Convert.ToInt32(cmd.ExecuteScalar());
                    string[,] codes2 = new string[,]
                   {
                     {"directors_name",Convert.ToString(string.IsNullOrEmpty(Directors._DirectorsName)?"":Directors._DirectorsName) },
                     {"directors_supplierheader_gid", "0" },
                     {"directors_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"directors_insert_date","sysdatetime()" },
                     {"directors_isremoved","Y" },
                     {"directors_sup_id",tmp_gid.ToString()}
                   };
                    string insertcommand2 = objCommonIUD.InsertCommon(codes2, "asms_trn_tdirectors");
                }



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

        public void DeleteDirector(int DirectorId)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tDirectors", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = DirectorId;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
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

        public void UpdateDirector(DirectorModel Directors)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tDirectors", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = Directors._DirectorsID;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(Directors._DirectorsName) ? "" : Directors._DirectorsName);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "edit";
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

        public string DirectorIsExists(string Director)
        {
            throw new NotImplementedException();
        }

        public List<SupplierHeader> GetOrganizationType()
        {
            List<SupplierHeader> lstOrganizationType = new List<SupplierHeader>();
            try
            {
                SupplierHeader objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_OrganizationType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da.SelectCommand = cmd;
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierHeader();
                    objModel._OrganizationTypeID = Convert.ToInt32(dt.Rows[i]["organizationtype_gid"].ToString());
                    objModel._OrganizationTypeName = Convert.ToString(dt.Rows[i]["organizationtype_name"].ToString());
                    lstOrganizationType.Add(objModel);
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
            return lstOrganizationType;
        }

        public List<SupplierHeader> GetServiceType()
        {
            List<SupplierHeader> lstServiceType = new List<SupplierHeader>();
            try
            {
                SupplierHeader objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_ServiceType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da.SelectCommand = cmd;
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierHeader();
                    objModel._ServiceTypeID = Convert.ToInt32(dt.Rows[i]["servicetype_gid"].ToString());
                    objModel._ServiceTypeName = Convert.ToString(dt.Rows[i]["servicetype_name"].ToString());
                    lstServiceType.Add(objModel);
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
            return lstServiceType;
        }

        public List<SupplierHeader> GetContactType()
        {
            List<SupplierHeader> lstServiceType = new List<SupplierHeader>();
            try
            {
                SupplierHeader objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getcontacttype";
                da.SelectCommand = cmd;
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierHeader();
                    objModel._SupContactTypeID = Convert.ToInt32(dt.Rows[i]["contacttype_gid"].ToString());
                    objModel._SupContactTypeName = Convert.ToString(dt.Rows[i]["contacttype_name"].ToString());
                    lstServiceType.Add(objModel);
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
            return lstServiceType;
        }

        public IEnumerable<CustomersModel> GetCustomer()
        {
            List<CustomersModel> objCustomer = new List<CustomersModel>();
            try
            {
                CustomersModel objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_tcustomer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new CustomersModel();
                    objModel._CustomerID = Convert.ToInt32(dt.Rows[i]["customer_gid"].ToString() == null ? "0" : dt.Rows[i]["customer_gid"].ToString());
                    objModel._CustomerName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["customer_name"].ToString()) ? "" : dt.Rows[i]["customer_name"].ToString());
                    objModel._CustomerServiceName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["customer_servicename"].ToString()) ? "" : dt.Rows[i]["customer_servicename"].ToString());
                    objModel._CustomerContactPerson = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["customer_contactpersonname"].ToString()) ? "" : dt.Rows[i]["customer_contactpersonname"].ToString());
                    objModel._CustomerMobileNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["customer_mobileno"].ToString()) ? "" : dt.Rows[i]["customer_mobileno"].ToString());
                    objModel._CustomerPhoneNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["customer_phoneno"].ToString()) ? "" : dt.Rows[i]["customer_phoneno"].ToString());
                    objModel._CustomerAgeOfProduct = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["customer_ageofproduct"].ToString()) ? "" : dt.Rows[i]["customer_ageofproduct"].ToString());
                    objCustomer.Add(objModel);
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
            return objCustomer;
        }

        public CustomersModel GetCustomerById(int CustomerId)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_tcustomer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = CustomerId;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new CustomersModel()
                {
                    _CustomerID = Convert.ToInt32(dt.Rows[0]["customer_gid"].ToString()),
                    _CustomerName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[0]["customer_name"].ToString()) ? "" : dt.Rows[0]["customer_name"].ToString()),
                    _CustomerServiceName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[0]["customer_servicename"].ToString()) ? "" : dt.Rows[0]["customer_servicename"].ToString()),
                    _CustomerContactPerson = Convert.ToString(string.IsNullOrEmpty(dt.Rows[0]["customer_contactpersonname"].ToString()) ? "" : dt.Rows[0]["customer_contactpersonname"].ToString()),
                    _CustomerMobileNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[0]["customer_mobileno"].ToString()) ? "" : dt.Rows[0]["customer_mobileno"].ToString()),
                    _CustomerPhoneNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[0]["customer_phoneno"].ToString()) ? "" : dt.Rows[0]["customer_phoneno"].ToString()),
                    _CustomerAgeOfProduct = Convert.ToString(string.IsNullOrEmpty(dt.Rows[0]["customer_ageofproduct"].ToString()) ? "" : dt.Rows[0]["customer_ageofproduct"].ToString())
                };
                return model;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                CustomersModel objMod = new CustomersModel();
                return objMod;
            }
            finally
            {

            }
        }

        public void InsertCustomer(CustomersModel Customers)
        {
            try
            {
                //GetConnection();
                //cmd = new SqlCommand("asms_trn_customer", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                //cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = Customers._CustomerName;
                //cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                //cmd.Parameters.Add("@servicename", SqlDbType.VarChar).Value = Customers._CustomerServiceName;
                //cmd.Parameters.Add("@contactpersonname", SqlDbType.VarChar).Value = Customers._CustomerContactPerson;
                //cmd.Parameters.Add("@mobileno", SqlDbType.VarChar).Value = Customers._CustomerMobileNo;
                //cmd.Parameters.Add("@phoneno", SqlDbType.VarChar).Value = Customers._CustomerPhoneNo;
                //cmd.Parameters.Add("@createdby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                //cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "insert";
                //cmd.ExecuteNonQuery();

                string[,] codes = new string[,]
                   {
                     {"customer_name",Customers._CustomerName },
                     {"customer_supplierheader_gid", Convert.ToString(HttpContext.Current.Session["SupplierHeaderGid"]) },
                     {"customer_servicename",Convert.ToString(string.IsNullOrEmpty(Customers._CustomerServiceName)?"":Customers._CustomerServiceName) },
                     {"customer_contactpersonname",Convert.ToString(string.IsNullOrEmpty(Customers._CustomerContactPerson)?"":Customers._CustomerContactPerson) },
                     {"customer_mobileno",Convert.ToString(string.IsNullOrEmpty(Customers._CustomerMobileNo)?"":Customers._CustomerMobileNo) },
                     {"customer_phoneno",Convert.ToString(string.IsNullOrEmpty(Customers._CustomerPhoneNo)?"":Customers._CustomerPhoneNo) },
                     {"customer_ageofproduct",Convert.ToString(string.IsNullOrEmpty(Customers._CustomerAgeOfProduct)?"":Customers._CustomerAgeOfProduct) },
                     {"customer_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"customer_insert_date","sysdatetime()" }
                   };
                //GetPageMode();
                //string tname = "";
                //if (pageMode == 1 || pageMode == 2 || pageMode == 3)
                //{
                //    tname = "asms_tmp_tcustomer";
                //}
                //else
                //{
                //    tname = "asms_trn_tcustomer";
                //}
                string insertcommand = objCommonIUD.InsertCommon(codes, "asms_tmp_tcustomer");
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@insertby", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@table", SqlDbType.VarChar).Value = "customer";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "update";
                int tmp_gid = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                string sup_trn_gid = Convert.ToString(cmd.ExecuteScalar());

                string[,] codes2 = new string[,]
                   {
                     {"customer_name",Customers._CustomerName },
                     {"customer_supplierheader_gid", sup_trn_gid },
                     {"customer_servicename",Convert.ToString(string.IsNullOrEmpty(Customers._CustomerServiceName)?"":Customers._CustomerServiceName) },
                     {"customer_contactpersonname",Convert.ToString(string.IsNullOrEmpty(Customers._CustomerContactPerson)?"":Customers._CustomerContactPerson) },
                     {"customer_mobileno",Convert.ToString(string.IsNullOrEmpty(Customers._CustomerMobileNo)?"":Customers._CustomerMobileNo) },
                     {"customer_phoneno",Convert.ToString(string.IsNullOrEmpty(Customers._CustomerPhoneNo)?"":Customers._CustomerPhoneNo) },
                     {"customer_ageofproduct",Convert.ToString(string.IsNullOrEmpty(Customers._CustomerAgeOfProduct)?"":Customers._CustomerAgeOfProduct) },
                     {"customer_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"customer_insert_date","sysdatetime()" },
                     {"customer_isremoved","Y" },
                      {"customer_sup_id",tmp_gid.ToString() }
                   };

                string insertcommand2 = objCommonIUD.InsertCommon(codes2, "asms_trn_tcustomer");

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

        public void DeleteCustomer(int CustomerId)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tcustomer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = CustomerId;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "delete";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
        }

        public void UpdateCustomer(CustomersModel Customers)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tcustomer", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = Customers._CustomerID;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = Customers._CustomerName;
                cmd.Parameters.Add("@servicename", SqlDbType.VarChar).Value = Customers._CustomerServiceName;
                cmd.Parameters.Add("@contactpersonname", SqlDbType.VarChar).Value = Customers._CustomerContactPerson;
                cmd.Parameters.Add("@mobileno", SqlDbType.VarChar).Value = Customers._CustomerMobileNo;
                cmd.Parameters.Add("@phoneno", SqlDbType.VarChar).Value = Customers._CustomerPhoneNo;
                cmd.Parameters.Add("@ageofproduct", SqlDbType.VarChar).Value = Customers._CustomerAgeOfProduct;
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "edit";
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

        public string CustomerIsExists(string Customer)
        {
            throw new NotImplementedException();
        }

        public void InsertSupplierHeader(SupplierHeader supHeader)
        {
            try
            {
                string contractfrom = Convert.ToString(string.IsNullOrEmpty(supHeader._ContractFrom) ? "" : objCmnFunctions.convertoDateTimeStringasms(supHeader._ContractFrom));
                string contractto = Convert.ToString(string.IsNullOrEmpty(supHeader._ContractTo) ? "" : objCmnFunctions.convertoDateTimeStringasms(supHeader._ContractTo));
                //if (contractfrom != null && contractto != null)
                //{
                string[,] codes = new string[,]
                   {
                     {"supplierheader_suppliertype",Convert.ToString((supHeader._SupplierType)==null?"":supHeader._SupplierType) },
                     {"supplierheader_suppliercode",Convert.ToString((supHeader._SupplierCode)==null?"":supHeader._SupplierCode) },
                     {"supplierheader_name",Convert.ToString((supHeader._SupplierName)==null?"":supHeader._SupplierName) },
                     {"supplierheader_panno",Convert.ToString((supHeader._PanNo)==null?"":supHeader._PanNo) },
                     {"supplierheader_organizationtype_gid",Convert.ToString(supHeader.selectedOrganizationID) },
                     {"supplierheader_servicetype_gid",Convert.ToString(supHeader.selectedServiceID) },
                     {"supplierheader_companyregno",Convert.ToString((supHeader._CompanyRegNo)==null?"":supHeader._CompanyRegNo) },
                     {"supplierheader_contractfrom",contractfrom },
                     {"supplierheader_contractto",contractto },
                     {"supplierheader_website",Convert.ToString((supHeader._website)==null?"":supHeader._website) },
                     {"supplierheader_emailid",Convert.ToString((supHeader._EmailID)==null?"":supHeader._EmailID) },
                     {"supplierheader_approxspend",Convert.ToString(objCmnFunctions.convertoDecimal(supHeader._ApproxSpend)) },
                     {"supplierheader_actualspend",Convert.ToString(objCmnFunctions.convertoDecimal(supHeader._ActualSpend)) },
                     {"supplierheader_suppliercategory_gid",Convert.ToString(supHeader.SelectedSupplierCategoryID) },
                     {"supplierheader_isactivecontract",Convert.ToString(supHeader._IsActiveContract) },
                     {"supplierheader_reasonfornocontract",Convert.ToString((supHeader._ReasonForNoContract)==null?"":supHeader._ReasonForNoContract) },
                     {"supplierheader_requesttype",Convert.ToString(supHeader._RequestType) },
                     {"supplierheader_requeststatus",Convert.ToString(supHeader._Requeststatus) },
                     {"supplierheader_status",Convert.ToString(supHeader._SupplierStatus) },
                     {"supplierheader_owner_gid",Convert.ToString(supHeader._OwnerId) },
                     {"supplierheader_contacttype_gid",Convert.ToString(supHeader.SelectedSupContactTypeID) },
                     {"supplierheader_renewaldate",Convert.ToString(objCmnFunctions.convertoDateTimeString(supHeader._RenewalDate)) },
                     {"supplierheader_remarks",Convert.ToString((supHeader._SupplierTypeRemarks)==null?"":supHeader._SupplierTypeRemarks) },
                     {"supplierheader_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"supplierheader_insert_date","sysdatetime()" }
                   };
                //  GetPageMode();
                //string tname = "";
                //if (pageMode == 1 || pageMode == 2 || pageMode == 3)
                //{
                //    tname = "asms_tmp_tsupplierheader";
                //}
                //else
                //{
                //    tname = "asms_trn_tsupplierheader";
                //}
                string[,] codes2 = new string[,]
                   {
                     {"supplierheader_suppliertype",Convert.ToString((supHeader._SupplierType)==null?"":supHeader._SupplierType) },
                     {"supplierheader_suppliercode",Convert.ToString((supHeader._SupplierCode)==null?"":supHeader._SupplierCode) },
                     {"supplierheader_name",Convert.ToString((supHeader._SupplierName)==null?"":supHeader._SupplierName) },
                     {"supplierheader_panno",Convert.ToString((supHeader._PanNo)==null?"":supHeader._PanNo) },
                     {"supplierheader_organizationtype_gid",Convert.ToString(supHeader.selectedOrganizationID) },
                     {"supplierheader_servicetype_gid",Convert.ToString(supHeader.selectedServiceID) },
                     {"supplierheader_companyregno",Convert.ToString((supHeader._CompanyRegNo)==null?"":supHeader._CompanyRegNo) },
                     {"supplierheader_contractfrom",contractfrom },
                     {"supplierheader_contractto",contractto },
                     {"supplierheader_website",Convert.ToString((supHeader._website)==null?"":supHeader._website) },
                     {"supplierheader_emailid",Convert.ToString((supHeader._EmailID)==null?"":supHeader._EmailID) },
                     {"supplierheader_approxspend",Convert.ToString(objCmnFunctions.convertoDecimal(supHeader._ApproxSpend)) },
                     {"supplierheader_actualspend",Convert.ToString(objCmnFunctions.convertoDecimal(supHeader._ActualSpend)) },
                     {"supplierheader_suppliercategory_gid",Convert.ToString(supHeader.SelectedSupplierCategoryID) },
                     {"supplierheader_isactivecontract",Convert.ToString(supHeader._IsActiveContract) },
                     {"supplierheader_reasonfornocontract",Convert.ToString((supHeader._ReasonForNoContract)==null?"":supHeader._ReasonForNoContract) },
                     {"supplierheader_requesttype",Convert.ToString(supHeader._RequestType) },
                     {"supplierheader_requeststatus",Convert.ToString(supHeader._Requeststatus) },
                     {"supplierheader_status",Convert.ToString(supHeader._SupplierStatus) },
                     {"supplierheader_owner_gid",Convert.ToString(supHeader._OwnerId) },
                     {"supplierheader_contacttype_gid",Convert.ToString(supHeader.SelectedSupContactTypeID) },
                     {"supplierheader_renewaldate",Convert.ToString(objCmnFunctions.convertoDateTimeString(supHeader._RenewalDate)) },
                     {"supplierheader_remarks",Convert.ToString((supHeader._SupplierTypeRemarks)==null?"":supHeader._SupplierTypeRemarks) },
                     {"supplierheader_isremoved","Y" },
                     {"supplierheader_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"supplierheader_insert_date","sysdatetime()" }
                   };
                string insertcommand = objCommonIUD.InsertCommon(codes, "asms_tmp_tsupplierheader");
                string insertcommand1 = objCommonIUD.InsertCommon(codes2, "asms_trn_tsupplierheader");
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

        public void DeleteSupplierHeader(int SupHeaderId)
        {
            throw new NotImplementedException();
        }

        public void UpdateSupplierHeader(SupplierHeader supHeader)
        {
            //GetConnection();
            //cmd = new SqlCommand("pr_asms_trn_tsupplierheader", con);
            //cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.Add("@supid", SqlDbType.Int).Value = Convert.ToUInt32(HttpContext.Current.Session["SupplierHeaderGid"]);
            //cmd.Parameters.Add("@supcode", SqlDbType.VarChar).Value = supHeader._SupplierCode;
            //cmd.Parameters.Add("@supname", SqlDbType.VarChar).Value =Convert.ToString(string.IsNullOrEmpty(supHeader._SupplierName)?"":supHeader._SupplierName);
            //cmd.Parameters.Add("@suptype", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(supHeader._SupplierType)?"":supHeader._SupplierType);
            //cmd.Parameters.Add("@suporganizationtypeid", SqlDbType.Int).Value = supHeader.selectedOrganizationID;
            //cmd.Parameters.Add("@supservicetypeid", SqlDbType.Int).Value = supHeader.selectedServiceID;
            //cmd.Parameters.Add("@supcompanyregno", SqlDbType.VarChar).Value =  Convert.ToString(string.IsNullOrEmpty(supHeader._CompanyRegNo)?"":supHeader._CompanyRegNo);
            //cmd.Parameters.Add("@supcontractfrom", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(supHeader._ContractFrom)?"":objCmnFunctions.convertoDateTimeString(supHeader._ContractFrom));
            //cmd.Parameters.Add("@supcontractto", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(supHeader._ContractTo) ? "" : objCmnFunctions.convertoDateTimeString(supHeader._ContractTo));
            //cmd.Parameters.Add("@supwebsite", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(supHeader._website)?"":supHeader._website);
            //cmd.Parameters.Add("@supemailid", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(supHeader._EmailID)?"":supHeader._EmailID);
            //cmd.Parameters.Add("@supapproxspend", SqlDbType.Decimal).Value = objCmnFunctions.convertoDecimal(supHeader._ApproxSpend);
            //cmd.Parameters.Add("@supactualspend", SqlDbType.Decimal).Value =  objCmnFunctions.convertoDecimal(supHeader._ActualSpend);
            //cmd.Parameters.Add("@supcategoryid", SqlDbType.Int).Value = supHeader.SelectedSupplierCategoryID;
            //cmd.Parameters.Add("@supisactivecontract", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(supHeader._IsActiveContract)?"":supHeader._IsActiveContract);
            //cmd.Parameters.Add("@supreasonfornocontract", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(supHeader._ReasonForNoContract)?"":supHeader._ReasonForNoContract);
            //cmd.Parameters.Add("@suprequesttype", SqlDbType.VarChar).Value =  Convert.ToString(string.IsNullOrEmpty(supHeader._RequestType)?"":supHeader._RequestType);
            //cmd.Parameters.Add("@suprequeststatus", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(supHeader._Requeststatus)?"":supHeader._Requeststatus);
            //cmd.Parameters.Add("@supstatus", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(supHeader._SupplierStatus)?"":supHeader._SupplierStatus);
            //cmd.Parameters.Add("@supownergid", SqlDbType.Int).Value = supHeader._OwnerId;
            //cmd.Parameters.Add("@supcontacttypegid", SqlDbType.Int).Value = supHeader.SelectedSupContactTypeID;
            //cmd.Parameters.Add("@suprenewaldate", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(supHeader._RenewalDate)?"":objCmnFunctions.ddMMyyyyString(supHeader._RenewalDate));
            //cmd.Parameters.Add("@supcreatedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
            //cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
            //cmd.Parameters.Add("@suptouchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
            //cmd.Parameters.Add("@suptouchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
            //cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "edit";

            //cmd.Parameters.Add("@res", SqlDbType.VarChar, 5);
            //cmd.Parameters["@res"].Direction = ParameterDirection.Output;
            //cmd.ExecuteNonQuery();
            //string res = Convert.ToString(cmd.Parameters["@res"].Value.ToString());
            try
            {

                string contractfrom = Convert.ToString(string.IsNullOrEmpty(supHeader._ContractFrom) ? "" : objCmnFunctions.convertoDateTimeStringasms(supHeader._ContractFrom));

                string contractto = Convert.ToString(string.IsNullOrEmpty(supHeader._ContractTo) ? "" : objCmnFunctions.convertoDateTimeStringasms(supHeader._ContractTo));
                //if (contractfrom != null && contractto != null)
                //{
                string[,] codes = new string[,]
                   {
                     {"supplierheader_suppliertype",Convert.ToString((supHeader._SupplierType)==null?"":supHeader._SupplierType) },
                  //   {"supplierheader_suppliercode",Convert.ToString((supHeader._SupplierCode)==null?"":supHeader._SupplierCode) },
                     {"supplierheader_name",Convert.ToString((supHeader._SupplierName)==null?"":supHeader._SupplierName) },
                     {"supplierheader_panno",Convert.ToString((supHeader._PanNo)==null?"":supHeader._PanNo) },
                     {"supplierheader_organizationtype_gid",Convert.ToString(supHeader.selectedOrganizationID) },
                     {"supplierheader_servicetype_gid",Convert.ToString(supHeader.selectedServiceID) },
                     {"supplierheader_companyregno",Convert.ToString((supHeader._CompanyRegNo)==null?"":supHeader._CompanyRegNo) },
                     {"supplierheader_contractfrom",contractfrom},
                     {"supplierheader_contractto",contractto },
                     {"supplierheader_website",Convert.ToString((supHeader._website)==null?"":supHeader._website) },
                     {"supplierheader_emailid",Convert.ToString((supHeader._EmailID)==null?"":supHeader._EmailID) },
                     {"supplierheader_approxspend",Convert.ToString(objCmnFunctions.convertoDecimal(supHeader._ApproxSpend)) },
                     {"supplierheader_actualspend",Convert.ToString(objCmnFunctions.convertoDecimal(supHeader._ActualSpend)) },
                     {"supplierheader_suppliercategory_gid",Convert.ToString(supHeader.SelectedSupplierCategoryID) },
                     {"supplierheader_isactivecontract",Convert.ToString(supHeader._IsActiveContract) },
                     {"supplierheader_reasonfornocontract",Convert.ToString((supHeader._ReasonForNoContract)==null?"":supHeader._ReasonForNoContract) },
                     {"supplierheader_requesttype",Convert.ToString(string.IsNullOrEmpty(supHeader._RequestType)?"":supHeader._RequestType)  },
                     {"supplierheader_requeststatus",Convert.ToString(string.IsNullOrEmpty(supHeader._Requeststatus)?"":supHeader._Requeststatus) },
                    // {"supplierheader_status",Convert.ToString(string.IsNullOrEmpty(supHeader._SupplierStatus)?"":supHeader._SupplierStatus) },
                     {"supplierheader_owner_gid",Convert.ToString(supHeader._OwnerId) },
                     {"supplierheader_contacttype_gid",Convert.ToString(supHeader.SelectedSupContactTypeID) },
                     {"supplierheader_renewaldate",Convert.ToString(objCmnFunctions.convertoDateTimeString(supHeader._RenewalDate)) },
                     {"supplierheader_noofyearsinbusiness",Convert.ToString(supHeader._NoofYearsIBusiness) },
                     {"supplierheader_noofyearsofassociation",Convert.ToString(supHeader._NoofYearsOfAssociation) },
                     {"supplierheader_nooffactories",Convert.ToString(supHeader._NoOfFactories) },
                     {"supplierheader_noofoffices",Convert.ToString(supHeader._NoOfOffices) },
                     {"supplierheader_totalnoofemployees",Convert.ToString(supHeader._TotalEmployees) },
                     {"supplierheader_contractemployees",Convert.ToString(supHeader._ContractEmployees) },
                     {"supplierheader_permanentemployees",Convert.ToString(supHeader._PermanentEmployees) },
                     {"supplierheader_issueappointmentletters",Convert.ToString(string.IsNullOrEmpty(supHeader._IssueAppointmentLetters)?"":supHeader._IssueAppointmentLetters) },
                     {"supplierheader_performintegritychecks",Convert.ToString(string.IsNullOrEmpty(supHeader._PerformIntegrityChecks)?"":supHeader._PerformIntegrityChecks) },
                     {"supplierheader_integritycheckdescription",Convert.ToString(string.IsNullOrEmpty(supHeader._IntegrityCheckDesc)?"":supHeader._IntegrityCheckDesc) },
                     {"supplierheader_payminwages",Convert.ToString(string.IsNullOrEmpty(supHeader._Payminwages)?"":supHeader._Payminwages) },
                     {"supplierheader_employlabourbelow18yrs",Convert.ToString(string.IsNullOrEmpty(supHeader._EmployLabourbelow18Yrs)?"":supHeader._EmployLabourbelow18Yrs) },
                     {"supplierheader_emppfregno",Convert.ToString(string.IsNullOrEmpty(supHeader._EmpPFRegNo)?"":supHeader._EmpPFRegNo) },
                     {"supplierheader_shopregno",Convert.ToString(string.IsNullOrEmpty(supHeader._ShopRegNo)?"":supHeader._ShopRegNo) },
                     {"supplierheader_empstateregno",Convert.ToString(string.IsNullOrEmpty(supHeader._EmpStateRegNo)?"":supHeader._EmpStateRegNo) },
                     {"supplierheader_ismsmed",Convert.ToString(string.IsNullOrEmpty(supHeader._TaxisMSMED)?"":supHeader._TaxisMSMED) },
                     {"supplierheader_remarks",Convert.ToString((supHeader._SupplierTypeRemarks)==null?"":supHeader._SupplierTypeRemarks) },
                     {"supplierheader_update_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"supplierheader_update_date","sysdatetime()" }
                   };
                string[,] whrcol = new string[,]
	                 {
                          {"supplierheader_gid",  Convert.ToString(HttpContext.Current.Session["SupplierHeaderGid"])},
                          {"supplierheader_isremoved", "N"}
                     };
                //GetPageMode();
                //string tname = "";
                //if (pageMode == 1 || pageMode == 2 || pageMode == 3)
                //{
                //    tname = "asms_tmp_tsupplierheader";
                //}
                //else
                //{
                //    tname = "asms_trn_tsupplierheader";
                //}
                string insertcommand = objCommonIUD.UpdateCommon(codes, whrcol, "asms_tmp_tsupplierheader");
                //}

            }

            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }

        }

        public List<SupplierHeader> GetSupplierCategory()
        {
            List<SupplierHeader> objSupplierHeader = new List<SupplierHeader>();
            try
            {
                SupplierHeader objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_SupplierCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierHeader();
                    objModel._SupplierCategoryID = Convert.ToInt32(dt.Rows[i]["suppliercategory_gid"].ToString());
                    objModel._SupplierCategoryName = Convert.ToString(dt.Rows[i]["suppliercategory_name"].ToString());
                    objSupplierHeader.Add(objModel);
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
            return objSupplierHeader;
        }

        public List<SearchEmployee> GetEmployeeList()
        {
            List<SearchEmployee> objSearchEmployee = new List<SearchEmployee>();
            try
            {
                SearchEmployee objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getemployeelist";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SearchEmployee();
                    objModel._EmployeeGid = Convert.ToInt32(dt.Rows[i]["employee_gid"].ToString());
                    objModel._EmployeeCode = Convert.ToString(dt.Rows[i]["employee_code"].ToString());
                    objModel._EmployeeName = Convert.ToString(dt.Rows[i]["employee_name"].ToString());
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

        public IEnumerable<SupplierServiceDetails> GetSupplierServiceDetails()
        {
            List<SupplierServiceDetails> objSupplierServiceDetails = new List<SupplierServiceDetails>();
            try
            {
                SupplierServiceDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_tSupplierService", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierServiceDetails();
                    objModel._SupServiceDetailsID = Convert.ToInt32(dt.Rows[i]["service_gid"].ToString());
                    objModel._SupServiceDetailsName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["service_name"].ToString()) ? "" : dt.Rows[i]["service_name"].ToString());
                    objSupplierServiceDetails.Add(objModel);
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
            return objSupplierServiceDetails;

        }

        public long GetSupplierHeaderGid(SupplierHeader supHeader)
        {
            Int64 SupHeaderGid = 0;
            try
            {

                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tsupplierheader", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@supcode", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(supHeader._SupplierCode) ? "" : supHeader._SupplierCode);
                cmd.Parameters.Add("@supcreatedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getsuppliergid";
                SupHeaderGid = Convert.ToInt64(cmd.ExecuteScalar());


            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return SupHeaderGid;
        }

        public int GetDirectorsCount()
        {
            int result = 0;
            try
            {

                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tDirectors", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getcount";
                result = Convert.ToInt32(cmd.ExecuteScalar());

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

        public SupplierServiceDetails GetSupplierServiceDetailsById(int SupplierServiceDetailsId)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_tSupplierService", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = SupplierServiceDetailsId;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new SupplierServiceDetails()
                {
                    _SupServiceDetailsID = Convert.ToInt32(dt.Rows[0]["service_gid"].ToString()),
                    _SupServiceDetailsName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[0]["service_name"].ToString()) ? "" : dt.Rows[0]["service_name"].ToString())

                };
                return model;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                SupplierServiceDetails objMod = new SupplierServiceDetails();
                return objMod;
            }
            finally
            {
                con.Close();
            }
        }

        public void InsertSupplierServiceDetails(SupplierServiceDetails SupplierServiceDetail)
        {
            try
            {
                //GetConnection();
                //cmd = new SqlCommand("pr_asms_trn_tSupplierService", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                //cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = SupplierServiceDetail._SupServiceDetailsName;
                //cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                //cmd.Parameters.Add("@createdby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                //cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "insert";
                //cmd.ExecuteNonQuery();
                string[,] codes = new string[,]
                   {
                     {"service_name",Convert.ToString(string.IsNullOrEmpty(SupplierServiceDetail._SupServiceDetailsName)?"":SupplierServiceDetail._SupServiceDetailsName) },
                     {"service_supplierheader_gid",Convert.ToString(HttpContext.Current.Session["SupplierHeaderGid"]) },
                     {"service_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"service_insert_date",Convert.ToString(objCommonIUD.GetCurrentDate()) }
                   };
                //GetPageMode();
                //string tname = "";
                //if (pageMode == 1 || pageMode == 2 || pageMode == 3)
                //{
                //    tname = "asms_tmp_tservice";
                //}
                //else
                //{
                //    tname = "asms_trn_tservice";
                //}

                string insertcommand = objCommonIUD.InsertCommon(codes, "asms_tmp_tservice");
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@insertby", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@table", SqlDbType.VarChar).Value = "service";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "update";
                int tmp_gid = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                string sup_trn_gid = Convert.ToString(cmd.ExecuteScalar());

                string[,] codes2 = new string[,]
                   {
                     {"service_name",Convert.ToString(string.IsNullOrEmpty(SupplierServiceDetail._SupServiceDetailsName)?"":SupplierServiceDetail._SupServiceDetailsName) },
                     {"service_supplierheader_gid",sup_trn_gid },
                     {"service_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"service_insert_date",Convert.ToString(objCommonIUD.GetCurrentDate()) },
                     {"service_isremoved","Y" },
                      {"service_sup_id",tmp_gid.ToString() }
                   };
                string insertcommand2 = objCommonIUD.InsertCommon(codes2, "asms_trn_tservice");
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

        public void DeleteSupplierServiceDetails(int SupplierServiceDetailsId)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tSupplierService", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = SupplierServiceDetailsId;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "delete";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
        }

        public void UpdateSupplierServiceDetails(SupplierServiceDetails SupplierServiceDetail)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tSupplierService", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = SupplierServiceDetail._SupServiceDetailsID;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(SupplierServiceDetail._SupServiceDetailsName) ? "" : SupplierServiceDetail._SupServiceDetailsName);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "edit";
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

        public IEnumerable<SubContractorDetails> GetSubContractorDetails()
        {
            List<SubContractorDetails> objSubContractorDetails = new List<SubContractorDetails>();
            try
            {

                SubContractorDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_tSubContractor", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SubContractorDetails();
                    objModel._SubContractorID = Convert.ToInt32(dt.Rows[i]["subcontractor_gid"].ToString());
                    objModel._SubContractorName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["subcontractor_name"].ToString()) ? "" : dt.Rows[i]["subcontractor_name"].ToString());
                    objModel._SubContractorServiceName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["subcontractor_servicename"].ToString()) ? "" : dt.Rows[i]["subcontractor_servicename"].ToString());
                    objSubContractorDetails.Add(objModel);
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
            return objSubContractorDetails;
        }

        public SubContractorDetails GetSubContractorDetailsById(int SubContractorDetailsId)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_tSubContractor", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = SubContractorDetailsId;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new SubContractorDetails()
                {
                    _SubContractorID = Convert.ToInt32(dt.Rows[0]["subcontractor_gid"].ToString()),
                    _SubContractorName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[0]["subcontractor_name"].ToString()) ? "" : dt.Rows[0]["subcontractor_name"].ToString()),
                    _SubContractorServiceName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[0]["subcontractor_servicename"].ToString()) ? "" : dt.Rows[0]["subcontractor_servicename"].ToString())

                };
                return model;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                SubContractorDetails objMod = new SubContractorDetails();
                return objMod;
            }
            finally
            {
                con.Close();
            }
        }

        public void InsertSubContractorDetails(SubContractorDetails SubContractorDetail)
        {
            try
            {
                //GetConnection();
                //cmd = new SqlCommand("pr_asms_trn_tSubContractor", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                //cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = SubContractorDetail._SubContractorName;
                //cmd.Parameters.Add("@servicename", SqlDbType.VarChar).Value = SubContractorDetail._SubContractorServiceName;
                //cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                //cmd.Parameters.Add("@createdby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                //cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "insert";
                //cmd.ExecuteNonQuery();
                string[,] codes = new string[,]
                   {
                     {"subcontractor_name",Convert.ToString(string.IsNullOrEmpty(SubContractorDetail._SubContractorName)?"":SubContractorDetail._SubContractorName) },
                     {"subcontractor_servicename",Convert.ToString(string.IsNullOrEmpty(SubContractorDetail._SubContractorServiceName)?"":SubContractorDetail._SubContractorServiceName) },
                     {"subcontractor_supplierheader_gid",Convert.ToString(HttpContext.Current.Session["SupplierHeaderGid"]) },
                     {"subcontractor_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"subcontractor_insert_date",Convert.ToString(objCommonIUD.GetCurrentDate()) }
                   };
                //GetPageMode();
                //string tname = "";
                //if (pageMode == 1 || pageMode == 2 || pageMode == 3)
                //{
                //    tname = "asms_tmp_tsubcontractor";
                //}
                //else
                //{
                //    tname = "asms_trn_tsubcontractor";
                //}
                string insertcommand = objCommonIUD.InsertCommon(codes, "asms_tmp_tsubcontractor");
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@insertby", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@table", SqlDbType.VarChar).Value = "subcontractor";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "update";
                int tmp_gid = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                string sup_trn_gid = Convert.ToString(cmd.ExecuteScalar());

                string[,] codes2 = new string[,]
                   {
                     {"subcontractor_name",Convert.ToString(string.IsNullOrEmpty(SubContractorDetail._SubContractorName)?"":SubContractorDetail._SubContractorName) },
                     {"subcontractor_servicename",Convert.ToString(string.IsNullOrEmpty(SubContractorDetail._SubContractorServiceName)?"":SubContractorDetail._SubContractorServiceName) },
                     {"subcontractor_supplierheader_gid", sup_trn_gid},
                     {"subcontractor_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"subcontractor_insert_date",Convert.ToString(objCommonIUD.GetCurrentDate()) },
                     {"subcontractor_isremoved","Y" },
                      {"subcontractor_sup_id",tmp_gid.ToString()}
                   };
                string insertcommand2 = objCommonIUD.InsertCommon(codes2, "asms_trn_tsubcontractor");
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

        public void DeleteSubContractorDetails(int SubContractorDetailsId)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tSubContractor", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = SubContractorDetailsId;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "delete";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
        }

        public void UpdateSubContractorDetails(SubContractorDetails SubContractorDetail)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tSubContractor", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = SubContractorDetail._SubContractorID;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(SubContractorDetail._SubContractorName) ? "" : SubContractorDetail._SubContractorName);
                cmd.Parameters.Add("@servicename", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(SubContractorDetail._SubContractorServiceName) ? "" : SubContractorDetail._SubContractorServiceName);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "edit";
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

        //--Pandiaraj 22/06/17
        public IEnumerable<SupAttachment> getrole()
        {
            List<SupAttachment> objSupAttachment = new List<SupAttachment>();
            try
            {

                SupAttachment objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("GST_MST_TMP_SUPPLIER_SL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@suppliergst_supplierheader_gid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@UId", SqlDbType.Int).Value = (Int32)objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@StatementType", SqlDbType.VarChar).Value = "VMUROLE";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupAttachment();
                    objModel.ischeckingmaker = dt.Rows[i]["ischecker"].ToString();
                    
                 objSupAttachment.Add(objModel);
                }


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
            return objSupAttachment; 
        }

        public IEnumerable<SupAttachment> GetSupAttachment()
        {

           
            List<SupAttachment> objSupAttachment = new List<SupAttachment>();
            try
            {

                SupAttachment objModel;
                GetConnection();
                if (pageMode == 4 || pageMode == 5 || pageMode == 6)
                {
                    pageMode = 2;
                }
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_tsupplierattachments", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupAttachment();
                    objModel._AttachmentFor = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["attachmentfor"].ToString()) ? "" : dt.Rows[i]["attachmentfor"].ToString());
                    objModel._SupAttachmentID = Convert.ToInt32(dt.Rows[i]["gid"].ToString());
                    // objModel._SupDocumentGroupID = Convert.ToInt32(dt.Rows[i]["supplierattachments_documentgroup_gid"].ToString());
                    objModel._SupDocumentGroupName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["documentgroup_name"].ToString()) ? "" : dt.Rows[i]["documentgroup_name"].ToString());
                    //  objModel._SupDocumentNameID = Convert.ToInt32(dt.Rows[i]["supplierattachments_supplierdocuments_gid"].ToString());
                    objModel._SupDocumentName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["documentname_name"].ToString()) ? "" : dt.Rows[i]["documentname_name"].ToString());
                    objModel._SupAttachedActualFileName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["actualfilename"].ToString()) ? "" : dt.Rows[i]["actualfilename"].ToString());
                    objModel._SupAttachedFileName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["filename"].ToString()) ? "" : dt.Rows[i]["filename"].ToString());
                    objModel._SupAttachDescription = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["description"].ToString()) ? "" : dt.Rows[i]["description"].ToString());
                    objModel._SupAttachDate = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["attachmentdate"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["attachmentdate"].ToString()));
                    objSupAttachment.Add(objModel);
                }


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
            return objSupAttachment;

        }

        public SupAttachment GetSupAttachmentById(int SupAttachmentId)
        {
            try
            {
                GetConnection();
                if (pageMode == 4 || pageMode == 5 || pageMode == 6)
                {
                    pageMode = 2;
                }
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_tsupplierattachments", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = SupAttachmentId;
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new SupAttachment()
                {
                    _SupAttachmentID = Convert.ToInt32(dt.Rows[0]["supplierattachments_gid"].ToString()),
                    selectedDocumentGroupID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[0]["supplierattachments_documentgroup_gid"].ToString()) ? "0" : dt.Rows[0]["supplierattachments_documentgroup_gid"].ToString()),
                    selectedDocumentNameID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[0]["supplierattachments_supplierdocuments_gid"].ToString()) ? "" : dt.Rows[0]["supplierattachments_supplierdocuments_gid"].ToString()),
                    _SupAttachedFileName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[0]["supplierattachments_filename"].ToString()) ? "" : dt.Rows[0]["supplierattachments_filename"].ToString()),
                    _SupAttachDescription = Convert.ToString(string.IsNullOrEmpty(dt.Rows[0]["supplierattachments_description"].ToString()) ? "" : dt.Rows[0]["supplierattachments_description"].ToString()),
                    _SupAttachDate = Convert.ToString(string.IsNullOrEmpty(dt.Rows[0]["supplierattachments_date"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[0]["supplierattachments_date"].ToString())),
                    _SupAttachedActualFileName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[0]["supplierattachments_actual_filename"].ToString()) ? "" : dt.Rows[0]["supplierattachments_actual_filename"].ToString())
                };
                return model;

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                SupAttachment objMod = new SupAttachment();
                return objMod;
            }
            finally
            {
                con.Close();
                da.Dispose();
            }
        }

        public void InsertSupAttachment(SupAttachment SupAttachments)
        {
            try
            {
                string[,] codes = new string[,]
                   {
                     {"supplierattachments_documentgroup_gid",Convert.ToString(SupAttachments.selectedDocumentGroupID)},
                     {"supplierattachments_supplierdocuments_gid",Convert.ToString(SupAttachments.selectedDocumentNameID) },
                     {"supplierattachments_filename",Convert.ToString(string.IsNullOrEmpty(SupAttachments._SupAttachedFileName)?"":SupAttachments._SupAttachedFileName) },
                     {"supplierattachments_actual_filename",Convert.ToString(string.IsNullOrEmpty(SupAttachments._SupAttachedActualFileName)?"":SupAttachments._SupAttachedActualFileName) },
                     {"supplierattachments_description",Convert.ToString(string.IsNullOrEmpty(SupAttachments._SupAttachDescription)?"": SupAttachments._SupAttachDescription)},
                     {"supplierattachments_supplierheader_gid",Convert.ToString(HttpContext.Current.Session["SupplierHeaderGid"]) },
                     {"supplierattachments_date","sysdatetime()" },
                     {"supplierattachments_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"supplierattachments_insert_date","sysdatetime()" }
                   };
                //GetPageMode();
                //string tname = "";
                //if (pageMode == 1 || pageMode == 2 || pageMode == 3)
                //{
                //    tname = "asms_tmp_tsupplierattachments";
                //}
                //else
                //{
                //    tname = "asms_trn_tsupplierattachments";
                //}
                string insertcommand = objCommonIUD.InsertCommon(codes, "asms_tmp_tsupplierattachments");
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@insertby", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@table", SqlDbType.VarChar).Value = "subcontractor";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "update";
                int tmp_gid = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                string sup_trn_gid = Convert.ToString(cmd.ExecuteScalar());

                string[,] codes2 = new string[,] 
                   {
                     {"supplierattachments_documentgroup_gid",Convert.ToString(SupAttachments.selectedDocumentGroupID)},
                     {"supplierattachments_supplierdocuments_gid",Convert.ToString(SupAttachments.selectedDocumentNameID) },
                     {"supplierattachments_filename",Convert.ToString(string.IsNullOrEmpty(SupAttachments._SupAttachedFileName)?"":SupAttachments._SupAttachedFileName) },
                     {"supplierattachments_actual_filename",Convert.ToString(string.IsNullOrEmpty(SupAttachments._SupAttachedActualFileName)?"":SupAttachments._SupAttachedActualFileName) },
                     {"supplierattachments_description",Convert.ToString(string.IsNullOrEmpty(SupAttachments._SupAttachDescription)?"": SupAttachments._SupAttachDescription)},
                     {"supplierattachments_supplierheader_gid",Convert.ToString(HttpContext.Current.Session["SupplierHeaderGid"]) },
                     {"supplierattachments_date","sysdatetime()" },
                     {"supplierattachments_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"supplierattachments_insert_date","sysdatetime()" },
                     {"supplierattachments_isremoved","Y" }
                   };
                string insertcommand2 = objCommonIUD.InsertCommon(codes2, "asms_trn_tsupplierattachments");
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

        public void DeleteSupAttachment(int SupAttachmentId)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tsupplierattachments", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = SupAttachmentId;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "delete";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
        }

        public void UpdateSupAttachment(SupAttachment SupAttachments)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tsupplierattachments", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = SupAttachments._SupAttachmentID;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@documentgroup", SqlDbType.Int).Value = Convert.ToInt32(SupAttachments.selectedDocumentGroupID);
                cmd.Parameters.Add("@supplierdocumentsgid", SqlDbType.Int).Value = Convert.ToInt32(SupAttachments.selectedDocumentNameID);
                cmd.Parameters.Add("@filename", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(SupAttachments._SupAttachedFileName) ? "" : SupAttachments._SupAttachedFileName);
                cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(SupAttachments._SupAttachDescription) ? "" : SupAttachments._SupAttachDescription);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "edit";
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

        public List<SupAttachment> GetDocumentGroup()
        {
            List<SupAttachment> objSupAttachment = new List<SupAttachment>();
            try
            {

                SupAttachment objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_documentgroup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupAttachment();
                    objModel._DocumentGroupID = Convert.ToInt32(dt.Rows[i]["documentgroup_gid"].ToString());
                    objModel._DocumentGroupName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["documentgroup_name"].ToString()) ? "" : dt.Rows[i]["documentgroup_name"].ToString());
                    objSupAttachment.Add(objModel);
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
            return objSupAttachment;
        }

        public List<SupAttachment> GetDocumentName(int DocGroupID)
        {
            List<SupAttachment> objSupAttachment = new List<SupAttachment>();
            try
            {

                SupAttachment objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_documentname", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@Grpid", SqlDbType.Int).Value = DocGroupID;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupAttachment();
                    objModel._DocumentNameID = Convert.ToInt32(dt.Rows[i]["documentname_gid"].ToString());
                    objModel._DocumentName = Convert.ToString(dt.Rows[i]["documentname_name"].ToString());
                    objSupAttachment.Add(objModel);
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
            return objSupAttachment;
        }

        public int GetSupAttachmentGID()
        {
            try
            {
                //int supHeadGid = Convert.ToInt32(HttpContext.Current.Session["SupplierHeaderGid"]);
                int supHeadGid = 1;
                int EmpGid = Convert.ToInt32(objCmnFunctions.GetLoginUserGid());
                int SupAttachmentGid = 0;
                GetConnection();
                string lsqry = "select max(supplierattachments_gid) from asms_trn_tsupplierattachments ";
                lsqry += "where supplierattachments_supplierheader_gid =" + supHeadGid + " and supplierattachments_insert_by =" + EmpGid;
                cmd = new SqlCommand(lsqry, con);
                cmd.CommandType = CommandType.Text;
                SupAttachmentGid = Convert.ToInt32(cmd.ExecuteScalar());
                return SupAttachmentGid;
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

        public IEnumerable<SupplierBranchDetails> GetSupplierBranchDetails()
        {
            List<SupplierBranchDetails> objSupplierBranchDetails = new List<SupplierBranchDetails>();
            try
            {

                SupplierBranchDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_tsupplierbranch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierBranchDetails();
                    objModel._SupBranchID = Convert.ToInt32(dt.Rows[i]["supplierbranch_gid"].ToString());
                    objModel.selectedCityID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["supplierbranch_city"].ToString()) ? "" : dt.Rows[i]["supplierbranch_city"].ToString());
                    objModel._SupBranchName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["city_name"].ToString()) ? "" : dt.Rows[i]["city_name"].ToString());
                    objSupplierBranchDetails.Add(objModel);
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
            return objSupplierBranchDetails;
        }

        public SupplierBranchDetails GetSupplierBranchDetailsById(int SupplierBranchDetailsId)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_tsupplierbranch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = SupplierBranchDetailsId;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new SupplierBranchDetails()
                {
                    _SupBranchID = Convert.ToInt32(dt.Rows[0]["supplierbranch_gid"].ToString()),
                    selectedCityID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[0]["supplierbranch_city"].ToString()) ? "" : dt.Rows[0]["supplierbranch_city"].ToString()),
                    _SupBranchName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[0]["city_name"].ToString()) ? "" : dt.Rows[0]["city_name"].ToString())

                };
                return model;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                SupplierBranchDetails objMod = new SupplierBranchDetails();
                return objMod;
            }
            finally
            {

            }
        }

        public void InsertSupplierBranchDetails(SupplierBranchDetails SupplierBranchDetail)
        {
            try
            {
                string[,] codes = new string[,]
                   {
                     {"supplierbranch_city",Convert.ToString(SupplierBranchDetail.selectedCityID) },
                     {"supplierbranch_supplierheader_gid",Convert.ToString(HttpContext.Current.Session["SupplierHeaderGid"]) },
                     {"supplierbranch_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"supplierbranch_insert_date","sysdatetime()" }
                   };
                //GetPageMode();
                //string tname = "";
                //if (pageMode == 1 || pageMode == 2 || pageMode == 3)
                //{
                //    tname = "asms_tmp_tsupplierbranch";
                //}
                //else
                //{
                //    tname = "asms_trn_tsupplierbranch";
                //}
                string insertcommand = objCommonIUD.InsertCommon(codes, "asms_tmp_tsupplierbranch");
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@insertby", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@table", SqlDbType.VarChar).Value = "supplierbranch";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "update";
                int tmp_gid = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                string sup_trn_gid = Convert.ToString(cmd.ExecuteScalar());

                string[,] codes2 = new string[,]
                   {
                     {"supplierbranch_city",Convert.ToString(SupplierBranchDetail.selectedCityID) },
                     {"supplierbranch_supplierheader_gid", sup_trn_gid},
                     {"supplierbranch_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"supplierbranch_insert_date","sysdatetime()"  },
                     {"supplierbranch_isremoved","Y" }, {"supplierbranch_sup_id",tmp_gid.ToString() }
                   };
                string insertcommand2 = objCommonIUD.InsertCommon(codes2, "asms_trn_tsupplierbranch");
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

        public void DeleteSupplierBranchDetails(int SupplierBranchDetailsId)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tsupplierbranch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = SupplierBranchDetailsId;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "delete";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
        }

        public void UpdateSupplierBranchDetails(SupplierBranchDetails SupplierBranchDetail)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tsupplierbranch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = SupplierBranchDetail._SupBranchID;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@citygid", SqlDbType.Int).Value = SupplierBranchDetail.selectedCityID;
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "edit";
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

        public IEnumerable<SupAwardDetails> GetSupAwardDetails()
        {
            List<SupAwardDetails> objSupAwardDetails = new List<SupAwardDetails>();
            try
            {

                SupAwardDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_taward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupAwardDetails();
                    objModel._SupAwardID = Convert.ToInt32(dt.Rows[i]["award_gid"].ToString());
                    objModel._SupAwardName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["award_name"].ToString()) ? "" : dt.Rows[i]["award_name"].ToString());
                    objModel._SupAwardDesc = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["award_description"].ToString()) ? "" : dt.Rows[i]["award_description"].ToString());
                    objSupAwardDetails.Add(objModel);
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
            return objSupAwardDetails;
        }

        public SupAwardDetails GetSupAwardDetailsById(int SupAwardDetailsId)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_taward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = SupAwardDetailsId;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new SupAwardDetails()
                {
                    _SupAwardID = Convert.ToInt32(dt.Rows[0]["award_gid"].ToString()),
                    _SupAwardName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[0]["award_name"].ToString()) ? "" : dt.Rows[0]["award_name"].ToString()),
                    _SupAwardDesc = Convert.ToString(string.IsNullOrEmpty(dt.Rows[0]["award_description"].ToString()) ? "" : dt.Rows[0]["award_description"].ToString())

                };
                return model;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                SupAwardDetails objMod = new SupAwardDetails();
                return objMod;
            }
            finally
            {

            }
        }

        public void InsertSupAwardDetails(SupAwardDetails SupAwardDetail)
        {
            try
            {

                string[,] codes = new string[,]
                   {
                     {"award_name",Convert.ToString(string.IsNullOrEmpty(SupAwardDetail._SupAwardName)?"":SupAwardDetail._SupAwardName) },
                     {"award_description",Convert.ToString(string.IsNullOrEmpty(SupAwardDetail._SupAwardDesc)?"":SupAwardDetail._SupAwardDesc) },
                     {"award_supplierheader_gid",Convert.ToString(HttpContext.Current.Session["SupplierHeaderGid"]) },
                     {"award_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"award_insert_date",Convert.ToString(objCommonIUD.GetCurrentDate()) }
                   };
                //GetPageMode();
                //string tname = "";
                //if (pageMode == 1 || pageMode == 2 || pageMode == 3)
                //{
                //    tname = "asms_tmp_taward";
                //}
                //else
                //{
                //    tname = "asms_trn_taward";
                //}
                string insertcommand = objCommonIUD.InsertCommon(codes, "asms_tmp_taward");
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@insertby", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@table", SqlDbType.VarChar).Value = "award";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "update";
                int tmp_gid = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                string sup_trn_gid = Convert.ToString(cmd.ExecuteScalar());

                string[,] codes2 = new string[,]
                   {
                     {"award_name",Convert.ToString(string.IsNullOrEmpty(SupAwardDetail._SupAwardName)?"":SupAwardDetail._SupAwardName) },
                     {"award_description",Convert.ToString(string.IsNullOrEmpty(SupAwardDetail._SupAwardDesc)?"":SupAwardDetail._SupAwardDesc) },
                     {"award_supplierheader_gid",sup_trn_gid  },
                     {"award_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"award_insert_date",Convert.ToString(objCommonIUD.GetCurrentDate()) },
                      {"award_isremoved","Y" },
                        {"award_sup_id",tmp_gid.ToString()  }
                   };
                string insertcommand2 = objCommonIUD.InsertCommon(codes2, "asms_trn_taward");
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

        public void DeleteSupAwardDetails(int SupAwardDetailsId)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_taward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = SupAwardDetailsId;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "delete";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
        }

        public void UpdateSupAwardDetails(SupAwardDetails SupAwardDetail)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_taward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = SupAwardDetail._SupAwardID;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(SupAwardDetail._SupAwardName) ? "" : SupAwardDetail._SupAwardName);
                cmd.Parameters.Add("@desc", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(SupAwardDetail._SupAwardDesc) ? "" : SupAwardDetail._SupAwardDesc);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "edit";
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

        public IEnumerable<ClientDetails> GetClient()
        {
            List<ClientDetails> objClient = new List<ClientDetails>();
            try
            {

                ClientDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_tclient", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ClientDetails();
                    objModel._ClientID = Convert.ToInt32(dt.Rows[i]["client_gid"].ToString());
                    objModel._ClientName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["client_name"].ToString()) ? "" : dt.Rows[i]["client_name"].ToString());
                    //  objModel._ClientAgeOfProduct = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["client_ageofproduct"].ToString()) ? "" : dt.Rows[i]["client_ageofproduct"].ToString());
                    objModel._ClientAddress = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["client_address"].ToString()) ? "" : dt.Rows[i]["client_address"].ToString());
                    // objModel._ClientMobileNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["client_mobileno"].ToString()) ? "" : dt.Rows[i]["client_mobileno"].ToString());
                    // objModel._ClientPhoneNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["client_phoneno"].ToString()) ? "" : dt.Rows[i]["client_phoneno"].ToString());
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

        public ClientDetails GetClientById(int ClientId)
        {
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_tclient", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = ClientId;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new ClientDetails()
                {
                    _ClientID = Convert.ToInt32(dt.Rows[0]["client_gid"].ToString()),
                    _ClientName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[0]["client_name"].ToString()) ? "" : dt.Rows[0]["client_name"].ToString()),
                    //  _ClientAgeOfProduct = Convert.ToString(string.IsNullOrEmpty(dt.Rows[0]["client_ageofproduct"].ToString()) ? "" : dt.Rows[0]["client_ageofproduct"].ToString()),
                    _ClientAddress = Convert.ToString(string.IsNullOrEmpty(dt.Rows[0]["client_address"].ToString()) ? "" : dt.Rows[0]["client_address"].ToString())
                    // _ClientMobileNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[0]["client_mobileno"].ToString()) ? "" : dt.Rows[0]["client_mobileno"].ToString()),
                    //  _ClientPhoneNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[0]["client_phoneno"].ToString()) ? "" : dt.Rows[0]["client_phoneno"].ToString())
                };
                return model;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                ClientDetails objMod = new ClientDetails();
                return objMod;
            }
            finally
            {
                con.Close();
            }
        }

        public void InsertClient(ClientDetails Clients)
        {
            try
            {
                string[,] codes = new string[,]
                   {
                     {"client_name",Convert.ToString(string.IsNullOrEmpty(Clients._ClientName)?"":Clients._ClientName) },
                     {"client_supplierheader_gid", Convert.ToString(HttpContext.Current.Session["SupplierHeaderGid"]) },
                  //   {"client_ageofproduct",Convert.ToString(string.IsNullOrEmpty(Clients._ClientAgeOfProduct)?"":Clients._ClientAgeOfProduct) },
                     {"client_address",Convert.ToString(string.IsNullOrEmpty(Clients._ClientAddress)?"":Clients._ClientAddress) },
                    // {"client_mobileno",Convert.ToString(string.IsNullOrEmpty(Clients._ClientMobileNo)?"":Clients._ClientMobileNo) },
                   //  {"client_phoneno",Convert.ToString(string.IsNullOrEmpty(Clients._ClientPhoneNo)?"":Clients._ClientPhoneNo) },
                     {"client_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"client_insert_date","sysdatetime()" }
                   };
                //GetPageMode();
                //string tname = "";
                //if (pageMode == 1 || pageMode == 2 || pageMode == 3)
                //{
                //    tname = "asms_tmp_tclient";
                //}
                //else
                //{
                //    tname = "asms_trn_tclient";
                //}
                string insertcommand = objCommonIUD.InsertCommon(codes, "asms_tmp_tclient");
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@insertby", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@table", SqlDbType.VarChar).Value = "client";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "update";
                int tmp_gid = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                string sup_trn_gid = Convert.ToString(cmd.ExecuteScalar());

                string[,] codes2 = new string[,] 
                   {
                     {"client_name",Convert.ToString(string.IsNullOrEmpty(Clients._ClientName)?"":Clients._ClientName) },
                     {"client_supplierheader_gid",sup_trn_gid },
                  //   {"client_ageofproduct",Convert.ToString(string.IsNullOrEmpty(Clients._ClientAgeOfProduct)?"":Clients._ClientAgeOfProduct) },
                     {"client_address",Convert.ToString(string.IsNullOrEmpty(Clients._ClientAddress)?"":Clients._ClientAddress) },
                    // {"client_mobileno",Convert.ToString(string.IsNullOrEmpty(Clients._ClientMobileNo)?"":Clients._ClientMobileNo) },
                   //  {"client_phoneno",Convert.ToString(string.IsNullOrEmpty(Clients._ClientPhoneNo)?"":Clients._ClientPhoneNo) },
                     {"client_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"client_insert_date","sysdatetime()" },
                     {"client_isremoved","Y" },
                     {"client_sup_id",tmp_gid.ToString()  }
                   };
                string insertcommand2 = objCommonIUD.InsertCommon(codes2, "asms_trn_tclient");
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

        public void DeleteClient(int ClientId)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tclient", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = ClientId;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "delete";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
        }

        public void UpdateClient(ClientDetails Clients)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tclient", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = Clients._ClientID;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(Clients._ClientName) ? "" : Clients._ClientName);
                cmd.Parameters.Add("@ageofproduct", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(Clients._ClientAgeOfProduct) ? "" : Clients._ClientAgeOfProduct);
                cmd.Parameters.Add("@address", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(Clients._ClientAddress) ? "" : Clients._ClientAddress);
                cmd.Parameters.Add("@mobileno", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(Clients._ClientMobileNo) ? "" : Clients._ClientMobileNo);
                cmd.Parameters.Add("@phoneno", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(Clients._ClientPhoneNo) ? "" : Clients._ClientPhoneNo);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "edit";
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

        public string ClientIsExists(string Client)
        {
            throw new NotImplementedException();
        }

        public void InsertSupplierProfile(SupplierProfile supProfile)
        {
            try
            {
                string[,] codes = new string[,]
                   {
                     {"supplierprofile_noofyearsinbusiness",string.IsNullOrEmpty(Convert.ToString(supProfile._NoofYearsIBusiness))?"0":Convert.ToString(supProfile._NoofYearsIBusiness)},
                     {"supplierprofile_noofyearsofassociation",string.IsNullOrEmpty(Convert.ToString(supProfile._NoofYearsOfAssociation))?"0":Convert.ToString(supProfile._NoofYearsOfAssociation) },
                     {"supplierprofile_nooffactories",string.IsNullOrEmpty(Convert.ToString(supProfile._NoOfFactories))?"0":Convert.ToString(supProfile._NoOfFactories) },
                     {"supplierprofile_noofoffices",string.IsNullOrEmpty(Convert.ToString(supProfile._NoOfOffices))?"0":Convert.ToString(supProfile._NoOfOffices) },
                     {"supplierprofile_totalnoofemployees",string.IsNullOrEmpty(Convert.ToString(supProfile._TotalEmployees))?"0":Convert.ToString(supProfile._TotalEmployees) },
                     {"supplierprofile_contractemployees",string.IsNullOrEmpty(Convert.ToString(supProfile._ContractEmployees))?"0":Convert.ToString(supProfile._ContractEmployees) },
                     {"supplierprofile_permanentemployees",string.IsNullOrEmpty(Convert.ToString(supProfile._PermanentEmployees))?"0":Convert.ToString(supProfile._PermanentEmployees) },
                     {"supplierprofile_supplierheader_gid",Convert.ToString(HttpContext.Current.Session["SupplierHeaderGid"]) },
                     {"supplierprofile_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"supplierprofile_insert_date",Convert.ToString(objCommonIUD.GetCurrentDate()) }
                   };
                //GetPageMode();
                //string tname = "";
                //if (pageMode == 1 || pageMode == 2 || pageMode == 3)
                //{
                //    tname = "asms_tmp_tsupplierprofile";
                //}
                //else
                //{
                //    tname = "asms_trn_tsupplierprofile";
                //}
                string insertcommand = objCommonIUD.InsertCommon(codes, "asms_tmp_tsupplierprofile");
                GetConnection();

                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@insertby", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@table", SqlDbType.VarChar).Value = "supplierprofile";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "update";
                int tmp_gid = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                string sup_trn_gid = Convert.ToString(cmd.ExecuteScalar());

                string[,] codes2 = new string[,] 
                   {
                     {"supplierprofile_noofyearsinbusiness",string.IsNullOrEmpty(Convert.ToString(supProfile._NoofYearsIBusiness))?"0":Convert.ToString(supProfile._NoofYearsIBusiness)},
                     {"supplierprofile_noofyearsofassociation",string.IsNullOrEmpty(Convert.ToString(supProfile._NoofYearsOfAssociation))?"0":Convert.ToString(supProfile._NoofYearsOfAssociation) },
                     {"supplierprofile_nooffactories",string.IsNullOrEmpty(Convert.ToString(supProfile._NoOfFactories))?"0":Convert.ToString(supProfile._NoOfFactories) },
                     {"supplierprofile_noofoffices",string.IsNullOrEmpty(Convert.ToString(supProfile._NoOfOffices))?"0":Convert.ToString(supProfile._NoOfOffices) },
                     {"supplierprofile_totalnoofemployees",string.IsNullOrEmpty(Convert.ToString(supProfile._TotalEmployees))?"0":Convert.ToString(supProfile._TotalEmployees) },
                     {"supplierprofile_contractemployees",string.IsNullOrEmpty(Convert.ToString(supProfile._ContractEmployees))?"0":Convert.ToString(supProfile._ContractEmployees) },
                     {"supplierprofile_permanentemployees",string.IsNullOrEmpty(Convert.ToString(supProfile._PermanentEmployees))?"0":Convert.ToString(supProfile._PermanentEmployees) },
                     {"supplierprofile_supplierheader_gid",sup_trn_gid},
                     {"supplierprofile_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"supplierprofile_insert_date",Convert.ToString(objCommonIUD.GetCurrentDate()) },
                      {"supplierprofile_isremoved","Y" },
                      {"supplierprofile_sup_id",tmp_gid.ToString() }
                   };
                string insertcommand2 = objCommonIUD.InsertCommon(codes2, "asms_trn_tsupplierprofile");
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

        public void DeleteSupplierProfile(int SupProfileId)
        {
            throw new NotImplementedException();
        }

        public void UpdateSupplierProfile(SupplierProfile supProfile)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tsupplierprofile", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.VarChar).Value = Convert.ToInt32(supProfile._SupProfileID);
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@noofyearsinbusiness", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(Convert.ToString(supProfile._NoofYearsIBusiness)) ? "0" : Convert.ToString(supProfile._NoofYearsIBusiness));
                cmd.Parameters.Add("@noofyearsofassociation", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(Convert.ToString(supProfile._NoofYearsOfAssociation)) ? "0" : Convert.ToString(supProfile._NoofYearsOfAssociation));
                cmd.Parameters.Add("@nooffactories", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(Convert.ToString(supProfile._NoOfFactories)) ? "0" : Convert.ToString(supProfile._NoOfFactories));
                cmd.Parameters.Add("@noofoffices", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(Convert.ToString(supProfile._NoOfOffices)) ? "0" : Convert.ToString(supProfile._NoOfOffices));
                cmd.Parameters.Add("@totalnoofemployees", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(Convert.ToString(supProfile._TotalEmployees)) ? "0" : Convert.ToString(supProfile._TotalEmployees));
                cmd.Parameters.Add("@contractemployees", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(Convert.ToString(supProfile._ContractEmployees)) ? "0" : Convert.ToString(supProfile._ContractEmployees));
                cmd.Parameters.Add("@permanentemployees", SqlDbType.Int).Value = Convert.ToInt32(string.IsNullOrEmpty(Convert.ToString(supProfile._PermanentEmployees)) ? "0" : Convert.ToString(supProfile._PermanentEmployees));
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "edit";
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 5);
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

        public long GetSupplierProfileGid(SupplierProfile supProfile)
        {
            try
            {
                Int64 SupProfileGid = 0;
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tsupplierprofile", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.VarChar).Value = Convert.ToInt32(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@createdby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getsupplierprofilegid";
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                SupProfileGid = Convert.ToInt64(cmd.ExecuteScalar());

                return SupProfileGid;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {

            }
        }

        public List<SupplierContactDetails> GetCountry()
        {
            List<SupplierContactDetails> objSupContact = new List<SupplierContactDetails>();
            try
            {

                SupplierContactDetails objModel;
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
                    objModel = new SupplierContactDetails();
                    objModel._CountryID = Convert.ToInt32(dt.Rows[i]["country_gid"].ToString());
                    objModel._CountryName = Convert.ToString(dt.Rows[i]["country_name"].ToString());
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

        public List<SupplierContactDetails> GetState(int CountryID)
        {
            List<SupplierContactDetails> objSupContact = new List<SupplierContactDetails>();
            try
            {

                SupplierContactDetails objModel;
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
                    objModel = new SupplierContactDetails();
                    objModel._StateID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["state_gid"].ToString()) ? "0" : dt.Rows[i]["state_gid"].ToString());
                    objModel._StateName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["state_name"].ToString()) ? "" : dt.Rows[i]["state_name"].ToString());
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

        public List<SupplierContactDetails> GetCity(int StateID, int CountryID)
        {
            List<SupplierContactDetails> objSupContact = new List<SupplierContactDetails>();
            try
            {

                SupplierContactDetails objModel;
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
                    objModel = new SupplierContactDetails();
                    objModel._CityID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["city_gid"].ToString()) ? "" : dt.Rows[i]["city_gid"].ToString());
                    objModel._CityName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["city_name"].ToString()) ? "" : dt.Rows[i]["city_name"].ToString());
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
        public List<SupplierContactDetails> GetCityByState(int StateID, int CountryID)
        {
            List<SupplierContactDetails> objSupContact = new List<SupplierContactDetails>();
            try
            {

                SupplierContactDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@stateid", SqlDbType.Int).Value = StateID;
                cmd.Parameters.Add("@countryid", SqlDbType.Int).Value = CountryID;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getcity";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierContactDetails();
                    objModel._CityID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["city_gid"].ToString()) ? "" : dt.Rows[i]["city_gid"].ToString());
                    objModel._CityName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["city_name"].ToString()) ? "" : dt.Rows[i]["city_name"].ToString());
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

        public List<SupplierContactDetails> GetAddressType()
        {
            List<SupplierContactDetails> objSupContact = new List<SupplierContactDetails>();
            try
            {

                SupplierContactDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getaddresstype";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierContactDetails();
                    objModel._AddressTypeID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["addresstype_gid"].ToString()) ? "0" : dt.Rows[i]["addresstype_gid"].ToString());
                    objModel._AddressTypeName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["addresstype_name"].ToString()) ? "" : dt.Rows[i]["addresstype_name"].ToString());
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

        public IEnumerable<SupplierContactDetails> GetSupContactDetails()
        {
            List<SupplierContactDetails> objClient = new List<SupplierContactDetails>();
            try
            {

                SupplierContactDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_tsuppliercontact", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@dismode", SqlDbType.Int).Value = 1;
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierContactDetails();
                    objModel._SupContactDetailsID = Convert.ToInt32(dt.Rows[i]["suppliercontact_gid"].ToString());
                    objModel.selectedAddressTypeID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["suppliercontact_addresstype_gid"].ToString()) ? "0" : dt.Rows[i]["suppliercontact_addresstype_gid"].ToString());
                    objModel._AddressTypeName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["addresstype_name"].ToString()) ? "" : dt.Rows[i]["addresstype_name"].ToString());
                    objModel._Address1 = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["suppliercontact_address1"].ToString()) ? "" : dt.Rows[i]["suppliercontact_address1"].ToString());
                    objModel._Address2 = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["suppliercontact_address2"].ToString()) ? "" : dt.Rows[i]["suppliercontact_address2"].ToString());
                    objModel._Address3 = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["suppliercontact_address3"].ToString()) ? "" : dt.Rows[i]["suppliercontact_address3"].ToString());
                    objModel.selectedCountryID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["suppliercontact_country_gid"].ToString()) ? "0" : dt.Rows[i]["suppliercontact_country_gid"].ToString());
                    objModel._CountryName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["country_name"].ToString()) ? "" : dt.Rows[i]["country_name"].ToString());
                    objModel.selectedStateID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["suppliercontact_state_gid"].ToString()) ? "0" : dt.Rows[i]["suppliercontact_state_gid"].ToString());
                    objModel._StateName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["state_name"].ToString()) ? "" : dt.Rows[i]["state_name"].ToString());
                    objModel.selectedCityID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["suppliercontact_city_gid"].ToString()) ? "0" : dt.Rows[i]["suppliercontact_city_gid"].ToString());
                    objModel._CityName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["city_name"].ToString()) ? "" : dt.Rows[i]["city_name"].ToString());
                    objModel._PinCode = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["suppliercontact_pincode"].ToString()) ? "0" : dt.Rows[i]["suppliercontact_pincode"].ToString());
                    objModel._DistrictID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["suppliercontact_district_gid"].ToString()) ? "0" : dt.Rows[i]["suppliercontact_district_gid"].ToString());
                    objModel._DistrictName = string.IsNullOrEmpty(dt.Rows[i]["district_name"].ToString()) ? "0" : dt.Rows[i]["district_name"].ToString();
                    objModel._PhoneNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["suppliercontact_phoneno"].ToString()) ? "" : dt.Rows[i]["suppliercontact_phoneno"].ToString());
                    objModel._Fax = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["suppliercontact_fax"].ToString()) ? "" : dt.Rows[i]["suppliercontact_fax"].ToString());
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

        //public SupplierContactDetails GetSupContactDetailsById(int SupContactDetailsId)
        //{
        //    try
        //    {
        //        GetConnection();
        //        DataTable dt = new DataTable();
        //        cmd = new SqlCommand("pr_asms_trn_tsuppliercontact", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@id", SqlDbType.Int).Value = SupContactDetailsId;
        //        cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
        //        cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
        //        da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        var model = new SupplierContactDetails()
        //        {
        //             _SupContactDetailsID = Convert.ToInt32(dt.Rows[0]["suppliercontact_gid"].ToString()),
        //            selectedAddressTypeID = Convert.ToInt32(dt.Rows[0]["suppliercontact_addresstype_gid"].ToString()),
        //            _Address1 = Convert.ToString(dt.Rows[0]["suppliercontact_address1"].ToString()),
        //            _Address2 = Convert.ToString(dt.Rows[0]["suppliercontact_address2"].ToString()),
        //            _Address3 = Convert.ToString(dt.Rows[0]["suppliercontact_address3"].ToString()),
        //            selectedCountryID = Convert.ToInt32(dt.Rows[0]["suppliercontact_country_gid"].ToString()),
        //            selectedStateID = Convert.ToInt32(dt.Rows[0]["suppliercontact_state_gid"].ToString()),
        //            selectedCityID = Convert.ToInt32(dt.Rows[0]["suppliercontact_city_gid"].ToString()),
        //            _PinCode = Convert.ToInt32(dt.Rows[0]["suppliercontact_pincode"].ToString()),
        //            _PhoneNo = Convert.ToString(dt.Rows[0]["suppliercontact_phoneno"].ToString()),
        //            _Fax = Convert.ToString(dt.Rows[0]["suppliercontact_fax"].ToString())
        //        };
        //        return model;
        //    }
        //    catch (Exception ex)
        //    {
        //         objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
        //    }
        //    finally
        //    {

        //    }
        //}

        public void InsertSupContactDetails(SupplierContactDetails SupContactDetails)
        {
            try
            {
                string[,] codes = new string[,]
                   {
                     {"suppliercontact_supplierheader_gid",Convert.ToString(HttpContext.Current.Session["SupplierHeaderGid"]) },
                     {"suppliercontact_addresstype_gid",Convert.ToString(SupContactDetails.selectedAddressTypeID)},
                     {"suppliercontact_address1",Convert.ToString(string.IsNullOrEmpty(SupContactDetails._Address1)?"":SupContactDetails._Address1)},
                     {"suppliercontact_address2",Convert.ToString(string.IsNullOrEmpty(SupContactDetails._Address2)?"":SupContactDetails._Address2) },
                     {"suppliercontact_address3",Convert.ToString(string.IsNullOrEmpty(SupContactDetails._Address3)?"":SupContactDetails._Address3) },
                     {"suppliercontact_country_gid",Convert.ToString(SupContactDetails.selectedCountryID)},
                     {"suppliercontact_state_gid",Convert.ToString(SupContactDetails.selectedStateID)},
                     {"suppliercontact_city_gid",Convert.ToString(SupContactDetails.selectedCityID)},
                     {"suppliercontact_district_gid",Convert.ToString(SupContactDetails.selectedDistrictID)},
                     {"suppliercontact_pincode",Convert.ToString(SupContactDetails._PinCode == null?0:SupContactDetails._PinCode)},
                     {"suppliercontact_phoneno",Convert.ToString(string.IsNullOrEmpty(SupContactDetails._PhoneNo)?"":SupContactDetails._PhoneNo)},
                     {"suppliercontact_fax",Convert.ToString(string.IsNullOrEmpty(SupContactDetails._Fax)?"":SupContactDetails._Fax)},
                     {"suppliercontact_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"suppliercontact_insert_date",Convert.ToString(objCommonIUD.GetCurrentDate()) }
                   };
                //GetPageMode();
                //string tname = "";
                //if (pageMode == 1 || pageMode == 2 || pageMode == 3)
                //{
                //    tname = "asms_tmp_tsuppliercontact";
                //}
                //else
                //{
                //    tname = "asms_trn_tsuppliercontact";
                //}
                string insertcommand = objCommonIUD.InsertCommon(codes, "asms_tmp_tsuppliercontact");


                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@insertby", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@table", SqlDbType.VarChar).Value = "suppliercontact";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "update";
                int tmp_gid = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                string sup_trn_gid = Convert.ToString(cmd.ExecuteScalar());


                string[,] codes2 = new string[,]
                   {
                     {"suppliercontact_supplierheader_gid",sup_trn_gid },
                     {"suppliercontact_addresstype_gid",Convert.ToString(SupContactDetails.selectedAddressTypeID)},
                     {"suppliercontact_address1",Convert.ToString(string.IsNullOrEmpty(SupContactDetails._Address1)?"":SupContactDetails._Address1)},
                     {"suppliercontact_address2",Convert.ToString(string.IsNullOrEmpty(SupContactDetails._Address2)?"":SupContactDetails._Address2) },
                     {"suppliercontact_address3",Convert.ToString(string.IsNullOrEmpty(SupContactDetails._Address3)?"":SupContactDetails._Address3) },
                     {"suppliercontact_country_gid",Convert.ToString(SupContactDetails.selectedCountryID)},
                     {"suppliercontact_state_gid",Convert.ToString(SupContactDetails.selectedStateID)},
                     {"suppliercontact_city_gid",Convert.ToString(SupContactDetails.selectedCityID)},
                      {"suppliercontact_district_gid",Convert.ToString(SupContactDetails.selectedDistrictID)},
                     {"suppliercontact_pincode",Convert.ToString(SupContactDetails._PinCode == null?0:SupContactDetails._PinCode)},
                     {"suppliercontact_phoneno",Convert.ToString(string.IsNullOrEmpty(SupContactDetails._PhoneNo)?"":SupContactDetails._PhoneNo)},
                     {"suppliercontact_fax",Convert.ToString(string.IsNullOrEmpty(SupContactDetails._Fax)?"":SupContactDetails._Fax)},
                     {"suppliercontact_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"suppliercontact_insert_date",Convert.ToString(objCommonIUD.GetCurrentDate()) },
                     {"suppliercontact_isremoved","Y" },
                      {"suppliercontact_sup_id",tmp_gid.ToString()}
                   };
                string insertcommand2 = objCommonIUD.InsertCommon(codes2, "asms_trn_tsuppliercontact");
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

        public void DeleteSupContactDetails(int SupContactDetailsId)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tsuppliercontact", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = SupContactDetailsId;
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "delete";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
        }

        public void UpdateSupContactDetails(SupplierContactDetails SupContactDetails)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tsuppliercontact", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = SupContactDetails._SupContactDetailsID;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@address1", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(SupContactDetails._Address1) ? "" : SupContactDetails._Address1);
                cmd.Parameters.Add("@address2", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(SupContactDetails._Address2) ? "" : SupContactDetails._Address2);
                cmd.Parameters.Add("@address3", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(SupContactDetails._Address3) ? "" : SupContactDetails._Address3);
                cmd.Parameters.Add("@addresstypegid", SqlDbType.Int).Value = Convert.ToInt32(SupContactDetails.selectedAddressTypeID);
                cmd.Parameters.Add("@countrygid", SqlDbType.Int).Value = Convert.ToInt32(SupContactDetails.selectedCountryID);
                cmd.Parameters.Add("@stategid", SqlDbType.Int).Value = Convert.ToInt32(SupContactDetails.selectedStateID);
                cmd.Parameters.Add("@citygid", SqlDbType.Int).Value = Convert.ToInt32(SupContactDetails.selectedCityID);
                cmd.Parameters.Add("@districtgid", SqlDbType.Int).Value = Convert.ToInt32(SupContactDetails.selectedDistrictID);
                cmd.Parameters.Add("@pincode", SqlDbType.Int).Value = Convert.ToInt32(SupContactDetails._PinCode);
                cmd.Parameters.Add("@phoneno", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(SupContactDetails._PhoneNo) ? "" : SupContactDetails._PhoneNo);
                cmd.Parameters.Add("@fax", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(SupContactDetails._Fax) ? "" : SupContactDetails._Fax);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = Convert.ToString(objCommonIUD.GetCurrentDate());
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

        public IEnumerable<SupplierContactPersonDetails> GetSupContactPersonDetails()
        {
            List<SupplierContactPersonDetails> objClient = new List<SupplierContactPersonDetails>();
            try
            {

                SupplierContactPersonDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_tcontactperson", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierContactPersonDetails();
                    objModel._SupContactPersonID = Convert.ToInt32(dt.Rows[i]["contactperson_gid"].ToString());
                    objModel._SupContactPersonName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["contactperson_name"].ToString()) ? "" : dt.Rows[i]["contactperson_name"].ToString());
                    objModel._DesignationName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["contactperson_designationname"].ToString()) ? "" : dt.Rows[i]["contactperson_designationname"].ToString());
                    //  objModel.selectedDesignationID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["contactperson_designation"].ToString()) ? "0" : dt.Rows[i]["contactperson_designation"].ToString());
                    objModel._SupContactPersonLocation = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["contactperson_location"].ToString()) ? "" : dt.Rows[i]["contactperson_location"].ToString());
                    objModel._SupContactPersonMobileNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["contactperson_mobileno"].ToString()) ? "" : dt.Rows[i]["contactperson_mobileno"].ToString());
                    objModel._SupContactPersonPhoneNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["contactperson_phoneno"].ToString()) ? "" : dt.Rows[i]["contactperson_phoneno"].ToString());
                    objModel._SupContactPersonEmailId = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["contactperson_emailid"].ToString()) ? "" : dt.Rows[i]["contactperson_emailid"].ToString());
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

        public void InsertSupContactPersonDetails(SupplierContactPersonDetails SupContactPersonDetails)
        {
            try
            {
                string[,] codes = new string[,]
                   {
                     {"contactperson_supplierheader_gid",Convert.ToString(HttpContext.Current.Session["SupplierHeaderGid"]) },
                     {"contactperson_name",Convert.ToString(string.IsNullOrEmpty(SupContactPersonDetails._SupContactPersonName)?"":SupContactPersonDetails._SupContactPersonName)},
                     {"contactperson_designationname",Convert.ToString(string.IsNullOrEmpty(SupContactPersonDetails._DesignationName)?"":SupContactPersonDetails._DesignationName) }, 
                     {"contactperson_location",Convert.ToString(string.IsNullOrEmpty(SupContactPersonDetails._SupContactPersonLocation)?"":SupContactPersonDetails._SupContactPersonLocation) },
                     {"contactperson_mobileno",Convert.ToString(string.IsNullOrEmpty(SupContactPersonDetails._SupContactPersonMobileNo)?"":SupContactPersonDetails._SupContactPersonMobileNo) },
                     {"contactperson_phoneno",Convert.ToString(string.IsNullOrEmpty(SupContactPersonDetails._SupContactPersonPhoneNo)?"":SupContactPersonDetails._SupContactPersonPhoneNo)},
                     {"contactperson_emailid",Convert.ToString(string.IsNullOrEmpty(SupContactPersonDetails._SupContactPersonEmailId)?"":SupContactPersonDetails._SupContactPersonEmailId)},
                     {"contactperson_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"contactperson_insert_date",Convert.ToString(objCommonIUD.GetCurrentDate()) }
                   };
                //GetPageMode();
                //string tname = "";
                //if (pageMode == 1 || pageMode == 2 || pageMode == 3)
                //{
                //    tname = "asms_tmp_tcontactperson";
                //}
                //else
                //{
                //    tname = "asms_trn_tcontactperson";
                //}
                string insertcommand = objCommonIUD.InsertCommon(codes, "asms_tmp_tcontactperson");

                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@insertby", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@table", SqlDbType.VarChar).Value = "contactperson";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "update";
                int tmp_gid = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                string sup_trn_gid = Convert.ToString(cmd.ExecuteScalar());

                string[,] codes2 = new string[,]
                   {
                     {"contactperson_supplierheader_gid",sup_trn_gid },
                     {"contactperson_name",Convert.ToString(string.IsNullOrEmpty(SupContactPersonDetails._SupContactPersonName)?"":SupContactPersonDetails._SupContactPersonName)},
                     {"contactperson_designationname",Convert.ToString(string.IsNullOrEmpty(SupContactPersonDetails._DesignationName)?"":SupContactPersonDetails._DesignationName) }, 
                     {"contactperson_location",Convert.ToString(string.IsNullOrEmpty(SupContactPersonDetails._SupContactPersonLocation)?"":SupContactPersonDetails._SupContactPersonLocation) },
                     {"contactperson_mobileno",Convert.ToString(string.IsNullOrEmpty(SupContactPersonDetails._SupContactPersonMobileNo)?"":SupContactPersonDetails._SupContactPersonMobileNo) },
                     {"contactperson_phoneno",Convert.ToString(string.IsNullOrEmpty(SupContactPersonDetails._SupContactPersonPhoneNo)?"":SupContactPersonDetails._SupContactPersonPhoneNo)},
                     {"contactperson_emailid",Convert.ToString(string.IsNullOrEmpty(SupContactPersonDetails._SupContactPersonEmailId)?"":SupContactPersonDetails._SupContactPersonEmailId)},
                     {"contactperson_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"contactperson_insert_date",Convert.ToString(objCommonIUD.GetCurrentDate()) },
                     {"contactperson_isremoved","Y" },
                      {"contactperson_sup_id",tmp_gid.ToString() }
                   };
                string insertcommand2 = objCommonIUD.InsertCommon(codes2, "asms_trn_tcontactperson");
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

        public void DeleteSupContactPersonDetails(int SupContactPersonDetailsId)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tcontactperson", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = SupContactPersonDetailsId;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "delete";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
        }

        public void UpdateSupContactPersonDetails(SupplierContactPersonDetails SupContactPersonDetails)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tcontactperson", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = SupContactPersonDetails._SupContactPersonID;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(SupContactPersonDetails._SupContactPersonName) ? "" : SupContactPersonDetails._SupContactPersonName);
                cmd.Parameters.Add("@designation", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(SupContactPersonDetails._DesignationName) ? "" : SupContactPersonDetails._DesignationName);
                cmd.Parameters.Add("@location", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(SupContactPersonDetails._SupContactPersonLocation) ? "" : SupContactPersonDetails._SupContactPersonLocation);
                cmd.Parameters.Add("@mobileno", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(SupContactPersonDetails._SupContactPersonMobileNo) ? "" : SupContactPersonDetails._SupContactPersonMobileNo);
                cmd.Parameters.Add("@phoneno", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(SupContactPersonDetails._SupContactPersonPhoneNo) ? "" : SupContactPersonDetails._SupContactPersonPhoneNo);
                cmd.Parameters.Add("@emailid", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(SupContactPersonDetails._SupContactPersonEmailId) ? "" : SupContactPersonDetails._SupContactPersonEmailId);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = Convert.ToString(objCommonIUD.GetCurrentDate());
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

        public List<SupplierContactPersonDetails> GetDesignation()
        {
            List<SupplierContactPersonDetails> objSupContactPerson = new List<SupplierContactPersonDetails>();
            try
            {

                SupplierContactPersonDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getdesignation";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierContactPersonDetails();
                    objModel._DesignationID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["designation_gid"].ToString()) ? "0" : dt.Rows[i]["designation_gid"].ToString());
                    objModel._DesignationName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["designation_name"].ToString()) ? "" : dt.Rows[i]["designation_name"].ToString());
                    objSupContactPerson.Add(objModel);
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
            return objSupContactPerson;
        }

        public IEnumerable<SupplierTaxDetails> GetSupTaxDetails()
        {
            List<SupplierTaxDetails> objSupTaxDetails = new List<SupplierTaxDetails>();
            try
            {

                SupplierTaxDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_ttaxdetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierTaxDetails();
                    objModel._TaxDetailsID = Convert.ToInt32(dt.Rows[i]["taxdetails_gid"].ToString());
                    objModel._TaxTypeID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["taxdetails_tax_gid"].ToString()) ? "0" : dt.Rows[i]["taxdetails_tax_gid"].ToString());
                    objModel._TaxTypeName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["tax_name"].ToString()) ? "" : dt.Rows[i]["tax_name"].ToString());
                    objModel._TaxTypeCode = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["tax_code"].ToString()) ? "" : dt.Rows[i]["tax_code"].ToString());
                    objModel._IsAllowAdd = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["isallowadd"].ToString()) ? "0" : dt.Rows[i]["isallowadd"].ToString());
                    objModel._TaxReceivableFlag = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["tax_receivable_flag"].ToString()) ? "0" : dt.Rows[i]["tax_receivable_flag"].ToString());
                    objModel._TaxRegNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["taxdetails_regno"].ToString()) ? "" : dt.Rows[i]["taxdetails_regno"].ToString());
                    objModel._TaxRate = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["taxdetails_rate"].ToString()) ? "" : dt.Rows[i]["taxdetails_rate"].ToString());
                    objSupTaxDetails.Add(objModel);
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
            return objSupTaxDetails;
        }

        public void InsertSupTaxDetails(SupplierTaxDetails SupTaxDetails)
        {
            try
            {
                string[,] codes = new string[,]
                   {
                     {"taxdetails_supplierheader_gid",Convert.ToString(HttpContext.Current.Session["SupplierHeaderGid"]) },
                     {"taxdetails_tax_gid",Convert.ToString(SupTaxDetails.selectedTaxTypeNameID)},
                     {"taxdetails_regno",Convert.ToString(string.IsNullOrEmpty(SupTaxDetails._TaxRegNo)?"":SupTaxDetails._TaxRegNo)},
                     {"taxdetails_rate",Convert.ToString(string.IsNullOrEmpty(SupTaxDetails._TaxRate)?"0":SupTaxDetails._TaxRate) },
                     {"taxdetails_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"taxdetails_insert_date","sysdatetime()"  }
                   };
                //GetPageMode();
                //string tname = "";
                //if (pageMode == 1 || pageMode == 2 || pageMode == 3)
                //{
                //    tname = "asms_tmp_ttaxdetails";
                //}
                //else
                //{
                //    tname = "asms_trn_ttaxdetails";
                //}
                string insertcommand = objCommonIUD.InsertCommon(codes, "asms_tmp_ttaxdetails");
                GetConnection();
                string sup_tmp_id = Convert.ToString(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = sup_tmp_id;
                cmd.Parameters.Add("@insertby", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@table", SqlDbType.VarChar).Value = "taxdetails";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "update";
                int tmp_gid = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                string sup_trn_gid = Convert.ToString(cmd.ExecuteScalar());


                if (HttpContext.Current.Session["isFinancialReviewer"] != null)
                {
                    if (Convert.ToString(HttpContext.Current.Session["isFinancialReviewer"]) == "yes")
                    {
                        string[,] codes3 = new string[,]
                   {
                     {"taxdetails_supplierheader_gid",sup_trn_gid },
                     {"taxdetails_tax_gid",Convert.ToString(SupTaxDetails.selectedTaxTypeNameID)},
                     {"taxdetails_regno",Convert.ToString(string.IsNullOrEmpty(SupTaxDetails._TaxRegNo)?"":SupTaxDetails._TaxRegNo)},
                     {"taxdetails_rate",Convert.ToString(string.IsNullOrEmpty(SupTaxDetails._TaxRate)?"0":SupTaxDetails._TaxRate) },
                     {"taxdetails_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"taxdetails_insert_date","sysdatetime()" },
                     {"taxdetails_sup_id",tmp_gid.ToString()}
                   };
                        string insertcommand3 = objCommonIUD.InsertCommon(codes3, "asms_trn_ttaxdetails");
                    }
                }
                else
                {
                    string[,] codes2 = new string[,]
                   {
                     {"taxdetails_supplierheader_gid",sup_trn_gid },
                     {"taxdetails_tax_gid",Convert.ToString(SupTaxDetails.selectedTaxTypeNameID)},
                     {"taxdetails_regno",Convert.ToString(string.IsNullOrEmpty(SupTaxDetails._TaxRegNo)?"":SupTaxDetails._TaxRegNo)},
                     {"taxdetails_rate",Convert.ToString(string.IsNullOrEmpty(SupTaxDetails._TaxRate)?"0":SupTaxDetails._TaxRate) },
                     {"taxdetails_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"taxdetails_insert_date","sysdatetime()"  },
                     {"taxdetails_isremoved","Y" },
                      {"taxdetails_sup_id",tmp_gid.ToString().ToString()}
                   };
                    string insertcommand2 = objCommonIUD.InsertCommon(codes2, "asms_trn_ttaxdetails");
                }

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

        public void DeleteSupTaxDetails(int SupTaxDetailsId)
        {
            try
            {
                GetConnection();

                if (HttpContext.Current.Session["isFinancialReviewer"] != null)
                {
                    if (HttpContext.Current.Session["isFinancialReviewer"] == "yes")
                    {
                        cmd = new SqlCommand("pr_asms_GetFinancialReviewSummary", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = SupTaxDetailsId;
                        cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                        cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                        cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = Convert.ToString(objCommonIUD.GetCurrentDate());
                        cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "deletetaxdetailsfinreview";
                    }
                }
                else
                {
                    cmd = new SqlCommand("pr_asms_trn_ttaxdetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = SupTaxDetailsId;
                    cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                    cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                    cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                    cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = Convert.ToString(objCommonIUD.GetCurrentDate());
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "delete";
                }
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

        public void UpdateSupTaxDetails(SupplierTaxDetails SupTaxDetails)
        {
            try
            {
                GetConnection();
                if (HttpContext.Current.Session["isFinancialReviewer"] != null)
                {
                    if (HttpContext.Current.Session["isFinancialReviewer"] == "yes")
                    {
                        cmd = new SqlCommand("pr_asms_GetFinancialReviewSummary", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = SupTaxDetails._TaxDetailsID;
                        cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                        cmd.Parameters.Add("@taxgid", SqlDbType.VarChar).Value = Convert.ToString(SupTaxDetails.selectedTaxTypeNameID);
                        cmd.Parameters.Add("@rate", SqlDbType.Decimal).Value = objCmnFunctions.convertoDecimal(string.IsNullOrEmpty(SupTaxDetails._TaxRate) ? "0" : SupTaxDetails._TaxRate);
                        cmd.Parameters.Add("@regno", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(SupTaxDetails._TaxRegNo) ? "" : SupTaxDetails._TaxRegNo);
                        cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                        cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                        cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = Convert.ToString(objCommonIUD.GetCurrentDate());
                        cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updatetaxdetailsfinreview";
                    }
                }
                else
                {
                    cmd = new SqlCommand("pr_asms_trn_ttaxdetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = SupTaxDetails._TaxDetailsID;
                    cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                    cmd.Parameters.Add("@taxgid", SqlDbType.VarChar).Value = Convert.ToString(SupTaxDetails.selectedTaxTypeNameID);
                    cmd.Parameters.Add("@rate", SqlDbType.Decimal).Value = objCmnFunctions.convertoDecimal(string.IsNullOrEmpty(SupTaxDetails._TaxRate) ? "0" : SupTaxDetails._TaxRate);
                    cmd.Parameters.Add("@regno", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(SupTaxDetails._TaxRegNo) ? "" : SupTaxDetails._TaxRegNo);
                    cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                    cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                    cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = Convert.ToString(objCommonIUD.GetCurrentDate());
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "edit";
                }
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

        public List<SupplierTaxDetails> GetTaxType()
        {
            List<SupplierTaxDetails> objSupTax = new List<SupplierTaxDetails>();
            try
            {

                SupplierTaxDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "gettax";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierTaxDetails();
                    objModel._TaxTypeID = Convert.ToInt32(dt.Rows[i]["tax_gid"].ToString());
                    //objModel._TaxTypeName = Convert.ToString(dt.Rows[i]["tax_name"].ToString());
                    objModel._TaxTypeName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["tax_code"].ToString()) ? "" : dt.Rows[i]["tax_code"].ToString());
                    objSupTax.Add(objModel);
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
            return objSupTax;
        }

        public List<SupplierTaxDetails> GetTaxDocuments()
        {
            List<SupplierTaxDetails> objSupTax = new List<SupplierTaxDetails>();
            try
            {

                SupplierTaxDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "gettaxdocuments";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierTaxDetails();
                    objModel._TaxDocumentNameID = Convert.ToInt32(dt.Rows[i]["documentname_gid"].ToString());
                    objModel._TaxDocumentName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["documentname_name"].ToString()) ? "" : dt.Rows[i]["documentname_name"].ToString());
                    objSupTax.Add(objModel);
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
            return objSupTax;
        }

        public List<SupplierTaxDetails> GetTaxSubType(int taxName)
        {
            List<SupplierTaxDetails> objSupTax = new List<SupplierTaxDetails>();
            try
            {

                SupplierTaxDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@taxgid", SqlDbType.Int).Value = taxName;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "gettaxsubtype";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierTaxDetails();
                    objModel._TdsServiceTypeID = Convert.ToInt32(dt.Rows[i]["taxsubtype_gid"].ToString());
                    objModel._TdsServiceTypeName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["taxsubtype_name"].ToString()) ? "" : dt.Rows[i]["taxsubtype_name"].ToString());
                    objModel._TdsServiceTypeSection = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["taxsubtype_code"].ToString()) ? "" : dt.Rows[i]["taxsubtype_code"].ToString());
                    objSupTax.Add(objModel);
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
            return objSupTax;
        }

        public List<SupplierTaxDetails> GetTdsSection(int ServiceTypeID)
        {
            List<SupplierTaxDetails> objSupTax = new List<SupplierTaxDetails>();
            try
            {

                SupplierTaxDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@taxsubtypegid", SqlDbType.Int).Value = ServiceTypeID;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "gettaxsubtypeSection";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierTaxDetails();
                    objModel._TDSRate = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["taxrate_rate"].ToString()) ? "" : dt.Rows[i]["taxrate_rate"].ToString());
                    objModel._TdsServiceTypeSection = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["taxsubtype_code"].ToString()) ? "" : dt.Rows[i]["taxsubtype_code"].ToString());
                    objSupTax.Add(objModel);
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
            return objSupTax;
        }

        public IEnumerable<SupplierTaxDetails> GetVatDetails()
        {
            List<SupplierTaxDetails> objVatDetails = new List<SupplierTaxDetails>();
            try
            {

                SupplierTaxDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_tvat", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@taxdetailsgid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["TaxDetailsID"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierTaxDetails();
                    objModel._VatID = Convert.ToInt32(dt.Rows[i]["vat_gid"].ToString());
                    objModel._VatStateID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["vat_state_gid"].ToString()) ? "" : dt.Rows[i]["vat_state_gid"].ToString());
                    objModel._VatStateName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["state_name"].ToString()) ? "" : dt.Rows[i]["state_name"].ToString());
                    objModel._VatRate = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["vat_rate"].ToString()) ? "0" : dt.Rows[i]["vat_rate"].ToString());
                    objVatDetails.Add(objModel);
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
            return objVatDetails;
        }

        public void InsertVatDetails(SupplierTaxDetails VatDetails)
        {
            try
            {
                string[,] codes = new string[,]
                   {
                     {"vat_taxdetails_gid",Convert.ToString(HttpContext.Current.Session["TaxDetailsID"]) },
                     {"vat_state_gid",Convert.ToString(VatDetails.selectedVatStateID)},
                     {"vat_rate",Convert.ToString(string.IsNullOrEmpty(VatDetails._VatRate)?"":VatDetails._VatRate)},
                     {"vat_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"vat_insert_date",Convert.ToString(objCommonIUD.GetCurrentDate()) }
                   };
                //GetPageMode();
                //string tname = "";
                //if (pageMode == 1 || pageMode == 2 || pageMode == 3)
                //{
                //    tname = "asms_tmp_tvat";
                //}
                //else
                //{
                //    tname = "asms_trn_tvat";
                //}
                string insertcommand = objCommonIUD.InsertCommon(codes, "asms_tmp_tvat");
                string[,] codes2 = new string[,]
                   {
                     {"vat_taxdetails_gid",Convert.ToString(HttpContext.Current.Session["TaxDetailsID"]) },
                     {"vat_state_gid",Convert.ToString(VatDetails.selectedVatStateID)},
                     {"vat_rate",Convert.ToString(string.IsNullOrEmpty(VatDetails._VatRate)?"":VatDetails._VatRate)},
                     {"vat_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"vat_insert_date",Convert.ToString(objCommonIUD.GetCurrentDate()) },
                     {"vat_isremoved","Y" }
                   };
                string insertcommand2 = objCommonIUD.InsertCommon(codes2, "asms_trn_tvat");
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

        public void DeleteVatDetails(int VatDetailsId)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tvat", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = VatDetailsId;
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "delete";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
        }

        public void UpdateVatDetails(SupplierTaxDetails VatDetails)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tvat", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = VatDetails._VatID;
                cmd.Parameters.Add("@city", SqlDbType.Int).Value = Convert.ToInt32(VatDetails.selectedVatStateID);
                cmd.Parameters.Add("@rate", SqlDbType.Decimal).Value = objCmnFunctions.convertoDecimal(string.IsNullOrEmpty(VatDetails._VatRate) ? "0" : VatDetails._VatRate);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = Convert.ToString(objCommonIUD.GetCurrentDate());
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

        public IEnumerable<SupplierTaxDetails> GetTdsDetails()
        {
            List<SupplierTaxDetails> objVatDetails = new List<SupplierTaxDetails>();
            try
            {

                SupplierTaxDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_ttds", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@taxdetailsgid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["TaxDetailsID"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierTaxDetails();
                    objModel._TdsID = Convert.ToInt32(dt.Rows[i]["tdsdetails_gid"].ToString());
                    objModel.selectedTdsServiceTypeID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["tdsdetails_taxsubtype_gid"].ToString()) ? "" : dt.Rows[i]["tdsdetails_taxsubtype_gid"].ToString());
                    objModel._TdsServiceTypeName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["taxsubtype_name"].ToString()) ? "" : dt.Rows[i]["taxsubtype_name"].ToString());
                    objModel._TdsServiceTypeSection = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["taxsubtype_code"].ToString()) ? "" : dt.Rows[i]["taxsubtype_code"].ToString());
                    objModel._TDSRate = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["tdsdetails_rate"].ToString()) ? "0" : dt.Rows[i]["tdsdetails_rate"].ToString());
                    objModel._ExemptedRate = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["tdsdetails_exemption_rate"].ToString()) ? "0" : dt.Rows[i]["tdsdetails_exemption_rate"].ToString());
                    objModel._ExemptionPeriodFrom = string.IsNullOrEmpty(dt.Rows[i]["tdsdetails_exemption_periodfrom"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["tdsdetails_exemption_periodfrom"].ToString());
                    objModel._ExemptionPeriodTo = string.IsNullOrEmpty(dt.Rows[i]["tdsdetails_exemption_periodto"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["tdsdetails_exemption_periodto"].ToString());
                    //objModel._ExemptionPeriodFrom =objCmnFunctions.ddMMyyyyString(Convert.ToString(dt.Rows[i]["tdsdetails_exemption_periodfrom"].ToString()));
                    //objModel._ExemptionPeriodTo =objCmnFunctions.ddMMyyyyString(Convert.ToString(dt.Rows[i]["tdsdetails_exemption_periodto"].ToString()));
                    objModel._ExemptionThresholdValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["tdsdetails_exemption_thresholdvalue"].ToString()) ? "0" : dt.Rows[i]["tdsdetails_exemption_thresholdvalue"].ToString());
                    objModel._ExemptionCertificateNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["tdsdetails_certificateno"].ToString()) ? "" : dt.Rows[i]["tdsdetails_certificateno"].ToString());
                    objModel._ExemptionDescription = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["tdsdetails_exemption_description"].ToString()) ? "" : dt.Rows[i]["tdsdetails_exemption_description"].ToString());
                    objVatDetails.Add(objModel);
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
            return objVatDetails;
        }

        public void InsertTdsDetails(SupplierTaxDetails TdsDetails)
        {
            try
            {
                if (TdsDetails._TDSRate != null)
                {
                    string[,] codes = new string[,]
                   {
                     {"tdsdetails_taxdetails_gid",Convert.ToString(HttpContext.Current.Session["TaxDetailsID"]) },
                     {"tdsdetails_taxsubtype_gid",Convert.ToString(TdsDetails.selectedTdsServiceTypeID)},
                     {"tdsdetails_rate",Convert.ToString(TdsDetails._TDSRate == null ?"0":TdsDetails._TDSRate )},
                     {"tdsdetails_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"tdsdetails_insert_date",Convert.ToString(objCommonIUD.GetCurrentDate()) }
                   };
                    //GetPageMode();
                    //string tname = "";
                    //if (pageMode == 1 || pageMode == 2 || pageMode == 3)
                    //{
                    //    tname = "asms_tmp_ttdsdetails";
                    //}
                    //else
                    //{
                    //    tname = "asms_trn_ttdsdetails";
                    //}
                    string insertcommand = objCommonIUD.InsertCommon(codes, "asms_tmp_ttdsdetails");


                    GetConnection();
                    cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["TaxDetailsID"]);
                    cmd.Parameters.Add("@insertby", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid();
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updatetax";
                    int tmp_gid = Convert.ToInt32(cmd.ExecuteScalar());

                    cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["TaxDetailsID"]);
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "selecttax";
                    string sup_trn_gid = Convert.ToString(cmd.ExecuteScalar());




                    if (HttpContext.Current.Session["isFinancialReviewer"] != null)
                    {
                        if (HttpContext.Current.Session["isFinancialReviewer"].ToString() == "yes")
                        {
                            string[,] codes3 = new string[,]
                           {
                             {"tdsdetails_taxdetails_gid",sup_trn_gid },
                             {"tdsdetails_taxsubtype_gid",Convert.ToString(TdsDetails.selectedTdsServiceTypeID)},
                             {"tdsdetails_rate",Convert.ToString(TdsDetails._TDSRate == null ?"0":TdsDetails._TDSRate )},
                             {"tdsdetails_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                             {"tdsdetails_insert_date","sysdatetime()"} ,
                             {"tdsdetails_taxdetails_id",tmp_gid.ToString()}
                           };
                            string insertcommand3 = objCommonIUD.InsertCommon(codes3, "asms_trn_ttdsdetails");
                        }
                    }
                    else
                    {
                        string[,] codes2 = new string[,]
                        {
                            {"tdsdetails_taxdetails_gid",sup_trn_gid },
                            {"tdsdetails_taxsubtype_gid",Convert.ToString(TdsDetails.selectedTdsServiceTypeID)},
                            {"tdsdetails_rate",Convert.ToString(TdsDetails._TDSRate == null ?"0":TdsDetails._TDSRate )},
                            {"tdsdetails_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                            {"tdsdetails_insert_date","sysdatetime()"},
                            {"tdsdetails_isremoved","Y" } ,
                            {"tdsdetails_taxdetails_id",tmp_gid.ToString()}
                        };
                        string insertcommand2 = objCommonIUD.InsertCommon(codes2, "asms_trn_ttdsdetails");
                    }

                }
                else
                {
                    string[,] codes = new string[,]
                   {
                     {"tdsdetails_taxdetails_gid",Convert.ToString(HttpContext.Current.Session["TaxDetailsID"]) },
                     {"tdsdetails_taxsubtype_gid",Convert.ToString(TdsDetails.selectedTdsServiceTypeID)},
                     {"tdsdetails_exemption_rate",Convert.ToString(TdsDetails._ExemptedRate ==null ? "0" : TdsDetails._ExemptedRate )},
                     {"tdsdetails_exemption_periodfrom",Convert.ToString(TdsDetails._ExemptionPeriodFrom == null ? "" : objCmnFunctions.convertoDateTimeString(TdsDetails._ExemptionPeriodFrom))},
                     {"tdsdetails_exemption_periodto",Convert.ToString(TdsDetails._ExemptionPeriodTo == null ? "" : objCmnFunctions.convertoDateTimeString(TdsDetails._ExemptionPeriodTo))},
                     {"tdsdetails_exemption_thresholdvalue",Convert.ToString(TdsDetails._ExemptionThresholdValue == null ? "0" : TdsDetails._ExemptionThresholdValue)},
                     {"tdsdetails_certificateno",Convert.ToString(TdsDetails._ExemptionCertificateNo == null ? "" : TdsDetails._ExemptionCertificateNo)},
                     {"tdsdetails_exemption_description",Convert.ToString(TdsDetails._ExemptionDescription == null ? "" : TdsDetails._ExemptionDescription)},
                     {"tdsdetails_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"tdsdetails_insert_date","sysdatetime()"} 
                   };
                    //GetPageMode();
                    //string tname = "";
                    //if (pageMode == 1 || pageMode == 2 || pageMode == 3)
                    //{
                    //    tname = "asms_tmp_ttdsdetails";
                    //}
                    //else
                    //{
                    //    tname = "asms_trn_ttdsdetails";
                    //}
                    string insertcommand = objCommonIUD.InsertCommon(codes, "asms_tmp_ttdsdetails");


                    GetConnection();
                    cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["TaxDetailsID"]);
                    cmd.Parameters.Add("@insertby", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid();
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updatetax";
                    int tmp_gid = Convert.ToInt32(cmd.ExecuteScalar());

                    cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["TaxDetailsID"]);
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "selecttax";
                    string sup_trn_gid = Convert.ToString(cmd.ExecuteScalar());


                    if (HttpContext.Current.Session["isFinancialReviewer"] != null)
                    {
                        if (HttpContext.Current.Session["isFinancialReviewer"] == "yes")
                        {
                            string[,] codes3 = new string[,]
                           {
                             {"tdsdetails_taxdetails_gid",sup_trn_gid },
                             {"tdsdetails_taxsubtype_gid",Convert.ToString(TdsDetails.selectedTdsServiceTypeID)},
                             {"tdsdetails_exemption_rate",Convert.ToString(TdsDetails._ExemptedRate ==null ? "0" : TdsDetails._ExemptedRate )},
                             {"tdsdetails_exemption_periodfrom",Convert.ToString(TdsDetails._ExemptionPeriodFrom == null ? "" : objCmnFunctions.convertoDateTimeString(TdsDetails._ExemptionPeriodFrom))},
                             {"tdsdetails_exemption_periodto",Convert.ToString(TdsDetails._ExemptionPeriodTo == null ? "" : objCmnFunctions.convertoDateTimeString(TdsDetails._ExemptionPeriodTo))},
                             {"tdsdetails_exemption_thresholdvalue",Convert.ToString(TdsDetails._ExemptionThresholdValue == null ? "0" : TdsDetails._ExemptionThresholdValue)},
                             {"tdsdetails_certificateno",Convert.ToString(TdsDetails._ExemptionCertificateNo == null ? "" : TdsDetails._ExemptionCertificateNo)},
                             {"tdsdetails_exemption_description",Convert.ToString(TdsDetails._ExemptionDescription == null ? "" : TdsDetails._ExemptionDescription)},
                             {"tdsdetails_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                             {"tdsdetails_insert_date","sysdatetime()"},
                             {"tdsdetails_taxdetails_id",tmp_gid.ToString()}
                           };
                            string insertcommand3 = objCommonIUD.InsertCommon(codes3, "asms_trn_ttdsdetails");
                        }
                    }
                    else
                    {
                        string[,] codes2 = new string[,] 
                       {
                         {"tdsdetails_taxdetails_gid",sup_trn_gid },
                         {"tdsdetails_taxsubtype_gid",Convert.ToString(TdsDetails.selectedTdsServiceTypeID)},
                         {"tdsdetails_exemption_rate",Convert.ToString(TdsDetails._ExemptedRate ==null ? "0" : TdsDetails._ExemptedRate )},
                         {"tdsdetails_exemption_periodfrom",Convert.ToString(TdsDetails._ExemptionPeriodFrom == null ? "" : objCmnFunctions.convertoDateTimeString(TdsDetails._ExemptionPeriodFrom))},
                         {"tdsdetails_exemption_periodto",Convert.ToString(TdsDetails._ExemptionPeriodTo == null ? "" : objCmnFunctions.convertoDateTimeString(TdsDetails._ExemptionPeriodTo))},
                         {"tdsdetails_exemption_thresholdvalue",Convert.ToString(TdsDetails._ExemptionThresholdValue == null ? "0" : TdsDetails._ExemptionThresholdValue)},
                         {"tdsdetails_certificateno",Convert.ToString(TdsDetails._ExemptionCertificateNo == null ? "" : TdsDetails._ExemptionCertificateNo)},
                         {"tdsdetails_exemption_description",Convert.ToString(TdsDetails._ExemptionDescription == null ? "" : TdsDetails._ExemptionDescription)},
                         {"tdsdetails_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                         {"tdsdetails_insert_date","sysdatetime()"} ,
                         {"tdsdetails_isremoved","Y" },
                         {"tdsdetails_taxdetails_id",tmp_gid.ToString()}
                       };
                        string insertcommand2 = objCommonIUD.InsertCommon(codes2, "asms_trn_ttdsdetails");
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
                da.Dispose();
            }
        }

        public void DeleteTdsDetails(int TdsDetailsId)
        {
            try
            {
                GetConnection();
                if (HttpContext.Current.Session["isFinancialReviewer"] != null)
                {
                    if ((string)HttpContext.Current.Session["isFinancialReviewer"] == "yes")
                    {
                        cmd = new SqlCommand("pr_asms_GetFinancialReviewSummary", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = TdsDetailsId;
                        cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                        cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = Convert.ToString(objCommonIUD.GetCurrentDate());
                        cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "deletetdsdetailsfinreview";
                    }
                }
                else
                {
                    cmd = new SqlCommand("pr_asms_trn_ttds", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = TdsDetailsId;
                    cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                    cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                    cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
                    cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "delete";
                }

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
        }

        public void UpdateTdsDetails(SupplierTaxDetails TdsDetails)
        {
            try
            {
                GetConnection();

                if (TdsDetails._TDSRate != null)
                {
                    if (HttpContext.Current.Session["isFinancialReviewer"] != null)
                    {
                        if (HttpContext.Current.Session["isFinancialReviewer"] == "yes")
                        {
                            cmd = new SqlCommand("pr_asms_GetFinancialReviewSummary", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = TdsDetails._TdsID;
                            cmd.Parameters.Add("@taxsubtypegid", SqlDbType.Int).Value = Convert.ToInt32(TdsDetails.selectedTdsServiceTypeID);
                            cmd.Parameters.Add("@rate", SqlDbType.Decimal).Value = objCmnFunctions.convertoDecimal(string.IsNullOrEmpty(TdsDetails._TDSRate) ? "0" : TdsDetails._TDSRate);
                            cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                            cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                            cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = Convert.ToString(objCommonIUD.GetCurrentDate());
                            cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updatetdsdetailsfinreview";
                        }
                    }
                    else
                    {
                        cmd = new SqlCommand("pr_asms_trn_ttds", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = TdsDetails._TdsID;
                        cmd.Parameters.Add("@taxsubtypegid", SqlDbType.Int).Value = Convert.ToInt32(TdsDetails.selectedTdsServiceTypeID);
                        cmd.Parameters.Add("@rate", SqlDbType.Decimal).Value = objCmnFunctions.convertoDecimal(string.IsNullOrEmpty(TdsDetails._TDSRate) ? "0" : TdsDetails._TDSRate);
                        cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                        cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                        cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = Convert.ToString(objCommonIUD.GetCurrentDate());
                        cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "edit";
                        cmd.Parameters.Add("@res", SqlDbType.VarChar, 64);
                        cmd.Parameters["@res"].Direction = ParameterDirection.Output;
                    }
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    if (HttpContext.Current.Session["isFinancialReviewer"] != null)
                    {
                        if (HttpContext.Current.Session["isFinancialReviewer"] == "yes")
                        {
                            cmd = new SqlCommand("pr_asms_GetFinancialReviewSummary", con);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@id", SqlDbType.Int).Value = TdsDetails._TdsID;
                            cmd.Parameters.Add("@taxsubtypegid", SqlDbType.Int).Value = Convert.ToInt32(TdsDetails.selectedTdsServiceTypeID);
                            cmd.Parameters.Add("@rate", SqlDbType.Decimal).Value = objCmnFunctions.convertoDecimal(string.IsNullOrEmpty(TdsDetails._TDSRate) ? "0" : TdsDetails._TDSRate);
                            cmd.Parameters.Add("@exemptionrate", SqlDbType.Decimal).Value = objCmnFunctions.convertoDecimal(string.IsNullOrEmpty(TdsDetails._ExemptedRate) ? "0" : TdsDetails._ExemptedRate);
                            cmd.Parameters.Add("@exemptionperiodfrom", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(TdsDetails._ExemptionPeriodFrom) ? "" : TdsDetails._ExemptionPeriodFrom);
                            cmd.Parameters.Add("@exemptionperiodto", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(TdsDetails._ExemptionPeriodTo) ? "" : TdsDetails._ExemptionPeriodTo);
                            cmd.Parameters.Add("@exemptionthresholdvalue", SqlDbType.Decimal).Value = Convert.ToString(string.IsNullOrEmpty(TdsDetails._ExemptionThresholdValue) ? "0" : TdsDetails._ExemptionThresholdValue);
                            cmd.Parameters.Add("@exemptioncertificateno", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(TdsDetails._ExemptionCertificateNo) ? "" : TdsDetails._ExemptionCertificateNo);
                            //  cmd.Parameters.Add("@exemptionfilename", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(TdsDetails._ExemptionAttachedFileName) ? "" : TdsDetails._ExemptionAttachedFileName);
                            cmd.Parameters.Add("@exemptiondescription", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(TdsDetails._ExemptionDescription) ? "" : TdsDetails._ExemptionDescription);
                            cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                            cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                            cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = Convert.ToString(objCommonIUD.GetCurrentDate());
                            cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updatetdsdetailsfinreview";
                        }
                    }
                    else
                    {
                        cmd = new SqlCommand("pr_asms_trn_ttds", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = TdsDetails._TdsID;
                        cmd.Parameters.Add("@taxsubtypegid", SqlDbType.Int).Value = Convert.ToInt32(TdsDetails.selectedTdsServiceTypeID);
                        cmd.Parameters.Add("@rate", SqlDbType.Decimal).Value = objCmnFunctions.convertoDecimal(string.IsNullOrEmpty(TdsDetails._TDSRate) ? "0" : TdsDetails._TDSRate);
                        cmd.Parameters.Add("@exemptionrate", SqlDbType.Decimal).Value = objCmnFunctions.convertoDecimal(string.IsNullOrEmpty(TdsDetails._ExemptedRate) ? "0" : TdsDetails._ExemptedRate);
                        cmd.Parameters.Add("@exemptionperiodfrom", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(TdsDetails._ExemptionPeriodFrom) ? "" : TdsDetails._ExemptionPeriodFrom);
                        cmd.Parameters.Add("@exemptionperiodto", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(TdsDetails._ExemptionPeriodTo) ? "" : TdsDetails._ExemptionPeriodTo);
                        cmd.Parameters.Add("@exemptionthresholdvalue", SqlDbType.Decimal).Value = Convert.ToString(string.IsNullOrEmpty(TdsDetails._ExemptionThresholdValue) ? "0" : TdsDetails._ExemptionThresholdValue);
                        cmd.Parameters.Add("@exemptioncertificateno", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(TdsDetails._ExemptionCertificateNo) ? "" : TdsDetails._ExemptionCertificateNo);
                        //  cmd.Parameters.Add("@exemptionfilename", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(TdsDetails._ExemptionAttachedFileName) ? "" : TdsDetails._ExemptionAttachedFileName);
                        cmd.Parameters.Add("@exemptiondescription", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(TdsDetails._ExemptionDescription) ? "" : TdsDetails._ExemptionDescription);
                        cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                        cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                        cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = Convert.ToString(objCommonIUD.GetCurrentDate());
                        cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "edit";
                        cmd.Parameters.Add("@res", SqlDbType.VarChar, 64);
                        cmd.Parameters["@res"].Direction = ParameterDirection.Output;

                    }
                    cmd.ExecuteNonQuery();
                    string res = Convert.ToString(cmd.Parameters["@res"].Value.ToString());
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
        }

        public List<PaymentDetails> GetPaymode()
        {
            List<PaymentDetails> objSupPayment = new List<PaymentDetails>();
            try
            {

                PaymentDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getpaymode";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new PaymentDetails();
                    objModel._PaymodeID = Convert.ToInt32(dt.Rows[i]["paymode_gid"].ToString());
                    objModel._PaymodeName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["paymode_name"].ToString()) ? "" : dt.Rows[i]["paymode_name"].ToString());
                    objSupPayment.Add(objModel);
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
            return objSupPayment;
        }

        public List<PaymentDetails> GetBank()
        {
            List<PaymentDetails> objSupPayment = new List<PaymentDetails>();
            try
            {

                PaymentDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getbank";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new PaymentDetails();
                    objModel._BankID = Convert.ToInt32(dt.Rows[i]["bank_gid"].ToString());
                    objModel._BankName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["bank_name"].ToString()) ? "" : dt.Rows[i]["bank_name"].ToString());
                    objSupPayment.Add(objModel);
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
            return objSupPayment;
        }

        public List<PaymentDetails> GetAccountType()
        {
            List<PaymentDetails> objSupPayment = new List<PaymentDetails>();
            try
            {

                PaymentDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getaccounttype";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new PaymentDetails();
                    objModel._AccountTypeID = Convert.ToInt32(dt.Rows[i]["accounttype_gid"].ToString());
                    objModel._AccountTypeName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["accounttype_name"].ToString()) ? "" : dt.Rows[i]["accounttype_name"].ToString());
                    objSupPayment.Add(objModel);
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
            return objSupPayment;
        }

        public IEnumerable<PaymentDetails> GetPaymentDetails()
        {
            List<PaymentDetails> objPaymentDetails = new List<PaymentDetails>();
            try
            {

                PaymentDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_tpayment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new PaymentDetails();
                    objModel._PaymentID = Convert.ToInt32(dt.Rows[i]["payment_gid"].ToString());
                    objModel._PaymodeName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["paymode_name"].ToString()) ? "" : dt.Rows[i]["paymode_name"].ToString());
                    objModel._BankName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["bank_name"].ToString()) ? "" : dt.Rows[i]["bank_name"].ToString());
                    objModel._BankBranch = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["payment_bankbranch"].ToString()) ? "" : dt.Rows[i]["payment_bankbranch"].ToString());
                    objModel._IfscCode = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["payment_ifsccode"].ToString()) ? "" : dt.Rows[i]["payment_ifsccode"].ToString());
                    objModel._AccountNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["payment_accountno"].ToString()) ? "" : dt.Rows[i]["payment_accountno"].ToString());
                    objModel._AccountTypeName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["accounttype_name"].ToString()) ? "" : dt.Rows[i]["accounttype_name"].ToString());
                    objModel._BeneficiaryName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["payment_beneficiaryname"].ToString()) ? "" : dt.Rows[i]["payment_beneficiaryname"].ToString());
                    objPaymentDetails.Add(objModel);
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
            return objPaymentDetails;
        }

        public void InsertPaymentDetails(PaymentDetails PaymentDetails)
        {
            try
            {
                string[,] codes = new string[,]
                   {
                     {"payment_supplierheader_gid",Convert.ToString(HttpContext.Current.Session["SupplierHeaderGid"]) },
                     {"payment_paymode_gid",Convert.ToString(PaymentDetails.selectedPaymodeID)},
                     {"payment_bank_gid",Convert.ToString(PaymentDetails.selectedBankID)},
                     {"payment_accounttype_gid",Convert.ToString(PaymentDetails.selectedAccountTypeID)},
                     {"payment_bankbranch",Convert.ToString(string.IsNullOrEmpty(PaymentDetails._BankBranch)?"":PaymentDetails._BankBranch)},
                     {"payment_ifsccode",Convert.ToString(string.IsNullOrEmpty(PaymentDetails._IfscCode)?"":PaymentDetails._IfscCode)},
                     {"payment_accountno",Convert.ToString(string.IsNullOrEmpty(PaymentDetails._AccountNo)?"":PaymentDetails._AccountNo)},
                     {"payment_beneficiaryname",Convert.ToString(string.IsNullOrEmpty(PaymentDetails._BeneficiaryName)?"":PaymentDetails._BeneficiaryName)},
                     {"payment_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"payment_insert_date","sysdatetime()" }
                   };
                //GetPageMode();
                //string tname = "";
                //if (pageMode == 1 || pageMode == 2 || pageMode == 3)
                //{
                //    tname = "asms_tmp_tpayment";
                //}
                //else
                //{
                //    tname = "asms_trn_tpayment";
                //}
                string insertcommand = objCommonIUD.InsertCommon(codes, "asms_tmp_tpayment");

                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@insertby", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@table", SqlDbType.VarChar).Value = "payment";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "update";
                int tmp_gid = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                string sup_trn_gid = Convert.ToString(cmd.ExecuteScalar());


                string[,] codes2 = new string[,]
                   {
                     {"payment_supplierheader_gid",sup_trn_gid},
                     {"payment_paymode_gid",Convert.ToString(PaymentDetails.selectedPaymodeID)},
                     {"payment_bank_gid",Convert.ToString(PaymentDetails.selectedBankID)},
                     {"payment_accounttype_gid",Convert.ToString(PaymentDetails.selectedAccountTypeID)},
                     {"payment_bankbranch",Convert.ToString(string.IsNullOrEmpty(PaymentDetails._BankBranch)?"":PaymentDetails._BankBranch)},
                     {"payment_ifsccode",Convert.ToString(string.IsNullOrEmpty(PaymentDetails._IfscCode)?"":PaymentDetails._IfscCode)},
                     {"payment_accountno",Convert.ToString(string.IsNullOrEmpty(PaymentDetails._AccountNo)?"":PaymentDetails._AccountNo)},
                     {"payment_beneficiaryname",Convert.ToString(string.IsNullOrEmpty(PaymentDetails._BeneficiaryName)?"":PaymentDetails._BeneficiaryName)},
                     {"payment_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"payment_insert_date","sysdatetime()" },
                     {"payment_isremoved","Y" },
                      {"payment_sup_id",tmp_gid.ToString() }
                   };
                string insertcommand2 = objCommonIUD.InsertCommon(codes2, "asms_trn_tpayment");
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

        public void DeletePaymentDetails(int PaymentDetailsId)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tpayment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = PaymentDetailsId;
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "delete";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
        }

        public void UpdatePaymentDetails(PaymentDetails PaymentDetails)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tpayment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = PaymentDetails._PaymentID;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@paymodegid", SqlDbType.Int).Value = Convert.ToInt16(PaymentDetails.selectedPaymodeID);
                cmd.Parameters.Add("@bankgid", SqlDbType.Int).Value = Convert.ToInt32(PaymentDetails.selectedBankID);
                cmd.Parameters.Add("@accounttypegid", SqlDbType.Int).Value = Convert.ToInt32(PaymentDetails.selectedAccountTypeID);
                cmd.Parameters.Add("@bankbranch", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(PaymentDetails._BankBranch) ? "" : PaymentDetails._BankBranch);
                cmd.Parameters.Add("@ifsccode", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(PaymentDetails._IfscCode) ? "" : PaymentDetails._IfscCode);
                cmd.Parameters.Add("@accountno", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(PaymentDetails._AccountNo) ? "" : PaymentDetails._AccountNo);
                cmd.Parameters.Add("@beneficiaryname", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(PaymentDetails._BeneficiaryName) ? "" : PaymentDetails._BeneficiaryName);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = Convert.ToString(objCommonIUD.GetCurrentDate());
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

        public List<SupActivity> GetActivityDocName()
        {
            List<SupActivity> objSupActivity = new List<SupActivity>();
            try
            {

                SupActivity objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getactivitydocname";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupActivity();
                    objModel._DocNameID = Convert.ToInt32(dt.Rows[i]["documentname_gid"].ToString());
                    objModel._DocName = Convert.ToString(dt.Rows[i]["documentname_name"].ToString());
                    objSupActivity.Add(objModel);
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
            return objSupActivity;
        }

        public List<SupActivity> GetBidding()
        {
            List<SupActivity> objSupActivity = new List<SupActivity>();
            try
            {

                SupActivity objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getbidding";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupActivity();
                    objModel._BiddingID = Convert.ToInt32(dt.Rows[i]["bidding_gid"].ToString());
                    objModel._BiddingName = Convert.ToString(dt.Rows[i]["bidding_name"].ToString());
                    objSupActivity.Add(objModel);
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
            return objSupActivity;
        }

        public List<SupActivity> GetActivityName()
        {
            List<SupActivity> objSupActivity = new List<SupActivity>();
            try
            {

                SupActivity objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getactivityname";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupActivity();
                    objModel._ActivityNameID = Convert.ToInt32(dt.Rows[i]["servicetype_gid"].ToString());
                    objModel._ActivityName = Convert.ToString(dt.Rows[i]["servicetype_name"].ToString());
                    objSupActivity.Add(objModel);
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
            return objSupActivity;
        }

        public List<SupActivity> GetCategory(string IsProdorService)
        {
            List<SupActivity> objSupActivity = new List<SupActivity>();
            try
            {

                SupActivity objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@prodserviceflag", SqlDbType.VarChar).Value = IsProdorService;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getcategory";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupActivity();
                    objModel._CategoryID = Convert.ToInt32(dt.Rows[i]["prodservice_gid"].ToString());
                    objModel._CategoryName = Convert.ToString(dt.Rows[i]["prodservice_name"].ToString());
                    objSupActivity.Add(objModel);
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
            return objSupActivity;
        }

        public List<SupActivity> GetSubcategory(int categoryId)
        {
            List<SupActivity> objSupActivity = new List<SupActivity>();
            try
            {

                SupActivity objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@categoryid", SqlDbType.Int).Value = categoryId;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getsubcategory";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupActivity();
                    objModel._SubCategoryID = Convert.ToInt32(dt.Rows[i]["prodservice_gid"].ToString());
                    objModel._SubCategoryName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["prodservice_name"].ToString()) ? "" : dt.Rows[i]["prodservice_name"].ToString());
                    objSupActivity.Add(objModel);
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
            return objSupActivity;
        }

        public IEnumerable<SupActivity> GetActivityDetails()
        {
            List<SupActivity> objPaymentDetails = new List<SupActivity>();
            try
            {

                SupActivity objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_tmultipleactivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataTable dtusers = new DataTable();
                    objModel = new SupActivity();
                    objModel._ActivityID = Convert.ToInt32(dt.Rows[i]["multipleactivities_gid"].ToString());
                    objModel._Activitytype = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["multipleactivities_activitytype"].ToString()) ? "0" : dt.Rows[i]["multipleactivities_activitytype"].ToString());
                    objModel._DescOfActivityType = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["multipleactivities_descriptionofactivitytype"].ToString()) ? "" : dt.Rows[i]["multipleactivities_descriptionofactivitytype"].ToString());
                    //objModel._CategoryName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["category_name"].ToString()) ? "" : dt.Rows[i]["category_name"].ToString());
                    //objModel._SubCategoryName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["subcategory_name"].ToString()) ? "" : dt.Rows[i]["subcategory_name"].ToString());
                    objModel._FidelityInsuranceReqd = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["multipleactivities_fidelityinsurancereqd"].ToString()) ? "" : dt.Rows[i]["multipleactivities_fidelityinsurancereqd"].ToString());
                    objModel._BiddingName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["bidding_name"].ToString()) ? "" : dt.Rows[i]["bidding_name"].ToString());
                    objModel._OIC = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["multipleactivities_oic"].ToString()) ? "0" : dt.Rows[i]["multipleactivities_oic"].ToString());
                    objModel._ActivityName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["servicetype_name"].ToString()) ? "" : dt.Rows[i]["servicetype_name"].ToString());
                    objModel._ProjectContractSpend = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["multipleactivities_projectcontractspend"].ToString()) ? "0" : dt.Rows[i]["multipleactivities_projectcontractspend"].ToString());
                    objModel._Saves = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["multipleactivities_saves"].ToString()) ? "0" : dt.Rows[i]["multipleactivities_saves"].ToString());
                    objModel._ActivityStartDate = string.IsNullOrEmpty(dt.Rows[i]["multipleactivities_activitystartdate"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["multipleactivities_activitystartdate"].ToString());
                    objModel._ActivityEndDate = string.IsNullOrEmpty(dt.Rows[i]["multipleactivities_activityenddate"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["multipleactivities_activityenddate"].ToString());
                    objModel._ContactPerson = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["multipleactivities_contactperson"].ToString()) ? "0" : dt.Rows[i]["multipleactivities_contactperson"].ToString());
                    objModel._OICName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["oic_name"].ToString()) ? "" : dt.Rows[i]["oic_name"].ToString());
                    objModel._ContactPersonName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["contactperson_name"].ToString()) ? "" : dt.Rows[i]["contactperson_name"].ToString());
                    objModel._Reason = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["multipleactivities_reason"].ToString()) ? "" : dt.Rows[i]["multipleactivities_reason"].ToString());
                    objPaymentDetails.Add(objModel);
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
            return objPaymentDetails;
        }

        public void InsertActivityDetails(SupActivity ActivityDetails)
        {
            try
            {
                var colname1 = "1";
                var colvalue1 = "1";
                var colname2 = "1";
                var colvalue2 = "1";
                var activitystartdate = Convert.ToString(string.IsNullOrEmpty(ActivityDetails._ActivityStartDate) ? "" : objCmnFunctions.convertoDateTimeString(ActivityDetails._ActivityStartDate));
                var activityenddate = Convert.ToString(string.IsNullOrEmpty(ActivityDetails._ActivityEndDate) ? "" : objCmnFunctions.convertoDateTimeString(ActivityDetails._ActivityEndDate));
                if (activitystartdate != "")
                {
                    colname1 = "multipleactivities_activitystartdate";
                    colvalue1 = activitystartdate;
                }
                if (activityenddate != "")
                {
                    colname2 = "multipleactivities_activityenddate";
                    colvalue2 = activityenddate;
                }
                string[,] codes = new string[,]
                   {
                     {"multipleactivities_supplierheader_gid",Convert.ToString(HttpContext.Current.Session["SupplierHeaderGid"]) },
                     {"multipleactivities_activitytype",Convert.ToString(string.IsNullOrEmpty(ActivityDetails._Activitytype)?"":ActivityDetails._Activitytype)},
                     {"multipleactivities_descriptionofactivitytype",Convert.ToString(string.IsNullOrEmpty(ActivityDetails._DescOfActivityType)?"":ActivityDetails._DescOfActivityType)},
                     //{"multipleactivities_category_gid",Convert.ToString(ActivityDetails.selectedcategoryID)},
                     //{"multipleactivities_subcategory_gid",Convert.ToString(ActivityDetails.selectedSubcategoryID)},
                     {"multipleactivities_fidelityinsurancereqd",Convert.ToString(string.IsNullOrEmpty(ActivityDetails._FidelityInsuranceReqd)?"":ActivityDetails._FidelityInsuranceReqd)},
                     {"multipleactivities_bidding_gid",Convert.ToString(ActivityDetails.selectedBiddingID)},
                     {"multipleactivities_oic",Convert.ToString(ActivityDetails._OIC)},
                     {"multipleactivities_activity_gid",Convert.ToString(ActivityDetails.selectedActivityNameID)},
                     {"multipleactivities_projectcontractspend",Convert.ToString(string.IsNullOrEmpty(ActivityDetails._ProjectContractSpend)?"0":ActivityDetails._ProjectContractSpend.Replace(",","").ToString())},
                     {"multipleactivities_saves",Convert.ToString(string.IsNullOrEmpty(ActivityDetails._Saves)?"":ActivityDetails._Saves)},
                   //  {"multipleactivities_activitystartdate",Convert.ToString(string.IsNullOrEmpty(ActivityDetails._ActivityStartDate)?"":objCmnFunctions.convertoDateTimeString(ActivityDetails._ActivityStartDate))},
                   //  {"multipleactivities_activityenddate",Convert.ToString(string.IsNullOrEmpty(ActivityDetails._ActivityEndDate)?"":objCmnFunctions.convertoDateTimeString(ActivityDetails._ActivityEndDate))},
                     {colname1,colvalue1 },
                     {colname2,colvalue2},
                     {"multipleactivities_reason",Convert.ToString(string.IsNullOrEmpty(ActivityDetails._Reason)?"":ActivityDetails._Reason)},
                     {"multipleactivities_contactperson",Convert.ToString(ActivityDetails._ContactPerson)},
                     {"multipleactivities_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"multipleactivities_insert_date","sysdatetime()" }
                   };

                //GetPageMode(); 
                //string tname = "";
                //if (pageMode == 1 || pageMode == 2 || pageMode == 3)
                //{
                //    tname = "asms_tmp_tmultipleactivities";
                //}
                //else
                //{
                //    tname = "asms_trn_tmultipleactivities";
                //}
                string insertcommand = objCommonIUD.InsertCommon(codes, "asms_tmp_tmultipleactivities");
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@insertby", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@table", SqlDbType.VarChar).Value = "multipleactivities";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "update";
                int tmp_gid = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                string sup_trn_gid = Convert.ToString(cmd.ExecuteScalar());


                string[,] codes2 = new string[,]
                   {
                     {"multipleactivities_supplierheader_gid",sup_trn_gid },
                     {"multipleactivities_activitytype",Convert.ToString(string.IsNullOrEmpty(ActivityDetails._Activitytype)?"":ActivityDetails._Activitytype)},
                     {"multipleactivities_descriptionofactivitytype",Convert.ToString(string.IsNullOrEmpty(ActivityDetails._DescOfActivityType)?"":ActivityDetails._DescOfActivityType)},
                     //{"multipleactivities_category_gid",Convert.ToString(ActivityDetails.selectedcategoryID)},
                     //{"multipleactivities_subcategory_gid",Convert.ToString(ActivityDetails.selectedSubcategoryID)},
                     {"multipleactivities_fidelityinsurancereqd",Convert.ToString(string.IsNullOrEmpty(ActivityDetails._FidelityInsuranceReqd)?"":ActivityDetails._FidelityInsuranceReqd)},
                     {"multipleactivities_bidding_gid",Convert.ToString(ActivityDetails.selectedBiddingID)},
                     {"multipleactivities_oic",Convert.ToString(ActivityDetails._OIC)},
                     {"multipleactivities_activity_gid",Convert.ToString(ActivityDetails.selectedActivityNameID)},
                     {"multipleactivities_projectcontractspend",Convert.ToString(string.IsNullOrEmpty(ActivityDetails._ProjectContractSpend)?"0":ActivityDetails._ProjectContractSpend.Replace(",","").ToString())},
                     {"multipleactivities_saves",Convert.ToString(string.IsNullOrEmpty(ActivityDetails._Saves)?"":ActivityDetails._Saves)},
                    // {"multipleactivities_activitystartdate",Convert.ToString(string.IsNullOrEmpty(ActivityDetails._ActivityStartDate)?"":objCmnFunctions.convertoDateTimeString(ActivityDetails._ActivityStartDate))},
                    // {"multipleactivities_activityenddate",Convert.ToString(string.IsNullOrEmpty(ActivityDetails._ActivityEndDate)?"":objCmnFunctions.convertoDateTimeString(ActivityDetails._ActivityEndDate))},
                     {colname1,colvalue1}, 
                     {colname2,colvalue2},
                     {"multipleactivities_reason",Convert.ToString(string.IsNullOrEmpty(ActivityDetails._Reason)?"":ActivityDetails._Reason)},
                     {"multipleactivities_contactperson",Convert.ToString(ActivityDetails._ContactPerson)},
                     {"multipleactivities_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"multipleactivities_insert_date","sysdatetime()" },
                     {"multipleactivities_isremoved","Y" },
                     {"multipleactivities_sup_id",tmp_gid.ToString() }
                   };
                string insertcommand2 = objCommonIUD.InsertCommon(codes2, "asms_trn_tmultipleactivities");
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

        public void DeleteActivityDetails(int ActivityDetailsId)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tmultipleactivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = ActivityDetailsId;
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "delete";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
        }

        public void UpdateActivityDetails(SupActivity ActivityDetails)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tmultipleactivities", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = ActivityDetails._ActivityID;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@activitytype", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(ActivityDetails._Activitytype) ? "" : ActivityDetails._Activitytype);
                cmd.Parameters.Add("@descofactivitytype", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(ActivityDetails._DescOfActivityType) ? "" : ActivityDetails._DescOfActivityType);
                //cmd.Parameters.Add("@categoryid", SqlDbType.Int).Value = Convert.ToInt32(ActivityDetails.selectedcategoryID);
                //cmd.Parameters.Add("@subcategoryid", SqlDbType.Int).Value = Convert.ToInt32(ActivityDetails.selectedSubcategoryID);
                cmd.Parameters.Add("@fidelityinsurancereqd", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(ActivityDetails._FidelityInsuranceReqd) ? "" : ActivityDetails._FidelityInsuranceReqd);
                cmd.Parameters.Add("@biddinggid", SqlDbType.Int).Value = Convert.ToInt32(ActivityDetails.selectedBiddingID);
                cmd.Parameters.Add("@oic", SqlDbType.Int).Value = Convert.ToInt32(ActivityDetails._OIC);
                cmd.Parameters.Add("@activityname", SqlDbType.Int).Value = Convert.ToInt32(ActivityDetails.selectedActivityNameID);
                cmd.Parameters.Add("@projectcontractspend", SqlDbType.Decimal).Value = objCmnFunctions.convertoDecimal(string.IsNullOrEmpty(ActivityDetails._ProjectContractSpend) ? "0" : ActivityDetails._ProjectContractSpend);
                cmd.Parameters.Add("@saves", SqlDbType.Decimal).Value = objCmnFunctions.convertoDecimal(string.IsNullOrEmpty(ActivityDetails._Saves) ? "0" : ActivityDetails._Saves);
                cmd.Parameters.Add("@activitystartdate", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(ActivityDetails._ActivityStartDate) ? "" : objCmnFunctions.convertoDateTimeString(ActivityDetails._ActivityStartDate));
                cmd.Parameters.Add("@activityenddate", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(ActivityDetails._ActivityEndDate) ? "" : objCmnFunctions.convertoDateTimeString(ActivityDetails._ActivityEndDate));
                cmd.Parameters.Add("@contactperson", SqlDbType.Int).Value = Convert.ToInt32(ActivityDetails._ContactPerson);
                cmd.Parameters.Add("@reason", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(ActivityDetails._Reason) ? "" : ActivityDetails._Reason);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = Convert.ToString(objCommonIUD.GetCurrentDate());
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

        public IEnumerable<SupOtherDetails> GetEmpRelationship()
        {
            List<SupOtherDetails> objOtherDetails = new List<SupOtherDetails>();
            try
            {

                SupOtherDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_temprelationship", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupOtherDetails();
                    objModel._RelationshipID = Convert.ToInt32(dt.Rows[i]["emprelationship_gid"].ToString());
                    objModel._RelationName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["emprelationship_empname"].ToString()) ? "" : dt.Rows[i]["emprelationship_empname"].ToString());
                    objModel._RelationshipName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["emprelationship_relationship"].ToString()) ? "" : dt.Rows[i]["emprelationship_relationship"].ToString());
                    objOtherDetails.Add(objModel);
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
            return objOtherDetails;
        }

        public void InsertEmpRelationship(SupOtherDetails EmpRelationship)
        {
            try
            {
                string[,] codes = new string[,]
                   {
                     {"emprelationship_supplierheader_gid",Convert.ToString(HttpContext.Current.Session["SupplierHeaderGid"]) },
                     {"emprelationship_empname",Convert.ToString(string.IsNullOrEmpty(EmpRelationship._RelationName)?"":EmpRelationship._RelationName)},
                     {"emprelationship_relationship",Convert.ToString(string.IsNullOrEmpty(EmpRelationship._RelationshipName)?"":EmpRelationship._RelationshipName)},
                     {"emprelationship_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"emprelationship_insert_date",Convert.ToString(objCommonIUD.GetCurrentDate()) }
                   };
                //GetPageMode();
                //string tname = "";
                //if (pageMode == 1 || pageMode == 2 || pageMode == 3)
                //{
                //    tname = "asms_tmp_temprelationship";
                //}
                //else
                //{
                //    tname = "asms_trn_temprelationship";
                //}
                string insertcommand = objCommonIUD.InsertCommon(codes, "asms_tmp_temprelationship");
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@insertby", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@table", SqlDbType.VarChar).Value = "emprelationship";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "update";
                int tmp_gid = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                string sup_trn_gid = Convert.ToString(cmd.ExecuteScalar());


                string[,] codes2 = new string[,]
                   {
                     {"emprelationship_supplierheader_gid",sup_trn_gid },
                     {"emprelationship_empname",Convert.ToString(string.IsNullOrEmpty(EmpRelationship._RelationName)?"":EmpRelationship._RelationName)},
                     {"emprelationship_relationship",Convert.ToString(string.IsNullOrEmpty(EmpRelationship._RelationshipName)?"":EmpRelationship._RelationshipName)},
                     {"emprelationship_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"emprelationship_insert_date",Convert.ToString(objCommonIUD.GetCurrentDate()) },
                     {"emprelationship_isremoved","Y" },
                      {"emprelationship_sup_id",tmp_gid.ToString() }
                   };
                string insertcommand2 = objCommonIUD.InsertCommon(codes2, "asms_trn_temprelationship");
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

        public void DeleteEmpRelationship(int EmpRelationshipId)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_temprelationship", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = EmpRelationshipId;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
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

        public void UpdateEmpRelationship(SupOtherDetails EmpRelationship)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_temprelationship", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = EmpRelationship._RelationshipID;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@empname", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(EmpRelationship._RelationName) ? "" : EmpRelationship._RelationName);
                cmd.Parameters.Add("@relationship", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(EmpRelationship._RelationshipName) ? "" : EmpRelationship._RelationshipName);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = Convert.ToString(objCommonIUD.GetCurrentDate());
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

        public void InsertSupOthers(SupOtherDetails SupOthers)
        {
            try
            {
                string[,] codes = new string[,]
                   {
                     {"others_supplierheader_gid",Convert.ToString(HttpContext.Current.Session["SupplierHeaderGid"]) },
                     {"others_issueappointmentletters",Convert.ToString(string.IsNullOrEmpty(SupOthers._IssueAppointmentLetters)?"":SupOthers._IssueAppointmentLetters)},
                     {"others_performintegritychecks",Convert.ToString(string.IsNullOrEmpty(SupOthers._PerformIntegrityChecks)?"":SupOthers._PerformIntegrityChecks)},
                     {"others_integritycheckdescription",Convert.ToString(string.IsNullOrEmpty(SupOthers._IntegrityCheckDesc)?"":SupOthers._IntegrityCheckDesc)},
                     {"others_payminwages",Convert.ToString(string.IsNullOrEmpty(SupOthers._Payminwages)?"":SupOthers._Payminwages)},
                     {"others_employlabourbelow18yrs",Convert.ToString(string.IsNullOrEmpty(SupOthers._EmployLabourbelow18Yrs)?"":SupOthers._EmployLabourbelow18Yrs)},
                     {"others_emppfregno",Convert.ToString(string.IsNullOrEmpty(SupOthers._EmpPFRegNo)?"":SupOthers._EmpPFRegNo)},
                     {"others_shopregno",Convert.ToString(string.IsNullOrEmpty(SupOthers._ShopRegNo)?"":SupOthers._ShopRegNo)},
                     {"others_empstateregno",Convert.ToString(string.IsNullOrEmpty(SupOthers._EmpStateRegNo)?"":SupOthers._EmpStateRegNo)},
                     {"others_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"others_insert_date",Convert.ToString(objCommonIUD.GetCurrentDate()) }
                   };
                //GetPageMode();
                //string tname = "";
                //if (pageMode == 1 || pageMode == 2 || pageMode == 3)
                //{
                //    tname = "asms_tmp_tothers";
                //}
                //else
                //{
                //    tname = "asms_trn_tothers";
                //}
                string insertcommand = objCommonIUD.InsertCommon(codes, "asms_tmp_tothers");
                GetConnection();

                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@insertby", SqlDbType.VarChar).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@table", SqlDbType.VarChar).Value = "others";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "update";
                int tmp_gid = Convert.ToInt32(cmd.ExecuteScalar());

                cmd = new SqlCommand("pr_asms_trn_getTmpGid", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supid", SqlDbType.VarChar).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                string sup_trn_gid = Convert.ToString(cmd.ExecuteScalar());

                string[,] codes2 = new string[,]
                   {
                     {"others_supplierheader_gid",sup_trn_gid },
                     {"others_issueappointmentletters",Convert.ToString(string.IsNullOrEmpty(SupOthers._IssueAppointmentLetters)?"":SupOthers._IssueAppointmentLetters)},
                     {"others_performintegritychecks",Convert.ToString(string.IsNullOrEmpty(SupOthers._PerformIntegrityChecks)?"":SupOthers._PerformIntegrityChecks)},
                     {"others_integritycheckdescription",Convert.ToString(string.IsNullOrEmpty(SupOthers._IntegrityCheckDesc)?"":SupOthers._IntegrityCheckDesc)},
                     {"others_payminwages",Convert.ToString(string.IsNullOrEmpty(SupOthers._Payminwages)?"":SupOthers._Payminwages)},
                     {"others_employlabourbelow18yrs",Convert.ToString(string.IsNullOrEmpty(SupOthers._EmployLabourbelow18Yrs)?"":SupOthers._EmployLabourbelow18Yrs)},
                     {"others_emppfregno",Convert.ToString(string.IsNullOrEmpty(SupOthers._EmpPFRegNo)?"":SupOthers._EmpPFRegNo)},
                     {"others_shopregno",Convert.ToString(string.IsNullOrEmpty(SupOthers._ShopRegNo)?"":SupOthers._ShopRegNo)},
                     {"others_empstateregno",Convert.ToString(string.IsNullOrEmpty(SupOthers._EmpStateRegNo)?"":SupOthers._EmpStateRegNo)},
                     {"others_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"others_insert_date",Convert.ToString(objCommonIUD.GetCurrentDate()) },
                     {"others_isremoved","Y" } ,
                      {"others_sup_id",tmp_gid.ToString() } 
                   };
                string insertcommand2 = objCommonIUD.InsertCommon(codes2, "asms_trn_tothers");
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

        public void UpdateSupOthers(SupOtherDetails SupOthers)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tothers", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = SupOthers._OthersID;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@issueappointmentletters", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(SupOthers._IssueAppointmentLetters) ? "" : SupOthers._IssueAppointmentLetters);
                cmd.Parameters.Add("@performintegritychecks", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(SupOthers._PerformIntegrityChecks) ? "" : SupOthers._PerformIntegrityChecks);
                cmd.Parameters.Add("@integritycheckdesc", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(SupOthers._IntegrityCheckDesc) ? "" : SupOthers._IntegrityCheckDesc);
                cmd.Parameters.Add("@payminwages", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(SupOthers._Payminwages) ? "" : SupOthers._Payminwages);
                cmd.Parameters.Add("@employlabourbelow18yrs", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(SupOthers._EmployLabourbelow18Yrs) ? "" : SupOthers._EmployLabourbelow18Yrs);
                cmd.Parameters.Add("@emppfregno", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(SupOthers._EmpPFRegNo) ? "" : SupOthers._EmpPFRegNo);
                cmd.Parameters.Add("@shopregno", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(SupOthers._ShopRegNo) ? "" : SupOthers._ShopRegNo);
                cmd.Parameters.Add("@empstateregno", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(SupOthers._EmpStateRegNo) ? "" : SupOthers._EmpStateRegNo);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = Convert.ToString(objCommonIUD.GetCurrentDate());
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

        public int GetOthersID()
        {
            try
            {
                int result = 0;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_tothers", con);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "GetOthersID";
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

        public int GetEmpGid(string EmpCode)
        {

            try
            {
                int result = 0;
                string lsQry = "select employee_gid from iem_mst_temployee where employee_code='" + EmpCode + "' and employee_isremoved='N'";
                GetConnection();
                cmd = new SqlCommand(lsQry, con);
                cmd.CommandType = CommandType.Text;
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

        public void UpdateSupHeaderGidDirectors()
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tDirectors", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = Convert.ToString(objCommonIUD.GetCurrentDate());
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updatesupheadergid";
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

        public IEnumerable<AgeingBucket> GetAgeingBucket()
        {
            List<AgeingBucket> objparenttax = new List<AgeingBucket>();
            try
            {
                objparenttax.Add(new AgeingBucket { _AgeingBucketValue = "-1", _AgeingBucketText = "-- Select --", });
                objparenttax.Add(new AgeingBucket { _AgeingBucketValue = "0", _AgeingBucketText = "Expired", });
                objparenttax.Add(new AgeingBucket { _AgeingBucketValue = "30", _AgeingBucketText = "<30", });
                objparenttax.Add(new AgeingBucket { _AgeingBucketValue = "60", _AgeingBucketText = "30 to 60", });
                objparenttax.Add(new AgeingBucket { _AgeingBucketValue = "90", _AgeingBucketText = "61 to 90", });
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }

            return objparenttax;
        }

        public IEnumerable<SupplierHeader> GetSupHeaderDetails()
        {
            List<SupplierHeader> objSupHeaderDetails = new List<SupplierHeader>();
            try
            {
                HttpContext.Current.Session["ModificationQueueSearch"] = null;

                SupplierHeader objModel;
                GetConnection();
                DataTable dt = new DataTable();
                string actiontype = "";
                string protype = "";
                if (HttpContext.Current.Session["Supplierrenewalpro"] != null)
                {
                    protype = "pr_asms_GetCSC";
                    actiontype = "getcurrentapprovaldetailsrev";
                }
                else if (HttpContext.Current.Session["Totalsuppliers"] != null)
                {
                    protype = "pr_asms_GetDashboardDetailsstatus";
                    actiontype = HttpContext.Current.Session["Totalsuppliers"].ToString();
                }
                else
                {
                    protype = "pr_asms_GetCSC";
                    if (HttpContext.Current.Session["reqstatus"] != null)
                    {
                        actiontype = "getsupplierheaderdetailssearchdr";
                        cmd.Parameters.Add("@prodserviceflag", SqlDbType.VarChar).Value = Convert.ToString(HttpContext.Current.Session["reqstatus"].ToString());
                        cmd.Parameters.Add("@supname", SqlDbType.VarChar).Value = Convert.ToString("");
                        cmd.Parameters.Add("@taxname", SqlDbType.VarChar).Value = Convert.ToString("");
                    }
                    else
                    {
                        actiontype = "getsupplierheaderdetails";
                    }
                }
                cmd = new SqlCommand(protype, con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(objCmnFunctions.GetLoginUserGid());
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = actiontype.ToString();

                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierHeader();
                    objModel._HeaderID = Convert.ToInt32(dt.Rows[i]["supplierheader_gid"].ToString());
                    objModel._SupplierCode = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_suppliercode"].ToString()) ? "" : dt.Rows[i]["supplierheader_suppliercode"].ToString());
                    objModel._SupplierName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_name"].ToString()) ? "" : dt.Rows[i]["supplierheader_name"].ToString());
                    objModel._SupplierCategoryName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["suppliercategory_name"].ToString()) ? "" : dt.Rows[i]["suppliercategory_name"].ToString());
                    objModel._CompanyRegNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_companyregno"].ToString()) ? "" : dt.Rows[i]["supplierheader_companyregno"].ToString());
                    objModel._OwnerCode = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["employee_code"].ToString()) ? "" : dt.Rows[i]["employee_code"].ToString());
                    objModel._OwnerName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["employee_name"].ToString()) ? "" : dt.Rows[i]["employee_name"].ToString());
                    objModel._ContractFrom = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_contractfrom"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["supplierheader_contractfrom"].ToString()));
                    objModel._ContractTo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_contractto"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["supplierheader_contractto"].ToString()));
                    objModel._RenewalDate = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_renewaldate"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["supplierheader_renewaldate"].ToString()));

                    if (HttpContext.Current.Session["Supplierrenewalpro"] != null)
                    {
                        objModel._Activitycount = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["activitycount"].ToString()) ? "" : dt.Rows[i]["activitycount"].ToString());
                        objModel._AgeingBucket = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DiffDate"].ToString()) ? "" : dt.Rows[i]["DiffDate"].ToString());
                    }
                    else
                    {
                        objModel._Activitycount = "";
                        objModel._AgeingBucket = "";
                    }
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

        public SupplierHeader GetSupHeaderDetailsByID()
        {
            try
            {
                string queryname="";
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);

                //Pandiaraj 29/06/2017  for supplierquery Approved & pending TAB
               //queryname= HttpContext.Current.Session["isApproved"].ToString();
               //if (queryname == "P")
               //{
               //    pageMode = 2;                   
               //}
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode); 
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getsupplierheaderdetails";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new SupplierHeader()
                {
                    _HeaderID = Convert.ToInt32(dt.Rows[0]["supplierheader_gid"].ToString()),
                    _SupplierType = Convert.ToString(dt.Rows[0]["supplierheader_suppliertype"].ToString() == null ? "" : dt.Rows[0]["supplierheader_suppliertype"].ToString()),
                    _SupplierCode = Convert.ToString(dt.Rows[0]["supplierheader_suppliercode"].ToString() == null ? "" : dt.Rows[0]["supplierheader_suppliercode"].ToString()),
                    _SupplierName = Convert.ToString(dt.Rows[0]["supplierheader_name"].ToString() == null ? "" : dt.Rows[0]["supplierheader_name"].ToString()),
                    _SupplierCategoryName = Convert.ToString(dt.Rows[0]["suppliercategory_name"].ToString() == null ? "" : dt.Rows[0]["suppliercategory_name"].ToString()),
                    SelectedSupplierCategoryID = Convert.ToInt32(dt.Rows[0]["supplierheader_suppliercategory_gid"].ToString() == null ? "0" : dt.Rows[0]["supplierheader_suppliercategory_gid"].ToString()),
                    selectedOrganizationID = Convert.ToInt32(dt.Rows[0]["supplierheader_organizationtype_gid"].ToString() == null ? "0" : dt.Rows[0]["supplierheader_organizationtype_gid"].ToString()),
                    _OrganizationTypeName = Convert.ToString(dt.Rows[0]["organizationtype_name"].ToString() == null ? "" : dt.Rows[0]["organizationtype_name"].ToString()),
                    selectedServiceID = Convert.ToInt32(dt.Rows[0]["supplierheader_servicetype_gid"].ToString() == null ? "0" : dt.Rows[0]["supplierheader_servicetype_gid"].ToString()),
                    _ServiceTypeName = Convert.ToString(dt.Rows[0]["servicetype_name"].ToString() == null ? "" : dt.Rows[0]["servicetype_name"].ToString()),
                    SelectedSupContactTypeID = Convert.ToInt32(dt.Rows[0]["supplierheader_contacttype_gid"].ToString() == null ? "0" : dt.Rows[0]["supplierheader_contacttype_gid"].ToString()),
                    _SupContactTypeName = Convert.ToString(dt.Rows[0]["contacttype_name"].ToString() == null ? "" : dt.Rows[0]["contacttype_name"].ToString()),
                    _website = Convert.ToString(dt.Rows[0]["supplierheader_website"].ToString() == null ? "" : dt.Rows[0]["supplierheader_website"].ToString()),
                    _EmailID = Convert.ToString(dt.Rows[0]["supplierheader_emailid"].ToString() == null ? "" : dt.Rows[0]["supplierheader_emailid"].ToString()),
                    _CompanyRegNo = Convert.ToString(dt.Rows[0]["supplierheader_companyregno"].ToString() == null ? "" : dt.Rows[0]["supplierheader_companyregno"].ToString()),
                    _IsActiveContract = Convert.ToString(dt.Rows[0]["supplierheader_isactivecontract"].ToString() == null ? "" : dt.Rows[0]["supplierheader_isactivecontract"].ToString()),
                    _ReasonForNoContract = Convert.ToString(dt.Rows[0]["supplierheader_reasonfornocontract"].ToString() == null ? "" : dt.Rows[0]["supplierheader_reasonfornocontract"].ToString()),
                    _ContractFrom = Convert.ToString(dt.Rows[0]["supplierheader_contractfrom"].ToString() == null ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[0]["supplierheader_contractfrom"].ToString())),
                    _ContractTo = Convert.ToString(dt.Rows[0]["supplierheader_contractto"].ToString() == null ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[0]["supplierheader_contractto"].ToString())),
                    _ApproxSpend = Convert.ToString(dt.Rows[0]["supplierheader_approxspend"].ToString() == null ? "" : dt.Rows[0]["supplierheader_approxspend"].ToString()),
                    _ActualSpend = Convert.ToString(dt.Rows[0]["supplierheader_actualspend"].ToString() == null ? "" : dt.Rows[0]["supplierheader_actualspend"].ToString()),
                    _RenewalDate = Convert.ToString(dt.Rows[0]["supplierheader_renewaldate"].ToString() == null ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[0]["supplierheader_renewaldate"].ToString())),
                    _OwnerCode = Convert.ToString(dt.Rows[0]["employee_code"].ToString() == null ? "" : dt.Rows[0]["employee_code"].ToString()),
                    _OwnerName = Convert.ToString(dt.Rows[0]["employee_name"].ToString() == null ? "" : dt.Rows[0]["employee_name"].ToString()),
                    _OwnerId = Convert.ToInt32(dt.Rows[0]["supplierheader_owner_gid"].ToString() == null ? "0" : dt.Rows[0]["supplierheader_owner_gid"].ToString()),
                    _NoofYearsIBusiness = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[0]["supplierheader_noofyearsinbusiness"].ToString()) ? "0" : dt.Rows[0]["supplierheader_noofyearsinbusiness"].ToString()),
                    _NoofYearsOfAssociation = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[0]["supplierheader_noofyearsofassociation"].ToString()) ? "0" : dt.Rows[0]["supplierheader_noofyearsofassociation"].ToString()),
                    _NoOfOffices = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[0]["supplierheader_noofoffices"].ToString()) ? "0" : dt.Rows[0]["supplierheader_noofoffices"].ToString()),
                    _NoOfFactories = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[0]["supplierheader_nooffactories"].ToString()) ? "0" : dt.Rows[0]["supplierheader_nooffactories"].ToString()),
                    _TotalEmployees = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[0]["supplierheader_totalnoofemployees"].ToString()) ? "0" : dt.Rows[0]["supplierheader_totalnoofemployees"].ToString()),
                    _ContractEmployees = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[0]["supplierheader_contractemployees"].ToString()) ? "0" : dt.Rows[0]["supplierheader_contractemployees"].ToString()),
                    _PermanentEmployees = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[0]["supplierheader_permanentemployees"].ToString()) ? "0" : dt.Rows[0]["supplierheader_permanentemployees"].ToString()),
                    _IssueAppointmentLetters = Convert.ToString(dt.Rows[0]["supplierheader_issueappointmentletters"].ToString() == null ? "" : dt.Rows[0]["supplierheader_issueappointmentletters"].ToString()),
                    _PerformIntegrityChecks = Convert.ToString(dt.Rows[0]["supplierheader_performintegritychecks"].ToString() == null ? "" : dt.Rows[0]["supplierheader_performintegritychecks"].ToString()),
                    _IntegrityCheckDesc = Convert.ToString(dt.Rows[0]["supplierheader_integritycheckdescription"].ToString() == null ? "" : dt.Rows[0]["supplierheader_integritycheckdescription"].ToString()),
                    _Payminwages = Convert.ToString(dt.Rows[0]["supplierheader_payminwages"].ToString() == null ? "" : dt.Rows[0]["supplierheader_payminwages"].ToString()),
                    _EmployLabourbelow18Yrs = Convert.ToString(dt.Rows[0]["supplierheader_employlabourbelow18yrs"].ToString() == null ? "" : dt.Rows[0]["supplierheader_employlabourbelow18yrs"].ToString()),
                    _EmpPFRegNo = Convert.ToString(dt.Rows[0]["supplierheader_emppfregno"].ToString() == null ? "" : dt.Rows[0]["supplierheader_emppfregno"].ToString()),
                    _ShopRegNo = Convert.ToString(dt.Rows[0]["supplierheader_shopregno"].ToString() == null ? "" : dt.Rows[0]["supplierheader_shopregno"].ToString()),
                    _EmpStateRegNo = Convert.ToString(dt.Rows[0]["supplierheader_empstateregno"].ToString() == null ? "" : dt.Rows[0]["supplierheader_empstateregno"].ToString()),
                    _Requeststatus = Convert.ToString(dt.Rows[0]["supplierheader_requeststatus"].ToString() == null ? "" : dt.Rows[0]["supplierheader_requeststatus"].ToString()),
                    _RequestType = Convert.ToString(dt.Rows[0]["supplierheader_requesttype"].ToString() == null ? "" : dt.Rows[0]["supplierheader_requesttype"].ToString()),
                    _CurrentApprovalStage = Convert.ToInt32(dt.Rows[0]["supplierheader_currentapprovalstage"].ToString() == null ? "0" : dt.Rows[0]["supplierheader_currentapprovalstage"].ToString()),
                    _TaxisMSMED = Convert.ToString(dt.Rows[0]["supplierheader_ismsmed"].ToString() == null ? "" : dt.Rows[0]["supplierheader_ismsmed"].ToString()),
                    _PanNo = Convert.ToString(dt.Rows[0]["supplierheader_panno"].ToString() == null ? "" : dt.Rows[0]["supplierheader_panno"].ToString()),
                    _SupplierTypeRemarks = Convert.ToString(dt.Rows[0]["supplierheader_remarks"].ToString() == null ? "" : dt.Rows[0]["supplierheader_remarks"].ToString())
                };
                return model;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                SupplierHeader objMod = new SupplierHeader();
                return objMod;
            }
            finally
            {

            }
        }

       
      

        public int GetDirectorsCountForEdit()
        {
            try
            {
                int result = 0;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getdirectorscount";
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

        public void DeleteInvalidDirectors()
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_GetCSC", con);

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@createdby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "deleteinvaliddirectors";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
        }


        public int GetSupervisor()
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getsupervisor";
                cmd.Parameters["@res"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                int res = Convert.ToInt32(cmd.Parameters["@res"].Value.ToString());
                return res;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return 0;
            }
            finally
            {

            }
        }


        public List<SearchEmployee> GetEmployeeListForApproval()
        {
            List<SearchEmployee> objSearchEmployee = new List<SearchEmployee>();
            try
            {

                SearchEmployee objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "employeelistforapproval";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SearchEmployee();
                    objModel._EmployeeGid = Convert.ToInt32(dt.Rows[i]["employee_gid"].ToString());
                    objModel._EmployeeCode = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["employee_code"].ToString()) ? "" : dt.Rows[i]["employee_code"].ToString());
                    objModel._EmployeeName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["employee_name"].ToString()) ? "" : dt.Rows[i]["employee_name"].ToString());
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

        public IEnumerable<DashBoard> GetDashboardForMyApproval()
        {
            List<DashBoard> objDashboardDetails = new List<DashBoard>();
            try
            {

                DashBoard objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetDashboardDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(objCmnFunctions.GetLoginUserGid());
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getapproverdashboardcount";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new DashBoard();
                    objModel._DashBoardRequestType = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_requesttype"].ToString()) ? "" : dt.Rows[i]["supplierheader_requesttype"].ToString()).ToUpper();
                    objModel._DraftCount = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["draft"].ToString()) ? "0" : dt.Rows[i]["draft"].ToString());
                    objModel._InprocessCount = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["inprocess"].ToString()) ? "0" : dt.Rows[i]["inprocess"].ToString());
                    objModel._ApprovedCount = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["approved"].ToString()) ? "0" : dt.Rows[i]["approved"].ToString());
                    objModel._RejectedCount = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["rejected"].ToString()) ? "0" : dt.Rows[i]["rejected"].ToString());
                    objModel._WaitingForApprovalCount = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["waitingforapproval"].ToString()) ? "0" : dt.Rows[i]["waitingforapproval"].ToString());
                    objDashboardDetails.Add(objModel);
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
            return objDashboardDetails;
        }


        public IEnumerable<SupplierHeader> GetSupHeaderDetailsDashboard(string requesttype, string requeststatus)
        {
            List<SupplierHeader> objSupHeaderDetails = new List<SupplierHeader>();
            try
            {

                SupplierHeader objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetDashboardDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(objCmnFunctions.GetLoginUserGid());

                if (requeststatus == "WAITINGFORAPPROVAL")
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
                    objModel = new SupplierHeader();
                    objModel._HeaderID = Convert.ToInt32(dt.Rows[i]["supplierheader_gid"].ToString());
                    objModel._SupplierCode = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_suppliercode"].ToString()) ? "" : dt.Rows[i]["supplierheader_suppliercode"].ToString());
                    objModel._SupplierName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_name"].ToString()) ? "" : dt.Rows[i]["supplierheader_name"].ToString());
                    objModel._SupplierCategoryName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["suppliercategory_name"].ToString()) ? "" : dt.Rows[i]["suppliercategory_name"].ToString());
                    objModel._CompanyRegNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_companyregno"].ToString()) ? "" : dt.Rows[i]["supplierheader_companyregno"].ToString());
                    objModel._OwnerCode = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["employee_code"].ToString()) ? "" : dt.Rows[i]["employee_code"].ToString());
                    objModel._OwnerName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["employee_name"].ToString()) ? "" : dt.Rows[i]["employee_name"].ToString());
                    objModel._ContractFrom = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_contractfrom"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["supplierheader_contractfrom"].ToString()));
                    objModel._ContractTo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_contractto"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["supplierheader_contractto"].ToString()));
                    objModel._RenewalDate = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_renewaldate"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["supplierheader_renewaldate"].ToString()));
                    objModel._SupplierInsertBy = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_insert_by"].ToString()) ? "0" : dt.Rows[i]["supplierheader_insert_by"].ToString());
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

        public IEnumerable<SupplierTaxDetails> GetTaxAttachment()
        {
            List<SupplierTaxDetails> objTaxAttachment = new List<SupplierTaxDetails>();
            try
            {

                SupplierTaxDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_ttaxattachment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@taxdetailsgid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["TaxDetailsID"]);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierTaxDetails();
                    objModel._TaxAttachmentID = Convert.ToInt32(dt.Rows[i]["taxattachment_gid"].ToString());
                    objModel._TaxAttachmentFileName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["taxattachment_filename"].ToString()) ? "" : dt.Rows[i]["taxattachment_filename"].ToString());
                    objModel._TaxAttachmentActualFileName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["taxattachment_actual_filename"].ToString()) ? "" : dt.Rows[i]["taxattachment_actual_filename"].ToString());
                    objModel._TaxAttachmentDate = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["taxattachment_date"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["taxattachment_date"].ToString()));
                    objModel._TaxAttachmentDescription = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["taxattachment_description"].ToString()) ? "" : dt.Rows[i]["taxattachment_description"].ToString());
                    objModel._TaxDocumentGroupName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["documentgroup_name"].ToString()) ? "" : dt.Rows[i]["documentgroup_name"].ToString());
                    objModel._TaxDocumentName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["documentname_name"].ToString()) ? "" : dt.Rows[i]["documentname_name"].ToString());
                    objTaxAttachment.Add(objModel);
                }

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
            return objTaxAttachment;

        }


        public void InsertTaxAttachment(SupplierTaxDetails TaxAttachments)
        {
            try
            {
                string[,] codes = new string[,]
                   {
                     {"taxattachment_taxdetails_gid",Convert.ToString(HttpContext.Current.Session["TaxDetailsID"]) },
                     {"taxattachment_documentgroup_gid",Convert.ToString(TaxAttachments.selectedTaxDocumentGroupID) },
                     {"taxattachment_documentname_gid",Convert.ToString(TaxAttachments.selectedTaxDocumentNameID) },
                     {"taxattachment_filename",Convert.ToString(string.IsNullOrEmpty(TaxAttachments._TaxAttachmentFileName)?"":TaxAttachments._TaxAttachmentFileName) },
                     {"taxattachment_actual_filename",Convert.ToString(string.IsNullOrEmpty(TaxAttachments._TaxAttachmentActualFileName)?"":TaxAttachments._TaxAttachmentActualFileName) },
                     {"taxattachment_date","sysdatetime()" },
                     {"taxattachment_description",Convert.ToString(string.IsNullOrEmpty(TaxAttachments._TaxAttachmentDescription)?"":TaxAttachments._TaxAttachmentDescription) },
                     {"taxattachment_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"taxattachment_insert_date","sysdatetime()" }
                   };
                //GetPageMode();
                //string tname = "";
                //if (pageMode == 1 || pageMode == 2 || pageMode == 3)
                //{
                //    tname = "asms_tmp_ttaxattachment";
                //}
                //else
                //{
                //    tname = "asms_trn_ttaxattachment";
                //}
                string insertcommand = objCommonIUD.InsertCommon(codes, "asms_tmp_ttaxattachment");
                string[,] codes2 = new string[,]
                   {
                     {"taxattachment_taxdetails_gid",Convert.ToString(HttpContext.Current.Session["TaxDetailsID"]) },
                     {"taxattachment_documentgroup_gid",Convert.ToString(TaxAttachments.selectedTaxDocumentGroupID) },
                     {"taxattachment_documentname_gid",Convert.ToString(TaxAttachments.selectedTaxDocumentNameID) },
                     {"taxattachment_filename",Convert.ToString(string.IsNullOrEmpty(TaxAttachments._TaxAttachmentFileName)?"":TaxAttachments._TaxAttachmentFileName) },
                      {"taxattachment_actual_filename",Convert.ToString(string.IsNullOrEmpty(TaxAttachments._TaxAttachmentActualFileName)?"":TaxAttachments._TaxAttachmentActualFileName) },
                     {"taxattachment_date","sysdatetime()" },
                     {"taxattachment_description",Convert.ToString(string.IsNullOrEmpty(TaxAttachments._TaxAttachmentDescription)?"":TaxAttachments._TaxAttachmentDescription) },
                     {"taxattachment_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"taxattachment_insert_date","sysdatetime()" },
                     {"taxattachment_isremoved","Y" } 
                   };
                string insertcommand2 = objCommonIUD.InsertCommon(codes2, "asms_trn_ttaxattachment");
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

        public void DeleteTaxAttachment(int TaxAttachmentId)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_ttaxattachment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = TaxAttachmentId;
                // cmd.Parameters.Add("@taxdetailsgid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["TaxDetailsID"]);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
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

        public void UpdateTaxAttachment(SupplierTaxDetails TaxAttachments)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_ttaxattachment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = TaxAttachments._TaxAttachmentID;
                cmd.Parameters.Add("@taxdetailsgid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["TaxDetailsID"]);
                cmd.Parameters.Add("@documentname", SqlDbType.Int).Value = Convert.ToInt32(TaxAttachments.selectedTaxDocumentNameID);
                cmd.Parameters.Add("@filename", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(TaxAttachments._TaxAttachmentFileName) ? "" : TaxAttachments._TaxAttachmentFileName);
                cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(TaxAttachments._TaxAttachmentDescription) ? "" : TaxAttachments._TaxAttachmentDescription);
                cmd.Parameters.Add("@date", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(TaxAttachments._TaxAttachmentDate) ? "" : objCmnFunctions.convertoDateTimeString(TaxAttachments._TaxAttachmentDate));
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "edit";
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

        public void UpdateMSMED(SupplierTaxDetails TaxDetails)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_ttaxdetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@isMSMED", SqlDbType.VarChar).Value = Convert.ToString(string.IsNullOrEmpty(TaxDetails._TaxisMSMED) ? "" : TaxDetails._TaxisMSMED);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updateMSMED";
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
        public List<ApprovalModel> GetNextApprovalStage(string reqtype = null, int curlevel = 0)
        {
            List<ApprovalModel> objApproval = new List<ApprovalModel>();
            try
            {

                ApprovalModel objModel;
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
                    objModel = new ApprovalModel();
                    objModel._NextApprovalGroup = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["approvalstage_name"].ToString()) ? "" : dt.Rows[i]["approvalstage_name"].ToString());
                    objModel._QueueCurrentLevel = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["approvalstage_level"].ToString()) ? "" : dt.Rows[i]["approvalstage_level"].ToString());
                    objModel._QueueToType = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["approvalstage_type"].ToString()) ? "" : dt.Rows[i]["approvalstage_type"].ToString());
                    objModel._NextApprovalIsMandatory = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["approvalstage_ismandatory"].ToString()) ? "" : dt.Rows[i]["approvalstage_ismandatory"].ToString());
                    objApproval.Add(objModel);
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
            return objApproval;
        }
        public string SubmitToNextQueue(string queueto = "", string requesttype = "", string remarks = "", int actionstatus = 1, int skipflag = 0)
        {
            try
            {

                GetConnection();
                cmd = new SqlCommand("pr_asms_SubmitApproval", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@refgid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@requesttype", SqlDbType.VarChar).Value = Convert.ToString(requesttype);
                cmd.Parameters.Add("@actionremark", SqlDbType.VarChar).Value = Convert.ToString(remarks);
                cmd.Parameters.Add("@queueto", SqlDbType.VarChar).Value = Convert.ToString(queueto);
                cmd.Parameters.Add("@actionstatus", SqlDbType.Int).Value = actionstatus;
                cmd.Parameters.Add("@skipflag", SqlDbType.Int).Value = skipflag;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "submittonextapproval";
                cmd.ExecuteNonQuery();


                return "success";
            }
            catch (SqlException ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return ex.Message;
            }
            finally
            {
                con.Close();
            }
        }

        public IEnumerable<SupplierHeader> GetRequestHistory(int SupHeadGid)
        {
            List<SupplierHeader> objSupHeaderDetails = new List<SupplierHeader>();
            try
            {

                SupplierHeader objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetDashboardDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = SupHeadGid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getrequesthistory";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierHeader();
                    objModel._SupplierCode = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_suppliercode"].ToString()) ? "" : dt.Rows[i]["supplierheader_suppliercode"].ToString());
                    objModel._SupplierName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_name"].ToString()) ? "" : dt.Rows[i]["supplierheader_name"].ToString());
                    objModel._ApprovalStage = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["approvalstage_name"].ToString()) ? "" : dt.Rows[i]["approvalstage_name"].ToString());
                    objModel._ApprovalDate = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["requesthistory_requestdate"].ToString()) ? "" : dt.Rows[i]["requesthistory_requestdate"].ToString());
                    objModel._Raiser = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["employee"].ToString()) ? "" : dt.Rows[i]["employee"].ToString());
                    objModel._OwnerName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["ownername"].ToString()) ? "" : dt.Rows[i]["ownername"].ToString());
                    objModel._RequestType = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["requesthistory_requesttype"].ToString()) ? "" : dt.Rows[i]["requesthistory_requesttype"].ToString());
                    objModel._Requeststatus = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["requesthistory_requeststatus"].ToString()) ? "" : dt.Rows[i]["requesthistory_requeststatus"].ToString());
                    objModel._Remarks = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["requesthistory_remarks"].ToString()) ? "" : dt.Rows[i]["requesthistory_remarks"].ToString());
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


        public DataTable GetSupplierName(int supplierGid)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();

                cmd = new SqlCommand("pr_asms_GetDashboardDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = supplierGid;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getsuppliername";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());

            }
            finally
            {
                con.Close();
            }
            return dt;
        }


        public IEnumerable<SupplierHeader> GetAllRequestHistory()
        {
            List<SupplierHeader> objSupHeaderDetails = new List<SupplierHeader>();
            try
            {

                SupplierHeader objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetDashboardDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getallrequesthistory";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierHeader();
                    objModel._SupplierCode = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_suppliercode"].ToString()) ? "" : dt.Rows[i]["supplierheader_suppliercode"].ToString());
                    objModel._SupplierName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_name"].ToString()) ? "" : dt.Rows[i]["supplierheader_name"].ToString());
                    objModel._ApprovalStage = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["approvalstage_name"].ToString()) ? "" : dt.Rows[i]["approvalstage_name"].ToString());
                    objModel._ApprovalDate = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["requesthistory_requestdate"].ToString()) ? "" : dt.Rows[i]["requesthistory_requestdate"].ToString());
                    objModel._Raiser = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["employee"].ToString()) ? "" : dt.Rows[i]["employee"].ToString());
                    objModel._OwnerName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["ownername"].ToString()) ? "" : dt.Rows[i]["ownername"].ToString());
                    objModel._RequestType = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["requesthistory_requesttype"].ToString()) ? "" : dt.Rows[i]["requesthistory_requesttype"].ToString());
                    objModel._Requeststatus = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["requesthistory_requeststatus"].ToString()) ? "" : dt.Rows[i]["requesthistory_requeststatus"].ToString());
                    objModel._Remarks = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["requesthistory_remarks"].ToString()) ? "" : dt.Rows[i]["requesthistory_remarks"].ToString());
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


        public List<SupplierTaxDetails> GetVatState()
        {
            List<SupplierTaxDetails> objVatState = new List<SupplierTaxDetails>();
            try
            {

                SupplierTaxDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getvatstate";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierTaxDetails();
                    objModel._VatStateID = Convert.ToInt32(dt.Rows[i]["state_gid"].ToString());
                    objModel._VatStateName = Convert.ToString(dt.Rows[i]["state_name"].ToString());
                    objVatState.Add(objModel);
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
            return objVatState;
        }



        public DataTable GetCurrentApprovalStageDetails()
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();

                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt32(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getcurrentapprovaldetails";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return dt;
        }


        public IEnumerable<DashBoard> GetDashboardForSecondGrid()
        {
            List<DashBoard> objDashboardDetails1 = new List<DashBoard>();
            try
            {

                DashBoard objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetDashboardDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(objCmnFunctions.GetLoginUserGid());
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getSecodDashboardCount";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new DashBoard();
                    objModel._StatusName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["status"].ToString()) ? "" : dt.Rows[i]["status"].ToString()).ToUpper();
                    objModel._StatusCount = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["count"].ToString()) ? "0" : dt.Rows[i]["count"].ToString());
                    objDashboardDetails1.Add(objModel);
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
            return objDashboardDetails1;
        }

        public IEnumerable<SupplierHeader> GetMyRequestsGridDetails()
        {
            List<SupplierHeader> objSupHeaderDetails = new List<SupplierHeader>();
            try
            {

                SupplierHeader objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetDashboardGridDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(objCmnFunctions.GetLoginUserGid());
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getmyrequestgriddetails";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierHeader();
                    objModel._HeaderID = Convert.ToInt32(dt.Rows[i]["supplierheader_gid"].ToString());
                    objModel._SupplierCode = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_suppliercode"].ToString()) ? "" : dt.Rows[i]["supplierheader_suppliercode"].ToString());
                    objModel._SupplierName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_name"].ToString()) ? "" : dt.Rows[i]["supplierheader_name"].ToString());
                    objModel._SupplierCategoryName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["suppliercategory_name"].ToString()) ? "" : dt.Rows[i]["suppliercategory_name"].ToString());
                    objModel._SupplierStatus = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_status"].ToString()) ? "" : dt.Rows[i]["supplierheader_status"].ToString());
                    objModel._RequestType = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_requesttype"].ToString()) ? "" : dt.Rows[i]["supplierheader_requesttype"].ToString());
                    objModel._Requeststatus = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_requeststatus"].ToString()) ? "" : dt.Rows[i]["supplierheader_requeststatus"].ToString());
                    objModel._CurrentApprovalStage = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_currentapprovalstage"].ToString()) ? "0" : dt.Rows[i]["supplierheader_currentapprovalstage"].ToString());
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

        public IEnumerable<SupplierHeader> GetForMyApprovalGridDetails()
        {
            List<SupplierHeader> objSupHeaderDetails = new List<SupplierHeader>();
            try
            {

                SupplierHeader objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetDashboardGridDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(objCmnFunctions.GetLoginUserGid());
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getformyapprovalgriddetails";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierHeader();
                    objModel._HeaderID = Convert.ToInt32(dt.Rows[i]["supplierheader_gid"].ToString());
                    objModel._SupplierCode = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_suppliercode"].ToString()) ? "" : dt.Rows[i]["supplierheader_suppliercode"].ToString());
                    objModel._SupplierName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_name"].ToString()) ? "" : dt.Rows[i]["supplierheader_name"].ToString());
                    objModel._SupplierCategoryName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["suppliercategory_name"].ToString()) ? "" : dt.Rows[i]["suppliercategory_name"].ToString());
                    objModel._SupplierStatus = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_status"].ToString()) ? "" : dt.Rows[i]["supplierheader_status"].ToString());
                    objModel._RequestType = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_requesttype"].ToString()) ? "" : dt.Rows[i]["supplierheader_requesttype"].ToString());
                    objModel._Requeststatus = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_requeststatus"].ToString()) ? "" : dt.Rows[i]["supplierheader_requeststatus"].ToString());
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


        public IEnumerable<SupplierHeader> GetStatusGridSupplierDetails()
        {
            List<SupplierHeader> objSupHeaderDetails = new List<SupplierHeader>();
            try
            {

                SupplierHeader objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getsupplierheaderdetails";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierHeader();
                    objModel._HeaderID = Convert.ToInt32(dt.Rows[i]["supplierheader_gid"].ToString());
                    objModel._SupplierCode = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_suppliercode"].ToString()) ? "" : dt.Rows[i]["supplierheader_suppliercode"].ToString());
                    objModel._SupplierName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_name"].ToString()) ? "" : dt.Rows[i]["supplierheader_name"].ToString());
                    objModel._SupplierCategoryName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["suppliercategory_name"].ToString()) ? "" : dt.Rows[i]["suppliercategory_name"].ToString());
                    objModel._CompanyRegNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_companyregno"].ToString()) ? "" : dt.Rows[i]["supplierheader_companyregno"].ToString());
                    objModel._OwnerCode = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["employee_code"].ToString()) ? "" : dt.Rows[i]["employee_code"].ToString());
                    objModel._OwnerName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["employee_name"].ToString()) ? "" : dt.Rows[i]["employee_name"].ToString());
                    objModel._ContractFrom = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_contractfrom"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["supplierheader_contractfrom"].ToString()));
                    objModel._ContractTo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_contractto"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["supplierheader_contractto"].ToString()));
                    objModel._RenewalDate = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_renewaldate"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["supplierheader_renewaldate"].ToString()));
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


        public List<SupplierHeader> GetSupplierNameList()
        {
            List<SupplierHeader> objSupHeaderDetails = new List<SupplierHeader>();
            try
            {

                SupplierHeader objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supnamefor", SqlDbType.VarChar).Value = "search";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getsuppliernamesearch";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierHeader();
                    objModel._SupplierName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_name"].ToString()) ? "" : dt.Rows[i]["supplierheader_name"].ToString());
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


        public List<SupActivity> GetContactPersonNameList()
        {
            List<SupActivity> objSupContactPersonDetails = new List<SupActivity>();
            try
            {

                SupActivity objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_tcontactperson", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupActivity();
                    objModel._ContactPerson = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["contactperson_gid"].ToString()) ? "" : dt.Rows[i]["contactperson_gid"].ToString());
                    objModel._ContactPersonName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["contactperson_name"].ToString()) ? "" : dt.Rows[i]["contactperson_name"].ToString());
                    objSupContactPersonDetails.Add(objModel);
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
            return objSupContactPersonDetails;
        }
        public IEnumerable<SupplierHeader> GetSupHeaderDetailssearch(string supname, string supcode, string aging)
        {
            List<SupplierHeader> objSupHeaderDetails = new List<SupplierHeader>();
            try
            {
                HttpContext.Current.Session["ModificationQueueSearch"] = null;
                string protype = "";
                if (HttpContext.Current.Session["Supplierrenewalpro"] != null)
                {
                    protype = "pr_asms_GetCSC";

                }
                else if (HttpContext.Current.Session["Totalsuppliers"] != null)
                {
                    protype = "pr_asms_GetDashboardDetailsstatus";
                }
                else
                {
                    protype = "pr_asms_GetCSC";
                }

                SupplierHeader objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand(protype, con);
                cmd.CommandType = CommandType.StoredProcedure;
                string actiontype = "";
                string frstval = "";
                string scndval = "";

                if (HttpContext.Current.Session["Supplierrenewalpro"] != null)
                {
                    if (aging == "-1")
                    {
                        frstval = "0";
                        scndval = "0";

                    }
                    else if (aging == "0")
                    {
                        //frstval = "0";
                        //scndval = "0";
                        frstval = "30";
                        scndval = "0";
                        cmd.Parameters.Add("@cityid", SqlDbType.Int).Value = Convert.ToInt32(frstval);
                    }
                    else if (aging == "30")
                    {
                        frstval = "30";
                        // scndval = "0";
                        scndval = "1";
                        cmd.Parameters.Add("@cityid", SqlDbType.Int).Value = Convert.ToInt32(frstval);
                        cmd.Parameters.Add("@stateid", SqlDbType.Int).Value = Convert.ToInt32(scndval);
                    }
                    else if (aging == "60")
                    {
                        frstval = "60";
                        scndval = "30";
                        cmd.Parameters.Add("@cityid", SqlDbType.Int).Value = Convert.ToInt32(frstval);
                        cmd.Parameters.Add("@stateid", SqlDbType.Int).Value = Convert.ToInt32(scndval);
                    }
                    else
                    {
                        frstval = "90";
                        scndval = "61";
                        cmd.Parameters.Add("@cityid", SqlDbType.Int).Value = Convert.ToInt32(frstval);
                        cmd.Parameters.Add("@stateid", SqlDbType.Int).Value = Convert.ToInt32(scndval);
                    }
                    actiontype = "getcurrentapprovaldetailsrevsearch";
                    cmd.Parameters.Add("@supname", SqlDbType.VarChar).Value = Convert.ToString(supname);
                    cmd.Parameters.Add("@taxname", SqlDbType.VarChar).Value = Convert.ToString(supcode);
                }
                else if (HttpContext.Current.Session["Totalsuppliers"] != null)
                {
                    actiontype = HttpContext.Current.Session["Totalsuppliers"].ToString();
                    cmd.Parameters.Add("@supname", SqlDbType.VarChar).Value = Convert.ToString(supname);
                    cmd.Parameters.Add("@taxname", SqlDbType.VarChar).Value = Convert.ToString(supcode);
                }
                else
                {
                    if (HttpContext.Current.Session["reqstatus"] != null)
                    {
                        actiontype = "getsupplierheaderdetailssearchdr";
                        cmd.Parameters.Add("@prodserviceflag", SqlDbType.VarChar).Value = Convert.ToString(HttpContext.Current.Session["reqstatus"].ToString());
                    }
                    else
                    {
                        actiontype = "getsupplierheaderdetailssearch";
                    }
                    cmd.Parameters.Add("@supname", SqlDbType.VarChar).Value = Convert.ToString(supname);
                    cmd.Parameters.Add("@taxname", SqlDbType.VarChar).Value = Convert.ToString(supcode);
                }
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(objCmnFunctions.GetLoginUserGid());
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = actiontype.ToString();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierHeader();
                    objModel._HeaderID = Convert.ToInt32(dt.Rows[i]["supplierheader_gid"].ToString());
                    objModel._SupplierCode = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_suppliercode"].ToString()) ? "" : dt.Rows[i]["supplierheader_suppliercode"].ToString());
                    objModel._SupplierName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_name"].ToString()) ? "" : dt.Rows[i]["supplierheader_name"].ToString());
                    objModel._SupplierCategoryName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["suppliercategory_name"].ToString()) ? "" : dt.Rows[i]["suppliercategory_name"].ToString());
                    objModel._CompanyRegNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_companyregno"].ToString()) ? "" : dt.Rows[i]["supplierheader_companyregno"].ToString());
                    objModel._OwnerCode = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["employee_code"].ToString()) ? "" : dt.Rows[i]["employee_code"].ToString());
                    objModel._OwnerName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["employee_name"].ToString()) ? "" : dt.Rows[i]["employee_name"].ToString());
                    objModel._ContractFrom = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_contractfrom"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["supplierheader_contractfrom"].ToString()));
                    objModel._ContractTo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_contractto"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["supplierheader_contractto"].ToString()));
                    objModel._RenewalDate = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_renewaldate"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["supplierheader_renewaldate"].ToString()));

                    if (HttpContext.Current.Session["Supplierrenewalpro"] != null)
                    {
                        objModel._Activitycount = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["activitycount"].ToString()) ? "" : dt.Rows[i]["activitycount"].ToString());
                        objModel._AgeingBucket = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["DiffDate"].ToString()) ? "" : dt.Rows[i]["DiffDate"].ToString());
                    }
                    else
                    {
                        objModel._Activitycount = "";
                        objModel._AgeingBucket = "";
                    }
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

        public IEnumerable<ModificationSummary> GetModSummaySupplierHeader()
        {
            List<ModificationSummary> objSupHeaderDetails = new List<ModificationSummary>();
            try
            {

                ModificationSummary objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetModificationSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "supplierheader";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ModificationSummary();
                    objModel._ColumnName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["column_names"].ToString()) ? "" : dt.Rows[i]["column_names"].ToString());
                    objModel._ModifiedValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["modified_value"].ToString()) ? "" : dt.Rows[i]["modified_value"].ToString());
                    objModel._OldValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["old_value"].ToString()) ? "" : dt.Rows[i]["old_value"].ToString());
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

        public IEnumerable<ModificationSummary> GetModSummayDirectors()
        {
            List<ModificationSummary> objSupHeaderDetails = new List<ModificationSummary>();
            try
            {

                ModificationSummary objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetModificationSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "directorssummary";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ModificationSummary();
                    objModel._ColumnName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["column_names"].ToString()) ? "" : dt.Rows[i]["column_names"].ToString());
                    objModel._ModifiedValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["modified_value"].ToString()) ? "" : dt.Rows[i]["modified_value"].ToString());
                    objModel._OldValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["old_value"].ToString()) ? "" : dt.Rows[i]["old_value"].ToString());
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

        public IEnumerable<ModificationSummary> GetModSummayServicesHistory()
        {
            List<ModificationSummary> objSupHeaderDetails = new List<ModificationSummary>();
            try
            {

                ModificationSummary objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetModificationSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "supplierprofileservicehistory";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ModificationSummary();
                    objModel._ColumnName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["column_names"].ToString()) ? "" : dt.Rows[i]["column_names"].ToString());
                    objModel._ModifiedValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["modified_value"].ToString()) ? "" : dt.Rows[i]["modified_value"].ToString());
                    objModel._OldValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["old_value"].ToString()) ? "" : dt.Rows[i]["old_value"].ToString());
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
        public IEnumerable<ModificationSummary> GetModSummayBranchDetails()
        {
            List<ModificationSummary> objSupHeaderDetails = new List<ModificationSummary>();
            try
            {

                ModificationSummary objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetModificationSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "supplierprofilebranchdetails";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ModificationSummary();
                    objModel._ColumnName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["column_names"].ToString()) ? "" : dt.Rows[i]["column_names"].ToString());
                    objModel._ModifiedValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["modified_value"].ToString()) ? "" : dt.Rows[i]["modified_value"].ToString());
                    objModel._OldValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["old_value"].ToString()) ? "" : dt.Rows[i]["old_value"].ToString());
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
        public IEnumerable<ModificationSummary> GetModSummayManpowerDetails()
        {
            List<ModificationSummary> objSupHeaderDetails = new List<ModificationSummary>();
            try
            {

                ModificationSummary objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetModificationSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "supplierprofilemanpowerdetails";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ModificationSummary();
                    objModel._ColumnName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["column_names"].ToString()) ? "" : dt.Rows[i]["column_names"].ToString());
                    objModel._ModifiedValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["modified_value"].ToString()) ? "" : dt.Rows[i]["modified_value"].ToString());
                    objModel._OldValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["old_value"].ToString()) ? "" : dt.Rows[i]["old_value"].ToString());
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


        public IEnumerable<ModificationSummary> GetModSummayProductServiceDetails()
        {
            List<ModificationSummary> objSupHeaderDetails = new List<ModificationSummary>();
            try
            {

                ModificationSummary objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetModificationSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "productservicedetails";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ModificationSummary();
                    objModel._ColumnName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["column_names"].ToString()) ? "" : dt.Rows[i]["column_names"].ToString());
                    objModel._ModifiedValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["modified_value"].ToString()) ? "" : dt.Rows[i]["modified_value"].ToString());
                    objModel._OldValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["old_value"].ToString()) ? "" : dt.Rows[i]["old_value"].ToString());
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

        public IEnumerable<ModificationSummary> GetModSummaySubContractorDetails()
        {
            List<ModificationSummary> objSupHeaderDetails = new List<ModificationSummary>();
            try
            {

                ModificationSummary objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetModificationSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "subcontractorservicedetails";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ModificationSummary();
                    objModel._ColumnName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["column_names"].ToString()) ? "" : dt.Rows[i]["column_names"].ToString());
                    objModel._ModifiedValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["modified_value"].ToString()) ? "" : dt.Rows[i]["modified_value"].ToString());
                    objModel._OldValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["old_value"].ToString()) ? "" : dt.Rows[i]["old_value"].ToString());
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

        public IEnumerable<ModificationSummary> GetModSummayCustomerDetails()
        {
            List<ModificationSummary> objSupHeaderDetails = new List<ModificationSummary>();
            try
            {

                ModificationSummary objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetModificationSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "customerdetailssummary";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ModificationSummary();
                    objModel._ColumnName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["column_names"].ToString()) ? "" : dt.Rows[i]["column_names"].ToString());
                    objModel._ModifiedValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["modified_value"].ToString()) ? "" : dt.Rows[i]["modified_value"].ToString());
                    objModel._OldValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["old_value"].ToString()) ? "" : dt.Rows[i]["old_value"].ToString());
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

        public IEnumerable<ModificationSummary> GetModSummayBranchCityDetails()
        {
            List<ModificationSummary> objSupHeaderDetails = new List<ModificationSummary>();
            try
            {

                ModificationSummary objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetModificationSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "branchcitydetailssummary";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ModificationSummary();
                    objModel._ColumnName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["column_names"].ToString()) ? "" : dt.Rows[i]["column_names"].ToString());
                    objModel._ModifiedValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["modified_value"].ToString()) ? "" : dt.Rows[i]["modified_value"].ToString());
                    objModel._OldValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["old_value"].ToString()) ? "" : dt.Rows[i]["old_value"].ToString());
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


        public IEnumerable<ModificationSummary> GetModSummayAwardDetails()
        {
            List<ModificationSummary> objSupHeaderDetails = new List<ModificationSummary>();
            try
            {

                ModificationSummary objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetModificationSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "awarddetailssummary";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ModificationSummary();
                    objModel._ColumnName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["column_names"].ToString()) ? "" : dt.Rows[i]["column_names"].ToString());
                    objModel._ModifiedValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["modified_value"].ToString()) ? "" : dt.Rows[i]["modified_value"].ToString());
                    objModel._OldValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["old_value"].ToString()) ? "" : dt.Rows[i]["old_value"].ToString());
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

        public IEnumerable<ModificationSummary> GetModSummayClientDetails()
        {
            List<ModificationSummary> objSupHeaderDetails = new List<ModificationSummary>();
            try
            {

                ModificationSummary objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetModificationSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "clientdetailssummary";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ModificationSummary();
                    objModel._ColumnName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["column_names"].ToString()) ? "" : dt.Rows[i]["column_names"].ToString());
                    objModel._ModifiedValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["modified_value"].ToString()) ? "" : dt.Rows[i]["modified_value"].ToString());
                    objModel._OldValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["old_value"].ToString()) ? "" : dt.Rows[i]["old_value"].ToString());
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

        public IEnumerable<ModificationSummary> GetModSummaySupContactDetails()
        {
            List<ModificationSummary> objSupHeaderDetails = new List<ModificationSummary>();
            try
            {

                ModificationSummary objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetModificationSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "contactdetailssummary";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ModificationSummary();
                    objModel._ColumnName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["column_names"].ToString()) ? "" : dt.Rows[i]["column_names"].ToString());
                    objModel._ModifiedValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["modified_value"].ToString()) ? "" : dt.Rows[i]["modified_value"].ToString());
                    objModel._OldValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["old_value"].ToString()) ? "" : dt.Rows[i]["old_value"].ToString());
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

        public IEnumerable<ModificationSummary> GetModSummayContactPersonDetails()
        {
            List<ModificationSummary> objSupHeaderDetails = new List<ModificationSummary>();
            try
            {

                ModificationSummary objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetModificationSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "contactpersonsummary";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ModificationSummary();
                    objModel._ColumnName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["column_names"].ToString()) ? "" : dt.Rows[i]["column_names"].ToString());
                    objModel._ModifiedValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["modified_value"].ToString()) ? "" : dt.Rows[i]["modified_value"].ToString());
                    objModel._OldValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["old_value"].ToString()) ? "" : dt.Rows[i]["old_value"].ToString());
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


        public IEnumerable<ModificationSummary> GetModSummayTaxDetails()
        {
            List<ModificationSummary> objSupHeaderDetails = new List<ModificationSummary>();
            try
            {

                ModificationSummary objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetModificationSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "taxdetailssummary";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ModificationSummary();
                    objModel._ColumnName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["column_names"].ToString()) ? "" : dt.Rows[i]["column_names"].ToString());
                    objModel._ModifiedValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["modified_value"].ToString()) ? "" : dt.Rows[i]["modified_value"].ToString());
                    objModel._OldValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["old_value"].ToString()) ? "" : dt.Rows[i]["old_value"].ToString());
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

        public IEnumerable<ModificationSummary> GetModSummayTaxSubTypeDetails()
        {
            List<ModificationSummary> objSupHeaderDetails = new List<ModificationSummary>();
            try
            {

                ModificationSummary objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetModificationSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "taxdetailssubtypesummary";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ModificationSummary();
                    objModel._ColumnName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["column_names"].ToString()) ? "" : dt.Rows[i]["column_names"].ToString());
                    objModel._ModifiedValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["modified_value"].ToString()) ? "" : dt.Rows[i]["modified_value"].ToString());
                    objModel._OldValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["old_value"].ToString()) ? "" : dt.Rows[i]["old_value"].ToString());
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

        public IEnumerable<ModificationSummary> GetModSummayPaymentDetails()
        {
            List<ModificationSummary> objSupHeaderDetails = new List<ModificationSummary>();
            try
            {

                ModificationSummary objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetModificationSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "paymentdetailssummary";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ModificationSummary();
                    objModel._ColumnName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["column_names"].ToString()) ? "" : dt.Rows[i]["column_names"].ToString());
                    objModel._ModifiedValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["modified_value"].ToString()) ? "" : dt.Rows[i]["modified_value"].ToString());
                    objModel._OldValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["old_value"].ToString()) ? "" : dt.Rows[i]["old_value"].ToString());
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

        public IEnumerable<ModificationSummary> GetModSummayActivityDetails()
        {
            List<ModificationSummary> objSupHeaderDetails = new List<ModificationSummary>();
            try
            {

                ModificationSummary objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetModificationSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "activitydetails";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ModificationSummary();
                    objModel._ColumnName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["column_names"].ToString()) ? "" : dt.Rows[i]["column_names"].ToString());
                    objModel._ModifiedValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["modified_value"].ToString()) ? "" : dt.Rows[i]["modified_value"].ToString());
                    objModel._OldValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["old_value"].ToString()) ? "" : dt.Rows[i]["old_value"].ToString());
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

        public IEnumerable<ModificationSummary> GetModSummaySupAttachmentsDetails()
        {
            List<ModificationSummary> objSupHeaderDetails = new List<ModificationSummary>();
            try
            {

                ModificationSummary objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetModificationSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "supplierattachmentsummary";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ModificationSummary();
                    objModel._ColumnName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["column_names"].ToString()) ? "" : dt.Rows[i]["column_names"].ToString());
                    objModel._ModifiedValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["modified_value"].ToString()) ? "" : dt.Rows[i]["modified_value"].ToString());
                    objModel._OldValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["old_value"].ToString()) ? "" : dt.Rows[i]["old_value"].ToString());
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

        public IEnumerable<ModificationSummary> GetModSummaySupOthersDetails()
        {
            List<ModificationSummary> objSupHeaderDetails = new List<ModificationSummary>();
            try
            {

                ModificationSummary objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetModificationSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "otherssummary";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ModificationSummary();
                    objModel._ColumnName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["column_names"].ToString()) ? "" : dt.Rows[i]["column_names"].ToString());
                    objModel._ModifiedValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["modified_value"].ToString()) ? "" : dt.Rows[i]["modified_value"].ToString());
                    objModel._OldValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["old_value"].ToString()) ? "" : dt.Rows[i]["old_value"].ToString());
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

        public IEnumerable<ModificationSummary> GetModSummayEmpRelationshipDetails()
        {
            List<ModificationSummary> objSupHeaderDetails = new List<ModificationSummary>();
            try
            {

                ModificationSummary objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetModificationSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "emprelationshipsummary";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new ModificationSummary();
                    objModel._ColumnName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["column_names"].ToString()) ? "" : dt.Rows[i]["column_names"].ToString());
                    objModel._ModifiedValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["modified_value"].ToString()) ? "" : dt.Rows[i]["modified_value"].ToString());
                    objModel._OldValue = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["old_value"].ToString()) ? "" : dt.Rows[i]["old_value"].ToString());
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

        public IEnumerable<SupplierHeader> GetFinancialReviewSummary()
        {
            List<SupplierHeader> objSupHeaderDetails = new List<SupplierHeader>();
            try
            {

                SupplierHeader objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetFinancialReviewSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@empgid", SqlDbType.VarChar).Value = Convert.ToString(objCmnFunctions.GetLoginUserGid());
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getfinancialreviewsummary";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierHeader();
                    objModel._HeaderID = Convert.ToInt32(dt.Rows[i]["supplierheader_gid"].ToString());
                    objModel._SupplierCode = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_suppliercode"].ToString()) ? "" : dt.Rows[i]["supplierheader_suppliercode"].ToString());
                    objModel._SupplierName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_name"].ToString()) ? "" : dt.Rows[i]["supplierheader_name"].ToString());
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

        public string GetCityName(int cityId)
        {
            try
            {
                GetConnection();
                string[,] parameter = new string[,]
                {
                    {"@stateid",cityId.ToString()},
                    {"@action","getcityname"},
                };
                return objCommonIUD.ProcedureCommon(parameter, "pr_asms_GetCSC");
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

        public void UpdateFinReviewStatus()
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_GetFinancialReviewSummary", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "updatefinancialreviewstatus";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
        }


        public List<SupplierHeader> GetSupplierPanNoList()
        {
            List<SupplierHeader> objSupHeaderDetails = new List<SupplierHeader>();
            try
            {

                SupplierHeader objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supnamefor", SqlDbType.VarChar).Value = "search";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getsupplierpannosearch";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierHeader();
                    objModel._PanNo = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["supplierheader_panno"].ToString()) ? "" : dt.Rows[i]["supplierheader_panno"].ToString());
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


        public IEnumerable<SupplierTaxDetails> GetTdsAttachment()
        {
            List<SupplierTaxDetails> objSupAttachment = new List<SupplierTaxDetails>();
            try
            {

                SupplierTaxDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_trn_tdsattachment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@tdsdetailsgid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["TdsDetailsGid"]);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierTaxDetails();
                    objModel._TdsAttachmentID = Convert.ToInt32(dt.Rows[i]["tdsattachment_gid"].ToString());
                    objModel._TdsDocumentGroupID = Convert.ToInt32(dt.Rows[i]["tdsattachment_documentgroup_gid"].ToString());
                    objModel._TdsDocumentGroupName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["documentgroup_name"].ToString()) ? "" : dt.Rows[i]["documentgroup_name"].ToString());
                    objModel._TdsDocumentNameID = Convert.ToInt32(dt.Rows[i]["tdsattachment_documentname_gid"].ToString());
                    objModel._TdsDocumentName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["documentname_name"].ToString()) ? "" : dt.Rows[i]["documentname_name"].ToString());
                    objModel._TdsAttachmentFileName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["tdsattachment_filename"].ToString()) ? "" : dt.Rows[i]["tdsattachment_filename"].ToString());
                    objModel._TdsAttachmentActualFileName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["tdsattachment_actual_filename"].ToString()) ? "" : dt.Rows[i]["tdsattachment_actual_filename"].ToString());
                    objModel._TdsAttachmentDescription = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["tdsattachment_description"].ToString()) ? "" : dt.Rows[i]["tdsattachment_description"].ToString());
                    objModel._TdsAttachmentDate = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["tdsattachment_date"].ToString()) ? "" : objCmnFunctions.ddMMyyyyString(dt.Rows[i]["tdsattachment_date"].ToString()));
                    objSupAttachment.Add(objModel);
                }

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
            return objSupAttachment;
        }

        public SupplierTaxDetails GetTdsAttachmentById(int TdsAttachmentId)
        {
            throw new NotImplementedException();
        }

        public void InsertTdsAttachment(SupplierTaxDetails TdsAttachments)
        {
            try
            {
                string[,] codes = new string[,]
                   {
                     {"tdsattachment_documentgroup_gid",Convert.ToString(TdsAttachments.selectedTdsDocumentGroupID)},
                     {"tdsattachment_documentname_gid",Convert.ToString(TdsAttachments.selectedTdsDocumentNameID) },
                     {"tdsattachment_filename",Convert.ToString(string.IsNullOrEmpty(TdsAttachments._TdsAttachmentFileName)?"":TdsAttachments._TdsAttachmentFileName) },
                     {"tdsattachment_actual_filename",Convert.ToString(string.IsNullOrEmpty(TdsAttachments._TdsAttachmentActualFileName)?"":TdsAttachments._TdsAttachmentActualFileName) },
                     {"tdsattachment_description",Convert.ToString(string.IsNullOrEmpty(TdsAttachments._TdsAttachmentDescription)?"": TdsAttachments._TdsAttachmentDescription)},
                     {"tdsattachment_tdsdetails_gid",Convert.ToString(HttpContext.Current.Session["TdsDetailsGid"]) },
                     {"tdsattachment_date","sysdatetime()" },
                     {"tdsattachment_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"tdsattachment_insert_date","sysdatetime()" }
                   };
                //GetPageMode();
                //string tname = "";
                //if (pageMode == 1 || pageMode == 2 || pageMode == 3)
                //{
                //    tname = "asms_tmp_ttdsattachment";
                //}
                //else
                //{
                //    tname = "asms_trn_ttdsattachment";
                //}
                string insertcommand = objCommonIUD.InsertCommon(codes, "asms_tmp_ttdsattachment");
                string[,] codes2 = new string[,] 
                   {
                     {"tdsattachment_documentgroup_gid",Convert.ToString(TdsAttachments.selectedTdsDocumentGroupID)},
                     {"tdsattachment_documentname_gid",Convert.ToString(TdsAttachments.selectedTdsDocumentNameID) },
                     {"tdsattachment_filename",Convert.ToString(string.IsNullOrEmpty(TdsAttachments._TdsAttachmentFileName)?"":TdsAttachments._TdsAttachmentFileName) },
                     {"tdsattachment_actual_filename",Convert.ToString(string.IsNullOrEmpty(TdsAttachments._TdsAttachmentActualFileName)?"":TdsAttachments._TdsAttachmentActualFileName) },
                     {"tdsattachment_description",Convert.ToString(string.IsNullOrEmpty(TdsAttachments._TdsAttachmentDescription)?"": TdsAttachments._TdsAttachmentDescription)},
                     {"tdsattachment_tdsdetails_gid",Convert.ToString(HttpContext.Current.Session["TdsDetailsGid"]) },
                     {"tdsattachment_date","sysdatetime()" },
                     {"tdsattachment_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"tdsattachment_insert_date","sysdatetime()" },
                     {"tdsattachment_isremoved","Y" }
                   };
                string insertcommand2 = objCommonIUD.InsertCommon(codes2, "asms_trn_ttdsattachment");
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

        public void DeleteTdsAttachment(int TdsAttachmentId)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_trn_tdsattachment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = TdsAttachmentId;
                //   cmd.Parameters.Add("@tdsdetailsgid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["TdsDetailsGid"]);
                cmd.Parameters.Add("@pagemode", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@touchedby", SqlDbType.Int).Value = objCmnFunctions.GetLoginUserGid();
                cmd.Parameters.Add("@touchedon", SqlDbType.VarChar).Value = objCommonIUD.GetCurrentDate();
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "delete";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {

            }
        }

        public void UpdateTdsAttachment(SupplierTaxDetails TdsAttachments)
        {
            throw new NotImplementedException();
        }


        public int CheckSupNameIsExists(string SupplierName, string PanNo)
        {
            try
            {
                int result = 0;
                GetConnection();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (HttpContext.Current.Session["SupplierHeaderGid"] != null)
                {
                    cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                }
                else
                {
                    cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = 0;
                }
                cmd.Parameters.Add("@suppanno", SqlDbType.VarChar).Value = PanNo;
                cmd.Parameters.Add("@supname", SqlDbType.VarChar).Value = SupplierName;
                cmd.Parameters.Add("@supnamefor", SqlDbType.VarChar).Value = "check";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getsuppliernamesearch";
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
        public int CheckSupPanNoIsExists(string SupplierPanNo)
        {
            try
            {
                int result = 0;
                GetConnection();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                if (HttpContext.Current.Session["SupplierHeaderGid"] != null)
                {
                    cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                }
                else
                {
                    cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = 0;
                }
                cmd.Parameters.Add("@suppanno", SqlDbType.VarChar).Value = SupplierPanNo;
                cmd.Parameters.Add("@supnamefor", SqlDbType.VarChar).Value = "check";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getsupplierpannosearch";
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


        public List<SupplierContactDetails> GetAllState()
        {
            List<SupplierContactDetails> objSupContact = new List<SupplierContactDetails>();
            try
            {

                SupplierContactDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getallstate";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierContactDetails();
                    objModel._StateID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["state_gid"].ToString()) ? "0" : dt.Rows[i]["state_gid"].ToString());
                    objModel._StateName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["state_name"].ToString()) ? "" : dt.Rows[i]["state_name"].ToString());
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

        public List<SupplierContactDetails> GetAllCity()
        {
            List<SupplierContactDetails> objSupContact = new List<SupplierContactDetails>();
            try
            {

                SupplierContactDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getallcity";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierContactDetails();
                    objModel._CityID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["city_gid"].ToString()) ? "" : dt.Rows[i]["city_gid"].ToString());
                    objModel._CityName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["city_name"].ToString()) ? "" : dt.Rows[i]["city_name"].ToString());
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

        public List<SupplierBranchDetails> GetAllCityBranch()
        {
            List<SupplierBranchDetails> objSupCity = new List<SupplierBranchDetails>();
            try
            {

                SupplierBranchDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getallcity";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierBranchDetails();
                    objModel._CityID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["city_gid"].ToString()) ? "" : dt.Rows[i]["city_gid"].ToString());
                    objModel._CityName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["city_name"].ToString()) ? "" : dt.Rows[i]["city_name"].ToString());
                    objSupCity.Add(objModel);
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
            return objSupCity;
        }

        public string GetSupIdForMail(int id)
        {
            try
            {
                GetConnection();
                int LOGINID = objCmnFunctions.GetLoginUserGid();
                string lsParameter = "";
                string lsQry = "";
                lsQry += "SELECT max(queue_gid) AS queue_gid FROM iem_trn_tqueue ";
                lsQry += " INNER JOIN asms_trn_tsupplierheader ON asms_trn_tsupplierheader.supplierheader_gid=iem_trn_tqueue.queue_ref_gid";
                lsQry += " WHERE  queue_ref_gid='" + id.ToString() + "'  and ((queue_from='" + LOGINID.ToString() + "'  and  queue_isremoved='N') or (queue_action_by='" + LOGINID.ToString() + "' and queue_isremoved='Y'))";
                cmd.Connection = con;
                //con.Open();
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = lsQry;
                lsParameter = Convert.ToString(cmd.ExecuteScalar());
                con.Close();
                return lsParameter;
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

        public string IsExistsFlagApprover()
        {
            try
            {
                string result = "";
                GetConnection();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@empgid", SqlDbType.VarChar).Value = Convert.ToString(objCmnFunctions.GetLoginUserGid());
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "isexistsflag";
                result = Convert.ToString(cmd.ExecuteScalar());
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

        public DataTable GetMailDetails()
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();

                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getemaildetails";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return dt;
        }


        public int ChkEmpIsFinanceReviewer()
        {
            try
            {
                int result = 0;
                GetConnection();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(objCmnFunctions.GetLoginUserGid());
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getemployeerolegroup";
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


        public string GetSupplierNameForPayment()
        {
            try
            {
                string result = "";
                GetConnection();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(objCmnFunctions.GetLoginUserGid());
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getsuppliernameforpayment";
                result = Convert.ToString(cmd.ExecuteScalar());
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


        public IEnumerable<SupplierHeader> IsSupplierLocked(string SupplierCode)
        {
            List<SupplierHeader> lst = new List<SupplierHeader>();
            try
            {

                SupplierHeader objModel;
                DataTable dt = new DataTable();
                GetConnection();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@suppliercode", SqlDbType.VarChar).Value = SupplierCode.ToUpper().Trim();
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(objCmnFunctions.GetLoginUserGid());
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "issupplierlocked";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        objModel = new SupplierHeader();
                        objModel._LockedByCode = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["lockedby_code"].ToString()) ? "" : dt.Rows[i]["lockedby_code"].ToString());
                        objModel._LockedByName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["lockedby_name"].ToString()) ? "" : dt.Rows[i]["lockedby_name"].ToString());
                        objModel._LockedDate = string.IsNullOrEmpty(dt.Rows[i]["supplierheader_logeddate"].ToString()) ? "" : dt.Rows[i]["supplierheader_logeddate"].ToString();
                        lst.Add(objModel);
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
            return lst;
        }

        public void LockMySupplier(string SupplierCode)
        {
            try
            {
                string[,] codes = new string[,]
                   {
                     {"supplierheader_logedstatus","Y" },
                     {"supplierheader_logedby",Convert.ToString(objCmnFunctions.GetLoginUserGid()) },
                     {"supplierheader_logeddate","sysdatetime()" }
                   };
                string[,] whrcol = new string[,]
	                 {
                          {"supplierheader_suppliercode",  SupplierCode.ToUpper().Trim()},
                          {"supplierheader_isremoved", "N"}
                     };

                string insertcommand = objCommonIUD.UpdateCommon(codes, whrcol, "asms_tmp_tsupplierheader");
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

        public void ReleaseMySupplier(string SupplierCode)
        {
            try
            {
                string[,] codes = new string[,]
                   {
                     {"supplierheader_logedstatus","N" }
                    
                   };
                string[,] whrcol = new string[,]
	                 {
                          {"supplierheader_suppliercode",  SupplierCode.ToUpper().Trim()},
                          {"supplierheader_isremoved", "N"}
                     };

                string insertcommand = objCommonIUD.UpdateCommon(codes, whrcol, "asms_tmp_tsupplierheader");
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


        public List<SupAttachment> GetDocumentNameAll()
        {
            List<SupAttachment> objSupAttachment = new List<SupAttachment>();
            try
            {

                SupAttachment objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_documentname", con);
                cmd.CommandType = CommandType.StoredProcedure;
                // cmd.Parameters.Add("@Grpid", SqlDbType.Int).Value = DocGroupID;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "selectall";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupAttachment();
                    objModel._DocumentNameID = Convert.ToInt32(dt.Rows[i]["documentname_gid"].ToString());
                    objModel._DocumentName = Convert.ToString(dt.Rows[i]["documentname_name"].ToString());
                    objSupAttachment.Add(objModel);
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
            return objSupAttachment;
        }

        public List<SupAttachment> GetDocumentGroupById(int DocNameID)
        {
            List<SupAttachment> objSupAttachment = new List<SupAttachment>();
            try
            {

                SupAttachment objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_asms_mst_documentgroup", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@docnamegid", SqlDbType.Int).Value = DocNameID;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getdocgroupbydocname";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupAttachment();
                    objModel._DocumentGroupID = Convert.ToInt32(dt.Rows[i]["documentgroup_gid"].ToString());
                    objModel._DocumentGroupName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["documentgroup_name"].ToString()) ? "" : dt.Rows[i]["documentgroup_name"].ToString());
                    objSupAttachment.Add(objModel);
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
            return objSupAttachment;
        }


        public string GetSupplierPanNumber()
        {
            try
            {
                string result = "";
                GetConnection();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@supplierheadergid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "getsupplierpannofortds";
                result = Convert.ToString(cmd.ExecuteScalar());
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
        public string DeleteSupplierRenewal(string suppliercode = null)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@suppliercode", SqlDbType.VarChar).Value = string.IsNullOrEmpty(suppliercode) ? "" : suppliercode;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "deletesupplierrenuwal";
                cmd.ExecuteNonQuery();
                return "1";
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
        public string DeleteSupplier(string suppliercode = null)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_asms_GetCSC", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@suppliercode", SqlDbType.VarChar).Value = string.IsNullOrEmpty(suppliercode) ? "" : suppliercode;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "deletesupplier";
                cmd.ExecuteNonQuery();
                return "1";
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
        public DataTable getcontracttodate()
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                SqlCommand cmd = new SqlCommand("select datepickerextendedto from datepicker where datepickerMODULE='ASMS' and  datepickerTypeofpage='ONBOARDINGCONTRACTTODATE' and datepickerisremoved='N'", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return dt;
        }
        //******************************   GST ***********************************************

        public List<EntityGstvendor> GetState()
        {
            //  throw new NotImplementedException();
            try
            {
                List<EntityGstvendor> objOrgType = new List<EntityGstvendor>();
                EntityGstvendor objModel;
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("GST_MST_TMP_SUPPLIER_SL", con);
                cmd.Parameters.AddWithValue("@suppliergst_supplierheader_gid", Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]));
                cmd.Parameters.AddWithValue("@StatementType", "SUPSTATE");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new EntityGstvendor();
                    objModel.suppliergst_stateid = Convert.ToInt32(dt.Rows[i]["state_gid"].ToString());
                    //  objModel.Currencycode = Convert.ToString(dt.Rows[i]["district_code"].ToString());
                    objModel.suppliergst_state = Convert.ToString(dt.Rows[i]["state_name"].ToString());
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

        public EntityGstvendor GetStateById(int Disid)
        {
            //  throw new NotImplementedException();
            try
            {
                List<EntityGstvendor> objOrgType = new List<EntityGstvendor>();
                EntityGstvendor objModel;
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("GST_MST_State_SEL", con);
                cmd.Parameters.AddWithValue("@StatementType", "SID");
                cmd.Parameters.AddWithValue("@state_gid", Disid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objModel = new EntityGstvendor()
                {

                    suppliergst_stateid = Convert.ToInt32(dt.Rows[0]["state_gid"].ToString()),
                    //  objModel.Currencycode = Convert.ToString(dt.Rows[i]["district_code"].ToString());
                    suppliergst_state = Convert.ToString(dt.Rows[0]["state_name"].ToString()),
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
                SqlCommand cmd = new SqlCommand("GST_MST_TMP_SUPPLIER_IU", con);
                GetConnection();
                cmd.Parameters.AddWithValue("@StatementType", "D");
                cmd.Parameters.AddWithValue("@suppliergst_gid", PinId);
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


        //************************  GST *****************************************

        public string Insertvendor(EntityGstvendor Classifications)
        {
            string result;
            string res5 = "";
            // throw new NotImplementedException();
            try
            {

                CommonIUD commn = new CommonIUD();
                SqlCommand cmdn = new SqlCommand("GST_MST_TMP_SUPPLIER_IU", con);
                GetConnection();
                res5 = "YES";
                if (Classifications.suppliergst_app == "Yes")
                {
                    string ss = Classifications.suppliergst_tin.Substring(0, 2);// Classifications.suppliergst_tin.Split(new char[] { ' ' }, 2);
                    cmdn.Parameters.AddWithValue("@StatementType", "C");
                    cmdn.Parameters.AddWithValue("@state_code", ss);
                    cmdn.Parameters.AddWithValue("@suppliergst_state_gid", Convert.ToInt32(Classifications.suppliergst_stateid));
                    cmdn.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                    cmdn.CommandType = CommandType.StoredProcedure;
                    cmdn.ExecuteNonQuery();
                    res5 = cmdn.Parameters["@res"].Value.ToString();
                }
                else
                {
                    Classifications.suppliergst_tin = "";
                    Classifications.suppliergst_vertical = "";
                }
                if (res5 == "YES")
                {
                    Int64 supid = GetSupplierheader();
                    if (supid > 0)
                    {
                        GetConnection();
                        CommonIUD comms = new CommonIUD();
                        SqlCommand cmds = new SqlCommand("GST_MST_TMP_SUPPLIER_IU", con);
                        cmds.Parameters.AddWithValue("@StatementType", "E");
                        cmds.Parameters.AddWithValue("@suppliergst_gid ", "0");
                        cmds.Parameters.AddWithValue("@suppliergst_supplierheader_gid", supid);
                        cmds.Parameters.AddWithValue("@suppliergst_gstin", Classifications.suppliergst_tin);
                        cmds.Parameters.AddWithValue("@suppliergst_vertical", Classifications.suppliergst_vertical);
                        cmds.Parameters.AddWithValue("@suppliergst_state_gid", Convert.ToInt32(Classifications.suppliergst_stateid));
                        cmds.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                        cmds.CommandType = CommandType.StoredProcedure;
                        cmds.ExecuteNonQuery();
                        string res1 = cmds.Parameters["@res"].Value.ToString();
                        if (res1 == "Not There")
                        {
                            CommonIUD comm = new CommonIUD();
                            SqlCommand cmd = new SqlCommand("GST_MST_TMP_SUPPLIER_IU", con);
                            GetConnection();
                            cmd.Parameters.AddWithValue("@StatementType", "I");
                            cmd.Parameters.AddWithValue("@suppliergst_supplierheader_gid", supid);
                            cmd.Parameters.AddWithValue("@suppliergst_gid ", "0");
                            cmd.Parameters.AddWithValue("@suppliergst_gst_app", Classifications.suppliergst_app);
                            cmd.Parameters.AddWithValue("@suppliergst_gstin", Classifications.suppliergst_tin);
                            cmd.Parameters.AddWithValue("@suppliergst_vertical", Classifications.suppliergst_vertical);
                            cmd.Parameters.AddWithValue("@suppliergst_state_gid", Convert.ToInt32(Classifications.suppliergst_stateid));
                            cmd.Parameters.AddWithValue("@suppliergst_status", Classifications.suppliergst_status);
                            cmd.Parameters.AddWithValue("@suppliergst_isremoved", "N");
                            cmd.Parameters.AddWithValue("@suppliergst_insertby", Convert.ToInt32(objCmnFunctions.GetLoginUserGid()));
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

        public List<EntityGstvendor> getvendor()
        {
            try
            {
                List<EntityGstvendor> objOrgType = new List<EntityGstvendor>();
                EntityGstvendor objModel;
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("GST_MST_TMP_SUPPLIER_SL", con);
                cmd.Parameters.AddWithValue("@suppliergst_supplierheader_gid", Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]));
                cmd.Parameters.AddWithValue("@StatementType", "SEL");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new EntityGstvendor();
                    objModel.suppliergst_gid = Convert.ToInt32(dt.Rows[i]["suppliergst_gid"]);
                    objModel.suppliergst_app = dt.Rows[i]["suppliergst_gst_app"].ToString();
                    objModel.suppliergst_stateid = Convert.ToInt32(dt.Rows[i]["suppliergst_state_gid"]);
                    objModel.suppliergst_state = dt.Rows[i]["state_name"].ToString();
                    objModel.suppliergst_tin = dt.Rows[i]["suppliergst_gstin"].ToString();                   
                    objModel.suppliergst_vertical = dt.Rows[i]["suppliergst_vertical"].ToString();
                    objModel.suppliergst_status = dt.Rows[i]["suppliergst_status"].ToString();
                    //objModel.Pincode_isremoved = dt.Rows[0]["pincode_isremoved"].ToString();
                    //objModel.Pincode_insertby = dt.Rows[0]["pincode_insertby"].ToString();
                    //objModel.Pincode_updateby = dt.Rows[0]["pincode_updateby"].ToString();
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


        public EntityGstvendor getVendorid(int id)
        {

            try
            {
                List<EntityGstvendor> objOrgType = new List<EntityGstvendor>();
                EntityGstvendor objModel = new EntityGstvendor();
                GetConnection();
                DataTable dt = new DataTable();

                SqlCommand cmd = new SqlCommand("GST_MST_TMP_SUPPLIER_SL", con);
                cmd.Parameters.AddWithValue("@StatementType", "SID");
                cmd.Parameters.AddWithValue("@suppliergst_gid", id);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel.suppliergst_gid = Convert.ToInt32(dt.Rows[i]["suppliergst_gid"]);
                    objModel.suppliergst_app = dt.Rows[i]["suppliergst_gst_app"].ToString();
                    objModel.suppliergst_stateid = Convert.ToInt32(dt.Rows[i]["suppliergst_state_gid"]);
                    objModel.suppliergst_state = dt.Rows[i]["state_name"].ToString();
                    objModel.suppliergst_tin = dt.Rows[i]["suppliergst_gstin"].ToString();
                    objModel.suppliergst_vertical = dt.Rows[i]["suppliergst_vertical"].ToString();
                    objModel.suppliergst_status = dt.Rows[i]["suppliergst_status"].ToString();
                    objModel.selectedstate_gid = Convert.ToInt32(dt.Rows[i]["suppliergst_state_gid"]);
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

        public string Updatevendor(EntityGstvendor Classifications)
        {
            string result;
            // throw new NotImplementedException();            
            try
            {
                CommonIUD commn = new CommonIUD();
                SqlCommand cmdn = new SqlCommand("GST_MST_TMP_SUPPLIER_IU", con);
                GetConnection();
                string res5 = "YES";
                if (Classifications.suppliergst_app == "Y")
                {
                    string ss = Classifications.suppliergst_tin.Substring(0, 2);// Classifications.suppliergst_tin.Split(new char[] { ' ' }, 2);
                    cmdn.Parameters.AddWithValue("@StatementType", "C");
                    cmdn.Parameters.AddWithValue("@state_code", ss);
                    cmdn.Parameters.AddWithValue("@suppliergst_state_gid", Convert.ToInt32(Classifications.suppliergst_stateid));
                    cmdn.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                    cmdn.CommandType = CommandType.StoredProcedure;
                    cmdn.ExecuteNonQuery();
                    res5 = cmdn.Parameters["@res"].Value.ToString();
                }
                else
                {
                    Classifications.suppliergst_tin = "";
                    Classifications.suppliergst_vertical = "";
                }
                if (res5 == "YES")
                {
                    Int64 supid = GetSupplierheader();
                    if (supid > 0)
                    {
                        CommonIUD comms = new CommonIUD();
                        SqlCommand cmds = new SqlCommand("GST_MST_TMP_SUPPLIER_IU", con);
                        GetConnection();
                        cmds.Parameters.AddWithValue("@StatementType", "E");
                        cmds.Parameters.AddWithValue("@suppliergst_gid ", Classifications.suppliergst_gid);
                        cmds.Parameters.AddWithValue("@suppliergst_gstin", Classifications.suppliergst_tin);
                        cmds.Parameters.AddWithValue("@suppliergst_supplierheader_gid", Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]));
                        cmds.Parameters.AddWithValue("@suppliergst_vertical", Classifications.suppliergst_vertical);
                        cmds.Parameters.AddWithValue("@suppliergst_state_gid", Convert.ToInt32(Classifications.suppliergst_stateid));
                        cmds.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                        cmds.CommandType = CommandType.StoredProcedure;
                        cmds.ExecuteNonQuery();
                        string res1 = cmds.Parameters["@res"].Value.ToString();
                        if (res1 == "Not There")
                        {
                            CommonIUD comm = new CommonIUD();
                            SqlCommand cmd = new SqlCommand("GST_MST_TMP_SUPPLIER_IU", con);
                            GetConnection();
                            cmd.Parameters.AddWithValue("@StatementType", "U");
                            cmd.Parameters.AddWithValue("@suppliergst_supplierheader_gid", supid);
                            cmd.Parameters.AddWithValue("@suppliergst_gid", Classifications.suppliergst_gid);
                            cmd.Parameters.AddWithValue("@suppliergst_gst_app", Classifications.suppliergst_app);
                            cmd.Parameters.AddWithValue("@suppliergst_gstin", Classifications.suppliergst_tin);
                            cmd.Parameters.AddWithValue("@suppliergst_vertical", Classifications.suppliergst_vertical);
                            cmd.Parameters.AddWithValue("@suppliergst_state_gid", Convert.ToInt32(Classifications.suppliergst_stateid));
                            cmd.Parameters.AddWithValue("@suppliergst_status", Classifications.suppliergst_status);
                            cmd.Parameters.AddWithValue("@suppliergst_isremoved", "N");
                            cmd.Parameters.AddWithValue("@approved_status", Classifications.IsChecker);
                            cmd.Parameters.AddWithValue("@suppliergst_updateby", Convert.ToInt32(objCmnFunctions.GetLoginUserGid()));
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


       


        //************************  GST *****************************************

        public List<SupplierContactDetails> GetPincode()
        {
            List<SupplierContactDetails> objSupContact = new List<SupplierContactDetails>();
            try
            {

                SupplierContactDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("GST_MST_PINCODE_SL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@pincode_gid", SqlDbType.Int).Value = Convert.ToInt32(pageMode);
                cmd.Parameters.Add("@StatementType", SqlDbType.VarChar).Value = "SEL1";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierContactDetails();
                    objModel.Pincodeid = Convert.ToInt32(dt.Rows[i]["pincode_gid"].ToString());
                    objModel.Pincode = Convert.ToString(dt.Rows[i]["pincode_code"].ToString());
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


        public List<SupplierContactDetails> Getcitylist(int Pincodeid)
        {
            List<SupplierContactDetails> objSupContact = new List<SupplierContactDetails>();
            try
            {

                SupplierContactDetails objModel;
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
                    objModel = new SupplierContactDetails();
                    objModel._CountryID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["state_country_gid"].ToString()) ? "0" : dt.Rows[i]["state_country_gid"].ToString());
                    objModel._CountryName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["country_name"].ToString()) ? "" : dt.Rows[i]["country_name"].ToString());

                    objModel._StateID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["district_state_gid"].ToString()) ? "0" : dt.Rows[i]["district_state_gid"].ToString());
                    objModel._StateName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["state_name"].ToString()) ? "" : dt.Rows[i]["state_name"].ToString());

                    objModel._DistrictID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["pincode_district_gid"].ToString()) ? "0" : dt.Rows[i]["pincode_district_gid"].ToString());
                    objModel._DistrictName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["district_name"].ToString()) ? "" : dt.Rows[i]["district_name"].ToString());
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

        public string getUserRole()
        {
            try
            {
                string result = "";
                GetConnection();
                cmd = new SqlCommand("RoleBaseAttachment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@empgid", SqlDbType.Int).Value = Convert.ToInt32(objCmnFunctions.GetLoginUserGid());
                result = Convert.ToString(cmd.ExecuteScalar());
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

        public List<SupplierContactDetails> GetAllDistrict()
        {
            List<SupplierContactDetails> objSupContact = new List<SupplierContactDetails>();
            try
            {

                SupplierContactDetails objModel;
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("GST_MST_DISTRICT_SL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@StatementType", SqlDbType.VarChar).Value = "SEL";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupplierContactDetails();
                    objModel._DistrictID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["district_gid"].ToString()) ? "" : dt.Rows[i]["district_gid"].ToString());
                    objModel._DistrictName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["district_name"].ToString()) ? "" : dt.Rows[i]["district_name"].ToString());
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



        public List<SupplierContactDetails> GetDistrictpincode(string Pincodeid)
        {
            List<SupplierContactDetails> objSupContact = new List<SupplierContactDetails>();
            try
            {

                SupplierContactDetails objModel;
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
                    objModel = new SupplierContactDetails();
                    objModel._CountryID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["state_country_gid"].ToString()) ? "0" : dt.Rows[i]["state_country_gid"].ToString());
                    objModel._CountryName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["country_name"].ToString()) ? "" : dt.Rows[i]["country_name"].ToString());

                    objModel._StateID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["district_state_gid"].ToString()) ? "0" : dt.Rows[i]["district_state_gid"].ToString());
                    objModel._StateName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["state_name"].ToString()) ? "" : dt.Rows[i]["state_name"].ToString());

                    objModel._DistrictID = Convert.ToInt32(string.IsNullOrEmpty(dt.Rows[i]["pincode_district_gid"].ToString()) ? "0" : dt.Rows[i]["pincode_district_gid"].ToString());
                    objModel._DistrictName = Convert.ToString(string.IsNullOrEmpty(dt.Rows[i]["district_name"].ToString()) ? "" : dt.Rows[i]["district_name"].ToString());
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

        public IEnumerable<EntityGstvendor> getmaker()
        {
            try
            {
                List<EntityGstvendor> objOrgType = new List<EntityGstvendor>();
                EntityGstvendor objModel;
                GetConnection();
                DataTable dt = new DataTable();

                SqlCommand cmd = new SqlCommand("GST_MST_TMP_SUPPLIER_SL", con);
                cmd.Parameters.AddWithValue("@StatementType", "ROLE");
                cmd.Parameters.AddWithValue("@UId ", int.Parse(objCmnFunctions.GetLoginUserGid().ToString()));
                cmd.Parameters.Add("@suppliergst_supplierheader_gid", SqlDbType.Int).Value = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new EntityGstvendor();
                    objModel.IsChecker = dt.Rows[i]["IsChecker"].ToString();
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

        public long GetSupplierheader()
        {
            Int64 SupHeaderGid = 0;
            try
            {
                SupHeaderGid = Convert.ToInt64(HttpContext.Current.Session["SupplierHeaderGid"]);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                throw ex;
            }
            finally
            {
               // con.Close();
            }
            return SupHeaderGid;
        }

        //************************  GST *****************************************
    }
}
