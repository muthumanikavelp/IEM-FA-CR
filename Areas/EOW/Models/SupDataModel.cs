using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IEM.Common;

namespace IEM.Areas.EOW.Models
{
    public class SupDataModel : IRepository
    {
        ErrorLog objErrorLog = new ErrorLog();
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        DataTable dt = new DataTable();
        Common.CmnFunctions cmn = new Common.CmnFunctions();
        SupClassificationModel sub = new SupClassificationModel();
        string result;
        public SupDataModel()
        {
            GetConnection();
        }
        private void GetConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }

        public IEnumerable<SupClassificationModel> GetClassification()
        {
            List<SupClassificationModel> objClassification = new List<SupClassificationModel>();
            try
            {

                SupClassificationModel objModel;
                GetConnection();

                cmd = new SqlCommand("asms_mst_supplierclassification", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new SupClassificationModel();
                    objModel._ClassificationID = Convert.ToInt32(dt.Rows[i]["supplierclassification_gid"].ToString());
                    objModel._ClassificationName = Convert.ToString(dt.Rows[i]["supplierclassification_name"].ToString());
                    objClassification.Add(objModel);
                }

                return objClassification;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return objClassification;
            }
            finally
            {
                con.Close();
            }
        }

        public SupClassificationModel GetClassificationById(int ClassificationId)
        {
            SupClassificationModel vv = new SupClassificationModel();
            try
            {
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("asms_mst_supplierclassification", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = ClassificationId;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "select";
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new SupClassificationModel()
                {
                    _ClassificationID = Convert.ToInt32(dt.Rows[0]["supplierclassification_gid"].ToString()),
                    _ClassificationName = Convert.ToString(dt.Rows[0]["supplierclassification_name"].ToString()),
                };
                return model;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return vv;
            }
            finally
            {

            }
        }

        public void InsertClassification(SupClassificationModel Classifications)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("asms_mst_supplierclassification", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = Classifications._ClassificationName;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "insert";
                cmd.ExecuteNonQuery();
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

        public void DeleteClassification(int ClassificationId)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("asms_mst_supplierclassification", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = ClassificationId;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = "";
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

        public void UpdateClassification(SupClassificationModel Classifications)
        {
            try
            {
                GetConnection();
                cmd = new SqlCommand("asms_mst_supplierclassification", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = Classifications._ClassificationID;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = Classifications._ClassificationName;
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


        public string ClassificationIsExists(string classification)
        {
            try
            {
                string Result = "false";
                GetConnection();
                DataTable dt = new DataTable();
                cmd = new SqlCommand("asms_mst_supplierclassification", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@name", SqlDbType.VarChar).Value = classification;
                cmd.Parameters.Add("@action", SqlDbType.VarChar).Value = "isexists";
                Result = (string)cmd.ExecuteScalar();
                if (string.IsNullOrEmpty(Result) || string.IsNullOrWhiteSpace(Result))
                {
                    Result = "true";
                }
                else
                {
                    Result = "false";
                }
                return Result;
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

        public void Registration(SupClassificationModel sub)
        {
            try
            {
                con.Open();
                cmd = new SqlCommand("CREATEUSER", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@USERNAME", SqlDbType.VarChar).Value = sub.Username;
                cmd.Parameters.Add("@PASSWORD", SqlDbType.VarChar).Value = sub.Password;
                cmd.Parameters.Add("@GMAIL", SqlDbType.VarChar).Value = sub.Gmail;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "INSERT";
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

        //public string AddSlab(SupClassificationModel sub)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(sub.Slabrange_To))
        //        {
        //            sub.Slabrange_To = "100000000000";
        //        }
        //        con.Open();
        //        cmd = new SqlCommand("pr_iem_mst_tslab", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@slab_name", SqlDbType.VarChar).Value = sub.Slabname;
        //        cmd.Parameters.Add("@slab_insert_by", SqlDbType.Int).Value = 1;
        //        cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "ADDNEWSLAB";
        //        result = (string)cmd.ExecuteScalar();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return result;
        //}
        //public string AddSlabRange(SupClassificationModel sub)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(sub.Slabrange_To))
        //        {
        //            sub.Slabrange_To = "100000000000";
        //        }
        //        if (con.State == ConnectionState.Closed)
        //        {
        //            con.Open();
        //        }
        //        cmd = new SqlCommand("pr_iem_mst_tslab", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@slab_name", SqlDbType.VarChar).Value = sub.Slabname;
        //        cmd.Parameters.Add("@slab_insert_by", SqlDbType.Int).Value = 1;
        //        cmd.Parameters.Add("@rage_name", SqlDbType.VarChar).Value = sub.Slabrange_Name;
        //        cmd.Parameters.Add("@range_from", SqlDbType.VarChar).Value = sub.Slabrange_From;
        //        cmd.Parameters.Add("@range_to", SqlDbType.VarChar).Value = sub.Slabrange_To;
        //        cmd.Parameters.Add("@AddSlabName", SqlDbType.VarChar).Value = sub.newslabadd;
        //        cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "ADDSLABRANGE";

        //        result = (string)cmd.ExecuteScalar();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return result;
        //}
        public IEnumerable<SupClassificationModel> selectslabrange(string slabname, int id)
        {
            List<SupClassificationModel> obj = new List<SupClassificationModel>();
            try
            {

                SupClassificationModel obj1;
                cmd = new SqlCommand("pr_iem_mst_tslab", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@slab_gid", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@slab_name", SqlDbType.VarChar).Value = slabname;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTSLAB";
                da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 0;
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj1 = new SupClassificationModel();
                    obj1.Slabid = Convert.ToInt32(row["slab_gid"].ToString());
                    obj1.Slabrange_gid = Convert.ToInt32(row["slabrange_gid"].ToString());
                    obj1.Slabname = Convert.ToString(row["slab_name"].ToString());
                    obj1.Slabrange_Name = Convert.ToString(row["slabrange_name"].ToString());
                    obj1.Slabrange_From = Convert.ToString(row["slabrange_range_from"].ToString());
                    obj1.Slabrange_To = Convert.ToString(row["slabrange_range_to"].ToString());
                    obj.Add(obj1);

                }
                return obj;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }
        }

        public IEnumerable<SupClassificationModel> selectslabrangeedit(int id)
        {
            List<SupClassificationModel> obj = new List<SupClassificationModel>();
            try
            {

                SupClassificationModel obj1;
                cmd = new SqlCommand("pr_iem_mst_tslab", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@slab_gid", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTSLAB";
                da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = 0;
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    obj1 = new SupClassificationModel();
                    obj1.Slabid = Convert.ToInt32(row["slab_gid"].ToString());
                    obj1.Slabrange_gid = Convert.ToInt32(row["slabrange_gid"].ToString());
                    obj1.Slabname = Convert.ToString(row["slab_name"].ToString());
                    obj1.Slabrange_Name = Convert.ToString(row["slabrange_name"].ToString());
                    obj1.Slabrange_From = Convert.ToString(row["slabrange_range_from"].ToString());
                    obj1.Slabrange_To = Convert.ToString(row["slabrange_range_to"].ToString());
                    obj.Add(obj1);

            }
            return obj;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }
        }

        public IEnumerable<SupClassificationModel> SelectSlabname()
        {
            List<SupClassificationModel> obj = new List<SupClassificationModel>();
            try { 
            
            SupClassificationModel obj1;
            cmd = new SqlCommand("pr_iem_mst_tslab", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTSLABNAME";
            da = new SqlDataAdapter(cmd);
            da.SelectCommand.CommandTimeout = 0;
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                obj1 = new SupClassificationModel();
                obj1.Slabid = Convert.ToInt32(row["slab_gid"].ToString());
                obj1.Slabname = Convert.ToString(row["slab_name"].ToString());
                obj.Add(obj1);
            }
            return obj;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }
        }
        //public string UpdateSlab(SupClassificationModel sub)
        //{
        //    try
        //    {
        //        cmd = new SqlCommand("pr_iem_mst_tslab", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@slab_gid", SqlDbType.VarChar).Value = sub.Slabid;
        //        cmd.Parameters.Add("@slabrange_gid", SqlDbType.VarChar).Value = sub.Slabrange_gid;
        //        cmd.Parameters.Add("@slab_name", SqlDbType.VarChar).Value = sub.Slabname;
        //        cmd.Parameters.Add("@slab_update_by", SqlDbType.VarChar).Value = sub.slab_update_by;
        //        cmd.Parameters.Add("@rage_name", SqlDbType.VarChar).Value = sub.Slabrange_Name;
        //        cmd.Parameters.Add("@range_from", SqlDbType.VarChar).Value = sub.Slabrange_From;
        //        cmd.Parameters.Add("@range_to", SqlDbType.VarChar).Value = sub.Slabrange_To;
        //        cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "UPDATESLAB";
        //        result = (string)cmd.ExecuteScalar();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return result;
        //}
        public IEnumerable<SupClassificationModel> SearchSlabname()
        {
            List<SupClassificationModel> obj = new List<SupClassificationModel>();
            try { 
          

            cmd = new SqlCommand("pr_iem_mst_tslab", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@slab_name", SqlDbType.VarChar).Value = sub.Slabname;
            cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SEARCHSLABNAME";
            da = new SqlDataAdapter(cmd);
            da.SelectCommand.CommandTimeout = 0;
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                SupClassificationModel obj1 = new SupClassificationModel();
                obj1.Slabid = Convert.ToInt32(row["slab_gid"].ToString());
                obj1.Slabname = Convert.ToString(row["slab_name"].ToString());
                obj.Add(obj1);
            }
            con.Close();
            return obj;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }
        }
        public string DeleteSlab_all(int sub)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tslab", con);
                cmd.Parameters.AddWithValue("@slabrange_gid", SqlDbType.VarChar).Value = sub;
                cmd.Parameters.AddWithValue("@ACTION", SqlDbType.VarChar).Value = "duplicate_delmat_slab";
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes = new string[,]
	                {
                         {"slab_isremoved", "Y"}
	            
                         };
                    string[,] whrcol = new string[,]
	                    {
                          {"slab_gid", Convert.ToString(sub)}
                        };
                    string tblname = "iem_mst_tslab";


                    string deletetbl = delecomm.DeleteCommon(codes, whrcol, tblname);
                    // result = "success";
                    CommonIUD delecomm1 = new CommonIUD();
                    string[,] codes1 = new string[,]
                    {
                         {"slabrange_isremoved", "Y"}
	            
                         };
                    string[,] whrcol1 = new string[,]
                        {
                          {"slabrange_slab_gid", Convert.ToString(sub)}
                        };
                    string tblname1 = "iem_mst_tslabrange";


                    string deletetbl1 = delecomm1.DeleteCommon(codes1, whrcol1, tblname1);
                    result = "success";
                }
                else
                {
                    result = "This Slab Based On Some Other Delmat Matrix";
                }
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
        public string DeleteSlab_rangeedit(int sub)
        {
            try
            {
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tslab", con);
                cmd.Parameters.AddWithValue("@slabrange_gid", SqlDbType.VarChar).Value = sub;
                cmd.Parameters.AddWithValue("@ACTION", SqlDbType.VarChar).Value = "duplicate_delmat_slabrange";
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    CommonIUD delecomm1 = new CommonIUD();
                    string[,] codes1 = new string[,]
                    {
                         {"slabrange_isremoved", "Y"}
	            
                         };
                    string[,] whrcol1 = new string[,]
                        {
                          {"slabrange_gid", Convert.ToString(sub)}
                        };
                    string tblname1 = "iem_mst_tslabrange";


                    string deletetbl1 = delecomm1.DeleteCommon(codes1, whrcol1, tblname1);
                    result = "success";
                }
                else
                {
                    result = "This Slab Range Based On Some Other Delmat Matrix";
                }
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
        //public string DeleteSlab(SupClassificationModel sub)
        //{
        //    try
        //    {
        //        if (con.State == ConnectionState.Closed)
        //        {
        //            con.Open();
        //        }
        //        cmd = new SqlCommand("pr_iem_mst_tslab", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@slab_name", SqlDbType.VarChar).Value = sub.Slabname;
        //        cmd.Parameters.Add("@AddSlabName", SqlDbType.VarChar).Value = sub.newslabadd;
        //        cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "DELETESLABRANGE";
        //        result = (string)cmd.ExecuteScalar();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return result;
        //}
        //public string DeleteSlabIndex(int id)
        //{
        //    try
        //    {
        //        cmd = new SqlCommand("pr_iem_mst_tslab", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@slab_gid", SqlDbType.VarChar).Value = id;
        //        cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "DELETESLABINDEX";
        //        cmd.ExecuteNonQuery();
        //        result = (string)cmd.ExecuteScalar();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return result;
        //}


        public string IfExistsCheck(SupClassificationModel sub)
        {
            try
            {

                cmd = new SqlCommand("pr_iem_mst_tslab", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@slab_gid", SqlDbType.VarChar).Value = sub.Slabid;
                cmd.Parameters.Add("@slabrange_gid", SqlDbType.VarChar).Value = sub.Slabrange_gid;
                cmd.Parameters.Add("@slab_name", SqlDbType.VarChar).Value = sub.Slabname;
                cmd.Parameters.Add("@slab_update_by", SqlDbType.VarChar).Value = sub.slab_update_by;
                cmd.Parameters.Add("@rage_name", SqlDbType.VarChar).Value = sub.Slabrange_Name;
                cmd.Parameters.Add("@range_from", SqlDbType.VarChar).Value = sub.Slabrange_From;
                cmd.Parameters.Add("@range_to", SqlDbType.VarChar).Value = sub.Slabrange_To;
                cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "IFEXISTSCHECK";
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
        public DataTable GetEmployeeGender(string empid)
        {
            DataTable dt = new DataTable();
            int result = 0;
            try
            {
                GetConnection();
                cmd = new SqlCommand("pr_iem_mst_tslab", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@emp_gid", empid);
                cmd.Parameters.AddWithValue("@action", "GetEmployeeGender");             
               // da = new SqlDataAdapter(cmd);
               //  cmd = new SqlCommand("select employee_gid,employee_name,employee_code,employee_dept_name,employee_gender from iem_mst_temployee where employee_code='" + empid + "'", con);
                SqlDataAdapter sa = new SqlDataAdapter(cmd);
                sa.Fill(dt);
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return dt;
            }
            // result =(int) cmd.ExecuteScalar();
            //  result = Convert.ToInt32(dt.Rows[0]["employee_gid"].ToString());
            return dt;
        }
        public string SaveSlab(SupClassificationModel sub)
        {
            try
            {
                if (string.IsNullOrEmpty(sub.Slabrange_To))
                {
                    sub.Slabrange_To = "100000000000";
                }
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                DataTable getserilno = new DataTable();
                SqlCommand cmd1 = new SqlCommand();
                cmd1 = new SqlCommand("pr_iem_mst_tslab", con);
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.Parameters.AddWithValue("@slab_name", sub.Slabname);
                cmd1.Parameters.AddWithValue("@action", "save_slab_name");  
                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                da.Fill(getserilno);
                if (getserilno.Rows.Count == 0)
                {
                    cmd = new SqlCommand("pr_iem_mst_tslab", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@slab_gid", SqlDbType.VarChar).Value = sub.Slabid;
                    cmd.Parameters.Add("@slab_name", SqlDbType.VarChar).Value = sub.Slabname;
                    cmd.Parameters.Add("@rage_name", SqlDbType.VarChar).Value = sub.Slabrange_Name;
                    cmd.Parameters.Add("@range_from", SqlDbType.VarChar).Value = sub.Slabrange_From;
                    cmd.Parameters.Add("@range_to", SqlDbType.VarChar).Value = sub.Slabrange_To;
                    cmd.Parameters.Add("@slab_insert_by", SqlDbType.Int).Value = cmn.GetLoginUserGid();
                    cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "ADDNEWSLAB";
                    result = cmd.ExecuteNonQuery().ToString();
                }
                else
                {
                    result = "Duplicate Slab Name !!";
                }
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


        public string SaveSlabRanges(SupClassificationModel sub)
        {
            try
            {
                if (string.IsNullOrEmpty(sub.Slabrange_To))
                {
                    sub.Slabrange_To = "100000000000";
                }
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                //cmd = new SqlCommand("pr_iem_mst_tslab", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.Add("@slabrange_gid", SqlDbType.VarChar).Value = sub.Slabrange_gid;
                //cmd.Parameters.Add("@slab_name", SqlDbType.VarChar).Value = sub.Slabname;
                //cmd.Parameters.Add("@rage_name", SqlDbType.VarChar).Value = sub.Slabrange_Name;
                //cmd.Parameters.Add("@range_from", SqlDbType.VarChar).Value = sub.Slabrange_From;
                //cmd.Parameters.Add("@range_to", SqlDbType.VarChar).Value = sub.Slabrange_To;
                //cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "ADDSLABRANGE";
                //result = cmd.ExecuteNonQuery().ToString();
                CommonIUD comm = new CommonIUD();

                if (sub.Slabrange_gid == 0)
                {
                    DataTable getserilno = new DataTable();
                    SqlCommand cmd1 = new SqlCommand();
                    cmd1 = new SqlCommand("pr_iem_mst_tslab", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.AddWithValue("@slab_name", sub.Slabname);
                    cmd1.Parameters.AddWithValue("@action", "save_slab_gid");  
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(getserilno);
                    if (getserilno.Rows.Count > 0)
                    {
                        sub.Slabrange_gid = Convert.ToInt32(getserilno.Rows[0]["slab_gid"].ToString());
                    }
                }
                SqlCommand cmd = new SqlCommand("pr_iem_mst_tslab", con);
                cmd.Parameters.AddWithValue("@range_to", SqlDbType.VarChar).Value = sub.Slabrange_To;
                cmd.Parameters.AddWithValue("@ACTION", SqlDbType.VarChar).Value = "ADDSLABRANGE";
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                if (res1 == "Not There")
                {
                    DataTable getserilnoedit = new DataTable();
                    SqlCommand cmd1edit = new SqlCommand();
                    cmd1edit = new SqlCommand("pr_iem_mst_tslab", con);
                    cmd1edit.CommandType = CommandType.StoredProcedure;
                    cmd1edit.Parameters.AddWithValue("@slab_gid", sub.Slabid);
                    cmd1edit.Parameters.AddWithValue("@action", "save_slab_range");  
                    SqlDataAdapter daedit = new SqlDataAdapter(cmd1edit);
                    daedit.Fill(getserilnoedit);
                    if (getserilnoedit.Rows.Count > 0)
                    {
                        SqlCommand cmdedit = new SqlCommand("pr_iem_mst_tslab", con);
                        cmdedit.Parameters.AddWithValue("@slabrange_gid", SqlDbType.VarChar).Value = sub.Slabid;
                        cmdedit.Parameters.AddWithValue("@ACTION", SqlDbType.VarChar).Value = "duplicate_delmat_slabrange";
                        cmdedit.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                        cmdedit.CommandType = CommandType.StoredProcedure;
                        cmdedit.ExecuteNonQuery();
                        string res1edit = cmdedit.Parameters["@res"].Value.ToString();
                        if (res1edit == "Not There")
                        {
                            decimal compareFrom = 0;
                            decimal compareTo = 0;
                            DataTable getslaprange = new DataTable();
                            SqlCommand cmd1e = new SqlCommand();
                            cmd1e = new SqlCommand("pr_iem_mst_tslab", con);
                            cmd1e.CommandType = CommandType.StoredProcedure;
                            cmd1e.Parameters.AddWithValue("@slab_gid", sub.Slabrange_gid);
                            cmd1e.Parameters.AddWithValue("@slabrange_gid", sub.Slabid);
                            cmd1e.Parameters.AddWithValue("@action", "Check_slap_range");
                            SqlDataAdapter da = new SqlDataAdapter(cmd1e);
                            da.Fill(getslaprange);

                            if (getslaprange.Rows.Count > 0)
                            {
                                compareTo = Convert.ToDecimal(getslaprange.Rows[0]["compare_withTOFrom"]);
                                compareFrom = Convert.ToDecimal(getslaprange.Rows[1]["compare_withTOFrom"]);

                                compareTo = compareTo - Convert.ToDecimal(0.01);
                                compareFrom = compareFrom + Convert.ToDecimal(0.01);

                            }

                            if ((Convert.ToDecimal(sub.Slabrange_To) == compareTo || compareTo == Convert.ToDecimal(-0.01)) && (Convert.ToDecimal(sub.Slabrange_From) == compareFrom || compareFrom == Convert.ToDecimal(0.01)))
                            {


                                if (Convert.ToDecimal(sub.Slabrange_From) < Convert.ToDecimal(sub.Slabrange_To))
                                {
                                    CommonIUD editcomm = new CommonIUD();
                                    string[,] codes = new string[,]
                 
                   {
                          {"slabrange_slab_gid", Convert.ToString(sub.Slabrange_gid)},
                        {"slabrange_name",sub.Slabrange_Name},                      
                        {"slabrange_range_from", sub.Slabrange_From},                        
                        {"slabrange_range_to",sub.Slabrange_To},
                        {"slabrange_isremoved", "N"}                  
                  };
                                string[,] whrcol = new string[,]
	                 {
                          {"slabrange_gid", Convert.ToString( sub.Slabid)}
            
                     };
                                string tblname = "iem_mst_tslabrange";

                                    string updcomm = editcomm.UpdateCommon(codes, whrcol, tblname);
                                    result = "sub";
                                }
                                else
                                {
                                    result = "Can't Insert Less Than Slab To Range";
                                }
                            }
                            else
                            {
                                result = "Please Enter Correct Flow Range";
                            }
                        }
                        else
                        {
                            result = "This Slab Range Already used in Delmat Matrix";
                        }

                    }
                    else
                    {
                        string value = "";
                        decimal torange = 0;
                        SqlCommand cmdedit = new SqlCommand("pr_iem_mst_tslab", con);
                        cmdedit.Parameters.AddWithValue("@slabrange_gid", SqlDbType.VarChar).Value = sub.Slabrange_gid;
                        cmdedit.Parameters.AddWithValue("@ACTION", SqlDbType.VarChar).Value = "duplicate_delmat_slab";
                        cmdedit.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                        cmdedit.CommandType = CommandType.StoredProcedure;
                        cmdedit.ExecuteNonQuery();
                        string res1edit = cmdedit.Parameters["@res"].Value.ToString();
                        if (res1edit == "Not There")
                        {
                            DataTable getserilno1 = new DataTable();
                            SqlCommand cmd11 = new SqlCommand();
                            cmd11 = new SqlCommand("pr_iem_mst_tslab", con);
                            cmd11.CommandType = CommandType.StoredProcedure;
                            cmd11.Parameters.AddWithValue("@slabrange_gid", sub.Slabrange_gid);
                            cmd11.Parameters.AddWithValue("@action", "save_slab_Max_range");  
                            SqlDataAdapter da = new SqlDataAdapter(cmd11);
                            da.Fill(getserilno1);
                            if (getserilno1.Rows.Count > 0)
                            {
                                value = Convert.ToString((getserilno1.Rows[0]["slabrange_range_to"]));
                                if (value != "")
                                {
                                    torange = Convert.ToDecimal(getserilno1.Rows[0]["slabrange_range_to"].ToString());
                                    torange = torange + Convert.ToDecimal(0.01);
                                    torange = System.Math.Round(torange, 2);
                                }
                                else
                                {
                                    torange = Convert.ToDecimal(sub.Slabrange_From);
                                }
                            }
                            if (torange == Convert.ToDecimal(sub.Slabrange_From))
                            {
                                if (Convert.ToDecimal(sub.Slabrange_From) < Convert.ToDecimal(sub.Slabrange_To))
                                {
                                    string[,] codes = new string[,]
                   {
                        {"slabrange_slab_gid", Convert.ToString(sub.Slabrange_gid)},
                        {"slabrange_name",sub.Slabrange_Name},                      
                        {"slabrange_range_from", sub.Slabrange_From},                        
                        {"slabrange_range_to",sub.Slabrange_To},
                        {"slabrange_isremoved", "N"}
                    
                  };
                                    string tname = "iem_mst_tslabrange";
                                    string insertcommend = comm.InsertCommon(codes, tname);
                                    result = "sub";
                                }
                                else
                                {
                                    result = "Can't Insert Less Than Slab To Range";
                                }
                            }
                            else
                            {
                                result = "Please Enter Correct Flow Range";
                            }
                        }
                        else
                        {
                            result = "This Slab  Based On Some Other Delmat Matrix";
                        }
                    }

                }
                else
                {
                    result = "Duplicate record !";
                }
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
        public IEnumerable<SupClassificationModel> GridLoad()
        {
            List<SupClassificationModel> obj = new List<SupClassificationModel>();
            try { 
            cmd = new SqlCommand("pr_iem_mst_tslab", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@slab_gid", SqlDbType.VarChar).Value = sub.Slabid;
            cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTGRIDLOAD";
            da = new SqlDataAdapter(cmd);
            da.SelectCommand.CommandTimeout = 0;
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                SupClassificationModel obj1 = new SupClassificationModel();
                obj1.Slabrange_Slabgid = Convert.ToInt32(row["slabrange_gid"].ToString());
                obj1.Slabname = Convert.ToString(row["slab_name"].ToString());
                obj1.Slabrange_Name = Convert.ToString(row["slabrange_name"].ToString());
                obj1.Slabrange_From = Convert.ToString(row["slabrange_range_from"].ToString());
                obj1.Slabrange_To = Convert.ToString(row["slabrange_range_to"].ToString());
                obj.Add(obj1);
            }
            con.Close();
            return obj;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }
        }
        public IEnumerable<SupClassificationModel> GridLoadEdit(int id)
        {
            List<SupClassificationModel> obj = new List<SupClassificationModel>();
            try { 
            cmd = new SqlCommand("pr_iem_mst_tslab", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@slab_gid", SqlDbType.VarChar).Value = id;
            cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "SELECTGRIDLOAD";
            da = new SqlDataAdapter(cmd);
            da.SelectCommand.CommandTimeout = 0;
            dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow row in dt.Rows)
            {
                SupClassificationModel obj1 = new SupClassificationModel();
                obj1.Slabid = Convert.ToInt32(row["slab_gid"].ToString());
                obj1.Slabrange_Slabgid = Convert.ToInt32(row["slabrange_gid"].ToString());
                obj1.Slabname = Convert.ToString(row["slab_name"].ToString());
                obj1.Slabrange_Name = Convert.ToString(row["slabrange_name"].ToString());
                obj1.Slabrange_From = Convert.ToString(row["slabrange_range_from"].ToString());
                obj1.Slabrange_To = Convert.ToString(row["slabrange_range_to"].ToString());
                obj.Add(obj1);
            }
            con.Close();
            return obj;
            }
            catch (Exception ex)
            {
                objErrorLog.WriteErrorLog(ex.Message.ToString(), ex.ToString());
                return obj;
            }
        }
        //public string EditSaveSlab(SupClassificationModel sub)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(sub.Slabrange_To))
        //        {
        //            sub.Slabrange_To = "100000000000";
        //        }
        //        if (con.State == ConnectionState.Closed)
        //        {
        //            con.Open();
        //        }
        //        cmd = new SqlCommand("pr_iem_mst_tslab", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@slab_gid", SqlDbType.VarChar).Value = sub.Slabid;
        //        cmd.Parameters.Add("@slab_name", SqlDbType.VarChar).Value = sub.Slabname;
        //        cmd.Parameters.Add("@rage_name", SqlDbType.VarChar).Value = sub.Slabrange_Name;
        //        cmd.Parameters.Add("@range_from", SqlDbType.VarChar).Value = sub.Slabrange_From;
        //        cmd.Parameters.Add("@range_to", SqlDbType.VarChar).Value = sub.Slabrange_To;
        //        cmd.Parameters.Add("@slab_insert_by", SqlDbType.Int).Value = cmn.GetLoginUserGid();
        //        cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "ADDNEWSLAB";
        //        result = cmd.ExecuteNonQuery().ToString();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return result;
        //}

        //public string EditSaveSlabRanges(SupClassificationModel sub)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(sub.Slabrange_To))
        //        {
        //            sub.Slabrange_To = "100000000000";
        //        }
        //        if (con.State == ConnectionState.Closed)
        //        {
        //            con.Open();
        //        }
        //        cmd = new SqlCommand("pr_iem_mst_tslab", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@slab_gid", SqlDbType.VarChar).Value = sub.Slabid;
        //        cmd.Parameters.Add("@slab_name", SqlDbType.VarChar).Value = sub.Slabname;
        //        cmd.Parameters.Add("@rage_name", SqlDbType.VarChar).Value = sub.Slabrange_Name;
        //        cmd.Parameters.Add("@range_from", SqlDbType.VarChar).Value = sub.Slabrange_From;
        //        cmd.Parameters.Add("@range_to", SqlDbType.VarChar).Value = sub.Slabrange_To;
        //        cmd.Parameters.Add("@ACTION", SqlDbType.VarChar).Value = "ADDSLABRANGE";
        //        result = cmd.ExecuteNonQuery().ToString();

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        con.Close();
        //    }
        //    return result;
        //}


        public string AddSlab(SupClassificationModel sub)
        {
            throw new NotImplementedException();
        }

        public string AddSlabRange(SupClassificationModel sub)
        {
            throw new NotImplementedException();
        }

        public string UpdateSlab(SupClassificationModel sub)
        {
            throw new NotImplementedException();
        }

        public string DeleteSlab(SupClassificationModel sub)
        {
            throw new NotImplementedException();
        }

        public string DeleteSlabIndex(int id)
        {
            throw new NotImplementedException();
        }

        public string EditSaveSlab(SupClassificationModel sub)
        {
            throw new NotImplementedException();
        }

        public string EditSaveSlabRanges(SupClassificationModel sub)
        {
            throw new NotImplementedException();
        }
    }
}