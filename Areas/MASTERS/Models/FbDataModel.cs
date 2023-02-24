using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using IEM.Areas.MASTERS.Models;
using System.Globalization;
using IEM.Common;
namespace IEM.Areas.MASTERS.Models
{
    public class FbDataModel : FbIrepository
    {
        SqlConnection conn = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable objTable;
        CmnFunctions objCmnFunctions = new CmnFunctions();
        CommonIUD objcommon = new CommonIUD();
        public void getconnection()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                conn.Open();
            }
        }

        public IEnumerable<FbEntity> GetProductServiceDetails()
        {
            try
            {

                getconnection();
                objTable = new DataTable();
                List<FbEntity> objProduct = new List<FbEntity>();
                FbEntity objDetails;
                cmd = new SqlCommand("pr_fb_mst_prodservice", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "Select");
                da = new SqlDataAdapter(cmd);
                da.Fill(objTable);
                DataSet ds = new DataSet();
                ds.Tables.Add(objTable);
                HttpContext.Current.Session["ExcelExportProduct"] = ds;
                for (int i = 0; i < objTable.Rows.Count;i++ )
                {
                    objDetails = new FbEntity();
                    objDetails.slNo = i + 1;
                    //objDetails.productGid = Convert.ToInt32(objTable.Rows[i]["Productservice Gid"]);
                    //objDetails.productServflag = objTable.Rows[i]["Productservice Flag"].ToString();
                    //objDetails.productCode = objTable.Rows[i]["Productservice Code"].ToString();
                    //objDetails.productName = objTable.Rows[i]["Productservice Name"].ToString();
                    //objDetails.productDescription = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(objTable.Rows[i]["Productservice Description"].ToString());
                    //objDetails.productService = objTable.Rows[i]["Productservice Category"].ToString();
                    //objDetails.parentFlag = objTable.Rows[i]["Product Service Parent"].ToString();
                    //objDetails.parentProduct = objTable.Rows[i]["Parent Code"].ToString();
                    //objDetails.assetCategory = objTable.Rows[i]["Assetcategory Name"].ToString();
                    //objDetails.glCode = objTable.Rows[i]["Assetcategory Glno"].ToString();
                    objDetails.productGid = Convert.ToInt32(objTable.Rows[i]["prodservice_gid"]);
                    objDetails.productServflag = objTable.Rows[i]["prodservice_flag"].ToString();
                    objDetails.productCode = objTable.Rows[i]["prodservice_code"].ToString();
                    objDetails.productName = objTable.Rows[i]["prodservice_name"].ToString();
                    objDetails.productDescription = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(objTable.Rows[i]["prodservice_description"].ToString());
                    objDetails.productService = objTable.Rows[i]["prodservice_Category"].ToString();
                    objDetails.parentFlag = objTable.Rows[i]["prodservice_parent"].ToString();
                    objDetails.parentProduct = objTable.Rows[i]["parentcode"].ToString();
                    objDetails.assetCategory = objTable.Rows[i]["assetcategory_name"].ToString();
                    objDetails.glCode = objTable.Rows[i]["assetcategory_glno"].ToString();
                    objProduct.Add(objDetails);
                }
                return objProduct;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public List<FbEntity> GetProductServiceDetailsById(int ProductGid)
        {
            try
            {
                getconnection();
                objTable = new DataTable();
                List<FbEntity> objProduct = new List<FbEntity>();
                FbEntity objDetails;
                cmd = new SqlCommand("pr_fb_mst_prodservice", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prodservicegid", ProductGid);
                cmd.Parameters.AddWithValue("@actionName", "selectprodservicebyid");
                da = new SqlDataAdapter(cmd);
                da.Fill(objTable);
                foreach (DataRow row in objTable.Rows)
                {
                    objDetails = new FbEntity();
                    objDetails.productGid = Convert.ToInt32(row["prodservice_gid"].ToString());
                    objDetails.productServflag = row["prodservice_flag"].ToString();
                    objDetails.productCode = row["prodservice_code"].ToString();
                    objDetails.productName = row["prodservice_name"].ToString();
                    objDetails.productDescription = row["prodservice_description"].ToString();
                    objDetails.category = row["prodservice_category"].ToString();
                    objDetails.parentFlag = row["prodservice_parent"].ToString();
                    objDetails.productParentGid = Convert.ToInt32(string.IsNullOrEmpty(row["prodservice_prodservicegid"].ToString()) ? "0" : row["prodservice_prodservicegid"].ToString());
                    objDetails.parentProduct = row["parentcode"].ToString();
                 //   objDetails.assetCategory = row["assetcategory_name"].ToString();
                    objDetails.SelectedAssetCatGid = Convert.ToInt32(string.IsNullOrEmpty(row["prodservice_assetcategory_gid"].ToString()) ? "0" : row["prodservice_assetcategory_gid"]);
                    objDetails.SelectedAssetSubCatGid = Convert.ToInt32(string.IsNullOrEmpty(row["prodservice_assetsubcategory_gid"].ToString()) ? "0" : row["prodservice_assetsubcategory_gid"]);
                    objDetails.SelectedExpGid = Convert.ToInt32(string.IsNullOrEmpty(row["prodservice_expsubcategory_gid"].ToString()) ? "0" : row["prodservice_expsubcategory_gid"]);
                    objDetails.SelectedExpNatureGid = Convert.ToInt32(string.IsNullOrEmpty(row["prodservice_expcatcategory_gid"].ToString()) ? "0" : row["prodservice_expcatcategory_gid"]);
                    objDetails.glCode = string.IsNullOrEmpty(row["glcode"].ToString()) ? "0" : row["glcode"].ToString();
                    objProduct.Add(objDetails);
                }
                return objProduct;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public string InsertProductServiceDetails(FbEntity objDetails)
        {

            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_mst_prodservice", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "existforinsert");
                if (objDetails.productServflag == "Product")
                {
                    cmd.Parameters.AddWithValue("@prodserviceCode", objDetails.productCode);
                }
                if (objDetails.productServflag == "Service")
                {
                    cmd.Parameters.AddWithValue("@prodserviceCode", objDetails.service);
                }
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 64);
                cmd.Parameters["@res"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res = Convert.ToString(cmd.Parameters["@res"].Value.ToString());
                if (res == "Not there")
                {
                    cmd = new SqlCommand("pr_fb_mst_prodservice", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@actionName", "Insert");
                    cmd.Parameters.AddWithValue("@prodserviceinsertby", objCmnFunctions.GetLoginUserGid());
                    cmd.Parameters.AddWithValue("@prodserviceinsertdate", DateTime.Now.ToString("dd/MMM/yyyy"));
                    cmd.Parameters.AddWithValue("@prodserviceglcode", string.IsNullOrEmpty(objDetails.glCode) ? "" : objDetails.glCode);
                    if (objDetails.category == "Asset")
                    {
                        cmd.Parameters.AddWithValue("@prodserviceassetcategory", objDetails.assetGid);
                        cmd.Parameters.AddWithValue("@prodserviceassetsubcat", objDetails.AssetSubCatGid);
                        cmd.Parameters.AddWithValue("@prodserviceCategory", "A");
                        if (objDetails.productServflag == "Product")
                        {
                            cmd.Parameters.AddWithValue("@prodserviceFlag", "P");
                            cmd.Parameters.AddWithValue("@prodserviceCode", objDetails.productCode);
                            cmd.Parameters.AddWithValue("@prodserviceName", objDetails.productName.ToString().ToUpper());
                            cmd.Parameters.AddWithValue("@prodservicedesc", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(objDetails.productDescription.ToString()));

                            if (objDetails.parentFlag == "Yes")
                            {

                                cmd.Parameters.AddWithValue("@prodserviceParent", "Y");
                            }
                            else
                            {
                               
                                cmd.Parameters.AddWithValue("@prodserviceParent", "N");
                                cmd.Parameters.AddWithValue("@prodserviceparentgid", objDetails.parentProductGid);

                            }
                        }
                        if (objDetails.productServflag == "Service")
                        {
                            cmd.Parameters.AddWithValue("@prodserviceFlag", "S");
                            cmd.Parameters.AddWithValue("@prodserviceCode", objDetails.service);
                            cmd.Parameters.AddWithValue("@prodserviceName", objDetails.serviceName.ToString().ToUpper());
                            cmd.Parameters.AddWithValue("@prodservicedesc", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(objDetails.serviceDescription.ToString()));

                            if (objDetails.parentFlag == "Yes")
                            {
                                cmd.Parameters.AddWithValue("@prodserviceParent", "Y");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@prodserviceParent", "N");
                                cmd.Parameters.AddWithValue("@prodserviceparentgid", objDetails.parentProductGid);
                            }
                        }

                    }
                    if (objDetails.category == "Expense")
                    {
                        cmd.Parameters.AddWithValue("@prodserviceexpcatcategory", objDetails.ExpNatureGid);
                        cmd.Parameters.AddWithValue("@prodserviceexpsubcat", objDetails.expenseGid);
                        cmd.Parameters.AddWithValue("@prodserviceCategory", "E");
                        if (objDetails.productServflag == "Product")
                        {
                            cmd.Parameters.AddWithValue("@prodserviceFlag", "P");
                            cmd.Parameters.AddWithValue("@prodserviceCode", objDetails.productCode);
                            cmd.Parameters.AddWithValue("@prodserviceName", objDetails.productName.ToString().ToUpper());
                            cmd.Parameters.AddWithValue("@prodservicedesc", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(objDetails.productDescription.ToString()));
                            if (objDetails.parentFlag == "Yes")
                            {
                                cmd.Parameters.AddWithValue("@prodserviceParent", "Y");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@prodserviceParent", "N");
                                cmd.Parameters.AddWithValue("@prodserviceparentgid", objDetails.parentProductGid);
                            }
                        }
                        if (objDetails.productServflag == "Service")
                        {

                            cmd.Parameters.AddWithValue("@prodserviceCode", objDetails.service);
                            cmd.Parameters.AddWithValue("@prodserviceName", objDetails.serviceName.ToString().ToUpper());
                            cmd.Parameters.AddWithValue("@prodserviceFlag", "S");
                            if (objDetails.parentFlag == "Yes")
                            {
                                cmd.Parameters.AddWithValue("@prodserviceParent", "Y");
                            }
                            cmd.Parameters.AddWithValue("@prodservicedesc", CultureInfo.CurrentCulture.TextInfo.ToTitleCase((objDetails.serviceDescription.ToString())));
                            if (objDetails.parentFlag == "No")
                            {
                                cmd.Parameters.AddWithValue("@prodserviceParent", "N");
                                cmd.Parameters.AddWithValue("@prodserviceparentgid", objDetails.parentProductGid);
                            }
                        }

                    }
                    objDetails.result = Convert.ToString(cmd.ExecuteNonQuery());
                }
                return objDetails.result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public string UpdateProductServiceDetails(FbEntity objDetails)
        {
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_mst_prodservice", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "Update");
                if (objDetails.productServflag == "Product")
                {
                    cmd.Parameters.AddWithValue("@prodserviceName", objDetails.productName.ToString().ToUpper());
                    cmd.Parameters.AddWithValue("@prodservicedesc", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(objDetails.productDescription.ToString()));
                    if (objDetails.parentFlag == "Yes")
                    {
                        cmd.Parameters.AddWithValue("@prodserviceParent", "Y");
                        if (objDetails.serviceParentGid != null || objDetails.productParentGid != 0)
                        {
                            cmd.Parameters.AddWithValue("@prodserviceparentgid", objDetails.productParentGid);
                        }
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@prodserviceParent", "N");
                        cmd.Parameters.AddWithValue("@prodserviceparentgid", objDetails.productParentGid);

                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@prodserviceName", objDetails.serviceName.ToString().ToUpper());
                    cmd.Parameters.AddWithValue("@prodservicedesc", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(objDetails.serviceDescription.ToString()));
                    if (objDetails.parentFlag == "Yes")
                    {
                      
                        cmd.Parameters.AddWithValue("@prodserviceParent", "Y");
                        if (objDetails.serviceParentGid != null || objDetails.serviceParentGid != 0)
                        {
                            cmd.Parameters.AddWithValue("@prodserviceparentgid", objDetails.serviceParentGid);
                        }
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@prodserviceParent", "N");
                        cmd.Parameters.AddWithValue("@prodserviceparentgid", objDetails.serviceParentGid);
                    }
                   
                }
                if (objDetails.category == "Asset")
                {
                    cmd.Parameters.AddWithValue("@prodserviceCategory", "A");
                    cmd.Parameters.AddWithValue("@prodserviceassetcategory", objDetails.assetGid);
                

                }
                else
                {

                    cmd.Parameters.AddWithValue("@prodserviceCategory", "E");
                    cmd.Parameters.AddWithValue("@prodserviceexpcatcategory", objDetails.expenseGid);
                }
                cmd.Parameters.AddWithValue("@prodserviceupdateby",objCmnFunctions.GetLoginUserGid());
                cmd.Parameters.AddWithValue("@prodserviceupdatedate", DateTime.Now.ToString("dd/MMM/yyyy"));
                cmd.Parameters.AddWithValue("@prodservicegid", objDetails.productId);
                objDetails.result = Convert.ToString(cmd.ExecuteNonQuery());
                return objDetails.result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public void DeleteProductServiceDetails(FbEntity objDelete)
        {
            try
            {

                string[,] wherecond = new string[,]
	        {
                {"prodservice_gid", objDelete.productGid.ToString ()}
            };
                string[,] column = new string[,]
	       {
                 {"prodservice_isremoved", "Y"}
	            
           };
                string tblname = "fb_mst_tprodservice";
                string deletetbl = objcommon.DeleteCommon(column, wherecond, tblname);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<FbEntity> GetServiceListUpd()
        {
            try
            {
                List<FbEntity> objProduct;
                objProduct = new List<FbEntity>();
                FbEntity obj;
                getconnection();
                objTable = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_prodservice", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "UpdService");
                da = new SqlDataAdapter(cmd);
                da.Fill(objTable);
                foreach (DataRow row in objTable.Rows)
                {
                    obj = new FbEntity();
                    obj.serviceName = row["prodservice_code"].ToString();
                    obj.serviceParentGid = Convert.ToInt32(row["prodservice_gid"].ToString());
                    objProduct.Add(obj);
                }
                return objProduct;
            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                conn.Close();
            }

        }
        public List<FbEntity> GetServiceList()
        {
            try
            {
                List<FbEntity> objService;
                objService = new List<FbEntity>();
                FbEntity obj;
                getconnection();
                objTable = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_prodservice", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectService");
                da = new SqlDataAdapter(cmd);
                da.Fill(objTable);
                foreach (DataRow row in objTable.Rows)
                {
                    obj = new FbEntity();
                    obj.serviceName = row["prodservice_code"].ToString();
                    obj.serviceGid = Convert.ToInt32(row["prodservice_gid"].ToString());
                    objService.Add(obj);
                }
                return objService;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public IEnumerable<FbEntity> GetProductListUpd()
        {
            try
            {
                List<FbEntity> objProduct;
                objProduct = new List<FbEntity>();
                FbEntity obj;
                getconnection();
                objTable = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_prodservice", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "UpdProduct");
                da = new SqlDataAdapter(cmd);
                da.Fill(objTable);
                foreach (DataRow row in objTable.Rows)
                {
                    obj = new FbEntity();
                    obj.parentProductName = row["prodservice_code"].ToString();
                    obj.productParentGid = Convert.ToInt32(row["prodservice_gid"].ToString());
                    objProduct.Add(obj);
                }

                return objProduct;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public IEnumerable<FbEntity> GetProductList()
        {
            try
            {
                List<FbEntity> objproduct;
                objproduct = new List<FbEntity>();
                FbEntity obj;
                getconnection();
                objTable = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_prodservice", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectProduct");
                da = new SqlDataAdapter(cmd);
                da.Fill(objTable);
                foreach (DataRow row in objTable.Rows)
                {
                    obj = new FbEntity();
                    obj.parentProductName = row["prodservice_code"].ToString();
                    obj.parentProductGid = Convert.ToInt32(row["prodservice_gid"].ToString());
                    objproduct.Add(obj);
                }

                return objproduct;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public List<FbEntity> GetAssetCategory()
        {
            try
            {
                List<FbEntity> objAssetCategory = new List<FbEntity>();
                FbEntity obj;
                getconnection();
                objTable = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_prodservice", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "SelectAsset");
                da = new SqlDataAdapter(cmd);
                da.Fill(objTable);
                foreach (DataRow row in objTable.Rows)
                {
                    obj = new FbEntity();
                    obj.assetCategory = row["assetcategory_name"].ToString();
                    obj.assetGid = Convert.ToInt32(row["assetcategory_gid"].ToString());
                    objAssetCategory.Add(obj);
                }
                return objAssetCategory;
            }

            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public string GetAssetGl(int assetGid)
        {
            try
            {
                getconnection();
                FbEntity objDetails = new  FbEntity();
                cmd = new SqlCommand("pr_fb_mst_prodservice", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "Asset");
                cmd.Parameters.AddWithValue("@assetgid", assetGid);
                objDetails.glCode = Convert.ToString(cmd.ExecuteScalar());
                return objDetails.glCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public string GetExpenseGl(int expenseGid)
        {
            try
            {
                getconnection();
                FbEntity objDetails = new FbEntity();
                cmd = new SqlCommand("pr_fb_mst_prodservice", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "Expense");
                cmd.Parameters.AddWithValue("@expcatgid", expenseGid);
                objDetails.glCode = Convert.ToString(cmd.ExecuteScalar());
                return objDetails.glCode;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<FbEntity> GetExpenseCategory()
        {
            try
            {
                List<FbEntity> objExpCategory = new List<FbEntity>();
                FbEntity obj;
                getconnection();
                objTable = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_prodservice", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "SelectExpense");
                da = new SqlDataAdapter(cmd);
                da.Fill(objTable);
                foreach (DataRow row in objTable.Rows)
                {
                    obj = new FbEntity();
                    obj.expenseCategory = row["expcat_name"].ToString();
                    obj.expenseGid = Convert.ToInt32(row["expcat_gid"].ToString());
                    objExpCategory.Add(obj);
                }
                return objExpCategory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public DataSet GetCategory(int parentProductGid)
        {
            try
            {
                DataSet dt = new DataSet();
                getconnection();
                //string[,] parameter = new string[,]
                //{
                //    {"@prodservicegid",parentProductGid.ToString()},
                //    {"@actionName","selecttype"},
                //};

                //return objcommon.ProcedureCommon(parameter, "pr_fb_mst_prodservice");
                cmd = new SqlCommand("pr_fb_mst_prodservice", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prodservicegid", parentProductGid.ToString());
                cmd.Parameters.AddWithValue("@actionName", "selecttype");
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
                conn.Close();
            }
        }
        //Project Owner
        public List<ProjectOwner> GetEmployeeDetails()
        {
            try
            {
                getconnection();
                objTable = new DataTable();
                List<ProjectOwner> obj = new List<ProjectOwner>();
                ProjectOwner objproject;
                cmd = new SqlCommand("pr_fb_iem_mst_employee", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "Select");
                da = new SqlDataAdapter(cmd);
                da.Fill(objTable);
                for (int i = 0; i < objTable.Rows.Count; i++)
                {
                    objproject = new ProjectOwner();
                    objproject.slNo = i + 1;
                    objproject.employeeGid = Convert.ToInt32(objTable.Rows[i]["employee_gid"]);
                    objproject.empCode = objTable.Rows[i]["employee_code"].ToString();
                    objproject.empName = objTable.Rows[i]["employee_name"].ToString();
                    obj.Add(objproject);
                }
                    //foreach (DataRow row in objtable.Rows)
                    //{
                    //    objproject = new projectOwner();
                    //    objproject.employeeGid = Convert.ToInt32(row["employee_gid"]);
                    //    objproject.empCode = row["employee_code"].ToString();
                    //    objproject.empName = row["employee_name"].ToString();
                    //    obj.Add(objproject);
                    //}
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public string InsertProjOwnerDetails(ProjectOwner objowner, string empName, string empCode, string employeeGid)
        {
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_iem_mst_projectowner", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@projectownerempgid", objowner.employeeGid);
                cmd.Parameters.AddWithValue("@projectownername", objowner.empName);
                cmd.Parameters.AddWithValue("@actionName", "exist");
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 64);
                cmd.Parameters["@res"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res = Convert.ToString(cmd.Parameters["@res"].Value.ToString());
                if (res == "Not there")
                {
                    string[,] codes = new string[,]
	               {
                         
	                       {"projectowner_employeegid",objowner.employeeGid.ToString()},
                           {"projectowner_name",objowner.empName},
                           {"projectowner_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                           {"projectowner_insert_date",DateTime.Now.ToString("dd/MMM/yyyy")},
                   };

                    string tablename = "iem_mst_tprojectowner";
                    objowner.result = objcommon.InsertCommon(codes, tablename);
                }
                return objowner.result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

        }
        public List<ProjectOwner> GetProjectOwner()
        {
            try
            {
                getconnection();
                DataTable objtable = new DataTable();
                List<ProjectOwner> obj = new List<ProjectOwner>();
                ProjectOwner objProject;
                cmd = new SqlCommand("pr_fb_iem_mst_projectowner", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "select");
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                DataSet ds = new DataSet();
                objtable.TableName = "ProjectOwner";
                ds.Tables.Add(objtable);
                HttpContext.Current.Session["ExcelExportProject"] = ds;
                for (int i = 0; i < objtable.Rows.Count;i++ )
                {
                    objProject = new ProjectOwner();
                    objProject.slNo = i + 1;
                    objProject.projOwnerGid = Convert.ToInt32(objtable.Rows[i]["Projectowner Gid"]);
                    objProject.projOwner = objtable.Rows[i]["Projectowner Name"].ToString();
                    obj.Add(objProject);
                }
                    return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public string GetProjectOwnerById(int id)
        {
            try
            {
                getconnection();
                string[,] parameter = new string[,]
                {
                    {"@projectownerid",id.ToString()},
                    {"@actionName","selectbyid"},
                };
                return objcommon.ProcedureCommon(parameter, "pr_fb_iem_mst_projectowner");
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public string DeleteProjectOwnerDetails(ProjectOwner objOwner)
        {
            try
            {
                getconnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_prodservice", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prodservicegid", objOwner.projOwnerGid);
                cmd.Parameters.AddWithValue("@actionName", "ProjectOwner");
                //cmd.ExecuteNonQuery();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    objOwner.projOwnerGid = Convert.ToInt32(dt.Rows[0]["projectgid"]);
                }
                return objOwner.projOwner;
           //     string[,] wherecond = new string[,]
           // {
           //     {"projectowner_gid", objOwner.projOwnerGid.ToString ()}
           // };
           //     string[,] column = new string[,]
           //{
           //      {"projectowner_isremoved", "Y"}
	            
           //};
           //     string tblname = "iem_mst_tprojectowner";
           //     string deletetbl = objcommon.DeleteCommon(column, wherecond, tblname);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        //Budget Owner
        public List<BudgetOwner> GetEmpDetails()
        {
            try
            {
                getconnection();
                DataTable objtable = new DataTable();
                List<BudgetOwner> obj = new List<BudgetOwner>();
                BudgetOwner objbudget;
                cmd = new SqlCommand("pr_fb_iem_mst_employee", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "Select");
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                for (int i = 0; i < objtable.Rows.Count; i++)
                {
                    objbudget = new BudgetOwner();
                    objbudget.slNo = i + 1;
                    objbudget.employeeGid = Convert.ToInt32(objtable.Rows[i]["employee_gid"]);
                    objbudget.empCode = objtable.Rows[i]["employee_code"].ToString();
                    objbudget.empName =objtable.Rows[i]["employee_name"].ToString();
                    obj.Add(objbudget);
                }
                //foreach (DataRow row in objtable.Rows)
                //{
                //    objbudget = new budgetOwner();
                //    objbudget.employeeGid = Convert.ToInt32(row["employee_gid"]);
                //    objbudget.empCode = row["employee_code"].ToString();
                //    objbudget.empName = row["employee_name"].ToString();
                //    obj.Add(objbudget);
                //}
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public string InsertBudgOwnerDetails(BudgetOwner objowner, string empName, string empCode, string employeeGid)
        {
            string result;
            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_mst_budgetowner", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@budgetownerempgid", objowner.employeeGid);
                cmd.Parameters.AddWithValue("@budgetownername", objowner.empName);
                cmd.Parameters.AddWithValue("@actionName", "exist");
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 64);
                cmd.Parameters["@res"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res = Convert.ToString(cmd.Parameters["@res"].Value.ToString());
                if (res == "Not there")
                {
                    string[,] codes = new string[,]
	               {
                         
	                       {"budgetowner_employeegid",objowner.employeeGid.ToString()},
                           {"budgetowner_name",objowner.empName},
                           {"budgetowner_insert_by",Convert.ToString(objCmnFunctions.GetLoginUserGid())},
                           {"budgetowner_insert_date",DateTime.Now.ToString("dd/MMM/yyyy")},
                   };

                    string tablename = "fb_mst_tbudgetowner";
                    string insertcommend = objcommon.InsertCommon(codes, tablename);
                    result = "Record Inserted Successfully";
                }
                else
                {
                    result = "0";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
            return result;
        }
        public List<BudgetOwner> GetBudgetOwner()
        {
            try
            {
                getconnection();
                DataTable objtable = new DataTable();
                List<BudgetOwner> obj = new List<BudgetOwner>();
                BudgetOwner objbudget;
                cmd = new SqlCommand("pr_fb_mst_budgetowner", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "select");
                da = new SqlDataAdapter(cmd);
                da.Fill(objtable);
                DataSet ds = new DataSet();
                objtable.TableName = "BudgetOwner";
                ds.Tables.Add(objtable);
                HttpContext.Current.Session["ExcelExportBudget"] = ds;
                for (int i = 0; i < objtable.Rows.Count;i++ )
                {
                    objbudget = new BudgetOwner();
                    objbudget.slNo = i + 1;
                    objbudget.BudgetOwnergid = Convert.ToInt32(objtable.Rows[i]["Budgetowner Gid"]);
                    objbudget.BudgOwner = objtable.Rows[i]["Budgetowner Name"].ToString();
                    obj.Add(objbudget);
                }
                    
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public string GetBudgetOwnerById(int id)
        {
            try
            {
                getconnection();
                string[,] parameter = new string[,]
                {
                    {"@budgetownergid",id.ToString()},
                    {"@actionName","selectbyid"},
                };
                return objcommon.ProcedureCommon(parameter, "pr_fb_mst_budgetowner");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public void DeletebBudgetOwnerDetails(BudgetOwner objowner)
        {
            try
            {
                getconnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_prodservice", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prodservicegid", objowner.BudgetOwnergid);
                cmd.Parameters.AddWithValue("@actionName", "budgetowner");
                //cmd.ExecuteNonQuery();
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    objowner.BudgetOwnergid = Convert.ToInt32(dt.Rows[0]["projectgid"]);
                }
           //     string[,] wherecond = new string[,]
           // {
           //     {"budgetowner_gid", objowner.BudgetOwnergid.ToString ()}
           // };
           //     string[,] column = new string[,]
           //{
           //      {"budgetowner_isremoved", "Y"}
	            
           //};
           //     string tblname = "fb_mst_tbudgetowner";
           //     string deletetbl = objcommon.DeleteCommon(column, wherecond, tblname);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public int GetParentProductGid(int productGid)
        {
            try
            {
                getconnection();
                FbEntity obj = new FbEntity();
                cmd = new SqlCommand("pr_fb_mst_prodservice", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@prodservicegid", productGid);
                cmd.Parameters.AddWithValue("@actionName", "existparent");
                obj.parentProductGid = Convert.ToInt32(cmd.ExecuteScalar());
                return obj.parentProductGid;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public List<FbEntity> GetExpNatureList() 
        {
            try
            {
                List<FbEntity> objService;
                objService = new List<FbEntity>();
                FbEntity obj;
                getconnection();
                objTable = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_prodservice", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectExpCategory");
                da = new SqlDataAdapter(cmd);
                da.Fill(objTable);
                foreach (DataRow row in objTable.Rows)
                {
                    obj = new FbEntity();
                    obj.ExpNature = row["expcat_name"].ToString();
                    obj.ExpNatureGid = Convert.ToInt32(row["expcat_gid"].ToString());
                    objService.Add(obj);
                }
                return objService;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public List<FbEntity> GetAssetSubCatList(int assetgid = 0) 
        {
            try
            {
                List<FbEntity> objService;
                objService = new List<FbEntity>();
                FbEntity obj;
                getconnection();
                objTable = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_prodservice", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectassetsubcat");
                cmd.Parameters.AddWithValue("@assetgid", assetgid);
                da = new SqlDataAdapter(cmd);
                da.Fill(objTable);
                foreach (DataRow row in objTable.Rows)
                {
                    obj = new FbEntity();
                    obj.AssetSubCat = row["asset_description"].ToString();
                    obj.AssetSubCatGid = Convert.ToInt32(row["asset_gid"].ToString());
                    obj.glCode = row["assetcategory_glno"].ToString();
                    objService.Add(obj);
                }
                return objService;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }

        public List<FbEntity> GetExpenseCatList(int expnatgid = 0) 
        { 
            try
            {
                List<FbEntity> objService;
                objService = new List<FbEntity>();
                FbEntity obj;
                getconnection();
                objTable = new DataTable();
                cmd = new SqlCommand("pr_fb_mst_prodservice", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@actionName", "selectexpensecat");
                cmd.Parameters.AddWithValue("@expcatgid", expnatgid);
                da = new SqlDataAdapter(cmd);
                da.Fill(objTable);
                foreach (DataRow row in objTable.Rows)
                {
                    obj = new FbEntity();
                    obj.expenseCategory = row["expsubcat_name"].ToString();
                    obj.expenseGid = Convert.ToInt32(row["expsubcat_gid"].ToString());
                    obj.glCode = row["expcat_gl_no"].ToString();
                    objService.Add(obj);
                }
                return objService;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
        public string UpdateProdServNew(FbEntity objDetails) 
        {

            try
            {
                getconnection();
                cmd = new SqlCommand("pr_fb_mst_prodservice", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@actionName", "existforinsert");
                cmd.Parameters.AddWithValue("@actionName", "existforUpdate");
                cmd.Parameters.AddWithValue("@prodservicegid", objDetails.productGid);
                if (objDetails.productServflag == "Product")
                {
                    cmd.Parameters.AddWithValue("@prodserviceCode", objDetails.productCode);
                }
                if (objDetails.productServflag == "Service")
                {
                    cmd.Parameters.AddWithValue("@prodserviceCode", objDetails.service);
                }
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 64);
                cmd.Parameters["@res"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                string res = Convert.ToString(cmd.Parameters["@res"].Value.ToString());
                if (res == "Not there")
                {
                    cmd = new SqlCommand("pr_fb_mst_prodservice", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@actionName", "UpdateNew");
                    cmd.Parameters.AddWithValue("@prodserviceinsertby", objCmnFunctions.GetLoginUserGid());
                    cmd.Parameters.AddWithValue("@prodserviceinsertdate", DateTime.Now.ToString("dd/MMM/yyyy"));
                    cmd.Parameters.AddWithValue("@prodserviceglcode", string.IsNullOrEmpty(objDetails.glCode) ? "" : objDetails.glCode);
                    if (objDetails.category == "Asset")
                    {
                        cmd.Parameters.AddWithValue("@prodserviceassetcategory", objDetails.assetGid);
                        cmd.Parameters.AddWithValue("@prodserviceassetsubcat", objDetails.AssetSubCatGid);
                        cmd.Parameters.AddWithValue("@prodserviceCategory", "A");
                        if (objDetails.productServflag == "Product")
                        {
                            cmd.Parameters.AddWithValue("@prodserviceFlag", "P");
                            cmd.Parameters.AddWithValue("@prodserviceCode", objDetails.productCode);
                            cmd.Parameters.AddWithValue("@prodserviceName", objDetails.productName.ToString().ToUpper());
                            cmd.Parameters.AddWithValue("@prodservicedesc", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(objDetails.productDescription.ToString()));

                            if (objDetails.parentFlag == "Yes")
                            {

                                cmd.Parameters.AddWithValue("@prodserviceParent", "Y");
                            }
                            else
                            {

                                cmd.Parameters.AddWithValue("@prodserviceParent", "N");
                                cmd.Parameters.AddWithValue("@prodserviceparentgid", objDetails.parentProductGid);

                            }
                        }
                        if (objDetails.productServflag == "Service")
                        {
                            cmd.Parameters.AddWithValue("@prodserviceFlag", "S");
                            cmd.Parameters.AddWithValue("@prodserviceCode", objDetails.service);
                            cmd.Parameters.AddWithValue("@prodserviceName", objDetails.serviceName.ToString().ToUpper());
                            cmd.Parameters.AddWithValue("@prodservicedesc", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(objDetails.serviceDescription.ToString()));

                            if (objDetails.parentFlag == "Y")
                            {
                                cmd.Parameters.AddWithValue("@prodserviceParent", "Y");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@prodserviceParent", "N");
                                cmd.Parameters.AddWithValue("@prodserviceparentgid", objDetails.parentProductGid);
                            }
                        }

                    }
                    if (objDetails.category == "Expense")
                    {
                        cmd.Parameters.AddWithValue("@prodserviceexpcatcategory", objDetails.ExpNatureGid);
                        cmd.Parameters.AddWithValue("@prodserviceexpsubcat", objDetails.expenseGid);
                        cmd.Parameters.AddWithValue("@prodserviceCategory", "E");
                        if (objDetails.productServflag == "Product")
                        {
                            cmd.Parameters.AddWithValue("@prodserviceFlag", "P");
                            cmd.Parameters.AddWithValue("@prodserviceCode", objDetails.productCode);
                            cmd.Parameters.AddWithValue("@prodserviceName", objDetails.productName.ToString().ToUpper());
                            cmd.Parameters.AddWithValue("@prodservicedesc", CultureInfo.CurrentCulture.TextInfo.ToTitleCase(objDetails.productDescription.ToString()));
                            if (objDetails.parentFlag == "Yes")
                            {
                                cmd.Parameters.AddWithValue("@prodserviceParent", "Y");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("@prodserviceParent", "N");
                                cmd.Parameters.AddWithValue("@prodserviceparentgid", objDetails.parentProductGid);
                            }
                        }
                        if (objDetails.productServflag == "Service")
                        {

                            cmd.Parameters.AddWithValue("@prodserviceCode", objDetails.service);
                            cmd.Parameters.AddWithValue("@prodserviceName", objDetails.serviceName.ToString().ToUpper());
                            cmd.Parameters.AddWithValue("@prodserviceFlag", "S");
                            if (objDetails.parentFlag == "Yes")
                            {
                                cmd.Parameters.AddWithValue("@prodserviceParent", "Y");
                            }
                            cmd.Parameters.AddWithValue("@prodservicedesc", CultureInfo.CurrentCulture.TextInfo.ToTitleCase((objDetails.serviceDescription.ToString())));
                            if (objDetails.parentFlag == "No")
                            {
                                cmd.Parameters.AddWithValue("@prodserviceParent", "N");
                                cmd.Parameters.AddWithValue("@prodserviceparentgid", objDetails.parentProductGid);
                            }
                        }

                    }
                    cmd.Parameters.AddWithValue("@prodservgid", objDetails.productGid);
                    objDetails.result = Convert.ToString(cmd.ExecuteNonQuery());
                }
                return objDetails.result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}


