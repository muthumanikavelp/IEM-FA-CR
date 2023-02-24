﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using IEM.Areas.MASTERS.Models.IEM;
using System.Web.Mvc;
using System.Web.Helpers; 
using IEM.Common;


namespace IEM.Areas.MASTERS.Models
{
    public class GSTFiccBranchModel : GSTFIccBranch
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions com = new CmnFunctions();


      
        DataTable dt = new DataTable();
        CommonIUD comm = new CommonIUD();
        ErrorLog objErrorLog = new ErrorLog();
        Common.CmnFunctions cmnfun = new Common.CmnFunctions();
        string month;
        string result;
        string[,] codes;

        public GSTFiccBranchModel()
        {
            GetCon();
        }
         
        private void GetCon()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }
        public IEnumerable<EntityGSTFiccBranch> SelectBranch()
        {
            try
            {
                List<EntityGSTFiccBranch> emp = new List<EntityGSTFiccBranch>();
                EntityGSTFiccBranch objModel;
                cmd = new SqlCommand("pr_iem_mst_tbranch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTBRANCH";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new EntityGSTFiccBranch();
                    if (row["branch_gid"].ToString() != "" && row["branch_gid"].ToString() != null)
                    {
                        objModel.Branch_gid = Convert.ToInt16(row["branch_gid"].ToString());
                    }
                    objModel.BranchCode = row["branch_code"].ToString();
                    objModel.BranchName = row["branch_name"].ToString();
                    if (row["branch_city_gid"].ToString() != "" && row["branch_city_gid"].ToString() != null)
                    {
                        objModel.City_gid = Convert.ToInt16(row["branch_city_gid"].ToString());
                    }
                    objModel.City = row["branch_city_name"].ToString();
                    objModel.BranchTier = row["branch_tier"].ToString();
                    objModel.BranchIncharge = row["EMPLOYEENAME"].ToString();
                    objModel.StartDate = row["branch_start_date"].ToString();
                    objModel.ClosedDate = row["branch_end_date"].ToString();
                    objModel.ActiveStatus = row["branch_active"].ToString();
                    objModel.Branchfl = row["branch_flag"].ToString();
                    objModel.BranchAddress = row["branch_addr1"].ToString();
                    objModel.Location = row["LOCATIONNAME"].ToString();
                    objModel.OldBranchCode = row["branch_oldbranch_code"].ToString();
                    //if (row["branch_branchtype_gid"].ToString() != "" && row["branch_branchtype_gid"].ToString()!=null)
                    //{
                    //    objModel.Branchtype_gid = Convert.ToInt16(row["branch_branchtype_gid"].ToString());
                    //}
                    emp.Add(objModel);
                }
                return emp;
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

        public IEnumerable<EntityGSTFiccBranch> SlectBranchType()
        {
            try
            {
                List<EntityGSTFiccBranch> emp = new List<EntityGSTFiccBranch>();
                EntityGSTFiccBranch objModel;
                cmd = new SqlCommand("pr_iem_mst_tbranch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "BRANCHTYPECOMBOBOX";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new EntityGSTFiccBranch();
                    objModel.Branchtype_gid = Convert.ToInt32(row["branchtype_gid"].ToString());
                    objModel.BranchType = row["branchtype_name"].ToString();
                    emp.Add(objModel);
                }
                return emp;
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

        public IEnumerable<EntityGSTFiccBranch> SelectCity()
        {
            try
            {
                List<EntityGSTFiccBranch> emp = new List<EntityGSTFiccBranch>();
                EntityGSTFiccBranch objModel;
                cmd = new SqlCommand("pr_iem_mst_tbranch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "CITYCOMBOBOX";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new EntityGSTFiccBranch();
                    objModel.City_gid = Convert.ToInt32(row["city_gid"].ToString());
                    objModel.City = row["city_name"].ToString();
                    emp.Add(objModel);
                }
                return emp;
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
        public IEnumerable<EntityGSTFiccBranch> SelectEmployee()
        {
            try
            {
                List<EntityGSTFiccBranch> emp = new List<EntityGSTFiccBranch>();
                EntityGSTFiccBranch objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTEMPLOYEEDETAILSSEARCH";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new EntityGSTFiccBranch();
                    objModel.EmployeeId = Convert.ToInt32(row["employee_gid"].ToString());
                    objModel.EmployeeDepartment = row["employee_dept_name"].ToString();
                    objModel.RaiserName = row["employee_name"].ToString();
                    objModel.RaiserCode = row["employee_code"].ToString();
                    emp.Add(objModel);
                }
                return emp;
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

        public IEnumerable<EntityGSTFiccBranch> SelectEmployeeSearch(string EmployeeName, string EmployeeCode)
        {
            try
            {
                List<EntityGSTFiccBranch> emp = new List<EntityGSTFiccBranch>();
                EntityGSTFiccBranch objModel;
                cmd = new SqlCommand("pr_iem_trn_tcentralinward", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RAISERNAME", SqlDbType.VarChar).Value = EmployeeName;
                cmd.Parameters.Add("@RAISERCODE", SqlDbType.VarChar).Value = EmployeeCode;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTEMPLOYEEDETAILSSEARCH";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new EntityGSTFiccBranch();
                    objModel.EmployeeId = Convert.ToInt32(row["employee_gid"].ToString());
                    objModel.EmployeeDepartment = row["employee_dept_name"].ToString();
                    objModel.RaiserName = row["employee_name"].ToString();
                    objModel.RaiserCode = row["employee_code"].ToString();
                    emp.Add(objModel);
                }
                return emp;
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
        public string InsertBranch(EntityGSTFiccBranch bran)
        {
            try
            {
                GetCon();
                cmd = new SqlCommand("pr_iem_mst_tbranch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@BRANCH_CODE", SqlDbType.VarChar).Value = bran.BranchCode.ToString();

                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "IfCheckDuplicateAdd";
                result = (string)cmd.ExecuteScalar();
                if (result == "Duplicate record")
                {
                    result = "DuplicateBranchCode";
                    return result;
                }
                else
                {
                    if (bran.Branchfl == "Yes")
                    {
                        bran.Branchfl = "B";
                    }
                    else
                    {
                        bran.Branchfl = "N";
                    }
                    if (bran.ActiveStatus == "Yes")
                    {
                        bran.ActiveStatus = "Y";
                    }
                    else
                    {
                        bran.ActiveStatus = "N";
                    }
                    string[,] codes = new string[,]            
	                {   
                         {"branch_name",bran.BranchName.ToString()},
                         {"branch_branchtype_gid",bran.Branchtype_gid.ToString()},
                         {"branch_oldbranch_code",bran.OldBranchCode.ToString()},
                         {"branch_code",bran.BranchCode.ToString()},
                        // {"centralinward_supplier_name",bran.BranchTier.ToString()},
                         {"branch_incharge",bran.EmployeeId.ToString()},
                         {"branch_start_date",cmnfun.convertoDateTimeString(bran.StartDate.ToString())},    
                         {"branch_end_date",cmnfun.convertoDateTimeString(bran.ClosedDate.ToString())},
                         {"branch_city_gid",bran.City_gid.ToString()},
                         {"branch_city_name",bran.City.ToString()},
                         {"branch_location_name",bran.LocationName},
                         {"branch_location_gid",bran.Location_gid.ToString()},
                         {"branch_addr1",bran.BranchAddress.ToString()},
                         {"branch_flag",bran.Branchfl},
                         {"branch_active",bran.ActiveStatus.ToString()},
                         {"branch_insert_date","sysdatetime()"},
                         {"branch_insert_by",Convert.ToString (cmnfun.GetLoginUserGid())},
                         {"branch_branchtier",bran.BranchTier.ToString()}
                         
                         
                    };
                    string tname = "iem_mst_tbranch";
                    result = comm.InsertCommon(codes, tname);
                    if (result == "Success")
                    {
                        result = "suc";
                    }

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
        //public string EditBranch(EntityGSTFiccBranch bran)
        //{

        //    throw new NotImplementedException();
        //}


        public IEnumerable<EntityGSTFiccBranch> SelectBranchEdit(int id)
        {
            try
            {
                List<EntityGSTFiccBranch> emp = new List<EntityGSTFiccBranch>();
                EntityGSTFiccBranch objModel;
                cmd = new SqlCommand("pr_iem_mst_tbranch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@branch_gid", SqlDbType.VarChar).Value = id;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTBRANCHFOREDIT";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new EntityGSTFiccBranch();
                    if (row["branch_gid"].ToString() != "" && row["branch_gid"].ToString() != null)
                    {
                        objModel.Branch_gid = Convert.ToInt32(row["branch_gid"].ToString());
                    }
                    objModel.BranchCode = row["branch_code"].ToString();
                    objModel.BranchName = row["branch_name"].ToString();
                    if (row["branch_city_gid"].ToString() != "" && row["branch_city_gid"].ToString() != null)
                    {
                        objModel.City_gid = Convert.ToInt32(row["branch_city_gid"].ToString());
                    }
                    objModel.City = row["CITY"].ToString();
                    objModel.City = row["branch_city_name"].ToString();
                    objModel.BranchTier = row["branch_tier"].ToString();
                    objModel.BranchIncharge = row["EMPLOYEENAME"].ToString();
                    objModel.StartDate = row["branch_start_date"].ToString();
                    objModel.ClosedDate = row["branch_end_date"].ToString();
                    if (row["branch_active"].ToString() == "Y")
                    {
                        objModel.ActiveStatus = "Yes";
                    }
                    if (row["branch_active"].ToString() == "N")
                    {
                        objModel.ActiveStatus = "No";
                    }
                    if (row["branch_flag"].ToString() == "B")
                    {
                        objModel.Branchfl = "Branch";
                    }
                    if (row["branch_flag"].ToString() == "N")
                    {
                        objModel.Branchfl = "Non Branch";
                    }
                    if (row["branch_incharge"].ToString() != "" && row["branch_incharge"].ToString() != null)
                    {
                        objModel.EmployeeId = Convert.ToInt32(row["branch_incharge"].ToString());
                    }
                    objModel.BranchAddress = row["branch_addr1"].ToString();
                    objModel.Location = row["LOCATIONNAME"].ToString();
                    objModel.OldBranchCode = row["branch_oldbranch_code"].ToString();
                    //if (row["branch_branchtype_gid"].ToString() != "" && row["branch_branchtype_gid"].ToString()!=null)
                    //{
                    //    objModel.Branchtype_gid = Convert.ToInt16(row["branch_branchtype_gid"].ToString());
                    //}
                    objModel.StartLeaseDate = row["branch_lease_start_date"].ToString();
                    objModel.EndLeaseDate = row["branch_lease_end_date"].ToString();
                    objModel.PinCode = row["branch_pincode"].ToString();

                    objModel.BranchType = row["branch_branchtype_name"].ToString();
                    objModel.pincode_gid = Convert.ToInt32(string.IsNullOrEmpty(row["pincode_gid"].ToString()) ? "0" : row["pincode_gid"].ToString());
                    objModel.pincode_code = row["pincode_code"].ToString();
                    objModel.district_gid = Convert.ToInt32(row["branch_district_gid"]);
                    objModel.state_gid = Convert.ToInt32(row["branch_state_gid"]);
                    objModel.branch_gstin = string.IsNullOrEmpty(row["branch_gstin"].ToString()) ? " " : row["branch_gstin"].ToString();
                    
                    emp.Add(objModel);
                }
                return emp;
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
        public IEnumerable<EntityGSTFiccBranch> SelectLoction()
        {
            try
            {
                List<EntityGSTFiccBranch> emp = new List<EntityGSTFiccBranch>();
                EntityGSTFiccBranch objModel;
                cmd = new SqlCommand("pr_iem_mst_tbranch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "CMBLOCATION";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new EntityGSTFiccBranch();
                    objModel.Location_gid = Convert.ToInt32(row["location_gid"].ToString());
                    objModel.Location = row["location_name"].ToString();
                    emp.Add(objModel);
                }
                return emp;
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
        public string UpdateBranch(EntityGSTFiccBranch bran)
        {
            try
            {
                GetCon();

                if (bran.LocationName == null)
                {
                    bran.LocationName = "";
                }

                if (bran.StartLeaseDate == null)
                {
                    bran.StartLeaseDate = "";
                }
                if (bran.EndLeaseDate == null)
                {
                    bran.EndLeaseDate = "";

                }
                if (bran.StartDate == null)
                {
                    bran.StartDate = "";
                }
                if (bran.ClosedDate == null)
                {
                    bran.ClosedDate = "";

                }
                if (bran.PinCode == null)
                {
                    bran.PinCode = "";
                }
                if (bran.City == null)
                {
                    bran.City = "";
                }
                if (bran.branch_gstin == null)
                {
                    bran.branch_gstin = "";
                }
                if (bran.dictrictcode == null)
                {
                    bran.dictrictcode = "";
                }
                if (bran.statecode == null)
                {
                    bran.statecode = "";
                }
                if (bran.BranchAddress == null)
                {
                    bran.BranchAddress = "";
                }

                codes = new string[,]            
	                {   
                        
                         

                          {"branch_city_gid", string.IsNullOrEmpty( bran.City_gid.ToString())?"":  bran.City_gid.ToString() },
                         {"branch_city_name",bran.City},
                         {"branch_addr1",bran.BranchAddress.ToString()},
                         {"branch_location_name",bran.LocationName},
                         {"branch_location_gid",bran.Location_gid.ToString()},
                         {"branch_update_date","sysdatetime()"},
                         {"branch_update_by",Convert.ToString (cmnfun.GetLoginUserGid())},
                         {"branch_lease_start_date",string.IsNullOrEmpty(bran.StartLeaseDate.ToString())?"": cmnfun.convertoDateTimeString(bran.StartLeaseDate.ToString())},
                         {"branch_lease_end_date", string.IsNullOrEmpty(bran.EndLeaseDate.ToString())?"": cmnfun.convertoDateTimeString(bran.EndLeaseDate.ToString())},
                         {"branch_pincode",bran.PinCode.ToString()},
                         {"branch_start_date",string.IsNullOrEmpty(bran.StartDate.ToString())?"": cmnfun.convertoDateTimeString(bran.StartDate.ToString())},
                         {"branch_end_date",string.IsNullOrEmpty(bran.ClosedDate.ToString())?"": cmnfun.convertoDateTimeString(bran.ClosedDate.ToString())},                         
                         {"branch_gstin",bran.branch_gstin.ToString()},
                         {"branch_district_gid",bran.district_gid.ToString()},
                         {"branch_district_name",bran.dictrictcode},
                         {"branch_state_gid",bran.state_gid.ToString()},
                         {"branch_state_name",bran.statecode}

                         
                    };

                string[,] whrcol = new string[,]
	                 {
                          {"branch_gid", bran.Branch_gid.ToString()},
                          {"branch_isremoved", "N"}
                     };
                string tname = "iem_mst_tbranch";
                result = comm.UpdateCommon(codes, whrcol, tname);
                if (result == "Success")
                {
                    result = "suc";
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
        public string DeleteBranch(int id)
        {
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {

                string[,] codes = new string[,]
	       {
                 {"branch_isremoved", "Y"}
	            
           };
                string[,] whrcol = new string[,]
	        {
                {"branch_gid",id.ToString ()}
            };
                string tblname = "iem_mst_tbranch";
                result = delecomm.DeleteCommon(codes, whrcol, tblname);
                if (result == "Sucess")
                {
                    result = "del";
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
            return result;
        }
        public IEnumerable<EntityGSTFiccBranch> Search(string BranchCode = null, string BranchName = null, string BranchType = null, string City = null, string Branch = null, string ActiveStatus = null)
        {
            try
            {
                if (Branch == "Yes")
                {
                    Branch = "B";
                }
                if (Branch == "No")
                {
                    Branch = "N";
                }
                if (ActiveStatus == "Yes")
                {
                    ActiveStatus = "Y";
                }
                if (ActiveStatus == "No")
                {
                    ActiveStatus = "N";
                }
                List<EntityGSTFiccBranch> emp = new List<EntityGSTFiccBranch>();
                EntityGSTFiccBranch objModel;
                cmd = new SqlCommand("pr_iem_mst_tbranch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@BRANCH_CODE", SqlDbType.VarChar).Value = BranchCode;
                cmd.Parameters.Add("@BRANCH_NAME", SqlDbType.VarChar).Value = BranchName;
                cmd.Parameters.Add("@BRANCH", SqlDbType.VarChar).Value = Branch;
                cmd.Parameters.Add("@BRANCH_ACTIVE", SqlDbType.VarChar).Value = ActiveStatus;
                if (BranchType != "" && BranchType != null)
                {
                    cmd.Parameters.Add("@branchtype_gid", SqlDbType.VarChar).Value = Convert.ToInt16(BranchType);
                }
                if (City != "" && City != null)
                {
                    cmd.Parameters.Add("@CITY_GID", SqlDbType.VarChar).Value = Convert.ToInt16(City);
                }
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SEARCH";
                da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new EntityGSTFiccBranch();
                    if (row["branch_gid"].ToString() != "" && row["branch_gid"].ToString() != null)
                    {
                        objModel.Branch_gid = Convert.ToInt16(row["branch_gid"].ToString());
                    }
                    objModel.BranchCode = row["branch_code"].ToString();
                    objModel.BranchName = row["branch_name"].ToString();
                    if (row["branch_city_gid"].ToString() != "" && row["branch_city_gid"].ToString() != null)
                    {
                        objModel.City_gid = Convert.ToInt16(row["branch_city_gid"].ToString());
                    }
                    objModel.City = row["branch_city_name"].ToString();
                    objModel.BranchTier = row["branch_tier"].ToString();
                    objModel.BranchIncharge = row["EMPLOYEENAME"].ToString();
                    objModel.StartDate = row["branch_start_date"].ToString();
                    objModel.ClosedDate = row["branch_end_date"].ToString();
                    objModel.ActiveStatus = row["branch_active"].ToString();
                    objModel.Branchfl = row["branch_flag"].ToString();
                    objModel.BranchAddress = row["branch_addr1"].ToString();
                    objModel.Location = row["LOCATIONNAME"].ToString();
                    objModel.OldBranchCode = row["branch_oldbranch_code"].ToString();
                    //if (row["branch_branchtype_gid"].ToString() != "" && row["branch_branchtype_gid"].ToString() != null)
                    //{
                    //    objModel.Branchtype_gid = Convert.ToInt16(row["branch_branchtype_gid"].ToString());
                    //}
                    emp.Add(objModel);
                }
                return emp;
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
        public DataTable BranchLoad()
        {
            dt = new DataTable();
            try
            {
                cmd = new SqlCommand("pr_iem_mst_tbranch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "BRANCHLOAD";
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

        public IEnumerable<EntityGSTFiccBranch> getPincode()
        {
            try
            {
                List<EntityGSTFiccBranch> objCountrytype = new List<EntityGSTFiccBranch>();
                EntityGSTFiccBranch objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("GST_MST_TMP_SUPPLIER_SL", con);
                cmd.Parameters.AddWithValue("@StatementType", "SPIN");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new EntityGSTFiccBranch();
                    objModel.pincode_gid = Convert.ToInt32(dt.Rows[i]["pincode_gid"].ToString());
                  //  objModel.pincode_list = Convert.ToString(dt.Rows[i]["pincode_gid"].ToString()); 
                    objModel.pincode_code = Convert.ToString(dt.Rows[i]["pincode_code"].ToString()); 
                    
                    objCountrytype.Add(objModel);
                }
                return objCountrytype;
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



        public IEnumerable<EntityGSTFiccBranch> getstate()
        {
            try
            {
                List<EntityGSTFiccBranch> objCountrytype = new List<EntityGSTFiccBranch>();
                EntityGSTFiccBranch objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("GST_MST_TMP_SUPPLIER_SL", con);
                cmd.Parameters.AddWithValue("@StatementType", "getstate");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new EntityGSTFiccBranch();
                    objModel.state_gid = Convert.ToInt32(dt.Rows[i]["state_gid"].ToString());
                    //  objModel.pincode_list = Convert.ToString(dt.Rows[i]["pincode_gid"].ToString()); 
                    objModel.statecode = Convert.ToString(dt.Rows[i]["state_code"].ToString());

                    objCountrytype.Add(objModel);
                }
                return objCountrytype;
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



        public IEnumerable<EntityGSTFiccBranch> getdistrict()
        {
            try
            {
                List<EntityGSTFiccBranch> objCountrytype = new List<EntityGSTFiccBranch>();
                EntityGSTFiccBranch objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("GST_MST_TMP_SUPPLIER_SL", con);
                cmd.Parameters.AddWithValue("@StatementType", "getdistrict");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new EntityGSTFiccBranch();
                    objModel.district_gid = Convert.ToInt32(dt.Rows[i]["district_gid"].ToString());
                    //  objModel.pincode_list = Convert.ToString(dt.Rows[i]["pincode_gid"].ToString()); 
                    objModel.dictrictcode = Convert.ToString(dt.Rows[i]["district_code"].ToString());

                    objCountrytype.Add(objModel);
                }
                return objCountrytype;
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


        public IEnumerable<EntityGSTFiccBranch> GetDistrictByStateId(string stateid)
        {
            

            //try
            //{
                List<EntityGSTFiccBranch> objOrgType = new List<EntityGSTFiccBranch>();
            //    EntityGSTPincode objModel;
            //    GetCon();
            //    DataTable dt = new DataTable();
            //    SqlCommand cmd = new SqlCommand("GST_MST_DISTRICT_SL", con);
            //    cmd.Parameters.AddWithValue("@StatementType", "STATEID");
            //    cmd.Parameters.AddWithValue("@district_state_gid", stateid);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    SqlDataAdapter da = new SqlDataAdapter(cmd);
            //    da = new SqlDataAdapter(cmd);
            //    da.Fill(dt);
            //    for (int i = 0; i < dt.Rows.Count; i++)
            //    {
            //        objModel = new EntityGSTPincode();
            //        objModel.Pincode_district_gid = Convert.ToInt32(dt.Rows[i]["district_gid"]);
            //        objModel.Pincode_district_name = dt.Rows[i]["district_name"].ToString();
            //        objOrgType.Add(objModel);
            //    }

            //    return objOrgType;
            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    con.Close();
            //}
            return objOrgType;
        }

    }
}