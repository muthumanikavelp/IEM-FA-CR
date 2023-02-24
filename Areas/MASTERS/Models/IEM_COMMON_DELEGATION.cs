using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using IEM.Areas.MASTERS.Models;
using System.Web.Mvc;
using System.Web.Helpers;
using IEM.Common;

namespace IEM.Areas.MASTERS.Models
{
    public class IEM_COMMON_DELEGATION : Iiem_common_delegation
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions cmnfun = new CmnFunctions();
        private void GetCon()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }

        public IEnumerable<iem_common_delegation> GetDelegationList()
        {
            try
            {
                List<iem_common_delegation> objOrgType = new List<iem_common_delegation>();
                iem_common_delegation objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_delegation", con);
                //cmd.Parameters.AddWithValue("@delegate_by", cmnfun.GetLoginUserGid().ToString());
                cmd.Parameters.AddWithValue("@action", "SelectList");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                DataSet ds = new DataSet();
                dt.TableName = "Delegation";
                ds.Tables.Add(dt);
                HttpContext.Current.Session["ExcelExportDelegation"] = ds;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_common_delegation();
                    objModel.delegate_gid = Convert.ToInt32(dt.Rows[i]["Delegate Gid"].ToString());
                    objModel.delegate_bygid = Convert.ToInt32(dt.Rows[i]["Delegate By Gid"].ToString());
                    objModel.delegate_togid = Convert.ToInt32(dt.Rows[i]["Delegate To Gid"].ToString());
                    objModel.delegate_period_from = Convert.ToString(dt.Rows[i]["Delegate Period From"].ToString());
                    objModel.delegate_period_to = Convert.ToString(dt.Rows[i]["delegate Period To"].ToString());
                    objModel.delegate_active = Convert.ToString(dt.Rows[i]["Delegate Active"].ToString());
                    objModel.delegate_remark = Convert.ToString(dt.Rows[i]["Delegate Remark"].ToString());
                    objModel.temp_delegate_delmattype = Convert.ToString(dt.Rows[i]["Delegate Delmattype Name"].ToString());                    
                    objModel.employee_gid = Convert.ToInt32(cmnfun.GetLoginUserGid().ToString());
                    objModel.delegate_byempcode = Convert.ToString(dt.Rows[i]["DelegateBy Code"].ToString());
                    objModel.delegate_byempname = Convert.ToString(dt.Rows[i]["DelegateBy Name"].ToString());
                    objModel.delegate_toempcode = Convert.ToString(dt.Rows[i]["DelegateTo Code"].ToString());
                    objModel.delegate_toempname = Convert.ToString(dt.Rows[i]["DelegateTo Name"].ToString());
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

        public IEnumerable<iem_common_delegation> GetDelegation(string delegateby, string delegateto, string periodfrom, string periodto, string delmatype)
        {
            try
            {
                List<iem_common_delegation> objOrgType = new List<iem_common_delegation>();
                iem_common_delegation objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_delegation", con);
                cmd.Parameters.AddWithValue("@action", "DSearch");
                if (periodfrom != "")
                {
                    cmd.Parameters.AddWithValue("@delegate_period_from", cmnfun.convertoDateTimeString(periodfrom));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@delegate_period_from", periodfrom);
                }
                if (periodto != "")
                {
                    cmd.Parameters.AddWithValue("@delegate_period_to", cmnfun.convertoDateTimeString(periodto));
                }
                else
                {
                    cmd.Parameters.AddWithValue("@delegate_period_to", periodto);
                }
                cmd.Parameters.AddWithValue("@delmattype_name", delmatype);
                cmd.Parameters.AddWithValue("@delegate_by", delegateby);
                cmd.Parameters.AddWithValue("@delegate_to1", delegateto);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_common_delegation();
                    objModel.delegate_gid = Convert.ToInt32(dt.Rows[i]["Delegate Gid"].ToString());
                    objModel.delegate_bygid = Convert.ToInt32(dt.Rows[i]["Delegate By Gid"].ToString());
                    objModel.delegate_togid = Convert.ToInt32(dt.Rows[i]["Delegate To Gid"].ToString());
                    objModel.delegate_period_from = Convert.ToString(dt.Rows[i]["Delegate Period From"].ToString());
                    objModel.delegate_period_to = Convert.ToString(dt.Rows[i]["delegate Period To"].ToString());
                    objModel.delegate_active = Convert.ToString(dt.Rows[i]["Delegate Active"].ToString());
                    objModel.delegate_remark = Convert.ToString(dt.Rows[i]["Delegate Remark"].ToString());
                    objModel.temp_delegate_delmattype = Convert.ToString(dt.Rows[i]["Delegate Delmattype Name"].ToString());
                    objModel.employee_gid = Convert.ToInt32(cmnfun.GetLoginUserGid().ToString());
                    objModel.delegate_byempcode = Convert.ToString(dt.Rows[i]["DelegateBy Code"].ToString());
                    objModel.delegate_byempname = Convert.ToString(dt.Rows[i]["DelegateBy Name"].ToString());
                    objModel.delegate_toempcode = Convert.ToString(dt.Rows[i]["DelegateTo Code"].ToString());
                    objModel.delegate_toempname = Convert.ToString(dt.Rows[i]["DelegateTo Name"].ToString());
                    objOrgType.Add(objModel);
                }

                return objOrgType;
            }
            catch (Exception ex)
            {
                //return View("Error", new HandleErrorInfo(ex, "EmployeeInfo", "Create"));
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }

        public iem_common_delegation GetDelegationById(int DelegationGid)
        {
            var objModel = new iem_common_delegation();
            try
            {
                List<iem_common_delegation> objOrgType = new List<iem_common_delegation>();
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_delegation", con);
                cmd.Parameters.AddWithValue("@action", "selectDelById");
                cmd.Parameters.AddWithValue("@gid", DelegationGid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objModel = new iem_common_delegation()
                {
                    delegate_gid = Convert.ToInt32(dt.Rows[0]["delegate_gid"].ToString()),
                    delegate_bygid = Convert.ToInt32(dt.Rows[0]["delegate_bygid"].ToString()),
                    delegate_byempcode = Convert.ToString(dt.Rows[0]["delegate_byempcode"].ToString()),
                    delegate_byempname = Convert.ToString(dt.Rows[0]["delegate_byempname"].ToString()),
                    delegate_togid = Convert.ToInt32(dt.Rows[0]["delegate_togid"].ToString()),
                    delegate_toempcode = Convert.ToString(dt.Rows[0]["delegate_toempcode"].ToString()),
                    delegate_toempname = Convert.ToString(dt.Rows[0]["delegate_toempname"].ToString()),
                    delegate_period_from = Convert.ToString(dt.Rows[0]["delegate_period_from"].ToString()),
                    delegate_period_to = Convert.ToString(dt.Rows[0]["delegate_period_to"].ToString()),
                    delegate_active = Convert.ToString(dt.Rows[0]["delegate_active"].ToString()),
                    delegate_remark = Convert.ToString(dt.Rows[0]["delegate_remark"].ToString()),
                    delmattype_gid = Convert.ToInt32(dt.Rows[0]["delmattype_gid"].ToString()),
                    delegate_department_gid = Convert.ToInt32(dt.Rows[0]["dept_gid"].ToString()),
                };
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
        }

        public string InsertCommonDelegationitem(iem_common_delegation DelegationModel)
        {
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_delegation", con);
                cmd.Parameters.AddWithValue("@action", "IFExist");
                cmd.Parameters.AddWithValue("@delegate_period_from", cmnfun.convertoDateTimeString(DelegationModel.delegate_period_from));
                cmd.Parameters.AddWithValue("@delegate_period_to", cmnfun.convertoDateTimeString(DelegationModel.delegate_period_to));                
                cmd.Parameters.AddWithValue("@delegate_delmattype", DelegationModel.delegate_delmattype);
                cmd.Parameters.AddWithValue("@delgate_deptid", DelegationModel.delegate_department_gid);
                cmd.Parameters.AddWithValue("@gid", DelegationModel.delegate_bygid);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["@res"].Value.ToString();

                if (result == "Not There")
                {
                    if (DelegationModel.delegate_remark == null)
                    {
                        DelegationModel.delegate_remark = "";
                    }
                    CommonIUD comm = new CommonIUD();
                    string[,] codes = new string[,]            
	                {
                       
                        {"delegate_by",Convert.ToString(DelegationModel.delegate_bygid)},
                        {"delegate_to",Convert.ToString(DelegationModel.delegate_togid)},
                        {"delegate_delmat_flag",Convert.ToString (DelegationModel.delegate_delmat_flag)},
                        {"delegate_supervisory_flag",Convert.ToString (DelegationModel.delegate_supervisory_flag)},
                        {"delegate_period_from",cmnfun.convertoDateTimeString(DelegationModel.delegate_period_from)},
                        {"delegate_period_to",cmnfun.convertoDateTimeString(DelegationModel.delegate_period_to)},
                        {"delegate_active",DelegationModel.delegate_active},                      
                        {"delegate_remark",DelegationModel.delegate_remark},
                        {"delegate_delmattype",Convert.ToString(DelegationModel.delegate_delmattype)},
                        {"delegate_deptid",Convert.ToString(DelegationModel.delegate_department_gid)},
                        {"delegate_insert_by",cmnfun.GetLoginUserGid().ToString()},
                        {"delegate_insert_date",comm.GetCurrentDate()},
                    };
                    string tname = "iem_mst_tdelegate";
                    string insertcommend = comm.InsertCommon(codes, tname);
                }
                else
                {
                    return "Duplicate record !";
                }
            }
            catch (Exception ex)
            {
                return ex.Message.ToString();
            }
            finally
            {
                con.Close();
            }

            return "success";
        }

        public string DeleteDelegation(int DelegationGid)
        {
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {

                string[,] codes = new string[,]
	       {
                 {"delegate_isremoved", "Y"}
	            
           };
                string[,] whrcol = new string[,]
	        {
                {"delegate_gid", DelegationGid.ToString ()}
            };
                string tblname = "iem_mst_tdelegate";


                string deletetbl = delecomm.DeleteCommon(codes, whrcol, tblname);
                return deletetbl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public string UpdateDelegation(iem_common_delegation UpdateDelegationmodel)
        {
            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_delegation", con);
                cmd.Parameters.AddWithValue("@action", "UpdateDelegation");
                cmd.Parameters.AddWithValue("@delegate_period_from", cmnfun.convertoDateTimeString(UpdateDelegationmodel.delegate_period_from));
                cmd.Parameters.AddWithValue("@delegate_period_to", cmnfun.convertoDateTimeString(UpdateDelegationmodel.delegate_period_to));
                cmd.Parameters.AddWithValue("@delegate_to", UpdateDelegationmodel.delegate_togid);
                cmd.Parameters.AddWithValue("@gid", UpdateDelegationmodel.delegate_gid);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["@res"].Value.ToString();
                if (UpdateDelegationmodel.delegate_remark == null)
                {
                    UpdateDelegationmodel.delegate_remark = "";
                }
                if (result == "Not There")
                {
                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes = new string[,]
	                  {
                        //{"delegate_by",Convert.ToString(UpdateDelegationmodel.delegate_bygid)},
                        {"delegate_to",Convert.ToString(UpdateDelegationmodel.delegate_togid)},
                        {"delegate_delmat_flag",Convert.ToString (UpdateDelegationmodel.delegate_delmat_flag)},
                        {"delegate_supervisory_flag",Convert.ToString (UpdateDelegationmodel.delegate_supervisory_flag)},
                        {"delegate_period_from",cmnfun.convertoDateTimeString(UpdateDelegationmodel.delegate_period_from)},
                        {"delegate_period_to",cmnfun.convertoDateTimeString(UpdateDelegationmodel.delegate_period_to)},
                        {"delegate_active",UpdateDelegationmodel.delegate_active},                      
                        {"delegate_remark",UpdateDelegationmodel.delegate_remark},
                        {"delegate_delmattype",Convert.ToString(UpdateDelegationmodel.delegate_delmattype)},
                        {"delegate_update_by",cmnfun.GetLoginUserGid().ToString()},
                        {"delegate_update_date",delecomm.GetCurrentDate()},
                        {"delegate_deptid",UpdateDelegationmodel.delegate_department_gid.ToString()},

	                  };
                    string[,] whrcol = new string[,]
	                 {
                          {"delegate_gid", UpdateDelegationmodel.delegate_gid.ToString ()},
                          {"delegate_isremoved", "N"}
                     };

                    string updcomm = delecomm.UpdateCommon(codes, whrcol, "iem_mst_tdelegate");
                }
                else
                {
                    return "Duplicate record !";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                con.Close();
            }

            return "success";
        }


        public IEnumerable<GetDelmattype> GetDelmattype()
        {
            try
            {
                List<GetDelmattype> objCountrytype = new List<GetDelmattype>();
                GetDelmattype objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_delegation", con);
                cmd.Parameters.AddWithValue("@action", "SelectDelmattype");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new GetDelmattype();
                    objModel.delmatypegid = Convert.ToInt32(dt.Rows[i]["delmattype_gid"].ToString());
                    objModel.delmattypename = Convert.ToString(dt.Rows[i]["delmattype_name"].ToString());
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
        }


        public IEnumerable<iem_common_delegation> EmployeeSearch()
        {
            try
            {
                List<iem_common_delegation> objOrgType = new List<iem_common_delegation>();
                iem_common_delegation objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_DelegationHiearchi", con);
                cmd.Parameters.AddWithValue("@action", "gethierarchy");
                cmd.Parameters.AddWithValue("@empgid", cmnfun.GetLoginUserGid().ToString());
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_common_delegation();
                    objModel.EmployeeId = Convert.ToInt32(dt.Rows[i]["empgid"].ToString());
                    objModel.RaiserCode = Convert.ToString(dt.Rows[i]["empcode"].ToString());
                    objModel.RaiserName = Convert.ToString(dt.Rows[i]["empname"].ToString());
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

        public IEnumerable<iem_common_delegation> EmployeeSearch(string EmpName, string Empcode)
        {
            throw new NotImplementedException();
        }



        public IEnumerable<GetDepartment> GetDepartmenttype()
        {
            try
            {
                
                List<GetDepartment> objCountrytype = new List<GetDepartment>();
                GetDepartment objModel;
                GetCon();

                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_delegation", con);
                cmd.Parameters.AddWithValue("@action", "SelectDept");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new GetDepartment();
                    objModel.dept_gid = Convert.ToInt32(dt.Rows[i]["dept_gid"].ToString());
                    objModel.dept_name = Convert.ToString(dt.Rows[i]["dept_name"].ToString());
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
        }

        public iem_common_delegation GetDelegationLoginDetails(int LoginGid)
        {
            var objModel = new iem_common_delegation();
            try
            {
                List<iem_common_delegation> objOrgType = new List<iem_common_delegation>();
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_delegation", con);
                cmd.Parameters.AddWithValue("@action", "SelectLogin");
                cmd.Parameters.AddWithValue("@gid", LoginGid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objModel = new iem_common_delegation()
                {
                    delegate_bygid = Convert.ToInt32(dt.Rows[0]["Emp_gid"].ToString()),
                    delegate_byempcode = Convert.ToString(dt.Rows[0]["Emp_code"].ToString()),
                    delegate_byempname = Convert.ToString(dt.Rows[0]["Emp_name"].ToString()),
                    delegate_department_gid = Convert.ToInt32(dt.Rows[0]["Emp_deptId"].ToString()),
                    delegate_isAdmin = Convert.ToString(dt.Rows[0]["Emp_IsAdmin"].ToString())
                    
                };
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
        }

    }
}