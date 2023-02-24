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
    public class IEM_MST_UOM : Iiem_mst_uom
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();

        private void GetCon()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }

        public IEnumerable<iem_mst_uom> getuom()
        {

            try
            {
                List<iem_mst_uom> objOrgType = new List<iem_mst_uom>();
                iem_mst_uom objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_uom", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_uom();
                    objModel.uom_gid= Convert.ToInt32(dt.Rows[i]["uom_gid"].ToString());
                    objModel.uom_code = Convert.ToString(dt.Rows[i]["uom_code"].ToString());
                    objModel.uom_name= Convert.ToString(dt.Rows[i]["uom_name"].ToString());
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
            //throw new NotImplementedException();
        }

        public IEnumerable<iem_mst_uom> getuom(string filter_code,string filter_name)
        {

            try
            {
                List<iem_mst_uom> objOrgType = new List<iem_mst_uom>();
                iem_mst_uom objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_uom", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.Parameters.AddWithValue("@uom_code", filter_code);
                cmd.Parameters.AddWithValue("@uom_name", filter_name);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_uom();
                    objModel.uom_gid = Convert.ToInt32(dt.Rows[i]["uom_gid"].ToString());
                    objModel.uom_code = Convert.ToString(dt.Rows[i]["uom_code"].ToString());
                    objModel.uom_name = Convert.ToString(dt.Rows[i]["uom_name"].ToString());
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
            //throw new NotImplementedException();
        }

        public iem_mst_uom GetUomById(int UomId)
        {

            try
            {
                List<iem_mst_uom> objOrgType = new List<iem_mst_uom>();
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_uom", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", UomId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new iem_mst_uom()
                {
                    uom_gid= Convert.ToInt32(dt.Rows[0]["uom_gid"].ToString()),
                    uom_name = Convert.ToString(dt.Rows[0]["uom_name"].ToString()),
                    uom_code = Convert.ToString(dt.Rows[0]["uom_code"].ToString()),
                };

                return model;
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

        public string InsertUom(iem_mst_uom Uom)
        {
            string result=string.Empty;

            try
            {
                SqlCommand cmd = new SqlCommand("pr_iem_mst_uom", con);
                GetCon();
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.AddWithValue("@uom_code", Uom.uom_code);
                cmd.Parameters.AddWithValue("@uom_name", Uom.uom_name);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                CommonIUD comm = new CommonIUD();
                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]            
	                {
                        {"uom_code",Uom.uom_code },
                        {"uom_name",Uom.uom_name},
                        {"uom_insert_by",Convert.ToString (Uom.uom_insert_by)},
                        {"uom_insert_date",comm.GetCurrentDate()}	  
                    };

                  
                    string insertcommend = comm.InsertCommon(codes, "iem_mst_tuom");
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

       

        public string UpdateUom(iem_mst_uom Uom)
        {
            string result = string.Empty;
            try
            {
                SqlCommand cmd = new SqlCommand("pr_iem_mst_uom", con);
                GetCon();
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.AddWithValue("@gid", Uom.uom_gid);
                cmd.Parameters.AddWithValue("@uom_code", Uom.uom_code);
                cmd.Parameters.AddWithValue("@uom_name", Uom.uom_name);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string res1 = cmd.Parameters["@res"].Value.ToString();
                CommonIUD delecomm = new CommonIUD();
                if (res1 == "Not There")
                {
                    string[,] codes = new string[,]
	                {
                        {"uom_code", Uom.uom_code},
                        {"uom_name", Uom.uom_name},
                        {"uom_update_by", Uom.uom_update_by.ToString ()},
                        {"uom_update_date", delecomm.GetCurrentDate()}
	                };
                    string[,] whrcol = new string[,]
	                {
                        {"uom_gid", Uom.uom_gid.ToString ()},
                        {"uom_isremoved", "N"}
                    };

                   
                    string updcomm = delecomm.UpdateCommon(codes, whrcol, "iem_mst_tuom");
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


        public string DeleteUom(int UomId)
        {
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;

            try
            {
                string[,] codes = new string[,]
	            {
                    {"uom_isremoved", "Y"}
                };

                string[,] whrcol = new string[,]
	            {
                    {"uom_gid", UomId.ToString ()},
                    {"uom_isremoved", "N"}
                };

                string deletetbl = delecomm.DeleteCommon(codes, whrcol, "iem_mst_tuom");
                return deletetbl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            // throw new NotImplementedException();
        }
    }
}