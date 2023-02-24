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
    public class IEM_MST_EXPENSECATEGORY : Iiem_mst_expensecategory
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions cmnfunction = new CmnFunctions();
        DataTable dt = new DataTable();
        private void GetCon()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }

        public IEnumerable<iem_mst_expensecategory> getexpcategory()
        {
            try
            {
                List<iem_mst_expensecategory> objOrgType = new List<iem_mst_expensecategory>();
                iem_mst_expensecategory objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_expcat", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_expensecategory();
                    objModel.expcat_gid = Convert.ToInt32(dt.Rows[i]["expcat_gid"].ToString());
                    objModel.expcat_code = Convert.ToString(dt.Rows[i]["expcat_code"].ToString());
                    objModel.expcat_name = Convert.ToString(dt.Rows[i]["expcat_name"].ToString());
                    objModel.expnature_name = Convert.ToString(dt.Rows[i]["expnature_name"].ToString());
                    objModel.gl_no = Convert.ToString(dt.Rows[i]["expcat_gl_no"].ToString());
                    objModel.expcat_active = Convert.ToString(dt.Rows[i]["expcat_active"]);
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

        public IEnumerable<iem_mst_expensecategory> getexpcategory(string expcat_code, string expcat_name, string naturegid)
        {
            try
            {
                List<iem_mst_expensecategory> objOrgType = new List<iem_mst_expensecategory>();
                iem_mst_expensecategory objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_expcat", con);
                if (naturegid != "0")
                {
                    cmd.Parameters.AddWithValue("@action", "selectwithnature");
                    cmd.Parameters.AddWithValue("@expcat_gid", naturegid);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@action", "select");
                }
                cmd.Parameters.AddWithValue("@expcode_code", expcat_code);
                cmd.Parameters.AddWithValue("@expname_name", expcat_name);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_expensecategory();
                    objModel.expcat_gid = Convert.ToInt32(dt.Rows[i]["expcat_gid"].ToString());
                    objModel.expcat_code = Convert.ToString(dt.Rows[i]["expcat_code"].ToString());
                    objModel.expcat_name = Convert.ToString(dt.Rows[i]["expcat_name"].ToString());
                    objModel.expnature_name = Convert.ToString(dt.Rows[i]["expnature_name"].ToString());
                    objModel.gl_no = Convert.ToString(dt.Rows[i]["expcat_gl_no"].ToString());
                    objModel.expcat_active = Convert.ToString(dt.Rows[i]["expcat_active"]);
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
        public IEnumerable<iem_mst_expensecategory> SelectGLCode()
        {
            try
            {
                List<iem_mst_expensecategory> emp = new List<iem_mst_expensecategory>();
                iem_mst_expensecategory objModel;
                GetCon();
                cmd = new SqlCommand("pr_iem_mst_gl", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new iem_mst_expensecategory();
                    objModel.expcat_code = row["gl_no"].ToString();
                    objModel.expcat_name = row["gl_name"].ToString();
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
        public IEnumerable<iem_mst_expensecategory> SelectGLSearch(string EmployeeName, string EmployeeCode)
        {
            try
            {
                List<iem_mst_expensecategory> emp = new List<iem_mst_expensecategory>();
                iem_mst_expensecategory objModel;
                GetCon();
                cmd = new SqlCommand("pr_iem_mst_gl", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@expname_name", SqlDbType.VarChar).Value = EmployeeName;
                cmd.Parameters.Add("@expcode_code", SqlDbType.VarChar).Value = EmployeeCode;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "GLsearch";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new iem_mst_expensecategory();
                    objModel.expcat_code = row["gl_no"].ToString();
                    objModel.expcat_name = row["gl_name"].ToString();
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
        public iem_mst_expensecategory GetexpcategoryById(int ExpId)
        {
            try
            {
                List<iem_mst_expensecategory> objOrgType = new List<iem_mst_expensecategory>();
                iem_mst_expensecategory objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_expcat", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@expcat_gid", ExpId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                objModel = new iem_mst_expensecategory()
                {
                    expcat_gid = Convert.ToInt32(dt.Rows[0]["expcat_gid"].ToString()),
                    expcat_code = Convert.ToString(dt.Rows[0]["expcat_code"].ToString()),
                    expcat_name = Convert.ToString(dt.Rows[0]["expcat_name"].ToString()),
                    selectedexpnature_gid = Convert.ToInt32(dt.Rows[0]["expnature_gid"].ToString()),
                    //selectedgl_no = Convert.ToInt32(dt.Rows[0]["expcat_gl_no"].ToString()),
                    selectedgl_no = dt.Rows[0]["expcat_gl_no"].ToString(),
                    expnature_name = Convert.ToString(dt.Rows[0]["expnature_name"].ToString()),
                    gl_no = Convert.ToString(dt.Rows[0]["expcat_gl_no"].ToString()),
                    expcat_active = Convert.ToString(dt.Rows[0]["expcat_active"]),
                    expnature_gid = Convert.ToInt32(dt.Rows[0]["expnature_gid"].ToString()),

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
            // throw new NotImplementedException();
        }

        public string Insertexpcategory(iem_mst_expensecategory Expcate)
        {
            string result;

            try
            {
                CommonIUD comm = new CommonIUD();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_expcat", con);
                GetCon();
                cmd.Parameters.AddWithValue("@action", "InsExist");
                cmd.Parameters.AddWithValue("@expcode_code", Expcate.expcat_code);
                cmd.Parameters.AddWithValue("@expname_name", Expcate.expcat_name);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                string valid = (string)cmd.ExecuteScalar();
                if (valid == "Not There")
                {
                    string[,] codes = new string[,]
	                {
                        {"expcat_code",Expcate.expcat_code},
                        {"expcat_name",Expcate.expcat_name.ToString() },
	                    {"expcat_expnature_gid", Expcate.expnature_gid.ToString ()},
	                    {"expcat_gl_no", Expcate.gl_no.ToString ()},
                        {"expcat_active", Expcate.expcat_active.ToString ()},
                        {"expcat_insert_by", Expcate.expcat_insert_by.ToString ()},
                        {"expcat_insert_date", comm.GetCurrentDate()}                        
                    };
                    string insertcommend = comm.InsertCommon(codes, "iem_mst_texpcat");
                    result = "success";
                }
                else
                {
                    result = "Duplicate record !";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            finally
            {
                con.Close();
            }

            return result;
        }

        public string Deletexpcategory(int ExpCatId)
        {
            string result;
            try
            {

                GetCon();
                DataTable getserilno = new DataTable();
                SqlCommand cmd = new SqlCommand("select * from iem_mst_texpsubcat where expsubcat_expcat_gid=" + ExpCatId + " and expsubcat_isremoved='N' ", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(getserilno);
                if (getserilno.Rows.Count == 0)
                {
                    CommonIUD delecomm = new CommonIUD();
                    string col_value = string.Empty;
                    string col_temp = string.Empty;

                    string[,] codes = new string[,]
	            {
                    {"expcat_isremoved", "Y"}
                };
                    string[,] whrcol = new string[,]
	            {
                    {"expcat_gid", ExpCatId.ToString ()}
                };

                    string deletetbl = delecomm.DeleteCommon(codes, whrcol, "iem_mst_texpcat");
                    result = "success";

                }
                else
                {
                    result = "Can Not Delete this, Its Based On Some Other category";
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

        public string Updatexpcategory(iem_mst_expensecategory ExpCat)
        {
            string result;

            try
            {
                CommonIUD comm = new CommonIUD();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_expcat", con);
                GetCon();
                cmd.Parameters.AddWithValue("@expcat_gid", ExpCat.expcat_gid);
                cmd.Parameters.AddWithValue("@expcode_code", ExpCat.expcat_code);
                cmd.Parameters.AddWithValue("@expname_name", ExpCat.expcat_name);
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.CommandType = CommandType.StoredProcedure;
                string valid = (string)cmd.ExecuteScalar();
                if (valid == "Not There")
                {
                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes = new string[,]
	                {
                        {" expcat_code",ExpCat.expcat_code},
                        {"expcat_name",ExpCat.expcat_name.ToString() },
	                    {"expcat_expnature_gid", ExpCat.expcat_expnature_gid .ToString ()},
	                    {"expcat_gl_no", ExpCat.gl_no .ToString ()},
                       // {"expcat_mode", ExpCat.expcat_mode.ToString ()},
                        {"expcat_active", ExpCat.expcat_active.ToString ()},
                        {"expcat_update_by",cmnfunction.GetLoginUserGid().ToString ()},
                        {"expcat_update_date", "sysdatetime()"}                        
                    };
                    string[,] whrcol = new string[,]
	                {
                        {" expcat_gid", ExpCat.expcat_gid.ToString ()},                        
                     };

                    string updcomm = delecomm.UpdateCommon(codes, whrcol, "iem_mst_texpcat");
                    result = "success";
                }
                else
                {
                    result = "Duplicate record !";
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            finally
            {
                con.Close();
            }

            return result;
        }


        public IEnumerable<Getexpnature> Getexpnature()
        {
            try
            {
                List<Getexpnature> objExpNature = new List<Getexpnature>();
                Getexpnature objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_expnature", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new Getexpnature();
                    objModel.expnature_gid = Convert.ToInt32(dt.Rows[i]["expnature_gid"].ToString());
                    objModel.expnature_name = Convert.ToString(dt.Rows[i]["expnature_name"].ToString());

                    objExpNature.Add(objModel);
                }

                return objExpNature;
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

        public GetexpnatureById GetexpnatureById(int expnature_gid)
        {
            try
            {
                List<GetexpnatureById> objOrgType = new List<GetexpnatureById>();
                GetexpnatureById objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_expnature", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", expnature_gid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    objModel = new GetexpnatureById()
                    {
                        expnature_gid = Convert.ToInt32(dt.Rows[0]["expnature_gid"].ToString()),
                        expnature_name = Convert.ToString(dt.Rows[0]["expnature_name"].ToString()),
                    };
                }
                else
                {
                    objModel = new GetexpnatureById()
                    {
                        expnature_gid = 0,
                        expnature_name = "",
                    };
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
            //  throw new NotImplementedException();
        }

        public IEnumerable<GetGL> GetGL()
        {
            try
            {
                List<GetGL> objGl = new List<GetGL>();
                GetGL objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_gl", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    objModel = new GetGL();
                //    objModel.gl_gid = Convert.ToInt32(dt.Rows[i]["gl_gid"].ToString());
                //    objModel.gl_no = Convert.ToString(dt.Rows[i]["gl_no"].ToString());
                //    objGl.Add(objModel);
                //}
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new GetGL();
                    objModel.gl_gid = Convert.ToInt32(row["gl_gid"].ToString());
                    objModel.gl_no = row["gl_no"].ToString();
                    objGl.Add(objModel);
                }


                return objGl;
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

        public GetGLById GetGLById(int gl_no)
        {
            try
            {
                GetGLById objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_gl", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", gl_no);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count == 1)
                {
                    objModel = new GetGLById()
                    {
                        gl_gid = Convert.ToInt32(dt.Rows[0]["gl_gid"].ToString()),
                        gl_no = Convert.ToString(dt.Rows[0]["gl_no"].ToString()),
                    };
                }
                else
                {
                    objModel = new GetGLById()
                    {
                        gl_gid = 0,
                        gl_no = "",
                    };
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
        }
    }
}