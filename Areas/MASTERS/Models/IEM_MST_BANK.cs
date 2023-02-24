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
    public class IEM_MST_BANK:Iiem_mst_bank
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
        
        public IEnumerable<iem_mst_bank> getbank()
        {
            try
            {
                List<iem_mst_bank> objOrgType = new List<iem_mst_bank>();
                iem_mst_bank objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_bank", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_bank();
                    objModel.bank_gid = Convert.ToInt32(dt.Rows[i]["bank_gid"].ToString());
                    objModel.bank_code = Convert.ToString(dt.Rows[i]["bank_code"].ToString());
                    objModel.bank_name = Convert.ToString(dt.Rows[i]["bank_name"].ToString());
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

            
          //  throw new NotImplementedException();
        }

        public IEnumerable<iem_mst_bank> getbank(string code,string name)
        {
            try
            {
                List<iem_mst_bank> objOrgType = new List<iem_mst_bank>();
                iem_mst_bank objModel;

                GetCon();
                DataTable dt = new DataTable();

                SqlCommand cmd = new SqlCommand("pr_iem_mst_bank", con);
                cmd.Parameters.AddWithValue("@action", "select");
                cmd.Parameters.AddWithValue("@bank_code", code);
                cmd.Parameters.AddWithValue("@bank_name", name);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objModel = new iem_mst_bank();
                    objModel.bank_gid = Convert.ToInt32(dt.Rows[i]["bank_gid"].ToString());
                    objModel.bank_code = Convert.ToString(dt.Rows[i]["bank_code"].ToString());
                    objModel.bank_name = Convert.ToString(dt.Rows[i]["bank_name"].ToString());
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


            //  throw new NotImplementedException();
        }

        public iem_mst_bank GetBankById(int ClassificationId)
        {
            try
            {
                List<iem_mst_bank> objOrgType = new List<iem_mst_bank>();
                iem_mst_bank objModel;
                GetCon();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_bank", con);
                cmd.Parameters.AddWithValue("@action", "selectById");
                cmd.Parameters.AddWithValue("@gid", ClassificationId);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);    
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new iem_mst_bank()
                {
                    bank_gid = Convert.ToInt32(dt.Rows[0]["bank_gid"].ToString()),
                    bank_name = Convert.ToString(dt.Rows[0]["bank_name"].ToString()),
                    bank_code = Convert.ToString(dt.Rows[0]["bank_code"].ToString()),
                };
                return model;

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

        public string InsertBank(iem_mst_bank Classifications)
        {

            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_bank", con);
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.AddWithValue("@bank_code", Classifications.bank_code);
                cmd.Parameters.AddWithValue("@bank_name", Classifications.bank_name);               
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();
                string result = cmd.Parameters["@res"].Value.ToString();

                if (result == "Not There")
                {
                    CommonIUD comm = new CommonIUD();
                    string[,] codes = new string[,]            
	                {
                        {"bank_code",Classifications.bank_code },
                        {"bank_name",Classifications.bank_name},
                        {"bank_insert_by",Convert.ToString (Classifications.bank_insert_by)},
                        {"bank_insert_date",comm.GetCurrentDate()}
                    };
                    string tname = "iem_mst_tbank";
                    string insertcommend = comm.InsertCommon(codes, tname);
                }
                else
                {
                    return "Duplicate record !";
                }
            }
            catch(Exception ex)
            {
                return ex.Message.ToString();
            }
            finally
            {
                con.Close();
            }

            return "success";
        }

      

        public string UpdateBank(iem_mst_bank Classifications)
        {

            try
            {
                GetCon();
                SqlCommand cmd = new SqlCommand("pr_iem_mst_bank", con);
                cmd.Parameters.AddWithValue("@action", "Exist");
                cmd.Parameters.AddWithValue("@gid", Classifications.bank_gid);
                cmd.Parameters.AddWithValue("@bank_code", Classifications.bank_code);
                cmd.Parameters.AddWithValue("@bank_name", Classifications.bank_name);
                cmd.Parameters.Add("@res", SqlDbType.VarChar, 45).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.ExecuteNonQuery();

                string result = cmd.Parameters["@res"].Value.ToString();

                if (result == "Not There")
                {
                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes = new string[,]
	                  {
                          {"bank_code", Classifications.bank_code},
                          {"bank_name", Classifications.bank_name},
                          {"bank_update_by", Classifications.bank_update_by.ToString ()},
                          {"bank_update_date", delecomm.GetCurrentDate()}
	                  };
                    string[,] whrcol = new string[,]
	                 {
                          {"bank_gid", Classifications.bank_gid.ToString ()},
                          {"bank_isremoved", "N"}
                     };

                    string updcomm = delecomm.UpdateCommon(codes, whrcol, "iem_mst_tbank");
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


        public string DeleteBank(int ClassificationId)
        {
            CommonIUD delecomm = new CommonIUD();
            string col_value = string.Empty;
            string col_temp = string.Empty;
            try
            {

                string[,] codes = new string[,]
	       {
                 {"bank_isremoved", "Y"}
	            
           };
                string[,] whrcol = new string[,]
	        {
                {"bank_gid", ClassificationId.ToString ()}
            };
                string tblname = "iem_mst_tbank";


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
            // throw new NotImplementedException();
        }
    }

    }
     

