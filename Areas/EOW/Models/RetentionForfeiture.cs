using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using IEM.Common;
using System.Configuration;

namespace IEM.Areas.EOW.Models
{
    public class RetentionForfeiture : RetentionForfeitureRepository
    {
        SqlConnection con = new SqlConnection();
        SqlCommand cmd = new SqlCommand();
        SqlDataAdapter da = new SqlDataAdapter();
        CmnFunctions cmnfun = new CmnFunctions();

        CmnFunctions objCmnFunctions = new CmnFunctions();
        CommonIUD objCommonIUD = new CommonIUD();

        private void GetConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionstring"].ToString();
                con.Open();
            }
        }

        public IEnumerable<Eow_RetentionForfeiture> RetentionReleaseGrid()
        {
            try
            {
                List<Eow_RetentionForfeiture> GridReceipt = new List<Eow_RetentionForfeiture>();
                Eow_RetentionForfeiture objModel;
                GetConnection();
                DataTable dt = new DataTable();

                SqlCommand cmd = new SqlCommand("pr_iem_trn_RetentionRelease", con);
                cmd.Parameters.AddWithValue("@ACTION", "ExtendSelect");
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new Eow_RetentionForfeiture()
                    {
                        invoice_gid = Convert.ToInt32(row["invoice_gid"].ToString()),
                        invoice_ecf_gid = Convert.ToInt32(row["invoice_ecf_gid"].ToString()),
                        ecf_date = row["ecf_date"].ToString(),
                        ecf_no = row["ecf_no"].ToString(),
                        ecf_suppliercode = row["supplierheader_suppliercode"].ToString(),
                        ecf_suppliername = row["supplierheader_name"].ToString(),
                        invoice_retention_amount = Convert.ToDecimal(row["invoice_retention_amount"].ToString()),
                        invoice_retention_exception = Convert.ToDecimal(row["invoice_retention_exception"].ToString()),
                        invoice_no = row["invoice_no"].ToString(),
                        invoice_amount = Convert.ToDecimal(row["invoice_amount"].ToString()),
                        invoice_retention_releaseon = row["invoice_retention_releaseon"].ToString(),
                        // retentionrelease_amount = Convert.ToString(row["retentionrelease_amount"].ToString()),
                    };
                    GridReceipt.Add(objModel);
                }

                return GridReceipt;
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

        public Eow_RetentionForfeiture GetRetentionExtendRecordById(int invoicegid)
        {
            try
            {
                List<Eow_RetentionForfeiture> objOrgType = new List<Eow_RetentionForfeiture>();
                Eow_RetentionForfeiture objModel;
                GetConnection();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand("pr_iem_trn_RetentionRelease", con);
                cmd.Parameters.AddWithValue("@ACTION", "SELECT_ID");
                cmd.Parameters.AddWithValue("@invoice_gid", invoicegid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                var model = new Eow_RetentionForfeiture()
                {
                    invoice_gid = Convert.ToInt32(dt.Rows[0]["invoice_gid"].ToString()),
                    invoice_ecf_gid = Convert.ToInt32(dt.Rows[0]["invoice_ecf_gid"].ToString()),
                    invoice_date = Convert.ToString(dt.Rows[0]["invoice_date"].ToString()),
                    ecf_date = Convert.ToString(dt.Rows[0]["ecf_date"].ToString()),
                    ecf_amount = Convert.ToDecimal(dt.Rows[0]["ecf_amount"].ToString()),
                    ecf_no = Convert.ToString(dt.Rows[0]["ecf_no"].ToString()),
                    supplierheader_suppliercode = Convert.ToString(dt.Rows[0]["supplierheader_suppliercode"].ToString()),
                    supplierheader_name = Convert.ToString(dt.Rows[0]["supplierheader_name"].ToString()),
                    invoice_retention_amount = Convert.ToDecimal(dt.Rows[0]["invoice_retention_amount"].ToString()),
                    invoice_retention_exception = Convert.ToDecimal(dt.Rows[0]["invoice_retention_exception"].ToString()),
                    invoice_no = Convert.ToString(dt.Rows[0]["invoice_no"].ToString()),
                    invoice_amount = Convert.ToDecimal(dt.Rows[0]["invoice_amount"].ToString()),
                    ecf_remark = Convert.ToString(dt.Rows[0]["ecf_remark"].ToString()),
                    invoice_desc = Convert.ToString(dt.Rows[0]["invoice_desc"].ToString()),
                    invoice_retention_releaseon = Convert.ToString(dt.Rows[0]["invoice_retention_releaseon"].ToString()),

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
        }

        public DataTable GetSerialNo()
        {
            DataTable getserilno = new DataTable();
            try
            {
                GetConnection();
                SqlCommand cmd = new SqlCommand("select MAX(retention_serialno)  as 'Serial No' from iem_trn_tretentionlog", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(getserilno);
                return getserilno;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return getserilno;
        }

        public DataTable GetRetentioninformation(int invoicegid)
        {
            DataTable dt = new DataTable();
            try
            {
                GetConnection();
                SqlCommand cmd = new SqlCommand("pr_iem_trn_RetentionRelease", con);
                cmd.Parameters.AddWithValue("@ACTION", "SearchInformation");
                cmd.Parameters.AddWithValue("@invoice_gid", invoicegid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return dt;
        }

        public DataTable CheckData(int invoivegid)
        {
            DataTable checkexist = new DataTable();
            try
            {
                GetConnection();
                SqlCommand cmd = new SqlCommand("pr_iem_trn_RetentionRelease", con);
                cmd.Parameters.AddWithValue("@ACTION", "SelectExist");
                cmd.Parameters.AddWithValue("@invoice_gid", invoivegid);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da1 = new SqlDataAdapter(cmd);

                da1.Fill(checkexist);
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return checkexist;
        }

        public string Insertextenddetails(Eow_RetentionForfeiture Extendinformation)
        {
            string insertcommend = string.Empty;
            string updcomm = string.Empty;
            string retentionstatus = string.Empty;
            string checkretentionstatus = string.Empty;
            CommonIUD comm = new CommonIUD();

            try
            {
                GetConnection();
                SqlCommand insert = new SqlCommand("select * from  iem_trn_tretentionlog where retention_invoice_gid='" + Extendinformation.invoice_gid + "'", con);
                SqlDataAdapter dains = new SqlDataAdapter(insert);
                DataTable ins = new DataTable();
                dains.Fill(ins);
                if (ins.Rows.Count > 0)
                {
                    //updaterecord              
                    Extendinformation.retention_isactive = "N";
                    Extendinformation.invoice_retention_status = int.Parse(ConfigurationManager.AppSettings["Forfeiture"].ToString());
                    CommonIUD delecomm = new CommonIUD();
                    string[,] codes1 = new string[,]
	                  {
                        {"retention_isactive",Extendinformation.retention_isactive.ToString ()},                       
                        
	                  };
                    string[,] whrcol = new string[,]
	                 {
                          {"retention_invoice_gid", Extendinformation.invoice_gid.ToString ()}                       
                     };
                    updcomm = delecomm.UpdateCommon(codes1, whrcol, "iem_trn_tretentionlog");


                    //insert

                    SqlCommand check = new SqlCommand("select retention_status from iem_trn_tretentionlog where retention_invoice_gid='"+Extendinformation.invoice_gid+"' and retention_status='Forfeiture'", con);
                    SqlDataAdapter dacheck = new SqlDataAdapter(check);
                    DataTable dtcheck = new DataTable();
                    dacheck.Fill(dtcheck);
                    if (dtcheck.Rows.Count == 0)
                    {
                        Extendinformation.retention_status = Convert.ToString(ConfigurationManager.AppSettings["6"]);
                        Extendinformation.retention_isactive = "Y";
                        string[,] codes = new string[,]           
	                {
                        
                        {"retention_date",cmnfun.convertoDateTimeString(Extendinformation.invoice_retention_releaseon.ToString ())},
                        {"retention_invoice_gid",Extendinformation.invoice_gid.ToString ()},
                        {"retention_serialno",Extendinformation.retention_serialno.ToString ()},
                        {"retention_rate",Extendinformation.invoice_retention_amount.ToString ()},
                        {"retention_amount",Extendinformation.invoice_retention_amount.ToString ()},
                        {"retention_releaseamount",Extendinformation.retention_releaseamount.ToString ()},
                        {"retention_exception",Extendinformation.retention_exception.ToString ()},
                        {"retention_release_gid",Extendinformation.retention_release_gid.ToString ()},
                        //{"retention_expiry",cmnfun.convertoDateTimeString(Extendinformation.retention_expiry.ToString ())},
                        {"retention_status",Extendinformation.retention_status.ToString ()},
                        {"retention_insertby",cmnfun.GetLoginUserGid().ToString ()},
                        {"retention_inserton",comm.GetCurrentDate().ToString ()},
                        {"retention_isactive",Extendinformation.retention_isactive.ToString ()},
                        {"remarks",string.IsNullOrEmpty(Extendinformation.remarks)?"":Extendinformation.remarks}
                    };
                        string tname = "iem_trn_tretentionlog";
                        insertcommend = comm.InsertCommon(codes, tname);
                        if (insertcommend == "success")
                        {

                            Extendinformation.invoice_retention_status = int.Parse(ConfigurationManager.AppSettings["Forfeiture"].ToString());
                            string[,] codes2 = new string[,]
	                  {
                        {"invoice_retention_status",Extendinformation.invoice_retention_status.ToString ()},
                        {"invoice_retention_curr_status","2"},
                        
	                  };
                            string[,] whrcol1 = new string[,]
	                 {
                          {"invoice_gid", Extendinformation.invoice_gid.ToString ()}                       
                     };

                            updcomm = delecomm.UpdateCommon(codes2, whrcol1, "iem_trn_tinvoice");
                        }

                    }
                    else
                    {
                        Extendinformation.retention_isactive = "Y";
                        Extendinformation.retention_status = Convert.ToString(ConfigurationManager.AppSettings["6"]);
                        //CommonIUD delecomm = new CommonIUD();
                        string[,] codes2 = new string[,]
                                             {
                                                //{"retention_expiry",cmnfun.convertoDateTimeString(Extendinformation.retention_expiry.ToString ())},                      
                                                //{"remarks",Extendinformation.remarks.ToString ()},
                                                {"retention_isactive",Extendinformation.retention_isactive}
                                             };
                        string[,] whrcol2 = new string[,]
                                             {
                                                  {"retention_invoice_gid", Extendinformation.invoice_gid.ToString ()},  
                                                  {"retention_status", Extendinformation.retention_status.ToString ()}
                                             };

                        updcomm = delecomm.UpdateCommon(codes2, whrcol2, "iem_trn_tretentionlog");

                        Extendinformation.invoice_retention_status = int.Parse(ConfigurationManager.AppSettings["Forfeiture"].ToString());
                        string[,] codes3 = new string[,]
	                  {
                        {"invoice_retention_status",Extendinformation.invoice_retention_status.ToString ()},                       
                          {"invoice_retention_curr_status","2"},
	                  };
                        string[,] whrcol3 = new string[,]
	                 {
                          {"invoice_gid", Extendinformation.invoice_gid.ToString ()}                       
                     };

                        updcomm = delecomm.UpdateCommon(codes3, whrcol3, "iem_trn_tinvoice");
                    }
                }

                return updcomm;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            return "Doesn't Inserted";
        }


        public IEnumerable<Eow_RetentionForfeiture> RetentionReleaseGrid(string releasedate, string ecfdate, string ecfno, string invoiceno, string suppcode, string suppname, string extenddate)
        {

            try
            {
                List<Eow_RetentionForfeiture> GridReceipt = new List<Eow_RetentionForfeiture>();
                Eow_RetentionForfeiture objModel;
                GetConnection();
                DataTable dt = new DataTable();

                SqlCommand cmd = new SqlCommand("pr_iem_trn_RetentionRelease", con);
                cmd.Parameters.AddWithValue("@ACTION", "RetentionExtendReport");
                cmd.Parameters.AddWithValue("@ecfdate", ecfdate);
                cmd.Parameters.AddWithValue("@retention_expiry", extenddate);
                cmd.Parameters.AddWithValue("@ecf_no", ecfno);
                cmd.Parameters.AddWithValue("@invoice_no", invoiceno);
                cmd.Parameters.AddWithValue("@supplierheader_suppliercode", suppcode);
                cmd.Parameters.AddWithValue("@supplierheader_name", suppname);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                foreach (DataRow row in dt.Rows)
                {
                    objModel = new Eow_RetentionForfeiture()
                    {
                        invoice_gid = Convert.ToInt32(row["invoice_gid"].ToString()),
                        invoice_ecf_gid = Convert.ToInt32(row["invoice_ecf_gid"].ToString()),
                        ecf_date = row["ecf_date"].ToString(),
                        ecf_no = row["ecf_no"].ToString(),
                        ecf_suppliercode = row["supplierheader_suppliercode"].ToString(),
                        ecf_suppliername = row["supplierheader_name"].ToString(),
                        invoice_retention_amount = Convert.ToDecimal(row["invoice_retention_amount"].ToString()),
                        invoice_retention_exception = Convert.ToDecimal(row["invoice_retention_exception"].ToString()),
                        invoice_no = row["invoice_no"].ToString(),
                        invoice_amount = Convert.ToDecimal(row["invoice_amount"].ToString()),
                        invoice_retention_releaseon = row["invoice_retention_releaseon"].ToString(),
                        //release_amount = Convert.ToDecimal(row["retentionrelease_amount"].ToString()),
                        // retention_expiry = row["retention_expiry"].ToString(),
                    };
                    GridReceipt.Add(objModel);
                }

                return GridReceipt;
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